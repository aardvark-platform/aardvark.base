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
        [Test]
        public static void FromM34d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomShift3, a => (M34d)a, b => Shift3d.FromM34d(b));

        [Test]
        public static void FromM44d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomShift3, a => (M44d)a, b => Shift3d.FromM44d(b));

        [Test]
        public static void FromEuclidean3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomShift3, a => (Euclidean3d)a, b => Shift3d.FromEuclidean3d(b));

        [Test]
        public static void FromSimilarity3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomShift3, a => (Similarity3d)a, b => Shift3d.FromSimilarity3d(b));

        [Test]
        public static void FromAffine3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomShift3, a => (Affine3d)a, b => Shift3d.FromAffine3d(b));

        [Test]
        public static void FromTrafo3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomShift3, a => (Trafo3d)a, b => Shift3d.FromTrafo3d(b));

        [Test]
        public static void Comparison()
            => TrafoTesting.GenericComparisonTest(TrafoTesting.GetRandomShift3, a => new Shift3d(a.V + V3d.OII));

        [Test]
        public static void InverseTest()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var s = TrafoTesting.GetRandomShift3(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = s.Transform(p);

                // Inverse property
                var res = s.Inverse.Transform(q);

                // InvTransform
                var res2 = s.InvTransform(q);

                // Invert method
                Shift.Invert(ref s);
                var res3 = s.Transform(q);

                TrafoTesting.AreEqual(p, res);
                TrafoTesting.AreEqual(p, res2);
                TrafoTesting.AreEqual(p, res3);
            });
        }

        [Test]
        public static void Multiplication3x3Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var s1 = TrafoTesting.GetRandomShift3(rnd);
                var s2 = TrafoTesting.GetRandomShift3(rnd);
                var r = s1 * s2;
                var m = (M33d)s1 * (M33d)s2;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                    var res = r.Transform(new V3d(p, 1)).XY;
                    var res2 = m.TransformPos(p);

                    Assert.IsTrue(Fun.ApproximateEquals(res, res2, 0.00001), "{0} != {1}", res, res2);
                }
            });
        }

        [Test]
        public static void Multiplication3x4Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var r1 = TrafoTesting.GetRandomShift3(rnd);
                var r2 = TrafoTesting.GetRandomShift3(rnd);
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
            });
        }

        [Test]
        public static void Multiplication4x4Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var r1 = TrafoTesting.GetRandomShift3(rnd);
                var r2 = TrafoTesting.GetRandomShift3(rnd);
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
            });
        }

        [Test]
        public static void MultiplicationShiftTest()
            => TrafoTesting.GenericMultiplicationTest<Shift3d, Shift3d, Shift3d>(TrafoTesting.GetRandomShift3, TrafoTesting.GetRandomShift3, Shift.Transform);   

        [Test]
        public static void MultiplicationRotTest()
            => TrafoTesting.GenericMultiplicationTest<Shift3d, Rot3d, Euclidean3d>(TrafoTesting.GetRandomShift3, TrafoTesting.GetRandomRot3, Euclidean.TransformPos);        
        [Test]
        public static void MultiplicationScaleTest()
            => TrafoTesting.GenericMultiplicationTest<Shift3d, Scale3d, Affine3d>(TrafoTesting.GetRandomShift3, TrafoTesting.GetRandomScale3, Affine.TransformPos);

        [Test]
        public static void ConsistentWithMatrixShiftTest()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var shift = rnd.UniformV3d() * 10;
                var m = M34d.Translation(shift);
                var r = new Shift3d(shift);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var res = m.TransformPos(p);
                var res2 = r.Transform(p);

                TrafoTesting.AreEqual(res, res2);
            });
        }

        [Test]
        public static void ToStringAndParse()
            => TrafoTesting.GenericToStringAndParseTest(TrafoTesting.GetRandomShift3, Shift3d.Parse);
    }
}
