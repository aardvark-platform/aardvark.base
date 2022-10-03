using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
//#if !__MonoCS__ && !__ANDROID__
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//#endif

namespace Aardvark.Base
{
    public interface IPixVolumeVisitor<T>
    {
        T Visit<TData>(PixVolume<TData> volume);
    }

    public abstract class PixVolume : IPix
    {
        public Col.Format Format;

        #region Constructors

        //static PixVolume() { PixImageExtensions.Init(); }

        public PixVolume()
            : this(Col.Format.None)
        { }

        public PixVolume(Col.Format format)
        {
            Format = format;
        }

        #endregion

        #region Properties

        public PixVolumeInfo Info { get { return new PixVolumeInfo(PixFormat, Size); } }

        // public IEnumerable<INode> SubNodes { get { return Enumerable.Empty<INode>(); } }

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

        protected static Dictionary<(Type, Type), Func<object, object>>
            s_copyFunMap =
            new Dictionary<(Type, Type), Func<object, object>>()
            {
                { (typeof(byte), typeof(byte)), v => ((Tensor4<byte>)v).CopyWindow() },
                { (typeof(byte), typeof(ushort)), v => ((Tensor4<byte>)v).ToUShortColor() },
                { (typeof(byte), typeof(uint)), v => ((Tensor4<byte>)v).ToUIntColor() },
                { (typeof(byte), typeof(Half)), v => ((Tensor4<byte>)v).ToHalfColor() },
                { (typeof(byte), typeof(float)), v => ((Tensor4<byte>)v).ToFloatColor() },
                { (typeof(byte), typeof(double)), v => ((Tensor4<byte>)v).ToDoubleColor() },

                { (typeof(ushort), typeof(byte)), v => ((Tensor4<ushort>)v).ToByteColor() },
                { (typeof(ushort), typeof(ushort)), v => ((Tensor4<ushort>)v).CopyWindow() },
                { (typeof(ushort), typeof(uint)), v => ((Tensor4<ushort>)v).ToUIntColor() },
                { (typeof(ushort), typeof(Half)), v => ((Tensor4<ushort>)v).ToHalfColor() },
                { (typeof(ushort), typeof(float)), v => ((Tensor4<ushort>)v).ToFloatColor() },
                { (typeof(ushort), typeof(double)), v => ((Tensor4<ushort>)v).ToDoubleColor() },

                { (typeof(uint), typeof(byte)), v => ((Tensor4<uint>)v).ToByteColor() },
                { (typeof(uint), typeof(ushort)), v => ((Tensor4<uint>)v).ToUShortColor() },
                { (typeof(uint), typeof(uint)), v => ((Tensor4<uint>)v).CopyWindow() },
                { (typeof(uint), typeof(Half)), v => ((Tensor4<uint>)v).ToHalfColor() },
                { (typeof(uint), typeof(float)), v => ((Tensor4<uint>)v).ToFloatColor() },
                { (typeof(uint), typeof(double)), v => ((Tensor4<uint>)v).ToDoubleColor() },

                { (typeof(Half), typeof(byte)), v => ((Tensor4<Half>)v).ToByteColor() },
                { (typeof(Half), typeof(ushort)), v => ((Tensor4<Half>)v).ToUShortColor() },
                { (typeof(Half), typeof(uint)), v => ((Tensor4<Half>)v).ToUIntColor() },
                { (typeof(Half), typeof(Half)), v => ((Tensor4<Half>)v).CopyWindow() },
                { (typeof(Half), typeof(float)), v => ((Tensor4<Half>)v).ToFloatColor() },
                { (typeof(Half), typeof(double)), v => ((Tensor4<Half>)v).ToDoubleColor() },

                { (typeof(float), typeof(byte)), v => ((Tensor4<float>)v).ToByteColor() },
                { (typeof(float), typeof(ushort)), v => ((Tensor4<float>)v).ToUShortColor() },
                { (typeof(float), typeof(uint)), v => ((Tensor4<float>)v).ToUIntColor() },
                { (typeof(float), typeof(Half)), v => ((Tensor4<float>)v).ToHalfColor() },
                { (typeof(float), typeof(float)), v => ((Tensor4<float>)v).CopyWindow() },
                { (typeof(float), typeof(double)), v => ((Tensor4<float>)v).ToDoubleColor() },

                { (typeof(double), typeof(byte)), v => ((Tensor4<double>)v).ToByteColor() },
                { (typeof(double), typeof(ushort)), v => ((Tensor4<double>)v).ToUShortColor() },
                { (typeof(double), typeof(uint)), v => ((Tensor4<double>)v).ToUIntColor() },
                { (typeof(double), typeof(Half)), v => ((Tensor4<double>)v).ToHalfColor() },
                { (typeof(double), typeof(float)), v => ((Tensor4<double>)v).ToFloatColor() },
                { (typeof(double), typeof(double)), v => ((Tensor4<double>)v).CopyWindow() },
            };

