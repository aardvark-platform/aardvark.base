using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        [Test]
        public void MedianWindowMatchesUpperMedianOracleAcrossFillWrapAndReset()
        {
            foreach (int capacity in new[] { 1, 4, 5 })
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
        }

        private static IEnumerable<Assembly> CoreAssemblies()
        {
            yield return typeof(MedianWindow).Assembly;
            string path = Path.GetFullPath(Path.Combine(
                Path.GetDirectoryName(typeof(MedianWindow).Assembly.Location)!,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Missing sibling core assembly: {path}");
            yield return Assembly.LoadFile(path);
        }

        private static void VerifyNaNPhases(Assembly assembly, int capacity, params double[][] phases)
        {
            var type = assembly.GetType(typeof(MedianWindow).FullName!, throwOnError: true)!;
            var window = Activator.CreateInstance(type, capacity)!;
            T Bind<T>(string name) where T : Delegate
                => (T)Delegate.CreateDelegate(typeof(T), window, type.GetMethod(name)!);
            var insert = Bind<Func<double, double>>(nameof(MedianWindow.Insert));
            var median = Bind<Func<double>>("get_Value");
            var last = Bind<Func<double>>("get_Last");
            var reset = Bind<Action>(nameof(MedianWindow.Reset));
            var history = Bind<Func<IReadOnlyList<double>>>("get_History")();
            var expectedHistory = new double[capacity];
            Assert.That(history.Count, Is.EqualTo(capacity));

            for (int phase = 0; phase < phases.Length; phase++)
            {
                string target = assembly == typeof(MedianWindow).Assembly ? "native" : "standard";
                string context = $"{target}, capacity {capacity}, phase {phase}";
                reset();
                AssertSameBits(0.0, median(), context);
                AssertSameBits(0.0, last(), context);
                for (int slot = 0; slot < capacity; slot++)
                    AssertSameBits(expectedHistory[slot], history[slot], $"{context}, reset slot {slot}");

                var active = new List<double>(capacity);
                for (int i = 0; i < phases[phase].Length; i++)
                {
                    double value = phases[phase][i];
                    if (active.Count == capacity) active.RemoveAt(0);
                    active.Add(value);
                    double expected = UpperMedian(active);
                    string message = $"{context}, insertion {i}, value {value:R}, active [{string.Join(",", active)}]";
                    Assert.That(insert(value), Is.EqualTo(expected), message);
                    Assert.That(median(), Is.EqualTo(expected), message);
                    AssertSameBits(value, last(), message);
                    expectedHistory[i % capacity] = value;
                    for (int slot = 0; slot < capacity; slot++)
                        AssertSameBits(expectedHistory[slot], history[slot], $"{message}, slot {slot}");
                }
            }
        }

        [Test]
        public void MedianWindowOrdersNaNsAndRecoversAfterEviction()
        {
            var special = s_medianValues.Concat(new[]
            {
                double.NaN, BitConverter.Int64BitsToDouble(0x7ff8000000000042L),
                BitConverter.Int64BitsToDouble(unchecked((long)0xfff8000000000011UL))
            }).ToArray();
            foreach (var assembly in CoreAssemblies())
            {
                var type = assembly.GetType(typeof(MedianWindow).FullName!, throwOnError: true)!;
                var window = Activator.CreateInstance(type, 3)!;
                var insert = (Func<double, double>)Delegate.CreateDelegate(
                    typeof(Func<double, double>), window, type.GetMethod(nameof(MedianWindow.Insert))!);
                double result = 0.0;
                foreach (double value in new[] { 1.0, double.NaN, 2.0, 3.0, 4.0 }) result = insert(value);
                Assert.That(result, Is.EqualTo(3.0), "capacity 3, final NaN-free window [2,3,4]");
                foreach (int capacity in Enumerable.Range(1, 8))
                {
                    var dense = Enumerable.Range(0, 3 * capacity + 1)
                        .Select(i => special[special.Length - 1 - i % 3]).ToArray();
                    var mixed = Enumerable.Range(0, 8 * special.Length)
                        .Select(i => i % 3 == 0 ? double.NaN : special[i % special.Length]).ToArray();
                    var recovery = Enumerable.Range(1, 2 * capacity).Select(i => (double)i).ToArray();
                    VerifyNaNPhases(assembly, capacity, dense, mixed.Concat(recovery).ToArray(),
                        new[] { double.NaN }, special.Take(capacity - 1).ToArray(), recovery);
                }
            }
        }

        [Test]
        public void MedianWindowNaNsMatchSortAcrossSeededWrapsAndResets()
        {
            var values = s_medianValues.Concat(new[] { double.NaN, double.NaN, double.NaN }).ToArray();
            foreach (var assembly in CoreAssemblies())
            foreach (int capacity in Enumerable.Range(1, 8))
            {
                var random = new Random(71033 + capacity);
                var phases = Enumerable.Range(0, 8).Select(phase =>
                    Enumerable.Range(0, 257 + phase).Select(_ => values[random.Next(values.Length)])
                        .Concat(Enumerable.Range(0, capacity).Select(i => (double)i)).ToArray()).ToArray();
                VerifyNaNPhases(assembly, capacity, phases);
            }
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
