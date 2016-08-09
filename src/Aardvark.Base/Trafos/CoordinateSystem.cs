using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static class CoordinateSystem
    {
        [Flags]
        public enum Axis
        {
            X = 1,
            Y = 2,
            Z = 4
        }

        public enum Handedness
        {
            Left,
            Right
        }

        public struct Info
        {
            public double UnitScale;
            public Handedness Handedness;
            public Axis UpVector;

            public Info(double s, Handedness h, Axis u)
            {
                UnitScale = s;
                Handedness = h;
                UpVector = u;
            }

            public static readonly Info Vrml = new Info(1, Handedness.Right, Axis.Y);

            public static readonly Info Aardvark = new Info(1, Handedness.Right, Axis.Z);

            public bool IsAardvark
            {
                get { return this.Equals(Aardvark); }
            }
        }

        /// <summary>
        /// Creates a transformation from the specified coordinate system  
        /// to the aardvark coordinate system (Meters, Right-Handed, Z-Up).
        /// </summary>
        public static Trafo3d ToAardvark(double scale, Handedness hand, Axis up)
        {
            var t = scale != 1 ? Trafo3d.Scale(scale) : Trafo3d.Identity;

            if (hand != Handedness.Right)
                t *= SwapHand;

            if (up != Axis.Z)
                t *= FromToRH(up, Axis.Z);

            return t;
        }

        /// <summary>
        /// Creates a transformation from the specified coordinate system  
        /// to the aardvark coordinate system (Meters, Right-Handed, Z-Up).
        /// </summary>
        public static Trafo3d ToAardvark(this Info from)
        {
            return ToAardvark(from.UnitScale, from.Handedness, from.UpVector);
        }

        /// <summary>
        /// Creates a transformation from the specified coordinate system  
        /// to the aardvark coordinate system (Meters, Right-Handed, Z-Up).
        /// </summary>
        public static Trafo3d ToAardvark(Handedness hand, Axis up)
        {
            return ToAardvark(1, hand, up);
        }

        /// <summary>
        /// Creates a transformation from the specified coordinate system  
        /// to the aardvark coordinate system (Meters, Right-Handed, Z-Up).
        /// </summary>
        public static Trafo3d ToAardvark(Axis up)
        {
            return ToAardvark(1, Handedness.Right, up);
        }

        /// <summary>
        /// Creates a transformation from the specified coordinate system  
        /// to the aardvark coordinate system (Meters, Right-Handed, Z-Up).
        /// </summary>
        public static Trafo3d ToAardvark(Handedness hand)
        {
            return ToAardvark(1, hand, Axis.Z);
        }

        /// <summary>
        /// Gets the cooresponding vector for a given axis
        /// </summary>
        public static V3d GetAxisVector(this Axis ax)
        {
            return ax == Axis.X ? V3d.XAxis :
                   ax == Axis.Y ? V3d.YAxis :
                                  V3d.ZAxis;
        }

        /// <summary>
        /// Builds transformation from one coordinate system to another.
        /// </summary>
        public static Trafo3d FromTo(Info from, Info to)
        {
            var t = Trafo3d.Identity;
            if (from.UnitScale != to.UnitScale)
                t = Trafo3d.Scale(to.UnitScale / from.UnitScale);

            if (from.Handedness != to.Handedness)
                t *= SwapHand;

            if (from.UpVector != to.UpVector)
                t *= FromToRH(from.UpVector, to.UpVector);

            return t;
        }

        static Trafo3d FromTo(Axis from, Axis to, int s)
        {
            if (from == to)
                return Trafo3d.Identity;

            var axis = (from == Axis.X || to == Axis.X) ?
                          (from == Axis.Y || to == Axis.Y) ? 
                              Axis.Z : Axis.Y : Axis.X;

            var rotation = axis == Axis.X ? new M44d(1, 0, 0, 0,  0, 0,-s, 0,  0, s, 0, 0,  0, 0, 0, 1) :
                           axis == Axis.Y ? new M44d(0, 0, s, 0,  0, 1, 0, 0, -s, 0, 0, 0,  0, 0, 0, 1) :
                                            new M44d(0,-s, 0, 0,  s, 0, 0, 0,  0, 0, 1, 0,  0, 0, 0, 1);

            return new Trafo3d(rotation, rotation.Transposed);
        }

        public static Trafo3d FromToRH(Axis from, Axis to)
        {
            var s = Fun.Sign(((int)to) - ((int)from)); // +/- sin(90°)

            return FromTo(from, to, s);
        }

        public static Trafo3d FromToLH(Axis from, Axis to)
        {
            var s = ((int)to) > ((int)from) ? -1 : 1; // +/- sin(90°)

            return FromTo(from, to, s);
        }

        static M44d s_swapHand = new M44d(
                                     1, 0, 0, 0,
                                     0, 1, 0, 0,
                                     0, 0,-1, 0,
                                     0, 0, 0, 1);

        public static readonly Trafo3d SwapHand = new Trafo3d(s_swapHand, s_swapHand);
    }
}
