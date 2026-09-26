using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class CircleIntersectionTests
    {
        private static readonly V3d CenterD = V3d.Zero;
        private static readonly V3f CenterF = V3f.Zero;
        private static readonly V3d NormalD = V3d.ZAxis;
        private static readonly V3f NormalF = V3f.ZAxis;

        [Test]
        public void OrdinaryAndRimHitsAreReported()
        {
            var ordinaryD = new Ray3d(new V3d(0.25, -0.5, -2.0), V3d.ZAxis);
            Assert.That(ordinaryD.HitsCircle(CenterD, NormalD, 1.0, 0.0, 10.0, out var ordinaryTd), Is.True);
            Assert.That(ordinaryTd, Is.EqualTo(2.0));

            var rimD = new Ray3d(new V3d(1.0, 0.0, -2.0), V3d.ZAxis);
            Assert.That(rimD.HitsCircle(CenterD, NormalD, 1.0, 0.0, 10.0, out var rimTd), Is.True);
            Assert.That(rimTd, Is.EqualTo(2.0));

            var centerD = new Ray3d(new V3d(0.0, 0.0, -2.0), V3d.ZAxis);
            Assert.That(centerD.HitsCircle(CenterD, NormalD, 0.0, 0.0, 10.0, out var centerTd), Is.True);
            Assert.That(centerTd, Is.EqualTo(2.0));

            var ordinaryF = new Ray3f(new V3f(0.25f, -0.5f, -2.0f), V3f.ZAxis);
            Assert.That(ordinaryF.HitsCircle(CenterF, NormalF, 1.0f, 0.0f, 10.0f, out var ordinaryTf), Is.True);
            Assert.That(ordinaryTf, Is.EqualTo(2.0f));

            var rimF = new Ray3f(new V3f(1.0f, 0.0f, -2.0f), V3f.ZAxis);
            Assert.That(rimF.HitsCircle(CenterF, NormalF, 1.0f, 0.0f, 10.0f, out var rimTf), Is.True);
            Assert.That(rimTf, Is.EqualTo(2.0f));

            var centerF = new Ray3f(new V3f(0.0f, 0.0f, -2.0f), V3f.ZAxis);
            Assert.That(centerF.HitsCircle(CenterF, NormalF, 0.0f, 0.0f, 10.0f, out var centerTf), Is.True);
            Assert.That(centerTf, Is.EqualTo(2.0f));
        }

        [Test]
        public void ParameterIntervalIsHalfOpen()
        {
            var rayD = new Ray3d(new V3d(0.0, 0.0, -2.0), V3d.ZAxis);
            Assert.That(rayD.HitsCircle(CenterD, NormalD, 1.0, 2.0, 3.0, out var td), Is.True);
            Assert.That(td, Is.EqualTo(2.0));
            AssertMiss(rayD, CenterD, NormalD, 1.0, 0.0, 2.0);
            AssertMiss(rayD, CenterD, NormalD, 1.0, 2.0, 2.0);

            var rayF = new Ray3f(new V3f(0.0f, 0.0f, -2.0f), V3f.ZAxis);
            Assert.That(rayF.HitsCircle(CenterF, NormalF, 1.0f, 2.0f, 3.0f, out var tf), Is.True);
            Assert.That(tf, Is.EqualTo(2.0f));
            AssertMiss(rayF, CenterF, NormalF, 1.0f, 0.0f, 2.0f);
            AssertMiss(rayF, CenterF, NormalF, 1.0f, 2.0f, 2.0f);
        }

        [Test]
        public void ParallelRaysMissWithNaNParameter()
        {
            AssertMiss(new Ray3d(new V3d(0.0, 0.0, 1.0), V3d.XAxis), CenterD, NormalD, 1.0, 0.0, 10.0);
            AssertMiss(new Ray3f(new V3f(0.0f, 0.0f, 1.0f), V3f.XAxis), CenterF, NormalF, 1.0f, 0.0f, 10.0f);
        }

        [Test]
        public void InvalidRadiiMissWithNaNParameter()
        {
            var rayD = new Ray3d(new V3d(0.0, 0.0, -2.0), V3d.ZAxis);
            AssertMiss(rayD, CenterD, NormalD, -1.0, 0.0, 10.0);
            AssertMiss(rayD, CenterD, NormalD, double.NaN, 0.0, 10.0);
            AssertMiss(rayD, CenterD, NormalD, double.PositiveInfinity, 0.0, 10.0);

            var rayF = new Ray3f(new V3f(0.0f, 0.0f, -2.0f), V3f.ZAxis);
            AssertMiss(rayF, CenterF, NormalF, -1.0f, 0.0f, 10.0f);
            AssertMiss(rayF, CenterF, NormalF, float.NaN, 0.0f, 10.0f);
            AssertMiss(rayF, CenterF, NormalF, float.PositiveInfinity, 0.0f, 10.0f);
        }

        [Test]
        public void OffDiskMissPreservesEntireAccumulator()
        {
            var rayD = new Ray3d(new V3d(2.0, 0.0, -2.0), V3d.ZAxis);
            var hitD = SentinelHitD(10.0);
            var beforeD = hitD;
            Assert.That(rayD.HitsCircle(CenterD, NormalD, 1.0, 0.0, 10.0, ref hitD), Is.False);
            AssertRayHitEqual(beforeD, hitD);
            AssertMiss(rayD, CenterD, NormalD, 1.0, 0.0, 10.0);

            var rayF = new Ray3f(new V3f(2.0f, 0.0f, -2.0f), V3f.ZAxis);
            var hitF = SentinelHitF(10.0f);
            var beforeF = hitF;
            Assert.That(rayF.HitsCircle(CenterF, NormalF, 1.0f, 0.0f, 10.0f, ref hitF), Is.False);
            AssertRayHitEqual(beforeF, hitF);
            AssertMiss(rayF, CenterF, NormalF, 1.0f, 0.0f, 10.0f);
        }

        [Test]
        public void ExistingNearerOrEqualHitIsPreserved()
        {
            var rayD = new Ray3d(new V3d(0.0, 0.0, -2.0), V3d.ZAxis);
            foreach (var previousT in new[] { 1.0, 2.0 })
            {
                var hit = SentinelHitD(previousT);
                var before = hit;
                Assert.That(rayD.HitsCircle(CenterD, NormalD, 1.0, 0.0, 10.0, ref hit), Is.False);
                AssertRayHitEqual(before, hit);
            }

            var rayF = new Ray3f(new V3f(0.0f, 0.0f, -2.0f), V3f.ZAxis);
            foreach (var previousT in new[] { 1.0f, 2.0f })
            {
                var hit = SentinelHitF(previousT);
                var before = hit;
                Assert.That(rayF.HitsCircle(CenterF, NormalF, 1.0f, 0.0f, 10.0f, ref hit), Is.False);
                AssertRayHitEqual(before, hit);
            }
        }

        [Test]
        public void ValidCloserHitUpdatesStandardFieldsAndPreservesPart()
        {
            var rayD = new Ray3d(new V3d(0.25, -0.5, -2.0), V3d.ZAxis);
            var hitD = SentinelHitD(10.0);
            Assert.That(rayD.HitsCircle(CenterD, NormalD, 1.0, 0.0, 10.0, ref hitD), Is.True);
            Assert.That(hitD.T, Is.EqualTo(2.0));
            Assert.That(hitD.Point, Is.EqualTo(new V3d(0.25, -0.5, 0.0)));
            Assert.That(hitD.Coord.X, Is.NaN);
            Assert.That(hitD.Coord.Y, Is.NaN);
            Assert.That(hitD.BackSide, Is.False);
            Assert.That(hitD.Part, Is.EqualTo(17));

            var rayF = new Ray3f(new V3f(0.25f, -0.5f, -2.0f), V3f.ZAxis);
            var hitF = SentinelHitF(10.0f);
            Assert.That(rayF.HitsCircle(CenterF, NormalF, 1.0f, 0.0f, 10.0f, ref hitF), Is.True);
            Assert.That(hitF.T, Is.EqualTo(2.0f));
            Assert.That(hitF.Point, Is.EqualTo(new V3f(0.25f, -0.5f, 0.0f)));
            Assert.That(hitF.Coord.X, Is.NaN);
            Assert.That(hitF.Coord.Y, Is.NaN);
            Assert.That(hitF.BackSide, Is.False);
            Assert.That(hitF.Part, Is.EqualTo(17));
        }

        [Test]
        public void OverflowScaleDiskContainmentDistinguishesInsideAndOutside()
        {
            const double radiusD = 1e200;
            AssertHit(new Ray3d(new V3d(0.5e200, 0.0, -1.0), V3d.ZAxis), CenterD, NormalD, radiusD, 1.0);
            AssertMiss(new Ray3d(new V3d(2e200, 0.0, -1.0), V3d.ZAxis), CenterD, NormalD, radiusD, 0.0, 2.0);

            const float radiusF = 1e20f;
            AssertHit(new Ray3f(new V3f(0.5e20f, 0.0f, -1.0f), V3f.ZAxis), CenterF, NormalF, radiusF, 1.0f);
            AssertMiss(new Ray3f(new V3f(2e20f, 0.0f, -1.0f), V3f.ZAxis), CenterF, NormalF, radiusF, 0.0f, 2.0f);
        }

        [Test]
        public void UnderflowScaleDiskContainmentDistinguishesInsideAndOutside()
        {
            const double radiusD = 1e-200;
            AssertHit(new Ray3d(new V3d(0.5e-200, 0.0, -1.0), V3d.ZAxis), CenterD, NormalD, radiusD, 1.0);
            AssertMiss(new Ray3d(new V3d(2e-200, 0.0, -1.0), V3d.ZAxis), CenterD, NormalD, radiusD, 0.0, 2.0);
            AssertMiss(new Ray3d(new V3d(1e-200, 0.0, -1.0), V3d.ZAxis), CenterD, NormalD, 0.0, 0.0, 2.0);
            AssertMiss(new Ray3d(new V3d(2.5e-162, 0.0, -1.0), V3d.ZAxis), CenterD, NormalD, 2e-162, 0.0, 2.0);

            const float radiusF = 1e-30f;
            AssertHit(new Ray3f(new V3f(0.5e-30f, 0.0f, -1.0f), V3f.ZAxis), CenterF, NormalF, radiusF, 1.0f);
            AssertMiss(new Ray3f(new V3f(2e-30f, 0.0f, -1.0f), V3f.ZAxis), CenterF, NormalF, radiusF, 0.0f, 2.0f);
            AssertMiss(new Ray3f(new V3f(1e-30f, 0.0f, -1.0f), V3f.ZAxis), CenterF, NormalF, 0.0f, 0.0f, 2.0f);
            AssertMiss(new Ray3f(new V3f(4e-23f, 0.0f, -1.0f), V3f.ZAxis), CenterF, NormalF, 3e-23f, 0.0f, 2.0f);
        }

        [Test]
        public void InvalidCircleMissesPreserveEntireAccumulator()
        {
            var rayD = new Ray3d(new V3d(0.0, 0.0, -2.0), V3d.ZAxis);
            var hitD = SentinelHitD(10.0);
            var beforeD = hitD;
            Assert.That(rayD.HitsCircle(CenterD, NormalD, -1.0, 0.0, 10.0, ref hitD), Is.False);
            AssertRayHitEqual(beforeD, hitD);

            var rayF = new Ray3f(new V3f(0.0f, 0.0f, -2.0f), V3f.ZAxis);
            var hitF = SentinelHitF(10.0f);
            var beforeF = hitF;
            Assert.That(rayF.HitsCircle(CenterF, NormalF, -1.0f, 0.0f, 10.0f, ref hitF), Is.False);
            AssertRayHitEqual(beforeF, hitF);
        }

        private static void AssertHit(Ray3d ray, V3d center, V3d normal, double radius, double expected)
        {
            Assert.That(ray.HitsCircle(center, normal, radius, 0.0, 2.0, out var t), Is.True);
            Assert.That(t, Is.EqualTo(expected));
        }

        private static void AssertHit(Ray3f ray, V3f center, V3f normal, float radius, float expected)
        {
            Assert.That(ray.HitsCircle(center, normal, radius, 0.0f, 2.0f, out var t), Is.True);
            Assert.That(t, Is.EqualTo(expected));
        }

        private static void AssertMiss(Ray3d ray, V3d center, V3d normal, double radius, double tmin, double tmax)
        {
            Assert.That(ray.HitsCircle(center, normal, radius, tmin, tmax, out var t), Is.False);
            Assert.That(t, Is.NaN);
        }

        private static void AssertMiss(Ray3f ray, V3f center, V3f normal, float radius, float tmin, float tmax)
        {
            Assert.That(ray.HitsCircle(center, normal, radius, tmin, tmax, out var t), Is.False);
            Assert.That(t, Is.NaN);
        }

        private static RayHit3d SentinelHitD(double t)
            => new(t)
            {
                Point = new V3d(7.0, 8.0, 9.0),
                Coord = new V2d(3.0, 4.0),
                BackSide = true,
                Part = 17
            };

        private static RayHit3f SentinelHitF(float t)
            => new(t)
            {
                Point = new V3f(7.0f, 8.0f, 9.0f),
                Coord = new V2d(3.0, 4.0),
                BackSide = true,
                Part = 17
            };

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
