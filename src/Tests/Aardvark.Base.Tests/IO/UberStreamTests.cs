using Aardvark.Base.Coder;
using NUnit.Framework;
using System;
using System.IO;

namespace Aardvark.Tests.IO
{
    static class UberStreamTests
    {
        [Test]
        public static void SingleStreamReadAndWriteRespectSlice()
        {
            using (var stream = new UberStream(new MemoryStream(new byte[] { 0, 1, 2, 3, 4 }), 2, 2))
            {
                var buffer = new byte[2];

                Assert.AreEqual(2, stream.Read(buffer, 0, buffer.Length));
                CollectionAssert.AreEqual(new byte[] { 2, 3 }, buffer);
            }

            var bytes = new byte[] { 0, 1, 2, 3, 4 };
            using (var stream = new UberStream(new MemoryStream(bytes, true), 1, 3))
            {
                stream.Write(new byte[] { 9, 8, 7 }, 0, 3);
            }

            CollectionAssert.AreEqual(new byte[] { 0, 9, 8, 7, 4 }, bytes);
        }

        [Test]
        public static void MultipleStreamReadAndWriteCrossBoundaries()
        {
            using (var stream = new UberStream(
                new Stream[]
                {
                    new MemoryStream(new byte[] { 1, 2 }),
                    new MemoryStream(new byte[] { 3, 4, 5 })
                },
                1,
                3))
            {
                var buffer = new byte[3];

                Assert.AreEqual(3, stream.Read(buffer, 0, buffer.Length));
                CollectionAssert.AreEqual(new byte[] { 2, 3, 4 }, buffer);
            }

            var first = new byte[2];
            var second = new byte[3];
            using (var stream = new UberStream(
                new Stream[]
                {
                    new MemoryStream(first, true),
                    new MemoryStream(second, true)
                },
                1,
                3))
            {
                stream.Write(new byte[] { 9, 8, 7 }, 0, 3);
            }

            CollectionAssert.AreEqual(new byte[] { 0, 9 }, first);
            CollectionAssert.AreEqual(new byte[] { 8, 7, 0 }, second);
        }

        [Test]
        public static void ConstructorsRejectInvalidArguments()
        {
            var singleNull = Assert.Throws<ArgumentNullException>(() => new UberStream((Stream)null, 0, 1));
            Assert.AreEqual("stream", singleNull.ParamName);

            using (var source = new MemoryStream(new byte[1]))
            {
                var start = Assert.Throws<ArgumentOutOfRangeException>(() => new UberStream(source, -1, 1));
                Assert.AreEqual("startPos", start.ParamName);
            }

            using (var source = new MemoryStream(new byte[1]))
            {
                var length = Assert.Throws<ArgumentOutOfRangeException>(() => new UberStream(source, 0, -1));
                Assert.AreEqual("numOfBytes", length.ParamName);
            }

            var arrayNull = Assert.Throws<ArgumentNullException>(() => new UberStream((Stream[])null, 0, 1));
            Assert.AreEqual("streams", arrayNull.ParamName);

            var emptyArray = Assert.Throws<ArgumentException>(() => new UberStream(Array.Empty<Stream>(), 0, 0));
            Assert.AreEqual("streams", emptyArray.ParamName);

            var arrayStart = Assert.Throws<ArgumentOutOfRangeException>(() => new UberStream(new Stream[] { new MemoryStream(new byte[1]) }, -1, 1));
            Assert.AreEqual("startPos", arrayStart.ParamName);

            var arrayLength = Assert.Throws<ArgumentOutOfRangeException>(() => new UberStream(new Stream[] { new MemoryStream(new byte[1]) }, 0, -1));
            Assert.AreEqual("numOfBytes", arrayLength.ParamName);

            var arrayElementNull = Assert.Throws<ArgumentNullException>(() => new UberStream(new Stream[] { new MemoryStream(), null }, 0, 0));
            Assert.AreEqual("streams", arrayElementNull.ParamName);
        }

        [Test]
        public static void ReadRejectsInvalidArgumentsBeforeReading()
        {
            using (var stream = new UberStream(new MemoryStream(new byte[4]), 0, 4))
            {
                var nullBuffer = Assert.Throws<ArgumentNullException>(() => stream.Read(null, 0, 1));
                Assert.AreEqual("buffer", nullBuffer.ParamName);

                var negativeOffset = Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(new byte[2], -1, 1));
                Assert.AreEqual("offset", negativeOffset.ParamName);

                var negativeCount = Assert.Throws<ArgumentOutOfRangeException>(() => stream.Read(new byte[2], 0, -1));
                Assert.AreEqual("count", negativeCount.ParamName);

                var invalidRange = Assert.Throws<ArgumentException>(() => stream.Read(new byte[2], 1, 2));
                Assert.AreEqual("count", invalidRange.ParamName);
            }
        }

        [Test]
        public static void WriteRejectsInvalidArgumentsBeforeWriting()
        {
            using (var stream = new UberStream(new MemoryStream(new byte[4], true), 0, 4))
            {
                var nullBuffer = Assert.Throws<ArgumentNullException>(() => stream.Write(null, 0, 1));
                Assert.AreEqual("buffer", nullBuffer.ParamName);

                var negativeOffset = Assert.Throws<ArgumentOutOfRangeException>(() => stream.Write(new byte[2], -1, 1));
                Assert.AreEqual("offset", negativeOffset.ParamName);

                var negativeCount = Assert.Throws<ArgumentOutOfRangeException>(() => stream.Write(new byte[2], 0, -1));
                Assert.AreEqual("count", negativeCount.ParamName);

                var invalidRange = Assert.Throws<ArgumentException>(() => stream.Write(new byte[2], 1, 2));
                Assert.AreEqual("count", invalidRange.ParamName);
            }
        }

        [Test]
        public static void WriteRejectsReadOnlyStreams()
        {
            using (var stream = new UberStream(new MemoryStream(new byte[1], false), 0, 1))
            {
                Assert.Throws<NotSupportedException>(() => stream.Write(new byte[] { 1 }, 0, 1));
            }
        }
    }
}
