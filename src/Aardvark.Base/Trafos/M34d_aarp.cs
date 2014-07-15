using System;
using System.Text;

namespace Aardvark.Base
{
    public partial struct M34d : IMatrix<M34d, V4d, V3d, double>
    {

        #region Matrix Arithmetics

        public static M44d Add(M34d a, M44d b)
        {
            return new M44d(
                a.M00 + b.M00, a.M01 + b.M01, a.M02 + b.M02, a.M03 + b.M03,
                a.M10 + b.M10, a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13,
                a.M20 + b.M20, a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23,
                b.M30, b.M31, b.M32, 1 + b.M33
                );
        }

        public static M44d Subtract(M34d a, M44d b)
        {
            return new M44d(
                a.M00 - b.M00, a.M01 - b.M01, a.M02 - b.M02, a.M03 - b.M03,
                a.M10 - b.M10, a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
                a.M20 - b.M20, a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23,
                -b.M30, -b.M31, -b.M32, 1 - b.M33
                );
        }

        public static M44d Subtract(M44d a, M34d b)
        {
            return new M44d(
                a.M00 - b.M00, a.M01 - b.M01, a.M02 - b.M02, a.M03 - b.M03,
                a.M10 - b.M10, a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
                a.M20 - b.M20, a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23,
                a.M30, a.M31, a.M32, a.M33 - 1
                );
        }

        public static M34d Multiply(M34d m, Scale3d s)
        {
            return new M34d(
                m.M00 * s.X, m.M01 * s.Y, m.M02 * s.Z, m.M03,
                m.M10 * s.X, m.M11 * s.Y, m.M12 * s.Z, m.M13,
                m.M20 * s.X, m.M21 * s.Y, m.M22 * s.Z, m.M23
                );
        }

        public static M34d Multiply(M34d m, Shift3d t)
        {
            return new M34d(
                m.M00, m.M01, m.M02, m.M00 * t.X + m.M01 * t.Y + m.M02 * t.Z + m.M03,
                m.M10, m.M11, m.M12, m.M10 * t.X + m.M11 * t.Y + m.M12 * t.Z + m.M13,
                m.M20, m.M21, m.M22, m.M20 * t.X + m.M21 * t.Y + m.M22 * t.Z + m.M23
                );
        }

        /// <summary>
        /// Transforms a <see cref="M34d"/> to a <see cref="M33d"/> by deleting the
        /// specified row and column. Internally the <see cref="M34d"/> is tarnsformed 
        /// to a <see cref="M44d"/> to delete the row and column.
        /// </summary>
        /// <param name="deleted_row">Row to delete.</param>
        /// <param name="deleted_column">Column to delete.</param>
        /// <returns>A <see cref="M33d"/>.</returns>
        public M33d Minor(int deleted_row, int deleted_column)
        {
            M44d temp = (M44d)this;

            M33d result = new M33d();
            int checked_row = 0;

            for (int actual_row = 0; actual_row < 4; actual_row++)
            {
                int checked_column = 0;

                if (actual_row != deleted_row)
                {
                    for (int actual_column = 0; actual_column < 4; actual_column++)
                    {
                        if (actual_column != deleted_column)
                        {
                            result[checked_row, checked_column] = temp[actual_row, actual_column];
                            checked_column++;
                        }
                    }
                    checked_row++;
                }
            }

            return result;
        }

        /// <summary>
        /// Calculates the determinant of a <see cref="M34d"/>.
        /// </summary>
        /// <returns>Determinant as a double.</returns>
        public double Determinant()
        {
            double determinant = 0.0f;

            for (int actual_column = 0; actual_column < 4; actual_column++)
            {
                if ((actual_column % 2) == 0)
                {
                    determinant += this[0, actual_column] * Minor(0, actual_column).Determinant();
                }

                else
                {
                    determinant -= this[0, actual_column] * Minor(0, actual_column).Determinant();
                }
            }

            return determinant;

        }

        /// <summary>
        /// Transforms a <see cref="V4d"/> by a <see cref="M34d"/>.
        /// </summary>
        public static V4d Transform(M34d m, V4d v)
        {
            return new V4d(
                m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z + m.M03 * v.W,
                m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z + m.M13 * v.W,
                m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z + m.M23 * v.W,
                v.W);
        }

