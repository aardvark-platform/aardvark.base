using System;
using System.Runtime.CompilerServices;
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

        #region Spherical Linear Interpolation

        /// <summary>
        /// Spherical linear interpolation.
        ///
        /// Assumes q1 and q2 are normalized and that t in [0,1].
        ///
        /// This method interpolates along the shortest arc between q1 and q2.
        /// </summary>
        [Obsolete("Use Rot.SlerpShortest or Quaternion.SlerpShortest instead")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3d SlerpShortest(Rot3d q1, Rot3d q2, double t)
            => Rot.SlerpShortest(q1, q2, t);

        /// <summary>
        /// Spherical linear interpolation.
        ///
        /// Assumes q1 and q2 are normalized and that t in [0,1].
        ///
        /// This method interpolates along the shortest arc between q1 and q2.
        /// </summary>
        [Obsolete("Use Rot.SlerpShortest or Quaternion.SlerpShortest instead")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Rot3f SlerpShortest(Rot3f q1, Rot3f q2, float t)
            => Rot.SlerpShortest(q1, q2, t);


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
