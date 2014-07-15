using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class Hull
    {
        #region Flags32

        [Flags]
        public enum Flags32
        {
            None = 0x00000000,
            Plane00 = 0x00000001,
            Plane01 = 0x00000002,
            Plane02 = 0x00000004,
            Plane03 = 0x00000008,
            Plane04 = 0x00000010,
            Plane05 = 0x00000020,
            Plane06 = 0x00000040,
            Plane07 = 0x00000080,
            Plane08 = 0x00000100,
            Plane09 = 0x00000200,
            Plane10 = 0x00000400,
            Plane11 = 0x00000800,
            Plane12 = 0x00001000,
            Plane13 = 0x00002000,
            Plane14 = 0x00004000,
            Plane15 = 0x00008000,
            Plane16 = 0x00010000,
            Plane17 = 0x00020000,
            Plane18 = 0x00040000,
            Plane19 = 0x00080000,
            Plane20 = 0x00100000,
            Plane21 = 0x00200000,
            Plane22 = 0x00400000,
            Plane23 = 0x00800000,
            Plane24 = 0x01000000,
            Plane25 = 0x02000000,
            Plane26 = 0x04000000,
            Plane27 = 0x08000000,
            Plane28 = 0x10000000,
            Plane29 = 0x20000000,
            Plane30 = 0x40000000,
            Plane31 = (int)-0x80000000,
        }

        public static Hull.Flags32 OrPlane(this Hull.Flags32 flags, int i)
        {
            return flags | (Hull.Flags32)((int)flags | (int)Hull.Flags32.Plane00 << i);
        }
        public static Hull.Flags32 AndNotPlane(this Hull.Flags32 flags, int i)
        {
            return flags & (Hull.Flags32)~((int)flags | (int)Hull.Flags32.Plane00 << i);
        }

        #endregion
    }

    /// <summary>
    /// A hull is a set of planes that bounds a convex polyhedron. Notably
    /// a frustum can be represented as a hull.
    /// known issue: Hull3d.Intersects() expect the normals to point outside, but Hull3d(Frustum3d)-Normals point inside
    /// </summary>
    public struct Hull3d
    {
        public Plane3d[] PlaneArray;

        #region Constructors

        /// <summary>
        /// Create an empty Hull3d with count planes.
        /// </summary>
        public Hull3d(int count)
        {
            PlaneArray = new Plane3d[count];
        }

        public Hull3d(Plane3d[] planes)
        {
            PlaneArray = planes;
        }

        public Hull3d(Box3d box)
        {
            PlaneArray = new[]
            {
                new Plane3d(V3d.XAxis, box.Min),
                new Plane3d(V3d.YAxis, box.Min),
                new Plane3d(V3d.ZAxis, box.Min),
                new Plane3d(-V3d.XAxis, box.Max),
                new Plane3d(-V3d.YAxis, box.Max),
                new Plane3d(-V3d.ZAxis, box.Max)
            };
        }

        public Hull3d(Hull3d hull)
        {
            PlaneArray = hull.PlaneArray.Copy();
        }

        #endregion

        #region Constants

        public static readonly Hull3d Invalid = new Hull3d(null);

        #endregion

        #region Properties

        public bool IsValid { get { return PlaneArray != null; } }
        public bool IsInvalid { get { return PlaneArray == null; } }

        public int PlaneCount { get { return PlaneArray.Length; } }

        #endregion

        #region Transformation

        public Hull3d Transformed(Trafo3d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull3d(count);
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane3d(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
            return hull;
        }

        public void TransformInto(Trafo3d trafo, ref Hull3d hull)
        {
            int count = PlaneCount;
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane3d(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
        }

        public Hull3d Reversed()
        {
            return new Hull3d(PlaneArray.Copy(p => p.Reversed));
        }

        public void Reverse()
        {
            PlaneArray.Apply(p => p.Reversed);
        }

        #endregion
    }

    /// <summary>
    /// A fast hull is a set of planes bounding a convex polyhedron,
    /// that can be quickly tested against intersection of an axis-
    /// aligned bounding box.
    /// </summary>
    public struct FastHull3d
    {
        public Hull3d Hull;
        public int[] MinCornerIndexArray;

        #region Constructor

        /// <summary>
        /// Create an empty FastHull3d with count planes.
        /// </summary>
        public FastHull3d(int count)
        {
            Hull = new Hull3d(count);
            MinCornerIndexArray = new int[count];
        }

        public FastHull3d(FastHull3d fastHull)
        {
            Hull = new Hull3d(fastHull.Hull);
            MinCornerIndexArray = fastHull.MinCornerIndexArray.Copy();
        }

        public FastHull3d(Hull3d hull)
        {
            Hull = hull;
            MinCornerIndexArray = ComputeMinCornerIndexArray(Hull.PlaneArray);
        }

        #endregion

        #region Private Helper Methods

        private static int[] ComputeMinCornerIndexArray(Plane3d[] planeArray)
        {
            int count = planeArray.Length;
            var minCornerIndices = new int[count];
            for (int pi = 0; pi < count; pi++)
            {
                int minCorner = 0;

                if (planeArray[pi].Normal.X < 0) minCorner |= 1;
                if (planeArray[pi].Normal.Y < 0) minCorner |= 2;
                if (planeArray[pi].Normal.Z < 0) minCorner |= 4;

                minCornerIndices[pi] = minCorner;
            }
            return minCornerIndices;
        }

        private static void ComputeMinCornerIndexArrayInto(
            Plane3d[] planeArray, int[] minCornerIndexArray)
        {
            int count = planeArray.Length;
            for (int pi = 0; pi < count; pi++)
            {
                int minCorner = 0;

                if (planeArray[pi].Normal.X < 0) minCorner |= 1;
                if (planeArray[pi].Normal.Y < 0) minCorner |= 2;
                if (planeArray[pi].Normal.Z < 0) minCorner |= 4;

                minCornerIndexArray[pi] = minCorner;
            }
        }

        #endregion

        #region Intersection Methods

        /// <summary>
        /// Test hull against intersection of the supplied bounding box
        /// specified by an array of its eight corner vertices, that must
        /// be ordered as returned by the <see cref="ComputeCorners"/>
        /// call of the axis aligned bounding box. The avaibility of this
        /// corner array slightly improves the performance of the test. 
        /// Note that this is a conservative test, since in some cases
        /// around the edges of the hull it may return true although the
        /// hull does not intersect the box.
        /// </summary>
        public bool IntersectsAxisAlignedBox(
                V3d[] corners)
        {
            var planes = Hull.PlaneArray;
            int count = planes.Length;
            bool intersecting = false;
            for (int pi = 0; pi < count; pi++)
            {
                int minCornerIndex = MinCornerIndexArray[pi];
                if (planes[pi].Height(corners[minCornerIndex]) > 0)
                    return false;
                if (planes[pi].Height(corners[minCornerIndex ^ 7]) >= 0)
                    intersecting = true;
            }
            if (intersecting) return true;
            return true;
        }

        #endregion

        #region Transformations

        public FastHull3d Transformed(Trafo3d trafo)
        {
            var newFastHull = new FastHull3d()
            {
                Hull = this.Hull.Transformed(trafo)
            };
            newFastHull.MinCornerIndexArray =
                ComputeMinCornerIndexArray(newFastHull.Hull.PlaneArray);
            return newFastHull;
        }

        public void TransformInto(Trafo3d trafo, ref FastHull3d fastHull)
        {
            Hull.TransformInto(trafo, ref fastHull.Hull);
            ComputeMinCornerIndexArrayInto(fastHull.Hull.PlaneArray,
                                        fastHull.MinCornerIndexArray);
        }

        #endregion
    }

    public static class Hull3dExtensions
    {
        public static List<V3d> ComputeCorners(this Hull3d hull)
        {
            List<V3d> Corners = new List<V3d>();
            int count = hull.PlaneArray.Length;
            for (int i0 = 0; i0 < count; i0++)
            {
                for (int i1 = 0; i1 < count; i1++)
                {
                    for (int i2 = 0; i2 < count; i2++)
                    {
                        if (i0 != i1 && i1 != i2 && i0 != i2)
                        {
                            V3d temp;
                            if (hull.PlaneArray[0].Intersects(hull.PlaneArray[i1], hull.PlaneArray[i2], out temp))
                            {
                                if(!temp.IsNaN && !temp.AnyInfinity)
                                    Corners.Add(temp);
                            }
                        }
                    }
                }
            }

            return Corners;
        }
    }
}
