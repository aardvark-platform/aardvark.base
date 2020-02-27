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
            TrafoTesting.GenericTest(rnd =>
            {
                var rot = rnd.UniformV3dFull() * Constant.PiTimesFour - Constant.PiTimesTwo;

                var mat = M44d.RotationEuler(rot);
                var mat2 = (M44d)Rot3d.FromM33d((M33d)mat);

                Assert.IsFalse(mat.Elements.Any(x => x.IsNaN()), "NaN");

                if (!Fun.ApproximateEquals(mat, mat2, 1e-9))
                    Assert.Fail("FAIL");
            });
        }

        [Test]
        public static void FromM44d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot3, a => (M44d)a, b => Rot3d.FromM44d(b));

        [Test]
        public static void FromEuclidean3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot3, a => (Euclidean3d)a, b => Rot3d.FromEuclidean3d(b));

        [Test]
        public static void FromSimilarity3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot3, a => (Similarity3d)a, b => Rot3d.FromSimilarity3d(b));

        [Test]
        public static void FromAffine3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot3, a => (Affine3d)a, b => Rot3d.FromAffine3d(b));

        [Test]
        public static void FromTrafo3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot3, a => (Trafo3d)a, b => Rot3d.FromTrafo3d(b));

        [Test]
        public static void ToStringAndParse()
            => TrafoTesting.GenericToStringAndParseTest(TrafoTesting.GetRandomRot3, Rot3d.Parse);

        [Test]
        public static void MultiplicationRotTest()
            => TrafoTesting.GenericMultiplicationTest<Rot3d, Rot3d, Rot3d>(TrafoTesting.GetRandomRot3, TrafoTesting.GetRandomRot3, Rot.Transform); 

        [Test]
        public static void MultiplicationShiftTest()
            => TrafoTesting.GenericMultiplicationTest<Rot3d, Shift3d, Euclidean3d>(TrafoTesting.GetRandomRot3, TrafoTesting.GetRandomShift3, Euclidean.TransformPos);        
        [Test]
        public static void MultiplicationScaleTest()
            => TrafoTesting.GenericMultiplicationTest<Rot3d, Scale3d, Affine3d>(TrafoTesting.GetRandomRot3, TrafoTesting.GetRandomScale3, Affine.TransformPos);

        [Test]
        public static void RotationXYZ()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var angle = rnd.UniformDouble() * Constant.PiTimesFour - Constant.PiTimesTwo;

                var rotX1 = Rot3d.RotationX(angle);
                var rotX2 = Rot3d.Rotation(V3d.XAxis, angle);

                TrafoTesting.AreEqual(rotX1, rotX2);

                var rotY1 = Rot3d.RotationY(angle);
                var rotY2 = Rot3d.Rotation(V3d.YAxis, angle);

                TrafoTesting.AreEqual(rotY1, rotY2);

                var rotZ1 = Rot3d.RotationZ(angle);
                var rotZ2 = Rot3d.Rotation(V3d.ZAxis, angle);

                TrafoTesting.AreEqual(rotZ1, rotZ2);
            });
        }

        [Test]
        public static void FromEuler()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var euler = rnd.UniformV3dFull() * Constant.PiTimesFour - Constant.PiTimesTwo;

                var rot = Rot3d.RotationEuler(euler);

                var qx = Rot3d.RotationX(euler.X);
                var qy = Rot3d.RotationY(euler.Y);
                var qz = Rot3d.RotationZ(euler.Z);
                var test = qz * qy * qx;

                TrafoTesting.AreEqual(rot, test);

                var euler2 = rot.GetEulerAngles();
                var rot2 = Rot3d.RotationEuler(euler2);

                var rot2M = (M33d)rot2;
                var rotM = (M33d)rot;

                if (!Fun.ApproximateEquals(rot2M, rotM, 1e-6))
                    Report.Line("FA");

                Assert.IsTrue(Fun.ApproximateEquals(rot2M, rotM, 1e-6));
            });
        }

        [Test]
        public static void YawPitchRoll()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var yaw = rnd.UniformDouble() * Constant.PiTimesFour - Constant.PiTimesTwo;
                var pitch = rnd.UniformDouble() * Constant.PiTimesFour - Constant.PiTimesTwo;
                var roll = rnd.UniformDouble() * Constant.PiTimesFour - Constant.PiTimesTwo;

                // Aardvark euler angles: roll (X), pitch (Y), yaw (Z). Ther are applied in reversed order.
                var mat = (M33d)(M44d.RotationZ(yaw) * M44d.RotationY(pitch) * M44d.RotationX(roll));
                var mat2 = (M33d)M44d.RotationEuler(roll, pitch, yaw);
                var mat3 = (M33d)(Rot3d.RotationZ(yaw) * Rot3d.RotationY(pitch) * Rot3d.RotationX(roll));
                var mat4 = (M33d)Rot3d.RotationEuler(roll, pitch, yaw);

                Assert.IsTrue(Fun.ApproximateEquals(mat, mat2, 1e-7));
                Assert.IsTrue(Fun.ApproximateEquals(mat, mat3, 1e-7));
                Assert.IsTrue(Fun.ApproximateEquals(mat, mat4, 1e-7));
            });
        }

        [Test]
        public static void RotIntoCornerCase()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                // some vectors will not normalize to 1.0 -> provoke numerical issues in Rot3d
                var vecd = new V3d(0, 0, -rnd.UniformDouble());
                var rotd = Rot3d.RotateInto(V3d.OOI, vecd.Normalized);
                var testd = rotd.Transform(V3d.OOI);
                Assert.True((testd + V3d.OOI).Length < 1e-8);

                //var vecf = new V3f(0, 0, -rnd.NextDouble());
                //var rotf = new Rot3f(V3f.OOI, vecf.Normalized);
                //var testf = rotf.TransformDir(V3f.OOI);
                //Assert.True((testf + V3f.OOI).Length < 1e-5);
            });
        }

        [Test]
        public static void FromInto()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var dir = rnd.UniformV3d().Normalized;
                var rotId = Rot3d.RotateInto(dir, dir);
                var matId = (M33d)rotId;

                Assert.IsTrue(matId.IsIdentity(0));

                var rot = Rot3d.RotateInto(dir, -dir);
                var invDir = rot.Transform(dir);

                Assert.IsTrue(invDir.ApproximateEquals(-dir, 1e-14));

                var dirF = rnd.UniformV3f().Normalized;
                var rotIdF = Rot3f.RotateInto(dirF, dirF);
                var matIdF = (M33f)rotIdF;

                Assert.IsTrue(matIdF.IsIdentity(0));

                var rotF = Rot3f.RotateInto(dirF, -dirF);
                var invDirF = rotF.Transform(dirF);

                Assert.IsTrue(invDirF.ApproximateEquals(-dirF, 1e-6f));
            });
        }

        [Test]
        public static void FromIntoEpislon()
        {
            TrafoTesting.GenericTest((rnd, i) =>
            {
                var dir = rnd.UniformV3d().Normalized;
                var eps = rnd.UniformV3d() * (i / 100) * 1e-22;
                var rotId = Rot3d.RotateInto(dir, (dir + eps).Normalized);
                var matId = (M33d)rotId;

                Assert.IsTrue(matId.IsIdentity(1e-10));

                var dirF = rnd.UniformV3f().Normalized;
                var epsF = rnd.UniformV3f() * (i / 100) * 1e-12f;
                var rotIdF = Rot3f.RotateInto(dirF, (dirF + epsF).Normalized);
                var matIdF = (M33f)rotIdF;

                Assert.IsTrue(matIdF.IsIdentity(1e-7f));
            });
        }
    }
}
