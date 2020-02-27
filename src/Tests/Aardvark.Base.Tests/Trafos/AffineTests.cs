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
        [Test]
        public static void FromM34d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomAffine, a => (M34d)a, b => Affine3d.FromM34d(b));

        [Test]
        public static void FromM44d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomAffine, a => (M44d)a, b => Affine3d.FromM44d(b));

        [Test]
        public static void FromTrafo3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomAffine, a => (Trafo3d)a, b => Affine3d.FromTrafo3d(b));

        [Test]
        public static void Comparison()
            => TrafoTesting.GenericComparisonTest(TrafoTesting.GetRandomAffine, a => new Affine3d(a.Linear, a.Trans + V3d.OII));

        [Test]
        public static void InverseTest()
            => TrafoTesting.GenericTest(rnd =>
            {
                var a = TrafoTesting.GetRandomAffine(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = a.TransformPos(p);

                // Inverse property
                var res = a.Inverse.TransformPos(q);

                // Invert method
                Affine.Invert(ref a);
                var res2 = a.TransformPos(q);

                TrafoTesting.AreEqual(p, res);
                TrafoTesting.AreEqual(p, res2);
            });

        [Test]
        public static void Multiplication3x3Test()
            => TrafoTesting.GenericTest(rnd =>
            {
                var a1 = TrafoTesting.GetRandomAffine(rnd, false);
                var a2 = TrafoTesting.GetRandomAffine(rnd, false);
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

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }

                {
                    var p = new V4d(rnd.UniformV3d() * rnd.UniformInt(1000), 1);
                    var res = (a * p).XYZ;
                    var res2 = m * p.XYZ;
                    var res3 = am * p;
                    var res4 = ma * p;

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }
            });

        [Test]
        public static void Multiplication4x4Test()
            => TrafoTesting.GenericTest(rnd =>
            {
                var a1 = TrafoTesting.GetRandomAffine(rnd);
                var a2 = TrafoTesting.GetRandomAffine(rnd);
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

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }

                {
                    var p = rnd.UniformV4d() * rnd.UniformInt(1000);
                    var res = a * p;
                    var res2 = m * p;
                    var res3 = am * p;
                    var res4 = ma * p;

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }
            });

        [Test]
        public static void MultiplicationAffineTest()
            => TrafoTesting.GenericMultiplicationTest<Affine3d, Affine3d, Affine3d>(TrafoTesting.GetRandomAffine, TrafoTesting.GetRandomAffine, Affine.TransformPos);

        [Test]
        public static void MultiplicationEuclideanTest()
            => TrafoTesting.GenericMultiplicationTest<Affine3d, Euclidean3d, Affine3d>(TrafoTesting.GetRandomAffine, TrafoTesting.GetRandomEuclidean, Affine.TransformPos);

        [Test]
        public static void MultiplicationRotTest()
            => TrafoTesting.GenericMultiplicationTest<Affine3d, Rot3d, Affine3d>(TrafoTesting.GetRandomAffine, TrafoTesting.GetRandomRot3, Affine.TransformPos);

        [Test]
        public static void MultiplicationScaleTest()
            => TrafoTesting.GenericMultiplicationTest<Affine3d, Scale3d, Affine3d>(TrafoTesting.GetRandomAffine, TrafoTesting.GetRandomScale3, Affine.TransformPos);

        [Test]
        public static void MultiplicationShiftTest()
            => TrafoTesting.GenericMultiplicationTest<Affine3d, Shift3d, Affine3d>(TrafoTesting.GetRandomAffine, TrafoTesting.GetRandomShift3, Affine.TransformPos);

        [Test]
        public static void MultiplicationSimilarityTest()
            => TrafoTesting.GenericMultiplicationTest<Affine3d, Similarity3d, Affine3d>(TrafoTesting.GetRandomAffine, TrafoTesting.GetRandomSimilarity, Affine.TransformPos);

        [Test]
        public static void ToStringAndParse()
            => TrafoTesting.GenericToStringAndParseTest(TrafoTesting.GetRandomAffine, Affine3d.Parse);
    }
}
