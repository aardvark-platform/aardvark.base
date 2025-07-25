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

/// <summary>
/// </summary>
public static class IDisposableExtensions
{
    /// <summary>
    /// Calls Dispose() and returns true if not null,
    /// returns false otherwise.
    /// </summary>
    public static bool TryDispose(this IDisposable self)
    {
        if (self == null) return false;
        self.Dispose();
        return true;
    }

    /// <summary>
    /// Checks if the objects impelemtns the IDisposable interface,
    /// performs Dispose() in case and returns true, otherwise false.
    /// </summary>
    public static bool TryDispose(this object obj)
    {
        if (obj is not IDisposable d) return false;
        d.Dispose();
        return true;
    }

    /// <summary>
    /// Disposes a sequence of IDisposables.
    /// </summary>
    public static void DisposeAll(this IEnumerable<IDisposable> disposables)
    {
        foreach (var d in disposables) d.Dispose();
    }

    /// <summary>
    /// Disposes a list of IDisposables.
    /// </summary>
    public static void DisposeAll(this List<IDisposable> disposables)
    {
        foreach (var d in disposables) d.Dispose();
    }

    /// <summary>
    /// Disposes an array of IDisposables.
    /// </summary>
    public static void DisposeAll(this IDisposable[] disposables)
    {
        foreach (var d in disposables) d.Dispose();
    }

    /// <summary>
    /// Disposes all valid disposables of a sequence.
    /// </summary>
    public static void TryDisposeAll(this IEnumerable<IDisposable> disposables)
    {
        disposables.WhereNotNull().ForEach(x => x.Dispose());
    }

    /// <summary>
    /// Disposes all disposables in a collections and clears the collection afterwards.
    /// </summary>
    public static void DisposeAllAndClear(this ICollection<IDisposable> disposables)
    {
        if (disposables.IsReadOnly) throw new Exception("The collection is read-only... it cannot be cleared.");
        foreach (var d in disposables) d.Dispose();
        disposables.Clear();
    }

    /// <summary>
    /// Disposes all disposables in the list and clears it afterwards.
    /// </summary>
    public static void DisposeAllAndClear(this List<IDisposable> disposables)
    {
        foreach (var d in disposables) d.Dispose();
        disposables.Clear();
    }

}

/// <summary>
/// </summary>
public static class Try
{
    /// <summary>
    /// Disposes the Disposable if not null and sets its reference to null.
    /// NOTE: needs to be generic since refs somehow do not work with interfaces.
    /// </summary>
    public static void Dispose<T>(ref T x) where T : IDisposable
    {
        if (x != null)
        {
            x.Dispose();
            x = default;
        }
    }
}
