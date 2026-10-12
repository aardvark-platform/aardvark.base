using System;
using System.Collections.Generic;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class RandomGaussianTests
    {
        private sealed class ScriptedRandom : Random
        {
            private readonly double[] values;
            private int index;
            public int DrawCount { get; private set; }
            public int ThrowAt = -1;
            public Exception Failure;

            public ScriptedRandom(double[] values) => this.values = values;

            public override double NextDouble()
            {
                int draw = DrawCount++;
                if (draw == ThrowAt) throw Failure;
                if (index == values.Length) throw new InvalidOperationException("Unexpected uniform draw.");
                return values[index++];
            }
        }

        private static RandomGaussian Gaussian(ScriptedRandom source)
            => new RandomGaussian(new RandomSystem(0) { Generator = source });

        private static (double First, double Second) Expected(double u, double v)
        {
            double x = 2.0 * u - 1.0, y = 2.0 * v - 1.0;
            double radius = x * x + y * y;
            double scale = Math.Sqrt(-2.0 * Math.Log(radius) / radius);
            return (x * scale, y * scale);
        }

        private static void AssertExact(double expected, double actual, string context)
            => Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context);

        [Test]
        public void CenterPairsAreRetriedBeforeReturningAndCachingSamples()
        {
            foreach (int centers in new[] { 1, 2, 8 })
            foreach (var pair in new[] { (0.75, 0.625), (0.25, 0.375), (0.5, 0.75), (0.75, 0.5) })
            {
                string context = $"centers={centers}, accepted={pair}";
                var values = new List<double>();
                for (int i = 0; i < centers; i++) values.AddRange(new[] { 0.5, 0.5 });
                values.AddRange(new[] { pair.Item1, pair.Item2, 0.5, 0.5, 0.625, 0.875 });
                var source = new ScriptedRandom(values.ToArray());
                var gaussian = Gaussian(source);
                var expected = Expected(pair.Item1, pair.Item2);
                AssertExact(expected.First, gaussian.GetDouble(), context + ", first");
                Assert.AreEqual(2 * centers + 2, source.DrawCount, context);
                AssertExact(expected.Second, gaussian.GetDouble(), context + ", cached");
                Assert.AreEqual(2 * centers + 2, source.DrawCount, context);
                expected = Expected(0.625, 0.875);
                AssertExact(expected.First, gaussian.GetDouble(), context + ", next pair");
                Assert.AreEqual(2 * centers + 6, source.DrawCount, context);
                AssertExact(expected.Second, gaussian.GetDouble(), context + ", next cached");
                Assert.AreEqual(2 * centers + 6, source.DrawCount, context);
            }
        }

        [Test]
        public void OrdinarySamplesRetainRejectionFormulaCacheAndScaling()
        {
            foreach (var rejected in new[] { Array.Empty<double>(), new[] { 0.5, 0.0 }, new[] { 0.0, 0.5, 0.0, 0.0, 0.125, 0.0625 } })
            foreach (var pair in new[] { (0.75, 0.625), (0.25, 0.375), (0.5, 0.75), (0.75, 0.5), (0.5000000000000001, 0.5), (0.0000000000000001, 0.5) })
            foreach (var distribution in new[] { (0.0, 1.0), (-3.0, 0.5), (2.0, 0.0), (7.0, -2.0) })
            {
                string context = $"rejected=[{string.Join(",", rejected)}], accepted={pair}, mean/sigma={distribution}";
                var values = new List<double>(rejected) { pair.Item1, pair.Item2 };
                var source = new ScriptedRandom(values.ToArray());
                var gaussian = Gaussian(source);
                var expected = Expected(pair.Item1, pair.Item2);
                for (int sample = 0; sample < 2; sample++)
                {
                    double raw = sample == 0 ? expected.First : expected.Second;
                    double actual = distribution == (0.0, 1.0) ? gaussian.GetDouble() : gaussian.GetDouble(distribution.Item1, distribution.Item2);
                    AssertExact(distribution == (0.0, 1.0) ? raw : raw * distribution.Item2 + distribution.Item1, actual, context + $", sample={sample}");
                    Assert.AreEqual(rejected.Length + 2, source.DrawCount, context + $", sample={sample}");
                }
            }
        }

        [Test]
        public void SourceFailuresPropagateWithoutPublishingACachedSample()
        {
            foreach (var rejected in new[] { Array.Empty<double>(), new[] { 0.0, 0.0 } })
            foreach (bool secondDraw in new[] { false, true })
            {
                string context = $"rejected draws={rejected.Length}, fail on second draw={secondDraw}";
                var values = new List<double>(rejected);
                if (secondDraw) values.Add(0.25);
                values.AddRange(new[] { 0.75, 0.625 });
                var failure = new InvalidOperationException("uniform failure");
                int failAt = rejected.Length + (secondDraw ? 1 : 0);
                var source = new ScriptedRandom(values.ToArray()) { ThrowAt = failAt, Failure = failure };
                var gaussian = Gaussian(source);
                Assert.AreSame(failure, Assert.Throws<InvalidOperationException>(() => gaussian.GetDouble(), context), context);
                Assert.AreEqual(failAt + 1, source.DrawCount, context);
                var expected = Expected(0.75, 0.625);
                AssertExact(expected.First, gaussian.GetDouble(), context + ", recovered");
                Assert.AreEqual(failAt + 3, source.DrawCount, context);
                AssertExact(expected.Second, gaussian.GetDouble(), context + ", cached");
                Assert.AreEqual(failAt + 3, source.DrawCount, context);
            }
        }
    }
}
