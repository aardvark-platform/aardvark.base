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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

#pragma warning disable IDE0290 // Use primary constructor

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

#region Polygon2f

/// <summary>
/// A polygon internally represented by an array of points. Implemented
/// as a structure, the validity of the polygon can be checked via its
/// PointCount, which must be bigger than 0 for a polygon to hold any
/// points, and bigger than 2 for a polygon to be geometrically valid.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public partial struct Polygon2f : IEquatable<Polygon2f>, IValidity, IPolygon<V2f>, IBoundingBox2f
{
    internal int m_pointCount;
    internal V2f[] m_pointArray;

    #region Constructors

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon2f(V2f[] pointArray, int pointCount)
    {
        if (pointArray != null)
        {
            if (pointCount <= pointArray.Length)
            {
                m_pointCount = pointCount;
                m_pointArray = pointArray;
            }
            else
                throw new ArgumentException("point count must be smaller or equal array length");
        }
        else
        {
            m_pointCount = 0;
            m_pointArray = null;
        }
    }

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon2f(params V2f[] pointArray)
    {
        m_pointCount = pointArray != null ? pointArray.Length : 0;
        m_pointArray = pointArray;
    }

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon2f(V2f[] pointArray, int startIndex, int count)
    {
        if (startIndex < 0 || startIndex >= pointArray.Length - 1) throw new ArgumentOutOfRangeException(nameof(startIndex));
        if (count <= 0 || startIndex + count >= pointArray.Length) throw new ArgumentOutOfRangeException(nameof(count));
        m_pointCount = count;
        m_pointArray = new V2f[count];
        for (int i = 0; i < count; i++) m_pointArray[i] = pointArray[startIndex + i];
    }

    /// <summary>
    /// Creates a polygon from point count and point creator function.
    /// </summary>
    public Polygon2f(int pointCount, Func<int, V2f> index_pointCreator)
        : this(new V2f[pointCount].SetByIndex(index_pointCreator))
    { }

    /// <summary>
    /// Creates a polygon from a sequence of points.
    /// </summary>
    public Polygon2f(IEnumerable<V2f> points)
        : this([.. points])
    { }

    /// <summary>
    /// Creates a polygon from the points of a pointArray that
    /// are selected by an index array.
    /// </summary>
    public Polygon2f(int[] indexArray, V2f[] pointArray)
        : this(indexArray.Map(i => pointArray[i]))
    { }

    /// <summary>
    /// Creates a polygon from a triangle.
    /// </summary>
    public Polygon2f(Triangle2f triangle)
        : this(triangle.GetPointArray())
    { }

    /// <summary>
    /// Creates a polygon from a quad.
    /// </summary>
    public Polygon2f(Quad2f quad)
        : this(quad.GetPointArray())
    { }

    /// <summary>
    /// Copy constructor.
    /// Performs deep copy of original.
    /// </summary>
    public Polygon2f(Polygon2f original)
        : this(original.GetPointArray())
    { }

    #endregion

    #region Constants

    public static readonly Polygon2f Invalid = new(null, 0);

    #endregion

    #region Properties

    public readonly bool IsValid => m_pointArray != null;

    public readonly bool IsInvalid => m_pointArray == null;

    /// <summary>
    /// The number of points in the polygon. If this is 0, the polygon
    /// is invalid.
    /// </summary>
    public readonly int PointCount => m_pointCount;

    /// <summary>
    /// Enumerates points.
    /// </summary>
    public readonly IEnumerable<V2f> Points
    {
        get { for (int pi = 0; pi < m_pointCount; pi++) yield return m_pointArray[pi]; }
    }

    #endregion

    #region Conversions

    /// <summary>
    /// Returns a copy of the polygons point array.
    /// </summary>
    public readonly V2f[] GetPointArray()
    {
        var pc = m_pointCount;
        var pa = new V2f[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
        return pa;
    }

    /// <summary>
    /// [P0, P1, P2] -> [P0, P1, P2, P0].
    /// </summary>
    public readonly V2f[] GetPointArrayWithRepeatedFirstPoint()
    {
        var pc = m_pointCount;
        var pa = new V2f[pc + 1];
        for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
        pa[pc] = pa[0];
        return pa;
    }

    /// <summary>
    /// Returns a transformed copy of the polygons point array.
    /// </summary>
    public readonly T[] GetPointArray<T>(Func<V2f, T> point_copyFun)
    {
        var pc = m_pointCount;
        var pa = new T[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = point_copyFun(m_pointArray[pi]);
        return pa;
    }

    /// <summary>
    /// Returns a transformed copy of the polygons point array.
    /// </summary>
    public readonly T[] GetPointArray<T>(Func<V2f, int, T> point_index_copyFun)
    {
        var pc = m_pointCount;
        var pa = new T[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = point_index_copyFun(m_pointArray[pi], pi);
        return pa;
    }

    #endregion

    #region Indexing

    /// <summary>
    /// Gets the index-th point of this polygon.
    /// </summary>
    public readonly V2f this[int index]
    {
        get { return m_pointArray[index]; }
        set { m_pointArray[index] = value; }
    }

    #endregion

    #region Edges and Lines

    /// <summary>
    /// Index-th edge as vector (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly V2f Edge(int index)
    {
        var p0 = m_pointArray[index++];
        var p1 = m_pointArray[index < m_pointCount ? index : 0];
        return p1 - p0;
    }

    /// <summary>
    /// Edges as vectors (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly IEnumerable<V2f> Edges
    {
        get
        {
            var pc = m_pointCount;
            var p = m_pointArray[0];
            var p0 = p;
            for (int pi = 1; pi < pc; pi++)
            {
                var p1 = m_pointArray[pi];
                yield return p1 - p0;
                p0 = p1;
            }
            yield return p - p0;
        }
    }

    /// <summary>
    /// Index-th edge as line segment (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly Line2f EdgeLine(int index)
    {
        var p0 = m_pointArray[index++];
        var p1 = m_pointArray[index < m_pointCount ? index : 0];
        return new Line2f(p0, p1);
    }

    /// <summary>
    /// Edges as line segments (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly IEnumerable<Line2f> EdgeLines
    {
        get
        {
            var pc = m_pointCount;
            if (pc < 1) yield break;
            var p0 = m_pointArray[0];
            var p = p0;
            for (int pi = 1; pi < pc; pi++)
            {
                var p1 = m_pointArray[pi];
                yield return new Line2f(p0, p1);
                p0 = p1;
            }
            yield return new Line2f(p0, p);
        }
    }

    /// <summary>
    /// Edges as vectors (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly V2f[] GetEdgeArray()
    {
        var pc = m_pointCount;
        if (pc < 2) return [];
        var edgeArray = new V2f[pc];
        var p = m_pointArray[0];
        var p0 = p;
        for (int pi = 1; pi < pc; pi++)
        {
            var p1 = m_pointArray[pi];
            edgeArray[pi - 1] = p1 - p0;
            p0 = p1;
        }
        edgeArray[pc - 1] = p - p0;
        return edgeArray;
    }

    /// <summary>
    /// Edges as line segments (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly Line2f[] GetEdgeLineArray()
    {
        var pc = PointCount;
        if (pc < 2) return [];
        var ela = new Line2f[pc];
        var p = m_pointArray[0];
        var p0 = p;
        for (int pi = 1; pi < pc; pi++)
        {
            var p1 = m_pointArray[pi];
            ela[pi - 1] = new Line2f(p0, p1);
            p0 = p1;
        }
        ela[pc - 1] = new Line2f(p0, p);
        return ela;
    }

    #endregion

    #region Transformations

    /// <summary>
    /// Returns copy of polygon. Same as Map(p => p).
    /// </summary>
    public readonly Polygon2f Copy()
    {
        return new Polygon2f(m_pointArray.Copy());
    }

    /// <summary>
    /// Returns transformed copy of this polygon.
    /// </summary>
    public readonly Polygon2f Map(Func<V2f, V2f> point_fun)
    {
        var pc = m_pointCount;
        V2f[] opa = m_pointArray, npa = new V2f[pc];
        for (int pi = 0; pi < pc; pi++) npa[pi] = point_fun(opa[pi]);
        return new Polygon2f(npa, pc);
    }

    /// <summary>
    /// Gets copy with reversed order of vertices.
    /// </summary>
    public readonly Polygon2f Reversed
    {
        get
        {
            var pc = m_pointCount;
            V2f[] opa = m_pointArray, npa = new V2f[pc];
            for (int pi = 0, pj = pc - 1; pi < pc; pi++, pj--) npa[pi] = opa[pj];
            return new Polygon2f(npa, pc);
        }
    }

    /// <summary>
    /// Reverses order of vertices in-place.
    /// </summary>
    public readonly void Reverse()
    {
        var pa = m_pointArray;
        for (int pi = 0, pj = m_pointCount - 1; pi < pj; pi++, pj--)
        {
            (pa[pj], pa[pi]) = (pa[pi], pa[pj]);
        }
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Polygon2f a, Polygon2f b)
    {
        if (a.m_pointCount != b.m_pointCount) return false;
        for (int pi = 0; pi < a.m_pointCount; pi++)
            if (a.m_pointArray[pi] != b.m_pointArray[pi]) return false;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Polygon2f a, Polygon2f b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return m_pointArray.GetCombinedHashCode(m_pointCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Polygon2f other)
    {
        if (m_pointCount != other.m_pointCount) return false;
        for (int pi = 0; pi < m_pointCount; pi++)
            if (!m_pointArray[pi].Equals(other.m_pointArray[pi])) return false;
        return true;
    }

    public override readonly bool Equals(object other)
        => (other is Polygon2f o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture,
            "[{0}]", Points.Select(x => x.ToString()).Join(", ")
            );
    }

    public static Polygon2f Parse(string s)
    {
        var va = s.NestedBracketSplitLevelOne().ToArray();
        return new Polygon2f(va.Select(x => V2f.Parse(x)));
    }

    #endregion

    #region IBoundingBox2f Members

    /// <summary>
    /// Bounding box of polygon.
    /// </summary>
    public readonly Box2f BoundingBox2f
    {
        get { return new Box2f(m_pointArray, 0, m_pointCount); }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Polygon2f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Polygon2f a, Polygon2f b, float tolerance)
    {
        if (a.m_pointCount != b.m_pointCount) return false;
        for (int pi = 0; pi < a.m_pointCount; pi++)
            if (!ApproximateEquals(a.m_pointArray[pi], b.m_pointArray[pi], tolerance)) return false;
        return true;
    }

    /// <summary>
    /// Returns whether the given <see cref="Polygon2f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Polygon2f a, Polygon2f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region Polygon2fExtensions

public static partial class Polygon2fExtensions
{
    #region Geometric Properties

    /// <summary>
    /// The vertex centroid is the average of the vertex coordinates.
    /// </summary>
    public static V2f ComputeVertexCentroid(this Polygon2f polygon)
    {
        var sum = V2f.Zero;
        int pc = polygon.PointCount;
        for (int i = 0; i < pc; i++) sum += polygon[i];
        float scale = 1 / (float)pc;
        return sum * scale;
    }

    public static float ComputePerimeter(this Polygon2f polygon)
    {
        var pc = polygon.PointCount;
        var p0 = polygon[pc - 1];
        float r = 0;
        for (int i = 0; i < pc; i++)
        {
            var p1 = polygon[i];
            r += Vec.Distance(p0, p1);
            p0 = p1;
        }
        return r;
    }

    #endregion

    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Polygon2f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon2f polygon, float scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Scaled(this Polygon2f polygon, float scale)
    {
        var result = new Polygon2f(polygon);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon2f polygon, V2f center, float scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = center + (polygon.m_pointArray[pi] - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Scaled(this Polygon2f polygon, V2f center, float scale)
    {
        var result = new Polygon2f(polygon);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutVertexCentroid(this ref Polygon2f polygon, float scale)
    {
        var center = polygon.ComputeVertexCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f ScaledAboutVertexCentroid(this Polygon2f polygon, float scale)
    {
        var result = new Polygon2f(polygon);
        result.ScaleAboutVertexCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon2f polygon, V2f scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Scaled(this Polygon2f polygon, V2f scale)
    {
        var result = new Polygon2f(polygon);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon2f polygon, V2f center, V2f scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = center + (polygon.m_pointArray[pi] - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Scaled(this Polygon2f polygon, V2f center, V2f scale)
    {
        var result = new Polygon2f(polygon);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutVertexCentroid(this ref Polygon2f polygon, V2f scale)
    {
        var center = polygon.ComputeVertexCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f ScaledAboutVertexCentroid(this Polygon2f polygon, V2f scale)
    {
        var result = new Polygon2f(polygon);
        result.ScaleAboutVertexCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2f polygon, M33f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Transformed(this Polygon2f polygon, M33f t)
    {
        var result = new Polygon2f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2f polygon, Euclidean2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Transformed(this Polygon2f polygon, Euclidean2f t)
    {
        var result = new Polygon2f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the inverse of the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2f polygon, Euclidean2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the inverse of the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f InvTransformed(this Polygon2f polygon, Euclidean2f t)
    {
        var result = new Polygon2f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2f polygon, Similarity2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Transformed(this Polygon2f polygon, Similarity2f t)
    {
        var result = new Polygon2f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the inverse of the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2f polygon, Similarity2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the inverse of the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f InvTransformed(this Polygon2f polygon, Similarity2f t)
    {
        var result = new Polygon2f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the given <see cref="Affine2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2f polygon, Affine2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the given <see cref="Affine2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Transformed(this Polygon2f polygon, Affine2f t)
    {
        var result = new Polygon2f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2f polygon, Shift2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Transformed(this Polygon2f polygon, Shift2f t)
    {
        var result = new Polygon2f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the inverse of the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2f polygon, Shift2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the inverse of the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f InvTransformed(this Polygon2f polygon, Shift2f t)
    {
        var result = new Polygon2f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2f polygon, Rot2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Transformed(this Polygon2f polygon, Rot2f t)
    {
        var result = new Polygon2f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the inverse of the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2f polygon, Rot2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the inverse of the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f InvTransformed(this Polygon2f polygon, Rot2f t)
    {
        var result = new Polygon2f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2f polygon, Scale2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Transformed(this Polygon2f polygon, Scale2f t)
    {
        var result = new Polygon2f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the inverse of the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2f polygon, Scale2f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the inverse of the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f InvTransformed(this Polygon2f polygon, Scale2f t)
    {
        var result = new Polygon2f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2f"/> by the given <see cref="M22f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2f polygon, M22f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2f"/> transformed by the given <see cref="M22f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2f Transformed(this Polygon2f polygon, M22f t)
    {
        var result = new Polygon2f(polygon);
        result.Transform(t);
        return result;
    }

    public static Polygon2f WithoutMultiplePoints(this Polygon2f polygon, float eps = 1e-5f)
    {
        eps *= eps;
        var opc = polygon.PointCount;
        var pa = new V2f[opc];
        var pc = 0;
        pa[0] = polygon[0];
        for (int pi = 1; pi < opc; pi++)
            if (Vec.DistanceSquared(pa[pc], polygon[pi]) > eps)
                pa[++pc] = polygon[pi];
        if (Vec.DistanceSquared(pa[pc], polygon[0]) > eps)
            ++pc;
        return new Polygon2f(pa, pc);
    }

    #endregion

    #region Clipping

    /// <summary>
    /// Clip the supplied polygon at the supplied line. The method should
    /// work with all non-selfintersecting polygons. Returns all parts of
    /// the polygon that are at the positive side of the line.
    /// </summary>
    public static Polygon2f ConvexClipped(
            this Polygon2f polygon, Plane2f line, float eps = 1e-5f)
    {
        var opc = polygon.PointCount;
        V2f[] pa = new V2f[opc + 1];
        var pc = 0;
        var pf = polygon[0];
        var hf = line.Height(pf); bool hfp = hf > eps, hfn = hf < -eps;
        if (hf >= -eps) pa[pc++] = pf;
        var p0 = pf; var h0 = hf; var h0p = hfp; var h0n = hfn;
        for (int vi = 1; vi < opc; vi++)
        {
            var p1 = polygon[vi];
            var h1 = line.Height(p1); bool h1p = h1 > eps, h1n = h1 < -eps;
            if (h0p && h1n || h0n && h1p) pa[pc++] = p0 + (p1 - p0) * (h0 / (h0 - h1));
            if (h1 >= -eps) pa[pc++] = p1;
            p0 = p1; h0 = h1; h0p = h1p; h0n = h1n;
        }
        if (h0p && hfn || h0n && hfp) pa[pc++] = p0 + (pf - p0) * (h0 / (h0 - hf));
        return new Polygon2f(pa, pc);
    }

    /// <summary>
    /// Returns the convex polygon clipped by the set of lines (defined
    /// as Plane2fs), i.e. all parts of the polygon that are at the positive
    /// side of the lines.
    /// </summary>
    public static Polygon2f ConvexClipped(
            this Polygon2f polygon, Plane2f[] lines, float eps = 1e-5f)
    {
        foreach (var c in lines)
        {
            polygon = polygon.ConvexClipped(c, eps);
            if (polygon.PointCount == 0) break;
        }
        return polygon;
    }

    /// <summary>
    /// Returns the polygon clipped by the hull, i.e. all parts of the
    /// polygon that are at the positive side of the hull lines.
    /// </summary>
    public static Polygon2f ConvexClipped(
            this Polygon2f polygon, Hull2f hull, float eps = 1e-5f)
    {
        return polygon.ConvexClipped(hull.PlaneArray, eps);
    }

    /// <summary>
    /// TODO summary.
    /// </summary>
    public static Polygon2f ConvexClipped(
#pragma warning disable IDE0060 // Remove unused parameter
            this Polygon2f polygon, Box2f box, float eps = 1e-5f)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        var lines = new[]
        {
            new Plane2f(V2f.XAxis, box.Min), new Plane2f(-V2f.XAxis, box.Max),
            new Plane2f(V2f.YAxis, box.Min), new Plane2f(-V2f.YAxis, box.Max),
        };
        return polygon.ConvexClipped(lines);
    }

    #endregion
}

#endregion

#region IndexPolygon2f

[StructLayout(LayoutKind.Sequential)]
public readonly partial struct IndexPolygon2f : IValidity, IPolygon<V2f>
{
    private readonly int m_pointCount;
    private readonly int m_firstIndex;
    private readonly int[] m_indexArray;
    private readonly V2f[] m_pointArray;

    #region Constructors

    public IndexPolygon2f(int[] indexArray, int firstIndex, int pointCount, V2f[] pointArray)
    {
        m_indexArray = indexArray;
        m_firstIndex = firstIndex;
        m_pointCount = pointCount;
        m_pointArray = pointArray;
    }

    public IndexPolygon2f(V2f[] pointArray, int firstIndex, int pointCount)
        : this(new int[pointCount].SetByIndex(i => firstIndex + i), 0, pointCount, pointArray)
    { }

    public IndexPolygon2f(V2f[] pointArray)
        : this(new int[pointArray.Length].SetByIndex(i => i), 0, pointArray.Length, pointArray)
    { }

    #endregion

    #region Constants

    public static readonly IndexPolygon2f Invalid = new(null, 0, 0, null);

    #endregion

    #region Properties

    public bool IsValid { get { return m_indexArray != null && m_pointArray != null; } }
    public bool IsInvalid { get { return m_indexArray == null || m_pointArray == null; } }

    public int PointCount
    {
        get { return m_pointCount; }
    }

    public int FirstIndex { get { return m_firstIndex; } }

    /// <summary>
    /// The index array that contains the point indices of the
    /// index polygon at the index range [FirstIndex, FirstIndex + PointCount).
    /// NOTE: This is different from the array returned by GetIndexArray().
    /// </summary>
    public int[] IndexArray { get { return m_indexArray; } }

    /// <summary>
    /// The point array that contains the points referenced by
    /// the index array. Note: This is different from the array
    /// returned by GetPointArray().
    /// </summary>
    public V2f[] PointArray { get { return m_pointArray; } }

    public IEnumerable<V2f> Points
    {
        get
        {
            for (int i = 0; i < m_pointCount; i++)
                yield return m_pointArray[m_indexArray[m_firstIndex + i]];
        }
    }

    public IEnumerable<int> Indices
    {
        get
        {
            for (int i = 0; i < m_pointCount; i++)
                yield return m_indexArray[m_firstIndex + i];
        }
    }

    #endregion

    #region Indexing

    public V2f this[int index]
    {
        get { return m_pointArray[m_indexArray[m_firstIndex + index]]; }
        set { m_pointArray[m_indexArray[m_firstIndex + index]] = value; }
    }

    #endregion

    #region Conversions

    public void ForEachIndex(Action<int> index_act)
    {
        var ia = m_indexArray; int fi = m_firstIndex;
        for (int i = 0; i < m_pointCount; i++) index_act(ia[fi + i]);
    }

    /// <summary>
    /// Returns a newly created array containing only the actual indices
    /// of the index polygon. NOTE: This is different from the
    /// IndexArray property!
    /// </summary>
    public int[] GetIndexArray()
    {
        return m_indexArray.Copy(m_firstIndex, m_pointCount);
    }

    /// <summary>
    /// Returns a newly created array containing only the actual points
    /// of the index polygon. NOTE: This is different from the
    /// PointArray property!
    /// </summary>
    public V2f[] GetPointArray()
    {
        var pa = m_pointArray;
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pa[i]);
    }

    public T[] GetPointArray<T>(T[] pointArray)
    {
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pointArray[i]);
    }

    public T[] GetPointArray<T>(List<T> pointList)
    {
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pointList[i]);
    }

    #endregion
}

#endregion

#region IndexPolygon2fExtensions

public static partial class IndexPolygon2fExtensions
{
    #region Conversions

    public static Polygon2f ToPolygon2f(this IndexPolygon2f polygon)
    {
        return new Polygon2f(polygon.GetPointArray());
    }

    #endregion

    #region Geometric Properties

    /// <summary>
    /// The vertex centroid is the average of the vertex coordinates.
    /// </summary>
    public static V2f ComputeVertexCentroid(this IndexPolygon2f polygon)
    {
        var sum = V2f.Zero;
        int pc = polygon.PointCount;
        for (int i = 0; i < pc; i++) sum += polygon[i];
        float scale = 1 / (float)pc;
        return sum * scale;
    }

    public static float ComputePerimeter(this IndexPolygon2f polygon)
    {
        var pc = polygon.PointCount;
        var p0 = polygon[pc - 1];
        float r = 0;
        for (int i = 0; i < pc; i++)
        {
            var p1 = polygon[i];
            r += Vec.Distance(p0, p1);
            p0 = p1;
        }
        return r;
    }

    #endregion
}

#endregion

#region Line2f

[StructLayout(LayoutKind.Sequential)]
public partial struct Line2f : IEquatable<Line2f>, IValidity, IPolygon<V2f>, IBoundingBox2f
{
    public V2f P0, P1;

    #region Constructors

    /// <summary>
    /// Creates line from 2 points.
    /// </summary>
    public Line2f(V2f p0, V2f p1)
    {
        P0 = p0; P1 = p1;
    }

    /// <summary>
    /// Creates line from first 2 points in the sequence.
    /// </summary>
    public Line2f(IEnumerable<V2f> points)
    {
        var pa = points.TakeToArray(2);
        P0 = pa[0]; P1 = pa[1];
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    public readonly int PointCount { get { return 2; } }

    public readonly IEnumerable<V2f> Points
    {
        get { yield return P0; yield return P1; }
    }

    public readonly Line2f Reversed
    {
        get { return new Line2f(P1, P0); }
    }

    #endregion

    #region Indexer

    public V2f this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Line2f Copy(Func<V2f, V2f> point_copyFun)
    {
        return new Line2f(point_copyFun(P0), point_copyFun(P1));
    }

    public readonly Line3d ToLine3d(Func<V2f, V3d> point_copyFun)
    {
        return new Line3d(point_copyFun(P0), point_copyFun(P1));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Line2f a, Line2f b)
        => (a.P0 == b.P0) && (a.P1 == b.P1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Line2f a, Line2f b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Line2f other)
        => P0.Equals(other.P0) && P1.Equals(other.P1);

    public override readonly bool Equals(object other)
         => (other is Line2f o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", P0, P1);
    }

    public static Line2f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Line2f(V2f.Parse(x[0]), V2f.Parse(x[1]));
    }

    #endregion

    #region IBoundingBox2f Members

    public readonly Box2f BoundingBox2f
    {
        get
        {
            return new Box2f(P0, P1).Repair();
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Line2f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Line2f a, Line2f b, float tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Line2f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Line2f a, Line2f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region Line2fExtensions

public static partial class Line2fExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Line2f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line2f polygon, float scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Scaled(this Line2f polygon, float scale)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line2f polygon, V2f center, float scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Scaled(this Line2f polygon, V2f center, float scale)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Line2f polygon, float scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f ScaledAboutCentroid(this Line2f polygon, float scale)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line2f polygon, V2f scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Scaled(this Line2f polygon, V2f scale)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line2f polygon, V2f center, V2f scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Scaled(this Line2f polygon, V2f center, V2f scale)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Line2f polygon, V2f scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f ScaledAboutCentroid(this Line2f polygon, V2f scale)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2f polygon, M33f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Transformed(this Line2f polygon, M33f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2f polygon, Euclidean2f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Transformed(this Line2f polygon, Euclidean2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the inverse of the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2f polygon, Euclidean2f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the inverse of the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f InvTransformed(this Line2f polygon, Euclidean2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2f polygon, Similarity2f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Transformed(this Line2f polygon, Similarity2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the inverse of the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2f polygon, Similarity2f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the inverse of the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f InvTransformed(this Line2f polygon, Similarity2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the given <see cref="Affine2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2f polygon, Affine2f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the given <see cref="Affine2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Transformed(this Line2f polygon, Affine2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2f polygon, Shift2f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Transformed(this Line2f polygon, Shift2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the inverse of the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2f polygon, Shift2f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the inverse of the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f InvTransformed(this Line2f polygon, Shift2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2f polygon, Rot2f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Transformed(this Line2f polygon, Rot2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the inverse of the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2f polygon, Rot2f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the inverse of the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f InvTransformed(this Line2f polygon, Rot2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2f polygon, Scale2f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Transformed(this Line2f polygon, Scale2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the inverse of the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2f polygon, Scale2f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the inverse of the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f InvTransformed(this Line2f polygon, Scale2f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2f"/> by the given <see cref="M22f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2f polygon, M22f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2f"/> transformed by the given <see cref="M22f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2f Transformed(this Line2f polygon, M22f t)
    {
        var result = new Line2f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V2f[] GetPointArray(this Line2f line)
    {
        var pa = new V2f[2];
        pa[0] = line.P0;
        pa[1] = line.P1;
        return pa;
    }

    public static V2f ComputeCentroid(this Line2f line)
    {
        return 0.5f * (line.P0 + line.P1);
    }
}

#endregion

#region Triangle2f

[StructLayout(LayoutKind.Sequential)]
public partial struct Triangle2f : IEquatable<Triangle2f>, IValidity, IPolygon<V2f>, IBoundingBox2f
{
    public V2f P0, P1, P2;

    #region Constructors

    /// <summary>
    /// Creates triangle from 3 points.
    /// </summary>
    public Triangle2f(V2f p0, V2f p1, V2f p2)
    {
        P0 = p0; P1 = p1; P2 = p2;
    }

    /// <summary>
    /// Creates triangle from first 3 points in the sequence.
    /// </summary>
    public Triangle2f(IEnumerable<V2f> points)
    {
        var pa = points.TakeToArray(3);
        P0 = pa[0]; P1 = pa[1]; P2 = pa[2];
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    /// <summary>
    /// Edge P1 - P0
    /// </summary>
    public readonly V2f Edge01 { get { return P1 - P0; } }
    /// <summary>
    /// Edge P2 - P0
    /// </summary>
    public readonly V2f Edge02 { get { return P2 - P0; } }
    /// <summary>
    /// Edge P2 - P1
    /// </summary>
    public readonly V2f Edge12 { get { return P2 - P1; } }
    /// <summary>
    /// Edge P0 - P1
    /// </summary>
    public readonly V2f Edge10 { get { return P0 - P1; } }
    /// <summary>
    /// Edge P0 - P2
    /// </summary>
    public readonly V2f Edge20 { get { return P0 - P2; } }
    /// <summary>
    /// Edge P1 - P2
    /// </summary>
    public readonly V2f Edge21 { get { return P1 - P2; } }

    public readonly IEnumerable<V2f> Edges
    {
        get
        {
            yield return P1 - P0;
            yield return P2 - P1;
            yield return P0 - P2;
        }
    }

    public readonly V2f[] EdgeArray
    {
        get
        {
            var a = new V2f[3];
            a[0] = P1 - P0;
            a[1] = P2 - P1;
            a[2] = P0 - P2;
            return a;
        }
    }

    public readonly IEnumerable<Line2f> EdgeLines
    {
        get
        {
            yield return new Line2f(P0, P1);
            yield return new Line2f(P1, P2);
            yield return new Line2f(P2, P0);
        }
    }

    public readonly Line2f[] EdgeLineArray
    {
        get
        {
            var a = new Line2f[3];
            a[0] = new Line2f(P0, P1);
            a[1] = new Line2f(P1, P2);
            a[2] = new Line2f(P2, P0);
            return a;
        }
    }

    public readonly Line2f GetEdgeLine(int index) => index switch
    {
        0 => new Line2f(P0, P1),
        1 => new Line2f(P1, P2),
        2 => new Line2f(P2, P0),
        _ => throw new InvalidOperationException()
    };

    public readonly V2f GetEdge(int index) => index switch
    {
        0 => P1 - P0,
        1 => P2 - P1,
        2 => P0 - P2,
        _ => throw new InvalidOperationException()
    };

    public readonly Line2f Line01 { get { return new Line2f(P0, P1); } }
    public readonly Line2f Line02 { get { return new Line2f(P0, P2); } }
    public readonly Line2f Line12 { get { return new Line2f(P1, P2); } }
    public readonly Line2f Line10 { get { return new Line2f(P1, P0); } }
    public readonly Line2f Line20 { get { return new Line2f(P2, P0); } }
    public readonly Line2f Line21 { get { return new Line2f(P2, P1); } }

    public readonly int PointCount { get { return 3; } }

    public readonly IEnumerable<V2f> Points
    {
        get { yield return P0; yield return P1; yield return P2; }
    }

    public readonly Triangle2f Reversed
    {
        get { return new Triangle2f(P2, P1, P0); }
    }

    #endregion

    #region Indexer

    public V2f this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                2 => P2,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                case 2: P2 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Triangle2f Copy(Func<V2f, V2f> point_copyFun)
    {
        return new Triangle2f(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
    }

    public readonly Triangle3d ToTriangle3d(Func<V2f, V3d> point_copyFun)
    {
        return new Triangle3d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Triangle2f a, Triangle2f b)
        => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.P2 == b.P2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Triangle2f a, Triangle2f b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1, P2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Triangle2f other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2);

    public override readonly bool Equals(object other)
         => (other is Triangle2f o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, P2);
    }

    public static Triangle2f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Triangle2f(V2f.Parse(x[0]), V2f.Parse(x[1]), V2f.Parse(x[2]));
    }

    #endregion

    #region IBoundingBox2f Members

    public readonly Box2f BoundingBox2f
    {
        get
        {
            return new Box2f(P0, P1, P2);
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Triangle2f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Triangle2f a, Triangle2f b, float tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance) && ApproximateEquals(a.P2, b.P2, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Triangle2f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Triangle2f a, Triangle2f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region Triangle2fExtensions

public static partial class Triangle2fExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Triangle2f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle2f polygon, float scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Scaled(this Triangle2f polygon, float scale)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle2f polygon, V2f center, float scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Scaled(this Triangle2f polygon, V2f center, float scale)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Triangle2f polygon, float scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f ScaledAboutCentroid(this Triangle2f polygon, float scale)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle2f polygon, V2f scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Scaled(this Triangle2f polygon, V2f scale)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle2f polygon, V2f center, V2f scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Scaled(this Triangle2f polygon, V2f center, V2f scale)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Triangle2f polygon, V2f scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f ScaledAboutCentroid(this Triangle2f polygon, V2f scale)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2f polygon, M33f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Transformed(this Triangle2f polygon, M33f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2f polygon, Euclidean2f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Transformed(this Triangle2f polygon, Euclidean2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the inverse of the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2f polygon, Euclidean2f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the inverse of the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f InvTransformed(this Triangle2f polygon, Euclidean2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2f polygon, Similarity2f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Transformed(this Triangle2f polygon, Similarity2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the inverse of the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2f polygon, Similarity2f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the inverse of the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f InvTransformed(this Triangle2f polygon, Similarity2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the given <see cref="Affine2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2f polygon, Affine2f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the given <see cref="Affine2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Transformed(this Triangle2f polygon, Affine2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2f polygon, Shift2f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Transformed(this Triangle2f polygon, Shift2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the inverse of the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2f polygon, Shift2f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the inverse of the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f InvTransformed(this Triangle2f polygon, Shift2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2f polygon, Rot2f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Transformed(this Triangle2f polygon, Rot2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the inverse of the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2f polygon, Rot2f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the inverse of the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f InvTransformed(this Triangle2f polygon, Rot2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2f polygon, Scale2f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Transformed(this Triangle2f polygon, Scale2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the inverse of the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2f polygon, Scale2f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the inverse of the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f InvTransformed(this Triangle2f polygon, Scale2f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2f"/> by the given <see cref="M22f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2f polygon, M22f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2f"/> transformed by the given <see cref="M22f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2f Transformed(this Triangle2f polygon, M22f t)
    {
        var result = new Triangle2f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V2f[] GetPointArray(this Triangle2f triangle)
    {
        var pa = new V2f[3];
        pa[0] = triangle.P0;
        pa[1] = triangle.P1;
        pa[2] = triangle.P2;
        return pa;
    }

    public static V2f ComputeCentroid(this Triangle2f triangle)
    {
        return ConstantF.OneThird * (triangle.P0 + triangle.P1 + triangle.P2);
    }
}

#endregion

#region Quad2f

[StructLayout(LayoutKind.Sequential)]
public partial struct Quad2f : IEquatable<Quad2f>, IValidity, IPolygon<V2f>, IBoundingBox2f
{
    public V2f P0, P1, P2, P3;

    #region Constructors

    /// <summary>
    /// Creates quad from 4 points.
    /// </summary>
    public Quad2f(V2f p0, V2f p1, V2f p2, V2f p3)
    {
        P0 = p0; P1 = p1; P2 = p2; P3 = p3;
    }

    /// <summary>
    /// Creates quad from first 4 points in the sequence.
    /// </summary>
    public Quad2f(IEnumerable<V2f> points)
    {
        var pa = points.TakeToArray(4);
        P0 = pa[0]; P1 = pa[1]; P2 = pa[2]; P3 = pa[3];
    }

    /// <summary>
    /// Creates quad from point and two vectors representing edges.
    /// </summary>
    public Quad2f(V2f p0, V2f edge01, V2f edge03)
    {
        P0 = p0;
        P1 = p0 + edge01;
        P2 = P1 + edge03;
        P3 = p0 + edge03;
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    /// <summary>
    /// Edge P1 - P0
    /// </summary>
    public readonly V2f Edge01 { get { return P1 - P0; } }
    /// <summary>
    /// Edge P3 - P0
    /// </summary>
    public readonly V2f Edge03 { get { return P3 - P0; } }
    /// <summary>
    /// Edge P2 - P1
    /// </summary>
    public readonly V2f Edge12 { get { return P2 - P1; } }
    /// <summary>
    /// Edge P0 - P1
    /// </summary>
    public readonly V2f Edge10 { get { return P0 - P1; } }
    /// <summary>
    /// Edge P3 - P2
    /// </summary>
    public readonly V2f Edge23 { get { return P3 - P2; } }
    /// <summary>
    /// Edge P1 - P2
    /// </summary>
    public readonly V2f Edge21 { get { return P1 - P2; } }
    /// <summary>
    /// Edge P0 - P3
    /// </summary>
    public readonly V2f Edge30 { get { return P0 - P3; } }
    /// <summary>
    /// Edge P2 - P3
    /// </summary>
    public readonly V2f Edge32 { get { return P2 - P3; } }

    public readonly IEnumerable<V2f> Edges
    {
        get
        {
            yield return P1 - P0;
            yield return P2 - P1;
            yield return P3 - P2;
            yield return P0 - P3;
        }
    }

    public readonly V2f[] EdgeArray
    {
        get
        {
            var a = new V2f[4];
            a[0] = P1 - P0;
            a[1] = P2 - P1;
            a[2] = P3 - P2;
            a[3] = P0 - P3;
            return a;
        }
    }

    public readonly IEnumerable<Line2f> EdgeLines
    {
        get
        {
            yield return new Line2f(P0, P1);
            yield return new Line2f(P1, P2);
            yield return new Line2f(P2, P3);
            yield return new Line2f(P3, P0);
        }
    }

    public readonly Line2f[] EdgeLineArray
    {
        get
        {
            var a = new Line2f[4];
            a[0] = new Line2f(P0, P1);
            a[1] = new Line2f(P1, P2);
            a[2] = new Line2f(P2, P3);
            a[3] = new Line2f(P3, P0);
            return a;
        }
    }

    public readonly Line2f GetEdgeLine(int index) => index switch
    {
        0 => new Line2f(P0, P1),
        1 => new Line2f(P1, P2),
        2 => new Line2f(P2, P3),
        3 => new Line2f(P3, P0),
        _ => throw new InvalidOperationException()
    };

    public readonly V2f GetEdge(int index) => index switch
    {
        0 => P1 - P0,
        1 => P2 - P1,
        2 => P3 - P2,
        3 => P0 - P3,
        _ => throw new InvalidOperationException()
    };

    public readonly Line2f Line01 { get { return new Line2f(P0, P1); } }
    public readonly Line2f Line03 { get { return new Line2f(P0, P3); } }
    public readonly Line2f Line12 { get { return new Line2f(P1, P2); } }
    public readonly Line2f Line10 { get { return new Line2f(P1, P0); } }
    public readonly Line2f Line23 { get { return new Line2f(P2, P3); } }
    public readonly Line2f Line21 { get { return new Line2f(P2, P1); } }
    public readonly Line2f Line30 { get { return new Line2f(P3, P0); } }
    public readonly Line2f Line32 { get { return new Line2f(P3, P2); } }

    public readonly int PointCount { get { return 4; } }

    public readonly IEnumerable<V2f> Points
    {
        get { yield return P0; yield return P1; yield return P2; yield return P3; }
    }

    public readonly Quad2f Reversed
    {
        get { return new Quad2f(P3, P2, P1, P0); }
    }

    #endregion

    #region Indexer

    public V2f this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                2 => P2,
                3 => P3,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                case 2: P2 = value; return;
                case 3: P3 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Quad2f Copy(Func<V2f, V2f> point_copyFun)
    {
        return new Quad2f(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
    }

    public readonly Quad3d ToQuad3d(Func<V2f, V3d> point_copyFun)
    {
        return new Quad3d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quad2f a, Quad2f b)
        => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.P2 == b.P2) && (a.P3 == b.P3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quad2f a, Quad2f b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1, P2, P3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Quad2f other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

    public override readonly bool Equals(object other)
         => (other is Quad2f o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", P0, P1, P2, P3);
    }

    public static Quad2f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Quad2f(V2f.Parse(x[0]), V2f.Parse(x[1]), V2f.Parse(x[2]), V2f.Parse(x[3]));
    }

    #endregion

    #region IBoundingBox2f Members

    public readonly Box2f BoundingBox2f
    {
        get
        {
            return new Box2f(P0, P1, P2, P3);
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Quad2f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Quad2f a, Quad2f b, float tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance) && ApproximateEquals(a.P2, b.P2, tolerance) && ApproximateEquals(a.P3, b.P3, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Quad2f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Quad2f a, Quad2f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region Quad2fExtensions

public static partial class Quad2fExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Quad2f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad2f polygon, float scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
        polygon.P3 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Scaled(this Quad2f polygon, float scale)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad2f polygon, V2f center, float scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
        polygon.P3 = center + (polygon.P3 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Scaled(this Quad2f polygon, V2f center, float scale)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Quad2f polygon, float scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f ScaledAboutCentroid(this Quad2f polygon, float scale)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad2f polygon, V2f scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
        polygon.P3 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Scaled(this Quad2f polygon, V2f scale)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad2f polygon, V2f center, V2f scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
        polygon.P3 = center + (polygon.P3 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Scaled(this Quad2f polygon, V2f center, V2f scale)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Quad2f polygon, V2f scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f ScaledAboutCentroid(this Quad2f polygon, V2f scale)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2f polygon, M33f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Transformed(this Quad2f polygon, M33f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2f polygon, Euclidean2f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Transformed(this Quad2f polygon, Euclidean2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the inverse of the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2f polygon, Euclidean2f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
        polygon.P3 = t.InvTransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the inverse of the given <see cref="Euclidean2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f InvTransformed(this Quad2f polygon, Euclidean2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2f polygon, Similarity2f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Transformed(this Quad2f polygon, Similarity2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the inverse of the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2f polygon, Similarity2f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
        polygon.P3 = t.InvTransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the inverse of the given <see cref="Similarity2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f InvTransformed(this Quad2f polygon, Similarity2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the given <see cref="Affine2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2f polygon, Affine2f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the given <see cref="Affine2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Transformed(this Quad2f polygon, Affine2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2f polygon, Shift2f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Transformed(this Quad2f polygon, Shift2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the inverse of the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2f polygon, Shift2f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the inverse of the given <see cref="Shift2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f InvTransformed(this Quad2f polygon, Shift2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2f polygon, Rot2f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Transformed(this Quad2f polygon, Rot2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the inverse of the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2f polygon, Rot2f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the inverse of the given <see cref="Rot2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f InvTransformed(this Quad2f polygon, Rot2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2f polygon, Scale2f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Transformed(this Quad2f polygon, Scale2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the inverse of the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2f polygon, Scale2f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the inverse of the given <see cref="Scale2f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f InvTransformed(this Quad2f polygon, Scale2f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2f"/> by the given <see cref="M22f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2f polygon, M22f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2f"/> transformed by the given <see cref="M22f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2f Transformed(this Quad2f polygon, M22f t)
    {
        var result = new Quad2f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V2f[] GetPointArray(this Quad2f quad)
    {
        var pa = new V2f[4];
        pa[0] = quad.P0;
        pa[1] = quad.P1;
        pa[2] = quad.P2;
        pa[3] = quad.P3;
        return pa;
    }

    public static V2f ComputeCentroid(this Quad2f quad)
    {
        return 0.25f * (quad.P0 + quad.P1 + quad.P2 + quad.P3);
    }
}

#endregion

#region Polygon3f

/// <summary>
/// A polygon internally represented by an array of points. Implemented
/// as a structure, the validity of the polygon can be checked via its
/// PointCount, which must be bigger than 0 for a polygon to hold any
/// points, and bigger than 2 for a polygon to be geometrically valid.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public partial struct Polygon3f : IEquatable<Polygon3f>, IValidity, IPolygon<V3f>, IBoundingBox3f
{
    internal int m_pointCount;
    internal V3f[] m_pointArray;

    #region Constructors

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon3f(V3f[] pointArray, int pointCount)
    {
        if (pointArray != null)
        {
            if (pointCount <= pointArray.Length)
            {
                m_pointCount = pointCount;
                m_pointArray = pointArray;
            }
            else
                throw new ArgumentException("point count must be smaller or equal array length");
        }
        else
        {
            m_pointCount = 0;
            m_pointArray = null;
        }
    }

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon3f(params V3f[] pointArray)
    {
        m_pointCount = pointArray != null ? pointArray.Length : 0;
        m_pointArray = pointArray;
    }

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon3f(V3f[] pointArray, int startIndex, int count)
    {
        if (startIndex < 0 || startIndex >= pointArray.Length - 1) throw new ArgumentOutOfRangeException(nameof(startIndex));
        if (count <= 0 || startIndex + count >= pointArray.Length) throw new ArgumentOutOfRangeException(nameof(count));
        m_pointCount = count;
        m_pointArray = new V3f[count];
        for (int i = 0; i < count; i++) m_pointArray[i] = pointArray[startIndex + i];
    }

    /// <summary>
    /// Creates a polygon from point count and point creator function.
    /// </summary>
    public Polygon3f(int pointCount, Func<int, V3f> index_pointCreator)
        : this(new V3f[pointCount].SetByIndex(index_pointCreator))
    { }

    /// <summary>
    /// Creates a polygon from a sequence of points.
    /// </summary>
    public Polygon3f(IEnumerable<V3f> points)
        : this([.. points])
    { }

    /// <summary>
    /// Creates a polygon from the points of a pointArray that
    /// are selected by an index array.
    /// </summary>
    public Polygon3f(int[] indexArray, V3f[] pointArray)
        : this(indexArray.Map(i => pointArray[i]))
    { }

    /// <summary>
    /// Creates a polygon from a triangle.
    /// </summary>
    public Polygon3f(Triangle3f triangle)
        : this(triangle.GetPointArray())
    { }

    /// <summary>
    /// Creates a polygon from a quad.
    /// </summary>
    public Polygon3f(Quad3f quad)
        : this(quad.GetPointArray())
    { }

    /// <summary>
    /// Copy constructor.
    /// Performs deep copy of original.
    /// </summary>
    public Polygon3f(Polygon3f original)
        : this(original.GetPointArray())
    { }

    #endregion

    #region Constants

    public static readonly Polygon3f Invalid = new(null, 0);

    #endregion

    #region Properties

    public readonly bool IsValid => m_pointArray != null;

    public readonly bool IsInvalid => m_pointArray == null;

    /// <summary>
    /// The number of points in the polygon. If this is 0, the polygon
    /// is invalid.
    /// </summary>
    public readonly int PointCount => m_pointCount;

    /// <summary>
    /// Enumerates points.
    /// </summary>
    public readonly IEnumerable<V3f> Points
    {
        get { for (int pi = 0; pi < m_pointCount; pi++) yield return m_pointArray[pi]; }
    }

    #endregion

    #region Conversions

    /// <summary>
    /// Returns a copy of the polygons point array.
    /// </summary>
    public readonly V3f[] GetPointArray()
    {
        var pc = m_pointCount;
        var pa = new V3f[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
        return pa;
    }

    /// <summary>
    /// [P0, P1, P2] -> [P0, P1, P2, P0].
    /// </summary>
    public readonly V3f[] GetPointArrayWithRepeatedFirstPoint()
    {
        var pc = m_pointCount;
        var pa = new V3f[pc + 1];
        for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
        pa[pc] = pa[0];
        return pa;
    }

    /// <summary>
    /// Returns a transformed copy of the polygons point array.
    /// </summary>
    public readonly T[] GetPointArray<T>(Func<V3f, T> point_copyFun)
    {
        var pc = m_pointCount;
        var pa = new T[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = point_copyFun(m_pointArray[pi]);
        return pa;
    }

    /// <summary>
    /// Returns a transformed copy of the polygons point array.
    /// </summary>
    public readonly T[] GetPointArray<T>(Func<V3f, int, T> point_index_copyFun)
    {
        var pc = m_pointCount;
        var pa = new T[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = point_index_copyFun(m_pointArray[pi], pi);
        return pa;
    }

    #endregion

    #region Indexing

    /// <summary>
    /// Gets the index-th point of this polygon.
    /// </summary>
    public readonly V3f this[int index]
    {
        get { return m_pointArray[index]; }
        set { m_pointArray[index] = value; }
    }

    #endregion

    #region Edges and Lines

    /// <summary>
    /// Index-th edge as vector (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly V3f Edge(int index)
    {
        var p0 = m_pointArray[index++];
        var p1 = m_pointArray[index < m_pointCount ? index : 0];
        return p1 - p0;
    }

    /// <summary>
    /// Edges as vectors (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly IEnumerable<V3f> Edges
    {
        get
        {
            var pc = m_pointCount;
            var p = m_pointArray[0];
            var p0 = p;
            for (int pi = 1; pi < pc; pi++)
            {
                var p1 = m_pointArray[pi];
                yield return p1 - p0;
                p0 = p1;
            }
            yield return p - p0;
        }
    }

    /// <summary>
    /// Index-th edge as line segment (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly Line3f EdgeLine(int index)
    {
        var p0 = m_pointArray[index++];
        var p1 = m_pointArray[index < m_pointCount ? index : 0];
        return new Line3f(p0, p1);
    }

    /// <summary>
    /// Edges as line segments (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly IEnumerable<Line3f> EdgeLines
    {
        get
        {
            var pc = m_pointCount;
            if (pc < 1) yield break;
            var p0 = m_pointArray[0];
            var p = p0;
            for (int pi = 1; pi < pc; pi++)
            {
                var p1 = m_pointArray[pi];
                yield return new Line3f(p0, p1);
                p0 = p1;
            }
            yield return new Line3f(p0, p);
        }
    }

    /// <summary>
    /// Edges as vectors (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly V3f[] GetEdgeArray()
    {
        var pc = m_pointCount;
        if (pc < 2) return [];
        var edgeArray = new V3f[pc];
        var p = m_pointArray[0];
        var p0 = p;
        for (int pi = 1; pi < pc; pi++)
        {
            var p1 = m_pointArray[pi];
            edgeArray[pi - 1] = p1 - p0;
            p0 = p1;
        }
        edgeArray[pc - 1] = p - p0;
        return edgeArray;
    }

    /// <summary>
    /// Edges as line segments (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly Line3f[] GetEdgeLineArray()
    {
        var pc = PointCount;
        if (pc < 2) return [];
        var ela = new Line3f[pc];
        var p = m_pointArray[0];
        var p0 = p;
        for (int pi = 1; pi < pc; pi++)
        {
            var p1 = m_pointArray[pi];
            ela[pi - 1] = new Line3f(p0, p1);
            p0 = p1;
        }
        ela[pc - 1] = new Line3f(p0, p);
        return ela;
    }

    #endregion

    #region Transformations

    /// <summary>
    /// Returns copy of polygon. Same as Map(p => p).
    /// </summary>
    public readonly Polygon3f Copy()
    {
        return new Polygon3f(m_pointArray.Copy());
    }

    /// <summary>
    /// Returns transformed copy of this polygon.
    /// </summary>
    public readonly Polygon3f Map(Func<V3f, V3f> point_fun)
    {
        var pc = m_pointCount;
        V3f[] opa = m_pointArray, npa = new V3f[pc];
        for (int pi = 0; pi < pc; pi++) npa[pi] = point_fun(opa[pi]);
        return new Polygon3f(npa, pc);
    }

    /// <summary>
    /// Gets copy with reversed order of vertices.
    /// </summary>
    public readonly Polygon3f Reversed
    {
        get
        {
            var pc = m_pointCount;
            V3f[] opa = m_pointArray, npa = new V3f[pc];
            for (int pi = 0, pj = pc - 1; pi < pc; pi++, pj--) npa[pi] = opa[pj];
            return new Polygon3f(npa, pc);
        }
    }

    /// <summary>
    /// Reverses order of vertices in-place.
    /// </summary>
    public readonly void Reverse()
    {
        var pa = m_pointArray;
        for (int pi = 0, pj = m_pointCount - 1; pi < pj; pi++, pj--)
        {
            (pa[pj], pa[pi]) = (pa[pi], pa[pj]);
        }
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Polygon3f a, Polygon3f b)
    {
        if (a.m_pointCount != b.m_pointCount) return false;
        for (int pi = 0; pi < a.m_pointCount; pi++)
            if (a.m_pointArray[pi] != b.m_pointArray[pi]) return false;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Polygon3f a, Polygon3f b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return m_pointArray.GetCombinedHashCode(m_pointCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Polygon3f other)
    {
        if (m_pointCount != other.m_pointCount) return false;
        for (int pi = 0; pi < m_pointCount; pi++)
            if (!m_pointArray[pi].Equals(other.m_pointArray[pi])) return false;
        return true;
    }

    public override readonly bool Equals(object other)
        => (other is Polygon3f o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture,
            "[{0}]", Points.Select(x => x.ToString()).Join(", ")
            );
    }

    public static Polygon3f Parse(string s)
    {
        var va = s.NestedBracketSplitLevelOne().ToArray();
        return new Polygon3f(va.Select(x => V3f.Parse(x)));
    }

    #endregion

    #region IBoundingBox3f Members

    /// <summary>
    /// Bounding box of polygon.
    /// </summary>
    public readonly Box3f BoundingBox3f
    {
        get { return new Box3f(m_pointArray, 0, m_pointCount); }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Polygon3f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Polygon3f a, Polygon3f b, float tolerance)
    {
        if (a.m_pointCount != b.m_pointCount) return false;
        for (int pi = 0; pi < a.m_pointCount; pi++)
            if (!ApproximateEquals(a.m_pointArray[pi], b.m_pointArray[pi], tolerance)) return false;
        return true;
    }

    /// <summary>
    /// Returns whether the given <see cref="Polygon3f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Polygon3f a, Polygon3f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region Polygon3fExtensions

public static partial class Polygon3fExtensions
{
    #region Geometric Properties

    /// <summary>
    /// The vertex centroid is the average of the vertex coordinates.
    /// </summary>
    public static V3f ComputeVertexCentroid(this Polygon3f polygon)
    {
        var sum = V3f.Zero;
        int pc = polygon.PointCount;
        for (int i = 0; i < pc; i++) sum += polygon[i];
        float scale = 1 / (float)pc;
        return sum * scale;
    }

    public static float ComputePerimeter(this Polygon3f polygon)
    {
        var pc = polygon.PointCount;
        var p0 = polygon[pc - 1];
        float r = 0;
        for (int i = 0; i < pc; i++)
        {
            var p1 = polygon[i];
            r += Vec.Distance(p0, p1);
            p0 = p1;
        }
        return r;
    }

    #endregion

    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Polygon3f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon3f polygon, float scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Scaled(this Polygon3f polygon, float scale)
    {
        var result = new Polygon3f(polygon);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon3f polygon, V3f center, float scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = center + (polygon.m_pointArray[pi] - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Scaled(this Polygon3f polygon, V3f center, float scale)
    {
        var result = new Polygon3f(polygon);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutVertexCentroid(this ref Polygon3f polygon, float scale)
    {
        var center = polygon.ComputeVertexCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f ScaledAboutVertexCentroid(this Polygon3f polygon, float scale)
    {
        var result = new Polygon3f(polygon);
        result.ScaleAboutVertexCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon3f polygon, V3f scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Scaled(this Polygon3f polygon, V3f scale)
    {
        var result = new Polygon3f(polygon);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon3f polygon, V3f center, V3f scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = center + (polygon.m_pointArray[pi] - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Scaled(this Polygon3f polygon, V3f center, V3f scale)
    {
        var result = new Polygon3f(polygon);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutVertexCentroid(this ref Polygon3f polygon, V3f scale)
    {
        var center = polygon.ComputeVertexCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f ScaledAboutVertexCentroid(this Polygon3f polygon, V3f scale)
    {
        var result = new Polygon3f(polygon);
        result.ScaleAboutVertexCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the given <see cref="M44f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3f polygon, M44f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the given <see cref="M44f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Transformed(this Polygon3f polygon, M44f t)
    {
        var result = new Polygon3f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3f polygon, Euclidean3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Transformed(this Polygon3f polygon, Euclidean3f t)
    {
        var result = new Polygon3f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the inverse of the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3f polygon, Euclidean3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the inverse of the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f InvTransformed(this Polygon3f polygon, Euclidean3f t)
    {
        var result = new Polygon3f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3f polygon, Similarity3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Transformed(this Polygon3f polygon, Similarity3f t)
    {
        var result = new Polygon3f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the inverse of the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3f polygon, Similarity3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the inverse of the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f InvTransformed(this Polygon3f polygon, Similarity3f t)
    {
        var result = new Polygon3f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the given <see cref="Affine3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3f polygon, Affine3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the given <see cref="Affine3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Transformed(this Polygon3f polygon, Affine3f t)
    {
        var result = new Polygon3f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3f polygon, Shift3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Transformed(this Polygon3f polygon, Shift3f t)
    {
        var result = new Polygon3f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the inverse of the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3f polygon, Shift3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the inverse of the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f InvTransformed(this Polygon3f polygon, Shift3f t)
    {
        var result = new Polygon3f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3f polygon, Rot3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Transformed(this Polygon3f polygon, Rot3f t)
    {
        var result = new Polygon3f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the inverse of the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3f polygon, Rot3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the inverse of the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f InvTransformed(this Polygon3f polygon, Rot3f t)
    {
        var result = new Polygon3f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3f polygon, Scale3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Transformed(this Polygon3f polygon, Scale3f t)
    {
        var result = new Polygon3f(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the inverse of the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3f polygon, Scale3f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the inverse of the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f InvTransformed(this Polygon3f polygon, Scale3f t)
    {
        var result = new Polygon3f(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3f"/> by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3f polygon, M33f t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3f"/> transformed by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3f Transformed(this Polygon3f polygon, M33f t)
    {
        var result = new Polygon3f(polygon);
        result.Transform(t);
        return result;
    }

    public static Polygon3f WithoutMultiplePoints(this Polygon3f polygon, float eps = 1e-5f)
    {
        eps *= eps;
        var opc = polygon.PointCount;
        var pa = new V3f[opc];
        var pc = 0;
        pa[0] = polygon[0];
        for (int pi = 1; pi < opc; pi++)
            if (Vec.DistanceSquared(pa[pc], polygon[pi]) > eps)
                pa[++pc] = polygon[pi];
        if (Vec.DistanceSquared(pa[pc], polygon[0]) > eps)
            ++pc;
        return new Polygon3f(pa, pc);
    }

    #endregion

    #region Clipping

    /// <summary>
    /// Clip the supplied polygon at the supplied plane. The method should
    /// work with all non-selfintersecting polygons. Returns all parts of
    /// the polygon that are at the positive side of the plane.
    /// </summary>
    public static Polygon3f ConvexClipped(
            this Polygon3f polygon, Plane3f plane, float eps = 1e-5f)
    {
        var opc = polygon.PointCount;
        V3f[] pa = new V3f[opc + 1];
        var pc = 0;
        var pf = polygon[0];
        var hf = plane.Height(pf); bool hfp = hf > eps, hfn = hf < -eps;
        if (hf >= -eps) pa[pc++] = pf;
        var p0 = pf; var h0 = hf; var h0p = hfp; var h0n = hfn;
        for (int vi = 1; vi < opc; vi++)
        {
            var p1 = polygon[vi];
            var h1 = plane.Height(p1); bool h1p = h1 > eps, h1n = h1 < -eps;
            if (h0p && h1n || h0n && h1p) pa[pc++] = p0 + (p1 - p0) * (h0 / (h0 - h1));
            if (h1 >= -eps) pa[pc++] = p1;
            p0 = p1; h0 = h1; h0p = h1p; h0n = h1n;
        }
        if (h0p && hfn || h0n && hfp) pa[pc++] = p0 + (pf - p0) * (h0 / (h0 - hf));
        return new Polygon3f(pa, pc);
    }

    /// <summary>
    /// Returns the convex polygon clipped by the set of planes (defined
    /// as Plane3fs), i.e. all parts of the polygon that are at the positive
    /// side of the planes.
    /// </summary>
    public static Polygon3f ConvexClipped(
            this Polygon3f polygon, Plane3f[] planes, float eps = 1e-5f)
    {
        foreach (var c in planes)
        {
            polygon = polygon.ConvexClipped(c, eps);
            if (polygon.PointCount == 0) break;
        }
        return polygon;
    }

    /// <summary>
    /// Returns the polygon clipped by the hull, i.e. all parts of the
    /// polygon that are at the positive side of the hull planes.
    /// </summary>
    public static Polygon3f ConvexClipped(
            this Polygon3f polygon, Hull3f hull, float eps = 1e-5f)
    {
        return polygon.ConvexClipped(hull.PlaneArray, eps);
    }

    /// <summary>
    /// TODO summary.
    /// </summary>
    public static Polygon3f ConvexClipped(
#pragma warning disable IDE0060 // Remove unused parameter
            this Polygon3f polygon, Box3f box, float eps = 1e-5f)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        var planes = new[]
        {
            new Plane3f(V3f.XAxis, box.Min), new Plane3f(-V3f.XAxis, box.Max),
            new Plane3f(V3f.YAxis, box.Min), new Plane3f(-V3f.YAxis, box.Max),
            new Plane3f(V3f.ZAxis, box.Min), new Plane3f(-V3f.ZAxis, box.Max),
        };
        return polygon.ConvexClipped(planes);
    }

    #endregion
}

#endregion

#region IndexPolygon3f

[StructLayout(LayoutKind.Sequential)]
public readonly partial struct IndexPolygon3f : IValidity, IPolygon<V3f>
{
    private readonly int m_pointCount;
    private readonly int m_firstIndex;
    private readonly int[] m_indexArray;
    private readonly V3f[] m_pointArray;

    #region Constructors

    public IndexPolygon3f(int[] indexArray, int firstIndex, int pointCount, V3f[] pointArray)
    {
        m_indexArray = indexArray;
        m_firstIndex = firstIndex;
        m_pointCount = pointCount;
        m_pointArray = pointArray;
    }

    public IndexPolygon3f(V3f[] pointArray, int firstIndex, int pointCount)
        : this(new int[pointCount].SetByIndex(i => firstIndex + i), 0, pointCount, pointArray)
    { }

    public IndexPolygon3f(V3f[] pointArray)
        : this(new int[pointArray.Length].SetByIndex(i => i), 0, pointArray.Length, pointArray)
    { }

    #endregion

    #region Constants

    public static readonly IndexPolygon3f Invalid = new(null, 0, 0, null);

    #endregion

    #region Properties

    public bool IsValid { get { return m_indexArray != null && m_pointArray != null; } }
    public bool IsInvalid { get { return m_indexArray == null || m_pointArray == null; } }

    public int PointCount
    {
        get { return m_pointCount; }
    }

    public int FirstIndex { get { return m_firstIndex; } }

    /// <summary>
    /// The index array that contains the point indices of the
    /// index polygon at the index range [FirstIndex, FirstIndex + PointCount).
    /// NOTE: This is different from the array returned by GetIndexArray().
    /// </summary>
    public int[] IndexArray { get { return m_indexArray; } }

    /// <summary>
    /// The point array that contains the points referenced by
    /// the index array. Note: This is different from the array
    /// returned by GetPointArray().
    /// </summary>
    public V3f[] PointArray { get { return m_pointArray; } }

    public IEnumerable<V3f> Points
    {
        get
        {
            for (int i = 0; i < m_pointCount; i++)
                yield return m_pointArray[m_indexArray[m_firstIndex + i]];
        }
    }

    public IEnumerable<int> Indices
    {
        get
        {
            for (int i = 0; i < m_pointCount; i++)
                yield return m_indexArray[m_firstIndex + i];
        }
    }

    #endregion

    #region Indexing

    public V3f this[int index]
    {
        get { return m_pointArray[m_indexArray[m_firstIndex + index]]; }
        set { m_pointArray[m_indexArray[m_firstIndex + index]] = value; }
    }

    #endregion

    #region Conversions

    public void ForEachIndex(Action<int> index_act)
    {
        var ia = m_indexArray; int fi = m_firstIndex;
        for (int i = 0; i < m_pointCount; i++) index_act(ia[fi + i]);
    }

    /// <summary>
    /// Returns a newly created array containing only the actual indices
    /// of the index polygon. NOTE: This is different from the
    /// IndexArray property!
    /// </summary>
    public int[] GetIndexArray()
    {
        return m_indexArray.Copy(m_firstIndex, m_pointCount);
    }

    /// <summary>
    /// Returns a newly created array containing only the actual points
    /// of the index polygon. NOTE: This is different from the
    /// PointArray property!
    /// </summary>
    public V3f[] GetPointArray()
    {
        var pa = m_pointArray;
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pa[i]);
    }

    public T[] GetPointArray<T>(T[] pointArray)
    {
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pointArray[i]);
    }

    public T[] GetPointArray<T>(List<T> pointList)
    {
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pointList[i]);
    }

    #endregion
}

#endregion

#region IndexPolygon3fExtensions

public static partial class IndexPolygon3fExtensions
{
    #region Conversions

    public static Polygon3f ToPolygon3f(this IndexPolygon3f polygon)
    {
        return new Polygon3f(polygon.GetPointArray());
    }

    #endregion

    #region Geometric Properties

    /// <summary>
    /// The vertex centroid is the average of the vertex coordinates.
    /// </summary>
    public static V3f ComputeVertexCentroid(this IndexPolygon3f polygon)
    {
        var sum = V3f.Zero;
        int pc = polygon.PointCount;
        for (int i = 0; i < pc; i++) sum += polygon[i];
        float scale = 1 / (float)pc;
        return sum * scale;
    }

    public static float ComputePerimeter(this IndexPolygon3f polygon)
    {
        var pc = polygon.PointCount;
        var p0 = polygon[pc - 1];
        float r = 0;
        for (int i = 0; i < pc; i++)
        {
            var p1 = polygon[i];
            r += Vec.Distance(p0, p1);
            p0 = p1;
        }
        return r;
    }

    #endregion
}

#endregion

#region Line3f

[StructLayout(LayoutKind.Sequential)]
public partial struct Line3f : IEquatable<Line3f>, IValidity, IPolygon<V3f>, IBoundingBox3f
{
    public V3f P0, P1;

    #region Constructors

    /// <summary>
    /// Creates line from 2 points.
    /// </summary>
    public Line3f(V3f p0, V3f p1)
    {
        P0 = p0; P1 = p1;
    }

    /// <summary>
    /// Creates line from first 2 points in the sequence.
    /// </summary>
    public Line3f(IEnumerable<V3f> points)
    {
        var pa = points.TakeToArray(2);
        P0 = pa[0]; P1 = pa[1];
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    public readonly int PointCount { get { return 2; } }

    public readonly IEnumerable<V3f> Points
    {
        get { yield return P0; yield return P1; }
    }

    public readonly Line3f Reversed
    {
        get { return new Line3f(P1, P0); }
    }

    #endregion

    #region Indexer

    public V3f this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Line3f Copy(Func<V3f, V3f> point_copyFun)
    {
        return new Line3f(point_copyFun(P0), point_copyFun(P1));
    }

    public readonly Line2d ToLine2d(Func<V3f, V2d> point_copyFun)
    {
        return new Line2d(point_copyFun(P0), point_copyFun(P1));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Line3f a, Line3f b)
        => (a.P0 == b.P0) && (a.P1 == b.P1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Line3f a, Line3f b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Line3f other)
        => P0.Equals(other.P0) && P1.Equals(other.P1);

    public override readonly bool Equals(object other)
         => (other is Line3f o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", P0, P1);
    }

    public static Line3f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Line3f(V3f.Parse(x[0]), V3f.Parse(x[1]));
    }

    #endregion

    #region IBoundingBox3f Members

    public readonly Box3f BoundingBox3f
    {
        get
        {
            return new Box3f(P0, P1).Repair();
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Line3f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Line3f a, Line3f b, float tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Line3f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Line3f a, Line3f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region Line3fExtensions

public static partial class Line3fExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Line3f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line3f polygon, float scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Scaled(this Line3f polygon, float scale)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line3f polygon, V3f center, float scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Scaled(this Line3f polygon, V3f center, float scale)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Line3f polygon, float scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f ScaledAboutCentroid(this Line3f polygon, float scale)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line3f polygon, V3f scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Scaled(this Line3f polygon, V3f scale)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line3f polygon, V3f center, V3f scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Scaled(this Line3f polygon, V3f center, V3f scale)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Line3f polygon, V3f scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f ScaledAboutCentroid(this Line3f polygon, V3f scale)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the given <see cref="M44f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3f polygon, M44f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the given <see cref="M44f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Transformed(this Line3f polygon, M44f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3f polygon, Euclidean3f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Transformed(this Line3f polygon, Euclidean3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the inverse of the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3f polygon, Euclidean3f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the inverse of the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f InvTransformed(this Line3f polygon, Euclidean3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3f polygon, Similarity3f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Transformed(this Line3f polygon, Similarity3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the inverse of the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3f polygon, Similarity3f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the inverse of the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f InvTransformed(this Line3f polygon, Similarity3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the given <see cref="Affine3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3f polygon, Affine3f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the given <see cref="Affine3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Transformed(this Line3f polygon, Affine3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3f polygon, Shift3f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Transformed(this Line3f polygon, Shift3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the inverse of the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3f polygon, Shift3f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the inverse of the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f InvTransformed(this Line3f polygon, Shift3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3f polygon, Rot3f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Transformed(this Line3f polygon, Rot3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the inverse of the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3f polygon, Rot3f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the inverse of the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f InvTransformed(this Line3f polygon, Rot3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3f polygon, Scale3f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Transformed(this Line3f polygon, Scale3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the inverse of the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3f polygon, Scale3f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the inverse of the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f InvTransformed(this Line3f polygon, Scale3f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3f"/> by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3f polygon, M33f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3f"/> transformed by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3f Transformed(this Line3f polygon, M33f t)
    {
        var result = new Line3f(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V3f[] GetPointArray(this Line3f line)
    {
        var pa = new V3f[2];
        pa[0] = line.P0;
        pa[1] = line.P1;
        return pa;
    }

    public static V3f ComputeCentroid(this Line3f line)
    {
        return 0.5f * (line.P0 + line.P1);
    }
}

#endregion

#region Triangle3f

[StructLayout(LayoutKind.Sequential)]
public partial struct Triangle3f : IEquatable<Triangle3f>, IValidity, IPolygon<V3f>, IBoundingBox3f
{
    public V3f P0, P1, P2;

    #region Constructors

    /// <summary>
    /// Creates triangle from 3 points.
    /// </summary>
    public Triangle3f(V3f p0, V3f p1, V3f p2)
    {
        P0 = p0; P1 = p1; P2 = p2;
    }

    /// <summary>
    /// Creates triangle from first 3 points in the sequence.
    /// </summary>
    public Triangle3f(IEnumerable<V3f> points)
    {
        var pa = points.TakeToArray(3);
        P0 = pa[0]; P1 = pa[1]; P2 = pa[2];
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    /// <summary>
    /// Edge P1 - P0
    /// </summary>
    public readonly V3f Edge01 { get { return P1 - P0; } }
    /// <summary>
    /// Edge P2 - P0
    /// </summary>
    public readonly V3f Edge02 { get { return P2 - P0; } }
    /// <summary>
    /// Edge P2 - P1
    /// </summary>
    public readonly V3f Edge12 { get { return P2 - P1; } }
    /// <summary>
    /// Edge P0 - P1
    /// </summary>
    public readonly V3f Edge10 { get { return P0 - P1; } }
    /// <summary>
    /// Edge P0 - P2
    /// </summary>
    public readonly V3f Edge20 { get { return P0 - P2; } }
    /// <summary>
    /// Edge P1 - P2
    /// </summary>
    public readonly V3f Edge21 { get { return P1 - P2; } }

    public readonly IEnumerable<V3f> Edges
    {
        get
        {
            yield return P1 - P0;
            yield return P2 - P1;
            yield return P0 - P2;
        }
    }

    public readonly V3f[] EdgeArray
    {
        get
        {
            var a = new V3f[3];
            a[0] = P1 - P0;
            a[1] = P2 - P1;
            a[2] = P0 - P2;
            return a;
        }
    }

    public readonly IEnumerable<Line3f> EdgeLines
    {
        get
        {
            yield return new Line3f(P0, P1);
            yield return new Line3f(P1, P2);
            yield return new Line3f(P2, P0);
        }
    }

    public readonly Line3f[] EdgeLineArray
    {
        get
        {
            var a = new Line3f[3];
            a[0] = new Line3f(P0, P1);
            a[1] = new Line3f(P1, P2);
            a[2] = new Line3f(P2, P0);
            return a;
        }
    }

    public readonly Line3f GetEdgeLine(int index) => index switch
    {
        0 => new Line3f(P0, P1),
        1 => new Line3f(P1, P2),
        2 => new Line3f(P2, P0),
        _ => throw new InvalidOperationException()
    };

    public readonly V3f GetEdge(int index) => index switch
    {
        0 => P1 - P0,
        1 => P2 - P1,
        2 => P0 - P2,
        _ => throw new InvalidOperationException()
    };

    public readonly Line3f Line01 { get { return new Line3f(P0, P1); } }
    public readonly Line3f Line02 { get { return new Line3f(P0, P2); } }
    public readonly Line3f Line12 { get { return new Line3f(P1, P2); } }
    public readonly Line3f Line10 { get { return new Line3f(P1, P0); } }
    public readonly Line3f Line20 { get { return new Line3f(P2, P0); } }
    public readonly Line3f Line21 { get { return new Line3f(P2, P1); } }

    public readonly int PointCount { get { return 3; } }

    public readonly IEnumerable<V3f> Points
    {
        get { yield return P0; yield return P1; yield return P2; }
    }

    public readonly Triangle3f Reversed
    {
        get { return new Triangle3f(P2, P1, P0); }
    }

    #endregion

    #region Indexer

    public V3f this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                2 => P2,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                case 2: P2 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Triangle3f Copy(Func<V3f, V3f> point_copyFun)
    {
        return new Triangle3f(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
    }

    public readonly Triangle2d ToTriangle2d(Func<V3f, V2d> point_copyFun)
    {
        return new Triangle2d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Triangle3f a, Triangle3f b)
        => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.P2 == b.P2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Triangle3f a, Triangle3f b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1, P2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Triangle3f other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2);

    public override readonly bool Equals(object other)
         => (other is Triangle3f o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, P2);
    }

    public static Triangle3f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Triangle3f(V3f.Parse(x[0]), V3f.Parse(x[1]), V3f.Parse(x[2]));
    }

    #endregion

    #region IBoundingBox3f Members

    public readonly Box3f BoundingBox3f
    {
        get
        {
            return new Box3f(P0, P1, P2);
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Triangle3f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Triangle3f a, Triangle3f b, float tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance) && ApproximateEquals(a.P2, b.P2, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Triangle3f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Triangle3f a, Triangle3f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region Triangle3fExtensions

public static partial class Triangle3fExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Triangle3f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle3f polygon, float scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Scaled(this Triangle3f polygon, float scale)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle3f polygon, V3f center, float scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Scaled(this Triangle3f polygon, V3f center, float scale)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Triangle3f polygon, float scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f ScaledAboutCentroid(this Triangle3f polygon, float scale)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle3f polygon, V3f scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Scaled(this Triangle3f polygon, V3f scale)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle3f polygon, V3f center, V3f scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Scaled(this Triangle3f polygon, V3f center, V3f scale)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Triangle3f polygon, V3f scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f ScaledAboutCentroid(this Triangle3f polygon, V3f scale)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the given <see cref="M44f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3f polygon, M44f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the given <see cref="M44f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Transformed(this Triangle3f polygon, M44f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3f polygon, Euclidean3f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Transformed(this Triangle3f polygon, Euclidean3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the inverse of the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3f polygon, Euclidean3f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the inverse of the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f InvTransformed(this Triangle3f polygon, Euclidean3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3f polygon, Similarity3f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Transformed(this Triangle3f polygon, Similarity3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the inverse of the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3f polygon, Similarity3f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the inverse of the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f InvTransformed(this Triangle3f polygon, Similarity3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the given <see cref="Affine3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3f polygon, Affine3f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the given <see cref="Affine3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Transformed(this Triangle3f polygon, Affine3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3f polygon, Shift3f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Transformed(this Triangle3f polygon, Shift3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the inverse of the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3f polygon, Shift3f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the inverse of the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f InvTransformed(this Triangle3f polygon, Shift3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3f polygon, Rot3f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Transformed(this Triangle3f polygon, Rot3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the inverse of the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3f polygon, Rot3f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the inverse of the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f InvTransformed(this Triangle3f polygon, Rot3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3f polygon, Scale3f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Transformed(this Triangle3f polygon, Scale3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the inverse of the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3f polygon, Scale3f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the inverse of the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f InvTransformed(this Triangle3f polygon, Scale3f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3f"/> by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3f polygon, M33f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3f"/> transformed by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3f Transformed(this Triangle3f polygon, M33f t)
    {
        var result = new Triangle3f(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V3f[] GetPointArray(this Triangle3f triangle)
    {
        var pa = new V3f[3];
        pa[0] = triangle.P0;
        pa[1] = triangle.P1;
        pa[2] = triangle.P2;
        return pa;
    }

    public static V3f ComputeCentroid(this Triangle3f triangle)
    {
        return ConstantF.OneThird * (triangle.P0 + triangle.P1 + triangle.P2);
    }
}

#endregion

#region Quad3f

[StructLayout(LayoutKind.Sequential)]
public partial struct Quad3f : IEquatable<Quad3f>, IValidity, IPolygon<V3f>, IBoundingBox3f
{
    public V3f P0, P1, P2, P3;

    #region Constructors

    /// <summary>
    /// Creates quad from 4 points.
    /// </summary>
    public Quad3f(V3f p0, V3f p1, V3f p2, V3f p3)
    {
        P0 = p0; P1 = p1; P2 = p2; P3 = p3;
    }

    /// <summary>
    /// Creates quad from first 4 points in the sequence.
    /// </summary>
    public Quad3f(IEnumerable<V3f> points)
    {
        var pa = points.TakeToArray(4);
        P0 = pa[0]; P1 = pa[1]; P2 = pa[2]; P3 = pa[3];
    }

    /// <summary>
    /// Creates quad from point and two vectors representing edges.
    /// </summary>
    public Quad3f(V3f p0, V3f edge01, V3f edge03)
    {
        P0 = p0;
        P1 = p0 + edge01;
        P2 = P1 + edge03;
        P3 = p0 + edge03;
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    /// <summary>
    /// Edge P1 - P0
    /// </summary>
    public readonly V3f Edge01 { get { return P1 - P0; } }
    /// <summary>
    /// Edge P3 - P0
    /// </summary>
    public readonly V3f Edge03 { get { return P3 - P0; } }
    /// <summary>
    /// Edge P2 - P1
    /// </summary>
    public readonly V3f Edge12 { get { return P2 - P1; } }
    /// <summary>
    /// Edge P0 - P1
    /// </summary>
    public readonly V3f Edge10 { get { return P0 - P1; } }
    /// <summary>
    /// Edge P3 - P2
    /// </summary>
    public readonly V3f Edge23 { get { return P3 - P2; } }
    /// <summary>
    /// Edge P1 - P2
    /// </summary>
    public readonly V3f Edge21 { get { return P1 - P2; } }
    /// <summary>
    /// Edge P0 - P3
    /// </summary>
    public readonly V3f Edge30 { get { return P0 - P3; } }
    /// <summary>
    /// Edge P2 - P3
    /// </summary>
    public readonly V3f Edge32 { get { return P2 - P3; } }

    public readonly IEnumerable<V3f> Edges
    {
        get
        {
            yield return P1 - P0;
            yield return P2 - P1;
            yield return P3 - P2;
            yield return P0 - P3;
        }
    }

    public readonly V3f[] EdgeArray
    {
        get
        {
            var a = new V3f[4];
            a[0] = P1 - P0;
            a[1] = P2 - P1;
            a[2] = P3 - P2;
            a[3] = P0 - P3;
            return a;
        }
    }

    public readonly IEnumerable<Line3f> EdgeLines
    {
        get
        {
            yield return new Line3f(P0, P1);
            yield return new Line3f(P1, P2);
            yield return new Line3f(P2, P3);
            yield return new Line3f(P3, P0);
        }
    }

    public readonly Line3f[] EdgeLineArray
    {
        get
        {
            var a = new Line3f[4];
            a[0] = new Line3f(P0, P1);
            a[1] = new Line3f(P1, P2);
            a[2] = new Line3f(P2, P3);
            a[3] = new Line3f(P3, P0);
            return a;
        }
    }

    public readonly Line3f GetEdgeLine(int index) => index switch
    {
        0 => new Line3f(P0, P1),
        1 => new Line3f(P1, P2),
        2 => new Line3f(P2, P3),
        3 => new Line3f(P3, P0),
        _ => throw new InvalidOperationException()
    };

    public readonly V3f GetEdge(int index) => index switch
    {
        0 => P1 - P0,
        1 => P2 - P1,
        2 => P3 - P2,
        3 => P0 - P3,
        _ => throw new InvalidOperationException()
    };

    public readonly Line3f Line01 { get { return new Line3f(P0, P1); } }
    public readonly Line3f Line03 { get { return new Line3f(P0, P3); } }
    public readonly Line3f Line12 { get { return new Line3f(P1, P2); } }
    public readonly Line3f Line10 { get { return new Line3f(P1, P0); } }
    public readonly Line3f Line23 { get { return new Line3f(P2, P3); } }
    public readonly Line3f Line21 { get { return new Line3f(P2, P1); } }
    public readonly Line3f Line30 { get { return new Line3f(P3, P0); } }
    public readonly Line3f Line32 { get { return new Line3f(P3, P2); } }

    public readonly int PointCount { get { return 4; } }

    public readonly IEnumerable<V3f> Points
    {
        get { yield return P0; yield return P1; yield return P2; yield return P3; }
    }

    public readonly Quad3f Reversed
    {
        get { return new Quad3f(P3, P2, P1, P0); }
    }

    #endregion

    #region Indexer

    public V3f this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                2 => P2,
                3 => P3,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                case 2: P2 = value; return;
                case 3: P3 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Quad3f Copy(Func<V3f, V3f> point_copyFun)
    {
        return new Quad3f(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
    }

    public readonly Quad2d ToQuad2d(Func<V3f, V2d> point_copyFun)
    {
        return new Quad2d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quad3f a, Quad3f b)
        => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.P2 == b.P2) && (a.P3 == b.P3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quad3f a, Quad3f b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1, P2, P3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Quad3f other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

    public override readonly bool Equals(object other)
         => (other is Quad3f o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", P0, P1, P2, P3);
    }

    public static Quad3f Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Quad3f(V3f.Parse(x[0]), V3f.Parse(x[1]), V3f.Parse(x[2]), V3f.Parse(x[3]));
    }

    #endregion

    #region IBoundingBox3f Members

    public readonly Box3f BoundingBox3f
    {
        get
        {
            return new Box3f(P0, P1, P2, P3);
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Quad3f"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Quad3f a, Quad3f b, float tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance) && ApproximateEquals(a.P2, b.P2, tolerance) && ApproximateEquals(a.P3, b.P3, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Quad3f"/> are equal within
    /// Constant&lt;float&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Quad3f a, Quad3f b)
        => ApproximateEquals(a, b, Constant<float>.PositiveTinyValue);
}

#endregion

#region Quad3fExtensions

public static partial class Quad3fExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Quad3f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad3f polygon, float scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
        polygon.P3 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Scaled(this Quad3f polygon, float scale)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad3f polygon, V3f center, float scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
        polygon.P3 = center + (polygon.P3 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Scaled(this Quad3f polygon, V3f center, float scale)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Quad3f polygon, float scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f ScaledAboutCentroid(this Quad3f polygon, float scale)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3f"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad3f polygon, V3f scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
        polygon.P3 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Scaled(this Quad3f polygon, V3f scale)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3f"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad3f polygon, V3f center, V3f scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
        polygon.P3 = center + (polygon.P3 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Scaled(this Quad3f polygon, V3f center, V3f scale)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3f"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Quad3f polygon, V3f scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f ScaledAboutCentroid(this Quad3f polygon, V3f scale)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the given <see cref="M44f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3f polygon, M44f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the given <see cref="M44f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Transformed(this Quad3f polygon, M44f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3f polygon, Euclidean3f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Transformed(this Quad3f polygon, Euclidean3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the inverse of the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3f polygon, Euclidean3f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
        polygon.P3 = t.InvTransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the inverse of the given <see cref="Euclidean3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f InvTransformed(this Quad3f polygon, Euclidean3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3f polygon, Similarity3f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Transformed(this Quad3f polygon, Similarity3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the inverse of the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3f polygon, Similarity3f t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
        polygon.P3 = t.InvTransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the inverse of the given <see cref="Similarity3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f InvTransformed(this Quad3f polygon, Similarity3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the given <see cref="Affine3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3f polygon, Affine3f t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the given <see cref="Affine3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Transformed(this Quad3f polygon, Affine3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3f polygon, Shift3f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Transformed(this Quad3f polygon, Shift3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the inverse of the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3f polygon, Shift3f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the inverse of the given <see cref="Shift3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f InvTransformed(this Quad3f polygon, Shift3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3f polygon, Rot3f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Transformed(this Quad3f polygon, Rot3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the inverse of the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3f polygon, Rot3f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the inverse of the given <see cref="Rot3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f InvTransformed(this Quad3f polygon, Rot3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3f polygon, Scale3f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Transformed(this Quad3f polygon, Scale3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the inverse of the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3f polygon, Scale3f t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the inverse of the given <see cref="Scale3f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f InvTransformed(this Quad3f polygon, Scale3f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3f"/> by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3f"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3f polygon, M33f t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3f"/> transformed by the given <see cref="M33f"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3f to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3f Transformed(this Quad3f polygon, M33f t)
    {
        var result = new Quad3f(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V3f[] GetPointArray(this Quad3f quad)
    {
        var pa = new V3f[4];
        pa[0] = quad.P0;
        pa[1] = quad.P1;
        pa[2] = quad.P2;
        pa[3] = quad.P3;
        return pa;
    }

    public static V3f ComputeCentroid(this Quad3f quad)
    {
        return 0.25f * (quad.P0 + quad.P1 + quad.P2 + quad.P3);
    }
}

#endregion

#region Polygon2d

/// <summary>
/// A polygon internally represented by an array of points. Implemented
/// as a structure, the validity of the polygon can be checked via its
/// PointCount, which must be bigger than 0 for a polygon to hold any
/// points, and bigger than 2 for a polygon to be geometrically valid.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public partial struct Polygon2d : IEquatable<Polygon2d>, IValidity, IPolygon<V2d>, IBoundingBox2d
{
    internal int m_pointCount;
    internal V2d[] m_pointArray;

    #region Constructors

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon2d(V2d[] pointArray, int pointCount)
    {
        if (pointArray != null)
        {
            if (pointCount <= pointArray.Length)
            {
                m_pointCount = pointCount;
                m_pointArray = pointArray;
            }
            else
                throw new ArgumentException("point count must be smaller or equal array length");
        }
        else
        {
            m_pointCount = 0;
            m_pointArray = null;
        }
    }

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon2d(params V2d[] pointArray)
    {
        m_pointCount = pointArray != null ? pointArray.Length : 0;
        m_pointArray = pointArray;
    }

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon2d(V2d[] pointArray, int startIndex, int count)
    {
        if (startIndex < 0 || startIndex >= pointArray.Length - 1) throw new ArgumentOutOfRangeException(nameof(startIndex));
        if (count <= 0 || startIndex + count >= pointArray.Length) throw new ArgumentOutOfRangeException(nameof(count));
        m_pointCount = count;
        m_pointArray = new V2d[count];
        for (int i = 0; i < count; i++) m_pointArray[i] = pointArray[startIndex + i];
    }

    /// <summary>
    /// Creates a polygon from point count and point creator function.
    /// </summary>
    public Polygon2d(int pointCount, Func<int, V2d> index_pointCreator)
        : this(new V2d[pointCount].SetByIndex(index_pointCreator))
    { }

    /// <summary>
    /// Creates a polygon from a sequence of points.
    /// </summary>
    public Polygon2d(IEnumerable<V2d> points)
        : this([.. points])
    { }

    /// <summary>
    /// Creates a polygon from the points of a pointArray that
    /// are selected by an index array.
    /// </summary>
    public Polygon2d(int[] indexArray, V2d[] pointArray)
        : this(indexArray.Map(i => pointArray[i]))
    { }

    /// <summary>
    /// Creates a polygon from a triangle.
    /// </summary>
    public Polygon2d(Triangle2d triangle)
        : this(triangle.GetPointArray())
    { }

    /// <summary>
    /// Creates a polygon from a quad.
    /// </summary>
    public Polygon2d(Quad2d quad)
        : this(quad.GetPointArray())
    { }

    /// <summary>
    /// Copy constructor.
    /// Performs deep copy of original.
    /// </summary>
    public Polygon2d(Polygon2d original)
        : this(original.GetPointArray())
    { }

    #endregion

    #region Constants

    public static readonly Polygon2d Invalid = new(null, 0);

    #endregion

    #region Properties

    public readonly bool IsValid => m_pointArray != null;

    public readonly bool IsInvalid => m_pointArray == null;

    /// <summary>
    /// The number of points in the polygon. If this is 0, the polygon
    /// is invalid.
    /// </summary>
    public readonly int PointCount => m_pointCount;

    /// <summary>
    /// Enumerates points.
    /// </summary>
    public readonly IEnumerable<V2d> Points
    {
        get { for (int pi = 0; pi < m_pointCount; pi++) yield return m_pointArray[pi]; }
    }

    #endregion

    #region Conversions

    /// <summary>
    /// Returns a copy of the polygons point array.
    /// </summary>
    public readonly V2d[] GetPointArray()
    {
        var pc = m_pointCount;
        var pa = new V2d[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
        return pa;
    }

    /// <summary>
    /// [P0, P1, P2] -> [P0, P1, P2, P0].
    /// </summary>
    public readonly V2d[] GetPointArrayWithRepeatedFirstPoint()
    {
        var pc = m_pointCount;
        var pa = new V2d[pc + 1];
        for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
        pa[pc] = pa[0];
        return pa;
    }

    /// <summary>
    /// Returns a transformed copy of the polygons point array.
    /// </summary>
    public readonly T[] GetPointArray<T>(Func<V2d, T> point_copyFun)
    {
        var pc = m_pointCount;
        var pa = new T[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = point_copyFun(m_pointArray[pi]);
        return pa;
    }

    /// <summary>
    /// Returns a transformed copy of the polygons point array.
    /// </summary>
    public readonly T[] GetPointArray<T>(Func<V2d, int, T> point_index_copyFun)
    {
        var pc = m_pointCount;
        var pa = new T[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = point_index_copyFun(m_pointArray[pi], pi);
        return pa;
    }

    #endregion

    #region Indexing

    /// <summary>
    /// Gets the index-th point of this polygon.
    /// </summary>
    public readonly V2d this[int index]
    {
        get { return m_pointArray[index]; }
        set { m_pointArray[index] = value; }
    }

    #endregion

    #region Edges and Lines

    /// <summary>
    /// Index-th edge as vector (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly V2d Edge(int index)
    {
        var p0 = m_pointArray[index++];
        var p1 = m_pointArray[index < m_pointCount ? index : 0];
        return p1 - p0;
    }

    /// <summary>
    /// Edges as vectors (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly IEnumerable<V2d> Edges
    {
        get
        {
            var pc = m_pointCount;
            var p = m_pointArray[0];
            var p0 = p;
            for (int pi = 1; pi < pc; pi++)
            {
                var p1 = m_pointArray[pi];
                yield return p1 - p0;
                p0 = p1;
            }
            yield return p - p0;
        }
    }

    /// <summary>
    /// Index-th edge as line segment (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly Line2d EdgeLine(int index)
    {
        var p0 = m_pointArray[index++];
        var p1 = m_pointArray[index < m_pointCount ? index : 0];
        return new Line2d(p0, p1);
    }

    /// <summary>
    /// Edges as line segments (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly IEnumerable<Line2d> EdgeLines
    {
        get
        {
            var pc = m_pointCount;
            if (pc < 1) yield break;
            var p0 = m_pointArray[0];
            var p = p0;
            for (int pi = 1; pi < pc; pi++)
            {
                var p1 = m_pointArray[pi];
                yield return new Line2d(p0, p1);
                p0 = p1;
            }
            yield return new Line2d(p0, p);
        }
    }

    /// <summary>
    /// Edges as vectors (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly V2d[] GetEdgeArray()
    {
        var pc = m_pointCount;
        if (pc < 2) return [];
        var edgeArray = new V2d[pc];
        var p = m_pointArray[0];
        var p0 = p;
        for (int pi = 1; pi < pc; pi++)
        {
            var p1 = m_pointArray[pi];
            edgeArray[pi - 1] = p1 - p0;
            p0 = p1;
        }
        edgeArray[pc - 1] = p - p0;
        return edgeArray;
    }

    /// <summary>
    /// Edges as line segments (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly Line2d[] GetEdgeLineArray()
    {
        var pc = PointCount;
        if (pc < 2) return [];
        var ela = new Line2d[pc];
        var p = m_pointArray[0];
        var p0 = p;
        for (int pi = 1; pi < pc; pi++)
        {
            var p1 = m_pointArray[pi];
            ela[pi - 1] = new Line2d(p0, p1);
            p0 = p1;
        }
        ela[pc - 1] = new Line2d(p0, p);
        return ela;
    }

    #endregion

    #region Transformations

    /// <summary>
    /// Returns copy of polygon. Same as Map(p => p).
    /// </summary>
    public readonly Polygon2d Copy()
    {
        return new Polygon2d(m_pointArray.Copy());
    }

    /// <summary>
    /// Returns transformed copy of this polygon.
    /// </summary>
    public readonly Polygon2d Map(Func<V2d, V2d> point_fun)
    {
        var pc = m_pointCount;
        V2d[] opa = m_pointArray, npa = new V2d[pc];
        for (int pi = 0; pi < pc; pi++) npa[pi] = point_fun(opa[pi]);
        return new Polygon2d(npa, pc);
    }

    /// <summary>
    /// Gets copy with reversed order of vertices.
    /// </summary>
    public readonly Polygon2d Reversed
    {
        get
        {
            var pc = m_pointCount;
            V2d[] opa = m_pointArray, npa = new V2d[pc];
            for (int pi = 0, pj = pc - 1; pi < pc; pi++, pj--) npa[pi] = opa[pj];
            return new Polygon2d(npa, pc);
        }
    }

    /// <summary>
    /// Reverses order of vertices in-place.
    /// </summary>
    public readonly void Reverse()
    {
        var pa = m_pointArray;
        for (int pi = 0, pj = m_pointCount - 1; pi < pj; pi++, pj--)
        {
            (pa[pj], pa[pi]) = (pa[pi], pa[pj]);
        }
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Polygon2d a, Polygon2d b)
    {
        if (a.m_pointCount != b.m_pointCount) return false;
        for (int pi = 0; pi < a.m_pointCount; pi++)
            if (a.m_pointArray[pi] != b.m_pointArray[pi]) return false;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Polygon2d a, Polygon2d b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return m_pointArray.GetCombinedHashCode(m_pointCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Polygon2d other)
    {
        if (m_pointCount != other.m_pointCount) return false;
        for (int pi = 0; pi < m_pointCount; pi++)
            if (!m_pointArray[pi].Equals(other.m_pointArray[pi])) return false;
        return true;
    }

    public override readonly bool Equals(object other)
        => (other is Polygon2d o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture,
            "[{0}]", Points.Select(x => x.ToString()).Join(", ")
            );
    }

    public static Polygon2d Parse(string s)
    {
        var va = s.NestedBracketSplitLevelOne().ToArray();
        return new Polygon2d(va.Select(x => V2d.Parse(x)));
    }

    #endregion

    #region IBoundingBox2d Members

    /// <summary>
    /// Bounding box of polygon.
    /// </summary>
    public readonly Box2d BoundingBox2d
    {
        get { return new Box2d(m_pointArray, 0, m_pointCount); }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Polygon2d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Polygon2d a, Polygon2d b, double tolerance)
    {
        if (a.m_pointCount != b.m_pointCount) return false;
        for (int pi = 0; pi < a.m_pointCount; pi++)
            if (!ApproximateEquals(a.m_pointArray[pi], b.m_pointArray[pi], tolerance)) return false;
        return true;
    }

    /// <summary>
    /// Returns whether the given <see cref="Polygon2d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Polygon2d a, Polygon2d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

#region Polygon2dExtensions

public static partial class Polygon2dExtensions
{
    #region Geometric Properties

    /// <summary>
    /// The vertex centroid is the average of the vertex coordinates.
    /// </summary>
    public static V2d ComputeVertexCentroid(this Polygon2d polygon)
    {
        var sum = V2d.Zero;
        int pc = polygon.PointCount;
        for (int i = 0; i < pc; i++) sum += polygon[i];
        double scale = 1 / (double)pc;
        return sum * scale;
    }

    public static double ComputePerimeter(this Polygon2d polygon)
    {
        var pc = polygon.PointCount;
        var p0 = polygon[pc - 1];
        double r = 0;
        for (int i = 0; i < pc; i++)
        {
            var p1 = polygon[i];
            r += Vec.Distance(p0, p1);
            p0 = p1;
        }
        return r;
    }

    #endregion

    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Polygon2d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon2d polygon, double scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Scaled(this Polygon2d polygon, double scale)
    {
        var result = new Polygon2d(polygon);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon2d polygon, V2d center, double scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = center + (polygon.m_pointArray[pi] - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Scaled(this Polygon2d polygon, V2d center, double scale)
    {
        var result = new Polygon2d(polygon);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutVertexCentroid(this ref Polygon2d polygon, double scale)
    {
        var center = polygon.ComputeVertexCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d ScaledAboutVertexCentroid(this Polygon2d polygon, double scale)
    {
        var result = new Polygon2d(polygon);
        result.ScaleAboutVertexCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon2d polygon, V2d scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Scaled(this Polygon2d polygon, V2d scale)
    {
        var result = new Polygon2d(polygon);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon2d polygon, V2d center, V2d scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = center + (polygon.m_pointArray[pi] - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Scaled(this Polygon2d polygon, V2d center, V2d scale)
    {
        var result = new Polygon2d(polygon);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon2d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutVertexCentroid(this ref Polygon2d polygon, V2d scale)
    {
        var center = polygon.ComputeVertexCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d ScaledAboutVertexCentroid(this Polygon2d polygon, V2d scale)
    {
        var result = new Polygon2d(polygon);
        result.ScaleAboutVertexCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2d polygon, M33d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Transformed(this Polygon2d polygon, M33d t)
    {
        var result = new Polygon2d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2d polygon, Euclidean2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Transformed(this Polygon2d polygon, Euclidean2d t)
    {
        var result = new Polygon2d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the inverse of the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2d polygon, Euclidean2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the inverse of the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d InvTransformed(this Polygon2d polygon, Euclidean2d t)
    {
        var result = new Polygon2d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2d polygon, Similarity2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Transformed(this Polygon2d polygon, Similarity2d t)
    {
        var result = new Polygon2d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the inverse of the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2d polygon, Similarity2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the inverse of the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d InvTransformed(this Polygon2d polygon, Similarity2d t)
    {
        var result = new Polygon2d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the given <see cref="Affine2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2d polygon, Affine2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the given <see cref="Affine2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Transformed(this Polygon2d polygon, Affine2d t)
    {
        var result = new Polygon2d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2d polygon, Shift2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Transformed(this Polygon2d polygon, Shift2d t)
    {
        var result = new Polygon2d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the inverse of the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2d polygon, Shift2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the inverse of the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d InvTransformed(this Polygon2d polygon, Shift2d t)
    {
        var result = new Polygon2d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2d polygon, Rot2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Transformed(this Polygon2d polygon, Rot2d t)
    {
        var result = new Polygon2d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the inverse of the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2d polygon, Rot2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the inverse of the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d InvTransformed(this Polygon2d polygon, Rot2d t)
    {
        var result = new Polygon2d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2d polygon, Scale2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Transformed(this Polygon2d polygon, Scale2d t)
    {
        var result = new Polygon2d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the inverse of the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon2d polygon, Scale2d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the inverse of the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d InvTransformed(this Polygon2d polygon, Scale2d t)
    {
        var result = new Polygon2d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon2d"/> by the given <see cref="M22d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon2d polygon, M22d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon2d"/> transformed by the given <see cref="M22d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon2d Transformed(this Polygon2d polygon, M22d t)
    {
        var result = new Polygon2d(polygon);
        result.Transform(t);
        return result;
    }

    public static Polygon2d WithoutMultiplePoints(this Polygon2d polygon, double eps = 1e-8)
    {
        eps *= eps;
        var opc = polygon.PointCount;
        var pa = new V2d[opc];
        var pc = 0;
        pa[0] = polygon[0];
        for (int pi = 1; pi < opc; pi++)
            if (Vec.DistanceSquared(pa[pc], polygon[pi]) > eps)
                pa[++pc] = polygon[pi];
        if (Vec.DistanceSquared(pa[pc], polygon[0]) > eps)
            ++pc;
        return new Polygon2d(pa, pc);
    }

    #endregion

    #region Clipping

    /// <summary>
    /// Clip the supplied polygon at the supplied line. The method should
    /// work with all non-selfintersecting polygons. Returns all parts of
    /// the polygon that are at the positive side of the line.
    /// </summary>
    public static Polygon2d ConvexClipped(
            this Polygon2d polygon, Plane2d line, double eps = 1e-8)
    {
        var opc = polygon.PointCount;
        V2d[] pa = new V2d[opc + 1];
        var pc = 0;
        var pf = polygon[0];
        var hf = line.Height(pf); bool hfp = hf > eps, hfn = hf < -eps;
        if (hf >= -eps) pa[pc++] = pf;
        var p0 = pf; var h0 = hf; var h0p = hfp; var h0n = hfn;
        for (int vi = 1; vi < opc; vi++)
        {
            var p1 = polygon[vi];
            var h1 = line.Height(p1); bool h1p = h1 > eps, h1n = h1 < -eps;
            if (h0p && h1n || h0n && h1p) pa[pc++] = p0 + (p1 - p0) * (h0 / (h0 - h1));
            if (h1 >= -eps) pa[pc++] = p1;
            p0 = p1; h0 = h1; h0p = h1p; h0n = h1n;
        }
        if (h0p && hfn || h0n && hfp) pa[pc++] = p0 + (pf - p0) * (h0 / (h0 - hf));
        return new Polygon2d(pa, pc);
    }

    /// <summary>
    /// Returns the convex polygon clipped by the set of lines (defined
    /// as Plane2ds), i.e. all parts of the polygon that are at the positive
    /// side of the lines.
    /// </summary>
    public static Polygon2d ConvexClipped(
            this Polygon2d polygon, Plane2d[] lines, double eps = 1e-8)
    {
        foreach (var c in lines)
        {
            polygon = polygon.ConvexClipped(c, eps);
            if (polygon.PointCount == 0) break;
        }
        return polygon;
    }

    /// <summary>
    /// Returns the polygon clipped by the hull, i.e. all parts of the
    /// polygon that are at the positive side of the hull lines.
    /// </summary>
    public static Polygon2d ConvexClipped(
            this Polygon2d polygon, Hull2d hull, double eps = 1e-8)
    {
        return polygon.ConvexClipped(hull.PlaneArray, eps);
    }

    /// <summary>
    /// TODO summary.
    /// </summary>
    public static Polygon2d ConvexClipped(
#pragma warning disable IDE0060 // Remove unused parameter
            this Polygon2d polygon, Box2d box, double eps = 1e-8)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        var lines = new[]
        {
            new Plane2d(V2d.XAxis, box.Min), new Plane2d(-V2d.XAxis, box.Max),
            new Plane2d(V2d.YAxis, box.Min), new Plane2d(-V2d.YAxis, box.Max),
        };
        return polygon.ConvexClipped(lines);
    }

    #endregion
}

#endregion

#region IndexPolygon2d

[StructLayout(LayoutKind.Sequential)]
public readonly partial struct IndexPolygon2d : IValidity, IPolygon<V2d>
{
    private readonly int m_pointCount;
    private readonly int m_firstIndex;
    private readonly int[] m_indexArray;
    private readonly V2d[] m_pointArray;

    #region Constructors

    public IndexPolygon2d(int[] indexArray, int firstIndex, int pointCount, V2d[] pointArray)
    {
        m_indexArray = indexArray;
        m_firstIndex = firstIndex;
        m_pointCount = pointCount;
        m_pointArray = pointArray;
    }

    public IndexPolygon2d(V2d[] pointArray, int firstIndex, int pointCount)
        : this(new int[pointCount].SetByIndex(i => firstIndex + i), 0, pointCount, pointArray)
    { }

    public IndexPolygon2d(V2d[] pointArray)
        : this(new int[pointArray.Length].SetByIndex(i => i), 0, pointArray.Length, pointArray)
    { }

    #endregion

    #region Constants

    public static readonly IndexPolygon2d Invalid = new(null, 0, 0, null);

    #endregion

    #region Properties

    public bool IsValid { get { return m_indexArray != null && m_pointArray != null; } }
    public bool IsInvalid { get { return m_indexArray == null || m_pointArray == null; } }

    public int PointCount
    {
        get { return m_pointCount; }
    }

    public int FirstIndex { get { return m_firstIndex; } }

    /// <summary>
    /// The index array that contains the point indices of the
    /// index polygon at the index range [FirstIndex, FirstIndex + PointCount).
    /// NOTE: This is different from the array returned by GetIndexArray().
    /// </summary>
    public int[] IndexArray { get { return m_indexArray; } }

    /// <summary>
    /// The point array that contains the points referenced by
    /// the index array. Note: This is different from the array
    /// returned by GetPointArray().
    /// </summary>
    public V2d[] PointArray { get { return m_pointArray; } }

    public IEnumerable<V2d> Points
    {
        get
        {
            for (int i = 0; i < m_pointCount; i++)
                yield return m_pointArray[m_indexArray[m_firstIndex + i]];
        }
    }

    public IEnumerable<int> Indices
    {
        get
        {
            for (int i = 0; i < m_pointCount; i++)
                yield return m_indexArray[m_firstIndex + i];
        }
    }

    #endregion

    #region Indexing

    public V2d this[int index]
    {
        get { return m_pointArray[m_indexArray[m_firstIndex + index]]; }
        set { m_pointArray[m_indexArray[m_firstIndex + index]] = value; }
    }

    #endregion

    #region Conversions

    public void ForEachIndex(Action<int> index_act)
    {
        var ia = m_indexArray; int fi = m_firstIndex;
        for (int i = 0; i < m_pointCount; i++) index_act(ia[fi + i]);
    }

    /// <summary>
    /// Returns a newly created array containing only the actual indices
    /// of the index polygon. NOTE: This is different from the
    /// IndexArray property!
    /// </summary>
    public int[] GetIndexArray()
    {
        return m_indexArray.Copy(m_firstIndex, m_pointCount);
    }

    /// <summary>
    /// Returns a newly created array containing only the actual points
    /// of the index polygon. NOTE: This is different from the
    /// PointArray property!
    /// </summary>
    public V2d[] GetPointArray()
    {
        var pa = m_pointArray;
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pa[i]);
    }

    public T[] GetPointArray<T>(T[] pointArray)
    {
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pointArray[i]);
    }

    public T[] GetPointArray<T>(List<T> pointList)
    {
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pointList[i]);
    }

    #endregion
}

#endregion

#region IndexPolygon2dExtensions

public static partial class IndexPolygon2dExtensions
{
    #region Conversions

    public static Polygon2d ToPolygon2d(this IndexPolygon2d polygon)
    {
        return new Polygon2d(polygon.GetPointArray());
    }

    #endregion

    #region Geometric Properties

    /// <summary>
    /// The vertex centroid is the average of the vertex coordinates.
    /// </summary>
    public static V2d ComputeVertexCentroid(this IndexPolygon2d polygon)
    {
        var sum = V2d.Zero;
        int pc = polygon.PointCount;
        for (int i = 0; i < pc; i++) sum += polygon[i];
        double scale = 1 / (double)pc;
        return sum * scale;
    }

    public static double ComputePerimeter(this IndexPolygon2d polygon)
    {
        var pc = polygon.PointCount;
        var p0 = polygon[pc - 1];
        double r = 0;
        for (int i = 0; i < pc; i++)
        {
            var p1 = polygon[i];
            r += Vec.Distance(p0, p1);
            p0 = p1;
        }
        return r;
    }

    #endregion
}

#endregion

#region Line2d

[StructLayout(LayoutKind.Sequential)]
public partial struct Line2d : IEquatable<Line2d>, IValidity, IPolygon<V2d>, IBoundingBox2d
{
    public V2d P0, P1;

    #region Constructors

    /// <summary>
    /// Creates line from 2 points.
    /// </summary>
    public Line2d(V2d p0, V2d p1)
    {
        P0 = p0; P1 = p1;
    }

    /// <summary>
    /// Creates line from first 2 points in the sequence.
    /// </summary>
    public Line2d(IEnumerable<V2d> points)
    {
        var pa = points.TakeToArray(2);
        P0 = pa[0]; P1 = pa[1];
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    public readonly int PointCount { get { return 2; } }

    public readonly IEnumerable<V2d> Points
    {
        get { yield return P0; yield return P1; }
    }

    public readonly Line2d Reversed
    {
        get { return new Line2d(P1, P0); }
    }

    #endregion

    #region Indexer

    public V2d this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Line2d Copy(Func<V2d, V2d> point_copyFun)
    {
        return new Line2d(point_copyFun(P0), point_copyFun(P1));
    }

    public readonly Line3d ToLine3d(Func<V2d, V3d> point_copyFun)
    {
        return new Line3d(point_copyFun(P0), point_copyFun(P1));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Line2d a, Line2d b)
        => (a.P0 == b.P0) && (a.P1 == b.P1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Line2d a, Line2d b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Line2d other)
        => P0.Equals(other.P0) && P1.Equals(other.P1);

    public override readonly bool Equals(object other)
         => (other is Line2d o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", P0, P1);
    }

    public static Line2d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Line2d(V2d.Parse(x[0]), V2d.Parse(x[1]));
    }

    #endregion

    #region IBoundingBox2d Members

    public readonly Box2d BoundingBox2d
    {
        get
        {
            return new Box2d(P0, P1).Repair();
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Line2d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Line2d a, Line2d b, double tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Line2d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Line2d a, Line2d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

#region Line2dExtensions

public static partial class Line2dExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Line2d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line2d polygon, double scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Scaled(this Line2d polygon, double scale)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line2d polygon, V2d center, double scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Scaled(this Line2d polygon, V2d center, double scale)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Line2d polygon, double scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d ScaledAboutCentroid(this Line2d polygon, double scale)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line2d polygon, V2d scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Scaled(this Line2d polygon, V2d scale)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line2d polygon, V2d center, V2d scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Scaled(this Line2d polygon, V2d center, V2d scale)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line2d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Line2d polygon, V2d scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d ScaledAboutCentroid(this Line2d polygon, V2d scale)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2d polygon, M33d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Transformed(this Line2d polygon, M33d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2d polygon, Euclidean2d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Transformed(this Line2d polygon, Euclidean2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the inverse of the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2d polygon, Euclidean2d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the inverse of the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d InvTransformed(this Line2d polygon, Euclidean2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2d polygon, Similarity2d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Transformed(this Line2d polygon, Similarity2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the inverse of the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2d polygon, Similarity2d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the inverse of the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d InvTransformed(this Line2d polygon, Similarity2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the given <see cref="Affine2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2d polygon, Affine2d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the given <see cref="Affine2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Transformed(this Line2d polygon, Affine2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2d polygon, Shift2d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Transformed(this Line2d polygon, Shift2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the inverse of the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2d polygon, Shift2d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the inverse of the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d InvTransformed(this Line2d polygon, Shift2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2d polygon, Rot2d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Transformed(this Line2d polygon, Rot2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the inverse of the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2d polygon, Rot2d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the inverse of the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d InvTransformed(this Line2d polygon, Rot2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2d polygon, Scale2d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Transformed(this Line2d polygon, Scale2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the inverse of the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line2d polygon, Scale2d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the inverse of the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d InvTransformed(this Line2d polygon, Scale2d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line2d"/> by the given <see cref="M22d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line2d polygon, M22d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line2d"/> transformed by the given <see cref="M22d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line2d Transformed(this Line2d polygon, M22d t)
    {
        var result = new Line2d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V2d[] GetPointArray(this Line2d line)
    {
        var pa = new V2d[2];
        pa[0] = line.P0;
        pa[1] = line.P1;
        return pa;
    }

    public static V2d ComputeCentroid(this Line2d line)
    {
        return 0.5 * (line.P0 + line.P1);
    }
}

#endregion

#region Triangle2d

[StructLayout(LayoutKind.Sequential)]
public partial struct Triangle2d : IEquatable<Triangle2d>, IValidity, IPolygon<V2d>, IBoundingBox2d
{
    public V2d P0, P1, P2;

    #region Constructors

    /// <summary>
    /// Creates triangle from 3 points.
    /// </summary>
    public Triangle2d(V2d p0, V2d p1, V2d p2)
    {
        P0 = p0; P1 = p1; P2 = p2;
    }

    /// <summary>
    /// Creates triangle from first 3 points in the sequence.
    /// </summary>
    public Triangle2d(IEnumerable<V2d> points)
    {
        var pa = points.TakeToArray(3);
        P0 = pa[0]; P1 = pa[1]; P2 = pa[2];
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    /// <summary>
    /// Edge P1 - P0
    /// </summary>
    public readonly V2d Edge01 { get { return P1 - P0; } }
    /// <summary>
    /// Edge P2 - P0
    /// </summary>
    public readonly V2d Edge02 { get { return P2 - P0; } }
    /// <summary>
    /// Edge P2 - P1
    /// </summary>
    public readonly V2d Edge12 { get { return P2 - P1; } }
    /// <summary>
    /// Edge P0 - P1
    /// </summary>
    public readonly V2d Edge10 { get { return P0 - P1; } }
    /// <summary>
    /// Edge P0 - P2
    /// </summary>
    public readonly V2d Edge20 { get { return P0 - P2; } }
    /// <summary>
    /// Edge P1 - P2
    /// </summary>
    public readonly V2d Edge21 { get { return P1 - P2; } }

    public readonly IEnumerable<V2d> Edges
    {
        get
        {
            yield return P1 - P0;
            yield return P2 - P1;
            yield return P0 - P2;
        }
    }

    public readonly V2d[] EdgeArray
    {
        get
        {
            var a = new V2d[3];
            a[0] = P1 - P0;
            a[1] = P2 - P1;
            a[2] = P0 - P2;
            return a;
        }
    }

    public readonly IEnumerable<Line2d> EdgeLines
    {
        get
        {
            yield return new Line2d(P0, P1);
            yield return new Line2d(P1, P2);
            yield return new Line2d(P2, P0);
        }
    }

    public readonly Line2d[] EdgeLineArray
    {
        get
        {
            var a = new Line2d[3];
            a[0] = new Line2d(P0, P1);
            a[1] = new Line2d(P1, P2);
            a[2] = new Line2d(P2, P0);
            return a;
        }
    }

    public readonly Line2d GetEdgeLine(int index) => index switch
    {
        0 => new Line2d(P0, P1),
        1 => new Line2d(P1, P2),
        2 => new Line2d(P2, P0),
        _ => throw new InvalidOperationException()
    };

    public readonly V2d GetEdge(int index) => index switch
    {
        0 => P1 - P0,
        1 => P2 - P1,
        2 => P0 - P2,
        _ => throw new InvalidOperationException()
    };

    public readonly Line2d Line01 { get { return new Line2d(P0, P1); } }
    public readonly Line2d Line02 { get { return new Line2d(P0, P2); } }
    public readonly Line2d Line12 { get { return new Line2d(P1, P2); } }
    public readonly Line2d Line10 { get { return new Line2d(P1, P0); } }
    public readonly Line2d Line20 { get { return new Line2d(P2, P0); } }
    public readonly Line2d Line21 { get { return new Line2d(P2, P1); } }

    public readonly int PointCount { get { return 3; } }

    public readonly IEnumerable<V2d> Points
    {
        get { yield return P0; yield return P1; yield return P2; }
    }

    public readonly Triangle2d Reversed
    {
        get { return new Triangle2d(P2, P1, P0); }
    }

    #endregion

    #region Indexer

    public V2d this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                2 => P2,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                case 2: P2 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Triangle2d Copy(Func<V2d, V2d> point_copyFun)
    {
        return new Triangle2d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
    }

    public readonly Triangle3d ToTriangle3d(Func<V2d, V3d> point_copyFun)
    {
        return new Triangle3d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Triangle2d a, Triangle2d b)
        => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.P2 == b.P2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Triangle2d a, Triangle2d b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1, P2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Triangle2d other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2);

    public override readonly bool Equals(object other)
         => (other is Triangle2d o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, P2);
    }

    public static Triangle2d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Triangle2d(V2d.Parse(x[0]), V2d.Parse(x[1]), V2d.Parse(x[2]));
    }

    #endregion

    #region IBoundingBox2d Members

    public readonly Box2d BoundingBox2d
    {
        get
        {
            return new Box2d(P0, P1, P2);
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Triangle2d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Triangle2d a, Triangle2d b, double tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance) && ApproximateEquals(a.P2, b.P2, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Triangle2d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Triangle2d a, Triangle2d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

#region Triangle2dExtensions

public static partial class Triangle2dExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Triangle2d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle2d polygon, double scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Scaled(this Triangle2d polygon, double scale)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle2d polygon, V2d center, double scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Scaled(this Triangle2d polygon, V2d center, double scale)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Triangle2d polygon, double scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d ScaledAboutCentroid(this Triangle2d polygon, double scale)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle2d polygon, V2d scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Scaled(this Triangle2d polygon, V2d scale)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle2d polygon, V2d center, V2d scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Scaled(this Triangle2d polygon, V2d center, V2d scale)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle2d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Triangle2d polygon, V2d scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d ScaledAboutCentroid(this Triangle2d polygon, V2d scale)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2d polygon, M33d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Transformed(this Triangle2d polygon, M33d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2d polygon, Euclidean2d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Transformed(this Triangle2d polygon, Euclidean2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the inverse of the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2d polygon, Euclidean2d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the inverse of the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d InvTransformed(this Triangle2d polygon, Euclidean2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2d polygon, Similarity2d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Transformed(this Triangle2d polygon, Similarity2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the inverse of the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2d polygon, Similarity2d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the inverse of the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d InvTransformed(this Triangle2d polygon, Similarity2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the given <see cref="Affine2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2d polygon, Affine2d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the given <see cref="Affine2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Transformed(this Triangle2d polygon, Affine2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2d polygon, Shift2d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Transformed(this Triangle2d polygon, Shift2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the inverse of the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2d polygon, Shift2d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the inverse of the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d InvTransformed(this Triangle2d polygon, Shift2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2d polygon, Rot2d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Transformed(this Triangle2d polygon, Rot2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the inverse of the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2d polygon, Rot2d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the inverse of the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d InvTransformed(this Triangle2d polygon, Rot2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2d polygon, Scale2d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Transformed(this Triangle2d polygon, Scale2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the inverse of the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle2d polygon, Scale2d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the inverse of the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d InvTransformed(this Triangle2d polygon, Scale2d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle2d"/> by the given <see cref="M22d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle2d polygon, M22d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle2d"/> transformed by the given <see cref="M22d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle2d Transformed(this Triangle2d polygon, M22d t)
    {
        var result = new Triangle2d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V2d[] GetPointArray(this Triangle2d triangle)
    {
        var pa = new V2d[3];
        pa[0] = triangle.P0;
        pa[1] = triangle.P1;
        pa[2] = triangle.P2;
        return pa;
    }

    public static V2d ComputeCentroid(this Triangle2d triangle)
    {
        return Constant.OneThird * (triangle.P0 + triangle.P1 + triangle.P2);
    }
}

#endregion

#region Quad2d

[StructLayout(LayoutKind.Sequential)]
public partial struct Quad2d : IEquatable<Quad2d>, IValidity, IPolygon<V2d>, IBoundingBox2d
{
    public V2d P0, P1, P2, P3;

    #region Constructors

    /// <summary>
    /// Creates quad from 4 points.
    /// </summary>
    public Quad2d(V2d p0, V2d p1, V2d p2, V2d p3)
    {
        P0 = p0; P1 = p1; P2 = p2; P3 = p3;
    }

    /// <summary>
    /// Creates quad from first 4 points in the sequence.
    /// </summary>
    public Quad2d(IEnumerable<V2d> points)
    {
        var pa = points.TakeToArray(4);
        P0 = pa[0]; P1 = pa[1]; P2 = pa[2]; P3 = pa[3];
    }

    /// <summary>
    /// Creates quad from point and two vectors representing edges.
    /// </summary>
    public Quad2d(V2d p0, V2d edge01, V2d edge03)
    {
        P0 = p0;
        P1 = p0 + edge01;
        P2 = P1 + edge03;
        P3 = p0 + edge03;
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    /// <summary>
    /// Edge P1 - P0
    /// </summary>
    public readonly V2d Edge01 { get { return P1 - P0; } }
    /// <summary>
    /// Edge P3 - P0
    /// </summary>
    public readonly V2d Edge03 { get { return P3 - P0; } }
    /// <summary>
    /// Edge P2 - P1
    /// </summary>
    public readonly V2d Edge12 { get { return P2 - P1; } }
    /// <summary>
    /// Edge P0 - P1
    /// </summary>
    public readonly V2d Edge10 { get { return P0 - P1; } }
    /// <summary>
    /// Edge P3 - P2
    /// </summary>
    public readonly V2d Edge23 { get { return P3 - P2; } }
    /// <summary>
    /// Edge P1 - P2
    /// </summary>
    public readonly V2d Edge21 { get { return P1 - P2; } }
    /// <summary>
    /// Edge P0 - P3
    /// </summary>
    public readonly V2d Edge30 { get { return P0 - P3; } }
    /// <summary>
    /// Edge P2 - P3
    /// </summary>
    public readonly V2d Edge32 { get { return P2 - P3; } }

    public readonly IEnumerable<V2d> Edges
    {
        get
        {
            yield return P1 - P0;
            yield return P2 - P1;
            yield return P3 - P2;
            yield return P0 - P3;
        }
    }

    public readonly V2d[] EdgeArray
    {
        get
        {
            var a = new V2d[4];
            a[0] = P1 - P0;
            a[1] = P2 - P1;
            a[2] = P3 - P2;
            a[3] = P0 - P3;
            return a;
        }
    }

    public readonly IEnumerable<Line2d> EdgeLines
    {
        get
        {
            yield return new Line2d(P0, P1);
            yield return new Line2d(P1, P2);
            yield return new Line2d(P2, P3);
            yield return new Line2d(P3, P0);
        }
    }

    public readonly Line2d[] EdgeLineArray
    {
        get
        {
            var a = new Line2d[4];
            a[0] = new Line2d(P0, P1);
            a[1] = new Line2d(P1, P2);
            a[2] = new Line2d(P2, P3);
            a[3] = new Line2d(P3, P0);
            return a;
        }
    }

    public readonly Line2d GetEdgeLine(int index) => index switch
    {
        0 => new Line2d(P0, P1),
        1 => new Line2d(P1, P2),
        2 => new Line2d(P2, P3),
        3 => new Line2d(P3, P0),
        _ => throw new InvalidOperationException()
    };

    public readonly V2d GetEdge(int index) => index switch
    {
        0 => P1 - P0,
        1 => P2 - P1,
        2 => P3 - P2,
        3 => P0 - P3,
        _ => throw new InvalidOperationException()
    };

    public readonly Line2d Line01 { get { return new Line2d(P0, P1); } }
    public readonly Line2d Line03 { get { return new Line2d(P0, P3); } }
    public readonly Line2d Line12 { get { return new Line2d(P1, P2); } }
    public readonly Line2d Line10 { get { return new Line2d(P1, P0); } }
    public readonly Line2d Line23 { get { return new Line2d(P2, P3); } }
    public readonly Line2d Line21 { get { return new Line2d(P2, P1); } }
    public readonly Line2d Line30 { get { return new Line2d(P3, P0); } }
    public readonly Line2d Line32 { get { return new Line2d(P3, P2); } }

    public readonly int PointCount { get { return 4; } }

    public readonly IEnumerable<V2d> Points
    {
        get { yield return P0; yield return P1; yield return P2; yield return P3; }
    }

    public readonly Quad2d Reversed
    {
        get { return new Quad2d(P3, P2, P1, P0); }
    }

    #endregion

    #region Indexer

    public V2d this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                2 => P2,
                3 => P3,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                case 2: P2 = value; return;
                case 3: P3 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Quad2d Copy(Func<V2d, V2d> point_copyFun)
    {
        return new Quad2d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
    }

    public readonly Quad3d ToQuad3d(Func<V2d, V3d> point_copyFun)
    {
        return new Quad3d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quad2d a, Quad2d b)
        => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.P2 == b.P2) && (a.P3 == b.P3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quad2d a, Quad2d b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1, P2, P3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Quad2d other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

    public override readonly bool Equals(object other)
         => (other is Quad2d o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", P0, P1, P2, P3);
    }

    public static Quad2d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Quad2d(V2d.Parse(x[0]), V2d.Parse(x[1]), V2d.Parse(x[2]), V2d.Parse(x[3]));
    }

    #endregion

    #region IBoundingBox2d Members

    public readonly Box2d BoundingBox2d
    {
        get
        {
            return new Box2d(P0, P1, P2, P3);
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Quad2d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Quad2d a, Quad2d b, double tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance) && ApproximateEquals(a.P2, b.P2, tolerance) && ApproximateEquals(a.P3, b.P3, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Quad2d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Quad2d a, Quad2d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

#region Quad2dExtensions

public static partial class Quad2dExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Quad2d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad2d polygon, double scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
        polygon.P3 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Scaled(this Quad2d polygon, double scale)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad2d polygon, V2d center, double scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
        polygon.P3 = center + (polygon.P3 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Scaled(this Quad2d polygon, V2d center, double scale)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Quad2d polygon, double scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d ScaledAboutCentroid(this Quad2d polygon, double scale)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad2d polygon, V2d scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
        polygon.P3 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Scaled(this Quad2d polygon, V2d scale)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad2d polygon, V2d center, V2d scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
        polygon.P3 = center + (polygon.P3 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Scaled(this Quad2d polygon, V2d center, V2d scale)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad2d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Quad2d polygon, V2d scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d ScaledAboutCentroid(this Quad2d polygon, V2d scale)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2d polygon, M33d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Transformed(this Quad2d polygon, M33d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2d polygon, Euclidean2d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Transformed(this Quad2d polygon, Euclidean2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the inverse of the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2d polygon, Euclidean2d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
        polygon.P3 = t.InvTransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the inverse of the given <see cref="Euclidean2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d InvTransformed(this Quad2d polygon, Euclidean2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2d polygon, Similarity2d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Transformed(this Quad2d polygon, Similarity2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the inverse of the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2d polygon, Similarity2d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
        polygon.P3 = t.InvTransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the inverse of the given <see cref="Similarity2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d InvTransformed(this Quad2d polygon, Similarity2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the given <see cref="Affine2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2d polygon, Affine2d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the given <see cref="Affine2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Transformed(this Quad2d polygon, Affine2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2d polygon, Shift2d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Transformed(this Quad2d polygon, Shift2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the inverse of the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2d polygon, Shift2d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the inverse of the given <see cref="Shift2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d InvTransformed(this Quad2d polygon, Shift2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2d polygon, Rot2d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Transformed(this Quad2d polygon, Rot2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the inverse of the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2d polygon, Rot2d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the inverse of the given <see cref="Rot2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d InvTransformed(this Quad2d polygon, Rot2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2d polygon, Scale2d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Transformed(this Quad2d polygon, Scale2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the inverse of the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad2d polygon, Scale2d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the inverse of the given <see cref="Scale2d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d InvTransformed(this Quad2d polygon, Scale2d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad2d"/> by the given <see cref="M22d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad2d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad2d polygon, M22d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad2d"/> transformed by the given <see cref="M22d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad2d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad2d Transformed(this Quad2d polygon, M22d t)
    {
        var result = new Quad2d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V2d[] GetPointArray(this Quad2d quad)
    {
        var pa = new V2d[4];
        pa[0] = quad.P0;
        pa[1] = quad.P1;
        pa[2] = quad.P2;
        pa[3] = quad.P3;
        return pa;
    }

    public static V2d ComputeCentroid(this Quad2d quad)
    {
        return 0.25 * (quad.P0 + quad.P1 + quad.P2 + quad.P3);
    }
}

#endregion

#region Polygon3d

/// <summary>
/// A polygon internally represented by an array of points. Implemented
/// as a structure, the validity of the polygon can be checked via its
/// PointCount, which must be bigger than 0 for a polygon to hold any
/// points, and bigger than 2 for a polygon to be geometrically valid.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
public partial struct Polygon3d : IEquatable<Polygon3d>, IValidity, IPolygon<V3d>, IBoundingBox3d
{
    internal int m_pointCount;
    internal V3d[] m_pointArray;

    #region Constructors

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon3d(V3d[] pointArray, int pointCount)
    {
        if (pointArray != null)
        {
            if (pointCount <= pointArray.Length)
            {
                m_pointCount = pointCount;
                m_pointArray = pointArray;
            }
            else
                throw new ArgumentException("point count must be smaller or equal array length");
        }
        else
        {
            m_pointCount = 0;
            m_pointArray = null;
        }
    }

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon3d(params V3d[] pointArray)
    {
        m_pointCount = pointArray != null ? pointArray.Length : 0;
        m_pointArray = pointArray;
    }

    /// <summary>
    /// Creates a polygon from given points.
    /// </summary>
    public Polygon3d(V3d[] pointArray, int startIndex, int count)
    {
        if (startIndex < 0 || startIndex >= pointArray.Length - 1) throw new ArgumentOutOfRangeException(nameof(startIndex));
        if (count <= 0 || startIndex + count >= pointArray.Length) throw new ArgumentOutOfRangeException(nameof(count));
        m_pointCount = count;
        m_pointArray = new V3d[count];
        for (int i = 0; i < count; i++) m_pointArray[i] = pointArray[startIndex + i];
    }

    /// <summary>
    /// Creates a polygon from point count and point creator function.
    /// </summary>
    public Polygon3d(int pointCount, Func<int, V3d> index_pointCreator)
        : this(new V3d[pointCount].SetByIndex(index_pointCreator))
    { }

    /// <summary>
    /// Creates a polygon from a sequence of points.
    /// </summary>
    public Polygon3d(IEnumerable<V3d> points)
        : this([.. points])
    { }

    /// <summary>
    /// Creates a polygon from the points of a pointArray that
    /// are selected by an index array.
    /// </summary>
    public Polygon3d(int[] indexArray, V3d[] pointArray)
        : this(indexArray.Map(i => pointArray[i]))
    { }

    /// <summary>
    /// Creates a polygon from a triangle.
    /// </summary>
    public Polygon3d(Triangle3d triangle)
        : this(triangle.GetPointArray())
    { }

    /// <summary>
    /// Creates a polygon from a quad.
    /// </summary>
    public Polygon3d(Quad3d quad)
        : this(quad.GetPointArray())
    { }

    /// <summary>
    /// Copy constructor.
    /// Performs deep copy of original.
    /// </summary>
    public Polygon3d(Polygon3d original)
        : this(original.GetPointArray())
    { }

    #endregion

    #region Constants

    public static readonly Polygon3d Invalid = new(null, 0);

    #endregion

    #region Properties

    public readonly bool IsValid => m_pointArray != null;

    public readonly bool IsInvalid => m_pointArray == null;

    /// <summary>
    /// The number of points in the polygon. If this is 0, the polygon
    /// is invalid.
    /// </summary>
    public readonly int PointCount => m_pointCount;

    /// <summary>
    /// Enumerates points.
    /// </summary>
    public readonly IEnumerable<V3d> Points
    {
        get { for (int pi = 0; pi < m_pointCount; pi++) yield return m_pointArray[pi]; }
    }

    #endregion

    #region Conversions

    /// <summary>
    /// Returns a copy of the polygons point array.
    /// </summary>
    public readonly V3d[] GetPointArray()
    {
        var pc = m_pointCount;
        var pa = new V3d[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
        return pa;
    }

    /// <summary>
    /// [P0, P1, P2] -> [P0, P1, P2, P0].
    /// </summary>
    public readonly V3d[] GetPointArrayWithRepeatedFirstPoint()
    {
        var pc = m_pointCount;
        var pa = new V3d[pc + 1];
        for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
        pa[pc] = pa[0];
        return pa;
    }

    /// <summary>
    /// Returns a transformed copy of the polygons point array.
    /// </summary>
    public readonly T[] GetPointArray<T>(Func<V3d, T> point_copyFun)
    {
        var pc = m_pointCount;
        var pa = new T[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = point_copyFun(m_pointArray[pi]);
        return pa;
    }

    /// <summary>
    /// Returns a transformed copy of the polygons point array.
    /// </summary>
    public readonly T[] GetPointArray<T>(Func<V3d, int, T> point_index_copyFun)
    {
        var pc = m_pointCount;
        var pa = new T[pc];
        for (int pi = 0; pi < pc; pi++) pa[pi] = point_index_copyFun(m_pointArray[pi], pi);
        return pa;
    }

    #endregion

    #region Indexing

    /// <summary>
    /// Gets the index-th point of this polygon.
    /// </summary>
    public readonly V3d this[int index]
    {
        get { return m_pointArray[index]; }
        set { m_pointArray[index] = value; }
    }

    #endregion

    #region Edges and Lines

    /// <summary>
    /// Index-th edge as vector (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly V3d Edge(int index)
    {
        var p0 = m_pointArray[index++];
        var p1 = m_pointArray[index < m_pointCount ? index : 0];
        return p1 - p0;
    }

    /// <summary>
    /// Edges as vectors (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly IEnumerable<V3d> Edges
    {
        get
        {
            var pc = m_pointCount;
            var p = m_pointArray[0];
            var p0 = p;
            for (int pi = 1; pi < pc; pi++)
            {
                var p1 = m_pointArray[pi];
                yield return p1 - p0;
                p0 = p1;
            }
            yield return p - p0;
        }
    }

    /// <summary>
    /// Index-th edge as line segment (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly Line3d EdgeLine(int index)
    {
        var p0 = m_pointArray[index++];
        var p1 = m_pointArray[index < m_pointCount ? index : 0];
        return new Line3d(p0, p1);
    }

    /// <summary>
    /// Edges as line segments (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly IEnumerable<Line3d> EdgeLines
    {
        get
        {
            var pc = m_pointCount;
            if (pc < 1) yield break;
            var p0 = m_pointArray[0];
            var p = p0;
            for (int pi = 1; pi < pc; pi++)
            {
                var p1 = m_pointArray[pi];
                yield return new Line3d(p0, p1);
                p0 = p1;
            }
            yield return new Line3d(p0, p);
        }
    }

    /// <summary>
    /// Edges as vectors (edgeEndPos - edgeBeginPos).
    /// </summary>
    public readonly V3d[] GetEdgeArray()
    {
        var pc = m_pointCount;
        if (pc < 2) return [];
        var edgeArray = new V3d[pc];
        var p = m_pointArray[0];
        var p0 = p;
        for (int pi = 1; pi < pc; pi++)
        {
            var p1 = m_pointArray[pi];
            edgeArray[pi - 1] = p1 - p0;
            p0 = p1;
        }
        edgeArray[pc - 1] = p - p0;
        return edgeArray;
    }

    /// <summary>
    /// Edges as line segments (edgeBeginPos, edgeEndPos).
    /// </summary>
    public readonly Line3d[] GetEdgeLineArray()
    {
        var pc = PointCount;
        if (pc < 2) return [];
        var ela = new Line3d[pc];
        var p = m_pointArray[0];
        var p0 = p;
        for (int pi = 1; pi < pc; pi++)
        {
            var p1 = m_pointArray[pi];
            ela[pi - 1] = new Line3d(p0, p1);
            p0 = p1;
        }
        ela[pc - 1] = new Line3d(p0, p);
        return ela;
    }

    #endregion

    #region Transformations

    /// <summary>
    /// Returns copy of polygon. Same as Map(p => p).
    /// </summary>
    public readonly Polygon3d Copy()
    {
        return new Polygon3d(m_pointArray.Copy());
    }

    /// <summary>
    /// Returns transformed copy of this polygon.
    /// </summary>
    public readonly Polygon3d Map(Func<V3d, V3d> point_fun)
    {
        var pc = m_pointCount;
        V3d[] opa = m_pointArray, npa = new V3d[pc];
        for (int pi = 0; pi < pc; pi++) npa[pi] = point_fun(opa[pi]);
        return new Polygon3d(npa, pc);
    }

    /// <summary>
    /// Gets copy with reversed order of vertices.
    /// </summary>
    public readonly Polygon3d Reversed
    {
        get
        {
            var pc = m_pointCount;
            V3d[] opa = m_pointArray, npa = new V3d[pc];
            for (int pi = 0, pj = pc - 1; pi < pc; pi++, pj--) npa[pi] = opa[pj];
            return new Polygon3d(npa, pc);
        }
    }

    /// <summary>
    /// Reverses order of vertices in-place.
    /// </summary>
    public readonly void Reverse()
    {
        var pa = m_pointArray;
        for (int pi = 0, pj = m_pointCount - 1; pi < pj; pi++, pj--)
        {
            (pa[pj], pa[pi]) = (pa[pi], pa[pj]);
        }
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Polygon3d a, Polygon3d b)
    {
        if (a.m_pointCount != b.m_pointCount) return false;
        for (int pi = 0; pi < a.m_pointCount; pi++)
            if (a.m_pointArray[pi] != b.m_pointArray[pi]) return false;
        return true;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Polygon3d a, Polygon3d b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return m_pointArray.GetCombinedHashCode(m_pointCount);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Polygon3d other)
    {
        if (m_pointCount != other.m_pointCount) return false;
        for (int pi = 0; pi < m_pointCount; pi++)
            if (!m_pointArray[pi].Equals(other.m_pointArray[pi])) return false;
        return true;
    }

    public override readonly bool Equals(object other)
        => (other is Polygon3d o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture,
            "[{0}]", Points.Select(x => x.ToString()).Join(", ")
            );
    }

    public static Polygon3d Parse(string s)
    {
        var va = s.NestedBracketSplitLevelOne().ToArray();
        return new Polygon3d(va.Select(x => V3d.Parse(x)));
    }

    #endregion

    #region IBoundingBox3d Members

    /// <summary>
    /// Bounding box of polygon.
    /// </summary>
    public readonly Box3d BoundingBox3d
    {
        get { return new Box3d(m_pointArray, 0, m_pointCount); }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Polygon3d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Polygon3d a, Polygon3d b, double tolerance)
    {
        if (a.m_pointCount != b.m_pointCount) return false;
        for (int pi = 0; pi < a.m_pointCount; pi++)
            if (!ApproximateEquals(a.m_pointArray[pi], b.m_pointArray[pi], tolerance)) return false;
        return true;
    }

    /// <summary>
    /// Returns whether the given <see cref="Polygon3d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Polygon3d a, Polygon3d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

#region Polygon3dExtensions

public static partial class Polygon3dExtensions
{
    #region Geometric Properties

    /// <summary>
    /// The vertex centroid is the average of the vertex coordinates.
    /// </summary>
    public static V3d ComputeVertexCentroid(this Polygon3d polygon)
    {
        var sum = V3d.Zero;
        int pc = polygon.PointCount;
        for (int i = 0; i < pc; i++) sum += polygon[i];
        double scale = 1 / (double)pc;
        return sum * scale;
    }

    public static double ComputePerimeter(this Polygon3d polygon)
    {
        var pc = polygon.PointCount;
        var p0 = polygon[pc - 1];
        double r = 0;
        for (int i = 0; i < pc; i++)
        {
            var p1 = polygon[i];
            r += Vec.Distance(p0, p1);
            p0 = p1;
        }
        return r;
    }

    #endregion

    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Polygon3d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon3d polygon, double scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Scaled(this Polygon3d polygon, double scale)
    {
        var result = new Polygon3d(polygon);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon3d polygon, V3d center, double scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = center + (polygon.m_pointArray[pi] - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Scaled(this Polygon3d polygon, V3d center, double scale)
    {
        var result = new Polygon3d(polygon);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutVertexCentroid(this ref Polygon3d polygon, double scale)
    {
        var center = polygon.ComputeVertexCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d ScaledAboutVertexCentroid(this Polygon3d polygon, double scale)
    {
        var result = new Polygon3d(polygon);
        result.ScaleAboutVertexCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon3d polygon, V3d scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Scaled(this Polygon3d polygon, V3d scale)
    {
        var result = new Polygon3d(polygon);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    public static void Scale(this ref Polygon3d polygon, V3d center, V3d scale)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = center + (polygon.m_pointArray[pi] - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Scaled(this Polygon3d polygon, V3d center, V3d scale)
    {
        var result = new Polygon3d(polygon);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Polygon3d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutVertexCentroid(this ref Polygon3d polygon, V3d scale)
    {
        var center = polygon.ComputeVertexCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d ScaledAboutVertexCentroid(this Polygon3d polygon, V3d scale)
    {
        var result = new Polygon3d(polygon);
        result.ScaleAboutVertexCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the given <see cref="M44d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3d polygon, M44d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the given <see cref="M44d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Transformed(this Polygon3d polygon, M44d t)
    {
        var result = new Polygon3d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3d polygon, Euclidean3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Transformed(this Polygon3d polygon, Euclidean3d t)
    {
        var result = new Polygon3d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the inverse of the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3d polygon, Euclidean3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the inverse of the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d InvTransformed(this Polygon3d polygon, Euclidean3d t)
    {
        var result = new Polygon3d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3d polygon, Similarity3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Transformed(this Polygon3d polygon, Similarity3d t)
    {
        var result = new Polygon3d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the inverse of the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3d polygon, Similarity3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the inverse of the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d InvTransformed(this Polygon3d polygon, Similarity3d t)
    {
        var result = new Polygon3d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the given <see cref="Affine3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3d polygon, Affine3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.TransformPos(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the given <see cref="Affine3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Transformed(this Polygon3d polygon, Affine3d t)
    {
        var result = new Polygon3d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3d polygon, Shift3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Transformed(this Polygon3d polygon, Shift3d t)
    {
        var result = new Polygon3d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the inverse of the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3d polygon, Shift3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the inverse of the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d InvTransformed(this Polygon3d polygon, Shift3d t)
    {
        var result = new Polygon3d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3d polygon, Rot3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Transformed(this Polygon3d polygon, Rot3d t)
    {
        var result = new Polygon3d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the inverse of the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3d polygon, Rot3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the inverse of the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d InvTransformed(this Polygon3d polygon, Rot3d t)
    {
        var result = new Polygon3d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3d polygon, Scale3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Transformed(this Polygon3d polygon, Scale3d t)
    {
        var result = new Polygon3d(polygon);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the inverse of the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void InvTransform(this ref Polygon3d polygon, Scale3d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.InvTransform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the inverse of the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d InvTransformed(this Polygon3d polygon, Scale3d t)
    {
        var result = new Polygon3d(polygon);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Polygon3d"/> by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Polygon3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    public static void Transform(this ref Polygon3d polygon, M33d t)
    {
        for (int pi = 0; pi < polygon.m_pointCount; pi++)
            polygon.m_pointArray[pi] = t.Transform(polygon.m_pointArray[pi]);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Polygon3d"/> transformed by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Polygon3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Polygon3d Transformed(this Polygon3d polygon, M33d t)
    {
        var result = new Polygon3d(polygon);
        result.Transform(t);
        return result;
    }

    public static Polygon3d WithoutMultiplePoints(this Polygon3d polygon, double eps = 1e-8)
    {
        eps *= eps;
        var opc = polygon.PointCount;
        var pa = new V3d[opc];
        var pc = 0;
        pa[0] = polygon[0];
        for (int pi = 1; pi < opc; pi++)
            if (Vec.DistanceSquared(pa[pc], polygon[pi]) > eps)
                pa[++pc] = polygon[pi];
        if (Vec.DistanceSquared(pa[pc], polygon[0]) > eps)
            ++pc;
        return new Polygon3d(pa, pc);
    }

    #endregion

    #region Clipping

    /// <summary>
    /// Clip the supplied polygon at the supplied plane. The method should
    /// work with all non-selfintersecting polygons. Returns all parts of
    /// the polygon that are at the positive side of the plane.
    /// </summary>
    public static Polygon3d ConvexClipped(
            this Polygon3d polygon, Plane3d plane, double eps = 1e-8)
    {
        var opc = polygon.PointCount;
        V3d[] pa = new V3d[opc + 1];
        var pc = 0;
        var pf = polygon[0];
        var hf = plane.Height(pf); bool hfp = hf > eps, hfn = hf < -eps;
        if (hf >= -eps) pa[pc++] = pf;
        var p0 = pf; var h0 = hf; var h0p = hfp; var h0n = hfn;
        for (int vi = 1; vi < opc; vi++)
        {
            var p1 = polygon[vi];
            var h1 = plane.Height(p1); bool h1p = h1 > eps, h1n = h1 < -eps;
            if (h0p && h1n || h0n && h1p) pa[pc++] = p0 + (p1 - p0) * (h0 / (h0 - h1));
            if (h1 >= -eps) pa[pc++] = p1;
            p0 = p1; h0 = h1; h0p = h1p; h0n = h1n;
        }
        if (h0p && hfn || h0n && hfp) pa[pc++] = p0 + (pf - p0) * (h0 / (h0 - hf));
        return new Polygon3d(pa, pc);
    }

    /// <summary>
    /// Returns the convex polygon clipped by the set of planes (defined
    /// as Plane3ds), i.e. all parts of the polygon that are at the positive
    /// side of the planes.
    /// </summary>
    public static Polygon3d ConvexClipped(
            this Polygon3d polygon, Plane3d[] planes, double eps = 1e-8)
    {
        foreach (var c in planes)
        {
            polygon = polygon.ConvexClipped(c, eps);
            if (polygon.PointCount == 0) break;
        }
        return polygon;
    }

    /// <summary>
    /// Returns the polygon clipped by the hull, i.e. all parts of the
    /// polygon that are at the positive side of the hull planes.
    /// </summary>
    public static Polygon3d ConvexClipped(
            this Polygon3d polygon, Hull3d hull, double eps = 1e-8)
    {
        return polygon.ConvexClipped(hull.PlaneArray, eps);
    }

    /// <summary>
    /// TODO summary.
    /// </summary>
    public static Polygon3d ConvexClipped(
#pragma warning disable IDE0060 // Remove unused parameter
            this Polygon3d polygon, Box3d box, double eps = 1e-8)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        var planes = new[]
        {
            new Plane3d(V3d.XAxis, box.Min), new Plane3d(-V3d.XAxis, box.Max),
            new Plane3d(V3d.YAxis, box.Min), new Plane3d(-V3d.YAxis, box.Max),
            new Plane3d(V3d.ZAxis, box.Min), new Plane3d(-V3d.ZAxis, box.Max),
        };
        return polygon.ConvexClipped(planes);
    }

    #endregion
}

#endregion

#region IndexPolygon3d

[StructLayout(LayoutKind.Sequential)]
public readonly partial struct IndexPolygon3d : IValidity, IPolygon<V3d>
{
    private readonly int m_pointCount;
    private readonly int m_firstIndex;
    private readonly int[] m_indexArray;
    private readonly V3d[] m_pointArray;

    #region Constructors

    public IndexPolygon3d(int[] indexArray, int firstIndex, int pointCount, V3d[] pointArray)
    {
        m_indexArray = indexArray;
        m_firstIndex = firstIndex;
        m_pointCount = pointCount;
        m_pointArray = pointArray;
    }

    public IndexPolygon3d(V3d[] pointArray, int firstIndex, int pointCount)
        : this(new int[pointCount].SetByIndex(i => firstIndex + i), 0, pointCount, pointArray)
    { }

    public IndexPolygon3d(V3d[] pointArray)
        : this(new int[pointArray.Length].SetByIndex(i => i), 0, pointArray.Length, pointArray)
    { }

    #endregion

    #region Constants

    public static readonly IndexPolygon3d Invalid = new(null, 0, 0, null);

    #endregion

    #region Properties

    public bool IsValid { get { return m_indexArray != null && m_pointArray != null; } }
    public bool IsInvalid { get { return m_indexArray == null || m_pointArray == null; } }

    public int PointCount
    {
        get { return m_pointCount; }
    }

    public int FirstIndex { get { return m_firstIndex; } }

    /// <summary>
    /// The index array that contains the point indices of the
    /// index polygon at the index range [FirstIndex, FirstIndex + PointCount).
    /// NOTE: This is different from the array returned by GetIndexArray().
    /// </summary>
    public int[] IndexArray { get { return m_indexArray; } }

    /// <summary>
    /// The point array that contains the points referenced by
    /// the index array. Note: This is different from the array
    /// returned by GetPointArray().
    /// </summary>
    public V3d[] PointArray { get { return m_pointArray; } }

    public IEnumerable<V3d> Points
    {
        get
        {
            for (int i = 0; i < m_pointCount; i++)
                yield return m_pointArray[m_indexArray[m_firstIndex + i]];
        }
    }

    public IEnumerable<int> Indices
    {
        get
        {
            for (int i = 0; i < m_pointCount; i++)
                yield return m_indexArray[m_firstIndex + i];
        }
    }

    #endregion

    #region Indexing

    public V3d this[int index]
    {
        get { return m_pointArray[m_indexArray[m_firstIndex + index]]; }
        set { m_pointArray[m_indexArray[m_firstIndex + index]] = value; }
    }

    #endregion

    #region Conversions

    public void ForEachIndex(Action<int> index_act)
    {
        var ia = m_indexArray; int fi = m_firstIndex;
        for (int i = 0; i < m_pointCount; i++) index_act(ia[fi + i]);
    }

    /// <summary>
    /// Returns a newly created array containing only the actual indices
    /// of the index polygon. NOTE: This is different from the
    /// IndexArray property!
    /// </summary>
    public int[] GetIndexArray()
    {
        return m_indexArray.Copy(m_firstIndex, m_pointCount);
    }

    /// <summary>
    /// Returns a newly created array containing only the actual points
    /// of the index polygon. NOTE: This is different from the
    /// PointArray property!
    /// </summary>
    public V3d[] GetPointArray()
    {
        var pa = m_pointArray;
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pa[i]);
    }

    public T[] GetPointArray<T>(T[] pointArray)
    {
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pointArray[i]);
    }

    public T[] GetPointArray<T>(List<T> pointList)
    {
        return m_indexArray.Map(m_firstIndex, m_pointCount, i => pointList[i]);
    }

    #endregion
}

#endregion

#region IndexPolygon3dExtensions

public static partial class IndexPolygon3dExtensions
{
    #region Conversions

    public static Polygon3d ToPolygon3d(this IndexPolygon3d polygon)
    {
        return new Polygon3d(polygon.GetPointArray());
    }

    #endregion

    #region Geometric Properties

    /// <summary>
    /// The vertex centroid is the average of the vertex coordinates.
    /// </summary>
    public static V3d ComputeVertexCentroid(this IndexPolygon3d polygon)
    {
        var sum = V3d.Zero;
        int pc = polygon.PointCount;
        for (int i = 0; i < pc; i++) sum += polygon[i];
        double scale = 1 / (double)pc;
        return sum * scale;
    }

    public static double ComputePerimeter(this IndexPolygon3d polygon)
    {
        var pc = polygon.PointCount;
        var p0 = polygon[pc - 1];
        double r = 0;
        for (int i = 0; i < pc; i++)
        {
            var p1 = polygon[i];
            r += Vec.Distance(p0, p1);
            p0 = p1;
        }
        return r;
    }

    #endregion
}

#endregion

#region Line3d

[StructLayout(LayoutKind.Sequential)]
public partial struct Line3d : IEquatable<Line3d>, IValidity, IPolygon<V3d>, IBoundingBox3d
{
    public V3d P0, P1;

    #region Constructors

    /// <summary>
    /// Creates line from 2 points.
    /// </summary>
    public Line3d(V3d p0, V3d p1)
    {
        P0 = p0; P1 = p1;
    }

    /// <summary>
    /// Creates line from first 2 points in the sequence.
    /// </summary>
    public Line3d(IEnumerable<V3d> points)
    {
        var pa = points.TakeToArray(2);
        P0 = pa[0]; P1 = pa[1];
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    public readonly int PointCount { get { return 2; } }

    public readonly IEnumerable<V3d> Points
    {
        get { yield return P0; yield return P1; }
    }

    public readonly Line3d Reversed
    {
        get { return new Line3d(P1, P0); }
    }

    #endregion

    #region Indexer

    public V3d this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Line3d Copy(Func<V3d, V3d> point_copyFun)
    {
        return new Line3d(point_copyFun(P0), point_copyFun(P1));
    }

    public readonly Line2d ToLine2d(Func<V3d, V2d> point_copyFun)
    {
        return new Line2d(point_copyFun(P0), point_copyFun(P1));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Line3d a, Line3d b)
        => (a.P0 == b.P0) && (a.P1 == b.P1);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Line3d a, Line3d b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Line3d other)
        => P0.Equals(other.P0) && P1.Equals(other.P1);

    public override readonly bool Equals(object other)
         => (other is Line3d o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}]", P0, P1);
    }

    public static Line3d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Line3d(V3d.Parse(x[0]), V3d.Parse(x[1]));
    }

    #endregion

    #region IBoundingBox3d Members

    public readonly Box3d BoundingBox3d
    {
        get
        {
            return new Box3d(P0, P1).Repair();
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Line3d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Line3d a, Line3d b, double tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Line3d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Line3d a, Line3d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

#region Line3dExtensions

public static partial class Line3dExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Line3d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line3d polygon, double scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Scaled(this Line3d polygon, double scale)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line3d polygon, V3d center, double scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Scaled(this Line3d polygon, V3d center, double scale)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Line3d polygon, double scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d ScaledAboutCentroid(this Line3d polygon, double scale)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line3d polygon, V3d scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Scaled(this Line3d polygon, V3d scale)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Line3d polygon, V3d center, V3d scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Scaled(this Line3d polygon, V3d center, V3d scale)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Line3d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Line3d polygon, V3d scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d ScaledAboutCentroid(this Line3d polygon, V3d scale)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the given <see cref="M44d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3d polygon, M44d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the given <see cref="M44d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Transformed(this Line3d polygon, M44d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3d polygon, Euclidean3d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Transformed(this Line3d polygon, Euclidean3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the inverse of the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3d polygon, Euclidean3d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the inverse of the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d InvTransformed(this Line3d polygon, Euclidean3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3d polygon, Similarity3d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Transformed(this Line3d polygon, Similarity3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the inverse of the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3d polygon, Similarity3d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the inverse of the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d InvTransformed(this Line3d polygon, Similarity3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the given <see cref="Affine3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3d polygon, Affine3d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the given <see cref="Affine3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Transformed(this Line3d polygon, Affine3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3d polygon, Shift3d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Transformed(this Line3d polygon, Shift3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the inverse of the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3d polygon, Shift3d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the inverse of the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d InvTransformed(this Line3d polygon, Shift3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3d polygon, Rot3d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Transformed(this Line3d polygon, Rot3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the inverse of the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3d polygon, Rot3d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the inverse of the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d InvTransformed(this Line3d polygon, Rot3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3d polygon, Scale3d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Transformed(this Line3d polygon, Scale3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the inverse of the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Line3d polygon, Scale3d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the inverse of the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d InvTransformed(this Line3d polygon, Scale3d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Line3d"/> by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Line3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Line3d polygon, M33d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Line3d"/> transformed by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Line3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Line3d Transformed(this Line3d polygon, M33d t)
    {
        var result = new Line3d(polygon.P0, polygon.P1);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V3d[] GetPointArray(this Line3d line)
    {
        var pa = new V3d[2];
        pa[0] = line.P0;
        pa[1] = line.P1;
        return pa;
    }

    public static V3d ComputeCentroid(this Line3d line)
    {
        return 0.5 * (line.P0 + line.P1);
    }
}

#endregion

#region Triangle3d

[StructLayout(LayoutKind.Sequential)]
public partial struct Triangle3d : IEquatable<Triangle3d>, IValidity, IPolygon<V3d>, IBoundingBox3d
{
    public V3d P0, P1, P2;

    #region Constructors

    /// <summary>
    /// Creates triangle from 3 points.
    /// </summary>
    public Triangle3d(V3d p0, V3d p1, V3d p2)
    {
        P0 = p0; P1 = p1; P2 = p2;
    }

    /// <summary>
    /// Creates triangle from first 3 points in the sequence.
    /// </summary>
    public Triangle3d(IEnumerable<V3d> points)
    {
        var pa = points.TakeToArray(3);
        P0 = pa[0]; P1 = pa[1]; P2 = pa[2];
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    /// <summary>
    /// Edge P1 - P0
    /// </summary>
    public readonly V3d Edge01 { get { return P1 - P0; } }
    /// <summary>
    /// Edge P2 - P0
    /// </summary>
    public readonly V3d Edge02 { get { return P2 - P0; } }
    /// <summary>
    /// Edge P2 - P1
    /// </summary>
    public readonly V3d Edge12 { get { return P2 - P1; } }
    /// <summary>
    /// Edge P0 - P1
    /// </summary>
    public readonly V3d Edge10 { get { return P0 - P1; } }
    /// <summary>
    /// Edge P0 - P2
    /// </summary>
    public readonly V3d Edge20 { get { return P0 - P2; } }
    /// <summary>
    /// Edge P1 - P2
    /// </summary>
    public readonly V3d Edge21 { get { return P1 - P2; } }

    public readonly IEnumerable<V3d> Edges
    {
        get
        {
            yield return P1 - P0;
            yield return P2 - P1;
            yield return P0 - P2;
        }
    }

    public readonly V3d[] EdgeArray
    {
        get
        {
            var a = new V3d[3];
            a[0] = P1 - P0;
            a[1] = P2 - P1;
            a[2] = P0 - P2;
            return a;
        }
    }

    public readonly IEnumerable<Line3d> EdgeLines
    {
        get
        {
            yield return new Line3d(P0, P1);
            yield return new Line3d(P1, P2);
            yield return new Line3d(P2, P0);
        }
    }

    public readonly Line3d[] EdgeLineArray
    {
        get
        {
            var a = new Line3d[3];
            a[0] = new Line3d(P0, P1);
            a[1] = new Line3d(P1, P2);
            a[2] = new Line3d(P2, P0);
            return a;
        }
    }

    public readonly Line3d GetEdgeLine(int index) => index switch
    {
        0 => new Line3d(P0, P1),
        1 => new Line3d(P1, P2),
        2 => new Line3d(P2, P0),
        _ => throw new InvalidOperationException()
    };

    public readonly V3d GetEdge(int index) => index switch
    {
        0 => P1 - P0,
        1 => P2 - P1,
        2 => P0 - P2,
        _ => throw new InvalidOperationException()
    };

    public readonly Line3d Line01 { get { return new Line3d(P0, P1); } }
    public readonly Line3d Line02 { get { return new Line3d(P0, P2); } }
    public readonly Line3d Line12 { get { return new Line3d(P1, P2); } }
    public readonly Line3d Line10 { get { return new Line3d(P1, P0); } }
    public readonly Line3d Line20 { get { return new Line3d(P2, P0); } }
    public readonly Line3d Line21 { get { return new Line3d(P2, P1); } }

    public readonly int PointCount { get { return 3; } }

    public readonly IEnumerable<V3d> Points
    {
        get { yield return P0; yield return P1; yield return P2; }
    }

    public readonly Triangle3d Reversed
    {
        get { return new Triangle3d(P2, P1, P0); }
    }

    #endregion

    #region Indexer

    public V3d this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                2 => P2,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                case 2: P2 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Triangle3d Copy(Func<V3d, V3d> point_copyFun)
    {
        return new Triangle3d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
    }

    public readonly Triangle2d ToTriangle2d(Func<V3d, V2d> point_copyFun)
    {
        return new Triangle2d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Triangle3d a, Triangle3d b)
        => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.P2 == b.P2);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Triangle3d a, Triangle3d b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1, P2);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Triangle3d other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2);

    public override readonly bool Equals(object other)
         => (other is Triangle3d o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", P0, P1, P2);
    }

    public static Triangle3d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Triangle3d(V3d.Parse(x[0]), V3d.Parse(x[1]), V3d.Parse(x[2]));
    }

    #endregion

    #region IBoundingBox3d Members

    public readonly Box3d BoundingBox3d
    {
        get
        {
            return new Box3d(P0, P1, P2);
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Triangle3d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Triangle3d a, Triangle3d b, double tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance) && ApproximateEquals(a.P2, b.P2, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Triangle3d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Triangle3d a, Triangle3d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

#region Triangle3dExtensions

public static partial class Triangle3dExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Triangle3d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle3d polygon, double scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Scaled(this Triangle3d polygon, double scale)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle3d polygon, V3d center, double scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Scaled(this Triangle3d polygon, V3d center, double scale)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Triangle3d polygon, double scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d ScaledAboutCentroid(this Triangle3d polygon, double scale)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle3d polygon, V3d scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Scaled(this Triangle3d polygon, V3d scale)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Triangle3d polygon, V3d center, V3d scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Scaled(this Triangle3d polygon, V3d center, V3d scale)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Triangle3d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Triangle3d polygon, V3d scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d ScaledAboutCentroid(this Triangle3d polygon, V3d scale)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the given <see cref="M44d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3d polygon, M44d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the given <see cref="M44d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Transformed(this Triangle3d polygon, M44d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3d polygon, Euclidean3d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Transformed(this Triangle3d polygon, Euclidean3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the inverse of the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3d polygon, Euclidean3d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the inverse of the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d InvTransformed(this Triangle3d polygon, Euclidean3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3d polygon, Similarity3d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Transformed(this Triangle3d polygon, Similarity3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the inverse of the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3d polygon, Similarity3d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the inverse of the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d InvTransformed(this Triangle3d polygon, Similarity3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the given <see cref="Affine3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3d polygon, Affine3d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the given <see cref="Affine3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Transformed(this Triangle3d polygon, Affine3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3d polygon, Shift3d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Transformed(this Triangle3d polygon, Shift3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the inverse of the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3d polygon, Shift3d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the inverse of the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d InvTransformed(this Triangle3d polygon, Shift3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3d polygon, Rot3d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Transformed(this Triangle3d polygon, Rot3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the inverse of the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3d polygon, Rot3d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the inverse of the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d InvTransformed(this Triangle3d polygon, Rot3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3d polygon, Scale3d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Transformed(this Triangle3d polygon, Scale3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the inverse of the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Triangle3d polygon, Scale3d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the inverse of the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d InvTransformed(this Triangle3d polygon, Scale3d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Triangle3d"/> by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Triangle3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Triangle3d polygon, M33d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Triangle3d"/> transformed by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Triangle3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Triangle3d Transformed(this Triangle3d polygon, M33d t)
    {
        var result = new Triangle3d(polygon.P0, polygon.P1, polygon.P2);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V3d[] GetPointArray(this Triangle3d triangle)
    {
        var pa = new V3d[3];
        pa[0] = triangle.P0;
        pa[1] = triangle.P1;
        pa[2] = triangle.P2;
        return pa;
    }

    public static V3d ComputeCentroid(this Triangle3d triangle)
    {
        return Constant.OneThird * (triangle.P0 + triangle.P1 + triangle.P2);
    }
}

#endregion

#region Quad3d

[StructLayout(LayoutKind.Sequential)]
public partial struct Quad3d : IEquatable<Quad3d>, IValidity, IPolygon<V3d>, IBoundingBox3d
{
    public V3d P0, P1, P2, P3;

    #region Constructors

    /// <summary>
    /// Creates quad from 4 points.
    /// </summary>
    public Quad3d(V3d p0, V3d p1, V3d p2, V3d p3)
    {
        P0 = p0; P1 = p1; P2 = p2; P3 = p3;
    }

    /// <summary>
    /// Creates quad from first 4 points in the sequence.
    /// </summary>
    public Quad3d(IEnumerable<V3d> points)
    {
        var pa = points.TakeToArray(4);
        P0 = pa[0]; P1 = pa[1]; P2 = pa[2]; P3 = pa[3];
    }

    /// <summary>
    /// Creates quad from point and two vectors representing edges.
    /// </summary>
    public Quad3d(V3d p0, V3d edge01, V3d edge03)
    {
        P0 = p0;
        P1 = p0 + edge01;
        P2 = P1 + edge03;
        P3 = p0 + edge03;
    }

    #endregion

    #region Properties

    public readonly bool IsValid { get { return true; } }
    public readonly bool IsInvalid { get { return false; } }

    /// <summary>
    /// Edge P1 - P0
    /// </summary>
    public readonly V3d Edge01 { get { return P1 - P0; } }
    /// <summary>
    /// Edge P3 - P0
    /// </summary>
    public readonly V3d Edge03 { get { return P3 - P0; } }
    /// <summary>
    /// Edge P2 - P1
    /// </summary>
    public readonly V3d Edge12 { get { return P2 - P1; } }
    /// <summary>
    /// Edge P0 - P1
    /// </summary>
    public readonly V3d Edge10 { get { return P0 - P1; } }
    /// <summary>
    /// Edge P3 - P2
    /// </summary>
    public readonly V3d Edge23 { get { return P3 - P2; } }
    /// <summary>
    /// Edge P1 - P2
    /// </summary>
    public readonly V3d Edge21 { get { return P1 - P2; } }
    /// <summary>
    /// Edge P0 - P3
    /// </summary>
    public readonly V3d Edge30 { get { return P0 - P3; } }
    /// <summary>
    /// Edge P2 - P3
    /// </summary>
    public readonly V3d Edge32 { get { return P2 - P3; } }

    public readonly IEnumerable<V3d> Edges
    {
        get
        {
            yield return P1 - P0;
            yield return P2 - P1;
            yield return P3 - P2;
            yield return P0 - P3;
        }
    }

    public readonly V3d[] EdgeArray
    {
        get
        {
            var a = new V3d[4];
            a[0] = P1 - P0;
            a[1] = P2 - P1;
            a[2] = P3 - P2;
            a[3] = P0 - P3;
            return a;
        }
    }

    public readonly IEnumerable<Line3d> EdgeLines
    {
        get
        {
            yield return new Line3d(P0, P1);
            yield return new Line3d(P1, P2);
            yield return new Line3d(P2, P3);
            yield return new Line3d(P3, P0);
        }
    }

    public readonly Line3d[] EdgeLineArray
    {
        get
        {
            var a = new Line3d[4];
            a[0] = new Line3d(P0, P1);
            a[1] = new Line3d(P1, P2);
            a[2] = new Line3d(P2, P3);
            a[3] = new Line3d(P3, P0);
            return a;
        }
    }

    public readonly Line3d GetEdgeLine(int index) => index switch
    {
        0 => new Line3d(P0, P1),
        1 => new Line3d(P1, P2),
        2 => new Line3d(P2, P3),
        3 => new Line3d(P3, P0),
        _ => throw new InvalidOperationException()
    };

    public readonly V3d GetEdge(int index) => index switch
    {
        0 => P1 - P0,
        1 => P2 - P1,
        2 => P3 - P2,
        3 => P0 - P3,
        _ => throw new InvalidOperationException()
    };

    public readonly Line3d Line01 { get { return new Line3d(P0, P1); } }
    public readonly Line3d Line03 { get { return new Line3d(P0, P3); } }
    public readonly Line3d Line12 { get { return new Line3d(P1, P2); } }
    public readonly Line3d Line10 { get { return new Line3d(P1, P0); } }
    public readonly Line3d Line23 { get { return new Line3d(P2, P3); } }
    public readonly Line3d Line21 { get { return new Line3d(P2, P1); } }
    public readonly Line3d Line30 { get { return new Line3d(P3, P0); } }
    public readonly Line3d Line32 { get { return new Line3d(P3, P2); } }

    public readonly int PointCount { get { return 4; } }

    public readonly IEnumerable<V3d> Points
    {
        get { yield return P0; yield return P1; yield return P2; yield return P3; }
    }

    public readonly Quad3d Reversed
    {
        get { return new Quad3d(P3, P2, P1, P0); }
    }

    #endregion

    #region Indexer

    public V3d this[int index]
    {
        readonly get
        {
            return index switch
            {
                0 => P0,
                1 => P1,
                2 => P2,
                3 => P3,
                _ => throw new IndexOutOfRangeException()
            };
        }
        set
        {
            switch (index)
            {
                case 0: P0 = value; return;
                case 1: P1 = value; return;
                case 2: P2 = value; return;
                case 3: P3 = value; return;
                default: throw new IndexOutOfRangeException();
            }
        }
    }

    #endregion

    #region Transformations

    public readonly Quad3d Copy(Func<V3d, V3d> point_copyFun)
    {
        return new Quad3d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
    }

    public readonly Quad2d ToQuad2d(Func<V3d, V2d> point_copyFun)
    {
        return new Quad2d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
    }

    #endregion

    #region Comparisons

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator ==(Quad3d a, Quad3d b)
        => (a.P0 == b.P0) && (a.P1 == b.P1) && (a.P2 == b.P2) && (a.P3 == b.P3);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool operator !=(Quad3d a, Quad3d b)
        => !(a == b);

    #endregion

    #region Overrides

    public override readonly int GetHashCode()
    {
        return HashCode.GetCombined(P0, P1, P2, P3);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public readonly bool Equals(Quad3d other)
        => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

    public override readonly bool Equals(object other)
         => (other is Quad3d o) && Equals(o);

    public override readonly string ToString()
    {
        return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", P0, P1, P2, P3);
    }

    public static Quad3d Parse(string s)
    {
        var x = s.NestedBracketSplitLevelOne().ToArray();
        return new Quad3d(V3d.Parse(x[0]), V3d.Parse(x[1]), V3d.Parse(x[2]), V3d.Parse(x[3]));
    }

    #endregion

    #region IBoundingBox3d Members

    public readonly Box3d BoundingBox3d
    {
        get
        {
            return new Box3d(P0, P1, P2, P3);
        }
    }

    #endregion
}

public static partial class Fun
{
    /// <summary>
    /// Returns whether the given <see cref="Quad3d"/> are equal within the given tolerance.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Quad3d a, Quad3d b, double tolerance)
        => ApproximateEquals(a.P0, b.P0, tolerance) && ApproximateEquals(a.P1, b.P1, tolerance) && ApproximateEquals(a.P2, b.P2, tolerance) && ApproximateEquals(a.P3, b.P3, tolerance);

    /// <summary>
    /// Returns whether the given <see cref="Quad3d"/> are equal within
    /// Constant&lt;double&gt;.PositiveTinyValue.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool ApproximateEquals(this Quad3d a, Quad3d b)
        => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
}

#endregion

#region Quad3dExtensions

public static partial class Quad3dExtensions
{
    #region Geometric Transformations

    /// <summary>
    /// Scales the <see cref="Quad3d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad3d polygon, double scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
        polygon.P3 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Scaled(this Quad3d polygon, double scale)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad3d polygon, V3d center, double scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
        polygon.P3 = center + (polygon.P3 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Scaled(this Quad3d polygon, V3d center, double scale)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Quad3d polygon, double scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d ScaledAboutCentroid(this Quad3d polygon, double scale)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3d"/> by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad3d polygon, V3d scale)
    {
        polygon.P0 *= scale;
        polygon.P1 *= scale;
        polygon.P2 *= scale;
        polygon.P3 *= scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> scaled by the given factor.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Scaled(this Quad3d polygon, V3d scale)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3d"/> by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Scale(this ref Quad3d polygon, V3d center, V3d scale)
    {
        polygon.P0 = center + (polygon.P0 - center) * scale;
        polygon.P1 = center + (polygon.P1 - center) * scale;
        polygon.P2 = center + (polygon.P2 - center) * scale;
        polygon.P3 = center + (polygon.P3 - center) * scale;
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> scaled by the given factor about the given center.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="center">The scaling center.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Scaled(this Quad3d polygon, V3d center, V3d scale)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Scale(center, scale);
        return result;
    }

    /// <summary>
    /// Scales the <see cref="Quad3d"/> by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void ScaleAboutCentroid(this ref Quad3d polygon, V3d scale)
    {
        var center = polygon.ComputeCentroid();
        polygon.Scale(center, scale);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> scaled by the given factor about the centroid.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to scale.</param>
    /// <param name="scale">The scale factor.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d ScaledAboutCentroid(this Quad3d polygon, V3d scale)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.ScaleAboutCentroid(scale);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the given <see cref="M44d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3d polygon, M44d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the given <see cref="M44d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Transformed(this Quad3d polygon, M44d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3d polygon, Euclidean3d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Transformed(this Quad3d polygon, Euclidean3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the inverse of the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3d polygon, Euclidean3d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
        polygon.P3 = t.InvTransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the inverse of the given <see cref="Euclidean3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d InvTransformed(this Quad3d polygon, Euclidean3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3d polygon, Similarity3d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Transformed(this Quad3d polygon, Similarity3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the inverse of the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3d polygon, Similarity3d t)
    {
        polygon.P0 = t.InvTransformPos(polygon.P0);
        polygon.P1 = t.InvTransformPos(polygon.P1);
        polygon.P2 = t.InvTransformPos(polygon.P2);
        polygon.P3 = t.InvTransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the inverse of the given <see cref="Similarity3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d InvTransformed(this Quad3d polygon, Similarity3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the given <see cref="Affine3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3d polygon, Affine3d t)
    {
        polygon.P0 = t.TransformPos(polygon.P0);
        polygon.P1 = t.TransformPos(polygon.P1);
        polygon.P2 = t.TransformPos(polygon.P2);
        polygon.P3 = t.TransformPos(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the given <see cref="Affine3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Transformed(this Quad3d polygon, Affine3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3d polygon, Shift3d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Transformed(this Quad3d polygon, Shift3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the inverse of the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3d polygon, Shift3d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the inverse of the given <see cref="Shift3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d InvTransformed(this Quad3d polygon, Shift3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3d polygon, Rot3d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Transformed(this Quad3d polygon, Rot3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the inverse of the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3d polygon, Rot3d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the inverse of the given <see cref="Rot3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d InvTransformed(this Quad3d polygon, Rot3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3d polygon, Scale3d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Transformed(this Quad3d polygon, Scale3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the inverse of the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void InvTransform(this ref Quad3d polygon, Scale3d t)
    {
        polygon.P0 = t.InvTransform(polygon.P0);
        polygon.P1 = t.InvTransform(polygon.P1);
        polygon.P2 = t.InvTransform(polygon.P2);
        polygon.P3 = t.InvTransform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the inverse of the given <see cref="Scale3d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d InvTransformed(this Quad3d polygon, Scale3d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.InvTransform(t);
        return result;
    }

    /// <summary>
    /// Transforms the <see cref="Quad3d"/> by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The <see cref="Quad3d"/> to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void Transform(this ref Quad3d polygon, M33d t)
    {
        polygon.P0 = t.Transform(polygon.P0);
        polygon.P1 = t.Transform(polygon.P1);
        polygon.P2 = t.Transform(polygon.P2);
        polygon.P3 = t.Transform(polygon.P3);
    }

    /// <summary>
    /// Returns a copy of the <see cref="Quad3d"/> transformed by the given <see cref="M33d"/> transformation.
    /// </summary>
    /// <param name="polygon">The Quad3d to transform.</param>
    /// <param name="t">The transformation to apply.</param>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Quad3d Transformed(this Quad3d polygon, M33d t)
    {
        var result = new Quad3d(polygon.P0, polygon.P1, polygon.P2, polygon.P3);
        result.Transform(t);
        return result;
    }

    #endregion

    public static V3d[] GetPointArray(this Quad3d quad)
    {
        var pa = new V3d[4];
        pa[0] = quad.P0;
        pa[1] = quad.P1;
        pa[2] = quad.P2;
        pa[3] = quad.P3;
        return pa;
    }

    public static V3d ComputeCentroid(this Quad3d quad)
    {
        return 0.25 * (quad.P0 + quad.P1 + quad.P2 + quad.P3);
    }
}

#endregion

