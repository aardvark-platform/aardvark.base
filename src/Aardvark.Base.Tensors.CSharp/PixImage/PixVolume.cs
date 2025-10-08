using System;
using System.Collections.Generic;
using System.Linq;

namespace Aardvark.Base
{
    /// <summary>
    /// Defines a visitor for strongly typed PixVolume instances.
    /// Use this to dispatch operations based on the underlying pixel data type without reflection.
    /// </summary>
    /// <typeparam name="TResult">The result type produced by the visitor.</typeparam>
    public interface IPixVolumeVisitor<TResult>
    {
        /// <summary>
        /// Visits the given PixVolume with element type <typeparamref name="TData"/>.
        /// </summary>
        /// <typeparam name="TData">The pixel element type (e.g. byte, float, etc.).</typeparam>
        /// <param name="volume">The typed PixVolume instance to visit.</param>
        /// <returns>The visitor-specific result.</returns>
        TResult Visit<TData>(PixVolume<TData> volume);
    }

    /// <summary>
    /// Lightweight description of a <see cref="PixVolume"/>, containing its pixel format and dimensions.
    /// </summary>
    /// <param name="Format">The pixel format (channel type and color format).</param>
    /// <param name="Size">The 3D size in pixels (X = width, Y = height, Z = depth).</param>
    public record PixVolumeInfo(PixFormat Format, V3i Size);

    /// <summary>
    /// Base (non-generic) 3D pixel container abstraction. Provides format, size and conversion utilities
    /// for image volumes independent of the underlying element type.
    /// </summary>
    public abstract class PixVolume : IPix
    {
        /// <summary>
        /// Color format describing channel layout and semantics.
        /// </summary>
        public Col.Format Format;

        #region Constructors

        public PixVolume() : this(Col.Format.None) { }

