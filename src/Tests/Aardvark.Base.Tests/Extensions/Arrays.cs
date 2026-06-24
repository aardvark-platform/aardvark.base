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
