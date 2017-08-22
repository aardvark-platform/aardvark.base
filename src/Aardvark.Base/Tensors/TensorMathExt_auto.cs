using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
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

        public static float DotProduct(this Vector<float> v0, Vector<float> v1)
        {
            float result = 0.0f;
            float[] a0 = v0.Data, a1 = v1.Data;
            for (int i0 = (int)v0.Origin, i1 = (int)v1.Origin, e0 = i0 + (int)v0.DSX, d0 = (int)v0.D, d1 = (int)v1.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result += a0[i0] * a1[i1];
            return result;
        }

        public static float NormSquared(this Vector<float> v)
        {
            float result = 0.0f;
            float[] a = v.Data;
            for (int i = (int)v.Origin, e = i + (int)v.DSX, d = (int)v.D; i != e; i += d)
                result += a[i] * a[i];
            return result;
        }

        public static Vector<float> Multiply(this Vector<float> a, Vector<float> b)
        {
            if (a.Dim != b.Dim) throw new InvalidOperationException("Cannot multiply vectors with different lengths!");

            var result = new float[a.Dim];
            var ri = 0;
            float[] a0 = a.Data, a1 = b.Data;
            for (int i0 = (int)a.Origin, i1 = (int)b.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D, d1 = (int)b.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result[ri++] = a0[i0] * a1[i1];
            return Vector.Create(result);
        }

        /// <summary>
        /// Multiply vector a as column vector with vector b as row vector resulting a matrix [b.Dim x a.Dim]
        /// </summary>
        public static Matrix<float> MultiplyTransposed(this Vector<float> a, Vector<float> b)
        {
            var result = new float[b.Dim * a.Dim];
            var ri = 0;
            float[] a0 = a.Data, a1 = b.Data;
            int f1 = (int)b.Origin, e1 = f1 + (int)b.DSX, d1 = (int)b.D;
            for (int i0 = (int)a.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D; i0 != e0; i0 += d0)
                for (int i1 = f1; i1 != e1; i1 += d1)
                    result[ri++] = a0[i0] * a1[i1];
            return Matrix.Create(result, b.Dim, a.Dim);
        }

        public static Vector<float> Multiply(this Vector<float> vec, float s)
        {
            var result = new float[vec.Dim];
            var ri = 0;
            float[] a0 = vec.Data;
            for (int i0 = (int)vec.Origin, e0 = i0 + (int)vec.DSX, d0 = (int)vec.D; i0 != e0; i0 += d0)
                result[ri++] = a0[i0] * s;
            return Vector.Create(result);
        }

        public static Vector<float> Subtract(this Vector<float> a, Vector<float> b)
        {
            if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));
            
            var result = new float[a.Dim];
            var ri = 0;
            float[] a0 = a.Data, a1 = b.Data;
            for (int i0 = (int)a.Origin, i1 = (int)b.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D, d1 = (int)b.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result[ri++] = a0[i0] - a1[i1];
            return Vector.Create(result);
        }

        public static Vector<float> Subtract(this Vector<float> a, float b)
        {
            var result = new float[a.Dim];
            var ri = 0;
            float[] a0 = a.Data;
            for (int i0 = (int)a.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D; i0 != e0; i0 += d0)
                result[ri++] = a0[i0] - b;
            return Vector.Create(result);
        }

        public static Vector<float> Add(this Vector<float> a, Vector<float> b)
        {
            if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));

            var result = new float[a.Dim];
            var ri = 0;
            float[] a0 = a.Data, a1 = b.Data;
            for (int i0 = (int)a.Origin, i1 = (int)b.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D, d1 = (int)b.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result[ri++] = a0[i0] + a1[i1];
            return Vector.Create(result);
        }

        public static Vector<float> Add(this Vector<float> a, float b)
        {
            var result = new float[a.Dim];
            var ri = 0;
            float[] a0 = a.Data;
            for (int i0 = (int)a.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D; i0 != e0; i0 += d0)
                result[ri++] = a0[i0] + b;
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
            if (mat.Dim.X != vec.Dim) throw new InvalidOperationException(String.Format("Cannot multiply matrix {0} with vector of size {2}", mat.Dim, vec.Dim));
            
            var result = new float[mat.Dim.Y];

            var data0 = mat.Data; var data1 = vec.Data;
            int my0 = (int)mat.DY, d1 = (int)vec.D;
            int mf1 = (int)vec.FirstIndex;
            int ds0 = (int)mat.DSX, d0 = (int)mat.DX;
            for (int ri = 0, ye = (int)mat.FirstIndex + (int)mat.DSY, f0 = (int)mat.FirstIndex, e0 = f0 + ds0;
                 f0 != ye; f0 += my0, e0 += my0, ri++)
            {
                float dot = 0.0f;
                for (int i0 = f0, i1 = mf1; i0 != e0; i0 += d0, i1 += d1)
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
            int i = (int)result.FirstIndex, yj = (int)result.JY, my0 = (int)m0.DY;
            int xs = (int)result.DSX, mf1 = (int)m1.FirstIndex, xj = (int)result.JX, mx1 = (int)m1.DX;
            int ds0 = (int)m0.DSX, d0 = (int)m0.DX, d1 = (int)m1.DY;
            for (int ye = i + (int)result.DSY, f0 = (int)m0.FirstIndex, e0 = f0 + ds0;
                 i != ye; i += yj, f0 += my0, e0 += my0)
                for (int xe = i + xs, f1 = mf1; i != xe; i += xj, f1 += mx1)
                {
                    float dot = 0.0f;
                    for (int i0 = f0, i1 = f1; i0 != e0; i0 += d0, i1 += d1)
                        dot += data0[i0] * data1[i1];
                    data[i] = dot;
                }

            return result;
        }

        public static Matrix<float> Multiply(this Matrix<float> mat, float a)
        {
            var result = new float[mat.SX * mat.SY];

            var data0 = mat.Data;
            int my0 = (int)mat.DY;
            int mf0 = (int)mat.FirstIndex;
            int ds0 = (int)mat.DSX, d0 = (int)mat.DX;
            for (int ri = 0, ye = (int)mat.FirstIndex + (int)mat.DSY, f0 = (int)mat.FirstIndex, e0 = f0 + ds0; 
                f0 != ye; f0 += my0, e0 += my0)
            {
                for (int i0 = f0; i0 != e0; i0 += d0, ri++)
                    result[ri] = data0[i0] * a;
            }
            return Matrix.Create(result, mat.SX, mat.SY);
        }

        public static Matrix<float> Subtract(this Matrix<float> m0, Matrix<float> m1)
        {
            if (m0.Dim != m1.Dim) throw new InvalidOperationException(String.Format("Cannot subtract matrix A={0} and matrix B={1}", m0.Dim, m1.Dim));

            var result = new float[m0.SX * m0.SY];

            var data0 = m0.Data; var data1 = m1.Data;
            int mf0 = (int)m0.FirstIndex, my0 = (int)m0.DY, my1 = (int)m0.DY;
            int ds0 = (int)m0.DSX, d0 = (int)m0.DX, d1 = (int)m1.DX;
            for (int ye = mf0 + (int)m0.DSY, f0 = mf0, f1 = (int)m1.FirstIndex, ri = 0;
                 f0 != ye; f0 += my0, f1 += my1)
                for (int xe = f0 + ds0, i0 = f0, i1 = f1; i0 != xe; i0 += d0, i1 += d1, ri++)
                    result[ri] = data0[i0] - data1[i1];

            return Matrix.Create(result, m0.SX, m0.SY);
        }

        public static Matrix<float> Subtract(this Matrix<float> mat, float a)
        {
            var result = new float[mat.SX * mat.SY];

            var data0 = mat.Data;
            int my0 = (int)mat.DY;
            int mf0 = (int)mat.FirstIndex;
            int ds0 = (int)mat.DSX, d0 = (int)mat.DX;
            for (int ri = 0, ye = (int)mat.FirstIndex + (int)mat.DSY, f0 = (int)mat.FirstIndex, e0 = f0 + ds0;
                f0 != ye; f0 += my0, e0 += my0)
            {
                for (int i0 = f0; i0 != e0; i0 += d0, ri++)
                    result[ri] = data0[i0] - a;
            }
            return Matrix.Create(result, mat.SX, mat.SY);
        }

        public static Matrix<float> Add(this Matrix<float> m0, Matrix<float> m1)
        {
            if (m0.Dim != m1.Dim) throw new InvalidOperationException(String.Format("Cannot add matrix A={0} and matrix B={1}", m0.Dim, m1.Dim));

            var result = new float[m0.SX * m0.SY];

            var data0 = m0.Data; var data1 = m1.Data;
            int mf0 = (int)m0.FirstIndex, my0 = (int)m0.DY, my1 = (int)m0.DY;
            int ds0 = (int)m0.DSX, d0 = (int)m0.DX, d1 = (int)m1.DX;
            for (int ye = mf0 + (int)m0.DSY, f0 = mf0, f1 = (int)m1.FirstIndex, ri = 0;
                 f0 != ye; f0 += my0, f1 += my1)
                for (int xe = f0 + ds0, i0 = f0, i1 = f1; i0 != xe; i0 += d0, i1 += d1, ri++)
                    result[ri] = data0[i0] + data1[i1];

            return Matrix.Create(result, m0.SX, m0.SY);
        }

        public static Matrix<float> Add(this Matrix<float> mat, float a)
        {
            var result = new float[mat.SX * mat.SY];

            var data0 = mat.Data;
            int my0 = (int)mat.DY;
            int mf0 = (int)mat.FirstIndex;
            int ds0 = (int)mat.DSX, d0 = (int)mat.DX;
            for (int ri = 0, ye = (int)mat.FirstIndex + (int)mat.DSY, f0 = (int)mat.FirstIndex, e0 = f0 + ds0;
                f0 != ye; f0 += my0, e0 += my0)
            {
                for (int i0 = f0; i0 != e0; i0 += d0, ri++)
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

        public static double DotProduct(this Vector<double> v0, Vector<double> v1)
        {
            double result = 0.0d;
            double[] a0 = v0.Data, a1 = v1.Data;
            for (int i0 = (int)v0.Origin, i1 = (int)v1.Origin, e0 = i0 + (int)v0.DSX, d0 = (int)v0.D, d1 = (int)v1.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result += a0[i0] * a1[i1];
            return result;
        }

        public static double NormSquared(this Vector<double> v)
        {
            double result = 0.0d;
            double[] a = v.Data;
            for (int i = (int)v.Origin, e = i + (int)v.DSX, d = (int)v.D; i != e; i += d)
                result += a[i] * a[i];
            return result;
        }

        public static Vector<double> Multiply(this Vector<double> a, Vector<double> b)
        {
            if (a.Dim != b.Dim) throw new InvalidOperationException("Cannot multiply vectors with different lengths!");

            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data, a1 = b.Data;
            for (int i0 = (int)a.Origin, i1 = (int)b.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D, d1 = (int)b.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result[ri++] = a0[i0] * a1[i1];
            return Vector.Create(result);
        }

        /// <summary>
        /// Multiply vector a as column vector with vector b as row vector resulting a matrix [b.Dim x a.Dim]
        /// </summary>
        public static Matrix<double> MultiplyTransposed(this Vector<double> a, Vector<double> b)
        {
            var result = new double[b.Dim * a.Dim];
            var ri = 0;
            double[] a0 = a.Data, a1 = b.Data;
            int f1 = (int)b.Origin, e1 = f1 + (int)b.DSX, d1 = (int)b.D;
            for (int i0 = (int)a.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D; i0 != e0; i0 += d0)
                for (int i1 = f1; i1 != e1; i1 += d1)
                    result[ri++] = a0[i0] * a1[i1];
            return Matrix.Create(result, b.Dim, a.Dim);
        }

        public static Vector<double> Multiply(this Vector<double> vec, double s)
        {
            var result = new double[vec.Dim];
            var ri = 0;
            double[] a0 = vec.Data;
            for (int i0 = (int)vec.Origin, e0 = i0 + (int)vec.DSX, d0 = (int)vec.D; i0 != e0; i0 += d0)
                result[ri++] = a0[i0] * s;
            return Vector.Create(result);
        }

        public static Vector<double> Subtract(this Vector<double> a, Vector<double> b)
        {
            if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));
            
            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data, a1 = b.Data;
            for (int i0 = (int)a.Origin, i1 = (int)b.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D, d1 = (int)b.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result[ri++] = a0[i0] - a1[i1];
            return Vector.Create(result);
        }

        public static Vector<double> Subtract(this Vector<double> a, double b)
        {
            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data;
            for (int i0 = (int)a.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D; i0 != e0; i0 += d0)
                result[ri++] = a0[i0] - b;
            return Vector.Create(result);
        }

        public static Vector<double> Add(this Vector<double> a, Vector<double> b)
        {
            if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));

            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data, a1 = b.Data;
            for (int i0 = (int)a.Origin, i1 = (int)b.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D, d1 = (int)b.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result[ri++] = a0[i0] + a1[i1];
            return Vector.Create(result);
        }

        public static Vector<double> Add(this Vector<double> a, double b)
        {
            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data;
            for (int i0 = (int)a.Origin, e0 = i0 + (int)a.DSX, d0 = (int)a.D; i0 != e0; i0 += d0)
                result[ri++] = a0[i0] + b;
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
            if (mat.Dim.X != vec.Dim) throw new InvalidOperationException(String.Format("Cannot multiply matrix {0} with vector of size {2}", mat.Dim, vec.Dim));
            
            var result = new double[mat.Dim.Y];

            var data0 = mat.Data; var data1 = vec.Data;
            int my0 = (int)mat.DY, d1 = (int)vec.D;
            int mf1 = (int)vec.FirstIndex;
            int ds0 = (int)mat.DSX, d0 = (int)mat.DX;
            for (int ri = 0, ye = (int)mat.FirstIndex + (int)mat.DSY, f0 = (int)mat.FirstIndex, e0 = f0 + ds0;
                 f0 != ye; f0 += my0, e0 += my0, ri++)
            {
                double dot = 0.0d;
                for (int i0 = f0, i1 = mf1; i0 != e0; i0 += d0, i1 += d1)
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
            int i = (int)result.FirstIndex, yj = (int)result.JY, my0 = (int)m0.DY;
            int xs = (int)result.DSX, mf1 = (int)m1.FirstIndex, xj = (int)result.JX, mx1 = (int)m1.DX;
            int ds0 = (int)m0.DSX, d0 = (int)m0.DX, d1 = (int)m1.DY;
            for (int ye = i + (int)result.DSY, f0 = (int)m0.FirstIndex, e0 = f0 + ds0;
                 i != ye; i += yj, f0 += my0, e0 += my0)
                for (int xe = i + xs, f1 = mf1; i != xe; i += xj, f1 += mx1)
                {
                    double dot = 0.0d;
                    for (int i0 = f0, i1 = f1; i0 != e0; i0 += d0, i1 += d1)
                        dot += data0[i0] * data1[i1];
                    data[i] = dot;
                }

            return result;
        }

        public static Matrix<double> Multiply(this Matrix<double> mat, double a)
        {
            var result = new double[mat.SX * mat.SY];

            var data0 = mat.Data;
            int my0 = (int)mat.DY;
            int mf0 = (int)mat.FirstIndex;
            int ds0 = (int)mat.DSX, d0 = (int)mat.DX;
            for (int ri = 0, ye = (int)mat.FirstIndex + (int)mat.DSY, f0 = (int)mat.FirstIndex, e0 = f0 + ds0; 
                f0 != ye; f0 += my0, e0 += my0)
            {
                for (int i0 = f0; i0 != e0; i0 += d0, ri++)
                    result[ri] = data0[i0] * a;
            }
            return Matrix.Create(result, mat.SX, mat.SY);
        }

        public static Matrix<double> Subtract(this Matrix<double> m0, Matrix<double> m1)
        {
            if (m0.Dim != m1.Dim) throw new InvalidOperationException(String.Format("Cannot subtract matrix A={0} and matrix B={1}", m0.Dim, m1.Dim));

            var result = new double[m0.SX * m0.SY];

            var data0 = m0.Data; var data1 = m1.Data;
            int mf0 = (int)m0.FirstIndex, my0 = (int)m0.DY, my1 = (int)m0.DY;
            int ds0 = (int)m0.DSX, d0 = (int)m0.DX, d1 = (int)m1.DX;
            for (int ye = mf0 + (int)m0.DSY, f0 = mf0, f1 = (int)m1.FirstIndex, ri = 0;
                 f0 != ye; f0 += my0, f1 += my1)
                for (int xe = f0 + ds0, i0 = f0, i1 = f1; i0 != xe; i0 += d0, i1 += d1, ri++)
                    result[ri] = data0[i0] - data1[i1];

            return Matrix.Create(result, m0.SX, m0.SY);
        }

        public static Matrix<double> Subtract(this Matrix<double> mat, double a)
        {
            var result = new double[mat.SX * mat.SY];

            var data0 = mat.Data;
            int my0 = (int)mat.DY;
            int mf0 = (int)mat.FirstIndex;
            int ds0 = (int)mat.DSX, d0 = (int)mat.DX;
            for (int ri = 0, ye = (int)mat.FirstIndex + (int)mat.DSY, f0 = (int)mat.FirstIndex, e0 = f0 + ds0;
                f0 != ye; f0 += my0, e0 += my0)
            {
                for (int i0 = f0; i0 != e0; i0 += d0, ri++)
                    result[ri] = data0[i0] - a;
            }
            return Matrix.Create(result, mat.SX, mat.SY);
        }

        public static Matrix<double> Add(this Matrix<double> m0, Matrix<double> m1)
        {
            if (m0.Dim != m1.Dim) throw new InvalidOperationException(String.Format("Cannot add matrix A={0} and matrix B={1}", m0.Dim, m1.Dim));

            var result = new double[m0.SX * m0.SY];

            var data0 = m0.Data; var data1 = m1.Data;
            int mf0 = (int)m0.FirstIndex, my0 = (int)m0.DY, my1 = (int)m0.DY;
            int ds0 = (int)m0.DSX, d0 = (int)m0.DX, d1 = (int)m1.DX;
            for (int ye = mf0 + (int)m0.DSY, f0 = mf0, f1 = (int)m1.FirstIndex, ri = 0;
                 f0 != ye; f0 += my0, f1 += my1)
                for (int xe = f0 + ds0, i0 = f0, i1 = f1; i0 != xe; i0 += d0, i1 += d1, ri++)
                    result[ri] = data0[i0] + data1[i1];

            return Matrix.Create(result, m0.SX, m0.SY);
        }

        public static Matrix<double> Add(this Matrix<double> mat, double a)
        {
            var result = new double[mat.SX * mat.SY];

            var data0 = mat.Data;
            int my0 = (int)mat.DY;
            int mf0 = (int)mat.FirstIndex;
            int ds0 = (int)mat.DSX, d0 = (int)mat.DX;
            for (int ri = 0, ye = (int)mat.FirstIndex + (int)mat.DSY, f0 = (int)mat.FirstIndex, e0 = f0 + ds0;
                f0 != ye; f0 += my0, e0 += my0)
            {
                for (int i0 = f0; i0 != e0; i0 += d0, ri++)
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
}
