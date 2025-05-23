﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using BundleReader = SingleFileExtractor.Core.ExecutableReader;
using BundleFileEntry = SingleFileExtractor.Core.FileEntry;

#if NET8_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace Aardvark.Base
{
    internal static class FileUtils
    {
        public static DateTime GetLastWriteTimeSafe(string path)
        {
            try
            {
                return File.Exists(path) ? File.GetLastWriteTimeUtc(path) : DateTime.MaxValue;
            }
            catch (Exception)
            {
                Report.Warn($"Could not get write time for: {path}");
                return DateTime.MaxValue;
            }
        }

        public static bool TryFindInDirectory(string directory, string filename, out string absolutePath)
        {
            try
            {
                foreach (var path in Directory.GetFiles(directory))
                {
                    var name = Path.GetFileName(path);
                    if (string.Equals(name, filename, StringComparison.OrdinalIgnoreCase))
                    {
                        absolutePath = path;
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Report.Warn($"Error while trying to find {filename} in directory '{directory}': {e.Message}");
            }

            absolutePath = null;
            return false;
        }
    }

    internal static class PathUtils
    {
        public static string GetDirectoryNameSafe(string path)
        {
            try
            {
                return (path != null) ? Path.GetDirectoryName(path) : null;
            }
            catch { return null; }
        }
    }

    internal static class AssemblyExtenions
    {
        public static string GetLocationSafe(this Assembly assembly)
        {
            try
            {
                var location = assembly.Location;
                return location.IsNullOrEmpty() ? null : location;
            }
            catch { return null; }
        }

        public static bool HasLocation(this Assembly assembly)
            => assembly.GetLocationSafe() != null;

        public static DateTime GetLastWriteTimeSafe(this Assembly assembly)
        {
            var location = assembly?.GetLocationSafe() ?? IntrospectionProperties.CurrentEntryBundle;
            return FileUtils.GetLastWriteTimeSafe(location);
        }
    }

    public static class CachingProperties
    {
        /// <summary>
        /// Naming schemes for cache files.
        /// </summary>
        public enum NamingScheme
        {
            /// <summary>
            /// Name is based on the version of the assembly.
            /// </summary>
            Version,

            /// <summary>
            /// Name is based on the modification time of the assembly file.
            /// </summary>
            Timestamp
        }

        private static string InitializeCacheDirectory()
        {
            var path = CustomCacheDirectory;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (string.IsNullOrWhiteSpace(path))
                {
                    Report.Warn("Environment.SpecialFolder.LocalApplicationData does not exist!");
                    path = "Cache"; // using ./Cache
                }
                else
                {
                    path = Path.Combine(path, "Aardvark", "Cache");
                }
            }

            Report.Line(4, "using cache dir: {0}", path);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        private static readonly Lazy<string> s_cacheDirectory = new Lazy<string>(InitializeCacheDirectory);

        /// <summary>
        /// If set, determines the directory holding cache files. Otherwise, a default directory is used.
        /// Must be set before <see cref="CacheDirectory"/> is used.
        /// </summary>
        public static string CustomCacheDirectory { get; set; }

        /// <summary>
        /// Directory holding cache files, if <see cref="CustomCacheDirectory"/> is not set, a default directory based on
        /// <see cref="Environment.SpecialFolder.ApplicationData"/> will be used instead.
        /// </summary>
        public static string CacheDirectory => s_cacheDirectory.Value;

        private static NamingScheme s_pluginsCacheFileNaming        = NamingScheme.Version;
        private static NamingScheme s_introspectionCacheFileNaming  = NamingScheme.Timestamp;

        /// <summary>
        /// Gets or sets the naming scheme used for plugins cache files.
        /// Default scheme is based on assembly version.
        /// </summary>
        public static NamingScheme PluginsCacheFileNaming
        {
            get => s_pluginsCacheFileNaming;
            set => s_pluginsCacheFileNaming = value;
        }

        /// <summary>
        /// Gets or sets the naming scheme used for introspection cache files.
        /// Default scheme is based on assembly file timestamp.
        /// </summary>
        public static NamingScheme IntrospectionCacheFileNaming
        {
            get => s_introspectionCacheFileNaming;
            set => s_introspectionCacheFileNaming = value;
        }

        internal static string GetIdentifier(this Assembly asm, NamingScheme scheme)
        {
            return scheme switch
            {
                NamingScheme.Version =>   asm.GetName().Version.ToString(),
                NamingScheme.Timestamp => asm.GetLastWriteTimeSafe().ToBinary().ToString(),
                _ => ""
            };
        }
    }

    public static class IntrospectionProperties
    {
        /// <summary>
        /// Introspection is based on Assembly.GetEntryAssembly which represents the managed
        /// entry point (i.e. the first assembly that was executed by AppDomain.ExecuteAssembly).
        /// However, started from an unmanaged entry point (like VisualStudio tests) Assembly.GetEntryAssembly
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
                    else
                    {
                        Report.Warn($"Could not find bundle executable: {path}");
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        private static readonly HashSet<string> s_defaultAssemblyBlacklist =
            new HashSet<string>(
                new[]
                {
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
                    "Unofficial.Typography",
                }
            );

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
                a =>
                {
                    if (a.FullName == null) return false;
                    else return AssemblyFilter(a.GetName().Name);
                };

        [Obsolete]
        public static string BundleEntryPoint = "";

        [Obsolete]
        public static Func<Assembly, DateTime> GetLastWriteTimeUtc =
            (Assembly assembly) => assembly.GetLastWriteTimeSafe();
    }

    public static class Introspection
    {
        private static readonly CultureInfo s_cultureInfoEnUs = new CultureInfo("en-us");
        private static readonly Dictionary<string, Assembly> s_assemblies;
        private static readonly HashSet<string> s_assembliesThatFailedToLoad = new HashSet<string>();
        private static readonly HashSet<Assembly> s_allAssemblies = new HashSet<Assembly>();

        private static string InitializeCacheDirectory()
        {
            var path = Path.Combine(CachingProperties.CacheDirectory, "Introspection");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        private static readonly Lazy<string> s_cacheDirectory = new Lazy<string>(InitializeCacheDirectory);

        /// <summary>
        /// Returns the directory of the introspection cache files.
        /// </summary>
        public static string CacheDirectory => s_cacheDirectory.Value;

        /// <summary>
        /// Registers an additional assembly at runtime to be
        /// used for subsequent introspection queries. This
        /// may be used for e.g. plugin scenarios.
        /// All assemblies that are reachable from the entry
        /// assembly are registered automatically at startup.
        /// </summary>
        public static void RegisterAssembly(Assembly a)
        {
            if (a == null) return;
            if (s_allAssemblies.Contains(a)) return;

            s_allAssemblies.Add(a);
        }

        /// <summary>
        /// Enumerates all known assemblies.
        /// </summary>
        public static IEnumerable<Assembly> AllAssemblies => s_allAssemblies;

        /// <summary>
        /// Enumerates all classes implementing the specified interface.
        /// </summary>
        public static IEnumerable<Type> GetAllClassesImplementingInterface(Type interfaceType)
            => AllAssemblies.SelectMany(a => GetAllClassesImplementingInterface(a, interfaceType));

        /// <summary>
        /// Enumerates all classes inheriting from the specified base class.
        /// </summary>
        public static IEnumerable<Type> GetAllClassesInheritingFrom(Type baseType)
            => AllAssemblies.SelectMany(a => GetAllClassesInheritingFrom(a, baseType));

        /// <summary>
        /// Enumerates all types decorated with attribute T as tuples of type
        /// and its one or more T-attributes.
        /// </summary>
        public static IEnumerable<(Type, T[])> GetAllTypesWithAttribute<T>()
            => AllAssemblies.SelectMany(a => GetAllTypesWithAttribute<T>(a));

        /// <summary>
        /// Enumerates all methods decorated with attribute T as tuples of MethodInfo
        /// and its one or more T-attributes.
        /// </summary>
        public static IEnumerable<(MethodInfo, T[])> GetAllMethodsWithAttribute<T>()
            => AllAssemblies.SelectMany(a => GetAllMethodsWithAttribute<T>(a));

        /// <summary>
        /// Enumerates all classes from the specified assembly
        /// implementing the specified interface.
        /// </summary>
        public static Type[] GetAllClassesImplementingInterface(Assembly a, Type interfaceType)
            => GetAll___<Type>(a, interfaceType.FullName,
                lines => lines.Select(s => Type.GetType(s)),
                types => types.Where(t => (t.IsClass || t.IsValueType) && t.GetInterfaces().Contains(interfaceType)),
                result => result.Select(t => t.AssemblyQualifiedName)
                );

        /// <summary>
        /// Enumerates all classes from the specified assembly
        /// inheriting from the specified base class.
        /// </summary>
        public static Type[] GetAllClassesInheritingFrom(Assembly a, Type baseType)
            => GetAll___<Type>(a, baseType.FullName,
                lines => lines.Select(s => Type.GetType(s)),
                types => types.Where(t => t.IsSubclassOf(baseType)),
                result => result.Select(t => t.AssemblyQualifiedName)
                );

        /// <summary>
        /// Enumerates all types from the specified assembly
        /// decorated with attribute T as tuples of type
        /// and its one or more T-attributes.
        /// </summary>
        public static (Type, T[])[] GetAllTypesWithAttribute<T>(Assembly a)
            => GetAll___<(Type, T[])>(a, typeof(T).FullName,
               lines => lines.Select(s => Type.GetType(s))
                        .Select(t => (t, TryGetCustomAttributes<T>(t))),
               types => from t in types
                        let attribs = TryGetCustomAttributes<T>(t)
                        where attribs.Length > 0
                        select (t, attribs.Select(x => (T)x).ToArray()),
               result => result.Select(t => t.Item1.AssemblyQualifiedName)
               );

        private static T[] TryGetCustomAttributes<T>(Type t)
        {
            try
            {
                return t.GetCustomAttributes(typeof(T), false).Select(x => (T)x).ToArray();
            }
            catch (Exception e)
            {
                Report.Line(3, "[Introspection] Failed to get custom attributes for {0}: {1} ({2})", t.FullName, e.Message, e.GetType().Name);
            }
            return Array.Empty<T>();
        }

        private static T[] TryGetCustomAttributes<T>(MethodInfo t)
        {
            try
            {
                return t.GetCustomAttributes(typeof(T), false).Select(x => (T)x).ToArray();
            }
            catch (Exception e)
            {
                Report.Line(3, "[Introspection] Failed to get custom attributes for {0}.{1}: {2} ({3})", t.DeclaringType.FullName, t.Name, e.Message, e.GetType().Name);
            }
            return Array.Empty<T>();
        }

        /// <summary>
        /// Enumerates all methods from the specified assembly
        /// decorated with attribute T as tuples of MethodInfo
        /// and its one or more T-attributes.
        /// </summary>
        public static (MethodInfo, T[])[] GetAllMethodsWithAttribute<T>(Assembly a)
            => GetAll___<(MethodInfo, T[])>(a, typeof(T).FullName,
                  lines => from line in lines
                           let t = Type.GetType(line)
                           where t != null
                           from m in t.GetMethods()
                           let attribs = TryGetCustomAttributes<T>(m)
                           where attribs.Length > 0
                           select (m, attribs),
                  types => from t in types
                           where t != null
                           from m in t.GetMethods()
                           let attribs = TryGetCustomAttributes<T>(m)
                           where attribs.Length > 0
                           select (m, attribs),
                  result => result.Select(m => m.Item1.DeclaringType.AssemblyQualifiedName)
                  );

        static Introspection()
        {
            // enumerating all assemblies reachable from entry assembly
            s_assemblies = new Dictionary<string, Assembly>();

            var entryAssembly = IntrospectionProperties.CurrentEntryAssembly ?? typeof(Aardvark).Assembly;

            if (entryAssembly == null)
            {
                Report.Warn("[Introspection] Could not determine entry assembly");
                RegisterAllAssembliesInPath(IntrospectionProperties.CurrentEntryPath);
            }
            else
            {
                var location = IntrospectionProperties.CurrentEntryBundle ?? entryAssembly.GetLocationSafe();
                if (location != null)
                    Report.Line(4, $"[Introspection] Entry assembly: {entryAssembly.FullName} (path: {location})");
                else
                    Report.Line(4, $"[Introspection] Entry assembly: {entryAssembly.FullName} (unknown location)");

                var name = entryAssembly.GetName().Name;
                EnumerateAssemblies(name, entryAssembly);
            }
        }

        /// <summary>
        /// Tries to load and register all assemblies in given path.
        /// </summary>
        [DebuggerNonUserCode]
        [Obsolete("Use overload without verbose parameter.")]
        public static void RegisterAllAssembliesInPath(string path, bool verbose)
            => RegisterAllAssembliesInPath(path);

        /// <summary>
        /// Tries to load and register all assemblies in given path.
        /// </summary>
        [DebuggerNonUserCode]
        public static void RegisterAllAssembliesInPath(string path)
        {
            Report.Begin(4, $"[Introspection] Registering assemblies in: {path}");

            try
            {
                foreach (var file in Directory.GetFiles(path))
                {
                    var ext = Path.GetExtension(file).ToLowerInvariant();
                    if (ext != ".dll" &&  ext != ".exe") continue;

                    try
                    {
                        var name = AssemblyName.GetAssemblyName(file);
                        Report.Line(4, $"{Path.GetFileName(file)}");
                        EnumerateAssemblies(name.Name);
                    }
                    catch
                    {
                    }
                }
            }
            catch (Exception e)
            {
                Report.Warn($"Error while registering assemblies in '{path}': {e.Message}");
            }
            finally
            {
                Report.End(4);
            }
        }

        /// <summary>
        /// Note by hs: Since this function throws and catches exceptions in non exceptional cases we
        /// use [DebuggerNonUserCode] to deactive first chance exceptions here
        /// at least if non user code is deactivated in Options/Debugging.
        /// </summary>
        /// <param name="name">the name of the entry assembly</param>
        /// <param name="customAssembly">If there the root assembly is not the assembly which has been startet
        /// by the AppDomain a customAssembly is used alternatively.</param>
        [DebuggerNonUserCode]
        private static void EnumerateAssemblies(string name, Assembly customAssembly = null)
        {
            if (string.IsNullOrEmpty(name)) return;
            if (s_assembliesThatFailedToLoad.Contains(name)) return;
            if (s_assemblies.ContainsKey(name)) return;

            if (!IntrospectionProperties.AssemblyFilter(name))
            {
                Report.Line(4, "[Introspection] Ignoring assembly {0} due to filter", name);
                return;
            }

            try
            {
                var assembly = customAssembly ?? Assembly.Load(name);
                s_assemblies[name] = assembly;
                RegisterAssembly(assembly);
                foreach (var a in assembly.GetReferencedAssemblies())
                {
                    string s = a.Name;
                    if (!s_assemblies.ContainsKey(s)) EnumerateAssemblies(s);
                }
            }
            catch //(Exception e)
            {
                s_assembliesThatFailedToLoad.Add(name);
                //Report.Warn(e.ToString());
                //Report.Warn("{0}", name);
            }
        }

        private static string GetQueryCacheFilename(Assembly asm, Guid queryGuid)
        {
            var name = asm.GetName().Name;
            var id = asm.GetIdentifier(CachingProperties.IntrospectionCacheFileNaming);
            return Path.Combine(CacheDirectory, $"{name}_{id}_{queryGuid}.query");
        }

        private class CacheFileHeader
        {
            public int Version;
            public DateTime TimeStampOfCachedFile;

            public override string ToString()
            {
                if (Version <= 0) throw new ArgumentOutOfRangeException(nameof(Version));
                return string.Format(s_cultureInfoEnUs,
                    "version {0} timestamp {1}", Version, TimeStampOfCachedFile.ToBinary()
                    );
            }

            public static CacheFileHeader Parse(string s)
            {
                if (string.IsNullOrEmpty(s)) throw new ArgumentNullException(nameof(s));
                if (!s.StartsWith("version")) return null; // old file without header
                var tokens = s.Split(' ');
                if (tokens.Length != 4) throw new FormatException();
                return new CacheFileHeader
                {
                    Version = int.Parse(tokens[1]),
                    TimeStampOfCachedFile = DateTime.FromBinary(long.Parse(tokens[3]))
                };
            }
        }
        private static T[] GetAll___<T>(
            Assembly a, string discriminator,
            Func<IEnumerable<string>, IEnumerable<T>> decode,
            Func<IEnumerable<Type>, IEnumerable<T>> createResult,
            Func<T[], IEnumerable<string>> encode
            )
        {
            var cacheFileName = "";
            var assemblyTimeStamp = DateTime.MinValue;

            // whatever happens, don't halt just because of caching... this actually happens for self-contained deployments https://github.com/aardvark-platform/aardvark.base/issues/65
            try
            {
                cacheFileName = GetQueryCacheFilename(a, discriminator.ToGuid());
                assemblyTimeStamp = a.GetLastWriteTimeSafe();

                // for standalone deployments cacheFileNames cannot be retrieved robustly - we skip those
                if (!String.IsNullOrEmpty(cacheFileName) && File.Exists(cacheFileName))
                {
                    var lines = File.ReadAllLines(cacheFileName);
                    var header = lines.Length > 0 ? CacheFileHeader.Parse(lines[0]) : null;
                    if (header != null && header.TimeStampOfCachedFile == assemblyTimeStamp)
                    {
                        // return cached types
                        Report.Line(4, "[cache hit ] {0}", a);
                        return decode(lines.Skip(1)).ToArray();
                    }
                }
            } catch(Exception e)
            {
                Report.Warn("Could not get cache for {1}: {0}", e.Message, a.FullName);
            }

            Report.Line(4, "[cache miss] {0}", a);

            // Notes by hs:
            // previously (rev 19495) typeloadexception resulted in empty result set.
            // even in case of typeloadexception there may be some successfully loaded
            // types in result set. Just continue processing with these types
            // effect: dlls with external unused dependencies don't have to be shipped.
            Type[] ts = Array.Empty<Type>();
            try
            {
                ts = a.GetTypes().ToArray();
            }
            catch (ReflectionTypeLoadException e)
            {
                Report.Begin("ReflectionTypeLoadException error in assembly {0}", a.GetName().Name);
                Report.Line("Full assembly name is {0}.", a.FullName);
                Report.Line("Exception is {0}", e);
                Report.Begin("Loader exceptions are");
                foreach (var x in e.LoaderExceptions)
                {
                    Report.Line("{0}", x);
                }
                Report.End();
                Report.End();
                ts = e.Types.Where(t => t != null).ToArray();
            }

            var result = createResult(ts).ToArray();


            // whatever happens, dont halt everything just because caching fails
            try
            {
                // for standalone deployments cacheFileNames cannot be retrieved robustly - we skip those
                if (!string.IsNullOrEmpty(cacheFileName))
                {

                    // write cache file
                    var headerLine =
                        new CacheFileHeader { Version = 1, TimeStampOfCachedFile = assemblyTimeStamp }
                        .ToString()
                        .IntoIEnumerable()
                        ;

                    File.WriteAllLines(cacheFileName, headerLine.Concat(encode(result)).ToArray());
                }
            } catch(Exception e)
            {
                Report.Warn("Could not write cache for {1}: {0}", e.Message, a.FullName);
            }

            return result;
        }
    }

    public class OnAardvarkInitAttribute : Attribute
    {
        public OnAardvarkInitAttribute() { }
    }

    internal static class Kernel32
    {
        public const uint DONT_RESOLVE_DLL_REFERENCES = 0x00000001;
        public const uint LOAD_IGNORE_CODE_AUTHZ_LEVEL = 0x00000010;
        public const uint LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR = 0x00000100;
        public const uint LOAD_LIBRARY_SEARCH_APPLICATION_DIR = 0x00000200;
        public const uint LOAD_LIBRARY_SEARCH_USER_DIRS = 0x00000400;
        public const uint LOAD_LIBRARY_SEARCH_SYSTEM32 = 0x00000800;
        public const uint LOAD_LIBRARY_SEARCH_DEFAULT_DIRS = 0x00001000;

        public const uint LOAD_WITH_ALTERED_SEARCH_PATH = 0x00000008;

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibraryEx(string path, IntPtr handle, uint dwFlags);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr AddDllDirectory(string path);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetLastError();

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetCurrentDirectory(string pathName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetDllDirectory(string pathName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetDllDirectory(int nBufferLength, StringBuilder lpPathName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr handle, string name);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern uint SetErrorMode(uint mode);
    }

    internal static class Dl
    {

        [DllImport("libdl", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr dlopen(string path, int flag);

        [DllImport("libdl", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr dlsym(IntPtr handle, string name);

    }

    // TODO: static class
    [Serializable]
    public class Aardvark
    {
        /// <summary>
        /// Cache containing information about whether an assembly is an Aardvark plugin.
        /// </summary>
        [CollectionDataContract(Name = "Plugins", ItemName = "Assembly", KeyName = "Path", ValueName = "Data")]
        private class PluginCache : Dictionary<string, PluginCache.Data>
        {
            [DataContract]
            public struct Data
            {
                /// <summary>
                /// Modification time stamp of the assembly when the cache was created.
                /// Used to determine if the cache has been invalidated.
                /// </summary>
                [DataMember(IsRequired = true)]
                public DateTime LastModified;

                /// <summary>
                /// Indicates whether the assembly is an Aardvark plugin.
                /// </summary>
                [DataMember(IsRequired = true)]
                public bool IsPlugin;

                public Data(DateTime lastModified, bool isPlugin)
                {
                    LastModified = lastModified;
                    IsPlugin = isPlugin;
                }
            }

            private static readonly DataContractSerializer serializer = new DataContractSerializer(typeof(PluginCache));

            public static PluginCache Deserialize(Stream stream)
                => (PluginCache)serializer.ReadObject(stream);

            public void Serialize(Stream stream)
            {
                using var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true });
                serializer.WriteObject(writer, this);
            }
        }

        private static readonly Lazy<string> s_pluginsCacheDirectory =
            new (() =>
            {
                var path = Path.Combine(CachingProperties.CacheDirectory, "Plugins");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                return path;
            });

        /// <summary>
        /// Returns the directory of the plugins cache files.
        /// </summary>
        public static string PluginsCacheDirectory => s_pluginsCacheDirectory.Value;

        [Obsolete("Use PluginsCacheDirectory instead.")]
        public static string CacheDirectory => PluginsCacheDirectory;

        private static readonly Lazy<string> s_pluginsCacheFile =
            new(() =>
            {
                Assembly entryAssembly = IntrospectionProperties.CurrentEntryAssembly;
                string entryAssemblyName = entryAssembly?.GetName().Name ?? "unknown";
                string entryAssemblyId = entryAssembly?.GetIdentifier(CachingProperties.PluginsCacheFileNaming) ?? "unknown";
                string fileName = string.Format("{0}_{1}_plugins.xml", entryAssemblyName, entryAssemblyId);

                return Path.Combine(PluginsCacheDirectory, fileName);
            });

        /// <summary>
        /// Returns the path of the plugins cache file.
        /// </summary>
        public static string PluginsCacheFile => s_pluginsCacheFile.Value;

        [Obsolete("Use PluginsCacheFile instead.")]
        public string CacheFile = string.Empty;

        [Obsolete("Use static methods instead.")]
        public Aardvark()
        {
        }

        private static PluginCache ReadCacheFile()
        {
            if (File.Exists(PluginsCacheFile))
            {
                try
                {
                    using var stream = new FileStream(PluginsCacheFile, FileMode.Open);
                    var result = PluginCache.Deserialize(stream);
                    Report.Line(3, $"[ReadCacheFile] Loaded plugins cache file: {PluginsCacheFile}");
                    return result;
                }
                catch (Exception e)
                {
                    Report.Line(3, $"[ReadCacheFile] Could not load plugins cache file '{PluginsCacheFile}': {e.Message}");
                    return new PluginCache();
                }
            }
            else
            {
                Report.Line(3, $"[ReadCacheFile] Using new plugins cache file: {PluginsCacheFile}");
                return new PluginCache();
            }
        }


        private static void WriteCacheFile(PluginCache cache)
        {
            if (string.IsNullOrEmpty(PluginsCacheFile))
            {
                Report.Warn("Could not write plugins cache file since Aardvark.PluginCacheFile was null or empty");
            }
            else
            {
                try
                {
                    if (File.Exists(PluginsCacheFile)) File.Delete(PluginsCacheFile);

                    using var stream = new FileStream(PluginsCacheFile, FileMode.CreateNew);
                    cache.Serialize(stream);
                }
                catch(Exception e)
                {
                    Report.Warn($"Could not write plugins cache file '{PluginsCacheFile}': {e.Message}");
                }
            }
        }

        private static readonly Regex versionRx = new Regex(@"^[ \t]*(?<name>[\.A-Za-z_0-9]+)[ \t]*,[ \t]*(v|V)ersion[ \t]*=[ \t]*(?<version>[\.A-Za-z_0-9]+)$");

        private static unsafe bool ProbeForPlugin(Stream stream)
        {
            using var v = new PEReader(stream, PEStreamOptions.LeaveOpen);

            if (v.PEHeaders.CorHeader == null || !v.HasMetadata) return false;
            var data = v.GetMetadata();
            var m = new MetadataReader(data.Pointer, data.Length);

            var assdef = m.GetAssemblyDefinition();
            foreach (var att in assdef.GetCustomAttributes())
            {
                var attDef = m.GetCustomAttribute(att);
                if (attDef.Constructor.Kind == HandleKind.MemberReference)
                {
                    var hh = (MemberReferenceHandle)attDef.Constructor;
                    var e = m.GetMemberReference(hh);
                    var pp = e.Parent;
                    if (pp.Kind == HandleKind.TypeReference)
                    {
                        var attType = m.GetTypeReference((TypeReferenceHandle)pp);
                        var nameStr = m.GetString(attType.Name);
                        var nsStr = m.GetString(attType.Namespace);
                        if (nsStr == "System.Runtime.Versioning" && nameStr == "TargetFrameworkAttribute")
                        {
                            var reader = m.GetBlobReader(attDef.Value);
                            if (reader.ReadUInt16() == 1)
                            {
                                var version = reader.ReadSerializedString();
                                var match = versionRx.Match(version);
                                if (match.Success)
                                {
                                    var fwName = match.Groups["name"].Value;
                                    var isLoadable =
                                        (fwName == ".NETCoreApp") ||
                                        (fwName == ".NETStandard");
                                    if (!isLoadable) return false;
                                }
                            }
                        }
                    }
                }
            }

            foreach (var t in m.TypeDefinitions)
            {
                var def = m.GetTypeDefinition(t);
                foreach (var meth in def.GetMethods())
                {
                    var mdef = m.GetMethodDefinition(meth);
                    var hasInitAtt =
                        mdef.GetCustomAttributes().Any(att =>
                        {
                            var attDef = m.GetCustomAttribute(att);
                            if (attDef.Constructor.Kind == HandleKind.MemberReference)
                            {
                                var hh = (MemberReferenceHandle)attDef.Constructor;
                                var e = m.GetMemberReference(hh);
                                var pp = e.Parent;
                                if (pp.Kind == HandleKind.TypeReference)
                                {
                                    var attType = m.GetTypeReference((TypeReferenceHandle)pp);
                                    var nameStr = m.GetString(attType.Name);
                                    var nsStr = m.GetString(attType.Namespace);
                                    return nsStr == "Aardvark.Base" && nameStr == nameof(OnAardvarkInitAttribute);
                                }
                                else return false;
                            }
                            else return false;
                        });

                    if (hasInitAtt) return true;
                }
            }

            return false;
        }

        private abstract class AssemblySource
        {
            public abstract string Path { get; }

            public abstract DateTime LastModified { get; }

            public abstract Stream OpenRead();

            public abstract Assembly Load();
        }

        private class FileAssemblySource : AssemblySource
        {
            public override string Path { get; }

            public override DateTime LastModified { get; }

            public FileAssemblySource(string path)
            {
                Path = path;
                LastModified = FileUtils.GetLastWriteTimeSafe(path);
            }

            public override Stream OpenRead()
                => File.OpenRead(Path);

            public override Assembly Load()
            {
#if NETCOREAPP3_1_OR_GREATER
                // In .NET core Assembly.LoadFile uses a separate context, resulting in assemblies being
                // potentially loaded multiple times -> leads to problems with static fields in unit tests
                // See: https://github.com/dotnet/runtime/issues/39783
                return AssemblyLoadContext.Default.LoadFromAssemblyPath(Path);
#else
                return Assembly.LoadFile(Path);
#endif
            }
        }

        private class BundleAssemblySource : AssemblySource
        {
            private readonly BundleFileEntry m_entry;

            private byte[] m_data;

            public override string Path { get; }

            public override DateTime LastModified { get; }

            public BundleReader Reader => m_entry.ExecutableReader;

            public BundleAssemblySource(BundleFileEntry entry)
            {
                var bundlePath = entry.ExecutableReader.FileName;
                Path = System.IO.Path.Combine(bundlePath, entry.RelativePath);
                LastModified = FileUtils.GetLastWriteTimeSafe(bundlePath);
                m_entry = entry;
            }

            private byte[] GetData()
            {
                if (m_data == null)
                {
                    using var s = m_entry.AsStream();
                    using var ms = new MemoryStream(s.CanSeek ? (int)s.Length : 0);
                    s.CopyTo(ms);
                    m_data = ms.GetBuffer();
                }

                return m_data;
            }

            public override Stream OpenRead()
                => new MemoryStream(GetData());

            public override Assembly Load()
            {
#if NETCOREAPP3_1_OR_GREATER
                using var s = OpenRead();
                return AssemblyLoadContext.Default.LoadFromStream(s);
#else
                return Assembly.Load(GetData());
#endif
            }
        }

        private class AssemblySourceList : List<AssemblySource>, IDisposable
        {
            private BundleReader m_reader;

            public void AddDirectory(string path)
            {
                foreach (var p in Directory.GetFiles(path))
                {
                    Add(new FileAssemblySource(p));
                }
            }

            public void AddBundle(BundleReader reader)
            {
                if (m_reader != null)
                    throw new InvalidOperationException("Cannot add multiple bundles.");

                m_reader = reader;

                if (reader.IsSingleFile && reader.IsSupported)
                {
                    foreach (var e in reader.Bundle.Files)
                    {
                        Add(new BundleAssemblySource(e));
                    }
                }
                else
                {
                    Report.Warn($"Cannot read bundle executable: {reader.FileName}");
                }
            }

            public void Dispose()
            {
                m_reader?.Dispose();
                m_reader = null;
            }
        }

        private static AssemblySourceList FindAssemblySources()
        {
            var sources = new AssemblySourceList();

            try
            {
                var bundlePath = IntrospectionProperties.CurrentEntryBundle;

                if (bundlePath != null)
                {
                    try
                    {
                        var reader = new BundleReader(bundlePath);
                        sources.AddBundle(reader);
                    }
                    catch (Exception e)
                    {
                        Report.Warn($"Failed to get assemblies from single file application: {e.Message}");
                    }
                }
                else
                {
                    var rootPath = IntrospectionProperties.CurrentEntryPath;

                    try
                    {
                        sources.AddDirectory(rootPath);
                    }
                    catch (Exception e)
                    {
                        Report.Warn($"Failed to enumerate assemblies in '{rootPath}': {e.Message}");
                    }
                }
            }
            catch (Exception e)
            {
                Report.Warn($"Error while locating plugin assemblies: {e}");
            }

            return sources;
        }

        private static bool IsPlugin(AssemblySource source, PluginCache oldCache, PluginCache newCache)
        {
            var ext = Path.GetExtension(source.Path).ToLowerInvariant();
            if (ext != ".dll" &&  ext != ".exe") return false;

            var name = Path.GetFileNameWithoutExtension(source.Path);
            if (!IntrospectionProperties.AssemblyFilter(name))
            {
                Report.Line(4, $"[IsPlugin] Ignoring assembly {name} due to filter");
                return false;
            }

            bool isPlugin = false;
            bool exists = oldCache.TryGetValue(source.Path, out PluginCache.Data cacheValue);

            if (exists && source.LastModified <= cacheValue.LastModified)
            {
                Report.Line(4, $"[IsPlugin] Cache found for: {source.Path}");
                isPlugin = cacheValue.IsPlugin;
            }
            else
            {
                if (exists)
                    Report.Line(4, $"[IsPlugin] Retrying to load because cache is outdated: {source.Path}");
                else
                    Report.Line(4, $"[IsPlugin] Retrying to load because not in cache: {source.Path}");

                try
                {
                    using var s = source.OpenRead();
                    isPlugin = ProbeForPlugin(s);
                }
                catch (Exception e)
                {
                    Report.Line(4, $"[IsPlugin] Error while probing assembly '{source.Path}': {e.Message}");
                }
            }

            if (isPlugin) Report.Line(3, $"[IsPlugin] Plugin found: {source.Path}");
            newCache[source.Path] = new PluginCache.Data(source.LastModified, isPlugin);
            return isPlugin;
        }

        public static List<Assembly> LoadPlugins()
        {
            var oldCache = ReadCacheFile();
            var newCache = new PluginCache();

            using var sources = FindAssemblySources();
            List<Assembly> assemblies = new ();

            try
            {
                foreach (var source in sources)
                {
                    try
                    {
                        if (!IsPlugin(source, oldCache, newCache))
                            continue;

                        var asm = source.Load();
                        assemblies.Add(asm);
                    }
                    catch (Exception e)
                    {
                        Report.Warn($"Failed to load plugin assembly '{source.Path}': {e.Message}");
                    }
                }
            }
            finally
            {
                WriteCacheFile(newCache);
            }

            return assemblies;
        }

        #region LdConfig


        private static class LdConfig
        {
            static readonly Regex rx = new Regex(@"[ \t]*(?<name>[^ \t]+)[ \t]+\((?<libc>[^,]+)\,(?<arch>[^,\)]+)[^\)]*\)[ \t]*\=\>[ \t]*(?<path>.*)");
            static readonly Dictionary<string, string> result = new Dictionary<string, string>();
            static bool loaded = false;

            static void Load()
            {
                string myArch;
                switch (RuntimeInformation.ProcessArchitecture)
                {
                    case Architecture.X86:
                        myArch = "x86";
                        break;
                    case Architecture.X64:
                        myArch = "x86-64";
                        break;
                    case Architecture.Arm:
                        myArch = "arm";
                        break;
                    case Architecture.Arm64:
                        myArch = "arm-64";
                        break;
                    default:
                        myArch = "unknown";
                        break;
                }
                var info = new ProcessStartInfo("/bin/sh", "-c \"ldconfig -p\"")
                {
                    RedirectStandardOutput = true,
                    UseShellExecute = false
                };

                using (var proc = Process.Start(info))
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

            public static Dictionary<string, string> Paths
            {
                get
                {
                    lock(result)
                    {
                        if(!loaded)
                        {
                            Load();
                            loaded = true;
                        }
                        return result;
                    }
                }
            }

            public static bool TryGetPath(string name, out string path)
            {
                return Paths.TryGetValue(name, out path);
            }

        }

        #endregion

        #region DllMap

        private enum OS
        {
            Unknown = 0,
            Win32 = 1,
            Linux = 2,
            MacOS = 3
        }

        private static bool TryParseOS(string os, out OS value)
        {
            os = os.ToLower();
            if (os == "win" || os == "windows" || os == "win32" || os == "win64")
            {
                value = OS.Win32;
                return true;
            }
            else if (os == "linux" || os == "nix" || os == "unix")
            {
                value = OS.Linux;
                return true;
            }
            else if (os == "mac" || os == "macos" || os == "macosx")
            {
                value = OS.MacOS;
                return true;
            }
            else
            {
                value = OS.Unknown;
                return false;
            }

        }

        private static OS GetOS()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) return OS.Win32;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) return OS.MacOS;
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux)) return OS.Linux;
            else return OS.Unknown;
        }

        private static Dictionary<string, string> GetSymlinks(XDocument document)
        {
            var dict = new Dictionary<string, string>();
            var myOs = GetOS();
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
                            if(myOs == osVal)
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

        [DllImport("libc")]
        private static extern int symlink(string src, string linkName);

        private static void CreateSymlink(string baseDir, string src, string dst)
        {
            var os = GetOS();
            if (os == OS.Win32 || os == OS.Unknown) return;

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

                Report.Line(3, $"Creating symlink {src} -> {targetName}");

                if (File.Exists(linkPath))
                {
                    Report.Line(3, $"Deleting old symlink {src}");
                    File.Delete(linkPath);
                }

                if (symlink(targetName, linkPath) != 0)
                {
                    Report.Warn($"Could not create symlink {src}");
                }
            }
            else
            {
                Report.Warn($"Could not create symlink to {targetName} (does not exist)");
            }
        }

        #endregion

        private static void GetPlatformAndArch(out string platform, out string arch)
        {
            switch (RuntimeInformation.ProcessArchitecture)
            {
                case Architecture.X86:
                    arch = "x86";
                    break;
                case Architecture.X64:
                    arch = "AMD64";
                    break;
                case Architecture.Arm:
                    arch = "ARM";
                    break;
                case Architecture.Arm64:
                    arch = "ARM64";
                    break;
                default:
                    arch = "unknown";
                    break;
            }

            switch (GetOS())
            {
                case OS.Win32:
                    platform = "windows";
                    break;
                case OS.Linux:
                    platform = "linux";
                    break;
                case OS.MacOS:
                    platform = "mac";
                    break;
                default:
                    platform = "unknown";
                    break;
            }
        }

        private static readonly Regex nativeLibraryRx =
            GetOS() switch
            {
                OS.Win32 => new Regex(@"\.(dll|exe)$", RegexOptions.Compiled),
                OS.Linux => new Regex(@"\.so(\.[0-9\-]+)?$", RegexOptions.Compiled),
                OS.MacOS => new Regex(@"\.dylib$", RegexOptions.Compiled),
                _ => null
            };

        /// <summary>
        /// Returns whether the file path appears to be a native library for the current platform.
        /// </summary>
        private static bool IsNativeLibrary(string path)
            => nativeLibraryRx?.IsMatch(path) ?? false;

        /// <summary>
        /// Unpacks and lists native dependencies of the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to unpack.</param>
        /// <param name="outputDir">The output directory for the native dependencies. Defaults to <see cref="IntrospectionProperties.CurrentEntryPath"/> if null or empty.</param>
        /// <returns>An array of unpacked native library names.</returns>
        public static string[] UnpackAndListNativeDependencies(Assembly assembly, string outputDir = null)
        {
            if (assembly.IsDynamic) return [];
            if (outputDir.IsNullOrEmpty()) outputDir = IntrospectionProperties.CurrentEntryPath;

            Report.Begin(3, $"Unpacking native dependencies for {assembly.FullName}");

            try
            {
                try
                {
                    var info = assembly.GetManifestResourceInfo("native.zip");
                    if (info == null)
                    {
                        Report.Line(3, $"Assembly does not contain native dependencies.");
                        return [];
                    }

                    GetPlatformAndArch(out string platform, out string arch);

                    string[] copyPaths = [ $"{platform}/{arch}/", $"{arch}/" ];
                    List<string> libNames = [];
                    Dictionary<string, string> remap = [];

                    bool TryGetLocalName(string entryName, out string localName)
                    {
                        foreach(var prefix in copyPaths)
                        {
                            if (entryName.StartsWith(prefix))
                            {
                                var name = entryName.Substring(prefix.Length);
#if NET8_0_OR_GREATER
                                var parts = name.Split('/', StringSplitOptions.RemoveEmptyEntries);
#else
                                var parts = name.Split(['/'], StringSplitOptions.RemoveEmptyEntries);
#endif

                                if (parts.Length > 0)
                                {
                                    localName = Path.Combine(parts);
                                    return true;
                                }
                            }
                        }

                        localName = null;
                        return false;
                    }

                    using var s = assembly.GetManifestResourceStream("native.zip");
                    using var archive = new ZipArchive(s);

                    foreach (var e in archive.Entries)
                    {
                        var entryName = e.FullName.Replace(Path.DirectorySeparatorChar, '/');

                        if (entryName == "remap.xml")
                        {
                            using var es = e.Open();
                            var doc = XDocument.Load(es);
                            remap = GetSymlinks(doc);
                        }
                        else if (TryGetLocalName(entryName, out var localName))
                        {
                            var dstPath = Path.Combine(outputDir, localName);
                            var dstDir = Path.GetDirectoryName(dstPath);

                            if (!File.Exists(dstDir)) Directory.CreateDirectory(dstDir);

                            if (!File.Exists(dstPath) || FileUtils.GetLastWriteTimeSafe(dstPath) < e.LastWriteTime.UtcDateTime)
                            {
                                Report.Line(3, $"Unpacking {localName}");
                                e.ExtractToFile(dstPath, true);
                            }

                            if (IsNativeLibrary(localName)) libNames.Add(localName);
                        }
                    }

                    foreach (var kvp in remap)
                    {
                        CreateSymlink(outputDir, kvp.Key, kvp.Value);
                        if (IsNativeLibrary(kvp.Key)) libNames.Add(kvp.Key);
                    }

                    return libNames.ToArray();
                }
                catch (Exception e)
                {
                    Report.Warn($"Could not unpack native dependencies for {assembly.FullName}: {e.Message}");
                    return [];
                }
            }
            finally
            {
                Report.End(3);
            }
        }

        /// <summary>
        /// Unpacks native dependencies of the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to unpack.</param>
        /// <param name="outputDir">The output directory for the native dependencies. Defaults to <see cref="IntrospectionProperties.CurrentEntryPath"/> if null or empty.</param>
        public static void UnpackNativeDependencies(Assembly assembly, string outputDir)
            => UnpackAndListNativeDependencies(assembly, outputDir);

        /// <summary>
        /// Unpacks native dependencies of the given assembly to <see cref="IntrospectionProperties.CurrentEntryPath"/>.
        /// </summary>
        /// <param name="assembly">The assembly to unpack.</param>
        public static void UnpackNativeDependencies(Assembly assembly)
            => UnpackAndListNativeDependencies(assembly, null);

        [Obsolete("Use UnpackNativeDependencies instead.")]
        public static void UnpackNativeDependenciesToBaseDir(Assembly a, string baseDir)
            => UnpackNativeDependencies(a, baseDir);

        /// The path native dlls will be extracted to, each either each library to a separate folder or directly to the NativeLibraryPath
        /// directory (depending on the configuration of SeparateLibraryDirectories).
        ///
        /// The default configuration extract native libraries to shared temp path with each library in its unique versioned directory.
        ///
        /// Setting NativeLibraryPath to the application path and SeparateLibraryDirectories to false will result in the "old" style of
        /// loading native libs, that is not compatible if the application does not have write permission to the directory (e.g. ProgramFiles).
        [Obsolete("using explicit native library path is no longer possble https://github.com/aardvark-platform/aardvark.base/issues/64")]
        public static string NativeLibraryPath = Path.Combine(Path.GetTempPath(), "aardvark-native");

        /// Specify if native libraries should be extracted each to its own sub folder or directory to the NativeLibraryPath
        /// NOTE: When using global shared NativeLibraryPath, SeparateLibraryDirectories should not be set to false, as this there might be version conflicts
        public static bool SeparateLibraryDirectories = true;

        private static readonly Lazy<string> s_nativeLibraryCacheDirectory =
            new(() => Path.Combine(CachingProperties.CacheDirectory, "Native"));

        public static readonly string NativeLibraryCacheDirectory = s_nativeLibraryCacheDirectory.Value;

        private static readonly Dictionary<Assembly, string> s_nativePaths = new Dictionary<Assembly, string>();
        private static string[] s_allPaths = null;

        public static string[] GetNativeLibraryPaths()
        {
            lock(s_nativePaths)
            {
                s_allPaths ??= s_nativePaths.Values.Where(Directory.Exists).ToArray();
                return s_allPaths;
            }
        }

        public static bool TryGetNativeLibraryPath(Assembly assembly, out string path)
        {
            if (assembly.IsDynamic) { path = null; return false; }

            lock (s_nativePaths)
            {
                if (s_nativePaths.TryGetValue(assembly, out path))
                {
                    if (path == null) return false;
                    else return true;
                }
                else
                {
                    var info = assembly.GetManifestResourceInfo("native.zip");
                    if (info == null)
                    {
                        s_nativePaths[assembly] = null;
                        return false;
                    }
                    else
                    {
                        using var s = assembly.GetManifestResourceStream("native.zip");
                        string dstFolder = NativeLibraryCacheDirectory;

                        if (SeparateLibraryDirectories)
                        {
#if NET8_0_OR_GREATER
                            var hash = SHA1.HashData(s);
                            var guid = new Guid(hash.AsSpan(0, 16));
#else
                            using var sha1 = SHA1.Create();
                            var hash = sha1.ComputeHash(s);
                            Array.Resize(ref hash, 16);
                            var guid = new Guid(hash);
#endif

                            GetPlatformAndArch(out var platform, out var arch);
                            dstFolder = Path.Combine(dstFolder, assembly.GetName().Name, guid.ToString(), platform, arch);
                        }

                        s_nativePaths[assembly] = dstFolder;
                        s_allPaths = null;
                        path = dstFolder;
                        return true;
                    }
                }
            }
        }

        private static readonly Dictionary<(Assembly, string), string> s_cache = new Dictionary<(Assembly, string), string>();

        private static bool TryLoadNativeLibrary(string path, out IntPtr handle)
        {
#if NETCOREAPP3_1_OR_GREATER
            return NativeLibrary.TryLoad(path, out handle);
#else
            handle = (GetOS() == OS.Win32) ? Kernel32.LoadLibrary(path) : Dl.dlopen(path, 1);
            return handle != IntPtr.Zero;
#endif
        }

        private static bool TryLoadNativeLibrary(Assembly assembly, string name, out IntPtr handle)
        {
#if NETCOREAPP3_1_OR_GREATER
            return (assembly != null)
                ? NativeLibrary.TryLoad(name, assembly, null, out handle)
                : NativeLibrary.TryLoad(name, out handle);
#else
            return TryLoadNativeLibrary(name, out handle);
#endif
        }

        public static IntPtr LoadLibrary(Assembly assembly, string nativeName)
        {
            var os = GetOS();
            lock (s_cache)
            {
                if(s_cache.TryGetValue((assembly, nativeName), out var path))
                {
                    return TryLoadNativeLibrary(path, out var handle) ? handle : IntPtr.Zero;
                }
            }

            IntPtr ptr = IntPtr.Zero;
            string probe = null;
            var paths = new List<string>(capacity: 1);

            try
            {
                var location = Path.GetDirectoryName(assembly?.GetLocationSafe()) ?? IntrospectionProperties.CurrentEntryPath;
                if (location != null)
                    paths.Add(location);
            }
            catch
            {
            }

            try
            {
                string[] formats = [];

                if (os == OS.Linux) formats = ["{0}.so", "lib{0}.so", "lib{0}.so.1"];
                else if (os == OS.Win32) formats = ["{0}.dll"];
                else if (os == OS.MacOS) formats = ["{0}.dylib", "lib{0}.dylib"];

                if (assembly != null)
                {
                    if (TryGetNativeLibraryPath(assembly, out var dstFolder))
                    {
                        paths.Add(dstFolder);
                    }
                    else
                    {
                        paths.AddRange(GetNativeLibraryPaths());
                    }
                }
                else
                {
                    paths.AddRange(GetNativeLibraryPaths());
                }

                var realName = Path.GetFileNameWithoutExtension(nativeName);
                Report.Begin(4, "probing paths for {0}", realName);
                foreach(var path in paths)
                {
                    Report.Line(4, "{0}", path);
                }
                Report.End(4);

                // NOTE: IPP will try to load other IPP native libs internally (not triggering the ResolvingUnmanagedDll callback) and show a message box if it fails -> make sure native dependencies between them can be resolved
                //        - for some reason SetDllDirectory is required even LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR is used that will include the directory of the library that should be loaded
                //        - for some other reason AddDllDirectory does not help loading libraries, even it should provide similar behavior than SetDllDirectory
                // NOTE++: We don't use IPP anymore, this was broken anyway because the assembly name was not "IPP" anymore for some reason.
                //var setDllDir = os == OS.Win32 && assembly != null && assembly.GetName().Name == "IPP";

                foreach (var fmt in formats)
                {
                    var libName = string.Format(fmt, realName);

                    // probe all paths.
                    foreach (var p in paths)
                    {
                        if (!Directory.Exists(p))
                            continue;

                        if (FileUtils.TryFindInDirectory(p, libName, out var libPath))
                        {
                            probe = libPath;
                            if (TryLoadNativeLibrary(libPath, out ptr)) return ptr;
                        }
                    }

                    // try the plain loading mechanism.
                    probe = libName;
                    if (TryLoadNativeLibrary(assembly, libName, out ptr)) return ptr;
                }

                // try the standard loading mechanism as a last resort.
                probe = nativeName;
                return TryLoadNativeLibrary(assembly, nativeName, out ptr) ? ptr : IntPtr.Zero;
            }
            catch (Exception e)
            {
                Report.Warn($"Could not load native library {nativeName}: {e.Message}");
                return IntPtr.Zero;
            }
            finally
            {
                if (ptr == IntPtr.Zero)
                {
                    Report.Warn($"Could not load native library {nativeName} ({probe})");
                }
                else
                {
                    lock (s_cache) { s_cache[(assembly, nativeName)] = probe; }
                    Report.Line(4, "[Introspection] loaded native library {0} from {1}", nativeName, probe);
                }
            }
        }

        public static IntPtr GetProcAddress(IntPtr handle, string name)
        {
            if (handle == IntPtr.Zero) return IntPtr.Zero;

#if NETCOREAPP3_1_OR_GREATER
            IntPtr ptr;
            if (NativeLibrary.TryGetExport(handle, name, out ptr)) return ptr;
            else return IntPtr.Zero;
#else
            var os = GetOS();
            if (os == OS.Win32) return Kernel32.GetProcAddress(handle, name);
            else return Dl.dlsym(handle, name);
#endif
        }

        public static void LoadNativeDependencies(Assembly a)
        {
            if (TryGetNativeLibraryPath(a, out string dstFolder))
            {
                try
                {
                    Report.BeginTimed(3, $"Loading native dependencies for {a.FullName}");
                    try
                    {
                        var libNames = UnpackAndListNativeDependencies(a, dstFolder);

#if NETCOREAPP3_1_OR_GREATER
                        NativeLibrary.SetDllImportResolver(a, (name, ass, searchPath) =>
                        {
                            return LoadLibrary(ass, name);
                        });
#else
                        var isWindows = GetOS() == OS.Win32;

                        // NOTE: libraries such as IPP are spread over multiple dlls that will now get loaded in an nondeterministic order -> make sure native dependencies between them can be resolved
                        //        - for some reason SetDllDirectory is required even LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR is used that will include the directory of the library that should be loaded
                        //        - for some other reason AddDllDirectory does not help loading libraries, even it should provide similar behavior than SetDllDirectory
                        if (isWindows)
                            Kernel32.SetDllDirectory(dstFolder);

                        foreach (var file in libNames)
                        {
                            try
                            {
                                var ptr = IntPtr.Zero;
                                var filePath = Path.Combine(dstFolder, file);
                                if (isWindows)
                                    ptr = Kernel32.LoadLibraryEx(filePath, IntPtr.Zero, Kernel32.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | Kernel32.LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
                                else
                                    ptr = Dl.dlopen(filePath, 0x01102);

                                if (ptr == IntPtr.Zero)
                                {
                                    if (isWindows)
                                    {
                                        var err = Kernel32.GetLastError();
                                        Report.Warn($"Could not load native library {filePath} (Error = {err})");
                                    }
                                    else
                                    {
                                        Report.Warn($"Could not load native library {filePath}");
                                    }
                                }
                                else
                                {
                                    Report.Line(3, $"Loaded {file}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Report.Warn($"Could not load native library {file}: {ex.Message}");
                            }
                        }
#endif
                    }
                    finally
                    {
                        Report.End(3);
                    }
                }
                catch (Exception e)
                {
                    Report.Warn($"Could not load native dependencies for {a.FullName}: {e.Message}");
                }
            }
        }

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

        private static bool isInitialized = false;

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
            var platform = GetOS();
            if (platform == OS.Win32)
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
                Report.BeginTimed("Loading plugins");
                var pluginsList = LoadPlugins();
                Report.End();

                LoadAll(pluginsList);
            }

            Report.End();

            isInitialized = true;
        }

        private static void LoadAll(IEnumerable<Assembly> xs)
        {
            var assemblies = Enumerable.Concat(Introspection.AllAssemblies, xs).GroupBy(a => a.FullName).Select(x => x.First()).ToArray();

            foreach (var ass in assemblies)
            {
                Report.BeginTimed(10, "GetAllMethodsWithAttribute: {0}", String.IsNullOrEmpty(ass.FullName) ? "assembly with no name" : ass.FullName);
                var initMethods = Introspection.GetAllMethodsWithAttribute<OnAardvarkInitAttribute>(ass).Select(t => t.Item1).Distinct().ToArray();
                Report.EndTimed(10);

                foreach (var mi in initMethods)
                {
                    var parameters = mi.GetParameters();

                    Report.BeginTimed($"Initializing {mi.DeclaringType.Name}");

                    try
                    {
                        if (parameters.Length == 0)
                            mi.Invoke(null, null);
                        else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IEnumerable<Assembly>))
                            mi.Invoke(null, new object[] { Introspection.AllAssemblies });
                        else
                            Report.Warn("Strange aardvark init method found: {0}, should be Init : IEnumberable<Assembly> -> unit or unit -> unit", mi);
                    }
                    catch (Exception e)
                    {
                        Report.Warn("Failed: {0}", e);
                    }
                    finally
                    {
                        Report.End();
                    }
                }
            }
        }
    }
}
