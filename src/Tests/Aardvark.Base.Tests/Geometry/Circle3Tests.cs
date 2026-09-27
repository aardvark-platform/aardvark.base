using Aardvark.Base;
using NUnit.Framework;
using System;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Circle3Tests
    {
        [Test]
        public void Circle3f_PointIsRepresentativeCircumferencePoint()
        {
            var circle = new Circle3f(
                new V3f(1.25f, -2.5f, 4.0f),
                new V3f(1.0f, 2.0f, 3.0f).Normalized,
                3.5f);

            Assert.That((circle.Point - circle.Center).Length, Is.EqualTo(circle.Radius).Within(1e-5f));
            Assert.That(circle.Point, Is.EqualTo(circle.Center + circle.AxisU));
            Assert.That(circle.Point, Is.EqualTo(circle.GetPoint(0.0f)));
        }

        [Test]
        public void Circle3d_PointIsRepresentativeCircumferencePoint()
        {
            var circle = new Circle3d(
                new V3d(1.25, -2.5, 4.0),
                new V3d(1.0, 2.0, 3.0).Normalized,
                3.5);

            Assert.That((circle.Point - circle.Center).Length, Is.EqualTo(circle.Radius).Within(1e-12));
            Assert.That(circle.Point, Is.EqualTo(circle.Center + circle.AxisU));
            Assert.That(circle.Point, Is.EqualTo(circle.GetPoint(0.0)));
        }

        [Test]
        public void Circle3f_AxesFormOrientedRadiusLengthFrame()
        {
            var normal = new V3f(1.0f, 2.0f, 3.0f).Normalized;
            var circle = new Circle3f(V3f.Zero, normal, 7.0f);
            var reference = Math.Abs(normal.X) > 0.9f ? V3f.YAxis : V3f.XAxis;
            var expectedU = normal.Cross(reference).Normalized * circle.Radius;
            var expectedV = expectedU.Cross(normal).Normalized * circle.Radius;

            AssertVector(circle.AxisU, expectedU, 2e-5f);
            AssertVector(circle.AxisV, expectedV, 2e-5f);
            Assert.That(circle.AxisU.Length, Is.EqualTo(circle.Radius).Within(2e-5f));
            Assert.That(circle.AxisV.Length, Is.EqualTo(circle.Radius).Within(2e-5f));
            Assert.That(Math.Abs(circle.AxisU.Dot(normal)), Is.LessThan(2e-5f));
            Assert.That(Math.Abs(circle.AxisV.Dot(normal)), Is.LessThan(2e-5f));
            Assert.That(Math.Abs(circle.AxisU.Dot(circle.AxisV)), Is.LessThan(2e-4f));
        }

        [Test]
        public void Circle3d_AxesFormOrientedRadiusLengthFrame()
        {
            var normal = new V3d(1.0, 2.0, 3.0).Normalized;
            var circle = new Circle3d(V3d.Zero, normal, 7.0);
            var reference = Math.Abs(normal.X) > 0.9 ? V3d.YAxis : V3d.XAxis;
            var expectedU = normal.Cross(reference).Normalized * circle.Radius;
            var expectedV = expectedU.Cross(normal).Normalized * circle.Radius;

            AssertVector(circle.AxisU, expectedU, 2e-12);
            AssertVector(circle.AxisV, expectedV, 2e-12);
            Assert.That(circle.AxisU.Length, Is.EqualTo(circle.Radius).Within(2e-12));
            Assert.That(circle.AxisV.Length, Is.EqualTo(circle.Radius).Within(2e-12));
            Assert.That(Math.Abs(circle.AxisU.Dot(normal)), Is.LessThan(2e-12));
            Assert.That(Math.Abs(circle.AxisV.Dot(normal)), Is.LessThan(2e-12));
            Assert.That(Math.Abs(circle.AxisU.Dot(circle.AxisV)), Is.LessThan(2e-11));
        }

        [Test]
        public void Circle3f_BoundsUseExactObliqueProjectionExtents()
        {
            var normal = new V3f(1.0f, 1.0f, 1.0f).Normalized;
            var circle = new Circle3f(new V3f(2.0f, -3.0f, 5.0f), normal, 4.0f);
            var expectedExtent = new V3f(
                circle.Radius * MathF.Sqrt(MathF.Max(0.0f, 1.0f - normal.X * normal.X)),
                circle.Radius * MathF.Sqrt(MathF.Max(0.0f, 1.0f - normal.Y * normal.Y)),
                circle.Radius * MathF.Sqrt(MathF.Max(0.0f, 1.0f - normal.Z * normal.Z)));
            var bounds = circle.BoundingBox3f;

            AssertVector(bounds.Min, circle.Center - expectedExtent, 2e-5f);
            AssertVector(bounds.Max, circle.Center + expectedExtent, 2e-5f);
            AssertContainsDenseCircumference(circle, bounds, 2e-5f);
        }

        [Test]
        public void Circle3d_BoundsUseExactObliqueProjectionExtents()
        {
            var normal = new V3d(1.0, 1.0, 1.0).Normalized;
            var circle = new Circle3d(new V3d(2.0, -3.0, 5.0), normal, 4.0);
            var expectedExtent = new V3d(
                circle.Radius * Math.Sqrt(Math.Max(0.0, 1.0 - normal.X * normal.X)),
                circle.Radius * Math.Sqrt(Math.Max(0.0, 1.0 - normal.Y * normal.Y)),
                circle.Radius * Math.Sqrt(Math.Max(0.0, 1.0 - normal.Z * normal.Z)));
            var bounds = circle.BoundingBox3d;

            AssertVector(bounds.Min, circle.Center - expectedExtent, 2e-12);
            AssertVector(bounds.Max, circle.Center + expectedExtent, 2e-12);
            AssertContainsDenseCircumference(circle, bounds, 2e-12);
        }

        [Test]
        public void Circle3_ZeroRadiusCollapsesFramePointsAndBoundsToCenter()
        {
            var centerF = new V3f(1.0f, -2.0f, 3.0f);
            var circleF = new Circle3f(centerF, new V3f(1.0f, 2.0f, 3.0f).Normalized, 0.0f);
            Assert.That(circleF.AxisU, Is.EqualTo(V3f.Zero));
            Assert.That(circleF.AxisV, Is.EqualTo(V3f.Zero));
            Assert.That(circleF.Point, Is.EqualTo(centerF));
            Assert.That(circleF.GetPoint(1.25f), Is.EqualTo(centerF));
            Assert.That(circleF.BoundingBox3f.Min, Is.EqualTo(centerF));
            Assert.That(circleF.BoundingBox3f.Max, Is.EqualTo(centerF));

            var centerD = new V3d(1.0, -2.0, 3.0);
            var circleD = new Circle3d(centerD, new V3d(1.0, 2.0, 3.0).Normalized, 0.0);
            Assert.That(circleD.AxisU, Is.EqualTo(V3d.Zero));
            Assert.That(circleD.AxisV, Is.EqualTo(V3d.Zero));
            Assert.That(circleD.Point, Is.EqualTo(centerD));
            Assert.That(circleD.GetPoint(1.25), Is.EqualTo(centerD));
            Assert.That(circleD.BoundingBox3d.Min, Is.EqualTo(centerD));
            Assert.That(circleD.BoundingBox3d.Max, Is.EqualTo(centerD));
        }

        [TestCase(1e-30f)]
        [TestCase(1e30f)]
        public void Circle3f_AxisAlignedExtremeRadiusKeepsFiniteAxesAndBounds(float radius)
        {
            var circle = new Circle3f(V3f.Zero, V3f.ZAxis, radius);

            Assert.That(circle.AxisU, Is.EqualTo(new V3f(0.0f, radius, 0.0f)));
            Assert.That(circle.AxisV, Is.EqualTo(new V3f(radius, 0.0f, 0.0f)));
            Assert.That(circle.Point, Is.EqualTo(new V3f(0.0f, radius, 0.0f)));
            Assert.That(circle.BoundingBox3f.Min, Is.EqualTo(new V3f(-radius, -radius, 0.0f)));
            Assert.That(circle.BoundingBox3f.Max, Is.EqualTo(new V3f(radius, radius, 0.0f)));
            Assert.That(AllFinite(circle.AxisU) && AllFinite(circle.AxisV), Is.True);
        }

        [TestCase(1e-200)]
        [TestCase(1e200)]
        public void Circle3d_AxisAlignedExtremeRadiusKeepsFiniteAxesAndBounds(double radius)
        {
            var circle = new Circle3d(V3d.Zero, V3d.ZAxis, radius);

            Assert.That(circle.AxisU, Is.EqualTo(new V3d(0.0, radius, 0.0)));
            Assert.That(circle.AxisV, Is.EqualTo(new V3d(radius, 0.0, 0.0)));
            Assert.That(circle.Point, Is.EqualTo(new V3d(0.0, radius, 0.0)));
            Assert.That(circle.BoundingBox3d.Min, Is.EqualTo(new V3d(-radius, -radius, 0.0)));
            Assert.That(circle.BoundingBox3d.Max, Is.EqualTo(new V3d(radius, radius, 0.0)));
            Assert.That(AllFinite(circle.AxisU) && AllFinite(circle.AxisV), Is.True);
        }

        private static void AssertContainsDenseCircumference(Circle3f circle, Box3f bounds, float epsilon)
        {
            for (int i = 0; i < 4096; i++)
            {
                var point = circle.GetPoint(i * (ConstantF.PiTimesTwo / 4096.0f));
                Assert.That(point.X, Is.InRange(bounds.Min.X - epsilon, bounds.Max.X + epsilon));
                Assert.That(point.Y, Is.InRange(bounds.Min.Y - epsilon, bounds.Max.Y + epsilon));
                Assert.That(point.Z, Is.InRange(bounds.Min.Z - epsilon, bounds.Max.Z + epsilon));
            }
        }

        private static void AssertContainsDenseCircumference(Circle3d circle, Box3d bounds, double epsilon)
        {
            for (int i = 0; i < 4096; i++)
            {
                var point = circle.GetPoint(i * (Constant.PiTimesTwo / 4096.0));
                Assert.That(point.X, Is.InRange(bounds.Min.X - epsilon, bounds.Max.X + epsilon));
                Assert.That(point.Y, Is.InRange(bounds.Min.Y - epsilon, bounds.Max.Y + epsilon));
                Assert.That(point.Z, Is.InRange(bounds.Min.Z - epsilon, bounds.Max.Z + epsilon));
            }
        }

        private static void AssertVector(V3f actual, V3f expected, float epsilon)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(epsilon));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(epsilon));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(epsilon));
        }

        private static void AssertVector(V3d actual, V3d expected, double epsilon)
        {
            Assert.That(actual.X, Is.EqualTo(expected.X).Within(epsilon));
            Assert.That(actual.Y, Is.EqualTo(expected.Y).Within(epsilon));
            Assert.That(actual.Z, Is.EqualTo(expected.Z).Within(epsilon));
        }

        private static bool AllFinite(V3f value)
            => float.IsFinite(value.X) && float.IsFinite(value.Y) && float.IsFinite(value.Z);

        private static bool AllFinite(V3d value)
            => double.IsFinite(value.X) && double.IsFinite(value.Y) && double.IsFinite(value.Z);
    }
}
