using Aardvark.Base.Sorting;
using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class ListFun_
    {
        [Obsolete("Use 'MapToArray' instead (same functionality and parameters)", false)]
        public static Tr[] ToArray<T, Tr>(this List<T> self, Func<T, Tr> fun)
        {
            return self.MapToArray(self.Count, fun);
        }

        [Obsolete("Use 'MapToArray' instead (same functionality and parameters)", false)]
        public static Tr[] ToArray<T, Tr>(this List<T> self, int count, Func<T, Tr> fun)
        {
            return self.MapToArray(count, fun);
        }

        [Obsolete("Use 'MapToArray' instead (same functionality and parameters)", false)]
        public static Tr[] ToArray<T, Tr>(this List<T> self, int start, int count, Func<T, Tr> fun)
        {
            return self.MapToArray(start, count, fun);
        }
        
        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public static List<Tr> Copy<T, Tr>(this List<T> list, Func<T, Tr> fun)
        {
            return list.Map(fun);
        }

        [Obsolete("Use 'Map' instead (same functionality and parameters)", false)]
        public static List<Tr> Copy<T, Tr>(this List<T> list, Func<T, int, Tr> fun)
        {
            return list.Map(fun);
        }

        [Obsolete("Use 'MapToArray' instead (same functionality and parameters)", false)]
        public static Tr[] CopyToArray<T, Tr>(this List<T> list, Func<T, Tr> item_fun)
        {
            return list.MapToArray(list.Count, item_fun);
        }

        [Obsolete("Use 'MapToArray' instead (same functionality and parameters)", false)]
        public static Tr[] CopyToArray<T, Tr>(this List<T> list, int count, Func<T, Tr> item_fun)
        {
            return list.MapToArray(count, item_fun);
        }

        [Obsolete("Use 'MapToArray' instead (same functionality and parameters)", false)]
        public static Tr[] CopyToArray<T, Tr>(
                this List<T> list, int start, int count, Func<T, Tr> item_fun)
        {
            return list.MapToArray(start, count, item_fun);
        }

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
