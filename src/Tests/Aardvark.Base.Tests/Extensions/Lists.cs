using NUnit.Framework;
using Aardvark.Base;
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
    }
}
