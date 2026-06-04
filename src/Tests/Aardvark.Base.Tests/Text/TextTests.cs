using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.TextUtilities
{
    [TestFixture]
    public static class TextTests
    {
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
    }
}
