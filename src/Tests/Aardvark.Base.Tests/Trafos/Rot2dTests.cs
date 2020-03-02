using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class Rot2dTests
    {
        [Test]
        public static void DistanceTest()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var delta = (rnd.UniformDouble() - 0.5) * 2.0 * Constant.Pi;
                var r1 = TrafoTesting.GetRandomRot2(rnd);
                var r2 = new Rot2d(r1.Angle + (delta + Constant.PiTimesTwo * rnd.UniformInt(10)));

                var dist = r1.Distance(r2);
                TrafoTesting.AreEqual(dist, Fun.Abs(delta));
            });
        }

        [Test]
        public static void InverseTest()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var r = TrafoTesting.GetRandomRot2(rnd);
                
                var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                var q = r.Transform(p);

                // Inverse property
                var res = r.Inverse.Transform(q);

                // Invert method
                Rot.Invert(ref r);
                var res2 = r.Transform(q);

                TrafoTesting.AreEqual(p, res);
                TrafoTesting.AreEqual(p, res2);
            });
        }

        [Test]
        public static void Multiplication2x2Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var r1 = TrafoTesting.GetRandomRot2(rnd);
                var r2 = TrafoTesting.GetRandomRot2(rnd);
                var r = r1 * r2;
                var rm = (M22d)r1 * r2;
                var mr = r1 * (M22d)r2;
                var m = (M22d)r1 * (M22d)r2;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
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
        public static void Multiplication2x3Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var r1 = TrafoTesting.GetRandomRot2(rnd);
                var r2 = TrafoTesting.GetRandomRot2(rnd);
                var r = r1 * r2;
                var rm = (M23d)r1 * r2;
                var mr = r1 * (M23d)r2;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                    var res = r.Transform(p);
                    var res2 = rm.TransformPos(p);
                    var res3 = mr.TransformPos(p);

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                }
            });
        }

        [Test]
        public static void MultiplicationFull2x3Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var m = TrafoTesting.GetRandom2x3(rnd);
                var r = TrafoTesting.GetRandomRot2(rnd);

                var mr = m * r;
                var rm = r * m;

                var mr_ref = m * (M33d)r;
                var rm_ref = (M23d)r * (M33d)m;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                    var res_mr = mr.TransformPos(p);
                    var res_mr_ref = mr_ref.TransformPos(p);
                    var res_rm = rm.TransformPos(p);
                    var res_rm_ref = rm_ref.TransformPos(p);

                    TrafoTesting.AreEqual(res_mr, res_mr_ref);
                    TrafoTesting.AreEqual(res_rm, res_rm_ref);
                }
            });
        }

        [Test]
        public static void Multiplication3x3Test()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var r1 = TrafoTesting.GetRandomRot2(rnd);
                var r2 = TrafoTesting.GetRandomRot2(rnd);
                var r = r1 * r2;
                var rm = (M33d)r1 * r2;
                var mr = r1 * (M33d)r2;
                var m = (M33d)r1 * (M33d)r2;

                {
                    var p = rnd.UniformV2d() * rnd.UniformInt(1000);
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
        public static void FromM22d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot2, a => (M22d)a, Rot2d.FromM22d);

        [Test]
        public static void FromM33d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot2, a => (M33d)a, b => Rot2d.FromM33d(b));

        [Test]
        public static void FromEuclidean2d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot2, a => (Euclidean2d)a, b => Rot2d.FromEuclidean2d(b));

        [Test]
        public static void FromSimilarity2d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot2, a => (Similarity2d)a, b => Rot2d.FromSimilarity2d(b));

        [Test]
        public static void FromAffine2d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot2, a => (Affine2d)a, b => Rot2d.FromAffine2d(b));

        [Test]
        public static void FromTrafo2d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomRot2, a => (Trafo2d)a, b => Rot2d.FromTrafo2d(b));

        [Test]
        public static void MultiplicationShiftTest()
            => TrafoTesting.GenericTest(rnd =>
            {
                var a = TrafoTesting.GetRandomRot2(rnd);
                var b = TrafoTesting.GetRandomShift2(rnd);

                var p = rnd.UniformV2d() * rnd.UniformInt(1000);

                {
                    var trafo = a * b;
                    var res = trafo.TransformPos(p);

                    var trafoRef = (M33d)a * (M33d)b;
                    var resRef = trafoRef.TransformPos(p);

                    TrafoTesting.AreEqual(res, resRef);
                }

                {
                    var trafo = b * a;
                    var res = trafo.TransformPos(p);

                    var trafoRef = (M33d)b * (M33d)a;
                    var resRef = trafoRef.TransformPos(p);

                    TrafoTesting.AreEqual(res, resRef);
                }
            });
        
        [Test]
        public static void MultiplicationScaleTest()
            => TrafoTesting.GenericTest(rnd =>
            {
                var a = TrafoTesting.GetRandomRot2(rnd);
                var b = TrafoTesting.GetRandomScale2(rnd);

                var p = rnd.UniformV2d() * rnd.UniformInt(1000);

                {
                    var trafo = a * b;
                    var res = trafo.TransformPos(p);

                    var trafoRef = (M33d)a * (M33d)b;
                    var resRef = trafoRef.TransformPos(p);

                    TrafoTesting.AreEqual(res, resRef);
                }

                {
                    var trafo = b * a;
                    var res = trafo.TransformPos(p);

                    var trafoRef = (M33d)b * (M33d)a;
                    var resRef = trafoRef.TransformPos(p);

                    TrafoTesting.AreEqual(res, resRef);
                }
            });

        [Test]
        public static void ConsistentWithMatrixRotationTest()
        {
            TrafoTesting.GenericTest(rnd =>
            {
                var angle = rnd.UniformDouble() * Constant.PiTimesTwo;
                var m = M22d.Rotation(angle);
                var r = new Rot2d(angle);

                var p = rnd.UniformV2d() * rnd.UniformInt(1000);
                var res = m.Transform(p);
                var res2 = r.Transform(p);

                TrafoTesting.AreEqual(res, res2);
            });
        }

        [Test]
        public static void ToStringAndParse()
            => TrafoTesting.GenericToStringAndParseTest(TrafoTesting.GetRandomRot2, Rot2d.Parse);
    }
}
