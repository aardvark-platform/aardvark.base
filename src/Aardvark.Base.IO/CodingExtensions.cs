using Aardvark.Base;
using System;
using System.IO;

namespace Aardvark.VRVis
{
    public static class CodingExtensions
    {
        public static byte[] Encode<T>(this T self)
        {
            using (var stream = new MemoryStream())
            {
                using (var coder = new BinaryWritingCoder(stream))
                {
                    coder.CodeT(ref self);
                }
                return stream.ToArray();
            }
        }

        public static T Decode<T>(this byte[] self)
        {
            using (var stream = new MemoryStream(self))
            {
                T result = default(T);
                using (var coder = new BinaryReadingCoder(stream)) coder.CodeT(ref result);
                return result;
            }
        }

        public static void CodeColFormat(this ICoder coder, ref Col.Format colFormat)
        {
            var reading = coder.IsReading;
            string formatName = reading ? null : colFormat.GetName().ToString();
            coder.CodeString(ref formatName);
            if (reading) colFormat = Col.FormatOfName(formatName);
        }

        public static void CodePixFormat(this ICoder coder, ref PixFormat pixFormat)
        {
            var reading = coder.IsReading;
            Type type = reading ? null : pixFormat.Type;
            string formatName = reading ? null : pixFormat.Format.GetName().ToString();
            coder.CodeType(ref type);
            coder.CodeString(ref formatName);
            if (reading) pixFormat = new PixFormat(type, Col.FormatOfName(formatName));
        }


    }
}
