using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aardvark.Base;

namespace Aardvark.Base
{
    /// <summary>
    /// Wrappers for the best (fastest) available implementation of the respective tensor operation.
    /// </summary>
    public static partial class TensorExtensions
    {

        #region Image Scaling

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte> targetMat, Matrix<byte> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix
        /// using the supplied cubic interpolator.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte> targetMat, Matrix<byte> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Fun.LinComRawF, Fun.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  Col.ByteFromByteInFloatClamped);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<byte> targetMat, Matrix<byte> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  Fun.Lanczos3f, Fun.Lanczos3f, Fun.LinComRawF, Fun.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  Col.ByteFromByteInFloatClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix
        /// using the supplied cubic interpolator.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Fun.LinComRawF, Fun.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  Col.UShortFromUShortInFloatClamped);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<ushort> targetMat, Matrix<ushort> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  Fun.Lanczos3f, Fun.Lanczos3f, Fun.LinComRawF, Fun.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  Col.UShortFromUShortInFloatClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float> targetMat, Matrix<float> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix
        /// using the supplied cubic interpolator.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float> targetMat, Matrix<float> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, Fun.LinCom, Fun.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<float> targetMat, Matrix<float> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  Fun.Lanczos3f, Fun.Lanczos3f, Fun.LinCom, Fun.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix
        /// using the supplied cubic interpolator.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C3b.LinComRawC3f, C3f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Copy(Col.ByteFromByteInFloatClamped));
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<byte, C3b> targetMat, Matrix<byte, C3b> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  Fun.Lanczos3f, Fun.Lanczos3f, C3b.LinComRawC3f, C3f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Copy(Col.ByteFromByteInFloatClamped));
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix
        /// using the supplied cubic interpolator.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C3us.LinComRawC3f, C3f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Copy(Col.UShortFromUShortInFloatClamped));
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<ushort, C3us> targetMat, Matrix<ushort, C3us> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  Fun.Lanczos3f, Fun.Lanczos3f, C3us.LinComRawC3f, C3f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Copy(Col.UShortFromUShortInFloatClamped));
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix
        /// using the supplied cubic interpolator.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C3f.LinCom, C3f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<float, C3f> targetMat, Matrix<float, C3f> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  Fun.Lanczos3f, Fun.Lanczos3f, C3f.LinCom, C3f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix
        /// using the supplied cubic interpolator.
        /// </summary>
        public static void SetScaledCubic(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C4b.LinComRawC4f, C4f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Copy(Col.ByteFromByteInFloatClamped));
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  Fun.Lanczos3f, Fun.Lanczos3f, C4b.LinComRawC4f, C4f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Copy(Col.ByteFromByteInFloatClamped));
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix
        /// using the supplied cubic interpolator.
        /// </summary>
        public static void SetScaledCubic(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C4us.LinComRawC4f, C4f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped,
                                  col => col.Copy(Col.UShortFromUShortInFloatClamped));
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<ushort, C4us> targetMat, Matrix<ushort, C4us> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  Fun.Lanczos3f, Fun.Lanczos3f, C4us.LinComRawC4f, C4f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped,
                                  col => col.Copy(Col.UShortFromUShortInFloatClamped));
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix
        /// using the supplied cubic interpolator.
        /// </summary>
        public static void SetScaledCubic(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled16(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                 interpolator, interpolator, C4f.LinCom, C4f.LinCom,
                                 Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<float, C4f> targetMat, Matrix<float, C4f> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();
            targetMat.SetScaled36(sourceMat, scale.X, scale.Y, 0.5 * scale.X - 0.5, 0.5 * scale.Y - 0.5,
                                  Fun.Lanczos3f, Fun.Lanczos3f, C4f.LinCom, C4f.LinCom,
                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped);
        }

        #endregion
    }
}