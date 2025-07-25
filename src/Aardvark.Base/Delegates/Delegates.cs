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

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
#pragma warning restore IDE0079 // Remove unnecessary suppression

namespace Aardvark.Base;

public delegate void ActionValRef<T0, T1>(T0 a0, ref T1 a1);
public delegate void ActionRefValVal<T0, T1, T2>(ref T0 a0, T1 a1, T2 a2);

public static class LambdaApplicationExtensions
{
    public static bool ApplyByType<T, T0>(
        this T obj, Action<T0> proc0)
        where T0 : class
    {
        if (obj is T0 a0) { proc0(a0); return true; }
        return false;
    }

    public static void ApplyByTypeStrict<T, T0>(
        this T obj, Action<T0> proc0)
        where T0 : class
    {
        if (obj is T0 a0) { proc0(a0); return; }
        throw new ArgumentException();
    }

    public static bool ApplyByType<T, T0, T1>(
        this T obj, Action<T0> proc0, Action<T1> proc1)
        where T0 : class
        where T1 : class
    {
        if (obj is T0 a0) { proc0(a0); return true; }
        if (obj is T1 a1) { proc1(a1); return true; }
        return false;
    }

    public static void ApplyByTypeStrict<T, T0, T1>(
        this T obj, Action<T0> proc0, Action<T1> proc1)
        where T0 : class
        where T1 : class
    {
        if (obj is T0 a0) { proc0(a0); return; }
        if (obj is T1 a1) { proc1(a1); return; }
        throw new ArgumentException();
    }

    public static Tr ApplyByTypeStrict<T, T0, Tr>(
        this T obj, Func<T0, Tr> fun0)
        where T0 : class
    {
        if (obj is T0 a0) return fun0(a0);
        throw new ArgumentException();
    }

    public static Tr ApplyByTypeStrict<T, T0, T1, Tr>(
        this T obj, Func<T0, Tr> fun0, Func<T1, Tr> fun1)
        where T0 : class
        where T1 : class
    {
        if (obj is T0 a0) return fun0(a0);
        if (obj is T1 a1) return fun1(a1);
        throw new ArgumentException();
    }
}
