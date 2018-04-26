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
        public void Contains_Circle2d_Circle2d()
        {
            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 0.5).Contains(new Circle2d(new V2d(0, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(0, 0), 1)));
            Assert.IsTrue(new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(0, 0), 0.5)));
            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(1, 0), 1)));
            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(2, 0), 1)));

            Assert.IsTrue(!new Circle2d(new V2d(0, 0), 1).Contains(new Circle2d(new V2d(2.000001, 0), 1)));
        }

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

        [Test]
        public void Intersects_Line2d_Line2d()
        {
            // non-parallel, crossing
            Assert.IsTrue(
                new Line2d(new V2d(0,  0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(1, -1), new V2d(1, 1))
                ));

            // non-parallel, touching
            Assert.IsTrue(
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(1, 0), new V2d(1, 1))
                ));

            // non-parallel, almost touching
            Assert.IsTrue(!
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(1, 0.000001), new V2d(1, 1))
                ));

            // identical
            Assert.IsTrue(
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(0, 0), new V2d(2, 0))
                ));
            
            // on same line, almost touching
            Assert.IsTrue(!
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(2.000001, 0), new V2d(4, 0))
                ));

            // on same line, not touching
            Assert.IsTrue(!
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(3, 0), new V2d(5, 0))
                ));
        }

        [Test]
        public void Intersects_Line2d_Line2d_ParallelAndNotTouching()
        {
            // parallel, not touching
            Assert.IsTrue(!
                new Line2d(new V2d(1, 0), new V2d(3, 0)).Intersects(
                new Line2d(new V2d(2, 5), new V2d(4, 5))
                ));
        }

        [Test]
        public void Intersects_Line2d_Line2d_Overlapping()
        {
            // on same line, overlapping
            Assert.IsTrue(
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(1, 0), new V2d(3, 0))
                ));

            // on same line, touching
            Assert.IsTrue(
                new Line2d(new V2d(0, 0), new V2d(2, 0)).Intersects(
                new Line2d(new V2d(2, 0), new V2d(4, 0))
                ));
        }
    }
}
