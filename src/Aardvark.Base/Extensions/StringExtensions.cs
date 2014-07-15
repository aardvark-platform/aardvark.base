using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Aardvark.Base
{
    public static class Localization
    {
        public static readonly IFormatProvider FormatEnUS =
            new CultureInfo("en-US", false);
    }

    public static class CharFun
    {
        public static bool IsWhiteSpace(this char ch)
        {
            return ch == ' ' || ch == '\t' || ch == '\r' || ch == '\n';
        }

        public static bool IsSpaceOrTab(this char ch)
        {
            return ch == ' ' || ch == '\t';
        }
    }

    public static class StringFun
    {
        #region To<Type>

        public static Symbol ToSymbol(this string str)
        {
            return Symbol.Create(str);
        }

        public static bool ToBool(this string self)
        {
            var s = self.Trim().ToLower();
            if (s == "true" || s == "t") return true;
            if (s == "false" || s == "f") return false;
            double r;
            // Double.TryParse() sets r to 0 if the conversion fails, 
            // which means we don't really need to check the return value.
            Double.TryParse(s, out r);
            return r != 0.0;
        }

        public static byte ToByte(this string self)
        {
            return byte.Parse(self, Localization.FormatEnUS);
        }

        public static sbyte ToSByte(this string self)
        {
            return sbyte.Parse(self, Localization.FormatEnUS);
        }

        public static short ToShort(this string self)
        {
            return short.Parse(self, Localization.FormatEnUS);
        }

        public static ushort ToUShort(this string self)
        {
            return ushort.Parse(self, Localization.FormatEnUS);
        }

        public static int ToInt(this string self)
        {
            return int.Parse(self, Localization.FormatEnUS);
        }

        public static uint ToUInt(this string self)
        {
            return uint.Parse(self, Localization.FormatEnUS);
        }

        public static long ToLong(this string self)
        {
            return long.Parse(self, Localization.FormatEnUS);
        }

        public static ulong ToULong(this string self)
        {
            return ulong.Parse(self, Localization.FormatEnUS);
        }

        public static float ToFloat(this string self)
        {
            return float.Parse(self, Localization.FormatEnUS);
        }

        public static double ToDouble(this string self)
        {
            return double.Parse(self, Localization.FormatEnUS);
        }

        public static DateTime ToDateTime(this string self)
        {
            return DateTime.Parse(self, Localization.FormatEnUS);
        }

        #endregion

        #region Manipulations

        public static string Capitalized(this string self)
        {
            if (self == null || self == "") return self;
            return self.Substring(0, 1).ToUpperInvariant() + self.Substring(1);
        }

        public static string[] ToLower(this string[] self)
        {
            return self.Copy(s => s.ToLower());
        }

        public static string[] ToUpper(this string[] self)
        {
            return self.Copy(s => s.ToUpper());
        }

        public static IEnumerable<string> ToLower(this IEnumerable<string> self)
        {
            foreach (var s in self)
                yield return s.ToLower();
        }

        public static IEnumerable<string> ToUpper(this IEnumerable<string> self)
        {
            foreach (var s in self)
                yield return s.ToUpper();
        }

        #endregion

        #region Plural

        /// <summary>
        /// Returns the english plural of the supplied word (i.e. the word
        /// with "s" appended) if the supplied integer is not equal to one.
        /// Otherwise the word is returned unchanged. 
        /// </summary>
        public static string Plural(this string word, int s)
        {
            return word.Plural((long)s);
        }

        static readonly Dictionary<string, string> s_irregularPlural
            = new Dictionary<string, string>
        {
            { "aircraft", "aircraft" },
            { "axis", "axes" },
            { "bison", "bison" },
            { "child", "children" },
            { "deer", "deer" },
            { "fish", "fish" },
            { "foot", "feet" },
            { "goose", "geese" },
            { "louse", "lice" },
            { "man", "men" },
            { "moose", "moose" },
            { "mouse", "mice" },
            { "ox", "oxen" },
            { "pike", "pike" },
            { "salmon", "salmon" },
            { "sheep", "sheep" },
            { "tooth", "teeth" },
            { "trout", "trout" },
            { "woman", "women" },
        };

        static Dictionary<string, string>[] s_pluralEndings
            = new Dictionary<string, string>[]
        {
            null,
            new Dictionary<string, string> { { "y", "ies" } },
            new Dictionary<string, string>
            {
                { "ch" , "ches" }, { "ex", "ices" },
                { "sh" , "shes" }, { "ss", "sses" },
            },
        };

        /// <summary>
        /// Returns the english plural of the supplied word (i.e. the word
        /// with "s" appended) if the supplied integer is not equal to one.
        /// Otherwise the word is returned unchanged.
        /// </summary>
        public static string Plural(this string word, long s)
        {
            if (s == 1) return word;
            string irregular;
            if (s_irregularPlural.TryGetValue(word, out irregular))
                return irregular;
            else
            {
                for (int i = s_pluralEndings.Length-1; i > 0; i--)
                {
                    string ending = word.Substring(word.Length - i);
                    string plural;
                    if (s_pluralEndings[i].TryGetValue(ending, out plural))
                        return word.Substring(0, word.Length - i) + plural;
                }
                return word + "s";
            }
        }

        #endregion

        #region Sub strings

        /// <summary>
        /// Returns leftmost n characters of this string.
        /// </summary>
        public static string Left(this string self, int n)
        {
            if (self.Length <= n) return self;
            return self.Substring(0, n);
        }

        /// <summary>
        /// Returns rightmost n characters of this string.
        /// </summary>
        public static string Right(this string self, int n)
        {
            int length = self.Length;
            if (length <= n) return self;
            return self.Substring(length - n);
        }

        /// <summary>
        /// Return the substring for the range [start, end). For the start
        /// index, negative indices count from the end. For the end index,
        /// negative and the indices and the index 0 count from the end.
        /// The ranges are clamped to the input string so that no exceptions
        /// can occur. If the the start and end index are crossed, the null
        /// string is returned.  As an example the call s.Sub(-3, 0) returns
        /// a string containing the last 3 characters s or the complete
        /// string s if its length is less than 3 characters.
        /// </summary>
        public static string Sub(this string self, int start, int end)
        {
            int len = self.Length;
            start = start < 0 ? Math.Max(0, len + start) : Math.Min(start, len);
            end = end <= 0 ? Math.Max(0, len + end) : Math.Min(end, len);
            return start < end ? self.Substring(start, end - start) : null;
        }

        public static string Join(this IEnumerable<string> strings)
        {
            var result = new StringBuilder();
            foreach (var s in strings) result.Append(s);
            return result.ToString();
        }

        public static string Join(this IEnumerable<string> strings, string delimiter)
        {
            var result = new StringBuilder();
            bool notFirst = false;
            foreach (var s in strings)
            {
                if (notFirst) result.Append(delimiter); else notFirst = true;
                result.Append(s);
            }
            return result.ToString();
        }

        private static readonly char[] s_whiteSpace
                = new char[] { ' ', '\t', '\n', '\r' };

        public static string[] SplitOnWhitespace(this string s)
        {
            return s.Split(s_whiteSpace, StringSplitOptions.RemoveEmptyEntries);
        }

        #endregion

        #region Square Brackets/Comma Split

        /// <summary>
        /// Splits a nested structure of comma-separated square bracket
        /// delimited lists at level 1, i.e. inside the top most bracket.
        /// E.g. "[[a, b], foo, [c, d]]" returns "[a, b]", "foo" and "[c, d]".
        /// </summary>
        public static IEnumerable<string> NestedBracketSplitLevelOne(this string text)
        {
            int level = 0;
            int begin = 0;
            int len = text.Length;
            for (int pos = 0; pos < len; pos++)
            {
                switch (text[pos])
                {
                    case '[':
                        if (++level == 1) begin = pos + 1;
                        break;
                    case ']':
                        if (level-- == 1)
                            yield return text.Substring(begin, pos - begin).Trim();
                        break;
                    case ',':
                        if (level == 1)
                        {
                            yield return text.Substring(begin, pos - begin).Trim();
                            begin = pos + 1;
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// Splits a nested structure of comma-separated square bracked
        /// delimited lists at a specified split level. Level 0 means that
        /// the split is performed outside the outermost square brackets.
        /// Level 1 means that the split is peformed inside the outermost
        /// square brackets.
        /// NOTE: The resulting parts are not trimmed. Use the
        /// <see cref="Trim"/> extension to trim all resulting strings.
        /// </summary>
        public static IEnumerable<string> NestedBracketSplit(this string text, int splitLevel)
        {
            int level = 0;
            int begin = 0;
            int len = text.Length;
            for (int pos = 0; pos < len; pos++)
            {
                switch (text[pos])
                {
                    case '[':
                        ++level;
                        if (level == splitLevel) begin = pos + 1;
                        break;
                    case ']':
                        if (level == splitLevel)
                            yield return text.Substring(begin, pos - begin);
                        --level;
                        break;
                    case ',':
                        if (level == splitLevel)
                        {
                            yield return text.Substring(begin, pos - begin);
                            begin = pos + 1;
                        }
                        break;
                }
            }
            if (level == splitLevel && len > begin)
                yield return text.Substring(begin, len - begin);
        }

        #endregion

        #region Guid

        public static Guid ToGuid(this string self)
        {
            if (string.IsNullOrEmpty(self)) return Guid.Empty;
            return new Guid(MD5.Create().ComputeHash(Encoding.Unicode.GetBytes(self)));
            //return (Guid)Hash.From<char>(self.ToCharArray());
        }

        public static Guid ToGuid(this IEnumerable<string> self)
        {
            var sb = new StringBuilder();
            foreach (var x in self) sb.Append(x);
            return sb.ToString().ToGuid();
        }

        public static Guid ToGuid(this IEnumerable<Guid> self)
        {
            var sb = new StringBuilder();
            foreach (var x in self) sb.Append(x.ToString());
            return sb.ToString().ToGuid();
        }

        #endregion

        #region Properties

        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }

        #endregion

        #region Formatting
        /// <summary>
        /// Same as String.Format, but uses the InvariantCulture formatter 
        /// (The one we usually want. You know, . as decimal point,...)
        /// </summary>
        public static string FormatInvariant(this string format, params object[] args)
        {
            return string.Format(System.Globalization.CultureInfo.InvariantCulture, format, args);
        }
        
        #endregion
    }

}
