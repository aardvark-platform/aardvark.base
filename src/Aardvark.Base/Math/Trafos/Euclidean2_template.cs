using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var s2f = isDouble ? "2d" : "2f";
    //#   var s3f = isDouble ? "3d" : "3f";
    //#   var v2t = isDouble ? "V2d" : "V2f";
    //#   var r2t = isDouble ? "Rot2d" : "Rot2f";
    //#   var e2t = isDouble ? "Euclidean2d" : "Euclidean2f";
    //#   var a2t = isDouble ? "Affine2d" : "Affine2f";
    #region __e2t__

    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in 2D that is composed of a 
    /// 2D rotation Rot and a subsequent translation by a 2D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __e2t__ : IValidity
    {
        [DataMember]
        public __r2t__ Rot;
        [DataMember]
        public __v2t__ Trans;

        #region Constructors

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public __e2t__(__r2t__ rot, __v2t__ trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public __e2t__(M2__s2f__ rot, __v2t__ trans)
        {
            Rot = __r2t__.FromM2__s2f__(rot);
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a trafo <paramref name="trafo"/>.
        /// </summary>
        public __e2t__(Trafo2d trafo)
        {
            Rot = __r2t__.FromM2__s2f__((M2__s2f__)trafo.Forward);
            Trans = (__v2t__)trafo.Forward.C2.XY;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Identity (__r2t__.Identity, __v2t__.Zero)
        /// </summary>
        public static __e2t__ Identity => new __e2t__(__r2t__.Identity, __v2t__.Zero);

        #endregion

        #region Properties

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the (multiplicative) inverse of this Euclidean transformation.
        /// [Rot^T,-Rot^T Trans]
        /// </summary>
        public __e2t__ Inverse
        {
            get
            {
                var newR = Rot.Inverse;
                return new __e2t__(newR, -newR.Transform(Trans));
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Euclidean transformations.
        /// This concatenates the two rigid transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        public static __e2t__ operator *(__e2t__ a, __e2t__ b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new __e2t__(a.Rot * b.Rot, a.Trans + a.Rot.Transform(b.Trans));
        }

#if false //// [todo ISSUE 20090421 andi : andi] check if these are really necessary and comment them what they really do.
        /// <summary>
        /// </summary>
        public static V__s3f__ operator *(Euclidean__s3f__ rot, __v2t__ v)
        {
            return (M3__s3f__)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V__s3f__ operator *(Euclidean__s3f__ rot, V__s3f__ v)
        {
            return (M3__s3f__)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V4f operator *(Euclidean__s3f__ rot, V4f v)
        {
            return (M3__s3f__)rot * v;
        }

        /// <summary>
        /// </summary>
        public static M3__s3f__ operator *(Euclidean__s3f__ r3, __r2t__ r2)
        {
            M3__s3f__ m33 = (M3__s3f__)r3;
            M2__s2f__ m22 = (M2__s2f__)r2;
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
                public static M3__s3f__ operator *(Euclidean__s3f__ r, Rot__s3f__ m)
                {
                    return (M34f)r * m;
                }

                public static M3__s3f__ operator *(Euclidean__s3f__ r, Scale__s3f__ m)
                {
                    return (M34f)r * m;
                }
        */
        // [todo ISSUE 20090427 andi : andi] this is again a Euclidean__s3f__.
        // [todo ISSUE 20090427 andi : andi] Rot__s3f__ * Shift__s3f__ should return a Euclidean__s3f__!

        public static M2__s3f__ operator *(M2__s2f__ m, __e2t__ r)
        {
            return m * (M2__s3f__)r;
        }
        /*
        public static M34f operator *(M3__s3f__ m, Euclidean__s3f__ r)
        {
            return (M34f)m * (M34f)r;
        }
        */
        #endregion

        #region Comparison Operators

        public static bool operator ==(__e2t__ r0, __e2t__ r1)
        {
            return r0.Rot == r1.Rot && r0.Trans == r1.Trans;
        }

        public static bool operator !=(__e2t__ r0, __e2t__ r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Conversion

        // [todo ISSUE 20090421 andi] caching of the Matrix would greatly improve performance
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M2__s3f__(__e2t__ r)
        {
            M2__s3f__ rv = (M2__s3f__)r.Rot;
            rv.C2 = r.Trans;
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M3__s3f__(__e2t__ r)
        {
            M3__s3f__ rv = (M3__s3f__)r.Rot;
            rv.C2 = r.Trans.XYI;
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __a2t__(__e2t__ r)
            => new __a2t__(r);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        public override bool Equals(object other)
        {
            return (other is __e2t__) ? (this == (__e2t__)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Rot, Trans);
        }

        public static __e2t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __e2t__(__r2t__.Parse(x[0]), __v2t__.Parse(x[1]));
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
        public static __v2t__ TransformDir(this __e2t__ r, __v2t__ v)
        {
            return r.Rot.Transform(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ TransformPos(this __e2t__ r, __v2t__ p)
        {
            return r.Rot.Transform(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ InvTransformDir(this __e2t__ r, __v2t__ v)
        {
            return r.Rot.InvTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __v2t__ InvTransformPos(this __e2t__ r, __v2t__ p)
        {
            return r.Rot.InvTransform(p - r.Trans);
        }

        #endregion

        #region Invert

        /// <summary>
        /// Inverts the given rigid transformation (multiplicative inverse), yielding [Rot^T,-Rot^T Trans].
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref __e2t__ r)
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
        public static bool ApproximateEquals(this __e2t__ r0, __e2t__ r1)
        {
            return ApproximateEquals(r0, r1, Constant<__ft__>.PositiveTinyValue, Constant<__ft__>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __e2t__ r0, __e2t__ r1, __ft__ angleTol, __ft__ posTol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, posTol) && r0.Rot.ApproximateEquals(r1.Rot, angleTol);
        }

        #endregion
    }

    #endregion

    //# }
}
