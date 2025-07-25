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

/// <summary>
/// Provides various methods determining parallelism
/// </summary>
public static class Parallelism
{
    // 2-Dimensional

    #region V2f - V2f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this V2f u, V2f v)
        => Fun.IsTiny(u.X * v.Y - u.Y * v.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this V2f u, V2f v, float epsilon = 1e-4f)
    {
        var un = u.Normalized;
        var vn = v.Normalized;
        return (un - vn).Length < epsilon || (un + vn).Length < epsilon;
    }

    #endregion

    #region Ray2f - V2f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray2f ray, V2f v)
        => ray.Direction.IsParallelTo(v);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray2f ray, V2f v, float epsilon = 1e-4f)
        => ray.Direction.IsParallelTo(v, epsilon);

    #endregion

    #region Ray2f - Ray2f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray2f r0, Ray2f r1)
        => r0.Direction.IsParallelTo(r1.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray2f r0, Ray2f r1, float epsilon = 1e-4f)
        => r0.Direction.IsParallelTo(r1.Direction, epsilon);

    #endregion

    #region Line2f - Line2f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Line2f l0, Line2f l1)
        => l0.Direction.IsParallelTo(l1.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Line2f l0, Line2f l1, float epsilon = 1e-4f)
        => l0.Direction.IsParallelTo(l1.Direction, epsilon);

    #endregion

    // 3-Dimensional

    #region V3f - V3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this V3f u, V3f v)
        => Fun.IsTiny(u.Cross(v).Norm1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this V3f u, V3f v, float epsilon = 1e-4f)
    {
        var un = u.Normalized;
        var vn = v.Normalized;

        return (un - vn).Length < epsilon || (un + vn).Length < epsilon;
    }

    #endregion

    #region Ray3f - V3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3f ray, V3f vec)
        => ray.Direction.IsParallelTo(vec);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3f ray, V3f vec, float epsilon = 1e-4f)
        => ray.Direction.IsParallelTo(vec, epsilon);

    #endregion

    #region Ray3f - Ray3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3f r0, Ray3f r1)
        => r0.Direction.IsParallelTo(r1.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3f r0, Ray3f r1, float epsilon = 1e-4f)
        => r0.Direction.IsParallelTo(r1.Direction, epsilon);

    #endregion

    #region Plane3f - Plane3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Plane3f p0, Plane3f p1)
        => p0.Normal.IsParallelTo(p1.Normal);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Plane3f p0, Plane3f p1, float epsilon = 1e-4f)
        => p0.Normal.IsParallelTo(p1.Normal, epsilon);

    #endregion

    #region Ray3f - Plane3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3f ray, Plane3f plane)
        => ray.Direction.IsOrthogonalTo(plane.Normal);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Plane3f plane, Ray3f ray)
        => ray.Direction.IsOrthogonalTo(plane.Normal);

    #endregion

    #region Line3f - Line3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Line3f l0, Line3f l1)
        => l0.Direction.IsParallelTo(l1.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Line3f l0, Line3f l1, float epsilon = 1e-4f)
        => l0.Direction.IsParallelTo(l1.Direction, epsilon);

    #endregion

    // 2-Dimensional

    #region V2d - V2d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this V2d u, V2d v)
        => Fun.IsTiny(u.X * v.Y - u.Y * v.X);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this V2d u, V2d v, double epsilon = 1e-6)
    {
        var un = u.Normalized;
        var vn = v.Normalized;
        return (un - vn).Length < epsilon || (un + vn).Length < epsilon;
    }

    #endregion

    #region Ray2d - V2d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray2d ray, V2d v)
        => ray.Direction.IsParallelTo(v);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray2d ray, V2d v, double epsilon = 1e-6)
        => ray.Direction.IsParallelTo(v, epsilon);

    #endregion

    #region Ray2d - Ray2d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray2d r0, Ray2d r1)
        => r0.Direction.IsParallelTo(r1.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray2d r0, Ray2d r1, double epsilon = 1e-6)
        => r0.Direction.IsParallelTo(r1.Direction, epsilon);

    #endregion

    #region Line2d - Line2d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Line2d l0, Line2d l1)
        => l0.Direction.IsParallelTo(l1.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Line2d l0, Line2d l1, double epsilon = 1e-6)
        => l0.Direction.IsParallelTo(l1.Direction, epsilon);

    #endregion

    // 3-Dimensional

    #region V3d - V3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this V3d u, V3d v)
        => Fun.IsTiny(u.Cross(v).Norm1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this V3d u, V3d v, double epsilon = 1e-6)
    {
        var un = u.Normalized;
        var vn = v.Normalized;

        return (un - vn).Length < epsilon || (un + vn).Length < epsilon;
    }

    #endregion

    #region Ray3d - V3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3d ray, V3d vec)
        => ray.Direction.IsParallelTo(vec);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3d ray, V3d vec, double epsilon = 1e-6)
        => ray.Direction.IsParallelTo(vec, epsilon);

    #endregion

    #region Ray3d - Ray3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3d r0, Ray3d r1)
        => r0.Direction.IsParallelTo(r1.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3d r0, Ray3d r1, double epsilon = 1e-6)
        => r0.Direction.IsParallelTo(r1.Direction, epsilon);

    #endregion

    #region Plane3d - Plane3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Plane3d p0, Plane3d p1)
        => p0.Normal.IsParallelTo(p1.Normal);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Plane3d p0, Plane3d p1, double epsilon = 1e-6)
        => p0.Normal.IsParallelTo(p1.Normal, epsilon);

    #endregion

    #region Ray3d - Plane3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Ray3d ray, Plane3d plane)
        => ray.Direction.IsOrthogonalTo(plane.Normal);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Plane3d plane, Ray3d ray)
        => ray.Direction.IsOrthogonalTo(plane.Normal);

    #endregion

    #region Line3d - Line3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Line3d l0, Line3d l1)
        => l0.Direction.IsParallelTo(l1.Direction);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsParallelTo(this Line3d l0, Line3d l1, double epsilon = 1e-6)
        => l0.Direction.IsParallelTo(l1.Direction, epsilon);

    #endregion

}
