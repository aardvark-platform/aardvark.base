using System.Linq;
using System.Runtime.InteropServices;

namespace Aardvark.Base
{
    /// <summary>
    /// WARNING: Sektch!
    /// Defines the conic section CXX x^2 + CYY y^2 + CZZ z^2 + CXY xy + CYZ yz + CYZ yz = 0
    /// with (x,y,z) being homogenous coordinates.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Conic2d
    {
        public double CXX;
        public double CYY;
        public double CZZ;
        public double CXY;
        public double CXZ;
        public double CYZ;

        public Conic2d(double cxx, double cyy, double czz, double cxy, double cxz, double cyz)
        {
            CXX = cxx; CYY = cyy; CZZ = czz; CXY = cxy; CXZ = cxz; CYZ = cyz;
        }


        public double Discriminant { get { return CXX * CYY - CXY * CXY; } }

        public int ConicType
        {
            get
            {
                var d = Discriminant;
                return d > Constant<double>.PositiveTinyValue ? 1
                    : d < Constant<double>.NegativeTinyValue ? -1 : 0;
            }
        }
    }
}
