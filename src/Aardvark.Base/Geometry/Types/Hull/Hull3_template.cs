using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Hull3" + tc;
    //#   var type2 = "Hull3" + tc2;
    //#   var v3t = "V3" + tc;
    //#   var box3t = "Box3" + tc;
    //#   var plane3t = "Plane3" + tc;
    //#   var trafo3t = "Trafo3" + tc;
    //#   var iboundingbox = "IBoundingBox3" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var dotnine = isDouble ? "0.9" : "0.9f";
    //#   var constant = isDouble ? "Constant" : "ConstantF";
    #region __type__

    /// <summary>
    /// A hull is a set of planes that bounds a convex polyhedron.
    /// Normals are expected to point outside.
    /// </summary>
    public struct __type__ : IEquatable<__type__>, IValidity
    {
        public __plane3t__[] PlaneArray;

        #region Constructors

        /// <summary>
        /// Create an empty __type__ with count planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(int count)
        {
            PlaneArray = new __plane3t__[count];
        }

        /// <summary>
        /// Creates a __type__ from the given planes.
        /// The plane normals are expected to point outside.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__plane3t__[] planes)
        {
            PlaneArray = planes;
        }

        /// <summary>
        /// Creates a __type__ from the given box where plane normals point outside.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__box3t__ box)
        {
            PlaneArray = new[]
            {
                new __plane3t__(-__v3t__.XAxis, box.Min),
                new __plane3t__(-__v3t__.YAxis, box.Min),
                new __plane3t__(-__v3t__.ZAxis, box.Min),
                new __plane3t__(__v3t__.XAxis, box.Max),
                new __plane3t__(__v3t__.YAxis, box.Max),
                new __plane3t__(__v3t__.ZAxis, box.Max)
            };
        }

        /// <summary>
        /// Creates a __type__ from another __type__.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ hull)
        {
            PlaneArray = hull.PlaneArray.Copy();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ hull)
        {
            PlaneArray = hull.PlaneArray.Map(p => new __plane3t__(p));
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator __type__(__type2__ h)
            => new __type__(h);

        #endregion

        #region Constants

        public static __type__ Invalid
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new __type__(null);
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

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ a, __type__ b)
            => a.Equals(b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
            => !a.Equals(b);

        #endregion

        #region Override

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override int GetHashCode()
        {
            if (PlaneArray == null || PlaneArray.Length == 0) return 0;
            var h = PlaneArray[0].GetHashCode();
            for (var i = 1; i < PlaneArray.Length; i++) HashCode.GetCombined(h, PlaneArray[i].GetHashCode());
            return h;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
        {
            if (PlaneArray == null || other.PlaneArray == null) return false;
            for (var i = 0; i < PlaneArray.Length; i++) if (PlaneArray[i] != other.PlaneArray[i]) return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public override string ToString()
            => PlaneArray != null
                ? string.Format(CultureInfo.InvariantCulture, "[{0}]", string.Join(",", PlaneArray.Map(x => x.ToString())))
                : "[null]"
                ;

        /// <summary>
        /// Parses __type__ from a string created with __type__.ToString().
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Parse(string s)
        {
            if (s == "[null]") return __type__.Invalid;
            var planes = s.NestedBracketSplitLevelOne().Select(__plane3t__.Parse).ToArray();
            return new __type__(planes);
        }

        #endregion

        #region Transformation

        public __type__ Transformed(__trafo3t__ trafo)
        {
            int count = PlaneCount;
            var hull = new __type__(count);
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new __plane3t__(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
            return hull;
        }

        public void TransformInto(__trafo3t__ trafo, ref __type__ hull)
        {
            int count = PlaneCount;
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new __plane3t__(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
        }

        #endregion

        #region Reversal

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__ Reversed()
            => new __type__(PlaneArray.Map(p => p.Reversed));

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
    public struct Fast__type__
    {
        public __type__ Hull;
        public int[] MinCornerIndexArray;

        #region Constructor

        /// <summary>
        /// Create an empty Fast__type__ with count planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fast__type__(int count)
        {
            Hull = new __type__(count);
            MinCornerIndexArray = new int[count];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fast__type__(Fast__type__ fastHull)
        {
            Hull = new __type__(fastHull.Hull);
            MinCornerIndexArray = fastHull.MinCornerIndexArray.Copy();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fast__type__(__type__ hull)
        {
            Hull = hull;
            MinCornerIndexArray = ComputeMinCornerIndexArray(Hull.PlaneArray);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Fast__type__(Fast__type2__ fastHull)
        {
            Hull = new __type__(fastHull.Hull);
            MinCornerIndexArray = fastHull.MinCornerIndexArray.Copy();
        }

        #endregion

        #region Conversions

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static explicit operator Fast__type__(Fast__type2__ h)
            => new Fast__type__(h);

        #endregion

        #region Private Helper Methods

        private static int[] ComputeMinCornerIndexArray(__plane3t__[] planeArray)
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
            __plane3t__[] planeArray, int[] minCornerIndexArray)
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
                __v3t__[] corners)
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

        public Fast__type__ Transformed(__trafo3t__ trafo)
        {
            var newFastHull = new Fast__type__()
            {
                Hull = this.Hull.Transformed(trafo)
            };
            newFastHull.MinCornerIndexArray =
                ComputeMinCornerIndexArray(newFastHull.Hull.PlaneArray);
            return newFastHull;
        }

        public void TransformInto(__trafo3t__ trafo, ref Fast__type__ fastHull)
        {
            Hull.TransformInto(trafo, ref fastHull.Hull);
            ComputeMinCornerIndexArrayInto(fastHull.Hull.PlaneArray,
                                        fastHull.MinCornerIndexArray);
        }

        #endregion
    }

    public static class __type__Extensions
    {
        /// <summary>
         /// Returns unordered set of corners of this hull.
         /// </summary>
        public static HashSet<__v3t__> ComputeCorners(this __type__ hull)
        {
            var corners = new HashSet<__v3t__>();
            int count = hull.PlaneArray.Length;
            for (var i0 = 0; i0 < count; i0++)
            {
                for (var i1 = i0 + 1; i1 < count; i1++)
                {
                    for (var i2 = i1 + 1; i2 < count; i2++)
                    {
                        if (hull.PlaneArray[i0].Intersects(hull.PlaneArray[i1], hull.PlaneArray[i2], out __v3t__ temp))
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

    //# }
}
