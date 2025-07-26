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

public static class Vector
{
    /// <summary>
    /// Vector constructor from a given array that does not require to write generic type
    /// </summary>
    public static Vector<T> Create<T>(T[] array)
    {
        return new Vector<T>(array);
    }
}

public static class Matrix
{
    /// <summary>
    /// Matrix constructor from a given array that does not require to write generic type.
    /// </summary>
    public static Matrix<T> Create<T>(T[] array, int sx, int sy)
    {
        if (array.Length != sx * sy) throw new ArgumentException("Data array of matrix does not match given size");
        return new Matrix<T>(array, sx, sy);
    }

    /// <summary>
    /// Matrix constructor from a given array that does not require to write generic type.
    /// </summary>
    public static Matrix<T> Create<T>(T[] array, long sx, long sy)
    {
        if (array.Length != sx * sy) throw new ArgumentException("Data array of matrix does not match given size");
        return new Matrix<T>(array, sx, sy);
    }
}

public static class TensorMathExt
{
    #region Vector Extensions

    /// <summary>
    /// Calculates the dot product of two vectors:
    /// a dot b = Sum(a0*b0 + a1*b1 + ... aN*bN)
    /// </summary>
    public static float DotProduct(this Vector<float> v0, Vector<float> v1)
    {
        float result = 0.0f;
        float[] a0 = v0.Data, a1 = v1.Data;
        for (long i0 = v0.Origin, i1 = v1.Origin, e0 = i0 + v0.DSX, d0 = v0.D, d1 = v1.D;
            i0 != e0; i0 += d0, i1 += d1)
            result += a0[i0] * a1[i1];
        return result;
    }

    /// <summary>
    /// Calculates the squared norm of a vector:
    /// ||a|| = Sum(a0*a0 + a1*a1 + ... aN*aN)
    /// </summary>
    public static float NormSquared(this Vector<float> v)
    {
        float result = 0.0f;
        float[] a = v.Data;
        for (long i = v.Origin, e = i + v.DSX, d = v.D; i != e; i += d)
            result += a[i] * a[i];
        return result;
    }

    public static float Dist1(this Vector<float> v0, Vector<float> v1)
        => v0.InnerProduct(v1, (x0, x1) => Fun.Abs(x1 - x0), 0.0f, (s, p) => s + p);

    public static float Dist2Squared(this Vector<float> v0, Vector<float> v1)
        => v0.InnerProduct(v1, (x0, x1) => Fun.Square(x1 - x0), 0.0f, (s, p) => s + p);

    public static float Dist2(this Vector<float> v0, Vector<float> v1)
        => Fun.Sqrt(v0.Dist2Squared(v1));

    public static float DistMax(this Vector<float> v0, Vector<float> v1)
        => v0.InnerProduct(v1, (x0, x1) => Fun.Abs(x1 - x0), 0.0f, Fun.Max);

    public static Vector<float> Multiply(this Vector<float> a, Vector<float> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException("Cannot multiply vectors with different lengths!");

        var result = new float[a.Dim];
        float[] a0 = a.Data, a1 = b.Data;
        for (long i0 = a.Origin, ri = 0, i1 = b.Origin, e0 = i0 + a.DSX, d0 = a.D, d1 = b.D;
            i0 != e0; i0 += d0, i1 += d1, ri++)
            result[ri] = a0[i0] * a1[i1];
        return Vector.Create(result);
    }

    /// <summary>
    /// Multiply vector a as column vector with vector b as row vector resulting a matrix [b.Dim x a.Dim]
    /// </summary>
    public static Matrix<float> MultiplyTransposed(this Vector<float> a, Vector<float> b)
    {
        var result = new float[b.Dim * a.Dim];
        float[] a0 = a.Data, a1 = b.Data;
        long f1 = b.Origin, e1 = f1 + b.DSX, d1 = b.D;
        for (long i0 = a.Origin, ri = 0, e0 = i0 + a.DSX, d0 = a.D; i0 != e0; i0 += d0)
            for (long i1 = f1; i1 != e1; i1 += d1, ri++)
                result[ri] = a0[i0] * a1[i1];
        return Matrix.Create(result, b.Dim, a.Dim);
    }

