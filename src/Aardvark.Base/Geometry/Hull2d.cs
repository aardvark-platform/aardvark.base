using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    /// <summary>
    /// A hull is an alternative representation of a convex polygon.
    /// </summary>
    public struct Hull2d : IValidity
    {
        public Plane2d[] PlaneArray;

        #region Constructors

        /// <summary>
        /// Create an empty Hull3d with count planes.
        /// </summary>
        public Hull2d(int count)
        {
            PlaneArray = new Plane2d[count];
        }

        public Hull2d(Plane2d[] planeArray)
        {
            PlaneArray = planeArray;
        }

        public Hull2d(Box2d box)
        {
            PlaneArray = new[]
            {
                new Plane2d(V2d.XAxis, box.Min),
                new Plane2d(V2d.YAxis, box.Min),
                new Plane2d(-V2d.XAxis, box.Max),
                new Plane2d(-V2d.YAxis, box.Max),
            };
        }

        public Hull2d(Hull2d hull)
        {
            PlaneArray = hull.PlaneArray.Copy();
        }

        #endregion

        #region Constants

        public static readonly Hull2d Invalid = new Hull2d(null);

        #endregion

        #region Properties

        public bool IsValid { get { return PlaneArray != null; } }
        public bool IsInvalid { get { return PlaneArray == null; } }

        public int PlaneCount { get { return PlaneArray.Length; } }

        #endregion

        #region Transformation

        public Hull2d Transformed(Trafo2d trafo)
        {
            int count = PlaneCount;
            var hull = new Hull2d(new Plane2d[count]);
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane2d(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
            return hull;
        }

        public void TransformInto(Trafo2d trafo, ref Hull2d hull)
        {
            int count = PlaneCount;
            var invTr = trafo.Backward.Transposed;
            for (int i = 0; i < count; i++)
                hull.PlaneArray[i]
                    = new Plane2d(
                            invTr.TransformDir(PlaneArray[i].Normal).Normalized,
                            trafo.Forward.TransformPos(PlaneArray[i].Point));
        }

        public Hull2d Reversed()
        {
            return new Hull2d(PlaneArray.Map(p => p.Reversed));
        }

        public void Reverse()
        {
            PlaneArray.Apply(p => p.Reversed);
        }

        #endregion
    }

    public static class Hull2dExtensions
    {
        /// <summary>
        /// Creates an inward hull (i.e. a hull whose normal vectors point
        /// inside) from a counter-clockwise enumerated array of polygon
        /// points.
        /// </summary>
        public static Hull2d ToInwardHull(this V2d[] polygon, int pointCount = 0)
        {
            if (pointCount == 0) pointCount = polygon.Length;
            var planeArray = new Plane2d[pointCount];
            var p0 = polygon[pointCount - 1];
            for (int i = 0; i < pointCount; i++)
            {
                var p1 = polygon[i];
                planeArray[i] = new Plane2d((p1 - p0).Rot90.Normalized, p0);
                p0 = p1;
            }
            return new Hull2d(planeArray);
        }

        /// <summary>
        /// Returns true if the supplied point is inside a hull with
        /// planes whose normal vectors point to the inside of the hull.
        /// The optional offset parameter is measured in normal direction,
        /// i.e. a positive offset makes the hull smaller.
        /// </summary>
        public static bool IsInsideInwardHull(this V2d point, Hull2d hull, double offset = 0.0)
        {
            for (int i = 0; i < hull.PlaneCount; i++)
                if (hull.PlaneArray[i].Height(point) < offset) return false;
            return true;
        }

        /// <summary>
        /// Returns unordered set of corners of this hull.
        /// </summary>
        public static HashSet<V2d> ComputeCorners(this Hull2d hull)
        {
            var corners = new HashSet<V2d>();
            int count = hull.PlaneArray.Length;
            for (var i0 = 0; i0 < count; i0++)
            {
                for (var i1 = i0 + 1; i1 < count; i1++)
                {
                    if (hull.PlaneArray[i0].Intersects(hull.PlaneArray[i1], out V2d temp))
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
}