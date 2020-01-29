using System;

namespace Aardvark.Base
{
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
}
