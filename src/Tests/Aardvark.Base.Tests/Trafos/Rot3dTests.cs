using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class Rot3dTests
    {
        [Test]
        public static void FromM33d()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < 1000000; i++)
            {
                var rot = rnd.UniformV3dFull() * Constant.PiTimesFour - Constant.PiTimesTwo;

                var mat = M44d.Rotation(rot);
                var mat2 = (M44d)Rot3d.FromM33d((M33d)mat);

                Assert.IsFalse(mat.Elements.Any(x => x.IsNaN()), "NaN");

                if (!Fun.ApproximateEquals(mat, mat2, 1e-9))
                    Assert.Fail("FAIL");
            }
        }
        
        static bool ApproxEq(Rot3d a, Rot3d b)
        {
            return a.V.ApproximateEquals(b.V, 1e-8) && a.W.ApproximateEquals(b.W, 1e-8);
        }

        [Test]
        public static void RotationXYZ()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < 1000000; i++)
            {
                var angle = rnd.UniformDouble() * Constant.PiTimesFour - Constant.PiTimesTwo;

                var rotX1 = Rot3d.RotationX(angle);
                var rotX2 = new Rot3d(V3d.XAxis, angle);

                Assert.True(ApproxEq(rotX1, rotX2), "EQUAL");

                var rotY1 = Rot3d.RotationY(angle);
                var rotY2 = new Rot3d(V3d.YAxis, angle);

                Assert.True(ApproxEq(rotY1, rotY2), "EQUAL");

                var rotZ1 = Rot3d.RotationZ(angle);
                var rotZ2 = new Rot3d(V3d.ZAxis, angle);

                Assert.True(ApproxEq(rotZ1, rotZ2), "EQUAL");
            }
        }

        [Test]
        public static void FromEuler()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < 1000000; i++)
            {
                var euler = rnd.UniformV3dFull() * Constant.PiTimesFour - Constant.PiTimesTwo;
                
                var rot = Rot3d.FromEulerAngles(euler);

                var qx = Rot3d.RotationX(euler.X);
                var qy = Rot3d.RotationY(euler.Y);
                var qz = Rot3d.RotationZ(euler.Z);
                var test = qz * qy * qx;

                Assert.True(ApproxEq(rot, test), "EQUAL");

                var euler2 = rot.GetEulerAngles();
                var rot2 = Rot3d.FromEulerAngles(euler2);

                var rot2M = (M33d)rot2;
                var rotM = (M33d)rot;

                if (!Fun.ApproximateEquals(rot2M, rotM, 1e-6))
                    Report.Line("FA");

                Assert.IsTrue(Fun.ApproximateEquals(rot2M, rotM, 1e-6));
            }
        }

        [Test]
        public static void YawPitchRoll()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < 1; i++)
            {
                var yaw = rnd.UniformDouble() * Constant.PiTimesFour - Constant.PiTimesTwo;
                var pitch = rnd.UniformDouble() * Constant.PiTimesFour - Constant.PiTimesTwo;
                var roll = rnd.UniformDouble() * Constant.PiTimesFour - Constant.PiTimesTwo;

                // Aardvark euler angles: Yaw (X), pitch (Y), roll (Z). Ther are applied in that order.
                var mat = (M33d)(M44d.RotationZ(roll) * M44d.RotationY(pitch) * M44d.RotationX(yaw));
                var mat2 = (M33d)M44d.Rotation(yaw, pitch, roll);
                var mat3 = (M33d)(Rot3d.RotationZ(roll) * Rot3d.RotationY(pitch) * Rot3d.RotationX(yaw));
                var mat4 = (M33d)new Rot3d(yaw, pitch, roll);

                Assert.IsTrue(Fun.ApproximateEquals(mat, mat2, 1e-7));
                Assert.IsTrue(Fun.ApproximateEquals(mat, mat3, 1e-7));
                Assert.IsTrue(Fun.ApproximateEquals(mat, mat4, 1e-7));
            }
        }

        [Test]
        public static void RotIntoCornerCase()
        {
            var rnd = new Random(2);
            for (int i = 0; i < 100000; i++)
            {
                // some vectors will not normalize to 1.0 -> provoke numerical issues in Rot3d
                var vecd = new V3d(0, 0, -rnd.NextDouble());
                var rotd = new Rot3d(V3d.OOI, vecd.Normalized);
                var testd = rotd.Transform(V3d.OOI);
                Assert.True((testd + V3d.OOI).Length < 1e-8);

                //var vecf = new V3f(0, 0, -rnd.NextDouble());
                //var rotf = new Rot3f(V3f.OOI, vecf.Normalized);
                //var testf = rotf.TransformDir(V3f.OOI);
                //Assert.True((testf + V3f.OOI).Length < 1e-5);
            }
        }

        [Test]
        public static void FromInto()
        {
            var rnd = new RandomSystem(1337);
            for (int i = 0; i < 1000000; i++)
            {
                var dir = rnd.UniformV3d().Normalized;
                var rotId = new Rot3d(dir, dir);
                var matId = (M33d)rotId;

                Assert.IsTrue(matId.IsIdentity(0));

                var rot = new Rot3d(dir, -dir);
                var invDir = rot.Transform(dir);

                Assert.IsTrue(invDir.ApproximateEquals(-dir, 1e-14));

                var dirF = rnd.UniformV3f().Normalized;
                var rotIdF = new Rot3f(dirF, dirF);
                var matIdF = (M33f)rotIdF;

                Assert.IsTrue(matIdF.IsIdentity(0));

                var rotF = new Rot3f(dirF, -dirF);
                var invDirF = rotF.Transform(dirF);

                Assert.IsTrue(invDirF.ApproximateEquals(-dirF, 1e-6f));
            }
        }

        [Test]
        public static void FromIntoEpislon()
        {
            var rnd = new RandomSystem(1337);
            for (int i = 0; i < 1000000; i++)
            {
                var dir = rnd.UniformV3d().Normalized;
                var eps = rnd.UniformV3d() * (i / 100) * 1e-22;
                var rotId = new Rot3d(dir, (dir + eps).Normalized);
                var matId = (M33d)rotId;

                Assert.IsTrue(matId.IsIdentity(1e-10));

                var dirF = rnd.UniformV3f().Normalized;
                var epsF = rnd.UniformV3f() * (i / 100) * 1e-12f;
                var rotIdF = new Rot3f(dirF, (dirF + epsF).Normalized);
                var matIdF = (M33f)rotIdF;

                Assert.IsTrue(matIdF.IsIdentity(1e-7f));
            }
        }
    }
}
