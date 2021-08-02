using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Conic2f

    /// <summary>
    /// WARNING: Sektch!
    /// Defines the conic section CXX x^2 + CYY y^2 + CZZ z^2 + CXY xy + CYZ yz + CYZ yz = 0
    /// with (x,y,z) being homogenous coordinates.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Conic2f
    {
        [DataMember]
        public float CXX;
        [DataMember]
        public float CYY;
        [DataMember]
        public float CZZ;
        [DataMember]
        public float CXY;
        [DataMember]
        public float CXZ;
        [DataMember]
        public float CYZ;

        public Conic2f(float cxx, float cyy, float czz, float cxy, float cxz, float cyz)
        {
            CXX = cxx; CYY = cyy; CZZ = czz; CXY = cxy; CXZ = cxz; CYZ = cyz;
        }


        public float Discriminant { get { return CXX * CYY - CXY * CXY; } }

        public int ConicType
        {
            get
            {
                var d = Discriminant;
                return d > Constant<float>.PositiveTinyValue ? 1
                    : d < Constant<float>.NegativeTinyValue ? -1 : 0;
            }
        }
    }

    #endregion

    #region Conic2d

    /// <summary>
    /// WARNING: Sektch!
    /// Defines the conic section CXX x^2 + CYY y^2 + CZZ z^2 + CXY xy + CYZ yz + CYZ yz = 0
    /// with (x,y,z) being homogenous coordinates.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct Conic2d
    {
        [DataMember]
        public double CXX;
        [DataMember]
        public double CYY;
        [DataMember]
        public double CZZ;
        [DataMember]
        public double CXY;
        [DataMember]
        public double CXZ;
        [DataMember]
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

    #endregion

}
