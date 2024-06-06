using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Quad2f

    /// <summary>
    /// A two-dimensional quadrangle specified by its four points.
    /// </summary>
    public partial struct Quad2f
    {
        #region Geometric Properties

        public readonly float Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 0.5f * ((P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y)).Abs();
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
        public readonly float WindingOrder
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y);
        }

        #endregion
    }

    #endregion

    #region Quad2d

    /// <summary>
    /// A two-dimensional quadrangle specified by its four points.
    /// </summary>
    public partial struct Quad2d
    {
        #region Geometric Properties

        public readonly double Area
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => 0.5 * ((P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y)).Abs();
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
        public readonly double WindingOrder
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (P2.X - P0.X) * (P3.Y - P1.Y) - (P3.X - P1.X) * (P2.Y - P0.Y);
        }

        #endregion
    }

    #endregion

}
