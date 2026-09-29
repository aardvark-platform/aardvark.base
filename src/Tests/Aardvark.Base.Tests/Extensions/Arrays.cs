using NUnit.Framework;
using System;
using System.Linq;
using Aardvark.Base;

namespace Aardvark.Tests.Extensions
{
    static class Arrays
    {
        private static void AssertParamName<TException>(string paramName, TestDelegate code)
            where TException : ArgumentException
        {
            var ex = Assert.Throws<TException>(code);
            Assert.AreEqual(paramName, ex.ParamName);
        }

        [Test]
        public static void UntypedArrayCopyPreservesShapeValuesAndIndependentStorage()
        {
            void Check(Array source)
            {
                var value = 1;
                ForEachArrayCoordinate(source, indices => source.SetValue(value++, indices));
                var copy = AssertUntypedArrayCopy(source);
                if (source.Length == 0) return;

                var first = Enumerable.Range(0, source.Rank).Select(source.GetLowerBound).ToArray();
                var context = ArrayShape(source);
                source.SetValue(-37, first);
                Assert.AreEqual(1, copy.GetValue(first), context + ": source mutation changed copy");
                copy.SetValue(-73, first);
                Assert.AreEqual(-37, source.GetValue(first), context + ": copy mutation changed source");
            }

            // Concrete regression: reflective reconstruction discarded these lower bounds.
            Check(Array.CreateInstance(typeof(int), new[] { 2, 3 }, new[] { -2, 5 }));
            foreach (var source in new Array[]
            {
                Array.Empty<int>(), new[] { 3, 7, 11 }, new int[2, 3], new int[2, 0], new int[1, 2, 3]
            }) Check(source);

            foreach (var lengths in new[]
            {
                new[] { 5 }, new[] { 2, 3 }, new[] { 2, 2, 3 }, new[] { 1, 2, 1, 3 },
                new[] { 0 }, new[] { 0, 3 }, new[] { 2, 0 }, new[] { 0, 2, 3 },
                new[] { 2, 0, 3 }, new[] { 2, 3, 0 }, new[] { 1, 0, 1, 3 }
            })
            foreach (var lowerBounds in new[]
            {
                new int[lengths.Length],
                Enumerable.Range(1, lengths.Length).ToArray(),
                Enumerable.Range(1, lengths.Length).Select(i => -i).ToArray(),
                Enumerable.Range(0, lengths.Length).Select(i => i % 2 == 0 ? -2 : 5).ToArray()
            }) Check(Array.CreateInstance(typeof(int), lengths, lowerBounds));
        }

        [Test]
        public static void UntypedArrayCopyKeepsReferenceAndJaggedElementsShared()
        {
            var reference = new System.Text.StringBuilder("shared");
            var inner = new[] { 17, 23 };
            foreach (var source in new Array[]
            {
                new object[] { reference, null, reference },
                new[] { new string('a', 2), null, "last" },
                new[] { inner, null, inner },
                Array.CreateInstance(typeof(object), new[] { 2, 2 }, new[] { 3, -2 }),
                Array.CreateInstance(typeof(int[]), new[] { 2, 1, 2 }, new[] { -2, 3, 1 }),
                Array.Empty<object>(), Array.Empty<int[]>(),
                Array.CreateInstance(typeof(object), new[] { 0, 2 }, new[] { -1, 4 })
            })
            {
                if (source.Rank > 1)
                {
                    object item = source.GetType().GetElementType() == typeof(int[]) ? inner : reference;
                    var ordinal = 0;
                    ForEachArrayCoordinate(source, indices => source.SetValue(ordinal++ % 2 == 0 ? item : null, indices));
                }
                var copy = AssertUntypedArrayCopy(source);
                var context = ArrayShape(source);
                ForEachArrayCoordinate(source, indices =>
                    Assert.AreSame(source.GetValue(indices), copy.GetValue(indices), context + $": [{string.Join(",", indices)}]"));
                if (source.Length == 0) continue;

                var first = Enumerable.Range(0, source.Rank).Select(source.GetLowerBound).ToArray();
                var original = source.GetValue(first);
                if (original is System.Text.StringBuilder text)
                {
                    text.Append('!');
                    Assert.AreEqual(text.ToString(), ((System.Text.StringBuilder)copy.GetValue(first)).ToString(), context);
                }
                else if (original is int[] nested)
                {
                    nested[0]++;
                    Assert.AreEqual(nested[0], ((int[])copy.GetValue(first))[0], context);
                }
                copy.SetValue(null, first);
                Assert.AreSame(original, source.GetValue(first), context + ": copy mutation changed source");
                copy.SetValue(original, first);
                source.SetValue(null, first);
                Assert.AreSame(original, copy.GetValue(first), context + ": source mutation changed copy");
            }
        }

        [Test]
        public static void UntypedArrayCopyPreservesNullInputException()
        {
            Assert.Throws<NullReferenceException>(() => NonGenericArrayExtensions.Copy((Array)null));
        }

        private static string ArrayShape(Array array)
            => $"{array.GetType()}, lengths=[{string.Join(",", Enumerable.Range(0, array.Rank).Select(array.GetLength))}], " +
               $"lowerBounds=[{string.Join(",", Enumerable.Range(0, array.Rank).Select(array.GetLowerBound))}]";

