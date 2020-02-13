using System;
using System.Text;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var x2t = isDouble ? "2d" : "2f";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    public partial struct M3__x3t__
    {
        #region Coordinate-System Transforms

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
}
