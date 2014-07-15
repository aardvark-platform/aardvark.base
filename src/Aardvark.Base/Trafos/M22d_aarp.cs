using System;
using System.Text;

namespace Aardvark.Base
{
    
    public partial struct M22d : ISquareMatrix<M22d, V2d, double, double>
    {
        #region Matrix Arithmetics

        /// <summary>
        /// Multiplies matrix with a V3d.
        /// </summary>
        public static V3d Multiply(M22d mat, V3d vec)
        {
            return new V3d(mat.M00 * vec.X + mat.M01 * vec.Y,
                           mat.M10 * vec.X + mat.M11 * vec.Y,
                           vec.Z);
        }

        /// <summary>
        /// Multiplies matrix with a V4d.
        /// </summary>
        public static V4d Multiply(M22d mat, V4d vec)
        {
            return new V4d(mat.M00 * vec.X + mat.M01 * vec.Y,
                           mat.M10 * vec.X + mat.M11 * vec.Y,
                           vec.Z,
                           vec.W);
        }

        public static M33d Multiply(M22d matrix, Scale3d scale)
        {
            return new M33d(matrix.M00 * scale.X,
                             matrix.M01 * scale.Y,
                             0,

                             matrix.M10 * scale.X,
                             matrix.M11 * scale.Y,
                             0,

                             0,
                             0,
                             scale.Z);
        }

        public static M34d Multiply(M22d matrix, Shift3d shift)
        {
            return new M34d(matrix.M00,
                             matrix.M01,
                             0,
                             matrix.M00 * shift.X +
                             matrix.M01 * shift.Y,

                             matrix.M10,
                             matrix.M11,
                             0,
                             matrix.M10 * shift.X +
                             matrix.M11 * shift.Y,

                             0,
                             0,
                             1,
                             shift.Z);
        }

        #region Complex.

        public static M33d Enlarge(M22d m)
        {
            return new M33d(m.M00, m.M01, 0, m.M10, m.M11, 0, 0, 0, 1);
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public static V2d Transform(M22d m, V2d v)
        {
            return new V2d(
                m.M00 * v.X + m.M01 * v.Y,
                m.M10 * v.X + m.M11 * v.Y
                );
        }

        #endregion

        #endregion

        #region Multiplication

        /// <summary>
        /// </summary>
        public static V3d operator *(M22d m, V3d n)
        {
            return M22d.Multiply(m, n);
        }

        /// <summary>
        /// </summary>
        public static V4d operator *(M22d m, V4d n)
        {
            return M22d.Multiply(m, n);
        }

        /// <summary>
        /// </summary>
        public static M33d operator *(M22d m, Scale3d n)
        {
            return M22d.Multiply(m, n);
        }

        /// <summary>
        /// </summary>
        public static M34d operator *(M22d m, Shift3d n)
        {
            return M22d.Multiply(m, n);
        }

        /// <summary>
        /// Multiplies a matrix with given rotation also represented as 2x2 matrix.
        /// </summary>
        public static M22d operator *(M22d m, Rot2d q)
        {
            return m * (M22d)q;
        }

        #endregion

        #region ITransform<V3d> implementation.

        /// <summary>
        /// Converts this matrix to its adjoint.
        /// </summary>
        /// <returns>This.</returns>
        public M22d Adjoin()
        {
            return this = Adjoint;
        }

        /// <summary>
        /// Returns adjoint of this matrix.
        /// </summary>
        public M22d Adjoint
        {
            get
            {
                return new M22d(  M11, -M10,
                                - M01,  M00);
            }
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public V2d Transform(V2d v)
        {
            return this * v;
        }

        #endregion

        #region Static creators

        /// <summary>
        /// Creates a 2D rotation matrix with the specified angle in radians.
        /// </summary>
        /// <returns>2D Rotation Matrix</returns>
        public static M22d Rotation(double angleInRadians)
        {
            double cos = Fun.Cos(angleInRadians);
            double sin = Fun.Sin(angleInRadians);
            return new M22d(cos, -sin,
                            sin, cos);
        }

        #endregion
    }

    public static partial class M22Extensions
    {
        #region V2d Extensions
        /// <summary>
        /// Returns the outer product (tensor-product) of v1 * v2^T as a 3x3 Matrix.
        /// </summary>
        public static M22d OuterProduct(this V2d v1, V2d v2)
        {
            return new M22d(
                v2.X * v1.X, v2.Y * v1.X, 
                v2.X * v1.Y, v2.Y * v1.Y);
        }
        #endregion
    }
}
