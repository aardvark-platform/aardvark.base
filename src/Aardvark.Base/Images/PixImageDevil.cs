using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using System.Runtime.InteropServices;

#if USE_DEVIL
namespace Aardvark.Base
{
	public abstract partial class PixImage
	{
		private static object s_devilLock = new object();
		private static DevIL.ImageImporter s_devilImporter = null;
		private static DevIL.ImageExporter s_devilExporter = null;

		private static Dictionary<DevIL.DataFormat, Col.Format> s_pixColorFormats = new Dictionary<DevIL.DataFormat, Col.Format>()
		{
			{DevIL.DataFormat.BGR, Col.Format.BGR},
			{DevIL.DataFormat.BGRA, Col.Format.BGRA},
			{DevIL.DataFormat.RGB, Col.Format.RGB},
			{DevIL.DataFormat.RGBA, Col.Format.RGBA},
			{DevIL.DataFormat.Alpha, Col.Format.Gray},
		};


		private static Dictionary<Col.Format, DevIL.DataFormat> s_devilColorFormats = new Dictionary<Col.Format, DevIL.DataFormat>()
		{
			{Col.Format.BGR, DevIL.DataFormat.BGR},
			{Col.Format.BGRA, DevIL.DataFormat.BGRA},
			{Col.Format.RGB, DevIL.DataFormat.RGB},
			{Col.Format.RGBA, DevIL.DataFormat.RGBA},
			{Col.Format.Gray, DevIL.DataFormat.Alpha},
		};

		private static Dictionary<DevIL.DataType, Type> s_pixDataTypes = new Dictionary<DevIL.DataType, Type>()
		{
			{DevIL.DataType.Byte, typeof(sbyte)},
			{DevIL.DataType.Short, typeof(short)},
			{DevIL.DataType.Int, typeof(int)},
			{DevIL.DataType.UnsignedByte, typeof(byte)},
			{DevIL.DataType.UnsignedShort, typeof(ushort)},
			{DevIL.DataType.UnsignedInt, typeof(uint)},
			{DevIL.DataType.Float, typeof(float)},
			{DevIL.DataType.Double, typeof(double)},
		};

		private static Dictionary<Type, DevIL.DataType> s_devilDataTypes = new Dictionary<Type, DevIL.DataType>()
		{
			{typeof(sbyte), DevIL.DataType.Byte},
			{typeof(short), DevIL.DataType.Short},
			{typeof(int), DevIL.DataType.Int},
			{typeof(byte), DevIL.DataType.UnsignedByte},
			{typeof(ushort), DevIL.DataType.UnsignedShort},
			{typeof(uint), DevIL.DataType.UnsignedInt},
			{typeof(float), DevIL.DataType.Float},
			{typeof(double), DevIL.DataType.Double},
		};

		private static Dictionary<PixFileFormat, DevIL.ImageType> s_fileFormats = new Dictionary<PixFileFormat, DevIL.ImageType>()
		{
			{PixFileFormat.Bmp, DevIL.ImageType.Bmp},
			{PixFileFormat.Cut, DevIL.ImageType.Cut},
			{PixFileFormat.Dds, DevIL.ImageType.Dds},
			{PixFileFormat.Exr, DevIL.ImageType.Exr},
			{PixFileFormat.Gif, DevIL.ImageType.Gif},
			{PixFileFormat.Hdr, DevIL.ImageType.Hdr},
			{PixFileFormat.Ico, DevIL.ImageType.Ico},
			{PixFileFormat.Iff, DevIL.ImageType.Iff},
			{PixFileFormat.Jng, DevIL.ImageType.Jng},
			{PixFileFormat.Jp2, DevIL.ImageType.Jp2},
			{PixFileFormat.Jpeg, DevIL.ImageType.Jpg},
			{PixFileFormat.Mng, DevIL.ImageType.Mng},
			{PixFileFormat.Pcd, DevIL.ImageType.Pcd},
			{PixFileFormat.Pcx, DevIL.ImageType.Pcx},
			{PixFileFormat.Pict, DevIL.ImageType.Pic},
			{PixFileFormat.Png, DevIL.ImageType.Png},
			{PixFileFormat.Psd, DevIL.ImageType.PSD},
			{PixFileFormat.Raw, DevIL.ImageType.Raw},
			{PixFileFormat.Sgi, DevIL.ImageType.Sgi},
			{PixFileFormat.Targa, DevIL.ImageType.Tga},
			{PixFileFormat.Tiff, DevIL.ImageType.Tiff},
			{PixFileFormat.Wbmp, DevIL.ImageType.Wbmp},
			{PixFileFormat.Xpm, DevIL.ImageType.Xpm},
		};

