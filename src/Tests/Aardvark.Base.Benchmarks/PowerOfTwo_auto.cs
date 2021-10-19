using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class PowerOfTwoFloat
    {
        const int count = 10000000;
        readonly int[] exponents = new int[count];

        public PowerOfTwoFloat()
        {
            var rnd = new RandomSystem(1);
            exponents.SetByIndex(i => rnd.UniformInt(64) - 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private float PowerOfTwoSigned(int exponent)
        {
            return (exponent < 0) ? 1.0f / Fun.PowerOfTwo(-exponent) : Fun.PowerOfTwo(exponent);
        }

        [Benchmark]
        public float Pow()
        {
            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Fun.Pow(2.0f, (float)exponents[i]);
            }

            return sum;
        }

        [Benchmark]
        public float PowerOfTwoSigned()
        {
            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += PowerOfTwoSigned(exponents[i]);
            }

            return sum;
        }

        [Test]
        public void PowerOfTwoTest()
        {
            for (int i = 0; i < count; i++)
            {
                var e = exponents[i];
                var x = Fun.Pow(2.0f, e);
                var y = PowerOfTwoSigned(e);

                if (y > float.MinValue && y < float.MaxValue)
                {
                    if (x != 0 && y != 0)
                        Assert.IsTrue(Fun.ApproximateEquals(x / y, 1.0, 1e-3), "{0} != {1}", x, y);
                }
            }
        }
    }

    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class PowerOfTwoDouble
    {
        const int count = 10000000;
        readonly int[] exponents = new int[count];

        public PowerOfTwoDouble()
        {
            var rnd = new RandomSystem(1);
            exponents.SetByIndex(i => rnd.UniformInt(64) - 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private double PowerOfTwoSigned(int exponent)
        {
            return (exponent < 0) ? 1.0 / Fun.PowerOfTwo(-exponent) : Fun.PowerOfTwo(exponent);
        }

        [Benchmark]
        public double Pow()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Fun.Pow(2.0, (double)exponents[i]);
            }

            return sum;
        }

        [Benchmark]
        public double PowerOfTwoSigned()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += PowerOfTwoSigned(exponents[i]);
            }

            return sum;
        }

        [Test]
        public void PowerOfTwoTest()
        {
            for (int i = 0; i < count; i++)
            {
                var e = exponents[i];
                var x = Fun.Pow(2.0, e);
                var y = PowerOfTwoSigned(e);

                if (y > double.MinValue && y < double.MaxValue)
                {
                    if (x != 0 && y != 0)
                        Assert.IsTrue(Fun.ApproximateEquals(x / y, 1.0, 1e-3), "{0} != {1}", x, y);
                }
            }
        }
    }

}