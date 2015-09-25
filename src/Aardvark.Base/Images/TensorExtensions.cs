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

        #region Cubic Image Scaling

        public static void SetScaledHermite(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat,
                                            double par = -0.5)
        {
            var hermiteSpline = Fun.CreateCubicTup4f(par);
            targetMat.SetScaledCubic(sourceMat, hermiteSpline);
        }

        public static void SetScaledCubic(this Matrix<byte, C4b> targetMat, Matrix<byte, C4b> sourceMat,
                                          Func<double, Tup4<float>> interpolator)
        {

            // create the cubic weighting function. Parameter a=-0.5 results in the cubic Hermite spline.
            var hermiteSpline = Fun.CreateCubicTup4f(-0.5);

            var scale = sourceMat.Size.ToV2d() / targetMat.Size.ToV2d();

            double scaleX = scale.X, shiftX = 0.5 * scale.X - 0.5;
            double scaleY = scale.Y, shiftY = 0.5 * scale.Y - 0.5;

            targetMat.ForeachIndex((x, y, i) =>
            {
                // Note: LinComRawC4f in x direction results in a byte color (range 0-255) stored
                // in a C4f. The second C4f.LinCom for the y direction does not perform any additional
                // scaling, thus we need to copy the "ByteInFloat" color back to a byte color at the
                // end (this perfoms clamping). Tensor.Tensor.Index6SamplesClamped clamps to the border
                // region and allows any double pixel address.
                targetMat[i] = sourceMat.Sample16(x * scaleX + shiftX, y * scaleX + shiftX,
                                               interpolator, interpolator,
                                               C4b.LinComRawC4f, C4f.LinCom,
                                               Tensor.Index4SamplesClamped, Tensor.Index4SamplesClamped)
                                                .Copy(Col.ByteFromByteInFloatClamped);
            });
        }

        #endregion


        #region Conversions (Matrix, Volume) to PixImage<T>

        public static PixImage<T> ToPixImage<T>(this Matrix<T> matrix)
        {
            return matrix.AsVolume().ToPixImage();
        }

        public static PixImage<T> ToPixImage<T>(this Volume<T> volume)
        {
            return new PixImage<T>(volume.ToImage());
        }

        public static PixImage<T> ToPixImage<T>(this Volume<T> volume, Col.Format format)
        {
            var ch = format.ChannelCount();
            if (ch > volume.Size.Z)
                throw new ArgumentException("volume has not enough channels for requested format");
            if (ch < volume.Size.Z)
                volume = volume.SubVolume(V3l.Zero, new V3l(volume.SX, volume.SY, ch)); 
            return new PixImage<T>(format, volume.ToImage());
        }

        public static PixImage<T> ToPixImage<T>(this Matrix<C3b> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Size);
            pixImage.GetMatrix<C3b>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this Matrix<C3us> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Size);
            pixImage.GetMatrix<C3us>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this Matrix<C3f> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Size);
            pixImage.GetMatrix<C3f>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this Matrix<C4b> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Size);
            pixImage.GetMatrix<C4b>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this Matrix<C4us> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Size);
            pixImage.GetMatrix<C4us>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this Matrix<C4f> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Size);
            pixImage.GetMatrix<C4f>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T, TMatrixData>(this Matrix<TMatrixData, C3b> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Size);
            pixImage.GetMatrix<C3b>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T, TMatrixData>(this Matrix<TMatrixData, C3us> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Size);
            pixImage.GetMatrix<C3us>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T, TMatrixData>(this Matrix<TMatrixData, C3f> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Size);
            pixImage.GetMatrix<C3f>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T, TMatrixData>(this Matrix<TMatrixData, C4b> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Size);
            pixImage.GetMatrix<C4b>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T, TMatrixData>(this Matrix<TMatrixData, C4us> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Size);
            pixImage.GetMatrix<C4us>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T, TMatrixData>(this Matrix<TMatrixData, C4f> matrix)
        {
            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Size);
            pixImage.GetMatrix<C4f>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this IMatrix<C3b> matrix)
        {
            if (matrix is Matrix<byte, C3b>)
                return ((Matrix<byte, C3b>)matrix).ToPixImage<T, byte>(); ;

            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Dim);
            pixImage.GetMatrix<C3b>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this IMatrix<C3us> matrix)
        {
            if (matrix is Matrix<ushort, C3us>)
                return ((Matrix<ushort, C3us>)matrix).ToPixImage<T, ushort>(); ;

            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Dim);
            pixImage.GetMatrix<C3us>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this IMatrix<C3f> matrix)
        {
            if (matrix is Matrix<float, C3f>)
                return ((Matrix<float, C3f>)matrix).ToPixImage<T, float>(); ;

            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Dim);
            pixImage.GetMatrix<C3f>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this IMatrix<C4b> matrix)
        {
            if (matrix is Matrix<byte, C4b>)
                return ((Matrix<byte, C4b>)matrix).ToPixImage<T, byte>(); ;

            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Dim);
            pixImage.GetMatrix<C4b>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this IMatrix<C4us> matrix)
        {
            if (matrix is Matrix<ushort, C4us>)
                return ((Matrix<ushort, C4us>)matrix).ToPixImage<T, ushort>(); ;

            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Dim);
            pixImage.GetMatrix<C4us>().Set(matrix);
            return pixImage;
        }

        public static PixImage<T> ToPixImage<T>(this IMatrix<C4f> matrix)
        {
            if (matrix is Matrix<float, C4f>)
                return ((Matrix<float, C4f>)matrix).ToPixImage<T, float>(); ;

            var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Dim);
            pixImage.GetMatrix<C4f>().Set(matrix);
            return pixImage;
        }

        #endregion

        #region Conversions Volume to Volume (byte, ushort, uint, float, double) [CSharp (internal)]

        // All the following conversions are internal only, since we do not
        // know about the channel order at this level. Only PixImages know
        // about channel order.

        internal static Volume<ushort> ToUShortColor(this Volume<byte> volume)
        {
            return volume.CopyImageWindow(Col.UShortFromByte);
        }

        internal static Volume<uint> ToUIntColor(this Volume<byte> volume)
        {
            return volume.CopyImageWindow(Col.UIntFromByte);
        }

        internal static Volume<float> ToFloatColor(this Volume<byte> volume)
        {
            return volume.CopyImageWindow(Col.FloatFromByte);
        }

        internal static Volume<double> ToDoubleColor(this Volume<byte> volume)
        {
            return volume.CopyImageWindow(Col.DoubleFromByte);
        }

        internal static Volume<byte> ToByteColor(this Volume<ushort> volume)
        {
            return volume.CopyImageWindow(Col.ByteFromUShort);
        }

        internal static Volume<uint> ToUIntColor(this Volume<ushort> volume)
        {
            return volume.CopyImageWindow(Col.UIntFromUShort);
        }

        internal static Volume<float> ToFloatColor(this Volume<ushort> volume)
        {
            return volume.CopyImageWindow(Col.FloatFromUShort);
        }

        internal static Volume<double> ToDoubleColor(this Volume<ushort> volume)
        {
            return volume.CopyImageWindow(Col.DoubleFromUShort);
        }

        internal static Volume<byte> ToByteColor(this Volume<uint> volume)
        {
            return volume.CopyImageWindow(Col.ByteFromUInt);
        }

        internal static Volume<ushort> ToUShortColor(this Volume<uint> volume)
        {
            return volume.CopyImageWindow(Col.UShortFromUInt);
        }

        internal static Volume<float> ToFloatColor(this Volume<uint> volume)
        {
            return volume.CopyImageWindow(Col.FloatFromUInt);
        }

        internal static Volume<double> ToDoubleColor(this Volume<uint> volume)
        {
            return volume.CopyImageWindow(Col.DoubleFromUInt);
        }

        internal static Volume<byte> ToByteColor(this Volume<float> volume)
        {
            return volume.CopyImageWindow(Col.ByteFromFloat);
        }

        internal static Volume<ushort> ToUShortColor(this Volume<float> volume)
        {
            return volume.CopyImageWindow(Col.UShortFromFloat);
        }

        internal static Volume<uint> ToUIntColor(this Volume<float> volume)
        {
            return volume.CopyImageWindow(Col.UIntFromFloat);
        }

        internal static Volume<double> ToDoubleColor(this Volume<float> volume)
        {
            return volume.CopyImageWindow(Col.DoubleFromFloat);
        }

        internal static Volume<byte> ToByteColor(this Volume<double> volume)
        {
            return volume.CopyImageWindow(Col.ByteFromDouble);
        }

        internal static Volume<ushort> ToUShortColor(this Volume<double> volume)
        {
            return volume.CopyImageWindow(Col.UShortFromDouble);
        }

        internal static Volume<uint> ToUIntColor(this Volume<double> volume)
        {
            return volume.CopyImageWindow(Col.UIntFromDouble);
        }

        internal static Volume<float> ToFloatColor(this Volume<double> volume)
        {
            return volume.CopyImageWindow(Col.FloatFromDouble);
        }
        
        #endregion

        #region Conversions Tensor4 to Tensor4 (byte, ushort, uint, float, double) [CSharp (internal)]

        // All the following conversions are internal only, since we do not
        // know about the channel order at this level. Only PixVolumes know
        // about channel order.

        internal static Tensor4<ushort> ToUShortColor(this Tensor4<byte> tensor4)
        {
            return tensor4.CopyImageWindow(Col.UShortFromByte);
        }

        internal static Tensor4<uint> ToUIntColor(this Tensor4<byte> tensor4)
        {
            return tensor4.CopyImageWindow(Col.UIntFromByte);
        }

        internal static Tensor4<float> ToFloatColor(this Tensor4<byte> tensor4)
        {
            return tensor4.CopyImageWindow(Col.FloatFromByte);
        }

        internal static Tensor4<double> ToDoubleColor(this Tensor4<byte> tensor4)
        {
            return tensor4.CopyImageWindow(Col.DoubleFromByte);
        }

        internal static Tensor4<byte> ToByteColor(this Tensor4<ushort> tensor4)
        {
            return tensor4.CopyImageWindow(Col.ByteFromUShort);
        }

        internal static Tensor4<uint> ToUIntColor(this Tensor4<ushort> tensor4)
        {
            return tensor4.CopyImageWindow(Col.UIntFromUShort);
        }

        internal static Tensor4<float> ToFloatColor(this Tensor4<ushort> tensor4)
        {
            return tensor4.CopyImageWindow(Col.FloatFromUShort);
        }

        internal static Tensor4<double> ToDoubleColor(this Tensor4<ushort> tensor4)
        {
            return tensor4.CopyImageWindow(Col.DoubleFromUShort);
        }

        internal static Tensor4<byte> ToByteColor(this Tensor4<uint> tensor4)
        {
            return tensor4.CopyImageWindow(Col.ByteFromUInt);
        }

        internal static Tensor4<ushort> ToUShortColor(this Tensor4<uint> tensor4)
        {
            return tensor4.CopyImageWindow(Col.UShortFromUInt);
        }

        internal static Tensor4<float> ToFloatColor(this Tensor4<uint> tensor4)
        {
            return tensor4.CopyImageWindow(Col.FloatFromUInt);
        }

        internal static Tensor4<double> ToDoubleColor(this Tensor4<uint> tensor4)
        {
            return tensor4.CopyImageWindow(Col.DoubleFromUInt);
        }

        internal static Tensor4<byte> ToByteColor(this Tensor4<float> tensor4)
        {
            return tensor4.CopyImageWindow(Col.ByteFromFloat);
        }

        internal static Tensor4<ushort> ToUShortColor(this Tensor4<float> tensor4)
        {
            return tensor4.CopyImageWindow(Col.UShortFromFloat);
        }

        internal static Tensor4<uint> ToUIntColor(this Tensor4<float> tensor4)
        {
            return tensor4.CopyImageWindow(Col.UIntFromFloat);
        }

        internal static Tensor4<double> ToDoubleColor(this Tensor4<float> tensor4)
        {
            return tensor4.CopyImageWindow(Col.DoubleFromFloat);
        }

        internal static Tensor4<byte> ToByteColor(this Tensor4<double> tensor4)
        {
            return tensor4.CopyImageWindow(Col.ByteFromDouble);
        }

        internal static Tensor4<ushort> ToUShortColor(this Tensor4<double> tensor4)
        {
            return tensor4.CopyImageWindow(Col.UShortFromDouble);
        }

        internal static Tensor4<uint> ToUIntColor(this Tensor4<double> tensor4)
        {
            return tensor4.CopyImageWindow(Col.UIntFromDouble);
        }

        internal static Tensor4<float> ToFloatColor(this Tensor4<double> tensor4)
        {
            return tensor4.CopyImageWindow(Col.FloatFromDouble);
        }

        #endregion

        #region Get/Set Matrix Rows/Cols

        public static Vector<T> GetRow<T>(this Matrix<T> m, int i)
        {
            return m.SubXVector(i);
        }

        public static Vector<T> GetCol<T>(this Matrix<T> m, int i)
        {
            return m.SubYVector(i);
        }

        public static void SetRow<T>(this Matrix<T> m, int i, ref Vector<T> data)
        {
            m.SubXVector(i).Set(data);
        }

        public static void SetCol<T>(this Matrix<T> m, int i, ref Vector<T> data)
        {
            m.SubYVector(i).Set(data);
        }

        #endregion

        #region Transformed (Matrix, Volume) [CSharp]

        public static Matrix<T> Transformed<T>(
                this Matrix<T> matrix, ImageTrafo rotation)
        {
            long sx = matrix.Size.X, sy = matrix.Size.Y;
            long dx = matrix.Delta.X, dy = matrix.Delta.Y;
            switch (rotation)
            {
                case ImageTrafo.Rot0: return matrix;
                case ImageTrafo.Rot90: return matrix.SubMatrix(sx - 1, 0, sy, sx, dy, -dx);
                case ImageTrafo.Rot180: return matrix.SubMatrix(sx - 1, sy - 1, sx, sy, -dx, -dy);
                case ImageTrafo.Rot270: return matrix.SubMatrix(0, sy - 1, sy, sx, -dy, dx);
                case ImageTrafo.MirrorX: return matrix.SubMatrix(sx - 1, 0, sx, sy, -dx, dy);
                case ImageTrafo.Transpose: return matrix.SubMatrix(0, 0, sy, sx, dy, dx);
                case ImageTrafo.MirrorY: return matrix.SubMatrix(0, sy - 1, sx, sy, dx, -dy);
                case ImageTrafo.Transverse: return matrix.SubMatrix(sx - 1, sy - 1, sy, sx, -dy, -dx);
            }
            throw new ArgumentException();
        }

        public static Volume<T> Transformed<T>(
                this Volume<T> volume, ImageTrafo rotation)
        {
            long sx = volume.Size.X, sy = volume.Size.Y, sz = volume.Size.Z;
            long dx = volume.Delta.X, dy = volume.Delta.Y, dz = volume.Delta.Z;
            switch (rotation)
            {
                case ImageTrafo.Rot0: return volume;
                case ImageTrafo.Rot90: return volume.SubVolume(sx - 1, 0, 0, sy, sx, sz, dy, -dx, dz);
                case ImageTrafo.Rot180: return volume.SubVolume(sx - 1, sy - 1, 0, sx, sy, sz, -dx, -dy, dz);
                case ImageTrafo.Rot270: return volume.SubVolume(0, sy - 1, 0, sy, sx, sz, -dy, dx, dz);
                case ImageTrafo.MirrorX: return volume.SubVolume(sx - 1, 0, 0, sx, sy, sz, -dx, dy, dz);
                case ImageTrafo.Transpose: return volume.SubVolume(0, 0, 0, sy, sx, sz, dy, dx, dz);
                case ImageTrafo.MirrorY: return volume.SubVolume(0, sy - 1, 0, sx, sy, sz, dx, -dy, dz);
                case ImageTrafo.Transverse: return volume.SubVolume(sx - 1, sy - 1, 0, sy, sx, sz, -dy, -dx, dz);
            }
            throw new ArgumentException();
        }

        #endregion

    }
}
