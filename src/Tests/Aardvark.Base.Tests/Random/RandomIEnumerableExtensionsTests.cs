using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Linq;

namespace Aardvark.Tests
{
    [TestFixture]
    public class RandomIEnumerableExtensionsTests
    {
        private sealed class ConstantRandom : IRandomUniform
        {
            private readonly double m_value;

            public ConstantRandom(double value)
            {
                m_value = value;
            }

            public int RandomBits => 31;

            public bool GeneratesFullDoubles => false;

            public void ReSeed(int seed) { }

            public int UniformInt() => throw new NotSupportedException();

            public uint UniformUInt() => throw new NotSupportedException();

            public long UniformLong() => throw new NotSupportedException();

            public ulong UniformULong() => throw new NotSupportedException();

            public float UniformFloat() => (float)m_value;

            public float UniformFloatClosed() => (float)m_value;

            public float UniformFloatOpen() => (float)m_value;

            public double UniformDouble() => m_value;

            public double UniformDoubleClosed() => m_value;

            public double UniformDoubleOpen() => m_value;
        }

        private static readonly int[] s_values = { 1, 2, 3 };

        [Test]
        public void TakeRandomlyRejectsNaNProbabilityWhenEnumerated()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => s_values.TakeRandomly(double.NaN).ToArray());

            Assert.AreEqual("p", exception.ParamName);
        }

        [Test]
        public void TakeRandomlyWithSelectorRejectsNaNProbabilityWhenEnumerated()
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => s_values.TakeRandomly(v => v.ToString(), double.NaN).ToArray());

            Assert.AreEqual("p", exception.ParamName);
        }

        [TestCase(-0.01)]
        [TestCase(1.01)]
        public void TakeRandomlyRejectsOutOfRangeProbabilities(double p)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => s_values.TakeRandomly(p).ToArray());

            Assert.AreEqual("p", exception.ParamName);
        }

        [TestCase(-0.01)]
        [TestCase(1.01)]
        public void TakeRandomlyWithSelectorRejectsOutOfRangeProbabilities(double p)
        {
            var exception = Assert.Throws<ArgumentOutOfRangeException>(
                () => s_values.TakeRandomly(v => v.ToString(), p).ToArray());

            Assert.AreEqual("p", exception.ParamName);
        }

        [Test]
        public void TakeRandomlyAcceptsBoundaryProbabilities()
        {
            var random = new ConstantRandom(0.5);

            CollectionAssert.IsEmpty(s_values.TakeRandomly(0.0, random).ToArray());
            CollectionAssert.AreEqual(s_values, s_values.TakeRandomly(1.0, random).ToArray());
        }

        [Test]
        public void TakeRandomlyWithSelectorAcceptsBoundaryProbabilities()
        {
            var random = new ConstantRandom(0.5);

            CollectionAssert.IsEmpty(s_values.TakeRandomly(v => v.ToString(), 0.0, random).ToArray());
            CollectionAssert.AreEqual(new[] { "1", "2", "3" }, s_values.TakeRandomly(v => v.ToString(), 1.0, random).ToArray());
        }
    }
}
