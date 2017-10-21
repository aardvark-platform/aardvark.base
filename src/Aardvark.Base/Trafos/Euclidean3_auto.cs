using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in 3D that is composed of a 
    /// 3D rotation Rot and a subsequent translation by a 3D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Euclidean3f : IValidity
    {
        [DataMember]
        public Rot3f Rot;
        [DataMember]
        public V3f Trans;

        #region Constructors

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public Euclidean3f(Rot3f rot, V3f trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public Euclidean3f(M33f rot, V3f trans, float epsilon = (float)0.00001)
        {
            Rot = Rot3f.FromM33f(rot, epsilon);
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a matrix <paramref name="m"/>.
        /// </summary>
        public Euclidean3f(M44f m, float epsilon = (float)0.00001)
            : this(((M33f)m) / m.M33, m.C3.XYZ / m.M33, epsilon)
        {
            Requires.That(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon), "Matrix contains perspective components.");
            Requires.That(!m.M33.IsTiny(epsilon), "Matrix is not homogeneous.");
        }

        /// <summary>
        /// Creates a rigid transformation from a trafo <paramref name="trafo"/>.
        /// </summary>
        public Euclidean3f(Trafo3d trafo, float epsilon = (float)0.00001)
            : this((M44f)trafo.Forward, epsilon)
        {
        }


        #endregion

        #region Constants

        /// <summary>
        /// Identity (Rot3f.Identity, V3f.Zero)
        /// </summary>
        public static readonly Euclidean3f Identity = new Euclidean3f(Rot3f.Identity, V3f.Zero);

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
        public static Euclidean3f Multiply(Euclidean3f a, Euclidean3f b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new Euclidean3f(Rot3f.Multiply(a.Rot, b.Rot), a.Trans + a.Rot.TransformDir(b.Trans));
        }

        public static M34f Multiply(M33f m, Euclidean3f r)
        {
            return M34f.Multiply(m, (M34f)r);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V3f TransformDir(Euclidean3f r, V3f v)
        {
            return r.Rot.TransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by rigid transformation r.
        /// </summary>
        public static V3f TransformPos(Euclidean3f r, V3f p)
        {
            return r.Rot.TransformPos(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of the rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V3f InvTransformDir(Euclidean3f r, V3f v)
        {
            return r.Rot.InvTransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        public static V3f InvTransformPos(Euclidean3f r, V3f p)
        {
            return r.Rot.InvTransformPos(p - r.Trans);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V3f TransformDir(V3f v)
        {
            return TransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by this rigid transformation.
        /// </summary>
        public V3f TransformPos(V3f p)
        {
            return TransformPos(this, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V3f InvTransformDir(V3f v)
        {
            return InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of this rigid transformation.
        /// </summary>
        public V3f InvTransformPos(V3f p)
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
        public Euclidean3f Normalized
        {
            get
            {
                return new Euclidean3f(Rot.Normalized, Trans);
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
        public Euclidean3f Inverse
        {
            get
            {
                var newR = Rot.Inverse;
                return new Euclidean3f(newR, -newR.TransformDir(Trans));
            }
        }

        public static bool ApproxEqual(Euclidean3f r0, Euclidean3f r1)
        {
            return ApproxEqual(r0, r1, Constant<float>.PositiveTinyValue, Constant<float>.PositiveTinyValue);
        }

        public static bool ApproxEqual(Euclidean3f r0, Euclidean3f r1, float angleTol, float posTol)
        {
            return V3f.ApproxEqual(r0.Trans, r1.Trans, posTol) && Rot3f.ApproxEqual(r0.Rot, r1.Rot, angleTol);
        }

        #endregion

        #region Arithmetic Operators

        public static Euclidean3f operator *(Euclidean3f a, Euclidean3f b)
        {
            return Euclidean3f.Multiply(a, b);
        }

#if false //// [todo ISSUE 20090421 andi : andi] check if these are really necessary and comment them what they really do.
        /// <summary>
        /// </summary>
        public static V3f operator *(Euclidean3f rot, V2f v)
        {
            return (M33f)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V3f operator *(Euclidean3f rot, V3f v)
        {
            return (M33f)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V4f operator *(Euclidean3f rot, V4f v)
        {
            return (M33f)rot * v;
        }

        /// <summary>
        /// </summary>
        public static M33f operator *(Euclidean3f r3, Rot2f r2)
        {
            M33f m33 = (M33f)r3;
            M22f m22 = (M22f)r2;
            return new M33f(
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
                public static M33f operator *(Euclidean3f r, Rot3f m)
                {
                    return (M34f)r * m;
                }

                public static M33f operator *(Euclidean3f r, Scale3f m)
                {
                    return (M34f)r * m;
                }
        */
        // [todo ISSUE 20090427 andi : andi] this is again a Euclidean3f.
        // [todo ISSUE 20090427 andi : andi] Rot3f * Shift3f should return a Euclidean3f!
        public static M34f operator *(Euclidean3f r, Shift3f m)
        {
            return (M34f)r * m;
        }

        public static M34f operator *(M33f m, Euclidean3f r)
        {
            return Multiply(m, r);
        }
        /*
        public static M34f operator *(M33f m, Euclidean3f r)
        {
            return (M34f)m * (M34f)r;
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
        public static Euclidean3f Transformed(Euclidean3f self, Similarity3f t)
        {
            return new Euclidean3f(t.Rot * self.Rot, t.TransformPos(self.Trans));
        }

        /// <summary>
        /// Returns a new Euclidean transformation by transforming this by a t.
        /// Note: This is not a concatenation. 
        /// t is fully applied to the Translation and Rotation,
        /// but the scale is not reflected in the resulting Euclidean transformation.
        /// </summary>
        public Euclidean3f Transformed(Similarity3f t)
        {
            return Transformed(this, t);
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Euclidean3f r0, Euclidean3f r1)
        {
            return r0.Rot == r1.Rot && r0.Trans == r1.Trans;
        }

        public static bool operator !=(Euclidean3f r0, Euclidean3f r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Conversion

        // [todo ISSUE 20090421 andi] caching of the Matrix would greatly improve performance
        public static explicit operator M34f(Euclidean3f r)
        {
            M34f rv = (M34f)r.Rot;
            rv.C3 = r.Trans;
            return rv;
        }

        public static explicit operator M44f(Euclidean3f r)
        {
            M44f rv = (M44f)r.Rot;
            rv.C3 = r.Trans.XYZI;
            return rv;
        }

        public static explicit operator Similarity3f(Euclidean3f r)
        {
            return new Similarity3f(1, r);
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        public override bool Equals(object other)
        {
            return (other is Euclidean3f) ? (this == (Euclidean3f)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Rot, Trans);
        }

        public static Euclidean3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Euclidean3f(Rot3f.Parse(x[0]), V3f.Parse(x[1]));
        }

        #endregion

    }

    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in 3D that is composed of a 
    /// 3D rotation Rot and a subsequent translation by a 3D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Euclidean3d : IValidity
    {
        [DataMember]
        public Rot3d Rot;
        [DataMember]
        public V3d Trans;

        #region Constructors

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public Euclidean3d(Rot3d rot, V3d trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public Euclidean3d(M33d rot, V3d trans, double epsilon = (double)0.00001)
        {
            Rot = Rot3d.FromM33d(rot, epsilon);
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a matrix <paramref name="m"/>.
        /// </summary>
        public Euclidean3d(M44d m, double epsilon = (double)0.00001)
            : this(((M33d)m) / m.M33, m.C3.XYZ / m.M33, epsilon)
        {
            Requires.That(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon), "Matrix contains perspective components.");
            Requires.That(!m.M33.IsTiny(epsilon), "Matrix is not homogeneous.");
        }

        /// <summary>
        /// Creates a rigid transformation from a trafo <paramref name="trafo"/>.
        /// </summary>
        public Euclidean3d(Trafo3d trafo, double epsilon = (double)0.00001)
            : this((M44d)trafo.Forward, epsilon)
        {
        }


        #endregion

        #region Constants

        /// <summary>
        /// Identity (Rot3d.Identity, V3d.Zero)
        /// </summary>
        public static readonly Euclidean3d Identity = new Euclidean3d(Rot3d.Identity, V3d.Zero);

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
        public static Euclidean3d Multiply(Euclidean3d a, Euclidean3d b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new Euclidean3d(Rot3d.Multiply(a.Rot, b.Rot), a.Trans + a.Rot.TransformDir(b.Trans));
        }

        public static M34d Multiply(M33d m, Euclidean3d r)
        {
            return M34d.Multiply(m, (M34d)r);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V3d TransformDir(Euclidean3d r, V3d v)
        {
            return r.Rot.TransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by rigid transformation r.
        /// </summary>
        public static V3d TransformPos(Euclidean3d r, V3d p)
        {
            return r.Rot.TransformPos(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of the rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V3d InvTransformDir(Euclidean3d r, V3d v)
        {
            return r.Rot.InvTransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        public static V3d InvTransformPos(Euclidean3d r, V3d p)
        {
            return r.Rot.InvTransformPos(p - r.Trans);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V3d TransformDir(V3d v)
        {
            return TransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by this rigid transformation.
        /// </summary>
        public V3d TransformPos(V3d p)
        {
            return TransformPos(this, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V3d InvTransformDir(V3d v)
        {
            return InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of this rigid transformation.
        /// </summary>
        public V3d InvTransformPos(V3d p)
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
        public Euclidean3d Normalized
        {
            get
            {
                return new Euclidean3d(Rot.Normalized, Trans);
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
        public Euclidean3d Inverse
        {
            get
            {
                var newR = Rot.Inverse;
                return new Euclidean3d(newR, -newR.TransformDir(Trans));
            }
        }

        public static bool ApproxEqual(Euclidean3d r0, Euclidean3d r1)
        {
            return ApproxEqual(r0, r1, Constant<double>.PositiveTinyValue, Constant<double>.PositiveTinyValue);
        }

        public static bool ApproxEqual(Euclidean3d r0, Euclidean3d r1, double angleTol, double posTol)
        {
            return V3d.ApproxEqual(r0.Trans, r1.Trans, posTol) && Rot3d.ApproxEqual(r0.Rot, r1.Rot, angleTol);
        }

        #endregion

        #region Arithmetic Operators

        public static Euclidean3d operator *(Euclidean3d a, Euclidean3d b)
        {
            return Euclidean3d.Multiply(a, b);
        }

#if false //// [todo ISSUE 20090421 andi : andi] check if these are really necessary and comment them what they really do.
        /// <summary>
        /// </summary>
        public static V3d operator *(Euclidean3d rot, V2f v)
        {
            return (M33d)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V3d operator *(Euclidean3d rot, V3d v)
        {
            return (M33d)rot * v;
        }

        /// <summary>
        /// </summary>
        public static V4d operator *(Euclidean3d rot, V4d v)
        {
            return (M33d)rot * v;
        }

        /// <summary>
        /// </summary>
        public static M33d operator *(Euclidean3d r3, Rot2f r2)
        {
            M33d m33 = (M33d)r3;
            M22f m22 = (M22f)r2;
            return new M33d(
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
                public static M33d operator *(Euclidean3d r, Rot3d m)
                {
                    return (M34d)r * m;
                }

                public static M33d operator *(Euclidean3d r, Scale3d m)
                {
                    return (M34d)r * m;
                }
        */
        // [todo ISSUE 20090427 andi : andi] this is again a Euclidean3d.
        // [todo ISSUE 20090427 andi : andi] Rot3d * Shift3d should return a Euclidean3d!
        public static M34d operator *(Euclidean3d r, Shift3d m)
        {
            return (M34d)r * m;
        }

        public static M34d operator *(M33d m, Euclidean3d r)
        {
            return Multiply(m, r);
        }
        /*
        public static M34d operator *(M33d m, Euclidean3d r)
        {
            return (M34d)m * (M34d)r;
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
        public static Euclidean3d Transformed(Euclidean3d self, Similarity3d t)
        {
            return new Euclidean3d(t.Rot * self.Rot, t.TransformPos(self.Trans));
        }

        /// <summary>
        /// Returns a new Euclidean transformation by transforming this by a t.
        /// Note: This is not a concatenation. 
        /// t is fully applied to the Translation and Rotation,
        /// but the scale is not reflected in the resulting Euclidean transformation.
        /// </summary>
        public Euclidean3d Transformed(Similarity3d t)
        {
            return Transformed(this, t);
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Euclidean3d r0, Euclidean3d r1)
        {
            return r0.Rot == r1.Rot && r0.Trans == r1.Trans;
        }

        public static bool operator !=(Euclidean3d r0, Euclidean3d r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Conversion

        // [todo ISSUE 20090421 andi] caching of the Matrix would greatly improve performance
        public static explicit operator M34d(Euclidean3d r)
        {
            M34d rv = (M34d)r.Rot;
            rv.C3 = r.Trans;
            return rv;
        }

        public static explicit operator M44d(Euclidean3d r)
        {
            M44d rv = (M44d)r.Rot;
            rv.C3 = r.Trans.XYZI;
            return rv;
        }

        public static explicit operator Similarity3d(Euclidean3d r)
        {
            return new Similarity3d(1, r);
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        public override bool Equals(object other)
        {
            return (other is Euclidean3d) ? (this == (Euclidean3d)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Rot, Trans);
        }

        public static Euclidean3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Euclidean3d(Rot3d.Parse(x[0]), V3d.Parse(x[1]));
        }

        #endregion

    }

}
