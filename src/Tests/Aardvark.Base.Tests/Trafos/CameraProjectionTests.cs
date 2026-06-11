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
    }
}
