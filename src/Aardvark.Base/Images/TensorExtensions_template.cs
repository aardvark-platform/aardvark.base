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
        //#     Tup.Create("byte", "C4b", "Byte", "C4f"),
        //#     Tup.Create("byte", "C3b", "Byte", "C3f"),
        //#     Tup.Create("ushort", "C4us", "UShort", "C4f"),
        //#     Tup.Create("ushort", "C3us", "UShort", "C3f"),
        //#     Tup.Create("float", "C3f", "", "C3f"),
        //#     Tup.Create("float", "C4f", "", "C4f"),
        //# };
        //# intConfigs.ForEach((dt, ct, dtn, fct) => {
        //#     var clamp = dtn != "";
        //#     var rfct = dtn != "" ? "Raw" + fct : "";
        public static void SetScaledHermite(this Matrix<__dt__, __ct__> targetMat, Matrix<__dt__, __ct__> sourceMat,
                                            double par = -0.5)
        {
            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledCubic(this Matrix<__dt__, __ct__> targetMat, Matrix<__dt__, __ct__> sourceMat,
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
                                        if (clamp) { */.Copy(Col.__dtn__From__dtn__InFloatClamped)/*# } */);
        }

        public static void SetScaledLanczos(this Matrix<__dt__, __ct__> targetMat, Matrix<__dt__, __ct__> sourceMat)
        {
            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();

            double scaleX = scale.X, shiftX = 0.5 * scale.X - 0.5;
            double scaleY = scale.Y, shiftY = 0.5 * scale.Y - 0.5;

            targetMat.ForeachIndex((x, y, i) =>
                targetMat[i] = sourceMat.Sample36(x * scaleX + shiftX, y * scaleY + shiftY,
                                                  Fun.Lanczos3f, Fun.Lanczos3f,
                                                  __ct__.LinCom__rfct__, __fct__.LinCom,
                                                  Tensor.Index6SamplesClamped, Tensor.Index6SamplesClamped)/*#
                                        if (clamp) { */.Copy(Col.__dtn__From__dtn__InFloatClamped)/*# } */);
        }

        //# }); // configs
        #endregion
    }
}