using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Numerics;

namespace Aardvark.Tests
{
    [TestFixture]
    public class CommonDivisorMultipleTests
    {
        private static BigInteger ReferenceGcd(BigInteger a, BigInteger b)
            => BigInteger.GreatestCommonDivisor(a, b);

        private static BigInteger ReferenceLcm(BigInteger a, BigInteger b)
        {
            if (a.IsZero || b.IsZero) return BigInteger.Zero;
            return BigInteger.Abs(a / ReferenceGcd(a, b) * b);
        }

        private static void AssertOverflow(Action action, string description)
        {
            try
            {
                action();
            }
            catch (OverflowException)
            {
                return;
            }

            Assert.Fail("Expected OverflowException for {0}.", description);
        }

        private static void AssertIntPair(int a, int b)
        {
            var expectedGcd = ReferenceGcd(a, b);
            var expectedLcm = ReferenceLcm(a, b);
            var description = $"({a}, {b})";

            if (expectedGcd <= int.MaxValue)
                Assert.That(Fun.GreatestCommonDivisor(a, b), Is.EqualTo((int)expectedGcd), $"GCD {description}");
            else
                AssertOverflow(() => Fun.GreatestCommonDivisor(a, b), $"GCD {description}");

            if (expectedLcm <= int.MaxValue)
                Assert.That(Fun.LeastCommonMultiple(a, b), Is.EqualTo((int)expectedLcm), $"LCM {description}");
            else
                AssertOverflow(() => Fun.LeastCommonMultiple(a, b), $"LCM {description}");
        }

        private static void AssertLongPair(long a, long b)
        {
            var expectedGcd = ReferenceGcd(a, b);
            var expectedLcm = ReferenceLcm(a, b);
            var description = $"({a}, {b})";

            if (expectedGcd <= long.MaxValue)
                Assert.That(Fun.GreatestCommonDivisor(a, b), Is.EqualTo((long)expectedGcd), $"GCD {description}");
            else
                AssertOverflow(() => Fun.GreatestCommonDivisor(a, b), $"GCD {description}");

            if (expectedLcm <= long.MaxValue)
                Assert.That(Fun.LeastCommonMultiple(a, b), Is.EqualTo((long)expectedLcm), $"LCM {description}");
            else
                AssertOverflow(() => Fun.LeastCommonMultiple(a, b), $"LCM {description}");
        }

        private static void AssertUIntPair(uint a, uint b)
        {
            var expectedGcd = ReferenceGcd(a, b);
            var expectedLcm = ReferenceLcm(a, b);
            var description = $"({a}, {b})";

            Assert.That(Fun.GreatestCommonDivisor(a, b), Is.EqualTo((uint)expectedGcd), $"GCD {description}");

            if (expectedLcm <= uint.MaxValue)
                Assert.That(Fun.LeastCommonMultiple(a, b), Is.EqualTo((uint)expectedLcm), $"LCM {description}");
            else
                AssertOverflow(() => Fun.LeastCommonMultiple(a, b), $"LCM {description}");
        }

        private static void AssertULongPair(ulong a, ulong b)
        {
            var expectedGcd = ReferenceGcd(a, b);
            var expectedLcm = ReferenceLcm(a, b);
            var description = $"({a}, {b})";

            Assert.That(Fun.GreatestCommonDivisor(a, b), Is.EqualTo((ulong)expectedGcd), $"GCD {description}");

            if (expectedLcm <= ulong.MaxValue)
                Assert.That(Fun.LeastCommonMultiple(a, b), Is.EqualTo((ulong)expectedLcm), $"LCM {description}");
            else
                AssertOverflow(() => Fun.LeastCommonMultiple(a, b), $"LCM {description}");
        }

        private static ulong NextUInt64(ref ulong state)
        {
            unchecked
            {
                state += 0x9E3779B97F4A7C15UL;
                var value = state;
                value = (value ^ (value >> 30)) * 0xBF58476D1CE4E5B9UL;
                value = (value ^ (value >> 27)) * 0x94D049BB133111EBUL;
                return value ^ (value >> 31);
            }
        }

        [Test]
        public void SignedOverloadsMatchBigIntegerExhaustively()
        {
            for (var a = -128; a <= 128; a++)
            {
                for (var b = -128; b <= 128; b++)
                {
                    AssertIntPair(a, b);
                    AssertLongPair(a, b);
                }
            }
        }

        [Test]
        public void AllScalarOverloadsMatchBigIntegerForFullWidthValues()
        {
            ulong state = 0xC6BC279692B5CC83UL;

            for (var i = 0; i < 512; i++)
            {
                var a = NextUInt64(ref state);
                var b = NextUInt64(ref state);

                AssertIntPair(unchecked((int)a), unchecked((int)b));
                AssertLongPair(unchecked((long)a), unchecked((long)b));
                AssertUIntPair((uint)a, (uint)b);
                AssertULongPair(a, b);

                // Same-value pairs keep full-width LCM results representable.
                AssertIntPair(unchecked((int)a), unchecked((int)a));
                AssertLongPair(unchecked((long)a), unchecked((long)a));
                AssertUIntPair((uint)a, (uint)a);
                AssertULongPair(a, a);
            }
        }

