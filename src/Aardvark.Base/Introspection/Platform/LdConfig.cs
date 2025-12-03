namespace Aardvark.Base;

using System;
using System.Collections.Generic;
using System.Diagnostics;

public partial class Aardvark
{
    private static class LdConfig
    {
        private static readonly Lazy<Dictionary<string, string>> s_lookup = new(Load);

        static List<string> Run(string path)
        {
            Report.Begin(3, $"{path} -p");

            try
            {
                using var p = new Process();
                p.StartInfo.FileName = path;
                p.StartInfo.Arguments = "-p";
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.CreateNoWindow = true;

                var result = new List<string>();

                p.ErrorDataReceived += (_, _) => { };
                p.OutputDataReceived += (_, args) =>
                {
                    lock (result)
                    {
                        if (args.Data != null) result.Add(args.Data);
                    }
                };

                p.Start();
                p.BeginOutputReadLine();
                p.BeginErrorReadLine();
                p.WaitForExit();

                if (p.ExitCode == 0)
                {
                    Report.End(3, " - success");
                    return result;
                }

                var output = string.Join(Environment.NewLine, result);
                Report.Line(3, output);

            }
            catch (Exception e)
            {
                Report.Line(3, $"{e.GetType().Name}: {e.Message}");
            }

            Report.End(3, " - failed");
            return null;
        }

        static Dictionary<string, string> Load()
        {
            var result = new Dictionary<string, string>();
            Report.BeginTimed(3, "Building shared library location cache");

            try
            {
                string[] commands = ["/usr/sbin/ldconfig", "/sbin/ldconfig", "ldconfig"];

                List<string> output = null;
                foreach (var command in commands)
                {
                    output = Run(command);
                    if (output != null) break;
                }

                var directories = new HashSet<string>();

                if (output != null)
                {
                    foreach (var data in output)
                    {
                        var m = RegexPatterns.LdConfig.Entry?.Match(data);
                        if (m is { Success: true })
                        {
                            var name = m.Groups["name"].Value.ToLowerInvariant();
                            var path = m.Groups["path"].Value;

                            var directory = PathUtils.GetDirectoryNameSafe(path);
                            if (directory != null) directories.Add(directory);

                            result[name] = path;
                        }
                    }
                }

                // Find files in library directories that are not in cache
                foreach (var directory in directories)
                {
                    foreach (var path in DirectoryUtils.GetFilesSafe(directory))
                    {
                        var name = PathUtils.GetFileNameSafe(path)?.ToLowerInvariant();

                        if (name != null && IsNativeLibrary(name) && !result.ContainsKey(name))
                        {
                            result[name] = path;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Report.Warn($"{e.GetType().Name}: {e.Message}");
            }

            Report.Line(3, $"Found {result.Count} libraries");
            Report.EndTimed(3);

            return result;
        }

        public static ICollection<string> AllPaths => s_lookup.Value.Values;

        public static bool TryGetPath(string name, out string path)
        {
            if (name != null) return s_lookup.Value.TryGetValue(name.ToLowerInvariant(), out path);
            path = null;
            return false;
        }
    }
}