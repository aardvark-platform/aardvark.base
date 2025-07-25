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
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

//# foreach (var isDouble in new[] { false, true }) {
//#   var ftype = isDouble ? "double" : "float";
//#   var ftype2 = isDouble ? "float" : "double";
//#   var tc = isDouble ? "d" : "f";
//#   var tc2 = isDouble ? "f" : "d";
//#   var ray2t = "Ray2" + tc;
//#   var ray2t2 = "Ray2" + tc2;
//#   var fastray2t = "FastRay2" + tc;
//#   var fastray2t2 = "FastRay2" + tc2;
//#   var v2t = "V2" + tc;
//#   var range1t = "Range1" + tc;
//#   var box2t = "Box2" + tc;
//#   var plane2t = "Plane2" + tc;
//#   var line2t = "Line2" + tc;
//#   var iboundingbox = "IBoundingBox2" + tc;
//#   var half = isDouble ? "0.5" : "0.5f";
#region __ray2t__

/// <summary>
/// A two-dimensional ray with an origin and a direction.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public partial struct __ray2t__ : IEquatable<__ray2t__>, IValidity, __iboundingbox__
{
    [DataMember]
    public __v2t__ Origin;
    [DataMember]
    public __v2t__ Direction;

    #region Constructors

    /// <summary>
    /// Creates Ray from origin point and directional vector
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public __ray2t__(__v2t__ origin, __v2t__ direction)
    {
        Origin = origin;
        Direction = direction;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public __ray2t__(__ray2t__ o)
    {
        Origin = o.Origin;
        Direction = o.Direction;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public __ray2t__(__ray2t2__ o)
    {
        Origin = (__v2t__)o.Origin;
        Direction = (__v2t__)o.Direction;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __ray2t__ FromEndPoints(__v2t__ origin, __v2t__ target)
        => new __ray2t__(origin, target - origin);

    #endregion

    #region Conversions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator __ray2t__(__ray2t2__ c)
        => new __ray2t__(c);

    #endregion

    #region Constants

    /// <summary>
    /// An invalid ray has a zero direction.
    /// </summary>
    public static __ray2t__ Invalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new __ray2t__(__v2t__.NaN, __v2t__.Zero);
    }

    #endregion

    #region Properties

    /// <summary>
    /// A ray is valid if its direction is non-zero.
    /// </summary>
    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Direction != __v2t__.Zero;
    }

    /// <summary>
    /// A ray is invalid if its direction is zero.
    /// </summary>
    public readonly bool IsInvalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Direction == __v2t__.Zero;
    }

    /// <summary>
    /// Returns true if either the origin or the direction contains any NaN value.
    /// </summary>
    public readonly bool AnyNaN
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Origin.AnyNaN || Direction.AnyNaN;
    }

    public readonly __line2t__ __line2t__
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new __line2t__(Origin, Origin + Direction);
    }

    public readonly __plane2t__ __plane2t__
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new __plane2t__(new __v2t__(-Direction.Y, Direction.X), Origin); // Direction.Rot90
    }

    public readonly __ray2t__ Reversed
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new __ray2t__(Origin, -Direction);
    }

    /// <summary>
    /// Returns the ray with its directional normalized.
    /// </summary>
    public readonly __ray2t__ Normalized => new(Origin, Direction.Normalized);

    #endregion

    #region Ray Arithmetics

    /// <summary>
    /// Gets the point on the ray that is t * direction from origin.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __v2t__ GetPointOnRay(__ftype__ t) => (Origin + Direction * t);

    /// <summary>
    /// Gets segment on the ray starting at range.Min * direction from origin
    /// and ending at range.Max * direction from origin.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __line2t__ Get__line2t__OnRay(__range1t__ range)
        => new __line2t__(Origin + Direction * range.Min, Origin + Direction * range.Max);

    /// <summary>
    /// Gets segment on the ray starting at tMin * direction from origin
    /// and ending at tMax * direction from origin.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __line2t__ Get__line2t__OnRay(__ftype__ tMin, __ftype__ tMax)
        => new __line2t__(Origin + Direction * tMin, Origin + Direction * tMax);

    /// <summary>
    /// Gets the t for a point p on this ray.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __ftype__ GetT(__v2t__ p)
    {
        var v = p - Origin;
        return (Direction.X.Abs() > Direction.Y.Abs())
            ? (v.X / Direction.X)
            : (v.Y / Direction.Y);
    }

    /// <summary>
    /// Gets the point on the ray that is closest to the given point.
    /// Ray direction must be normalized (length 1).
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __v2t__ GetClosestPointOnRay(__v2t__ p)
        => Origin + Direction * Direction.Dot(p - Origin);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __ftype__ GetDistanceToRay(__v2t__ p)
    {
        var f = GetClosestPointOnRay(p);
        return (f - p).Length;
    }

    public readonly __v2t__ Intersect(__ray2t__ r)
    {
        __v2t__ a = r.Origin - Origin;
        if (a.Abs().AllSmaller(Constant<__ftype__>.PositiveTinyValue))
            return Origin; // Early exit when rays have same origin

        __ftype__ cross = Direction.Dot270(r.Direction);
        if (!Fun.IsTiny(cross)) // Rays not parallel
            return Origin + Direction * r.Direction.Dot90(a) / cross;
        else // Rays are parallel
            return __v2t__.NaN;
    }

    public readonly __v2t__ Intersect(__v2t__ dirVector)
    {
        if (Origin.Abs().AllSmaller(Constant<__ftype__>.PositiveTinyValue))
            return Origin; // Early exit when rays have same origin

        __ftype__ cross = Direction.Dot270(dirVector);
        if (!Fun.IsTiny(cross)) // Rays not parallel
            return Origin + Direction * dirVector.Dot270(Origin) / cross;
        else // Rays are parallel
            return __v2t__.NaN;
    }

    /// <summary>
    /// Returns the angle between this and the given <see cref="__ray2t__"/> in radians.
    /// The direction vectors of the input rays have to be normalized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __ftype__ AngleBetweenFast(__ray2t__ r)
        => Direction.AngleBetweenFast(r.Direction);

    /// <summary>
    /// Returns the angle between this and the given <see cref="__ray2t__"/> in radians using a numerically stable algorithm.
    /// The direction vectors of the input rays have to be normalized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __ftype__ AngleBetween(__ray2t__ r)
        => Direction.AngleBetween(r.Direction);

    /// <summary>
    /// Returns the signed angle between this and the given <see cref="__ray2t__"/> in radians.
    /// The direction vectors of the input rays have to be normalized.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __ftype__ AngleBetweenSigned(__ray2t__ r)
        => Direction.AngleBetweenSigned(r.Direction);

    /// <summary>
    /// Returns a signed value where left is negative and right positive.
    /// The magnitude is equal to the __ftype__ size of the triangle the ray + direction and p.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly __ftype__ GetPointSide(__v2t__ p) => Direction.Dot90(p - Origin);

    #endregion

    #region Comparison Operators

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(__ray2t__ a, __ray2t__ b)
        => (a.Origin == b.Origin) && (a.Direction == b.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(__ray2t__ a, __ray2t__ b)
        => !((a.Origin == b.Origin) && (a.Direction == b.Direction));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly int LexicalCompare(__ray2t__ other)
    {
        var cmp = Origin.LexicalCompare(other.Origin);
        if (cmp != 0) return cmp;
        return Direction.LexicalCompare(other.Direction);
    }

    #endregion

    #region Overrides

    /// <summary>
    /// Calculates Hash-code of the given ray.
    /// </summary>
    /// <returns>Hash-code.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly int GetHashCode() => HashCode.GetCombined(Origin, Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(__ray2t__ other)
        => Origin.Equals(other.Origin) && Direction.Equals(other.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly bool Equals(object other)
        => (other is __ray2t__ o) ? Equals(o) : false;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public override readonly string ToString()
        => string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", Origin, Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __ray2t__ Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new __ray2t__(__v2t__.Parse(x[0]), __v2t__.Parse(x[1]));
    }

    #endregion

    #region __iboundingbox__

    public readonly __box2t__ BoundingBox2__tc__
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => __box2t__.FromPoints(Origin, Direction + Origin);
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="__ray2t__"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this __ray2t__ a, __ray2t__ b, __ftype__ tolerance) =>
        ApproximateEquals(a.Origin, b.Origin, tolerance) &&
        ApproximateEquals(a.Direction, b.Direction, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="__ray2t__"/> are equal within
    /// Constant&lt;__ftype__&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this __ray2t__ a, __ray2t__ b)
        => ApproximateEquals(a, b, Constant<__ftype__>.PositiveTinyValue);
}

