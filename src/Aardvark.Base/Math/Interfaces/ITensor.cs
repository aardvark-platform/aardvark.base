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
namespace Aardvark.Base;

/// <summary>
/// Non-generic Interface for Dim.X x Dim.Y x Dim.Z, Dim.W - dimensional volumes.
/// </summary>
public interface ITensor4
{
    V4l Dim { get; }
    object GetValue(long x, long y, long z, long w);
    void SetValue(object value, long x, long y, long z, long w);
    object GetValue(V4l v);
    void SetValue(object value, V4l v);
}

/// <summary>
/// Non-generic Interface for arbitrarily sized tensors.
/// </summary>
public interface ITensor
{
    long[] Dim { get; }
    object GetValue(params long[] v);
    void SetValue(object value, params long[] v);
}

/// <summary>
/// Generic Interface for NxMxLXK-dimensional volume of Type T.
/// The indexer of this interface has arguments of type long.
/// </summary>
public interface ITensor4<T> : ITensor4
{
    T this[long x, long y, long z, long w] { get; set; }
    T this[V4l v] { get; set; }
}

/// <summary>
/// Generic Interface for arbitrarily sized tensors of Type T.
/// The indexer of this interface has arguments of type long.
/// </summary>
public interface ITensor<T> : ITensor
{
    int Rank { get; }
    T this[params long[] v] { get; set; }
}
