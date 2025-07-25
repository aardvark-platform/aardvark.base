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
using System.Xml.Serialization;

namespace Aardvark.Base;

// AUTO GENERATED CODE - DO NOT CHANGE!

#region Line3f

/// <summary>
/// A three-dimensional line with specified start and end points.
/// </summary>
[Serializable]
public partial struct Line3f : IBoundingSphere3f
{
    #region Geometric Properties

    /// <summary>
    /// P0
    /// </summary>
    [XmlIgnore]
    public V3f Origin
    {
        readonly get { return P0; }
        set { P0 = value; }
    }

    /// <summary>
    /// P1 - P0
    /// </summary>
    [XmlIgnore]
    public V3f Direction
    {
        readonly get { return P1 - P0; }
        set { P1 = P0 + value; }
    }

    public readonly Ray3f Ray3f => new(P0, P1 - P0);

    public readonly bool IsDegenerated => !Direction.Abs().AnyGreater(Constant<float>.PositiveTinyValue);

    #endregion

    #region IBoundingSphere3f Members

    public readonly Sphere3f BoundingSphere3f => new(this.ComputeCentroid(), 0.5f * Direction.Length);

    #endregion

    public readonly Line3f Flipped => new(P1, P0);
}

#endregion

#region Line3d

/// <summary>
/// A three-dimensional line with specified start and end points.
/// </summary>
[Serializable]
public partial struct Line3d : IBoundingSphere3d
{
    #region Geometric Properties

    /// <summary>
    /// P0
    /// </summary>
    [XmlIgnore]
    public V3d Origin
    {
        readonly get { return P0; }
        set { P0 = value; }
    }

    /// <summary>
    /// P1 - P0
    /// </summary>
    [XmlIgnore]
    public V3d Direction
    {
        readonly get { return P1 - P0; }
        set { P1 = P0 + value; }
    }

    public readonly Ray3d Ray3d => new(P0, P1 - P0);

    public readonly bool IsDegenerated => !Direction.Abs().AnyGreater(Constant<double>.PositiveTinyValue);

    #endregion

    #region IBoundingSphere3d Members

    public readonly Sphere3d BoundingSphere3d => new(this.ComputeCentroid(), 0.5 * Direction.Length);

    #endregion

    public readonly Line3d Flipped => new(P1, P0);
}

#endregion

