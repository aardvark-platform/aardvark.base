using NUnit.Framework;
using System;
using System.Reflection;
using System.IO;
using Aardvark.Base;
using System.Linq;

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

            if (netStandard20)
            {
                var assemblyPath = Path.Combine("..", "netstandard2.0", "Aardvark.Base.dll");
                if (!File.Exists(assemblyPath))
                {
                    Assert.Inconclusive("Could not find netstandard2.0 Aardvark.Base assembly.");
                }

                // We want to test the netstandard2.0 implementation, is there an easier way?
                var asm = Assembly.LoadFile(Path.GetFullPath(assemblyPath));
                var ext = asm.GetType($"Aardvark.Base.{nameof(StreamExtensions)}");
                Type[] parameters = [typeof(Stream), typeof(byte[]), typeof(int), typeof(int)];
                var mi = ext.GetMethod(nameof(StreamExtensions.ReadBytes), parameters);

                mi.Invoke(null, [s, result, offset, count]);
            }
            else
            {
                s.ReadBytes(result, offset, count);
            }

            for (int i = 0; i < count; i++)
                Assert.AreEqual(data[i], result[i + offset]);
        }
    }
}
