using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class RandomSystemTests
    {
        private sealed class ByteSequenceRandom : Random
        {
            private readonly byte[][] m_values;
            private int m_index;

            public ByteSequenceRandom(params byte[][] values)
            {
                m_values = values;
            }

            public override void NextBytes(byte[] buffer)
            {
                Array.Clear(buffer, 0, buffer.Length);

                var value = m_values[m_index++];
                Array.Copy(value, buffer, Fun.Min(value.Length, buffer.Length));
            }
        }

        private static RandomSystem Create(params byte[][] values)
        {
            return new RandomSystem(0)
            {
                Generator = new ByteSequenceRandom(values)
            };
        }

        private static byte[] Bytes(uint value) => BitConverter.GetBytes(value);

        [Test]
        public void UniformIntCanReturnIntMaxValue()
        {
            var random = Create(Bytes(uint.MaxValue));

            Assert.AreEqual(int.MaxValue, random.UniformInt());
        }

        [Test]
        public void UniformFloatClosedMapsZeroAndMaxToClosedBoundaries()
        {
            Assert.AreEqual(0.0f, Create(Bytes(0u)).UniformFloatClosed());
            Assert.AreEqual(1.0f, Create(Bytes(uint.MaxValue)).UniformFloatClosed());
        }

        [Test]
        public void UniformDoubleClosedMapsZeroAndMaxToClosedBoundaries()
        {
            Assert.AreEqual(0.0, Create(Bytes(0u)).UniformDoubleClosed());
            Assert.AreEqual(1.0, Create(Bytes(uint.MaxValue)).UniformDoubleClosed());
        }

        [Test]
        public void UniformDoubleOpenSkipsZero()
        {
            var value = Create(Bytes(0u), Bytes(1u)).UniformDoubleOpen();

            Assert.AreEqual(1.0 / 2147483648.0, value);
            Assert.IsTrue(value > 0.0);
            Assert.IsTrue(value < 1.0);
        }

        [Test]
        public void UniformDoubleOpenKeepsMaxBelowOne()
        {
            var value = Create(Bytes(uint.MaxValue)).UniformDoubleOpen();

            Assert.AreEqual(int.MaxValue * (1.0 / 2147483648.0), value);
            Assert.IsTrue(value > 0.0);
            Assert.IsTrue(value < 1.0);
        }
    }
}
