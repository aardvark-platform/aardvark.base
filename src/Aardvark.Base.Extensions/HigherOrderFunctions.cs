using System;
using System.Collections.Generic;
using static System.Math;

namespace Aardvark.Base
{
    public static class ControlFun
    {
        #region ForEach on ints

        /// <summary>
        /// Perform the supplied action for all integers i in [0, n).
        /// </summary>
        public static void ForEach(this int n, Action<int> i_act)
        {
            for (int k = 0; k < n; k++) { i_act(k); }
        }

        /// <summary>
        /// Perform the supplied action for all integers in [start, n).
        /// </summary>
        public static void ForEach(this int n, int start, Action<int> i_act)
        {
            for (int k = start; k < n; k++) { i_act(k); }
        }

        /// <summary>
        /// Perform the supplied action for all integers i in [0, n).
        /// Perform the supplied separating action between the integers.
        /// </summary>
        public static void ForEach(this int n, Action<int> i_act, Action sep)
        {
            for (int k = 0; k < n - 1; k++) { i_act(k); sep(); }
            i_act(n - 1);
        }

        /// <summary>
        /// Perform the supplied action for all integers i in [start, n).
        /// Perform the supplied separating action between the integers.
        /// </summary>
        public static void ForEach(
                this int n, int start, Action<int> i_act, Action sep)
        {
            for (int k = start; k < n - 1; k++) { i_act(k); sep(); }
            i_act(n - 1);
        }

        /// <summary>
        /// Perform the supplied action for all integers i in [0, n).
        /// Perform the supplied separating action between the integers.
        /// The seperating action receives the index of the preceeding item.
        /// </summary>
        public static void ForEach(this int n, Action<int> i_act, Action<int> i_sep)
        {
            for (int k = 0; k < n - 1; k++) { i_act(k); i_sep(k); }
            i_act(n - 1);
        }

        /// <summary>
        /// Perform the supplied action for all integers i in [start, n).
        /// Perform the supplied separating action between the integers.
        /// The seperating action receives the index of the preceeding item.
        /// </summary>
        public static void ForEach(
                this int n, int start, Action<int> i_act, Action<int> i_sep)
        {
            for (int k = start; k < n - 1; k++) { i_act(k); i_sep(k); }
            i_act(n - 1);
        }

        #endregion

        #region ForEach on longs

        /// <summary>
        /// Perform the supplied action for all integers i in [0, n).
        /// </summary>
        public static void ForEach(this long n, Action<long> i_act)
        {
            for (long k = 0; k < n; k++) { i_act(k); }
        }

        /// <summary>
        /// Perform the supplied action for all integers in [start, n).
        /// </summary>
        public static void ForEach(this long n, long start, Action<long> i_act)
        {
            for (long k = start; k < n; k++) { i_act(k); }
        }

        #endregion

        #region ForEach on IEnumerable<T>

        /// <summary>
        /// Perform the supplied action for all items in the IEnumerable.
        /// </summary>
        public static void ForEach<T>(
                this IEnumerable<T> items, Action<T> item_act)
        {
            foreach (var item in items) item_act(item);
        }

        /// <summary>
        /// Perform the supplied action for all items in the IEnumerable.
        /// The action gets the index of the item i out of [0, n) as last
        /// parameter.
        /// </summary>
        public static void ForEach<T>(
                this IEnumerable<T> items, Action<T, int> item_i_act)
        {
            int i = 0;
            foreach (var item in items) item_i_act(item, i++);
        }

        /// <summary>
        /// Perform the supplied action for all pairs of items in the IEnumerable
        /// and from the second IEnumerable.
        /// </summary>
        public static void ForEach<T0, T1>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1,
                Action<T0, T1> item0_item1_act)
        {
            foreach (var item in items0.ZipTuples(items1))
                item0_item1_act(item.E0, item.E1);
        }

