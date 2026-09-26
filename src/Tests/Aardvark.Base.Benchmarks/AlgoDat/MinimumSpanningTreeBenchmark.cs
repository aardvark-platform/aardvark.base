using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    /// <summary>
    /// Run with: dotnet run -c Release --project src/Tests/Aardvark.Base.Benchmarks -- --filter '*MinimumSpanningTreeBenchmark*'
    /// </summary>
    [MemoryDiagnoser]
    public class MinimumSpanningTreeBenchmark
    {
        private ((int, int), int)[] m_longPath;
        private ((int, int), int)[] m_sparse;
        private ((int, int), int)[] m_dense;

        [GlobalSetup]
        public void Setup()
        {
            Report.RootTarget = Report.NoTarget;
            m_longPath = CreatePath(4096);
            m_sparse = CreateSparse(2048, 8192, 12345);
            m_dense = CreateDense(256, 54321);
        }

        [Benchmark]
        public long LongPath() => Consume(m_longPath);

        [Benchmark]
        public long SparseConnected() => Consume(m_sparse);

        [Benchmark]
        public long DenseConnected() => Consume(m_dense);

        private static long Consume(IEnumerable<((int, int), int)> edges)
        {
            long checksum = 0;
            int count = 0;
            foreach (var edge in MinimumSpanningTree.Create(edges))
            {
                checksum += edge.Item2;
                count++;
            }
            return checksum + count;
        }

        private static ((int, int), int)[] CreatePath(int vertexCount)
        {
            var edges = new ((int, int), int)[vertexCount - 1];
            for (int vertex = 1; vertex < vertexCount; vertex++)
                edges[vertex - 1] = ((vertex - 1, vertex), 1 + vertex % 97);
            return edges;
        }

        private static ((int, int), int)[] CreateSparse(int vertexCount, int edgeCount, int seed)
        {
            var random = new Random(seed);
            var edges = new List<((int, int), int)>(edgeCount);
            for (int vertex = 1; vertex < vertexCount; vertex++)
                edges.Add(((random.Next(vertex), vertex), random.Next(1, 1_000_001)));

            while (edges.Count < edgeCount)
            {
                int v0 = random.Next(vertexCount);
                int v1 = random.Next(vertexCount);
                if (v0 == v1) continue;
                edges.Add(((v0, v1), random.Next(1, 1_000_001)));
            }
            return edges.ToArray();
        }

        private static ((int, int), int)[] CreateDense(int vertexCount, int seed)
        {
            var random = new Random(seed);
            var edges = new ((int, int), int)[vertexCount * (vertexCount - 1) / 2];
            int index = 0;
            for (int v0 = 0; v0 < vertexCount; v0++)
                for (int v1 = v0 + 1; v1 < vertexCount; v1++)
                    edges[index++] = ((v0, v1), random.Next(1, 1_000_001));
            return edges;
        }
    }
}
