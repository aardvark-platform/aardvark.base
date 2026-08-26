using System;
using System.Collections.Generic;
using NUnit.Framework;
using AardvarkHalf = Aardvark.Base.Half;
using SystemHalf = System.Half;

namespace Aardvark.Tests
{
    [TestFixture]
    public class HalfSemanticsTests
    {
        private static readonly ushort[] s_representativeBits =
        {
            0xffff, 0xfe00, 0xfc01, 0xfc00, 0xfbff,
            0xc000, 0xbc00, 0x8400, 0x83ff, 0x8001, 0x8000,
            0x0000, 0x0001, 0x03ff, 0x0400, 0x3c00, 0x4000,
            0x7bff, 0x7c00, 0x7c01, 0x7e00, 0x7fff
        };

        private static SystemHalf ToSystemHalf(ushort bits)
            => BitConverter.UInt16BitsToHalf(bits);

        private static void Check(bool condition, ushort bits, string operation)
        {
            if (!condition)
                Assert.Fail($"Half {operation} mismatch for bits 0x{bits:x4}.");
        }

        private static void Check(bool condition, ushort left, ushort right, string operation)
        {
            if (!condition)
                Assert.Fail($"Half {operation} mismatch for bits 0x{left:x4} and 0x{right:x4}.");
        }

        private static void ValidatePair(ushort leftBits, ushort rightBits)
        {
            AardvarkHalf left = AardvarkHalf.ToHalf(leftBits);
            AardvarkHalf right = AardvarkHalf.ToHalf(rightBits);
            SystemHalf systemLeft = ToSystemHalf(leftBits);
            SystemHalf systemRight = ToSystemHalf(rightBits);

            Check((left == right) == (systemLeft == systemRight), leftBits, rightBits, "operator ==");
            Check((left != right) == (systemLeft != systemRight), leftBits, rightBits, "operator !=");
            Check((left < right) == (systemLeft < systemRight), leftBits, rightBits, "operator <");
            Check((left > right) == (systemLeft > systemRight), leftBits, rightBits, "operator >");
            Check((left <= right) == (systemLeft <= systemRight), leftBits, rightBits, "operator <=");
            Check((left >= right) == (systemLeft >= systemRight), leftBits, rightBits, "operator >=");
            Check(left.Equals(right) == systemLeft.Equals(systemRight), leftBits, rightBits, "Equals");
            Check(left.CompareTo(right) == systemLeft.CompareTo(systemRight), leftBits, rightBits, "CompareTo");

            bool hasNaN = SystemHalf.IsNaN(systemLeft) || SystemHalf.IsNaN(systemRight);
            AardvarkHalf max = AardvarkHalf.Max(left, right);
            AardvarkHalf min = AardvarkHalf.Min(left, right);
            if (hasNaN)
            {
                Check(AardvarkHalf.IsNaN(max), leftBits, rightBits, "Max NaN propagation");
                Check(AardvarkHalf.IsNaN(min), leftBits, rightBits, "Min NaN propagation");
            }
            else
            {
                ushort expectedMax = BitConverter.HalfToUInt16Bits(SystemHalf.Max(systemLeft, systemRight));
                ushort expectedMin = BitConverter.HalfToUInt16Bits(SystemHalf.Min(systemLeft, systemRight));
                Check(AardvarkHalf.GetBits(max) == expectedMax, leftBits, rightBits, "Max");
                Check(AardvarkHalf.GetBits(min) == expectedMin, leftBits, rightBits, "Min");
            }
        }

        [Test]
        public void UnaryAndSelfComparisonSemanticsMatchSystemHalfForEveryEncoding()
        {
            for (int raw = ushort.MinValue; raw <= ushort.MaxValue; raw++)
            {
                ushort bits = (ushort)raw;
                AardvarkHalf value = AardvarkHalf.ToHalf(bits);
                AardvarkHalf same = AardvarkHalf.ToHalf(bits);
                SystemHalf systemValue = ToSystemHalf(bits);
                SystemHalf systemSame = ToSystemHalf(bits);

                Check(AardvarkHalf.IsNaN(value) == SystemHalf.IsNaN(systemValue), bits, "IsNaN");
                Check((value == same) == (systemValue == systemSame), bits, "self operator ==");
                Check((value != same) == (systemValue != systemSame), bits, "self operator !=");
                Check((value < same) == (systemValue < systemSame), bits, "self operator <");
                Check((value > same) == (systemValue > systemSame), bits, "self operator >");
                Check((value <= same) == (systemValue <= systemSame), bits, "self operator <=");
                Check((value >= same) == (systemValue >= systemSame), bits, "self operator >=");
                Check(value.Equals(same) == systemValue.Equals(systemSame), bits, "self Equals");
                Check(value.CompareTo(same) == systemValue.CompareTo(systemSame), bits, "self CompareTo");
                Check(value.GetHashCode() == systemValue.GetHashCode(), bits, "GetHashCode");

                if (SystemHalf.IsNaN(systemValue))
                {
                    bool threw = false;
                    try
                    {
                        _ = AardvarkHalf.Sign(value);
                    }
                    catch (ArithmeticException)
                    {
                        threw = true;
                    }

                    Check(threw, bits, "Sign NaN exception");
                }
                else
                {
                    Check(AardvarkHalf.Sign(value) == SystemHalf.Sign(systemValue), bits, "Sign");
                }
            }
        }

