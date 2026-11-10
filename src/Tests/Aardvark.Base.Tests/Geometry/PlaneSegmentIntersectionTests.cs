using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class PlaneSegmentIntersectionTests
    {
        private static readonly V2d[] Normals = { V2d.XAxis, V2d.YAxis, new V2d(1, 1), new V2d(1, -1) };
        private static readonly double[] Scales = { .125, .5, 1, 4, 16 };

        private static IEnumerable<(V2d normal, V2d origin, V2d tangent)> Frames()
        {
            foreach (var normal in Normals)
            foreach (var translation in new[] { -16.0, -.5, 0.0, 8.0 })
            {
                var tangent = new V2d(-normal.Y, normal.X);
                // Cardinal planes move normally as well as tangentially. The
                // diagonals keep exact cancellation after tangent translation.
                var origin = normal.X * normal.Y == 0 ? new V2d(translation, translation * .5) : translation * tangent;
                yield return (normal, origin, tangent);
            }
        }

        [Test]
        public void ZeroToleranceReturnsEachContactEndpoint()
        {
            foreach (var core in Cores())
            {
                foreach (var frame in Frames())
                foreach (var scale in Scales)
                foreach (var offset in new[] { -2.0, 0.0, 3.0 })
                foreach (var side in new[] { -1, 1 })
                {
                    var contact = frame.origin + offset * scale * frame.tangent;
                    var other = contact + side * scale * frame.normal + scale * frame.tangent;
                    core.CheckReversals(frame.normal, frame.origin, contact, other, 0, true, "single endpoint contact");
                }
                core.SignedZeros();
            }
        }

        [Test]
        public void ZeroToleranceKeepsCoincidentAndPointSegments()
        {
            foreach (var core in Cores())
            {
                foreach (var frame in Frames())
                foreach (var scale in Scales)
                foreach (var offset in new[] { -2.0, 0.0, 3.0 })
                {
                    var a = frame.origin + offset * scale * frame.tangent;
                    var b = frame.origin + (offset + 4) * scale * frame.tangent;
                    core.CheckReversals(frame.normal, frame.origin, a, b, 0, true, "coincident midpoint");
                    core.CheckReversals(frame.normal, frame.origin, a, a, 0, true, "on-plane point segment");
                }
                // Nonzero-distance oblique planes with exact endpoint heights.
                foreach (var normal in new[] { new V2d(1, 1), new V2d(1, -1) })
                foreach (var scale in Scales)
                {
                    var a = new V2d(scale, 0);
                    var b = new V2d(0, scale * normal.Y);
                    core.CheckReversals(normal, a, a, b, 0, true, "translated oblique coincidence");
                }
            }
        }

        [Test]
        public void CrossingsMissesAndPositiveToleranceStayCompatible()
        {
            foreach (var core in Cores())
            {
                foreach (var frame in Frames())
                foreach (var scale in Scales)
                {
                    var a = frame.origin - scale * frame.normal - 2 * scale * frame.tangent;
                    foreach (var ratio in new[] { .5, 1.0, 3.0 })
                    {
                        var b = frame.origin + ratio * scale * frame.normal + 3 * scale * frame.tangent;
                        core.CheckReversals(frame.normal, frame.origin, a, b, 0, true, "strict crossing");
                        core.CheckReversals(frame.normal, frame.origin, a, b - 8 * scale * frame.normal, 0, true, "strict miss");
                    }
                    core.CheckReversals(frame.normal, frame.origin, a, a, 0, true, "off-plane point miss");
                }
                foreach (var epsilon in new[] { .125, .5, 2.0 })
                foreach (var a in new[] { -2 * epsilon, -epsilon, -epsilon * .5, 0, epsilon * .5, epsilon, 2 * epsilon })
                foreach (var b in new[] { -2 * epsilon, -epsilon, -epsilon * .5, 0, epsilon * .5, epsilon, 2 * epsilon })
                    core.CheckReversals(V2d.XAxis, new V2d(8, -4), new V2d(8 + a, -3), new V2d(8 + b, 5), epsilon, false, "strict positive-tolerance boundaries");
                core.ToleranceNeighborsAndLegacy();
            }
        }

        private sealed class PlaneCore
        {
            private readonly bool single;
            private readonly string name;
            private readonly Type scalar, vector, plane, line;
            private readonly ConstructorInfo vectorConstructor, planeConstructor, lineConstructor;
            private readonly MethodInfo pointDefault, pointEpsilon, booleanDefault, booleanEpsilon, height;
            private readonly FieldInfo x, y;
            public PlaneCore(Assembly assembly, bool single, string target)
            {
                this.single = single; name = target + (single ? "/float" : "/double");
                scalar = single ? typeof(float) : typeof(double);
                string suffix = single ? "f" : "d";
                vector = assembly.GetType("Aardvark.Base.V2" + suffix, true);
                plane = assembly.GetType("Aardvark.Base.Plane2" + suffix, true);
                line = assembly.GetType("Aardvark.Base.Line2" + suffix, true);
                foreach (var type in new[] { vector, plane, line }) Assert.AreSame(assembly, type.Assembly, name);
                vectorConstructor = vector.GetConstructor(new[] { scalar, scalar });
                planeConstructor = plane.GetConstructor(new[] { vector, scalar });
                lineConstructor = line.GetConstructor(new[] { vector, vector });
                foreach (var constructor in new[] { vectorConstructor, planeConstructor, lineConstructor }) Assert.AreSame(assembly, constructor.DeclaringType.Assembly, name);
                x = vector.GetField("X"); y = vector.GetField("Y");
                foreach (var field in new[] { x, y }) { Assert.AreSame(assembly, field.DeclaringType.Assembly, name); Assert.AreEqual(scalar, field.FieldType, name); }
                var geometry = assembly.GetType("Aardvark.Base.GeometryFun", true);
                pointDefault = Bind(geometry, "Intersects", typeof(bool), plane, line, vector.MakeByRefType());
                pointEpsilon = Bind(geometry, "Intersects", typeof(bool), plane, line, scalar, vector.MakeByRefType());
                booleanDefault = Bind(geometry, "Intersects", typeof(bool), plane, line);
                booleanEpsilon = Bind(geometry, "Intersects", typeof(bool), plane, line, scalar);
                height = Bind(plane, "Height", scalar, vector);
                Assert.IsTrue(pointDefault.IsStatic && pointEpsilon.IsStatic && booleanDefault.IsStatic && booleanEpsilon.IsStatic, name);
            }
            private MethodInfo Bind(Type owner, string method, Type result, params Type[] parameters)
            {
                var info = owner.GetMethod(method, parameters);
                Assert.IsNotNull(info, name + method); Assert.AreSame(owner.Assembly, info.DeclaringType.Assembly, name); Assert.AreEqual(result, info.ReturnType, name);
                return info;
            }
            private double R(double value) => single ? (float)value : value;
            private object Box(double value) => single ? (object)(float)value : value;
            private V2d Round(V2d value) => new V2d(R(value.X), R(value.Y));
            private object Vector(V2d value) => vectorConstructor.Invoke(new[] { Box(value.X), Box(value.Y) });
            private V2d Read(object value) => new V2d(Convert.ToDouble(x.GetValue(value)), Convert.ToDouble(y.GetValue(value)));
            private double Height(V2d n, double d, V2d p) => R(R(R(n.X * p.X) + R(n.Y * p.Y)) - d);
            private static decimal D(double value) => decimal.Parse(value.ToString("R", System.Globalization.CultureInfo.InvariantCulture), System.Globalization.CultureInfo.InvariantCulture);
            private static int Sign(decimal value, decimal epsilon) => value == 0 ? 0 : Math.Abs(value) < epsilon ? 0 : Math.Sign(value);
            private void Bits(double expected, double actual, string context) => Assert.AreEqual(BitConverter.DoubleToInt64Bits(expected), BitConverter.DoubleToInt64Bits(actual), context + $", expected={expected:R}, actual={actual:R}");
            private void Exact(V2d expected, V2d actual, string context) { Bits(expected.X, actual.X, context + " X"); Bits(expected.Y, actual.Y, context + " Y"); }

            public void CheckReversals(V2d m, V2d origin, V2d a, V2d b, double epsilon, bool zeroOverloads, string label)
            {
                foreach (var direction in new[] { -1, 1 })
                foreach (var reverse in new[] { false, true })
                {
                    var normal = direction * m;
                    var p0 = Round(reverse ? b : a); var p1 = Round(reverse ? a : b);
                    var n = Round(normal / Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y));
                    double k = normal.X * origin.X + normal.Y * origin.Y;
                    double d = R(k / Math.Sqrt(normal.X * normal.X + normal.Y * normal.Y));
                    decimal h0 = D(normal.X) * D(p0.X) + D(normal.Y) * D(p0.Y) - D(k);
                    decimal h1 = D(normal.X) * D(p1.X) + D(normal.Y) * D(p1.Y) - D(k);
                    int s0 = Sign(h0, D(epsilon)), s1 = Sign(h1, D(epsilon));
                    bool hit = s0 == 0 || s1 == 0 || s0 != s1;
                    bool exact = s0 == 0 || s1 == 0;
                    V2d expected = V2d.NaN;
                    if (s0 == 0 && s1 == 0) expected = Round(Round(p0 + p1) * .5);
                    else if (s0 == 0) expected = p0;
                    else if (s1 == 0) expected = p1;
                    else if (hit)
                    {
                        decimal t = h0 / (h0 - h1);
                        expected = new V2d((double)(D(p0.X) + t * (D(p1.X) - D(p0.X))), (double)(D(p0.Y) + t * (D(p1.Y) - D(p0.Y))));
                    }
                    string context = $"{name}: {label}, normal={n}, distance={d:R}, P0={p0}, P1={p1}, epsilon={epsilon:R}, heights={h0}/{h1}";
                    var pl = planeConstructor.Invoke(new[] { Vector(n), Box(d) });
                    if (h0 == 0) Assert.AreEqual(0, Convert.ToDouble(height.Invoke(pl, new[] { Vector(p0) })), context + " exact contact setup P0");
                    if (h1 == 0) Assert.AreEqual(0, Convert.ToDouble(height.Invoke(pl, new[] { Vector(p1) })), context + " exact contact setup P1");
                    Invoke(n, d, p0, p1, epsilon, false, hit, expected, exact, context);
                    if (zeroOverloads)
                    {
                        Invoke(n, d, p0, p1, 0, true, hit, expected, exact, context + " default");
                        Invoke(n, d, p0, p1, -0.0, false, hit, expected, exact, context + " negative-zero epsilon");
                    }
                }
            }
            private void Invoke(V2d n, double d, V2d p0, V2d p1, double epsilon, bool useDefault, bool expectedHit, V2d expected, bool exact, string context)
            {
                var pl = planeConstructor.Invoke(new[] { Vector(n), Box(d) });
                var segment = lineConstructor.Invoke(new[] { Vector(p0), Vector(p1) });
                object[] args = useDefault ? new[] { pl, segment, Vector(new V2d(123, -456)) } : new[] { pl, segment, Box(epsilon), Vector(new V2d(123, -456)) };
                bool hit = (bool)(useDefault ? pointDefault : pointEpsilon).Invoke(null, args);
                Assert.AreEqual(expectedHit, hit, context + " classification");
                var actual = Read(args[args.Length - 1]);
                if (!hit) Assert.IsTrue(double.IsNaN(actual.X) && double.IsNaN(actual.Y), context + " miss replaces poisoned output with NaN");
                else if (exact) Exact(expected, actual, context + " representative bits");
                else
                {
                    double bound = 16 * Math.ScaleB(1, single ? -23 : -52) * (Math.Abs(p0.X) + Math.Abs(p0.Y) + Math.Abs(p1.X) + Math.Abs(p1.Y));
                    Assert.IsTrue(double.IsFinite(actual.X) && double.IsFinite(actual.Y), context + " finite crossing");
                    Assert.That(Math.Abs(actual.X - expected.X), Is.LessThanOrEqualTo(bound), context + " analytic X");
                    Assert.That(Math.Abs(actual.Y - expected.Y), Is.LessThanOrEqualTo(bound), context + " analytic Y");
                }
                // Boolean-only overloads deliberately retain their separate contracts.
                double h0 = Height(n, d, p0), h1 = Height(n, d, p1);
                Assert.AreEqual(R(h0 * h1) <= 0, (bool)booleanDefault.Invoke(null, new[] { pl, segment }), context + " unchanged Boolean default");
                double length2 = R(R(n.X * n.X) + R(n.Y * n.Y));
                bool booleanExpected = R(h0 * h1) < R(R(R(epsilon * epsilon) * length2));
                Assert.AreEqual(booleanExpected, (bool)booleanEpsilon.Invoke(null, new[] { pl, segment, Box(epsilon) }), context + " unchanged Boolean epsilon");
            }
            public void SignedZeros()
            {
                double negativeZero = BitConverter.Int64BitsToDouble(long.MinValue);
                var n = new V2d(1, negativeZero); var a = new V2d(negativeZero, 0);
                var pl = planeConstructor.Invoke(new[] { Vector(n), Box(0) });
                Bits(negativeZero, Convert.ToDouble(height.Invoke(pl, new[] { Vector(a) })), name + " negative-zero endpoint height");
                foreach (var side in new[] { -1, 1 })
                foreach (var reverse in new[] { false, true })
                foreach (var useDefault in new[] { false, true })
                {
                    var b = new V2d(side, 2);
                    Invoke(n, 0, reverse ? b : a, reverse ? a : b, 0, useDefault, true, a, true, name + " signed-zero endpoint contact");
                }
                Invoke(n, 0, a, a, 0, false, true, a, true, name + " signed-zero point midpoint");
            }
            public void ToleranceNeighborsAndLegacy()
            {
                // No translated addition can round these immediate neighbors
                // back onto the strict positive-tolerance boundary.
                double e = .5;
                double below = single ? MathF.BitDecrement((float)e) : Math.BitDecrement(e);
                double above = single ? MathF.BitIncrement((float)e) : Math.BitIncrement(e);
                foreach (var a in new[] { -above, -e, -below, 0, below, e, above })
                foreach (var b in new[] { -above, -e, -below, 0, below, e, above })
                    CheckReversals(V2d.XAxis, V2d.Zero, new V2d(a, -2), new V2d(b, 3), e, false, "tolerance neighbors");
                var random = new Random(193153);
                for (int i = 0; i < 256; i++)
                {
                    var a = new V2d(random.Next(-32, 33) * .25, random.Next(-32, 33) * .25);
                    var b = new V2d(random.Next(-32, 33) * .25, random.Next(-32, 33) * .25);
                    CheckReversals(V2d.XAxis, V2d.Zero, a, b, .5, false, "seed193153 case=" + i);
                }
            }
        }
        private static IEnumerable<PlaneCore> Cores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Plane2d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new PlaneCore(assembly, true, target);
                yield return new PlaneCore(assembly, false, target);
            }
        }
    }
}
