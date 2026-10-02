using NUnit.Framework;
using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Tests.Extensions
{
    static class Lists
    {
        private static void AssertParamName<TException>(string paramName, TestDelegate code)
            where TException : ArgumentException
        {
            var ex = Assert.Throws<TException>(code);
            Assert.AreEqual(paramName, ex.ParamName);
        }

        [Test]
        public static void FindIndexCircularOrderAndFirstMatchesAgreeWithWideOracle()
        {
            IList<int> minimal = new[] { 0, 1, 2, 3, 4 };
            foreach (var forward in new[] { true, false })
            foreach (var tail in new[] { false, true })
            {
                var context = $"minimal: forward={forward}, tail={tail}";
                Assert.AreEqual(4, SearchRange(minimal, 2, 3, forward, int.MinValue, tail, _ => true), context);
                CheckCircularSearch(minimal, 2, 3, forward, int.MinValue, tail, context);
            }

            const int seed = 89017;
            var random = new Random(seed);
            for (var sample = 0; sample < 128; sample++)
            {
                var length = random.Next(1, 129);
                var values = Enumerable.Range(0, length).ToArray();
                IList<int> list = sample % 2 == 0 ? values : new List<int>(values);
                var start = random.Next(length);
                var count = random.Next(1, length - start + 1);
                foreach (var tail in new[] { false, true })
                foreach (var forward in new[] { true, false })
                {
                    var rangeCount = tail ? length - start : count;
                    foreach (var search in new[]
                    {
                        int.MinValue, int.MinValue + 1, int.MaxValue, int.MaxValue - 1,
                        -37, -1, start - 1, start, start + rangeCount - 1, start + rangeCount,
                        (int)random.NextInt64(int.MinValue, (long)int.MaxValue + 1)
                    }.Distinct())
                    {
                        var context = $"seed={seed}, sample={sample}, length={length}, start={start}, count={rangeCount}, search={search}, forward={forward}, tail={tail}";
                        CheckCircularSearch(list, start, rangeCount, forward, search, tail, context);
                    }
                }
            }
        }

        private static int SearchRange(IList<int> list, int start, int count, bool forward, int search, bool tail, Predicate<int> match)
            => tail
                ? IListExtensions.FindIndex(list, start, forward, search, match)
                : IListExtensions.FindIndex(list, start, count, forward, search, match);

        private static int[] CircularOrder(int start, int count, int search, bool forward)
        {
            // Rank each candidate by its nonnegative circular distance from the requested
            // start, rather than normalizing a start index and reproducing the search loops.
            return Enumerable.Range(start, count).OrderBy(index =>
            {
                var distance = forward ? (long)index - search : (long)search - index;
                return (distance % count + count) % count;
            }).ToArray();
        }

        private static void CheckCircularSearch(IList<int> list, int start, int count, bool forward, int search, bool tail, string context)
        {
            var order = CircularOrder(start, count, search, forward);
            foreach (var mode in new[] { 0, 1, 2, 3 })
            {
                // No match, immediate match, multiple matches, and a last-visited match.
                Predicate<int> accept = mode switch
                {
                    0 => _ => false,
                    1 => _ => true,
                    2 => value => value % 3 == 0,
                    _ => value => value == order[order.Length - 1]
                };
                var expectedOffset = Array.FindIndex(order, accept);
                var expectedIndex = expectedOffset < 0 ? -1 : order[expectedOffset];
                var expectedCalls = expectedOffset < 0 ? order.Length : expectedOffset + 1;
                var visited = new List<int>();
                var actual = SearchRange(list, start, count, forward, search, tail, value =>
                {
                    visited.Add(value);
                    return accept(value);
                });
                Assert.AreEqual(expectedIndex, actual, context + $", predicate={mode}");
                CollectionAssert.AreEqual(order.Take(expectedCalls), visited, context + $", predicate={mode}: visitation");
            }
        }

        [Test]
        public static void FindIndexEmptyAndSingletonRangesPreservePredicateCalls()
        {
            foreach (var length in new[] { 0, 1, 5 })
            foreach (var forward in new[] { true, false })
            foreach (var search in new[] { int.MinValue, -1, 0, 17, int.MaxValue })
            {
                IList<int> list = Enumerable.Range(0, length).ToArray();
                for (var start = 0; start <= length; start++)
                {
                    var context = $"length={length}, start={start}, search={search}, forward={forward}";
                    Predicate<int> unused = _ => { Assert.Fail(context + ": empty range invoked predicate"); return true; };
                    Assert.AreEqual(-1, IListExtensions.FindIndex(list, start, 0, forward, search, unused), context);
                    if (start == length)
                        Assert.AreEqual(-1, IListExtensions.FindIndex(list, start, forward, search, unused), context);
                    else
                    {
                        CheckCircularSearch(list, start, 1, forward, search, false, context + ": singleton");
                        if (start + 1 == length)
                            CheckCircularSearch(list, start, 1, forward, search, true, context + ": singleton tail");
                    }
                }
            }
        }

        [Test]
        public static void FindIndexPropagatesPredicateExceptionsWithoutFurtherCalls()
        {
            IList<int> list = new[] { 0, 1, 2, 3, 4, 5, 6 };
            foreach (var tail in new[] { false, true })
            foreach (var forward in new[] { true, false })
            foreach (var search in new[] { int.MinValue, -37, 3, int.MaxValue })
            {
                var count = tail ? 5 : 3;
                var order = CircularOrder(2, count, search, forward);
                foreach (var throwAfter in new[] { 1, count })
                {
                    var context = $"tail={tail}, forward={forward}, search={search}, throwAfter={throwAfter}";
                    var failure = new InvalidOperationException(context);
                    var visited = new List<int>();
                    var error = Assert.Throws<InvalidOperationException>(() => SearchRange(list, 2, count, forward, search, tail, value =>
                    {
                        visited.Add(value);
                        if (visited.Count == throwAfter) throw failure;
                        return false;
                    }));
                    Assert.AreSame(failure, error, context);
                    CollectionAssert.AreEqual(order.Take(throwAfter), visited, context);
                }
            }
        }

        [Test]
        public static void FindIndexRetainsValidationOrderBeforeEmptyRangeReturn()
        {
            IList<int> list = new[] { 0, 1, 2 };
            foreach (var forward in new[] { true, false })
            foreach (var search in new[] { int.MinValue, int.MaxValue })
            {
                AssertParamName<ArgumentNullException>("list", () => IListExtensions.FindIndex<int>(null, -1, -1, forward, search, null));
                AssertParamName<ArgumentNullException>("list", () => IListExtensions.FindIndex<int>(null, -1, forward, search, null));
                foreach (var start in new[] { -1, 4 })
                {
                    AssertParamName<ArgumentOutOfRangeException>("startIndex", () => IListExtensions.FindIndex(list, start, -1, forward, search, null));
                    AssertParamName<ArgumentOutOfRangeException>("startIndex", () => IListExtensions.FindIndex(list, start, forward, search, null));
                }
                foreach (var count in new[] { -1, 4, int.MaxValue })
                    AssertParamName<ArgumentOutOfRangeException>("count", () => IListExtensions.FindIndex(list, 0, count, forward, search, null));
                AssertParamName<ArgumentNullException>("match", () => IListExtensions.FindIndex(list, 3, 0, forward, search, null));
                AssertParamName<ArgumentNullException>("match", () => IListExtensions.FindIndex(list, 3, forward, search, null));
                AssertParamName<ArgumentNullException>("match", () => IListExtensions.FindIndex(list, 0, 1, forward, search, null));
            }
        }

        [Test]
        public static void ListFunMap3TruncatesToThirdList()
        {
            var list0 = new List<int> { 10, 20, 30, 40 };
            var list1 = new List<int> { 1, 2, 3, 4 };
            var list2 = new List<int> { 100, 200 };

            var result = list0.Map3(list1, list2, (a, b, c) => a + b + c);

            CollectionAssert.AreEqual(new[] { 111, 222 }, result);
        }

        [Test]
        public static void ListFunMap3IndexedTruncatesToThirdListAndPreservesIndex()
        {
            var list0 = new List<string> { "a", "b", "c" };
            var list1 = new List<string> { "x", "y", "z" };
            var list2 = new List<string> { "p", "q" };

            var result = list0.Map3(list1, list2, (a, b, c, i) => $"{i}:{a}{b}{c}");

            CollectionAssert.AreEqual(new[] { "0:axp", "1:byq" }, result);
        }

        [Test]
        public static void ListFunCopyToArrayCountRejectsInvalidArguments()
        {
            List<int> self = null;
            AssertParamName<ArgumentNullException>("self", () => self.CopyToArray(0));

            self = new List<int> { 1, 2, 3 };
            AssertParamName<ArgumentOutOfRangeException>("count", () => self.CopyToArray(-1));
            AssertParamName<ArgumentOutOfRangeException>("count", () => self.CopyToArray(self.Count + 1));
        }

        [Test]
        public static void ListFunMapToArrayCountRejectsInvalidArguments()
        {
            List<int> list = null;
            Func<int, int> map = x => x * 2;
            AssertParamName<ArgumentNullException>("list", () => list.MapToArray(0, map));

            list = new List<int> { 1, 2, 3 };
            map = null;
            AssertParamName<ArgumentNullException>("item_fun", () => list.MapToArray(0, map));
            AssertParamName<ArgumentOutOfRangeException>("count", () => list.MapToArray(-1, x => x));
        }

        [Test]
        public static void ListFunMapToArrayCountPreservesPaddedResult()
        {
            var list = new List<int> { 1, 2 };

            var result = list.MapToArray(4, x => x * 10);

            CollectionAssert.AreEqual(new[] { 10, 20, 0, 0 }, result);
        }

        [Test]
        public static void ListFunMapToArrayStartCountRejectsInvalidArguments()
        {
            List<int> list = null;
            Func<int, int> map = x => x * 2;
            AssertParamName<ArgumentNullException>("list", () => list.MapToArray(0, 0, map));

            list = new List<int> { 1, 2, 3 };
            map = null;
            AssertParamName<ArgumentNullException>("item_fun", () => list.MapToArray(0, 0, map));
            AssertParamName<ArgumentOutOfRangeException>("start", () => list.MapToArray(-1, 0, x => x));
            AssertParamName<ArgumentOutOfRangeException>("start", () => list.MapToArray(list.Count + 1, 0, x => x));
            AssertParamName<ArgumentOutOfRangeException>("count", () => list.MapToArray(0, -1, x => x));
        }

        [Test]
        public static void ListFunMapToArrayStartCountPreservesClampedPaddedResult()
        {
            var list = new List<int> { 1, 2, 3 };

            var result = list.MapToArray(1, 4, x => x * 10);
            var emptyTail = list.MapToArray(list.Count, 2, x => x * 10);

            CollectionAssert.AreEqual(new[] { 20, 30, 0, 0 }, result);
            CollectionAssert.AreEqual(new[] { 0, 0 }, emptyTail);
        }

        [Test]
        public static void SubRangeRejectsInvalidConstructorArguments()
        {
            IList<int> nullSource = null;
            var nullSourceException = Assert.Throws<ArgumentNullException>(() => new SubRange<int>(nullSource, 0, 0));
            Assert.AreEqual("of", nullSourceException.ParamName);

            var source = new List<int> { 1, 2, 3 };

            var negativeIndex = Assert.Throws<ArgumentOutOfRangeException>(() => new SubRange<int>(source, -1, 1));
            Assert.AreEqual("index", negativeIndex.ParamName);

            var tooLargeIndex = Assert.Throws<ArgumentOutOfRangeException>(() => new SubRange<int>(source, source.Count + 1, 0));
            Assert.AreEqual("index", tooLargeIndex.ParamName);

            var negativeCount = Assert.Throws<ArgumentOutOfRangeException>(() => new SubRange<int>(source, 0, -1));
            Assert.AreEqual("count", negativeCount.ParamName);

            var tooLargeCount = Assert.Throws<ArgumentOutOfRangeException>(() => new SubRange<int>(source, 2, 2));
            Assert.AreEqual("count", tooLargeCount.ParamName);

            var overflowSizedCount = Assert.Throws<ArgumentOutOfRangeException>(() => new SubRange<int>(source, 1, int.MaxValue));
            Assert.AreEqual("count", overflowSizedCount.ParamName);
        }

        [Test]
        public static void SubRangeAllowsEmptyRangeAtEnd()
        {
            var source = new List<int> { 1, 2, 3 };
            var range = new SubRange<int>(source, source.Count, 0);

            Assert.AreEqual(0, range.Count);
            CollectionAssert.IsEmpty(range);
            Assert.AreEqual(-1, range.IndexOf(1));
        }

        [Test]
        public static void SubRangeIndexerRejectsIndexAtCount()
        {
            var source = new List<int> { 1, 2, 3 };
            var range = new SubRange<int>(source, 1, 2);

            Assert.Throws<IndexOutOfRangeException>(() => { var _ = range[range.Count]; });
            Assert.Throws<IndexOutOfRangeException>(() => range[range.Count] = 10);
        }

        [Test]
        public static void SubRangeIndexOfAndContainsHandleNullValues()
        {
            var source = new List<string> { "before", null, "hit", null, "after" };
            var range = new SubRange<string>(source, 1, 3);

            Assert.AreEqual(0, range.IndexOf(null));
            Assert.IsTrue(range.Contains(null));
            Assert.AreEqual(1, range.IndexOf("hit"));
            Assert.IsTrue(range.Contains("hit"));
            Assert.AreEqual(-1, range.IndexOf("before"));
            Assert.IsFalse(range.Contains("missing"));
        }

        [Test]
        public static void SubRangeCopyToCopiesValidRangeWithOffset()
        {
            var source = new List<int> { 0, 1, 2, 3, 4 };
            var range = new SubRange<int>(source, 1, 3);
            var target = new[] { -1, -1, -1, -1, -1 };

            range.CopyTo(target, 1);

            CollectionAssert.AreEqual(new[] { -1, 1, 2, 3, -1 }, target);
        }

        [Test]
        public static void SubRangeCopyToRejectsNullDestination()
        {
            var source = new List<int> { 1, 2, 3 };
            var range = new SubRange<int>(source, 0, 2);

            AssertParamName<ArgumentNullException>("array", () => range.CopyTo(null, 0));
        }

        [Test]
        public static void SubRangeCopyToRejectsNegativeArrayIndex()
        {
            var source = new List<int> { 1, 2, 3 };
            var range = new SubRange<int>(source, 0, 2);

            AssertParamName<ArgumentOutOfRangeException>("arrayIndex", () => range.CopyTo(new int[3], -1));
        }

        [Test]
        public static void SubRangeCopyToRejectsArrayIndexPastDestinationLength()
        {
            var source = new List<int> { 1, 2, 3 };
            var range = new SubRange<int>(source, 0, 2);

            AssertParamName<ArgumentOutOfRangeException>("arrayIndex", () => range.CopyTo(new int[3], 4));
        }

        [Test]
        public static void SubRangeCopyToRejectsInsufficientDestinationCapacity()
        {
            var source = new List<int> { 1, 2, 3 };
            var range = new SubRange<int>(source, 0, 3);
            var target = new[] { -1, -1, -1, -1 };

            AssertParamName<ArgumentException>("array", () => range.CopyTo(target, 2));
        }

        [Test]
        public static void SubRangeCopyToDoesNotPartiallyMutateDestinationOnFailure()
        {
            var source = new List<int> { 1, 2, 3 };
            var range = new SubRange<int>(source, 0, 3);
            var target = new[] { -1, -1, -1, -1 };

            Assert.Throws<ArgumentException>(() => range.CopyTo(target, 2));

            CollectionAssert.AreEqual(new[] { -1, -1, -1, -1 }, target);
        }

        [Test]
        public static void FirstIndexOfRejectsNullSelf()
        {
            IList<int> self = null;
            var other = new List<int> { 1 };

            var ex = Assert.Throws<ArgumentNullException>(() => self.FirstIndexOf(other, 0));
            Assert.AreEqual("self", ex.ParamName);
        }

        [Test]
        public static void FirstIndexOfRejectsNullOther()
        {
            var self = new List<int> { 1, 2, 3 };
            IList<int> other = null;

            var ex = Assert.Throws<ArgumentNullException>(() => self.FirstIndexOf(other, 0));
            Assert.AreEqual("other", ex.ParamName);
        }

        [Test]
        public static void FirstIndexOfRejectsInvalidStartIndex()
        {
            var self = new List<int> { 1, 2, 3 };
            var other = new List<int> { 2 };

            var negative = Assert.Throws<ArgumentOutOfRangeException>(() => self.FirstIndexOf(other, -1));
            Assert.AreEqual("startIndex", negative.ParamName);

            var tooLarge = Assert.Throws<ArgumentOutOfRangeException>(() => self.FirstIndexOf(other, self.Count + 1));
            Assert.AreEqual("startIndex", tooLarge.ParamName);
        }

        [Test]
        public static void FirstIndexOfReturnsStartIndexForEmptyOtherAtEnd()
        {
            var self = new List<int> { 1, 2, 3 };
            var other = new List<int>();

            Assert.AreEqual(self.Count, self.FirstIndexOf(other, self.Count));
        }

        [Test]
        public static void FirstIndexOfReturnsMinusOneWhenOtherCannotFitRemainingRange()
        {
            var self = new List<int> { 1, 2, 3, 4 };
            var other = new List<int> { 3, 4, 5 };

            Assert.AreEqual(-1, self.FirstIndexOf(other, 2));
        }

        [Test]
        public static void FirstIndexOfFindsMatchFromNonZeroStartIndex()
        {
            var self = new List<int> { 1, 2, 1, 2, 3 };
            var other = new List<int> { 1, 2 };

            Assert.AreEqual(2, self.FirstIndexOf(other, 1));
        }

        [Test]
        public static void IListRankIndexHelpersRejectNullInputs()
        {
            IList<int> self = null;

            AssertParamName<ArgumentNullException>("self", () => self.SmallestIndex());
            AssertParamName<ArgumentNullException>("self", () => self.LargestIndex());
            AssertParamName<ArgumentNullException>("self", () => self.NSmallestIndex(0));
            AssertParamName<ArgumentNullException>("self", () => self.NLargestIndex(0));
        }

        [Test]
        public static void IListRankIndexHelpersRejectEmptyInputs()
        {
            IList<int> self = new List<int>();

            AssertParamName<ArgumentOutOfRangeException>("self", () => self.SmallestIndex());
            AssertParamName<ArgumentOutOfRangeException>("self", () => self.LargestIndex());
            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NSmallestIndex(0));
            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NLargestIndex(0));
        }

        [Test]
        public static void IListRankIndexHelpersRejectInvalidRank()
        {
            IList<int> self = new List<int> { 1, 2 };

            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NSmallestIndex(-1));
            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NLargestIndex(-1));
            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NSmallestIndex(self.Count));
            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NLargestIndex(self.Count));
        }

        [Test]
        public static void IListRankIndexHelpersPreserveDuplicateValueBehavior()
        {
            IList<int> self = new List<int> { 5, 1, 3, 1, 5 };

            Assert.AreEqual(1, self.SmallestIndex());
            Assert.AreEqual(0, self.LargestIndex());
            Assert.AreEqual(1, self.NSmallestIndex(1));
            Assert.AreEqual(0, self.NLargestIndex(1));
        }

        [Test]
        public static void ListRankIndexHelpersRejectNullInputs()
        {
            List<int> self = null;

            AssertParamName<ArgumentNullException>("a", () => self.NSmallestIndex(0));
            AssertParamName<ArgumentNullException>("a", () => self.NLargestIndex(0));
        }

        [Test]
        public static void ListRankIndexHelpersRejectEmptyInputs()
        {
            var self = new List<int>();

            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NSmallestIndex(0));
            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NLargestIndex(0));
        }

        [Test]
        public static void ListRankIndexHelpersRejectInvalidRank()
        {
            var self = new List<int> { 1, 2 };

            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NSmallestIndex(-1));
            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NLargestIndex(-1));
            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NSmallestIndex(self.Count));
            AssertParamName<ArgumentOutOfRangeException>("n", () => self.NLargestIndex(self.Count));
        }

        [Test]
        public static void ListRankIndexHelpersPreserveDuplicateValueBehavior()
        {
            var self = new List<int> { 5, 1, 3, 1, 5 };

            Assert.AreEqual(1, self.NSmallestIndex(0));
            Assert.AreEqual(0, self.NLargestIndex(0));
        }
    }
}
