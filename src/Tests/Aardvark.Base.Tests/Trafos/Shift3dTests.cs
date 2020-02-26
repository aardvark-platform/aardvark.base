using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class Shift3dTests
    {
        private static readonly int iterations = 10000;

        private static Shift3d GetRandomShift(RandomSystem rnd)
        {
            return new Shift3d(rnd.UniformV3d() * 10);
        }

        [Test]
        public static void FromM34d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomShift(rnd);
                var m = (M34d)a;

                var restored = Shift3d.FromM34d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromM44d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomShift(rnd);
                var m = (M44d)a;

                var restored = Shift3d.FromM44d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromEuclidean3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomShift(rnd);
                var m = (Euclidean3d)a;

                var restored = Shift3d.FromEuclidean3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromSimilarity3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomShift(rnd);
                var m = (Similarity3d)a;

                var restored = Shift3d.FromSimilarity3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromAffine3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomShift(rnd);
                var m = (Affine3d)a;

                var restored = Shift3d.FromAffine3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromTrafo3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomShift(rnd);
                var m = (Trafo3d)a;

                var restored = Shift3d.FromTrafo3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void Comparison()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var s0 = GetRandomShift(rnd);
                var s1 = new Shift3d(s0.V + V3d.OII);

                Assert.IsFalse(s0.Equals(s1));
                Assert.IsFalse(s0 == s1);
                Assert.IsTrue(s0 != s1);
            }
        }

        [Test]
        public static void InverseTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var s = GetRandomShift(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = s.Transform(p);

                // Inverse property
                var res = s.Inverse.Transform(q);

                // InvTransform
                var res2 = s.InvTransform(q);

                // Invert method
                Shift.Invert(ref s);
                var res3 = s.Transform(q);

                Assert.IsTrue(Fun.ApproximateEquals(p, res, 0.00001), "{0} != {1}", p, res);
                Assert.IsTrue(Fun.ApproximateEquals(p, res2, 0.00001), "{0} != {1}", p, res2);
                Assert.IsTrue(Fun.ApproximateEquals(p, res2, 0.00001), "{0} != {1}", p, res3);
            }
        }

        [Test]
        public static void Multiplication3x3Test()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var s1 = GetRandomShift(rnd);
                var s2 = GetRandomShift(rnd);
                var r = s1 * s2;
                var m = (M33d)s1 * (M33d)s2;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                    var res = r.Transform(new V3d(p, 1)).XY;
                    var res2 = m.TransformPos(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                }
            }
        }

        [Test]
        public static void Multiplication3x4Test()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var r1 = GetRandomShift(rnd);
                var r2 = GetRandomShift(rnd);
                var r = r1 * r2;
                var rm = (M34d)r1 * r2;
                var mr = r1 * (M34d)r2;
                var m = (M34d)r1 * (M44d)r2;

                {
                    var p = rnd.UniformV3d() * rnd.UniformInt(1000);
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
        public static void Multiplication4x4Test()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var r1 = GetRandomShift(rnd);
                var r2 = GetRandomShift(rnd);
                var r = r1 * r2;
                var rm = (M44d)r1 * r2;
                var mr = r1 * (M44d)r2;
                var m = (M44d)r1 * (M44d)r2;

                {
                    var p = rnd.UniformV3d() * rnd.UniformInt(1000);
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
        public static void ConsistentWithMatrixShiftTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var shift = rnd.UniformV3d() * 10;
                var m = M34d.Translation(shift);
                var r = new Shift3d(shift);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var res = m.TransformPos(p);
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
                var s = GetRandomShift(rnd);

                var str = s.ToString();
                var parsed = Shift3d.Parse(str);

                Assert.IsTrue(Fun.ApproximateEquals(parsed, s, 0.00001));
            }
        }
    }
}
