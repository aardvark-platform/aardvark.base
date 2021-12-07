using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using static System.Math;

namespace Aardvark.Base
{
    public static class ArrayFun
    {
        #region Generic Array Setting (initialization)

        /// <summary>
        /// Set all elements to the same supplied value.
        /// </summary>
        /// <returns>this</returns>
        public static T[] Set<T>(this T[] self, T value)
        {
            var len = self.LongLength;
            for (int i = 0; i < len; i++) self[i] = value;
            return self;
        }

        /// <summary>
        /// Set count elements to the same  supplied value.
        /// </summary>
        /// <returns>this</returns>
        public static T[] Set<T>(this T[] self, long count, T value)
        {
            if (count > self.LongLength) count = self.LongLength;
            for (long i = 0; i < count; i++) self[i] = value;
            return self;
        }

        /// <summary>
        /// Set count elements starting at the supplied index to the same
        /// supplied value.
        /// </summary>
        /// <returns>this</returns>
        public static T[] Set<T>(
                this T[] self, long start, long count, T value)
        {
            long end = start + Math.Min(count, self.LongLength - start);
            for (long i = start; i < end; i++) self[i] = value;
            return self;
        }

        /// <summary>
        /// Set all elements to a function of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[] SetByIndex<T>(this T[] self, Func<int, T> index_fun)
        {
            int count = self.Length;
            for (int i = 0; i < count; i++) self[i] = index_fun(i);
            return self;
        }

        /// <summary>
        /// Set all elements to a function of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[] SetByIndexLong<T>(this T[] self, Func<long, T> index_fun)
        {
            long count = self.LongLength;
            for (long i = 0; i < count; i++) self[i] = index_fun(i);
            return self;
        }

        /// <summary>
        /// Set count elements to a function of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[] SetByIndex<T>(
                this T[] self, int count, Func<int, T> index_fun)
        {
            if (count > self.Length) count = self.Length;
            for (int i = 0; i < count; i++) self[i] = index_fun(i);
            return self;
        }

