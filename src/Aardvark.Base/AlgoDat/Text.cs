using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Aardvark.Base
{
    /// <summary>
    /// A facade structure that represents a part of a string.
    /// This can be used to do string manipulations without copying
    /// too many strings.
    /// </summary>
    public struct Text : IEquatable<Text>
    {
        public readonly int Start;
        public readonly int End;
        public readonly string String;

        #region Constructors

        public Text(string str)
        {
            Start = 0;
            End = str.Length;
            String = str;
        }

        public Text(string str, int start)
        {
            Start = start;
            End = str.Length;
            String = str;
        }

        public Text(string str, int start, int count)
        {
            Start = start;
            End = start + count;
            String = str;
        }

        public Text(int start, int end, string str)
        {
            Start = start;
            End = end;
            String = str;
        }

        #endregion

        #region Constants

        public static readonly Text Empty = new Text(null, 0, 0);

        #endregion

        #region Properties

        public int Count
        {
            get { return End - Start; }
        }

        public bool IsEmpty
        {
            get { return Count <= 0; }
        }

        public bool IsWhiteSpace
        {
            get { return IsOnly(CharFun.IsWhiteSpace); }
        }

        public Text WhiteSpaceAtStartTrimmed
        {
            get { return TrimmedAtStart(CharFun.IsWhiteSpace); }
        }

        public Text WhiteSpaceAtEndTrimmed
        {
            get { return TrimmedAtEnd(CharFun.IsWhiteSpace); }
        }

        public Text WhiteSpaceTrimmed
        {
            get { return Trimmed(CharFun.IsWhiteSpace); }
        }

        #endregion

        #region Indexer

        public char this[int index]
        {
            get { return String[Start + index]; }
        }

        #endregion

        #region Processing Methods

        public int IndexOf(char ch)
        {
            var index = String.IndexOf(ch, Start, Count);
            return index < 0 ? -1 : index - Start;
        }

        public int IndexOf(string str)
        {
            var index = String.IndexOf(str, Start, Count);
            return index < 0 ? -1 : index - Start;
        }

        public int IndexOf(char ch, int start)
        {
            if (start >= 0) { start += Start; if (start >= End) return -1; }
            else start = Math.Max(Start, start + End);
            var index = String.IndexOf(ch, start, End - start);
            return index < 0 ? -1 : index - Start;
        }

        public int IndexOf(string str, int start)
        {
            if (start >= 0) { start += Start; if (start >= End) return -1; }
            else start = Math.Max(Start, start + End);
            var index = String.IndexOf(str, start, End - start);
            return index < 0 ? -1 : index - Start;
        }

        public int IndexOf(char ch, int start, int count)
        {
            if (start >= 0) { start += Start; if (start >= End) return -1; }
            else start = Math.Max(Start, start + End);
            var index = String.IndexOf(ch, start, Math.Min(count, End - start));
            return index < 0 ? -1 : index - Start;
        }

        public int IndexOf(string str, int start, int count)
        {
            if (start >= 0) { start += Start; if (start >= End) return -1; }
            else start = Math.Max(Start, start + End);
            var index = String.IndexOf(str, start, Math.Min(count, End - start));
            return index < 0 ? -1 : index - Start;
        }

        public int LastIndexOf(char ch)
        {
            var index = String.LastIndexOf(ch, Start, Count);
            return index < 0 ? -1 : index - Start;
        }

        public int LastIndexOf(string str)
        {
            var index = String.LastIndexOf(str, Start, Count);
            return index < 0 ? -1 : index - Start;
        }

        public int LastIndexOf(char ch, int start)
        {
            if (start >= 0) { start += Start; if (start >= End) return -1; }
            else start = Math.Max(Start, start + End);
            var index = String.LastIndexOf(ch, start, End - start);
            return index < 0 ? -1 : index - Start;
        }

        public int LastIndexOf(string str, int start)
        {
            if (start >= 0) { start += Start; if (start >= End) return -1; }
            else start = Math.Max(Start, start + End);
            var index = String.LastIndexOf(str, start, End - start);
            return index < 0 ? -1 : index - Start;
        }

        public int LastIndexOf(char ch, int start, int count)
        {
            if (start >= 0) { start += Start; if (start >= End) return -1; }
            else start = Math.Max(Start, start + End);
            var index = String.LastIndexOf(ch, start, Math.Min(count, End - start));
            return index < 0 ? -1 : index - Start;
        }

        public int LastIndexOf(string str, int start, int count)
        {
            if (start >= 0) { start += Start; if (start >= End) return -1; }
            else start = Math.Max(Start, start + End);
            var index = String.LastIndexOf(str, start, Math.Min(count, End - start));
            return index < 0 ? -1 : index - Start;
        }

        /// <summary>
        /// Return the subtext in the range [start, end). For the start
        /// index, negative indices count from the end. For the end index,
        /// negative indices and the index 0 count from the end. The ranges
        /// are clamped to the input text so that no exceptions can occur.
        /// If the the start and end index are crossed, the empty text is
        /// returned. As an example the call t.Sub(-3, 0) returns a text
        /// containing the last 3 characters t or the complete text t, if
        /// its length is less than 3 characters.
        /// </summary>
        public Text Sub(int start, int end)
        {
            start = start < 0   ? Math.Max(Start, End + start)
                                : Math.Min(Start + start, End);
            end = end <= 0      ? Math.Max(Start, End + end)
                                : Math.Min(Start + end, End);
            return start < end ? new Text(start, end, String) : Empty;
        }

        public string SubString(int start, int count)
        {
            start += Start;
            return start < End
                    ? String.Substring(start, Math.Min(count, End - start))
                    : "";
        }

        public Text SubText(int start)
        {
            start += Start;
            return start < End ? new Text(start, End, String) : Empty;
        }

        public Text SubText(int start, int count)
        {
            start += Start;
            return start < End
                    ? new Text(start, Math.Min(start + count, End), String)
                    : Empty;
        }

        public bool StartsWith(string prefix)
        {
            int c = prefix.Length;
            if (c > Count) return false;
            for (int i = 0, p = Start; i < c; i++, p++)
                if (prefix[i] != String[p]) return false;
            return true;
        }

        public bool EndsWith(string postfix)
        {
            int c = postfix.Length;
            if (c > Count) return false;
            for (int i = 0, p = End - c; i < c; i++, p++)
                if (postfix[i] != String[p]) return false;
            return true;
        }

        public bool StartsWith(Text prefix)
        {
            int c = prefix.Count;
            if (c > Count) return false;
            for (int p = Start, e = Start + c, q = prefix.Start; p < e; p++, q++)
                if (prefix.String[q] != String[p]) return false;
            return true;
        }

        public bool EndsWith(Text postfix)
        {
            int c = postfix.Count;
            if (c > Count) return false;
            for (int p = End - c, e = End, q = postfix.Start; p < e; p++, q++)
                if (postfix.String[q] != String[p]) return false;
            return true;
        }

        /// <summary>
        /// Returns true if the supplied predicate is true for all characters
        /// of the text.
        /// </summary>
        public bool IsOnly(Func<char, bool> charPredicate)
        {
            for (int i = Start, e = End; i < e; i++)
            {
                if (charPredicate(String[i])) continue;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns the text without the characters at the start, for which
        /// the supplied predicate is true.
        /// </summary>
        public Text TrimmedAtStart(Func<char, bool> trimIfTrue)
        {
            for (int s = Start, e = End; s < e; s++)
            {
                if (trimIfTrue(String[s])) continue;
                return new Text(s, e, String);
            }
            return Empty;
        }

        /// <summary>
        /// Returns the text without the characters at the end, for which
        /// the supplied predicate is true.
        /// </summary>
        public Text TrimmedAtEnd(Func<char, bool> trimIfTrue)
        {
            for (int s = Start, e = End - 1; s <= e; e--)
            {
                if (trimIfTrue(String[e])) continue;
                return new Text(s, 1 + e, String);
            }
            return Empty;
        }


        /// <summary>
        /// Returns the text without the caracters at the start and at the
        /// end, for which the supplied predicate is true.
        /// </summary>
        /// <param name="trimIfTrue"></param>
        public Text Trimmed(Func<char, bool> trimIfTrue)
        {
            for (int s = Start, e = End; s < e; s++)
            {
                if (trimIfTrue(String[s])) continue;
                for (--e; s < e; e--) // String[s] already tested
                {
                    if (trimIfTrue(String[e])) continue;
                    return new Text(s, 1 + e, String);
                }
            }
            return Empty;
        }

        #endregion

        #region Overrides

        public override string ToString()
        {
            return String.Substring(Start, Count);
        }

        public override int GetHashCode()
        {
            int hc = Count;
            for (int i = Start, e = End; i < e; i++)
                hc = hc * 31 + String[i];
            return hc;
        }

        public override bool Equals(object obj)
        {
            return obj is Text ? this == (Text)obj : false;
        }

        #endregion

        #region Operators

        public static bool operator ==(Text t, Text t1)
        {
            if (t.Count != t1.Count) return false;
            string s = t.String, s1 = t1.String;
            if (s == s1 && t.Start == t1.Start) return true;
            for (int i = t.Start, e = t.End, i1 = t1.Start; i < e; i++, i1++)
                if (s[i] != s1[i1]) return false;
            return true;
        }

        public static bool operator !=(Text t, Text t1)
        {
            if (t.Count != t1.Count) return true;
            string s = t.String, s1 = t1.String;
            if (s == s1 && t.Start == t1.Start) return false;
            for (int i = t.Start, e = t.End, i1 = t1.Start; i < e; i++, i1++)
                if (s[i] != s1[i1]) return true;
            return false;
        }

        public static bool operator ==(Text t, string s1)
        {
            if (t.Count != s1.Length) return false;
            string s = t.String;
            if (s == s1 && t.Start == 0) return true;
            for (int i = t.Start, e = t.End, i1 = 0; i < e; i++, i1++)
                if (s[i] != s1[i1]) return false;
            return true;
        }

        public static bool operator !=(Text t, string s1)
        {
            if (t.Count != s1.Length) return true;
            string s = t.String;
            if (s == s1 && t.Start == 0) return false;
            for (int i = t.Start, e = t.End, i1 = 0; i < e; i++, i1++)
                if (s[i] != s1[i1]) return true;
            return false;
        }

        #endregion

        #region Various Operations

        public static Regex IdentifierRegex = new Regex(@"\b[A-Za-z_][0-9A-Za-z_]*\b");

        public Text ReplaceIdentifiers(
                Dictionary<string, string> changeMap)
        {
            return ReplaceParts(IdentifierRegex, changeMap);
        }

        public Text ReplaceParts(
                Regex partRegex,
                Dictionary<string, string> changeMap)
        {
            StringBuilder newText = new StringBuilder();
            var str = String;
            int pos = Start, end = End;
            Match match = partRegex.Match(str, pos, end - pos);
            while (match.Success)
            {
                Group group = match.Groups[1];
                newText.Append(str, pos, match.Index - pos);
                string changed = null;
                var unchanged = match.Value;
                if (changeMap.TryGetValue(unchanged, out changed))
                    newText.Append(changed);
                else
                    newText.Append(unchanged);
                pos = match.Index + match.Length;
                if (pos >= end) break;
                match = partRegex.Match(str, pos, end - pos);
            }
            if (pos > 0)
            {
                if (pos < end)
                    newText.Append(str, pos, end - pos);
                return new Text(newText.ToString());
            }
            return this;
        }

        /// <summary>
        /// Splits a nested structure of comma-separated square bracked
        /// delimited lists at a specified split level. Level 0 means that
        /// the split is performed outside the outermost square brackets.
        /// Level 1 means that the split is peformed inside the outermost
        /// square brackets.
        /// NOTE: The resulting parts are not trimmed. Use the Trim extension
        /// to trim all resulting strings.
        /// </summary>
        public IEnumerable<Text> NestedBracketSplit(int splitLevel)
        {
            int level = 0;
            int begin = Start;
            int end = End;
            for (int pos = Start; pos < end; pos++)
            {
                switch (String[pos])
                {
                    case '[':
                        ++level;
                        if (level == splitLevel) begin = pos + 1;
                        break;
                    case ']':
                        if (level == splitLevel)
                            yield return new Text(begin, pos, String);
                        --level;
                        break;
                    case ',':
                        if (level == splitLevel)
                        {
                            yield return new Text(begin, pos, String);
                            begin = pos + 1;
                        }
                        break;
                }
            }
            if (level == splitLevel && begin < end)
                yield return new Text(begin, end, String);
        }

        #endregion

        #region Parsing

        /// <summary>
        /// This structure holds info about where the line with the number
        /// (Index - 1) starts.
        /// </summary>
        public struct Line
        {
            public int Index;
            public int Start;

            public Line(int count, int start) { Index = count; Start = start; }
        }

        /// <summary>
        /// Given a known previous line, get the line at the current position.
        /// </summary>
        public Line GetLineOfPos(Line line, int pos)
        {
            int i = line.Start;
            while (i < pos)
            {
                if (String[Start + i] == '\n')
                    { ++i; ++line.Index; line.Start = i; }
                else
                    ++i;
            }
            return line;
        }

        /// <summary>
        /// Get the line of the current position without knowledge of any
        /// pervious line.
        /// </summary>
        public Line GetLineOfPos(int pos)
        {
            return GetLineOfPos(new Line(0, 0), pos);
        }

        /// <summary>
        /// Returns the first position after position start in the text, that
        /// does not contain a whitespace character, or the length of the text
        /// if it is all whitespace.
        /// </summary>
        public int SkipWhiteSpace(int start = 0)
        {
            return Skip(CharFun.IsWhiteSpace, start);
        }

        public int Skip(Func<char, bool> skipFun, int start = 0)
        {
            for (int i = Start + start, e = End; i < e; i++)
            {
                if (skipFun(String[i])) continue;
                return i - Start;
            }
            return End - Start;
        }

        public bool ParseBool()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfBoolAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public byte ParseByte()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfByteAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public sbyte ParseSByte()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfSByteAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public short ParseShort()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfShortAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public ushort ParseUShort()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfUShortAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public int ParseInt()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfIntAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public uint ParseUInt()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfUIntAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public long ParseLong()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfLongAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public ulong ParseULong()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfULongAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public float ParseFloat()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfFloatAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        public double ParseDouble()
        {
            int i = SkipWhiteSpace(), c = Count;
            if (i == c) throw new ArgumentException();
            var pv = ParsedValueOfDoubleAt(i);
            if (pv.Error != ParseError.None) throw new ArgumentException();
            i += pv.Length;
            if (i < c && SkipWhiteSpace(i) < c) throw new ArgumentException();
            return pv.Value;
        }

        private static readonly Regex s_boolRegex
                = new Regex(@"(?<1>false|f)|(?<2>true|t)|(?<3>.|\r)",
                            RegexOptions.Singleline | RegexOptions.IgnoreCase
                            | RegexOptions.ExplicitCapture | RegexOptions.Compiled);

        public ParsedValue<bool> ParsedValueOfBoolAt(int start)
        {
            start += Start;
            if (start >= End) return new ParsedValue<bool>(ParseError.EndOfText, 0);
            var match = s_boolRegex.Match(String, start, 5);
            if (!match.Success) return new ParsedValue<bool>(ParseError.OutOfRange, 0);
            if (match.Groups[1].Success) return new ParsedValue<bool>(false, match.Length);
            if (match.Groups[2].Success) return new ParsedValue<bool>(true, match.Length);
            return new ParsedValue<bool>(ParseError.OutOfRange, 0);
        }

        public ParsedValue<byte> ParsedValueOfByteAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            int ch = 0;
            if (pos >= end) return new ParsedValue<byte>(ParseError.EndOfText, 0);
            if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
            {
                ++pos;
                byte v = (byte)ch; int c = 1;
                while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    byte v1 = (byte)(v * 10 + ch);
                    if (++c > 2 && v != v1 / 10)
                        return new ParsedValue<byte>(ParseError.OutOfRange, pos - start);
                    v = v1; ++pos;
                }
                return new ParsedValue<byte>(v, pos - start);
            }
            return new ParsedValue<byte>(ParseError.IllegalCharacter, pos - start);
        }

        public ParsedValue<sbyte> ParsedValueOfSByteAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            int ch = 0;
            if (pos >= end) return new ParsedValue<sbyte>(ParseError.EndOfText, 0);
            if (String[pos] == '-')
            {
                if (++pos == end) return new ParsedValue<sbyte>(ParseError.EndOfText, 1);
                if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ++pos;
                    sbyte v = (sbyte)(-ch); int c = 1;
                    while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                    {
                        sbyte v1 = (sbyte)(v * 10 - ch);
                        if (++c > 2 && v != v1 / 10L)
                            return new ParsedValue<sbyte>(ParseError.OutOfRange, pos - start);
                        v = v1; ++pos;
                    }
                    return new ParsedValue<sbyte>(v, pos - start);
                }
            }
            else
            {
                if (String[pos] == '+')
                {
                    if (++pos == end) return new ParsedValue<sbyte>(ParseError.EndOfText, 1);
                }
                if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ++pos;
                    sbyte v = (sbyte)ch; int c = 1;
                    while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                    {
                        sbyte v1 = (sbyte)(v * 10 + ch);
                        if (++c > 2 && v != v1 / 10L)
                            return new ParsedValue<sbyte>(ParseError.OutOfRange, pos - start);
                        v = v1; ++pos;
                    }
                    return new ParsedValue<sbyte>(v, pos - start);
                }
            }
            return new ParsedValue<sbyte>(ParseError.IllegalCharacter, pos - start);
        }

        public ParsedValue<short> ParsedValueOfShortAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            int ch = 0;
            if (pos >= end) return new ParsedValue<short>(ParseError.EndOfText, 0);
            if (String[pos] == '-')
            {
                if (++pos == end) return new ParsedValue<short>(ParseError.EndOfText, 1);
                if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ++pos;
                    short v = (short)(-ch); int c = 1;
                    while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                    {
                        short v1 = (short)(v * 10 - ch);
                        if (++c > 4 && v != v1 / 10L)
                            return new ParsedValue<short>(ParseError.OutOfRange, pos - start);
                        v = v1; ++pos;
                    }
                    return new ParsedValue<short>(v, pos - start);
                }
            }
            else
            {
                if (String[pos] == '+')
                {
                    if (++pos == end) return new ParsedValue<short>(ParseError.EndOfText, 1);
                }
                if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ++pos;
                    short v = (short)ch; int c = 1;
                    while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                    {
                        short v1 = (short)(v * 10 + ch);
                        if (++c > 4 && v != v1 / 10L)
                            return new ParsedValue<short>(ParseError.OutOfRange, pos - start);
                        v = v1; ++pos;
                    }
                    return new ParsedValue<short>(v, pos - start);
                }
            }
            return new ParsedValue<short>(ParseError.IllegalCharacter, pos - start);
        }

        public ParsedValue<ushort> ParsedValueOfUShortAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            int ch = 0;
            if (pos >= end) return new ParsedValue<ushort>(ParseError.EndOfText, 0);
            if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
            {
                ++pos;
                ushort v = (ushort)ch; int c = 1;
                while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ushort v1 = (ushort)(v * 10 + ch);
                    if (++c > 4 && v != v1 / 10L)
                        return new ParsedValue<ushort>(ParseError.OutOfRange, pos - start);
                    v = v1; ++pos;
                }
                return new ParsedValue<ushort>(v, pos - start);
            }
            return new ParsedValue<ushort>(ParseError.IllegalCharacter, pos - start);
        }

        public ParsedValue<int> ParsedValueOfIntAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            int ch = 0;
            if (pos >= end) return new ParsedValue<int>(ParseError.EndOfText, 0);
            if (String[pos] == '-')
            {
                if (++pos == end) return new ParsedValue<int>(ParseError.EndOfText, 1);
                if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ++pos;
                    int v = -ch; int c = 1;
                    while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                    {
                        int v1 = v * 10 - ch;
                        if (++c > 9 && v != v1 / 10L)
                            return new ParsedValue<int>(ParseError.OutOfRange, pos - start);
                        v = v1;  ++pos;
                    }
                    return new ParsedValue<int>(v, pos - start);
                }
            }
            else
            {
                if (String[pos] == '+')
                {
                    if (++pos == end) return new ParsedValue<int>(ParseError.EndOfText, 1);
                }
                if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ++pos;
                    int v = ch; int c = 1;
                    while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                    {
                        int v1 = v * 10 + ch;
                        if (++c > 9 && v != v1 / 10L)
                            return new ParsedValue<int>(ParseError.OutOfRange, pos - start);
                        v = v1;  ++pos;
                    }
                    return new ParsedValue<int>(v, pos - start);
                }
            }
            return new ParsedValue<int>(ParseError.IllegalCharacter, pos - start);
        }

        public ParsedValue<uint> ParsedValueOfUIntAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            int ch = 0;
            if (pos >= end) return new ParsedValue<uint>(ParseError.EndOfText, 0);
            if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
            {
                ++pos;
                uint v = (uint)ch; int c = 1;
                while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    uint v1 = v * 10 + (uint)ch;
                    if (++c > 9 && v != v1 / 10L)
                        return new ParsedValue<uint>(ParseError.OutOfRange, pos - start);
                    v = v1; ++pos;
                }
                return new ParsedValue<uint>(v, pos - start);
            }
            return new ParsedValue<uint>(ParseError.IllegalCharacter, pos - start);
        }

        public ParsedValue<long> ParsedValueOfLongAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            int ch = 0;
            if (pos >= end) return new ParsedValue<long>(ParseError.EndOfText, 0);
            if (String[pos] == '-')
            {
                if (++pos == end) return new ParsedValue<long>(ParseError.EndOfText, 1);
                if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ++pos;
                    long v = -ch; int c = 1;
                    while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                    {
                        long v1 = v * 10 - ch;
                        if (++c > 18 && v != v1 / 10L)
                            return new ParsedValue<long>(ParseError.OutOfRange, pos - start);
                        v = v1; ++pos;
                    }
                    return new ParsedValue<long>(v, pos - start);
                }
            }
            else
            {
                if (String[pos] == '+')
                {
                    if (++pos == end) return new ParsedValue<long>(ParseError.EndOfText, 1);
                }
                if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ++pos;
                    long v = ch; int c = 1;
                    while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                    {
                        long v1 = v * 10 + ch;
                        if (++c > 18 && v != v1 / 10L)
                            return new ParsedValue<long>(ParseError.OutOfRange, pos - start);
                        v = v1; ++pos;
                    }
                    return new ParsedValue<long>(v, pos - start);
                }
            }
            return new ParsedValue<long>(ParseError.IllegalCharacter, pos - start);
        }

        public ParsedValue<ulong> ParsedValueOfULongAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            int ch = 0;
            if (pos >= end) return new ParsedValue<ulong>(ParseError.EndOfText, 0);
            if ((ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
            {
                ++pos;
                ulong v = (ulong)ch;
                int c = 1;
                while (pos < end && (ch = (int)(String[pos] - '0')) >= 0 && ch < 10)
                {
                    ulong v1 = v * 10 + (ulong)ch;
                    if (++c > 18 && v != v1 / 10L)
                        return new ParsedValue<ulong>(ParseError.OutOfRange, pos - start);
                    v = v1; ++pos;
                }
                return new ParsedValue<ulong>(v, pos - start);
            }
            return new ParsedValue<ulong>(ParseError.IllegalCharacter, pos - start);
        }

        public ParsedValue<float> ParsedValueOfFloatAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            if (pos >= end) return new ParsedValue<float>(ParseError.EndOfText, 0);
            char ch;
            if ((ch = String[pos]) == '-' || ch == '+')
            {
                pos++;
                if (pos == end) return new ParsedValue<float>(ParseError.EndOfText, pos - start);
            }
            if ((ch = String[pos]) < '0' || ch > '9')
                return new ParsedValue<float>(ParseError.IllegalCharacter, pos - start);
            while (pos < end && (ch = String[pos]) >= '0' && ch <= '9') pos++;
            if (pos < end && ch == '.')
            {
                ++pos;
                while (pos < end && (ch = String[pos]) >= '0' && ch <= '9') pos++;
            }
            if (pos < end && (ch == 'e' || ch == 'E'))
            {
                ++pos;
                if (pos == end) return new ParsedValue<float>(ParseError.EndOfText, pos - start);
                if ((ch = String[pos]) == '-' || ch == '+')
                {
                    pos++;
                    if (pos == end) return new ParsedValue<float>(ParseError.EndOfText, pos - start);
                }
                if ((ch = String[pos]) < '0' || ch > '9')
                    return new ParsedValue<float>(ParseError.IllegalCharacter, pos - start);
                ++pos;
                while (pos < end && (ch = String[pos]) >= '0' && ch <= '9') pos++;
            }
            int len = pos - start;
            try
            {
                return new ParsedValue<float>(float.Parse(String.Substring(start, len), Localization.FormatEnUS), len);
            }
            catch
            {
                return new ParsedValue<float>(ParseError.OutOfRange, len);
            }
        }

        public ParsedValue<double> ParsedValueOfDoubleAt(int start)
        {
            start += Start;
            int pos = start;
            int end = End;
            if (pos >= end) return new ParsedValue<double>(ParseError.EndOfText, 0);
            char ch;
            if ((ch = String[pos]) == '-' || ch == '+')
            {
                pos++;
                if (pos == end) return new ParsedValue<double>(ParseError.EndOfText, pos - start);
            }
            if ((ch = String[pos]) < '0' || ch > '9')
                return new ParsedValue<double>(ParseError.IllegalCharacter, pos - start);
            while (pos < end && (ch = String[pos]) >= '0' && ch <= '9') pos++;
            if (pos < end && ch == '.')
            {
                ++pos;
                while (pos < end && (ch = String[pos]) >= '0' && ch <= '9') pos++;
            }
            if (pos < end && (ch == 'e' || ch == 'E'))
            {
                ++pos;
                if (pos == end) return new ParsedValue<double>(ParseError.EndOfText, pos - start);
                if ((ch = String[pos]) == '-' || ch == '+')
                {
                    pos++;
                    if (pos == end) return new ParsedValue<double>(ParseError.EndOfText, pos - start);
                }
                if ((ch = String[pos]) < '0' || ch > '9')
                    return new ParsedValue<double>(ParseError.IllegalCharacter, pos - start);
                ++pos;
                while (pos < end && (ch = String[pos]) >= '0' && ch <= '9') pos++;
            }
            int len = pos - start;
            try
            {
                return new ParsedValue<double>(double.Parse(String.Substring(start, len), Localization.FormatEnUS), len);
            }
            catch
            {
                return new ParsedValue<double>(ParseError.OutOfRange, len);
            }
        }

        #endregion

        #region IEquatable<Text> Members

        public bool Equals(Text other)
        {
            return this == other;
        }

        #endregion
    }

    public struct Rx
    {
        public readonly string Pattern;
        public readonly Regex Regex;

        /// <summary>
        /// A second regex is built if the match has to be anchored at the
        /// start of the search (sub-)string (the caret only works for non-
        /// sub strings). This is done by adding an always matching second
        /// group of one character. Thus if the second group succeeds, the
        /// regex did not match anchored at the start of the search string.
        /// </summary>
        public readonly Regex AnchoredRegex;

        #region Constructor

        public Rx(string pattern)
        {
            Pattern = pattern;
            Regex = new Regex(pattern, RegexOptions.Singleline
                                       | RegexOptions.ExplicitCapture
                                       | RegexOptions.Compiled);

            AnchoredRegex = new Regex("(?<1>" + pattern + ")|(?<2>.|\r)",
                                      RegexOptions.Singleline
                                      | RegexOptions.ExplicitCapture
                                      | RegexOptions.Compiled);
        }

        public Rx(string pattern, RegexOptions options)
        {
            Pattern = pattern;
            Regex = new Regex(pattern, options | RegexOptions.Singleline
                                       | RegexOptions.ExplicitCapture
                                       | RegexOptions.Compiled);

            AnchoredRegex = new Regex("(?<1>" + pattern + ")|(?<2>.|\r)",
                                      options | RegexOptions.Singleline
                                      | RegexOptions.ExplicitCapture
                                      | RegexOptions.Compiled);
        }

        #endregion
    }

    public enum ParseError
    {
        None = 0,
        EndOfText,
        IllegalCharacter,
        OutOfRange,
    }

    /// <summary>
    /// A parsed value contains the actual value, the number of characters
    /// that have been parsed in Length, and an error value of the parse
    /// operation.
    /// </summary>
    public struct ParsedValue<T>
    {
        public readonly ParseError Error;
        public readonly T Value;
        public readonly int Length;

        #region Constructors

        public ParsedValue(T value, int count)
        {
            Value = value; Error = ParseError.None; Length = count;
        }

        public ParsedValue(ParseError error, int count)
        {
            Value = default(T); Error = error; Length = count;
        }

        #endregion

        #region Properties

        public bool IsValid { get { return Error == ParseError.None; } }
        public bool IsInValid { get { return Error != ParseError.None; } }

        #endregion
    }

    public static class Text<T>
    {
        public static readonly Func<Text, T> Parse;
        public static readonly Func<Text, int, ParsedValue<T>> ParsedValueAt;

        #region Constructor

        static Text()
        {
            Text<byte>.Parse = t => t.ParseByte();
            Text<sbyte>.Parse = t => t.ParseSByte();
            Text<short>.Parse = t => t.ParseShort();
            Text<ushort>.Parse = t => t.ParseUShort();
            Text<int>.Parse = t => t.ParseInt();
            Text<uint>.Parse = t => t.ParseUInt();
            Text<long>.Parse = t => t.ParseLong();
            Text<ulong>.Parse = t => t.ParseULong();
            Text<float>.Parse = t => t.ParseFloat();
            Text<double>.Parse = t => t.ParseDouble();

            Text<byte>.ParsedValueAt = (t, i) => t.ParsedValueOfByteAt(i);
            Text<sbyte>.ParsedValueAt = (t, i) => t.ParsedValueOfSByteAt(i);
            Text<short>.ParsedValueAt = (t, i) => t.ParsedValueOfShortAt(i);
            Text<ushort>.ParsedValueAt = (t, i) => t.ParsedValueOfUShortAt(i);
            Text<int>.ParsedValueAt = (t, i) => t.ParsedValueOfIntAt(i);
            Text<uint>.ParsedValueAt = (t, i) => t.ParsedValueOfUIntAt(i);
            Text<long>.ParsedValueAt = (t, i) => t.ParsedValueOfLongAt(i);
            Text<ulong>.ParsedValueAt = (t, i) => t.ParsedValueOfULongAt(i);
            Text<float>.ParsedValueAt = (t, i) => t.ParsedValueOfFloatAt(i);
            Text<double>.ParsedValueAt = (t, i) => t.ParsedValueOfDoubleAt(i);
        }

        #endregion
    }

    public static class TextExtensions
    {
        #region String Extension

        public static Text ToText(this String str)
        {
            return new Text(str);
        }

        public static string ReplaceIdentifiers(this string str, Dictionary<string, string> changeMap)
        {
            return str.ToText().ReplaceIdentifiers(changeMap).ToString();
        }

        #endregion

        #region StringBuilder Extension

        public static StringBuilder Append(this StringBuilder builder, Text text)
        {
            return builder.Append(text.String, text.Start, text.Count);
        }

        public static Text ToText(this StringBuilder builder)
        {
            return new Text(builder.ToString());
        }

        #endregion

        #region List<Text> Extensions

        public static List<string> ToListOfString(this List<Text> textList)
        {
            return textList.Copy(t => t.ToString());
        }

        public static string[] ToStringArray(this List<Text> textList)
        {
            return textList.ToArray(t => t.ToString());
        }

        public static string JoinToString(this List<Text> textList, string delimiter)
        {
            return textList.Copy(t => t.ToString()).Join(delimiter);
        }

        #endregion

        #region TextArray Extensions

        public static string[] ToStringArray(this Text[] textArray)
        {
            return textArray.Copy(t => t.ToString());
        }

        public static List<string> ToListOfString(this Text[] textArray)
        {
            return textArray.ToList(t => t.ToString());
        }


        public static int NestedBracketSplitCount(this Text text, int splitLevel)
        {
            int count = 0;
            int level = 0;
            for (int i = text.Start, e = text.End; i < e; i++)
            {
                switch (text.String[i])
                {
                    case '[':  ++level; break;
                    case ']': --level; break;
                    case ',':
                        if (level == splitLevel) ++count;
                        break;
                }
            }
            if (level == splitLevel) ++count;
            return count;
        }

        #endregion

        #region Text Extensions

        public static TArray NestedBracketSplit<T, TArray>(
                this Text text, int splitLevel, Func<Text, T> parse,
                Func<TArray> creator, Action<TArray, long, T> setter)
        {
            int level = 0;
            int start = text.Start;
            int end = text.End;
            long ai = 0;
            var array = creator();
            for (int i = start; i < end; i++)
            {
                switch (text.String[i])
                {
                    case '[':
                        ++level;
                        if (level == splitLevel) start = i + 1;
                        break;
                    case ']':
                        if (level == splitLevel)
                            setter(array, ai++, parse(new Text(start, i, text.String)));
                        --level;
                        break;
                    case ',':
                        if (level == splitLevel)
                        {
                            setter(array, ai++, parse(new Text(start, i, text.String)));
                            start = i + 1;
                        }
                        break;
                }
            }
            if (level == splitLevel && start < end)
                setter(array, ai, parse(new Text(start, end, text.String)));
            return array;
        }

        public static TStruct NestedBracketSplit<T, TStruct>(
                this Text text, int splitLevel, Func<Text, T> parse,
                ActionRefValVal<TStruct, int, T> setter)
        {
            int level = 0;
            int start = text.Start;
            int end = text.End;
            int ai = 0;
            var str = default(TStruct);
            for (int i = start; i < end; i++)
            {
                switch (text.String[i])
                {
                    case '[':
                        ++level;
                        if (level == splitLevel) start = i + 1;
                        break;
                    case ']':
                        if (level == splitLevel)
                            setter(ref str, ai++, parse(new Text(start, i, text.String)));
                        --level;
                        break;
                    case ',':
                        if (level == splitLevel)
                        {
                            setter(ref str, ai++, parse(new Text(start, i, text.String)));
                            start = i + 1;
                        }
                        break;
                }
            }
            if (level == splitLevel && start < end)
                setter(ref str, ai, parse(new Text(start, end, text.String)));
            return str;
        }

        public static T[] NestedBracketSplit<T>(
                this Text text, int splitLevel, Func<Text, T> parse, Func<T[]> creator)
        {
            int level = 0;
            int start = text.Start;
            int end = text.End;
            int ai = 0;
            var array = creator();
            for (int i = start; i < end; i++)
            {
                switch (text.String[i])
                {
                    case '[':
                        ++level;
                        if (level == splitLevel) start = i + 1;
                        break;
                    case ']':
                        if (level == splitLevel)
                            array[ai++] = parse(new Text(start, i, text.String));
                        --level;
                        break;
                    case ',':
                        if (level == splitLevel)
                        {
                            array[ai++] = parse(new Text(start, i, text.String));
                            start = i + 1;
                        }
                        break;
                }
            }
            if (level == splitLevel && start < end)
                array[ai] = parse(new Text(start, end, text.String));
            return array;
        }

        public static IEnumerable<Text> Split(this Text text, char c)
        {
            int start = text.Start;
            int end = text.Start;

            while (end < text.End)
            {
                if (text.String[end++] == c)
                {
                    yield return new Text(start, end - 1, text.String);
                    start = end;
                }
            }

            yield return new Text(start, end, text.String);
        }
   
        #endregion
    }

}
