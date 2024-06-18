using System;
using Ex = System.Linq.Expressions.Expression;

namespace Aardvark.Base
{
    #region Ipol

    public static partial class Ipol
    {
        #region Generators

        public static Func<U, T> Generate<T, U>(
            T a, T b, Func<T, T, U, T> interpolator)
        {
            return Ipol<T, U>.Generate(a, b, interpolator);
        }

        public static Func<U, T> Generate<T, U>(
            T a, T b, T c, T d, Func<T, T, T, T, U, T> interpolator)
        {
            return Ipol<T, U>.Generate(a, b, c, d, interpolator);
        }

        #endregion
    }

    #endregion

    #region Ipol<T, U>

    public static class Ipol<T, U>
    {
        internal static Func<T, T, U, T> s_lerpInterpolator;

        #region Generators

        public static Func<U, T> Generate(
            T a, T b, Func<T, T, U, T> interpolator)
        {
            return x => interpolator(a, b, x);
        }

        public static Func<U, T> Generate(
            T a, T b, T c, T d, Func<T, T, T, T, U, T> interpolator)
        {
            return x => interpolator(a, b, c, d, x);
        }

        #endregion

        #region Linear Interpolation

        /// <summary>
        /// Gets a function that performs linear interpolation for values of T:
        /// (T a, T b, U t) => a * (1 - t) + b * t.
        /// For t = 0.0 the funtion will return a, and for t = 1.0 the function will return b.
        /// </summary>
        public static Func<T, T, U, T> Lerp
        {
            get
            {
                if (s_lerpInterpolator == null)
                {
                    var a = Ex.Parameter(typeof(T), "a");
                    var b = Ex.Parameter(typeof(T), "b");
                    var x = Ex.Parameter(typeof(U), "x");

                    var expr = Ex.Lambda<Func<T, T, U, T>>(
                        Ex.Add(
                            Ex.Multiply(a, Ex.Subtract(Ex.Constant(1.0), x)),
                            Ex.Multiply(b, x)
                            ),

                        a, b, x
                        );
                    s_lerpInterpolator = expr.Compile();
                }

                return s_lerpInterpolator;
            }
        }

        #endregion
    }

    #endregion
}
