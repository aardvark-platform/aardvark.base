using Aardvark.VRVis;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Tests
{
    [TestFixture]
    public class TreeTraversalTests
    {
        private sealed class TestNode : INode
        {
            public TestNode(string name, params TestNode[] children)
            {
                Name = name;
                Children = new TrackingEnumerable<TestNode>(children);
            }

            public string Name { get; }
            public TrackingEnumerable<TestNode> Children { get; }
            public IEnumerable<INode> SubNodes => Children;
        }

        private sealed class TrackingEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> m_values;
            private bool m_enumerated;

            public TrackingEnumerable(IEnumerable<T> values)
            {
                m_values = values;
            }

            public int EnumeratorCount { get; private set; }
            public int DisposeCount { get; private set; }
            public int? ThrowOnMoveNext { get; set; }

            public IEnumerator<T> GetEnumerator()
            {
                if (m_enumerated)
                    throw new InvalidOperationException("The sequence was enumerated more than once.");

                m_enumerated = true;
                EnumeratorCount++;
                return new TrackingEnumerator(this, m_values.GetEnumerator());
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

            private sealed class TrackingEnumerator : IEnumerator<T>
            {
                private readonly TrackingEnumerable<T> m_owner;
                private readonly IEnumerator<T> m_inner;
                private int m_moveNextCount;
                private bool m_disposed;

                public TrackingEnumerator(TrackingEnumerable<T> owner, IEnumerator<T> inner)
                {
                    m_owner = owner;
                    m_inner = inner;
                }

                public T Current => m_inner.Current;
                object IEnumerator.Current => Current;

                public bool MoveNext()
                {
                    m_moveNextCount++;
                    if (m_owner.ThrowOnMoveNext == m_moveNextCount)
                        throw new InvalidOperationException("MoveNext failed.");

                    return m_inner.MoveNext();
                }

                public void Reset() => m_inner.Reset();

                public void Dispose()
                {
                    if (m_disposed)
                        throw new InvalidOperationException("The enumerator was disposed more than once.");

                    m_disposed = true;
                    m_owner.DisposeCount++;
                    m_inner.Dispose();
                }
            }
        }

        [Test]
        public void ComputeDepthEnumeratesEachChildSequenceOnce()
        {
            var leaf = new TestNode("leaf");
            var level2 = new TestNode("level2", leaf);
            var level1 = new TestNode("level1", level2);
            var sibling = new TestNode("sibling");
            var root = new TestNode("root", level1, sibling);
            var nodes = new[] { root, level1, level2, leaf, sibling };

            Assert.AreEqual(3, root.ComputeDepth());

            foreach (var node in nodes)
            {
                Assert.AreEqual(1, node.Children.EnumeratorCount, node.Name);
                Assert.AreEqual(1, node.Children.DisposeCount, node.Name);
            }
        }

        [Test]
        public void DepthFirstUsesPreorderAndDisposesCompletedEnumerators()
        {
            var leftLeaf = new TestNode("left-leaf");
            var left = new TestNode("left", leftLeaf);
            var rightLeaf = new TestNode("right-leaf");
            var right = new TestNode("right", rightLeaf);
            var root = new TestNode("root", left, right);
            var nodes = new[] { root, left, leftLeaf, right, rightLeaf };

            var names = root.DepthFirst(node => node.Children).Select(node => node.Name).ToArray();

            CollectionAssert.AreEqual(
                new[] { "root", "left", "left-leaf", "right", "right-leaf" },
                names);

            foreach (var node in nodes)
            {
                Assert.AreEqual(1, node.Children.EnumeratorCount, node.Name);
                Assert.AreEqual(1, node.Children.DisposeCount, node.Name);
            }
        }

        [Test]
        public void DepthFirstDisposesActiveEnumeratorsWhenStoppedEarly()
        {
            var leaf = new TestNode("leaf");
            var child = new TestNode("child", leaf);
            var sibling = new TestNode("sibling");
            var root = new TestNode("root", child, sibling);

            using (var enumerator = root.DepthFirst(node => node.Children).GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreSame(root, enumerator.Current);
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreSame(child, enumerator.Current);
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreSame(leaf, enumerator.Current);
            }

            Assert.AreEqual(1, root.Children.DisposeCount);
            Assert.AreEqual(1, child.Children.DisposeCount);
            Assert.AreEqual(0, leaf.Children.EnumeratorCount);
            Assert.AreEqual(0, sibling.Children.EnumeratorCount);
        }

        [Test]
        public void DepthFirstDisposesActiveEnumeratorsWhenTraversalFails()
        {
            var leaf = new TestNode("leaf");
            var child = new TestNode("child", leaf);
            var sibling = new TestNode("sibling");
            var root = new TestNode("root", child, sibling);
            child.Children.ThrowOnMoveNext = 1;

            using (var enumerator = root.DepthFirst(node => node.Children).GetEnumerator())
            {
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreSame(root, enumerator.Current);
                Assert.IsTrue(enumerator.MoveNext());
                Assert.AreSame(child, enumerator.Current);

                var exception = Assert.Throws<InvalidOperationException>(
                    () => enumerator.MoveNext());
                Assert.AreEqual("MoveNext failed.", exception.Message);
            }

            Assert.AreEqual(1, root.Children.DisposeCount);
            Assert.AreEqual(1, child.Children.DisposeCount);
            Assert.AreEqual(0, leaf.Children.EnumeratorCount);
            Assert.AreEqual(0, sibling.Children.EnumeratorCount);
        }
    }
}
