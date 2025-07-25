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

public static class LinearCombination
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var tc = isDouble ? "d" : "f";
    //#   var v2t = "V2" + tc;
    //#   var v3t = "V3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";

    #region __v3t__ - __v3t__

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLinearCombinationOf(this __v3t__ x, __v3t__ u, __v3t__ v)
    {
        __v3t__ n = u.Cross(v);
        return n.IsOrthogonalTo(x);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLinearCombinationOf(this __v3t__ x, __v3t__ u)
        => x.IsParallelTo(u);

    public static bool IsLinearCombinationOf(this __v3t__ x, __v3t__ u, __v3t__ v, out __ftype__ t0, out __ftype__ t1)
    {
        //x == t2*u + t1*v
        __v3t__ n = u.Cross(v);

        __ftype__[,] mat = new __ftype__[3, 3]
        {
            {u.X,v.X,n.X},
            {u.Y,v.Y,n.Y},
            {u.Z,v.Z,n.Z}
        };

        __ftype__[] result = [ x.X, x.Y, x.Z ];

        int[] perm = mat.LuFactorize();
        __v3t__ t = new(mat.LuSolve(perm, result));

        if (Fun.IsTiny(t.Z))
        {
            t0 = t.X;
            t1 = t.Y;

            return true;
        }
        else
        {
            t0 = __ftype__.NaN;
            t1 = __ftype__.NaN;

            return false;
        }

        //x == 
    }

    #endregion

    //# }
}
