using Aardvark.Base;
using System.Collections.Generic;
using System.IO;

namespace Aardvark.VRVis
{
    public class ChunkedMemoryStream : Stream
    {
        private long m_chunkSize;
        List<byte[]> m_chunkList;

        private long m_position;
        private int m_posChunk;
        private long m_posOffset;
        private long m_length;

        #region Constructor

        public ChunkedMemoryStream(long chunkSize)
        {
            m_chunkSize = chunkSize;
            m_chunkList = new List<byte[]>();
            m_chunkList.Add(new byte[m_chunkSize]);
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
                m_position = value;
                m_posChunk = (int)(value / m_chunkSize);
                m_posOffset = m_position % m_chunkSize;
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
            count = Fun.Min(count, (int)(m_length - m_position));
            int done = 0;
            while (done < count)
            {
                var block = (int)Fun.Min((long)(count - done), m_chunkSize - m_posOffset);

                var chunk = m_chunkList[m_posChunk];
                for (int end = offset + block; offset < end; offset++)
                    buffer[offset] = chunk[m_posOffset++];

                if (m_posOffset >= m_chunkSize) { ++m_posChunk; m_posOffset = 0L; }

                done += block;
            }
            m_position += done;
            return done;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin: m_position = offset; break;
                case SeekOrigin.Current: m_position += offset; break;
                case SeekOrigin.End: m_position = m_length + offset; break;
            }
            m_posChunk = (int)(m_position / m_chunkSize);
            m_posOffset = m_position % m_chunkSize;
            return m_position;
        }

        public override void SetLength(long value)
        {
            var newChunk = (int)(value / m_chunkSize);
            var oldChunk = (int)(m_length / m_chunkSize);

            if (oldChunk > newChunk)
            {
                m_chunkList.RemoveRange(newChunk + 1, oldChunk - newChunk);
            }
            else if (oldChunk < newChunk)
            {
                while (oldChunk < newChunk)
                {
                    m_chunkList.Add(new byte[m_chunkSize]);
                    oldChunk++;
                }
            }
            m_length = value;
        }

        public override void WriteByte(byte value)
        {
            m_chunkList[m_posChunk][m_posOffset++] = value;
            if (m_posOffset >= m_chunkSize)
            {
                ++m_posChunk; m_posOffset = 0;
                if (m_posChunk >= m_chunkList.Count)
                    m_chunkList.Add(new byte[m_chunkSize]);
            }
            ++m_position; if (m_position > m_length) m_length = m_position;
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            int done = 0;
            while (done < count)
            {
                var block = (int)Fun.Min((long)(count - done), m_chunkSize - m_posOffset);

                var chunk = m_chunkList[m_posChunk];
                for (int end = offset + block; offset < end; offset++)
                    chunk[m_posOffset++] = buffer[offset];

                if (m_posOffset >= m_chunkSize)
                {
                    ++m_posChunk; m_posOffset = 0L;
                    if (m_posChunk >= m_chunkList.Count)
                        m_chunkList.Add(new byte[m_chunkSize]);
                }

                done += block;
            }
            m_position += done; if (m_position > m_length) m_length = m_position;
        }
    }
}
