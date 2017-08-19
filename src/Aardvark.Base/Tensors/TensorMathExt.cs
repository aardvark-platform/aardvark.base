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

        #region Vector Extensions

        public static double DotProduct(this Vector<double> v0, Vector<double> v1)
        {
            double result = 0.0;
            double[] a0 = v0.Data, a1 = v1.Data;
            for (long i0 = v0.Origin, i1 = v1.Origin, e0 = i0 + v0.DSX, d0 = v0.D, d1 = v1.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result += a0[i0] * a1[i1];
            return result;
        }

        public static double NormSquared(this Vector<double> v)
        {
            double result = 0.0;
            double[] a = v.Data;
            for (long i = v.Origin, e = i + v.DSX, d = v.D; i != e; i += d)
                result += a[i] * a[i];
            return result;
        }

        public static Vector<double> Multiply(this Vector<double> a, Vector<double> b)
        {
            if (a.Dim != b.Dim) throw new InvalidOperationException("Cannot multiply vectors with different lengths!");

            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data, a1 = b.Data;
            for (long i0 = a.Origin, i1 = b.Origin, e0 = i0 + a.DSX, d0 = a.D, d1 = b.D;
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
            for (long i0 = a.Origin, e0 = i0 + a.DSX, d0 = a.D; i0 != e0; i0 += d0)
                for (long i1 = b.Origin, e1 = i1 + b.DSX, d1 = b.D; i1 != e1; i1 += d1)
                    result[ri++] = a0[i0] * a1[i1];
            return Matrix.Create(result, b.Dim, a.Dim);
        }

        public static Vector<double> Multiply(this Vector<double> vec, double s)
        {
            var result = new double[vec.Dim];
            var ri = 0;
            double[] a0 = vec.Data;
            for (long i0 = vec.Origin, e0 = i0 + vec.DSX, d0 = vec.D; i0 != e0; i0 += d0)
                result[ri++] = a0[i0] * s;
            return Vector.Create(result);
        }

        public static Vector<double> Subtract(this Vector<double> a, Vector<double> b)
        {
            if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));
            
            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data, a1 = b.Data;
            for (long i0 = a.Origin, i1 = b.Origin, e0 = i0 + a.DSX, d0 = a.D, d1 = b.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result[ri++] = a0[i0] - a1[i1];
            return Vector.Create(result);
        }

        public static Vector<double> Subtract(this Vector<double> a, double b)
        {
            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data;
            for (long i0 = a.Origin, e0 = i0 + a.DSX, d0 = a.D; i0 != e0; i0 += d0)
                result[ri++] = a0[i0] - b;
            return Vector.Create(result);
        }

        public static Vector<double> Add(this Vector<double> a, Vector<double> b)
        {
            if (a.Dim != b.Dim) throw new InvalidOperationException(String.Format("Cannot subtract vectors with length {0} and {1}", a.Dim, b.Dim));

            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data, a1 = b.Data;
            for (long i0 = a.Origin, i1 = b.Origin, e0 = i0 + a.DSX, d0 = a.D, d1 = b.D;
                 i0 != e0; i0 += d0, i1 += d1)
                result[ri++] = a0[i0] + a1[i1];
            return Vector.Create(result);
        }

        public static Vector<double> Add(this Vector<double> a, double b)
        {
            var result = new double[a.Dim];
            var ri = 0;
            double[] a0 = a.Data;
            for (long i0 = a.Origin, e0 = i0 + a.DSX, d0 = a.D; i0 != e0; i0 += d0)
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
    }

    public static class Matrix
    {
        /// <summary>
        /// Matrix constructor from a given array that does not require to write generic type.
        /// </summary>
        public static Matrix<T> Create<T>(T[] array, int sx, int sy)
        {
            return new Matrix<T>(array, sx, sy);
        }

        /// <summary>
        /// Matrix constructor from a given array that does not require to write generic type.
        /// </summary>
        public static Matrix<T> Create<T>(T[] array, long sx, long sy)
        {
            return new Matrix<T>(array, sx, sy);
        }

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
            long my0 = mat.DY, d1 = vec.D;
            long mf1 = vec.FirstIndex;
            long ds0 = mat.DSX, d0 = mat.DX;
            for (long ri = 0, ye = mat.FirstIndex + mat.DSY, f0 = mat.FirstIndex, e0 = f0 + ds0;
                 f0 != ye; f0 += my0, e0 += my0, ri++)
            {
                double dot = 0.0;
                for (long i0 = f0, i1 = vec.FirstIndex; i0 != e0; i0 += d0, i1 += d1)
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
            long mf0 = mat.FirstIndex;
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
            long mf0 = m0.FirstIndex, my0 = m0.DY, my1 = m0.DY;
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
            long mf0 = mat.FirstIndex;
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
            long mf0 = m0.FirstIndex, my0 = m0.DY, my1 = m0.DY;
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
            long mf0 = mat.FirstIndex;
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
}
