using Aardvark.Base.Coder;
using NUnit.Framework;
using System;
using System.IO;

namespace Aardvark.Tests.IO
{
    static class UberStreamTests
    {
        private sealed class ShortReadStream : Stream
        {
            private readonly MemoryStream m_inner;

            public ShortReadStream(byte[] bytes)
            {
                m_inner = new MemoryStream(bytes);
            }

            public override bool CanRead { get { return true; } }
            public override bool CanSeek { get { return true; } }
            public override bool CanWrite { get { return false; } }
            public override long Length { get { return m_inner.Length; } }

            public override long Position
            {
                get { return m_inner.Position; }
                set { m_inner.Position = value; }
            }

            public override void Flush()
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return m_inner.Read(buffer, offset, Math.Min(count, 1));
            }

            public override long Seek(long offset, SeekOrigin origin)
            {
                return m_inner.Seek(offset, origin);
            }

            public override void SetLength(long value)
            {
                m_inner.SetLength(value);
            }

            public override void Write(byte[] buffer, int offset, int count)
            {
                throw new NotSupportedException();
            }

            protected override void Dispose(bool disposing)
            {
                if (disposing)
                    m_inner.Dispose();

                base.Dispose(disposing);
            }
        }

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
        public static void ReadAdvancesByActualShortReadCountInSingleStreamSlice()
        {
            using (var stream = new UberStream(new ShortReadStream(new byte[] { 10, 11, 12, 13 }), 1, 3))
            {
                var buffer = new byte[] { 99, 99, 99 };

                Assert.AreEqual(1, stream.Read(buffer, 0, buffer.Length));
                Assert.AreEqual(1, stream.Position);
                CollectionAssert.AreEqual(new byte[] { 11, 99, 99 }, buffer);

                Assert.AreEqual(1, stream.Read(buffer, 1, buffer.Length - 1));
                Assert.AreEqual(2, stream.Position);
                CollectionAssert.AreEqual(new byte[] { 11, 12, 99 }, buffer);
            }
        }

        [Test]
        public static void RepeatedShortReadsAcrossStreamsDoNotSkipOrMisplaceBytes()
        {
            using (var stream = new UberStream(
                new Stream[]
                {
                    new ShortReadStream(new byte[] { 1, 2 }),
                    new ShortReadStream(new byte[] { 3, 4 })
                },
                0,
                4))
            {
                var buffer = new byte[] { 99, 99, 99, 99 };

                Assert.AreEqual(1, stream.Read(buffer, 0, buffer.Length));
                Assert.AreEqual(1, stream.Position);
                CollectionAssert.AreEqual(new byte[] { 1, 99, 99, 99 }, buffer);

                Assert.AreEqual(2, stream.Read(buffer, 1, buffer.Length - 1));
                Assert.AreEqual(3, stream.Position);
                CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 99 }, buffer);

                Assert.AreEqual(1, stream.Read(buffer, 3, buffer.Length - 3));
                Assert.AreEqual(4, stream.Position);
                CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4 }, buffer);

                Assert.AreEqual(0, stream.Read(buffer, 0, buffer.Length));
                Assert.AreEqual(4, stream.Position);
            }
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
