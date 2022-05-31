using System;
using System.Threading;
using System.Collections.Generic;
using System.IO;

using System.Runtime.InteropServices;
using Devil = DevILSharp;
using DevILSharp;

namespace Aardvark.Base
{
    using I = DevILSharp.IL;

    public static class PixImageDevil
    {
        private static readonly object s_devilLock = new object();
        private static bool s_initialized = false;

        [OnAardvarkInit]
        public static void InitDevil()
        {
            try
            {
                lock (s_devilLock)
                {
                    if (s_initialized)
                        return;

                    Bootstrap.Init();
                    I.Init();
                    I.Enable(EnableCap.AbsoluteOrigin);
                    I.OriginFunc(OriginMode.UpperLeft);
                    I.Enable(EnableCap.OverwriteExistingFile);
                    I.Enable(EnableCap.ConvertPalette);

                    PixImage.AddLoader(Loader);

                    s_initialized = true;
                }
            }
            catch(Exception e)
            {
                Report.Warn("[PixImageDevil] could not InitDevil: {0}", e.Message);
            }
        }

        static PixImageDevil()
        {
            InitDevil();
        }

        private static readonly Dictionary<Devil.ChannelFormat, Col.Format> s_pixColorFormats = new Dictionary<Devil.ChannelFormat, Col.Format>()
        {
            {Devil.ChannelFormat.Alpha, Col.Format.Alpha},
            {Devil.ChannelFormat.BGR, Col.Format.BGR},
            {Devil.ChannelFormat.BGRA, Col.Format.BGRA},
            {Devil.ChannelFormat.ColorIndex, Col.Format.None},
            {Devil.ChannelFormat.Luminance, Col.Format.Gray},
            {Devil.ChannelFormat.LuminanceAlpha, Col.Format.GrayAlpha},
            {Devil.ChannelFormat.RGB, Col.Format.RGB},
            {Devil.ChannelFormat.RGBA, Col.Format.RGBA},
        };

        private static readonly Dictionary<Devil.ChannelType, Type> s_pixDataTypes = new Dictionary<Devil.ChannelType, Type>()
        {
            {Devil.ChannelType.Byte, typeof(sbyte)},
            {Devil.ChannelType.Short, typeof(short)},
            {Devil.ChannelType.Int, typeof(int)},
            {Devil.ChannelType.UnsignedByte, typeof(byte)},
            {Devil.ChannelType.UnsignedShort, typeof(ushort)},
            {Devil.ChannelType.UnsignedInt, typeof(uint)},
            {Devil.ChannelType.Float, typeof(float)},
            {Devil.ChannelType.Double, typeof(double)},
        };

        private static readonly Dictionary<Type, Devil.ChannelType> s_devilDataTypes = new Dictionary<Type, Devil.ChannelType>()
        {
            {typeof(sbyte), Devil.ChannelType.Byte},
            {typeof(short), Devil.ChannelType.Short},
            {typeof(int), Devil.ChannelType.Int},
            {typeof(byte), Devil.ChannelType.UnsignedByte},
            {typeof(ushort), Devil.ChannelType.UnsignedShort},
            {typeof(uint), Devil.ChannelType.UnsignedInt},
            {typeof(float), Devil.ChannelType.Float},
            {typeof(double), Devil.ChannelType.Double},
        };

        private static readonly Dictionary<Col.Format, Devil.ChannelFormat> s_devilColorFormats = new Dictionary<Col.Format, Devil.ChannelFormat>()
        {
            { Col.Format.RGB, Devil.ChannelFormat.RGB },
            { Col.Format.RGBA, Devil.ChannelFormat.RGBA },
            { Col.Format.BGR, Devil.ChannelFormat.BGR },
            { Col.Format.BGRA, Devil.ChannelFormat.BGRA },
            { Col.Format.Gray, Devil.ChannelFormat.Luminance },
            { Col.Format.GrayAlpha, Devil.ChannelFormat.LuminanceAlpha },
        };

