using Aardvark.Base.Coder;
using NUnit.Framework;
using System;
using System.IO;

namespace Aardvark.Tests.IO
{
    static class ChunkedMemoryStreamTests
    {
        private static void AssertParamName<TException>(string paramName, TestDelegate code)
            where TException : ArgumentException
        {
            var exception = Assert.Throws<TException>(code);
            Assert.AreEqual(paramName, exception.ParamName);
        }

        [Test]
        public static void ConstructorRejectsInvalidChunkSizes()
        {
            AssertParamName<ArgumentOutOfRangeException>("chunkSize", () => new ChunkedMemoryStream(0));
            AssertParamName<ArgumentOutOfRangeException>("chunkSize", () => new ChunkedMemoryStream(-1));
        }

        [Test]
        public static void ReadRejectsInvalidArgumentsBeforeChangingPosition()
        {
            using (var stream = new ChunkedMemoryStream(2))
            {
                stream.Write(new byte[] { 1, 2, 3 }, 0, 3);
                stream.Position = 1;

                AssertParamName<ArgumentNullException>("buffer", () => stream.Read(null, 0, 1));
                Assert.AreEqual(1, stream.Position);

                AssertParamName<ArgumentOutOfRangeException>("offset", () => stream.Read(new byte[2], -1, 1));
                Assert.AreEqual(1, stream.Position);

                AssertParamName<ArgumentOutOfRangeException>("count", () => stream.Read(new byte[2], 0, -1));
                Assert.AreEqual(1, stream.Position);

                AssertParamName<ArgumentException>("count", () => stream.Read(new byte[2], 1, 2));
                Assert.AreEqual(1, stream.Position);
                Assert.AreEqual(3, stream.Length);
            }
        }

        [Test]
        public static void WriteRejectsInvalidArgumentsBeforeChangingState()
        {
            foreach (var truncate in new[] { false, true })
            foreach (var position in new[] { 1, 9, 17 })
            {
                using var stream = new ChunkedMemoryStream(4);
                using var reference = new MemoryStream();
                var data = InitialContents(9);
                stream.Write(data, 0, data.Length);
                reference.Write(data, 0, data.Length);
                if (truncate) { stream.SetLength(2); reference.SetLength(2); }
                stream.Position = reference.Position = position;
                var chunks = stream.ChunkList.ToArray();
                var contents = stream.ChunkList.ConvertAll(chunk => (byte[])chunk.Clone());

                foreach (var invalid in new (byte[] Buffer, int Offset, int Count, Type Exception, string Parameter)[]
                {
                    (null, 0, 0, typeof(ArgumentNullException), "buffer"),
                    (null, 0, 1, typeof(ArgumentNullException), "buffer"),
                    (new byte[2], -1, 0, typeof(ArgumentOutOfRangeException), "offset"),
                    (new byte[2], -1, 1, typeof(ArgumentOutOfRangeException), "offset"),
                    (new byte[2], 0, -1, typeof(ArgumentOutOfRangeException), "count"),
                    (new byte[2], 1, 2, typeof(ArgumentException), "count"),
                    (new byte[2], 3, 0, typeof(ArgumentException), "count"),
                    (new byte[2], int.MaxValue, 0, typeof(ArgumentException), "count"),
                    (new byte[2], 1, int.MaxValue, typeof(ArgumentException), "count")
                })
                {
                    var context = $"truncate={truncate}, position={position}, bufferLength={invalid.Buffer?.Length}, offset={invalid.Offset}, count={invalid.Count}";
                    var error = Assert.Throws(invalid.Exception,
                        () => stream.Write(invalid.Buffer, invalid.Offset, invalid.Count), context);
                    Assert.AreEqual(invalid.Parameter, ((ArgumentException)error).ParamName, context);
                    // MemoryStream may throw a more specific argument exception.
                    Assert.Catch(invalid.Exception,
                        () => reference.Write(invalid.Buffer, invalid.Offset, invalid.Count), context);
                    Assert.AreEqual(chunks.Length, stream.ChunkList.Count, context);
                    for (var i = 0; i < chunks.Length; i++)
                    {
                        Assert.AreSame(chunks[i], stream.ChunkList[i], context);
                        CollectionAssert.AreEqual(contents[i], chunks[i], context);
                    }
                    AssertEquivalent(reference, stream, context);
                }
            }
        }

