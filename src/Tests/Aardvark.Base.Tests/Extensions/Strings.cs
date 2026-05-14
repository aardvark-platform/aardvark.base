using NUnit.Framework;
using Aardvark.Base;

namespace Aardvark.Tests.Extensions
{
    static class Strings
    {
        [Test]
        public static void PluralHandlesEmptyString()
        {
            Assert.AreEqual("s", "".Plural(2));
        }

        [Test]
        public static void PluralHandlesOneCharacterWord()
        {
            Assert.AreEqual("xs", "x".Plural(2));
        }

        [Test]
        public static void PluralWithCountOneReturnsOriginalShortWord()
        {
            Assert.AreEqual("x", "x".Plural(1));
        }

        [Test]
        public static void PluralPreservesExistingSuffixRule()
        {
            Assert.AreEqual("indices", "index".Plural(2));
        }
    }
}
