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
using System;
using System.Collections.Generic;

namespace Aardvark.Base;

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

    #region Polygon2f

    public static Polygon2f ToPolygon2f<T>(
            this IPolygon<T> polygon, Func<T, V2f> point_copyFun)
    {
        var pc = polygon.PointCount;
        var pointArray = new V2f[pc];
        for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_copyFun(polygon[pi]);
        return new Polygon2f(pointArray);
    }

    public static Polygon2f ToPolygon2f<T>(
            this IPolygon<T> polygon, Func<T, int, V2f> point_index_copyFun)
    {
        var pc = polygon.PointCount;
        var pointArray = new V2f[pc];
        for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_index_copyFun(polygon[pi], pi);
        return new Polygon2f(pointArray);
    }

    #endregion

    #region Polygon3f

    public static Polygon3f ToPolygon3f<T>(
            this IPolygon<T> polygon, Func<T, V3f> point_copyFun)
    {
        var pc = polygon.PointCount;
        var pointArray = new V3f[pc];
        for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_copyFun(polygon[pi]);
        return new Polygon3f(pointArray);
    }

    public static Polygon3f ToPolygon3f<T>(
            this IPolygon<T> polygon, Func<T, int, V3f> point_index_copyFun)
    {
        var pc = polygon.PointCount;
        var pointArray = new V3f[pc];
        for (int pi = 0; pi < pc; pi++) pointArray[pi] = point_index_copyFun(polygon[pi], pi);
        return new Polygon3f(pointArray);
    }

    #endregion

    #region Polygon2d

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

    #endregion

    #region Polygon3d

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

    #endregion
}
