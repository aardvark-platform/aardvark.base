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
            using (var stream = new ChunkedMemoryStream(2))
            {
                stream.Write(new byte[] { 1, 2, 3 }, 0, 3);
                stream.Position = 1;

                AssertParamName<ArgumentNullException>("buffer", () => stream.Write(null, 0, 1));
                AssertUnchanged(stream);

                AssertParamName<ArgumentOutOfRangeException>("offset", () => stream.Write(new byte[2], -1, 1));
                AssertUnchanged(stream);

                AssertParamName<ArgumentOutOfRangeException>("count", () => stream.Write(new byte[2], 0, -1));
                AssertUnchanged(stream);

                AssertParamName<ArgumentException>("count", () => stream.Write(new byte[2], 1, 2));
                AssertUnchanged(stream);
            }

            void AssertUnchanged(ChunkedMemoryStream stream)
            {
                Assert.AreEqual(1, stream.Position);
                Assert.AreEqual(3, stream.Length);
                CollectionAssert.AreEqual(new byte[] { 1, 2, 3 }, ReadAll(stream));
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
