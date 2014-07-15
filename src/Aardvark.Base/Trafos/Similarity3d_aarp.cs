using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    // [todo ISSUE 20090427 andi : andi] define Similarity2*
    /// <summary>
    /// Represents a Similarity Transformation in 3D that is composed of a 
    /// Uniform Scale and a subsequent Euclidean transformation (3D rotation Rot and a subsequent translation by a 3D vector Trans).
    /// This is an angle preserving Transformation.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Similarity3d
    {
        public double Scale;
        public Euclidean3d EuclideanTransformation;

        /// <summary>
        /// Shortcut for Rot of EuclideanTransformation
        /// </summary>
        public Rot3d Rot { get { return EuclideanTransformation.Rot; } }

        /// <summary>
        /// Shortcut for Trans of EuclideanTransformation
        /// </summary>
        public V3d Trans { get { return EuclideanTransformation.Trans; } }

        #region Constructors

        /// <summary>
        /// Creates a similarity transformation from an uniform scale by factor <paramref name="scale"/>, and a (subsequent) rigid transformation <paramref name="euclideanTransformation"/>.
        /// </summary>
        public Similarity3d(double scale, Euclidean3d euclideanTransformation)
        {
            Scale = scale;
            EuclideanTransformation = euclideanTransformation;
        }

        /// <summary>
        /// Creates a similarity transformation from a rigid transformation <paramref name="euclideanTransformation"/> and an (subsequent) uniform scale by factor <paramref name="scale"/>.
        /// </summary>
        public Similarity3d(Euclidean3d euclideanTransformation, double scale)
        {
            Scale = scale;
            EuclideanTransformation = new Euclidean3d(
                euclideanTransformation.Rot,
                scale * euclideanTransformation.Trans
                );
        }

        public Similarity3d(M44d m, double epsilon = (double)0.00001)
        {
            Trace.Assert(m.M30.IsTiny(epsilon) && m.M31.IsTiny(epsilon) && m.M32.IsTiny(epsilon), "Matrix contains perspective components.");
            Trace.Assert(!m.M33.IsTiny(epsilon), "Matrix is not homogeneous.");
            m /= m.M33; //normalize it
            var m33 = (M33d)m;
            var s0 = m33.C0.Norm2;
            var s1 = m33.C1.Norm2;
            var s2 = m33.C2.Norm2;
            var s = (s0 * s1 * s2).Pow((double)1.0/3); //geometric mean of scale
            Trace.Assert((s0 / s - 1).IsTiny(epsilon) && (s1 / s - 1).IsTiny(epsilon) && (s2 / s - 1).IsTiny(epsilon), "Matrix features non-uniform scaling");
            m33 /= s;
            Scale = s;
            EuclideanTransformation = new Euclidean3d(m33, m.C3.XYZ);
        }

        #endregion

        #region Constants

        /// <summary>
        /// Identity (1, EuclideanTransformation.Identity)
        /// </summary>
        public static readonly Similarity3d Identity = new Similarity3d(1, Euclidean3d.Identity);

        #endregion

        #region Similarity Transformation Arithmetics

        /// <summary>
        /// Multiplies 2 Similarity transformations.
        /// This concatenates the two similarity transformations into a double one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        public static Similarity3d Multiply(Similarity3d a, Similarity3d b)
        {
            //a.Scale * b.Scale, a.Rot * b.Rot, a.Trans + a.Rot * a.Scale * b.Trans
            return new Similarity3d(a.Scale * b.Scale, new Euclidean3d(
                Rot3d.Multiply(a.Rot, b.Rot),
                a.Trans + a.Rot.TransformDir(a.Scale * b.Trans))
                );
        }

        /// <summary>
        /// Multiplies a Similarity transformation by an Euclidean transformation.
        /// This concatenates the two transformations into a double one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        public static Similarity3d Multiply(Similarity3d a, Euclidean3d b)
        {
            return Multiply(a, (Similarity3d)b);
        }

        /// <summary>
        /// Multiplies an Euclidean transformation by a Similarity transformation.
        /// This concatenates the two transformations into a double one, first b is applied, then a.
        /// Attention: Multiplication is NOT commutative!
        /// </summary>
        public static Similarity3d Multiply(Euclidean3d a, Similarity3d b)
        {
            return Multiply((Similarity3d)a, b);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by similarity transformation t.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V3d TransformDir(Similarity3d t, V3d v)
        {
            return t.EuclideanTransformation.TransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by similarity transformation t.
        /// </summary>
        public static V3d TransformPos(Similarity3d t, V3d p)
        {
            return t.EuclideanTransformation.TransformPos(t.Scale * p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of the similarity transformation t.
        /// Actually, only the rotation is used.
        /// </summary>
        public static V3d InvTransformDir(Similarity3d t, V3d v)
        {
            return t.EuclideanTransformation.InvTransformDir(v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of the similarity transformation t.
        /// </summary>
        public static V3d InvTransformPos(Similarity3d t, V3d p)
        {
            return t.EuclideanTransformation.InvTransformPos(p / t.Scale);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by this similarity transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V3d TransformDir(V3d v)
        {
            return TransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by this similarity transformation.
        /// </summary>
        public V3d TransformPos(V3d p)
        {
            return TransformPos(this, p);
        }

        /// <summary>
        /// Transforms direction vector v (v.w is presumed 0.0) by the inverse of this similarity transformation.
        /// Actually, only the rotation is used.
        /// </summary>
        public V3d InvTransformDir(V3d v)
        {
            return InvTransformDir(this, v);
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) by the inverse of this similarity transformation.
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
            EuclideanTransformation.Normalize();
        }

        /// <summary>
        /// Returns a new version of this Similarity transformation with a normalized rotation quaternion.
        /// </summary>
        public Similarity3d Normalized
        {
            get
            {
                return new Similarity3d(Scale, EuclideanTransformation.Normalized);
            }
        }

        /// <summary>
        /// Inverts this similarity transformation (multiplicative inverse).
        /// this = [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        /// <remarks>Not tested.</remarks>
        // [todo ISSUE 20090807 andi : andi] Test
        public void Invert()
        {
            Scale = 1 / Scale;
            EuclideanTransformation.Invert();
            EuclideanTransformation.Trans *= Scale;
        }

        /// <summary>
        /// Gets the (multiplicative) inverse of this Similarity transformation.
        /// [1/Scale, Rot^T,-Rot^T Trans/Scale]
        /// </summary>
        /// <remarks>Not tested.</remarks>
        // [todo ISSUE 20090807 andi : andi] Test
        public Similarity3d Inverse
        {
            get
            {
                var newS = 1 / Scale;
                var newR = EuclideanTransformation.Inverse;
                newR.Trans *= newS;
                return new Similarity3d(newS, newR);
            }
        }

        public static bool ApproxEqual(Similarity3d t0, Similarity3d t1)
        {
            return ApproxEqual(t0, t1, Constant<double>.PositiveTinyValue, Constant<double>.PositiveTinyValue, Constant<double>.PositiveTinyValue);
        }

        public static bool ApproxEqual(Similarity3d t0, Similarity3d t1, double angleTol, double posTol, double scaleTol)
        {
            return t0.Scale.ApproximateEquals(t1.Scale, scaleTol) && Euclidean3d.ApproxEqual(t0.EuclideanTransformation, t1.EuclideanTransformation, angleTol, posTol);
        }

        #endregion

        #region Arithmetic Operators

        public static Similarity3d operator *(Similarity3d a, Similarity3d b)
        {
            return Similarity3d.Multiply(a, b);
        }

        public static Similarity3d operator *(Euclidean3d a, Similarity3d b)
        {
            return Similarity3d.Multiply(a, b);
        }

        public static Similarity3d operator *(Similarity3d a, Euclidean3d b)
        {
            return Similarity3d.Multiply(a, b);
        }

        // [todo ISSUE 20090427 andi : andi] add operator * for all other Trafo structs.
/*
        public static M33d operator *(Similarity3d t, Rot3d m)
        {
            return (M34d)t * m;
        }

        public static M33d operator *(Similarity3d t, Scale3d m)
        {
            return (M34d)t * m;
        }
*/
/*
        // [todo ISSUE 20090427 andi : andi] this is again a Similarity3d.
        // [todo ISSUE 20090427 andi : andi] Rot3d * Shift3d should return a Similarity3d!
        public static M34d operator *(Similarity3d t, Shift3d m)
        {
            return (M34d)t * m;
        }

        public static M33d operator *(Similarity3d t, M33d m)
        {
            return (M34d)t * m;
        }

        public static M33d operator *(M33d m, Similarity3d t)
        {
            return m * (M34d)t;
        }
*/
        #endregion

        #region Comparison Operators

        public static bool operator ==(Similarity3d t0, Similarity3d t1)
        {
            return t0.Scale == t1.Scale && t0.EuclideanTransformation == t1.EuclideanTransformation;
        }

        public static bool operator !=(Similarity3d t0, Similarity3d t1)
        {
            return !(t0 == t1);
        }

        #endregion

        #region Conversion

        public static explicit operator M34d(Similarity3d t)
        {
            M34d rv = (M34d)t.EuclideanTransformation;
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

        public static explicit operator M44d(Similarity3d t)
        {
            M44d rv = (M44d)t.EuclideanTransformation;
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
            return (other is Similarity3d) ? (this == (Similarity3d)other) : false;
        }

        public override string ToString()
        {
            return string.Format(Localization.FormatEnUS, "[{0}, {1}]", Scale, EuclideanTransformation);
        }

        public static Similarity3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Similarity3d(double.Parse(x[0]), Euclidean3d.Parse(x[1]));
        }

        #endregion
    }
}
