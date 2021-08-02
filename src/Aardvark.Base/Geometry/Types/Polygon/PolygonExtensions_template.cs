using System;
using System.Collections.Generic;

namespace Aardvark.Base
{
    public static class IPolygonExtensions
    {
        // AUTO GENERATED CODE - DO NOT CHANGE!

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

        //# foreach (var isDouble in new[] { false, true }) {
        //#   var ftype = isDouble ? "double" : "float";
        //#   var tc = isDouble ? "d" : "f";
        //#   var polygon2t = "Polygon2" + tc;
        //#   var polygon3t = "Polygon3" + tc;
        //#   var v2t = "V2" + tc;
        //#   var v3t = "V3" + tc;
        #region __polygon2t__

        public static __polygon2t__ To__polygon2t__<T>(
                this IPolygon<T> polygon, Func<T, __v2t__> point_copyFun)
        {
            var pc = polygon.PointCount;
            var pointArray = new __v2t__[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_copyFun(polygon[pi]);
            return new __polygon2t__(pointArray);
        }

        public static __polygon2t__ To__polygon2t__<T>(
                this IPolygon<T> polygon, Func<T, int, __v2t__> point_index_copyFun)
        {
            var pc = polygon.PointCount;
            var pointArray = new __v2t__[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_index_copyFun(polygon[pi], pi);
            return new __polygon2t__(pointArray);
        }

        #endregion

        #region __polygon3t__

        public static __polygon3t__ To__polygon3t__<T>(
                this IPolygon<T> polygon, Func<T, __v3t__> point_copyFun)
        {
            var pc = polygon.PointCount;
            var pointArray = new __v3t__[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_copyFun(polygon[pi]);
            return new __polygon3t__(pointArray);
        }

        public static __polygon3t__ To__polygon3t__<T>(
                this IPolygon<T> polygon, Func<T, int, __v3t__> point_index_copyFun)
        {
            var pc = polygon.PointCount;
            var pointArray = new __v3t__[pc];
            for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_index_copyFun(polygon[pi], pi);
            return new __polygon3t__(pointArray);
        }

        #endregion

        //# }
        #endregion
    }
}
