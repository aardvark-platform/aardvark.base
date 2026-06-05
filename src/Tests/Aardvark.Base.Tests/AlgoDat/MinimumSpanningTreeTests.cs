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
        public void CreateRejectsNullEdges()
        {
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                _ = MinimumSpanningTree.Create<string, int>(null);
            });

            Assert.AreEqual("edges", ex.ParamName);
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
