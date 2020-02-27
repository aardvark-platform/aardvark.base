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
        public static void Multiplication2x2Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var s1 = TrafoTesting.GetRandomScale3(rnd);
                var s2 = TrafoTesting.GetRandomScale3(rnd);
                var r = s1 * s2;
                var sm = (M22d)s1 * s2;
                var ms = s1 * (M22d)s2;
                var m = (M22d)s1 * (M22d)s2;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                    var res = r.Transform(new V3d(p, 1)).XY;
                    var res2 = m.Transform(p);
                    var res3 = sm.Transform(p);
                    var res4 = ms.Transform(p);

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }
            });
        }

        [Test]
        public static void Multiplication3x3Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var r1 = TrafoTesting.GetRandomScale3(rnd);
                var r2 = TrafoTesting.GetRandomScale3(rnd);
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

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }
            });
        }

        [Test]
        public static void Multiplication4x4Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var r1 = TrafoTesting.GetRandomScale3(rnd);
                var r2 = TrafoTesting.GetRandomScale3(rnd);
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

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }
            });
        }

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
                var m = M33d.Scale(scale);
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
