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

#if !NET8_0_OR_GREATER
    // See: https://github.com/dotnet/runtime/blob/main/src/libraries/Common/src/System/IO/PathInternal.Windows.cs
    private static bool IsValidDriveChar(char value)
        => (uint)((value | 0x20) - 'a') <= (uint)('z' - 'a');

    private static bool IsDirectorySeparator(char c)
        => c == Path.DirectorySeparatorChar || c == Path.AltDirectorySeparatorChar;

    private static bool IsPartiallyQualified(string path)
    {
        if (path.Length < 2)
        {
            // It isn't fixed, it must be relative.  There is no way to specify a fixed
            // path with one character (or less).
            return true;
        }

        if (IsDirectorySeparator(path[0]))
        {
            // There is no valid way to specify a relative path with two initial slashes or
            // \? as ? isn't valid for drive relative paths and \??\ is equivalent to \\?\
            return !(path[1] == '?' || IsDirectorySeparator(path[1]));
        }

        // The only way to specify a fixed path that doesn't begin with two slashes
        // is the drive, colon, slash format- i.e. C:\
        return !((path.Length >= 3)
                 && (path[1] == Path.VolumeSeparatorChar)
                 && IsDirectorySeparator(path[2])
                 // To match old behavior we'll check the drive character for validity as the path is technically
                 // not qualified if you don't have a valid drive. "=:\" is the "=" file's default data stream.
                 && IsValidDriveChar(path[0]));
    }

    public static bool IsPathFullyQualified(string path) => !IsPartiallyQualified(path);
#endif
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