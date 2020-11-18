using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class FunTests : TestSuite
    {
        public FunTests() : base() { }
        public FunTests(TestSuite.Options options) : base(options) { }

        [Test]
        public static void AngleDistanceTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 1000; i++)
            {
                var alpha = rnd.UniformDouble() * Constant.PiTimesTwo;
                var delta = (rnd.UniformDouble() - 0.5) * 2.0 * Constant.Pi;
                var beta = alpha + (delta + Constant.PiTimesTwo * rnd.UniformInt(10));

                var dist = alpha.AngleDistance(beta);
                Assert.AreEqual(dist, Fun.Abs(delta), 1e-8);
            }
        }

        [Test]
        public static void AngleDifferenceTest()
        {
            var rnd = new RandomSystem(1);

            for (int i = 0; i < 1000; i++)
            {
                var alpha = rnd.UniformDouble() * Constant.PiTimesTwo * rnd.UniformInt(10);
                var delta = (rnd.UniformDouble() - 0.5) * Constant.Pi;
                if (delta == Constant.Pi)
                    delta = -Constant.Pi;

                var beta = alpha + (delta + Constant.PiTimesTwo * rnd.UniformInt(10));

                var dist = alpha.AngleDifference(beta);
                Assert.AreEqual(dist, delta, 1e-8);
            }
        }

        [Test]
        public void MinMaxVarArg()
        {
            var rnd = new RandomSystem(1);
            var a = rnd.UniformInt();
            var b = rnd.UniformInt();
            var c = rnd.UniformInt();
            var d = rnd.UniformInt();
            var e = rnd.UniformInt();

            var min = Fun.Min(a, b, c, d, e);
            var min_ref = Fun.Min(Fun.Min(Fun.Min(Fun.Min(a, b), c), d), e);
            Assert.AreEqual(min, min_ref, "Min not equal to reference");

            var max = Fun.Min(a, b, c, d, e);
            var max_ref = Fun.Min(Fun.Min(Fun.Min(Fun.Min(a, b), c), d), e);
            Assert.AreEqual(max, max_ref, "Max not equal to reference");
        }

        [Test]
        public void MinMaxVecVarArg()
        {
            var rnd = new RandomSystem(1);
            var a = rnd.UniformV4i();
            var b = rnd.UniformV4i();
            var c = rnd.UniformV4i();
            var d = rnd.UniformV4i();
            var e = rnd.UniformV4i();

            var min = Fun.Min(a, b, c, d, e);
            var min_ref = Fun.Min(Fun.Min(Fun.Min(Fun.Min(a, b), c), d), e);
            Assert.AreEqual(min, min_ref, "Min not equal to reference");

            var max = Fun.Min(a, b, c, d, e);
            var max_ref = Fun.Min(Fun.Min(Fun.Min(Fun.Min(a, b), c), d), e);
            Assert.AreEqual(max, max_ref, "Max not equal to reference");
        }
    }
}
