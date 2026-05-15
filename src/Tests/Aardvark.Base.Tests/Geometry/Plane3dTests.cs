using Aardvark.Base;
using NUnit.Framework;
using System;
using static System.Math;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Plane3dTests
    {
        [Test]
        public void Plane3d_Constructor1()
        {
            var a = new Plane3d();
            Assert.IsTrue(!a.IsValid);
            Assert.IsTrue(a.IsInvalid);
        }

        [Test]
        public void Plane3d_Constructor2()
        {
            var a = new Plane3d(V3d.ZAxis, 1.5);
            Assert.IsTrue(a.IsValid);
            Assert.IsTrue(!a.IsInvalid);
            Assert.IsTrue(a.Normal == V3d.ZAxis && a.Distance == 1.5);
        }

        [Test]
        public void Plane3d_Constructor3()
        {
            var a = new Plane3d(V3d.ZAxis, new V3d(123.345, 4556.234, 1.5));
            Assert.IsTrue(a.IsValid);
            Assert.IsTrue(!a.IsInvalid);
            Assert.IsTrue(a.Normal == V3d.ZAxis && a.Distance == 1.5);
        }

        [Test]
        public void Plane3d_Constructor4()
        {
            var a = new Plane3d(new V3d(12, 46, 1.5), new V3d(-2, 412, 1.5), new V3d(876, -23, 1.5));
            Assert.IsTrue(a.IsValid);
            Assert.IsTrue(!a.IsInvalid);
            Assert.IsTrue((a.Normal == V3d.ZAxis && a.Distance == 1.5) ||
                          (a.Normal == -V3d.ZAxis && a.Distance == -1.5));
        }

        [Test]
        public void ProjectPointArrayAllowsFullArrayRange()
        {
            var plane = new Plane3d(V3d.ZAxis, 1.5);
            var points = new[]
            {
                new V3d(1, 2, 5),
                new V3d(-3, 4, -1),
                new V3d(0.25, -0.5, 1.5),
            };

            var result = plane.Project(points, 0, points.Length);

            Assert.AreEqual(points.Length, result.Length);
            for (var i = 0; i < points.Length; i++)
                Assert.AreEqual(new V3d(points[i].X, points[i].Y, 1.5), result[i]);
        }

        [Test]
        public void ProjectPointArrayAlongDirectionAllowsFullArrayRange()
        {
            var plane = new Plane3d(V3d.ZAxis, 1.5);
            var direction = -V3d.ZAxis;
            var points = new[]
            {
                new V3d(1, 2, 2),
                new V3d(-3, 4, 8),
                new V3d(0.25, -0.5, 3),
            };

            var result = plane.Project(points, direction, 0, points.Length);

            Assert.AreEqual(points.Length, result.Length);
            for (var i = 0; i < points.Length; i++)
                Assert.AreEqual(new V3d(points[i].X, points[i].Y, 1.5), result[i]);
        }

        [Test]
        public void ProjectPointArrayAllowsSliceEndingAtArrayLength()
        {
            var plane = new Plane3d(V3d.ZAxis, 1.5);
            var points = new[]
            {
                new V3d(100, 200, 300),
                new V3d(1, 2, 5),
                new V3d(-3, 4, -1),
            };

            var result = plane.Project(points, 1, points.Length - 1);

            Assert.AreEqual(2, result.Length);
            Assert.AreEqual(new V3d(1, 2, 1.5), result[0]);
            Assert.AreEqual(new V3d(-3, 4, 1.5), result[1]);
        }

        [Test]
        public void ToStringAndParse()
        {
            var a = new Plane3d(new V3d(12, 46, 1.5), new V3d(-2, 412, 1.5), new V3d(876, -23, 1.5));
            var b = Plane3d.Parse(a.ToString());
            Assert.AreEqual(a, b);
        }
    }
}
