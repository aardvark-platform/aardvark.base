using System;
using System.Collections.Generic;
using System.IO;
using Aardvark.Base;
using Aardvark.Base.Coder;
using NUnit.Framework;

namespace Aardvark.Tests.IO
{
    [TestFixture]
    public class NetworkOrderBinaryTests
    {
        [Test]
        public void ScalarWritesUseExactBigEndianBytes()
        {
            var bytes = Write(writer =>
            {
                writer.Write((byte)0x12);
                writer.Write(unchecked((sbyte)0x9a));
                writer.Write((short)-2);
                writer.Write((ushort)0xabcd);
                writer.Write(0x12345678);
                writer.Write(0x89abcdefU);
                writer.Write(0x0123456789abcdefL);
                writer.Write(0xfedcba9876543210UL);
                writer.Write(Fun.FloatFromBits(unchecked((int)0xc1234567)));
                writer.Write(Fun.FloatFromBits(unchecked((long)0x8000000000000000)));
            });

            CollectionAssert.AreEqual(new byte[]
            {
                0x12, 0x9a,
                0xff, 0xfe,
                0xab, 0xcd,
                0x12, 0x34, 0x56, 0x78,
                0x89, 0xab, 0xcd, 0xef,
                0x01, 0x23, 0x45, 0x67, 0x89, 0xab, 0xcd, 0xef,
                0xfe, 0xdc, 0xba, 0x98, 0x76, 0x54, 0x32, 0x10,
                0xc1, 0x23, 0x45, 0x67,
                0x80, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            }, bytes);
        }

        [Test]
        public void IntegerBoundariesRoundTrip()
        {
            var signedByteValues = new[] { sbyte.MinValue, (sbyte)-1, (sbyte)0, (sbyte)1, sbyte.MaxValue };
            var byteValues = new[] { byte.MinValue, (byte)1, (byte)0x80, byte.MaxValue };
            var int16Values = new[] { short.MinValue, (short)-1, (short)0, (short)1, short.MaxValue };
            var uint16Values = new[] { ushort.MinValue, (ushort)1, (ushort)0x8000, ushort.MaxValue };
            var int32Values = new[] { int.MinValue, -1, 0, 1, int.MaxValue };
            var uint32Values = new[] { uint.MinValue, 1U, 0x80000000U, uint.MaxValue };
            var int64Values = new[] { long.MinValue, -1L, 0L, 1L, long.MaxValue };
            var uint64Values = new[] { ulong.MinValue, 1UL, 0x8000000000000000UL, ulong.MaxValue };

            var bytes = Write(writer =>
            {
                foreach (var value in signedByteValues) writer.Write(value);
                foreach (var value in byteValues) writer.Write(value);
                foreach (var value in int16Values) writer.Write(value);
                foreach (var value in uint16Values) writer.Write(value);
                foreach (var value in int32Values) writer.Write(value);
                foreach (var value in uint32Values) writer.Write(value);
                foreach (var value in int64Values) writer.Write(value);
                foreach (var value in uint64Values) writer.Write(value);
            });

            using var reader = Reader(bytes);
            foreach (var expected in signedByteValues) Assert.AreEqual(expected, reader.ReadSByte());
            foreach (var expected in byteValues) Assert.AreEqual(expected, reader.ReadByte());
            foreach (var expected in int16Values) Assert.AreEqual(expected, reader.ReadInt16());
            foreach (var expected in uint16Values) Assert.AreEqual(expected, reader.ReadUInt16());
            foreach (var expected in int32Values) Assert.AreEqual(expected, reader.ReadInt32());
            foreach (var expected in uint32Values) Assert.AreEqual(expected, reader.ReadUInt32());
            foreach (var expected in int64Values) Assert.AreEqual(expected, reader.ReadInt64());
            foreach (var expected in uint64Values) Assert.AreEqual(expected, reader.ReadUInt64());
            Assert.AreEqual(bytes.Length, reader.BaseStream.Position);
        }

