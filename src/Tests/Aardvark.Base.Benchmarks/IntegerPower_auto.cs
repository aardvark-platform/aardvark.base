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
    public class IntegerPowerInt
    {
        const int count = 10000000;
        readonly int[] numbers = new int[count];
        readonly int[] exponents = new int[count];

        public IntegerPowerInt()
        {
            var rnd = new RandomSystem(1);
            numbers.SetByIndex(i =>
            {
                var x = rnd.UniformDouble() - 0.5;
                x += 0.1 * Fun.Sign(x);
                return (int)(x * 10);
            });
            exponents.SetByIndex(i => rnd.UniformInt(32));
        }

        [Benchmark]
        public double Pow()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += numbers[i].Pow((double)exponents[i]);
            }

            return sum;
        }

        [Benchmark]
        public int Pown()
        {
            int sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += numbers[i].Pown(exponents[i]);
            }

            return sum;
        }

        [Test]
        public void IntegerPowerTest()
        {
            for (int i = 0; i < count; i++)
            {
                var n = numbers[i]; var e = exponents[i];
                var x = Fun.Pown(n, e);
                var y = Fun.Pow((double)n, (double)e);

                if (y > int.MinValue && y < int.MaxValue)
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
    public class IntegerPowerLong
    {
        const int count = 10000000;
        readonly long[] numbers = new long[count];
        readonly int[] exponents = new int[count];

        public IntegerPowerLong()
        {
            var rnd = new RandomSystem(1);
            numbers.SetByIndex(i =>
            {
                var x = rnd.UniformDouble() - 0.5;
                x += 0.1 * Fun.Sign(x);
                return (long)(x * 10);
            });
            exponents.SetByIndex(i => rnd.UniformInt(32));
        }

        [Benchmark]
        public double Pow()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += numbers[i].Pow((double)exponents[i]);
            }

            return sum;
        }

        [Benchmark]
        public long Pown()
        {
            long sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += numbers[i].Pown(exponents[i]);
            }

            return sum;
        }

        [Test]
        public void IntegerPowerTest()
        {
            for (int i = 0; i < count; i++)
            {
                var n = numbers[i]; var e = exponents[i];
                var x = Fun.Pown(n, e);
                var y = Fun.Pow((double)n, (double)e);

                if (y > long.MinValue && y < long.MaxValue)
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
    public class IntegerPowerFloat
    {
        const int count = 10000000;
        readonly float[] numbers = new float[count];
        readonly int[] exponents = new int[count];

        public IntegerPowerFloat()
        {
            var rnd = new RandomSystem(1);
            numbers.SetByIndex(i =>
            {
                var x = rnd.UniformDouble() - 0.5;
                x += 0.1 * Fun.Sign(x);
                return (float)(x * 10);
            });
            exponents.SetByIndex(i => rnd.UniformInt(32));
        }

        [Benchmark]
        public float Pow()
        {
            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += numbers[i].Pow((float)exponents[i]);
            }

            return sum;
        }

        [Benchmark]
        public float Pown()
        {
            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += numbers[i].Pown(exponents[i]);
            }

            return sum;
        }

        [Test]
        public void IntegerPowerTest()
        {
            for (int i = 0; i < count; i++)
            {
                var n = numbers[i]; var e = exponents[i];
                var x = Fun.Pown(n, e);
                var y = Fun.Pow((double)n, (double)e);

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
    public class IntegerPowerDouble
    {
        const int count = 10000000;
        readonly double[] numbers = new double[count];
        readonly int[] exponents = new int[count];

        public IntegerPowerDouble()
        {
            var rnd = new RandomSystem(1);
            numbers.SetByIndex(i =>
            {
                var x = rnd.UniformDouble() - 0.5;
                x += 0.1 * Fun.Sign(x);
                return (double)(x * 10);
            });
            exponents.SetByIndex(i => rnd.UniformInt(32));
        }

        [Benchmark]
        public double Pow()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += numbers[i].Pow((double)exponents[i]);
            }

            return sum;
        }

        [Benchmark]
        public double Pown()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += numbers[i].Pown(exponents[i]);
            }

            return sum;
        }

        [Test]
        public void IntegerPowerTest()
        {
            for (int i = 0; i < count; i++)
            {
                var n = numbers[i]; var e = exponents[i];
                var x = Fun.Pown(n, e);
                var y = Fun.Pow((double)n, (double)e);

                if (y > double.MinValue && y < double.MaxValue)
                {
                    if (x != 0 && y != 0)
                        Assert.IsTrue(Fun.ApproximateEquals(x / y, 1.0, 1e-3), "{0} != {1}", x, y);
                }
            }
        }
    }

}