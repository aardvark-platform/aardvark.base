using System;
using System.Collections.Generic;
using static System.Math;

namespace Aardvark.Base
{
    public static class ListFun
    {
        #region Copying

        public static List<T> Copy<T>(this List<T> self)
        {
            var result = new List<T>(self.Count);
            foreach (var item in self) result.Add(item);
            return result;
        }

        /// <summary>
        /// Create a copy with the elements piped through a function.
        /// </summary>
        public static List<Tr> Map<T, Tr>(this List<T> list, Func<T, Tr> item_fun)
        {
            var count = list.Count;
            var result = new List<Tr>(count);
            for (var i = 0; i < count; i++) result.Add(item_fun(list[i]));
            return result;
        }

        public static List<Tr> Map2<T0, T1, Tr>(
                this List<T0> list0, List<T1> list1, Func<T0, T1, Tr> item0_item1_fun)
        {
            var count = Min(list0.Count, list1.Count);
            var result = new List<Tr>(count);
            for (var i = 0; i < count; i++)
                result.Add(item0_item1_fun(list0[i], list1[i]));
            return result;
        }

        public static List<Tr> Map3<T0, T1, T2, Tr>(
                this List<T0> list0, List<T1> list1, List<T2> list2,
                Func<T0, T1, T2, Tr> item0_item1_item2_fun)
        {
            var count = Min(list0.Count, list1.Count);
            var result = new List<Tr>(count);
            for (var i = 0; i < count; i++)
                result.Add(item0_item1_item2_fun(list0[i], list1[i], list2[i]));
            return result;
        }
        
        /// <summary>
        /// Create a copy with the elements piped through a function.
        /// </summary>
        public static List<Tr> Map<T, Tr>(this List<T> list, Func<T, int, Tr> item_index_fun)
        {
            var count = list.Count;
            var result = new List<Tr>(count);
            for (int i = 0; i < count; i++) result.Add(item_index_fun(list[i], i));
            return result;
        }

        public static List<Tr> Map2<T0, T1, Tr>(
                this List<T0> list0, List<T1> list1, Func<T0, T1, int, Tr> item0_item1_index_fun)
        {
            var count = Min(list0.Count, list1.Count);
            var result = new List<Tr>(count);
            for (var i = 0; i < count; i++)
                result.Add(item0_item1_index_fun(list0[i], list1[i], i));
            return result;
        }

        public static List<Tr> Map3<T0, T1, T2, Tr>(
                this List<T0> list0, List<T1> list1, List<T2> list2,
                Func<T0, T1, T2, int, Tr> item0_item1_item2_index_fun)
        {
            var count = Min(list0.Count, list1.Count);
            var result = new List<Tr>(count);
            for (var i = 0; i < count; i++)
                result.Add(item0_item1_item2_index_fun(list0[i], list1[i], list2[i], i));
            return result;
        }

        public static T[] CopyToArray<T>(this List<T> self)
        {
            return self.CopyToArray(self.Count);
        }

        public static T[] CopyToArray<T>(this List<T> self, int count)
        {
            var result = new T[count];
            for (int i = 0; i < count; i++) result[i] = self[i];
            return result;
        }

        public static Tr[] MapToArray<T, Tr>(this List<T> list, Func<T, Tr> item_fun)
        {
            return list.MapToArray(list.Count, item_fun);
        }

        public static Tr[] MapToArray<T, Tr>(this List<T> list, int count, Func<T, Tr> item_fun)
        {
            var result = new Tr[count];
            if (list.Count < count) count = list.Count;
            for (int i = 0; i < count; i++) result[i] = item_fun(list[i]);
            return result;
        }

        public static Tr[] MapToArray<T, Tr>(
                this List<T> list, int start, int count, Func<T, Tr> item_fun)
        {
            var result = new Tr[count];
            if (start + count > list.Count) count = list.Count - start;
            for (int i = 0; i < count; i++) result[i] = item_fun(list[i + start]);
            return result;
        }

        #endregion

        #region Mapped Copy

        /// <summary>
        /// Returns an array copy of this list that is created by copying the
        /// elements at the positions specified in the supplied backward map.
        /// </summary>
        public static T[] BackwardMappedCopyToArray<T>(
            this List<T> sourceArray,
            int[] backwardMap)
        {
            T[] targetArray = new T[backwardMap.Length];
            sourceArray.BackwardMappedCopyToArray(targetArray, backwardMap, 0);
            return targetArray;
        }

