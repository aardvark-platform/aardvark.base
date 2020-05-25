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
            => TrafoTesting.GenericMatrixMultiplicationTest<Affine3d, M33d, M34d>(
                rnd => TrafoTesting.GetRandomAffine(rnd, false),
                Affine.TransformPos,
                Mat.Transform,
                Mat.TransformPos,
                (a, v) => (a * new V4d(v, 1)).XYZ,
                (m, v) => m * v,
                (m, v) => m * new V4d(v, 1));

        [Test]
        public static void Multiplication3x4Test()
            => TrafoTesting.Generic3x4MultiplicationTest(
                TrafoTesting.GetRandomAffine,
                Affine.TransformPos,
                (a, v) => (a * new V4d(v, 1)).XYZ);

        [Test]
        public static void Multiplication4x4Test()
            => TrafoTesting.Generic4x4MultiplicationTest(
                TrafoTesting.GetRandomAffine,
                Affine.TransformPos,
                (a, v) => (a * new V4d(v, 1)).XYZ);

        [Test]
        public static void MultiplicationAffine3x4Test()
            => TrafoTesting.GenericTest(rnd =>
            {
                var a = TrafoTesting.GetRandomAffine(rnd);
                var b = TrafoTesting.GetRandomAffine(rnd);
                var ma = (M34d) a;
                var mb = (M34d) b;

                var a_x_b = a * b;
                var ma_x_mb = Mat.MultiplyAffine(ma, mb);

                TrafoTesting.AreEqual((M34d)a_x_b, ma_x_mb);
            });

        [Test]
        public static void MultiplicationFull4x4Test()
            => TrafoTesting.GenericFull4x4MultiplicationTest(TrafoTesting.GetRandomAffine);

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
