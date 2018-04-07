using System;
using static System.Math;

namespace Aardvark.Base
{
    #region Vec

    public partial class Vec
    {
        [Flags]
        public enum DirFlags
        {
            None = 0x00,
            NegativeX = 0x01,
            PositiveX = 0x02,
            NegativeY = 0x04,
            PositiveY = 0x08,
            NegativeZ = 0x10,
            PositiveZ = 0x20,
            NegativeW = 0x40,
            PositiveW = 0x80,

            X = NegativeX | PositiveX,
            Y = NegativeY | PositiveY,
            Z = NegativeZ | PositiveZ,
        };
    }

    #endregion

    #region V2f

    public partial struct V2f
    {
        public static V2f FromPolar(float angleInRadians, float radius)
            => new V2f(Cos(angleInRadians) * radius, Sin(angleInRadians) * radius);

        public static V2f FromPolar(float angleInRadians)
            => new V2f(Cos(angleInRadians), Sin(angleInRadians));
    }

    #endregion

    #region V2d

    public partial struct V2d
    {
        public static V2d FromPolar(double angleInRadians, double radius)
            => new V2d(Cos(angleInRadians) * radius, Sin(angleInRadians) * radius);

        public static V2d FromPolar(double angleInRadians)
            => new V2d(Cos(angleInRadians), Sin(angleInRadians));
    }

    #endregion

    #region V2i

    public partial struct V2i
    {
    }

    #endregion

    #region V2l

    public partial struct V2l
    {
    }

    #endregion

    #region V3f

    public partial struct V3f
    {
    }

    #endregion

    #region V3d

    public partial struct V3d
    {
    }

    #endregion

    #region V3i

    public partial struct V3i
    {
    }

    #endregion

    #region V3l

    public partial struct V3l
    {
    }

    #endregion

    #region V4f

    public partial struct V4f
    {
    }

    #endregion

    #region V4d

    public partial struct V4d
    {
    }

    #endregion

    #region V4i

    public partial struct V4i
    {
    }

    #endregion

    #region V4l

    public partial struct V4l
    {
    }

    #endregion
}
