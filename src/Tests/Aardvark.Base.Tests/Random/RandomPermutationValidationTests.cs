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
        private sealed class ScriptedRandom : IRandomUniform
        {
            private readonly double[] m_values;
            private int m_index;

            public ScriptedRandom(params double[] values)
            {
                m_values = values;
            }

            public int DrawCount => m_index;
            public int RemainingCount => m_values.Length - m_index;
            public int RandomBits => 53;
            public bool GeneratesFullDoubles => true;

            public void ReSeed(int seed) { }

            public int UniformInt() => throw new NotSupportedException();
            public uint UniformUInt() => throw new NotSupportedException();
            public long UniformLong() => throw new NotSupportedException();
            public ulong UniformULong() => throw new NotSupportedException();
            public float UniformFloat() => throw new NotSupportedException();
            public float UniformFloatClosed() => throw new NotSupportedException();
            public float UniformFloatOpen() => throw new NotSupportedException();

            public double UniformDouble()
            {
                if (m_index >= m_values.Length)
                    throw new InvalidOperationException("Unexpected random draw.");

                return m_values[m_index++];
            }

            public double UniformDoubleClosed() => throw new NotSupportedException();
            public double UniformDoubleOpen() => throw new NotSupportedException();
        }

        private static void AssertParamName<TException>(string paramName, TestDelegate code)
            where TException : ArgumentException
        {
            var exception = Assert.Throws<TException>(code);
            Assert.AreEqual(paramName, exception.ParamName);
        }

        private static double Choice(int value, int size)
            => (value + 0.5) / size;

        private static void AssertDrawCount(int expected, Action<IRandomUniform> action)
        {
            var random = new ScriptedRandom(Enumerable.Repeat(0.0, expected).ToArray());
            action(random);
            Assert.That(random.DrawCount, Is.EqualTo(expected));
            Assert.That(random.RemainingCount, Is.Zero);
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
        public void ThreeElementShuffleChoicePathsProduceAllPermutations()
        {
            var permutations = new HashSet<string>();

            for (int first = 0; first < 3; first++)
            {
                for (int second = 0; second < 2; second++)
                {
                    var random = new ScriptedRandom(Choice(first, 3), Choice(second, 2));
                    var values = new[] { 0, 1, 2 };

                    random.Randomize(values);

                    CollectionAssert.AreEquivalent(new[] { 0, 1, 2 }, values);
                    Assert.That(random.DrawCount, Is.EqualTo(2));
                    Assert.That(random.RemainingCount, Is.Zero);
                    permutations.Add(string.Join(",", values));
                }
            }

            Assert.That(permutations.Count, Is.EqualTo(6));
        }

        [Test]
        public void CreatePermutationArraysUseFisherYatesChoices()
        {
            var intRandom = new ScriptedRandom(
                Choice(1, 4), Choice(0, 3), Choice(1, 2));
            var longRandom = new ScriptedRandom(
                Choice(1, 4), Choice(0, 3), Choice(1, 2));

            CollectionAssert.AreEqual(
                new[] { 2, 3, 0, 1 },
                intRandom.CreatePermutationArray(4));
            CollectionAssert.AreEqual(
                new long[] { 2, 3, 0, 1 },
                longRandom.CreatePermutationArrayLong(4));
            Assert.That(intRandom.DrawCount, Is.EqualTo(3));
            Assert.That(longRandom.DrawCount, Is.EqualTo(3));
        }

        [Test]
        public void RandomizeFullArrayAndListUseFisherYatesChoices()
        {
            var arrayRandom = new ScriptedRandom(
                Choice(1, 4), Choice(0, 3), Choice(1, 2));
            var listRandom = new ScriptedRandom(
                Choice(1, 4), Choice(0, 3), Choice(1, 2));
            var array = new[] { 0, 1, 2, 3 };
            var list = new List<int> { 0, 1, 2, 3 };

            arrayRandom.Randomize(array);
            listRandom.Randomize(list);

            CollectionAssert.AreEqual(new[] { 2, 3, 0, 1 }, array);
            CollectionAssert.AreEqual(new[] { 2, 3, 0, 1 }, list);
            Assert.That(arrayRandom.DrawCount, Is.EqualTo(3));
            Assert.That(listRandom.DrawCount, Is.EqualTo(3));
        }

        [Test]
        public void RandomizeRangesStayInsideSelectedSegment()
        {
            var intRandom = new ScriptedRandom(
                Choice(1, 4), Choice(0, 3), Choice(1, 2));
            var longRandom = new ScriptedRandom(
                Choice(1, 4), Choice(0, 3), Choice(1, 2));
            var listRandom = new ScriptedRandom(
                Choice(1, 4), Choice(0, 3), Choice(1, 2));
            var intRange = new[] { 0, 1, 2, 3, 4, 5 };
            var longRange = new[] { 0, 1, 2, 3, 4, 5 };
            var listRange = new List<int> { 0, 1, 2, 3, 4, 5 };

            intRandom.Randomize(intRange, 1, 4);
            longRandom.Randomize(longRange, 1L, 4L);
            listRandom.Randomize(listRange, 1, 4);

            var expected = new[] { 0, 3, 4, 1, 2, 5 };
            CollectionAssert.AreEqual(expected, intRange);
            CollectionAssert.AreEqual(expected, longRange);
            CollectionAssert.AreEqual(expected, listRange);
            Assert.That(intRandom.DrawCount, Is.EqualTo(3));
            Assert.That(longRandom.DrawCount, Is.EqualTo(3));
            Assert.That(listRandom.DrawCount, Is.EqualTo(3));
        }

        [Test]
        public void RandomizeNontrivialSelectionsUseCountMinusOneDraws()
        {
            AssertDrawCount(3, random => random.Randomize(new[] { 0, 1, 2, 3 }, 4L));
            AssertDrawCount(3, random => random.Randomize(new[] { 0, 1, 2, 3 }));
            AssertDrawCount(3, random => random.Randomize(new List<int> { 0, 1, 2, 3 }));
            AssertDrawCount(3, random => random.Randomize(new[] { 0, 1, 2, 3, 4 }, 1, 4));
            AssertDrawCount(3, random => random.Randomize(new[] { 0, 1, 2, 3, 4 }, 1L, 4L));
            AssertDrawCount(3, random => random.Randomize(new List<int> { 0, 1, 2, 3, 4 }, 1, 4));
            AssertDrawCount(3, random => { random.CreatePermutationArray(4); });
            AssertDrawCount(3, random => { random.CreatePermutationArrayLong(4); });
        }

        [Test]
        public void EmptyAndSingletonSelectionsUseNoRandomValues()
        {
            AssertDrawCount(0, random => random.Randomize(Array.Empty<int>()));
            AssertDrawCount(0, random => random.Randomize(new[] { 0 }));
            AssertDrawCount(0, random => random.Randomize(new int[2], 0L));
            AssertDrawCount(0, random => random.Randomize(new int[2], 1L));
            AssertDrawCount(0, random => random.Randomize(new List<int>()));
            AssertDrawCount(0, random => random.Randomize(new List<int> { 0 }));
            AssertDrawCount(0, random => random.Randomize(new int[2], 1, 0));
            AssertDrawCount(0, random => random.Randomize(new int[2], 1, 1));
            AssertDrawCount(0, random => random.Randomize(new int[2], 1L, 0L));
            AssertDrawCount(0, random => random.Randomize(new int[2], 1L, 1L));
            AssertDrawCount(0, random => random.Randomize(new List<int> { 0, 1 }, 1, 0));
            AssertDrawCount(0, random => random.Randomize(new List<int> { 0, 1 }, 1, 1));
            AssertDrawCount(0, random => { random.CreatePermutationArray(0); });
            AssertDrawCount(0, random => { random.CreatePermutationArray(1); });
            AssertDrawCount(0, random => { random.CreatePermutationArrayLong(0); });
            AssertDrawCount(0, random => { random.CreatePermutationArrayLong(1); });
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
