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
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var rtype = isDouble ? "double" : "float";
    //#   var tc = isDouble ? "d" : "f";
    //#   var v2t = "V2" + tc;
    //#   var v3t = "V3" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var ray2t = "Ray2" + tc;
    //#   var ray3t = "Ray3" + tc;
    // 2-Dimensional

    #region __v2t__ - __v2t__

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this __v2t__ u, __v2t__ v)
        => Fun.IsTiny(u.Dot(v));

    #endregion

    #region __ray2t__ - __v2t__

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this __ray2t__ ray, __v2t__ v)
        => ray.Direction.IsOrthogonalTo(v);

    #endregion

    #region __ray2t__ - __ray2t__

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this __ray2t__ r0, __ray2t__ r1)
        => r0.Direction.IsOrthogonalTo(r1.Direction);

    #endregion

    // 3-Dimensional

    #region __v3t__ - __v3t__

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this __v3t__ u, __v3t__ v)
        => Fun.IsTiny(u.Dot(v));

    #endregion

    #region __ray3t__ - __v3t__

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this __ray3t__ ray, __v3t__ vec)
        => ray.Direction.IsOrthogonalTo(vec);

    #endregion

    #region __ray3t__ - __ray3t__

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this __ray3t__ r0, __ray3t__ r1)
        => r0.Direction.IsOrthogonalTo(r1.Direction);

    #endregion

    #region __plane3t__ - __plane3t__

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this __plane3t__ p0, __plane3t__ p1)
        => p0.Normal.IsOrthogonalTo(p1.Normal);

    #endregion

    #region __ray3t__ - __plane3t__

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsOrthogonalTo(this __ray3t__ ray, __plane3t__ plane)
        => ray.Direction.IsParallelTo(plane.Normal);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNormalTo(this __plane3t__ plane, __ray3t__ ray)
        => ray.Direction.IsParallelTo(plane.Normal);

    #endregion

    //# }
}
