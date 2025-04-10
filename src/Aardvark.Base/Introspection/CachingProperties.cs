using System;
using System.IO;
using System.Reflection;

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

        private static readonly Lazy<string> s_cacheDirectory = new(() =>
        {
            var path = CustomCacheDirectory;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                if (string.IsNullOrWhiteSpace(path))
                {
                    Report.Warn("Environment.SpecialFolder.LocalApplicationData does not exist");
                    path = "Cache"; // using ./Cache
                }
                else
                {
                    path = Path.Combine(path, "Aardvark", "Cache");
                }
            }

            Report.Line(4, $"Using cache directory: {path}");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            return path;
        });

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

        /// <summary>
        /// Gets or sets the naming scheme used for plugins cache files.
        /// Default scheme is based on assembly version.
        /// </summary>
        public static NamingScheme PluginsCacheFileNaming { get; set; } = NamingScheme.Version;

        /// <summary>
        /// Gets or sets the naming scheme used for introspection cache files.
        /// Default scheme is based on assembly version.
        /// </summary>
        public static NamingScheme IntrospectionCacheFileNaming { get; set; } = NamingScheme.Version;

        internal static string GetIdentifier(this Assembly assembly, NamingScheme scheme)
        {
            return scheme switch
            {
                NamingScheme.Version => assembly.GetName().Version?.ToString() ?? "unknown",
                NamingScheme.Timestamp => assembly.GetLastWriteTimeSafe().ToBinary().ToString(),
                _ => "unknown"
            };
        }
    }
}