        [Test]
        public static void PositionSeekAndSetLengthRejectInvalidValues()
        {
            using (var stream = new ChunkedMemoryStream(2))
            {
                stream.Write(new byte[] { 1, 2, 3 }, 0, 3);
                stream.Position = 1;

                AssertParamName<ArgumentOutOfRangeException>("value", () => stream.Position = -1);
                Assert.AreEqual(1, stream.Position);

                Assert.Throws<IOException>(() => stream.Seek(-1, SeekOrigin.Begin));
                Assert.AreEqual(1, stream.Position);

                Assert.Throws<IOException>(() => stream.Seek(-2, SeekOrigin.Current));
                Assert.AreEqual(1, stream.Position);

                Assert.Throws<IOException>(() => stream.Seek(-4, SeekOrigin.End));
                Assert.AreEqual(1, stream.Position);

                AssertParamName<ArgumentException>("origin", () => stream.Seek(0, (SeekOrigin)123));
                Assert.AreEqual(1, stream.Position);

                AssertParamName<ArgumentOutOfRangeException>("value", () => stream.SetLength(-1));
                Assert.AreEqual(1, stream.Position);
                Assert.AreEqual(3, stream.Length);
            }
        }

        [Test]
        public static void WriteAfterSeekingBeyondLengthZeroFillsGapAndExtendsLength()
        {
            using (var stream = new ChunkedMemoryStream(2))
            {
                stream.WriteByte(1);
                stream.Seek(5, SeekOrigin.Begin);
                stream.WriteByte(9);

                Assert.AreEqual(6, stream.Length);
                Assert.AreEqual(6, stream.Position);
                CollectionAssert.AreEqual(new byte[] { 1, 0, 0, 0, 0, 9 }, ReadAll(stream));
            }
        }

        [Test]
        public static void WriteAfterShrinkingWithPositionBeyondLengthExtendsWithZeroGap()
        {
            using (var stream = new ChunkedMemoryStream(3))
            {
                stream.Write(new byte[] { 1, 2, 3, 4, 5, 6, 7 }, 0, 7);
                stream.Position = 6;
                stream.SetLength(2);
                stream.WriteByte(9);

                Assert.AreEqual(7, stream.Length);
                Assert.AreEqual(7, stream.Position);
                CollectionAssert.AreEqual(new byte[] { 1, 2, 0, 0, 0, 0, 9 }, ReadAll(stream));
            }
        }

        [Test]
        public static void ReadsAndWritesCrossMultipleChunks()
        {
            using (var stream = new ChunkedMemoryStream(2))
            {
                stream.Write(new byte[] { 99, 1, 2, 3, 4, 5, 99 }, 1, 5);

                Assert.AreEqual(5, stream.Length);
                Assert.AreEqual(5, stream.Position);

                stream.Position = 0;
                var buffer = new byte[7];
                Assert.AreEqual(5, stream.Read(buffer, 1, 5));
                CollectionAssert.AreEqual(new byte[] { 0, 1, 2, 3, 4, 5, 0 }, buffer);
                Assert.AreEqual(5, stream.Position);
            }
        }

        [Test]
        public static void EmptyWritesAfterSeekingExtendWithReadableZeroFilledGaps()
        {
            foreach (var chunkSize in new[] { 4, 1, 7 })
            foreach (var initialLength in new[] { 1, 0, chunkSize + 1 })
            foreach (var position in new[] { 2 * chunkSize + 1, chunkSize - 1, chunkSize, 2 * chunkSize, 5 * chunkSize - 1, 5 * chunkSize, 5 * chunkSize + 1 })
            foreach (var buffer in new[] { Array.Empty<byte>(), new byte[] { 91, 92 } })
            {
                if (position <= initialLength) continue;
                var context = $"chunk={chunkSize}, initialLength={initialLength}, position={position}, offset={buffer.Length}";
                using var stream = new ChunkedMemoryStream(chunkSize);
                using var reference = new MemoryStream();
                var data = InitialContents(initialLength);
                stream.Write(data, 0, data.Length);
                reference.Write(data, 0, data.Length);
                stream.Seek(position, SeekOrigin.Begin);
                reference.Seek(position, SeekOrigin.Begin);

                stream.Write(buffer, buffer.Length, 0);
                reference.Write(buffer, buffer.Length, 0);
                Assert.AreEqual(position, stream.Position, context);
                Assert.AreEqual(position, stream.Length, context);
                AssertEquivalent(reference, stream, context);

                // A following ordinary write must also work at an exact chunk boundary.
                stream.Write(buffer, 0, buffer.Length);
                reference.Write(buffer, 0, buffer.Length);
                stream.WriteByte(123);
                reference.WriteByte(123);
                AssertEquivalent(reference, stream, context + ": following write");
            }
        }

