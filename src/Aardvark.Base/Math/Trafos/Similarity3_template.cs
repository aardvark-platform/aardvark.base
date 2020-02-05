using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    // [todo ISSUE 20090427 andi : andi] define Similarity2*
    #region Similarity__x3t__

    /// <summary>
    /// Represents a Similarity Transformation in 3D that is composed of a 
    /// Uniform Scale and a subsequent Euclidean transformation (3D rotation Rot and a subsequent translation by a 3D vector Trans).
    /// This is an angle preserving Transformation.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Similarity__x3t__
    {
        [DataMember]
        public __ft__ Scale;
        [DataMember]
        public Euclidean__x3t__ EuclideanTransformation;

        /// <summary>
        /// Shortcut for Rot of EuclideanTransformation
        /// </summary>
        public Rot__x3t__ Rot
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return EuclideanTransformation.Rot; }
        }

        /// <summary>
        /// Shortcut for Trans of EuclideanTransformation
        /// </summary>
        public V__x3t__ Trans
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get { return EuclideanTransformation.Trans; }
        }

        #region Constructors

        /// <summary>
        /// Creates a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rigid transformation <paramref name="euclideanTransformation"/>.
        /// </summary>
        public Similarity__x3t__(__ft__ scale, Euclidean__x3t__ euclideanTransformation)
        {
            Scale = scale;
            EuclideanTransformation = euclideanTransformation;
        }

        /// <summary>
        /// Creates a similarity transformation from a rigid transformation <paramref name="euclideanTransformation"/> and an (subsequent) uniform scale by factor <paramref name="scale"/>.
        /// </summary>
        public Similarity__x3t__(Euclidean__x3t__ euclideanTransformation, __ft__ scale)
        {
            Scale = scale;
            EuclideanTransformation = new Euclidean__x3t__(
                euclideanTransformation.Rot,
                scale * euclideanTransformation.Trans
                );
        }

        public Similarity__x3t__(M4__x4t__ m, __ft__ epsilon = (__ft__)0.00001)
        {
            if (!(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon)))
                throw new ArgumentException("Matrix contains perspective components.");
            if (m.M33.IsTiny(epsilon)) throw new ArgumentException("Matrix is not homogeneous.");
            m /= m.M33; //normalize it
            var m33 = (M3__x3t__)m;
            var s0 = m33.C0.Norm2;
            var s1 = m33.C1.Norm2;
            var s2 = m33.C2.Norm2;
            var s = (s0 * s1 * s2).Pow((__ft__)1.0 / 3); //geometric mean of scale
            if (!((s0 / s - 1).IsTiny(epsilon) && (s1 / s - 1).IsTiny(epsilon) && (s2 / s - 1).IsTiny(epsilon)))
                throw new ArgumentException("Matrix features non-uniform scaling");
            m33 /= s;
            Scale = s;
            EuclideanTransformation = new Euclidean__x3t__(m33, m.C3.XYZ);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Identity (1, EuclideanTransformation.Identity)
        /// </summary>
        public static Similarity__x3t__ Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Similarity__x3t__(1, Euclidean__x3t__.Identity);
        }

        #endregion

        #region Similarity Transformation Arithmetics

        /// <summary>
        /// Returns a new version of this Similarity transformation with a normalized rotation quaternion.
        /// </summary>
        public Similarity__x3t__ Normalized
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                return new Similarity__x3t__(Scale, EuclideanTransformation.Normalized);
            }
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this Similarity transformation.
        /// [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        public Similarity__x3t__ Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var newS = 1 / Scale;
                var newR = EuclideanTransformation.Inverse;
                newR.Trans *= newS;
                return new Similarity__x3t__(newS, newR);
            }
        }

        #endregion

        #region Arithmetic Operators

        /// <summary>
        /// Multiplies 2 Similarity transformations.
        /// This concatenates the two similarity transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity__x3t__ operator *(Similarity__x3t__ a, Similarity__x3t__ b)
        {
            //a.Scale * b.Scale, a.Rot * b.Rot, a.Trans + a.Rot * a.Scale * b.Trans
            return new Similarity__x3t__(a.Scale * b.Scale, new Euclidean__x3t__(
                a.Rot * b.Rot,
                a.Trans + a.Rot.Transform(a.Scale * b.Trans))
                );
        }

        /// <summary>
        /// Multiplies an Euclidean transformation by a Similarity transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity__x3t__ operator *(Euclidean__x3t__ a, Similarity__x3t__ b)
        {
            return (Similarity__x3t__)a * b;
        }

        /// <summary>
        /// Multiplies a Similarity transformation by an Euclidean transformation.
        /// This concatenates the two transformations into a single one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Similarity__x3t__ operator *(Similarity__x3t__ a, Euclidean__x3t__ b)
        {
            return a * (Similarity__x3t__)b;
        }

        // [todo ISSUE 20090427 andi : andi] add operator * for all other Trafo structs.
        /*
                public static M3__x3t__ operator *(Similarity__x3t__ t, Rot__x3t__ m)
                {
                    return (M3__x4t__)t * m;
                }

                public static M3__x3t__ operator *(Similarity__x3t__ t, Scale__x3t__ m)
                {
                    return (M3__x4t__)t * m;
                }
        */
        /*
                // [todo ISSUE 20090427 andi : andi] this is again a Similarity__x3t__.
                // [todo ISSUE 20090427 andi : andi] Rot__x3t__ * Shift__x3t__ should return a Similarity__x3t__!
                public static M3__x4t__ operator *(Similarity__x3t__ t, Shift__x3t__ m)
                {
                    return (M3__x4t__)t * m;
                }

                public static M3__x3t__ operator *(Similarity__x3t__ t, M3__x3t__ m)
                {
                    return (M3__x4t__)t * m;
                }

                public static M3__x3t__ operator *(M3__x3t__ m, Similarity__x3t__ t)
                {
                    return m * (M3__x4t__)t;
                }
        */
        #endregion

        #region Comparison Operators

        public static bool operator ==(Similarity__x3t__ t0, Similarity__x3t__ t1)
        {
            return t0.Scale == t1.Scale && t0.EuclideanTransformation == t1.EuclideanTransformation;
        }

        public static bool operator !=(Similarity__x3t__ t0, Similarity__x3t__ t1)
        {
            return !(t0 == t1);
        }

        #endregion

        #region Conversion

        public static explicit operator M3__x4t__(Similarity__x3t__ t)
        {
            M3__x4t__ rv = (M3__x4t__)t.EuclideanTransformation;
            rv.M00 *= t.Scale;
            rv.M01 *= t.Scale;
            rv.M02 *= t.Scale;
            rv.M10 *= t.Scale;
            rv.M11 *= t.Scale;
            rv.M12 *= t.Scale;
            rv.M20 *= t.Scale;
            rv.M21 *= t.Scale;
            rv.M22 *= t.Scale;
            return rv;
        }

        public static explicit operator M4__x4t__(Similarity__x3t__ t)
        {
            M4__x4t__ rv = (M4__x4t__)t.EuclideanTransformation;
            rv.M00 *= t.Scale;
            rv.M01 *= t.Scale;
            rv.M02 *= t.Scale;
            rv.M10 *= t.Scale;
            rv.M11 *= t.Scale;
            rv.M12 *= t.Scale;
            rv.M20 *= t.Scale;
            rv.M21 *= t.Scale;
            rv.M22 *= t.Scale;
            return rv;
        }

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Scale, EuclideanTransformation);
        }

        public override bool Equals(object other)
        {
            return (other is Similarity__x3t__) ? (this == (Similarity__x3t__)other) : false;
        }

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Scale, EuclideanTransformation);
        }

        public static Similarity__x3t__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Similarity__x3t__(__ft__.Parse(x[0]), Euclidean__x3t__.Parse(x[1]));
        }

        #endregion
    }

    public static partial class Similarity
    {
        #region Invert, Normalize

        /// <summary>
        /// Inverts the similarity transformation (multiplicative inverse).
        /// this = [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Invert(this ref Similarity__x3t__ t)
        {
            t.Scale = 1 / t.Scale;
            t.EuclideanTransformation.Invert();
            t.EuclideanTransformation.Trans *= t.Scale;
        }

        /// <summary>
        /// Normalizes the rotation quaternion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Normalize(this ref Similarity__x3t__ t)
        {
            t.EuclideanTransformation.Normalize();
        }

        #endregion

        #region Transform

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by similarity transformation t.
        /// Actually, only the rotation and scale is used.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V__x3t__ TransformDir(this Similarity__x3t__ t, V__x3t__ v)
        {
            return t.EuclideanTransformation.TransformDir(t.Scale * v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by similarity transformation t.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V__x3t__ TransformPos(this Similarity__x3t__ t, V__x3t__ p)
        {
            return t.EuclideanTransformation.TransformPos(t.Scale * p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of the similarity transformation t.
        /// Actually, only the rotation and scale is used.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V__x3t__ InvTransformDir(this Similarity__x3t__ t, V__x3t__ v)
        {
            return t.EuclideanTransformation.InvTransformDir(v) / t.Scale;
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of the similarity transformation t.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V__x3t__ InvTransformPos(this Similarity__x3t__ t, V__x3t__ p)
        {
            return t.EuclideanTransformation.InvTransformPos(p) / t.Scale;
        }

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity__x3t__ t0, Similarity__x3t__ t1)
        {
            return ApproximateEquals(t0, t1, Constant<__ft__>.PositiveTinyValue, Constant<__ft__>.PositiveTinyValue, Constant<__ft__>.PositiveTinyValue);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Similarity__x3t__ t0, Similarity__x3t__ t1, __ft__ angleTol, __ft__ posTol, __ft__ scaleTol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, scaleTol) && t0.EuclideanTransformation.ApproximateEquals(t1.EuclideanTransformation, angleTol, posTol);
        }

        #endregion
    }

    #endregion

    //# }
}
