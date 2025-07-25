/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Runtime.CompilerServices;

namespace Aardvark.Base;

#region Rot2f

/// <summary>
/// Represents a 2D rotation counterclockwise around the origin.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Rot2f : IEquatable<Rot2f>
{
    [DataMember]
    public float Angle;

    #region Constructors

    /// <summary>
    /// Constructs a <see cref="Rot2f"/> transformation given a rotation angle in radians.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rot2f(float angleInRadians)
    {
        Angle = angleInRadians;
    }

    /// <summary>
    /// Constructs a copy of a <see cref="Rot2f"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rot2f(Rot2f r)
    {
        Angle = r.Angle;
    }

    /// <summary>
    /// Constructs a <see cref="Rot2f"/> transformation from a <see cref="Rot2d"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rot2f(Rot2d r)
    {
        Angle = (float)r.Angle;
    }

    #endregion

    #region Constants

    /// <summary>
    /// Gets the identity <see cref="Rot2f"/> transformation.
    /// </summary>
    public static Rot2f Identity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(0);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the inverse of this <see cref="Rot2f"/> tranformation.
    /// </summary>
    public readonly Rot2f Inverse
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(-Angle);
    }

    #endregion

    #region Arithmetic operators

    /// <summary>
    /// Multiplies two <see cref="Rot2f"/> transformations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f operator *(Rot2f r0, Rot2f r1)
        => new(r0.Angle + r1.Angle);

    #region Rot / Vector Multiplication

    /// <summary>
    /// Multiplies a <see cref="Rot2f"/> transformation with a <see cref="V2f"/>.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2f operator *(Rot2f rot, V2f vec)
    {
        float a = Fun.Cos(rot.Angle);
        float b = Fun.Sin(rot.Angle);

        return new(a * vec.X + -b * vec.Y, b * vec.X + a * vec.Y);
    }

    #endregion

    #region Rot / Matrix Multiplication

    /// <summary>
    /// Multiplies a <see cref="Rot2f"/> transformation (as a 2x2 matrix) with a <see cref="M22f"/>.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M22f operator *(Rot2f r, M22f m)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
            a * m.M00 + -b * m.M10, 
            a * m.M01 + -b * m.M11,

            b * m.M00 + a * m.M10, 
            b * m.M01 + a * m.M11);
    }

    /// <summary>
    /// Multiplies a <see cref="M22f"/> with a <see cref="Rot2f"/> transformation (as a 2x2 matrix).
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M22f operator *(M22f m, Rot2f r)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
            m.M00 * a + m.M01 * b, 
            m.M00 * -b + m.M01 * a,

            m.M10 * a + m.M11 * b, 
            m.M10 * -b + m.M11 * a);
    }

    /// <summary>
    /// Multiplies a <see cref="Rot2f"/> transformation (as a 2x2 matrix) with a <see cref="M23f"/>.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M23f operator *(Rot2f r, M23f m)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
            a * m.M00 + -b * m.M10, 
            a * m.M01 + -b * m.M11, 
            a * m.M02 + -b * m.M12,

            b * m.M00 + a * m.M10, 
            b * m.M01 + a * m.M11, 
            b * m.M02 + a * m.M12);
    }

    /// <summary>
    /// Multiplies a <see cref="M23f"/> with a <see cref="Rot2f"/> transformation (as a 3x3 matrix).
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M23f operator *(M23f m, Rot2f r)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
            m.M00 * a + m.M01 * b, 
            m.M00 * -b + m.M01 * a,
            m.M02,

            m.M10 * a + m.M11 * b, 
            m.M10 * -b + m.M11 * a,
            m.M12);
    }

    /// <summary>
    /// Multiplies a <see cref="Rot2f"/> transformation (as a 3x3 matrix) with a <see cref="M33f"/>.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M33f operator *(Rot2f r, M33f m)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
            a * m.M00 + -b * m.M10, 
            a * m.M01 + -b * m.M11, 
            a * m.M02 + -b * m.M12,

            b * m.M00 + a * m.M10, 
            b * m.M01 + a * m.M11, 
            b * m.M02 + a * m.M12,
            
            m.M20, m.M21, m.M22);
    }

    /// <summary>
    /// Multiplies a <see cref="M33f"/> with a <see cref="Rot2f"/> transformation (as a 3x3 matrix).
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M33f operator *(M33f m, Rot2f r)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
            m.M00 * a + m.M01 * b, 
            m.M00 * -b + m.M01 * a,
            m.M02,

            m.M10 * a + m.M11 * b, 
            m.M10 * -b + m.M11 * a,
            m.M12,

            m.M20 * a + m.M21 * b, 
            m.M20 * -b + m.M21 * a,
            m.M22);
    }

    #endregion

    #region Rot / Shift, Scale Multiplication

    /// <summary>
    /// Multiplies a <see cref="Rot2f"/> transformation with a <see cref="Shift2f"/> transformation.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Euclidean2f operator *(Rot2f a, Shift2f b)
        => new(a, a * b.V);

    /// <summary>
    /// Multiplies a <see cref="Rot2f"/> transformation with a <see cref="Scale2f"/> transformation.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Affine2f operator *(Rot2f a, Scale2f b)
        => new((M22f)a * (M22f)b);

    #endregion

    #endregion

    #region Comparison Operators

    /// <summary>
    /// Checks if 2 rotations are equal.
    /// </summary>
    /// <returns>Result of comparison.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Rot2f rotation1, Rot2f rotation2)
        => Rot.Distance(rotation1, rotation2) == 0;

    /// <summary>
    /// Checks if 2 rotations are not equal.
    /// </summary>
    /// <returns>Result of comparison.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Rot2f rotation1, Rot2f rotation2)
        => Rot.Distance(rotation1, rotation2) != 0;

    #endregion

    #region Static Creators

    /// <summary>
    /// Creates a <see cref="Rot2f"/> transformation from a <see cref="M22f"/> matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f FromM22f(M22f m)
        => new(m.GetRotation());

    /// <summary>
    /// Creates a <see cref="Rot2f"/> transformation from a <see cref="M33f"/> matrix.
    /// The matrix has to be homogeneous and must not contain perspective components.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f FromM33f(M33f m, float epsilon = 1e-5f)
    {
        if (!(m.M20.IsTiny(epsilon) && m.M21.IsTiny(epsilon)))
            throw new ArgumentException("Matrix contains perspective components.");

        if (!m.C2.XY.ApproximateEquals(V2f.Zero, epsilon))
            throw new ArgumentException("Matrix contains translational component.");

        if (m.M22.IsTiny(epsilon))
            throw new ArgumentException("Matrix is not homogeneous.");

        return FromM22f(((M22f)m) / m.M22);
    }

    /// <summary>
    /// Creates a <see cref="Rot2f"/> transformation from a <see cref="Euclidean2f"/>.
    /// The transformation <paramref name="euclidean"/> must only consist of a rotation.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f FromEuclidean2f(Euclidean2f euclidean, float epsilon = 1e-5f)
    {
        if (!euclidean.Trans.ApproximateEquals(V2f.Zero, epsilon))
            throw new ArgumentException("Euclidean transformation contains translational component");

        return euclidean.Rot;
    }

    /// <summary>
    /// Creates a <see cref="Rot2f"/> transformation from a <see cref="Similarity2f"/>.
    /// The transformation <paramref name="similarity"/> must only consist of a rotation.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f FromSimilarity2f(Similarity2f similarity, float epsilon = 1e-5f)
    {
        if (!similarity.Scale.ApproximateEquals(1, epsilon))
            throw new ArgumentException("Similarity transformation contains scaling component");

        if (!similarity.Trans.ApproximateEquals(V2f.Zero, epsilon))
            throw new ArgumentException("Similarity transformation contains translational component");

        return similarity.Rot;
    }

    /// <summary>
    /// Creates a <see cref="Rot2f"/> transformation from an <see cref="Affine2f"/>.
    /// The transformation <paramref name="affine"/> must only consist of a rotation.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f FromAffine2f(Affine2f affine, float epsilon = 1e-5f)
        => FromM33f((M33f)affine, epsilon);

    /// <summary>
    /// Creates a <see cref="Rot2f"/> transformation from a <see cref="Trafo2f"/>.
    /// The transformation <paramref name="trafo"/> must only consist of a rotation.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f FromTrafo2f(Trafo2f trafo, float epsilon = 1e-5f)
        => FromM33f(trafo.Forward, epsilon);

    /// <summary>
    /// Creates a <see cref="Rot2f"/> transformation with the specified angle in radians.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f FromRadians(float angleInRadians)
        => new(angleInRadians);

    /// <summary>
    /// Creates a <see cref="Rot2f"/> transformation with the specified angle in degrees.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f FromDegrees(float angleInDegrees)
        => new(angleInDegrees.RadiansFromDegrees());

    #endregion

    #region Conversion Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M22f(Rot2f r)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
             a, -b, 
             b,  a);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M23f(Rot2f r)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
             a, -b,  0, 
             b,  a,  0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M33f(Rot2f r)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
             a, -b,  0, 
             b,  a,  0, 
             0,  0,  1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M34f(Rot2f r)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
             a, -b,  0,  0, 
             b,  a,  0,  0, 
             0,  0,  1,  0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M44f(Rot2f r)
    {
        float a = Fun.Cos(r.Angle);
        float b = Fun.Sin(r.Angle);

        return new(
             a, -b,  0,  0, 
             b,  a,  0,  0, 
             0,  0,  1,  0, 
             0,  0,  0,  1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Euclidean2f(Rot2f r)
        => new(r, V2f.Zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Similarity2f(Rot2f r)
        => new(1, r, V2f.Zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Affine2f(Rot2f r)
        => new((M22f)r);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Trafo2f(Rot2f r)
        => new((M33f)r, (M33f)r.Inverse);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Rot2d(Rot2f r)
        => new((double)r.Angle);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
        => Angle.GetHashCode();

    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}]", Angle);

    public static Rot2f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new(
            float.Parse(x[0], CultureInfo.InvariantCulture)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Rot2f other)
        => Rot.Distance(this, other) == 0;

    public override readonly bool Equals(object other)
        => (other is Rot2f o) && Equals(o);

    #endregion
}

public static partial class Rot
{
    #region Invert

    /// <summary>
    /// Returns the inverse of a <see cref="Rot2f"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2f Inverse(Rot2f rot)
        => rot.Inverse;

    /// <summary>
    /// Inverts a <see cref="Rot2f"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Invert(this ref Rot2f rot)
    {
        rot.Angle = -rot.Angle;
    }

    #endregion

    #region Distance

    /// <summary>
    /// Returns the absolute difference in radians between two <see cref="Rot2f"/> rotations.
    /// The result is within the range of [0, Pi].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Distance(this Rot2f r1, Rot2f r2)
        => Fun.AngleDistance(r1.Angle, r2.Angle);

    /// <summary>
    /// Returns the signed difference in radians between two <see cref="Rot2f"/> rotations.
    /// The result is within the range of [-Pi, Pi).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Difference(this Rot2f r1, Rot2f r2)
        => Fun.AngleDifference(r1.Angle, r2.Angle);

    #endregion

    #region Transform

    /// <summary>
    /// Transforms a <see cref="V2f"/> vector by a <see cref="Rot2f"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2f Transform(this Rot2f rot, V2f v)
        => rot * v;

    /// <summary>
    /// Transforms a <see cref="V3f"/> vector by a <see cref="Rot2f"/> transformation.
    /// The z coordinate of the vector is unaffected.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3f Transform(this Rot2f rot, V3f v)
    {
        float a = Fun.Cos(rot.Angle);
        float b = Fun.Sin(rot.Angle);

        return new(a * v.X + -b * v.Y,
                    b * v.X + a * v.Y,
                    v.Z);
    }

    /// <summary>
    /// Transforms a <see cref="V4f"/> vector by a <see cref="Rot2f"/> transformation.
    /// The z and w coordinates of the vector are unaffected.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V4f Transform(this Rot2f rot, V4f v)
    {
        float a = Fun.Cos(rot.Angle);
        float b = Fun.Sin(rot.Angle);

        return new(a * v.X + -b * v.Y,
                    b * v.X + a * v.Y,
                    v.Z, v.W);
    }

    /// <summary>
    /// Transforms a <see cref="V2f"/> vector by the inverse of a <see cref="Rot2f"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2f InvTransform(this Rot2f rot, V2f v)
    {
        float a = Fun.Cos(-rot.Angle);
        float b = Fun.Sin(-rot.Angle);

        return new(a * v.X + -b * v.Y, b * v.X + a * v.Y);
    }

    /// <summary>
    /// Transforms a <see cref="V3f"/> vector by the inverse of a <see cref="Rot2f"/> transformation.
    /// The z coordinate of the vector is unaffected.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3f InvTransform(this Rot2f rot, V3f v)
        => Transform(rot.Inverse, v);

    /// <summary>
    /// Transforms a <see cref="V4f"/> vector by the inverse of a <see cref="Rot2f"/> transformation.
    /// The z and w coordinates of the vector are unaffected.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V4f InvTransform(this Rot2f rot, V4f v)
        => Transform(rot.Inverse, v);

    #endregion
}

public static partial class Fun
{
    #region ApproximateEquals

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Rot2f r0, Rot2f r1)
        => ApproximateEquals(r0, r1, Constant<float>.PositiveTinyValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Rot2f r0, Rot2f r1, float tolerance)
        => Rot.Distance(r0, r1) <= tolerance;

    #endregion
}

