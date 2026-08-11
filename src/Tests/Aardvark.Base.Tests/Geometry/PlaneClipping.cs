using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class PlaneClipping
    {
        private static readonly Plane2d s_plane2d = new Plane2d(V2d.XAxis, 0.0);
        private static readonly Plane2f s_plane2f = new Plane2f(V2f.XAxis, 0.0f);
        private static readonly Plane3d s_plane3d = new Plane3d(V3d.XAxis, 0.0);
        private static readonly Plane3f s_plane3f = new Plane3f(V3f.XAxis, 0.0f);

        [Test]
        public void CrossingSegmentsPreserveDirectionAndInsideEndpoint()
        {
            var line2d = new Line2d(new V2d(-2.0, 1.0), new V2d(2.0, 3.0));
            AssertLine(line2d.ClipByPlane(s_plane2d, 0.0), new V2d(0.0, 2.0), line2d.P1);
            AssertLine(line2d.Reversed.ClipByPlane(s_plane2d, 0.0), line2d.P1, new V2d(0.0, 2.0));

            var line2f = new Line2f(new V2f(-2.0f, 1.0f), new V2f(2.0f, 3.0f));
            AssertLine(line2f.ClipByPlane(s_plane2f, 0.0f), new V2f(0.0f, 2.0f), line2f.P1);
            AssertLine(line2f.Reversed.ClipByPlane(s_plane2f, 0.0f), line2f.P1, new V2f(0.0f, 2.0f));

            var line3d = new Line3d(new V3d(-2.0, 1.0, 3.0), new V3d(2.0, 3.0, 5.0));
            AssertLine(line3d.ClipByPlane(s_plane3d, 0.0), new V3d(0.0, 2.0, 4.0), line3d.P1);
            AssertLine(line3d.Reversed.ClipByPlane(s_plane3d, 0.0), line3d.P1, new V3d(0.0, 2.0, 4.0));

            var line3f = new Line3f(new V3f(-2.0f, 1.0f, 3.0f), new V3f(2.0f, 3.0f, 5.0f));
            AssertLine(line3f.ClipByPlane(s_plane3f, 0.0f), new V3f(0.0f, 2.0f, 4.0f), line3f.P1);
            AssertLine(line3f.Reversed.ClipByPlane(s_plane3f, 0.0f), line3f.P1, new V3f(0.0f, 2.0f, 4.0f));
        }

        [Test]
        public void RetainedAndRejectedSegmentsUsePositiveHalfSpace()
        {
            var retained2d = new Line2d(new V2d(1.0, 2.0), new V2d(3.0, 4.0));
            Assert.That(retained2d.ClipByPlane(s_plane2d), Is.EqualTo(retained2d));
            AssertRejected(new Line2d(new V2d(-3.0, 2.0), new V2d(-1.0, 4.0)).ClipByPlane(s_plane2d));

            var retained2f = new Line2f(new V2f(1.0f, 2.0f), new V2f(3.0f, 4.0f));
            Assert.That(retained2f.ClipByPlane(s_plane2f), Is.EqualTo(retained2f));
            AssertRejected(new Line2f(new V2f(-3.0f, 2.0f), new V2f(-1.0f, 4.0f)).ClipByPlane(s_plane2f));

            var retained3d = new Line3d(new V3d(1.0, 2.0, 3.0), new V3d(3.0, 4.0, 5.0));
            Assert.That(retained3d.ClipByPlane(s_plane3d), Is.EqualTo(retained3d));
            AssertRejected(new Line3d(new V3d(-3.0, 2.0, 3.0), new V3d(-1.0, 4.0, 5.0)).ClipByPlane(s_plane3d));

            var retained3f = new Line3f(new V3f(1.0f, 2.0f, 3.0f), new V3f(3.0f, 4.0f, 5.0f));
            Assert.That(retained3f.ClipByPlane(s_plane3f), Is.EqualTo(retained3f));
            AssertRejected(new Line3f(new V3f(-3.0f, 2.0f, 3.0f), new V3f(-1.0f, 4.0f, 5.0f)).ClipByPlane(s_plane3f));
        }

        [Test]
        public void PlaneAndToleranceBoundariesAreInclusive()
        {
            var onPlane2d = new Line2d(new V2d(0.0, 1.0), new V2d(0.0, 2.0));
            Assert.That(onPlane2d.ClipByPlane(s_plane2d, 0.0), Is.EqualTo(onPlane2d));
            var tolerance2d = new Line2d(new V2d(-0.25, 1.0), new V2d(-0.25, 2.0));
            Assert.That(tolerance2d.ClipByPlane(s_plane2d, 0.25), Is.EqualTo(tolerance2d));

            var onPlane2f = new Line2f(new V2f(0.0f, 1.0f), new V2f(0.0f, 2.0f));
            Assert.That(onPlane2f.ClipByPlane(s_plane2f, 0.0f), Is.EqualTo(onPlane2f));
            var tolerance2f = new Line2f(new V2f(-0.25f, 1.0f), new V2f(-0.25f, 2.0f));
            Assert.That(tolerance2f.ClipByPlane(s_plane2f, 0.25f), Is.EqualTo(tolerance2f));

            var onPlane3d = new Line3d(new V3d(0.0, 1.0, 2.0), new V3d(0.0, 2.0, 3.0));
            Assert.That(onPlane3d.ClipByPlane(s_plane3d, 0.0), Is.EqualTo(onPlane3d));
            var tolerance3d = new Line3d(new V3d(-0.25, 1.0, 2.0), new V3d(-0.25, 2.0, 3.0));
            Assert.That(tolerance3d.ClipByPlane(s_plane3d, 0.25), Is.EqualTo(tolerance3d));

            var onPlane3f = new Line3f(new V3f(0.0f, 1.0f, 2.0f), new V3f(0.0f, 2.0f, 3.0f));
            Assert.That(onPlane3f.ClipByPlane(s_plane3f, 0.0f), Is.EqualTo(onPlane3f));
            var tolerance3f = new Line3f(new V3f(-0.25f, 1.0f, 2.0f), new V3f(-0.25f, 2.0f, 3.0f));
            Assert.That(tolerance3f.ClipByPlane(s_plane3f, 0.25f), Is.EqualTo(tolerance3f));
        }

        [Test]
        public void BoundaryTangencyReturnsExactPointSegment()
        {
            var boundary2d = new V2d(-0.25, 1.0);
            AssertLine(
                new Line2d(boundary2d, new V2d(-1.0, 2.0)).ClipByPlane(s_plane2d, 0.25),
                boundary2d, boundary2d
            );

            var boundary2f = new V2f(-0.25f, 1.0f);
            AssertLine(
                new Line2f(new V2f(-1.0f, 2.0f), boundary2f).ClipByPlane(s_plane2f, 0.25f),
                boundary2f, boundary2f
            );

            var boundary3d = new V3d(-0.25, 1.0, 2.0);
            AssertLine(
                new Line3d(boundary3d, new V3d(-1.0, 2.0, 3.0)).ClipByPlane(s_plane3d, 0.25),
                boundary3d, boundary3d
            );

            var boundary3f = new V3f(-0.25f, 1.0f, 2.0f);
            AssertLine(
                new Line3f(new V3f(-1.0f, 2.0f, 3.0f), boundary3f).ClipByPlane(s_plane3f, 0.25f),
                boundary3f, boundary3f
            );
        }

        [Test]
        public void AbsoluteToleranceIsInvariantUnderPlaneScaling()
        {
            var line2d = new Line2d(new V2d(0.0, 1.0), new V2d(4.0, 3.0));
            Assert.That(
                line2d.ClipByPlane(new Plane2d(V2d.XAxis, 2.0), 0.125),
                Is.EqualTo(line2d.ClipByPlane(new Plane2d(8.0 * V2d.XAxis, 16.0), 0.125))
            );

            var line2f = new Line2f(new V2f(0.0f, 1.0f), new V2f(4.0f, 3.0f));
            Assert.That(
                line2f.ClipByPlane(new Plane2f(V2f.XAxis, 2.0f), 0.125f),
                Is.EqualTo(line2f.ClipByPlane(new Plane2f(8.0f * V2f.XAxis, 16.0f), 0.125f))
            );

            var line3d = new Line3d(new V3d(0.0, 1.0, 2.0), new V3d(4.0, 3.0, 4.0));
            Assert.That(
                line3d.ClipByPlane(new Plane3d(V3d.XAxis, 2.0), 0.125),
                Is.EqualTo(line3d.ClipByPlane(new Plane3d(8.0 * V3d.XAxis, 16.0), 0.125))
            );

            var line3f = new Line3f(new V3f(0.0f, 1.0f, 2.0f), new V3f(4.0f, 3.0f, 4.0f));
            Assert.That(
                line3f.ClipByPlane(new Plane3f(V3f.XAxis, 2.0f), 0.125f),
                Is.EqualTo(line3f.ClipByPlane(new Plane3f(8.0f * V3f.XAxis, 16.0f), 0.125f))
            );
        }

        [Test]
        public void ZeroLengthSegmentsAreRetainedOrRejectedByPosition()
        {
            var inside2d = new Line2d(new V2d(1.0, 2.0), new V2d(1.0, 2.0));
            Assert.That(inside2d.ClipByPlane(s_plane2d, 0.0), Is.EqualTo(inside2d));
            AssertRejected(new Line2d(new V2d(-1.0, 2.0), new V2d(-1.0, 2.0)).ClipByPlane(s_plane2d, 0.0));

            var inside2f = new Line2f(new V2f(1.0f, 2.0f), new V2f(1.0f, 2.0f));
            Assert.That(inside2f.ClipByPlane(s_plane2f, 0.0f), Is.EqualTo(inside2f));
            AssertRejected(new Line2f(new V2f(-1.0f, 2.0f), new V2f(-1.0f, 2.0f)).ClipByPlane(s_plane2f, 0.0f));

            var inside3d = new Line3d(new V3d(1.0, 2.0, 3.0), new V3d(1.0, 2.0, 3.0));
            Assert.That(inside3d.ClipByPlane(s_plane3d, 0.0), Is.EqualTo(inside3d));
            AssertRejected(new Line3d(new V3d(-1.0, 2.0, 3.0), new V3d(-1.0, 2.0, 3.0)).ClipByPlane(s_plane3d, 0.0));

            var inside3f = new Line3f(new V3f(1.0f, 2.0f, 3.0f), new V3f(1.0f, 2.0f, 3.0f));
            Assert.That(inside3f.ClipByPlane(s_plane3f, 0.0f), Is.EqualTo(inside3f));
            AssertRejected(new Line3f(new V3f(-1.0f, 2.0f, 3.0f), new V3f(-1.0f, 2.0f, 3.0f)).ClipByPlane(s_plane3f, 0.0f));
        }

        [Test]
        public void ZeroNormalPlanesAreNoOpEvenWithInvalidDistance()
        {
            var line2d = new Line2d(new V2d(-1.0, 2.0), new V2d(1.0, 3.0));
            Assert.That(line2d.ClipByPlane(new Plane2d(V2d.Zero, double.NaN)), Is.EqualTo(line2d));

            var line2f = new Line2f(new V2f(-1.0f, 2.0f), new V2f(1.0f, 3.0f));
            Assert.That(line2f.ClipByPlane(new Plane2f(V2f.Zero, float.PositiveInfinity)), Is.EqualTo(line2f));

            var line3d = new Line3d(new V3d(-1.0, 2.0, 3.0), new V3d(1.0, 3.0, 4.0));
            Assert.That(line3d.ClipByPlane(new Plane3d(V3d.Zero, double.PositiveInfinity)), Is.EqualTo(line3d));

            var line3f = new Line3f(new V3f(-1.0f, 2.0f, 3.0f), new V3f(1.0f, 3.0f, 4.0f));
            Assert.That(line3f.ClipByPlane(new Plane3f(V3f.Zero, float.NaN)), Is.EqualTo(line3f));
        }

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

        private static void AssertLine(Line3d actual, V3d p0, V3d p1)
        {
            Assert.That(actual.P0, Is.EqualTo(p0));
            Assert.That(actual.P1, Is.EqualTo(p1));
        }

        private static void AssertLine(Line3f actual, V3f p0, V3f p1)
        {
            Assert.That(actual.P0, Is.EqualTo(p0));
            Assert.That(actual.P1, Is.EqualTo(p1));
        }

        private static void AssertRejected(Line2d actual)
            => Assert.That(actual.P0.IsNaN && actual.P1.IsNaN, Is.True);

        private static void AssertRejected(Line2f actual)
            => Assert.That(actual.P0.IsNaN && actual.P1.IsNaN, Is.True);

        private static void AssertRejected(Line3d actual)
            => Assert.That(actual.P0.IsNaN && actual.P1.IsNaN, Is.True);

        private static void AssertRejected(Line3f actual)
            => Assert.That(actual.P0.IsNaN && actual.P1.IsNaN, Is.True);
    }
}
