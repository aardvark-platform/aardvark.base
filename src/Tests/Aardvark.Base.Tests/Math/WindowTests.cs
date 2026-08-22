using System;
using System.Collections.Generic;
using System.Linq;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class WindowTests : TestSuite
    {
        public WindowTests() : base() { }
        public WindowTests(TestSuite.Options options) : base(options) { }

        private static readonly double[] s_medianValues =
        {
            7.0, -3.0, double.PositiveInfinity, 7.0, double.NegativeInfinity,
            0.0, -0.0, 5.0, 1.0, 5.0, -2.0, 9.0, 9.0, -0.0, 0.0,
            4.0, -8.0, 6.0, 2.0, 6.0, double.PositiveInfinity, -1.0,
            double.NegativeInfinity, 3.0, 8.0,
        };

        private static double UpperMedian(List<double> values)
        {
            var sorted = values.ToArray();
            Array.Sort(sorted);
            return sorted[sorted.Length >> 1];
        }

        private static void AssertSameBits(double expected, double actual, string message)
            => Assert.That(BitConverter.DoubleToInt64Bits(actual),
                Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)), message);

        private static void VerifyAgainstOracle(MedianWindow window, int capacity, IEnumerable<double> sequence)
        {
            var active = new List<double>(capacity);
            int insertion = 0;
            foreach (double value in sequence)
            {
                if (active.Count == capacity)
                    active.RemoveAt(0);
                active.Add(value);

                double expected = UpperMedian(active);
                double actual = window.Insert(value);
                string message = $"capacity {capacity}, insertion {insertion}";

                Assert.That(actual, Is.EqualTo(expected), message);
                Assert.That(window.Value, Is.EqualTo(expected), message);
                AssertSameBits(value, window.Last, message);
                insertion++;
            }
        }

        private static void AssertSignedZero(double value, bool negative)
        {
            Assert.That(value, Is.EqualTo(0.0));
            Assert.That(BitConverter.DoubleToInt64Bits(value) < 0, Is.EqualTo(negative));
        }

        [Test]
        public void MedianWindowRejectsNonPositiveWindowSize()
        {
            var zero = Assert.Throws<ArgumentOutOfRangeException>(() => new MedianWindow(0));
            var negative = Assert.Throws<ArgumentOutOfRangeException>(() => new MedianWindow(-1));

            Assert.That(zero.ParamName, Is.EqualTo("count"));
            Assert.That(negative.ParamName, Is.EqualTo("count"));
        }

        [Test]
        public void MedianWindowKeepsRollingMedianForValidWindowSize()
        {
            var window = new MedianWindow(3);

            Assert.That(window.Insert(5.0), Is.EqualTo(5.0));
            Assert.That(window.Insert(1.0), Is.EqualTo(5.0));
            Assert.That(window.Insert(9.0), Is.EqualTo(5.0));
            Assert.That(window.Insert(2.0), Is.EqualTo(2.0));

            Assert.That(window.Value, Is.EqualTo(2.0));
            Assert.That(window.Last, Is.EqualTo(2.0));
        }

        [Test]
        public void MedianWindowLastIsZeroWhenEmptyAndAfterReset()
        {
            var window = new MedianWindow(3);

            Assert.That(window.Last, Is.EqualTo(0.0));
            Assert.That(window.Value, Is.EqualTo(0.0));

            window.Insert(-4.0);
            var history = window.History.ToArray();
            window.Reset();

            Assert.That(window.Last, Is.EqualTo(0.0));
            Assert.That(window.Value, Is.EqualTo(0.0));
            CollectionAssert.AreEqual(history, window.History);
        }

        [TestCase(1)]
        [TestCase(4)]
        [TestCase(5)]
        public void MedianWindowMatchesUpperMedianOracleAcrossFillWrapAndReset(int capacity)
        {
            var window = new MedianWindow(capacity);

            VerifyAgainstOracle(window, capacity, s_medianValues);
            var history = window.History.ToArray();

            window.Reset();

            Assert.That(window.Last, Is.EqualTo(0.0));
            Assert.That(window.Value, Is.EqualTo(0.0));
            CollectionAssert.AreEqual(history, window.History);
            VerifyAgainstOracle(window, capacity, s_medianValues.Reverse());
        }

        [Test]
        public void MedianWindowPreservesSignedZeroTieOrder()
        {
            var window = new MedianWindow(4);
            var values = new[] { 0.0, -0.0, 0.0, -0.0, -0.0, 0.0, -0.0 };
            var expectedNegativeMedian = new[] { false, true, true, false, false, false, true };

            for (int i = 0; i < values.Length; i++)
            {
                double median = window.Insert(values[i]);
                AssertSignedZero(median, expectedNegativeMedian[i]);
                AssertSignedZero(window.Value, expectedNegativeMedian[i]);
                AssertSameBits(values[i], window.Last, $"insertion {i}");
            }

            window.Reset();
            AssertSignedZero(window.Insert(-0.0), true);
            AssertSignedZero(window.Insert(0.0), false);
        }
    }
}
