using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Aardvark.Base
{
    public static class FilterKernelFun
    {
        #region Gaussian
        /// <summary>
        /// Return a function that computes the gauss function with mu = 0
        /// and the specified sigma.
        /// </summary>
        public static Func<float, float> Gaussian(float sigma)
        {
            double s2 = sigma * sigma;
            double a = 1.0 / (Constant.SqrtPiTimesTwo * sigma);
            double b = -1.0 / (2.0 * s2);
            return (float x) => (float)(a * Fun.Exp(b * (x * x)));
        }

        /// <summary>
        /// Return a function that computes the derivative of the gauss
        /// function with mu = 0 and specified sigma.
        /// </summary>
        public static Func<float, float> GaussianDx(float sigma)
        {
            double s2 = sigma * sigma;
            double a = -1.0 / (Constant.SqrtPiTimesTwo * s2 * sigma);
            double b = -1.0 / (2.0 * s2);
            return (float x) => (float)(a * x * Fun.Exp(b * (x * x)));
        }

        /// <summary>
        /// Return a function that computes the two-dimensional gauss
        /// function with mu = (0, 0) and sigma = DiagonalMatrix(sigma).
        /// </summary>
        public static Func<float, float, float> Gaussian2(float sigma)
        {
            double s2 = sigma * sigma;
            double a = 1.0 / (Constant.PiTimesTwo * s2);
            double b = -1.0 / (2.0 * s2);
            return (float x, float y) =>
                        (float)(a * Fun.Exp(b * (x * x + y * y)));
        }

        /// <summary>
        /// Return a function that computes the partial derivative in X of
        /// the two-dimensional gauss function with mu = (0, 0) and
        /// sigma = DiagonalMatrix(sigma).
        /// </summary>
        public static Func<float, float, float> Gaussian2Dx(float sigma)
        {
            double s2 = sigma * sigma;
            double a = -1.0 / (Constant.PiTimesTwo * s2 * s2);
            double b = -1.0 / (2.0 * s2);
            return (float x, float y) =>
                        (float)(a * x * Fun.Exp(b * (x * x + y * y)));
        }

        /// <summary>
        /// Return a function that computes the partial derivative in Y of
        /// the two-dimensional gauss function with mu = (0, 0) and
        /// sigma = DiagonalMatrix(sigma).
        /// </summary>
        public static Func<float, float, float> Gaussian2Dy(float sigma)
        {
            double s2 = sigma * sigma;
            double a = -1.0 / (Constant.PiTimesTwo * s2 * s2);
            double b = -1.0 / (2.0 * s2);
            return (float x, float y) =>
                        (float)(a * y * Fun.Exp(b * (x * x + y * y)));
        }

        /// <summary>
        /// Return a function that computes the difference of two
        /// one-dimensional gauss functions (Gauss(sigma1)-Gauss(sigma2)).
        /// </summary>
        public static Func<float, float> DoG(float sigma1, float sigma2)
        {
            var g1 = Gaussian(sigma1);
            var g2 = Gaussian(sigma2);
            return (float x) => g1(x) - g2(x);
        }

        /// <summary>
        /// Return a function that computes the difference of two
        /// two-dimensional gauss functions (Gauss(sigma1)-Gauss(sigma2)).
        /// </summary>
        public static Func<float, float, float> DoG2(float sigma1, float sigma2)
        {
            var g1 = Gaussian2(sigma1);
            var g2 = Gaussian2(sigma2);
            return (float x, float y) => g1(x, y) - g2(x, y);
        }

        /// <summary>
        /// Return a function that computes the one-dimensional
        /// Laplacian of Gaussian function for a given sigma.
        /// Also known as Marr-Hildreth-Operator or Sombrero-Filter.
        /// </summary>
        public static Func<float, float> LoG(float sigma)
        {
            float s2 = sigma * sigma;
            float s4 = s2 * s2;
            return x =>
            {
                double a = (x * x) / (2 * s2);
                return (float)((-1 / (System.Math.PI * s4)) * System.Math.Exp(-a) * (1 - a));
            };
        }

        /// <summary>
        /// Return a function that computes the two-dimensional
        /// Laplacian of Gaussian function for a given sigma.
        /// Also known as Marr-Hildreth-Operator or Sombrero-Filter.
        /// </summary>
        public static Func<float, float, float> LoG2(float sigma)
        {
            float s2 = sigma * sigma;
            float s4 = s2 * s2;
            return (x, y) =>
            {
                double a = (x * x + y * y) / (2 * s2);
                return (float)((-1 / (System.Math.PI * s4)) * System.Math.Exp(-a) * (1 - a));
            };
        }

        #endregion Gaussian
    }

    /// <summary>
    /// Convolution filter kernels.
    /// </summary>
    public static class FilterKernel
    {

        #region Normalization

        /// <summary>
        /// Normalizes the supplied matrix so that the sum of all entries sums to 1. MODIFIES SOURCE!
        /// </summary>
        /// <returns>this</returns>
        public static Matrix<float> NormalizeToSum1(this Matrix<float> matrixWillBeModified)
        {
            var sum = matrixWillBeModified.Norm(x => x, 0.0, (s, x) => s + (double)x);
            if (sum != 0.0)
            {
                double scale = 1.0 / sum;
                matrixWillBeModified.Apply(x => (float)(scale * x));
            }
            return matrixWillBeModified;
        }

        /// <summary>
        /// Normalizes the supplied matrix so that the sum of all entries sums to 1. MODIFIES SOURCE!
        /// </summary>
        /// <returns>this</returns>
        public static Matrix<double> NormalizeToSum1(this Matrix<double> matrixWillBeModified)
        {

            var sum = matrixWillBeModified.Norm(x => x, KahanSum.Zero, (s, x) => s + x);
            if (sum.Value != 0.0)
            {
                double scale = 1.0 / sum.Value;
                matrixWillBeModified.Apply(x => scale * x);
            }
            return matrixWillBeModified;
        }

        /// <summary>
        /// Normalizes the supplied vector so that the sum of all entries sums to 1. MODIFIES SOURCE!
        /// </summary>
        /// <returns>this</returns>
        public static Vector<float> NormalizeToSum1(this Vector<float> vectorWillBeModified)
        {
            var sum = vectorWillBeModified.Norm(x => x, 0.0, (s, x) => s + (double)x);
            if (sum != 0.0)
            {
                double scale = 1.0 / sum;
                vectorWillBeModified.Apply(x => (float)(scale * x));
            }
            return vectorWillBeModified;
        }

        /// <summary>
        /// Normalizes the supplied vector so that the sum of all entries sums to 1. MODIFIES SOURCE!
        /// </summary>
        /// <returns>this</returns>
        public static Vector<double> NormalizeToSum1(this Vector<double> vectorWillBeModified)
        {

            var sum = vectorWillBeModified.Norm(x => x, KahanSum.Zero, (s, x) => s + x);
            if (sum.Value != 0.0)
            {
                double scale = 1.0 / sum.Value;
                vectorWillBeModified.Apply(x => scale * x);
            }
            return vectorWillBeModified;
        }

        #endregion

        /// <summary>
        /// Create a 1D filter kernel Vector populated by filterKernelFun,
        /// with size S = 2 * radius + 1 and anchor in the center at index 0.
        /// Functions to use as filterKernelFun are available in FilterKernelFun.
        /// </summary>
        /// <param name="filterKernelFun">Function is evaluated at integer coordinates from -radius to +radius.</param>
        public static Vector<float> Create1DKernel(
                long radius, Func<float, float> filterKernelFun)
        {
            var kernel = new Vector<float>(2 * radius + 1) { F = -radius };
            kernel.SetByCoord(v => filterKernelFun((float)v));

            return kernel;
        }

        /// <summary>
        /// Create a 2D filter kernel matrix populated in accordance with
        /// filterKernelFun, with size (SX, SY) = (2 * radius + 1,
        /// 2 * radius + 1) and anchor in the center at index (0, 0).
        /// Functions to use as filterKernelFun are available in FilterKernelFun.
        /// </summary>
        /// <param name="filterKernelFun">Function is evaluated at integer coordinates from (-radius,-radius) to (+radius,+radius).</param>
        public static Matrix<float> Create2DKernel(
                long radius, Func<float, float, float> filterKernelFun)
        {
            var kernel = new Matrix<float>(2 * radius + 1, 2 * radius + 1) { FX = -radius, FY = -radius };
            kernel.SetByCoord((long x, long y) => filterKernelFun((float)x, (float)y));

            return kernel;
        }

        /// <summary>
        /// Create a 2D kernel from two 1D kernels via outer product.
        /// 
        /// Swapping the two 1D kernel parameters gives the transposed 2D kernel.
        /// </summary>
        public static Matrix<float> Create2DKernel(
                Vector<float> kernelX, Vector<float> kernelY)
        {
            return Matrix<float>.CreateOuterProduct(kernelX, kernelY, (x, y) => x * y);
        }

        /// <summary>
        /// Create a 2D kernel Matrix corresponding to the provided 1D kernel
        /// vector, with 1 row and kernel.Size columns.
        /// </summary>
        public static Matrix<float> To2DKernelX(this Vector<float> kernel)
        {
            return new Matrix<float>(kernel.Data, kernel.Origin,
                        new V2l(kernel.S, 1L), new V2l(kernel.D, kernel.DSX), new V2l(kernel.F, 0));
        }

        /// <summary>
        /// Create a 2D kernel Matrix corresponding to the provided 1D kernel
        /// vector, with kernel.Size rows and 1 column.
        /// </summary>
        public static Matrix<float> To2DKernelY(this Vector<float> kernel)
        {
            return new Matrix<float>(kernel.Data, kernel.Origin,
                        new V2l(1L, kernel.S), new V2l(kernel.DSX, kernel.D), new V2l(0, kernel.F));
        }


        /// <summary>
        /// Creates a Gaussian FilterKernel with given sigma.
        /// Radius is set to 3 * sigma.
        /// </summary>
        public static Matrix<float> Gaussian2(float sigma)
        {
            return Gaussian2(sigma, (int)Fun.Ceiling(3 * sigma));
        }

        /// <summary>
        /// Creates a Gaussian FilterKernel with given sigma and radius.
        /// </summary>
        public static Matrix<float> Gaussian2(float sigma, int radius)
        {
            return Create2DKernel(radius, FilterKernelFun.Gaussian2(sigma)).NormalizeToSum1();
        }

        /// <summary>
        /// Creates a Gaussian FilterKernel with given sigma.
        /// Radius is set to 3 * sigma.
        /// </summary>
        public static Vector<float> Gaussian1(float sigma)
        {
            return Gaussian1(sigma, (int)Fun.Ceiling(3 * sigma));
        }

        /// <summary>
        /// Creates a Gaussian FilterKernel with given sigma and radius.
        /// </summary>
        public static Vector<float> Gaussian1(float sigma, int radius)
        {
            return Create1DKernel(radius, FilterKernelFun.Gaussian(sigma)).NormalizeToSum1();
        }

    }
}
