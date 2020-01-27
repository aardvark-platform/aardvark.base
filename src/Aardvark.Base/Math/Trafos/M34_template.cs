using System;
using System.Text;

namespace Aardvark.Base
{
    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    public partial struct M3__x4t__ : IMatrix<M3__x4t__, V__x4t__, V__x3t__, __ft__>
    {

        #region Matrix Arithmetics

        public static M4__x4t__ Add(M3__x4t__ a, M4__x4t__ b)
        {
            return new M4__x4t__(
                a.M00 + b.M00, a.M01 + b.M01, a.M02 + b.M02, a.M03 + b.M03,
                a.M10 + b.M10, a.M11 + b.M11, a.M12 + b.M12, a.M13 + b.M13,
                a.M20 + b.M20, a.M21 + b.M21, a.M22 + b.M22, a.M23 + b.M23,
                b.M30, b.M31, b.M32, 1 + b.M33
                );
        }

        public static M4__x4t__ Subtract(M3__x4t__ a, M4__x4t__ b)
        {
            return new M4__x4t__(
                a.M00 - b.M00, a.M01 - b.M01, a.M02 - b.M02, a.M03 - b.M03,
                a.M10 - b.M10, a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
                a.M20 - b.M20, a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23,
                -b.M30, -b.M31, -b.M32, 1 - b.M33
                );
        }

        public static M4__x4t__ Subtract(M4__x4t__ a, M3__x4t__ b)
        {
            return new M4__x4t__(
                a.M00 - b.M00, a.M01 - b.M01, a.M02 - b.M02, a.M03 - b.M03,
                a.M10 - b.M10, a.M11 - b.M11, a.M12 - b.M12, a.M13 - b.M13,
                a.M20 - b.M20, a.M21 - b.M21, a.M22 - b.M22, a.M23 - b.M23,
                a.M30, a.M31, a.M32, a.M33 - 1
                );
        }

        public static M3__x4t__ Multiply(M3__x4t__ m, Scale__x3t__ s)
        {
            return new M3__x4t__(
                m.M00 * s.X, m.M01 * s.Y, m.M02 * s.Z, m.M03,
                m.M10 * s.X, m.M11 * s.Y, m.M12 * s.Z, m.M13,
                m.M20 * s.X, m.M21 * s.Y, m.M22 * s.Z, m.M23
                );
        }

        public static M3__x4t__ Multiply(M3__x4t__ m, Shift__x3t__ t)
        {
            return new M3__x4t__(
                m.M00, m.M01, m.M02, m.M00 * t.X + m.M01 * t.Y + m.M02 * t.Z + m.M03,
                m.M10, m.M11, m.M12, m.M10 * t.X + m.M11 * t.Y + m.M12 * t.Z + m.M13,
                m.M20, m.M21, m.M22, m.M20 * t.X + m.M21 * t.Y + m.M22 * t.Z + m.M23
                );
        }

        /// <summary>
        /// Transforms a <see cref="M3__x4t__"/> to a <see cref="M3__x3t__"/> by deleting the
        /// specified row and column. Internally the <see cref="M3__x4t__"/> is tarnsformed 
        /// to a <see cref="M4__x4t__"/> to delete the row and column.
        /// </summary>
        /// <param name="deleted_row">Row to delete.</param>
        /// <param name="deleted_column">Column to delete.</param>
        /// <returns>A <see cref="M3__x3t__"/>.</returns>
        public M3__x3t__ Minor(int deleted_row, int deleted_column)
        {
            M4__x4t__ temp = (M4__x4t__)this;

            M3__x3t__ result = new M3__x3t__();
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
        /// Calculates the determinant of a <see cref="M3__x4t__"/>.
        /// </summary>
        /// <returns>Determinant as a __ft__.</returns>
        public __ft__ Determinant()
        {
            __ft__ determinant = 0.0f;

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
        /// Transforms a <see cref="V__x4t__"/> by a <see cref="M3__x4t__"/>.
        /// </summary>
        public static V__x4t__ Transform(M3__x4t__ m, V__x4t__ v)
        {
            return new V__x4t__(
                m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z + m.M03 * v.W,
                m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z + m.M13 * v.W,
                m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z + m.M23 * v.W,
                v.W);
        }

        /// <summary>
        /// Transforms <see cref="V__x3t__"/> direction by a transposed <see cref="M3__x4t__"/>.
        /// </summary>
        public static V__x3t__ TransposedTransformDir(M3__x4t__ m, V__x3t__ v)
        {
            return new V__x3t__(
                m.M00 * v.X + m.M10 * v.Y + m.M20 * v.Z,
                m.M01 * v.X + m.M11 * v.Y + m.M21 * v.Z,
                m.M02 * v.X + m.M12 * v.Y + m.M22 * v.Z
                );
        }

        /// <summary>
        /// Transforms <see cref="V__x3t__"/> position by a transposed <see cref="M3__x4t__"/>.
        /// </summary>
        public static V__x3t__ TransposedTransformPos(M3__x4t__ m, V__x3t__ p)
        {
            return new V__x3t__(
                m.M00 * p.X + m.M10 * p.Y + m.M20 * p.Z,
                m.M01 * p.X + m.M11 * p.Y + m.M21 * p.Z,
                m.M02 * p.X + m.M12 * p.Y + m.M22 * p.Z
                );
        }

        /// <summary>
        /// Transforms <see cref="V__x3t__"/> position by a transposed <see cref="M3__x4t__"/>.
        /// </summary>
        public static V__x3t__ TransposedTransformPosProj(M3__x4t__ m, V__x3t__ p)
        {
            __ft__ s = 1 / (m.M03 * p.X + m.M13 * p.Y + m.M23 * p.Z + 1);
            return (TransposedTransformDir(m, p)) * s;
        }

        #endregion

        #region Arithmetic Operators

        public static M4__x4t__ operator +(M3__x4t__ a, M4__x4t__ b)
        {
            return M3__x4t__.Add(a, b);
        }

        public static M4__x4t__ operator -(M3__x4t__ a, M4__x4t__ b)
        {
            return M3__x4t__.Subtract(a, b);
        }

        public static M4__x4t__ operator -(M4__x4t__ a, M3__x4t__ b)
        {
            return M3__x4t__.Subtract(a, b);
        }

        public static M3__x4t__ operator *(M3__x4t__ m, Scale__x3t__ s)
        {
            return M3__x4t__.Multiply(m, s);
        }

        public static M3__x4t__ operator *(M3__x4t__ m, Shift__x3t__ t)
        {
            return M3__x4t__.Multiply(m, t);
        }

        #endregion

        #region Comparison Operators

        /// <summary>
        /// Checks if a <see cref="M3__x4t__"/> and a <see cref="M4__x4t__"/> are equal.
        /// </summary>
        public static bool operator ==(M3__x4t__ a, M4__x4t__ b)
        {
            return
                a.M00 == b.M00 && a.M01 == b.M01 && a.M02 == b.M02 && a.M03 == b.M03 &&
                a.M10 == b.M10 && a.M11 == b.M11 && a.M12 == b.M12 && a.M13 == b.M13 &&
                a.M20 == b.M20 && a.M21 == b.M21 && a.M22 == b.M22 && a.M23 == b.M23 &&
                    0 == b.M30 && 0 == b.M31 && 0 == b.M32 && 1 == b.M33
                ;
        }

        /// <summary>
        /// Checks if a <see cref="M3__x4t__"/> and a <see cref="M4__x4t__"/> are equal.
        /// </summary>
        public static bool operator ==(M4__x4t__ a, M3__x4t__ b)
        {
            return b == a;
        }

        /// <summary>
        /// Checks if a <see cref="M3__x4t__"/> and a <see cref="M4__x4t__"/> are different
        /// </summary>
        public static bool operator !=(M3__x4t__ a, M4__x4t__ b)
        {
            return !(a == b);
        }

        /// <summary>
        /// Checks if a <see cref="M3__x4t__"/> and a <see cref="M4__x4t__"/> are different
        /// </summary>
        public static bool operator !=(M4__x4t__ a, M3__x4t__ b)
        {
            return !(a == b);
        }

        #endregion

        #region Static Creators

        public static M3__x4t__ Translation(__ft__ tx, __ft__ ty, __ft__ tz)
        {
            return new M3__x4t__(
                1, 0, 0, tx,
                0, 1, 0, ty,
                0, 0, 1, tz
                );
        }

        public static M3__x4t__ Translation(V__x3t__ vector)
        {
            return new M3__x4t__(1, 0, 0, vector.X,
                            0, 1, 0, vector.Y,
                            0, 0, 1, vector.Z);
        }

        public static M3__x4t__ Translation(Shift__x3t__ shift)
        {
            return new M3__x4t__(1, 0, 0, shift.X,
                            0, 1, 0, shift.Y,
                            0, 0, 1, shift.Z);
        }

        public static M3__x4t__ Scale(__ft__ scaleFactor)
        {
            return new M3__x4t__(scaleFactor, 0, 0, 0,
                            0, scaleFactor, 0, 0,
                            0, 0, scaleFactor, 0);
        }

        public static M3__x4t__ Scale(__ft__ sx, __ft__ sy, __ft__ sz)
        {
            return new M3__x4t__(sx, 0, 0, 0,
                            0, sy, 0, 0,
                            0, 0, sz, 0);
        }

        public static M3__x4t__ Scale(V__x3t__ scaleFactors)
        {
            return new M3__x4t__(scaleFactors.X, 0, 0, 0,
                            0, scaleFactors.Y, 0, 0,
                            0, 0, scaleFactors.Z, 0);
        }

        public static M3__x4t__ Scale(Scale__x3t__ scale)
        {
            return new M3__x4t__(scale.X, 0, 0, 0,
                            0, scale.Y, 0, 0,
                            0, 0, scale.Z, 0);
        }

        public static M3__x4t__ Rotation(V__x3t__ axis, __ft__ angleRadians)
        {
            return (M3__x4t__)(new Rot__x3t__(axis, angleRadians));
        }

        public static M3__x4t__ Rotation(Rot__x3t__ q)
        {
            return (M3__x4t__)q;
        }

        public static M3__x4t__ RotationX(__ft__ angleRadians)
        {
            __ft__ cos = Fun.Cos(angleRadians);
            __ft__ sin = Fun.Sin(angleRadians);

            return new M3__x4t__(1, 0, 0, 0,
                            0, cos, -sin, 0,
                            0, sin, cos, 0);
        }

        public static M3__x4t__ RotationY(__ft__ angleRadians)
        {
            __ft__ cos = Fun.Cos(angleRadians);
            __ft__ sin = Fun.Sin(angleRadians);

            return new M3__x4t__(cos, 0, -sin, 0,
                            0, 1, 0, 0,
                            sin, 0, cos, 0);
        }

        public static M3__x4t__ RotationZ(__ft__ angleRadians)
        {
            __ft__ cos = Fun.Cos(angleRadians);
            __ft__ sin = Fun.Sin(angleRadians);

            return new M3__x4t__(cos, -sin, 0, 0,
                            sin, cos, 0, 0,
                            0, 0, 1, 0);
        }

        #endregion

        #region ITransform<V__x3t__> implementation.

        /// <summary>
        /// Returns the trace of this matrix.
        /// The trace is defined as the sum of the diagonal elements,
        /// and is only defined for square matrices.
        /// </summary>
        public __ft__ Trace
        {
            get { return M00 + M11 + M22 + 1; }
        }

        /// <summary>
        /// Returns the determinant of this matrix.
        /// The determinant is only defined for square matrices.
        /// </summary>
        public __ft__ Det
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
        public M4__x4t__ Adjoin()
        {
            return (M4__x4t__)Adjoint;
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
        /// Inverts this transform.
        /// </summary>
        public bool Invert()
        {
            var mx = (M4__x4t__)this;
            var success = mx.Invert();
            this = (M3__x4t__)mx;
            return success;
        }

        /// <summary>
        /// Returns inverse of this transform.
        /// </summary>
        public M3__x4t__ Inverse
        {
            get
            {
                var mx = (M4__x4t__)this;
                return (M3__x4t__)(mx.Inverse);
            }
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public V__x4t__ Transform(V__x4t__ v)
        {
            return new V__x4t__(
                M00 * v.X + M01 * v.Y + M02 * v.Z + M03 * v.W,
                M10 * v.X + M11 * v.Y + M12 * v.Z + M13 * v.W,
                M20 * v.X + M21 * v.Y + M22 * v.Z + M23 * v.W,
                1 * v.W
                );
        }

        #endregion

    }
    //# } // isDouble
}
