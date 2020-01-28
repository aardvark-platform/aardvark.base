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
                var q = Rot3d.FromM33d((M33d)mat); // TODO tweak default epsilon
                //var q = Rot3d.FromM33d((M33d)mat, 1e-16); // this epsilon work...

                var mat2 = (M44d)q;

                Assert.IsFalse(mat.Elements.Any(x => x.IsNaN()), "NaN");

                if (!Fun.ApproximateEquals(mat, mat2, 1e-8))
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
            }
        }

        [Test]
        public static void YawPitchRoll()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < 100000; i++)
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
    }
}
