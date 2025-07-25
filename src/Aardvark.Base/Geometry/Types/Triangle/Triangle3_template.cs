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
using System.Runtime.CompilerServices;

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

//# foreach (var isDouble in new[] { false, true }) {
//#   var ftype = isDouble ? "double" : "float";
//#   var ftype2 = isDouble ? "float" : "double";
//#   var tc = isDouble ? "d" : "f";
//#   var tc2 = isDouble ? "f" : "d";
//#   var type = "Triangle3" + tc;
//#   var type2 = "Triangle3" + tc2;
//#   var v3t = "V3" + tc;
//#   var box3t = "Box3" + tc;
//#   var plane3t = "Plane3" + tc;
//#   var sphere3t = "Sphere3" + tc;
//#   var iboundingsphere3t = "IBoundingSphere3" + tc;
//#   var half = isDouble ? "0.5" : "0.5f";
//#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
//#   var eps = isDouble ? "1e-9" : "1e-5f";
#region __type__

/// <summary>
/// A three-dimensional triangle represented by its three points.
/// </summary>
public partial struct __type__ : __iboundingsphere3t__
{
    #region Geometric Properties

    /// <summary>
    /// Returns the area of the triangle.
    /// </summary>
    public readonly __ftype__ Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Triangle.Area(this);
    }

    /// <summary>
    /// Returns whether the triangle is degenerated, i.e. its area is zero.
    /// </summary>
    public readonly bool IsDegenerated
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Triangle.IsDegenerated(this);
    }

    /// <summary>
    /// Returns the normal of the triangle.
    /// </summary>
    public readonly __v3t__ Normal
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Triangle.Normal(this);
    }

    /// <summary>
    /// Returns the plane that contains the points of the triangle.
    /// </summary>
    public readonly __plane3t__ Plane
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Triangle.Plane(this);
    }

    #endregion

    #region __iboundingsphere3t__ Members

    /// <summary>
    /// Returns the bounding sphere of the triangle.
    /// </summary>
    public readonly __sphere3t__ BoundingSphere3__tc__
    {
        get
        {
            var edge01 = Edge01;
            var edge02 = Edge02;
            __ftype__ dot0101 = Vec.Dot(edge01, edge01);
            __ftype__ dot0102 = Vec.Dot(edge01, edge02);
            __ftype__ dot0202 = Vec.Dot(edge02, edge02);
            __ftype__ d = 2 * (dot0101 * dot0202 - dot0102 * dot0102);
            if (d.Abs() <= __eps__) return __sphere3t__.Invalid;
            __ftype__ s = (dot0101 * dot0202 - dot0202 * dot0102) / d;
            __ftype__ t = (dot0202 * dot0101 - dot0101 * dot0102) / d;
            var p = P0;
            var sph = new __sphere3t__();
            if (s <= 0)
                sph.Center = __half__ * (P0 + P2);
            else if (t <= 0)
                sph.Center = __half__ * (P0 + P1);
            else if (s + t >= 1)
            {
                sph.Center = __half__ * (P1 + P2);
                p = P1;
            }
            else
                sph.Center = P0 + s * edge01 + t * edge02;
            sph.Radius = (sph.Center - p).Length;
            return sph;
        }
    }

    #endregion
}

/// <summary>
/// Contains static methods for triangles.
/// </summary>
public static partial class Triangle
{
    #region Area

    /// <summary>
    /// Returns the area of the triangle defined by the given points.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __ftype__ Area(__v3t__ p0, __v3t__ p1, __v3t__ p2)
        => (p1 - p0).Cross(p2 - p0).Length * __half__;

    /// <summary>
    /// Returns the area of the given triangle.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __ftype__ Area(__type__ t)
        => Area(t.P0, t.P1, t.P2);

    #endregion

    #region IsDegenerated

    /// <summary>
    /// Returns whether the triangle defined by the give points is degenerated, i.e. its area is zero.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDegenerated(__v3t__ p0, __v3t__ p1, __v3t__ p2)
        => (p1 - p0).Cross(p2 - p0).AllTiny;

    /// <summary>
    /// Returns whether the given triangle is degenerated, i.e. its area is zero.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDegenerated(__type__ t)
        => IsDegenerated(t.P0, t.P1, t.P2);

    #endregion

    #region Normal

    /// <summary>
    /// Returns the normal of the triangle defined by the given points.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __v3t__ Normal(__v3t__ p0, __v3t__ p1, __v3t__ p2)
        => (p1 - p0).Cross(p2 - p0).Normalized;

    /// <summary>
    /// Returns the normal of the given triangle.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __v3t__ Normal(__type__ t)
        => Normal(t.P0, t.P1, t.P2);

    #endregion

    #region Plane

    /// <summary>
    /// Returns the plane that contains the given points.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __plane3t__ Plane(__v3t__ p0, __v3t__ p1, __v3t__ p2)
        => new(Normal(p0, p1, p2), p0);

    /// <summary>
    /// Returns the plane that contains the points of the given triangle.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __plane3t__ Plane(__type__ t)
        => Plane(t.P0, t.P1, t.P2);

    #endregion

    #region SolidAngle

    /// <summary>
    /// Computes the solid angle for a planar triangle as seen from the origin.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __ftype__ SolidAngle(__type__ t)
        => SolidAngle(t.P0, t.P1, t.P2);

    /// <summary>
    /// Computes the solid angle for a planar triangle as seen from the origin.
    ///
    /// Van Oosterom, A., and Strackee, J. (1983). 
    /// The solid angle of a plane triangle.
    /// IEEE transactions on Biomedical Engineering, (2), 125-126.
    /// 
    /// https://en.wikipedia.org/wiki/Solid_angle#Tetrahedron
    /// </summary>
    public static __ftype__ SolidAngle(__v3t__ va, __v3t__ vb, __v3t__ vc)
    {
        var ma = va.Length;
        var mb = vb.Length;
        var mc = vc.Length;

        var numerator = Fun.Abs(Vec.Dot(va, (Vec.Cross(vb, vc))));

        var denom = ma * mb * mc + Vec.Dot(va, vb) * mc + Vec.Dot(va, vc) * mb + Vec.Dot(vb, vc) * ma;

        var halfSA = Fun.Atan2(numerator, denom);

        return 2 * (halfSA >= 0 ? halfSA : halfSA + __pi__);
    }

    /// <summary>
    /// Computes the solid angle for a planar triangle as seen from the given point.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __ftype__ SolidAngle(__v3t__ va, __v3t__ vb, __v3t__ vc, __v3t__ p)
        => SolidAngle(va - p, vb - p, vc - p);

    /// <summary>
    /// Computes the solid angle for a planar triangle as seen from the given point.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __ftype__ SolidAngle(__type__ t, __v3t__ p)
        => SolidAngle(t.P0 - p, t.P1 - p, t.P2 - p);

    #endregion

    #region Distance

    /// <summary>
    /// Gets the distance between the closest point on the triangle [a, b, c] and the given query point.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static __ftype__ Distance(__v3t__ a, __v3t__ b, __v3t__ c, __v3t__ query)
    {
        var cp = query.GetClosestPointOnTriangle(a, b, c);
        return (cp - query).Length;
    }

    #endregion
}

#endregion

//# }
