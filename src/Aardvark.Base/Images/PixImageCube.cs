using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    [Flags]
    public enum CubeSide
    {
        PositiveX = 0,
        NegativeX = 1,
        PositiveY = 2,
        NegativeY = 3,
        PositiveZ = 4,
        NegativeZ = 5,
    }

    public class PixImageCube 
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

    }
}
