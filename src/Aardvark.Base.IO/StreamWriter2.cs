using System;
using System.IO;
using System.Text;

namespace Aardvark.VRVis
{
    /// <summary>
    /// Due to a bug, it is not possible to set the FormatProvider in StreamWriter constructor.
    /// This is a workaround.
    /// </summary>
    public class StreamWriter2 : StreamWriter
    {
        //constructors from base class
        public StreamWriter2(Stream stream)
            : base(stream)
        { }
        public StreamWriter2(string path)
            : base(path)
        { }
        public StreamWriter2(Stream stream, Encoding encoding)
            : base(stream, encoding)
        { }
        public StreamWriter2(string path, bool append)
            : base(path, append)
        { }
        public StreamWriter2(Stream stream, Encoding encoding, int bufferSize)
            : base(stream, encoding, bufferSize)
        { }
        public StreamWriter2(string path, bool append, Encoding encoding)
            : base(path, append, encoding)
        { }
        public StreamWriter2(string path, bool append, Encoding encoding, int bufferSize)
            : base(path, append, encoding, bufferSize)
        { }

        //constructors from base class with additional parameter formatProvider
        public StreamWriter2(Stream stream, IFormatProvider formatProvider, string newLine)
            : base(stream)
        { m_internalFormatProvider = formatProvider; NewLine = newLine; }
        public StreamWriter2(string path, IFormatProvider formatProvider, string newLine)
            : base(path)
        { m_internalFormatProvider = formatProvider; NewLine = newLine; }
        public StreamWriter2(Stream stream, Encoding encoding, IFormatProvider formatProvider, string newLine)
            : base(stream, encoding)
        { m_internalFormatProvider = formatProvider; NewLine = newLine; }
        public StreamWriter2(string path, bool append, IFormatProvider formatProvider, string newLine)
            : base(path, append)
        { m_internalFormatProvider = formatProvider; NewLine = newLine; }
        public StreamWriter2(Stream stream, Encoding encoding, int bufferSize, IFormatProvider formatProvider, string newLine)
            : base(stream, encoding, bufferSize)
        { m_internalFormatProvider = formatProvider; NewLine = newLine; }
        public StreamWriter2(string path, bool append, Encoding encoding, IFormatProvider formatProvider, string newLine)
            : base(path, append, encoding)
        { m_internalFormatProvider = formatProvider; NewLine = newLine; }
        public StreamWriter2(string path, bool append, Encoding encoding, int bufferSize, IFormatProvider formatProvider, string newLine)
            : base(path, append, encoding, bufferSize)
        { m_internalFormatProvider = formatProvider; NewLine = newLine; }

        //hack, to change the format provider
        protected IFormatProvider m_internalFormatProvider = null;

        public override IFormatProvider FormatProvider
        {
            get
            {
                if (this.m_internalFormatProvider == null)
                {
                    return base.FormatProvider;
                }
                return this.m_internalFormatProvider;
            }
        }

    }

}
