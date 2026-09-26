using BenchmarkDotNet.Attributes;

namespace Aardvark.Base.Benchmarks
{
    [MemoryDiagnoser]
    public class PrimeBenchmark
    {
        private int[] m_indices;
        private long[] m_values;

        [GlobalSetup]
        public void Setup()
        {
            Prime.WithIndex(8191);

            m_indices = new int[1024];
            for (int i = 0; i < m_indices.Length; i++)
                m_indices[i] = (i * 4051) & 8191;

            m_values = new long[256];
            for (int i = 0; i < m_values.Length; i++)
                m_values[i] = 2 + (i * 7919L) % 96_000L;
        }

        [Benchmark]
        public long CachedWithIndex()
        {
            long sum = 0;
            for (int i = 0; i < m_indices.Length; i++)
                sum += Prime.WithIndex(m_indices[i]);
            return sum;
        }

        [Benchmark]
        public double CachedInverseWithIndex()
        {
            double sum = 0.0;
            for (int i = 0; i < m_indices.Length; i++)
                sum += Prime.InverseWithIndex(m_indices[i]);
            return sum;
        }

        [Benchmark]
        public int IsTrueFor()
        {
            int count = 0;
            for (int i = 0; i < m_values.Length; i++)
                if (Prime.IsTrueFor(m_values[i])) count++;
            return count;
        }
    }
}
