namespace Aardvark.Base;

using System;
using System.Runtime.InteropServices;

public partial class Aardvark
{
    /// <summary>
    /// Indicates whether the DLL directory is altered temporarily before loading a native library from a file.
    /// The DLL directory is set to the directory of the library so that transitive dependencies in
    /// the same folder can be found. This is usually not required as the dynamic linker should search the directory of
    /// a library for its dependencies. However, some libraries (e.g., IPP) may use another loading mechanism that require
    /// setting the DLL directory explicitly.
    /// </summary>
    /// <remarks>Only works on Windows, disabled by default.</remarks>
    public static bool UseSetDllDirectory { get; set; } = false;

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
        public const int SEM_FAILCRITICALERRORS = 0x0001;

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibrary(string path);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr LoadLibraryEx(string path, IntPtr handle, uint dwFlags);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern IntPtr AddDllDirectory(string path);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetCurrentDirectory(string pathName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern bool SetDllDirectory(string pathName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        public static extern uint GetDllDirectory(uint nBufferLength, [Out] char[] lpPathName);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern IntPtr GetProcAddress(IntPtr handle, string name);

        [DllImport("Kernel32.dll", SetLastError = true, CharSet = CharSet.Ansi)]
        public static extern uint SetErrorMode(uint mode);

        public static int TryGetDllDirectory(out string path)
        {
            char[] buffer;
            uint requiredSize = 128;

            do
            {
                buffer = new char[(int)requiredSize + 1];
                requiredSize = GetDllDirectory((uint)buffer.Length - 1, buffer);
            } while (requiredSize > 0 && requiredSize > buffer.Length);

            if (requiredSize == 0)
            {
                path = null;
                return Marshal.GetLastWin32Error();
            }

            path = new string(buffer, 0, (int)requiredSize);
            return 0;
        }
    }

    private struct DllDirectoryDisposable(string previousPath) : IDisposable
    {
        private bool isActive = true;

        public void Dispose()
        {
            if (isActive)
            {
                if (!Kernel32.SetDllDirectory(previousPath))
                {
                    var error = Marshal.GetLastWin32Error();
                    Report.Line(4, $"Failed to reset DLL directory to '{previousPath}' (error: {error})");
                }
                isActive = false;
            }
        }
    }

    /// <seealso cref="Aardvark.UseSetDllDirectory"/>
    private static DllDirectoryDisposable SetDllDirectory(string libraryPath)
    {
        if (!UseSetDllDirectory || GetOS() != OS.Win32)
        {
            return new DllDirectoryDisposable();
        }

        var directoryPath = PathUtils.HasDirectoryInformation(libraryPath) ? PathUtils.GetDirectoryNameSafe(libraryPath) : null;
        if (string.IsNullOrEmpty(directoryPath))
        {
            return new DllDirectoryDisposable();
        }

        Report.Begin(4, $"Setting DLL directory to '{directoryPath}'");

        try
        {
            var result = Kernel32.TryGetDllDirectory(out var previousPath);
            if (result != 0)
            {
                Report.Line(4, $"Failed to retrieve current DLL directory (error: {result})");
            }
            else if (previousPath != null)
            {
                Report.Line(4, $"Previous path: {previousPath}");
            }

            if (Kernel32.SetDllDirectory(directoryPath))
            {
                return new DllDirectoryDisposable(previousPath);
            }

            var error = Marshal.GetLastWin32Error();
            Report.Line(4, $"Failed to set DLL directory to '{directoryPath}' (error: {error})");
        }
        catch (Exception e)
        {
            Report.Line(4, $"{e.GetType().Name}: {e.Message}");
        }
        finally
        {
            Report.End(4);
        }

        return new DllDirectoryDisposable();
    }
}
