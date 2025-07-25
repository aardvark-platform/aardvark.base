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

#region Ellipse2f

public partial struct Ellipse2f : IValidity
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

    public readonly float Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Fun.Abs(Axis0.X * Axis1.Y - Axis0.Y * Axis1.X) * ConstantF.Pi;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static float GetArea(V2f axis0, V2f axis1)
        => Fun.Abs(axis0.X * axis1.Y - axis0.Y * axis1.X) * ConstantF.Pi;
}

#endregion

#region Ellipse2d

public partial struct Ellipse2d : IValidity
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

    public readonly double Area
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get => Fun.Abs(Axis0.X * Axis1.Y - Axis0.Y * Axis1.X) * Constant.Pi;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    static double GetArea(V2d axis0, V2d axis1)
        => Fun.Abs(axis0.X * axis1.Y - axis0.Y * axis1.X) * Constant.Pi;
}

#endregion

