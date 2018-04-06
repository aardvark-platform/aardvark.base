using Aardvark.Base.Sorting;
using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class ListFun_
    {
        public static int NSmallestIndex<T>(this List<T> a, int n)
            where T : IComparable<T>
        {
            if (n == 0) return a.SmallestIndex();
            var p = a.CreatePermutationQuickMedianAscending(n);
            return p[n];
        }

        public static int NLargestIndex<T>(this List<T> a, int n)
            where T : IComparable<T>
        {
            if (n == 0) return a.LargestIndex();
            var p = a.CreatePermutationQuickMedianDescending(n);
            return p[n];
        }
    }
}
