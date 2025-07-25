/*
    Copyright 2006-2025. The Aardvark Platform Team.

        https://aardvark.graphics

    Licensed under the Apache License, Version 2.0 (the "License");
    you may not use this file except in compliance with the License.
    You may obtain a copy of the License at

        http://www.apache.org/licenses/LICENSE-2.0

    Unless required by applicable law or agreed to in writing, software
    distributed under the License is distributed on an "AS IS" BASIS,
    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
    See the License for the specific language governing permissions and
    limitations under the License.
*/
using System;
using System.IO;

namespace Aardvark.Base.Coder;

public static class CodingExtensions
{
    public static byte[] Encode<T>(this T self)
    {
        using var stream = new MemoryStream();
        using (var coder = new BinaryWritingCoder(stream))
        {
            coder.CodeT(ref self);
        }
        return stream.ToArray();
    }

    public static T Decode<T>(this byte[] self)
    {
        using var stream = new MemoryStream(self);
        T result = default;
        using (var coder = new BinaryReadingCoder(stream)) coder.CodeT(ref result);
        return result;
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
