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
/// Provides various methods determining normalism
/// </summary>
public static class Orthogonality
{
    // 2-Dimensional

    #region V2f - V2f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this V2f u, V2f v)
        => Fun.IsTiny(u.Dot(v));

    #endregion

    #region Ray2f - V2f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray2f ray, V2f v)
        => ray.Direction.IsOrthogonalTo(v);

    #endregion

    #region Ray2f - Ray2f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray2f r0, Ray2f r1)
        => r0.Direction.IsOrthogonalTo(r1.Direction);

    #endregion

    // 3-Dimensional

    #region V3f - V3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this V3f u, V3f v)
        => Fun.IsTiny(u.Dot(v));

    #endregion

    #region Ray3f - V3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray3f ray, V3f vec)
        => ray.Direction.IsOrthogonalTo(vec);

    #endregion

    #region Ray3f - Ray3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray3f r0, Ray3f r1)
        => r0.Direction.IsOrthogonalTo(r1.Direction);

    #endregion

    #region Plane3f - Plane3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Plane3f p0, Plane3f p1)
        => p0.Normal.IsOrthogonalTo(p1.Normal);

    #endregion

    #region Ray3f - Plane3f

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray3f ray, Plane3f plane)
        => ray.Direction.IsParallelTo(plane.Normal);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNormalTo(this Plane3f plane, Ray3f ray)
        => ray.Direction.IsParallelTo(plane.Normal);

    #endregion

    // 2-Dimensional

    #region V2d - V2d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this V2d u, V2d v)
        => Fun.IsTiny(u.Dot(v));

    #endregion

    #region Ray2d - V2d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray2d ray, V2d v)
        => ray.Direction.IsOrthogonalTo(v);

    #endregion

    #region Ray2d - Ray2d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray2d r0, Ray2d r1)
        => r0.Direction.IsOrthogonalTo(r1.Direction);

    #endregion

    // 3-Dimensional

    #region V3d - V3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this V3d u, V3d v)
        => Fun.IsTiny(u.Dot(v));

    #endregion

    #region Ray3d - V3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray3d ray, V3d vec)
        => ray.Direction.IsOrthogonalTo(vec);

    #endregion

    #region Ray3d - Ray3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray3d r0, Ray3d r1)
        => r0.Direction.IsOrthogonalTo(r1.Direction);

    #endregion

    #region Plane3d - Plane3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Plane3d p0, Plane3d p1)
        => p0.Normal.IsOrthogonalTo(p1.Normal);

    #endregion

    #region Ray3d - Plane3d

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this Ray3d ray, Plane3d plane)
        => ray.Direction.IsParallelTo(plane.Normal);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNormalTo(this Plane3d plane, Ray3d ray)
        => ray.Direction.IsParallelTo(plane.Normal);

    #endregion

}
