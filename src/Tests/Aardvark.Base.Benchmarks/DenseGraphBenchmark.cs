using BenchmarkDotNet.Attributes;
using System;

namespace Aardvark.Base.Benchmarks
{
    [MemoryDiagnoser]
    public class DenseGraphBenchmark
    {
        [Params(32, 128, 512)]
        public int VertexCount;

        private DenseGraph<int, int> m_graph;

        [GlobalSetup]
        public void Setup()
        {
            var vertices = new int[VertexCount];
            for (int i = 0; i < vertices.Length; i++)
                vertices[i] = i;

            m_graph = new DenseGraph<int, int>(vertices, GetCost);
        }

        [Benchmark]
        public AbstractGraph<int, int>.Tree BuildMinimumSpanningTreePrim()
            => m_graph.BuildMinimumSpanningTreePrim();

        private static int GetCost(int i, int vertexI, int j, int vertexJ)
        {
            if (i == j) return 0;

            int min = Math.Min(i, j);
            int max = Math.Max(i, j);
            return 1 + (int)(((long)min * 7919 + (long)max * 104729) % 10000);
        }
    }
}
