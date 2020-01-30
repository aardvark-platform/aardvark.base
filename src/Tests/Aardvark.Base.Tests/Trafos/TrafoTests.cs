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
            var rnd = new RandomSystem(1123);
            for (int i = 0; i < 10000; i++)
            {
                var jitter = (i / 100) * 1e-15;
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.YAxis, V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.ZAxis, V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.XAxis, V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.ZAxis, V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.YAxis, V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.XAxis, V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.YAxis, V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.ZAxis, V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.XAxis, V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.ZAxis, V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.YAxis, V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.XAxis, V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.YAxis, V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.ZAxis, V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.XAxis, V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.ZAxis, V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.YAxis, V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.XAxis, V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.YAxis, -V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, V3d.ZAxis, -V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.XAxis, -V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, V3d.ZAxis, -V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.YAxis, -V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, V3d.XAxis, -V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.YAxis, V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.ZAxis, V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.XAxis, V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.ZAxis, V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.YAxis, V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.XAxis, V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.YAxis, -V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, V3d.ZAxis, -V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.XAxis, -V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, V3d.ZAxis, -V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.YAxis, -V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, V3d.XAxis, -V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.YAxis, -V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.XAxis, -V3d.ZAxis, -V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.XAxis, -V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.YAxis, -V3d.ZAxis, -V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.YAxis, -V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(V3d.ZAxis, -V3d.XAxis, -V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));

                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.YAxis, -V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.XAxis, -V3d.ZAxis, -V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.XAxis, -V3d.ZAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.YAxis, -V3d.ZAxis, -V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.YAxis, -V3d.XAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
                TestDecompose(Trafo3d.FromOrthoNormalBasis(-V3d.ZAxis, -V3d.XAxis, -V3d.YAxis), new Rot3d(rnd.UniformV3dDirection(), jitter * rnd.UniformDouble()));
            }
        }

        void TestDecompose(Trafo3d trafo, Rot3d jitter)
        {
            var x = (M44d)jitter;
            trafo = trafo * new Trafo3d(x, x.Inverse);

            V3d r_d, s_d, t_d;
            trafo.Decompose(out s_d, out r_d, out t_d);

            var recomposed = Trafo3d.FromComponents(s_d, r_d, t_d);

            Assert.IsFalse(s_d.AnyNaN || r_d.AnyNaN || t_d.AnyNaN, "something NaN");

            var dt = trafo.Forward - recomposed.Forward;
            var e = dt.NormMax.Abs();
            Assert.IsTrue(e < 1e-9, "DIFF");

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
                    Trafo3d.RotateInto(V3d.XAxis, (V3d.XAxis + ry).Normalized),
                    Trafo3d.RotateInto(V3d.YAxis, (V3d.YAxis + rz).Normalized),
                    Trafo3d.RotateInto(V3d.ZAxis, (V3d.ZAxis + rx).Normalized),
                };
                foreach (var r in req)
                    Assert.True(CheckForwardBackwardConsistency(r));

                // 180° cases
                var r180 = new[]
                {
                    Trafo3d.RotateInto(-V3d.XAxis, ( V3d.XAxis + ry).Normalized),
                    Trafo3d.RotateInto( V3d.XAxis, (-V3d.XAxis + rz).Normalized),
                    Trafo3d.RotateInto(-V3d.YAxis, ( V3d.YAxis + rz).Normalized),
                    Trafo3d.RotateInto( V3d.YAxis, (-V3d.YAxis + rx).Normalized),
                    Trafo3d.RotateInto(-V3d.ZAxis, ( V3d.ZAxis + rx).Normalized),
                    Trafo3d.RotateInto( V3d.ZAxis, (-V3d.ZAxis + ry).Normalized)
                };
                foreach (var r in r180)
                    Assert.True(CheckForwardBackwardConsistency(r));

                // 90° cases
                var r90 = new[]
                {
                    Trafo3d.RotateInto((-V3d.XAxis + rz).Normalized, V3d.ZAxis),
                    Trafo3d.RotateInto(( V3d.XAxis + ry).Normalized, V3d.ZAxis),
                    Trafo3d.RotateInto((-V3d.YAxis + rx).Normalized, V3d.ZAxis),
                    Trafo3d.RotateInto(( V3d.YAxis + rz).Normalized, V3d.ZAxis),

                    Trafo3d.RotateInto(-V3d.XAxis, (-V3d.ZAxis + rx).Normalized),
                    Trafo3d.RotateInto( V3d.XAxis, (-V3d.ZAxis + rx).Normalized),
                    Trafo3d.RotateInto(-V3d.YAxis, (-V3d.ZAxis + ry).Normalized),
                    Trafo3d.RotateInto( V3d.YAxis, (-V3d.ZAxis + ry).Normalized),
                };
                foreach (var r in r90)
                    Assert.True(CheckForwardBackwardConsistency(r));
            }
        }
        
        void ValidateTrafos(Trafo3d a, Trafo3d b)
        {
            var e = M44d.DistanceMax(a.Forward, b.Forward);
            Assert.IsTrue(e.Abs() < 1e-8, "not equal");

            var eq = CheckForwardBackwardConsistency(new Trafo3d(a.Forward, b.Backward))
                  && CheckForwardBackwardConsistency(new Trafo3d(b.Forward, a.Backward));

            Assert.True(eq, "trafo not consistent");
        }

        bool CheckForwardBackwardConsistency(Trafo3d trafo)
        {
            var i = trafo.Forward * trafo.Backward;
            // i should be Identity
            return i.C0.ApproximateEquals(V4d.IOOO, 1e-7)
                && i.C1.ApproximateEquals(V4d.OIOO, 1e-7)
                && i.C2.ApproximateEquals(V4d.OOIO, 1e-7)
                && i.C3.ApproximateEquals(V4d.OOOI, 1e-7);
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