        public abstract PixVolume<T1> ToPixVolume<T1>();

        public PixVolume<T> AsPixVolume<T>()
        {
            return this as PixVolume<T>;
        }

        public PixVolume<T> ToPixVolume<T>(Col.Format format)
        {
            if (this is PixVolume<T> castVolume && castVolume.Format == format
                && castVolume.ChannelCount == format.ChannelCount())
                return castVolume;
            return new PixVolume<T>(format, this);
        }

        #endregion

        #region Abstract Methods

        public abstract PixFormat PixFormat { get; }

        public abstract Tensor4Info Tensor4Info { get; }

        public abstract V3i Size { get; }

        public abstract V3l SizeL { get; }

        public abstract int ChannelCount { get; }

        public abstract Array Array { get; }

        public abstract void CopyChannelTo<Tv>(long channelIndex, Volume<Tv> target);

        public abstract PixVolume ToPixVolume(Col.Format format);

        public abstract PixVolume CopyToPixVolume();

        public abstract PixVolume CopyToPixVolumeWithCanonicalDenseLayout();

        public abstract TResult Visit<TResult>(IPixVolumeVisitor<TResult> visitor);

        #endregion

        #region IPix

        public Tr Op<Tr>(IPixOp<Tr> op) { return op.PixVolume(this); }

        #endregion
    }

    public class PixVolume<T> : PixVolume, IPixImage3d
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
        public PixVolume(Col.Format format, PixVolume pixImage)
        {
            var size = pixImage.Size;
            var channelCount = format.ChannelCount();
            var tensor4 = CreateTensor4<T>(size.X, size.Y, size.Z, channelCount);
            var order = format.ChannelOrder();
            var typedPixImage = pixImage as PixVolume<T>;
            if (channelCount != pixImage.ChannelCount
                && !(channelCount == 3 && pixImage.ChannelCount == 4))
            {
                if (channelCount == 4 && pixImage.ChannelCount == 3)
                {
                    channelCount = 3;   // only copy three channels, and set channel 4 (implied alpha) to 'opaque'
                    tensor4.SubXYZVolume(3).Set(Col.Info<T>.MaxValue);
                }
                else if (channelCount > 1 && pixImage.ChannelCount == 1)
                {
                    if (typedPixImage != null)
                    {
                        var vol = typedPixImage.Volume;
                        for (int i = 0; i < channelCount; i++)
                            tensor4.SubXYZVolume(i).Set(vol);
                    }
                    else
                    {
                        for (int i = 0; i < channelCount; i++)
                            pixImage.CopyChannelTo(0, tensor4.SubXYZVolume(i));
                    }
                    Tensor4 = tensor4;
                    Format = format;
                    return;
                }
                else
                    throw new ArgumentException("cannot perform color space conversion");
            }

            if (format.IsPremultiplied() != pixImage.Format.IsPremultiplied())
            {
                throw new NotImplementedException(
                        "conversion between alpha and premultiplied alpha not implemented yet");
            }

            if (typedPixImage != null)
            {
                var channelArray = typedPixImage.ChannelArray;
                for (int i = 0; i < channelCount; i++)
                    tensor4.SubXYZVolume(order[i]).Set(channelArray[i]);
            }
            else
            {
                for (int i = 0; i < channelCount; i++)
                    pixImage.CopyChannelTo(i, tensor4.SubXYZVolume(order[i]));
            }

            Tensor4 = tensor4;
            Format = format;
            
        
        }

