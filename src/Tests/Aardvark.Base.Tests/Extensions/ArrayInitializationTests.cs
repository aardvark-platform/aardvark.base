using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.Extensions
{
    [TestFixture]
    public class ArrayInitializationTests
    {
        private static T[] Initialize<T>(T[] array, int start, long count, bool longIndices, Func<long, T> callback)
            => longIndices
                ? ArrayFun.SetByIndexLong(array, start, count, callback)
                : ArrayFun.SetByIndex(array, start, checked((int)count), callback == null ? null : i => callback(i));

        private static int[] ExpectedIndices(int length, int start, long count)
        {
            // Select from existing indices using an unbounded mathematical endpoint.
            var end = (BigInteger)start + count;
            return Enumerable.Range(0, length).Where(i => i >= start && i < end).ToArray();
        }

        [Test]
        public void PositiveCountsClipToCapacityAndMatchWideOracle()
        {
            Assert.Multiple(() =>
            {
                foreach (var longIndices in new[] { false, true })
                {
                    var maximum = longIndices ? long.MaxValue : int.MaxValue;
                    foreach (var start in new[] { 1, 2, 4 })
                    foreach (var count in new[] { maximum, maximum - 1, maximum - start + 1 })
                        CheckSpan(5, start, count, longIndices, "extreme nonzero start");
                }
            });

            foreach (var longIndices in new[] { false, true })
            {
                var maximum = longIndices ? long.MaxValue : int.MaxValue;
                foreach (var length in new[] { 0, 1, 5, 16 })
                for (var start = 0; start <= length; start++)
                foreach (var count in new long[] { 1, 2, Math.Max(1, length - start), length + 7, maximum, maximum - 1 })
                    CheckSpan(length, start, count, longIndices, "bounded/clipped span");

                const int seed = 48197;
                var random = new Random(seed);
                for (var sample = 0; sample < 256; sample++)
                {
                    var length = random.Next(0, 257);
                    var start = random.Next(length + 1);
                    var count = sample % 3 == 0 ? maximum - random.Next(0, 257)
                        : sample % 3 == 1 ? random.Next(1, 514)
                        : longIndices ? random.NextInt64(1, long.MaxValue) : random.Next(1, int.MaxValue);
                    CheckSpan(length, start, count, longIndices, $"seed={seed}, sample={sample}");
                }
            }
        }

        private static void CheckSpan(int length, int start, long count, bool longIndices, string context)
        {
            context += $", longIndices={longIndices}, length={length}, start={start}, count={count}";
            var array = Enumerable.Repeat(-1, length).ToArray();
            var expected = (int[])array.Clone();
            var indices = ExpectedIndices(length, start, count);
            foreach (var index in indices) expected[index] = 7 + 3 * index;
            var calls = new List<long>();
            var result = Initialize(array, start, count, longIndices, index =>
            {
                calls.Add(index);
                Assert.AreEqual(-1, array[index], context + ": callback precedes its write");
                if (index > start) Assert.AreEqual(7 + 3 * (index - 1), array[index - 1], context + ": preceding write visible");
                return 7 + 3 * (int)index;
            });
            Assert.AreSame(array, result, context);
            CollectionAssert.AreEqual(indices.Select(i => (long)i), calls, context + ": callback indices and count");
            CollectionAssert.AreEqual(expected, array, context + ": writes and untouched elements");
        }

        [Test]
        public void EmptySpansReturnTheSameArrayWithoutCallingTheCallback()
        {
            foreach (var longIndices in new[] { false, true })
            foreach (var length in new[] { 0, 1, 5 })
            for (var start = 0; start <= length; start++)
            foreach (var count in new long[] { 0, -1, -7, int.MinValue, longIndices ? long.MinValue : int.MinValue, 1, longIndices ? long.MaxValue : int.MaxValue })
            {
                if (count > 0 && start < length) continue;
                var context = $"longIndices={longIndices}, length={length}, start={start}, count={count}";
                var array = Enumerable.Repeat(19, length).ToArray();
                var snapshot = (int[])array.Clone();
                var calls = 0;
                Assert.AreSame(array, Initialize(array, start, count, longIndices, _ => { calls++; throw new AssertionException(context); }), context);
                Assert.AreEqual(0, calls, context);
                Assert.AreSame(array, Initialize<int>(array, start, count, longIndices, null), context + ": null callback");
                CollectionAssert.AreEqual(snapshot, array, context);
            }
        }

        [Test]
        public void CallbackFailuresPropagateAndRetainOnlyPrecedingWrites()
        {
            foreach (var longIndices in new[] { false, true })
            foreach (var start in new[] { 1, 3, 7 })
            foreach (var count in new long[] { 1, 3, 64, longIndices ? long.MaxValue : int.MaxValue })
            {
                var indices = ExpectedIndices(8, start, count);
                foreach (var throwOffset in new[] { 0, indices.Length / 2, indices.Length - 1 }.Distinct())
                {
                    var context = $"longIndices={longIndices}, start={start}, count={count}, throwOffset={throwOffset}";
                    var array = Enumerable.Repeat(-1, 8).ToArray();
                    var expected = (int[])array.Clone();
                    foreach (var index in indices.Take(throwOffset)) expected[index] = index + 23;
                    var calls = new List<long>();
                    var failure = new InvalidOperationException(context);
                    var error = Assert.Throws<InvalidOperationException>(() => Initialize(array, start, count, longIndices, index =>
                    {
                        calls.Add(index);
                        if (calls.Count == throwOffset + 1) throw failure;
                        return (int)index + 23;
                    }), context);
                    Assert.AreSame(failure, error, context);
                    CollectionAssert.AreEqual(indices.Take(throwOffset + 1).Select(i => (long)i), calls, context);
                    CollectionAssert.AreEqual(expected, array, context);
                }
                var untouched = Enumerable.Repeat(-1, 8).ToArray();
                Assert.Throws<NullReferenceException>(() => Initialize<int>(untouched, start, count, longIndices, null));
                CollectionAssert.AreEqual(Enumerable.Repeat(-1, 8), untouched);
            }
        }
    }
}
