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
namespace Aardvark.Base.Coder;

/// <summary>
/// Implement this to awake an object from decoding after all fields of
/// the object have already been decoded. This is normally used to
/// initialize member variables that serve as caches.
/// </summary>
public interface IAwakeable
{
    /// <summary>
    /// This method is called directly after an object has been
    /// deserialized. It get the version of the coded object as a
    /// parameter.
    /// </summary>
    void Awake(int codedVersion);
}
