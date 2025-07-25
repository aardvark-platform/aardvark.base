/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Linq;

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

public static class IImmutablePolygonExtensions
{
    /// <summary>
    /// Maps index into range [0, count).
    /// </summary>
    internal static int RepairIndex(int count, int index)
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
    /// Maps arbitrary index into valid range.
    /// </summary>
    public static int RepairIndex<T>(this IImmutablePolygon<T> self, int index)
        => RepairIndex(self.Count, index);
}

#region IImmutablePolygon2fExtensions

/// <summary>
/// Extensions for IImmutablePolygon(of T).
/// </summary>
public static class IImmutablePolygon2fExtensions
{
    /// <summary>
    /// Converts this IImmutablePolygon(of V2f) to a Polygon2f.
    /// </summary>
    public static Polygon2f ToPolygon2f(this IImmutablePolygon<V2f> self) => new(self.Points);

    /// <summary>
    /// Returns the index and distance of the polygon's closest point to the given query point.
    /// </summary>
    public static Tuple<int, double> QueryNearestVertex(this IImmutablePolygon<V2f> self, V2f queryPoint)
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
    public static IImmutablePolygon<V2f> MovePoint(this IImmutablePolygon<V2f> self, int index, V2f delta)
        => self.SetPoint(index, self.Points[index] + delta);

    /// <summary>
    /// Returns new polygon with point transformed. 
    /// </summary>
    public static IImmutablePolygon<V2f> TransformPoint(this IImmutablePolygon<V2f> self, int index, M33f trafo)
        => self.SetPoint(index, trafo.TransformPos(self.Points[index]));

    /// <summary>
    /// Gets the index-th edge of this polygon.
    /// </summary>
    public static Line2f GetEdge(this IImmutablePolygon<V2f> self, int index)
    {
        index = self.RepairIndex(index);
        var p0 = self.Points[index++];
        var p1 = self.Points[index < self.Count ? index : 0];
        return new Line2f(p0, p1);
    }

    /// <summary>
    /// Sets the index-th edge of this polygon.
    /// </summary>
    public static IImmutablePolygon<V2f> SetEdge(this IImmutablePolygon<V2f> self, int index, Line2f edge)
    {
        index = self.RepairIndex(index);
        var i0 = index++;
        var i1 = index < self.Count ? index : 0;
        return self.SetPoint(i0, edge.P0).SetPoint(i1, edge.P1);
    }

    /// <summary>
    /// Maps arbitrary index into valid range.
    /// </summary>
    public static int RepairIndex(this Polygon2f self, int index)
        => IImmutablePolygonExtensions.RepairIndex(self.PointCount, index);

    /// <summary>
    /// Maps arbitrary index into valid range.
    /// </summary>
    public static int RepairIndex(this Polygon3f self, int index)
        => IImmutablePolygonExtensions.RepairIndex(self.PointCount, index);

    /// <summary>
    /// Makes index-th edge parallel to x- or y-axis.
    /// </summary>
    public static IImmutablePolygon<V2f> AlignEdge(this IImmutablePolygon<V2f> self, int index)
    {
        var e = self.GetEdge(index);

        if ((Math.Abs(e.P0.X - e.P1.X) < Math.Abs(e.P0.Y - e.P1.Y)))
        {
            var x = (e.P0.X + e.P1.X) * 0.5;
            return self.SetEdge(index, new Line2f(new V2f(x, e.P0.Y), new V2f(x, e.P1.Y)));
        }
        else
        {
            var y = (e.P0.Y + e.P1.Y) * 0.5;
            return self.SetEdge(index, new Line2f(new V2f(e.P0.X, y), new V2f(e.P1.X, y)));
        }
    }

    /// <summary>
    /// Ensures that the outline is oriented counter-clockwise.
    /// </summary>
    public static IImmutablePolygon<V2f> ToCounterClockwise(this IImmutablePolygon<V2f> self)
        => self.ToPolygon2f().IsCcw() ? self : new ImmutablePolygon<V2f>(self.Points.Reverse());

    /// <summary>
    /// Ensures that the outline is oriented clockwise.
    /// </summary>
    public static IImmutablePolygon<V2f> ToClockwise(this IImmutablePolygon<V2f> self)
        => self.ToPolygon2f().IsCcw() ? new ImmutablePolygon<V2f>(self.Points.Reverse()) : self;
}

#endregion

#region IImmutablePolygon2dExtensions

/// <summary>
/// Extensions for IImmutablePolygon(of T).
/// </summary>
public static class IImmutablePolygon2dExtensions
{
    /// <summary>
    /// Converts this IImmutablePolygon(of V2d) to a Polygon2d.
    /// </summary>
    public static Polygon2d ToPolygon2d(this IImmutablePolygon<V2d> self) => new(self.Points);

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
        => self.SetPoint(index, self.Points[index] + delta);

    /// <summary>
    /// Returns new polygon with point transformed. 
    /// </summary>
    public static IImmutablePolygon<V2d> TransformPoint(this IImmutablePolygon<V2d> self, int index, M33d trafo)
        => self.SetPoint(index, trafo.TransformPos(self.Points[index]));

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
    public static int RepairIndex(this Polygon2d self, int index)
        => IImmutablePolygonExtensions.RepairIndex(self.PointCount, index);

    /// <summary>
    /// Maps arbitrary index into valid range.
    /// </summary>
    public static int RepairIndex(this Polygon3d self, int index)
        => IImmutablePolygonExtensions.RepairIndex(self.PointCount, index);

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
        => self.ToPolygon2d().IsCcw() ? self : new ImmutablePolygon<V2d>(self.Points.Reverse());

    /// <summary>
    /// Ensures that the outline is oriented clockwise.
    /// </summary>
    public static IImmutablePolygon<V2d> ToClockwise(this IImmutablePolygon<V2d> self)
        => self.ToPolygon2d().IsCcw() ? new ImmutablePolygon<V2d>(self.Points.Reverse()) : self;
}

#endregion

