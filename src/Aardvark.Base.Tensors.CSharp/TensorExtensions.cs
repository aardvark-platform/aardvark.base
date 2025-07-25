/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Aardvark.Base;

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
        if (matrix is Matrix<byte, C3b> matrix1)
            return matrix1.ToPixImage<T, byte>(); ;

        var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Dim);
        pixImage.GetMatrix<C3b>().Set(matrix);
        return pixImage;
    }

    public static PixImage<T> ToPixImage<T>(this IMatrix<C3us> matrix)
    {
        if (matrix is Matrix<ushort, C3us> m)
            return m.ToPixImage<T, ushort>(); ;

        var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Dim);
        pixImage.GetMatrix<C3us>().Set(matrix);
        return pixImage;
    }

    public static PixImage<T> ToPixImage<T>(this IMatrix<C3f> matrix)
    {
        if (matrix is Matrix<float, C3f> m)
            return m.ToPixImage<T, float>(); ;

        var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 3), (V2i)matrix.Dim);
        pixImage.GetMatrix<C3f>().Set(matrix);
        return pixImage;
    }

    public static PixImage<T> ToPixImage<T>(this IMatrix<C4b> matrix)
    {
        if (matrix is Matrix<byte, C4b> m)
            return m.ToPixImage<T, byte>(); ;

        var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Dim);
        pixImage.GetMatrix<C4b>().Set(matrix);
        return pixImage;
    }

    public static PixImage<T> ToPixImage<T>(this IMatrix<C4us> matrix)
    {
        if (matrix is Matrix<ushort, C4us> m)
            return m.ToPixImage<T, ushort>(); ;

        var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Dim);
        pixImage.GetMatrix<C4us>().Set(matrix);
        return pixImage;
    }

    public static PixImage<T> ToPixImage<T>(this IMatrix<C4f> matrix)
    {
        if (matrix is Matrix<float, C4f> m)
            return m.ToPixImage<T, float>(); ;

        var pixImage = new PixImage<T>(Col.FormatDefaultOf(typeof(T), 4), (V2i)matrix.Dim);
        pixImage.GetMatrix<C4f>().Set(matrix);
        return pixImage;
    }

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
        return trafo switch
        {
            ImageTrafo.Identity => info,
            ImageTrafo.Rot90 => info.SubMatrix(sx - 1, 0, sy, sx, dy, -dx),
            ImageTrafo.Rot180 => info.SubMatrix(sx - 1, sy - 1, sx, sy, -dx, -dy),
            ImageTrafo.Rot270 => info.SubMatrix(0, sy - 1, sy, sx, -dy, dx),
            ImageTrafo.MirrorX => info.SubMatrix(sx - 1, 0, sx, sy, -dx, dy),
            ImageTrafo.Transpose => info.SubMatrix(0, 0, sy, sx, dy, dx),
            ImageTrafo.MirrorY => info.SubMatrix(0, sy - 1, sx, sy, dx, -dy),
            ImageTrafo.Transverse => info.SubMatrix(sx - 1, sy - 1, sy, sx, -dy, -dx),
            _ => throw new ArgumentException(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Matrix<T> Transformed<T>(this Matrix<T> matrix, ImageTrafo trafo)
        => new(matrix.Data, matrix.Info.Transformed(trafo));

    public static VolumeInfo Transformed(this VolumeInfo info, ImageTrafo trafo)
    {
        long sx = info.Size.X, sy = info.Size.Y, sz = info.Size.Z;
        long dx = info.Delta.X, dy = info.Delta.Y, dz = info.Delta.Z;
        return trafo switch
        {
            ImageTrafo.Identity => info,
            ImageTrafo.Rot90 => info.SubVolume(sx - 1, 0, 0, sy, sx, sz, dy, -dx, dz),
            ImageTrafo.Rot180 => info.SubVolume(sx - 1, sy - 1, 0, sx, sy, sz, -dx, -dy, dz),
            ImageTrafo.Rot270 => info.SubVolume(0, sy - 1, 0, sy, sx, sz, -dy, dx, dz),
            ImageTrafo.MirrorX => info.SubVolume(sx - 1, 0, 0, sx, sy, sz, -dx, dy, dz),
            ImageTrafo.Transpose => info.SubVolume(0, 0, 0, sy, sx, sz, dy, dx, dz),
            ImageTrafo.MirrorY => info.SubVolume(0, sy - 1, 0, sx, sy, sz, dx, -dy, dz),
            ImageTrafo.Transverse => info.SubVolume(sx - 1, sy - 1, 0, sy, sx, sz, -dy, -dx, dz),
            _ => throw new ArgumentException(),
        };
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Volume<T> Transformed<T>(this Volume<T> volume, ImageTrafo trafo)
        => new(volume.Data, volume.Info.Transformed(trafo));

    #endregion

    #region Scaling

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Half LerpHalf(double t, Half x, Half y) => Fun.Lerp((Half)t, x, y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static float LerpFloat(double t, float x, float y) => Fun.Lerp((float)t, x, y);

    private static readonly Dictionary<Type, Action<object, object>> scaleNearestTable =
        new()
        {
            {typeof(byte),   (src, dst) => ((Matrix<byte>)dst).SetScaledNearest((Matrix<byte>)src)},
            {typeof(ushort), (src, dst) => ((Matrix<ushort>)dst).SetScaledNearest((Matrix<ushort>)src)},
            {typeof(uint),   (src, dst) => ((Matrix<uint>)dst).SetScaledNearest((Matrix<uint>)src)},
            {typeof(Half),   (src, dst) => ((Matrix<Half>)dst).SetScaledNearest((Matrix<Half>)src)},
            {typeof(float),  (src, dst) => ((Matrix<float>)dst).SetScaledNearest((Matrix<float>)src)},
            {typeof(double), (src, dst) => ((Matrix<double>)dst).SetScaledNearest((Matrix<double>)src)}
        };

    private static readonly Dictionary<Type, Action<object, object>> scaleLinearTable =
        new()
        {
            {typeof(byte),   (src, dst) => ((Matrix<byte>)dst).SetScaledLinear((Matrix<byte>)src, Fun.Lerp, Fun.Lerp)},
            {typeof(ushort), (src, dst) => ((Matrix<ushort>)dst).SetScaledLinear((Matrix<ushort>)src, Fun.Lerp, Fun.Lerp)},
            {typeof(uint),   (src, dst) => ((Matrix<uint>)dst).SetScaledLinear((Matrix<uint>)src, Fun.Lerp, Fun.Lerp)},
            {typeof(Half),   (src, dst) => ((Matrix<Half>)dst).SetScaledLinear((Matrix<Half>)src, LerpHalf, LerpHalf)},
            {typeof(float),  (src, dst) => ((Matrix<float>)dst).SetScaledLinear((Matrix<float>)src, LerpFloat, LerpFloat)},
            {typeof(double), (src, dst) => ((Matrix<double>)dst).SetScaledLinear((Matrix<double>)src, Fun.Lerp, Fun.Lerp)}
        };

    private static readonly Dictionary<Type, Action<object, object>> scaleCubicTable =
        new()
        {
            {typeof(byte),   (src, dst) => ((Matrix<byte>)dst).SetScaledCubic((Matrix<byte>)src)},
            {typeof(ushort), (src, dst) => ((Matrix<ushort>)dst).SetScaledCubic((Matrix<ushort>)src)},
            {typeof(uint),   (src, dst) => ((Matrix<uint>)dst).SetScaledCubic((Matrix<uint>)src)},
            {typeof(Half),   (src, dst) => ((Matrix<Half>)dst).SetScaledCubic((Matrix<Half>)src)},
            {typeof(float),  (src, dst) => ((Matrix<float>)dst).SetScaledCubic((Matrix<float>)src)},
            {typeof(double), (src, dst) => ((Matrix<double>)dst).SetScaledCubic((Matrix<double>)src)}
        };

    private static readonly Dictionary<Type, Action<object, object>> scaleLanczosTable =
        new()
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
