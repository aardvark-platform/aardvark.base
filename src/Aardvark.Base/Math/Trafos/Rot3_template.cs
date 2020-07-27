using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;
using System.Xml.Serialization;

namespace Aardvark.Base
{
    //# Action comma = () => Out(", ");
    //# Action commaln = () => Out("," + Environment.NewLine);
    //# Action add = () => Out(" + ");
    //# Action and = () => Out(" && ");
    //# Action or = () => Out(" || ");
    //# Action andLit = () => Out(" and ");
    //# var qfields = new[] {"W", "X", "Y", "Z"};
    //# var qfieldsL = new[] {"w", "x", "y", "z"};
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Rot3" + tc;
    //#   var type2 = "Rot3" + tc2;
    //#   var quatt = "Quaternion" + tc.ToUpper();
    //#   var v2t = "V2" + tc;
    //#   var v3t = "V3" + tc;
    //#   var v4t = "V4" + tc;
    //#   var rot2t = "Rot2" + tc;
    //#   var scale3t = "Scale3" + tc;
    //#   var shift3t = "Shift3" + tc;
    //#   var affine3t = "Affine3" + tc;
    //#   var similarity3t = "Similarity3" + tc;
    //#   var euclidean3t = "Euclidean3" + tc;
    //#   var trafo3t = "Trafo3" + tc;
    //#   var m22t = "M22" + tc;
    //#   var m23t = "M23" + tc;
    //#   var m33t = "M33" + tc;
    //#   var m34t = "M34" + tc;
    //#   var m44t = "M44" + tc;
    //#   var assertEps = isDouble ? "1e-10" : "1e-6f";
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var pi = isDouble ? "Constant.Pi" : "Constant.PiF";
    //#   var piHalf = isDouble ? "Constant.PiHalf" : "(float)Constant.PiHalf";
    //#   var assertNorm = "Debug.Assert(Fun.ApproximateEquals(NormSquared, 1, " + assertEps + "))";
    //#   var eps = isDouble ? "1e-12" : "1e-5f";
    //#   var getptr = "&" + qfields[0];
    #region __type__

    /// <summary>
    /// Represents a rotation in three dimensions using a unit quaternion.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__
    {
        /// <summary>
        /// Scalar (real) part of the quaternion.
        /// </summary>
        [DataMember]
        public __ftype__ W;

        /// <summary>
        /// First component of vector (imaginary) part of the quaternion.
        /// </summary>
        [DataMember]
        public __ftype__ X;

        /// <summary>
        /// Second component of vector (imaginary) part of the quaternion.
        /// </summary>
        [DataMember]
        public __ftype__ Y;

        /// <summary>
        /// Third component of vector (imaginary) part of the quaternion.
        /// </summary>
        [DataMember]
        public __ftype__ Z;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="__type__"/> transformation from the quaternion (w, (x, y, z)).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__ w, __ftype__ x, __ftype__ y, __ftype__ z)
        {
            W = w;
            X = x; Y = y; Z = z;
            __assertNorm__;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> transformation from the quaternion (w, (v.x, v.y, v.z)).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__ w, __v3t__ v)
        {
            W = w;
            X = v.X; Y = v.Y; Z = v.Z;
            __assertNorm__;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> transformation from the quaternion <paramref name="q"/>.
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__quatt__ q)
        {
            /*# qfields.ForEach(f => {*/__f__ = q.__f__; /*# });*/
            __assertNorm__;
        }

        /// <summary>
        /// Constructs a copy of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ r)
        {
            /*# qfields.ForEach(f => {*/__f__ = r.__f__; /*# });*/
            __assertNorm__;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> transformation from the quaternion (a[0], (a[1], a[2], a[3])).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__[] a)
        {
            W = a[0];
            X = a[1]; Y = a[2]; Z = a[3];
            __assertNorm__;
        }

