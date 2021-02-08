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

    //# var smalltypes = new [] { Meta.SByteType, Meta.ShortType, Meta.ByteType, Meta.UShortType };
    //# foreach (var t in Meta.VecFieldTypes) {
    //#     var type = t.Name;
    //#     var Type = t.Caps;
    //#     var rtype = (type == "float") ? "float" : "double";
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class IntegerPower__Type__
    {
        const int count = 10000000;
        readonly __type__[] numbers = new __type__[count];
        readonly int[] exponents = new int[count];

        public IntegerPower__Type__()
        {
            var rnd = new RandomSystem(1);
            numbers.SetByIndex(i =>
            {
                var x = rnd.UniformDouble() - 0.5;
                x += 0.1 * Fun.Sign(x);
                return (__type__)(x * 10);
            });
            exponents.SetByIndex(i => rnd.UniformInt(32));
        }

        [Benchmark]
        public __rtype__ Pow()
        {
            __rtype__ sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += numbers[i].Pow((__rtype__)exponents[i]);
            }

            return sum;
        }

        [Benchmark]
        public __type__ Pown()
        {
            __type__ sum = 0;

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

                if (y > __type__.MinValue && y < __type__.MaxValue)
                {
                    if (x != 0 && y != 0)
                        Assert.IsTrue(Fun.ApproximateEquals(x / y, 1.0, 1e-3), "{0} != {1}", x, y);
                }
            }
        }
    }

    //# }
}