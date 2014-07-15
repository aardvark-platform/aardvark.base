using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    internal static class Tools
    {
        public static void CheckMatching(long v1, long v2)
        {
            if (v1 != v2) throw new Exception("mismatch");
        }

        public static void CheckMatching(long v1, long v2, long v3)
        {
            CheckMatching(v1, v2); CheckMatching(v1, v3);
        }

        public static void CheckMatching(long v1, long v2, long v3, long v4)
        {
            CheckMatching(v1, v2); CheckMatching(v1, v3); CheckMatching(v1, v4);
        }

        public static void CheckMatching(V2l v1, V2l v2)
        {
            if (v1 != v2) throw new Exception("mismatch");
        }

        public static void CheckMatching(V2l v1, V2l v2, V2l v3)
        {
            CheckMatching(v1, v2); CheckMatching(v1, v3);
        }

        public static void CheckMatching(V2l v1, V2l v2, V2l v3, V2l v4)
        {
            CheckMatching(v1, v2); CheckMatching(v1, v3); CheckMatching(v1, v4);
        }

        public static void CheckMatching(V3l v1, V3l v2)
        {
            if (v1 != v2) throw new Exception("mismatch");
        }

        public static void CheckMatching(V3l v1, V3l v2, V3l v3)
        {
            CheckMatching(v1, v2); CheckMatching(v1, v3);
        }

        public static void CheckMatching(V3l v1, V3l v2, V3l v3, V3l v4)
        {
            CheckMatching(v1, v2); CheckMatching(v1, v3); CheckMatching(v1, v4);
        }

        public static long[] DenseDelta(long[] length, out long total)
        {
            int rank = (int)length.Length;
            var delta = new long[rank];
            total = 1;
            for (int r = 0; r < rank; r++)
            {
                delta[r] = total;
                total *= length[r];
            }
            return delta;
        }
    }
}
