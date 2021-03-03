using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{

    [PlainExporter, MemoryDiagnoser]
    public class FracTest
    {
        public double[] foo = new double[10000].SetByIndex(i => i);

        public double Fract_Impl(double x)
        {
            return x - Math.Floor(x);
        }

        public double Fract_Impl2(double x)
        {
            return (x - Math.Floor(x)) % 1.0;
        }

        public double Fract_Impl3(double x)
        {
            var res = x - Math.Floor(x);
            return res == 1.0 ? 0.0 : res;
        }

        [Benchmark]
        public double Fract()
        {
            var a = foo;
            var fs = 0.0;
            for (int i = 0; i < a.Length; i++)
                fs += Fract_Impl(a[i]);
            return fs;
        }

        [Benchmark]
        public double Fract2()
        {
            var a = foo;
            var fs = 0.0;
            for (int i = 0; i < a.Length; i++)
                fs += Fract_Impl2(a[i]);
            return fs;
        }

        [Benchmark]
        public double Fract3()
        {
            var a = foo;
            var fs = 0.0;
            for (int i = 0; i < a.Length; i++)
                fs += Fract_Impl3(a[i]);
            return fs;
        }
    }
}
