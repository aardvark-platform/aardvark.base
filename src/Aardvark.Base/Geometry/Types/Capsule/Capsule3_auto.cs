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
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

#region Capsule3f

[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Capsule3f : IEquatable<Capsule3f>, IValidity, IBoundingBox3f
{
    [DataMember]
    public V3f P0;
    [DataMember]
    public V3f P1;
    [DataMember]
    public float Radius;

    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Capsule3f(V3f p0, V3f p1, float radius)
    {
        P0 = p0;
        P1 = p1;
        Radius = radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Capsule3f(Line3f axis, float radius)
    {
        P0 = axis.P0;
        P1 = axis.P1;
        Radius = radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Capsule3f(Capsule3f capsule)
    {
        P0 = capsule.P0;
        P1 = capsule.P1;
        Radius = capsule.Radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Capsule3f(Capsule3d capsule)
    {
        P0 = (V3f)capsule.P0;
        P1 = (V3f)capsule.P1;
        Radius = (float)capsule.Radius;
    }

    #endregion

    #region Conversions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Capsule3f(Capsule3d c) => new(c);

    #endregion

    #region Constants

    public static readonly Capsule3f Invalid = new(V3f.NaN, V3f.NaN, -1);

    #endregion

    #region Properties

    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Radius >= 0.0;
    }

    public readonly bool IsInvalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Radius < 0.0;
    }

    public readonly Line3f Axis
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(P0, P1);
    }

    public readonly Cylinder3f Cylider
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(P0, P1, Radius);
    }

    #endregion

    #region IBoundingBox3f Members

    public readonly Box3f BoundingBox3f
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(new Sphere3f(P0, Radius).BoundingBox3f, new Sphere3f(P1, Radius).BoundingBox3f);
    }

    #endregion

    #region Overrides

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode()
        => HashCode.GetCombined(P0, P1, Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Capsule3f other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && Radius.Equals(other.Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals(object other)
        => (other is Capsule3f o) && Equals(o);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Capsule3f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Capsule3f(V3f.Parse(x[0]), V3f.Parse(x[1]), float.Parse(x[2], CultureInfo.InvariantCulture));
    }

    #endregion

}

#endregion

#region Capsule3d

[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct Capsule3d : IEquatable<Capsule3d>, IValidity, IBoundingBox3d
{
    [DataMember]
    public V3d P0;
    [DataMember]
    public V3d P1;
    [DataMember]
    public double Radius;

    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Capsule3d(V3d p0, V3d p1, double radius)
    {
        P0 = p0;
        P1 = p1;
        Radius = radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Capsule3d(Line3d axis, double radius)
    {
        P0 = axis.P0;
        P1 = axis.P1;
        Radius = radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Capsule3d(Capsule3d capsule)
    {
        P0 = capsule.P0;
        P1 = capsule.P1;
        Radius = capsule.Radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Capsule3d(Capsule3f capsule)
    {
        P0 = (V3d)capsule.P0;
        P1 = (V3d)capsule.P1;
        Radius = (double)capsule.Radius;
    }

    #endregion

    #region Conversions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Capsule3d(Capsule3f c) => new(c);

    #endregion

    #region Constants

    public static readonly Capsule3d Invalid = new(V3d.NaN, V3d.NaN, -1);

    #endregion

    #region Properties

    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Radius >= 0.0;
    }

    public readonly bool IsInvalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Radius < 0.0;
    }

    public readonly Line3d Axis
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(P0, P1);
    }

    public readonly Cylinder3d Cylider
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(P0, P1, Radius);
    }

    #endregion

    #region IBoundingBox3d Members

    public readonly Box3d BoundingBox3d
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(new Sphere3d(P0, Radius).BoundingBox3d, new Sphere3d(P1, Radius).BoundingBox3d);
    }

    #endregion

    #region Overrides

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode()
        => HashCode.GetCombined(P0, P1, Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Capsule3d other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && Radius.Equals(other.Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals(object other)
        => (other is Capsule3d o) && Equals(o);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Capsule3d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Capsule3d(V3d.Parse(x[0]), V3d.Parse(x[1]), double.Parse(x[2], CultureInfo.InvariantCulture));
    }

    #endregion

}

#endregion

