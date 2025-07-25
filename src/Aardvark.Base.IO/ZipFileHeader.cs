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
using System.Text;

// Important: .NET 4.5 contains full zip support.
namespace Aardvark.Base.Coder.Legacy
{
    // Based on .ZIP File Format Specification Version 6.3.3

    // ZIP file format:
    //
    // [local file header 1]
    // [encryption header 1]
    // [file data 1]
    // [data descriptor 1] (optional)
    // ...
    // ...
    // [local file header n]
    // [encryption header n]
    // [file data n]
    // [data descriptor n]
    // [archive decryption header] (necessary if central directory is encrypted)
    // [archive extra data record] (optional)
    // [central directory header 1]
    // ...
    // ...
    // [central directory header n]
    // [zip64 end of central directory record]
    // [zip64 end of central directory locator]
    // [end of central directory record]

    // Zip Signatures:
    //
    //PK = 0x4b50,
    //LocalFileHeader = 0x04034b50,
    //DataDescriptor = 0x08074b50,
    //ArchiveExtraDataRecord = 0x08064b50,
    //CentralDirectoryFileHeader = 0x02014b50,
    //DigitalSignature = 0x05054b50,
    //Zip64EndOfCentralDirectoryRecord = 0x06064b50,
    //Zip64EndOfCentralDirectoryLocator = 0x07064b50,
    //EndOfCentralDirectoryRecord = 0x06054b50,
    //Zip64ExtendedInfoExtraField = 0x0001

    /// <summary>
    /// Important: .NET 4.5 contains full zip support.
    /// </summary>
    public struct TZipLocalFileHeader
    {
        public const UInt32 c_Signature = 0x04034b50;
        public const long MinLength = 30;
        public const long MaxLength = MinLength + 2 * UInt16.MaxValue;

        public readonly long Length
        {
            get { return MinLength + lengthFileName + lengthExtra; }
        }

        #region public fields and properties

        /// <summary>
        /// version needed to extract
        /// </summary>
        public UInt16 verNeeded;
        /// <summary>
        /// general purpose bit flag
        /// </summary>
        public UInt16 flag;
        /// <summary>
        /// compression method
        /// </summary>
        public UInt16 compression;
        /// <summary>
        /// last mod file time
        /// </summary>
        public UInt16 modTime;
        /// <summary>
        /// last mod file date
        /// </summary>
        public UInt16 modDate;
        /// <summary>
        /// crc-32
        /// </summary>
        public UInt32 crc32;
        /// <summary>
        /// compressed size
        /// </summary>
        public UInt32 sizeComp;
        /// <summary>
        /// uncompressed size
        /// </summary>
        public UInt32 sizeUncomp;
        /// <summary>
        /// file name length
        /// </summary>
        public UInt16 lengthFileName;
        /// <summary>
        /// extra field length
        /// </summary>
        public UInt16 lengthExtra;

        public string filename;
        ///// <summary>
        ///// extra field (variable size)
        ///// </summary>
        //public UInt32 xtraField;

        #endregion

        /// <summary>
        /// Loads header from the stream at the current position.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public TZipLocalFileHeader Load(Stream stream)
        {
            return Load(new BinaryReader(stream));
        }

        public TZipLocalFileHeader Load(BinaryReader binReader)
        {
            if (c_Signature != binReader.ReadUInt32())
                throw new Exception("Zip: wrong signature of local-file-header.");

            verNeeded = binReader.ReadUInt16();
            flag = binReader.ReadUInt16();
            compression = binReader.ReadUInt16();
            modTime = binReader.ReadUInt16();
            modDate = binReader.ReadUInt16();
            crc32 = binReader.ReadUInt32();
            sizeComp = binReader.ReadUInt32();
            sizeUncomp = binReader.ReadUInt32();
            lengthFileName = binReader.ReadUInt16();
            lengthExtra = binReader.ReadUInt16();

            //Deserialize the filename, depending on fNameLen and save in string
            byte[] fileNameByte = new byte[lengthFileName];
            binReader.Read(fileNameByte, 0, (int)lengthFileName);
            filename = ASCIIEncoding.ASCII.GetString(fileNameByte);

            // skip rest of header
            binReader.BaseStream.Seek(lengthExtra, SeekOrigin.Current);

            return this;
        }
    }

