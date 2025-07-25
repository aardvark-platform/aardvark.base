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

//# foreach (var isDouble in new[] { false, true }) {
//#   var ftype = isDouble ? "double" : "float";
//#   var ftype2 = isDouble ? "float" : "double";
//#   var tc = isDouble ? "d" : "f";
//#   var tc2 = isDouble ? "f" : "d";
//#   var type = "Capsule3" + tc;
//#   var type2 = "Capsule3" + tc2;
//#   var v3t = "V3" + tc;
//#   var line3t = "Line3" + tc;
//#   var cylinder3t = "Cylinder3" + tc;
//#   var box3t = "Box3" + tc;
//#   var sphere3t = "Sphere3" + tc;
//#   var iboundingbox = "IBoundingBox3" + tc;
//#   var half = isDouble ? "0.5" : "0.5f";
//#   var fourbythree = isDouble ? "(4.0 / 3.0)" : "(4.0f / 3.0f)";
//#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
#region __type__

[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct __type__ : IEquatable<__type__>, IValidity, __iboundingbox__
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
    public __type__(__type__ capsule)
    {
        P0 = capsule.P0;
        P1 = capsule.P1;
        Radius = capsule.Radius;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public __type__(__type2__ capsule)
    {
        P0 = (__v3t__)capsule.P0;
        P1 = (__v3t__)capsule.P1;
        Radius = (__ftype__)capsule.Radius;
    }

    #endregion

    #region Conversions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator __type__(__type2__ c) => new(c);

    #endregion

    #region Constants

    public static readonly __type__ Invalid = new(__v3t__.NaN, __v3t__.NaN, -1);

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

    public readonly __line3t__ Axis
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(P0, P1);
    }

    public readonly __cylinder3t__ Cylider
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(P0, P1, Radius);
    }

    #endregion

    #region __iboundingbox__ Members

    public readonly __box3t__ BoundingBox3__tc__
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(new __sphere3t__(P0, Radius).BoundingBox3__tc__, new __sphere3t__(P1, Radius).BoundingBox3__tc__);
    }

    #endregion

    #region Overrides

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode()
        => HashCode.GetCombined(P0, P1, Radius);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(__type__ other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && Radius.Equals(other.Radius);

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
        return new __type__(__v3t__.Parse(x[0]), __v3t__.Parse(x[1]), __ftype__.Parse(x[2], CultureInfo.InvariantCulture));
    }

    #endregion

}

#endregion

//# }
