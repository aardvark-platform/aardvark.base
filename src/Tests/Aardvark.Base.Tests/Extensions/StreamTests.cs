using NUnit.Framework;
using System;
using System.Reflection;
using System.IO;
using Aardvark.Base;
using System.Runtime.ExceptionServices;

namespace Aardvark.Tests.Extensions
{
    internal class TestStream(Stream inner) : Stream
    {
        private bool isDiposed = false;
        private readonly RandomSystem rnd = new RandomSystem();

        public TestStream(byte[] data) : this(new MemoryStream(data))
        { }

        public override bool CanRead => inner.CanRead;

        public override bool CanSeek => inner.CanSeek;

        public override bool CanWrite => inner.CanWrite;

        public override long Length => inner.Length;

        public override long Position
        {
            get => inner.Position;
            set => inner.Position = value;
        }

        public override void Flush() => inner.Flush();

        public override long Seek(long offset, SeekOrigin origin) => inner.Seek(offset, origin);

        public override void SetLength(long value) => inner.SetLength(value);

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (count > 1)
            {
                count = 1 + rnd.UniformInt(count - 1);
            }

            return inner.Read(buffer, offset, count);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            inner.Write(buffer, offset, count);
        }

        protected override void Dispose(bool disposing)
        {
            if (!isDiposed) inner.Dispose();
            isDiposed = true;
        }
    }

    static class StreamTests
    {
        private static MethodInfo NetStandardReadBytesMethod
        {
            get
            {
                var outputDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                var assemblyPath = Path.GetFullPath(Path.Combine(outputDirectory!, "..", "netstandard2.0", "Aardvark.Base.dll"));

                Assert.That(
                    File.Exists(assemblyPath),
                    Is.True,
                    $"Expected sibling netstandard2.0 Aardvark.Base assembly at '{assemblyPath}'."
                );

                // We want to test the netstandard2.0 implementation, is there an easier way?
                var asm = Assembly.LoadFile(assemblyPath);
                var ext = asm.GetType($"Aardvark.Base.{nameof(StreamExtensions)}");
                Type[] parameters = [typeof(Stream), typeof(byte[]), typeof(int), typeof(int)];
                return ext.GetMethod(nameof(StreamExtensions.ReadBytes), parameters);
            }
        }

        private static void ReadBytes(bool netStandard20, Stream stream, byte[] buffer, int offset = 0, int count = -1)
        {
            if (netStandard20)
            {
                try
                {
                    NetStandardReadBytesMethod.Invoke(null, [stream, buffer, offset, count]);
                }
                catch (TargetInvocationException e) when (e.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                }
            }
            else
            {
                stream.ReadBytes(buffer, offset, count);
            }
        }

        [Test]
        public static void ReadBytes([Values(false, true)] bool netStandard20)
        {
            var rnd = new RandomSystem();
            var data = new byte[1234];
            for (int i = 0; i < data.Length; i++) data[i] = (byte)rnd.UniformInt();

            using TestStream s = new TestStream(data);
            var result = new byte[57];
            var offset = 3;
            var count = result.Length - offset;

            ReadBytes(netStandard20, s, result, offset, count);

            for (int i = 0; i < count; i++)
                Assert.AreEqual(data[i], result[i + offset]);
        }

        [Test]
        public static void ReadBytesRejectsNullStream([Values(false, true)] bool netStandard20)
        {
            var e = Assert.Throws<ArgumentNullException>(() => ReadBytes(netStandard20, null, new byte[1]));
            Assert.AreEqual("stream", e.ParamName);
        }

        [Test]
        public static void ReadBytesRejectsNullBufferWithDefaultCount([Values(false, true)] bool netStandard20)
        {
            using TestStream s = new TestStream([1]);

            var e = Assert.Throws<ArgumentNullException>(() => ReadBytes(netStandard20, s, null));
            Assert.AreEqual("buffer", e.ParamName);
        }

        [Test]
        public static void ReadBytesRejectsNegativeOffset([Values(false, true)] bool netStandard20)
        {
            using TestStream s = new TestStream([1]);

            var e = Assert.Throws<ArgumentOutOfRangeException>(() => ReadBytes(netStandard20, s, new byte[1], -1, 1));
            Assert.AreEqual("offset", e.ParamName);
        }

        [Test]
        public static void ReadBytesRejectsCountPastBufferEnd([Values(false, true)] bool netStandard20)
        {
            using TestStream s = new TestStream([1, 2]);

            var e = Assert.Throws<ArgumentException>(() => ReadBytes(netStandard20, s, new byte[2], 1, 2));
            Assert.AreEqual("count", e.ParamName);
        }

        [Test]
        public static void ReadBytesThrowsEndOfStreamWhenSourceIsShort([Values(false, true)] bool netStandard20)
        {
            using TestStream s = new TestStream([1]);

            Assert.Throws<EndOfStreamException>(() => ReadBytes(netStandard20, s, new byte[2], 0, 2));
        }
    }
}
