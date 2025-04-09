using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

#if NET8_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace Aardvark.Base
{
    public enum OS
    {
        Unknown = 0,
        Win32 = 1,
        Linux = 2,
        MacOS = 3
    }

    // TODO: static class
    [Serializable]
    public partial class Aardvark
    {
        [Obsolete("Use static methods instead.")]
        public Aardvark()
        {
        }

        private static readonly Lazy<OS> s_currentOS = new(() =>
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return OS.Win32;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return OS.MacOS;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return OS.Linux;
            return OS.Unknown;
        });

        /// <summary>
        /// The current operating system.
        /// </summary>
        public static OS Platform => s_currentOS.Value;

        private static string ArchitectureString(Architecture arch)
        {
            switch (arch)
            {
                case Architecture.X86: return "x86";
                case Architecture.X64: return "x64";
                case Architecture.Arm: return "arm";
                case Architecture.Arm64: return "arm64";
                default: return arch.ToString();
            }
        }

        private static bool isInitialized;

        public static void Init()
        {
            if (isInitialized) return;

            Report.BeginTimed("initializing aardvark");

            Report.Begin("System Information:");
            Report.Line("System:      {0}", RuntimeInformation.OSDescription);
            Report.Line("Processor:   {0} core {1}", Environment.ProcessorCount, ArchitectureString(RuntimeInformation.OSArchitecture));
            Report.Line("Process:     {0}", ArchitectureString(RuntimeInformation.ProcessArchitecture));
            Report.Line("Framework:   {0}", RuntimeInformation.FrameworkDescription);

            if (RuntimeInformation.ProcessArchitecture != Architecture.X64)
            {
                Report.Error("{0} is not officially supported yet: some features may not work correctly", ArchitectureString(RuntimeInformation.ProcessArchitecture));
            }

            Report.End();

#if NETCOREAPP3_1_OR_GREATER
            // Assembly loading via name fails if the assembly is not referenced as runtime or compile library.
            // This happens when a plugin is not referenced but simply dropped next to the entry assembly (e.g. multiple projects share the same output folder).
            // Here we just try to locate the assembly next to the entry path.
            // See: https://github.com/Particular/Workshop/issues/64
            AssemblyLoadContext.Default.Resolving += (ctx, name) =>
            {
                Report.Begin(4, $"Trying to resolve assembly: {name.FullName}");

                try
                {
                    var path = Path.Combine(IntrospectionProperties.CurrentEntryPath, name.Name + ".dll");

                    if (File.Exists(path))
                    {
                        var asm = ctx.LoadFromAssemblyPath(path);
                        Report.End(4, $" - success: {path}");
                        return asm;
                    }
                    else
                    {
                        Report.End(4, $" - not found");
                        return null;
                    }
                }
                catch (Exception e)
                {
                    Report.End(4, $" - failed: {e}");
                    return null;
                }
            };

            AssemblyLoadContext.Default.ResolvingUnmanagedDll += (ass, name) =>
            {
                Report.Line(4, "trying to resolve native library {0}", name);
                try { return LoadLibrary(null, name); }
                catch (Exception) { return IntPtr.Zero; }
            };
#else
            if (Platform == OS.Win32)
                Kernel32.SetErrorMode(1); // set error mode to SEM_FAILCRITICALERRORS 0x0001: do not display cirital error message boxes
#endif

            static void loadNativeDependencies(Assembly assembly)
            {
                try
                {
                    // https://github.com/aardvark-platform/aardvark.base/issues/61
                    if (IntrospectionProperties.UnpackNativeLibrariesFilter(assembly))
                    {
                        LoadNativeDependencies(assembly);
                    }
                    else
                    {
                        Report.Line(4, "Skipped loading native dependencies for {0}", assembly.FullName);
                    }
                }
                catch (Exception ex) // actually catching exns here might not even be possible due to mscorlib recursive resource bug detection
                {
                    Report.Warn($"Could not load native dependencies for {assembly.FullName}: {ex.Message}");
                }
            }

            if (IntrospectionProperties.NativeLibraryUnpackingAllowed)
            {
                AppDomain.CurrentDomain.AssemblyLoad += (s, e) =>
                {
                    loadNativeDependencies(e.LoadedAssembly);
                };

                foreach (var a in AppDomain.CurrentDomain.GetAssemblies())
                {
                    loadNativeDependencies(a);
                }
            }
            else
            {
                Report.Line(4, "Skipping native dependency loading since NativeLibraryUnpackingAllowed = false");
            }

            if (IntrospectionProperties.PluginsEnabled)
            {
                Plugins.Init();
            }

            Report.End();

            isInitialized = true;
        }
    }
}