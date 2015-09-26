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

        //# var intConfigs = new []
        //# {
        //#     Tup.Create("byte",      "Byte",     "Fun",  "Fun"),
        //#     Tup.Create("ushort",    "UShort",   "Fun",  "Fun"),
        //#     Tup.Create("float",     "",         "Fun",  "Fun"),
        //#     Tup.Create("byte",      "Byte",     "C3b",  "C3f"),
        //#     Tup.Create("ushort",    "UShort",   "C3us", "C3f"),
        //#     Tup.Create("float",     "",         "C3f",  "C3f"),
        //#     Tup.Create("byte",      "Byte",     "C4b",  "C4f"),
        //#     Tup.Create("ushort",    "UShort",   "C4us", "C4f"),
        //#     Tup.Create("float",     "",         "C4f",  "C4f"),
        //# };
        //# intConfigs.ForEach((dt, dtn, ct, fct) => {
        //#     var clamp = dtn != "" && ct == "Fun";
        //#     var clampMap = dtn != "" && ct != "Fun";
        //#     var rfct = dtn == "" ? "" : ct == "Fun" ? "RawF" : "Raw" + fct ;
        //#     var dtct = ct == "Fun" ? dt : dt + ", " + ct;
        /// <summary>
        /// Use Cubic Spline interpolation to scale the source matrix into the target matrix.
        /// The supplied parameter selects the spline to use. The default value of -0.5 generates
        /// Hermite Splines. If you call this repeatedly with the same selection parameter,
        /// build the cubic weighting function with 'Fun.CreateCubicTup4f(par)' and use the
        /// result as a paramter to the function call.
        /// </summary>
        public static void SetScaledCubic(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat,
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
        public static void SetScaledCubic(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();

            double scaleX = scale.X, shiftX = 0.5 * scale.X - 0.5;
            double scaleY = scale.Y, shiftY = 0.5 * scale.Y - 0.5;

            targetMat.ForeachIndex((x, y, i) =>
                targetMat[i] = sourceMat.Sample16(x * scaleX + shiftX, y * scaleY + shiftY,
                                                  interpolator, interpolator,
                                                  __ct__.LinCom__rfct__, __fct__.LinCom,
                                                  Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped)/*#
                                        if (clamp) { */
                                        .Col__dtn__InFloatTo__dtn__Clamped()/*# } else if (clampMap) { */
                                        .Copy(Col.__dtn__From__dtn__InFloatClamped)/*# } */);
        }

        /// <summary>
        /// Use Lanczos Interpoation to scale the source matrix into the target matrix.
        /// </summary>
        public static void SetScaledLanczos(this Matrix<__dtct__> targetMat, Matrix<__dtct__> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();

            double scaleX = scale.X, shiftX = 0.5 * scale.X - 0.5;
            double scaleY = scale.Y, shiftY = 0.5 * scale.Y - 0.5;

            targetMat.ForeachIndex((x, y, i) =>
                targetMat[i] = sourceMat.Sample36(x * scaleX + shiftX, y * scaleY + shiftY,
                                                  Fun.Lanczos3f, Fun.Lanczos3f,
                                                  __ct__.LinCom__rfct__, __fct__.LinCom,
                                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped)/*#
                                        if (clamp) { */
                                        .Col__dtn__InFloatTo__dtn__Clamped()/*# } else if (clampMap) { */
                                        .Copy(Col.__dtn__From__dtn__InFloatClamped)/*# } */);
        }

        //# }); // configs
        #endregion
    }
}