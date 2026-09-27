using System;
using System.IO;
using Aardvark.Base.Coder;
using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks.IO
{
    /// <summary>
    /// Run with: dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*NetworkOrderBinaryBenchmark*'
    /// </summary>
    [MemoryDiagnoser]
    public class NetworkOrderBinaryBenchmark
    {
        private const int Count = 1024;
        private const int BufferSize = 64 * 1024;

        private MemoryStream m_writeStream;
        private NetworkOrderBinaryWriter m_writer;
        private MemoryStream m_intReadStream;
        private NetworkOrderBinaryReader m_intReader;
        private MemoryStream m_doubleReadStream;
        private NetworkOrderBinaryReader m_doubleReader;
        private MemoryStream m_aggregateReadStream;
        private NetworkOrderBinaryReader m_aggregateReader;

        private readonly int m_intValue = unchecked((int)0x89abcdef);
        private readonly double m_doubleValue = -12345.6789012345;
        private readonly V3d m_vectorValue = new V3d(-1.25, 2.5, 123456.75);
        private readonly C4f m_colorValue = new C4f(0.125f, -0.25f, 0.5f, 1.0f);

        [GlobalSetup]
        public void Setup()
        {
            m_writeStream = WritableStream();
            m_writer = new NetworkOrderBinaryWriter(m_writeStream);

            m_intReadStream = CreatePayload(writer => writer.Write(m_intValue));
            m_intReader = new NetworkOrderBinaryReader(m_intReadStream);

            m_doubleReadStream = CreatePayload(writer => writer.Write(m_doubleValue));
            m_doubleReader = new NetworkOrderBinaryReader(m_doubleReadStream);

            m_aggregateReadStream = CreatePayload(writer =>
            {
                writer.Write((float)m_vectorValue.X);
                writer.Write((float)m_vectorValue.Y);
                writer.Write((float)m_vectorValue.Z);
                writer.Write(m_colorValue);
            });
            m_aggregateReader = new NetworkOrderBinaryReader(m_aggregateReadStream);
        }

        [GlobalCleanup]
        public void Cleanup()
        {
            m_writer.Dispose();
            m_intReader.Dispose();
            m_doubleReader.Dispose();
            m_aggregateReader.Dispose();
        }

        [Benchmark]
        public long WriteInt32()
        {
            m_writeStream.Position = 0;
            for (int i = 0; i < Count; i++) m_writer.Write(m_intValue);
            return m_writeStream.Position;
        }

        [Benchmark]
        public long WriteDouble()
        {
            m_writeStream.Position = 0;
            for (int i = 0; i < Count; i++) m_writer.Write(m_doubleValue);
            return m_writeStream.Position;
        }

        [Benchmark]
        public long ReadInt32()
        {
            m_intReadStream.Position = 0;
            long sum = 0;
            for (int i = 0; i < Count; i++) sum += m_intReader.ReadInt32();
            return sum;
        }

        [Benchmark]
        public double ReadDouble()
        {
            m_doubleReadStream.Position = 0;
            double sum = 0.0;
            for (int i = 0; i < Count; i++) sum += m_doubleReader.ReadDouble();
            return sum;
        }

        [Benchmark]
        public long WriteAggregates()
        {
            m_writeStream.Position = 0;
            for (int i = 0; i < Count; i++)
            {
                m_writer.Write(m_vectorValue);
                m_writer.Write(m_colorValue);
            }
            return m_writeStream.Position;
        }

        [Benchmark]
        public double ReadAggregates()
        {
            m_aggregateReadStream.Position = 0;
            double sum = 0.0;
            for (int i = 0; i < Count; i++)
            {
                var vector = m_aggregateReader.ReadV3f();
                var color = m_aggregateReader.ReadC4f();
                sum += vector.X + vector.Y + vector.Z + color.R + color.G + color.B + color.A;
            }
            return sum;
        }

        private static MemoryStream WritableStream()
            => new MemoryStream(new byte[BufferSize], 0, BufferSize, true, true);

        private static MemoryStream CreatePayload(Action<NetworkOrderBinaryWriter> writeOne)
        {
            using var stream = new MemoryStream();
            using var writer = new NetworkOrderBinaryWriter(stream);
            for (int i = 0; i < Count; i++) writeOne(writer);
            writer.Flush();
            return new MemoryStream(stream.ToArray(), false);
        }
    }
}
