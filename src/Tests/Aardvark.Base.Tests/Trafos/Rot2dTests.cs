using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class Rot2dTests
    {
        private static readonly int iterations = 10000;

        private static Rot2d GetRandomRot(RandomSystem rnd)
        {
            return new Rot2d(rnd.UniformDouble() * Constant.PiTimesTwo);
        }

        [Test]
        public static void DistanceTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var delta = (rnd.UniformDouble() - 0.5) * 2.0 * Constant.Pi;
                var r1 = GetRandomRot(rnd);
                var r2 = new Rot2d(r1.Angle + (delta + Constant.PiTimesTwo * rnd.UniformInt(10)));

                var dist = r1.Distance(r2);
                Assert.IsTrue(Fun.ApproximateEquals(dist, Fun.Abs(delta), 0.00001), "{0} != {1}", dist, delta);
            }
        }

        [Test]
        public static void InverseTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var r = GetRandomRot(rnd);

                var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                var q = r.Transform(p);

                // Inverse property
                var res = r.Inverse.Transform(q);

                // Invert method
                Rot.Invert(ref r);
                var res2 = r.Transform(q);

                Assert.IsTrue(Fun.ApproximateEquals(p, res, 0.00001), "{0} != {1}", p, res);
                Assert.IsTrue(Fun.ApproximateEquals(p, res2, 0.00001), "{0} != {1}", p, res2);
            }
        }

        [Test]
        public static void Multiplication2x2Test()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var r1 = GetRandomRot(rnd);
                var r2 = GetRandomRot(rnd);
                var r = r1 * r2;
                var rm = (M22d)r1 * r2;
                var mr = r1 * (M22d)r2;
                var m = (M22d)r1 * (M22d)r2;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                    var res = r.Transform(p);
                    var res2 = m.Transform(p);
                    var res3 = rm.Transform(p);
                    var res4 = mr.Transform(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res3, 0.00001), "{0} != {1}", res, res3);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res4, 0.00001), "{0} != {1}", res, res4);
                }
            }
        }

        [Test]
        public static void Multiplication3x3Test()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var r1 = GetRandomRot(rnd);
                var r2 = GetRandomRot(rnd);
                var r = r1 * r2;
                var rm = (M33d)r1 * r2;
                var mr = r1 * (M33d)r2;
                var m = (M33d)r1 * (M33d)r2;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                    var res = r.Transform(p);
                    var res2 = m.TransformPos(p);
                    var res3 = rm.TransformPos(p);
                    var res4 = mr.TransformPos(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res3, 0.00001), "{0} != {1}", res, res3);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res4, 0.00001), "{0} != {1}", res, res4);
                }
            }
        }

        [Test]
        public static void FromM22d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var angle = rnd.UniformDouble() * Constant.PiTimesTwo;
                var m = Rot2d.FromM22d(M22d.Rotation(angle));
                var r = new Rot2d(angle);
                var rmr = Rot2d.FromM22d((M22d)r);

                Assert.IsTrue(Fun.ApproximateEquals(m, r, 0.00001), "{2}: {0} != {1}", m, r, i);
                Assert.IsTrue(Fun.ApproximateEquals(rmr, r, 0.00001), "{2}: {0} != {1}", rmr, r, i);
            }
        }

        [Test]
        public static void FromM33d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var angle = rnd.UniformDouble() * Constant.PiTimesTwo;
                var m = Rot2d.FromM33d(M33d.Rotation(angle));
                var r = new Rot2d(angle);
                var rmr = Rot2d.FromM33d((M33d)r);

                Assert.IsTrue(Fun.ApproximateEquals(m, r, 0.00001), "{2}: {0} != {1}", m, r, i);
                Assert.IsTrue(Fun.ApproximateEquals(rmr, r, 0.00001), "{2}: {0} != {1}", rmr, r, i);
            }
        }

        [Test]
        public static void FromEuclidean2d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomRot(rnd);
                var m = (Euclidean2d)a;

                var restored = Rot2d.FromEuclidean2d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromSimilarity2d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomRot(rnd);
                var m = (Similarity2d)a;

                var restored = Rot2d.FromSimilarity2d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromAffine2d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomRot(rnd);
                var m = (Affine2d)a;

                var restored = Rot2d.FromAffine2d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromTrafo2d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomRot(rnd);
                var m = (Trafo2d)a;

                var restored = Rot2d.FromTrafo2d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void ConsistentWithMatrixRotationTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var angle = rnd.UniformDouble() * Constant.PiTimesTwo;
                var m = M22d.Rotation(angle);
                var r = new Rot2d(angle);

                var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                var res = m.Transform(p);
                var res2 = r.Transform(p);

                Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
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
                var parsed = Rot2d.Parse(str);

                Assert.IsTrue(Fun.ApproximateEquals(parsed, r, 0.00001));
            }
        }
    }
}
