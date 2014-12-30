using System.Collections.Generic;

namespace Aardvark.Base
{
    /// <summary>
    /// An immutable polygon.
    /// </summary>
    public interface IImmutablePolygon2d
    {
        /// <summary>
        /// Polygon outline.
        /// </summary>
        IReadOnlyList<V2d> Points { get; }

        /// <summary>
        /// Gets number of vertices.
        /// </summary>
        int Count { get; }

        /// <summary>
        /// Returns new polygon with point added. 
        /// </summary>
        IImmutablePolygon2d AddPoint(V2d p);

        /// <summary>
        /// Returns new polygon with points added. 
        /// </summary>
        IImmutablePolygon2d AddPoints(IEnumerable<V2d> points);

        /// <summary>
        /// Returns new polygon with point replaced. 
        /// </summary>
        IImmutablePolygon2d SetPoint(int index, V2d p);

        /// <summary>
        /// Returns new polygon with point p inserted at given index. 
        /// </summary>
        IImmutablePolygon2d InsertPoint(int index, V2d p);

        /// <summary>
        /// Returns new polygon with point removed. 
        /// </summary>
        IImmutablePolygon2d RemovePoint(int index);

        /// <summary>
        /// Returns new polygon with points removed. 
        /// </summary>
        IImmutablePolygon2d RemovePoints(IEnumerable<int> indexes);

        /// <summary>
        /// Returns new polygon with transformed points.
        /// </summary>
        IImmutablePolygon2d Transform(M33d trafo);
    }
}
