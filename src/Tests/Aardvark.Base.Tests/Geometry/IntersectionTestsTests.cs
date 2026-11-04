using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Versioning;
using BigInteger = System.Numerics.BigInteger;
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

        [Test]
        public void BoxTrianglesRecognizeEnclosureRegardlessOfOrder()
        {
            CheckBoxTriangleVariants("box strictly enclosed", true,
                new V2d(-4, -4), new V2d(8, -4), new V2d(-4, 8));
        }

        [Test]
        public void BoxTrianglesRejectSeparatedFallbacksRegardlessOfOrder()
        {
            // Every box point has x+y <= 2; every triangle point has x+y >= 4.
            // Their coordinate bounds overlap, so a common outside flag cannot reject them.
            CheckBoxTriangleVariants("separated by x+y=4", false,
                new V2d(6, -2), new V2d(14, 6), new V2d(-2, 6));
        }

        [Test]
        public void BoxTrianglesKeepEarlyOutContactsAndSeparation()
        {
            CheckBoxTriangleVariants("triangle inside box", true,
                new V2d(.25, .25), new V2d(.75, .25), new V2d(.25, .75));
            CheckBoxTriangleVariants("crossing edges, no vertex inside", true,
                new V2d(-1, .5), new V2d(2, .5), new V2d(.5, 2));
            CheckBoxTriangleVariants("crossing through corners", true,
                new V2d(-1, -1), new V2d(2, 2), new V2d(2, 1.5));
            CheckBoxTriangleVariants("edge contact", true,
                new V2d(-1, 1), new V2d(2, 1), new V2d(.5, 3));
            CheckBoxTriangleVariants("vertex contact", true,
                new V2d(1, 1), new V2d(2, 1), new V2d(1, 2));
            CheckBoxTriangleVariants("same-side separation", false,
                new V2d(2, 0), new V2d(3, 0), new V2d(2, 1));
        }

        private static void CheckBoxTriangleVariants(string label, bool expected, params V2d[] points)
        {
            var orders = new[] { new[] { 0, 1, 2 }, new[] { 0, 2, 1 }, new[] { 1, 0, 2 },
                                 new[] { 1, 2, 0 }, new[] { 2, 0, 1 }, new[] { 2, 1, 0 } };
            foreach (var core in BoxTriangleCores())
            foreach (var scale in new[] { .125, .5, 1, 2, 16, 1024 })
            foreach (var turn in new[] { 0, 1, 2, 3 })
            foreach (var offset in new[] { V2d.Zero, new V2d(16, -32), new V2d(-.5, .25), new V2d(65536, -32768) })
            {
                V2d Transform(V2d p)
                {
                    for (int i = 0; i < turn; i++) p = new V2d(-p.Y, p.X);
                    return offset + scale * p;
                }
                var first = Transform(V2d.Zero);
                var last = Transform(V2d.One);
                var min = new V2d(Min(first.X, last.X), Min(first.Y, last.Y));
                var max = new V2d(Max(first.X, last.X), Max(first.Y, last.Y));
                var transformed = points.Select(Transform).ToArray();
                var context = $"{label}; scale={scale:R}, quarter-turns={turn}, offset={offset}, box=[{min},{max}]";
                Assert.AreEqual(expected, BoxTriangleSeparatingAxes(min, max, transformed), context + " independent geometry");
                foreach (var order in orders)
                    core.Check(min, max, transformed[order[0]], transformed[order[1]], transformed[order[2]], expected,
                        context + ", order=" + string.Join(",", order));
            }
        }

        private static bool BoxTriangleSeparatingAxes(V2d min, V2d max, V2d[] triangle)
        {
            // All fixture coordinates are exactly on this global dyadic grid, including
            // the large translations. Integer projection intervals avoid barycentric algebra.
            (BigInteger X, BigInteger Y) Grid(V2d p)
            {
                Assert.AreEqual(Truncate(p.X * 32), p.X * 32, "fixture X grid");
                Assert.AreEqual(Truncate(p.Y * 32), p.Y * 32, "fixture Y grid");
                return (new BigInteger(p.X * 32), new BigInteger(p.Y * 32));
            }
            var tri = triangle.Select(Grid).ToArray();
            var box = new[] { min, new V2d(max.X, min.Y), max, new V2d(min.X, max.Y) }.Select(Grid).ToArray();
            var axes = new List<(BigInteger X, BigInteger Y)> { (1, 0), (0, 1) };
            for (int i = 0; i < 3; i++)
            {
                var a = tri[i]; var b = tri[(i + 1) % 3];
                axes.Add((a.Y - b.Y, b.X - a.X));
            }
            foreach (var axis in axes)
            {
                var t = tri.Select(p => p.X * axis.X + p.Y * axis.Y).ToArray();
                var b = box.Select(p => p.X * axis.X + p.Y * axis.Y).ToArray();
                if (t.Max() < b.Min() || b.Max() < t.Min()) return false;
            }
            return true;
        }

        private sealed class BoxTriangleCore
        {
            private readonly Type scalar;
            private readonly ConstructorInfo vector, box, triangle;
            private readonly MethodInfo vertices, primitive;
            private readonly string name;

            public BoxTriangleCore(Assembly assembly, bool wide, string target)
            {
                name = target + (wide ? "/double" : "/float");
                scalar = wide ? typeof(double) : typeof(float);
                var suffix = wide ? "d" : "f";
                var vt = assembly.GetType("Aardvark.Base.V2" + suffix, true);
                var bt = assembly.GetType("Aardvark.Base.Box2" + suffix, true);
                var tt = assembly.GetType("Aardvark.Base.Triangle2" + suffix, true);
                var fun = assembly.GetType("Aardvark.Base.GeometryFun", true);
                foreach (var type in new[] { vt, bt, tt, fun }) Assert.AreSame(assembly, type.Assembly, name);
                vector = vt.GetConstructor(new[] { scalar, scalar });
                box = bt.GetConstructor(new[] { vt, vt });
                triangle = tt.GetConstructor(new[] { vt, vt, vt });
                vertices = fun.GetMethod("IntersectsTriangle", new[] { bt, vt, vt, vt });
                primitive = fun.GetMethod("Intersects", new[] { bt, tt });
                foreach (var method in new MethodBase[] { vector, box, triangle, vertices, primitive })
                {
                    Assert.IsNotNull(method, name);
                    Assert.AreSame(assembly, method.DeclaringType.Assembly, name);
                }
            }

            private object Vector(V2d p)
            {
                object Scalar(double value)
                {
                    if (scalar == typeof(double)) return value;
                    Assert.AreEqual(value, (double)(float)value, name + " exact float fixture: " + value.ToString("R"));
                    return (float)value;
                }
                return vector.Invoke(new[] { Scalar(p.X), Scalar(p.Y) });
            }

            public void Check(V2d min, V2d max, V2d p0, V2d p1, V2d p2, bool expected, string context)
            {
                var b = box.Invoke(new[] { Vector(min), Vector(max) });
                var a = Vector(p0); var c = Vector(p1); var d = Vector(p2);
                var message = $"{name}: {context}, triangle=[{p0},{p1},{p2}]";
                Assert.AreEqual(expected, vertices.Invoke(null, new[] { b, a, c, d }), message + " IntersectsTriangle");
                var t = triangle.Invoke(new[] { a, c, d });
                Assert.AreEqual(expected, primitive.Invoke(null, new[] { b, t }), message + " Intersects(Triangle2*)");
            }
        }

        private static IEnumerable<BoxTriangleCore> BoxTriangleCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Box2d).Assembly.Location), "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required sibling core: " + path);
                var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0",
                    assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName, path);
                yield return new BoxTriangleCore(assembly, false, target);
                yield return new BoxTriangleCore(assembly, true, target);
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
