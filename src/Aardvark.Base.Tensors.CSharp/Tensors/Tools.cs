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

namespace Aardvark.Base;

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
