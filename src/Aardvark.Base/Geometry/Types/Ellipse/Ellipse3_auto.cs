using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Ellipse3f

    public partial struct Ellipse3f : IValidity
    {
        public static Ellipse3f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ellipse3f(V3f.NaN, V3f.Zero, V3f.NaN, V3f.NaN);
        }

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal != V3f.Zero;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal == V3f.Zero;
        }

        public readonly float Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vec.Cross(Axis0, Axis1).Length * ConstantF.Pi;
        }
    }

    #endregion

    #region Ellipse3d

    public partial struct Ellipse3d : IValidity
    {
        public static Ellipse3d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Ellipse3d(V3d.NaN, V3d.Zero, V3d.NaN, V3d.NaN);
        }

        public readonly bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal != V3d.Zero;
        }

        public readonly bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Normal == V3d.Zero;
        }

        public readonly double Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Vec.Cross(Axis0, Axis1).Length * Constant.Pi;
        }
    }

    #endregion

}
