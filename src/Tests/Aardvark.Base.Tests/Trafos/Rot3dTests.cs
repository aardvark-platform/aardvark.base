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
        private static readonly int iterations = 10000;

        private static Rot3d GetRandomRot(RandomSystem rnd)
        {
            return Rot3d.Rotation(rnd.UniformV3dDirection(), rnd.UniformDouble() * Constant.PiTimesTwo);
        }

        [Test]
        public static void FromM33d()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < iterations; i++)
            {
                var rot = rnd.UniformV3dFull() * Constant.PiTimesFour - Constant.PiTimesTwo;

                var mat = M44d.RotationEuler(rot);
                var mat2 = (M44d)Rot3d.FromM33d((M33d)mat);

                Assert.IsFalse(mat.Elements.Any(x => x.IsNaN()), "NaN");

                if (!Fun.ApproximateEquals(mat, mat2, 1e-9))
                    Assert.Fail("FAIL");
            }
        }

        [Test]
        public static void FromM44d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var r = GetRandomRot(rnd);
                var rmr = Rot3d.FromM44d((M44d)r);

                Assert.IsTrue(Fun.ApproximateEquals(rmr, r, 0.00001), "{2}: {0} != {1}", rmr, r, i);
            }
        }

        [Test]
        public static void FromEuclidean3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomRot(rnd);
                var m = (Euclidean3d)a;

                var restored = Rot3d.FromEuclidean3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromSimilarity3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomRot(rnd);
                var m = (Similarity3d)a;

                var restored = Rot3d.FromSimilarity3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromAffine3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomRot(rnd);
                var m = (Affine3d)a;

                var restored = Rot3d.FromAffine3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromTrafo3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomRot(rnd);
                var m = (Trafo3d)a;

                var restored = Rot3d.FromTrafo3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void ToStringAndParse()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var r = GetRandomRot(rnd);

                var str = r.ToString();
                var parsed = Rot3d.Parse(str);

                Assert.IsTrue(Fun.ApproximateEquals(parsed, r, 0.00001));
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
            for (int i = 0; i < iterations; i++)
            {
                var angle = rnd.UniformDouble() * Constant.PiTimesFour - Constant.PiTimesTwo;

                var rotX1 = Rot3d.RotationX(angle);
                var rotX2 = Rot3d.Rotation(V3d.XAxis, angle);

                Assert.True(ApproxEq(rotX1, rotX2), "EQUAL");

                var rotY1 = Rot3d.RotationY(angle);
                var rotY2 = Rot3d.Rotation(V3d.YAxis, angle);

                Assert.True(ApproxEq(rotY1, rotY2), "EQUAL");

                var rotZ1 = Rot3d.RotationZ(angle);
                var rotZ2 = Rot3d.Rotation(V3d.ZAxis, angle);

                Assert.True(ApproxEq(rotZ1, rotZ2), "EQUAL");
            }
        }

        [Test]
        public static void FromEuler()
        {
            var rnd = new RandomSystem(1);
            for (int i = 0; i < iterations; i++)
            {
                var euler = rnd.UniformV3dFull() * Constant.PiTimesFour - Constant.PiTimesTwo;
                
                var rot = Rot3d.RotationEuler(euler);

                var qx = Rot3d.RotationX(euler.X);
                var qy = Rot3d.RotationY(euler.Y);
                var qz = Rot3d.RotationZ(euler.Z);
                var test = qz * qy * qx;

                Assert.True(ApproxEq(rot, test), "EQUAL");

                var euler2 = rot.GetEulerAngles();
                var rot2 = Rot3d.RotationEuler(euler2);

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

                // Aardvark euler angles: roll (X), pitch (Y), yaw (Z). Ther are applied in reversed order.
                var mat = (M33d)(M44d.RotationZ(yaw) * M44d.RotationY(pitch) * M44d.RotationX(roll));
                var mat2 = (M33d)M44d.RotationEuler(roll, pitch, yaw);
                var mat3 = (M33d)(Rot3d.RotationZ(yaw) * Rot3d.RotationY(pitch) * Rot3d.RotationX(roll));
                var mat4 = (M33d)Rot3d.RotationEuler(roll, pitch, yaw);

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
                var rotd = Rot3d.RotateInto(V3d.OOI, vecd.Normalized);
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
            for (int i = 0; i < iterations; i++)
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
            }
        }

        [Test]
        public static void FromIntoEpislon()
        {
            var rnd = new RandomSystem(1337);
            for (int i = 0; i < iterations; i++)
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
            }
        }
    }
}
