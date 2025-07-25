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
using Aardvark.Base.Sorting;
using System;

namespace Aardvark.Base;

public static class ComparableArrayExtensions
{
    #region Comparable Array
    
		public static int IndexOfNSmallest<T>(this T[] a, int n)
        where T : IComparable<T>
    {
        if (n == 0) return a.IndexOfMin();
        var p = a.CreatePermutationQuickMedianAscending(n);
        return p[n];
    }

    public static int IndexOfNLargest<T>(this T[] a, int n)
        where T : IComparable<T>
    {
        if (n == 0) return a.IndexOfMax();
        var p = a.CreatePermutationQuickMedianDescending(n);
        return p[n];
    }
    
    #endregion
}
