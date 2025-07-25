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
/// Position (x,y) in normalized device coordinates.
/// The canonical view volume is defined as [(-1, -1, 0), (+1, +1, 1)].
/// </summary>
public readonly struct Ndc2d
{
    /// <summary>
    /// Normalized device coordinates.
    /// </summary>
    public readonly V2d Position;

    /// <summary>
    /// Position (xy) in normalized device coordinates.
    /// </summary>
    public Ndc2d(V2d position)
    {
        Position = position;
    }

    /// <summary>
    /// Position (xy) in normalized device coordinates.
    /// </summary>
    public Ndc2d(double x, double y)
    {
        Position = new V2d(x, y);
    }

    /// <summary>
    /// Position (xy) in normalized device coordinates.
    /// </summary>
    public Ndc2d(PixelPosition p)
    {
        var np = p.NormalizedPosition;
        Position = new V2d(
            -1.0 + np.X * 2.0,
            +1.0 - np.Y * 2.0
            );
    }

    /// <summary>
    /// Transform the normalized device coordinate to a [0, 1] texture coordinate (flipping Y).
    /// </summary>
    public V2d TextureCoordinate
    {
        get
        {
            return new V2d(Position.X * 0.5 + 0.5, -Position.Y * 0.5 + 0.5);
        }
    }
}

/// <summary>
/// Position (xyz) in normalized device coordinates.
/// The canonical view volume is defined as [(-1, -1, 0), (+1, +1, 1)].
/// </summary>
public readonly struct Ndc3d
{
    /// <summary>
    /// Normalized device coordinates.
    /// </summary>
    public readonly V3d Position;

    /// <summary>
    /// Position (xyz) in normalized device coordinates.
    /// </summary>
    public Ndc3d(V3d position)
    {
        Position = position;
    }

    /// <summary>
    /// Position (xy) in normalized device coordinates.
    /// </summary>
    public Ndc3d(double x, double y, double z)
    {
        Position = new V3d(x, y, z);
    }

    /// <summary>
    /// Transform the normalized device coordinate to a [0, 1] texture coordinate (flipping Y) with depth.
    /// </summary>
    public V3d TextureCoordinate
    {
        get
        {
            return new V3d(Position.X * 0.5 + 0.5, -Position.Y * 0.5 + 0.5, Position.Z);
        }
    }
}