        /// <summary>
        /// Copies from this list into the target array starting at the
        /// supplied offset using the supplied backward map that contains
        /// the source index for each index of the target array.
        /// </summary>
        public static void BackwardMappedCopyToArray<T>(
            this List<T> source,
            T[] target,
            int[] backwardMap,
            int offset)
        {
            for (int i = 0; i < backwardMap.Length; i++)
                target[i + offset] = source[backwardMap[i]];
        }

        /// <summary>
        /// Returns an array copy of this list that is created by copying the
        /// elements at the positions specified in the supplied backward map.
        /// </summary>
        public static Tr[] BackwardMappedCopyToArray<T, Tr>(
            this List<T> sourceArray,
            int[] backwardMap,
            Func<T, Tr> fun)
        {
            Tr[] targetArray = new Tr[backwardMap.Length];
            sourceArray.BackwardMappedCopyToArray(targetArray, backwardMap, 0, fun);
            return targetArray;
        }

        /// <summary>
        /// Copies from this list into the target array starting at the
        /// supplied offset using the supplied backward map that contains
        /// the source index for each index of the target array.
        /// </summary>
        public static void BackwardMappedCopyToArray<T, Tr>(
            this List<T> source,
            Tr[] target,
            int[] backwardMap,
            int offset,
            Func<T, Tr> fun)
        {
            for (int i = 0; i < backwardMap.Length; i++)
                target[i + offset] = fun(source[backwardMap[i]]);
        }

        #endregion

        #region Adding Values


        public static List<T> AddRange<T>(this List<T> list, T value, int count)
        {
            for (int i = 0; i < count; i++) list.Add(value);
            return list;
        }

        /// <summary>
        /// Add a specifable number of elements, each generated by a supplied
        /// function of the respective element index in the list.
        /// </summary>
        /// <returns>the supplied list, so that the function can be directly used after initialization</returns>
        public static List<T> AddByIndex<T>(this List<T> list, int count, Func<int, T> index_fun)
        {
            for (int i = list.Count, e = i + count; i < e; i++) list.Add(index_fun(i));
            return list;
        }

        #endregion

        #region Setters

        /// <summary>
        /// Set all elements to the same supplied value.
        /// </summary>
        /// <returns>this</returns>
        public static List<T> Set<T>(this List<T> self, T value)
        {
            for (int i = 0; i < self.Count; i++) self[i] = value;
            return self;
        }

        /// <summary>
        /// Set all elements to a function of the element index.
        /// </summary>
        /// <returns>this</returns>
        public static List<T> Set<T>(this List<T> self, Func<int, T> fun)
        {
            for (int i = 0; i < self.Count; i++) self[i] = fun(i);
            return self;
        }

        /// <summary>
        /// Set count elements starting at the supplied index to the same
        /// supplied value.
        /// </summary>
        /// <returns>this</returns>
        public static List<T> Set<T>(this List<T> self, int start, int count,
                                     T value)
        {
            int end = System.Math.Min(start + count, self.Count);
            for (int i = start; i < end; i++) self[i] = value;
            return self;
        }

        /// <summary>
        /// Set count elements starting at the supplied index to the same
        /// supplied value.
        /// </summary>
        /// <returns>this</returns>
        public static List<T> Set<T>(this List<T> self, int start, int count,
                                     Func<long, T> fun)
        {
            int end = System.Math.Min(start + count, self.Count);
            for (int i = start; i < end; i++) self[i] = fun(i);
            return self;
        }

        #endregion

        #region

        /// <summary>
        /// Swap the two elements specified by ther indices.
        /// </summary>
        public static void Swap<T>(this List<T> self, int i, int j)
        {
            T help = self[i]; self[i] = self[j]; self[j] = help;
        }
        
        #endregion

        #region Heap

        public static IEnumerable<ComparableIndexedValue<T>> NLargestIndicesAscending<T>(
                this IEnumerable<T> values, int n)
            where T : IComparable<T>
        {
            return values.NLargestIndices(n).HeapAscendingDequeueAll();
        }

