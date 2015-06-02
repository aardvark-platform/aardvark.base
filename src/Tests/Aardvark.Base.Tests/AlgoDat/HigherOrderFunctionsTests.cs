using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Aardvark.Base;
using NUnit.Framework;

namespace Aardvark.Tests
{
    [TestFixture]
    public class HighFunTests : TestSuite
    {
        public HighFunTests() : base() { }
        public HighFunTests(TestSuite.Options options) : base(options) { }

        [Test]
        public void TestHighFun()
        {
            FixedPointTests();
            CurryingTests();
        }

        public void FixedPointTests()
        {
            Test.Begin("fixed point combinator tests");
            var fib = HighFun.Y<int, int>(
                            f => n => n > 1 ? f(n - 1) + f(n - 2) : n);
            var fact = HighFun.Y<int, int>(
                            f => n => n > 1 ? n * f(n - 1) : 1);
            var factk = HighFun.Y<int, int, int>(
                            f => (n, k) => n > 1 ? n * f(n - k, k) : 1);
            var fastfib = HighFun.Y<int, int, int, int>(
                            f => (n, a, b) => n < 2 ? b : f(n - 1, b, a + b))
                        .ApplyArg1Arg2(0, 1);

            Test.IsTrue(fib(7) == 13, "fib(7) != 13");
            Test.IsTrue(fastfib(7) == 13, "ffib(7) != 13");
            Test.IsTrue(fib(8) == 21, "fib(8) != 21");
            Test.IsTrue(fastfib(8) == 21, "ffib(8) != 21");
            Test.IsTrue(fact(6) == 720, "fact(6) != 120");
            Test.IsTrue(factk(7, 2) == 105, "factk(7,2) != 105");
            Test.End();
        }

        public void CurryingTests()
        {
            Test.Begin("currying tests");
            Func<double, double, double> Power = Fun.Pow;
            var CurriedPower = Power.Curry();
            var PowerOfThree = CurriedPower(3.0);
            var Cube = Power.ApplyArg1(3.0); // we start counting args at 0

            for (var x = 0.0; x < 10.01; x += 0.5)
            {
                Test.IsTrue(Cube(x) == Fun.Pow(x, 3.0),
                        "Cube({0}) != Fun.Pow({0}, 3.0)", x);
                Test.IsTrue(PowerOfThree(x) == Fun.Pow(3.0, x),
                        "PowerOfThree({0}) == Fun.Pow(3.0,{0})", x);
            }
            Test.End();
        }
    
    }
}
