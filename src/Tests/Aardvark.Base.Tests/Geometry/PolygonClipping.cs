using Aardvark.Base;
using NUnit.Framework;
using System;

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