        [TestCase(0, 0, 0, 0)]
        [TestCase(0, 15, 15, 0)]
        [TestCase(18, 24, 6, 72)]
        [TestCase(35, 64, 1, 2240)]
        public void SignedResultsAreNonNegativeAndSignSymmetric(int a, int b, int gcd, int lcm)
        {
            var signs = new[] { (a, b), (-a, b), (a, -b), (-a, -b) };

            foreach (var pair in signs)
            {
                Assert.That(Fun.GreatestCommonDivisor(pair.Item1, pair.Item2), Is.EqualTo(gcd));
                Assert.That(Fun.LeastCommonMultiple(pair.Item1, pair.Item2), Is.EqualTo(lcm));
                Assert.That(Fun.GreatestCommonDivisor((long)pair.Item1, pair.Item2), Is.EqualTo(gcd));
                Assert.That(Fun.LeastCommonMultiple((long)pair.Item1, pair.Item2), Is.EqualTo(lcm));
            }
        }

        [Test]
        public void UnsignedZeroCasesFollowDefinitions()
        {
            Assert.That(Fun.GreatestCommonDivisor(0u, 0u), Is.Zero);
            Assert.That(Fun.LeastCommonMultiple(0u, 0u), Is.Zero);
            Assert.That(Fun.GreatestCommonDivisor(0UL, 0UL), Is.Zero);
            Assert.That(Fun.LeastCommonMultiple(0UL, 0UL), Is.Zero);
        }

        [Test]
        public void SignedMinValuesFollowMathematicalMagnitudeAndOverflowRules()
        {
            AssertIntPair(int.MinValue, 0);
            AssertIntPair(int.MinValue, int.MinValue);
            AssertIntPair(int.MinValue, -1);
            AssertIntPair(int.MinValue, 2);
            AssertIntPair(int.MinValue, 1 << 30);

            AssertLongPair(long.MinValue, 0);
            AssertLongPair(long.MinValue, long.MinValue);
            AssertLongPair(long.MinValue, -1);
            AssertLongPair(long.MinValue, 2);
            AssertLongPair(long.MinValue, 1L << 62);
        }

        [Test]
        public void DivisionBeforeMultiplicationPreservesRepresentableResults()
        {
            Assert.That(Fun.LeastCommonMultiple(65_536, 65_536), Is.EqualTo(65_536));
            Assert.That(Fun.LeastCommonMultiple(int.MaxValue, int.MaxValue), Is.EqualTo(int.MaxValue));

            const long longFactor = 4_294_967_296L;
            Assert.That(Fun.LeastCommonMultiple(longFactor, longFactor), Is.EqualTo(longFactor));
            Assert.That(Fun.LeastCommonMultiple(long.MaxValue, long.MaxValue), Is.EqualTo(long.MaxValue));

            Assert.That(Fun.LeastCommonMultiple(65_536u, 65_536u), Is.EqualTo(65_536u));
            Assert.That(Fun.LeastCommonMultiple(65_521u, 65_519u), Is.EqualTo(4_292_870_399u));
            Assert.That(Fun.LeastCommonMultiple(uint.MaxValue, uint.MaxValue), Is.EqualTo(uint.MaxValue));

            const ulong ulongFactor = 4_294_967_296UL;
            Assert.That(Fun.LeastCommonMultiple(ulongFactor, ulongFactor), Is.EqualTo(ulongFactor));
            Assert.That(Fun.LeastCommonMultiple(4_000_000_007UL, 4_000_000_009UL), Is.EqualTo(16_000_000_064_000_000_063UL));
            Assert.That(Fun.LeastCommonMultiple(ulong.MaxValue, ulong.MaxValue), Is.EqualTo(ulong.MaxValue));
        }

        [Test]
        public void ExactLcmOverflowIsReported()
        {
            AssertOverflow(() => Fun.LeastCommonMultiple(int.MaxValue, 2), "int LCM");
            AssertOverflow(() => Fun.LeastCommonMultiple(long.MaxValue, 2), "long LCM");
            AssertOverflow(() => Fun.LeastCommonMultiple(uint.MaxValue, 2), "uint LCM");
            AssertOverflow(() => Fun.LeastCommonMultiple(ulong.MaxValue, 2), "ulong LCM");
        }

        [Test]
        public void GeneratedVectorOverloadsUseCorrectScalarSemantics()
        {
            Assert.That(
                new V4i(-12, 18, 0, int.MinValue).GreatestCommonDivisor(new V4i(18, -24, 0, 2)),
                Is.EqualTo(new V4i(6, 6, 0, 2))
            );
            Assert.That(
                new V4i(-12, 18, 0, 65_536).LeastCommonMultiple(new V4i(18, -24, 7, 65_536)),
                Is.EqualTo(new V4i(36, 72, 0, 65_536))
            );

            const long factor = 4_294_967_296L;
            Assert.That(
                new V3l(-21, factor, 0).LeastCommonMultiple(new V3l(6, factor, long.MinValue)),
                Is.EqualTo(new V3l(42, factor, 0))
            );

            Assert.That(
                new V2ui(uint.MaxValue, 65_536).GreatestCommonDivisor(new V2ui(0, 65_536)),
                Is.EqualTo(new V2ui(uint.MaxValue, 65_536))
            );
            Assert.That(
                new V2ui(uint.MaxValue, 65_536).LeastCommonMultiple(new V2ui(uint.MaxValue, 65_536)),
                Is.EqualTo(new V2ui(uint.MaxValue, 65_536))
            );
        }
    }
}
