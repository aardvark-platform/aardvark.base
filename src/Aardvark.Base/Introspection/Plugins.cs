namespace Aardvark.Base;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Runtime.Serialization;
using System.Xml;
using BundleReader = SingleFileExtractor.Core.ExecutableReader;
using BundleFileEntry = SingleFileExtractor.Core.FileEntry;

#if NET8_0_OR_GREATER
using System.Runtime.Loader;
#endif

[AttributeUsage(AttributeTargets.Method)]
public class OnAardvarkInitAttribute : Attribute;

public partial class Aardvark
{
    private static class Plugins
    {
        #region Paths

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

        private static readonly Lazy<string> s_pluginsCacheFile =
            new(() =>
            {
                Assembly entryAssembly = IntrospectionProperties.CurrentEntryAssembly;
                string entryAssemblyName = entryAssembly?.GetName().Name ?? "unknown";
                string entryAssemblyId = entryAssembly?.GetIdentifier(CachingProperties.PluginsCacheFileNaming) ?? "unknown";
                string fileName = $"{entryAssemblyName}_{entryAssemblyId}_plugins.xml";

                return Path.Combine(CacheDirectory, fileName);
            });

        /// <summary>
        /// Returns the directory of the plugins cache files.
        /// </summary>
        public static string CacheDirectory => s_pluginsCacheDirectory.Value;

        /// <summary>
        /// Returns the path of the plugins cache file.
        /// </summary>
        public static string CacheFile => s_pluginsCacheFile.Value;

        #endregion

        #region Assembly Source

        private abstract class AssemblySource
        {
            public abstract string Path { get; }

            public abstract DateTime LastModified { get; }

            public abstract Stream OpenRead();

            public abstract Assembly Load();
        }

        private class FileAssemblySource(string path) : AssemblySource
        {
            public override string Path { get; } = path;

            public override DateTime LastModified { get; } = FileUtils.GetLastWriteTimeSafe(path);

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
                foreach (var p in DirectoryUtils.GetFilesSafe(path))
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

        #endregion

        #region Cache

        /// <summary>
        /// Cache containing information about whether an assembly is an Aardvark plugin.
        /// </summary>
        [CollectionDataContract(Name = "Plugins", ItemName = "Assembly", KeyName = "Path", ValueName = "Data")]
        private class PluginCache : Dictionary<string, PluginCache.Data>
        {
            [DataContract]
            public struct Data(DateTime lastModified, bool isPlugin)
            {
                /// <summary>
                /// Modification time stamp of the assembly when the cache was created.
                /// Used to determine if the cache has been invalidated.
                /// </summary>
                [DataMember(IsRequired = true)]
                public DateTime LastModified = lastModified;

                /// <summary>
                /// Indicates whether the assembly is an Aardvark plugin.
                /// </summary>
                [DataMember(IsRequired = true)]
                public bool IsPlugin = isPlugin;
            }

            private static readonly DataContractSerializer serializer = new DataContractSerializer(typeof(PluginCache));

            public static PluginCache ReadFromFile()
            {
                if (File.Exists(CacheFile))
                {
                    try
                    {
                        using var stream = new FileStream(CacheFile, FileMode.Open);
                        var result = (PluginCache)serializer.ReadObject(stream);
                        Report.Line(3, $"Loaded plugins cache file: {CacheFile}");
                        return result;
                    }
                    catch (Exception e)
                    {
                        Report.Line(3, $"Could not load plugins cache file '{CacheFile}': {e.Message}");
                        return new PluginCache();
                    }
                }

                Report.Line(3, $"Using new plugins cache file: {CacheFile}");
                return new PluginCache();
            }

            public void WriteToFile()
            {
                if (string.IsNullOrEmpty(CacheFile))
                {
                    Report.Warn("Could not write plugins cache file since Aardvark.Plugins.CacheFile is null or empty");
                }
                else
                {
                    try
                    {
                        if (File.Exists(CacheFile)) File.Delete(CacheFile);

                        using var stream = new FileStream(CacheFile, FileMode.CreateNew);
                        using var writer = XmlWriter.Create(stream, new XmlWriterSettings { Indent = true });
                        serializer.WriteObject(writer, this);
                    }
                    catch(Exception e)
                    {
                        Report.Warn($"Could not write plugins cache file '{CacheFile}': {e.Message}");
                    }
                }
            }
        }

        #endregion

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
                                var match = version != null ? RegexPatterns.Assembly.TargetFramework.Match(version) : null;
                                if (match is { Success: true })
                                {
                                    var fwName = match.Groups["name"].Value;
                                    var isLoadable = fwName is ".NETCoreApp" or ".NETStandard";
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
                    sources.AddDirectory(rootPath);
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
            var ext = PathUtils.GetExtensionSafe(source.Path)?.ToLowerInvariant();
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

        internal static List<Assembly> Find()
        {
            var oldCache = PluginCache.ReadFromFile();
            var newCache = new PluginCache();

            using var sources = FindAssemblySources();
            List<Assembly> assemblies = [];

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
                newCache.WriteToFile();
            }

            return assemblies;
        }

        internal static void Init()
        {
            Report.BeginTimed("Loading plugins");

            try
            {
                var plugins = Find();
                var allAssemblies = Introspection.AllAssemblies.Concat(plugins).GroupBy(a => a.FullName).Select(x => x.First()).ToArray();

                foreach (var assembly in allAssemblies)
                {
                    var initMethods = Introspection.GetAllMethodsWithAttribute<OnAardvarkInitAttribute>(assembly).Select(t => t.Item1).Distinct().ToArray();

                    foreach (var mi in initMethods)
                    {
                        var parameters = mi.GetParameters();

                        Report.BeginTimed($"Initializing {mi.DeclaringType?.Name ?? mi.Name}");

                        try
                        {
                            if (parameters.Length == 0)
                                mi.Invoke(null, null);
                            else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(IEnumerable<Assembly>))
                                mi.Invoke(null, [Introspection.AllAssemblies]);
                            else
                                Report.Warn($"Strange Aardvark init method found: {mi}, should be IEnumberable<Assembly> -> unit or unit -> unit");
                        }
                        catch (Exception e)
                        {
                            Report.Warn($"Failed: {e}");
                        }
                        finally
                        {
                            Report.End();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Report.Warn($"{e.GetType().Name} occurred while loading plugins: {e.Message}");
            }

            Report.EndTimed();
        }
    }

    /// <summary>
    /// Returns the directory of the plugins cache files.
    /// </summary>
    public static string PluginsCacheDirectory => Plugins.CacheDirectory;

    /// <summary>
    /// Returns the path of the plugins cache file.
    /// </summary>
    public static string PluginsCacheFile => Plugins.CacheFile;

    #region Obsolete

    [Obsolete("Use PluginsCacheDirectory instead.")]
    public static string CacheDirectory => PluginsCacheDirectory;

    [Obsolete("Use PluginsCacheFile instead.")]
    public string CacheFile = string.Empty;

    [Obsolete]
    public static List<Assembly> LoadPlugins() => Plugins.Find();

    #endregion
}