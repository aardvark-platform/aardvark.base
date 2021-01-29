using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static partial class TensorNumericExtensions
    {
        #region Partially pivoting LU factorization of Matrix<float>

        /// <summary>
        /// Perform a partially pivoting LU factorization of the supplied
        /// matrix in-place, i.e A = inv(P) LU, so that any matrix equation
        /// A x = b can be solved using LU x = P b. The permutation matrix P
        /// is returned as a permutation vector, and the supplied matrix A is
        /// overwritten with its factorization LU, both in the parameter
        /// inputAndLuMatrix. If the supplied matrix is singular, null is
        /// returned.
        /// </summary>
        public static int[] LuFactorize(this Matrix<float> inputAndLuMatrix)
        {
            var n = inputAndLuMatrix.SX;
            if (n != inputAndLuMatrix.SY) throw new ArgumentException("cannot factorize non-square matrix");
            var p = new int[n];
            if (inputAndLuMatrix.Data.LuFactorize(inputAndLuMatrix.Origin,
                        inputAndLuMatrix.DX, inputAndLuMatrix.DY, p))
                return p;
            return null;
        }

        /// <summary>
        /// Solve a matrix equation of the form LU x = P b.
        /// LU is the supplied factorized matrix lu, P is a permutation matrix
        /// supplied as a permutation array p, and b is the right hand side
        /// supplied as a vector. The solution vector x is returned.
        /// </summary>
        public static Vector<float> LuSolve(this Matrix<float> luMatrix, int[] p, Vector<float> b)
        {
            var x = new Vector<float>(b.Info);
            luMatrix.Data.LuSolve(luMatrix.Origin, luMatrix.DX, luMatrix.DY, p,
                                  b.Data, b.Origin, b.Delta, x.Data, x.Origin, x.Delta);
            return x;
        }

        /// <summary>
        /// Solve a matrix equation of the form LU x = P b.
        /// LU is the supplied factorized matrix, P is a permutation matrix
        /// supplied as a permutation array, and b is the right hand side
        /// supplied as a matrix. The solution matrix x is returned.
        /// </summary>
        public static Matrix<float> LuSolve(this Matrix<float> luMatrix, int[] p, Matrix<float> b)
        {
            var x = new Matrix<float>(b.Info);
            luMatrix.Data.LuSolve(luMatrix.Origin, luMatrix.DX, luMatrix.DY, p,
                                  b.SX, b.Data, b.Origin, b.DX, b.DY, x.Data, x.Origin, x.DX, x.DY);
            return x;
        }

        /// <summary>
        /// Compute the inverse of an PLU factorized matrix, i.e LU x = P I.
        /// LU is the supplied factorized matrix, P is a permutation matrix
        /// supplied as a permutation array, and I is the square identity matrix.
        /// The inverse matrix x is returned.
        /// </summary>
        public static Matrix<float> LuInverse(this Matrix<float> luMatrix, int[] p)
        {
            var inv = new Matrix<float>(luMatrix.Info);
            luMatrix.Data.LuInverse(luMatrix.Origin, luMatrix.DX, luMatrix.DY, p,
                                    inv.Data, inv.Origin, inv.DX, inv.DY);
            return inv;
        }

        /// <summary>
        /// Compute the inverse of a square matrix using PLU factorization.
        /// The inverse matrix x is returned. Returns an invalid matrix if
        /// the factorization failed.
        /// </summary>
        public static Matrix<float> LuInverse(this Matrix<float> matrix)
        {
            var n = matrix.SX;
            if (n != matrix.SY) throw new ArgumentException("cannot invert non-square matrix");
            var lu = matrix.Copy();
            var p = new int[n];
            if (!lu.Data.LuFactorize(lu.Origin, lu.DX, lu.DY, p))
                return default(Matrix<float>);
            var inv = new Matrix<float>(matrix.Info);
            lu.Data.LuInverse(lu.Origin, lu.DX, lu.DY, p, inv.Data, inv.Origin, inv.DX, inv.DY);
            return inv;
        }

        /// <summary>
        /// Invert square matrix using PLU factorization in-place.
        /// If the factorization fails, the matrix is unchanged and
        /// false is returned.
        /// </summary>
        public static bool LuInvert(this Matrix<float> matrix)
        {
            var n = matrix.SX;
            if (n != matrix.SY) throw new ArgumentException("cannot invert non-square matrix");
            var lu = matrix.Copy();
            var p = new int[n];
            if (!lu.Data.LuFactorize(lu.Origin, lu.DX, lu.DY, p)) return false;
            lu.Data.LuInverse(lu.Origin, lu.DX, lu.DY, p, matrix.Data, matrix.Origin, matrix.DX, matrix.DY);
            return true;
        }

        #endregion

        #region Partially pivoting LU factorization of Matrix<double>

        /// <summary>
        /// Perform a partially pivoting LU factorization of the supplied
        /// matrix in-place, i.e A = inv(P) LU, so that any matrix equation
        /// A x = b can be solved using LU x = P b. The permutation matrix P
        /// is returned as a permutation vector, and the supplied matrix A is
        /// overwritten with its factorization LU, both in the parameter
        /// inputAndLuMatrix. If the supplied matrix is singular, null is
        /// returned.
        /// </summary>
        public static int[] LuFactorize(this Matrix<double> inputAndLuMatrix)
        {
            var n = inputAndLuMatrix.SX;
            if (n != inputAndLuMatrix.SY) throw new ArgumentException("cannot factorize non-square matrix");
            var p = new int[n];
            if (inputAndLuMatrix.Data.LuFactorize(inputAndLuMatrix.Origin,
                        inputAndLuMatrix.DX, inputAndLuMatrix.DY, p))
                return p;
            return null;
        }

        /// <summary>
        /// Solve a matrix equation of the form LU x = P b.
        /// LU is the supplied factorized matrix lu, P is a permutation matrix
        /// supplied as a permutation array p, and b is the right hand side
        /// supplied as a vector. The solution vector x is returned.
        /// </summary>
        public static Vector<double> LuSolve(this Matrix<double> luMatrix, int[] p, Vector<double> b)
        {
            var x = new Vector<double>(b.Info);
            luMatrix.Data.LuSolve(luMatrix.Origin, luMatrix.DX, luMatrix.DY, p,
                                  b.Data, b.Origin, b.Delta, x.Data, x.Origin, x.Delta);
            return x;
        }

        /// <summary>
        /// Solve a matrix equation of the form LU x = P b.
        /// LU is the supplied factorized matrix, P is a permutation matrix
        /// supplied as a permutation array, and b is the right hand side
        /// supplied as a matrix. The solution matrix x is returned.
        /// </summary>
        public static Matrix<double> LuSolve(this Matrix<double> luMatrix, int[] p, Matrix<double> b)
        {
            var x = new Matrix<double>(b.Info);
            luMatrix.Data.LuSolve(luMatrix.Origin, luMatrix.DX, luMatrix.DY, p,
                                  b.SX, b.Data, b.Origin, b.DX, b.DY, x.Data, x.Origin, x.DX, x.DY);
            return x;
        }

        /// <summary>
        /// Compute the inverse of an PLU factorized matrix, i.e LU x = P I.
        /// LU is the supplied factorized matrix, P is a permutation matrix
        /// supplied as a permutation array, and I is the square identity matrix.
        /// The inverse matrix x is returned.
        /// </summary>
        public static Matrix<double> LuInverse(this Matrix<double> luMatrix, int[] p)
        {
            var inv = new Matrix<double>(luMatrix.Info);
            luMatrix.Data.LuInverse(luMatrix.Origin, luMatrix.DX, luMatrix.DY, p,
                                    inv.Data, inv.Origin, inv.DX, inv.DY);
            return inv;
        }

        /// <summary>
        /// Compute the inverse of a square matrix using PLU factorization.
        /// The inverse matrix x is returned. Returns an invalid matrix if
        /// the factorization failed.
        /// </summary>
        public static Matrix<double> LuInverse(this Matrix<double> matrix)
        {
            var n = matrix.SX;
            if (n != matrix.SY) throw new ArgumentException("cannot invert non-square matrix");
            var lu = matrix.Copy();
            var p = new int[n];
            if (!lu.Data.LuFactorize(lu.Origin, lu.DX, lu.DY, p))
                return default(Matrix<double>);
            var inv = new Matrix<double>(matrix.Info);
            lu.Data.LuInverse(lu.Origin, lu.DX, lu.DY, p, inv.Data, inv.Origin, inv.DX, inv.DY);
            return inv;
        }

        /// <summary>
        /// Invert square matrix using PLU factorization in-place.
        /// If the factorization fails, the matrix is unchanged and
        /// false is returned.
        /// </summary>
        public static bool LuInvert(this Matrix<double> matrix)
        {
            var n = matrix.SX;
            if (n != matrix.SY) throw new ArgumentException("cannot invert non-square matrix");
            var lu = matrix.Copy();
            var p = new int[n];
            if (!lu.Data.LuFactorize(lu.Origin, lu.DX, lu.DY, p)) return false;
            lu.Data.LuInverse(lu.Origin, lu.DX, lu.DY, p, matrix.Data, matrix.Origin, matrix.DX, matrix.DY);
            return true;
        }

        #endregion

        #region Partially pivoting LU factorization of Matrix<ComplexD>

        /// <summary>
        /// Perform a partially pivoting LU factorization of the supplied
        /// matrix in-place, i.e A = inv(P) LU, so that any matrix equation
        /// A x = b can be solved using LU x = P b. The permutation matrix P
        /// is returned as a permutation vector, and the supplied matrix A is
        /// overwritten with its factorization LU, both in the parameter
        /// inputAndLuMatrix. If the supplied matrix is singular, null is
        /// returned.
        /// </summary>
        public static int[] LuFactorize(this Matrix<ComplexD> inputAndLuMatrix)
        {
            var n = inputAndLuMatrix.SX;
            if (n != inputAndLuMatrix.SY) throw new ArgumentException("cannot factorize non-square matrix");
            var p = new int[n];
            if (inputAndLuMatrix.Data.LuFactorize(inputAndLuMatrix.Origin,
                        inputAndLuMatrix.DX, inputAndLuMatrix.DY, p))
                return p;
            return null;
        }

        /// <summary>
        /// Solve a matrix equation of the form LU x = P b.
        /// LU is the supplied factorized matrix lu, P is a permutation matrix
        /// supplied as a permutation array p, and b is the right hand side
        /// supplied as a vector. The solution vector x is returned.
        /// </summary>
        public static Vector<ComplexD> LuSolve(this Matrix<ComplexD> luMatrix, int[] p, Vector<ComplexD> b)
        {
            var x = new Vector<ComplexD>(b.Info);
            luMatrix.Data.LuSolve(luMatrix.Origin, luMatrix.DX, luMatrix.DY, p,
                                  b.Data, b.Origin, b.Delta, x.Data, x.Origin, x.Delta);
            return x;
        }

        /// <summary>
        /// Solve a matrix equation of the form LU x = P b.
        /// LU is the supplied factorized matrix, P is a permutation matrix
        /// supplied as a permutation array, and b is the right hand side
        /// supplied as a matrix. The solution matrix x is returned.
        /// </summary>
        public static Matrix<ComplexD> LuSolve(this Matrix<ComplexD> luMatrix, int[] p, Matrix<ComplexD> b)
        {
            var x = new Matrix<ComplexD>(b.Info);
            luMatrix.Data.LuSolve(luMatrix.Origin, luMatrix.DX, luMatrix.DY, p,
                                  b.SX, b.Data, b.Origin, b.DX, b.DY, x.Data, x.Origin, x.DX, x.DY);
            return x;
        }

        /// <summary>
        /// Compute the inverse of an PLU factorized matrix, i.e LU x = P I.
        /// LU is the supplied factorized matrix, P is a permutation matrix
        /// supplied as a permutation array, and I is the square identity matrix.
        /// The inverse matrix x is returned.
        /// </summary>
        public static Matrix<ComplexD> LuInverse(this Matrix<ComplexD> luMatrix, int[] p)
        {
            var inv = new Matrix<ComplexD>(luMatrix.Info);
            luMatrix.Data.LuInverse(luMatrix.Origin, luMatrix.DX, luMatrix.DY, p,
                                    inv.Data, inv.Origin, inv.DX, inv.DY);
            return inv;
        }

        /// <summary>
        /// Compute the inverse of a square matrix using PLU factorization.
        /// The inverse matrix x is returned. Returns an invalid matrix if
        /// the factorization failed.
        /// </summary>
        public static Matrix<ComplexD> LuInverse(this Matrix<ComplexD> matrix)
        {
            var n = matrix.SX;
            if (n != matrix.SY) throw new ArgumentException("cannot invert non-square matrix");
            var lu = matrix.Copy();
            var p = new int[n];
            if (!lu.Data.LuFactorize(lu.Origin, lu.DX, lu.DY, p))
                return default(Matrix<ComplexD>);
            var inv = new Matrix<ComplexD>(matrix.Info);
            lu.Data.LuInverse(lu.Origin, lu.DX, lu.DY, p, inv.Data, inv.Origin, inv.DX, inv.DY);
            return inv;
        }

        /// <summary>
        /// Invert square matrix using PLU factorization in-place.
        /// If the factorization fails, the matrix is unchanged and
        /// false is returned.
        /// </summary>
        public static bool LuInvert(this Matrix<ComplexD> matrix)
        {
            var n = matrix.SX;
            if (n != matrix.SY) throw new ArgumentException("cannot invert non-square matrix");
            var lu = matrix.Copy();
            var p = new int[n];
            if (!lu.Data.LuFactorize(lu.Origin, lu.DX, lu.DY, p)) return false;
            lu.Data.LuInverse(lu.Origin, lu.DX, lu.DY, p, matrix.Data, matrix.Origin, matrix.DX, matrix.DY);
            return true;
        }

        #endregion

    }
}
