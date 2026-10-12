using Aardvark.Base;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

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

        private sealed class BoundedLookupComparer : IEqualityComparer<string>
        {
            private readonly IEqualityComparer<string> inner;
            public readonly List<string> Keys = new List<string>();
            public int Budget = int.MaxValue;
            public int ThrowAt = int.MaxValue;
            public Exception Failure;

            public BoundedLookupComparer(IEqualityComparer<string> inner) => this.inner = inner;
            public bool Equals(string a, string b) => inner.Equals(a, b);
            public int GetHashCode(string key)
            {
                Keys.Add(key);
                if (Keys.Count > Budget) throw new InvalidOperationException("Replacement exceeded its match lookup budget.");
                if (Keys.Count == ThrowAt) throw Failure;
                return inner.GetHashCode(key);
            }
        }

        private static Dictionary<string, string> ReplacementMap(IEqualityComparer<string> comparer, int emptyMode)
        {
            var map = new Dictionary<string, string>(comparer)
            {
                ["a"] = "A$&", ["b"] = null, ["ab"] = "[AB]", ["_id"] = "__", ["WORD"] = "value"
            };
            if (emptyMode > 0) map[""] = emptyMode == 1 ? "|" : null;
            return map;
        }

        private static void CheckReplacement(Text view, Regex regex, StringComparer comparer, int emptyMode, bool identifiers = false)
        {
            string context = $"text={view}, start={view.Start}, pattern={regex}, options={regex.Options}, comparer={comparer}, emptyMode={emptyMode}";
            var map = ReplacementMap(comparer, emptyMode);
            var expectedKeys = new List<string>();
            string expected = regex.Replace(view.ToString(), match =>
            {
                expectedKeys.Add(match.Value);
                return map.TryGetValue(match.Value, out string replacement) ? replacement : match.Value;
            });
            var bounded = new BoundedLookupComparer(comparer);
            var actualMap = new Dictionary<string, string>(map, bounded);
            bounded.Keys.Clear();
            bounded.Budget = expectedKeys.Count + 1;
            Text actual = default;
            Assert.DoesNotThrow(() => actual = identifiers ? view.ReplaceIdentifiers(actualMap) : view.ReplaceParts(regex, actualMap), context);
            Assert.AreEqual(expected, actual.ToString(), context);
            CollectionAssert.AreEqual(expectedKeys, bounded.Keys, context);
            if (identifiers && view.Start == 0)
                Assert.AreEqual(expected, view.ToString().ReplaceIdentifiers(map), context);
            if (expectedKeys.Count == 0 && view.Start == 0)
            {
                Assert.AreSame(view.String, actual.String, context);
                Assert.AreEqual(view.Start, actual.Start, context);
                Assert.AreEqual(view.End, actual.End, context);
            }
        }

        private static IEnumerable<Text> ReplacementViews(string value)
        {
            yield return new Text(value);
            foreach (int offset in new[] { 2, 7 })
                yield return new Text(new string('x', offset) + value + "zz", offset, value.Length);
        }

        [Test]
        public static void ReplacePartsAdvancesZeroLengthAndMixedMatchesWithinViews()
        {
            foreach (string value in new[] { "a", "ab", "ba", "Aa b", "\na\n", " a b ", "🙂ab" })
            foreach (var view in ReplacementViews(value))
            foreach (string pattern in new[] { "", "^", @"\A", "(?=a)", "(?<=a)", @"\b", "a*", "a?", "(?=.)|.", "^a", "(ab)|(?=b)|$", "(?=z)|(?<=x)" })
            foreach (var options in new[] { RegexOptions.None, RegexOptions.Multiline })
            foreach (var comparer in new[] { StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase })
            foreach (int emptyMode in new[] { 0, 1, 2 })
                CheckReplacement(view, new Regex(pattern, options), comparer, emptyMode);
        }

        [Test]
        public static void ReplacePartsProcessesTerminalMatchesAndNonNullEmptyViews()
        {
            foreach (string value in new[] { "", "a", "ab", "\na\n" })
            foreach (var view in ReplacementViews(value))
            foreach (string pattern in new[] { "", "$", @"\z", "^|$", "a|$", "a*" })
            foreach (int emptyMode in new[] { 0, 1, 2 })
                CheckReplacement(view, new Regex(pattern), StringComparer.Ordinal, emptyMode);
            foreach (string pattern in new[] { "", "^", "$", @"\A|\z" })
                CheckReplacement(new Text("outside", 0, 0), new Regex(pattern), StringComparer.Ordinal, 1);
        }

        [Test]
        public static void ReplacePartsRetainsOrdinaryAndIdentifierLookupSemantics()
        {
            foreach (string value in new[] { "", "missing!", "a b a", "Aa BB WORD word", "a.b_a _id 77", "aéb ab\na" })
            foreach (var view in ReplacementViews(value))
            foreach (var comparer in new[] { StringComparer.Ordinal, StringComparer.OrdinalIgnoreCase })
            {
                foreach (string pattern in new[] { "a", "a|b", "(a)", @"\w+", "x+" })
                    CheckReplacement(view, new Regex(pattern), comparer, 0);
                CheckReplacement(view, Text.IdentifierRegex, comparer, 0, identifiers: true);
            }
        }

        [Test]
        public static void ReplacePartsPropagatesLookupAndArgumentFailures()
        {
            foreach (var view in ReplacementViews("aba"))
            {
                Assert.Throws<NullReferenceException>(() => view.ReplaceParts(null, ReplacementMap(StringComparer.Ordinal, 0)));
                Assert.Throws<NullReferenceException>(() => view.ReplaceParts(new Regex("a"), null));
                Assert.AreEqual(view.ToString(), view.ReplaceParts(new Regex("missing"), null).ToString());
                foreach (string pattern in new[] { "a", "", "a|$" })
                {
                    var regex = new Regex(pattern);
                    int count = regex.Matches(view.ToString()).Count;
                    for (int failAt = 1; failAt <= count; failAt++)
                    {
                        var failure = new InvalidOperationException("lookup failure");
                        var bounded = new BoundedLookupComparer(StringComparer.Ordinal);
                        var map = new Dictionary<string, string>(ReplacementMap(StringComparer.Ordinal, 1), bounded);
                        bounded.Keys.Clear();
                        bounded.Budget = count + 1;
                        bounded.ThrowAt = failAt;
                        bounded.Failure = failure;
                        string context = $"start={view.Start}, pattern={pattern}, failAt={failAt}";
                        Assert.AreSame(failure, Assert.Throws<InvalidOperationException>(() => view.ReplaceParts(regex, map), context), context);
                        Assert.AreEqual(failAt, bounded.Keys.Count, context);
                    }
                }
            }
            foreach (var view in new[] { Text.Empty, default(Text) })
                Assert.Throws<ArgumentNullException>(() => view.ReplaceParts(new Regex(""), ReplacementMap(StringComparer.Ordinal, 1)));
        }
    }
}
