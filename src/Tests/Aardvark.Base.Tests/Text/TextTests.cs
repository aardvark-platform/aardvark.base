using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests.TextUtilities
{
    [TestFixture]
    public static class TextTests
    {
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
