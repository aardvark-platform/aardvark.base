using Aardvark.Base.Sorting;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
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
}
