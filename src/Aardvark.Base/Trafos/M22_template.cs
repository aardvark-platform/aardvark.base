using System;
using System.Text;

namespace Aardvark.Base
{

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var x2t = isDouble ? "2d" : "2f";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    public partial struct M2__x2t__ : ISquareMatrix<M2__x2t__, V__x2t__, __ft__, __ft__>
    {
        #region Matrix Arithmetics

        /// <summary>
        /// Multiplies matrix with a V__x3t__.
        /// </summary>
        public static V__x3t__ Multiply(M2__x2t__ mat, V__x3t__ vec)
        {
            return new V__x3t__(mat.M00 * vec.X + mat.M01 * vec.Y,
                           mat.M10 * vec.X + mat.M11 * vec.Y,
                           vec.Z);
        }

        /// <summary>
        /// Multiplies matrix with a V__x4t__.
        /// </summary>
        public static V__x4t__ Multiply(M2__x2t__ mat, V__x4t__ vec)
        {
            return new V__x4t__(mat.M00 * vec.X + mat.M01 * vec.Y,
                           mat.M10 * vec.X + mat.M11 * vec.Y,
                           vec.Z,
                           vec.W);
        }

        public static M3__x3t__ Multiply(M2__x2t__ matrix, Scale__x3t__ scale)
        {
            return new M3__x3t__(matrix.M00 * scale.X,
                             matrix.M01 * scale.Y,
                             0,

                             matrix.M10 * scale.X,
                             matrix.M11 * scale.Y,
                             0,

                             0,
                             0,
                             scale.Z);
        }

        public static M3__x4t__ Multiply(M2__x2t__ matrix, Shift__x3t__ shift)
        {
            return new M3__x4t__(matrix.M00,
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

        public static M3__x3t__ Enlarge(M2__x2t__ m)
        {
            return new M3__x3t__(m.M00, m.M01, 0, m.M10, m.M11, 0, 0, 0, 1);
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public static V__x2t__ Transform(M2__x2t__ m, V__x2t__ v)
        {
            return new V__x2t__(
                m.M00 * v.X + m.M01 * v.Y,
                m.M10 * v.X + m.M11 * v.Y
                );
        }

        #endregion

        #endregion

        #region Multiplication

        /// <summary>
        /// </summary>
        public static V__x3t__ operator *(M2__x2t__ m, V__x3t__ n)
        {
            return M2__x2t__.Multiply(m, n);
        }

        /// <summary>
        /// </summary>
        public static V__x4t__ operator *(M2__x2t__ m, V__x4t__ n)
        {
            return M2__x2t__.Multiply(m, n);
        }

        /// <summary>
        /// </summary>
        public static M3__x3t__ operator *(M2__x2t__ m, Scale__x3t__ n)
        {
            return M2__x2t__.Multiply(m, n);
        }

        /// <summary>
        /// </summary>
        public static M3__x4t__ operator *(M2__x2t__ m, Shift__x3t__ n)
        {
            return M2__x2t__.Multiply(m, n);
        }

        /// <summary>
        /// Multiplies a matrix with given rotation also represented as 2x2 matrix.
        /// </summary>
        public static M2__x2t__ operator *(M2__x2t__ m, Rot__x2t__ q)
        {
            return m * (M2__x2t__)q;
        }

        #endregion

        #region ITransform<V__x3t__> implementation.

        /// <summary>
        /// Converts this matrix to its adjoint.
        /// </summary>
        /// <returns>This.</returns>
        public M2__x2t__ Adjoin()
        {
            return this = Adjoint;
        }

        /// <summary>
        /// Returns adjoint of this matrix.
        /// </summary>
        public M2__x2t__ Adjoint
        {
            get
            {
                return new M2__x2t__(M11, -M10,
                                -M01, M00);
            }
        }

        /// <summary>
        /// Transforms vector v.
        /// </summary>
        public V__x2t__ Transform(V__x2t__ v)
        {
            return this * v;
        }

        #endregion

        #region Static creators

        /// <summary>
        /// Creates a 2D rotation matrix with the specified angle in radians.
        /// </summary>
        /// <returns>2D Rotation Matrix</returns>
        public static M2__x2t__ Rotation(__ft__ angleInRadians)
        {
            __ft__ cos = Fun.Cos(angleInRadians);
            __ft__ sin = Fun.Sin(angleInRadians);
            return new M2__x2t__(cos, -sin,
                            sin, cos);
        }

        #endregion
    }

    public static partial class M22Extensions
    {
        #region V__x2t__ Extensions
        /// <summary>
        /// Returns the outer product (tensor-product) of v1 * v2^T as a 3x3 Matrix.
        /// </summary>
        public static M2__x2t__ OuterProduct(this V__x2t__ v1, V__x2t__ v2)
        {
            return new M2__x2t__(
                v2.X * v1.X, v2.Y * v1.X,
                v2.X * v1.Y, v2.Y * v1.Y);
        }
        #endregion
    }
    //# } // isDouble
}
