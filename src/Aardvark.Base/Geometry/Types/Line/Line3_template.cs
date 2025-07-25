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

//# foreach (var isDouble in new[] { false, true }) {
//#   var ftype = isDouble ? "double" : "float";
//#   var ftype2 = isDouble ? "float" : "double";
//#   var tc = isDouble ? "d" : "f";
//#   var tc2 = isDouble ? "f" : "d";
//#   var type = "Line3" + tc;
//#   var type2 = "Line3" + tc2;
//#   var v2t = "V2" + tc;
//#   var ray3t = "Ray3" + tc;
//#   var plane3t = "Plane3" + tc;
//#   var sphere3t = "Sphere3" + tc;
//#   var v3t = "V3" + tc;
//#   var boundingbox3t = "BoundingBox3" + tc;
//#   var iboundingsphere = "IBoundingSphere3" + tc;
//#   var half = isDouble ? "0.5" : "0.5f";
//#   var eps = isDouble ? "1e-9" : "1e-5f";
#region __type__

/// <summary>
/// A three-dimensional line with specified start and end points.
/// </summary>
[Serializable]
public partial struct __type__ : __iboundingsphere__
{
    #region Geometric Properties

    /// <summary>
    /// P0
    /// </summary>
    [XmlIgnore]
    public __v3t__ Origin
    {
        readonly get { return P0; }
        set { P0 = value; }
    }

    /// <summary>
    /// P1 - P0
    /// </summary>
    [XmlIgnore]
    public __v3t__ Direction
    {
        readonly get { return P1 - P0; }
        set { P1 = P0 + value; }
    }

    public readonly __ray3t__ __ray3t__ => new(P0, P1 - P0);

    public readonly bool IsDegenerated => !Direction.Abs().AnyGreater(Constant<__ftype__>.PositiveTinyValue);

    #endregion

    #region __iboundingsphere__ Members

    public readonly __sphere3t__ BoundingSphere3__tc__ => new(this.ComputeCentroid(), __half__ * Direction.Length);

    #endregion

    public readonly __type__ Flipped => new(P1, P0);
}

#endregion

//# }
