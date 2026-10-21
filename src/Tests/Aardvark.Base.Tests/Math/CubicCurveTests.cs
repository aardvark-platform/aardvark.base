using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

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

        private static IEnumerable<Assembly> BoundsAssemblies()
        {
            yield return typeof(Ipol).Assembly;
            string path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Ipol).Assembly.Location)!,
                "..", "netstandard2.0", "Aardvark.Base.dll"));
            Assert.That(File.Exists(path), Is.True, $"Missing sibling core assembly: {path}");
            yield return Assembly.LoadFile(path);
        }

        private static Func<double, double, V3d[], double, double, Box3d> BindBounds(Assembly assembly, int wrapper = 0)
        {
            var vector = assembly.GetType(typeof(V3d).FullName!, throwOnError: true)!;
            var range = assembly.GetType(typeof(Range1d).FullName!, throwOnError: true)!;
            var owner = assembly.GetType(typeof(Ipol.KochanekBartels).FullName!, throwOnError: true)!;
            var min = Expression.Parameter(typeof(double)); var max = Expression.Parameter(typeof(double));
            var points = Expression.Parameter(typeof(V3d[]));
            var tension = Expression.Parameter(typeof(double)); var bias = Expression.Parameter(typeof(double));
            var ctor = vector.GetConstructor(new[] { typeof(double), typeof(double), typeof(double) })!;
            Expression Point(int i) => Expression.New(ctor, "XYZ".Select(c =>
                Expression.Field(Expression.ArrayIndex(points, Expression.Constant(i)), c.ToString())));
            var args = new List<Expression>();
            if (wrapper == 0) args.Add(Expression.New(range.GetConstructor(new[] { typeof(double), typeof(double) })!, min, max));
            args.AddRange(Enumerable.Range(0, 4).Select(Point));
            if (wrapper != 2) { args.Add(tension); args.Add(bias); }
            var method = owner.GetMethod(nameof(Ipol.KochanekBartels.Bounds), args.Select(a => a.Type).ToArray())!;
            Assert.That(method.ReturnType.Assembly, Is.SameAs(assembly));
            var result = Expression.Variable(method.ReturnType);
            Expression NativeVector(string field) => Expression.New(typeof(V3d).GetConstructor(new[] { typeof(double), typeof(double), typeof(double) })!,
                "XYZ".Select(c => Expression.Field(Expression.Field(result, field), c.ToString())));
            var body = Expression.Block(new[] { result }, Expression.Assign(result, Expression.Call(method, args)),
                Expression.New(typeof(Box3d).GetConstructor(new[] { typeof(V3d), typeof(V3d) })!, NativeVector("Min"), NativeVector("Max")));
            return Expression.Lambda<Func<double, double, V3d[], double, double, Box3d>>(body, min, max, points, tension, bias).Compile();
        }

        private static V3d[] PolynomialControls(V3d cubic, V3d quadratic, V3d linear, V3d constant)
            => new[] { cubic + quadratic - linear + constant, constant,
                cubic + quadratic + linear + constant, 6 * cubic + 4 * quadratic + 2 * linear + constant };

        private static IEnumerable<(string Name, V3d[] Points)> BoundsCurves()
        {
            yield return ("linear reproduction", new[] { V3d.Zero, new V3d(1), new V3d(2), new V3d(3) });
            yield return ("constant", Enumerable.Repeat(new V3d(2, -3, 7), 4).ToArray());
            yield return ("quadratic", PolynomialControls(V3d.Zero, new V3d(1, -1, 2), new V3d(2, -4, -1), new V3d(1, 3, -2)));
            yield return ("cubic", PolynomialControls(new V3d(1, -1, 2), new V3d(-3, 1, -6), new V3d(2, 2, 4), new V3d(1, -2, 3)));
            yield return ("repeated stationary root", PolynomialControls(new V3d(1, -1, 0), new V3d(-4.5, 4.5, 0),
                new V3d(6.75, -6.75, 1), new V3d(-3.375, 3.375, 0)));
            var random = new Random(73031);
            for (int i = 0; i < 12; i++)
                yield return ($"seeded {i}", Enumerable.Range(0, 4).Select(_ =>
                    new V3d(random.Next(-8, 9) * 0.25, random.Next(-8, 9) * 0.25, random.Next(-8, 9) * 0.25)).ToArray());
        }

        private static readonly (double Tension, double Bias)[] s_boundsParameters =
        {
            (0, 0), (0.25, -0.5), (-0.5, 0.75), (0.75, -0.25), (1, 0.5), (-1.25, 1.5)
        };

        private static IEnumerable<Range1d> BoundsDomains(bool extrapolated)
        {
            double[] parameters = { -3, -2, -1, -0.25, 0, Math.BitIncrement(0), 0.125, 0.25, 0.5,
                0.75, 0.875, Math.BitDecrement(1), 1, Math.BitIncrement(1), 1.25, 2, 3 };
            for (int i = 0; i < parameters.Length; i++)
            for (int j = i; j < parameters.Length; j++)
                if ((parameters[i] < 0 || parameters[j] > 1) == extrapolated)
                    yield return new Range1d(parameters[i], parameters[j]);
        }

        private static IEnumerable<double> StationaryRoots(double cubic, double quadratic, double linear)
        {
            double a = 3 * cubic, b = 2 * quadratic, c = linear;
            if (a == 0)
            {
                if (b != 0) yield return -c / b;
                yield break;
            }
            double scale = Math.Max(Math.Abs(a), Math.Max(Math.Abs(b), Math.Abs(c)));
            a /= scale; b /= scale; c /= scale;
            double discriminant = b * b - 4 * a * c;
            if (discriminant < 0) yield break;
            if (discriminant == 0) { yield return -b / (2 * a); yield break; }
            double q = -0.5 * (b + Math.CopySign(Math.Sqrt(discriminant), b));
            yield return q / a;
            yield return c / q;
        }

        private static void CheckBounds(Box3d actual, Range1d domain, V3d[] points, double tension, double bias, string context)
        {
            Assert.That(actual.IsValid, Is.True, context);
            for (int axis = 0; axis < 3; axis++)
            {
                double p0 = points[0][axis], p1 = points[1][axis], p2 = points[2][axis], p3 = points[3][axis];
                // Expand the control-point coefficients independently, not via production tangents/Eval.
                double scale = (1 - tension) / 2;
                double incoming = -scale * (1 + bias) * p0 + 2 * scale * bias * p1 + scale * (1 - bias) * p2;
                double outgoing = -scale * (1 + bias) * p1 + 2 * scale * bias * p2 + scale * (1 - bias) * p3;
                double a = 2 * p1 - 2 * p2 + incoming + outgoing;
                double b = -3 * p1 + 3 * p2 - 2 * incoming - outgoing;
                double c = incoming, d = p1;
                double Eval(double t) => ((a * t + b) * t + c) * t + d;
                var values = new List<double> { Eval(domain.Min), Eval(domain.Max) };
                foreach (double t in StationaryRoots(a, b, c))
                    if (t >= domain.Min && t <= domain.Max) values.Add(Eval(t));
                double expectedMin = values.Min(), expectedMax = values.Max();
                double tolerance = 4e-13 + 4e-12 * Math.Max(Math.Abs(expectedMin), Math.Abs(expectedMax));
                string message = $"{context}, axis {axis}, domain {domain}, tension {tension}, bias {bias}";
                Assert.That(double.IsFinite(actual.Min[axis]) && double.IsFinite(actual.Max[axis]), Is.True, message);
                Assert.That(actual.Min[axis], Is.EqualTo(expectedMin).Within(tolerance), message + " min");
                Assert.That(actual.Max[axis], Is.EqualTo(expectedMax).Within(tolerance), message + " max");
                for (int sample = 0; sample <= 4; sample++)
                {
                    double value = Eval(domain.Min + (domain.Max - domain.Min) * sample / 4);
                    Assert.That(value, Is.InRange(actual.Min[axis] - tolerance, actual.Max[axis] + tolerance), message + $", sample {sample}");
                }
            }
        }

        [Test]
        public void ExtrapolatedSplineBoundsMatchPolynomialExtrema()
        {
            foreach (var assembly in BoundsAssemblies())
            {
                var call = BindBounds(assembly);
                var linear = new[] { V3d.Zero, new V3d(1), new V3d(2), new V3d(3) };
                var regression = call(-1, 2, linear, 0, 0);
                Assert.That(regression.Min, Is.EqualTo(V3d.Zero), "[-1,2] linear minimum");
                Assert.That(regression.Max, Is.EqualTo(new V3d(3)), "[-1,2] linear maximum");
                foreach (var curve in BoundsCurves())
                foreach (var p in s_boundsParameters)
                foreach (var domain in BoundsDomains(true))
                    CheckBounds(call(domain.Min, domain.Max, curve.Points, p.Tension, p.Bias), domain,
                        curve.Points, p.Tension, p.Bias, curve.Name);
            }
        }

        [Test]
        public void SplineBoundsKeepUnitSubintervalAndWrapperBehavior()
        {
            foreach (var assembly in BoundsAssemblies())
            {
                var call = BindBounds(assembly);
                var unit = BindBounds(assembly, 1); var defaults = BindBounds(assembly, 2);
                foreach (var curve in BoundsCurves())
                {
                    Assert.That(defaults(0, 1, curve.Points, 0, 0), Is.EqualTo(call(0, 1, curve.Points, 0, 0)), curve.Name);
                    foreach (var p in s_boundsParameters)
                    {
                        Assert.That(unit(0, 1, curve.Points, p.Tension, p.Bias),
                            Is.EqualTo(call(0, 1, curve.Points, p.Tension, p.Bias)), curve.Name);
                        foreach (var domain in BoundsDomains(false))
                            CheckBounds(call(domain.Min, domain.Max, curve.Points, p.Tension, p.Bias), domain,
                                curve.Points, p.Tension, p.Bias, curve.Name);
                    }
                }
            }
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
