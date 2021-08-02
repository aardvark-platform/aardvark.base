using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var tc = isDouble ? "d" : "f";
    //#   var type = "Ellipse3" + tc;
    //#   var v3t = "V3" + tc;
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    #region __type__

    public partial struct __type__ : IValidity
    {
        public static __type__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(__v3t__.NaN, __v3t__.Zero, __v3t__.NaN, __v3t__.NaN);
        }

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal != __v3t__.Zero;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal == __v3t__.Zero;
        }

        public __ftype__ Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vec.Cross(Axis0, Axis1).Length * __pi__;
        }
    }

    #endregion

    //# }
}
