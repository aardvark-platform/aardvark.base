using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Ellipse2f

    public partial struct Ellipse2f : IValidity
    {
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => true;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }

        public float Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Fun.Abs(Axis0.X * Axis1.Y - Axis0.Y * Axis1.X) * ConstantF.Pi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static float GetArea(V2f axis0, V2f axis1)
            => Fun.Abs(axis0.X * axis1.Y - axis0.Y * axis1.X) * ConstantF.Pi;
    }

    #endregion

    #region Ellipse2d

    public partial struct Ellipse2d : IValidity
    {
        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => true;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => false;
        }

        public double Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Fun.Abs(Axis0.X * Axis1.Y - Axis0.Y * Axis1.X) * Constant.Pi;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static double GetArea(V2d axis0, V2d axis1)
            => Fun.Abs(axis0.X * axis1.Y - axis0.Y * axis1.X) * Constant.Pi;
    }

    #endregion

}
