using System;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class VectorTests
    {
        [Test]
        public void Vector_MinMaxElement()
        {
            var rnd = new RandomSystem(1);
            var v = rnd.UniformV4i();

            var min1 = v.MinElement;
            var min2 = Vec.MinElement(v);
            var min_ref = v.ToArray().Min();

            Assert.AreEqual(min1, min_ref);
            Assert.AreEqual(min2, min_ref);

            var max1 = v.MaxElement;
            var max2 = Vec.MaxElement(v);
            var max_ref = v.ToArray().Max();

            Assert.AreEqual(max1, max_ref);
            Assert.AreEqual(max2, max_ref);
        }

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
