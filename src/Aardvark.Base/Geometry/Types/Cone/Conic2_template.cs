using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Conic2" + tc;
    //#   var type2 = "Conic2" + tc2;
    //#   var v2t = "V2" + tc;
    #region __type__

    /// <summary>
    /// WARNING: Sektch!
    /// Defines the conic section CXX x^2 + CYY y^2 + CZZ z^2 + CXY xy + CYZ yz + CYZ yz = 0
    /// with (x,y,z) being homogenous coordinates.
    /// </summary>
    [DataContract]
    [StructLayout(LayoutKind.Sequential)]
    public struct __type__
    {
        [DataMember]
        public __ftype__ CXX;
        [DataMember]
        public __ftype__ CYY;
        [DataMember]
        public __ftype__ CZZ;
        [DataMember]
        public __ftype__ CXY;
        [DataMember]
        public __ftype__ CXZ;
        [DataMember]
        public __ftype__ CYZ;

        public __type__(__ftype__ cxx, __ftype__ cyy, __ftype__ czz, __ftype__ cxy, __ftype__ cxz, __ftype__ cyz)
        {
            CXX = cxx; CYY = cyy; CZZ = czz; CXY = cxy; CXZ = cxz; CYZ = cyz;
        }


        public __ftype__ Discriminant { get { return CXX * CYY - CXY * CXY; } }

        public int ConicType
        {
            get
            {
                var d = Discriminant;
                return d > Constant<__ftype__>.PositiveTinyValue ? 1
                    : d < Constant<__ftype__>.NegativeTinyValue ? -1 : 0;
            }
        }
    }

    #endregion

    //# }
}