    public static Vector<float> Multiply(this Vector<float> vec, float s)
    {
        var result = new float[vec.Dim];
        float[] a0 = vec.Data;
        for (long i0 = vec.Origin, ri = 0, e0 = i0 + vec.DSX, d0 = vec.D; i0 != e0; i0 += d0, ri++)
            result[ri] = a0[i0] * s;
        return Vector.Create(result);
    }

    public static Vector<float> Subtract(this Vector<float> a, Vector<float> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));
        
        var result = new float[a.Dim];
        float[] a0 = a.Data, a1 = b.Data;
        for (long i0 = a.Origin, ri = 0, i1 = b.Origin, e0 = i0 + a.DSX, d0 = a.D, d1 = b.D;
            i0 != e0; i0 += d0, i1 += d1, ri++)
            result[ri] = a0[i0] - a1[i1];
        return Vector.Create(result);
    }

    public static Vector<float> Subtract(this Vector<float> a, float b)
    {
        var result = new float[a.Dim];
        float[] a0 = a.Data;
        for (long i0 = a.Origin, ri = 0, e0 = i0 + a.DSX, d0 = a.D; i0 != e0; i0 += d0, ri++)
            result[ri] = a0[i0] - b;
        return Vector.Create(result);
    }

    public static Vector<float> Add(this Vector<float> a, Vector<float> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));

        var result = new float[a.Dim];
        float[] a0 = a.Data, a1 = b.Data;
        for (long i0 = a.Origin, ri = 0, i1 = b.Origin, e0 = i0 + a.DSX, d0 = a.D, d1 = b.D;
            i0 != e0; i0 += d0, i1 += d1, ri++)
            result[ri] = a0[i0] + a1[i1];
        return Vector.Create(result);
    }

    public static Vector<float> Add(this Vector<float> a, float b)
    {
        var result = new float[a.Dim];
        float[] a0 = a.Data;
        for (long i0 = a.Origin, ri = 0, e0 = i0 + a.DSX, d0 = a.D; i0 != e0; i0 += d0, ri++)
            result[ri] = a0[i0] + b;
        return Vector.Create(result);
    }

    public static bool EqualTo(this Vector<float> a, Vector<float> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException("Cannot compare vectors with different lengths!");
        bool eq = true;
        a.ForeachCoord(crd => eq &= a[crd] == b[crd]);
        return eq;
    }

    #endregion

    #region Matrix Extensions

    /// <summary>
    /// Multiply matrix with vector as column vector. 
    /// Vector must have save length as columns of matrix.
    /// Returns a vector with size of matrix rows.
    /// </summary>
    public static Vector<float> Multiply(this Matrix<float> mat, Vector<float> vec)
    {
        if (mat.Dim.X != vec.Dim) throw new InvalidOperationException(String.Format("Cannot multiply matrix {0} with vector of size {1}", mat.Dim, vec.Dim));
        
        var result = new float[mat.Dim.Y];

        var data0 = mat.Data; var data1 = vec.Data;
        long my0 = mat.DY, d1 = vec.D;
        long mf1 = vec.FirstIndex;
        long ds0 = mat.DSX, d0 = mat.DX;
        for (long ri = 0, ye = mat.FirstIndex + mat.DSY, f0 = mat.FirstIndex, e0 = f0 + ds0;
            f0 != ye; f0 += my0, e0 += my0, ri++)
        {
            float dot = 0.0f;
            for (long i0 = f0, i1 = mf1; i0 != e0; i0 += d0, i1 += d1)
                dot += data0[i0] * data1[i1];

            result[ri] = dot;
        }
        return Vector.Create(result);
    }
    
    public static Matrix<float> Multiply(this Matrix<float> m0, Matrix<float> m1)
    {
        if (m0.SX != m1.SY)
            throw new ArgumentException("m0.SX != m1.SY");

        var result = new Matrix<float>(m1.SX, m0.SY);

        var data = result.Data; var data0 = m0.Data; var data1 = m1.Data;
        long i = result.FirstIndex, yj = result.JY, my0 = m0.DY;
        long xs = result.DSX, mf1 = m1.FirstIndex, xj = result.JX, mx1 = m1.DX;
        long ds0 = m0.DSX, d0 = m0.DX, d1 = m1.DY;
        for (long ye = i + result.DSY, f0 = m0.FirstIndex, e0 = f0 + ds0;
            i != ye; i += yj, f0 += my0, e0 += my0)
            for (long xe = i + xs, f1 = mf1; i != xe; i += xj, f1 += mx1)
            {
                float dot = 0.0f;
                for (long i0 = f0, i1 = f1; i0 != e0; i0 += d0, i1 += d1)
                    dot += data0[i0] * data1[i1];
                data[i] = dot;
            }
        return result;
    }

    public static Matrix<float> Multiply(this Matrix<float> mat, float a)
    {
        var result = new float[mat.SX * mat.SY];

        var data0 = mat.Data;
        long my0 = mat.DY;

        //long mf0 = mat.FirstIndex; // mf0 is never used

        long ds0 = mat.DSX, d0 = mat.DX;
        for (long ri = 0, ye = mat.FirstIndex + mat.DSY, f0 = mat.FirstIndex, e0 = f0 + ds0;
        f0 != ye; f0 += my0, e0 += my0)
        {
            for (long i0 = f0; i0 != e0; i0 += d0, ri++)
                result[ri] = data0[i0] * a;
        }
        return Matrix.Create(result, mat.SX, mat.SY);
    }

    public static Matrix<float> Subtract(this Matrix<float> m0, Matrix<float> m1)
    {
        if (m0.Dim != m1.Dim) throw new InvalidOperationException(String.Format("Cannot subtract matrix A={0} and matrix B={1}", m0.Dim, m1.Dim));

        var result = new float[m0.SX * m0.SY];

        var data0 = m0.Data; var data1 = m1.Data;
        long mf0 = m0.FirstIndex, my0 = m0.DY, my1 = m1.DY;
        long ds0 = m0.DSX, d0 = m0.DX, d1 = m1.DX;
        for (long ye = mf0 + m0.DSY, f0 = mf0, f1 = m1.FirstIndex, ri = 0;
            f0 != ye; f0 += my0, f1 += my1)
            for (long xe = f0 + ds0, i0 = f0, i1 = f1; i0 != xe; i0 += d0, i1 += d1, ri++)
                result[ri] = data0[i0] - data1[i1];
        return Matrix.Create(result, m0.SX, m0.SY);
    }

    public static Matrix<float> Subtract(this Matrix<float> mat, float a)
    {
        var result = new float[mat.SX * mat.SY];

        var data0 = mat.Data;
        long my0 = mat.DY;
        //long mf0 = mat.FirstIndex; // mf0 is never used
        long ds0 = mat.DSX, d0 = mat.DX;
        for (long ri = 0, ye = mat.FirstIndex + mat.DSY, f0 = mat.FirstIndex, e0 = f0 + ds0;
        f0 != ye; f0 += my0, e0 += my0)
        {
            for (long i0 = f0; i0 != e0; i0 += d0, ri++)
                result[ri] = data0[i0] - a;
        }
        return Matrix.Create(result, mat.SX, mat.SY);
    }

    public static Matrix<float> Add(this Matrix<float> m0, Matrix<float> m1)
    {
        if (m0.Dim != m1.Dim) throw new InvalidOperationException(String.Format("Cannot add matrix A={0} and matrix B={1}", m0.Dim, m1.Dim));

        var result = new float[m0.SX * m0.SY];

        var data0 = m0.Data; var data1 = m1.Data;
        long mf0 = m0.FirstIndex, my0 = m0.DY, my1 = m1.DY;
        long ds0 = m0.DSX, d0 = m0.DX, d1 = m1.DX;
        for (long ye = mf0 + m0.DSY, f0 = mf0, f1 = m1.FirstIndex, ri = 0;
            f0 != ye; f0 += my0, f1 += my1)
            for (long xe = f0 + ds0, i0 = f0, i1 = f1; i0 != xe; i0 += d0, i1 += d1, ri++)
                result[ri] = data0[i0] + data1[i1];
        return Matrix.Create(result, m0.SX, m0.SY);
    }

    public static Matrix<float> Add(this Matrix<float> mat, float a)
    {
        var result = new float[mat.SX * mat.SY];

        var data0 = mat.Data;
        long my0 = mat.DY;
        //long mf0 = mat.FirstIndex; // mf0 is never used
        long ds0 = mat.DSX, d0 = mat.DX;
        for (long ri = 0, ye = mat.FirstIndex + mat.DSY, f0 = mat.FirstIndex, e0 = f0 + ds0;
        f0 != ye; f0 += my0, e0 += my0)
        {
            for (long i0 = f0; i0 != e0; i0 += d0, ri++)
                result[ri] = data0[i0] + a;
        }
        return Matrix.Create(result, mat.SX, mat.SY);
    }

    public static bool EqualTo(this Matrix<float> a, Matrix<float> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot compare matrix A={0} and matrix B={1}", a.Dim, b.Dim));
        bool eq = true;
        a.ForeachCoord(crd => eq &= a[crd] == b[crd]);
        return eq;
    }

    #endregion

    #region Vector Extensions

    /// <summary>
    /// Calculates the dot product of two vectors:
    /// a dot b = Sum(a0*b0 + a1*b1 + ... aN*bN)
    /// </summary>
    public static double DotProduct(this Vector<double> v0, Vector<double> v1)
    {
        double result = 0.0;
        double[] a0 = v0.Data, a1 = v1.Data;
        for (long i0 = v0.Origin, i1 = v1.Origin, e0 = i0 + v0.DSX, d0 = v0.D, d1 = v1.D;
            i0 != e0; i0 += d0, i1 += d1)
            result += a0[i0] * a1[i1];
        return result;
    }

    /// <summary>
    /// Calculates the squared norm of a vector:
    /// ||a|| = Sum(a0*a0 + a1*a1 + ... aN*aN)
    /// </summary>
    public static double NormSquared(this Vector<double> v)
    {
        double result = 0.0;
        double[] a = v.Data;
        for (long i = v.Origin, e = i + v.DSX, d = v.D; i != e; i += d)
            result += a[i] * a[i];
        return result;
    }

    public static double Dist1(this Vector<double> v0, Vector<double> v1)
        => v0.InnerProduct(v1, (x0, x1) => Fun.Abs(x1 - x0), 0.0, (s, p) => s + p);

    public static double Dist2Squared(this Vector<double> v0, Vector<double> v1)
        => v0.InnerProduct(v1, (x0, x1) => Fun.Square(x1 - x0), 0.0, (s, p) => s + p);

    public static double Dist2(this Vector<double> v0, Vector<double> v1)
        => Fun.Sqrt(v0.Dist2Squared(v1));

    public static double DistMax(this Vector<double> v0, Vector<double> v1)
        => v0.InnerProduct(v1, (x0, x1) => Fun.Abs(x1 - x0), 0.0, Fun.Max);

    public static Vector<double> Multiply(this Vector<double> a, Vector<double> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException("Cannot multiply vectors with different lengths!");

        var result = new double[a.Dim];
        double[] a0 = a.Data, a1 = b.Data;
        for (long i0 = a.Origin, ri = 0, i1 = b.Origin, e0 = i0 + a.DSX, d0 = a.D, d1 = b.D;
            i0 != e0; i0 += d0, i1 += d1, ri++)
            result[ri] = a0[i0] * a1[i1];
        return Vector.Create(result);
    }

    /// <summary>
    /// Multiply vector a as column vector with vector b as row vector resulting a matrix [b.Dim x a.Dim]
    /// </summary>
    public static Matrix<double> MultiplyTransposed(this Vector<double> a, Vector<double> b)
    {
        var result = new double[b.Dim * a.Dim];
        double[] a0 = a.Data, a1 = b.Data;
        long f1 = b.Origin, e1 = f1 + b.DSX, d1 = b.D;
        for (long i0 = a.Origin, ri = 0, e0 = i0 + a.DSX, d0 = a.D; i0 != e0; i0 += d0)
            for (long i1 = f1; i1 != e1; i1 += d1, ri++)
                result[ri] = a0[i0] * a1[i1];
        return Matrix.Create(result, b.Dim, a.Dim);
    }

    public static Vector<double> Multiply(this Vector<double> vec, double s)
    {
        var result = new double[vec.Dim];
        double[] a0 = vec.Data;
        for (long i0 = vec.Origin, ri = 0, e0 = i0 + vec.DSX, d0 = vec.D; i0 != e0; i0 += d0, ri++)
            result[ri] = a0[i0] * s;
        return Vector.Create(result);
    }

    public static Vector<double> Subtract(this Vector<double> a, Vector<double> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));
        
        var result = new double[a.Dim];
        double[] a0 = a.Data, a1 = b.Data;
        for (long i0 = a.Origin, ri = 0, i1 = b.Origin, e0 = i0 + a.DSX, d0 = a.D, d1 = b.D;
            i0 != e0; i0 += d0, i1 += d1, ri++)
            result[ri] = a0[i0] - a1[i1];
        return Vector.Create(result);
    }

    public static Vector<double> Subtract(this Vector<double> a, double b)
    {
        var result = new double[a.Dim];
        double[] a0 = a.Data;
        for (long i0 = a.Origin, ri = 0, e0 = i0 + a.DSX, d0 = a.D; i0 != e0; i0 += d0, ri++)
            result[ri] = a0[i0] - b;
        return Vector.Create(result);
    }

    public static Vector<double> Add(this Vector<double> a, Vector<double> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));

        var result = new double[a.Dim];
        double[] a0 = a.Data, a1 = b.Data;
        for (long i0 = a.Origin, ri = 0, i1 = b.Origin, e0 = i0 + a.DSX, d0 = a.D, d1 = b.D;
            i0 != e0; i0 += d0, i1 += d1, ri++)
            result[ri] = a0[i0] + a1[i1];
        return Vector.Create(result);
    }

    public static Vector<double> Add(this Vector<double> a, double b)
    {
        var result = new double[a.Dim];
        double[] a0 = a.Data;
        for (long i0 = a.Origin, ri = 0, e0 = i0 + a.DSX, d0 = a.D; i0 != e0; i0 += d0, ri++)
            result[ri] = a0[i0] + b;
        return Vector.Create(result);
    }

    public static bool EqualTo(this Vector<double> a, Vector<double> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException("Cannot compare vectors with different lengths!");
        bool eq = true;
        a.ForeachCoord(crd => eq &= a[crd] == b[crd]);
        return eq;
    }

    #endregion

    #region Matrix Extensions

    /// <summary>
    /// Multiply matrix with vector as column vector. 
    /// Vector must have save length as columns of matrix.
    /// Returns a vector with size of matrix rows.
    /// </summary>
    public static Vector<double> Multiply(this Matrix<double> mat, Vector<double> vec)
    {
        if (mat.Dim.X != vec.Dim) throw new InvalidOperationException(String.Format("Cannot multiply matrix {0} with vector of size {1}", mat.Dim, vec.Dim));
        
        var result = new double[mat.Dim.Y];

        var data0 = mat.Data; var data1 = vec.Data;
        long my0 = mat.DY, d1 = vec.D;
        long mf1 = vec.FirstIndex;
        long ds0 = mat.DSX, d0 = mat.DX;
        for (long ri = 0, ye = mat.FirstIndex + mat.DSY, f0 = mat.FirstIndex, e0 = f0 + ds0;
            f0 != ye; f0 += my0, e0 += my0, ri++)
        {
            double dot = 0.0;
            for (long i0 = f0, i1 = mf1; i0 != e0; i0 += d0, i1 += d1)
                dot += data0[i0] * data1[i1];

            result[ri] = dot;
        }
        return Vector.Create(result);
    }
    
    public static Matrix<double> Multiply(this Matrix<double> m0, Matrix<double> m1)
    {
        if (m0.SX != m1.SY)
            throw new ArgumentException("m0.SX != m1.SY");

        var result = new Matrix<double>(m1.SX, m0.SY);

        var data = result.Data; var data0 = m0.Data; var data1 = m1.Data;
        long i = result.FirstIndex, yj = result.JY, my0 = m0.DY;
        long xs = result.DSX, mf1 = m1.FirstIndex, xj = result.JX, mx1 = m1.DX;
        long ds0 = m0.DSX, d0 = m0.DX, d1 = m1.DY;
        for (long ye = i + result.DSY, f0 = m0.FirstIndex, e0 = f0 + ds0;
            i != ye; i += yj, f0 += my0, e0 += my0)
            for (long xe = i + xs, f1 = mf1; i != xe; i += xj, f1 += mx1)
            {
                double dot = 0.0;
                for (long i0 = f0, i1 = f1; i0 != e0; i0 += d0, i1 += d1)
                    dot += data0[i0] * data1[i1];
                data[i] = dot;
            }
        return result;
    }

    public static Matrix<double> Multiply(this Matrix<double> mat, double a)
    {
        var result = new double[mat.SX * mat.SY];

        var data0 = mat.Data;
        long my0 = mat.DY;

        //long mf0 = mat.FirstIndex; // mf0 is never used

        long ds0 = mat.DSX, d0 = mat.DX;
        for (long ri = 0, ye = mat.FirstIndex + mat.DSY, f0 = mat.FirstIndex, e0 = f0 + ds0;
        f0 != ye; f0 += my0, e0 += my0)
        {
            for (long i0 = f0; i0 != e0; i0 += d0, ri++)
                result[ri] = data0[i0] * a;
        }
        return Matrix.Create(result, mat.SX, mat.SY);
    }

    public static Matrix<double> Subtract(this Matrix<double> m0, Matrix<double> m1)
    {
        if (m0.Dim != m1.Dim) throw new InvalidOperationException(String.Format("Cannot subtract matrix A={0} and matrix B={1}", m0.Dim, m1.Dim));

        var result = new double[m0.SX * m0.SY];

        var data0 = m0.Data; var data1 = m1.Data;
        long mf0 = m0.FirstIndex, my0 = m0.DY, my1 = m1.DY;
        long ds0 = m0.DSX, d0 = m0.DX, d1 = m1.DX;
        for (long ye = mf0 + m0.DSY, f0 = mf0, f1 = m1.FirstIndex, ri = 0;
            f0 != ye; f0 += my0, f1 += my1)
            for (long xe = f0 + ds0, i0 = f0, i1 = f1; i0 != xe; i0 += d0, i1 += d1, ri++)
                result[ri] = data0[i0] - data1[i1];
        return Matrix.Create(result, m0.SX, m0.SY);
    }

    public static Matrix<double> Subtract(this Matrix<double> mat, double a)
    {
        var result = new double[mat.SX * mat.SY];

        var data0 = mat.Data;
        long my0 = mat.DY;
        //long mf0 = mat.FirstIndex; // mf0 is never used
        long ds0 = mat.DSX, d0 = mat.DX;
        for (long ri = 0, ye = mat.FirstIndex + mat.DSY, f0 = mat.FirstIndex, e0 = f0 + ds0;
        f0 != ye; f0 += my0, e0 += my0)
        {
            for (long i0 = f0; i0 != e0; i0 += d0, ri++)
                result[ri] = data0[i0] - a;
        }
        return Matrix.Create(result, mat.SX, mat.SY);
    }

    public static Matrix<double> Add(this Matrix<double> m0, Matrix<double> m1)
    {
        if (m0.Dim != m1.Dim) throw new InvalidOperationException(String.Format("Cannot add matrix A={0} and matrix B={1}", m0.Dim, m1.Dim));

        var result = new double[m0.SX * m0.SY];

        var data0 = m0.Data; var data1 = m1.Data;
        long mf0 = m0.FirstIndex, my0 = m0.DY, my1 = m1.DY;
        long ds0 = m0.DSX, d0 = m0.DX, d1 = m1.DX;
        for (long ye = mf0 + m0.DSY, f0 = mf0, f1 = m1.FirstIndex, ri = 0;
            f0 != ye; f0 += my0, f1 += my1)
            for (long xe = f0 + ds0, i0 = f0, i1 = f1; i0 != xe; i0 += d0, i1 += d1, ri++)
                result[ri] = data0[i0] + data1[i1];
        return Matrix.Create(result, m0.SX, m0.SY);
    }

    public static Matrix<double> Add(this Matrix<double> mat, double a)
    {
        var result = new double[mat.SX * mat.SY];

        var data0 = mat.Data;
        long my0 = mat.DY;
        //long mf0 = mat.FirstIndex; // mf0 is never used
        long ds0 = mat.DSX, d0 = mat.DX;
        for (long ri = 0, ye = mat.FirstIndex + mat.DSY, f0 = mat.FirstIndex, e0 = f0 + ds0;
        f0 != ye; f0 += my0, e0 += my0)
        {
            for (long i0 = f0; i0 != e0; i0 += d0, ri++)
                result[ri] = data0[i0] + a;
        }
        return Matrix.Create(result, mat.SX, mat.SY);
    }

    public static bool EqualTo(this Matrix<double> a, Matrix<double> b)
    {
        if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot compare matrix A={0} and matrix B={1}", a.Dim, b.Dim));
        bool eq = true;
        a.ForeachCoord(crd => eq &= a[crd] == b[crd]);
        return eq;
    }

    #endregion

}
