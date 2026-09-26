using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class UniformDoubleFullTests
    {
        private const double Unit53 = 1.0 / 9007199254740992.0;

        private sealed class ScriptedRandom : IRandomUniform
        {
            private readonly bool m_generatesFullDoubles;
            private readonly int[] m_integerValues;
            private readonly double[] m_doubleValues;
            private int m_integerIndex;
            private int m_doubleIndex;

            public ScriptedRandom(
                bool generatesFullDoubles,
                int[] integerValues = null,
                double[] doubleValues = null)
            {
                m_generatesFullDoubles = generatesFullDoubles;
                m_integerValues = integerValues ?? Array.Empty<int>();
                m_doubleValues = doubleValues ?? Array.Empty<double>();
            }

            public int CapabilityReadCount { get; private set; }
            public int IntegerDrawCount => m_integerIndex;
            public int DoubleDrawCount => m_doubleIndex;
            public int RandomBits => m_generatesFullDoubles ? 53 : 31;

            public bool GeneratesFullDoubles
            {
                get
                {
                    CapabilityReadCount++;
                    return m_generatesFullDoubles;
                }
            }

            public void ReSeed(int seed) { }

            public int UniformInt()
            {
                if (m_integerIndex >= m_integerValues.Length)
                    throw new InvalidOperationException("Unexpected integer draw.");

                return m_integerValues[m_integerIndex++];
            }

            public uint UniformUInt() => throw new NotSupportedException();
            public long UniformLong() => throw new NotSupportedException();
            public ulong UniformULong() => throw new NotSupportedException();
            public float UniformFloat() => throw new NotSupportedException();
            public float UniformFloatClosed() => throw new NotSupportedException();
            public float UniformFloatOpen() => throw new NotSupportedException();

            public double UniformDouble()
            {
                if (m_doubleIndex >= m_doubleValues.Length)
                    throw new InvalidOperationException("Unexpected double draw.");

                return m_doubleValues[m_doubleIndex++];
            }

            public double UniformDoubleClosed() => throw new NotSupportedException();
            public double UniformDoubleOpen() => throw new NotSupportedException();
        }

        [Test]
        public void FillUniformFullReconstructsExactHalfOpenValues()
        {
            var random = new ScriptedRandom(
                false,
                new[] { 0, 0, 31, 63, int.MaxValue, int.MaxValue });
            var values = new double[3];

            random.FillUniformFull(values);

            CollectionAssert.AreEqual(
                new[]
                {
                    0.0,
                    67108865.0 * Unit53,
                    9007199254740991.0 * Unit53,
                },
                values);
            Assert.That(values[2], Is.LessThan(1.0));
            Assert.That(random.IntegerDrawCount, Is.EqualTo(2 * values.Length));
            Assert.That(random.DoubleDrawCount, Is.Zero);
            Assert.That(random.CapabilityReadCount, Is.EqualTo(1));
        }

        [Test]
        public void FillUniformFullUsesNativeFullDoublesDirectly()
        {
            var expected = new[] { 0.0, 0.5, 1.0 - Unit53 };
            var random = new ScriptedRandom(true, doubleValues: expected);
            var values = new double[expected.Length];

            random.FillUniformFull(values);

            CollectionAssert.AreEqual(expected, values);
            Assert.That(random.IntegerDrawCount, Is.Zero);
            Assert.That(random.DoubleDrawCount, Is.EqualTo(values.Length));
            Assert.That(random.CapabilityReadCount, Is.EqualTo(1));
        }

        [Test]
        public void FillUniformFullMatchesRepeatedScalarGeneration()
        {
            var integerScript = new[]
            {
                0x00000010, 0x00000020,
                0x12345678, 0x23456789,
                0x34567890, 0x45678901,
                0x56789012, 0x67890123,
            };
            var bulkIntegerRandom = new ScriptedRandom(false, integerScript);
            var scalarIntegerRandom = new ScriptedRandom(false, integerScript);
            var bulkIntegers = new double[4];
            var scalarIntegers = new double[4];

            bulkIntegerRandom.FillUniformFull(bulkIntegers);
            for (int i = 0; i < scalarIntegers.Length; i++)
                scalarIntegers[i] = scalarIntegerRandom.UniformDoubleFull();

            CollectionAssert.AreEqual(scalarIntegers, bulkIntegers);
            Assert.That(bulkIntegerRandom.IntegerDrawCount, Is.EqualTo(8));
            Assert.That(scalarIntegerRandom.IntegerDrawCount, Is.EqualTo(8));
            Assert.That(bulkIntegerRandom.CapabilityReadCount, Is.EqualTo(1));
            Assert.That(scalarIntegerRandom.CapabilityReadCount, Is.EqualTo(4));

            var doubleScript = new[] { 0.125, 0.25, 0.5, 0.75 };
            var bulkDoubleRandom = new ScriptedRandom(true, doubleValues: doubleScript);
            var scalarDoubleRandom = new ScriptedRandom(true, doubleValues: doubleScript);
            var bulkDoubles = new double[4];
            var scalarDoubles = new double[4];

            bulkDoubleRandom.FillUniformFull(bulkDoubles);
            for (int i = 0; i < scalarDoubles.Length; i++)
                scalarDoubles[i] = scalarDoubleRandom.UniformDoubleFull();

            CollectionAssert.AreEqual(scalarDoubles, bulkDoubles);
            Assert.That(bulkDoubleRandom.DoubleDrawCount, Is.EqualTo(4));
            Assert.That(scalarDoubleRandom.DoubleDrawCount, Is.EqualTo(4));
            Assert.That(bulkDoubleRandom.CapabilityReadCount, Is.EqualTo(1));
            Assert.That(scalarDoubleRandom.CapabilityReadCount, Is.EqualTo(4));
        }

        [Test]
        public void CreateUniformDoubleFullArrayUsesFullPrecisionPaths()
        {
            var reconstructedRandom = new ScriptedRandom(
                false,
                new[] { 0, 0, int.MaxValue, int.MaxValue });
            var nativeExpected = new[] { 0.25, 0.75 };
            var nativeRandom = new ScriptedRandom(true, doubleValues: nativeExpected);

            var reconstructed = reconstructedRandom.CreateUniformDoubleFullArray(2);
            var native = nativeRandom.CreateUniformDoubleFullArray(2);

            CollectionAssert.AreEqual(
                new[] { 0.0, 9007199254740991.0 * Unit53 },
                reconstructed);
            CollectionAssert.AreEqual(nativeExpected, native);
            Assert.That(reconstructedRandom.IntegerDrawCount, Is.EqualTo(4));
            Assert.That(reconstructedRandom.DoubleDrawCount, Is.Zero);
            Assert.That(reconstructedRandom.CapabilityReadCount, Is.EqualTo(1));
            Assert.That(nativeRandom.IntegerDrawCount, Is.Zero);
            Assert.That(nativeRandom.DoubleDrawCount, Is.EqualTo(2));
            Assert.That(nativeRandom.CapabilityReadCount, Is.EqualTo(1));
        }

        [TestCase(false)]
        [TestCase(true)]
        public void EmptyFullDoubleArraysConsumeNoRandomValues(bool generatesFullDoubles)
        {
            var random = new ScriptedRandom(generatesFullDoubles);

            random.FillUniformFull(Array.Empty<double>());
            var created = random.CreateUniformDoubleFullArray(0);

            CollectionAssert.IsEmpty(created);
            Assert.That(random.IntegerDrawCount, Is.Zero);
            Assert.That(random.DoubleDrawCount, Is.Zero);
            Assert.That(random.CapabilityReadCount, Is.EqualTo(2));
        }
    }
}
