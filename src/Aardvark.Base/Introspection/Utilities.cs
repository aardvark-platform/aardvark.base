namespace Aardvark.Base;

using System;
using System.IO;
using System.Reflection;

#if !NET8_0_OR_GREATER
using System.Linq;
#endif

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
}

internal static class DirectoryUtils
{
    public static string[] GetFilesSafe(string directory)
    {
        try
        {
            return Directory.Exists(directory) ? Directory.GetFiles(directory) : [];
        }
        catch (Exception e)
        {
            Report.Warn($"Failed to enumerate files in '{directory}': {e.Message}");
            return [];
        }
    }
}

internal static class PathUtils
{
    public static bool HasDirectoryInformation(string path)
        => path != null && (path.Contains(Path.DirectorySeparatorChar) || path.Contains(Path.AltDirectorySeparatorChar));

    public static string GetDirectoryNameSafe(string path)
    {
        try
        {
            return !string.IsNullOrEmpty(path) ? Path.GetDirectoryName(path) : null;
        }
        catch
        {
            return null;
        }
    }

    public static string GetFileNameSafe(string path)
    {
        try
        {
            return Path.GetFileName(path);
        }
        catch
        {
            return null;
        }
    }

    public static string GetExtensionSafe(string path)
    {
        try
        {
            return Path.GetExtension(path);
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