        public static List<ComparableIndexedValue<T>> NLargestIndices<T>(
                this IEnumerable<T> values, int n)
            where T : IComparable<T>
        {
            if (n < 0) throw new ArgumentException("n >= 0 required");
            var list = new List<ComparableIndexedValue<T>>(n + 1);
            if (n == 0) return list;
            int i = 0;
            foreach (var v in values)
            {
                var vi = i++;
                if (list.Count < n)
                {
                    list.HeapAscendingEnqueue(v.ComparableIndexedValue(vi));
                    continue;
                }
                if (v.CompareTo(list[0].Value) < 0) continue;
                list.HeapAscendingEnqueue(v.ComparableIndexedValue(vi));
                list.HeapAscendingDequeue();
            }
            return list;
        }

        public static IEnumerable<ComparableIndexedValue<T>> NSmallestIndicesDescending<T>(
                this IEnumerable<T> values, int n)
            where T : IComparable<T>
        {
            return values.NSmallestIndices(n).HeapDescendingDequeueAll();
        }

        public static List<ComparableIndexedValue<T>> NSmallestIndices<T>(
                this IEnumerable<T> values, int n)
            where T : IComparable<T>
        {
            if (n < 0) throw new ArgumentException("n < 0 requried");
            var list = new List<ComparableIndexedValue<T>>(n + 1);
            if (n == 0) return list;
            int i = 0;
            foreach (var v in values)
            {
                var vi = i++;
                if (list.Count < n)
                {
                    list.HeapDescendingEnqueue(v.ComparableIndexedValue(vi));
                    continue;
                }
                if (v.CompareTo(list[0].Value) > 0) continue;
                list.HeapDescendingEnqueue(v.ComparableIndexedValue(vi));
                list.HeapDescendingDequeue();
            }
            return list;
        }

        /// <summary>
        /// Adds an element to the heap, and maintains the heap condition.
        /// For default comparison functions the smallest item is on top of
        /// the heap.
        /// </summary>
        public static void HeapEnqueue<T>(
                this List<T> heap, Func<T, T, int> compare, T element)
        {
            int i = heap.Count;
            heap.Add(element);
            while (i > 0)
            {
                int i2 = (i - 1) / 2;
                if (compare(element, heap[i2]) > 0) break;
                heap[i] = heap[i2];
                i = i2;
            }
            heap[i] = element;
        }

        /// <summary>
        /// Removes and returns the item at the top of the heap (i.e. the
        /// 0th position of the list). For default comparison functions this
        /// is the smallest element.
        /// </summary>
        public static T HeapDequeue<T>(
                this List<T> heap, Func<T, T, int> compare)
        {
            var result = heap[0];
            var count = heap.Count;
            if (count == 1) { heap.Clear(); return result; }
            var element = heap[--count];
            heap.RemoveAt(count);
            int i = 0, i1 = 1;
            while (i1 < count) // at least one child
            {
                int i2 = i1 + 1;
                int ni = (i2 < count // two children?
                          && compare(heap[i1], heap[i2]) > 0)
                             ? i2 : i1; // smaller child
                if (compare(heap[ni], element) > 0) break;
                heap[i] = heap[ni];
                i = ni; i1 = 2 * i + 1;
            }
            heap[i] = element;
            return result;
        }

        /// <summary>
        /// Removes and returns all items from the heap as an IEnumerable. For
        /// default comparison functiosn this is in ascending order.
        /// </summary>
        public static IEnumerable<T> HeapDequeueAll<T>(
                this List<T> heap, Func<T, T, int> compare)
        {
            while (heap.Count > 0) yield return heap.HeapDequeue(compare);
        }

        /// <summary>
        /// Reomves an arbitrary element from the heap, maintains the heap
        /// conditions, and returns the removed element.
        /// </summary>
        public static T HeapRemoveAt<T>(
                this List<T> heap, Func<T, T, int> compare, int index)
        {
            var result = heap[index];
            var count = heap.Count;
            if (count == 1) { heap.Clear(); return result; }
            var element = heap[--count];
            heap.RemoveAt(count);
            if (index == count) return result;

            int i = index;
            while (i > 0)
            {
                int i2 = (i - 1) / 2;
                if (compare(element, heap[i2]) > 0) break;
                heap[i] = heap[i2];
                i = i2;
            }
            if (i == index)
            {
                int i1 = 2 * i + 1;
                while (i1 < count) // at least one child
                {
                    int i2 = i1 + 1;
                    int ni = (i2 < count // two children?
                              && compare(heap[i1], heap[i2]) > 0)
                                 ? i2 : i1; // smaller child
                    if (compare(heap[ni], element) > 0) break;
                    heap[i] = heap[ni];
                    i = ni; i1 = 2 * i + 1;
                }
            }
            heap[i] = element;
            return result;
        }

