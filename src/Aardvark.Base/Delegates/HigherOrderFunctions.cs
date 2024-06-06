using System;
using System.Collections.Generic;
using System.Linq;
using static System.Math;

namespace Aardvark.Base
{
    public class ActionTable<T> : Dict<T, Action>
    { }

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

        public override readonly bool Equals(object obj)
        {
            return F.Equals(obj);
        }
        public override readonly int GetHashCode()
        {
            return F.GetHashCode();
        }
    }
}
