using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.TextUtilities
{
    [TestFixture]
    public static class TextTests
    {
        private static TextParser CreateParser(Text text)
        {
            return new TextParser
            {
                Text = text,
                Pos = text.Start,
                LastEnd = text.Start,
                Line = new Text.Line(0, text.Start),
                LastWhiteSpace = -1,
                LastCase = -1,
            };
        }

        [Test]
        public static void ToStringHandlesTextEmpty()
        {
            Assert.AreEqual(string.Empty, Text.Empty.ToString());
        }

        [Test]
        public static void ToStringHandlesEmptySlice()
        {
            var text = new Text("abc", 3, 0);

            Assert.AreEqual(string.Empty, text.ToString());
        }

        [Test]
        public static void IndexOfFindsValuesInFullText()
        {
            var text = new Text("abcabc");

            Assert.AreEqual(0, text.IndexOf('a'));
            Assert.AreEqual(1, text.IndexOf("bc"));
        }

        [Test]
        public static void IndexOfFindsValuesInSlicedText()
        {
            var text = new Text("xxabcabcxx", 2, 6);

            Assert.AreEqual(0, text.IndexOf('a'));
            Assert.AreEqual(1, text.IndexOf("bc"));
            Assert.AreEqual(-1, text.IndexOf('x'));
            Assert.AreEqual(-1, text.IndexOf("xx"));
        }

        [Test]
        public static void IndexOfStartOverloadsClampToTextSlice()
        {
            var text = new Text("xxabcabcxx", 2, 6);

            Assert.AreEqual(3, text.IndexOf('a', 2));
            Assert.AreEqual(-1, text.IndexOf('a', -1));
            Assert.AreEqual(-1, text.IndexOf('a', 100));
            Assert.AreEqual(0, text.IndexOf('a', -100));

            Assert.AreEqual(3, text.IndexOf("ab", 2));
            Assert.AreEqual(-1, text.IndexOf("ab", -1));
            Assert.AreEqual(-1, text.IndexOf("ab", 100));
            Assert.AreEqual(0, text.IndexOf("ab", -100));
        }

        [Test]
        public static void IndexOfStartCountOverloadsClampToTextSlice()
        {
            var text = new Text("xxabcabcxx", 2, 6);

            Assert.AreEqual(0, text.IndexOf('a', 0, 2));
            Assert.AreEqual(-1, text.IndexOf('a', 2, 1));
            Assert.AreEqual(3, text.IndexOf('a', 2, 2));
            Assert.AreEqual(3, text.IndexOf('a', 2, 100));
            Assert.AreEqual(-1, text.IndexOf('a', 0, 0));
            Assert.AreEqual(-1, text.IndexOf('a', 0, -1));

            Assert.AreEqual(0, text.IndexOf("ab", 0, 2));
            Assert.AreEqual(-1, text.IndexOf("ab", 2, 2));
            Assert.AreEqual(3, text.IndexOf("ab", 2, 3));
            Assert.AreEqual(3, text.IndexOf("ab", 2, 100));
            Assert.AreEqual(-1, text.IndexOf("ab", 0, 0));
            Assert.AreEqual(-1, text.IndexOf("ab", 0, -1));
        }

        [Test]
        public static void IndexOfReturnsMinusOneForMissingValues()
        {
            var text = new Text("abcabc");

            Assert.AreEqual(-1, text.IndexOf('z'));
            Assert.AreEqual(-1, text.IndexOf("zz"));
            Assert.AreEqual(-1, text.IndexOf('a', 4));
            Assert.AreEqual(-1, text.IndexOf("bc", 0, 1));
        }

        [Test]
        public static void IndexOfHandlesEmptyText()
        {
            var text = Text.Empty;

            Assert.AreEqual(-1, text.IndexOf('a'));
            Assert.AreEqual(-1, text.IndexOf("a"));
            Assert.AreEqual(-1, text.IndexOf('a', 0));
            Assert.AreEqual(-1, text.IndexOf("a", 0));
            Assert.AreEqual(-1, text.IndexOf('a', 0, 1));
            Assert.AreEqual(-1, text.IndexOf("a", 0, 1));
        }

        [Test]
        public static void LastIndexOfFindsValuesInFullText()
        {
            var text = new Text("abcabc");

            Assert.AreEqual(3, text.LastIndexOf('a'));
            Assert.AreEqual(4, text.LastIndexOf("bc"));
        }

        [Test]
        public static void LastIndexOfFindsValuesInSlicedText()
        {
            var text = new Text("xxabcabcxx", 2, 6);

            Assert.AreEqual(3, text.LastIndexOf('a'));
            Assert.AreEqual(4, text.LastIndexOf("bc"));
            Assert.AreEqual(-1, text.LastIndexOf('x'));
            Assert.AreEqual(-1, text.LastIndexOf("xx"));
        }

        [Test]
        public static void LastIndexOfStartOverloadsClampToTextSlice()
        {
            var text = new Text("xxabcabcxx", 2, 6);

            Assert.AreEqual(0, text.LastIndexOf('a', 2));
            Assert.AreEqual(3, text.LastIndexOf('a', -1));
            Assert.AreEqual(3, text.LastIndexOf('a', 100));
            Assert.AreEqual(0, text.LastIndexOf('a', -100));

            Assert.AreEqual(0, text.LastIndexOf("ab", 2));
            Assert.AreEqual(3, text.LastIndexOf("ab", -1));
            Assert.AreEqual(3, text.LastIndexOf("ab", 100));
            Assert.AreEqual(0, text.LastIndexOf("a", -100));
            Assert.AreEqual(-1, text.LastIndexOf("ab", -100));
        }

        [Test]
        public static void LastIndexOfStartCountOverloadsClampToTextSlice()
        {
            var text = new Text("xxabcabcxx", 2, 6);

            Assert.AreEqual(-1, text.LastIndexOf('a', 5, 2));
            Assert.AreEqual(3, text.LastIndexOf('a', 5, 3));
            Assert.AreEqual(0, text.LastIndexOf('a', 2, 100));
            Assert.AreEqual(-1, text.LastIndexOf('a', 5, 0));

            Assert.AreEqual(-1, text.LastIndexOf("ab", 5, 2));
            Assert.AreEqual(3, text.LastIndexOf("ab", 5, 3));
            Assert.AreEqual(0, text.LastIndexOf("ab", 2, 100));
            Assert.AreEqual(-1, text.LastIndexOf("ab", 5, 0));
        }

        [Test]
        public static void LastIndexOfReturnsMinusOneForMissingValues()
        {
            var text = new Text("abcabc");

            Assert.AreEqual(-1, text.LastIndexOf('z'));
            Assert.AreEqual(-1, text.LastIndexOf("zz"));
            Assert.AreEqual(-1, text.LastIndexOf('c', 1));
            Assert.AreEqual(-1, text.LastIndexOf("bc", 0, 1));
        }

        [Test]
        public static void LastIndexOfHandlesEmptyText()
        {
            var text = Text.Empty;

            Assert.AreEqual(-1, text.LastIndexOf('a'));
            Assert.AreEqual(-1, text.LastIndexOf("a"));
            Assert.AreEqual(-1, text.LastIndexOf('a', 0));
            Assert.AreEqual(-1, text.LastIndexOf("a", 0));
            Assert.AreEqual(-1, text.LastIndexOf('a', 0, 1));
            Assert.AreEqual(-1, text.LastIndexOf("a", 0, 1));
        }

        [Test]
        public static void TextParserTrySkipCharMatchesFinalCharacter()
        {
            var text = new Text("cba");
            var parser = CreateParser(text);
            parser.Pos = text.End - 1;

            Assert.IsTrue(parser.TrySkip('a'));
            Assert.AreEqual(text.End, parser.Pos);
        }

        [Test]
        public static void TextParserTrySkipStringUsesAbsolutePositionInSlicedText()
        {
            var text = new Text("xxabcxx", 2, 3);
            var parser = CreateParser(text);

            Assert.IsTrue(parser.TrySkip("abc"));
            Assert.AreEqual(text.End, parser.Pos);
        }

        [Test]
        public static void TextParserTrySkipRegexAdvancesToMatchEnd()
        {
            var text = new Text("abc123");
            var parser = CreateParser(text);

            Assert.IsTrue(parser.TrySkip(new Rx("[A-Za-z]+")));
            Assert.AreEqual(3, parser.Pos);
            Assert.AreEqual(3, parser.LastEnd);
        }

        [Test]
        public static void TextParserTrySkipRegexCountsLines()
        {
            var text = new Text("ab\ncd!");
            var parser = CreateParser(text);

            Assert.IsTrue(parser.TrySkip(new Rx("ab\ncd")));
            Assert.AreEqual(5, parser.Pos);
            Assert.AreEqual(1, parser.Line.Index);
            Assert.AreEqual(3, parser.Line.Start);
        }

        [Test]
        public static void TextParserTrySkipRegexMismatchDoesNotAdvance()
        {
            var text = new Text("abc");
            var parser = CreateParser(text);
            var pos = parser.Pos;
            var lastEnd = parser.LastEnd;

            Assert.IsFalse(parser.TrySkip(new Rx("[0-9]+")));
            Assert.AreEqual(pos, parser.Pos);
            Assert.AreEqual(lastEnd, parser.LastEnd);
        }

        [Test]
        public static void TextParserTrySkipCharMismatchAtFinalCharacterDoesNotAdvance()
        {
            var text = new Text("cba");
            var parser = CreateParser(text);
            parser.Pos = text.End - 1;
            var pos = parser.Pos;

            Assert.IsFalse(parser.TrySkip('b'));
            Assert.AreEqual(pos, parser.Pos);
        }

        [Test]
        public static void XmlParserKeepsQualifiedNamesWhenSimpleNamesDisabled()
        {
            var parser = new XmlParser
            {
                SeenName = false,
                SeenSlash = false,
                PreserveWhiteSpace = true,
                SimpleNames = false,
                Separator = ':',
            };
            var root = new XmlItem();

            TextParser<XmlParser>.Parse(new Text("<foo:bar baz:qux=\"value\" />"), parser, XmlParser.TextState, root);

            Assert.AreEqual("foo:bar", root.Name.ToString());
            Assert.AreEqual(1, root.AttList.Count);
            Assert.AreEqual("baz:qux", root.AttList[0].Key.ToString());
        }

        private static void AssertParsedBool(Text text, int start, ParseError error, bool value, int length, string context)
        {
            var parsed = text.ParsedValueOfBoolAt(start);
            Assert.AreEqual(error, parsed.Error, context);
            Assert.AreEqual(value, parsed.Value, context);
            Assert.AreEqual(length, parsed.Length, context);
        }

        [Test]
        public static void BooleanParsingAcceptsLiteralViewsAndReportsLengths()
        {
            foreach (var literal in new[] { ("t", true), ("T", true), ("true", true), ("TRUE", true), ("TrUe", true),
                                            ("f", false), ("F", false), ("false", false), ("FALSE", false), ("FaLsE", false) })
            foreach (var leading in new[] { "", " ", "\t", "\r\n", " \t\r\n" })
            foreach (var trailing in new[] { "", " ", "\t", "\r\n", " \t\r\n" })
            foreach (var offset in new[] { 0, 3, 7 })
            foreach (var outside in new[] { "", "excluded" })
            {
                var content = leading + literal.Item1 + trailing;
                var text = new Text(new string('#', offset) + content + outside, offset, content.Length);
                var context = $"literal={literal.Item1}, leading=[{leading}], trailing=[{trailing}], offset={offset}, outside={outside}";
                Assert.AreEqual(literal.Item2, text.ParseBool(), context);
                AssertParsedBool(text, leading.Length, ParseError.None, literal.Item2, literal.Item1.Length, context);
            }
        }

        [Test]
        public static void BooleanParsingConfinesTokensAndRetainsPrefixConsumption()
        {
            foreach (var literal in new[] { ("true", true), ("TrUe", true), ("TRUE", true),
                                            ("false", false), ("FaLsE", false), ("FALSE", false) })
            foreach (var offset in new[] { 0, 3, 7 })
            foreach (var start in new[] { 0, 2, 5 })
            {
                var backing = new string('#', offset) + new string(' ', start) + literal.Item1 + "!excluded";
                foreach (var length in new[] { 2, 3, 4, 5, 1, 0 })
                {
                    if (length > literal.Item1.Length) continue;
                    var text = new Text(backing, offset, start + length);
                    var context = $"literal={literal.Item1}, length={length}, offset={offset}, start={start}";
                    if (length == 1 || length == literal.Item1.Length)
                        Assert.AreEqual(literal.Item2, text.ParseBool(), context);
                    else
                        Assert.Throws<System.ArgumentException>(() => text.ParseBool(), context);
                    AssertParsedBool(text, start, length == 0 ? ParseError.EndOfText : ParseError.None,
                                     length != 0 && literal.Item2, length == literal.Item1.Length ? length : length == 0 ? 0 : 1, context);
                }
                // A direct parse consumes a prefix; ParseBool still rejects an unconsumed suffix inside the view.
                foreach (var token in new[] { literal.Item1, literal.Item1.Substring(0, 1) })
                {
                    var content = new string(' ', start) + token + "!";
                    var text = new Text(new string('#', offset) + content, offset, content.Length);
                    var context = $"prefix={token}, offset={offset}, start={start}";
                    AssertParsedBool(text, start, ParseError.None, literal.Item2, token.Length, context);
                    Assert.Throws<System.ArgumentException>(() => text.ParseBool(), context);
                }
            }
        }

        [Test]
        public static void BooleanParsingRetainsEmptyEndAndInvalidTokenErrors()
        {
            foreach (var text in new[] { Text.Empty, default(Text), new Text(""), new Text("#####", 3, 0), new Text("#####", 5, 0) })
            {
                var context = $"empty start={text.Start}, end={text.End}, null backing={text.String == null}";
                AssertParsedBool(text, 0, ParseError.EndOfText, false, 0, context);
                AssertParsedBool(text, 1, ParseError.EndOfText, false, 0, context);
                Assert.Throws<System.ArgumentException>(() => text.ParseBool(), context);
            }
            foreach (var content in new[] { "x", "0", "1", "+true", "-false", "\u00a0", "\uFEFF", " ", "\t", "\r\n", " \t\r\n" })
            foreach (var offset in new[] { 0, 3, 7 })
            foreach (var outside in new[] { "", "truefalse" })
            {
                var text = new Text(new string('#', offset) + content + outside, offset, content.Length);
                var context = $"invalid=[{content}], offset={offset}, outside={outside}";
                AssertParsedBool(text, 0, ParseError.OutOfRange, false, 0, context);
                AssertParsedBool(text, text.Count, ParseError.EndOfText, false, 0, context);
                AssertParsedBool(text, text.Count + 1, ParseError.EndOfText, false, 0, context);
                Assert.Throws<System.ArgumentException>(() => text.ParseBool(), context);
            }
            foreach (var content in new[] { "t", "true", "f", "false" })
            {
                var text = new Text("###" + content, 3, content.Length);
                AssertParsedBool(text, text.Count, ParseError.EndOfText, false, 0, content);
                AssertParsedBool(text, text.Count + 1, ParseError.EndOfText, false, 0, content);
            }
        }
    }
}
