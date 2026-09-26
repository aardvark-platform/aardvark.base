using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class SphereIntersectionTests
    {
        [Test]
        public void ExteriorHitsUseNearestParameterWithNonUnitDirections()
        {
            var sphereD = new Sphere3d(V3d.Zero, 1.0);
            var rayD = new Ray3d(new V3d(-5.0, 0.0, 0.0), 2.0 * V3d.XAxis);
            Assert.That(rayD.HitsSphere(sphereD.Center, sphereD.Radius, 0.0, 10.0, out var td), Is.True);
            Assert.That(td, Is.EqualTo(2.0));
            Assert.That(rayD.Hits(sphereD, 0.0, 10.0, out var sphereTd), Is.True);
            Assert.That(sphereTd, Is.EqualTo(td));

            var sphereF = new Sphere3f(V3f.Zero, 1.0f);
            var rayF = new Ray3f(new V3f(-5.0f, 0.0f, 0.0f), 2.0f * V3f.XAxis);
            Assert.That(rayF.HitsSphere(sphereF.Center, sphereF.Radius, 0.0f, 10.0f, out var tf), Is.True);
            Assert.That(tf, Is.EqualTo(2.0f));
            Assert.That(rayF.Hits(sphereF, 0.0f, 10.0f, out var sphereTf), Is.True);
            Assert.That(sphereTf, Is.EqualTo(tf));
        }

        [Test]
        public void InteriorHitsUseExitAndPreserveRootSideClassification()
        {
            var rayD = new Ray3d(new V3d(0.5, 0.0, 0.0), V3d.XAxis);
            var exitD = SentinelHitD(10.0);
            Assert.That(rayD.HitsSphere(V3d.Zero, 1.0, 0.0, 10.0, ref exitD), Is.True);
            Assert.That(exitD.T, Is.EqualTo(0.5));
            Assert.That(exitD.BackSide, Is.True);
            Assert.That(exitD.Part, Is.EqualTo(17));

            var entryD = SentinelHitD(10.0);
            Assert.That(rayD.HitsSphere(V3d.Zero, 1.0, -2.0, 0.0, ref entryD), Is.True);
            Assert.That(entryD.T, Is.EqualTo(-1.5));
            Assert.That(entryD.BackSide, Is.False);

            var rayF = new Ray3f(new V3f(0.5f, 0.0f, 0.0f), V3f.XAxis);
            var exitF = SentinelHitF(10.0f);
            Assert.That(rayF.HitsSphere(V3f.Zero, 1.0f, 0.0f, 10.0f, ref exitF), Is.True);
            Assert.That(exitF.T, Is.EqualTo(0.5f));
            Assert.That(exitF.BackSide, Is.True);
            Assert.That(exitF.Part, Is.EqualTo(17));

            var entryF = SentinelHitF(10.0f);
            Assert.That(rayF.HitsSphere(V3f.Zero, 1.0f, -2.0f, 0.0f, ref entryF), Is.True);
            Assert.That(entryF.T, Is.EqualTo(-1.5f));
            Assert.That(entryF.BackSide, Is.False);
        }

        [Test]
        public void TangenciesAndPointSpheresAreClosedHits()
        {
            var tangentD = new Ray3d(new V3d(-2.0, 1.0, 0.0), V3d.XAxis);
            AssertHit(tangentD, V3d.Zero, 1.0, 0.0, 4.0, 2.0);
            var tangentHitD = SentinelHitD(4.0);
            Assert.That(tangentD.HitsSphere(V3d.Zero, 1.0, 0.0, 4.0, ref tangentHitD), Is.True);
            Assert.That(tangentHitD.BackSide, Is.False);

            var pointD = new Ray3d(new V3d(-2.0, 0.0, 0.0), V3d.XAxis);
            AssertHit(pointD, V3d.Zero, 0.0, 0.0, 4.0, 2.0);
            AssertHit(new Ray3d(V3d.Zero, V3d.XAxis), V3d.Zero, 0.0, 0.0, 1.0, 0.0);

            var tangentF = new Ray3f(new V3f(-2.0f, 1.0f, 0.0f), V3f.XAxis);
            AssertHit(tangentF, V3f.Zero, 1.0f, 0.0f, 4.0f, 2.0f);
            var tangentHitF = SentinelHitF(4.0f);
            Assert.That(tangentF.HitsSphere(V3f.Zero, 1.0f, 0.0f, 4.0f, ref tangentHitF), Is.True);
            Assert.That(tangentHitF.BackSide, Is.False);

            var pointF = new Ray3f(new V3f(-2.0f, 0.0f, 0.0f), V3f.XAxis);
            AssertHit(pointF, V3f.Zero, 0.0f, 0.0f, 4.0f, 2.0f);
            AssertHit(new Ray3f(V3f.Zero, V3f.XAxis), V3f.Zero, 0.0f, 0.0f, 1.0f, 0.0f);
        }

        [Test]
        public void ParameterIntervalIsHalfOpen()
        {
            var rayD = new Ray3d(new V3d(-3.0, 0.0, 0.0), V3d.XAxis);
            AssertHit(rayD, V3d.Zero, 1.0, 2.0, 3.0, 2.0);
            AssertMiss(rayD, V3d.Zero, 1.0, 0.0, 2.0);
            AssertMiss(rayD, V3d.Zero, 1.0, 3.0, 4.0);
            var farD = SentinelHitD(10.0);
            Assert.That(rayD.HitsSphere(V3d.Zero, 1.0, 4.0, 5.0, ref farD), Is.True);
            Assert.That(farD.T, Is.EqualTo(4.0));
            Assert.That(farD.BackSide, Is.True);

            var rayF = new Ray3f(new V3f(-3.0f, 0.0f, 0.0f), V3f.XAxis);
            AssertHit(rayF, V3f.Zero, 1.0f, 2.0f, 3.0f, 2.0f);
            AssertMiss(rayF, V3f.Zero, 1.0f, 0.0f, 2.0f);
            AssertMiss(rayF, V3f.Zero, 1.0f, 3.0f, 4.0f);
            var farF = SentinelHitF(10.0f);
            Assert.That(rayF.HitsSphere(V3f.Zero, 1.0f, 4.0f, 5.0f, ref farF), Is.True);
            Assert.That(farF.T, Is.EqualTo(4.0f));
            Assert.That(farF.BackSide, Is.True);
        }

        [Test]
        public void InvalidRadiiAndRangesReturnNaN()
        {
            var rayD = new Ray3d(new V3d(-3.0, 0.0, 0.0), V3d.XAxis);
            AssertMiss(rayD, V3d.Zero, -1.0, 0.0, 10.0);
            AssertMiss(rayD, V3d.Zero, double.NaN, 0.0, 10.0);
            AssertMiss(rayD, V3d.Zero, double.PositiveInfinity, 0.0, 10.0);
            AssertMiss(rayD, V3d.Zero, 1.0, 2.0, 2.0);
            AssertMiss(rayD, V3d.Zero, 1.0, 3.0, 2.0);
            AssertMiss(rayD, V3d.Zero, 1.0, double.NaN, 10.0);
            AssertMiss(rayD, V3d.Zero, 1.0, 0.0, double.NaN);

            var rayF = new Ray3f(new V3f(-3.0f, 0.0f, 0.0f), V3f.XAxis);
            AssertMiss(rayF, V3f.Zero, -1.0f, 0.0f, 10.0f);
            AssertMiss(rayF, V3f.Zero, float.NaN, 0.0f, 10.0f);
            AssertMiss(rayF, V3f.Zero, float.PositiveInfinity, 0.0f, 10.0f);
            AssertMiss(rayF, V3f.Zero, 1.0f, 2.0f, 2.0f);
            AssertMiss(rayF, V3f.Zero, 1.0f, 3.0f, 2.0f);
            AssertMiss(rayF, V3f.Zero, 1.0f, float.NaN, 10.0f);
            AssertMiss(rayF, V3f.Zero, 1.0f, 0.0f, float.NaN);
        }

        [Test]
        public void InvalidGeometryAndDirectionsReturnNaN()
        {
            AssertMiss(new Ray3d(V3d.Zero, V3d.Zero), V3d.Zero, 1.0, 0.0, 10.0);
            AssertMiss(new Ray3d(V3d.Zero, new V3d(double.NaN, 0.0, 0.0)), V3d.Zero, 1.0, 0.0, 10.0);
            AssertMiss(new Ray3d(V3d.Zero, new V3d(double.PositiveInfinity, 0.0, 0.0)), V3d.Zero, 1.0, 0.0, 10.0);
            AssertMiss(new Ray3d(new V3d(double.NaN, 0.0, 0.0), V3d.XAxis), V3d.Zero, 1.0, 0.0, 10.0);
            AssertMiss(new Ray3d(V3d.Zero, V3d.XAxis), new V3d(double.PositiveInfinity, 0.0, 0.0), 1.0, 0.0, 10.0);

            AssertMiss(new Ray3f(V3f.Zero, V3f.Zero), V3f.Zero, 1.0f, 0.0f, 10.0f);
            AssertMiss(new Ray3f(V3f.Zero, new V3f(float.NaN, 0.0f, 0.0f)), V3f.Zero, 1.0f, 0.0f, 10.0f);
            AssertMiss(new Ray3f(V3f.Zero, new V3f(float.PositiveInfinity, 0.0f, 0.0f)), V3f.Zero, 1.0f, 0.0f, 10.0f);
            AssertMiss(new Ray3f(new V3f(float.NaN, 0.0f, 0.0f), V3f.XAxis), V3f.Zero, 1.0f, 0.0f, 10.0f);
            AssertMiss(new Ray3f(V3f.Zero, V3f.XAxis), new V3f(float.PositiveInfinity, 0.0f, 0.0f), 1.0f, 0.0f, 10.0f);
        }

        [Test]
        public void MissesAndNonCloserCandidatesPreserveEntireAccumulator()
        {
            var rayD = new Ray3d(new V3d(-3.0, 0.0, 0.0), V3d.XAxis);
            foreach (var previousT in new[] { 1.0, 2.0, 10.0 })
            {
                var hit = SentinelHitD(previousT);
                var before = hit;
                var radius = previousT == 10.0 ? -1.0 : 1.0;
                Assert.That(rayD.HitsSphere(V3d.Zero, radius, 0.0, 10.0, ref hit), Is.False);
                AssertRayHitEqual(before, hit);
            }

            var rayF = new Ray3f(new V3f(-3.0f, 0.0f, 0.0f), V3f.XAxis);
            foreach (var previousT in new[] { 1.0f, 2.0f, 10.0f })
            {
                var hit = SentinelHitF(previousT);
                var before = hit;
                var radius = previousT == 10.0f ? -1.0f : 1.0f;
                Assert.That(rayF.HitsSphere(V3f.Zero, radius, 0.0f, 10.0f, ref hit), Is.False);
                AssertRayHitEqual(before, hit);
            }
        }

        [Test]
        public void CloserHitsUpdateStandardFieldsAndPreservePart()
        {
            var rayD = new Ray3d(new V3d(-3.0, 0.0, 0.0), V3d.XAxis);
            var hitD = SentinelHitD(10.0);
            Assert.That(rayD.HitsSphere(V3d.Zero, 1.0, 0.0, 10.0, ref hitD), Is.True);
            Assert.That(hitD.T, Is.EqualTo(2.0));
            Assert.That(hitD.Point, Is.EqualTo(new V3d(-1.0, 0.0, 0.0)));
            Assert.That(hitD.Coord.X, Is.NaN);
            Assert.That(hitD.Coord.Y, Is.NaN);
            Assert.That(hitD.BackSide, Is.False);
            Assert.That(hitD.Part, Is.EqualTo(17));

            var rayF = new Ray3f(new V3f(-3.0f, 0.0f, 0.0f), V3f.XAxis);
            var hitF = SentinelHitF(10.0f);
            Assert.That(rayF.HitsSphere(V3f.Zero, 1.0f, 0.0f, 10.0f, ref hitF), Is.True);
            Assert.That(hitF.T, Is.EqualTo(2.0f));
            Assert.That(hitF.Point, Is.EqualTo(new V3f(-1.0f, 0.0f, 0.0f)));
            Assert.That(hitF.Coord.X, Is.NaN);
            Assert.That(hitF.Coord.Y, Is.NaN);
            Assert.That(hitF.BackSide, Is.False);
            Assert.That(hitF.Part, Is.EqualTo(17));
        }

        [Test]
        public void WarmedIntersectionsAllocateNoManagedMemory()
        {
            var rayD = RayForAllocationsD(0.0);
            var rayF = RayForAllocationsF(0.0f);
            var hitD = RayHit3d.MaxRange;
            var hitF = RayHit3f.MaxRange;

            rayD.HitsSphere(V3d.Zero, 1.0, 0.0, 10.0, out _);
            rayD.HitsSphere(V3d.Zero, 1.0, 0.0, 10.0, ref hitD);
            rayF.HitsSphere(V3f.Zero, 1.0f, 0.0f, 10.0f, out _);
            rayF.HitsSphere(V3f.Zero, 1.0f, 0.0f, 10.0f, ref hitF);

            var sum = 0.0;
            var before = GC.GetAllocatedBytesForCurrentThread();
            for (var i = 0; i < 10_000; i++)
            {
                if (rayD.HitsSphere(V3d.Zero, 1.0, 0.0, 10.0, out var td))
                    sum += td;
                hitD.T = double.MaxValue;
                if (rayD.HitsSphere(V3d.Zero, 1.0, 0.0, 10.0, ref hitD))
                    sum += hitD.T;

                if (rayF.HitsSphere(V3f.Zero, 1.0f, 0.0f, 10.0f, out var tf))
                    sum += tf;
                hitF.T = float.MaxValue;
                if (rayF.HitsSphere(V3f.Zero, 1.0f, 0.0f, 10.0f, ref hitF))
                    sum += hitF.T;
            }
            var allocated = GC.GetAllocatedBytesForCurrentThread() - before;

            GC.KeepAlive(sum);
            Assert.That(allocated, Is.Zero);
        }

        [Test]
        public void OverflowingSquaresRetainRepresentableHits()
        {
            const double scaleD = 1e200;
            AssertHit(new Ray3d(new V3d(-scaleD, 0.0, 0.0), scaleD * V3d.XAxis),
                V3d.Zero, 0.5 * scaleD, 0.0, 2.0, 0.5);

            const float scaleF = 1e30f;
            AssertHit(new Ray3f(new V3f(-scaleF, 0.0f, 0.0f), scaleF * V3f.XAxis),
                V3f.Zero, 0.5f * scaleF, 0.0f, 2.0f, 0.5f);
        }

        [Test]
        public void OverflowingOffsetSubtractionRetainsRepresentableHits()
        {
            const double scaleD = 1e308;
            AssertHit(new Ray3d(new V3d(-scaleD, 0.0, 0.0), scaleD * V3d.XAxis),
                new V3d(scaleD, 0.0, 0.0), scaleD, 0.0, 2.0, 1.0);

            const float scaleF = 2e38f;
            AssertHit(new Ray3f(new V3f(-scaleF, 0.0f, 0.0f), scaleF * V3f.XAxis),
                new V3f(scaleF, 0.0f, 0.0f), scaleF, 0.0f, 2.0f, 1.0f);
        }

        [Test]
        public void UnderflowingSquaresRetainRepresentableHits()
        {
            const double scaleD = 1e-300;
            AssertHit(new Ray3d(new V3d(-scaleD, 0.0, 0.0), scaleD * V3d.XAxis),
                V3d.Zero, 0.5 * scaleD, 0.0, 2.0, 0.5);

            const float scaleF = 1e-30f;
            AssertHit(new Ray3f(new V3f(-scaleF, 0.0f, 0.0f), scaleF * V3f.XAxis),
                V3f.Zero, 0.5f * scaleF, 0.0f, 2.0f, 0.5f);
        }

        [Test]
        public void SubnormalGeometryRetainsRepresentableHits()
        {
            var scaleD = 4.0 * double.Epsilon;
            AssertHit(new Ray3d(new V3d(-scaleD, 0.0, 0.0), scaleD * V3d.XAxis),
                V3d.Zero, 2.0 * double.Epsilon, 0.0, 2.0, 0.5);

            var scaleF = 4.0f * float.Epsilon;
            AssertHit(new Ray3f(new V3f(-scaleF, 0.0f, 0.0f), scaleF * V3f.XAxis),
                V3f.Zero, 2.0f * float.Epsilon, 0.0f, 2.0f, 0.5f);
        }

        private static Ray3d RayForAllocationsD(double y)
            => new(new V3d(-3.0, y, 0.0), V3d.XAxis);

        private static Ray3f RayForAllocationsF(float y)
            => new(new V3f(-3.0f, y, 0.0f), V3f.XAxis);

        private static void AssertHit(
            Ray3d ray, V3d center, double radius,
            double tmin, double tmax, double expected)
        {
            Assert.That(ray.HitsSphere(center, radius, tmin, tmax, out var t), Is.True);
            Assert.That(t, Is.EqualTo(expected).Within(1e-14));
        }

        private static void AssertHit(
            Ray3f ray, V3f center, float radius,
            float tmin, float tmax, float expected)
        {
            Assert.That(ray.HitsSphere(center, radius, tmin, tmax, out var t), Is.True);
            Assert.That(t, Is.EqualTo(expected).Within(1e-6f));
        }

        private static void AssertMiss(
            Ray3d ray, V3d center, double radius,
            double tmin, double tmax)
        {
            Assert.That(ray.HitsSphere(center, radius, tmin, tmax, out var t), Is.False);
            Assert.That(t, Is.NaN);
        }

        private static void AssertMiss(
            Ray3f ray, V3f center, float radius,
            float tmin, float tmax)
        {
            Assert.That(ray.HitsSphere(center, radius, tmin, tmax, out var t), Is.False);
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
