using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base.Rendering
{
    public enum AlphaTestFunction
    {
        None,
        Never,
        Less,
        Equal,
        LessOrEqual,
        Greater,
        NotEqual,
        GreaterOrEqual,
        Always,
    }

    public enum FillMode
    {
        Fill,
        Line,
        Point
    }

    public enum DepthTestComparison
    {
        None,
        Less,
        LessOrEqual,
        Greater,
        GreaterOrEqual,
        Equal,
        NotEqual,
        Never,
        Always
    }

    public enum Face
    {
        Front = 0x00001,
        Back = 0x00002,
        FrontAndBack = Front | Back
    }

    public enum CullMode
    {
        None,
        Clockwise,
        CounterClockwise
    }
}
