using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

#if NET8_0_OR_GREATER
using System.Runtime.Loader;
#endif

namespace Aardvark.Base
{
    public partial class Aardvark
    {
        private static class Kernel32
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

        private static class Dl
        {
            public const int RTLD_LAZY = 0x00001;
            public const int RTLD_NOW = 0x00002;
            public const int RTLD_BINDING_MASK = 0x3;
            public const int RTLD_NOLOAD = 0x00004;
            public const int RTLD_DEEPBIND = 0x00008;
            public const int RTLD_GLOBAL = 0x00100;
            public const int RTLD_LOCAL = 0;
            public const int RTLD_NODELETE = 0x01000;

            [DllImport("libdl", SetLastError = true, CharSet = CharSet.Ansi)]
            public static extern IntPtr dlopen(string path, int flag);

            [DllImport("libdl", SetLastError = true, CharSet = CharSet.Ansi)]
            public static extern IntPtr dlsym(IntPtr handle, string name);
        }

        private static void GetPlatformAndArch(out string platform, out string arch)
        {
            arch = RuntimeInformation.ProcessArchitecture switch
            {
                Architecture.X86 => "x86",
                Architecture.X64 => "AMD64",
                Architecture.Arm => "ARM",
                Architecture.Arm64 => "ARM64",
                _ => "unknown"
            };

            platform = Platform switch
            {
                OS.Win32 => "windows",
                OS.Linux => "linux",
                OS.MacOS => "mac",
                _ => "unknown"
            };
        }

        private static readonly Regex nativeLibraryRx =
            Platform switch
            {
                OS.Win32 => new Regex(@"\.(dll|exe)$", RegexOptions.Compiled),
                OS.Linux => new Regex(@"\.so(\.[0-9\-]+)?$", RegexOptions.Compiled),
                OS.MacOS => new Regex(@"\.dylib$", RegexOptions.Compiled),
                _ => null
            };

        /// <summary>
        /// Returns whether the file path appears to be a native library for the current platform.
        /// </summary>
        private static bool IsNativeLibrary(string path)
            => nativeLibraryRx?.IsMatch(path) ?? false;

        /// <summary>
        /// Unpacks and lists native dependencies of the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to unpack.</param>
        /// <param name="outputDir">The output directory for the native dependencies. Defaults to <see cref="IntrospectionProperties.CurrentEntryPath"/> if null or empty.</param>
        /// <returns>An array of unpacked native library names.</returns>
        public static string[] UnpackAndListNativeDependencies(Assembly assembly, string outputDir = null)
        {
            if (assembly.IsDynamic) return [];
            if (outputDir.IsNullOrEmpty()) outputDir = IntrospectionProperties.CurrentEntryPath;

            Report.Begin(3, $"Unpacking native dependencies for {assembly.FullName}");

            try
            {
                try
                {
                    var info = assembly.GetManifestResourceInfo("native.zip");
                    if (info == null)
                    {
                        Report.Line(3, $"Assembly does not contain native dependencies.");
                        return [];
                    }

                    GetPlatformAndArch(out string platform, out string arch);

                    string[] copyPaths = [ $"{platform}/{arch}/", $"{arch}/" ];
                    List<string> libNames = [];
                    Dictionary<string, string> remap = [];

                    bool TryGetLocalName(string entryName, out string localName)
                    {
                        foreach(var prefix in copyPaths)
                        {
                            if (entryName.StartsWith(prefix))
                            {
                                var name = entryName.Substring(prefix.Length);
#if NET8_0_OR_GREATER
                                var parts = name.Split('/', StringSplitOptions.RemoveEmptyEntries);
#else
                                var parts = name.Split(['/'], StringSplitOptions.RemoveEmptyEntries);
#endif

                                if (parts.Length > 0)
                                {
                                    localName = Path.Combine(parts);
                                    return true;
                                }
                            }
                        }

                        localName = null;
                        return false;
                    }

                    using var s = assembly.GetManifestResourceStream("native.zip");
                    using var archive = new ZipArchive(s);

                    foreach (var e in archive.Entries)
                    {
                        var entryName = e.FullName.Replace(Path.DirectorySeparatorChar, '/');

                        if (entryName == "remap.xml")
                        {
                            using var es = e.Open();
                            var doc = XDocument.Load(es);
                            remap = GetSymlinks(doc);
                        }
                        else if (TryGetLocalName(entryName, out var localName))
                        {
                            var dstPath = Path.Combine(outputDir, localName);
                            var dstDir = Path.GetDirectoryName(dstPath);

                            if (!File.Exists(dstDir)) Directory.CreateDirectory(dstDir);

                            if (!File.Exists(dstPath) || FileUtils.GetLastWriteTimeSafe(dstPath) < e.LastWriteTime.UtcDateTime)
                            {
                                Report.Line(3, $"Unpacking {localName}");
                                e.ExtractToFile(dstPath, true);
                            }

                            if (IsNativeLibrary(localName)) libNames.Add(localName);
                        }
                    }

                    foreach (var kvp in remap)
                    {
                        CreateSymlink(outputDir, kvp.Key, kvp.Value);
                        if (IsNativeLibrary(kvp.Key)) libNames.Add(kvp.Key);
                    }

                    return libNames.ToArray();
                }
                catch (Exception e)
                {
                    Report.Warn($"Could not unpack native dependencies for {assembly.FullName}: {e.Message}");
                    return [];
                }
            }
            finally
            {
                Report.End(3);
            }
        }

