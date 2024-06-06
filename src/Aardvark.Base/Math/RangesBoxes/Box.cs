using System.Runtime.Serialization;

namespace Aardvark.Base
{
    #region OctoBox2d

    [DataContract]
    public struct OctoBox2d
    {
        [DataMember]
        public double PX, PY, NX, NY, PXPY, PXNY, NXPY, NXNY;

        #region Constructors

        public OctoBox2d(
                double px, double py, double nx, double ny,
                double pxpy, double pxny, double nxpy, double nxny)
        {
            PX = px; PY = py; NX = nx; NY = ny;
            PXPY = pxpy; PXNY = pxny; NXPY = nxpy; NXNY = nxny;
        }

        #endregion

        #region Constants

        public static readonly OctoBox2d Invalid =
                new OctoBox2d(double.MinValue, double.MinValue, double.MinValue, double.MinValue,
                              double.MinValue, double.MinValue, double.MinValue, double.MinValue);

        public static readonly OctoBox2d Zero =
                new OctoBox2d(0, 0, 0, 0, 0, 0, 0, 0);

        #endregion

        #region Properties

        public readonly double Area
        {
            get
            {
                return (PX + NX) * (PY + NY)
                        - 0.5 * (Fun.Square(PX + PY - PXPY) + Fun.Square(PX + NY - PXNY)
                                 + Fun.Square(NX + PY - NXPY) + Fun.Square(NX + NY - NXNY));
            }
        }

        #endregion

        #region Manipulation

        public void ExtendBy(V2d p)
        {
            ExtendBy(p.X, p.Y);
        }

        public void ExtendBy(double x, double y)
        {
            if (x > PX) PX = x;
            if (y > PY) PY = y;
            var nx = -x; if (nx > NX) NX = nx;
            var ny = -y; if (ny > NY) NY = ny;
            var pxpy = x + y; if (pxpy > PXPY) PXPY = pxpy;
            var pxny = x - y; if (pxny > PXNY) PXNY = pxny;
            var nxpy = -x + y; if (nxpy > NXPY) NXPY = nxpy;
            var nxny = -x - y; if (nxny > NXNY) NXNY = nxny;
        }

        #endregion
    }

    #endregion
}