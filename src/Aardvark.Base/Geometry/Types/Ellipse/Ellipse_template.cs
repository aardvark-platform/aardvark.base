using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# { // ELLIPSES
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var constant = isDouble ? "Constant" : "ConstantF";
    //# for (int d = 2; d < 4; d++) {
    //#     var vt = "V" + d + tc;
    //#     var vt2 = "V" + d + tc2;
    //#     var et = "Ellipse" + d + tc;
    //#     var et2 = "Ellipse" + d + tc2;
    #region __et__

    /// <summary>
    /// A __d__d ellipse is defined by its center/*# if (d == 3) { */, its plane normal,/*# } */
    /// and two half-axes.
    /// Note that in principle any two conjugate half-diameters
    /// can be used as axes, however some algorithms require that
    /// the major and minor half axes are known. By convention
    /// in this case, axis0 is the major half axis.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __et__
    {
        public __vt__ Center;
        //# if (d == 3) {
        public __vt__ Normal;
        //# }
        public __vt__ Axis0;
        public __vt__ Axis1;

        #region Constructors

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __et__(__vt__ center, /*# if (d == 3) { */__vt__ normal, /*# } */__vt__ axis0, __vt__ axis1)
        {
            Center = center;
            //# if (d == 3) {
            Normal = normal;
            //# }
            Axis0 = axis0;
            Axis1 = axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __et__(__et__ o)
        {
            Center = o.Center;
            //# if (d == 3) {
            Normal = o.Normal;
            //# }
            Axis0 = o.Axis0;
            Axis1 = o.Axis1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __et__(__et2__ o)
        {
            Center = (__vt__)o.Center;
            //# if (d == 3) {
            Normal = (__vt__)o.Normal;
            //# }
            Axis0 = (__vt__)o.Axis0;
            Axis1 = (__vt__)o.Axis1;
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// </summary>
        public static __et__ FromConjugateDiameters(__vt__ center, /*# if (d == 3) { */__vt__ normal, /*# } */__vt__ a, __vt__ b)
        {
            var ab = Vec.Dot(a, b);
            __ftype__ a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) return new __et__(center, /*# if (d == 3) { */normal, /*# } */a, b);
                else return new __et__(center, /*# if (d == 3) { */normal, /*# } */b, a);
            }
            else
            {
                var t = __half__ * Fun.Atan2(2 * ab, a2 - b2);
                __ftype__ ct = Fun.Cos(t), st = Fun.Sin(t);
                __vt__ v0 = a * ct + b * st, v1 = b * ct - a * st;
                if (v0.LengthSquared >= v1.LengthSquared) return new __et__(center, /*# if (d == 3) { */normal, /*# } */v0, v1);
                else return new __et__(center, /*# if (d == 3) { */normal, /*# } */v1, v0);
            }
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// Also computes the squared lengths of the major and minor
        /// half axis.
        /// </summary>
        public static __et__ FromConjugateDiameters(__vt__ center, /*# if (d == 3) { */__vt__ normal, /*# } */__vt__ a, __vt__ b,
                out __ftype__ major2, out __ftype__ minor2)
        {
            var ab = Vec.Dot(a, b);
            __ftype__ a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new __et__(center, /*# if (d == 3) { */normal, /*# } */a, b); }
                else { major2 = b2; minor2 = a2; return new __et__(center, /*# if (d == 3) { */normal, /*# } */b, a); }
            }
            else
            {
                var t = __half__ * Fun.Atan2(2 * ab, a2 - b2);
                __ftype__ ct = Fun.Cos(t), st = Fun.Sin(t);
                __vt__ v0 = a * ct + b * st, v1 = b * ct - a * st;
                a2 = v0.LengthSquared; b2 = v1.LengthSquared;
                if (a2 >= b2) { major2 = a2; minor2 = b2; return new __et__(center, /*# if (d == 3) { */normal, /*# } */v0, v1); }
                else { major2 = b2; minor2 = a2; return new __et__(center, /*# if (d == 3) { */normal, /*# } */v1, v0); }
            }
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __et__(__et2__ o)
            => new __et__(o);

        #endregion

        #region Constants

        public static __et__ Zero
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __et__(__vt__.Zero, /*# if (d == 3) { */__vt__.Zero, /*# } */__vt__.Zero, __vt__.Zero);
        }

        #endregion

        #region Operations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vt__ GetVector(__ftype__ alpha)
            => Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __vt__ GetPoint(__ftype__ alpha)
            => Center + Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);

        /// <summary>
        /// Perform the supplied action for each of count vectors from the center
        /// of the ellipse to the circumference. 
        /// </summary>
        public void ForEachVector(int count, Action<int, __vt__> index_vector_act)
        {
            __ftype__ d = __constant__.PiTimesTwo / count;
            __ftype__ a = Fun.Sin(d * __half__).Square() * 2, b = Fun.Sin(d); // init trig. recurrence
            __ftype__ ct = 1, st = 0;

            index_vector_act(0, Axis0);
            for (int i = 1; i < count; i++)
            {
                __ftype__ dct = a * ct + b * st, dst = a * st - b * ct; ;  // trig. recurrence
                ct -= dct; st -= dst;                                   // cos (t + d), sin (t + d)
                index_vector_act(i, Axis0 * ct + Axis1 * st);
            }
        }

        /// <summary>
        /// Get count points on the circumference of the ellipse.
        /// </summary>
        public __vt__[] GetPoints(int count)
        {
            var array = new __vt__[count];
            var c = Center;
            ForEachVector(count, (i, v) => array[i] = c + v);
            return array;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__et__ a, __et__ b) =>
            (a.Center == b.Center) &&
            //# if (d == 3) {
            (a.Normal == b.Normal) &&
            //# }
            (a.Axis0 == b.Axis0) &&
            (a.Axis1 == b.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__et__ a, __et__ b)
            => !(a == b);

        #endregion

        #region Overrides

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
            => HashCode.GetCombined(Center, /*# if (d == 3) { */Normal, /*# }*/Axis0, Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__et__ other) =>
            Center.Equals(other.Center) &&
            //# if (d == 3) {
            Normal.Equals(other.Normal) &&
            //# }
            Axis0.Equals(other.Axis0) &&
            Axis1.Equals(other.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
             => (other is __et__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
        {
            //# if (d == 3) {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", Center, Normal, Axis0, Axis1);
            //# } else {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Center, Axis0, Axis1);
            //# }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __et__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __et__(
                __vt__.Parse(x[0]),
                __vt__.Parse(x[1]),
                __vt__.Parse(x[2])/*# if (d == 3) { */,
                __vt__.Parse(x[3])/*# }*/
            );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__et__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __et__ a, __et__ b, __ftype__ tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            //# if (d == 3) {
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            //# }
            ApproximateEquals(a.Axis0, b.Axis0, tolerance) &&
            ApproximateEquals(a.Axis1, b.Axis1, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="__et__"/> are equal within
        /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __et__ a, __et__ b)
            => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
    }

    #endregion

    //# } // d
    //# } // ELLIPSES
    //# } // isDouble
}
