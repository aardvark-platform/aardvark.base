using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Aardvark.Tests
{
    [TestFixture]
    public class ConcurrentHashSetTests
    {
        private sealed class SingleUseEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> m_values;
            private bool m_used;

            public SingleUseEnumerable(IEnumerable<T> values)
            {
                m_values = values;
            }

            public int EnumeratorCount { get; private set; }

            public IEnumerator<T> GetEnumerator()
            {
                if (m_used)
                    throw new InvalidOperationException("The sequence was enumerated more than once.");

                m_used = true;
                EnumeratorCount++;
                return m_values.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Test]
        public void SequenceConstructorDeduplicatesElements()
        {
            var set = new ConcurrentHashSet<int>(new[] { 1, 2, 1, 3, 2, 3 });

            Assert.AreEqual(3, set.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, set);
        }

        [Test]
        public void SequenceConstructorDeduplicatesUsingCustomComparer()
        {
            var set = new ConcurrentHashSet<string>(
                new[] { "alpha", "ALPHA", "beta", "BETA" },
                StringComparer.OrdinalIgnoreCase);

            Assert.AreEqual(2, set.Count);
            Assert.IsTrue(set.Contains("AlPhA"));
            Assert.IsTrue(set.Contains("BeTa"));
            Assert.IsFalse(set.Add("aLpHa"));
        }

        [Test]
        public void SequenceConstructorEnumeratesInputOnce()
        {
            var source = new SingleUseEnumerable<int>(new[] { 1, 2, 1, 3 });

            var set = new ConcurrentHashSet<int>(source);

            Assert.AreEqual(1, source.EnumeratorCount);
            Assert.AreEqual(3, set.Count);
            CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, set);
        }

        [Test]
        public void SequenceConstructorsRejectNullCollection()
        {
            IEnumerable<int> collection = null;

            var defaultComparerException = Assert.Throws<ArgumentNullException>(
                () => new ConcurrentHashSet<int>(collection));
            var customComparerException = Assert.Throws<ArgumentNullException>(
                () => new ConcurrentHashSet<int>(collection, EqualityComparer<int>.Default));

            Assert.AreEqual("collection", defaultComparerException.ParamName);
            Assert.AreEqual("collection", customComparerException.ParamName);
        }

        [Test]
        public void GenericAndNonGenericEnumerationReturnElements()
        {
            var set = new ConcurrentHashSet<int>(new[] { 1, 2, 3 });
            var genericValues = new List<int>();
            var nonGenericValues = new List<int>();

            foreach (var value in (IEnumerable<int>)set)
                genericValues.Add(value);

            foreach (var value in (IEnumerable)set)
            {
                Assert.IsInstanceOf<int>(value);
                nonGenericValues.Add((int)value);
            }

            CollectionAssert.AreEquivalent(genericValues, nonGenericValues);
            CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, nonGenericValues);
        }
    }
}
