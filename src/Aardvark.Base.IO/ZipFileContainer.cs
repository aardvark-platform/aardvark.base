using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

// Important: .NET 4.5 contains full zip support.
namespace Aardvark.Base.Coder.Legacy
{
    /// <summary>
    /// Important: .NET 4.5 contains full zip support.
    /// Read-Only access to ZipFiles.
    /// </summary>
    public class ZipFile : IDisposable
    {
        #region private fields

        private struct SubFileHeader
        {
            public int ZipFileIndex;
            public long LocalHeaderOffset;
            public long LocalHeaderSize;
            public long CompressedSize;
            public long UncompressedSize;
        }

        private string[] m_ZipFileNames = new string[0];
        private WeakReference[] m_ZipFileStreams = new WeakReference[0];
        private SymbolDict<SubFileHeader> m_Files = new SymbolDict<SubFileHeader>();
        private SymbolSet m_Directories = new SymbolSet();
        private bool m_contentCaseSensitive = false;

        #endregion

        #region public properties or fields

        /// <summary>
        /// Returns the Name of the Container Parts.
        /// if different parts: e.g. OPC.z01, OPC.z02, ..., OPC.zip
        /// if single part: e.g. OPC.zip
        /// </summary>
        public string[] ZipFileNames
        {
            get { return m_ZipFileNames; }
        }

        public string MainZipFileName
        {
            get { return m_ZipFileNames.Last(); }
        }

        /// <summary>
        /// Names of files inside of the ZipFile.
        /// </summary>
        public IEnumerable<string> FileNames
        {
            get { return m_Files.Keys.Select(sym => sym.ToString()); }
        }

        /// <summary>
        /// Naems of directories inside of the ZipFile.
        /// </summary>
        public IEnumerable<string> DirectoryNames
        {
            get { return m_Directories.Keys.Select(sym => sym.ToString()); }
        }

        #endregion

        public ZipFile() { }

        /// <summary>
        /// Initializes container.
        /// </summary>
        /// <param name="containerPath"></param>
        public ZipFile(string containerPath)
        {
            Init(containerPath);
        }

        /// <summary>
        /// </summary>
        /// <param name="contentCaseSensitive">Load and handle content case sensitive.</param>
        public ZipFile(bool contentCaseSensitive)
        {
            m_contentCaseSensitive = contentCaseSensitive;
        }

