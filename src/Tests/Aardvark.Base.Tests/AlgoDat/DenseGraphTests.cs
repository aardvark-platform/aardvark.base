using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Tests
{
    [TestFixture]
    public class DenseGraphTests
    {
        [Test]
        public void PrimFindsGlobalMinimumAndTraversalsVisitTreeOnce()
        {
            var costs = new[,]
            {
                { 0,   1,   2,   3 },
                { 1,   0, 100, 100 },
                { 2, 100,   0, 100 },
                { 3, 100, 100,   0 }
            };

            var tree = CreateGraph(costs).BuildMinimumSpanningTreePrim();
            var vertices = new List<int>();
            var edges = new List<AbstractGraph<int, int>.Edge>();

            tree.Traverse(vertices.Add, edges.Add);

            Assert.AreEqual(4, tree.VertexCount);
            Assert.AreEqual(3, tree.EdgeCount);
            Assert.AreEqual(6, tree.Cost);
            Assert.AreEqual(4, vertices.Count);
            Assert.AreEqual(4, vertices.Distinct().Count());
            CollectionAssert.AreEquivalent(new[] { 0, 1, 2, 3 }, vertices);
            Assert.AreEqual(3, edges.Count);
            Assert.AreEqual(3, edges.Select(EdgeKey).Distinct().Count());
            CollectionAssert.AreEquivalent(
                new[] { (0, 1), (0, 2), (0, 3) },
                edges.Select(EdgeKey).ToArray());

            AssertEulerWalk(tree);
        }

        [Test]
        public void PrimBreaksEqualCostsByLowestVertexAndParentIndex()
        {
            var costs = new[,]
            {
                { 0, 1, 1, 1 },
                { 1, 0, 1, 1 },
                { 1, 1, 0, 1 },
                { 1, 1, 1, 0 }
            };

            var tree = CreateGraph(costs).BuildMinimumSpanningTreePrim();
            var vertices = new List<int>();
            var edges = new List<AbstractGraph<int, int>.Edge>();
            tree.Traverse(vertices.Add, edges.Add);

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3 }, vertices);
            CollectionAssert.AreEqual(
                new[] { (0, 1), (0, 2), (0, 3) },
                edges.Select(EdgeKey).ToArray());
        }

        [Test]
        public void EmptyAndSingletonTreesTraverseSafely()
        {
            var empty = new DenseGraph<int, int>(
                Array.Empty<int>(), (i, vertexI, j, vertexJ) => 0
            ).BuildMinimumSpanningTreePrim();

            int emptyVertices = 0;
            int emptyEdges = 0;
            int emptyEuler = 0;
            empty.Traverse(vertex => emptyVertices++, edge => emptyEdges++);
            empty.TraverseEuler(vertex => emptyEuler++);

            Assert.AreEqual(0, empty.VertexCount);
            Assert.AreEqual(0, empty.EdgeCount);
            Assert.AreEqual(0, empty.Cost);
            Assert.AreEqual(0, emptyVertices);
            Assert.AreEqual(0, emptyEdges);
            Assert.AreEqual(0, emptyEuler);

            var singleton = new DenseGraph<int, int>(
                new[] { 17 }, (i, vertexI, j, vertexJ) => 0
            ).BuildMinimumSpanningTreePrim();
            var singletonVertices = new List<int>();
            var singletonEdges = new List<AbstractGraph<int, int>.Edge>();
            var singletonEuler = new List<int>();

            singleton.Traverse(singletonVertices.Add, singletonEdges.Add);
            singleton.TraverseEuler(singletonEuler.Add);

            Assert.AreEqual(1, singleton.VertexCount);
            Assert.AreEqual(0, singleton.EdgeCount);
            Assert.AreEqual(0, singleton.Cost);
            CollectionAssert.AreEqual(new[] { 17 }, singletonVertices);
            Assert.IsEmpty(singletonEdges);
            CollectionAssert.AreEqual(new[] { 0 }, singletonEuler);
        }

        [TestCase(1)]
        [TestCase(17)]
        [TestCase(123456789)]
        public void PrimMatchesKruskalOnFixedDenseGraphs(int seed)
        {
            uint state = unchecked((uint)seed);

            for (int count = 2; count <= 12; count++)
            {
                var costs = new int[count, count];
                for (int i = 0; i < count; i++)
                {
                    for (int j = i + 1; j < count; j++)
                    {
                        state = state * 1664525U + 1013904223U;
                        int cost = 1 + (int)(state % 1000U);
                        costs[i, j] = cost;
                        costs[j, i] = cost;
                    }
                }

                var tree = CreateGraph(costs).BuildMinimumSpanningTreePrim();

                Assert.AreEqual(
                    KruskalCost(costs), tree.Cost,
                    $"seed {seed}, vertex count {count}");
                Assert.AreEqual(count - 1, tree.EdgeCount);
                AssertEulerWalk(tree);
            }
        }

        private static DenseGraph<int, int> CreateGraph(int[,] costs)
        {
            int count = costs.GetLength(0);
            var vertices = new int[count];
            for (int i = 0; i < count; i++) vertices[i] = i;

            return new DenseGraph<int, int>(
                vertices, (i, vertexI, j, vertexJ) => costs[i, j]
            );
        }

        private static (int, int) EdgeKey(AbstractGraph<int, int>.Edge edge)
            => (edge.Index0, edge.Index1);

        private static void AssertEulerWalk(AbstractGraph<int, int>.Tree tree)
        {
            var treeEdges = new HashSet<(int, int)>();
            tree.Traverse(vertex => { }, edge => treeEdges.Add(EdgeKey(edge)));

            var walk = new List<int>();
            tree.TraverseEuler(walk.Add);

            if (tree.VertexCount == 0)
            {
                Assert.IsEmpty(walk);
                return;
            }

            Assert.AreEqual(2 * tree.EdgeCount + 1, walk.Count);
            Assert.AreEqual(walk[0], walk[walk.Count - 1]);
            Assert.AreEqual(tree.VertexCount, walk.Distinct().Count());

            var traversals = new Dictionary<(int, int), int>();
            for (int i = 1; i < walk.Count; i++)
            {
                int a = Math.Min(walk[i - 1], walk[i]);
                int b = Math.Max(walk[i - 1], walk[i]);
                var edge = (a, b);

                Assert.IsTrue(treeEdges.Contains(edge), $"non-tree step {a}-{b}");
                traversals.TryGetValue(edge, out int count);
                traversals[edge] = count + 1;
            }

            CollectionAssert.AreEquivalent(treeEdges, traversals.Keys);
            Assert.IsTrue(traversals.Values.All(count => count == 2));
        }

        private static int KruskalCost(int[,] costs)
        {
            int count = costs.GetLength(0);
            var edges = new List<WeightedEdge>(count * (count - 1) / 2);
            for (int i = 0; i < count; i++)
                for (int j = i + 1; j < count; j++)
                    edges.Add(new WeightedEdge(i, j, costs[i, j]));

            edges.Sort((left, right) =>
            {
                int comparison = left.Cost.CompareTo(right.Cost);
                if (comparison != 0) return comparison;
                comparison = left.Vertex0.CompareTo(right.Vertex0);
                return comparison != 0
                    ? comparison
                    : left.Vertex1.CompareTo(right.Vertex1);
            });

            var parents = new int[count];
            for (int i = 0; i < count; i++) parents[i] = i;

            int result = 0;
            int accepted = 0;
            foreach (var edge in edges)
            {
                int root0 = FindRoot(parents, edge.Vertex0);
                int root1 = FindRoot(parents, edge.Vertex1);
                if (root0 == root1) continue;

                parents[root1] = root0;
                result += edge.Cost;
                if (++accepted == count - 1) break;
            }

            return result;
        }

        private static int FindRoot(int[] parents, int vertex)
        {
            while (parents[vertex] != vertex)
            {
                parents[vertex] = parents[parents[vertex]];
                vertex = parents[vertex];
            }

            return vertex;
        }

        private readonly struct WeightedEdge
        {
            public readonly int Vertex0;
            public readonly int Vertex1;
            public readonly int Cost;

            public WeightedEdge(int vertex0, int vertex1, int cost)
            {
                Vertex0 = vertex0;
                Vertex1 = vertex1;
                Cost = cost;
            }
        }
    }
}
