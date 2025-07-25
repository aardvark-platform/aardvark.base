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
//#   var tc = isDouble ? "d" : "f";
//#   var type = "Quad2" + tc;
//#   var v2t = "V2" + tc;
//#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
//#   var half = isDouble ? "0.5" : "0.5f";
#region __type__

/// <summary>
/// A two-dimensional quadrangle specified by its four points.
/// </summary>
public partial struct __type__
{
    #region Geometric Properties

    public readonly __ftype__ Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => __half__ * ((P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y)).Abs();
    }

    public readonly bool IsDegenerated
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (P1 - P0).AllTiny || (P2 - P1).AllTiny || (P3 - P2).AllTiny || (P0 - P3).AllTiny || WindingOrder.IsTiny();
    }

    /// <summary>
    /// Returns a value less than zero for ccw and greater than zero for cw.
    /// The magnitude magnitude is twice the area.
    /// </summary>
    public readonly __ftype__ WindingOrder
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => (P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y);
    }

    #endregion
}

#endregion

//# }
