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
using System.Collections.Concurrent;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Aardvark.Base;

public static class TypeExtensions
{
    private static readonly MethodInfo unsafeSizeOfMeth = typeof(Unsafe).GetMethod(nameof(Unsafe.SizeOf), BindingFlags.Public | BindingFlags.Static);

    private static readonly ConcurrentDictionary<Type, int> unsafeTypeSizes = new();

    /// <summary>
    /// Returns the managed size of the given type.
    /// </summary>
    public static int GetCLRSize(this Type t)
    {
        return unsafeTypeSizes.GetOrAdd(t, t =>
        {
            var mi = unsafeSizeOfMeth.MakeGenericMethod(t);
            return (int)mi.Invoke(null, null);
        });
    }
}
