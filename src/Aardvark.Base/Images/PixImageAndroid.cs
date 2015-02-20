#if __ANDROID__

using System;
using System.IO;
using Android.Graphics;
using System.Collections.Generic;

using System.Runtime.InteropServices;
using AFormat = Android.Graphics.Format;
using BitmapFactory = Android.Graphics.BitmapFactory;

namespace Aardvark.Base
{
	public abstract partial class PixImage
	{
		private static Dictionary<Format, PixFormat> s_androidFormats = new Dictionary<Format, PixFormat>()
		{
			{ AFormat.A8, PixFormat.ByteGray },
			{ AFormat.Rgb888, PixFormat.ByteRGB },
			{ AFormat.Rgba8888, PixFormat.ByteRGBA },
		};


		/// <summary>
		/// Load image from stream via devil.
		/// </summary>
		/// <returns>If file could not be read, returns null, otherwise a Piximage.</returns>
		private static PixImage CreateRawAndroid(
			Stream stream,
			PixLoadOptions loadFlags = PixLoadOptions.Default)
		{
			var bmp = BitmapFactory.DecodeStream (stream);
			var info = bmp.GetBitmapInfo ();

			var size = new V2i(info.Width, info.Height);
			var format = s_androidFormats [info.Format];


			var pi = PixImage.Create (format, size.X, size.Y);

			pi.Data.UnsafeCoercedApply<byte>(b =>
			{
				var ptr = bmp.LockPixels ();
				Marshal.Copy(ptr, b, 0, b.Length);
				bmp.UnlockPixels();
			});

			bmp.Dispose ();
			return pi;   
		}

		/// <summary>
		/// Save image to stream via devil.
		/// </summary>
		/// <returns>True if the file was successfully saved.</returns>
		private bool SaveAsImageAndroid(
			Stream stream, PixFileFormat format,
			PixSaveOptions options, int qualityLevel)
		{
			throw new NotImplementedException ();
		}

		/// <summary>
		/// Gets info about a PixImage without loading the entire image into memory.
		/// </summary>
		/// <returns>null if the file info could not be loaded.</returns>
		public static PixImageInfo InfoFromFileNameAndroid(
			string fileName, PixLoadOptions options)
		{
			// TODO: implement
			return null;
		}
	}
}

#endif