        /// <summary>
        /// Reads Zip file central directory for later access.
        /// </summary>
        /// <param name="containerPath">Path to main zip container (e.g. MyZip.zip).</param>
        /// <returns></returns>
        public bool Init(string containerPath)
        {
            if (string.IsNullOrWhiteSpace(containerPath)) throw new ArgumentNullException(nameof(containerPath));
            Report.BeginTimed("Init Zip container '" + containerPath + "'.");

            FileStream mainFileStream = null;
            FileStream[] zipStreams = new FileStream[0];

            try
            {
                mainFileStream = File.Open(containerPath, FileMode.Open, FileAccess.Read, FileShare.None);

                // Load End Of Central Directory Record
                var zipEoCDR = new TZipEndOfCentralDirectoryRecord();
                zipEoCDR.Load(mainFileStream);

                if (zipEoCDR.Position < 0)
                    throw new Exception("ZipFile: couldn't find central directory record.");

                int numberOfEntries = zipEoCDR.cd_totalEntries;
                long cd_offset = zipEoCDR.cd_offset;
                int cd_diskId = zipEoCDR.cd_diskId;

                // init number of zip containers
                m_ZipFileNames = new string[zipEoCDR.cd_diskId + 1];
                m_ZipFileNames[zipEoCDR.cd_diskId] = containerPath;

                zipStreams = new FileStream[zipEoCDR.cd_diskId + 1];
                zipStreams[zipEoCDR.cd_diskId] = mainFileStream;
                
                // Load all parts of a multi zip
                if (zipEoCDR.cd_diskId > 0)
                {
                    Report.Line("Is multi zip container.");

                    // Segment 1 = filename.z01
                    // Segment n-1 = filename.z(n-1)
                    // Segment n = filename.zip

                    var baseName =
                        0 == containerPath.Substring(containerPath.Length - 4).ToLower().CompareTo(".zip") ?
                        containerPath.Substring(0, containerPath.Length - 4) :
                        containerPath;
                    baseName += ".z";

                    for (int i = 0; i < zipEoCDR.cd_diskId; i++)
                    {
                        m_ZipFileNames[i] = baseName + (i+1).ToString("D2");
                        zipStreams[i] = File.Open(m_ZipFileNames[i], FileMode.Open, FileAccess.Read, FileShare.None);
                    }
                }
                
                // Try to load Zip64 End Of Central Directory Locator
                var zip64Locator = new TZip64EndOfCentralDirectoryLocator();
                zip64Locator.Load(mainFileStream, zipEoCDR.Position);

                if (zip64Locator.IsValid)
                {
                    Report.Line("Is Zip64 container.");

                    // Load Zip64 End of Central Directory Record
                    var zip64EoCDR = new TZip64EndOfCentralDirectoryRecord();
                    zip64EoCDR.Load(
                        zipStreams[zip64Locator.ecd64_diskId],
                        (long)zip64Locator.ecd64_relOffset
                        );

                    numberOfEntries = (int)zip64EoCDR.cd_totalEntries;
                    cd_offset = (long)zip64EoCDR.cd_offset;
                    cd_diskId = (int)zip64EoCDR.cd_diskId;
                }

                // Load file headers in central directory
                this.ReadZipCentralDirectoryFileHeaders(zipStreams, cd_diskId, cd_offset, numberOfEntries);

                // save weak links to streams
                m_ZipFileStreams = new WeakReference[zipStreams.Length];
                for (int i = 0; i < zipStreams.Length; i++)
                    m_ZipFileStreams[i] = new WeakReference(zipStreams[i]);
            }
            catch (Exception)
            {
                Report.Warn("Error initializing container.");
                Report.End();

                foreach (var stream in zipStreams)
                    if (stream != null) stream.Close();

                if (mainFileStream != null) mainFileStream.Close();

                return false;
            }

            Report.End("Init Zip container finished.");
            return true;
        }

        public static bool IsZipFile(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName)) throw new ArgumentNullException(nameof(fileName));

