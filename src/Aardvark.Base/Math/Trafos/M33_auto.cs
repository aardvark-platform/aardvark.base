using System;
using System.Text;

namespace Aardvark.Base
{

    public partial struct M33f : ISquareMatrix<M33f, V3f, V2f, float>
    {

        #region Matrix Arithmetics

        public static M33f Multiply(M33f m, Scale3f s)
        {
            return new M33f(
                m.M00 * s.X, m.M01 * s.Y, m.M02 * s.Z,
                m.M10 * s.X, m.M11 * s.Y, m.M12 * s.Z,
                m.M20 * s.X, m.M21 * s.Y, m.M22 * s.Z
                );
        }

        public static M34f Multiply(M33f m, Shift3f t)
        {
            return new M34f(
               m.M00, m.M01, m.M02, m.M00 * t.X + m.M01 * t.Y + m.M02 * t.Z,
               m.M10, m.M11, m.M12, m.M10 * t.X + m.M11 * t.Y + m.M12 * t.Z,
               m.M20, m.M21, m.M22, m.M20 * t.X + m.M21 * t.Y + m.M22 * t.Z
               );
        }

        public static M44f Enlarge(M33f m)
        {
            M44f enlarged = new M44f(
                    m.M00, m.M01, m.M02, 0,
                    m.M10, m.M11, m.M12, 0,
                    m.M20, m.M21, m.M22, 0,
                    0, 0, 0, 1.0f);
            return enlarged;
        }

        public static V3f Transform(M33f m, V3f v)
        {
            return new V3f(
                m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z,
                m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z,
                m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z
                );
        }

        public static V3f TransposedTransform(M33f m, V3f v)
        {
            return new V3f(
                m.M00 * v.X + m.M10 * v.Y + m.M20 * v.Z,
                m.M01 * v.X + m.M11 * v.Y + m.M21 * v.Z,
                m.M02 * v.X + m.M12 * v.Y + m.M22 * v.Z
                );
        }


        #endregion

        #region Basic Operators

        public static M33f operator *(M33f m, Scale3f s)
        {
            return M33f.Multiply(m, s);
        }

