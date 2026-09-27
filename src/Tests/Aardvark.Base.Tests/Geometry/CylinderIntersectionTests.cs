using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class CylinderIntersectionTests
    {
        private static readonly V3d P0d = V3d.Zero;
        private static readonly V3d P1d = 4.0 * V3d.ZAxis;
        private static readonly V3f P0f = V3f.Zero;
        private static readonly V3f P1f = 4.0f * V3f.ZAxis;

        private static void AssertHit(Ray3d ray, double expected, double tolerance = 1e-12)
        {
            Assert.That(ray.HitsCylinder(P0d, P1d, 1.0, 0.0, double.MaxValue, out var t), Is.True);
            Assert.That(double.IsFinite(t), Is.True);
            Assert.That(t, Is.EqualTo(expected).Within(tolerance));

            var hit = RayHit3d.MaxRange;
            Assert.That(ray.HitsCylinder(P0d, P1d, 1.0, 0.0, double.MaxValue, ref hit), Is.True);
            Assert.That(hit.T, Is.EqualTo(t));
            Assert.That(hit.Point, Is.EqualTo(ray.GetPointOnRay(t)));
        }

        private static void AssertHit(Ray3f ray, float expected, float tolerance = 1e-5f)
        {
            Assert.That(ray.HitsCylinder(P0f, P1f, 1.0f, 0.0f, float.MaxValue, out var t), Is.True);
            Assert.That(float.IsFinite(t), Is.True);
            Assert.That(t, Is.EqualTo(expected).Within(tolerance));

            var hit = RayHit3f.MaxRange;
            Assert.That(ray.HitsCylinder(P0f, P1f, 1.0f, 0.0f, float.MaxValue, ref hit), Is.True);
            Assert.That(hit.T, Is.EqualTo(t));
            Assert.That(hit.Point, Is.EqualTo(ray.GetPointOnRay(t)));
        }

        private static void AssertMiss(Ray3d ray, V3d p0, V3d p1, double radius, double tmin = 0.0, double tmax = double.MaxValue)
        {
            Assert.That(ray.HitsCylinder(p0, p1, radius, tmin, tmax, out var t), Is.False);
            Assert.That(double.IsNaN(t), Is.True);
        }

        private static void AssertMiss(Ray3f ray, V3f p0, V3f p1, float radius, float tmin = 0.0f, float tmax = float.MaxValue)
        {
            Assert.That(ray.HitsCylinder(p0, p1, radius, tmin, tmax, out var t), Is.False);
            Assert.That(float.IsNaN(t), Is.True);
        }

        [Test]
        public void ObliqueNonUnitBarrelRootsUsePerpendicularDirection()
        {
            AssertHit(new Ray3d(new V3d(-3.0, 0.0, 0.5), new V3d(2.0, 0.0, 1.0)), 1.0);
            AssertHit(new Ray3f(new V3f(-3.0f, 0.0f, 0.5f), new V3f(2.0f, 0.0f, 1.0f)), 1.0f);
        }

        [Test]
        public void CapAndAxisParallelHitsAreFinite()
        {
            AssertHit(new Ray3d(new V3d(0.5, 0.0, -2.0), new V3d(0.0, 0.0, 2.0)), 1.0);
            AssertHit(new Ray3f(new V3f(0.5f, 0.0f, -2.0f), new V3f(0.0f, 0.0f, 2.0f)), 1.0f);

            AssertMiss(new Ray3d(new V3d(1.5, 0.0, -2.0), V3d.ZAxis), P0d, P1d, 1.0);
            AssertMiss(new Ray3f(new V3f(1.5f, 0.0f, -2.0f), V3f.ZAxis), P0f, P1f, 1.0f);
        }

        [Test]
        public void ObliqueCapIsSelectedBeforeLaterBarrel()
        {
            AssertHit(new Ray3d(new V3d(0.0, 0.0, 6.0), new V3d(0.25, 0.0, -1.0)), 2.0);
            AssertHit(new Ray3f(new V3f(0.0f, 0.0f, 6.0f), new V3f(0.25f, 0.0f, -1.0f)), 2.0f);
        }

        [Test]
        public void BarrelTangencyCountsAsAHit()
        {
            AssertHit(new Ray3d(new V3d(-2.0, 1.0, 2.0), V3d.XAxis), 2.0);
            AssertHit(new Ray3f(new V3f(-2.0f, 1.0f, 2.0f), V3f.XAxis), 2.0f);
        }

        [Test]
        public void RaysStartingInsideSelectForwardExit()
        {
            AssertHit(new Ray3d(new V3d(0.0, 0.0, 2.0), 2.0 * V3d.XAxis), 0.5);
            AssertHit(new Ray3f(new V3f(0.0f, 0.0f, 2.0f), 2.0f * V3f.XAxis), 0.5f);
            AssertHit(new Ray3d(new V3d(0.0, 0.0, 2.0), V3d.ZAxis), 2.0);
            AssertHit(new Ray3f(new V3f(0.0f, 0.0f, 2.0f), V3f.ZAxis), 2.0f);
        }

        [Test]
        public void BehindOriginIntersectionsMiss()
        {
            AssertMiss(new Ray3d(new V3d(-2.0, 0.0, 2.0), -V3d.XAxis), P0d, P1d, 1.0);
            AssertMiss(new Ray3f(new V3f(-2.0f, 0.0f, 2.0f), -V3f.XAxis), P0f, P1f, 1.0f);
        }

        [Test]
        public void ReversingCylinderEndpointsPreservesHit()
        {
            var rayD = new Ray3d(new V3d(-3.0, 0.0, 1.0), V3d.XAxis);
            Assert.That(rayD.HitsCylinder(P1d, P0d, 1.0, 0.0, double.MaxValue, out var td), Is.True);
            Assert.That(td, Is.EqualTo(2.0));

            var rayF = new Ray3f(new V3f(-3.0f, 0.0f, 1.0f), V3f.XAxis);
            Assert.That(rayF.HitsCylinder(P1f, P0f, 1.0f, 0.0f, float.MaxValue, out var tf), Is.True);
            Assert.That(tf, Is.EqualTo(2.0f));
        }

        [Test]
        public void ParameterIntervalIsHalfOpen()
        {
            var rayD = new Ray3d(new V3d(-2.0, 0.0, 2.0), V3d.XAxis);
            Assert.That(rayD.HitsCylinder(P0d, P1d, 1.0, 1.0, 2.0, out var td), Is.True);
            Assert.That(td, Is.EqualTo(1.0));
            AssertMiss(rayD, P0d, P1d, 1.0, 0.0, 1.0);

            var rayF = new Ray3f(new V3f(-2.0f, 0.0f, 2.0f), V3f.XAxis);
            Assert.That(rayF.HitsCylinder(P0f, P1f, 1.0f, 1.0f, 2.0f, out var tf), Is.True);
            Assert.That(tf, Is.EqualTo(1.0f));
            AssertMiss(rayF, P0f, P1f, 1.0f, 0.0f, 1.0f);
        }

        [Test]
        public void AccumulatorChangesOnlyForCloserValidHits()
        {
            var rayD = new Ray3d(new V3d(-2.0, 0.0, 2.0), V3d.XAxis);
            var hitD = new RayHit3d(0.5)
            {
                Point = new V3d(7.0, 8.0, 9.0),
                Coord = new V2d(3.0, 4.0),
                BackSide = true,
                Part = 17
            };
            var beforeD = hitD;
            Assert.That(rayD.HitsCylinder(P0d, P1d, 1.0, 0.0, 10.0, ref hitD), Is.False);
            AssertRayHitEqual(beforeD, hitD);
            Assert.That(rayD.HitsCylinder(P0d, P0d, 1.0, 0.0, 10.0, ref hitD), Is.False);
            AssertRayHitEqual(beforeD, hitD);

            hitD.T = 10.0;
            Assert.That(rayD.HitsCylinder(P0d, P1d, 1.0, 0.0, 10.0, ref hitD), Is.True);
            Assert.That(hitD.T, Is.EqualTo(1.0));
            Assert.That(hitD.Point, Is.EqualTo(new V3d(-1.0, 0.0, 2.0)));
            Assert.That(hitD.Coord, Is.EqualTo(V2d.NaN));
            Assert.That(hitD.BackSide, Is.False);
            Assert.That(hitD.Part, Is.EqualTo(17));

            var rayF = new Ray3f(new V3f(-2.0f, 0.0f, 2.0f), V3f.XAxis);
            var hitF = new RayHit3f(0.5f)
            {
                Point = new V3f(7.0f, 8.0f, 9.0f),
                Coord = new V2d(3.0, 4.0),
                BackSide = true,
                Part = 17
            };
            var beforeF = hitF;
            Assert.That(rayF.HitsCylinder(P0f, P1f, 1.0f, 0.0f, 10.0f, ref hitF), Is.False);
            AssertRayHitEqual(beforeF, hitF);
            Assert.That(rayF.HitsCylinder(P0f, P0f, 1.0f, 0.0f, 10.0f, ref hitF), Is.False);
            AssertRayHitEqual(beforeF, hitF);
        }

        [Test]
        public void DegenerateAndInvalidInputsMissWithNaNParameter()
        {
            var rayD = new Ray3d(new V3d(-2.0, 0.0, 2.0), V3d.XAxis);
            AssertMiss(rayD, P0d, P0d, 1.0);
            AssertMiss(new Ray3d(V3d.Zero, V3d.Zero), P0d, P1d, 1.0);
            AssertMiss(rayD, P0d, P1d, -1.0);
            AssertMiss(rayD, P0d, P1d, double.NaN);
            AssertMiss(rayD, P0d, P1d, double.PositiveInfinity);
            AssertMiss(rayD, V3d.NaN, P1d, 1.0);
            AssertMiss(rayD, P0d, new V3d(0.0, 0.0, double.PositiveInfinity), 1.0);
            AssertMiss(new Ray3d(new V3d(double.PositiveInfinity, 0.0, 0.0), V3d.XAxis), P0d, P1d, 1.0);
            AssertMiss(new Ray3d(V3d.Zero, new V3d(double.PositiveInfinity, 0.0, 0.0)), P0d, P1d, 1.0);
            AssertMiss(rayD, P0d, P1d, 1.0, 2.0, 2.0);
            AssertMiss(rayD, P0d, P1d, 1.0, double.NaN, 2.0);

            var rayF = new Ray3f(new V3f(-2.0f, 0.0f, 2.0f), V3f.XAxis);
            AssertMiss(rayF, P0f, P0f, 1.0f);
            AssertMiss(new Ray3f(V3f.Zero, V3f.Zero), P0f, P1f, 1.0f);
            AssertMiss(rayF, P0f, P1f, -1.0f);
            AssertMiss(rayF, P0f, P1f, float.NaN);
            AssertMiss(rayF, P0f, P1f, float.PositiveInfinity);
            AssertMiss(rayF, V3f.NaN, P1f, 1.0f);
            AssertMiss(rayF, P0f, new V3f(0.0f, 0.0f, float.PositiveInfinity), 1.0f);
            AssertMiss(new Ray3f(new V3f(float.PositiveInfinity, 0.0f, 0.0f), V3f.XAxis), P0f, P1f, 1.0f);
            AssertMiss(new Ray3f(V3f.Zero, new V3f(float.PositiveInfinity, 0.0f, 0.0f)), P0f, P1f, 1.0f);
            AssertMiss(rayF, P0f, P1f, 1.0f, 2.0f, 2.0f);
            AssertMiss(rayF, P0f, P1f, 1.0f, float.NaN, 2.0f);
        }

        [Test]
        public void ZeroRadiusCylinderRetainsLineSegmentHits()
        {
            var rayD = new Ray3d(new V3d(0.0, 0.0, -1.0), V3d.ZAxis);
            Assert.That(rayD.HitsCylinder(P0d, P1d, 0.0, 0.0, 10.0, out var td), Is.True);
            Assert.That(td, Is.EqualTo(1.0));
            AssertMiss(new Ray3d(new V3d(1e-6, 0.0, -1.0), V3d.ZAxis), P0d, P1d, 0.0);

            var rayF = new Ray3f(new V3f(0.0f, 0.0f, -1.0f), V3f.ZAxis);
            Assert.That(rayF.HitsCylinder(P0f, P1f, 0.0f, 0.0f, 10.0f, out var tf), Is.True);
            Assert.That(tf, Is.EqualTo(1.0f));
            AssertMiss(new Ray3f(new V3f(1e-3f, 0.0f, -1.0f), V3f.ZAxis), P0f, P1f, 0.0f);
        }

        [Test]
        public void ExtremeFiniteScalesProduceFiniteRoots()
        {
            var largeD = new Ray3d(new V3d(-2e200, 0.0, 0.5), new V3d(1e200, 0.0, 0.0));
            Assert.That(largeD.HitsCylinder(V3d.Zero, V3d.ZAxis, 1e200, 0.0, 10.0, out var largeTd), Is.True);
            Assert.That(largeTd, Is.EqualTo(1.0).Within(1e-12));

            var tinyD = new Ray3d(new V3d(-2.0, 0.0, 0.5), new V3d(1e-200, 0.0, 0.0));
            Assert.That(tinyD.HitsCylinder(V3d.Zero, V3d.ZAxis, 1.0, 0.0, double.MaxValue, out var tinyTd), Is.True);
            Assert.That(tinyTd, Is.EqualTo(1e200).Within(1e186));

            var largeF = new Ray3f(new V3f(-2e20f, 0.0f, 0.5f), new V3f(1e20f, 0.0f, 0.0f));
            Assert.That(largeF.HitsCylinder(V3f.Zero, V3f.ZAxis, 1e20f, 0.0f, 10.0f, out var largeTf), Is.True);
            Assert.That(largeTf, Is.EqualTo(1.0f).Within(1e-5f));

            var tinyF = new Ray3f(new V3f(-2.0f, 0.0f, 0.5f), new V3f(1e-20f, 0.0f, 0.0f));
            Assert.That(tinyF.HitsCylinder(V3f.Zero, V3f.ZAxis, 1.0f, 0.0f, float.MaxValue, out var tinyTf), Is.True);
            Assert.That(tinyTf, Is.EqualTo(1e20f).Within(1e15f));
        }

        [Test]
        public void DistanceScalePreservesDistanceBasedRadiusGrowth()
        {
            var cylinderD = new Cylinder3d(P0d, P1d, 0.25);
            var rayD = new Ray3d(new V3d(-3.0, 0.0, 2.0), new V3d(2.0, 0.0, 0.0));
            var unscaledD = RayHit3d.MaxRange;
            var scaledD = RayHit3d.MaxRange;
            Assert.That(rayD.Hits(cylinderD, 0.0, 10.0, 0.0, ref unscaledD), Is.True);
            Assert.That(rayD.Hits(cylinderD, 0.0, 10.0, 3.0, ref scaledD), Is.True);
            Assert.That(unscaledD.T, Is.EqualTo(1.375).Within(1e-12));
            Assert.That(scaledD.T, Is.EqualTo(1.25).Within(1e-12));

            var parallelD = RayHit3d.MaxRange;
            var parallelRayD = new Ray3d(new V3d(0.0, 0.0, -2.0), V3d.ZAxis);
            Assert.That(parallelRayD.Hits(cylinderD, 0.0, 10.0, 3.0, ref parallelD), Is.True);
            Assert.That(parallelD.T, Is.EqualTo(2.0));

            var invalidScaleD = new RayHit3d(10.0) { Point = V3d.One, Part = 3 };
            var beforeInvalidScaleD = invalidScaleD;
            Assert.That(rayD.Hits(cylinderD, 0.0, 10.0, double.NaN, ref invalidScaleD), Is.False);
            AssertRayHitEqual(beforeInvalidScaleD, invalidScaleD);

            var cylinderF = new Cylinder3f(P0f, P1f, 0.25f);
            var rayF = new Ray3f(new V3f(-3.0f, 0.0f, 2.0f), new V3f(2.0f, 0.0f, 0.0f));
            var unscaledF = RayHit3f.MaxRange;
            var scaledF = RayHit3f.MaxRange;
            Assert.That(rayF.Hits(cylinderF, 0.0f, 10.0f, 0.0f, ref unscaledF), Is.True);
            Assert.That(rayF.Hits(cylinderF, 0.0f, 10.0f, 3.0f, ref scaledF), Is.True);
            Assert.That(unscaledF.T, Is.EqualTo(1.375f).Within(1e-5f));
            Assert.That(scaledF.T, Is.EqualTo(1.25f).Within(1e-5f));

            var parallelF = RayHit3f.MaxRange;
            var parallelRayF = new Ray3f(new V3f(0.0f, 0.0f, -2.0f), V3f.ZAxis);
            Assert.That(parallelRayF.Hits(cylinderF, 0.0f, 10.0f, 3.0f, ref parallelF), Is.True);
            Assert.That(parallelF.T, Is.EqualTo(2.0f));

            var invalidScaleF = new RayHit3f(10.0f) { Point = V3f.One, Part = 3 };
            var beforeInvalidScaleF = invalidScaleF;
            Assert.That(rayF.Hits(cylinderF, 0.0f, 10.0f, float.NaN, ref invalidScaleF), Is.False);
            AssertRayHitEqual(beforeInvalidScaleF, invalidScaleF);
        }

        private static void AssertRayHitEqual(RayHit3d expected, RayHit3d actual)
        {
            Assert.That(actual.T, Is.EqualTo(expected.T));
            Assert.That(actual.Point, Is.EqualTo(expected.Point));
            Assert.That(actual.Coord, Is.EqualTo(expected.Coord));
            Assert.That(actual.BackSide, Is.EqualTo(expected.BackSide));
            Assert.That(actual.Part, Is.EqualTo(expected.Part));
        }

        private static void AssertRayHitEqual(RayHit3f expected, RayHit3f actual)
        {
            Assert.That(actual.T, Is.EqualTo(expected.T));
            Assert.That(actual.Point, Is.EqualTo(expected.Point));
            Assert.That(actual.Coord, Is.EqualTo(expected.Coord));
            Assert.That(actual.BackSide, Is.EqualTo(expected.BackSide));
            Assert.That(actual.Part, Is.EqualTo(expected.Part));
        }
    }
}
