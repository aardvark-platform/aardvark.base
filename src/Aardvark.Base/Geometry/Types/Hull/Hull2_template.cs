using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ftype = isDouble ? "double" : "float";
    //#   var ftype2 = isDouble ? "float" : "double";
    //#   var tc = isDouble ? "d" : "f";
    //#   var tc2 = isDouble ? "f" : "d";
    //#   var type = "Hull2" + tc;
    //#   var type2 = "Hull2" + tc2;
    //#   var v2t = "V2" + tc;
    //#   var box2t = "Box2" + tc;
    //#   var plane2t = "Plane2" + tc;
    //#   var plane2t2 = "Plane2" + tc2;
    //#   var trafo2t = "Trafo2" + tc;
    //#   var iboundingbox = "IBoundingBox2" + tc;
    //#   var half = isDouble ? "0.5" : "0.5f";
    //#   var pi = isDouble ? "Constant.Pi" : "ConstantF.Pi";
    #region __type__

    /// <summary>
    /// A hull is an alternative representation of a convex polygon.
    /// </summary>
    public partial struct __type__ : IEquatable<__type__>, IValidity
    {
        public __plane2t__[] PlaneArray;

        #region Constructors

        /// <summary>
        /// Create an empty Hull3d with count planes.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(int count)
        {
            PlaneArray = new __plane2t__[count];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__plane2t__[] planeArray)
        {
            PlaneArray = planeArray;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__box2t__ box)
        {
            PlaneArray = new[]
            {
                new __plane2t__(__v2t__.XAxis, box.Min),
                new __plane2t__(__v2t__.YAxis, box.Min),
                new __plane2t__(-__v2t__.XAxis, box.Max),
                new __plane2t__(-__v2t__.YAxis, box.Max),
            };
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type__ hull)
        {
            PlaneArray = hull.PlaneArray.Copy();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__(__type2__ hull)
        {
            PlaneArray = hull.PlaneArray.Map(p => new __plane2t__(p));
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
            var planes = s.NestedBracketSplitLevelOne().Select(__plane2t__.Parse).ToArray();
            return new __type__(planes);
        }

        #endregion

        #region Transformation

        public __type__ Transformed(__trafo2t__ trafo)
        {
            int count = PlaneCount;
            var hull = new __type__(new __plane2t__[count]);
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new __plane2t__(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
            return hull;
        }

        public void TransformInto(__trafo2t__ trafo, ref __type__ hull)
        {
            int count = PlaneCount;
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new __plane2t__(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public __type__ Reversed()
            => new __type__(PlaneArray.Map(p => p.Reversed));

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Reverse()
            => PlaneArray.Apply(p => p.Reversed);

        #endregion
    }

    public static class __type__Extensions
    {
        /// <summary>
        /// Creates an inward hull (i.e. a hull whose normal vectors point
        /// inside) from a counter-clockwise enumerated array of polygon
        /// points.
        /// </summary>
        public static __type__ ToInwardHull(this __v2t__[] polygon, int pointCount = 0)
        {
            if (pointCount == 0) pointCount = polygon.Length;
            var planeArray = new __plane2t__[pointCount];
            var p0 = polygon[pointCount - 1];
            for (int i = 0; i < pointCount; i++)
            {
                var p1 = polygon[i];
                planeArray[i] = new __plane2t__((p1 - p0).Rot90.Normalized, p0);
                p0 = p1;
            }
            return new __type__(planeArray);
        }

        /// <summary>
        /// Returns true if the supplied point is inside a hull with
        /// planes whose normal vectors point to the inside of the hull.
        /// The optional offset parameter is measured in normal direction,
        /// i.e. a positive offset makes the hull smaller.
        /// </summary>
        public static bool IsInsideInwardHull(this __v2t__ point, __type__ hull, __ftype__ offset = 0)
        {
            for (int i = 0; i < hull.PlaneCount; i++)
                if (hull.PlaneArray[i].Height(point) < offset) return false;
            return true;
        }

        /// <summary>
        /// Returns unordered set of corners of this hull.
        /// </summary>
        public static HashSet<__v2t__> ComputeCorners(this __type__ hull)
        {
            var corners = new HashSet<__v2t__>();
            int count = hull.PlaneArray.Length;
            for (var i0 = 0; i0 < count; i0++)
            {
                for (var i1 = i0 + 1; i1 < count; i1++)
                {
                    if (hull.PlaneArray[i0].Intersects(hull.PlaneArray[i1], out __v2t__ temp))
                    {
                        if (temp.IsNaN || temp.AnyInfinity) continue;

                        var inside = true;
                        for (var j = 0; j < count; j++)
                        {
                            if (j == i0 || j == i1) continue;
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

            return corners;
        }
    }

    #endregion

    //# }
}