#endregion

#region __fastray2t__

/// <summary>
/// A fast ray contains a ray and a number of precomputed flags and
/// fields for fast intersection computation with bounding boxes and
/// other axis-aligned sturctures such as kd-Trees.
/// </summary>
[DataContract]
[StructLayout(LayoutKind.Sequential)]
public readonly partial struct Fast__ray2t__
{
    [DataMember]
    public readonly __ray2t__ Ray;
    [DataMember]
    public readonly DirFlags DirFlags;
    [DataMember]
    public readonly __v2t__ InvDir;

    #region Constructors

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Fast__ray2t__(__ray2t__ ray)
    {
        Ray = ray;
        DirFlags = ray.Direction.DirFlags();
        InvDir = 1 / ray.Direction;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Fast__ray2t__(__v2t__ origin, __v2t__ direction)
        : this(new __ray2t__(origin, direction))
    { }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Fast__ray2t__(Fast__ray2t__ o)
    {
        Ray = o.Ray;
        DirFlags = o.DirFlags;
        InvDir = o.InvDir;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public Fast__ray2t__(Fast__ray2t2__ o)
    {
        Ray = (__ray2t__)o.Ray;
        DirFlags = o.DirFlags;
        InvDir = (__v2t__)o.InvDir;
    }

    #endregion

    #region Conversions

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static explicit operator Fast__ray2t__(Fast__ray2t2__ r)
        => new Fast__ray2t__(r);

    #endregion

    #region Ray Arithmetics

    public bool Intersects(
        __box2t__ box,
        ref __ftype__ tmin,
        ref __ftype__ tmax
    )
    {
        var dirFlags = DirFlags;

        if ((dirFlags & DirFlags.PositiveX) != 0)
        {
            {
                __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            {
                __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeX) != 0)
        {
            {
                __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            {
                __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else    // ray parallel to X-plane
        {
            if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                return false;
        }

        if ((dirFlags & DirFlags.PositiveY) != 0)
        {
            {
                __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            {
                __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else if ((dirFlags & DirFlags.NegativeY) != 0)
        {
            {
                __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) tmax = t;
            }
            {
                __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                if (t >= tmax) return false;
                if (t > tmin) tmin = t;
            }
        }
        else    // ray parallel to Y-plane
        {
            if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                return false;
        }

        if (tmin > tmax) return false;

        return true;
    }

    /// <summary>
    /// This variant of the intersection method returns the affected
    /// planes of the box if the box was hit.
    /// </summary>
    public bool Intersects(
        __box2t__ box,
        ref __ftype__ tmin,
        ref __ftype__ tmax,
        out Box.Flags tminFlags,
        out Box.Flags tmaxFlags
    )
    {
        var dirFlags = DirFlags;
        tminFlags = Box.Flags.None;
        tmaxFlags = Box.Flags.None;

        if ((dirFlags & DirFlags.PositiveX) != 0)
        {
            {
                __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxX; }
            }
            {
                __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinX; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeX) != 0)
        {
            {
                __ftype__ t = (box.Min.X - Ray.Origin.X) * InvDir.X;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinX; }
            }
            {
                __ftype__ t = (box.Max.X - Ray.Origin.X) * InvDir.X;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxX; }
            }
        }
        else    // ray parallel to X-plane
        {
            if (Ray.Origin.X < box.Min.X || Ray.Origin.X > box.Max.X)
                return false;
        }

        if ((dirFlags & DirFlags.PositiveY) != 0)
        {
            {
                __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MaxY; }
            }
            {
                __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MinY; }
            }
        }
        else if ((dirFlags & DirFlags.NegativeY) != 0)
        {
            {
                __ftype__ t = (box.Min.Y - Ray.Origin.Y) * InvDir.Y;
                if (t <= tmin) return false;
                if (t < tmax) { tmax = t; tmaxFlags = Box.Flags.MinY; }
            }
            {
                __ftype__ t = (box.Max.Y - Ray.Origin.Y) * InvDir.Y;
                if (t >= tmax) return false;
                if (t > tmin) { tmin = t; tminFlags = Box.Flags.MaxY; }
            }
        }
        else    // ray parallel to Y-plane
        {
            if (Ray.Origin.Y < box.Min.Y || Ray.Origin.Y > box.Max.Y)
                return false;
        }

        if (tmin > tmax) return false;

        return true;
    }

    #endregion
}

#endregion

//# }
