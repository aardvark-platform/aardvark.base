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
        public static void Init()
        {
            PixImage<byte>.SetRemappedFun(Remapped);
            PixImage<ushort>.SetRemappedFun(Remapped);
            PixImage<float>.SetRemappedFun(Remapped);

            PixImage<byte>.SetRotatedFun(Rotated);
            PixImage<ushort>.SetRotatedFun(Rotated);
            PixImage<float>.SetRotatedFun(Rotated);

            PixImage<byte>.SetScaledFun(Scaled);
            PixImage<ushort>.SetScaledFun(Scaled);
            PixImage<float>.SetScaledFun(Scaled);
        }

        #region Compute Gradient

        public static Pair<Volume<float>> ComputeGradients(
                this Volume<float> src, EdgeFilter filterType)
        {
            var channels = src.Size.Z;
            var vdx = new Volume<float>(src.Info);
            var vdy = new Volume<float>(src.Info);

            for (int i = 0; i < channels; i++)
            {
                var gradients = src.SubXYMatrixWindow(i).ComputeGradients(filterType);
                vdx.SubXYMatrixWindow(i).Set(gradients.E0);
                vdy.SubXYMatrixWindow(i).Set(gradients.E1);
            }
            return Pair.Create(vdx, vdy);
        }


        public static Pair<Matrix<float>> ComputeGradients(
                this Matrix<float> src, EdgeFilter filterType)
        {
            #if USE_IPPI
            return src.ComputeGradientsIppi(filterType);
            #else
            throw new NotImplementedException();
            #endif
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

        #region Bilateral Filtering (byte, float) [CSharp]

        /// <summary>
        /// Returns this image volume blurred using a bilateral filter adapted from Alois Zingl's FilterMeister
        /// Photoshop plugin implementation.
        /// 
        /// rangeBlur is expected to be between 0 and 255 (inclusive).
        /// 
        /// In the case of four-channel images, processes and returns only the first three channels.
        /// </summary>
        public static Volume<byte> FilterBilateralCSharp(this Volume<byte> source, int spatialBlur, int rangeBlur)
        {
            if (rangeBlur < 0 || rangeBlur > 255)
                throw new ArgumentException("FilterBilateralCSharp: rangeBlur must be between 0 and 255.");

            #region Common Variables
            int numChannels = (source.SZ == 4 || source.SZ == 3) ? 3 : 1;

            double s = (10.0 * (double)spatialBlur / 7.4) + 0.9; // magic numbers from Zingl's implementation
            int blur = (int)Fun.Min(256.0, s * Fun.Sqrt(Fun.Log(256.0))) - 1; // blur box radius
            Border2l border = new Border2l((long)blur);

            int[] spatialBuffer = new int[(blur + 1) * 2 - 1];
            int[] rangeBuffer = new int[256];

            // populate spatialBuffer with spatial Gaussian bell curve
            s = -1.0 / (s * s);
            spatialBuffer[blur] = 256;
            for (int i = 1; i <= blur; i++)
            {
                spatialBuffer[blur + i] = (int)(256.0 * Fun.Exp(i * i * s));
                spatialBuffer[blur - i] = spatialBuffer[blur + i];
            }

            // populate rangeBuffer with range Gaussian bell curve (only half of the curve needed)
            s = (2.0 * (double)rangeBlur / 1.48) + 0.9;
            s = -1.0 / (s * s);
            for (int i = 0; i < 256; i++)
                rangeBuffer[i] = (int)(256.0 * Fun.Exp(i * i * s));

            int weight;
            Volume<byte> sourceVolume = source.SubVolumeWindow(V3l.Zero, new V3l(source.SX, source.SY, numChannels));
            var finalVolume = new V3l(source.SX, source.SY, numChannels).CreateImageVolume<byte>();
            #endregion

            if (numChannels == 3)
            {
                float cumulativeWeight, weightedR, weightedB, weightedG;
                C3b pixel, rangePixel;

                Matrix<byte, C3b> sourceC3bWindow = sourceVolume.SubMatrix<C3b>(V3l.Zero, sourceVolume.S.XY, sourceVolume.D.XY);
                sourceC3bWindow.Accessors = TensorAccessors.Get<byte, C3b>(typeof(byte), typeof(C3b), Col.Intent.BGR, sourceVolume.DeltaArray);

                var verticalSource = sourceC3bWindow.CopyWindow<C3b>(x => x);
                var verticalTarget = new Matrix<C3b>(sourceC3bWindow.S);
                var finalMatrix = new Matrix<C3b>(sourceC3bWindow.S);

                Matrix<byte, C3b> finalWindow = finalVolume.SubMatrix<C3b>(V3l.Zero, finalVolume.S.XY, finalVolume.D.XY);
                finalWindow.Accessors = TensorAccessors.Get<byte, C3b>(typeof(byte), typeof(C3b), Col.Intent.BGR, finalVolume.DeltaArray);

                var horizontalSource = verticalTarget.SubMatrixWindow(V2l.Zero, verticalTarget.S.YX, verticalTarget.D.YX); // transpose
                var horizontalTarget = finalMatrix.SubMatrixWindow(V2l.Zero, finalMatrix.S.YX, finalMatrix.D.YX); // transpose
                var curSourceMatrix = verticalSource;

                #region Filter Pass Delegate for C3b
                Action<Matrix<C3b>> pass = m => m.SetByCoord((x, y) =>
                {
                    cumulativeWeight = weightedR = weightedG = weightedB = 0f;
                    pixel = curSourceMatrix[x, y];

                    for (long i = Fun.Max(curSourceMatrix.FY, y - blur); i <= Fun.Min(curSourceMatrix.SY - 1, y + blur); i++)
                    {
                        rangePixel = curSourceMatrix[x, i];
                        weight = spatialBuffer[blur + (i - y)]
                            * rangeBuffer[Fun.Max(Fun.Abs(rangePixel.R - pixel.R),
                                                  Fun.Abs(rangePixel.G - pixel.G),
                                                  Fun.Abs(rangePixel.B - pixel.B))];

                        cumulativeWeight += weight;

                        weightedR += rangePixel.R * weight;
                        weightedG += rangePixel.G * weight;
                        weightedB += rangePixel.B * weight;
                    }

                    return new C3b(Convert.ToByte(weightedR / cumulativeWeight),
                        Convert.ToByte(weightedG / cumulativeWeight), Convert.ToByte(weightedB / cumulativeWeight));
                });
                #endregion

                #region Processing
                // carry out vertical pass
                verticalTarget.ApplyCenterBordersAndCorners(border,
                    pass,
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]));

                curSourceMatrix = horizontalSource;

                // carry out horizontal pass
                horizontalTarget.ApplyCenterBordersAndCorners(border,
                    pass,
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]));
                #endregion

                finalWindow.Set(finalMatrix);
                finalVolume.F = source.F;
            }
            else // single-channel image Volume
            {
                float cumulativeWeight, weightedIntensity;
                byte pixel, rangePixel;

                Matrix<byte> sourceByteWindow = sourceVolume.SubMatrix(V3l.Zero, sourceVolume.S.XY, sourceVolume.D.XY);

                var verticalSource = sourceByteWindow.CopyWindow(x => x);
                var verticalTarget = new Matrix<byte>(sourceByteWindow.S.XY);
                var finalMatrix = new Matrix<byte>(sourceByteWindow.S.XY);

                Matrix<byte> finalWindow = finalVolume.SubMatrix(V3l.Zero, finalVolume.S.XY, finalVolume.D.XY);

                var horizontalSource = verticalTarget.SubMatrixWindow(V2l.Zero, verticalTarget.S.YX, verticalTarget.D.YX); // transpose
                var horizontalTarget = finalMatrix.SubMatrixWindow(V2l.Zero, finalMatrix.S.YX, finalMatrix.D.YX); // transpose
                var curSourceMatrix = verticalSource;

                #region Filter Pass Delegate for byte
                Action<Matrix<byte>> pass = m => m.SetByCoord((x, y) =>
                {
                    cumulativeWeight = weightedIntensity = 0f;
                    pixel = curSourceMatrix[x, y];

                    for (long i = Fun.Max(curSourceMatrix.FY, y - blur); i <= Fun.Min(curSourceMatrix.SY - 1, y + blur); i++)
                    {
                        rangePixel = curSourceMatrix[x, i];
                        weight = spatialBuffer[blur + (i - y)] * rangeBuffer[Fun.Abs(rangePixel - pixel)];

                        cumulativeWeight += weight;

                        weightedIntensity += rangePixel * weight;
                    }

                    return Convert.ToByte(weightedIntensity / cumulativeWeight);
                });
                #endregion

                #region Processing
                // carry out vertical pass
                verticalTarget.ApplyCenterBordersAndCorners(border,
                    pass,
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]));

                curSourceMatrix = horizontalSource;

                // carry out horizontal pass
                horizontalTarget.ApplyCenterBordersAndCorners(border,
                    pass,
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]),
                    m => m.SetByCoord(c => curSourceMatrix[c]));
                #endregion

                finalWindow.Set(finalMatrix);
                finalVolume.F = source.F;
            }

            return finalVolume;
        }

        /// <summary>
        /// Returns this image volume blurred using a bilateral filter adapted from Alois Zingl's FilterMeister
        /// Photoshop plugin implementation.
        /// 
        /// rangeBlur is expected to be between 0.0 and 1.0 (inclusive).
        /// 
        /// In the case of four-channel images, processes and returns only the first three channels.
        /// </summary>
        public static Volume<byte> FilterBilateralCSharp(this Volume<byte> source, int spatialBlur, float rangeBlur)
        {
            if (rangeBlur < 0.0 || rangeBlur > 1.0)
                throw new ArgumentException("FilterBilateralCSharp: rangeBlur must be between 0.0 and 1.0.");

            return source.FilterBilateralCSharp(spatialBlur, (int)(rangeBlur * 255.0));
        }

        /// <summary>
        /// Returns this image volume blurred using a bilateral filter adapted from Alois Zingl's FilterMeister
        /// Photoshop plugin implementation.
        /// 
        /// rangeBlur is expected to be between 0.0 and 1.0 (inclusive).
        /// 
        /// In the case of four-channel images, processes and returns only the first three channels.
        /// </summary>
        public static Volume<float> FilterBilateralCSharp(this Volume<float> source, int spatialBlur, float rangeBlur)
        {
            if (rangeBlur < 0.0 || rangeBlur > 1.0)
                throw new ArgumentException("FilterBilateralCSharp: rangeBlur must be between 0.0 and 1.0.");

            var byteVolume = source.ToByteColor();
            var filteredByteVolume = byteVolume.FilterBilateralCSharp(spatialBlur, (int)(rangeBlur * 255.0));

            return filteredByteVolume.ToFloatColor();
        }

        #endregion

        #region Canny Edge Detection Matrix (byte, float) [Ippi]

        /// <summary>
        /// IPP-canny edge detector with gradients provided.
        /// </summary>
        /// <param name="gradXY">E0 contains horizontal gradients, E1 contains vertial gradients</param>
        /// <param name="low"></param>
        /// <param name="high"></param>
        /// <returns>Edges are white</returns>
        public static Matrix<byte> EdgeDetectionCanny(
                this Pair<Matrix<float>> gradXY, float low, float high)
        {
            #if USE_IPPI
            return gradXY.EdgeDetectionCannyIppi(low, high);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Applies gradient-filter on source-image and uses them as input for Canny edge detector.
        /// <param name="src">Input image will be converted to float values before further processing.</param>
        /// <param name="direction"></param>
        /// <param name="filter"></param>
        /// <param name="high"></param>
        /// <param name="low"></param>
        /// <returns>Edges are white</returns>
        /// </summary>
        public static Matrix<byte> EdgeDetectionCanny(this Matrix<byte> src,
                EdgeFilter filter, float low, float high,
                EdgeDirection direction = EdgeDirection.Both)
        {
            return src.ToFloatColor().EdgeDetectionCanny(filter, low, high, direction);
        }

        /// <summary>
        /// Applies gradient-filter on source-image and uses them as input for Canny edge detector.
        /// If gradients are already available, the function PixImageExtensions.EdgeDetectionCanny(gradX, gradY, ...) will be a better solution.
        /// <param name="filter">Type of gradient filter</param>
        /// <param name="direction">Which directon to be processed. Only one gradient image will be used if a value different from EdgeDirection.Both is used.</param>
        /// <param name="low">Low threshold of the hysteresis.</param>
        /// <param name="high">High threshold of the hysteresis.</param>
        /// <param name="src">the source matrix</param>
        /// </summary>
        public static Matrix<byte> EdgeDetectionCanny(this Matrix<float> src,
                EdgeFilter filter, float low, float high,
                EdgeDirection direction = EdgeDirection.Both)
        {
            var gradients = src.ComputeGradients(filter);
            if (direction == EdgeDirection.Both)
                return gradients.EdgeDetectionCanny(low, high);
            if (direction == EdgeDirection.Horizontal)
                return Pair.Create(gradients.E1, gradients.E1).EdgeDetectionCanny(low, high);
            if (direction == EdgeDirection.Vertical)
                return Pair.Create(gradients.E0, gradients.E0).EdgeDetectionCanny(low, high);
            throw new ArgumentException();
        }

        #endregion

        #region Data Range (Matrix, Volume) (byte, float) [CSharp, Ippi]

        public static Range1b DataRange(this Matrix<byte> source)
        {
            return source.AsVolume().DataRange();
        }

        public static Range1f DataRange(this Matrix<float> source)
        {
            return source.AsVolume().DataRange();
        }

        public static Range1b DataRange(this Volume<byte> source)
        {
            #if USE_IPPI
            return source.DataRangeIppi();
            #else
            var range = Range1b.Invalid;
            var data = source.Data;
            source.ForeachIndex(i => range.ExtendBy(data[i]));
            return range;
            #endif
        }

        public static Range1f DataRange(this Volume<float> source)
        {
            #if USE_IPPI
            return source.DataRangeIppi();
            #else
            var range = Range1f.Invalid;
            var data = source.Data;
            source.ForeachIndex(i => range.ExtendBy(data[i]));
            return range;
            #endif
        }

        #endregion

        #region Data Scaled (Matrix, Volume) (byte, float) [CSharp, Ippi]

        public static Matrix<byte> DataScaled(this Matrix<byte> source)
        {
            var range = source.DataRange();
            double scale = 1.0 / (range.Max - range.Min);
            return source.Copy(v => (byte)((v - range.Min) * scale));
        }

        public static Matrix<float> DataScaled(this Matrix<float> source)
        {
            var range = source.DataRange();
            double scale = 1.0 / (range.Max - range.Min);
            return source.Copy(v => (float)((v - range.Min) * scale));
        }

        public static Volume<byte> DataScaled(this Volume<byte> source)
        {
            var range = source.DataRange();
            double scale = 255.0 / (range.Max - range.Min);
            return source.CopyImage(v => (byte)((v - range.Min) * scale));
        }

        public static Volume<float> DataScaled(this Volume<float> source)
        {
            var range = source.DataRange();
            double scale = 1.0 / (range.Max - range.Min);
            return source.CopyImage(v => (float)((v - range.Min) * scale));
        }

        #endregion

        #region Gradient Magnitude (Matrix, Volume) (float) [CSharp]

        /// <summary>
        /// Compute gradient magnitudes from x and y gradients.
        /// </summary>
        /// <param name="gradXY">E0 contains x gradients, E1 contains y gradients</param>
        public static Matrix<float> GradientMagnitude(this Pair<Matrix<float>> gradXY)
        {
            return Matrix<float>.Create(gradXY.E0, gradXY.E1, (vx, vy) => Fun.Sqrt(vx * vx + vy * vy));
        }

        /// <summary>
        /// Compute gradient magnitudes from x and y gradients.
        /// </summary>
        /// <param name="gradXY">E0 contains x gradients, E1 contains y gradients</param>
        public static Volume<float> GradientMagnitude(this Pair<Volume<float>> gradXY)
        {
            return Volume<float>.Create(gradXY.E0, gradXY.E1, (vx, vy) => Fun.Sqrt(vx * vx + vy * vy));
        }

        #endregion

        #region Normalization (float, double) [CSharp]

        /// <summary>
        /// Normalizes the supplied matrix so that the sum of all Abs entries sums to 1. MODIFIES SOURCE!
        /// </summary>
        /// <returns>this</returns>
        public static Matrix<float> NormalizeSum(this Matrix<float> matrixWillBeModified)
        {
            var sum = matrixWillBeModified.Norm(x => x, 0.0, (s, x) => s + (double)System.Math.Abs(x));
            if (sum != 0.0)
            {
                double scale = 1.0 / sum;
                matrixWillBeModified.Apply(x => (float)(scale * x));
            }
            return matrixWillBeModified;
        }

        /// <summary>
        /// Normalizes the supplied matrix so that the sum of all Abs entries sums to 1. MODIFIES SOURCE!
        /// </summary>
        /// <returns>this</returns>
        public static Matrix<double> NormalizeSum(this Matrix<double> matrixWillBeModified)
        {

            var sum = matrixWillBeModified.Norm(x => x, KahanSum.Zero, (s, x) => s + System.Math.Abs(x));
            if (sum.Value != 0.0)
            {
                double scale = 1.0 / sum.Value;
                matrixWillBeModified.Apply(x => scale * x);
            }
            return matrixWillBeModified;
        }

        #endregion

        #region Difference of Gaussians (float) [Ippi]

        public static Matrix<float> DoGFiltered(
                this Matrix<float> source,
                float sigma0, float sigma1,
                int kernelSize0 = 0, int kernelSize1 = 0,
                ImageBorderType borderType = ImageBorderType.Mirror)
        {
            #if USE_IPPI
            if (kernelSize0 == 0) kernelSize0 = (int)(6 * sigma0);
            if (kernelSize1 == 0) kernelSize1 = (int)(6 * sigma1);
            return source.DoGFilteredIppi(sigma0, sigma1, kernelSize0, kernelSize1, borderType);
            #else
            throw new NotImplementedException();
            #endif
        }

        #endregion

        #region Gaussian Filtered (Matrix, Volume) (byte, float) [Ippi]

        /// <summary>
        /// Applies Gauss filter with border. If no kernel size is specified,
        /// 6 times sigma is used. If no image border type is specified,
        /// ImageBorderType.Mirror is used.
        /// </summary>
        public static Matrix<byte> GaussianFiltered(
                this Matrix<byte> source, float sigma,
                int kernelSize = 0,
                ImageBorderType borderType = ImageBorderType.Mirror)
        {
            return source.ToFloatColor().GaussianFiltered(sigma, kernelSize, borderType).ToByteColor();
        }



        /// <summary>
        /// Applies Gauss filter with border. If no kernel size is specified,
        /// 6 times sigma is used. If no image border type is specified,
        /// ImageBorderType.Mirror is used.
        /// </summary>
        public static Matrix<float> GaussianFiltered(
                this Matrix<float> source, float sigma,
                int kernelSize = 0,
                ImageBorderType borderType = ImageBorderType.Mirror)
        {
            #if USE_IPPI
            if (kernelSize == 0) kernelSize = (int)(6 * sigma);
            return source.GaussianFilteredIppi(sigma, kernelSize, borderType);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Applies Gauss filter with border. If no kernel size is specified,
        /// 6 times sigma is used. If no image border type is specified,
        /// ImageBorderType.Mirror is used.
        /// </summary>
        public static Volume<byte> GaussianFiltered(
                this Volume<byte> source, float sigma,
                int kernelSize = 0,
                ImageBorderType borderType = ImageBorderType.Mirror)
        {
            return source.ToFloatColor().GaussianFiltered(sigma, kernelSize, borderType).ToByteColor();
        }

        /// <summary>
        /// Applies Gauss filter with border. If no kernel size is specified,
        /// 6 times sigma is used. If no image border type is specified,
        /// ImageBorderType.Mirror is used.
        /// </summary>
        public static Volume<float> GaussianFiltered(
                this Volume<float> source, float sigma,
                int kernelSize = 0,
                ImageBorderType borderType = ImageBorderType.Mirror)
        {
            if (source.SZ == 1)
                return source.AsMatrixWindow().GaussianFiltered(sigma, kernelSize, borderType).AsVolume();
            else
            {
                var target = new Volume<float>(source.Info);
                for (long i = 0; i < source.SZ; i++)
                {
                    var filteredChannel = source.SubXYMatrix(i).ToImage()
                                            .GaussianFiltered(sigma, kernelSize, borderType);
                    target.SubXYMatrix(i).Set(filteredChannel);
                }
                return target;
            }
        }

        #endregion

        #region General Filtered (Matrix, Volume) (byte, float) [Ippi]

        /// <summary>
        /// Perform convolution of this image matrix with a matrix kernel.
        /// </summary>
        public static Matrix<byte> GeneralFiltered(
                this Matrix<byte> source, Matrix<float> kernel)
        {
            return source.AsVolume().GeneralFiltered(kernel)
                                    .AsMatrixWindow();
        }

        /// <summary>
        /// Perform convolution of this image matrix with a matrix kernel.
        /// </summary>
        public static Matrix<float> GeneralFiltered(
                this Matrix<float> source, Matrix<float> kernel)
        {
            return source.AsVolume().GeneralFiltered(kernel)
                                    .AsMatrixWindow();
        }

        /// <summary>
        /// Perform convolution of this image volume with a matrix kernel.
        /// </summary>
        public static Volume<byte> GeneralFiltered(
                this Volume<byte> source, Matrix<float> kernel)
        {
            #if USE_IPPI
            return source.GeneralFilteredIppi(kernel);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Perform convolution of this image volume with a matrix kernel.
        /// </summary>
        public static Volume<float> GeneralFiltered(
                this Volume<float> source, Matrix<float> kernel)
        {
            #if USE_IPPI
            return source.GeneralFilteredIppi(kernel);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Perform convolution of this image volume with a vector kernel
        /// (kernelX) in the horizontal direction followed by a vector
        /// kernel (kernelY) in the vertical direction.
        /// </summary>
        public static Volume<byte> GeneralFiltered(
                this Volume<byte> source,
                Vector<float> kernelX, Vector<float> kernelY)
        {
            return GeneralFilteredY(GeneralFilteredX(source, kernelX), kernelY);
        }

        /// <summary>
        /// Perform convolution of this image volume with a horizontal
        /// vector kernel.
        /// </summary>
        public static Volume<byte> GeneralFilteredX(
                this Volume<byte> source, Vector<float> kernelX)
        {
            return GeneralFiltered(source, kernelX.To2DKernelX());
        }

        /// <summary>
        /// Perform convolution of this image volume with a vertical
        /// vector kernel.
        /// </summary>
        public static Volume<byte> GeneralFilteredY(
                this Volume<byte> source, Vector<float> kernelY)
        {
            return GeneralFiltered(source, kernelY.To2DKernelY());
        }

        #endregion

        #region Median Filtered (Matrix, Volume) (byte) [Ippi]

        /// <summary>
        /// Median Filter.
        /// </summary>
        public static Matrix<byte> MedianFiltered(this Matrix<byte> source, V2i filterSize, V2i point)
        {
            return source.AsVolume().MedianFiltered(filterSize, point)
                                    .AsMatrixWindow();
        }

        /// <summary>
        /// Median Filter.
        /// </summary>
        public static Volume<byte> MedianFiltered(this Volume<byte> source, V2i filterSize, V2i point)
        {
            #if USE_IPPI
            return source.MedianFilteredIppi(filterSize, point);
            #else
            throw new NotImplementedException();
            #endif
        }

        #endregion

        #region Morphological Operations (Matrix, Volume) (byte, float) [Ippi]

        /// <summary>
        /// Returns the original dilated, then eroded. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Matrix<byte> MorphClosed(this Matrix<byte> source, int maskSize)
        {
            return source.AsVolume().MorphClosed(maskSize)
                                    .AsMatrixWindow();
        }
        
        /// <summary>
        /// Returns the original dilated, then eroded. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Matrix<float> MorphClosed(this Matrix<float> source, int maskSize)
        {
            return source.AsVolume().MorphClosed(maskSize)
                                    .AsMatrixWindow();
        }
        
        /// <summary>
        /// Returns the original eroded, then dilated. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Matrix<byte> MorphOpened(this Matrix<byte> source, int maskSize)
        {
            return source.AsVolume().MorphOpened(maskSize)
                                    .AsMatrixWindow();
        }

        /// <summary>
        /// Returns the original eroded, then dilated. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Matrix<float> MorphOpened(this Matrix<float> source, int maskSize)
        {
            return source.AsVolume().MorphOpened(maskSize)
                                    .AsMatrixWindow();
        }

        /// <summary>
        /// Returns the original dilated. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Matrix<byte> MorphDilated(this Matrix<byte> source, int maskSize)
        {
            return source.AsVolume().MorphDilated(maskSize)
                                    .AsMatrixWindow();
        }

        /// <summary>
        /// Returns the original dilated. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Matrix<float> MorphDilated(this Matrix<float> source, int maskSize)
        {
            return source.AsVolume().MorphDilated(maskSize)
                                    .AsMatrixWindow();
        }

        /// <summary>
        /// Returns the original eroded. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Matrix<byte> MorphEroded(this Matrix<byte> source, int maskSize)
        {
            return source.AsVolume().MorphEroded(maskSize)
                                    .AsMatrixWindow();
        }

        /// <summary>
        /// Returns the original eroded. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Matrix<float> MorphEroded(this Matrix<float> source, int maskSize)
        {
            return source.AsVolume().MorphEroded(maskSize)
                                    .AsMatrixWindow();
        }
        
        /// <summary>
        /// Returns the original dilated, then eroded. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Volume<byte> MorphClosed(this Volume<byte> source, int maskSize)
        {
            #if USE_IPPI
            return source.MorphClosedIppi(maskSize);
            #else
            throw new NotImplementedException();
            #endif
        }
        
        /// <summary>
        /// Returns the original dilated, then eroded. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Volume<float> MorphClosed(this Volume<float> source, int maskSize)
        {
            #if USE_IPPI
            return source.MorphClosedIppi(maskSize);
            #else
            throw new NotImplementedException();
            #endif
        }
        
        /// <summary>
        /// Returns the original eroded, then dilated. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Volume<byte> MorphOpened(this Volume<byte> source, int maskSize)
        {
            #if USE_IPPI
            return source.MorphOpenedIppi(maskSize);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Returns the original eroded, then dilated. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Volume<float> MorphOpened(this Volume<float> source, int maskSize)
        {
            #if USE_IPPI
            return source.MorphOpenedIppi(maskSize);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Returns the original dilated. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Volume<byte> MorphDilated(this Volume<byte> source, int maskSize)
        {
            #if USE_IPPI
            return source.MorphDilatedIppi(maskSize);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Returns the original dilated. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Volume<float> MorphDilated(this Volume<float> source, int maskSize)
        {
            #if USE_IPPI
            return source.MorphDilatedIppi(maskSize);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Returns the original eroded. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Volume<byte> MorphEroded(this Volume<byte> source, int maskSize)
        {
            #if USE_IPPI
            return source.MorphErodedIppi(maskSize);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Returns the original eroded, then dialated. The original is not
        /// modified. The supplied maskSize defines the size of the square
        /// mask around each pixel.
        /// </summary>
        public static Volume<float> MorphEroded(this Volume<float> source, int maskSize)
        {
            #if USE_IPPI
            return source.MorphErodedIppi(maskSize);
            #else
            throw new NotImplementedException();
            #endif
        }
        
        #endregion

        #region Remapped (Matrix, Volume) (byte, ushort, float) [Ippi]

        public static Matrix<byte> Remapped(
                this Matrix<byte> source,
                Matrix<float> mapx, Matrix<float> mapy,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Remapped(mapx, mapy, ip).AsMatrixWindow();
        }

        public static Matrix<ushort> Remapped(
                this Matrix<ushort> source,
                Matrix<float> mapx, Matrix<float> mapy,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Remapped(mapx, mapy, ip).AsMatrixWindow();
        }

        public static Matrix<float> Remapped(
                this Matrix<float> source,
                Matrix<float> mapx, Matrix<float> mapy,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Remapped(mapx, mapy, ip).AsMatrixWindow();
        }

        public static Volume<byte> Remapped(
                this Volume<byte> source,
                Matrix<float> mapx, Matrix<float> mapy,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return source.RemappedIppi(mapx, mapy, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        public static Volume<ushort> Remapped(
                this Volume<ushort> source,
                Matrix<float> mapx, Matrix<float> mapy,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return source.RemappedIppi(mapx, mapy, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        public static Volume<float> Remapped(
                this Volume<float> source,
                Matrix<float> mapx, Matrix<float> mapy,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return source.RemappedIppi(mapx, mapy, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        private static Dictionary<Type, Func<object, Matrix<float>, Matrix<float>, ImageInterpolation, object>>
            s_remappedFunMap =
            new Dictionary<Type, Func<object, Matrix<float>, Matrix<float>, ImageInterpolation, object>>()
            {
                { typeof(byte), (v, x, y, i) => ((Volume<byte>)v).Remapped(x, y, i) },
                { typeof(ushort), (v, x, y, i) => ((Volume<ushort>)v).Remapped(x, y, i) },
                { typeof(float), (v, x, y, i) => ((Volume<float>)v).Remapped(x, y, i) },
            };

        public static Volume<T> Remapped<T>(
                this Volume<T> source,
                Matrix<float> xMap, Matrix<float> yMap,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return (Volume<T>)s_remappedFunMap[typeof(T)](source, xMap, yMap, ip);
        }

        public static Volume<byte> Remapped(
                this Volume<byte>[] sources,
                Matrix<float>[] xMaps, Matrix<float>[] yMaps,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return sources.RemappedIppi(xMaps, yMaps, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        public static Volume<ushort> Remapped(
                this Volume<ushort>[] sources,
                Matrix<float>[] xMaps, Matrix<float>[] yMaps,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return sources.RemappedIppi(xMaps, yMaps, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        public static Volume<float> Remapped(
                this Volume<float>[] sources,
                Matrix<float>[] xMaps, Matrix<float>[] yMaps,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return sources.RemappedIppi(xMaps, yMaps, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        private static Dictionary<Type, Func<object, Matrix<float>[], Matrix<float>[], ImageInterpolation, object>>
            s_remappedArrayFunMap = new Dictionary<Type, Func<object, Matrix<float>[], Matrix<float>[], ImageInterpolation, object>>
            {
                { typeof(byte), (v, x, y, i) => ((Volume<byte>[])v).Remapped(x, y, i) },
                { typeof(ushort), (v, x, y, i) => ((Volume<ushort>[])v).Remapped(x, y, i) },
                { typeof(float), (v, x, y, i) => ((Volume<float>[])v).Remapped(x, y, i) },
            };

        public static Volume<T> Remapped<T>(
                this Volume<T>[] sources,
                Matrix<float>[] xMaps, Matrix<float>[] yMaps,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return (Volume<T>)s_remappedArrayFunMap[typeof(T)](sources, xMaps, yMaps, ip);
        }

        #endregion

        #region Resized (Matrix, Volume) (byte, ushort, float) [Ippi]

        public static Matrix<byte> Resized(
                this Matrix<byte> source, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Resized(targetSize, ip)
                                    .AsMatrixWindow();
        }

        public static Matrix<ushort> Resized(
                this Matrix<ushort> source, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Resized(targetSize, ip)
                                    .AsMatrixWindow();
        }

        public static Matrix<float> Resized(
                this Matrix<float> source, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Resized(targetSize, ip)
                                    .AsMatrixWindow();
        }

        public static Volume<byte> Resized(
                this Volume<byte> source, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.Scaled((V2d)targetSize.XY / (V2d)source.Size.XY, ip);
        }

        public static Volume<ushort> Resized(
                this Volume<ushort> source, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.Scaled((V2d)targetSize.XY / (V2d)source.Size.XY, ip);
        }

        public static Volume<float> Resized(
                this Volume<float> source, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.Scaled((V2d)targetSize.XY / (V2d)source.Size.XY, ip);
        }

        #endregion

        #region Rotated (Matrix, Volume) (byte, ushort, float) [Ippi]

        public static Matrix<byte> Rotated(
                this Matrix<byte> source, double angleInRadiansCCW,
                bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Rotated(angleInRadiansCCW, resize, ip)
                                    .AsMatrixWindow();
        }

        public static Matrix<ushort> Rotated(
                this Matrix<ushort> source, double angleInRadiansCCW,
                bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Rotated(angleInRadiansCCW, resize, ip)
                                    .AsMatrixWindow();
        }

        public static Matrix<float> Rotated(
                this Matrix<float> source, double angleInRadiansCCW,
                bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Rotated(angleInRadiansCCW, resize, ip)
                                    .AsMatrixWindow();
        }

        public static Volume<byte> Rotated(
                this Volume<byte> source, double angleInRadiansCCW,
                bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return source.RotatedIppi(angleInRadiansCCW, resize, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        public static Volume<ushort> Rotated(
                this Volume<ushort> source, double angleInRadiansCCW,
                bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return source.RotatedIppi(angleInRadiansCCW, resize, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        public static Volume<float> Rotated(
                this Volume<float> source, double angleInRadiansCCW,
                bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return source.RotatedIppi(angleInRadiansCCW, resize, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        private static Dictionary<Type, Func<object, double, bool, ImageInterpolation, object>>
            s_rotatedFunMap =
            new Dictionary<Type, Func<object, double, bool, ImageInterpolation, object>>()
            {
                { typeof(byte), (v, a, r, ip) => ((Volume<byte>)v).Rotated(a, r, ip) },
                { typeof(ushort), (v, a, r, ip) => ((Volume<ushort>)v).Rotated(a, r, ip) },
                { typeof(float), (v, a, r, ip) => ((Volume<float>)v).Rotated(a, r, ip) },
            };

        public static Volume<T> Rotated<T>(
                this Volume<T> source, double angleInRadiansCCW,
                bool resize = true, ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return (Volume<T>)s_rotatedFunMap[typeof(T)](
                    source, angleInRadiansCCW, resize, ip);
        }

        #endregion

        #region Scaled (Matrix, Volume) (byte, ushort, float) [Ippi]

        public static Matrix<byte> Scaled(
                this Matrix<byte> source, V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Scaled(scaleFactor, ip)
                                    .AsMatrix();
        }

        public static Matrix<ushort> Scaled(
                this Matrix<ushort> source, V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Scaled(scaleFactor, ip)
                                    .AsMatrix();
        }

        public static Matrix<float> Scaled(
                this Matrix<float> source, V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Scaled(scaleFactor, ip)
                                    .AsMatrix();
        }

        public static Matrix<T> Scaled<T>(
                this Matrix<T> source, V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().Scaled(scaleFactor, ip)
                                    .AsMatrix();
        }

        public static Volume<byte> Scaled(
                this Volume<byte> source, V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            var targetSize = (new V2d(0.5, 0.5) + scaleFactor * (V2d)source.Size.XY).ToV2l();
            return source.ScaledAndShifted(scaleFactor, V2d.Zero, targetSize, ip);
        }

        public static Volume<ushort> Scaled(
                this Volume<ushort> source, V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            var targetSize = (new V2d(0.5, 0.5) + scaleFactor * (V2d)source.Size.XY).ToV2l();
            return source.ScaledAndShifted(scaleFactor, V2d.Zero, targetSize, ip);
        }

        public static Volume<float> Scaled(
                this Volume<float> source, V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            var targetSize = (new V2d(0.5, 0.5) + scaleFactor * (V2d)source.Size.XY).ToV2l();
            return source.ScaledAndShifted(scaleFactor, V2d.Zero, targetSize, ip);
        }

        public static Volume<T> Scaled<T>(
                this Volume<T> source, V2d scaleFactor,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            var targetSize = (new V2d(0.5, 0.5) + scaleFactor * (V2d)source.Size.XY).ToV2l();
            return (Volume<T>)s_scaledAndShiftedFunMap[typeof(T)](
                                    source, scaleFactor, V2d.Zero, targetSize, ip);
        }

        #endregion

        #region ScaledAndShifted (Matrix, Volume) (byte, ushort, float) [Ippi]

        public static Matrix<byte> ScaledAndShifted(
                this Matrix<byte> source, V2d scaleFactor, V2d shift, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().ScaledAndShifted(scaleFactor, shift, targetSize, ip).AsMatrix();
        }

        public static Matrix<ushort> ScaledAndShifted(
                this Matrix<ushort> source, V2d scaleFactor, V2d shift, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().ScaledAndShifted(scaleFactor, shift, targetSize, ip).AsMatrix();
        }

        public static Matrix<float> ScaledAndShifted(
                this Matrix<float> source, V2d scaleFactor, V2d shift, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().ScaledAndShifted(scaleFactor, shift, targetSize, ip).AsMatrix();
        }

        public static Matrix<T> ScaledAndShifted<T>(
                this Matrix<T> source, V2d scaleFactor, V2d shift, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return source.AsVolume().ScaledAndShifted(scaleFactor, shift, targetSize, ip).AsMatrix();
        }

        public static Volume<byte> ScaledAndShifted(
                this Volume<byte> source, V2d scaleFactor, V2d shift, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI

            // The Intel® IPP Resize function ippiResizeSqrPixel crashed when the image size is less than 6 
            // and use interpolation method LANCZOS or CUBIC.
            // http://software.intel.com/en-us/articles/resize-function-ippiresizesqrpixel-crashed-for-small-image/
            if ((source.Size.X < 6) || (source.Size.Y < 6))
                ip = ImageInterpolation.Linear;

            return source.ScaledAndShiftedIppi(scaleFactor, shift, targetSize, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        public static Volume<ushort> ScaledAndShifted(
                this Volume<ushort> source, V2d scaleFactor, V2d shift, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return source.ScaledAndShiftedIppi(scaleFactor, shift, targetSize, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        public static Volume<float> ScaledAndShifted(
                this Volume<float> source, V2d scaleFactor, V2d shift, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            return source.ScaledAndShiftedIppi(scaleFactor, shift, targetSize, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        private static Dictionary<Type, Func<object, V2d, V2d, V2l, ImageInterpolation, object>>
            s_scaledAndShiftedFunMap = new Dictionary<Type, Func<object, V2d, V2d, V2l, ImageInterpolation, object>>
            {
                { typeof(byte), (v, f, t, s, i) => ((Volume<byte>)v).ScaledAndShifted(f, t, s, i) },
                { typeof(ushort), (v, f, t, s, i) => ((Volume<ushort>)v).ScaledAndShifted(f, t, s, i) },
                { typeof(float), (v, f, t, s, i) => ((Volume<float>)v).ScaledAndShifted(f, t, s, i) },
            };

        public static Volume<T> ScaledAndShifted<T>(
                this Volume<T> source, V2d scaleFactor, V2d shift, V2l targetSize,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            return (Volume<T>)s_scaledAndShiftedFunMap[typeof(T)](
                                    source, scaleFactor, shift, targetSize, ip);
        }

        #endregion

        #region WarpAffine Volume (byte, ushort, float) [Ippi]

        /// <summary>
        /// Performs general affine transform of the source image.
        /// <param name="trafo">The affine transform coefficients.</param>
        /// <param name="ip">Specifies the interpolation mode. Possible values are: Near, Linear, Cubic, SmoothEdge.</param>
        /// <param name="source">the source volume</param>
        /// <param name="target">the target volume</param>
        /// </summary>
        public static void WarpAffine(
                this Volume<byte> source, Volume<byte> target, M23d trafo,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            source.WarpAffineIppi(target, trafo, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Performs general affine transform of the source image.
        /// <param name="trafo">The affine transform coefficients.</param>
        /// <param name="ip">Specifies the interpolation mode. Possible values are: Near, Linear, Cubic, SmoothEdge.</param>
        /// <param name="source">the source volume</param>
        /// <param name="target">the target volume</param>
        /// </summary>
        public static void WarpAffine(
                this Volume<ushort> source, Volume<ushort> target, M23d trafo,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            source.WarpAffineIppi(target, trafo, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        /// <summary>
        /// Performs general affine transform of the source image.
        /// <param name="trafo">The affine transform coefficients.</param>
        /// <param name="ip">Specifies the interpolation mode. Possible values are: Near, Linear, Cubic, SmoothEdge.</param>
        /// <param name="source">the source volume</param>
        /// <param name="target">the target volume</param>
        /// </summary>
        public static void WarpAffine(
                this Volume<float> source, Volume<float> target, M23d trafo,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            #if USE_IPPI
            source.WarpAffineIppi(target, trafo, ip);
            #else
            throw new NotImplementedException();
            #endif
        }

        private static Dictionary<Type, Action<object, object, M23d, ImageInterpolation>>
            s_warpAffineActMap = new Dictionary<Type, Action<object, object, M23d, ImageInterpolation>>
            {
                { typeof(byte), (s, t, m, i) => ((Volume<byte>)s).WarpAffine((Volume<byte>)t, m, i) },
                { typeof(ushort), (s, t, m, i) => ((Volume<ushort>)s).WarpAffine((Volume<ushort>)t, m, i) },
                { typeof(float), (s, t, m, i) => ((Volume<float>)s).WarpAffine((Volume<float>)t, m, i) },
            };

        /// <summary>
        /// Performs general affine transform of the source image.
        /// <param name="trafo">The affine transform coefficients.</param>
        /// <param name="ip">Specifies the interpolation mode. Possible values are: Near, Linear, Cubic, SmoothEdge.</param>
        /// <param name="source">the source volume</param>
        /// <param name="target">the target volume</param>
        /// </summary>
        public static void WarpAffine<T>(
                this Volume<T> source, Volume<T> target, M23d trafo,
                ImageInterpolation ip = ImageInterpolation.Cubic)
        {
            s_warpAffineActMap[typeof(T)](source, target, trafo, ip);
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
