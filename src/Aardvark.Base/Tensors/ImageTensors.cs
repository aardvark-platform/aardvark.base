using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// Interpolation scheme.
    /// Values correspond to IPPI_INTERP_*
    /// </summary>
    public enum ImageInterpolation : int
    {
        SmoothEdge = -2147483648, ///IPPI_SMOOTH_EDGE  = (1 << 31)
        Near = 1, ///IPPI_INTER_NN
        Linear = 2, ///IPPI_INTER_LINEAR
        Cubic = 4, ///IPPI_INTER_CUBIC
        SuperSample = 8, ///IPPI_INTER_SUPER
        Lanczos = 16, ///IPPI_INTER_LANCZOS
    }

    public enum ImageTrafo
    {
        /// <summary>No Trafo.</summary>
        Rot0 = 0,
        /// <summary>Rotate 90° CCW.</summary>
        Rot90 = 1,
        /// <summary>Rotate 180°.</summary>
        Rot180 = 2,
        /// <summary>Rotate 270° CCW.</summary>
        Rot270 = 3,
        /// <summary>Mirror along X-direction, horizontally, about Y-Axis.</summary>
        MirrorX = 4,
        /// <summary>Exchange X and Y axes, flip about diagonal +x/+y.</summary>
        Transpose = 5,
        /// <summary>Mirror along Y-direction, vertically, about X-Axis.</summary>
        MirrorY = 6,
        /// <summary>Exchange X and Y axes and rotate 180°, flip about diagonal +x/-y.</summary>
        Transverse = 7,
    };

    public enum ImageBorderType
    {
        Const = 0,
        Repl = 1,
        Wrap = 2,
        Mirror = 3,
        MirrorR = 4,
        Mirror2 = 5,
        InMem = 6,
        InMemTop = 16,
        InMemBottom = 32,
    }

    public enum Norm
    {
        NormInf = 0,
        NormL1 = 1,
        NormL2 = 2,
        NormFM = 3,
    }

    public static class ImageTensors
    {
        #region Layout

        public static bool HasImageLayout<T>(this Matrix<T> matrix)
        {
            return matrix.First == V2l.Zero && matrix.Origin == 0L
                    && matrix.Info.Delta.X == 1L
                    && matrix.Info.Delta.Y == matrix.Info.Size.X
                    && matrix.Data.LongLength == matrix.Info.Count;
        }

        public static bool HasImageWindowLayout<T>(this Matrix<T> matrix)
        {
            return matrix.Info.Delta.X == 1L && matrix.Info.Delta.Y > 0L; 
        }

        public static bool HasImageLayout<T>(this Volume<T> volume)
        {
            return volume.First == V3l.Zero && volume.Origin == 0L
                    && volume.Info.Delta.Z == 1L
                    && volume.Info.Delta.X == volume.Info.Size.Z
                    && volume.Info.Delta.Y == volume.Info.Size.X * volume.Info.Delta.X
                    && volume.Data.LongLength == volume.Info.Size.Y * volume.Info.Delta.Y;
        }

        public static bool HasImageWindowLayout<T>(this Volume<T> vol)
        {
            return vol.Info.Delta.Z == 1L
                    && vol.Info.Delta.X == vol.Info.Size.Z
                    && vol.Info.Delta.Y > 0L;
        }

        public static bool HasImageLayout<T>(this Tensor4<T> tensor4)
        {
            return tensor4.First == V4l.Zero && tensor4.Origin == 0L
                    && tensor4.Info.Delta.W == 1L
                    && tensor4.Info.Delta.X == tensor4.Info.Size.W
                    && tensor4.Info.Delta.Y == tensor4.Info.Size.X * tensor4.Info.Delta.X
                    && tensor4.Info.Delta.Z == tensor4.Info.Size.Y * tensor4.Info.Delta.Y
                    && tensor4.Data.LongLength == tensor4.Info.Size.Z * tensor4.Info.Delta.Z;
        }

        public static bool HasImageWindowLayout<T>(this Tensor4<T> tensor4)
        {
            return tensor4.Info.Delta.W == 1L
                    && tensor4.Info.Delta.X == tensor4.Info.Size.W
                    && tensor4.Info.Delta.Y > 0L
                    && tensor4.Info.Delta.Z > 0L;
        }

        #endregion

        #region Creation

        /// <summary>
        /// Create a valid image matrix, with stride appropriate for the
        /// given size. Note that this is just the same as a normal matrix.
        /// </summary>
        public static Matrix<T> CreateImageMatrix<T>(this V2l size)
        {
            return new Matrix<T>(size);
        }

        /// <summary>
        /// Create a valid image volume, with stride appropriate for the
        /// given size. For image volumes DZ == 1.
        /// </summary>
        public static Volume<T> CreateImageVolume<T>(this V3l size)
        {
            return new Volume<T>(0L, size, new V3l(size.Z, size.Z * size.X, 1L));
        }


        public static Volume<T> CreateImageVolume<T>(this T[] data, V3l size)
        {
            return new Volume<T>(data, 0L, size, new V3l(size.Z, size.Z * size.X, 1L));
        }


        #endregion

        #region VecPixels

        /// <summary>
        /// Access a single pixel of an image volume as a Vec. Note that this
        /// involves an indexing operation with two index multiplications.
        /// </summary>
        public static Vec<T> VecPixel<T>(
                this Volume<T> vol, long x, long y)
        {
            #if DEBUG
            if (vol.Info.Delta.Z != 1) throw new ArgumentException();
            #endif
            return new Vec<T>(vol.Data,
                              vol.Info.Origin + x * vol.Info.Delta.X
                                              + y * vol.Info.Delta.Y,
                              vol.Info.Size.Z);
        }

        /// <summary>
        /// Access a single pixel of an image volume as a Vec. Note that this
        /// involves an indexing operation with two index multiplications.
        /// </summary>
        public static Vec<T> VecPixel<T>(
                this Volume<T> vol, V2l position)
        {
            #if DEBUG
            if (vol.Info.Delta.Z != 1) throw new ArgumentException();
            #endif
            return new Vec<T>(vol.Data,
                              vol.Info.Origin + position.X * vol.Info.Delta.X
                                              + position.Y * vol.Info.Delta.Y,
                              vol.Info.Size.Z);
        }

        public static IEnumerable<Vec<T>> VecPixelsXY<T>(
                this Volume<T> vol)
        {
            return vol.Info.IndicesXY.Select(i => new Vec<T>(vol.Data, i, vol.Info.Size.Z));
        }

        public static IEnumerable<Vec<T>> VecPixelsYX<T>(
                this Volume<T> vol)
        {
            return vol.Info.IndicesYX.Select(i => new Vec<T>(vol.Data, i, vol.Info.Size.Z));
        }

        public static IEnumerable<Vec<T>> VecPixelsX<T>(
                this Volume<T> vol)
        {
            return vol.Info.IndicesX.Select(i => new Vec<T>(vol.Data, i, vol.Info.Size.Z));
        }

        public static IEnumerable<Vec<T>> VecPixelsY<T>(
                this Volume<T> vol)
        {
            return vol.Info.IndicesY.Select(i => new Vec<T>(vol.Data, i, vol.Info.Size.Z));
        }

        #endregion

        #region Tensor4

        /// <summary>
        /// Create a valid image tensor4, with stride appropriate for the
        /// given size.
        /// </summary>
        public static Tensor4<T> CreateImageTensor4<T>(this V4l size)
        {
            return new Tensor4<T>(0L, size,
                    new V4l(size.W, size.W * size.X, size.W * size.X * size.Y, 1L));
        }

        public static Volume<T> SubImageWindowAtZ<T>(this Tensor4<T> tensor4, long z)
        {
            return tensor4.SubXYWVolumeWindow(z);
        }

        public static Volume<T> SubImageAtZ<T>(this Tensor4<T> tensor4, long z)
        {
            return tensor4.SubXYWVolume(z);
        }

        #endregion

        #region SubImages

        /// <summary>
        /// Return a sub image volume beginning at the supplied pixel
        /// coordinate with the supplied size in pixels. The sub image
        /// retains the coordinates of the parent image.
        /// </summary>
        public static Volume<T> SubImageWindow<T>(
                this Volume<T> vol, V2l begin, V2l size)
        {
            return vol.SubVolumeWindow(new V3l(begin.X, begin.Y, vol.FZ),
                                 new V3l(size.X, size.Y, vol.SZ));
        }

        /// <summary>
        /// Return a sub image volume beginning at the supplied pixel
        /// coordinate with the supplied size in pixels. The sub image
        /// retains the coordinates of the parent image.
        /// </summary>
        public static Volume<T> SubImageWindow<T>(
                this Volume<T> vol, long beginX, long beginY, long sizeX, long sizeY)
        {
            return vol.SubVolumeWindow(new V3l(beginX, beginY, vol.FZ),
                                       new V3l(sizeX, sizeY, vol.SZ));
        }

        /// <summary>
        /// Return a sub image volume beginning at the supplied pixel
        /// coordinate with the supplied size in pixels. The coordinates
        /// of the sub image volume start at 0, 0, 0.
        /// </summary>
        public static Volume<T> SubImage<T>(
                this Volume<T> vol, V2l begin, V2l size)
        {
            return vol.SubVolume(new V3l(begin.X, begin.Y, vol.FZ),
                                 new V3l(size.X, size.Y, vol.SZ));
        }

        /// <summary>
        /// Return a sub image volume beginning at the supplied pixel
        /// coordinate with the supplied size in pixels. The coordinates
        /// of the sub image volume start at 0, 0, 0.
        /// </summary>
        public static Volume<T> SubImage<T>(
                this Volume<T> vol, long beginX, long beginY, long sizeX, long sizeY)
        {
            return vol.SubVolume(new V3l(beginX, beginY, vol.FZ),
                                 new V3l(sizeX, sizeY, vol.SZ));
        }

        /// <summary>
        /// Returns a single image row as an image volume.
        /// </summary>
        public static Volume<T> ImageWindowRow<T>(
                this Volume<T> vol, long y)
        {
            return vol.SubVolumeWindow(new V3l(vol.FX, y, vol.FZ),
                                       new V3l(vol.SX, 1, vol.SZ));
        }


        /// <summary>
        /// Returns a single image column as an image volume.
        /// </summary>
        public static Volume<T> ImageWindowCol<T>(
                this Volume<T> vol, long x)
        {
            return vol.SubVolumeWindow(new V3l(x, vol.FY, vol.FZ),
                                       new V3l(1, vol.SY, vol.SZ));
        }

        /// <summary>
        /// Returns a single image row as an image volume with coordinates
        /// starting at 0, 0, 0.
        /// </summary>
        public static Volume<T> ImageRow<T>(
                this Volume<T> vol, long y)
        {
            return vol.SubVolume(new V3l(vol.FX, y, vol.FZ),
                                 new V3l(vol.SX, 1, vol.SZ));
        }

        /// <summary>
        /// Returns a single image column as an image volume with coordinates
        /// starting at 0, 0, 0.
        /// </summary>
        public static Volume<T> ImageCol<T>(
                this Volume<T> vol, long x)
        {
            return vol.SubVolume(new V3l(x, vol.FY, vol.FZ),
                                 new V3l(1, vol.SY, vol.SZ));
        }

        #endregion

        #region CopyImage

        /// <summary>
        /// Copy this volume to image memory layout.
        /// Retains the coordinates of the original image.
        /// </summary>
        public static Volume<T> CopyImageWindow<T>(this Volume<T> volume)
        {
            var copy = CreateImageVolume<T>(volume.S);
            copy.F = volume.F;
            copy.Set(volume);
            return copy;
        }

        /// <summary>
        /// Copy this volume to image memory layout.
        /// Coordinates of the result start at [0, 0, 0].
        /// </summary>
        public static Volume<T> CopyImage<T>(this Volume<T> volume)
        {
            var copy = CreateImageVolume<T>(volume.S);
            copy.Set(volume);
            return copy;
        }

        /// <summary>
        /// Copy this volume to image memory layout, while piping all elements
        /// through the supplied function.
        /// Retains the coordinates of the original image.
        /// </summary>
        public static Volume<T1> CopyImageWindow<T, T1>(this Volume<T> volume, Func<T, T1> fun)
        {
            var copy = CreateImageVolume<T1>(volume.S);
            copy.F = volume.F;
            copy.Set(volume, fun);
            return copy;
        }

        /// <summary>
        /// Copy this volume to image memory layout, while piping all elements
        /// through the supplied function.
        /// Coordinates of the result start at [0, 0, 0].
        /// </summary>
        public static Volume<T1> CopyImage<T, T1>(this Volume<T> volume, Func<T, T1> fun)
        {
            var copy = CreateImageVolume<T1>(volume.S);
            copy.Set(volume, fun);
            return copy;
        }

        /// <summary>
        /// Copy this tensor4 to image memory layout.
        /// Retains the coordinates of the original image.
        /// </summary>
        public static Tensor4<T> CopyImageWindow<T>(this Tensor4<T> tensor4)
        {
            var copy = CreateImageTensor4<T>(tensor4.S);
            copy.F = tensor4.F;
            copy.Set(tensor4);
            return copy;
        }

        /// <summary>
        /// Copy this tensor4 to image memory layout.
        /// Coordinates of the result start at [0, 0, 0, 0].
        /// </summary>
        public static Tensor4<T> CopyImage<T>(this Tensor4<T> tensor4)
        {
            var copy = CreateImageTensor4<T>(tensor4.S);
            copy.Set(tensor4);
            return copy;
        }

        /// <summary>
        /// Copy this tensor4 to image memory layout, while piping all elements
        /// through the supplied function.
        /// Retains the coordinates of the original image.
        /// </summary>
        public static Tensor4<T1> CopyImageWindow<T, T1>(this Tensor4<T> tensor4, Func<T, T1> fun)
        {
            var copy = CreateImageTensor4<T1>(tensor4.S);
            copy.F = tensor4.F;
            copy.Set(tensor4, fun);
            return copy;
        }

        /// <summary>
        /// Copy this tensor4 to image memory layout, while piping all elements
        /// through the supplied function.
        /// Coordinates of the result start at [0, 0, 0, 0].
        /// </summary>
        public static Tensor4<T1> CopyImage<T, T1>(this Tensor4<T> tensor4, Func<T, T1> fun)
        {
            var copy = CreateImageTensor4<T1>(tensor4.S);
            copy.Set(tensor4, fun);
            return copy;
        }

        #endregion

        #region ToImage

        /// <summary>
        /// Convert this matrix to image memory layout (if it is not already).
        /// Coordinates of the result start at [0, 0].
        /// </summary>
        public static Matrix<T> ToImage<T>(this Matrix<T> matrix)
        {
            if (matrix.HasImageLayout()) return matrix;
            return matrix.Copy();
        }

        /// <summary>
        /// Convert this matrix to image memory layout (if it is not already).
        /// Retains coordinates of original image.
        /// </summary>
        public static Matrix<T> ToImageWindow<T>(this Matrix<T> matrix)
        {
            if (matrix.HasImageWindowLayout()) return matrix;
            return matrix.CopyWindow();
        }

        /// <summary>
        /// Convert this volume to image memory layout (if it is not already).
        /// Coordinates of the result start at [0, 0, 0].
        /// </summary>
        public static Volume<T> ToImage<T>(this Volume<T> volume)
        {
            if (volume.HasImageLayout()) return volume;
            return volume.CopyImage();
        }

        /// <summary>
        /// Convert this volume to image memory layout (if it is not already).
        /// Retains coordinates of th original volume.
        /// </summary>
        public static Volume<T> ToImageWindow<T>(this Volume<T> volume)
        {
            if (volume.HasImageWindowLayout()) return volume;
            return volume.CopyImageWindow();
        }

        #endregion

        #region Reinterpretation

        /// <summary>
        /// Reinterpret an image volume as a matrix of colors with supplied color intent.
        /// </summary>
        public static Matrix<Td, Tv> GetMatrix<Td, Tv>(this Volume<Td> volume, Symbol intent)
        {
            var matrix = volume.SubXYMatrixWindow<Tv>(0L);
            matrix.Accessors = TensorAccessors.Get<Td, Tv>(typeof(Td), typeof(Tv),
                                                           intent, volume.DeltaArray);
            return matrix;
        }

        /// <summary>
        /// Reinterpret an image tensor as a volume of colors with supplied color intent.
        /// </summary>
        public static Volume<Td, Tv> GetVolume<Td, Tv>(this Tensor4<Td> tensor4, Symbol intent)
        {
            var volume = tensor4.SubXYZVolumeWindow<Tv>(0L);
            volume.Accessors = TensorAccessors.Get<Td, Tv>(typeof(Td), typeof(Tv),
                                                           intent, tensor4.DeltaArray);
            return volume;
        }

        /// <summary>
        /// Reinterpret a matrix as a volume with Size.Z == 1. The new volume starts at [0, 0, 0].
        /// </summary>
        public static Volume<T> AsVolume<T>(this Matrix<T> matrix)
        {
            return new Volume<T>(matrix.Data, matrix.Origin, matrix.Size.XYI, matrix.Delta.XYI);
        }

        /// <summary>
        /// Reinterpret a matrix as a volume with Size.Z == 1. Retain coordinates of the matrix.
        /// </summary>
        public static Volume<T> AsVolumeWindow<T>(this Matrix<T> matrix)
        {
            return new Volume<T>(matrix.Data, matrix.Origin, matrix.Size.XYI, matrix.Delta.XYI)
                                { F = matrix.F.XYO };
        }

        /// <summary>
        /// Reinterpret a volume with Size.Z == 1 as matrix. The new matrix starts at [0, 0].
        /// </summary>
        public static Matrix<T> AsMatrix<T>(this Volume<T> volume)
        {
            if (volume.SZ != 1L)
                throw new ArgumentException("cannot matrix-convert a volume with Size.Z != 1");
            return new Matrix<T>(volume.Data, volume.FirstIndex, volume.S.XY, volume.D.XY);
        }

        /// <summary>
        /// Reinterpret a volume with Size.Z == 1 as matrix. Retain the coordinates of the volume.
        /// </summary>
        public static Matrix<T> AsMatrixWindow<T>(this Volume<T> volume)
        {
            if (volume.SZ != 1L)
                throw new ArgumentException("cannot matrix-convert a volume with Size.Z != 1");
            return new Matrix<T>(volume.Data, volume.Info.FirstIndex, volume.S.XY, volume.D.XY)
                        { F = volume.F.XY };
        }

        /// <summary>
        /// Reinterpret a volume as a tensor4 with Size.W == 1. The new tensor4 starts at [0, 0, 0, 0].
        /// </summary>
        public static Tensor4<T> AsTensor4<T>(this Volume<T> volume)
        {
            return new Tensor4<T>(volume.Data, volume.Origin, volume.Size.XYZI, volume.Delta.XYZI);
        }

        /// <summary>
        /// Reinterpret a volume as a tensor4 with Size.W == 1. Retain coordinates of the volume.
        /// </summary>
        public static Tensor4<T> AsTensor4Window<T>(this Volume<T> volume)
        {
            return new Tensor4<T>(volume.Data, volume.Origin, volume.Size.XYZI, volume.Delta.XYZI)
                                 { F = volume.F.XYZO };
        }


        /// <summary>
        /// Reinterpret a tensor4 with Size.W == 1 as volume. The new volume starts at [0, 0, 0].
        /// </summary>
        public static Volume<T> AsVolume<T>(this Tensor4<T> tensor4)
        {
            if (tensor4.SW != 1L)
                throw new ArgumentException("cannot volume-convert a tensor4 with Size.W != 1");
            return new Volume<T>(tensor4.Data, tensor4.FirstIndex, tensor4.S.XYZ, tensor4.D.XYZ);
        }

        /// <summary>
        /// Reinterpret a tensor4 with Size.W == 1 as volume. Retain the coordinates of the tensor4.
        /// </summary>
        public static Volume<T> AsVolumeWindow<T>(this Tensor4<T> tensor4)
        {
            if (tensor4.SW != 1L)
                throw new ArgumentException("cannot volume-convert a tensor4 with Size.W != 1");
            return new Volume<T>(tensor4.Data, tensor4.FirstIndex, tensor4.S.XYZ, tensor4.D.XYZ)
                                { F = tensor4.F.XYZ };
        }

        #endregion

        #region Color Conversions

        public static Matrix<ushort> ToUShortColor(this Matrix<byte> matrix)
        {
            return matrix.CopyWindow(Col.UShortFromByte);
        }

        public static Matrix<uint> ToUIntColor(this Matrix<byte> matrix)
        {
            return matrix.CopyWindow(Col.UIntFromByte);
        }

        public static Matrix<float> ToFloatColor(this Matrix<byte> matrix)
        {
            return matrix.CopyWindow(Col.FloatFromByte);
        }

        public static Matrix<double> ToDoubleColor(this Matrix<byte> matrix)
        {
            return matrix.CopyWindow(Col.DoubleFromByte);
        }

        public static Matrix<byte> ToByteColor(this Matrix<ushort> matrix)
        {
            return matrix.CopyWindow(Col.ByteFromUShort);
        }

        public static Matrix<uint> ToUIntColor(this Matrix<ushort> matrix)
        {
            return matrix.CopyWindow(Col.UIntFromUShort);
        }

        public static Matrix<float> ToFloatColor(this Matrix<ushort> matrix)
        {
            return matrix.CopyWindow(Col.FloatFromUShort);
        }

        public static Matrix<double> ToDoubleColor(this Matrix<ushort> matrix)
        {
            return matrix.CopyWindow(Col.DoubleFromUShort);
        }

        public static Matrix<byte> ToByteColor(this Matrix<uint> matrix)
        {
            return matrix.CopyWindow(Col.ByteFromUInt);
        }

        public static Matrix<ushort> ToUShortColor(this Matrix<uint> matrix)
        {
            return matrix.CopyWindow(Col.UShortFromUInt);
        }

        public static Matrix<float> ToFloatColor(this Matrix<uint> matrix)
        {
            return matrix.CopyWindow(Col.FloatFromUInt);
        }

        public static Matrix<double> ToDoubleColor(this Matrix<uint> matrix)
        {
            return matrix.CopyWindow(Col.DoubleFromUInt);
        }

        public static Matrix<byte> ToByteColor(this Matrix<float> matrix)
        {
            return matrix.CopyWindow(Col.ByteFromFloat);
        }

        public static Matrix<ushort> ToUShortColor(this Matrix<float> matrix)
        {
            return matrix.CopyWindow(Col.UShortFromFloat);
        }

        public static Matrix<uint> ToUIntColor(this Matrix<float> matrix)
        {
            return matrix.CopyWindow(Col.UIntFromFloat);
        }

        public static Matrix<double> ToDoubleColor(this Matrix<float> matrix)
        {
            return matrix.CopyWindow(Col.DoubleFromFloat);
        }

        public static Matrix<byte> ToByteColor(this Matrix<double> matrix)
        {
            return matrix.CopyWindow(Col.ByteFromDouble);
        }

        public static Matrix<ushort> ToUShortColor(this Matrix<double> matrix)
        {
            return matrix.CopyWindow(Col.UShortFromDouble);
        }

        public static Matrix<uint> ToUIntColor(this Matrix<double> matrix)
        {
            return matrix.CopyWindow(Col.UIntFromDouble);
        }

        public static Matrix<float> ToFloatColor(this Matrix<double> matrix)
        {
            return matrix.CopyWindow(Col.FloatFromDouble);
        }

        #endregion
    }
}
