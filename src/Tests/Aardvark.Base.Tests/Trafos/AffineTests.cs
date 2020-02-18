using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class AffineTests
    {
        private static readonly int iterations = 10000;

        private static Scale3d GetRandomScale(RandomSystem rnd)
        {
            return new Scale3d(rnd.UniformV3d() * 5);
        }

        private static Rot3d GetRandomRot(RandomSystem rnd)
        {
            return new Rot3d(rnd.UniformV3dDirection(), rnd.UniformDouble() * Constant.PiTimesTwo);
        }

        private static Shift3d GetRandomShift(RandomSystem rnd)
        {
            return new Shift3d(rnd.UniformV3d() * 10);
        }

        private static Euclidean3d GetRandomEuclidean(RandomSystem rnd, bool withTranslation = true)
        {
            var translation = withTranslation ? GetRandomShift(rnd).V : V3d.Zero;
            return new Euclidean3d(GetRandomRot(rnd), translation);
        }

        private static Similarity3d GetRandomSimilarity(RandomSystem rnd, bool withTranslation = true)
        {
            return new Similarity3d(GetRandomScale(rnd).X, GetRandomEuclidean(rnd, withTranslation));
        }

        private static Affine3d GetRandomAffine(RandomSystem rnd, bool withTranslation = true) 
        {
            return new Affine3d(GetRandomSimilarity(rnd, withTranslation));
        }


        [Test]
        public static void Comparison()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a0 = GetRandomAffine(rnd);
                var a1 = new Affine3d(a0.Linear, a0.Trans + V3d.OII);

                Assert.IsFalse(a0.Equals(a1));
                Assert.IsFalse(a0 == a1);
                Assert.IsTrue(a0 != a1);
            }
        }

        [Test]
        public static void InverseTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomAffine(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = a.TransformPos(p);

                // Inverse property
                var res = a.Inverse.TransformPos(q);

                // Invert method
                Affine.Invert(ref a);
                var res2 = a.TransformPos(q);

                Assert.IsTrue(Fun.ApproximateEquals(p, res, 0.00001), "{0} != {1}", p, res);
                Assert.IsTrue(Fun.ApproximateEquals(p, res2, 0.00001), "{0} != {1}", p, res2);
            }
        }

        [Test]
        public static void Multiplication3x3Test()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a1 = GetRandomAffine(rnd, false);
                var a2 = GetRandomAffine(rnd, false);
                var a = a1 * a2;
                var am = (M33d)a1 * a2;
                var ma = a1 * (M33d)a2;
                var m = (M33d)a1 * (M33d)a2;

                {
                    var p = new V4d(rnd.UniformV3d() * rnd.UniformInt(1000), 1);
                    var res = a.Transform(p).XYZ;
                    var res2 = m.Transform(p).XYZ;
                    var res3 = am.Transform(p);
                    var res4 = ma.Transform(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res3, 0.00001), "{0} != {1}", res, res3);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res4, 0.00001), "{0} != {1}", res, res4);
                }

                {
                    var p = new V4d(rnd.UniformV3d() * rnd.UniformInt(1000), 1);
                    var res = (a * p).XYZ;
                    var res2 = m * p.XYZ;
                    var res3 = am * p;
                    var res4 = ma * p;

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
                var a1 = GetRandomAffine(rnd);
                var a2 = GetRandomAffine(rnd);
                var a = a1 * a2;
                var am = (M44d)a1 * a2;
                var ma = a1 * (M44d)a2;
                var m = (M44d)a1 * (M44d)a2;

                {
                    var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                    var res = a.TransformPos(p);
                    var res2 = m.TransformPos(p);
                    var res3 = am.TransformPos(p);
                    var res4 = ma.TransformPos(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res3, 0.00001), "{0} != {1}", res, res3);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res4, 0.00001), "{0} != {1}", res, res4);
                }

                {
                    var p = rnd.UniformV4d() * rnd.UniformInt(1000);
                    var res = a * p;
                    var res2 = m * p;
                    var res3 = am * p;
                    var res4 = ma * p;

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res3, 0.00001), "{0} != {1}", res, res3);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res4, 0.00001), "{0} != {1}", res, res4);
                }
            }
        }

        private static void GenericMultiplicationTest<T>(Func<RandomSystem, T> frnd)
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomAffine(rnd);
                dynamic t = frnd(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);

                {
                    var trafo = a * t;
                    var res = Affine.TransformPos(trafo, p);

                    var trafoRef = (M44d)a * (M44d)t;
                    var resRef = trafoRef.TransformPos(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, resRef, 0.00001), "{0} != {1}", res, resRef);
                }

                {
                    var trafo = t * a;
                    var res = Affine.TransformPos(trafo, p);

                    var trafoRef = (M44d)t * (M44d)a;
                    var resRef = trafoRef.TransformPos(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, resRef, 0.00001), "{0} != {1}", res, resRef);
                }
            }
        }

        [Test]
        public static void MultiplicationEuclideanTest()
            => GenericMultiplicationTest(rnd => GetRandomEuclidean(rnd));

        [Test]
        public static void MultiplicationRotTest()
            => GenericMultiplicationTest(GetRandomRot);
        
        [Test]
        public static void MultiplicationScaleTest()
            => GenericMultiplicationTest(GetRandomScale);

        [Test]
        public static void MultiplicationShiftTest()
            => GenericMultiplicationTest(GetRandomShift);

        [Test]
        public static void MultiplicationSimilarityTest()
            => GenericMultiplicationTest(rnd => GetRandomSimilarity(rnd));

        [Test]
        public static void ToStringAndParse()
        {
            var rnd = new RandomSystem(1);
            
            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomAffine(rnd);

                var str = a.ToString();
                var parsed = Affine3d.Parse(str);

                Assert.IsTrue(Fun.ApproximateEquals(parsed, a, 0.00001));
            }
        }
    }
}
