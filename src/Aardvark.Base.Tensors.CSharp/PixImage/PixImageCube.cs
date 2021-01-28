using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    /// <summary>
    /// Enumeration of cube faces. The integer value of the enum also gives the index of the face in a texture array representing a cubemap.
    /// </summary>
    public enum CubeSide
    {
        PositiveX = 0,
        NegativeX = 1,
        PositiveY = 2,
        NegativeY = 3,
        PositiveZ = 4,
        NegativeZ = 5,
    }

    /// <summary>
    /// Bit field of cube face sides. The index of a bit represents the index of the face in a texture array representing a cubemap.
    /// </summary>
    [Flags]
    public enum CubeSideFlags
    {
        PositiveX = 0x1,
        NegativeX = 0x2,
        PositiveY = 0x4,
        NegativeY = 0x8,
        PositiveZ = 0x10,
        NegativeZ = 0x20,
        All = 0x3f,
    }

    public class PixImageCube : IPix
    {
        public PixImageMipMap[] MipMapArray;

        #region Constructor

        public PixImageCube(PixImageMipMap[] mipMapArray)
        {
            MipMapArray = mipMapArray;
        }

        #endregion

        #region Properties

        public PixImageMipMap this[CubeSide side]
        {
            get { return MipMapArray[(int)side]; }
        }

        #endregion

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixImageCube(this); }
    }
}
