using Aardvark.Base.Coder;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

#pragma warning disable CA2022

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

        private sealed class StreamApi
        {
            internal readonly string Name;
            internal readonly ConstructorInfo Constructor;
            internal readonly PropertyInfo Chunks;
            internal readonly FieldInfo Chunk, Offset;
            internal readonly MethodInfo ChunkCount;

            internal StreamApi(string target)
            {
                Name = target;
                string path = Path.GetFullPath(Path.Combine(TestContext.CurrentContext.TestDirectory, "..", target, "Aardvark.Base.IO.dll"));
                Assert.That(File.Exists(path), Is.True, "Required actual IO target: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.That(assembly.Location, Is.EqualTo(path));
                Assert.That(assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,
                    Is.EqualTo(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0"));
                var type = assembly.GetType("Aardvark.Base.Coder.ChunkedMemoryStream", true);
                Assert.That(type.BaseType, Is.SameAs(typeof(Stream)), Name);
                Constructor = type.GetConstructor(new[] { typeof(long) });
                Chunks = type.GetProperty("ChunkList");
                Chunk = type.GetField("m_posChunk", BindingFlags.Instance | BindingFlags.NonPublic);
                Offset = type.GetField("m_posOffset", BindingFlags.Instance | BindingFlags.NonPublic);
                ChunkCount = type.GetMethod("ChunkCountForLength", BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.That(Constructor, Is.Not.Null, Name);
                Assert.That(Chunks.PropertyType, Is.EqualTo(typeof(List<byte[]>)), Name);
                Assert.That(Chunk.FieldType, Is.EqualTo(typeof(int)), Name);
                Assert.That(Offset.FieldType, Is.EqualTo(typeof(long)), Name);
                Assert.That(ChunkCount.ReturnType, Is.EqualTo(typeof(int)), Name);
                foreach (var method in new[] { type.GetProperty("Position").SetMethod, type.GetMethod("Seek"), type.GetMethod("SetLength"),
                    type.GetMethod("Read", new[] { typeof(byte[]), typeof(int), typeof(int) }), type.GetMethod("Write", new[] { typeof(byte[]), typeof(int), typeof(int) }), type.GetMethod("WriteByte") })
                    Assert.That(method.DeclaringType.Assembly, Is.SameAs(assembly), Name);
            }
            internal Stream Create(long size) => (Stream)Constructor.Invoke(new object[] { size });
            internal List<byte[]> Storage(Stream stream) => (List<byte[]>)Chunks.GetValue(stream);
        }

        private sealed class StreamState
        {
            private readonly long position, length, offset;
            private readonly int chunk, capacity;
            private readonly List<byte[]> storage;
            private readonly byte[][] references, contents;
            internal StreamState(StreamApi api, Stream stream)
            {
                position = stream.Position; length = stream.Length;
                chunk = (int)api.Chunk.GetValue(stream); offset = (long)api.Offset.GetValue(stream);
                storage = api.Storage(stream); capacity = storage.Capacity;
                references = storage.ToArray(); contents = new byte[references.Length][];
                for (int i = 0; i < references.Length; i++) contents[i] = (byte[])references[i].Clone();
            }
            internal void AssertSame(StreamApi api, Stream stream, string context, bool address = true)
            {
                if (address)
                {
                    Assert.That(stream.Position, Is.EqualTo(position), context + " position");
                    Assert.That(api.Chunk.GetValue(stream), Is.EqualTo(chunk), context + " chunk index");
                    Assert.That(api.Offset.GetValue(stream), Is.EqualTo(offset), context + " chunk offset");
                }
                Assert.That(stream.Length, Is.EqualTo(length), context + " length");
                Assert.That(api.Storage(stream), Is.SameAs(storage), context + " storage");
                Assert.That(storage.Capacity, Is.EqualTo(capacity), context + " capacity");
                Assert.That(storage.Count, Is.EqualTo(references.Length), context + " chunk count");
                for (int i = 0; i < references.Length; i++)
                {
                    Assert.That(storage[i], Is.SameAs(references[i]), context + " chunk identity " + i);
                    Assert.That(storage[i], Is.EqualTo(contents[i]), context + " chunk contents " + i);
                }
            }
        }

        private static void Address(Stream stream, long target, int mode)
        {
            if (mode == 0) stream.Position = target;
            else
            {
                long offset = mode == 1 ? target : mode == 2 ? target - stream.Position : target - stream.Length;
                Assert.That(stream.Seek(offset, (SeekOrigin)(mode - 1)), Is.EqualTo(target), "seek return");
            }
        }

        private static void Seed(Stream stream, int size)
        {
            var data = new byte[3 * size + 1];
            for (int i = 0; i < data.Length; i++) data[i] = (byte)(17 + i * 37);
            stream.Write(data, 0, data.Length);
            stream.Position = 1;
        }

        [Test]
        public static void OversizedAddressesRejectWithoutChangingStream()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                var api = new StreamApi(target);
                foreach (int size in new[] { 1, 2, 3, 7, 16, 257 })
                using (var stream = api.Create(size))
                {
                    Seed(stream, size);
                    foreach (long position in new[] { (1L << 32) * size, ((long)int.MaxValue + 1) * size,
                        (long)uint.MaxValue * size, (1L << 32) * size + size - 1, long.MaxValue })
                    foreach (int mode in new[] { 0, 1, 2, 3 })
                    {
                        string context = $"{target}: chunkSize={size}, position={position}, mode={mode}";
                        var state = new StreamState(api, stream);
                        Assert.Throws<IOException>(() => Address(stream, position, mode), context);
                        state.AssertSame(api, stream, context);
                        // Read/write at the retained cursor to check its internal addressing too.
                        int saved = stream.ReadByte(); stream.Position = 1;
                        stream.WriteByte((byte)(saved ^ 255)); stream.Position = 1;
                        Assert.That(stream.ReadByte(), Is.EqualTo(saved ^ 255), context + " retained cursor");
                        stream.Position = 1; stream.WriteByte((byte)saved); stream.Position = 1;
                        state.AssertSame(api, stream, context + " restored control");
                    }
                }
            }
        }

        [Test]
        public static void OversizedLengthsRejectBeforeAllocationOrMutation()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                var api = new StreamApi(target);
                foreach (int size in new[] { 1, 2, 3, 7, 16, 257 })
                using (var stream = api.Create(size))
                {
                    Seed(stream, size);
                    // Every original wrapped count here is nonpositive or one: no huge allocation,
                    // even when the original implementation is substituted for regression sensitivity.
                    foreach (long length in new[] { (long)int.MaxValue * size + 1, ((long)int.MaxValue + 1) * size,
                        ((long)int.MaxValue + 2) * size + 1, (1L << 32) * size + 1, ((1L << 32) + 1) * size })
                    {
                        string context = $"{target}: chunkSize={size}, length={length}";
                        var state = new StreamState(api, stream);
                        Assert.Throws<IOException>(() => stream.SetLength(length), context);
                        state.AssertSame(api, stream, context);
                    }
                }
            }
        }

        [Test]
        public static void RepresentableBoundariesAndOrdinaryTransfersStayCompatible()
        {
            foreach (string target in new[] { "net10.0", "netstandard2.0" })
            {
                var api = new StreamApi(target);
                foreach (int size in new[] { 1, 2, 3, 7, 16, 257 })
                {
                    using (var stream = api.Create(size))
                    {
                        Seed(stream, size);
                        var state = new StreamState(api, stream);
                        foreach (long position in new[] { (long)int.MaxValue * size - 1, (long)int.MaxValue * size,
                            (long)int.MaxValue * size + size - 1 })
                        foreach (int mode in new[] { 0, 1, 2, 3 })
                        {
                            string context = $"{target}: chunkSize={size}, boundary={position}, mode={mode}";
                            // Positioning only: never write or allocate at these representable extremes.
                            Address(stream, position, mode);
                            Assert.That(stream.Position, Is.EqualTo(position), context);
                            Assert.That(api.Chunk.GetValue(stream), Is.EqualTo((int)(position / size)), context);
                            Assert.That(api.Offset.GetValue(stream), Is.EqualTo(position % size), context);
                            state.AssertSame(api, stream, context, false);
                        }
                        Assert.That(api.ChunkCount.Invoke(stream, new object[] { (long)int.MaxValue * size }), Is.EqualTo(int.MaxValue), target + " representable count");
                        var boundaryState = new StreamState(api, stream);
                        Assert.Throws<IOException>(() => stream.Seek(long.MaxValue, SeekOrigin.Current), target + " checked current addition");
                        boundaryState.AssertSame(api, stream, target + " checked current addition");
                        Assert.Throws<IOException>(() => stream.Seek(long.MaxValue, SeekOrigin.End), target + " checked end addition");
                        boundaryState.AssertSame(api, stream, target + " checked end addition");
                    }
                    using (var stream = api.Create(size))
                    using (var reference = new MemoryStream())
                    {
                        var random = new Random(207187);
                        for (int step = 0; step < 256; step++)
                        {
                            string context = $"{target}: chunkSize={size}, step={step}";
                            switch (step % 6)
                            {
                                case 0:
                                    long position = random.Next(3 * size + 33);
                                    int mode = (step / 6) % 4; Address(stream, position, mode); Address(reference, position, mode); break;
                                case 1:
                                    var input = new byte[random.Next(1, 32)]; random.NextBytes(input);
                                    stream.Write(input, 0, input.Length); reference.Write(input, 0, input.Length); break;
                                case 2:
                                    byte value = (byte)random.Next(256); stream.WriteByte(value); reference.WriteByte(value); break;
                                case 3:
                                    var a = new byte[37]; var b = new byte[37]; Array.Fill(a, (byte)173); Array.Fill(b, (byte)173);
                                    int count = random.Next(1, 32); Assert.That(stream.Read(a, 2, count), Is.EqualTo(reference.Read(b, 2, count)), context);
                                    Assert.That(a, Is.EqualTo(b), context + " read bytes"); break;
                                case 4:
                                    long saved = reference.Position; long length = random.Next(4 * size + 37);
                                    stream.SetLength(length); reference.SetLength(length);
                                    // This stream retains position on shrink, unlike MemoryStream.
                                    reference.Position = saved; break;
                                case 5: Assert.That(stream.ReadByte(), Is.EqualTo(reference.ReadByte()), context); break;
                            }
                            Assert.That(stream.Position, Is.EqualTo(reference.Position), context + " position");
                            Assert.That(stream.Length, Is.EqualTo(reference.Length), context + " length");
                            var expected = reference.ToArray(); var chunks = api.Storage(stream);
                            for (int i = 0; i < expected.Length; i++) Assert.That(chunks[i / size][i % size], Is.EqualTo(expected[i]), context + " content offset=" + i);
                            Assert.That(chunks.Count * (long)size, Is.LessThanOrEqualTo(5L * size + 128), context + " allocation bound");
                        }
                    }
                }
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
