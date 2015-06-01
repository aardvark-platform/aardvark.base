using System;
using System.Text;

namespace Aardvark.Base
{

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var x2t = isDouble ? "2d" : "2f";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    public partial struct M4__x4t__ : ISquareMatrix<M4__x4t__, V__x4t__, V__x3t__, __ft__>
    {
        #region Matrix Arithmetics

        /// <summary>
        /// Multiplacation of a <see cref="M4__x4t__"/> with a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static M4__x4t__ Multiply(M4__x4t__ matrix, Scale__x3t__ scale)
        {
            return new M4__x4t__(
                    matrix.M00 * scale.X,
                    matrix.M01 * scale.Y,
                    matrix.M02 * scale.Z,
                    matrix.M03,

                    matrix.M10 * scale.X,
                    matrix.M11 * scale.Y,
                    matrix.M12 * scale.Z,
                    matrix.M13,

                    matrix.M20 * scale.X,
                    matrix.M21 * scale.Y,
                    matrix.M22 * scale.Z,
                    matrix.M23,

                    matrix.M30 * scale.X,
                    matrix.M31 * scale.Y,
                    matrix.M32 * scale.Z,
                    matrix.M33
                    );
        }

        /// <summary>
        /// Multiplacation of a <see cref="M4__x4t__"/> with a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static M4__x4t__ Multiply(M4__x4t__ matrix, Shift__x3t__ shift)
        {

            return new M4__x4t__(
                    matrix.M00,
                    matrix.M01,
                    matrix.M02,
                    matrix.M00 * shift.X +
                    matrix.M01 * shift.Y +
                    matrix.M02 * shift.Z +
                    matrix.M03,

                    matrix.M10,
                    matrix.M11,
                    matrix.M12,
                    matrix.M10 * shift.X +
                    matrix.M11 * shift.Y +
                    matrix.M12 * shift.Z +
                    matrix.M13,

                    matrix.M20,
                    matrix.M21,
                    matrix.M22,
                    matrix.M20 * shift.X +
                    matrix.M21 * shift.Y +
                    matrix.M22 * shift.Z +
                    matrix.M23,

                    matrix.M30,
                    matrix.M31,
                    matrix.M32,
                    matrix.M33
                    );
        }

        /// <summary>
        /// A <see cref="M4__x4t__"/> is transformed to <see cref="M3__x3t__"/> by deleting specified row and column
        /// </summary>

        public M3__x3t__ Minor(int rowToDelete, int columnToDelete)
        {
            M3__x3t__ result = new M3__x3t__();
            int checked_row = 0;
            for (int actual_row = 0; actual_row < 4; actual_row++)
            {
                int checked_column = 0;

                if (actual_row != rowToDelete)
                {
                    for (int actual_column = 0; actual_column < 4; actual_column++)
                    {
                        if (actual_column != columnToDelete)
                        {
                            result[checked_row, checked_column] = this[actual_row, actual_column];
                            checked_column++;
                        }
                    }
                    checked_row++;
                }
            }
            return result;
        }

        /// <summary>
        /// Returns transpose of a <see cref="M4__x4t__"/>.
        /// </summary>
        public static M4__x4t__ Transpose(M4__x4t__ m)
        {
            return new M4__x4t__
            {
                M00 = m.M00,
                M01 = m.M10,
                M02 = m.M20,
                M03 = m.M30,
                M10 = m.M01,
                M11 = m.M11,
                M12 = m.M21,
                M13 = m.M31,
                M20 = m.M02,
                M21 = m.M12,
                M22 = m.M22,
                M23 = m.M32,
                M30 = m.M03,
                M31 = m.M13,
                M32 = m.M23,
                M33 = m.M33
            };
        }

        /// <summary>
        /// Transforms a direction <see cref="V__x4t__"/> by a <see cref="M4__x4t__"/>.
        /// </summary>
        public static V__x4t__ Transform(M4__x4t__ m, V__x4t__ v)
        {
            return new V__x4t__(
                m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z + m.M03 * v.W,
                m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z + m.M13 * v.W,
                m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z + m.M23 * v.W,
                m.M30 * v.X + m.M31 * v.Y + m.M32 * v.Z + m.M33 * v.W
                );
        }

        /// <summary>
        /// Transforms a <see cref="V__x3t__"/> position by the transpose of a <see cref="M4__x4t__"/>.
        /// Projective transform is performed.
        /// </summary>
        public static V__x3t__ TransposedTransformPosProj(M4__x4t__ matrix, V__x3t__ position)
        {
            __ft__ s = 1 / (matrix.M03 * position.X
                           + matrix.M13 * position.Y
                           + matrix.M23 * position.Z + matrix.M33);
            return (TransposedTransformDir(matrix, position)) * s;
        }

        #endregion

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

            V__x3t__ xVec = V__x3t__.Cross(normal, min);
            xVec.Normalize(); // this is now guaranteed to be normal to the input normal
            V__x3t__ yVec = V__x3t__.Cross(normal, xVec);
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



        /// <summary>
        /// Builds a customized, left-handed perspective Off-Center projection matrix.
        /// </summary>
        public static M4__x4t__ PerspectiveProjectionTransformLH(__ft__ l, __ft__ r, __ft__ t, __ft__ b, __ft__ n, __ft__ f)
        {
            //  Fx     0      0     0
            //  0      Fy     0     0
            //  Sx     Sy     A     1
            //  0      0      B     0
            __ft__ Fx = 2 * n / (r - l);
            __ft__ Fy = 2 * n / (t - b);
            __ft__ Sx = (l + r) / (l - r);
            __ft__ Sy = (t + b) / (b - t);
            __ft__ A = f / (f - n);
            __ft__ B = n * f / (n - f);

            M4__x4t__ P = new M4__x4t__(
                Fx, 0, 0, 0,
                0, Fy, 0, 0,
                Sx, Sy, A, 1,
                0, 0, B, 0);
            return P;
        }
        #endregion

        #region Operators

        /// <summary>
        /// Calculates the product of a <see cref="M4__x4t__"/> with a <see cref="Shift__x3t__"/>.
        /// </summary>
        public static M4__x4t__ operator *(M4__x4t__ matrix, Shift__x3t__ shift)
        {
            return M4__x4t__.Multiply(matrix, shift);
        }

        /// <summary>
        /// Calculates the product of a <see cref="M4__x4t__"/> with a <see cref="Scale__x3t__"/>.
        /// </summary>
        public static M4__x4t__ operator *(M4__x4t__ matrix, Scale__x3t__ scale)
        {
            return M4__x4t__.Multiply(matrix, scale);
        }

        #endregion

        #region Static creators

        /// <summary>
        /// Creates new Identity with 3 __ft__ values for translation.
        /// </summary>
        /// <returns>Translation matrix.</returns>
        public static M4__x4t__ Translation(__ft__ dx, __ft__ dy, __ft__ dz)
        {
            return new M4__x4t__(1, 0, 0, dx,
                            0, 1, 0, dy,
                            0, 0, 1, dz,
                            0, 0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with V__x3t__ vector for translation.
        /// </summary>
        /// <returns>Translation matrix.</returns>
        public static M4__x4t__ Translation(V__x3t__ v)
        {
            return new M4__x4t__(1, 0, 0, v.X,
                            0, 1, 0, v.Y,
                            0, 0, 1, v.Z,
                            0, 0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity <see cref="M4__x4t__"/> with a <see cref="Shift__x3t__"/> for translation.
        /// </summary>
        /// <returns>Translation matrix.</returns>
        public static M4__x4t__ Translation(Shift__x3t__ s)
        {
            return new M4__x4t__(1, 0, 0, s.X,
                            0, 1, 0, s.Y,
                            0, 0, 1, s.Z,
                            0, 0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with scalar value for uniform-scaling.
        /// </summary>
        /// <returns>Uniform-scaling matrix.</returns>
        public static M4__x4t__ Scale(__ft__ s)
        {
            return new M4__x4t__(s, 0, 0, 0,
                            0, s, 0, 0,
                            0, 0, s, 0,
                            0, 0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with 3 scalar values for scaling.
        /// </summary>
        /// <returns>Scaling matrix.</returns>
        public static M4__x4t__ Scale(__ft__ sx, __ft__ sy, __ft__ sz)
        {
            return new M4__x4t__(sx, 0, 0, 0,
                            0, sy, 0, 0,
                            0, 0, sz, 0,
                            0, 0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with V__x3t__ for scaling.
        /// </summary>
        /// <returns>Scaling matrix.</returns>
        public static M4__x4t__ Scale(V__x3t__ v)
        {
            return new M4__x4t__(v.X, 0, 0, 0,
                            0, v.Y, 0, 0,
                            0, 0, v.Z, 0,
                            0, 0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity <see cref="M4__x4t__"/> with <see cref="Scale__x3t__"/> for scaling.
        /// </summary>
        /// <param name="scale"></param>
        /// <returns>A <see cref="M4__x4t__"/> scaling matrix.</returns>
        public static M4__x4t__ Scale(Scale__x3t__ scale)
        {
            return new M4__x4t__(scale.X, 0, 0, 0,
                            0, scale.Y, 0, 0,
                            0, 0, scale.Z, 0,
                            0, 0, 0, 1);
        }

        /// <summary>
        /// Creates rotation matrix from axis and angle.
        /// </summary>
        public static M4__x4t__ Rotation(V__x3t__ axis, __ft__ angleInRadians)
        {
            return (M4__x4t__)(new Rot__x3t__(axis, angleInRadians));
        }

        /// <summary>
        /// Creates rotation matrix from yaw, pitch, and roll. 
        /// </summary>
        public static M4__x4t__ Rotation(
            __ft__ yawInRadians, __ft__ pitchInRadians, __ft__ rollInRadians
            )
        {
            return (M4__x4t__)(new Rot__x3t__(
                yawInRadians, pitchInRadians, rollInRadians
                ));
        }

        /// <summary>
        /// Creates rotation matrix from yaw, pitch, and roll Vector.
        /// </summary>
        public static M4__x4t__ Rotation(V__x3t__ yaw_pitch_roll_inRadians)
        {
            return (M4__x4t__)(new Rot__x3t__(
                yaw_pitch_roll_inRadians.X,
                yaw_pitch_roll_inRadians.Y,
                yaw_pitch_roll_inRadians.Z));
        }

        /// <summary>
        /// Creates rotation matrix which rotates one vector into another.
        /// </summary>
        public static M4__x4t__ Rotation(V__x3t__ from, V__x3t__ into)
        {
            return (M4__x4t__)(new Rot__x3t__(from, into));
        }

        /// <summary>
        /// Creates rotational matrix from quaternion.
        /// </summary>
        /// <returns>Rotational matrix.</returns>
        public static M4__x4t__ Rotation(Rot__x3t__ r)
        {
            return (M4__x4t__)r;
        }

        /// <summary>
        /// Creates new rotational matrix for "__ft__ value"-radians around X-Axis.
        /// </summary>
        /// <returns>Rotational matrix.</returns>
        public static M4__x4t__ RotationX(__ft__ angleRadians)
        {
            __ft__ cos = Fun.Cos(angleRadians);
            __ft__ sin = Fun.Sin(angleRadians);
            return new M4__x4t__(1, 0, 0, 0,
                            0, cos, -sin, 0,
                            0, sin, cos, 0,
                            0, 0, 0, 1);
        }

        /// <summary>
        /// Creates new rotational matrix for "__ft__ value"-radians around Y-Axis.
        /// </summary>
        /// <returns>Rotational matrix.</returns>
        public static M4__x4t__ RotationY(__ft__ angleRadians)
        {
            __ft__ cos = Fun.Cos(angleRadians);
            __ft__ sin = Fun.Sin(angleRadians);
            return new M4__x4t__(cos, 0, -sin, 0,
                            0, 1, 0, 0,
                            sin, 0, cos, 0,
                            0, 0, 0, 1);
        }

        /// <summary>
        /// Creates new rotational matrix for "__ft__ value"-radians around Z-Axis.
        /// </summary>
        /// <returns>Rotational matrix.</returns>
        public static M4__x4t__ RotationZ(__ft__ angleRadians)
        {
            __ft__ cos = Fun.Cos(angleRadians);
            __ft__ sin = Fun.Sin(angleRadians);
            return new M4__x4t__(cos, -sin, 0, 0,
                            sin, cos, 0, 0,
                            0, 0, 1, 0,
                            0, 0, 0, 1);
        }

        public static M4__x4t__ ShearXY(__ft__ factorX, __ft__ factorY)
        {
            return new M4__x4t__(1, 0, factorX, 0,
                            0, 1, factorY, 0,
                            0, 0, 1, 0,
                            0, 0, 0, 1);
        }

        public static M4__x4t__ ShearXZ(__ft__ factorX, __ft__ factorZ)
        {
            return new M4__x4t__(1, factorX, 0, 0,
                            0, 1, 0, 0,
                            0, factorZ, 1, 0,
                            0, 0, 0, 1);
        }

        public static M4__x4t__ ShearYZ(__ft__ factorY, __ft__ factorZ)
        {
            return new M4__x4t__(1, 0, 0, 0,
                            factorY, 1, 0, 0,
                            factorZ, 0, 1, 0,
                            0, 0, 0, 1);
        }


        /// <summary>
        /// Returns the matrix that transforms from the coordinate system
        /// specified by the basis into the world cordinate system.
        /// </summary>
        public static M4__x4t__ FromBasis(V__x3t__ xAxis, V__x3t__ yAxis, V__x3t__ zAxis, V__x3t__ orign)
        {
            return new M4__x4t__(
                xAxis.X, yAxis.X, zAxis.X, orign.X,
                xAxis.Y, yAxis.Y, zAxis.Y, orign.Y,
                xAxis.Z, yAxis.Z, zAxis.Z, orign.Z,
                0, 0, 0, 1);
        }

        #endregion

        #region ITransform<V__x3t__> implementation.

        /// <summary>
        /// Converts this matrix to its adjoint.
        /// </summary>
        /// <returns>This.</returns>
        public M4__x4t__ Adjoin()
        {
            return this = Adjoint;
        }

        /// <summary>
        /// Returns adjoint of this matrix.
        /// </summary>
        public M4__x4t__ Adjoint
        {
            get
            {
                M4__x4t__ result = new M4__x4t__();
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        if (((col + row) % 2) == 0)
                            result[col, row] = Minor(row, col).Det;
                        else
                            result[col, row] = -Minor(row, col).Det;
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public V__x4t__ Transform(V__x4t__ v)
        {
            return this * v;
        }

        /// <summary>
        /// Transforms direction vector v (p.w is presumed 0)
        /// with the inverse of this transform.
        /// </summary>
        public V__x3t__ InvTransformDir(V__x3t__ v)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) with the inverse of this transform.
        /// No projective transform is performed.
        /// </summary>
        public V__x3t__ InvTransformPos(V__x3t__ p)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Transforms point p (p.w is presumed 1.0) with the inverse of this transform.
        /// Projective transform is performed.
        /// </summary>
        public V__x3t__ InvTransformPosProj(V__x3t__ p)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
    //# } // isDouble
}