        [Test]
        public void FloatingPointBitPatternsRoundTripExactly()
        {
            var singleBits = new[]
            {
                0x00000000,
                unchecked((int)0x80000000),
                0x00000001,
                0x7f800000,
                unchecked((int)0xff800000),
                0x7fc12345,
                unchecked((int)0xffa54321),
            };
            var doubleBits = new[]
            {
                0x0000000000000000L,
                unchecked((long)0x8000000000000000),
                0x0000000000000001L,
                0x7ff0000000000000L,
                unchecked((long)0xfff0000000000000),
                0x7ff8123456789abcL,
                unchecked((long)0xfff123456789abcd),
            };

            var bytes = Write(writer =>
            {
                foreach (var bits in singleBits) writer.Write(Fun.FloatFromBits(bits));
                foreach (var bits in doubleBits) writer.Write(Fun.FloatFromBits(bits));
            });

            var expectedBytes = new List<byte>();
            foreach (var bits in singleBits) AppendBigEndian(expectedBytes, bits);
            foreach (var bits in doubleBits) AppendBigEndian(expectedBytes, bits);
            CollectionAssert.AreEqual(expectedBytes, bytes);

            using var reader = Reader(bytes);
            foreach (var expected in singleBits)
                Assert.AreEqual(expected, Fun.FloatToBits(reader.ReadSingle()));
            foreach (var expected in doubleBits)
                Assert.AreEqual(expected, Fun.FloatToBits(reader.ReadDouble()));
        }

        [Test]
        public void VectorAndColorWritesPreserveComponentOrderAndBits()
        {
            var f0 = Fun.FloatFromBits(0x3f800000);
            var f1 = Fun.FloatFromBits(unchecked((int)0xc0200000));
            var f2 = Fun.FloatFromBits(0x7fc12345);
            var f3 = Fun.FloatFromBits(unchecked((int)0x80000000));
            var d0 = Fun.FloatFromBits(0x3ff0000000000000L);
            var d1 = Fun.FloatFromBits(unchecked((long)0x8000000000000000));
            var d2 = Fun.FloatFromBits(0x7ff8123456789abcL);

            var bytes = Write(writer =>
            {
                writer.Write(new V2f(f0, f1));
                writer.Write(new V2d(d0, d1));
                writer.Write(new V3f(f2, f3, f0));
                writer.Write(new V3d(d2, d1, d0));
                writer.Write(new C3f(f1, f2, f3));
                writer.Write(new C4f(f3, f0, f1, f2));
            });

            var expected = new List<byte>();
            AppendBigEndian(expected, 0x3f800000, unchecked((int)0xc0200000));
            AppendBigEndian(expected, 0x3ff0000000000000L, unchecked((long)0x8000000000000000));
            AppendBigEndian(expected, 0x7fc12345, unchecked((int)0x80000000), 0x3f800000);
            AppendBigEndian(expected, 0x7ff8123456789abcL, unchecked((long)0x8000000000000000), 0x3ff0000000000000L);
            AppendBigEndian(expected, unchecked((int)0xc0200000), 0x7fc12345, unchecked((int)0x80000000));
            AppendBigEndian(expected, unchecked((int)0x80000000), 0x3f800000, unchecked((int)0xc0200000), 0x7fc12345);
            CollectionAssert.AreEqual(expected, bytes);

            using var reader = Reader(bytes);
            AssertBits(new[] { 0x3f800000, unchecked((int)0xc0200000) }, reader.ReadV2f());
            Assert.AreEqual(0x3ff0000000000000L, Fun.FloatToBits(reader.ReadDouble()));
            Assert.AreEqual(unchecked((long)0x8000000000000000), Fun.FloatToBits(reader.ReadDouble()));
            AssertBits(new[] { 0x7fc12345, unchecked((int)0x80000000), 0x3f800000 }, reader.ReadV3f());
            Assert.AreEqual(0x7ff8123456789abcL, Fun.FloatToBits(reader.ReadDouble()));
            Assert.AreEqual(unchecked((long)0x8000000000000000), Fun.FloatToBits(reader.ReadDouble()));
            Assert.AreEqual(0x3ff0000000000000L, Fun.FloatToBits(reader.ReadDouble()));
            AssertBits(new[] { unchecked((int)0xc0200000), 0x7fc12345, unchecked((int)0x80000000) }, reader.ReadC3f());
            AssertBits(new[] { unchecked((int)0x80000000), 0x3f800000, unchecked((int)0xc0200000), 0x7fc12345 }, reader.ReadC4f());
        }

