namespace Aardvark.Base;

using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Xml.Linq;

public partial class Aardvark
{
    private static readonly Lazy<string> s_nativeLibraryCacheDirectory =
        new(() => Path.Combine(CachingProperties.CacheDirectory, "Native"));

    /// <summary>
    /// Directory containing unpacked native dependencies.
    /// </summary>
    public static readonly string NativeLibraryCacheDirectory = s_nativeLibraryCacheDirectory.Value;

    /// Specify if native libraries should be extracted each to its own sub folder or directory to the NativeLibraryPath
    /// NOTE: When using global shared NativeLibraryPath, SeparateLibraryDirectories should not be set to false, as there might be version conflicts
    public static bool SeparateLibraryDirectories = true;

    #region Dependencies Unpacking

    private static readonly Dictionary<Assembly, string> s_nativeDependenciesDirectory = new();
    private static readonly Dictionary<Assembly, string[]> s_nativeLibraryPaths = new();

    /// <summary>
    /// Returns whether the file path appears to be a native library for the current platform.
    /// </summary>
    private static bool IsNativeLibrary(string path)
        => RegexPatterns.NativeLibrary.Extension.IsMatch(path);

    private static string PlatformString(OSPlatform platform)
    {
        return platform == OSPlatform.Windows ? "windows" :
               platform == OSPlatform.OSX     ? "mac" :
               platform == OSPlatform.Linux   ? "linux" :
               "unknown";
    }

    private static string ArchitectureString(Architecture arch) =>
        arch switch
        {
            Architecture.X86   => "x86",
            Architecture.X64   => "AMD64",
            Architecture.Arm   => "ARM",
            Architecture.Arm64 => "ARM64",
            _                  => arch.ToString()
        };

    #region DllMap

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

                    if(!string.IsNullOrWhiteSpace(srcStr) && !string.IsNullOrWhiteSpace(dstStr) && TryParseOS(osStr, out var osVal))
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

#if NET8_0_OR_GREATER
    private static void CreateSymlink(string src, string dst)
        => File.CreateSymbolicLink(src, dst);
#else
    [DllImport("libc")]
    private static extern int symlink(string oldpath, string newpath);

    private static void CreateSymlink(string src, string dst)
    {
        var ret = symlink(dst, src);
        if (ret != 0) throw new Exception($"symlink() failed (exit code = {ret})");
    }
#endif

    private static void CreateSymlink(string baseDir, string src, string dst)
    {
        var os = GetOS();
        if (os is OS.Win32 or OS.Unknown) return;

        Report.Begin(3, $"Creating symlink '{src}' -> '{dst}'");

        try
        {
            var srcPath = Path.Combine(baseDir, src);
            Report.Line(3, $"Directory: {baseDir}");

            var dstPath = os is OS.Linux && LdConfig.TryGetPath(dst, out string resolvedPath) ? resolvedPath : Path.Combine(baseDir, dst);
            Report.Line(3, $"Target path: {dstPath}");

            if (!File.Exists(dstPath))
            {
                Report.End(3, " - target not found");
                return;
            }

            if (File.Exists(srcPath))
            {
                File.Delete(srcPath);
            }
            else
            {
                var srcDir = PathUtils.GetDirectoryNameSafe(srcPath);
                if (!Directory.Exists(srcDir)) Directory.CreateDirectory(srcDir);
            }

            CreateSymlink(srcPath, dstPath);
            Report.End(3, " - success");
        }
        catch (Exception e)
        {
            Report.Line(3, $"{e.GetType().Name}: {e.Message}");
            Report.End(3, " - failure");
        }
    }

    #endregion