        private static readonly Dictionary<PixFileFormat, Devil.ImageType> s_fileFormats = new Dictionary<PixFileFormat, Devil.ImageType>()
        {
            {PixFileFormat.Bmp,   Devil.ImageType.Bmp},
            {PixFileFormat.Cut,   Devil.ImageType.Cut},
            {PixFileFormat.Dds,   Devil.ImageType.Dds},
            {PixFileFormat.Exr,   Devil.ImageType.Exr0},
            {PixFileFormat.Gif,   Devil.ImageType.Gif},
            {PixFileFormat.Hdr,   Devil.ImageType.Hdr},
            {PixFileFormat.Ico,   Devil.ImageType.Ico},
            {PixFileFormat.Iff,   Devil.ImageType.Iff0},
            {PixFileFormat.Jng,   Devil.ImageType.Jng},
            {PixFileFormat.Jp2,   Devil.ImageType.Jp20},
            {PixFileFormat.Jpeg,  Devil.ImageType.Jpg},
            {PixFileFormat.Mng,   Devil.ImageType.Mng},
            {PixFileFormat.Pcd,   Devil.ImageType.Pcd},
            {PixFileFormat.Pcx,   Devil.ImageType.Pcx},
            {PixFileFormat.Pict,  Devil.ImageType.Pic},
            {PixFileFormat.Png,   Devil.ImageType.Png},
            {PixFileFormat.Psd,   Devil.ImageType.Psd},
            {PixFileFormat.Raw,   Devil.ImageType.Raw},
            {PixFileFormat.Sgi,   Devil.ImageType.Sgi},
            {PixFileFormat.Targa, Devil.ImageType.Tga},
            {PixFileFormat.Tiff,  Devil.ImageType.Tif},
            {PixFileFormat.Wbmp,  Devil.ImageType.WBmp},
            {PixFileFormat.Xpm,   Devil.ImageType.Xpm},
        };

        private class PixLoader : IPixLoader
        {
            public string Name => "DevIL";

            private static void Fail(string call)
            {
                var status = I.GetError();
                throw new ImageLoadException($"IL.{call}() failed, error code = {status}.");
            }

            private static T TemporaryImage<T>(Func<int, T> f)
            {
                var img = I.GenImage();
                I.BindImage(img);

                try
                {
                    return f(img);
                }
                finally
                {
                    I.BindImage(0);
                    I.DeleteImage(img);
                }
            }

            private static void TemporaryImage(Action<int> f)
                => TemporaryImage(img => { f(img); return 0; });

            private static T LoadImage<T>(Action loadData, Func<V2i, Col.Format, Type, int, T> parse)
            {
                lock (s_devilLock)
                {
                    return TemporaryImage(img =>
                    {
                        loadData();

                        var size = new V2i(I.GetInteger(IntName.ImageWidth), I.GetInteger(IntName.ImageHeight));
                        var dataType = I.GetDataType();
                        var format = I.GetFormat();
                        var channels = I.GetInteger(IntName.ImageChannels);

                        // try to get format information
                        if (!s_pixDataTypes.TryGetValue(dataType, out Type type))
                            throw new ImageLoadException($"Unsupported channel type {dataType}.");

                        if (!s_pixColorFormats.TryGetValue(format, out Col.Format fmt))
                            throw new ImageLoadException($"Unsupported format {format}.");

                        return parse(size, fmt, type, channels);
                    });
                }
            }

            private static PixImage LoadPixImage(Action load)
                => LoadImage(load, (size, format, type, channels) =>
                {
                    // if the image has a lower-left origin flip it
                    var mode = (OriginMode)I.GetInteger((IntName)0x0603);
                    if (mode == OriginMode.LowerLeft && !ILU.FlipImage())
                        return null;

                    // create an appropriate PixImage instance
                    var imageType = typeof(PixImage<>).MakeGenericType(type);
                    var pix = (PixImage)Activator.CreateInstance(imageType, format, size, channels);

                    // copy the data to the PixImage
                    var gc = GCHandle.Alloc(pix.Data, GCHandleType.Pinned);
                    try
                    {
                        var ptr = I.GetData();
                        ptr.CopyTo(pix.Data, 0, pix.Data.Length);
                        return pix;
                    }
                    finally
                    {
                        gc.Free();
                    }
                });

            private static void IlLoadImage(string filename)
            {
                if (!I.LoadImage(filename))
                    Fail("LoadImage");
            }

            private static void IlLoadStream(Stream stream)
            {
                if (!I.LoadStream(stream))
                    Fail("LoadStream");
            }

            public PixImage LoadFromFile(string filename)
                => LoadPixImage(() => IlLoadImage(filename));

            public PixImage LoadFromStream(Stream stream)
                => LoadPixImage(() => IlLoadStream(stream));

