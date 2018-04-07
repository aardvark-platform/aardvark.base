using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

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
        public static readonly __e3t__ Identity = new __e3t__(__r3t__.Identity, __v3t__.Zero);

        #endregion

        #region Properties

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        #endregion

        #region Euclidean Transformation Arithmetics

        /// <summary>
        /// Multiplies 2 Euclidean transformations.
        /// This concatenates the two rigid transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        public static __e3t__ Multiply(__e3t__ a, __e3t__ b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new __e3t__(__r3t__.Multiply(a.Rot, b.Rot), a.Trans + a.Rot.TransformDir(b.Trans));
        }

        public static M3__s4f__ Multiply(M3__s3f__ m, __e3t__ r)
        {
            return M3__s4f__.Multiply(m, (M3__s4f__)r);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static __v3t__ TransformDir(__e3t__ r, __v3t__ v)
        {
            return r.Rot.TransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by rigid transformation r.
        /// </summary>
        public static __v3t__ TransformPos(__e3t__ r, __v3t__ p)
        {
            return r.Rot.TransformPos(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of the rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static __v3t__ InvTransformDir(__e3t__ r, __v3t__ v)
        {
            return r.Rot.InvTransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        public static __v3t__ InvTransformPos(__e3t__ r, __v3t__ p)
        {
            return r.Rot.InvTransformPos(p - r.Trans);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public __v3t__ TransformDir(__v3t__ v)
        {
            return TransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by this rigid transformation.
        /// </summary>
        public __v3t__ TransformPos(__v3t__ p)
        {
            return TransformPos(this, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public __v3t__ InvTransformDir(__v3t__ v)
        {
            return InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of this rigid transformation.
        /// </summary>
        public __v3t__ InvTransformPos(__v3t__ p)
        {
            return InvTransformPos(this, p);
        }

        /// <summary>
        /// Normalizes the rotation quaternion.
        /// </summary>
        public void Normalize()
        {
            Rot.Normalize();
        }

        /// <summary>
        /// Returns a new version of this Euclidean transformation with a normalized rotation quaternion.
        /// </summary>
        public __e3t__ Normalized
        {
            get
            {
                return new __e3t__(Rot.Normalized, Trans);
            }
        }

        /// <summary>
        /// Inverts this rigid transformation (multiplicative inverse).
        /// this = [Rot^T,-Rot^T Trans]
        /// </summary>
        public void Invert()
        {
            Rot.Invert();
            Trans = -Rot.TransformDir(Trans);
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this Euclidean transformation.
        /// [Rot^T,-Rot^T Trans]
        /// </summary>
        public __e3t__ Inverse
        {
            get
            {
                var newR = Rot.Inverse;
                return new __e3t__(newR, -newR.TransformDir(Trans));
            }
        }

        public static bool ApproxEqual(__e3t__ r0, __e3t__ r1)
        {
            return ApproxEqual(r0, r1, Constant<__ft__>.PositiveTinyValue, Constant<__ft__>.PositiveTinyValue);
        }

        public static bool ApproxEqual(__e3t__ r0, __e3t__ r1, __ft__ angleTol, __ft__ posTol)
        {
            return __v3t__.ApproxEqual(r0.Trans, r1.Trans, posTol) && __r3t__.ApproxEqual(r0.Rot, r1.Rot, angleTol);
        }

        #endregion

        #region Arithmetic Operators

        public static __e3t__ operator *(__e3t__ a, __e3t__ b)
        {
            return __e3t__.Multiply(a, b);
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
        public static M3__s4f__ operator *(__e3t__ r, Shift__s3f__ m)
        {
            return (M3__s4f__)r * m;
        }

        public static M3__s4f__ operator *(M3__s3f__ m, __e3t__ r)
        {
            return Multiply(m, r);
        }
        /*
        public static M3__s4f__ operator *(M3__s3f__ m, __e3t__ r)
        {
            return (M3__s4f__)m * (M3__s4f__)r;
        }
        */
        #endregion

        #region Transformations yielding a Euclidean transformation

        /// <summary>
        /// Returns a new Euclidean transformation by transforming self by a Trafo t.
        /// Note: This is not a concatenation. 
        /// t is fully applied to the Translation and Rotation,
        /// but the scale is not reflected in the resulting Euclidean transformation.
        /// </summary>
        // [todo ISSUE 20090810 andi : andi] Rethink this notation. Maybe write Transformed methods for all transformations.
        public static __e3t__ Transformed(__e3t__ self, Similarity__s3f__ t)
        {
            return new __e3t__(t.Rot * self.Rot, t.TransformPos(self.Trans));
        }

        /// <summary>
        /// Returns a new Euclidean transformation by transforming this by a t.
        /// Note: This is not a concatenation. 
        /// t is fully applied to the Translation and Rotation,
        /// but the scale is not reflected in the resulting Euclidean transformation.
        /// </summary>
        public __e3t__ Transformed(Similarity__s3f__ t)
        {
            return Transformed(this, t);
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

    //# } // isDouble
}
