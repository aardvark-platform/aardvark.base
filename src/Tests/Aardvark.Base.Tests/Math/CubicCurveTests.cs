using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests
{
    [TestFixture]
    public class CubicCurveTests
    {
        private static readonly V2d s_v2p0 = new V2d(-2.0, 0.25);
        private static readonly V2d s_v2p1 = new V2d(0.5, 2.0);
        private static readonly V2d s_v2p2 = new V2d(3.0, -0.75);
        private static readonly V2d s_v2p3 = new V2d(4.5, 1.25);
        private static readonly V3d s_v3p0 = new V3d(-2.0, 1.5, 0.25);
        private static readonly V3d s_v3p1 = new V3d(0.5, -1.0, 2.0);
        private static readonly V3d s_v3p2 = new V3d(3.0, 2.5, -0.75);
        private static readonly V3d s_v3p3 = new V3d(4.5, -2.0, 1.25);

        [Test]
        public void CubicHermiteMatchesAnalyticalPolynomialAndExtrapolation()
        {
            // p(t) = 2t^3 - 3t^2 + 4t - 5, represented by endpoint values and derivatives.
            foreach (double t in new[] { -2.0, -0.5, 0.0, 0.25, 0.75, 1.0, 1.5, 3.0 })
            {
                double t2 = t * t;
                double t3 = t2 * t;
                Assert.That(Ipol.CubicHermite.Eval(t, -5.0, -2.0, 4.0, 4.0),
                    Is.EqualTo(2.0 * t3 - 3.0 * t2 + 4.0 * t - 5.0).Within(1e-12));
                Assert.That(Ipol.CubicHermite.EvalD1(t, -5.0, -2.0, 4.0, 4.0),
                    Is.EqualTo(6.0 * t2 - 6.0 * t + 4.0).Within(1e-12));
                Assert.That(Ipol.CubicHermite.EvalD2(t, -5.0, -2.0, 4.0, 4.0),
                    Is.EqualTo(12.0 * t - 6.0).Within(1e-12));
                Assert.That(Ipol.CubicHermite.EvalD3(t, -5.0, -2.0, 4.0, 4.0),
                    Is.EqualTo(12.0));
            }
        }

        [Test]
        public void CubicHermitePreservesEndpointAndTangentIdentities()
        {
            Assert.That(Ipol.CubicHermite.Eval(0.0, -3.0, 7.0, 11.0, -13.0), Is.EqualTo(-3.0));
            Assert.That(Ipol.CubicHermite.Eval(1.0, -3.0, 7.0, 11.0, -13.0), Is.EqualTo(7.0));
            Assert.That(Ipol.CubicHermite.EvalD1(0.0, -3.0, 7.0, 11.0, -13.0), Is.EqualTo(11.0));
            Assert.That(Ipol.CubicHermite.EvalD1(1.0, -3.0, 7.0, 11.0, -13.0), Is.EqualTo(-13.0));

            Assert.That(Ipol.CubicHermite.Eval(0.0, s_v2p0, s_v2p1, s_v2p2, s_v2p3), Is.EqualTo(s_v2p0));
            Assert.That(Ipol.CubicHermite.Eval(1.0, s_v2p0, s_v2p1, s_v2p2, s_v2p3), Is.EqualTo(s_v2p1));
            Assert.That(Ipol.CubicHermite.EvalD1(0.0, s_v2p0, s_v2p1, s_v2p2, s_v2p3), Is.EqualTo(s_v2p2));
            Assert.That(Ipol.CubicHermite.EvalD1(1.0, s_v2p0, s_v2p1, s_v2p2, s_v2p3), Is.EqualTo(s_v2p3));

            Assert.That(Ipol.CubicHermite.Eval(0.0, s_v3p0, s_v3p1, s_v3p2, s_v3p3), Is.EqualTo(s_v3p0));
            Assert.That(Ipol.CubicHermite.Eval(1.0, s_v3p0, s_v3p1, s_v3p2, s_v3p3), Is.EqualTo(s_v3p1));
            Assert.That(Ipol.CubicHermite.EvalD1(0.0, s_v3p0, s_v3p1, s_v3p2, s_v3p3), Is.EqualTo(s_v3p2));
            Assert.That(Ipol.CubicHermite.EvalD1(1.0, s_v3p0, s_v3p1, s_v3p2, s_v3p3), Is.EqualTo(s_v3p3));
        }

        [Test]
        public void ScalarAndVectorOverloadsHaveComponentParity()
        {
            foreach (double t in new[] { -0.75, 0.0, 0.125, 0.5, 1.0, 1.75 })
            {
                for (int derivative = 0; derivative <= 3; derivative++)
                {
                    V2d actual2 = Hermite(derivative, t, s_v2p0, s_v2p1, s_v2p2, s_v2p3);
                    Assert.That(actual2.X, Is.EqualTo(Hermite(derivative, t,
                        s_v2p0.X, s_v2p1.X, s_v2p2.X, s_v2p3.X)));
                    Assert.That(actual2.Y, Is.EqualTo(Hermite(derivative, t,
                        s_v2p0.Y, s_v2p1.Y, s_v2p2.Y, s_v2p3.Y)));

                    V3d actual3 = Hermite(derivative, t, s_v3p0, s_v3p1, s_v3p2, s_v3p3);
                    Assert.That(actual3.X, Is.EqualTo(Hermite(derivative, t,
                        s_v3p0.X, s_v3p1.X, s_v3p2.X, s_v3p3.X)));
                    Assert.That(actual3.Y, Is.EqualTo(Hermite(derivative, t,
                        s_v3p0.Y, s_v3p1.Y, s_v3p2.Y, s_v3p3.Y)));
                    Assert.That(actual3.Z, Is.EqualTo(Hermite(derivative, t,
                        s_v3p0.Z, s_v3p1.Z, s_v3p2.Z, s_v3p3.Z)));
                }
            }
        }

        [Test]
        public void CatmullRomDelegatesToCubicHermite()
        {
            V3d tangentIn = (s_v3p2 - s_v3p0) * 0.5;
            V3d tangentOut = (s_v3p3 - s_v3p1) * 0.5;

            foreach (double t in new[] { -1.25, 0.0, 0.375, 1.0, 2.25 })
            {
                for (int derivative = 0; derivative <= 3; derivative++)
                {
                    V3d expected = Hermite(derivative, t, s_v3p1, s_v3p2, tangentIn, tangentOut);
                    V3d actual = CatmullRom(derivative, t, s_v3p0, s_v3p1, s_v3p2, s_v3p3);
                    Assert.That(actual, Is.EqualTo(expected), $"derivative {derivative}, t {t}");
                }
            }
        }

        [Test]
        public void KochanekBartelsDelegatesToCubicHermite()
        {
            const double tension = 0.35;
            const double bias = -0.2;
            double scale = (1.0 - tension) * 0.5;
            double x1 = scale * (1.0 + bias);
            double x2 = scale * (1.0 - bias);
            V3d tangentIn = x1 * (s_v3p1 - s_v3p0) + x2 * (s_v3p2 - s_v3p1);
            V3d tangentOut = x1 * (s_v3p2 - s_v3p1) + x2 * (s_v3p3 - s_v3p2);

            foreach (double t in new[] { -1.25, 0.0, 0.375, 1.0, 2.25 })
            {
                for (int derivative = 0; derivative <= 3; derivative++)
                {
                    V3d expected = Hermite(derivative, t, s_v3p1, s_v3p2, tangentIn, tangentOut);
                    V3d actual = KochanekBartels(derivative, t,
                        s_v3p0, s_v3p1, s_v3p2, s_v3p3, tension, bias);
                    Assert.That(actual, Is.EqualTo(expected), $"derivative {derivative}, t {t}");
                }
            }
        }

        [Test]
        public void ThirdDerivativeIsIndependentOfParameter()
        {
            double expectedScalar = Ipol.CubicHermite.EvalD3(double.NegativeInfinity, -2.0, 3.0, 5.0, 7.0);
            V2d expected2 = Ipol.CubicHermite.EvalD3(double.NaN, s_v2p0, s_v2p1, s_v2p2, s_v2p3);
            V3d expected3 = Ipol.CubicHermite.EvalD3(-1e300, s_v3p0, s_v3p1, s_v3p2, s_v3p3);

            foreach (double t in new[] { double.NegativeInfinity, -1e300, -1.0, 0.0, 1.0, 1e300, double.PositiveInfinity, double.NaN })
            {
                Assert.That(Ipol.CubicHermite.EvalD3(t, -2.0, 3.0, 5.0, 7.0), Is.EqualTo(expectedScalar));
                Assert.That(Ipol.CubicHermite.EvalD3(t, s_v2p0, s_v2p1, s_v2p2, s_v2p3), Is.EqualTo(expected2));
                Assert.That(Ipol.CubicHermite.EvalD3(t, s_v3p0, s_v3p1, s_v3p2, s_v3p3), Is.EqualTo(expected3));
            }
        }

        [Test]
        public void CubicHermitePreservesLegacyArithmeticOrdering()
        {
            foreach (double t in new[] { -1e100, -2.25, -0.0, 1e-150, 0.375, 1.0, 3.5, 1e100 })
            {
                for (int derivative = 0; derivative <= 3; derivative++)
                {
                    AssertBitsEqual(
                        LegacyHermite(derivative, t, -2.0, 3.0, 5.0, 7.0),
                        Hermite(derivative, t, -2.0, 3.0, 5.0, 7.0));

                    V3d actual = Hermite(derivative, t, s_v3p0, s_v3p1, s_v3p2, s_v3p3);
                    AssertBitsEqual(LegacyHermite(derivative, t,
                        s_v3p0.X, s_v3p1.X, s_v3p2.X, s_v3p3.X), actual.X);
                    AssertBitsEqual(LegacyHermite(derivative, t,
                        s_v3p0.Y, s_v3p1.Y, s_v3p2.Y, s_v3p3.Y), actual.Y);
                    AssertBitsEqual(LegacyHermite(derivative, t,
                        s_v3p0.Z, s_v3p1.Z, s_v3p2.Z, s_v3p3.Z), actual.Z);
                }
            }
        }

        [Test]
        public void WarmedPublicEvaluationPathsAllocateNoManagedMemory()
        {
            double warmup = ExercisePublicPaths(10_000);
            long before = GC.GetAllocatedBytesForCurrentThread();
            double checksum = ExercisePublicPaths(100_000);
            long allocated = GC.GetAllocatedBytesForCurrentThread() - before;

            Assert.That(double.IsFinite(warmup), Is.True);
            Assert.That(double.IsFinite(checksum), Is.True);
            Assert.That(allocated, Is.Zero);
        }

        private static double Hermite(int derivative, double t, double a, double b, double tangentIn, double tangentOut)
            => derivative switch
            {
                0 => Ipol.CubicHermite.Eval(t, a, b, tangentIn, tangentOut),
                1 => Ipol.CubicHermite.EvalD1(t, a, b, tangentIn, tangentOut),
                2 => Ipol.CubicHermite.EvalD2(t, a, b, tangentIn, tangentOut),
                _ => Ipol.CubicHermite.EvalD3(t, a, b, tangentIn, tangentOut),
            };

        private static V2d Hermite(int derivative, double t, V2d a, V2d b, V2d tangentIn, V2d tangentOut)
            => derivative switch
            {
                0 => Ipol.CubicHermite.Eval(t, a, b, tangentIn, tangentOut),
                1 => Ipol.CubicHermite.EvalD1(t, a, b, tangentIn, tangentOut),
                2 => Ipol.CubicHermite.EvalD2(t, a, b, tangentIn, tangentOut),
                _ => Ipol.CubicHermite.EvalD3(t, a, b, tangentIn, tangentOut),
            };

        private static V3d Hermite(int derivative, double t, V3d a, V3d b, V3d tangentIn, V3d tangentOut)
            => derivative switch
            {
                0 => Ipol.CubicHermite.Eval(t, a, b, tangentIn, tangentOut),
                1 => Ipol.CubicHermite.EvalD1(t, a, b, tangentIn, tangentOut),
                2 => Ipol.CubicHermite.EvalD2(t, a, b, tangentIn, tangentOut),
                _ => Ipol.CubicHermite.EvalD3(t, a, b, tangentIn, tangentOut),
            };

        private static V3d CatmullRom(int derivative, double t, V3d p0, V3d p1, V3d p2, V3d p3)
            => derivative switch
            {
                0 => Ipol.CatmullRom.Eval(t, p0, p1, p2, p3),
                1 => Ipol.CatmullRom.EvalD1(t, p0, p1, p2, p3),
                2 => Ipol.CatmullRom.EvalD2(t, p0, p1, p2, p3),
                _ => Ipol.CatmullRom.EvalD3(t, p0, p1, p2, p3),
            };

        private static V3d KochanekBartels(
            int derivative, double t, V3d p0, V3d p1, V3d p2, V3d p3, double tension, double bias)
            => derivative switch
            {
                0 => Ipol.KochanekBartels.Eval(t, p0, p1, p2, p3, tension, bias),
                1 => Ipol.KochanekBartels.EvalD1(t, p0, p1, p2, p3, tension, bias),
                2 => Ipol.KochanekBartels.EvalD2(t, p0, p1, p2, p3, tension, bias),
                _ => Ipol.KochanekBartels.EvalD3(t, p0, p1, p2, p3, tension, bias),
            };

        private static double LegacyHermite(
            int derivative, double t, double a, double b, double tangentIn, double tangentOut)
        {
            double[] weights;
            if (derivative == 0)
            {
                var tt = t * t; var ttt = tt * t;
                var tt3 = tt * 3; var ttt2 = ttt * 2;
                weights = new[] { ttt2 - tt3 + 1, ttt - 2 * tt + t, ttt - tt, -ttt2 + tt3 };
            }
            else if (derivative == 1)
            {
                var tt = t * t;
                var tt6 = tt * 6; var tt3 = tt * 3; var t6 = t * 6;
                weights = new[] { tt6 - t6, tt3 - 4 * t + 1, tt3 - 2 * t, -tt6 + t6 };
            }
            else if (derivative == 2)
            {
                var t12 = t * 12; var t6 = t * 6;
                weights = new[] { t12 - 6, t6 - 4, t6 - 2, -t12 + 6 };
            }
            else
            {
                weights = new[] { 12.0, 6.0, 6.0, -12.0 };
            }

            return weights[0] * a + weights[1] * tangentIn + weights[2] * tangentOut + weights[3] * b;
        }

        private static void AssertBitsEqual(double expected, double actual)
        {
            Assert.That(BitConverter.DoubleToInt64Bits(actual),
                Is.EqualTo(BitConverter.DoubleToInt64Bits(expected)));
        }

        private static double ExercisePublicPaths(int count)
        {
            double scalar = 0.0;
            V2d vector2 = V2d.Zero;
            V3d vector3 = V3d.Zero;
            for (int i = 0; i < count; i++)
            {
                double t = (i & 1023) * (1.0 / 1023.0);
                scalar += Ipol.CubicHermite.Eval(t, -2.0, 3.0, 5.0, 7.0);
                scalar += Ipol.CubicHermite.EvalD1(t, -2.0, 3.0, 5.0, 7.0);
                scalar += Ipol.CubicHermite.EvalD2(t, -2.0, 3.0, 5.0, 7.0);
                scalar += Ipol.CubicHermite.EvalD3(t, -2.0, 3.0, 5.0, 7.0);
                vector2 += Ipol.CubicHermite.Eval(t, s_v2p0, s_v2p1, s_v2p2, s_v2p3);
                vector2 += Ipol.CubicHermite.EvalD2(t, s_v2p0, s_v2p1, s_v2p2, s_v2p3);
                vector3 += Ipol.CubicHermite.EvalD1(t, s_v3p0, s_v3p1, s_v3p2, s_v3p3);
                vector3 += Ipol.CubicHermite.EvalD3(t, s_v3p0, s_v3p1, s_v3p2, s_v3p3);
                vector3 += Ipol.CatmullRom.Eval(t, s_v3p0, s_v3p1, s_v3p2, s_v3p3);
                vector3 += Ipol.KochanekBartels.Eval(t,
                    s_v3p0, s_v3p1, s_v3p2, s_v3p3, 0.35, -0.2);
            }

            return scalar + vector2.X + vector2.Y + vector3.X + vector3.Y + vector3.Z;
        }
    }
}
