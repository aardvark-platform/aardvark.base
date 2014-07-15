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
    public struct Euclidean2f : IValidity
    {
        public Rot2f Rot;
        public V2f Trans;

        #region Constructors

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="Rot"/> and a (subsequent) translation <paramref name="Trans"/>.
        /// </summary>
        public Euclidean2f(Rot2f rot, V2f trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation matrix <paramref name="Rot"/> and a (subsequent) translation <paramref name="Trans"/>.
        /// </summary>
        public Euclidean2f(M22f rot, V2f trans)
        {
            Rot = Rot2f.FromM22f(rot);
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a trafo <paramref name="Rot"/> and a (subsequent) translation <paramref name="Trans"/>.
        /// </summary>
        public Euclidean2f(Trafo2d trafo)
        {
            Rot = Rot2f.FromM22f((M22f)trafo.Forward);
            Trans = (V2f)trafo.Forward.C2.XY;
        }


        #endregion

        #region Constants

        /// <summary>
        /// Identity (Rot3f.Identity, V3f.Zero)
        /// </summary>
        public static readonly Euclidean2f Identity = new Euclidean2f(Rot2f.Identity, V2f.Zero);

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
        public static Euclidean2f Multiply(Euclidean2f a, Euclidean2f b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new Euclidean2f(Rot2f.Multiply(a.Rot, b.Rot),a.Trans + a.Rot.TransformDir(b.Trans));
        }

        public static M23f Multiply(M22f m, Euclidean2f r)
        {
            return M23f.Multiply(m, (M23f)r);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V2f TransformDir(Euclidean2f r, V2f v)
        {
            return r.Rot.TransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by rigid transformation r.
        /// </summary>
        public static V2f TransformPos(Euclidean2f r, V2f p)
        {
            return r.Rot.TransformPos(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of the rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V2f InvTransformDir(Euclidean2f r, V2f v)
        {
            return r.Rot.InvTransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        public static V2f InvTransformPos(Euclidean2f r, V2f p)
        {
            return r.Rot.InvTransformPos(p - r.Trans);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V2f TransformDir(V2f v)
        {
            return TransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by this rigid transformation.
        /// </summary>
        public V2f TransformPos(V2f p)
        {
            return TransformPos(this, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V2f InvTransformDir(V2f v)
        {
            return InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of this rigid transformation.
        /// </summary>
        public V2f InvTransformPos(V2f p)
        {
            return InvTransformPos(this, p);
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
        public Euclidean2f Inverse
        {
            get
            {
                var newR = Rot.Inverse;
                return new Euclidean2f(newR, -newR.TransformDir(Trans));
            }
        }

        public static bool ApproxEqual(Euclidean2f r0, Euclidean2f r1)
        {
            return ApproxEqual(r0, r1, Constant<float>.PositiveTinyValue, Constant<float>.PositiveTinyValue);
        }

        public static bool ApproxEqual(Euclidean2f r0, Euclidean2f r1, float angleTol, float posTol)
        {
            return V2f.ApproxEqual(r0.Trans, r1.Trans, posTol) && Rot2f.ApproxEqual(r0.Rot, r1.Rot, angleTol);
        }

        #endregion

        #region Arithmetic Operators

        public static Euclidean2f operator *(Euclidean2f a, Euclidean2f b)
        {
            return Euclidean2f.Multiply(a, b);
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

        public static M23f operator *(M22f m, Euclidean2f r)
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

        #region Comparison Operators

        public static bool operator ==(Euclidean2f r0, Euclidean2f r1)
        {
            return r0.Rot == r1.Rot && r0.Trans == r1.Trans;
        }

        public static bool operator !=(Euclidean2f r0, Euclidean2f r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Conversion

        // [todo ISSUE 20090421 andi] caching of the Matrix would greatly improve performance
        public static explicit operator M23f(Euclidean2f r)
        {
            M23f rv = (M23f)r.Rot;
            rv.C2 = r.Trans;
            return rv;
        }

        public static explicit operator M33f(Euclidean2f r)
        {
            M33f rv = (M33f)r.Rot;
            rv.C2 = r.Trans.XYI;
            return rv;
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        public override bool Equals(object other)
        {
            return (other is Euclidean2f) ? (this == (Euclidean2f)other) : false;
        }

        public override string ToString()
        {
            return string.Format(Localization.FormatEnUS, "[{0}, {1}]", Rot, Trans);
        }

        public static Euclidean2f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Euclidean2f(Rot2f.Parse(x[0]), V2f.Parse(x[1]));
        }

        #endregion

    }

}
