using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

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

        public static Flags32 OrPlane(this Flags32 flags, int i)
        {
            return flags | (Flags32)((int)flags | (int)Flags32.Plane00 << i);
        }
        public static Flags32 AndNotPlane(this Flags32 flags, int i)
        {
            return flags & (Flags32)~((int)flags | (int)Flags32.Plane00 << i);
        }

        #endregion
    }

    #region Hull3f

    /// <summary>
    /// A hull is a set of planes that bounds a convex polyhedron.
    /// Normals are expected to point outside.
    /// </summary>
    public struct Hull3f
    {
        public Plane3f[] PlaneArray;

        #region Constructors

        /// <summary>
        /// Create an empty Hull3f with count planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3f(int count)
        {
            PlaneArray = new Plane3f[count];
        }

        /// <summary>
        /// Creates a Hull3f from the given planes.
        /// The plane normals are expected to point outside.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3f(Plane3f[] planes)
        {
            PlaneArray = planes;
        }

        /// <summary>
        /// Creates a Hull3f from the given box where plane normals point outside.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3f(Box3f box)
        {
            PlaneArray = new[]
            {
                new Plane3f(-V3f.XAxis, box.Min),
                new Plane3f(-V3f.YAxis, box.Min),
                new Plane3f(-V3f.ZAxis, box.Min),
                new Plane3f(V3f.XAxis, box.Max),
                new Plane3f(V3f.YAxis, box.Max),
                new Plane3f(V3f.ZAxis, box.Max)
            };
        }

        /// <summary>
        /// Creates a Hull3f from another Hull3f.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3f(Hull3f hull)
        {
            PlaneArray = hull.PlaneArray.Copy();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3f(Hull3d hull)
        {
            PlaneArray = hull.PlaneArray.Map(p => new Plane3f(p));
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Hull3f(Hull3d h)
            => new Hull3f(h);

        #endregion

        #region Constants

        public static Hull3f Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Hull3f(null);
        }

        #endregion

        #region Properties

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray != null;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray == null;
        }

        public int PlaneCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray.Length;
        }

        #endregion

        #region Transformation

        public Hull3f Transformed(Trafo3f trafo)
        {
            int count = PlaneCount;
            var hull = new Hull3f(count);
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane3f(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
            return hull;
        }

        public void TransformInto(Trafo3f trafo, ref Hull3f hull)
        {
            int count = PlaneCount;
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane3f(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
        }

        #endregion

        #region Reversal

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3f Reversed()
            => new Hull3f(PlaneArray.Map(p => p.Reversed));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reverse()
            => PlaneArray.Apply(p => p.Reversed);

        #endregion
    }

    /// <summary>
    /// A fast hull is a set of planes bounding a convex polyhedron,
    /// that can be quickly tested against intersection of an axis-
    /// aligned bounding box.
    /// </summary>
    public struct FastHull3f
    {
        public Hull3f Hull;
        public int[] MinCornerIndexArray;

        #region Constructor

        /// <summary>
        /// Create an empty FastHull3f with count planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastHull3f(int count)
        {
            Hull = new Hull3f(count);
            MinCornerIndexArray = new int[count];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastHull3f(FastHull3f fastHull)
        {
            Hull = new Hull3f(fastHull.Hull);
            MinCornerIndexArray = fastHull.MinCornerIndexArray.Copy();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastHull3f(Hull3f hull)
        {
            Hull = hull;
            MinCornerIndexArray = ComputeMinCornerIndexArray(Hull.PlaneArray);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastHull3f(FastHull3d fastHull)
        {
            Hull = new Hull3f(fastHull.Hull);
            MinCornerIndexArray = fastHull.MinCornerIndexArray.Copy();
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator FastHull3f(FastHull3d h)
            => new FastHull3f(h);

        #endregion

        #region Private Helper Methods

        private static int[] ComputeMinCornerIndexArray(Plane3f[] planeArray)
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
            Plane3f[] planeArray, int[] minCornerIndexArray)
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
        /// be ordered as returned by the <see pref="corners"/>
        /// call of the axis aligned bounding box. The avaibility of this
        /// corner array slightly improves the performance of the test. 
        /// Note that this is a conservative test, since in some cases
        /// around the edges of the hull it may return true although the
        /// hull does not intersect the box.
        /// </summary>
        public bool IntersectsAxisAlignedBox(
                V3f[] corners)
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

        public FastHull3f Transformed(Trafo3f trafo)
        {
            var newFastHull = new FastHull3f()
            {
                Hull = this.Hull.Transformed(trafo)
            };
            newFastHull.MinCornerIndexArray =
                ComputeMinCornerIndexArray(newFastHull.Hull.PlaneArray);
            return newFastHull;
        }

        public void TransformInto(Trafo3f trafo, ref FastHull3f fastHull)
        {
            Hull.TransformInto(trafo, ref fastHull.Hull);
            ComputeMinCornerIndexArrayInto(fastHull.Hull.PlaneArray,
                                        fastHull.MinCornerIndexArray);
        }

        #endregion
    }

    public static class Hull3fExtensions
    {
        /// <summary>
         /// Returns unordered set of corners of this hull.
         /// </summary>
        public static HashSet<V3f> ComputeCorners(this Hull3f hull)
        {
            var corners = new HashSet<V3f>();
            int count = hull.PlaneArray.Length;
            for (var i0 = 0; i0 < count; i0++)
            {
                for (var i1 = i0 + 1; i1 < count; i1++)
                {
                    for (var i2 = i1 + 1; i2 < count; i2++)
                    {
                        if (hull.PlaneArray[i0].Intersects(hull.PlaneArray[i1], hull.PlaneArray[i2], out V3f temp))
                        {
                            if (temp.IsNaN || temp.AnyInfinity) continue;

                            var inside = true;
                            for (var j = 0; j < count; j++)
                            {
                                if (j == i0 || j == i1 || j == i2) continue;
                                var h = hull.PlaneArray[j].Height(temp);
                                if (h > 0) { inside = false; break; }
                            }

                            if (inside)
                            {
                                corners.Add(temp);
                            }
                        }
                    }
                }
            }

            return corners;
        }
    }

    #endregion

    #region Hull3d

    /// <summary>
    /// A hull is a set of planes that bounds a convex polyhedron.
    /// Normals are expected to point outside.
    /// </summary>
    public struct Hull3d
    {
        public Plane3d[] PlaneArray;

        #region Constructors

        /// <summary>
        /// Create an empty Hull3d with count planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3d(int count)
        {
            PlaneArray = new Plane3d[count];
        }

        /// <summary>
        /// Creates a Hull3d from the given planes.
        /// The plane normals are expected to point outside.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3d(Plane3d[] planes)
        {
            PlaneArray = planes;
        }

        /// <summary>
        /// Creates a Hull3d from the given box where plane normals point outside.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3d(Box3d box)
        {
            PlaneArray = new[]
            {
                new Plane3d(-V3d.XAxis, box.Min),
                new Plane3d(-V3d.YAxis, box.Min),
                new Plane3d(-V3d.ZAxis, box.Min),
                new Plane3d(V3d.XAxis, box.Max),
                new Plane3d(V3d.YAxis, box.Max),
                new Plane3d(V3d.ZAxis, box.Max)
            };
        }

        /// <summary>
        /// Creates a Hull3d from another Hull3d.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3d(Hull3d hull)
        {
            PlaneArray = hull.PlaneArray.Copy();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3d(Hull3f hull)
        {
            PlaneArray = hull.PlaneArray.Map(p => new Plane3d(p));
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Hull3d(Hull3f h)
            => new Hull3d(h);

        #endregion

        #region Constants

        public static Hull3d Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new Hull3d(null);
        }

        #endregion

        #region Properties

        public bool IsValid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray != null;
        }

        public bool IsInvalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray == null;
        }

        public int PlaneCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => PlaneArray.Length;
        }

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

        #endregion

        #region Reversal

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Hull3d Reversed()
            => new Hull3d(PlaneArray.Map(p => p.Reversed));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reverse()
            => PlaneArray.Apply(p => p.Reversed);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastHull3d(int count)
        {
            Hull = new Hull3d(count);
            MinCornerIndexArray = new int[count];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastHull3d(FastHull3d fastHull)
        {
            Hull = new Hull3d(fastHull.Hull);
            MinCornerIndexArray = fastHull.MinCornerIndexArray.Copy();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastHull3d(Hull3d hull)
        {
            Hull = hull;
            MinCornerIndexArray = ComputeMinCornerIndexArray(Hull.PlaneArray);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public FastHull3d(FastHull3f fastHull)
        {
            Hull = new Hull3d(fastHull.Hull);
            MinCornerIndexArray = fastHull.MinCornerIndexArray.Copy();
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator FastHull3d(FastHull3f h)
            => new FastHull3d(h);

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
        /// be ordered as returned by the <see pref="corners"/>
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
        /// <summary>
         /// Returns unordered set of corners of this hull.
         /// </summary>
        public static HashSet<V3d> ComputeCorners(this Hull3d hull)
        {
            var corners = new HashSet<V3d>();
            int count = hull.PlaneArray.Length;
            for (var i0 = 0; i0 < count; i0++)
            {
                for (var i1 = i0 + 1; i1 < count; i1++)
                {
                    for (var i2 = i1 + 1; i2 < count; i2++)
                    {
                        if (hull.PlaneArray[i0].Intersects(hull.PlaneArray[i1], hull.PlaneArray[i2], out V3d temp))
                        {
                            if (temp.IsNaN || temp.AnyInfinity) continue;

                            var inside = true;
                            for (var j = 0; j < count; j++)
                            {
                                if (j == i0 || j == i1 || j == i2) continue;
                                var h = hull.PlaneArray[j].Height(temp);
                                if (h > 0) { inside = false; break; }
                            }

                            if (inside)
                            {
                                corners.Add(temp);
                            }
                        }
                    }
                }
            }

            return corners;
        }
    }

    #endregion

}
