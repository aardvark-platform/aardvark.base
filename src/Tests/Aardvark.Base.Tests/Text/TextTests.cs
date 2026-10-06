using System;
using System.Collections.Generic;
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

        private static void AssertTrimmedView(Text result, Text input, int leading, string content, string context)
        {
            Assert.AreEqual(content, result.ToString(), context);
            Assert.AreEqual(content.Length, result.Count, context);
            Assert.AreEqual(input.Start + leading, result.Start, context);
            Assert.AreEqual(result.Start + content.Length, result.End, context);
            Assert.AreSame(input.String, result.String, context);
        }

        [Test]
        public static void TrimmedRetainsSurvivingViewsAndSliceCoordinates()
        {
            var modes = new[]
            {
                (Name: "whitespace", Predicate: (Func<char, bool>)CharFun.IsWhiteSpace,
                 Padding: new[] { "", " ", "\t", "\r\n", " \t\r\n " }),
                (Name: "custom", Predicate: (Func<char, bool>)(c => c == '_' || char.IsDigit(c)),
                 Padding: new[] { "", "_", "12", "__345_" })
            };
            foreach (var mode in modes)
            foreach (var content in new[] { "a", "Z", "é", "\0", "\ud800", "ab", "a b", "a1_b", new string('v', 1024) })
            foreach (var leading in mode.Padding)
            foreach (var trailing in mode.Padding)
            foreach (var offset in new[] { 0, 2, 7 })
            {
                var payload = leading + content + trailing;
                var backing = offset == 0 ? payload : new string('#', offset) + payload + "#outside";
                var input = new Text(backing, offset, payload.Length);
                var context = $"{mode.Name}, content length={content.Length}, first=U+{(int)content[0]:X4}, " +
                              $"leading length={leading.Length}, trailing length={trailing.Length}, offset={offset}";
                AssertTrimmedView(input.Trimmed(mode.Predicate), input, leading.Length, content, context);
                if (mode.Name == "whitespace")
                    AssertTrimmedView(input.WhiteSpaceTrimmed, input, leading.Length, content, context);
            }
        }

        [Test]
        public static void TrimmedReturnsCanonicalEmptyWithoutReadingOutsideSlices()
        {
            var modes = new[]
            {
                (Predicate: (Func<char, bool>)CharFun.IsWhiteSpace, Inputs: new[] { "", " ", "\t", " \r\n\t " }),
                (Predicate: (Func<char, bool>)(c => c == '_' || char.IsDigit(c)), Inputs: new[] { "", "_", "123", "__12_" })
            };
            foreach (var mode in modes)
            foreach (var payload in mode.Inputs)
            foreach (var offset in new[] { 0, 3 })
            {
                var input = new Text(new string('#', offset) + payload + "#outside", offset, payload.Length);
                var visited = new List<char>();
                var result = input.Trimmed(c => { visited.Add(c); return mode.Predicate(c); });
                var context = $"all-trimmed length={payload.Length}, offset={offset}";
                Assert.AreEqual(0, result.Start, context);
                Assert.AreEqual(0, result.End, context);
                Assert.IsNull(result.String, context);
                CollectionAssert.AreEqual(payload, visited, context);
            }
            foreach (var input in new[] { Text.Empty, default(Text), new Text(""), new Text("outside", 3, 0), new Text(5, 3, "outside") })
            {
                Assert.IsNull(input.Trimmed(null).String);
                Assert.AreEqual(0, input.Trimmed(c => throw new InvalidOperationException()).Count);
            }
        }

        [Test]
        public static void TrimmedPreservesPredicateTraversalAndExceptionIdentity()
        {
            foreach (var content in new[] { "a", "abc" })
            foreach (var offset in new[] { 0, 3 })
            {
                var payload = " _" + content + "_ ";
                var input = new Text(new string('#', offset) + payload + "#outside", offset, payload.Length);
                var expectedVisits = content.Length == 1 ? " _a _" : " _a _c";
                var visited = new List<char>();
                Func<char, bool> predicate = c => { visited.Add(c); return c == ' ' || c == '_'; };
                var context = $"content={content}, offset={offset}";
                AssertTrimmedView(input.Trimmed(predicate), input, 2, content, context);
                CollectionAssert.AreEqual(expectedVisits, visited, context);
                for (var throwAt = 0; throwAt < expectedVisits.Length; throwAt++)
                {
                    visited.Clear();
                    var error = new InvalidOperationException("predicate failure");
                    var actual = Assert.Throws<InvalidOperationException>(() => input.Trimmed(c =>
                    {
                        visited.Add(c);
                        if (visited.Count == throwAt + 1) throw error;
                        return c == ' ' || c == '_';
                    }), $"{context}, throwAt={throwAt}");
                    Assert.AreSame(error, actual, context);
                    CollectionAssert.AreEqual(expectedVisits.Substring(0, throwAt + 1), visited, context);
                }
                Assert.Throws<NullReferenceException>(() => input.Trimmed(null), context);
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
