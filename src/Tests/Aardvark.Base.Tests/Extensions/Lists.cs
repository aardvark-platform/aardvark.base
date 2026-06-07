using NUnit.Framework;
using Aardvark.Base;
using System;
using System.Collections.Generic;

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
