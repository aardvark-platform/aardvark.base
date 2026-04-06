using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class FunTests : TestSuite
    {
        public FunTests() : base() { }
        public FunTests(TestSuite.Options options) : base(options) { }

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
