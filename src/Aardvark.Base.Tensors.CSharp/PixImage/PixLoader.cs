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

    public class PixJpegSaveParams : PixSaveParams
    {
        /// <summary>
        /// Compression quality within 0 - 100.
        /// </summary>
        public int Quality { get; }

        public PixJpegSaveParams(int quality = DefaultQuality) : base(PixFileFormat.Jpeg)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentException($"Quality must be within 0 - 100, is ${quality}");
            Quality = quality;
        }

        public const int DefaultQuality = 90;
    }

    public class PixPngSaveParams : PixSaveParams
    {
        /// <summary>
        /// Compression level within 0 - 9.
        /// </summary>
        public int CompressionLevel { get; }

        public PixPngSaveParams(int compressionLevel = DefaultCompressionLevel) : base(PixFileFormat.Png)
        {
            if (compressionLevel < 0 || compressionLevel > 9)
                throw new ArgumentException($"Compression level must be within 0 - 9, is ${compressionLevel}");
            CompressionLevel = compressionLevel;
        }

        public const int DefaultCompressionLevel = 6;
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
        /// Loads a PixImage from a file.
        /// </summary>
        /// <param name="filename">The file to load.</param>
        /// <returns>The loaded PixImage.</returns>
        /// <exception cref="ImageLoadException">if the image could not be loaded.</exception>
        PixImage LoadFromFile(string filename);

        /// <summary>
        /// Loads a PixImage from a stream.
        /// </summary>
        /// <param name="stream">The strean to load.</param>
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
}