using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    public static class TrafoTesting
    {
        public static readonly int Iterations = 10000;

        public static readonly double Epsilon = 1e-8;

        public static void AreEqual(dynamic a, dynamic b)
            => Assert.IsTrue(Fun.ApproximateEquals(a, b, Epsilon), "{0} != {1}", a, b);

        public static M23d GetRandom2x3(RandomSystem rnd)
            => new M23d(rnd.CreateUniformDoubleArray(16));

        public static M33d GetRandom3x3(RandomSystem rnd)
            => new M33d(rnd.CreateUniformDoubleArray(16));

        public static M34d GetRandom3x4(RandomSystem rnd)
            => new M34d(rnd.CreateUniformDoubleArray(16));

        public static M44d GetRandom4x4(RandomSystem rnd)
            => new M44d(rnd.CreateUniformDoubleArray(16));

        public static Scale2d GetRandomScale2(RandomSystem rnd)
            => new Scale2d(rnd.UniformV2d() * 5);

        public static Scale3d GetRandomScale3(RandomSystem rnd)
            => new Scale3d(rnd.UniformV3d() * 5);

        public static Rot2d GetRandomRot2(RandomSystem rnd)
            => new Rot2d(rnd.UniformDouble() * Constant.PiTimesTwo);

        public static Rot3d GetRandomRot3(RandomSystem rnd)
            => Rot3d.Rotation(rnd.UniformV3dDirection(), rnd.UniformDouble() * Constant.PiTimesTwo);

        public static Shift2d GetRandomShift2(RandomSystem rnd)
            => new Shift2d(rnd.UniformV2d() * 10);

        public static Shift3d GetRandomShift3(RandomSystem rnd)
            => new Shift3d(rnd.UniformV3d() * 10);

        public static Euclidean3d GetRandomEuclidean(RandomSystem rnd, bool withTranslation)
        {
            var translation = withTranslation ? GetRandomShift3(rnd).V : V3d.Zero;
            return new Euclidean3d(GetRandomRot3(rnd), translation);
        }

        public static Euclidean3d GetRandomEuclidean(RandomSystem rnd)
            => GetRandomEuclidean(rnd, true);

        public static Similarity3d GetRandomSimilarity(RandomSystem rnd, bool withTranslation)
            => new Similarity3d(GetRandomScale3(rnd).X, GetRandomEuclidean(rnd, withTranslation));

        public static Similarity3d GetRandomSimilarity(RandomSystem rnd)
            => GetRandomSimilarity(rnd, true);

        public static Affine3d GetRandomAffine(RandomSystem rnd, bool withTranslation)
            => (Affine3d)GetRandomSimilarity(rnd, withTranslation);

        public static Affine3d GetRandomAffine(RandomSystem rnd)
            => GetRandomAffine(rnd, true);

        /// <summary>
        /// Runs the given test function multiple times and provides a RandomSystem.
        /// </summary>
        public static void GenericTest(Action<RandomSystem, int> f)
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < Iterations; i++)
            {
                f(rnd, i);
            }
        }

        /// <summary>
        /// Runs the given test function multiple times and provides a RandomSystem.
        /// </summary>
        public static void GenericTest(Action<RandomSystem> f)
            => GenericTest((rnd, i) => f(rnd));

        /// <summary>
        /// Tests if the comparison operators and Equals work as intended. The mutate function
        /// takes a T trafo as input and returns a different one.
        /// </summary>
        public static void GenericComparisonTest<T>(Func<RandomSystem, T> frnd, Func<T, T> mutate)
        {
            GenericTest(rnd =>
            {
                dynamic a = frnd(rnd);
                dynamic b = mutate(a);

                Assert.IsFalse(a.Equals(b));
                Assert.IsFalse(a == b);
                Assert.IsTrue(a != b);
            });
        }

        /// <summary>
        /// Tests if the conversion from T1 to T2 and back is working, that is the same trafo is returned as initially generated.
        /// </summary>
        public static void GenericConversionTest<T1, T2>(Func<RandomSystem, T1> frnd, Func<T1, T2> cast, Func<T2, T1> restore)
        {
            GenericTest(rnd =>
            {
                dynamic a = frnd(rnd);
                dynamic b = cast(a);

                dynamic restored = restore(b);
                AreEqual(a, restored);
            });
        }

        /// <summary>
        /// Tests if the multiplication between a T1 and T2 trafo (resulting in a U trafo) is consistent with multiplying the corresponding matrix representations.
        /// </summary>
        public static void GenericMultiplicationTest<T1, T2, U>(Func<RandomSystem, T1> frnd1, Func<RandomSystem, T2> frnd2, Func<U, V3d, V3d> transform)
        {
            GenericTest(rnd =>
            {
                dynamic a = frnd1(rnd);
                dynamic b = frnd2(rnd);

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);

                {
                    var trafo = a * b;
                    var res = transform(trafo, p);

                    var trafoRef = (M44d)a * (M44d)b;
                    var resRef = trafoRef.TransformPos(p);

                    AreEqual(res, resRef);
                }

                {
                    var trafo = b * a;
                    var res = transform(trafo, p);

                    var trafoRef = (M44d)b * (M44d)a;
                    var resRef = trafoRef.TransformPos(p);

                    AreEqual(res, resRef);
                }
            });
        }

        /// <summary>
        /// Tests multiplication of T trafos with their M matrix representations.
        /// </summary>
        public static void GenericMatrixMultiplicationTest<T, M, M2>(
                Func<RandomSystem, T> frnd,
                Func<T, V3d, V3d> trafoTransform,
                Func<M, V3d, V3d> matrixTransform,
                Func<M2, V3d, V3d> matrixTransform2,
                Func<T, V3d, V3d> trafoMul,
                Func<M, V3d, V3d> matrixMul,
                Func<M2, V3d, V3d> matrixMul2,
                bool squareMatrix = true)
            => GenericTest(rnd =>
            {
                dynamic a = frnd(rnd);
                dynamic b = frnd(rnd);
                dynamic ma = (M)a;
                dynamic mb = (M)b;

                var a_x_b = a * b;
                var ma_x_b = ma * b;
                var a_x_mb = a * mb;
                var ma_x_mb = (squareMatrix) ? ma * mb : null;

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);

                {
                    var res = trafoTransform(a_x_b, p);
                    var res2 = matrixTransform2(ma_x_b, p);
                    var res3 = matrixTransform2(a_x_mb, p);
                    var res4 = squareMatrix ? matrixTransform(ma_x_mb, p) : res3;

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }

                {
                    var res = trafoMul(a_x_b, p);
                    var res2 = matrixMul2(ma_x_b, p);
                    var res3 = matrixMul2(a_x_mb, p);
                    var res4 = squareMatrix ? matrixMul(ma_x_mb, p) : res3;

                    TrafoTesting.AreEqual(res, res2);
                    TrafoTesting.AreEqual(res, res3);
                    TrafoTesting.AreEqual(res, res4);
                }
            });

        /// <summary>
        /// Tests multiplication of T trafos with random M matrices.
        /// </summary>
        public static void GenericFullMatrixMultiplicationTest<T, M, MAugmented>(
                Func<RandomSystem, T> frnd,
                Func<RandomSystem, M> frndMatrix,
                Func<M, V3d, V3d> matrixTransform,
                Func<M, V3d, V3d> matrixMul)
            => GenericTest(rnd =>
            {
                dynamic m = frndMatrix(rnd);
                dynamic t = frnd(rnd);
                dynamic maugm = (MAugmented)m;
                dynamic maugt = (MAugmented)t;
                dynamic mt = (M)t;

                var m_x_t = m * t;
                var t_x_m = t * m;

                var m_x_t_ref = m * maugt;
                var t_x_m_ref = mt * maugm;

                var p = rnd.UniformV3d() * rnd.UniformInt(1000);

                {
                    var res_mt = matrixTransform(m_x_t, p);
                    var res_mt_ref = matrixTransform(m_x_t_ref, p);
                    var res_tm = matrixTransform(t_x_m, p);
                    var res_tm_ref = matrixTransform(t_x_m_ref, p);

                    TrafoTesting.AreEqual(res_mt, res_mt_ref);
                    TrafoTesting.AreEqual(res_tm, res_tm_ref);
                }

                {
                    var res_mt = matrixMul(m_x_t, p);
                    var res_mt_ref = matrixMul(m_x_t_ref, p);
                    var res_tm = matrixMul(t_x_m, p);
                    var res_tm_ref = matrixMul(t_x_m_ref, p);

                    TrafoTesting.AreEqual(res_mt, res_mt_ref);
                    TrafoTesting.AreEqual(res_tm, res_tm_ref);
                }
            });


        /// <summary>
        /// Tests multiplication of T trafos with their 3x3 matrix representation.
        /// </summary>
        public static void Generic3x3MultiplicationTest<T>(Func<RandomSystem, T> frnd, Func<T, V3d, V3d> trafoTransform, Func<T, V3d, V3d> trafoMul)
            => GenericMatrixMultiplicationTest<T, M33d, M33d>(
                    frnd,
                    trafoTransform,
                    Mat.Transform,
                    Mat.Transform, 
                    trafoMul,
                    (m, v) => m * v,
                    (m, v) => m * v);

        /// <summary>
        /// Tests multiplication of T trafos with their 3x4 matrix representation.
        /// </summary>
        public static void Generic3x4MultiplicationTest<T>(Func<RandomSystem, T> frnd, Func<T, V3d, V3d> trafoTransform, Func<T, V3d, V3d> trafoMul)
            => GenericMatrixMultiplicationTest<T, M34d, M34d>(
                    frnd,
                    trafoTransform,
                    Mat.TransformPos,
                    Mat.TransformPos,
                    trafoMul,
                    (m, v) => (m * new V4d(v, 1)),
                    (m, v) => (m * new V4d(v, 1)),
                    false);

        /// <summary>
        /// Tests multiplication of T trafos with random 3x4 matrices.
        /// </summary>
        public static void GenericFull3x4MultiplicationTest<T>(Func<RandomSystem, T> frnd)
            => GenericFullMatrixMultiplicationTest<T, M34d, M44d>(
                    frnd,
                    GetRandom3x4,
                    Mat.TransformPos,
                    (m, v) => (m * new V4d(v, 1)));

        /// <summary>
        /// Tests multiplication of T trafos with their 4x4 matrix representation.
        /// </summary>
        public static void Generic4x4MultiplicationTest<T>(Func<RandomSystem, T> frnd, Func<T, V3d, V3d> trafoTransform, Func<T, V3d, V3d> trafoMul)
            => GenericMatrixMultiplicationTest<T, M44d, M44d>(
                    frnd,
                    trafoTransform, 
                    Mat.TransformPos,
                    Mat.TransformPos,
                    trafoMul,
                    (m, v) => (m * new V4d(v, 1)).XYZ,
                    (m, v) => (m * new V4d(v, 1)).XYZ);

        /// <summary>
        /// Tests multiplication of T trafos with random 4x4 matrices.
        /// </summary>
        public static void GenericFull4x4MultiplicationTest<T>(Func<RandomSystem, T> frnd)
            => GenericFullMatrixMultiplicationTest<T, M44d, M44d>(
                    frnd,
                    GetRandom4x4,
                    Mat.TransformPos,
                    (m, v) => (m * new V4d(v, 1)).XYZ);

        /// <summary>
        /// Tests if a T trafo can be converted into its string representation and parsed from it again.
        /// </summary>
        public static void GenericToStringAndParseTest<T>(Func<RandomSystem, T> frnd, Func<string, T> fparse)
            => GenericConversionTest(frnd, a => a.ToString(), fparse);
    }
}
