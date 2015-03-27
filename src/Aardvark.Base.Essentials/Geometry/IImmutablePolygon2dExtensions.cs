using System;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// Extensions for IImmutablePolygon(of T).
    /// </summary>
    public static class IImmutablePolygonExtensions
    {
        /// <summary>
        /// Converts this IImmutablePolygon(of V2d) to a Polygon2d.
        /// </summary>
        public static Polygon2d ToPolygon2d(this IImmutablePolygon<V2d> self)
        {
            return new Polygon2d(self.Points);
        }

        /// <summary>
        /// Returns the index and distance of the polygon's closest point to the given query point.
        /// </summary>
        public static Tuple<int, double> QueryNearestVertex(this IImmutablePolygon<V2d> self, V2d queryPoint)
        {
            if (self.Count == 0) return null;

            int bestIndex = 0;
            double bestDist = double.MaxValue;
            for (int i = 0; i < self.Count; i++)
            {
                var d = (queryPoint - self.Points[i]).LengthSquared;
                if (d < bestDist) { bestDist = d; bestIndex = i; }
            }

            return Tuple.Create(bestIndex, bestDist.Sqrt());
        }

        /// <summary>
        /// Returns new polygon with point moved. 
        /// </summary>
        public static IImmutablePolygon<V2d> MovePoint(this IImmutablePolygon<V2d> self, int index, V2d delta)
        {
            return self.SetPoint(index, self.Points[index] + delta);
        }

        /// <summary>
        /// Returns new polygon with point transformed. 
        /// </summary>
        public static IImmutablePolygon<V2d> TransformPoint(this IImmutablePolygon<V2d> self, int index, M33d trafo)
        {
            return self.SetPoint(index, trafo.TransformPos(self.Points[index]));
        }

        /// <summary>
        /// Gets the index-th edge of this polygon.
        /// </summary>
        public static Line2d GetEdge(this IImmutablePolygon<V2d> self, int index)
        {
            index = self.RepairIndex(index);
            var p0 = self.Points[index++];
            var p1 = self.Points[index < self.Count ? index : 0];
            return new Line2d(p0, p1);
        }

        /// <summary>
        /// Sets the index-th edge of this polygon.
        /// </summary>
        public static IImmutablePolygon<V2d> SetEdge(this IImmutablePolygon<V2d> self, int index, Line2d edge)
        {
            index = self.RepairIndex(index);
            var i0 = index++;
            var i1 = index < self.Count ? index : 0;
            return self.SetPoint(i0, edge.P0).SetPoint(i1, edge.P1);
        }

        /// <summary>
        /// Maps arbitrary index into valid range.
        /// </summary>
        public static int RepairIndex<T>(this IImmutablePolygon<T> self, int index)
        {
            return RepairIndex(self.Count, index);
        }

        /// <summary>
        /// Maps arbitrary index into valid range.
        /// </summary>
        public static int RepairIndex(this Polygon2d self, int index)
        {
            return RepairIndex(self.PointCount, index);
        }

        /// <summary>
        /// Maps arbitrary index into valid range.
        /// </summary>
        public static int RepairIndex(this Polygon3d self, int index)
        {
            return RepairIndex(self.PointCount, index);
        }

        /// <summary>
        /// Maps index into range [0, count).
        /// </summary>
        private static int RepairIndex(int count, int index)
        {
            if (index >= 0)
            {
                return (index < count) ? index : (index % count);
            }
            else
            {
                if (index >= -count)
                {
                    return count + index;
                }
                else
                {
                    return count - 1 + (index + 1) % count;
                }
            }
        }

        /// <summary>
        /// Makes index-th edge parallel to x- or y-axis.
        /// </summary>
        public static IImmutablePolygon<V2d> AlignEdge(this IImmutablePolygon<V2d> self, int index)
        {
            var e = self.GetEdge(index);

            if ((Math.Abs(e.P0.X - e.P1.X) < Math.Abs(e.P0.Y - e.P1.Y)))
            {
                var x = (e.P0.X + e.P1.X) * 0.5;
                return self.SetEdge(index, new Line2d(new V2d(x, e.P0.Y), new V2d(x, e.P1.Y)));
            }
            else
            {
                var y = (e.P0.Y + e.P1.Y) * 0.5;
                return self.SetEdge(index, new Line2d(new V2d(e.P0.X, y), new V2d(e.P1.X, y)));
            }
        }

        /// <summary>
        /// Ensures that the outline is oriented counter-clockwise.
        /// </summary>
        public static IImmutablePolygon<V2d> ToCounterClockwise(this IImmutablePolygon<V2d> self)
        {
            return self.ToPolygon2d().IsCcw() ? self : new ImmutablePolygon<V2d>(self.Points.Reverse());
        }

        /// <summary>
        /// Ensures that the outline is oriented clockwise.
        /// </summary>
        public static IImmutablePolygon<V2d> ToClockwise(this IImmutablePolygon<V2d> self)
        {
            return self.ToPolygon2d().IsCcw() ? new ImmutablePolygon<V2d>(self.Points.Reverse()) : self;
        }
    }
}
