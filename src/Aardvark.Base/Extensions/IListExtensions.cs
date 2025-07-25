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

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA1510 // Use ArgumentNullException throw helper
#pragma warning restore IDE0079 // Remove unnecessary suppression

namespace Aardvark.Base;

public static class IListExtensions
{
    /// <summary>
    /// Swap the two elements specified by ther indices.
    /// </summary>
    public static void Swap<T>(this IList<T> self, int i, int j)
    {
        (self[j], self[i]) = (self[i], self[j]);
    }

    public static SubRange<T> SubRange<T>(this IList<T> self, int index, int count)
        => new(self, index, count);

    #region FindIndex

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, 
    /// and returns the zero-based index of the first occurrence within the range of elements in the IList(of T) 
    /// that starts at the <paramref name="startIndex"/> and contains <paramref name="count"/> elements.
    /// If startSearch differs from startIndex, a cyclic search is performed in the range starting at <paramref name="startSearch"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the IList(of T).</typeparam>
    /// <param name="list">The list to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search range.</param>
    /// <param name="count">The number of elements in the search range.</param>
    /// <param name="startSearch">The first element to search in a cyclic manner. Will be wrapped around, if outside [startIndex, startIndex+count-1]. Non-cyclick search if this equals <paramref name="startIndex"/>.</param>
    /// <param name="forward">If true the search is performed in direction of increasing indices, else in the direction of decreasing indices.</param>
    /// <param name="match">The Predicate(of T) that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
    public static int FindIndex<T>(this IList<T> list, int startIndex, int count, bool forward, int startSearch, Predicate<T> match)
    {
        if (list == null)
            throw new ArgumentNullException(nameof(list));
        var listCount = list.Count;
        if ((startIndex < 0) || (startIndex > listCount))
            throw new ArgumentOutOfRangeException(nameof(startIndex), "Index was out of range. Must be non-negative and less than the size of the collection.");
        if ((count < 0) || (startIndex > (listCount - count)))
            throw new ArgumentOutOfRangeException(nameof(count), "Count must be positive and count must refer to a location within the string/array/collection.");
        if (match == null)
            throw new ArgumentNullException(nameof(match));
        if (count == 0)
            return -1;
        int endIndex = startIndex + count;
        if ((startSearch < startIndex) || (startSearch >= endIndex))
        {
            //wrap around startSearch to be in the interval [startIndex, startIndex+count-1]
            startSearch = (startSearch - startIndex) % count;
            if (startSearch < 0)
                startSearch += count; //this is the ModP implementation. Since Fun.ModP() is in Aardvark.Math it is not accessible here.
            startSearch += startIndex;
        }
        if (forward)
        {
            for (int i = startSearch; i < endIndex; i++)
                if (match(list[i]))
                    return i;
            for (int i = startIndex; i < startSearch; i++)
                if (match(list[i]))
                    return i;
        }
        else
        {
            for (int i = startSearch; i >= startIndex; i--)
                if (match(list[i]))
                    return i;
            for (int i = endIndex - 1; i > startSearch; i--)
                if (match(list[i]))
                    return i;
        }
        return -1;
    }

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, 
    /// and returns the zero-based index of the first occurrence within the range of elements in the IList(of T) 
    /// that starts at the <paramref name="startIndex"/> and contains <paramref name="count"/> elements.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the IList(of T).</typeparam>
    /// <param name="list">The list to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search range.</param>
    /// <param name="count">The number of elements in the search range.</param>
    /// <param name="forward">If true the search is performed in direction of increasing indices, else in the direction of decreasing indices.</param>
    /// <param name="match">The Predicate(of T) that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
    public static int FindIndex<T>(this IList<T> list, int startIndex, int count, bool forward, Predicate<T> match)
        => FindIndex(list, startIndex, count, forward, startIndex, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, 
    /// and returns the zero-based index of the first occurrence within the range of elements in the IList(of T) 
    /// that extends from the <paramref name="startIndex"/> to the end of the list.
    /// If startSearch differs from startIndex, a cyclic search is performed in the range starting at <paramref name="startSearch"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the IList(of T).</typeparam>
    /// <param name="list">The list to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search range.</param>
    /// <param name="startSearch">The first element to search in a cyclic manner. Non-cyclick search if this equals <paramref name="startIndex"/></param>
    /// <param name="forward">If true the search is performed in direction of increasing indices, else in the direction of decreasing indices.</param>
    /// <param name="match">The Predicate(of T) that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
    public static int FindIndex<T>(this IList<T> list, int startIndex, bool forward, int startSearch, Predicate<T> match)
    {
        if (list == null)
            throw new ArgumentNullException(nameof(list));
        return FindIndex(list, startIndex, list.Count - startIndex, forward, startSearch, match);
    }

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, 
    /// and returns the zero-based index of the first occurrence within the range of elements in the IList(of T) 
    /// that extends from the <paramref name="startIndex"/> to the end of the list.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the IList(of T).</typeparam>
    /// <param name="list">The list to search.</param>
    /// <param name="startIndex">The zero-based starting index of the search range.</param>
    /// <param name="forward">If true the search is performed in direction of increasing indices, else in the direction of decreasing indices.</param>
    /// <param name="match">The Predicate(of T) that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
    public static int FindIndex<T>(this IList<T> list, int startIndex, bool forward, Predicate<T> match)
        => FindIndex(list, startIndex, forward, startIndex, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, 
    /// and returns the zero-based index of the first occurrence.
    /// If startSearch differs from startIndex, a cyclic search is performed in the range starting at <paramref name="startSearch"/>.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the IList(of T).</typeparam>
    /// <param name="list">The list to search.</param>
    /// <param name="startSearch">The first element to search in a cyclic manner.</param>
    /// <param name="forward">If true the search is performed in direction of increasing indices, else in the direction of decreasing indices.</param>
    /// <param name="match">The Predicate(of T) that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
    public static int FindIndex<T>(this IList<T> list, bool forward, int startSearch, Predicate<T> match)
        => FindIndex(list, 0, forward, startSearch, match);

    /// <summary>
    /// Searches for an element that matches the conditions defined by the specified predicate, 
    /// and returns the zero-based index of the first occurrence.
    /// </summary>
    /// <typeparam name="T">The type of the elements of the IList(of T).</typeparam>
    /// <param name="list">The list to search.</param>
    /// <param name="forward">If true the search is performed in direction of increasing indices, else in the direction of decreasing indices.</param>
    /// <param name="match">The Predicate(of T) that defines the conditions of the element to search for.</param>
    /// <returns>The zero-based index of the first occurrence of an element that matches the conditions defined by match, if found; otherwise, –1.</returns>
    public static int FindIndex<T>(this IList<T> list, bool forward, Predicate<T> match)
        => FindIndex(list, 0, forward, 0, match);


    #endregion FindIndex

    #region IList of IComparables

    public static int CompareTo<T>(this IList<T> self, IList<T> other)
        where T : IComparable<T>
    {
        int imax = System.Math.Min(self.Count, other.Count);
        for (int i = 0; i < imax; i++)
        {
            int r = self[i].CompareTo(other[i]);
            if (r != 0) return r;
        }
        return (self.Count < other.Count) ? -1 : ((self.Count > other.Count) ? 1 : 0);
    }

    public static int FirstIndexOf<T>(this IList<T> self, IList<T> other) where T : IComparable<T>
        => self.FirstIndexOf(other, 0);

    /// <summary>
    /// Search first occurance of other in self, and return index if found
    /// or -1 if not found. NOTE: simple algorithm, not optimized.
    /// </summary>
    public static int FirstIndexOf<T>(this IList<T> self, IList<T> other,
                                 int startIndex)
        where T : IComparable<T>
    {
        for (int i = startIndex; i < 1 + self.Count - other.Count; i++)
        {
            bool match = true;
            for (int j = 0; j < other.Count; j++)
                if (self[i + j].CompareTo(other[j]) != 0)
                {
                    match = false; break;
                }
            if (match) return i;
        }
        return -1;
    }

    public static int SmallestIndex<T>(this IList<T> self)
        where T: IComparable<T>
    {
        int index = 0;
        T min = self[0];
        for (int i = 1; i < self.Count; i++)
            if (self[i].CompareTo(min) < 0) { min = self[i]; index = i; }
        return index;
    }

    public static int LargestIndex<T>(this IList<T> self)
        where T : IComparable<T>
    {
        int index = 0;
        T max = self[0];
        for (int i = 1; i < self.Count; i++)
            if (self[i].CompareTo(max) > 0) { max = self[i]; index = i; }
        return index;
    }

    /// <summary>
    /// Returns the index of the n-smallest item.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="n">[0-self.Count()-1]; n=0 -> index of min element</param>
    public static int NSmallestIndex<T>(this IList<T> self, int n)
        where T : IComparable<T>
    {
        if (n == 0) return self.SmallestIndex();
        T min;
        var sorted = new List<T>(self);
        sorted.Sort();
        min = sorted[n];
        return self.IndexOf(min);
    }

    /// <summary>
    /// Returns the index of the n-largest item.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="n">[0-self.Count()-1]; n=0 -> index of max element</param>
    public static int NLargestIndex<T>(this IList<T> self, int n)
        where T : IComparable<T>
    {
        if (n == 0) return self.LargestIndex();
        T max;
        var sorted = new List<T>(self);
        sorted.Sort();
        max = sorted[sorted.Count - 1 - n];
        return self.IndexOf(max);
    }

    #endregion
    
    #region Generic IList Copying

    /// <summary>
    /// Use this instead of Clone() in order to get a typed array back.
    /// </summary>
    public static T[] Copy<T>(this IList<T> xs)
    {
        var count = xs.Count;
        var result = new T[count];
        for (var i = 0; i < count; i++) result[i] = xs[i];
        return result;
    }

    /// <summary>
    /// Create a copy of the specified length. If the copy is longer
    /// it is filled with default elements.
    /// </summary>
    public static T[] Copy<T>(this IList<T> xs, int count)
    {
        var result = new T[count];
        var len = Math.Min(count, xs.Count);
        for (var i = 0; i < len; i++) result[i] = xs[i];
        return result;
    }

    /// <summary>
    /// Create a copy of the specified length starting at the specified
    /// start. If the copy is longer it is filled with default elements.
    /// </summary>
    public static T[] Copy<T>(this IList<T> xs, int start, int count)
    {
        var result = new T[count];
        var len = Math.Min(count, xs.Count - start);
        for (var i = 0; i < len; i++) result[i] = xs[i + start];
        return result;
    }

    /// <summary>
    /// Create a copy with the elements piped through a function.
    /// </summary>
    public static Tr[] Map<T, Tr>(this IList<T> xs, Func<T, Tr> item_fun)
    {
        var len = xs.Count;
        var result = new Tr[len];
        for (var i = 0; i < len; i++) result[i] = item_fun(xs[i]);
        return result;
    }

    /// <summary>
    /// Create an array of resulting items by applying a supplied binary function
    /// to corresponding pairs of the supplied ILists.
    /// </summary>
    /// <returns></returns>
    public static Tr[] Map2<T0, T1, Tr>(
            this IList<T0> xs0, IList<T1> xs1, Func<T0, T1, Tr> item0_item1_fun)
    {
        var len = Fun.Min(xs0.Count, xs1.Count);
        var result = new Tr[len];
        for (var i = 0; i < len; i++) result[i] = item0_item1_fun(xs0[i], xs1[i]);
        return result;
    }

    /// <summary>
    /// Create an array of resulting items by applying a supplied ternary function
    /// to corresponding triples of the supplied ILists.
    /// </summary>
    /// <returns></returns>
    public static Tr[] Map3<T0, T1, T2, Tr>(
            this IList<T0> xs0, IList<T1> xs1, IList<T2> xs2, Func<T0, T1, T2, Tr> item0_item1_item2_fun)
    {
        var len = Fun.Min(xs0.Count, xs1.Count, xs2.Count);
        var result = new Tr[len];
        for (var i = 0; i < len; i++)
            result[i] = item0_item1_item2_fun(xs0[i], xs1[i], xs2[i]);
        return result;
    }
    
    /// <summary>
    /// Create a copy with the elements piped through a function.
    /// The function gets the index of the element as a second argument.
    /// </summary>
    public static Tr[] Map<T, Tr>(this IList<T> xs, Func<T, int, Tr> item_index_fun)
    {
        var len = xs.Count;
        var result = new Tr[len];
        for (var i = 0; i < len; i++) result[i] = item_index_fun(xs[i], i);
        return result;
    }

    public static Tr[] Map2<T0, T1, Tr>(
            this IList<T0> xs0, IList<T1> xs1, Func<T0, T1, int, Tr> item0_item1_index_fun)
    {
        var len = Fun.Min(xs0.Count, xs1.Count);
        var result = new Tr[len];
        for (var i = 0; i < len; i++)
            result[i] = item0_item1_index_fun(xs0[i], xs1[i], i);
        return result;
    }

    public static Tr[] Map3<T0, T1, T2, Tr>(
            this IList<T0> xs0, IList<T1> xs1, IList<T2> xs2,
            Func<T0, T1, T2, int, Tr> item0_item1_item2_index_fun)
    {
        var len = Fun.Min(xs0.Count, xs1.Count, xs2.Count);
        var result = new Tr[len];
        for (var i = 0; i < len; i++)
            result[i] = item0_item1_item2_index_fun(xs0[i], xs1[i], xs2[i], i);
        return result;
    }
    
    /// <summary>
    /// Create a copy of count elements with the elements piped through a
    /// function. count may be longer than the input, in this case the
    /// result array has default elements at the end.
    /// </summary>
    public static Tr[] Map<T, Tr>(
            this IList<T> xs, int count, Func<T, Tr> element_fun)
    {
        var result = new Tr[count];
        var len = Math.Min(count, xs.Count);
        for (var i = 0; i < len; i++) result[i] = element_fun(xs[i]);
        return result;
    }
    
    /// <summary>
    /// Create a copy of count elements with the elements piped through a
    /// function. count may be longer than the input, in this case the
    /// result array has default elements at the end.
    /// The function gets the index of the element as a second argument.
    /// </summary>
    public static Tr[] Map<T, Tr>(
            this IList<T> xs, int count, Func<T, int, Tr> element_index_fun)
    {
        var result = new Tr[count];
        var len = Math.Min(count, xs.Count);
        for (var i = 0; i < len; i++) result[i] = element_index_fun(xs[i], i);
        return result;
    }

    /// <summary>
    /// Create a copy of specified length starting at the specified
    /// offset with the elements piped through a function.
    /// </summary>
    public static Tr[] Map<T, Tr>(
            this IList<T> xs, int start, int count, Func<T, Tr> element_fun)
    {
        var result = new Tr[count];
        var len = Math.Min(count, xs.Count - start);
        for (var i = 0; i < len; i++) result[i] = element_fun(xs[start + i]);
        return result;
    }
    
    /// <summary>
    /// Create a copy of specified length starting at the specified
    /// offset with the elements piped through a function.
    /// The function gets the target index of the element as a second
    /// argument.
    /// </summary>
    public static Tr[] Map<T, Tr>(
            this IList<T> xs, int start, int count, Func<T, int, Tr> element_index_fun)
    {
        var result = new Tr[count];
        var len = Math.Min(count, xs.Count - start);
        for (var i = 0; i < len; i++) result[i] = element_index_fun(xs[start + i], i);
        return result;
    }

    /// <summary>
    /// Copy a range of elements to the target array.
    /// </summary>
    public static void CopyTo<T>(this IList<T> xs, int count, T[] target, int targetStart)
    {
        for (var i = 0; i < count; i++)
            target[targetStart + i] = xs[i];
    }

    /// <summary>
    /// Copy a range of elements to the target array.
    /// </summary>
    public static void CopyTo<T>(this IList<T> xs, int start, int count, T[] target, int targetStart)
    {
        for (var i = 0; i < count; i++)
            target[targetStart + i] = xs[start + i];
    }

    /// <summary>
    /// Copies the IList into a list.
    /// </summary>
    public static List<T> CopyToList<T>(this IList<T> xs)
    {
        var result = new List<T>(xs.Count);
        result.AddRange(xs);
        return result;
    }
    
    public static List<Tr> MapToList<T, Tr>(this IList<T> xs, Func<T, Tr> element_fun)
    {
        var count = xs.Count;
        var result = new List<Tr>(count);
        for (int i = 0; i < count; i++) result.Add(element_fun(xs[i]));
        return result;
    }
    
    public static List<Tr> MapToList<T, Tr>(this IList<T> xs, Func<T, int, Tr> item_index_fun)
    {
        var count = xs.Count;
        var result = new List<Tr>(count);
        for (int i = 0; i < count; i++) result.Add(item_index_fun(xs[i], i));
        return result;
    }

    /// <summary>
    /// Copies the specified range of elements to the specified destination
    /// within the same IList.
    /// </summary>
    public static void CopyRange<T>(this IList<T> xs, int start, int count, int targetStart)
    {
        var end = start + count;
        if (targetStart < start || targetStart >= end)
        {
            while (start < end) xs[targetStart++] = xs[start++];
        }
        else
        {
            targetStart += count;
            while (start < end) xs[--targetStart] = xs[--end];
        }
    }

    /// <summary>
    /// Create a copy with the elements reversed. 
    /// </summary>
    public static T[] CopyReversed<T>(this IList<T> xs)
    {
        var result = new T[xs.Count];
        var lastIndex = xs.Count - 1;
        for (var i = 0; i <= lastIndex; i++)
        {
            result[i] = xs[lastIndex - i];
        }
        return result;
    }

    #endregion
}
