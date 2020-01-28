using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    //# foreach (var rt in Meta.RealTypes) {
    //#     var rtype = rt.Name;
    //#     var Rtype = rt.Caps;
    //#     var v3rt = Meta.VecTypeOf(3, rt).Name;
    //#     var m44rt = Meta.MatTypeOf(4, 4, rt).Name;
    //#     var eps = (rt == Meta.DoubleType) ? 0.0001f : 0.01f;
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class AngleBetween__Rtype__
    {
        const int count = 10000000;
        readonly __v3rt__[] A = new __v3rt__[count];
        readonly __v3rt__[] B = new __v3rt__[count];
        readonly __rtype__[] angles = new __rtype__[count];

        public AngleBetween__Rtype__()
        {
            var rnd = new RandomSystem(1);
            A.SetByIndex(i => new __v3rt__(rnd.UniformV3dDirection()));
            angles.SetByIndex(i => rnd.Uniform__Rtype__() * (__rtype__)Constant.Pi);
            B.SetByIndex(i =>
            {
                __v3rt__ v;
                do { v = new __v3rt__(rnd.UniformV3dDirection()); } 
                while (v.Dot(A[i]).IsTiny());

                __v3rt__ axis = v.Cross(A[i]).Normalized;

                return __m44rt__.Rotation(axis, angles[i]).TransformDir(A[i]);
            });
        }

        private double[] ComputeRelativeError(Func<__v3rt__, __v3rt__, __rtype__> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var v = (double) angles[i];
                var va = (double) f(A[i], B[i]);

                error[i] = Fun.Abs(v - va) / Fun.Abs(v);
            }

            return error;
        }

        public void BenchmarkNumericalStability()
        {
            var methods = new Dictionary<string, Func<__v3rt__, __v3rt__, __rtype__>>()
                {
                    { "Stable", VecFun.AngleBetween },
                    { "Fast", VecFun.AngleBetweenFast }
                };

            Console.WriteLine("Benchmarking numerical stability for AngleBetween__Rtype__");
            Console.WriteLine();

            foreach (var m in methods)
            {
                var errors = ComputeRelativeError(m.Value);
                var min = errors.Min();
                var max = errors.Max();
                var mean = errors.Mean();
                var var = errors.Variance();

                Report.Begin("Relative error for '{0}'", m.Key);
                Report.Line("Min = {0}", min);
                Report.Line("Max = {0}", max);
                Report.Line("Mean = {0}", mean);
                Report.Line("Variance = {0}", var);
                Report.End();

                Console.WriteLine();
            }
        }

        [Benchmark]
        public __rtype__ AngleBetweenStable()
        {
            __rtype__ sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += A[i].AngleBetween(B[i]);
            }

            return sum;
        }

        [Benchmark]
        public __rtype__ AngleBetweenFast()
        {
            __rtype__ sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += A[i].AngleBetweenFast(B[i]);
            }

            return sum;
        }

        [Test]
        public void AngleBetweenStableTest()
        {
            for (int i = 0; i < count; i++)
            {
                var x = A[i].AngleBetween(B[i]);
                var y = angles[i];

                Assert.IsTrue(Fun.ApproximateEquals(x, y, __eps__), "{0} != {1}", x, y);
            }
        }

        [Test]
        public void AngleBetweenFastTest()
        {
            for (int i = 0; i < count; i++)
            {
                var x = A[i].AngleBetweenFast(B[i]);
                var y = angles[i];

                Assert.IsTrue(Fun.ApproximateEquals(x, y, __eps__), "{0} != {1}", x, y);
            }
        }
    }

    //# }
}