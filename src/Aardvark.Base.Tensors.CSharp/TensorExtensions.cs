using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Aardvark.Base
{
    /// <summary>
    /// Wrappers for the best (fastest) available implementation of the respective tensor operation.
    /// </summary>
    public static partial class TensorExtensions
    {
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

        #region Byte

        internal static Volume<ushort> ToUShortColor(this Volume<byte> volume)
        {
            return volume.MapToImageWindow(Col.ByteToUShort);
        }

        internal static Volume<uint> ToUIntColor(this Volume<byte> volume)
        {
            return volume.MapToImageWindow(Col.ByteToUInt);
        }

        internal static Volume<Half> ToHalfColor(this Volume<byte> volume)
        {
            return volume.MapToImageWindow(Col.ByteToHalf);
        }

        internal static Volume<float> ToFloatColor(this Volume<byte> volume)
        {
            return volume.MapToImageWindow(Col.ByteToFloat);
        }

        internal static Volume<double> ToDoubleColor(this Volume<byte> volume)
        {
            return volume.MapToImageWindow(Col.ByteToDouble);
        }

        #endregion

        #region UShort

        internal static Volume<byte> ToByteColor(this Volume<ushort> volume)
        {
            return volume.MapToImageWindow(Col.UShortToByte);
        }

        internal static Volume<uint> ToUIntColor(this Volume<ushort> volume)
        {
            return volume.MapToImageWindow(Col.UShortToUInt);
        }

        internal static Volume<Half> ToHalfColor(this Volume<ushort> volume)
        {
            return volume.MapToImageWindow(Col.UShortToHalf);
        }

        internal static Volume<float> ToFloatColor(this Volume<ushort> volume)
        {
            return volume.MapToImageWindow(Col.UShortToFloat);
        }

        internal static Volume<double> ToDoubleColor(this Volume<ushort> volume)
        {
            return volume.MapToImageWindow(Col.UShortToDouble);
        }

        #endregion

        #region UInt

        internal static Volume<byte> ToByteColor(this Volume<uint> volume)
        {
            return volume.MapToImageWindow(Col.UIntToByte);
        }

        internal static Volume<ushort> ToUShortColor(this Volume<uint> volume)
        {
            return volume.MapToImageWindow(Col.UIntToUShort);
        }

        internal static Volume<Half> ToHalfColor(this Volume<uint> volume)
        {
            return volume.MapToImageWindow(Col.UIntToHalf);
        }

        internal static Volume<float> ToFloatColor(this Volume<uint> volume)
        {
            return volume.MapToImageWindow(Col.UIntToFloat);
        }

        internal static Volume<double> ToDoubleColor(this Volume<uint> volume)
        {
            return volume.MapToImageWindow(Col.UIntToDouble);
        }

        #endregion

        #region Half

        internal static Volume<byte> ToByteColor(this Volume<Half> volume)
        {
            return volume.MapToImageWindow(Col.HalfToByte);
        }

        internal static Volume<ushort> ToUShortColor(this Volume<Half> volume)
        {
            return volume.MapToImageWindow(Col.HalfToUShort);
        }

        internal static Volume<uint> ToUIntColor(this Volume<Half> volume)
        {
            return volume.MapToImageWindow(Col.HalfToUInt);
        }

        internal static Volume<float> ToFloatColor(this Volume<Half> volume)
        {
            return volume.MapToImageWindow(Col.HalfToFloat);
        }

        internal static Volume<double> ToDoubleColor(this Volume<Half> volume)
        {
            return volume.MapToImageWindow(Col.HalfToDouble);
        }

        #endregion

        #region Float

        internal static Volume<byte> ToByteColor(this Volume<float> volume)
        {
            return volume.MapToImageWindow(Col.FloatToByte);
        }

        internal static Volume<ushort> ToUShortColor(this Volume<float> volume)
        {
            return volume.MapToImageWindow(Col.FloatToUShort);
        }

        internal static Volume<uint> ToUIntColor(this Volume<float> volume)
        {
            return volume.MapToImageWindow(Col.FloatToUInt);
        }

        internal static Volume<Half> ToHalfColor(this Volume<float> volume)
        {
            return volume.MapToImageWindow(Col.FloatToHalf);
        }

        internal static Volume<double> ToDoubleColor(this Volume<float> volume)
        {
            return volume.MapToImageWindow(Col.FloatToDouble);
        }

        #endregion

        #region Double

        internal static Volume<byte> ToByteColor(this Volume<double> volume)
        {
            return volume.MapToImageWindow(Col.DoubleToByte);
        }

        internal static Volume<ushort> ToUShortColor(this Volume<double> volume)
        {
            return volume.MapToImageWindow(Col.DoubleToUShort);
        }

        internal static Volume<uint> ToUIntColor(this Volume<double> volume)
        {
            return volume.MapToImageWindow(Col.DoubleToUInt);
        }

        internal static Volume<Half> ToHalfColor(this Volume<double> volume)
        {
            return volume.MapToImageWindow(Col.DoubleToHalf);
        }

        internal static Volume<float> ToFloatColor(this Volume<double> volume)
        {
            return volume.MapToImageWindow(Col.DoubleToFloat);
        }

        #endregion

        #endregion

        #region Conversions Tensor4 to Tensor4 (byte, ushort, uint, float, double) [CSharp (internal)]

        // All the following conversions are internal only, since we do not
        // know about the channel order at this level. Only PixVolumes know
        // about channel order.

        #region Byte

        internal static Tensor4<ushort> ToUShortColor(this Tensor4<byte> tensor4)
        {
            return tensor4.MapToImageWindow(Col.ByteToUShort);
        }

        internal static Tensor4<uint> ToUIntColor(this Tensor4<byte> tensor4)
        {
            return tensor4.MapToImageWindow(Col.ByteToUInt);
        }

        internal static Tensor4<Half> ToHalfColor(this Tensor4<byte> tensor4)
        {
            return tensor4.MapToImageWindow(Col.ByteToHalf);
        }

        internal static Tensor4<float> ToFloatColor(this Tensor4<byte> tensor4)
        {
            return tensor4.MapToImageWindow(Col.ByteToFloat);
        }

        internal static Tensor4<double> ToDoubleColor(this Tensor4<byte> tensor4)
        {
            return tensor4.MapToImageWindow(Col.ByteToDouble);
        }

        #endregion

        #region UShort

        internal static Tensor4<byte> ToByteColor(this Tensor4<ushort> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UShortToByte);
        }

        internal static Tensor4<uint> ToUIntColor(this Tensor4<ushort> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UShortToUInt);
        }

        internal static Tensor4<Half> ToHalfColor(this Tensor4<ushort> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UShortToHalf);
        }

        internal static Tensor4<float> ToFloatColor(this Tensor4<ushort> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UShortToFloat);
        }

        internal static Tensor4<double> ToDoubleColor(this Tensor4<ushort> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UShortToDouble);
        }

        #endregion

        #region UInt

        internal static Tensor4<byte> ToByteColor(this Tensor4<uint> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UIntToByte);
        }

        internal static Tensor4<ushort> ToUShortColor(this Tensor4<uint> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UIntToUShort);
        }

        internal static Tensor4<Half> ToHalfColor(this Tensor4<uint> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UIntToHalf);
        }

        internal static Tensor4<float> ToFloatColor(this Tensor4<uint> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UIntToFloat);
        }

        internal static Tensor4<double> ToDoubleColor(this Tensor4<uint> tensor4)
        {
            return tensor4.MapToImageWindow(Col.UIntToDouble);
        }

        #endregion

        #region Half

        internal static Tensor4<byte> ToByteColor(this Tensor4<Half> tensor4)
        {
            return tensor4.MapToImageWindow(Col.HalfToByte);
        }

        internal static Tensor4<ushort> ToUShortColor(this Tensor4<Half> tensor4)
        {
            return tensor4.MapToImageWindow(Col.HalfToUShort);
        }

        internal static Tensor4<uint> ToUIntColor(this Tensor4<Half> tensor4)
        {
            return tensor4.MapToImageWindow(Col.HalfToUInt);
        }

        internal static Tensor4<float> ToFloatColor(this Tensor4<Half> tensor4)
        {
            return tensor4.MapToImageWindow(Col.HalfToFloat);
        }

        internal static Tensor4<double> ToDoubleColor(this Tensor4<Half> tensor4)
        {
            return tensor4.MapToImageWindow(Col.HalfToDouble);
        }

        #endregion

        #region Float

        internal static Tensor4<byte> ToByteColor(this Tensor4<float> tensor4)
        {
            return tensor4.MapToImageWindow(Col.FloatToByte);
        }

        internal static Tensor4<ushort> ToUShortColor(this Tensor4<float> tensor4)
        {
            return tensor4.MapToImageWindow(Col.FloatToUShort);
        }

        internal static Tensor4<uint> ToUIntColor(this Tensor4<float> tensor4)
        {
            return tensor4.MapToImageWindow(Col.FloatToUInt);
        }

        internal static Tensor4<Half> ToHalfColor(this Tensor4<float> tensor4)
        {
            return tensor4.MapToImageWindow(Col.FloatToHalf);
        }

        internal static Tensor4<double> ToDoubleColor(this Tensor4<float> tensor4)
        {
            return tensor4.MapToImageWindow(Col.FloatToDouble);
        }

        #endregion

        #region Double

        internal static Tensor4<byte> ToByteColor(this Tensor4<double> tensor4)
        {
            return tensor4.MapToImageWindow(Col.DoubleToByte);
        }

        internal static Tensor4<ushort> ToUShortColor(this Tensor4<double> tensor4)
        {
            return tensor4.MapToImageWindow(Col.DoubleToUShort);
        }

        internal static Tensor4<uint> ToUIntColor(this Tensor4<double> tensor4)
        {
            return tensor4.MapToImageWindow(Col.DoubleToUInt);
        }
        internal static Tensor4<Half> ToHalfColor(this Tensor4<double> tensor4)
        {
            return tensor4.MapToImageWindow(Col.DoubleToHalf);
        }

        internal static Tensor4<float> ToFloatColor(this Tensor4<double> tensor4)
        {
            return tensor4.MapToImageWindow(Col.DoubleToFloat);
        }

        #endregion

        #endregion

        #region Get/Set Matrix Rows/Cols

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<T> GetRow<T>(this Matrix<T> m, int i)
        {
            return m.SubXVector(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Vector<T> GetCol<T>(this Matrix<T> m, int i)
        {
            return m.SubYVector(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetRow<T>(this Matrix<T> m, int i, ref Vector<T> data)
        {
            m.SubXVector(i).Set(data);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetCol<T>(this Matrix<T> m, int i, ref Vector<T> data)
        {
            m.SubYVector(i).Set(data);
        }

        #endregion

        #region Transformed (Matrix, Volume) [CSharp]

        public static MatrixInfo Transformed(this MatrixInfo info, ImageTrafo trafo)
        {
            long sx = info.Size.X, sy = info.Size.Y;
            long dx = info.Delta.X, dy = info.Delta.Y;
            switch (trafo)
            {
                case ImageTrafo.Identity: return info;
                case ImageTrafo.Rot90: return info.SubMatrix(sx - 1, 0, sy, sx, dy, -dx);
                case ImageTrafo.Rot180: return info.SubMatrix(sx - 1, sy - 1, sx, sy, -dx, -dy);
                case ImageTrafo.Rot270: return info.SubMatrix(0, sy - 1, sy, sx, -dy, dx);
                case ImageTrafo.MirrorX: return info.SubMatrix(sx - 1, 0, sx, sy, -dx, dy);
                case ImageTrafo.Transpose: return info.SubMatrix(0, 0, sy, sx, dy, dx);
                case ImageTrafo.MirrorY: return info.SubMatrix(0, sy - 1, sx, sy, dx, -dy);
                case ImageTrafo.Transverse: return info.SubMatrix(sx - 1, sy - 1, sy, sx, -dy, -dx);
            }
            throw new ArgumentException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Matrix<T> Transformed<T>(this Matrix<T> matrix, ImageTrafo trafo)
            => new Matrix<T>(matrix.Data, matrix.Info.Transformed(trafo));

        public static VolumeInfo Transformed(this VolumeInfo info, ImageTrafo trafo)
        {
            long sx = info.Size.X, sy = info.Size.Y, sz = info.Size.Z;
            long dx = info.Delta.X, dy = info.Delta.Y, dz = info.Delta.Z;
            switch (trafo)
            {
                case ImageTrafo.Identity: return info;
                case ImageTrafo.Rot90: return info.SubVolume(sx - 1, 0, 0, sy, sx, sz, dy, -dx, dz);
                case ImageTrafo.Rot180: return info.SubVolume(sx - 1, sy - 1, 0, sx, sy, sz, -dx, -dy, dz);
                case ImageTrafo.Rot270: return info.SubVolume(0, sy - 1, 0, sy, sx, sz, -dy, dx, dz);
                case ImageTrafo.MirrorX: return info.SubVolume(sx - 1, 0, 0, sx, sy, sz, -dx, dy, dz);
                case ImageTrafo.Transpose: return info.SubVolume(0, 0, 0, sy, sx, sz, dy, dx, dz);
                case ImageTrafo.MirrorY: return info.SubVolume(0, sy - 1, 0, sx, sy, sz, dx, -dy, dz);
                case ImageTrafo.Transverse: return info.SubVolume(sx - 1, sy - 1, 0, sy, sx, sz, -dy, -dx, dz);
            }
            throw new ArgumentException();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Volume<T> Transformed<T>(this Volume<T> volume, ImageTrafo trafo)
            => new Volume<T>(volume.Data, volume.Info.Transformed(trafo));

        #endregion

        #region Scaling

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Half LerpHalf(double t, Half x, Half y) => Fun.Lerp((Half)t, x, y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float LerpFloat(double t, float x, float y) => Fun.Lerp((float)t, x, y);

        private static readonly Dictionary<Type, Action<object, object>> scaleNearestTable =
            new Dictionary<Type, Action<object, object>>()
            {
                {typeof(byte),   (src, dst) => ((Matrix<byte>)dst).SetScaledNearest((Matrix<byte>)src)},
                {typeof(ushort), (src, dst) => ((Matrix<ushort>)dst).SetScaledNearest((Matrix<ushort>)src)},
                {typeof(uint),   (src, dst) => ((Matrix<uint>)dst).SetScaledNearest((Matrix<uint>)src)},
                {typeof(Half),   (src, dst) => ((Matrix<Half>)dst).SetScaledNearest((Matrix<Half>)src)},
                {typeof(float),  (src, dst) => ((Matrix<float>)dst).SetScaledNearest((Matrix<float>)src)},
                {typeof(double), (src, dst) => ((Matrix<double>)dst).SetScaledNearest((Matrix<double>)src)}
            };

        private static readonly Dictionary<Type, Action<object, object>> scaleLinearTable =
            new Dictionary<Type, Action<object, object>>()
            {
                {typeof(byte),   (src, dst) => ((Matrix<byte>)dst).SetScaledLinear((Matrix<byte>)src, Fun.Lerp, Fun.Lerp)},
                {typeof(ushort), (src, dst) => ((Matrix<ushort>)dst).SetScaledLinear((Matrix<ushort>)src, Fun.Lerp, Fun.Lerp)},
                {typeof(uint),   (src, dst) => ((Matrix<uint>)dst).SetScaledLinear((Matrix<uint>)src, Fun.Lerp, Fun.Lerp)},
                {typeof(Half),   (src, dst) => ((Matrix<Half>)dst).SetScaledLinear((Matrix<Half>)src, LerpHalf, LerpHalf)},
                {typeof(float),  (src, dst) => ((Matrix<float>)dst).SetScaledLinear((Matrix<float>)src, LerpFloat, LerpFloat)},
                {typeof(double), (src, dst) => ((Matrix<double>)dst).SetScaledLinear((Matrix<double>)src, Fun.Lerp, Fun.Lerp)}
            };

        private static readonly Dictionary<Type, Action<object, object>> scaleCubicTable =
            new Dictionary<Type, Action<object, object>>()
            {
                {typeof(byte),   (src, dst) => ((Matrix<byte>)dst).SetScaledCubic((Matrix<byte>)src)},
                {typeof(ushort), (src, dst) => ((Matrix<ushort>)dst).SetScaledCubic((Matrix<ushort>)src)},
                {typeof(uint),   (src, dst) => ((Matrix<uint>)dst).SetScaledCubic((Matrix<uint>)src)},
                {typeof(Half),   (src, dst) => ((Matrix<Half>)dst).SetScaledCubic((Matrix<Half>)src)},
                {typeof(float),  (src, dst) => ((Matrix<float>)dst).SetScaledCubic((Matrix<float>)src)},
                {typeof(double), (src, dst) => ((Matrix<double>)dst).SetScaledCubic((Matrix<double>)src)}
            };

        private static readonly Dictionary<Type, Action<object, object>> scaleLanczosTable =
            new Dictionary<Type, Action<object, object>>()
            {
                {typeof(byte),   (src, dst) => ((Matrix<byte>)dst).SetScaledLanczos((Matrix<byte>)src)},
                {typeof(ushort), (src, dst) => ((Matrix<ushort>)dst).SetScaledLanczos((Matrix<ushort>)src)},
                {typeof(uint),   (src, dst) => ((Matrix<uint>)dst).SetScaledLanczos((Matrix<uint>)src)},
                {typeof(Half),   (src, dst) => ((Matrix<Half>)dst).SetScaledLanczos((Matrix<Half>)src)},
                {typeof(float),  (src, dst) => ((Matrix<float>)dst).SetScaledLanczos((Matrix<float>)src)},
                {typeof(double), (src, dst) => ((Matrix<double>)dst).SetScaledLanczos((Matrix<double>)src)}
            };

        private static void SetScaled<T>(Dictionary<Type, Action<object, object>> table, Matrix<T> src, Matrix<T> dst)
        {
            if (table.TryGetValue(typeof(T), out var setScaled))
            {
                setScaled(src, dst);
            }
            else
            {
                throw new InvalidOperationException($"Cannot invoke Scaled() for Matrix<{typeof(T).Name}>");
            }
        }

        public static Volume<T> Scaled<T>(this Volume<T> source, V2d scaleFactor, ImageInterpolation interpolation)
        {
            if (scaleFactor.AnySmaller(0.0))
            {
                throw new ArgumentException("Scale factor cannot be negative");
            }

            var table =
                interpolation switch
                {
                    ImageInterpolation.Near => scaleNearestTable,
                    ImageInterpolation.Linear => scaleLinearTable,
                    ImageInterpolation.Cubic => scaleCubicTable,
                    ImageInterpolation.Lanczos => scaleLanczosTable,
                    _ => throw new NotImplementedException($"{interpolation} filter not supported.")
                };

            var size = (V2d.Half + scaleFactor * (V2d)source.Size.XY).ToV2l();
            var channels = source.Size.Z;
            var target = ImageTensors.CreateImageVolume<T>(new V3l(size.XY, channels));

            for (int c = 0; c < channels; c++)
            {
                SetScaled(table, source.SubXYMatrixWindow(c), target.SubXYMatrixWindow(c));
            }

            return target;
        }

        #endregion
    }
}
