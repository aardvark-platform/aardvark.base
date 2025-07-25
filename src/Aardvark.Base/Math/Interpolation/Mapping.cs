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
using System;
using static System.Math;

namespace Aardvark.Base;

public static class Mapping
{
    /// <summary>
    /// [-inf, +inf] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> Trunc = x => x < 0 ? 0 : (x > 1 ? 1 : x);

    /// <summary>
    /// [-inf, +inf] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> Repeat = x => x - Floor(x);

    /// <summary>
    /// [-inf, +inf] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> RepeatAndMirror = x =>
    {
        x -= Floor(x * 0.5) * 2;
        return x < 1 ? x : 2 - x;
    };

    /// <summary>
    /// [0, 1] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> Linear = x => x;

    /// <summary>
    /// [0, 1] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> Cosine = x => (1 - Cos(x * PI)) * 0.5;

    /// <summary>
    /// [0, 1] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> EaseIn = x => Pow(x, 1.3);

    /// <summary>
    /// [0, 1] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> EaseOut = x => 1 - EaseIn(1 - x);

    /// <summary>
    /// [0, 1] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> EaseInEaseOut = Cosine;

    /// <summary>
    /// [0, 1] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> Min = x => 0;

    /// <summary>
    /// [0, 1] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> Max = x => 1;

    /// <summary>
    /// [0, 1] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> JumpStart = x => x > 0 ? 1 : 0;

    /// <summary>
    /// [0, 1] => [0, 1]
    /// </summary>
    public static readonly Func<double, double> JumpStop = x => x < 1 ? 0 : 1;
}
