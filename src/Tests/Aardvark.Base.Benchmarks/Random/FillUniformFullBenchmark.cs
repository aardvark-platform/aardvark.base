using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace Aardvark.Base.Benchmarks
{
    // From the repository root:
    // dotnet run --project src/Tests/Aardvark.Base.Benchmarks -c Release -- --filter '*FillUniformFullBenchmark*'
    [MemoryDiagnoser]
    public class FillUniformFullBenchmark
    {
        private sealed class BenchmarkRandom : IRandomUniform
        {
            private readonly bool m_generatesFullDoubles;
            private ulong m_state;

            public BenchmarkRandom(bool generatesFullDoubles, ulong seed)
            {
                m_generatesFullDoubles = generatesFullDoubles;
                m_state = seed;
            }

            public int RandomBits => m_generatesFullDoubles ? 53 : 31;
            public bool GeneratesFullDoubles => m_generatesFullDoubles;

            public void ReSeed(int seed) => m_state = (uint)seed + 1UL;

            public int UniformInt() => (int)(Next() >> 33);
            public uint UniformUInt() => (uint)Next();
            public long UniformLong() => (long)(Next() >> 1);
            public ulong UniformULong() => Next();
            public float UniformFloat() => (Next() >> 40) * (1.0f / 16777216.0f);
            public float UniformFloatClosed() => throw new NotSupportedException();
            public float UniformFloatOpen() => throw new NotSupportedException();
            public double UniformDouble() => (Next() >> 11) * (1.0 / 9007199254740992.0);
            public double UniformDoubleClosed() => throw new NotSupportedException();
            public double UniformDoubleOpen() => throw new NotSupportedException();

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            private ulong Next()
            {
                ulong value = m_state;
                value ^= value >> 12;
                value ^= value << 25;
                value ^= value >> 27;
                m_state = value;
                return value * 2685821657736338717UL;
            }
        }

        private IRandomUniform m_bulkRandom;
        private IRandomUniform m_scalarRandom;
        private double[] m_values;

        [Params(false, true)]
        public bool NativeFull { get; set; }

        [Params(1024)]
        public int Count { get; set; }

        [GlobalSetup]
        public void Setup()
        {
            m_bulkRandom = new BenchmarkRandom(NativeFull, 0x9e3779b97f4a7c15UL);
            m_scalarRandom = new BenchmarkRandom(NativeFull, 0x9e3779b97f4a7c15UL);
            m_values = new double[Count];
        }

        [Benchmark(Baseline = true)]
        public double ScalarLoop()
        {
            var random = m_scalarRandom;
            var values = m_values;
            for (int i = 0; i < values.Length; i++)
                values[i] = random.UniformDoubleFull();
            return values[values.Length - 1];
        }

        [Benchmark]
        public double BulkFill()
        {
            m_bulkRandom.FillUniformFull(m_values);
            return m_values[m_values.Length - 1];
        }
    }
}
