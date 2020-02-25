using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    //# Action comma = () => Out(", ");
    //# Action commaln = () => Out("," + Environment.NewLine);
    //# Action add = () => Out(" + ");
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var tc = isDouble ? "d" : "f";
    //#   var v2t = "V2" + tc;
    //#   var v3t = "V3" + tc;
    //#   var v4t = "V4" + tc;
    //#   var type = "Rot2" + tc;
    //#   var rot3t = "Rot3" + tc;
    //#   var scale3t = "Scale3" + tc;
    //#   var shift3t = "Shift3" + tc;
    //#   var m22t = "M22" + tc;
    //#   var m23t = "M23" + tc;
    //#   var m33t = "M33" + tc;
    //#   var m34t = "M34" + tc;
    //#   var m44t = "M44" + tc;
    #region __type__

    /// <summary>
    /// Represents a 2D rotation counterclockwise around the origin.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __type__
    {
        [DataMember]
        public __ftype__ Angle;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="__type__"/> transformation given an rotation angle in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__ angleInRadians)
        {
            Angle = angleInRadians;
        }

        /// <summary>
        /// Constructs a copy of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ r)
        {
            Angle = r.Angle;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity <see cref="__type__"/> transformation.
        /// </summary>
        public static __type__ Identity
        { 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(0);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the inverse of this <see cref="__type__"/> tranformation.
        /// </summary>
        public __type__ Inverse
        { 
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(-Angle);
        }

        #endregion

        #region Arithmetic operators

        /// <summary>
        /// Multiplies two <see cref="__type__"/> transformations.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ r0, __type__ r1)
        {
            return new __type__(r0.Angle + r1.Angle);
        }

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation with a <see cref="__v2t__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ operator *(__type__ rot, __v2t__ vec)
        {
            __ftype__ a = Fun.Cos(rot.Angle);
            __ftype__ b = Fun.Sin(rot.Angle);

            return new __v2t__(a * vec.X + -b * vec.Y, b * vec.X + a * vec.Y);
        }

        /// <summary>
        /// Multiplies a <see cref="__v2t__"/> with the inverse of a <see cref="__type__"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ operator *(__v2t__ vec, __type__ rot)
        {
            __ftype__ a = Fun.Cos(-rot.Angle);
            __ftype__ b = Fun.Sin(-rot.Angle);

            return new __v2t__(a * vec.X + -b * vec.Y, b * vec.X + a * vec.Y);
        }

        //# for (int n = 2; n <= 4; n++) {
        //# for (int m = n; m <= (n+1) && m <= 4; m++) {
        //#     var mat = "M" + n + m + tc;
        //#     var nsub2 = n - 2;
        //#     var msub2 = m - 2;
        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation (as a __n__x__n__ matrix) with a <see cref="__mat__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mat__ operator *(__type__ r, __mat__ m)
        {
            __ftype__ a = Fun.Cos(r.Angle);
            __ftype__ b = Fun.Sin(r.Angle);

            return new __mat__(/*# 2.ForEach(i => { m.ForEach(j => { */
                //# var x = (i == 0) ? "a" : "b";
                //# var y = (i == 0) ? "-b" :  "a";
                __x__ * m.M0__j__ + __y__ * m.M1__j__/*# }, comma); }, commaln); if (nsub2 > 0) {*/,
                /*# nsub2.ForEach(i => { var ip2 = i + 2; */
                /*# m.ForEach(j => { */m.M__ip2____j__/*# }, comma); }, comma); }*/);
        }

        //# if (n == m) {
        /// <summary>
        /// Multiplies a <see cref="__mat__"/> with a <see cref="__type__"/> transformation (as a __n__x__n__ matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __mat__ operator *(__mat__ m, __type__ r)
        {
            __ftype__ a = Fun.Cos(r.Angle);
            __ftype__ b = Fun.Sin(r.Angle);

            return new __mat__(/*# n.ForEach(i => { 2.ForEach(j => { */
                //# var x = (j == 0) ? "a" : "-b";
                //# var y = (j == 0) ? "b" :  "a";
                m.M__i__0 * __x__ + m.M__i__1 * __y__/*# }, comma); if (msub2 > 0) {*/,
                /*# msub2.ForEach(jj => { var jjp2 = jj + 2; */m.M__i____jjp2__/*# }, comma); } }, commaln);*/);
        }

        //# }
        //# } }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks if 2 rotations are equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ rotation1, __type__ rotation2)
        {
            return (rotation1.Angle == rotation2.Angle);
        }

        /// <summary>
        /// Checks if 2 rotations are not equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ rotation1, __type__ rotation2)
        {
            return !(rotation1.Angle == rotation2.Angle);
        }

        #endregion

        #region Static Creators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__m22t__(__m22t__ m)
        {
            return new __type__(m.RotationAngle());
        }

        #endregion

        #region Conversion Operators

        //# for (int n = 2; n <= 4; n++) {
        //# for (int m = n; m <= (n+1) && m <= 4; m++) {
        //#     var mat = "M" + n + m + tc;
        //#     string[,] val = new string[,] {{" a", "-b"}, {" b", " a"}};
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __mat__(__type__ r)
        {
            __ftype__ a = Fun.Cos(r.Angle);
            __ftype__ b = Fun.Sin(r.Angle);

            return new __mat__(/*# n.ForEach(i => { */
                /*# m.ForEach(j => {
                   var x = (i < 2 && j < 2) ? val[i, j] : ((i == j) ? " 1" : " 0");
                */__x__/*# }, comma); }, comma);*/);
        }

        //# } }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Euclidean2__tc__(__type__ r)
            => new Euclidean2__tc__(r, __v2t__.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine2__tc__(__type__ r)
            => new Affine2__tc__(r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo2__tc__(__type__ r)
        {
            var f = (__m33t__)r;
            var b = (__m33t__)r.Inverse;
            return new Trafo2__tc__(f, b);
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return Angle.GetHashCode();
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}]", Angle);
        }

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(
                __ftype__.Parse(x[0], CultureInfo.InvariantCulture)
            );
        }

        public override bool Equals(object other)
        {
            if (other is __type__ r)
                return Rot.Distance(this, r) == 0;
            return false;
        }

        #endregion
    }

    public static partial class Rot
    {
        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Inverse(__type__ rot)
            => rot.Inverse;

        /// <summary>
        /// Inverts a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref __type__ rot)
        {
            rot.Angle = -rot.Angle;
        }

        #endregion

        #region Distance

        /// <summary>
        /// Returns the absolute difference in radians between two <see cref="__type__"/> rotations.
        /// The result is within the range of [0, Pi].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Distance(this __type__ r1, __type__ r2)
        {
            //# var cast = isDouble ? "" : "(float)"; 
            var phi = Fun.Abs(r2.Angle - r1.Angle) % __cast__Constant.PiTimesTwo;
            return (phi > __cast__Constant.Pi) ? __cast__Constant.PiTimesTwo - phi : phi;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="__v2t__"/> vector by a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ Transform(this __type__ rot, __v2t__ v)
        {
            return rot * v;
        }

        /// <summary>
        /// Transforms a <see cref="__v3t__"/> vector by a <see cref="__type__"/> transformation.
        /// The z coordinate of the vector is unaffected.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ Transform(this __type__ rot, __v3t__ v)
        {
            __ftype__ a = Fun.Cos(rot.Angle);
            __ftype__ b = Fun.Sin(rot.Angle);

            return new __v3t__(a * v.X + -b * v.Y,
                        b * v.X + a * v.Y,
                        v.Z);
        }

        /// <summary>
        /// Transforms a <see cref="__v4t__"/> vector by a <see cref="__type__"/> transformation.
        /// The z and w coordinates of the vector are unaffected.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v4t__ Transform(this __type__ rot, __v4t__ v)
        {
            __ftype__ a = Fun.Cos(rot.Angle);
            __ftype__ b = Fun.Sin(rot.Angle);

            return new __v4t__(a * v.X + -b * v.Y,
                        b * v.X + a * v.Y,
                        v.Z, v.W);
        }

        /// <summary>
        /// Transforms a <see cref="__v2t__"/> vector by the inverse of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ InvTransform(this __type__ rot, __v2t__ v)
            => v * rot;

        /// <summary>
        /// Transforms a <see cref="__v3t__"/> vector by the inverse of a <see cref="__type__"/> transformation.
        /// The z coordinate of the vector is unaffected.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ InvTransform(this __type__ rot, __v3t__ v)
            => Transform(rot.Inverse, v);

        /// <summary>
        /// Transforms a <see cref="__v4t__"/> vector by the inverse of a <see cref="__type__"/> transformation.
        /// The z and w coordinates of the vector are unaffected.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v4t__ InvTransform(this __type__ rot, __v4t__ v)
            => Transform(rot.Inverse, v);

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ r0, __type__ r1)
        {
            return ApproximateEquals(r0, r1, Constant<__ftype__>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ r0, __type__ r1, __ftype__ tolerance)
        {
            return Rot.Distance(r0, r1) <= tolerance;
        }

        #endregion
    }

    #endregion

    //# }
}
