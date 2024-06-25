using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    public interface IPixVolumeVisitor<T>
    {
        T Visit<TData>(PixVolume<TData> volume);
    }

    public record PixVolumeInfo(PixFormat Format, V3i Size);

    public abstract class PixVolume
    {
        public Col.Format Format;

        #region Constructors

        public PixVolume()
            : this(Col.Format.None)
        { }

        public PixVolume(Col.Format format)
        {
            Format = format;
        }

        #endregion

        #region Properties

        public abstract Array Data { get; }

        public abstract PixFormat PixFormat { get; }

        public abstract Tensor4Info Tensor4Info { get; }

        public abstract int BytesPerChannel { get; }

        public PixVolumeInfo Info => new PixVolumeInfo(PixFormat, Size);

        public V3i Size => (V3i)Tensor4Info.Size.XYZ;

        public V3l SizeL => Tensor4Info.Size.XYZ;

        public int ChannelCount => (int)Tensor4Info.Size.W;

        public long ChannelCountL => Tensor4Info.Size.W;

        #region Obsolete

        [Obsolete("Use Data instead.")]
        public Array Array => Data;

        #endregion

        #endregion

        #region Static Creator Functions

        public static Tensor4<T> CreateTensor4<T>(V4l size)
        {
            return size.CreateImageTensor4<T>();
        }

        public static Tensor4<T> CreateTensor4<T>(long sizeX, long sizeY, long sizeZ, long channelCount)
        {
            return new V4l(sizeX, sizeY, sizeZ, channelCount).CreateImageTensor4<T>();
        }

        #endregion

        #region Conversions

        private static void ToGray<T, Tv, R>(PixVolume src, object dst, Func<Tv, R> toGray)
        {
            ((Volume<R>)dst).SetMap(src.AsPixVolume<T>().GetVolume<Tv>(), toGray);
        }

        protected static readonly Dictionary<(Type, Type), Action<PixVolume, object>> s_rgbToGrayMap = new()
            {
                { (typeof(byte), typeof(byte)),     (src, dst) => ToGray<byte, C3b, byte>(src, dst, Col.ToGrayByte) },
                { (typeof(ushort), typeof(ushort)), (src, dst) => ToGray<ushort, C3us, ushort>(src, dst, Col.ToGrayUShort) },
                { (typeof(uint), typeof(uint)),     (src, dst) => ToGray<uint, C3ui, uint>(src, dst, Col.ToGrayUInt) },
                { (typeof(float), typeof(float)),   (src, dst) => ToGray<float, C3f, float>(src, dst, Col.ToGrayFloat) },
                { (typeof(double), typeof(double)), (src, dst) => ToGray<double, C3d, double>(src, dst, Col.ToGrayDouble) },
            };

        public PixVolume<T> AsPixVolume<T>() => this as PixVolume<T>;

        public PixVolume<T1> ToPixVolume<T1>() => AsPixVolume<T1>() ?? new PixVolume<T1>(this);

        public abstract PixVolume ToPixVolume(Col.Format format);

        public PixVolume<T> ToPixVolume<T>(Col.Format format)
        {
            if (this is PixVolume<T> castVolume && castVolume.Format == format && castVolume.ChannelCount == format.ChannelCount())
                return castVolume;
            return new PixVolume<T>(format, this);
        }

        public abstract PixVolume ToCanonicalDenseLayout();

        #endregion

        #region Copy

        public abstract void CopyChannelTo<Tv>(long channelIndex, Volume<Tv> target);

        public abstract void CopyTensor4To<Tv>(Tensor4<Tv> target);

        public abstract PixVolume CopyToPixVolume();

        public abstract PixVolume CopyToPixVolumeWithCanonicalDenseLayout();

        #endregion

        #region IPixVolumeVisitor

        public abstract TResult Visit<TResult>(IPixVolumeVisitor<TResult> visitor);

        #endregion
    }

    public class PixVolume<T> : PixVolume
    {
        public Tensor4<T> Tensor4;

        #region Constructors

        public PixVolume(Col.Format format, Tensor4<T> tensor4)
            : base(format)
        {
            Tensor4 = tensor4;
        }

        public PixVolume() { }

        public PixVolume(V3i size, int channelCount)
            : this(Col.FormatDefaultOf(typeof(T), channelCount),
                   CreateTensor4<T>(size.X, size.Y, size.Z, channelCount))
        { }

        public PixVolume(int sizeX, int sizeY, int sizeZ, int channelCount)
            : this(Col.FormatDefaultOf(typeof(T), channelCount),
                   CreateTensor4<T>(sizeX, sizeY, sizeZ, channelCount))
        { }

        public PixVolume(Col.Format format, V3i size)
            : this(format, CreateTensor4<T>(size.X, size.Y, size.Z, Col.ChannelCount(format)))
        { }

        public PixVolume(Col.Format format, int sizeX, int sizeY, int sizeZ)
            : this(format, CreateTensor4<T>(sizeX, sizeY, sizeZ, Col.ChannelCount(format)))
        { }

        public PixVolume(Col.Format format, V3i size, int channelCount)
            : this(format, CreateTensor4<T>(size.X, size.Y, size.Z, channelCount))
        { }

        public PixVolume(Col.Format format, int sizeX, int sizeY, int sizeZ, int channelCount)
            : this(format, CreateTensor4<T>(sizeX, sizeY, sizeZ, channelCount))
        { }

        /// <summary>
        /// Copy constructor: ALWAYS creates a copy of the data!
        /// </summary>
        public PixVolume(PixVolume pixVolume)
            : this(Col.FormatDefaultOf(typeof(T), pixVolume.Format.ChannelCount()), pixVolume)
        { }

        /// <summary>
        /// Copy constructor: ALWAYS creates a copy of the data!
        /// </summary>
        public PixVolume(Col.Format format, PixVolume pixVolume)
        {
            if (format.IsPremultiplied() != pixVolume.Format.IsPremultiplied())
            {
                throw new NotImplementedException(
                    "Conversion between alpha and premultiplied alpha formats not implemented yet."
                );
            }

            var srcInfo = pixVolume.Tensor4Info;
            var dstChannels = Col.ChannelsOfFormat(format);
            var tensor4 = CreateTensor4<T>(srcInfo.Size.X, srcInfo.Size.Y, srcInfo.Size.Z, dstChannels.Length);
            tensor4.F = pixVolume.Tensor4Info.F;

            if (format == pixVolume.Format)
            {
                pixVolume.CopyTensor4To(tensor4);
            }
            else
            {
                var srcChannels = Col.ChannelsOfFormat(pixVolume.Format);

                for (int dstIndex = 0; dstIndex < dstChannels.Length; dstIndex++)
                {
                    var channel = dstChannels[dstIndex];
                    var volume = tensor4.SubXYZVolume(dstIndex);
                    var srcIndex = srcChannels.IndexOf(channel);

                    // If we have an RGB channel, we may also just copy a Gray or BW channel
                    if (srcIndex == -1 && (channel == Col.Channel.Red || channel == Col.Channel.Green || channel == Col.Channel.Blue))
                    {
                        var bw = srcChannels.IndexOf(Col.Channel.BW);
                        var gray = srcChannels.IndexOf(Col.Channel.Gray);
                        srcIndex = Fun.Max(bw, gray);
                    }

                    if (srcIndex > -1)
                    {
                        // Channel exists in source image, just copy
                        if (pixVolume is PixVolume<T> pi)
                        {
                            volume.Set(pi.GetChannelInFormatOrder(srcIndex));
                        }
                        else
                        {
                            var order = pixVolume.Format.ChannelOrder();
                            pixVolume.CopyChannelTo(order[srcIndex], volume); // CopyChannelTo uses canonical order
                        }
                    }
                    else if (channel == Col.Channel.Alpha || channel == Col.Channel.PremultipliedAlpha)
                    {
                        // Alpha channel does not exist in source image, fill with max value
                        volume.Set(Col.Info<T>.MaxValue);
                    }
                    else if (channel == Col.Channel.Gray &&
                             srcChannels.Contains(Col.Channel.Red) &&
                             srcChannels.Contains(Col.Channel.Green) &&
                             srcChannels.Contains(Col.Channel.Blue))
                    {
                        var t1 = pixVolume.PixFormat.Type;
                        var t2 = typeof(T);

                        if (s_rgbToGrayMap.TryGetValue((t1, t2), out var toGray))
                        {
                            toGray(pixVolume, volume);
                        }
                        else
                        {
                            throw new NotImplementedException(
                                $"Conversion from {t1} image with format {pixVolume.Format} to {t2} grayscale not implemented."
                            );
                        }
                    }
                    else if (channel == Col.Channel.Blue &&
                             pixVolume.Format == Col.Format.RG &&
                             dstChannels.Contains(Col.Channel.Red) &&
                             dstChannels.Contains(Col.Channel.Green))
                    {
                        // Allow expanding from RG to RGB formats, blue channel is set to zero
                    }
                    else
                    {
                        throw new NotSupportedException(
                            $"Conversion from format {pixVolume.Format} to format {format} is not supported."
                        );
                    }
                }
            }

            Tensor4 = tensor4;
            Format = format;
        }

        #endregion

        #region Properties

        public override Array Data => Tensor4.Data;

        public override PixFormat PixFormat => new PixFormat(typeof(T), Format);

        public override Tensor4Info Tensor4Info => Tensor4.Info;

        public override int BytesPerChannel => typeof(T).GetCLRSize();

        /// <summary>
        /// Returns the channels of the image in canonical order: red, green,
        /// blue, (alpha).
        /// </summary>
        public IEnumerable<Volume<T>> Channels
        {
            get
            {
                long[] order = Format.ChannelOrder();
                for (long i = 0; i < order.Length; i++)
                    yield return GetChannelInFormatOrder(order[i]);
            }
        }

        /// <summary>
        /// Returns the array containing the cahnnels in canonical order: red,
        /// green, blue, (alpha).
        /// </summary>
        public Volume<T>[] ChannelArray => Channels.ToArray();

        /// <summary>
        /// Returns the volume representation of the tensor4 if there is only
        /// one channel. Fails if there are multiple channels.
        /// </summary>
        public Volume<T> Volume => Tensor4.AsVolumeWindow();

        #endregion

        #region Conversions

        public override PixVolume ToPixVolume(Col.Format format)
        {
            if (Format == format && ChannelCount == format.ChannelCount())
                return this;
            return new PixVolume<T>(format, this);
        }

        public PixVolume<T> ToFormat(Col.Format format) => Format == format ? this : new PixVolume<T>(format, this);

        public PixVolume<T> ToImageLayout() => !Tensor4.HasImageLayout() ? new PixVolume<T>(Format, this) : this;

        public override PixVolume ToCanonicalDenseLayout() => ToImageLayout();

        #endregion

        #region Copy

        public override void CopyChannelTo<Tv>(long channelIndex, Volume<Tv> target)
        {
            var subMatrix = GetChannel<Tv>(channelIndex);
            target.Set(subMatrix);
        }

        public override void CopyTensor4To<Tv>(Tensor4<Tv> target)
        {
            if (Tensor4 is Tensor4<Tv> source)
                target.Set(source);
            else
                target.Set(Tensor4.AsTensor4<T, Tv>());
        }

        public PixVolume<T> CopyToImageLayout()
        {
            if (Tensor4.HasImageLayout())
                return new PixVolume<T>(Format, Tensor4.CopyToImage());
            return new PixVolume<T>(this);
        }

        public PixVolume<T> Copy() => new PixVolume<T>(Format, Tensor4.CopyToImageWindow());

        public override PixVolume CopyToPixVolume() => Copy();

        public override PixVolume CopyToPixVolumeWithCanonicalDenseLayout() => CopyToImageLayout();

        /// <summary>
        /// Copy function for color conversions.
        /// </summary>
        /// <typeparam name="Tv"></typeparam>
        /// <param name="fun"></param>
        /// <returns></returns>
        public PixImage<T> Copy<Tv>(Func<Tv, Tv> fun)
        {
            return Copy<Tv>(fun, Format);
        }

        /// <summary>
        /// Copy function for color conversions. Note that the
        /// new color format must have the same number of channels
        /// as the old one, and the result of the supplied conversion
        /// function is reinterpreted as a color in the new format.
        /// </summary>
        /// <typeparam name="Tv"></typeparam>
        /// <param name="fun"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public PixImage<T> Copy<Tv>(Func<Tv, Tv> fun, Col.Format format)
        {
            var mat = GetVolume<Tv>().MapWindow(fun);
            var vol = new Volume<T>(mat.Data, Volume.Info);
            return new PixImage<T>(format, vol);
        }

        #endregion

        #region Obtaining Volumes

        /// <summary>
        /// Returns the specified channel, based on the canonical channel
        /// order: red, green, blue, (alpha).
        /// </summary>
        public Volume<T> GetChannel(long channelIndex)
        {
            var order = Format.ChannelOrder();
            return GetChannelInFormatOrder(order[channelIndex]);
        }

        /// <summary>
        /// Returns the specified channel.
        /// </summary>
        public Volume<T> GetChannel(Col.Channel channel)
        {
            return GetChannelInFormatOrder(Format.ChannelIndex(channel));
        }


        /// <summary>
        /// Returns the specified channel (based on the canonical order) with
        /// a different view type.
        /// </summary>
        public Volume<T, Tv> GetChannel<Tv>(long channelIndex)
        {
            return GetChannelInFormatOrder<Tv>(Format.ChannelOrder()[channelIndex]);
        }

        /// <summary>
        /// Returns the specified channel based on the order of the volumes's
        /// color format.
        /// </summary>
        public Volume<T> GetChannelInFormatOrder(long formatChannelIndex)
        {
            return Tensor4.SubXYZVolumeWindow(formatChannelIndex);
        }

        /// <summary>
        /// Returns the specified channel based on the order of the volumes's
        /// color format with a different view type.
        /// <param name="formatChannelIndex">formatChannelIndex</param>
        /// </summary>
        public Volume<T, Tv> GetChannelInFormatOrder<Tv>(long formatChannelIndex)
        {
            var volume = Tensor4.SubXYZVolume<Tv>(formatChannelIndex);
            volume.Accessors = TensorAccessors.Get<T, Tv>(TensorAccessors.Intent.ColorChannel, Tensor4.DeltaArray);
            return volume;
        }

        public Volume<T, Tv> GetVolume<Tv>()
        {
            var volume = Tensor4.SubXYZVolumeWindow<Tv>(0L);
            volume.Accessors = TensorAccessors.Get<T, Tv>(Format.GetIntent(), Tensor4.DeltaArray);
            return volume;
        }

        #endregion

        #region IPixVolumeVisitor

        public override TResult Visit<TResult>(IPixVolumeVisitor<TResult> visitor)
        {
            return visitor.Visit(this);
        }

        #endregion
    }
}
