using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Extensions
{
    [TestFixture]
    public class SubRangeCopyTests
    {
        private static SubRange<T> View<T>(IList<T> source, int start, int count, int depth)
        {
            for (var level = 0; level < depth; level++)
            {
                var offset = (start + 1) / 2;
                // Trim both ends while leaving the requested interval inside every parent.
                var end = start + count + (source.Count - start - count) / 2;
                source = new SubRange<T>(source, offset, end - offset);
                start -= offset;
            }
            return new SubRange<T>(source, start, count);
        }

        private static void AssertContents<T>(T[] expected, T[] actual, string context)
        {
            CollectionAssert.AreEqual(expected, actual, context);
            if (!typeof(T).IsValueType)
                for (var i = 0; i < expected.Length; i++)
                    Assert.AreSame(expected[i], actual[i], context + $", reference at {i}");
        }

        [Test]
        public void ArrayBackedCopiesMatchAPreCopySnapshot()
        {
            Assert.Multiple(() =>
            {
                foreach (var start in new[] { 0, 3 })
                foreach (var depth in new[] { 0, 1, 3 })
                {
                    var minimal = Enumerable.Range(0, start + 5).ToArray();
                    var expected = (int[])minimal.Clone();
                    for (var i = 0; i < 4; i++) expected[start + 1 + i] = minimal[start + i];
                    View(minimal, start, 4, depth).CopyTo(minimal, start + 1);
                    CollectionAssert.AreEqual(expected, minimal, $"rightward overlap: start={start}, depth={depth}");
                }
            });

            foreach (var length in new[] { 0, 1, 2, 5, 9, 16 })
            {
                CheckArrayCopies(Enumerable.Range(0, length).ToArray());
                var shared = new object();
                CheckArrayCopies(Enumerable.Range(0, length)
                    .Select(i => i % 3 == 0 ? null : i % 3 == 1 ? shared : new object()).ToArray());
            }
        }

        private static void CheckArrayCopies<T>(T[] values)
        {
            for (var start = 0; start <= values.Length; start++)
            for (var count = 0; count <= values.Length - start; count++)
            foreach (var depth in new[] { 0, 1, 3 })
            foreach (var sameArray in new[] { true, false })
            for (var destinationIndex = 0; destinationIndex <= values.Length - count; destinationIndex++)
            {
                var context = $"type={typeof(T)}, length={values.Length}, start={start}, count={count}, depth={depth}, sameArray={sameArray}, destinationIndex={destinationIndex}";
                var source = (T[])values.Clone();
                var destination = sameArray ? source : new T[values.Length];
                var snapshot = (T[])source.Clone();
                var expected = (T[])destination.Clone();
                for (var i = 0; i < count; i++) expected[destinationIndex + i] = snapshot[start + i];

                View(source, start, count, depth).CopyTo(destination, destinationIndex);

                AssertContents(expected, destination, context);
                if (!sameArray) AssertContents(snapshot, source, context + ": source unchanged");
            }
        }

        [Test]
        public void ListAndCustomSourcesRetainIndexerBehavior()
        {
            foreach (var depth in new[] { 0, 1, 3 })
            foreach (var count in new[] { 0, 1, 4 })
            {
                var values = Enumerable.Range(0, 9).ToArray();
                var list = new List<int>(values);
                var observed = new ObservedList(values);
                var derived = new ObservedSubRange(values);
                foreach (var source in new IList<int>[] { list, observed, derived })
                {
                    var context = $"source={source.GetType().Name}, depth={depth}, count={count}";
                    var destination = Enumerable.Repeat(-1, 8).ToArray();
                    var expected = (int[])destination.Clone();
                    var adjustment = ReferenceEquals(source, list) ? 0 : 100;
                    for (var i = 0; i < count; i++) expected[2 + i] = values[3 + i] + adjustment;
                    View(source, 3, count, depth).CopyTo(destination, 2);
                    CollectionAssert.AreEqual(expected, destination, context);
                    if (source is ObservedList)
                        CollectionAssert.AreEqual(Enumerable.Range(3, count), observed.Reads, context);
                    if (source is ObservedSubRange)
                        CollectionAssert.AreEqual(Enumerable.Range(3, count), derived.Reads, context);
                    CollectionAssert.AreEqual(Enumerable.Range(0, 9), values, context + ": backing array");
                    CollectionAssert.AreEqual(values, list, context + ": source list");
                }
            }
        }

        [Test]
        public void IndexerFailuresRetainPartialCopyAndExceptionIdentity()
        {
            foreach (var depth in new[] { 0, 2 })
            foreach (var throwAt in new[] { 2, 3, 5 })
            {
                var values = Enumerable.Range(0, 8).ToArray();
                var failure = new InvalidOperationException("indexer failure");
                var observed = new ObservedList(values) { ThrowAt = throwAt, Failure = failure };
                var derived = new ObservedSubRange(values) { ThrowAt = throwAt, Failure = failure };
                foreach (var source in new IList<int>[] { observed, derived })
                {
                    var context = $"source={source.GetType().Name}, depth={depth}, throwAt={throwAt}";
                    var destination = Enumerable.Repeat(-1, 7).ToArray();
                    var expected = (int[])destination.Clone();
                    for (var i = 2; i < throwAt; i++) expected[1 + i - 2] = values[i] + 100;
                    var range = View(source, 2, 4, depth);
                    Assert.AreSame(failure, Assert.Throws<InvalidOperationException>(() => range.CopyTo(destination, 1)), context);
                    CollectionAssert.AreEqual(expected, destination, context);
                    var reads = source is ObservedList ? observed.Reads : derived.Reads;
                    CollectionAssert.AreEqual(Enumerable.Range(2, throwAt - 2 + 1), reads, context);
                }
            }
        }

        [Test]
        public void DestinationValidationPrecedesSourceReadsAndLeavesStorageUnchanged()
        {
            foreach (var count in new[] { 0, 3 })
            foreach (var depth in new[] { 0, 2 })
            {
                var values = Enumerable.Range(0, 7).ToArray();
                var observed = new ObservedList(values) { ThrowAt = 2 };
                var derived = new ObservedSubRange(values) { ThrowAt = 2 };
                foreach (var source in new IList<int>[] { values, new List<int>(values), observed, derived })
                {
                    var range = View(source, 2, count, depth);
                    foreach (var invalid in new (int Length, int Index, Type Type, string Parameter)[]
                    {
                        (-1, -1, typeof(ArgumentNullException), "array"),
                        (-1, int.MaxValue, typeof(ArgumentNullException), "array"),
                        (4, -1, typeof(ArgumentOutOfRangeException), "arrayIndex"),
                        (4, int.MinValue, typeof(ArgumentOutOfRangeException), "arrayIndex"),
                        (4, 5, typeof(ArgumentOutOfRangeException), "arrayIndex"),
                        (4, int.MaxValue, typeof(ArgumentOutOfRangeException), "arrayIndex"),
                        (0, 0, typeof(ArgumentException), "array"),
                        (4, 2, typeof(ArgumentException), "array")
                    })
                    {
                        if (count == 0 && invalid.Type == typeof(ArgumentException)) continue;
                        var context = $"source={source.GetType().Name}, count={count}, depth={depth}, length={invalid.Length}, index={invalid.Index}";
                        var destination = invalid.Length < 0 ? null : Enumerable.Repeat(-1, invalid.Length).ToArray();
                        var snapshot = destination == null ? null : (int[])destination.Clone();
                        var error = (ArgumentException)Assert.Throws(invalid.Type, () => range.CopyTo(destination, invalid.Index), context);
                        Assert.AreEqual(invalid.Parameter, error.ParamName, context);
                        if (destination != null) CollectionAssert.AreEqual(snapshot, destination, context);
                        CollectionAssert.AreEqual(Enumerable.Range(0, 7), values, context + ": source");
                        Assert.IsEmpty(observed.Reads, context);
                        Assert.IsEmpty(derived.Reads, context);
                    }
                }
            }
        }

        [Test]
        public void CovariantArraysRetainElementwiseCompatibility()
        {
            foreach (var source in new object[][]
            {
                new string[] { "first", null, "last" },
                new object[] { "first", null, "last" },
                new Uri[] { null, null, null },
                new object[] { "first", new object(), null }
            })
            foreach (var count in new[] { 0, 3 })
            foreach (var depth in new[] { 0, 2 })
            {
                var context = $"sourceType={source.GetType()}, count={count}, depth={depth}";
                object[] destination = new string[] { "before", "a", "b", "c", "after" };
                object[] expected = (string[])destination.Clone();
                Exception expectedError = null;
                try { for (var i = 0; i < count; i++) expected[1 + i] = source[i]; }
                catch (ArrayTypeMismatchException error) { expectedError = error; }
                var range = View(source, 0, count, depth);
                if (expectedError == null) range.CopyTo(destination, 1);
                else Assert.Throws(expectedError.GetType(), () => range.CopyTo(destination, 1), context);
                AssertContents(expected, destination, context);
            }
        }

        private sealed class ObservedList : IList<int>
        {
            private readonly int[] values;
            public readonly List<int> Reads = new List<int>();
            public int ThrowAt = -1;
            public Exception Failure = new InvalidOperationException("unexpected source read");
            public ObservedList(int[] values) { this.values = values; }
            public int this[int index]
            {
                get { Reads.Add(index); if (index == ThrowAt) throw Failure; return values[index] + 100; }
                set => throw new NotSupportedException();
            }
            public int Count => values.Length;
            public bool IsReadOnly => true;
            public void CopyTo(int[] array, int index) => throw new AssertionException("Source CopyTo must not be used.");
            public IEnumerator<int> GetEnumerator() => throw new AssertionException("Source enumeration must not be used.");
            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
            public void Add(int item) => throw new NotSupportedException();
            public void Clear() => throw new NotSupportedException();
            public bool Contains(int item) => throw new NotSupportedException();
            public int IndexOf(int item) => throw new NotSupportedException();
            public void Insert(int index, int item) => throw new NotSupportedException();
            public bool Remove(int item) => throw new NotSupportedException();
            public void RemoveAt(int index) => throw new NotSupportedException();
        }

        private sealed class ObservedSubRange : SubRange<int>, IList<int>
        {
            public readonly List<int> Reads = new List<int>();
            public int ThrowAt = -1;
            public Exception Failure = new InvalidOperationException("unexpected derived indexer read");
            public ObservedSubRange(int[] source) : base(source, 0, source.Length) { }
            int IList<int>.this[int index]
            {
                get { Reads.Add(index); if (index == ThrowAt) throw Failure; return base[index] + 100; }
                set => throw new NotSupportedException();
            }
        }
    }
}
