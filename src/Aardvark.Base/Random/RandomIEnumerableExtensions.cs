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
using System.Collections.Generic;

namespace Aardvark.Base;

public static class RandomIEnumerableExtensions
{
    /// <summary>
    /// Yields each element with propability p.
    /// </summary>
    public static IEnumerable<T> TakeRandomly<T>(this IEnumerable<T> self, double p,
                                                 IRandomUniform random = null)
    {
#if NETSTANDARD2_0
        if (self == null) throw new ArgumentNullException();
#else
        ArgumentNullException.ThrowIfNull(self);
#endif
        if (p < 0 || p > 1) throw new ArgumentOutOfRangeException(nameof(p));

        random ??= new RandomSystem();
        foreach (var s in self) if (random.UniformDouble() <= p) yield return s;
    }

    /// <summary>
    /// Yields each element with propability p.
    /// </summary>
    public static IEnumerable<R> TakeRandomly<T, R>(this IEnumerable<T> self, Func<T, R> selector, double p,
                                                    IRandomUniform random = null)
    {
#if NETSTANDARD2_0
        if (self == null) throw new ArgumentNullException();
#else
        ArgumentNullException.ThrowIfNull(self);
#endif
        if (p < 0 || p > 1) throw new ArgumentOutOfRangeException(nameof(p));

        random ??= new RandomSystem();
        foreach (var s in self) if (random.UniformDouble() <= p) yield return selector(s);
    }
}
