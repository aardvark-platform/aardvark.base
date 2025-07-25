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

namespace Aardvark.Base;

/// <summary>
/// Enumeration of cube faces. The integer value of the enum also gives the index of the face in a texture array representing a cubemap.
/// </summary>
public enum CubeSide
{
    PositiveX = 0,
    NegativeX = 1,
    PositiveY = 2,
    NegativeY = 3,
    PositiveZ = 4,
    NegativeZ = 5,
}

/// <summary>
/// Bit field of cube face sides. The index of a bit represents the index of the face in a texture array representing a cubemap.
/// </summary>
[Flags]
public enum CubeSideFlags
{
    PositiveX = 0x1,
    NegativeX = 0x2,
    PositiveY = 0x4,
    NegativeY = 0x8,
    PositiveZ = 0x10,
    NegativeZ = 0x20,
    All = 0x3f,
}

/// <summary>
/// A cube map consisting of six image mipmaps.
/// </summary>
public class PixCube : IPix
{
    /// <summary>
    /// Array of image mipmaps representing the cube sides.
    /// </summary>
    public PixImageMipMap[] MipMapArray;

    #region Constructors

    /// <summary>
    /// Creates a cube map from a mipmap array.
    /// The order of the array follows the <see cref="CubeSide"/> enumeration.
    /// </summary>
    /// <param name="sides">An array of mipmaps representing the sides of the mipmap.</param>
    public PixCube(PixImageMipMap[] sides)
        => MipMapArray = sides;

    /// <summary>
    /// Creates a cube map from an image array.
    /// The order of the array follows the <see cref="CubeSide"/> enumeration.
    /// </summary>
    /// <param name="sides">An array of images representing the sides of the mipmap.</param>
    public PixCube(PixImage[] sides)
        => MipMapArray = sides.Map(image => new PixImageMipMap(image));

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets an image mipmap of the sides array.
    /// </summary>
    public PixImageMipMap this[CubeSide side]
    {
        get => MipMapArray[(int)side];
        set => MipMapArray[(int)side] = value;
    }

    /// <summary>
    /// Returns the format of the cube map.
    /// </summary>
    public PixFormat PixFormat => MipMapArray[0].PixFormat;

    #endregion
}
