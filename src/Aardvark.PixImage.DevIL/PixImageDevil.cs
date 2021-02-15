using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Runtime.InteropServices;
using Devil = DevILSharp;
using DevILSharp;

namespace Aardvark.Base
{
    using I = DevILSharp.IL;

    public static class PixImageDevil
    {
        private static object s_devilLock = new object();
        private static bool s_initialized = false;

        [OnAardvarkInit]
        public static void InitDevil()
        {
            try
            {
                if (s_initialized) return;
                s_initialized = true;

                Bootstrap.Init();
                I.Init();
                I.Enable(EnableCap.AbsoluteOrigin);
                I.OriginFunc(OriginMode.UpperLeft);
                I.Enable(EnableCap.OverwriteExistingFile);
                I.Enable(EnableCap.ConvertPalette);

                AddLoaders();
            } catch(Exception e)
            {
                Report.Warn("[PixImageDevil] could not InitDevil: {0}", e.Message);
            }
        }

        static PixImageDevil()
        {
            InitDevil();
        }

        private static Dictionary<Devil.ChannelFormat, Col.Format> s_pixColorFormats = new Dictionary<Devil.ChannelFormat, Col.Format>()
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
        
        private static Dictionary<Devil.ChannelType, Type> s_pixDataTypes = new Dictionary<Devil.ChannelType, Type>()
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
        
        private static Dictionary<Type, Devil.ChannelType> s_devilDataTypes = new Dictionary<Type, Devil.ChannelType>()
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
        
        private static Dictionary<Col.Format, Devil.ChannelFormat> s_devilColorFormats = new Dictionary<Col.Format, Devil.ChannelFormat>()
        {
            { Col.Format.RGB, Devil.ChannelFormat.RGB },
            { Col.Format.RGBA, Devil.ChannelFormat.RGBA },
            { Col.Format.BGR, Devil.ChannelFormat.BGR },
            { Col.Format.BGRA, Devil.ChannelFormat.BGRA },
            { Col.Format.Gray, Devil.ChannelFormat.Luminance },
            { Col.Format.GrayAlpha, Devil.ChannelFormat.LuminanceAlpha },
        };
        
        private static Dictionary<PixFileFormat, Devil.ImageType> s_fileFormats = new Dictionary<PixFileFormat, Devil.ImageType>()
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


        /// <summary>
        /// Load image from stream via devil.
        /// </summary>
        /// <returns>If file could not be read, returns null, otherwise a Piximage.</returns>
        internal static PixImage CreateRawDevil(
                Stream stream,
                PixLoadOptions loadFlags = PixLoadOptions.Default)
        {
            lock (s_devilLock)
            {
                var img = I.GenImage();

                I.BindImage(img);
                if (!I.LoadStream(stream))
                {
                    var code = I.GetError();
                    throw new ImageLoadException(String.Format("CreateRawDevil] could not load image, error code = {0}", code));
                }

                return LoadImage(img);

            }            
        }

        /// <summary>
        /// Load image from stream via devil.
        /// </summary>
        /// <returns>If file could not be read, returns null, otherwise a Piximage.</returns>
        internal static PixImage CreateRawDevil(
                string fileName,
                PixLoadOptions loadFlags = PixLoadOptions.Default)
        {
            lock (s_devilLock)
            {
                var img = I.GenImage();

                I.BindImage(img);
                if (!I.LoadImage(fileName))
                    throw new ImageLoadException(string.Format("Could not load image: {0}", fileName));

                return LoadImage(img);

            }
        }

        private static PixImage LoadImage(int img)
        {

            var size = new V2i(I.GetInteger(IntName.ImageWidth), I.GetInteger(IntName.ImageHeight));
            var dataType = I.GetDataType();
            var format = I.GetFormat();
            var channels = I.GetInteger(IntName.ImageChannels);
            Type type;
            Col.Format fmt;

            // try to get format information
            if (!s_pixDataTypes.TryGetValue(dataType, out type)) return null;
            if (!s_pixColorFormats.TryGetValue(format, out fmt)) return null;

            // if the image has a lower-left origin flip it
            var mode = (OriginMode)I.GetInteger((IntName)0x0603);
            if (mode == OriginMode.LowerLeft && !ILU.FlipImage())
                return null;
            

            // create an appropriate PixImage instance
            var imageType = typeof(PixImage<>).MakeGenericType(type);
            var pix = (PixImage)Activator.CreateInstance(imageType, fmt, size, channels);

            // copy the data to the PixImage
            var gc = GCHandle.Alloc(pix.Data, GCHandleType.Pinned);
            try
            {
                var ptr = I.GetData();
                ptr.CopyTo(pix.Data, 0, pix.Data.Length);
            }
            finally
            {
                gc.Free();
            }

            // unbind and delete the DevIL image
            I.BindImage(0);
            I.DeleteImage(img);

            return pix;
        }