        [Test]
        public void TruncatedScalarsThrowEndOfStreamException()
        {
            AssertTruncated(sizeof(short), reader => reader.ReadInt16());
            AssertTruncated(sizeof(ushort), reader => reader.ReadUInt16());
            AssertTruncated(sizeof(int), reader => reader.ReadInt32());
            AssertTruncated(sizeof(uint), reader => reader.ReadUInt32());
            AssertTruncated(sizeof(long), reader => reader.ReadInt64());
            AssertTruncated(sizeof(ulong), reader => reader.ReadUInt64());
            AssertTruncated(sizeof(float), reader => reader.ReadSingle());
            AssertTruncated(sizeof(double), reader => reader.ReadDouble());
        }

        private static byte[] Write(Action<NetworkOrderBinaryWriter> action)
        {
            using var stream = new MemoryStream();
            using (var writer = new NetworkOrderBinaryWriter(stream))
            {
                action(writer);
                writer.Flush();
                return stream.ToArray();
            }
        }

        private static NetworkOrderBinaryReader Reader(byte[] bytes)
            => new NetworkOrderBinaryReader(new MemoryStream(bytes, false));

        private static void AssertTruncated(int width, Action<NetworkOrderBinaryReader> read)
        {
            for (int length = 0; length < width; length++)
            {
                using var reader = Reader(new byte[length]);
                Assert.Throws<EndOfStreamException>(() => read(reader), $"Width {width}, length {length}");
            }
        }

        private static void AppendBigEndian(List<byte> bytes, params int[] values)
        {
            foreach (var value in values)
            {
                bytes.Add((byte)(value >> 24));
                bytes.Add((byte)(value >> 16));
                bytes.Add((byte)(value >> 8));
                bytes.Add((byte)value);
            }
        }

        private static void AppendBigEndian(List<byte> bytes, params long[] values)
        {
            foreach (var value in values)
            {
                bytes.Add((byte)(value >> 56));
                bytes.Add((byte)(value >> 48));
                bytes.Add((byte)(value >> 40));
                bytes.Add((byte)(value >> 32));
                bytes.Add((byte)(value >> 24));
                bytes.Add((byte)(value >> 16));
                bytes.Add((byte)(value >> 8));
                bytes.Add((byte)value);
            }
        }

        private static void AssertBits(int[] expected, V2f actual)
        {
            Assert.AreEqual(expected[0], Fun.FloatToBits(actual.X));
            Assert.AreEqual(expected[1], Fun.FloatToBits(actual.Y));
        }

        private static void AssertBits(int[] expected, V3f actual)
        {
            Assert.AreEqual(expected[0], Fun.FloatToBits(actual.X));
            Assert.AreEqual(expected[1], Fun.FloatToBits(actual.Y));
            Assert.AreEqual(expected[2], Fun.FloatToBits(actual.Z));
        }

        private static void AssertBits(int[] expected, C3f actual)
        {
            Assert.AreEqual(expected[0], Fun.FloatToBits(actual.R));
            Assert.AreEqual(expected[1], Fun.FloatToBits(actual.G));
            Assert.AreEqual(expected[2], Fun.FloatToBits(actual.B));
        }

        private static void AssertBits(int[] expected, C4f actual)
        {
            Assert.AreEqual(expected[0], Fun.FloatToBits(actual.R));
            Assert.AreEqual(expected[1], Fun.FloatToBits(actual.G));
            Assert.AreEqual(expected[2], Fun.FloatToBits(actual.B));
            Assert.AreEqual(expected[3], Fun.FloatToBits(actual.A));
        }
    }
}
