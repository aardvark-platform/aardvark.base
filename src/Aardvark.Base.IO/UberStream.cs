using Aardvark.Base;
using System;
using System.IO;
using System.Linq;

namespace Aardvark.Base.Coder
{
    /// <summary>
    /// To bundle multiple streams to one or create a sub-stream for an existing stream.
    /// </summary>
    public class UberStream : Stream
    {
        private struct SubStream
        {
            public Stream Stream;
            public long StartPos; // start position in underlying stream
            public long Length; // number of elements the substream covers
            public long PosOffset; // pos offset in UberStream
        }

        #region private fields

        private long m_position;
        private readonly long m_totalLength;
        private readonly SubStream[] m_streams;

        #endregion

        #region validation

        private static void ValidateStartAndLength(long startPos, long numOfBytes)
        {
            if (startPos < 0)
                throw new ArgumentOutOfRangeException(nameof(startPos), "startPos can not be less than 0.");

            if (numOfBytes < 0)
                throw new ArgumentOutOfRangeException(nameof(numOfBytes), "numOfBytes can not be less than 0.");
        }

        private static void ValidateUberStreamBufferArguments(byte[] buffer, int offset, int count)
        {
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            if (offset < 0)
                throw new ArgumentOutOfRangeException(nameof(offset));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (offset > buffer.Length - count)
                throw new ArgumentException("Offset and count do not describe a valid range in the buffer.", nameof(count));
        }

        #endregion

        #region private methods

        private int DoItSo(byte[] buffer, int offset, int count, Func<Stream, byte[], int, int, int> func)
        {
            if (m_position < 0 || m_position >= m_totalLength) return 0;

            var firstId = m_streams.FindIndex(true, sub => sub.PosOffset + sub.Length > m_position);

            var haveBeenDone = 0;
            var shouldBeenDone = 0;
            for (int i = firstId;
                shouldBeenDone < count && i < m_streams.Count();
                i++)
            {
                var sub = m_streams[i];
                var subPosition = m_position + shouldBeenDone - sub.PosOffset;

                // set position in sub-stream
                if (sub.StartPos != 0 || subPosition != 0)
                    sub.Stream.Position = sub.StartPos + subPosition;

                // calculate bytes to process in sub-stream
                var bytesToDo = (int)(sub.Length - subPosition);
                if (bytesToDo > count - shouldBeenDone)
                    bytesToDo = count - shouldBeenDone;

                // do it so
                haveBeenDone += func(sub.Stream, buffer, offset + shouldBeenDone, bytesToDo);
                shouldBeenDone += bytesToDo;
            }
            m_position += shouldBeenDone;

            return haveBeenDone;
        }

        #endregion
        
        #region constructors

        /// <summary>
        /// Create Sub-stream.
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="startPos"></param>
        /// <param name="numOfBytes"></param>
        public UberStream(Stream stream, long startPos, long numOfBytes)
            : base()
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            ValidateStartAndLength(startPos, numOfBytes);

            if (stream.CanSeek && startPos > stream.Length - numOfBytes)
                throw new ArgumentOutOfRangeException(nameof(numOfBytes));

            m_streams = new SubStream[]
            {
                new SubStream()
                {
                    Stream = stream,
                    StartPos = startPos,
                    Length = numOfBytes,
                    PosOffset = 0
                }
            };

            m_position = 0;
            m_totalLength = numOfBytes;
        }

        /// <summary>
        /// Bundle streams to one in given order.
        /// Stream-1 (startPos->End) ... Stream-n (Begin->End) ... Stream-last (Begin->endPos)
        /// endPos = Stream-last.length - (AllStreams.length - startPos - numOfBytes)
        /// </summary>
        /// <param name="streams"></param>
        /// <param name="startPos">Position in first stream, where bundle should begin.</param>
        /// <param name="numOfBytes">Number of bytes the uber-stream covers.</param>
        public UberStream(Stream[] streams, long startPos, long numOfBytes)
            : base()
        {
            if (streams == null)
                throw new ArgumentNullException(nameof(streams));

            if (streams.Length == 0)
                throw new ArgumentException("At least one stream is required.", nameof(streams));

            ValidateStartAndLength(startPos, numOfBytes);

            if (streams.Any(s => s == null))
                throw new ArgumentNullException(nameof(streams), "Stream array can not contain null entries.");

            if (streams.All(s => s.CanSeek) && startPos > streams.Select(s => s.Length).Sum() - numOfBytes)
                throw new ArgumentOutOfRangeException(nameof(numOfBytes));

            m_position = 0;
            m_totalLength = numOfBytes;

            m_streams = new SubStream[streams.Count()];

            // set first element
            if (streams.First().Length < startPos)
                throw new ArgumentException("First stream is shorter than startPos.");

            m_streams[0] = new SubStream()
            {
                Stream = streams.First(),
                StartPos = startPos,
                Length = streams.First().Length - startPos,
                PosOffset = 0
            };

            // set elements
            for (var i = 1; i < streams.Count(); i++)
            {
                m_streams[i] = new SubStream()
                {
                    Stream = streams[i],
                    StartPos = 0,
                    Length = streams[i].Length,
                    PosOffset = m_streams[i - 1].PosOffset + m_streams[i - 1].Length
                };
            }

            // set last element
            var streamsTotalLength = streams.Select(stream => stream.Length).Sum();
            var diff = streamsTotalLength - startPos - numOfBytes;

            if (diff < 0)
                throw new ArgumentException("streams don't have enough length (< startPos + numOfBytes).");

            if (streams.Last().Length < diff)
                throw new ArgumentException("Last stream is shorter than the requested bytes.");

            m_streams[m_streams.Length - 1] = new SubStream()
            {
                Stream = m_streams[m_streams.Length - 1].Stream,
                StartPos = m_streams[m_streams.Length - 1].StartPos,
                Length = m_streams[m_streams.Length - 1].Length - diff,
                PosOffset = m_streams[m_streams.Length - 1].PosOffset // numOfBytes - Length // also: streamsTotalLength - streams.Last().Length - startPos
            };
        }

