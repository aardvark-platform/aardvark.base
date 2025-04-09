using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Aardvark.Base
{
    public static class Introspection
    {
        private static readonly CultureInfo s_cultureInfoEnUs = new("en-us");
        private static readonly HashSet<string> s_processedAssemblies = new();
        private static readonly HashSet<Assembly> s_allAssemblies = new();

        private static readonly Lazy<string> s_cacheDirectory = new(() =>
        {
            var path = Path.Combine(CachingProperties.CacheDirectory, "Introspection");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        });

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
            => AllAssemblies.SelectMany(GetAllTypesWithAttribute<T>);

        /// <summary>
        /// Enumerates all methods decorated with attribute T as tuples of MethodInfo
        /// and its one or more T-attributes.
        /// </summary>
        public static IEnumerable<(MethodInfo, T[])> GetAllMethodsWithAttribute<T>()
            => AllAssemblies.SelectMany(GetAllMethodsWithAttribute<T>);

        /// <summary>
        /// Enumerates all classes from the specified assembly
        /// implementing the specified interface.
        /// </summary>
        public static Type[] GetAllClassesImplementingInterface(Assembly a, Type interfaceType)
            => GetAll___(a, interfaceType.FullName,
                lines => lines.Select(Type.GetType),
                types => types.Where(t => (t.IsClass || t.IsValueType) && t.GetInterfaces().Contains(interfaceType)),
                result => result.Select(t => t.AssemblyQualifiedName)
                );

        /// <summary>
        /// Enumerates all classes from the specified assembly
        /// inheriting from the specified base class.
        /// </summary>
        public static Type[] GetAllClassesInheritingFrom(Assembly a, Type baseType)
            => GetAll___(a, baseType.FullName,
                lines => lines.Select(Type.GetType),
                types => types.Where(t => t.IsSubclassOf(baseType)),
                result => result.Select(t => t.AssemblyQualifiedName)
                );

        /// <summary>
        /// Enumerates all types from the specified assembly
        /// decorated with attribute T as tuples of type
        /// and its one or more T-attributes.
        /// </summary>
        public static (Type, T[])[] GetAllTypesWithAttribute<T>(Assembly a)
            => GetAll___(a, typeof(T).FullName,
               lines => lines.Select(Type.GetType)
                        .Select(t => (t, TryGetCustomAttributes<T>(t))),
               types => from t in types
                        let attribs = TryGetCustomAttributes<T>(t)
                        where attribs.Length > 0
                        select (t, attribs.ToArray()),
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
            return [];
        }

        private static T[] TryGetCustomAttributes<T>(MethodInfo t)
        {
            try
            {
                return t.GetCustomAttributes(typeof(T), false).Select(x => (T)x).ToArray();
            }
            catch (Exception e)
            {
                Report.Line(3, "[Introspection] Failed to get custom attributes for {0}.{1}: {2} ({3})", t.DeclaringType?.FullName, t.Name, e.Message, e.GetType().Name);
            }
            return [];
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
                  result => result.Select(m => m.Item1.DeclaringType?.AssemblyQualifiedName)
                  );

        static Introspection()
        {
            // enumerating all assemblies reachable from entry assembly
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
                    catch (Exception e)
                    {
                        Report.Warn($"[Introspection] Failed to register assembly '{file}': {e.Message}'");
                    }
                }
            }
            catch (Exception e)
            {
                Report.Warn($"[Introspection] Error while registering assemblies in '{path}': {e.Message}");
            }
            finally
            {
                Report.End(4);
            }
        }

        /// <summary>
        /// Note by hs: Since this function throws and catches exceptions in non-exceptional cases we
        /// use [DebuggerNonUserCode] to deactive first chance exceptions here
        /// at least if non-user code is deactivated in Options/Debugging.
        /// </summary>
        /// <param name="name">the name of the entry assembly</param>
        /// <param name="customAssembly">If the root assembly is not the assembly which has been started
        /// by the AppDomain a customAssembly is used alternatively.</param>
        [DebuggerNonUserCode]
        private static void EnumerateAssemblies(string name, Assembly customAssembly = null)
        {
            if (string.IsNullOrEmpty(name)) return;
            if (!s_processedAssemblies.Add(name)) return;

            if (!IntrospectionProperties.AssemblyFilter(name))
            {
                Report.Line(4, $"[Introspection] Ignoring assembly {name} due to filter");
                return;
            }

            try
            {
                var assembly = customAssembly ?? Assembly.Load(name);
                RegisterAssembly(assembly);

                foreach (var referencedName in assembly.GetReferencedAssemblies())
                {
                    EnumerateAssemblies(referencedName.Name);
                }
            }
            catch (Exception e)
            {
                Report.Warn($"[Introspection] Error while loading assembly {name}: {e.Message}");
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
            }
            catch(Exception e)
            {
                Report.Warn("[Introspection] Could not get cache for {1}: {0}", e.Message, a.FullName);
            }

            Report.Line(4, "[cache miss] {0}", a);

            // Notes by hs:
            // previously (rev 19495) typeloadexception resulted in empty result set.
            // even in case of typeloadexception there may be some successfully loaded
            // types in result set. Just continue processing with these types
            // effect: dlls with external unused dependencies don't have to be shipped.
            Type[] ts;
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


            // whatever happens, don't halt everything just because caching fails
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
}
