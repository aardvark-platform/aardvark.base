using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class LineDistance
    {
        const int Iter = 10000;
        const int SubDiv = 100;

        private static void AssertNear(double expected, double actual, double tolerance = 1e-10)
            => Assert.That(actual, Is.EqualTo(expected).Within(tolerance));

        private static void AssertNear(float expected, float actual, float tolerance = 1e-5f)
            => Assert.That(actual, Is.EqualTo(expected).Within(tolerance));

        private static void AssertRelative(double expected, double actual, double tolerance = 1e-12)
            => Assert.That(actual / expected, Is.EqualTo(1.0).Within(tolerance));

        private static void AssertRelative(float expected, float actual, float tolerance = 1e-5f)
            => Assert.That(actual / expected, Is.EqualTo(1.0f).Within(tolerance));

        private static void AssertNear(V2d expected, V2d actual, double tolerance = 1e-10)
            => Assert.That((actual - expected).Length, Is.LessThanOrEqualTo(tolerance));

        private static void AssertNear(V2f expected, V2f actual, float tolerance = 1e-5f)
            => Assert.That((actual - expected).Length, Is.LessThanOrEqualTo(tolerance));

        private static void AssertNear(V3d expected, V3d actual, double tolerance = 1e-10)
            => Assert.That((actual - expected).Length, Is.LessThanOrEqualTo(tolerance));

        private static void AssertNear(V3f expected, V3f actual, float tolerance = 1e-5f)
            => Assert.That((actual - expected).Length, Is.LessThanOrEqualTo(tolerance));

        [Test]
        public void PointRayProjectionParametersDouble()
        {
            var ray2d = new Ray2d(V2d.OO, new V2d(2.0, 0.0));
            var point2d = new V2d(5.0, 3.0);
            AssertNear(2.5, point2d.GetClosestPointTOn(ray2d));
            AssertNear(new V2d(5.0, 0.0), point2d.GetClosestPointOn(ray2d, out var t2d));
            AssertNear(2.5, t2d);

            var ray3d = new Ray3d(new V3d(1.0, 2.0, 3.0), new V3d(2.0, 0.0, 0.0));
            var ahead = new V3d(7.0, 5.0, 3.0);
            AssertNear(3.0, ahead.GetMinimalDistanceTo(ray3d, out var aheadT));
            AssertNear(3.0, ahead.GetMinimalDistanceTo(ray3d));
            AssertNear(3.0, ray3d.GetMinimalDistanceTo(ahead));
            AssertNear(3.0, aheadT);
            AssertNear(new V3d(7.0, 2.0, 3.0), ahead.GetClosestPointOn(ray3d, out var closestT));
            AssertNear(3.0, closestT);

            var behind = new V3d(-3.0, 5.0, 3.0);
            AssertNear(3.0, behind.GetMinimalDistanceTo(ray3d, out var behindT));
            AssertNear(-2.0, behindT);

            var pointRay2d = new Ray2d(new V2d(2.0, 4.0), V2d.Zero);
            AssertNear(0.0, point2d.GetClosestPointTOn(pointRay2d));
            AssertNear(pointRay2d.Origin, point2d.GetClosestPointOn(pointRay2d, out var pointT2d));
            AssertNear(0.0, pointT2d);

            var pointRay3d = new Ray3d(new V3d(2.0, 4.0, 6.0), V3d.Zero);
            AssertNear((ahead - pointRay3d.Origin).Length, ahead.GetMinimalDistanceTo(pointRay3d, out var pointT3d));
            AssertNear(0.0, pointT3d);
            AssertNear(pointRay3d.Origin, ahead.GetClosestPointOn(pointRay3d, out var closestPointT3d));
            AssertNear(0.0, closestPointT3d);
        }

        [Test]
        public void PointRayProjectionParametersFloat()
        {
            var ray2f = new Ray2f(V2f.OO, new V2f(0.0f, 4.0f));
            var point2f = new V2f(3.0f, 10.0f);
            AssertNear(2.5f, point2f.GetClosestPointTOn(ray2f));
            AssertNear(new V2f(0.0f, 10.0f), point2f.GetClosestPointOn(ray2f, out var t2f));
            AssertNear(2.5f, t2f);

            var ray3f = new Ray3f(new V3f(1.0f, 2.0f, 3.0f), new V3f(0.0f, 4.0f, 0.0f));
            var ahead = new V3f(4.0f, 12.0f, 3.0f);
            AssertNear(3.0f, ahead.GetMinimalDistanceTo(ray3f, out var aheadT));
            AssertNear(2.5f, aheadT);
            AssertNear(new V3f(1.0f, 12.0f, 3.0f), ahead.GetClosestPointOn(ray3f, out var closestT));
            AssertNear(2.5f, closestT);

            var behind = new V3f(4.0f, -4.0f, 3.0f);
            AssertNear(3.0f, behind.GetMinimalDistanceTo(ray3f, out var behindT));
            AssertNear(-1.5f, behindT);

            var pointRay = new Ray3f(new V3f(2.0f, 4.0f, 6.0f), V3f.Zero);
            AssertNear((ahead - pointRay.Origin).Length, ahead.GetMinimalDistanceTo(pointRay, out var pointT));
            AssertNear(0.0f, pointT);
            AssertNear(pointRay.Origin, ahead.GetClosestPointOn(pointRay, out var closestPointT));
            AssertNear(0.0f, closestPointT);
        }

        [Test]
        public void RayPairParametersDouble()
        {
            var ray2d0 = new Ray2d(V2d.OO, new V2d(2.0, 0.0));
            var ray2d1 = new Ray2d(new V2d(4.0, -3.0), new V2d(0.0, 3.0));
            AssertNear(0.0, ray2d0.GetMinimalDistanceTo(ray2d1, out var t2d0, out var t2d1));
            AssertNear(2.0, t2d0);
            AssertNear(1.0, t2d1);
            AssertNear(ray2d0.GetPointOnRay(t2d0), ray2d1.GetPointOnRay(t2d1));

            AssertNear(0.0, ray2d1.GetMinimalDistanceTo(ray2d0, out var reverseT2d0, out var reverseT2d1));
            AssertNear(t2d1, reverseT2d0);
            AssertNear(t2d0, reverseT2d1);

            var scaled2d0 = new Ray2d(ray2d0.Origin, ray2d0.Direction * 1e8);
            var scaled2d1 = new Ray2d(ray2d1.Origin, ray2d1.Direction * 1e-8);
            AssertNear(0.0, scaled2d0.GetMinimalDistanceTo(scaled2d1, out var scaledT2d0, out var scaledT2d1), 1e-8);
            AssertNear(ray2d0.GetPointOnRay(t2d0), scaled2d0.GetPointOnRay(scaledT2d0), 1e-8);
            AssertNear(ray2d1.GetPointOnRay(t2d1), scaled2d1.GetPointOnRay(scaledT2d1), 1e-8);

            var parallel = new Ray2d(new V2d(3.0, 4.0), new V2d(7.0, 0.0));
            AssertNear(4.0, ray2d0.GetMinimalDistanceTo(parallel, out var parallelT0, out var parallelT1));
            AssertNear(1.5, parallelT0);
            AssertNear(0.0, parallelT1);

            var antiparallel = new Ray2d(parallel.Origin, -parallel.Direction);
            AssertNear(4.0, ray2d0.GetMinimalDistanceTo(antiparallel, out var antiparallelT0, out var antiparallelT1));
            AssertNear(1.5, antiparallelT0);
            AssertNear(0.0, antiparallelT1);

            var nearParallel = new Ray2d(parallel.Origin, new V2d(7.0, 7e-8));
            AssertNear(4.0, ray2d0.GetMinimalDistanceTo(nearParallel, out var nearT0, out var nearT1));
            AssertNear(1.5, nearT0);
            AssertNear(0.0, nearT1);

            var scaledNearRay0 = new Ray2d(ray2d0.Origin, ray2d0.Direction * 1e8);
            var scaledNearRay1 = new Ray2d(nearParallel.Origin, nearParallel.Direction * 1e-8);
            AssertNear(4.0, scaledNearRay0.GetMinimalDistanceTo(scaledNearRay1, out var scaledNearT0, out var scaledNearT1));
            AssertNear(1.5e-8, scaledNearT0);
            AssertNear(0.0, scaledNearT1);

            var ray3d0 = new Ray3d(V3d.OOO, new V3d(2.0, 0.0, 0.0));
            var ray3d1 = new Ray3d(new V3d(4.0, 3.0, 5.0), new V3d(0.0, 3.0, 0.0));
            AssertNear(5.0, ray3d0.GetMinimalDistanceTo(ray3d1, out var t3d0, out var t3d1));
            AssertNear(2.0, t3d0);
            AssertNear(-1.0, t3d1);
            AssertNear(new V3d(4.0, 0.0, 0.0), ray3d0.GetPointOnRay(t3d0));
            AssertNear(new V3d(4.0, 0.0, 5.0), ray3d1.GetPointOnRay(t3d1));
            AssertNear(ray3d1.GetPointOnRay(t3d1), ray3d0.GetClosestPointOn(ray3d1));

            AssertNear(5.0, ray3d1.GetMinimalDistanceTo(ray3d0, out var reverseT3d0, out var reverseT3d1));
            AssertNear(t3d1, reverseT3d0);
            AssertNear(t3d0, reverseT3d1);

            var scaled3d0 = new Ray3d(ray3d0.Origin, ray3d0.Direction * 1e8);
            var scaled3d1 = new Ray3d(ray3d1.Origin, ray3d1.Direction * 1e-8);
            AssertNear(5.0, scaled3d0.GetMinimalDistanceTo(scaled3d1, out var scaledT3d0, out var scaledT3d1));
            AssertNear(ray3d0.GetPointOnRay(t3d0), scaled3d0.GetPointOnRay(scaledT3d0), 1e-8);
            AssertNear(ray3d1.GetPointOnRay(t3d1), scaled3d1.GetPointOnRay(scaledT3d1), 1e-8);
        }

        [Test]
        public void RayPairParametersFloat()
        {
            var ray2f0 = new Ray2f(V2f.OO, new V2f(2.0f, 0.0f));
            var ray2f1 = new Ray2f(new V2f(4.0f, -3.0f), new V2f(0.0f, 3.0f));
            AssertNear(0.0f, ray2f0.GetMinimalDistanceTo(ray2f1, out var t2f0, out var t2f1));
            AssertNear(2.0f, t2f0);
            AssertNear(1.0f, t2f1);
            AssertNear(ray2f0.GetPointOnRay(t2f0), ray2f1.GetPointOnRay(t2f1));

            var scaled2f0 = new Ray2f(ray2f0.Origin, ray2f0.Direction * 1e4f);
            var scaled2f1 = new Ray2f(ray2f1.Origin, ray2f1.Direction * 1e-4f);
            AssertNear(0.0f, scaled2f0.GetMinimalDistanceTo(scaled2f1, out var scaledT2f0, out var scaledT2f1), 1e-4f);
            AssertNear(ray2f0.GetPointOnRay(t2f0), scaled2f0.GetPointOnRay(scaledT2f0), 1e-4f);
            AssertNear(ray2f1.GetPointOnRay(t2f1), scaled2f1.GetPointOnRay(scaledT2f1), 1e-4f);

            var nearParallel = new Ray2f(new V2f(3.0f, 4.0f), new V2f(7.0f, 0.0035f));
            AssertNear(4.0f, ray2f0.GetMinimalDistanceTo(nearParallel, out var nearT0, out var nearT1), 1e-4f);
            AssertNear(1.5f, nearT0);
            AssertNear(0.0f, nearT1);

            var scaledNearRay0 = new Ray2f(ray2f0.Origin, ray2f0.Direction * 1e4f);
            var scaledNearRay1 = new Ray2f(nearParallel.Origin, nearParallel.Direction * 1e-4f);
            AssertNear(4.0f, scaledNearRay0.GetMinimalDistanceTo(scaledNearRay1, out var scaledNearT0, out var scaledNearT1), 1e-4f);
            AssertNear(1.5e-4f, scaledNearT0);
            AssertNear(0.0f, scaledNearT1);

            var ray3f0 = new Ray3f(V3f.OOO, new V3f(2.0f, 0.0f, 0.0f));
            var ray3f1 = new Ray3f(new V3f(4.0f, 3.0f, 5.0f), new V3f(0.0f, 3.0f, 0.0f));
            AssertNear(5.0f, ray3f0.GetMinimalDistanceTo(ray3f1, out var t3f0, out var t3f1));
            AssertNear(2.0f, t3f0);
            AssertNear(-1.0f, t3f1);
            AssertNear(ray3f1.GetPointOnRay(t3f1), ray3f0.GetClosestPointOn(ray3f1));

            AssertNear(5.0f, ray3f1.GetMinimalDistanceTo(ray3f0, out var reverseT3f0, out var reverseT3f1));
            AssertNear(t3f1, reverseT3f0);
            AssertNear(t3f0, reverseT3f1);
        }

        [Test]
        public void ExtremeDirectionScalesPreservePointProjection()
        {
            foreach (var scale in new[] { 1e-200, 1e200 })
            {
                var ray2d = new Ray2d(V2d.Zero, scale * V2d.XAxis);
                var point2d = new V2d(3.0 * scale, 4.0);
                AssertNear(3.0, point2d.GetClosestPointTOn(ray2d));
                AssertNear(new V2d(3.0 * scale, 0.0), point2d.GetClosestPointOn(ray2d, out var t2d), 1e-10 * scale.Abs());
                AssertNear(3.0, t2d);

                var ray3d = new Ray3d(V3d.Zero, scale * V3d.XAxis);
                var point3d = new V3d(3.0 * scale, 4.0, 0.0);
                AssertNear(4.0, point3d.GetMinimalDistanceTo(ray3d, out var t3d));
                AssertNear(3.0, t3d);
            }

            foreach (var scale in new[] { 1e-30f, 1e20f })
            {
                var ray2f = new Ray2f(V2f.Zero, scale * V2f.XAxis);
                var point2f = new V2f(3.0f * scale, 4.0f);
                AssertNear(3.0f, point2f.GetClosestPointTOn(ray2f));
                AssertNear(new V2f(3.0f * scale, 0.0f), point2f.GetClosestPointOn(ray2f, out var t2f), 1e-5f * scale.Abs());
                AssertNear(3.0f, t2f);

                var ray3f = new Ray3f(V3f.Zero, scale * V3f.XAxis);
                var point3f = new V3f(3.0f * scale, 4.0f, 0.0f);
                AssertNear(4.0f, point3f.GetMinimalDistanceTo(ray3f, out var t3f));
                AssertNear(3.0f, t3f);
            }
        }

        [Test]
        public void ExtremeDirectionScalesPreserveRayPairGeometry()
        {
            foreach (var scale in new[] { 1e-200, 1e200 })
            {
                var ray2d0 = new Ray2d(V2d.Zero, scale * V2d.XAxis);
                var ray2d1 = new Ray2d(new V2d(2.0, -3.0), scale * V2d.YAxis);
                AssertNear(0.0, ray2d0.GetMinimalDistanceTo(ray2d1, out var t2d0, out var t2d1));
                AssertRelative(2.0 / scale, t2d0);
                AssertRelative(3.0 / scale, t2d1);
                AssertNear(ray2d0.GetPointOnRay(t2d0), ray2d1.GetPointOnRay(t2d1));

                var ray3d0 = new Ray3d(V3d.Zero, scale * V3d.XAxis);
                var ray3d1 = new Ray3d(new V3d(2.0, -3.0, 5.0), scale * V3d.YAxis);
                AssertNear(5.0, ray3d0.GetMinimalDistanceTo(ray3d1, out var t3d0, out var t3d1));
                AssertRelative(2.0 / scale, t3d0);
                AssertRelative(3.0 / scale, t3d1);
            }

            foreach (var scale in new[] { 1e-20f, 1e20f })
            {
                var ray2f0 = new Ray2f(V2f.Zero, scale * V2f.XAxis);
                var ray2f1 = new Ray2f(new V2f(2.0f, -3.0f), scale * V2f.YAxis);
                AssertNear(0.0f, ray2f0.GetMinimalDistanceTo(ray2f1, out var t2f0, out var t2f1));
                AssertRelative(2.0f / scale, t2f0);
                AssertRelative(3.0f / scale, t2f1);
                AssertNear(ray2f0.GetPointOnRay(t2f0), ray2f1.GetPointOnRay(t2f1));

                var ray3f0 = new Ray3f(V3f.Zero, scale * V3f.XAxis);
                var ray3f1 = new Ray3f(new V3f(2.0f, -3.0f, 5.0f), scale * V3f.YAxis);
                AssertNear(5.0f, ray3f0.GetMinimalDistanceTo(ray3f1, out var t3f0, out var t3f1));
                AssertRelative(2.0f / scale, t3f0);
                AssertRelative(3.0f / scale, t3f1);
            }
        }

        [Test]
        public void ExtremeAndZeroDirectionRayPairs()
        {
            var point2d = new Ray2d(new V2d(2.0, 3.0), V2d.Zero);
            var large2d = new Ray2d(V2d.Zero, 1e200 * V2d.XAxis);
            AssertNear(3.0, point2d.GetMinimalDistanceTo(large2d, out var pointT2d, out var largeT2d));
            AssertNear(0.0, pointT2d);
            AssertRelative(2e-200, largeT2d);
            AssertNear(3.0, large2d.GetMinimalDistanceTo(point2d, out largeT2d, out pointT2d));
            AssertRelative(2e-200, largeT2d);
            AssertNear(0.0, pointT2d);

            var point3f = new Ray3f(new V3f(2.0f, 3.0f, 4.0f), V3f.Zero);
            var large3f = new Ray3f(V3f.Zero, 1e20f * V3f.XAxis);
            AssertNear(5.0f, point3f.GetMinimalDistanceTo(large3f, out var pointT3f, out var largeT3f));
            AssertNear(0.0f, pointT3f);
            AssertRelative(2e-20f, largeT3f);
            AssertNear(5.0f, large3f.GetMinimalDistanceTo(point3f, out largeT3f, out pointT3f));
            AssertRelative(2e-20f, largeT3f);
            AssertNear(0.0f, pointT3f);
        }

        [Test]
        public void ZeroDirectionRayPairs()
        {
            var pointRay2d = new Ray2d(new V2d(1.0, 2.0), V2d.Zero);
            var ray2d = new Ray2d(new V2d(4.0, 2.0), new V2d(2.0, 0.0));
            AssertNear(0.0, pointRay2d.GetMinimalDistanceTo(ray2d, out var pointT2d, out var rayT2d));
            AssertNear(0.0, pointT2d);
            AssertNear(-1.5, rayT2d);
            AssertNear(0.0, ray2d.GetMinimalDistanceTo(pointRay2d, out var reverseRayT2d, out var reversePointT2d));
            AssertNear(-1.5, reverseRayT2d);
            AssertNear(0.0, reversePointT2d);

            var otherPointRay2d = new Ray2d(new V2d(4.0, 6.0), V2d.Zero);
            AssertNear(5.0, pointRay2d.GetMinimalDistanceTo(otherPointRay2d, out var point0T2d, out var point1T2d));
            AssertNear(0.0, point0T2d);
            AssertNear(0.0, point1T2d);

            var pointRay3d = new Ray3d(new V3d(1.0, 2.0, 3.0), V3d.Zero);
            var ray3d = new Ray3d(new V3d(4.0, 2.0, 3.0), new V3d(2.0, 0.0, 0.0));
            AssertNear(0.0, pointRay3d.GetMinimalDistanceTo(ray3d, out var pointT3d, out var rayT3d));
            AssertNear(0.0, pointT3d);
            AssertNear(-1.5, rayT3d);
            AssertNear(pointRay3d.Origin, pointRay3d.GetClosestPointOn(ray3d));

            var otherPointRay3d = new Ray3d(new V3d(4.0, 6.0, 3.0), V3d.Zero);
            AssertNear(5.0, pointRay3d.GetMinimalDistanceTo(otherPointRay3d, out var point0T3d, out var point1T3d));
            AssertNear(0.0, point0T3d);
            AssertNear(0.0, point1T3d);

            var pointRay2f = new Ray2f(new V2f(1.0f, 2.0f), V2f.Zero);
            var ray2f = new Ray2f(new V2f(4.0f, 2.0f), new V2f(2.0f, 0.0f));
            AssertNear(0.0f, pointRay2f.GetMinimalDistanceTo(ray2f, out var pointT2f, out var rayT2f));
            AssertNear(0.0f, pointT2f);
            AssertNear(-1.5f, rayT2f);

            var pointRay3f = new Ray3f(new V3f(1.0f, 2.0f, 3.0f), V3f.Zero);
            var ray3f = new Ray3f(new V3f(4.0f, 2.0f, 3.0f), new V3f(2.0f, 0.0f, 0.0f));
            AssertNear(0.0f, pointRay3f.GetMinimalDistanceTo(ray3f, out var pointT3f, out var rayT3f));
            AssertNear(0.0f, pointT3f);
            AssertNear(-1.5f, rayT3f);
        }

        [Test]
        public void RayDistanceCallersUseOriginalParameters()
        {
            var ray3d = new Ray3d(V3d.OOO, new V3d(2.0, 0.0, 0.0));
            var crossingRay3d = new Ray3d(new V3d(4.0, -3.0, 0.0), new V3d(0.0, 3.0, 0.0));
            Assert.That(ray3d.Intersects(crossingRay3d, out var rayT3d, out var crossingRayT3d), Is.True);
            AssertNear(2.0, rayT3d);
            AssertNear(1.0, crossingRayT3d);

            var line3d = new Line3d(new V3d(4.0, -3.0, 0.0), new V3d(4.0, 3.0, 0.0));
            AssertNear(0.0, ray3d.GetMinimalDistanceTo(line3d, out var lineRayT3d));
            AssertNear(2.0, lineRayT3d);
            Assert.That(ray3d.Intersects(line3d, 1e-10, out var intersectionT3d), Is.True);
            AssertNear(2.0, intersectionT3d);

            var line3d0 = new Line3d(V3d.OOO, new V3d(4.0, 0.0, 0.0));
            var line3d1 = new Line3d(new V3d(2.0, -2.0, 0.0), new V3d(2.0, 2.0, 0.0));
            AssertNear(0.0, line3d0.GetMinimalDistanceTo(line3d1, out var linePoint3d));
            AssertNear(new V3d(2.0, 0.0, 0.0), linePoint3d);
            AssertNear(new V3d(2.0, 0.0, 0.0), line3d0.GetClosestPointOn(line3d1));
            Assert.That(line3d0.Intersects(line3d1), Is.True);

            var ray3f = new Ray3f(V3f.OOO, new V3f(2.0f, 0.0f, 0.0f));
            var crossingRay3f = new Ray3f(new V3f(4.0f, -3.0f, 0.0f), new V3f(0.0f, 3.0f, 0.0f));
            Assert.That(ray3f.Intersects(crossingRay3f, out var rayT3f, out var crossingRayT3f), Is.True);
            AssertNear(2.0f, rayT3f);
            AssertNear(1.0f, crossingRayT3f);

            var line3f = new Line3f(new V3f(4.0f, -3.0f, 0.0f), new V3f(4.0f, 3.0f, 0.0f));
            AssertNear(0.0f, ray3f.GetMinimalDistanceTo(line3f, out var lineRayT3f));
            AssertNear(2.0f, lineRayT3f);
            Assert.That(ray3f.Intersects(line3f, 1e-5f, out var intersectionT3f), Is.True);
            AssertNear(2.0f, intersectionT3f);
        }

        [Test]
        public void DistanceToLineAndGetMinimalDistanceToConsistency()
        {
            var line = new Line3d(new V3d(1, 2, 3), new V3d(1, 2, 3));
            var p = V3d.OOO;

            var x = p.DistanceToLine(line.P0, line.P1);
            var md = p.GetMinimalDistanceTo(line);

            Assert.AreEqual(x, md);
        }

        [Test]
        public void LineToPointDistance2d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < Iter; i++)
            {
                var p00 = rnd.UniformV2d();
                var line0 = new Line2d(p00, p00 + rnd.UniformV2d() * rnd.UniformDouble() * 0.2);

                var p01 = rnd.UniformV2d();

                var dist = p01.GetMinimalDistanceTo(line0);

                var refDist = 5.0;
                var r0 = line0.Ray2d;
                for (int j = 0; j <= SubDiv; j++)
                {
                    var x0 = r0.GetPointOnRay(j / (double)SubDiv);
                    refDist = Fun.Min(refDist, Vec.Distance(x0, p01));
                }
                var e = 0.2 / SubDiv;
                Assert.IsTrue(refDist.ApproximateEquals(dist, e));
            }
        }

        [Test]
        public void LineToLineDistance2d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < Iter; i++)
            {
                var p00 = rnd.UniformV2d();
                var line0 = new Line2d(p00, p00 + rnd.UniformV2d() * rnd.UniformDouble() * 0.2);

                var p01 = rnd.UniformV2d();
                var line1 = new Line2d(p01, p01 + rnd.UniformV2d() * rnd.UniformDouble() * 0.2);

                var dist = line0.GetMinimalDistanceTo(line1);

                // Alternative: assuming Line3d distance is correct
                //var refDist = new Line3d(line0.P0.XYO, line0.P1.XYO).GetMinimalDistanceTo(new Line3d(line1.P0.XYO, line1.P1.XYO)); 

                var refDist = 5.0;
                var r0 = line0.Ray2d;
                var r1 = line1.Ray2d;
                for (int j = 0; j <= SubDiv; j++)
                {
                    var x0 = r0.GetPointOnRay(j / (double)SubDiv);
                    for (int k = 0; k <= SubDiv; k++)
                    {
                        var x1 = r1.GetPointOnRay(k / (double)SubDiv);
                        refDist = Fun.Min(refDist, Vec.Distance(x0, x1));
                    }
                }
                var e = 0.2 / SubDiv;
                Assert.IsTrue(refDist.ApproximateEquals(dist, e));
            }
        }

        [Test]
        public void LineToPointDistance3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < Iter; i++)
            {
                var p00 = rnd.UniformV3d();
                var line0 = new Line3d(p00, p00 + rnd.UniformV3d() * rnd.UniformDouble() * 0.2);

                var p01 = rnd.UniformV3d();

                var dist = p01.GetMinimalDistanceTo(line0);

                var refDist = 5.0;
                var r0 = line0.Ray3d;
                for (int j = 0; j <= SubDiv; j++)
                {
                    var x0 = r0.GetPointOnRay(j / (double)SubDiv);
                    refDist = Fun.Min(refDist, Vec.Distance(x0, p01));
                }
                var e = 0.2 / SubDiv;
                Assert.IsTrue(refDist.ApproximateEquals(dist, e));
            }
        }

        [Test]
        public void LineToLineDistance3d()
        {
            var rnd = new RandomSystem(2);

            for (int i = 0; i < Iter; i++)
            {
                var p00 = rnd.UniformV3d();
                var line0 = new Line3d(p00, p00 + rnd.UniformV3d() * rnd.UniformDouble() * 0.2);

                var p01 = rnd.UniformV3d();
                var line1 = new Line3d(p01, p01 + rnd.UniformV3d() * rnd.UniformDouble() * 0.2);

                var dist = line0.GetMinimalDistanceTo(line1);
                var refDist = 5.0;
                var r0 = line0.Ray3d;
                var r1 = line1.Ray3d;
                for (int j = 0; j <= SubDiv; j++)
                {
                    var x0 = r0.GetPointOnRay(j / (double)SubDiv);
                    for (int k = 0; k <= SubDiv; k++)
                    {
                        var x1 = r1.GetPointOnRay(k / (double)SubDiv);
                        refDist = Fun.Min(refDist, Vec.Distance(x0, x1));
                    }
                }
                var e = 0.2 / SubDiv;
                Assert.IsTrue(refDist.ApproximateEquals(dist, e));
            }
        }
    }
}