        //public UberStream(IEnumerable<Tup<Stream, long, long>> streams)
        //{
        //}

        #endregion

        #region implement Stream

        public override bool CanRead
        {
            get { return m_streams.All(sub => sub.Stream.CanRead); }
        }

        public override bool CanSeek
        {
            get { return m_streams.All(sub => sub.Stream.CanSeek); }
        }

        public override bool CanWrite
        {
            get { return m_streams.All(sub => sub.Stream.CanWrite); }
        }

        public override long Length
        {
            get { return m_totalLength; }
        }

        public override long Position
        {
            get
            {
                return m_position;
            }
            set
            {
                Seek(value, SeekOrigin.Begin);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                foreach (var subStream in m_streams)
                    subStream.Stream.Dispose();

            base.Dispose(disposing);
        }

        public override void Flush()
        {
            foreach (var sub in m_streams)
                sub.Stream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            ValidateUberStreamBufferArguments(buffer, offset, count);

            if (!CanRead)
                throw new NotSupportedException();

            //throw new ObjectDisposedException();

            return DoItSo(buffer, offset, count,
                (s, b, o, c) => s.Read(b, o, c));
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            if (!CanSeek) throw new NotSupportedException();
            //throw new IOException();
            //throw new ObjectDisposedException();

            switch (origin)
            {
                case SeekOrigin.Begin:
                    m_position = offset;
                    break;
                case SeekOrigin.End:
                    m_position = m_totalLength + offset;
                    break;
                case SeekOrigin.Current:
                    m_position += offset;
                    break;
            }

            return m_position;
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            ValidateUberStreamBufferArguments(buffer, offset, count);

            if (!CanWrite)
                throw new NotSupportedException();

            DoItSo(buffer, offset, count,
                (s, b, o, c) => { s.Write(b, o, c); return c; });
        }

        #endregion
    }

    //public class SubStream : Stream
    //{
    //    #region private fields
    //    private Stream m_stream;
    //    private long m_startPos; // start position in underlying stream
    //    private long m_length; // number of elements the substream covers
    //    #endregion

    //    #region constructors

    //    public SubStream(Stream stream, long startPos, long numOfElements)
    //        : base()
    //    {
    //        m_stream = stream;
    //        m_startPos = startPos;
    //        m_length = numOfElements;
    //    }

    //    #endregion

    //    #region implement Stream

    //    public override bool CanRead
    //    {
    //        get { return m_stream.CanRead; }
    //    }

    //    public override bool CanSeek
    //    {
    //        get { return m_stream.CanSeek; }
    //    }

    //    public override bool CanWrite
    //    {
    //        get { return m_stream.CanWrite; }
    //    }

    //    public override void Flush()
    //    {
    //        m_stream.Flush();
    //    }

    //    public override long Length
    //    {
    //        get { return m_length; }
    //    }

    //    public override long Position
    //    {
    //        get
    //        {
    //            return m_stream.Position - m_startPos;
    //        }
    //        set
    //        {
    //            m_stream.Position = m_startPos + value;
    //        }
    //    }

    //    public override int Read(byte[] buffer, int offset, int count)
    //    {
    //        if ((Position + count) > m_length)
    //            count = (int)(m_length - Position);

    //        return m_stream.Read(buffer, offset, count);
    //    }

    //    public override long Seek(long offset, SeekOrigin origin)
    //    {
    //        switch (origin)
    //        {
    //            case SeekOrigin.Begin:
    //                offset += m_startPos;
    //                break;
    //            case SeekOrigin.End:
    //                offset += m_length - m_stream.Length;
    //                break;
    //            //case SeekOrigin.Current:
    //        }

    //        return m_stream.Seek(offset, origin);
    //    }

    //    public override void SetLength(long value)
    //    {
    //        m_length = value;

    //        if ((value + m_startPos) > m_stream.Length)
    //            m_stream.SetLength(m_startPos + value);
    //    }

    //    public override void Write(byte[] buffer, int offset, int count)
    //    {
    //        m_stream.Write(buffer, offset, count);
    //    }

    //    #endregion
    //}
}
