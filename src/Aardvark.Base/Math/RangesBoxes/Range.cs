using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public partial struct Range1b : IRange<byte, Range1b>
    {
    }

    public partial struct Range1sb : IRange<sbyte, Range1sb>
    {
    }

    public partial struct Range1s : IRange<short, Range1s>
    {
    }

    public partial struct Range1us : IRange<ushort, Range1us>
    {
    }

    public partial struct Range1i : IRange<int, Range1i>
    {
    }

    public partial struct Range1ui : IRange<uint, Range1ui>
    {
    }

    public partial struct Range1l : IRange<long, Range1l>
    {
    }

    public partial struct Range1ul : IRange<ulong, Range1ul>
    {
    }

    public partial struct Range1f : IRange<float, Range1f>
    {
        public Range1f(Range1d box)
        {
            Min = (float)box.Min;
            Max = (float)box.Max;
        }

        public static explicit operator Range1f(Range1d box)
        {
            return new Range1f(box);
        }
    }

    public partial struct Range1d : IRange<double, Range1d>
    {
        public Range1d(Range1f box)
        {
            Min = (double)box.Min;
            Max = (double)box.Max;
        }

        public static explicit operator Range1d(Range1f box)
        {
            return new Range1d(box);
        }
    }

}