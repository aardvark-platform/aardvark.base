using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Aardvark.Base
{
    public static class PixImageExtensions
    {
        //#region Initialization

        //private static bool s_isInitialized = false;

        //internal static void Init()
        //{
        //    if (s_isInitialized) return;
        //    s_isInitialized = true;
        //    TypeInfo.Add(new[]
        //    {
        //        new PixImage<byte>().GetType(),
        //        new PixImage<ushort>().GetType(),
        //        new PixImage<uint>().GetType(),
        //        new PixImage<float>().GetType(),
        //        new PixImage<double>().GetType(),

        //        new PixVolume<byte>().GetType(),
        //        new PixVolume<ushort>().GetType(),
        //        new PixVolume<uint>().GetType(),
        //        new PixVolume<float>().GetType(),
        //        new PixVolume<double>().GetType(),
        //    });
        //}

        //#endregion

        //#region IPixImage2d Extensions

        //public static PixImage<T> ToPixImage<T>(this IPixImage2d image)
        //{
        //    var pixImage = image as PixImage<T>;
        //    if (pixImage != null) return pixImage;
        //    var size = image.Size;
        //    var pixFormat = image.PixFormat;
        //    var data = image.Data as T[];
        //    if (data != null)
        //    {
        //        if (data.GetType().GetElementType() != pixFormat.Type)
        //            throw new ArgumentException("type mismatch in supplied IPixImage2d");
        //        var volume = data.CreateImageVolume(new V3l(size.X, size.Y, Col.ChannelCount(pixFormat.Format)));
        //        return new PixImage<T>(pixFormat.Format, volume);
        //    }
        //    return image.ToPixImage().ToPixImage<T>();
        //}

        //public static PixImage ToPixImage(this IPixImage2d image)
        //{
        //    var pixImage = image as PixImage;
        //    if (pixImage != null) return pixImage;
        //    var size = image.Size;
        //    var pixFormat = image.PixFormat;
        //    if (image.Data.GetType().GetElementType() != pixFormat.Type)
        //        throw new ArgumentException("type mismatch in supplied IPixImage2d");
        //    return PixImage.Create(image.Data, pixFormat.Format, size.X, size.Y);
        //}

        //#endregion

        //#region IPixMipMap2d Extensions

        //public static PixImageMipMap ToPixImageMipMap(this IPixMipMap2d mipmap)
        //{
        //    var pixImageMipMap = mipmap as PixImageMipMap;
        //    if (pixImageMipMap != null) return pixImageMipMap;
        //    var count = mipmap.LevelCount;
        //    var pixImageArray = new PixImage[count].SetByIndex(i => mipmap[i].ToPixImage());
        //    return new PixImageMipMap(pixImageArray);
        //}

        //#endregion

        #region Black and White Conversions

        public static Matrix<byte> ToBlackAndWhiteMatrix(this PixImage<byte> pixImage, int threshold)
        {
            Requires.That(pixImage.ChannelCount == 1);
            return pixImage.GetChannel(Col.Channel.Gray).CopyWindow<byte>(b => (byte)(b < threshold ? 0 : 255));
        }

        public static Matrix<byte> ToBlackAndWhiteMatrix(this PixImage<byte> pixImage)
        {
            return pixImage.ToBlackAndWhiteMatrix(128);
        }

        public static PixImage<byte> ToBlackAndWhitePixImage(this PixImage<byte> pixImage, int threshold)
        {
            return new PixImage<byte>(Col.Format.BW, pixImage.ToBlackAndWhiteMatrix(threshold));
        }

        public static PixImage<byte> ToBlackAndWhitePixImage(this PixImage<byte> pixImage)
        {
            return new PixImage<byte>(Col.Format.BW, pixImage.ToBlackAndWhiteMatrix());
        }

        #endregion

        #region Grayscale Conversions

        public static Matrix<byte> ToGrayscaleMatrix(this PixImage<byte> pixImage)
        {
            if (pixImage.ChannelCount == 1L)
            {
                if (pixImage.Format == Col.Format.BW || pixImage.Format == Col.Format.Gray)
                    return pixImage.Volume.AsMatrixWindow();
            }
            return pixImage.GetMatrix<C3b>().CopyWindow<byte>(Col.GrayByteFromC3b);
        }

        public static PixImage<byte> ToGrayscalePixImage(this PixImage<byte> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray)
                return pixImage;
            return new PixImage<byte>(pixImage.ToGrayscaleMatrix());
        }

        public static Matrix<ushort> ToGrayscaleMatrix(this PixImage<ushort> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray)
                return pixImage.Volume.AsMatrixWindow();
            return pixImage.GetMatrix<C3us>().CopyWindow<ushort>(Col.GrayUShortFromC3us);
        }

        public static PixImage<ushort> ToGrayscalePixImage(this PixImage<ushort> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray)
                return pixImage;
            return new PixImage<ushort>(pixImage.ToGrayscaleMatrix());
        }

        public static Matrix<float> ToGrayscaleMatrix(this PixImage<float> pixImage)
        {
            if (pixImage.ChannelCount == 1L)
            {
                if (pixImage.Format == Col.Format.BW || pixImage.Format == Col.Format.Gray)
                    return pixImage.Volume.AsMatrixWindow();
            }
            return pixImage.GetMatrix<C3f>().CopyWindow<float>(Col.GrayFloatFromC3f);
        }

        public static PixImage<float> ToGrayscalePixImage(this PixImage<float> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray)
                return pixImage;
            return new PixImage<float>(pixImage.ToGrayscaleMatrix());
        }

        public static Matrix<float> ToClampedGrayscaleMatrix(this PixImage<float> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray)
                return pixImage.Volume.AsMatrixWindow();
            return pixImage.GetMatrix<C3f>().CopyWindow<float>(Col.GrayFloatClampedFromC3f);
        }

        public static PixImage<float> ToClampedGrayscalePixImage(this PixImage<float> pixImage)
        {
            if (pixImage.ChannelCount == 1L && pixImage.Format == Col.Format.Gray)
                return pixImage;
            return new PixImage<float>(pixImage.ToClampedGrayscaleMatrix());
        }

        #endregion

        #region Inversion

        public static PixImage<byte> Inverted(this PixImage<byte> pixImage)
        {
            return new PixImage<byte>(pixImage.Volume.CopyImageWindow(b => (byte)(255 - b)));
        }

        public static PixImage<ushort> Inverted(this PixImage<ushort> pixImage)
        {
            return new PixImage<ushort>(pixImage.Volume.CopyImageWindow(us => (ushort)(65535 - us)));
        }

        public static PixImage<uint> Inverted(this PixImage<uint> pixImage)
        {
            return new PixImage<uint>(pixImage.Volume.CopyImageWindow(ui => (uint)(0xffffffffu - ui)));
        }

        public static PixImage<float> Inverted(this PixImage<float> pixImage)
        {
            return new PixImage<float>(pixImage.Volume.CopyImageWindow(f => (float)(1.0f - f)));
        }

        public static PixImage<double> Inverted(this PixImage<double> pixImage)
        {
            return new PixImage<double>(pixImage.Volume.CopyImageWindow(d => 1.0 - d));
        }

        /// <summary>
        /// In-place invert. No Copy is created.
        /// </summary>
        /// <param name="pixImage">Returns this pixImage.</param>
        /// <returns></returns>
        public static PixImage<byte> Invert(this PixImage<byte> pixImage)
        {
            pixImage.Volume.Apply(b => (byte)(255 - b));
            return pixImage;
        }

        /// <summary>
        /// In-place invert. No Copy is created.
        /// </summary>
        /// <param name="pixImage">Returns this pixImage.</param>
        /// <returns></returns>
        public static PixImage<ushort> Invert(this PixImage<ushort> pixImage)
        {
            pixImage.Volume.Apply(us => (ushort)(65535 - us));
            return pixImage;
        }

        /// <summary>
        /// In-place invert. No Copy is created.
        /// </summary>
        /// <param name="pixImage">Returns this pixImage.</param>
        /// <returns></returns>
        public static PixImage<uint> Invert(this PixImage<uint> pixImage)
        {
            pixImage.Volume.Apply(ui => (uint)(0xffffffffu - ui));
            return pixImage;
        }

        /// <summary>
        /// In-place invert. No Copy is created.
        /// </summary>
        /// <param name="pixImage">Returns this pixImage.</param>
        /// <returns></returns>
        public static PixImage<float> Invert(this PixImage<float> pixImage)
        {
            pixImage.Volume.Apply(f => (float)(1.0f - f));
            return pixImage;
        }

        /// <summary>
        /// In-place invert. No Copy is created.
        /// </summary>
        /// <param name="pixImage">Returns this pixImage.</param>
        /// <returns></returns>
        public static PixImage<double> Invert(this PixImage<double> pixImage)
        {
            pixImage.Volume.Apply(d => 1.0 - d);
            return pixImage;
        }

        #endregion

        //#region Gradients on PixImage

        ///// <summary>
        ///// Wrapper function for gradient-computation on images.
        ///// </summary>
        //public static Pair<PixImage<float>> ComputeGradients(
        //        this PixImage<float> src, EdgeFilter filterType)
        //{
        //    return src.Volume.ComputeGradients(filterType).Copy(iv => iv.ToPixImage());
        //}

        ///// <summary>
        ///// Compute gradient magnitudes from x and y gradients.
        ///// </summary>
        ///// <param name="gradXY">E0 contains x gradients, E1 contains y gradients</param>
        //public static PixImage<float> GradientMagnitude(
        //        this Pair<PixImage<float>> gradXY)
        //{
        //    return gradXY.Copy(pi => pi.Volume).GradientMagnitude().ToPixImage();
        //}

        //#endregion
    }
}
