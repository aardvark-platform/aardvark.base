using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class BoxPlaneIntersectionTests
    {
        private static readonly Box2d s_boxDouble = new(V2d.Zero, new V2d(2.0, 2.0));
        private static readonly Box2f s_boxFloat = new(V2f.Zero, new V2f(2.0f, 2.0f));

        [Test]
        public void DoubleCrossingsAreScaleInvariantAndPreserveOrdering()
        {
            foreach (double scale in new[] { 1.0, 7.0, -3.0, 1e-20 })
            {
                AssertHit(
                    s_boxDouble,
                    new Plane2d(new V2d(scale, scale), 2.0 * scale),
                    new V2d(2.0, 0.0),
                    new V2d(0.0, 2.0));

                AssertHit(
                    s_boxDouble,
                    new Plane2d(new V2d(scale, -scale), 0.0),
                    new V2d(0.0, 0.0),
                    new V2d(2.0, 2.0));
            }
        }

        [Test]
        public void FloatCrossingsAreScaleInvariantAndPreserveOrdering()
        {
            foreach (float scale in new[] { 1.0f, 7.0f, -3.0f, 1e-10f })
            {
                AssertHit(
                    s_boxFloat,
                    new Plane2f(new V2f(scale, scale), 2.0f * scale),
                    new V2f(2.0f, 0.0f),
                    new V2f(0.0f, 2.0f));

                AssertHit(
                    s_boxFloat,
                    new Plane2f(new V2f(scale, -scale), 0.0f),
                    new V2f(0.0f, 0.0f),
                    new V2f(2.0f, 2.0f));
            }
        }

        [Test]
        public void AxisAlignedPlanesUseTheirFullEquation()
        {
            AssertHit(s_boxDouble, new Plane2d(new V2d(0.0, -4.0), -4.0), new V2d(0.0, 1.0), new V2d(2.0, 1.0));
            AssertHit(s_boxDouble, new Plane2d(new V2d(-3.0, 0.0), -3.0), new V2d(1.0, 0.0), new V2d(1.0, 2.0));
            AssertMiss(s_boxDouble, new Plane2d(new V2d(0.0, 2.0), 6.0));

            AssertHit(s_boxFloat, new Plane2f(new V2f(0.0f, -4.0f), -4.0f), new V2f(0.0f, 1.0f), new V2f(2.0f, 1.0f));
            AssertHit(s_boxFloat, new Plane2f(new V2f(-3.0f, 0.0f), -3.0f), new V2f(1.0f, 0.0f), new V2f(1.0f, 2.0f));
            AssertMiss(s_boxFloat, new Plane2f(new V2f(0.0f, 2.0f), 6.0f));
        }

        [Test]
        public void EdgeAndCornerContactUseClosedSemantics()
        {
            AssertHit(s_boxDouble, new Plane2d(new V2d(1.0, 0.0), 0.0), new V2d(0.0, 0.0), new V2d(0.0, 2.0));
            AssertHit(s_boxDouble, new Plane2d(new V2d(1.0, 1.0), 0.0), V2d.Zero, V2d.Zero);
            AssertHit(s_boxDouble, new Plane2d(new V2d(1.0, 1.0), 4.0), new V2d(2.0, 2.0), new V2d(2.0, 2.0));

            AssertHit(s_boxFloat, new Plane2f(new V2f(1.0f, 0.0f), 0.0f), new V2f(0.0f, 0.0f), new V2f(0.0f, 2.0f));
            AssertHit(s_boxFloat, new Plane2f(new V2f(1.0f, 1.0f), 0.0f), V2f.Zero, V2f.Zero);
            AssertHit(s_boxFloat, new Plane2f(new V2f(1.0f, 1.0f), 4.0f), new V2f(2.0f, 2.0f), new V2f(2.0f, 2.0f));
        }

        [Test]
        public void DegenerateBoxesProduceSegmentsOrPoints()
        {
            var lineBoxDouble = new Box2d(new V2d(0.0, 1.0), new V2d(2.0, 1.0));
            AssertHit(lineBoxDouble, new Plane2d(new V2d(0.0, 2.0), 2.0), lineBoxDouble.Min, lineBoxDouble.Max);
            AssertHit(lineBoxDouble, new Plane2d(new V2d(1.0, 0.0), 1.0), new V2d(1.0, 1.0), new V2d(1.0, 1.0));

            var pointBoxDouble = new Box2d(new V2d(1.0, 1.0), new V2d(1.0, 1.0));
            AssertHit(pointBoxDouble, new Plane2d(new V2d(1.0, 1.0), 2.0), pointBoxDouble.Min, pointBoxDouble.Min);
            AssertMiss(pointBoxDouble, new Plane2d(new V2d(1.0, 1.0), 3.0));

            var lineBoxFloat = new Box2f(new V2f(0.0f, 1.0f), new V2f(2.0f, 1.0f));
            AssertHit(lineBoxFloat, new Plane2f(new V2f(0.0f, 2.0f), 2.0f), lineBoxFloat.Min, lineBoxFloat.Max);
            AssertHit(lineBoxFloat, new Plane2f(new V2f(1.0f, 0.0f), 1.0f), new V2f(1.0f, 1.0f), new V2f(1.0f, 1.0f));

            var pointBoxFloat = new Box2f(new V2f(1.0f, 1.0f), new V2f(1.0f, 1.0f));
            AssertHit(pointBoxFloat, new Plane2f(new V2f(1.0f, 1.0f), 2.0f), pointBoxFloat.Min, pointBoxFloat.Min);
            AssertMiss(pointBoxFloat, new Plane2f(new V2f(1.0f, 1.0f), 3.0f));
        }

        [Test]
        public void InvalidInputsMissAndLeaveDefaultOutput()
        {
            AssertMiss(Box2d.Invalid, new Plane2d(V2d.XAxis, 0.0));
            AssertMiss(new Box2d(new V2d(2.0, 0.0), new V2d(1.0, 1.0)), new Plane2d(V2d.XAxis, 0.0));
            AssertMiss(new Box2d(V2d.Zero, new V2d(double.PositiveInfinity, 1.0)), new Plane2d(V2d.XAxis, 0.0));
            AssertMiss(s_boxDouble, Plane2d.Invalid);
            AssertMiss(s_boxDouble, new Plane2d(new V2d(double.NaN, 1.0), 0.0));
            AssertMiss(s_boxDouble, new Plane2d(V2d.XAxis, double.PositiveInfinity));

            AssertMiss(Box2f.Invalid, new Plane2f(V2f.XAxis, 0.0f));
            AssertMiss(new Box2f(new V2f(2.0f, 0.0f), new V2f(1.0f, 1.0f)), new Plane2f(V2f.XAxis, 0.0f));
            AssertMiss(new Box2f(V2f.Zero, new V2f(float.PositiveInfinity, 1.0f)), new Plane2f(V2f.XAxis, 0.0f));
            AssertMiss(s_boxFloat, Plane2f.Invalid);
            AssertMiss(s_boxFloat, new Plane2f(new V2f(float.NaN, 1.0f), 0.0f));
            AssertMiss(s_boxFloat, new Plane2f(V2f.XAxis, float.PositiveInfinity));

            Assert.That(GeometryFun.Intersects(1.0, 0.0, 0.0, 2.0, 0.0, 1.0, 1.0, out Line2d lineDouble), Is.False);
            Assert.That(lineDouble, Is.EqualTo(default(Line2d)));
            Assert.That(GeometryFun.Intersects(1.0f, 0.0f, 0.0f, 2.0f, 0.0f, 1.0f, 1.0f, out Line2f lineFloat), Is.False);
            Assert.That(lineFloat, Is.EqualTo(default(Line2f)));
        }

        [Test]
        public void DoubleBooleanAndSegmentOverloadsAgree()
        {
            V2d[] normals =
            {
                new V2d(1.0, 0.0), new V2d(0.0, 1.0), new V2d(1.0, 1.0),
                new V2d(2.0, -1.0), new V2d(-0.25, 3.0), new V2d(-4.0, -2.0)
            };

            foreach (V2d normal in normals)
            {
                for (int i = -4; i <= 12; i++)
                {
                    var plane = new Plane2d(normal, i * 0.5);
                    bool expected = ReferenceIntersects(s_boxDouble, plane);
                    Assert.That(s_boxDouble.Intersects(plane), Is.EqualTo(expected), $"boolean {normal}, {plane.Distance}");
                    Assert.That(s_boxDouble.Intersects(plane, out Line2d line), Is.EqualTo(expected), $"segment {normal}, {plane.Distance}");
                    if (expected) AssertLine(s_boxDouble, plane, line);
                    else Assert.That(line, Is.EqualTo(default(Line2d)));
                }
            }
        }

        [Test]
        public void FloatBooleanAndSegmentOverloadsAgree()
        {
            V2f[] normals =
            {
                new V2f(1.0f, 0.0f), new V2f(0.0f, 1.0f), new V2f(1.0f, 1.0f),
                new V2f(2.0f, -1.0f), new V2f(-0.25f, 3.0f), new V2f(-4.0f, -2.0f)
            };

            foreach (V2f normal in normals)
            {
                for (int i = -4; i <= 12; i++)
                {
                    var plane = new Plane2f(normal, i * 0.5f);
                    bool expected = ReferenceIntersects(s_boxFloat, plane);
                    Assert.That(s_boxFloat.Intersects(plane), Is.EqualTo(expected), $"boolean {normal}, {plane.Distance}");
                    Assert.That(s_boxFloat.Intersects(plane, out Line2f line), Is.EqualTo(expected), $"segment {normal}, {plane.Distance}");
                    if (expected) AssertLine(s_boxFloat, plane, line);
                    else Assert.That(line, Is.EqualTo(default(Line2f)));
                }
            }
        }

        private static void AssertHit(Box2d box, Plane2d plane, V2d expected0, V2d expected1)
        {
            Assert.That(box.Intersects(plane), Is.True);
            Assert.That(box.Intersects(plane, out Line2d line), Is.True);
            AssertV2(expected0, line.P0, 1e-12);
            AssertV2(expected1, line.P1, 1e-12);
            AssertLine(box, plane, line);
        }

        private static void AssertHit(Box2f box, Plane2f plane, V2f expected0, V2f expected1)
        {
            Assert.That(box.Intersects(plane), Is.True);
            Assert.That(box.Intersects(plane, out Line2f line), Is.True);
            AssertV2(expected0, line.P0, 2e-5f);
            AssertV2(expected1, line.P1, 2e-5f);
            AssertLine(box, plane, line);
        }

        private static void AssertMiss(Box2d box, Plane2d plane)
        {
            Assert.That(box.Intersects(plane), Is.False);
            Assert.That(box.Intersects(plane, out Line2d line), Is.False);
            Assert.That(line, Is.EqualTo(default(Line2d)));
        }

        private static void AssertMiss(Box2f box, Plane2f plane)
        {
            Assert.That(box.Intersects(plane), Is.False);
            Assert.That(box.Intersects(plane, out Line2f line), Is.False);
            Assert.That(line, Is.EqualTo(default(Line2f)));
        }

        private static bool ReferenceIntersects(Box2d box, Plane2d plane)
        {
            double d0 = Vec.Dot(plane.Normal, box.Min);
            double d1 = Vec.Dot(plane.Normal, new V2d(box.Max.X, box.Min.Y));
            double d2 = Vec.Dot(plane.Normal, box.Max);
            double d3 = Vec.Dot(plane.Normal, new V2d(box.Min.X, box.Max.Y));
            double min = Math.Min(Math.Min(d0, d1), Math.Min(d2, d3));
            double max = Math.Max(Math.Max(d0, d1), Math.Max(d2, d3));
            return min <= plane.Distance && plane.Distance <= max;
        }

        private static bool ReferenceIntersects(Box2f box, Plane2f plane)
        {
            float d0 = Vec.Dot(plane.Normal, box.Min);
            float d1 = Vec.Dot(plane.Normal, new V2f(box.Max.X, box.Min.Y));
            float d2 = Vec.Dot(plane.Normal, box.Max);
            float d3 = Vec.Dot(plane.Normal, new V2f(box.Min.X, box.Max.Y));
            float min = Math.Min(Math.Min(d0, d1), Math.Min(d2, d3));
            float max = Math.Max(Math.Max(d0, d1), Math.Max(d2, d3));
            return min <= plane.Distance && plane.Distance <= max;
        }

        private static void AssertLine(Box2d box, Plane2d plane, Line2d line)
        {
            AssertPoint(box, plane, line.P0);
            AssertPoint(box, plane, line.P1);
            Assert.That(line.P0.Y < line.P1.Y || line.P0.Y == line.P1.Y && line.P0.X <= line.P1.X, Is.True, "endpoint order");
        }

        private static void AssertLine(Box2f box, Plane2f plane, Line2f line)
        {
            AssertPoint(box, plane, line.P0);
            AssertPoint(box, plane, line.P1);
            Assert.That(line.P0.Y < line.P1.Y || line.P0.Y == line.P1.Y && line.P0.X <= line.P1.X, Is.True, "endpoint order");
        }

        private static void AssertPoint(Box2d box, Plane2d plane, V2d point)
        {
            double scale = Math.Max(1.0, Math.Abs(plane.Distance));
            Assert.That(Vec.Dot(plane.Normal, point), Is.EqualTo(plane.Distance).Within(2e-12 * scale), "plane equation");
            Assert.That(point.X, Is.InRange(box.Min.X - 1e-12, box.Max.X + 1e-12), "x bounds");
            Assert.That(point.Y, Is.InRange(box.Min.Y - 1e-12, box.Max.Y + 1e-12), "y bounds");
        }

        private static void AssertPoint(Box2f box, Plane2f plane, V2f point)
        {
            float scale = Math.Max(1.0f, Math.Abs(plane.Distance));
            Assert.That(Vec.Dot(plane.Normal, point), Is.EqualTo(plane.Distance).Within(3e-5f * scale), "plane equation");
            Assert.That(point.X, Is.InRange(box.Min.X - 2e-5f, box.Max.X + 2e-5f), "x bounds");
            Assert.That(point.Y, Is.InRange(box.Min.Y - 2e-5f, box.Max.Y + 2e-5f), "y bounds");
        }

        private static void AssertV2(V2d expected, V2d actual, double epsilon)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(epsilon));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(epsilon));
        }

        private static void AssertV2(V2f expected, V2f actual, float epsilon)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(epsilon));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(epsilon));
        }
    }
}
