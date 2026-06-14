using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class WindowTests : TestSuite
    {
        public WindowTests() : base() { }
        public WindowTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void MedianWindowRejectsNonPositiveWindowSize()
        {
            var zero = Assert.Throws<ArgumentOutOfRangeException>(() => new MedianWindow(0));
            var negative = Assert.Throws<ArgumentOutOfRangeException>(() => new MedianWindow(-1));

            Assert.That(zero.ParamName, Is.EqualTo("count"));
            Assert.That(negative.ParamName, Is.EqualTo("count"));
        }

        [Test]
        public void MedianWindowKeepsRollingMedianForValidWindowSize()
        {
            var window = new MedianWindow(3);

            Assert.That(window.Insert(5.0), Is.EqualTo(5.0));
            Assert.That(window.Insert(1.0), Is.EqualTo(5.0));
            Assert.That(window.Insert(9.0), Is.EqualTo(5.0));
            Assert.That(window.Insert(2.0), Is.EqualTo(2.0));

            Assert.That(window.Value, Is.EqualTo(2.0));
            Assert.That(window.Last, Is.EqualTo(2.0));
        }
    }
}
