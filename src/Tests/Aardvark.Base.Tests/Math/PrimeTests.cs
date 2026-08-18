using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class PrimeTests
    {
        private static bool ReferenceIsPrime(long value)
        {
            if (value < 2) return false;

            for (long divisor = 2; divisor <= value / divisor; divisor++)
                if (value % divisor == 0) return false;

            return true;
        }

        [Test]
        public void OverloadsMatchIndependentReferenceAcrossBoundedRange()
        {
            for (int value = -1_000; value <= 100_000; value++)
            {
                var expected = ReferenceIsPrime(value);

                if (Fun.IsPrime(value) != expected)
                    Assert.Fail("Fun.IsPrime(int) disagrees with the reference for {0}.", value);

                if (Fun.IsPrime((long)value) != expected)
                    Assert.Fail("Fun.IsPrime(long) disagrees with the reference for {0}.", value);
            }
        }

        [TestCase(int.MinValue, false)]
        [TestCase(-17, false)]
        [TestCase(0, false)]
        [TestCase(1, false)]
        [TestCase(2, true)]
        [TestCase(3, true)]
        [TestCase(5, true)]
        [TestCase(97, true)]
        [TestCase(49, false)]
        [TestCase(46_337 * 46_337, false)]
        [TestCase(int.MaxValue, true)]
        public void IntOverloadHandlesCuratedCases(int value, bool expected)
        {
            Assert.That(Fun.IsPrime(value), Is.EqualTo(expected));
        }

        [TestCase(long.MinValue, false)]
        [TestCase(-17L, false)]
        [TestCase(0L, false)]
        [TestCase(1L, false)]
        [TestCase(2L, true)]
        [TestCase(3L, true)]
        [TestCase(5L, true)]
        [TestCase(97L, true)]
        [TestCase(49L, false)]
        [TestCase(int.MaxValue, true)]
        [TestCase(32_416_190_071L, true)]
        [TestCase(1_000_006_000_009L, false)]
        [TestCase(long.MaxValue, false)]
        public void LongOverloadHandlesCuratedCases(long value, bool expected)
        {
            Assert.That(Fun.IsPrime(value), Is.EqualTo(expected));
        }
    }
}
