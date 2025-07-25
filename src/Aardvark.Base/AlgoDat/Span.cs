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

public static class SpanPinning
{
    /// <summary>
    /// Pins a span and performs the given action with the resulting pointer.
    /// This method is required due to missing support for pinning spans in F#.
    /// </summary>
    /// <param name="span">The span to pin.</param>
    /// <param name="action">The action to perform.</param>
    /// <returns>The result of the action.</returns>
    public unsafe static Result Pin<T, Result>(Span<T> span, Func<IntPtr, Result> action) where T : unmanaged
    {
        fixed (T* ptr = span)
        {
            return action((IntPtr)ptr);
        }
    }
}