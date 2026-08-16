using Aardvark.Base;
using Aardvark.Base.Coder;
using NUnit.Framework;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Aardvark.Tests.IO
{
    static class StreamCodeReaderTests
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct Sample
        {
            public int First;
            public short Second;

            public Sample(int first, short second)
            {
                First = first;
                Second = second;
            }
        }

        private sealed class OneByteReadStream : MemoryStream
        {
            public OneByteReadStream(byte[] bytes)
                : base(bytes, false)
            {
            }

            public override int Read(byte[] buffer, int offset, int count)
            {
                return base.Read(buffer, offset, Math.Min(count, 1));
            }

            public override int Read(Span<byte> buffer)
            {
                return base.Read(buffer.Slice(0, Math.Min(buffer.Length, 1)));
            }
        }

        private static byte[] Serialize(Action<StreamCodeWriter> write)
        {
            using (var stream = new MemoryStream())
            using (var writer = new StreamCodeWriter(stream))
            {
                write(writer);
                writer.Flush();
                return stream.ToArray();
            }
        }

        private static StreamCodeReader CreateReader(byte[] bytes)
        {
            return new StreamCodeReader(new OneByteReadStream(bytes));
        }

        [Test]
        public static void ReadByteArrayFillsDestinationRangeAcrossShortReads()
        {
            var destination = new byte[] { 99, 99, 99, 99, 99, 99, 99 };

            using (var reader = CreateReader(new byte[] { 1, 2, 3, 4, 5 }))
            {
                Assert.AreEqual(5, reader.ReadArray(destination, 1, 5));
            }

            CollectionAssert.AreEqual(new byte[] { 99, 1, 2, 3, 4, 5, 99 }, destination);
        }

        [Test]
        public static void ReadByteArrayReturnsPartialEofCount()
        {
            var destination = new byte[] { 99, 99, 99, 99, 99 };

            using (var reader = CreateReader(new byte[] { 1, 2, 3 }))
            {
                Assert.AreEqual(3, reader.ReadArray(destination, 0, destination.Length));
            }

            CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 99, 99 }, destination);
        }

        [Test]
        public static void ReadOneDimensionalArrayPreservesOffsetAcrossShortReads()
        {
            var source = new[] { new Sample(1, 2), new Sample(3, 4) };
            var bytes = Serialize(writer => writer.WriteArray(source, 0, source.Length));
            var sentinel = new Sample(-1, -1);
            var destination = new[] { sentinel, sentinel, sentinel, sentinel };

            using (var reader = CreateReader(bytes))
            {
                Assert.AreEqual(2, reader.ReadArray(destination, 1, source.Length));
            }

            Assert.AreEqual(sentinel, destination[0]);
            Assert.AreEqual(source[0], destination[1]);
            Assert.AreEqual(source[1], destination[2]);
            Assert.AreEqual(sentinel, destination[3]);
        }

        [Test]
        public static void ReadTwoDimensionalArrayCompletesAcrossShortReads()
        {
            var source = new[,]
            {
                { new Sample(1, 2), new Sample(3, 4) },
                { new Sample(5, 6), new Sample(7, 8) }
            };
            var bytes = Serialize(writer => writer.WriteArray(source, source.Length));
            var destination = new Sample[2, 2];

            using (var reader = CreateReader(bytes))
            {
                Assert.AreEqual(source.Length, reader.ReadArray(destination, source.Length));
            }

            for (var x = 0; x < source.GetLength(0); x++)
                for (var y = 0; y < source.GetLength(1); y++)
                    Assert.AreEqual(source[x, y], destination[x, y]);
        }

        [Test]
        public static void ReadThreeDimensionalArrayCompletesAcrossShortReads()
        {
            var source = new[,,]
            {
                {
                    { new Sample(1, 2), new Sample(3, 4) },
                    { new Sample(5, 6), new Sample(7, 8) }
                }
            };
            var bytes = Serialize(writer => writer.WriteArray(source, source.Length));
            var destination = new Sample[1, 2, 2];

            using (var reader = CreateReader(bytes))
            {
                Assert.AreEqual(source.Length, reader.ReadArray(destination, source.Length));
            }

            for (var x = 0; x < source.GetLength(0); x++)
                for (var y = 0; y < source.GetLength(1); y++)
                    for (var z = 0; z < source.GetLength(2); z++)
                        Assert.AreEqual(source[x, y, z], destination[x, y, z]);
        }

        [Test]
        public static void ReadStructArrayReturnsCompleteElementsAtPartialEof()
        {
            var source = new[] { new Sample(1, 2), new Sample(3, 4), new Sample(5, 6) };
            var bytes = Serialize(writer => writer.WriteArray(source, 0, source.Length));
            Array.Resize(ref bytes, bytes.Length - 1);
            var destination = new Sample[source.Length];

            using (var reader = CreateReader(bytes))
            {
                Assert.AreEqual(2, reader.ReadArray(destination, 0, destination.Length));
            }

            Assert.AreEqual(source[0], destination[0]);
            Assert.AreEqual(source[1], destination[1]);
        }

        [Test]
        public static void ReadGuidSymbolCompletesAcrossShortReads()
        {
            var guid = new Guid("10325476-98ba-dcfe-0123-456789abcdef");
            var symbol = Symbol.Create(guid);
            var bytes = Serialize(writer => writer.WriteGuidSymbol(symbol));

            using (var reader = CreateReader(bytes))
            {
                Assert.AreEqual(symbol, reader.ReadGuidSymbol());
            }
        }

        [Test]
        public static void ReadGuidSymbolRejectsTruncatedInput()
        {
            var bytes = new Guid("10325476-98ba-dcfe-0123-456789abcdef").ToByteArray();
            Array.Resize(ref bytes, bytes.Length - 1);

            using (var reader = CreateReader(bytes))
            {
                Assert.Throws<EndOfStreamException>(() => reader.ReadGuidSymbol());
            }
        }
    }
}
