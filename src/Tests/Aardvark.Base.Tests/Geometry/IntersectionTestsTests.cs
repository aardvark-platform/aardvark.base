using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Versioning;
using static System.Math;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class IntersectionTestsTests
    {
        #region Contains

        #region Triangle2d contains *

        [Test]
        public void Contains_Triangle2d_V2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new V2d(1, 1)));
            Assert.IsTrue(t.Contains(new V2d(2, 0)));
            Assert.IsTrue(!t.Contains(new V2d(0, 0.000001)));
        }

        [Test]
        public void Contains_Triangle2d_Line2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new Line2d(new V2d(0, 0), new V2d(2, 0))));
            Assert.IsTrue(t.Contains(new Line2d(new V2d(2, 0), new V2d(1, 2))));
            Assert.IsTrue(t.Contains(new Line2d(new V2d(1, 2), new V2d(0, 0))));
            Assert.IsTrue(t.Contains(new Line2d(new V2d(0.5, 0.1), new V2d(1.5, 0.2))));
            Assert.IsTrue(!t.Contains(new Line2d(new V2d(0.5, 0.1), new V2d(2, 2))));
            Assert.IsTrue(!t.Contains(new Line2d(new V2d(0.1, 0.2), new V2d(1.1, 10))));
        }

        [Test]
        public void Contains_Triangle2d_Triangle2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2))));
            Assert.IsTrue(t.Contains(new Triangle2d(new V2d(0.2, 0.1), new V2d(1.5, 0.5), new V2d(1, 1.5))));
            Assert.IsTrue(!t.Contains(new Triangle2d(new V2d(0.2, 0.1), new V2d(1.5, 0.5), new V2d(1, 2.000001))));
            Assert.IsTrue(!t.Contains(new Triangle2d(new V2d(-1, -1), new V2d(3, -1), new V2d(1, 5))));
        }

        [Test]
        public void Contains_Triangle2d_Quad2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new Quad2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2), new V2d(1, 2))));
            Assert.IsTrue(t.Contains(new Quad2d(new V2d(0, 0), new V2d(2, 0), new V2d(1.5, 0.2), new V2d(0.5, 0.2))));
            Assert.IsTrue(!t.Contains(new Quad2d(new V2d(0, 0), new V2d(2, 0), new V2d(1.5, 0.2), new V2d(0.5, 1.1))));
            Assert.IsTrue(!t.Contains(new Quad2d(new V2d(-1, -1), new V2d(5, -1), new V2d(5, 5), new V2d(0, 5))));
        }

        [Test]
        public void Contains_Triangle2d_Circle2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new Circle2d(new V2d(1, 1), 0.4)));
            Assert.IsTrue(t.Contains(new Circle2d(new V2d(1, 1), 1.0 / Sqrt(5.0))));
            Assert.IsTrue(t.Contains(new Circle2d(new V2d(1, 2), 0.0)));
            Assert.IsTrue(!t.Contains(new Circle2d(new V2d(1, 0.1), 0.100001)));
            Assert.IsTrue(!t.Contains(new Circle2d(new V2d(1, 2), 10)));
        }

        [Test]
        public void Contains_Triangle2f_Circle2f()
        {
            var t = new Triangle2f(new V2f(0, 0), new V2f(4, 0), new V2f(0, 3));
            Assert.IsTrue(t.Contains(new Circle2f(new V2f(1, 1), 0.5f)));
            Assert.IsTrue(t.Contains(new Circle2f(new V2f(1, 1), 1.0f)));
            Assert.IsTrue(!t.Contains(new Circle2f(new V2f(1, 1), 1.000001f)));
            Assert.IsTrue(!t.Contains(new Circle2f(new V2f(3, 2), 0.0f)));
        }

        #endregion

        #region Quad2d contains *

        [Test]
        public void Contains_Quad2d_Line2d()
        {
            var q = new Quad2d(new V2d(0, 0), new V2d(4, 0), new V2d(4, 3), new V2d(0, 3));
            Assert.IsTrue(q.Contains(new Line2d(new V2d(1, 1), new V2d(3, 2))));
            Assert.IsTrue(q.Contains(new Line2d(new V2d(0, 0), new V2d(4, 0))));
            Assert.IsTrue(!q.Contains(new Line2d(new V2d(1, 1), new V2d(5, 1))));
        }

        [Test]
        public void Contains_Quad2d_Triangle2d()
        {
            var q = new Quad2d(new V2d(0, 0), new V2d(4, 0), new V2d(4, 3), new V2d(0, 3));
            Assert.IsTrue(q.Contains(new Triangle2d(new V2d(1, 1), new V2d(3, 1), new V2d(2, 2))));
            Assert.IsTrue(q.Contains(new Triangle2d(new V2d(0, 0), new V2d(4, 0), new V2d(0, 3))));
            Assert.IsTrue(!q.Contains(new Triangle2d(new V2d(1, 1), new V2d(3, 1), new V2d(2, 4))));
        }

        [Test]
        public void Contains_Quad2d_Quad2d()
        {
            var q = new Quad2d(new V2d(0, 0), new V2d(4, 0), new V2d(4, 3), new V2d(0, 3));
            Assert.IsTrue(q.Contains(new Quad2d(new V2d(1, 1), new V2d(3, 1), new V2d(3, 2), new V2d(1, 2))));
            Assert.IsTrue(q.Contains(new Quad2d(new V2d(0, 0), new V2d(4, 0), new V2d(4, 3), new V2d(0, 3))));
            Assert.IsTrue(!q.Contains(new Quad2d(new V2d(1, 1), new V2d(5, 1), new V2d(5, 2), new V2d(1, 2))));
        }

        [Test]
        public void Contains_Quad2d_Circle2d()
        {
            var q = new Quad2d(new V2d(0, 0), new V2d(4, 0), new V2d(4, 3), new V2d(0, 3));
            Assert.IsTrue(q.Contains(new Circle2d(new V2d(2, 1.5), 1.0)));
            Assert.IsTrue(q.Contains(new Circle2d(new V2d(2, 1.5), 1.5)));
            Assert.IsTrue(q.Contains(new Circle2d(new V2d(0, 0), 0.0)));
            Assert.IsTrue(!q.Contains(new Circle2d(new V2d(2, 1.5), 1.500001)));
            Assert.IsTrue(!q.Contains(new Circle2d(new V2d(5, 1.5), 0.0)));
        }

        [Test]
        public void Contains_Quad2f_Circle2f()
        {
            var q = new Quad2f(new V2f(0, 0), new V2f(4, 0), new V2f(4, 3), new V2f(0, 3));
            Assert.IsTrue(q.Contains(new Circle2f(new V2f(2, 1.5f), 1.5f)));
            Assert.IsTrue(!q.Contains(new Circle2f(new V2f(2, 1.5f), 1.500001f)));
        }

        #endregion

        #region Circle2d contains *

        [Test]
        public void Contains_Circle2d_Circle2d()
        {
            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 0.5).Contains(new Circle2d(new V2d(0, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(0, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(0, 0), 0.5)));
            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(1, 0), 1)));
            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(2, 0), 1)));

            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(2.000001, 0), 1)));
        }

        #endregion

        #endregion

        #region Intersects

        private static readonly V3d[] PlanePairNormals =
        {
            V3d.XAxis, V3d.YAxis, V3d.ZAxis, new V3d(3, 4, 0) / 5,
            new V3d(0, 3, 4) / 5, new V3d(1, 2, 2) / 3, new V3d(2, 3, 6) / 7
        };

        [Test]
        public void PlanePairsRecognizeReversedCoincidence()
        {
            foreach (var core in PlanePairCores())
            foreach (var normal in PlanePairNormals)
            foreach (var sign in new[] { -1, 1 })
            foreach (var distance in new[] { -8.0, -1, -.25, -0.0, 0.0, .5, 1, 8 })
                core.Check(sign * normal, distance, -sign * normal, -distance,
                    true, true, true, "reversed coincident plane");
        }

        [Test]
        public void PlanePairsRejectSeparatedOppositeNormals()
        {
            foreach (var core in PlanePairCores())
            foreach (var normal in PlanePairNormals)
            foreach (var sign in new[] { -1, 1 })
            foreach (var distances in new[] { (1.0, -1.0), (1.0, 0.0), (0.0, 1.0), (-8.0, -2.0), (.25, 8.0) })
                core.Check(sign * normal, distances.Item1, -sign * normal, -distances.Item2,
                    false, false, true, "separated parallel planes");
        }

        [Test]
        public void PlanePairsKeepCoOrientedAndCrossingContracts()
        {
            foreach (var core in PlanePairCores())
            {
                foreach (var normal in PlanePairNormals)
                foreach (var sign in new[] { -1, 1 })
                foreach (var distance in new[] { -8.0, -1, -.25, -0.0, 0.0, .5, 1, 8 })
                {
                    core.Check(sign * normal, distance, sign * normal, distance,
                        true, true, true, "co-oriented coincidence");
                    core.Check(sign * normal, distance, sign * normal, distance + 2,
                        false, false, true, "co-oriented separation");
                }
                var pairs = new[] { (V3d.XAxis, V3d.YAxis), (V3d.XAxis, V3d.ZAxis),
                    (V3d.YAxis, V3d.ZAxis), (PlanePairNormals[3], PlanePairNormals[4]) };
                foreach (var pair in pairs)
                foreach (var point in new[] { V3d.Zero, new V3d(1, 2, 3), new V3d(-4, .5, 2), new V3d(1024, -2048, 4096) })
                foreach (var sign0 in new[] { -1, 1 })
                foreach (var sign1 in new[] { -1, 1 })
                {
                    var n0 = core.Round(sign0 * pair.Item1); var n1 = core.Round(sign1 * pair.Item2);
                    core.Check(n0, core.Dot(n0, point), n1, core.Dot(n1, point),
                        true, true, false, "crossing planes through known point " + point);
                }
                foreach (var delta in new[] { 0.0, core.Previous(core.Tiny), core.Tiny, core.Next(core.Tiny), 2 * core.Tiny })
                foreach (var sign in new[] { -1, 1 })
                foreach (var orientation in new[] { -1, 1 })
                {
                    bool coincident = delta < core.Tiny;
                    core.Check(V3d.XAxis, 0, orientation * V3d.XAxis, sign * delta,
                        coincident, coincident, true, "strict distance tolerance", core.Tiny);
                }
                // Keep the existing L1 boolean / L2 ray parallel classifications distinct.
                foreach (var fraction in new[] { .25, .6, 2.0 })
                {
                    var n = new V3d(1, core.Scalar(fraction * core.Tiny), core.Scalar(fraction * core.Tiny));
                    core.Check(V3d.XAxis, 0, n, 1, fraction > .5, fraction > 1, fraction < 1,
                        "parallel-classification control, fraction=" + fraction);
                }
            }
        }

        private sealed class PlanePairCore
        {
            private readonly bool wide;
            private readonly string name;
            private readonly ConstructorInfo vector, plane, ray;
            private readonly MethodInfo boolean, withRay;
            private readonly FieldInfo x, y, z, origin, direction;
            public double Unit => wide ? Math.ScaleB(1, -52) : Math.ScaleB(1, -23);
            public double Tiny => 4 * Unit;
            public double Scalar(double value) => wide ? value : (float)value;
            public V3d Round(V3d value) => new V3d(Scalar(value.X), Scalar(value.Y), Scalar(value.Z));
            public double Previous(double value) => wide ? Math.BitDecrement(value) : float.BitDecrement((float)value);
            public double Next(double value) => wide ? Math.BitIncrement(value) : float.BitIncrement((float)value);
            public double Dot(V3d a, V3d b) => Scalar(Scalar(Scalar(a.X * b.X) + Scalar(a.Y * b.Y)) + Scalar(a.Z * b.Z));

            public PlanePairCore(Assembly assembly, bool wide, string target)
            {
                this.wide = wide; name = target + (wide ? "/double" : "/float");
                var scalar = wide ? typeof(double) : typeof(float); var suffix = wide ? "d" : "f";
                var vt = assembly.GetType("Aardvark.Base.V3" + suffix, true);
                var pt = assembly.GetType("Aardvark.Base.Plane3" + suffix, true);
                var rt = assembly.GetType("Aardvark.Base.Ray3" + suffix, true);
                var fun = assembly.GetType("Aardvark.Base.GeometryFun", true);
                foreach (var type in new[] { vt, pt, rt, fun }) Assert.AreSame(assembly, type.Assembly, name);
                vector = vt.GetConstructor(new[] { scalar, scalar, scalar });
                plane = pt.GetConstructor(new[] { vt, scalar }); ray = rt.GetConstructor(new[] { vt, vt });
                boolean = fun.GetMethod("Intersects", new[] { pt, pt });
                withRay = fun.GetMethod("Intersects", new[] { pt, pt, rt.MakeByRefType() });
                x = vt.GetField("X"); y = vt.GetField("Y"); z = vt.GetField("Z");
                origin = rt.GetField("Origin"); direction = rt.GetField("Direction");
                foreach (var method in new MethodBase[] { vector, plane, ray, boolean, withRay })
                {
                    Assert.IsNotNull(method, name); Assert.AreSame(assembly, method.DeclaringType.Assembly, name);
                }
            }
            private object Box(double value) => wide ? (object)value : (float)value;
            private object Vector(V3d p) => vector.Invoke(new[] { Box(p.X), Box(p.Y), Box(p.Z) });
            private V3d ReadVector(object value) => new V3d(Convert.ToDouble(x.GetValue(value)), Convert.ToDouble(y.GetValue(value)), Convert.ToDouble(z.GetValue(value)));
            private void Same(double expected, double actual, string message)
                => Assert.AreEqual(wide ? BitConverter.DoubleToInt64Bits(expected) : BitConverter.SingleToInt32Bits((float)expected),
                    wide ? BitConverter.DoubleToInt64Bits(actual) : BitConverter.SingleToInt32Bits((float)actual), message);
            private void Equation(V3d n, V3d p, double distance, double slack, string context)
            {
                double dot = n.X * p.X + n.Y * p.Y + n.Z * p.Z;
                double scale = Abs(n.X * p.X) + Abs(n.Y * p.Y) + Abs(n.Z * p.Z) + Abs(distance);
                Assert.That(Abs(dot - distance), Is.LessThanOrEqualTo(slack + 16 * Unit * scale), context + " plane equation");
            }
            public void Check(V3d n0, double d0, V3d n1, double d1, bool expectedBool, bool expectedRay, bool parallel, string label, double slack = 0)
            {
                n0 = Round(n0); n1 = Round(n1); d0 = Scalar(d0); d1 = Scalar(d1);
                foreach (var swap in new[] { false, true })
                {
                    var a = swap ? n1 : n0; var b = swap ? n0 : n1;
                    var da = swap ? d1 : d0; var db = swap ? d0 : d1;
                    var p0 = plane.Invoke(new[] { Vector(a), Box(da) }); var p1 = plane.Invoke(new[] { Vector(b), Box(db) });
                    var context = $"{name}: {label}, swap={swap}, planes=({a};{da:R}),({b};{db:R})";
                    Assert.AreEqual(expectedBool, boolean.Invoke(null, new[] { p0, p1 }), context + " boolean");
                    var poison = ray.Invoke(new[] { Vector(new V3d(17, 19, 23)), Vector(new V3d(2, 3, 5)) });
                    var args = new[] { p0, p1, poison };
                    Assert.AreEqual(expectedRay, withRay.Invoke(null, args), context + " out ray");
                    var point = ReadVector(origin.GetValue(args[2])); var dir = ReadVector(direction.GetValue(args[2]));
                    if (!expectedRay || parallel)
                    {
                        foreach (var component in new[] { dir.X, dir.Y, dir.Z }) Same(0, component, context + " zero direction");
                        if (!expectedRay)
                        {
                            Assert.IsTrue(double.IsNaN(point.X) && double.IsNaN(point.Y) && double.IsNaN(point.Z), context + " Invalid ray origin");
                            continue;
                        }
                        var representative = Round(a * da);
                        Same(representative.X, point.X, context + " representative X");
                        Same(representative.Y, point.Y, context + " representative Y");
                        Same(representative.Z, point.Z, context + " representative Z");
                    }
                    Equation(a, point, da, slack, context + " first"); Equation(b, point, db, slack, context + " second");
                    if (!parallel)
                    {
                        Equation(a, dir, 0, 0, context + " direction/first normal");
                        Equation(b, dir, 0, 0, context + " direction/second normal");
                        Assert.That(Abs(dir.LengthSquared - 1), Is.LessThanOrEqualTo(16 * Unit), context + " unit direction");
                        Assert.That(dir.Dot(a.Cross(b)), Is.GreaterThan(0), context + " crossing orientation");
                        Equation(a, point + .5 * dir, da, 0, context + " another point/first");
                        Equation(b, point + .5 * dir, db, 0, context + " another point/second");
                    }
                }
            }
        }
        private static IEnumerable<PlanePairCore> PlanePairCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Plane3d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required sibling core: " + path); var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0", assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new PlanePairCore(assembly, false, target); yield return new PlanePairCore(assembly, true, target);
            }
        }

        [Test]
        public void Intersects_Circle2d_Circle2d()
        {
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(0, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(0, 0), 0.5)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 0.5).Intersects(new Circle2d(new V2d(0, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(1, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(2, 0), 1)));
            
            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(2.000001, 0), 1)));
        }

        [Test]
        public void Intersects_Line2d_Line2d()
        {
            // non-parallel, crossing
            Assert.IsTrue(
                new Line2d(new V2d(0,  0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(1, -1), new V2d(1, 1))
                ));

            // non-parallel, touching
            Assert.IsTrue(
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(1, 0), new V2d(1, 1))
                ));

            // non-parallel, almost touching
            Assert.IsTrue(!
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(1, 0.000001), new V2d(1, 1))
                ));

            // identical
            Assert.IsTrue(
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(0, 0), new V2d(2, 0))
                ));
            
            // on same line, almost touching
            Assert.IsTrue(!
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(2.000001, 0), new V2d(4, 0))
                ));

            // on same line, not touching
            Assert.IsTrue(!
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(3, 0), new V2d(5, 0))
                ));
        }

        [Test]
        public void Intersects_Line2d_Line2d_ParallelAndNotTouching()
        {
            // parallel, not touching
            Assert.IsTrue(!
                new Line2d(new V2d(1, 0), new V2d(3, 0)).Intersects(
                new Line2d(new V2d(2, 5), new V2d(4, 5))
                ));
        }

        [Test]
        public void Intersects_Line2d_Line2d_Overlapping()
        {

            // on same line, overlapping
            Assert.IsTrue(
                new Line2d(new V2d(0, 0), new V2d(2, 0)).IntersectsLine(
                new V2d(1, 0), new V2d(3, 0), true, out var foo1
                ));

            // on same line, touching
            Assert.IsTrue(
                new Line2d(new V2d(0, 0), new V2d(2, 0)).IntersectsLine(
                new V2d(2, 0), new V2d(4, 0), true, out var foo2
                ));
        }

        #endregion
    }
}