            private static void SaveImage(PixImage image, PixSaveParams saveParams, string saveMethod, Func<ImageType, bool> save)
            {
                if (!s_fileFormats.TryGetValue(saveParams.Format, out ImageType imageType))
                    throw new NotSupportedException($"Unsupported PixImage file format {saveParams.Format}.");

                lock (s_devilLock)
                {
                    TemporaryImage(img =>
                    {
                        if (!s_devilDataTypes.TryGetValue(image.PixFormat.Type, out ChannelType type))
                            throw new NotSupportedException($"Unsupported PixFormat type {image.PixFormat.Type}.");

                        if (!s_devilColorFormats.TryGetValue(image.PixFormat.Format, out ChannelFormat fmt))
                            throw new NotSupportedException($"Unsupported Col.Format {image.PixFormat.Format}.");

                        var gc = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
                        try
                        {
                            if (!I.TexImage(image.Size.X, image.Size.Y, 1, (byte)image.ChannelCount, fmt, type, gc.AddrOfPinnedObject()))
                                Fail("TexImage");

                            I.RegisterOrigin(OriginMode.UpperLeft);

                            if (saveParams is PixJpegSaveParams jpeg)
                                I.SetInteger(IntName.JpgQuality, jpeg.Quality);

                            if (saveParams is PixPngSaveParams png)
                            {
                                if (png.CompressionLevel != PixPngSaveParams.DefaultCompressionLevel)
                                    Report.Warn("DevIL does not support setting PNG compression levels.");
                            }

                            if (!save(imageType))
                            {
                                var status = I.GetError();

                                if (status == ErrorCode.InvalidEnum)
                                    throw new NotSupportedException($"DevIL does not support saving {imageType} images.");
                                else
                                    throw new ImageLoadException($"IL.{saveMethod}() failed, error code = {status}.");
                            }
                        }
                        finally
                        {
                            gc.Free();
                        }
                    });
                }
            }

            public void SaveToFile(string filename, PixImage image, PixSaveParams saveParams)
                => SaveImage(image, saveParams, "Save", imageType => I.Save(imageType, filename));

            public void SaveToStream(Stream stream, PixImage image, PixSaveParams saveParams)
            {
                if (saveParams.Format == PixFileFormat.Tiff)
                {
                    // Not sure why...
                    throw new NotSupportedException("DevIL does not support saving TIFF images to streams.");
                }
                else
                {
                    SaveImage(image, saveParams, "SaveStream", imageType => I.SaveStream(imageType, stream));
                }
            }

            private static PixImageInfo LoadPixImageInfo(Action load)
                => LoadImage(load, (size, format, type, _) =>
                        new PixImageInfo(new PixFormat(type, format), size)
                );

            public PixImageInfo GetInfoFromFile(string filename)
                => LoadPixImageInfo(() => IlLoadImage(filename));

            public PixImageInfo GetInfoFromStream(Stream stream)
                => LoadPixImageInfo(() => IlLoadStream(stream));
        }

        public static readonly IPixLoader Loader = new PixLoader();

        /// <summary>
        /// Load image from stream via devil.
        /// </summary>
        /// <returns>If file could not be read, returns null, otherwise a Piximage.</returns>
        [Obsolete("Use PixImage.LoadRaw() with DevIL.Loader")]
        public static PixImage CreateRawDevil(
                Stream stream,
                PixLoadOptions loadFlags = PixLoadOptions.Default)
            => PixImage.LoadRaw(stream, Loader);

        /// <summary>
        /// Load image from stream via devil.
        /// </summary>
        /// <returns>If file could not be read, returns null, otherwise a Piximage.</returns>
        [Obsolete("Use PixImage.LoadRaw() with DevIL.Loader")]
        public static PixImage CreateRawDevil(
                string fileName,
                PixLoadOptions loadFlags = PixLoadOptions.Default)
            => PixImage.LoadRaw(fileName, Loader);

        /// <summary>
        /// Save image to stream via devil.
        /// </summary>
        /// <returns>True if the file was successfully saved.</returns>
        [Obsolete("Use PixImage.Save() or PixImage.SaveAsJpeg() with DevIL.Loader")]
        public static bool SaveAsImageDevil(
                this PixImage image,
                Stream stream, PixFileFormat format,
                PixSaveOptions options, int qualityLevel)
        {
            if (format == PixFileFormat.Jpeg)
                image.SaveAsJpeg(stream, qualityLevel);
            else
                image.Save(stream, format);

            return true;
        }

        [Obsolete("Use PixImage.Save() or PixImage.SaveAsJpeg() with DevIL.Loader")]
        public static bool SaveAsImageDevil(
                this PixImage image,
                string file, PixFileFormat format,
                PixSaveOptions options, int qualityLevel)
        {
            var normalizeFilename = ((options & PixSaveOptions.NormalizeFilename) != 0);

            if (format == PixFileFormat.Jpeg)
                image.SaveAsJpeg(file, qualityLevel, normalizeFilename);
            else
                image.Save(file, format, normalizeFilename);

            return true;
        }

        /// <summary>
        /// Gets info about a PixImage without loading the entire image into memory.
        /// </summary>
        /// <returns>null if the file info could not be loaded.</returns>
        [Obsolete("Use PixImage.GetInfoFromFile() with DevIL.Loader")]
        public static PixImageInfo InfoFromFileNameDevil(
                string fileName, PixLoadOptions options)
            => PixImage.GetInfoFromFile(fileName, Loader);


        [Obsolete("Use PixImage.AddLoader with DevIL.Loader to modify priority")]
        public static void AddLoaders()
            => PixImage.AddLoader(Loader);
    }
}
