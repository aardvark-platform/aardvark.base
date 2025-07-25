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
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Runtime.Serialization;

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

#region Torus3f

[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Torus3f : IEquatable<Torus3f>, IBoundingBox3f
{
    [DataMember]
    public V3f Position;
    [DataMember]
    public V3f Direction;
    [DataMember]
    public float MajorRadius;
    [DataMember]
    public float MinorRadius;

    #region Constructor

    public Torus3f(V3f position, V3f direction, float majorRadius, float minorRadius)
    {
        Position = position;
        Direction = direction;
        MinorRadius = minorRadius;
        MajorRadius = majorRadius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Torus3f(Torus3f o)
    {
        Position = o.Position;
        Direction = o.Direction;
        MinorRadius = o.MinorRadius;
        MajorRadius = o.MajorRadius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Torus3f(Torus3d o)
    {
        Position = (V3f)o.Position;
        Direction = (V3f)o.Direction;
        MinorRadius = (float)o.MinorRadius;
        MajorRadius = (float)o.MajorRadius;
    }

    #endregion

    #region Conversions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Torus3f(Torus3d o)
        => new Torus3f(o);

    #endregion

    #region Properties

    public readonly Circle3f MajorCircle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetMajorCircle();
    }

    public readonly float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 4 * ConstantF.PiSquared * MajorRadius * MinorRadius;
    }

    public readonly float Volume
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 2 * ConstantF.PiSquared * MajorRadius * MinorRadius * MinorRadius;
    }

    #endregion

    #region Operations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Circle3f GetMajorCircle() => new Circle3f(Position, Direction, MajorRadius);

    public readonly Circle3f GetMinorCircle(float angle)
    {
        var c = GetMajorCircle();
        var p = c.GetPoint(angle);
        var dir = (p - Position).Normalized.Cross(Direction).Normalized;
        return new Circle3f(p, dir, MinorRadius);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly float GetMinimalDistance(V3f p) => GetMinimalDistance(p, Position, Direction, MajorRadius, MinorRadius);

    public static float GetMinimalDistance(V3f p, V3f position, V3f direction, float majorRadius, float minorRadius)
    {
        var plane = new Plane3f(direction, position);
        var planePoint = p.GetClosestPointOn(plane);
        var distanceOnPlane = (Vec.Distance(planePoint, position) - majorRadius).Abs();
        var distanceToCircle = (Vec.DistanceSquared(planePoint, p) + distanceOnPlane.Square()).Sqrt();
        return (distanceToCircle - minorRadius).Abs();
    }

    #endregion

    #region Comparison operators

    /// <summary>
    /// Tests whether two specified spheres are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Torus3f a, Torus3f b) =>
        (a.Position == b.Position) &&
        (a.Direction == b.Direction) &&
        (a.MajorRadius == b.MajorRadius) &&
        (a.MinorRadius == b.MinorRadius);

    /// <summary>
    /// Tests whether two specified spheres are not equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Torus3f a, Torus3f b)
        => !(a == b);

    #endregion

    #region Overrides

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode() => HashCode.GetCombined(Position, Direction, MajorRadius, MinorRadius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Torus3f other) =>
        Position.Equals(other.Position) &&
        Direction.Equals(other.Direction) &&
        MajorRadius.Equals(other.MajorRadius) &&
        MinorRadius.Equals(other.MinorRadius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals(object other)
        => (other is Torus3f o) ? Equals(o) : false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", Position, Direction, MajorRadius, MinorRadius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Torus3f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Torus3f(
            V3f.Parse(x[0]),
            V3f.Parse(x[1]),
            float.Parse(x[2], CultureInfo.InvariantCulture),
            float.Parse(x[3], CultureInfo.InvariantCulture)
        );
    }

    #endregion

    #region IBoundingBox3f Members

    readonly Box3f IBoundingBox3f.BoundingBox3f
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetMajorCircle().BoundingBox3f.EnlargedBy(MinorRadius);
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Torus3f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Torus3f a, Torus3f b, float tolerance) =>
        ApproximateEquals(a.Position, b.Position, tolerance) &&
        ApproximateEquals(a.Direction, b.Direction, tolerance) &&
        ApproximateEquals(a.MajorRadius, b.MajorRadius, tolerance) &&
        ApproximateEquals(a.MinorRadius, b.MinorRadius, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Torus3f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Torus3f a, Torus3f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region Torus3d

[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Torus3d : IEquatable<Torus3d>, IBoundingBox3d
{
    [DataMember]
    public V3d Position;
    [DataMember]
    public V3d Direction;
    [DataMember]
    public double MajorRadius;
    [DataMember]
    public double MinorRadius;

    #region Constructor

    public Torus3d(V3d position, V3d direction, double majorRadius, double minorRadius)
    {
        Position = position;
        Direction = direction;
        MinorRadius = minorRadius;
        MajorRadius = majorRadius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Torus3d(Torus3d o)
    {
        Position = o.Position;
        Direction = o.Direction;
        MinorRadius = o.MinorRadius;
        MajorRadius = o.MajorRadius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Torus3d(Torus3f o)
    {
        Position = (V3d)o.Position;
        Direction = (V3d)o.Direction;
        MinorRadius = (double)o.MinorRadius;
        MajorRadius = (double)o.MajorRadius;
    }

    #endregion

    #region Conversions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Torus3d(Torus3f o)
        => new Torus3d(o);

    #endregion

    #region Properties

    public readonly Circle3d MajorCircle
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetMajorCircle();
    }

    public readonly double Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 4 * Constant.PiSquared * MajorRadius * MinorRadius;
    }

    public readonly double Volume
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => 2 * Constant.PiSquared * MajorRadius * MinorRadius * MinorRadius;
    }

    #endregion

    #region Operations

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly Circle3d GetMajorCircle() => new Circle3d(Position, Direction, MajorRadius);

    public readonly Circle3d GetMinorCircle(double angle)
    {
        var c = GetMajorCircle();
        var p = c.GetPoint(angle);
        var dir = (p - Position).Normalized.Cross(Direction).Normalized;
        return new Circle3d(p, dir, MinorRadius);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly double GetMinimalDistance(V3d p) => GetMinimalDistance(p, Position, Direction, MajorRadius, MinorRadius);

    public static double GetMinimalDistance(V3d p, V3d position, V3d direction, double majorRadius, double minorRadius)
    {
        var plane = new Plane3d(direction, position);
        var planePoint = p.GetClosestPointOn(plane);
        var distanceOnPlane = (Vec.Distance(planePoint, position) - majorRadius).Abs();
        var distanceToCircle = (Vec.DistanceSquared(planePoint, p) + distanceOnPlane.Square()).Sqrt();
        return (distanceToCircle - minorRadius).Abs();
    }

    #endregion

    #region Comparison operators

    /// <summary>
    /// Tests whether two specified spheres are equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Torus3d a, Torus3d b) =>
        (a.Position == b.Position) &&
        (a.Direction == b.Direction) &&
        (a.MajorRadius == b.MajorRadius) &&
        (a.MinorRadius == b.MinorRadius);

    /// <summary>
    /// Tests whether two specified spheres are not equal.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Torus3d a, Torus3d b)
        => !(a == b);

    #endregion

    #region Overrides

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode() => HashCode.GetCombined(Position, Direction, MajorRadius, MinorRadius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Torus3d other) =>
        Position.Equals(other.Position) &&
        Direction.Equals(other.Direction) &&
        MajorRadius.Equals(other.MajorRadius) &&
        MinorRadius.Equals(other.MinorRadius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals(object other)
        => (other is Torus3d o) ? Equals(o) : false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", Position, Direction, MajorRadius, MinorRadius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Torus3d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Torus3d(
            V3d.Parse(x[0]),
            V3d.Parse(x[1]),
            double.Parse(x[2], CultureInfo.InvariantCulture),
            double.Parse(x[3], CultureInfo.InvariantCulture)
        );
    }

    #endregion

    #region IBoundingBox3d Members

    readonly Box3d IBoundingBox3d.BoundingBox3d
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => GetMajorCircle().BoundingBox3d.EnlargedBy(MinorRadius);
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Torus3d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Torus3d a, Torus3d b, double tolerance) =>
        ApproximateEquals(a.Position, b.Position, tolerance) &&
        ApproximateEquals(a.Direction, b.Direction, tolerance) &&
        ApproximateEquals(a.MajorRadius, b.MajorRadius, tolerance) &&
        ApproximateEquals(a.MinorRadius, b.MinorRadius, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Torus3d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Torus3d a, Torus3d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

