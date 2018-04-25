using System;

namespace Aardvark.Base
{
    public static class PixImageExtensions
    {
        #region IPixImage2d Extensions

        public static PixImage<T> ToPixImage<T>(this IPixImage2d image)
        {
            if (image is PixImage<T> pixImage) return pixImage;
            var size = image.Size;
            var pixFormat = image.PixFormat;
            if (image.Data is T[] data)
            {
                if (data.GetType().GetElementType() != pixFormat.Type)
                    throw new ArgumentException("type mismatch in supplied IPixImage2d");
                var volume = data.CreateImageVolume(new V3l(size.X, size.Y, Col.ChannelCount(pixFormat.Format)));
                return new PixImage<T>(pixFormat.Format, volume);
            }
            return image.ToPixImage().ToPixImage<T>();
        }

        public static PixImage ToPixImage(this IPixImage2d image)
        {
            if (image is PixImage pixImage) return pixImage;
            var size = image.Size;
            var pixFormat = image.PixFormat;
            if (image.Data.GetType().GetElementType() != pixFormat.Type)
                throw new ArgumentException("type mismatch in supplied IPixImage2d");
            return PixImage.Create(image.Data, pixFormat.Format, size.X, size.Y);
        }

        #endregion

        #region IPixMipMap2d Extensions

        public static PixImageMipMap ToPixImageMipMap(this IPixMipMap2d mipmap)
        {
            if (mipmap is PixImageMipMap pixImageMipMap) return pixImageMipMap;
            var count = mipmap.LevelCount;
            var pixImageArray = new PixImage[count].SetByIndex(i => mipmap[i].ToPixImage());
            return new PixImageMipMap(pixImageArray);
        }

        #endregion

        #region Black and White Conversions

        public static Matrix<byte> ToBlackAndWhiteMatrix(this PixImage<byte> pixImage, int threshold)
        {
            if (pixImage.ChannelCount != 1) throw new ArgumentOutOfRangeException(nameof(pixImage.ChannelCount));
            return pixImage.GetChannel(Col.Channel.Gray).MapWindow<byte>(b => (byte)(b < threshold ? 0 : 255));
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
            return pixImage.GetMatrix<C3b>().MapWindow<byte>(Col.GrayByteFromC3b);
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
            return pixImage.GetMatrix<C3us>().MapWindow<ushort>(Col.GrayUShortFromC3us);
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
            return pixImage.GetMatrix<C3f>().MapWindow<float>(Col.GrayFloatFromC3f);
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
            return pixImage.GetMatrix<C3f>().MapWindow<float>(Col.GrayFloatClampedFromC3f);
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
            return new PixImage<byte>(pixImage.Volume.MapToImageWindow(b => (byte)(255 - b)));
        }

        public static PixImage<ushort> Inverted(this PixImage<ushort> pixImage)
        {
            return new PixImage<ushort>(pixImage.Volume.MapToImageWindow(us => (ushort)(65535 - us)));
        }

        public static PixImage<uint> Inverted(this PixImage<uint> pixImage)
        {
            return new PixImage<uint>(pixImage.Volume.MapToImageWindow(ui => (uint)(0xffffffffu - ui)));
        }

        public static PixImage<float> Inverted(this PixImage<float> pixImage)
        {
            return new PixImage<float>(pixImage.Volume.MapToImageWindow(f => (float)(1.0f - f)));
        }

        public static PixImage<double> Inverted(this PixImage<double> pixImage)
        {
            return new PixImage<double>(pixImage.Volume.MapToImageWindow(d => 1.0 - d));
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
    }
}