#endregion

#region Rot2d

/// <summary>
/// Represents a 2D rotation counterclockwise around the origin.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Rot2d : IEquatable<Rot2d>
{
    [DataMember]
    public double Angle;

    #region Constructors

    /// <summary>
    /// Constructs a <see cref="Rot2d"/> transformation given a rotation angle in radians.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rot2d(double angleInRadians)
    {
        Angle = angleInRadians;
    }

    /// <summary>
    /// Constructs a copy of a <see cref="Rot2d"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rot2d(Rot2d r)
    {
        Angle = r.Angle;
    }

    /// <summary>
    /// Constructs a <see cref="Rot2d"/> transformation from a <see cref="Rot2f"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Rot2d(Rot2f r)
    {
        Angle = (double)r.Angle;
    }

    #endregion

    #region Constants

    /// <summary>
    /// Gets the identity <see cref="Rot2d"/> transformation.
    /// </summary>
    public static Rot2d Identity
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(0);
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the inverse of this <see cref="Rot2d"/> tranformation.
    /// </summary>
    public readonly Rot2d Inverse
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(-Angle);
    }

    #endregion

    #region Arithmetic operators

    /// <summary>
    /// Multiplies two <see cref="Rot2d"/> transformations.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d operator *(Rot2d r0, Rot2d r1)
        => new(r0.Angle + r1.Angle);

    #region Rot / Vector Multiplication

    /// <summary>
    /// Multiplies a <see cref="Rot2d"/> transformation with a <see cref="V2d"/>.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2d operator *(Rot2d rot, V2d vec)
    {
        double a = Fun.Cos(rot.Angle);
        double b = Fun.Sin(rot.Angle);

        return new(a * vec.X + -b * vec.Y, b * vec.X + a * vec.Y);
    }

    #endregion

    #region Rot / Matrix Multiplication

    /// <summary>
    /// Multiplies a <see cref="Rot2d"/> transformation (as a 2x2 matrix) with a <see cref="M22d"/>.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M22d operator *(Rot2d r, M22d m)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
            a * m.M00 + -b * m.M10, 
            a * m.M01 + -b * m.M11,

            b * m.M00 + a * m.M10, 
            b * m.M01 + a * m.M11);
    }

    /// <summary>
    /// Multiplies a <see cref="M22d"/> with a <see cref="Rot2d"/> transformation (as a 2x2 matrix).
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M22d operator *(M22d m, Rot2d r)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
            m.M00 * a + m.M01 * b, 
            m.M00 * -b + m.M01 * a,

            m.M10 * a + m.M11 * b, 
            m.M10 * -b + m.M11 * a);
    }

    /// <summary>
    /// Multiplies a <see cref="Rot2d"/> transformation (as a 2x2 matrix) with a <see cref="M23d"/>.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M23d operator *(Rot2d r, M23d m)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
            a * m.M00 + -b * m.M10, 
            a * m.M01 + -b * m.M11, 
            a * m.M02 + -b * m.M12,

            b * m.M00 + a * m.M10, 
            b * m.M01 + a * m.M11, 
            b * m.M02 + a * m.M12);
    }

    /// <summary>
    /// Multiplies a <see cref="M23d"/> with a <see cref="Rot2d"/> transformation (as a 3x3 matrix).
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M23d operator *(M23d m, Rot2d r)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
            m.M00 * a + m.M01 * b, 
            m.M00 * -b + m.M01 * a,
            m.M02,

            m.M10 * a + m.M11 * b, 
            m.M10 * -b + m.M11 * a,
            m.M12);
    }

    /// <summary>
    /// Multiplies a <see cref="Rot2d"/> transformation (as a 3x3 matrix) with a <see cref="M33d"/>.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M33d operator *(Rot2d r, M33d m)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
            a * m.M00 + -b * m.M10, 
            a * m.M01 + -b * m.M11, 
            a * m.M02 + -b * m.M12,

            b * m.M00 + a * m.M10, 
            b * m.M01 + a * m.M11, 
            b * m.M02 + a * m.M12,
            
            m.M20, m.M21, m.M22);
    }

    /// <summary>
    /// Multiplies a <see cref="M33d"/> with a <see cref="Rot2d"/> transformation (as a 3x3 matrix).
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static M33d operator *(M33d m, Rot2d r)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
            m.M00 * a + m.M01 * b, 
            m.M00 * -b + m.M01 * a,
            m.M02,

            m.M10 * a + m.M11 * b, 
            m.M10 * -b + m.M11 * a,
            m.M12,

            m.M20 * a + m.M21 * b, 
            m.M20 * -b + m.M21 * a,
            m.M22);
    }

    #endregion

    #region Rot / Shift, Scale Multiplication

    /// <summary>
    /// Multiplies a <see cref="Rot2d"/> transformation with a <see cref="Shift2d"/> transformation.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Euclidean2d operator *(Rot2d a, Shift2d b)
        => new(a, a * b.V);

    /// <summary>
    /// Multiplies a <see cref="Rot2d"/> transformation with a <see cref="Scale2d"/> transformation.
    /// Attention: Multiplication is NOT commutative!
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Affine2d operator *(Rot2d a, Scale2d b)
        => new((M22d)a * (M22d)b);

    #endregion

    #endregion

    #region Comparison Operators

    /// <summary>
    /// Checks if 2 rotations are equal.
    /// </summary>
    /// <returns>Result of comparison.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Rot2d rotation1, Rot2d rotation2)
        => Rot.Distance(rotation1, rotation2) == 0;

    /// <summary>
    /// Checks if 2 rotations are not equal.
    /// </summary>
    /// <returns>Result of comparison.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Rot2d rotation1, Rot2d rotation2)
        => Rot.Distance(rotation1, rotation2) != 0;

    #endregion

    #region Static Creators

    /// <summary>
    /// Creates a <see cref="Rot2d"/> transformation from a <see cref="M22d"/> matrix.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d FromM22d(M22d m)
        => new(m.GetRotation());

    /// <summary>
    /// Creates a <see cref="Rot2d"/> transformation from a <see cref="M33d"/> matrix.
    /// The matrix has to be homogeneous and must not contain perspective components.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d FromM33d(M33d m, double epsilon = 1e-12)
    {
        if (!(m.M20.IsTiny(epsilon) && m.M21.IsTiny(epsilon)))
            throw new ArgumentException("Matrix contains perspective components.");

        if (!m.C2.XY.ApproximateEquals(V2d.Zero, epsilon))
            throw new ArgumentException("Matrix contains translational component.");

        if (m.M22.IsTiny(epsilon))
            throw new ArgumentException("Matrix is not homogeneous.");

        return FromM22d(((M22d)m) / m.M22);
    }

    /// <summary>
    /// Creates a <see cref="Rot2d"/> transformation from a <see cref="Euclidean2d"/>.
    /// The transformation <paramref name="euclidean"/> must only consist of a rotation.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d FromEuclidean2d(Euclidean2d euclidean, double epsilon = 1e-12)
    {
        if (!euclidean.Trans.ApproximateEquals(V2d.Zero, epsilon))
            throw new ArgumentException("Euclidean transformation contains translational component");

        return euclidean.Rot;
    }

    /// <summary>
    /// Creates a <see cref="Rot2d"/> transformation from a <see cref="Similarity2d"/>.
    /// The transformation <paramref name="similarity"/> must only consist of a rotation.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d FromSimilarity2d(Similarity2d similarity, double epsilon = 1e-12)
    {
        if (!similarity.Scale.ApproximateEquals(1, epsilon))
            throw new ArgumentException("Similarity transformation contains scaling component");

        if (!similarity.Trans.ApproximateEquals(V2d.Zero, epsilon))
            throw new ArgumentException("Similarity transformation contains translational component");

        return similarity.Rot;
    }

    /// <summary>
    /// Creates a <see cref="Rot2d"/> transformation from an <see cref="Affine2d"/>.
    /// The transformation <paramref name="affine"/> must only consist of a rotation.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d FromAffine2d(Affine2d affine, double epsilon = 1e-12)
        => FromM33d((M33d)affine, epsilon);

    /// <summary>
    /// Creates a <see cref="Rot2d"/> transformation from a <see cref="Trafo2d"/>.
    /// The transformation <paramref name="trafo"/> must only consist of a rotation.
    /// </summary>
    /// <exception cref="ArgumentException"></exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d FromTrafo2d(Trafo2d trafo, double epsilon = 1e-12)
        => FromM33d(trafo.Forward, epsilon);

    /// <summary>
    /// Creates a <see cref="Rot2d"/> transformation with the specified angle in radians.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d FromRadians(double angleInRadians)
        => new(angleInRadians);

    /// <summary>
    /// Creates a <see cref="Rot2d"/> transformation with the specified angle in degrees.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d FromDegrees(double angleInDegrees)
        => new(angleInDegrees.RadiansFromDegrees());

    #endregion

    #region Conversion Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M22d(Rot2d r)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
             a, -b, 
             b,  a);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M23d(Rot2d r)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
             a, -b,  0, 
             b,  a,  0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M33d(Rot2d r)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
             a, -b,  0, 
             b,  a,  0, 
             0,  0,  1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M34d(Rot2d r)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
             a, -b,  0,  0, 
             b,  a,  0,  0, 
             0,  0,  1,  0);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator M44d(Rot2d r)
    {
        double a = Fun.Cos(r.Angle);
        double b = Fun.Sin(r.Angle);

        return new(
             a, -b,  0,  0, 
             b,  a,  0,  0, 
             0,  0,  1,  0, 
             0,  0,  0,  1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Euclidean2d(Rot2d r)
        => new(r, V2d.Zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Similarity2d(Rot2d r)
        => new(1, r, V2d.Zero);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Affine2d(Rot2d r)
        => new((M22d)r);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Trafo2d(Rot2d r)
        => new((M33d)r, (M33d)r.Inverse);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Rot2f(Rot2d r)
        => new((float)r.Angle);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
        => Angle.GetHashCode();

    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}]", Angle);

    public static Rot2d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new(
            double.Parse(x[0], CultureInfo.InvariantCulture)
        );
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Rot2d other)
        => Rot.Distance(this, other) == 0;

    public override readonly bool Equals(object other)
        => (other is Rot2d o) && Equals(o);

    #endregion
}