        private static bool SaveDevIL(this PixImage image, Func<bool> save, int qualityLevel)
        {
            //Devil.ImageType imageType;
            //if (!s_fileFormats.TryGetValue(format, out imageType))
            //    return false;

            var img = I.GenImage();
            I.BindImage(img);

            ChannelType type;
            ChannelFormat fmt;

            if (!s_devilDataTypes.TryGetValue(image.PixFormat.Type, out type)) return false;
            if (!s_devilColorFormats.TryGetValue(image.PixFormat.Format, out fmt)) return false;

            var gc = GCHandle.Alloc(image.Data, GCHandleType.Pinned);
            try
            {
                if (!I.TexImage(image.Size.X, image.Size.Y, 1, (byte)image.ChannelCount, fmt, type, gc.AddrOfPinnedObject()))
                    return false;

                I.RegisterOrigin(OriginMode.UpperLeft);

                if (qualityLevel != -1)
                    I.SetInteger(IntName.JpgQuality, qualityLevel);

                return save();
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                gc.Free();
                I.BindImage(0);
                I.DeleteImage(img);
            }
        }

        /// <summary>
        /// Save image to stream via devil.
        /// </summary>
        /// <returns>True if the file was successfully saved.</returns>
        internal static bool SaveAsImageDevil(
                this PixImage image,
                Stream stream, PixFileFormat format,
                PixSaveOptions options, int qualityLevel)
        {
            Devil.ImageType imageType;
            if (!s_fileFormats.TryGetValue(format, out imageType))
                return false;

            lock (s_devilLock)
            {
                return image.SaveDevIL(() => I.SaveStream(imageType, stream), qualityLevel);
            }
        }
        
        internal static bool SaveAsImageDevil(
                this PixImage image,
                string file, PixFileFormat format,
                PixSaveOptions options, int qualityLevel)
        {
            Devil.ImageType imageType;
            if (!s_fileFormats.TryGetValue(format, out imageType))
                return false;

            lock (s_devilLock)
            {
                return image.SaveDevIL(() => I.Save(imageType, file), qualityLevel);
            }
        }
        
        /// <summary>
        /// Gets info about a PixImage without loading the entire image into memory.
        /// </summary>
        /// <returns>null if the file info could not be loaded.</returns>
        public static PixImageInfo InfoFromFileNameDevil(
                string fileName, PixLoadOptions options)
        {
            lock (s_devilLock)
            {
                try
                {
                    var img = I.GenImage();
                    I.BindImage(img);

                    if (!I.LoadImage(fileName))
                        throw new ArgumentException("fileName");

                    var dataType = I.GetDataType();
                    var format = I.GetFormat();
                    var width = I.GetInteger(IntName.ImageWidth);
                    var height = I.GetInteger(IntName.ImageHeight);
                    //var channels = I.GetInteger(IntName.ImageChannels);

                    I.BindImage(0);
                    I.DeleteImage(img);


                    Type type;
                    Col.Format fmt;

                    if (!s_pixDataTypes.TryGetValue(dataType, out type)) return null;
                    if (!s_pixColorFormats.TryGetValue(format, out fmt)) return null;

                    var size = new V2i(width, height);
                    return new PixImageInfo(new PixFormat(type, fmt), size);
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }


        public static void AddLoaders()
        {
            Func<string, PixSaveOptions, PixFileFormat, int, PixImage, bool> fileSave =
                    (file, o, format, q, pi) => SaveAsImageDevil(pi, file, format, o, q);
            Func<Stream, PixSaveOptions, PixFileFormat, int, PixImage, bool> streamSave =
                    (stream, o, format, q, pi) => SaveAsImageDevil(pi, stream, format, o, q);
            Func<string, PixImageInfo> imageInfo = s => InfoFromFileNameDevil(s, PixLoadOptions.Default);

            PixImage.RegisterLoadingLib(
                CreateRawDevil,
                CreateRawDevil,
                fileSave, streamSave, imageInfo);
        }
    }
}
