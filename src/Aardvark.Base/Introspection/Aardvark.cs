namespace Aardvark.Base;

using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

#if NET8_0_OR_GREATER
using System.Runtime.Loader;
#endif

// TODO: static class
[Serializable]
public partial class Aardvark
{
    [Obsolete("Use static methods instead.")]
    public Aardvark()
    {
    }

    private enum OS
    {
        Unknown = 0,
        Win32 = 1,
        Linux = 2,
        MacOS = 3
    }

    private static bool TryParseOS(string os, out OS value)
    {
        value =
            os?.ToLower() switch
            {
                "win" or "win32" or "win64" or "windows" => OS.Win32,
                "linux" or "nix" or "unix"               => OS.Linux,
                "mac" or "macos" or "macosx"             => OS.MacOS,
                _ => OS.Unknown
            };

        return value != OS.Unknown;
    }

    private static OS GetOS()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OS.Win32 :
               RuntimeInformation.IsOSPlatform(OSPlatform.OSX)     ? OS.MacOS :
               RuntimeInformation.IsOSPlatform(OSPlatform.Linux)   ? OS.Linux :
               OS.Unknown;
    }

    /// <summary>
    /// Gets the current operating system platform as an <see cref="OSPlatform"/> value.
    /// </summary>
    /// <remarks>
    /// The method checks <see cref="System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform)"/>
    /// and returns one of <see cref="OSPlatform.Windows"/>, <see cref="OSPlatform.OSX"/>, or <see cref="OSPlatform.Linux"/>.
    /// If none of these are detected, an <see cref="OSPlatform"/> with the name "UNKNOWN" is returned via
    /// <see cref="OSPlatform.Create(string)"/>. This can happen on unsupported or future platforms.
    /// </remarks>
    /// <returns>
    /// The current platform descriptor.
    /// </returns>
    public static OSPlatform GetOSPlatform()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? OSPlatform.Windows :
               RuntimeInformation.IsOSPlatform(OSPlatform.OSX)     ? OSPlatform.OSX :
               RuntimeInformation.IsOSPlatform(OSPlatform.Linux)   ? OSPlatform.Linux :
               OSPlatform.Create("UNKNOWN");
    }

    private static bool isInitialized;

    public static void Init()
    {
        if (isInitialized) return;

        Report.BeginTimed("Initializing Aardvark");

        const string framework =
#if NET8_0
            ".NET 8.0";
#elif NETSTANDARD2_0
            ".NET Standard 2.0";
#endif

        Report.Begin("System Information:");
        Report.Line("System:      {0}", RuntimeInformation.OSDescription);
        Report.Line("Processor:   {0} core {1}", Environment.ProcessorCount, ArchitectureString(RuntimeInformation.OSArchitecture));
        Report.Line("Process:     {0}", ArchitectureString(RuntimeInformation.ProcessArchitecture));
        Report.Line("Runtime:     {0}", RuntimeInformation.FrameworkDescription);
        Report.Line("Framework:   {0}", framework);

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

                Report.End(4, $" - not found");
                return null;
            }
            catch (Exception e)
            {
                Report.End(4, $" - failed: {e}");
                return null;
            }
        };

        // Called whenever a native library cannot be resolved by the managed runtime.
        // Note that this is only called for native libraries that are loaded from managed code.
        // Dependencies of native libraries are resolved using the dynamic linker of the platform.
        // On Linux, this can be problematic if an assembly has native dependencies consisting of
        // multiple libraries (e.g., liba and libb will be unpacked to the same directory, where liba requires libb).
        // In this case the libraries must be built with rpath set to $ORIGIN so that libb can be found next to liba.
        AssemblyLoadContext.Default.ResolvingUnmanagedDll += (assembly, name) =>
        {
            Report.Begin(4, $"Resolving native library '{name}'");
            var ptr = LoadLibrary(assembly, name, global: false, resolving: true);
            Report.End(4);
            return ptr;
        };
#endif
        // Set error mode to SEM_FAILCRITICALERRORS:
        // The system does not display the critical-error-handler message box.
        // Instead, the system sends the error to the calling process.
        if (GetOS() == OS.Win32) Kernel32.SetErrorMode(Kernel32.SEM_FAILCRITICALERRORS);

        if (IntrospectionProperties.NativeLibraryUnpackingAllowed)
        {
            AppDomain.CurrentDomain.AssemblyLoad += (_, args) =>
            {
                LoadNativeDependencies(args.LoadedAssembly);
            };

            Report.BeginTimed("Loading native dependencies");

            foreach (var assmebly in AppDomain.CurrentDomain.GetAssemblies())
            {
                LoadNativeDependencies(assmebly);
            }

            Report.EndTimed();
        }
        else
        {
            Report.Line(4, "Skipping native dependency loading since NativeLibraryUnpackingAllowed = false");
        }

        if (IntrospectionProperties.PluginsEnabled)
        {
            Plugins.Init();
        }
        else
        {
            Report.Line(4, "Skipping plugin loading since PluginsEnabled = false");
        }

        Report.End();

        isInitialized = true;
    }
}