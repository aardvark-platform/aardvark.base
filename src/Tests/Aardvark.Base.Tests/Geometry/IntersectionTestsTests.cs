using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
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
        public void RadialPairsRejectExtremeSeparatedBounds()
        {
            foreach (var core in RadialCores())
            {
                foreach (var exponent in core.Exponents)
                {
                    var scale = Math.ScaleB(1, exponent);
                    foreach (var offset in new[] { -2, 0, 2 })
                    foreach (var separation in new[] { new[] { 3, 0, 0 }, new[] { 2, 2, 0 }, new[] { 2, 2, 2 } })
                    {
                        var a = new[] { offset * scale, offset * scale, offset * scale };
                        var b = new[] { (offset + separation[0]) * scale, (offset + separation[1]) * scale, (offset + separation[2]) * scale };
                        core.CheckExact(a, scale, b, scale, $"separated, exponent={exponent}, offset={offset}");
                        core.CheckExact(a, 0, b, 0, $"distinct points, exponent={exponent}, offset={offset}");
                    }
                }
                var max = core.Wide ? double.MaxValue : float.MaxValue;
                var tiny = core.Wide ? double.Epsilon : float.Epsilon;
                core.CheckExact(new[] { -max, 0.0, 0.0 }, max / 2, new[] { max, 0.0, 0.0 }, max / 2, "overflowing difference");
                core.CheckExact(new[] { -max, -max, -max }, max, new[] { max, max, max }, max, "overflowing differences and radius sum, diagonal");
                core.CheckExact(new[] { max, 0.0, 0.0 }, 0, new[] { max, tiny, 0.0 }, 0, "retain tiny local difference beside a large common coordinate");
                var below = core.Wide ? Math.BitDecrement(max) : float.BitDecrement((float)max);
                var gap = max - below;
                core.CheckExact(new[] { max, 0.0, 0.0 }, gap / 4, new[] { below, 0.0, 0.0 }, gap / 4, "neighboring large centers retain local difference");
            }
        }

        [Test]
        public void RadialPairsKeepClosedContactsAcrossScales()
        {
            foreach (var core in RadialCores())
            {
                foreach (var exponent in core.Exponents)
                {
                    var scale = Math.ScaleB(1, exponent);
                    foreach (var offset in new[] { -2, 0, 2 })
                    {
                        var a = new[] { offset * scale, offset * scale, offset * scale };
                        foreach (var separation in new[] { new[] { 0, 0, 0 }, new[] { 2, 0, 0 }, new[] { 3, 4, 0 }, new[] { 5, 12, 0 }, new[] { 1, 2, 2 }, new[] { 2, 3, 6 } })
                        foreach (var radii in new[] { (0, 0), (1, 1), (1, 2), (2, 3), (3, 4), (6, 7), (4, 2), (8, 0) })
                        {
                            var b = new[] { (offset + separation[0]) * scale, (offset + separation[1]) * scale, (offset + separation[2]) * scale };
                            core.CheckExact(a, radii.Item1 * scale, b, radii.Item2 * scale, $"contact/overlap/containment, exponent={exponent}, offset={offset}");
                        }
                    }
                }
                var max = core.Wide ? double.MaxValue : float.MaxValue;
                core.CheckExact(new[] { -max, 0.0, 0.0 }, max, new[] { max, 0.0, 0.0 }, max, "tangent with overflowing difference and sum");
                core.CheckExact(new[] { -max / 2, 0.0, 0.0 }, max, new[] { max / 2, 0.0, 0.0 }, max, "overlap with overflowing radius sum");
                var random = new Random(149123 + core.Dimension);
                foreach (var exponent in new[] { core.Exponents[0], core.Wide ? -600 : -80, 0, core.Wide ? 600 : 80 })
                for (var i = 0; i < 256; i++)
                {
                    var scale = Math.ScaleB(1, exponent);
                    var a = new[] { random.Next(-6,7) * scale, random.Next(-6,7) * scale, random.Next(-6,7) * scale };
                    var b = new[] { random.Next(-6,7) * scale, random.Next(-6,7) * scale, random.Next(-6,7) * scale };
                    core.CheckExact(a, random.Next(0,5) * scale, b, random.Next(0,5) * scale, $"seed=149123+dimension, exponent={exponent}, case={i}");
                }
            }
        }

        [Test]
        public void RadialPairsPreserveOrdinaryAndInvalidResults()
        {
            foreach (var core in RadialCores())
            {
                var random = new Random(149131 + core.Dimension);
                for (var i = 0; i < 1024; i++)
                {
                    var a = new[] { random.Next(-64,65) * .25, random.Next(-64,65) * .25, random.Next(-64,65) * .25 };
                    var b = new[] { random.Next(-64,65) * .25, random.Next(-64,65) * .25, random.Next(-64,65) * .25 };
                    core.CheckLegacy(a, random.Next(1,65) * .25, b, random.Next(1,65) * .25, "ordinary seed=149131+dimension, case=" + i);
                }
                var values = new[] { -1.0, -0.0, 0, 1, -(core.Wide ? double.Epsilon : float.Epsilon), core.Wide ? double.Epsilon : float.Epsilon, -(core.Wide ? double.MaxValue : float.MaxValue), core.Wide ? double.MaxValue : float.MaxValue, double.NaN, double.PositiveInfinity, double.NegativeInfinity };
                foreach (var coordinate in values)
                foreach (var r0 in values)
                foreach (var r1 in values)
                    if (!double.IsFinite(coordinate) || !(r0 >= 0 && double.IsFinite(r0)) || !(r1 >= 0 && double.IsFinite(r1)))
                        core.CheckLegacy(new[] { coordinate, 0.0, 0.0 }, r0, new[] { 1.0, 0.0, 0.0 }, r1, "legacy invalid inputs");
            }
        }

        private sealed class RadialCore
        {
            public readonly bool Wide;
            public readonly int Dimension;
            public readonly int[] Exponents;
            private readonly string name;
            private readonly ConstructorInfo vector, bound;
            private readonly MethodInfo intersects;
            public RadialCore(Assembly assembly, bool wide, int dimension, string target)
            {
                Wide = wide; Dimension = dimension; name = target + (wide ? "/double/" : "/float/") + dimension;
                Exponents = wide ? new[] { -1074,-1073,-1070,-1060,-1022,-900,-600,-540,-538,-537,-520,-512,-511,-300,-40,0,40,300,510,511,512,513,900,1018,1019,1020 }
                    : new[] { -149,-148,-146,-140,-126,-100,-80,-75,-74,-65,-64,-63,-20,0,20,60,62,63,64,65,100,123,124 };
                var suffix = wide ? "d" : "f"; var scalar = wide ? typeof(double) : typeof(float);
                var v = assembly.GetType("Aardvark.Base.V" + dimension + suffix, true);
                var b = assembly.GetType("Aardvark.Base." + (dimension == 2 ? "Circle2" : "Sphere3") + suffix, true);
                Assert.AreSame(assembly, v.Assembly, name); Assert.AreSame(assembly, b.Assembly, name);
                vector = v.GetConstructor(dimension == 2 ? new[] { scalar,scalar } : new[] { scalar,scalar,scalar });
                bound = b.GetConstructor(new[] { v,scalar });
                intersects = assembly.GetType("Aardvark.Base.GeometryFun",true).GetMethod("Intersects",new[] { b,b });
                Assert.IsNotNull(vector,name); Assert.IsNotNull(bound,name); Assert.IsNotNull(intersects,name);
            }
            private double Scalar(double value) => Wide ? value : (float)value;
            private object BoxScalar(double value) => Wide ? (object)value : (float)value;
            private object Bound(double[] p, double radius)
            {
                var values = new object[Dimension];
                for (var i = 0; i < Dimension; i++) values[i] = BoxScalar(p[i]);
                return bound.Invoke(new[] { vector.Invoke(values),BoxScalar(radius) });
            }
            private void Check(double[] a, double r0, double[] b, double r1, bool expected, string label)
            {
                var x = Bound(a,r0); var y = Bound(b,r1);
                var context = $"{name}: {label}; a=({string.Join(",",a)}), r0={r0:R}; b=({string.Join(",",b)}), r1={r1:R}";
                Assert.AreEqual(expected,intersects.Invoke(null,new[] { x,y }),context);
                Assert.AreEqual(expected,intersects.Invoke(null,new[] { y,x }),context + "; swapped");
            }
            public void CheckExact(double[] a, double r0, double[] b, double r1, string label)
            {
                // Exact IEEE dyadic values represented as integers in units of 2^-1074.
                BigInteger Integer(double value)
                {
                    value = Scalar(value); Assert.IsTrue(double.IsFinite(value),name + " finite fixture");
                    var bits = BitConverter.DoubleToInt64Bits(value); var exponent = (int)((bits >> 52) & 2047);
                    var significand = new BigInteger(bits & 0xfffffffffffffL);
                    if (exponent != 0) significand = (significand + (BigInteger.One << 52)) << (exponent - 1);
                    return bits < 0 ? -significand : significand;
                }
                var distanceSquared = BigInteger.Zero;
                for (var i = 0; i < Dimension; i++) { var d = Integer(a[i]) - Integer(b[i]); distanceSquared += d*d; }
                var radius = Integer(r0) + Integer(r1);
                Check(a,r0,b,r1,distanceSquared <= radius*radius,label);
            }
            public void CheckLegacy(double[] a, double r0, double[] b, double r1, string label)
            {
                bool expected;
                if (Wide)
                {
                    var x=a[0]-b[0]; var y=a[1]-b[1]; var z=a[2]-b[2]; var radius=r0+r1;
                    expected = (Dimension == 2 ? x*x+y*y : x*x+y*y+z*z) <= radius*radius;
                }
                else
                {
                    var x=(float)a[0]-(float)b[0]; var y=(float)a[1]-(float)b[1]; var z=(float)a[2]-(float)b[2]; var radius=(float)r0+(float)r1;
                    expected = (Dimension == 2 ? x*x+y*y : x*x+y*y+z*z) <= radius*radius;
                }
                Check(a,r0,b,r1,expected,label);
            }
        }
        private static IEnumerable<RadialCore> RadialCores()
        {
            foreach (var target in new[] { "net10.0","netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Circle2d).Assembly.Location),"..",target,"Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path),"Required sibling core: " + path); var assembly = Assembly.LoadFile(path);
                Assert.AreEqual(target == "net10.0" ? ".NETCoreApp,Version=v10.0" : ".NETStandard,Version=v2.0",assembly.GetCustomAttribute<TargetFrameworkAttribute>().FrameworkName,path);
                foreach (var wide in new[] { false,true }) foreach (var dimension in new[] { 2,3 }) yield return new RadialCore(assembly,wide,dimension,target);
            }
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
