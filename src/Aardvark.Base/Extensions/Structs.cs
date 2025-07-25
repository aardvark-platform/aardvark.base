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
using System.Linq;

namespace Aardvark.Base;

#region IndexedValue

public readonly struct IndexedValue<T>(int index, T value)
{
    public readonly int Index = index;
    public readonly T Value = value;
}

#endregion

#region ComparableIndexedValue

public readonly struct ComparableIndexedValue<T>(int index, T value)
    : IComparable<ComparableIndexedValue<T>>
    where T : IComparable<T>
{
    public readonly int Index = index;
    public readonly T Value = value;

    #region IComparable<IndexedValue<T>> Members

    public int CompareTo(ComparableIndexedValue<T> other)
    {
        return Value.CompareTo(other.Value);
    }

    #endregion
}

#endregion

public static class StructsExtensions
{
    #region ComparableIndexedValue

    public static IEnumerable<ComparableIndexedValue<T>> ComparableIndexedValues<T>(
            this IEnumerable<T> self)
        where T : IComparable<T>
    {
        return self.Select((item, i) => new ComparableIndexedValue<T>(i, item));
    }
    
    public static ComparableIndexedValue<T> ComparableIndexedValue<T>(
            this T self, int index)
        where T : IComparable<T>
    {
        return new ComparableIndexedValue<T>(index, self);
    }

    #endregion
}