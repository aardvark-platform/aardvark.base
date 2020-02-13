using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class Scale3dTests
    {
        private static readonly int iterations = 10000;

        private static Scale3d GetRandomScale(RandomSystem rnd)
        {
            return new Scale3d(rnd.UniformV3d() * 10);
        }

        [Test]
        public static void InverseTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var s = GetRandomScale(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = s.Transform(p);

                // Inverse property
                var res = s.Inverse.Transform(q);

                // InvTransform
                var res2 = s.InvTransform(q);

                // Invert method
                Scale.Invert(ref s);
                var res3 = s.Transform(q);

                Assert.IsTrue(Fun.ApproximateEquals(p, res, 0.00001), "{0} != {1}", p, res);
                Assert.IsTrue(Fun.ApproximateEquals(p, res2, 0.00001), "{0} != {1}", p, res2);
                Assert.IsTrue(Fun.ApproximateEquals(p, res2, 0.00001), "{0} != {1}", p, res3);
            }
        }

        [Test]
        public static void Multiplication2x2Test()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var s1 = GetRandomScale(rnd);
                var s2 = GetRandomScale(rnd);
                var r = s1 * s2;
                var sm = (M22d)s1 * s2;
                var ms = s1 * (M22d)s2;
                var m = (M22d)s1 * (M22d)s2;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                    var res = r.Transform(p);
                    var res2 = m.Transform(p);
                    var res3 = sm.Transform(p);
                    var res4 = ms.Transform(p);

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
                var r1 = GetRandomScale(rnd);
                var r2 = GetRandomScale(rnd);
                var r = r1 * r2;
                var rm = (M33d)r1 * r2;
                var mr = r1 * (M33d)r2;
                var m = (M33d)r1 * (M33d)r2;

                {
                    var p = rnd.UniformV3d() * rnd.UniformInt(1000);
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
        public static void Multiplication4x4Test()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var r1 = GetRandomScale(rnd);
                var r2 = GetRandomScale(rnd);
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
        public static void ConsistentWithMatrixScaleTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var scale = rnd.UniformV3d() * 10;
                var m = M33d.Scale(scale);
                var r = new Scale3d(scale);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var res = m.Transform(p);
                var res2 = r.Transform(p);

                Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
            }
        }
    }
}
