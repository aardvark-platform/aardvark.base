using NUnit.Framework;
using Aardvark.Base;
using System;
using System.Collections.Generic;

namespace Aardvark.Tests.Extensions
{
    static class Lists
    {
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
    }
}
