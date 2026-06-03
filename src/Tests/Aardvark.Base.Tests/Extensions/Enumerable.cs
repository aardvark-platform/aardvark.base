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

        private sealed class DisposeTrackingEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> m_values;

            public DisposeTrackingEnumerable(IEnumerable<T> values)
            {
                m_values = values;
            }

            public int EnumeratorCount { get; private set; }
            public int DisposeCount { get; private set; }

            public IEnumerator<T> GetEnumerator()
            {
                EnumeratorCount++;
                return new Enumerator(this, m_values.GetEnumerator());
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

            private sealed class Enumerator : IEnumerator<T>
            {
                private readonly DisposeTrackingEnumerable<T> m_owner;
                private readonly IEnumerator<T> m_inner;
                private bool m_disposed;

                public Enumerator(DisposeTrackingEnumerable<T> owner, IEnumerator<T> inner)
                {
                    m_owner = owner;
                    m_inner = inner;
                }

                public T Current => m_inner.Current;

                object System.Collections.IEnumerator.Current => Current;

                public bool MoveNext() => m_inner.MoveNext();

                public void Reset() => m_inner.Reset();

                public void Dispose()
                {
                    if (!m_disposed)
                    {
                        m_disposed = true;
                        m_owner.DisposeCount++;
                        m_inner.Dispose();
                    }
                }
            }
        }

        private static DisposeTrackingEnumerable<int> Track(params int[] values)
            => new DisposeTrackingEnumerable<int>(values);

        private static void AssertDisposedOnce(params DisposeTrackingEnumerable<int>[] values)
        {
            foreach (var value in values)
            {
                Assert.AreEqual(1, value.EnumeratorCount);
                Assert.AreEqual(1, value.DisposeCount);
            }
        }

        private static void AssertParamName<TException>(string paramName, TestDelegate code)
            where TException : ArgumentException
        {
            var ex = Assert.Throws<TException>(code);
            Assert.AreEqual(paramName, ex.ParamName);
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
        public static void MaxElementDisposesEnumerator()
        {
            var source = Track(1, 4, 2);

            var result = source.MaxElement(x => x * 2, (a, b) => a > b, 0, -1, out var maxValue);

            Assert.AreEqual(4, result);
            Assert.AreEqual(8, maxValue);
            AssertDisposedOnce(source);
        }

        [Test]
        public static void MinElementDisposesEnumerator()
        {
            var source = Track(5, 2, 8);

            var result = source.MinElement(x => x, (a, b) => a < b, int.MaxValue, -1, out var minValue);

            Assert.AreEqual(2, result);
            Assert.AreEqual(2, minValue);
            AssertDisposedOnce(source);
        }

        [Test]
        public static void MinIndexDisposesEnumerator()
        {
            var source = Track(5, 2, 8);

            var index = source.MinIndex((a, b) => a < b, out var minValue);

            Assert.AreEqual(1, index);
            Assert.AreEqual(2, minValue);
            AssertDisposedOnce(source);
        }

        [Test]
        public static void MaxIndexDisposesEnumerator()
        {
            var source = Track(5, 2, 8);

            var index = source.MaxIndex((a, b) => a > b, out var maxValue);

            Assert.AreEqual(2, index);
            Assert.AreEqual(8, maxValue);
            AssertDisposedOnce(source);
        }

        [Test]
        public static void MinMaxIndexDisposeEmptyEnumerators()
        {
            var minSource = Track();
            var maxSource = Track();

            Assert.AreEqual(-1, minSource.MinIndex((a, b) => a < b, out var minValue));
            Assert.AreEqual(default(int), minValue);
            AssertDisposedOnce(minSource);

            Assert.AreEqual(-1, maxSource.MaxIndex((a, b) => a > b, out var maxValue));
            Assert.AreEqual(default(int), maxValue);
            AssertDisposedOnce(maxSource);
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

        [Test]
        public static void SetEqualsReturnsTrueForTwoNullInputs()
        {
            IEnumerable<int> left = null;
            IEnumerable<int> right = null;

            Assert.IsTrue(left.SetEquals(right));
        }

        [Test]
        public static void SetEqualsReturnsFalseForOneNullInput()
        {
            IEnumerable<int> nullSet = null;
            var set = new[] { 1 };

            Assert.IsFalse(nullSet.SetEquals(set));
            Assert.IsFalse(set.SetEquals(nullSet));
        }

        [Test]
        public static void SetEqualsReturnsTrueForEqualSetsWithDifferentOrder()
        {
            Assert.IsTrue(new[] { 1, 2, 3 }.SetEquals(new[] { 3, 1, 2 }));
        }

        [Test]
        public static void SetEqualsReturnsFalseForUnequalSets()
        {
            Assert.IsFalse(new[] { 1, 2, 3 }.SetEquals(new[] { 1, 2, 4 }));
            Assert.IsFalse(new[] { 1, 2 }.SetEquals(new[] { 1, 2, 3 }));
        }

        [Test]
        public static void SetEqualsThrowsForDuplicatesInEitherInput()
        {
            var leftDuplicate = Assert.Throws<Exception>(() => new[] { 1, 1 }.SetEquals(new[] { 1 }));
            var rightDuplicate = Assert.Throws<Exception>(() => new[] { 1 }.SetEquals(new[] { 1, 1 }));

            Assert.AreEqual("not a proper set", leftDuplicate.Message);
            Assert.AreEqual("not a proper set", rightDuplicate.Message);
        }

        [Test]
        public static void SetEqualsEnumeratesEachInputOnlyOnce()
        {
            var left = new SingleUseEnumerable<int>(new[] { 1, 2, 3 });
            var right = new SingleUseEnumerable<int>(new[] { 3, 1, 2 });

            Assert.IsTrue(left.SetEquals(right));

            Assert.AreEqual(1, left.EnumeratorCount);
            Assert.AreEqual(1, right.EnumeratorCount);
        }

        [Test]
        public static void PairSequenceReturnsNonOverlappingPairs()
        {
            var result = new[] { 1, 2, 3, 4 }.PairSequence().ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(2, result[0].Item2);
            Assert.AreEqual(3, result[1].Item1);
            Assert.AreEqual(4, result[1].Item2);
        }

        [Test]
        public static void PairSequenceDropsIncompleteTrailingItem()
        {
            var result = new[] { 1, 2, 3 }.PairSequence().ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(2, result[0].Item2);
            CollectionAssert.IsEmpty(new[] { 1 }.PairSequence());
        }

        [Test]
        public static void PairSequenceEnumeratesSourceOnlyOnce()
        {
            var source = new SingleUseEnumerable<int>(new[] { 1, 2, 3, 4 });

            var result = source.PairSequence().ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(2, result[0].Item2);
            Assert.AreEqual(3, result[1].Item1);
            Assert.AreEqual(4, result[1].Item2);
            Assert.AreEqual(1, source.EnumeratorCount);
        }

        [Test]
        public static void PairSequenceDisposesEnumeratorAfterEarlyTermination()
        {
            var source = Track(1, 2, 3, 4);

            var result = source.PairSequence().Take(1).ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(2, result[0].Item2);
            AssertDisposedOnce(source);
        }

        [Test]
        public static void TripleSequenceReturnsNonOverlappingTriples()
        {
            var result = new[] { 1, 2, 3, 4, 5, 6 }.TripleSequence().ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(2, result[0].Item2);
            Assert.AreEqual(3, result[0].Item3);
            Assert.AreEqual(4, result[1].Item1);
            Assert.AreEqual(5, result[1].Item2);
            Assert.AreEqual(6, result[1].Item3);
        }

        [Test]
        public static void TripleSequenceDropsIncompleteTrailingItems()
        {
            var result = new[] { 1, 2, 3, 4, 5 }.TripleSequence().ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(2, result[0].Item2);
            Assert.AreEqual(3, result[0].Item3);
            CollectionAssert.IsEmpty(new[] { 1, 2 }.TripleSequence());
        }

        [Test]
        public static void TripleSequenceEnumeratesSourceOnlyOnce()
        {
            var source = new SingleUseEnumerable<int>(new[] { 1, 2, 3, 4, 5, 6 });

            var result = source.TripleSequence().ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(2, result[0].Item2);
            Assert.AreEqual(3, result[0].Item3);
            Assert.AreEqual(4, result[1].Item1);
            Assert.AreEqual(5, result[1].Item2);
            Assert.AreEqual(6, result[1].Item3);
            Assert.AreEqual(1, source.EnumeratorCount);
        }

        [Test]
        public static void TripleSequenceDisposesEnumeratorAfterEarlyTermination()
        {
            var source = Track(1, 2, 3, 4, 5, 6);

            var result = source.TripleSequence().Take(1).ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(2, result[0].Item2);
            Assert.AreEqual(3, result[0].Item3);
            AssertDisposedOnce(source);
        }

        [Test]
        public static void TakeToArrayDefaultDisposesEnumerator()
        {
            var source = Track(1, 2);

            var result = source.TakeToArrayDefault(4);

            CollectionAssert.AreEqual(new[] { 1, 2, 0, 0 }, result);
            AssertDisposedOnce(source);
        }

        [Test]
        public static void TakeHelpersThrowForNullSources()
        {
            IEnumerable<int> source = null;

            AssertParamName<ArgumentNullException>("self", () => source.TakeToArrayDefault(1));
            AssertParamName<ArgumentNullException>("self", () => source.TakeToArray(1));
            AssertParamName<ArgumentNullException>("self", () => source.TakeToList(1));
        }

        [Test]
        public static void TakeHelpersThrowForNegativeCounts()
        {
            var source = new[] { 1 };

            AssertParamName<ArgumentOutOfRangeException>("count", () => source.TakeToArrayDefault(-1));
            AssertParamName<ArgumentOutOfRangeException>("count", () => source.TakeToArray(-1));
            AssertParamName<ArgumentOutOfRangeException>("count", () => source.TakeToList(-1));
        }

        [Test]
        public static void TakeToArrayDisposesEnumerator()
        {
            var source = Track(1, 2, 3);

            var result = source.TakeToArray(2);

            CollectionAssert.AreEqual(new[] { 1, 2 }, result);
            AssertDisposedOnce(source);
        }

        [Test]
        public static void TakeToArrayDisposesEnumeratorWhenSourceIsShorterThanCount()
        {
            var source = Track(1, 2);

            Assert.Throws<ArgumentException>(() => source.TakeToArray(3));

            AssertDisposedOnce(source);
        }

        [Test]
        public static void TakeToListDisposesEnumerator()
        {
            var source = Track(1, 2);

            var result = source.TakeToList(4);

            CollectionAssert.AreEqual(new[] { 1, 2 }, result);
            AssertDisposedOnce(source);
        }

        [Test]
        public static void FirstIndexOfDisposesEnumeratorAfterEarlyMatch()
        {
            var source = Track(1, 2, 3);

            Assert.AreEqual(1, source.FirstIndexOf(x => x == 2));

            AssertDisposedOnce(source);
        }

        [Test]
        public static void FirstIndexOfThrowsForNullInputs()
        {
            IEnumerable<int> source = null;
            IEnumerable<int> values = new[] { 1 };

            AssertParamName<ArgumentNullException>("self", () => source.FirstIndexOf(x => x == 1));
            AssertParamName<ArgumentNullException>("where", () => values.FirstIndexOf((Func<int, bool>)null));
        }

        [Test]
        public static void FirstIndexOfDisposesEnumeratorAfterNoMatch()
        {
            var source = Track(1, 2, 3);

            Assert.AreEqual(-1, source.FirstIndexOf(x => x == 4));

            AssertDisposedOnce(source);
        }

        [Test]
        public static void IndexOfFindsNullValue()
        {
            var source = new[] { "first", null, "third" };

            Assert.AreEqual(1, source.IndexOf(null));
        }

        [Test]
        public static void IndexOfSkipsNullsWhenFindingNonNullValue()
        {
            var source = new[] { null, "target", null };

            Assert.AreEqual(1, source.IndexOf("target"));
        }

        [Test]
        public static void IndexOfReturnsMinusOneForMissingValueWithNullElements()
        {
            var source = new[] { "first", null, "third" };

            Assert.AreEqual(-1, source.IndexOf("missing"));
        }

        [Test]
        public static void IndexOfThrowsForNullSource()
        {
            IEnumerable<string> source = null;

            AssertParamName<ArgumentNullException>("list", () => source.IndexOf("value"));
        }

        [Test]
        public static void ZipDisposesEnumeratorsAfterFullEnumeration()
        {
            var first = Track(1, 2);
            var second = Track(10, 20, 30);

            CollectionAssert.AreEqual(new[] { 1, 10, 2, 20 }, EnumerableEx.Zip(first, second).ToArray());

            AssertDisposedOnce(first, second);
        }

        [Test]
        public static void ZipThreeInputsDisposesEnumeratorsAfterEarlyTermination()
        {
            var first = Track(1, 2);
            var second = Track(10, 20);
            var third = Track(100, 200);

            CollectionAssert.AreEqual(new[] { 1, 10 }, EnumerableEx.Zip(first, second, third).Take(2).ToArray());

            AssertDisposedOnce(first, second, third);
        }

        [Test]
        public static void ZipAllDisposesEnumeratorsAfterEarlyTermination()
        {
            var first = Track(1, 2, 3);
            var second = Track(10, 20, 30);

            CollectionAssert.AreEqual(new[] { 1 }, first.ZipAll(second).Take(1).ToArray());

            AssertDisposedOnce(first, second);
        }

        [Test]
        public static void ZipAllThreeInputsDisposesEnumeratorsAfterFullEnumeration()
        {
            var first = Track(1, 2);
            var second = Track(10, 20, 30);
            var third = Track(100, 200, 300, 400);

            CollectionAssert.AreEqual(
                new[] { 1, 10, 100, 2, 20, 200, 30, 300, 400 },
                first.ZipAll(second, third).ToArray());

            AssertDisposedOnce(first, second, third);
        }

        [Test]
        public static void ZipPairsDisposesEnumeratorsAfterFullEnumeration()
        {
            var first = Track(1, 2);
            var second = Track(10, 20, 30);

            var result = first.ZipPairs(second).ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(10, result[0].Item2);
            Assert.AreEqual(2, result[1].Item1);
            Assert.AreEqual(20, result[1].Item2);
            AssertDisposedOnce(first, second);
        }

        [Test]
        public static void ZipTriplesDisposesEnumeratorsAfterEarlyTermination()
        {
            var first = Track(1, 2);
            var second = Track(10, 20);
            var third = Track(100, 200);

            var result = first.ZipTriples(second, third).Take(1).ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual(10, result[0].Item2);
            Assert.AreEqual(100, result[0].Item3);
            AssertDisposedOnce(first, second, third);
        }

        [Test]
        public static void ZipTuplesDisposesEnumeratorsAfterFullEnumeration()
        {
            var first = Track(1, 2);
            var second = new DisposeTrackingEnumerable<string>(new[] { "a", "b", "c" });

            var result = first.ZipTuples(second).ToArray();

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(1, result[0].Item1);
            Assert.AreEqual("a", result[0].Item2);
            Assert.AreEqual(2, result[1].Item1);
            Assert.AreEqual("b", result[1].Item2);
            Assert.AreEqual(1, first.EnumeratorCount);
            Assert.AreEqual(1, first.DisposeCount);
            Assert.AreEqual(1, second.EnumeratorCount);
            Assert.AreEqual(1, second.DisposeCount);
        }

        [Test]
        public static void ZipTuplesThreeInputsDisposesEnumeratorsAfterEarlyTermination()
        {
            var first = Track(1, 2);
            var second = new DisposeTrackingEnumerable<string>(new[] { "a", "b" });
            var third = Track(100, 200);

            var result = first.ZipTuples(second, third).Take(1).ToArray();

            Assert.AreEqual(1, result.Length);
            Assert.AreEqual(1, result[0].E0);
            Assert.AreEqual("a", result[0].E1);
            Assert.AreEqual(100, result[0].E2);
            Assert.AreEqual(1, first.EnumeratorCount);
            Assert.AreEqual(1, first.DisposeCount);
            Assert.AreEqual(1, second.EnumeratorCount);
            Assert.AreEqual(1, second.DisposeCount);
            Assert.AreEqual(1, third.EnumeratorCount);
            Assert.AreEqual(1, third.DisposeCount);
        }
    }
}
