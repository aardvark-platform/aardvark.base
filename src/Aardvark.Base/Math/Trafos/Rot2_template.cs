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
    //# Action and = () => Out(" && ");
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var v2t = "V2" + tc;
    //#   var v3t = "V3" + tc;
    //#   var v4t = "V4" + tc;
    //#   var type = "Rot2" + tc;
    //#   var type2 = "Rot2" + tc2;
    //#   var rot3t = "Rot3" + tc;
    //#   var scale2t = "Scale2" + tc;
    //#   var shift2t = "Shift2" + tc;
    //#   var affine2t = "Affine2" + tc;
    //#   var similarity2t = "Similarity2" + tc;
    //#   var euclidean2t = "Euclidean2" + tc;
    //#   var trafo2t = "Trafo2" + tc;
    //#   var m22t = "M22" + tc;
    //#   var m23t = "M23" + tc;
    //#   var m33t = "M33" + tc;
    //#   var m34t = "M34" + tc;
    //#   var m44t = "M44" + tc;
    //#   var eps = isDouble ? "1e-12" : "1e-5f";
    #region __type__

    /// <summary>
    /// Represents a 2D rotation counterclockwise around the origin.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>
    {
        [DataMember]
        public __ftype__ Angle;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="__type__"/> transformation given a rotation angle in radians.
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

        /// <summary>
        /// Constructs a <see cref="__type__"/> transformation from a <see cref="__type2__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ r)
        {
            Angle = (__ftype__)r.Angle;
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

        #region Rot / Vector Multiplication

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

        #endregion

        #region Rot / Matrix Multiplication

        //# for (int n = 2; n <= 3; n++) {
        //# for (int m = n; m <= (n+1) && m <= 3; m++) {
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

        /// <summary>
        /// Multiplies a <see cref="__mat__"/> with a <see cref="__type__"/> transformation (as a __m__x__m__ matrix).
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

        //# } }
        #endregion

        #region Rot / Shift, Scale Multiplication

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation with a <see cref="__shift2t__"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __euclidean2t__ operator *(__type__ a, __shift2t__ b)
            => new __euclidean2t__(a, a * b.V);

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation with a <see cref="__scale2t__"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __affine2t__ operator *(__type__ a, __scale2t__ b)
            => new __affine2t__((__m22t__)a * (__m22t__)b);

        #endregion

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks if 2 rotations are equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ rotation1, __type__ rotation2)
            => Rot.Distance(rotation1, rotation2) == 0;

        /// <summary>
        /// Checks if 2 rotations are not equal.
        /// </summary>
        /// <returns>Result of comparison.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ rotation1, __type__ rotation2)
            => Rot.Distance(rotation1, rotation2) != 0;

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__m22t__"/> matrix.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__m22t__(__m22t__ m)
        {
            return new __type__(m.GetRotation());
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__m33t__"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__m33t__(__m33t__ m, __ftype__ epsilon = __eps__)
        {
            if (!(/*#2.ForEach(j => {*/m.M2__j__.IsTiny(epsilon)/*# }, and);*/))
                throw new ArgumentException("Matrix contains perspective components.");

            if (!m.C2.XY.ApproximateEquals(__v2t__.Zero, epsilon))
                throw new ArgumentException("Matrix contains translational component.");

            if (m.M22.IsTiny(epsilon))
                throw new ArgumentException("Matrix is not homogeneous.");

            return From__m22t__(((__m22t__)m) / m.M22);
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__euclidean2t__"/>.
        /// The transformation <paramref name="euclidean"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__euclidean2t__(__euclidean2t__ euclidean, __ftype__ epsilon = __eps__)
        {
            if (!euclidean.Trans.ApproximateEquals(__v2t__.Zero, epsilon))
                throw new ArgumentException("Euclidean transformation contains translational component");

            return euclidean.Rot;
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__similarity2t__"/>.
        /// The transformation <paramref name="similarity"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__similarity2t__(__similarity2t__ similarity, __ftype__ epsilon = __eps__)
        {
            if (!similarity.Scale.ApproximateEquals(1, epsilon))
                throw new ArgumentException("Similarity transformation contains scaling component");

            if (!similarity.Trans.ApproximateEquals(__v2t__.Zero, epsilon))
                throw new ArgumentException("Similarity transformation contains translational component");

            return similarity.Rot;
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from an <see cref="__affine2t__"/>.
        /// The transformation <paramref name="affine"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__affine2t__(__affine2t__ affine, __ftype__ epsilon = __eps__)
            => From__m33t__((__m33t__)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__trafo2t__"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__trafo2t__(__trafo2t__ trafo, __ftype__ epsilon = __eps__)
            => From__m33t__(trafo.Forward, epsilon);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation with the specified angle in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromRadians(__ftype__ angleInRadians)
            => new __type__(angleInRadians);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation with the specified angle in degrees.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromDegrees(__ftype__ angleInDegrees)
            => new __type__(angleInDegrees.RadiansFromDegrees());

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
        public static explicit operator __euclidean2t__(__type__ r)
            => new __euclidean2t__(r, __v2t__.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __similarity2t__(__type__ r)
            => new __similarity2t__(1, r, __v2t__.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __affine2t__(__type__ r)
            => new __affine2t__((__m22t__)r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __trafo2t__(__type__ r)
            => new __trafo2t__((__m33t__)r, (__m33t__)r.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type2__(__type__ r)
            => new __type2__((__ftype2__)r.Angle);

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => Rot.Distance(this, other) == 0;

        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

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
            => Fun.AngleDistance(r1.Angle, r2.Angle);

        /// <summary>
        /// Returns the signed difference in radians between two <see cref="__type__"/> rotations.
        /// The result is within the range of [-Pi, Pi).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Difference(this __type__ r1, __type__ r2)
            => Fun.AngleDifference(r1.Angle, r2.Angle);

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
        {
            __ftype__ a = Fun.Cos(-rot.Angle);
            __ftype__ b = Fun.Sin(-rot.Angle);

            return new __v2t__(a * v.X + -b * v.Y, b * v.X + a * v.Y);
        }

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
