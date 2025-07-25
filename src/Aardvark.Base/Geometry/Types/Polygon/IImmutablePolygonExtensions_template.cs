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

//# foreach (var isDouble in new[] { false, true }) {
//#   var ftype = isDouble ? "double" : "float";
//#   var ftype2 = isDouble ? "float" : "double";
//#   var tc = isDouble ? "d" : "f";
//#   var tc2 = isDouble ? "f" : "d";
//#   var type = "Circle2" + tc;
//#   var type2 = "Circle2" + tc2;
//#   var v2t = "V2" + tc;
//#   var m33t = "M33" + tc;
//#   var box2t = "Box2" + tc;
//#   var plane2t = "Plane2" + tc;
//#   var line2t = "Line2" + tc;
//#   var polygon2t = "Polygon2" + tc;
//#   var polygon3t = "Polygon3" + tc;
//#   var half = isDouble ? "0.5" : "0.5f";
#region IImmutable__polygon2t__Extensions

/// <summary>
/// Extensions for IImmutablePolygon(of T).
/// </summary>
public static class IImmutable__polygon2t__Extensions
{
    /// <summary>
    /// Converts this IImmutablePolygon(of __v2t__) to a __polygon2t__.
    /// </summary>
    public static __polygon2t__ To__polygon2t__(this IImmutablePolygon<__v2t__> self) => new(self.Points);

    /// <summary>
    /// Returns the index and distance of the polygon's closest point to the given query point.
    /// </summary>
    public static Tuple<int, double> QueryNearestVertex(this IImmutablePolygon<__v2t__> self, __v2t__ queryPoint)
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
    public static IImmutablePolygon<__v2t__> MovePoint(this IImmutablePolygon<__v2t__> self, int index, __v2t__ delta)
        => self.SetPoint(index, self.Points[index] + delta);

    /// <summary>
    /// Returns new polygon with point transformed. 
    /// </summary>
    public static IImmutablePolygon<__v2t__> TransformPoint(this IImmutablePolygon<__v2t__> self, int index, __m33t__ trafo)
        => self.SetPoint(index, trafo.TransformPos(self.Points[index]));

    /// <summary>
    /// Gets the index-th edge of this polygon.
    /// </summary>
    public static __line2t__ GetEdge(this IImmutablePolygon<__v2t__> self, int index)
    {
        index = self.RepairIndex(index);
        var p0 = self.Points[index++];
        var p1 = self.Points[index < self.Count ? index : 0];
        return new __line2t__(p0, p1);
    }

    /// <summary>
    /// Sets the index-th edge of this polygon.
    /// </summary>
    public static IImmutablePolygon<__v2t__> SetEdge(this IImmutablePolygon<__v2t__> self, int index, __line2t__ edge)
    {
        index = self.RepairIndex(index);
        var i0 = index++;
        var i1 = index < self.Count ? index : 0;
        return self.SetPoint(i0, edge.P0).SetPoint(i1, edge.P1);
    }

    /// <summary>
    /// Maps arbitrary index into valid range.
    /// </summary>
    public static int RepairIndex(this __polygon2t__ self, int index)
        => IImmutablePolygonExtensions.RepairIndex(self.PointCount, index);

    /// <summary>
    /// Maps arbitrary index into valid range.
    /// </summary>
    public static int RepairIndex(this __polygon3t__ self, int index)
        => IImmutablePolygonExtensions.RepairIndex(self.PointCount, index);

    /// <summary>
    /// Makes index-th edge parallel to x- or y-axis.
    /// </summary>
    public static IImmutablePolygon<__v2t__> AlignEdge(this IImmutablePolygon<__v2t__> self, int index)
    {
        var e = self.GetEdge(index);

        if ((Math.Abs(e.P0.X - e.P1.X) < Math.Abs(e.P0.Y - e.P1.Y)))
        {
            var x = (e.P0.X + e.P1.X) * 0.5;
            return self.SetEdge(index, new __line2t__(new __v2t__(x, e.P0.Y), new __v2t__(x, e.P1.Y)));
        }
        else
        {
            var y = (e.P0.Y + e.P1.Y) * 0.5;
            return self.SetEdge(index, new __line2t__(new __v2t__(e.P0.X, y), new __v2t__(e.P1.X, y)));
        }
    }

    /// <summary>
    /// Ensures that the outline is oriented counter-clockwise.
    /// </summary>
    public static IImmutablePolygon<__v2t__> ToCounterClockwise(this IImmutablePolygon<__v2t__> self)
        => self.To__polygon2t__().IsCcw() ? self : new ImmutablePolygon<__v2t__>(self.Points.Reverse());

    /// <summary>
    /// Ensures that the outline is oriented clockwise.
    /// </summary>
    public static IImmutablePolygon<__v2t__> ToClockwise(this IImmutablePolygon<__v2t__> self)
        => self.To__polygon2t__().IsCcw() ? new ImmutablePolygon<__v2t__>(self.Points.Reverse()) : self;
}

#endregion

//# }
