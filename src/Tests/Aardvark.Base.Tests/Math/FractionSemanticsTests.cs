using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.InteropServices;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class FractionSemanticsTests
    {
        [Test]
        public void UnreducedFiniteValuesUseNumericalEqualityAndHashing()
        {
            var half = new Fraction(1, 2);
            var equivalent = new Fraction(3, 6);
            var negativeHalf = new Fraction(-5, 10);
            var zeroA = new Fraction(0, 7);
            var zeroB = new Fraction(0, long.MaxValue);

            Assert.IsTrue(half == equivalent);
            Assert.IsFalse(half != equivalent);
            Assert.IsTrue(half.Equals(equivalent));
            Assert.AreEqual(half.GetHashCode(), equivalent.GetHashCode());

            Assert.IsTrue(half != negativeHalf);
            Assert.IsTrue(negativeHalf < half);
            Assert.IsTrue(zeroA == zeroB);
            Assert.IsTrue(zeroA.Equals(zeroB));
            Assert.AreEqual(zeroA.GetHashCode(), zeroB.GetHashCode());
            Assert.IsTrue(typeof(IEquatable<Fraction>).IsAssignableFrom(typeof(Fraction)));
        }

        [Test]
        public void HashSetUsesNumericalAndCollectionEquality()
        {
            var values = new HashSet<Fraction>
            {
                new Fraction(1, 2),
                new Fraction(2, 4),
                new Fraction(0, 3),
                new Fraction(0, 17),
                Fraction.NaN,
                new Fraction(0, 0),
                Fraction.PositiveInfinity,
                new Fraction(19, 0),
                Fraction.NegativeInfinity,
                new Fraction(-23, 0),
            };

            Assert.AreEqual(5, values.Count);
            Assert.IsTrue(values.Contains(new Fraction(17, 34)));
            Assert.IsTrue(values.Contains(Fraction.NaN));
            Assert.IsTrue(values.Contains(new Fraction(123, 0)));
            Assert.IsTrue(values.Contains(new Fraction(-123, 0)));
        }

        [Test]
        public void RoundedDoubleTiesUseExactOrdering()
        {
            var integer = new Fraction(9_007_199_254_740_992L, 1);
            var nextInteger = new Fraction(9_007_199_254_740_993L, 1);
            Assert.AreEqual(integer.Value, nextInteger.Value);
            AssertExactOrder(integer, nextInteger, -1);

            long max = long.MaxValue;
            var a = new Fraction(max, max - 1);
            var b = new Fraction(max - 1, max - 2);
            Assert.AreEqual(a.Value, b.Value);
            AssertExactOrder(a, b, -1);

            var negativeA = new Fraction(-max, max - 1);
            var negativeB = new Fraction(-(max - 1), max - 2);
            Assert.AreEqual(negativeA.Value, negativeB.Value);
            AssertExactOrder(negativeA, negativeB, 1);
        }

        [Test]
        public void RandomFiniteComparisonsMatchBigIntegerReference()
        {
            ulong state = 0xd1b54a32d192ed03UL;
            for (int i = 0; i < 25_000; i++)
            {
                long aNumerator = unchecked((long)Next(ref state));
                long bNumerator = unchecked((long)Next(ref state));
                long aDenominator = (long)(Next(ref state) & long.MaxValue);
                long bDenominator = (long)(Next(ref state) & long.MaxValue);
                if (aDenominator == 0) aDenominator = 1;
                if (bDenominator == 0) bDenominator = 1;

                var a = new Fraction(aNumerator, aDenominator);
                var b = new Fraction(bNumerator, bDenominator);
                int expected = System.Math.Sign(
                    ((BigInteger)a.Numerator * b.Denominator)
                    .CompareTo((BigInteger)b.Numerator * a.Denominator));

                AssertExactOrder(a, b, expected);
            }
        }

        [Test]
        public void ExtremeRoundedTiesMatchBigIntegerReference()
        {
            ulong state = 0x94d049bb133111ebUL;
            for (int i = 0; i < 2_000; i++)
            {
                long aDenominator = (long.MaxValue - 2048 - (long)(Next(ref state) & 0xfffffUL)) & ~2047L;
                long bDenominator = (long.MaxValue - 2048 - (long)(Next(ref state) & 0xfffffUL)) & ~2047L;
                long aNumerator = aDenominator + 1 + (long)(Next(ref state) & 31UL);
                long bNumerator = bDenominator + 1 + (long)(Next(ref state) & 31UL);
                var a = new Fraction(aNumerator, aDenominator);
                var b = new Fraction(bNumerator, bDenominator);

                Assert.AreEqual(a.Value, b.Value);
                int expected = System.Math.Sign(
                    ((BigInteger)a.Numerator * b.Denominator)
                    .CompareTo((BigInteger)b.Numerator * a.Denominator));
                AssertExactOrder(a, b, expected);
            }
        }

        [Test]
        public void LongExtremeStoredRepresentationsRemainExact()
        {
            var values = new[]
            {
                Raw(long.MinValue, 1),
                Raw(long.MaxValue, 1),
                Raw(long.MinValue, -1),
                Raw(long.MinValue, long.MinValue),
                Raw(1, long.MinValue),
                Raw(-1, long.MinValue),
                Raw(0, long.MinValue),
            };

            foreach (var a in values)
            {
                var reduced = a.Reduced;
                AssertExactOrder(a, reduced, 0);
                foreach (var b in values)
                    AssertExactOrder(a, b, ReferenceOrder(a, b));
            }
        }

        [Test]
        public void NaNAndInfinityPredicatesAreMutuallyCorrect()
        {
            var positive = new Fraction(7, 0);
            var negative = new Fraction(-7, 0);

            Assert.IsTrue(Fraction.IsNaN(Fraction.NaN));
            Assert.IsFalse(Fraction.IsInfinity(Fraction.NaN));
            Assert.IsFalse(Fraction.IsPositiveInfinity(Fraction.NaN));
            Assert.IsFalse(Fraction.IsNegativeInfinity(Fraction.NaN));

            Assert.IsFalse(Fraction.IsNaN(positive));
            Assert.IsTrue(Fraction.IsInfinity(positive));
            Assert.IsTrue(Fraction.IsPositiveInfinity(positive));
            Assert.IsFalse(Fraction.IsNegativeInfinity(positive));

            Assert.IsFalse(Fraction.IsNaN(negative));
            Assert.IsTrue(Fraction.IsInfinity(negative));
            Assert.IsFalse(Fraction.IsPositiveInfinity(negative));
            Assert.IsTrue(Fraction.IsNegativeInfinity(negative));

            Assert.IsFalse(Fraction.IsNaN(Fraction.Zero));
            Assert.IsFalse(Fraction.IsInfinity(Fraction.Zero));
        }

        [Test]
        public void OperatorsUseIeeeNaNSemanticsAndExactComplement()
        {
            var nan = Fraction.NaN;
            var anotherNaN = Fraction.NaN;
            var finite = new Fraction(1, 2);

            Assert.IsFalse(nan == anotherNaN);
            Assert.IsTrue(nan != anotherNaN);
            Assert.IsFalse(nan < anotherNaN);
            Assert.IsFalse(nan <= anotherNaN);
            Assert.IsFalse(nan > anotherNaN);
            Assert.IsFalse(nan >= anotherNaN);

            Assert.IsFalse(nan == finite);
            Assert.IsTrue(nan != finite);
            Assert.IsFalse(nan < finite);
            Assert.IsFalse(nan <= finite);
            Assert.IsFalse(nan > finite);
            Assert.IsFalse(nan >= finite);

            Assert.IsTrue(nan.Equals(Fraction.NaN));
            Assert.IsTrue(nan.Equals((object)Fraction.NaN));
            Assert.AreEqual(nan.GetHashCode(), Fraction.NaN.GetHashCode());

            var values = new[]
            {
                Fraction.NaN,
                Fraction.NegativeInfinity,
                new Fraction(-1, 3),
                Fraction.Zero,
                new Fraction(1, 3),
                Fraction.PositiveInfinity,
            };
            foreach (var a in values)
            foreach (var b in values)
                Assert.AreEqual(!(a == b), a != b);
        }

        [Test]
        public void InfinityOperatorsUseSignedIeeeOrdering()
        {
            var positive = new Fraction(17, 0);
            var negative = new Fraction(-19, 0);

            Assert.IsTrue(positive == Fraction.PositiveInfinity);
            Assert.IsTrue(negative == Fraction.NegativeInfinity);
            Assert.IsTrue(positive.Equals(Fraction.PositiveInfinity));
            Assert.IsTrue(negative.Equals(Fraction.NegativeInfinity));
            Assert.AreEqual(positive.GetHashCode(), Fraction.PositiveInfinity.GetHashCode());
            Assert.AreEqual(negative.GetHashCode(), Fraction.NegativeInfinity.GetHashCode());

            Assert.IsTrue(positive > Fraction.MaxValue);
            Assert.IsTrue(positive >= Fraction.PositiveInfinity);
            Assert.IsTrue(negative < Fraction.MinValue);
            Assert.IsTrue(negative <= Fraction.NegativeInfinity);
            Assert.IsTrue(negative < positive);
            Assert.IsTrue(positive != negative);
        }

        [Test]
        public void ReducedCanonicalizesFiniteAndSpecialValues()
        {
            AssertRaw(new Fraction(6, 8).Reduced, 3, 4);
            AssertRaw(new Fraction(-6, 8).Reduced, -3, 4);
            AssertRaw(new Fraction(0, long.MaxValue).Reduced, 0, 1);
            AssertRaw(new Fraction(long.MinValue, 2).Reduced, long.MinValue / 2, 1);

            AssertRaw(Fraction.NaN.Reduced, 0, 0);
            AssertRaw(new Fraction(17, 0).Reduced, 1, 0);
            AssertRaw(new Fraction(-17, 0).Reduced, -1, 0);
        }

        [Test]
        public void InfinityAdditionAndSubtractionAreDefined()
        {
            var finite = new Fraction(3, 7);
            var positive = new Fraction(17, 0);
            var negative = new Fraction(-19, 0);

            AssertRaw(positive + positive, 1, 0);
            AssertRaw(negative + negative, -1, 0);
            AssertRaw(positive + finite, 1, 0);
            AssertRaw(finite + positive, 1, 0);
            AssertRaw(negative + finite, -1, 0);
            AssertRaw(finite + negative, -1, 0);
            Assert.IsTrue(Fraction.IsNaN(positive + negative));
            Assert.IsTrue(Fraction.IsNaN(negative + positive));
            Assert.IsTrue(Fraction.IsNaN(Fraction.NaN + finite));
            Assert.IsTrue(Fraction.IsNaN(finite + Fraction.NaN));

            Assert.IsTrue(Fraction.IsNaN(positive - positive));
            Assert.IsTrue(Fraction.IsNaN(negative - negative));
            AssertRaw(positive - negative, 1, 0);
            AssertRaw(negative - positive, -1, 0);
            AssertRaw(finite - positive, -1, 0);
            AssertRaw(finite - negative, 1, 0);
            Assert.IsTrue(Fraction.IsNaN(Fraction.NaN - positive));
        }

        [Test]
        public void StructLayoutAndRawFieldsRemainStable()
        {
            Assert.AreEqual(16, Marshal.SizeOf<Fraction>());
            Assert.AreEqual(IntPtr.Zero, Marshal.OffsetOf<Fraction>(nameof(Fraction.Numerator)));
            Assert.AreEqual(new IntPtr(8), Marshal.OffsetOf<Fraction>(nameof(Fraction.Denominator)));

            var value = new Fraction(12, 30);
            AssertRaw(value, 12, 30);
            Assert.AreEqual("12/30", value.ToString());
        }

        private static void AssertExactOrder(Fraction a, Fraction b, int expected)
        {
            Assert.AreEqual(expected < 0, a < b);
            Assert.AreEqual(expected <= 0, a <= b);
            Assert.AreEqual(expected == 0, a == b);
            Assert.AreEqual(expected != 0, a != b);
            Assert.AreEqual(expected >= 0, a >= b);
            Assert.AreEqual(expected > 0, a > b);
            Assert.AreEqual(expected == 0, a.Equals(b));
            if (expected == 0) Assert.AreEqual(a.GetHashCode(), b.GetHashCode());
        }

        private static void AssertRaw(Fraction actual, long numerator, long denominator)
        {
            Assert.AreEqual(numerator, actual.Numerator);
            Assert.AreEqual(denominator, actual.Denominator);
        }

        private static Fraction Raw(long numerator, long denominator)
            => new Fraction { Numerator = numerator, Denominator = denominator };

        private static int ReferenceOrder(Fraction a, Fraction b)
        {
            BigInteger aNumerator = a.Numerator;
            BigInteger aDenominator = a.Denominator;
            BigInteger bNumerator = b.Numerator;
            BigInteger bDenominator = b.Denominator;
            if (aDenominator < 0)
            {
                aNumerator = -aNumerator;
                aDenominator = -aDenominator;
            }
            if (bDenominator < 0)
            {
                bNumerator = -bNumerator;
                bDenominator = -bDenominator;
            }
            return System.Math.Sign((aNumerator * bDenominator).CompareTo(bNumerator * aDenominator));
        }

        private static ulong Next(ref ulong state)
        {
            state ^= state >> 12;
            state ^= state << 25;
            state ^= state >> 27;
            return state * 0x2545f4914f6cdd1dUL;
        }
    }
}
