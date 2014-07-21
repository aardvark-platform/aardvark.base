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
    public struct Euclidean2d : IValidity
    {
        public Rot2d Rot;
        public V2d Trans;

        #region Constructors

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public Euclidean2d(Rot2d rot, V2d trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        public Euclidean2d(M22d rot, V2d trans)
        {
            Rot = Rot2d.FromM22d(rot);
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a trafo <paramref name="trafo"/>.
        /// </summary>
        public Euclidean2d(Trafo2d trafo)
        {
            Rot = Rot2d.FromM22d((M22d)trafo.Forward);
            Trans = (V2d)trafo.Forward.C2.XY;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Identity (Rot3d.Identity, V3d.Zero)
        /// </summary>
        public static readonly Euclidean2d Identity = new Euclidean2d(Rot2d.Identity, V2d.Zero);

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
        public static Euclidean2d Multiply(Euclidean2d a, Euclidean2d b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new Euclidean2d(Rot2d.Multiply(a.Rot, b.Rot),a.Trans + a.Rot.TransformDir(b.Trans));
        }

        public static M23d Multiply(M22d m, Euclidean2d r)
        {
            return M23d.Multiply(m, (M23d)r);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V2d TransformDir(Euclidean2d r, V2d v)
        {
            return r.Rot.TransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by rigid transformation r.
        /// </summary>
        public static V2d TransformPos(Euclidean2d r, V2d p)
        {
            return r.Rot.TransformPos(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of the rigid transformation r.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V2d InvTransformDir(Euclidean2d r, V2d v)
        {
            return r.Rot.InvTransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        public static V2d InvTransformPos(Euclidean2d r, V2d p)
        {
            return r.Rot.InvTransformPos(p - r.Trans);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V2d TransformDir(V2d v)
        {
            return TransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by this rigid transformation.
        /// </summary>
        public V2d TransformPos(V2d p)
        {
            return TransformPos(this, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of this rigid transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V2d InvTransformDir(V2d v)
        {
            return InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of this rigid transformation.
        /// </summary>
        public V2d InvTransformPos(V2d p)
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
        public Euclidean2d Inverse
        {
            get
            {
                var newR = Rot.Inverse;
                return new Euclidean2d(newR, -newR.TransformDir(Trans));
            }
        }

        public static bool ApproxEqual(Euclidean2d r0, Euclidean2d r1)
        {
            return ApproxEqual(r0, r1, Constant<double>.PositiveTinyValue, Constant<double>.PositiveTinyValue);
        }

        public static bool ApproxEqual(Euclidean2d r0, Euclidean2d r1, double angleTol, double posTol)
        {
            return V2d.ApproxEqual(r0.Trans, r1.Trans, posTol) && Rot2d.ApproxEqual(r0.Rot, r1.Rot, angleTol);
        }

        #endregion

        #region Arithmetic Operators

        public static Euclidean2d operator *(Euclidean2d a, Euclidean2d b)
        {
            return Euclidean2d.Multiply(a, b);
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

        public static M23d operator *(M22d m, Euclidean2d r)
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

        #region Comparison Operators

        public static bool operator ==(Euclidean2d r0, Euclidean2d r1)
        {
            return r0.Rot == r1.Rot && r0.Trans == r1.Trans;
        }

        public static bool operator !=(Euclidean2d r0, Euclidean2d r1)
        {
            return !(r0 == r1);
        }

        #endregion

        #region Conversion

        // [todo ISSUE 20090421 andi] caching of the Matrix would greatly improve performance
        public static explicit operator M23d(Euclidean2d r)
        {
            M23d rv = (M23d)r.Rot;
            rv.C2 = r.Trans;
            return rv;
        }

        public static explicit operator M33d(Euclidean2d r)
        {
            M33d rv = (M33d)r.Rot;
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
            return (other is Euclidean2d) ? (this == (Euclidean2d)other) : false;
        }

        public override string ToString()
        {
            return string.Format(Localization.FormatEnUS, "[{0}, {1}]", Rot, Trans);
        }

        public static Euclidean2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Euclidean2d(Rot2d.Parse(x[0]), V2d.Parse(x[1]));
        }

        #endregion

    }

}
