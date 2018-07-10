using System;

namespace Aardvark.Base
{
    public static class BoxExtensions
    {
        /// <summary>
        /// Octant 0-7.
        /// 0th, 1st and 2nd bit encodes x-, y-, z-axis, respectively.
        /// E.g. 0 is octant [box.Min, box.Center], 7 is octant [box.Center, box.Max].
        /// </summary>
        public static Box3d GetOctant(this Box3d box, int i)
        {
            if (i < 0 || i > 7) throw new IndexOutOfRangeException();

            double x0, x1, y0, y1, z0, z1;
            var c = box.Center;
            if ((i & 1) == 0) { x0 = box.Min.X; x1 = c.X; } else { x0 = c.X; x1 = box.Max.X; }
            if ((i & 2) == 0) { y0 = box.Min.Y; y1 = c.Y; } else { y0 = c.Y; y1 = box.Max.Y; }
            if (i < 4) { z0 = box.Min.Z; z1 = c.Z; } else { z0 = c.Z; z1 = box.Max.Z; }
            return new Box3d(new V3d(x0, y0, z0), new V3d(x1, y1, z1));
        }

        /// <summary>
        /// Gets outline corner indices of a box as seen from given position (in counter-clockwise order).
        /// Returns null if position is inside the box.
        /// </summary>
        public static int[] GetOutlineCornerIndicesCCW(this Box3d box, V3d fromPosition)
        {
            if (fromPosition.X < box.Min.X)
            { //-X
                if (fromPosition.Y < box.Min.Y)
                { //-Y
                    if (fromPosition.Z < box.Min.Z)
                    { // -X -Y -Z
                        return new[] { 1, 5, 4, 6, 2, 3 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // -X -Y +Z
                        return new[] { 0, 1, 5, 7, 6, 2 };
                    }
                    else
                    { // -X -Y  Z
                        return new[] { 1, 5, 6, 2 };
                    }
                }
                else if (fromPosition.Y > box.Max.Y)
                { // +Y
                    if (fromPosition.Z < box.Min.Z)
                    { // -X +Y -Z
                        return new[] { 0, 4, 6, 7, 3, 1 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // -X +Y +Z
                        return new[] { 0, 4, 5, 7, 3, 2 };
                    }
                    else
                    { // -X +Y  Z
                        return new[] { 0, 4, 7, 3 };
                    }
                }
                else
                { // Y
                    if (fromPosition.Z < box.Min.Z)
                    { // -X  Y -Z
                        return new[] { 1, 4, 6, 3 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // -X  Y +Z
                        return new[] { 0, 5, 7, 2 };
                    }
                    else
                    { // -X  Y  Z
                        return new[] { 0, 4, 6, 2 };
                    }
                }
            }
            else if (fromPosition.X > box.Max.X)
            { // +X
                if (fromPosition.Y < box.Min.Y)
                { //-Y
                    if (fromPosition.Z < box.Min.Z)
                    { // +X -Y -Z
                        return new[] { 0, 2, 3, 7, 5, 4 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // +X -Y +Z
                        return new[] { 0, 1, 3, 7, 6, 4 };
                    }
                    else
                    { // +X -Y  Z
                        return new[] { 0, 3, 7, 4 };
                    }
                }
                else if (fromPosition.Y > box.Max.Y)
                { // +Y
                    if (fromPosition.Z < box.Min.Z)
                    { // +X +Y -Z
                        return new[] { 0, 2, 6, 7, 5, 1 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // +X +Y +Z
                        return new[] { 1, 3, 2, 6, 4, 5 };
                    }
                    else
                    { // +X +Y  Z
                        return new[] { 1, 2, 6, 5 };
                    }
                }
                else
                { // Y
                    if (fromPosition.Z < box.Min.Z)
                    { // +X  Y -Z
                        return new[] { 0, 2, 7, 5 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // +X  Y +Z
                        return new[] { 1, 3, 6, 4 };
                    }
                    else
                    { // +X  Y  Z
                        return new[] { 1, 3, 7, 5 };
                    }
                }
            }
            else
            { // X
                if (fromPosition.Y < box.Min.Y)
                { //-Y
                    if (fromPosition.Z < box.Min.Z)
                    { //  X -Y -Z
                        return new[] { 2, 3, 5, 4 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { //  X -Y +Z
                        return new[] { 0, 1, 7, 6 };
                    }
                    else
                    { //  X -Y  Z
                        return new[] { 0, 1, 5, 4 };
                    }
                }
                else if (fromPosition.Y > box.Max.Y)
                { // +Y
                    if (fromPosition.Z < box.Min.Z)
                    { //  X +Y -Z
                        return new[] { 0, 6, 7, 1 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { //  X +Y +Z
                        return new[] { 2, 4, 5, 3 };
                    }
                    else
                    { //  X +Y  Z
                        return new[] { 2, 6, 7, 3 };
                    }
                }
                else
                { // Y
                    if (fromPosition.Z < box.Min.Z)
                    { //  X  Y -Z
                        return new[] { 0, 2, 3, 1 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { //  X  Y +Z
                        return new[] { 4, 5, 7, 6 };
                    }
                    else
                    { //  X  Y  Z
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets outline corner indices of a box as seen from given position (in clockwise order).
        /// Returns null if position is inside the box.
        /// </summary>
        public static int[] GetOutlineCornerIndicesCW(this Box3d box, V3d fromPosition)
        {
            if (fromPosition.X < box.Min.X)
            { //-X
                if (fromPosition.Y < box.Min.Y)
                { //-Y
                    if (fromPosition.Z < box.Min.Z)
                    { // -X -Y -Z
                        return new[] { 1, 3, 2, 6, 4, 5 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // -X -Y +Z
                        return new[] { 0, 2, 6, 7, 5, 1 };
                    }
                    else
                    { // -X -Y  Z
                        return new[] { 1, 2, 6, 5 };
                    }
                }
                else if (fromPosition.Y > box.Max.Y)
                { // +Y
                    if (fromPosition.Z < box.Min.Z)
                    { // -X +Y -Z
                        return new[] { 0, 1, 3, 7, 6, 4 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // -X +Y +Z
                        return new[] { 0, 2, 3, 7, 5, 4 };
                    }
                    else
                    { // -X +Y  Z
                        return new[] { 0, 3, 7, 4 };
                    }
                }
                else
                { // Y
                    if (fromPosition.Z < box.Min.Z)
                    { // -X  Y -Z
                        return new[] { 1, 3, 6, 4 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // -X  Y +Z
                        return new[] { 0, 2, 7, 5 };
                    }
                    else
                    { // -X  Y  Z
                        return new[] { 0, 2, 6, 4 };
                    }
                }
            }
            else if (fromPosition.X > box.Max.X)
            { // +X
                if (fromPosition.Y < box.Min.Y)
                { //-Y
                    if (fromPosition.Z < box.Min.Z)
                    { // +X -Y -Z
                        return new[] { 0, 4, 5, 7, 3, 2 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // +X -Y +Z
                        return new[] { 0, 4, 6, 7, 3, 1 };
                    }
                    else
                    { // +X -Y  Z
                        return new[] { 0, 4, 7, 3 };
                    }
                }
                else if (fromPosition.Y > box.Max.Y)
                { // +Y
                    if (fromPosition.Z < box.Min.Z)
                    { // +X +Y -Z
                        return new[] { 0, 1, 5, 7, 6, 2 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // +X +Y +Z
                        return new[] { 1, 5, 4, 6, 2, 3 };
                    }
                    else
                    { // +X +Y  Z
                        return new[] { 1, 5, 6, 2 };
                    }
                }
                else
                { // Y
                    if (fromPosition.Z < box.Min.Z)
                    { // +X  Y -Z
                        return new[] { 0, 5, 7, 2 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { // +X  Y +Z
                        return new[] { 1, 4, 6, 3 };
                    }
                    else
                    { // +X  Y  Z
                        return new[] { 1, 5, 7, 3 };
                    }
                }
            }
            else
            { // X
                if (fromPosition.Y < box.Min.Y)
                { //-Y
                    if (fromPosition.Z < box.Min.Z)
                    { //  X -Y -Z
                        return new[] { 2, 4, 5, 3 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { //  X -Y +Z
                        return new[] { 0, 6, 7, 1 };
                    }
                    else
                    { //  X -Y  Z
                        return new[] { 0, 4, 5, 1 };
                    }
                }
                else if (fromPosition.Y > box.Max.Y)
                { // +Y
                    if (fromPosition.Z < box.Min.Z)
                    { //  X +Y -Z
                        return new[] { 0, 1, 7, 6 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { //  X +Y +Z
                        return new[] { 2, 3, 5, 4 };
                    }
                    else
                    { //  X +Y  Z
                        return new[] { 2, 3, 7, 6 };
                    }
                }
                else
                { // Y
                    if (fromPosition.Z < box.Min.Z)
                    { //  X  Y -Z
                        return new[] { 0, 1, 3, 2 };
                    }
                    else if (fromPosition.Z > box.Max.Z)
                    { //  X  Y +Z
                        return new[] { 4, 6, 7, 5 };
                    }
                    else
                    { //  X  Y  Z
                        return null;
                    }
                }
            }
        }

        /// <summary>
        /// Gets outline corners of a box as seen from given position (in counter-clockwise order).
        /// Returns null if position is inside the box.
        /// </summary>
        public static V3d[] GetOutlineCornersCCW(this Box3d box, V3d fromPosition)
        {
            var cs = box.ComputeCorners();
            return GetOutlineCornerIndicesCCW(box, fromPosition)?.Map(i => cs[i]);
        }

        /// <summary>
        /// Gets outline corners of a box as seen from given position (in clockwise order).
        /// Returns null if position is inside the box.
        /// </summary>
        public static V3d[] GetOutlineCornersCW(this Box3d box, V3d fromPosition)
        {
            var cs = box.ComputeCorners();
            return GetOutlineCornerIndicesCW(box, fromPosition)?.Map(i => cs[i]);
        }
    }
}
