using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// An immutable polygon.
    /// </summary>
    public class ImmutablePolygon2d : IImmutablePolygon2d
    {
        /// <summary>
        /// The empty polygon (no points).
        /// </summary>
        public static readonly ImmutablePolygon2d Empty = new ImmutablePolygon2d(new V2d[0]);

        /// <summary>
        /// Vertices.
        /// </summary>
        private ImmutableList<V2d> m_ps = ImmutableList<V2d>.Empty;

        /// <summary>
        /// Creates an immutable polygon from given outline.
        /// </summary>
        public ImmutablePolygon2d(IEnumerable<V2d> outline)
        {
            if (outline == null) throw new ArgumentNullException();
            m_ps = ImmutableList.CreateRange(outline);
        }

        /// <summary>
        /// Creates an immutable polygon from given outline.
        /// </summary>
        public ImmutablePolygon2d(ImmutableList<V2d> outline)
        {
            if (outline == null) throw new ArgumentNullException();
            m_ps = outline;
        }

        /// <summary>
        /// Gets index-th point.
        /// Index will be wrapped around if not in range [0, count).
        /// </summary>
        public V2d GetPoint(int index)
        {
            return m_ps[this.RepairIndex(index)];
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return string.Format("[{0}]", string.Join(", ", m_ps.Select(x => x.ToString())));
        }

        #region IImmutablePolygon

        /// <summary>
        /// Polygon outline.
        /// </summary>
        public IReadOnlyList<V2d> Points { get { return m_ps; } }

        /// <summary>
        /// Gets number of vertices.
        /// </summary>
        public int Count { get { return m_ps.Count; } }

        /// <summary>
        /// Returns new polygon with point added. 
        /// </summary>
        public IImmutablePolygon2d AddPoint(V2d p)
        {
            return new ImmutablePolygon2d(m_ps.Add(p));
        }

        /// <summary>
        /// Returns new polygon with points added. 
        /// </summary>
        public IImmutablePolygon2d AddPoints(IEnumerable<V2d> points)
        {
            return new ImmutablePolygon2d(m_ps.AddRange(points));
        }

        /// <summary>
        /// Returns new polygon with point replaced. 
        /// </summary>
        public IImmutablePolygon2d SetPoint(int index, V2d p)
        {
            return new ImmutablePolygon2d(m_ps.SetItem(index, p));
        }

        /// <summary>
        /// Returns new polygon with point p inserted at given index. 
        /// </summary>
        public IImmutablePolygon2d InsertPoint(int index, V2d p)
        {
            return new ImmutablePolygon2d(m_ps.Insert(index, p));
        }

        /// <summary>
        /// Returns new polygon with point removed. 
        /// </summary>
        public IImmutablePolygon2d RemovePoint(int index)
        {
            return new ImmutablePolygon2d(m_ps.RemoveAt(index));
        }

        /// <summary>
        /// Returns new polygon with points removed. 
        /// </summary>
        public IImmutablePolygon2d RemovePoints(IEnumerable<int> indexes)
        {
            var builder = m_ps.ToBuilder();
            foreach (var index in indexes.OrderByDescending(i => i))
            {
                builder.RemoveAt(index);
            }
            return new ImmutablePolygon2d(builder.ToImmutable());
        }

        /// <summary>
        /// Returns new polygon with transformed points.
        /// </summary>
        public IImmutablePolygon2d Transform(M33d trafo)
        {
            return new ImmutablePolygon2d(m_ps.Select(x => trafo.TransformPos(x)));
        }

        #endregion
    }

}
