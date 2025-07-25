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
using System.IO;
using System.IO.Compression;

namespace Aardvark.Base.Coder;

public static class GzipUtils
{
    public static void GzipFile(string fileName)
    {
        Report.BeginTimed("compressing {0}", Path.GetFileName(fileName));
        using (var ofs = new FileStream(
            fileName + ".gz", FileMode.OpenOrCreate, FileAccess.Write
            ))
        {
            using var ifs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var gzs = new GZipStream(ofs, CompressionMode.Compress);
            var buf = new byte[65536];
            int count;
            while ((count = ifs.Read(buf, 0, 65536)) > 0)
            {
                gzs.Write(buf, 0, count);
            }
        }
        Report.End();
    }

    /// <summary>
    /// Uncompress gzipped file.
    /// </summary>
    /// <param name="fileName">Path without .gz at end of file name.</param>
    public static void UnGzipFile(string fileName)
    {
        using var ofs = new FileStream(
            fileName, FileMode.OpenOrCreate, FileAccess.Write
            );
        var srcFileName = fileName + ".gz";
        Report.BeginTimed("decompressing {0}", Path.GetFileName(srcFileName));
        using (var ifs = new FileStream(
            srcFileName, FileMode.Open, FileAccess.Read
            ))
        {
            using var gzs = new GZipStream(ifs, CompressionMode.Decompress);
            var buf = new byte[65536];
            int count;
            while ((count = gzs.Read(buf, 0, 65536)) > 0)
            {
                ofs.Write(buf, 0, count);
            }
        }
        Report.End();
    }

    public static Stream UnGzipFileToStream(string fileName)
    {
        var ofs = new MemoryStream();

        Report.BeginTimed("decompressing {0}", Path.GetFileName(fileName));
        using (var ifs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
            using var gzs = new GZipStream(ifs, CompressionMode.Decompress);
            var buf = new byte[65536];
            int count;
            while ((count = gzs.Read(buf, 0, 65536)) > 0)
            {
                ofs.Write(buf, 0, count);
            }
        }
        Report.End();

        ofs.Seek(0, SeekOrigin.Begin);
        return ofs;
    }
}
