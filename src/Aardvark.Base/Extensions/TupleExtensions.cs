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

namespace Aardvark.Base;

public static class TupleExtensions
{
    /// <summary>
    /// Number of NaNs in tuple.
    /// </summary>
    public static int CountNonNaNs(this (double, double) p)
    {
        int count = 2;
        if (double.IsNaN(p.Item1)) --count;
        if (double.IsNaN(p.Item2)) --count;
        return count;
    }

    /// <summary>
    /// Number of NaNs in tuple.
    /// </summary>
    public static int CountNonNaNs(this (double, double, double) p)
    {
        int count = 3;
        if (double.IsNaN(p.Item1)) --count;
        if (double.IsNaN(p.Item2)) --count;
        if (double.IsNaN(p.Item3)) --count;
        return count;
    }

    /// <summary>
    /// Number of NaNs in tuple.
    /// </summary>
    public static int CountNonNaNs(this (double, double, double, double) p)
    {
        int count = 4;
        if (double.IsNaN(p.Item1)) --count;
        if (double.IsNaN(p.Item2)) --count;
        if (double.IsNaN(p.Item3)) --count;
        if (double.IsNaN(p.Item4)) --count;
        return count;
    }

    /// <summary>
    /// Gets i-th value of tuple.
    /// </summary>
    public static double Get(this (double, double) p, int i) => i switch
    {
        0 => p.Item1,
        1 => p.Item2,
        _ => throw new IndexOutOfRangeException(),
    };

    /// <summary>
    /// Gets i-th value of tuple.
    /// </summary>
    public static double Get(this (double, double, double) p, int i) => i switch
    {
        0 => p.Item1,
        1 => p.Item2,
        2 => p.Item3,
        _ => throw new IndexOutOfRangeException(),
    };

    /// <summary>
    /// Gets i-th value of tuple.
    /// </summary>
    public static double Get(this (double, double, double, double) p, int i) => i switch
    {
        0 => p.Item1,
        1 => p.Item2,
        2 => p.Item3,
        3 => p.Item4,
        _ => throw new IndexOutOfRangeException(),
    };

#if !TRAVIS_CI

    /// <summary>
    /// Sets i-th value in tuple (in-place).
    /// </summary>
    public static void Set(this ref (double, double) p, int i, double value)
    {
        switch (i)
        {
            case 0: p.Item1 = value; break;
            case 1: p.Item2 = value; break;
            default: throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Sets i-th value in tuple (in-place).
    /// </summary>
    public static void Set(this ref (double, double, double) p, int i, double value)
    {
        switch (i)
        {
            case 0: p.Item1 = value; break;
            case 1: p.Item2 = value; break;
            case 2: p.Item3 = value; break;
            default: throw new IndexOutOfRangeException();
        }
    }

    /// <summary>
    /// Sets i-th value in tuple (in-place).
    /// </summary>
    public static void Set(this ref (double, double, double, double) p, int i, double value)
    {
        switch (i)
        {
            case 0: p.Item1 = value; break;
            case 1: p.Item2 = value; break;
            case 2: p.Item3 = value; break;
            case 3: p.Item4 = value; break;
            default: throw new IndexOutOfRangeException();
        }
    }

#endif

    /// <summary>
    /// Creates tuple from given values with values in ascending order. 
    /// </summary>
    public static (double, double) CreateAscending(double d0, double d1)
        => d0 < d1 ? (d0, d1) : (d1, d0);

    /// <summary>
    /// Creates tuple from given values with values in ascending order. 
    /// </summary>
    public static (double, double, double) CreateAscending(double d0, double d1, double d2)
    {
        if (d0 < d1)
        {
            if (d1 < d2)
                return (d0, d1, d2);
            else
                return (d0 < d2) ? (d0, d2, d1) : (d2, d0, d1);
        }
        else
        {
            if (d2 < d1)
                return (d2, d1, d0);
            else
                return (d0 < d2) ? (d1, d0, d2) : (d1, d2, d0);
        }
    }
}
