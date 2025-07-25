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
using static Aardvark.Base.HashCode;
using static System.Math;

namespace Aardvark.Base;

public static class VectorHashCodeExtensions
{
    /// <summary>
    /// Compute the first of four possible hashcodes for hashing in a 2-D
    /// unit grid. Add items with this function, retrieve with function
    /// HashCode.Get4.
    /// </summary>
    public static int Get1of4(V2d point)
    {
        var xi = (long)Floor(point.X);
        var yi = (long)Floor(point.Y);

        return Combine((int)(xi >> 1), (int)(yi >> 1));
    }

    /// <summary>
    /// Compute all four possible hashcodes for hashing in a 2-D unit grid.
    /// Retrive all items with the four hashodes written into the supplied
    /// array. Items need to be added just with the first of the four
    /// hashcodes (also computed by function HashCodeGet1of4).
    /// </summary>
    public static void Get4(V2d point, int[] hca)
    {
        var xi = (long)Floor(point.X);
        var yi = (long)Floor(point.Y);

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
    public static int Get1of8(V3d point)
    {
        var xi = (long)Floor(point.X);
        var yi = (long)Floor(point.Y);
        var zi = (long)Floor(point.Z);

        return Combine((int)(xi >> 1), (int)(yi >> 1), (int)(zi >> 1));
    }

    /// <summary>
    /// Compute all eight possible hashcodes for hashing in a 3-D unit grid.
    /// Retrive all items with the eight hashodes written into the supplied
    /// array. Items need to be added just with the first of the eight
    /// hashcodes (also computed by function HashCodeGet1of2).
    /// </summary>
    public static void Get8(V3d point, int[] hca)
    {
        var xi = (long)Floor(point.X);
        var yi = (long)Floor(point.Y);
        var zi = (long)Floor(point.Z);

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
    public static int Get1of16(V4d point)
    {
        var xi = (long)Floor(point.X);
        var yi = (long)Floor(point.Y);
        var zi = (long)Floor(point.Z);
        var wi = (long)Floor(point.W);

        return Combine((int)(xi >> 1), (int)(yi >> 1), (int)(zi >> 1), (int)(wi >> 1));
    }

    /// <summary>
    /// Compute all 16 possible hashcodes for hashing in a 4-D unit grid.
    /// Retrive all items with the 16 hashodes written into the supplied
    /// array. Items need to be added just with the first of the 16
    /// hashcodes (also computed by function HashCodeGet1of16).
    /// </summary>
    public static void Get16(V4d point, int[] hca)
    {
        var xi = (long)Floor(point.X);
        var yi = (long)Floor(point.Y);
        var zi = (long)Floor(point.Z);
        var wi = (long)Floor(point.W);

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
