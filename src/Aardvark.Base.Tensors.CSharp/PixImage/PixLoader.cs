using System;
using System.IO;

namespace Aardvark.Base
{
    /// <summary>
    /// Base class for save parameters.
    /// Derived classes provide format-specific parameters and settings.
    /// </summary>
    public class PixSaveParams
    {
        /// <summary>
        /// The format of the file to save.
        /// </summary>
        public PixFileFormat Format { get; }

        public PixSaveParams(PixFileFormat format)
        {
            Format = format;
        }
    }

    /// <summary>
    /// Save parameters for JPEG images.
    /// </summary>
    public class PixJpegSaveParams : PixSaveParams
    {
        /// <summary>
        /// Compression quality within 0 - 100.
        /// </summary>
        public int Quality { get; }

        /// <summary>
        /// Creates JPEG save parameters.
        /// </summary>
        /// <param name="quality">Compression quality from 0 (lowest) to 100 (highest). Default is <see cref="DefaultQuality"/>.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="quality"/> is outside the range 0–100.</exception>
        public PixJpegSaveParams(int quality = DefaultQuality) : base(PixFileFormat.Jpeg)
        {
            if (quality is < 0 or > 100)
                throw new ArgumentException($"Quality must be within 0 - 100, is ${quality}.");
            Quality = quality;
        }

        public const int DefaultQuality = 90;
    }

    /// <summary>
    /// Save parameters for WebP images.
    /// </summary>
    public class PixWebpSaveParams : PixSaveParams
    {
        /// <summary>
        /// Compression quality within 0 - 100.
        /// For lossy compression, higher quality results in better results with a larger file size.
        /// For lossless compression, higher quality results in a smaller file size but higher processing time.
        /// </summary>
        public int Quality { get; }

        /// <summary>
        /// Whether to use lossless or lossy compression. Default is lossy.
        /// </summary>
        public bool Lossless { get; }

        /// <summary>
        /// Creates WebP save parameters.
        /// </summary>
        /// <param name="quality">Compression quality from 0 (lowest) to 100 (highest). Default is <see cref="DefaultQuality"/>.</param>
        /// <param name="lossless">True to use lossless compression; false for lossy (default).</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="quality"/> is outside the range 0–100.</exception>
        public PixWebpSaveParams(int quality = DefaultQuality, bool lossless = false) : base(PixFileFormat.Webp)
        {
            if (quality is < 0 or > 100)
                throw new ArgumentException($"Quality must be within 0 - 100, is ${quality}.");
            Quality = quality;
            Lossless = lossless;
        }

        public const int DefaultQuality = 75;
    }

    /// <summary>
    /// Save parameters for PNG images.
    /// </summary>
    public class PixPngSaveParams : PixSaveParams
    {
        /// <summary>
        /// Compression level within 0 - 9.
        /// </summary>
        public int CompressionLevel { get; }

        /// <summary>
        /// Creates PNG save parameters.
        /// </summary>
        /// <param name="compressionLevel">Compression level from 0 (no compression) to 9 (maximum compression). Default is <see cref="DefaultCompressionLevel"/>.</param>
        /// <exception cref="ArgumentException">Thrown if <paramref name="compressionLevel"/> is outside the range 0–9.</exception>
        public PixPngSaveParams(int compressionLevel = DefaultCompressionLevel) : base(PixFileFormat.Png)
        {
            if (compressionLevel is < 0 or > 9)
                throw new ArgumentException($"Compression level must be within 0 - 9, is ${compressionLevel}.");
            CompressionLevel = compressionLevel;
        }

        public const int DefaultCompressionLevel = 6;
    }

    /// <summary>
    /// Supported compression schemes for TIFF files.
    /// </summary>
    public enum PixTiffCompression
    {
        /// <summary>
        /// Compression scheme is chosen by the image library.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Do not compress the image data.
        /// </summary>
        None = 1,

        /// <summary>
        /// CCITT Group 3 fax encoding.
        /// </summary>
        Ccitt3 = 2,

        /// <summary>
        /// CCITT Group 4 fax encoding.
        /// </summary>
        Ccitt4 = 3,

        /// <summary>
        /// LZW compression.
        /// </summary>
        Lzw = 4,

        /// <summary>
        /// JPEG compression.
        /// </summary>
        Jpeg = 5,

        /// <summary>
        /// Deflate compression, also known as ZLIB compression.
        /// </summary>
        Deflate = 6,

        /// <summary>
        /// PackBits compression.
        /// </summary>
        PackBits = 7,
    }

    /// <summary>
    /// Save parameters for TIFF images.
    /// </summary>
    public class PixTiffSaveParams : PixSaveParams
    {
        /// <summary>
        /// Used compression scheme.
        /// </summary>
        public PixTiffCompression Compression { get; }

