/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;

namespace Aardvark.Base;

public static partial class TensorNumericExtensions
{
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
        var x = new Vector<double>(qr.SX);
        qr.Data.QrSolve(qr.Origin, qr.DX, qr.DY, qr.SX, qr.SY, diag,
                        b.Data, b.Origin, b.Delta, x.Data, x.Origin, x.Delta, out _);
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
            qr.Data.QrSolve(qr.Origin, qr.DX, qr.DY, qr.SX, qr.SY, diag,
                            b.Data, b0, b.DY, x.Data, x0, x.DY, out _);
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

        var inv = new Matrix<double>(n, n);

        for (long i = 0, io = inv.Origin; i < n; i++, io += inv.DX)
        {
            for (int j = 0; j < n; j++) b[j] = (i == j ? 1.0 : 0.0);
            qr.Data.QrSolve(qr.Origin, qr.DX, qr.DY, qr.SX, qr.SY, diag,
                            b, 0, 1, inv.Data, io, inv.DY, out _);
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