        public static M34f operator *(M33f m, Shift3f t)
        {
            return M33f.Multiply(m, t);
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates new Identity with 2 float values for translation.
        /// </summary>
        public static M33f Translation(float dx, float dy)
        {
            return new M33f(1, 0, dx,
                            0, 1, dy,
                            0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with V3f vector for translation.
        /// </summary>
        public static M33f Translation(V2f v)
        {
            return new M33f(1, 0, v.X,
                            0, 1, v.Y,
                            0, 0, 1);
        }

#if TODO

        /// <summary>
        /// Creates new Identity <see cref="M44f"/> with a <see cref="Shift3f"/> for translation.
        /// </summary>
        /// <param name="shift"></param>
        /// <returns>A <see cref="M44f"/> translation matrix.</returns>
        public static M44f Translation(Shift3f shift)
        {
            return new M44f(1, 0, 0, shift.X,
                            0, 1, 0, shift.Y,
                            0, 0, 1, shift.Z,
                            0, 0, 0, 1);
        }
#endif
        /// <summary>
        /// Creates new Identity with scalar value for uniform-scaling.
        /// </summary>
        /// <returns>Uniform-scaling matrix.</returns>
        public static M33f Scale(float s)
        {
            return new M33f(s, 0, 0,
                            0, s, 0,
                            0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with 3 scalar values for scaling.
        /// </summary>
        /// <returns>Scaling matrix.</returns>
        public static M33f Scale(float sx, float sy)
        {
            return new M33f(sx, 0, 0,
                            0, sy, 0,
                            0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with V3f for scaling.
        /// </summary>
        /// <returns>Scaling matrix.</returns>
        public static M33f Scale(V2f v)
        {
            return new M33f(v.X, 0, 0,
                            0, v.Y, 0,
                            0, 0, 1);
        }

#if TODO
        /// <summary>
        /// Creates new Identity <see cref="M44f"/> with <see cref="Scale3f"/> for scaling.
        /// </summary>
        /// <param name="scale"></param>
        /// <returns>A <see cref="M44f"/> scaling matrix.</returns>
        public static M44f Scale(Scale3f scale)
        {
            return new M44f(scale.X, 0, 0, 0,
                            0, scale.Y, 0, 0,
                            0, 0, scale.Z, 0,
                            0, 0, 0, 1);
        }
#endif
        /// <summary>
        /// Creates rotation matrix from axis and angle.
        /// </summary>
        public static M33f Rotation(float angleInRadians)
        {
            return Rotation(new Rot2f(angleInRadians));
        }


        /// <summary>
        /// Creates rotational matrix from quaternion.
        /// </summary>
        /// <returns>Rotational matrix.</returns>
        public static M33f Rotation(Rot2f r)
        {
            return (M33f)r;
        }

        #endregion

        #region Coordinate-System Transforms

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

        #region ITransform<V3f> implementation.

        /// <summary>
        /// Converts this matrix to its adjoint.
        /// </summary>
        /// <returns>This.</returns>
        public M33f Adjoin()
        {
            return this = Adjoint;
        }

        /// <summary>
        /// Returns adjoint of this matrix.
        /// </summary>
        public M33f Adjoint
        {
            get
            {
                M44f ad = (M44f)M33f.Enlarge(this);
                M44f result = new M44f();
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        if (((col + row) % 2) == 0)
                            result[col, row] = ad.Minor(row, col).Determinant();
                        else
                            result[col, row] = -ad.Minor(row, col).Determinant();
                    }
                }
                return result.Minor(3, 3);
            }
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public V3f Transform(V3f v)
        {
            return Transform(this, v);
        }

        /// <summary>
        /// Transforms vector v by the transposed version of this.
        /// </summary>
        public V3f TransposedTransform(V3f v)
        {
            return TransposedTransform(this, v);
        }

        #endregion

        #region V3f Extensions
        /// <summary>
        /// Returns the skew-symmetric "cross" matrix (A^T = -A) of the vector v.
        /// </summary>
        public static M33f CrossMatrix(V3f v)
        {
            return new M33f(
                0, -v.Z, +v.Y,
                +v.Z, 0, -v.X,
                -v.Y, +v.X, 0);
        }

        /// <summary>
        /// Returns the outer product (tensor-product) of v1 * v2^T as a 3x3 Matrix.
        /// </summary>
        public static M33f OuterProduct(V3f v1, V3f v2)
        {
            return new M33f(
                v2.X * v1.X, v2.Y * v1.X, v2.Z * v1.X,
                v2.X * v1.Y, v2.Y * v1.Y, v2.Z * v1.Y,
                v2.X * v1.Z, v2.Y * v1.Z, v2.Z * v1.Z);
        }
        #endregion

    }

    public static partial class M33Extensions
    {
        #region V3f Extensions
        /// <summary>
        /// Returns the skew-symmetric "cross" matrix (A^T = -A) of the vector v.
        /// </summary>
        public static M33f CrossMatrix(this V3f v)
        {
            return new M33f(
                0, -v.Z, +v.Y,
                +v.Z, 0, -v.X,
                -v.Y, +v.X, 0);
        }

        /// <summary>
        /// Returns the outer product (tensor-product) of v1 * v2^T as a 3x3 Matrix.
        /// </summary>
        public static M33f OuterProduct(this V3f v1, V3f v2)
        {
            return new M33f(
                v2.X * v1.X, v2.Y * v1.X, v2.Z * v1.X,
                v2.X * v1.Y, v2.Y * v1.Y, v2.Z * v1.Y,
                v2.X * v1.Z, v2.Y * v1.Z, v2.Z * v1.Z);
        }
        #endregion
    }
    public partial struct M33d : ISquareMatrix<M33d, V3d, V2d, double>
    {

        #region Matrix Arithmetics

        public static M33d Multiply(M33d m, Scale3d s)
        {
            return new M33d(
                m.M00 * s.X, m.M01 * s.Y, m.M02 * s.Z,
                m.M10 * s.X, m.M11 * s.Y, m.M12 * s.Z,
                m.M20 * s.X, m.M21 * s.Y, m.M22 * s.Z
                );
        }

        public static M34d Multiply(M33d m, Shift3d t)
        {
            return new M34d(
               m.M00, m.M01, m.M02, m.M00 * t.X + m.M01 * t.Y + m.M02 * t.Z,
               m.M10, m.M11, m.M12, m.M10 * t.X + m.M11 * t.Y + m.M12 * t.Z,
               m.M20, m.M21, m.M22, m.M20 * t.X + m.M21 * t.Y + m.M22 * t.Z
               );
        }

        public static M44d Enlarge(M33d m)
        {
            M44d enlarged = new M44d(
                    m.M00, m.M01, m.M02, 0,
                    m.M10, m.M11, m.M12, 0,
                    m.M20, m.M21, m.M22, 0,
                    0, 0, 0, 1.0f);
            return enlarged;
        }

        public static V3d Transform(M33d m, V3d v)
        {
            return new V3d(
                m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z,
                m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z,
                m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z
                );
        }

        public static V3d TransposedTransform(M33d m, V3d v)
        {
            return new V3d(
                m.M00 * v.X + m.M10 * v.Y + m.M20 * v.Z,
                m.M01 * v.X + m.M11 * v.Y + m.M21 * v.Z,
                m.M02 * v.X + m.M12 * v.Y + m.M22 * v.Z
                );
        }


        #endregion

        #region Basic Operators

        public static M33d operator *(M33d m, Scale3d s)
        {
            return M33d.Multiply(m, s);
        }

        public static M34d operator *(M33d m, Shift3d t)
        {
            return M33d.Multiply(m, t);
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates new Identity with 2 double values for translation.
        /// </summary>
        public static M33d Translation(double dx, double dy)
        {
            return new M33d(1, 0, dx,
                            0, 1, dy,
                            0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with V3d vector for translation.
        /// </summary>
        public static M33d Translation(V2d v)
        {
            return new M33d(1, 0, v.X,
                            0, 1, v.Y,
                            0, 0, 1);
        }

#if TODO

        /// <summary>
        /// Creates new Identity <see cref="M44d"/> with a <see cref="Shift3d"/> for translation.
        /// </summary>
        /// <param name="shift"></param>
        /// <returns>A <see cref="M44d"/> translation matrix.</returns>
        public static M44d Translation(Shift3d shift)
        {
            return new M44d(1, 0, 0, shift.X,
                            0, 1, 0, shift.Y,
                            0, 0, 1, shift.Z,
                            0, 0, 0, 1);
        }
#endif
        /// <summary>
        /// Creates new Identity with scalar value for uniform-scaling.
        /// </summary>
        /// <returns>Uniform-scaling matrix.</returns>
        public static M33d Scale(double s)
        {
            return new M33d(s, 0, 0,
                            0, s, 0,
                            0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with 3 scalar values for scaling.
        /// </summary>
        /// <returns>Scaling matrix.</returns>
        public static M33d Scale(double sx, double sy)
        {
            return new M33d(sx, 0, 0,
                            0, sy, 0,
                            0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with V3d for scaling.
        /// </summary>
        /// <returns>Scaling matrix.</returns>
        public static M33d Scale(V2d v)
        {
            return new M33d(v.X, 0, 0,
                            0, v.Y, 0,
                            0, 0, 1);
        }

#if TODO
        /// <summary>
        /// Creates new Identity <see cref="M44d"/> with <see cref="Scale3d"/> for scaling.
        /// </summary>
        /// <param name="scale"></param>
        /// <returns>A <see cref="M44d"/> scaling matrix.</returns>
        public static M44d Scale(Scale3d scale)
        {
            return new M44d(scale.X, 0, 0, 0,
                            0, scale.Y, 0, 0,
                            0, 0, scale.Z, 0,
                            0, 0, 0, 1);
        }
#endif
        /// <summary>
        /// Creates rotation matrix from axis and angle.
        /// </summary>
        public static M33d Rotation(double angleInRadians)
        {
            return Rotation(new Rot2d(angleInRadians));
        }


        /// <summary>
        /// Creates rotational matrix from quaternion.
        /// </summary>
        /// <returns>Rotational matrix.</returns>
        public static M33d Rotation(Rot2d r)
        {
            return (M33d)r;
        }

        #endregion

        #region Coordinate-System Transforms

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

        #region ITransform<V3d> implementation.

        /// <summary>
        /// Converts this matrix to its adjoint.
        /// </summary>
        /// <returns>This.</returns>
        public M33d Adjoin()
        {
            return this = Adjoint;
        }

        /// <summary>
        /// Returns adjoint of this matrix.
        /// </summary>
        public M33d Adjoint
        {
            get
            {
                M44d ad = (M44d)M33d.Enlarge(this);
                M44d result = new M44d();
                for (int row = 0; row < 4; row++)
                {
                    for (int col = 0; col < 4; col++)
                    {
                        if (((col + row) % 2) == 0)
                            result[col, row] = ad.Minor(row, col).Determinant();
                        else
                            result[col, row] = -ad.Minor(row, col).Determinant();
                    }
                }
                return result.Minor(3, 3);
            }
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public V3d Transform(V3d v)
        {
            return Transform(this, v);
        }

        /// <summary>
        /// Transforms vector v by the transposed version of this.
        /// </summary>
        public V3d TransposedTransform(V3d v)
        {
            return TransposedTransform(this, v);
        }

        #endregion

        #region V3d Extensions
        /// <summary>
        /// Returns the skew-symmetric "cross" matrix (A^T = -A) of the vector v.
        /// </summary>
        public static M33d CrossMatrix(V3d v)
        {
            return new M33d(
                0, -v.Z, +v.Y,
                +v.Z, 0, -v.X,
                -v.Y, +v.X, 0);
        }

        /// <summary>
        /// Returns the outer product (tensor-product) of v1 * v2^T as a 3x3 Matrix.
        /// </summary>
        public static M33d OuterProduct(V3d v1, V3d v2)
        {
            return new M33d(
                v2.X * v1.X, v2.Y * v1.X, v2.Z * v1.X,
                v2.X * v1.Y, v2.Y * v1.Y, v2.Z * v1.Y,
                v2.X * v1.Z, v2.Y * v1.Z, v2.Z * v1.Z);
        }
        #endregion

    }

    public static partial class M33Extensions
    {
        #region V3d Extensions
        /// <summary>
        /// Returns the skew-symmetric "cross" matrix (A^T = -A) of the vector v.
        /// </summary>
        public static M33d CrossMatrix(this V3d v)
        {
            return new M33d(
                0, -v.Z, +v.Y,
                +v.Z, 0, -v.X,
                -v.Y, +v.X, 0);
        }

        /// <summary>
        /// Returns the outer product (tensor-product) of v1 * v2^T as a 3x3 Matrix.
        /// </summary>
        public static M33d OuterProduct(this V3d v1, V3d v2)
        {
            return new M33d(
                v2.X * v1.X, v2.Y * v1.X, v2.Z * v1.X,
                v2.X * v1.Y, v2.Y * v1.Y, v2.Z * v1.Y,
                v2.X * v1.Z, v2.Y * v1.Z, v2.Z * v1.Z);
        }
        #endregion
    }
}