    /// <summary>
    /// Important: .NET 4.5 contains full zip support.
    /// </summary>
    public struct TZipCentralDirectoryFileHeader
    {
        public const UInt32 c_Signature = 0x02014b50;
        public const UInt32 c_Zip64ExtInfoExtraFieldSignature = 0x0001;
        //public const long MinLength = 46;
        //public const long MaxLength = MinLength + 3 * UInt16.MaxValue;

        //public long Length
        //{
        //    get { return MinLength + lengthFileName + lengthExtra + lengthComment; }
        //}

        #region public fields and properties

        /// <summary>
        /// version made by
        /// </summary>
        public UInt16 verMade;
        /// <summary>
        /// version needed to extract
        /// </summary>
        public UInt16 verNeeded;
        /// <summary>
        /// general purpose bit flag
        /// </summary>
        public UInt16 flag;
        /// <summary>
        /// compression method
        /// </summary>
        public UInt16 compression;
        /// <summary>
        /// last mod file time
        /// </summary>
        public UInt16 modTime;
        /// <summary>
        /// last mod file date
        /// </summary>
        public UInt16 modDate;
        /// <summary>
        /// crc-32
        /// </summary>
        public UInt32 crc32;
        /// <summary>
        /// compressed size
        /// </summary>
        public UInt32 sizeComp;
        /// <summary>
        /// uncompressed size
        /// </summary>
        public UInt32 sizeUncomp;
        /// <summary>
        /// file name length
        /// </summary>
        public UInt16 lengthFileName;
        /// <summary>
        /// extra field length
        /// </summary>
        public UInt16 lengthExtra;
        /// <summary>
        /// file comment length
        /// </summary>
        public UInt16 lengthComment;
        /// <summary>
        /// disk number start
        /// </summary>
        public UInt16 diskStart;
        /// <summary>
        /// internal file attributes
        /// </summary>
        public UInt16 attrInt;
        /// <summary>
        /// external file attributes
        /// </summary>
        public UInt32 attrExt;
        /// <summary>
        /// relative offset of local header
        /// </summary>
        public UInt64 hdrRelOffset; // 32bit in Zip, 64bit in Zip64
        /// <summary>
        /// file name (variable size)
        /// </summary>
        public string filename;
        ///// <summary>
        ///// extra field (variable size)
        ///// </summary>
        //public UInt32 xtraField;
        ///// <summary>
        ///// file comment (variable size)
        ///// </summary>
        //public string fileComment;

        #endregion

        /// <summary>
        /// Loads header from the stream at the current position.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public TZipCentralDirectoryFileHeader Load(Stream stream)
        {
            return Load(new BinaryReader(stream));
        }

        public TZipCentralDirectoryFileHeader Load(BinaryReader binReader)
        {
            if (c_Signature != binReader.ReadUInt32())
                throw new Exception("Zip: wrong signature of central-direcory-file-header.");

            verMade = binReader.ReadUInt16();
            verNeeded = binReader.ReadUInt16();
            flag = binReader.ReadUInt16();
            compression = binReader.ReadUInt16();
            modTime = binReader.ReadUInt16();
            modDate = binReader.ReadUInt16();
            crc32 = binReader.ReadUInt32();
            sizeComp = binReader.ReadUInt32();
            sizeUncomp = binReader.ReadUInt32();
            lengthFileName = binReader.ReadUInt16();
            lengthExtra = binReader.ReadUInt16();
            lengthComment = binReader.ReadUInt16();
            diskStart = binReader.ReadUInt16();
            attrInt = binReader.ReadUInt16();
            attrExt = binReader.ReadUInt32();
            hdrRelOffset = binReader.ReadUInt32();

            //Deserialize the filename, depending on fNameLen and save in string
            byte[] fileNameByte = new byte[lengthFileName];
            binReader.Read(fileNameByte, 0, (int)lengthFileName);
            filename = ASCIIEncoding.ASCII.GetString(fileNameByte);

            if (hdrRelOffset == 0xFFFFFFFF)
            {
                if (c_Zip64ExtInfoExtraFieldSignature == binReader.ReadUInt16())
                {
                    /* var size = */ binReader.ReadUInt16();
                    hdrRelOffset = binReader.ReadUInt64();
                }

                // skip rest of header
                binReader.BaseStream.Seek(lengthExtra - 12 + lengthComment, SeekOrigin.Current);
            }
            else
            {   // skip rest of header
                binReader.BaseStream.Seek(lengthExtra + lengthComment, SeekOrigin.Current);
            }

            return this;
        }
    }

