using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class CameraProjectionTests
    {
        [Test]
        public static void OrthoClippingWindowRejectsInvalidValue()
        {
            var projection = new CameraProjectionOrtho(-1.0, 1.0, -1.0, 1.0, 0.1, 10.0);
            var previous = projection.ClippingWindow;

            Assert.Throws<ArgumentOutOfRangeException>(() => projection.ClippingWindow = Box2d.Invalid);
            Assert.That(projection.ClippingWindow, Is.EqualTo(previous));
        }

        [Test]
        public static void OrthoClippingWindowAcceptsValidValue()
        {
            var projection = new CameraProjectionOrtho(-1.0, 1.0, -1.0, 1.0, 0.1, 10.0);
            var clippingWindow = new Box2d(new V2d(-2.0, -1.5), new V2d(3.0, 2.5));

            projection.ClippingWindow = clippingWindow;

            Assert.That(projection.ClippingWindow, Is.EqualTo(clippingWindow));
        }

        [Test]
        public static void OrthoConstructorRejectsInvalidClippingParams()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionOrtho(1.0, 1.0, -1.0, 1.0, 0.1, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionOrtho(-1.0, 1.0, 1.0, 1.0, 0.1, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionOrtho(-1.0, 1.0, -1.0, 1.0, 0.0, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionOrtho(-1.0, 1.0, -1.0, 1.0, 10.0, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionOrtho(double.NaN, 1.0, -1.0, 1.0, 0.1, 10.0));
        }

        [Test]
        public static void OrthoSetClippingParamsRejectsInvalidValuesWithoutMutation()
        {
            var projection = new CameraProjectionOrtho(-1.0, 1.0, -1.0, 1.0, 0.1, 10.0);

            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(1.0, 1.0, -1.0, 1.0, 0.1, 10.0));
            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(-1.0, 1.0, -1.0, 1.0, 0.0, 10.0));
            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(-1.0, 1.0, -1.0, 1.0, 0.1, 0.1));
        }

        [Test]
        public static void OrthoSetClippingParamsAcceptsValidUpdate()
        {
            var projection = new CameraProjectionOrtho(-1.0, 1.0, -1.0, 1.0, 0.1, 10.0);

            projection.SetClippingParams(-2.0, 4.0, -3.0, 5.0, 0.5, 50.0);

            Assert.That(projection.Near, Is.EqualTo(0.5));
            Assert.That(projection.Far, Is.EqualTo(50.0));
            Assert.That(projection.ClippingWindow, Is.EqualTo(new Box2d(new V2d(-2.0, -3.0), new V2d(4.0, 5.0))));
        }

        [Test]
        public static void PerspectiveClippingWindowRejectsInvalidValue()
        {
            var projection = new CameraProjectionPerspective(-1.0, 1.0, -1.0, 1.0, 0.1, 10.0);
            var previous = projection.ClippingWindow;

            Assert.Throws<ArgumentException>(() => projection.ClippingWindow = Box2d.Invalid);
            Assert.That(projection.ClippingWindow, Is.EqualTo(previous));
        }

        [Test]
        public static void PerspectiveClippingWindowAcceptsValidValue()
        {
            var projection = new CameraProjectionPerspective(-1.0, 1.0, -1.0, 1.0, 0.1, 10.0);
            var clippingWindow = new Box2d(new V2d(-2.0, -1.5), new V2d(3.0, 2.5));

            projection.ClippingWindow = clippingWindow;

            Assert.That(projection.ClippingWindow, Is.EqualTo(clippingWindow));
        }

        [Test]
        public static void PerspectiveConstructorsRejectInvalidClippingParams()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionPerspective(1.0, 1.0, -1.0, 1.0, 0.1, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionPerspective(-1.0, 1.0, 1.0, 1.0, 0.1, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionPerspective(-1.0, 1.0, -1.0, 1.0, 0.0, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionPerspective(-1.0, 1.0, -1.0, 1.0, 10.0, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionPerspective(0.0, 0.1, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionPerspective(180.0, 0.1, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionPerspective(60.0, 0.0, 10.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionPerspective(60.0, 0.1, 10.0, 0.0));
            Assert.Throws<ArgumentOutOfRangeException>(() => new CameraProjectionPerspective(double.PositiveInfinity, 0.1, 10.0));
        }

        [Test]
        public static void PerspectiveSetClippingParamsRejectsInvalidValuesWithoutMutation()
        {
            var projection = new CameraProjectionPerspective(-1.0, 1.0, -1.0, 1.0, 0.1, 10.0);

            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(1.0, 1.0, -1.0, 1.0, 0.1, 10.0));
            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(-1.0, 1.0, -1.0, 1.0, 0.0, 10.0));
            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(-1.0, 1.0, -1.0, 1.0, 0.1, 0.1));
        }

        [Test]
        public static void PerspectiveFovSetClippingParamsRejectsInvalidValuesWithoutMutation()
        {
            var projection = new CameraProjectionPerspective(60.0, 0.1, 10.0, 1.5);

            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(0.0, 0.1, 10.0, 1.5));
            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(180.0, 0.1, 10.0, 1.5));
            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(60.0, 0.0, 10.0, 1.5));
            AssertRejectedWithoutMutation(projection, () => projection.SetClippingParams(60.0, 0.1, 10.0, 0.0));
        }

        [Test]
        public static void PerspectiveSetClippingParamsAcceptsValidUpdate()
        {
            var projection = new CameraProjectionPerspective(-1.0, 1.0, -1.0, 1.0, 0.1, 10.0);

            projection.SetClippingParams(-2.0, 4.0, -3.0, 5.0, 0.5, 50.0);

            Assert.That(projection.Near, Is.EqualTo(0.5));
            Assert.That(projection.Far, Is.EqualTo(50.0));
            Assert.That(projection.ClippingWindow, Is.EqualTo(new Box2d(new V2d(-2.0, -3.0), new V2d(4.0, 5.0))));
        }

        [Test]
        public static void PerspectiveFovSetClippingParamsAcceptsValidUpdate()
        {
            var projection = new CameraProjectionPerspective(45.0, 0.1, 10.0, 1.0);

            projection.SetClippingParams(60.0, 0.5, 50.0, 2.0);

            var halfWidth = Math.Tan(Conversion.RadiansFromDegrees(60.0) * 0.5) * 0.5;
            var halfHeight = halfWidth / 2.0;
            Assert.That(projection.Near, Is.EqualTo(0.5));
            Assert.That(projection.Far, Is.EqualTo(50.0));
            Assert.That(projection.ClippingWindow.Min.X, Is.EqualTo(-halfWidth).Within(1e-12));
            Assert.That(projection.ClippingWindow.Max.X, Is.EqualTo(halfWidth).Within(1e-12));
            Assert.That(projection.ClippingWindow.Min.Y, Is.EqualTo(-halfHeight).Within(1e-12));
            Assert.That(projection.ClippingWindow.Max.Y, Is.EqualTo(halfHeight).Within(1e-12));
        }

        private static void AssertRejectedWithoutMutation(ICameraProjection projection, TestDelegate action)
        {
            var near = projection.Near;
            var far = projection.Far;
            var clippingWindow = projection.ClippingWindow;
            var trafo = projection.ProjectionTrafo;

            Assert.Throws<ArgumentOutOfRangeException>(action);
            Assert.That(projection.Near, Is.EqualTo(near));
            Assert.That(projection.Far, Is.EqualTo(far));
            Assert.That(projection.ClippingWindow, Is.EqualTo(clippingWindow));
            Assert.That(projection.ProjectionTrafo, Is.EqualTo(trafo));
        }
    }
}