        /// <summary>
        /// Transforms <see cref="V3d"/> direction by a transposed <see cref="M34d"/>.
        /// </summary>
        public static V3d TransposedTransformDir(M34d m, V3d v)
        {
            return new V3d(
                m.M00 * v.X + m.M10 * v.Y + m.M20 * v.Z,
                m.M01 * v.X + m.M11 * v.Y + m.M21 * v.Z,
                m.M02 * v.X + m.M12 * v.Y + m.M22 * v.Z
                );
        }

        /// <summary>
        /// Transforms <see cref="V3d"/> position by a transposed <see cref="M34d"/>.
        /// </summary>
        public static V3d TransposedTransformPos(M34d m, V3d p)
        {
            return new V3d(
                m.M00 * p.X + m.M10 * p.Y + m.M20 * p.Z,
                m.M01 * p.X + m.M11 * p.Y + m.M21 * p.Z,
                m.M02 * p.X + m.M12 * p.Y + m.M22 * p.Z
                );
        }

        /// <summary>
        /// Transforms <see cref="V3d"/> position by a transposed <see cref="M34d"/>.
        /// </summary>
        public static V3d TransposedTransformPosProj(M34d m, V3d p)
        {
            double s = 1 / (m.M03 * p.X + m.M13 * p.Y + m.M23 * p.Z + 1);
            return (TransposedTransformDir(m, p)) * s;
        }

        #endregion

        #region Arithmetic Operators

        public static M44d operator +(M34d a, M44d b)
        {
            return M34d.Add(a, b);
        }

        public static M44d operator -(M34d a, M44d b)
        {
            return M34d.Subtract(a, b);
        }

        public static M44d operator -(M44d a, M34d b)
        {
            return M34d.Subtract(a, b);
        }

        public static M34d operator *(M34d m, Scale3d s)
        {
            return M34d.Multiply(m, s);
        }