        /// <summary>
        /// Set count elements to a function of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[] SetByIndexLong<T>(
                this T[] self, long count, Func<long, T> index_fun)
        {
            if (count > self.LongLength) count = self.LongLength;
            for (long i = 0; i < count; i++) self[i] = index_fun(i);
            return self;
        }

        /// <summary>
        /// Set count elements starting at the supplied index to a function
        /// of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[] SetByIndex<T>(
                this T[] self, int start, int count, Func<int, T> index_fun)
        {
            int end = Math.Min(start + count, self.Length);
            for (int i = start; i < end; i++) self[i] = index_fun(i);
            return self;
        }

        /// <summary>
        /// Set count elements starting at the supplied index to a function
        /// of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[] SetByIndexLong<T>(
                this T[] self, long start, long count, Func<long, T> index_fun)
        {
            long end = Math.Min(start + count, self.LongLength);
            for (long i = start; i < end; i++) self[i] = index_fun(i);
            return self;
        }

        /// <summary>
        /// Set all elements to the elements of the supplied array.
        /// </summary>
        public static T[] Set<T>(this T[] self, T[] array)
        {
            long len = Math.Min(self.LongLength, array.LongLength);
            for (long i = 0; i < len; i++) self[i] = array[i];
            return self;
        }

        /// <summary>
        /// Set all elements to the same supplied value.
        /// </summary>
        /// <returns>this</returns>
        public static T[,] Set<T>(this T[,] self, T value)
        {

            for (long i = 0; i < self.GetLongLength(0); i++)
                for (long j = 0; j < self.GetLongLength(1); j++)
                    self[i, j] = value;
            return self;
        }

        #endregion

        #region Generic Array Copying

        /// <summary>
        /// Use this instead of Clone() in order to get a typed array back.
        /// </summary>
        public static T[] Copy<T>(this T[] array)
        {
            var count = array.LongLength;
            var result = new T[count];
            for (var i = 0L; i < count; i++) result[i] = array[i];
            return result;
        }

        /// <summary>
        /// Create a copy of the specified length. If the copy is longer
        /// it is filled with default elements.
        /// </summary>
        public static T[] Copy<T>(this T[] array, long count)
        {
            var result = new T[count];
            var len = Math.Min(count, array.LongLength);
            for (var i = 0L; i < len; i++) result[i] = array[i];
            return result;
        }

        /// <summary>
        /// Create a copy of the specified length. If the copy is longer
        /// it is filled with default elements.
        /// </summary>
        public static T[] Copy<T>(this T[] array, int count)
        {
            var result = new T[count];
            var len = Math.Min(count, array.Length);
            for (var i = 0; i < len; i++) result[i] = array[i];
            return result;
        }

        /// <summary>
        /// Create a copy of the specified length starting at the specified
        /// start. If the copy is longer it is filled with default elements.
        /// </summary>
        public static T[] Copy<T>(this T[] array, long start, long count)
        {
            var result = new T[count];
            var len = Math.Min(count, array.LongLength - start);
            for (var i = 0L; i < len; i++) result[i] = array[i + start];
            return result;
        }

        /// <summary>
        /// Create a copy of the specified length starting at the specified
        /// start. If the copy is longer it is filled with default elements.
        /// </summary>
        public static T[] Copy<T>(this T[] array, int start, int count)
        {
            var result = new T[count];
            var len = Math.Min(count, array.Length - start);
            for (var i = 0; i < len; i++) result[i] = array[i + start];
            return result;
        }

        /// <summary>
        /// Create a copy with the elements piped through a function.
        /// </summary>
        public static Tr[] Map<T, Tr>(this T[] array, Func<T, Tr> item_fun)
        {
            var len = array.LongLength;
            var result = new Tr[len];
            for (var i = 0L; i < len; i++) result[i] = item_fun(array[i]);
            return result;
        }

        /// <summary>
        /// Create an array of resulting items by applying a supplied binary function
        /// to corresponding pairs of the supplied arrays.
        /// </summary>
        /// <returns></returns>
        public static Tr[] Map2<T0, T1, Tr>(
                this T0[] array0, T1[] array1, Func<T0, T1, Tr> item0_item1_fun)
        {
            var len = Min(array0.LongLength, array1.LongLength);
            var result = new Tr[len];
            for (var i = 0L; i < len; i++) result[i] = item0_item1_fun(array0[i], array1[i]);
            return result;
        }

        /// <summary>
        /// Create an array of resulting items by applying a supplied ternary function
        /// to corresponding triples of the supplied arrays.
        /// </summary>
        /// <returns></returns>
        public static Tr[] Map3<T0, T1, T2, Tr>(
                this T0[] array0, T1[] array1, T2[] array2, Func<T0, T1, T2, Tr> item0_item1_item2_fun)
        {
            var len = Min(array0.LongLength, Min(array1.LongLength, array2.LongLength));
            var result = new Tr[len];
            for (var i = 0L; i < len; i++)
                result[i] = item0_item1_item2_fun(array0[i], array1[i], array2[i]);
            return result;
        }

        /// <summary>
        /// Create a copy with the elements piped through a function.
        /// The function gets the index of the element as a second argument.
        /// </summary>
        public static Tr[] Map<T, Tr>(this T[] array, Func<T, long, Tr> item_index_fun)
        {
            var len = array.LongLength;
            var result = new Tr[len];
            for (var i = 0L; i < len; i++) result[i] = item_index_fun(array[i], i);
            return result;
        }

        public static Tr[] Map2<T0, T1, Tr>(
                this T0[] array0, T1[] array1, Func<T0, T1, long, Tr> item0_item1_index_fun)
        {
            var len = Min(array0.LongLength, array1.LongLength);
            var result = new Tr[len];
            for (var i = 0L; i < len; i++)
                result[i] = item0_item1_index_fun(array0[i], array1[i], i);
            return result;
        }

        public static Tr[] Map3<T0, T1, T2, Tr>(
                this T0[] array0, T1[] array1, T2[] array2,
                Func<T0, T1, T2, long, Tr> item0_item1_item2_index_fun)
        {
            var len = Min(array0.LongLength, Min(array1.LongLength, array2.LongLength));
            var result = new Tr[len];
            for (var i = 0L; i < len; i++)
                result[i] = item0_item1_item2_index_fun(array0[i], array1[i], array2[i], i);
            return result;
        }

        /// <summary>
        /// Create a copy of count elements with the elements piped through a
        /// function. count may be longer than the array, in this case the
        /// result array has default elements at the end.
        /// </summary>
        public static Tr[] Map<T, Tr>(
                this T[] array, long count, Func<T, Tr> element_fun)
        {
            var result = new Tr[count];
            var len = Math.Min(count, array.LongLength);
            for (var i = 0L; i < len; i++) result[i] = element_fun(array[i]);
            return result;
        }

        /// <summary>
        /// Create a copy of count elements with the elements piped through a
        /// function. count may be longer than the array, in this case the
        /// result array has default elements at the end.
        /// </summary>
        public static Tr[] Map<T, Tr>(
                this T[] array, int count, Func<T, Tr> element_fun)
        {
            var result = new Tr[count];
            var len = Math.Min(count, array.Length);
            for (var i = 0; i < len; i++) result[i] = element_fun(array[i]);
            return result;
        }

        /// <summary>
        /// Create a copy of count elements with the elements piped through a
        /// function. count may be longer than the array, in this case the
        /// result array has default elements at the end.
        /// The function gets the index of the element as a second argument.
        /// </summary>
        public static Tr[] Map<T, Tr>(
                this T[] array, long count, Func<T, long, Tr> element_index_fun)
        {
            var result = new Tr[count];
            var len = Math.Min(count, array.LongLength);
            for (var i = 0L; i < len; i++) result[i] = element_index_fun(array[i], i);
            return result;
        }

        /// <summary>
        /// Create a copy of specified length starting at the specified
        /// offset with the elements piped through a function.
        /// </summary>
        public static Tr[] Map<T, Tr>(
                this T[] array, long start, long count, Func<T, Tr> element_fun)
        {
            var result = new Tr[count];
            var len = Math.Min(count, array.LongLength - start);
            for (var i = 0L; i < len; i++) result[i] = element_fun(array[start + i]);
            return result;
        }

        /// <summary>
        /// Create a copy of specified length starting at the specified
        /// offset with the elements piped through a function.
        /// </summary>
        public static Tr[] Map<T, Tr>(
                this T[] array, int start, int count, Func<T, Tr> element_fun)
        {
            var result = new Tr[count];
            var len = Math.Min(count, array.Length - start);
            for (var i = 0; i < len; i++) result[i] = element_fun(array[start + i]);
            return result;
        }

        /// <summary>
        /// Create a copy of specified length starting at the specified
        /// offset with the elements piped through a function.
        /// The function gets the target index of the element as a second
        /// argument.
        /// </summary>
        public static Tr[] Map<T, Tr>(
                this T[] array, long start, long count, Func<T, long, Tr> element_index_fun)
        {
            var result = new Tr[count];
            var len = Math.Min(count, array.LongLength - start);
            for (var i = 0L; i < len; i++) result[i] = element_index_fun(array[start + i], i);
            return result;
        }

        /// <summary>
        /// Create a copy of specified length starting at the specified
        /// offset with the elements piped through a function.
        /// The function gets the target index of the element as a second
        /// argument.
        /// </summary>
        public static Tr[] Map<T, Tr>(
                this T[] array, int start, int count, Func<T, int, Tr> element_index_fun)
        {
            var result = new Tr[count];
            var len = Math.Min(count, array.Length - start);
            for (var i = 0; i < len; i++) result[i] = element_index_fun(array[start + i], i);
            return result;
        }

        /// <summary>
        /// Copy a range of elements to the target array.
        /// </summary>
        public static void CopyTo<T>(this T[] array, long count, T[] target, long targetStart)
        {
            for (var i = 0L; i < count; i++)
                target[targetStart + i] = array[i];
        }

        /// <summary>
        /// Copy a range of elements to the target array.
        /// </summary>
        public static void CopyTo<T>(this T[] array, int count, T[] target, int targetStart)
        {
            for (var i = 0; i < count; i++)
                target[targetStart + i] = array[i];
        }

        /// <summary>
        /// Copy a range of elements to the target array.
        /// </summary>
        public static void CopyTo<T>(this T[] array, long start, long count, T[] target, long targetStart)
        {
            for (var i = 0L; i < count; i++)
                target[targetStart + i] = array[start + i];
        }

        /// <summary>
        /// Copy a range of elements to the target array.
        /// </summary>
        public static void CopyTo<T>(this T[] array, int start, int count, T[] target, int targetStart)
        {
            for (var i = 0; i < count; i++)
                target[targetStart + i] = array[start + i];
        }

        /// <summary>
        /// Copies the array into a list.
        /// </summary>
        public static List<T> CopyToList<T>(this T[] array)
        {
            var result = new List<T>(array.Length);
            result.AddRange(array);
            return result;
        }

        public static List<Tr> MapToList<T, Tr>(this T[] array, Func<T, Tr> element_fun)
        {
            var count = array.Length;
            var result = new List<Tr>(count);
            for (int i = 0; i < count; i++) result.Add(element_fun(array[i]));
            return result;
        }

        public static List<Tr> MapToList<T, Tr>(this T[] array, Func<T, long, Tr> item_index_fun)
        {
            var count = array.Length;
            var result = new List<Tr>(count);
            for (int i = 0; i < count; i++) result.Add(item_index_fun(array[i], i));
            return result;
        }

        /// <summary>
        /// Copies the specified range of elements to the specified destination
        /// within the same array.
        /// </summary>
        public static void CopyRange<T>(this T[] array, long start, long count, long targetStart)
        {
            var end = start + count;
            if (targetStart < start || targetStart >= end)
            {
                while (start < end) array[targetStart++] = array[start++];
            }
            else
            {
                targetStart += count;
                while (start < end) array[--targetStart] = array[--end];
            }
        }

        /// <summary>
        /// Create a copy with the elements reversed. 
        /// </summary>
        public static T[] CopyReversed<T>(this T[] array)
        {
            var result = new T[array.LongLength];
            var lastIndex = array.LongLength - 1;
            for (var i = 0L; i <= lastIndex; i++)
            {
                result[i] = array[lastIndex - i];
            }
            return result;
        }

        /// <summary>
        /// Concatenates two arrays.
        /// </summary>
        /// <returns>A new array with content of <paramref name="first"/> concatenated with content of <paramref name="second"/>.</returns>
        public static T[] Concat<T>(this T[] first, T[] second)
        {
            var conArray = new T[first.Length + second.Length];
            first.CopyTo(conArray, 0);
            second.CopyTo(conArray, first.Length);
            return conArray;
        }

        #endregion

        #region Generic Array Operations

        public static long LongSum<T>(this T[] array, Func<T, long> selector)
        {
            long sum = 0;
            for (long i = 0; i < array.LongLength; i++) sum += selector(array[i]);
            return sum;
        }

        public static T[] Resized<T>(this T[] array, int newSize)
        {
            Array.Resize(ref array, newSize);
            return array;
        }

        public static T[] Resized<T>(this T[] array, long newSize)
        {
            var resized = new T[newSize];
            newSize = Math.Min(array.LongLength, newSize);
            for (long i = 0; i < newSize; i++) resized[i] = array[i];
            return resized;
        }

        /// <summary>
        /// Enumerates the given fraction of the array elements from the
        /// beginning of the array. E.g. TakeFraction(0.1) would enumerate
        /// the first 1/10 of the array elements. Fraction must be in range
        /// [0.0, 1.0].
        /// </summary>
        public static IEnumerable<T> TakeFraction<T>(
                this T[] array, double fraction)
        {
            if (fraction < 0.0 || fraction > 1.0)
                throw new ArgumentOutOfRangeException("Fraction not in range [0.0, 1.0].");
            long take = (long)(array.LongLength * fraction);
            for (long i = 0; i < take; i++) yield return array[i];
        }

        /// <summary>
        /// Enumerate all but last elements of an array. The number of
        /// elements to omit is supplied as a parameter and defaults to 1.
        /// </summary>
        public static IEnumerable<T> SkipLast<T>(this T[] array, long count = 1)
        {
            count = array.LongLength - count;
            for (long i = 0; i < count; i++) yield return array[i];
        }

        /// <summary>
        /// Enumerate all but last elements of an array. The number of
        /// elements to omit is supplied as a parameter and defaults to 1.
        /// </summary>
        public static IEnumerable<T> SkipLast<T>(this T[] array, int count = 1)
        {
            return SkipLast(array, (long)count);
        }

        /// <summary>
        /// Merges two already sorted arrays.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">Sorted array.</param>
        /// <param name="left">Sorted array.</param>
        /// <param name="right">Sorted array.</param>
        /// <param name="comparer"></param>
        public static void Merge<T>(this T[] array, T[] left, T[] right, Func<T, T, int> comparer)
        {
            long li = 0, ri = 0, ti = 0;
            long lc = left.LongLength, rc = right.LongLength;

            while (li < lc && ri < rc)
            {
                if (comparer(left[li], right[ri]) < 0)
                {
                    array[ti++] = left[li++];
                }
                else
                {
                    array[ti++] = right[ri++];
                }
            }
            while (li < lc) array[ti++] = left[li++];
            while (ri < rc) array[ti++] = right[ri++];
        }

        private class AComparer<T> : IComparer<T>
        {
            public Func<T, T, int> Fun;
            public int Compare(T x, T y) { return Fun(x, y); }
        }

        public static int BinarySearch<T>(this T[] self, T item, Func<T, T, int> comparer)
        {
            return Array.BinarySearch(self, item, new AComparer<T> { Fun = comparer });
        }

        public static IEnumerable<T> Elements<T>(
                this T[] array, long first, long count)
        {
            for (long e = first + count; first < e; first++) yield return array[first];
        }

        public static IEnumerable<T> ElementsWhere<T>(
                this T[] array, int first, int count, Func<T, bool> predicate)
        {
            for (int end = first + count; first < end; first++)
                if (predicate(array[first])) yield return array[first];
        }

        public static IEnumerable<T> ElementsWhere<T>(
                this T[] array, long first, long count, Func<T, bool> predicate)
        {
            for (long end = first + count; first < end; first++)
                if (predicate(array[first])) yield return array[first];
        }

        #endregion

        #region Generic Array Modifications

        /// <summary>
        /// Apply the supplied transformation function to each element of the
        /// array.
        /// </summary>
        public static T[] Apply<T>(this T[] self, Func<T, T> element_fun)
        {
            long count = self.LongLength;
            for (long i = 0; i < count; i++) self[i] = element_fun(self[i]);
            return self;
        }

        /// <summary>
        /// Apply the supplied transformation function to the first count
        /// elements of the array.
        /// </summary>
        public static T[] Apply<T>(this T[] self, long count, Func<T, T> element_fun)
        {
            if (count > self.LongLength) count = self.LongLength;
            for (long i = 0; i < count; i++) self[i] = element_fun(self[i]);
            return self;
        }

        /// <summary>
        /// Apply the supplied transformation function to count
        /// elements of the array, starting at the supplied start index.
        /// </summary>
        public static T[] Apply<T>(this T[] self, long start, long count, Func<T, T> element_fun)
        {
            var end = Min(start + count, self.LongLength);
            for (long i = start; i < end; i++) self[i] = element_fun(self[i]);
            return self;
        }

        public static T[] Apply<T>(this T[] self, Func<T, long, T> element_index_fun)
        {
            long count = self.LongLength;
            for (long i = 0; i < count; i++) self[i] = element_index_fun(self[i], i);
            return self;
        }

        /// <summary>
        /// Apply the supplied transformation function to the first count
        /// elements of the array.
        /// </summary>
        public static T[] Apply<T>(this T[] self, long count, Func<T, long, T> element_index_fun)
        {
            if (count > self.LongLength) count = self.LongLength;
            for (long i = 0; i < count; i++) self[i] = element_index_fun(self[i], i);
            return self;
        }

        /// <summary>
        /// Apply the supplied transformation function to count
        /// elements of the array, starting at the supplied start index.
        /// </summary>
        public static T[] Apply<T>(this T[] self, long start, long count, Func<T, long, T> element_index_fun)
        {
            var end = Min(start + count, self.LongLength);
            for (long i = start; i < end; i++) self[i] = element_index_fun(self[i], i);
            return self;
        }

        /// <summary>
        /// Move an element from a given source index to a destination index,
        /// and shift all elements in between by one.
        /// </summary>
        public static void Move<T>(
                this T[] self, long sourceIndex, long targetIndex)
        {
            if (sourceIndex == targetIndex) return;

            T help = self[sourceIndex];
            if (targetIndex > sourceIndex)
                for (long i = sourceIndex; i < targetIndex; i++) self[i] = self[i + 1];
            else
                for (long i = sourceIndex; i > targetIndex; i--) self[i] = self[i - 1];
            self[targetIndex] = help;
        }


        /// <summary>
        /// Sets the array items to a mapped version of the items of array0.
        /// </summary>
        public static void SetMap<T, T0>(
                this T[] array, T0[] array0, Func<T0, T> item0_fun)
        {
            var count = Min(array.LongLength, array0.LongLength);
            for (long i = 0; i < count; i++)
                array[i] = item0_fun(array0[i]);
        }

        /// <summary>
        /// Sets the array items to a mapped version of corresponding
        /// pairs of items of array0 and array1.
        /// </summary>
        public static void SetMap2<T, T0, T1>(
                this T[] array, T0[] array0, T1[] array1,
                Func<T0, T1, T> item0_item1_fun)
        {
            var count = Min(array.LongLength, Min(array0.LongLength, array1.LongLength));
            for (long i = 0; i < count; i++)
                array[i] = item0_item1_fun(array0[i], array1[i]);
        }

        /// <summary>
        /// Sets the array items to a mapped version of corresponding
        /// triples of  items of array0, array1, and array2.
        /// </summary>
        public static void SetMap3<T, T0, T1, T2>(
                this T[] array, T0[] array0, T1[] array1, T2[] array2,
                Func<T0, T1, T2, T> item0_item1_item2_fun)
        {
            var count = Min(
                Min(array.LongLength, array0.LongLength),
                Min(array1.LongLength, array2.LongLength)
                );
            for (long i = 0; i < count; i++)
                array[i] = item0_item1_item2_fun(array0[i], array1[i], array2[i]);
        }

        /// <summary>
        /// Swap the two elements specified by their indices.
        /// </summary>
        public static void Swap<T>(this T[] self, int i, int j)
        {
            T help = self[i]; self[i] = self[j]; self[j] = help;
        }

        /// <summary>
        /// Swap the two elements specified by their indices.
        /// </summary>
        public static void Swap<T>(this T[] self, long i, long j)
        {
            T help = self[i]; self[i] = self[j]; self[j] = help;
        }

        /// <summary>
        /// Reverse the order of elements in the supplied range [lo, hi[.
        /// </summary>
        public static void ReverseRange<T>(
                this T[] a, long lo, long hi)
        {
            hi--;
            while (lo < hi) { var t = a[lo]; a[lo++] = a[hi]; a[hi--] = t; }
        }

        public static void Revert<T>(this T[] array)
        {
            array.ReverseRange(0, array.LongLength);
        }

        /// <summary>
        /// Returns a copy of the array with the specified item replaced.
        /// </summary>
        public static T[] With<T>(this T[] array, int index, T item)
        {
            if (array == null)
            {
                array = new T[index + 1];
                array[index] = item;
                return array;
            }
            var len = array.LongLength;
            if (index < len) { array[index] = item; return array; }
            var newArray = new T[index + 1];
            for (long i = 0; i < len; i++) newArray[i] = array[i];
            newArray[index] = item;
            return newArray;
        }

        /// <summary>
        /// Synonym for WithAppended().
        /// </summary>
        public static T[] WithAdded<T>(this T[] array, T item)
            where T : class
        {
            return array.WithAppended(item);
        }

        /// <summary>
        /// Returns a copy of the array with the specified item appended
        /// at the end. If the item is already contained in the array,
        /// the original array is returned.
        /// </summary>
        public static T[] WithAppended<T>(this T[] array, T item)
            where T : class
        {
            if (array == null) return new T[] { item };
            var len = array.Length;
            for (int i = 0; i < len; i++) if (array[i] == item) return array;
            var newArray = new T[len + 1];
            for (int i = 0; i < len; i++) newArray[i] = array[i];
            newArray[len] = item;
            return newArray;
        }

        /// <summary>
        /// Returns a copy of the array with the specified item prepended
        /// to the front. If the item is already contained in the array,
        /// the original array is returned.
        /// </summary>
        public static T[] WithPrepended<T>(this T[] array, T item)
            where T : class
        {
            if (array == null) return new T[] { item };
            var len = array.Length;
            for (int i = 0; i < len; i++) if (array[i] == item) return array;
            var newArray = new T[len + 1];
            for (int i = 0; i < len; i++) newArray[i+1] = array[i];
            newArray[0] = item;
            return newArray;
        }

        /// <summary>
        /// Returns a copy of the item with the specified item removed. If the
        /// item is not contained in the array, the original array is returned.
        /// </summary>
        public static T[] WithRemoved<T>(this T[] array, T item)
            where T : class
        {
            if (array == null) return array;
            var len = array.Length;
            if (len == 1)
            {
                if (array[0] == item) return null;
            }
            else
            {
                for (int i = 0; i < len; i++)
                    if (array[i] == item)
                    {
                        var newArray = new T[len - 1];
                        for (int j = 0; j < i; j++) newArray[j] = array[j];
                        for (int j = i; j < len - 1; j++) newArray[j] = array[j + 1];
                        return newArray;
                    }
            }
            return array;
        }


        #endregion

        #region Generic Array Functional Programming Standard Operations

        /// <summary>
        /// Performs an aggregation of all items in an array with the
        /// supplied aggregation function starting from the left and with the
        /// supplied seed, and returns the aggregated result.
        /// </summary>
        public static TSum FoldLeft<TVal, TSum>(
                this TVal[] array, TSum seed, Func<TSum, TVal, TSum> sum_item_fun)
        {
            long count = array.LongLength;
            for (long i = 0; i < count; i++)
                seed = sum_item_fun(seed, array[i]);
            return seed;
        }

        /// <summary>
        /// Performs an aggregation of all corresponding pairs of items of
        /// the supplied arrays with the supplied aggregation function
        /// starting from the left and with the  supplied seed, and returns
        /// the aggregated result.
        /// </summary>
        public static TSum FoldLeft2<T0, T1, TSum>(
                this T0[] array0, T1[] array1,
                TSum seed, Func<TSum, T0, T1, TSum> sum_item0_item1_fun)
        {
            long count = Min(array0.LongLength, array1.LongLength);
            for (long i = 0; i < count; i++)
                seed = sum_item0_item1_fun(seed, array0[i], array1[i]);
            return seed;
        }

        /// <summary>
        /// Performs an aggregation of all corresponding triples of items of
        /// the supplied arrays with the supplied aggregation function
        /// starting from the left and with the  supplied seed, and returns
        /// the aggregated result.
        /// </summary>
        public static TSum FoldLeft3<T0, T1, T2, TSum>(
                this T0[] array0, T1[] array1, T2[] array2,
                TSum seed, Func<TSum, T0, T1, T2, TSum> sum_item0_item1_item2_fun)
        {
            long count = Min(array0.LongLength, Min(array1.LongLength, array2.LongLength));
            for (long i = 0; i < count; i++)
                seed = sum_item0_item1_item2_fun(seed, array0[i], array1[i], array2[i]);
            return seed;
        }

        /// <summary>
        /// Performs an aggregation of all items in an array with the
        /// supplied aggregation function starting from the right and with the
        /// supplied seed, and returns the aggregated result.
        /// </summary>
        public static TSum FoldRight<TVal, TSum>(
                this TVal[] array, Func<TVal, TSum, TSum> item_sum_fun, TSum seed)
        {
            long count = array.LongLength;
            for (long i = count - 1; i >= 0; i--)
                seed = item_sum_fun(array[i], seed);
            return seed;
        }

        /// <summary>
        /// Performs an aggregation of all corresponding pairs of items of
        /// the supplied arrays with the supplied aggregation function
        /// starting from the rigth and with the  supplied seed, and returns
        /// the aggregated result.
        /// </summary>
        public static TSum FoldRight2<T0, T1, TSum>(
                this T0[] array0, T1[] array1,
                Func<T0, T1, TSum, TSum> item0_item1_sum_fun, TSum seed)
        {
            long count = Min(array0.LongLength, array1.LongLength);
            for (long i = count - 1; i >= 0; i--)
                seed = item0_item1_sum_fun(array0[i], array1[i], seed);
            return seed;
        }

        /// <summary>
        /// Performs an aggregation of all corresponding triples of items of
        /// the supplied arrays with the supplied aggregation function
        /// starting from the rigth and with the  supplied seed, and returns
        /// the aggregated result.
        /// </summary>
        public static TSum FoldRight3<T0, T1, T2, TSum>(
                this T0[] array0, T1[] array1, T2[] array2,
                Func<T0, T1, T2, TSum, TSum> item0_item1_item2_sum_fun, TSum seed)
        {
            long count = Min(array0.LongLength, Min(array1.LongLength, array2.LongLength));
            for (long i = count - 1; i >= 0; i--)
                seed = item0_item1_item2_sum_fun(array0[i], array1[i], array2[i], seed);
            return seed;
        }

        /// <summary>
        /// Performs an aggregation of the specified slice of items in an
        /// array with the supplied aggregation function starting from the
        /// left and with the initial supplied left sum, and returns the
        /// aggregated result.
        /// </summary>
        public static TSum FoldLeft<TVal, TSum>(
                this TVal[] array, long start, long count,
                TSum seed, Func<TSum, TVal, TSum> sum_item_fun)
        {
            for (long i = start, e = start + count; i < e; i++)
                seed = sum_item_fun(seed, array[i]);
            return seed;
        }

        /// <summary>
        /// Performs an aggregation of the specified slice of items in an
        /// array with the supplied aggregation function starting from the
        /// right and with the initial supplied right sum, and returns the
        /// aggregated result.
        /// </summary>
        public static TSum FoldRight<TVal, TSum>(
                this TVal[] array, long start, long count,
                Func<TVal, TSum, TSum> item_sum_fun, TSum seed)
        {
            for (long i = start + count - 1; i >= start; i--)
                seed = item_sum_fun(array[i], seed);
            return seed;
        }

        /// <summary>
        /// Performs an aggregation of all elements in an array with the
        /// supplied aggregation function starting from the left and with the
        /// initial supplied left sum.
        /// All intermediate results are returned as an array with the same
        /// length as the original.
        /// </summary>
        public static TSum[] ScanLeft<TVal, TSum>(
                this TVal[] array, TSum leftSum, Func<TSum, TVal, TSum> sum_val_addFun)
        {
            long count = array.LongLength;
            var result = new TSum[count];
            for (long i = 0; i < count; i++)
            {
                leftSum = sum_val_addFun(leftSum, array[i]);
                result[i] = leftSum;
            }
            return result;
        }

        /// <summary>
        /// Performs an aggregation of all elements in an array with the
        /// supplied aggregation function starting from the left and with the
        /// initial supplied left sum.
        /// All intermediate results are returned as an array with the same
        /// length as the original.
        /// </summary>
        public static TSum[] ScanLeft<TVal, TSum>(
            this TVal[] array, TSum leftSum, Func<TSum, TVal, long, TSum> sum_val_index_addFun)
        {
            long count = array.LongLength;
            var result = new TSum[count];
            for (long i = 0; i < count; i++)
            {
                leftSum = sum_val_index_addFun(leftSum, array[i], i);
                result[i] = leftSum;
            }
            return result;
        }

        /// <summary>
        /// Performs an aggregation of all elements in an array with the
        /// supplied aggregation function starting from the right and with the
        /// initial supplied right sum.
        /// All intermediate results are returned as an array with the same
        /// length as the original.
        /// </summary>
        public static TSum[] ScanRight<TVal, TSum>(
                this TVal[] array, Func<TVal, TSum, TSum> val_sum_addFun, TSum rightSum)
        {
            long count = array.LongLength;
            var result = new TSum[count];
            for (long i = count - 1; i >= 0; i--)
            {
                rightSum = val_sum_addFun(array[i], rightSum);
                result[i] = rightSum;
            }
            return result;
        }

        /// <summary>
        /// Performs an aggregation of all elements in an array with the
        /// supplied aggregation function starting from the right and with the
        /// initial supplied right sum.
        /// All intermediate results are returned as an array with the same
        /// length as the original.
        /// </summary>
        public static TSum[] ScanRight<TVal, TSum>(
            this TVal[] array, Func<TVal, long, TSum, TSum> val_index_sum_addFun, TSum rightSum)
        {
            long count = array.LongLength;
            var result = new TSum[count];
            for (long i = count - 1; i >= 0; i--)
            {
                rightSum = val_index_sum_addFun(array[i], i, rightSum);
                result[i] = rightSum;
            }
            return result;
        }

        /// <summary>
        /// Performs an aggregation of the specified slice of elements in an
        /// array with the supplied aggregation function starting from the
        /// left and with the initial supplied left sum.
        /// All intermediate results are returned as an array with the given
        /// count as length.
        /// </summary>
        public static TSum[] ScanLeft<TVal, TSum>(
                this TVal[] array, long start, long count,
                TSum leftSum, Func<TSum, TVal, TSum> sum_val_addFun)
        {
            var result = new TSum[count];
            for (long i = 0; i < count; i++)
            {
                leftSum = sum_val_addFun(leftSum, array[start + i]);
                result[i] = leftSum;
            }
            return result;
        }

        /// <summary>
        /// Performs an aggregation of the specified slice of elements in an
        /// array with the supplied aggregation function starting from the
        /// left and with the initial supplied left sum.
        /// All intermediate results are returned as an array with the given
        /// count as length.
        /// </summary>
        public static TSum[] ScanLeft<TVal, TSum>(
            this TVal[] array, long start, long count,
            TSum leftSum, Func<TSum, TVal, long, TSum> sum_val_index_addFun)
        {
            var result = new TSum[count];
            for (long i = 0; i < count; i++)
            {
                leftSum = sum_val_index_addFun(leftSum, array[start + i], start + i);
                result[i] = leftSum;
            }
            return result;
        }

        /// <summary>
        /// Performs an aggregation of the specified slice of elements in an
        /// array with the supplied aggregation function starting from the
        /// right and with the initial supplied right sum.
        /// All intermediate results are returned as an array with the given
        /// count as length.
        /// </summary>
        public static TSum[] ScanRight<TVal, TSum>(
                this TVal[] array, long start, long count,
                Func<TVal, TSum, TSum> val_sum_addFun, TSum rightSum)
        {
            var result = new TSum[count];
            for (long i = count - 1; i >= 0; i--)
            {
                rightSum = val_sum_addFun(array[start + i], rightSum);
                result[i] = rightSum;
            }
            return result;
        }

        /// <summary>
        /// Performs an aggregation of the specified slice of elements in an
        /// array with the supplied aggregation function starting from the
        /// right and with the initial supplied right sum.
        /// All intermediate results are returned as an array with the given
        /// count as length.
        /// </summary>
        public static TSum[] ScanRight<TVal, TSum>(
            this TVal[] array, long start, long count,
            Func<TVal, long, TSum, TSum> val_index_sum_addFun, TSum rightSum)
        {
            var result = new TSum[count];
            for (long i = count - 1; i >= 0; i--)
            {
                rightSum = val_index_sum_addFun(array[start + i], start + i, rightSum);
                result[i] = rightSum;
            }
            return result;
        }

        /// <summary>
        /// Apply the supplied mulFun to the first count corresponding pairs
        /// of elements from the supplied arrays. Aggregate the results
        /// starting with the supplied seed and the supplied addFun.
        /// </summary>
        public static TSum InnerProduct<T1, T2, TMul, TSum>(
                this T1[] array, T2[] other, long count,
                Func<T1, T2, TMul> mulFun,
                TSum seed, Func<TSum, TMul, TSum> addFun)
        {
            if (array == null) throw new ArgumentException("array must be != null");
            if (count > Math.Min(array.LongLength, other.LongLength))
                throw new ArgumentException("count must be smaller than minimum of array lengths");
            for (long i = 0; i < count; i++)
                seed = addFun(seed, mulFun(array[i], other[i]));
            return seed;
        }

        /// <summary>
        /// Apply the supplied mulFun to the first count corresponding pairs
        /// of elements from the supplied arrays. Aggregate the results
        /// starting with the supplied seed and the supplied addFun.
        /// </summary>
        public static TSum InnerProduct<T1, T2, TMul, TSum>(
                this T1[] array, T2[] other, long count,
                Func<T1, T2, TMul> mulFun,
                TSum seed, Func<TSum, TMul, TSum> addFun,
                Func<TSum, bool> sum_exitIfTrueFun)
        {
            if (array == null) throw new ArgumentException("array must be != null");
            if (count > Min(array.LongLength, other.LongLength))
                throw new ArgumentException("count must be smaller than minimum of array lengths");
            for (long i = 0; i < count; i++)
            {
                seed = addFun(seed, mulFun(array[i], other[i]));
                if (sum_exitIfTrueFun(seed)) break;
            }
            return seed;
        }

        /// <summary>
        /// Apply the supplied productFun to corresponding pairs of elements
        /// from supplied arrays. Aggregate the results starting with the
        /// supplied seed and the supplied sumFun.
        /// </summary>
        public static TSum InnerProduct<T1, T2, TProduct, TSum>(
                this T1[] array0, T2[] array1,
                Func<T1, T2, TProduct> item0_item1_productFun,
                TSum seed, Func<TSum, TProduct, TSum> sum_product_sumFun)
        {
            return InnerProduct(array0, array1,
                                Math.Min(array0.LongLength, array1.LongLength),
                                item0_item1_productFun, seed, sum_product_sumFun);
        }

        /// <summary>
        /// Apply the supplied productFun to corresponding pairs of elements
        /// from supplied arrays. Aggregate the results starting with the
        /// supplied seed and the supplied sumFun.
        /// </summary>
        public static TSum InnerProduct<T1, T2, TProduct, TSum>(
                this T1[] array0, T2[] array1,
                Func<T1, T2, TProduct> item0_item1_productFun,
                TSum seed, Func<TSum, TProduct, TSum> sum_product_sumFun,
                Func<TSum, bool> sum_exitIfTrueFun)
        {
            return InnerProduct(array0, array1,
                                Math.Min(array0.LongLength, array1.LongLength),
                                item0_item1_productFun, seed, sum_product_sumFun, sum_exitIfTrueFun);
        }

        public static TProduct[] ProductArray<T0, T1, TProduct>(
                this T0[] array0, T1[] array1,
                Func<T0, T1, TProduct> item0_item1_productFun)
        {
            return ProductArray(array0, array1, Math.Min(array0.LongLength, array1.LongLength),
                                item0_item1_productFun);
        }

        public static TProduct[] ProductArray<T0, T1, TProduct>(
                this T0[] array0, T1[] array1, long count,
                Func<T0, T1, TProduct> item0_item1_productFun)
        {
            var result = new TProduct[count];
            for (long i = 0; i < count; i++)
                result[i] = item0_item1_productFun(array0[i], array1[i]);
            return result;
        }

        public static TProduct[] ProductArray<T0, T1, TProduct>(
                this T0[] array0, T1[] array1,
                Func<T0, T1, long, TProduct> item0_item1_index_productFun)
        {
            return ProductArray(array0, array1, Math.Min(array0.LongLength, array1.LongLength),
                                item0_item1_index_productFun);
        }

        public static TProduct[] ProductArray<T0, T1, TProduct>(
                this T0[] array0, T1[] array1, long count,
                Func<T0, T1, long, TProduct> item0_item1_index_productFun)
        {
            var result = new TProduct[count];
            for (long i = 0; i < count; i++)
                result[i] = item0_item1_index_productFun(array0[i], array1[i], i);
            return result;
        }

        public static bool AllEqual<T0, T1>(
                this T0[] array0, T1[] array1, Func<T0, T1, bool> item0_item1_equalFun)
        {
            var len = array0.LongLength;
            return len == array1.LongLength
                    && array0.InnerProduct(array1, len, item0_item1_equalFun,
                                           true, (s, p) => s && p, s => !s);
        }

        #endregion

        #region Generic Jagged Array Operations

        /// <summary>
        /// Use this instead of Clone() or Copy() in order to get a full copy of the jagged array back.
        /// </summary>
        public static T[][] JaggedCopy<T>(this T[][] array)
        {
            long count = array.LongLength;
            var result = new T[count][];
            for (long i = 0; i < count; i++) result[i] = array[i].Copy();
            return result;
        }

        public static T[][][] JaggedCopy<T>(this T[][][] array)
        {
            long count = array.LongLength;
            var result = new T[count][][];
            for (long i = 0; i < count; i++) result[i] = array[i].JaggedCopy();
            return result;
        }

        public static long FlatCount<T>(this T[][] array)
        {
            return array.LongSum(a => a.LongLength);
        }

        public static long FlatCount<T>(this T[][][] array)
        {
            return array.LongSum(a => a.LongSum(b => b.LongLength));
        }

        public static void FlatCopyTo<T>(this T[][] array, T[] target, long start)
        {
            for (long j = 0; j < array.LongLength; j++)
            {
                var aj = array[j];
                for (long i = 0; i < aj.LongLength; i++)
                    target[start++] = aj[i];
            }
        }

        public static void FlatCopyTo<T>(this T[][][] array, T[] target, long start)
        {
            for (long k = 0; k < array.LongLength; k++)
            {
                var ak = array[k];
                for (long j = 0; j < ak.LongLength; j++)
                {
                    var akj = ak[j];
                    for (long i = 0; i < akj.LongLength; i++)
                        target[start++] = akj[i];
                }
            }
        }

        public static T[] FlatCopy<T>(this T[][] array)
        {
            var result = new T[array.FlatCount()];
            array.FlatCopyTo(result, 0);
            return result;
        }

        public static T[] FlatCopy<T>(this T[][][] array)
        {
            var result = new T[array.FlatCount()];
            array.FlatCopyTo(result, 0);
            return result;
        }

        #endregion

        #region Mapped Copy Functions

        public static int ForwardMapAdd(
            this int[] forwardMap, int index, ref int forwardCount)
        {
            int newIndex = forwardMap[index];
            if (newIndex < 0) forwardMap[index] = newIndex = forwardCount++;
            return newIndex;
        }

        static public int ForwardMapAdd(
            this int[] forwardMap, int forwardCount,
            int[] indexArray, int indexCount)
        {
            for (int i = 0; i < indexCount; i++)
            {
                int index = indexArray[i];
                if (forwardMap[index] < 0)
                    forwardMap[index] = forwardCount++;
            }
            return forwardCount;
        }

        /// <summary>
        /// Copies from this array into the target array starting at the
        /// supplied offset and using the supplied forward map that
        /// specifies the new index for each element. The forward map
        /// may contain negative values for elements to be skipped.
        /// </summary>
        public static void ForwardMappedCopyTo<T>(
            this T[] sourceArray,
            T[] targetArray,
            int[] forwardMap,
            int offset)
        {
            for (int i = 0; i < forwardMap.Length; i++)
            {
                int ni = forwardMap[i];
                if (ni >= 0) targetArray[ni + offset] = sourceArray[i];
            }
        }

        /// <summary>
        /// Copies from this array into the target array starting at the
        /// supplied offset using the supplied backward map that contains
        /// the source index for each index of the target array.
        /// </summary>
        public static void BackMappedCopyTo<T>(
                this T[] source, T[] target, int[] backMap, int offset)
        {
            int count = backMap.Length;
            for (int i = 0; i < count; i++)
                target[i + offset] = source[backMap[i]];
        }

        public static void BackMappedGroupCopyTo<T>(
                this T[] source, int[] groupBackMap, int groupCount,
                int[] fia, T[] target, int start)
        {

            for (int gi = 0; gi < groupCount; gi++)
            {
                int ogi = groupBackMap[gi]; if (ogi < 0) continue;
                for (int ogei = fia[ogi], ogee = fia[ogi + 1]; ogei < ogee; ogei++)
                    target[start++] = source[ogei];
            }
        }

        public static void BackMappedGroupCopyTo(
                this int[] source, int[] groupBackMap, int groupCount,
                int[] fia, int[] elementForwardMap, int[] target, int start)
        {
            for (int gi = 0; gi < groupCount; gi++)
            {
                int ogi = groupBackMap[gi]; if (ogi < 0) continue;
                for (int ogei = fia[ogi], ogee = fia[ogi + 1]; ogei < ogee; ogei++)
                    target[start++] = elementForwardMap[source[ogei]];
            }
        }

        /// <summary>
        /// Copies from this array into the target array starting at the
        /// supplied offset using the supplied backward map that contains
        /// the source index for each index of the target array.
        /// </summary>
        public static void BackMappedCopyTo<T>(
                this T[] source, T[] target, long[] backMap, long offset)
        {
            long count = backMap.LongLength;
            for (long i = 0; i < count; i++)
                target[i + offset] = source[backMap[i]];
        }

        /// <summary>
        /// Returns a copy of this array by placing each element at a new
        /// position specified by the supplied forward map. The forward map
        /// may contain negative values for elements that should not be
        /// copied. The length of the result is computed to hold all forward
        /// mapped elements of the soruce array.
        /// </summary>
        public static T[] ForwardMappedCopy<T>(
                this T[] sourceArray, int[] forwardMap)
        {
            return sourceArray.ForwardMappedCopy(forwardMap, forwardMap.Max() + 1);
        }

        /// <summary>
        /// Returns a copy of this array by placing each element at a new
        /// position specified by the supplied forward map. The forward map
        /// may contain negative values for elements that should not be
        /// copied. The length of the result needs to be supplied as
        /// a parameter.
        /// </summary>
        public static T[] ForwardMappedCopy<T>(
                this T[] sourceArray, int[] forwardMap, int targetLength)
        {
            T[] targetArray = new T[targetLength];
            sourceArray.ForwardMappedCopyTo(targetArray, forwardMap, 0);
            return targetArray;
        }

        /// <summary>
        /// Returns a copy of this array that is created by copying the
        /// elements at the positions specified in the supplied backward
        /// map. An optional count argument is used for the size of the
        /// resulting array.
        /// </summary>
        public static T[] BackMappedCopy<T>(
                this T[] array, int[] backMap, int count = 0)
        {
            if (count <= 0) count = backMap.Length;
            var target = new T[count];
            count = Min(backMap.Length, count);
            for (int i = 0; i < count; i++) target[i] = array[backMap[i]];
            return target;
        }

        /// <summary>
        /// Returns a copy of this array that is created by copying the
        /// elements at the positions specified in the supplied backward
        /// map. An optional count argument is used for the size of the
        /// resulting array.
        /// </summary>
        public static T[] BackMappedCopySafe<T>(
                this T[] array, int[] backMap, T defaultValue, int count = 0)
        {
            if (count <= 0) count = backMap.Length;
            var target = new T[count].Set(defaultValue);
            count = Min(backMap.Length, count);
            for (int i = 0; i < count; i++)
            {
                var ti = backMap[i]; if (ti < 0) continue;
                target[i] = array[ti];
            }
            return target;
        }

        /// <summary>
        /// Returns a copy of this array that is created by copying the
        /// elements at the positions specified in the supplied backward
        /// map and applying a function to each element. An optional
        /// count argument is used for the size of the resulting array.
        /// </summary>
        public static Tr[] BackMappedCopy<T, Tr>(
                this T[] array, int[] backMap, Func<T, Tr> fun, int count = 0)
        {
            if (count <= 0) count = backMap.Length;
            var target = new Tr[count];
            count = Min(backMap.Length, count);
            for (int i = 0; i < count; i++) target[i] = fun(array[backMap[i]]);
            return target;
        }

        /// <summary>
        /// Returns a copy of this array that is created by copying the
        /// elements at the positions specified in the supplied backward
        /// map. An optional count argument is used for the size of the
        /// resulting array.
        /// </summary>
        public static T[] BackMappedCopy<T>(
                this T[] array, long[] backMap, long count = 0)
        {
            if (count <= 0) count = backMap.LongLength;
            var target = new T[count];
            count = Min(backMap.Length, count);
            for (long i = 0; i < count; i++) target[i] = array[backMap[i]];
            return target;
        }

        /// <summary>
        /// Returns a copy of this array that is created by copying the
        /// elements at the positions specified in the supplied backward
        /// map and applying a function to each element. An optional
        /// count argument is used for the size of the resulting array.
        /// </summary>
        public static Tr[] BackMappedCopy<T, Tr>(
                this T[] array, long[] backMap, Func<T, Tr> fun, long count = 0)
        {
            if (count <= 0) count = backMap.LongLength;
            var target = new Tr[count];
            count = Min(backMap.Length, count);
            for (long i = 0; i < count; i++) target[i] = fun(array[backMap[i]]);
            return target;
        }

        /// <summary>
        /// Returns a copy of this array that is created by copying the
        /// elements at the positions specified in the index array of the
        /// supplied backward map. If the index array is null only the
        /// backward map is used to compute the position. An optional count
        /// argument is used for the size of the resulting array.
        /// </summary>
        public static T[] BackMappedCopy<T>(
                this  T[] array, int[] indexArray, int[] backMap, int count = 0)
        {
            if (indexArray == null) return array.BackMappedCopy(backMap, count);
            if (count <= 0) count = backMap.Length;
            var target = new T[count];
            count = Min(backMap.Length, count);
            for (int i = 0; i < count; i++) target[i] = array[indexArray[backMap[i]]];
            return target;
        }

        /// <summary>
        /// Returns a copy of this array that is created by copying the
        /// elements at the positions specified in the index array of the
        /// supplied backward map and applying a function to each element.
        /// If the index array is null only the backward map is used to
        /// compute the position. An optional count argument is used for
        /// the size of the resulting array.
        /// </summary>
        public static Tr[] BackMappedCopy<T, Tr>(
                this T[] array, int[] indexArray, int[] backMap,
                Func<T, Tr> fun, int count = 0)
        {
            if (indexArray == null) return array.BackMappedCopy(backMap, fun, count);
            if (count <= 0) count = backMap.Length;
            var target = new Tr[count];
            count = Min(backMap.Length, count);
            for (int i = 0; i < count; i++) target[i] = fun(array[indexArray[backMap[i]]]);
            return target;
        }

        /// <summary>
        /// Copies all source arrays into this array, using the supplied
        /// array of forward maps.
        /// </summary>
        /// <returns>targetArray</returns>
        public static T[] ForwardMappedCopyFrom<T>(
                this T[] targetArray,
                T[][] sourceArrays,
                int[][] forwardMaps)
        {
            var offset = 0;
            for (int i = 0; i < sourceArrays.Count(); i++)
            {
                sourceArrays[i].ForwardMappedCopyTo(
                                    targetArray, forwardMaps[i], offset);
                offset += forwardMaps[i].Max() + 1;
            }
            return targetArray;
        }

        /// <summary>
        /// copies all srcArrays into this array
        /// indexed by an inversed indexMap
        /// returns itself
        /// </summary>
        public static T[] BackMappedCopyFrom<T>(
                this T[] targetArray,
                T[][] sourceArrays,
                int[][] backwardMaps)
        {
            int offset = 0;
            int count = sourceArrays.Length;
            for (int i = 0; i < count; i++)
            {
                sourceArrays[i].BackMappedCopyTo(
                                    targetArray, backwardMaps[i], offset);
                offset += backwardMaps[i].Length;
            }
            return targetArray;
        }

        /// <summary>
        /// copies this indexArray to destinationIndexArray
        /// over iaIndexMap at position diaOffset
        /// and returns aIndexMap for
        /// copying the arrays that are indexed by sourceIndexArray
        /// !! destinationIndexArray has to be initialized with -1
        /// </summary>
        public static int[] ForwardMappedCopyIndexArrayTo(
                this int[] sourceArray,
                int[] targetArray,
                int[] forwardMap,
                int targetOffset)
        {
            int count = forwardMap.Max() + 1;
            int targetIndexOffset = targetArray.Max() + 1;

            // creating destinationIndexArray, still indexing sourceArray
            sourceArray.ForwardMappedCopyTo(
                            targetArray, forwardMap, targetOffset);

            var indexMap = new int[sourceArray.Max() + 1].Set(-1);
            var indexCount = 0;
            for (var ti = targetOffset; ti < targetOffset + count; ti++)
            {
                var oldIndex = targetArray[ti];
                // creating indexMap
                var newIndex = indexMap[oldIndex];
                if (newIndex == -1)
                    indexMap[oldIndex] = newIndex = indexCount++;
                // reindexing targetArray
                targetArray[ti] = newIndex + targetIndexOffset;
            }

            return indexMap;
        }

        /// <summary>
        /// Copies this index array to the target array over iaIndexMap at
        /// position diaOffset and returns aIndexMap for
        /// copying the arrays that are indexed by sourceIndexArray
        /// !! destinationIndexArray has to be initialized with -1
        /// </summary>
        public static int[] BackMappedCopyIndexArrayTo(
                this int[] sourceArray,
                int[] targetArray,
                int[] backwardMap,
                int targetOffest)
        {
            int count = backwardMap.Count();
            int targetIndexOffset = targetArray.Max() + 1;

            // creating targetArray, still indexing sourceArray
            sourceArray.BackMappedCopyTo(
                            targetArray, backwardMap, targetOffest);

            var indexMap = new int[sourceArray.Max() + 1].Set(-1);
            var indexCount = 0;
            for (var ti = targetOffest; ti < targetOffest + count; ti++)
            {
                var oldIndex = targetArray[ti];
                // creating indexMap
                var newIndex = indexMap[oldIndex];
                if (newIndex == -1)
                    indexMap[oldIndex] = newIndex = indexCount++;
                // reindexing targetArray
                targetArray[ti] = newIndex + targetIndexOffset;
            }

            return indexMap;
        }

        /// <summary>
        /// copies from this indexArray to destinationIndexArray
        /// over iaIndexMap
        /// and returns aIndexMap for
        /// copying the arrays that are indexed by sourceIndexArray
        /// </summary>
        public static int[] ForwardMappedCopyIndexArrayTo(
                this int[] sourceArray,
                out int[] targetArray,
                int[] forwardMap)
        {
            targetArray = new int[forwardMap.Max() + 1].Set(-1);
            return sourceArray.ForwardMappedCopyIndexArrayTo(
                targetArray, forwardMap, 0);
        }

        /// <summary>
        /// copies from this indexArray to destinationIndexArray
        /// over iaIndexMap
        /// and returns aIndexMap for
        /// copying the arrays that are indexed by sourceIndexArray
        /// </summary>
        public static int[] BackMappedCopyIndexArrayTo(
                this int[] sourceArray,
                out int[] targetArray,
                int[] backwardMap)
        {
            targetArray = new int[backwardMap.Count()].Set(-1);
            return sourceArray.BackMappedCopyIndexArrayTo(
                targetArray, backwardMap, 0);
        }

        /// <summary>
        /// copies all srcIndexArrays to this indexArray
        /// over iaIndexMaps
        /// and returns aIndexMaps for
        /// copying the arrays that are indexed by srcIndexArrays
        /// </summary>
        public static int[][] ForwardMappedCopyFromIndexArrays(
                this int[] targetArray,
                int[][] sourceArrays,
                int[][] forwardMaps)
        {
            var count = sourceArrays.Count();
            var indexMaps = new int[count][];
            var offset = 0;
            for (int i = 0; i < count; i++)
            {
                indexMaps[i] = sourceArrays[i]
                                .ForwardMappedCopyIndexArrayTo(
                                    targetArray, forwardMaps[i], offset);
                offset += forwardMaps[i].Max() + 1;
            }
            return indexMaps;
        }

        /// <summary>
        /// copies all srcIndexArrays to this indexArray
        /// over iaIndexMaps
        /// and returns aIndexMaps for
        /// copying the arrays that are indexed by srcIndexArrays
        /// </summary>
        public static int[][] BackMappedCopyFromIndexArrays(
                this int[] targetArray,
                int[][] sourceArrays,
                int[][] backwardMaps)
        {
            var count = sourceArrays.Count();
            var indexMaps = new int[count][];
            var offset = 0;
            for (int i = 0; i < count; i++)
            {
                indexMaps[i] = sourceArrays[i]
                                .BackMappedCopyIndexArrayTo(
                                    targetArray, backwardMaps[i], offset);
                offset += backwardMaps[i].Count();
            }
            return indexMaps;
        }

        public static void ReverseGroups<T>(
                this T[] array, int[] groupFirstIndices, int groupCount,
                Func<int, bool> reverseMap)
        {
            for (int gvi = groupFirstIndices[0], fi = 0; fi < groupCount; fi++)
            {
                int gve = groupFirstIndices[fi + 1];
                if (reverseMap(fi))
                    for (int rfvi = gve - 1; gvi < rfvi; gvi++, rfvi--)
                        array.Swap(gvi, rfvi);
                gvi = gve;
            }
        }


        /// <summary>
        /// Return an array with reversed groups. The specified first group
        /// indices array defines which consecutive elements constitute a
        /// group, and the reverse map function specifies wich of these
        /// groups should actually be reversed.
        /// </summary>
        public static T[] GroupReversedCopy<T>(
                this T[] array, int[] groupFirstIndices, int groupCount,
                Func<int, bool> reverseMap)
        {
            var result = new T[array.Length];
            for (int gvi = groupFirstIndices[0], gi = 0; gi < groupCount; gi++)
            {
                int gve = groupFirstIndices[gi + 1];
                if (reverseMap(gi))
                    for (int rgvi = gve; gvi < gve; gvi++)
                        result[--rgvi] = array[gvi];
                else
                    for (; gvi < gve; gvi++) result[gvi] = array[gvi];
            }
            return result;
        }

        #endregion

        #region Creating Backward Maps

        /// <summary>
        /// returns the inverse of the given indexMap
        /// (be carefull with inversing inversedIndexMaps => can be to short!
        /// fix if needed: use inverseIndexMap(int[] indexMap, int size) instead)
        /// </summary>
        public static int[] CreateBackMap(
                this int[] forwardMap)
        {
            return CreateBackMap(forwardMap, forwardMap.Max() + 1);
        }

        public static int[] CreateBackToFirstMap(
                this int[] forwardMap, int resultLength)
        {
            var backMap = new int[resultLength].Set(-1);
            var count = forwardMap.Length;
            for (int i = 0; i < count; i++)
            {
                int ni = forwardMap[i]; if (ni < 0) continue;
                if (backMap[ni] < 0) backMap[ni] = i;
            }
            return backMap;
        }

        /// <summary>
        /// returns the backward map in the given size of the given forward map
        /// </summary>
        public static int[] CreateBackMap(
                this int[] forwardMap, int resultLength)
        {
            var backMap = new int[resultLength].Set(-1);
            var count = forwardMap.Length;
            for (int i = 0; i < count; i++)
            {
                int ni = forwardMap[i]; if (ni < 0) continue;
                backMap[ni] = i;
            }
            return backMap;
        }

        public static int[] CreateBackMap(
                this int[] forwardMap, int resultLength, int offset)
        {
            var backMap = new int[resultLength];
            var count = forwardMap.Length;
            for (int i = 0; i < count; i++)
            {
                int ni = forwardMap[i]; if (ni < 0) continue;
                backMap[ni - offset] = i;
            }
            return backMap;
        }

        #endregion

        #region Integration

        /// <summary>
        /// Converts an array that contains the number of elements
        /// at each index into an array that holds the indices of
        /// the first element if the elements are stored in
        /// consecutive order. Returns the sum of all elements.
        /// Using double precision during integration.
        /// </summary>
        public static double Integrate(this float[] array, double sum = 0)
        {
            for (long i = 0; i < array.LongLength; i++)
            {
                var delta = array[i]; array[i] = (float)sum; sum += delta;
            }
            return sum;
        }

        /// <summary>
        /// Converts an array that contains the number of elements
        /// at each index into an array that holds the indices of
        /// the first element if the elements are stored in
        /// consecutive order. Returns the sum of all elements.
        /// Using single precision during integration.
        /// </summary>
        public static float Integrate(this float[] array, float sum = 0)
        {
            for (long i = 0; i < array.LongLength; i++)
            {
                var delta = array[i]; array[i] = sum; sum += delta;
            }
            return sum;
        }

        /// <summary>
        /// Converts an array that contains the number of elements
        /// at each index into an array that holds the indices of
        /// the first element if the elements are stored in
        /// consecutive order. Returns the sum of all elements.
        /// </summary>
        public static int Integrate(this int[] array, int sum = 0)
        {
            for (long i = 0; i < array.LongLength; i++)
            {
                var delta = array[i]; array[i] = sum; sum += delta;
            }
            return sum;
        }

        /// <summary>
        /// Converts an array that contains the number of elements
        /// at each index into an array that holds the indices of
        /// the first element if the elements are stored in
        /// consecutive order. Returns the sum of all elements.
        /// </summary>
        public static long Integrate(this long[] array, long sum = 0)
        {
            for (long i = 0; i < array.LongLength; i++)
            {
                var delta = array[i]; array[i] = sum; sum += delta;
            }
            return sum;
        }

        /// <summary>
        /// Integrates the array and returns the sum. Note that the
        /// value of the starting element will be the supplied initial
        /// sum value after the operation.
        /// </summary>
        public static double Integrate(this double[] array, double sum = 0.0)
        {
            for (long i = 0; i < array.LongLength; i++)
            {
                var value = array[i]; array[i] = sum; sum += value;
            }
            return sum;
        }

        /// <summary>
        /// Creates an array that contains the integrated sum up to each element of input array. 
        /// The length of the integrated array is +1 of the input array and contains the total
        /// sum in the last element.
        /// Using double precision during integration.
        /// </summary>
        public static float[] Integrated(this float[] array, double sum = 0)
        {
            var integrated = new float[array.Length + 1];
            integrated[0] = (float)sum;
            for (long i = 0; i < array.LongLength;)
            {
                sum += array[i];
                integrated[++i] = (float)sum;
            }
            return integrated;
        }

        /// <summary>
        /// Creates an array that contains the integrated sum up to each element of input array. 
        /// The length of the integrated array is +1 of the input array and contains the total
        /// sum in the last element.
        /// Using single precision during integration.
        /// </summary>
        public static float[] Integrated(this float[] array, float sum = 0)
        {
            var integrated = new float[array.Length + 1];
            integrated[0] = sum;
            for (long i = 0; i < array.LongLength;)
            {
                sum += array[i];
                integrated[++i] = sum;
            }
            return integrated;
        }

        /// <summary>
        /// Creates an array that contains the integrated sum up to each element of input array. 
        /// The length of the integrated array is +1 of the input array and contains the total
        /// sum in the last element.
        /// </summary>
        public static int[] Integrated(this int[] array, int sum = 0)
        {
            var integrated = new int[array.Length + 1];
            integrated[0] = sum;
            for (long i = 0; i < array.LongLength;)
            {
                sum += array[i];
                integrated[++i] = sum;
            }
            return integrated;
        }

        /// <summary>
        /// Creates an array that contains the integrated sum up to each element of input array. 
        /// The length of the integrated array is +1 of the input array and contains the total
        /// sum in the last element.
        /// </summary>
        public static long[] Integrated(this long[] array, long sum = 0)
        {
            var integrated = new long[array.Length + 1];
            integrated[0] = sum;
            for (long i = 0; i < array.LongLength;)
            {
                sum += array[i];
                integrated[++i] = sum;
            }
            return integrated;
        }

        /// <summary>
        /// Creates an array that contains the integrated sum up to each element of input array. 
        /// The length of the integrated array is +1 of the input array and contains the total
        /// sum in the last element.
        /// </summary>
        public static double[] Integrated(this double[] array, double sum = 0)
        {
            var integrated = new double[array.Length + 1];
            integrated[0] = sum;
            for (long i = 0; i < array.LongLength;)
            {
                sum += array[i];
                integrated[++i] = sum;
            }
            return integrated;
        }

        #endregion

        #region Dense Forward Maps

        /// <summary>
        /// Count the indices in an index array that are actually used.
        /// </summary>
        static public int DenseCount(
                this int[] indexArray, int indexCount, int maxIndex)
        {
            int[] forwardMap = new int[maxIndex].Set(-1);
            return forwardMap.ForwardMapAdd(0, indexArray, indexCount);
        }

        /// <summary>
        /// Create a forward map from an index array that contains new
        /// indices in such a way, that no index is unused.
        /// </summary>
        static public int[] DenseForwardMap(
                this int[] indexArray, int indexCount,
                int maxIndex, out int denseCount)
        {
            int[] forwardMap = new int[maxIndex].Set(-1);
            denseCount = forwardMap.ForwardMapAdd(
                            0, indexArray, indexCount);
            return forwardMap;
        }

        public static int[] DenseForwardMap(
                this int[] indexArray,
                int[] groupSelectionArray,
                int groupSize,
                out int denseCount)
        {
            int count = 0;
            int[] forwardMap = new int[indexArray.Length].Set(-1);
            for (int gi = 0; gi < groupSelectionArray.Length; gi++)
            {
                int g = groupSelectionArray[gi] * groupSize;
                for (int i = g; i < g + groupSize; i++)
                {
                    int index = indexArray[i];
                    if (forwardMap[index] < 0)
                        forwardMap[index] = count++;
                }
            }
            denseCount = count;
            return forwardMap;
        }

        #endregion

        #region Comparable Array

        public static T[] MergeSorted<T>(this T[] a0, T[] a1)
            where T : IComparable<T>
        {
            int count0 = a0.Length;
            int count1 = a1.Length;
            var a = new T[count0 + count1];
            int i = 0, i0 = 0, i1 = 0;
            while (i0 < count0 && i1 < count1)
            {
                if (a0[i0].CompareTo(a1[i1]) < 0)
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < count0) a[i++] = a0[i0++];
            while (i1 < count1) a[i++] = a1[i1++];
            return a;
        }

        public static int IndexOfMin<T>(this T[] a)
            where T : IComparable<T>
        {
            int index = 0;
            T min = a[0];
            for (int i = 1; i < a.Length; i++)
                if (a[i].CompareTo(min) < 0) { min = a[i]; index = i; }
            return index;
        }

        public static int IndexOfMin<T>(this T[] a, int start, int count)
            where T : IComparable<T>
        {
            int index = start;
            T min = a[start];
            for (int i = start + 1, e = start + count; i < e; i++)
                if (a[i].CompareTo(min) < 0) { min = a[i]; index = i; }
            return index;
        }

        public static int IndexOfMax<T>(this T[] a)
            where T : IComparable<T>
        {
            int index = 0;
            T max = a[0];
            for (int i = 1; i < a.Length; i++)
                if (a[i].CompareTo(max) > 0) { max = a[i]; index = i; }
            return index;
        }

        public static int IndexOfMax<T>(this T[] a, int start, int count)
            where T : IComparable<T>
        {
            int index = start;
            T max = a[start];
            for (int i = start + 1, e = start + count; i < e; i++)
                if (a[i].CompareTo(max) > 0) { max = a[i]; index = i; }
            return index;
        }

        public static int IndexOfMin<T>(this T[] a, Func<T, T, int> compare)
        {
            int index = 0;
            T min = a[0];
            for (int i = 1; i < a.Length; i++)
                if (compare(a[i], min) < 0) { min = a[i]; index = i; }
            return index;
        }

        public static int IndexOfMax<T>(this T[] a, Func<T, T, int> compare)
        {
            int index = 0;
            T max = a[0];
            for (int i = 1; i < a.Length; i++)
                if (compare(a[i], max) > 0) { max = a[i]; index = i; }
            return index;
        }

        public static long LongIndexOfMin<T>(this T[] a)
            where T : IComparable<T>
        {
            long index = 0;
            T min = a[0];
            for (long i = 1; i < a.LongLength; i++)
                if (a[i].CompareTo(min) < 0) { min = a[i]; index = i; }
            return index;
        }

        public static long LongIndexOfMax<T>(this T[] a)
            where T : IComparable<T>
        {
            long index = 0;
            T max = a[0];
            for (long i = 1; i < a.LongLength; i++)
                if (a[i].CompareTo(max) > 0) { max = a[i]; index = i; }
            return index;
        }

        public static long LongIndexOfMin<T>(this T[] a, Func<T, T, int> compare)
        {
            long index = 0;
            T min = a[0];
            for (long i = 1; i < a.LongLength; i++)
                if (compare(a[i], min) < 0) { min = a[i]; index = i; }
            return index;
        }

        public static long LongIndexOfMax<T>(this T[] a, Func<T, T, int> compare)
        {
            long index = 0;
            T max = a[0];
            for (long i = 1; i < a.LongLength; i++)
                if (compare(a[i], max) > 0) { max = a[i]; index = i; }
            return index;
        }

        #endregion

        #region Multi-Dimensional Arrays

        /// <summary>
        /// Set all elements to a function of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[,] SetByIndex<T>(this T[,] self, Func<int, int, T> fun)
        {
            int count0 = self.GetLength(0);
            int count1 = self.GetLength(1);
            for (int i0 = 0; i0 < count0; i0++)
                for (int i1 = 0; i1 < count1; i1++)
                    self[i0, i1] = fun(i0, i1);
            return self;
        }

        /// <summary>
        /// Set all elements to a function of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[,] SetByIndexLong<T>(this T[,] self, Func<long, long, T> fun)
        {
            long count0 = self.GetLongLength(0);
            long count1 = self.GetLongLength(1);
            for (long i0 = 0; i0 < count0; i0++)
                for (long i1 = 0; i1 < count1; i1++)
                    self[i0, i1] = fun(i0, i1);
            return self;
        }

        /// <summary>
        /// Set all elements to a function of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[, ,] SetByIndex<T>(this T[, ,] self, Func<int, int, int, T> fun)
        {
            int count0 = self.GetLength(0);
            int count1 = self.GetLength(1);
            int count2 = self.GetLength(2);
            for (int i0 = 0; i0 < count0; i0++)
                for (int i1 = 0; i1 < count1; i1++)
                    for (int i2 = 0; i2 < count2; i2++)
                        self[i0, i1, i2] = fun(i0, i1, i2);
            return self;
        }

        /// <summary>
        /// Set all elements to a function of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static T[, ,] SetByIndexLong<T>(this T[, ,] self, Func<long, long, long, T> fun)
        {
            long count0 = self.GetLongLength(0);
            long count1 = self.GetLongLength(1);
            long count2 = self.GetLongLength(2);
            for (long i0 = 0; i0 < count0; i0++)
                for (long i1 = 0; i1 < count1; i1++)
                    for (long i2 = 0; i2 < count2; i2++)
                        self[i0, i1, i2] = fun(i0, i1, i2);
            return self;
        }

        #endregion

        #region Memcopy

        private static class Msvcrt
        {
            [DllImport("msvcrt.dll")]
            public static extern int memcpy (IntPtr target, IntPtr src, UIntPtr size);
        }

        private static class Libc
        {
            [DllImport("libc")]
            public static extern int memcpy (IntPtr target, IntPtr src, UIntPtr size);
        }

        /// <summary>
        /// Copies the specified part of an array to the target-pointer.
        /// NOTE: May cause AccessViolationException if the target-pointer
        ///       is not sufficiently allocated.
        /// </summary>
        /// <param name="input">The input Array</param>
        /// <param name="offset">The start index for copying</param>
        /// <param name="length">The number of elements to copy</param>
        /// <param name="target">The target pointer</param>
        public static void CopyTo(this Array input, int offset, int length, IntPtr target)
        {
            var gc = GCHandle.Alloc (input, GCHandleType.Pinned);
            var type = input.GetType().GetElementType();
            var typeSize = Marshal.SizeOf (type);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
                Libc.memcpy (target, gc.AddrOfPinnedObject () + offset * typeSize, (UIntPtr)(length * typeSize));
            else
                Msvcrt.memcpy (target, gc.AddrOfPinnedObject () + offset * typeSize, (UIntPtr)(length * typeSize));

            gc.Free ();
        }

        public static void CopyTo(this Array input, int length, IntPtr target)
        {
            CopyTo(input, 0, length, target);
        }

        public static void CopyTo(this Array input, IntPtr target)
        {
            CopyTo(input, 0, input.Length, target);
        }

        public static void CopyTo(this IntPtr input, Array target, int offset, int length)
        {
            var gc = GCHandle.Alloc (target, GCHandleType.Pinned);
            var type = target.GetType().GetElementType();
            var typeSize = Marshal.SizeOf (type);

            if (Environment.OSVersion.Platform == PlatformID.Unix)
                Libc.memcpy (gc.AddrOfPinnedObject () + offset * typeSize, input, (UIntPtr)(length * typeSize));
            else
                Msvcrt.memcpy (gc.AddrOfPinnedObject () + offset * typeSize, input, (UIntPtr)(length * typeSize));

            gc.Free ();
        }

        public static void CopyTo(this IntPtr input, Array target, int length)
        {
            CopyTo(input, target, 0, length);
        }

        public static void CopyTo(this IntPtr input, Array target)
        {
            CopyTo(input, target, target.Length);
        }


        public static void CopyTo(this IntPtr input, IntPtr target, int size)
        {
            if (Environment.OSVersion.Platform == PlatformID.Unix)
                Libc.memcpy(target, input, (UIntPtr)size);
            else
                Msvcrt.memcpy(target, input, (UIntPtr)size);

        }

        #endregion

        #region Hashes

        /// <summary>
        /// Computes the MD5 hash of the data array.
        /// </summary>
        /// <returns>128bit/16byte data hash</returns>
        public static byte[] ComputeMD5Hash(this byte[] data)
        {
            using (var md5 = SHA1.Create())
                return md5.ComputeHash(data);
        }

        /// <summary>
        /// Computes the MD5 hash of the data array.
        /// </summary>
        /// <returns>128bit/16byte data hash</returns>
        public static byte[] ComputeMD5Hash(this Array data)
        {
            byte[] hash = null;
            data.UnsafeCoercedApply<byte>(array => hash = array.ComputeMD5Hash());
            return hash;
        }

        /// <summary>
        /// Computes the MD5 hash of the given string.
        /// <returns>128bit/16byte data hash</returns>
        /// </summary>
        public static byte[] ComputeMD5Hash(this string s)
        {
            return Encoding.Unicode.GetBytes(s).ComputeMD5Hash();
        }

        /// <summary>
        /// Computes the SHA1 hash of the data array.
        /// </summary>
        /// <returns>160bit/20byte data hash</returns>
        public static byte[] ComputeSHA1Hash(this byte[] data)
        {
            using (var sha1 = SHA1.Create())
                return sha1.ComputeHash(data);
        }

        /// <summary>
        /// Computes the SHA1 hash of the data array.
        /// </summary>
        /// <returns>160bit/20byte data hash</returns>
        public static byte[] ComputeSHA1Hash(this Array data)
        {
            byte[] hash = null;
            data.UnsafeCoercedApply<byte>(array => hash = array.ComputeSHA1Hash());
            return hash;
        }

        /// <summary>
        /// Computes the SHA1 hash of the given string.
        /// <returns>160bit/20byte data hash</returns>
        /// </summary>
        public static byte[] ComputeSHA1Hash(this string s)
        {
            return Encoding.Unicode.GetBytes(s).ComputeSHA1Hash();
        }

        /// <summary>
        /// Computes the SHA256 hash of the data array.
        /// </summary>
        /// <returns>256bit/32byte data hash</returns>
        public static byte[] ComputeSHA256Hash(this byte[] data)
        {
            using (var sha256 = SHA256.Create())
                return sha256.ComputeHash(data);
        }

        /// <summary>
        /// Computes the SHA256 hash of the data array.
        /// </summary>
        /// <returns>256bit/32byte data hash</returns>
        public static byte[] ComputeSHA256Hash(this Array data)
        {
            byte[] hash = null;
            data.UnsafeCoercedApply<byte>(array => hash = array.ComputeSHA256Hash());
            return hash;
        }

        /// <summary>
        /// Computes the SHA256 hash of the given string.
        /// </summary>
        /// <returns>256bit/32byte data hash</returns>
        public static byte[] ComputeSHA256Hash(this string s)
        {
            return Encoding.Unicode.GetBytes(s).ComputeSHA256Hash();
        }

        /// <summary>
        /// Computes the SHA512 hash of the data array.
        /// </summary>
        /// <returns>512bit/64byte data hash</returns>
        public static byte[] ComputeSHA512Hash(this byte[] data)
        {
            using (var sha512 = SHA512.Create())
                return sha512.ComputeHash(data);
        }

        /// <summary>
        /// Computes the SHA512 hash of the data array.
        /// </summary>
        /// <returns>512bit/64byte data hash</returns>
        public static byte[] ComputeSHA512Hash(this Array data)
        {
            byte[] hash = null;
            data.UnsafeCoercedApply<byte>(array => hash = array.ComputeSHA512Hash());
            return hash;
        }

        /// <summary>
        /// Computes the SHA512 hash of the given string.
        /// </summary>
        /// <returns>512bit/64byte data hash</returns>
        public static byte[] ComputeSHA512Hash(this string s)
        {
            return Encoding.Unicode.GetBytes(s).ComputeSHA512Hash();
        }

        /// <summary>
        /// Computes a checksum of the data array using the Adler-32 algorithm (<see cref="Adler32"/>).
        /// </summary>
        public static uint ComputeAdler32Checksum(this byte[] data)
        {
            var a = new Adler32();
            a.Update(data);
            return a.Checksum;
        }

        /// <summary>
        /// Computes a checksum of the data array using the Adler-32 algorithm (<see cref="Adler32"/>).
        /// </summary>
        public static uint ComputeAdler32Checksum(this Array data)
        {
            var a = new Adler32();
            data.UnsafeCoercedApply<byte>(array => a.Update(array));
            return a.Checksum;            
        }

        /// <summary>
        /// Computes a checksum of the given string using the Adler-32 algorithm (<see cref="Adler32"/>).
        /// </summary>
        public static uint ComputeAdler32Checksum(this string s)
        {
            return Encoding.Unicode.GetBytes(s).ComputeAdler32Checksum();
        }

        #endregion
    }
}