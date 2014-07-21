using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    // [todo ISSUE 20090427 andi : andi] define Euclidean2*
    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in 3D that is composed of a 
    /// 3D rotation Rot and a subsequent translation by a 3D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Euclidean3d : IValidity
    {
        public Rot3d Rot;
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
            Trace.Assert(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon), "Matrix contains perspective components.");
            Trace.Assert(!m.M33.IsTiny(epsilon), "Matrix is not homogeneous.");
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
        /// This concatenates the two rigid transformations into a double one, first b is applied, then a.
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
        public static V3d operator *(Euclidean3d rot, V2d v)
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
        public static M33d operator *(Euclidean3d r3, Rot2d r2)
        {
            M33d m33 = (M33d)r3;
            M22d m22 = (M22d)r2;
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
            return string.Format(Localization.FormatEnUS, "[{0}, {1}]", Rot, Trans);
        }

        public static Euclidean3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Euclidean3d(Rot3d.Parse(x[0]), V3d.Parse(x[1]));
        }

        #endregion

    }

}
