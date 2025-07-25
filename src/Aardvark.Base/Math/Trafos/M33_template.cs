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

//# foreach (var isDouble in new[] { false, true }) {
//#   var ft = isDouble ? "double" : "float";
//#   var x2t = isDouble ? "2d" : "2f";
//#   var x3t = isDouble ? "3d" : "3f";
//#   var x4t = isDouble ? "4d" : "4f";
public partial struct M3__x3t__
{
    #region Coordinate-System Transforms

    /// <summary>
    /// Creates an orthonormal basis from the given normal as z-axis.
    /// The resulting matrix transforms from the local to the global coordinate system.
    /// The normal is expected to be normalized.
    /// 
    /// The implementation is based on:
    /// Building an Orthonormal Basis, Revisited, by Duff et al. 2017
    /// </summary>
    public static M3__x3t__ NormalFrame(V__x3t__ n)
    {
        var sg = n.Z >= 0 ? 1 : -1; // original uses copysign(1.0, n.Z) -> not the same as sign where 0 -> 0
        var a = -1 / (sg + n.Z);
        var b = n.X * n.Y * a;
        // column 0: [1 + sg * n.X * n.X * a, sg * b, -sg * n.X]
        // column 1: [b, sg + n.Y * n.Y * a, -n.Y]
        // column 2: n
        return new M3__x3t__(1 + sg * n.X * n.X * a, b, n.X,
                             sg * b, sg + n.Y * n.Y * a, n.Y,
                             -sg * n.X, -n.Y, n.Z);
    }        

    /// <summary>
    /// Computes from a <see cref="V__x3t__"/> normal the transformation matrix
    /// from the local coordinate system where the normal is the z-axis to
    /// the global coordinate system.
    /// </summary>
    /// <param name="normal">The normal vector of the new ground plane.</param>
    public static M3__x3t__ NormalFrameLocal2Global(V__x3t__ normal)
    {
        V__x3t__ min;
        double x = Fun.Abs(normal.X);
        double y = Fun.Abs(normal.Y);
        double z = Fun.Abs(normal.Z);

        if (x < y)
        {
            if (x < z) { min = V__x3t__.XAxis; } else { min = V__x3t__.ZAxis; }
        }
        else
        {
            if (y < z) { min = V__x3t__.YAxis; } else { min = V__x3t__.ZAxis; }
        }

        var xVec = Vec.Cross(normal, min);
        xVec.Normalize(); // this is now guaranteed to be normal to the input normal
        var yVec = Vec.Cross(normal, xVec);
        yVec.Normalize();
        var zVec = normal;
        zVec.Normalize();

        return new M3__x3t__(xVec.X, yVec.X, zVec.X,
                        xVec.Y, yVec.Y, zVec.Y,
                        xVec.Z, yVec.Z, zVec.Z);
    }

    #endregion
}

//# }