        /// <summary>
        /// Adds an element to the heap, and maintains the heap condition.
        /// For default comparison functions the smallest item is on top of
        /// the heap.
        /// </summary>
        public static void HeapAscendingEnqueue<T>(
                this List<T> heap, T element)
            where T : IComparable<T>
        {
            int i = heap.Count;
            heap.Add(element);
            while (i > 0)
            {
                int i2 = (i - 1) / 2;
                if (element.CompareTo(heap[i2]) > 0) break;
                heap[i] = heap[i2];
                i = i2;
            }
            heap[i] = element;
        }

        /// <summary>
        /// Removes and returns the item at the top of the heap (i.e. the
        /// 0th position of the list). For default comparison functions this
        /// is the smallest element.
        /// </summary>
        public static T HeapAscendingDequeue<T>(
                this List<T> heap)
            where T : IComparable<T>
        {
            var result = heap[0];
            var count = heap.Count;
            if (count == 1) { heap.Clear(); return result; }
            var element = heap[--count];
            heap.RemoveAt(count);
            int i = 0, i1 = 1;
            while (i1 < count) // at least one child
            {
                int i2 = i1 + 1;
                int ni = (i2 < count // two children?
                          && heap[i1].CompareTo(heap[i2]) > 0)
                             ? i2 : i1; // smaller child
                if (heap[ni].CompareTo(element) > 0) break;
                heap[i] = heap[ni];
                i = ni; i1 = 2 * i + 1;
            }
            heap[i] = element;
            return result;
        }

        /// <summary>
        /// Removes and returns all items from the heap as an IEnumerable. For
        /// default comparison functiosn this is in ascending order.
        /// </summary>
        public static IEnumerable<T> HeapAscendingDequeueAll<T>(
                this List<T> heap)
            where T : IComparable<T>
        {
            while (heap.Count > 0) yield return heap.HeapAscendingDequeue();
        }

        /// <summary>
        /// Reomves an arbitrary element from the heap, maintains the heap
        /// conditions, and returns the removed element.
        /// </summary>
        public static T HeapAscendingRemoveAt<T>(
                this List<T> heap, int index)
            where T : IComparable<T>
        {
            var result = heap[index];
            var count = heap.Count;
            if (count == 1) { heap.Clear(); return result; }
            var element = heap[--count];
            heap.RemoveAt(count);
            if (index == count) return result;

            int i = index;
            while (i > 0)
            {
                int i2 = (i - 1) / 2;
                if (element.CompareTo(heap[i2]) > 0) break;
                heap[i] = heap[i2];
                i = i2;
            }
            if (i == index)
            {
                int i1 = 2 * i + 1;
                while (i1 < count) // at least one child
                {
                    int i2 = i1 + 1;
                    int ni = (i2 < count // two children?
                              && heap[i1].CompareTo(heap[i2]) > 0)
                                 ? i2 : i1; // smaller child
                    if (heap[ni].CompareTo(element) > 0) break;
                    heap[i] = heap[ni];
                    i = ni; i1 = 2 * i + 1;
                }
            }
            heap[i] = element;
            return result;
        }

        /// <summary>
        /// Adds an element to the heap, and maintains the heap condition.
        /// For default comparison functions the smallest item is on top of
        /// the heap.
        /// </summary>
        public static void HeapDescendingEnqueue<T>(
                this List<T> heap, T element)
            where T : IComparable<T>
        {
            int i = heap.Count;
            heap.Add(element);
            while (i > 0)
            {
                int i2 = (i - 1) / 2;
                if (element.CompareTo(heap[i2]) < 0) break;
                heap[i] = heap[i2];
                i = i2;
            }
            heap[i] = element;
        }

