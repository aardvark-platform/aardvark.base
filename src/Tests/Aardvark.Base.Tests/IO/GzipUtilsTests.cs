using Aardvark.Base;
using Aardvark.Base.Coder;
using NUnit.Framework;
using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace Aardvark.Tests.IO
{
    [TestFixture]
    public class GzipUtilsTests
    {
        private static readonly int[] Sizes = { 0, 1, 65535, 65536, 65537, 131089 };
        private string m_directory;

        [SetUp]
        public void SetUp()
        {
            m_directory = Path.Combine(Path.GetTempPath(), "aardvark-gzip-" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(m_directory);
        }

        [TearDown]
        public void TearDown() => Directory.Delete(m_directory, recursive: true);

        [Test]
        public void CompressionCreatesOrReplacesTheWholeDestination()
        {
            var source = Path.Combine(m_directory, "payload.bin");
            var destination = source + ".gz";
            foreach (var size in Sizes)
            foreach (var random in new[] { false, true })
            {
                var payload = Payload(size, random);
                var expected = Compress(payload);
                File.WriteAllBytes(source, payload);
                foreach (var previousLength in DestinationLengths(expected.Length))
                {
                    PrepareDestination(destination, previousLength);
                    var context = $"size={size}, random={random}, previousLength={previousLength}";
                    GzipUtils.GzipFile(source);
                    Assert.AreEqual(expected.Length, new FileInfo(destination).Length, context);
                    CollectionAssert.AreEqual(payload, Decompress(File.ReadAllBytes(destination)), context);
                    CollectionAssert.AreEqual(payload, File.ReadAllBytes(source), "source changed: " + context);
                }
            }
        }

        [Test]
        public void DecompressionCreatesOrReplacesTheWholeDestination()
        {
            var destination = Path.Combine(m_directory, "payload.bin");
            var source = destination + ".gz";
            foreach (var size in Sizes)
            foreach (var random in new[] { false, true })
            {
                var payload = Payload(size, random);
                var encoded = Compress(payload);
                File.WriteAllBytes(source, encoded);
                foreach (var previousLength in DestinationLengths(size))
                {
                    PrepareDestination(destination, previousLength);
                    var context = $"size={size}, random={random}, previousLength={previousLength}";
                    GzipUtils.UnGzipFile(destination);
                    Assert.AreEqual(size, new FileInfo(destination).Length, context);
                    CollectionAssert.AreEqual(payload, File.ReadAllBytes(destination), context);
                    CollectionAssert.AreEqual(encoded, File.ReadAllBytes(source), "source changed: " + context);
                }
            }
        }

        [Test]
        public void RecompressionRemovesAStaleConcatenatedMember()
        {
            var source = Path.Combine(m_directory, "payload.bin");
            var payload = Encoding.UTF8.GetBytes("NEW");
            var stale = Encoding.UTF8.GetBytes("STALE");
            var currentMember = Compress(payload);
            var concatenated = currentMember.Concat(Compress(stale)).ToArray();
            CollectionAssert.AreEqual(payload.Concat(stale).ToArray(), Decompress(concatenated), "fixture must contain two members");
            File.WriteAllBytes(source, payload);
            File.WriteAllBytes(source + ".gz", concatenated);

            GzipUtils.GzipFile(source);

            Assert.AreEqual(currentMember.Length, new FileInfo(source + ".gz").Length);
            CollectionAssert.AreEqual(payload, Decompress(File.ReadAllBytes(source + ".gz")));
        }

        [Test]
        public void RepeatedRoundTripsKeepTheSuffixAndReplaceOldContents()
        {
            // Even an input already ending in .gz gets another .gz appended.
            var file = Path.Combine(m_directory, "payload with spaces ü.gz");
            foreach (var size in new[] { 131089, 65536, 1, 0, 65537, 65535, 0 })
            {
                var payload = Payload(size, random: true);
                File.WriteAllBytes(file, payload);
                GzipUtils.GzipFile(file);
                Assert.AreEqual(Compress(payload).Length, new FileInfo(file + ".gz").Length, $"compressed size={size}");
                CollectionAssert.AreEqual(payload, Decompress(File.ReadAllBytes(file + ".gz")), $"compressed size={size}");
                PrepareDestination(file, size + 37);
                GzipUtils.UnGzipFile(file);
                Assert.AreEqual(size, new FileInfo(file).Length, $"decoded size={size}");
                CollectionAssert.AreEqual(payload, File.ReadAllBytes(file), $"decoded size={size}");
            }
        }

        [Test]
        public void MissingInputsLeaveExistingOrAbsentDestinationsUntouched()
        {
            foreach (var compress in new[] { true, false })
            foreach (var existing in new[] { true, false })
            {
                var file = Path.Combine(m_directory, "missing-" + compress + "-" + existing);
                var source = compress ? file : file + ".gz";
                var destination = compress ? file + ".gz" : file;
                var original = Payload(73, random: true);
                if (existing) File.WriteAllBytes(destination, original);
                try
                {
                    var error = Assert.Throws<FileNotFoundException>(() =>
                    {
                        if (compress) GzipUtils.GzipFile(file);
                        else GzipUtils.UnGzipFile(file);
                    }, $"compress={compress}, existing={existing}");
                    Assert.AreEqual(source, error.FileName);
                }
                finally
                {
                    // The helpers' existing timed-report scope is not ended on an I/O exception.
                    Report.End();
                }
                Assert.AreEqual(existing, File.Exists(destination), $"compress={compress}, existing={existing}");
                if (existing) CollectionAssert.AreEqual(original, File.ReadAllBytes(destination), $"compress={compress}");
            }
        }

        private static int[] DestinationLengths(int length)
            => new[] { -1, 0, Math.Max(0, length - 1), length, length + 73 }.Distinct().ToArray();

        private static void PrepareDestination(string path, int length)
        {
            File.Delete(path);
            if (length >= 0) File.WriteAllBytes(path, Enumerable.Repeat((byte)0xa5, length).ToArray());
        }

        private static byte[] Payload(int length, bool random)
        {
            var result = new byte[length];
            if (random) new Random(18973 + length).NextBytes(result);
            else for (var i = 0; i < length; i++) result[i] = (byte)(i % 17);
            return result;
        }

        private static byte[] Compress(byte[] payload)
        {
            using var output = new MemoryStream();
            using (var gzip = new GZipStream(output, CompressionMode.Compress, leaveOpen: true))
            {
                // Preserve the existing no-write representation of an empty input.
                if (payload.Length > 0) gzip.Write(payload, 0, payload.Length);
            }
            return output.ToArray();
        }

        private static byte[] Decompress(byte[] encoded)
        {
            using var input = new MemoryStream(encoded);
            using var gzip = new GZipStream(input, CompressionMode.Decompress);
            using var output = new MemoryStream();
            gzip.CopyTo(output);
            return output.ToArray();
        }
    }
}
