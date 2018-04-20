using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Tests.Plane
{
    [TestFixture]
    public class PlaneTest : TestSuite
    {
        public PlaneTest() : base() { }
        public PlaneTest(TestSuite.Options options) : base(options) { }
        
        [Test]
        public void ProjectUnprojectTest()
        {
            var normal = new V3d(-3.38012399699281E-17, -0.000407306344606881, -0.999999917050768);
            var distance = -13.109839358218661;
            var plane = new Plane3d(normal, distance);

            var testPoint = new V3d(-97.076092093139, -148.399088699813, 13.170284341045);

            // NOTE: Rot3d in Euclidean3d has numerical issue
            var projectedPoint = plane.ProjectToPlaneSpace(testPoint);
            var unprojectedPoint = plane.Unproject(projectedPoint);

            var planeTrafo = Trafo3d.FromNormalFrame(plane.Point, plane.Normal);
            var projectedPoint2 = planeTrafo.Backward.TransformPos(testPoint).XY;
            var unprojectedPoint2 = planeTrafo.Forward.TransformPos(projectedPoint2.XYO);

            if (!((unprojectedPoint - unprojectedPoint2).Length < 0.01)) throw new Exception();
        }
    }
}
