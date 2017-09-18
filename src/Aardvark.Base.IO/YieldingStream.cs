//using System.Diagnostics;
//using System.IO;
//using System.Threading;

//namespace Aardvark.Base.Coder
//{
//    public class YieldingStream : Stream
//    {
//        public YieldingStream(Stream stream)
//        {
//            m_stream = stream;
//        }

//        public override bool CanRead
//        {
//            get { return m_stream.CanRead; }
//        }

//        public override bool CanSeek
//        {
//            get { return m_stream.CanSeek; }
//        }

//        public override bool CanWrite
//        {
//            get { return m_stream.CanWrite; }
//        }

//        public override void Flush()
//        {
//            m_stream.Flush();
//            Thread.Sleep(0);
//        }

//        public override long Length
//        {
//            get { return m_stream.Length; }
//        }

//        public override long Position
//        {
//            get
//            {
//                return m_stream.Position;
//            }
//            set
//            {
//                m_stream.Position = value;
//            }
//        }

//        public override int Read(byte[] buffer, int offset, int count)
//        {
//            Trace.WriteLine(
//                "YieldingStream: Read, offset = " + offset + ", " +
//                "count = " + count);
//            int result = 0;
//            for (int i = 0; i < count; i += 8192)
//            {
//                int c = System.Math.Min(8192, count - i);
//                result += m_stream.Read(buffer, offset + i, c);
//                Thread.Sleep(0);
//            }
            
//            return result;
//        }

//        public override long Seek(long offset, SeekOrigin origin)
//        {
//            long result = m_stream.Seek(offset, origin);
//            Thread.Sleep(0);
//            return result;
//        }

//        public override void SetLength(long value)
//        {
//            m_stream.SetLength(value);
//            Thread.Sleep(0);
//        }

//        public override void Write(byte[] buffer, int offset, int count)
//        {
//            m_stream.Write(buffer, offset, count);
//            Thread.Sleep(0);
//        }

//        private Stream m_stream;
//    }
//}
