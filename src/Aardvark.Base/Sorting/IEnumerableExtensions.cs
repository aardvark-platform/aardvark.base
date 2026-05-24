using Aardvark.Base.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    public static class IEnumerableSortingExtensions
    {
        public static SymbolSet ToSymbolSet(this IEnumerable<Symbol> symbols) => new SymbolSet(symbols);

        public static bool SetEquals<T>(this IEnumerable<T> self, IEnumerable<T> other)
        {
            if (self == null) return other == null;
            if (other == null) return false;

            var selfSet = new HashSet<T>();
            foreach (var x in self)
                if (!selfSet.Add(x)) throw new Exception("not a proper set");

            var otherSet = new HashSet<T>();
            foreach (var x in other)
                if (!otherSet.Add(x)) throw new Exception("not a proper set");

            if (selfSet.Count != otherSet.Count) return false;
            foreach (var x in selfSet) if (!otherSet.Contains(x)) return false;

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
        public static T Median<T>(this IEnumerable<T> self) where T : IComparable<T>
            => Median(self, (a, b) => a.CompareTo(b));

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
        public static T Median<T>(this T[] self) where T : IComparable<T>
            => Median(self, (a, b) => a.CompareTo(b));

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
        public static T Median<T>(this List<T> self) where T : IComparable<T>
            => Median(self, (a, b) => a.CompareTo(b));

        #endregion
    }
}
