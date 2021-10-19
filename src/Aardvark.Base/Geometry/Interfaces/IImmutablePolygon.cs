using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    /// <summary>
    /// An immutable polygon.
    /// </summary>
    public interface IImmutablePolygon<T>
    {
        /// <summary>
        /// Polygon outline.
        /// </summary>
        IReadOnlyList<T> Points { get; }

        /// <summary>
        /// Gets number of vertices.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns new polygon with point added. 
        /// </summary>
        IImmutablePolygon<T> AddPoint(T p);

        /// <summary>
        /// Returns new polygon with points added. 
        /// </summary>
        IImmutablePolygon<T> AddPoints(IEnumerable<T> points);

        /// <summary>
        /// Returns new polygon with point replaced. 
        /// </summary>
        IImmutablePolygon<T> SetPoint(int index, T p);

        /// <summary>
        /// Returns new polygon with point p inserted at given index. 
        /// </summary>
        IImmutablePolygon<T> InsertPoint(int index, T p);

        /// <summary>
        /// Returns new polygon with point removed. 
        /// </summary>
        IImmutablePolygon<T> RemovePoint(int index);

        /// <summary>
        /// Returns new polygon with points removed. 
        /// </summary>
        IImmutablePolygon<T> RemovePoints(IEnumerable<int> indexes);

        /// <summary>
        /// Returns new polygon with transformed points.
        /// </summary>
        IImmutablePolygon<U> Transform<U>(Func<T, U> transform);
    }
}
