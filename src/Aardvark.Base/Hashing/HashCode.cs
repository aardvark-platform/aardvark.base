﻿using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using static System.Math;

namespace Aardvark.Base
{
    public static class HashCode
    {
        /*
            Permission is hereby granted, free of charge, to any person or organization
            obtaining a copy of the software and accompanying documentation covered by
            this license (the "Software") to use, reproduce, display, distribute,
            execute, and transmit the Software, and to prepare derivative works of the
            Software, and to permit third-parties to whom the Software is furnished to
            do so, all subject to the following:

            The copyright notices in the Software and this entire statement, including
            the above license grant, this restriction and the following disclaimer,
            must be included in all copies of the Software, in whole or in part, and
            all derivative works of the Software, unless such copies or derivative
            works are solely in the form of machine-executable object code generated by
            a source language processor.

            THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
            IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
            FITNESS FOR A PARTICULAR PURPOSE, TITLE AND NON-INFRINGEMENT. IN NO EVENT
            SHALL THE COPYRIGHT HOLDERS OR ANYONE DISTRIBUTING THE SOFTWARE BE LIABLE
            FOR ANY DAMAGES OR OTHER LIABILITY, WHETHER IN CONTRACT, TORT OR OTHERWISE,
            ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
            DEALINGS IN THE SOFTWARE.
        */

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UCombine(int a, int b)
            => (uint)a ^ (uint)b + 0x9e3779b9 + ((uint)a << 6) + ((uint)a >> 2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UCombine(uint a, int b)
            => a ^ (uint)b + 0x9e3779b9 + (a << 6) + (a >> 2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UCombine(int a, uint b)
            => (uint)a ^ (uint)b + 0x9e3779b9 + ((uint)a << 6) + ((uint)a >> 2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint UCombine(uint a, uint b)
            => a ^ b + 0x9e3779b9 + (a << 6) + (a >> 2);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(uint a, uint b)
            => (int)(a ^ b + 0x9e3779b9 + (a << 6) + (a >> 2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, uint b)
            => (int)((uint)a ^ b + 0x9e3779b9 + ((uint)a << 6) + ((uint)a >> 2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(uint a, int b)
            => (int)(a ^ (uint)b + 0x9e3779b9 + (a << 6) + (a >> 2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, int b)
            => (int)((uint)a ^ (uint)b + 0x9e3779b9 + ((uint)a << 6) + ((uint)a >> 2));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(long a, int b)
            => Combine(UCombine((int)(a >> 32), (int)a), b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, long b)
            => Combine(a, UCombine((int)(b >> 32), (int)b));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(long a, long b)
            => Combine(UCombine((int)(a >> 32), (int)a),
                       UCombine((int)(b >> 32), (int)b));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, int b, int c)
        {
            return Combine(UCombine(a, b), c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, int b, int c, int d)
        {
            return Combine(UCombine(UCombine(a, b), c), d);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, int b, int c, int d, int e)
        {
            return Combine(UCombine(UCombine(UCombine(a, b), c), d), e);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, int b, int c, int d, int e, int f)
        {
            return Combine(UCombine(UCombine(UCombine(UCombine(a, b), c), d), e), f);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, int b, int c, int d, int e, int f, int g)
        {
            return Combine(UCombine(UCombine(UCombine(UCombine(UCombine(a, b), c), d), e), f), g);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, int b, int c, int d, int e, int f, int g, int h)
        {
            return Combine(UCombine(UCombine(UCombine(UCombine(UCombine(UCombine(a, b), c), d), e), f), g), h);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(int a, int b, params int[] rest)
        {
            uint h = UCombine(a, b);
            foreach (var i in rest) h = UCombine(h, i);
            return (int)h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombined<T0, T1>(T0 e0, T1 e1)
            where T0 : struct
            where T1 : struct
            => Combine(e0.GetHashCode(), e1.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombined<T0, T1, T2>(T0 e0, T1 e1, T2 e2)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            => Combine(UCombine(e0.GetHashCode(), e1.GetHashCode()), e2.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombined<T0, T1, T2, T3>(T0 e0, T1 e1, T2 e2, T3 e3)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            => Combine(UCombine(UCombine(e0.GetHashCode(), e1.GetHashCode()), e2.GetHashCode()), e3.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombined<T0, T1, T2, T3, T4>(T0 e0, T1 e1, T2 e2, T3 e3, T4 e4)
            where T0 : struct
            where T1 : struct
            where T2 : struct
            where T3 : struct
            where T4 : struct
            => Combine(UCombine(UCombine(UCombine(e0.GetHashCode(),
                e1.GetHashCode()), e2.GetHashCode()), e3.GetHashCode()), e4.GetHashCode());

        /// <summary>
        /// Compute the combined hash code of an IEnumerable of items.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombinedHashCode<T>(this IEnumerable<T> items)
            where T : struct
            => (int)items.Aggregate(0U, (h, i) => UCombine(h, i.GetHashCode()));

        /// <summary>
        /// Compute the combined hash code of an array of items (explicit
        /// implementation in order to be faster than IEnumerable version).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombinedHashCode<T>(this T[] array)
            where T : struct
            => array.GetCombinedHashCode(array.LongLength);

        /// <summary>
        /// Compute the combined hash code of an array of items (explicit
        /// implementation in order to be faster than IEnumerable version).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombinedHashCode<T>(this T[] array, long count)
            where T : struct
        {
            if (count == 0) return 0;
            var h = (uint)array[0].GetHashCode();
            for (var i = 1; i < count; i++)
                h = UCombine(h, array[i].GetHashCode());
            return (int)h;
        }

        /// <summary>
        /// Compute the combined hash code of an array of items (explicit
        /// implementation in order to be faster than IEnumerable version).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombinedHashCode<T>(this T[] array, long first, long count)
            where T : struct
        {
            if (count == 0) return 0;
            var h = (uint)array[first].GetHashCode();
            for (long i = first + 1, e = first + count; i < e; i++)
                h = UCombine(h, array[i].GetHashCode());
            return (int)h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombinedWithDefaultZero<T0, T1>(T0 e0, T1 e1)
            => Combine(e0.Equals(default(T0)) ? 0 : e0.GetHashCode(),
                       e1.Equals(default(T1)) ? 0 : e1.GetHashCode());
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombinedWithDefaultZero<T0, T1, T2>(T0 e0, T1 e1, T2 e2)
            => Combine(UCombine(e0.Equals(default(T0)) ? 0 : e0.GetHashCode(),
                                e1.Equals(default(T1)) ? 0 : e1.GetHashCode()),
                                e2.Equals(default(T2)) ? 0 : e2.GetHashCode());

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetCombinedWithDefaultZero<T0, T1, T2, T3>(T0 e0, T1 e1, T2 e2, T3 e3)
            => Combine(UCombine(UCombine(e0.Equals(default(T0)) ? 0 : e0.GetHashCode(),
                                         e1.Equals(default(T1)) ? 0 : e1.GetHashCode()),
                                         e2.Equals(default(T2)) ? 0 : e2.GetHashCode()),
                                         e3.Equals(default(T3)) ? 0 : e3.GetHashCode());

        /// <summary>
        /// Compute the first of two possible hashcodes for hashing in a 1-D
        /// unit grid. Add items with this function, retrieve with function
        /// HashCode.Get2.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get1of2(double x)
        {
            var xi = (long)Floor(x);
            return (int)(xi >> 1);
        }

        /// <summary>
        /// Compute all two possible hashcodes for hashing in a 1-D unit grid.
        /// Retrive all items with the two hashodes written into the supplied
        /// array. Items need to be added just with the first of the two
        /// hashcodes (also computed by function HashCodeGet1of2).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get2(double x, int[] hca)
        {
            var xi = (long)Floor(x);

            int xh0 = (int)(xi >> 1);

            hca[0] = xh0;
            hca[1] = xh0 - 1 + ((int)(xi & 1) << 1);
        }

        /// <summary>
        /// Compute the first of four possible hashcodes for hashing in a 2-D
        /// unit grid. Add items with this function, retrieve with function
        /// HashCode.Get4.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get1of4(double x, double y)
        {
            var xi = (long)Floor(x);
            var yi = (long)Floor(y);

            return Combine((int)(xi >> 1), (int)(yi >> 1));
        }

        /// <summary>
        /// Compute all four possible hashcodes for hashing in a 2-D unit grid.
        /// Retrive all items with the four hashodes written into the supplied
        /// array. Items need to be added just with the first of the four
        /// hashcodes (also computed by function HashCodeGet1of4).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get4(double x, double y, int[] hca)
        {
            var xi = (long)Floor(x);
            var yi = (long)Floor(y);

            int xh0 = (int)(xi >> 1), xh1 = xh0 - 1 + ((int)(xi & 1) << 1);
            int yh0 = (int)(yi >> 1), yh1 = yh0 - 1 + ((int)(yi & 1) << 1);

            hca[0] = Combine(xh0, yh0);
            hca[1] = Combine(xh1, yh0);
            hca[2] = Combine(xh0, yh1);
            hca[3] = Combine(xh1, yh1);
        }

        /// <summary>
        /// Compute the first of eight possible hashcodes for hashing in a 3-D
        /// unit grid. Add items with this function, retrieve with function
        /// HashCode.Get8.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get1of8(double x, double y, double z)
        {
            var xi = (long)Floor(x);
            var yi = (long)Floor(y);
            var zi = (long)Floor(z);

            return Combine((int)(xi >> 1), (int)(yi >> 1), (int)(zi >> 1));
        }

        /// <summary>
        /// Compute all eight possible hashcodes for hashing in a 3-D unit grid.
        /// Retrive all items with the eight hashodes written into the supplied
        /// array. Items need to be added just with the first of the eight
        /// hashcodes (also computed by function HashCodeGet1of2).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get8(double x, double y, double z, int[] hca)
        {
            var xi = (long)Floor(x);
            var yi = (long)Floor(y);
            var zi = (long)Floor(z);

            int xh0 = (int)(xi >> 1), xh1 = xh0 - 1 + ((int)(xi & 1) << 1);
            int yh0 = (int)(yi >> 1), yh1 = yh0 - 1 + ((int)(yi & 1) << 1);
            int zh0 = (int)(zi >> 1), zh1 = zh0 - 1 + ((int)(zi & 1) << 1);

            hca[0] = Combine(xh0, yh0, zh0);
            hca[1] = Combine(xh1, yh0, zh0);
            hca[2] = Combine(xh0, yh1, zh0);
            hca[3] = Combine(xh1, yh1, zh0);
            hca[4] = Combine(xh0, yh0, zh1);
            hca[5] = Combine(xh1, yh0, zh1);
            hca[6] = Combine(xh0, yh1, zh1);
            hca[7] = Combine(xh1, yh1, zh1);
        }

        /// <summary>
        /// Compute the first of 16 possible hashcodes for hashing in a 4-D
        /// unit grid. Add items with this function, retrieve with function
        /// HashCode.Get16.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Get1of16(double x, double y, double z, double w)
        {
            var xi = (long)Floor(x);
            var yi = (long)Floor(y);
            var zi = (long)Floor(z);
            var wi = (long)Floor(w);

            return Combine((int)(xi >> 1), (int)(yi >> 1), (int)(zi >> 1), (int)(wi >> 1));
        }

        /// <summary>
        /// Compute all 16 possible hashcodes for hashing in a 4-D unit grid.
        /// Retrive all items with the 16 hashodes written into the supplied
        /// array. Items need to be added just with the first of the 16
        /// hashcodes (also computed by function HashCodeGet1of16).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Get16(double x, double y, double z, double w, int[] hca)
        {
            var xi = (long)Floor(x);
            var yi = (long)Floor(y);
            var zi = (long)Floor(z);
            var wi = (long)Floor(w);

            int xh0 = (int)(xi >> 1), xh1 = xh0 - 1 + ((int)(xi & 1) << 1);
            int yh0 = (int)(yi >> 1), yh1 = yh0 - 1 + ((int)(yi & 1) << 1);
            int zh0 = (int)(zi >> 1), zh1 = zh0 - 1 + ((int)(zi & 1) << 1);
            int dh0 = (int)(wi >> 1), dh1 = dh0 - 1 + ((int)(wi & 1) << 1);

            hca[0] = Combine(xh0, yh0, zh0, dh0);
            hca[1] = Combine(xh1, yh0, zh0, dh0);
            hca[2] = Combine(xh0, yh1, zh0, dh0);
            hca[3] = Combine(xh1, yh1, zh0, dh0);
            hca[4] = Combine(xh0, yh0, zh1, dh0);
            hca[5] = Combine(xh1, yh0, zh1, dh0);
            hca[6] = Combine(xh0, yh1, zh1, dh0);
            hca[7] = Combine(xh1, yh1, zh1, dh0);
            hca[8] = Combine(xh0, yh0, zh0, dh1);
            hca[9] = Combine(xh1, yh0, zh0, dh1);
            hca[10] = Combine(xh0, yh1, zh0, dh1);
            hca[11] = Combine(xh1, yh1, zh0, dh1);
            hca[12] = Combine(xh0, yh0, zh1, dh1);
            hca[13] = Combine(xh1, yh0, zh1, dh1);
            hca[14] = Combine(xh0, yh1, zh1, dh1);
            hca[15] = Combine(xh1, yh1, zh1, dh1);
        }
    }
}
