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
/// Non-generic Interface for Dim - dimensional vectors.
/// 
/// </summary>
public interface IVector
{
    long Dim { get; }
    object GetValue(long index);
    void SetValue(object value, long index);
}

/// <summary>
/// Generic Interface for N-dimensional vector of Type T.
/// The indexer of this interface has arguments of type long.
/// </summary>
public interface IVector<T> : IVector
{
    T this[long i] { get; set; }
}
