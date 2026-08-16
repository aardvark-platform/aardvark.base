using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Aardvark.Base.Coder
{
    public static class Dir
    {
        private static readonly StringComparer s_pathComparer =
            Path.DirectorySeparatorChar == '\\'
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal;

        private static readonly char[] s_pathSeparators =
        {
            Path.DirectorySeparatorChar,
            Path.AltDirectorySeparatorChar
        };

        private static string NormalizeRoot(string root)
        {
            if (Path.AltDirectorySeparatorChar != Path.DirectorySeparatorChar)
                root = root.Replace(Path.AltDirectorySeparatorChar, Path.DirectorySeparatorChar);

            var length = root.Length;
            while (length > 1 && root[length - 1] == Path.DirectorySeparatorChar)
                length--;

            return length == root.Length ? root : root.Substring(0, length);
        }

        private static string[] GetPathComponents(string path, out string root)
        {
            var fullPath = Path.GetFullPath(path);
            var pathRoot = Path.GetPathRoot(fullPath) ?? string.Empty;
            root = NormalizeRoot(pathRoot);

            return fullPath
                .Substring(pathRoot.Length)
                .Split(s_pathSeparators, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Computes a lexical path from <paramref name="relativeToDir"/> to
        /// <paramref name="absoluteDir"/>.
        /// </summary>
        /// <param name="absoluteDir">The directory where the result should point at.</param>
        /// <param name="relativeToDir">The directory from where the result should start from.</param>
        /// <returns>
        /// The relative path, or <c>null</c> when the normalized path roots are incompatible.
        /// Equal roots and components are compared ordinally and case-insensitively on Windows,
        /// and ordinally and case-sensitively on other platforms. Identical directories return
        /// an empty string; every non-empty result ends with
        /// <see cref="Path.DirectorySeparatorChar"/>.
        /// </returns>
        public static string RelativeDir(string absoluteDir, string relativeToDir)
        {
            return RelativeDir(new DirectoryInfo(absoluteDir), new DirectoryInfo(relativeToDir));
        }

        /// <summary>
        /// Computes a lexical path from <paramref name="relativeToDir"/> to
        /// <paramref name="absoluteDir"/>.
        /// </summary>
        /// <param name="absoluteDir">The directory where the result should point at.</param>
        /// <param name="relativeToDir">The directory from where the result should start from.</param>
        /// <returns>
        /// The relative path, or <c>null</c> when the normalized path roots are incompatible.
        /// Equal roots and components are compared ordinally and case-insensitively on Windows,
        /// and ordinally and case-sensitively on other platforms. Identical directories return
        /// an empty string; every non-empty result ends with
        /// <see cref="Path.DirectorySeparatorChar"/>.
        /// </returns>
        public static string RelativeDir(DirectoryInfo absoluteDir, DirectoryInfo relativeToDir)
        {
            var absoluteDirectories = GetPathComponents(absoluteDir.FullName, out var absoluteRoot);
            var relativeDirectories = GetPathComponents(relativeToDir.FullName, out var relativeRoot);

            if (!s_pathComparer.Equals(absoluteRoot, relativeRoot))
                return null;

            var commonCount = 0;
            var commonLimit = Math.Min(absoluteDirectories.Length, relativeDirectories.Length);
            while (commonCount < commonLimit &&
                   s_pathComparer.Equals(absoluteDirectories[commonCount], relativeDirectories[commonCount]))
            {
                commonCount++;
            }

            var relativeComponents = new List<string>(
                relativeDirectories.Length - commonCount + absoluteDirectories.Length - commonCount);

            for (var index = commonCount; index < relativeDirectories.Length; index++)
                relativeComponents.Add("..");

            for (var index = commonCount; index < absoluteDirectories.Length; index++)
                relativeComponents.Add(absoluteDirectories[index]);

            if (relativeComponents.Count == 0)
                return string.Empty;

            return string.Join(Path.DirectorySeparatorChar.ToString(), relativeComponents) +
                   Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Computes a lexical file path from <paramref name="relativeTo"/> to
        /// <paramref name="absoluteFile"/>.
        /// </summary>
        /// <param name="absoluteFile">The file where the result should point at.</param>
        /// <param name="relativeTo">The directory from where the result should start from.</param>
        /// <returns>
        /// The relative file path, or <c>null</c> when the normalized path roots are incompatible.
        /// Directory components use ordinal case-insensitive comparison on Windows and ordinal
        /// case-sensitive comparison on other platforms.
        /// </returns>
        public static string RelativeFile(FileInfo absoluteFile, DirectoryInfo relativeTo)
        {
            var relativeDir = RelativeDir(absoluteFile.Directory, relativeTo);
            return relativeDir == null ? null : relativeDir + absoluteFile.Name;
        }

        /// <summary>
        /// Computes a lexical file path from <paramref name="relativeTo"/> to
        /// <paramref name="absoluteFile"/>.
        /// </summary>
        /// <param name="absoluteFile">The file where the result should point at.</param>
        /// <param name="relativeTo">The directory from where the result should start from.</param>
        /// <returns>
        /// The relative file path, or <c>null</c> when the normalized path roots are incompatible.
        /// Directory components use ordinal case-insensitive comparison on Windows and ordinal
        /// case-sensitive comparison on other platforms.
        /// </returns>
        public static string RelativeFile(string absoluteFile, string relativeTo)
        {
            return RelativeFile(new FileInfo(absoluteFile), new DirectoryInfo(relativeTo));
        }

        /// <summary>
        /// Returns the lexical relative directory path when the roots are compatible. Otherwise,
        /// returns <paramref name="absoluteDir"/> when <paramref name="useAbsolutePathsIfNecessary"/>
        /// is true, or <see cref="string.Empty"/> when it is false.
        /// </summary>
        public static string TryGetRelativeDir(string absoluteDir, string relativeTo, bool useAbsolutePathsIfNecessary)
        {
            if (string.IsNullOrEmpty(absoluteDir))
                return String.Empty;

            var relDirName = Dir.RelativeDir(absoluteDir, relativeTo);

            // if no relative filename was found, store the absolute path (if not disabled)
            if (relDirName == null)
            {
                if (useAbsolutePathsIfNecessary)
                    relDirName = absoluteDir;
                else
                    relDirName = String.Empty;
            }

            return relDirName;
        }

        /// <summary>
        /// Returns the lexical relative file path when the roots are compatible. Otherwise,
        /// returns <paramref name="absoluteFile"/> when <paramref name="useAbsolutePathsIfNecessary"/>
        /// is true, or <see cref="string.Empty"/> when it is false.
        /// </summary>
        public static string TryGetRelativeFileName(string absoluteFile, string relativeTo, bool useAbsolutePathsIfNecessary)
        {
            if (string.IsNullOrEmpty(absoluteFile))
                return String.Empty;

            var relFileName = Dir.RelativeFile(absoluteFile, relativeTo);

            // if no relative filename was found, store the absolute path (if not disabled)
            if (relFileName == null)
            {
                if (useAbsolutePathsIfNecessary)
                    relFileName = absoluteFile;
                else
                    relFileName = String.Empty;
            }

            return relFileName;
        }

        public static string GetAbsoluteFileName(string fileName, string rootPath, bool useAbsolutePathsIfNecessary = true)
        {
            if (string.IsNullOrEmpty(fileName))
                return String.Empty;

            // check if fileName is relative
            if (!Path.IsPathRooted(fileName))
                return Path.Combine(rootPath, fileName);
            else if (useAbsolutePathsIfNecessary)
                return fileName;
            else
                return String.Empty;
        }
    }

    public static class WorkDir
    {
        private const string c_workDirNameEnvVariable = "AARDVARK_WORKDIR";
        private static string s_workDirName = null;

        private static readonly string[] s_fallbackWorkDirNames =
        {
            @"C:\Aardwork",
            @"C:\Data\Aardwork",
        };

        static WorkDir()
        {
            s_workDirName = Environment.GetEnvironmentVariable(c_workDirNameEnvVariable);

            if (s_workDirName == null)
            {
                foreach (var s in s_fallbackWorkDirNames)
                {
                    try
                    {
                        if (Directory.Exists(s))
                        {
                            s_workDirName = s;
                            return;
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        public static string Name
        {
            get { return s_workDirName; }
            set
            {
                if (!Directory.Exists(value))
                    throw new ArgumentException("directory does not exist: " + value);
                s_workDirName = value;
            }
        }

        public static bool Exists { get { return s_workDirName != null; } }

        public static string FindFile(string fileName)
        {
            if (!Exists) throw new InvalidOperationException(
                "Aardvark work dir is undefined. Set env variable " + c_workDirNameEnvVariable);

            var result = FindFileBreadthFirst(
                fileName, new string[] { Name }
                );

            return result;
        }

        public static IEnumerable<string> FindFiles(IEnumerable<string> fileNames)
        {
            if (!Exists) throw new InvalidOperationException(
                   "Aardvark work dir is undefined. Set env variable " + c_workDirNameEnvVariable);

            return from fileName in fileNames select FindFile(fileName);
        }

        private static string FindFileBreadthFirst(
            string pattern, IEnumerable<string> dirs
            )
        {
            if (dirs.Count() == 0) return null;

            var fileNames =
                from dir in dirs
                from file in Directory.GetFiles(dir, pattern)
                select file;

            var result = fileNames.FirstOrDefault();
            if (result != null) return result;

            return FindFileBreadthFirst(
                pattern,
                from dir in dirs
                from subdir in Directory.GetDirectories(dir)
                select subdir
                );
        }

        public static string FindDir(string dirName)
        {
            if (!Exists) throw new InvalidOperationException(
                   "Aardvark work dir is undefined. Set env variable " + c_workDirNameEnvVariable);

            if (dirName == ".") return Name;

            var result = FindDirBreadthFirst(
                dirName, new string[] { Name }
                );

            if (result == null)
            {
                result = Path.Combine(Name, dirName);
                Directory.CreateDirectory(result);
            }

            return result;
        }

        private static string FindDirBreadthFirst(
            string pattern, IEnumerable<string> dirs
            )
        {
            if (dirs.Count() == 0) return null;

            var result = (
                from dir in dirs
                from subdir in Directory.GetDirectories(dir, pattern)
                select subdir
                ).FirstOrDefault();

            if (result != null) return result;

            return FindDirBreadthFirst(
                pattern,
                from dir in dirs
                from subdir in Directory.GetDirectories(dir)
                select subdir
                );
        }

        /// <summary>
        /// Converts the absolute path into a path relative to the 
        /// AARDVARK_WORKDIR or Null if not possible.
        /// </summary>
        public static string FindRelativePath(string absolutePath)
        {
            if (!Exists) throw new InvalidOperationException(
                   "Aardvark work dir is undefined. Set env variable " + c_workDirNameEnvVariable);

            return Dir.RelativeDir(absolutePath, WorkDir.Name);
        }
    }
}
