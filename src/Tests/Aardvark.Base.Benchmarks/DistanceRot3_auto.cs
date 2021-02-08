using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class DistanceRot3f
    {
        const int count = 10000000;
        readonly Rot3f[] A = new Rot3f[count];
        readonly Rot3f[] B = new Rot3f[count];
        readonly float[] angles = new float[count];

        private static V3f RndAxis(RandomSystem rnd)
            => rnd.UniformV3fDirection();
        
        private static float RndAngle(RandomSystem rnd)
            => rnd.UniformFloat() * (float)Constant.Pi;

        private static Rot3f RndRot(RandomSystem rnd)
            => Rot3f.Rotation(RndAxis(rnd), RndAngle(rnd));

        public DistanceRot3f()
        {
            var rnd = new RandomSystem(1);
            A.SetByIndex(i => RndRot(rnd));
            angles.SetByIndex(i =>
            {
                float a = 0;
                do { a = RndAngle(rnd); }
                while (a == 0);

                return a;
            });

            B.SetByIndex(i =>
            {
                var r = Rot3f.Rotation(RndAxis(rnd), angles[i]);
                return r * A[i];
            });
        }

        private double[] ComputeRelativeError(Func<Rot3f, Rot3f, float> f)
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
            var methods = new Dictionary<string, Func<Rot3f, Rot3f, float>>()
                {
                    { "Stable", Rot.Distance },
                    { "Fast", Rot.DistanceFast }
                };

            Console.WriteLine("Benchmarking numerical stability for Rot3f distance");
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
        public float DistanceStable()
        {
            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += A[i].Distance(B[i]);
            }

            return sum;
        }

        [Benchmark]
        public float DistanceFast()
        {
            float sum = 0;

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
                Assert.IsTrue(Fun.ApproximateEquals(x, y, 0.01f),
                        "Solutions do not match{0} != {1}", x, y);

                // Correctness
                Assert.GreaterOrEqual(x, 0);
                Assert.LessOrEqual(x, (float)Constant.Pi);
                Assert.IsTrue(Fun.ApproximateEquals(x, angles[i], 0.01f),
                        "Solution does not match reference {0} != {1}", x, angles[i]);
            }
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class DistanceRot3d
    {
        const int count = 10000000;
        readonly Rot3d[] A = new Rot3d[count];
        readonly Rot3d[] B = new Rot3d[count];
        readonly double[] angles = new double[count];

        private static V3d RndAxis(RandomSystem rnd)
            => rnd.UniformV3dDirection();
        
        private static double RndAngle(RandomSystem rnd)
            => rnd.UniformDouble() * Constant.Pi;

        private static Rot3d RndRot(RandomSystem rnd)
            => Rot3d.Rotation(RndAxis(rnd), RndAngle(rnd));

        public DistanceRot3d()
        {
            var rnd = new RandomSystem(1);
            A.SetByIndex(i => RndRot(rnd));
            angles.SetByIndex(i =>
            {
                double a = 0;
                do { a = RndAngle(rnd); }
                while (a == 0);

                return a;
            });

            B.SetByIndex(i =>
            {
                var r = Rot3d.Rotation(RndAxis(rnd), angles[i]);
                return r * A[i];
            });
        }

        private double[] ComputeRelativeError(Func<Rot3d, Rot3d, double> f)
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
            var methods = new Dictionary<string, Func<Rot3d, Rot3d, double>>()
                {
                    { "Stable", Rot.Distance },
                    { "Fast", Rot.DistanceFast }
                };

            Console.WriteLine("Benchmarking numerical stability for Rot3d distance");
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
        public double DistanceStable()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += A[i].Distance(B[i]);
            }

            return sum;
        }

        [Benchmark]
        public double DistanceFast()
        {
            double sum = 0;

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
                Assert.IsTrue(Fun.ApproximateEquals(x, y, 0.0001),
                        "Solutions do not match{0} != {1}", x, y);

                // Correctness
                Assert.GreaterOrEqual(x, 0);
                Assert.LessOrEqual(x, (double)Constant.Pi);
                Assert.IsTrue(Fun.ApproximateEquals(x, angles[i], 0.0001),
                        "Solution does not match reference {0} != {1}", x, angles[i]);
            }
        }
    }

}