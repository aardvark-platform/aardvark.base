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
