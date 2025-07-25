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

#pragma warning disable IDE0079 // Remove unnecessary suppression
#pragma warning disable CA2208 // Instantiate argument exceptions correctly
#pragma warning restore IDE0079 // Remove unnecessary suppression

namespace Aardvark.Base;

public static partial class NumericExtensions
{
    #region Partially pivoting LU factorization of float[,] arrays

    // Note that this version of the LU factorization is mostly provided
    // for studying the algorithm. In practice the Matrix<float> version
    // is noticeably faster, since it performs incremental indexing.

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix, i.e A = inv(P) LU, so that any matrix equation A x = b
    /// can be solved using LU x = P b. The permutation matrix P is
    /// returns as permutation vector. WARNING: the supplied matrix A is
    /// overwritten with its factorization LU, both in the parameter alu.
    /// If the matrix is singular an ArgumentException is thrown.
    /// </summary>
    public static int[] LuFactorize(this float[,] alu)
    {
        int n = Fun.Min(alu.GetLength(0), alu.GetLength(1));
        int[] perm = new int[n];
        if (alu.LuFactorize(perm)) return perm;
        throw new ArgumentException("singular matrix");
    }

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix, i.e A = inv(P) LU, so that any matrix equation A x = b
    /// can be solved using LU x = P b. The permutation matrix P is
    /// filled into the supplied permutation vector p, and the supplied
    /// matrix A is overwritten with its factorization LU, both in the
    /// parameter alu. The function returns true if the factorization
    /// is successful, otherwise the matrix is singular and false is
    /// returned.
    /// </summary>
    public static bool LuFactorize(this float[,] alu, int[] p)
    {
        int n = p.Length;
        p.SetByIndex(i => i);
        for (int k = 0; k < n - 1; k++)
        {
            int pi = k;
            float pivot = alu[pi, k], absPivot = Fun.Abs(pivot);
            for (int i = k + 1; i < n; i++)
            {
                float value = alu[i, k], absValue = Fun.Abs(value);
                if (absValue > absPivot)
                {
                    pivot = value; absPivot = absValue; pi = i;
                }
            }
            if (Fun.IsTiny(pivot)) return false;
            if (pi != k)
            {
                Fun.Swap(ref p[pi], ref p[k]);
                for (int i = 0; i < n; i++)
                    Fun.Swap(ref alu[pi, i], ref alu[k, i]);
            }
            for (int j = k + 1; j < n; j++)
            {
                float factor = alu[j, k] / pivot;
                alu[j, k] = factor; // construct L
                for (int i = k + 1; i < n; i++)
                    alu[j, i] -= alu[k, i] * factor;
            }
        }
        return true;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector. The returned x satisfies the equation.
    /// </summary>
    public static float[] LuSolve(this float[,] lu, int[] p, float[] b)
    {
        int n = Fun.Min(lu.GetLength(0), lu.GetLength(1));
        if (n != p.GetLength(0)) throw new ArgumentException();
        if (n > b.GetLength(0)) throw new ArgumentException();
        var x = new float[n];
        lu.LuSolve(p, b, x);
        return x;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector. The result vector that satisfies the
    /// equation is filled into the supplied parameter x.
    /// </summary>
    public static void LuSolve(this float[,] lu, int[] p, float[] b, float[] x)
    {
        int n = p.Length;
        for (int i = 0; i < n; i++) x[i] = b[p[i]];

        for (int j = 1; j < n; j++)
        {
            float scalar = x[j];
            for (int i = 0; i < j; i++) scalar -= lu[j, i] * x[i];
            x[j] = scalar;
        }

        for (int j = n - 1; j >= 0; j--)
        {
            float scalar = x[j];
            for (int i = j + 1; i < n; i++) scalar -= lu[j, i] * x[i];
            x[j] = scalar / lu[j, j];
        }
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix, P is a permutation matrix
    /// supplied as a permutation array, and b is a supplied matrix.
    /// The returned matrix x satisfies the equation.
    /// </summary>
    public static float[,] LuSolve(this float[,] lu, int[] p, float[,] b)
    {
        int n = Fun.Min(lu.GetLength(0), lu.GetLength(1));
        if (n != p.GetLength(0)) throw new ArgumentException();
        if (n > b.GetLength(0)) throw new ArgumentException();
        int m = b.GetLength(1);
        float[,] x = new float[n, m];
        lu.LuSolve(p, b, x);
        return x;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix, P is a permutation matrix
    /// supplied as a permutation array, and b is a supplied matrix.
    /// The result matrix that satisfies the equation is filled into the
    /// supplied parameter x.
    /// </summary>
    public static void LuSolve(this float[,] lu, int[] p, float[,] b, float[,] x)
    {
        int n = p.Length;
        int m = b.GetLength(1);
        for (int j = 0; j < n; j++)
            for (int i = 0; i < m; i++)
                x[j, i] = b[p[j], i];

        for (int k = 1; k < n; k++)
            for (int j = 0; j < k; j++)
                for (int i = 0; i < m; i++)
                    x[k, i] -= lu[k, j] * x[j, i];

        for (int k = n - 1; k >= 0; k--)
        {
            for (int j = k + 1; j < n; j++)
                for (int i = 0; i < m; i++)
                    x[k, i] -= lu[k, j] * x[j, i];
            var factor = lu[k, k]; // storing the inverse would increase the mean error
            for (int i = 0; i < m; i++)
                x[k, i] /= factor;
        }
    }

    /// <summary>
    /// Return the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p.
    /// </summary>
    public static float[,] LuInverse(this float[,] lu, int[] p)
    {
        int n = Fun.Min(lu.GetLength(0), lu.GetLength(1));
        if (n != p.GetLength(0)) throw new ArgumentException();
        float[,] inv = new float[n, n];
        lu.LuInverse(p, inv);
        return inv;
    }

    /// <summary>
    /// Compute the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p. The result matrix is filled into the
    /// supplied matrix.
    /// </summary>
    public static void LuInverse(this float[,] lu, int[] p, float[,] inv)
    {
        int n = p.Length;
        for (int j = 0; j < n; j++)
            for (int i = 0; i < n; i++)
                inv[j, i] = p[j] == i ? 1 : 0;

        for (int k = 1; k < n; k++)
            for (int j = 0; j < k; j++)
                for (int i = 0; i < n; i++)
                    inv[k, i] -= lu[k, j] * inv[j, i];

        for (int k = n - 1; k >= 0; k--)
        {
            for (int j = k + 1; j < n; j++)
                for (int i = 0; i < n; i++)
                    inv[k, i] -= lu[k, j] * inv[j, i];
            var factor = lu[k, k]; // storing the inverse would increase the mean error
            for (int i = 0; i < n; i++)
                inv[k, i] /= factor;
        }
    }

    #endregion

    #region Partially pivoting LU factorization of float[] arrays with offset and strides

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix in-place, i.e A = inv(P) LU, so that any matrix equation
    /// A x = b can be solved using LU x = P b. The permutation matrix P
    /// is filled into the supplied permutation vector p, and the supplied
    /// matrix A is overwritten with its factorization LU, both in the
    /// parameter alu. The function returns true if the factorization
    /// is successful, otherwise the matrix is singular and false is
    /// returned. No size checks are performed.
    /// </summary>
    public static bool LuFactorize(
            this float[] alu, long a0, long ax, long ay, int[] p)
    {
        long n = p.LongLength;
        p.SetByIndex(i => i);
        for (long k = 0, ak = a0, a_k = 0; k < n - 1; k++, ak += ay, a_k += ax)
        {
            long pi = k;
            float pivot = alu[ak + a_k], absPivot = Fun.Abs(pivot);
            for (long i = k + 1, aik = ak + ay + a_k; i < n; i++, aik += ay)
            {
                float value = alu[aik], absValue = Fun.Abs(value);
                if (absValue > absPivot)
                {
                    pivot = value; absPivot = absValue; pi = i;
                }
            }
            if (Fun.IsTiny(pivot)) return false;
            if (pi != k)
            {
                long api = a0 + pi * ay;
                Fun.Swap(ref p[pi], ref p[k]);
                for (long i = 0, apii = api, aki = ak; i < n; i++, apii += ax, aki += ax)
                    Fun.Swap(ref alu[apii], ref alu[aki]);
            }
            for (long j = k + 1, aj = ak + ay, ajk = aj + a_k; j < n; j++, aj += ay, ajk += ay)
            {
                float factor = alu[ajk] / pivot; alu[ajk] = factor;
                for (long i = k + 1, aji = ajk + ax, aki = ak + a_k + ax; i < n; i++, aji += ax, aki += ax)
                    alu[aji] -= alu[aki] * factor;
            }
        }
        return true;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector starting at b0 with element to element
    /// delta bD. The result vector that satisfies the equation is
    /// filled into the supplied parameter x beginning at index x0
    /// with element to element delta xD.
    /// </summary>
    public static void LuSolve(
            this float[] lu, long l0, long lx, long ly, int[] p,
            float[] b, long b0, long bd, float[] x, long x0, long xd)
    {
        long n = p.LongLength;
        for (long i = 0, xi = x0; i < n; i++, xi += xd)
            x[xi] = b[b0 + p[i] * bd];

        for (long j = 1, xj = x0 + xd, lj = l0 + ly; j < n; j++, xj += xd, lj += ly)
        {
            float scalar = x[xj];
            for (long i = 0, lji = lj, xi = x0; i < j; i++, lji += lx, xi += xd)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar;
        }

        for (long j = n - 1, xj = x0 + j * xd, lj = l0 + j * ly, ljj = lj + j * lx;
             j >= 0; j--, xj -= xd, lj -= ly, ljj -= ly + lx)
        {
            float scalar = x[xj];
            for (long i = j + 1, xi = xj + xd, lji = ljj + lx; i < n; i++, xi += xd, lji += lx)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar / lu[ljj];
        }
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix, P is a permutation matrix
    /// supplied as a permutation array, and b is a supplied matrix.
    /// The result matrix that satisfies the equation is filled into the
    /// supplied parameter x.
    /// </summary>
    public static void LuSolve(
            this float[] lu, long l0, long lx, long ly, int[] p,
            long m, float[] b, long b0, long bx, long by,
            float[] x, long x0, long xx, long xy)
    {
        int n = p.Length;
        for (long j = 0, xj = x0; j < n; j++, xj += xy)
            for (long i = 0, bpji = b0 + p[j] * by, xji = xj; i < m; i++, bpji += bx, xji += xx)
                x[xji] = b[bpji];

        for (long k = 1, xk = x0 + xy, lk = l0 + ly; k < n; k++, xk += xy, lk += ly)
            for (long j = 0, xj = x0, lkj = lk; j < k; j++, xj += xy, lkj += lx)
                for (long i = 0, xki = xk, xji = xj; i < m; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];

        for (long k = n - 1, xk = x0 + k * xy, lk = l0 + k * ly, lkk = lk + k * lx;
             k >= 0; k--, xk -= xy, lk -= ly, lkk -= ly + lx)
        {
            for (long j = k + 1, lkj = lkk + lx, xj = xk + xy; j < n; j++, lkj += lx, xj += xy)
                for (long i = 0, xki = xk, xji = xj; i < m; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];
            var factor = lu[lkk]; // storing the inverse would increase the mean error
            for (long i = 0, xki = xk; i < m; i++, xki += xx)
                x[xki] /= factor;
        }
    }

    /// <summary>
    /// Compute the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p. The inverse matrix is filled into the
    /// supplied matrix x.
    /// </summary>
    public static void LuInverse(
            this float[] lu, long l0, long lx, long ly, int[] p,
            float[] x, long x0, long xx, long xy)
    {
        int n = p.Length;
        for (long j = 0, xj = x0; j < n; j++, xj += xy)
            for (long i = 0, xji = xj; i < n; i++, xji += xx)
                x[xji] = p[j] == i ? 1 : 0;

        for (long k = 1, xk = x0 + xy, lk = l0 + ly; k < n; k++, xk += xy, lk += ly)
            for (long j = 0, xj = x0, lkj = lk; j < k; j++, xj += xy, lkj += lx)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];

        for (long k = n - 1, xk = x0 + k * xy, lk = l0 + k * ly, lkk = lk + k * lx;
             k >= 0; k--, xk -= xy, lk -= ly, lkk -= ly + lx)
        {
            for (long j = k + 1, lkj = lkk + lx, xj = xk + xy; j < n; j++, lkj += lx, xj += xy)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];
            var factor = lu[lkk]; // storing the inverse would increase the mean error
            for (long i = 0, xki = xk; i < n; i++, xki += xx)
                x[xki] /= factor;
        }
    }

    #endregion


    #region Partially pivoting LU factorization of float* pointers with offset and strides

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix in-place, i.e A = inv(P) LU, so that any matrix equation
    /// A x = b can be solved using LU x = P b. The permutation matrix P
    /// is filled into the supplied permutation vector p, and the supplied
    /// matrix A is overwritten with its factorization LU, both in the
    /// parameter alu. The function returns true if the factorization
    /// is successful, otherwise the matrix is singular and false is
    /// returned. No size checks are performed.
    /// </summary>
    public static unsafe bool LuFactorize(
            float* alu, long a0, long ax, long ay, int* p, long n)
    {
        for (long i = 0; i < n; i++) p[i] = (int)i;

        for (long k = 0, ak = a0, a_k = 0; k < n - 1; k++, ak += ay, a_k += ax)
        {
            long pi = k;
            float pivot = alu[ak + a_k], absPivot = Fun.Abs(pivot);
            for (long i = k + 1, aik = ak + ay + a_k; i < n; i++, aik += ay)
            {
                float value = alu[aik], absValue = Fun.Abs(value);
                if (absValue > absPivot)
                {
                    pivot = value; absPivot = absValue; pi = i;
                }
            }
            if (Fun.IsTiny(pivot)) return false;
            if (pi != k)
            {
                long api = a0 + pi * ay;
                Fun.Swap(ref p[pi], ref p[k]);
                for (long i = 0, apii = api, aki = ak; i < n; i++, apii += ax, aki += ax)
                    Fun.Swap(ref alu[apii], ref alu[aki]);
            }
            for (long j = k + 1, aj = ak + ay, ajk = aj + a_k; j < n; j++, aj += ay, ajk += ay)
            {
                float factor = alu[ajk] / pivot; alu[ajk] = factor;
                for (long i = k + 1, aji = ajk + ax, aki = ak + a_k + ax; i < n; i++, aji += ax, aki += ax)
                    alu[aji] -= alu[aki] * factor;
            }
        }
        return true;
    }


    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector starting at b0 with element to element
    /// delta bD. The result vector that satisfies the equation is
    /// filled into the supplied parameter x beginning at index x0
    /// with element to element delta xD.
    /// </summary>
    public static unsafe void LuSolve(
            float* lu, long l0, long lx, long ly, int* p,
            float* b, long b0, long bd, float* x, long x0, long xd, long n)
    {
        for (long i = 0, xi = x0; i < n; i++, xi += xd)
            x[xi] = b[b0 + p[i] * bd];

        for (long j = 1, xj = x0 + xd, lj = l0 + ly; j < n; j++, xj += xd, lj += ly)
        {
            float scalar = x[xj];
            for (long i = 0, lji = lj, xi = x0; i < j; i++, lji += lx, xi += xd)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar;
        }

        for (long j = n - 1, xj = x0 + j * xd, lj = l0 + j * ly, ljj = lj + j * lx;
             j >= 0; j--, xj -= xd, lj -= ly, ljj -= ly + lx)
        {
            float scalar = x[xj];
            for (long i = j + 1, xi = xj + xd, lji = ljj + lx; i < n; i++, xi += xd, lji += lx)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar / lu[ljj];
        }
    }


    /// <summary>
    /// Compute the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p. The inverse matrix is filled into the
    /// supplied matrix x.
    /// </summary>
    public static unsafe void LuInverse(
            float* lu, long l0, long lx, long ly, int* p,
            float* x, long x0, long xx, long xy, long n)
    {
        for (long j = 0, xj = x0; j < n; j++, xj += xy)
            for (long i = 0, xji = xj; i < n; i++, xji += xx)
                x[xji] = p[j] == i ? 1 : 0;

        for (long k = 1, xk = x0 + xy, lk = l0 + ly; k < n; k++, xk += xy, lk += ly)
            for (long j = 0, xj = x0, lkj = lk; j < k; j++, xj += xy, lkj += lx)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];

        for (long k = n - 1, xk = x0 + k * xy, lk = l0 + k * ly, lkk = lk + k * lx;
             k >= 0; k--, xk -= xy, lk -= ly, lkk -= ly + lx)
        {
            for (long j = k + 1, lkj = lkk + lx, xj = xk + xy; j < n; j++, lkj += lx, xj += xy)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];
            var factor = lu[lkk]; // storing the inverse would increase the mean error
            for (long i = 0, xki = xk; i < n; i++, xki += xx)
                x[xki] /= factor;
        }
    }


    #endregion


    #region Partially pivoting LU factorization of double[,] arrays

    // Note that this version of the LU factorization is mostly provided
    // for studying the algorithm. In practice the Matrix<double> version
    // is noticeably faster, since it performs incremental indexing.

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix, i.e A = inv(P) LU, so that any matrix equation A x = b
    /// can be solved using LU x = P b. The permutation matrix P is
    /// returns as permutation vector. WARNING: the supplied matrix A is
    /// overwritten with its factorization LU, both in the parameter alu.
    /// If the matrix is singular an ArgumentException is thrown.
    /// </summary>
    public static int[] LuFactorize(this double[,] alu)
    {
        int n = Fun.Min(alu.GetLength(0), alu.GetLength(1));
        int[] perm = new int[n];
        if (alu.LuFactorize(perm)) return perm;
        throw new ArgumentException("singular matrix");
    }

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix, i.e A = inv(P) LU, so that any matrix equation A x = b
    /// can be solved using LU x = P b. The permutation matrix P is
    /// filled into the supplied permutation vector p, and the supplied
    /// matrix A is overwritten with its factorization LU, both in the
    /// parameter alu. The function returns true if the factorization
    /// is successful, otherwise the matrix is singular and false is
    /// returned.
    /// </summary>
    public static bool LuFactorize(this double[,] alu, int[] p)
    {
        int n = p.Length;
        p.SetByIndex(i => i);
        for (int k = 0; k < n - 1; k++)
        {
            int pi = k;
            double pivot = alu[pi, k], absPivot = Fun.Abs(pivot);
            for (int i = k + 1; i < n; i++)
            {
                double value = alu[i, k], absValue = Fun.Abs(value);
                if (absValue > absPivot)
                {
                    pivot = value; absPivot = absValue; pi = i;
                }
            }
            if (Fun.IsTiny(pivot)) return false;
            if (pi != k)
            {
                Fun.Swap(ref p[pi], ref p[k]);
                for (int i = 0; i < n; i++)
                    Fun.Swap(ref alu[pi, i], ref alu[k, i]);
            }
            for (int j = k + 1; j < n; j++)
            {
                double factor = alu[j, k] / pivot;
                alu[j, k] = factor; // construct L
                for (int i = k + 1; i < n; i++)
                    alu[j, i] -= alu[k, i] * factor;
            }
        }
        return true;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector. The returned x satisfies the equation.
    /// </summary>
    public static double[] LuSolve(this double[,] lu, int[] p, double[] b)
    {
        int n = Fun.Min(lu.GetLength(0), lu.GetLength(1));
        if (n != p.GetLength(0)) throw new ArgumentException();
        if (n > b.GetLength(0)) throw new ArgumentException();
        var x = new double[n];
        lu.LuSolve(p, b, x);
        return x;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector. The result vector that satisfies the
    /// equation is filled into the supplied parameter x.
    /// </summary>
    public static void LuSolve(this double[,] lu, int[] p, double[] b, double[] x)
    {
        int n = p.Length;
        for (int i = 0; i < n; i++) x[i] = b[p[i]];

        for (int j = 1; j < n; j++)
        {
            double scalar = x[j];
            for (int i = 0; i < j; i++) scalar -= lu[j, i] * x[i];
            x[j] = scalar;
        }

        for (int j = n - 1; j >= 0; j--)
        {
            double scalar = x[j];
            for (int i = j + 1; i < n; i++) scalar -= lu[j, i] * x[i];
            x[j] = scalar / lu[j, j];
        }
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix, P is a permutation matrix
    /// supplied as a permutation array, and b is a supplied matrix.
    /// The returned matrix x satisfies the equation.
    /// </summary>
    public static double[,] LuSolve(this double[,] lu, int[] p, double[,] b)
    {
        int n = Fun.Min(lu.GetLength(0), lu.GetLength(1));
        if (n != p.GetLength(0)) throw new ArgumentException();
        if (n > b.GetLength(0)) throw new ArgumentException();
        int m = b.GetLength(1);
        double[,] x = new double[n, m];
        lu.LuSolve(p, b, x);
        return x;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix, P is a permutation matrix
    /// supplied as a permutation array, and b is a supplied matrix.
    /// The result matrix that satisfies the equation is filled into the
    /// supplied parameter x.
    /// </summary>
    public static void LuSolve(this double[,] lu, int[] p, double[,] b, double[,] x)
    {
        int n = p.Length;
        int m = b.GetLength(1);
        for (int j = 0; j < n; j++)
            for (int i = 0; i < m; i++)
                x[j, i] = b[p[j], i];

        for (int k = 1; k < n; k++)
            for (int j = 0; j < k; j++)
                for (int i = 0; i < m; i++)
                    x[k, i] -= lu[k, j] * x[j, i];

        for (int k = n - 1; k >= 0; k--)
        {
            for (int j = k + 1; j < n; j++)
                for (int i = 0; i < m; i++)
                    x[k, i] -= lu[k, j] * x[j, i];
            var factor = lu[k, k]; // storing the inverse would increase the mean error
            for (int i = 0; i < m; i++)
                x[k, i] /= factor;
        }
    }

    /// <summary>
    /// Return the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p.
    /// </summary>
    public static double[,] LuInverse(this double[,] lu, int[] p)
    {
        int n = Fun.Min(lu.GetLength(0), lu.GetLength(1));
        if (n != p.GetLength(0)) throw new ArgumentException();
        double[,] inv = new double[n, n];
        lu.LuInverse(p, inv);
        return inv;
    }

    /// <summary>
    /// Compute the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p. The result matrix is filled into the
    /// supplied matrix.
    /// </summary>
    public static void LuInverse(this double[,] lu, int[] p, double[,] inv)
    {
        int n = p.Length;
        for (int j = 0; j < n; j++)
            for (int i = 0; i < n; i++)
                inv[j, i] = p[j] == i ? 1 : 0;

        for (int k = 1; k < n; k++)
            for (int j = 0; j < k; j++)
                for (int i = 0; i < n; i++)
                    inv[k, i] -= lu[k, j] * inv[j, i];

        for (int k = n - 1; k >= 0; k--)
        {
            for (int j = k + 1; j < n; j++)
                for (int i = 0; i < n; i++)
                    inv[k, i] -= lu[k, j] * inv[j, i];
            var factor = lu[k, k]; // storing the inverse would increase the mean error
            for (int i = 0; i < n; i++)
                inv[k, i] /= factor;
        }
    }

    #endregion

    #region Partially pivoting LU factorization of double[] arrays with offset and strides

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix in-place, i.e A = inv(P) LU, so that any matrix equation
    /// A x = b can be solved using LU x = P b. The permutation matrix P
    /// is filled into the supplied permutation vector p, and the supplied
    /// matrix A is overwritten with its factorization LU, both in the
    /// parameter alu. The function returns true if the factorization
    /// is successful, otherwise the matrix is singular and false is
    /// returned. No size checks are performed.
    /// </summary>
    public static bool LuFactorize(
            this double[] alu, long a0, long ax, long ay, int[] p)
    {
        long n = p.LongLength;
        p.SetByIndex(i => i);
        for (long k = 0, ak = a0, a_k = 0; k < n - 1; k++, ak += ay, a_k += ax)
        {
            long pi = k;
            double pivot = alu[ak + a_k], absPivot = Fun.Abs(pivot);
            for (long i = k + 1, aik = ak + ay + a_k; i < n; i++, aik += ay)
            {
                double value = alu[aik], absValue = Fun.Abs(value);
                if (absValue > absPivot)
                {
                    pivot = value; absPivot = absValue; pi = i;
                }
            }
            if (Fun.IsTiny(pivot)) return false;
            if (pi != k)
            {
                long api = a0 + pi * ay;
                Fun.Swap(ref p[pi], ref p[k]);
                for (long i = 0, apii = api, aki = ak; i < n; i++, apii += ax, aki += ax)
                    Fun.Swap(ref alu[apii], ref alu[aki]);
            }
            for (long j = k + 1, aj = ak + ay, ajk = aj + a_k; j < n; j++, aj += ay, ajk += ay)
            {
                double factor = alu[ajk] / pivot; alu[ajk] = factor;
                for (long i = k + 1, aji = ajk + ax, aki = ak + a_k + ax; i < n; i++, aji += ax, aki += ax)
                    alu[aji] -= alu[aki] * factor;
            }
        }
        return true;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector starting at b0 with element to element
    /// delta bD. The result vector that satisfies the equation is
    /// filled into the supplied parameter x beginning at index x0
    /// with element to element delta xD.
    /// </summary>
    public static void LuSolve(
            this double[] lu, long l0, long lx, long ly, int[] p,
            double[] b, long b0, long bd, double[] x, long x0, long xd)
    {
        long n = p.LongLength;
        for (long i = 0, xi = x0; i < n; i++, xi += xd)
            x[xi] = b[b0 + p[i] * bd];

        for (long j = 1, xj = x0 + xd, lj = l0 + ly; j < n; j++, xj += xd, lj += ly)
        {
            double scalar = x[xj];
            for (long i = 0, lji = lj, xi = x0; i < j; i++, lji += lx, xi += xd)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar;
        }

        for (long j = n - 1, xj = x0 + j * xd, lj = l0 + j * ly, ljj = lj + j * lx;
             j >= 0; j--, xj -= xd, lj -= ly, ljj -= ly + lx)
        {
            double scalar = x[xj];
            for (long i = j + 1, xi = xj + xd, lji = ljj + lx; i < n; i++, xi += xd, lji += lx)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar / lu[ljj];
        }
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix, P is a permutation matrix
    /// supplied as a permutation array, and b is a supplied matrix.
    /// The result matrix that satisfies the equation is filled into the
    /// supplied parameter x.
    /// </summary>
    public static void LuSolve(
            this double[] lu, long l0, long lx, long ly, int[] p,
            long m, double[] b, long b0, long bx, long by,
            double[] x, long x0, long xx, long xy)
    {
        int n = p.Length;
        for (long j = 0, xj = x0; j < n; j++, xj += xy)
            for (long i = 0, bpji = b0 + p[j] * by, xji = xj; i < m; i++, bpji += bx, xji += xx)
                x[xji] = b[bpji];

        for (long k = 1, xk = x0 + xy, lk = l0 + ly; k < n; k++, xk += xy, lk += ly)
            for (long j = 0, xj = x0, lkj = lk; j < k; j++, xj += xy, lkj += lx)
                for (long i = 0, xki = xk, xji = xj; i < m; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];

        for (long k = n - 1, xk = x0 + k * xy, lk = l0 + k * ly, lkk = lk + k * lx;
             k >= 0; k--, xk -= xy, lk -= ly, lkk -= ly + lx)
        {
            for (long j = k + 1, lkj = lkk + lx, xj = xk + xy; j < n; j++, lkj += lx, xj += xy)
                for (long i = 0, xki = xk, xji = xj; i < m; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];
            var factor = lu[lkk]; // storing the inverse would increase the mean error
            for (long i = 0, xki = xk; i < m; i++, xki += xx)
                x[xki] /= factor;
        }
    }

    /// <summary>
    /// Compute the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p. The inverse matrix is filled into the
    /// supplied matrix x.
    /// </summary>
    public static void LuInverse(
            this double[] lu, long l0, long lx, long ly, int[] p,
            double[] x, long x0, long xx, long xy)
    {
        int n = p.Length;
        for (long j = 0, xj = x0; j < n; j++, xj += xy)
            for (long i = 0, xji = xj; i < n; i++, xji += xx)
                x[xji] = p[j] == i ? 1 : 0;

        for (long k = 1, xk = x0 + xy, lk = l0 + ly; k < n; k++, xk += xy, lk += ly)
            for (long j = 0, xj = x0, lkj = lk; j < k; j++, xj += xy, lkj += lx)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];

        for (long k = n - 1, xk = x0 + k * xy, lk = l0 + k * ly, lkk = lk + k * lx;
             k >= 0; k--, xk -= xy, lk -= ly, lkk -= ly + lx)
        {
            for (long j = k + 1, lkj = lkk + lx, xj = xk + xy; j < n; j++, lkj += lx, xj += xy)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];
            var factor = lu[lkk]; // storing the inverse would increase the mean error
            for (long i = 0, xki = xk; i < n; i++, xki += xx)
                x[xki] /= factor;
        }
    }

    #endregion


    #region Partially pivoting LU factorization of double* pointers with offset and strides

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix in-place, i.e A = inv(P) LU, so that any matrix equation
    /// A x = b can be solved using LU x = P b. The permutation matrix P
    /// is filled into the supplied permutation vector p, and the supplied
    /// matrix A is overwritten with its factorization LU, both in the
    /// parameter alu. The function returns true if the factorization
    /// is successful, otherwise the matrix is singular and false is
    /// returned. No size checks are performed.
    /// </summary>
    public static unsafe bool LuFactorize(
            double* alu, long a0, long ax, long ay, int* p, long n)
    {
        for (long i = 0; i < n; i++) p[i] = (int)i;

        for (long k = 0, ak = a0, a_k = 0; k < n - 1; k++, ak += ay, a_k += ax)
        {
            long pi = k;
            double pivot = alu[ak + a_k], absPivot = Fun.Abs(pivot);
            for (long i = k + 1, aik = ak + ay + a_k; i < n; i++, aik += ay)
            {
                double value = alu[aik], absValue = Fun.Abs(value);
                if (absValue > absPivot)
                {
                    pivot = value; absPivot = absValue; pi = i;
                }
            }
            if (Fun.IsTiny(pivot)) return false;
            if (pi != k)
            {
                long api = a0 + pi * ay;
                Fun.Swap(ref p[pi], ref p[k]);
                for (long i = 0, apii = api, aki = ak; i < n; i++, apii += ax, aki += ax)
                    Fun.Swap(ref alu[apii], ref alu[aki]);
            }
            for (long j = k + 1, aj = ak + ay, ajk = aj + a_k; j < n; j++, aj += ay, ajk += ay)
            {
                double factor = alu[ajk] / pivot; alu[ajk] = factor;
                for (long i = k + 1, aji = ajk + ax, aki = ak + a_k + ax; i < n; i++, aji += ax, aki += ax)
                    alu[aji] -= alu[aki] * factor;
            }
        }
        return true;
    }


    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector starting at b0 with element to element
    /// delta bD. The result vector that satisfies the equation is
    /// filled into the supplied parameter x beginning at index x0
    /// with element to element delta xD.
    /// </summary>
    public static unsafe void LuSolve(
            double* lu, long l0, long lx, long ly, int* p,
            double* b, long b0, long bd, double* x, long x0, long xd, long n)
    {
        for (long i = 0, xi = x0; i < n; i++, xi += xd)
            x[xi] = b[b0 + p[i] * bd];

        for (long j = 1, xj = x0 + xd, lj = l0 + ly; j < n; j++, xj += xd, lj += ly)
        {
            double scalar = x[xj];
            for (long i = 0, lji = lj, xi = x0; i < j; i++, lji += lx, xi += xd)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar;
        }

        for (long j = n - 1, xj = x0 + j * xd, lj = l0 + j * ly, ljj = lj + j * lx;
             j >= 0; j--, xj -= xd, lj -= ly, ljj -= ly + lx)
        {
            double scalar = x[xj];
            for (long i = j + 1, xi = xj + xd, lji = ljj + lx; i < n; i++, xi += xd, lji += lx)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar / lu[ljj];
        }
    }


    /// <summary>
    /// Compute the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p. The inverse matrix is filled into the
    /// supplied matrix x.
    /// </summary>
    public static unsafe void LuInverse(
            double* lu, long l0, long lx, long ly, int* p,
            double* x, long x0, long xx, long xy, long n)
    {
        for (long j = 0, xj = x0; j < n; j++, xj += xy)
            for (long i = 0, xji = xj; i < n; i++, xji += xx)
                x[xji] = p[j] == i ? 1 : 0;

        for (long k = 1, xk = x0 + xy, lk = l0 + ly; k < n; k++, xk += xy, lk += ly)
            for (long j = 0, xj = x0, lkj = lk; j < k; j++, xj += xy, lkj += lx)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];

        for (long k = n - 1, xk = x0 + k * xy, lk = l0 + k * ly, lkk = lk + k * lx;
             k >= 0; k--, xk -= xy, lk -= ly, lkk -= ly + lx)
        {
            for (long j = k + 1, lkj = lkk + lx, xj = xk + xy; j < n; j++, lkj += lx, xj += xy)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];
            var factor = lu[lkk]; // storing the inverse would increase the mean error
            for (long i = 0, xki = xk; i < n; i++, xki += xx)
                x[xki] /= factor;
        }
    }


    #endregion



    #region Partially pivoting LU factorization of ComplexD[,] arrays

    // Note that this version of the LU factorization is mostly provided
    // for studying the algorithm. In practice the Matrix<ComplexD> version
    // is noticeably faster, since it performs incremental indexing.

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix, i.e A = inv(P) LU, so that any matrix equation A x = b
    /// can be solved using LU x = P b. The permutation matrix P is
    /// returns as permutation vector. WARNING: the supplied matrix A is
    /// overwritten with its factorization LU, both in the parameter alu.
    /// If the matrix is singular an ArgumentException is thrown.
    /// </summary>
    public static int[] LuFactorize(this ComplexD[,] alu)
    {
        int n = Fun.Min(alu.GetLength(0), alu.GetLength(1));
        int[] perm = new int[n];
        if (alu.LuFactorize(perm)) return perm;
        throw new ArgumentException("singular matrix");
    }

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix, i.e A = inv(P) LU, so that any matrix equation A x = b
    /// can be solved using LU x = P b. The permutation matrix P is
    /// filled into the supplied permutation vector p, and the supplied
    /// matrix A is overwritten with its factorization LU, both in the
    /// parameter alu. The function returns true if the factorization
    /// is successful, otherwise the matrix is singular and false is
    /// returned.
    /// </summary>
    public static bool LuFactorize(this ComplexD[,] alu, int[] p)
    {
        int n = p.Length;
        p.SetByIndex(i => i);
        for (int k = 0; k < n - 1; k++)
        {
            int pi = k;
            ComplexD pivot = alu[pi, k];
            double absPivot = pivot.NormSquared;
            for (int i = k + 1; i < n; i++)
            {
                ComplexD value = alu[i, k];
                double absValue = value.NormSquared;
                if (absValue > absPivot)
                {
                    pivot = value; absPivot = absValue; pi = i;
                }
            }
            if (Fun.IsTiny(pivot.Real) && Fun.IsTiny(pivot.Imag)) return false;
            if (pi != k)
            {
                Fun.Swap(ref p[pi], ref p[k]);
                for (int i = 0; i < n; i++)
                    Fun.Swap(ref alu[pi, i], ref alu[k, i]);
            }
            for (int j = k + 1; j < n; j++)
            {
                ComplexD factor = alu[j, k] / pivot;
                alu[j, k] = factor; // construct L
                for (int i = k + 1; i < n; i++)
                    alu[j, i] -= alu[k, i] * factor;
            }
        }
        return true;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector. The returned x satisfies the equation.
    /// </summary>
    public static ComplexD[] LuSolve(this ComplexD[,] lu, int[] p, ComplexD[] b)
    {
        int n = Fun.Min(lu.GetLength(0), lu.GetLength(1));
        if (n != p.GetLength(0)) throw new ArgumentException();
        if (n > b.GetLength(0)) throw new ArgumentException();
        var x = new ComplexD[n];
        lu.LuSolve(p, b, x);
        return x;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector. The result vector that satisfies the
    /// equation is filled into the supplied parameter x.
    /// </summary>
    public static void LuSolve(this ComplexD[,] lu, int[] p, ComplexD[] b, ComplexD[] x)
    {
        int n = p.Length;
        for (int i = 0; i < n; i++) x[i] = b[p[i]];

        for (int j = 1; j < n; j++)
        {
            ComplexD scalar = x[j];
            for (int i = 0; i < j; i++) scalar -= lu[j, i] * x[i];
            x[j] = scalar;
        }

        for (int j = n - 1; j >= 0; j--)
        {
            ComplexD scalar = x[j];
            for (int i = j + 1; i < n; i++) scalar -= lu[j, i] * x[i];
            x[j] = scalar / lu[j, j];
        }
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix, P is a permutation matrix
    /// supplied as a permutation array, and b is a supplied matrix.
    /// The returned matrix x satisfies the equation.
    /// </summary>
    public static ComplexD[,] LuSolve(this ComplexD[,] lu, int[] p, ComplexD[,] b)
    {
        int n = Fun.Min(lu.GetLength(0), lu.GetLength(1));
        if (n != p.GetLength(0)) throw new ArgumentException();
        if (n > b.GetLength(0)) throw new ArgumentException();
        int m = b.GetLength(1);
        ComplexD[,] x = new ComplexD[n, m];
        lu.LuSolve(p, b, x);
        return x;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix, P is a permutation matrix
    /// supplied as a permutation array, and b is a supplied matrix.
    /// The result matrix that satisfies the equation is filled into the
    /// supplied parameter x.
    /// </summary>
    public static void LuSolve(this ComplexD[,] lu, int[] p, ComplexD[,] b, ComplexD[,] x)
    {
        int n = p.Length;
        int m = b.GetLength(1);
        for (int j = 0; j < n; j++)
            for (int i = 0; i < m; i++)
                x[j, i] = b[p[j], i];

        for (int k = 1; k < n; k++)
            for (int j = 0; j < k; j++)
                for (int i = 0; i < m; i++)
                    x[k, i] -= lu[k, j] * x[j, i];

        for (int k = n - 1; k >= 0; k--)
        {
            for (int j = k + 1; j < n; j++)
                for (int i = 0; i < m; i++)
                    x[k, i] -= lu[k, j] * x[j, i];
            var factor = lu[k, k]; // storing the inverse would increase the mean error
            for (int i = 0; i < m; i++)
                x[k, i] /= factor;
        }
    }

    /// <summary>
    /// Return the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p.
    /// </summary>
    public static ComplexD[,] LuInverse(this ComplexD[,] lu, int[] p)
    {
        int n = Fun.Min(lu.GetLength(0), lu.GetLength(1));
        if (n != p.GetLength(0)) throw new ArgumentException();
        ComplexD[,] inv = new ComplexD[n, n];
        lu.LuInverse(p, inv);
        return inv;
    }

    /// <summary>
    /// Compute the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p. The result matrix is filled into the
    /// supplied matrix.
    /// </summary>
    public static void LuInverse(this ComplexD[,] lu, int[] p, ComplexD[,] inv)
    {
        int n = p.Length;
        for (int j = 0; j < n; j++)
            for (int i = 0; i < n; i++)
                inv[j, i] = p[j] == i ? 1 : 0;

        for (int k = 1; k < n; k++)
            for (int j = 0; j < k; j++)
                for (int i = 0; i < n; i++)
                    inv[k, i] -= lu[k, j] * inv[j, i];

        for (int k = n - 1; k >= 0; k--)
        {
            for (int j = k + 1; j < n; j++)
                for (int i = 0; i < n; i++)
                    inv[k, i] -= lu[k, j] * inv[j, i];
            var factor = lu[k, k]; // storing the inverse would increase the mean error
            for (int i = 0; i < n; i++)
                inv[k, i] /= factor;
        }
    }

    #endregion

    #region Partially pivoting LU factorization of ComplexD[] arrays with offset and strides

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix in-place, i.e A = inv(P) LU, so that any matrix equation
    /// A x = b can be solved using LU x = P b. The permutation matrix P
    /// is filled into the supplied permutation vector p, and the supplied
    /// matrix A is overwritten with its factorization LU, both in the
    /// parameter alu. The function returns true if the factorization
    /// is successful, otherwise the matrix is singular and false is
    /// returned. No size checks are performed.
    /// </summary>
    public static bool LuFactorize(
            this ComplexD[] alu, long a0, long ax, long ay, int[] p)
    {
        long n = p.LongLength;
        p.SetByIndex(i => i);
        for (long k = 0, ak = a0, a_k = 0; k < n - 1; k++, ak += ay, a_k += ax)
        {
            long pi = k;
            ComplexD pivot = alu[ak + a_k];
            double absPivot = pivot.NormSquared;
            for (long i = k + 1, aik = ak + ay + a_k; i < n; i++, aik += ay)
            {
                ComplexD value = alu[aik];
                double absValue = value.NormSquared;
                if (absValue > absPivot)
                {
                    pivot = value; absPivot = absValue; pi = i;
                }
            }
            if (Fun.IsTiny(pivot.Real) && Fun.IsTiny(pivot.Imag)) return false;
            if (pi != k)
            {
                long api = a0 + pi * ay;
                Fun.Swap(ref p[pi], ref p[k]);
                for (long i = 0, apii = api, aki = ak; i < n; i++, apii += ax, aki += ax)
                    Fun.Swap(ref alu[apii], ref alu[aki]);
            }
            for (long j = k + 1, aj = ak + ay, ajk = aj + a_k; j < n; j++, aj += ay, ajk += ay)
            {
                ComplexD factor = alu[ajk] / pivot; alu[ajk] = factor;
                for (long i = k + 1, aji = ajk + ax, aki = ak + a_k + ax; i < n; i++, aji += ax, aki += ax)
                    alu[aji] -= alu[aki] * factor;
            }
        }
        return true;
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector starting at b0 with element to element
    /// delta bD. The result vector that satisfies the equation is
    /// filled into the supplied parameter x beginning at index x0
    /// with element to element delta xD.
    /// </summary>
    public static void LuSolve(
            this ComplexD[] lu, long l0, long lx, long ly, int[] p,
            ComplexD[] b, long b0, long bd, ComplexD[] x, long x0, long xd)
    {
        long n = p.LongLength;
        for (long i = 0, xi = x0; i < n; i++, xi += xd)
            x[xi] = b[b0 + p[i] * bd];

        for (long j = 1, xj = x0 + xd, lj = l0 + ly; j < n; j++, xj += xd, lj += ly)
        {
            ComplexD scalar = x[xj];
            for (long i = 0, lji = lj, xi = x0; i < j; i++, lji += lx, xi += xd)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar;
        }

        for (long j = n - 1, xj = x0 + j * xd, lj = l0 + j * ly, ljj = lj + j * lx;
             j >= 0; j--, xj -= xd, lj -= ly, ljj -= ly + lx)
        {
            ComplexD scalar = x[xj];
            for (long i = j + 1, xi = xj + xd, lji = ljj + lx; i < n; i++, xi += xd, lji += lx)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar / lu[ljj];
        }
    }

    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix, P is a permutation matrix
    /// supplied as a permutation array, and b is a supplied matrix.
    /// The result matrix that satisfies the equation is filled into the
    /// supplied parameter x.
    /// </summary>
    public static void LuSolve(
            this ComplexD[] lu, long l0, long lx, long ly, int[] p,
            long m, ComplexD[] b, long b0, long bx, long by,
            ComplexD[] x, long x0, long xx, long xy)
    {
        int n = p.Length;
        for (long j = 0, xj = x0; j < n; j++, xj += xy)
            for (long i = 0, bpji = b0 + p[j] * by, xji = xj; i < m; i++, bpji += bx, xji += xx)
                x[xji] = b[bpji];

        for (long k = 1, xk = x0 + xy, lk = l0 + ly; k < n; k++, xk += xy, lk += ly)
            for (long j = 0, xj = x0, lkj = lk; j < k; j++, xj += xy, lkj += lx)
                for (long i = 0, xki = xk, xji = xj; i < m; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];

        for (long k = n - 1, xk = x0 + k * xy, lk = l0 + k * ly, lkk = lk + k * lx;
             k >= 0; k--, xk -= xy, lk -= ly, lkk -= ly + lx)
        {
            for (long j = k + 1, lkj = lkk + lx, xj = xk + xy; j < n; j++, lkj += lx, xj += xy)
                for (long i = 0, xki = xk, xji = xj; i < m; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];
            var factor = lu[lkk]; // storing the inverse would increase the mean error
            for (long i = 0, xki = xk; i < m; i++, xki += xx)
                x[xki] /= factor;
        }
    }

    /// <summary>
    /// Compute the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p. The inverse matrix is filled into the
    /// supplied matrix x.
    /// </summary>
    public static void LuInverse(
            this ComplexD[] lu, long l0, long lx, long ly, int[] p,
            ComplexD[] x, long x0, long xx, long xy)
    {
        int n = p.Length;
        for (long j = 0, xj = x0; j < n; j++, xj += xy)
            for (long i = 0, xji = xj; i < n; i++, xji += xx)
                x[xji] = p[j] == i ? 1 : 0;

        for (long k = 1, xk = x0 + xy, lk = l0 + ly; k < n; k++, xk += xy, lk += ly)
            for (long j = 0, xj = x0, lkj = lk; j < k; j++, xj += xy, lkj += lx)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];

        for (long k = n - 1, xk = x0 + k * xy, lk = l0 + k * ly, lkk = lk + k * lx;
             k >= 0; k--, xk -= xy, lk -= ly, lkk -= ly + lx)
        {
            for (long j = k + 1, lkj = lkk + lx, xj = xk + xy; j < n; j++, lkj += lx, xj += xy)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];
            var factor = lu[lkk]; // storing the inverse would increase the mean error
            for (long i = 0, xki = xk; i < n; i++, xki += xx)
                x[xki] /= factor;
        }
    }

    #endregion

    #region Partially pivoting LU factorization of ComplexD* pointers with offset and strides

    /// <summary>
    /// Perform a partially pivoting LU factorization of the supplied
    /// matrix in-place, i.e A = inv(P) LU, so that any matrix equation
    /// A x = b can be solved using LU x = P b. The permutation matrix P
    /// is filled into the supplied permutation vector p, and the supplied
    /// matrix A is overwritten with its factorization LU, both in the
    /// parameter alu. The function returns true if the factorization
    /// is successful, otherwise the matrix is singular and false is
    /// returned. No size checks are performed.
    /// </summary>
    public static unsafe bool LuFactorize(
            ComplexD* alu, long a0, long ax, long ay, int* p, long n)
    {
        for (long i = 0; i < n; i++) p[i] = (int)i;

        for (long k = 0, ak = a0, a_k = 0; k < n - 1; k++, ak += ay, a_k += ax)
        {
            long pi = k;
            ComplexD pivot = alu[ak + a_k];
            double absPivot = pivot.NormSquared;
            for (long i = k + 1, aik = ak + ay + a_k; i < n; i++, aik += ay)
            {
                ComplexD value = alu[aik];
                double absValue = value.NormSquared;
                if (absValue > absPivot)
                {
                    pivot = value; absPivot = absValue; pi = i;
                }
            }
            if (Fun.IsTiny(pivot.Real) && Fun.IsTiny(pivot.Imag)) return false;
            if (pi != k)
            {
                long api = a0 + pi * ay;
                Fun.Swap(ref p[pi], ref p[k]);
                for (long i = 0, apii = api, aki = ak; i < n; i++, apii += ax, aki += ax)
                    Fun.Swap(ref alu[apii], ref alu[aki]);
            }
            for (long j = k + 1, aj = ak + ay, ajk = aj + a_k; j < n; j++, aj += ay, ajk += ay)
            {
                ComplexD factor = alu[ajk] / pivot; alu[ajk] = factor;
                for (long i = k + 1, aji = ajk + ax, aki = ak + a_k + ax; i < n; i++, aji += ax, aki += ax)
                    alu[aji] -= alu[aki] * factor;
            }
        }
        return true;
    }


    /// <summary>
    /// Solve a matrix equation of the form LU x = P b.
    /// LU is the supplied factorized matrix lu, P is a permutation matrix
    /// supplied as a permutation array p, and b is the right hand side
    /// supplied as a vector starting at b0 with element to element
    /// delta bD. The result vector that satisfies the equation is
    /// filled into the supplied parameter x beginning at index x0
    /// with element to element delta xD.
    /// </summary>
    public static unsafe void LuSolve(
            ComplexD* lu, long l0, long lx, long ly, int* p,
            ComplexD* b, long b0, long bd, ComplexD* x, long x0, long xd, long n)
    {
        for (long i = 0, xi = x0; i < n; i++, xi += xd)
            x[xi] = b[b0 + p[i] * bd];

        for (long j = 1, xj = x0 + xd, lj = l0 + ly; j < n; j++, xj += xd, lj += ly)
        {
            ComplexD scalar = x[xj];
            for (long i = 0, lji = lj, xi = x0; i < j; i++, lji += lx, xi += xd)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar;
        }

        for (long j = n - 1, xj = x0 + j * xd, lj = l0 + j * ly, ljj = lj + j * lx;
             j >= 0; j--, xj -= xd, lj -= ly, ljj -= ly + lx)
        {
            ComplexD scalar = x[xj];
            for (long i = j + 1, xi = xj + xd, lji = ljj + lx; i < n; i++, xi += xd, lji += lx)
                scalar -= lu[lji] * x[xi];
            x[xj] = scalar / lu[ljj];
        }
    }


    /// <summary>
    /// Compute the inverse inv(A) of a the supplied LU factorized matrix
    /// lu, where A = inv(P) LU. The permuation matrix P is supplied as
    /// the permutation vector p. The inverse matrix is filled into the
    /// supplied matrix x.
    /// </summary>
    public static unsafe void LuInverse(
            ComplexD* lu, long l0, long lx, long ly, int* p,
            ComplexD* x, long x0, long xx, long xy, long n)
    {
        for (long j = 0, xj = x0; j < n; j++, xj += xy)
            for (long i = 0, xji = xj; i < n; i++, xji += xx)
                x[xji] = p[j] == i ? 1 : 0;

        for (long k = 1, xk = x0 + xy, lk = l0 + ly; k < n; k++, xk += xy, lk += ly)
            for (long j = 0, xj = x0, lkj = lk; j < k; j++, xj += xy, lkj += lx)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];

        for (long k = n - 1, xk = x0 + k * xy, lk = l0 + k * ly, lkk = lk + k * lx;
             k >= 0; k--, xk -= xy, lk -= ly, lkk -= ly + lx)
        {
            for (long j = k + 1, lkj = lkk + lx, xj = xk + xy; j < n; j++, lkj += lx, xj += xy)
                for (long i = 0, xki = xk, xji = xj; i < n; i++, xki += xx, xji += xx)
                    x[xki] -= lu[lkj] * x[xji];
            var factor = lu[lkk]; // storing the inverse would increase the mean error
            for (long i = 0, xki = xk; i < n; i++, xki += xx)
                x[xki] /= factor;
        }
    }


    #endregion





}
