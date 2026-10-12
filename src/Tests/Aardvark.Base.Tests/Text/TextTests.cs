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

        private static void AssertSubResult(Text source, int start, int end, int expectedStart, int expectedEnd, string expectedContents)
        {
            string context = $"view=[{source.Start},{source.End}), backing={source.String ?? "<null>"}, start={start}, end={end}";
            var result = source.Sub(start, end);
            Assert.AreEqual(expectedStart, result.Start, context + ", start coordinate");
            Assert.AreEqual(expectedEnd, result.End, context + ", end coordinate");
            Assert.AreEqual(expectedContents.Length, result.Count, context + ", count");
            if (expectedContents.Length == 0)
                Assert.IsNull(result.String, context + ", canonical empty backing");
            else
            {
                Assert.AreSame(source.String, result.String, context + ", shared backing");
                Assert.GreaterOrEqual(result.Start, source.Start, context);
                Assert.LessOrEqual(result.End, source.End, context);
            }
            Assert.AreEqual(expectedContents, result.ToString(), context + ", contents");
        }

        [Test]
        public static void SubClampsExtremePositiveIndicesOnOffsetViews()
        {
            foreach (var view in new[] { new Text("abc", 1), new Text("abc", 2), new Text("xabc!", 1, 3), new Text("xxabcxx", 2, 3) })
            {
                AssertSubResult(view, 0, int.MaxValue, view.Start, view.End, view.ToString());
                AssertSubResult(view, -1, int.MaxValue, view.End - 1, view.End, "c");
                AssertSubResult(view, int.MaxValue, 0, 0, 0, string.Empty);
                AssertSubResult(view, int.MaxValue, 1, 0, 0, string.Empty);
                AssertSubResult(view, int.MaxValue - 1, int.MaxValue, 0, 0, string.Empty);
            }
        }

        [Test]
        public static void SubMatchesWideRelativeIndexClamping()
        {
            var views = new[]
            {
                Text.Empty, default(Text), new Text(string.Empty),
                new Text("abc", 0, 0), new Text("abc", 1, 0), new Text("abc", 3, 0),
                new Text("a"), new Text("abc"), new Text("abc", 1), new Text("xxabcxx", 2, 3),
                new Text("a🙂éz"), new Text("!a🙂éz!", 1, 5), new Text("xxxxxxxxxxxxxxxxxabc!", 17, 3),
            };
            var indices = new[] { int.MinValue, int.MinValue + 1, -100, -6, -4, -3, -2, -1, 0, 1, 2, 3, 4, 6, 100, int.MaxValue - 1, int.MaxValue };
            foreach (var view in views)
            foreach (int start in indices)
            foreach (int end in indices)
            {
                long count = view.Count;
                long relativeStart = start < 0 ? count + (long)start : start;
                long relativeEnd = end <= 0 ? count + (long)end : end;
                relativeStart = System.Math.Max(0L, System.Math.Min(count, relativeStart));
                relativeEnd = System.Math.Max(0L, System.Math.Min(count, relativeEnd));
                int expectedStart = 0, expectedEnd = 0;
                string expectedContents = string.Empty;
                if (relativeStart < relativeEnd)
                {
                    expectedStart = view.Start + (int)relativeStart;
                    expectedEnd = view.Start + (int)relativeEnd;
                    expectedContents = view.String.Substring(expectedStart, expectedEnd - expectedStart);
                }
                AssertSubResult(view, start, end, expectedStart, expectedEnd, expectedContents);
            }
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
    }
}
