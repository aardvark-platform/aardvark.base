using Aardvark.Base;
using NUnit.Framework;
using System;
using static System.Math;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Circle2dTests
    {
        [Test]
        public void Circle2d_Constructor1()
        {
            var a = new Circle2d(new V2d(1.2, 4.7), 3.14);
            Assert.IsTrue(a.Center == new V2d(1.2, 4.7));
            Assert.IsTrue(a.Radius == 3.14);
            Assert.IsTrue(!a.IsInvalid);
            Assert.IsTrue(a.IsValid);
        }

        [Test]
        public void Circle2d_Constructor2()
        {
            var a = new Circle2d(new V2d(1.2, 4.7), new V2d(1.2, 6.7));
            Assert.IsTrue(a.Center == new V2d(1.2, 4.7));
            Assert.IsTrue(a.Radius == 2);
            Assert.IsTrue(!a.IsInvalid);
            Assert.IsTrue(a.IsValid);
        }

        [Test]
        public void Circle2d_Constructor3()
        {
            var a = new Circle2d(new V2d(1, 0), new V2d(2, 1), new V2d(3, 0));
            Assert.IsTrue(a.Center == new V2d(2, 0));
            Assert.IsTrue(a.Radius == 1);
            Assert.IsTrue(!a.IsInvalid);
            Assert.IsTrue(a.IsValid);
        }

        [Test]
        public void Circle2d_Zero()
        {
            var a = Circle2d.Zero;
            Assert.IsTrue(!a.IsInvalid);
            Assert.IsTrue(a.IsValid);
        }

        [Test]
        public void Circle2d_Unit()
        {
            var a = Circle2d.Unit;
            Assert.IsTrue(!a.IsInvalid);
            Assert.IsTrue(a.IsValid);
        }

        [Test]
        public void Circle2d_Invalid()
        {
            var a = Circle2d.Invalid;
            Assert.IsTrue(a.IsInvalid);
            Assert.IsTrue(!a.IsValid);
        }

        [Test]
        public void Circle2d_Invalid2()
        {
            var a = new Circle2d(V2d.NaN, 1.0);
            Assert.IsTrue(a.IsInvalid);
            Assert.IsTrue(!a.IsValid);
        }

        [Test]
        public void Circle2d_Invalid3()
        {
            var a = new Circle2d(V2d.Zero, double.NaN);
            Assert.IsTrue(a.IsInvalid);
            Assert.IsTrue(!a.IsValid);
        }

        [Test]
        public void Circle2d_Invalid4()
        {
            var a = new Circle2d(V2d.NaN, double.NaN);
            Assert.IsTrue(a.IsInvalid);
            Assert.IsTrue(!a.IsValid);
        }

        [Test]
        public void Circle2d_Invalid5()
        {
            var a = new Circle2d(V2d.Zero, -1);
            Assert.IsTrue(a.IsInvalid);
            Assert.IsTrue(!a.IsValid);
        }

        [Test]
        public void Circle2d_RadiusSquared()
        {
            var a = new Circle2d(V2d.Zero, 2.0);
            Assert.IsTrue(a.RadiusSquared == 4.0);
        }

        [Test]
        public void Circle2d_Circumference()
        {
            var a = new Circle2d(V2d.Zero, 3.0);
            Assert.IsTrue(a.Circumference == 6 * PI);
        }
        
        [Test]
        public void Circle2d_Area()
        {
            var a = new Circle2d(V2d.Zero, 3.0);
            Assert.IsTrue(a.Area == 9 * PI);
        }

        [Test]
        public void Circle2d_Contains()
        {
            var a = new Circle2d(new V2d(5, 0), 3);
            Assert.IsTrue(a.Contains(new V2d(7, 2)));
            Assert.IsTrue(a.Contains(new V2d(8, 0)));
            Assert.IsTrue(!a.Contains(new V2d(0, 0)));
            Assert.IsTrue(!a.Contains(new V2d(5, 9)));
            Assert.IsTrue(!a.Contains(new V2d(8.000001, 0)));
        }

        [Test]
        public void Circle2d_InscribedSquare()
        {
            var a = new Circle2d(V2d.Zero, Constant.Sqrt2);
            var s = a.InscribedSquare;
            Assert.IsTrue(s.Center == V2d.Zero);
            Assert.IsTrue(s.Min == new V2d(-1));
            Assert.IsTrue(s.Max == new V2d(+1));
        }

        [Test]
        public void Circle2d_HashCode1()
        {
            var a = new Circle2d(V2d.Zero, 1.0);
            var b = new Circle2d(V2d.Zero, 2.0);
            Assert.IsTrue(a.GetHashCode() != b.GetHashCode());
        }

        [Test]
        public void Circle2d_HashCode2()
        {
            var a = new Circle2d(V2d.Zero, 1.0);
            var b = new Circle2d(V2d.Zero, double.NaN);
            Assert.IsTrue(a.GetHashCode() != b.GetHashCode());
        }

        [Test]
        public void Circle2d_HashCode3()
        {
            var a = new Circle2d(V2d.Zero, 1.0);
            var b = new Circle2d(V2d.NaN, 1.0);
            Assert.IsTrue(a.GetHashCode() != b.GetHashCode());
        }

        [Test]
        public void Circle2d_Equals()
        {
            var a = new Circle2d(V2d.Zero, 1.0);
            var b = new Circle2d(V2d.Zero, 1.0);
            Assert.IsTrue(a.Equals(b));
            Assert.IsTrue(!a.Equals(null));
        }

        [Test]
        public void Circle2d_Parse1()
        {
            var a = new Circle2d(new V2d(1.2, 3.4), 5.6);
            var b = Circle2d.Parse(a.ToString());
            Assert.IsTrue(a.Equals(b));
        }

        [Test]
        public void Circle2d_Parse2()
        {
            var a = new Circle2d(new V2d(1.2, 3.4), double.NaN);
            Assert.IsTrue(Circle2d.Parse(a.ToString()).IsInvalid);
        }

        [Test]
        public void Circle2d_Parse3()
        {
            var a = new Circle2d(new V2d(double.NaN, 3.4), 5.6);
            Assert.IsTrue(Circle2d.Parse(a.ToString()).IsInvalid);
        }

        [Test]
        public void Circle2d_Parse4()
        {
            var a = new Circle2d(new V2d(1.2, double.NaN), 5.6);
            Assert.IsTrue(Circle2d.Parse(a.ToString()).IsInvalid);
        }

        [Test]
        public void Circle2d_Parse5()
        {
            var a = new Circle2d(V2d.NaN, 5.6);
            Assert.IsTrue(Circle2d.Parse(a.ToString()).IsInvalid);
        }

        [Test]
        public void Circle2d_Parse6()
        {
            var a = new Circle2d(V2d.NaN, double.NaN);
            Assert.IsTrue(Circle2d.Parse(a.ToString()).IsInvalid);
        }

        [Test]
        public void Circle2d_BoundingBox1()
        {
            var a = Circle2d.Unit;
            Assert.IsTrue(a.BoundingBox2d == new Box2d(new V2d(-1, -1), new V2d(+1, +1)));
        }
        
        [Test]
        public void Circle2d_BoundingBox2()
        {
            var a = Circle2d.Invalid;
            var bb = a.BoundingBox2d;
            Assert.IsTrue(bb.IsInvalid);
        }

        [Test]
        public void Circle2d_GetBoundingCircle2d()
        {
            var a = Box2d.Unit;
            var b = a.GetBoundingCircle2d();
            var c = new Circle2d(new V2d(0.5, 0.5), Constant.Sqrt2Half);
            Assert.IsTrue(b.Equals(c));
        }

        [Test]
        public void Circle2d_GetBoundingCircle2d_Invalid()
        {
            var a = Box2d.Invalid;
            var b = a.GetBoundingCircle2d();
            Assert.IsTrue(b.IsInvalid);
        }
    }
}
