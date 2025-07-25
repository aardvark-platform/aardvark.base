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
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base;

public static class CastExtensions
{
    public static T[] ToArrayOfT<T>(this IEnumerable s) => [.. (from T o in s select o)];

    public static List<T> ToListOfT<T>(this IEnumerable s) => [.. (from T o in s select o)];

    public static IEnumerable<T> ToIEnumerableOfT<T>(this IEnumerable s) => from T o in s select o;

    public static List<T> IntoList<T>(this T item) => [item];

    public static T[] IntoArray<T>(this T item) => [item];

    public static IEnumerable<T> IntoIEnumerable<T>(this T item) => [item];

    /// <summary>
    /// Returns the item as a T.
    /// Or throws an exception if not convertible.
    /// </summary>
    public static T To<T>(this object item) where T : class
    {
        if (item is T) return item as T;
        throw new ArgumentException(string.Format("expected {0} instead of {1}", typeof(T), item.GetType()));
    }
}
