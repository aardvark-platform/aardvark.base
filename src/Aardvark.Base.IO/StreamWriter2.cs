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

namespace Aardvark.Base.Coder;

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