        public static M34d operator *(M34d m, Shift3d t)
        {
            return M34d.Multiply(m, t);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks if a <see cref="M34d"/> and a <see cref="M44d"/> are equal.
        /// </summary>
        public static bool operator ==(M34d a, M44d b)
        {
            return
                a.M00 == b.M00 && a.M01 == b.M01 && a.M02 == b.M02 && a.M03 == b.M03 &&
                a.M10 == b.M10 && a.M11 == b.M11 && a.M12 == b.M12 && a.M13 == b.M13 &&
                a.M20 == b.M20 && a.M21 == b.M21 && a.M22 == b.M22 && a.M23 == b.M23 &&
                    0 == b.M30 &&     0 == b.M31 &&     0 == b.M32 &&     1 == b.M33
                ;
        }

        /// <summary>
        /// Checks if a <see cref="M34d"/> and a <see cref="M44d"/> are equal.
        /// </summary>
        public static bool operator ==(M44d a, M34d b)
        {
            return b == a;
        }

        /// <summary>
        /// Checks if a <see cref="M34d"/> and a <see cref="M44d"/> are different
        /// </summary>
        public static bool operator !=(M34d a, M44d b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Checks if a <see cref="M34d"/> and a <see cref="M44d"/> are different
        /// </summary>
        public static bool operator !=(M44d a, M34d b)
        {
            return !(a == b);
        }

        #endregion

        #region Static Creators

        public static M34d Translation(double tx, double ty, double tz)
        {
            return new M34d(
                1, 0, 0, tx,
                0, 1, 0, ty,
                0, 0, 1, tz
                );
        }

        public static M34d Translation(V3d vector)
        {
            return new M34d(1, 0, 0, vector.X,
                            0, 1, 0, vector.Y,
                            0, 0, 1, vector.Z);
        }

        public static M34d Translation(Shift3d shift)
        {
            return new M34d(1, 0, 0, shift.X,
                            0, 1, 0, shift.Y,
                            0, 0, 1, shift.Z);
        }

        public static M34d Scale(double scaleFactor)
        {
            return new M34d(scaleFactor, 0, 0, 0,
                            0, scaleFactor, 0, 0,
                            0, 0, scaleFactor, 0);
        }

        public static M34d Scale(double sx, double sy, double sz)
        {
            return new M34d(sx, 0, 0, 0,
                            0, sy, 0, 0,
                            0, 0, sz, 0);
        }

        public static M34d Scale(V3d scaleFactors)
        {
            return new M34d(scaleFactors.X, 0, 0, 0,
                            0, scaleFactors.Y, 0, 0,
                            0, 0, scaleFactors.Z, 0);
        }

        public static M34d Scale(Scale3d scale)
        {
            return new M34d(scale.X, 0, 0, 0,
                            0, scale.Y, 0, 0,
                            0, 0, scale.Z, 0);
        }

        public static M34d Rotation(V3d axis, double angleRadians)
        {
            return (M34d)(new Rot3d(axis, angleRadians));
        }

        public static M34d Rotation(Rot3d q)
        {
            return (M34d)q;
        }

        public static M34d RotationX(double angleRadians)
        {
            double cos = (double)System.Math.Cos(angleRadians);
            double sin = (double)System.Math.Sin(angleRadians);

            return new M34d(1, 0, 0, 0,
                            0, cos, -sin, 0,
                            0, sin,  cos, 0);
        }

        public static M34d RotationY(double angleRadians)
        {
            double cos = (double)System.Math.Cos(angleRadians);
            double sin = (double)System.Math.Sin(angleRadians);

            return new M34d(cos, 0, -sin, 0,
                            0, 1, 0, 0,
                            sin, 0,  cos, 0);
        }

        public static M34d RotationZ(double angleRadians)
        {
            double cos = (double)System.Math.Cos(angleRadians);
            double sin = (double)System.Math.Sin(angleRadians);

            return new M34d(cos, -sin, 0, 0,
                            sin,  cos, 0, 0,
                            0, 0, 1, 0);
        }

        #endregion

        #region ITransform<V3d> implementation.

        /// <summary>
        /// Returns the trace of this matrix.
        /// The trace is defined as the sum of the diagonal elements,
        /// and is only defined for square matrices.
        /// </summary>
        public double Trace
        {
            get { return M00 + M11 + M22 + 1; }
        }

        /// <summary>
        /// Returns the determinant of this matrix.
        /// The determinant is only defined for square matrices.
        /// </summary>
        public double Det
        {
            get
            {
                return
                    M00 * M11 * M22 + M01 * M12 * M20 + M02 * M10 * M21
                  - M20 * M11 * M02 - M21 * M12 * M00 - M22 * M10 * M01
                  ;
            }
        }

        /// <summary>
        /// Returns whether this matrix is invertible.
        /// A square matrix is invertible if its determinant is not zero.
        /// </summary>
        public bool Invertible { get { return Det != 0.0f; } }

        /// <summary>
        /// Returns whether this matrix is singular.
        /// A matrix is singular if its determinant is zero.
        /// </summary>
        public bool Singular { get { return Det == 0.0f; } }

        /// <summary>
        /// Transposes this matrix.
        /// </summary>
        /// <returns>This.</returns>
        public void Transpose()
        {
            Fun.Swap(ref M01, ref M10);
            Fun.Swap(ref M02, ref M20);
            this.M03 = 0;
            Fun.Swap(ref M12, ref M21);
            this.M13 = 0;
            this.M23 = 0;
        }

        /// <summary>
        /// Converts this matrix to its adjoint.
        /// </summary>
        /// <returns>This.</returns>
        public M44d Adjoin()
        {
            return (M44d)Adjoint;
        }

        /// <summary>
        /// Returns adjoint of this matrix.
        /// </summary>
        public M44d Adjoint
        {
            get
            {
                M44d result = new M44d();
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
        /// Inverts this transform.
        /// </summary>
        public bool Invert()
        {
            var mx = (M44d)this;
            var success = mx.Invert();
            this = (M34d)mx;
            return success;
        }

        /// <summary>
        /// Returns inverse of this transform.
        /// </summary>
        public M34d Inverse
        {
            get
            {
                var mx = (M44d)this;
                return (M34d)(mx.Inverse);
            }
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public V4d Transform(V4d v)
        {
            return new V4d(
                M00 * v.X + M01 * v.Y + M02 * v.Z + M03 * v.W,
                M10 * v.X + M11 * v.Y + M12 * v.Z + M13 * v.W,
                M20 * v.X + M21 * v.Y + M22 * v.Z + M23 * v.W,
                1 * v.W
                );
        }

        #endregion

    }
}