        [Test]
        public void RepresentativePairsMatchSystemHalf()
        {
            foreach (ushort left in s_representativeBits)
                foreach (ushort right in s_representativeBits)
                    ValidatePair(left, right);
        }

        [Test]
        public void DeterministicPairsMatchSystemHalfAcrossAllLeftEncodings()
        {
            var random = new Random(0x5eed115);
            for (int raw = ushort.MinValue; raw <= ushort.MaxValue; raw++)
                ValidatePair((ushort)raw, (ushort)random.Next(ushort.MaxValue + 1));
        }

        [Test]
        public void EqualityAndHashingSupportNumericCollectionSemantics()
        {
            AardvarkHalf positiveZero = AardvarkHalf.ToHalf(0x0000);
            AardvarkHalf negativeZero = AardvarkHalf.ToHalf(0x8000);
            AardvarkHalf positiveNaN = AardvarkHalf.ToHalf(0x7c01);
            AardvarkHalf positiveNaNCopy = AardvarkHalf.ToHalf(0x7c01);
            AardvarkHalf negativeNaN = AardvarkHalf.ToHalf(0xffff);

            Assert.That(positiveZero == negativeZero, Is.True);
            Assert.That(positiveZero != negativeZero, Is.False);
            Assert.That(positiveNaN == positiveNaNCopy, Is.False);
            Assert.That(positiveNaN != positiveNaNCopy, Is.True);
            Assert.That(positiveNaN.Equals(negativeNaN), Is.True);
            Assert.That(positiveZero.GetHashCode(), Is.EqualTo(negativeZero.GetHashCode()));
            Assert.That(positiveNaN.GetHashCode(), Is.EqualTo(negativeNaN.GetHashCode()));

            var values = new HashSet<AardvarkHalf>
            {
                positiveZero, negativeZero, positiveNaN, negativeNaN
            };
            Assert.That(values.Count, Is.EqualTo(2));
        }

        [Test]
        public void ExtremaPropagateNaNAndChooseSignedZeroSymmetrically()
        {
            AardvarkHalf positiveZero = AardvarkHalf.ToHalf(0x0000);
            AardvarkHalf negativeZero = AardvarkHalf.ToHalf(0x8000);
            AardvarkHalf one = AardvarkHalf.One;
            ushort canonicalNaN = AardvarkHalf.GetBits(AardvarkHalf.NaN);

            foreach (ushort bits in new ushort[] { 0x7c01, 0x7e00, 0xfc01, 0xffff })
            {
                AardvarkHalf nan = AardvarkHalf.ToHalf(bits);
                Assert.That(AardvarkHalf.GetBits(AardvarkHalf.Max(nan, one)), Is.EqualTo(canonicalNaN));
                Assert.That(AardvarkHalf.GetBits(AardvarkHalf.Max(one, nan)), Is.EqualTo(canonicalNaN));
                Assert.That(AardvarkHalf.GetBits(AardvarkHalf.Min(nan, one)), Is.EqualTo(canonicalNaN));
                Assert.That(AardvarkHalf.GetBits(AardvarkHalf.Min(one, nan)), Is.EqualTo(canonicalNaN));
            }

            Assert.That(AardvarkHalf.GetBits(AardvarkHalf.Max(positiveZero, negativeZero)), Is.EqualTo(0x0000));
            Assert.That(AardvarkHalf.GetBits(AardvarkHalf.Max(negativeZero, positiveZero)), Is.EqualTo(0x0000));
            Assert.That(AardvarkHalf.GetBits(AardvarkHalf.Min(positiveZero, negativeZero)), Is.EqualTo(0x8000));
            Assert.That(AardvarkHalf.GetBits(AardvarkHalf.Min(negativeZero, positiveZero)), Is.EqualTo(0x8000));
        }
    }
}
