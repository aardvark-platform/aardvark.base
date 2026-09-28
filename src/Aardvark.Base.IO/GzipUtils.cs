using Aardvark.Base;
using System.IO;
using System.IO.Compression;

namespace Aardvark.Base.Coder
{
    public static class GzipUtils
    {
        /// <summary>
        /// Compress a file to its name plus .gz, replacing the entire destination.
        /// </summary>
        /// <remarks>
        /// The input is opened before the destination is created or truncated.
        /// Replacement is not atomic: a later failure may leave a partial destination.
        /// </remarks>
        public static void GzipFile(string fileName)
        {
            Report.BeginTimed("compressing {0}", Path.GetFileName(fileName));
            using (var ifs = new FileStream(
                fileName, FileMode.Open, FileAccess.Read
                ))
            {
                using (var ofs = new FileStream(fileName + ".gz", FileMode.Create, FileAccess.Write))
                {
                    using (var gzs = new GZipStream(ofs, CompressionMode.Compress))
                    {
                        var buf = new byte[65536];
                        int count;
                        while ((count = ifs.Read(buf, 0, 65536)) > 0)
                        {
                            gzs.Write(buf, 0, count);
                        }
                    }
                }
            }
            Report.End();
        }

        /// <summary>
        /// Uncompress a gzipped file, replacing the entire destination.
        /// </summary>
        /// <remarks>
        /// The input is opened before the destination is created or truncated.
        /// Replacement is not atomic: a later failure may leave a partial destination.
        /// </remarks>
        /// <param name="fileName">Path without .gz at end of file name.</param>
        public static void UnGzipFile(string fileName)
        {
            var srcFileName = fileName + ".gz";
            Report.BeginTimed("decompressing {0}", Path.GetFileName(srcFileName));
            using (var ifs = new FileStream(
                srcFileName, FileMode.Open, FileAccess.Read
                ))
            {
                using (var ofs = new FileStream(
                    fileName, FileMode.Create, FileAccess.Write
                    ))
                {
                    using (var gzs = new GZipStream(ifs, CompressionMode.Decompress))
                    {
                        var buf = new byte[65536];
                        int count;
                        while ((count = gzs.Read(buf, 0, 65536)) > 0)
                        {
                            ofs.Write(buf, 0, count);
                        }
                    }
                }
                Report.End();
            }
        }

        public static Stream UnGzipFileToStream(string fileName)
        {
            var ofs = new MemoryStream();

            Report.BeginTimed("decompressing {0}", Path.GetFileName(fileName));
            using (var ifs = new FileStream(
                fileName, FileMode.Open, FileAccess.Read
                ))
            {
                using (var gzs = new GZipStream(ifs, CompressionMode.Decompress))
                {
                    var buf = new byte[65536];
                    int count;
                    while ((count = gzs.Read(buf, 0, 65536)) > 0)
                    {
                        ofs.Write(buf, 0, count);
                    }
                }
            }
            Report.End();

            ofs.Seek(0, SeekOrigin.Begin);
            return ofs;
        }
    }
}
