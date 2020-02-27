using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class EuclideanTests
    {
        [Test]
        public static void FromM33dAndV3d()
            => TrafoTesting.GenericTest(rnd =>
            {
                var a = TrafoTesting.GetRandomEuclidean(rnd);
                var m = (M33d)a.Rot;
                var t = a.Trans;

                var restored = Euclidean3d.FromM33dAndV3d(m, t);
                TrafoTesting.AreEqual(a, restored);
            });

        [Test]
        public static void FromM34d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomEuclidean, a => (M34d)a, b => Euclidean3d.FromM34d(b));

        [Test]
        public static void FromM44d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomEuclidean, a => (M44d)a, b => Euclidean3d.FromM44d(b));

        [Test]
        public static void FromSimilarity3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomEuclidean, a => (Similarity3d)a, b => Euclidean3d.FromSimilarity3d(b));

        [Test]
        public static void FromAffine3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomEuclidean, a => (Affine3d)a, b => Euclidean3d.FromAffine3d(b));

        [Test]
        public static void FromTrafo3d()
            => TrafoTesting.GenericConversionTest(TrafoTesting.GetRandomEuclidean, a => (Trafo3d)a, b => Euclidean3d.FromTrafo3d(b));

        [Test]
        public static void Comparison()
            => TrafoTesting.GenericComparisonTest(TrafoTesting.GetRandomEuclidean, a => new Euclidean3d(a.Rot, a.Trans + V3d.OII));

        [Test]
        public static void InverseTest()
            => TrafoTesting.GenericTest(rnd =>
            {
                var e = TrafoTesting.GetRandomEuclidean(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var q = e.TransformPos(p);

                // Inverse property
                var res = e.Inverse.TransformPos(q);

                // Invert method
                Euclidean.Invert(ref e);
                var res2 = e.TransformPos(q);

                TrafoTesting.AreEqual(p, res);
                TrafoTesting.AreEqual(p, res2);
            });

        [Test]
        public static void Multiplication3x3Test()
            => TrafoTesting.GenericTest(rnd =>
            {
                var e1 = TrafoTesting.GetRandomEuclidean(rnd, false);
                var e2 = TrafoTesting.GetRandomEuclidean(rnd, false);
                var e = e1 * e2;
                var em = (M33d)e1 * e2;
                var me = e1 * (M33d)e2;
                var m = (M33d)e1 * (M33d)e2;

                {
                    var p = new V4d(rnd.UniformV3d() * rnd.UniformInt(1000), 1);
                    var res = e.Transform(p).XYZ;
                    var res2 = m.Transform(p).XYZ;
                    var res3 = em.Transform(p);
                    var res4 = me.Transform(p);

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }

                {
                    var p = new V4d(rnd.UniformV3d() * rnd.UniformInt(1000), 1);
                    var res = (e * p).XYZ;
                    var res2 = m * p.XYZ;
                    var res3 = em * p;
                    var res4 = me * p;

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }
            });

        [Test]
        public static void Multiplication4x4Test()
            => TrafoTesting.GenericTest(rnd =>
            {
                var e1 = TrafoTesting.GetRandomEuclidean(rnd);
                var e2 = TrafoTesting.GetRandomEuclidean(rnd);
                var a = e1 * e2;
                var am = (M44d)e1 * e2;
                var ma = e1 * (M44d)e2;
                var m = (M44d)e1 * (M44d)e2;

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
        public static void ConsistentWithMatrixRotationAndShiftTest()
            => TrafoTesting.GenericTest(rnd =>
            {
                var trans = rnd.UniformV3d() * 10;

                var axis = rnd.UniformV3dDirection();
                var angle = rnd.UniformDouble() * Constant.PiTimesTwo;

                var m = M44d.Translation(trans) * M44d.Rotation(axis, angle);
                var e = new Euclidean3d(Rot3d.Rotation(axis, angle), trans);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);
                var res = m.TransformPos(p);
                var res2 = e.TransformPos(p);

                TrafoTesting.AreEqual(res, res2);
            });

        [Test]
        public static void MultiplicationEuclideanTest()
            => TrafoTesting.GenericMultiplicationTest<Euclidean3d, Euclidean3d, Euclidean3d>(TrafoTesting.GetRandomEuclidean, TrafoTesting.GetRandomEuclidean, Euclidean.TransformPos);

        [Test]
        public static void MultiplicationRotTest()
            => TrafoTesting.GenericMultiplicationTest<Euclidean3d, Rot3d, Euclidean3d>(TrafoTesting.GetRandomEuclidean, TrafoTesting.GetRandomRot3, Euclidean.TransformPos);

        [Test]
        public static void MultiplicationShiftTest()
            => TrafoTesting.GenericMultiplicationTest<Euclidean3d, Shift3d, Euclidean3d>(TrafoTesting.GetRandomEuclidean, TrafoTesting.GetRandomShift3, Euclidean.TransformPos);

        [Test]
        public static void MultiplicationScaleTest()
            => TrafoTesting.GenericMultiplicationTest<Euclidean3d, Scale3d, Affine3d>(TrafoTesting.GetRandomEuclidean, TrafoTesting.GetRandomScale3, Affine.TransformPos);

        [Test]
        public static void ToStringAndParse()
            => TrafoTesting.GenericToStringAndParseTest(TrafoTesting.GetRandomEuclidean, Euclidean3d.Parse);
    }
}