        /// <summary>
        /// Perform the supplied action for all triples of items from
        /// each of the supplied IEnumerables.
        /// </summary>
        public static void ForEach<T0, T1, T2>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1, IEnumerable<T2> items2,
                Action<T0, T1, T2> item0_item1_item2_act)
        {
            foreach (var item in items0.ZipTuples(items1, items2))
                item0_item1_item2_act(item.E0, item.E1, item.E2);
        }

        /// <summary>
        /// Perform the supplied action for all corresponding pairs
        /// of items from the IEnumerable and from the second IEnumerable.
        /// The action gets the index of the item as last parameter.
        /// </summary>
        public static void ForEach<T0, T1>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1,
                Action<T0, T1, int> item0_item1_i_act)
        {
            int i = 0;
            foreach (var item in items0.ZipTuples(items1))
                item0_item1_i_act(item.E0, item.E1, i++);
        }

        /// <summary>
        /// Perform the supplied action for all triples of items from
        /// each of the supplied IEnumerables.
        /// The action gets the index of the item as last parameter.
        /// </summary>
        public static void ForEach<T0, T1, T2>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1, IEnumerable<T2> items2,
                Action<T0, T1, T2, int> item0_item1_item2_i_act)
        {
            int i = 0;
            foreach (var item in items0.ZipTuples(items1, items2))
                item0_item1_item2_i_act(item.E0, item.E1, item.E2, i++);
        }

        /// <summary>
        /// Perform the supplied action for all items in the IEnumerable.
        /// Perform the supplied separating action between the items.
        /// </summary>
        public static void ForEach<T>(
                this IEnumerable<T> items, Action<T> item_act, Action sep)
        {
            bool notfirst = false;
            foreach (var item in items)
            {
                if (notfirst) sep(); else notfirst = true;
                item_act(item);
            }
        }

        /// <summary>
        /// Perform the supplied action for all items in the IEnumerable.
        /// The action gets the index i out of [0, n) of the item as last
        /// parameter. Perform the supplied separating action between items.
        /// </summary>
        public static void ForEach<T>(
                this IEnumerable<T> items,
                Action<T, int> item_i_act, Action sep)
        {
            int i = -1;
            foreach (var item in items)
            {
                if (i >= 0) sep();
                item_i_act(item, ++i);
            }
        }

        /// <summary>
        /// Perform the supplied action for all items in the IEnumerable.
        /// The action gets the index of the item as last parameter.
        /// Perform the supplied separating action between items.
        /// The seperating action receives the index of the preceeding item.
        /// </summary>
        public static void ForEach<T>(
                this IEnumerable<T> items,
                Action<T, int> item_i_act, Action<int> i_sep)
        {
            int i = -1;
            foreach (var item in items)
            {
                if (i >= 0) i_sep(i);
                item_i_act(item, ++i);
            }
        }

        /// <summary>
        /// Perform the supplied action for all corresponding pairs
        /// of items from the IEnumerable and from the second IEnumerable.
        /// Perform the supplied separating action between the items.
        /// </summary>
        public static void ForEach<T0, T1>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1,
                Action<T0, T1> item0_item1_act, Action sep)
        {
            bool notfirst = false;
            foreach (var item in items0.ZipTuples(items1))
            {
                if (notfirst) sep(); else notfirst = true;
                item0_item1_act(item.E0, item.E1);
            }
        }

        /// <summary>
        /// Perform the supplied action for all triples of items from
        /// each of the supplied IEnumerables.
        /// Perform the supplied separating action between the items.
        /// </summary>
        public static void ForEach<T0, T1, T2>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1, IEnumerable<T2> items2,
                Action<T0, T1, T2> item0_item1_item2_act, Action sep)
        {
            bool notfirst = false;
            foreach (var item in items0.ZipTuples(items1, items2))
            {
                if (notfirst) sep(); else notfirst = true;
                item0_item1_item2_act(item.E0, item.E1, item.E2);
            }
        }

        /// <summary>
        /// Perform the supplied action for all corresponding pairs
        /// of items from the IEnumerable and from the second IEnumerable.
        /// The action gets the index of the item as last parameter.
        /// Perform the supplied separating action between the items.
        /// </summary>
        public static void ForEach<T0, T1>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1,
                Action<T0, T1, int> item0_item1_i_act, Action sep)
        {
            int i = -1;
            foreach (var item in items0.ZipTuples(items1))
            {
                if (i >= 0) sep();
                item0_item1_i_act(item.E0, item.E1, ++i);
            }
        }

        /// <summary>
        /// Perform the supplied action for all triples of items from
        /// each of the supplied IEnumerables.
        /// The action gets the index of the item as last parameter.
        /// Perform the supplied separating action between the items.
        /// </summary>
        public static void ForEach<T0, T1, T2>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1, IEnumerable<T2> items2,
                Action<T0, T1, T2, int> item0_item1_item2_int_act, Action sep)
        {
            int i = -1;
            foreach (var item in items0.ZipTuples(items1, items2))
            {
                if (i >= 0) sep();
                item0_item1_item2_int_act(item.E0, item.E1, item.E2, ++i);
            }
        }

        /// <summary>
        /// Perform the supplied action for all corresponding pairs
        /// of items from the IEnumerable and from the second IEnumerable.
        /// The action gets the index of the item as last parameter.
        /// Perform the supplied separating action between the items.
        /// The seperating action receives the index of the preceeding items.
        /// </summary>
        public static void ForEach<T0, T1>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1,
                Action<T0, T1, int> item0_item1_i_act, Action<int> i_sep)
        {
            int i = -1;
            foreach (var item in items0.ZipTuples(items1))
            {
                if (i >= 0) i_sep(i);
                item0_item1_i_act(item.E0, item.E1, ++i);
            }
        }

        /// <summary>
        /// Perform the supplied action for all triples of items from
        /// each of the supplied IEnumerables.
        /// The action gets the index of the item as last parameter.
        /// Perform the supplied separating action between the items.
        /// The seperating action receives the index of the preceeding items.
        /// </summary>
        public static void ForEach<T0, T1, T2>(
                this IEnumerable<T0> items0, IEnumerable<T1> items1, IEnumerable<T2> items2,
                Action<T0, T1, T2, int> item0_item1_item2_i_act, Action<int> i_sep)
        {
            int i = -1;
            foreach (var item in items0.ZipTuples(items1, items2))
            {
                if (i >= 0) i_sep(i);
                item0_item1_item2_i_act(item.E0, item.E1, item.E2, ++i);
            }
        }

        #endregion

        #region ForEach on Arrays

        /// <summary>
        /// Perform the supplied action for all items in the IEnumerable.
        /// </summary>
        public static void ForEach<T>(
                this T[] array, Action<T> item_act)
        {
            int count = array.Length;
            for (int i = 0; i < count; i++) item_act(array[i]);
        }

        /// <summary>
        /// Perform the supplied action for all items in the IEnumerable.
        /// The action gets the index of the item i out of [0, n) as last
        /// parameter.
        /// </summary>
        public static void ForEach<T>(
                this T[] array, Action<T, int> item_i_act)
        {
            int count = array.Length;
            for (int i = 0; i < count; i++) item_i_act(array[i], i);
        }

        /// <summary>
        /// Perform the supplied action for all pairs of items in the array
        /// and from the second array.
        /// </summary>
        public static void ForEach<T0, T1>(
                this T0[] array0, T1[] array1,
                Action<T0, T1> item0_item1_act)
        {
            int count = Min(array0.Length, array1.Length);
            for (int i = 0; i < count; i++)
                item0_item1_act(array0[i], array1[i]);
        }

        /// <summary>
        /// Perform the supplied action for all triples of items from
        /// each of the supplied arrays.
        /// </summary>
        public static void ForEach<T0, T1, T2>(
                this T0[] array0, T1[] array1, T2[] array2,
                Action<T0, T1, T2> item0_item1_item2_act)
        {
            int count = Min(array0.Length, Min(array1.Length, array2.Length));
            for (int i = 0; i < count; i++)
                item0_item1_item2_act(array0[i], array1[i], array2[i]);
        }

        /// <summary>
        /// Perform the supplied action for all corresponding pairs
        /// of items from the array and from the second array.
        /// The action gets the index of the item as last parameter.
        /// </summary>
        public static void ForEach<T0, T1>(
                this T0[] array0, T1[] array1,
                Action<T0, T1, int> item0_item1_i_act)
        {
            int count = Min(array0.Length, array1.Length);
            for (int i = 0; i < count; i++)
                item0_item1_i_act(array0[i], array1[i], i);
        }

        /// <summary>
        /// Perform the supplied action for all triples of items from
        /// each of the supplied arrays.
        /// The action gets the index of the item as last parameter.
        /// </summary>
        public static void ForEach<T0, T1, T2>(
                this T0[] array0, T1[] array1, T2[] array2,
                Action<T0, T1, T2, int> item0_item1_item2_i_act)
        {
            int count = Min(array0.Length, Min(array1.Length, array2.Length));
            for (int i = 0; i < count; i++)
                item0_item1_item2_i_act(array0[i], array1[i], array2[i], i);
        }

        #endregion

        #region Switch

        public static void Switch<T>(
            this T value, Dictionary<T, Action<T>> actionDict)
        {
            if (actionDict.TryGetValue(value, out Action<T> action))
                action(value);
        }

        public static void Switch<T>(
            this T value, Dictionary<T, Action<T>> actionDict, Action<T> defaultAction)
        {
            if (actionDict.TryGetValue(value, out Action<T> action))
                action(value);
            else
                defaultAction(value);
        }

        public static TResult Switch<T, TResult>(
            this T value, Dictionary<T, Func<T, TResult>> funcDict)
        {
            if (funcDict.TryGetValue(value, out Func<T, TResult> func))
                return func(value);
            else
                return default(TResult);
        }

        public static TResult Switch<T, TResult>(
            this T value, Dictionary<T, Func<T, TResult>> funcDict, Func<T, TResult> defaultFunc)
        {
            if (funcDict.TryGetValue(value, out Func<T, TResult> func))
                return func(value);
            else
                return defaultFunc(value);
        }

        #endregion
    }
}
