using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using Aardvark.Base;
using System.Linq;

namespace Aardvark.Tests.Extensions
{
    static class Enumerables
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

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
        }

        [Test]
        public static void MaxIndex()
        {
            var en = 0.UpTo(5);

            var maxIndex = en.MaxIndex();
            var maxValue = en.Max();
            Assert.True(en.ElementAt(maxIndex) == maxValue);

            Assert.True(0.IntoIEnumerable().MaxIndex() == 0);

            Assert.True(Enumerable.Empty<int>().MaxIndex() < 0);
        }

        [Test]
        public static void MinIndex()
        {
            var en = 0.UpTo(5);

            var minIndex = en.MinIndex();
            var minValue = en.Min();
            Assert.True(en.ElementAt(minIndex) == minValue);

            Assert.True(0.IntoIEnumerable().MinIndex() == 0);

            Assert.True(Enumerable.Empty<int>().MinIndex() < 0);
        }

        [Test]
        public static void SelectToListFromArray()
        {
            var source = new[] { 1, 2, 3 };

            var result = source.SelectToList(x => $"v{x * 2}");

            CollectionAssert.AreEqual(new[] { "v2", "v4", "v6" }, result);
        }

        [Test]
        public static void SelectToListFromList()
        {
            var source = new List<string> { "a", "bb", "ccc" };

            var result = source.SelectToList(x => x.Length);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, result);
        }

        [Test]
        public static void AllDistinctHandlesTrivialAndNormalCases()
        {
            IEnumerable<int> nullValues = null;

            Assert.IsTrue(nullValues.AllDistinct());
            Assert.IsTrue(Enumerable.Empty<int>().AllDistinct());
            Assert.IsTrue(new[] { 1 }.AllDistinct());
            Assert.IsTrue(new[] { 1, 2, 3 }.AllDistinct());
            Assert.IsFalse(new[] { 1, 2, 1 }.AllDistinct());
        }

        [Test]
        public static void AllEqualHandlesTrivialAndNormalCases()
        {
            IEnumerable<int> nullValues = null;

            Assert.IsTrue(nullValues.AllEqual());
            Assert.IsTrue(Enumerable.Empty<int>().AllEqual());
            Assert.IsTrue(new[] { 1 }.AllEqual());
            Assert.IsTrue(new[] { 1, 1, 1 }.AllEqual());
            Assert.IsFalse(new[] { 1, 1, 2 }.AllEqual());
        }

        [Test]
        public static void AllDistinctUsesOneEnumerator()
        {
            var distinct = new SingleUseEnumerable<int>(new[] { 1, 2, 3 });
            var duplicate = new SingleUseEnumerable<int>(new[] { 1, 2, 1 });

            Assert.IsTrue(distinct.AllDistinct());
            Assert.AreEqual(1, distinct.EnumeratorCount);

            Assert.IsFalse(duplicate.AllDistinct());
            Assert.AreEqual(1, duplicate.EnumeratorCount);
        }

        [Test]
        public static void AllEqualUsesOneEnumerator()
        {
            var equal = new SingleUseEnumerable<int>(new[] { 1, 1, 1 });
            var unequal = new SingleUseEnumerable<int>(new[] { 1, 1, 2 });

            Assert.IsTrue(equal.AllEqual());
            Assert.AreEqual(1, equal.EnumeratorCount);

            Assert.IsFalse(unequal.AllEqual());
            Assert.AreEqual(1, unequal.EnumeratorCount);
        }
    }
}
