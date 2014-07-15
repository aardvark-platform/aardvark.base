using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public class ActionTable<T> : Dict<T, Action>
    { }

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
            for (int k = 0; k < n - 1; k++) { i_act(k); sep(); } i_act(n - 1);
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
            int count = Fun.Min(array0.Length, array1.Length);
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
            int count = Fun.Min(array0.Length, array1.Length, array2.Length);
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
            int count = Fun.Min(array0.Length, array1.Length);
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
            int count = Fun.Min(array0.Length, array1.Length, array2.Length);
            for (int i = 0; i < count; i++)
                item0_item1_item2_i_act(array0[i], array1[i], array2[i], i);
        }

        #endregion

        #region Switch

        public static void Switch<T>(
            this T value, Dict<T, Action<T>> actionDict)
        {
            Action<T> action;
            if (actionDict.TryGetValue(value, out action))
                action(value);
        }

        public static void Switch<T>(
            this T value, Dict<T, Action<T>> actionDict, Action<T> defaultAction)
        {
            Action<T> action;
            if (actionDict.TryGetValue(value, out action))
                action(value);
            else
                defaultAction(value);
        }

        public static TResult Switch<T, TResult>(
            this T value, Dict<T, Func<T, TResult>> funcDict)
        {
            Func<T, TResult> func;
            if (funcDict.TryGetValue(value, out func))
                return func(value);
            else
                return default(TResult);
        }

        public static TResult Switch<T, TResult>(
            this T value, Dict<T, Func<T, TResult>> funcDict, Func<T, TResult> defaultFunc)
        {
            Func<T, TResult> func;
            if (funcDict.TryGetValue(value, out func))
                return func(value);
            else
                return defaultFunc(value);
        }

        #endregion
    }

    public static class HighFun
    {
        #region IntoFunc

        /// <summary>
        /// Wrap a value into a function.
        /// </summary>
        public static Func<T> IntoFunc<T>(this T x) { return () => x; }

        #endregion

        #region Compose

        /// <summary>
        /// Fun.Compose(f0, f1) returns f1(f0).
        /// </summary>
        public static Func<T0, T2> Compose<T0, T1, T2>(
                this Func<T0, T1> f0, Func<T1, T2> f1)
        {
            return x => f1(f0(x));
        }

        /// <summary>
        /// Fun.Compose(f0, f1, f2) returns f2(f1(f0)).
        /// </summary>
        public static Func<T0, T3> Compose<T0, T1, T2, T3>(
                this Func<T0, T1> f0, Func<T1, T2> f1, Func<T2, T3> f2)
        {
            return x => f2(f1(f0(x)));
        }

        /// <summary>
        /// Fun.Compose(f0, f1, f2, f3) returns f3(f2(f1(f0))).
        /// </summary>
        public static Func<T0, T4> Compose<T0, T1, T2, T3, T4>(
                this Func<T0, T1> f0, Func<T1, T2> f1, Func<T2, T3> f2, Func<T3, T4> f3)
        {
            return x => f3(f2(f1(f0(x))));
        }

        #endregion

        #region Partial Application

        public static Func<TArg1, TResult>
            ApplyArg0<TArg0, TArg1, TResult>(
                this Func<TArg0, TArg1, TResult> fun, TArg0 a0)
        {
            return a1 => fun(a0, a1);
        }

        public static Func<TArg0, TResult>
            ApplyArg1<TArg0, TArg1, TResult>(
                this Func<TArg0, TArg1, TResult> fun, TArg1 a1)
        {
            return a0 => fun(a0, a1);
        }

        public static Func<TArg1, TArg2, TResult>
            ApplyArg0<TArg0, TArg1, TArg2, TResult>(
                this Func<TArg0, TArg1, TArg2, TResult> fun, TArg0 a0)
        {
            return (a1, a2) => fun(a0, a1, a2);
        }

        public static Func<TArg0, TArg2, TResult>
            ApplyArg1<TArg0, TArg1, TArg2, TResult>(
                this Func<TArg0, TArg1, TArg2, TResult> fun, TArg1 a1)
        {
            return (a0, a2) => fun(a0, a1, a2);
        }

        public static Func<TArg0, TArg1, TResult>
            ApplyArg2<TArg0, TArg1, TArg2, TResult>(
                this Func<TArg0, TArg1, TArg2, TResult> fun, TArg2 a2)
        {
            return (a0, a1) => fun(a0, a1, a2);
        }

        public static Func<TArg2, TResult>
            ApplyArg0Arg1<TArg0, TArg1, TArg2, TResult>(
                this Func<TArg0, TArg1, TArg2, TResult> fun, TArg0 a0, TArg1 a1)
        {
            return a2 => fun(a0, a1, a2);
        }

        public static Func<TArg1, TResult>
            ApplyArg0Arg2<TArg0, TArg1, TArg2, TResult>(
                this Func<TArg0, TArg1, TArg2, TResult> fun, TArg0 a0, TArg2 a2)
        {
            return a1 => fun(a0, a1, a2);
        }

        public static Func<TArg0, TResult>
            ApplyArg1Arg2<TArg0, TArg1, TArg2, TResult>(
                this Func<TArg0, TArg1, TArg2, TResult> fun, TArg1 a1, TArg2 a2)
        {
            return a0 => fun(a0, a1, a2);
        }

        public static Func<TArg1, TArg2, TArg3, TResult>
            ApplyArg0<TArg0, TArg1, TArg2, TArg3, TResult>(
                this Func<TArg0, TArg1, TArg2, TArg3, TResult> fun, TArg0 a0)
        {
            return (a1, a2, a3) => fun(a0, a1, a2, a3);
        }

        public static Func<TArg0, TArg2, TArg3, TResult>
            ApplyArg1<TArg0, TArg1, TArg2, TArg3, TResult>(
                this Func<TArg0, TArg1, TArg2, TArg3, TResult> fun, TArg1 a1)
        {
            return (a0, a2, a3) => fun(a0, a1, a2, a3);
        }

        public static Func<TArg0, TArg1, TArg3, TResult>
            ApplyArg2<TArg0, TArg1, TArg2, TArg3, TResult>(
                this Func<TArg0, TArg1, TArg2, TArg3, TResult> fun, TArg2 a2)
        {
            return (a0, a1, a3) => fun(a0, a1, a2, a3);
        }

        public static Func<TArg0, TArg1, TArg2, TResult>
            ApplyArg3<TArg0, TArg1, TArg2, TArg3, TResult>(
                this Func<TArg0, TArg1, TArg2, TArg3, TResult> fun, TArg3 a3)
        {
            return (a0, a1, a2) => fun(a0, a1, a2, a3);
        }

        #endregion

        #region Currying

        public static Func<TArg0, Func<TArg1, TResult>>
            Curry<TArg0, TArg1, TResult>(
                this Func<TArg0, TArg1,
                TResult> fun)
        {
            return a0 => a1 => fun(a0, a1);
        }

        public static Func<TArg0, Func<TArg1, Func<TArg2, TResult>>>
            Curry<TArg0, TArg1, TArg2, TResult>(
                this Func<TArg0, TArg1, TArg2,
                TResult> fun)
        {
            return a0 => a1 => a2 => fun(a0, a1, a2);
        }

        public static Func<TArg0, Func<TArg1,  Func<TArg2, Func<TArg3, TResult>>>>
            Curry<TArg0, TArg1, TArg2, TArg3, TResult>(
                this Func<TArg0, TArg1, TArg2, TArg3,
                TResult> fun)
        {
            return a0 => a1 => a2 => a3 => fun(a0, a1, a2, a3);
        }

        #endregion

        #region FixedPointCombinators

        public static Func<T0, TR> Y<T0, TR>(
                Func<Func<T0, TR>, Func<T0, TR>> f)
        {
            Recursive<T0, TR> rec = r => a0 => f(r(r))(a0);
            return rec(rec);
        }

        public static Func<T0, T1, TR> Y<T0, T1, TR>(
                Func<Func<T0, T1, TR>, Func<T0, T1, TR>> f)
        {
            Recursive<T0, T1, TR> rec = r => (a0, a1) => f(r(r))(a0, a1);
            return rec(rec);
        }

        public static Func<T0, T1, T2, TR> Y<T0, T1, T2, TR>(
                Func<Func<T0, T1, T2, TR>, Func<T0, T1, T2, TR>> f)
        {
            Recursive<T0, T1, T2, TR> rec
                = r => (a0, a1, a2) => f(r(r))(a0, a1, a2);
            return rec(rec);
        }

        public static Func<T0, T1, T2, T3, TR> Y<T0, T1, T2, T3, TR>(
                Func<Func<T0, T1, T2, T3, TR>, Func<T0, T1, T2, T3, TR>> f)
        {
            Recursive<T0, T1, T2, T3, TR> rec
                = r => (a0, a1, a2, a3) => f(r(r))(a0, a1, a2, a3);
            return rec(rec);
        }

        #endregion
    }

    public struct FuncOfT2Bool<T>
    {
        public Func<T, bool> F { get; private set; }

        public FuncOfT2Bool(Func<T, bool> f)
            : this()
        {
            F = f;
        }

        public static implicit operator Func<T, bool>(FuncOfT2Bool<T> x)
        {
            return x.F;
        }
        public static implicit operator FuncOfT2Bool<T>(Func<T, bool> x)
        {
            return new FuncOfT2Bool<T>(x);
        }

        //public static bool operator false(FuncOfT2Bool<T> x)
        //{
        //    return false;
        //}
        //public static bool operator true(FuncOfT2Bool<T> x)
        //{
        //    return true;
        //}

        public static FuncOfT2Bool<T> operator &(FuncOfT2Bool<T> a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) & b.F(et));
        }
        public static FuncOfT2Bool<T> operator |(FuncOfT2Bool<T> a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) | b.F(et));
        }
        public static FuncOfT2Bool<T> operator ^(FuncOfT2Bool<T> a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) ^ b.F(et));
        }
        public static FuncOfT2Bool<T> operator !(FuncOfT2Bool<T> a)
        {
            return new FuncOfT2Bool<T>(et => !a.F(et));
        }
        public static FuncOfT2Bool<T> operator ==(FuncOfT2Bool<T> a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) == b.F(et));
        }
        public static FuncOfT2Bool<T> operator !=(FuncOfT2Bool<T> a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) != b.F(et));
        }

        public static FuncOfT2Bool<T> operator &(FuncOfT2Bool<T> a, bool b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) & b);
        }
        public static FuncOfT2Bool<T> operator |(FuncOfT2Bool<T> a, bool b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) | b);
        }
        public static FuncOfT2Bool<T> operator ^(FuncOfT2Bool<T> a, bool b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) ^ b);
        }
        public static FuncOfT2Bool<T> operator ==(FuncOfT2Bool<T> a, bool b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) == b);
        }
        public static FuncOfT2Bool<T> operator !=(FuncOfT2Bool<T> a, bool b)
        {
            return new FuncOfT2Bool<T>(et => a.F(et) != b);
        }
        
        public static FuncOfT2Bool<T> operator &(bool a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a & b.F(et));
        }
        public static FuncOfT2Bool<T> operator |(bool a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a | b.F(et));
        }
        public static FuncOfT2Bool<T> operator ^(bool a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a ^ b.F(et));
        }
        public static FuncOfT2Bool<T> operator ==(bool a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a == b.F(et));
        }
        public static FuncOfT2Bool<T> operator !=(bool a, FuncOfT2Bool<T> b)
        {
            return new FuncOfT2Bool<T>(et => a != b.F(et));
        }

        public override bool Equals(object obj)
        {
            return F.Equals(obj);
        }
        public override int GetHashCode()
        {
            return F.GetHashCode();
        }
    }
}
