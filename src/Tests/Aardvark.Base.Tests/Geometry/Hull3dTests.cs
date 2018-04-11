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

            var expectedCorners = new V3d[] { V3d.OOO, V3d.OOI, V3d.OIO, V3d.OII, V3d.IOO, V3d.IOI, V3d.IIO, V3d.III };

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

            var expectedCorners = new V3d[]
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
    }
}
