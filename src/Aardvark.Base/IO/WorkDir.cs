using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static class Dir
    {
        /// <summary>
        /// Converts the absolute directory path into a relative one, relative
        /// to the provided relativeTo directory path. Returns null if not possible
        /// (e.g. paths are on different drives).
        /// </summary>
        /// <param name="absoluteDir">The directory where the result should point at.</param>
        /// <param name="relativeToDir">The directory from where the result should start from.</param>
        /// <returns>The relative path from <paramref name="relativeToDir"/> to <paramref name="absoluteDir"/> 
        /// or null if no relative path can be found.</returns>
        public static string RelativeDir(string absoluteDir, string relativeToDir)
        {
            return RelativeDir(new DirectoryInfo(absoluteDir), new DirectoryInfo(relativeToDir));
        }

        /// <summary>
        /// Converts the absolute directory path into a relative one, relative
        /// to the provided relativeTo directory path. Returns null if not possible
        /// (e.g. paths are on different drives).
        /// </summary>
        /// <param name="absoluteDir">The directory where the result should point at.</param>
        /// <param name="relativeToDir">The directory from where the result should start from.</param>
        /// <returns>The relative path from <paramref name="relativeToDir"/> to <paramref name="absoluteDir"/> 
        /// or null if no relative path can be found.</returns>
        public static string RelativeDir(DirectoryInfo absoluteDir, DirectoryInfo relativeToDir)
        {
            var absoluteDirS = absoluteDir.FullName;
            var relativeToDirS = relativeToDir.FullName;

            var separators = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar };
            string[] absoluteDirectories = Path.GetFullPath(absoluteDirS).Split(separators, StringSplitOptions.RemoveEmptyEntries);
            string[] relativeDirectories = Path.GetFullPath(relativeToDirS).Split(separators, StringSplitOptions.RemoveEmptyEntries);

            // get the shortest of the two paths
            int length = System.Math.Min(absoluteDirectories.Length, relativeDirectories.Length);


            // find common root
            int index;
            for (index = 0; index < length; index++)
                if (absoluteDirectories[index].ToLower() != relativeDirectories[index].ToLower())
                    break;
            // use to determine where in the loop we exited
            int lastCommonRoot = index - 1;

            // if we didn't find a common prefix then return null
            if (lastCommonRoot < 0)
                return null;

            // build up the relative path
            StringBuilder relativePath = new StringBuilder();

            // add on the ..
            for (index = lastCommonRoot + 1; index < relativeDirectories.Length; index++)
                if (relativeDirectories[index].Length > 0)
                    relativePath.Append(".." + Path.DirectorySeparatorChar);

            // add on the folders
            for (index = lastCommonRoot + 1; index < absoluteDirectories.Length; index++)
                relativePath.Append(absoluteDirectories[index] + Path.DirectorySeparatorChar);

            return relativePath.ToString();
        }

        /// <summary>
        /// Converts the absolute file path into a relative one, relative
        /// to the provided relativeTo directory path. Returns null if not possible
        /// (e.g. paths are on different drives).
        /// </summary>
        /// <param name="absoluteFile">The file where the result should point at.</param>
        /// <param name="relativeTo">The directory from where the result should start from.</param>
        /// <returns>The relative path from <paramref name="relativeTo"/> to <paramref name="absoluteFile"/> 
        /// or null if no relative path can be found.</returns>
        public static string RelativeFile(FileInfo absoluteFile, DirectoryInfo relativeTo)
        {
            return RelativeDir(absoluteFile.Directory, relativeTo) + absoluteFile.Name;
        }

        /// <summary>
        /// Converts the absolute file path into a relative one, relative
        /// to the provided relativeTo directory path. Returns null if not possible
        /// (e.g. paths are on different drives).
        /// </summary>
        /// <param name="absoluteFile">The file where the result should point at.</param>
        /// <param name="relativeTo">The directory from where the result should start from.</param>
        /// <returns>The relative path from <paramref name="relativeTo"/> to <paramref name="absoluteFile"/> 
        /// or null if no relative path can be found.</returns>
        public static string RelativeFile(string absoluteFile, string relativeTo)
        {
            return RelativeFile(new FileInfo(absoluteFile), new DirectoryInfo(relativeTo));
        }

        /// <summary>
        /// Returns the relative directory path if possible, if not it returns the absolute path if
        /// UseAbsolutePathsIfNecessary is set to true (default behaviour) else returns String.Empty.
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
        /// Returns the relative file path if possible, if not it returns the absolute path if
        /// UseAbsolutePathsIfNecessary is set to true (default behaviour) else returns String.Empty.
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

        private static string[] s_fallbackWorkDirNames =
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
