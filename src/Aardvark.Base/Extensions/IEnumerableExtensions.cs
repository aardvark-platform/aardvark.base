using Aardvark.Base.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    public static class IEnumerableFun_
    {
        public static SymbolSet ToSymbolSet(this IEnumerable<Symbol> symbols)
        {
            return new SymbolSet(symbols);
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