        /// <summary>
        /// Creates TIFF save parameters.
        /// </summary>
        /// <param name="compression">The compression scheme to use. Default is <see cref="PixTiffCompression.Default"/>.</param>
        public PixTiffSaveParams(PixTiffCompression compression = PixTiffCompression.Default) : base(PixFileFormat.Tiff)
        {
            Compression = compression;
        }
    }

    /// <summary>
    /// Supported compression schemes for OpenEXR (EXR) files.
    /// </summary>
    public enum PixExrCompression
    {
        /// <summary>
        /// Compression scheme is chosen by the image library.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Do not compress the image data.
        /// </summary>
        None = 1,

        /// <summary>
        /// ZLIB compression, in blocks of 16 scan lines.
        /// </summary>
        Zip = 2,

        /// <summary>
        /// PIZ-based wavelet compression.
        /// </summary>
        Piz = 3,

        /// <summary>
        /// 24-bit float compression (lossy).
        /// </summary>
        Pxr24 = 4,

        /// <summary>
        /// 44% float compression (lossy).
        /// </summary>
        B44 = 5,
    }

    /// <summary>
    /// Save parameters for OpenEXR (EXR) images.
    /// </summary>
    public class PixExrSaveParams : PixSaveParams
    {
        /// <summary>
        /// Used compression scheme.
        /// </summary>
        public PixExrCompression Compression { get; }

        /// <summary>
        /// Encode with one luminance and two chroma channels (lossy compression).
        /// </summary>
        public bool LuminanceChroma { get; }

        /// <summary>
        /// Creates OpenEXR (EXR) save parameters.
        /// </summary>
        /// <param name="compression">The compression scheme to use. Default is <see cref="PixExrCompression.Default"/>.</param>
        /// <param name="luminanceChroma">If true, encodes as one luminance and two chroma channels (lossy).</param>
        public PixExrSaveParams(PixExrCompression compression = PixExrCompression.Default, bool luminanceChroma = false) : base(PixFileFormat.Exr)
        {
            Compression = compression;
            LuminanceChroma = luminanceChroma;
        }
    }

    /// <summary>
    /// Interface for PixImage loaders.
    /// </summary>
    public interface IPixLoader
    {
        /// <summary>
        /// The name of the loader.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Returns whether the loader supports encoding images (i.e. saving to files and streams).
        /// </summary>
        bool CanEncode { get; }

        /// <summary>
        /// Returns whether the loader supports decoding images (i.e. loading from files and streams).
        /// </summary>
        bool CanDecode { get; }

        /// <summary>
        /// Loads a PixImage from a file.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>The loaded PixImage.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        PixImage LoadFromFile(string filename);

        /// <summary>
        /// Loads a PixImage from a stream.
        /// </summary>
        /// <param name="stream">The stream to load.</param>
        /// <returns>The loaded PixImage.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        PixImage LoadFromStream(Stream stream);

        /// <summary>
        /// Saves a PixImage to a file.
        /// </summary>
        /// <param name="filename">The file to save to.</param>
        /// <param name="image">The image to save.</param>
        /// <param name="saveParams">Format-specific parameters and settings.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        void SaveToFile(string filename, PixImage image, PixSaveParams saveParams);

        /// <summary>
        /// Saves a PixImage to a stream.
        /// </summary>
        /// <param name="stream">The stream to save to.</param>
        /// <param name="image">The image to save.</param>
        /// <param name="saveParams">Format-specific parameters and settings.</param>
        /// <exception cref="ImageLoadException">if the image could not be saved.</exception>
        void SaveToStream(Stream stream, PixImage image, PixSaveParams saveParams);

        /// <summary>
        /// Loads basic information about an image from a file without loading its content.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>A PixImageInfo instance containing basic information about the image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        PixImageInfo GetInfoFromFile(string filename);

        /// <summary>
        /// Loads basic information about an image from a stream without loading its content.
        /// </summary>
        /// <param name="stream">The stream to load.</param>
        /// <returns>A PixImageInfo instance containing basic information about the image.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        PixImageInfo GetInfoFromStream(Stream stream);
    }

    /// <summary>
    /// Interface for PixImage loaders supporting loading mipmap chains.
    /// </summary>
    public interface IPixMipmapLoader : IPixLoader
    {
        /// <summary>
        /// Loads a PixImageMipMap from a file.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>The loaded PixImageMipMap.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        PixImageMipMap LoadMipmapFromFile(string filename);

        /// <summary>
        /// Loads a PixImageMipMap from a stream.
        /// </summary>
        /// <param name="stream">The stream to load.</param>
        /// <returns>The loaded PixImageMipMap.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        PixImageMipMap LoadMipmapFromStream(Stream stream);
    }
}