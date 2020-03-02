using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    #region Similarity2f

    /// <summary>
    /// Represents a Similarity Transformation in 2D that is composed of a 
    /// Uniform Scale and a subsequent Euclidean transformation (2D rotation Rot and a subsequent translation by a 2D vector Trans).
    /// This is an angle preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Similarity2f
    {
        [DataMember]
        public float Scale;
        [DataMember]
        public Euclidean2f Euclidean;

        /// <summary>
        /// Gets the rotational component of this <see cref="Similarity2f"/> transformation.
        /// </summary>
        public Rot2f Rot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Rot; }
        }

        /// <summary>
        /// Gets the translational component of this <see cref="Similarity2f"/> transformation.
        /// </summary>
        public V2f Trans
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Trans; }
        }

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="Similarity2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Similarity2f(Similarity2f s)
        {
            Scale = s.Scale;
            Euclidean = s.Euclidean;
        }

        /// <summary>
        /// Creates a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rigid transformation <paramref name="euclideanTransformation"/>.
        /// </summary>
        public Similarity2f(float scale, Euclidean2f euclideanTransformation)
        {
            Scale = scale;
            Euclidean = euclideanTransformation;
        }

        /// <summary>
        /// Constructs a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rotation <paramref name="rotation"/> and translation <paramref name="translation"/>.
        /// </summary>
        public Similarity2f(float scale, Rot2f rotation, V2f translation)
        {
            Scale = scale;
            Euclidean = new Euclidean2f(rotation, translation);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Similarity2f Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Similarity2f(1, Euclidean2f.Identity);
        }

        #endregion

        #region Similarity Transformation Arithmetics

        /// <summary>
        /// Gets the (multiplicative) inverse of this Similarity transformation.
        /// [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        public Similarity2f Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newS = 1 / Scale;
                var newR = Euclidean.Inverse;
                newR.Trans *= newS;
                return new Similarity2f(newS, newR);
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Similarity transformations.
        /// This concatenates the two similarity transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2f operator *(Similarity2f a, Similarity2f b)
        {
            //a.Scale * b.Scale, a.Rot * b.Rot, a.Trans + a.Rot * a.Scale * b.Trans
            return new Similarity2f(a.Scale * b.Scale, new Euclidean2f(
                a.Rot * b.Rot,
                a.Trans + a.Rot.Transform(a.Scale * b.Trans))
                );
        }

        /// <summary>
        /// Transforms a <see cref="V3f"/> vector by a <see cref="Similarity2f"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f operator *(Similarity2f s, V3f v)
            => s.Euclidean * new V3f(s.Scale * v.X, s.Scale * v.Y, v.Z);

        /// <summary>
        /// Multiplies a <see cref="Similarity2f"/> transformation (as a 3x3 matrix) with a <see cref="M33f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(Similarity2f s, M33f m)
        {
            var t = (M23f)s;
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
        /// Multiplies a <see cref="M33f"/> with a <see cref="Similarity2f"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f operator *(M33f m, Similarity2f s)
        {
            var t = (M23f)s;
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
        /// Multiplies a <see cref="Similarity2f"/> transformation (as a 2x3 matrix) with a <see cref="M23f"/> (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23f operator *(Similarity2f s, M23f m)
        {
            var t = (M23f)s;
            return new M23f(
                t.M00 * m.M00 + t.M01 * m.M10, 
                t.M00 * m.M01 + t.M01 * m.M11, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02,

                t.M10 * m.M00 + t.M11 * m.M10, 
                t.M10 * m.M01 + t.M11 * m.M11, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12);
        }

        /// <summary>
        /// Multiplies a <see cref="M23f"/> with a <see cref="Similarity2f"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23f operator *(M23f m, Similarity2f s)
        {
            var t = (M23f)s;
            return new M23f(
                m.M00 * t.M00 + m.M01 * t.M10, 
                m.M00 * t.M01 + m.M01 * t.M11, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02,

                m.M10 * t.M00 + m.M11 * t.M10, 
                m.M10 * t.M01 + m.M11 * t.M11, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12);
        }

        /// <summary>
        /// Multiplies a <see cref="Similarity2f"/> (as a 2x3 matrix) and a <see cref="M22f"/> (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23f operator *(Similarity2f s, M22f m)
            => new M23f(s.Rot * m * s.Scale, s.Trans);

        /// <summary>
        /// Multiplies a <see cref="M22f"/> and a <see cref="Similarity2f"/> (as a 2x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23f operator *(M22f m, Similarity2f s)
            => new M23f(s.Scale * m * s.Rot, m * s.Trans);

        /// <summary>
        /// Multiplies a <see cref="Similarity2f"/> and a <see cref="Rot2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2f operator *(Similarity2f s, Rot2f r)
            => new Similarity2f(s.Scale, s.Euclidean * r);

        /// <summary>
        /// Multiplies a <see cref="Rot2f"/> and a <see cref="Similarity2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2f operator *(Rot2f r, Similarity2f s)
               => new Similarity2f(s.Scale, r * s.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="Similarity2f"/> and a <see cref="Shift2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2f operator *(Similarity2f a, Shift2f b)
            => new Similarity2f(a.Scale, a.Euclidean * (a.Scale * b));

        /// <summary>
        /// Multiplies a <see cref="Shift2f"/> and a <see cref="Similarity2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2f operator *(Shift2f a, Similarity2f b)
            => new Similarity2f(b.Scale, a * b.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="Similarity2f"/> and a <see cref="Scale2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine2f operator *(Similarity2f a, Scale2f b)
        {
            var t = (M22f)a;
            return new Affine2f(new M22f(
                t.M00 * b.X, t.M01 * b.Y, 
                t.M10 * b.X, t.M11 * b.Y),
                a.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Scale2f"/> and a <see cref="Similarity2f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine2f operator *(Scale2f a, Similarity2f b)
        {
            var t = (M22f)b;
            return new Affine2f(new M22f(
                t.M00 * a.X, t.M01 * a.X, 
                t.M10 * a.Y, t.M11 * a.Y),
                b.Trans * a.V);
        }

        /// <summary>
        /// Multiplies an Euclidean transformation by a Similarity transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2f operator *(Euclidean2f a, Similarity2f b)
        {
            return new Similarity2f(b.Scale, a * b.Euclidean);
            //return (Similarity2f)a * b;
        }

        /// <summary>
        /// Multiplies a Similarity transformation by an Euclidean transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2f operator *(Similarity2f a, Euclidean2f b)
        {
            return a * (Similarity2f)b;
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Similarity2f t0, Similarity2f t1)
        {
            return t0.Scale == t1.Scale && t0.Euclidean == t1.Euclidean;
        }

        public static bool operator !=(Similarity2f t0, Similarity2f t1)
        {
            return !(t0 == t1);
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="Similarity2f"/> transformation from a <see cref="M22f"/> matrix and a translation <see cref="V2f"/>.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2f FromM22fAndV2f(M22f m, V2f trans, float epsilon = 1e-5f)
        {
            var s0 = m.C0.Norm2;
            var s1 = m.C1.Norm2;
            var s = (s0 * s1).Pow(1.0f / 2); //geometric mean of scale

            if (!((s0 / s - 1).IsTiny(epsilon) && (s1 / s - 1).IsTiny(epsilon)))
                throw new ArgumentException("Matrix features non-uniform scaling");

            m /= s;
            return new Similarity2f(s, Euclidean2f.FromM22fAndV2f(m, trans));
        }

        /// <summary>
        /// Creates a <see cref="Similarity2f"/> transformation from a <see cref="M23f"/> matrix.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2f FromM23f(M23f m, float epsilon = 1e-5f)
            => FromM22fAndV2f((M22f)m, m.C2);

        /// <summary>
        /// Creates a <see cref="Similarity2f"/> transformation from a <see cref="M33f"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components or
        /// a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2f FromM33f(M33f m, float epsilon = 1e-5f)
        {
            if (!(m.M20.IsTiny(epsilon) && m.M21.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");

            if (m.M22.IsTiny(epsilon))
                throw new ArgumentException("Matrix is not homogeneous.");

            return FromM22fAndV2f(((M22f)m) / m.M22, m.C2.XY);
        }

        /// <summary>
        /// Creates a <see cref="Similarity2f"/> transformation from an <see cref="Scale2f"/>.
        /// The transformation <paramref name="scale"/> must represent a uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2f FromScale2f(Scale2f scale, float epsilon = 1e-5f)
        {
            var s = (scale.X * scale.Y).Pow(1.0f / 2);

            if (!scale.ApproximateEquals(new Scale2f(s), epsilon))
                throw new ArgumentException("Matrix features non-uniform scaling");

            return new Similarity2f(s, Euclidean2f.Identity);
        }

        /// <summary>
        /// Creates a <see cref="Similarity2f"/> transformation from an <see cref="Affine2f"/>.
        /// The transformation <paramref name="affine"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2f FromAffine2f(Affine2f affine, float epsilon = 1e-5f)
            => FromM33f((M33f)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Similarity2f"/> transformation from a <see cref="Trafo2f"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2f FromTrafo2f(Trafo2f trafo, float epsilon = 1e-5f)
            => FromM33f(trafo.Forward, epsilon);

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M23f(Similarity2f s)
        {
            M23f rv = (M23f)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M22f(Similarity2f s)
        {
            M22f rv = (M22f)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33f(Similarity2f s)
        {
            M33f rv = (M33f)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine2f(Similarity2f s)
        {
            var m = (M23f)s;
            return new Affine2f((M22f)m, m.C2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo2f(Similarity2f s)
            => new Trafo2f((M33f)s, (M33f)s.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity2d(Similarity2f s)
            => new Similarity2d((double)s.Scale, (Euclidean2d)s.Euclidean);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Scale, Euclidean);
        }

        public override bool Equals(object other)
        {
            return (other is Similarity2f) ? (this == (Similarity2f)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Scale, Euclidean);
        }

        public static Similarity2f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Similarity2f(float.Parse(x[0], CultureInfo.InvariantCulture), Euclidean2f.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Similarity
    {
        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="Similarity2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2f Inverse(Similarity2f s)
            => s.Inverse;

        /// <summary>
        /// Inverts the similarity transformation (multiplicative inverse).
        /// this = [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Similarity2f t)
        {
            t.Scale = 1 / t.Scale;
            t.Euclidean.Invert();
            t.Euclidean.Trans *= t.Scale;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="V3f"/> by a <see cref="Similarity2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f Transform(this Similarity2f s, V3f v)
            => s * v;

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f TransformDir(this Similarity2f t, V2f v)
        {
            return t.Euclidean.TransformDir(t.Scale * v);
        }

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f TransformPos(this Similarity2f t, V2f p)
        {
            return t.Euclidean.TransformPos(t.Scale * p);
        }

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f InvTransformDir(this Similarity2f t, V2f v)
        {
            return t.Euclidean.InvTransformDir(v) / t.Scale;
        }

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f InvTransformPos(this Similarity2f t, V2f p)
        {
            return t.Euclidean.InvTransformPos(p) / t.Scale;
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity2f t0, Similarity2f t1)
        {
            return ApproximateEquals(t0, t1, Constant<float>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity2f t0, Similarity2f t1, float tol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, tol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity2f t0, Similarity2f t1, float angleTol, float posTol, float scaleTol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, scaleTol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, angleTol, posTol);
        }

        #endregion
    }

    #endregion

    #region Similarity3f

    /// <summary>
    /// Represents a Similarity Transformation in 3D that is composed of a 
    /// Uniform Scale and a subsequent Euclidean transformation (3D rotation Rot and a subsequent translation by a 3D vector Trans).
    /// This is an angle preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Similarity3f
    {
        [DataMember]
        public float Scale;
        [DataMember]
        public Euclidean3f Euclidean;

        /// <summary>
        /// Gets the rotational component of this <see cref="Similarity3f"/> transformation.
        /// </summary>
        public Rot3f Rot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Rot; }
        }

        /// <summary>
        /// Gets the translational component of this <see cref="Similarity3f"/> transformation.
        /// </summary>
        public V3f Trans
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Trans; }
        }

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="Similarity3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Similarity3f(Similarity3f s)
        {
            Scale = s.Scale;
            Euclidean = s.Euclidean;
        }

        /// <summary>
        /// Creates a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rigid transformation <paramref name="euclideanTransformation"/>.
        /// </summary>
        public Similarity3f(float scale, Euclidean3f euclideanTransformation)
        {
            Scale = scale;
            Euclidean = euclideanTransformation;
        }

        /// <summary>
        /// Constructs a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rotation <paramref name="rotation"/> and translation <paramref name="translation"/>.
        /// </summary>
        public Similarity3f(float scale, Rot3f rotation, V3f translation)
        {
            Scale = scale;
            Euclidean = new Euclidean3f(rotation, translation);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Similarity3f Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Similarity3f(1, Euclidean3f.Identity);
        }

        #endregion

        #region Similarity Transformation Arithmetics

        /// <summary>
        /// Returns a new version of this Similarity transformation with a normalized rotation quaternion.
        /// </summary>
        public Similarity3f Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Similarity3f(Scale, Euclidean.Normalized);
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this Similarity transformation.
        /// [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        public Similarity3f Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newS = 1 / Scale;
                var newR = Euclidean.Inverse;
                newR.Trans *= newS;
                return new Similarity3f(newS, newR);
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Similarity transformations.
        /// This concatenates the two similarity transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3f operator *(Similarity3f a, Similarity3f b)
        {
            //a.Scale * b.Scale, a.Rot * b.Rot, a.Trans + a.Rot * a.Scale * b.Trans
            return new Similarity3f(a.Scale * b.Scale, new Euclidean3f(
                a.Rot * b.Rot,
                a.Trans + a.Rot.Transform(a.Scale * b.Trans))
                );
        }

        /// <summary>
        /// Transforms a <see cref="V4f"/> vector by a <see cref="Similarity3f"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f operator *(Similarity3f s, V4f v)
            => s.Euclidean * new V4f(s.Scale * v.X, s.Scale * v.Y, s.Scale * v.Z, v.W);

        /// <summary>
        /// Multiplies a <see cref="Similarity3f"/> transformation (as a 4x4 matrix) with a <see cref="M44f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44f operator *(Similarity3f s, M44f m)
        {
            var t = (M34f)s;
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
        /// Multiplies a <see cref="M44f"/> with a <see cref="Similarity3f"/> transformation (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44f operator *(M44f m, Similarity3f s)
        {
            var t = (M34f)s;
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
        /// Multiplies a <see cref="Similarity3f"/> transformation (as a 3x4 matrix) with a <see cref="M34f"/> (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(Similarity3f s, M34f m)
        {
            var t = (M34f)s;
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
        /// Multiplies a <see cref="M34f"/> with a <see cref="Similarity3f"/> transformation (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(M34f m, Similarity3f s)
        {
            var t = (M34f)s;
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
        /// Multiplies a <see cref="Similarity3f"/> (as a 3x4 matrix) and a <see cref="M33f"/> (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(Similarity3f s, M33f m)
            => new M34f(s.Rot * m * s.Scale, s.Trans);

        /// <summary>
        /// Multiplies a <see cref="M33f"/> and a <see cref="Similarity3f"/> (as a 3x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34f operator *(M33f m, Similarity3f s)
            => new M34f(s.Scale * m * s.Rot, m * s.Trans);

        /// <summary>
        /// Multiplies a <see cref="Similarity3f"/> and a <see cref="Rot3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3f operator *(Similarity3f s, Rot3f r)
            => new Similarity3f(s.Scale, s.Euclidean * r);

        /// <summary>
        /// Multiplies a <see cref="Rot3f"/> and a <see cref="Similarity3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3f operator *(Rot3f r, Similarity3f s)
               => new Similarity3f(s.Scale, r * s.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="Similarity3f"/> and a <see cref="Shift3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3f operator *(Similarity3f a, Shift3f b)
            => new Similarity3f(a.Scale, a.Euclidean * (a.Scale * b));

        /// <summary>
        /// Multiplies a <see cref="Shift3f"/> and a <see cref="Similarity3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3f operator *(Shift3f a, Similarity3f b)
            => new Similarity3f(b.Scale, a * b.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="Similarity3f"/> and a <see cref="Scale3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3f operator *(Similarity3f a, Scale3f b)
        {
            var t = (M33f)a;
            return new Affine3f(new M33f(
                t.M00 * b.X, t.M01 * b.Y, t.M02 * b.Z, 
                t.M10 * b.X, t.M11 * b.Y, t.M12 * b.Z, 
                t.M20 * b.X, t.M21 * b.Y, t.M22 * b.Z),
                a.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Scale3f"/> and a <see cref="Similarity3f"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3f operator *(Scale3f a, Similarity3f b)
        {
            var t = (M33f)b;
            return new Affine3f(new M33f(
                t.M00 * a.X, t.M01 * a.X, t.M02 * a.X, 
                t.M10 * a.Y, t.M11 * a.Y, t.M12 * a.Y, 
                t.M20 * a.Z, t.M21 * a.Z, t.M22 * a.Z),
                b.Trans * a.V);
        }

        /// <summary>
        /// Multiplies an Euclidean transformation by a Similarity transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3f operator *(Euclidean3f a, Similarity3f b)
        {
            return new Similarity3f(b.Scale, a * b.Euclidean);
            //return (Similarity3f)a * b;
        }

        /// <summary>
        /// Multiplies a Similarity transformation by an Euclidean transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3f operator *(Similarity3f a, Euclidean3f b)
        {
            return a * (Similarity3f)b;
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Similarity3f t0, Similarity3f t1)
        {
            return t0.Scale == t1.Scale && t0.Euclidean == t1.Euclidean;
        }

        public static bool operator !=(Similarity3f t0, Similarity3f t1)
        {
            return !(t0 == t1);
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="Similarity3f"/> transformation from a <see cref="M33f"/> matrix and a translation <see cref="V3f"/>.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3f FromM33fAndV3f(M33f m, V3f trans, float epsilon = 1e-5f)
        {
            var s0 = m.C0.Norm2;
            var s1 = m.C1.Norm2;
            var s2 = m.C2.Norm2;
            var s = (s0 * s1 * s2).Pow(1.0f / 3); //geometric mean of scale

            if (!((s0 / s - 1).IsTiny(epsilon) && (s1 / s - 1).IsTiny(epsilon) && (s2 / s - 1).IsTiny(epsilon)))
                throw new ArgumentException("Matrix features non-uniform scaling");

            m /= s;
            return new Similarity3f(s, Euclidean3f.FromM33fAndV3f(m, trans, epsilon));
        }

        /// <summary>
        /// Creates a <see cref="Similarity3f"/> transformation from a <see cref="M34f"/> matrix.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3f FromM34f(M34f m, float epsilon = 1e-5f)
            => FromM33fAndV3f((M33f)m, m.C3, epsilon);

        /// <summary>
        /// Creates a <see cref="Similarity3f"/> transformation from a <see cref="M44f"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components or
        /// a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3f FromM44f(M44f m, float epsilon = 1e-5f)
        {
            if (!(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");

            if (m.M33.IsTiny(epsilon))
                throw new ArgumentException("Matrix is not homogeneous.");

            return FromM33fAndV3f(((M33f)m) / m.M33, m.C3.XYZ, epsilon);
        }

        /// <summary>
        /// Creates a <see cref="Similarity3f"/> transformation from an <see cref="Scale3f"/>.
        /// The transformation <paramref name="scale"/> must represent a uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3f FromScale3f(Scale3f scale, float epsilon = 1e-5f)
        {
            var s = (scale.X * scale.Y * scale.Z).Pow(1.0f / 3);

            if (!scale.ApproximateEquals(new Scale3f(s), epsilon))
                throw new ArgumentException("Matrix features non-uniform scaling");

            return new Similarity3f(s, Euclidean3f.Identity);
        }

        /// <summary>
        /// Creates a <see cref="Similarity3f"/> transformation from an <see cref="Affine3f"/>.
        /// The transformation <paramref name="affine"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3f FromAffine3f(Affine3f affine, float epsilon = 1e-5f)
            => FromM44f((M44f)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Similarity3f"/> transformation from a <see cref="Trafo3f"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3f FromTrafo3f(Trafo3f trafo, float epsilon = 1e-5f)
            => FromM44f(trafo.Forward, epsilon);

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M34f(Similarity3f s)
        {
            M34f rv = (M34f)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; rv.M02 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; rv.M12 *= s.Scale; 
            rv.M20 *= s.Scale; rv.M21 *= s.Scale; rv.M22 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33f(Similarity3f s)
        {
            M33f rv = (M33f)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; rv.M02 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; rv.M12 *= s.Scale; 
            rv.M20 *= s.Scale; rv.M21 *= s.Scale; rv.M22 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M44f(Similarity3f s)
        {
            M44f rv = (M44f)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; rv.M02 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; rv.M12 *= s.Scale; 
            rv.M20 *= s.Scale; rv.M21 *= s.Scale; rv.M22 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine3f(Similarity3f s)
        {
            var m = (M34f)s;
            return new Affine3f((M33f)m, m.C3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo3f(Similarity3f s)
            => new Trafo3f((M44f)s, (M44f)s.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity3d(Similarity3f s)
            => new Similarity3d((double)s.Scale, (Euclidean3d)s.Euclidean);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Scale, Euclidean);
        }

        public override bool Equals(object other)
        {
            return (other is Similarity3f) ? (this == (Similarity3f)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Scale, Euclidean);
        }

        public static Similarity3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Similarity3f(float.Parse(x[0], CultureInfo.InvariantCulture), Euclidean3f.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Similarity
    {
        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="Similarity3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3f Inverse(Similarity3f s)
            => s.Inverse;

        /// <summary>
        /// Inverts the similarity transformation (multiplicative inverse).
        /// this = [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Similarity3f t)
        {
            t.Scale = 1 / t.Scale;
            t.Euclidean.Invert();
            t.Euclidean.Trans *= t.Scale;
        }

        #endregion

        #region Normalize

        /// <summary>
        /// Returns a copy of a <see cref="Similarity3f"/> with its rotation quaternion normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3f Normalized(Similarity3f s)
            => s.Normalized;

        /// <summary>
        /// Normalizes the rotation quaternion of a <see cref="Similarity3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Similarity3f t)
        {
            t.Euclidean.Normalize();
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="V4f"/> by a <see cref="Similarity3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f Transform(this Similarity3f s, V4f v)
            => s * v;

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f TransformDir(this Similarity3f t, V3f v)
        {
            return t.Euclidean.TransformDir(t.Scale * v);
        }

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f TransformPos(this Similarity3f t, V3f p)
        {
            return t.Euclidean.TransformPos(t.Scale * p);
        }

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransformDir(this Similarity3f t, V3f v)
        {
            return t.Euclidean.InvTransformDir(v) / t.Scale;
        }

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransformPos(this Similarity3f t, V3f p)
        {
            return t.Euclidean.InvTransformPos(p) / t.Scale;
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity3f t0, Similarity3f t1)
        {
            return ApproximateEquals(t0, t1, Constant<float>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity3f t0, Similarity3f t1, float tol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, tol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity3f t0, Similarity3f t1, float angleTol, float posTol, float scaleTol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, scaleTol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, angleTol, posTol);
        }

        #endregion
    }

    #endregion

    #region Similarity2d

    /// <summary>
    /// Represents a Similarity Transformation in 2D that is composed of a 
    /// Uniform Scale and a subsequent Euclidean transformation (2D rotation Rot and a subsequent translation by a 2D vector Trans).
    /// This is an angle preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Similarity2d
    {
        [DataMember]
        public double Scale;
        [DataMember]
        public Euclidean2d Euclidean;

        /// <summary>
        /// Gets the rotational component of this <see cref="Similarity2d"/> transformation.
        /// </summary>
        public Rot2d Rot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Rot; }
        }

        /// <summary>
        /// Gets the translational component of this <see cref="Similarity2d"/> transformation.
        /// </summary>
        public V2d Trans
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Trans; }
        }

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="Similarity2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Similarity2d(Similarity2d s)
        {
            Scale = s.Scale;
            Euclidean = s.Euclidean;
        }

        /// <summary>
        /// Creates a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rigid transformation <paramref name="euclideanTransformation"/>.
        /// </summary>
        public Similarity2d(double scale, Euclidean2d euclideanTransformation)
        {
            Scale = scale;
            Euclidean = euclideanTransformation;
        }

        /// <summary>
        /// Constructs a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rotation <paramref name="rotation"/> and translation <paramref name="translation"/>.
        /// </summary>
        public Similarity2d(double scale, Rot2d rotation, V2d translation)
        {
            Scale = scale;
            Euclidean = new Euclidean2d(rotation, translation);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Similarity2d Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Similarity2d(1, Euclidean2d.Identity);
        }

        #endregion

        #region Similarity Transformation Arithmetics

        /// <summary>
        /// Gets the (multiplicative) inverse of this Similarity transformation.
        /// [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        public Similarity2d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newS = 1 / Scale;
                var newR = Euclidean.Inverse;
                newR.Trans *= newS;
                return new Similarity2d(newS, newR);
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Similarity transformations.
        /// This concatenates the two similarity transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2d operator *(Similarity2d a, Similarity2d b)
        {
            //a.Scale * b.Scale, a.Rot * b.Rot, a.Trans + a.Rot * a.Scale * b.Trans
            return new Similarity2d(a.Scale * b.Scale, new Euclidean2d(
                a.Rot * b.Rot,
                a.Trans + a.Rot.Transform(a.Scale * b.Trans))
                );
        }

        /// <summary>
        /// Transforms a <see cref="V3d"/> vector by a <see cref="Similarity2d"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d operator *(Similarity2d s, V3d v)
            => s.Euclidean * new V3d(s.Scale * v.X, s.Scale * v.Y, v.Z);

        /// <summary>
        /// Multiplies a <see cref="Similarity2d"/> transformation (as a 3x3 matrix) with a <see cref="M33d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(Similarity2d s, M33d m)
        {
            var t = (M23d)s;
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
        /// Multiplies a <see cref="M33d"/> with a <see cref="Similarity2d"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d operator *(M33d m, Similarity2d s)
        {
            var t = (M23d)s;
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
        /// Multiplies a <see cref="Similarity2d"/> transformation (as a 2x3 matrix) with a <see cref="M23d"/> (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23d operator *(Similarity2d s, M23d m)
        {
            var t = (M23d)s;
            return new M23d(
                t.M00 * m.M00 + t.M01 * m.M10, 
                t.M00 * m.M01 + t.M01 * m.M11, 
                t.M00 * m.M02 + t.M01 * m.M12 + t.M02,

                t.M10 * m.M00 + t.M11 * m.M10, 
                t.M10 * m.M01 + t.M11 * m.M11, 
                t.M10 * m.M02 + t.M11 * m.M12 + t.M12);
        }

        /// <summary>
        /// Multiplies a <see cref="M23d"/> with a <see cref="Similarity2d"/> transformation (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23d operator *(M23d m, Similarity2d s)
        {
            var t = (M23d)s;
            return new M23d(
                m.M00 * t.M00 + m.M01 * t.M10, 
                m.M00 * t.M01 + m.M01 * t.M11, 
                m.M00 * t.M02 + m.M01 * t.M12 + m.M02,

                m.M10 * t.M00 + m.M11 * t.M10, 
                m.M10 * t.M01 + m.M11 * t.M11, 
                m.M10 * t.M02 + m.M11 * t.M12 + m.M12);
        }

        /// <summary>
        /// Multiplies a <see cref="Similarity2d"/> (as a 2x3 matrix) and a <see cref="M22d"/> (as a 3x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23d operator *(Similarity2d s, M22d m)
            => new M23d(s.Rot * m * s.Scale, s.Trans);

        /// <summary>
        /// Multiplies a <see cref="M22d"/> and a <see cref="Similarity2d"/> (as a 2x3 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M23d operator *(M22d m, Similarity2d s)
            => new M23d(s.Scale * m * s.Rot, m * s.Trans);

        /// <summary>
        /// Multiplies a <see cref="Similarity2d"/> and a <see cref="Rot2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2d operator *(Similarity2d s, Rot2d r)
            => new Similarity2d(s.Scale, s.Euclidean * r);

        /// <summary>
        /// Multiplies a <see cref="Rot2d"/> and a <see cref="Similarity2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2d operator *(Rot2d r, Similarity2d s)
               => new Similarity2d(s.Scale, r * s.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="Similarity2d"/> and a <see cref="Shift2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2d operator *(Similarity2d a, Shift2d b)
            => new Similarity2d(a.Scale, a.Euclidean * (a.Scale * b));

        /// <summary>
        /// Multiplies a <see cref="Shift2d"/> and a <see cref="Similarity2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2d operator *(Shift2d a, Similarity2d b)
            => new Similarity2d(b.Scale, a * b.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="Similarity2d"/> and a <see cref="Scale2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine2d operator *(Similarity2d a, Scale2d b)
        {
            var t = (M22d)a;
            return new Affine2d(new M22d(
                t.M00 * b.X, t.M01 * b.Y, 
                t.M10 * b.X, t.M11 * b.Y),
                a.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Scale2d"/> and a <see cref="Similarity2d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine2d operator *(Scale2d a, Similarity2d b)
        {
            var t = (M22d)b;
            return new Affine2d(new M22d(
                t.M00 * a.X, t.M01 * a.X, 
                t.M10 * a.Y, t.M11 * a.Y),
                b.Trans * a.V);
        }

        /// <summary>
        /// Multiplies an Euclidean transformation by a Similarity transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2d operator *(Euclidean2d a, Similarity2d b)
        {
            return new Similarity2d(b.Scale, a * b.Euclidean);
            //return (Similarity2d)a * b;
        }

        /// <summary>
        /// Multiplies a Similarity transformation by an Euclidean transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2d operator *(Similarity2d a, Euclidean2d b)
        {
            return a * (Similarity2d)b;
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Similarity2d t0, Similarity2d t1)
        {
            return t0.Scale == t1.Scale && t0.Euclidean == t1.Euclidean;
        }

        public static bool operator !=(Similarity2d t0, Similarity2d t1)
        {
            return !(t0 == t1);
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="Similarity2d"/> transformation from a <see cref="M22d"/> matrix and a translation <see cref="V2d"/>.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2d FromM22dAndV2d(M22d m, V2d trans, double epsilon = 1e-12)
        {
            var s0 = m.C0.Norm2;
            var s1 = m.C1.Norm2;
            var s = (s0 * s1).Pow(1.0 / 2); //geometric mean of scale

            if (!((s0 / s - 1).IsTiny(epsilon) && (s1 / s - 1).IsTiny(epsilon)))
                throw new ArgumentException("Matrix features non-uniform scaling");

            m /= s;
            return new Similarity2d(s, Euclidean2d.FromM22dAndV2d(m, trans));
        }

        /// <summary>
        /// Creates a <see cref="Similarity2d"/> transformation from a <see cref="M23d"/> matrix.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2d FromM23d(M23d m, double epsilon = 1e-12)
            => FromM22dAndV2d((M22d)m, m.C2);

        /// <summary>
        /// Creates a <see cref="Similarity2d"/> transformation from a <see cref="M33d"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components or
        /// a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2d FromM33d(M33d m, double epsilon = 1e-12)
        {
            if (!(m.M20.IsTiny(epsilon) && m.M21.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");

            if (m.M22.IsTiny(epsilon))
                throw new ArgumentException("Matrix is not homogeneous.");

            return FromM22dAndV2d(((M22d)m) / m.M22, m.C2.XY);
        }

        /// <summary>
        /// Creates a <see cref="Similarity2d"/> transformation from an <see cref="Scale2d"/>.
        /// The transformation <paramref name="scale"/> must represent a uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2d FromScale2d(Scale2d scale, double epsilon = 1e-12)
        {
            var s = (scale.X * scale.Y).Pow(1.0 / 2);

            if (!scale.ApproximateEquals(new Scale2d(s), epsilon))
                throw new ArgumentException("Matrix features non-uniform scaling");

            return new Similarity2d(s, Euclidean2d.Identity);
        }

        /// <summary>
        /// Creates a <see cref="Similarity2d"/> transformation from an <see cref="Affine2d"/>.
        /// The transformation <paramref name="affine"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2d FromAffine2d(Affine2d affine, double epsilon = 1e-12)
            => FromM33d((M33d)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Similarity2d"/> transformation from a <see cref="Trafo2d"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity2d FromTrafo2d(Trafo2d trafo, double epsilon = 1e-12)
            => FromM33d(trafo.Forward, epsilon);

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M23d(Similarity2d s)
        {
            M23d rv = (M23d)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M22d(Similarity2d s)
        {
            M22d rv = (M22d)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33d(Similarity2d s)
        {
            M33d rv = (M33d)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine2d(Similarity2d s)
        {
            var m = (M23d)s;
            return new Affine2d((M22d)m, m.C2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo2d(Similarity2d s)
            => new Trafo2d((M33d)s, (M33d)s.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity2f(Similarity2d s)
            => new Similarity2f((float)s.Scale, (Euclidean2f)s.Euclidean);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Scale, Euclidean);
        }

        public override bool Equals(object other)
        {
            return (other is Similarity2d) ? (this == (Similarity2d)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Scale, Euclidean);
        }

        public static Similarity2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Similarity2d(double.Parse(x[0], CultureInfo.InvariantCulture), Euclidean2d.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Similarity
    {
        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="Similarity2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity2d Inverse(Similarity2d s)
            => s.Inverse;

        /// <summary>
        /// Inverts the similarity transformation (multiplicative inverse).
        /// this = [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Similarity2d t)
        {
            t.Scale = 1 / t.Scale;
            t.Euclidean.Invert();
            t.Euclidean.Trans *= t.Scale;
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="V3d"/> by a <see cref="Similarity2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d Transform(this Similarity2d s, V3d v)
            => s * v;

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d TransformDir(this Similarity2d t, V2d v)
        {
            return t.Euclidean.TransformDir(t.Scale * v);
        }

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d TransformPos(this Similarity2d t, V2d p)
        {
            return t.Euclidean.TransformPos(t.Scale * p);
        }

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d InvTransformDir(this Similarity2d t, V2d v)
        {
            return t.Euclidean.InvTransformDir(v) / t.Scale;
        }

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d InvTransformPos(this Similarity2d t, V2d p)
        {
            return t.Euclidean.InvTransformPos(p) / t.Scale;
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity2d t0, Similarity2d t1)
        {
            return ApproximateEquals(t0, t1, Constant<double>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity2d t0, Similarity2d t1, double tol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, tol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity2d t0, Similarity2d t1, double angleTol, double posTol, double scaleTol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, scaleTol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, angleTol, posTol);
        }

        #endregion
    }

    #endregion

    #region Similarity3d

    /// <summary>
    /// Represents a Similarity Transformation in 3D that is composed of a 
    /// Uniform Scale and a subsequent Euclidean transformation (3D rotation Rot and a subsequent translation by a 3D vector Trans).
    /// This is an angle preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Similarity3d
    {
        [DataMember]
        public double Scale;
        [DataMember]
        public Euclidean3d Euclidean;

        /// <summary>
        /// Gets the rotational component of this <see cref="Similarity3d"/> transformation.
        /// </summary>
        public Rot3d Rot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Rot; }
        }

        /// <summary>
        /// Gets the translational component of this <see cref="Similarity3d"/> transformation.
        /// </summary>
        public V3d Trans
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return Euclidean.Trans; }
        }

        #region Constructors

        /// <summary>
        /// Constructs a copy of an <see cref="Similarity3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Similarity3d(Similarity3d s)
        {
            Scale = s.Scale;
            Euclidean = s.Euclidean;
        }

        /// <summary>
        /// Creates a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rigid transformation <paramref name="euclideanTransformation"/>.
        /// </summary>
        public Similarity3d(double scale, Euclidean3d euclideanTransformation)
        {
            Scale = scale;
            Euclidean = euclideanTransformation;
        }

        /// <summary>
        /// Constructs a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rotation <paramref name="rotation"/> and translation <paramref name="translation"/>.
        /// </summary>
        public Similarity3d(double scale, Rot3d rotation, V3d translation)
        {
            Scale = scale;
            Euclidean = new Euclidean3d(rotation, translation);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Similarity3d Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Similarity3d(1, Euclidean3d.Identity);
        }

        #endregion

        #region Similarity Transformation Arithmetics

        /// <summary>
        /// Returns a new version of this Similarity transformation with a normalized rotation quaternion.
        /// </summary>
        public Similarity3d Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Similarity3d(Scale, Euclidean.Normalized);
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this Similarity transformation.
        /// [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        public Similarity3d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newS = 1 / Scale;
                var newR = Euclidean.Inverse;
                newR.Trans *= newS;
                return new Similarity3d(newS, newR);
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies two Similarity transformations.
        /// This concatenates the two similarity transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3d operator *(Similarity3d a, Similarity3d b)
        {
            //a.Scale * b.Scale, a.Rot * b.Rot, a.Trans + a.Rot * a.Scale * b.Trans
            return new Similarity3d(a.Scale * b.Scale, new Euclidean3d(
                a.Rot * b.Rot,
                a.Trans + a.Rot.Transform(a.Scale * b.Trans))
                );
        }

        /// <summary>
        /// Transforms a <see cref="V4d"/> vector by a <see cref="Similarity3d"/> transformation.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d operator *(Similarity3d s, V4d v)
            => s.Euclidean * new V4d(s.Scale * v.X, s.Scale * v.Y, s.Scale * v.Z, v.W);

        /// <summary>
        /// Multiplies a <see cref="Similarity3d"/> transformation (as a 4x4 matrix) with a <see cref="M44d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44d operator *(Similarity3d s, M44d m)
        {
            var t = (M34d)s;
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
        /// Multiplies a <see cref="M44d"/> with a <see cref="Similarity3d"/> transformation (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44d operator *(M44d m, Similarity3d s)
        {
            var t = (M34d)s;
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
        /// Multiplies a <see cref="Similarity3d"/> transformation (as a 3x4 matrix) with a <see cref="M34d"/> (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(Similarity3d s, M34d m)
        {
            var t = (M34d)s;
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
        /// Multiplies a <see cref="M34d"/> with a <see cref="Similarity3d"/> transformation (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(M34d m, Similarity3d s)
        {
            var t = (M34d)s;
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
        /// Multiplies a <see cref="Similarity3d"/> (as a 3x4 matrix) and a <see cref="M33d"/> (as a 4x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(Similarity3d s, M33d m)
            => new M34d(s.Rot * m * s.Scale, s.Trans);

        /// <summary>
        /// Multiplies a <see cref="M33d"/> and a <see cref="Similarity3d"/> (as a 3x4 matrix).
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M34d operator *(M33d m, Similarity3d s)
            => new M34d(s.Scale * m * s.Rot, m * s.Trans);

        /// <summary>
        /// Multiplies a <see cref="Similarity3d"/> and a <see cref="Rot3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3d operator *(Similarity3d s, Rot3d r)
            => new Similarity3d(s.Scale, s.Euclidean * r);

        /// <summary>
        /// Multiplies a <see cref="Rot3d"/> and a <see cref="Similarity3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3d operator *(Rot3d r, Similarity3d s)
               => new Similarity3d(s.Scale, r * s.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="Similarity3d"/> and a <see cref="Shift3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3d operator *(Similarity3d a, Shift3d b)
            => new Similarity3d(a.Scale, a.Euclidean * (a.Scale * b));

        /// <summary>
        /// Multiplies a <see cref="Shift3d"/> and a <see cref="Similarity3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3d operator *(Shift3d a, Similarity3d b)
            => new Similarity3d(b.Scale, a * b.Euclidean);

        /// <summary>
        /// Multiplies a <see cref="Similarity3d"/> and a <see cref="Scale3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3d operator *(Similarity3d a, Scale3d b)
        {
            var t = (M33d)a;
            return new Affine3d(new M33d(
                t.M00 * b.X, t.M01 * b.Y, t.M02 * b.Z, 
                t.M10 * b.X, t.M11 * b.Y, t.M12 * b.Z, 
                t.M20 * b.X, t.M21 * b.Y, t.M22 * b.Z),
                a.Trans);
        }

        /// <summary>
        /// Multiplies a <see cref="Scale3d"/> and a <see cref="Similarity3d"/>.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Affine3d operator *(Scale3d a, Similarity3d b)
        {
            var t = (M33d)b;
            return new Affine3d(new M33d(
                t.M00 * a.X, t.M01 * a.X, t.M02 * a.X, 
                t.M10 * a.Y, t.M11 * a.Y, t.M12 * a.Y, 
                t.M20 * a.Z, t.M21 * a.Z, t.M22 * a.Z),
                b.Trans * a.V);
        }

        /// <summary>
        /// Multiplies an Euclidean transformation by a Similarity transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3d operator *(Euclidean3d a, Similarity3d b)
        {
            return new Similarity3d(b.Scale, a * b.Euclidean);
            //return (Similarity3d)a * b;
        }

        /// <summary>
        /// Multiplies a Similarity transformation by an Euclidean transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3d operator *(Similarity3d a, Euclidean3d b)
        {
            return a * (Similarity3d)b;
        }

        #endregion

        #region Comparison Operators

        public static bool operator ==(Similarity3d t0, Similarity3d t1)
        {
            return t0.Scale == t1.Scale && t0.Euclidean == t1.Euclidean;
        }

        public static bool operator !=(Similarity3d t0, Similarity3d t1)
        {
            return !(t0 == t1);
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates a <see cref="Similarity3d"/> transformation from a <see cref="M33d"/> matrix and a translation <see cref="V3d"/>.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3d FromM33dAndV3d(M33d m, V3d trans, double epsilon = 1e-12)
        {
            var s0 = m.C0.Norm2;
            var s1 = m.C1.Norm2;
            var s2 = m.C2.Norm2;
            var s = (s0 * s1 * s2).Pow(1.0 / 3); //geometric mean of scale

            if (!((s0 / s - 1).IsTiny(epsilon) && (s1 / s - 1).IsTiny(epsilon) && (s2 / s - 1).IsTiny(epsilon)))
                throw new ArgumentException("Matrix features non-uniform scaling");

            m /= s;
            return new Similarity3d(s, Euclidean3d.FromM33dAndV3d(m, trans, epsilon));
        }

        /// <summary>
        /// Creates a <see cref="Similarity3d"/> transformation from a <see cref="M34d"/> matrix.
        /// The matrix must not contain a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3d FromM34d(M34d m, double epsilon = 1e-12)
            => FromM33dAndV3d((M33d)m, m.C3, epsilon);

        /// <summary>
        /// Creates a <see cref="Similarity3d"/> transformation from a <see cref="M44d"/> matrix.
        /// The matrix has to be homogeneous and must not contain perspective components or
        /// a non-uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3d FromM44d(M44d m, double epsilon = 1e-12)
        {
            if (!(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");

            if (m.M33.IsTiny(epsilon))
                throw new ArgumentException("Matrix is not homogeneous.");

            return FromM33dAndV3d(((M33d)m) / m.M33, m.C3.XYZ, epsilon);
        }

        /// <summary>
        /// Creates a <see cref="Similarity3d"/> transformation from an <see cref="Scale3d"/>.
        /// The transformation <paramref name="scale"/> must represent a uniform scaling.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3d FromScale3d(Scale3d scale, double epsilon = 1e-12)
        {
            var s = (scale.X * scale.Y * scale.Z).Pow(1.0 / 3);

            if (!scale.ApproximateEquals(new Scale3d(s), epsilon))
                throw new ArgumentException("Matrix features non-uniform scaling");

            return new Similarity3d(s, Euclidean3d.Identity);
        }

        /// <summary>
        /// Creates a <see cref="Similarity3d"/> transformation from an <see cref="Affine3d"/>.
        /// The transformation <paramref name="affine"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3d FromAffine3d(Affine3d affine, double epsilon = 1e-12)
            => FromM44d((M44d)affine, epsilon);

        /// <summary>
        /// Creates a <see cref="Similarity3d"/> transformation from a <see cref="Trafo3d"/>.
        /// The transformation <paramref name="trafo"/> must only consist of a uniform scale, rotation, and translation.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        public static Similarity3d FromTrafo3d(Trafo3d trafo, double epsilon = 1e-12)
            => FromM44d(trafo.Forward, epsilon);

        #endregion

        #region Conversion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M34d(Similarity3d s)
        {
            M34d rv = (M34d)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; rv.M02 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; rv.M12 *= s.Scale; 
            rv.M20 *= s.Scale; rv.M21 *= s.Scale; rv.M22 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M33d(Similarity3d s)
        {
            M33d rv = (M33d)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; rv.M02 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; rv.M12 *= s.Scale; 
            rv.M20 *= s.Scale; rv.M21 *= s.Scale; rv.M22 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator M44d(Similarity3d s)
        {
            M44d rv = (M44d)s.Euclidean;
            rv.M00 *= s.Scale; rv.M01 *= s.Scale; rv.M02 *= s.Scale; 
            rv.M10 *= s.Scale; rv.M11 *= s.Scale; rv.M12 *= s.Scale; 
            rv.M20 *= s.Scale; rv.M21 *= s.Scale; rv.M22 *= s.Scale; 
            return rv;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Affine3d(Similarity3d s)
        {
            var m = (M34d)s;
            return new Affine3d((M33d)m, m.C3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo3d(Similarity3d s)
            => new Trafo3d((M44d)s, (M44d)s.Inverse);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Similarity3f(Similarity3d s)
            => new Similarity3f((float)s.Scale, (Euclidean3f)s.Euclidean);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Scale, Euclidean);
        }

        public override bool Equals(object other)
        {
            return (other is Similarity3d) ? (this == (Similarity3d)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Scale, Euclidean);
        }

        public static Similarity3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Similarity3d(double.Parse(x[0], CultureInfo.InvariantCulture), Euclidean3d.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Similarity
    {
        #region Invert

        /// <summary>
        /// Returns the inverse of a <see cref="Similarity3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3d Inverse(Similarity3d s)
            => s.Inverse;

        /// <summary>
        /// Inverts the similarity transformation (multiplicative inverse).
        /// this = [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Similarity3d t)
        {
            t.Scale = 1 / t.Scale;
            t.Euclidean.Invert();
            t.Euclidean.Trans *= t.Scale;
        }

        #endregion

        #region Normalize

        /// <summary>
        /// Returns a copy of a <see cref="Similarity3d"/> with its rotation quaternion normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity3d Normalized(Similarity3d s)
            => s.Normalized;

        /// <summary>
        /// Normalizes the rotation quaternion of a <see cref="Similarity3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Similarity3d t)
        {
            t.Euclidean.Normalize();
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms a <see cref="V4d"/> by a <see cref="Similarity3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d Transform(this Similarity3d s, V4d v)
            => s * v;

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d TransformDir(this Similarity3d t, V3d v)
        {
            return t.Euclidean.TransformDir(t.Scale * v);
        }

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d TransformPos(this Similarity3d t, V3d p)
        {
            return t.Euclidean.TransformPos(t.Scale * p);
        }

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransformDir(this Similarity3d t, V3d v)
        {
            return t.Euclidean.InvTransformDir(v) / t.Scale;
        }

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of the similarity transformation <paramref name="t"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransformPos(this Similarity3d t, V3d p)
        {
            return t.Euclidean.InvTransformPos(p) / t.Scale;
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity3d t0, Similarity3d t1)
        {
            return ApproximateEquals(t0, t1, Constant<double>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity3d t0, Similarity3d t1, double tol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, tol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, tol);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity3d t0, Similarity3d t1, double angleTol, double posTol, double scaleTol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, scaleTol) && t0.Euclidean.ApproximateEquals(t1.Euclidean, angleTol, posTol);
        }

        #endregion
    }

    #endregion

}
