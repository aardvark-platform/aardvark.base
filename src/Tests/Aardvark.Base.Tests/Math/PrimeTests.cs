using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Threading;

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

        [TestCase(long.MinValue, false)]
        [TestCase(-1L, false)]
        [TestCase(0L, false)]
        [TestCase(1L, false)]
        [TestCase(2L, true)]
        [TestCase(311L, true)]
        [TestCase(104_729L, true)]
        [TestCase(104_730L, false)]
        [TestCase(611_953L, true)]
        [TestCase(374_486_474_209L, false)]
        [TestCase(32_416_190_071L, true)]
        [TestCase(1_000_006_000_009L, false)]
        [TestCase(long.MaxValue, false)]
        public void PrimeApiHandlesCuratedValues(long value, bool expected)
        {
            Assert.That(Prime.IsTrueFor(value), Is.EqualTo(expected));
        }

        [Test]
        public void IndexedTableCrossesInitialCapacityBoundaryExactly()
        {
            Assert.That(Prime.WithIndex(62), Is.EqualTo(307));
            Assert.That(Prime.WithIndex(63), Is.EqualTo(311));
            Assert.That(Prime.WithIndex(64), Is.EqualTo(313));

            Assert.That(BitConverter.DoubleToInt64Bits(Prime.InverseWithIndex(63)),
                Is.EqualTo(BitConverter.DoubleToInt64Bits(1.0 / 311.0)));
            Assert.That(BitConverter.DoubleToInt64Bits(Prime.InverseWithIndex(64)),
                Is.EqualTo(BitConverter.DoubleToInt64Bits(1.0 / 313.0)));
        }

        [TestCase(0, 2)]
        [TestCase(1, 3)]
        [TestCase(2, 5)]
        [TestCase(999, 7_919)]
        [TestCase(9_999, 104_729)]
        [TestCase(49_999, 611_953)]
        public void IndexedPrimesAndInversesAreExact(int index, int expectedPrime)
        {
            Assert.That(Prime.WithIndex(index), Is.EqualTo(expectedPrime));
            Assert.That(BitConverter.DoubleToInt64Bits(Prime.InverseWithIndex(index)),
                Is.EqualTo(BitConverter.DoubleToInt64Bits(1.0 / expectedPrime)));
        }

        [Test]
        public void ConcurrentMixedGrowthPublishesCoherentPrimeAndInverseTables()
        {
            const int threadCount = 16;
            const int phaseCount = 12;
            const int phaseWidth = 1024;
            const int firstIndex = 100_000;
            const int lastIndex = firstIndex + phaseCount * phaseWidth - 1;
            var expected = CreateReferencePrimes(lastIndex + 1);
            var errors = new Exception[threadCount];
            var threads = new Thread[threadCount];

            using (var barrier = new Barrier(threadCount))
            {
                for (int ti = 0; ti < threadCount; ti++)
                {
                    int threadIndex = ti;
                    threads[ti] = new Thread(() =>
                    {
                        try
                        {
                            for (int phase = 0; phase < phaseCount; phase++)
                            {
                                if (!barrier.SignalAndWait(TimeSpan.FromSeconds(30)))
                                    throw new TimeoutException("Prime growth barrier timed out.");

                                int index = firstIndex + phase * phaseWidth
                                    + ((threadIndex * 4051) & (phaseWidth - 1));
                                int expectedPrime = expected[index];
                                if (((threadIndex + phase) & 1) == 0)
                                {
                                    int actual = Prime.WithIndex(index);
                                    if (actual != expectedPrime)
                                        throw new InvalidOperationException($"Prime[{index}] was {actual}, expected {expectedPrime}.");
                                }
                                else
                                {
                                    long actual = BitConverter.DoubleToInt64Bits(Prime.InverseWithIndex(index));
                                    long expectedBits = BitConverter.DoubleToInt64Bits(1.0 / expectedPrime);
                                    if (actual != expectedBits)
                                        throw new InvalidOperationException($"Inverse[{index}] did not match prime {expectedPrime}.");
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            errors[threadIndex] = e;
                        }
                    });
                    threads[ti].Start();
                }

                for (int ti = 0; ti < threadCount; ti++)
                    Assert.That(threads[ti].Join(TimeSpan.FromSeconds(45)), Is.True, "Prime growth worker did not finish.");
            }

            for (int ti = 0; ti < threadCount; ti++)
                Assert.That(errors[ti], Is.Null, errors[ti]?.ToString());

            for (int index = firstIndex; index <= lastIndex; index += 257)
            {
                int expectedPrime = expected[index];
                Assert.That(Prime.WithIndex(index), Is.EqualTo(expectedPrime));
                Assert.That(BitConverter.DoubleToInt64Bits(Prime.InverseWithIndex(index)),
                    Is.EqualTo(BitConverter.DoubleToInt64Bits(1.0 / expectedPrime)));
            }
        }

        private static int[] CreateReferencePrimes(int count)
        {
            int limit = count < 6
                ? 15
                : checked((int)Math.Ceiling(count * (Math.Log(count) + Math.Log(Math.Log(count)))) + 16);
            var composite = new bool[limit + 1];

            for (int prime = 2; prime <= limit / prime; prime++)
            {
                if (composite[prime]) continue;
                for (int multiple = prime * prime; multiple <= limit; multiple += prime)
                    composite[multiple] = true;
            }

            var result = new int[count];
            int resultCount = 0;
            for (int value = 2; value <= limit && resultCount < count; value++)
                if (!composite[value]) result[resultCount++] = value;

            if (resultCount != count)
                throw new InvalidOperationException("Independent prime sieve bound was insufficient.");
            return result;
        }
    }
}