        public PixVolume(Col.Format format)
        {
            Format = format;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying storage as a System.Array of the element type.
        /// </summary>
        public abstract Array Array { get; }

        /// <summary>
        /// Gets the pixel format of the volume (channel type and color format).
        /// </summary>
        public abstract PixFormat PixFormat { get; }

        /// <summary>
        /// Gets structural information about the underlying 4D tensor including size and layout.
        /// </summary>
        public abstract Tensor4Info Tensor4Info { get; }

        /// <summary>
        /// Gets the size in bytes of a single channel element (e.g. 1 for byte, 4 for float).
        /// </summary>
        public abstract int BytesPerChannel { get; }

        /// <summary>
        /// Gets a lightweight summary of this volume's format and size.
        /// </summary>
        public PixVolumeInfo Info => new(PixFormat, Size);

        /// <summary>
        /// Gets the 3D size of the volume in pixels.
        /// </summary>
        public V3i Size => (V3i)Tensor4Info.Size.XYZ;

        /// <summary>
        /// Gets the 3D size of the volume in pixels as 64-bit integers.
        /// </summary>
        public V3l SizeL => Tensor4Info.Size.XYZ;

        /// <summary>
        /// Gets the width of the volume in pixels.
        /// </summary>
        public int Width => Size.X;

        /// <summary>
        /// Gets the width of the volume in pixels as a 64-bit integer.
        /// </summary>
        public long WidthL => SizeL.X;

        /// <summary>
        /// Gets the height of the volume in pixels.
        /// </summary>
        public int Height => Size.Y;

        /// <summary>
        /// Gets the height of the volume in pixels as a 64-bit integer.
        /// </summary>
        public long HeightL => SizeL.Y;

        /// <summary>
        /// Gets the depth of the volume in pixels.
        /// </summary>
        public int Depth => Size.Z;

        /// <summary>
        /// Gets the depth of the volume in pixels as a 64-bit integer.
        /// </summary>
        public long DepthL => SizeL.Z;

        /// <summary>
        /// Gets the number of channels per voxel.
        /// </summary>
        public int ChannelCount => (int)Tensor4Info.Size.W;

        /// <summary>
        /// Gets the number of channels per voxel as a 64-bit integer.
        /// </summary>
        public long ChannelCountL => Tensor4Info.Size.W;

        #endregion

        #region Static Creator Functions

        /// <summary>
        /// Creates a 4D tensor with image-friendly memory layout for the given size.
        /// </summary>
        /// <typeparam name="T">Element type of the tensor.</typeparam>
        /// <param name="size">The tensor size as (width, height, depth, channels).</param>
        /// <returns>A new tensor with the requested size.</returns>
        public static Tensor4<T> CreateTensor4<T>(V4l size)
        {
            return size.CreateImageTensor4<T>();
        }

        /// <summary>
        /// Creates a 4D tensor with image-friendly memory layout for the given dimensions.
        /// </summary>
        /// <typeparam name="T">Element type of the tensor.</typeparam>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="depth">Depth in pixels.</param>
        /// <param name="channels">Number of channels.</param>
        /// <returns>A new tensor with the requested size.</returns>
        public static Tensor4<T> CreateTensor4<T>(long width, long height, long depth, long channels)
        {
            return new V4l(width, height, depth, channels).CreateImageTensor4<T>();
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

        /// <summary>
        /// Attempts to cast this instance to <see cref="PixVolume{T}"/>. Returns null if the element type differs.
        /// </summary>
        /// <typeparam name="T">Target element type.</typeparam>
        /// <returns>The typed volume or null.</returns>
        public PixVolume<T> AsPixVolume<T>() => this as PixVolume<T>;

        /// <summary>
        /// Returns a typed view of this volume. If the element type differs, a new volume with converted element type is created by copying.
        /// </summary>
        /// <typeparam name="T">Target element type.</typeparam>
        /// <returns>A typed volume of <typeparamref name="T"/>.</returns>
        public PixVolume<T> ToPixVolume<T>() => AsPixVolume<T>() ?? new PixVolume<T>(this);

        /// <summary>
        /// Converts this volume to the specified color format while keeping the underlying element type.
        /// If this volume already has the requested format, returns the same instance;
        /// in that case no data conversion or additional allocation is performed.
        /// </summary>
        /// <param name="format">Target color format.</param>
        /// <returns>A volume in the requested format; this instance if no conversion is required.</returns>
        public abstract PixVolume ToPixVolume(Col.Format format);

        /// <summary>
        /// Converts this volume to the specified color format and element type.
        /// If this volume already has the requested format and element type, returns the same instance;
        /// in that case no data conversion or additional allocation is performed.
        /// </summary>
        /// <param name="format">Target color format.</param>
        /// <typeparam name="T">Target element type.</typeparam>
        /// <returns>A typed volume in the requested format; this instance if no conversion is required.</returns>
        public PixVolume<T> ToPixVolume<T>(Col.Format format)
        {
            if (this is PixVolume<T> volume && volume.Format == format && volume.ChannelCount == format.ChannelCount()) return volume;
            return new PixVolume<T>(format, this);
        }

        /// <summary>
        /// Returns a representation with canonical, densely packed memory layout.
        /// If this volume is already in the correct layout, returns the same instance;
        /// in that case no data conversion or additional allocation is performed.
        /// </summary>
        public abstract PixVolume ToCanonicalDenseLayout();

        #endregion

        #region Copy

        /// <summary>
        /// Copies a single channel to the target volume (must match in size).
        /// If the element type differs, the data are reinterpreted.
        /// </summary>
        /// <typeparam name="Tv">Element type of the destination volume.</typeparam>
        /// <param name="channelIndex">Index of the channel in this volume.</param>
        /// <param name="target">Destination volume receiving the channel data.</param>
        /// <exception cref="NotSupportedException">if the element type <typeparamref name="Tv"/> is invalid.</exception>
        public abstract void CopyChannelTo<Tv>(long channelIndex, Volume<Tv> target);

        /// <summary>
        /// Copies the entire underlying tensor to the given target tensor (must match in size).
        /// If the element type differs, the data are reinterpreted.
        /// </summary>
        /// <typeparam name="Tv">Element type of the destination tensor.</typeparam>
        /// <param name="target">Destination tensor.</param>
        /// <exception cref="NotSupportedException">if the element type <typeparamref name="Tv"/> is invalid.</exception>
        public abstract void CopyTensor4To<Tv>(Tensor4<Tv> target);

        /// <summary>
        /// Creates a deep copy of this volume.
        /// </summary>
        public abstract PixVolume CopyToPixVolume();

        /// <summary>
        /// Creates a deep copy of this volume with canonical dense layout.
        /// </summary>
        public abstract PixVolume CopyToPixVolumeWithCanonicalDenseLayout();

        #endregion

        #region IPixVolumeVisitor

        /// <summary>
        /// Dispatches the current instance to a visitor based on its element type.
        /// </summary>
        /// <typeparam name="TResult">The result type produced by the visitor.</typeparam>
        /// <param name="visitor">The visitor implementation.</param>
        /// <returns>The visitor-specific result.</returns>
        public abstract TResult Visit<TResult>(IPixVolumeVisitor<TResult> visitor);

        #endregion
    }

    /// <summary>
    /// 3D pixel container with element type <typeparamref name="T"/>.
    /// Provides access to the underlying 4D tensor.
    /// </summary>
    /// <typeparam name="T">Per-channel element type.</typeparam>
    public class PixVolume<T> : PixVolume
    {
        /// <summary>
        /// The underlying 4D tensor storing the volume data.
        /// </summary>
        public Tensor4<T> Tensor4;

        #region Constructors

        /// <summary>
        /// Initializes a new empty PixVolume instance without allocating storage.
        /// Intended for serializers or deferred initialization scenarios. The
        /// <see cref="Tensor4"/> field must be assigned before use.
        /// </summary>
        public PixVolume() { }

        /// <summary>
        /// Creates a new PixVolume backed by the given tensor and using the specified color format.
        /// No data is copied; the instance takes a reference to <paramref name="tensor4"/>.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="tensor4">Backing tensor in image layout. Not copied.</param>
        public PixVolume(Col.Format format, Tensor4<T> tensor4)
            : base(format)
        {
            Tensor4 = tensor4;
        }

        /// <summary>
        /// Creates a new PixVolume backed by the given tensor and using the default color format for the element type.
        /// No data is copied; the instance takes a reference to <paramref name="tensor4"/>.
        /// </summary>
        /// <param name="tensor4">Backing tensor in image layout. Not copied.</param>
        public PixVolume(Tensor4<T> tensor4)
            : this(Col.FormatDefaultOf(typeof(T), tensor4.SW), tensor4)
        { }

        /// <summary>
        /// Allocates a new volume with the given 3D size and channel count using the default color format for the element type.
        /// </summary>
        /// <param name="size">Volume size in pixels (width, height, depth).</param>
        /// <param name="channels">Number of channels.</param>
        public PixVolume(V3i size, int channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), CreateTensor4<T>(size.X, size.Y, size.Z, channels))
        { }

        /// <inheritdoc cref="PixVolume{T}(V3i, int)"/>
        public PixVolume(V3l size, long channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), CreateTensor4<T>(size.X, size.Y, size.Z, channels))
        { }


