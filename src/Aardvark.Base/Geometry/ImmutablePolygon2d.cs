using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Globalization;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// An immutable polygon.
    /// </summary>
    public class ImmutablePolygon<T>: IImmutablePolygon<T>
    {
        /// <summary>
        /// The empty polygon (no points).
        /// </summary>
        public static readonly IImmutablePolygon<T> Empty = new ImmutablePolygon<T>(new T[0]);

        /// <summary>
        /// Vertices.
        /// </summary>
        private ImmutableList<T> m_ps = ImmutableList<T>.Empty;

        /// <summary>
        /// Creates an immutable polygon from given outline.
        /// </summary>
        public ImmutablePolygon(IEnumerable<T> outline)
        {
            if (outline == null) throw new ArgumentNullException();
            m_ps = ImmutableList.CreateRange(outline);
        }

        /// <summary>
        /// Creates an immutable polygon from given outline.
        /// </summary>
        public ImmutablePolygon(ImmutableList<T> outline)
        {
            m_ps = outline ?? throw new ArgumentNullException();
        }

        /// <summary>
        /// Gets index-th point.
        /// Index will be wrapped around if not in range [0, count).
        /// </summary>
        public T GetPoint(int index) => m_ps[this.RepairIndex(index)];

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
            => string.Format(CultureInfo.InvariantCulture, "[{0}]", string.Join(", ", m_ps.Select(x => x.ToString())));

        #region IImmutablePolygon

        /// <summary>
        /// Polygon outline.
        /// </summary>
        public IReadOnlyList<T> Points => m_ps;

        /// <summary>
        /// Gets number of vertices.
        /// </summary>
        public int Count => m_ps.Count;

        /// <summary>
        /// Returns new polygon with point added. 
        /// </summary>
        public IImmutablePolygon<T> AddPoint(T p)
            => new ImmutablePolygon<T>(m_ps.Add(p));

        /// <summary>
        /// Returns new polygon with points added. 
        /// </summary>
        public IImmutablePolygon<T> AddPoints(IEnumerable<T> points)
            => new ImmutablePolygon<T>(m_ps.AddRange(points));

        /// <summary>
        /// Returns new polygon with point replaced. 
        /// </summary>
        public IImmutablePolygon<T> SetPoint(int index, T p)
            => new ImmutablePolygon<T>(m_ps.SetItem(index, p));

        /// <summary>
        /// Returns new polygon with point p inserted at given index. 
        /// </summary>
        public IImmutablePolygon<T> InsertPoint(int index, T p)
            => new ImmutablePolygon<T>(m_ps.Insert(index, p));

        /// <summary>
        /// Returns new polygon with point removed. 
        /// </summary>
        public IImmutablePolygon<T> RemovePoint(int index)
            => new ImmutablePolygon<T>(m_ps.RemoveAt(index));

        /// <summary>
        /// Returns new polygon with points removed. 
        /// </summary>
        public IImmutablePolygon<T> RemovePoints(IEnumerable<int> indexes)
        {
            var builder = m_ps.ToBuilder();
            foreach (var index in indexes.OrderByDescending(i => i))
            {
                builder.RemoveAt(index);
            }
            return new ImmutablePolygon<T>(builder.ToImmutable());
        }

        /// <summary>
        /// Returns new polygon with transformed points.
        /// </summary>
        public IImmutablePolygon<U> Transform<U>(Func<T, U> transform)
            => new ImmutablePolygon<U>(m_ps.Select(x => transform(x)));

        #endregion
    }
}
