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
using System.Xml.Serialization;

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

#region Line2f

/// <summary>
/// A two dimensional line with specified start and end points.
/// </summary>
public partial struct Line2f : IBoundingCircle2f
{
    #region Geometric Properties

    public readonly V2f Center => (P0 + P1) * 0.5f;

    /// <summary>
    /// P0
    /// </summary>
    [XmlIgnore]
    public V2f Origin
    {
        readonly get { return P0; }
        set { P0 = value; }
    }

    /// <summary>
    /// P1 - P0
    /// </summary>
    [XmlIgnore]
    public V2f Direction
    {
        readonly get { return P1 - P0; }
        set { P1 = P0 + value; }
    }

    public readonly Ray2f Ray2f => new(P0, P1 - P0);

    public readonly Plane2f Plane2f => Ray2f.Plane2f;

    public readonly float LeftValueOfDir(V2f v) => v.X * (P0.Y - P1.Y) + v.Y * (P1.X - P0.X);

    public readonly float RightValueOfDir(V2f v) => v.X * (P1.Y - P0.Y) + v.Y * (P0.X - P1.X);

    public readonly float LeftValueOfPos(V2f p)
        => (p.X - P0.X) * (P0.Y - P1.Y) + (p.Y - P0.Y) * (P1.X - P0.X);

    public readonly float RightValueOfPos(V2f p)
        => (p.X - P0.X) * (P1.Y - P0.Y) + (p.Y - P0.Y) * (P0.X - P1.X);

    public readonly bool IsDegenerated => Direction.AllTiny;

    #endregion

    #region IBoundingCircle2f Members

    public readonly Circle2f BoundingCircle2f => new(Center, 0.5f * Direction.Length);

    #endregion

    #region Geometric Computations

    public readonly V2f GetClosestPointOnLine(V2f p)
    {
        var d = P0 - P1;
        var l = d.LengthSquared;
        if(Fun.IsTiny(l))
            return P0; //it does not matter which of the two points we choose
        var t = (P0.Dot(d) - p.Dot(d)) / l; //parametric distance from P0 to P1, where closest point to p is
        if (t <= 0)
            return P0;
        if (t >= 1)
            return P1;
        return P0 - t * d;
    }

    public readonly float GetDistanceToLine(V2f p)
    {
        var f = GetClosestPointOnLine(p);
        return (f - p).Length;
    }

    public readonly bool IsDistanceToPointSmallerThan(V2f p, float maxDist)
    {
        //speed-up by first checking the bounding box
        var box = BoundingBox2f;
        box.EnlargeBy(maxDist);
        if (!box.Contains(p))
            return false;
        return GetDistanceToLine(p) <= maxDist;
    }

    public readonly Line2f Flipped => new(P1, P0);

    #endregion

    /// <summary>
    /// Returns true if points a, b and c are exactly collinear.
    /// </summary>
    public static bool IsCollinear(V2f a, V2f b, V2f c)
        => (b.Y - a.Y) * (c.X - b.X) == (c.Y - b.Y) * (b.X - a.X);

    /// <summary>
    /// Returns true if points a, b and c are collinear within eps.
    /// </summary>
    public static bool IsCollinear(V2f a, V2f b, V2f c, float eps = 1e-5f)
        => Fun.ApproximateEquals((b.Y - a.Y) * (c.X - b.X), (c.Y - b.Y) * (b.X - a.X), eps);
}

#endregion

#region Line2d

/// <summary>
/// A two dimensional line with specified start and end points.
/// </summary>
public partial struct Line2d : IBoundingCircle2d
{
    #region Geometric Properties

    public readonly V2d Center => (P0 + P1) * 0.5;

    /// <summary>
    /// P0
    /// </summary>
    [XmlIgnore]
    public V2d Origin
    {
        readonly get { return P0; }
        set { P0 = value; }
    }

    /// <summary>
    /// P1 - P0
    /// </summary>
    [XmlIgnore]
    public V2d Direction
    {
        readonly get { return P1 - P0; }
        set { P1 = P0 + value; }
    }

    public readonly Ray2d Ray2d => new(P0, P1 - P0);

    public readonly Plane2d Plane2d => Ray2d.Plane2d;

    public readonly double LeftValueOfDir(V2d v) => v.X * (P0.Y - P1.Y) + v.Y * (P1.X - P0.X);

    public readonly double RightValueOfDir(V2d v) => v.X * (P1.Y - P0.Y) + v.Y * (P0.X - P1.X);

    public readonly double LeftValueOfPos(V2d p)
        => (p.X - P0.X) * (P0.Y - P1.Y) + (p.Y - P0.Y) * (P1.X - P0.X);

    public readonly double RightValueOfPos(V2d p)
        => (p.X - P0.X) * (P1.Y - P0.Y) + (p.Y - P0.Y) * (P0.X - P1.X);

    public readonly bool IsDegenerated => Direction.AllTiny;

    #endregion

    #region IBoundingCircle2d Members

    public readonly Circle2d BoundingCircle2d => new(Center, 0.5 * Direction.Length);

    #endregion

    #region Geometric Computations

    public readonly V2d GetClosestPointOnLine(V2d p)
    {
        var d = P0 - P1;
        var l = d.LengthSquared;
        if(Fun.IsTiny(l))
            return P0; //it does not matter which of the two points we choose
        var t = (P0.Dot(d) - p.Dot(d)) / l; //parametric distance from P0 to P1, where closest point to p is
        if (t <= 0)
            return P0;
        if (t >= 1)
            return P1;
        return P0 - t * d;
    }

    public readonly double GetDistanceToLine(V2d p)
    {
        var f = GetClosestPointOnLine(p);
        return (f - p).Length;
    }

    public readonly bool IsDistanceToPointSmallerThan(V2d p, double maxDist)
    {
        //speed-up by first checking the bounding box
        var box = BoundingBox2d;
        box.EnlargeBy(maxDist);
        if (!box.Contains(p))
            return false;
        return GetDistanceToLine(p) <= maxDist;
    }

    public readonly Line2d Flipped => new(P1, P0);

    #endregion

    /// <summary>
    /// Returns true if points a, b and c are exactly collinear.
    /// </summary>
    public static bool IsCollinear(V2d a, V2d b, V2d c)
        => (b.Y - a.Y) * (c.X - b.X) == (c.Y - b.Y) * (b.X - a.X);

    /// <summary>
    /// Returns true if points a, b and c are collinear within eps.
    /// </summary>
    public static bool IsCollinear(V2d a, V2d b, V2d c, double eps = 1e-9)
        => Fun.ApproximateEquals((b.Y - a.Y) * (c.X - b.X), (c.Y - b.Y) * (b.X - a.X), eps);
}

#endregion

