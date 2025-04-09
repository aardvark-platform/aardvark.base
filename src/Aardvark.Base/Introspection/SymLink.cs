using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Aardvark.Base
{
    public partial class Aardvark
    {
        #region LdConfig

        private static class LdConfig
        {
            static readonly Regex rx = new(@"[ \t]*(?<name>[^ \t]+)[ \t]+\((?<libc>[^,]+)\,(?<arch>[^,\)]+)[^\)]*\)[ \t]*\=\>[ \t]*(?<path>.*)", RegexOptions.Compiled);

            private static readonly Lazy<Dictionary<string, string>> s_paths = new(() =>
            {
                string myArch =
                    RuntimeInformation.ProcessArchitecture switch
                    {
                        Architecture.X86 => "x86",
                        Architecture.X64 => "x86-64",
                        Architecture.Arm => "arm",
                        Architecture.Arm64 => "arm-64",
                        _ => "unknown"
                    };

                Dictionary<string, string> result = new();

                try
                {
                    var info = new ProcessStartInfo("/bin/sh", "-c \"ldconfig -p\"")
                    {
                        RedirectStandardOutput = true,
                        UseShellExecute = false
                    };

                    using var proc = Process.Start(info);

                    if (proc == null)
                    {
                        Report.Warn("Unable to launch ldconfig.");
                    }
                    else
                    {
                        proc.OutputDataReceived += (s, e) =>
                        {
                            if (!String.IsNullOrWhiteSpace(e.Data))
                            {
                                var m = rx.Match(e.Data);
                                if (m.Success)
                                {
                                    var name = m.Groups["name"].Value;
                                    var arch = m.Groups["arch"].Value;
                                    var path = m.Groups["path"].Value;
                                    if (arch == myArch)
                                    {
                                        result[name] = path;
                                    }
                                }
                            }
                        };

                        proc.BeginOutputReadLine();
                        proc.WaitForExit();
                    }
                }
                catch (Exception e)
                {
                    Report.Warn($"Failed to query ldconfig: {e.Message}");
                }

                return result;
            });

            public static bool TryGetPath(string name, out string path)
                => s_paths.Value.TryGetValue(name, out path);
        }

        #endregion

        #region DllMap

        private static bool TryParseOS(string os, out OS value)
        {
            os = os.ToLower();
            if (os == "win" || os == "windows" || os == "win32" || os == "win64")
            {
                value = OS.Win32;
                return true;
            }

            if (os == "linux" || os == "nix" || os == "unix")
            {
                value = OS.Linux;
                return true;
            }

            if (os == "mac" || os == "macos" || os == "macosx")
            {
                value = OS.MacOS;
                return true;
            }

            value = OS.Unknown;
            return false;
        }

        private static Dictionary<string, string> GetSymlinks(XDocument document)
        {
            var dict = new Dictionary<string, string>();
            var root = document.Element(XName.Get("configuration"));
            if(root != null)
            {
                foreach(var e in root.Elements(XName.Get("dllmap")))
                {
                    var src     = e.Attribute(XName.Get("dll"));
                    var os      = e.Attribute(XName.Get("os"));
                    var dst     = e.Attribute(XName.Get("target"));

                    if(src != null && dst != null && os != null)
                    {
                        var srcStr  = src.Value;
                        var osStr   = os.Value;
                        var dstStr  = dst.Value;

                        if(!String.IsNullOrWhiteSpace(srcStr) && !String.IsNullOrWhiteSpace(osStr) && !String.IsNullOrWhiteSpace(dstStr) && TryParseOS(osStr, out var osVal))
                        {
                            if(Platform == osVal)
                            {
                                dict[srcStr] = dstStr;
                            }
                        }
                    }

                }
            }

            return dict;

        }

        #endregion

        #region Symlink

        [DllImport("libc", SetLastError = true)]
        private static extern int symlink(string src, string linkName);

        private static void CreateSymlink(string baseDir, string src, string dst)
        {
            if (Platform == OS.Win32 || Platform == OS.Unknown) return;

            string targetName;
            if (LdConfig.TryGetPath(dst, out string targetPath))
            {
                targetName = targetPath;
            }
            else
            {
                targetName = dst;
                targetPath = Path.Combine(baseDir, targetName);
            }

            if (File.Exists(targetPath))
            {
                var linkPath = Path.Combine(baseDir, src);

                Report.Line(3, $"Creating symlink '{src}' -> '{targetName}'");

                try
                {
                    if (File.Exists(linkPath))
                    {
                        File.Delete(linkPath);
                    }
                }
                catch (Exception e)
                {
                    Report.Warn($"Failed to delete symlink '{linkPath}': {e.Message}");
                }

                if (symlink(targetName, linkPath) != 0)
                {
                    Report.Warn($"Could not create symlink '{src}' (error: {Marshal.GetLastWin32Error()})");
                }
            }
            else
            {
                Report.Warn($"Could not create symlink to '{targetName}' (does not exist)");
            }
        }

        #endregion
    }
}