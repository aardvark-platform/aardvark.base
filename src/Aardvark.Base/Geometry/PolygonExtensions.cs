using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class IPolygonExtensions
    {
        #region Conversions

        public static IEnumerable<T> GetPoints<T>(this IPolygon<T> polygon)
        {
            var pc = polygon.PointCount;
            for (int pi = 0; pi < pc; pi++) yield return polygon[pi];
        }

        public static T[] GetPointArray<T>(this IPolygon<T> polygon)
        {
            var pc = polygon.PointCount;
            var pointArray = new T[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = polygon[pi];
            return pointArray;
        }

        public static TCopy[] GetPointArray<TCopy, T>(
                this IPolygon<T> polygon, Func<T, TCopy> point_copyFun)
        {
            var pc = polygon.PointCount;
            var pointArray = new TCopy[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_copyFun(polygon[pi]);
            return pointArray;
        }

        public static Polygon2d ToPolygon2d<T>(
                this IPolygon<T> polygon, Func<T, V2d> point_copyFun)
        {
            var pc = polygon.PointCount;
            var pointArray = new V2d[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_copyFun(polygon[pi]);
            return new Polygon2d(pointArray);
        }

        public static Polygon2d ToPolygon2d<T>(
                this IPolygon<T> polygon, Func<T, int, V2d> point_index_copyFun)
        {
            var pc = polygon.PointCount;
            var pointArray = new V2d[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_index_copyFun(polygon[pi], pi);
            return new Polygon2d(pointArray);
        }

        public static Polygon3d ToPolygon3d<T>(
                this IPolygon<T> polygon, Func<T, V3d> point_copyFun)
        {
            var pc = polygon.PointCount;
            var pointArray = new V3d[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_copyFun(polygon[pi]);
            return new Polygon3d(pointArray);
        }

        public static Polygon3d ToPolygon3d<T>(
                this IPolygon<T> polygon, Func<T, int, V3d> point_index_copyFun)
        {
            var pc = polygon.PointCount;
            var pointArray = new V3d[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_index_copyFun(polygon[pi], pi);
            return new Polygon3d(pointArray);
        }

        #endregion
    }
}