            using (var fileStream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None))
            {
                // Load End Of Central Directory Record
                var zipEoCDR = new TZipEndOfCentralDirectoryRecord();
                zipEoCDR.Load(fileStream);

                if (zipEoCDR.Position > 0) return true;
            }

            return false;
        }

        /// <summary>
        /// Tests if a requested file, indexed by fileName, exists in the ContainerFile.
        /// </summary>
        /// <param name="fileName">Name of the requested file.</param>
        /// <returns>Indication wheter the file exists or not.</returns>
        public bool FileExists(string fileName)
        {
            fileName = SanitizeFilename(fileName);
            return m_Files.ContainsKey(fileName);
        }

        /// <summary>
        /// Tests if a requested directory, indexed by its path, exists in the ContainerFile.
        /// </summary>
        /// <param name="path">Path of the requested directory.</param>
        /// <returns>Indication wheter the directory exists or not.</returns>
        public bool DirectoryExists(string path)
        {
            path = SanitizeFilename(path);
            return this.m_Directories.Contains(path);
        }

        /// <summary>
        /// Get directories of local path in ZipFile.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetDirectories(string path)
        {
            path = SanitizeFilename(path);
            var dirs = m_Directories
                .Select(dirSym => dirSym.ToString())
                .Where(dir => dir.StartsWith(path));

            if (dirs.IsEmpty()) throw new DirectoryNotFoundException();

            var dirLevel = path.Count(c => c == '\\');

            return dirs
                .Where(dir => dir.Count(c => c == '\\') == dirLevel + 1)
                .ToArray();
        }

        /// <summary>
        /// Get files of local path in ZipFile.
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string[] GetFiles(string path)
        {
            path = SanitizeFilename(path);
            var dirs = m_Files.Keys
                .Select(dirSym => dirSym.ToString())
                .Where(dir => dir.StartsWith(path));

            if (dirs.IsEmpty()) throw new DirectoryNotFoundException();

            var dirLevel = path.Count(c => c == '\\');

            return dirs
                .Where(dir => dir.Count(c => c == '\\') == dirLevel + 1)
                .ToArray();
        }

        public Stream GetStream(string fileName)
        {
            fileName = SanitizeFilename(fileName);
            return GetStream((Symbol)fileName);
        }
        public Stream GetStream(Symbol fileName)
        {
            SubFileHeader fileHeader;
            if (m_Files.TryGetValue(fileName, out fileHeader))
            {
                var totalStreamsLength = 0L;
                var fileIndex = fileHeader.ZipFileIndex;
                var fileStreams = new List<FileStream>();

                // check header size
                if (fileHeader.LocalHeaderSize <= 0)
                {
                    var fileStream = GetZipFileStream(fileIndex);
                    fileStream.Seek(fileHeader.LocalHeaderOffset, SeekOrigin.Begin);

                    var localFileHeader = new TZipLocalFileHeader();
                    localFileHeader.Load(fileStream);

                    fileHeader.LocalHeaderSize = localFileHeader.Length;
                }

                // open streams
                while (totalStreamsLength < (fileHeader.LocalHeaderOffset + fileHeader.LocalHeaderSize + fileHeader.CompressedSize))
                {
                    fileStreams.Add(GetZipFileStream(fileIndex++));
                    totalStreamsLength += fileStreams.Last().Length;
                }

                return new UberStream(fileStreams.ToArray(), fileHeader.LocalHeaderOffset + fileHeader.LocalHeaderSize, fileHeader.UncompressedSize);
            }
            else return null;
        }

        public void CloseAllFileStreams()
        {
            for (int i = 0; i < m_ZipFileStreams.Length; i++)
            {
                if (m_ZipFileStreams[i].Target != null)
                {
                    var stream = m_ZipFileStreams[i].Target as FileStream;
                    stream.Close();
                    m_ZipFileStreams[i].Target = null;
                }
            }
        }

        #region implement IDisposable

        public void Dispose()
        {
            CloseAllFileStreams();
        }

        #endregion

        #region private methods

        private FileStream GetZipFileStream(int fileIndex)
        {
            FileStream stream = null;

            if (m_ZipFileStreams[fileIndex].Target != null)
                stream = m_ZipFileStreams[fileIndex].Target as FileStream;

            if (stream == null || !stream.CanRead)
            {
                if (stream != null) stream.Close();
                stream = File.Open(m_ZipFileNames[fileIndex],
                    FileMode.Open, FileAccess.Read, FileShare.Read);
                m_ZipFileStreams[fileIndex].Target = stream;
            }

            return stream;
        }

        /// <summary>
        /// Reads directory of ZipFiles.
        /// </summary>
        /// <param name="streams">FileStreams of zip container.</param>
        /// <param name="cd_diskId">Id of central directory in streams.</param>
        /// <param name="cd_offset">Offset of central directory in stream.</param>
        /// <param name="numberOfEntries">Number of entries in central directory.</param>
        private void ReadZipCentralDirectoryFileHeaders(FileStream[] streams, int cd_diskId, long cd_offset, int numberOfEntries)
        {
            var mainFileStream = streams[cd_diskId];
            mainFileStream.Seek(cd_offset, SeekOrigin.Begin);

            //Deserialize stream data according to number of Entries
            for (uint i = 0; i < numberOfEntries; i++)
            {
                var cd_fileHeader = new TZipCentralDirectoryFileHeader();
                cd_fileHeader.Load(mainFileStream);

                // is file or directory?
                var fileName = SanitizeFilename(cd_fileHeader.filename);
                if (fileName.Last() != '\\')
                {
                    m_Files.Add(fileName,
                        new SubFileHeader()
                        {
                            ZipFileIndex = cd_fileHeader.diskStart,
                            CompressedSize = (long)cd_fileHeader.sizeComp,
                            UncompressedSize = (long)cd_fileHeader.sizeUncomp,
                            LocalHeaderOffset = (long)cd_fileHeader.hdrRelOffset,
                            LocalHeaderSize = 0
                        });
                }
                else
                {
                    this.m_Directories.Add(fileName.Substring(0, cd_fileHeader.filename.Length - 1));
                }
            }
        }

        private string SanitizeFilename(string fileName)
        {
            if (!m_contentCaseSensitive)
                fileName = fileName.ToLower();

            return fileName
                .Replace('/', '\\');
            //.Replace('?', 'ä');
        }

        #endregion

        
    }
}