        /// <summary>
        /// Unpacks native dependencies of the given assembly.
        /// </summary>
        /// <param name="assembly">The assembly to unpack.</param>
        /// <param name="outputDir">The output directory for the native dependencies. Defaults to <see cref="IntrospectionProperties.CurrentEntryPath"/> if null or empty.</param>
        public static void UnpackNativeDependencies(Assembly assembly, string outputDir)
            => UnpackAndListNativeDependencies(assembly, outputDir);

        /// <summary>
        /// Unpacks native dependencies of the given assembly to <see cref="IntrospectionProperties.CurrentEntryPath"/>.
        /// </summary>
        /// <param name="assembly">The assembly to unpack.</param>
        public static void UnpackNativeDependencies(Assembly assembly)
            => UnpackAndListNativeDependencies(assembly, null);

        [Obsolete("Use UnpackNativeDependencies instead.")]
        public static void UnpackNativeDependenciesToBaseDir(Assembly a, string baseDir)
            => UnpackNativeDependencies(a, baseDir);

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

        private static readonly Lazy<string> s_nativeLibraryCacheDirectory =
            new(() => Path.Combine(CachingProperties.CacheDirectory, "Native"));

        public static readonly string NativeLibraryCacheDirectory = s_nativeLibraryCacheDirectory.Value;

        private static readonly Dictionary<Assembly, string> s_nativePaths = new Dictionary<Assembly, string>();
        private static string[] s_allPaths = null;

        public static string[] GetNativeLibraryPaths()
        {
            lock(s_nativePaths)
            {
                s_allPaths ??= s_nativePaths.Values.Where(Directory.Exists).ToArray();
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

                    using var s = assembly.GetManifestResourceStream("native.zip");
                    string dstFolder = NativeLibraryCacheDirectory;

                    if (SeparateLibraryDirectories)
                    {
#if NET8_0_OR_GREATER
                        var hash = SHA1.HashData(s);
                        var guid = new Guid(hash.AsSpan(0, 16));
#else
                        using var sha1 = SHA1.Create();
                        var hash = sha1.ComputeHash(s);
                        Array.Resize(ref hash, 16);
                        var guid = new Guid(hash);
#endif

                        GetPlatformAndArch(out var platform, out var arch);
                        dstFolder = Path.Combine(dstFolder, assembly.GetName().Name, guid.ToString(), platform, arch);
                    }

                    s_nativePaths[assembly] = dstFolder;
                    s_allPaths = null;
                    path = dstFolder;
                    return true;
                }
            }
        }

        private static readonly Dictionary<(Assembly, string), string> s_cache = new Dictionary<(Assembly, string), string>();

        private static bool TryLoadNativeLibrary(string path, out IntPtr handle)
        {
#if NETCOREAPP3_1_OR_GREATER
            return NativeLibrary.TryLoad(path, out handle);
#else
            handle = (Platform == OS.Win32) ? Kernel32.LoadLibrary(path) : Dl.dlopen(path, 1);
            return handle != IntPtr.Zero;
#endif
        }

        private static bool TryLoadNativeLibrary(Assembly assembly, string name, out IntPtr handle)
        {
#if NETCOREAPP3_1_OR_GREATER
            return (assembly != null)
                ? NativeLibrary.TryLoad(name, assembly, null, out handle)
                : NativeLibrary.TryLoad(name, out handle);
#else
            return TryLoadNativeLibrary(name, out handle);
#endif
        }

