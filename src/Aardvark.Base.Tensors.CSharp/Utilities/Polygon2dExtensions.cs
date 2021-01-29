using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base
{
    public static class TensorPolygon2dExtensions
    {
        #region Rasterization

        /// <summary>
        /// Rasterizes an array of polygons into a matrix of given size.
        /// First polygon is rasterized with label 1, second polygon with label 2, and so on.
        /// </summary>
        /// <param name="polygons">Polygons to rasterize.</param>
        /// <param name="resolution">Resolution of result matrix.</param>
        /// <param name="bb"></param>
        public static Matrix<int> RasterizeAsLabels(this IEnumerable<Polygon2d> polygons, V2i resolution, Box2d bb)
        {
            var matrix = new Matrix<int>(resolution);
            var scale = (V2d)resolution / bb.Size;
            int i = 1;
            foreach (var poly in polygons)
            {
                RasterizePolygon(poly.ToPolygon2d(p => (p - bb.Min) * scale), matrix, i++);
            }
            return matrix;
        }

        /// <summary>
        /// Rasterizes this polygon into a given matrix using the given value (label).
        /// </summary>
        public static void RasterizePolygon(Polygon2d polygon, Matrix<int> intoMatrix, int value)
        {
            var lines = polygon.GetEdgeLineArray();
            for (int y = 0; y < intoMatrix.Size.Y; y++)
            {
                var scanLine = new Line2d(new V2d(0, y), new V2d(intoMatrix.Size.X, y));
                var intersections = new List<int>();
                foreach (var l in lines)
                {
                    if (scanLine.Intersects(l, out V2d p))
                        intersections.Add((int)(p.X + 0.5));
                }
                intersections.Sort();

                if (intersections.Count % 2 == 1) intersections.Add((int)intoMatrix.Size.X);
                for (int i = 0; i < intersections.Count; i += 2)
                {
                    for (int x = intersections[i]; x < intersections[i + 1]; x++)
                        intoMatrix[x, y] = value;
                }
            }
        }

        #endregion
    }
}
