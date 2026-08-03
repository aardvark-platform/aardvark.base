using System;
using System.Collections;
using System.Collections.Generic;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class VectorArrayExtensionsTests
    {
        private sealed class SingleUseEnumerable<T> : IEnumerable<T>
        {
            private readonly IEnumerable<T> m_source;

            public int EnumerationCount { get; private set; }

            public SingleUseEnumerable(IEnumerable<T> source)
            {
                m_source = source;
            }

            public IEnumerator<T> GetEnumerator()
            {
                EnumerationCount++;
                if (EnumerationCount > 1)
                    throw new InvalidOperationException("Sequence was enumerated more than once.");

                return m_source.GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private sealed class NeverEnumerate<T> : IEnumerable<T>
        {
            public IEnumerator<T> GetEnumerator()
            {
                throw new AssertionException("Sequence must not be enumerated.");
            }

            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }

        private static readonly V3d[] s_vectors =
        {
            new V3d(3.0, 0.0, 0.0),
            new V3d(0.0, 4.0, 0.0),
        };

        private static readonly V3d s_untouched = new V3d(-1.0, -1.0, -1.0);

        private static WeightedIndex[] Weight(int index)
        {
            return new[] { new WeightedIndex(1.0, index) };
        }

        private static IEnumerable<WeightedIndex[]> InfiniteWeights(Action consumed)
        {
            int index = 0;
            while (true)
            {
                consumed();
                yield return Weight(index++ % s_vectors.Length);
            }
        }

        private static Array Dispatch(
            bool normalize, IEnumerable<WeightedIndex[]> weights, V3d[] target, int offset)
        {
            return normalize
                ? ((Array)s_vectors).LerpAndNormalizeTo(weights, target, offset)
                : ((Array)s_vectors).LerpTo(weights, target, offset);
        }

        private static V3d Expected(int index, bool normalize)
        {
            return normalize ? s_vectors[index].Normalized : s_vectors[index];
        }

        private static void AssertParamName<TException>(string paramName, TestDelegate action)
            where TException : ArgumentException
        {
            var exception = Assert.Throws<TException>(action);
            Assert.AreEqual(paramName, exception.ParamName);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ConvenienceOverloadsEnumerateOneShotInputsOnceAndLeaveShortTails(bool normalize)
        {
            var weights = new SingleUseEnumerable<WeightedIndex[]>(
                new[] { Weight(0), Weight(1) });
            var target = new[] { s_untouched, s_untouched, s_untouched, s_untouched };

            var result = Dispatch(normalize, weights, target, 1);

            Assert.AreSame(target, result);
            Assert.AreEqual(1, weights.EnumerationCount);
            CollectionAssert.AreEqual(
                new[] { s_untouched, Expected(0, normalize), Expected(1, normalize), s_untouched },
                target);
        }

        [TestCase(false)]
        [TestCase(true)]
        [Timeout(2000)]
        public void ConvenienceOverloadsBoundInfiniteInputsToRemainingCapacity(bool normalize)
        {
            int consumed = 0;
            var target = new[] { s_untouched, s_untouched, s_untouched, s_untouched };

            Dispatch(normalize, InfiniteWeights(() => consumed++), target, 1);

            Assert.AreEqual(3, consumed);
            CollectionAssert.AreEqual(
                new[]
                {
                    s_untouched,
                    Expected(0, normalize),
                    Expected(1, normalize),
                    Expected(0, normalize),
                },
                target);
        }

        [Test]
        public void CombineToHonorsExplicitWriteLimitWithoutOverreading()
        {
            int consumed = 0;
            var target = new[] { s_untouched, s_untouched, s_untouched, s_untouched, s_untouched };

            s_vectors.CombineTo(
                InfiniteWeights(() => consumed++),
                (source, weights) => source.Lerp(weights),
                target, 1, 2);

            Assert.AreEqual(2, consumed);
            CollectionAssert.AreEqual(
                new[] { s_untouched, s_vectors[0], s_vectors[1], s_untouched, s_untouched },
                target);
        }

        [TestCase(false)]
        [TestCase(true)]
        public void ZeroLengthAndFullTargetOperationsAreNoOps(bool normalize)
        {
            var weights = new NeverEnumerate<WeightedIndex[]>();
            var target = new[] { s_untouched, s_untouched };

            s_vectors.CombineTo(
                weights,
                (source, indices) => throw new AssertionException("Combiner must not be called."),
                target, 1, 0);
            var result = Dispatch(normalize, weights, target, target.Length);

            Assert.AreSame(target, result);
            CollectionAssert.AreEqual(new[] { s_untouched, s_untouched }, target);
        }

        [Test]
        public void CombineToRejectsInvalidSlicesBeforeMutation()
        {
            var weights = new NeverEnumerate<WeightedIndex[]>();
            var target = new[] { s_untouched, s_untouched, s_untouched };
            Func<V3d[], WeightedIndex[], V3d> combine = (source, indices) => source.Lerp(indices);

            AssertParamName<ArgumentNullException>(
                "target", () => s_vectors.CombineTo(weights, combine, null, 0, 0));
            AssertParamName<ArgumentOutOfRangeException>(
                "offset", () => s_vectors.CombineTo(weights, combine, target, -1, 1));
            AssertParamName<ArgumentOutOfRangeException>(
                "offset", () => s_vectors.CombineTo(weights, combine, target, target.Length + 1, 0));
            AssertParamName<ArgumentOutOfRangeException>(
                "length", () => s_vectors.CombineTo(weights, combine, target, 1, -1));
            AssertParamName<ArgumentOutOfRangeException>(
                "length", () => s_vectors.CombineTo(weights, combine, target, 2, 2));
            AssertParamName<ArgumentOutOfRangeException>(
                "length", () => s_vectors.CombineTo(weights, combine, target, 1, int.MaxValue));
            AssertParamName<ArgumentOutOfRangeException>(
                "offset", () => ((Array)s_vectors).LerpTo(weights, target, -1));

            CollectionAssert.AreEqual(
                new[] { s_untouched, s_untouched, s_untouched }, target);
        }
    }
}
