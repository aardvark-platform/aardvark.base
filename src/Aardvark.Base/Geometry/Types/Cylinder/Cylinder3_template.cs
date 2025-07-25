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
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

//# foreach (var isDouble in new[] { false, true }) {
//#   var ftype = isDouble ? "double" : "float";
//#   var ftype2 = isDouble ? "float" : "double";
//#   var tc = isDouble ? "d" : "f";
//#   var tc2 = isDouble ? "f" : "d";
//#   var type = "Cylinder3" + tc;
//#   var type2 = "Cylinder3" + tc2;
//#   var v3t = "V3" + tc;
//#   var circle3t = "Circle3" + tc;
//#   var box3t = "Box3" + tc;
//#   var line3t = "Line3" + tc;
//#   var ray3t = "Ray3" + tc;
//#   var iboundingbox = "IBoundingBox3" + tc;
//#   var half = isDouble ? "0.5" : "0.5f";
//#   var dotnine = isDouble ? "0.9" : "0.9f";
//#   var constant = isDouble ? "Constant" : "ConstantF";
#region __type__

[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct __type__ : IEquatable<__type__>, __iboundingbox__, IValidity
{
    [DataMember]
    public __v3t__ P0;
    [DataMember]
    public __v3t__ P1;
    [DataMember]
    public __ftype__ Radius;

    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public __type__(__v3t__ p0, __v3t__ p1, __ftype__ radius)
    {
        P0 = p0;
        P1 = p1;
        Radius = radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public __type__(__line3t__ axis, __ftype__ radius)
    {
        P0 = axis.P0;
        P1 = axis.P1;
        Radius = radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public __type__(__type__ c)
    {
        P0 = c.P0;
        P1 = c.P1;
        Radius = c.Radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public __type__(__type2__ c)
    {
        P0 = (__v3t__)c.P0;
        P1 = (__v3t__)c.P1;
        Radius = (__ftype__)c.Radius;
    }

    #endregion

    #region Conversions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator __type__(__type2__ c) => new(c);

    #endregion

    #region Constants

    public static __type__ Invalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(__v3t__.NaN, __v3t__.NaN, -1);
    }

    #endregion

    #region Properties

    public readonly __ftype__ Height
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (P0 - P1).Length;
    }

    public readonly __v3t__ Center
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (P0 + P1) * __half__;
    }

    public readonly __line3t__ Axis
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(P0, P1);
    }

    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Radius >= 0;
    }

    public readonly bool IsInvalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Radius < 0;
    }

    public readonly __circle3t__ Circle0
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(P0, (P0 - P1).Normalized, Radius);
    }

    public readonly __circle3t__ Circle1
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(P1, (P1 - P0).Normalized, Radius);
    }

    public readonly __ftype__ Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Radius * __constant__.PiTimesTwo * (Radius + Height);
    }

    public readonly __ftype__ Volume
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Radius * Radius * __constant__.Pi * Height;
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(__type__ a, __type__ b)
        => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.Radius == b.Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(__type__ a, __type__ b)
        => !(a == b);

    #endregion

    #region Overrides

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode() => HashCode.GetCombined(P0, P1, Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(__type__ other) =>
        P0.Equals(other.P0) &&
        P1.Equals(other.P1) &&
        Radius.Equals(other.Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals(object other)
        => (other is __type__ o) && Equals(o);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __type__ Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new __type__(
            __v3t__.Parse(x[0]),
            __v3t__.Parse(x[1]),
            __ftype__.Parse(x[2], CultureInfo.InvariantCulture)
        );
    }

    #endregion

    #region Operations

    /// <summary>
    /// P0 has height 0.0, P1 has height 1.0
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __ftype__ GetHeight(__v3t__ p)
    {
        var dir = (P1 - P0).Normalized;
        var pp = p.GetClosestPointOn(new __ray3t__(P0, dir));
        return (pp - P0).Dot(dir);
    }
    /// <summary>
    /// Get circle at a specific height
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __circle3t__ GetCircle(__ftype__ height)
    {
        var dir = (P1 - P0).Normalized;
        return new __circle3t__(P0 + height * dir, dir, Radius);
    }

    #endregion

    #region __iboundingbox__ Members

    public readonly __box3t__ BoundingBox3__tc__
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(Circle0.BoundingBox3__tc__, Circle1.BoundingBox3__tc__);
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="__type__"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this __type__ a, __type__ b, __ftype__ tolerance) =>
        ApproximateEquals(a.P0, b.P0, tolerance) &&
        ApproximateEquals(a.P1, b.P1, tolerance) &&
        ApproximateEquals(a.Radius, b.Radius, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="__type__"/> are equal within
    /// __constant__&lt;__ftype__&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this __type__ a, __type__ b)
        => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
}

#endregion

//# }
