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

public interface ICameraView
{
    /// <summary>
    /// Gets or sets view trafo, which transforms world space into camera space.
    /// 
    /// In camera space the camera is placed at the origin looking
    /// down the negative z-axis, with the y-axis pointing upwards
    /// and the x-axis pointing to the right.
    /// </summary>
    Trafo3d ViewTrafo { get; set; }

    /// <summary>
    /// Changes of ViewTrafo.
    /// </summary>
    IEvent<Trafo3d> ViewTrafos { get; }

    /// <summary>
    /// Gets or sets camera location in world space (origin in camera space).
    /// </summary>
    V3d Location { get; set; }

    /// <summary>
    /// Gets or sets camera right vector in world space (+x in camera space).
    /// </summary>
    V3d Right { get; set; }

    /// <summary>
    /// Gets or sets camera up vector in world space (+y in camera space).
    /// </summary>
    V3d Up { get; set; }

    /// <summary>
    /// Gets or sets camera forward direction in world space (-z in camera space).
    /// </summary>
    V3d Forward { get; set; }

    /// <summary>
    /// Sets location and axes in a single transaction.
    /// </summary>
    void Set(V3d location, V3d right, V3d up, V3d forward);
}
