using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base
{
    public abstract partial class PixImage
    {

        private static PixImage CreateRawLibTiff(
                string filename, PixLoadOptions loadFlags)
        {
#if USE_LIBTIFF

            using (Tiff tif = Tiff.Open(filename, "r"))
            {
                if (tif == null) return null;

                var sx = tif.GetField(TiffTag.IMAGEWIDTH)[0].ToInt();
                var sy = tif.GetField(TiffTag.IMAGELENGTH)[0].ToInt();
            
                var vol = default(Volume<byte>);

                vol.Size = new V3l(sx, sy, 3);

                return new PixImage<byte>(vol);
            }
#endif
            return null;
        }

        private bool SaveAsImageLibTiff(
                Stream stream, PixFileFormat format, PixSaveOptions options, int qualityLevel)
        {
            return false;
        }

        private static PixImageInfo InfoFromFileNameLibTiff(
                string fileName, PixLoadOptions options)
        {
            // TODO: return something if its possible
            return null;
        }
    }
}
