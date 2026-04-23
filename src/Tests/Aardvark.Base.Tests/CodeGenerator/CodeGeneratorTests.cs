using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Aardvark.Tests.CodeGenerator
{
    [TestFixture]
    public class CodeGeneratorTests
    {
        private static string WriteConfig(params string[] lines)
        {
            var path = Path.Combine(Path.GetTempPath(), $"codegenerator-{Guid.NewGuid():N}.conf");
            File.WriteAllLines(path, lines);
            return path;
        }

        [Test]
        public void ParseConfigFileAcceptsValidRowsBlankLinesAndComments()
        {
            var configPath = WriteConfig(
                "",
                "   ",
                "# comment only",
                "template1.cs output1.cs",
                "\ttemplate2.fs\toutput2.fs\t",
                "   # another comment"
            );

            try
            {
                var entries = global::CodeGenerator.Program.ParseConfigFile(configPath);

                Assert.That(entries.Count, Is.EqualTo(2));
                Assert.That(entries[0].TemplateFileName, Is.EqualTo("template1.cs"));
                Assert.That(entries[0].OutputFileName, Is.EqualTo("output1.cs"));
                Assert.That(entries[1].TemplateFileName, Is.EqualTo("template2.fs"));
                Assert.That(entries[1].OutputFileName, Is.EqualTo("output2.fs"));
            }
            finally
            {
                File.Delete(configPath);
            }
        }

        [Test]
        public void ParseConfigFileRejectsMalformedRowsWithClearMessage()
        {
            var configPath = WriteConfig(
                "template1.cs output1.cs",
                "malformed-only-one-column"
            );

            try
            {
                var ex = Assert.Throws<InvalidDataException>(() => global::CodeGenerator.Program.ParseConfigFile(configPath));
                Assert.That(ex.Message, Does.Contain(Path.GetFileName(configPath)));
                Assert.That(ex.Message, Does.Contain("line 2"));
                Assert.That(ex.Message, Does.Contain("expected '<template> <output>'"));
                Assert.That(ex.Message, Does.Contain("malformed-only-one-column"));
            }
            finally
            {
                File.Delete(configPath);
            }
        }

        [Test]
        public void RunReturnsNonZeroForMalformedConfig()
        {
            var configPath = WriteConfig(
                "template1.cs output1.cs",
                "too many columns here"
            );

            try
            {
                var exitCode = global::CodeGenerator.Program.Run(new[] { configPath });
                Assert.That(exitCode, Is.Not.EqualTo(0));
            }
            finally
            {
                File.Delete(configPath);
            }
        }
    }
}
