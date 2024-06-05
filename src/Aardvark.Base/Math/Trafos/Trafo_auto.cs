using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Trafo2f

    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct Trafo2f : IEquatable<Trafo2f>
    {
        [DataMember]
        public readonly M33f Forward;
        [DataMember]
        public readonly M33f Backward;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="Trafo2f"/> from a forward and backward transformation <see cref="M33f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2f(M33f forward, M33f backward)
        {
            Forward = forward;
            Backward = backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2f"/> from another <see cref="Trafo2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2f(Trafo2f trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2f"/> from a <see cref="Trafo2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2f(Trafo2d trafo)
        {
            Forward = (M33f)trafo.Forward;
            Backward = (M33f)trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2f"/> from an <see cref="Affine2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2f(Affine2f trafo)
        {
            Forward = (M33f)trafo;
            Backward = (M33f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2f"/> from a <see cref="Euclidean2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2f(Euclidean2f trafo)
        {
            Forward = (M33f)trafo;
            Backward = (M33f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2f"/> from a <see cref="Similarity2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2f(Similarity2f trafo)
        {
            Forward = (M33f)trafo;
            Backward = (M33f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2f"/> from a <see cref="Rot2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2f(Rot2f trafo)
        {
            Forward = (M33f)trafo;
            Backward = (M33f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2f"/> from a <see cref="Scale2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2f(Scale2f trafo)
        {
            Forward = (M33f)trafo;
            Backward = (M33f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2f"/> from a <see cref="Shift2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2f(Shift2f trafo)
        {
            Forward = (M33f)trafo;
            Backward = (M33f)trafo.Inverse;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Trafo2f Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Trafo2f(M33f.Identity, M33f.Identity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the inverse of this <see cref="Trafo2f"/>.
        /// </summary>
        public Trafo2f Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Trafo2f(Backward, Forward);
        }

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Forward, Backward);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Trafo2f other)
            => Forward.Equals(other.Forward) && Backward.Equals(other.Backward);

        public override bool Equals(object other)
            => (other is Trafo2f o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        public static Trafo2f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Trafo2f(
                M33f.Parse(x[0].ToString()),
                M33f.Parse(x[1].ToString())
            );
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Builds a transformation matrix using the scale, rotation (in radians) and translation components.
        /// NOTE: Uses the Scale * Rotation * Translation notion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f FromComponents(V2f scale, float rotationInRadians, V2f translation)
            => Scale(scale) * Rotation(rotationInRadians) * Translation(translation);

        /// <summary>
        /// Returns the <see cref="Trafo2f"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f FromBasis(V2f xAxis, V2f yAxis, V2f origin)
        {
            var mat = new M33f(
                            xAxis.X, yAxis.X, origin.X, 
                            xAxis.Y, yAxis.Y, origin.Y,
                            0, 0, 1);

            return new Trafo2f(mat, mat.Inverse);
        }

        /// <summary>
        /// Returns the <see cref="Trafo2f"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// Note that the axes MUST be normalized and normal to each other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f FromOrthoNormalBasis(V2f xAxis, V2f yAxis)
        {
            return new Trafo2f(
                        new M33f(
                            xAxis.X, yAxis.X, 0, 
                            xAxis.Y, yAxis.Y, 0,
                            0, 0, 1),
                        new M33f(
                            xAxis.X, xAxis.Y, 0, 
                            yAxis.X, yAxis.Y, 0,
                            0, 0, 1)
                        );
        }

        #region Translation

        /// <summary>
        /// Creates an <see cref="Trafo2f"/> transformation with the translational component given by a <see cref="V2f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Translation(V2f v)
            => new Trafo2f(M33f.Translation(v), M33f.Translation(-v));

        /// <summary>
        /// Creates a <see cref="Trafo2f"/> transformation with the translational component given by 2 scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Translation(float tX, float tY)
            => Translation(new V2f(tX, tY));

        /// <summary>
        /// Creates an <see cref="Trafo2f"/> transformation with the translational component given by a <see cref="Shift2f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Translation(Shift2f shift)
            => Translation(shift.V);

        #endregion

        #region Scale

        /// <summary>
        /// Creates a scaling transformation using a <see cref="V2f"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Scale(V2f v) 
            => new Trafo2f(M33f.Scale(v), M33f.Scale(1 / v));

        /// <summary>
        /// Creates a scaling transformation using 2 scalars as scaling factors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Scale(float sX, float sY)
            => new Trafo2f(M33f.Scale(sX, sY),
                           M33f.Scale(1 / sX, 1 / sY));

        /// <summary>
        /// Creates a scaling transformation using a uniform scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Scale(float s)
        {
            var t = 1 / s;
            return new Trafo2f(M33f.Scale(s), M33f.Scale(t));
        }

        /// <summary>
        /// Creates a scaling transformation using a <see cref="Scale2f"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Scale(Scale2f scale)
            => new Trafo2f(scale);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="Rot2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Rotation(Rot2f rotation)
            => new Trafo2f(rotation);

        /// <summary>
        /// Creates a rotation transformation with the specified angle in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Rotation(float angleInRadians)
            => new Trafo2f(M33f.Rotation(angleInRadians), M33f.Rotation(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation with the specified angle in degrees.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f RotationInDegrees(float angleInDegrees)
            => Rotation(angleInDegrees.RadiansFromDegrees());

        #endregion

        #endregion

        #region Conversion Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo2d(Trafo2f r)
            => new Trafo2d(r);

        #endregion

        #region Operators

        /// <summary>
        /// Returns whether two <see cref="Trafo2f"/> transformations are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Trafo2f a, Trafo2f b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        /// <summary>
        /// Returns whether two <see cref="Trafo2f"/> transformations are different.
        /// </summary>
        public static bool operator !=(Trafo2f a, Trafo2f b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of Trafo2f multiplicaition is backward
        /// with respect to M33f multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static Trafo2f operator *(Trafo2f t0, Trafo2f t1)
            => new Trafo2f(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion 
    }

    public static partial class Trafo
    {
        #region Operations

        /// <summary>
        /// Returns the inverse of the given <see cref="Trafo2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2f Inverse(Trafo2f trafo)
            => trafo.Inverse;

        /// <summary>
        /// Returns the forward matrix the given <see cref="Trafo2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f Forward(Trafo2f trafo)
            => trafo.Forward;

        /// <summary>
        /// Returns the backward matrix the given <see cref="Trafo2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33f Backward(Trafo2f trafo)
            => trafo.Backward;

        #endregion

        #region Transformation Extraction

        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetScale(this Trafo2f trafo)
            => trafo.Forward.GetScale2();

        /// <summary>
        /// Extracts a scale vector from the given transformation by calculating the lengths of the basis vectors. 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f GetScaleVector(this Trafo2f trafo)
            => trafo.Forward.GetScaleVector2();


        #endregion

        #region Transformations

        /// <summary>
        /// Transforms a <see cref="V3f"/> by a <see cref="Trafo2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f Transform(this Trafo2f r, V3f v)
            => r.Forward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by a <see cref="Trafo2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f TransformDir(this Trafo2f r, V2f v)
            => r.Forward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by a <see cref="Trafo2f"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f TransformPos(this Trafo2f r, V2f p)
            => r.Forward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by a <see cref="Trafo2f"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f TransformPosProj(this Trafo2f r, V2f p)
            => r.Forward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by a <see cref="Trafo2f"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f TransformPosProjFull(this Trafo2f r, V2f p)
            => r.Forward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.Z is presumed 0.0) by a <see cref="Trafo2f"/>
        /// (i.e. by its transposed backward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f TransformNormal(this Trafo2f r, V2f n)
            => r.Backward.TransposedTransformDir(n);

        /// <summary>
        /// Transforms a <see cref="V3f"/> by the inverse of a <see cref="Trafo2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransform(this Trafo2f r, V3f v)
            => r.Backward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by the inverse of a <see cref="Trafo2f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f InvTransformDir(this Trafo2f r, V2f v)
            => r.Backward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of a <see cref="Trafo2f"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f InvTransformPos(this Trafo2f r, V2f p)
            => r.Backward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of a <see cref="Trafo2f"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f InvTransformPosProj(this Trafo2f r, V2f p)
            => r.Backward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of a <see cref="Trafo2f"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransformPosProjFull(this Trafo2f r, V2f p)
            => r.Backward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.Z is presumed 0.0) by the inverse of a <see cref="Trafo2f"/>
        /// (i.e. by its transposed forward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2f InvTransformNormal(this Trafo2f r, V2f n)
            => r.Forward.TransposedTransformDir(n);

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        /// <summary>
        /// Returns if two <see cref="Trafo2f"/> transformations are equal with regard to a threshold <paramref name="epsilon"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Trafo2f a, Trafo2f b, float epsilon)
            => a.Forward.ApproximateEquals(b.Forward, epsilon) && a.Backward.ApproximateEquals(b.Backward, epsilon);

        #endregion
    }

    #endregion

    #region Trafo3f

    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct Trafo3f : IEquatable<Trafo3f>
    {
        [DataMember]
        public readonly M44f Forward;
        [DataMember]
        public readonly M44f Backward;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="Trafo3f"/> from a forward and backward transformation <see cref="M44f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3f(M44f forward, M44f backward)
        {
            Forward = forward;
            Backward = backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3f"/> from another <see cref="Trafo3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3f(Trafo3f trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3f"/> from a <see cref="Trafo3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3f(Trafo3d trafo)
        {
            Forward = (M44f)trafo.Forward;
            Backward = (M44f)trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3f"/> from an <see cref="Affine3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3f(Affine3f trafo)
        {
            Forward = (M44f)trafo;
            Backward = (M44f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3f"/> from a <see cref="Euclidean3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3f(Euclidean3f trafo)
        {
            Forward = (M44f)trafo;
            Backward = (M44f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3f"/> from a <see cref="Similarity3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3f(Similarity3f trafo)
        {
            Forward = (M44f)trafo;
            Backward = (M44f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3f"/> from a <see cref="Rot3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3f(Rot3f trafo)
        {
            Forward = (M44f)trafo;
            Backward = (M44f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3f"/> from a <see cref="Scale3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3f(Scale3f trafo)
        {
            Forward = (M44f)trafo;
            Backward = (M44f)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3f"/> from a <see cref="Shift3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3f(Shift3f trafo)
        {
            Forward = (M44f)trafo;
            Backward = (M44f)trafo.Inverse;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Trafo3f Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Trafo3f(M44f.Identity, M44f.Identity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the inverse of this <see cref="Trafo3f"/>.
        /// </summary>
        public Trafo3f Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Trafo3f(Backward, Forward);
        }

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Forward, Backward);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Trafo3f other)
            => Forward.Equals(other.Forward) && Backward.Equals(other.Backward);

        public override bool Equals(object other)
            => (other is Trafo3f o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        public static Trafo3f Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Trafo3f(
                M44f.Parse(x[0].ToString()),
                M44f.Parse(x[1].ToString())
            );
        }

        #endregion

        #region Static Creators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f FromNormalFrame(V3f origin, V3f normal)
        {
            M44f.NormalFrame(origin, normal, out M44f forward, out M44f backward);
            return new Trafo3f(forward, backward);
        }

        /// <summary>
        /// Builds a transformation matrix using the scale, rotation and translation components.
        /// NOTE: Uses the Scale * Rotation * Translation notion. 
        ///       The rotation is in Euler-Angles (roll, pitch, yaw) in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f FromComponents(V3f scale, V3f rotationInRadians, V3f translation)
            => Scale(scale) * RotationEuler(rotationInRadians) * Translation(translation);

        /// <summary>
        /// Returns the <see cref="Trafo3f"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f FromBasis(V3f xAxis, V3f yAxis, V3f zAxis, V3f origin)
        {
            var mat = new M44f(
                            xAxis.X, yAxis.X, zAxis.X, origin.X, 
                            xAxis.Y, yAxis.Y, zAxis.Y, origin.Y, 
                            xAxis.Z, yAxis.Z, zAxis.Z, origin.Z,
                            0, 0, 0, 1);

            return new Trafo3f(mat, mat.Inverse);
        }

        /// <summary>
        /// Returns the <see cref="Trafo3f"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// Note that the axes MUST be normalized and normal to each other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f FromOrthoNormalBasis(V3f xAxis, V3f yAxis, V3f zAxis)
        {
            return new Trafo3f(
                        new M44f(
                            xAxis.X, yAxis.X, zAxis.X, 0, 
                            xAxis.Y, yAxis.Y, zAxis.Y, 0, 
                            xAxis.Z, yAxis.Z, zAxis.Z, 0,
                            0, 0, 0, 1),
                        new M44f(
                            xAxis.X, xAxis.Y, xAxis.Z, 0, 
                            yAxis.X, yAxis.Y, yAxis.Z, 0, 
                            zAxis.X, zAxis.Y, zAxis.Z, 0,
                            0, 0, 0, 1)
                        );
        }

        #region Translation

        /// <summary>
        /// Creates an <see cref="Trafo3f"/> transformation with the translational component given by a <see cref="V3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Translation(V3f v)
            => new Trafo3f(M44f.Translation(v), M44f.Translation(-v));

        /// <summary>
        /// Creates a <see cref="Trafo3f"/> transformation with the translational component given by 3 scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Translation(float tX, float tY, float tZ)
            => Translation(new V3f(tX, tY, tZ));

        /// <summary>
        /// Creates an <see cref="Trafo3f"/> transformation with the translational component given by a <see cref="Shift3f"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Translation(Shift3f shift)
            => Translation(shift.V);

        #endregion

        #region Scale

        /// <summary>
        /// Creates a scaling transformation using a <see cref="V3f"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Scale(V3f v) 
            => new Trafo3f(M44f.Scale(v), M44f.Scale(1 / v));

        /// <summary>
        /// Creates a scaling transformation using 3 scalars as scaling factors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Scale(float sX, float sY, float sZ)
            => new Trafo3f(M44f.Scale(sX, sY, sZ),
                           M44f.Scale(1 / sX, 1 / sY, 1 / sZ));

        /// <summary>
        /// Creates a scaling transformation using a uniform scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Scale(float s)
        {
            var t = 1 / s;
            return new Trafo3f(M44f.Scale(s), M44f.Scale(t));
        }

        /// <summary>
        /// Creates a scaling transformation using a <see cref="Scale3f"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Scale(Scale3f scale)
            => new Trafo3f(scale);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="Rot3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Rotation(Rot3f rotation)
            => new Trafo3f(rotation);

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in radians.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Rotation(V3f normalizedAxis, float angleInRadians)
            => new Trafo3f(M44f.Rotation(normalizedAxis, angleInRadians),
                           M44f.Rotation(normalizedAxis, -angleInRadians));

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in degrees.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationInDegrees(V3f normalizedAxis, float angleInDegrees)
            => Rotation(normalizedAxis, Conversion.RadiansFromDegrees(angleInDegrees));

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in radians. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationEuler(float rollInRadians, float pitchInRadians, float yawInRadians)
        {
            var m = M44f.RotationEuler(rollInRadians, pitchInRadians, yawInRadians);
            return new Trafo3f(m, m.Transposed); //transposed is equal but faster to inverted on orthonormal matrices like rotations.
        }

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in degrees. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationEulerInDegrees(float rollInDegrees, float pitchInDegrees, float yawInDegrees)
            => RotationEuler(rollInDegrees.RadiansFromDegrees(), pitchInDegrees.RadiansFromDegrees(), yawInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in radians.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationEuler(V3f rollPitchYawInRadians)
            => RotationEuler(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in degrees.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationEulerInDegrees(V3f rollPitchYawInDegrees)
            => RotationEulerInDegrees(rollPitchYawInDegrees.X, rollPitchYawInDegrees.Y, rollPitchYawInDegrees.Z);

        /// <summary>
        /// Creates a rotation transformation which rotates one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotateInto(V3f from, V3f into)
        {
            var rot = Rot3f.RotateInto(from, into);
            var inv = rot.Inverse;
            return new Trafo3f((M44f)rot, (M44f)inv);
        }

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationX(float angleInRadians)
            => new Trafo3f(M44f.RotationX(angleInRadians),
                           M44f.RotationX(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInDegrees"/> degrees around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationXInDegrees(float angleInDegrees)
            => RotationX(Conversion.RadiansFromDegrees(angleInDegrees));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInRadians"/> radians around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationY(float angleInRadians)
            => new Trafo3f(M44f.RotationY(angleInRadians),
                           M44f.RotationY(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInDegrees"/> degrees around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationYInDegrees(float angleInDegrees)
            => RotationY(Conversion.RadiansFromDegrees(angleInDegrees));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInRadians"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationZ(float angleInRadians)
            => new Trafo3f(M44f.RotationZ(angleInRadians),
                           M44f.RotationZ(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInDegrees"/> degrees around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f RotationZInDegrees(float angleInDegrees)
            => RotationZ(Conversion.RadiansFromDegrees(angleInDegrees));

        #endregion

        #region Shear

        /// <summary>
        /// Creates a shear transformation along the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f ShearYZ(float factorY, float factorZ)
            => new Trafo3f(M44f.ShearYZ(factorY, factorZ),
                           M44f.ShearYZ(-factorY, -factorZ));

        /// <summary>
        /// Creates a shear transformation along the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f ShearXZ(float factorX, float factorZ)
            => new Trafo3f(M44f.ShearXZ(factorX, factorZ),
                           M44f.ShearXZ(-factorX, -factorZ));

        /// <summary>
        /// Creates a shear transformation along the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f ShearXY(float factorX, float factorY)
            => new Trafo3f(M44f.ShearXY(factorX, factorY),
                           M44f.ShearXY(-factorX, -factorY));

        #endregion

        #region View transformation

        /// <summary>
        /// Creates a view transformation from the given vectors.
        /// </summary>
        /// <param name="location">Origin of the view</param>
        /// <param name="u">Right vector of the view-plane</param>
        /// <param name="v">Up vector of the view-plane</param>
        /// <param name="z">Normal vector of the view-plane. This vector is supposed to point in view-direction for a left-handed view transformation and in opposite direction in the right-handed case.</param>
        /// <returns>The view transformation</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f ViewTrafo(V3f location, V3f u, V3f v, V3f z)
        {
            return new Trafo3f(
                new M44f(
                    u.X, u.Y, u.Z, -Vec.Dot(u, location),
                    v.X, v.Y, v.Z, -Vec.Dot(v, location),
                    z.X, z.Y, z.Z, -Vec.Dot(z, location),
                    0, 0, 0, 1
                ),
                new M44f(
                    u.X, v.X, z.X, location.X,
                    u.Y, v.Y, z.Y, location.Y,
                    u.Z, v.Z, z.Z, location.Z,
                    0, 0, 0, 1
                ));
        }

        /// <summary>
        /// Creates a right-handed view trafo, where z-negative points into the scene.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f ViewTrafoRH(V3f location, V3f up, V3f forward)
            => ViewTrafo(location, forward.Cross(up), up, -forward);

        /// <summary>
        /// Creates a left-handed view trafo, where z-positive points into the scene.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f ViewTrafoLH(V3f location, V3f up, V3f forward)
            => ViewTrafo(location, up.Cross(forward), up, forward);

        #endregion

        #region Projection transformation

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f PerspectiveProjectionRH(float l, float r, float b, float t, float n, float f = float.PositiveInfinity)
        {
            float m22, m23, m32i;

            if (f.IsPositiveInfinity())
            {
                m22  = -1;
                m23  = -n;
                m32i = -1 / n;
            }
            else
            {
                m22  = f / (n - f);
                m23  = (f * n) / (n - f);
                m32i = (n - f) / (f * n);
            }

            return new Trafo3f(
                new M44f(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                     0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                     0,
                                    0,                     0,                   m22,                   m23,
                                    0,                     0,                    -1,                     0
                    ),
                new M44f(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                  m32i,                 1 / n
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f PerspectiveProjectionRH(float horizontalFovInRadians, float aspect, float n, float f = float.PositiveInfinity)
        {
            float d = Fun.Tan(0.5f * horizontalFovInRadians) * n;
            return Trafo3f.PerspectiveProjectionRH(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a right-handed reversed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, +1), (+1, +1, 0)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f PerspectiveProjectionReversedRH(float l, float r, float b, float t, float n, float f = float.PositiveInfinity)
        {
            float m22, m23, m32i, m33i;

            if (f.IsPositiveInfinity())
            {
                m22  = 0;
                m23  = n;
                m32i = 1 / n;
                m33i = 0;
            }
            else
            {
                m22  = n / (f - n);
                m23  = (f * n) / (f - n);
                m32i = (f - n) / (f * n);
                m33i = 1 / f;
            }

            return new Trafo3f(
                new M44f(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                     0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                     0,
                                    0,                     0,                   m22,                   m23,
                                    0,                     0,                    -1,                     0
                    ),
                new M44f(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                  m32i,                  m33i
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed reversed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, +1), (+1, +1, 0)].
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f PerspectiveProjectionReversedRH(float horizontalFovInRadians, float aspect, float n, float f = float.PositiveInfinity)
        {
            float d = Fun.Tan(0.5f * horizontalFovInRadians) * n;
            return Trafo3f.PerspectiveProjectionReversedRH(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)] and left-handed (handedness flip between view and NDC space).
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f PerspectiveProjectionGL(float l, float r, float b, float t, float n, float f = float.PositiveInfinity)
        {
            float m22, m23, m32i, m33i;

            if (f.IsPositiveInfinity())
            {
                m22  = -1;
                m23  = -2 * n;
                m32i = -1 / (2 * n);
                m33i = -m32i;
            }
            else
            {
                m22  = (f + n) / (n - f);
                m23  = (2 * f * n) / (n - f);
                m32i = (n - f) / (2 * f * n);
                m33i = (f + n) / (2 * f * n);
            }

            return new Trafo3f(
                new M44f(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                      0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                      0,
                                    0,                     0,                   m22,                    m23,
                                    0,                     0,                    -1,                      0
                    ),
                new M44f(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                   m32i,                 m33i
                    )
                );
        }

        [Obsolete("Use PerspectiveProjectionGL instead.")]
        public static Trafo3f PerspectiveProjectionOpenGl(float l, float r, float b, float t, float n, float f)
            => Trafo3f.PerspectiveProjectionGL(l, r, b, t, n, f);

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)] and left-handed (handedness flip between view and NDC space).
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f PerspectiveProjectionGL(float horizontalFovInRadians, float aspect, float n, float f = float.PositiveInfinity)
        {
            float d = Fun.Tan(0.5f * horizontalFovInRadians) * n;
            return Trafo3f.PerspectiveProjectionGL(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a right-handed reversed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, +1), (+1, +1, -1)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f PerspectiveProjectionReversedGL(float l, float r, float b, float t, float n, float f = float.PositiveInfinity)
        {
            float m22, m23, m32i, m33i;

            if (f.IsPositiveInfinity())
            {
                m22  = 1;
                m23  = 2 * n;
                m32i = 1 / (2 * n);
                m33i = m32i;
            }
            else
            {
                m22  = (f + n) / (f - n);
                m23  = (2 * f * n) / (f - n);
                m32i = (f - n) / (2 * f * n);
                m33i = (f + n) / (2 * f * n);
            }

            return new Trafo3f(
                new M44f(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                      0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                      0,
                                    0,                     0,                   m22,                    m23,
                                    0,                     0,                    -1,                      0
                    ),
                new M44f(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                   m32i,                 m33i
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed reversed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, +1), (+1, +1, -1)].
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f PerspectiveProjectionReversedGL(float horizontalFovInRadians, float aspect, float n, float f = float.PositiveInfinity)
        {
            float d = Fun.Tan(0.5f * horizontalFovInRadians) * n;
            return Trafo3f.PerspectiveProjectionReversedGL(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a left-handed perspective projection transform, where z-positive points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        [Obsolete("Broken, do not use.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f PerspectiveProjectionLH(float l, float r, float b, float t, float n, float f)
        {
            return new Trafo3f(
                new M44f(
                    (2 * n) / (r - l),                     0,                     0,                     0,
                                    0,     (2 * n) / (t - b),                     0,                     0,
                    (l + r) / (l - r),     (b + t) / (b - t),           f / (f - n),                     1,
                                    0,                     0,     (n * f) / (n - f),                     0
                    ),                                                     
                                                                       
                new M44f(                                      
                    (r - l) / (2 * n),                     0,                     0,                     0,
                                    0,     (t - b) / (2 * n),                     0,                     0,
                                    0,                     0,                     0,     (n - f) / (f * n),
                    (r + l) / (2 * n),     (t + b) / (2 * n),                     1,                 1 / n
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed orthographic projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f OrthoProjectionRH(float l, float r, float b, float t, float n, float f)
        {
            return new Trafo3f(
                new M44f(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     1 / (n - f),           n / (n - f),
                              0,               0,               0,                     1
                    ),
                new M44f(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,           n - f,                    -n,
                              0,               0,               0,                     1
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed orthographic projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)] and left-handed (handedness flip between view and NDC space).
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f OrthoProjectionGL(float l, float r, float b, float t, float n, float f)
        {
            return new Trafo3f(
                new M44f(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     2 / (n - f),     (f + n) / (n - f),
                              0,               0,               0,                     1
                    ),
                new M44f(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,     (n - f) / 2,          -(f + n) / 2,
                              0,               0,               0,                     1
                    )
                );
        }

        [Obsolete("Use OrthoProjectionGL instead.")]
        public static Trafo3f OrthoProjectionOpenGl(float l, float r, float b, float t, float n, float f)
            => Trafo3f.OrthoProjectionGL(l, r, b, t, n, f);

        #endregion

        #endregion

        #region Conversion Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo3d(Trafo3f r)
            => new Trafo3d(r);

        #endregion

        #region Operators

        /// <summary>
        /// Returns whether two <see cref="Trafo3f"/> transformations are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Trafo3f a, Trafo3f b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        /// <summary>
        /// Returns whether two <see cref="Trafo3f"/> transformations are different.
        /// </summary>
        public static bool operator !=(Trafo3f a, Trafo3f b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of Trafo3f multiplicaition is backward
        /// with respect to M44f multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static Trafo3f operator *(Trafo3f t0, Trafo3f t1)
            => new Trafo3f(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion 
    }

    public static partial class Trafo
    {
        #region Operations

        /// <summary>
        /// Returns the inverse of the given <see cref="Trafo3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f Inverse(Trafo3f trafo)
            => trafo.Inverse;

        /// <summary>
        /// Returns the forward matrix the given <see cref="Trafo3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44f Forward(Trafo3f trafo)
            => trafo.Forward;

        /// <summary>
        /// Returns the backward matrix the given <see cref="Trafo3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44f Backward(Trafo3f trafo)
            => trafo.Backward;

        #endregion

        #region Transformation Extraction

        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float GetScale(this Trafo3f trafo)
            => trafo.Forward.GetScale3();

        /// <summary>
        /// Extracts a scale vector from the given transformation by calculating the lengths of the basis vectors. 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetScaleVector(this Trafo3f trafo)
            => trafo.Forward.GetScaleVector3();

        /// <summary>
        /// Extracts the inverse/backward translation component of the given transformation, which when given 
        /// a view transformation represents the location of the camera in world space.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetViewPosition(this Trafo3f trafo)
            => trafo.Backward.C3.XYZ;

        /// <summary>
        /// Extracts the forward vector from the given view transformation.
        /// NOTE: A left-handed coordinates system transformation is expected, 
        /// where the view-space z-axis points in forward direction.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetViewDirectionLH(this Trafo3f trafo)
            => trafo.Forward.GetViewDirectionLH();

        /// <summary>
        /// Extracts the forward vector from the given view transformation.
        /// NOTE: A right-handed coordinates system transformation is expected, where 
        /// the view-space z-axis points opposit the forward vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetViewDirectionRH(this Trafo3f trafo)
            => trafo.Forward.GetViewDirectionRH();

        /// <summary>
        /// Extracts the translation component of the given transformation, which when given 
        /// a model transformation represents the model origin in world position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f GetModelOrigin(this Trafo3f trafo)
            => trafo.Forward.GetModelOrigin();

        /// <summary>
        /// Builds an ortho-normal orientation transformation form the given transform.
        /// Scale and Translation will be removed and basis vectors will be ortho-normalized.
        /// NOTE: The x-axis is untouched and y/z are forced to a normal-angle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3f GetOrthoNormalOrientation(this Trafo3f trafo)
        {
            var x = trafo.Forward.C0.XYZ.Normalized;
            var z = trafo.Forward.C2.XYZ;

            var y = z.Cross(x).Normalized;
            z = x.Cross(y).Normalized;

            return Trafo3f.FromOrthoNormalBasis(x, y, z);
        }

        /// <summary>
        /// Decomposes a transformation into a scale, rotation and translation component.
        /// NOTE: The input is assumed to be a valid affine transformation.
        ///       The rotation output is a vector with Euler-Angles [roll (X), pitch (Y), yaw (Z)] in radians of rotation order Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Decompose(this Trafo3f trafo, out V3f scale, out V3f rotationInRadians, out V3f translation)
        {
            translation = trafo.GetModelOrigin();
            
            var rt = trafo.GetOrthoNormalOrientation();
            if (rt.Forward.Determinant.IsTiny())
            {
                rotationInRadians = V3f.Zero;
            }
            else
            {
                var rot = Rot3f.FromFrame(rt.Forward.C0.XYZ, rt.Forward.C1.XYZ, rt.Forward.C2.XYZ);
                rotationInRadians = rot.GetEulerAngles();
            }

            scale = trafo.GetScaleVector();

            // if matrix is left-handed there must be some negative scale
            // since rotation remains the x-axis, the y-axis must be flipped
            if (trafo.Forward.Determinant < 0)
                scale.Y = -scale.Y;
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Transforms a <see cref="V4f"/> by a <see cref="Trafo3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f Transform(this Trafo3f r, V4f v)
            => r.Forward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by a <see cref="Trafo3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f TransformDir(this Trafo3f r, V3f v)
            => r.Forward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by a <see cref="Trafo3f"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f TransformPos(this Trafo3f r, V3f p)
            => r.Forward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by a <see cref="Trafo3f"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f TransformPosProj(this Trafo3f r, V3f p)
            => r.Forward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by a <see cref="Trafo3f"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f TransformPosProjFull(this Trafo3f r, V3f p)
            => r.Forward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.W is presumed 0.0) by a <see cref="Trafo3f"/>
        /// (i.e. by its transposed backward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f TransformNormal(this Trafo3f r, V3f n)
            => r.Backward.TransposedTransformDir(n);

        /// <summary>
        /// Transforms a <see cref="V4f"/> by the inverse of a <see cref="Trafo3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f InvTransform(this Trafo3f r, V4f v)
            => r.Backward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by the inverse of a <see cref="Trafo3f"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransformDir(this Trafo3f r, V3f v)
            => r.Backward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of a <see cref="Trafo3f"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransformPos(this Trafo3f r, V3f p)
            => r.Backward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of a <see cref="Trafo3f"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransformPosProj(this Trafo3f r, V3f p)
            => r.Backward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of a <see cref="Trafo3f"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4f InvTransformPosProjFull(this Trafo3f r, V3f p)
            => r.Backward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.W is presumed 0.0) by the inverse of a <see cref="Trafo3f"/>
        /// (i.e. by its transposed forward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3f InvTransformNormal(this Trafo3f r, V3f n)
            => r.Forward.TransposedTransformDir(n);

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        /// <summary>
        /// Returns if two <see cref="Trafo3f"/> transformations are equal with regard to a threshold <paramref name="epsilon"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Trafo3f a, Trafo3f b, float epsilon)
            => a.Forward.ApproximateEquals(b.Forward, epsilon) && a.Backward.ApproximateEquals(b.Backward, epsilon);

        #endregion
    }

    #endregion

    #region Trafo2d

    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct Trafo2d : IEquatable<Trafo2d>
    {
        [DataMember]
        public readonly M33d Forward;
        [DataMember]
        public readonly M33d Backward;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="Trafo2d"/> from a forward and backward transformation <see cref="M33d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2d(M33d forward, M33d backward)
        {
            Forward = forward;
            Backward = backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2d"/> from another <see cref="Trafo2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2d(Trafo2d trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2d"/> from a <see cref="Trafo2f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2d(Trafo2f trafo)
        {
            Forward = (M33d)trafo.Forward;
            Backward = (M33d)trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2d"/> from an <see cref="Affine2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2d(Affine2d trafo)
        {
            Forward = (M33d)trafo;
            Backward = (M33d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2d"/> from a <see cref="Euclidean2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2d(Euclidean2d trafo)
        {
            Forward = (M33d)trafo;
            Backward = (M33d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2d"/> from a <see cref="Similarity2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2d(Similarity2d trafo)
        {
            Forward = (M33d)trafo;
            Backward = (M33d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2d"/> from a <see cref="Rot2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2d(Rot2d trafo)
        {
            Forward = (M33d)trafo;
            Backward = (M33d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2d"/> from a <see cref="Scale2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2d(Scale2d trafo)
        {
            Forward = (M33d)trafo;
            Backward = (M33d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo2d"/> from a <see cref="Shift2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo2d(Shift2d trafo)
        {
            Forward = (M33d)trafo;
            Backward = (M33d)trafo.Inverse;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Trafo2d Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Trafo2d(M33d.Identity, M33d.Identity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the inverse of this <see cref="Trafo2d"/>.
        /// </summary>
        public Trafo2d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Trafo2d(Backward, Forward);
        }

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Forward, Backward);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Trafo2d other)
            => Forward.Equals(other.Forward) && Backward.Equals(other.Backward);

        public override bool Equals(object other)
            => (other is Trafo2d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        public static Trafo2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Trafo2d(
                M33d.Parse(x[0].ToString()),
                M33d.Parse(x[1].ToString())
            );
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Builds a transformation matrix using the scale, rotation (in radians) and translation components.
        /// NOTE: Uses the Scale * Rotation * Translation notion.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d FromComponents(V2d scale, double rotationInRadians, V2d translation)
            => Scale(scale) * Rotation(rotationInRadians) * Translation(translation);

        /// <summary>
        /// Returns the <see cref="Trafo2d"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d FromBasis(V2d xAxis, V2d yAxis, V2d origin)
        {
            var mat = new M33d(
                            xAxis.X, yAxis.X, origin.X, 
                            xAxis.Y, yAxis.Y, origin.Y,
                            0, 0, 1);

            return new Trafo2d(mat, mat.Inverse);
        }

        /// <summary>
        /// Returns the <see cref="Trafo2d"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// Note that the axes MUST be normalized and normal to each other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d FromOrthoNormalBasis(V2d xAxis, V2d yAxis)
        {
            return new Trafo2d(
                        new M33d(
                            xAxis.X, yAxis.X, 0, 
                            xAxis.Y, yAxis.Y, 0,
                            0, 0, 1),
                        new M33d(
                            xAxis.X, xAxis.Y, 0, 
                            yAxis.X, yAxis.Y, 0,
                            0, 0, 1)
                        );
        }

        #region Translation

        /// <summary>
        /// Creates an <see cref="Trafo2d"/> transformation with the translational component given by a <see cref="V2d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Translation(V2d v)
            => new Trafo2d(M33d.Translation(v), M33d.Translation(-v));

        /// <summary>
        /// Creates a <see cref="Trafo2d"/> transformation with the translational component given by 2 scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Translation(double tX, double tY)
            => Translation(new V2d(tX, tY));

        /// <summary>
        /// Creates an <see cref="Trafo2d"/> transformation with the translational component given by a <see cref="Shift2d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Translation(Shift2d shift)
            => Translation(shift.V);

        #endregion

        #region Scale

        /// <summary>
        /// Creates a scaling transformation using a <see cref="V2d"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Scale(V2d v) 
            => new Trafo2d(M33d.Scale(v), M33d.Scale(1 / v));

        /// <summary>
        /// Creates a scaling transformation using 2 scalars as scaling factors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Scale(double sX, double sY)
            => new Trafo2d(M33d.Scale(sX, sY),
                           M33d.Scale(1 / sX, 1 / sY));

        /// <summary>
        /// Creates a scaling transformation using a uniform scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Scale(double s)
        {
            var t = 1 / s;
            return new Trafo2d(M33d.Scale(s), M33d.Scale(t));
        }

        /// <summary>
        /// Creates a scaling transformation using a <see cref="Scale2d"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Scale(Scale2d scale)
            => new Trafo2d(scale);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="Rot2d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Rotation(Rot2d rotation)
            => new Trafo2d(rotation);

        /// <summary>
        /// Creates a rotation transformation with the specified angle in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Rotation(double angleInRadians)
            => new Trafo2d(M33d.Rotation(angleInRadians), M33d.Rotation(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation with the specified angle in degrees.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d RotationInDegrees(double angleInDegrees)
            => Rotation(angleInDegrees.RadiansFromDegrees());

        #endregion

        #endregion

        #region Conversion Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo2f(Trafo2d r)
            => new Trafo2f(r);

        #endregion

        #region Operators

        /// <summary>
        /// Returns whether two <see cref="Trafo2d"/> transformations are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Trafo2d a, Trafo2d b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        /// <summary>
        /// Returns whether two <see cref="Trafo2d"/> transformations are different.
        /// </summary>
        public static bool operator !=(Trafo2d a, Trafo2d b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of Trafo2d multiplicaition is backward
        /// with respect to M33d multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static Trafo2d operator *(Trafo2d t0, Trafo2d t1)
            => new Trafo2d(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion 
    }

    public static partial class Trafo
    {
        #region Operations

        /// <summary>
        /// Returns the inverse of the given <see cref="Trafo2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo2d Inverse(Trafo2d trafo)
            => trafo.Inverse;

        /// <summary>
        /// Returns the forward matrix the given <see cref="Trafo2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d Forward(Trafo2d trafo)
            => trafo.Forward;

        /// <summary>
        /// Returns the backward matrix the given <see cref="Trafo2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M33d Backward(Trafo2d trafo)
            => trafo.Backward;

        #endregion

        #region Transformation Extraction

        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetScale(this Trafo2d trafo)
            => trafo.Forward.GetScale2();

        /// <summary>
        /// Extracts a scale vector from the given transformation by calculating the lengths of the basis vectors. 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d GetScaleVector(this Trafo2d trafo)
            => trafo.Forward.GetScaleVector2();


        #endregion

        #region Transformations

        /// <summary>
        /// Transforms a <see cref="V3d"/> by a <see cref="Trafo2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d Transform(this Trafo2d r, V3d v)
            => r.Forward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by a <see cref="Trafo2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d TransformDir(this Trafo2d r, V2d v)
            => r.Forward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by a <see cref="Trafo2d"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d TransformPos(this Trafo2d r, V2d p)
            => r.Forward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by a <see cref="Trafo2d"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d TransformPosProj(this Trafo2d r, V2d p)
            => r.Forward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by a <see cref="Trafo2d"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d TransformPosProjFull(this Trafo2d r, V2d p)
            => r.Forward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.Z is presumed 0.0) by a <see cref="Trafo2d"/>
        /// (i.e. by its transposed backward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d TransformNormal(this Trafo2d r, V2d n)
            => r.Backward.TransposedTransformDir(n);

        /// <summary>
        /// Transforms a <see cref="V3d"/> by the inverse of a <see cref="Trafo2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransform(this Trafo2d r, V3d v)
            => r.Backward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.Z is presumed 0.0) by the inverse of a <see cref="Trafo2d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d InvTransformDir(this Trafo2d r, V2d v)
            => r.Backward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of a <see cref="Trafo2d"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d InvTransformPos(this Trafo2d r, V2d p)
            => r.Backward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of a <see cref="Trafo2d"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d InvTransformPosProj(this Trafo2d r, V2d p)
            => r.Backward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.Z is presumed 1.0) by the inverse of a <see cref="Trafo2d"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransformPosProjFull(this Trafo2d r, V2d p)
            => r.Backward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.Z is presumed 0.0) by the inverse of a <see cref="Trafo2d"/>
        /// (i.e. by its transposed forward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V2d InvTransformNormal(this Trafo2d r, V2d n)
            => r.Forward.TransposedTransformDir(n);

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        /// <summary>
        /// Returns if two <see cref="Trafo2d"/> transformations are equal with regard to a threshold <paramref name="epsilon"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Trafo2d a, Trafo2d b, double epsilon)
            => a.Forward.ApproximateEquals(b.Forward, epsilon) && a.Backward.ApproximateEquals(b.Backward, epsilon);

        #endregion
    }

    #endregion

    #region Trafo3d

    /// <summary>
    /// A trafo is a container for a forward and a backward matrix.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public readonly partial struct Trafo3d : IEquatable<Trafo3d>
    {
        [DataMember]
        public readonly M44d Forward;
        [DataMember]
        public readonly M44d Backward;

        #region Constructors

        /// <summary>
        /// Constructs a <see cref="Trafo3d"/> from a forward and backward transformation <see cref="M44d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3d(M44d forward, M44d backward)
        {
            Forward = forward;
            Backward = backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3d"/> from another <see cref="Trafo3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3d(Trafo3d trafo)
        {
            Forward = trafo.Forward;
            Backward = trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3d"/> from a <see cref="Trafo3f"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3d(Trafo3f trafo)
        {
            Forward = (M44d)trafo.Forward;
            Backward = (M44d)trafo.Backward;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3d"/> from an <see cref="Affine3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3d(Affine3d trafo)
        {
            Forward = (M44d)trafo;
            Backward = (M44d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3d"/> from a <see cref="Euclidean3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3d(Euclidean3d trafo)
        {
            Forward = (M44d)trafo;
            Backward = (M44d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3d"/> from a <see cref="Similarity3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3d(Similarity3d trafo)
        {
            Forward = (M44d)trafo;
            Backward = (M44d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3d"/> from a <see cref="Rot3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3d(Rot3d trafo)
        {
            Forward = (M44d)trafo;
            Backward = (M44d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3d"/> from a <see cref="Scale3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3d(Scale3d trafo)
        {
            Forward = (M44d)trafo;
            Backward = (M44d)trafo.Inverse;
        }

        /// <summary>
        /// Constructs a <see cref="Trafo3d"/> from a <see cref="Shift3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Trafo3d(Shift3d trafo)
        {
            Forward = (M44d)trafo;
            Backward = (M44d)trafo.Inverse;
        }

        #endregion

        #region Constants

        /// <summary>
        /// Gets the identity transformation.
        /// </summary>
        public static Trafo3d Identity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Trafo3d(M44d.Identity, M44d.Identity);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the inverse of this <see cref="Trafo3d"/>.
        /// </summary>
        public Trafo3d Inverse
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Trafo3d(Backward, Forward);
        }

        #endregion

        #region Overrides

        public override int GetHashCode() => HashCode.GetCombined(Forward, Backward);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Trafo3d other)
            => Forward.Equals(other.Forward) && Backward.Equals(other.Backward);

        public override bool Equals(object other)
            => (other is Trafo3d o) ? Equals(o) : false;

        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Forward, Backward);

        public static Trafo3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Trafo3d(
                M44d.Parse(x[0].ToString()),
                M44d.Parse(x[1].ToString())
            );
        }

        #endregion

        #region Static Creators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d FromNormalFrame(V3d origin, V3d normal)
        {
            M44d.NormalFrame(origin, normal, out M44d forward, out M44d backward);
            return new Trafo3d(forward, backward);
        }

        /// <summary>
        /// Builds a transformation matrix using the scale, rotation and translation components.
        /// NOTE: Uses the Scale * Rotation * Translation notion. 
        ///       The rotation is in Euler-Angles (roll, pitch, yaw) in radians.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d FromComponents(V3d scale, V3d rotationInRadians, V3d translation)
            => Scale(scale) * RotationEuler(rotationInRadians) * Translation(translation);

        /// <summary>
        /// Returns the <see cref="Trafo3d"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d FromBasis(V3d xAxis, V3d yAxis, V3d zAxis, V3d origin)
        {
            var mat = new M44d(
                            xAxis.X, yAxis.X, zAxis.X, origin.X, 
                            xAxis.Y, yAxis.Y, zAxis.Y, origin.Y, 
                            xAxis.Z, yAxis.Z, zAxis.Z, origin.Z,
                            0, 0, 0, 1);

            return new Trafo3d(mat, mat.Inverse);
        }

        /// <summary>
        /// Returns the <see cref="Trafo3d"/> that transforms from the coordinate system
        /// specified by the basis into the world coordinate system.
        /// Note that the axes MUST be normalized and normal to each other.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d FromOrthoNormalBasis(V3d xAxis, V3d yAxis, V3d zAxis)
        {
            return new Trafo3d(
                        new M44d(
                            xAxis.X, yAxis.X, zAxis.X, 0, 
                            xAxis.Y, yAxis.Y, zAxis.Y, 0, 
                            xAxis.Z, yAxis.Z, zAxis.Z, 0,
                            0, 0, 0, 1),
                        new M44d(
                            xAxis.X, xAxis.Y, xAxis.Z, 0, 
                            yAxis.X, yAxis.Y, yAxis.Z, 0, 
                            zAxis.X, zAxis.Y, zAxis.Z, 0,
                            0, 0, 0, 1)
                        );
        }

        #region Translation

        /// <summary>
        /// Creates an <see cref="Trafo3d"/> transformation with the translational component given by a <see cref="V3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Translation(V3d v)
            => new Trafo3d(M44d.Translation(v), M44d.Translation(-v));

        /// <summary>
        /// Creates a <see cref="Trafo3d"/> transformation with the translational component given by 3 scalars.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Translation(double tX, double tY, double tZ)
            => Translation(new V3d(tX, tY, tZ));

        /// <summary>
        /// Creates an <see cref="Trafo3d"/> transformation with the translational component given by a <see cref="Shift3d"/> vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Translation(Shift3d shift)
            => Translation(shift.V);

        #endregion

        #region Scale

        /// <summary>
        /// Creates a scaling transformation using a <see cref="V3d"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Scale(V3d v) 
            => new Trafo3d(M44d.Scale(v), M44d.Scale(1 / v));

        /// <summary>
        /// Creates a scaling transformation using 3 scalars as scaling factors.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Scale(double sX, double sY, double sZ)
            => new Trafo3d(M44d.Scale(sX, sY, sZ),
                           M44d.Scale(1 / sX, 1 / sY, 1 / sZ));

        /// <summary>
        /// Creates a scaling transformation using a uniform scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Scale(double s)
        {
            var t = 1 / s;
            return new Trafo3d(M44d.Scale(s), M44d.Scale(t));
        }

        /// <summary>
        /// Creates a scaling transformation using a <see cref="Scale3d"/> as scaling factor.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Scale(Scale3d scale)
            => new Trafo3d(scale);

        #endregion

        #region Rotation

        /// <summary>
        /// Creates a rotation transformation from a <see cref="Rot3d"/> transformation.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Rotation(Rot3d rotation)
            => new Trafo3d(rotation);

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in radians.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Rotation(V3d normalizedAxis, double angleInRadians)
            => new Trafo3d(M44d.Rotation(normalizedAxis, angleInRadians),
                           M44d.Rotation(normalizedAxis, -angleInRadians));

        /// <summary>
        /// Creates a rotation transformation from an axis vector and an angle in degrees.
        /// The axis vector has to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationInDegrees(V3d normalizedAxis, double angleInDegrees)
            => Rotation(normalizedAxis, Conversion.RadiansFromDegrees(angleInDegrees));

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in radians. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationEuler(double rollInRadians, double pitchInRadians, double yawInRadians)
        {
            var m = M44d.RotationEuler(rollInRadians, pitchInRadians, yawInRadians);
            return new Trafo3d(m, m.Transposed); //transposed is equal but faster to inverted on orthonormal matrices like rotations.
        }

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) in degrees. 
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationEulerInDegrees(double rollInDegrees, double pitchInDegrees, double yawInDegrees)
            => RotationEuler(rollInDegrees.RadiansFromDegrees(), pitchInDegrees.RadiansFromDegrees(), yawInDegrees.RadiansFromDegrees());

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in radians.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationEuler(V3d rollPitchYawInRadians)
            => RotationEuler(rollPitchYawInRadians.X, rollPitchYawInRadians.Y, rollPitchYawInRadians.Z);

        /// <summary>
        /// Creates a rotation transformation from roll (X), pitch (Y), and yaw (Z) vector in degrees.
        /// The rotation order is: Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationEulerInDegrees(V3d rollPitchYawInDegrees)
            => RotationEulerInDegrees(rollPitchYawInDegrees.X, rollPitchYawInDegrees.Y, rollPitchYawInDegrees.Z);

        /// <summary>
        /// Creates a rotation transformation which rotates one vector into another.
        /// The input vectors have to be normalized.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotateInto(V3d from, V3d into)
        {
            var rot = Rot3d.RotateInto(from, into);
            var inv = rot.Inverse;
            return new Trafo3d((M44d)rot, (M44d)inv);
        }

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInRadians"/> radians around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationX(double angleInRadians)
            => new Trafo3d(M44d.RotationX(angleInRadians),
                           M44d.RotationX(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInDegrees"/> degrees around the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationXInDegrees(double angleInDegrees)
            => RotationX(Conversion.RadiansFromDegrees(angleInDegrees));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInRadians"/> radians around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationY(double angleInRadians)
            => new Trafo3d(M44d.RotationY(angleInRadians),
                           M44d.RotationY(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInDegrees"/> degrees around the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationYInDegrees(double angleInDegrees)
            => RotationY(Conversion.RadiansFromDegrees(angleInDegrees));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInRadians"/> radians around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationZ(double angleInRadians)
            => new Trafo3d(M44d.RotationZ(angleInRadians),
                           M44d.RotationZ(-angleInRadians));

        /// <summary>
        /// Creates a rotation transformation by <paramref name="angleInDegrees"/> degrees around the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d RotationZInDegrees(double angleInDegrees)
            => RotationZ(Conversion.RadiansFromDegrees(angleInDegrees));

        #endregion

        #region Shear

        /// <summary>
        /// Creates a shear transformation along the x-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d ShearYZ(double factorY, double factorZ)
            => new Trafo3d(M44d.ShearYZ(factorY, factorZ),
                           M44d.ShearYZ(-factorY, -factorZ));

        /// <summary>
        /// Creates a shear transformation along the y-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d ShearXZ(double factorX, double factorZ)
            => new Trafo3d(M44d.ShearXZ(factorX, factorZ),
                           M44d.ShearXZ(-factorX, -factorZ));

        /// <summary>
        /// Creates a shear transformation along the z-axis.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d ShearXY(double factorX, double factorY)
            => new Trafo3d(M44d.ShearXY(factorX, factorY),
                           M44d.ShearXY(-factorX, -factorY));

        #endregion

        #region View transformation

        /// <summary>
        /// Creates a view transformation from the given vectors.
        /// </summary>
        /// <param name="location">Origin of the view</param>
        /// <param name="u">Right vector of the view-plane</param>
        /// <param name="v">Up vector of the view-plane</param>
        /// <param name="z">Normal vector of the view-plane. This vector is supposed to point in view-direction for a left-handed view transformation and in opposite direction in the right-handed case.</param>
        /// <returns>The view transformation</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d ViewTrafo(V3d location, V3d u, V3d v, V3d z)
        {
            return new Trafo3d(
                new M44d(
                    u.X, u.Y, u.Z, -Vec.Dot(u, location),
                    v.X, v.Y, v.Z, -Vec.Dot(v, location),
                    z.X, z.Y, z.Z, -Vec.Dot(z, location),
                    0, 0, 0, 1
                ),
                new M44d(
                    u.X, v.X, z.X, location.X,
                    u.Y, v.Y, z.Y, location.Y,
                    u.Z, v.Z, z.Z, location.Z,
                    0, 0, 0, 1
                ));
        }

        /// <summary>
        /// Creates a right-handed view trafo, where z-negative points into the scene.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d ViewTrafoRH(V3d location, V3d up, V3d forward)
            => ViewTrafo(location, forward.Cross(up), up, -forward);

        /// <summary>
        /// Creates a left-handed view trafo, where z-positive points into the scene.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d ViewTrafoLH(V3d location, V3d up, V3d forward)
            => ViewTrafo(location, up.Cross(forward), up, forward);

        #endregion

        #region Projection transformation

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d PerspectiveProjectionRH(double l, double r, double b, double t, double n, double f = double.PositiveInfinity)
        {
            double m22, m23, m32i;

            if (f.IsPositiveInfinity())
            {
                m22  = -1;
                m23  = -n;
                m32i = -1 / n;
            }
            else
            {
                m22  = f / (n - f);
                m23  = (f * n) / (n - f);
                m32i = (n - f) / (f * n);
            }

            return new Trafo3d(
                new M44d(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                     0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                     0,
                                    0,                     0,                   m22,                   m23,
                                    0,                     0,                    -1,                     0
                    ),
                new M44d(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                  m32i,                 1 / n
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d PerspectiveProjectionRH(double horizontalFovInRadians, double aspect, double n, double f = double.PositiveInfinity)
        {
            double d = Fun.Tan(0.5 * horizontalFovInRadians) * n;
            return Trafo3d.PerspectiveProjectionRH(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a right-handed reversed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, +1), (+1, +1, 0)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d PerspectiveProjectionReversedRH(double l, double r, double b, double t, double n, double f = double.PositiveInfinity)
        {
            double m22, m23, m32i, m33i;

            if (f.IsPositiveInfinity())
            {
                m22  = 0;
                m23  = n;
                m32i = 1 / n;
                m33i = 0;
            }
            else
            {
                m22  = n / (f - n);
                m23  = (f * n) / (f - n);
                m32i = (f - n) / (f * n);
                m33i = 1 / f;
            }

            return new Trafo3d(
                new M44d(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                     0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                     0,
                                    0,                     0,                   m22,                   m23,
                                    0,                     0,                    -1,                     0
                    ),
                new M44d(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                  m32i,                  m33i
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed reversed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, +1), (+1, +1, 0)].
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d PerspectiveProjectionReversedRH(double horizontalFovInRadians, double aspect, double n, double f = double.PositiveInfinity)
        {
            double d = Fun.Tan(0.5 * horizontalFovInRadians) * n;
            return Trafo3d.PerspectiveProjectionReversedRH(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)] and left-handed (handedness flip between view and NDC space).
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d PerspectiveProjectionGL(double l, double r, double b, double t, double n, double f = double.PositiveInfinity)
        {
            double m22, m23, m32i, m33i;

            if (f.IsPositiveInfinity())
            {
                m22  = -1;
                m23  = -2 * n;
                m32i = -1 / (2 * n);
                m33i = -m32i;
            }
            else
            {
                m22  = (f + n) / (n - f);
                m23  = (2 * f * n) / (n - f);
                m32i = (n - f) / (2 * f * n);
                m33i = (f + n) / (2 * f * n);
            }

            return new Trafo3d(
                new M44d(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                      0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                      0,
                                    0,                     0,                   m22,                    m23,
                                    0,                     0,                    -1,                      0
                    ),
                new M44d(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                   m32i,                 m33i
                    )
                );
        }

        [Obsolete("Use PerspectiveProjectionGL instead.")]
        public static Trafo3d PerspectiveProjectionOpenGl(double l, double r, double b, double t, double n, double f)
            => Trafo3d.PerspectiveProjectionGL(l, r, b, t, n, f);

        /// <summary>
        /// Creates a right-handed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)] and left-handed (handedness flip between view and NDC space).
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d PerspectiveProjectionGL(double horizontalFovInRadians, double aspect, double n, double f = double.PositiveInfinity)
        {
            double d = Fun.Tan(0.5 * horizontalFovInRadians) * n;
            return Trafo3d.PerspectiveProjectionGL(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a right-handed reversed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, +1), (+1, +1, -1)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume. Can be infinite.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d PerspectiveProjectionReversedGL(double l, double r, double b, double t, double n, double f = double.PositiveInfinity)
        {
            double m22, m23, m32i, m33i;

            if (f.IsPositiveInfinity())
            {
                m22  = 1;
                m23  = 2 * n;
                m32i = 1 / (2 * n);
                m33i = m32i;
            }
            else
            {
                m22  = (f + n) / (f - n);
                m23  = (2 * f * n) / (f - n);
                m32i = (f - n) / (2 * f * n);
                m33i = (f + n) / (2 * f * n);
            }

            return new Trafo3d(
                new M44d(
                    (2 * n) / (r - l),                     0,     (r + l) / (r - l),                      0,
                                    0,     (2 * n) / (t - b),     (t + b) / (t - b),                      0,
                                    0,                     0,                   m22,                    m23,
                                    0,                     0,                    -1,                      0
                    ),
                new M44d(
                    (r - l) / (2 * n),                     0,                     0,     (r + l) / (2 * n),
                                    0,     (t - b) / (2 * n),                     0,     (t + b) / (2 * n),
                                    0,                     0,                     0,                    -1,
                                    0,                     0,                   m32i,                 m33i
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed reversed perspective projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, +1), (+1, +1, -1)].
        /// </summary>
        /// <param name="horizontalFovInRadians">Horizontal field of view in radians.</param>
        /// <param name="aspect">Aspect ratio, defined as view space width divided by height.</param>
        /// <param name="n">Z-value of the near view-plane.</param>
        /// <param name="f">Z-value of the far view-plane. Can be infinite.</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d PerspectiveProjectionReversedGL(double horizontalFovInRadians, double aspect, double n, double f = double.PositiveInfinity)
        {
            double d = Fun.Tan(0.5 * horizontalFovInRadians) * n;
            return Trafo3d.PerspectiveProjectionReversedGL(-d, d, -d / aspect, d / aspect, n, f);
        }

        /// <summary>
        /// Creates a left-handed perspective projection transform, where z-positive points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        [Obsolete("Broken, do not use.")]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d PerspectiveProjectionLH(double l, double r, double b, double t, double n, double f)
        {
            return new Trafo3d(
                new M44d(
                    (2 * n) / (r - l),                     0,                     0,                     0,
                                    0,     (2 * n) / (t - b),                     0,                     0,
                    (l + r) / (l - r),     (b + t) / (b - t),           f / (f - n),                     1,
                                    0,                     0,     (n * f) / (n - f),                     0
                    ),                                                     
                                                                       
                new M44d(                                      
                    (r - l) / (2 * n),                     0,                     0,                     0,
                                    0,     (t - b) / (2 * n),                     0,                     0,
                                    0,                     0,                     0,     (n - f) / (f * n),
                    (r + l) / (2 * n),     (t + b) / (2 * n),                     1,                 1 / n
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed orthographic projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, 0), (+1, +1, +1)].
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d OrthoProjectionRH(double l, double r, double b, double t, double n, double f)
        {
            return new Trafo3d(
                new M44d(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     1 / (n - f),           n / (n - f),
                              0,               0,               0,                     1
                    ),
                new M44d(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,           n - f,                    -n,
                              0,               0,               0,                     1
                    )
                );
        }

        /// <summary>
        /// Creates a right-handed orthographic projection transform, where z-negative points into the scene.
        /// The resulting canonical view volume is [(-1, -1, -1), (+1, +1, +1)] and left-handed (handedness flip between view and NDC space).
        /// </summary>
        /// <param name="l">Minimum x-value of the view volume.</param>
        /// <param name="r">Maximum x-value of the view volume.</param>
        /// <param name="b">Minimum y-value of the view volume.</param>
        /// <param name="t">Maximum y-value of the view volume.</param>
        /// <param name="n">Minimum z-value of the view volume.</param>
        /// <param name="f">Maximum z-value of the view volume.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d OrthoProjectionGL(double l, double r, double b, double t, double n, double f)
        {
            return new Trafo3d(
                new M44d(
                    2 / (r - l),               0,               0,     (l + r) / (l - r),
                              0,     2 / (t - b),               0,     (b + t) / (b - t),
                              0,               0,     2 / (n - f),     (f + n) / (n - f),
                              0,               0,               0,                     1
                    ),
                new M44d(
                    (r - l) / 2,               0,               0,           (l + r) / 2,
                              0,     (t - b) / 2,               0,           (b + t) / 2,
                              0,               0,     (n - f) / 2,          -(f + n) / 2,
                              0,               0,               0,                     1
                    )
                );
        }

        [Obsolete("Use OrthoProjectionGL instead.")]
        public static Trafo3d OrthoProjectionOpenGl(double l, double r, double b, double t, double n, double f)
            => Trafo3d.OrthoProjectionGL(l, r, b, t, n, f);

        #endregion

        #endregion

        #region Conversion Operators

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Trafo3f(Trafo3d r)
            => new Trafo3f(r);

        #endregion

        #region Operators

        /// <summary>
        /// Returns whether two <see cref="Trafo3d"/> transformations are equal.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Trafo3d a, Trafo3d b)
            => a.Forward == b.Forward && a.Backward == b.Backward;

        /// <summary>
        /// Returns whether two <see cref="Trafo3d"/> transformations are different.
        /// </summary>
        public static bool operator !=(Trafo3d a, Trafo3d b)
            => a.Forward != b.Forward || a.Backward != b.Backward;

        /// <summary>
        /// The order of operation of Trafo3d multiplicaition is backward
        /// with respect to M44d multiplication in order to provide
        /// natural postfix notation.
        /// </summary>
        public static Trafo3d operator *(Trafo3d t0, Trafo3d t1)
            => new Trafo3d(t1.Forward * t0.Forward, t0.Backward * t1.Backward);

        #endregion 
    }

    public static partial class Trafo
    {
        #region Operations

        /// <summary>
        /// Returns the inverse of the given <see cref="Trafo3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d Inverse(Trafo3d trafo)
            => trafo.Inverse;

        /// <summary>
        /// Returns the forward matrix the given <see cref="Trafo3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44d Forward(Trafo3d trafo)
            => trafo.Forward;

        /// <summary>
        /// Returns the backward matrix the given <see cref="Trafo3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static M44d Backward(Trafo3d trafo)
            => trafo.Backward;

        #endregion

        #region Transformation Extraction

        /// <summary>
        /// Approximates the uniform scale value of the given transformation (average length of basis vectors).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double GetScale(this Trafo3d trafo)
            => trafo.Forward.GetScale3();

        /// <summary>
        /// Extracts a scale vector from the given transformation by calculating the lengths of the basis vectors. 
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetScaleVector(this Trafo3d trafo)
            => trafo.Forward.GetScaleVector3();

        /// <summary>
        /// Extracts the inverse/backward translation component of the given transformation, which when given 
        /// a view transformation represents the location of the camera in world space.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetViewPosition(this Trafo3d trafo)
            => trafo.Backward.C3.XYZ;

        /// <summary>
        /// Extracts the forward vector from the given view transformation.
        /// NOTE: A left-handed coordinates system transformation is expected, 
        /// where the view-space z-axis points in forward direction.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetViewDirectionLH(this Trafo3d trafo)
            => trafo.Forward.GetViewDirectionLH();

        /// <summary>
        /// Extracts the forward vector from the given view transformation.
        /// NOTE: A right-handed coordinates system transformation is expected, where 
        /// the view-space z-axis points opposit the forward vector.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetViewDirectionRH(this Trafo3d trafo)
            => trafo.Forward.GetViewDirectionRH();

        /// <summary>
        /// Extracts the translation component of the given transformation, which when given 
        /// a model transformation represents the model origin in world position.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d GetModelOrigin(this Trafo3d trafo)
            => trafo.Forward.GetModelOrigin();

        /// <summary>
        /// Builds a hull from the given view-projection transformation (left, right, bottom, top, near, far).
        /// The view volume is assumed to be [-1, -1, -1] [1, 1, 1].
        /// The normals of the hull planes point to the outside and are normalized. 
        /// A point inside the visual hull will has negative height to all planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Hull3d GetVisualHull(this Trafo3d viewProj)
            => viewProj.Forward.GetVisualHull();

        /// <summary>
        /// Builds an ortho-normal orientation transformation form the given transform.
        /// Scale and Translation will be removed and basis vectors will be ortho-normalized.
        /// NOTE: The x-axis is untouched and y/z are forced to a normal-angle.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Trafo3d GetOrthoNormalOrientation(this Trafo3d trafo)
        {
            var x = trafo.Forward.C0.XYZ.Normalized;
            var z = trafo.Forward.C2.XYZ;

            var y = z.Cross(x).Normalized;
            z = x.Cross(y).Normalized;

            return Trafo3d.FromOrthoNormalBasis(x, y, z);
        }

        /// <summary>
        /// Decomposes a transformation into a scale, rotation and translation component.
        /// NOTE: The input is assumed to be a valid affine transformation.
        ///       The rotation output is a vector with Euler-Angles [roll (X), pitch (Y), yaw (Z)] in radians of rotation order Z, Y, X.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Decompose(this Trafo3d trafo, out V3d scale, out V3d rotationInRadians, out V3d translation)
        {
            translation = trafo.GetModelOrigin();
            
            var rt = trafo.GetOrthoNormalOrientation();
            if (rt.Forward.Determinant.IsTiny())
            {
                rotationInRadians = V3d.Zero;
            }
            else
            {
                var rot = Rot3d.FromFrame(rt.Forward.C0.XYZ, rt.Forward.C1.XYZ, rt.Forward.C2.XYZ);
                rotationInRadians = rot.GetEulerAngles();
            }

            scale = trafo.GetScaleVector();

            // if matrix is left-handed there must be some negative scale
            // since rotation remains the x-axis, the y-axis must be flipped
            if (trafo.Forward.Determinant < 0)
                scale.Y = -scale.Y;
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Transforms a <see cref="V4d"/> by a <see cref="Trafo3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d Transform(this Trafo3d r, V4d v)
            => r.Forward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by a <see cref="Trafo3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d TransformDir(this Trafo3d r, V3d v)
            => r.Forward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by a <see cref="Trafo3d"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d TransformPos(this Trafo3d r, V3d p)
            => r.Forward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by a <see cref="Trafo3d"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d TransformPosProj(this Trafo3d r, V3d p)
            => r.Forward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by a <see cref="Trafo3d"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d TransformPosProjFull(this Trafo3d r, V3d p)
            => r.Forward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.W is presumed 0.0) by a <see cref="Trafo3d"/>
        /// (i.e. by its transposed backward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d TransformNormal(this Trafo3d r, V3d n)
            => r.Backward.TransposedTransformDir(n);

        /// <summary>
        /// Transforms a <see cref="V4d"/> by the inverse of a <see cref="Trafo3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d InvTransform(this Trafo3d r, V4d v)
            => r.Backward.Transform(v);

        /// <summary>
        /// Transforms direction vector v (v.W is presumed 0.0) by the inverse of a <see cref="Trafo3d"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransformDir(this Trafo3d r, V3d v)
            => r.Backward.TransformDir(v);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of a <see cref="Trafo3d"/>.
        /// No projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransformPos(this Trafo3d r, V3d p)
            => r.Backward.TransformPos(p);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of a <see cref="Trafo3d"/>.
        /// Projective transform is performed. Perspective Division is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransformPosProj(this Trafo3d r, V3d p)
            => r.Backward.TransformPosProj(p);

        /// <summary>
        /// Transforms point p (p.W is presumed 1.0) by the inverse of a <see cref="Trafo3d"/>.
        /// Projective transform is performed.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V4d InvTransformPosProjFull(this Trafo3d r, V3d p)
            => r.Backward.TransformPosProjFull(p);

        /// <summary>
        /// Transforms normal vector n (n.W is presumed 0.0) by the inverse of a <see cref="Trafo3d"/>
        /// (i.e. by its transposed forward matrix).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static V3d InvTransformNormal(this Trafo3d r, V3d n)
            => r.Forward.TransposedTransformDir(n);

        #endregion
    }

    public static partial class Fun
    {
        #region ApproximateEquals

        /// <summary>
        /// Returns if two <see cref="Trafo3d"/> transformations are equal with regard to a threshold <paramref name="epsilon"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Trafo3d a, Trafo3d b, double epsilon)
            => a.Forward.ApproximateEquals(b.Forward, epsilon) && a.Backward.ApproximateEquals(b.Backward, epsilon);

        #endregion
    }

    #endregion

}