        /// <summary>
        /// Allocates a new volume with the given dimensions and channel count using the default color format for the element type.
        /// </summary>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="depth">Depth in pixels.</param>
        /// <param name="channels">Number of channels.</param>
        public PixVolume(int width, int height, int depth, int channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), CreateTensor4<T>(width, height, depth, channels))
        { }

        /// <inheritdoc cref="PixVolume{T}(int, int, int, int)"/>
        public PixVolume(long width, long height, long depth, long channels)
            : this(Col.FormatDefaultOf(typeof(T), channels), CreateTensor4<T>(width, height, depth, channels))
        { }

        /// <summary>
        /// Allocates a new volume with the given size and channel format. The number of channels is
        /// derived from <paramref name="format"/>.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="size">Volume size in pixels (width, height, depth).</param>
        public PixVolume(Col.Format format, V3i size)
            : this(format, CreateTensor4<T>(size.X, size.Y, size.Z, Col.ChannelCount(format)))
        { }

        /// <inheritdoc cref="PixVolume{T}(Col.Format, V3i)"/>
        public PixVolume(Col.Format format, V3l size)
            : this(format, CreateTensor4<T>(size.X, size.Y, size.Z, Col.ChannelCount(format)))
        { }

        /// <summary>
        /// Allocates a new volume with the given dimensions and channel format. The number of channels is
        /// derived from <paramref name="format"/>.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="depth">Depth in pixels.</param>
        public PixVolume(Col.Format format, int width, int height, int depth)
            : this(format, CreateTensor4<T>(width, height, depth, Col.ChannelCount(format)))
        { }

        /// <inheritdoc cref="PixVolume{T}(Col.Format, int, int, int)"/>
        public PixVolume(Col.Format format, long width, long height, long depth)
            : this(format, CreateTensor4<T>(width, height, depth, Col.ChannelCount(format)))
        { }

        /// <summary>
        /// Allocates a new volume with the given size, explicit channel count and format.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="size">Volume size in pixels (width, height, depth).</param>
        /// <param name="channels">Number of channels.</param>
        public PixVolume(Col.Format format, V3i size, int channels)
            : this(format, CreateTensor4<T>(size.X, size.Y, size.Z, channels))
        { }

