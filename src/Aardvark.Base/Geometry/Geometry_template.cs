using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# Func<string, Action> Sep = s => () => Out(s);
    //# { // POLYGONS
    //# var planeArray = new[] { "", "", "line", "plane" };
    //# var typeArray = new[] { "", "Polygon", "Line", "Triangle", "Quad" };
    //# var nameArray = new[] { "", "polygon", "line", "triangle", "quad" };
    //# var scaleArray = new[] { "", "", "0.5", "Constant.OneThird", "0.25" };
    //# for (int d = 2; d < 4; d++) {
    //#     var dp1 = d + 1;
    //#     var dd = d.ToString() + "d";
    //#     var di = d.ToString() + "i";
    //#     var tvec = "V" + dd;
    //#     var tveci = "V" + di;
    //#     var tmat = "M" + d + dd;
    //#     var tmat1 = "M" + dp1 + dp1 + "d";
    //#     var teucl = "Euclidean" + dd;
    //#     var tsimi = "Similarity" + dd;
    //#     var taffi = "Affine" + dd;
    //#     var tshif = "Shift" + dd;
    //#     var trot = "Rot" + dd;
    //#     var tscale = "Scale" + dd;
    //#     var plane = planeArray[d];
    //#     var tplane = "Plane" + dd;
    //#     var tline = "Line" + dd;
    //#     var tbox = "Box" + dd;
    //#     var thull = "Hull" + dd;
    //#     var ttriangle = "Triangle" + dd;
    //#     var tquad = "Quad" + dd;
    //#
    //# for (int pc = 1; pc < 5; pc++) {
    //#     var pcsub1 = pc - 1;
    //#     var type = typeArray[pc] + dd;
    //#     var name = nameArray[pc];
    //#     var isPoly = pc == 1;
    //#     var scale = scaleArray[pc]; 
    //#
    //# foreach (var isIndexed in new[] { false, true }) {
    //#     if (isIndexed && !isPoly) continue;
    //#     var indextype = isIndexed ? "Index" + type : type;
    //#
    //# if (isPoly) {
    //# if (!isIndexed) {
    #region __type__

    /// <summary>
    /// A polygon internally represented by an array of points. Implemented
    /// as a structure, the validity of the polygon can be checked via its
    /// PointCount, which must be bigger than 0 for a polygon to hold any
    /// points, and bigger than 2 for a polygon to be geometrically valid.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>, IValidity, IPolygon<__tvec__>, IBounding__tbox__
    {
        internal int m_pointCount;
        internal __tvec__[] m_pointArray;

        #region Constructors

        /// <summary>
        /// Creates a polygon from given points.
        /// </summary>
        public __type__(__tvec__[] pointArray, int pointCount)
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
        public __type__(params __tvec__[] pointArray)
        {
            m_pointCount = pointArray != null ? pointArray.Length : 0;
            m_pointArray = pointArray;
        }

        /// <summary>
        /// Creates a polygon from given points.
        /// </summary>
        public __type__(__tvec__[] pointArray, int startIndex, int count)
        {
            if (startIndex < 0 || startIndex >= pointArray.Length - 1) throw new ArgumentException();
            if (count <= 0 || startIndex + count >= pointArray.Length) throw new ArgumentException();
            m_pointCount = count;
            m_pointArray = new __tvec__[count];
            for (int i = 0; i < count; i++) m_pointArray[i] = pointArray[startIndex + i];
        }

        /// <summary>
        /// Creates a polygon from point count and point creator function.
        /// </summary>
        public __type__(int pointCount, Func<int, __tvec__> index_pointCreator)
            : this(new __tvec__[pointCount].SetByIndex(index_pointCreator))
        { }

        /// <summary>
        /// Creates a polygon from a sequence of points.
        /// </summary>
        public __type__(IEnumerable<__tvec__> points)
            : this(points.ToArray())
        { }
        
        /// <summary>
        /// Creates a polygon from the points of a pointArray that
        /// are selected by an index array.
        /// </summary>
        public __type__(int[] indexArray, __tvec__[] pointArray)
            : this(indexArray.Map(i => pointArray[i]))
        { }

        /// <summary>
        /// Creates a polygon from a triangle.
        /// </summary>
        public __type__(__ttriangle__ triangle)
            : this(triangle.GetPointArray())
        { }

        /// <summary>
        /// Creates a polygon from a quad.
        /// </summary>
        public __type__(__tquad__ quad)
            : this(quad.GetPointArray())
        { }

        /// <summary>
        /// Copy constructor.
        /// Performs deep copy of original.
        /// </summary>
        public __type__(__type__ original)
            : this(original.GetPointArray())
        { }

        #endregion

        #region Constants

        public static readonly __type__ Invalid = new __type__(null, 0);

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
        public IEnumerable<__tvec__> Points
        {
            get { for (int pi = 0; pi < m_pointCount; pi++) yield return m_pointArray[pi]; }
        }

        #endregion

        #region Conversions

        /// <summary>
        /// Returns a copy of the polygons point array.
        /// </summary>
        public __tvec__[] GetPointArray()
        {
            var pc = m_pointCount;
            var pa = new __tvec__[pc];
            for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
            return pa;
        }

        /// <summary>
        /// [P0, P1, P2] -> [P0, P1, P2, P0].
        /// </summary>
        public __tvec__[] GetPointArrayWithRepeatedFirstPoint()
        {
            var pc = m_pointCount;
            var pa = new __tvec__[pc + 1];
            for (int pi = 0; pi < pc; pi++) pa[pi] = m_pointArray[pi];
            pa[pc] = pa[0];
            return pa;
        }

        /// <summary>
        /// Returns a transformed copy of the polygons point array.
        /// </summary>
        public T[] GetPointArray<T>(Func<__tvec__, T> point_copyFun)
        {
            var pc = m_pointCount;
            var pa = new T[pc];
            for (int pi = 0; pi < pc; pi++) pa[pi] = point_copyFun(m_pointArray[pi]);
            return pa;
        }

        /// <summary>
        /// Returns a transformed copy of the polygons point array.
        /// </summary>
        public T[] GetPointArray<T>(Func<__tvec__, int, T> point_index_copyFun)
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
        public __tvec__ this[int index]
        {
            get { return m_pointArray[index]; }
            set { m_pointArray[index] = value; }
        }

        #endregion

        #region Edges and Lines

        /// <summary>
        /// Index-th edge as vector (edgeEndPos - edgeBeginPos).
        /// </summary>
        public __tvec__ Edge(int index)
        {
            var p0 = m_pointArray[index++];
            var p1 = m_pointArray[index < m_pointCount ? index : 0];
            return p1 - p0;
        }

        /// <summary>
        /// Edges as vectors (edgeEndPos - edgeBeginPos).
        /// </summary>
        public IEnumerable<__tvec__> Edges
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
        public __tline__ EdgeLine(int index)
        {
            var p0 = m_pointArray[index++];
            var p1 = m_pointArray[index < m_pointCount ? index : 0];
            return new __tline__(p0, p1);
        }

        /// <summary>
        /// Edges as line segments (edgeBeginPos, edgeEndPos).
        /// </summary>
        public IEnumerable<__tline__> EdgeLines
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
                    yield return new __tline__(p0, p1);
                    p0 = p1;
                }
                yield return new __tline__(p0, p);
            }
        }

        /// <summary>
        /// Edges as vectors (edgeEndPos - edgeBeginPos).
        /// </summary>
        public __tvec__[] GetEdgeArray()
        {
            var pc = m_pointCount;
            if (pc < 2) return new __tvec__[0];
            var edgeArray = new __tvec__[pc];
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
        public __tline__[] GetEdgeLineArray()
        {
            var pc = PointCount;
            if (pc < 2) return new __tline__[0];
            var ela = new __tline__[pc];
            var p = m_pointArray[0];
            var p0 = p;
            for (int pi = 1; pi < pc; pi++)
            {
                var p1 = m_pointArray[pi];
                ela[pi - 1] = new __tline__(p0, p1);
                p0 = p1;
            }
            ela[pc - 1] = new __tline__(p0, p);
            return ela;
        }

        #endregion

        #region Transformations

        /// <summary>
        /// Returns copy of polygon. Same as Map(p => p).
        /// </summary>
        public __type__ Copy()
        {
            return new __type__(m_pointArray.Copy());
        }

        /// <summary>
        /// Returns transformed copy of this polygon.
        /// </summary>
        public __type__ Map(Func<__tvec__, __tvec__> point_fun)
        {
            var pc = m_pointCount;
            __tvec__[] opa = m_pointArray, npa = new __tvec__[pc];
            for (int pi = 0; pi < pc; pi++) npa[pi] = point_fun(opa[pi]);
            return new __type__(npa, pc);
        }

        /// <summary>
        /// Gets copy with reversed order of vertices. 
        /// </summary>
        public __type__ Reversed
        {
            get
            {
                var pc = m_pointCount;
                __tvec__[] opa = m_pointArray, npa = new __tvec__[pc];
                for (int pi = 0, pj = pc - 1; pi < pc; pi++, pj--) npa[pi] = opa[pj];
                return new __type__(npa, pc);
            }
        }

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
        public static bool operator ==(__type__ a, __type__ b)
        {
            if (a.m_pointCount != b.m_pointCount) return false;
            for (int pi = 0; pi < a.m_pointCount; pi++)
                if (a.m_pointArray[pi] != b.m_pointArray[pi]) return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return m_pointArray.GetCombinedHashCode(m_pointCount);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
        {
            if (m_pointCount != other.m_pointCount) return false;
            for (int pi = 0; pi < m_pointCount; pi++)
                if (!m_pointArray[pi].Equals(other.m_pointArray[pi])) return false;
            return true;
        }

        public override bool Equals(object other)
            => (other is __type__ o) ? Equals(o) : false;

        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture,
                "[{0}]", Points.Select(x => x.ToString()).Join(", ")
                );
        }

        public static __type__ Parse(string s)
        {
            var va = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(va.Select(x => __tvec__.Parse(x)));
        }

        #endregion

        #region IBounding__tbox__ Members

        /// <summary>
        /// Bounding box of polygon.
        /// </summary>
        public __tbox__ Bounding__tbox__
        {
            get { return new __tbox__(m_pointArray, 0, m_pointCount); }
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b, double tolerance)
        {
            if (a.m_pointCount != b.m_pointCount) return false;
            for (int pi = 0; pi < a.m_pointCount; pi++)
                if (!ApproximateEquals(a.m_pointArray[pi], b.m_pointArray[pi], tolerance)) return false;
            return true;
        }

        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

    //# } // !isIndexed
    //# if (isIndexed) {
    #region __indextype__

    [StructLayout(LayoutKind.Sequential)]
    public partial struct __indextype__ : IValidity, IPolygon<__tvec__>
    {
        private int m_pointCount;
        private int m_firstIndex;
        private int[] m_indexArray;
        private __tvec__[] m_pointArray;

        #region Constructors

        public __indextype__(int[] indexArray, int firstIndex, int pointCount, __tvec__[] pointArray)
        {
            m_indexArray = indexArray;
            m_firstIndex = firstIndex;
            m_pointCount = pointCount;
            m_pointArray = pointArray;
        }

        public __indextype__(__tvec__[] pointArray, int firstIndex, int pointCount)
            : this(new int[pointCount].SetByIndex(i => firstIndex + i), 0, pointCount, pointArray)
        { }

        public __indextype__(__tvec__[] pointArray)
            : this(new int[pointArray.Length].SetByIndex(i => i), 0, pointArray.Length, pointArray)
        { }

        #endregion

        #region Constants

        public static readonly __indextype__ Invalid = new __indextype__(null, 0, 0, null);

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
        public __tvec__[] PointArray { get { return m_pointArray; } }

        public IEnumerable<__tvec__> Points
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

        public __tvec__ this[int index]
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
        public __tvec__[] GetPointArray()
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

    //# } // isIndexed
    #region __indextype__Extensions

    public static partial class __indextype__Extensions
    {
        //# if (isIndexed) {
        #region Conversions

        public static __type__ To__type__(this __indextype__ polygon)
        {
            return new __type__(polygon.GetPointArray());
        }

        #endregion

        //# } // isIndexed
        #region Geometric Properties

        /// <summary>
        /// The vertex centroid is the average of the vertex coordinates.
        /// </summary>
        public static __tvec__ ComputeVertexCentroid(this __indextype__ polygon)
        {
            var sum = __tvec__.Zero;
            int pc = polygon.PointCount;
            for (int i = 0; i < pc; i++) sum += polygon[i];
            var scale = 1.0 / (double)pc;
            return sum * scale;
        }

        public static double ComputePerimeter(this __indextype__ polygon)
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
        //# if (!isIndexed) {

        #region Geometric Transformations

        //# var scales = new string [] { "double", tvec };
        //# foreach (string tscalefactor in scales) {
        /// <summary>
        /// Scales the <see cref="__type__"/> by the given factor.
        /// </summary>
        /// <param name="polygon">The <see cref="__type__"/> to scale.</param>
        /// <param name="scale">The scale factor.</param>
        public static void Scale(this __type__ polygon, __tscalefactor__ scale)
        {
            for (int pi = 0; pi < polygon.m_pointCount; pi++)
                polygon.m_pointArray[pi] *= scale;
        }

        /// <summary>
        /// Returns a copy of the <see cref="__type__"/> scaled by the given factor.
        /// </summary>
        /// <param name="polygon">The <see cref="__type__"/> to scale.</param>
        /// <param name="scale">The scale factor.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Scaled(this __type__ polygon, __tscalefactor__ scale)
        {
            var result = new __type__(polygon);
            result.Scale(scale);
            return result;
        }

        /// <summary>
        /// Scales the <see cref="__type__"/> by the given factor about the given center.
        /// </summary>
        /// <param name="polygon">The <see cref="__type__"/> to scale.</param>
        /// <param name="center">The scaling center.</param>
        /// <param name="scale">The scale factor.</param>
        public static void Scale(this __type__ polygon, __tvec__ center, __tscalefactor__ scale)
        {
            for (int pi = 0; pi < polygon.m_pointCount; pi++)
                polygon.m_pointArray[pi] = center + (polygon.m_pointArray[pi] - center) * scale;
        }

        /// <summary>
        /// Returns a copy of the <see cref="__type__"/> scaled by the given factor about the given center.
        /// </summary>
        /// <param name="polygon">The <see cref="__type__"/> to scale.</param>
        /// <param name="center">The scaling center.</param>
        /// <param name="scale">The scale factor.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Scaled(this __type__ polygon, __tvec__ center, __tscalefactor__ scale)
        {
            var result = new __type__(polygon);
            result.Scale(center, scale);
            return result;
        }

        /// <summary>
        /// Scales the <see cref="__type__"/> by the given factor about the centroid.
        /// </summary>
        /// <param name="polygon">The <see cref="__type__"/> to scale.</param>
        /// <param name="scale">The scale factor.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ScaleAboutVertexCentroid(this __type__ polygon, __tscalefactor__ scale)
        {
            var center = polygon.ComputeVertexCentroid();
            polygon.Scale(center, scale);
        }

        /// <summary>
        /// Returns a copy of the <see cref="__type__"/> scaled by the given factor about the centroid.
        /// </summary>
        /// <param name="polygon">The <see cref="__type__"/> to scale.</param>
        /// <param name="scale">The scale factor.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ ScaledAboutVertexCentroid(this __type__ polygon, __tscalefactor__ scale)
        {
            var result = new __type__(polygon);
            result.ScaleAboutVertexCentroid(scale);
            return result;
        }

        //# }

        //# var transforms = new string [] { tmat1, teucl, tsimi, taffi, tshif, trot, tscale, tmat };
        //# for (int i = 0; i < transforms.Length; i++) {
        //#     var ttrafo = transforms[i];
        //#     var transform = (i < 4) ? "TransformPos" : "Transform";
        //#     var hasInv = (i != 0 && i != 3 && i != 7);
        /// <summary>
        /// Transforms the <see cref="__type__"/> by the given <see cref="__ttrafo__"/> transformation.
        /// </summary>
        /// <param name="polygon">The <see cref="__type__"/> to transform.</param>
        /// <param name="t">The transformation to apply.</param>
        public static void Transform(this __type__ polygon, __ttrafo__ t)
        {
            for (int pi = 0; pi < polygon.m_pointCount; pi++)
                polygon.m_pointArray[pi] = t.__transform__(polygon.m_pointArray[pi]);
        }

        /// <summary>
        /// Returns a copy of the <see cref="__type__"/> transformed by the given <see cref="__ttrafo__"/> transformation.
        /// </summary>
        /// <param name="polygon">The __type__ to transform.</param>
        /// <param name="t">The transformation to apply.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ Transformed(this __type__ polygon, __ttrafo__ t)
        {
            var result = new __type__(polygon);
            result.Transform(t);
            return result;
        }

        //# if (hasInv) {
        /// <summary>
        /// Transforms the <see cref="__type__"/> by the inverse of the given <see cref="__ttrafo__"/> transformation.
        /// </summary>
        /// <param name="polygon">The <see cref="__type__"/> to transform.</param>
        /// <param name="t">The transformation to apply.</param>
        public static void InvTransform(this __type__ polygon, __ttrafo__ t)
        {
            for (int pi = 0; pi < polygon.m_pointCount; pi++)
                polygon.m_pointArray[pi] = t.Inv__transform__(polygon.m_pointArray[pi]);
        }

        /// <summary>
        /// Returns a copy of the <see cref="__type__"/> transformed by the inverse of the given <see cref="__ttrafo__"/> transformation.
        /// </summary>
        /// <param name="polygon">The __type__ to transform.</param>
        /// <param name="t">The transformation to apply.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __type__ InvTransformed(this __type__ polygon, __ttrafo__ t)
        {
            var result = new __type__(polygon);
            result.InvTransform(t);
            return result;
        }

        //# }
        //# }
        public static __type__ WithoutMultiplePoints(this __type__ polygon, double eps = 1e-8)
        {
            eps *= eps;
            var opc = polygon.PointCount;
            var pa = new __tvec__[opc];
            var pc = 0;
            pa[0] = polygon[0];
            for (int pi = 1; pi < opc; pi++)
                if (Vec.DistanceSquared(pa[pc], polygon[pi]) > eps)
                    pa[++pc] = polygon[pi];
            if (Vec.DistanceSquared(pa[pc], polygon[0]) > eps)
                ++pc;
            return new __type__(pa, pc);
        }

        #endregion

        #region Clipping

        /// <summary>
        /// Clip the supplied polygon at the supplied __plane__. The method should
        /// work with all non-selfintersecting polygons. Returns all parts of 
        /// the polygon that are at the positive side of the __plane__.
        /// </summary>
        public static __type__ ConvexClipped(
                this __type__ polygon, __tplane__ __plane__, double eps = 1e-8)
        {
            var opc = polygon.PointCount;
            __tvec__[] pa = new __tvec__[opc + 1];
            var pc = 0;
            var pf = polygon[0];
            var hf = __plane__.Height(pf); bool hfp = hf > eps, hfn = hf < -eps;
            if (hf >= -eps) pa[pc++] = pf;
            var p0 = pf; var h0 = hf; var h0p = hfp; var h0n = hfn;
            for (int vi = 1; vi < opc; vi++)
            {
                var p1 = polygon[vi];
                var h1 = __plane__.Height(p1); bool h1p = h1 > eps, h1n = h1 < -eps;
                if (h0p && h1n || h0n && h1p) pa[pc++] = p0 + (p1 - p0) * (h0 / (h0 - h1));
                if (h1 >= -eps) pa[pc++] = p1;
                p0 = p1; h0 = h1; h0p = h1p; h0n = h1n;
            }
            if (h0p && hfn || h0n && hfp) pa[pc++] = p0 + (pf - p0) * (h0 / (h0 - hf));
            return new __type__(pa, pc);
        }

        /// <summary>
        /// Returns the convex polygon clipped by the set of __plane__s (defined
        /// as __tplane__s), i.e. all parts of the polygon that are at the positive 
        /// side of the __plane__s.
        /// </summary>
        public static __type__ ConvexClipped(
                this __type__ polygon, __tplane__[] __plane__s, double eps = 1e-8)
        {
            foreach (var c in __plane__s)
            {
                polygon = polygon.ConvexClipped(c, eps);
                if (polygon.PointCount == 0) break;
            }
            return polygon;
        }

        /// <summary>
        /// Returns the polygon clipped by the hull, i.e. all parts of the
        /// polygon that are at the positive side of the hull __plane__s.
        /// </summary>
        public static __type__ ConvexClipped(
                this __type__ polygon, __thull__ hull, double eps = 1e-8)
        {
            return polygon.ConvexClipped(hull.PlaneArray, eps);
        }

        /// <summary>
        /// TODO summary.
        /// </summary>
        public static __type__ ConvexClipped(
                this __type__ polygon, __tbox__ box, double eps = 1e-8)
        {
            var __plane__s = new[]
            {
                //# Meta.VecFields.Take(d).ForEach(axis => {
                new __tplane__(__tvec__.__axis__Axis, box.Min), new __tplane__(-__tvec__.__axis__Axis, box.Max),
                //# });
            };
            return polygon.ConvexClipped(__plane__s);
        }

        #endregion
        //# } // !isIndexed
    }

    #endregion

    //# } else { // !isPoly
    #region __type__

    [StructLayout(LayoutKind.Sequential)]
    public partial struct __type__ : IEquatable<__type__>, IValidity, IPolygon<__tvec__>, IBounding__tbox__
    {
        public __tvec__ /*# pc.ForEach(i => { */P__i__/*# }, Sep(", ")); */;

        #region Constructors

        /// <summary>
        /// Creates __name__ from __pc__ points.
        /// </summary>
        public __type__(/*# pc.ForEach(i => { */__tvec__ p__i__/*# }, Sep(", ")); */)
        {
            /*# pc.ForEach(i => { */P__i__ = p__i__/*# }, Sep("; ")); */;
        }

        /// <summary>
        /// Creates __name__ from first __pc__ points in the sequence.
        /// </summary>
        public __type__(IEnumerable<__tvec__> points)
        {
            var pa = points.TakeToArray(__pc__);
            /*# pc.ForEach(i => { */P__i__ = pa[__i__]/*# }, Sep("; ")); */;
        }

        //# if (pc == 4) {
        /// <summary>
        /// Creates quad from point and two vectors representing edges.
        /// </summary>
        public __type__(__tvec__ p0, __tvec__ edge01, __tvec__ edge03)
        {
            P0 = p0;
            P1 = p0 + edge01;
            P2 = P1 + edge03;
            P3 = p0 + edge03;
        }

        //# }
        #endregion

        #region Properties

        public bool IsValid { get { return true; } }
        public bool IsInvalid { get { return false; } }

        //# if (pc > 2) {
        //# pc.ForEach(i => { foreach (var j in new[] { (i + 1) % pc, (i + pcsub1) % pc }) {
        /// <summary>
        /// Edge P__j__ - P__i__
        /// </summary>
        public __tvec__ Edge__i____j__ { get { return P__j__ - P__i__; } }
        //# } });

        public IEnumerable<__tvec__> Edges
        {
            get
            {
                //# pc.ForEach(i => { var j = (i + 1) % pc;
                yield return P__j__ - P__i__;
                //# });
            }
        }

        public __tvec__[] EdgeArray
        {
            get
            {
                var a = new __tvec__[__pc__];
                //# pc.ForEach(i => { var j = (i + 1) % pc;
                a[__i__] = P__j__ - P__i__;
                //# });
                return a;
            }
        }

        public IEnumerable<__tline__> EdgeLines
        {
            get
            {
                //# pc.ForEach(i => { var j = (i + 1) % pc;
                yield return new __tline__(P__i__, P__j__);
                //# });
            }
        }

        public __tline__[] EdgeLineArray
        {
            get
            {
                var a = new __tline__[__pc__];
                //# pc.ForEach(i => { var j = (i + 1) % pc;
                a[__i__] = new __tline__(P__i__, P__j__);
                //# });
                return a;
            }
        }

        public __tline__ GetEdgeLine(int index)
        {
            switch (index)
            {
                //# pc.ForEach(i => { var j = (i + 1) % pc;
                case __i__: return new __tline__(P__i__, P__j__);
                //# });
            }
            throw new InvalidOperationException();
        }

        public __tvec__ GetEdge(int index)
        {
            switch (index)
            {
                //# pc.ForEach(i => { var j = (i + 1) % pc;
                case __i__: return P__j__ - P__i__;
                //# });
            }
            throw new InvalidOperationException();
        }

        //# pc.ForEach(i => { foreach (var j in new[] { (i + 1) % pc, (i + pcsub1) % pc }) {
        public __tline__ Line__i____j__ { get { return new __tline__(P__i__, P__j__); } }
        //# } });

        //# } // pc > 2
        public int PointCount { get { return __pc__; } }

        public IEnumerable<__tvec__> Points
        {
            get { /*# pc.ForEach(i => { */yield return P__i__/*# }, Sep("; ")); */; }
        }

        public __type__ Reversed
        {
            get { return new __type__(/*# pc.ForEach(i => { var j = pcsub1 - i; */P__j__/*# }, Sep(", ")); */); }
        }

        #endregion

        #region Indexer

        public __tvec__ this[int index]
        {
            get
            {
                switch (index)
                {
                    //# pc.ForEach(i => {
                    case __i__: return P__i__;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }
            set
            {
                switch (index)
                {
                    //# pc.ForEach(i => {
                    case __i__: P__i__ = value; return;
                    //# });
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        #endregion

        #region Transformations

        public __type__ Copy(Func<__tvec__, __tvec__> point_copyFun)
        {
            return new __type__(/*# pc.ForEach(i => { */point_copyFun(P__i__)/*# }, Sep(", ")); */);
        }

        //# for (int od = 2; od < 4; od++) { if (d == od) continue;
        //#     var otype = typeArray[pc] + od + "d"; var otvec = "V" + od + "d";
        public __otype__ To__otype__(Func<__tvec__, __otvec__> point_copyFun)
        {
            return new __otype__(/*# pc.ForEach(i => { */point_copyFun(P__i__)/*# }, Sep(", ")); */);
        }

        //# } // od
        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__type__ a, __type__ b)
            => /*# pc.ForEach(i => { */(a.P__i__ == b.P__i__)/*# }, Sep(" && ")); */;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__type__ a, __type__ b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(/*# pc.ForEach(i => { */P__i__/*# }, Sep(", ")); */);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__type__ other)
            => /*# pc.ForEach(i => { */P__i__.Equals(other.P__i__)/*# }, Sep(" && ")); */;

        public override bool Equals(object other)
             => (other is __type__ o) ? Equals(o) : false;

        public override string ToString()
        {
            //# var format = "["; pc.ForEach(i => format += "{" + i + "}", () => format += ", "); format += "]";
            return string.Format(CultureInfo.InvariantCulture, "__format__", /*# pc.ForEach(i => { */P__i__/*# }, Sep(", ")); */);
        }

        public static __type__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __type__(/*# pc.ForEach(i => { */__tvec__.Parse(x[__i__])/*# }, Sep(", ")); */);
        }

        #endregion

        #region IBounding__tbox__ Members

        public __tbox__ Bounding__tbox__
        {
            get
            {
                return new __tbox__(/*# pc.ForEach(i => { */P__i__/*# }, Sep(", "));
                                        */)/*#if (pc == 2) {*/.Repair()/*# } */;
            }
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b, double tolerance)
            => /*# pc.ForEach(i => { */ApproximateEquals(a.P__i__, b.P__i__, tolerance)/*# }, Sep(" && ")); */;

        /// <summary>
        /// Returns whether the given <see cref="__type__"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __type__ a, __type__ b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

    #region __type__Extensions

    public static partial class __type__Extensions
    {
        public static __tvec__[] GetPointArray(this __type__ __name__)
        {
            var pa = new __tvec__[__pc__];
            //# pc.ForEach(i => {
            pa[__i__] = __name__.P__i__;
            //# });
            return pa;
        }

        public static __tvec__ ComputeCentroid(this __type__ __name__)
        {
            return __scale__ * (/*# pc.ForEach(i => { */__name__.P__i__/*# }, Sep(" + ")); */);
        }
    }

    #endregion

    //# } // !isPoly
    //# } // isIndexed
    //# } // pc
    //# } // d
    //# } // POLYGONS
    //# { // ELLIPSES
    //# for (int d = 2; d < 4; d++) {
    //#     var vt = "V" + d + "d";
    //#     var et = "Ellipse" + d + "d";
    #region __et__

    /// <summary>
    /// A __d__D ellipse is defined by its center/*# if (d == 3) { */, its plane normal,/*# } */
    /// and two half-axes.
    /// Note that in principle any two conjugate half-diameters
    /// can be used as axes, however some algorithms require that
    /// the major and minor half axes are known. By convention
    /// in this case, axis0 is the major half axis.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct __et__
    {
        public __vt__ Center;
        //# if (d == 3) {
        public __vt__ Normal;
        //# }
        public __vt__ Axis0;
        public __vt__ Axis1;

        #region Constructors

        public __et__(__vt__ center, /*# if (d == 3) { */__vt__ normal, /*# } */__vt__ axis0, __vt__ axis1)
        {
            Center = center;
            //# if (d == 3) {
            Normal = normal;
            //# }
            Axis0 = axis0;
            Axis1 = axis1;
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// </summary>
        public static __et__ FromConjugateDiameters(__vt__ center, /*# if (d == 3) { */__vt__ normal, /*# } */__vt__ a, __vt__ b)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2)   return new __et__(center, /*# if (d == 3) { */normal, /*# } */a, b);
                else            return new __et__(center, /*# if (d == 3) { */normal, /*# } */b, a);
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                __vt__ v0 = a * ct + b * st, v1 = b * ct - a * st;
                if (v0.LengthSquared >= v1.LengthSquared)   return new __et__(center, /*# if (d == 3) { */normal, /*# } */v0, v1);
                else                                        return new __et__(center, /*# if (d == 3) { */normal, /*# } */v1, v0);
            }
        }

        /// <summary>
        /// Construct ellipse from two conjugate diameters, and set
        /// Axis0 to the major axis and Axis1 to the minor axis.
        /// The algorithm was constructed from first principles.
        /// Also computes the squared lengths of the major and minor
        /// half axis.
        /// </summary>
        public static __et__ FromConjugateDiameters(__vt__ center, /*# if (d == 3) { */__vt__ normal, /*# } */__vt__ a, __vt__ b,
                out double major2, out double minor2)
        {
            var ab = Vec.Dot(a, b);
            double a2 = a.LengthSquared, b2 = b.LengthSquared;
            if (ab.IsTiny())
            {
                if (a2 >= b2)   { major2 = a2; minor2 = b2; return new __et__(center, /*# if (d == 3) { */normal, /*# } */a, b); }
                else            { major2 = b2; minor2 = a2; return new __et__(center, /*# if (d == 3) { */normal, /*# } */b, a); }
            }
            else
            {
                var t = 0.5 * Fun.Atan2(2 * ab, a2 - b2);
                double ct = Fun.Cos(t), st = Fun.Sin(t);
                __vt__ v0 = a * ct + b * st, v1 = b * ct - a * st;
                a2 = v0.LengthSquared; b2 = v1.LengthSquared;
                if (a2 >= b2)   { major2 = a2; minor2 = b2; return new __et__(center, /*# if (d == 3) { */normal, /*# } */v0, v1); }
                else            { major2 = b2; minor2 = a2; return new __et__(center, /*# if (d == 3) { */normal, /*# } */v1, v0); }
            }
        }

        #endregion

        #region Constants

        public static readonly __et__ Zero = new __et__(__vt__.Zero, /*# if (d == 3) { */__vt__.Zero, /*# } */__vt__.Zero, __vt__.Zero);

        #endregion

        #region Operations

        public __vt__ GetVector(double alpha)
        {
            return Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);
        }

        public __vt__ GetPoint(double alpha)
        {
            return Center + Axis0 * Fun.Cos(alpha) + Axis1 * Fun.Sin(alpha);
        }

        /// <summary>
        /// Perform the supplied action for each of count vectors from the center
        /// of the ellipse to the circumference. 
        /// </summary>
        public void ForEachVector(int count, Action<int, __vt__> index_vector_act)
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
        public __vt__[] GetPoints(int count)
        {
            var array = new __vt__[count];
            var c = Center;
            ForEachVector(count, (i, v) => array[i] = c + v);
            return array;
        }

        #endregion

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator ==(__et__ a, __et__ b) => 
            (a.Center == b.Center) && 
            //# if (d == 3) {
            (a.Normal == b.Normal) &&
            //# }
            (a.Axis0 == b.Axis0) && 
            (a.Axis1 == b.Axis1);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool operator !=(__et__ a, __et__ b)
            => !(a == b);

        #endregion

        #region Overrides

        public override int GetHashCode()
        {
            return HashCode.GetCombined(Center, /*# if (d == 3) { */Normal, /*# }*/Axis0, Axis1);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Equals(__et__ other) =>
            Center.Equals(other.Center) &&
            //# if (d == 3) {
            Normal.Equals(other.Normal) &&
            //# }
            Axis0.Equals(other.Axis0) &&
            Axis1.Equals(other.Axis1);

        public override bool Equals(object other)
             => (other is __et__ o) ? Equals(o) : false;

        public override string ToString()
        {
            //# if (d == 3) {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}, {3}]", Center, Normal, Axis0, Axis1);
            //# } else {
            return string.Format(CultureInfo.InvariantCulture, "[{0}, {1}, {2}]", Center, Axis0, Axis1);
            //# }
        }

        public static __et__ Parse(string s)
        {
            var x = s.NestedBracketSplitLevelOne().ToArray();
            return new __et__(
                __vt__.Parse(x[0]),
                __vt__.Parse(x[1]),
                __vt__.Parse(x[2])/*# if (d == 3) { */,
                __vt__.Parse(x[3])/*# }*/
            );
        }

        #endregion
    }

    public static partial class Fun
    {
        /// <summary>
        /// Returns whether the given <see cref="__et__"/> are equal within the given tolerance.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __et__ a, __et__ b, double tolerance) =>
            ApproximateEquals(a.Center, b.Center, tolerance) &&
            //# if (d == 3) {
            ApproximateEquals(a.Normal, b.Normal, tolerance) &&
            //# }
            ApproximateEquals(a.Axis0, b.Axis0, tolerance) &&
            ApproximateEquals(a.Axis1, b.Axis1, tolerance);

        /// <summary>
        /// Returns whether the given <see cref="__et__"/> are equal within
        /// Constant&lt;double&gt;.PositiveTinyValue.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ApproximateEquals(this __et__ a, __et__ b)
            => ApproximateEquals(a, b, Constant<double>.PositiveTinyValue);
    }

    #endregion

    //# } // d
    //# } // ELLIPSES
}