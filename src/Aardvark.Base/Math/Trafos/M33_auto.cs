using System;
using System.Text;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    public partial struct M33f
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
        public static M33f NormalFrame(V3f n)
        {
            var sg = n.Z >= 0 ? 1 : -1; // original uses copysign(1.0, n.Z) -> not the same as sign where 0 -> 0
            var a = -1 / (sg + n.Z);
            var b = n.X * n.Y * a;
            // column 0: [1 + sg * n.X * n.X * a, sg * b, -sg * n.X]
            // column 1: [b, sg + n.Y * n.Y * a, -n.Y]
            // column 2: n
            return new M33f(1 + sg * n.X * n.X * a, b, n.X,
                                 sg * b, sg + n.Y * n.Y * a, n.Y,
                                 -sg * n.X, -n.Y, n.Z);
        }        

        /// <summary>
        /// Computes from a <see cref="V3f"/> normal the transformation matrix
        /// from the local coordinate system where the normal is the z-axis to
        /// the global coordinate system.
        /// </summary>
        /// <param name="normal">The normal vector of the new ground plane.</param>
        public static M33f NormalFrameLocal2Global(V3f normal)
        {
            V3f min;
            double x = Fun.Abs(normal.X);
            double y = Fun.Abs(normal.Y);
            double z = Fun.Abs(normal.Z);

            if (x < y)
            {
                if (x < z) { min = V3f.XAxis; } else { min = V3f.ZAxis; }
            }
            else
            {
                if (y < z) { min = V3f.YAxis; } else { min = V3f.ZAxis; }
            }

            var xVec = Vec.Cross(normal, min);
            xVec.Normalize(); // this is now guaranteed to be normal to the input normal
            var yVec = Vec.Cross(normal, xVec);
            yVec.Normalize();
            var zVec = normal;
            zVec.Normalize();

            return new M33f(xVec.X, yVec.X, zVec.X,
                            xVec.Y, yVec.Y, zVec.Y,
                            xVec.Z, yVec.Z, zVec.Z);
        }

        #endregion
    }

    public partial struct M33d
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
        public static M33d NormalFrame(V3d n)
        {
            var sg = n.Z >= 0 ? 1 : -1; // original uses copysign(1.0, n.Z) -> not the same as sign where 0 -> 0
            var a = -1 / (sg + n.Z);
            var b = n.X * n.Y * a;
            // column 0: [1 + sg * n.X * n.X * a, sg * b, -sg * n.X]
            // column 1: [b, sg + n.Y * n.Y * a, -n.Y]
            // column 2: n
            return new M33d(1 + sg * n.X * n.X * a, b, n.X,
                                 sg * b, sg + n.Y * n.Y * a, n.Y,
                                 -sg * n.X, -n.Y, n.Z);
        }        

        /// <summary>
        /// Computes from a <see cref="V3d"/> normal the transformation matrix
        /// from the local coordinate system where the normal is the z-axis to
        /// the global coordinate system.
        /// </summary>
        /// <param name="normal">The normal vector of the new ground plane.</param>
        public static M33d NormalFrameLocal2Global(V3d normal)
        {
            V3d min;
            double x = Fun.Abs(normal.X);
            double y = Fun.Abs(normal.Y);
            double z = Fun.Abs(normal.Z);

            if (x < y)
            {
                if (x < z) { min = V3d.XAxis; } else { min = V3d.ZAxis; }
            }
            else
            {
                if (y < z) { min = V3d.YAxis; } else { min = V3d.ZAxis; }
            }

            var xVec = Vec.Cross(normal, min);
            xVec.Normalize(); // this is now guaranteed to be normal to the input normal
            var yVec = Vec.Cross(normal, xVec);
            yVec.Normalize();
            var zVec = normal;
            zVec.Normalize();

            return new M33d(xVec.X, yVec.X, zVec.X,
                            xVec.Y, yVec.Y, zVec.Y,
                            xVec.Z, yVec.Z, zVec.Z);
        }

        #endregion
    }

}