    /// <summary>
    /// Important: .NET 4.5 contains full zip support.
    /// </summary>
    public struct TZip64EndOfCentralDirectoryRecord
    {
        public const UInt32 signature = 0x06064b50;
        //public const long MinLength = 56;
        //public const long MaxLength = 12L + (long)UInt64.MaxValue;

        //public long Length
        //{
        //    get { return 12L + (long)recordSize; }
        //}


        #region public fields

        /// <summary>
        /// size of zip64 end of central directory record (without leading 12bits)
        /// </summary>
        public UInt64 recordSize;
        /// <summary>
        /// version made by
        /// </summary>
        public UInt16 verMade;
        /// <summary>
        /// version needed to extract
        /// </summary>
        public UInt16 verNeeded;
        /// <summary>
        /// number of this disk
        /// </summary>
        public UInt32 diskId;
        /// <summary>
        /// number of the disk with the start of the central directory
        /// </summary>
        public UInt32 cd_diskId;
        /// <summary>
        /// total number of entries in the central directory on this disk
        /// </summary>
        public UInt64 cd_totalDirEntriesOnDisk;
        /// <summary>
        /// total number of entries in the central directory
        /// </summary>
        public UInt64 cd_totalEntries;
        /// <summary>
        /// size of the central directory
        /// </summary>
        public UInt64 cd_size;
        /// <summary>
        /// offset of start of central directory with respect to the starting disk number
        /// </summary>
        public UInt64 cd_offset;
        ///// <summary>
        ///// zip64 extensible data sector
        ///// </summary>
        //public ?? extData;

        #endregion

        /// <summary>
        /// Loads record from stream at current position.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="position">Position of record in given stream.</param>
        /// <returns></returns>
        public TZip64EndOfCentralDirectoryRecord Load(Stream stream, long position)
        {
            return Load(new BinaryReader(stream), position);
        }

        public TZip64EndOfCentralDirectoryRecord Load(BinaryReader binReader, long position)
        {
            binReader.BaseStream.Seek(position, SeekOrigin.Begin);

            if (signature != binReader.ReadUInt32())
                throw new Exception("Zip64: wrong signature of end-of-central-direcory-record.");

            recordSize = binReader.ReadUInt64();
            verMade = binReader.ReadUInt16();
            verNeeded = binReader.ReadUInt16();
            diskId = binReader.ReadUInt32();
            cd_diskId = binReader.ReadUInt32();
            cd_totalDirEntriesOnDisk = binReader.ReadUInt64();
            cd_totalEntries = binReader.ReadUInt64();
            cd_size = binReader.ReadUInt64();
            cd_offset = binReader.ReadUInt64();

            return this;
        }
    }

    /// <summary>
    /// Important: .NET 4.5 contains full zip support.
    /// </summary>
    public struct TZip64EndOfCentralDirectoryLocator
    {
        public const UInt32 signature = 0x07064b50;
        public const long Length = 20;

        /// <summary>
        /// True if Locator exists.
        /// </summary>
        public bool IsValid;

        #region public fields

