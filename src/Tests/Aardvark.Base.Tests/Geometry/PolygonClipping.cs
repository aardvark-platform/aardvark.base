using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Tests.Geometry
{

    [TestFixture]
    public class PolygonClipping : TestSuite
    {
        public PolygonClipping() : base() { }
        public PolygonClipping(TestSuite.Options options) : base(options) { }

        [Test]
        public void TestConvexClipped()
        {
            var points = new[] { V3d.OOO, V3d.IOO, V3d.OIO };
            var poly = new Polygon3d(points);
            var box = Box3d.FromMinAndSize(-V3d.OOI, new V3d(1, 0.5, 2));
            var newHull = new Hull3d(box).Reversed(); // requires non-intuitive reversed
            var polyClipped = poly.ConvexClipped(newHull); // will return positive part of planes (outside of Hull3d)
            var clippedBox = polyClipped.BoundingBox3d;
            var test = new Box3d(0, 0, 0, 1, 0.5, 0);
            Test.IsTrue(clippedBox == test);
        }
    }
}
