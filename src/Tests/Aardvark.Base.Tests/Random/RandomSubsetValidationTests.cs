using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Tests
{
    [TestFixture]
    public class RandomSubsetValidationTests
    {
        private static void AssertParamName<TException>(string paramName, TestDelegate code)
            where TException : ArgumentException
        {
            var exception = Assert.Throws<TException>(code);
            Assert.AreEqual(paramName, exception.ParamName);
        }

        [Test]
        public void CreateRandomSubsetOfSizeRejectsNullInputs()
        {
            int[] array = null;
            IEnumerable<int> input = null;

            AssertParamName<ArgumentNullException>("array", () => array.CreateRandomSubsetOfSize(0));
            AssertParamName<ArgumentNullException>("input", () => input.CreateRandomSubsetOfSize(0));
        }

        [Test]
        public void SmallSubsetHelpersRejectNullRandom()
        {
            IRandomUniform rnd = null;

            AssertParamName<ArgumentNullException>("rnd", () => rnd.CreateSmallRandomSubsetIndexArray(0, 0));
            AssertParamName<ArgumentNullException>("rnd", () => rnd.CreateSmallRandomOrderedSubsetIndexArray(0, 0));
            AssertParamName<ArgumentNullException>("rnd", () => rnd.CreateSmallRandomSubsetIndexArrayLong(0, 0));
            AssertParamName<ArgumentNullException>("rnd", () => rnd.CreateSmallRandomOrderedSubsetIndexArrayLong(0, 0));
        }

        [Test]
        public void SmallSubsetHelpersRejectNegativeCount()
        {
            var rnd = new RandomSystem(0);

            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.CreateSmallRandomSubsetIndexArray(0, -1));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.CreateSmallRandomOrderedSubsetIndexArray(0, -1));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.CreateSmallRandomSubsetIndexArrayLong(0, -1));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.CreateSmallRandomOrderedSubsetIndexArrayLong(0, -1));
        }

        [Test]
        public void SmallSubsetHelpersRejectInvalidSubsetCount()
        {
            var rnd = new RandomSystem(0);

            AssertParamName<ArgumentOutOfRangeException>("subsetCount", () => rnd.CreateSmallRandomSubsetIndexArray(-1, 0));
            AssertParamName<ArgumentOutOfRangeException>("subsetCount", () => rnd.CreateSmallRandomOrderedSubsetIndexArray(-1, 0));
            AssertParamName<ArgumentOutOfRangeException>("subsetCount", () => rnd.CreateSmallRandomSubsetIndexArray(2, 1));
            AssertParamName<ArgumentOutOfRangeException>("subsetCount", () => rnd.CreateSmallRandomOrderedSubsetIndexArray(2, 1));

            AssertParamName<ArgumentOutOfRangeException>("subsetCount", () => rnd.CreateSmallRandomSubsetIndexArrayLong(-1, 0));
            AssertParamName<ArgumentOutOfRangeException>("subsetCount", () => rnd.CreateSmallRandomOrderedSubsetIndexArrayLong(-1, 0));
            AssertParamName<ArgumentOutOfRangeException>("subsetCount", () => rnd.CreateSmallRandomSubsetIndexArrayLong(2, 1));
            AssertParamName<ArgumentOutOfRangeException>("subsetCount", () => rnd.CreateSmallRandomOrderedSubsetIndexArrayLong(2, 1));
        }

        [Test]
        public void ZeroSubsetInputsAreValid()
        {
            var rnd = new RandomSystem(0);
            var array = new[] { 1, 2, 3 };
            var input = array.AsEnumerable();

            CollectionAssert.IsEmpty(array.CreateRandomSubsetOfSize(0));
            CollectionAssert.IsEmpty(input.CreateRandomSubsetOfSize(0));
            CollectionAssert.IsEmpty(array.CreateRandomSubsetOfSize(0, null));
            CollectionAssert.IsEmpty(input.CreateRandomSubsetOfSize(0, null));

            CollectionAssert.IsEmpty(rnd.CreateSmallRandomSubsetIndexArray(0, 0));
            CollectionAssert.IsEmpty(rnd.CreateSmallRandomOrderedSubsetIndexArray(0, 0));
            CollectionAssert.IsEmpty(rnd.CreateSmallRandomSubsetIndexArrayLong(0, 0));
            CollectionAssert.IsEmpty(rnd.CreateSmallRandomOrderedSubsetIndexArrayLong(0, 0));

            CollectionAssert.IsEmpty(rnd.CreateSmallRandomSubsetIndexArray(0, 3));
            CollectionAssert.IsEmpty(rnd.CreateSmallRandomOrderedSubsetIndexArray(0, 3));
            CollectionAssert.IsEmpty(rnd.CreateSmallRandomSubsetIndexArrayLong(0, 3));
            CollectionAssert.IsEmpty(rnd.CreateSmallRandomOrderedSubsetIndexArrayLong(0, 3));
        }

        [Test]
        public void OrderedSubsetHelpersReturnSortedIndices()
        {
            var rnd = new RandomSystem(0);

            var subset = rnd.CreateSmallRandomOrderedSubsetIndexArray(5, 20);
            var longSubset = rnd.CreateSmallRandomOrderedSubsetIndexArrayLong(5, 20);

            Assert.AreEqual(5, subset.Length);
            Assert.AreEqual(5, longSubset.Length);
            CollectionAssert.AreEqual(subset.OrderBy(x => x), subset);
            CollectionAssert.AreEqual(longSubset.OrderBy(x => x), longSubset);
            Assert.IsTrue(subset.All(x => x >= 0 && x < 20));
            Assert.IsTrue(longSubset.All(x => x >= 0 && x < 20));
            Assert.AreEqual(subset.Length, subset.Distinct().Count());
            Assert.AreEqual(longSubset.Length, longSubset.Distinct().Count());
        }
    }
}