		private static DevIL.ImageImporter Importer {
			get
			{
				if (s_devilImporter == null)
				{
					s_devilImporter = new DevIL.ImageImporter();
				}

				return s_devilImporter;
			}
		}

		private static DevIL.ImageExporter Exporter
		{
			get
			{
				if (s_devilExporter == null)
				{
					s_devilExporter = new DevIL.ImageExporter();
				}

				return s_devilExporter;
			}
		}

		/// <summary>
		/// Load image from stream via devil.
		/// </summary>
		/// <returns>If file could not be read, returns null, otherwise a Piximage.</returns>
		private static PixImage CreateRawDevil(
				Stream stream,
				PixLoadOptions loadFlags = PixLoadOptions.Default)
		{
			lock (s_devilLock)
			{
				var image = Importer.LoadImageFromStream(stream);
				var level0 = image.GetImageData(0);

				Type type;
				Col.Format fmt;
				if (!s_pixDataTypes.TryGetValue(level0.DataType, out type)) return null;
				if (!s_pixColorFormats.TryGetValue(level0.Format, out fmt)) return null;

				var size = new V2i(level0.Width, level0.Height);
				var imageType = typeof(PixImage<>).MakeGenericType(type);
				var arr = level0.Data;

				var pix = (PixImage)Activator.CreateInstance(imageType, fmt, size);
				var handle = GCHandle.Alloc(pix.Data, GCHandleType.Pinned);

				Marshal.Copy(arr, 0, handle.AddrOfPinnedObject(), arr.Length);
				handle.Free();

				image.Dispose();

				return pix;
			}            
		}

		/// <summary>
		/// Save image to stream via devil.
		/// </summary>
		/// <returns>True if the file was successfully saved.</returns>
		private bool SaveAsImageDevil(
				Stream stream, PixFileFormat format,
				PixSaveOptions options, int qualityLevel)
		{
			lock (s_devilLock)
			{
				var id = DevIL.Unmanaged.IL.GenerateImage();

				var bytes = new byte[this.Data.Length * this.PixFormat.ChannelCount * Marshal.SizeOf(this.PixFormat.Type)];
				var gcHandle = GCHandle.Alloc(this.Data, GCHandleType.Pinned);
				Marshal.Copy(gcHandle.AddrOfPinnedObject(), bytes, 0, bytes.Length);
				gcHandle.Free();

				try
				{
					DevIL.Unmanaged.IL.BindImage(id);
					DevIL.Unmanaged.IL.SetInteger(DevIL.Unmanaged.ILIntegerMode.ImageWidth, Size.X);
					DevIL.Unmanaged.IL.SetInteger(DevIL.Unmanaged.ILIntegerMode.ImageHeight, Size.Y);
					DevIL.Unmanaged.IL.SetDataFormat(s_devilColorFormats[Format]);
					DevIL.Unmanaged.IL.SetDataType(s_devilDataTypes[PixFormat.Type]);
					DevIL.Unmanaged.IL.SetImageData(bytes);
					DevIL.Unmanaged.IL.SetInteger(DevIL.Unmanaged.ILIntegerMode.JpgQuality, qualityLevel);
					DevIL.Unmanaged.IL.SaveImageToStream(s_fileFormats[format], stream);
					DevIL.Unmanaged.IL.BindImage(default(DevIL.Unmanaged.ImageID));

					return true;
				}
				catch (Exception e)
				{
					Report.Warn(e.ToString());

					return true;
				}
				finally
				{
					DevIL.Unmanaged.IL.DeleteImage(id);
				}  
			}
		}

		/// <summary>
		/// Gets info about a PixImage without loading the entire image into memory.
		/// </summary>
		/// <returns>null if the file info could not be loaded.</returns>
		public static PixImageInfo InfoFromFileNameDevil(
				string fileName, PixLoadOptions options)
		{
			// TODO: implement
			return null;
		}
	}
}
#endif