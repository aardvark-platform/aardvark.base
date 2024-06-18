using System;
using System.Text;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var x2t = isDouble ? "2d" : "2f";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    #region M4__x4t__

    public partial struct M4__x4t__
    {
        #region Coordinate-System Transforms

        /// <summary>
        /// Computes from a <see cref="V__x3t__"/> point (origin) and
        /// a <see cref="V__x3t__"/> normal the transformation matrix
        /// and its inverse.
        /// </summary>
        /// <param name="origin">The point which will become the new origin.</param>
        /// <param name="normal">The normal vector of the new ground plane.</param>
        /// <param name="local2global">A <see cref="M4__x4t__"/>The trafo from local to global system.</param>
        /// <param name="global2local">A <see cref="M4__x4t__"/>The trafofrom global to local system.</param>
        public static void NormalFrame(V__x3t__ origin, V__x3t__ normal,
            out M4__x4t__ local2global, out M4__x4t__ global2local
            )
        {
            V__x3t__ min;
            __ft__ x = Fun.Abs(normal.X);
            __ft__ y = Fun.Abs(normal.Y);
            __ft__ z = Fun.Abs(normal.Z);

            if (x < y)
            {
                if (x < z) { min = V__x3t__.XAxis; } else { min = V__x3t__.ZAxis; }
            }
            else
            {
                if (y < z) { min = V__x3t__.YAxis; } else { min = V__x3t__.ZAxis; }
            }

            V__x3t__ xVec = Vec.Cross(normal, min);
            xVec.Normalize(); // this is now guaranteed to be normal to the input normal
            V__x3t__ yVec = Vec.Cross(normal, xVec);
            yVec.Normalize();
            V__x3t__ zVec = normal;
            zVec.Normalize();

            local2global = new M4__x4t__(xVec.X, yVec.X, zVec.X, origin.X,
                                    xVec.Y, yVec.Y, zVec.Y, origin.Y,
                                    xVec.Z, yVec.Z, zVec.Z, origin.Z,
                                    0, 0, 0, 1);

            M4__x4t__ mat = new M4__x4t__(xVec.X, xVec.Y, xVec.Z, 0,
                                yVec.X, yVec.Y, yVec.Z, 0,
                                zVec.X, zVec.Y, zVec.Z, 0,
                                0, 0, 0, 1);

            var shift = M4__x4t__.Translation(-origin);
            global2local = mat * shift;
        }


        /// <summary>
        /// Computes a Coordiante Frame Transformation (Basis) from current CS into
        /// the (X, Y, Z)-System at a given Origin.
        /// Note: you can use it, to transform from RH to LH and vice-versa, all depending
        /// how you will specifie your new basis-vectors.
        /// </summary>
        /// <param name="xVec">New X Vector</param>
        /// <param name="yVec">New Y Vector</param>
        /// <param name="zVec">New Z vector</param>
        /// <param name="oVec">New Origin.</param>
        /// <param name="viewTrafo"></param>
        /// <param name="viewTrafoInverse"></param>
        public static void CoordinateFrameTransform(V__x3t__ xVec, V__x3t__ yVec, V__x3t__ zVec, V__x3t__ oVec,
            out M4__x4t__ viewTrafo, out M4__x4t__ viewTrafoInverse)
        {
            oVec = -oVec;
            viewTrafo = new M4__x4t__(
                xVec.X, xVec.Y, xVec.Z, xVec.X * oVec.X + xVec.Y * oVec.Y + xVec.Z * oVec.Z,
                yVec.X, yVec.Y, yVec.Z, yVec.X * oVec.X + yVec.Y * oVec.Y + yVec.Z * oVec.Z,
                zVec.X, zVec.Y, zVec.Z, zVec.X * oVec.X + zVec.Y * oVec.Y + zVec.Z * oVec.Z,
                0, 0, 0, 1
                );
            viewTrafoInverse = new M4__x4t__(
                xVec.X, yVec.X, zVec.X, -oVec.X,
                xVec.Y, yVec.Y, zVec.Y, -oVec.Y,
                xVec.Z, yVec.Z, zVec.Z, -oVec.Z,
                0, 0, 0, 1
                );
        }

        /// <summary>
        ///  Provides perspective projection matrix in terms of the vertical field of view angle a and the aspect ratio r.
        /// </summary>
        public static M4__x4t__ PerspectiveProjectionTransformRH(__ft__ a, __ft__ r, __ft__ n, __ft__ f)
        {
            //F / r     0      0      0
            //  0       F      0      0
            //  0       0      A      B
            //  0       0      -1     0
            __ft__ F = 1 / Fun.Tan(a / 2);
            __ft__ A = f / (n - f);
            __ft__ B = f * n / (n - f);

            M4__x4t__ P = new M4__x4t__(
                F / r, 0, 0, 0,
                0, F, 0, 0,
                0, 0, A, B,
                0, 0, -1, 0);
            return P;
        }

        /// <summary>
        ///  Provides perspective projection matrix. 
        ///  The parameters describe the dimensions of the view volume.
        /// </summary>
        public static M4__x4t__ PerspectiveProjectionTransformRH(V__x2t__ size, __ft__ n, __ft__ f)
        {
            __ft__ w = size.X;
            __ft__ h = size.Y;
            // Fx      0      0      0
            //  0      Fy     0      0
            //  0      0      A      B
            //  0      0      -1     0
            __ft__ Fx = 2 * n / w;
            __ft__ Fy = 2 * n / h;
            __ft__ A = f / (n - f);
            __ft__ B = n * f / (n - f);
            M4__x4t__ P = new M4__x4t__(
                Fx, 0, 0, 0,
                0, Fy, 0, 0,
                0, 0, A, B,
                0, 0, -1, 0);
            return P;
        }

        /// <summary>
        /// Builds a customized, right-handed perspective Off-Center projection matrix.
        /// </summary>
        public static M4__x4t__ PerspectiveProjectionTransformRH(__ft__ l, __ft__ r, __ft__ t, __ft__ b, __ft__ n, __ft__ f)
        {
            // Fx      0      Sx     0
            //  0      Fy     Sy     0
            //  0      0      A      B
            //  0      0      -1     0
            __ft__ Fx = 2 * n / (r - l);
            __ft__ Fy = 2 * n / (t - b);
            __ft__ Sx = (l + r) / (r - l);
            __ft__ Sy = (t + b) / (t - b);
            __ft__ A = f / (n - f);
            __ft__ B = n * f / (n - f);

            M4__x4t__ P = new M4__x4t__(
                Fx, 0, Sx, 0,
                0, Fy, Sy, 0,
                0, 0, A, B,
                0, 0, -1, 0);
            return P;
        }

        #endregion

        #region Static creators

        /// <summary>
        /// Returns the matrix that transforms from the coordinate system
        /// specified by the basis into the world cordinate system.
        /// </summary>
        public static M4__x4t__ FromBasis(V__x3t__ xAxis, V__x3t__ yAxis, V__x3t__ zAxis, V__x3t__ origin)
        {
            return new M4__x4t__(
                xAxis.X, yAxis.X, zAxis.X, origin.X,
                xAxis.Y, yAxis.Y, zAxis.Y, origin.Y,
                xAxis.Z, yAxis.Z, zAxis.Z, origin.Z,
                0, 0, 0, 1);
        }

        /// <summary>
        /// Creates a view tranformation from the given vectors.
        /// Transformation from world- into view-space.
        /// </summary>
        /// <param name="location">Origin of the view</param>
        /// <param name="right">Right vector of the view-plane</param>
        /// <param name="up">Up vector of the view-plane</param>
        /// <param name="normal">Normal vector of the view-plane. This vector is suppsoed to point in view-direction for a left-handed view transformation and in opposit direction in the right-handed case.</param>
        /// <returns>The view transformation</returns>
        public static M4__x4t__ ViewTrafo(V__x3t__ location, V__x3t__ right, V__x3t__ up, V__x3t__ normal)
        {
            return new M4__x4t__(
                    right.X,  right.Y,  right.Z,  -location.Dot(right),
                    up.X,     up.Y,     up.Z,     -location.Dot(up),
                    normal.X, normal.Y, normal.Z, -location.Dot(normal),
                    0,        0,        0,         1
                );
        }

        /// <summary>
        /// Creates a inverse view tranformation from the given vectors.
        /// Transformation from view- into world-space.
        /// The implementation is the same as FromBasis(right, up, normal, location)
        /// </summary>
        /// <param name="location">Origin of the view</param>
        /// <param name="right">Right vector of the view-plane</param>
        /// <param name="up">Up vector of the view-plane</param>
        /// <param name="normal">Normal vector of the view-plane. This vector is suppsoed to point in view-direction for a left-handed view transformation and in opposit direction in the right-handed case.</param>
        /// <returns>The inverse view transformation</returns>
        public static M4__x4t__ InvViewTrafo(V__x3t__ location, V__x3t__ right, V__x3t__ up, V__x3t__ normal)
        {
            return new M4__x4t__(
                    right.X, up.X, normal.X, location.X,
                    right.Y, up.Y, normal.Y, location.Y,
                    right.Z, up.Z, normal.Z, location.Z,
                    0, 0, 0, 1
                );
        }

        #endregion
    }

    #endregion

    //# } // isDouble
}
