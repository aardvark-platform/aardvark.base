using System;
using System.IO;
using System.Reflection;

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
                return Path.GetDirectoryName(path);
            }
            catch
            {
                return null;
            }
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
            catch
            {
                return null;
            }
        }

        public static bool HasLocation(this Assembly assembly)
            => assembly.GetLocationSafe() != null;

        public static DateTime GetLastWriteTimeSafe(this Assembly assembly)
        {
            var location = assembly?.GetLocationSafe() ?? IntrospectionProperties.CurrentEntryBundle;
            return FileUtils.GetLastWriteTimeSafe(location);
        }
    }
}