        [Test]
        public static void EmptyWritesAfterTruncationPreservePositionAndClearDiscardedContents()
        {
            foreach (var chunkSize in new[] { 4, 1, 7 })
            foreach (var length in new[] { 0, 1, chunkSize - 1, chunkSize, chunkSize + 1, 2 * chunkSize })
            foreach (var position in new[] { 3 * chunkSize - 1, 3 * chunkSize, 3 * chunkSize + 1, 7 * chunkSize })
            {
                if (position <= length) continue;
                var context = $"chunk={chunkSize}, truncatedLength={length}, retainedPosition={position}";
                using var stream = new ChunkedMemoryStream(chunkSize);
                using var reference = new MemoryStream();
                var data = InitialContents(6 * chunkSize + 3);
                stream.Write(data, 0, data.Length);
                reference.Write(data, 0, data.Length);
                stream.Position = reference.Position = position;
                stream.SetLength(length);
                reference.SetLength(length);
                Assert.AreEqual(position, stream.Position, context);
                // Unlike MemoryStream, ChunkedMemoryStream retains Position on truncation.
                reference.Position = position;

                stream.Write(data, data.Length, 0);
                reference.Write(data, data.Length, 0);
                Assert.AreEqual(position, stream.Position, context);
                AssertEquivalent(reference, stream, context);
            }
        }

        [Test]
        public static void EmptyWritesAtOrBeforeLengthLeaveContentsAndStorageUnchanged()
        {
            foreach (var chunkSize in new[] { 1, 4, 7 })
            foreach (var length in new[] { 0, 1, chunkSize, 3 * chunkSize + 1 })
            foreach (var position in new[] { 0, length / 2, length })
            {
                var context = $"chunk={chunkSize}, length={length}, position={position}";
                using var stream = new ChunkedMemoryStream(chunkSize);
                using var reference = new MemoryStream();
                var data = InitialContents(length);
                stream.Write(data, 0, data.Length);
                reference.Write(data, 0, data.Length);
                stream.Position = reference.Position = position;
                var chunks = stream.ChunkList.ToArray();
                var contents = stream.ChunkList.ConvertAll(chunk => (byte[])chunk.Clone());
                foreach (var offset in new[] { 0, data.Length })
                {
                    stream.Write(data, offset, 0);
                    reference.Write(data, offset, 0);
                    AssertEquivalent(reference, stream, context + $", offset={offset}");
                    Assert.AreEqual(chunks.Length, stream.ChunkList.Count, context);
                    for (var i = 0; i < chunks.Length; i++)
                    {
                        Assert.AreSame(chunks[i], stream.ChunkList[i], context);
                        CollectionAssert.AreEqual(contents[i], chunks[i], context);
                    }
                }
            }
        }

