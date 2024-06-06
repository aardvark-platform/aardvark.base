using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    #region Line1i

    public partial struct Line1i
    {
        #region Static Creator

        public static Line1i CreateSorted(int i0, int i1)
        {
            return i0 < i1 ? new Line1i(i0, i1) : new Line1i(i1, i0);
        }

        #endregion

        #region Properties

        public readonly Line1i Reversed { get { return new Line1i(I1, I0); } }

        #endregion
    }

    #endregion

    #region Triangle1i

    public partial struct Triangle1i
    {
        #region Static Creator

        public static Triangle1i CreateSorted(int i0, int i1, int i2)
        {
            if (i0 < i1)
            {
                if (i1 < i2)
                    return new Triangle1i(i0, i1, i2);
                else
                {
                    if (i0 < i2)
                        return new Triangle1i(i0, i2, i1);
                    else
                        return new Triangle1i(i2, i0, i1);
                }
            }
            else
            {
                if (i2 < i1)
                    return new Triangle1i(i2, i1, i0);
                else
                {
                    if (i0 < i2)
                        return new Triangle1i(i1, i0, i2);
                    else
                        return new Triangle1i(i1, i2, i0);
                }
            }
        }

        #endregion

        #region Properties

        public readonly Triangle1i Reversed { get { return new Triangle1i(I2, I1, I0); } }

        #endregion
    }

    #endregion

    #region Quad1i

    public partial struct Quad1i
    {
        #region Properties

        public readonly Quad1i Reversed { get { return new Quad1i(I3, I2, I1, I0); } }

        #endregion
    }

    #endregion
}
