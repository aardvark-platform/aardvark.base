using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Aardvark.Tests.Geometry
{

    [TestFixture]
    public class PolygonClipping : TestSuite
    {
        public PolygonClipping() : base() { }
        public PolygonClipping(TestSuite.Options options) : base(options) { }

        private static readonly Polygon2d s_squareD = new Polygon2d(
            new V2d(0.0, 0.0),
            new V2d(1.0, 0.0),
            new V2d(1.0, 1.0),
            new V2d(0.0, 1.0)
        );

        private static readonly Polygon2f s_squareF = new Polygon2f(
            new V2f(0.0f, 0.0f),
            new V2f(1.0f, 0.0f),
            new V2f(1.0f, 1.0f),
            new V2f(0.0f, 1.0f)
        );

        private static void AssertLine(Line2d actual, V2d p0, V2d p1)
        {
            Assert.That(actual.P0, Is.EqualTo(p0));
            Assert.That(actual.P1, Is.EqualTo(p1));
        }

        private static void AssertLine(Line2f actual, V2f p0, V2f p1)
        {
            Assert.That(actual.P0, Is.EqualTo(p0));
            Assert.That(actual.P1, Is.EqualTo(p1));
        }

        private static void AssertRejected(Line2d actual)
        {
            Assert.That(double.IsNaN(actual.P0.X), Is.True);
            Assert.That(double.IsNaN(actual.P0.Y), Is.True);
            Assert.That(double.IsNaN(actual.P1.X), Is.True);
            Assert.That(double.IsNaN(actual.P1.Y), Is.True);
        }

        private static void AssertRejected(Line2f actual)
        {
            Assert.That(float.IsNaN(actual.P0.X), Is.True);
            Assert.That(float.IsNaN(actual.P0.Y), Is.True);
            Assert.That(float.IsNaN(actual.P1.X), Is.True);
            Assert.That(float.IsNaN(actual.P1.Y), Is.True);
        }

        [Test]
        public void Issue62BoundarySegmentFromExactBitsIsNotRejected()
        {
            var polygonBits = new[]
            {
                new V2l(-4631936373040510188, -4625820201104931391),
                new V2l(4596112126756858047, -4625819553296130844),
                new V2l(4594971118245019810, 4598486623334369124),
                new V2l(-4631938632516426832, 4598486911425280084),
            };

            var lineP0Bits = new V2l(4593502233478970048, -4625819711659140416);
            var lineP1Bits = new V2l(4593504492954886720, 4598486666702384608);
            var polygon = new Polygon2d(polygonBits.Map(BitsToDouble));
            var line = new Line2d(BitsToDouble(lineP0Bits), BitsToDouble(lineP1Bits));

            Assert.That(line.ClipWithConvex(polygon), Is.EqualTo(line));
        }

        [Test]
        public void InsideSegmentsPreserveExactEndpoints()
        {
            var lineD = new Line2d(new V2d(0.25, 0.25), new V2d(0.75, 0.75));
            Assert.That(lineD.ClipWithConvex(s_squareD), Is.EqualTo(lineD));

            var lineF = new Line2f(new V2f(0.25f, 0.25f), new V2f(0.75f, 0.75f));
            Assert.That(lineF.ClipWithConvex(s_squareF), Is.EqualTo(lineF));
        }

        [Test]
        public void OneSidedClippingPreservesInsideEndpoint()
        {
            AssertLine(
                new Line2d(new V2d(-1.0, 0.5), new V2d(0.5, 0.5)).ClipWithConvex(s_squareD, 0.0),
                new V2d(0.0, 0.5), new V2d(0.5, 0.5)
            );
            AssertLine(
                new Line2f(new V2f(-1.0f, 0.5f), new V2f(0.5f, 0.5f)).ClipWithConvex(s_squareF, 0.0f),
                new V2f(0.0f, 0.5f), new V2f(0.5f, 0.5f)
            );
        }

        [Test]
        public void TwoSidedClippingReturnsOrderedInteriorSegment()
        {
            AssertLine(
                new Line2d(new V2d(-1.0, 0.5), new V2d(2.0, 0.5)).ClipWithConvex(s_squareD, 0.0),
                new V2d(0.0, 0.5), new V2d(1.0, 0.5)
            );
            AssertLine(
                new Line2f(new V2f(-1.0f, 0.5f), new V2f(2.0f, 0.5f)).ClipWithConvex(s_squareF, 0.0f),
                new V2f(0.0f, 0.5f), new V2f(1.0f, 0.5f)
            );
        }

        [Test]
        public void OutsideSegmentsAreRejected()
        {
            AssertRejected(
                new Line2d(new V2d(-1.0, 2.0), new V2d(2.0, 2.0)).ClipWithConvex(s_squareD)
            );
            AssertRejected(
                new Line2f(new V2f(-1.0f, 2.0f), new V2f(2.0f, 2.0f)).ClipWithConvex(s_squareF)
            );
        }

        [Test]
        public void CollinearBoundarySegmentsAreClippedInclusively()
        {
            AssertLine(
                new Line2d(new V2d(-1.0, 0.0), new V2d(2.0, 0.0)).ClipWithConvex(s_squareD, 0.0),
                new V2d(0.0, 0.0), new V2d(1.0, 0.0)
            );
            AssertLine(
                new Line2f(new V2f(-1.0f, 0.0f), new V2f(2.0f, 0.0f)).ClipWithConvex(s_squareF, 0.0f),
                new V2f(0.0f, 0.0f), new V2f(1.0f, 0.0f)
            );
        }

        [Test]
        public void VertexTangencyReturnsPointSegment()
        {
            AssertLine(
                new Line2d(new V2d(-1.0, 1.0), new V2d(1.0, -1.0)).ClipWithConvex(s_squareD, 0.0),
                V2d.Zero, V2d.Zero
            );
            AssertLine(
                new Line2f(new V2f(-1.0f, 1.0f), new V2f(1.0f, -1.0f)).ClipWithConvex(s_squareF, 0.0f),
                V2f.Zero, V2f.Zero
            );
        }

        [Test]
        public void ReversedSegmentsPreserveOriginalOrdering()
        {
            AssertLine(
                new Line2d(new V2d(2.0, 0.5), new V2d(-1.0, 0.5)).ClipWithConvex(s_squareD, 0.0),
                new V2d(1.0, 0.5), new V2d(0.0, 0.5)
            );
            AssertLine(
                new Line2f(new V2f(2.0f, 0.5f), new V2f(-1.0f, 0.5f)).ClipWithConvex(s_squareF, 0.0f),
                new V2f(1.0f, 0.5f), new V2f(0.0f, 0.5f)
            );
        }

        [Test]
        public void DuplicatePolygonVerticesAreIgnored()
        {
            var polygonD = new Polygon2d(
                new V2d(0.0, 0.0), new V2d(1.0, 0.0), new V2d(1.0, 0.0),
                new V2d(1.0, 1.0), new V2d(0.0, 1.0), new V2d(0.0, 1.0)
            );
            AssertLine(
                new Line2d(new V2d(-1.0, 0.5), new V2d(2.0, 0.5)).ClipWithConvex(polygonD, 0.0),
                new V2d(0.0, 0.5), new V2d(1.0, 0.5)
            );

            var polygonF = new Polygon2f(
                new V2f(0.0f, 0.0f), new V2f(1.0f, 0.0f), new V2f(1.0f, 0.0f),
                new V2f(1.0f, 1.0f), new V2f(0.0f, 1.0f), new V2f(0.0f, 1.0f)
            );
            AssertLine(
                new Line2f(new V2f(-1.0f, 0.5f), new V2f(2.0f, 0.5f)).ClipWithConvex(polygonF, 0.0f),
                new V2f(0.0f, 0.5f), new V2f(1.0f, 0.5f)
            );
        }

        [Test]
        public void AbsoluteEpsilonDefinesPointDistanceTolerance()
        {
            var lineD = new Line2d(new V2d(0.25, -0.05), new V2d(0.75, -0.05));
            Assert.That(lineD.ClipWithConvex(s_squareD, 0.1), Is.EqualTo(lineD));
            AssertRejected(lineD.ClipWithConvex(s_squareD, 0.01));

            var lineF = new Line2f(new V2f(0.25f, -0.05f), new V2f(0.75f, -0.05f));
            Assert.That(lineF.ClipWithConvex(s_squareF, 0.1f), Is.EqualTo(lineF));
            AssertRejected(lineF.ClipWithConvex(s_squareF, 0.01f));
        }

        [Test]
        public void ExtremeFiniteEdgeScalesPreserveHalfSpaces()
        {
            foreach (var width in new[] { 1e-200, 1e200 })
            {
                var polygon = new Polygon2d(
                    V2d.Zero,
                    new V2d(width, 0.0),
                    new V2d(width, 1.0),
                    V2d.YAxis
                );
                var x = 0.5 * width;
                var inside = new Line2d(new V2d(x, 0.25), new V2d(x, 0.75));
                Assert.That(inside.ClipWithConvex(polygon, 0.0), Is.EqualTo(inside));

                var nearBoundary = new Line2d(new V2d(x, -0.05), new V2d(x, -0.025));
                Assert.That(nearBoundary.ClipWithConvex(polygon, 0.1), Is.EqualTo(nearBoundary));
                AssertRejected(nearBoundary.ClipWithConvex(polygon, 0.01));
                AssertRejected(
                    new Line2d(new V2d(x, -1.0), new V2d(x, -0.5))
                        .ClipWithConvex(polygon, 0.0)
                );
            }

            foreach (var width in new[] { 1e-30f, 1e20f })
            {
                var polygon = new Polygon2f(
                    V2f.Zero,
                    new V2f(width, 0.0f),
                    new V2f(width, 1.0f),
                    V2f.YAxis
                );
                var x = 0.5f * width;
                var inside = new Line2f(new V2f(x, 0.25f), new V2f(x, 0.75f));
                Assert.That(inside.ClipWithConvex(polygon, 0.0f), Is.EqualTo(inside));

                var nearBoundary = new Line2f(new V2f(x, -0.05f), new V2f(x, -0.025f));
                Assert.That(nearBoundary.ClipWithConvex(polygon, 0.1f), Is.EqualTo(nearBoundary));
                AssertRejected(nearBoundary.ClipWithConvex(polygon, 0.01f));
                AssertRejected(
                    new Line2f(new V2f(x, -1.0f), new V2f(x, -0.5f))
                        .ClipWithConvex(polygon, 0.0f)
                );
            }
        }

        [Test]
        public void BoxClippingForwardsLooseTolerance()
        {
            foreach (var core in BoxClippingCores())
            foreach (var face in BoxFaces(core.Dimension))
            foreach (var gap in new[] { 0.03125, 0.0625, 0.125, 0.5 })
            {
                var points = FacePolygon(core.Dimension, face.Bounds, face.Axis, face.Max,
                    new[] { -gap, -gap, -gap / 2 }, new[] { 0.25, 0.75, 0.5 });
                var result = core.Clip(points, face.Bounds, 0.125);
                var context = $"{core.Name}: {face.Name}, loose gap={gap:R}";
                var expected = gap <= 0.125 ? points : Array.Empty<double>();
                Assert.That(result.Box, Is.EqualTo(expected), context + " retained/rejected vertices");
                Assert.That(result.Box, Is.EqualTo(result.Planes), context + " ordered plane equivalence");
                Assert.That(result.Omitted, Is.Empty, context + " omitted default rejects");
                Assert.That(result.Default, Is.EqualTo(result.Omitted), context + " explicit default");
            }
        }

        [Test]
        public void ZeroBoxToleranceRejectsSubDefaultOutsideVertices()
        {
            foreach (var core in BoxClippingCores())
            foreach (var face in BoxFaces(core.Dimension))
            foreach (var multiplier in new[] { 0.5, 1.0, 2.0 })
            {
                var gap = Math.ScaleB(multiplier, core.Wide ? -28 : -18);
                var points = FacePolygon(core.Dimension, face.Bounds, face.Axis, face.Max,
                    new[] { -gap, -gap, -gap / 2 }, new[] { 0.25, 0.75, 0.5 });
                var result = core.Clip(points, face.Bounds, 0);
                var context = $"{core.Name}: {face.Name}, zero tolerance, gap={gap:R}";
                Assert.That(result.Box, Is.Empty, context + " strict rejection");
                Assert.That(result.Planes, Is.Empty, context + " strict plane rejection");
                Assert.That(result.Omitted, Is.EqualTo(points), context + " omitted default retains triangle");
                Assert.That(result.Default, Is.EqualTo(result.Omitted), context + " explicit default");
            }
        }

        [Test]
        public void BoxClippingKeepsDefaultAndOrdinaryGeometry()
        {
            foreach (var core in BoxClippingCores())
            foreach (var face in BoxFaces(core.Dimension))
            foreach (var kind in new[] { "inside", "crossing", "outside" })
            foreach (var eps in new[] { 0.0, core.DefaultEpsilon, 0.125 })
            {
                var u = kind == "inside" ? new[] { 0.25, 0.75, 0.5 }
                    : kind == "crossing" ? new[] { -0.5, 0.5, 0.5 }
                    : new[] { -0.5, -0.5, -0.25 };
                var v = kind == "outside" ? new[] { 0.25, 0.75, 0.5 } : new[] { 0.25, 0.25, 0.75 };
                var points = FacePolygon(core.Dimension, face.Bounds, face.Axis, face.Max, u, v);
                var expected = kind == "inside" ? points : kind == "outside" ? Array.Empty<double>()
                    : FacePolygon(core.Dimension, face.Bounds, face.Axis, face.Max,
                        new[] { 0.0, 0.5, 0.5, 0.0 }, new[] { 0.25, 0.25, 0.75, 0.5 });
                var result = core.Clip(points, face.Bounds, eps);
                var context = $"{core.Name}: {face.Name}, {kind}, eps={eps:R}";
                Assert.That(result.Box, Is.EqualTo(expected), context + " explicit outcome/order");
                Assert.That(result.Box, Is.EqualTo(result.Planes), context + " ordered plane equivalence");
                Assert.That(result.Omitted, Is.EqualTo(expected), context + " omitted default");
                Assert.That(result.Default, Is.EqualTo(result.Omitted), context + " explicit default");
            }
        }

        private static IEnumerable<(double[] Bounds, int Axis, bool Max, string Name)> BoxFaces(int dimension)
        {
            foreach (var origin in new[] { new[] { 0.0, 0.0, 0.0 }, new[] { 2.0, -4.0, 6.0 },
                new[] { -8.0, 4.0, -2.0 }, new[] { 0.5, -0.25, 1.5 } })
            {
                var bounds = new double[2 * dimension];
                for (var i = 0; i < dimension; i++) { bounds[i] = origin[i]; bounds[i + dimension] = origin[i] + 1; }
                for (var axis = 0; axis < dimension; axis++)
                foreach (var max in new[] { false, true })
                    yield return (bounds, axis, max, $"origin=({string.Join(",", origin)}), axis={axis}, max={max}");
            }
        }

        // Coordinates are constructed in an inward normal/tangent frame, so the retained
        // and crossing expectations are analytic, not inferred solely from the plane API.
        private static double[] FacePolygon(int dimension, double[] bounds, int axis, bool max,
            double[] normal, double[] tangent)
        {
            var points = new double[dimension * normal.Length];
            for (var i = 0; i < normal.Length; i++)
            for (var k = 0; k < dimension; k++)
            {
                var offset = k == axis ? (max ? 1 - normal[i] : normal[i])
                    : k == (axis + 1) % dimension ? tangent[i] : 0.5;
                points[i * dimension + k] = bounds[k] + offset;
            }
            return points;
        }

        private sealed class BoxClipCore
        {
            public readonly string Name;
            public readonly bool Wide;
            public readonly int Dimension;
            public readonly double DefaultEpsilon;
            private readonly Type vector, plane, polygon;
            private readonly ConstructorInfo vectorCtor, planeCtor, polygonCtor, boxCtor;
            private readonly MethodInfo boxClip, planeClip, readPoints;
            private readonly FieldInfo[] coordinates;

            public BoxClipCore(Assembly assembly, string target, int dimension, bool wide)
            {
                Dimension = dimension; Wide = wide;
                var suffix = dimension.ToString() + (wide ? "d" : "f");
                Name = target + "/Polygon" + suffix;
                var scalar = wide ? typeof(double) : typeof(float);
                vector = assembly.GetType("Aardvark.Base.V" + suffix, true);
                plane = assembly.GetType("Aardvark.Base.Plane" + suffix, true);
                polygon = assembly.GetType("Aardvark.Base.Polygon" + suffix, true);
                var box = assembly.GetType("Aardvark.Base.Box" + suffix, true);
                var extensions = assembly.GetType("Aardvark.Base.Polygon" + suffix + "Extensions", true);
                var types = new Type[dimension]; for (var k = 0; k < dimension; k++) types[k] = scalar;
                vectorCtor = vector.GetConstructor(types);
                planeCtor = plane.GetConstructor(new[] { vector, vector });
                polygonCtor = polygon.GetConstructor(new[] { vector.MakeArrayType() });
                boxCtor = box.GetConstructor(new[] { vector, vector });
                boxClip = extensions.GetMethod("ConvexClipped", new[] { polygon, box, scalar });
                planeClip = extensions.GetMethod("ConvexClipped", new[] { polygon, plane.MakeArrayType(), scalar });
                readPoints = polygon.GetMethod("GetPointArray", Type.EmptyTypes);
                coordinates = new FieldInfo[dimension];
                for (var k = 0; k < dimension; k++) coordinates[k] = vector.GetField("XYZ"[k].ToString());
                DefaultEpsilon = wide ? 1e-8 : (double)1e-5f;
                Assert.AreEqual(DefaultEpsilon, Convert.ToDouble(boxClip.GetParameters()[2].DefaultValue), Name + " default metadata");
            }

            private object Scalar(double x) => Wide ? (object)x : (object)(float)x;
            private object Vector(double[] values, int offset)
            {
                var args = new object[Dimension];
                for (var k = 0; k < Dimension; k++) args[k] = Scalar(values[offset + k]);
                return vectorCtor.Invoke(args);
            }
            private double[] Points(object result)
            {
                var array = (Array)readPoints.Invoke(result, null);
                var values = new double[array.Length * Dimension];
                for (var i = 0; i < array.Length; i++)
                for (var k = 0; k < Dimension; k++) values[i * Dimension + k] = Convert.ToDouble(coordinates[k].GetValue(array.GetValue(i)));
                return values;
            }
            public (double[] Box, double[] Planes, double[] Omitted, double[] Default) Clip(double[] points, double[] bounds, double eps)
            {
                var ownPoints = Array.CreateInstance(vector, points.Length / Dimension);
                for (var i = 0; i < ownPoints.Length; i++) ownPoints.SetValue(Vector(points, i * Dimension), i);
                var poly = polygonCtor.Invoke(new object[] { ownPoints });
                var min = Vector(bounds, 0); var max = Vector(bounds, Dimension);
                var box = boxCtor.Invoke(new[] { min, max });
                var planes = Array.CreateInstance(plane, 2 * Dimension);
                for (var axis = 0; axis < Dimension; axis++)
                {
                    var normal = new double[Dimension]; normal[axis] = 1;
                    planes.SetValue(planeCtor.Invoke(new[] { Vector(normal, 0), min }), 2 * axis);
                    normal[axis] = -1;
                    planes.SetValue(planeCtor.Invoke(new[] { Vector(normal, 0), max }), 2 * axis + 1);
                }
                return (Points(boxClip.Invoke(null, new[] { poly, box, Scalar(eps) })),
                    Points(planeClip.Invoke(null, new[] { poly, planes, Scalar(eps) })),
                    Points(boxClip.Invoke(null, new[] { poly, box, Type.Missing })),
                    Points(boxClip.Invoke(null, new[] { poly, box, Scalar(DefaultEpsilon) })));
            }
        }

        private static IEnumerable<BoxClipCore> BoxClippingCores()
        {
            foreach (var target in new[] { "net10.0", "netstandard2.0" })
            {
                var path = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(typeof(Polygon2d).Assembly.Location),
                    "..", target, "Aardvark.Base.dll"));
                Assert.IsTrue(File.Exists(path), "Required core missing: " + path);
                var assembly = Assembly.LoadFile(path);
                foreach (var dimension in new[] { 2, 3 })
                foreach (var wide in new[] { false, true })
                    yield return new BoxClipCore(assembly, target, dimension, wide);
            }
        }

        [Test]
        public void TestConvexClipped()
        {
            var points = new[] { V3d.OOO, V3d.IOO, V3d.OIO };
            var poly = new Polygon3d(points);
            var box = Box3d.FromMinAndSize(-V3d.OOI, new V3d(1, 0.5, 2));
            var newHull = new Hull3d(box).Reversed(); // requires non-intuitive reversed
            var polyClipped = poly.ConvexClipped(newHull); // will return positive part of planes (outside of Hull3d)
            var clippedBox = polyClipped.BoundingBox3d;
            var test = new Box3d(0, 0, 0, 1, 0.5, 0);
            Test.IsTrue(clippedBox == test);
        }

        private static V2d BitsToDouble(V2l bits)
            => new V2d(
                BitConverter.Int64BitsToDouble(bits.X),
                BitConverter.Int64BitsToDouble(bits.Y)
            );
    }
}
