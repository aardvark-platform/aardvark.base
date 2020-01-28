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
        public void TrafoDecomposeCornerCasesTest()
        {
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.YAxis, V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.ZAxis, V3d.YAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.XAxis, V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.ZAxis, V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.YAxis, V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.XAxis, V3d.YAxis));
            
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.YAxis, V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.ZAxis, V3d.YAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.XAxis, V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.ZAxis, V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.YAxis, V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.XAxis, V3d.YAxis));

            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.YAxis, V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.ZAxis, V3d.YAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.XAxis, V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.ZAxis, V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.YAxis, V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.XAxis, V3d.YAxis));
            
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.YAxis, -V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.ZAxis, -V3d.YAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.XAxis, -V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.ZAxis, -V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.YAxis, -V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.XAxis, -V3d.YAxis));

            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.YAxis, V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.ZAxis, V3d.YAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.XAxis, V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.ZAxis, V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.YAxis, V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.XAxis, V3d.YAxis));

            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.YAxis, -V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.ZAxis, -V3d.YAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.XAxis, -V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.ZAxis, -V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.YAxis, -V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.XAxis, -V3d.YAxis));

            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.YAxis, -V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.ZAxis, -V3d.YAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.XAxis, -V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.ZAxis, -V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.YAxis, -V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.XAxis, -V3d.YAxis));

            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.YAxis, -V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.ZAxis, -V3d.YAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.XAxis, -V3d.ZAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.ZAxis, -V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.YAxis, -V3d.XAxis));
            TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.XAxis, -V3d.YAxis));
        }

        void TestDecompose(Trafo3d trafo)
        {
            trafo = trafo * Trafo3d.RotationXInDegrees(0.000000000000000001);

            V3d r_d, s_d, t_d;
            trafo.Decompose(out s_d, out r_d, out t_d);

            var recomposed = Trafo3d.FromComponents(s_d, r_d, t_d);

            Assert.IsFalse(s_d.AnyNaN || r_d.AnyNaN || t_d.AnyNaN, "something NaN");

            var eq = CheckForwardBackwardConsistency(new Trafo3d(trafo.Forward, recomposed.Backward))
                && CheckForwardBackwardConsistency(new Trafo3d(recomposed.Forward, trafo.Backward));

            Assert.True(eq, "trafo not consistent");
        }

        [Test]
        public void TrafoDecomposeTest()
        {
            var rnd = new RandomSystem(3);
            for (int i = 0; i < 1000000; i ++)
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

            var e_scale = (s_d.Abs() - scale.Abs()).LengthSquared;
            var e_trans = (t_d - translation).LengthSquared;
            Assert.True(e_scale < 1e-5, "Scale");
            Assert.True(e_trans < 1e-5, "Translation");

            ValidateTrafos(trafo, recomposed);
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
        
        void ValidateTrafos(Trafo3d a, Trafo3d b)
        {
            var eq = CheckForwardBackwardConsistency(new Trafo3d(a.Forward, b.Backward))
                  && CheckForwardBackwardConsistency(new Trafo3d(b.Forward, a.Backward));

            Assert.True(eq, "trafo not consistent");
        }

        bool CheckForwardBackwardConsistency(Trafo3d trafo)
        {
            var i = trafo.Forward * trafo.Backward;
            // i should be Identity // TODO: numerical robustness not acceptable
            return i.C0.ApproximateEquals(V4d.IOOO, 1e-4)
                && i.C1.ApproximateEquals(V4d.OIOO, 1e-4)
                && i.C2.ApproximateEquals(V4d.OOIO, 1e-4)
                && i.C3.ApproximateEquals(V4d.OOOI, 1e-4);
        }

        [Test]
        public void TrafoRotIntoCornerCase()
        {
            var rnd = new Random();
            for (int i = 0; i < 1000; i++)
            {
                // some vectors will not normalize to 1.0 -> provoke numerical issues in Rot3d
                var vecd = new V3d(0, 0, -rnd.NextDouble()); 
                var rotd = new Rot3d(V3d.OOI, vecd);
                var testd = rotd.TransformDir(V3d.OOI);
                Assert.True((testd + V3d.OOI).Length < 1e-7);

                var vecf = new V3f(0, 0, -rnd.NextDouble());
                var rotf = new Rot3f(V3f.OOI, vecf);
                var testf = rotf.TransformDir(V3f.OOI);
                Assert.True((testf + V3f.OOI).Length < 1e-3);
            }
        }

        [Test]
        public void CoordsystemTransformTest()
        {
            var sys = new[] 
                {
                  new CoordinateSystem.Info(1, CoordinateSystem.Handedness.Left, CoordinateSystem.Axis.X),
                  new CoordinateSystem.Info(1, CoordinateSystem.Handedness.Right, CoordinateSystem.Axis.X),
                  new CoordinateSystem.Info(1, CoordinateSystem.Handedness.Left, CoordinateSystem.Axis.Y),
                  new CoordinateSystem.Info(1, CoordinateSystem.Handedness.Right, CoordinateSystem.Axis.Y),
                  new CoordinateSystem.Info(1, CoordinateSystem.Handedness.Left, CoordinateSystem.Axis.Z),
                  new CoordinateSystem.Info(1, CoordinateSystem.Handedness.Right, CoordinateSystem.Axis.Z),
                  new CoordinateSystem.Info(10, CoordinateSystem.Handedness.Left, CoordinateSystem.Axis.X),
                  new CoordinateSystem.Info(10, CoordinateSystem.Handedness.Right, CoordinateSystem.Axis.X),
                  new CoordinateSystem.Info(10, CoordinateSystem.Handedness.Left, CoordinateSystem.Axis.Y),
                  new CoordinateSystem.Info(10, CoordinateSystem.Handedness.Right, CoordinateSystem.Axis.Y),
                  new CoordinateSystem.Info(10, CoordinateSystem.Handedness.Left, CoordinateSystem.Axis.Z),
                  new CoordinateSystem.Info(10, CoordinateSystem.Handedness.Right, CoordinateSystem.Axis.Z)
                };

            //foreach(var from in sys)
            //    foreach(var to in sys)
            //    {
            //        // TODO: think about what the correct validation is and implement this test properly...

            //        var direct = CoordinateSystem.FromTo(from, to);
            //        var usingAardvark = CoordinateSystem.ToAardvark(to).Inverse * CoordinateSystem.ToAardvark(from);

            //        ValidateTrafos(direct, usingAardvark);
            //    }
        }
    }
}
