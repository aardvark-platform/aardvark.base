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

#region Ellipse3f

public partial struct Ellipse3f : IValidity
{
    public static Ellipse3f Invalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(V3f.NaN, V3f.Zero, V3f.NaN, V3f.NaN);
    }

    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Normal != V3f.Zero;
    }

    public readonly bool IsInvalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Normal == V3f.Zero;
    }

    public readonly float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vec.Cross(Axis0, Axis1).Length * ConstantF.Pi;
    }
}

#endregion

#region Ellipse3d

public partial struct Ellipse3d : IValidity
{
    public static Ellipse3d Invalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => new(V3d.NaN, V3d.Zero, V3d.NaN, V3d.NaN);
    }

    public readonly bool IsValid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Normal != V3d.Zero;
    }

    public readonly bool IsInvalid
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Normal == V3d.Zero;
    }

    public readonly double Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Vec.Cross(Axis0, Axis1).Length * Constant.Pi;
    }
}

#endregion

