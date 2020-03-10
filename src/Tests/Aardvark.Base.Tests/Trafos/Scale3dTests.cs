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
        [Test]
        public static void FromM33d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomScale3, a => (M33d)a, b => Scale3d.FromM33d(b));

        [Test]
        public static void FromM44d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomScale3, a => (M44d)a, b => Scale3d.FromM44d(b));

        [Test]
        public static void FromSimilarity3d()
            => TrafoTesting.GenericConversionTest(rnd => new Scale3d(rnd.UniformDouble() * 10), a => Similarity3d.FromScale3d(a), b => Scale3d.FromSimilarity3d(b));

        [Test]
        public static void FromAffine3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomScale3, a => (Affine3d)a, b => Scale3d.FromAffine3d(b));

        [Test]
        public static void FromTrafo3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomScale3, a => (Trafo3d)a, b => Scale3d.FromTrafo3d(b));

        [Test]
        public static void Comparison()
            => TrafoTesting.GenericComparisonTest(TrafoTesting.GetRandomScale3, a => new Scale3d(a.V + V3d.OII));

        [Test]
        public static void InverseTest()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var s = TrafoTesting.GetRandomScale3(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = s.Transform(p);

                // Inverse property
                var res = s.Inverse.Transform(q);

                // InvTransform
                var res2 = s.InvTransform(q);

                // Invert method
                Scale.Invert(ref s);
                var res3 = s.Transform(q);

                TrafoTesting.AreEqual(p, res);
                TrafoTesting.AreEqual(p, res2);
                TrafoTesting.AreEqual(p, res3);
            });
        }

        [Test]
        public static void Multiplication3x3Test()
            => TrafoTesting.Generic3x3MultiplicationTest(
                TrafoTesting.GetRandomScale3,
                Scale.Transform,
                (r, v) => r * v);

        [Test]
        public static void Multiplication3x4Test()
            => TrafoTesting.Generic3x4MultiplicationTest(
                TrafoTesting.GetRandomScale3,
                Scale.Transform,
                (r, v) => r * v);

        [Test]
        public static void MultiplicationFull3x4Test()
            => TrafoTesting.GenericFull3x4MultiplicationTest(TrafoTesting.GetRandomScale3);

        [Test]
        public static void Multiplication4x4Test()
            => TrafoTesting.Generic4x4MultiplicationTest(
                TrafoTesting.GetRandomScale3,
                Scale.Transform,
                (r, v) => r * v);

        [Test]
        public static void MultiplicationFull4x4Test()
            => TrafoTesting.GenericFull3x4MultiplicationTest(TrafoTesting.GetRandomScale3);

        [Test]
        public static void MultiplicationScaleTest()
            => TrafoTesting.GenericMultiplicationTest<Scale3d, Scale3d, Scale3d>(TrafoTesting.GetRandomScale3, TrafoTesting.GetRandomScale3, Scale.Transform);  

        [Test]
        public static void MultiplicationRotTest()
            => TrafoTesting.GenericMultiplicationTest<Scale3d, Rot3d, Affine3d>(TrafoTesting.GetRandomScale3, TrafoTesting.GetRandomRot3, Affine.TransformPos);  
        
        [Test]
        public static void MultiplicationShiftTest()
            => TrafoTesting.GenericMultiplicationTest<Scale3d, Shift3d, Affine3d>(TrafoTesting.GetRandomScale3, TrafoTesting.GetRandomShift3, Affine.TransformPos);

        [Test]
        public static void ConsistentWithMatrixScaleTest()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var scale = rnd.UniformV3d() * 10;
                var m = M33d.Diagonal(scale);
                var r = new Scale3d(scale);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var res = m.Transform(p);
                var res2 = r.Transform(p);

                TrafoTesting.AreEqual(res, res2);
            });
        }

        [Test]
        public static void ToStringAndParse()
            => TrafoTesting.GenericToStringAndParseTest(TrafoTesting.GetRandomScale3, Scale3d.Parse);
    }
}
