using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Aardvark.Tests
{
    [TestFixture]
    public class FunTests : TestSuite
    {
        public FunTests() : base() { }
        public FunTests(TestSuite.Options options) : base(options) { }

        private sealed class SingleUseEnumerable<T> : IEnumerable<T>
        {
            private readonly T[] m_values;
            private bool m_wasEnumerated;

            public int EnumerationCount { get; private set; }

            public SingleUseEnumerable(params T[] values)
            {
                m_values = values;
            }

            public IEnumerator<T> GetEnumerator()
            {
                if (m_wasEnumerated)
                    throw new InvalidOperationException("The sequence was enumerated more than once.");

                m_wasEnumerated = true;
                EnumerationCount++;
                return ((IEnumerable<T>)m_values).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        private static double ExpectedBinaryEntropy(double positiveWeight, double negativeWeight)
        {
            var total = positiveWeight + negativeWeight;
            var positive = positiveWeight / total;
            var negative = negativeWeight / total;
            return -positive * Math.Log(positive, 2.0) - negative * Math.Log(negative, 2.0);
        }

        [Test]
        public static void AngleDistanceTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 1000; i++)
            {
                var alpha = rnd.UniformDouble() * Constant.PiTimesTwo;
                var delta = (rnd.UniformDouble() - 0.5) * 2.0 * Constant.Pi;
                var beta = alpha + (delta + Constant.PiTimesTwo * rnd.UniformInt(10));

                var dist = alpha.AngleDistance(beta);
                Assert.AreEqual(dist, Fun.Abs(delta), 1e-8);
            }
        }

        [Test]
        public static void AngleDifferenceTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 1000; i++)
            {
                var alpha = rnd.UniformDouble() * Constant.PiTimesTwo * rnd.UniformInt(10);
                var delta = (rnd.UniformDouble() - 0.5) * Constant.Pi;
                if (delta == Constant.Pi)
                    delta = -Constant.Pi;

                var beta = alpha + (delta + Constant.PiTimesTwo * rnd.UniformInt(10));

                var dist = alpha.AngleDifference(beta);
                Assert.AreEqual(dist, delta, 1e-8);
            }
        }

        [Test]
        public void MinMaxVarArg()
        {
            var rnd = new RandomSystem(1);
            var a = rnd.UniformInt();
            var b = rnd.UniformInt();
            var c = rnd.UniformInt();
            var d = rnd.UniformInt();
            var e = rnd.UniformInt();

            var min = Fun.Min(a, b, c, d, e);
            var min_ref = Fun.Min(Fun.Min(Fun.Min(Fun.Min(a, b), c), d), e);
            Assert.AreEqual(min, min_ref, "Min not equal to reference");

            var max = Fun.Max(a, b, c, d, e);
            var max_ref = Fun.Max(Fun.Max(Fun.Max(Fun.Max(a, b), c), d), e);
            Assert.AreEqual(max, max_ref, "Max not equal to reference");
        }

        [Test]
        public void MinMaxVecVarArg()
        {
            var rnd = new RandomSystem(1);
            var a = rnd.UniformV4i();
            var b = rnd.UniformV4i();
            var c = rnd.UniformV4i();
            var d = rnd.UniformV4i();
            var e = rnd.UniformV4i();

            var min = Fun.Min(a, b, c, d, e);
            var min_ref = Fun.Min(Fun.Min(Fun.Min(Fun.Min(a, b), c), d), e);
            Assert.AreEqual(min, min_ref, "Min not equal to reference");

            var max = Fun.Max(a, b, c, d, e);
            var max_ref = Fun.Max(Fun.Max(Fun.Max(Fun.Max(a, b), c), d), e);
            Assert.AreEqual(max, max_ref, "Max not equal to reference");
        }

        [Test]
        public void EntropyEnumeratesInputOnce()
        {
            var values = new SingleUseEnumerable<int>(1, 1, 2, 2);

            Assert.AreEqual(1.0, values.Entropy(), 1e-12);
            Assert.AreEqual(1, values.EnumerationCount);
        }

        [Test]
        public void EntropyHandlesEmptyAllEqualNullAndMixedDistributions()
        {
            Assert.AreEqual(0.0, Array.Empty<int>().Entropy(), 0.0);
            Assert.AreEqual(0.0, new[] { 7, 7, 7 }.Entropy(), 1e-12);
            Assert.AreEqual(1.0, new[] { "a", "a", "b", "b" }.Entropy(), 1e-12);
            Assert.AreEqual(1.5, new string[] { null, null, "x", "y" }.Entropy(), 1e-12);
        }

        [Test]
        public void WeightedBipartiteEntropyNormalizesByTotalWeight()
        {
            var classes = new[] { true, false };

            Assert.AreEqual(1.0, classes.Entropy(new[] { 2.0, 2.0 }), 1e-12);
            Assert.AreEqual(ExpectedBinaryEntropy(3.0, 1.0), classes.Entropy(new[] { 3.0, 1.0 }), 1e-12);
        }

        [Test]
        public void WeightedBipartiteEntropyIsScaleInvariant()
        {
            var classes = new[] { true, false };

            var entropy = classes.Entropy(new[] { 3.0, 1.0 });
            var scaledEntropy = classes.Entropy(new[] { 30.0, 10.0 });

            Assert.AreEqual(entropy, scaledEntropy, 1e-12);
        }

        [Test]
        public void WeightedBipartiteEntropyReturnsZeroForZeroClassTotals()
        {
            Assert.AreEqual(0.0, new[] { true, true }.Entropy(new[] { 2.0, 5.0 }), 0.0);
            Assert.AreEqual(0.0, new[] { false, false }.Entropy(new[] { 2.0, 5.0 }), 0.0);
            Assert.AreEqual(0.0, new[] { true, false }.Entropy(new[] { 0.0, 5.0 }), 0.0);
            Assert.AreEqual(0.0, new[] { true, false }.Entropy(new[] { 5.0, 0.0 }), 0.0);
        }

        private static double NextAfter(double input, int dir)
            => BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(input) + dir);

        private static float NextAfter(float input, int dir)
            => BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits(input) + dir);

        private static float SincFloatImpl(float x)
        {
            return MathF.Sin(ConstantF.Pi * x) / (ConstantF.Pi * x);
        }

        private static double SincDoubleImpl(double x)
        {
            return Math.Sin(Constant.Pi * x) / (Constant.Pi * x);
        }

        private static string FormatBits(float x)
        {
            return $"0x{BitConverter.SingleToInt32Bits(x):X8}";
        }

        private static string FormatBits(double x)
        {
            return $"0x{BitConverter.DoubleToInt64Bits(x):X16}";
        }

        private static float FindFirstFloatSincNotOne(float startInclusive, int maxUlps)
        {
            var x = startInclusive;

            for (var i = 0; i <= maxUlps; i++)
            {
                if (SincFloatImpl(x) != 1.0f)
                    return x;

                x = NextAfter(x, 1);
            }

            Assert.Fail(
                "Could not find float x with raw sinc(x) != 1 within {0} ULPs from x = {1:R} ({2})",
                maxUlps,
                startInclusive,
                FormatBits(startInclusive)
            );

            return default;
        }

        private static double FindFirstDoubleSincNotOne(double startInclusive, int maxUlps)
        {
            var x = startInclusive;

            for (var i = 0; i <= maxUlps; i++)
            {
                if (SincDoubleImpl(x) != 1.0)
                    return x;

                x = NextAfter(x, 1);
            }

            Assert.Fail(
                "Could not find double x with raw sinc(x) != 1 within {0} ULPs from x = {1:R} ({2})",
                maxUlps,
                startInclusive,
                FormatBits(startInclusive)
            );

            return default;
        }

        [Test]
        public void SincTest()
        {
            const int searchUlps = 4096;
            const int verifyUlps = 256;

            // Keep the current implementation cutoffs in sync with Fun.Sinc.
            {
                const float cutoff = 0.00017791694f;
                float[] thresholds = { cutoff, -cutoff };

                foreach (var threshold in thresholds)
                {
                    var insideCutoff = NextAfter(threshold, -1);
                    Assert.AreEqual(
                        1.0f,
                        Fun.Sinc(insideCutoff),
                        "Expected Fun.Sinc(x) = 1 one ULP inside the float cutoff, but got x = {0:R} ({1})",
                        insideCutoff,
                        FormatBits(insideCutoff)
                    );

                    var firstNonOne = FindFirstFloatSincNotOne(threshold, searchUlps);
                    Assert.AreNotEqual(
                        1.0f,
                        Fun.Sinc(firstNonOne),
                        "Expected Fun.Sinc(x) != 1 at or above the float cutoff, but got x = {0:R} ({1})",
                        firstNonOne,
                        FormatBits(firstNonOne)
                    );

                    var x = firstNonOne;
                    for (var i = 0; i < verifyUlps; i++)
                    {
                        Assert.AreNotEqual(
                            1.0f,
                            Fun.Sinc(x),
                            "Expected Fun.Sinc(x) != 1 in the verified float window above the cutoff, but got x = {0:R} ({1})",
                            x,
                            FormatBits(x)
                        );

                        x = NextAfter(x, 1);
                    }
                }
            }

            {
                const double cutoff = 6.840859302478615E-09;
                double[] thresholds = { cutoff, -cutoff };

                foreach (var threshold in thresholds)
                {
                    var insideCutoff = NextAfter(threshold, -1);
                    Assert.AreEqual(
                        1.0,
                        Fun.Sinc(insideCutoff),
                        "Expected Fun.Sinc(x) = 1 one ULP inside the double cutoff, but got x = {0:R} ({1})",
                        insideCutoff,
                        FormatBits(insideCutoff)
                    );

                    var firstNonOne = FindFirstDoubleSincNotOne(threshold, searchUlps);
                    Assert.AreNotEqual(
                        1.0,
                        Fun.Sinc(firstNonOne),
                        "Expected Fun.Sinc(x) != 1 at or above the double cutoff, but got x = {0:R} ({1})",
                        firstNonOne,
                        FormatBits(firstNonOne)
                    );

                    var x = firstNonOne;
                    for (var i = 0; i < verifyUlps; i++)
                    {
                        Assert.AreNotEqual(
                            1.0,
                            Fun.Sinc(x),
                            "Expected Fun.Sinc(x) != 1 in the verified double window above the cutoff, but got x = {0:R} ({1})",
                            x,
                            FormatBits(x)
                        );

                        x = NextAfter(x, 1);
                    }
                }
            }
        }
    }
}
