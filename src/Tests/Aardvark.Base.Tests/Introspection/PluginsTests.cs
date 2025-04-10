using Aardvark.Base;
using NUnit.Framework;
using System.Runtime.InteropServices;

namespace Aardvark.Tests
{
    [TestFixture]
    public static class PluginsTests
    {
        [OneTimeSetUp]
        public static void Setup()
        {
            IntrospectionProperties.CustomEntryAssembly = typeof(PluginsTests).Assembly;
            Aardvark.Base.Aardvark.Init();
        }

        [DllImport("testliba")]
        private static extern int foo();

        [Test]
        public static void Native()
        {
            var value = foo();
            Assert.AreEqual(90, value);
        }

        [Test]
        public static void Init()
        {
            // TestLib sets Report.Verbosity to 90, this way we can check if init was called without referencing the project directly
            var value = Report.Verbosity;
            Assert.AreEqual(90, value);
        }
    }
}
