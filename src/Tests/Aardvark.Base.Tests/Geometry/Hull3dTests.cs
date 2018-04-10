using Aardvark.Base;
using NUnit.Framework;
using System.Linq;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class Hull3dTests : TestSuite
    {
        [Test]
        public void TestCornersComputedCorrectly()
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

            Test.IsTrue(expectedCorners.Intersect(corners).Count() == expectedCorners.Count());
        }
    }
}
