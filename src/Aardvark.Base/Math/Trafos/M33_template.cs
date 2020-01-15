using System;
using System.Text;

namespace Aardvark.Base
{

    //# foreach (var isDouble in new[] { false, true }) {
    //#   var ft = isDouble ? "double" : "float";
    //#   var x2t = isDouble ? "2d" : "2f";
    //#   var x3t = isDouble ? "3d" : "3f";
    //#   var x4t = isDouble ? "4d" : "4f";
    public partial struct M3__x3t__ : ISquareMatrix<M3__x3t__, V__x3t__, V__x2t__, __ft__>
    {

        #region Matrix Arithmetics

        public static M3__x3t__ Multiply(M3__x3t__ m, Scale__x3t__ s)
        {
            return new M3__x3t__(
                m.M00 * s.X, m.M01 * s.Y, m.M02 * s.Z,
                m.M10 * s.X, m.M11 * s.Y, m.M12 * s.Z,
                m.M20 * s.X, m.M21 * s.Y, m.M22 * s.Z
                );
        }

        public static M3__x4t__ Multiply(M3__x3t__ m, Shift__x3t__ t)
        {
            return new M3__x4t__(
               m.M00, m.M01, m.M02, m.M00 * t.X + m.M01 * t.Y + m.M02 * t.Z,
               m.M10, m.M11, m.M12, m.M10 * t.X + m.M11 * t.Y + m.M12 * t.Z,
               m.M20, m.M21, m.M22, m.M20 * t.X + m.M21 * t.Y + m.M22 * t.Z
               );
        }

        public static M4__x4t__ Enlarge(M3__x3t__ m)
        {
            M4__x4t__ enlarged = new M4__x4t__(
                    m.M00, m.M01, m.M02, 0,
                    m.M10, m.M11, m.M12, 0,
                    m.M20, m.M21, m.M22, 0,
                    0, 0, 0, 1.0f);
            return enlarged;
        }

        public static V__x3t__ Transform(M3__x3t__ m, V__x3t__ v)
        {
            return new V__x3t__(
                m.M00 * v.X + m.M01 * v.Y + m.M02 * v.Z,
                m.M10 * v.X + m.M11 * v.Y + m.M12 * v.Z,
                m.M20 * v.X + m.M21 * v.Y + m.M22 * v.Z
                );
        }

        public static V__x3t__ TransposedTransform(M3__x3t__ m, V__x3t__ v)
        {
            return new V__x3t__(
                m.M00 * v.X + m.M10 * v.Y + m.M20 * v.Z,
                m.M01 * v.X + m.M11 * v.Y + m.M21 * v.Z,
                m.M02 * v.X + m.M12 * v.Y + m.M22 * v.Z
                );
        }


        #endregion

        #region Basic Operators

        public static M3__x3t__ operator *(M3__x3t__ m, Scale__x3t__ s)
        {
            return M3__x3t__.Multiply(m, s);
        }

        public static M3__x4t__ operator *(M3__x3t__ m, Shift__x3t__ t)
        {
            return M3__x3t__.Multiply(m, t);
        }

        #endregion

        #region Static Creators