        private static Array AssertUntypedArrayCopy(Array source)
        {
            var context = ArrayShape(source);
            var copy = NonGenericArrayExtensions.Copy(source);
            Assert.AreNotSame(source, copy, context);
            Assert.AreEqual(source.GetType(), copy.GetType(), context);
            Assert.AreEqual(source.Rank, copy.Rank, context);
            Assert.AreEqual(source.LongLength, copy.LongLength, context);
            for (var dimension = 0; dimension < source.Rank; dimension++)
            {
                Assert.AreEqual(source.GetLength(dimension), copy.GetLength(dimension), context + $", dimension={dimension}");
                Assert.AreEqual(source.GetLowerBound(dimension), copy.GetLowerBound(dimension), context + $", dimension={dimension}");
            }
            ForEachArrayCoordinate(source, indices =>
                Assert.AreEqual(source.GetValue(indices), copy.GetValue(indices), context + $": [{string.Join(",", indices)}]"));
            return copy;
        }

        private static void ForEachArrayCoordinate(Array array, Action<int[]> action)
        {
            var indices = Enumerable.Range(0, array.Rank).Select(array.GetLowerBound).ToArray();
            for (var element = 0; element < array.Length; element++)
            {
                action(indices);
                for (var dimension = array.Rank - 1; dimension >= 0; dimension--)
                {
                    if (++indices[dimension] <= array.GetUpperBound(dimension)) break;
                    indices[dimension] = array.GetLowerBound(dimension);
                }
            }
        }

        [Test]
        public unsafe static void CopyArrayToNative()
        {
            var src = new ushort[] { 5, 3, 8, 12, 83 };
            var dst = new ushort[3];

            fixed (ushort* ptr = dst)
            {
                src.CopyTo(1, 3, (IntPtr)ptr);
            }

            Assert.AreEqual(src[1], dst[0]);
            Assert.AreEqual(src[2], dst[1]);
            Assert.AreEqual(src[3], dst[2]);
        }

        [Test]
        public unsafe static void CopyNativeToArray()
        {
            var src = new ushort[] { 5, 3, 8, 12, 83 };
            var dst = new ushort[5];

            fixed (ushort* ptr = src)
            {
                ((IntPtr)ptr).CopyTo(dst, 1, 3);
            }

            Assert.AreEqual(src[0], dst[1]);
            Assert.AreEqual(src[1], dst[2]);
            Assert.AreEqual(src[2], dst[3]);
        }

        [Test]
        public unsafe static void CopyNativeToNative()
        {
            var src = new ushort[] { 5, 3, 8, 12, 83 };
            var dst = new ushort[src.Length];

            fixed (ushort* srcPtr = src, dstPtr = dst)
            {
                ((IntPtr)srcPtr).CopyTo((IntPtr)dstPtr, sizeof(ushort) * src.Length);
            }

            for (int i = 0; i < src.Length; i++)
                Assert.AreEqual(src[i], dst[i]);
        }

        [Test]
        public static void TakeFractionValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            CollectionAssert.AreEqual(Array.Empty<int>(), array.TakeFraction(0.0).ToArray());
            CollectionAssert.AreEqual(new[] { 1, 2 }, array.TakeFraction(0.4).ToArray());
            CollectionAssert.AreEqual(array, array.TakeFraction(1.0).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.TakeFraction(0.5).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("fraction", () => array.TakeFraction(double.NaN).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("fraction", () => array.TakeFraction(-0.1).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("fraction", () => array.TakeFraction(1.1).ToArray());
        }

        [Test]
        public static void SkipLastValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4 };

            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, array.SkipLast(1).ToArray());
            CollectionAssert.AreEqual(array, array.SkipLast(0).ToArray());
            CollectionAssert.AreEqual(new[] { 1, 2 }, array.SkipLast(2).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.SkipLast(4).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.SkipLast(5).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.SkipLast(5L).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.SkipLast(1).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.SkipLast(-1).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.SkipLast(-1L).ToArray());
        }

        [Test]
        public static void ElementsValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4, 5 };

            CollectionAssert.AreEqual(new[] { 2, 3, 4 }, array.Elements(1, 3).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.Elements(array.LongLength, 0).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.Elements(0, 0).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.Elements(-1, 0).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.Elements(array.LongLength + 1, 0).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.Elements(0, -1).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.Elements(1, array.LongLength).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.Elements(1, long.MaxValue).ToArray());
        }

        [Test]
        public static void ElementsWhereIntValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            Func<int, bool> even = x => x % 2 == 0;

            CollectionAssert.AreEqual(new[] { 2, 4 }, array.ElementsWhere(1, 4, even).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.ElementsWhere(array.Length, 0, even).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.ElementsWhere(0, 0, even).ToArray());
            AssertParamName<ArgumentNullException>("predicate", () => array.ElementsWhere(0, 0, (Func<int, bool>)null).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.ElementsWhere(-1, 0, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.ElementsWhere(array.Length + 1, 0, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(0, -1, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(1, array.Length, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(1, int.MaxValue, even).ToArray());
        }

        [Test]
        public static void ElementsWhereLongValidOutputAndInvalidInputs()
        {
            var array = new[] { 1, 2, 3, 4, 5 };
            Func<int, bool> even = x => x % 2 == 0;

            CollectionAssert.AreEqual(new[] { 2, 4 }, array.ElementsWhere(1L, 4L, even).ToArray());
            CollectionAssert.AreEqual(Array.Empty<int>(), array.ElementsWhere(array.LongLength, 0L, even).ToArray());

            int[] nullArray = null;
            AssertParamName<ArgumentNullException>("array", () => nullArray.ElementsWhere(0L, 0L, even).ToArray());
            AssertParamName<ArgumentNullException>("predicate", () => array.ElementsWhere(0L, 0L, (Func<int, bool>)null).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.ElementsWhere(-1L, 0L, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("first", () => array.ElementsWhere(array.LongLength + 1, 0L, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(0L, -1L, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(1L, array.LongLength, even).ToArray());
            AssertParamName<ArgumentOutOfRangeException>("count", () => array.ElementsWhere(1L, long.MaxValue, even).ToArray());
        }
    }
}
