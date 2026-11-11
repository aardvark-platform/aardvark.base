using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class TriangleCircumCircleTests
    {
        private static readonly int[][] Orders = { new[] { 0, 1, 2 }, new[] { 0, 2, 1 }, new[] { 1, 0, 2 }, new[] { 1, 2, 0 }, new[] { 2, 0, 1 }, new[] { 2, 1, 0 } };
        private static readonly V2d[] Shifts = { V2d.Zero, new V2d(-8, 4), new V2d(16, -8) };
        private static IEnumerable<(V2d U, V2d V)> Frames(bool axesOnly)
        {
            foreach (var u in axesOnly ? new[] { V2d.XAxis } : new[] { V2d.XAxis, new V2d(3, 4) })
            foreach (var handedness in new[] { -1, 1 })
            {
                var a = u; var b = handedness * new V2d(-u.Y, u.X);
                for (int turn = 0; turn < 4; turn++)
                {
                    yield return (a, b);
                    a = new V2d(-a.Y, a.X); b = new V2d(-b.Y, b.X);
                }
            }
        }

        [Test]
        public void SmallRightTrianglesKeepTheirCircumcircles()
        {
            foreach (var core in Cores())
            {
                foreach (int power in core.Single ? new[] { -60, -40, -24, -22, -21, -20, -12, 0 } : new[] { -400, -100, -54, -51, -50, -49, -20, 0 })
                foreach (var frame in Frames(false))
                foreach (var shift in Shifts)
                    Right(core, Math.ScaleB(1, power), frame.U, frame.V, shift, "small right/power=" + power);
                double cutoff = Math.ScaleB(1, core.Single ? -21 : -50);
                foreach (var size in new[] { core.Previous(cutoff), cutoff, core.Next(cutoff) })
                foreach (var frame in Frames(true))
                foreach (var shift in Shifts)
                    Right(core, size, frame.U, frame.V, shift, "horizontal cutoff neighbor");
            }
        }
        private static void Right(CircleCore core, double size, V2d u, V2d v, V2d shift, string label)
        {
            V2d Map(V2d p) => core.Round(size * ((p.X + shift.X) * u + (p.Y + shift.Y) * v));
            var a = Map(V2d.Zero); var b = Map(V2d.XAxis); var c = Map(V2d.YAxis);
            var ab = b - a; var ac = c - a;
            Assert.AreEqual(BigInteger.Zero, Exact(ab.X) * Exact(ac.X) + Exact(ab.Y) * Exact(ac.Y), label + " exact orthogonal setup");
            // The midpoint of a right triangle's hypotenuse is its circumcenter.
            var center = (b + c) * .5; var diameter = b - c;
            double squared = (diameter.X * diameter.X + diameter.Y * diameter.Y) * .25;
            core.Permutations(a, b, c, center, squared, label);
        }

        [Test]
        public void ThinTrianglesKeepDistinctBisectorsAndOrientations()
        {
            foreach (var core in Cores())
            {
                double cutoff = Math.ScaleB(1, core.Single ? -16 : -45);
                foreach (double width in new[] { Math.ScaleB(1, core.Single ? -20 : -48), cutoff * .5, core.Previous(cutoff), cutoff, core.Next(cutoff) })
                foreach (var frame in Frames(true))
                foreach (double scale in new[] { .5, 1.0, 4.0 })
                foreach (var shift in new[] { V2d.Zero, new V2d(-8, -4), new V2d(4, 8) })
                {
                    V2d Map(double x, double y) => core.Round(scale * ((x + shift.X * width) * frame.U + (y + shift.Y * 64) * frame.V));
                    var a = Map(0, 0); var b = Map(width, 64); var c = Map(0, 128);
                    V2d Local(V2d p) => new V2d(p.X * frame.U.X + p.Y * frame.U.Y, p.X * frame.V.X + p.Y * frame.V.Y);
                    var la = Local(a); var lb = Local(b); var lc = Local(c);
                    double actualWidth = lb.X - la.X, halfHeight = lb.Y - la.Y;
                    Assert.AreEqual(la.X, lc.X, "thin setup vertical base");
                    Assert.AreEqual(2 * halfHeight, lc.Y - la.Y, "thin setup equal heights");
                    Assert.That(actualWidth, Is.GreaterThan(0), "thin input must not collapse after translation");
                    // Equating distances to the two vertical endpoints fixes Y;
                    // the remaining vertex gives X without bisector slopes.
                    double offsetX = actualWidth * .5 - halfHeight * halfHeight / (2 * actualWidth);
                    var localCenter = new V2d(la.X + offsetX, la.Y + halfHeight);
                    var center = localCenter.X * frame.U + localCenter.Y * frame.V;
                    core.Permutations(a, b, c, center, offsetX * offsetX + halfHeight * halfHeight,
                        $"thin width={width:R}, scale={scale:R}, U={frame.U}, V={frame.V}, shift={shift}");
                }
            }
        }

        [Test]
        public void OrdinaryAndDegenerateCircumcircleContractsRemain()
        {
            var shapes = new[]
            {
                (A: V2d.Zero, B: new V2d(2, 0), C: new V2d(0, 2), Center: new V2d(1, 1), Squared: 2.0),
                (A: V2d.Zero, B: new V2d(4, 0), C: new V2d(2, 3), Center: new V2d(2, 5.0/6), Squared: 169.0/36),
                (A: V2d.Zero, B: new V2d(4, 0), C: new V2d(1, 1), Center: new V2d(2, -1), Squared: 5.0)
            };
            foreach (var core in Cores())
            foreach (var frame in Frames(false))
            foreach (double scale in new[] { .25, 1.0, 4.0 })
            foreach (var shift in Shifts)
            {
                V2d Map(V2d p) => core.Round(scale * ((p.X + shift.X) * frame.U + (p.Y + shift.Y) * frame.V));
                double length2 = frame.U.X * frame.U.X + frame.U.Y * frame.U.Y;
                foreach (var shape in shapes)
                    core.Permutations(Map(shape.A), Map(shape.B), Map(shape.C), scale * ((shape.Center.X + shift.X) * frame.U + (shape.Center.Y + shift.Y) * frame.V), shape.Squared * scale * scale * length2, "ordinary acute/right/obtuse", true);
                foreach (var points in new[]
                {
                    new[] { V2d.Zero, V2d.XAxis, 2 * V2d.XAxis },
                    new[] { V2d.Zero, V2d.YAxis, 2 * V2d.YAxis },
                    new[] { V2d.Zero, V2d.One, 2 * V2d.One },
                    new[] { V2d.One, V2d.One, V2d.One }
                }) core.Permutations(Map(points[0]), Map(points[1]), Map(points[2]), V2d.NaN, -1, "collinear/coincident failure contract", true);
            }
        }

        // Exact IEEE dyadics, in units of 2^-1074, keep equidistance residuals
        // independent of production radius arithmetic, even for very thin inputs.
        private static BigInteger Exact(double value)
        {
            long bits = BitConverter.DoubleToInt64Bits(value);
            int exponent = (int)((bits >> 52) & 2047);
            ulong fraction = (ulong)bits & 0x000fffffffffffffUL;
            var integer = new BigInteger(fraction);
            if (exponent != 0) integer = (integer + (BigInteger.One << 52)) << (exponent - 1);
            return bits < 0 ? -integer : integer;
        }
        private sealed class CircleCore
        {
            public readonly bool Single;
            private readonly string name;
            private readonly Type vector, triangle, scalar, circle;
            private readonly ConstructorInfo vectorConstructor, triangleConstructor;
            private readonly MethodInfo compute, ordinary, squared;
            private readonly FieldInfo x, y, center, radius;
            public CircleCore(Assembly assembly, bool single, string target)
            {
                Single = single; name = target + (single ? "/float" : "/double"); scalar = single ? typeof(float) : typeof(double);
                string suffix = single ? "f" : "d";
                vector = assembly.GetType("Aardvark.Base.V2" + suffix, true); triangle = assembly.GetType("Aardvark.Base.Triangle2" + suffix, true); circle = assembly.GetType("Aardvark.Base.Circle2" + suffix, true);
                foreach (var type in new[] { vector, triangle, circle }) Assert.AreSame(assembly, type.Assembly, name);
                vectorConstructor = vector.GetConstructor(new[] { scalar, scalar }); triangleConstructor = triangle.GetConstructor(new[] { vector, vector, vector });
                foreach (var constructor in new[] { vectorConstructor, triangleConstructor }) Assert.AreSame(assembly, constructor.DeclaringType.Assembly, name);
                x = vector.GetField("X"); y = vector.GetField("Y"); center = circle.GetField("Center"); radius = circle.GetField("Radius");
                foreach (var field in new[] { x, y, center, radius }) Assert.AreSame(assembly, field.DeclaringType.Assembly, name);
                Assert.AreEqual(vector, center.FieldType, name); Assert.AreEqual(scalar, radius.FieldType, name);
                compute = triangle.GetMethod("ComputeCircumCircleSquared", new[] { vector, vector, vector, vector.MakeByRefType(), scalar.MakeByRefType() });
                Assert.IsTrue(compute.IsStatic && compute.GetParameters()[3].IsOut && compute.GetParameters()[4].IsOut, name);
                Assert.AreEqual(typeof(void), compute.ReturnType, name); Assert.AreSame(assembly, compute.DeclaringType.Assembly, name);
                ordinary = triangle.GetProperty("CircumCircle").GetMethod; squared = triangle.GetProperty("CircumCircleSquared").GetMethod;
                foreach (var method in new[] { ordinary, squared }) { Assert.AreSame(assembly, method.DeclaringType.Assembly, name); Assert.AreEqual(circle, method.ReturnType, name); }
            }
            private object Box(double value) => Single ? (object)(float)value : value;
            public double R(double value) => Single ? (float)value : value;
            public double Previous(double value) => Single ? MathF.BitDecrement((float)value) : Math.BitDecrement(value);
            public double Next(double value) => Single ? MathF.BitIncrement((float)value) : Math.BitIncrement(value);
            public V2d Round(V2d value) => new V2d(R(value.X), R(value.Y));
            private object Vector(V2d p) => vectorConstructor.Invoke(new[] { Box(p.X), Box(p.Y) });
            private V2d Read(object p) => new V2d(Convert.ToDouble(x.GetValue(p)), Convert.ToDouble(y.GetValue(p)));
            private (V2d Center, double Squared) Legacy(V2d a, V2d b, V2d c)
            {
                double y01 = Math.Abs(R(a.Y - b.Y)), y12 = Math.Abs(R(b.Y - c.Y));
                double tiny = Math.ScaleB(1, Single ? -21 : -50);
                if (y01 < tiny && y12 < tiny) return (V2d.NaN, -1);
                double mx1 = R(.5 * R(a.X + b.X)), my1 = R(.5 * R(a.Y + b.Y));
                double mx2 = R(.5 * R(b.X + c.X)), my2 = R(.5 * R(b.Y + c.Y));
                double cx, cy;
                if (y01 < tiny)
                {
                    double m2 = R(R(b.X - c.X) / R(c.Y - b.Y));
                    cx = R(.5 * R(b.X + a.X)); cy = R(R(m2 * R(cx - mx2)) + my2);
                }
                else if (y12 < tiny)
                {
                    double m1 = R(R(a.X - b.X) / R(b.Y - a.Y));
                    cx = R(.5 * R(c.X + b.X)); cy = R(R(m1 * R(cx - mx1)) + my1);
                }
                else
                {
                    double m1 = R(R(a.X - b.X) / R(b.Y - a.Y)), m2 = R(R(b.X - c.X) / R(c.Y - b.Y));
                    double difference = R(m1 - m2);
                    if (Math.Abs(difference) < tiny) return (V2d.NaN, -1);
                    cx = R(R(R(R(R(m1 * mx1) - R(m2 * mx2)) + my2) - my1) / difference);
                    cy = y01 > y12 ? R(R(m1 * R(cx - mx1)) + my1) : R(R(m2 * R(cx - mx2)) + my2);
                }
                double dx = R(a.X - cx), dy = R(a.Y - cy);
                return (new V2d(cx, cy), R(R(dx * dx) + R(dy * dy)));
            }
            private double Budget(double value)
            {
                value = R(value);
                double up = Math.Abs(Next(value) - value), down = Math.Abs(value - Previous(value));
                return 16 * Math.Max(up, down);
            }
            private void Bits(double a, double b, string context) => Assert.AreEqual(BitConverter.DoubleToInt64Bits(a), BitConverter.DoubleToInt64Bits(b), context);
            private void Near(double expected, double actual, double bound, string context)
            {
                Assert.IsTrue(double.IsFinite(actual), context + " finite result");
                Assert.That(Math.Abs(actual - expected), Is.LessThanOrEqualTo(bound), context + $", expected={expected:R}, actual={actual:R}, bound={bound:R}");
            }
            public void Permutations(V2d a, V2d b, V2d c, V2d expectedCenter, double expectedSquared, string label, bool control = false)
            {
                var points = new[] { a, b, c };
                foreach (var order in Orders) Check(points[order[0]], points[order[1]], points[order[2]], expectedCenter, expectedSquared, label + ", order=" + string.Join(",", order), control);
            }
            public void Check(V2d a, V2d b, V2d c, V2d expectedCenter, double expectedSquared, string label, bool control = false)
            {
                string context = $"{name}: {label}; A={a}, B={b}, C={c}";
                object[] args = { Vector(a), Vector(b), Vector(c), Vector(new V2d(123, -456)), Box(789) };
                compute.Invoke(null, args); var actual = Read(args[3]); double r2 = Convert.ToDouble(args[4]);
                var tri = triangleConstructor.Invoke(new[] { args[0], args[1], args[2] });
                var cc = ordinary.Invoke(tri, null); var cs = squared.Invoke(tri, null);
                var oc = Read(center.GetValue(cc)); var sc = Read(center.GetValue(cs));
                double or = Convert.ToDouble(radius.GetValue(cc)), sr = Convert.ToDouble(radius.GetValue(cs));
                if (expectedSquared < 0)
                {
                    Assert.IsTrue(double.IsNaN(actual.X) && double.IsNaN(actual.Y) && double.IsNaN(oc.X) && double.IsNaN(oc.Y) && double.IsNaN(sc.X) && double.IsNaN(sc.Y), context + " invalid center");
                    Assert.AreEqual(-1, r2, context + " static failure squared radius"); Assert.AreEqual(-1, sr, context + " squared circle failure radius"); Assert.IsTrue(double.IsNaN(or), context + " ordinary circle failure radius");
                    return;
                }
                // Ordinary translated controls retain the old absolute-coordinate
                // arithmetic, including cancellation near a center coordinate of
                // zero. Freeze every result bit as well as checking geometry with
                // an input-scale roundoff allowance. Repaired fixtures keep their
                // unchanged component-specific 16-ULP budgets.
                double inheritedRoundoff = control ? 8 * Math.ScaleB(1, Single ? -23 : -52) * (Math.Abs(a.X) + Math.Abs(a.Y) + Math.Abs(b.X) + Math.Abs(b.Y) + Math.Abs(c.X) + Math.Abs(c.Y)) : 0;
                if (control)
                {
                    var legacy = Legacy(a, b, c);
                    Bits(legacy.Center.X, actual.X, context + " legacy center X"); Bits(legacy.Center.Y, actual.Y, context + " legacy center Y"); Bits(legacy.Squared, r2, context + " legacy squared radius");
                }
                double bx = Budget(expectedCenter.X) + inheritedRoundoff, by = Budget(expectedCenter.Y) + inheritedRoundoff;
                double radiusRoundoff = 2 * (Math.Abs(expectedCenter.X - a.X) * bx + Math.Abs(expectedCenter.Y - a.Y) * by) + bx * bx + by * by;
                Near(expectedCenter.X, actual.X, bx, context + " analytic center X"); Near(expectedCenter.Y, actual.Y, by, context + " analytic center Y");
                Near(expectedSquared, r2, Budget(expectedSquared) + radiusRoundoff, context + " analytic squared radius"); Near(Math.Sqrt(expectedSquared), or, Budget(Math.Sqrt(expectedSquared)) + radiusRoundoff / Math.Sqrt(expectedSquared), context + " analytic radius");
                Bits(actual.X, oc.X, context + " ordinary forwarder X"); Bits(actual.Y, oc.Y, context + " ordinary forwarder Y");
                Bits(actual.X, sc.X, context + " squared forwarder X"); Bits(actual.Y, sc.Y, context + " squared forwarder Y"); Bits(r2, sr, context + " squared-radius convention");
                double dx = R(a.X - actual.X), dy = R(a.Y - actual.Y);
                Bits(R(R(dx * dx) + R(dy * dy)), r2, context + " radius squared from returned center");
                Bits(R(Math.Sqrt(r2)), or, context + " ordinary square-root convention");
                // Equal squared distances reduce to two linear equations. Use
                // exact integer residuals and component-specific center ULPs.
                foreach (var p in new[] { b, c })
                {
                    BigInteger ex = Exact(p.X) - Exact(a.X), ey = Exact(p.Y) - Exact(a.Y);
                    BigInteger cx = Exact(actual.X) - Exact(a.X), cy = Exact(actual.Y) - Exact(a.Y);
                    BigInteger residual = BigInteger.Abs(2 * (ex * cx + ey * cy) - ex * ex - ey * ey);
                    BigInteger bound = 2 * (BigInteger.Abs(ex) * Exact(bx) + BigInteger.Abs(ey) * Exact(by));
                    Assert.That(residual <= bound, Is.True, context + " exact dyadic equidistance residual");
                }
            }
        }
        private static IEnumerable<CircleCore> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Triangle2d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new CircleCore(assembly, true, target); yield return new CircleCore(assembly, false, target);
            }
        }
    }
}