        /// <summary>
        /// Constructs a <see cref="__type__"/> transformation from the quaternion (a[start], (a[start + 1], a[start + 2], a[start + 3])).
        /// The quaternion must be of unit length.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__ftype__[] a, int start)
        {
            W = a[start];
            X = a[start + 1]; Y = a[start + 2]; Z = a[start + 3];
            __assertNorm__;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the vector part (x, y, z) of this <see cref="__type__"/> unit quaternion.
        /// </summary>
        [XmlIgnore]
        public __v3t__ V
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return new __v3t__(X, Y, Z); }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set { X = value.X; Y = value.Y; Z = value.Z; }
        }

        /// <summary>
        /// Gets the squared norm (or squared length) of this <see cref="__type__"/>.
        /// May not be exactly 1, due to numerical inaccuracy.
        /// </summary>
        public __ftype__ NormSquared
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => /*# qfields.ForEach(f => {*/__f__ * __f__/*# }, add);*/;
        }

        /// <summary>
        /// Gets the norm (or length) of this <see cref="__type__"/>.
        /// May not be exactly 1, due to numerical inaccuracy. 
        /// </summary>
        public __ftype__ Norm
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => NormSquared.Sqrt();
        }

        /// <summary>
        /// Gets normalized (unit) quaternion from this <see cref="__type__"/>
        /// </summary>
        public __type__ Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var rs = new __type__(this);
                rs.Normalize();
                return rs;
            }
        }

        /// <summary>
        /// Gets the inverse of this <see cref="__type__"/> transformation.
        /// </summary>
        public __type__ Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                __assertNorm__;
                return new __type__(W, -X, -Y, -Z);
            }
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity <see cref="__type__"/>.
        /// </summary>
        public static __type__ Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(1, 0, 0, 0);
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Returns the component-wise negation of a <see cref="__type__"/> unit quaternion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator -(__type__ q)
            => new __type__(/*# qfields.ForEach(f => {*/-q.__f__/*# }, comma);*/);

        /// <summary>
        /// Multiplies two <see cref="__type__"/> transformations.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ operator *(__type__ a, __type__ b)
        {
            return new __type__(
                a.W * b.W - a.X * b.X - a.Y * b.Y - a.Z * b.Z,
                a.W * b.X + a.X * b.W + a.Y * b.Z - a.Z * b.Y,
                a.W * b.Y + a.Y * b.W + a.Z * b.X - a.X * b.Z,
                a.W * b.Z + a.Z * b.W + a.X * b.Y - a.Y * b.X);
        }

        #region Rot / Vector Multiplication

        /// <summary>
        /// Transforms a <see cref="__v3t__"/> vector by a <see cref="__type__"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ operator *(__type__ r, __v3t__ v)
        {
            var w = -r.X * v.X - r.Y * v.Y - r.Z * v.Z;
            var x = r.W * v.X + r.Y * v.Z - r.Z * v.Y;
            var y = r.W * v.Y + r.Z * v.X - r.X * v.Z;
            var z = r.W * v.Z + r.X * v.Y - r.Y * v.X;

            return new __v3t__(
                -w * r.X + x * r.W - y * r.Z + z * r.Y,
                -w * r.Y + y * r.W - z * r.X + x * r.Z,
                -w * r.Z + z * r.W - x * r.Y + y * r.X);
        }

        #endregion

        #region Rot / Matrix Multiplication

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation (as a 3x3 matrix) with a <see cref="__m33t__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __m33t__ operator *(__type__ rot, __m33t__ m)
        {
            return (__m33t__)rot * m;
        }

        /// <summary>
        /// Multiplies a <see cref="__m33t__"/> with a <see cref="__type__"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __m33t__ operator *(__m33t__ m, __type__ rot)
        {
            return m * (__m33t__)rot;
        }

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation (as a 4x4 matrix) with a <see cref="__m44t__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __m44t__ operator *(__type__ rot, __m44t__ m)
        {
            var r = (__m33t__)rot;
            return new __m44t__(/*# 3.ForEach(i => { 4.ForEach(j => {*/
                /*# 3.ForEach(k => {*/r.M__i____k__ * m.M__k____j__/*# }, add); }, comma); }, commaln);*/,

                /*# 4.ForEach(j => {*/m.M__3____j__/*# }, comma);*/);
        }

        /// <summary>
        /// Multiplies a <see cref="__m44t__"/> with a <see cref="__type__"/> transformation (as a 4x4 matrix) .
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __m44t__ operator *(__m44t__ m, __type__ rot)
        {
            var r = (__m33t__)rot;
            return new __m44t__(/*# 4.ForEach(i => { 3.ForEach(j => {*/
                /*# 3.ForEach(k => {*/m.M__i____k__ * r.M__k____j__/*# }, add); }, comma);*/,
                m.M__i____3__/*# }, commaln);*/);
        }

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation (as a 3x3 matrix) with a <see cref="__m34t__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __m34t__ operator *(__type__ rot, __m34t__ m)
        {
            return (__m33t__)rot * m;
        }

        /// <summary>
        /// Multiplies a <see cref="__m34t__"/> with a <see cref="__type__"/> transformation (as a 4x4 matrix) .
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __m34t__ operator *(__m34t__ m, __type__ rot)
        {
            var r = (__m33t__)rot;
            return new __m34t__(/*# 3.ForEach(i => { 3.ForEach(j => {*/
                /*# 3.ForEach(k => {*/m.M__i____k__ * r.M__k____j__/*# }, add); }, comma);*/,
                m.M__i____3__/*# }, commaln);*/);
        }

        #endregion

        #region Rot / Quaternion arithmetics

        /// <summary>
        /// Returns the sum of a <see cref="__type__"/> and a <see cref="__quatt__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator +(__type__ r, __quatt__ q)
            => new __quatt__(/*# qfields.ForEach(f => {*/r.__f__ + q.__f__/*# }, comma);*/);

        /// <summary>
        /// Returns the sum of a <see cref="__quatt__"/> and a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator +(__quatt__ q, __type__ r)
            => new __quatt__(/*# qfields.ForEach(f => {*/q.__f__ + r.__f__/*# }, comma);*/);

        /// <summary>
        /// Returns the sum of a <see cref="__type__"/> and a real scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator +(__type__ r, __ftype__ s)
            => new __quatt__(r.W + s, r.X, r.Y, r.Z);

        /// <summary>
        /// Returns the sum of a real scalar and a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator +(__ftype__ s, __type__ r)
            => new __quatt__(r.W + s, r.X, r.Y, r.Z);

        /// <summary>
        /// Returns the difference between a <see cref="__type__"/> and a <see cref="__quatt__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator -(__type__ r, __quatt__ q)
            => new __quatt__(/*# qfields.ForEach(f => {*/r.__f__ - q.__f__/*# }, comma);*/);

        /// <summary>
        /// Returns the difference between a <see cref="__quatt__"/> and a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator -(__quatt__ q, __type__ r)
            => new __quatt__(/*# qfields.ForEach(f => {*/q.__f__ - r.__f__/*# }, comma);*/);

        /// <summary>
        /// Returns the difference between a <see cref="__type__"/> and a real scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator -(__type__ r, __ftype__ s)
            => new __quatt__(r.W - s, r.X, r.Y, r.Z);

        /// <summary>
        /// Returns the difference between a real scalar and a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator -(__ftype__ s, __type__ r)
            => new __quatt__(s - r.W, -r.X, -r.Y, -r.Z);

        /// <summary>
        /// Returns the product of a <see cref="__type__"/> and a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator *(__type__ r, __ftype__ s)
            => new __quatt__(/*# qfields.ForEach(f => {*/r.__f__ * s/*# }, comma);*/);

        /// <summary>
        /// Returns the product of a scalar and a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator *(__ftype__ s, __type__ r)
            => new __quatt__(/*# qfields.ForEach(f => {*/r.__f__ * s/*# }, comma);*/);

        /// <summary>
        /// Multiplies a <see cref="__type__"/> with a <see cref="__quatt__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator *(__type__ r, __quatt__ q)
        {
            return new __quatt__(
                r.W * q.W - r.X * q.X - r.Y * q.Y - r.Z * q.Z,
                r.W * q.X + r.X * q.W + r.Y * q.Z - r.Z * q.Y,
                r.W * q.Y + r.Y * q.W + r.Z * q.X - r.X * q.Z,
                r.W * q.Z + r.Z * q.W + r.X * q.Y - r.Y * q.X);
        }

        /// <summary>
        /// Multiplies a <see cref="__quatt__"/> with a <see cref="__type__"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator *(__quatt__ q, __type__ r)
        {
            return new __quatt__(
                q.W * r.W - q.X * r.X - q.Y * r.Y - q.Z * r.Z,
                q.W * r.X + q.X * r.W + q.Y * r.Z - q.Z * r.Y,
                q.W * r.Y + q.Y * r.W + q.Z * r.X - q.X * r.Z,
                q.W * r.Z + q.Z * r.W + q.X * r.Y - q.Y * r.X);
        }

        /// <summary>
        /// Divides a <see cref="__type__"/> by a <see cref="__quatt__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator /(__type__ r, __quatt__ q)
            => r * q.Inverse;

        /// <summary>
        /// Divides a <see cref="__quatt__"/> by a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator /(__quatt__ q, __type__ r)
            => q * r.Inverse;

        /// <summary>
        /// Divides a <see cref="__type__"/> by a scalar.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator /(__type__ r, __ftype__ s)
            => new __quatt__(/*# qfields.ForEach(f => {*/r.__f__ / s/*# }, comma);*/);

        /// <summary>
        /// Divides a scalar by a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __quatt__ operator /(__ftype__ s, __type__ r)
            => new __quatt__(/*# qfields.ForEach(f => {*/s / r.__f__/*# }, comma);*/);

        #endregion

        #region Rot / Shift, Scale Multiplication

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation with a <see cref="__shift3t__"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __euclidean3t__ operator *(__type__ a, __shift3t__ b)
            => new __euclidean3t__(a, a * b.V);

        /// <summary>
        /// Multiplies a <see cref="__type__"/> transformation with a <see cref="__scale3t__"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __affine3t__ operator *(__type__ a, __scale3t__ b)
            => new __affine3t__((__m33t__)a * (__m33t__)b);

        #endregion

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks whether two <see cref="__type__"/> transformations are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ r0, __type__ r1)
            => Rot.Distance(r0, r1) == 0;

        /// <summary>
        /// Checks whether two <see cref="__type__"/> transformations are different.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ r0, __type__ r1)
            => !(r0 == r1);

        #endregion

        #region Static Creators
        
        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from an orthonormal basis.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromFrame(__v3t__ x, __v3t__ y, __v3t__ z)
        {
            return From__m33t__(__m33t__.FromCols(x, y, z));
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a Rodrigues axis-angle vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ FromAngleAxis(__v3t__ angleAxis)
        {
            __ftype__ theta2 = angleAxis.LengthSquared;
            if (theta2 > Constant<__ftype__>.PositiveTinyValue)
            {
                var theta = Fun.Sqrt(theta2);
                var thetaHalf = theta / 2;
                var k = Fun.Sin(thetaHalf) / theta;
                return new __type__(Fun.Cos(thetaHalf), k * angleAxis);
            }
            else
                return new __type__(1, 0, 0, 0);
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static __type__ From__m33t__(__m33t__ m, __ftype__ epsilon = (__ftype__)1e-6)
        {
            if (!m.IsOrthonormal(epsilon)) throw new ArgumentException("Matrix is not orthonormal.");
            var tr = m.M00 + m.M11 + m.M22;

            if (tr > 0)
            {
                __ftype__ s = (tr + 1).Sqrt() * 2;
                __ftype__ x = (m.M21 - m.M12) / s;
                __ftype__ y = (m.M02 - m.M20) / s;
                __ftype__ z = (m.M10 - m.M01) / s;
                __ftype__ w = s / 4;
                return new __type__(new __quatt__(w, x, y, z).Normalized);
            }
            else if (m.M00 > m.M11 && m.M00 > m.M22)
            {
                __ftype__ s = Fun.Sqrt(1 + m.M00 - m.M11 - m.M22) * 2;
                __ftype__ x = s / 4;
                __ftype__ y = (m.M01 + m.M10) / s;
                __ftype__ z = (m.M02 + m.M20) / s;
                __ftype__ w = (m.M21 - m.M12) / s;
                return new __type__(new __quatt__(w, x, y, z).Normalized);
            }
            else if (m.M11 > m.M22)
            {
                __ftype__ s = Fun.Sqrt(1 + m.M11 - m.M00 - m.M22) * 2;
                __ftype__ x = (m.M01 + m.M10) / s;
                __ftype__ y = s / 4;
                __ftype__ z = (m.M12 + m.M21) / s;
                __ftype__ w = (m.M02 - m.M20) / s;
                return new __type__(new __quatt__(w, x, y, z).Normalized);
            }
            else
            {
                __ftype__ s = Fun.Sqrt(1 + m.M22 - m.M00 - m.M11) * 2;
                __ftype__ x = (m.M02 + m.M20) / s;
                __ftype__ y = (m.M12 + m.M21) / s;
                __ftype__ z = s / 4;
                __ftype__ w = (m.M10 - m.M01) / s;
                return new __type__(new __quatt__(w, x, y, z).Normalized);
            }
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__m44t__"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__m44t__(__m44t__ m, __ftype__ epsilon = __eps__)
        {
            if (!(/*#3.ForEach(j => {*/m.M3__j__.IsTiny(epsilon)/*# }, and);*/))
                throw new ArgumentException("Matrix contains perspective components.");

            if (!m.C3.XYZ.ApproximateEquals(__v3t__.Zero, epsilon))
                throw new ArgumentException("Matrix contains translational component.");

            if (m.M33.IsTiny(epsilon))
                throw new ArgumentException("Matrix is not homogeneous.");

            return From__m33t__(((__m33t__)m) / m.M33);
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__euclidean3t__"/>.
        /// The transformation <paramref name="euclidean"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__euclidean3t__(__euclidean3t__ euclidean, __ftype__ epsilon = __eps__)
        {
            if (!euclidean.Trans.ApproximateEquals(__v3t__.Zero, epsilon))
                throw new ArgumentException("Euclidean transformation contains translational component");

            return euclidean.Rot;
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__similarity3t__"/>.
        /// The transformation <paramref name="similarity"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__similarity3t__(__similarity3t__ similarity, __ftype__ epsilon = __eps__)
        {
            if (!similarity.Scale.ApproximateEquals(1, epsilon))
                throw new ArgumentException("Similarity transformation contains scaling component");

            if (!similarity.Trans.ApproximateEquals(__v3t__.Zero, epsilon))
                throw new ArgumentException("Similarity transformation contains translational component");

            return similarity.Rot;
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from an <see cref="__affine3t__"/>.
        /// The transformation <paramref name="affine"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__affine3t__(__affine3t__ affine, __ftype__ epsilon = __eps__)
            => From__m44t__((__m44t__)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation from a <see cref="__trafo3t__"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a rotation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ From__trafo3t__(__trafo3t__ trafo, __ftype__ epsilon = __eps__)
            => From__m44t__(trafo.Forward, epsilon);

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation representing a rotation around 
        /// an axis by an angle in radians.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Rotation(__v3t__ normalizedAxis, __ftype__ angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            var halfAngleSin = halfAngle.Sin();

            return new __type__(halfAngle.Cos(), normalizedAxis * halfAngleSin);
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation representing a rotation around 
        /// an axis by an angle in degrees.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationInDegrees(__v3t__ normalizedAxis, __ftype__ angleInDegrees)
            => Rotation(normalizedAxis, angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation representing a rotation from one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotateInto(__v3t__ from, __v3t__ into)
        {
            var d = Vec.Dot(from, into);

            if (d.ApproximateEquals(-1))
                return new __type__(0, from.AxisAlignedNormal());
            else
            {
                __quatt__ q = new __quatt__(d + 1, Vec.Cross(from, into));
                return new __type__(q.Normalized);
            }
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation by <paramref name="angleInRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationX(__ftype__ angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new __type__(halfAngle.Cos(), new __v3t__(halfAngle.Sin(), 0, 0));
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation by <paramref name="angleInDegrees"/> degrees around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationXInDegrees(__ftype__ angleInDegrees)
            => RotationX(angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation by <paramref name="angleInRadians"/> radians around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationY(__ftype__ angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new __type__(halfAngle.Cos(), new __v3t__(0, halfAngle.Sin(), 0));
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation by <paramref name="angleInDegrees"/> degrees around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationYInDegrees(__ftype__ angleInDegrees)
            => RotationY(angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation by <paramref name="angleInRadians"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationZ(__ftype__ angleInRadians)
        {
            var halfAngle = angleInRadians / 2;
            return new __type__(halfAngle.Cos(), new __v3t__(0, 0, halfAngle.Sin()));
        }

        /// <summary>
        /// Creates a <see cref="__type__"/> transformation by <paramref name="angleInDegrees"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationZInDegrees(__ftype__ angleInDegrees)
            => RotationZ(angleInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in radians. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEuler(__ftype__ rollInRadians, __ftype__ pitchInRadians, __ftype__ yawInRadians)
        {
            __ftype__ rollHalf = rollInRadians / 2;
            __ftype__ cr = Fun.Cos(rollHalf);
            __ftype__ sr = Fun.Sin(rollHalf);
            __ftype__ pitchHalf = pitchInRadians / 2;
            __ftype__ cp = Fun.Cos(pitchHalf);
            __ftype__ sp = Fun.Sin(pitchHalf);
            __ftype__ yawHalf = yawInRadians / 2;
            __ftype__ cy = Fun.Cos(yawHalf);
            __ftype__ sy = Fun.Sin(yawHalf);

            return new __type__(
                cy * cp * cr + sy * sp * sr,
                cy * cp * sr - sy * sp * cr,
                sy * cp * sr + cy * sp * cr,
                sy * cp * cr - cy * sp * sr);
        }

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in degrees. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEulerInDegrees(__ftype__ rollInDegrees, __ftype__ pitchInDegrees, __ftype__ yawInDegrees)
            => RotationEuler(
                rollInDegrees.RadiansFromDegrees(),
                pitchInDegrees.RadiansFromDegrees(),
                yawInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in radians.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEuler(__v3t__ rollPitchYawInRadians)
            => RotationEuler(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in degrees.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ RotationEulerInDegrees(__v3t__ rollPitchYawInDegrees)
            => RotationEulerInDegrees(rollPitchYawInDegrees.X, rollPitchYawInDegrees.Y, rollPitchYawInDegrees.Z);

        #endregion

        #region Conversion

        //# for (int n = 3; n <= 4; n++) {
        //# for (int m = n; m <= (n+1) && m <= 4; m++) {
        //#     var mat = "M" + n + m + tc;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __mat__(__type__ r)
        {
            //speed up by computing the multiplications only once (each is used 2 times below)
            __ftype__ xx = r.X * r.X;
            __ftype__ yy = r.Y * r.Y;
            __ftype__ zz = r.Z * r.Z;
            __ftype__ xy = r.X * r.Y;
            __ftype__ xz = r.X * r.Z;
            __ftype__ yz = r.Y * r.Z;
            __ftype__ xw = r.X * r.W;
            __ftype__ yw = r.Y * r.W;
            __ftype__ zw = r.Z * r.W;
            return new __mat__(
                1 - 2 * (yy + zz),
                2 * (xy - zw),
                2 * (xz + yw),
                /*# if (m == 4) {*/0,
                /*# }*/
                2 * (xy + zw),
                1 - 2 * (zz + xx),
                2 * (yz - xw),
                /*# if (m == 4) {*/0,
                /*# }*/
                2 * (xz - yw),
                2 * (yz + xw),
                1 - 2 * (yy + xx)/*# if (m == 4) {*/,
                0/*# } if (n == 4) {*/,

                /*# m.ForEach(i => { var x = (i == m - 1) ? 1 : 0; */__x__/*# }, comma); }*/);
        }

        //# } }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __ftype__[](__type__ r)
        {
            __ftype__[] array = new __ftype__[__qfields.Length__];
            /*# qfields.ForEach((f, i) => {*/array[__i__] = r.__f__;
            /*# });*/return array;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __euclidean3t__(__type__ r)
            => new __euclidean3t__(r, __v3t__.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __similarity3t__(__type__ r)
            => new __similarity3t__(1, r, __v3t__.Zero);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __affine3t__(__type__ r)
            => new __affine3t__((__m33t__)r);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __trafo3t__(__type__ r)
            => new __trafo3t__((__m44t__)r, (__m44t__)r.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type2__(__type__ r)
            => new __type2__(/*# qfields.ForEach(f => {*/(__ftype2__)r.__f__/*# }, comma);*/);

        #endregion

        #region Indexing

        /// <summary>
        /// Gets or sets the <paramref name="i"/>-th component of the <see cref="__type__"/> unit quaternion with components (W, (X, Y, Z)).
        /// </summary>
        public unsafe __ftype__ this[int i]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                fixed (__ftype__* ptr = __getptr__) { return ptr[i]; }
            }
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set
            {
                fixed (__ftype__* ptr = __getptr__) { ptr[i] = value; }
            }
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(W, V);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => Rot.Distance(this, other) == 0;

        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", W, V);
        }

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(__ftype__.Parse(x[0], CultureInfo.InvariantCulture), __v3t__.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Rot
    {
        #region Dot

        /// <summary> 
        /// Returns the dot product of two <see cref="__type__"/> unit quaternions.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Dot(this __type__ a, __type__ b)
        {
            return a.W * b.W + a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }

        #endregion

        #region Distance

        /// <summary>
        /// Returns the absolute difference in radians between two <see cref="__type__"/> rotations.
        /// The result is within the range of [0, Pi].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ DistanceFast(this __type__ r1, __type__ r2)
        {
            var d = Dot(r1, r2);
            return 2 * Fun.AcosClamped((d < 0) ? -d : d);
        }

        /// <summary>
        /// Returns the absolute difference in radians between two <see cref="__type__"/> rotations
        /// using a numerically stable algorithm.
        /// The result is within the range of [0, Pi].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ftype__ Distance(this __type__ r1, __type__ r2)
        {
            var q = r1.Inverse * r2;
            return 2 * Fun.Atan2(q.V.Length, (q.W < 0) ? -q.W : q.W);
        }

        #endregion

        #region Invert, Normalize

        /// <summary>
        /// Returns the inverse of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Inverse(__type__ r)
            => r.Inverse;

        /// <summary>
        /// Inverts the given <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref __type__ r)
        {
            r.X = -r.X;
            r.Y = -r.Y;
            r.Z = -r.Z;
        }

        /// <summary>
        /// Returns a normalized copy of a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Normalized(__type__ r)
            => r.Normalized;

        /// <summary>
        /// Normalizes a <see cref="__type__"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref __type__ r)
        {
            var norm = r.Norm;
            if (norm > 0)
            {
                var scale = 1 / norm;
                /*# qfields.ForEach(f => {*/
                r.__f__ *= scale;/*# });*/
            }
        }

        #endregion

        #region Conversion

        /// <summary>
        /// Returns the Rodrigues angle-axis vector of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ ToAngleAxis(this __type__ r)
        {
            var sinTheta2 = r.V.LengthSquared;
            if (sinTheta2 > Constant<__ftype__>.PositiveTinyValue)
            {
                __ftype__ sinTheta = Fun.Sqrt(sinTheta2);
                __ftype__ cosTheta = r.W;
                __ftype__ twoTheta = 2 * (cosTheta < 0 ? Fun.Atan2(-sinTheta, -cosTheta)
                                                    : Fun.Atan2(sinTheta, cosTheta));
                return r.V * (twoTheta / sinTheta);
            }
            else
                return __v3t__.Zero;
        }

        /// <summary>
        /// Returns the axis-angle representation of a <see cref="__type__"/> transformation.
        /// </summary>
        /// <param name="r">A <see cref="__type__"/> transformation.</param>
        /// <param name="axis">Output of normalized axis of rotation.</param>
        /// <param name="angleInRadians">Output of angle of rotation in radians about axis (Right Hand Rule).</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ToAxisAngle(this __type__ r, ref __v3t__ axis, ref __ftype__ angleInRadians)
        {
            angleInRadians = 2 * Fun.Acos(r.W);
            var s = Fun.Sqrt(1 - r.W * r.W); // assuming quaternion normalised then w is less than 1, so term always positive.
            if (s < 0.001)
            { // test to avoid divide by zero, s is always positive due to sqrt
                // if s close to zero then direction of axis not important
                axis.X = r.X; // if it is important that axis is normalised then replace with x=1; y=z=0;
                axis.Y = r.Y;
                axis.Z = r.Z;
            }
            else
            {
                axis.X = r.X / s; // normalise axis
                axis.Y = r.Y / s;
                axis.Z = r.Z / s;
            }
        }

        #endregion

        #region Euler Angles

        /// <summary>
        /// Returns the Euler-Angles from the given <see cref="__type__"/> as a <see cref="__v3t__"/> vector.
        /// The vector components represent [roll (X), pitch (Y), yaw (Z)] with rotation order is Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ GetEulerAngles(this __type__ r)
        {
            var test = r.W * r.Y - r.X * r.Z;
            if (test > __half__ - Constant<__ftype__>.PositiveTinyValue) // singularity at north pole
            {
                return new __v3t__(
                    2 * Fun.Atan2(r.X, r.W),
                    __piHalf__,
                    0);
            }
            if (test < -__half__ + Constant<__ftype__>.PositiveTinyValue) // singularity at south pole
            {
                return new __v3t__(
                    2 * Fun.Atan2(r.X, r.W),
                    -__piHalf__,
                    0);
            }
            // From Wikipedia, conversion between quaternions and Euler angles.
            return new __v3t__(
                        Fun.Atan2(2 * (r.W * r.X + r.Y * r.Z),
                                  1 - 2 * (r.X * r.X + r.Y * r.Y)),
                        Fun.AsinClamped(2 * test),
                        Fun.Atan2(2 * (r.W * r.Z + r.X * r.Y),
                                  1 - 2 * (r.Y * r.Y + r.Z * r.Z)));
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Transforms a <see cref="__v3t__"/> vector by a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ Transform(this __type__ r, __v3t__ v)
            => r * v;

        /// <summary>
        /// Transforms a <see cref="__v3t__"/> vector by the inverse of a <see cref="__type__"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ InvTransform(this __type__ r, __v3t__ v)
        {
            var w = r.X * v.X + r.Y * v.Y + r.Z * v.Z;
            var x = r.W * v.X - r.Y * v.Z + r.Z * v.Y;
            var y = r.W * v.Y - r.Z * v.X + r.X * v.Z;
            var z = r.W * v.Z - r.X * v.Y + r.Y * v.X;

            return new __v3t__(
                w * r.X + x * r.W + y * r.Z - z * r.Y,
                w * r.Y + y * r.W + z * r.X - x * r.Z,
                w * r.Z + z * r.W + x * r.Y - y * r.X
                );
        }

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
