using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ex = System.Linq.Expressions.Expression;

namespace Aardvark.Base
{
    #region Ipol

    public static partial class Ipol
    {
        #region Generators

        public static Func<double, T> Generate<T>(
            T a, T b, Func<T, T, double, T> interpolator)
        {
            return Ipol<T>.Generate(a, b, interpolator);
        }

        public static Func<double, T> Generate<T>(
            T a, T b, T c, T d, Func<T, T, T, T, double, T> interpolator)
        {
            return Ipol<T>.Generate(a, b, c, d, interpolator);
        }

        #endregion

        #region Spherical Linear Interpolation

        /// <summary>
        /// Spherical linear interpolation.
        ///
        /// Assumes q1 and q2 are normalized and that t in [0,1].
        ///
        /// This method does interpolate along the shortest arc
        /// between q1 and q2.
        /// </summary>
        /// <param name="t">[0,1]</param>
        /// <returns>Interpolant</returns>
        public static Rot3d SlerpShortest(
            Rot3d a, Rot3d b, double t
            )
        {
            Rot3d q3 = b;
            double cosomega = Rot3d.Dot(a, q3);

            if (cosomega < 0.0)
            {
                cosomega = -cosomega;
                q3 = -q3;
            }

            if (cosomega >= 1.0)
            {
                // Special case: q1 and q2 are the same, so just return one of them.
                // This also catches the case where cosomega is very slightly > 1.0

                return a;
            }

            double sinomega = (1 - cosomega * cosomega).Sqrt();

            Rot3d result;

            if (sinomega * double.MaxValue > 1)
            {
                double omega = cosomega.Acos();
                double s1 = ((1.0 - t) * omega).Sin() / sinomega;
                double s2 = (t * omega).Sin() / sinomega;

                result = s1 * a + s2 * q3;
            }
            else if (cosomega > 0)
            {
                // omega == 0

                double s1 = 1.0 - t;
                double s2 = t;

                result = s1 * a + s2 * q3;
            }
            else
            {
                // omega == -pi

                result = new Rot3d(a.Z, -a.Y, a.X, -a.W);

                double s1 = ((0.5 - t) * Constant.Pi).Sin();
                double s2 = (t * Constant.Pi).Sin();

                result = s1 * a + s2 * result;
            }

            return result;
        }

        /// <summary>
        /// Spherical linear interpolation.
        ///
        /// Assumes q1 and q2 are normalized and that t in [0,1].
        ///
        /// This method does interpolate along the shortest arc
        /// between q1 and q2.
        /// </summary>
        /// <param name="t">[0,1]</param>
        /// <returns>Interpolant</returns>
        public static Rot3f SlerpShortest(
            Rot3f q1, Rot3f q2, double t
            )
        {
            Rot3f q3 = q2;
            float cosomega = Rot3f.Dot(q1, q3);

            if (cosomega < 0.0)
            {
                cosomega = -cosomega;
                q3 *= -1; //q3 = -q3;
            }

            if (cosomega >= 1.0)
            {
                // Special case: q1 and q2 are the same, so just return one of them.
                // This also catches the case where cosomega is very slightly > 1.0

                return q1;
            }

            double sinomega = System.Math.Sqrt(1 - cosomega * cosomega);

            Rot3f result = new Rot3f();

            if (sinomega * double.MaxValue > 1)
            {
                double omega = System.Math.Acos(cosomega);
                float s1 = (float)(System.Math.Sin((1.0 - t) * omega) / sinomega);
                float s2 = (float)(System.Math.Sin(t * omega) / sinomega);

                result = s1 * q1 + s2 * q3;
            }
            else if (cosomega > 0)
            {
                // omega == 0

                float s1 = 1.0f - (float)t;
                float s2 = (float)t;

                result = s1 * q1 + s2 * q3;
            }
            else
            {
                // omega == -pi

                result.X = -q1.Y;
                result.Y = q1.X;
                result.Z = -q1.W;
                result.W = q1.Z;

                float s1 = (float)System.Math.Sin((0.5 - t) * System.Math.PI);
                float s2 = (float)System.Math.Sin(t * System.Math.PI);

                result = s1 * q1 + s2 * result;
            }

            return result;
        }

        #endregion
    }

    #endregion

    #region Ipol<T>

    public static partial class Ipol<T>
    {
        internal static Func<T, T, double, T> s_lerpInterpolator;
        internal static Func<T, T, double, T> s_slerpInterpolator;

        #region Static Constructor

        static Ipol()
        {
            //pre-defines
            Ipol<Rot3f>.s_slerpInterpolator = Ipol.SlerpShortest;
            Ipol<Rot3d>.s_slerpInterpolator = Ipol.SlerpShortest;

            // pre-define lerp for float (implicit casts would create a double result)
            Ipol<float>.s_lerpInterpolator = (a, b, x) => a * (1 - (float)x) + b * (float)x;
        }

        #endregion

        #region Generators

        public static Func<double, T> Generate(
            T a, T b, Func<T, T, double, T> interpolator)
        {
            return x => interpolator(a, b, x);
        }

        public static Func<double, T> Generate(
            T a, T b, T c, T d, Func<T, T, T, T, double, T> interpolator)
        {
            return x => interpolator(a, b, c, d, x);
        }

        #endregion

        #region Linear Interpolation

        /// <summary>
        /// Gets a function that performs linear interpolation for values of T:
        /// (T a, T b, double t) => a * (1 - t) + b * t.
        /// For t = 0.0 the funtion will return a, and for t = 1.0 the function
        /// will return b.
        /// </summary>
        public static Func<T, T, double, T> Lerp
        {
            get
            {
                if (s_lerpInterpolator == null)
                {
                    var a = Ex.Parameter(typeof(T), "a");
                    var b = Ex.Parameter(typeof(T), "b");
                    var x = Ex.Parameter(typeof(double), "x");

                    var expr = Ex.Lambda<Func<T, T, double, T>>(
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

        #region Spherical Linear Interpolation

        /// <summary>
        /// Gets a lambda that performs spherical linear interpolation
        /// for values of T, where T is one of
        /// QuaternionD, QuaternionF, V3f, V3d.
        /// </summary>
        public static Func<T, T, double, T> Slerp
        {
            get { return s_slerpInterpolator; }
        }

        #endregion

    }

    #endregion
}
