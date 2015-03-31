using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

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
			long end = start + System.Math.Min(count, self.LongLength - start);
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
			int end = System.Math.Min(start + count, self.Length);
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
			long end = System.Math.Min(start + count, self.LongLength);
			for (long i = start; i < end; i++) self[i] = index_fun(i);
			return self;
		}

		/// <summary>
		/// Set all elements to the elements of the supplied array.
		/// </summary>
		public static T[] Set<T>(this T[] self, T[] array)
		{
			long len = System.Math.Min(self.LongLength, array.LongLength);
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
			var len = System.Math.Min(count, array.LongLength);
			for (var i = 0L; i < len; i++) result[i] = array[i];
			return result;
		}

		/// <summary>
		/// Create a copy of the specified length starting at the specified
		/// start. If the copy is longer it is filled with default elements.
		/// </summary>
		public static T[] Copy<T>(this T[] array, long start, long count)
		{
			var result = new T[count];
			var len = System.Math.Min(count, array.LongLength - start);
			for (var i = 0L; i < len; i++) result[i] = array[i + start];
			return result;
		}

		/// <summary>
		/// Create a copy with the elements piped through a function.
		/// </summary>
		public static Tr[] Copy<T, Tr>(this T[] array, Func<T, Tr> element_fun)
		{
			var len = array.LongLength;
			var result = new Tr[len];
			for (var i = 0L; i < len; i++) result[i] = element_fun(array[i]);
			return result;
		}

		/// <summary>
		/// Create a copy with the elements piped through a function.
		/// The function gets the index of the element as a second argument.
		/// </summary>
		public static Tr[] Copy<T, Tr>(this T[] array, Func<T, long, Tr> element_index_fun)
		{
			var len = array.LongLength;
			var result = new Tr[len];
			for (var i = 0L; i < len; i++) result[i] = element_index_fun(array[i], i);
			return result;
		}

		/// <summary>
		/// Create a copy of count elements with the elements piped through a
		/// function. count may be longer than the array, in this case the
		/// result array has default elements at the end.
		/// </summary>
		public static Tr[] Copy<T, Tr>(
				this T[] array, long count, Func<T, Tr> element_fun)
		{
			var result = new Tr[count];
			var len = System.Math.Min(count, array.LongLength);
			for (var i = 0L; i < len; i++) result[i] = element_fun(array[i]);
			return result;
		}

		/// <summary>
		/// Create a copy of count elements with the elements piped through a
		/// function. count may be longer than the array, in this case the
		/// result array has default elements at the end.
		/// The function gets the index of the element as a second argument.
		/// </summary>
		public static Tr[] Copy<T, Tr>(
				this T[] array, long count, Func<T, long, Tr> element_index_fun)
		{
			var result = new Tr[count];
			var len = System.Math.Min(count, array.LongLength);
			for (var i = 0L; i < len; i++) result[i] = element_index_fun(array[i], i);
			return result;
		}

		/// <summary>
		/// Create a copy of specified length starting at the specified
		/// offset with the elements piped through a function.
		/// </summary>
		public static Tr[] Copy<T, Tr>(
				this T[] array, long start, long count, Func<T, Tr> element_fun)
		{
			var result = new Tr[count];
			var len = System.Math.Min(count, array.LongLength - start);
			for (var i = 0L; i < len; i++) result[i] = element_fun(array[start + i]);
			return result;
		}

		/// <summary>
		/// Create a copy of specified length starting at the specified
		/// offset with the elements piped through a function.
		/// The function gets the target index of the element as a second
		/// argument.
		/// </summary>
		public static Tr[] Copy<T, Tr>(
				this T[] array, long start, long count, Func<T, long, Tr> element_index_fun)
		{
			var result = new Tr[count];
			var len = System.Math.Min(count, array.LongLength - start);
			for (var i = 0L; i < len; i++) result[i] = element_index_fun(array[start + i], i);
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
		public static void CopyTo<T>(this T[] array, long start, long count, T[] target, long targetStart)
		{
			for (var i = 0L; i < count; i++)
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

		public static List<Tr> CopyToList<T, Tr>(this T[] array, Func<T, Tr> fun)
		{
			var count = array.Length;
			var result = new List<Tr>(count);
			for (int i = 0; i < count; i++) result.Add(fun(array[i]));
			return result;
		}

		public static List<Tr> ToList<T, Tr>(this T[] array, Func<T, Tr> fun)
		{
			var count = array.Length;
			var result = new List<Tr>(count);
			for (var i = 0; i < count; i++) result.Add(fun(array[i]));
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
		/// elements to omit is supplied as a parameter and defaults to
		/// 1.
		/// </summary>
		public static IEnumerable<T> SkipLast<T>(
				this T[] array, long count = 1)
		{
			count = array.LongLength - count;
			for (long i = 0; i < count; i++) yield return array[i];
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
			var end = Fun.Min(start + count, self.LongLength);
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
			var end = Fun.Min(start + count, self.LongLength);
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

		public static T[] With<T>(this T[] array, long index, T item)
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

		public static T[] WithAdded<T>(this T[] array, T item)
			where T : class
		{
			if (array == null) return new T[] { item };
			var len = array.LongLength;
			for (long i = 0; i < len; i++) if (array[i] == item) return array;
			var newArray = new T[len + 1];
			for (long i = 0; i < len; i++) newArray[i] = array[i];
			newArray[len] = item;
			return newArray;
		}

		public static T[] WithRemoved<T>(this T[] array, T item)
			where T : class
		{
			if (array == null) return array;
			var len = array.LongLength;
			if (len == 1)
			{
				if (array[0] == item) return null;
			}
			else
			{
				for (long i = 0; i < len; i++)
					if (array[i] == item)
					{
						var newArray = new T[len - 1];
						for (long j = 0; j < i; j++) newArray[j] = array[j];
						for (long j = i; j < len - 1; j++) newArray[j] = array[j + 1];
						return newArray;
					}
			}
			return array;
		}


		#endregion

		#region Generic Array Functional Programming Standard Operations

		/// <summary>
		/// Performs an aggregation of all elements in an array with the
		/// supplied aggregation function starting from the left and with the
		/// initial supplied left sum, and returns the aggregated result.
		/// </summary>
		public static TSum FoldLeft<TVal, TSum>(
				this TVal[] array, TSum leftSum, Func<TSum, TVal, TSum> sum_val_addFun)
		{
			long count = array.LongLength;
			for (long i = 0; i < count; i++)
				leftSum = sum_val_addFun(leftSum, array[i]);
			return leftSum;
		}

		/// <summary>
		/// Performs an aggregation of all elements in an array with the
		/// supplied aggregation function starting from the right and with the
		/// initial supplied right sum, and returns the aggregated result.
		/// </summary>
		public static TSum FoldRight<TVal, TSum>(
				this TVal[] array, Func<TVal, TSum, TSum> val_sum_addFun, TSum rightSum)
		{
			long count = array.LongLength;
			for (long i = count - 1; i >= 0; i--)
				rightSum = val_sum_addFun(array[i], rightSum);
			return rightSum;
		}

		/// <summary>
		/// Performs an aggregation of the specified slice of elements in an
		/// array with the supplied aggregation function starting from the
		/// left and with the initial supplied left sum, and returns the
		/// aggregated result.
		/// </summary>
		public static TSum FoldLeft<TVal, TSum>(
				this TVal[] array, long start, long count,
				TSum leftSum, Func<TSum, TVal, TSum> sum_val_addFun)
		{
			for (long i = start, e = start + count; i < e; i++)
				leftSum = sum_val_addFun(leftSum, array[i]);
			return leftSum;
		}

		/// <summary>
		/// Performs an aggregation of the specified slice of elements in an
		/// array with the supplied aggregation function starting from the
		/// right and with the initial supplied right sum, and returns the
		/// aggregated result.
		/// </summary>
		public static TSum FoldRight<TVal, TSum>(
				this TVal[] array, long start, long count,
				Func<TVal, TSum, TSum> val_sum_addFun, TSum rightSum)
		{
			for (long i = start + count - 1; i >= start; i--)
				rightSum = val_sum_addFun(array[i], rightSum);
			return rightSum;
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
			if (count > Math.Min(array.LongLength, other.LongLength))
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
			count = Fun.Min(backMap.Length, count);
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
			count = Fun.Min(backMap.Length, count);
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
			count = Fun.Min(backMap.Length, count);
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
			count = Fun.Min(backMap.Length, count);
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
			count = Fun.Min(backMap.Length, count);
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
			count = Fun.Min(backMap.Length, count);
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
			count = Fun.Min(backMap.Length, count);
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

		public static int IndexOfNSmallest<T>(this T[] a, int n)
			where T : IComparable<T>
		{
			if (n == 0) return a.IndexOfMin();
			var p = a.CreatePermutationQuickMedianAscending(n);
			return p[n];
		}

		public static int IndexOfNLargest<T>(this T[] a, int n)
			where T : IComparable<T>
		{
			if (n == 0) return a.IndexOfMax();
			var p = a.CreatePermutationQuickMedianDescending(n);
			return p[n];
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

		#region Non-Generic Array

		public static Dictionary<Type, Func<Array, Array>> CopyFunMap =
			new Dictionary<Type, Func<Array, Array>>
			{
				{ typeof(byte[]), a => ((byte[])a).Copy() },
				{ typeof(sbyte[]), a => ((sbyte[])a).Copy() },
				{ typeof(ushort[]), a => ((ushort[])a).Copy() },
				{ typeof(short[]), a => ((short[])a).Copy() },
				{ typeof(uint[]), a => ((uint[])a).Copy() },
				{ typeof(int[]), a => ((int[])a).Copy() },
				{ typeof(ulong[]), a => ((ulong[])a).Copy() },
				{ typeof(long[]), a => ((long[])a).Copy() },
				{ typeof(float[]), a => ((float[])a).Copy() },
				{ typeof(double[]), a => ((double[])a).Copy() },
				{ typeof(C3b[]), a => ((C3b[])a).Copy() },
				{ typeof(C3us[]), a => ((C3us[])a).Copy() },
				{ typeof(C3ui[]), a => ((C3ui[])a).Copy() },
				{ typeof(C3f[]), a => ((C3f[])a).Copy() },
				{ typeof(C3d[]), a => ((C3d[])a).Copy() },
				{ typeof(C4b[]), a => ((C4b[])a).Copy() },
				{ typeof(C4us[]), a => ((C4us[])a).Copy() },
				{ typeof(C4ui[]), a => ((C4ui[])a).Copy() },
				{ typeof(C4f[]), a => ((C4f[])a).Copy() },
				{ typeof(C4d[]), a => ((C4d[])a).Copy() },
				{ typeof(V2i[]), a => ((V2i[])a).Copy() },
				{ typeof(V2l[]), a => ((V2l[])a).Copy() },
				{ typeof(V2f[]), a => ((V2f[])a).Copy() },
				{ typeof(V2d[]), a => ((V2d[])a).Copy() },
				{ typeof(V3i[]), a => ((V3i[])a).Copy() },
				{ typeof(V3l[]), a => ((V3l[])a).Copy() },
				{ typeof(V3f[]), a => ((V3f[])a).Copy() },
				{ typeof(V3d[]), a => ((V3d[])a).Copy() },
				{ typeof(V4i[]), a => ((V4i[])a).Copy() },
				{ typeof(V4l[]), a => ((V4l[])a).Copy() },
				{ typeof(V4f[]), a => ((V4f[])a).Copy() },
				{ typeof(V4d[]), a => ((V4d[])a).Copy() },
			};

		public static Array Copy(this Array array)
		{
			return CopyFunMap[array.GetType()](array);
		}

		public static Dictionary<Type, Func<Array, object, Array>> CopyFunFunMap =
			new Dictionary<Type, Func<Array, object, Array>>
			{
				{ typeof(byte[]), (a, f) => ((byte[])a).Copy((Func<byte,byte>)f) },
				{ typeof(sbyte[]), (a, f) => ((sbyte[])a).Copy((Func<sbyte,sbyte>)f) },
				{ typeof(ushort[]), (a, f) => ((ushort[])a).Copy((Func<ushort,ushort>)f) },
				{ typeof(short[]), (a, f) => ((short[])a).Copy((Func<short,short>)f) },
				{ typeof(uint[]), (a, f) => ((uint[])a).Copy((Func<uint,uint>)f) },
				{ typeof(int[]), (a, f) => ((int[])a).Copy((Func<int,int>)f) },
				{ typeof(ulong[]), (a, f) => ((ulong[])a).Copy((Func<ulong,ulong>)f) },
				{ typeof(long[]), (a, f) => ((long[])a).Copy((Func<long,long>)f) },
				{ typeof(float[]), (a, f) => ((float[])a).Copy((Func<float,float>)f) },
				{ typeof(double[]), (a, f) => ((double[])a).Copy((Func<double,double>)f) },
				{ typeof(C3b[]), (a, f) => ((C3b[])a).Copy((Func<C3b,C3b>)f) },
				{ typeof(C3us[]), (a, f) => ((C3us[])a).Copy((Func<C3us,C3us>)f) },
				{ typeof(C3ui[]), (a, f) => ((C3ui[])a).Copy((Func<C3ui,C3ui>)f) },
				{ typeof(C3f[]), (a, f) => ((C3f[])a).Copy((Func<C3f,C3f>)f) },
				{ typeof(C3d[]), (a, f) => ((C3d[])a).Copy((Func<C3d,C3d>)f) },
				{ typeof(C4b[]), (a, f) => ((C4b[])a).Copy((Func<C4b,C4b>)f) },
				{ typeof(C4us[]), (a, f) => ((C4us[])a).Copy((Func<C4us,C4us>)f) },
				{ typeof(C4ui[]), (a, f) => ((C4ui[])a).Copy((Func<C4ui,C4ui>)f) },
				{ typeof(C4f[]), (a, f) => ((C4f[])a).Copy((Func<C4f,C4f>)f) },
				{ typeof(C4d[]), (a, f) => ((C4d[])a).Copy((Func<C4d,C4d>)f) },
				{ typeof(V2i[]), (a, f) => ((V2i[])a).Copy((Func<V2i,V2i>)f) },
				{ typeof(V2l[]), (a, f) => ((V2l[])a).Copy((Func<V2l,V2l>)f) },
				{ typeof(V2f[]), (a, f) => ((V2f[])a).Copy((Func<V2f,V2f>)f) },
				{ typeof(V2d[]), (a, f) => ((V2d[])a).Copy((Func<V2d,V2d>)f) },
				{ typeof(V3i[]), (a, f) => ((V3i[])a).Copy((Func<V3i,V3i>)f) },
				{ typeof(V3l[]), (a, f) => ((V3l[])a).Copy((Func<V3l,V3l>)f) },
				{ typeof(V3f[]), (a, f) => ((V3f[])a).Copy((Func<V3f,V3f>)f) },
				{ typeof(V3d[]), (a, f) => ((V3d[])a).Copy((Func<V3d,V3d>)f) },
				{ typeof(V4i[]), (a, f) => ((V4i[])a).Copy((Func<V4i,V4i>)f) },
				{ typeof(V4l[]), (a, f) => ((V4l[])a).Copy((Func<V4l,V4l>)f) },
				{ typeof(V4f[]), (a, f) => ((V4f[])a).Copy((Func<V4f,V4f>)f) },
				{ typeof(V4d[]), (a, f) => ((V4d[])a).Copy((Func<V4d,V4d>)f) },
			};

		public static Array Copy<T>(this Array array,
									Func<T, T> funOfElementTypeToElementType,
									Func<Array, Array> defaultFun = null)
		{
			var arrayType = array.GetType();
			if (typeof(T) == arrayType.GetElementType())
				return CopyFunFunMap[arrayType](array, funOfElementTypeToElementType);
			else if (defaultFun != null)
				return defaultFun(array);
			else
				return CopyFunMap[arrayType](array);
		}

		public static Dictionary<Type, Func<Array, int, Array>> ResizedFunMap =
			new Dictionary<Type, Func<Array, int, Array>>
			{
				{ typeof(byte[]), (a, s) => ((byte[])a).Resized(s) },
				{ typeof(sbyte[]), (a, s) => ((sbyte[])a).Resized(s) },
				{ typeof(ushort[]), (a, s) => ((ushort[])a).Resized(s) },
				{ typeof(short[]), (a, s) => ((short[])a).Resized(s) },
				{ typeof(uint[]), (a, s) => ((uint[])a).Resized(s) },
				{ typeof(int[]), (a, s) => ((int[])a).Resized(s) },
				{ typeof(ulong[]), (a, s) => ((ulong[])a).Resized(s) },
				{ typeof(long[]), (a, s) => ((long[])a).Resized(s) },
				{ typeof(float[]), (a, s) => ((float[])a).Resized(s) },
				{ typeof(double[]), (a, s) => ((double[])a).Resized(s) },
				{ typeof(C3b[]), (a, s) => ((C3b[])a).Resized(s) },
				{ typeof(C3us[]), (a, s) => ((C3us[])a).Resized(s) },
				{ typeof(C3ui[]), (a, s) => ((C3ui[])a).Resized(s) },
				{ typeof(C3f[]), (a, s) => ((C3f[])a).Resized(s) },
				{ typeof(C3d[]), (a, s) => ((C3d[])a).Resized(s) },
				{ typeof(C4b[]), (a, s) => ((C4b[])a).Resized(s) },
				{ typeof(C4us[]), (a, s) => ((C4us[])a).Resized(s) },
				{ typeof(C4ui[]), (a, s) => ((C4ui[])a).Resized(s) },
				{ typeof(C4f[]), (a, s) => ((C4f[])a).Resized(s) },
				{ typeof(C4d[]), (a, s) => ((C4d[])a).Resized(s) },
				{ typeof(V2i[]), (a, s) => ((V2i[])a).Resized(s) },
				{ typeof(V2l[]), (a, s) => ((V2l[])a).Resized(s) },
				{ typeof(V2f[]), (a, s) => ((V2f[])a).Resized(s) },
				{ typeof(V2d[]), (a, s) => ((V2d[])a).Resized(s) },
				{ typeof(V3i[]), (a, s) => ((V3i[])a).Resized(s) },
				{ typeof(V3l[]), (a, s) => ((V3l[])a).Resized(s) },
				{ typeof(V3f[]), (a, s) => ((V3f[])a).Resized(s) },
				{ typeof(V3d[]), (a, s) => ((V3d[])a).Resized(s) },
				{ typeof(V4i[]), (a, s) => ((V4i[])a).Resized(s) },
				{ typeof(V4l[]), (a, s) => ((V4l[])a).Resized(s) },
				{ typeof(V4f[]), (a, s) => ((V4f[])a).Resized(s) },
				{ typeof(V4d[]), (a, s) => ((V4d[])a).Resized(s) },
			};

		public static Array Resized(this Array array, int size)
		{
			return ResizedFunMap[array.GetType()](array, size);
		}

		public static Dictionary<Type, Func<Array, int[], int, Array>>
			BackMappedCopyFunMap = new Dictionary<Type, Func<Array, int[], int, Array>>
			{
				{ typeof(byte[]), (a,m,c) => ((byte[])a).BackMappedCopy(m, c) },
				{ typeof(sbyte[]), (a,m,c) => ((sbyte[])a).BackMappedCopy(m, c) },
				{ typeof(short[]), (a,m,c) => ((short[])a).BackMappedCopy(m, c) },
				{ typeof(ushort[]), (a,m,c) => ((ushort[])a).BackMappedCopy(m, c) },
				{ typeof(int[]), (a,m,c) => ((int[])a).BackMappedCopy(m, c) },
				{ typeof(uint[]), (a,m,c) => ((uint[])a).BackMappedCopy(m, c) },
				{ typeof(long[]), (a,m,c) => ((long[])a).BackMappedCopy(m, c) },
				{ typeof(ulong[]), (a,m,c) => ((ulong[])a).BackMappedCopy(m, c) },
				{ typeof(float[]), (a,m,c) => ((float[])a).BackMappedCopy(m, c) },
				{ typeof(double[]), (a,m,c) => ((double[])a).BackMappedCopy(m, c) },
				{ typeof(C3b[]), (a,m,c) => ((C3b[])a).BackMappedCopy(m, c) },
				{ typeof(C3us[]), (a,m,c) => ((C3us[])a).BackMappedCopy(m, c) },
				{ typeof(C3ui[]), (a,m,c) => ((C3ui[])a).BackMappedCopy(m, c) },
				{ typeof(C3f[]), (a,m,c) => ((C3f[])a).BackMappedCopy(m, c) },
				{ typeof(C3d[]), (a,m,c) => ((C3d[])a).BackMappedCopy(m, c) },
				{ typeof(C4b[]), (a,m,c) => ((C4b[])a).BackMappedCopy(m, c) },
				{ typeof(C4us[]), (a,m,c) => ((C4us[])a).BackMappedCopy(m, c) },
				{ typeof(C4ui[]), (a,m,c) => ((C4ui[])a).BackMappedCopy(m, c) },
				{ typeof(C4f[]), (a,m,c) => ((C4f[])a).BackMappedCopy(m, c) },
				{ typeof(C4d[]), (a,m,c) => ((C4d[])a).BackMappedCopy(m, c) },
				{ typeof(V2i[]), (a,m,c) => ((V2i[])a).BackMappedCopy(m, c) },
				{ typeof(V2l[]), (a,m,c) => ((V2l[])a).BackMappedCopy(m, c) },
				{ typeof(V2f[]), (a,m,c) => ((V2f[])a).BackMappedCopy(m, c) },
				{ typeof(V2d[]), (a,m,c) => ((V2d[])a).BackMappedCopy(m, c) },
				{ typeof(V3i[]), (a,m,c) => ((V3i[])a).BackMappedCopy(m, c) },
				{ typeof(V3l[]), (a,m,c) => ((V3l[])a).BackMappedCopy(m, c) },
				{ typeof(V3f[]), (a,m,c) => ((V3f[])a).BackMappedCopy(m, c) },
				{ typeof(V3d[]), (a,m,c) => ((V3d[])a).BackMappedCopy(m, c) },
				{ typeof(V4i[]), (a,m,c) => ((V4i[])a).BackMappedCopy(m, c) },
				{ typeof(V4l[]), (a,m,c) => ((V4l[])a).BackMappedCopy(m, c) },
				{ typeof(V4f[]), (a,m,c) => ((V4f[])a).BackMappedCopy(m, c) },
				{ typeof(V4d[]), (a,m,c) => ((V4d[])a).BackMappedCopy(m, c) },
			};

		public static Array BackMappedCopy(
				this Array source, int[] backMap, int count = 0)
		{
			Func<Array, int[], int, Array> fun;
			if (BackMappedCopyFunMap.TryGetValue(source.GetType(), out fun))
				return fun(source, backMap, count);
			return null;
		}

		public static Dictionary<Type, Func<Array, int[], int[], int, Array>>
			BackMappedIndexedCopyFunMap = new Dictionary<Type, Func<Array, int[], int[], int, Array>>
			{
				{ typeof(byte[]), (a,i,m,c) => ((byte[])a).BackMappedCopy(i, m, c) },
				{ typeof(sbyte[]), (a,i,m,c) => ((sbyte[])a).BackMappedCopy(i, m, c) },
				{ typeof(short[]), (a,i,m,c) => ((short[])a).BackMappedCopy(i, m, c) },
				{ typeof(ushort[]), (a,i,m,c) => ((ushort[])a).BackMappedCopy(i, m, c) },
				{ typeof(int[]), (a,i,m,c) => ((int[])a).BackMappedCopy(i, m, c) },
				{ typeof(uint[]), (a,i,m,c) => ((uint[])a).BackMappedCopy(i, m, c) },
				{ typeof(long[]), (a,i,m,c) => ((long[])a).BackMappedCopy(i, m, c) },
				{ typeof(ulong[]), (a,i,m,c) => ((ulong[])a).BackMappedCopy(i, m, c) },
				{ typeof(float[]), (a,i,m,c) => ((float[])a).BackMappedCopy(i, m, c) },
				{ typeof(double[]), (a,i,m,c) => ((double[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3b[]), (a,i,m,c) => ((C3b[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3us[]), (a,i,m,c) => ((C3us[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3ui[]), (a,i,m,c) => ((C3ui[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3f[]), (a,i,m,c) => ((C3f[])a).BackMappedCopy(i, m, c) },
				{ typeof(C3d[]), (a,i,m,c) => ((C3d[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4b[]), (a,i,m,c) => ((C4b[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4us[]), (a,i,m,c) => ((C4us[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4ui[]), (a,i,m,c) => ((C4ui[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4f[]), (a,i,m,c) => ((C4f[])a).BackMappedCopy(i, m, c) },
				{ typeof(C4d[]), (a,i,m,c) => ((C4d[])a).BackMappedCopy(i, m, c) },
				{ typeof(V2i[]), (a,i,m,c) => ((V2i[])a).BackMappedCopy(i, m, c) },
				{ typeof(V2l[]), (a,i,m,c) => ((V2l[])a).BackMappedCopy(i, m, c) },
				{ typeof(V2f[]), (a,i,m,c) => ((V2f[])a).BackMappedCopy(i, m, c) },
				{ typeof(V2d[]), (a,i,m,c) => ((V2d[])a).BackMappedCopy(i, m, c) },
				{ typeof(V3i[]), (a,i,m,c) => ((V3i[])a).BackMappedCopy(i, m, c) },
				{ typeof(V3l[]), (a,i,m,c) => ((V3l[])a).BackMappedCopy(i, m, c) },
				{ typeof(V3f[]), (a,i,m,c) => ((V3f[])a).BackMappedCopy(i, m, c) },
				{ typeof(V3d[]), (a,i,m,c) => ((V3d[])a).BackMappedCopy(i, m, c) },
				{ typeof(V4i[]), (a,i,m,c) => ((V4i[])a).BackMappedCopy(i, m, c) },
				{ typeof(V4l[]), (a,i,m,c) => ((V4l[])a).BackMappedCopy(i, m, c) },
				{ typeof(V4f[]), (a,i,m,c) => ((V4f[])a).BackMappedCopy(i, m, c) },
				{ typeof(V4d[]), (a,i,m,c) => ((V4d[])a).BackMappedCopy(i, m, c) },
			};

		public static Array BackMappedCopy(
				this Array source, int[] indexArray, int[] backMap, int count = 0)
		{
			Func<Array, int[], int[], int, Array> fun;
			if (BackMappedIndexedCopyFunMap.TryGetValue(source.GetType(), out fun))
				return fun(source, indexArray, backMap, count);
			return null;
		}

		public static Dictionary<Type, Action<Array, Array, int[], int>>
			BackMappedCopyToFunMap = new Dictionary<Type, Action<Array, Array, int[], int>>
			{
				{ typeof(byte[]), (s,t,m,o) => ((byte[])s).BackMappedCopyTo((byte[])t,m,o) },
				{ typeof(sbyte[]), (s,t,m,o) => ((sbyte[])s).BackMappedCopyTo((sbyte[])t,m,o) },
				{ typeof(short[]), (s,t,m,o) => ((short[])s).BackMappedCopyTo((short[])t,m,o) },
				{ typeof(ushort[]), (s,t,m,o) => ((ushort[])s).BackMappedCopyTo((ushort[])t,m,o) },
				{ typeof(int[]), (s,t,m,o) => ((int[])s).BackMappedCopyTo((int[])t,m,o) },
				{ typeof(uint[]), (s,t,m,o) => ((uint[])s).BackMappedCopyTo((uint[])t,m,o) },
				{ typeof(long[]), (s,t,m,o) => ((long[])s).BackMappedCopyTo((long[])t,m,o) },
				{ typeof(ulong[]), (s,t,m,o) => ((ulong[])s).BackMappedCopyTo((ulong[])t,m,o) },
				{ typeof(float[]), (s,t,m,o) => ((float[])s).BackMappedCopyTo((float[])t,m,o) },
				{ typeof(double[]), (s,t,m,o) => ((double[])s).BackMappedCopyTo((double[])t,m,o) },
				{ typeof(C3b[]), (s,t,m,o) => ((C3b[])s).BackMappedCopyTo((C3b[])t,m,o) },
				{ typeof(C3us[]), (s,t,m,o) => ((C3us[])s).BackMappedCopyTo((C3us[])t,m,o) },
				{ typeof(C3ui[]), (s,t,m,o) => ((C3ui[])s).BackMappedCopyTo((C3ui[])t,m,o) },
				{ typeof(C3f[]), (s,t,m,o) => ((C3f[])s).BackMappedCopyTo((C3f[])t,m,o) },
				{ typeof(C3d[]), (s,t,m,o) => ((C3d[])s).BackMappedCopyTo((C3d[])t,m,o) },
				{ typeof(C4b[]), (s,t,m,o) => ((C4b[])s).BackMappedCopyTo((C4b[])t,m,o) },
				{ typeof(C4us[]), (s,t,m,o) => ((C4us[])s).BackMappedCopyTo((C4us[])t,m,o) },
				{ typeof(C4ui[]), (s,t,m,o) => ((C4ui[])s).BackMappedCopyTo((C4ui[])t,m,o) },
				{ typeof(C4f[]), (s,t,m,o) => ((C4f[])s).BackMappedCopyTo((C4f[])t,m,o) },
				{ typeof(C4d[]), (s,t,m,o) => ((C4d[])s).BackMappedCopyTo((C4d[])t,m,o) },
				{ typeof(V2i[]), (s,t,m,o) => ((V2i[])s).BackMappedCopyTo((V2i[])t,m,o) },
				{ typeof(V2l[]), (s,t,m,o) => ((V2l[])s).BackMappedCopyTo((V2l[])t,m,o) },
				{ typeof(V2f[]), (s,t,m,o) => ((V2f[])s).BackMappedCopyTo((V2f[])t,m,o) },
				{ typeof(V2d[]), (s,t,m,o) => ((V2d[])s).BackMappedCopyTo((V2d[])t,m,o) },
				{ typeof(V3i[]), (s,t,m,o) => ((V3i[])s).BackMappedCopyTo((V3i[])t,m,o) },
				{ typeof(V3l[]), (s,t,m,o) => ((V3l[])s).BackMappedCopyTo((V3l[])t,m,o) },
				{ typeof(V3f[]), (s,t,m,o) => ((V3f[])s).BackMappedCopyTo((V3f[])t,m,o) },
				{ typeof(V3d[]), (s,t,m,o) => ((V3d[])s).BackMappedCopyTo((V3d[])t,m,o) },
				{ typeof(V4i[]), (s,t,m,o) => ((V4i[])s).BackMappedCopyTo((V4i[])t,m,o) },
				{ typeof(V4l[]), (s,t,m,o) => ((V4l[])s).BackMappedCopyTo((V4l[])t,m,o) },
				{ typeof(V4f[]), (s,t,m,o) => ((V4f[])s).BackMappedCopyTo((V4f[])t,m,o) },
				{ typeof(V4d[]), (s,t,m,o) => ((V4d[])s).BackMappedCopyTo((V4d[])t,m,o) },
			};

		public static Array BackMappedCopyTo(
				this Array source, int[] backwardMap,
				Array target, int offset)
		{
			BackMappedCopyToFunMap[source.GetType()](
					source, target, backwardMap, offset);
			return target;
		}


		public static Dictionary<Type, Action<Array, Array, int, int[], int[], int>>
			BackMappedGroupCopyToFunMap = new Dictionary<Type, Action<Array, Array, int, int[], int[], int>>
			{
				{ typeof(byte[]), (s,t,c,b,f,o) => ((byte[])s).BackMappedGroupCopyTo(b, c, f, (byte[])t, o) },
				{ typeof(sbyte[]), (s,t,c,b,f,o) => ((sbyte[])s).BackMappedGroupCopyTo(b, c, f, (sbyte[])t, o) },
				{ typeof(short[]), (s,t,c,b,f,o) => ((short[])s).BackMappedGroupCopyTo(b, c, f, (short[])t, o) },
				{ typeof(ushort[]), (s,t,c,b,f,o) => ((ushort[])s).BackMappedGroupCopyTo(b, c, f, (ushort[])t, o) },
				{ typeof(int[]), (s,t,c,b,f,o) => ((int[])s).BackMappedGroupCopyTo(b, c, f, (int[])t, o) },
				{ typeof(uint[]), (s,t,c,b,f,o) => ((uint[])s).BackMappedGroupCopyTo(b, c, f, (uint[])t, o) },
				{ typeof(long[]), (s,t,c,b,f,o) => ((long[])s).BackMappedGroupCopyTo(b, c, f, (long[])t, o) },
				{ typeof(ulong[]), (s,t,c,b,f,o) => ((ulong[])s).BackMappedGroupCopyTo(b, c, f, (ulong[])t, o) },
				{ typeof(float[]), (s,t,c,b,f,o) => ((float[])s).BackMappedGroupCopyTo(b, c, f, (float[])t, o) },
				{ typeof(double[]), (s,t,c,b,f,o) => ((double[])s).BackMappedGroupCopyTo(b, c, f, (double[])t, o) },
			};

		public static void BackMappedGroupCopyTo(this Array source, int[] faceBackMap, int faceCount, int[] fia, Array target, int offset)
		{
			BackMappedGroupCopyToFunMap[source.GetType()]
					(source, target, faceCount, faceBackMap, fia, offset);
		}

		public static Dictionary<Type, Action<Array, long, long>>
			SwapFunMap = new Dictionary<Type, Action<Array, long, long>>
			{
				{ typeof(byte[]), (a, i, j) => ((byte[])a).Swap(i, j) },
				{ typeof(sbyte[]), (a, i, j) => ((sbyte[])a).Swap(i, j) },
				{ typeof(short[]), (a, i, j) => ((short[])a).Swap(i, j) },
				{ typeof(ushort[]), (a, i, j) => ((ushort[])a).Swap(i, j) },
				{ typeof(int[]), (a, i, j) => ((int[])a).Swap(i, j) },
				{ typeof(uint[]), (a, i, j) => ((uint[])a).Swap(i, j) },
				{ typeof(long[]), (a, i, j) => ((long[])a).Swap(i, j) },
				{ typeof(ulong[]), (a, i, j) => ((ulong[])a).Swap(i, j) },
				{ typeof(float[]), (a, i, j) => ((float[])a).Swap(i, j) },
				{ typeof(double[]), (a, i, j) => ((double[])a).Swap(i, j) },
				{ typeof(C3b[]), (a, i, j) => ((C3b[])a).Swap(i, j) },
				{ typeof(C3us[]), (a, i, j) => ((C3us[])a).Swap(i, j) },
				{ typeof(C3ui[]), (a, i, j) => ((C3ui[])a).Swap(i, j) },
				{ typeof(C3f[]), (a, i, j) => ((C3f[])a).Swap(i, j) },
				{ typeof(C3d[]), (a, i, j) => ((C3d[])a).Swap(i, j) },
				{ typeof(C4b[]), (a, i, j) => ((C4b[])a).Swap(i, j) },
				{ typeof(C4us[]), (a, i, j) => ((C4us[])a).Swap(i, j) },
				{ typeof(C4ui[]), (a, i, j) => ((C4ui[])a).Swap(i, j) },
				{ typeof(C4f[]), (a, i, j) => ((C4f[])a).Swap(i, j) },
				{ typeof(C4d[]), (a, i, j) => ((C4d[])a).Swap(i, j) },
				{ typeof(V2i[]), (a, i, j) => ((V2i[])a).Swap(i, j) },
				{ typeof(V2l[]), (a, i, j) => ((V2l[])a).Swap(i, j) },
				{ typeof(V2f[]), (a, i, j) => ((V2f[])a).Swap(i, j) },
				{ typeof(V2d[]), (a, i, j) => ((V2d[])a).Swap(i, j) },
				{ typeof(V3i[]), (a, i, j) => ((V3i[])a).Swap(i, j) },
				{ typeof(V3l[]), (a, i, j) => ((V3l[])a).Swap(i, j) },
				{ typeof(V3f[]), (a, i, j) => ((V3f[])a).Swap(i, j) },
				{ typeof(V3d[]), (a, i, j) => ((V3d[])a).Swap(i, j) },
				{ typeof(V4i[]), (a, i, j) => ((V4i[])a).Swap(i, j) },
				{ typeof(V4l[]), (a, i, j) => ((V4l[])a).Swap(i, j) },
				{ typeof(V4f[]), (a, i, j) => ((V4f[])a).Swap(i, j) },
				{ typeof(V4d[]), (a, i, j) => ((V4d[])a).Swap(i, j) },
			};

		public static void Swap(this Array array, long i, long j)
		{
			SwapFunMap[array.GetType()](array, i, j);
		}

		public static Dictionary<Type, Func<Array, int[], int, Func<int, bool>, Array>>
			GroupReversedCopyMap = new Dictionary<Type, Func<Array, int[], int, Func<int, bool>, Array>>
			{
				{ typeof(byte[]), (a, g, c, r) => ((byte[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(sbyte[]), (a, g, c, r) => ((sbyte[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(short[]), (a, g, c, r) => ((short[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(ushort[]), (a, g, c, r) => ((ushort[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(int[]), (a, g, c, r) => ((int[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(uint[]), (a, g, c, r) => ((uint[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(long[]), (a, g, c, r) => ((long[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(ulong[]), (a, g, c, r) => ((ulong[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(float[]), (a, g, c, r) => ((float[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(double[]), (a, g, c, r) => ((double[])a).GroupReversedCopy(g, c, r) }, 
				{ typeof(C3b[]), (a, g, c, r) => ((C3b[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C3us[]), (a, g, c, r) => ((C3us[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C3ui[]), (a, g, c, r) => ((C3ui[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C3f[]), (a, g, c, r) => ((C3f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C3d[]), (a, g, c, r) => ((C3d[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4b[]), (a, g, c, r) => ((C4b[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4us[]), (a, g, c, r) => ((C4us[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4ui[]), (a, g, c, r) => ((C4ui[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4f[]), (a, g, c, r) => ((C4f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(C4d[]), (a, g, c, r) => ((C4d[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V2i[]), (a, g, c, r) => ((V2i[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V2l[]), (a, g, c, r) => ((V2l[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V2f[]), (a, g, c, r) => ((V2f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V2d[]), (a, g, c, r) => ((V2d[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V3i[]), (a, g, c, r) => ((V3i[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V3l[]), (a, g, c, r) => ((V3l[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V3f[]), (a, g, c, r) => ((V3f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V3d[]), (a, g, c, r) => ((V3d[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V4i[]), (a, g, c, r) => ((V4i[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V4l[]), (a, g, c, r) => ((V4l[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V4f[]), (a, g, c, r) => ((V4f[])a).GroupReversedCopy(g, c, r) },
				{ typeof(V4d[]), (a, g, c, r) => ((V4d[])a).GroupReversedCopy(g, c, r) },
			};

		public static Array GroupReversedCopy(
				this Array array, int[] groupArray, int groupCount,
				Func<int, bool> reverseMap)
		{
			return GroupReversedCopyMap[array.GetType()](
						array, groupArray, groupCount, reverseMap);
		}

		public static Dictionary<Type, Action<Array, int[], int, Func<int, bool>>>
			ReverseGroupsMap = new Dictionary<Type, Action<Array, int[], int, Func<int, bool>>>
			{
				{ typeof(byte[]), (a, g, c, r) => ((byte[])a).ReverseGroups(g, c, r) }, 
				{ typeof(sbyte[]), (a, g, c, r) => ((sbyte[])a).ReverseGroups(g, c, r) }, 
				{ typeof(short[]), (a, g, c, r) => ((short[])a).ReverseGroups(g, c, r) }, 
				{ typeof(ushort[]), (a, g, c, r) => ((ushort[])a).ReverseGroups(g, c, r) }, 
				{ typeof(int[]), (a, g, c, r) => ((int[])a).ReverseGroups(g, c, r) }, 
				{ typeof(uint[]), (a, g, c, r) => ((uint[])a).ReverseGroups(g, c, r) }, 
				{ typeof(long[]), (a, g, c, r) => ((long[])a).ReverseGroups(g, c, r) }, 
				{ typeof(ulong[]), (a, g, c, r) => ((ulong[])a).ReverseGroups(g, c, r) }, 
				{ typeof(float[]), (a, g, c, r) => ((float[])a).ReverseGroups(g, c, r) }, 
				{ typeof(double[]), (a, g, c, r) => ((double[])a).ReverseGroups(g, c, r) }, 
				{ typeof(C3b[]), (a, g, c, r) => ((C3b[])a).ReverseGroups(g, c, r) },
				{ typeof(C3us[]), (a, g, c, r) => ((C3us[])a).ReverseGroups(g, c, r) },
				{ typeof(C3ui[]), (a, g, c, r) => ((C3ui[])a).ReverseGroups(g, c, r) },
				{ typeof(C3f[]), (a, g, c, r) => ((C3f[])a).ReverseGroups(g, c, r) },
				{ typeof(C3d[]), (a, g, c, r) => ((C3d[])a).ReverseGroups(g, c, r) },
				{ typeof(C4b[]), (a, g, c, r) => ((C4b[])a).ReverseGroups(g, c, r) },
				{ typeof(C4us[]), (a, g, c, r) => ((C4us[])a).ReverseGroups(g, c, r) },
				{ typeof(C4ui[]), (a, g, c, r) => ((C4ui[])a).ReverseGroups(g, c, r) },
				{ typeof(C4f[]), (a, g, c, r) => ((C4f[])a).ReverseGroups(g, c, r) },
				{ typeof(C4d[]), (a, g, c, r) => ((C4d[])a).ReverseGroups(g, c, r) },
				{ typeof(V2i[]), (a, g, c, r) => ((V2i[])a).ReverseGroups(g, c, r) },
				{ typeof(V2l[]), (a, g, c, r) => ((V2l[])a).ReverseGroups(g, c, r) },
				{ typeof(V2f[]), (a, g, c, r) => ((V2f[])a).ReverseGroups(g, c, r) },
				{ typeof(V2d[]), (a, g, c, r) => ((V2d[])a).ReverseGroups(g, c, r) },
				{ typeof(V3i[]), (a, g, c, r) => ((V3i[])a).ReverseGroups(g, c, r) },
				{ typeof(V3l[]), (a, g, c, r) => ((V3l[])a).ReverseGroups(g, c, r) },
				{ typeof(V3f[]), (a, g, c, r) => ((V3f[])a).ReverseGroups(g, c, r) },
				{ typeof(V3d[]), (a, g, c, r) => ((V3d[])a).ReverseGroups(g, c, r) },
				{ typeof(V4i[]), (a, g, c, r) => ((V4i[])a).ReverseGroups(g, c, r) },
				{ typeof(V4l[]), (a, g, c, r) => ((V4l[])a).ReverseGroups(g, c, r) },
				{ typeof(V4f[]), (a, g, c, r) => ((V4f[])a).ReverseGroups(g, c, r) },
				{ typeof(V4d[]), (a, g, c, r) => ((V4d[])a).ReverseGroups(g, c, r) },
			};

		public static void ReverseGroups(
				this Array array, int[] groupArray, int groupCount,
				Func<int, bool> reverseMap)
		{
			ReverseGroupsMap[array.GetType()](
					array, groupArray, groupCount, reverseMap);
		}

		/// <summary>
		/// Apply a supplied function to each element of an array, with
		/// supplied conversion functions if the array is of a different
		/// type.
		/// </summary>
		public static Array Apply<T0, T1>(
			this Array array, Func<T1, T1> fun,
			Func<T0, T1> funT1ofT0, Func<T1, T0> funT0ofT1)
		{
			int length = array.Length;
			var t0a = array as T0[];
			if (t0a != null)
			{
				for (int i = 0; i < length; i++)
					t0a[i] = funT0ofT1(fun(funT1ofT0(t0a[i])));
				return array;
			}
			var t1a = array as T1[];
			if (t1a != null)
			{
				for (int i = 0; i < length; i++)
					t1a[i] = fun(t1a[i]);
				return array;
			}
			throw new InvalidOperationException();
		}

		public static IEnumerable<T1> Elements<T0, T1>(
			this Array array, Func<T0, T1> convert)
		{
			var t0a = array as T0[];
			if (t0a != null)
			{
				foreach (var e in t0a) yield return convert(e); yield break;
			}
			var t1a = array as T1[];
			if (t1a != null)
			{
				foreach (var e in t1a) yield return e; yield break;
			}
		}

		public static T1[] CopyAndConvert<T0, T1>(
			this Array array, Func<T0, T1> convert)
		{
			int length = array.Length;
			var result = new T1[length];
			var t0a = array as T0[];
			if (t0a != null)
			{
				for (int i = 0; i < length; i++)
					result[i] = convert(t0a[i]);
				return result;
			}
			var t1a = array as T1[];
			if (t1a != null)
			{
				for (int i = 0; i < length; i++)
					result[i] = t1a[i];
				return result;
			}
			throw new InvalidOperationException();
		}

		public static Tr[] CopyAndConvert<T0, T1, Tr>(
			this Array array, Func<T0, T1> convert, Func<T1, Tr> fun)
		{
			int length = array.Length;
			var result = new Tr[length];
			var t0a = array as T0[];
			if (t0a != null)
			{
				for (int i = 0; i < length; i++)
					result[i] = fun(convert(t0a[i]));
				return result;
			}
			var t1a = array as T1[];
			if (t1a != null)
			{
				for (int i = 0; i < length; i++)
					result[i] = fun(t1a[i]);
				return result;
			}
			throw new InvalidOperationException();
		}

		#endregion

		#region Unsafe Conversion

		public static IntPtr GetTypeIdUncached<T>()
			where T : struct
		{
			var gcHandle = GCHandle.Alloc(new T[1], GCHandleType.Pinned);
			var typeField = gcHandle.AddrOfPinnedObject() - 2 * IntPtr.Size;
			var typeId = Marshal.ReadIntPtr(typeField);
			gcHandle.Free();
			return typeId;
		}

		private static FastConcurrentDict<Type, IntPtr> s_typeIds = new FastConcurrentDict<Type, IntPtr>();

		private static IntPtr GetTypeId<T>()
			where T : struct
		{
			IntPtr typeId;
			if (!s_typeIds.TryGetValue(typeof(T), out typeId))
			{
				typeId = GetTypeIdUncached<T>();
				s_typeIds[typeof(T)] = typeId;
			}
			return typeId;
		}

		internal static TR[] UnsafeCoerce<TR>(this Array input, IntPtr targetId)
			where TR : struct
		{
			var inputSize = Marshal.SizeOf(input.GetType().GetElementType());
			var outputSize = Marshal.SizeOf(typeof(TR));
			var newLength = (input.Length * inputSize) / outputSize;

			var gcHandle = GCHandle.Alloc(input, GCHandleType.Pinned);
			IntPtr baseAddress = gcHandle.AddrOfPinnedObject();
			var sizeField = baseAddress - IntPtr.Size;
			var typeField = sizeField - IntPtr.Size;

			Marshal.WriteIntPtr(sizeField, (IntPtr)newLength);
			Marshal.WriteIntPtr(typeField, targetId);

			gcHandle.Free();

			return (TR[])(object)input;
		}

		/// <summary>
		/// Reinterprets an array as one of a different type
		/// Both types must be structs and you may cause memory leaks when the array-byte-sizes are not multiple of each other
		/// WARNING: destroys the original array
		/// </summary>
		public static TR[] UnsafeCoerce<TR>(this Array input)
			where TR : struct
		{
			return UnsafeCoerce<TR>(input, GetTypeId<TR>());
		}

		internal static void UnsafeCoercedApply<TR>(this Array input, Action<TR[]> action, IntPtr targetId)
			where TR : struct
		{
			var inputSize = Marshal.SizeOf(input.GetType().GetElementType());
			var outputSize = Marshal.SizeOf(typeof(TR));
			var originalLength = input.Length;
			var targetLength = (originalLength * inputSize) / outputSize;

			var gcHandle = GCHandle.Alloc(input, GCHandleType.Pinned);
			IntPtr baseAddress = gcHandle.AddrOfPinnedObject();
			var sizeField = baseAddress - IntPtr.Size;
			var typeField = sizeField - IntPtr.Size;

			var originalId = Marshal.ReadIntPtr(typeField);

			Marshal.WriteIntPtr(typeField, targetId);
			Marshal.WriteIntPtr(sizeField, (IntPtr)targetLength);

			action((TR[])(object)input);

			Marshal.WriteIntPtr(typeField, originalId);
			Marshal.WriteIntPtr(sizeField, (IntPtr)originalLength);

			gcHandle.Free();
		}

		public static void UnsafeCoercedApply<TR>(this Array input, Action<TR[]> action)
			where TR : struct
		{
			UnsafeCoercedApply(input, action, GetTypeId<TR>());
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
	}

}