        /// <summary>
        /// Creates new Identity with 2 __ft__ values for translation.
        /// </summary>
        public static M3__x3t__ Translation(__ft__ dx, __ft__ dy)
        {
            return new M3__x3t__(1, 0, dx,
                            0, 1, dy,
                            0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with V__x3t__ vector for translation.
        /// </summary>
        public static M3__x3t__ Translation(V__x2t__ v)
        {
            return new M3__x3t__(1, 0, v.X,
                            0, 1, v.Y,
                            0, 0, 1);
        }

#if TODO

        /// <summary>
        /// Creates new Identity <see cref="M4__x4t__"/> with a <see cref="Shift__x3t__"/> for translation.
        /// </summary>
        /// <param name="shift"></param>
        /// <returns>A <see cref="M4__x4t__"/> translation matrix.</returns>
        public static M4__x4t__ Translation(Shift__x3t__ shift)
        {
            return new M4__x4t__(1, 0, 0, shift.X,
                            0, 1, 0, shift.Y,
                            0, 0, 1, shift.Z,
                            0, 0, 0, 1);
        }
#endif
        /// <summary>
        /// Creates new Identity with scalar value for uniform-scaling.
        /// </summary>
        /// <returns>Uniform-scaling matrix.</returns>
        public static M3__x3t__ Scale(__ft__ s)
        {
            return new M3__x3t__(s, 0, 0,
                            0, s, 0,
                            0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with 3 scalar values for scaling.
        /// </summary>
        /// <returns>Scaling matrix.</returns>
        public static M3__x3t__ Scale(__ft__ sx, __ft__ sy)
        {
            return new M3__x3t__(sx, 0, 0,
                            0, sy, 0,
                            0, 0, 1);
        }

        /// <summary>
        /// Creates new Identity with V__x3t__ for scaling.
        /// </summary>
        /// <returns>Scaling matrix.</returns>
        public static M3__x3t__ Scale(V__x2t__ v)
        {
            return new M3__x3t__(v.X, 0, 0,
                            0, v.Y, 0,
                            0, 0, 1);
        }

#if TODO
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
#endif
        /// <summary>
        /// Creates rotation matrix from axis and angle.
        /// </summary>
        public static M3__x3t__ Rotation(__ft__ angleInRadians)
        {
            return Rotation(new Rot__x2t__(angleInRadians));
        }


        /// <summary>
        /// Creates rotational matrix from quaternion.
        /// </summary>
        /// <returns>Rotational matrix.</returns>
        public static M3__x3t__ Rotation(Rot__x2t__ r)
        {
            return (M3__x3t__)r;
        }

        #endregion

        #region Coordinate-System Transforms

        /// <summary>
        /// Computes from a <see cref="V__x3t__"/> normal the transformation matrix
        /// from the local coordinate system where the normal is the z-axis to
        /// the global coordinate system.
        /// </summary>
        /// <param name="normal">The normal vector of the new ground plane.</param>
        public static M3__x3t__ NormalFrameLocal2Global(V__x3t__ normal)
        {
            V__x3t__ min;
            double x = Fun.Abs(normal.X);
            double y = Fun.Abs(normal.Y);
            double z = Fun.Abs(normal.Z);

            if (x < y)
            {
                if (x < z) { min = V__x3t__.XAxis; } else { min = V__x3t__.ZAxis; }
            }
            else
            {
                if (y < z) { min = V__x3t__.YAxis; } else { min = V__x3t__.ZAxis; }
            }

            var xVec = V__x3t__.Cross(normal, min);
            xVec.Normalize(); // this is now guaranteed to be normal to the input normal
            var yVec = V__x3t__.Cross(normal, xVec);
            yVec.Normalize();
            var zVec = normal;
            zVec.Normalize();

            return new M3__x3t__(xVec.X, yVec.X, zVec.X,
                            xVec.Y, yVec.Y, zVec.Y,
                            xVec.Z, yVec.Z, zVec.Z);
        }

        #endregion

        #region ITransform<V__x3t__> implementation.

        /// <summary>
        /// Converts this matrix to its adjoint.
        /// </summary>
        /// <returns>This.</returns>
        public M3__x3t__ Adjoin()
        {
            return this = Adjoint;
        }

        /// <summary>
        /// Returns adjoint of this matrix.
        /// </summary>
        public M3__x3t__ Adjoint
        {
            get
            {
                M4__x4t__ ad = (M4__x4t__)M3__x3t__.Enlarge(this);
                M4__x4t__ result = new M4__x4t__();
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
        public V__x3t__ Transform(V__x3t__ v)
        {
            return Transform(this, v);
        }

        /// <summary>
        /// Transforms vector v by the transposed version of this.
        /// </summary>
        public V__x3t__ TransposedTransform(V__x3t__ v)
        {
            return TransposedTransform(this, v);
        }

        #endregion

        #region V__x3t__ Extensions
        /// <summary>
        /// Returns the skew-symmetric "cross" matrix (A^T = -A) of the vector v.
        /// </summary>
        public static M3__x3t__ CrossMatrix(V__x3t__ v)
        {
            return new M3__x3t__(
                0, -v.Z, +v.Y,
                +v.Z, 0, -v.X,
                -v.Y, +v.X, 0);
        }

        /// <summary>
        /// Returns the outer product (tensor-product) of v1 * v2^T as a 3x3 Matrix.
        /// </summary>
        public static M3__x3t__ OuterProduct(V__x3t__ v1, V__x3t__ v2)
        {
            return new M3__x3t__(
                v2.X * v1.X, v2.Y * v1.X, v2.Z * v1.X,
                v2.X * v1.Y, v2.Y * v1.Y, v2.Z * v1.Y,
                v2.X * v1.Z, v2.Y * v1.Z, v2.Z * v1.Z);
        }
        #endregion

    }

    public static partial class M33Extensions
    {
        #region V__x3t__ Extensions
        /// <summary>
        /// Returns the skew-symmetric "cross" matrix (A^T = -A) of the vector v.
        /// </summary>
        public static M3__x3t__ CrossMatrix(this V__x3t__ v)
        {
            return new M3__x3t__(
                0, -v.Z, +v.Y,
                +v.Z, 0, -v.X,
                -v.Y, +v.X, 0);
        }

        /// <summary>
        /// Returns the outer product (tensor-product) of v1 * v2^T as a 3x3 Matrix.
        /// </summary>
        public static M3__x3t__ OuterProduct(this V__x3t__ v1, V__x3t__ v2)
        {
            return new M3__x3t__(
                v2.X * v1.X, v2.Y * v1.X, v2.Z * v1.X,
                v2.X * v1.Y, v2.Y * v1.Y, v2.Z * v1.Y,
                v2.X * v1.Z, v2.Y * v1.Z, v2.Z * v1.Z);
        }
        #endregion
    }
    //# } // isDouble
}
