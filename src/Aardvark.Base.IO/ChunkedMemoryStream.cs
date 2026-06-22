using Aardvark.Base;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aardvark.Base.Coder
{
    public class ChunkedMemoryStream : Stream
    {
        private readonly long m_chunkSize;
        readonly List<byte[]> m_chunkList;

        private long m_position;
        private int m_posChunk;
        private long m_posOffset;
        private long m_length;

        #region Constructor

        public ChunkedMemoryStream(long chunkSize)
        {
            if (chunkSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(chunkSize));

            m_chunkSize = chunkSize;
            m_chunkList = new List<byte[]>();
            m_chunkList.Add(CreateChunk());
            m_position = 0L;
            m_posChunk = 0;
            m_posOffset = 0L;

            m_length = 0L;
        }

        #endregion

        #region Properties

        public override bool CanRead
        {
            get { return true; }
        }

        public override bool CanSeek
        {
            get { return true; }
        }

        public override bool CanWrite
        {
            get { return true; }
        }

        public override long Length
        {
            get { return m_length; }
        }

        public override long Position
        {
            get
            {
                return m_position;
            }
            set
            {
                if (value < 0)
                    throw new ArgumentOutOfRangeException(nameof(value));

                SetPosition(value);
            }
        }

        public List<byte[]> ChunkList
        {
            get { return m_chunkList; }
        }

        #endregion

        public override void Flush()
        {
        }

        public override int ReadByte()
        {
            if (m_position < m_length)
            {
                int value = m_chunkList[m_posChunk][m_posOffset++];
                m_position++;
                if (m_posOffset >= m_chunkSize) { m_posChunk++; m_posOffset = 0L; }
                return value;
            }
            else
                return -1;
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ValidateReadWriteArguments(buffer, offset, count);

            var available = m_length - m_position;
            if (available <= 0)
                return 0;

            count = (int)Fun.Min((long)count, available);
            int done = 0;
            while (done < count)
            {
                var block = (int)Fun.Min((long)(count - done), m_chunkSize - m_posOffset);

                var chunk = m_chunkList[m_posChunk];
                Buffer.BlockCopy(chunk, (int)m_posOffset, buffer, offset + done, block);

                SetPosition(m_position + block);

                done += block;
            }
            return done;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            long position;
            switch (origin)
            {
                case SeekOrigin.Begin:
                    position = offset;
                    break;
                case SeekOrigin.Current:
                    position = CheckedAdd(m_position, offset);
                    break;
                case SeekOrigin.End:
                    position = CheckedAdd(m_length, offset);
                    break;
                default:
                    throw new ArgumentException("Invalid seek origin.", nameof(origin));
            }

            if (position < 0)
                throw new IOException("Cannot seek before the beginning of the stream.");

            SetPosition(position);
            return m_position;
        }

        public override void SetLength(long value)
        {
            if (value < 0)
                throw new ArgumentOutOfRangeException(nameof(value));

            if (value > m_length)
            {
                EnsureChunksForLength(value);
                ClearRange(m_length, value - m_length);
            }
            else if (value < m_length)
            {
                ClearTailFrom(value);
                TrimChunksForLength(value);
            }

            m_length = value;
        }

        public override void WriteByte(byte value)
        {
            EnsureChunkForPosition();

            m_chunkList[m_posChunk][m_posOffset] = value;
            SetPosition(m_position + 1);

            if (m_position > m_length)
                m_length = m_position;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ValidateReadWriteArguments(buffer, offset, count);

            int done = 0;
            while (done < count)
            {
                EnsureChunkForPosition();

                var block = (int)Fun.Min((long)(count - done), m_chunkSize - m_posOffset);

                var chunk = m_chunkList[m_posChunk];
                Buffer.BlockCopy(buffer, offset + done, chunk, (int)m_posOffset, block);
                SetPosition(m_position + block);

                done += block;
            }

            if (m_position > m_length)
                m_length = m_position;
        }

        private static void ValidateReadWriteArguments(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));
            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (buffer.Length - offset < count)
                throw new ArgumentException("The offset and count range exceeds the buffer length.", nameof(count));
        }

        private static long CheckedAdd(long value, long offset)
        {
            try
            {
                return checked(value + offset);
            }
            catch (OverflowException ex)
            {
                throw new IOException("Cannot seek to the requested position.", ex);
            }
        }

        private byte[] CreateChunk()
        {
            return new byte[m_chunkSize];
        }

        private void SetPosition(long value)
        {
            m_position = value;
            m_posChunk = (int)(value / m_chunkSize);
            m_posOffset = m_position % m_chunkSize;
        }

        private int ChunkCountForLength(long length)
        {
            if (length == 0)
                return 1;

            return (int)((length - 1) / m_chunkSize) + 1;
        }

        private void EnsureChunksForLength(long length)
        {
            var chunkCount = ChunkCountForLength(length);
            while (m_chunkList.Count < chunkCount)
                m_chunkList.Add(CreateChunk());
        }

        private void EnsureChunkForPosition()
        {
            while (m_chunkList.Count <= m_posChunk)
                m_chunkList.Add(CreateChunk());
        }

        private void TrimChunksForLength(long length)
        {
            var chunkCount = ChunkCountForLength(length);
            if (m_chunkList.Count > chunkCount)
                m_chunkList.RemoveRange(chunkCount, m_chunkList.Count - chunkCount);
        }

        private void ClearTailFrom(long position)
        {
            var chunkIndex = (int)(position / m_chunkSize);
            if (chunkIndex >= m_chunkList.Count)
                return;

            var chunkOffset = (int)(position % m_chunkSize);
            var chunk = m_chunkList[chunkIndex];
            Array.Clear(chunk, chunkOffset, chunk.Length - chunkOffset);
        }

        private void ClearRange(long position, long count)
        {
            while (count > 0)
            {
                var chunkIndex = (int)(position / m_chunkSize);
                var chunkOffset = (int)(position % m_chunkSize);
                var block = (int)Fun.Min(count, m_chunkSize - chunkOffset);

                Array.Clear(m_chunkList[chunkIndex], chunkOffset, block);

                position += block;
                count -= block;
            }
        }
    }
}