        /// <summary>
        /// number of the disk with the start of the zip64 end of central directory
        /// </summary>
        public UInt32 ecd64_diskId;
        /// <summary>
        /// relative offset of the zip64 end of central directory record
        /// </summary>
        public UInt64 ecd64_relOffset;
        /// <summary>
        /// total number of disks
        /// </summary>
        public UInt32 totalDisks;

        #endregion

        /// <summary>
        /// Loads Locator from the stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="ecd_position">Position of the 'End of Central Directory Record'</param>
        /// <returns></returns>
        public TZip64EndOfCentralDirectoryLocator Load(Stream stream, long ecd_position)
        {
            return Load(new BinaryReader(stream), ecd_position);
        }

        public TZip64EndOfCentralDirectoryLocator Load(BinaryReader binReader, long ecd_position)
        {
            // Seek to the beginning of the possible Zip64 Locator.
            binReader.BaseStream.Seek(ecd_position - Length, SeekOrigin.Begin);

            IsValid = signature == binReader.ReadUInt32();

            if (IsValid)
            {
                ecd64_diskId = binReader.ReadUInt32();
                ecd64_relOffset = binReader.ReadUInt64();
                totalDisks = binReader.ReadUInt32();
            }

            return this;
        }

    }

    /// <summary>
    /// Important: .NET 4.5 contains full zip support.
    /// </summary>
    public struct TZipEndOfCentralDirectoryRecord
    {
        public const UInt32 signature = 0x06054b50;
        public const long MinLength = 22;
        public const long MaxLength = MinLength + UInt16.MaxValue;

        public readonly long Length
        {
            get { return MinLength + cmntLen; }
        }

        public long Position;

        #region public fields read from zip

        /// <summary>
        /// Number of this disk
        /// </summary>
        public UInt16 diskId;
        /// <summary>
        /// number of the disk with the start of the central directory
        /// </summary>
        public UInt16 cd_diskId;
        /// <summary>
        /// total number of entries in the central directory on this disk
        /// </summary>
        public UInt16 cd_totalDirEntriesOnDisk;
        /// <summary>
        /// total number of entries in the central directory
        /// </summary>
        public UInt16 cd_totalEntries;
        /// <summary>
        /// size of the central directory
        /// </summary>
        public UInt32 cd_size;
        /// <summary>
        /// offset of start of central directory with respect to the starting disk number
        /// </summary>
        public UInt32 cd_offset;
        /// <summary>
        /// .ZIP file comment length
        /// </summary>
        public UInt16 cmntLen;
        ///// <summary>
        ///// .ZIP file comment (variable size)
        ///// </summary>
        //public string comment;

        #endregion

        /// <summary>
        /// Finds position of record and loads it.
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public TZipEndOfCentralDirectoryRecord Load(Stream stream)
        {
            Position = FindPositionOfRecord(stream);
            if (Position < 0) return this;

            var binReader = new BinaryReader(stream);
            binReader.BaseStream.Seek(Position + 4, SeekOrigin.Begin);

            diskId = binReader.ReadUInt16();
            cd_diskId = binReader.ReadUInt16();
            cd_totalDirEntriesOnDisk = binReader.ReadUInt16();
            cd_totalEntries = binReader.ReadUInt16();
            cd_size = binReader.ReadUInt32();
            cd_offset = binReader.ReadUInt32();
            cmntLen = binReader.ReadUInt16();

            return this;
        }

        static private long FindPositionOfRecord(Stream stream)
        {
            var pos = MinLength;
            var binReader = new BinaryReader(stream);

            while (pos <= MaxLength && pos < stream.Length)
            {
                // find signature
                stream.Seek(-pos, SeekOrigin.End);
                if (signature == binReader.ReadUInt32())
                {
                    // check total length of record to verify
                    stream.Seek(16, SeekOrigin.Current);
                    var commentLength = binReader.ReadUInt16();

                    if ((MinLength + commentLength) == pos)
                        return stream.Length - pos;
                }

                pos += 8; // jump to next element for checking
            }

            //throw new Exception("Zip: wrong signature of end-of-central-direcory-record.");
            return -1;
        }
    }
}
