using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public static partial class NumericExtensions
    {
        #region double[,] array helpers

        /// <summary>
        /// L2 norm of complete matrix row.
        /// </summary>
        public static double RowNorm2(
                this double[,] array, long row)
        {
            return array.RowNorm2(row, 0, array.GetLongLength(1));
        }

        /// <summary>
        /// L2 norm of complete matrix column.
        /// </summary>
        public static double ColNorm2(
                this double[,] array, long col)
        {
            return array.ColNorm2(col, 0, array.GetLongLength(0));
        }

        /// <summary>
        /// L2 norm of matrix row elemnts including start and excluding end. 
        /// </summary>
        public static double RowNorm2(
                this double[,] array, long row, long start, long end)
        {
            double sum = 0.0;
            for (long i = start; i < end; i++) sum += array[row, i] * array[row, i];
            return Fun.Sqrt(sum);
        }

        /// <summary>
        /// L2 norm of matrix column elemnts including start and excluding end. 
        /// </summary>
        public static double ColNorm2(
                this double[,] array, long col, long start, long end)
        {
            double sum = 0.0;
            for (long i = start; i < end; i++) sum += array[i, col] * array[i, col];
            return Fun.Sqrt(sum);
        }

        /// <summary>
        /// Dot product of complete matrix rows.
        /// </summary>
        public static double RowDotRow(
                this double[,] array, long r0, long r1)
        {
            return array.RowDotRow(r0, r1, 0, array.GetLongLength(1));
        }

        /// <summary>
        /// Dot product of complete matrix columns.
        /// </summary>
        public static double ColDotCol(
                this double[,] array, long c0, long c1)
        {
            return array.ColDotCol(c0, c1, 0, array.GetLongLength(0));
        }

        /// <summary>
        /// Dot product of matrix row elements including start and excluding end.
        /// </summary>
        public static double RowDotRow(
                this double[,] array, long r0, long r1, long start, long end)
        {
            double sum = 0.0;
            for (long i = start; i < end; i++) sum += array[r0, i] * array[r1, i];
            return sum;
        }

        /// <summary>
        /// Dot product of matrix column elements including start and excluding end.
        /// </summary>
        public static double ColDotCol(
                this double[,] array, long c0, long c1, long start, long end)
        {
            double sum = 0.0;
            for (long i = start; i < end; i++) sum += array[i, c0] * array[i, c1];
            return sum;
        }

        #endregion

        #region QR factorization of double[,] arrays using Householder transformations

        // Note that this version of the QR factorization is mostly provided
        // for studying the algorithm. In practice the Matrix<double> version
        // is noticeably faster, since it performs incremental indexing.

        #region Helpers

        private static void FlipWithNormalizedColHouseholder(
                this double[] vector, double[,] A, int col, int end)
        {
            double p = 0.0;
            for (int i = col; i < end; i++) p += 2.0 * vector[i] * A[i, col];
            for (int i = col; i < end; i++) vector[i] -= p * A[i, col];
        }

        private static void FlipWithNormalizedRowHouseholder(
                this double[] vector, double[,] A, int row, int end)
        {
            double p = 0.0;
            for (int i = row; i < end; i++) p += 2.0 * vector[i] * A[row, i];
            for (int i = row; i < end; i++) vector[i] -= p * A[row, i];
        }

        #endregion

        private static void QrFactorize(this double[,] A, double[] diagonal)
        {
            int rows = A.GetLength(0);
            int cols = A.GetLength(1);

            if (rows < cols)
            {
                for (int r = 0; r < rows; r++)
                {
                    double alpha = -System.Math.Sign(A[r, r]) * A.RowNorm2(r, r, cols);
                    double norm_v = System.Math.Sqrt(-2.0 * alpha * (A[r, r] - alpha));
                    diagonal[r] = alpha;
                    A[r, r] = (A[r, r] - alpha) / norm_v;

                    for (int c = r + 1; c < cols; c++)
                        A[r, c] /= norm_v;

                    for (int j = r + 1; j < rows; j++)
                    {
                        double p_j = 2.0 * A.RowDotRow(r, j, r, cols);
                        for (int c = r; c < cols; c++) A[j, c] -= p_j * A[r, c];
                    }
                }
            }
            else
            {
                for (int c = 0; c < cols; c++)
                {
                    double alpha = -System.Math.Sign(A[c, c]) * A.ColNorm2(c, c, rows);
                    double norm_v = System.Math.Sqrt(-2.0 * alpha * (A[c, c] - alpha));
                    diagonal[c] = alpha;
                    A[c, c] = (A[c, c] - alpha) / norm_v;

                    for (int r = c + 1; r < rows; r++)
                        A[r, c] /= norm_v;

                    for (int j = c + 1; j < cols; j++)
                    {
                        double p_j = 2.0 * A.ColDotCol(c, j, c, rows);
                        for (int r = c; r < rows; r++) A[r, j] -= p_j * A[r, c];
                    }
                }
            }
        }

        /// <summary>
        /// Perform a QR factorization of the supplied m x n matrix using
        /// Householder transofmations, i.e. A = QR, where Q is orthogonal
        /// (a product of n-2 Householder-Transformations) and R is a right
        /// upper n x n triangular matrix. An array of the diagonal elements
        /// of R is returned. WARNING: the supplied matrix A is overwritten
        ///  with its factorization QR, both in the parameter aqr.
        /// </summary>
        public static double[] QrFactorize(this double[,] aqr)
        {
            double[] diag = new double[Fun.Min(aqr.GetLength(0), aqr.GetLength(1))];
            aqr.QrFactorize(diag);
            return diag;
        }

        /// <summary>
        /// Solves the Equation QR x = b. QR must be a factorized matrix (QrFactorize).
        /// diag provides the diagonal elements of R (QrFactorize)
        /// If QR is not quadratic the x where |QR*x - b| == min is calculated
        /// residual holds the minimal value of |QR*x - b|
        /// </summary>
        public static void QrSolve(
                this double[,] qr, double[] diag, double[] b, double[] x, out double residual)
        {
            int rows = qr.GetLength(0);
            int cols = qr.GetLength(1);

            if (rows < cols)
            {
                for (int r = 0; r < rows; r++)
                {
                    var value = b[r];
                    for (int j = 0; j < r; j++) value -= qr[r, j] * x[j];
                    x[r] = value / diag[r];
                }
                for (int i = rows; i < cols; i++) x[i] = 0.0;

                for (int c = rows - 1; c >= 0; c--)
                    x.FlipWithNormalizedRowHouseholder(qr, c, cols);

                residual = 0.0;
            }
            else
            {
                var c = b.Copy(rows);

                for (int i = 0; i < cols; i++)
                    c.FlipWithNormalizedColHouseholder(qr, i, rows);

                for (int r = cols - 1; r >= 0; r--)
                {
                    var value = c[r];
                    for (int j = r + 1; j < cols; j++) value -= qr[r, j] * x[j];
                    x[r] = value / diag[r];
                }

                residual = 0.0;
                if (cols < rows)
                {
                    for (int i = cols; i < b.Length; i++) residual += c[i] * c[i];
                    residual = Fun.Sqrt(residual);
                }
            }
        }

        /// <summary>
        /// Solves the Equation QR*x = b. QR must be a factorized matrix (QRFactorize).
        /// diag provides the diagonal elements of R (QRFactorize)
        /// If QR is not quadratic the x where |QR*x - b| == min is calculated
        /// </summary>
        public static double[] QrSolve(
                this double[,] qr, double[] diag, double[] b)
        {
            double unusedResidual;
            double[] x = new double[qr.GetLength(1)];
            QrSolve(qr, diag, b, x, out unusedResidual);
            return x;
        }

        /// <summary>
        /// Solves the Equation QR*x = b. QR must be a factorized matrix (QRFactorize).
        /// diag provides the diagonal elements of R (QRFactorize)
        /// If QR is not quadratic the x where |QR*x - b| == min is returned
        /// residual holds the minimal value of |QR*x - b|
        /// </summary>
        public static double[] QrSolve(
                this double[,] qr, double[] diag, double[] b, out double residual)
        {
            double[] x = new double[qr.GetLength(1)];
            QrSolve(qr, diag, b, x, out residual);
            return x;
        }

        public static void QrInverse(this double[,] QR, double[] diag, double[,] inv)
        {
            int m = QR.GetLength(0);
            int n = QR.GetLength(1);
            if (m != n) throw new ArgumentException("Can only invert quadratic matrices");

            double[] temp = new double[n];
            double[] b = new double[n];
            double err;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++) b[j] = (i == j ? 1.0 : 0.0);

                QR.QrSolve(diag, b, temp, out err);

                for (int j = 0; j < n; j++)
                    inv[j, i] = temp[j];
            }
        }

        /// <summary>
        /// Calculates the inverse Matrix to A using Householder-Transformations
        /// A has to be Quadratic
        /// </summary>
        public static double[,] QrInverse(this double[,] matrix)
        {
            double[,] qr = (double[,])matrix.Clone();
            double[] diag = qr.QrFactorize();
            double[,] inv = new double[matrix.GetLength(1), matrix.GetLength(0)];
            qr.QrInverse(diag, inv);
            return inv;
        }

        #endregion

        #region Matrix<double> helpers

        /// <summary>
        /// L2 norm of complete matrix row.
        /// </summary>
        public static double RowNorm2(
                this Matrix<double> matrix, long row)
        {
            return matrix.Data.RowNorm2(matrix.Origin, matrix.DX, matrix.DY, row, matrix.FX, matrix.EX);
        }

        /// <summary>
        /// L2 norm of complete matrix column.
        /// </summary>
        public static double ColNorm2(
                this Matrix<double> matrix, long col)
        {
            return matrix.Data.ColNorm2(matrix.Origin, matrix.DX, matrix.DY, col, matrix.FY, matrix.EY);
        }

        /// <summary>
        /// L2 norm of matrix row elemnts including start and excluding end. 
        /// </summary>
        public static double RowNorm2(
                this Matrix<double> matrix, long row, long start, long end)
        {
            return matrix.Data.RowNorm2(matrix.Origin, matrix.DX, matrix.DY, row, start, end);
        }

        /// <summary>
        /// L2 norm of matrix column elemnts including start and excluding end. 
        /// </summary>
        public static double ColNorm2(
                this Matrix<double> matrix, long col, long start, long end)
        {
            return matrix.Data.ColNorm2(matrix.Origin, matrix.DX, matrix.DY, col, start, end);
        }

        /// <summary>
        /// Dot product of complete matrix rows.
        /// </summary>
        public static double RowDotRow(
                this Matrix<double> matrix, long row0, long row1)
        {
            return matrix.Data.RowDotRow(matrix.Origin, matrix.DX, matrix.DY, row0, row1, matrix.FX, matrix.EX);
        }

        /// <summary>
        /// Dot product of complete matrix columns.
        /// </summary>
        public static double ColDotCol(
                this Matrix<double> matrix, long col0, long col1)
        {
            return matrix.Data.ColDotCol(matrix.Origin, matrix.DX, matrix.DY, col0, col1, matrix.FY, matrix.EY);
        }

        /// <summary>
        /// Dot product of matrix row elements including start and excluding end.
        /// </summary>
        public static double RowDotRow(
                this Matrix<double> matrix, long row0, long row1, long start, long end)
        {
            return matrix.Data.RowDotRow(matrix.Origin, matrix.DX, matrix.DY, row0, row1, start, end);
        }

        /// <summary>
        /// Dot product of matrix column elements including start and excluding end.
        /// </summary>
        public static double ColDotCol(
                this Matrix<double> matrix, long col0, long col1, long start, long end)
        {
            return matrix.Data.ColDotCol(matrix.Origin, matrix.DX, matrix.DY, col0, col1, start, end);
        }

        #endregion

        #region QR factorization of double[] with offset and strides using Householder transformations

        #region Helpers

        /// <summary>
        /// L2 norm of matrix row elemnts including start and excluding end. 
        /// </summary>
        private static double RowNorm2(
                this double[] matrix, long m0, long mx, long my, long row, long start, long end)
        {
            m0 += row * my;
            var norm = 0.0;
            for (long mi = m0 + start * mx, me = m0 + end * mx; mi != me; mi += mx)
                norm += matrix[mi] * matrix[mi];
            return Fun.Sqrt(norm);
        }

        /// <summary>
        /// L2 norm of matrix column elemnts including start and excluding end. 
        /// </summary>
        private static double ColNorm2(
                this double[] matrix, long m0, long mx, long my, long col, long start, long end)
        {
            m0 += col * mx;
            var norm = 0.0;
            for (long mi = m0 + start * my, me = m0 + end * my; mi != me; mi += my)
                norm += matrix[mi] * matrix[mi];
            return Fun.Sqrt(norm);
        }

        /// <summary>
        /// Dot product of matrix row elements including start and excluding end.
        /// </summary>
        private static double RowDotRow(
                this double[] matrix, long m0, long mx, long my, long row0, long row1, long start, long end)
        {
            m0 += start * mx;
            var dot = 0.0;
            for (long i0 = m0 + row0 * my, i1 = m0 + row1 * my, e0 = i0 + (end - start) * mx;
                 i0 != e0; i0 += mx, i1 += mx)
                dot += matrix[i0] * matrix[i1];
            return dot;
        }

        /// <summary>
        /// Dot product of matrix column elements including start and excluding end.
        /// </summary>
        private static double ColDotCol(
                this double[] matrix, long m0, long mx, long my, long col0, long col1, long start, long end)
        {
            m0 += start * my;
            var dot = 0.0;
            for (long i0 = m0 + col0 * mx, i1 = m0 + col1 * mx, e0 = i0 + (end - start) * my;
                 i0 != e0; i0 += my, i1 += my)
                dot += matrix[i0] * matrix[i1];
            return dot;
        }

        private static void FlipWithNormalizedColHouseholder(
                this double[] vector, double[] matrix, long m0, long mx, long my, long col, long end)
        {
            m0 += col * (mx + my);
            double p = 0.0;
            for (long i = col, mi = m0; i < end; i++, mi += my) p += 2.0 * vector[i] * matrix[mi];
            for (long i = col, mi = m0; i < end; i++, mi += my) vector[i] -= p * matrix[mi];
        }

        private static void FlipWithNormalizedRowHouseholder(
                this double[] vector, long v0, long vd, double[] matrix, long m0, long mx, long my, long row, long end)
        {
            m0 += row * (mx + my);
            double p = 0.0;
            for (long i = row, vi = v0, mi = m0; i < end; i++, vi += vd, mi += mx) p += 2.0 * vector[vi] * matrix[mi];
            for (long i = row, vi = v0, mi = m0; i < end; i++, vi += vd, mi += mx) vector[vi] -= p * matrix[mi];
        }

        #endregion

        /// <summary>
        /// Perform a QR factorization of the supplied m x n matrix using
        /// Householder transofmations, i.e. A = QR, where Q is orthogonal
        /// (a product of n-2 Householder-Transformations) and R is a right
        /// upper n x n triangular matrix. The diagonal elements of R are
        /// put in the supplied array diagonal. WARNING: the supplied
        /// matrix A is overwritten with its factorization QR, both in the
        /// parameter aqr.
        /// </summary>
        public static void QrFactorize(
                this double[] aqr, long a0, long ax, long ay, long cols, long rows, double[] diagonal)
        {
            if (rows < cols)
            {
                for (long r = 0, arr = a0; r < rows; r++, arr += ax + ay)
                {
                    var value = aqr[arr];
                    double alpha = -System.Math.Sign(value) * aqr.RowNorm2(a0, ax, ay, r, r, cols);
                    value -= alpha;
                    double norm_v = System.Math.Sqrt(-2.0 * alpha * value);
                    diagonal[r] = alpha;
                    aqr[arr] = value / norm_v;

                    for (long c = r + 1, arc = arr + ax; c < cols; c++, arc += ax)
                        aqr[arc] /= norm_v;
                    for (long j = r + 1, aj_ = arr + ay; j < rows; j++, aj_ += ay)
                    {
                        double p_j = 2.0 * aqr.RowDotRow(a0, ax, ay, r, j, r, cols);
                        for (long c = r, ajc = aj_, arc = arr; c < cols; c++, ajc += ax, arc += ax)
                            aqr[ajc] -= p_j * aqr[arc];
                    }
                }
            }
            else
            {
                for (long c = 0, acc = a0; c < cols; c++, acc += ax + ay)
                {
                    var value = aqr[acc];
                    double alpha = -System.Math.Sign(value) * aqr.ColNorm2(a0, ax, ay, c, c, rows);
                    value -= alpha;
                    double norm_v = System.Math.Sqrt(-2.0 * alpha * value);
                    diagonal[c] = alpha;
                    aqr[acc] = value / norm_v;

                    for (long r = c + 1, arc = acc + ay; r < rows; r++, arc += ay)
                        aqr[arc] /= norm_v;
                    for (long j = c + 1, a_j = acc + ax; j < cols; j++, a_j += ax)
                    {
                        double p_j = 2.0 * aqr.ColDotCol(a0, ax, ay, c, j, c, rows);
                        for (long r = c, arj = a_j, arc = acc; r < rows; r++, arj += ay, arc += ay)
                            aqr[arj] -= p_j * aqr[arc];
                    }
                }
            }
        }

        /// <summary>
        /// Solves the Equation QR x = b. QR must be a factorized matrix (QrFactorize).
        /// diag provides the diagonal elements of R (QrFactorize)
        /// If QR is not quadratic the x where |QR x - b| == min is calculated
        /// residual holds the minimal value of |QR x - b|
        /// </summary>
        public static void QrSolve(
                this double[] qr, long q0, long qx, long qy, long cols, long rows, double[] diag,
                double[] b, long b0, long bd, double[] x, long x0, long xd, out double residual)
        {
            if (rows < cols)
            {
                var xr = x0;
                for (long r = 0, br = b0, qr_ = q0; r < rows; r++, br += bd, qr_ += qy, xr += xd)
                {
                    var value = b[br];
                    for (long j = 0, qrj = qr_, xj = x0; j < r; j++, qrj += qx, xj += xd)
                        value -= qr[qrj] * x[xj];
                    x[xr] = value / diag[r];
                }
                for (long i = rows; i < cols; i++, xr += xd)
                    x[xr] = 0.0;
                for (long c = rows - 1; c >= 0; c--)
                    x.FlipWithNormalizedRowHouseholder(x0, xd, qr, q0, qx, qy, c, cols);
                residual = 0.0;
            }
            else
            {
                double[] c = new double[rows];
                for (long i = 0, bi = b0; i < rows; i++, bi += bd)
                    c[i] = b[bi];
                for (long i = 0; i < cols; i++)
                    c.FlipWithNormalizedColHouseholder(qr, q0, qx, qy, i, rows);
                for (long r = cols - 1, xr = x0 + r * xd, qr_ = q0 + r * qy;
                     r >= 0; r--, xr -= xd, qr_ -= qy)
                {
                    var value = c[r];
                    for (long j = r + 1, qrj = qr_ + j * qx, xj = x0 + j * xd;
                         j < cols; j++, qrj += qx, xj += xd)
                        value -= qr[qrj] * x[xj];
                    x[xr] = value / diag[r];
                }
                residual = 0.0;
                if (cols < rows)
                {
                    for (long i = cols; i < rows; i++) residual += c[i] * c[i];
                    residual = Fun.Sqrt(residual);
                }
            }
        }

        #endregion

        #region QR factorization of Matrix<double> using Householder transformations

        /// <summary>
        /// Perform a QR factorization of the supplied m x n matrix using
        /// Householder transofmations, i.e. A = QR, where Q is orthogonal
        /// (a product of n-2 Householder-Transformations) and R is a right
        /// upper n x n triangular matrix. An array of the diagonal elements
        /// of R is returned. WARNING: the supplied matrix A is overwritten
        ///  with its factorization QR, both in the parameter aqr.
        /// </summary>
        public static double[] QrFactorize(
                this Matrix<double> aqr)
        {
            double[] diag = new double[Fun.Min(aqr.SX, aqr.SY)];
            aqr.Data.QrFactorize(aqr.Origin, aqr.DX, aqr.DY, aqr.SX, aqr.SY, diag);
            return diag;
        }

        /// <summary>
        /// Solves the Equation QR x = b. QR must be a factorized matrix (QrFactorize).
        /// diag provides the diagonal elements of R (QrFactorize)
        /// If QR is not quadratic the x where |QR x - b| == min is returned.
        /// </summary>
        public static Vector<double> QrSolve(
                this Matrix<double> qr, double[] diag, Vector<double> b)
        {
            double unusedResidual;
            var x = new Vector<double>(qr.SX);
            qr.Data.QrSolve(qr.Origin, qr.DX, qr.DY, qr.SX, qr.SY, diag,
                            b.Data, b.Origin, b.Delta, x.Data, x.Origin, x.Delta, out unusedResidual);
            return x;
        }

        /// <summary>
        /// Solves the Equation QR x = b. QR must be a factorized matrix (QrFactorize).
        /// diag provides the diagonal elements of R (QrFactorize)
        /// If QR is not quadratic the x where |QR x - b| == min is returned
        /// residual holds the minimal value of |QR x - b|
        /// </summary>
        public static Vector<double> QrSolve(
                this Matrix<double> qr, double[] diag, Vector<double> b, out double residual)
        {
            var x = new Vector<double>(qr.SX);
            qr.Data.QrSolve(qr.Origin, qr.DX, qr.DY, qr.SX, qr.SY, diag,
                            b.Data, b.Origin, b.Delta, x.Data, x.Origin, x.Delta, out residual);
            return x;
        }

        /// <summary>
        /// Solves the Equation QR x = b, where x and b are matrices and each column of x
        /// is the solution of the eqation with the corresponding column of b.
        /// QR must be a factorized matrix (QrFactorize). diag provides the diagonal elements
        /// of R (QrFactorize) If QR is not quadratic the x where |QR x - b| == min is returned
        /// </summary>
        public static Matrix<double> QrSolve(
                this Matrix<double> qr, double[] diag, Matrix<double> b)
        {
            var x = new Matrix<double>(b.SX, qr.SX);
            for (long i = 0, b0 = b.Origin, x0 = x.Origin; i < b.SX; i++, b0 += b.DX, x0 += x.DX)
            {
                double unusedResidual;
                qr.Data.QrSolve(qr.Origin, qr.DX, qr.DY, qr.SX, qr.SY, diag,
                                b.Data, b0, b.DY, x.Data, x0, x.DY, out unusedResidual);
            }
            return x;
        }

        /// <summary>
        /// Solves the Equation QR x = b, where x and b are matrices and each column of x
        /// is the solution of the eqation with the corresponding column of b.
        /// QR must be a factorized matrix (QrFactorize). diag provides the diagonal elements
        /// of R (QrFactorize) If QR is not quadratic the x where |QR x - b| == min is returned
        /// The residual vector holds the minimal value of |QR x - b| for each column of x.
        /// </summary>
        public static Matrix<double> QrSolve(
                this Matrix<double> qr, double[] diag, Matrix<double> b, Vector<double> residual)
        {
            if (b.SX != residual.Size)
                throw new ArgumentException("b.SX != residual.Size");
            var x = new Matrix<double>(b.SX, qr.SX);
            for (long i = 0, b0 = b.Origin, x0 = x.Origin, ri = residual.Origin;
                 i < b.SX; i++, b0 += b.DX, x0 += x.DX, ri += residual.Delta)
            {
                qr.Data.QrSolve(qr.Origin, qr.DX, qr.DY, qr.SX, qr.SY, diag,
                                b.Data, b0, b.DY, x.Data, x0, x.DY, out residual.Data[ri]);
            }
            return x;
        }

        /// <summary>
        /// Calculates the inverse of a matrix given its QR factorization using
        /// Householder transformations. The matrix has to be quadratic.
        /// </summary>
        public static Matrix<double> QrInverse(
                this Matrix<double> qr, double[] diag)
        {
            long n = qr.SX;
            if (n != qr.SY) throw new ArgumentException("Can only invert quadratic matrices");

            double[] b = new double[n];

            Matrix<double> inv = new Matrix<double>(n, n);

            for (long i = 0, io = inv.Origin; i < n; i++, io += inv.DX)
            {
                double unusedResidual;
                for (int j = 0; j < n; j++) b[j] = (i == j ? 1.0 : 0.0);
                qr.Data.QrSolve(qr.Origin, qr.DX, qr.DY, qr.SX, qr.SY, diag,
                                b, 0, 1, inv.Data, io, inv.DY, out unusedResidual);
            }
            return inv;
        }

        /// <summary>
        /// Calculates the inverse matrix using Householder transformations.
        /// The matrix has to be quadratic.
        /// </summary>
        public static Matrix<double> QrInverse(
                this Matrix<double> matrix)
        {
            var qr = matrix.Copy();
            double[] diag = qr.QrFactorize();
            return qr.QrInverse(diag);
        }

        /// <summary>
        /// Calculates the inverse matrix using Householder transformations
        /// </summary>
        public static M22d QrInverse(this M22d mat)
        {
            var qr = new Matrix<double>((double[])mat, 2, 2);
            var diag = qr.QrFactorize();
            return (M22d)(qr.QrInverse(diag).Data);
        }

        /// <summary>
        /// Calculates the inverse matrix using Householder transformations
        /// </summary>
        public static M33d QrInverse(this M33d mat)
        {
            var qr = new Matrix<double>((double[])mat, 3, 3);
            var diag = qr.QrFactorize();
            return (M33d)(qr.QrInverse(diag).Data);
        }

        /// <summary>
        /// Calculates the inverse matrix using Householder transformations
        /// </summary>
        public static M44d QrInverse(this M44d mat)
        {
            var qr = new Matrix<double>((double[])mat, 4, 4);
            var diag = qr.QrFactorize();
            return (M44d)(qr.QrInverse(diag).Data);
        }

        #endregion

    }
}