        public PixVolume<T> CopyToImageLayout()
        {
            if (Tensor4.HasImageLayout())
                return new PixVolume<T>(Format, Tensor4.CopyToImage());
            return new PixVolume<T>(this);
        }

        public PixVolume<T> Copy()
        {
            return new PixVolume<T>(Format, Tensor4.CopyToImageWindow());
        }

        public override PixVolume CopyToPixVolume()
        {
            return Copy();
        }

        public override PixVolume CopyToPixVolumeWithCanonicalDenseLayout()
        {
            return CopyToImageLayout();
        }

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

        #region Properties

        public override Tensor4Info Tensor4Info => Tensor4.Info;

        public override V3i Size
        {
            get { return (V3i)Tensor4.Info.Size.XYZ; }
        }

        public override V3l SizeL
        {
            get { return Tensor4.Info.Size.XYZ; }
        }

        public override int ChannelCount
        {
            get { return (int)Tensor4.Info.Size.W; }
        }

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
        public Volume<T>[] ChannelArray
        {
            get { return Channels.ToArray(); }
        }
        
        public int BytesPerChannel
        {
            get { return System.Runtime.InteropServices.Marshal.SizeOf(typeof(T)); }
        }

        public override Array Array
        {
            get { return Tensor4.Data; }
        }

        /// <summary>
        /// Returns the volume representation of the tensor4 if there is only
        /// one channel. Fails if there are multiple channels.
        /// </summary>
        public Volume<T> Volume
        {
            get { return Tensor4.AsVolumeWindow(); }
        }

        #endregion

        #region Conversions

        public PixVolume<T> ToImageLayout()
        {
            if (!Tensor4.HasImageLayout())
                return new PixVolume<T>(Format, this);
            else return this;
        }

        public PixVolume<T> ToFormat(Col.Format format)
        {
            return Format == format ? this : new PixVolume<T>(format, this);
        }

        public override PixVolume<T1> ToPixVolume<T1>()
        {
            var castVolume = this as PixVolume<T1>;
            if (castVolume != null) return castVolume;
            var format = typeof(T1).FormatDefaultOf(ChannelCount);
            if (Format == format)
            {
                var copy = s_copyFunMap[(typeof(T), typeof(T1))];
                return new PixVolume<T1>(format, (Tensor4<T1>)copy(Tensor4));
            }
            return new PixVolume<T1>(this);
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
            volume.Accessors = TensorAccessors.Get<T, Tv>(
                    typeof(T), typeof(Tv), TensorAccessors.Intent.ColorChannel, Tensor4.DeltaArray);
            return volume;
        }

        public Volume<T, Tv> GetVolume<Tv>()
        {
            var volume = Tensor4.SubXYZVolumeWindow<Tv>(0L);
            volume.Accessors = TensorAccessors.Get<T, Tv>(
                                        typeof(T), typeof(Tv),
                                        Format.GetIntent(),
                                        Tensor4.DeltaArray);
            return volume;
        }

        #endregion

        #region Concrete Implementation Of Abstract Functions

        public override PixFormat PixFormat
        {
            get { return new PixFormat(typeof(T), Format); }
        }

        public override void CopyChannelTo<Tv>(long channelIndex, Volume<Tv> target)
        {
            var subMatrix = GetChannel<Tv>(channelIndex);
            target.Set(subMatrix);
        }

        public override PixVolume ToPixVolume(Col.Format format)
        {
            if (Format == format && ChannelCount == format.ChannelCount())
                return this;
            return new PixVolume<T>(format, this);
        }

        public override TResult Visit<TResult>(IPixVolumeVisitor<TResult> visitor)
        {
            return visitor.Visit(this);
        }

        #endregion
        
        #region IPixImage3d Members

        public Array Data
        {
            get { return Tensor4.Data; }
        }

        #endregion
    }
}
