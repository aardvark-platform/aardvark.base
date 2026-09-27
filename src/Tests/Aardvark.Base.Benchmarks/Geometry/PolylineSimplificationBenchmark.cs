using BenchmarkDotNet.Attributes;
using System;

namespace Aardvark.Base.Benchmarks
{
    [MemoryDiagnoser]
    public class PolylineSimplificationBenchmark
    {
        private V2d[] m_flat;
        private V2d[] m_wavy;
        private V2d[] m_splitHeavy;

        [GlobalSetup]
        public void Setup()
        {
            m_flat = new V2d[4096];
            m_wavy = new V2d[4096];
            for (int i = 0; i < m_flat.Length; i++)
            {
                double x = i * 0.01;
                m_flat[i] = new V2d(x, 0.0);
                m_wavy[i] = new V2d(x, Math.Sin(i * 0.05) + 0.1 * Math.Sin(i * 0.17));
            }

            m_splitHeavy = new V2d[512];
            for (int i = 0; i < m_splitHeavy.Length; i++)
                m_splitHeavy[i] = new V2d(i, (i & 1) == 0 ? -1.0 : 1.0);
        }

        [Benchmark]
        public int[] Flat()
            => m_flat.Simplify(0.001);

        [Benchmark]
        public int[] Wavy()
            => m_wavy.Simplify(0.02);

        [Benchmark]
        public int[] SplitHeavy()
            => m_splitHeavy.Simplify(0.0);
    }
}
