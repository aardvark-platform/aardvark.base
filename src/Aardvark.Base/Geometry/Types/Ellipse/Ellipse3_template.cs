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
//#   var type = "Ellipse3" + tc;
//#   var v3t = "V3" + tc;
//#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
#region __type__

public partial struct __type__ : IValidity
{
    public static __type__ Invalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(__v3t__.NaN, __v3t__.Zero, __v3t__.NaN, __v3t__.NaN);
    }

    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Normal != __v3t__.Zero;
    }

    public readonly bool IsInvalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Normal == __v3t__.Zero;
    }

    public readonly __ftype__ Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vec.Cross(Axis0, Axis1).Length * __pi__;
    }
}

#endregion

//# }