        /// <inheritdoc cref="PixVolume{T}(Col.Format, V3i, int)"/>
        public PixVolume(Col.Format format, V3l size, long channels)
            : this(format, CreateTensor4<T>(size.X, size.Y, size.Z, channels))
        { }

        /// <summary>
        /// Allocates a new volume with the given dimensions, explicit channel count and format.
        /// </summary>
        /// <param name="format">Color format describing the channel layout and semantics.</param>
        /// <param name="width">Width in pixels.</param>
        /// <param name="height">Height in pixels.</param>
        /// <param name="depth">Depth in pixels.</param>
        /// <param name="channels">Number of channels.</param>
        public PixVolume(Col.Format format, int width, int height, int depth, int channels)
            : this(format, CreateTensor4<T>(width, height, depth, channels))
        { }

        /// <inheritdoc cref="PixVolume{T}(Col.Format, int, int, int, int)"/>
        public PixVolume(Col.Format format, long width, long height, long depth, long channels)
            : this(format, CreateTensor4<T>(width, height, depth, channels))
        { }

        /// <summary>
        /// Creates a typed copy of the given volume. Always allocates new storage and copies data.
        /// The channel count is taken from <paramref name="source"/>, the element type becomes
        /// <typeparamref name="T"/> (conversion may occur).
        /// </summary>
        /// <param name="source">Source volume to copy from.</param>
        /// <remarks>
        /// <inheritdoc cref="PixVolume{T}(Col.Format, PixVolume)" path="/remarks"/>
        /// </remarks>
        public PixVolume(PixVolume source)
            : this(Col.FormatDefaultOf(typeof(T), source.Format.ChannelCount()), source)
        { }

