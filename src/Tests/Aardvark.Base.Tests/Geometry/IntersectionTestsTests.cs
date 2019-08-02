using Aardvark.Base;
using NUnit.Framework;
using System;
using static System.Math;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class IntersectionTestsTests
    {
        #region Contains

        #region Triangle2d contains *

        [Test]
        public void Contains_Triangle2d_V2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new V2d(1, 1)));
            Assert.IsTrue(t.Contains(new V2d(2, 0)));
            Assert.IsTrue(!t.Contains(new V2d(0, 0.000001)));
        }

        [Test]
        public void Contains_Triangle2d_Line2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new Line2d(new V2d(0, 0), new V2d(2, 0))));
            Assert.IsTrue(t.Contains(new Line2d(new V2d(2, 0), new V2d(1, 2))));
            Assert.IsTrue(t.Contains(new Line2d(new V2d(1, 2), new V2d(0, 0))));
            Assert.IsTrue(t.Contains(new Line2d(new V2d(0.5, 0.1), new V2d(1.5, 0.2))));
            Assert.IsTrue(!t.Contains(new Line2d(new V2d(0.5, 0.1), new V2d(2, 2))));
            Assert.IsTrue(!t.Contains(new Line2d(new V2d(0.1, 0.2), new V2d(1.1, 10))));
        }

        [Test]
        public void Contains_Triangle2d_Triangle2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2))));
            Assert.IsTrue(t.Contains(new Triangle2d(new V2d(0.2, 0.1), new V2d(1.5, 0.5), new V2d(1, 1.5))));
            Assert.IsTrue(!t.Contains(new Triangle2d(new V2d(0.2, 0.1), new V2d(1.5, 0.5), new V2d(1, 2.000001))));
            Assert.IsTrue(!t.Contains(new Triangle2d(new V2d(-1, -1), new V2d(3, -1), new V2d(1, 5))));
        }

        [Test]
        public void Contains_Triangle2d_Quad2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new Quad2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2), new V2d(1, 2))));
            Assert.IsTrue(t.Contains(new Quad2d(new V2d(0, 0), new V2d(2, 0), new V2d(1.5, 0.2), new V2d(0.5, 0.2))));
            Assert.IsTrue(!t.Contains(new Quad2d(new V2d(0, 0), new V2d(2, 0), new V2d(1.5, 0.2), new V2d(0.5, 1.1))));
            Assert.IsTrue(!t.Contains(new Quad2d(new V2d(-1, -1), new V2d(5, -1), new V2d(5, 5), new V2d(0, 5))));
        }

        [Test, Ignore("not implemented")]
        public void Contains_Triangle2d_Circle2d()
        {
            var t = new Triangle2d(new V2d(0, 0), new V2d(2, 0), new V2d(1, 2));
            Assert.IsTrue(t.Contains(new Circle2d(new V2d(1, 1), 0.5)));
            Assert.IsTrue(t.Contains(new Circle2d(new V2d(1, 2), 0.0)));
            Assert.IsTrue(!t.Contains(new Circle2d(new V2d(1, 0.1), 0.100001)));
            Assert.IsTrue(!t.Contains(new Circle2d(new V2d(1, 2), 10)));
        }

        #endregion

        #region Circle2d contains *

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

        #endregion

        #endregion

        #region Intersects

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
                new Line2d(new V2d(0, 0), new V2d(2, 0)).IntersectsLine(
                new V2d(1, 0), new V2d(3, 0), true, out var foo1
                ));

            // on same line, touching
            Assert.IsTrue(
                new Line2d(new V2d(0, 0), new V2d(2, 0)).IntersectsLine(
                new V2d(2, 0), new V2d(4, 0), true, out var foo2
                ));
        }

        #endregion
    }
}