        /// <summary>
        /// Removes and returns the item at the top of the heap (i.e. the
        /// 0th position of the list). For default comparison functions this
        /// is the smallest element.
        /// </summary>
        public static T HeapDescendingDequeue<T>(
                this List<T> heap)
            where T : IComparable<T>
        {
            var result = heap[0];
            var count = heap.Count;
            if (count == 1) { heap.Clear(); return result; }
            var element = heap[--count];
            heap.RemoveAt(count);
            int i = 0, i1 = 1;
            while (i1 < count) // at least one child
            {
                int i2 = i1 + 1;
                int ni = (i2 < count // two children?
                          && heap[i1].CompareTo(heap[i2]) < 0)
                             ? i2 : i1; // smaller child
                if (heap[ni].CompareTo(element) < 0) break;
                heap[i] = heap[ni];
                i = ni; i1 = 2 * i + 1;
            }
            heap[i] = element;
            return result;
        }

        /// <summary>
        /// Removes and returns all items from the heap as an IEnumerable. For
        /// default comparison functiosn this is in ascending order.
        /// </summary>
        public static IEnumerable<T> HeapDescendingDequeueAll<T>(
                this List<T> heap)
            where T : IComparable<T>
        {
            while (heap.Count > 0) yield return heap.HeapDescendingDequeue();
        }

        /// <summary>
        /// Reomves an arbitrary element from the heap, maintains the heap
        /// conditions, and returns the removed element.
        /// </summary>
        public static T HeapDescendingRemoveAt<T>(
                this List<T> heap, int index)
            where T : IComparable<T>
        {
            var result = heap[index];
            var count = heap.Count;
            if (count == 1) { heap.Clear(); return result; }
            var element = heap[--count];
            heap.RemoveAt(count);
            if (index == count) return result;

            int i = index;
            while (i > 0)
            {
                int i2 = (i - 1) / 2;
                if (element.CompareTo(heap[i2]) < 0) break;
                heap[i] = heap[i2];
                i = i2;
            }
            if (i == index)
            {
                int i1 = 2 * i + 1;
                while (i1 < count) // at least one child
                {
                    int i2 = i1 + 1;
                    int ni = (i2 < count // two children?
                              && heap[i1].CompareTo(heap[i2]) < 0)
                                 ? i2 : i1; // smaller child
                    if (heap[ni].CompareTo(element) < 0) break;
                    heap[i] = heap[ni];
                    i = ni; i1 = 2 * i + 1;
                }
            }
            heap[i] = element;
            return result;
        }

        #endregion

        #region Lists of IComparables

        public static List<T> MergeSortedAscending<T>(this List<T> a0, List<T> a1)
            where T : IComparable<T>
        {
            int c0 = a0.Count;
            int c1 = a1.Count;
            var a = new List<T>(c0 + c1);
            int i0 = 0, i1 = 0;
            while (i0 < c0 && i1 < c1)
            {
                if (a0[i0].CompareTo(a1[i1]) <= 0)
                    a.Add(a0[i0++]);
                else
                    a.Add(a1[i1++]);
            }
            while (i0 < c0) a.Add(a0[i0++]);
            while (i1 < c1) a.Add(a1[i1++]);
            return a;
        }

        public static T[] MergeSortAscending<T>(this T[] a0, T[] a1)
            where T : IComparable<T>
        {
            long c0 = a0.LongLength;
            long c1 = a1.LongLength;
            var a = new T[c0 + c1];
            long i0 = 0, i1 = 0, i = 0;
            while (i0 < c0 && i1 < c1)
            {
                if (a0[i0].CompareTo(a1[i1]) <= 0)
                    a[i++] = a0[i0++];
                else
                    a[i++] = a1[i1++];
            }
            while (i0 < c0) a[i++] = a0[i0++];
            while (i1 < c1) a[i++] = a1[i1++];
            return a;
        }

        public static int SmallestIndex<T>(this List<T> a)
            where T : IComparable<T>
        {
            int index = 0;
            T min = a[0];
            for (int i = 1; i < a.Count; i++)
                if (a[i].CompareTo(min) < 0) { min = a[i]; index = i; }
            return index;
        }

        public static int LargestIndex<T>(this List<T> a)
            where T : IComparable<T>
        {
            int index = 0;
            T max = a[0];
            for (int i = 1; i < a.Count; i++)
                if (a[i].CompareTo(max) > 0) { max = a[i]; index = i; }
            return index;
        }

        #endregion
    }
}