public static partial class Rot
{
    #region Invert

    /// <summary>
    /// Returns the inverse of a <see cref="Rot2d"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Rot2d Inverse(Rot2d rot)
        => rot.Inverse;

    /// <summary>
    /// Inverts a <see cref="Rot2d"/>.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Invert(this ref Rot2d rot)
    {
        rot.Angle = -rot.Angle;
    }

    #endregion

    #region Distance

    /// <summary>
    /// Returns the absolute difference in radians between two <see cref="Rot2d"/> rotations.
    /// The result is within the range of [0, Pi].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Distance(this Rot2d r1, Rot2d r2)
        => Fun.AngleDistance(r1.Angle, r2.Angle);

    /// <summary>
    /// Returns the signed difference in radians between two <see cref="Rot2d"/> rotations.
    /// The result is within the range of [-Pi, Pi).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static double Difference(this Rot2d r1, Rot2d r2)
        => Fun.AngleDifference(r1.Angle, r2.Angle);

    #endregion

    #region Transform

    /// <summary>
    /// Transforms a <see cref="V2d"/> vector by a <see cref="Rot2d"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2d Transform(this Rot2d rot, V2d v)
        => rot * v;

    /// <summary>
    /// Transforms a <see cref="V3d"/> vector by a <see cref="Rot2d"/> transformation.
    /// The z coordinate of the vector is unaffected.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3d Transform(this Rot2d rot, V3d v)
    {
        double a = Fun.Cos(rot.Angle);
        double b = Fun.Sin(rot.Angle);

        return new(a * v.X + -b * v.Y,
                    b * v.X + a * v.Y,
                    v.Z);
    }

