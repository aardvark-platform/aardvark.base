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

    //# foreach (var t in Meta.RealTypes) {
    //#     var type = t.Name;
    //#     var Type = t.Caps;
    //#     var rtype = (type == "float") ? "float" : "double";
    //#     var one = (type == "float") ? "1.0f" : "1.0";
    //#     var two = (type == "float") ? "2.0f" : "2.0";
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class PowerOfTwo__Type__
    {
        const int count = 10000000;
        readonly int[] exponents = new int[count];

        public PowerOfTwo__Type__()
        {
            var rnd = new RandomSystem(1);
            exponents.SetByIndex(i => rnd.UniformInt(64) - 32);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private __rtype__ PowerOfTwoSigned(int exponent)
        {
            return (exponent < 0) ? __one__ / Fun.PowerOfTwo(-exponent) : Fun.PowerOfTwo(exponent);
        }

        [Benchmark]
        public __rtype__ Pow()
        {
            __rtype__ sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += Fun.Pow(__two__, (__rtype__)exponents[i]);
            }

            return sum;
        }

        [Benchmark]
        public __rtype__ PowerOfTwoSigned()
        {
            __rtype__ sum = 0;

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
                var x = Fun.Pow(__two__, e);
                var y = PowerOfTwoSigned(e);

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