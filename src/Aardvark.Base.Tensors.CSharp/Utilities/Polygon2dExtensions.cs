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
using System.Collections.Generic;

namespace Aardvark.Base;

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