        /// <summary>
        /// Creates a typed copy of the given volume in the requested color format. Always allocates new storage
        /// and copies or converts channel data as needed.
        /// </summary>
        /// <param name="format">Target color format for the new volume.</param>
        /// <param name="source">Source volume to copy from.</param>
        /// <remarks>
        /// <list type="bullet">
        /// <item>
        /// Premultiplied state must match between <paramref name="format"/> and <paramref name="source"/>; otherwise a
        ///   <see cref="NotImplementedException"/> is thrown.
        /// </item>
        /// <item>
        /// If the formats match and sizes are equal, a straight copy is performed.
        /// </item>
        /// <item>
        /// Missing alpha is filled with the maximum value of <typeparamref name="T"/>.
        /// </item>
        /// <item>
        /// Gray can be computed from RGB for a few common element types; unsupported combinations throw.
        /// </item>
        /// <item>
        /// Expanding RG to RGB sets blue to zero.
        /// </item>
        /// <item>
        /// Other unmapped conversions throw <see cref="NotSupportedException"/>.
        /// </item>
        /// </list>
        /// </remarks>
        public PixVolume(Col.Format format, PixVolume source)
        {
            if (format.IsPremultiplied() != source.Format.IsPremultiplied())
            {
                throw new NotImplementedException(
                    "Conversion between alpha and premultiplied alpha formats not implemented yet."
                );
            }

            var srcInfo = source.Tensor4Info;
            var dstChannels = Col.ChannelsOfFormat(format);
            var tensor4 = CreateTensor4<T>(srcInfo.Size.X, srcInfo.Size.Y, srcInfo.Size.Z, dstChannels.Length);
            tensor4.F = source.Tensor4Info.F;

            if (format == source.Format && srcInfo.Size == tensor4.Size)
            {
                source.CopyTensor4To(tensor4);
            }
            else
            {
                var srcChannels = Col.ChannelsOfFormat(source.Format);

                for (int dstIndex = 0; dstIndex < dstChannels.Length; dstIndex++)
                {
                    var channel = dstChannels[dstIndex];
                    var volume = tensor4.SubXYZVolume(dstIndex);
                    var srcIndex = srcChannels.IndexOf(channel);

                    // If we have an RGB channel, we may also just copy a Gray or BW channel
                    if (srcIndex == -1 && channel is Col.Channel.Red or Col.Channel.Green or Col.Channel.Blue)
                    {
                        var bw = srcChannels.IndexOf(Col.Channel.BW);
                        var gray = srcChannels.IndexOf(Col.Channel.Gray);
                        srcIndex = Fun.Max(bw, gray);
                    }

                    if (srcIndex > -1)
                    {
                        // Channel exists in source image, just copy
                        if (source is PixVolume<T> pi)
                        {
                            volume.Set(pi.GetChannelInFormatOrder(srcIndex));
                        }
                        else
                        {
                            var order = source.Format.ChannelOrder();
                            source.CopyChannelTo(order[srcIndex], volume); // CopyChannelTo uses canonical order
                        }
                    }
                    else if (channel is Col.Channel.Alpha or Col.Channel.PremultipliedAlpha)
                    {
                        // Alpha channel does not exist in source image, fill with max value
                        volume.Set(Col.Info<T>.MaxValue);
                    }
                    else if (channel == Col.Channel.Gray &&
                             srcChannels.Contains(Col.Channel.Red) &&
                             srcChannels.Contains(Col.Channel.Green) &&
                             srcChannels.Contains(Col.Channel.Blue))
                    {
                        var t1 = source.PixFormat.Type;
                        var t2 = typeof(T);

                        if (s_rgbToGrayMap.TryGetValue((t1, t2), out var toGray))
                        {
                            toGray(source, volume);
                        }
                        else
                        {
                            throw new NotImplementedException(
                                $"Conversion from {t1} image with format {source.Format} to {t2} grayscale not implemented."
                            );
                        }
                    }
                    else if (channel == Col.Channel.Blue &&
                             source.Format == Col.Format.RG &&
                             dstChannels.Contains(Col.Channel.Red) &&
                             dstChannels.Contains(Col.Channel.Green))
                    {
                        // Allow expanding from RG to RGB formats, blue channel is set to zero
                    }
                    else
                    {
                        throw new NotSupportedException(
                            $"Conversion from format {source.Format} to format {format} is not supported."
                        );
                    }
                }
            }

            Tensor4 = tensor4;
            Format = format;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the underlying storage as a <typeparamref name="T"/> array.
        /// </summary>
        public T[] Data => Tensor4.Data;

        /// <inheritdoc />
        public override Array Array => Tensor4.Data;

        /// <inheritdoc />
        public override PixFormat PixFormat => new(typeof(T), Format);

        /// <inheritdoc />
        public override Tensor4Info Tensor4Info => Tensor4.Info;

        /// <inheritdoc />
        public override int BytesPerChannel => typeof(T).GetCLRSize();

        /// <summary>
        /// Gets the channel volumes in canonical order (red, green, blue, alpha).
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
        /// Gets the channel volumes as array in canonical order (red, green, blue, alpha).
        /// </summary>
        public Volume<T>[] ChannelArray => Channels.ToArray();

        /// <summary>
        /// Returns the volume representation of the 4D tensor if there is only one channel.
        /// </summary>
        /// <exception cref="InvalidOperationException">if there are multiple channels.</exception>
        public Volume<T> Volume => Tensor4.AsVolumeWindow();

        #endregion

        #region Conversions

        /// <inheritdoc cref="ToFormat"/>
        public override PixVolume ToPixVolume(Col.Format format) => ToFormat(format);

        /// <summary>
        /// Converts this volume to the specified color format. If the current format already matches
        /// and the channel counts are equal, the current instance is returned; otherwise a new instance
        /// with converted channels is created.
        /// </summary>
        /// <param name="format">Target color format.</param>
        /// <returns>A volume in the requested format.</returns>
        public PixVolume<T> ToFormat(Col.Format format)
        {
            if (Format == format && ChannelCount == format.ChannelCount()) return this;
            return new PixVolume<T>(format, this);
        }

        /// <inheritdoc cref="ToCanonicalDenseLayout" />
        public PixVolume<T> ToImageLayout() => !Tensor4.HasImageLayout() ? new PixVolume<T>(Format, this) : this;

        /// <inheritdoc />
        public override PixVolume ToCanonicalDenseLayout() => ToImageLayout();

        #endregion

        #region Copy

        /// <inheritdoc />
        public override void CopyChannelTo<Tv>(long channelIndex, Volume<Tv> target)
        {
            var subMatrix = GetChannel<Tv>(channelIndex);
            target.Set(subMatrix);
        }

        /// <inheritdoc />
        public override void CopyTensor4To<Tv>(Tensor4<Tv> target)
        {
            if (Tensor4 is Tensor4<Tv> source)
                target.Set(source);
            else
                target.Set(Tensor4.AsTensor4<T, Tv>());
        }

        /// <summary>
        /// Creates a deep copy using canonical image layout regardless of the current layout.
        /// If already in image layout, the data is copied; otherwise, data is transformed to image layout.
        /// </summary>
        public PixVolume<T> CopyToImageLayout()
        {
            return Tensor4.HasImageLayout() ? new PixVolume<T>(Format, Tensor4.CopyToImage()) : new PixVolume<T>(this);
        }

        /// <inheritdoc cref="CopyToPixVolume" />
        public PixVolume<T> Copy() => new(Format, Tensor4.CopyToImageWindow());

        /// <inheritdoc />
        public override PixVolume CopyToPixVolume() => Copy();

        /// <inheritdoc />
        public override PixVolume CopyToPixVolumeWithCanonicalDenseLayout() => CopyToImageLayout();

        [Obsolete("Should return PixVolume.")]
        public PixImage<T> Copy<Tv>(Func<Tv, Tv> fun)
        {
            return Copy(fun, Format);
        }

        [Obsolete("Should return PixVolume.")]
        public PixImage<T> Copy<Tv>(Func<Tv, Tv> fun, Col.Format format)
        {
            var mat = GetVolume<Tv>().MapWindow(fun);
            var vol = new Volume<T>(mat.Data, Volume.Info);
            return new PixImage<T>(format, vol);
        }

        #endregion

        #region Obtaining Volumes

        /// <summary>
        /// Returns the specified channel using the canonical order (red, green, blue, alpha).
        /// </summary>
        /// <param name="channelIndex">Index within the canonical order of channels.</param>
        /// <returns>A volume window over the selected channel.</returns>
        public Volume<T> GetChannel(long channelIndex)
        {
            var order = Format.ChannelOrder();
            return GetChannelInFormatOrder(order[channelIndex]);
        }

        /// <summary>
        /// Returns the specified channel by semantic name according to the current format.
        /// </summary>
        /// <param name="channel">The channel to access (e.g. Red, Green, Blue, Alpha, Gray).</param>
        /// <returns>A volume window over the selected channel.</returns>
        public Volume<T> GetChannel(Col.Channel channel)
        {
            return GetChannelInFormatOrder(Format.ChannelIndex(channel));
        }


        /// <summary>
        /// Returns the specified channel (using the canonical order) with a different view type.
        /// </summary>
        /// <typeparam name="Tv">Element view type of the returned volume.</typeparam>
        /// <param name="channelIndex">Index within the canonical order of channels.</param>
        /// <returns>A typed volume window over the selected channel.</returns>
        /// <exception cref="NotSupportedException">if the element type and view type are incompatible.</exception>
        public Volume<T, Tv> GetChannel<Tv>(long channelIndex)
        {
            return GetChannelInFormatOrder<Tv>(Format.ChannelOrder()[channelIndex]);
        }

        /// <summary>
        /// Returns the specified channel based on the order of the color format.
        /// </summary>
        /// <param name="formatChannelIndex">Index of the channel in the current format's order.</param>
        /// <returns>A volume window over the selected channel.</returns>
        public Volume<T> GetChannelInFormatOrder(long formatChannelIndex)
        {
            return Tensor4.SubXYZVolumeWindow(formatChannelIndex);
        }

        /// <summary>
        /// Returns the specified channel based on the order of the color format with a different view type.
        /// </summary>
        /// <typeparam name="Tv">Element view type of the returned volume.</typeparam>
        /// <param name="formatChannelIndex">Index of the channel in the current format's order.</param>
        /// <returns>A typed volume window over the selected channel.</returns>
        /// <exception cref="NotSupportedException">if the element type and view type are incompatible.</exception>
        public Volume<T, Tv> GetChannelInFormatOrder<Tv>(long formatChannelIndex)
        {
            var volume = Tensor4.SubXYZVolume<Tv>(formatChannelIndex);
            volume.Accessors = TensorAccessors.Get<T, Tv>(TensorAccessors.Intent.ColorChannel, Tensor4.DeltaArray);
            return volume;
        }

        /// <summary>
        /// Reinterprets the underlying 4D image tensor as a volume of the specified type.
        /// </summary>
        /// <typeparam name="Tv">Element view type of the returned volume (e.g. a color type such as <see cref="C3f"/> or <see cref="C4ui"/>).</typeparam>
        /// <returns>A typed volume window over the whole 4D image tensor.</returns>
        /// <exception cref="NotSupportedException">if the view type is invalid for the format and element type.</exception>
        public Volume<T, Tv> GetVolume<Tv>() => Tensor4.GetVolume<T, Tv>(Format.GetIntent());

        #endregion

        #region IPixVolumeVisitor

        /// <inheritdoc />
        public override TResult Visit<TResult>(IPixVolumeVisitor<TResult> visitor)
        {
            return visitor.Visit(this);
        }

        #endregion
    }
}
