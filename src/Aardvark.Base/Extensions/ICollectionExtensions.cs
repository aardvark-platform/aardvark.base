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
using System.Collections.Generic;

namespace Aardvark.Base;

public static class ICollectionExtensions
{
    /// <summary>
    /// Adds the item to the collection if not contained, otherwise removes the item from the collection.
    /// </summary>
    /// <returns>Containment state of the item after the operation</returns>
    public static bool ToggleContainment<T>(this ICollection<T> self, T item)
    {
        if (!self.Remove(item))
        {
            self.Add(item);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Adds the elements in the items enumeration to the collection.
    /// </summary>
    public static void AddRange<T>(this ICollection<T> self, IEnumerable<T> items)
    {
        foreach (var item in items)
            self.Add(item);
    }
}
