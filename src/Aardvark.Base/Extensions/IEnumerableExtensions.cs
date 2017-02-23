using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static class EnumerableUtils
    {
        public static IEnumerable<T> Generate<T>(this Func<T> anotherElement)
        {
            Requires.NotNull(anotherElement);
            while (true) yield return anotherElement();
        }

        public static IEnumerable<Guid> Guids()
        {
            while (true) yield return Guid.NewGuid();
        }
    }

    public static class IEnumerableFun
    {
        public static SymbolSet ToSymbolSet(this IEnumerable<Symbol> symbols)
        {
            return new SymbolSet(symbols);
        }
    }

    public static partial class EnumerableEx
    {
        #region Indexed Values

        public static IEnumerable<IndexedValue<T>> IndexedValues<T>(
                this IEnumerable<T> self)
        {
            return self.Select((item, i) => new IndexedValue<T>(i, item));
        }

        /// <summary>
        /// If you have a sequence of indexed values, where all indexes
        /// starting from 0 up to some maximal value are present and
        /// unique, sorting can be performed this simple array based
        /// reshuffling operation with O(n) cost.
        /// </summary>
        public static T[] SortedByDenseIndex<T>(
                this IEnumerable<IndexedValue<T>> indexedValues)
        {
            var array = indexedValues.ToArray();
            var len = array.Length;
            var sorted = new T[len];
            for (int i = 0; i < len; i++) sorted[array[i].Index] = array[i].Value;
            return sorted;
        }

        /// <summary>
        /// Returns elements with index into 'other'. 
        /// </summary>
        public static IEnumerable<IndexedValue<T>> IndexIntoOther<T>(
                this IEnumerable<T> self, IList<T> other)
        {
            var map = new Dictionary<T, int>();
            for (int i = 0; i < other.Count; i++) map[other[i]] = i;
            return self.Select(item => new IndexedValue<T>(map[item], item));
        }

        #endregion

        #region Comparable Indexed Values

        public static IEnumerable<ComparableIndexedValue<T>> ComparableIndexedValues<T>(
                this IEnumerable<T> self)
            where T : IComparable<T>
        {
            return self.Select((item, i) => new ComparableIndexedValue<T>(i, item));
        }

        #endregion

        #region Special Selects

        public static IEnumerable<T> WhereNotNull<T>(this IEnumerable<T> self)
        {
            return self.Where(x => x != null);
        }

        public static IEnumerable<int> IndicesOf<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            int i = 0;
            foreach (var x in self)
            {
                if (predicate(x))
                    yield return i;
                i++;
            }
        }

        public static IEnumerable<TResult> SelectNotNull<T, TResult>(this IEnumerable<T> self, Func<T, TResult> selector)
        {
            return self.Select(selector).WhereNotNull();
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> self, T defaultValue)
        {
            foreach (var x in self)
                return x;
            return defaultValue;
        }

        public static T FirstOrDefault<T>(this IEnumerable<T> self, Func<T, bool> predicate, T defaultValue)
        {
            foreach (var x in self)
                if (predicate(x))
                    return x;
            return defaultValue;
        }

        /// <summary>
        /// Yields every stride-th element.
        /// </summary>
        public static IEnumerable<T> TakePeriodic<T>(
            this IEnumerable<T> self, int stride)
        {
            Requires.NotNull(self);
            Requires.That(stride >= 1);

            var i = 0;
            foreach (var s in self) if (i++ % stride == 0) yield return s;
        }

        /// <summary>
        /// Take the first count items of a sequence and put them into an
        /// array. If the sequence is shorter than the specified count, the
        /// array is filled up with default values.
        /// </summary>
        public static T[] TakeToArrayDefault<T>(this IEnumerable<T> self, int count)
        {
            var array = new T[count];
            var en = self.GetEnumerator();
            for (int i = 0; i < count && en.MoveNext(); i++)
                array[i] = en.Current;
            return array;
        }

        /// <summary>
        /// Take the first count items of a sequence and put them into an
        /// array. If the sequence is shorter than the specified count, an
        /// Argument exception is thrown.
        /// </summary>
        public static T[] TakeToArray<T>(this IEnumerable<T> self, int count)
        {
            var array = new T[count];
            var en = self.GetEnumerator();
            for (int i = 0; i < count; i++)
            {
                if (en.MoveNext())
                    array[i] = en.Current;
                else
                    throw new ArgumentException();
            }
            return array;
        }

        /// <summary>
        /// Take the first count items of a sequence and put them into a
        /// list. If the supplied sequence has less than count elements
        /// the resulting list is shorter than count items.
        /// </summary>
        public static List<T> TakeToList<T>(this IEnumerable<T> self, int count)
        {
            var list = new List<T>(count);
            var en = self.GetEnumerator();
            for (int i = 0; i < count && en.MoveNext(); i++)
                list.Add(en.Current);
            return list;
        }

        /// <summary>
        /// Returns the index of the first element triggering the condition or -1 as default.
        /// </summary>
        public static int FirstIndexOf<T>(this IEnumerable<T> self, Func<T, bool> where)
        {
            var en = self.GetEnumerator();
            for (int i = 0; en.MoveNext(); i++)
                if (where(en.Current))
                    return i;
            return -1;
        }

        /// <summary>
        /// Maps the elements of an array to a result array.
        /// </summary>
        public static R[] SelectToArray<T, R>(this T[] self, Func<T, R> selector)
        {
            var result = new R[self.Length];
            for (int i = 0; i < self.Length; i++)
            {
                result[i] = selector(self[i]);
            }
            return result;
        }
        
        /// <summary>
        /// Maps the elements of an array to a result list.
        /// </summary>
        public static List<R> SelectToList<T, R>(this T[] self, Func<T, R> selector)
        {
            var result = new List<R>(self.Length);
            for (int i = 0; i < self.Length; i++)
            {
                result[i] = selector(self[i]);
            }
            return result;
        }

        /// <summary>
        /// Maps the elements of a list to a result array.
        /// </summary>
        public static R[] SelectToArray<T, R>(this List<T> self, Func<T, R> selector)
        {
            var result = new R[self.Count];
            for (int i = 0; i < self.Count; i++)
            {
                result[i] = selector(self[i]);
            }
            return result;
        }

        /// <summary>
        /// Maps the elements of a list to a result list.
        /// </summary>
        public static List<R> SelectToList<T, R>(this List<T> self, Func<T, R> selector)
        {
            var result = new List<R>(self.Count);
            for (int i = 0; i < self.Count; i++)
            {
                result[i] = selector(self[i]);
            }
            return result;
        }

        #endregion

        #region Zipping

        /// <summary>
        /// Interleave this' elements with other's elements resulting in
        /// a sequence of length min(this.Length, other.Length).
        /// </summary>
        public static IEnumerable<T> Zip<T>(
            this IEnumerable<T> self, IEnumerable<T> other)
        {
            Requires.NotNull(self);
            Requires.NotNull(other);

            var e0 = self.GetEnumerator();
            var e1 = other.GetEnumerator();

            while (e0.MoveNext() && e1.MoveNext())
            {
                yield return e0.Current;
                yield return e1.Current;
            }
        }

        public static IEnumerable<T> Zip<T>(
            this IEnumerable<T> self,
            IEnumerable<T> other1, IEnumerable<T> other2)
        {
            Requires.NotNull(self);
            Requires.NotNull(other1);
            Requires.NotNull(other2);

            var e0 = self.GetEnumerator();
            var e1 = other1.GetEnumerator();
            var e2 = other2.GetEnumerator();

            while (e0.MoveNext() && e1.MoveNext() && e2.MoveNext())
            {
                yield return e0.Current;
                yield return e1.Current;
                yield return e2.Current;
            }
        }

        public static IEnumerable<T> ZipAll<T>(
            this IEnumerable<T> self, IEnumerable<T> other)
        {
            Requires.NotNull(self);
            Requires.NotNull(other);

            var e0 = self.GetEnumerator();
            var e1 = other.GetEnumerator();

            bool t0 = e0.MoveNext(), t1 = e1.MoveNext();

            while (t0 && t1)
            {
                yield return e0.Current; yield return e1.Current;
                t0 = e0.MoveNext(); t1 = e1.MoveNext();
            }

            while (t0) { yield return e0.Current; t0 = e0.MoveNext(); }
            while (t1) { yield return e1.Current; t1 = e1.MoveNext(); }
        }

        public static IEnumerable<T> ZipAll<T>(
            this IEnumerable<T> self,
            IEnumerable<T> other1,
            IEnumerable<T> other2)
        {
            Requires.NotNull(self);
            Requires.NotNull(other1);
            Requires.NotNull(other2);

            var e0 = self.GetEnumerator();
            var e1 = other1.GetEnumerator();
            var e2 = other2.GetEnumerator();

            bool t0 = e0.MoveNext(), t1 = e1.MoveNext(), t2 = e2.MoveNext();

            while (t0 && t1 && t2)
            {
                yield return e0.Current;
                yield return e1.Current;
                yield return e2.Current;
                t0 = e0.MoveNext(); t1 = e1.MoveNext(); t2 = e2.MoveNext();
            }

            while (t0 && t1)
            {
                yield return e0.Current; yield return e1.Current;
                t0 = e0.MoveNext(); t1 = e1.MoveNext();
            }

            while (t0 && t2)
            {
                yield return e0.Current; yield return e2.Current;
                t0 = e0.MoveNext(); t2 = e2.MoveNext();
            }

            while (t1 && t2)
            {
                yield return e1.Current; yield return e2.Current;
                t1 = e1.MoveNext(); t2 = e2.MoveNext();
            }

            while (t0) { yield return e0.Current; t0 = e0.MoveNext(); }
            while (t1) { yield return e1.Current; t1 = e1.MoveNext(); }
            while (t2) { yield return e2.Current; t2 = e2.MoveNext(); }
        }

        public static IEnumerable<Pair<T>> ZipPairs<T>(
            this IEnumerable<T> self, IEnumerable<T> other)
        {
            Requires.NotNull(self);
            Requires.NotNull(other);

            var e0 = self.GetEnumerator();
            var e1 = other.GetEnumerator();

            while (e0.MoveNext() && e1.MoveNext())
                yield return new Pair<T>(e0.Current, e1.Current);
        }

        public static IEnumerable<Triple<T>> ZipTriples<T>(
            this IEnumerable<T> self,
            IEnumerable<T> other1,
            IEnumerable<T> other2)
        {
            Requires.NotNull(self);
            Requires.NotNull(other1);
            Requires.NotNull(other2);

            var e0 = self.GetEnumerator();
            var e1 = other1.GetEnumerator();
            var e2 = other2.GetEnumerator();

            while (e0.MoveNext() && e1.MoveNext() && e2.MoveNext())
                yield return new Triple<T>(e0.Current, e1.Current, e2.Current);
        }

        public static IEnumerable<Tup<T0, T1>> ZipTuples<T0, T1>(
            this IEnumerable<T0> self, IEnumerable<T1> other)
        {
            Requires.NotNull(self);
            Requires.NotNull(other);

            var e0 = self.GetEnumerator();
            var e1 = other.GetEnumerator();

            while (e0.MoveNext() && e1.MoveNext())
                yield return new Tup<T0, T1>(e0.Current, e1.Current);
        }

        public static IEnumerable<Tup<T0, T1, T2>> ZipTuples<T0, T1, T2>(
            this IEnumerable<T0> self,
            IEnumerable<T1> other1,
            IEnumerable<T2> other2)
        {
            Requires.NotNull(self);
            Requires.NotNull(other1);
            Requires.NotNull(other2);

            var e0 = self.GetEnumerator();
            var e1 = other1.GetEnumerator();
            var e2 = other2.GetEnumerator();

            while (e0.MoveNext() && e1.MoveNext() && e2.MoveNext())
                yield return new Tup<T0, T1, T2>(
                                    e0.Current, e1.Current, e2.Current);
        }

        #endregion

        #region Chunking

        public static IEnumerable<T[]> Chunk<T>(
            this IEnumerable<T> self, long chunkSize
            )
        {
            Requires.NotNull(self);
            Requires.That(chunkSize >= 1);

            var chunk = new List<T>();

            foreach (var item in self)
            {
                chunk.Add(item);
                if (chunk.Count == chunkSize) // yield full chunk
                {
                    yield return chunk.ToArray();
                    chunk.Clear();
                }
            }

            // yield rest
            if (chunk.Count > 0) yield return chunk.ToArray();

        }

        public static IEnumerable<Tr[]> Chunk<T, Tr>(
            this IEnumerable<T> self, long chunkSize, Func<T, Tr> selector
            )
        {
            Requires.NotNull(self);
            Requires.That(chunkSize >= 1);
            Requires.NotNull(selector);

            var chunk = new List<Tr>();

            foreach (var item in self)
            {
                chunk.Add(selector(item));
                if (chunk.Count == chunkSize) // yield full chunk
                {
                    yield return chunk.ToArray();
                    chunk.Clear();
                }
            }

            // yield rest
            if (chunk.Count > 0) yield return chunk.ToArray();

        }

        #endregion

        #region Pairs

        /// <summary>
        /// A B C D ... -> (A, B) (C, D) ...
        /// If sequence does not have even elements the last element is not selected.
        /// </summary>
        public static IEnumerable<Pair<T>> PairSequence<T>(this IEnumerable<T> self)
        {
            Requires.NotNull(self);

            var first = self.TakePeriodic(2);
            var second = self.Skip(1).TakePeriodic(2);

            return first.ZipPairs(second); //todo: very inefficient: double enumeration
        }

        /// <summary>
        /// wrap = false: A B C D ... -> (A, B) (B, C) (C, D) ...
        /// wrap = true:  A B C D -> (A, B) (B, C) (C, D) (D, A)
        /// </summary>
        public static IEnumerable<Pair<T>> PairChain<T>(this IEnumerable<T> self, bool wrap = false)
        {
            Requires.NotNull(self);

            using (var e = self.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    T first = e.Current;
                    T prev = first;
                    while (e.MoveNext())
                    {
                        var x = e.Current;
                        yield return new Pair<T>(prev, x);
                        prev = x;
                    }
                    if (wrap)
                        yield return new Pair<T>(prev, first);
                }
            }
        }

        /// <summary>
        /// wrap = false: A B C D ... -> (0, 1) (1, 2) (2, 3) ...
        /// wrap = true:  A B C D -> (0, 1) (1, 2) (2, 3) (3, 0)
        /// </summary>
        public static IEnumerable<Pair<int>> PairChainIndexed<T>(this IEnumerable<T> self, bool wrap = false)
        {
            Requires.NotNull(self);

            using (var e = self.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    int prev = 0;
                    while (e.MoveNext())
                    {
                        var x = prev + 1;
                        yield return new Pair<int>(prev, x);
                        prev = x;
                    }
                    if (wrap)
                        yield return new Pair<int>(prev, 0);
                }
            }
        }

        /// <summary>
        /// A B C D -> (A, B) (B, C) (C, D) (D, A)
        /// </summary>
        public static IEnumerable<Pair<T>> PairChainWrap<T>(this IEnumerable<T> self)
        {
            return PairChain(self, true);
        }

        /// <summary>
        /// A B C -> (0, 1) (1, 2) (2, 0)
        /// </summary>
        public static IEnumerable<Pair<int>> PairChainWrapIndexed<T>(this IEnumerable<T> self)
        {
            return PairChainIndexed(self, true);
        }

        /// <summary>
        /// A B C -> (A, A) (A, B) (A, C) (B, A) (B, B) (B, C) (C, A) (C, B) (C, C)
        /// </summary>
        public static IEnumerable<Pair<T>> Pairs<T>(this IEnumerable<T> self)
        {
            return Pairs(self, false, false);
        }

        /// <summary>
        /// A B C -> (A, A) (A, B) (A, C) (B, A) (B, B) (B, C) (C, A) (C, B) (C, C)
        /// excludeIdenticalPairs: A B C -> (A, B) (A, C) (B, A) (B, C) (C, A) (C, B)
        /// excludeReversePairs:   A B C -> (A, A) (A, B) (A, C) (B, B) (B, C) (C, C)
        /// both:                  A B C -> (A, B) (A, C) (B, C)
        /// </summary>
        public static IEnumerable<Pair<T>> Pairs<T>(this IEnumerable<T> self, bool excludeIdenticalPairs, bool excludeReversePairs)
        {
            Requires.NotNull(self);

            var a = self.ToArray();
            for (int i = 0; i < a.Length; i++)
                for (int j = excludeReversePairs ? i : 0; j < a.Length; j++)
                {
                    if (excludeIdenticalPairs && i == j) continue;
                    yield return Pair.Create(a[i], a[j]);
                }
        }

        /// <summary>
        /// Computes how many pairs method Pairs() will generate.
        /// Is efficient on ILists(of T), but will fully enumerate other IEnumerables.
        /// </summary>
        public static int PairsCount<T>(this IEnumerable<T> self, bool excludeIdenticalPairs, bool excludeReversePairs)
        {
            Requires.NotNull(self);

            var c = self.Count();
            if(excludeIdenticalPairs)
            {
                if (excludeReversePairs)
                    return (c * (c - 1)) / 2;
                else
                    return c * (c - 1);
            }
            else
            {
                if (excludeReversePairs)
                    return ((c + 1) * c) / 2;
                else
                    return c * c;
            }
        }

        /// <summary>
        /// [A B C].Pair([x y z]) -> (A, x) (A, y) (A, z) (B, x) (B, y) (B, z) (C, x) (C, y) (C, z)
        /// </summary>
        public static IEnumerable<Pair<T>> Pairs<T>(this IEnumerable<T> self, IEnumerable<T> other)
        {
            Requires.NotNull(self);
            Requires.NotNull(other);

            var a = self.ToArray();
            var b = other.ToArray();
            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < b.Length; j++)
                    yield return Pair.Create(a[i], b[j]);
        }

        #endregion

        #region Triples

        /// <summary>
        /// A B C D ... -> (A, B, C) (D, E, F) ...
        /// If sequence does not have even elements the last element is not selected.
        /// </summary>
        public static IEnumerable<Triple<T>> TripleSequence<T>(this IEnumerable<T> self)
        {
            Requires.NotNull(self);

            var first = self.TakePeriodic(3);
            var second = self.Skip(1).TakePeriodic(3);
            var third = self.Skip(2).TakePeriodic(3);

            return first.ZipTriples(second, third); //todo: very inefficient: triple enumeration
        }

        /// <summary>
        /// wrap = false: A B C D ... -> (A, B, C) (B, C, D) (C, D, E) ...
        /// wrap = true:  A B C D -> (A, B, C) (B, C, D) (C, D, A) (D, A, B)
        /// </summary>
        public static IEnumerable<Triple<T>> TripleChain<T>(this IEnumerable<T> self, bool wrap = false)
        {
            Requires.NotNull(self);

            using (var e = self.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    T first = e.Current;
                    if (e.MoveNext())
                    {
                        T second = e.Current;
                        T prev1 = first;
                        T prev2 = second;
                        while (e.MoveNext())
                        {
                            var x = e.Current;
                            yield return new Triple<T>(prev1, prev2, x);
                            prev1 = prev2;
                            prev2 = x;
                        }
                        if (wrap)
                        {
                            yield return new Triple<T>(prev1, prev2, first);
                            yield return new Triple<T>(prev2, first, second);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// A B C D -> (A, B, C) (B, C, D) (C, D, A) (D, A, B)
        /// </summary>
        public static IEnumerable<Triple<T>> TripleChainWrap<T>(this IEnumerable<T> self)
        {
            return TripleChain(self, true);
        }

        /// <summary>
        /// A B C -> AAA, AAB, AAC, ABA, ABB, ABC, ACA, ACB, ACC, BAA, BAB, BAC, BBA, BBB, BBC, BCA, BCB, BCC, CAA, CAB, CAC, CBA, CBB, CBC, CCA, CCB, CCC
        /// </summary>
        public static IEnumerable<Triple<T>> Triples<T>(this IEnumerable<T> self)
        {
            return Triples(self, false, false);
        }

        /// <summary>
        /// A B C -> AAA, AAB, AAC, ABA, ABB, ABC, ACA, ACB, ACC, BAA, BAB, BAC, BBA, BBB, BBC, BCA, BCB, BCC, CAA, CAB, CAC, CBA, CBB, CBC, CCA, CCB, CCC
        /// excludeIdenticalPairs: A B C -> ABC, CBA
        /// excludeReversePairs:   A B C -> AAA, AAB, AAC, ABA, ABB, ABC, ACA, ACB, ACC, BAB, BAC, BBB, BBC, BCB, BCC, CAC, CBC, CCC
        /// both:                  A B C -> ABC
        /// </summary>
        public static IEnumerable<Triple<T>> Triples<T>(this IEnumerable<T> self, bool excludeIdenticalPairs, bool excludeReversePairs)
        { //todo: check implementation; + what about excludePermutations? reversePairs do not extend to triples.
            Requires.NotNull(self);

            var a = self.ToArray();
            for (int i = 0; i < a.Length; i++)
                for (int j = excludeReversePairs ? i : 0; j < a.Length; j++)
                {
                    if (excludeIdenticalPairs && i == j) continue;
                    for (int k = excludeReversePairs ? j : 0; k < a.Length; k++)
                    {
                        if (excludeIdenticalPairs && (j == k || i == k)) continue;
                        yield return new Triple<T>(a[i], a[j], a[k]);
                    }
                }
        }
/*
        /// <summary>
        /// Computes how many triples method Triples() will generate.
        /// Is efficient on ILists(of T), but will fully enumerate other IEnumerables.
        /// </summary>
        public static int TripleCount<T>(this IEnumerable<T> self, bool excludeIdenticalPairs, bool excludeReversePairs)
        {
            Requires.NotNull(self);

            var c = self.Count();
            if (excludeIdenticalPairs)
            {
                if (excludeReversePairs)
                    return (c * (c - 1) * (c - 2)) / 6;
                else
                    return c * (c - 1) * (c - 2);
            }
            else
            {
                if (excludeReversePairs)
                    throw new NotImplementedException(); //todo: return ((c + 1) * c) / 2;
                else
                    return c * c * c;
            }
        }
*/
        /// <summary>
        /// [A B].Triple([x y], [X Y]) -> (A, x, X) (A, x, Y) (A, y, X) (A, y, Y) (B, x, X) (B, x, Y) (B, y, X) (B, y, Y) (C, x, X) (C, x, Y) (C, y, X) (C, y, Y)
        /// </summary>
        public static IEnumerable<Triple<T>> Triples<T>(this IEnumerable<T> self, IEnumerable<T> other1, IEnumerable<T> other2)
        {
            Requires.NotNull(self);
            Requires.NotNull(other1);
            Requires.NotNull(other2);

            var a = self.ToArray();
            var b = other1.ToArray();
            var c = other2.ToArray();
            for (int i = 0; i < a.Length; i++)
                for (int j = 0; j < b.Length; j++)
                    for (int k = 0; k < c.Length; k++)
                        yield return new Triple<T>(a[i], b[j], c[k]);
        }

        #endregion

        #region Repetition

        /// <summary>
        /// Returns an enumeration which will infinitely yield copies of 'self'.
        /// A.Repeat -> [A A A A ...]
        /// </summary>
        public static IEnumerable<T> Repeat<T>(this T self)
        {
            while (true) yield return self;
        }

        /// <summary>
        /// Returns an enumeration which will yield 'count' copies of 'self'.
        /// A.Repeat(3) -> [A A A]
        /// </summary>
        public static IEnumerable<T> Repeat<T>(this T self, long count)
        {
            Requires.That(count >= 0);

            for (long i = 0; i < count; i++) yield return self;
        }

        /// <summary>
        /// Appends an infinite number of copies of the sequence's last element.
        /// [A B C].WithRepeatedLast() -> [A B C C C C C ...]
        /// Does nothing for an empty sequence.
        /// Throws an exception for a null sequence.
        /// </summary>
        public static IEnumerable<T> WithRepeatedLast<T>(this IEnumerable<T> self)
        {
            Requires.NotNull(self);
            if (self.IsEmptyOrNull()) yield break;

            T last = default(T);
            foreach (var x in self) yield return last = x;
            while (true) yield return last;
        }

        /// <summary>
        /// Duplicates each element of the sequence.
        /// [A B C].DupElements() -> [A A B B C C]
        /// </summary>
        public static IEnumerable<T> Dup<T>(this IEnumerable<T> self)
        {
            Requires.NotNull(self);

            foreach (var x in self) { yield return x; yield return x; }
        }
        
        /// <summary>
        /// Duplicates each element of the sequence n times.
        /// [A B C].DupElements(4) -> [A A A A B B B B C C C C]
        /// </summary>
        public static IEnumerable<T> Dup<T>(this IEnumerable<T> self, int n)
        {
            Requires.NotNull(self);
            Requires.That(n >= 0);

            foreach (var x in self)
                for (int i = 0; i < n; i++)
                    yield return x;
        }

        #endregion

        #region Reordering

        /// <summary>
        /// Moves the first element to the end of the sequence.
        /// [A B C].AddFirstToEnd() -> [A B C A]
        /// </summary>
        public static IEnumerable<T> AddFirstToEnd<T>(this IEnumerable<T> self)
        {
            Requires.NotNull(self);

            if (self.IsEmpty()) yield break;

            var first = self.First();
            foreach (var x in self)
            {
                yield return x;
            }
            yield return first;
        }

        /// <summary>
        /// Moves the first element to the end of the sequence.
        /// [A B C D E].MoveFirstToEnd() -> [B C D E A]
        /// </summary>
        public static IEnumerable<T> WithFirstMovedToEnd<T>(this IEnumerable<T> self)
        {
            Requires.NotNull(self);

            if (self.IsEmpty()) return self;
            return self.Skip(1).Concat(self.Take(1));
        }

        /// <summary>
        /// Interleaves given IEnumerable with specified element.
        /// E.g. { 'A', 'B', 'C' }.Interleave('X') -> { 'A', 'X', 'B', 'X', 'C' }.
        /// </summary>
        public static IEnumerable<T> Interleave<T>(
            this IEnumerable<T> self, T separator)
        {
            Requires.NotNull(self);

            bool notFirst = false;
            foreach (var x in self)
            {
                if (notFirst) yield return separator; else notFirst = true;
                yield return x;
            }
        }

        #endregion

        #region Grouping

        /// <summary>
        /// Groups enumeration by an array of keys.
        /// </summary>
        public static IEnumerable<TElement> FlatGroupByMany<TElement>(
            this IEnumerable<TElement> elements,
            params Func<TElement, object>[] groupSelectorFuncs)
        {
            Requires.NotNull(elements);
            Requires.NotNull(groupSelectorFuncs);

            IEnumerable<TElement> result = elements;
            foreach (var selectorFunc in groupSelectorFuncs.Reverse())
            {
                Requires.NotNull(selectorFunc);
                result = from g in
                             (from item in result group item by selectorFunc(item))
                         from e in g
                         select e;
            }
            return result;
        }

        #endregion

        #region All

        public static bool AllDistinct<T>(this IEnumerable<T> elements)
        {
            if (elements == null)
                return true;
            //if (elements.IsEmptyOrNull()) return true; //this performs an unnecessary enumeration (at least of the first element)
            return elements.Distinct().Count() == elements.Count();
        }

        public static bool AllEqual<T>(this IEnumerable<T> elements)
        {
            if (elements == null)
                return true;
            //if (elements.IsEmptyOrNull()) return true; //this performs an unnecessary enumeration (at least of the first element)
            return elements.Distinct().Count() <= 1;
        }

        #endregion

        #region Comparisons

        /// <summary>
        /// Implemented in System.Interactive.dll.
        /// Returns true if elements contains no items or if elements is null,
        /// false otherwise.
        /// </summary>
        public static bool IsEmptyOrNull<T>(this IEnumerable<T> elements)
        {
            if (elements == null) return true;
            return elements.Take(1).Count() == 0;
        }

        public static IEnumerable<T> NonNull<T>(IEnumerable<T> elements)
        {
            if (elements == null) return Enumerable.Empty<T>();
            return elements;
        }

        public static bool SetEquals<T>(this IEnumerable<T> self, IEnumerable<T> other)
        {
            if (self == null && other != null) return false;
            if (self != null && other == null) return false;
            if (self.Count() != other.Count()) return false;
            if (self.Count() != self.Distinct().Count()) throw new Exception("not a proper set");
            if (other.Count() != other.Distinct().Count()) throw new Exception("not a proper set");

            var tmp = new Dictionary<T, T>();
            tmp.AddRange(self.Select(x => new KeyValuePair<T, T>(x, x)));
            foreach (var x in other) if (!tmp.ContainsKey(x)) return false;

            return true;
        }

        /// <summary>
        /// Compares both enumerables using SequenceEqual and additionally checks
        /// whether both enumerables are null.
        /// </summary>
        public static bool EnumerableEquals<T>(this IEnumerable<T> self, IEnumerable<T> other)
        {
            if (self == null && other == null)
                return true;
            if (self != null && other == null)
                return false;
            if (self != null && self.SequenceEqual(other))
                return true;
            else
                return false;
        }

        #endregion

        #region Min/Max/Most

        public static T Min<T>(this IEnumerable<T> seq, Func<T, T, bool> lessThan)
        {
            Requires.NotNull(seq);
            Requires.NotNull(lessThan);
            Requires.That(!seq.IsEmptyOrNull());

            var min = seq.First();
            foreach (var x in seq.Skip(1))
                if (lessThan(x, min)) min = x;
            return min;
        }

        public static T Max<T>(this IEnumerable<T> seq, Func<T, T, bool> greaterThan)
        {
            Requires.NotNull(seq);
            Requires.NotNull(greaterThan);
            Requires.That(!seq.IsEmptyOrNull());

            var max = seq.First();
            foreach (var x in seq.Skip(1))
                if (greaterThan(x, max)) max = x;
            return max;
        }

        /// <summary>
        /// Aardvark.Runtime.IEnumerableExtensions:
        /// No-Throw version. Finds the smallest element in seq according to lessThan, that is smaller than maxValue, or the first such element if there are equally small elements, or maxValue if no element is smaller.
        /// For normal operation set maxValue = +Inf or the maximum of T.
        /// </summary>
        public static T Min<T>(this IEnumerable<T> seq, Func<T, T, bool> lessThan, T maxValue)
        {
            var min = maxValue;
            foreach (var x in seq)
                if (lessThan(x, min)) min = x;
            return min;
        }

        /// <summary>
        /// Aardvark.Runtime.IEnumerableExtensions:
        /// No-Throw version. Finds the largest element in seq according to greaterThan, that is greater than minValue, or the first such element if there are equally large elements, or minValue if no element is greater.
        /// For normal operation set minValue = -Inf or the minimum of T.
        /// </summary>
        public static T Max<T>(this IEnumerable<T> seq, Func<T, T, bool> greaterThan, T minValue)
        {
            var max = minValue;
            foreach (var x in seq)
                if (greaterThan(x, max)) max = x;
            return max;
        }

        /// <summary>
        /// Aardvark.Runtime.IEnumerableExtensions:
        /// Invokes a transform function on each element of a sequence and 
        /// returns the element that yielded the maximum value (and the maximum value in rv_maxVal).
        /// Only elements that yield larger values than minVal are considered.
        /// If multiple elements yield the maximum value, the first in the sequence is chosen.
        /// </summary>
        /// <typeparam name="TSrc">The type of the elements of source.</typeparam>
        /// <typeparam name="TDst">The type in which the elements are to be transformed before being compared.</typeparam>
        /// <param name="source">A sequence of values to determine the maximum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="greaterThan">A function how to compare elements of type TDst. greaterThan(a,b) => a>b (Sorry, is needed for general operation without providing a function for all types.)</param>
        /// <param name="minVal">An element in the sequence has to yield a value larger than this to be considered.</param>
        /// <param name="defaultRv">The return value if no element is greater than minVal, or the source is empty.</param>
        /// <param name="rv_maxVal">The maximum value found, or minVal if no element yielded a larger value.</param>
        /// <returns>The element that yielded the maximum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">source, selector or greaterThan is null.</exception>
        public static TSrc MaxElement<TSrc, TDst>(this IEnumerable<TSrc> source, Func<TSrc, TDst> selector, Func<TDst, TDst, bool> greaterThan, TDst minVal, TSrc defaultRv, out TDst rv_maxVal)
        {
            if (source == null) throw new ArgumentNullException();
            if (selector == null) throw new ArgumentNullException();
            if (greaterThan == null) throw new ArgumentNullException();
            var e = source.GetEnumerator();
            TSrc maxEl = defaultRv;
            rv_maxVal = minVal;
            while (e.MoveNext())
            {
                TSrc el = e.Current;
                TDst val = selector(el);
                if (greaterThan(val, rv_maxVal))
                {
                    rv_maxVal = val;
                    maxEl = el;
                }
            }
            return maxEl;
        }

        /// <summary>
        /// Aardvark.Runtime.IEnumerableExtensions:
        /// Invokes a transform function on each element of a sequence and 
        /// returns the element that yielded the minimum value (and the minimum value in rv_minVal).
        /// Only elements that yield smaller values than maxVal are considered.
        /// If multiple elements yield the minimum value, the first in the sequence is chosen.
        /// </summary>
        /// <typeparam name="TSrc">The type of the elements of source.</typeparam>
        /// <typeparam name="TDst">The type in which the elements are to be transformed before being compared.</typeparam>
        /// <param name="source">A sequence of values to determine the minimum value of.</param>
        /// <param name="selector">A transform function to apply to each element.</param>
        /// <param name="smallerThan">A function how to compare elements of type TDst. smallerThan(a,b) => a&lt;b (Sorry, is needed for general operation without providing a function for all types.)</param>
        /// <param name="maxVal">An element in the sequence has to yield a value smaller than this to be considered.</param>
        /// <param name="defaultRv">The return value if no element is smaller than maxVal, or the source is empty.</param>
        /// <param name="rv_minVal">The minimum value found, or maxVal if no element yielded a larger value.</param>
        /// <returns>The element that yielded the minimum value in the sequence.</returns>
        /// <exception cref="System.ArgumentNullException">source, selector or smallerThan is null.</exception>
        public static TSrc MinElement<TSrc, TDst>(this IEnumerable<TSrc> source, Func<TSrc, TDst> selector, Func<TDst, TDst, bool> smallerThan, TDst maxVal, TSrc defaultRv, out TDst rv_minVal)
        {
            if (source == null) throw new ArgumentNullException();
            if (selector == null) throw new ArgumentNullException();
            if (smallerThan == null) throw new ArgumentNullException();
            var e = source.GetEnumerator();
            TSrc minEl = defaultRv;
            rv_minVal = maxVal;
            while (e.MoveNext())
            {
                TSrc el = e.Current;
                TDst val = selector(el);
                if (smallerThan(val, rv_minVal))
                {
                    rv_minVal = val;
                    minEl = el;
                }
            }
            return minEl;
        }

        /// <summary>
        /// Returns true if more elements than not satisfy the given predicate.
        /// </summary>
        public static bool Most<T>(this IEnumerable<T> self, Func<T, bool> predicate)
        {
            Contract.Requires(self != null);
            long countTrue = 0;
            long countFalse = 0;
            foreach (var x in self)
                if (predicate(x)) countTrue++; else countFalse++;
            return countTrue > countFalse;
        }

        #endregion

        #region Min/Max-Index

        public static int MinIndex<T>(this IEnumerable<T> self, Func<T, T, bool> lessThan, out T minValue)
        {
            var e = self.GetEnumerator();
            if (!e.MoveNext())
            {
                minValue = default(T);
                return -1;
            }

            var currentMinValue = e.Current;
            var currentMinIndex = 0;
            var index = 0;
            while (e.MoveNext())
            {
                if (lessThan(e.Current, currentMinValue))
                {
                    currentMinValue = e.Current;
                    currentMinIndex = index;
                }
                index++;
            }

            minValue = currentMinValue;
            return currentMinIndex;
        }

        public static int MaxIndex<T>(this IEnumerable<T> self, Func<T, T, bool> greaterThan, out T maxValue)
        {
            var e = self.GetEnumerator();
            if (!e.MoveNext())
            {
                maxValue = default(T);
                return -1;
            }

            var currentMaxValue = e.Current;
            var currentMaxIndex = 0;
            var index = 0;
            while (e.MoveNext())
            {
                if (greaterThan(e.Current, currentMaxValue))
                {
                    currentMaxValue = e.Current;
                    currentMaxIndex = index;
                }
                index++;
            }

            maxValue = currentMaxValue;
            return currentMaxIndex;
        }

        public static int MinIndex<T>(this IEnumerable<T> self, out T minValue)
            where T : IComparable<T>
        {
            return MinIndex(self, (a, b) => a.CompareTo(b) < 0, out minValue);
        }

        public static int MaxIndex<T>(this IEnumerable<T> self, out T minValue)
            where T : IComparable<T>
        {
            return MaxIndex(self, (a, b) => a.CompareTo(b) > 0, out minValue);
        }

        public static int MinIndex<T>(this IEnumerable<T> self)
           where T : IComparable<T>
        {
            T foo;
            return MinIndex(self, (a, b) => a.CompareTo(b) < 0, out foo);
        }

        public static int MaxIndex<T>(this IEnumerable<T> self)
            where T : IComparable<T>
        {
            T foo;
            return MaxIndex(self, (a, b) => a.CompareTo(b) > 0, out foo);
        }


        public static int MinIndex<T>(this T[] self, Func<T, T, bool> lessThan, out T minValue)
        {
            if (self.Length <= 0)
            {
                minValue = default(T);
                return -1;
            }

            var currentMinValue = self[0];
            var currentMinIndex = 0;
            for(int i = 1; i < self.Length; i++)
            {
                if (lessThan(self[i], currentMinValue))
                {
                    currentMinValue = self[i];
                    currentMinIndex = i;
                }
            }

            minValue = currentMinValue;
            return currentMinIndex;
        }

        public static int MaxIndex<T>(this T[] self, Func<T, T, bool> greaterThan, out T maxValue)
        {
            if (self.Length <= 0)
            {
                maxValue = default(T);
                return -1;
            }

            var currentMaxValue = self[0];
            var currentMaxIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (greaterThan(self[i], currentMaxValue))
                {
                    currentMaxValue = self[i];
                    currentMaxIndex = i;
                }
            }

            maxValue = currentMaxValue;
            return currentMaxIndex;
        }

        public static int MinIndex<T>(this T[] self, out T minValue)
            where T : IComparable<T>
        {
            return MinIndex(self, (a, b) => a.CompareTo(b) < 0, out minValue);
        }

        public static int MaxIndex<T>(this T[] self, out T maxValue)
            where T : IComparable<T>
        {
            return MaxIndex(self, (a, b) => a.CompareTo(b) > 0, out maxValue);
        }

        public static int MinIndex<T>(this T[] self)
            where T : IComparable<T>
        {
            T foo;
            return MinIndex(self, (a, b) => a.CompareTo(b) < 0, out foo);
        }

        public static int MaxIndex<T>(this T[] self)
            where T : IComparable<T>
        {
            T foo;
            return MaxIndex(self, (a, b) => a.CompareTo(b) > 0, out foo);
        }


        public static int MinIndex<T>(this List<T> self, Func<T, T, bool> lessThan, out T minValue)
        {
            if (self.Count <= 0)
            {
                minValue = default(T);
                return -1;
            }

            var currentMinValue = self[0];
            var currentMinIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (lessThan(self[i], currentMinValue))
                {
                    currentMinValue = self[i];
                    currentMinIndex = i;
                }
            }

            minValue = currentMinValue;
            return currentMinIndex;
        }

        public static int MaxIndex<T>(this List<T> self, Func<T, T, bool> greaterThan, out T maxValue)
        {
            if (self.Count <= 0)
            {
                maxValue = default(T);
                return -1;
            }

            var currentMaxValue = self[0];
            var currentMaxIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (greaterThan(self[i], currentMaxValue))
                {
                    currentMaxValue = self[i];
                    currentMaxIndex = i;
                }
            }

            maxValue = currentMaxValue;
            return currentMaxIndex;
        }

        public static int MinIndex<T>(this List<T> self, out T minValue)
            where T : IComparable<T>
        {
            return MinIndex(self, (a, b) => a.CompareTo(b) < 0, out minValue);
        }

        public static int MaxIndex<T>(this List<T> self, out T maxValue)
            where T : IComparable<T>
        {
            return MaxIndex(self, (a, b) => a.CompareTo(b) > 0, out maxValue);
        }

        public static int MinIndex<T>(this List<T> self)
            where T : IComparable<T>
        {
            T foo;
            return MinIndex(self, (a, b) => a.CompareTo(b) < 0, out foo);
        }

        public static int MaxIndex<T>(this List<T> self)
            where T : IComparable<T>
        {
            T foo;
            return MaxIndex(self, (a, b) => a.CompareTo(b) > 0, out foo);
        }

        #region Specializations

        public static int MinIndex(this byte[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this sbyte[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this short[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this ushort[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this int[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this uint[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this long[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this ulong[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this float[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this double[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this decimal[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        public static int MaxIndex(this byte[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this sbyte[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this short[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this ushort[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this int[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this uint[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this long[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this ulong[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this float[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this double[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this decimal[] self)
        {
            if (self == null || self.Length == 0) throw new ArgumentException();
            if (self.Length == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Length; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        public static int MinIndex(this List<byte> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<sbyte> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<short> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<ushort> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<int> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<uint> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<long> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<ulong> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<float> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<double> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MinIndex(this List<decimal> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] < bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        public static int MaxIndex(this List<byte> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<sbyte> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<short> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<ushort> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<int> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<uint> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<long> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<ulong> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<float> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<double> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }
        public static int MaxIndex(this List<decimal> self)
        {
            if (self == null || self.Count == 0) throw new ArgumentException();
            if (self.Count == 1) return 0;
            var bestValue = self[0];
            var bestIndex = 0;
            for (int i = 1; i < self.Count; i++)
            {
                if (self[i] > bestValue)
                {
                    bestValue = self[i];
                    bestIndex = i;
                }
            }
            return bestIndex;
        }

        #endregion

        #endregion

        #region Predicates

        public static bool TrueForAny<T>(
                this IEnumerable<T> sequence, Func<T, bool> predicate)
        {
            foreach (var item in sequence)
                if (predicate(item)) return true;
            return false;
        }

        public static bool TrueForAll<T>(
                this IEnumerable<T> sequence, Func<T, bool> predicate)
        {
            foreach (var item in sequence)
                if (!predicate(item)) return false;
            return true;
        }

        #endregion


        #region Conversions

        public static T[] ToArrayDebug<T>(this IEnumerable<T> self)
        {
            Requires.NotNull(self);

            var array = self.ToArray();
            return array;
        }

        /// <summary>
        /// Outputs the enumeration in the form "(begin)element0(between)element1[...](end)".
        /// Each element is formatted by the delegate format.
        /// You may set any parameter to null, in which case the default values
        /// (v => v.ToString(), "", ", ", "") are used.
        /// </summary>
        public static string ToString<T>(
            this IEnumerable<T> self, Func<T, string> format, string begin = null, string between = null, string end = null)
        {
            Requires.NotNull(self);
            if(format==null)
                format = v => v == null ? "" : v.ToString();
            if (begin == null)
                begin = "";
            if (between == null)
                between = ", ";
            if (end == null)
                end = "";

            var sb = new StringBuilder(begin);
            bool first = true;
            foreach (var e in self)
            {
                if (!first)
                    sb.Append(between);
                first = false;
                sb.Append(format(e));
            }
            sb.Append(end);
            return sb.ToString();
        }

        /// <summary>
        /// Creates a Dictionary(of TKey,TValue) from an IEnumerable(of TSource)
        ///  according to specified key selector and element selector functions.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <typeparam name="TElement">The type of the value returned by elementSelector.</typeparam>
        /// <param name="source">An IEnumerable(of TSource) to create a Dictionary(of TKey,TValue) from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="duplicateValue">If a duplicate key was found, insert this element as value for the key. This value must be distinct from all other values generated by elementSelector.</param>
        /// <returns>A Dictionary(of TKey,TValue) that contains values of type TElement selected from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">source or keySelector or elementSelector is null. -or- keySelector produces a key that is null.</exception>
        public static Dictionary<TKey, TElement> ToDictionaryDistinct<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, TElement duplicateValue)
        {
            Requires.NotNull(source);
            Requires.NotNull(keySelector);
            Requires.NotNull(elementSelector);

            Dictionary<TKey, TElement> dictionary = new Dictionary<TKey, TElement>();
            foreach (TSource local in source)
            {
                try
                {
                    dictionary.Add(keySelector(local), elementSelector(local));
                }
                catch (ArgumentException)
                { //An element with the same key already exists in the Dictionary<TKey,TValue>.
                    dictionary[keySelector(local)] = duplicateValue;
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Creates a Dictionary(of TKey,TValue) from an IEnumerable(of TSource)
        /// containing all Key Value pairs with distinct keys.
        /// For duplicate keys, the returned value of <paramref name="duplicateKeyKeepElement"/> is inserted instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <typeparam name="TElement">The type of the value returned by elementSelector.</typeparam>
        /// <param name="source">An IEnumerable(of T) to create a Dictionary(of TKey,TValue) from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="duplicateKeyKeepElement">If a duplicate key was found, the function is called with the element to be inserted and the duplicate element already associated with the key, and must return true if the first (original) element should be kept of false if the second (new) element should be inserted.</param>
        /// <returns>A System.Collections.Generic.Dictionary(of TKey,TValue) that contains values of type TElement selected from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">Any of the arguments is null. -or- keySelector produces a key that is null.</exception>
        public static Dictionary<TKey, TElement> ToDictionaryDistinct<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TElement, TElement, bool> duplicateKeyKeepElement)
        {
            Requires.NotNull(source);
            Requires.NotNull(keySelector);
            Requires.NotNull(elementSelector);
            Requires.NotNull(duplicateKeyKeepElement);

            var dictionary = new Dictionary<TKey, TElement>();
            foreach (var v in source)
            {
                var key = keySelector(v);
                var element = elementSelector(v);
                try
                {
                    dictionary.Add(key, element);
                }
                catch (ArgumentException)
                { //[inefficient ISSUE 20091202 andi] Unfortunately very inefficient because of Dictionary implementation, since it is necessary to hash 3 times!
                    if(!duplicateKeyKeepElement(dictionary[key], element))
                        dictionary[key] = element;
                }
            }
            return dictionary;
        }

        /// <summary>
        /// Creates a Dictionary(of TKey,TValue) from an IEnumerable(of TSource)
        /// containing all Key Value pairs with distinct keys.
        /// For duplicate keys, the returned value of <paramref name="duplicateKeyElementSelector"/> is inserted instead.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by keySelector.</typeparam>
        /// <typeparam name="TElement">The type of the value returned by elementSelector.</typeparam>
        /// <param name="source">An IEnumerable(of T) to create a Dictionary(of TKey,TValue) from.</param>
        /// <param name="keySelector">A function to extract a key from each element.</param>
        /// <param name="elementSelector">A transform function to produce a result element value from each element.</param>
        /// <param name="duplicateKeyElementSelector">If a duplicate key was found, the function is called with the element to be inserted and the duplicate element already associated with the key, and must return the element to be inserted instead at this key.</param>
        /// <returns>A System.Collections.Generic.Dictionary(of TKey,TValue) that contains values of type TElement selected from the input sequence.</returns>
        /// <exception cref="ArgumentNullException">Any of the arguments is null. -or- keySelector produces a key that is null.</exception>
        public static Dictionary<TKey, TElement> ToDictionaryDistinct<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, Func<TElement, TElement, TElement> duplicateKeyElementSelector)
        {
            Requires.NotNull(source);
            Requires.NotNull(keySelector);
            Requires.NotNull(elementSelector);
            Requires.NotNull(duplicateKeyElementSelector);

            var dictionary = new Dictionary<TKey, TElement>();
            foreach (var v in source)
            {
                var key = keySelector(v);
                var element = elementSelector(v);
                try
                {
                    dictionary.Add(key, element);
                }
                catch (ArgumentException)
                { //[inefficient ISSUE 20091202 andi] Unfortunately very inefficient because of Dictionary implementation, since it is necessary to hash 3 times!
                    dictionary[key] = duplicateKeyElementSelector(dictionary[key], element);
                }
            }
            return dictionary;
        }

        /// <summary>
        /// This is a more efficient version of System.Linq.Enumerable.ToArray() when the number of elements of <paramref name="source"/> cannot be efficiently determined but is known.
        /// Creates an array from a <see cref="T:System.Collections.Generic.IEnumerable`1" />.
        /// </summary>
        /// <returns>An array of size count that contains the elements from the input sequence.</returns>
        /// <param name="source">An <see cref="T:System.Collections.Generic.IEnumerable`1" /> to create an array from.</param>
        /// <param name="count">The known size of the result-array.</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> is null.</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="count"/> is different from the number of elements in <paramref name="source" />.</exception>
        public static TElement[] ToArray<TElement>(this IEnumerable<TElement> source, int count)
        {
            if (count == 0)
            {
                return new TElement[0];
            }
            else
            {
                TElement[] array = null;
                int num = 0;
                ICollection<TElement> collection = source as ICollection<TElement>;
                if (collection != null)
                {
                    num = collection.Count;
                    if (num > 0)
                    {
                        array = new TElement[num];
                        collection.CopyTo(array, 0);
                    }
                }
                else
                {
                    array = new TElement[count];
                    foreach (TElement current in source)
                    {
                        if (count == num)
                            throw new ArgumentException("Enumerable has more elements than count.", "count");
                        array[num] = current;
                        num++;
                    }
                }
                if (count > num)
                    throw new ArgumentException("Enumerable has less elements than count.", "count");
                return array;
            }
        }

        #endregion

        #region Missing Operators

        /// <summary>
        /// Lazily applies action to each element of sequence.
        /// </summary>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> self, Action<T> action)
        {
            foreach (var x in self)
            {
                action(x);
                yield return x;
            }
        }

        /// <summary>
        /// Lazily applies action to each element of sequence.
        /// </summary>
        public static IEnumerable<T> Do<T>(this IEnumerable<T> self, Action<T, long> action)
        {
            long index = 0;
            foreach (var x in self)
            {
                action(x, index++);
                yield return x;
            }
        }

        /// <summary>
        /// Creates sequence containing the given item.
        /// </summary>
        public static IEnumerable<T> Return<T>(this T item)
        {
            yield return item;
        }

        /// <summary>
        /// Determines whether a sequence contains no elements.
        /// </summary>
        public static bool IsEmpty<T>(this IEnumerable<T> self)
        {
            return !self.Any();
        }

        /// <summary>
        /// Returns a sequence that contains only distinct contiguous elements.
        /// </summary>
        public static IEnumerable<T> DistinctUntilChanged<T>(this IEnumerable<T> self)
            where T : IEquatable<T>
        {
            bool first = true;
            T current = default(T);
            foreach (var x in self)
            {
                if (first) first = false;
                else if (x.Equals(current)) continue;
                current = x;
                yield return current;
            }
        }

        /// <summary>
        /// Returns index of first occurence of elementToFind.
        /// </summary>
        public static int IndexOf<T>(this IEnumerable<T> list, T elementToFind)
        {
            int i = 0;
            foreach (T element in list)
            {
                if (element.Equals(elementToFind))
                    return i;
                i++;
            }
            return -1;
        }
        
        /// <summary>
        /// Concats single item to sequence. 
        /// </summary>
        public static IEnumerable<T> Concat<T>(this IEnumerable<T> sequence, T item)
        {
            foreach (var x in sequence) yield return x;
            yield return item;
        }

        /// <summary>
        /// Concats sequence to single item. 
        /// </summary>
        public static IEnumerable<T> Concat<T>(this T item, IEnumerable<T> sequence)
        {
            yield return item;
            foreach (var x in sequence) yield return x;
        }

        /// <summary>
        /// Returns sequence of indices [0, sequence.Count()-1].
        /// </summary>
        public static IEnumerable<int> Indices<T>(this IEnumerable<T> sequence)
        {
            var i = 0;
            foreach (var x in sequence) yield return i++;
        }

        #endregion

        #region Math

        /// <summary>
        /// Converts a sequence that contains numbers of elements
        /// into a sequence that represents the indices of the
        /// first element if the elements are stored in consecutive 
        /// order and is closed by the total count elemtns.
        /// The length of the integrated sequence is +1 of the input.
        /// </summary>
        public static IEnumerable<int> Integrated(this IEnumerable<int> counts, int sum = 0)
        {
            foreach (var c in counts)
            {
                yield return sum;
                sum += c;
            }
            yield return sum;
        }

        /// <summary>
        /// Integrates a sequence of values. 
        /// Returns a sequence of N+1 values where the last is the sum off all value.
        /// </summary>
        public static IEnumerable<double> Integrated(this IEnumerable<double> values, double sum = 0)
        {
            foreach (var c in values)
            {
                yield return sum;
                sum += c;
            }
            yield return sum;
        }

        #endregion

        #region Median

        /// <summary>
        /// Searches for the median-element in the Enumerable (according to cmp) and returns its value.
        /// Runtime is in O(N) and Memory in O(N)
        /// For partitioning use QuickMedian
        /// </summary>
        public static T Median<T>(this IEnumerable<T> self, Func<T, T, int> cmp)
        {
            var array = self.ToArray();
            var med = array.Length / 2;

            array.QuickMedian(cmp, med);
            
            var result = array[med];
            array = null;

            return result;
        }

        /// <summary>
        /// Searches for the median-element in the Enumerable and returns its value.
        /// Runtime is in O(N) and Memory in O(N)
        /// For partitioning use QuickMedian
        /// </summary>
        public static T Median<T>(this IEnumerable<T> self)
            where T : IComparable<T>
        {
            return Median(self, (a, b) => a.CompareTo(b));
        }

        /// <summary>
        /// Searches for the median-element in the Array (according to cmp) and returns its value.
        /// Does not change the given Array
        /// Runtime is in O(N) and Memory in O(N)
        /// For partitioning use QuickMedian
        /// </summary>
        public static T Median<T>(this T[] self, Func<T, T, int> cmp)
        { 
            var med = self.Length / 2;
            var indices = self.CreatePermutationQuickMedian(cmp, med);
            var result = self[indices[med]];
            indices = null;

            return result;
        }

        /// <summary>
        /// Searches for the median-element in the Array and returns its value.
        /// Does not change the given Array
        /// Runtime is in O(N) and Memory in O(N)
        /// For partitioning use QuickMedian
        /// </summary>
        public static T Median<T>(this T[] self)
            where T : IComparable<T>
        {
            return Median(self, (a, b) => a.CompareTo(b));
        }

        /// <summary>
        /// Searches for the median-element in the List (according to cmp) and returns its value.
        /// Does not change the given List
        /// Runtime is in O(N) and Memory in O(N)
        /// For partitioning use QuickMedian
        /// </summary>
        public static T Median<T>(this List<T> self, Func<T, T, int> cmp)
        {
            var med = self.Count / 2;
            var indices = self.CreatePermutationQuickMedian(cmp, med);
            var result = self[indices[med]];
            indices = null;

            return result;
        }

        /// <summary>
        /// Searches for the median-element in the List and returns its value.
        /// Does not change the given List
        /// Runtime is in O(N) and Memory in O(N)
        /// For partitioning use QuickMedian
        /// </summary>
        public static T Median<T>(this List<T> self)
            where T : IComparable<T>
        {
            return Median(self, (a, b) => a.CompareTo(b));
        }

        #endregion
    }
}
