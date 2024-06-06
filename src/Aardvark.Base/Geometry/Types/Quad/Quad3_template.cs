using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var tc = isDouble ? "d" : "f";
    //#   var type = "Quad3" + tc;
    //#   var v3t = "V3" + tc;
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    //#   var half = isDouble ? "0.5" : "0.5f";
    #region __type__

    /// <summary>
    /// A three-dimensional quadrangle specified by its four points.
    /// The points are not required to lie on the same plane.
    /// </summary>
    public partial struct __type__
    {
        #region Geometric Properties

        /// <summary>
        /// Returns the area when projected onto a plane normal to the area
        /// weighted average normal of the triangles (P0,P1,P2) and (P0,P2,P3).
        /// </summary>
        public readonly __ftype__ Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var v02 = P2 - P0;
                return __half__ * ((P1 - P0).Cross(v02) + v02.Cross(P3 - P0)).Length;
            }
        }

        public readonly bool IsDegenerated
        {
            get
            {
                var v01 = P1 - P0; if (v01.AllTiny) return true;
                var v03 = P3 - P0; if (v03.AllTiny) return true;
                if ((P2 - P1).AllTiny || (P3 - P2).AllTiny) return true;
                var v02 = P2 - P0;
                return (v01.Cross(v02) + v02.Cross(v03)).AllTiny;
            }
        }

        public readonly __v3t__ Normal
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                var v02 = P2 - P0;
                return ((P1 - P0).Cross(v02) + v02.Cross(P3 - P0)).Normalized;
            }
        }

        #endregion
    }

    #endregion

    //# }
}
