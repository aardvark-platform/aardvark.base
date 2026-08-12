namespace Aardvark.Base;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

public static class Introspection
{
    private static readonly CultureInfo s_cultureInfoEnUs = new("en-us");
    private static readonly Dictionary<string, Assembly> s_assemblies;
    private static readonly HashSet<string> s_assembliesThatFailedToLoad = [];
    private static readonly HashSet<Assembly> s_allAssemblies = [];

    private static string InitializeCacheDirectory()
    {
        var path = Path.Combine(CachingProperties.CacheDirectory, "Introspection");

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        return path;
    }

    private static readonly Lazy<string> s_cacheDirectory = new(InitializeCacheDirectory);

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
    public static void RegisterAssembly(Assembly assembly)
    {
        if (assembly != null) s_allAssemblies.Add(assembly);
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
    /// Enumerates all public instance and static methods declared by known types and
    /// decorated with attribute T. Each method is returned once together with all of
    /// its T-attribute instances.
    /// </summary>
    public static IEnumerable<(MethodInfo, T[])> GetAllMethodsWithAttribute<T>()
        => AllAssemblies.SelectMany(GetAllMethodsWithAttribute<T>);

    /// <summary>
    /// Enumerates all classes from the specified assembly
    /// implementing the specified interface. Successful matches are retained if
    /// inspecting another type fails; incomplete live scans are not cached.
    /// </summary>
    public static Type[] GetAllClassesImplementingInterface(Assembly assembly, Type interfaceType)
        => GetAll___(assembly, interfaceType.FullName,
            (IEnumerable<string> lines, ref QueryDiagnostics diagnostics) =>
                ResolveTypes(lines, false, ref diagnostics),
            (Type[] types, ref QueryDiagnostics diagnostics) =>
                FilterTypes(types,
                    t => (t.IsClass || t.IsValueType) && t.GetInterfaces().Contains(interfaceType),
                    ref diagnostics),
            result => result.Select(t => t.AssemblyQualifiedName)
        );

    /// <summary>
    /// Enumerates all classes from the specified assembly
    /// inheriting from the specified base class. Successful matches are retained
    /// if inspecting another type fails; incomplete live scans are not cached.
    /// </summary>
    public static Type[] GetAllClassesInheritingFrom(Assembly assembly, Type baseType)
        => GetAll___(assembly, baseType.FullName,
            (IEnumerable<string> lines, ref QueryDiagnostics diagnostics) =>
                ResolveTypes(lines, false, ref diagnostics),
            (Type[] types, ref QueryDiagnostics diagnostics) =>
                FilterTypes(types, t => t.IsSubclassOf(baseType), ref diagnostics),
            result => result.Select(t => t.AssemblyQualifiedName)
        );

    /// <summary>
    /// Enumerates all types from the specified assembly
    /// decorated with attribute T as tuples of type
    /// and its one or more T-attributes. Successful matches are retained when
    /// other attributes cannot be constructed; incomplete live scans are not cached.
    /// </summary>
    public static (Type, T[])[] GetAllTypesWithAttribute<T>(Assembly assembly)
        => GetAll___<(Type, T[])>(assembly, typeof(T).FullName,
           (IEnumerable<string> lines, ref QueryDiagnostics diagnostics) =>
                DecodeTypesWithAttribute<T>(lines, ref diagnostics),
           (Type[] types, ref QueryDiagnostics diagnostics) =>
                GetTypesWithAttribute<T>(types, ref diagnostics),
           result => result.Select(t => t.Item1.AssemblyQualifiedName)
        );

    private static T[] GetCustomAttributes<T>(Type type, ref QueryDiagnostics diagnostics)
    {
        if (type == null) return [];
        try
        {
            return type.GetCustomAttributes(typeof(T), false).Select(x => (T)x).ToArray();
        }
        catch (Exception e)
        {
            AddDiagnostic(ref diagnostics, DiagnosticKind.TypeAttributes, GetTypeNameSafe(type), e);
        }
        return [];
    }

    private static T[] GetCustomAttributes<T>(MethodInfo mi, ref QueryDiagnostics diagnostics)
    {
        if (mi == null) return [];
        try
        {
            return mi.GetCustomAttributes(typeof(T), false).Select(x => (T)x).ToArray();
        }
        catch (Exception e)
        {
            AddDiagnostic(ref diagnostics, DiagnosticKind.MethodAttributes,
                GetMethodNameSafe(mi), e);
        }
        return [];
    }

    private const BindingFlags PublicDeclaredMethods =
        BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;

    private static ScanResult<(MethodInfo, T[])> GetMethodsWithAttribute<T>(
        Type[] types, ref QueryDiagnostics diagnostics
    )
    {
        var initialFailureCount = GetFailureCount(diagnostics);
        var result = new List<(MethodInfo, T[])>();

        foreach (var type in types)
        {
            if (type == null) continue;

            MethodInfo[] methods;
            try
            {
                methods = type.GetMethods(PublicDeclaredMethods) ?? [];
            }
            catch (Exception e)
            {
                AddDiagnostic(ref diagnostics, DiagnosticKind.MethodEnumeration,
                    GetTypeNameSafe(type), e);
                continue;
            }

            foreach (var method in methods)
            {
                var attributes = GetCustomAttributes<T>(method, ref diagnostics);
                if (attributes.Length > 0) result.Add((method, attributes));
            }
        }

        return new ScanResult<(MethodInfo, T[])>(
            result.ToArray(), GetFailureCount(diagnostics) != initialFailureCount
        );
    }

    private static ScanResult<Type> ResolveTypes(
        IEnumerable<string> lines, bool unique, ref QueryDiagnostics diagnostics
    )
    {
        var initialFailureCount = GetFailureCount(diagnostics);
        var result = new List<Type>();
        string first = null;
        HashSet<string> seen = null;

        foreach (var line in lines)
        {
            if (line == null) continue;

            if (unique && first == null)
            {
                first = line;
            }
            else if (unique)
            {
                seen ??= new HashSet<string>(StringComparer.Ordinal) { first };
                if (!seen.Add(line)) continue;
            }

            try
            {
                var type = GetType(line);
                if (type != null)
                {
                    result.Add(type);
                }
                else
                {
                    AddDiagnostic(ref diagnostics, DiagnosticKind.CacheTypeResolution, line,
                        nameof(TypeLoadException), "Cached type could not be resolved.");
                }
            }
            catch (Exception e)
            {
                AddDiagnostic(ref diagnostics, DiagnosticKind.CacheTypeResolution, line, e);
            }
        }

        return new ScanResult<Type>(
            result.ToArray(), GetFailureCount(diagnostics) != initialFailureCount
        );
    }

    private static ScanResult<Type> FilterTypes(
        Type[] types, Func<Type, bool> predicate, ref QueryDiagnostics diagnostics
    )
    {
        var initialFailureCount = GetFailureCount(diagnostics);
        var result = new List<Type>();

        foreach (var type in types)
        {
            if (type == null) continue;

            try
            {
                if (predicate(type)) result.Add(type);
            }
            catch (Exception e)
            {
                AddDiagnostic(ref diagnostics, DiagnosticKind.TypeInspection,
                    GetTypeNameSafe(type), e);
            }
        }

        return new ScanResult<Type>(
            result.ToArray(), GetFailureCount(diagnostics) != initialFailureCount
        );
    }

    private static ScanResult<(Type, T[])> DecodeTypesWithAttribute<T>(
        IEnumerable<string> lines, ref QueryDiagnostics diagnostics
    )
    {
        var resolved = ResolveTypes(lines, false, ref diagnostics);
        var result = GetTypesWithAttribute<T>(resolved.Items, ref diagnostics);
        return new ScanResult<(Type, T[])>(result.Items, resolved.Incomplete || result.Incomplete);
    }

    private static ScanResult<(Type, T[])> GetTypesWithAttribute<T>(
        Type[] types, ref QueryDiagnostics diagnostics
    )
    {
        var initialFailureCount = GetFailureCount(diagnostics);
        var result = new List<(Type, T[])>();

        foreach (var type in types)
        {
            if (type == null) continue;

            var attributes = GetCustomAttributes<T>(type, ref diagnostics);
            if (attributes.Length > 0) result.Add((type, attributes));
        }

        return new ScanResult<(Type, T[])>(
            result.ToArray(), GetFailureCount(diagnostics) != initialFailureCount
        );
    }

    private static ScanResult<(MethodInfo, T[])> DecodeMethodsWithAttribute<T>(
        IEnumerable<string> lines, ref QueryDiagnostics diagnostics
    )
    {
        var resolved = ResolveTypes(lines, true, ref diagnostics);
        var result = GetMethodsWithAttribute<T>(resolved.Items, ref diagnostics);
        return new ScanResult<(MethodInfo, T[])>(result.Items, resolved.Incomplete || result.Incomplete);
    }

    private static IEnumerable<string> GetUniqueDeclaringTypeNames<T>((MethodInfo, T[])[] methods)
    {
        string first = null;
        HashSet<string> seen = null;

        foreach (var method in methods)
        {
            var name = method.Item1.DeclaringType?.AssemblyQualifiedName;
            if (name == null) continue;

            if (first == null)
            {
                first = name;
            }
            else
            {
                seen ??= new HashSet<string>(StringComparer.Ordinal) { first };
                if (!seen.Add(name)) continue;
            }

            yield return name;
        }
    }

    /// <summary>
    /// Enumerates public instance and static methods declared by types in the specified
    /// assembly and decorated with attribute T. Each method is returned once together
    /// with all of its T-attribute instances. The query cache stores each declaring type
    /// once and ignores repeated declaring-type lines in legacy cache files. Successful
    /// matches are retained when other reflection operations fail; incomplete cache
    /// entries are retried live and incomplete live scans are not cached.
    /// </summary>
    public static (MethodInfo, T[])[] GetAllMethodsWithAttribute<T>(Assembly assembly)
        => GetAll___<(MethodInfo, T[])>(assembly, typeof(T).FullName,
              (IEnumerable<string> lines, ref QueryDiagnostics diagnostics) =>
                  DecodeMethodsWithAttribute<T>(lines, ref diagnostics),
              (Type[] types, ref QueryDiagnostics diagnostics) =>
                  GetMethodsWithAttribute<T>(types, ref diagnostics),
              GetUniqueDeclaringTypeNames
        );

#if NET8_0_OR_GREATER
    /// <summary>
    /// Returns the type with the given name.
    /// Uses <see cref="IntrospectionProperties.AssemblyLoadContext"/> to load the type.
    /// </summary>
    /// <param name="typeName">Name of the type to retrieve.</param>
    /// <returns>Type with given name, or null on failure.</returns>
#else
    /// <summary>
    /// Returns the type with the given name.
    /// </summary>
    /// <param name="typeName">Name of the type to retrieve.</param>
    /// <returns>Type with given name, or null if not found.</returns>
#endif
    public static Type GetType(string typeName)
    {
#if NET8_0_OR_GREATER
        using var _ = IntrospectionProperties.AssemblyLoadContext.EnterContextualReflection();
#endif
        return typeName != null ? Type.GetType(typeName) : null;
    }

    static Introspection()
    {
        Report.BeginTimed("Enumerating assemblies for introspection");

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

        Report.EndTimed();
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
            foreach (var file in DirectoryUtils.GetFilesSafe(path))
            {
                var ext = PathUtils.GetExtensionSafe(file)?.ToLowerInvariant();
                if (ext != ".dll" &&  ext != ".exe") continue;

                try
                {
                    var name = AssemblyName.GetAssemblyName(file);
                    Report.Line(4, $"{PathUtils.GetFileNameSafe(file)}");
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
        if (s_assembliesThatFailedToLoad.Contains(name)) return;
        if (s_assemblies.ContainsKey(name)) return;

        if (!IntrospectionProperties.AssemblyFilter(name))
        {
            Report.Line(4, "[Introspection] Ignoring assembly {0} due to filter", name);
            return;
        }

        try
        {
#if NET8_0_OR_GREATER
            var assembly = customAssembly ?? IntrospectionProperties.AssemblyLoadContext.LoadFromAssemblyName(new AssemblyName(name));
#else
            var assembly = customAssembly ?? Assembly.Load(name);
#endif
            s_assemblies[name] = assembly;
            RegisterAssembly(assembly);
            foreach (var a in assembly.GetReferencedAssemblies())
            {
                if (a.Name != null && !s_assemblies.ContainsKey(a.Name))
                {
                    EnumerateAssemblies(a.Name);
                }
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
            return string.Format(s_cultureInfoEnUs, "version {0} timestamp {1}", Version, TimeStampOfCachedFile.ToBinary());
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

    private enum DiagnosticKind
    {
        LoaderExceptions,
        TypeEnumeration,
        CacheTypeResolution,
        TypeInspection,
        TypeAttributes,
        MethodEnumeration,
        MethodAttributes,
        CacheAccess,
    }

    private sealed class DiagnosticExample
    {
        public readonly string ExceptionType;
        public readonly string Message;
        public readonly string Subject;

        public DiagnosticExample(string exceptionType, string message, string subject)
        {
            ExceptionType = exceptionType;
            Message = message;
            Subject = subject;
        }
    }

    private sealed class DiagnosticGroup
    {
        private const int MaxExamples = 3;
        private readonly HashSet<string> m_unique = new(StringComparer.Ordinal);
        private readonly List<DiagnosticExample> m_examples = [];

        public int Count { get; private set; }
        public int UniqueCount => m_unique.Count;
        public IReadOnlyList<DiagnosticExample> Examples => m_examples;

        public void Add(string exceptionType, string message, string subject)
        {
            Count++;
            var key = $"{exceptionType}\n{message}";
            if (!m_unique.Add(key)) return;
            if (m_examples.Count < MaxExamples)
                m_examples.Add(new DiagnosticExample(exceptionType, message, subject));
        }
    }

    private sealed class QueryDiagnostics
    {
        private readonly DiagnosticGroup[] m_groups =
            new DiagnosticGroup[(int)DiagnosticKind.CacheAccess + 1];

        public int FailureCount { get; private set; }
        public int ReflectionTypeLoadCount { get; private set; }

        public void Add(DiagnosticKind kind, string subject, string exceptionType, string message)
        {
            FailureCount++;
            var index = (int)kind;
            var group = m_groups[index] ??= new DiagnosticGroup();
            group.Add(Compact(exceptionType), Compact(message), Compact(subject));
        }

        public void Add(ReflectionTypeLoadException exception)
        {
            ReflectionTypeLoadCount++;
            var loaderExceptions = exception.LoaderExceptions;
            if (loaderExceptions == null || loaderExceptions.Length == 0)
            {
                Add(DiagnosticKind.LoaderExceptions, null, exception.GetType().Name, exception.Message);
                return;
            }

            var added = false;
            foreach (var loaderException in loaderExceptions)
            {
                if (loaderException == null) continue;
                var unwrapped = Unwrap(loaderException);
                Add(DiagnosticKind.LoaderExceptions, null, unwrapped.GetType().Name, unwrapped.Message);
                added = true;
            }

            if (!added)
                Add(DiagnosticKind.LoaderExceptions, null, exception.GetType().Name, exception.Message);
        }

        public DiagnosticGroup GetGroup(DiagnosticKind kind) => m_groups[(int)kind];

        private static string Compact(string value)
        {
            if (string.IsNullOrEmpty(value)) return value ?? "";

            const int maxLength = 240;
            var compact = value.Replace('\r', ' ').Replace('\n', ' ');
            return compact.Length <= maxLength ? compact : compact.Substring(0, maxLength) + "...";
        }
    }

    private readonly struct ScanResult<T>
    {
        public readonly T[] Items;
        public readonly bool Incomplete;

        public ScanResult(T[] items, bool incomplete)
        {
            Items = items;
            Incomplete = incomplete;
        }
    }

    private delegate ScanResult<T> DecodeQuery<T>(
        IEnumerable<string> lines, ref QueryDiagnostics diagnostics
    );

    private delegate ScanResult<T> ScanTypes<T>(
        Type[] types, ref QueryDiagnostics diagnostics
    );

    private static int GetFailureCount(QueryDiagnostics diagnostics)
        => diagnostics?.FailureCount ?? 0;

    private static Exception Unwrap(Exception exception)
    {
        while (exception is TargetInvocationException && exception.InnerException != null)
            exception = exception.InnerException;
        return exception;
    }

    private static string GetTypeNameSafe(Type type)
    {
        if (type == null) return "<unknown type>";
        try
        {
            return type.FullName ?? type.Name ?? "<unknown type>";
        }
        catch
        {
            return "<unknown type>";
        }
    }

    private static string GetMethodNameSafe(MethodInfo method)
    {
        if (method == null) return "<unknown method>";
        try
        {
            return $"{GetTypeNameSafe(method.DeclaringType)}.{method.Name}";
        }
        catch
        {
            return "<unknown method>";
        }
    }

    private static string GetAssemblyNameSafe(Assembly assembly)
    {
        if (assembly == null) return "<unknown assembly>";
        try
        {
            return assembly.GetName()?.Name ?? assembly.FullName ?? "<unknown assembly>";
        }
        catch
        {
            return "<unknown assembly>";
        }
    }

    private static void AddDiagnostic(
        ref QueryDiagnostics diagnostics, DiagnosticKind kind, string subject, Exception exception
    )
    {
        exception = Unwrap(exception);
        AddDiagnostic(ref diagnostics, kind, subject, exception.GetType().Name, exception.Message);
    }

    private static void AddDiagnostic(
        ref QueryDiagnostics diagnostics, DiagnosticKind kind, string subject,
        string exceptionType, string message
    )
    {
        diagnostics ??= new QueryDiagnostics();
        diagnostics.Add(kind, subject, exceptionType, message);
    }

    private static string GetDiagnosticLabel(DiagnosticKind kind)
    {
        return kind switch
        {
            DiagnosticKind.LoaderExceptions => "loader exceptions",
            DiagnosticKind.TypeEnumeration => "type enumeration",
            DiagnosticKind.CacheTypeResolution => "cached type resolution",
            DiagnosticKind.TypeInspection => "type inspection",
            DiagnosticKind.TypeAttributes => "type attribute construction",
            DiagnosticKind.MethodEnumeration => "method enumeration",
            DiagnosticKind.MethodAttributes => "method attribute construction",
            DiagnosticKind.CacheAccess => "cache access",
            _ => "unknown",
        };
    }

    private static void ReportDiagnostics(
        Assembly assembly, string discriminator, QueryDiagnostics diagnostics
    )
    {
        if (diagnostics == null) return;

        Report.Begin(3,
            "[Introspection] Query {0} for assembly {1} encountered {2} failure(s)",
            discriminator, GetAssemblyNameSafe(assembly), diagnostics.FailureCount);
        try
        {
            if (diagnostics.ReflectionTypeLoadCount > 0)
            {
                Report.Line(3, "ReflectionTypeLoadException affected {0} assembly type scan(s).",
                    diagnostics.ReflectionTypeLoadCount);
            }

            for (var i = 0; i <= (int)DiagnosticKind.CacheAccess; i++)
            {
                var kind = (DiagnosticKind)i;
                var group = diagnostics.GetGroup(kind);
                if (group == null) continue;

                Report.Line(3, "{0}: {1} failure(s), {2} unique diagnostic(s).",
                    GetDiagnosticLabel(kind), group.Count, group.UniqueCount);

                foreach (var example in group.Examples)
                {
                    if (string.IsNullOrEmpty(example.Subject))
                        Report.Line(3, "  {0}: {1}", example.ExceptionType, example.Message);
                    else
                        Report.Line(3, "  {0}: {1} [{2}]",
                            example.ExceptionType, example.Message, example.Subject);
                }

                var omitted = group.UniqueCount - group.Examples.Count;
                if (omitted > 0)
                    Report.Line(3, "  {0} additional unique diagnostic(s) omitted.", omitted);
            }
        }
        finally
        {
            Report.End(3);
        }
    }

    private static T[] GetAll___<T>(
        Assembly a, string discriminator,
        DecodeQuery<T> decode,
        ScanTypes<T> createResult,
        Func<T[], IEnumerable<string>> encode
        )
    {
        var cacheFileName = "";
        var assemblyTimeStamp = DateTime.MinValue;
        QueryDiagnostics diagnostics = null;

        // whatever happens, don't halt just because of caching... this actually happens for self-contained deployments https://github.com/aardvark-platform/aardvark.base/issues/65
        try
        {
            cacheFileName = GetQueryCacheFilename(a, discriminator.ToGuid());
            assemblyTimeStamp = a.GetLastWriteTimeSafe();

            // for standalone deployments cacheFileNames cannot be retrieved robustly - we skip those
            if (!string.IsNullOrEmpty(cacheFileName) && File.Exists(cacheFileName))
            {
                var lines = File.ReadAllLines(cacheFileName);
                var header = lines.Length > 0 ? CacheFileHeader.Parse(lines[0]) : null;
                if (header != null && header.TimeStampOfCachedFile == assemblyTimeStamp)
                {
                    Report.Line(4, "[cache hit ] {0}", a);
                    ScanResult<T> cached;
                    try
                    {
                        cached = decode(lines.Skip(1), ref diagnostics);
                    }
                    catch (Exception e)
                    {
                        AddDiagnostic(ref diagnostics, DiagnosticKind.CacheAccess,
                            cacheFileName, e);
                        cached = new ScanResult<T>([], true);
                    }

                    if (!cached.Incomplete) return cached.Items;

                    TryDeleteCacheFile(cacheFileName, ref diagnostics);
                    Report.Line(3, "[Introspection] Retrying incomplete cache query live for {0}", a);
                }
            }
        }
        catch(Exception e)
        {
            AddDiagnostic(ref diagnostics, DiagnosticKind.CacheAccess, cacheFileName, e);
        }

        Report.Line(4, "[cache miss] {0}", a);

        // Notes by hs:
        // previously (rev 19495) typeloadexception resulted in empty result set.
        // even in case of typeloadexception there may be some successfully loaded
        // types in result set. Just continue processing with these types
        // effect: dlls with external unused dependencies don't have to be shipped.
        Type[] ts;
        var typeScanIncomplete = false;
        try
        {
            ts = a.GetTypes() ?? [];
        }
        catch (ReflectionTypeLoadException e)
        {
            diagnostics ??= new QueryDiagnostics();
            diagnostics.Add(e);
            typeScanIncomplete = true;
            ts = e.Types?.Where(t => t != null).ToArray() ?? [];
        }
        catch (Exception e)
        {
            AddDiagnostic(ref diagnostics, DiagnosticKind.TypeEnumeration,
                GetAssemblyNameSafe(a), e);
            typeScanIncomplete = true;
            ts = [];
        }

        ScanResult<T> scan;
        try
        {
            scan = createResult(ts, ref diagnostics);
        }
        catch (Exception e)
        {
            AddDiagnostic(ref diagnostics, DiagnosticKind.TypeEnumeration,
                GetAssemblyNameSafe(a), e);
            scan = new ScanResult<T>([], true);
        }

        var incomplete = typeScanIncomplete || scan.Incomplete;
        if (incomplete)
        {
            TryDeleteCacheFile(cacheFileName, ref diagnostics);
        }
        else
        {
            // whatever happens, don't halt everything just because caching fails
            try
            {
                // for standalone deployments cacheFileNames cannot be retrieved robustly - we skip those
                if (!string.IsNullOrEmpty(cacheFileName))
                {
                    var headerLine =
                        new CacheFileHeader { Version = 1, TimeStampOfCachedFile = assemblyTimeStamp }
                        .ToString()
                        .IntoIEnumerable();

                    File.WriteAllLines(cacheFileName, headerLine.Concat(encode(scan.Items)).ToArray());
                }
            }
            catch(Exception e)
            {
                AddDiagnostic(ref diagnostics, DiagnosticKind.CacheAccess, cacheFileName, e);
            }
        }

        ReportDiagnostics(a, discriminator, diagnostics);
        return scan.Items;
    }

    private static void TryDeleteCacheFile(
        string cacheFileName, ref QueryDiagnostics diagnostics
    )
    {
        if (string.IsNullOrEmpty(cacheFileName) || !File.Exists(cacheFileName)) return;

        try
        {
            File.Delete(cacheFileName);
        }
        catch (Exception e)
        {
            AddDiagnostic(ref diagnostics, DiagnosticKind.CacheAccess, cacheFileName, e);
        }
    }
}
