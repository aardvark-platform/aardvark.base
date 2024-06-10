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
    //#     var fc = rt.Char;
    //#     var rot3t = "Rot3" + fc;
    //#     var v3rt = Meta.VecTypeOf(3, rt).Name;
    //#     var m44rt = Meta.MatTypeOf(4, 4, rt).Name;
    //#     var eps = (rt == Meta.DoubleType) ? "0.0001" : "0.01f";
    //#     var half = (rt == Meta.DoubleType) ? "0.5" : "0.5f";
    //#     var cast = (rt == Meta.DoubleType) ? "" : "(" + rtype + ")"; 
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class DistanceRot3__fc__
    {
        const int count = 10000000;
        readonly __rot3t__[] A = new __rot3t__[count];
        readonly __rot3t__[] B = new __rot3t__[count];
        readonly __rtype__[] angles = new __rtype__[count];

        private static __v3rt__ RndAxis(RandomSystem rnd)
            => rnd.Uniform__v3rt__Direction();
        
        private static __rtype__ RndAngle(RandomSystem rnd)
            => rnd.Uniform__Rtype__() * __cast__Constant.Pi;

        private static __rot3t__ RndRot(RandomSystem rnd)
            => __rot3t__.Rotation(RndAxis(rnd), RndAngle(rnd));

        public DistanceRot3__fc__()
        {
            var rnd = new RandomSystem(1);
            A.SetByIndex(i => RndRot(rnd));
            angles.SetByIndex(i =>
            {
                __rtype__ a = 0;
                do { a = RndAngle(rnd); }
                while (a == 0);

                return a;
            });

            B.SetByIndex(i =>
            {
                var r = __rot3t__.Rotation(RndAxis(rnd), angles[i]);
                return r * A[i];
            });
        }

        private double[] ComputeRelativeError(Func<__rot3t__, __rot3t__, __rtype__> f)
        {
            double[] error = new double[count];

            for (int i = 0; i < count; i++)
            {
                var v = (double)angles[i];
                var va = (double)f(A[i], B[i]);

                error[i] = Fun.Abs(v - va) / Fun.Abs(v);
            }

            return error;
        }

        public void BenchmarkNumericalStability()
        {
            var methods = new Dictionary<string, Func<__rot3t__, __rot3t__, __rtype__>>()
                {
                    { "Stable", Rot.Distance },
                    { "Fast", Rot.DistanceFast }
                };

            Console.WriteLine("Benchmarking numerical stability for __rot3t__ distance");
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
        public __rtype__ DistanceStable()
        {
            __rtype__ sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += A[i].Distance(B[i]);
            }

            return sum;
        }

        [Benchmark]
        public __rtype__ DistanceFast()
        {
            __rtype__ sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += A[i].DistanceFast(B[i]);
            }

            return sum;
        }

        [Test]
        public void ConsistentAndCorrect()
        {
            for (int i = 0; i < count / 100; i++)
            {
                var x = Rot.Distance(A[i], B[i]);
                var y = Rot.DistanceFast(A[i], B[i]);

                // Consistency
                Assert.IsTrue(Fun.ApproximateEquals(x, y, __eps__),
                        "Solutions do not match{0} != {1}", x, y);

                // Correctness
                Assert.GreaterOrEqual(x, 0);
                Assert.LessOrEqual(x, (__rtype__)Constant.Pi);
                Assert.IsTrue(Fun.ApproximateEquals(x, angles[i], __eps__),
                        "Solution does not match reference {0} != {1}", x, angles[i]);
            }
        }
    }

    //# }
}