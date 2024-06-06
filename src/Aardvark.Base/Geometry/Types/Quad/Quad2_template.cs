using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var tc = isDouble ? "d" : "f";
    //#   var type = "Quad2" + tc;
    //#   var v2t = "V2" + tc;
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    //#   var half = isDouble ? "0.5" : "0.5f";
    #region __type__

    /// <summary>
    /// A two-dimensional quadrangle specified by its four points.
    /// </summary>
    public partial struct __type__
    {
        #region Geometric Properties

        public readonly __ftype__ Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => __half__ * ((P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y)).Abs();
        }

        public readonly bool IsDegenerated
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (P1 - P0).AllTiny || (P2 - P1).AllTiny || (P3 - P2).AllTiny || (P0 - P3).AllTiny || WindingOrder.IsTiny();
        }

        /// <summary>
        /// Returns a value less than zero for ccw and greater than zero for cw.
        /// The magnitude magnitude is twice the area.
        /// </summary>
        public readonly __ftype__ WindingOrder
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y);
        }

        #endregion
    }

    #endregion

    //# }
}