        public static IntPtr LoadLibrary(Assembly assembly, string nativeName)
        {
            lock (s_cache)
            {
                if(s_cache.TryGetValue((assembly, nativeName), out var path))
                {
                    return TryLoadNativeLibrary(path, out var handle) ? handle : IntPtr.Zero;
                }
            }

            IntPtr ptr = IntPtr.Zero;
            string probe = null;
            var paths = new List<string>(capacity: 1);

            try
            {
                var location = Path.GetDirectoryName(assembly?.GetLocationSafe()) ?? IntrospectionProperties.CurrentEntryPath;
                if (location != null)
                    paths.Add(location);
            }
            catch
            {
            }

            try
            {
                string[] formats = [];

                if (Platform == OS.Linux) formats = ["{0}.so", "lib{0}.so", "lib{0}.so.1"];
                else if (Platform == OS.Win32) formats = ["{0}.dll"];
                else if (Platform == OS.MacOS) formats = ["{0}.dylib", "lib{0}.dylib"];

                if (assembly != null)
                {
                    if (TryGetNativeLibraryPath(assembly, out var dstFolder))
                    {
                        paths.Add(dstFolder);
                    }
                    else
                    {
                        paths.AddRange(GetNativeLibraryPaths());
                    }
                }
                else
                {
                    paths.AddRange(GetNativeLibraryPaths());
                }

                var realName = Path.GetFileNameWithoutExtension(nativeName);
                Report.Begin(4, "probing paths for {0}", realName);
                foreach(var path in paths)
                {
                    Report.Line(4, "{0}", path);
                }
                Report.End(4);

                // NOTE: IPP will try to load other IPP native libs internally (not triggering the ResolvingUnmanagedDll callback) and show a message box if it fails -> make sure native dependencies between them can be resolved
                //        - for some reason SetDllDirectory is required even LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR is used that will include the directory of the library that should be loaded
                //        - for some other reason AddDllDirectory does not help loading libraries, even it should provide similar behavior than SetDllDirectory
                // NOTE++: We don't use IPP anymore, this was broken anyway because the assembly name was not "IPP" anymore for some reason.
                //var setDllDir = os == OS.Win32 && assembly != null && assembly.GetName().Name == "IPP";

                foreach (var fmt in formats)
                {
                    var libName = string.Format(fmt, realName);

                    // probe all paths.
                    foreach (var p in paths)
                    {
                        if (!Directory.Exists(p))
                            continue;

                        if (FileUtils.TryFindInDirectory(p, libName, out var libPath))
                        {
                            probe = libPath;
                            if (TryLoadNativeLibrary(libPath, out ptr)) return ptr;
                        }
                    }

                    // try the plain loading mechanism.
                    probe = libName;
                    if (TryLoadNativeLibrary(assembly, libName, out ptr)) return ptr;
                }

                // try the standard loading mechanism as a last resort.
                probe = nativeName;
                return TryLoadNativeLibrary(assembly, nativeName, out ptr) ? ptr : IntPtr.Zero;
            }
            catch (Exception e)
            {
                Report.Warn($"Could not load native library {nativeName}: {e.Message}");
                return IntPtr.Zero;
            }
            finally
            {
                if (ptr == IntPtr.Zero)
                {
                    Report.Warn($"Could not load native library {nativeName} ({probe})");
                }
                else
                {
                    lock (s_cache) { s_cache[(assembly, nativeName)] = probe; }
                    Report.Line(4, "[Introspection] loaded native library {0} from {1}", nativeName, probe);
                }
            }
        }

        public static IntPtr GetProcAddress(IntPtr handle, string name)
        {
            if (handle == IntPtr.Zero) return IntPtr.Zero;

#if NETCOREAPP3_1_OR_GREATER
            if (NativeLibrary.TryGetExport(handle, name, out var ptr)) return ptr;
            return IntPtr.Zero;
#else
            if (Platform == OS.Win32) return Kernel32.GetProcAddress(handle, name);
            return Dl.dlsym(handle, name);
#endif
        }

        public static void LoadNativeDependencies(Assembly a)
        {
            if (TryGetNativeLibraryPath(a, out string dstFolder))
            {
                try
                {
                    Report.BeginTimed(3, $"Loading native dependencies for {a.FullName}");
                    try
                    {
                        var libNames = UnpackAndListNativeDependencies(a, dstFolder);

#if NETCOREAPP3_1_OR_GREATER
                        NativeLibrary.SetDllImportResolver(a, (name, ass, searchPath) =>
                        {
                            return LoadLibrary(ass, name);
                        });
#else
                        var isWindows = Platform == OS.Win32;

                        // NOTE: libraries such as IPP are spread over multiple dlls that will now get loaded in a nondeterministic order -> make sure native dependencies between them can be resolved
                        //        - for some reason SetDllDirectory is required even LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR is used that will include the directory of the library that should be loaded
                        //        - for some other reason AddDllDirectory does not help loading libraries, even it should provide similar behavior than SetDllDirectory
                        if (isWindows)
                            Kernel32.SetDllDirectory(dstFolder);

                        foreach (var file in libNames)
                        {
                            try
                            {
                                IntPtr ptr;
                                var filePath = Path.Combine(dstFolder, file);
                                if (isWindows)
                                    ptr = Kernel32.LoadLibraryEx(filePath, IntPtr.Zero, Kernel32.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | Kernel32.LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR);
                                else
                                    ptr = Dl.dlopen(filePath, 0x01102);

                                if (ptr == IntPtr.Zero)
                                {
                                    if (isWindows)
                                    {
                                        var err = Kernel32.GetLastError();
                                        Report.Warn($"Could not load native library {filePath} (Error = {err})");
                                    }
                                    else
                                    {
                                        Report.Warn($"Could not load native library {filePath}");
                                    }
                                }
                                else
                                {
                                    Report.Line(3, $"Loaded {file}");
                                }
                            }
                            catch (Exception ex)
                            {
                                Report.Warn($"Could not load native library {file}: {ex.Message}");
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
                    Report.Warn($"Could not load native dependencies for {a.FullName}: {e.Message}");
                }
            }
        }
    }
}