using Aardvark.Base;
using NUnit.Framework;
using System;
using static System.Math;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class IntersectionTestsTests
    {
        [Test]
        public void Intersects_Circle2d_Circle2d()
        {
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(0, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(0, 0), 0.5)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 0.5).Intersects(new Circle2d(new V2d(0, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(1, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(2, 0), 1)));
            
            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 1).Intersects(new Circle2d(new V2d(2.000001, 0), 1)));
        }
    }
}
