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
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.Text;

namespace Aardvark.Base
{
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

        public static string CurrentEntryPath => (CurrentEntryAssembly != null)
            ? Path.GetDirectoryName(CurrentEntryAssembly.Location)
            : null
            ;
    }

    public static class Introspection
    {
        private static CultureInfo s_cultureInfoEnUs = new CultureInfo("en-us");
        private static readonly string s_cacheDirName;
        private static Dictionary<string, Assembly> s_assemblies;
        private static HashSet<string> s_assembliesThatFailedToLoad = new HashSet<string>();
        private static HashSet<Assembly> s_allAssemblies = new HashSet<Assembly>();
        
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
                           from m in t.GetMethods()
                           let attribs = m.GetCustomAttributes(typeof(T), false)
                           where attribs.Length > 0
                           select (m, attribs.Select(x => (T)x).ToArray()),
                  types => from t in types
                           from m in t.GetMethods()
                           let attribs = m.GetCustomAttributes(typeof(T), false)
                           where attribs.Length > 0
                           select (m, attribs.Select(x => (T)x).ToArray()),
                  result => result.Select(m => m.Item1.DeclaringType.AssemblyQualifiedName)
                  );
        
        static Introspection()
        {
            // (1) initializing s_assembliesThatFailedToLoad
            new []
            {
                "Aardvark.Unmanaged",
                "Aardvark.Unmanaged.Diagnostics",
                "ASift.CLI",
                "CUDA.NET",
                "Emgu.CV",
                "FreeImageNET",
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
                "OpenTK"
            }
            .ForEach(x => s_assembliesThatFailedToLoad.Add(x));

            // (2) setting up cache directory (s_cacheDirName)
            s_cacheDirName = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            if (string.IsNullOrWhiteSpace(s_cacheDirName))
            {
                Report.Warn("SpecialFolder.CommonApplicationData does not exist!");
                s_cacheDirName = "cache"; // using ./cache
            }
            else
            {
                s_cacheDirName = Path.Combine(s_cacheDirName, @"Aardvark\cache");
            }
            Report.Line(4, "using cache dir: {0}", s_cacheDirName);

            if (!Directory.Exists(s_cacheDirName))
            {
                Directory.CreateDirectory(s_cacheDirName);
            }

            // (3) enumerating all assemblies reachable from entry assembly
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

            Report.Begin("trying all dlls and exes in current directory: {0}", IntrospectionProperties.CurrentEntryPath);
            foreach (var s in Directory.GetFiles(IntrospectionProperties.CurrentEntryPath, "*.dll").Concat(
                              Directory.GetFiles(IntrospectionProperties.CurrentEntryPath, "*.exe")))
            {
                try
                {
                    EnumerateAssemblies(AssemblyName.GetAssemblyName(s).Name);
                    Report.Line("{0}", s);
                }
                catch
                {
                }
            }
            Report.End();
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
            
            if (name.StartsWith("System.") ||
                name.StartsWith("VRLib.CLI") ||
                name.StartsWith("Microsoft") ||
                name.StartsWith("LidorSystems") ||
                name.StartsWith("WeifenLuo.") ||
                name.StartsWith("OpenCV") ||
                name.StartsWith("nunit.") ||
                name.StartsWith("Extreme.Numerics") ||
                name.StartsWith("fftwlib") ||
                name.StartsWith("GraphCutsCLI") ||
                name.StartsWith("Interop.MLApp") ||
                name.StartsWith("IPP") ||
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
                name.StartsWith("WindowsFormsIntegration") ||
                name.StartsWith("Roslyn") ||
                name.StartsWith("SharpDX") ||
                name.StartsWith("Aardvark.Jynx.Native") ||
                name.StartsWith("SurfaceQueueInteropHelper") ||
                name.StartsWith("ScintillaNET") ||
                name.StartsWith("IKVM") ||
                name.StartsWith("Super") ||
                name.StartsWith("Java") ||
                name.StartsWith("OpenTK")
                )
            {
                s_assembliesThatFailedToLoad.Add(name);
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
        private static string CreateCacheFileName(string fileName, Guid guid)
            => Path.Combine(s_cacheDirName, string.Format(@"{0}.{1}.txt", Path.GetFileName(fileName), guid));
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
            var assemblyFileName = a.Location;
            var cacheFileName = CreateCacheFileName(assemblyFileName, discriminator.ToGuid());
            var assemblyTimeStamp = File.GetLastWriteTimeUtc(assemblyFileName);
            if (File.Exists(cacheFileName))
            {
                /* var cacheFileTimeStamp = */ File.GetLastWriteTimeUtc(cacheFileName);
                var lines = File.ReadAllLines(cacheFileName);
                var header = lines.Length > 0 ? CacheFileHeader.Parse(lines[0]) : null;
                if (header != null && header.TimeStampOfCachedFile == assemblyTimeStamp)
                {
                    // return cached types
                    Report.Line(4, "[cache hit ] {0}", a);
                    return decode(lines.Skip(1)).ToArray();
                }
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

            // write cache file
            var headerLine =
                new CacheFileHeader { Version = 1, TimeStampOfCachedFile = assemblyTimeStamp }
                .ToString()
                .IntoIEnumerable()
                ;
            File.WriteAllLines(cacheFileName, headerLine.Concat(encode(result)).ToArray());

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
        private static string AppDataCache = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string s_cacheFile =  IntrospectionProperties.CurrentEntryAssembly == null
            ? ""
            : Path.Combine(AppDataCache, "Aardvark", "cache", IntrospectionProperties.CurrentEntryAssembly.GetName().Name + "_plugins.bin");

        public string CacheFile = string.Empty;
        
        public Aardvark()
        {
            CacheFile = s_cacheFile;
            Directory.CreateDirectory(Path.GetDirectoryName(CacheFile));
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

        private static bool IsPlugin(string file)
        {
            try
            {
                var a = Assembly.LoadFile(file);
                var empty = Introspection.GetAllMethodsWithAttribute<OnAardvarkInitAttribute>(a).IsEmpty();
                if (!empty)
                {
                    Report.Line(3, "[GetPluginAssemblyPaths] found plugins in: {0}", file);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch(FileLoadException e)
            {
                Report.Line(3, "[GetPluginAssemblyPaths] IsPlugin({0}) failed.", file);
                Report.Line(3, "[GetPluginAssemblyPaths] (FileLoad) Could not load potential plugin assembly (not necessarily an error. proceeding): {0}", e.Message);
                Report.Line(5, "[GetPluginAssemblyPaths] StackTrace (outer): {0}", e.StackTrace.ToString());
                try {
                    Report.Line(5, "[GetPluginAssemblyPaths] FusionLog: {0}", e.FusionLog);
                    if (e.InnerException != null)
                    {
                        Report.Line(5, "[GetPluginAssemblyPaths] Inner message: {0}", e.InnerException.Message);
                        Report.Line(5, "[GetPluginAssemblyPaths] Inner stackTrace: {0}", e.InnerException.StackTrace.ToString());
                    }
                } catch(Exception)
                {
                    Report.Line(5, "[GetPluginAssemblyPaths] could not print details for FileLoadException (most likely BadImageFormat)");
                }
                return false;
            }
            catch (Exception e)
            {
                Report.Line(3, "[GetPluginAssemblyPaths] IsPlugin({0}) failed.", file);
                Report.Line(3, "[GetPluginAssemblyPaths] Could not load potential plugin assembly (not necessarily an error. proceeding): {0}", e.Message);
                Report.Line(5, "[GetPluginAssemblyPaths] {0}", e.StackTrace.ToString());
                return false;
            }
        }

        public string[] GetPluginAssemblyPaths()
        {
            var cache = ReadCacheFile();
            var newCache = new Dictionary<string, Tuple<DateTime, bool>>();

            var verbosity = Report.Verbosity;
            Report.Verbosity = 0;
            var folder = IntrospectionProperties.CurrentEntryPath; 
            var assemblies = Directory.EnumerateFiles(folder)
                                      .Where(p => { var ext = Path.GetExtension(p).ToLowerInvariant(); return ext == ".dll" || ext == ".exe"; })
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
            Report.Verbosity = verbosity;
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

                Report.Line(3, "[LoadPlugins] Using plugin cache file name: {0}", Aardvark.s_cacheFile);
                aardvark.CacheFile = Aardvark.s_cacheFile;
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

                var myArch = IntPtr.Size == 8 ? "x86-64" : "x86";

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
            switch(Environment.OSVersion.Platform)
            {
                case PlatformID.Unix: return OS.Linux;
                case PlatformID.MacOSX: return OS.MacOS;
                default: return OS.Win32;
            }

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
            if (Environment.OSVersion.Platform != PlatformID.Unix) return;

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

                        var arch = IntPtr.Size == 8 ? "AMD64" : "x86";
                        var platform = "windows";
                        if (Environment.OSVersion.Platform == PlatformID.MacOSX) platform = "mac";
                        else if (Environment.OSVersion.Platform == PlatformID.Unix) platform = "linux";

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
        public static string NativeLibraryPath = Path.Combine(Path.GetTempPath(), "aardvark-native");

        /// Specify if native libraries should be extracted each to its own sub folder or directory to the NativeLibraryPath
        /// NOTE: When using global shared NativeLibraryPath, SeparateLibraryDirectories should not be set to false, as this there might be version conflicts
        public static bool SeparateLibraryDirectories = true;

        private static Dictionary<Assembly, string>  s_nativePaths = new Dictionary<Assembly, string>();

        public static string[] GetNativeLibraryPaths()
        {
            lock(s_nativePaths)
            {
                return s_nativePaths.Values.Where(p => p != null).ToArray();
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

                            s_nativePaths[assembly] = dstFolder;
                            path = dstFolder;
                            return true;
                        }
                    }
                }
            }
        }

        public static IntPtr LoadLibrary(Assembly assembly, string nativeName)
        {

            string[] formats = new string[0];
            bool windows = true;
            if (Environment.OSVersion.Platform == PlatformID.Unix)
            {
                windows = false;
                formats = new[] { "{0}.so", "lib{0}.so", "lib{0}.so.1" };
            }
            else formats = new[] { "{0}.dll" };
            IntPtr ptr;

            string[] paths;
            if (assembly != null)
            {
                if (TryGetNativeLibraryPath(assembly, out var dstFolder))
                {
                    paths = new[] { dstFolder };
                }
                else
                {
                    paths = new[] { Environment.CurrentDirectory };
                }
            }
            else
            {
                paths = GetNativeLibraryPaths();
            }

#if NETCOREAPP3_0

            var realName = Path.GetFileNameWithoutExtension(nativeName);
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
                        if (NativeLibrary.TryLoad(libPath, out ptr)) return ptr;
                        else Report.Warn("found native library {0} in {1} but it could not be loaded.", Path.GetFileName(libPath), p);
                    }
                }

                // try the plain loading mechanism.
                if (assembly != null && NativeLibrary.TryLoad(libName, assembly, null, out ptr)) return ptr;
                else if (NativeLibrary.TryLoad(libName, out ptr)) return ptr;

            }
            // try the standard loading mechanism as a last resort.
            if (assembly != null && NativeLibrary.TryLoad(nativeName, assembly, null, out ptr)) return ptr;
            else if (NativeLibrary.TryLoad(nativeName, out ptr)) return ptr;

            if (windows) return IntPtr.Zero;
            else return IntPtr.Zero;
#else

            Func<string, IntPtr> loadLibrary;
            if (windows) loadLibrary = (a) => Kernel32.LoadLibrary(a);
            else loadLibrary = (a) => Dl.dlopen(a, 1);


            ptr = loadLibrary(nativeName);
            if(ptr != IntPtr.Zero) return ptr;

            var realName = Path.GetFileNameWithoutExtension(nativeName);
            foreach (var fmt in formats)
            {
                var libName = string.Format(fmt, realName);

                ptr = loadLibrary(libName);
                if (ptr != IntPtr.Zero) return ptr;

                foreach (var p in paths)
                {
                    var libPath = Path.Combine(p, libName);
                    if (File.Exists(libPath))
                    {
                        ptr = loadLibrary(libPath);
                        if (ptr != IntPtr.Zero) return ptr;
                    }
                }
            }

            return IntPtr.Zero;
#endif
        }

        public static IntPtr GetProcAddress(IntPtr handle, string name)
        {
            if (handle == IntPtr.Zero) return IntPtr.Zero;

#if NETCOREAPP3_0
            IntPtr ptr;
            if (NativeLibrary.TryGetExport(handle, name, out ptr)) return ptr;
            else return IntPtr.Zero;
#else
            if (Environment.OSVersion.Platform == PlatformID.Unix) return Dl.dlsym(handle, name);
            else return Kernel32.GetProcAddress(handle, name);
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
                        var arch = IntPtr.Size == 8 ? "AMD64" : "x86";
                        var platform = "windows";
                        if (Environment.OSVersion.Platform == PlatformID.MacOSX) platform = "mac";
                        else if (Environment.OSVersion.Platform == PlatformID.Unix) platform = "linux";

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


#if NETCOREAPP3_0

                        string[] formats = new string[0];
                        if (Environment.OSVersion.Platform == PlatformID.Unix) formats = new[] { "{0}.so", "lib{0}.so", "lib{0}.so.1" };
                        else formats = new[] { "{0}.dll" };


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
                                        var err = Kernel32.GetLastError(); // so far always returned 0, NOTE: check documentation of SetErrorMode
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
        public static void Init(string basePath)
        {
            Report.BeginTimed("initializing aardvark");

            Report.Begin("System Information:");
            Report.Line("OSVersion: {0}", System.Environment.OSVersion);
            Report.Line("SystemArchitecture: {0}-bit", IntPtr.Size << 3);
            Report.Line("Environment.Version: {0}", Environment.Version);
            Report.End();

#if NETCOREAPP3_0
            System.Runtime.Loader.AssemblyLoadContext.Default.ResolvingUnmanagedDll += (ass, name) =>
            {
                return LoadLibrary(null, name);
            };
#endif

            AppDomain.CurrentDomain.AssemblyLoad += (s, e) =>
            {
                LoadNativeDependencies(e.LoadedAssembly);
            };

            foreach(var a in AppDomain.CurrentDomain.GetAssemblies())
            {
                LoadNativeDependencies(a);
            }

            Report.BeginTimed("Loading plugins");
            var pluginsList = LoadPlugins();

            Report.End();
            LoadAll(pluginsList);
            Report.End();
        }


        public static void Init()
        {
            var baseDir =
                IntrospectionProperties.CustomEntryAssembly != null
                ? Path.GetDirectoryName(IntrospectionProperties.CustomEntryAssembly.Location)
                : AppDomain.CurrentDomain.BaseDirectory;
            Init(baseDir);
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
                var initMethods = Introspection.GetAllMethodsWithAttribute<OnAardvarkInitAttribute>(ass).Select(t => t.Item1).Distinct().ToArray();

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
