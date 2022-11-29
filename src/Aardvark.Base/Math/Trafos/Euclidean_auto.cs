using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    #region Euclidean2f

    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in 2D that is composed of a 
    /// 2D rotation Rot and a subsequent translation by a 2D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Euclidean2f : IEquatable<Euclidean2f>
    {
        [DataMember]
        public Rot2f Rot;
        [DataMember]
        public V2f Trans;

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="Euclidean2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2f(Euclidean2f e)
        {
            Rot = e.Rot;
            Trans = e.Trans;
        }

        /// <summary>
        /// Constructs a <see cref="Euclidean2f"/> transformation from a <see cref="Euclidean2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2f(Euclidean2d e)
        {
            Rot = (Rot2f)e.Rot;
            Trans = (V2f)e.Trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2f(Rot2f rot)
        {
            Rot = rot;
            Trans = V2f.Zero;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2f(V2f trans)
        {
            Rot = Rot2f.Identity;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation by (<paramref name="tX"/>, <paramref name="tY"/>).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2f(float tX, float tY)
        {
            Rot = Rot2f.Identity;
            Trans = new V2f(tX, tY);
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2f(Rot2f rot, V2f trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation by (<paramref name="tX"/>, <paramref name="tY"/>)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2f(Rot2f rot, float tX, float tY)
        {
            Rot = rot;
            Trans = new V2f(tX, tY);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Euclidean2f Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Euclidean2f(Rot2f.Identity, V2f.Zero);
        }

        #endregion

        #region Properties


        /// <summary>
        /// Gets the (multiplicative) inverse of this Euclidean transformation.
        /// [Rot^T,-Rot^T Trans]
        /// </summary>
        public Euclidean2f Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newR = Rot.Inverse;
                return new Euclidean2f(newR, -newR.Transform(Trans));
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Euclidean transformations.
        /// This concatenates the two rigid transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f operator *(Euclidean2f a, Euclidean2f b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new Euclidean2f(a.Rot * b.Rot, a.Trans + a.Rot.Transform(b.Trans));
        }

        /// <summary>
        /// Transforms a <see cref="V3f"/> vector by a <see cref="Euclidean2f"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f operator *(Euclidean2f e, V3f v)
        {
            var rot = (M22f)e.Rot;
            return new V3f(
                rot.M00 * v.X + rot.M01 * v.Y + e.Trans.X * v.Z, 
                rot.M10 * v.X + rot.M11 * v.Y + e.Trans.Y * v.Z,
                v.Z);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean2f"/> transformation (as a 3x3 matrix) with a <see cref="M33f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(Euclidean2f e, M33f m)
        {
            var t = (M23f)e;
            return new M33f(
                t.M00 * m.M00 + t.M01 * m.M10 + t.M02 * m.M20, 
                t.M00 * m.M01 + t.M01 * m.M11 + t.M02 * m.M21, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02 * m.M22,

                t.M10 * m.M00 + t.M11 * m.M10 + t.M12 * m.M20, 
                t.M10 * m.M01 + t.M11 * m.M11 + t.M12 * m.M21, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12 * m.M22,

                m.M20, m.M21, m.M22);
        }

        /// <summary>
        /// Multiplies a <see cref="M33f"/> with a <see cref="Euclidean2f"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(M33f m, Euclidean2f e)
        {
            var t = (M23f)e;
            return new M33f(
                m.M00 * t.M00 + m.M01 * t.M10, 
                m.M00 * t.M01 + m.M01 * t.M11, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02,

                m.M10 * t.M00 + m.M11 * t.M10, 
                m.M10 * t.M01 + m.M11 * t.M11, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12,

                m.M20 * t.M00 + m.M21 * t.M10, 
                m.M20 * t.M01 + m.M21 * t.M11, 
                m.M20 * t.M02 + m.M21 * t.M12 + m.M22);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean2f"/> transformation (as a 2x3 matrix) with a <see cref="M23f"/> (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23f operator *(Euclidean2f e, M23f m)
        {
            var t = (M23f)e;
            return new M23f(
                t.M00 * m.M00 + t.M01 * m.M10, 
                t.M00 * m.M01 + t.M01 * m.M11, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02,

                t.M10 * m.M00 + t.M11 * m.M10, 
                t.M10 * m.M01 + t.M11 * m.M11, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12);
        }

        /// <summary>
        /// Multiplies a <see cref="M23f"/> with a <see cref="Euclidean2f"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23f operator *(M23f m, Euclidean2f e)
        {
            var t = (M23f)e;
            return new M23f(
                m.M00 * t.M00 + m.M01 * t.M10, 
                m.M00 * t.M01 + m.M01 * t.M11, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02,

                m.M10 * t.M00 + m.M11 * t.M10, 
                m.M10 * t.M01 + m.M11 * t.M11, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean2f"/> (as a 2x3 matrix) and a <see cref="M22f"/> (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23f operator *(Euclidean2f a, M22f m)
            => new M23f(a.Rot * m, a.Trans);

        /// <summary>
        /// Multiplies a <see cref="M22f"/> and a <see cref="Euclidean2f"/> (as a 2x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23f operator *(M22f m, Euclidean2f a)
            => new M23f(m * a.Rot, m * a.Trans);

        /// <summary>
        /// Multiplies a <see cref="Euclidean2f"/> and a <see cref="Rot2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f operator *(Euclidean2f e, Rot2f r)
            => new Euclidean2f(e.Rot * r, e.Trans);

        /// <summary>
        /// Multiplies a <see cref="Rot2f"/> and a <see cref="Euclidean2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f operator *(Rot2f r, Euclidean2f e)
            => new Euclidean2f(r * e.Rot, r * e.Trans);

        /// <summary>
        /// Multiplies a <see cref="Euclidean2f"/> and a <see cref="Shift2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f operator *(Euclidean2f e, Shift2f s)
        {
            return new Euclidean2f(e.Rot, e.Rot * s.V + e.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Shift2f"/> and a <see cref="Euclidean2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f operator *(Shift2f s, Euclidean2f e)
        {
            return new Euclidean2f(e.Rot, e.Trans + s.V);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean2f"/> and a <see cref="Scale2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine2f operator *(Euclidean2f r, Scale2f s)
        {
            var t = (M22f)r.Rot;
            return new Affine2f(new M22f(
                t.M00 * s.X, t.M01 * s.Y, 
                t.M10 * s.X, t.M11 * s.Y),
                r.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Scale2f"/> and a <see cref="Euclidean2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine2f operator *(Scale2f s, Euclidean2f r)
        {
            var t = (M22f)r.Rot;
            return new Affine2f(new M22f(
                t.M00 * s.X, t.M01 * s.X, 
                t.M10 * s.Y, t.M11 * s.Y),
                r.Trans * s.V);
        }

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

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="Euclidean2f"/> transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// The matrix <paramref name="rot"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f FromM22fAndV2f(M22f rot, V2f trans)
            => new Euclidean2f(Rot2f.FromM22f(rot), trans);


        /// <summary>
        /// Creates a <see cref="Euclidean2f"/> transformation from a <see cref="M33f"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components and its upper left 2x2 submatrix must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f FromM33f(M33f m, float epsilon = 1e-5f) 
        {
            if (!(m.M20.IsTiny(epsilon) && m.M21.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");
            if (m.M22.IsTiny(epsilon)) throw new ArgumentException("Matrix is not homogeneous.");

            return FromM22fAndV2f(((M22f)m) / m.M22,
                    m.C2.XY / m.M22);
        }

        /// <summary>
        /// Creates a <see cref="Euclidean2f"/> transformation from a <see cref="M23f"/> matrix.
        /// The left 2x2 submatrix of <paramref name="m"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f FromM23f(M23f m, float epsilon = 1e-5f)
        {
            return FromM22fAndV2f(((M22f)m),
                    m.C2.XY);
        }

        /// <summary>
        /// Creates a <see cref="Euclidean2f"/> transformation from a <see cref="Similarity2f"/>.
        /// The transformation <paramref name="similarity"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f FromSimilarity2f(Similarity2f similarity, float epsilon = 1e-5f)
        {
            if (!similarity.Scale.ApproximateEquals(1, epsilon))
                throw new ArgumentException("Similarity transformation contains scaling component");

            return similarity.Euclidean;
        }

        /// <summary>
        /// Creates a <see cref="Euclidean2f"/> transformation from an <see cref="Affine2f"/>.
        /// The transformation <paramref name="affine"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f FromAffine2f(Affine2f affine, float epsilon = 1e-5f)
            => FromM33f((M33f)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Euclidean2f"/> transformation from a <see cref="Trafo2f"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f FromTrafo2f(Trafo2f trafo, float epsilon = 1e-5f)
            => FromM33f(trafo.Forward, epsilon);

        #region Translation

        /// <summary>
        /// Creates a <see cref="Euclidean2f"/> transformation with the translational component given by 2 scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f Translation(float tX, float tY)
            => new Euclidean2f(tX, tY);

        /// <summary>
        /// Creates a <see cref="Euclidean2f"/> transformation with the translational component given by a <see cref="V2f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f Translation(V2f vector)
            => new Euclidean2f(vector);

        /// <summary>
        /// Creates an <see cref="Euclidean2f"/> transformation with the translational component given by a <see cref="Shift2f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f Translation(Shift2f shift)
            => new Euclidean2f(shift.V);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="Rot2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f Rotation(Rot2f rot)
            => new Euclidean2f(rot);

        /// <summary>
        /// Creates a rotation transformation with the specified angle in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f Rotation(float angleInRadians)
            => new Euclidean2f(new Rot2f(angleInRadians));

        /// <summary>
        /// Creates a rotation transformation with the specified angle in degrees.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f RotationInDegrees(float angleInDegrees)
            => Rotation(angleInDegrees.RadiansFromDegrees());

        #endregion

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M23f(Euclidean2f e)
            => new M23f((M22f)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M22f(Euclidean2f e)
            => (M22f)e.Rot;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33f(Euclidean2f e)
        {
            M33f rv = (M33f)e.Rot;
            rv.C2 = e.Trans.XYI;
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity2f(Euclidean2f e)
            => new Similarity2f(1, e);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine2f(Euclidean2f e)
            => new Affine2f((M22f)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo2f(Euclidean2f e)
            => new Trafo2f((M33f)e, (M33f)e.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Euclidean2d(Euclidean2f e)
            => new Euclidean2d((Rot2d)e.Rot, (V2d)e.Trans);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Euclidean2f other)
            => Rot.Equals(other.Rot) && Trans.Equals(other.Trans);

        public override bool Equals(object other)
            => (other is Euclidean2f o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Rot, Trans);
        }

        public static Euclidean2f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Euclidean2f(Rot2f.Parse(x[0]), V2f.Parse(x[1]));
        }

        #endregion

    }

    public static partial class Euclidean
    {
        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="Euclidean2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2f Inverse(Euclidean2f r)
            => r.Inverse;

        /// <summary>
        /// Inverts this rigid transformation (multiplicative inverse).
        /// this = [Rot^T,-Rot^T Trans]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Euclidean2f r)
        {
            r.Rot.Invert();
            r.Trans = -r.Rot.Transform(r.Trans);
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="V3f"/> by an <see cref="Euclidean2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f Transform(this Euclidean2f a, V3f v)
            => a * v;

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f TransformDir(this Euclidean2f r, V2f v)
        {
            return r.Rot.Transform(v);
        }

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f TransformPos(this Euclidean2f r, V2f p)
        {
            return r.Rot.Transform(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f InvTransformDir(this Euclidean2f r, V2f v)
        {
            return r.Rot.InvTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f InvTransformPos(this Euclidean2f r, V2f p)
        {
            return r.Rot.InvTransform(p - r.Trans);
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean2f r0, Euclidean2f r1)
        {
            return ApproximateEquals(r0, r1, Constant<float>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean2f r0, Euclidean2f r1, float tol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, tol) && r0.Rot.ApproximateEquals(r1.Rot, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean2f r0, Euclidean2f r1, float angleTol, float posTol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, posTol) && r0.Rot.ApproximateEquals(r1.Rot, angleTol);
        }

        #endregion
    }

    #endregion

    #region Euclidean3f

    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in 3D that is composed of a 
    /// 3D rotation Rot and a subsequent translation by a 3D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Euclidean3f : IEquatable<Euclidean3f>
    {
        [DataMember]
        public Rot3f Rot;
        [DataMember]
        public V3f Trans;

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="Euclidean3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3f(Euclidean3f e)
        {
            Rot = e.Rot;
            Trans = e.Trans;
        }

        /// <summary>
        /// Constructs a <see cref="Euclidean3f"/> transformation from a <see cref="Euclidean3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3f(Euclidean3d e)
        {
            Rot = (Rot3f)e.Rot;
            Trans = (V3f)e.Trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3f(Rot3f rot)
        {
            Rot = rot;
            Trans = V3f.Zero;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3f(V3f trans)
        {
            Rot = Rot3f.Identity;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation by (<paramref name="tX"/>, <paramref name="tY"/>, <paramref name="tZ"/>).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3f(float tX, float tY, float tZ)
        {
            Rot = Rot3f.Identity;
            Trans = new V3f(tX, tY, tZ);
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3f(Rot3f rot, V3f trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation by (<paramref name="tX"/>, <paramref name="tY"/>, <paramref name="tZ"/>)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3f(Rot3f rot, float tX, float tY, float tZ)
        {
            Rot = rot;
            Trans = new V3f(tX, tY, tZ);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Euclidean3f Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Euclidean3f(Rot3f.Identity, V3f.Zero);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns a new version of this Euclidean transformation with a normalized rotation quaternion.
        /// </summary>
        public Euclidean3f Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Euclidean3f(Rot.Normalized, Trans);
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this Euclidean transformation.
        /// [Rot^T,-Rot^T Trans]
        /// </summary>
        public Euclidean3f Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newR = Rot.Inverse;
                return new Euclidean3f(newR, -newR.Transform(Trans));
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Euclidean transformations.
        /// This concatenates the two rigid transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f operator *(Euclidean3f a, Euclidean3f b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new Euclidean3f(a.Rot * b.Rot, a.Trans + a.Rot.Transform(b.Trans));
        }

        /// <summary>
        /// Transforms a <see cref="V4f"/> vector by a <see cref="Euclidean3f"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f operator *(Euclidean3f e, V4f v)
        {
            var rot = (M33f)e.Rot;
            return new V4f(
                rot.M00 * v.X + rot.M01 * v.Y + rot.M02 * v.Z + e.Trans.X * v.W, 
                rot.M10 * v.X + rot.M11 * v.Y + rot.M12 * v.Z + e.Trans.Y * v.W, 
                rot.M20 * v.X + rot.M21 * v.Y + rot.M22 * v.Z + e.Trans.Z * v.W,
                v.W);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean3f"/> transformation (as a 4x4 matrix) with a <see cref="M44f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44f operator *(Euclidean3f e, M44f m)
        {
            var t = (M34f)e;
            return new M44f(
                t.M00 * m.M00 + t.M01 * m.M10 + t.M02 * m.M20 + t.M03 * m.M30, 
                t.M00 * m.M01 + t.M01 * m.M11 + t.M02 * m.M21 + t.M03 * m.M31, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02 * m.M22 + t.M03 * m.M32, 
                t.M00 * m.M03 + t.M01 * m.M13 + t.M02 * m.M23 + t.M03 * m.M33,

                t.M10 * m.M00 + t.M11 * m.M10 + t.M12 * m.M20 + t.M13 * m.M30, 
                t.M10 * m.M01 + t.M11 * m.M11 + t.M12 * m.M21 + t.M13 * m.M31, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12 * m.M22 + t.M13 * m.M32, 
                t.M10 * m.M03 + t.M11 * m.M13 + t.M12 * m.M23 + t.M13 * m.M33,

                t.M20 * m.M00 + t.M21 * m.M10 + t.M22 * m.M20 + t.M23 * m.M30, 
                t.M20 * m.M01 + t.M21 * m.M11 + t.M22 * m.M21 + t.M23 * m.M31, 
                t.M20 * m.M02 + t.M21 * m.M12 + t.M22 * m.M22 + t.M23 * m.M32, 
                t.M20 * m.M03 + t.M21 * m.M13 + t.M22 * m.M23 + t.M23 * m.M33,

                m.M30, m.M31, m.M32, m.M33);
        }

        /// <summary>
        /// Multiplies a <see cref="M44f"/> with a <see cref="Euclidean3f"/> transformation (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44f operator *(M44f m, Euclidean3f e)
        {
            var t = (M34f)e;
            return new M44f(
                m.M00 * t.M00 + m.M01 * t.M10 + m.M02 * t.M20, 
                m.M00 * t.M01 + m.M01 * t.M11 + m.M02 * t.M21, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02 * t.M22, 
                m.M00 * t.M03 + m.M01 * t.M13 + m.M02 * t.M23 + m.M03,

                m.M10 * t.M00 + m.M11 * t.M10 + m.M12 * t.M20, 
                m.M10 * t.M01 + m.M11 * t.M11 + m.M12 * t.M21, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12 * t.M22, 
                m.M10 * t.M03 + m.M11 * t.M13 + m.M12 * t.M23 + m.M13,

                m.M20 * t.M00 + m.M21 * t.M10 + m.M22 * t.M20, 
                m.M20 * t.M01 + m.M21 * t.M11 + m.M22 * t.M21, 
                m.M20 * t.M02 + m.M21 * t.M12 + m.M22 * t.M22, 
                m.M20 * t.M03 + m.M21 * t.M13 + m.M22 * t.M23 + m.M23,

                m.M30 * t.M00 + m.M31 * t.M10 + m.M32 * t.M20, 
                m.M30 * t.M01 + m.M31 * t.M11 + m.M32 * t.M21, 
                m.M30 * t.M02 + m.M31 * t.M12 + m.M32 * t.M22, 
                m.M30 * t.M03 + m.M31 * t.M13 + m.M32 * t.M23 + m.M33);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean3f"/> transformation (as a 3x4 matrix) with a <see cref="M34f"/> (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(Euclidean3f e, M34f m)
        {
            var t = (M34f)e;
            return new M34f(
                t.M00 * m.M00 + t.M01 * m.M10 + t.M02 * m.M20, 
                t.M00 * m.M01 + t.M01 * m.M11 + t.M02 * m.M21, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02 * m.M22, 
                t.M00 * m.M03 + t.M01 * m.M13 + t.M02 * m.M23 + t.M03,

                t.M10 * m.M00 + t.M11 * m.M10 + t.M12 * m.M20, 
                t.M10 * m.M01 + t.M11 * m.M11 + t.M12 * m.M21, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12 * m.M22, 
                t.M10 * m.M03 + t.M11 * m.M13 + t.M12 * m.M23 + t.M13,

                t.M20 * m.M00 + t.M21 * m.M10 + t.M22 * m.M20, 
                t.M20 * m.M01 + t.M21 * m.M11 + t.M22 * m.M21, 
                t.M20 * m.M02 + t.M21 * m.M12 + t.M22 * m.M22, 
                t.M20 * m.M03 + t.M21 * m.M13 + t.M22 * m.M23 + t.M23);
        }

        /// <summary>
        /// Multiplies a <see cref="M34f"/> with a <see cref="Euclidean3f"/> transformation (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(M34f m, Euclidean3f e)
        {
            var t = (M34f)e;
            return new M34f(
                m.M00 * t.M00 + m.M01 * t.M10 + m.M02 * t.M20, 
                m.M00 * t.M01 + m.M01 * t.M11 + m.M02 * t.M21, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02 * t.M22, 
                m.M00 * t.M03 + m.M01 * t.M13 + m.M02 * t.M23 + m.M03,

                m.M10 * t.M00 + m.M11 * t.M10 + m.M12 * t.M20, 
                m.M10 * t.M01 + m.M11 * t.M11 + m.M12 * t.M21, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12 * t.M22, 
                m.M10 * t.M03 + m.M11 * t.M13 + m.M12 * t.M23 + m.M13,

                m.M20 * t.M00 + m.M21 * t.M10 + m.M22 * t.M20, 
                m.M20 * t.M01 + m.M21 * t.M11 + m.M22 * t.M21, 
                m.M20 * t.M02 + m.M21 * t.M12 + m.M22 * t.M22, 
                m.M20 * t.M03 + m.M21 * t.M13 + m.M22 * t.M23 + m.M23);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean3f"/> (as a 3x4 matrix) and a <see cref="M33f"/> (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(Euclidean3f a, M33f m)
            => new M34f(a.Rot * m, a.Trans);

        /// <summary>
        /// Multiplies a <see cref="M33f"/> and a <see cref="Euclidean3f"/> (as a 3x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(M33f m, Euclidean3f a)
            => new M34f(m * a.Rot, m * a.Trans);

        /// <summary>
        /// Multiplies a <see cref="Euclidean3f"/> and a <see cref="Rot3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f operator *(Euclidean3f e, Rot3f r)
            => new Euclidean3f(e.Rot * r, e.Trans);

        /// <summary>
        /// Multiplies a <see cref="Rot3f"/> and a <see cref="Euclidean3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f operator *(Rot3f r, Euclidean3f e)
            => new Euclidean3f(r * e.Rot, r * e.Trans);

        /// <summary>
        /// Multiplies a <see cref="Euclidean3f"/> and a <see cref="Shift3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f operator *(Euclidean3f e, Shift3f s)
        {
            return new Euclidean3f(e.Rot, e.Rot * s.V + e.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Shift3f"/> and a <see cref="Euclidean3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f operator *(Shift3f s, Euclidean3f e)
        {
            return new Euclidean3f(e.Rot, e.Trans + s.V);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean3f"/> and a <see cref="Scale3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3f operator *(Euclidean3f r, Scale3f s)
        {
            var t = (M33f)r.Rot;
            return new Affine3f(new M33f(
                t.M00 * s.X, t.M01 * s.Y, t.M02 * s.Z, 
                t.M10 * s.X, t.M11 * s.Y, t.M12 * s.Z, 
                t.M20 * s.X, t.M21 * s.Y, t.M22 * s.Z),
                r.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Scale3f"/> and a <see cref="Euclidean3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3f operator *(Scale3f s, Euclidean3f r)
        {
            var t = (M33f)r.Rot;
            return new Affine3f(new M33f(
                t.M00 * s.X, t.M01 * s.X, t.M02 * s.X, 
                t.M10 * s.Y, t.M11 * s.Y, t.M12 * s.Y, 
                t.M20 * s.Z, t.M21 * s.Z, t.M22 * s.Z),
                r.Trans * s.V);
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

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="Euclidean3f"/> transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// The matrix <paramref name="rot"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f FromM33fAndV3f(M33f rot, V3f trans, float epsilon = 1e-5f)
            => new Euclidean3f(Rot3f.FromM33f(rot, epsilon), trans);


        /// <summary>
        /// Creates a <see cref="Euclidean3f"/> transformation from a <see cref="M44f"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components and its upper left 3x3 submatrix must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f FromM44f(M44f m, float epsilon = 1e-5f) 
        {
            if (!(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");
            if (m.M33.IsTiny(epsilon)) throw new ArgumentException("Matrix is not homogeneous.");

            return FromM33fAndV3f(((M33f)m) / m.M33,
                    m.C3.XYZ / m.M33,
                    epsilon);
        }

        /// <summary>
        /// Creates a <see cref="Euclidean3f"/> transformation from a <see cref="M34f"/> matrix.
        /// The left 3x3 submatrix of <paramref name="m"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f FromM34f(M34f m, float epsilon = 1e-5f)
        {
            return FromM33fAndV3f(((M33f)m),
                    m.C3.XYZ,
                    epsilon);
        }

        /// <summary>
        /// Creates a <see cref="Euclidean3f"/> transformation from a <see cref="Similarity3f"/>.
        /// The transformation <paramref name="similarity"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f FromSimilarity3f(Similarity3f similarity, float epsilon = 1e-5f)
        {
            if (!similarity.Scale.ApproximateEquals(1, epsilon))
                throw new ArgumentException("Similarity transformation contains scaling component");

            return similarity.Euclidean;
        }

        /// <summary>
        /// Creates a <see cref="Euclidean3f"/> transformation from an <see cref="Affine3f"/>.
        /// The transformation <paramref name="affine"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f FromAffine3f(Affine3f affine, float epsilon = 1e-5f)
            => FromM44f((M44f)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Euclidean3f"/> transformation from a <see cref="Trafo3f"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f FromTrafo3f(Trafo3f trafo, float epsilon = 1e-5f)
            => FromM44f(trafo.Forward, epsilon);

        #region Translation

        /// <summary>
        /// Creates a <see cref="Euclidean3f"/> transformation with the translational component given by 3 scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f Translation(float tX, float tY, float tZ)
            => new Euclidean3f(tX, tY, tZ);

        /// <summary>
        /// Creates a <see cref="Euclidean3f"/> transformation with the translational component given by a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f Translation(V3f vector)
            => new Euclidean3f(vector);

        /// <summary>
        /// Creates an <see cref="Euclidean3f"/> transformation with the translational component given by a <see cref="Shift3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f Translation(Shift3f shift)
            => new Euclidean3f(shift.V);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="Rot3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f Rotation(Rot3f rot)
            => new Euclidean3f(rot);

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in radians.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f Rotation(V3f normalizedAxis, float angleRadians)
            => new Euclidean3f(Rot3f.Rotation(normalizedAxis, angleRadians));

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in degrees.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationInDegrees(V3f normalizedAxis, float angleDegrees)
            => Rotation(normalizedAxis, angleDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in radians. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationEuler(float rollInRadians, float pitchInRadians, float yawInRadians)
            => new Euclidean3f(Rot3f.RotationEuler(rollInRadians, pitchInRadians, yawInRadians));

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in degrees. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationEulerInDegrees(float rollInDegrees, float pitchInDegrees, float yawInDegrees)
            => RotationEuler(
                rollInDegrees.RadiansFromDegrees(),
                pitchInDegrees.RadiansFromDegrees(),
                yawInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in radians.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationEuler(V3f rollPitchYawInRadians)
            => RotationEuler(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in degrees.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationEulerInDegrees(V3f rollPitchYawInDegrees)
            => RotationEulerInDegrees(
                rollPitchYawInDegrees.X,
                rollPitchYawInDegrees.Y,
                rollPitchYawInDegrees.Z);

        /// <summary>
        /// Creates a rotation transformation which rotates one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotateInto(V3f from, V3f into)
            => new Euclidean3f(Rot3f.RotateInto(from, into));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationX(float angleRadians)
            => new Euclidean3f(Rot3f.RotationX(angleRadians));

        /// <summary>
        /// Creates a rotation transformation for <paramref name="angleDegrees"/> degrees around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationXInDegrees(float angleDegrees)
            => RotationX(angleDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationY(float angleRadians)
            => new Euclidean3f(Rot3f.RotationY(angleRadians));

        /// <summary>
        /// Creates a rotation transformation for <paramref name="angleDegrees"/> degrees around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationYInDegrees(float angleDegrees)
            => RotationY(angleDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationZ(float angleRadians)
            => new Euclidean3f(Rot3f.RotationZ(angleRadians));

        /// <summary>
        /// Creates a rotation transformation for <paramref name="angleDegrees"/> degrees around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f RotationZInDegrees(float angleDegrees)
            => RotationZ(angleDegrees.RadiansFromDegrees());

        #endregion

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M34f(Euclidean3f e)
            => new M34f((M33f)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33f(Euclidean3f e)
            => (M33f)e.Rot;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M44f(Euclidean3f e)
        {
            M44f rv = (M44f)e.Rot;
            rv.C3 = e.Trans.XYZI;
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity3f(Euclidean3f e)
            => new Similarity3f(1, e);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine3f(Euclidean3f e)
            => new Affine3f((M33f)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo3f(Euclidean3f e)
            => new Trafo3f((M44f)e, (M44f)e.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Euclidean3d(Euclidean3f e)
            => new Euclidean3d((Rot3d)e.Rot, (V3d)e.Trans);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Euclidean3f other)
            => Rot.Equals(other.Rot) && Trans.Equals(other.Trans);

        public override bool Equals(object other)
            => (other is Euclidean3f o) ? Equals(o) : false;

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

    public static partial class Euclidean
    {
        #region Normalize

        /// <summary>
        /// Returns a copy of a <see cref="Euclidean3f"/> with its rotation quaternion normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f Normalized(Euclidean3f r)
            => r.Normalized;

        /// <summary>
        /// Normalizes the rotation quaternion of a <see cref="Euclidean3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Euclidean3f r)
        {
            r.Rot.Normalize();
        }

        #endregion

        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="Euclidean3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3f Inverse(Euclidean3f r)
            => r.Inverse;

        /// <summary>
        /// Inverts this rigid transformation (multiplicative inverse).
        /// this = [Rot^T,-Rot^T Trans]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Euclidean3f r)
        {
            r.Rot.Invert();
            r.Trans = -r.Rot.Transform(r.Trans);
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="V4f"/> by an <see cref="Euclidean3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f Transform(this Euclidean3f a, V4f v)
            => a * v;

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f TransformDir(this Euclidean3f r, V3f v)
        {
            return r.Rot.Transform(v);
        }

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f TransformPos(this Euclidean3f r, V3f p)
        {
            return r.Rot.Transform(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransformDir(this Euclidean3f r, V3f v)
        {
            return r.Rot.InvTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransformPos(this Euclidean3f r, V3f p)
        {
            return r.Rot.InvTransform(p - r.Trans);
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean3f r0, Euclidean3f r1)
        {
            return ApproximateEquals(r0, r1, Constant<float>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean3f r0, Euclidean3f r1, float tol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, tol) && r0.Rot.ApproximateEquals(r1.Rot, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean3f r0, Euclidean3f r1, float angleTol, float posTol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, posTol) && r0.Rot.ApproximateEquals(r1.Rot, angleTol);
        }

        #endregion
    }

    #endregion

    #region Euclidean2d

    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in 2D that is composed of a 
    /// 2D rotation Rot and a subsequent translation by a 2D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Euclidean2d : IEquatable<Euclidean2d>
    {
        [DataMember]
        public Rot2d Rot;
        [DataMember]
        public V2d Trans;

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="Euclidean2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2d(Euclidean2d e)
        {
            Rot = e.Rot;
            Trans = e.Trans;
        }

        /// <summary>
        /// Constructs a <see cref="Euclidean2d"/> transformation from a <see cref="Euclidean2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2d(Euclidean2f e)
        {
            Rot = (Rot2d)e.Rot;
            Trans = (V2d)e.Trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2d(Rot2d rot)
        {
            Rot = rot;
            Trans = V2d.Zero;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2d(V2d trans)
        {
            Rot = Rot2d.Identity;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation by (<paramref name="tX"/>, <paramref name="tY"/>).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2d(double tX, double tY)
        {
            Rot = Rot2d.Identity;
            Trans = new V2d(tX, tY);
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2d(Rot2d rot, V2d trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation by (<paramref name="tX"/>, <paramref name="tY"/>)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean2d(Rot2d rot, double tX, double tY)
        {
            Rot = rot;
            Trans = new V2d(tX, tY);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Euclidean2d Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Euclidean2d(Rot2d.Identity, V2d.Zero);
        }

        #endregion

        #region Properties


        /// <summary>
        /// Gets the (multiplicative) inverse of this Euclidean transformation.
        /// [Rot^T,-Rot^T Trans]
        /// </summary>
        public Euclidean2d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newR = Rot.Inverse;
                return new Euclidean2d(newR, -newR.Transform(Trans));
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Euclidean transformations.
        /// This concatenates the two rigid transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d operator *(Euclidean2d a, Euclidean2d b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new Euclidean2d(a.Rot * b.Rot, a.Trans + a.Rot.Transform(b.Trans));
        }

        /// <summary>
        /// Transforms a <see cref="V3d"/> vector by a <see cref="Euclidean2d"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d operator *(Euclidean2d e, V3d v)
        {
            var rot = (M22d)e.Rot;
            return new V3d(
                rot.M00 * v.X + rot.M01 * v.Y + e.Trans.X * v.Z, 
                rot.M10 * v.X + rot.M11 * v.Y + e.Trans.Y * v.Z,
                v.Z);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean2d"/> transformation (as a 3x3 matrix) with a <see cref="M33d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(Euclidean2d e, M33d m)
        {
            var t = (M23d)e;
            return new M33d(
                t.M00 * m.M00 + t.M01 * m.M10 + t.M02 * m.M20, 
                t.M00 * m.M01 + t.M01 * m.M11 + t.M02 * m.M21, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02 * m.M22,

                t.M10 * m.M00 + t.M11 * m.M10 + t.M12 * m.M20, 
                t.M10 * m.M01 + t.M11 * m.M11 + t.M12 * m.M21, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12 * m.M22,

                m.M20, m.M21, m.M22);
        }

        /// <summary>
        /// Multiplies a <see cref="M33d"/> with a <see cref="Euclidean2d"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(M33d m, Euclidean2d e)
        {
            var t = (M23d)e;
            return new M33d(
                m.M00 * t.M00 + m.M01 * t.M10, 
                m.M00 * t.M01 + m.M01 * t.M11, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02,

                m.M10 * t.M00 + m.M11 * t.M10, 
                m.M10 * t.M01 + m.M11 * t.M11, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12,

                m.M20 * t.M00 + m.M21 * t.M10, 
                m.M20 * t.M01 + m.M21 * t.M11, 
                m.M20 * t.M02 + m.M21 * t.M12 + m.M22);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean2d"/> transformation (as a 2x3 matrix) with a <see cref="M23d"/> (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23d operator *(Euclidean2d e, M23d m)
        {
            var t = (M23d)e;
            return new M23d(
                t.M00 * m.M00 + t.M01 * m.M10, 
                t.M00 * m.M01 + t.M01 * m.M11, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02,

                t.M10 * m.M00 + t.M11 * m.M10, 
                t.M10 * m.M01 + t.M11 * m.M11, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12);
        }

        /// <summary>
        /// Multiplies a <see cref="M23d"/> with a <see cref="Euclidean2d"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23d operator *(M23d m, Euclidean2d e)
        {
            var t = (M23d)e;
            return new M23d(
                m.M00 * t.M00 + m.M01 * t.M10, 
                m.M00 * t.M01 + m.M01 * t.M11, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02,

                m.M10 * t.M00 + m.M11 * t.M10, 
                m.M10 * t.M01 + m.M11 * t.M11, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean2d"/> (as a 2x3 matrix) and a <see cref="M22d"/> (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23d operator *(Euclidean2d a, M22d m)
            => new M23d(a.Rot * m, a.Trans);

        /// <summary>
        /// Multiplies a <see cref="M22d"/> and a <see cref="Euclidean2d"/> (as a 2x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23d operator *(M22d m, Euclidean2d a)
            => new M23d(m * a.Rot, m * a.Trans);

        /// <summary>
        /// Multiplies a <see cref="Euclidean2d"/> and a <see cref="Rot2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d operator *(Euclidean2d e, Rot2d r)
            => new Euclidean2d(e.Rot * r, e.Trans);

        /// <summary>
        /// Multiplies a <see cref="Rot2d"/> and a <see cref="Euclidean2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d operator *(Rot2d r, Euclidean2d e)
            => new Euclidean2d(r * e.Rot, r * e.Trans);

        /// <summary>
        /// Multiplies a <see cref="Euclidean2d"/> and a <see cref="Shift2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d operator *(Euclidean2d e, Shift2d s)
        {
            return new Euclidean2d(e.Rot, e.Rot * s.V + e.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Shift2d"/> and a <see cref="Euclidean2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d operator *(Shift2d s, Euclidean2d e)
        {
            return new Euclidean2d(e.Rot, e.Trans + s.V);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean2d"/> and a <see cref="Scale2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine2d operator *(Euclidean2d r, Scale2d s)
        {
            var t = (M22d)r.Rot;
            return new Affine2d(new M22d(
                t.M00 * s.X, t.M01 * s.Y, 
                t.M10 * s.X, t.M11 * s.Y),
                r.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Scale2d"/> and a <see cref="Euclidean2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine2d operator *(Scale2d s, Euclidean2d r)
        {
            var t = (M22d)r.Rot;
            return new Affine2d(new M22d(
                t.M00 * s.X, t.M01 * s.X, 
                t.M10 * s.Y, t.M11 * s.Y),
                r.Trans * s.V);
        }

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

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="Euclidean2d"/> transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// The matrix <paramref name="rot"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d FromM22dAndV2d(M22d rot, V2d trans)
            => new Euclidean2d(Rot2d.FromM22d(rot), trans);


        /// <summary>
        /// Creates a <see cref="Euclidean2d"/> transformation from a <see cref="M33d"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components and its upper left 2x2 submatrix must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d FromM33d(M33d m, double epsilon = 1e-12) 
        {
            if (!(m.M20.IsTiny(epsilon) && m.M21.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");
            if (m.M22.IsTiny(epsilon)) throw new ArgumentException("Matrix is not homogeneous.");

            return FromM22dAndV2d(((M22d)m) / m.M22,
                    m.C2.XY / m.M22);
        }

        /// <summary>
        /// Creates a <see cref="Euclidean2d"/> transformation from a <see cref="M23d"/> matrix.
        /// The left 2x2 submatrix of <paramref name="m"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d FromM23d(M23d m, double epsilon = 1e-12)
        {
            return FromM22dAndV2d(((M22d)m),
                    m.C2.XY);
        }

        /// <summary>
        /// Creates a <see cref="Euclidean2d"/> transformation from a <see cref="Similarity2d"/>.
        /// The transformation <paramref name="similarity"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d FromSimilarity2d(Similarity2d similarity, double epsilon = 1e-12)
        {
            if (!similarity.Scale.ApproximateEquals(1, epsilon))
                throw new ArgumentException("Similarity transformation contains scaling component");

            return similarity.Euclidean;
        }

        /// <summary>
        /// Creates a <see cref="Euclidean2d"/> transformation from an <see cref="Affine2d"/>.
        /// The transformation <paramref name="affine"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d FromAffine2d(Affine2d affine, double epsilon = 1e-12)
            => FromM33d((M33d)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Euclidean2d"/> transformation from a <see cref="Trafo2d"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d FromTrafo2d(Trafo2d trafo, double epsilon = 1e-12)
            => FromM33d(trafo.Forward, epsilon);

        #region Translation

        /// <summary>
        /// Creates a <see cref="Euclidean2d"/> transformation with the translational component given by 2 scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d Translation(double tX, double tY)
            => new Euclidean2d(tX, tY);

        /// <summary>
        /// Creates a <see cref="Euclidean2d"/> transformation with the translational component given by a <see cref="V2d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d Translation(V2d vector)
            => new Euclidean2d(vector);

        /// <summary>
        /// Creates an <see cref="Euclidean2d"/> transformation with the translational component given by a <see cref="Shift2d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d Translation(Shift2d shift)
            => new Euclidean2d(shift.V);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="Rot2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d Rotation(Rot2d rot)
            => new Euclidean2d(rot);

        /// <summary>
        /// Creates a rotation transformation with the specified angle in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d Rotation(double angleInRadians)
            => new Euclidean2d(new Rot2d(angleInRadians));

        /// <summary>
        /// Creates a rotation transformation with the specified angle in degrees.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d RotationInDegrees(double angleInDegrees)
            => Rotation(angleInDegrees.RadiansFromDegrees());

        #endregion

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M23d(Euclidean2d e)
            => new M23d((M22d)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M22d(Euclidean2d e)
            => (M22d)e.Rot;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33d(Euclidean2d e)
        {
            M33d rv = (M33d)e.Rot;
            rv.C2 = e.Trans.XYI;
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity2d(Euclidean2d e)
            => new Similarity2d(1, e);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine2d(Euclidean2d e)
            => new Affine2d((M22d)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo2d(Euclidean2d e)
            => new Trafo2d((M33d)e, (M33d)e.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Euclidean2f(Euclidean2d e)
            => new Euclidean2f((Rot2f)e.Rot, (V2f)e.Trans);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Euclidean2d other)
            => Rot.Equals(other.Rot) && Trans.Equals(other.Trans);

        public override bool Equals(object other)
            => (other is Euclidean2d o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Rot, Trans);
        }

        public static Euclidean2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Euclidean2d(Rot2d.Parse(x[0]), V2d.Parse(x[1]));
        }

        #endregion

    }

    public static partial class Euclidean
    {
        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="Euclidean2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean2d Inverse(Euclidean2d r)
            => r.Inverse;

        /// <summary>
        /// Inverts this rigid transformation (multiplicative inverse).
        /// this = [Rot^T,-Rot^T Trans]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Euclidean2d r)
        {
            r.Rot.Invert();
            r.Trans = -r.Rot.Transform(r.Trans);
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="V3d"/> by an <see cref="Euclidean2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d Transform(this Euclidean2d a, V3d v)
            => a * v;

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d TransformDir(this Euclidean2d r, V2d v)
        {
            return r.Rot.Transform(v);
        }

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d TransformPos(this Euclidean2d r, V2d p)
        {
            return r.Rot.Transform(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d InvTransformDir(this Euclidean2d r, V2d v)
        {
            return r.Rot.InvTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d InvTransformPos(this Euclidean2d r, V2d p)
        {
            return r.Rot.InvTransform(p - r.Trans);
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean2d r0, Euclidean2d r1)
        {
            return ApproximateEquals(r0, r1, Constant<double>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean2d r0, Euclidean2d r1, double tol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, tol) && r0.Rot.ApproximateEquals(r1.Rot, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean2d r0, Euclidean2d r1, double angleTol, double posTol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, posTol) && r0.Rot.ApproximateEquals(r1.Rot, angleTol);
        }

        #endregion
    }

    #endregion

    #region Euclidean3d

    /// <summary>
    /// Represents a Rigid Transformation (or Rigid Body Transformation) in 3D that is composed of a 
    /// 3D rotation Rot and a subsequent translation by a 3D vector Trans.
    /// This is also called an Euclidean Transformation and is a length preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Euclidean3d : IEquatable<Euclidean3d>
    {
        [DataMember]
        public Rot3d Rot;
        [DataMember]
        public V3d Trans;

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="Euclidean3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3d(Euclidean3d e)
        {
            Rot = e.Rot;
            Trans = e.Trans;
        }

        /// <summary>
        /// Constructs a <see cref="Euclidean3d"/> transformation from a <see cref="Euclidean3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3d(Euclidean3f e)
        {
            Rot = (Rot3d)e.Rot;
            Trans = (V3d)e.Trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3d(Rot3d rot)
        {
            Rot = rot;
            Trans = V3d.Zero;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3d(V3d trans)
        {
            Rot = Rot3d.Identity;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a translation by (<paramref name="tX"/>, <paramref name="tY"/>, <paramref name="tZ"/>).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3d(double tX, double tY, double tZ)
        {
            Rot = Rot3d.Identity;
            Trans = new V3d(tX, tY, tZ);
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3d(Rot3d rot, V3d trans)
        {
            Rot = rot;
            Trans = trans;
        }

        /// <summary>
        /// Creates a rigid transformation from a rotation <paramref name="rot"/> and a (subsequent) translation by (<paramref name="tX"/>, <paramref name="tY"/>, <paramref name="tZ"/>)
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Euclidean3d(Rot3d rot, double tX, double tY, double tZ)
        {
            Rot = rot;
            Trans = new V3d(tX, tY, tZ);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Euclidean3d Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Euclidean3d(Rot3d.Identity, V3d.Zero);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Returns a new version of this Euclidean transformation with a normalized rotation quaternion.
        /// </summary>
        public Euclidean3d Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Euclidean3d(Rot.Normalized, Trans);
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this Euclidean transformation.
        /// [Rot^T,-Rot^T Trans]
        /// </summary>
        public Euclidean3d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newR = Rot.Inverse;
                return new Euclidean3d(newR, -newR.Transform(Trans));
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Euclidean transformations.
        /// This concatenates the two rigid transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d operator *(Euclidean3d a, Euclidean3d b)
        {
            //a.Rot * b.Rot, a.Trans + a.Rot * b.Trans
            return new Euclidean3d(a.Rot * b.Rot, a.Trans + a.Rot.Transform(b.Trans));
        }

        /// <summary>
        /// Transforms a <see cref="V4d"/> vector by a <see cref="Euclidean3d"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d operator *(Euclidean3d e, V4d v)
        {
            var rot = (M33d)e.Rot;
            return new V4d(
                rot.M00 * v.X + rot.M01 * v.Y + rot.M02 * v.Z + e.Trans.X * v.W, 
                rot.M10 * v.X + rot.M11 * v.Y + rot.M12 * v.Z + e.Trans.Y * v.W, 
                rot.M20 * v.X + rot.M21 * v.Y + rot.M22 * v.Z + e.Trans.Z * v.W,
                v.W);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean3d"/> transformation (as a 4x4 matrix) with a <see cref="M44d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44d operator *(Euclidean3d e, M44d m)
        {
            var t = (M34d)e;
            return new M44d(
                t.M00 * m.M00 + t.M01 * m.M10 + t.M02 * m.M20 + t.M03 * m.M30, 
                t.M00 * m.M01 + t.M01 * m.M11 + t.M02 * m.M21 + t.M03 * m.M31, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02 * m.M22 + t.M03 * m.M32, 
                t.M00 * m.M03 + t.M01 * m.M13 + t.M02 * m.M23 + t.M03 * m.M33,

                t.M10 * m.M00 + t.M11 * m.M10 + t.M12 * m.M20 + t.M13 * m.M30, 
                t.M10 * m.M01 + t.M11 * m.M11 + t.M12 * m.M21 + t.M13 * m.M31, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12 * m.M22 + t.M13 * m.M32, 
                t.M10 * m.M03 + t.M11 * m.M13 + t.M12 * m.M23 + t.M13 * m.M33,

                t.M20 * m.M00 + t.M21 * m.M10 + t.M22 * m.M20 + t.M23 * m.M30, 
                t.M20 * m.M01 + t.M21 * m.M11 + t.M22 * m.M21 + t.M23 * m.M31, 
                t.M20 * m.M02 + t.M21 * m.M12 + t.M22 * m.M22 + t.M23 * m.M32, 
                t.M20 * m.M03 + t.M21 * m.M13 + t.M22 * m.M23 + t.M23 * m.M33,

                m.M30, m.M31, m.M32, m.M33);
        }

        /// <summary>
        /// Multiplies a <see cref="M44d"/> with a <see cref="Euclidean3d"/> transformation (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44d operator *(M44d m, Euclidean3d e)
        {
            var t = (M34d)e;
            return new M44d(
                m.M00 * t.M00 + m.M01 * t.M10 + m.M02 * t.M20, 
                m.M00 * t.M01 + m.M01 * t.M11 + m.M02 * t.M21, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02 * t.M22, 
                m.M00 * t.M03 + m.M01 * t.M13 + m.M02 * t.M23 + m.M03,

                m.M10 * t.M00 + m.M11 * t.M10 + m.M12 * t.M20, 
                m.M10 * t.M01 + m.M11 * t.M11 + m.M12 * t.M21, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12 * t.M22, 
                m.M10 * t.M03 + m.M11 * t.M13 + m.M12 * t.M23 + m.M13,

                m.M20 * t.M00 + m.M21 * t.M10 + m.M22 * t.M20, 
                m.M20 * t.M01 + m.M21 * t.M11 + m.M22 * t.M21, 
                m.M20 * t.M02 + m.M21 * t.M12 + m.M22 * t.M22, 
                m.M20 * t.M03 + m.M21 * t.M13 + m.M22 * t.M23 + m.M23,

                m.M30 * t.M00 + m.M31 * t.M10 + m.M32 * t.M20, 
                m.M30 * t.M01 + m.M31 * t.M11 + m.M32 * t.M21, 
                m.M30 * t.M02 + m.M31 * t.M12 + m.M32 * t.M22, 
                m.M30 * t.M03 + m.M31 * t.M13 + m.M32 * t.M23 + m.M33);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean3d"/> transformation (as a 3x4 matrix) with a <see cref="M34d"/> (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(Euclidean3d e, M34d m)
        {
            var t = (M34d)e;
            return new M34d(
                t.M00 * m.M00 + t.M01 * m.M10 + t.M02 * m.M20, 
                t.M00 * m.M01 + t.M01 * m.M11 + t.M02 * m.M21, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02 * m.M22, 
                t.M00 * m.M03 + t.M01 * m.M13 + t.M02 * m.M23 + t.M03,

                t.M10 * m.M00 + t.M11 * m.M10 + t.M12 * m.M20, 
                t.M10 * m.M01 + t.M11 * m.M11 + t.M12 * m.M21, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12 * m.M22, 
                t.M10 * m.M03 + t.M11 * m.M13 + t.M12 * m.M23 + t.M13,

                t.M20 * m.M00 + t.M21 * m.M10 + t.M22 * m.M20, 
                t.M20 * m.M01 + t.M21 * m.M11 + t.M22 * m.M21, 
                t.M20 * m.M02 + t.M21 * m.M12 + t.M22 * m.M22, 
                t.M20 * m.M03 + t.M21 * m.M13 + t.M22 * m.M23 + t.M23);
        }

        /// <summary>
        /// Multiplies a <see cref="M34d"/> with a <see cref="Euclidean3d"/> transformation (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(M34d m, Euclidean3d e)
        {
            var t = (M34d)e;
            return new M34d(
                m.M00 * t.M00 + m.M01 * t.M10 + m.M02 * t.M20, 
                m.M00 * t.M01 + m.M01 * t.M11 + m.M02 * t.M21, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02 * t.M22, 
                m.M00 * t.M03 + m.M01 * t.M13 + m.M02 * t.M23 + m.M03,

                m.M10 * t.M00 + m.M11 * t.M10 + m.M12 * t.M20, 
                m.M10 * t.M01 + m.M11 * t.M11 + m.M12 * t.M21, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12 * t.M22, 
                m.M10 * t.M03 + m.M11 * t.M13 + m.M12 * t.M23 + m.M13,

                m.M20 * t.M00 + m.M21 * t.M10 + m.M22 * t.M20, 
                m.M20 * t.M01 + m.M21 * t.M11 + m.M22 * t.M21, 
                m.M20 * t.M02 + m.M21 * t.M12 + m.M22 * t.M22, 
                m.M20 * t.M03 + m.M21 * t.M13 + m.M22 * t.M23 + m.M23);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean3d"/> (as a 3x4 matrix) and a <see cref="M33d"/> (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(Euclidean3d a, M33d m)
            => new M34d(a.Rot * m, a.Trans);

        /// <summary>
        /// Multiplies a <see cref="M33d"/> and a <see cref="Euclidean3d"/> (as a 3x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(M33d m, Euclidean3d a)
            => new M34d(m * a.Rot, m * a.Trans);

        /// <summary>
        /// Multiplies a <see cref="Euclidean3d"/> and a <see cref="Rot3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d operator *(Euclidean3d e, Rot3d r)
            => new Euclidean3d(e.Rot * r, e.Trans);

        /// <summary>
        /// Multiplies a <see cref="Rot3d"/> and a <see cref="Euclidean3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d operator *(Rot3d r, Euclidean3d e)
            => new Euclidean3d(r * e.Rot, r * e.Trans);

        /// <summary>
        /// Multiplies a <see cref="Euclidean3d"/> and a <see cref="Shift3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d operator *(Euclidean3d e, Shift3d s)
        {
            return new Euclidean3d(e.Rot, e.Rot * s.V + e.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Shift3d"/> and a <see cref="Euclidean3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d operator *(Shift3d s, Euclidean3d e)
        {
            return new Euclidean3d(e.Rot, e.Trans + s.V);
        }

        /// <summary>
        /// Multiplies a <see cref="Euclidean3d"/> and a <see cref="Scale3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3d operator *(Euclidean3d r, Scale3d s)
        {
            var t = (M33d)r.Rot;
            return new Affine3d(new M33d(
                t.M00 * s.X, t.M01 * s.Y, t.M02 * s.Z, 
                t.M10 * s.X, t.M11 * s.Y, t.M12 * s.Z, 
                t.M20 * s.X, t.M21 * s.Y, t.M22 * s.Z),
                r.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Scale3d"/> and a <see cref="Euclidean3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3d operator *(Scale3d s, Euclidean3d r)
        {
            var t = (M33d)r.Rot;
            return new Affine3d(new M33d(
                t.M00 * s.X, t.M01 * s.X, t.M02 * s.X, 
                t.M10 * s.Y, t.M11 * s.Y, t.M12 * s.Y, 
                t.M20 * s.Z, t.M21 * s.Z, t.M22 * s.Z),
                r.Trans * s.V);
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

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="Euclidean3d"/> transformation from a rotation matrix <paramref name="rot"/> and a (subsequent) translation <paramref name="trans"/>.
        /// The matrix <paramref name="rot"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d FromM33dAndV3d(M33d rot, V3d trans, double epsilon = 1e-12)
            => new Euclidean3d(Rot3d.FromM33d(rot, epsilon), trans);


        /// <summary>
        /// Creates a <see cref="Euclidean3d"/> transformation from a <see cref="M44d"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components and its upper left 3x3 submatrix must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d FromM44d(M44d m, double epsilon = 1e-12) 
        {
            if (!(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");
            if (m.M33.IsTiny(epsilon)) throw new ArgumentException("Matrix is not homogeneous.");

            return FromM33dAndV3d(((M33d)m) / m.M33,
                    m.C3.XYZ / m.M33,
                    epsilon);
        }

        /// <summary>
        /// Creates a <see cref="Euclidean3d"/> transformation from a <see cref="M34d"/> matrix.
        /// The left 3x3 submatrix of <paramref name="m"/> must be a valid rotation matrix.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d FromM34d(M34d m, double epsilon = 1e-12)
        {
            return FromM33dAndV3d(((M33d)m),
                    m.C3.XYZ,
                    epsilon);
        }

        /// <summary>
        /// Creates a <see cref="Euclidean3d"/> transformation from a <see cref="Similarity3d"/>.
        /// The transformation <paramref name="similarity"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d FromSimilarity3d(Similarity3d similarity, double epsilon = 1e-12)
        {
            if (!similarity.Scale.ApproximateEquals(1, epsilon))
                throw new ArgumentException("Similarity transformation contains scaling component");

            return similarity.Euclidean;
        }

        /// <summary>
        /// Creates a <see cref="Euclidean3d"/> transformation from an <see cref="Affine3d"/>.
        /// The transformation <paramref name="affine"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d FromAffine3d(Affine3d affine, double epsilon = 1e-12)
            => FromM44d((M44d)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Euclidean3d"/> transformation from a <see cref="Trafo3d"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a rotation and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d FromTrafo3d(Trafo3d trafo, double epsilon = 1e-12)
            => FromM44d(trafo.Forward, epsilon);

        #region Translation

        /// <summary>
        /// Creates a <see cref="Euclidean3d"/> transformation with the translational component given by 3 scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d Translation(double tX, double tY, double tZ)
            => new Euclidean3d(tX, tY, tZ);

        /// <summary>
        /// Creates a <see cref="Euclidean3d"/> transformation with the translational component given by a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d Translation(V3d vector)
            => new Euclidean3d(vector);

        /// <summary>
        /// Creates an <see cref="Euclidean3d"/> transformation with the translational component given by a <see cref="Shift3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d Translation(Shift3d shift)
            => new Euclidean3d(shift.V);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="Rot3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d Rotation(Rot3d rot)
            => new Euclidean3d(rot);

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in radians.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d Rotation(V3d normalizedAxis, double angleRadians)
            => new Euclidean3d(Rot3d.Rotation(normalizedAxis, angleRadians));

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in degrees.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationInDegrees(V3d normalizedAxis, double angleDegrees)
            => Rotation(normalizedAxis, angleDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in radians. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationEuler(double rollInRadians, double pitchInRadians, double yawInRadians)
            => new Euclidean3d(Rot3d.RotationEuler(rollInRadians, pitchInRadians, yawInRadians));

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in degrees. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationEulerInDegrees(double rollInDegrees, double pitchInDegrees, double yawInDegrees)
            => RotationEuler(
                rollInDegrees.RadiansFromDegrees(),
                pitchInDegrees.RadiansFromDegrees(),
                yawInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in radians.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationEuler(V3d rollPitchYawInRadians)
            => RotationEuler(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in degrees.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationEulerInDegrees(V3d rollPitchYawInDegrees)
            => RotationEulerInDegrees(
                rollPitchYawInDegrees.X,
                rollPitchYawInDegrees.Y,
                rollPitchYawInDegrees.Z);

        /// <summary>
        /// Creates a rotation transformation which rotates one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotateInto(V3d from, V3d into)
            => new Euclidean3d(Rot3d.RotateInto(from, into));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationX(double angleRadians)
            => new Euclidean3d(Rot3d.RotationX(angleRadians));

        /// <summary>
        /// Creates a rotation transformation for <paramref name="angleDegrees"/> degrees around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationXInDegrees(double angleDegrees)
            => RotationX(angleDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationY(double angleRadians)
            => new Euclidean3d(Rot3d.RotationY(angleRadians));

        /// <summary>
        /// Creates a rotation transformation for <paramref name="angleDegrees"/> degrees around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationYInDegrees(double angleDegrees)
            => RotationY(angleDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleRadians"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationZ(double angleRadians)
            => new Euclidean3d(Rot3d.RotationZ(angleRadians));

        /// <summary>
        /// Creates a rotation transformation for <paramref name="angleDegrees"/> degrees around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d RotationZInDegrees(double angleDegrees)
            => RotationZ(angleDegrees.RadiansFromDegrees());

        #endregion

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M34d(Euclidean3d e)
            => new M34d((M33d)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33d(Euclidean3d e)
            => (M33d)e.Rot;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M44d(Euclidean3d e)
        {
            M44d rv = (M44d)e.Rot;
            rv.C3 = e.Trans.XYZI;
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity3d(Euclidean3d e)
            => new Similarity3d(1, e);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine3d(Euclidean3d e)
            => new Affine3d((M33d)e.Rot, e.Trans);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo3d(Euclidean3d e)
            => new Trafo3d((M44d)e, (M44d)e.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Euclidean3f(Euclidean3d e)
            => new Euclidean3f((Rot3f)e.Rot, (V3f)e.Trans);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Rot, Trans);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Euclidean3d other)
            => Rot.Equals(other.Rot) && Trans.Equals(other.Trans);

        public override bool Equals(object other)
            => (other is Euclidean3d o) ? Equals(o) : false;

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

    public static partial class Euclidean
    {
        #region Normalize

        /// <summary>
        /// Returns a copy of a <see cref="Euclidean3d"/> with its rotation quaternion normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d Normalized(Euclidean3d r)
            => r.Normalized;

        /// <summary>
        /// Normalizes the rotation quaternion of a <see cref="Euclidean3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Euclidean3d r)
        {
            r.Rot.Normalize();
        }

        #endregion

        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="Euclidean3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Euclidean3d Inverse(Euclidean3d r)
            => r.Inverse;

        /// <summary>
        /// Inverts this rigid transformation (multiplicative inverse).
        /// this = [Rot^T,-Rot^T Trans]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Euclidean3d r)
        {
            r.Rot.Invert();
            r.Trans = -r.Rot.Transform(r.Trans);
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="V4d"/> by an <see cref="Euclidean3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d Transform(this Euclidean3d a, V4d v)
            => a * v;

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d TransformDir(this Euclidean3d r, V3d v)
        {
            return r.Rot.Transform(v);
        }

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d TransformPos(this Euclidean3d r, V3d p)
        {
            return r.Rot.Transform(p) + r.Trans;
        }

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransformDir(this Euclidean3d r, V3d v)
        {
            return r.Rot.InvTransform(v);
        }

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of the rigid transformation r.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransformPos(this Euclidean3d r, V3d p)
        {
            return r.Rot.InvTransform(p - r.Trans);
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean3d r0, Euclidean3d r1)
        {
            return ApproximateEquals(r0, r1, Constant<double>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean3d r0, Euclidean3d r1, double tol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, tol) && r0.Rot.ApproximateEquals(r1.Rot, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Euclidean3d r0, Euclidean3d r1, double angleTol, double posTol)
        {
            return ApproximateEquals(r0.Trans, r1.Trans, posTol) && r0.Rot.ApproximateEquals(r1.Rot, angleTol);
        }

        #endregion
    }

    #endregion

}
