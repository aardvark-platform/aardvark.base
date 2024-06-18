using System;
using System.Text;

namespace Aardvark.Base
{
    #region M44f

    public partial struct M44f
    {
        #region Coordinate-System Transforms

        /// <summary>
        /// Computes from a <see cref="V3f"/> point (origin) and
        /// a <see cref="V3f"/> normal the transformation matrix
        /// and its inverse.
        /// </summary>
        /// <param name="origin">The point which will become the new origin.</param>
        /// <param name="normal">The normal vector of the new ground plane.</param>
        /// <param name="local2global">A <see cref="M44f"/>The trafo from local to global system.</param>
        /// <param name="global2local">A <see cref="M44f"/>The trafofrom global to local system.</param>
        public static void NormalFrame(V3f origin, V3f normal,
            out M44f local2global, out M44f global2local
            )
        {
            V3f min;
            float x = Fun.Abs(normal.X);
            float y = Fun.Abs(normal.Y);
            float z = Fun.Abs(normal.Z);

            if (x < y)
            {
                if (x < z) { min = V3f.XAxis; } else { min = V3f.ZAxis; }
            }
            else
            {
                if (y < z) { min = V3f.YAxis; } else { min = V3f.ZAxis; }
            }

            V3f xVec = Vec.Cross(normal, min);
            xVec.Normalize(); // this is now guaranteed to be normal to the input normal
            V3f yVec = Vec.Cross(normal, xVec);
            yVec.Normalize();
            V3f zVec = normal;
            zVec.Normalize();

            local2global = new M44f(xVec.X, yVec.X, zVec.X, origin.X,
                                    xVec.Y, yVec.Y, zVec.Y, origin.Y,
                                    xVec.Z, yVec.Z, zVec.Z, origin.Z,
                                    0, 0, 0, 1);

            M44f mat = new M44f(xVec.X, xVec.Y, xVec.Z, 0,
                                yVec.X, yVec.Y, yVec.Z, 0,
                                zVec.X, zVec.Y, zVec.Z, 0,
                                0, 0, 0, 1);

            var shift = M44f.Translation(-origin);
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
        public static void CoordinateFrameTransform(V3f xVec, V3f yVec, V3f zVec, V3f oVec,
            out M44f viewTrafo, out M44f viewTrafoInverse)
        {
            oVec = -oVec;
            viewTrafo = new M44f(
                xVec.X, xVec.Y, xVec.Z, xVec.X * oVec.X + xVec.Y * oVec.Y + xVec.Z * oVec.Z,
                yVec.X, yVec.Y, yVec.Z, yVec.X * oVec.X + yVec.Y * oVec.Y + yVec.Z * oVec.Z,
                zVec.X, zVec.Y, zVec.Z, zVec.X * oVec.X + zVec.Y * oVec.Y + zVec.Z * oVec.Z,
                0, 0, 0, 1
                );
            viewTrafoInverse = new M44f(
                xVec.X, yVec.X, zVec.X, -oVec.X,
                xVec.Y, yVec.Y, zVec.Y, -oVec.Y,
                xVec.Z, yVec.Z, zVec.Z, -oVec.Z,
                0, 0, 0, 1
                );
        }

        /// <summary>
        ///  Provides perspective projection matrix in terms of the vertical field of view angle a and the aspect ratio r.
        /// </summary>
        public static M44f PerspectiveProjectionTransformRH(float a, float r, float n, float f)
        {
            //F / r     0      0      0
            //  0       F      0      0
            //  0       0      A      B
            //  0       0      -1     0
            float F = 1 / Fun.Tan(a / 2);
            float A = f / (n - f);
            float B = f * n / (n - f);

            M44f P = new M44f(
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
        public static M44f PerspectiveProjectionTransformRH(V2f size, float n, float f)
        {
            float w = size.X;
            float h = size.Y;
            // Fx      0      0      0
            //  0      Fy     0      0
            //  0      0      A      B
            //  0      0      -1     0
            float Fx = 2 * n / w;
            float Fy = 2 * n / h;
            float A = f / (n - f);
            float B = n * f / (n - f);
            M44f P = new M44f(
                Fx, 0, 0, 0,
                0, Fy, 0, 0,
                0, 0, A, B,
                0, 0, -1, 0);
            return P;
        }

        /// <summary>
        /// Builds a customized, right-handed perspective Off-Center projection matrix.
        /// </summary>
        public static M44f PerspectiveProjectionTransformRH(float l, float r, float t, float b, float n, float f)
        {
            // Fx      0      Sx     0
            //  0      Fy     Sy     0
            //  0      0      A      B
            //  0      0      -1     0
            float Fx = 2 * n / (r - l);
            float Fy = 2 * n / (t - b);
            float Sx = (l + r) / (r - l);
            float Sy = (t + b) / (t - b);
            float A = f / (n - f);
            float B = n * f / (n - f);

            M44f P = new M44f(
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
        public static M44f FromBasis(V3f xAxis, V3f yAxis, V3f zAxis, V3f origin)
        {
            return new M44f(
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
        public static M44f ViewTrafo(V3f location, V3f right, V3f up, V3f normal)
        {
            return new M44f(
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
        public static M44f InvViewTrafo(V3f location, V3f right, V3f up, V3f normal)
        {
            return new M44f(
                    right.X, up.X, normal.X, location.X,
                    right.Y, up.Y, normal.Y, location.Y,
                    right.Z, up.Z, normal.Z, location.Z,
                    0, 0, 0, 1
                );
        }

        #endregion
    }

    #endregion

    #region M44d

    public partial struct M44d
    {
        #region Coordinate-System Transforms

        /// <summary>
        /// Computes from a <see cref="V3d"/> point (origin) and
        /// a <see cref="V3d"/> normal the transformation matrix
        /// and its inverse.
        /// </summary>
        /// <param name="origin">The point which will become the new origin.</param>
        /// <param name="normal">The normal vector of the new ground plane.</param>
        /// <param name="local2global">A <see cref="M44d"/>The trafo from local to global system.</param>
        /// <param name="global2local">A <see cref="M44d"/>The trafofrom global to local system.</param>
        public static void NormalFrame(V3d origin, V3d normal,
            out M44d local2global, out M44d global2local
            )
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

            V3d xVec = Vec.Cross(normal, min);
            xVec.Normalize(); // this is now guaranteed to be normal to the input normal
            V3d yVec = Vec.Cross(normal, xVec);
            yVec.Normalize();
            V3d zVec = normal;
            zVec.Normalize();

            local2global = new M44d(xVec.X, yVec.X, zVec.X, origin.X,
                                    xVec.Y, yVec.Y, zVec.Y, origin.Y,
                                    xVec.Z, yVec.Z, zVec.Z, origin.Z,
                                    0, 0, 0, 1);

            M44d mat = new M44d(xVec.X, xVec.Y, xVec.Z, 0,
                                yVec.X, yVec.Y, yVec.Z, 0,
                                zVec.X, zVec.Y, zVec.Z, 0,
                                0, 0, 0, 1);

            var shift = M44d.Translation(-origin);
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
        public static void CoordinateFrameTransform(V3d xVec, V3d yVec, V3d zVec, V3d oVec,
            out M44d viewTrafo, out M44d viewTrafoInverse)
        {
            oVec = -oVec;
            viewTrafo = new M44d(
                xVec.X, xVec.Y, xVec.Z, xVec.X * oVec.X + xVec.Y * oVec.Y + xVec.Z * oVec.Z,
                yVec.X, yVec.Y, yVec.Z, yVec.X * oVec.X + yVec.Y * oVec.Y + yVec.Z * oVec.Z,
                zVec.X, zVec.Y, zVec.Z, zVec.X * oVec.X + zVec.Y * oVec.Y + zVec.Z * oVec.Z,
                0, 0, 0, 1
                );
            viewTrafoInverse = new M44d(
                xVec.X, yVec.X, zVec.X, -oVec.X,
                xVec.Y, yVec.Y, zVec.Y, -oVec.Y,
                xVec.Z, yVec.Z, zVec.Z, -oVec.Z,
                0, 0, 0, 1
                );
        }

        /// <summary>
        ///  Provides perspective projection matrix in terms of the vertical field of view angle a and the aspect ratio r.
        /// </summary>
        public static M44d PerspectiveProjectionTransformRH(double a, double r, double n, double f)
        {
            //F / r     0      0      0
            //  0       F      0      0
            //  0       0      A      B
            //  0       0      -1     0
            double F = 1 / Fun.Tan(a / 2);
            double A = f / (n - f);
            double B = f * n / (n - f);

            M44d P = new M44d(
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
        public static M44d PerspectiveProjectionTransformRH(V2d size, double n, double f)
        {
            double w = size.X;
            double h = size.Y;
            // Fx      0      0      0
            //  0      Fy     0      0
            //  0      0      A      B
            //  0      0      -1     0
            double Fx = 2 * n / w;
            double Fy = 2 * n / h;
            double A = f / (n - f);
            double B = n * f / (n - f);
            M44d P = new M44d(
                Fx, 0, 0, 0,
                0, Fy, 0, 0,
                0, 0, A, B,
                0, 0, -1, 0);
            return P;
        }

        /// <summary>
        /// Builds a customized, right-handed perspective Off-Center projection matrix.
        /// </summary>
        public static M44d PerspectiveProjectionTransformRH(double l, double r, double t, double b, double n, double f)
        {
            // Fx      0      Sx     0
            //  0      Fy     Sy     0
            //  0      0      A      B
            //  0      0      -1     0
            double Fx = 2 * n / (r - l);
            double Fy = 2 * n / (t - b);
            double Sx = (l + r) / (r - l);
            double Sy = (t + b) / (t - b);
            double A = f / (n - f);
            double B = n * f / (n - f);

            M44d P = new M44d(
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
        public static M44d FromBasis(V3d xAxis, V3d yAxis, V3d zAxis, V3d origin)
        {
            return new M44d(
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
        public static M44d ViewTrafo(V3d location, V3d right, V3d up, V3d normal)
        {
            return new M44d(
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
        public static M44d InvViewTrafo(V3d location, V3d right, V3d up, V3d normal)
        {
            return new M44d(
                    right.X, up.X, normal.X, location.X,
                    right.Y, up.Y, normal.Y, location.Y,
                    right.Z, up.Z, normal.Z, location.Z,
                    0, 0, 0, 1
                );
        }

        #endregion
    }

    #endregion

}
