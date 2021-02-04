using Aardvark.Base;
using NUnit.Framework;
using System;

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

        private static double NextAfter(double input, int dir)
            => BitConverter.Int64BitsToDouble(BitConverter.DoubleToInt64Bits(input) + dir);

        private static float NextAfter(float input, int dir)
            => BitConverter.Int32BitsToSingle(BitConverter.SingleToInt32Bits(input) + dir);

        private static float SincFloatImpl(float x)
        {
            return MathF.Sin(ConstantF.Pi * x) / (ConstantF.Pi * x);
        }

        private static float SincFloatFallbackImpl(float x)
        {
            return (float)Math.Sin(ConstantF.Pi * x) / (ConstantF.Pi * x);
        }

        private static double SincDoubleImpl(double x)
        {
            return Math.Sin(Constant.Pi * x) / (Constant.Pi * x);
        }

        [Test]
        public void SincTest()
        {
            bool findEps = false;
            int iterations = 25000000;

            // Check if sinc(x) != 1 for x with |x| > eps
            {
                float eps = 0.00017791693f;
                float[] inputs = new float[] { eps, -eps };

                for (int n = 0; n < iterations; n++)
                {
                    for (int i = 0; i < inputs.Length; i++)
                    {
                        inputs[i] = NextAfter(inputs[i], 1);
                        float output = Fun.Sinc(inputs[i]);
                        Assert.IsTrue(output != 1, "Found input x with Fun.Sinc(x) = 1 and |x| > eps: x = {0}", inputs[i]);
                    }
                }
            }

            {
                double eps = 6.840859302478614E-09;
                double[] inputs = new double[] { eps, -eps };

                for (int n = 0; n < iterations; n++)
                {
                    for (int i = 0; i < inputs.Length; i++)
                    {
                        inputs[i] = NextAfter(inputs[i], 1);
                        double output = Fun.Sinc(inputs[i]);
                        Assert.IsTrue(output != 1, "Found input x with Fun.Sinc(x) = 1 and |x| > eps: x = {0}", inputs[i]);
                    }
                }
            }

            // Finding values for eps
            if (findEps)
            {
                Console.WriteLine("Finding eps for SincFloatImpl");

                {
                    // Find biggest number with sinc(x) = 1
                    float input = 1.0f;
                    float output = 0.0f;

                    while (output != 1.0f)
                    {
                        input = NextAfter(input, -1);
                        output = SincFloatImpl(input);
                    }

                    Console.WriteLine("sinc({0}) = {1} -> eps = {2}", input, output, NextAfter(input, 1));
                }

                {
                    // Find smallest number with sinc(x) = 1
                    float input = -1.0f;
                    float output = 0.0f;

                    while (output != 1.0f)
                    {
                        input = NextAfter(input, -1);
                        output = SincFloatImpl(input);
                    }

                    Console.WriteLine("sinc({0}) = {1} -> eps = {2}", input, output, NextAfter(input, 1));
                }

                Console.WriteLine();
                Console.WriteLine("Finding eps for SincFloatFallbackImpl");

                {
                    // Find biggest number with sinc(x) = 1
                    float input = 1.0f;
                    float output = 0.0f;

                    while (output != 1.0f)
                    {
                        input = NextAfter(input, -1);
                        output = SincFloatFallbackImpl(input);
                    }

                    Console.WriteLine("sinc({0}) = {1} -> eps = {2}", input, output, NextAfter(input, 1));
                }

                {
                    // Find smallest number with sinc(x) = 1
                    float input = -1.0f;
                    float output = 0.0f;

                    while (output != 1.0f)
                    {
                        input = NextAfter(input, -1);
                        output = SincFloatFallbackImpl(input);
                    }

                    Console.WriteLine("sinc({0}) = {1} -> eps = {2}", input, output, NextAfter(input, 1));
                }

                Console.WriteLine();
                Console.WriteLine("Finding eps for SincDoubleImpl");

                {
                    // Find biggest number with sinc(x) = 1
                    double input = 6.84086E-09;
                    double output = 0.0;

                    while (output != 1.0)
                    {
                        input = NextAfter(input, -1);
                        output = SincDoubleImpl(input);
                    }

                    Console.WriteLine("sinc({0}) = {1} -> eps = {2}", input, output, NextAfter(input, 1));
                }

                {
                    // Find smallest number with sinc(x) = 1
                    double input = -6.84086E-09;
                    double output = 0.0;

                    while (output != 1.0)
                    {
                        input = NextAfter(input, -1);
                        output = SincDoubleImpl(input);
                    }

                    Console.WriteLine("sinc({0}) = {1} -> eps = {2}", input, output, NextAfter(input, 1));
                }
            }
        }
    }
}
