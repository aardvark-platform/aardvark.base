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
using Ex = System.Linq.Expressions.Expression;

namespace Aardvark.Base;

#region Ipol

public static partial class Ipol
{
    #region Generators

    public static Func<U, T> Generate<T, U>(
        T a, T b, Func<T, T, U, T> interpolator)
    {
        return Ipol<T, U>.Generate(a, b, interpolator);
    }

    public static Func<U, T> Generate<T, U>(
        T a, T b, T c, T d, Func<T, T, T, T, U, T> interpolator)
    {
        return Ipol<T, U>.Generate(a, b, c, d, interpolator);
    }

    #endregion
}

#endregion

#region Ipol<T, U>

public static class Ipol<T, U>
{
    internal static Func<T, T, U, T> s_lerpInterpolator;

    #region Generators

    public static Func<U, T> Generate(
        T a, T b, Func<T, T, U, T> interpolator)
    {
        return x => interpolator(a, b, x);
    }

    public static Func<U, T> Generate(
        T a, T b, T c, T d, Func<T, T, T, T, U, T> interpolator)
    {
        return x => interpolator(a, b, c, d, x);
    }

    #endregion

    #region Linear Interpolation

    /// <summary>
    /// Gets a function that performs linear interpolation for values of T:
    /// (T a, T b, U t) => a * (1 - t) + b * t.
    /// For t = 0.0 the funtion will return a, and for t = 1.0 the function will return b.
    /// </summary>
    public static Func<T, T, U, T> Lerp
    {
        get
        {
            if (s_lerpInterpolator == null)
            {
                var a = Ex.Parameter(typeof(T), "a");
                var b = Ex.Parameter(typeof(T), "b");
                var x = Ex.Parameter(typeof(U), "x");

                var expr = Ex.Lambda<Func<T, T, U, T>>(
                    Ex.Add(
                        Ex.Multiply(a, Ex.Subtract(Ex.Constant(1.0), x)),
                        Ex.Multiply(b, x)
                        ),

                    a, b, x
                    );
                s_lerpInterpolator = expr.Compile();
            }

            return s_lerpInterpolator;
        }
    }

    #endregion
}

#endregion
