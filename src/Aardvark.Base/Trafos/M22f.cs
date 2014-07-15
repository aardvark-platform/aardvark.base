using System;
using System.Text;

namespace Aardvark.Base
{
    
    public partial struct M22f : ISquareMatrix<M22f, V2f, float, float>
    {
        #region Matrix Arithmetics

        /// <summary>
        /// Multiplies matrix with a V3f.
        /// </summary>
        public static V3f Multiply(M22f mat, V3f vec)
        {
            return new V3f(mat.M00 * vec.X + mat.M01 * vec.Y,
                           mat.M10 * vec.X + mat.M11 * vec.Y,
                           vec.Z);
        }

        /// <summary>
        /// Multiplies matrix with a V4f.
        /// </summary>
        public static V4f Multiply(M22f mat, V4f vec)
        {
            return new V4f(mat.M00 * vec.X + mat.M01 * vec.Y,
                           mat.M10 * vec.X + mat.M11 * vec.Y,
                           vec.Z,
                           vec.W);
        }

        public static M33f Multiply(M22f matrix, Scale3f scale)
        {
            return new M33f(matrix.M00 * scale.X,
                             matrix.M01 * scale.Y,
                             0,

                             matrix.M10 * scale.X,
                             matrix.M11 * scale.Y,
                             0,

                             0,
                             0,
                             scale.Z);
        }

        public static M34f Multiply(M22f matrix, Shift3f shift)
        {
            return new M34f(matrix.M00,
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

        public static M33f Enlarge(M22f m)
        {
            return new M33f(m.M00, m.M01, 0, m.M10, m.M11, 0, 0, 0, 1);
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public static V2f Transform(M22f m, V2f v)
        {
            return new V2f(
                m.M00 * v.X + m.M01 * v.Y,
                m.M10 * v.X + m.M11 * v.Y
                );
        }

        #endregion

        #endregion

        #region Multiplication

        /// <summary>
        /// </summary>
        public static V3f operator *(M22f m, V3f n)
        {
            return M22f.Multiply(m, n);
        }

        /// <summary>
        /// </summary>
        public static V4f operator *(M22f m, V4f n)
        {
            return M22f.Multiply(m, n);
        }

        /// <summary>
        /// </summary>
        public static M33f operator *(M22f m, Scale3f n)
        {
            return M22f.Multiply(m, n);
        }

        /// <summary>
        /// </summary>
        public static M34f operator *(M22f m, Shift3f n)
        {
            return M22f.Multiply(m, n);
        }

        /// <summary>
        /// Multiplies a matrix with given rotation also represented as 2x2 matrix.
        /// </summary>
        public static M22f operator *(M22f m, Rot2f q)
        {
            return m * (M22f)q;
        }

        #endregion

        #region ITransform<V3f> implementation.

        /// <summary>
        /// Converts this matrix to its adjoint.
        /// </summary>
        /// <returns>This.</returns>
        public M22f Adjoin()
        {
            return this = Adjoint;
        }

        /// <summary>
        /// Returns adjoint of this matrix.
        /// </summary>
        public M22f Adjoint
        {
            get
            {
                return new M22f(  M11, -M10,
                                - M01,  M00);
            }
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public V2f Transform(V2f v)
        {
            return this * v;
        }

        #endregion

        #region Static creators

        /// <summary>
        /// Creates a 2D rotation matrix with the specified angle in radians.
        /// </summary>
        /// <returns>2D Rotation Matrix</returns>
        public static M22f Rotation(float angleInRadians)
        {
            float cos = Fun.Cos(angleInRadians);
            float sin = Fun.Sin(angleInRadians);
            return new M22f(cos, -sin,
                            sin, cos);
        }

        #endregion
    }

    public static partial class M22Extensions
    {
        #region V2f Extensions
        /// <summary>
        /// Returns the outer product (tensor-product) of v1 * v2^T as a 3x3 Matrix.
        /// </summary>
        public static M22f OuterProduct(this V2f v1, V2f v2)
        {
            return new M22f(
                v2.X * v1.X, v2.Y * v1.X, 
                v2.X * v1.Y, v2.Y * v1.Y);
        }
        #endregion
    }
}
