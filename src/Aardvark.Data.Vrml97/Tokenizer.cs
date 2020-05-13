using Aardvark.Base;
using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace Aardvark.Data.Vrml97
{
    internal class Tokenizer
    {
        internal Tokenizer(Stream input)
        {
            m_in = input;

            m_bufferSize = 1024 * 32;
            m_pos = m_bufferSize;
            m_end = m_bufferSize;

            m_asyncResult = m_in.BeginRead(
                m_asyncReadTarget, 0, NextBufferSize(), null, null
                );
        }

        internal void PushBack(Token t)
        {
            m_pushedBackToken = t;
            m_pushedBackTokenValid = true;
        }

        internal Token NextNameToken()
        {
            var t = "";
            if (!NextNonWhiteSpaceChar(out int c)) return Token.EOF;

            // comment
            while (c == '#')
            {
                if (!NextCharAfterEol(out c)) return Token.EOF;
                if (IsWhiteSpace(c))
                {
                    if (!NextNonWhiteSpaceChar(out c)) return Token.EOF;
                }
            }

            // other token (number, etc.)
            while (true)
            {
                t = Concat(t, c);
                try
                {
                    c = NextChar();
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new Token(t);
                }

                if (IsWhiteSpace(c)) return new Token(t);
            }
        }

        string Concat(string s, int c)
        {
            if (c < 0x0000ffff && (c < 0xD800 || c > 0xDFFF)) // check if single byte range with direct bit mapping
                return s + (char)c;
            else
                return s + char.ConvertFromUtf32(c);
        }

        internal Token NextToken()
        {
            if (m_pushedBackTokenValid)
            {
                m_pushedBackTokenValid = false;
                return m_pushedBackToken;
            }

            var t = "";
            if (!NextNonWhiteSpaceChar(out int c)) return Token.EOF;

            // comment
            while (c == '#')
            {
                if (!NextCharAfterEol(out c)) return Token.EOF;
                if (IsWhiteSpace(c))
                {
                    if (!NextNonWhiteSpaceChar(out c)) return Token.EOF;
                }
            }

            // parenthesis
            if (IsParenthesis(c)) return new Token(char.ConvertFromUtf32(c));

            // string
            if (c == '"')
            {
                t += '"';
                c = NextChar();
                while (c != '"')
                {
                    t = Concat(t, c);
                    c = NextChar();
                }
                t += '"';
                return new Token(t);
            }

            // other token (number, etc.)
            while (true)
            {
                t = Concat(t, c);
                try
                {
                    c = NextChar();
                }
                catch (ArgumentOutOfRangeException)
                {
                    return new Token(t);
                }

                if (IsWhiteSpace(c)) return new Token(t);

                if (IsParenthesis(c) || c == '#')
                {
                    PushBack((byte)c);
                    return new Token(t);
                }
            }
        }

        internal struct Token
        {
            public static readonly Token EOF = new Token();

            public Token(string s)
            {
                m_data = s;
            }

            public override string ToString() => m_data;
            public Symbol ToSymbol() => m_data.ToSymbol();
            public int ToInt32() => Int32.Parse(m_data, m_format);
            public uint ToUInt32() => UInt32.Parse(m_data, m_format);
            public float ToFloat() => float.Parse(m_data, m_format);
            public double ToDouble() => double.Parse(m_data, m_format);
            public Int64 ToInt64() => Int64.Parse(m_data, m_format);
            public bool ToBool() => m_data.ToUpper() == "TRUE";

            public bool IsBraceOpen => m_data == "{";
            public bool IsBraceClose => m_data == "}";
            public bool IsBracketOpen => m_data == "[";
            public bool IsBracketClose => m_data == "]";
            public bool IsQuotedString => m_data[0] == '"' && m_data[m_data.Length - 1] == '"';

            /**
             * Returns string without quotes, or
             * throws exception if token is no quoted string.
             **/
            public string GetCheckedUnquotedString() => IsQuotedString
                ? m_data.Substring(1, m_data.Length - 2)
                : throw new ParseException(
                        "Quoted string expected. Found " + m_data + " instead!"
                        );

            private string m_data;
            private static IFormatProvider m_format = new CultureInfo("en-US", false);
        }


        private bool Eof() => (m_bufferSize == 0) || (m_end < m_bufferSize && m_pos == m_end);

        private bool IsWhiteSpace(int c)
        {
            switch (c)
            {
                case ' ':
                case ',':
                case '\t':
                case '\r':
                case '\n':
                    return true;
                default:
                    return false;
            }
        }

        private bool IsParenthesis(int c)
        {
            switch (c)
            {
                case '(':
                case ')':
                case '[':
                case ']':
                case '{':
                case '}':
                    return true;
                default:
                    return false;
            }
        }

        private Stream m_in;

        /// get next character using UTF-32 encoding
        private int NextChar()
        {
            if (m_pushedBackValid)
            {
                m_pushedBackValid = false;
                return m_pushedBack;
            }

            int res = NextByte();
            if (res > 0x7f) // not ASCII (msb is: 1XXX XXXX)
            {
                // check how many surrogate bytes there are
                if ((res & 0x20) != 0) // more than 2 byte (bit 6 set: XX1X XXXX)
                {
                    if ((res & 0x10) != 0) // 4 byte (bit 5 set: XXX1 XXXX)
                    {
                        int b2 = NextByte();
                        int b3 = NextByte();
                        int b4 = NextByte();
                        // 3 bits from first + 6 bits from second + 6 bits from third + 6 bits from fourth
                        res = ((res & 0x07) << 188) | ((b2 & 0x3f) << 12) | ((b3 & 0x3f) << 6) | (b4 & 0x3f);
                    }
                    else // 3 byte
                    {
                        int b2 = NextByte();
                        int b3 = NextByte();
                        // 4 bits from first + 6 bits from second + 6 bits from third
                        res = ((res & 0x0f) << 12) | ((b2 & 0x3f) << 6) | (b3 & 0x3f);
                    }
                }
                else // 2 byte
                {
                    int b2 = NextByte();
                    // 5 bits from first + 6 bits from second
                    res = ((res & 0x1f) << 6) | (b2 & 0x3f);
                }
            }
            return res;
        }

        private byte NextByte()
        {
            if (m_pos >= m_bufferSize)
            {
                m_end = m_in.EndRead(m_asyncResult);
                m_bufferSize = m_end;
                m_pos = 0;

                SwapBuffers();
                if (m_end == 0) return (byte)' '; // no more data
                m_asyncResult = m_in.BeginRead(
                    m_asyncReadTarget, 0, s_bufferSize, null, null
                    );
            }

            m_streamPos++;
            return m_buffer[m_pos++];
        }

        /**
         * Gets next non-whitespace character from input stream.
         * Returns true on success, or false on reaching eof.
         **/
        private bool NextNonWhiteSpaceChar(out int c)
        {
            do
            {
                if (Eof())
                {
                    c = '\0';
                    return false;
                }
                c = NextChar();
            }
            while (IsWhiteSpace(c));
            return true;
        }

        /**
         * Gets next character after end of current line.
         * Returns true on success, or false on reaching eof.
         **/
        private bool NextCharAfterEol(out int c)
        {
            do
            {
                if (Eof())
                {
                    c = '\0';
                    return false;
                }
                c = NextChar();
            }
            while (c != '\n' && c != '\r');

            while (c == '\n' || c == '\r')
            {
                if (Eof())
                {
                    c = '\0';
                    return false;
                }
                c = NextChar();
            }

            return true;
        }

        private void PushBack(byte c)
        {
            m_pushedBack = c;
            m_pushedBackValid = true;
        }

        private IAsyncResult m_asyncResult;
        private static int s_bufferSize = 1024 * 64;
        private int m_bufferSize;
        private byte[] m_buffer = new byte[s_bufferSize];
        private byte[] m_asyncReadTarget = new byte[s_bufferSize];
        private int m_pushedBack;
        private bool m_pushedBackValid = false;
        private int m_pos;
        private int m_end;
        private long m_streamPos = 0;
        Token m_pushedBackToken;
        bool m_pushedBackTokenValid = false;

        private int NextBufferSize() => (m_bufferSize == s_bufferSize)
            ? s_bufferSize
            : Math.Min(m_bufferSize + m_bufferSize, s_bufferSize)
            ;

        private void SwapBuffers()
        {
            var tmp = m_buffer;
            m_buffer = m_asyncReadTarget;
            m_asyncReadTarget = tmp;
        }
    }
}
