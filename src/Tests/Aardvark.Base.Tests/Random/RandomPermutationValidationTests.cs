using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Tests
{
    [TestFixture]
    public class RandomPermutationValidationTests
    {
        private static void AssertParamName<TException>(string paramName, TestDelegate code)
            where TException : ArgumentException
        {
            var exception = Assert.Throws<TException>(code);
            Assert.AreEqual(paramName, exception.ParamName);
        }

        [Test]
        public void CreatePermutationArraysRejectInvalidArguments()
        {
            IRandomUniform rnd = null;

            AssertParamName<ArgumentNullException>("rnd", () => rnd.CreatePermutationArray(1));
            AssertParamName<ArgumentNullException>("rnd", () => rnd.CreatePermutationArrayLong(1));

            rnd = new RandomSystem(0);

            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.CreatePermutationArray(-1));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.CreatePermutationArrayLong(-1));
        }

        [Test]
        public void RandomizeArrayCountRejectsInvalidArguments()
        {
            IRandomUniform rnd = null;
            var array = new[] { 1, 2, 3 };

            AssertParamName<ArgumentNullException>("rnd", () => rnd.Randomize(array, 1L));

            rnd = new RandomSystem(0);

            AssertParamName<ArgumentNullException>("array", () => rnd.Randomize((int[])null));
            AssertParamName<ArgumentNullException>("array", () => rnd.Randomize((int[])null, 0L));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.Randomize(array, -1L));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.Randomize(array, array.LongLength + 1));
        }

        [Test]
        public void RandomizeListRejectsInvalidArguments()
        {
            IRandomUniform rnd = null;
            var list = new List<int> { 1, 2, 3 };

            AssertParamName<ArgumentNullException>("rnd", () => rnd.Randomize(list));

            rnd = new RandomSystem(0);

            AssertParamName<ArgumentNullException>("list", () => rnd.Randomize((List<int>)null));
        }

        [Test]
        public void RandomizeArrayRangeRejectsInvalidArguments()
        {
            IRandomUniform rnd = null;
            var array = new[] { 1, 2, 3 };

            AssertParamName<ArgumentNullException>("rnd", () => rnd.Randomize(array, 0, 0));
            AssertParamName<ArgumentNullException>("rnd", () => rnd.Randomize(array, 0L, 0L));

            rnd = new RandomSystem(0);

            AssertParamName<ArgumentNullException>("array", () => rnd.Randomize((int[])null, 0, 0));
            AssertParamName<ArgumentNullException>("array", () => rnd.Randomize((int[])null, 0L, 0L));
            AssertParamName<ArgumentOutOfRangeException>("start", () => rnd.Randomize(array, -1, 0));
            AssertParamName<ArgumentOutOfRangeException>("start", () => rnd.Randomize(array, -1L, 0L));
            AssertParamName<ArgumentOutOfRangeException>("start", () => rnd.Randomize(array, array.Length + 1, 0));
            AssertParamName<ArgumentOutOfRangeException>("start", () => rnd.Randomize(array, array.LongLength + 1, 0L));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.Randomize(array, 0, -1));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.Randomize(array, 0L, -1L));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.Randomize(array, 1, array.Length));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.Randomize(array, 1L, array.LongLength));
        }

        [Test]
        public void RandomizeListRangeRejectsInvalidArguments()
        {
            IRandomUniform rnd = null;
            var list = new List<int> { 1, 2, 3 };

            AssertParamName<ArgumentNullException>("rnd", () => rnd.Randomize(list, 0, 0));

            rnd = new RandomSystem(0);

            AssertParamName<ArgumentNullException>("list", () => rnd.Randomize((List<int>)null, 0, 0));
            AssertParamName<ArgumentOutOfRangeException>("start", () => rnd.Randomize(list, -1, 0));
            AssertParamName<ArgumentOutOfRangeException>("start", () => rnd.Randomize(list, list.Count + 1, 0));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.Randomize(list, 0, -1));
            AssertParamName<ArgumentOutOfRangeException>("count", () => rnd.Randomize(list, 1, list.Count));
        }

        [Test]
        public void RandomOrderRejectsNullSelf()
        {
            IEnumerable<int> self = null;

            AssertParamName<ArgumentNullException>("self", () => self.RandomOrder().ToArray());
        }

        [Test]
        public void ZeroLengthPermutationAndRandomizeRangesAreValid()
        {
            var rnd = new RandomSystem(0);
            var array = new[] { 1, 2, 3 };
            var list = new List<int> { 1, 2, 3 };

            CollectionAssert.IsEmpty(rnd.CreatePermutationArray(0));
            CollectionAssert.IsEmpty(rnd.CreatePermutationArrayLong(0));

            rnd.Randomize(array, 0L);
            rnd.Randomize(array, array.Length, 0);
            rnd.Randomize(array, array.LongLength, 0L);
            rnd.Randomize(list, list.Count, 0);

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, array);
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, list);
        }
    }
}
