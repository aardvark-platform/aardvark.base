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
        public void ToStringAndParse()
        {
            var a = new Plane3d(new V3d(12, 46, 1.5), new V3d(-2, 412, 1.5), new V3d(876, -23, 1.5));
            var b = Plane3d.Parse(a.ToString());
            Assert.AreEqual(a, b);
        }
    }
}
