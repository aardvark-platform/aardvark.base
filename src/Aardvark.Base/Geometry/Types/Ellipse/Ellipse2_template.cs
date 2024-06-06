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
    //#   var type = "Ellipse2" + tc;
    //#   var v2t = "V2" + tc;
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    #region __type__

    public partial struct __type__ : IValidity
    {
        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => true;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }

        public readonly __ftype__ Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Fun.Abs(Axis0.X * Axis1.Y - Axis0.Y * Axis1.X) * __pi__;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static __ftype__ GetArea(__v2t__ axis0, __v2t__ axis1)
            => Fun.Abs(axis0.X * axis1.Y - axis0.Y * axis1.X) * __pi__;
    }

    #endregion

    //# }
}
