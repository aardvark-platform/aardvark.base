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
        /// Tests if a T trafo can be converted into its string representation and parsed from it again.
        /// </summary>
        public static void GenericToStringAndParseTest<T>(Func<RandomSystem, T> frnd, Func<string, T> fparse)
            => GenericConversionTest(frnd, a => a.ToString(), fparse);
    }
}
