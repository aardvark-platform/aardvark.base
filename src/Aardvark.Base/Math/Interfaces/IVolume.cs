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
/// Non-generic Interface for Dim.X x Dim.Y x Dim.Z - dimensional volumes.
/// </summary>
public interface IVolume
{
    V3l Dim { get; }
    object GetValue(long x, long y, long z);
    void SetValue(object value, long x, long y, long z);
    object GetValue(V3l v);
    void SetValue(object value, V3l v);
}

/// <summary>
/// Generic Interface for NxMxL-dimensional volume of Type T.
/// The indexer of this interface has arguments of type long.
/// </summary>
public interface IVolume<T> : IVolume
{
    T this[long x, long y, long z] { get; set; }
    T this[V3l v] { get; set; }
}
