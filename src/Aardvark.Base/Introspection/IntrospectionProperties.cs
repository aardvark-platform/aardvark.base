namespace Aardvark.Base;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

public static class IntrospectionProperties
{
    /// <summary>
    /// Introspection is based on Assembly.GetEntryAssembly which represents the managed
    /// entry point (i.e. the first assembly that was executed by AppDomain.ExecuteAssembly).
    /// However, started from an unmanaged entry point (like Visual Studio tests) Assembly.GetEntryAssembly
    /// is null. To allow us to start from unmanaged hosting processes this alternative
    /// has been implemented.
    /// A second use case are managed interactive shells like fsi.exi. Here we want to
    /// start our dependency walk at a custom assembly instead of the running assembly
    /// which is the interactive shell host.
    /// </summary>
    public static Assembly CustomEntryAssembly { get; set; }

    public static Assembly CurrentEntryAssembly => CustomEntryAssembly ?? Assembly.GetEntryAssembly();

    /// <summary>
    /// Whether plugin loading should be enabled
    /// </summary>
    public static bool PluginsEnabled = true;

    /// <summary>
    /// Whether Aardvark.Init should unpack native libraries
    /// </summary>
    public static bool NativeLibraryUnpackingAllowed = true;


    public static string CurrentEntryPath
    {
        get
        {
            var location = CurrentEntryAssembly?.GetLocationSafe();
            return PathUtils.GetDirectoryNameSafe(location) ?? AppDomain.CurrentDomain.BaseDirectory;
        }
    }

    /// <summary>
    /// Returns the path to the single file deployed entry bundle if it exists, null otherwise.
    /// </summary>
    public static string CurrentEntryBundle
    {
        get
        {
            var entryAssembly = CurrentEntryAssembly;

            // If Location is empty or null, we might have a single-file application
            if (entryAssembly != null && !entryAssembly.HasLocation())
            {
                var name = entryAssembly.GetName().Name;
                var ext = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ".exe" : "";
                var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, name + ext);

                if (File.Exists(path))
                {
                    return path;
                }

                Report.Warn($"Could not find bundle executable: {path}");
            }

            return null;
        }
    }

    private static readonly HashSet<string> s_defaultAssemblyBlacklist =
        [
            "ASift.CLI",
            "CUDA.NET",
            "Emgu.CV",
            "levmarCLI",
            "LSDCli",
            "OpenNI.Net",
            "OpenSURFcs",
            "PdfSharp",
            "PresentationFramework",
            "PresentationCore",
            "PrimitiveShapesCLI",
            "SiftGpu.CLI",
            "SlamToroCli",
            "System",
            "WindowsBase",
            "OpenTK",
            "Suave",
            "Newtonsoft.Json",
            "Aether",
            "CommonMark",
            "CSharp.Data.Adaptive",
            "FSharp.Data.Adaptive",
            "ICSharpCode.SharpZipLib",
            "LibTessDotNet",
            "SixLabors.ImageSharp",
            "netstandard",
            "FsPickler",
            "WindowsFormsIntegration",
            "Unofficial.Typography"
        ];

    /// <summary>
    /// Given an assembly name, should it be considered as plugin/native resource etc. This one by default excludes portions of clr base library
    /// </summary>
    public static bool DefaultAssemblyFilter(string name)
    {
        return !(s_defaultAssemblyBlacklist.Contains(name) ||
                 name.StartsWith("System.") ||
                 name.StartsWith("VRLib.CLI") ||
                 name.StartsWith("Microsoft") ||
                 name.StartsWith("LidorSystems") ||
                 name.StartsWith("WeifenLuo.") ||
                 name.StartsWith("OpenCV") && name != "OpenCVSharp" || // OpenCVSharp.dll contains native assemblies that need to be unpacked
                 name.StartsWith("nunit.") ||
                 name.StartsWith("Extreme.Numerics") ||
                 name.StartsWith("fftwlib") ||
                 name.StartsWith("GraphCutsCLI") ||
                 name.StartsWith("Interop.MLApp") ||
                 name.StartsWith("IPP") && name != "IPP" || // IPP.dll contains native assemblies that need to be unpacked
                 name.StartsWith("IronRuby") ||
                 name.StartsWith("MapTools") ||
                 name.StartsWith("MetaDataExtractor") ||
                 name.StartsWith("mscorlib") ||
                 name.StartsWith("SlimDX") ||
                 name.StartsWith("TDx.TDxInput") ||
                 name.StartsWith("WiimoteLib") ||
                 name.StartsWith("OpenTK") ||
                 name.StartsWith("Kitware") ||
                 name.StartsWith("ICSharpCode") ||
                 name.StartsWith("Roslyn") ||
                 name.StartsWith("SharpDX") ||
                 name.StartsWith("Aardvark.Jynx.Native") ||
                 name.StartsWith("SurfaceQueueInteropHelper") ||
                 name.StartsWith("ScintillaNET") ||
                 name.StartsWith("IKVM") ||
                 name.StartsWith("Super") ||
                 name.StartsWith("Java") ||
                 name.StartsWith("PresentationFramework") ||
                 name.StartsWith("FShade") ||
                 name.StartsWith("Xceed") ||
                 name.StartsWith("UIAutomation") ||
                 name.StartsWith("Uncodium")
                 );
    }

    /// <summary>
    /// Determines if the built-in assembly filter is ignored or applied (default: false).
    /// </summary>
    public static bool IgnoreDefaultAssemblyFilter { get; set; } = false;

    /// <summary>
    /// Filter function determining if an assembly with the given name (without extension) should be loaded or ignored.
    /// <see cref="IgnoreDefaultAssemblyFilter"/> determines if the function is applied on top of the built-in filter rules.
    /// </summary>
    public static Func<string, bool> CustomAssemblyFilter { get; set; } = (_) => true;

    /// <summary>
    /// Filters assemblies according to DefaultAssemblyFilter
    /// </summary>
    public static bool AssemblyFilter(string name)
    {
        return (IgnoreDefaultAssemblyFilter || DefaultAssemblyFilter(name)) && CustomAssemblyFilter(name);
    }

    /// <summary>
    /// can be set from application code to disable native unpacking for specific assemblies. Per default, we use the same filter as for EnumerateAssemblies.
    /// It is absolute crucial to prevent exceptions within this code because of mscorlib resource bug detection
    /// </summary>
    public static Func<Assembly, bool> UnpackNativeLibrariesFilter =
        assembly =>
        {
            var name = assembly?.GetName()?.Name;
            return name != null && AssemblyFilter(name);
        };

    [Obsolete]
    public static string BundleEntryPoint = "";

    [Obsolete]
    public static Func<Assembly, DateTime> GetLastWriteTimeUtc =
        assembly => assembly.GetLastWriteTimeSafe();
}