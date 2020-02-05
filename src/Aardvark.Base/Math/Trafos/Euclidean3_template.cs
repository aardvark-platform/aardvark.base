using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var s3f = isDouble ? "3d" : "3f";
    //#   var s4f = isDouble ? "4d" : "4f";
    //#   var v3t = isDouble ? "V3d" : "V3f";
    //#   var r3t = isDouble ? "Rot3d" : "Rot3f";
    //#   var e3t = isDouble ? "Euclidean3d" : "Euclidean3f";
    //#   var eps = isDouble ? "1e-12" : "1e-5f";
    #region __e3t__

    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in 3D that is composed of a 
    /// 3D rotation Rot and a subsequent translation by a 3D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __e3t__ : IValidity
    {
        [DataMember]
        public __r3t__ Rot;
        [DataMember]
        public __v3t__ Trans;

        #region Constructors

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public __e3t__(__r3t__ rot, __v3t__ trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public __e3t__(M3__s3f__ rot, __v3t__ trans, __ft__ epsilon = __eps__)
        {
            Rot = __r3t__.FromM3__s3f__(rot, epsilon);
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a matrix <paramref name="m"/>.
        /// </summary>
        public __e3t__(M4__s4f__ m, __ft__ epsilon = __eps__)
            : this(((M3__s3f__)m) / m.M33, m.C3.XYZ / m.M33, epsilon)
        {
            if (!(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");
            if (m.M33.IsTiny(epsilon)) throw new ArgumentException("Matrix is not homogeneous.");
        }

        /// <summary>
        /// Creates a rigid transformation from a trafo <paramref name="trafo"/>.
        /// </summary>
        public __e3t__(Trafo3d trafo, __ft__ epsilon = __eps__)
            : this((M4__s4f__)trafo.Forward, epsilon)
        {
        }


        #endregion

        #region Constants

        /// <summary>
        /// Identity (__r3t__.Identity, __v3t__.Zero)
        /// </summary>
        public static __e3t__ Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __e3t__(__r3t__.Identity, __v3t__.Zero);
        }

        #endregion

        #region Properties

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        #endregion

        #region Euclidean Transformation Arithmetics

        /// <summary>
        /// Returns a new version of this Euclidean transformation with a normalized rotation quaternion.
        /// </summary>
        public __e3t__ Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __e3t__(Rot.Normalized, Trans);
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this Euclidean transformation.
        /// [Rot^T,-Rot^T Trans]
        /// </summary>
        public __e3t__ Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newR = Rot.Inverse;
                return new __e3t__(newR, -newR.Transform(Trans));
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies 2 Euclidean transformations.
        /// This concatenates the two rigid transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __e3t__ operator *(__e3t__ a, __e3t__ b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new __e3t__(a.Rot * b.Rot, a.Trans + a.Rot.Transform(b.Trans));
        }

#if false //// [todo ISSUE 20090421 andi : andi] check if these are really necessary and comment them what they really do.
        /// <summary>
        /// </summary>
        public static __v3t__ operator *(__e3t__ rot, V2f v)
        {
            return (M3__s3f__)rot * v;
        }

        /// <summary>
        /// </summary>
        public static __v3t__ operator *(__e3t__ rot, __v3t__ v)
        {
            return (M3__s3f__)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V__s4f__ operator *(__e3t__ rot, V__s4f__ v)
        {
            return (M3__s3f__)rot * v;
        }

        /// <summary>
        /// </summary>
        public static M3__s3f__ operator *(__e3t__ r3, Rot2f r2)
        {
            M3__s3f__ m33 = (M3__s3f__)r3;
            M22f m22 = (M22f)r2;
            return new M3__s3f__(
                m33.M00 * m22.M00 + m33.M01 * m22.M10,
                m33.M00 * m22.M01 + m33.M01 * m22.M11,
                m33.M02,

                m33.M10 * m22.M00 + m33.M11 * m22.M10,
                m33.M10 * m22.M01 + m33.M11 * m22.M11,
                m33.M12,

                m33.M20 * m22.M00 + m33.M21 * m22.M10,
                m33.M20 * m22.M01 + m33.M21 * m22.M11,
                m33.M22
                );

        }
#endif
        // [todo ISSUE 20090427 andi : andi] add operator * for all other Trafo structs.
        /*
                public static M3__s3f__ operator *(__e3t__ r, __r3t__ m)
                {
                    return (M3__s4f__)r * m;
                }

                public static M3__s3f__ operator *(__e3t__ r, Scale__s3f__ m)
                {
                    return (M3__s4f__)r * m;
                }
        */
        // [todo ISSUE 20090427 andi : andi] this is again a __e3t__.
        // [todo ISSUE 20090427 andi : andi] __r3t__ * Shift__s3f__ should return a __e3t__!
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__s4f__ operator *(__e3t__ r, Shift__s3f__ m)
        {
            return (M3__s4f__)r * m;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M3__s4f__ operator *(M3__s3f__ m, __e3t__ r)
        {
            return M3__s4f__.Multiply(m, (M3__s4f__)r);
        }
        #endregion

        #region Comparison Operators

        public static bool operator ==(__e3t__ r0, __e3t__ r1)
        {
            return r0.Rot == r1.Rot && r0.Trans == r1.Trans;
        }

        public static bool operator !=(__e3t__ r0, __e3t__ r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Conversion

        // [todo ISSUE 20090421 andi] caching of the Matrix would greatly improve performance
        public static explicit operator M3__s4f__(__e3t__ r)
        {
            M3__s4f__ rv = (M3__s4f__)r.Rot;
            rv.C3 = r.Trans;
            return rv;
        }

        public static explicit operator M4__s4f__(__e3t__ r)
        {
            M4__s4f__ rv = (M4__s4f__)r.Rot;
            rv.C3 = r.Trans.XYZI;
            return rv;
        }

        public static explicit operator Similarity__s3f__(__e3t__ r)
        {
            return new Similarity__s3f__(1, r);
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        public override bool Equals(object other)
        {
            return (other is __e3t__) ? (this == (__e3t__)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Rot, Trans);
        }

        public static __e3t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __e3t__(__r3t__.Parse(x[0]), __v3t__.Parse(x[1]));
        }

        #endregion

    }

    public static partial class Euclidean
    {
        #region Transform

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ TransformDir(this __e3t__ r, __v3t__ v)
        {
            return r.Rot.Transform(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ TransformPos(this __e3t__ r, __v3t__ p)
        {
            return r.Rot.Transform(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ InvTransformDir(this __e3t__ r, __v3t__ v)
        {
            return r.Rot.InvTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v3t__ InvTransformPos(this __e3t__ r, __v3t__ p)
        {
            return r.Rot.InvTransform(p - r.Trans);
        }

        /// <summary>
        /// Returns a new Euclidean transformation by transforming self by a Trafo t.
        /// Note: This is not a concatenation. 
        /// t is fully applied to the Translation and Rotation,
        /// but the scale is not reflected in the resulting Euclidean transformation.
        /// </summary>
        // [todo ISSUE 20090810 andi : andi] Rethink this notation. Maybe write Transformed methods for all transformations.
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __e3t__ Transformed(this __e3t__ self, Similarity__s3f__ t)
        {
            return new __e3t__(t.Rot * self.Rot, t.TransformPos(self.Trans));
        }

        #endregion

        #region Normalize and invert

        /// <summary>
        /// Normalizes the rotation quaternion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref __e3t__ r)
        {
            r.Rot.Normalize();
        }

        /// <summary>
        /// Inverts this rigid transformation (multiplicative inverse).
        /// this = [Rot^T,-Rot^T Trans]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref __e3t__ r)
        {
            r.Rot.Invert();
            r.Trans = -r.Rot.Transform(r.Trans);
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __e3t__ r0, __e3t__ r1)
        {
            return ApproximateEquals(r0, r1, Constant<__ft__>.PositiveTinyValue, Constant<__ft__>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __e3t__ r0, __e3t__ r1, __ft__ angleTol, __ft__ posTol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, posTol) && r0.Rot.ApproximateEquals(r1.Rot, angleTol);
        }

        #endregion
    }

    #endregion

    //# }
}
