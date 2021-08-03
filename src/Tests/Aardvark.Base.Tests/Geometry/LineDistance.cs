using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Geometry
{
    [TestFixture]
    public class LineDistance
    {
        const int Iter = 10000;
        const int SubDiv = 100;

        [Test]
        public void DistanceToLineAndGetMinimalDistanceToConsistency()
        {
            var line = new Line3d(new V3d(1, 2, 3), new V3d(1, 2, 3));
            var p = V3d.OOO;

            var x = p.DistanceToLine(line.P0, line.P1);
            var md = p.GetMinimalDistanceTo(line);

            Assert.AreEqual(x, md);
        }

        [Test]
        public void LineToPointDistance2d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < Iter; i++)
            {
                var p00 = rnd.UniformV2d();
                var line0 = new Line2d(p00, p00 + rnd.UniformV2d() * rnd.UniformDouble() * 0.2);

                var p01 = rnd.UniformV2d();

                var dist = p01.GetMinimalDistanceTo(line0);

                var refDist = 5.0;
                var r0 = line0.Ray2d;
                for (int j = 0; j <= SubDiv; j++)
                {
                    var x0 = r0.GetPointOnRay(j / (double)SubDiv);
                    refDist = Fun.Min(refDist, Vec.Distance(x0, p01));
                }
                var e = 0.2 / SubDiv;
                Assert.IsTrue(refDist.ApproximateEquals(dist, e));
            }
        }

        [Test]
        public void LineToLineDistance2d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < Iter; i++)
            {
                var p00 = rnd.UniformV2d();
                var line0 = new Line2d(p00, p00 + rnd.UniformV2d() * rnd.UniformDouble() * 0.2);

                var p01 = rnd.UniformV2d();
                var line1 = new Line2d(p01, p01 + rnd.UniformV2d() * rnd.UniformDouble() * 0.2);

                var dist = line0.GetMinimalDistanceTo(line1);

                // Alternative: assuming Line3d distance is correct
                //var refDist = new Line3d(line0.P0.XYO, line0.P1.XYO).GetMinimalDistanceTo(new Line3d(line1.P0.XYO, line1.P1.XYO)); 

                var refDist = 5.0;
                var r0 = line0.Ray2d;
                var r1 = line1.Ray2d;
                for (int j = 0; j <= SubDiv; j++)
                {
                    var x0 = r0.GetPointOnRay(j / (double)SubDiv);
                    for (int k = 0; k <= SubDiv; k++)
                    {
                        var x1 = r1.GetPointOnRay(k / (double)SubDiv);
                        refDist = Fun.Min(refDist, Vec.Distance(x0, x1));
                    }
                }
                var e = 0.2 / SubDiv;
                Assert.IsTrue(refDist.ApproximateEquals(dist, e));
            }
        }

        [Test]
        public void LineToPointDistance3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < Iter; i++)
            {
                var p00 = rnd.UniformV3d();
                var line0 = new Line3d(p00, p00 + rnd.UniformV3d() * rnd.UniformDouble() * 0.2);

                var p01 = rnd.UniformV3d();

                var dist = p01.GetMinimalDistanceTo(line0);

                var refDist = 5.0;
                var r0 = line0.Ray3d;
                for (int j = 0; j <= SubDiv; j++)
                {
                    var x0 = r0.GetPointOnRay(j / (double)SubDiv);
                    refDist = Fun.Min(refDist, Vec.Distance(x0, p01));
                }
                var e = 0.2 / SubDiv;
                Assert.IsTrue(refDist.ApproximateEquals(dist, e));
            }
        }

        [Test]
        public void LineToLineDistance3d()
        {
            var rnd = new RandomSystem(2);

            for (int i = 0; i < Iter; i++)
            {
                var p00 = rnd.UniformV3d();
                var line0 = new Line3d(p00, p00 + rnd.UniformV3d() * rnd.UniformDouble() * 0.2);

                var p01 = rnd.UniformV3d();
                var line1 = new Line3d(p01, p01 + rnd.UniformV3d() * rnd.UniformDouble() * 0.2);

                var dist = line0.GetMinimalDistanceTo(line1);
                var refDist = 5.0;
                var r0 = line0.Ray3d;
                var r1 = line1.Ray3d;
                for (int j = 0; j <= SubDiv; j++)
                {
                    var x0 = r0.GetPointOnRay(j / (double)SubDiv);
                    for (int k = 0; k <= SubDiv; k++)
                    {
                        var x1 = r1.GetPointOnRay(k / (double)SubDiv);
                        refDist = Fun.Min(refDist, Vec.Distance(x0, x1));
                    }
                }
                var e = 0.2 / SubDiv;
                Assert.IsTrue(refDist.ApproximateEquals(dist, e));
            }
        }
    }
}
