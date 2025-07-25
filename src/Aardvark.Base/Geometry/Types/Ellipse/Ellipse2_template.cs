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
//#   var type = "Ellipse2" + tc;
//#   var v2t = "V2" + tc;
//#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
#region __type__

public partial struct __type__ : IValidity
{
    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => true;
    }

    public readonly bool IsInvalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => false;
    }

    public readonly __ftype__ Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Fun.Abs(Axis0.X * Axis1.Y - Axis0.Y * Axis1.X) * __pi__;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static __ftype__ GetArea(__v2t__ axis0, __v2t__ axis1)
        => Fun.Abs(axis0.X * axis1.Y - axis0.Y * axis1.X) * __pi__;
}

#endregion

//# }
