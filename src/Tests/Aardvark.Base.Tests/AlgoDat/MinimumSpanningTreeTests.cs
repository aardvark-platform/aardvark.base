using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Tests
{
    [TestFixture]
    public class MinimumSpanningTreeTests
    {
        private sealed class SingleUseEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> m_values;
            private bool m_enumerated;

            public SingleUseEnumerable(IEnumerable<T> values)
            {
                m_values = values;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (m_enumerated)
                    throw new InvalidOperationException("The sequence was enumerated more than once.");

                m_enumerated = true;
                return m_values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Test]
        public void CreateBuildsMinimumSpanningTreeForConnectedGraph()
        {
            var tree = MinimumSpanningTree.Create(WeightedGraph()).ToArray();

            Assert.AreEqual(6, tree.Length);
            Assert.AreEqual(39, tree.Sum(e => e.Item2));
            AssertSpans(tree, "A", "B", "C", "D", "E", "F", "G");
        }

        [Test]
        public void CreateEnumeratesInputAtMostOnce()
        {
            var edges = new SingleUseEnumerable<((string, string), int)>(SmallWeightedGraph());

            var tree = MinimumSpanningTree.Create(edges).ToArray();

            Assert.AreEqual(3, tree.Length);
            Assert.AreEqual(6, tree.Sum(e => e.Item2));
            AssertSpans(tree, "A", "B", "C", "D");
        }

        [Test]
        public void CreateReturnsNoEdgesForEmptyAndSingleVertexGraphs()
        {
            var empty = MinimumSpanningTree.Create(Array.Empty<((string, string), int)>()).ToArray();
            var singleVertex = MinimumSpanningTree.Create(new[]
            {
                (("A", "A"), -10),
                (("A", "A"), 5),
            }).ToArray();

            Assert.That(empty, Is.Empty);
            Assert.That(singleVertex, Is.Empty);
        }

        [Test]
        public void CreateRejectsDisconnectedGraphsClearly()
        {
            var edges = new[]
            {
                (("A", "B"), 1),
                (("C", "D"), 2),
            };

            var exception = Assert.Throws<InvalidOperationException>(
                () => MinimumSpanningTree.Create(edges).ToArray());

            StringAssert.Contains("disconnected", exception.Message.ToLowerInvariant());
        }

        [Test]
        public void CreateUsesSourceOrderForEqualWeightCandidates()
        {
            var edges = new[]
            {
                (("A", "B"), 1),
                (("A", "C"), 1),
                (("B", "D"), 1),
                (("C", "D"), 1),
            };

            var tree = MinimumSpanningTree.Create(edges).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                (("A", "B"), 1),
                (("A", "C"), 1),
                (("B", "D"), 1),
            }, tree);
        }

        [Test]
        public void CreateIgnoresSelfLoopsAndChoosesCheapestParallelEdge()
        {
            var edges = new[]
            {
                (("A", "A"), -100),
                (("A", "B"), 5),
                (("B", "A"), 2),
                (("B", "C"), 4),
                (("A", "C"), 10),
                (("C", "C"), -100),
            };

            var tree = MinimumSpanningTree.Create(edges).ToArray();

            CollectionAssert.AreEqual(new[]
            {
                (("A", "B"), 2),
                (("B", "C"), 4),
            }, tree);
        }

        [Test]
        public void CreateMatchesReferenceKruskalOnRandomConnectedGraphs()
        {
            var random = new Random(19770317);
            for (int iteration = 0; iteration < 200; iteration++)
            {
                int vertexCount = random.Next(1, 13);
                var edges = new List<((int, int), int)>();

                if (vertexCount == 1)
                {
                    edges.Add(((0, 0), random.Next(-20, 21)));
                }
                else
                {
                    // Establish connectivity, with arbitrary source orientation.
                    for (int vertex = 1; vertex < vertexCount; vertex++)
                    {
                        int parent = random.Next(vertex);
                        int weight = random.Next(-20, 21);
                        edges.Add(random.Next(2) == 0
                            ? ((parent, vertex), weight)
                            : ((vertex, parent), weight));
                    }
                }

                // Include ties, parallel edges, and self-loops.
                int extraCount = random.Next(vertexCount * vertexCount + 1);
                for (int i = 0; i < extraCount; i++)
                {
                    int v0 = random.Next(vertexCount);
                    int v1 = random.Next(vertexCount);
                    edges.Add(((v0, v1), random.Next(-20, 21)));
                }

                var tree = MinimumSpanningTree.Create(edges).ToArray();

                Assert.AreEqual(vertexCount - 1, tree.Length, $"iteration {iteration}");
                AssertConnected(tree, vertexCount, iteration);
                Assert.AreEqual(
                    ReferenceKruskalWeight(edges, vertexCount),
                    tree.Sum(edge => edge.Item2),
                    $"iteration {iteration}");
            }
        }

        [Test]
        public void CreateRejectsNullEdges()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = MinimumSpanningTree.Create<string, int>(null);
            });

            Assert.AreEqual("edges", ex.ParamName);
        }

        private static int ReferenceKruskalWeight(
            IReadOnlyList<((int, int), int)> edges,
            int vertexCount)
        {
            var parent = Enumerable.Range(0, vertexCount).ToArray();
            var rank = new byte[vertexCount];
            int weight = 0;
            int count = 0;

            foreach (var item in edges
                .Select((edge, index) => (Edge: edge, Index: index))
                .OrderBy(item => item.Edge.Item2)
                .ThenBy(item => item.Index))
            {
                var endpoints = item.Edge.Item1;
                if (!Union(parent, rank, endpoints.Item1, endpoints.Item2)) continue;
                weight += item.Edge.Item2;
                if (++count == vertexCount - 1) break;
            }

            Assert.AreEqual(vertexCount - 1, count);
            return weight;
        }

        private static void AssertConnected(
            IEnumerable<((int, int), int)> tree,
            int vertexCount,
            int iteration)
        {
            var parent = Enumerable.Range(0, vertexCount).ToArray();
            var rank = new byte[vertexCount];
            foreach (var edge in tree)
            {
                Assert.That(
                    Union(parent, rank, edge.Item1.Item1, edge.Item1.Item2),
                    Is.True,
                    $"cycle in iteration {iteration}");
            }

            int root = Find(parent, 0);
            for (int vertex = 1; vertex < vertexCount; vertex++)
                Assert.AreEqual(root, Find(parent, vertex), $"iteration {iteration}, vertex {vertex}");
        }

        private static bool Union(int[] parent, byte[] rank, int left, int right)
        {
            int leftRoot = Find(parent, left);
            int rightRoot = Find(parent, right);
            if (leftRoot == rightRoot) return false;

            if (rank[leftRoot] < rank[rightRoot])
            {
                parent[leftRoot] = rightRoot;
            }
            else
            {
                parent[rightRoot] = leftRoot;
                if (rank[leftRoot] == rank[rightRoot]) rank[leftRoot]++;
            }
            return true;
        }

        private static int Find(int[] parent, int vertex)
        {
            while (parent[vertex] != vertex)
            {
                parent[vertex] = parent[parent[vertex]];
                vertex = parent[vertex];
            }
            return vertex;
        }

        private static IEnumerable<((string, string), int)> WeightedGraph()
        {
            yield return (("A", "B"), 7);
            yield return (("A", "D"), 5);
            yield return (("D", "B"), 9);
            yield return (("B", "C"), 8);
            yield return (("B", "E"), 7);
            yield return (("C", "E"), 5);
            yield return (("E", "D"), 15);
            yield return (("F", "D"), 6);
            yield return (("E", "F"), 8);
            yield return (("F", "G"), 11);
            yield return (("E", "G"), 9);
        }

        private static IEnumerable<((string, string), int)> SmallWeightedGraph()
        {
            yield return (("A", "B"), 1);
            yield return (("B", "C"), 2);
            yield return (("A", "C"), 5);
            yield return (("C", "D"), 3);
            yield return (("B", "D"), 4);
        }

        private static void AssertSpans(IEnumerable<((string, string), int)> tree, params string[] expectedVertices)
        {
            var actual = new HashSet<string>(tree.SelectMany(e => new[] { e.Item1.Item1, e.Item1.Item2 }));
            CollectionAssert.AreEquivalent(expectedVertices, actual);
        }
    }
}
