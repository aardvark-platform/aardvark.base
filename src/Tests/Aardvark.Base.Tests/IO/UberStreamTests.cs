using Aardvark.Base.Coder;
using NUnit.Framework;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

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

        private sealed class VirtualStream : Stream
        {
            private long m_position;
            private readonly long m_length;
            private readonly int m_seed;
            public readonly Dictionary<long, byte> Written = new Dictionary<long, byte>();
            public readonly List<(long Position, int Offset, int Count)> Calls = new List<(long, int, int)>();
            public Exception Failure;
            public int DisposeCount;

            public VirtualStream(long length, int seed) { m_length = length; m_seed = seed; }
            public static byte ByteAt(long position, int seed) => (byte)((position % 251 + seed) % 251);
            public override bool CanRead => true;
            public override bool CanSeek => true;
            public override bool CanWrite => true;
            public override long Length => m_length;
            public override long Position { get => m_position; set => m_position = value; }
            public override void Flush() { }
            public override void SetLength(long value) => throw new NotSupportedException();
            public override long Seek(long offset, SeekOrigin origin)
            {
                return Position = checked((origin == SeekOrigin.Begin ? 0 : origin == SeekOrigin.Current ? Position : Length) + offset);
            }
            private void Record(byte[] buffer, int offset, int count)
            {
                if (buffer == null) throw new ArgumentNullException(nameof(buffer));
                if (offset < 0 || count < 0 || offset > buffer.Length - count)
                    throw new ArgumentOutOfRangeException(nameof(count));
                Calls.Add((Position, offset, count));
                if (Failure != null) throw Failure;
            }
            public override int Read(byte[] buffer, int offset, int count)
            {
                Record(buffer, offset, count);
                var actual = (int)Math.Min(count, Math.Max(0, Length - Position));
                for (var i = 0; i < actual; i++)
                    buffer[offset + i] = Written.TryGetValue(Position + i, out var value) ? value : ByteAt(Position + i, m_seed);
                Position += actual;
                return actual;
            }
            public override void Write(byte[] buffer, int offset, int count)
            {
                Record(buffer, offset, count);
                if (count > Length - Position) throw new IOException("Write exceeds virtual storage.");
                for (var i = 0; i < count; i++) Written[Position + i] = buffer[offset + i];
                Position += count;
            }
            protected override void Dispose(bool disposing)
            {
                if (disposing) DisposeCount++;
                base.Dispose(disposing);
            }
        }

        private static readonly long[] SliceLengths =
        {
            0, 1, 8, 31, int.MaxValue - 1L, int.MaxValue, int.MaxValue + 1L,
            int.MaxValue + 2L, (1L << 32) - 1, 1L << 32, (1L << 32) + 1,
            (1L << 32) + 7, (1L << 40) + 13, long.MaxValue / 4 + 17
        };

        private static UberStream CreateVirtualSlice(bool bundled, long size, long start, out VirtualStream[] sources)
        {
            sources = bundled
                ? new[] { new VirtualStream(start + size, 11), new VirtualStream(size + 5, 67), new VirtualStream(7, 149) }
                : new[] { new VirtualStream(start + size + 5, 11) };
            return bundled
                ? new UberStream(sources, start, 2 * size + 9)
                : new UberStream(sources[0], start, size);
        }

        // Map one logical byte independently of the transfer loops and narrowing arithmetic.
        private static (int Source, long Position, int Seed) Locate(bool bundled, long size, long start, long position)
        {
            if (!bundled || position < size) return (0, start + position, 11);
            if (position < 2 * size + 5) return (1, position - size, 67);
            return (2, position - (2 * size + 5), 149);
        }

        private static long[] TransferPositions(bool bundled, long size)
        {
            var length = bundled ? 2 * size + 9 : size;
            return new[] { 0, Math.Min(1, length), size / 2, Math.Max(0, size - 3), size,
                bundled ? Math.Max(0, 2 * size + 2) : 0, Math.Max(0, length - 3), length, length + 7 }.Distinct().ToArray();
        }

        [Test]
        public static void ReadsOnLongSlicesPreserveContentsCountsAndPositions()
        {
            foreach (var bundled in new[] { false, true })
            foreach (var size in SliceLengths)
            foreach (var start in new long[] { 0, 3, 17 })
            foreach (var position in TransferPositions(bundled, size))
            foreach (var count in new[] { 0, 1, 8, 13 })
            {
                using (var stream = CreateVirtualSlice(bundled, size, start, out var sources))
                {
                    stream.Position = position;
                    for (var repetition = 0; repetition < 3; repetition++)
                    {
                        var p = stream.Position;
                        var expectedCount = (int)Math.Min(count, Math.Max(0, stream.Length - p));
                        var buffer = Enumerable.Repeat((byte)255, 19).ToArray();
                        var expected = (byte[])buffer.Clone();
                        var expectedPositions = sources.Select(s => s.Position).ToArray();
                        var previousRequested = sources.Sum(s => s.Calls.Sum(c => c.Count));
                        for (var i = 0; i < expectedCount; i++)
                        {
                            var location = Locate(bundled, size, start, p + i);
                            expected[2 + i] = VirtualStream.ByteAt(location.Position, location.Seed);
                            expectedPositions[location.Source] = location.Position + 1;
                        }
                        var context = $"bundled={bundled}, size={size}, start={start}, position={p}, count={count}, repetition={repetition}";
                        Assert.AreEqual(expectedCount, stream.Read(buffer, 2, count), context);
                        CollectionAssert.AreEqual(expected, buffer, context);
                        Assert.AreEqual(p + expectedCount, stream.Position, context);
                        Assert.AreEqual(previousRequested + expectedCount, sources.Sum(s => s.Calls.Sum(c => c.Count)), context);
                        for (var i = 0; i < sources.Length; i++)
                        {
                            Assert.AreEqual(0, sources[i].DisposeCount, context);
                            Assert.AreEqual(expectedPositions[i], sources[i].Position, context);
                        }
                    }
                }
            }
        }

        [Test]
        public static void WritesOnLongSlicesPreserveContentsCountsAndPositions()
        {
            foreach (var bundled in new[] { false, true })
            foreach (var size in SliceLengths)
            foreach (var start in new long[] { 0, 3, 17 })
            foreach (var position in TransferPositions(bundled, size))
            foreach (var count in new[] { 0, 1, 8, 13 })
            {
                using (var stream = CreateVirtualSlice(bundled, size, start, out var sources))
                {
                    stream.Position = position;
                    var expectedCount = (int)Math.Min(count, Math.Max(0, stream.Length - position));
                    var buffer = Enumerable.Range(0, 19).Select(i => (byte)(i * 7 + 1)).ToArray();
                    var unchanged = (byte[])buffer.Clone();
                    var context = $"bundled={bundled}, size={size}, start={start}, position={position}, count={count}";
                    Assert.DoesNotThrow(() => stream.Write(buffer, 2, count), context);
                    CollectionAssert.AreEqual(unchanged, buffer, context);
                    Assert.AreEqual(position + expectedCount, stream.Position, context);
                    Assert.AreEqual(expectedCount, sources.Sum(s => s.Written.Count), context);
                    Assert.AreEqual(expectedCount, sources.Sum(s => s.Calls.Sum(c => c.Count)), context);
                    for (var i = 0; i < expectedCount; i++)
                    {
                        var location = Locate(bundled, size, start, position + i);
                        Assert.AreEqual(buffer[2 + i], sources[location.Source].Written[location.Position], context);
                    }
                    for (var index = 0; index < sources.Length; index++)
                    {
                        var source = sources[index];
                        Assert.AreEqual(0, source.DisposeCount, context);
                        var lastWritten = source.Written.Count == 0 ? 0 : source.Written.Keys.Max() + 1;
                        Assert.AreEqual(lastWritten, source.Position, context);
                        Assert.That(source.Calls.All(c => c.Offset >= 2 && c.Offset + c.Count <= 2 + expectedCount), Is.True, context);
                    }
                }
            }
        }

        [Test]
        public static void TransferFailuresRetainPartialEffectsValidationAndOwnership()
        {
            foreach (var write in new[] { false, true })
            foreach (var size in new long[] { 31, (1L << 32) + 8 })
            {
                var failure = new IOException("selected transfer");
                var stream = CreateVirtualSlice(true, size, 3, out var sources);
                using (stream)
                {
                    sources[1].Failure = failure;
                    stream.Position = size - 2;
                    var buffer = Enumerable.Repeat((byte)255, 12).ToArray();
                    var context = $"write={write}, size={size}";
                    TestDelegate transfer = () => { if (write) stream.Write(buffer, 2, 8); else stream.Read(buffer, 2, 8); };
                    Assert.AreSame(failure, Assert.Throws<IOException>(transfer), context);
                    Assert.AreEqual(write ? size - 2 : size, stream.Position, context);
                    Assert.AreEqual((size + 1, 2, 2), sources[0].Calls.Single(), context);
                    Assert.AreEqual((0L, 4, 6), sources[1].Calls.Single(), context);
                    Assert.AreEqual(0, sources[2].Calls.Count, context);
                    Assert.AreEqual(size + 3, sources[0].Position, context);
                    Assert.AreEqual(0, sources[1].Position, context);
                    if (write)
                    {
                        Assert.AreEqual(2, sources[0].Written.Count, context);
                        Assert.AreEqual(255, sources[0].Written[size + 1], context);
                        Assert.AreEqual(255, sources[0].Written[size + 2], context);
                        Assert.AreEqual(0, sources[1].Written.Count, context);
                    }
                    else
                    {
                        Assert.AreEqual(VirtualStream.ByteAt(size + 1, 11), buffer[2], context);
                        Assert.AreEqual(VirtualStream.ByteAt(size + 2, 11), buffer[3], context);
                        Assert.That(buffer.Where((b, i) => i < 2 || i >= 4).All(b => b == 255), Is.True, context);
                    }
                    var calls = sources.Sum(s => s.Calls.Count);
                    var position = stream.Position;
                    Assert.AreEqual("buffer", Assert.Throws<ArgumentNullException>(() => { if (write) stream.Write(null, 0, 0); else stream.Read(null, 0, 0); }).ParamName, context);
                    Assert.AreEqual("count", Assert.Throws<ArgumentException>(() => { if (write) stream.Write(buffer, 11, 2); else stream.Read(buffer, 11, 2); }).ParamName, context);
                    if (write) stream.Write(buffer, buffer.Length, 0);
                    else Assert.AreEqual(0, stream.Read(buffer, buffer.Length, 0), context);
                    Assert.AreEqual(position, stream.Position, context);
                    Assert.AreEqual(calls, sources.Sum(s => s.Calls.Count), context);
                    Assert.That(sources.All(s => s.DisposeCount == 0), Is.True, context);
                }
                Assert.That(sources.All(s => s.DisposeCount == 1), Is.True, $"write={write}, size={size}");
            }
        }

        private static TException AssertFailedSeekKeepsPosition<TException>(UberStream stream, TestDelegate seek)
            where TException : Exception
        {
            var position = stream.Position;
            var exception = Assert.Throws<TException>(seek);
            Assert.AreEqual(position, stream.Position);
            return exception;
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
        public static void SeekRejectsInvalidOriginWithoutChangingPosition()
        {
            using (var stream = new UberStream(new MemoryStream(new byte[] { 1, 2, 3, 4 }), 0, 4))
            {
                stream.Seek(2, SeekOrigin.Begin);

                var exception = AssertFailedSeekKeepsPosition<ArgumentException>(stream, () => stream.Seek(0, (SeekOrigin)42));
                Assert.AreEqual("origin", exception.ParamName);
            }
        }

        [Test]
        public static void SeekRejectsNegativeTargetsWithoutChangingPosition()
        {
            using (var stream = new UberStream(new MemoryStream(new byte[] { 1, 2, 3, 4 }), 0, 4))
            {
                stream.Seek(2, SeekOrigin.Begin);
                AssertFailedSeekKeepsPosition<IOException>(stream, () => stream.Seek(-1, SeekOrigin.Begin));

                stream.Seek(1, SeekOrigin.Begin);
                AssertFailedSeekKeepsPosition<IOException>(stream, () => stream.Seek(-2, SeekOrigin.Current));

                stream.Seek(3, SeekOrigin.Begin);
                AssertFailedSeekKeepsPosition<IOException>(stream, () => stream.Seek(-5, SeekOrigin.End));
            }
        }

        [Test]
        public static void SeekConvertsOverflowTargetsToIOExceptionWithoutChangingPosition()
        {
            using (var stream = new UberStream(new MemoryStream(new byte[] { 1, 2, 3, 4 }), 0, 4))
            {
                stream.Seek(1, SeekOrigin.Begin);
                AssertFailedSeekKeepsPosition<IOException>(stream, () => stream.Seek(long.MaxValue, SeekOrigin.Current));

                stream.Seek(2, SeekOrigin.Begin);
                AssertFailedSeekKeepsPosition<IOException>(stream, () => stream.Seek(long.MaxValue, SeekOrigin.End));
            }
        }

        [Test]
        public static void SeekAllowsBeyondEndAndKeepsBoundedReadWriteBehavior()
        {
            var bytes = new byte[] { 1, 2, 3, 4 };
            using (var stream = new UberStream(new MemoryStream(bytes, true), 0, bytes.Length))
            {
                Assert.AreEqual(6, stream.Seek(2, SeekOrigin.End));
                Assert.AreEqual(6, stream.Position);

                var buffer = new byte[] { 99, 99 };
                Assert.AreEqual(0, stream.Read(buffer, 0, buffer.Length));
                Assert.AreEqual(6, stream.Position);
                CollectionAssert.AreEqual(new byte[] { 99, 99 }, buffer);

                stream.Write(new byte[] { 9 }, 0, 1);
                Assert.AreEqual(6, stream.Position);
                CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 4 }, bytes);
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
