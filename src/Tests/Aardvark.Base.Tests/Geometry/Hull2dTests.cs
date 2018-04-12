using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Hull2dTests
    {
        [Test]
        public void ComputeCorners_Square()
        {
            var hull = new Hull2d(new[]
            {
                new Plane2d(-V2d.IO, V2d.Zero),
                new Plane2d(-V2d.OI, V2d.Zero),
                new Plane2d(V2d.IO, V2d.One),
                new Plane2d(V2d.OI, V2d.One),
            });

            var corners = hull.ComputeCorners();

            var expectedCorners = new [] { V2d.OO, V2d.OI, V2d.IO, V2d.II };

            Assert.IsTrue(corners.SetEquals(expectedCorners));
        }

        [Test]
        public void ComputeCorners_Frustum()
        {
            var hull = new Hull2d(new[]
            {
                new Plane2d(new V2d(0, -1), V2d.OO),
                new Plane2d(new V2d(0, +1), new V2d(0, 0.25)),
                new Plane2d(new V2d(-1, +1).Normalized, V2d.OO),
                new Plane2d(new V2d(+1, +1).Normalized, V2d.IO),
            });

            var expectedCorners = new []
            {
                V2d.OO, V2d.IO,
                new V2d(0.25, 0.25),
                new V2d(0.75, 0.25),
            };

            var corners = hull.ComputeCorners();

            Assert.IsTrue(corners.SetEquals(expectedCorners));
        }
    }
}
