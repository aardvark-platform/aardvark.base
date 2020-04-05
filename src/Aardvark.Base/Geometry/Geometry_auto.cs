using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    #region Polygon2d

    /// <summary>
    /// A polygon internally represented by an array of points. Implemented
    /// as a structure, the validity of the polygon can be checked via its
    /// PointCount, which must be bigger than 0 for a polygon to hold any
    /// points, and bigger than 2 for a polygon to be geometrically valid.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Polygon2d : IValidity, IPolygon<V2d>, IBoundingBox2d
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
                    throw new ArgumentException(
                                "point count must be smaller or equal array length");
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
            if (startIndex < 0 || startIndex >= pointArray.Length - 1) throw new ArgumentException();
            if (count <= 0 || startIndex + count >= pointArray.Length) throw new ArgumentException();
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
            : this(points.ToArray())
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

        public static readonly Polygon2d Invalid = new Polygon2d(null, 0);

        public bool IsValid => m_pointArray != null;

        public bool IsInvalid => m_pointArray == null;

        #endregion

        #region Properties

        /// <summary>
        /// The number of points in the polygon. If this is 0, the polygon
        /// is invalid.
        /// </summary>
        public int PointCount => m_pointCount;

        /// <summary>
        /// Enumerates points.
        /// </summary>
        public IEnumerable<V2d> Points
        {
            get { for (int pi = 0; pi < m_pointCount; pi++) yield return m_pointArray[pi]; }
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Returns a copy of the polygons point array.
        /// </summary>
        public V2d[] GetPointArray()
        {
            var pc = m_pointCount;
            var pa = new V2d[pc];
            for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
            return pa;
        }

        /// <summary>
        /// [P0, P1, P2] -> [P0, P1, P2, P0].
        /// </summary>
        public V2d[] GetPointArrayWithRepeatedFirstPoint()
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
        public T[] GetPointArray<T>(Func<V2d, T> point_copyFun)
        {
            var pc = m_pointCount;
            var pa = new T[pc];
            for (int pi = 0; pi < pc; pi++) pa[pi] = point_copyFun(m_pointArray[pi]);
            return pa;
        }

        /// <summary>
        /// Returns a transformed copy of the polygons point array.
        /// </summary>
        public T[] GetPointArray<T>(Func<V2d, int, T> point_index_copyFun)
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
        public V2d this[int index]
        {
            get { return m_pointArray[index]; }
            set { m_pointArray[index] = value; }
        }

        #endregion

        #region Edges and Lines

        /// <summary>
        /// Index-th edge as vector (edgeEndPos - edgeBeginPos).
        /// </summary>
        public V2d Edge(int index)
        {
            var p0 = m_pointArray[index++];
            var p1 = m_pointArray[index < m_pointCount ? index : 0];
            return p1 - p0;
        }

        /// <summary>
        /// Edges as vectors (edgeEndPos - edgeBeginPos).
        /// </summary>
        public IEnumerable<V2d> Edges
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
        public Line2d EdgeLine(int index)
        {
            var p0 = m_pointArray[index++];
            var p1 = m_pointArray[index < m_pointCount ? index : 0];
            return new Line2d(p0, p1);
        }

        /// <summary>
        /// Edges as line segments (edgeBeginPos, edgeEndPos).
        /// </summary>
        public IEnumerable<Line2d> EdgeLines
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
        public V2d[] GetEdgeArray()
        {
            var pc = m_pointCount;
            if (pc < 2) return new V2d[0];
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
        public Line2d[] GetEdgeLineArray()
        {
            var pc = PointCount;
            if (pc < 2) return new Line2d[0];
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
        public Polygon2d Copy()
        {
            return new Polygon2d(m_pointArray.Copy());
        }

        /// <summary>
        /// Obsolete. Use 'Map' instead (same functionality and parameters).
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters).", false)]
        public Polygon2d Copy(Func<V2d, V2d> point_fun)
        {
            return Map(point_fun);
        }

        /// <summary>
        /// Returns transformed copy of this polygon.
        /// </summary>
        public Polygon2d Map(Func<V2d, V2d> point_fun)
        {
            var pc = m_pointCount;
            V2d[] opa = m_pointArray, npa = new V2d[pc];
            for (int pi = 0; pi < pc; pi++) npa[pi] = point_fun(opa[pi]);
            return new Polygon2d(npa, pc);
        }

        /// <summary>
        /// Gets copy with reversed order of vertices. 
        /// </summary>
        public Polygon2d Reversed
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
        /// Obsolete. Use 'Reverse' instead..
        /// </summary>
        [Obsolete("Use 'Reverse' instead.", false)]
        public void Revert() => Reverse();

        /// <summary>
        /// Reverses order of vertices in-place. 
        /// </summary>
        public void Reverse()
        {
            var pa = m_pointArray;
            for (int pi = 0, pj = m_pointCount - 1; pi < pj; pi++, pj--)
            { var t = pa[pi]; pa[pi] = pa[pj]; pa[pj] = t; }
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

        public override int GetHashCode()
        {
            return m_pointArray.GetCombinedHashCode(m_pointCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Polygon2d other)
        {
            if (m_pointCount != other.m_pointCount) return false;
            for (int pi = 0; pi < m_pointCount; pi++)
                if (!m_pointArray[pi].Equals(other.m_pointArray[pi])) return false;
            return true;
        }

        public override bool Equals(object other)
            => (other is Polygon2d o) ? Equals(o) : false;

        public override string ToString()
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
        public Box2d BoundingBox2d
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
            var scale = 1.0 / (double)pc;
            return sum * scale;
        }

        public static double ComputePerimeter(this Polygon2d polygon)
        {
            var pc = polygon.PointCount;
            var p0 = polygon[pc - 1];
            var r = 0.0;
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

        public static Polygon2d Scaled(
                this Polygon2d polygon, double scale)
        {
            return polygon.Map(p => p * scale);
        }

        public static Polygon2d Scaled(
                this Polygon2d polygon, V2d center, double scale)
        {
            return polygon.Map(p => center + (p - center) * scale);
        }

        public static Polygon2d ScaledAboutVertexCentroid(
                this Polygon2d polygon, double scale)
        {
            return polygon.Scaled(polygon.ComputeVertexCentroid(), scale);
        }

        public static Polygon2d Scaled(
                this Polygon2d polygon, V2d scale)
        {
            return polygon.Map(p => p * scale);
        }

        public static Polygon2d Scaled(
                this Polygon2d polygon, V2d center, V2d scale)
        {
            return polygon.Map(p => center + (p - center) * scale);
        }

        public static Polygon2d ScaledAboutVertexCentroid(
                this Polygon2d polygon, V2d scale)
        {
            return polygon.Scaled(polygon.ComputeVertexCentroid(), scale);
        }

        public static Polygon2d Transformed(
                this Polygon2d polygon, M22d m)
        {
            return polygon.Map(p => m.Transform(p));
        }

        public static Polygon2d Transformed(
                this Polygon2d polygon, M33d m)
        {
            return polygon.Map(p => m.TransformPos(p));
        }

        public static Polygon2d WithoutMultiplePoints(
                this Polygon2d polygon, double eps = 1e-8)
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
                this Polygon2d polygon, Box2d box, double eps = 1e-8)
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
    public partial struct IndexPolygon2d : IValidity, IPolygon<V2d>
    {
        private int m_pointCount;
        private int m_firstIndex;
        private int[] m_indexArray;
        private V2d[] m_pointArray;

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

        public static readonly IndexPolygon2d Invalid = new IndexPolygon2d(null, 0, 0, null);

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
            var scale = 1.0 / (double)pc;
            return sum * scale;
        }

        public static double ComputePerimeter(this IndexPolygon2d polygon)
        {
            var pc = polygon.PointCount;
            var p0 = polygon[pc - 1];
            var r = 0.0;
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
    public partial struct Line2d : IValidity, IPolygon<V2d>, IBoundingBox2d
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

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        public int PointCount { get { return 2; } }

        public IEnumerable<V2d> Points
        {
            get { yield return P0; yield return P1; }
        }

        public Line2d Reversed
        {
            get { return new Line2d(P1, P0); }
        }

        #endregion

        #region Indexer

        public V2d this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return P0;
                    case 1: return P1;
                    default: throw new IndexOutOfRangeException();
                }
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

        public Line2d Copy(Func<V2d, V2d> point_copyFun)
        {
            return new Line2d(point_copyFun(P0), point_copyFun(P1));
        }

        public Line3d ToLine3d(Func<V2d, V3d> point_copyFun)
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

        public override int GetHashCode()
        {
            return HashCode.GetCombined(P0, P1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Line2d other)
            => P0.Equals(other.P0) && P1.Equals(other.P1);

        public override bool Equals(object other)
             => (other is Line2d o) ? Equals(o) : false;

        public override string ToString()
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

        public Box2d BoundingBox2d
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
    public partial struct Triangle2d : IValidity, IPolygon<V2d>, IBoundingBox2d
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

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        /// <summary>
        /// Edge P1 - P0
        /// </summary>
        public V2d Edge01 { get { return P1 - P0; } }
        /// <summary>
        /// Edge P2 - P0
        /// </summary>
        public V2d Edge02 { get { return P2 - P0; } }
        /// <summary>
        /// Edge P2 - P1
        /// </summary>
        public V2d Edge12 { get { return P2 - P1; } }
        /// <summary>
        /// Edge P0 - P1
        /// </summary>
        public V2d Edge10 { get { return P0 - P1; } }
        /// <summary>
        /// Edge P0 - P2
        /// </summary>
        public V2d Edge20 { get { return P0 - P2; } }
        /// <summary>
        /// Edge P1 - P2
        /// </summary>
        public V2d Edge21 { get { return P1 - P2; } }

        public IEnumerable<V2d> Edges
        {
            get
            {
                yield return P1 - P0;
                yield return P2 - P1;
                yield return P0 - P2;
            }
        }

        public V2d[] EdgeArray
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

        public IEnumerable<Line2d> EdgeLines
        {
            get
            {
                yield return new Line2d(P0, P1);
                yield return new Line2d(P1, P2);
                yield return new Line2d(P2, P0);
            }
        }

        public Line2d[] EdgeLineArray
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

        public Line2d GetEdgeLine(int index)
        {
            switch (index)
            {
                case 0: return new Line2d(P0, P1);
                case 1: return new Line2d(P1, P2);
                case 2: return new Line2d(P2, P0);
            }
            throw new InvalidOperationException();
        }

        public V2d GetEdge(int index)
        {
            switch (index)
            {
                case 0: return P1 - P0;
                case 1: return P2 - P1;
                case 2: return P0 - P2;
            }
            throw new InvalidOperationException();
        }

        public Line2d Line01 { get { return new Line2d(P0, P1); } }
        public Line2d Line02 { get { return new Line2d(P0, P2); } }
        public Line2d Line12 { get { return new Line2d(P1, P2); } }
        public Line2d Line10 { get { return new Line2d(P1, P0); } }
        public Line2d Line20 { get { return new Line2d(P2, P0); } }
        public Line2d Line21 { get { return new Line2d(P2, P1); } }

        public int PointCount { get { return 3; } }

        public IEnumerable<V2d> Points
        {
            get { yield return P0; yield return P1; yield return P2; }
        }

        public Triangle2d Reversed
        {
            get { return new Triangle2d(P2, P1, P0); }
        }

        #endregion

        #region Indexer

        public V2d this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return P0;
                    case 1: return P1;
                    case 2: return P2;
                    default: throw new IndexOutOfRangeException();
                }
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

        public Triangle2d Copy(Func<V2d, V2d> point_copyFun)
        {
            return new Triangle2d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
        }

        public Triangle3d ToTriangle3d(Func<V2d, V3d> point_copyFun)
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

        public override int GetHashCode()
        {
            return HashCode.GetCombined(P0, P1, P2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Triangle2d other)
            => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2);

        public override bool Equals(object other)
             => (other is Triangle2d o) ? Equals(o) : false;

        public override string ToString()
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

        public Box2d BoundingBox2d
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
    public partial struct Quad2d : IValidity, IPolygon<V2d>, IBoundingBox2d
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

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        /// <summary>
        /// Edge P1 - P0
        /// </summary>
        public V2d Edge01 { get { return P1 - P0; } }
        /// <summary>
        /// Edge P3 - P0
        /// </summary>
        public V2d Edge03 { get { return P3 - P0; } }
        /// <summary>
        /// Edge P2 - P1
        /// </summary>
        public V2d Edge12 { get { return P2 - P1; } }
        /// <summary>
        /// Edge P0 - P1
        /// </summary>
        public V2d Edge10 { get { return P0 - P1; } }
        /// <summary>
        /// Edge P3 - P2
        /// </summary>
        public V2d Edge23 { get { return P3 - P2; } }
        /// <summary>
        /// Edge P1 - P2
        /// </summary>
        public V2d Edge21 { get { return P1 - P2; } }
        /// <summary>
        /// Edge P0 - P3
        /// </summary>
        public V2d Edge30 { get { return P0 - P3; } }
        /// <summary>
        /// Edge P2 - P3
        /// </summary>
        public V2d Edge32 { get { return P2 - P3; } }

        public IEnumerable<V2d> Edges
        {
            get
            {
                yield return P1 - P0;
                yield return P2 - P1;
                yield return P3 - P2;
                yield return P0 - P3;
            }
        }

        public V2d[] EdgeArray
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

        public IEnumerable<Line2d> EdgeLines
        {
            get
            {
                yield return new Line2d(P0, P1);
                yield return new Line2d(P1, P2);
                yield return new Line2d(P2, P3);
                yield return new Line2d(P3, P0);
            }
        }

        public Line2d[] EdgeLineArray
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

        public Line2d GetEdgeLine(int index)
        {
            switch (index)
            {
                case 0: return new Line2d(P0, P1);
                case 1: return new Line2d(P1, P2);
                case 2: return new Line2d(P2, P3);
                case 3: return new Line2d(P3, P0);
            }
            throw new InvalidOperationException();
        }

        public V2d GetEdge(int index)
        {
            switch (index)
            {
                case 0: return P1 - P0;
                case 1: return P2 - P1;
                case 2: return P3 - P2;
                case 3: return P0 - P3;
            }
            throw new InvalidOperationException();
        }

        public Line2d Line01 { get { return new Line2d(P0, P1); } }
        public Line2d Line03 { get { return new Line2d(P0, P3); } }
        public Line2d Line12 { get { return new Line2d(P1, P2); } }
        public Line2d Line10 { get { return new Line2d(P1, P0); } }
        public Line2d Line23 { get { return new Line2d(P2, P3); } }
        public Line2d Line21 { get { return new Line2d(P2, P1); } }
        public Line2d Line30 { get { return new Line2d(P3, P0); } }
        public Line2d Line32 { get { return new Line2d(P3, P2); } }

        public int PointCount { get { return 4; } }

        public IEnumerable<V2d> Points
        {
            get { yield return P0; yield return P1; yield return P2; yield return P3; }
        }

        public Quad2d Reversed
        {
            get { return new Quad2d(P3, P2, P1, P0); }
        }

        #endregion

        #region Indexer

        public V2d this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return P0;
                    case 1: return P1;
                    case 2: return P2;
                    case 3: return P3;
                    default: throw new IndexOutOfRangeException();
                }
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

        public Quad2d Copy(Func<V2d, V2d> point_copyFun)
        {
            return new Quad2d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
        }

        public Quad3d ToQuad3d(Func<V2d, V3d> point_copyFun)
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

        public override int GetHashCode()
        {
            return HashCode.GetCombined(P0, P1, P2, P3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Quad2d other)
            => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

        public override bool Equals(object other)
             => (other is Quad2d o) ? Equals(o) : false;

        public override string ToString()
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

        public Box2d BoundingBox2d
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
    public partial struct Polygon3d : IValidity, IPolygon<V3d>, IBoundingBox3d
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
                    throw new ArgumentException(
                                "point count must be smaller or equal array length");
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
            if (startIndex < 0 || startIndex >= pointArray.Length - 1) throw new ArgumentException();
            if (count <= 0 || startIndex + count >= pointArray.Length) throw new ArgumentException();
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
            : this(points.ToArray())
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

        public static readonly Polygon3d Invalid = new Polygon3d(null, 0);

        public bool IsValid => m_pointArray != null;

        public bool IsInvalid => m_pointArray == null;

        #endregion

        #region Properties

        /// <summary>
        /// The number of points in the polygon. If this is 0, the polygon
        /// is invalid.
        /// </summary>
        public int PointCount => m_pointCount;

        /// <summary>
        /// Enumerates points.
        /// </summary>
        public IEnumerable<V3d> Points
        {
            get { for (int pi = 0; pi < m_pointCount; pi++) yield return m_pointArray[pi]; }
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Returns a copy of the polygons point array.
        /// </summary>
        public V3d[] GetPointArray()
        {
            var pc = m_pointCount;
            var pa = new V3d[pc];
            for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
            return pa;
        }

        /// <summary>
        /// [P0, P1, P2] -> [P0, P1, P2, P0].
        /// </summary>
        public V3d[] GetPointArrayWithRepeatedFirstPoint()
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
        public T[] GetPointArray<T>(Func<V3d, T> point_copyFun)
        {
            var pc = m_pointCount;
            var pa = new T[pc];
            for (int pi = 0; pi < pc; pi++) pa[pi] = point_copyFun(m_pointArray[pi]);
            return pa;
        }

        /// <summary>
        /// Returns a transformed copy of the polygons point array.
        /// </summary>
        public T[] GetPointArray<T>(Func<V3d, int, T> point_index_copyFun)
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
        public V3d this[int index]
        {
            get { return m_pointArray[index]; }
            set { m_pointArray[index] = value; }
        }

        #endregion

        #region Edges and Lines

        /// <summary>
        /// Index-th edge as vector (edgeEndPos - edgeBeginPos).
        /// </summary>
        public V3d Edge(int index)
        {
            var p0 = m_pointArray[index++];
            var p1 = m_pointArray[index < m_pointCount ? index : 0];
            return p1 - p0;
        }

        /// <summary>
        /// Edges as vectors (edgeEndPos - edgeBeginPos).
        /// </summary>
        public IEnumerable<V3d> Edges
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
        public Line3d EdgeLine(int index)
        {
            var p0 = m_pointArray[index++];
            var p1 = m_pointArray[index < m_pointCount ? index : 0];
            return new Line3d(p0, p1);
        }

        /// <summary>
        /// Edges as line segments (edgeBeginPos, edgeEndPos).
        /// </summary>
        public IEnumerable<Line3d> EdgeLines
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
        public V3d[] GetEdgeArray()
        {
            var pc = m_pointCount;
            if (pc < 2) return new V3d[0];
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
        public Line3d[] GetEdgeLineArray()
        {
            var pc = PointCount;
            if (pc < 2) return new Line3d[0];
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
        public Polygon3d Copy()
        {
            return new Polygon3d(m_pointArray.Copy());
        }

        /// <summary>
        /// Obsolete. Use 'Map' instead (same functionality and parameters).
        /// </summary>
        [Obsolete("Use 'Map' instead (same functionality and parameters).", false)]
        public Polygon3d Copy(Func<V3d, V3d> point_fun)
        {
            return Map(point_fun);
        }

        /// <summary>
        /// Returns transformed copy of this polygon.
        /// </summary>
        public Polygon3d Map(Func<V3d, V3d> point_fun)
        {
            var pc = m_pointCount;
            V3d[] opa = m_pointArray, npa = new V3d[pc];
            for (int pi = 0; pi < pc; pi++) npa[pi] = point_fun(opa[pi]);
            return new Polygon3d(npa, pc);
        }

        /// <summary>
        /// Gets copy with reversed order of vertices. 
        /// </summary>
        public Polygon3d Reversed
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
        /// Obsolete. Use 'Reverse' instead..
        /// </summary>
        [Obsolete("Use 'Reverse' instead.", false)]
        public void Revert() => Reverse();

        /// <summary>
        /// Reverses order of vertices in-place. 
        /// </summary>
        public void Reverse()
        {
            var pa = m_pointArray;
            for (int pi = 0, pj = m_pointCount - 1; pi < pj; pi++, pj--)
            { var t = pa[pi]; pa[pi] = pa[pj]; pa[pj] = t; }
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

        public override int GetHashCode()
        {
            return m_pointArray.GetCombinedHashCode(m_pointCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Polygon3d other)
        {
            if (m_pointCount != other.m_pointCount) return false;
            for (int pi = 0; pi < m_pointCount; pi++)
                if (!m_pointArray[pi].Equals(other.m_pointArray[pi])) return false;
            return true;
        }

        public override bool Equals(object other)
            => (other is Polygon3d o) ? Equals(o) : false;

        public override string ToString()
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
        public Box3d BoundingBox3d
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
            var scale = 1.0 / (double)pc;
            return sum * scale;
        }

        public static double ComputePerimeter(this Polygon3d polygon)
        {
            var pc = polygon.PointCount;
            var p0 = polygon[pc - 1];
            var r = 0.0;
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

        public static Polygon3d Scaled(
                this Polygon3d polygon, double scale)
        {
            return polygon.Map(p => p * scale);
        }

        public static Polygon3d Scaled(
                this Polygon3d polygon, V3d center, double scale)
        {
            return polygon.Map(p => center + (p - center) * scale);
        }

        public static Polygon3d ScaledAboutVertexCentroid(
                this Polygon3d polygon, double scale)
        {
            return polygon.Scaled(polygon.ComputeVertexCentroid(), scale);
        }

        public static Polygon3d Scaled(
                this Polygon3d polygon, V3d scale)
        {
            return polygon.Map(p => p * scale);
        }

        public static Polygon3d Scaled(
                this Polygon3d polygon, V3d center, V3d scale)
        {
            return polygon.Map(p => center + (p - center) * scale);
        }

        public static Polygon3d ScaledAboutVertexCentroid(
                this Polygon3d polygon, V3d scale)
        {
            return polygon.Scaled(polygon.ComputeVertexCentroid(), scale);
        }

        public static Polygon3d Transformed(
                this Polygon3d polygon, M33d m)
        {
            return polygon.Map(p => m.Transform(p));
        }

        public static Polygon3d Transformed(
                this Polygon3d polygon, M44d m)
        {
            return polygon.Map(p => m.TransformPos(p));
        }

        public static Polygon3d WithoutMultiplePoints(
                this Polygon3d polygon, double eps = 1e-8)
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
                this Polygon3d polygon, Box3d box, double eps = 1e-8)
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
    public partial struct IndexPolygon3d : IValidity, IPolygon<V3d>
    {
        private int m_pointCount;
        private int m_firstIndex;
        private int[] m_indexArray;
        private V3d[] m_pointArray;

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

        public static readonly IndexPolygon3d Invalid = new IndexPolygon3d(null, 0, 0, null);

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
            var scale = 1.0 / (double)pc;
            return sum * scale;
        }

        public static double ComputePerimeter(this IndexPolygon3d polygon)
        {
            var pc = polygon.PointCount;
            var p0 = polygon[pc - 1];
            var r = 0.0;
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
    public partial struct Line3d : IValidity, IPolygon<V3d>, IBoundingBox3d
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

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        public int PointCount { get { return 2; } }

        public IEnumerable<V3d> Points
        {
            get { yield return P0; yield return P1; }
        }

        public Line3d Reversed
        {
            get { return new Line3d(P1, P0); }
        }

        #endregion

        #region Indexer

        public V3d this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return P0;
                    case 1: return P1;
                    default: throw new IndexOutOfRangeException();
                }
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

        public Line3d Copy(Func<V3d, V3d> point_copyFun)
        {
            return new Line3d(point_copyFun(P0), point_copyFun(P1));
        }

        public Line2d ToLine2d(Func<V3d, V2d> point_copyFun)
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

        public override int GetHashCode()
        {
            return HashCode.GetCombined(P0, P1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Line3d other)
            => P0.Equals(other.P0) && P1.Equals(other.P1);

        public override bool Equals(object other)
             => (other is Line3d o) ? Equals(o) : false;

        public override string ToString()
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

        public Box3d BoundingBox3d
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
    public partial struct Triangle3d : IValidity, IPolygon<V3d>, IBoundingBox3d
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

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        /// <summary>
        /// Edge P1 - P0
        /// </summary>
        public V3d Edge01 { get { return P1 - P0; } }
        /// <summary>
        /// Edge P2 - P0
        /// </summary>
        public V3d Edge02 { get { return P2 - P0; } }
        /// <summary>
        /// Edge P2 - P1
        /// </summary>
        public V3d Edge12 { get { return P2 - P1; } }
        /// <summary>
        /// Edge P0 - P1
        /// </summary>
        public V3d Edge10 { get { return P0 - P1; } }
        /// <summary>
        /// Edge P0 - P2
        /// </summary>
        public V3d Edge20 { get { return P0 - P2; } }
        /// <summary>
        /// Edge P1 - P2
        /// </summary>
        public V3d Edge21 { get { return P1 - P2; } }

        public IEnumerable<V3d> Edges
        {
            get
            {
                yield return P1 - P0;
                yield return P2 - P1;
                yield return P0 - P2;
            }
        }

        public V3d[] EdgeArray
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

        public IEnumerable<Line3d> EdgeLines
        {
            get
            {
                yield return new Line3d(P0, P1);
                yield return new Line3d(P1, P2);
                yield return new Line3d(P2, P0);
            }
        }

        public Line3d[] EdgeLineArray
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

        public Line3d GetEdgeLine(int index)
        {
            switch (index)
            {
                case 0: return new Line3d(P0, P1);
                case 1: return new Line3d(P1, P2);
                case 2: return new Line3d(P2, P0);
            }
            throw new InvalidOperationException();
        }

        public V3d GetEdge(int index)
        {
            switch (index)
            {
                case 0: return P1 - P0;
                case 1: return P2 - P1;
                case 2: return P0 - P2;
            }
            throw new InvalidOperationException();
        }

        public Line3d Line01 { get { return new Line3d(P0, P1); } }
        public Line3d Line02 { get { return new Line3d(P0, P2); } }
        public Line3d Line12 { get { return new Line3d(P1, P2); } }
        public Line3d Line10 { get { return new Line3d(P1, P0); } }
        public Line3d Line20 { get { return new Line3d(P2, P0); } }
        public Line3d Line21 { get { return new Line3d(P2, P1); } }

        public int PointCount { get { return 3; } }

        public IEnumerable<V3d> Points
        {
            get { yield return P0; yield return P1; yield return P2; }
        }

        public Triangle3d Reversed
        {
            get { return new Triangle3d(P2, P1, P0); }
        }

        #endregion

        #region Indexer

        public V3d this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return P0;
                    case 1: return P1;
                    case 2: return P2;
                    default: throw new IndexOutOfRangeException();
                }
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

        public Triangle3d Copy(Func<V3d, V3d> point_copyFun)
        {
            return new Triangle3d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2));
        }

        public Triangle2d ToTriangle2d(Func<V3d, V2d> point_copyFun)
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

        public override int GetHashCode()
        {
            return HashCode.GetCombined(P0, P1, P2);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Triangle3d other)
            => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2);

        public override bool Equals(object other)
             => (other is Triangle3d o) ? Equals(o) : false;

        public override string ToString()
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

        public Box3d BoundingBox3d
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
    public partial struct Quad3d : IValidity, IPolygon<V3d>, IBoundingBox3d
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

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        /// <summary>
        /// Edge P1 - P0
        /// </summary>
        public V3d Edge01 { get { return P1 - P0; } }
        /// <summary>
        /// Edge P3 - P0
        /// </summary>
        public V3d Edge03 { get { return P3 - P0; } }
        /// <summary>
        /// Edge P2 - P1
        /// </summary>
        public V3d Edge12 { get { return P2 - P1; } }
        /// <summary>
        /// Edge P0 - P1
        /// </summary>
        public V3d Edge10 { get { return P0 - P1; } }
        /// <summary>
        /// Edge P3 - P2
        /// </summary>
        public V3d Edge23 { get { return P3 - P2; } }
        /// <summary>
        /// Edge P1 - P2
        /// </summary>
        public V3d Edge21 { get { return P1 - P2; } }
        /// <summary>
        /// Edge P0 - P3
        /// </summary>
        public V3d Edge30 { get { return P0 - P3; } }
        /// <summary>
        /// Edge P2 - P3
        /// </summary>
        public V3d Edge32 { get { return P2 - P3; } }

        public IEnumerable<V3d> Edges
        {
            get
            {
                yield return P1 - P0;
                yield return P2 - P1;
                yield return P3 - P2;
                yield return P0 - P3;
            }
        }

        public V3d[] EdgeArray
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

        public IEnumerable<Line3d> EdgeLines
        {
            get
            {
                yield return new Line3d(P0, P1);
                yield return new Line3d(P1, P2);
                yield return new Line3d(P2, P3);
                yield return new Line3d(P3, P0);
            }
        }

        public Line3d[] EdgeLineArray
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

        public Line3d GetEdgeLine(int index)
        {
            switch (index)
            {
                case 0: return new Line3d(P0, P1);
                case 1: return new Line3d(P1, P2);
                case 2: return new Line3d(P2, P3);
                case 3: return new Line3d(P3, P0);
            }
            throw new InvalidOperationException();
        }

        public V3d GetEdge(int index)
        {
            switch (index)
            {
                case 0: return P1 - P0;
                case 1: return P2 - P1;
                case 2: return P3 - P2;
                case 3: return P0 - P3;
            }
            throw new InvalidOperationException();
        }

        public Line3d Line01 { get { return new Line3d(P0, P1); } }
        public Line3d Line03 { get { return new Line3d(P0, P3); } }
        public Line3d Line12 { get { return new Line3d(P1, P2); } }
        public Line3d Line10 { get { return new Line3d(P1, P0); } }
        public Line3d Line23 { get { return new Line3d(P2, P3); } }
        public Line3d Line21 { get { return new Line3d(P2, P1); } }
        public Line3d Line30 { get { return new Line3d(P3, P0); } }
        public Line3d Line32 { get { return new Line3d(P3, P2); } }

        public int PointCount { get { return 4; } }

        public IEnumerable<V3d> Points
        {
            get { yield return P0; yield return P1; yield return P2; yield return P3; }
        }

        public Quad3d Reversed
        {
            get { return new Quad3d(P3, P2, P1, P0); }
        }

        #endregion

        #region Indexer

        public V3d this[int index]
        {
            get
            {
                switch (index)
                {
                    case 0: return P0;
                    case 1: return P1;
                    case 2: return P2;
                    case 3: return P3;
                    default: throw new IndexOutOfRangeException();
                }
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

        public Quad3d Copy(Func<V3d, V3d> point_copyFun)
        {
            return new Quad3d(point_copyFun(P0), point_copyFun(P1), point_copyFun(P2), point_copyFun(P3));
        }

        public Quad2d ToQuad2d(Func<V3d, V2d> point_copyFun)
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

        public override int GetHashCode()
        {
            return HashCode.GetCombined(P0, P1, P2, P3);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Quad3d other)
            => P0.Equals(other.P0) && P1.Equals(other.P1) && P2.Equals(other.P2) && P3.Equals(other.P3);

        public override bool Equals(object other)
             => (other is Quad3d o) ? Equals(o) : false;

        public override string ToString()
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

        public Box3d BoundingBox3d
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

    #region Ellipse2d

    /// <summary>
    /// A 2D ellipse is defined by its center
    /// and two half-axes.
    /// Note that in principle any two conjugate half-diameters
    /// can be used as axes, however some algorithms require that
    /// the major and minor half axes are known. By convention
    /// in this case, axis0 is the major half axis.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Ellipse2d
    {
        public V2d Center;
        public V2d Axis0;
        public V2d Axis1;

        #region Constructors

        public Ellipse2d(V2d center, V2d axis0, V2d axis1)
        {
            Center = center;
            Axis0 = axis0;
            Axis1 = axis1;
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// </summary>
        public static Ellipse2d FromConjugateDiameters(V2d center, V2d a, V2d b)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2)   return new Ellipse2d(center, a, b);
                else            return new Ellipse2d(center, b, a);
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                V2d v0 = a * ct + b * st, v1 = b * ct - a * st;
                if (v0.LengthSquared >= v1.LengthSquared)   return new Ellipse2d(center, v0, v1);
                else                                        return new Ellipse2d(center, v1, v0);
            }
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// Also computes the squared lengths of the major and minor
        /// half axis.
        /// </summary>
        public static Ellipse2d FromConjugateDiameters(V2d center, V2d a, V2d b,
                out double major2, out double minor2)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2)   { major2 = a2; minor2 = b2; return new Ellipse2d(center, a, b); }
                else            { major2 = b2; minor2 = a2; return new Ellipse2d(center, b, a); }
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                V2d v0 = a * ct + b * st, v1 = b * ct - a * st;
                a2 = v0.LengthSquared; b2 = v1.LengthSquared;
                if (a2 >= b2)   { major2 = a2; minor2 = b2; return new Ellipse2d(center, v0, v1); }
                else            { major2 = b2; minor2 = a2; return new Ellipse2d(center, v1, v0); }
            }
        }

        #endregion

        #region Constants

        public static readonly Ellipse2d Zero = new Ellipse2d(V2d.Zero, V2d.Zero, V2d.Zero);

        #endregion

        #region Operations

        public V2d GetVector(double alpha)
        {
            return Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);
        }

        public V2d GetPoint(double alpha)
        {
            return Center + Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);
        }

        /// <summary>
        /// Perform the supplied action for each of count vectors from the center
        /// of the ellipse to the circumference. 
        /// </summary>
        public void ForEachVector(int count, Action<int, V2d> index_vector_act)
        {
            double d = Constant.PiTimesTwo / count;
            double a = Fun.Sin(d * 0.5).Square() * 2.0, b = Fun.Sin(d); // init trig. recurrence
            double ct = 1.0, st = 0.0;

            index_vector_act(0, Axis0);
            for (int i = 1; i < count; i++)
            {
                double dct = a * ct + b * st, dst = a * st - b * ct; ;  // trig. recurrence
                ct -= dct; st -= dst;                                   // cos (t + d), sin (t + d)
                index_vector_act(i, Axis0 * ct + Axis1 * st);
            }
        }

        /// <summary>
        /// Get count points on the circumference of the ellipse.
        /// </summary>
        public V2d[] GetPoints(int count)
        {
            var array = new V2d[count];
            var c = Center;
            ForEachVector(count, (i, v) => array[i] = c + v);
            return array;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ellipse2d a, Ellipse2d b) => 
            (a.Center == b.Center) && 
            (a.Axis0 == b.Axis0) && 
            (a.Axis1 == b.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ellipse2d a, Ellipse2d b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Center, Axis0, Axis1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Ellipse2d other) =>
            Center.Equals(other.Center) &&
            Axis0.Equals(other.Axis0) &&
            Axis1.Equals(other.Axis1);

        public override bool Equals(object other)
             => (other is Ellipse2d o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Center, Axis0, Axis1);
        }

        public static Ellipse2d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ellipse2d(
                V2d.Parse(x[0]),
                V2d.Parse(x[1]),
                V2d.Parse(x[2])
            );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Ellipse2d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse2d a, Ellipse2d b, double tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Axis0, b.Axis0, tolerance) &&
            ApproximateEquals(a.Axis1, b.Axis1, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Ellipse2d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse2d a, Ellipse2d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

    #region Ellipse3d

    /// <summary>
    /// A 3D ellipse is defined by its center, its plane normal,
    /// and two half-axes.
    /// Note that in principle any two conjugate half-diameters
    /// can be used as axes, however some algorithms require that
    /// the major and minor half axes are known. By convention
    /// in this case, axis0 is the major half axis.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct Ellipse3d
    {
        public V3d Center;
        public V3d Normal;
        public V3d Axis0;
        public V3d Axis1;

        #region Constructors

        public Ellipse3d(V3d center, V3d normal, V3d axis0, V3d axis1)
        {
            Center = center;
            Normal = normal;
            Axis0 = axis0;
            Axis1 = axis1;
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// </summary>
        public static Ellipse3d FromConjugateDiameters(V3d center, V3d normal, V3d a, V3d b)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2)   return new Ellipse3d(center, normal, a, b);
                else            return new Ellipse3d(center, normal, b, a);
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                V3d v0 = a * ct + b * st, v1 = b * ct - a * st;
                if (v0.LengthSquared >= v1.LengthSquared)   return new Ellipse3d(center, normal, v0, v1);
                else                                        return new Ellipse3d(center, normal, v1, v0);
            }
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// Also computes the squared lengths of the major and minor
        /// half axis.
        /// </summary>
        public static Ellipse3d FromConjugateDiameters(V3d center, V3d normal, V3d a, V3d b,
                out double major2, out double minor2)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2)   { major2 = a2; minor2 = b2; return new Ellipse3d(center, normal, a, b); }
                else            { major2 = b2; minor2 = a2; return new Ellipse3d(center, normal, b, a); }
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                V3d v0 = a * ct + b * st, v1 = b * ct - a * st;
                a2 = v0.LengthSquared; b2 = v1.LengthSquared;
                if (a2 >= b2)   { major2 = a2; minor2 = b2; return new Ellipse3d(center, normal, v0, v1); }
                else            { major2 = b2; minor2 = a2; return new Ellipse3d(center, normal, v1, v0); }
            }
        }

        #endregion

        #region Constants

        public static readonly Ellipse3d Zero = new Ellipse3d(V3d.Zero, V3d.Zero, V3d.Zero, V3d.Zero);

        #endregion

        #region Operations

        public V3d GetVector(double alpha)
        {
            return Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);
        }

        public V3d GetPoint(double alpha)
        {
            return Center + Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);
        }

        /// <summary>
        /// Perform the supplied action for each of count vectors from the center
        /// of the ellipse to the circumference. 
        /// </summary>
        public void ForEachVector(int count, Action<int, V3d> index_vector_act)
        {
            double d = Constant.PiTimesTwo / count;
            double a = Fun.Sin(d * 0.5).Square() * 2.0, b = Fun.Sin(d); // init trig. recurrence
            double ct = 1.0, st = 0.0;

            index_vector_act(0, Axis0);
            for (int i = 1; i < count; i++)
            {
                double dct = a * ct + b * st, dst = a * st - b * ct; ;  // trig. recurrence
                ct -= dct; st -= dst;                                   // cos (t + d), sin (t + d)
                index_vector_act(i, Axis0 * ct + Axis1 * st);
            }
        }

        /// <summary>
        /// Get count points on the circumference of the ellipse.
        /// </summary>
        public V3d[] GetPoints(int count)
        {
            var array = new V3d[count];
            var c = Center;
            ForEachVector(count, (i, v) => array[i] = c + v);
            return array;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(Ellipse3d a, Ellipse3d b) => 
            (a.Center == b.Center) && 
            (a.Normal == b.Normal) &&
            (a.Axis0 == b.Axis0) && 
            (a.Axis1 == b.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(Ellipse3d a, Ellipse3d b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Center, Normal, Axis0, Axis1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(Ellipse3d other) =>
            Center.Equals(other.Center) &&
            Normal.Equals(other.Normal) &&
            Axis0.Equals(other.Axis0) &&
            Axis1.Equals(other.Axis1);

        public override bool Equals(object other)
             => (other is Ellipse3d o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", Center, Normal, Axis0, Axis1);
        }

        public static Ellipse3d Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new Ellipse3d(
                V3d.Parse(x[0]),
                V3d.Parse(x[1]),
                V3d.Parse(x[2]),
                V3d.Parse(x[3])
            );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="Ellipse3d"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse3d a, Ellipse3d b, double tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            ApproximateEquals(a.Axis0, b.Axis0, tolerance) &&
            ApproximateEquals(a.Axis1, b.Axis1, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="Ellipse3d"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this Ellipse3d a, Ellipse3d b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

}