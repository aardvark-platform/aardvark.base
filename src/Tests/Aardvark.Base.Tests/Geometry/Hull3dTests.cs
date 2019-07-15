using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Hull3dTests
    {
        [Test]
        public void ComputeCorners_Cube()
        {
            var hull = new Hull3d(new[]
            {
                new Plane3d(-V3d.XAxis, V3d.Zero),
                new Plane3d(-V3d.YAxis, V3d.Zero),
                new Plane3d(-V3d.ZAxis, V3d.Zero),
                new Plane3d(V3d.XAxis, V3d.One),
                new Plane3d(V3d.YAxis, V3d.One),
                new Plane3d(V3d.ZAxis, V3d.One)
            });

            var corners = hull.ComputeCorners();

            var expectedCorners = new [] { V3d.OOO, V3d.OOI, V3d.OIO, V3d.OII, V3d.IOO, V3d.IOI, V3d.IIO, V3d.III };

            Assert.IsTrue(corners.SetEquals(expectedCorners));
        }

        [Test]
        public void ComputeCorners_Frustum()
        {
            var hull = new Hull3d(new[]
            {
                new Plane3d(new V3d(0, 0, -1), V3d.OOO),
                new Plane3d(new V3d(0, 0, +1), new V3d(0, 0, 0.25)),
                new Plane3d(new V3d(-1, 0, +1).Normalized, V3d.OOO),
                new Plane3d(new V3d(+1, 0, +1).Normalized, V3d.IOO),
                new Plane3d(new V3d(0, -1, +1).Normalized, V3d.OOO),
                new Plane3d(new V3d(0, +1, +1).Normalized, V3d.OIO),
            });

            var expectedCorners = new []
            {
                V3d.OOO, V3d.IOO, V3d.IIO, V3d.OIO,
                new V3d(0.25, 0.25, 0.25),
                new V3d(0.75, 0.25, 0.25),
                new V3d(0.75, 0.75, 0.25),
                new V3d(0.25, 0.75, 0.25),
            };

            var corners = hull.ComputeCorners();

            Assert.IsTrue(corners.SetEquals(expectedCorners));
        }

        [Test]
        public void VisualHullTest()
        {
            var view = Trafo3d.ViewTrafoRH(V3d.III, V3d.OOI, V3d.IOO);
            var proj = Trafo3d.PerspectiveProjectionOpenGl(-1, 1, -1, 1, 1, 100);
            var vpTrafo = view * proj;
            var frustumCorners = new Box3d(-V3d.III, V3d.III).ComputeCorners();

            //Min,                             0 near left bottom
            //new V3d(Max.X, Min.Y, Min.Z),    1 near right bottom
            //new V3d(Min.X, Max.Y, Min.Z),    2 near left top
            //new V3d(Max.X, Max.Y, Min.Z),    3 near right top
            //new V3d(Min.X, Min.Y, Max.Z),    4 far left bottom
            //new V3d(Max.X, Min.Y, Max.Z),    5 far right bottom
            //new V3d(Min.X, Max.Y, Max.Z),    6 far left top
            //Max                              7 far right top

            // use inverse view-projection to get vertices in world space
            frustumCorners.Apply(c => vpTrafo.Backward.TransformPosProj(c));

            // hull planes should point outside, assume right-handed transformation to build planes
            var refHull = new Hull3d(new[]
            {
                new Plane3d(frustumCorners[0], frustumCorners[2], frustumCorners[4]), // left
                new Plane3d(frustumCorners[1], frustumCorners[5], frustumCorners[3]), // right
                new Plane3d(frustumCorners[0], frustumCorners[4], frustumCorners[1]), // bottom
                new Plane3d(frustumCorners[2], frustumCorners[3], frustumCorners[6]), // top
                new Plane3d(frustumCorners[0], frustumCorners[1], frustumCorners[2]), // near
                new Plane3d(frustumCorners[4], frustumCorners[6], frustumCorners[5]), // far
            });

            var newHull = vpTrafo.GetVisualHull();

            for (int i = 0; i < 6; i++)
                Report.Line("OLD: {0} NEW: {1}", newHull.PlaneArray[i], refHull.PlaneArray[i]);

            // camera position should have height 1.0 from near-plane
            var hRef = refHull.PlaneArray[4].Height(view.GetViewPosition());
            var hNew = newHull.PlaneArray[4].Height(view.GetViewPosition());
            Assert.True(hRef.ApproximateEquals(1.0, 1e-7) && hRef.ApproximateEquals(hNew, 1e-7));
            
            for (int i = 0; i < 6; i++)
                Assert.True(newHull.PlaneArray[i].Coefficients.ApproxEqual(refHull.PlaneArray[i].Coefficients, 1e-7));
        }
    }
}