    /// <summary>
    /// Transforms a <see cref="V4d"/> vector by a <see cref="Rot2d"/> transformation.
    /// The z and w coordinates of the vector are unaffected.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V4d Transform(this Rot2d rot, V4d v)
    {
        double a = Fun.Cos(rot.Angle);
        double b = Fun.Sin(rot.Angle);

        return new(a * v.X + -b * v.Y,
                    b * v.X + a * v.Y,
                    v.Z, v.W);
    }

    /// <summary>
    /// Transforms a <see cref="V2d"/> vector by the inverse of a <see cref="Rot2d"/> transformation.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V2d InvTransform(this Rot2d rot, V2d v)
    {
        double a = Fun.Cos(-rot.Angle);
        double b = Fun.Sin(-rot.Angle);

        return new(a * v.X + -b * v.Y, b * v.X + a * v.Y);
    }

    /// <summary>
    /// Transforms a <see cref="V3d"/> vector by the inverse of a <see cref="Rot2d"/> transformation.
    /// The z coordinate of the vector is unaffected.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V3d InvTransform(this Rot2d rot, V3d v)
        => Transform(rot.Inverse, v);

    /// <summary>
    /// Transforms a <see cref="V4d"/> vector by the inverse of a <see cref="Rot2d"/> transformation.
    /// The z and w coordinates of the vector are unaffected.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static V4d InvTransform(this Rot2d rot, V4d v)
        => Transform(rot.Inverse, v);

    #endregion
}

public static partial class Fun
{
    #region ApproximateEquals

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Rot2d r0, Rot2d r1)
        => ApproximateEquals(r0, r1, Constant<double>.PositiveTinyValue);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Rot2d r0, Rot2d r1, double tolerance)
        => Rot.Distance(r0, r1) <= tolerance;

    #endregion
}

#endregion