    /// <summary>
    /// Extracts native libraries embedded in the specified assembly and returns their relative file names.
    /// </summary>
    /// <param name="assembly">The assembly whose embedded native dependencies should be unpacked. Dynamic assemblies are ignored.</param>
    /// <param name="platform">The target operating system platform used to select the appropriate files from the archive.</param>
    /// <param name="architecture">The target CPU architecture used to select the appropriate files from the archive.</param>
    /// <param name="outputDir">Destination directory where files are extracted. If <c>null</c> or empty, defaults to <see cref="IntrospectionProperties.CurrentEntryPath"/>.</param>
    /// <returns>
    /// An array of relative paths (using the archive's internal folder structure) of the native libraries that were present
    /// for the given platform/architecture combination. The returned paths are relative to <paramref name="outputDir"/>.
    /// </returns>
    public static string[] UnpackAndListNativeDependencies(Assembly assembly, OSPlatform platform, Architecture architecture, string outputDir = null)
    {
        if (assembly.IsDynamic) return [];
        if (outputDir.IsNullOrEmpty()) outputDir = IntrospectionProperties.CurrentEntryPath;

        Report.Begin(3, $"Unpacking native dependencies for {assembly.FullName}");

        try
        {
            Report.Line(3, $"Output directory: {outputDir}");

            var info = assembly.GetManifestResourceInfo("native.zip");
            if (info == null)
            {
                Report.Line(3, $"Assembly does not contain native dependencies.");
                return [];
            }

            var platformString = PlatformString(platform);
            var architectureString = ArchitectureString(architecture);

            string[] copyPaths = [ $"{platformString}/{architectureString}/", $"{architectureString}/" ];
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

            using var stream = assembly.GetManifestResourceStream("native.zip");
            using var archive = new ZipArchive(stream);

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
                    var dstDir = PathUtils.GetDirectoryNameSafe(dstPath);

                    if (!File.Exists(dstDir)) Directory.CreateDirectory(dstDir);

                    if (!File.Exists(dstPath) || FileUtils.GetLastWriteTimeSafe(dstPath) < e.LastWriteTime.UtcDateTime)
                    {
                        Report.Line(3, $"Unpacking: {localName}");
                        e.ExtractToFile(dstPath, true);
                    }
                    else
                    {
                        Report.Line(3, $"Found: {localName}");
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
        finally
        {
            Report.End(3);
        }
    }

    /// <summary>
    /// Extracts native libraries embedded in <paramref name="assembly"/> for the current OS platform and process architecture.
    /// </summary>
    /// <param name="assembly">The assembly whose embedded native dependencies should be unpacked.</param>
    /// <param name="outputDir">Destination directory where files are extracted. If <c>null</c> or empty, defaults to <see cref="IntrospectionProperties.CurrentEntryPath"/>.</param>
    /// <returns>
    /// An array of relative paths of the native libraries that were present for the current platform and architecture.
    /// The paths are relative to <paramref name="outputDir"/>.
    /// </returns>
    public static string[] UnpackAndListNativeDependencies(Assembly assembly, string outputDir = null)
        => UnpackAndListNativeDependencies(assembly, GetOSPlatform(), RuntimeInformation.ProcessArchitecture, outputDir);

    /// <summary>
    /// Extracts native libraries embedded in <paramref name="assembly"/> for the specified platform and architecture.
    /// </summary>
    /// <param name="assembly">The assembly whose embedded native dependencies should be unpacked.</param>
    /// <param name="platform">The target operating system platform used to select the appropriate files from the archive.</param>
    /// <param name="architecture">The target CPU architecture used to select the appropriate files from the archive.</param>
    /// <param name="outputDir">Destination directory where files are extracted. If <c>null</c> or empty, defaults to <see cref="IntrospectionProperties.CurrentEntryPath"/>.</param>
    public static void UnpackNativeDependencies(Assembly assembly, OSPlatform platform, Architecture architecture, string outputDir = null)
        => UnpackAndListNativeDependencies(assembly, platform, architecture, outputDir);

    /// <summary>
    /// Extracts native libraries embedded in <paramref name="assembly"/> for the current OS platform and process architecture.
    /// </summary>
    /// <param name="assembly">The assembly whose embedded native dependencies should be unpacked.</param>
    /// <param name="outputDir">Destination directory where files are extracted. If <c>null</c> or empty, defaults to <see cref="IntrospectionProperties.CurrentEntryPath"/>.</param>
    public static void UnpackNativeDependencies(Assembly assembly, string outputDir)
        => UnpackAndListNativeDependencies(assembly, outputDir);

    /// <summary>
    /// Extracts native libraries embedded in <paramref name="assembly"/> to <see cref="IntrospectionProperties.CurrentEntryPath"/>
    /// using the current OS platform and process architecture.
    /// </summary>
    /// <param name="assembly">The assembly whose embedded native dependencies should be unpacked.</param>
    public static void UnpackNativeDependencies(Assembly assembly)
        => UnpackAndListNativeDependencies(assembly);

    /// <summary>
    /// Returns the paths of the directories containing native dependencies of all assemblies.
    /// </summary>
    public static string[] GetNativeDependenciesDirectories()
    {
        lock(s_nativeDependenciesDirectory)
        {
            return s_nativeDependenciesDirectory.Values.Where(Directory.Exists).ToArray();
        }
    }

    /// <summary>
    /// Returns the path of the directory containing native dependencies of the given assembly.
    /// </summary>
    /// <param name="assembly">Assembly with native dependencies.</param>
    /// <param name="path">Returns the path of the directory, if the assembly has native dependencies.</param>
    /// <returns>True if the assembly has native dependencies, false otherwise.</returns>
    public static bool TryGetNativeDependenciesDirectory(Assembly assembly, out string path)
    {
        var name = assembly?.GetName().Name;
        if (assembly == null || assembly.IsDynamic || string.IsNullOrEmpty(name))
        {
            path = null;
            return false;
        }

        lock (s_nativeDependenciesDirectory)
        {
            if (s_nativeDependenciesDirectory.TryGetValue(assembly, out path))
            {
                return path != null;
            }

            var info = assembly.GetManifestResourceInfo("native.zip");
            if (info == null)
            {
                s_nativeDependenciesDirectory[assembly] = null;
                return false;
            }

            using var stream = assembly.GetManifestResourceStream("native.zip");
            string dstFolder = NativeLibraryCacheDirectory;

            if (SeparateLibraryDirectories)
            {
#if NET8_0_OR_GREATER
                var hash = SHA1.HashData(stream);
                var guid = new Guid(hash.AsSpan(0, 16));
#else
                using var sha1 = SHA1.Create();
                var hash = sha1.ComputeHash(stream);
                Array.Resize(ref hash, 16);
                var guid = new Guid(hash);
#endif

                var platform = PlatformString(GetOSPlatform());
                var arch = ArchitectureString(RuntimeInformation.ProcessArchitecture);
                dstFolder = Path.Combine(dstFolder, name, guid.ToString(), platform, arch);
            }

            s_nativeDependenciesDirectory[assembly] = dstFolder;
            path = dstFolder;
            return true;
        }
    }

    private static bool ShouldUnpackNativeDependencies(Assembly assembly)
    {
        try
        {
            return IntrospectionProperties.UnpackNativeLibrariesFilter(assembly);
        }
        catch (Exception e)
        {
            Report.Warn($"Error while invoking UnpackNativeLibrariesFilter for {assembly.FullName}: {e.Message}");
            return false;
        }
    }

    public static void LoadNativeDependencies(Assembly assembly)
    {
        if (assembly == null) return;

        if (!ShouldUnpackNativeDependencies(assembly))
        {
            Report.Line(4, $"Skipped loading native dependencies for {assembly.FullName}");
            return;
        }

        if (TryGetNativeDependenciesDirectory(assembly, out string dstFolder))
        {
            try
            {
                Report.BeginTimed(3, $"Loading native dependencies for {assembly.FullName}");

                var libraryNames = UnpackAndListNativeDependencies(assembly, dstFolder);
                var libraryPaths = new List<string>(capacity: libraryNames.Length);

                lock (s_nativeLibraryPaths)
                {
                    foreach (var libraryName in libraryNames)
                    {
                        var path = Path.Combine(dstFolder, libraryName);
                        if (File.Exists(path)) libraryPaths.Add(path);
                    }

                    s_nativeLibraryPaths[assembly] = libraryPaths.ToArray();
                }

#if !NETCOREAPP3_1_OR_GREATER
                // For .NET Standard 2.0, we don't have access to the ResolvingUnmanagedDll event (see Aardvark.Init())
                // Instead we load all the native libraries preemptively after unpacking.
                // Caveat: This does not work with symlinks (on Linux at least), but there is no reason to use the .NET Standard 2.0 build
                // for non-Windows platforms anyway.
                foreach (var libraryPath in libraryPaths)
                {
                    LoadLibrary(assembly, libraryPath, resolving: false, global: true);
                }
#endif
            }
            catch (Exception e)
            {
                Report.Warn($"Could not load native dependencies for {assembly.FullName}: {e.Message}");
            }
            finally
            {
                Report.EndTimed(3);
            }
        }
    }

    #endregion

    #region Library Loading

    private static string NativeLibraryExtension { get; } =
        GetOS() switch
        {
            OS.Win32 => ".dll",
            OS.MacOS => ".dylib",
            _ => ".so"
        };


    private static bool TryLoadNativeLibrary(string libraryName, bool global, out IntPtr handle)
    {
#if NETCOREAPP3_1_OR_GREATER
        return NativeLibrary.TryLoad(libraryName, out handle);
#else
        try
        {
            if (GetOS() == OS.Win32)
            {
                var flags = Kernel32.LOAD_LIBRARY_SEARCH_DEFAULT_DIRS | Kernel32.LOAD_LIBRARY_SEARCH_DLL_LOAD_DIR;
                handle = Kernel32.LoadLibraryEx(libraryName, IntPtr.Zero, flags);
                if (handle != IntPtr.Zero) return true;
                var error = Marshal.GetLastWin32Error();
                Report.Line(3, $"Kernel32.LoadLibrary failed (error: {error})");
            }
            else
            {
                var flags = global ? Dl.RTLD_NODELETE | Dl.RTLD_GLOBAL | Dl.RTLD_NOW : Dl.RTLD_LAZY;
                handle = Dl.dlopen(libraryName, flags);
            }
        }
        catch (Exception e)
        {
            Report.Line(3, $"{e.GetType().Name}: {e.Message}");
            handle = IntPtr.Zero;
        }

        return handle != IntPtr.Zero;
#endif
    }

    private static IEnumerable<string> GetNativeLibraryPaths(Assembly assembly)
    {
        var result = new List<string>(capacity: 1);

        // Search in current / assembly directory
        var currentDirectory = PathUtils.GetDirectoryNameSafe(assembly?.GetLocationSafe()) ?? IntrospectionProperties.CurrentEntryPath;
        result.AddRange(DirectoryUtils.GetFilesSafe(currentDirectory));

        lock (s_nativeLibraryPaths)
        {
            // Probe extracted native libraries
            if (assembly != null && s_nativeLibraryPaths.TryGetValue(assembly, out var filePathsForAssembly))
            {
                result.AddRange(filePathsForAssembly);
            }

            // Probe extracted native libraries for other assemblies
            foreach (var filePaths in s_nativeLibraryPaths.Values)
                result.AddRange(filePaths);
        }

        return result.Distinct();
    }

    private static readonly Dictionary<(Assembly, string), string> s_nativeLibraryCache = new();

    private static IntPtr LoadLibrary(Assembly assembly, string libraryNameOrPath, bool resolving, bool global)
    {
        if (string.IsNullOrEmpty(libraryNameOrPath)) return IntPtr.Zero;
        assembly ??= IntrospectionProperties.CurrentEntryAssembly ?? Assembly.GetExecutingAssembly();

        IntPtr ptr = IntPtr.Zero;

        bool TryLoad(string probe)
        {
            using var _ = SetDllDirectory(probe);

            if (TryLoadNativeLibrary(probe, global, out ptr))
            {
                Report.Line(4, $"Loaded: {probe}");
                try { lock (s_nativeLibraryCache) { s_nativeLibraryCache[(assembly, libraryNameOrPath)] = probe; } } catch {}
                return true;
            }

            Report.Line(4, $"Failed to load: {probe}");
            return false;
        }

        Report.Begin(3, $"Loading native library '{libraryNameOrPath}'");
        Report.Line(4, $"Assembly: {assembly}");

        try
        {
            // If the input is a path (i.e., it has a directory part) just load as is.
            if (PathUtils.HasDirectoryInformation(libraryNameOrPath))
            {
                if (!resolving) TryLoad(libraryNameOrPath);
                return ptr;
            }

            // Lookup input in cache
            Report.Begin(4, "Looking up in cache");

            try
            {
                lock (s_nativeLibraryCache)
                {
                    var inCache = s_nativeLibraryCache.TryGetValue((assembly, libraryNameOrPath), out var path);
                    if(inCache && File.Exists(path) && TryLoad(path)) return ptr;
                }
            }
            finally
            {
                Report.End(4);
            }

            // Determine file names of library to search for
            // If the file name has an unknown or no extension, we add the default library extenion for the current platform
            var libraryFileNames = new List<string>(capacity: 4)
            {
                libraryNameOrPath,
                $"lib{libraryNameOrPath}"
            };

            if (!string.Equals(PathUtils.GetExtensionSafe(libraryNameOrPath), NativeLibraryExtension, StringComparison.OrdinalIgnoreCase))
            {
                libraryFileNames.Add($"{libraryNameOrPath}{NativeLibraryExtension}");
                libraryFileNames.Add($"lib{libraryNameOrPath}{NativeLibraryExtension}");
            }

            // Search in known locations (i.e., entry directory and extracted native directories).
            Report.Begin(4, "Searching library directories");

            try
            {
                foreach (var filePath in GetNativeLibraryPaths(assembly))
                {
                    var fileName = PathUtils.GetFileNameSafe(filePath);

                    foreach (var libraryFileName in libraryFileNames)
                    {
                        if (string.Equals(fileName, libraryFileName, StringComparison.OrdinalIgnoreCase))
                        {
                            if (File.Exists(filePath) && TryLoad(filePath)) return ptr;
                        }
                    }
                }
            }
            finally
            {
                Report.End(4);
            }

            // Did not find the library in any of the search directories.
            // Try to load using the dynamic linker, i.e., only using the library name (and variants) instead of a path.
            Report.Begin(4, "Using dynamic linker");

            try
            {
                foreach (var libraryFileName in libraryFileNames)
                {
                    if (resolving && libraryFileName == libraryNameOrPath) continue;
                    if (TryLoad(libraryFileName)) return ptr;
                }
            }
            finally
            {
                Report.End(4);
            }

            // On Linux, as a final resort, we go over all the known libraries as returned by `ldconfig` and look for
            // the library ignoring version numbers.
            if (GetOS() == OS.Linux)
            {
                var libraryName = RegexPatterns.NativeLibrary.Extension.Replace(libraryNameOrPath, "");
                if (!string.IsNullOrEmpty(libraryName))
                {
                    Report.Begin(4, "Using ldconfig ignoring version numbers");

                    try
                    {
                        foreach (var filePath in LdConfig.AllPaths)
                        {
                            var fileName = PathUtils.GetFileNameSafe(filePath);

                            if (!string.IsNullOrEmpty(fileName))
                            {
                                fileName = RegexPatterns.NativeLibrary.Extension.Replace(fileName, "");

                                if (string.Equals(fileName, libraryName, StringComparison.OrdinalIgnoreCase) ||
                                    string.Equals(fileName, $"lib{libraryName}", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (File.Exists(filePath) && TryLoad(filePath)) return ptr;
                                }
                            }
                        }
                    }
                    finally
                    {
                        Report.End(4);
                    }
                }
            }

            return IntPtr.Zero;
        }
        catch (Exception e)
        {
            Report.Warn($"{e.GetType().Name} occurred while loading native library '{libraryNameOrPath}': {e.Message}");
            return IntPtr.Zero;
        }
        finally
        {
            Report.End(3, ptr != IntPtr.Zero ? $" - success" : " - failed");
        }
    }

    /// <summary>
    /// Loads the specified native library; the input can be a name or a file path.
    /// </summary>
    /// <remarks>
    /// If <paramref name="libraryNameOrPath"/> is a path (i.e., it contains a directory separator) the
    /// platform's dynamic linker is invoked directly with that value. Otherwise, the function
    /// tries to locate the library by:
    /// <list type="number">
    /// <item>Searching the directory where the assembly is located.</item>
    /// <item>Searching the directory of the native dependencies of the assembly.</item>
    /// <item>Searching the directories of the native dependencies of all the other assemblies.</item>
    /// <item>Invoking the dynamic linker.</item>
    /// <item>Resolving the path by using `ldconfig` ignoring version information (Linux only).</item>
    /// </list>
    /// Library name comparison is case-insensitive and variants ('lib' prefix, platform-specific extension) are considered.
    /// </remarks>
    /// <param name="libraryNameOrPath">Name or file path of the library to load.</param>
    /// <param name="assembly">Assembly loading the native library. If <c>null</c> the calling assembly is used.</param>
    /// <returns>Handle of the loaded library on success, <see cref="IntPtr.Zero"/> otherwise.</returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static IntPtr LoadLibrary(string libraryNameOrPath, Assembly assembly = null)
        => LoadLibrary(assembly ?? Assembly.GetCallingAssembly(), libraryNameOrPath, resolving: false, global: false);

    public static IntPtr GetProcAddress(IntPtr handle, string name)
    {
        if (handle == IntPtr.Zero) return IntPtr.Zero;

#if NETCOREAPP3_1_OR_GREATER
        return NativeLibrary.TryGetExport(handle, name, out var ptr) ? ptr : IntPtr.Zero;
#else
        return GetOS() == OS.Win32 ? Kernel32.GetProcAddress(handle, name) : Dl.dlsym(handle, name);
#endif
    }

    #endregion

    #region  Obsolete

    [Obsolete("Use UnpackNativeDependencies instead.")]
    public static void UnpackNativeDependenciesToBaseDir(Assembly a, string baseDir)
        => UnpackNativeDependencies(a, baseDir);

    [Obsolete("using explicit native library path is no longer possble https://github.com/aardvark-platform/aardvark.base/issues/64")]
    public static string NativeLibraryPath = Path.Combine(Path.GetTempPath(), "aardvark-native");

    [Obsolete("Use GetNativeDependenciesDirectories instead.")]
    public static string[] GetNativeLibraryPaths()
        => GetNativeDependenciesDirectories();

    [Obsolete("Use TryGetNativeDependenciesDirectory instead.")]
    public static bool TryGetNativeLibraryPath(Assembly assembly, out string path)
        => TryGetNativeDependenciesDirectory(assembly, out path);

    [MethodImpl(MethodImplOptions.NoInlining)]
    [Obsolete("Use overload with optional 'assembly' parameter instead.")]
    public static IntPtr LoadLibrary(Assembly assembly, string libraryNameOrPath)
        => LoadLibrary(assembly ?? Assembly.GetCallingAssembly(), libraryNameOrPath, resolving: false, global: false);

    #endregion
}