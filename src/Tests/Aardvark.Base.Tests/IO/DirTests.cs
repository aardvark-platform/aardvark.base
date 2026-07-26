using Aardvark.Base.Coder;
using NUnit.Framework;
using System.IO;

namespace Aardvark.Tests.IO
{
    static class DirTests
    {
        private static string Absolute(params string[] components)
        {
            var result = Path.GetPathRoot(Path.GetFullPath("."));
            foreach (var component in components)
                result = Path.Combine(result, component);

            return result;
        }

        private static string RelativeDirectory(params string[] components)
        {
            if (components.Length == 0)
                return string.Empty;

            return string.Join(Path.DirectorySeparatorChar.ToString(), components) +
                   Path.DirectorySeparatorChar;
        }

        [Test]
        public static void RelativeDirHandlesCommonDirectoryRelationships()
        {
            var origin = Absolute("base", "branch");

            Assert.AreEqual(string.Empty, Dir.RelativeDir(origin, origin));
            Assert.AreEqual(
                RelativeDirectory("child", "leaf"),
                Dir.RelativeDir(Absolute("base", "branch", "child", "leaf"), origin));
            Assert.AreEqual(
                RelativeDirectory("..", ".."),
                Dir.RelativeDir(Absolute(), origin));
            Assert.AreEqual(
                RelativeDirectory("..", "sibling"),
                Dir.RelativeDir(Absolute("base", "sibling"), origin));
            Assert.AreEqual(
                RelativeDirectory("..", "..", "target", "leaf"),
                Dir.RelativeDir(Absolute("target", "leaf"), origin));
        }

        [Test]
        public static void RelativeDirDirectoryInfoOverloadPreservesFormatting()
        {
            var origin = new DirectoryInfo(Absolute("base", "branch"));
            var child = new DirectoryInfo(Absolute("base", "branch", "child"));
            var sibling = new DirectoryInfo(Absolute("base", "sibling"));

            Assert.AreEqual(string.Empty, Dir.RelativeDir(origin, origin));
            Assert.AreEqual(RelativeDirectory("child"), Dir.RelativeDir(child, origin));
            Assert.AreEqual(RelativeDirectory("..", "sibling"), Dir.RelativeDir(sibling, origin));
        }

        [Test]
        public static void RelativeFileSupportsStringAndInfoOverloads()
        {
            var originPath = Absolute("base", "branch");
            var filePath = Absolute("target", "leaf", "data.bin");
            var expected = Path.Combine("..", "..", "target", "leaf", "data.bin");

            Assert.AreEqual(expected, Dir.RelativeFile(filePath, originPath));
            Assert.AreEqual(
                expected,
                Dir.RelativeFile(new FileInfo(filePath), new DirectoryInfo(originPath)));

            var localFile = Absolute("base", "branch", "local.bin");
            Assert.AreEqual("local.bin", Dir.RelativeFile(localFile, originPath));
        }

        [Test]
        public static void RelativeDirUsesPlatformCaseSemantics()
        {
            var upperCasePath = Absolute("CaseRoot", "Leaf");
            var lowerCasePath = Absolute("caseroot", "leaf");
            var expected = Path.DirectorySeparatorChar == '\\'
                ? string.Empty
                : RelativeDirectory("..", "..", "CaseRoot", "Leaf");

            Assert.AreEqual(expected, Dir.RelativeDir(upperCasePath, lowerCasePath));
        }

        [Test]
        public static void IncompatibleWindowsRootsReturnNullAndUseRequestedFallback()
        {
            if (Path.DirectorySeparatorChar != '\\')
                return;

            const string targetDirectory = @"D:\target\leaf";
            const string originDirectory = @"C:\base";
            const string targetFile = @"D:\target\leaf\data.bin";

            Assert.AreEqual(
                RelativeDirectory("..", "target"),
                Dir.RelativeDir(@"c:\target", originDirectory));
            Assert.AreEqual(
                RelativeDirectory("child"),
                Dir.RelativeDir(@"\\server\share\base\child", @"\\SERVER\SHARE\base"));
            Assert.IsNull(Dir.RelativeDir(@"\\server\other", @"\\server\share"));

            Assert.IsNull(Dir.RelativeDir(targetDirectory, originDirectory));
            Assert.IsNull(Dir.RelativeDir(
                new DirectoryInfo(targetDirectory),
                new DirectoryInfo(originDirectory)));
            Assert.IsNull(Dir.RelativeFile(targetFile, originDirectory));
            Assert.IsNull(Dir.RelativeFile(
                new FileInfo(targetFile),
                new DirectoryInfo(originDirectory)));

            Assert.AreEqual(
                targetDirectory,
                Dir.TryGetRelativeDir(targetDirectory, originDirectory, true));
            Assert.AreEqual(
                string.Empty,
                Dir.TryGetRelativeDir(targetDirectory, originDirectory, false));
            Assert.AreEqual(
                targetFile,
                Dir.TryGetRelativeFileName(targetFile, originDirectory, true));
            Assert.AreEqual(
                string.Empty,
                Dir.TryGetRelativeFileName(targetFile, originDirectory, false));
        }
    }
}
