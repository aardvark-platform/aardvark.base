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
        [Test]
        public static void FromM33dAndV3d()
            => TrafoTesting.GenericTest(rnd =>
            {
                var a = TrafoTesting.GetRandomSimilarity(rnd);
                var tmp = (M34d)a;
                var m = (M33d)tmp;
                var t = tmp.C3;

                var restored = Similarity3d.FromM33dAndV3d(m, t);
                TrafoTesting.AreEqual(a, restored);
            });

        [Test]
        public static void FromM34d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomSimilarity, a => (M34d)a, b => Similarity3d.FromM34d(b));

        [Test]
        public static void FromM44d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomSimilarity, a => (M44d)a, b => Similarity3d.FromM44d(b));

        [Test]
        public static void FromAffine3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomSimilarity, a => (Affine3d)a, b => Similarity3d.FromAffine3d(b));

        [Test]
        public static void FromTrafo3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomSimilarity, a => (Trafo3d)a, b => Similarity3d.FromTrafo3d(b));

        [Test]
        public static void Comparison()
            => TrafoTesting.GenericComparisonTest(TrafoTesting.GetRandomSimilarity, a => new Similarity3d(a.Scale + 1, a.Euclidean));

        [Test]
        public static void InverseTest()
            => TrafoTesting.GenericTest(rnd =>
            {
                var s = TrafoTesting.GetRandomSimilarity(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = s.TransformPos(p);

                // Inverse property
                var res = s.Inverse.TransformPos(q);

                // Invert method
                Similarity.Invert(ref s);
                var res2 = s.TransformPos(q);

                TrafoTesting.AreEqual(p, res);
                TrafoTesting.AreEqual(p, res2);
            });

        [Test]
        public static void Multiplication3x3Test()
            => TrafoTesting.GenericTest(rnd =>
            {
                var s1 = TrafoTesting.GetRandomSimilarity(rnd, false);
                var s2 = TrafoTesting.GetRandomSimilarity(rnd, false);
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

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }

                {
                    var p = new V4d(rnd.UniformV3d() * rnd.UniformInt(1000), 1);
                    var res = (s * p).XYZ;
                    var res2 = m * p.XYZ;
                    var res3 = sm * p;
                    var res4 = ms * p;

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }
            });

        [Test]
        public static void Multiplication4x4Test()
            => TrafoTesting.GenericTest(rnd =>
            {
                var s1 = TrafoTesting.GetRandomSimilarity(rnd);
                var s2 = TrafoTesting.GetRandomSimilarity(rnd);
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

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }

                {
                    var p = rnd.UniformV4d() * rnd.UniformInt(1000);
                    var res = s * p;
                    var res2 = m * p;
                    var res3 = sm * p;
                    var res4 = ms * p;

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }
            });

        [Test]
        public static void ConsistentWithMatrixRotationShiftAndScaleTest()
            => TrafoTesting.GenericTest(rnd =>
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

                TrafoTesting.AreEqual(res, res2);
            });

        [Test]
        public static void MultiplicationSimilarityTest()
            => TrafoTesting.GenericMultiplicationTest <Similarity3d, Similarity3d, Similarity3d>(TrafoTesting.GetRandomSimilarity, TrafoTesting.GetRandomSimilarity, Similarity.TransformPos);

        [Test]
        public static void MultiplicationEuclideanTest()
            => TrafoTesting.GenericMultiplicationTest <Similarity3d, Euclidean3d, Similarity3d>(TrafoTesting.GetRandomSimilarity, TrafoTesting.GetRandomEuclidean, Similarity.TransformPos);

        [Test]
        public static void MultiplicationRotTest()
            => TrafoTesting.GenericMultiplicationTest<Similarity3d, Rot3d, Similarity3d>(TrafoTesting.GetRandomSimilarity, TrafoTesting.GetRandomRot3, Similarity.TransformPos);

        [Test]
        public static void MultiplicationShiftTest()
            => TrafoTesting.GenericMultiplicationTest<Similarity3d, Shift3d, Similarity3d>(TrafoTesting.GetRandomSimilarity, TrafoTesting.GetRandomShift3, Similarity.TransformPos);

        [Test]
        public static void MultiplicationScaleTest()
            => TrafoTesting.GenericMultiplicationTest<Similarity3d, Scale3d, Affine3d>(TrafoTesting.GetRandomSimilarity, TrafoTesting.GetRandomScale3, Affine.TransformPos);

        [Test]
        public static void ToStringAndParse()
            => TrafoTesting.GenericToStringAndParseTest(TrafoTesting.GetRandomSimilarity, Similarity3d.Parse);
    }
}