        [Test]
        public static void SeededMixedOperationsMatchMemoryStream()
        {
            foreach (var chunkSize in new[] { 1, 4, 7, 16 })
            foreach (var seed in new[] { 18973, 73, 321 })
            {
                using var stream = new ChunkedMemoryStream(chunkSize);
                using var reference = new MemoryStream();
                var random = new Random(seed);
                var buffer = new byte[31];
                for (var step = 0; step < 256; step++)
                {
                    var operation = random.Next(8);
                    var context = $"chunk={chunkSize}, seed={seed}, step={step}, operation={operation}, position={reference.Position}, length={reference.Length}";
                    switch (operation)
                    {
                        case 0:
                            random.NextBytes(buffer);
                            var offset = random.Next(buffer.Length + 1);
                            var count = random.Next(buffer.Length - offset + 1);
                            stream.Write(buffer, offset, count);
                            reference.Write(buffer, offset, count);
                            break;
                        case 1:
                            var value = (byte)random.Next(256);
                            stream.WriteByte(value);
                            reference.WriteByte(value);
                            break;
                        case 2:
                            stream.Write(buffer, buffer.Length, 0);
                            reference.Write(buffer, buffer.Length, 0);
                            break;
                        case 3:
                            var target = random.Next(8 * chunkSize + 1);
                            var origin = (SeekOrigin)random.Next(3);
                            var distance = target - (origin == SeekOrigin.Begin ? 0 : origin == SeekOrigin.Current ? reference.Position : reference.Length);
                            Assert.AreEqual(reference.Seek(distance, origin), stream.Seek(distance, origin), context);
                            break;
                        case 4:
                            var position = stream.Position;
                            var length = random.Next(6 * chunkSize + 1);
                            stream.SetLength(length);
                            reference.SetLength(length);
                            Assert.AreEqual(position, stream.Position, context);
                            reference.Position = position; // Match the existing retained-position contract.
                            break;
                        case 5:
                            random.NextBytes(buffer);
                            var actual = (byte[])buffer.Clone();
                            var readOffset = random.Next(buffer.Length + 1);
                            var readCount = random.Next(buffer.Length - readOffset + 1);
                            Assert.AreEqual(reference.Read(buffer, readOffset, readCount), stream.Read(actual, readOffset, readCount), context);
                            CollectionAssert.AreEqual(buffer, actual, context);
                            break;
                        case 6:
                            Assert.AreEqual(reference.ReadByte(), stream.ReadByte(), context);
                            break;
                        case 7:
                            stream.Position = reference.Position = reference.Length + random.Next(1, 2 * chunkSize + 1);
                            stream.Write(Array.Empty<byte>(), 0, 0);
                            reference.Write(Array.Empty<byte>(), 0, 0);
                            break;
                    }
                    AssertEquivalent(reference, stream, context);
                }
            }
        }

        private static byte[] InitialContents(int length)
        {
            var result = new byte[length];
            for (var i = 0; i < length; i++) result[i] = (byte)(42 + i);
            return result;
        }

        private static void AssertEquivalent(MemoryStream reference, ChunkedMemoryStream stream, string context)
        {
            Assert.AreEqual(reference.Position, stream.Position, context + ": position");
            Assert.AreEqual(reference.Length, stream.Length, context + ": length");
            var chunkSize = stream.ChunkList[0].Length;
            Assert.GreaterOrEqual(stream.ChunkList.Count, (reference.Length + chunkSize - 1) / chunkSize, context + ": backing chunks");
            var position = stream.Position;
            var contents = reference.ToArray();
            var expected = new byte[contents.Length + 4];
            Array.Fill(expected, (byte)193);
            var actual = (byte[])expected.Clone();
            Array.Copy(contents, 0, expected, 2, contents.Length);
            stream.Position = 0;
            Assert.AreEqual(contents.Length, stream.Read(actual, 2, contents.Length + 1), context + ": Read through EOF");
            Assert.AreEqual(contents.Length, stream.Position, context);
            Assert.AreEqual(0, stream.Read(actual, actual.Length, 0), context + ": empty Read at EOF");
            Assert.AreEqual(0, stream.Read(actual, 1, 1), context + ": Read at EOF");
            CollectionAssert.AreEqual(expected, actual, context + ": Read contents and guards");
            stream.Position = 0;
            for (var i = 0; i < contents.Length; i++)
                Assert.AreEqual(contents[i], stream.ReadByte(), context + $": ReadByte index={i}");
            Assert.AreEqual(-1, stream.ReadByte(), context + ": ReadByte at EOF");
            Assert.AreEqual(contents.Length, stream.Position, context);
            stream.Position = position;
        }

        private static byte[] ReadAll(ChunkedMemoryStream stream)
        {
            var position = stream.Position;
            var result = new byte[stream.Length];

            stream.Position = 0;
            Assert.AreEqual(result.Length, stream.Read(result, 0, result.Length));
            stream.Position = position;

            return result;
        }
    }
}
