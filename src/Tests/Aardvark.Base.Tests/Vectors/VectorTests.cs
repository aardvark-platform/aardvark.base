using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class VectorTests
    {
        [Test]
        public void Vector_Invalid1()
        {
            Assert.IsTrue(new V2d(double.NaN, double.NaN).IsNaN);
        }

        [Test]
        public void Vector_Invalid2()
        {
            Assert.IsTrue(new V2d(double.NaN, 0.0).IsNaN);
        }

        [Test]
        public void Vector_Invalid3()
        {
            Assert.IsTrue(new V2d(0.0, double.NaN).IsNaN);
        }

        [Test]
        public void Vector_Invalid4()
        {
            Assert.IsTrue(!new V2d(0.0, 0.0).IsNaN);
        }
    }
}
