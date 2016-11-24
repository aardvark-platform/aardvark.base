using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Aardvark.Base;

namespace Aardvark.Tests
{
    [TestFixture]
    public class TrafoTests : TestSuite
    {
        [Test]
        public void TrafoDecomposeTest()
        {
            var rnd = new RandomSystem();
            for (int i = 0; i < 100000; i ++)
            {
                var rot = rnd.UniformV3dFull() * Constant.PiTimesFour - Constant.PiTimesTwo;
                var trans = rnd.UniformV3dFull() * 10 - 5;
                var scale = rnd.UniformV3dFull() * 4 - 2;

                TestDecompose(scale, rot, trans);
            }
        }

        void TestDecompose(V3d scale, V3d rotation, V3d translation)
        {
            var trafo = Trafo3d.FromComponents(scale, rotation, translation);
            V3d r_d, s_d, t_d;
            trafo.Decompose(out s_d, out r_d, out t_d);

            var recomposed = Trafo3d.FromComponents(s_d, r_d, t_d);

            Assert.IsFalse(s_d.AnyNaN || r_d.AnyNaN || t_d.AnyNaN, "something NaN");

            var eq = CheckForwardBackwardConsistency(new Trafo3d(trafo.Forward, recomposed.Backward))
                && CheckForwardBackwardConsistency(new Trafo3d(recomposed.Forward, trafo.Backward));

            Assert.True(eq, "trafo not consistent");
        }

        [Test]
        public void TrafoRotIntoTest()
        {
            var rnd = new Random();

            for (int i = 0; i < 500000; i++)
            {
                var rx = new V3d(rnd.NextDouble() * 1e-17, 0, 0) * (rnd.Next(100) >= 50 ? 1: -1);
                var ry = new V3d(0, rnd.NextDouble() * 1e-17, 0) * (rnd.Next(100) >= 50 ? 1 : -1);
                var rz = new V3d(0, 0, rnd.NextDouble() * 1e-17) * (rnd.Next(100) >= 50 ? 1 : -1);

                // equal cases
                var req = new[]
                {
                    Trafo3d.RotateInto(V3d.XAxis, V3d.XAxis + ry),
                    Trafo3d.RotateInto(V3d.YAxis, V3d.YAxis + rz),
                    Trafo3d.RotateInto(V3d.ZAxis, V3d.ZAxis + rx),
                };
                foreach (var r in req)
                    Assert.True(CheckForwardBackwardConsistency(r));

                // 180° cases
                var r180 = new[]
                {
                    Trafo3d.RotateInto(-V3d.XAxis,  V3d.XAxis + ry),
                    Trafo3d.RotateInto( V3d.XAxis, -V3d.XAxis + rz),
                    Trafo3d.RotateInto(-V3d.YAxis,  V3d.YAxis + rz),
                    Trafo3d.RotateInto( V3d.YAxis, -V3d.YAxis + rx),
                    Trafo3d.RotateInto(-V3d.ZAxis,  V3d.ZAxis + rx),
                    Trafo3d.RotateInto( V3d.ZAxis, -V3d.ZAxis + ry)
                };
                foreach (var r in r180)
                    Assert.True(CheckForwardBackwardConsistency(r));

                // 90° cases
                var r90 = new[]
                {
                    Trafo3d.RotateInto(-V3d.XAxis + rz, V3d.ZAxis),
                    Trafo3d.RotateInto( V3d.XAxis + ry, V3d.ZAxis),
                    Trafo3d.RotateInto(-V3d.YAxis + rx, V3d.ZAxis),
                    Trafo3d.RotateInto( V3d.YAxis + rz, V3d.ZAxis),

                    Trafo3d.RotateInto(-V3d.XAxis, -V3d.ZAxis + rx),
                    Trafo3d.RotateInto( V3d.XAxis, -V3d.ZAxis + rx),
                    Trafo3d.RotateInto(-V3d.YAxis, -V3d.ZAxis + ry),
                    Trafo3d.RotateInto( V3d.YAxis, -V3d.ZAxis + ry),
                };
                foreach (var r in r90)
                    Assert.True(CheckForwardBackwardConsistency(r));
            }
        }
        
        bool CheckForwardBackwardConsistency(Trafo3d trafo)
        {
            var i = trafo.Forward * trafo.Backward;
            // i should be Identity
            return i.C0.ApproxEqual(V4d.IOOO, 1e-1)
                && i.C1.ApproxEqual(V4d.OIOO, 1e-1)
                && i.C2.ApproxEqual(V4d.OOIO, 1e-1)
                && i.C3.ApproxEqual(V4d.OOOI, 1e-1);
        }
    }
}
