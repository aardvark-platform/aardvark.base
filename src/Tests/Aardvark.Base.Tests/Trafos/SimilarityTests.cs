using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class SimilarityTests
    {
        private static readonly int iterations = 10000;

        private static Scale3d GetRandomScale(RandomSystem rnd)
        {
            return new Scale3d(rnd.UniformV3d() * 5);
        }

        private static Rot3d GetRandomRot(RandomSystem rnd)
        {
            return Rot3d.Rotation(rnd.UniformV3dDirection(), rnd.UniformDouble() * Constant.PiTimesTwo);
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

        [Test]
        public static void FromM33dAndV3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomSimilarity(rnd);
                var tmp = (M34d)a;
                var m = (M33d)tmp;
                var t = tmp.C3;

                var restored = Similarity3d.FromM33dAndV3d(m, t);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromM34d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomSimilarity(rnd);
                var m = (M34d)a;

                var restored = Similarity3d.FromM34d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromM44d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomSimilarity(rnd);
                var m = (M44d)a;

                var restored = Similarity3d.FromM44d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromAffine3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomSimilarity(rnd);
                var m = (Affine3d)a;

                var restored = Similarity3d.FromAffine3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void FromTrafo3d()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomSimilarity(rnd);
                var m = (Trafo3d)a;

                var restored = Similarity3d.FromTrafo3d(m);
                Assert.IsTrue(Fun.ApproximateEquals(a, restored, 0.00001), "{0}: {1} != {2}", i, a, restored);
            }
        }

        [Test]
        public static void Comparison()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var s0 = GetRandomSimilarity(rnd);
                var s1 = new Similarity3d(s0.Scale + 1, s0.Euclidean);

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
                var s = GetRandomSimilarity(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = s.TransformPos(p);

                // Inverse property
                var res = s.Inverse.TransformPos(q);

                // Invert method
                Similarity.Invert(ref s);
                var res2 = s.TransformPos(q);

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
                var s1 = GetRandomSimilarity(rnd, false);
                var s2 = GetRandomSimilarity(rnd, false);
                var s = s1 * s2;
                var sm = (M33d)s1 * s2;
                var ms = s1 * (M33d)s2;
                var m = (M33d)s1 * (M33d)s2;

                {
                    var p = new V4d(rnd.UniformV3d() * rnd.UniformInt(1000), 1);
                    var res = s.Transform(p).XYZ;
                    var res2 = m.Transform(p).XYZ;
                    var res3 = sm.Transform(p);
                    var res4 = ms.Transform(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res3, 0.00001), "{0} != {1}", res, res3);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res4, 0.00001), "{0} != {1}", res, res4);
                }

                {
                    var p = new V4d(rnd.UniformV3d() * rnd.UniformInt(1000), 1);
                    var res = (s * p).XYZ;
                    var res2 = m * p.XYZ;
                    var res3 = sm * p;
                    var res4 = ms * p;

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
                var s1 = GetRandomSimilarity(rnd);
                var s2 = GetRandomSimilarity(rnd);
                var s = s1 * s2;
                var sm = (M44d)s1 * s2;
                var ms = s1 * (M44d)s2;
                var m = (M44d)s1 * (M44d)s2;

                {
                    var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                    var res = s.TransformPos(p);
                    var res2 = m.TransformPos(p);
                    var res3 = sm.TransformPos(p);
                    var res4 = ms.TransformPos(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res3, 0.00001), "{0} != {1}", res, res3);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res4, 0.00001), "{0} != {1}", res, res4);
                }

                {
                    var p = rnd.UniformV4d() * rnd.UniformInt(1000);
                    var res = s * p;
                    var res2 = m * p;
                    var res3 = sm * p;
                    var res4 = ms * p;

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res3, 0.00001), "{0} != {1}", res, res3);
                    Assert.IsTrue(Fun.ApproximateEquals(res, res4, 0.00001), "{0} != {1}", res, res4);
                }
            }
        }

        private static void GenericMultiplicationTest<T, U>(Func<RandomSystem, T> frnd, Func<U, V3d, V3d> transform)
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomSimilarity(rnd);
                dynamic t = frnd(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);

                {
                    var trafo = a * t;
                    var res = transform(trafo, p);

                    var trafoRef = (M44d)a * (M44d)t;
                    var resRef = trafoRef.TransformPos(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, resRef, 0.00001), "(1) {0} != {1}", res, resRef);
                }

                {
                    var trafo = t * a;
                    var res = transform(trafo, p);

                    var trafoRef = (M44d)t * (M44d)a;
                    var resRef = trafoRef.TransformPos(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, resRef, 0.00001), "(2) {0} != {1}", res, resRef);
                }
            }
        }

        [Test]
        public static void ConsistentWithMatrixRotationShiftAndScaleTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var trans = rnd.UniformV3d() * 10;

                var axis = rnd.UniformV3dDirection();
                var angle = rnd.UniformDouble() * Constant.PiTimesTwo;

                var scale = rnd.UniformDouble() * 5;

                var m = M44d.Translation(trans) * M44d.Rotation(axis, angle) * M44d.Scale(scale, scale, scale);
                var e = new Similarity3d(scale, Rot3d.Rotation(axis, angle), trans);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var res = m.TransformPos(p);
                var res2 = e.TransformPos(p);

                Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
            }
        }

        [Test]
        public static void MultiplicationEuclideanTest()
            => GenericMultiplicationTest<Euclidean3d, Similarity3d>(rnd => GetRandomEuclidean(rnd), Similarity.TransformPos);

        [Test]
        public static void MultiplicationRotTest()
            => GenericMultiplicationTest<Rot3d, Similarity3d>(GetRandomRot, Similarity.TransformPos);

        [Test]
        public static void MultiplicationShiftTest()
            => GenericMultiplicationTest<Shift3d, Similarity3d>(GetRandomShift, Similarity.TransformPos);

        [Test]
        public static void MultiplicationScaleTest()
            => GenericMultiplicationTest<Scale3d, Affine3d>(GetRandomScale, Affine.TransformPos);

        [Test]
        public static void FromM44dTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var s = GetRandomSimilarity(rnd);
                var m = (M44d)s;

                var restored = Similarity3d.FromM44d(m);
                Assert.IsTrue(Fun.ApproximateEquals(s, restored, 0.00001), "{0}: {1} != {2}", i, s, restored);
            }
        }

        [Test]
        public static void ToStringAndParse()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < iterations; i++)
            {
                var a = GetRandomSimilarity(rnd);

                var str = a.ToString();
                var parsed = Similarity3d.Parse(str);

                Assert.IsTrue(Fun.ApproximateEquals(parsed, a, 0.00001));
            }
        }
    }
}
