using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Aardvark.Base
{
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
                NamingScheme.Timestamp => IntrospectionProperties.GetLastWriteTimeUtc(asm).ToBinary().ToString(),
                _ => ""
            };
        }
    }

    public static class IntrospectionProperties
    {
        /// <summary>
        /// Introspection is based on Assembly.GetEntryAssembly which represents the managed 
        /// entry point (i.e. the first assembly that was executed by AppDomain.ExecuteAssembly).
        /// However, startet from an unmanged entry point (like VisualStudio tests) Assembly.GetEntryAssembly
        /// is null. To allow us to start from unmanaged hosting processes this alternative
        /// has been implented.
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


        public static string CurrentEntryPath => (CurrentEntryAssembly != null)
            ? Path.GetDirectoryName(CurrentEntryAssembly.Location)
            : null
            ;

        private static HashSet<string> s_defaultAssemblyBlacklist =
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
                    "Ceres",
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

        public static string BundleEntryPoint = "";

        /// <summary>
        /// Robustly tries to get DateTime also for bundled deployments. Use BundleEntryPoint to register dummy entry as a first fallback. returns DateTime.Now if all fallbacks fail.
        /// </summary>
        public static Func<Assembly, DateTime> GetLastWriteTimeUtc = (Assembly assembly) =>
        {
            if (!String.IsNullOrEmpty(assembly.Location) && File.Exists(assembly.Location)) // first choise - won't work for bundled deplyoments?
            {
                return File.GetLastWriteTimeUtc(assembly.Location);

            }
            else if (File.Exists(BundleEntryPoint)) // fallback 1 - use bundle entrypoint
            {
                return File.GetLastWriteTimeUtc(BundleEntryPoint);
            }
            else
            {
                // no option left.... 
                return DateTime.Now;
            }
        };
    }

    public static class Introspection
    {
        private static CultureInfo s_cultureInfoEnUs = new CultureInfo("en-us");
        private static Dictionary<string, Assembly> s_assemblies;
        private static HashSet<string> s_assembliesThatFailedToLoad = new HashSet<string>();
        private static HashSet<Assembly> s_allAssemblies = new HashSet<Assembly>();

        private static string InitializeCacheDirectory()
        {
            var path = Path.Combine(CachingProperties.CacheDirectory, "Introspection");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        private static Lazy<string> s_cacheDirectory = new Lazy<string>(InitializeCacheDirectory);

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
                Report.Warn(e.ToString());
            }
            return new T[0];
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
                           let attribs = m.GetCustomAttributes(typeof(T), false)
                           where attribs.Length > 0
                           select (m, attribs.Select(x => (T)x).ToArray()),
                  types => from t in types
                           where t != null
                           from m in t.GetMethods()
                           let attribs = m.GetCustomAttributes(typeof(T), false)
                           where attribs.Length > 0
                           select (m, attribs.Select(x => (T)x).ToArray()),
                  result => result.Select(m => m.Item1.DeclaringType.AssemblyQualifiedName)
                  );
        
        static Introspection()
        {
            // enumerating all assemblies reachable from entry assembly
            s_assemblies = new Dictionary<string, Assembly>();
            var entryAssembly = Assembly.GetEntryAssembly();

            if (Assembly.GetEntryAssembly() == null
               && IntrospectionProperties.CurrentEntryAssembly == null
               && typeof(Aardvark).Assembly != null)
            {
                if (typeof(Aardvark).Assembly != null)
                {
                    Report.Warn("Assembly.GetEntryAssembly() == null && IntrospectionProperties.CurrentEntryAssembly == null. This might be due to nunit like setups. trying to use typeof<Aardvark>.Assembly instead: {0}", typeof(Aardvark).Assembly.Location);
                    IntrospectionProperties.CustomEntryAssembly = typeof(Aardvark).Assembly;
                    RegisterAllAssembliesInCustomEntryPath();
                } else
                {
                    Report.Warn("All is null!!. Assembly.GetEntryAssembly(), IntrospectionProperties.CurrentEntryAssembly, typeof(Aardvark).Assembly. Aardvark stuff, based on introspection will most likely fail due to missing entry points. Optimistically continuing for now...");
                }
            }

            // hs: Assembly.GetEntryAssembly() returns null if started from unmanged hosting process (like visualstudio)
            // use CustomEntryAssembly assembly in this cases.
            // prior to rev. 17705 CustomEntryAssembly was only used if entryAssembly is null. However this precludes
            // hosting processes from using custom root assemblies (see commend in IntrospectionProperties.CustomEntryAssembly)
            // if this breaks your code just do not set IntrospectionProperties.CustomEntryAssembly or contact me (hs)
            if (IntrospectionProperties.CustomEntryAssembly != null)
            {
                Report.Warn("[Introspection] Assembly.GetEntryAssembly() was 0 but a CustomEntryAssembly was supplied.");
                EnumerateAssemblies(IntrospectionProperties.CustomEntryAssembly.GetName().Name, IntrospectionProperties.CustomEntryAssembly);
            } else if (entryAssembly != null)
            {
                var entryAssemblyName = entryAssembly.GetName();
                EnumerateAssemblies(entryAssemblyName.Name);
            }
            else
            {
                RegisterAllAssembliesInCustomEntryPath();
            }
            
            //Report.Line("s_telemetryEnumerateAssembliesCheckTime1: {0}", s_telemetryEnumerateAssembliesCheckTime1.Value);
            //Report.Line("s_telemetryEnumerateAssembliesCheckTime2: {0}", s_telemetryEnumerateAssembliesCheckTime2.Value);
        }

        private static void RegisterAllAssembliesInCustomEntryPath()
        {
            Report.Warn("[Introspection] Assembly.GetEntryAssembly() == null");
            // hs: why is this necessary here? bootstrapper should be initialized before anything is loaded?
            // sm: so let's see who complains if we comment it out ;-)
            //Bootstrapper.Init();

            RegisterAllAssembliesInPath(IntrospectionProperties.CurrentEntryPath);
        }

        /// <summary>
        /// Tries to load and register all assemblies in given path.
        /// </summary>
        [DebuggerNonUserCode]
        public static void RegisterAllAssembliesInPath(string path, bool verbose)
        {
            if (verbose) Report.Begin("[Introspection] registering all assemblies in {0}", path);
            var files = Directory.GetFiles(path, "*.dll").Concat(Directory.GetFiles(path, "*.exe"));
            foreach (var file in files)
            {
                try
                {
                    EnumerateAssemblies(AssemblyName.GetAssemblyName(file).Name);
                    if (verbose) Report.Line("{0}", Path.GetFileName(file));
                }
                catch
                {
                }
            }
            if (verbose) Report.End();
        }

        /// <summary>
        /// Tries to load and register all assemblies in given path.
        /// </summary>
        [DebuggerNonUserCode]
        public static void RegisterAllAssembliesInPath(string path) => RegisterAllAssembliesInPath(path, false);

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
            var name = Path.GetFileName(asm.Location);
            var id = asm.GetIdentifier(CachingProperties.IntrospectionCacheFileNaming);
            var fname = string.Format("{0}_{1}_{2}.txt", name, id, queryGuid);
            return Path.Combine(CacheDirectory, fname);
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
                assemblyTimeStamp = IntrospectionProperties.GetLastWriteTimeUtc(a);

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
            Type[] ts = new Type[0];
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

    [Serializable]
    public class Aardvark
    {
        private static string InitializeCacheDirectory()
        {
            var path = Path.Combine(CachingProperties.CacheDirectory, "Plugins");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        }

        private static Lazy<string> s_cacheDirectory = new Lazy<string>(InitializeCacheDirectory);

        /// <summary>
        /// Returns the directory of the plugins cache files.
        /// </summary>
        public static string CacheDirectory => s_cacheDirectory.Value;

        public string CacheFile = string.Empty;

        public Aardvark()
        {
            Assembly asm = IntrospectionProperties.CurrentEntryAssembly;
            string entryAssemblyName = asm?.GetName().Name ?? "unknown";
            string entryAssemblyId = asm?.GetIdentifier(CachingProperties.PluginsCacheFileNaming) ?? "unknown";
            string fileName = string.Format("{0}_{1}_plugins.bin", entryAssemblyName, entryAssemblyId);

            CacheFile = Path.Combine(CacheDirectory, fileName);
        }

        private Dictionary<string, Tuple<DateTime, bool>> ReadCacheFile()
        {
            if (File.Exists(CacheFile))
            {
                var formatter = new BinaryFormatter();

                try
                {
                    using (var stream = new FileStream(CacheFile, FileMode.Open))
                    {
                        var result = (Dictionary<string, Tuple<DateTime, bool>>)formatter.Deserialize(stream);
                        Report.Line(3, "[ReadCacheFile] loaded cache file: {0}", CacheFile);
                        return result;
                    }
                }
                catch (Exception e)
                {
                    Report.Line(3, "[ReadCacheFile] could not load cache file: {0} with {1}", CacheFile, e.Message);
                    return new Dictionary<string, Tuple<DateTime, bool>>();
                }
            }
            else
            {
                Report.Line(3, "[ReadCacheFile] no plugins cache file found at {0}", CacheFile);
                return new Dictionary<string, Tuple<DateTime, bool>>();
            }
        }

        private void WriteCacheFile(Dictionary<string, Tuple<DateTime, bool>> cache)
        {
            if (string.IsNullOrEmpty(CacheFile))
            {
                Report.Warn("Could not write cache file since CacheFile was null or empty");
            }
            else
            {
                try
                {
                    if (File.Exists(CacheFile)) File.Delete(CacheFile);

                    var formatter = new BinaryFormatter();
                    using (var stream = new FileStream(CacheFile, FileMode.CreateNew))
                    {
                        formatter.Serialize(stream, cache);
                    }
                } catch(Exception ex)
                {
                    Report.Warn("Could not write cache file: {0}", ex.Message);
                }
            }
        }

        private static Regex versionRx = new Regex(@"^[ \t]*(?<name>[\.A-Za-z_0-9]+)[ \t]*,[ \t]*(v|V)ersion[ \t]*=[ \t]*(?<version>[\.A-Za-z_0-9]+)$");

        private static unsafe bool IsPlugin(string file)
        {
            try
            {
                using (var s = File.OpenRead(file))
                using (var v = new System.Reflection.PortableExecutable.PEReader(s))
                {
                    if (v.PEHeaders.CorHeader == null || !v.HasMetadata) return false;
                    var data = v.GetMetadata();
                    var m = new System.Reflection.Metadata.MetadataReader(data.Pointer, data.Length);


                    var assdef = m.GetAssemblyDefinition();
                    foreach (var att in assdef.GetCustomAttributes())
                    {
                        var attDef = m.GetCustomAttribute(att);
                        if (attDef.Constructor.Kind == System.Reflection.Metadata.HandleKind.MemberReference)
                        {
                            var hh = (System.Reflection.Metadata.MemberReferenceHandle)attDef.Constructor;
                            var e = m.GetMemberReference(hh);
                            var pp = e.Parent;
                            if (pp.Kind == System.Reflection.Metadata.HandleKind.TypeReference)
                            {
                                var attType = m.GetTypeReference((System.Reflection.Metadata.TypeReferenceHandle)pp);
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
                                    if (attDef.Constructor.Kind == System.Reflection.Metadata.HandleKind.MemberReference)
                                    {
                                        var hh = (System.Reflection.Metadata.MemberReferenceHandle)attDef.Constructor;
                                        var e = m.GetMemberReference(hh);
                                        var pp = e.Parent;
                                        if (pp.Kind == System.Reflection.Metadata.HandleKind.TypeReference)
                                        {
                                            var attType = m.GetTypeReference((System.Reflection.Metadata.TypeReferenceHandle)pp);
                                            var nameStr = m.GetString(attType.Name);
                                            var nsStr = m.GetString(attType.Namespace);
                                            if (nsStr == "Aardvark.Base" && nameStr == "OnAardvarkInitAttribute") 
                                            {
                                                return true;
                                            }
                                            else return false;
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
            }
            catch(Exception)
            {
                Report.Warn("NO PLUGIN: {0}", file);
                return false;
            }
        }

        public string[] GetPluginAssemblyPaths()
        {

            var cache = ReadCacheFile();
            var newCache = new Dictionary<string, Tuple<DateTime, bool>>();

            var folder = new string[0];

            // attach folder contents if possilbe (e.g. not possible in bundle deployments if no bundle entry is specified)
            if (IntrospectionProperties.CurrentEntryPath != null)
                folder = Directory.EnumerateFiles(IntrospectionProperties.CurrentEntryPath).ToArray();
            else if (IntrospectionProperties.BundleEntryPoint != null)
                folder = Directory.EnumerateFiles(Path.GetDirectoryName(IntrospectionProperties.BundleEntryPoint)).ToArray();

            string[] assemblies =
                    folder
                        .Where(p => { var ext = Path.GetExtension(p).ToLowerInvariant(); return ext == ".dll" || ext == ".exe"; })
                        .Where(p => {
                            var name = Path.GetFileNameWithoutExtension(p);
                            var f = IntrospectionProperties.AssemblyFilter(name);
                            if (!f) { Report.Line(4, "[GetPluginAssemblyPaths] Ignoring assembly {0} due to filter", name); }
                            return f;
                        })
                        .Select(Path.GetFullPath)
                        .ToArray();

            var paths = new List<string>();

            foreach (var fileName in assemblies)
            {
                var lastWrite = DateTime.MaxValue;
                try { lastWrite = File.GetLastWriteTimeUtc(fileName); }
                catch(Exception)
                {
                    Report.Line(3, "[GetPluginAssemblyPaths] could not get write time for: {0}", fileName);
                }

                Tuple<DateTime, bool> cacheValue;
                if (cache.TryGetValue(fileName, out cacheValue) && lastWrite <= cacheValue.Item1)
                {
                    Report.Line(3, "[GetPluginAssemblyPaths] cache found for: {0}", fileName);
                    if (cacheValue.Item2)
                    {
                        newCache[fileName] = Tuple.Create(lastWrite, true);
                        paths.Add(fileName);
                    }
                    else
                    {
                        newCache[fileName] = Tuple.Create(lastWrite, false);
                    }
                }
                else
                {
                    
                    if (cacheValue != null && cacheValue.Item1 > lastWrite)
                        Report.Line(3, "[GetPluginAssemblyPaths] retrying to load because cache outdated {0}", fileName);
                    else
                        Report.Line(3, "[GetPluginAssemblyPaths] retrying to load because not in cache {0}", fileName);

                    if (IsPlugin(fileName))
                    {
                        Report.Line(3, "[GetPluginAssemblyPaths] plugin found {0}", fileName);
                        newCache[fileName] = Tuple.Create(lastWrite, true);
                        paths.Add(fileName);
                    }
                    else
                    {
                        newCache[fileName] = Tuple.Create(lastWrite, false);
                    }
                }
            }


            WriteCacheFile(newCache);
            return paths.ToArray();
        }


        public static List<Assembly> LoadPlugins()
        {
            //Note: I removed the separate AppDomain for Plugin probing because:
            //1) it made problems on startup in some setups
            //2) the code below seemed to not do anything in the new AppDomain since the call 
            //   var paths = aardvark.GetPluginAssemblyPaths();
            //   was actually executed in this AppDomain.
            //Changes are marked with APPD


            //APPD var setup = new AppDomainSetup();
            //APPD setup.ApplicationBase = IntrospectionProperties.CurrentEntryPath;

            try
            {
                //APPD var d = AppDomain.CreateDomain(Guid.NewGuid().ToString(), null, setup);
                var aardvark = new Aardvark(); //APPD (Aardvark)d.CreateInstanceAndUnwrap(typeof(Aardvark).Assembly.FullName, typeof(Aardvark).FullName);

                Report.Line(3, "[LoadPlugins] Using plugin cache file name: {0}", aardvark.CacheFile);
                var paths = aardvark.GetPluginAssemblyPaths();
                //APPD AppDomain.Unload(d);


                var assemblies = new List<Assembly>();

                foreach (var p in paths)
                {

                    try
                    {
                        var ass = Assembly.LoadFile(p);
                        assemblies.Add(ass);
                    }
                    catch (Exception e)
                    {
                        Report.Line(3, "[LoadPlugins] Could not load assembly: {0}", e.Message);
                    }
                }

                return assemblies;
            } catch(Exception e)
            {
                Report.Warn("[LoadPlugins] could not load plugins: {0}", e.Message);
                return new List<Assembly>();
            }
        }


#region LdConfig


        private static class LdConfig
        {
            static Regex rx = new Regex(@"[ \t]*(?<name>[^ \t]+)[ \t]+\((?<libc>[^,]+)\,(?<arch>[^\)]+)\)[ \t]*\=\>[ \t]*(?<path>.*)");
            static Dictionary<string, string> result = new Dictionary<string, string>();
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


        [DllImport("kernel32")]
        private static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int flags);

        private static void CreateSymlink(string baseDir, string src, string dst)
        {
            var os = GetOS();
            if (os == OS.Win32 || os == OS.Unknown) return;

            string targetName;
            string targetPath;
            if (LdConfig.TryGetPath(dst, out targetPath))
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

                Report.Line(3, "creating symlink {0} -> {1}", src, targetName);
                if (File.Exists(linkPath))
                {
                    Report.Line(3, "deleting old symlink {0}", src);
                    File.Delete(linkPath);
                }

                if (symlink(targetName, linkPath) != 0)
                {
                    Report.Warn("could not create symlink {0}", src);
                }
            }
            else
            {
                Report.Warn("could not create symlink to {0} (does not exist)", targetName);
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

        public static void UnpackNativeDependenciesToBaseDir(Assembly a, string baseDir)
        {
            if (a.IsDynamic) return;

            try
            {
                var symlinks = new Dictionary<string, string>();
                var info = a.GetManifestResourceInfo("native.zip");
                if (info == null) return;

                Report.Begin(3, "Unpacking native dependencies for {0}", a.FullName);


                using (var s = a.GetManifestResourceStream("native.zip"))
                {
                    using (var archive = new ZipArchive(s))
                    {
                        string arch;
                        string platform;
                        GetPlatformAndArch(out platform, out arch);

                        var copyPaths = platform + "/" + arch;
                        var remapFile = "remap.xml";


                        foreach (var e in archive.Entries)
                        {
                            var name = e.FullName.Replace('\\', '/');

                            if (e.FullName == remapFile)
                            {
                                var doc = System.Xml.Linq.XDocument.Load(e.Open());
                                symlinks = GetSymlinks(doc);
                            }
                            else if (name.StartsWith(copyPaths))
                            {
                                name = name.Substring(copyPaths.Length);
                                var localComponents = name.Split(new[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries);

                                if (localComponents.Length != 0)
                                {
                                    var localTarget = Path.Combine(localComponents);
                                    var outputPath = Path.Combine(baseDir, localTarget);

                                    var d = Path.GetDirectoryName(outputPath);
                                    if (!Directory.Exists(Path.GetDirectoryName(outputPath)))
                                    {
                                        Directory.CreateDirectory(d);
                                    }

                                    if (!File.Exists(outputPath))
                                    {
                                        e.ExtractToFile(outputPath);
                                        Report.Line(3, "unpacked: {0}", outputPath);
                                    }
                                    else if(File.GetLastWriteTimeUtc(outputPath) < e.LastWriteTime.UtcDateTime)
                                    {
                                        e.ExtractToFile(outputPath, true);
                                        Report.Line(3, "unpacked: {0} (outdated)", outputPath);
                                    }
                                }
                            }
                        }
                    }
                }

                var myOS = GetOS();
                foreach(var kvp in symlinks)
                {
                    var linkName = kvp.Key;
                    if (myOS == OS.Win32)
                    {
                        Report.Warn("could not create symlink {0}", linkName);
                        //if (!CreateSymbolicLink(kvp.Key, kvp.Value, 0x2))
                        //{
                        //    Report.Warn("could not create symlink {0}", kvp.Key);
                        //}
                    }
                    else
                    {
                        string targetName;
                        string targetPath;
                        if(LdConfig.TryGetPath(kvp.Value, out targetPath))
                        {
                            targetName = targetPath;
                        }
                        else {
                            targetName = kvp.Value;
                            targetPath = Path.Combine(baseDir, targetName);
                        }

                        if (File.Exists(targetPath))
                        {
                            var linkPath = Path.Combine(baseDir, linkName);

                            Report.Line(3, "creating symlink {0} -> {1}", linkName, targetName);
                            if (File.Exists(linkPath))
                            {
                                Report.Line(3, "deleting old symlink {0}", linkName);
                                File.Delete(linkPath);
                            }

                            if (symlink(targetName, linkPath) != 0)
                            {
                                Report.Warn("could not create symlink {0}", linkName);
                            }
                        }
                        else
                        {
                            Report.Warn("could not create symlink to {0} (does not exist)", targetName);
                        }

                    }
                }


                Report.End(3);
            }
            catch (Exception e)
            {
                Report.Warn("could not unpack native dependencies for {0}: {1}", a.FullName, e);
                Report.End(3);
            }
        }

        public static void UnpackNativeDependencies(Assembly a)
        {
            var baseDir =
                IntrospectionProperties.CustomEntryAssembly != null
                ? Path.GetDirectoryName(IntrospectionProperties.CustomEntryAssembly.Location)
                : AppDomain.CurrentDomain.BaseDirectory;
            UnpackNativeDependenciesToBaseDir(a,baseDir);
        }

        private static Regex soRx = new Regex(@"\.so(\.[0-9\-]+)?$");
        private static Regex dllRx = new Regex(@"\.(dll|exe)$");
        private static Regex dylibRx = new Regex(@"\.dylib$");

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

        private static Dictionary<Assembly, string>  s_nativePaths = new Dictionary<Assembly, string>();
        private static string[] s_allPaths = null;

        public static string[] GetNativeLibraryPaths()
        {
            lock(s_nativePaths)
            {
                if(s_allPaths == null) s_allPaths = s_nativePaths.Values.Where(p => p != null).ToArray();
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
                        using (var s = assembly.GetManifestResourceStream("native.zip"))
                        {
#pragma warning disable CS0618 // Type or member is obsolete
                            string dstFolder = NativeLibraryPath;
                            if (SeparateLibraryDirectories)
                            {
                                var md5 = System.Security.Cryptography.MD5.Create();
                                var hash = new Guid(md5.ComputeHash(s));
                                md5.Dispose();
                                var bits = IntPtr.Size * 8;
                                var folderName = string.Format("{0}-{1}-{2}", assembly.GetName().Name, hash.ToString(), bits);
                                dstFolder = Path.Combine(NativeLibraryPath, folderName);
                            }
#pragma warning restore CS0618 // Type or member is obsolete
                            s_nativePaths[assembly] = dstFolder;
                            s_allPaths = null;
                            path = dstFolder;
                            return true;
                        }
                    }
                }
            }
        }

        private static Dictionary<(Assembly, string), string> s_cache = new Dictionary<(Assembly, string), string>();

        public static IntPtr LoadLibrary(Assembly assembly, string nativeName)
        {
            var os = GetOS();
            lock (s_cache)
            {
                if(s_cache.TryGetValue((assembly, nativeName), out var path))
                {
#if NETCOREAPP3_1
                    if (NativeLibrary.TryLoad(path, out var pp)) return pp;
                    else return IntPtr.Zero;
#else
                    if (os == OS.Win32) return Kernel32.LoadLibrary(path);
                    else Dl.dlopen(path, 1);
#endif
                }
            }
            IntPtr ptr = IntPtr.Zero;
            string probe = Environment.CurrentDirectory;
            var nextToAssembly = new string[0];

            if (assembly != null)
            {
                try { nextToAssembly = new[] { Path.GetFullPath(Path.GetDirectoryName(assembly.Location)) }; }
                catch (Exception) { }
            }
            else
            {
                try
                {
                    var entry = IntrospectionProperties.CustomEntryAssembly != null ? IntrospectionProperties.CustomEntryAssembly : Assembly.GetEntryAssembly();
                    try { nextToAssembly = new[] { Path.GetFullPath(Path.GetDirectoryName(entry.Location)) }; }
                    catch (Exception) { }
                }
                catch { }
            }

            try
            {
                string[] formats = new string[0];

                if (os == OS.Linux) formats = new[] { "{0}.so", "lib{0}.so", "lib{0}.so.1" };
                else if (os == OS.Win32) formats = new[] { "{0}.dll" };
                else if (os == OS.MacOS) formats = new[] { "{0}.dylib", "lib{0}.dylib" };


                string[] paths;
                if (assembly != null)
                {
                    if (TryGetNativeLibraryPath(assembly, out var dstFolder))
                    {
                        paths = new[] { dstFolder };
                    }
                    else
                    {
                        paths = GetNativeLibraryPaths();
                    }
                }
                else
                {
                    paths = GetNativeLibraryPaths();
                }

                paths = nextToAssembly.Concat(paths);
#if NETCOREAPP3_1

                var realName = Path.GetFileNameWithoutExtension(nativeName);
                Report.Begin(4, "probing paths for {0}", realName);
                foreach(var path in paths)
                {
                    Report.Line(4, "{0}", path);
                }
                Report.End(4);


                if (os == OS.Linux && realName.ToLower() == "devil") formats = new[] { "libIL.so" }.Concat(formats);

                foreach (var fmt in formats)
                {
                    var libName = string.Format(fmt, realName);

                    // probe all paths.
                    foreach (var p in paths)
                    {
                        var lowerLibName = libName.ToLower();
                        var libPath = Directory.GetFiles(p).Where(fp => Path.GetFileName(fp).ToLower() == lowerLibName).FirstOrDefault();
                        if (libPath != null)
                        {
                            probe = libPath;
                            if (NativeLibrary.TryLoad(libPath, out ptr)) return ptr;
                            else Report.Warn("found native library {0} in {1} but it could not be loaded.", Path.GetFileName(libPath), p);
                        }
                    }

                    // try the plain loading mechanism.
                    probe = libName;
                    if (assembly != null && NativeLibrary.TryLoad(libName, assembly, null, out ptr)) return ptr;
                    else if (NativeLibrary.TryLoad(libName, out ptr)) return ptr;

                }

                probe = nativeName;
                // try the standard loading mechanism as a last resort.
                if (assembly != null && NativeLibrary.TryLoad(nativeName, assembly, null, out ptr)) return ptr;
                else if (NativeLibrary.TryLoad(nativeName, out ptr)) return ptr;

                return IntPtr.Zero;
#else

                Func<string, IntPtr> loadLibrary;
                if (os == OS.Win32) loadLibrary = (a) => Kernel32.LoadLibrary(a);
                else loadLibrary = (a) => Dl.dlopen(a, 1);

                ptr = loadLibrary(nativeName);
                if (ptr != IntPtr.Zero) return ptr;

                var realName = Path.GetFileNameWithoutExtension(nativeName);
                foreach (var fmt in formats)
                {
                    var libName = string.Format(fmt, realName);
                    probe = libName;

                    ptr = loadLibrary(libName);
                    if (ptr != IntPtr.Zero) return ptr;

                    foreach (var p in paths)
                    {
                        var libPath = Path.Combine(p, libName);

                        if (File.Exists(libPath))
                        {
                            probe = libPath;
                            ptr = loadLibrary(libPath);
                            if (ptr != IntPtr.Zero) return ptr;
                        }
                    }
                }

                return IntPtr.Zero;
#endif
            }
            finally
            {
                if (ptr == IntPtr.Zero) Report.Line(4, "[Introspection] could not load native library {0}", nativeName);
                else
                {
                    lock(s_cache) { s_cache[(assembly, nativeName)] = probe; }
                    Report.Line(4, "[Introspection] loaded native library {0} from {1}", nativeName, probe);
                }
            }
        }

        public static IntPtr GetProcAddress(IntPtr handle, string name)
        {
            if (handle == IntPtr.Zero) return IntPtr.Zero;

#if NETCOREAPP3_1
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
            string dstFolder;
            if(TryGetNativeLibraryPath(a, out dstFolder))
            {
                try
                {
                    var symlinks = new Dictionary<string, string>();
                    Report.BeginTimed(3, "Loading native dependencies for {0}", a.FullName);
                    try
                    {
                        string arch;
                        string platform;
                        GetPlatformAndArch(out platform, out arch);

                        var copyPaths = new string[] { platform + "/" + arch + "/", arch + "/" };
                        var toLoad = new List<string>();

                        using (var s = a.GetManifestResourceStream("native.zip"))
                        {
                            if (!Directory.Exists(dstFolder)) Directory.CreateDirectory(dstFolder);

                            var extensions = dllRx;
                            if (platform == "linux") extensions = soRx;
                            else if (platform == "mac") extensions = dylibRx;

                            var remap = new Dictionary<string, string>();

                            using (var archive = new ZipArchive(s))
                            {
                                foreach (var e in archive.Entries)
                                {
                                    var fullName = e.FullName;
                                    fullName = fullName.Replace('\\', '/');

                                    Report.Line(4, "found: {0}", fullName);

                                    if (fullName == "remap.xml")
                                    {
                                        var doc = System.Xml.Linq.XDocument.Load(e.Open());
                                        remap = GetSymlinks(doc);
                                        continue;
                                    }

                                    var rest = "";
                                    var found = false;
                                    foreach (var p in copyPaths)
                                    {
                                        if (fullName.StartsWith(p))
                                        {
                                            rest = fullName.Substring(p.Length);
                                            found = true;
                                            break;
                                        }
                                    }

                                    if (found)
                                    {
                                        var localName = Path.Combine(rest.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries));
                                        var file =
                                            Path.Combine(
                                                dstFolder,
                                                localName
                                            );

                                        Report.Line(4, "copy {0} to {1}", localName, file);

                                        var dir = Path.GetDirectoryName(file);
                                        if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                                        if (!File.Exists(file)) e.ExtractToFile(file);
                                        if (extensions.IsMatch(fullName)) toLoad.Add(localName);
                                    }
                                }
                            }

                            foreach (var kvp in remap)
                            {
                                CreateSymlink(dstFolder, kvp.Key, kvp.Value);
                                if (extensions.IsMatch(kvp.Key))
                                {
                                    toLoad.Add(kvp.Key);
                                }
                            }

                        }


#if NETCOREAPP3_1

                        string[] formats = new string[0];
                        var os = GetOS();

                        if (os == OS.Linux) formats = new[] { "{0}.so", "lib{0}.so", "lib{0}.so.1" };
                        else if (os == OS.Win32) formats = new[] { "{0}.dll" };
                        else if (os == OS.MacOS) formats = new[] { "{0}.dylib", "lib{0}.dylib" };

                        NativeLibrary.SetDllImportResolver(a, (name, ass, searchPath) =>
                        {
                            return LoadLibrary(ass, name);
                        });

#else

                        // NOTE: libraries such as IPP are spread over multiple dlls that will now get loaded in an undeterministic order -> make sure native depenendies between them can be resolved
                        //        - for some reason SetDllDirectory is required even LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR is used that will include the directoy of the library that should be loaded
                        //        - for some other reason AddDllDirectory does not help loading libraries, even it should provide similar behavior than SetDllDirectory
                        if (platform == "windows")
                            Kernel32.SetDllDirectory(dstFolder);

                        foreach (var file in toLoad)
                        {
                            try
                            {
                                var ptr = IntPtr.Zero;
                                var filePath = Path.Combine(dstFolder, file);
                                if (platform == "windows") ptr = Kernel32.LoadLibraryEx(filePath, IntPtr.Zero, Kernel32.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | Kernel32.LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
                                else ptr = Dl.dlopen(filePath, 0x01102);

                                if (ptr == IntPtr.Zero)
                                {
                                    if (platform == "windows")
                                    {
                                        var err = Kernel32.GetLastError();
                                        Report.Warn("could not load native library: {0} Error={1}", file, err);
                                    }
                                    else
                                    {
                                        Report.Warn("could not load native library: {0}", filePath);
                                    }
                                }
                                else
                                {
                                    Report.Line(3, "loaded {0}", file);
                                }
                            }
                            catch (Exception ex)
                            {
                                Report.Warn("could not load native library {0}: {1}", file, ex);
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
                    Report.Warn("could not load native dependencies for {0}: {1}", a.FullName, e);
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

        public static void Init()
        {
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

#if NETCOREAPP3_1
            System.Runtime.Loader.AssemblyLoadContext.Default.ResolvingUnmanagedDll += (ass, name) =>
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

            Action<Assembly> loadNativeDependencies = e =>
                {
                    if (IntrospectionProperties.NativeLibraryUnpackingAllowed)
                    {
                        try
                        {
                            // https://github.com/aardvark-platform/aardvark.base/issues/61
                            var shouldLoad = IntrospectionProperties.UnpackNativeLibrariesFilter(e);
                            if (shouldLoad)
                            {
                                Report.Begin(4, "trying to load native dependencies for {0}", e.FullName);
                                LoadNativeDependencies(e);
                                Report.End(4);
                            }
                            else
                            {
                                Report.Line(4, "skipped LoadNativeDependencies for {0}", e.FullName);
                            }
                        }
                        catch (Exception ex) // actually catching exns here might not even be possible due to mscorlib recursive resource bug detection
                        {
                            Report.Warn("Could not load native dependencies for {0}:{1}", e.FullName, ex.StackTrace.ToString());
                        }
                    }
                };

            AppDomain.CurrentDomain.AssemblyLoad += (s, e) =>
            {
                loadNativeDependencies(e.LoadedAssembly);
            };

            if (IntrospectionProperties.NativeLibraryUnpackingAllowed)
            {
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
        }

        private static void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            throw new NotImplementedException();
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

                    Report.BeginTimed("initializing {1}", mi.Name, mi.DeclaringType.Name);

                    try
                    {
                        if (parameters.Length == 0) mi.Invoke(null, null);
                        else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IEnumerable<Assembly>)) mi.Invoke(null, new object[] { Introspection.AllAssemblies });
                        else Report.Warn("Strange aardvark init method found: {0}, should be Init : IEnumberable<Assembly> -> unit or unit -> unit", mi);
                    }
                    catch (Exception e) 
                    {
                        Report.Warn("failed: {0}", e);
                    }

                    Report.End();
                }
            }
        }
    }
}
