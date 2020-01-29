using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Aardvark.Base.Benchmarks
{
    // AUTO GENERATED CODE - DO NOT CHANGE!

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class AngleBetweenFloat
    {
        const int count = 10000000;
        readonly V3f[] A = new V3f[count];
        readonly V3f[] B = new V3f[count];
        readonly float[] angles = new float[count];

        public AngleBetweenFloat()
        {
            var rnd = new RandomSystem(1);
            A.SetByIndex(i => rnd.UniformV3fDirection());
            angles.SetByIndex(i => rnd.UniformFloat() * (float)Constant.Pi);
            B.SetByIndex(i =>
            {
                V3f v;
                do { v = rnd.UniformV3fDirection(); } 
                while (v.Dot(A[i]).IsTiny());

                V3f axis = v.Cross(A[i]).Normalized;

                return M44f.Rotation(axis, angles[i]).TransformDir(A[i]);
            });
        }

        private double[] ComputeRelativeError(Func<V3f, V3f, float> f)
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
            var methods = new Dictionary<string, Func<V3f, V3f, float>>()
                {
                    { "Stable", VecFun.AngleBetween },
                    { "Fast", VecFun.AngleBetweenFast }
                };

            Console.WriteLine("Benchmarking numerical stability for AngleBetweenFloat");
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
        public float AngleBetweenStable()
        {
            float sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += A[i].AngleBetween(B[i]);
            }

            return sum;
        }

        [Benchmark]
        public float AngleBetweenFast()
        {
            float sum = 0;

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

                Assert.IsTrue(Fun.ApproximateEquals(x, y, 0.01), "{0} != {1}", x, y);
            }
        }

        [Test]
        public void AngleBetweenFastTest()
        {
            for (int i = 0; i < count; i++)
            {
                var x = A[i].AngleBetweenFast(B[i]);
                var y = angles[i];

                Assert.IsTrue(Fun.ApproximateEquals(x, y, 0.01), "{0} != {1}", x, y);
            }
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    // Uncomment following lines for assembly output, need to add
    //     <DebugType>pdbonly</DebugType>
    //     <DebugSymbols>true</DebugSymbols>
    // to Aardvark.Base.csproj for Release configuration.
    // [DisassemblyDiagnoser(printAsm: true, printSource: true)]
    public class AngleBetweenDouble
    {
        const int count = 10000000;
        readonly V3d[] A = new V3d[count];
        readonly V3d[] B = new V3d[count];
        readonly double[] angles = new double[count];

        public AngleBetweenDouble()
        {
            var rnd = new RandomSystem(1);
            A.SetByIndex(i => rnd.UniformV3dDirection());
            angles.SetByIndex(i => rnd.UniformDouble() * (double)Constant.Pi);
            B.SetByIndex(i =>
            {
                V3d v;
                do { v = rnd.UniformV3dDirection(); } 
                while (v.Dot(A[i]).IsTiny());

                V3d axis = v.Cross(A[i]).Normalized;

                return M44d.Rotation(axis, angles[i]).TransformDir(A[i]);
            });
        }

        private double[] ComputeRelativeError(Func<V3d, V3d, double> f)
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
            var methods = new Dictionary<string, Func<V3d, V3d, double>>()
                {
                    { "Stable", VecFun.AngleBetween },
                    { "Fast", VecFun.AngleBetweenFast }
                };

            Console.WriteLine("Benchmarking numerical stability for AngleBetweenDouble");
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
        public double AngleBetweenStable()
        {
            double sum = 0;

            for (int i = 0; i < count; i++)
            {
                sum += A[i].AngleBetween(B[i]);
            }

            return sum;
        }

        [Benchmark]
        public double AngleBetweenFast()
        {
            double sum = 0;

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

                Assert.IsTrue(Fun.ApproximateEquals(x, y, 0.0001), "{0} != {1}", x, y);
            }
        }

        [Test]
        public void AngleBetweenFastTest()
        {
            for (int i = 0; i < count; i++)
            {
                var x = A[i].AngleBetweenFast(B[i]);
                var y = angles[i];

                Assert.IsTrue(Fun.ApproximateEquals(x, y, 0.0001), "{0} != {1}", x, y);
            }
        }
    }

}