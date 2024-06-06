using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    struct StructThing
    {
        public double X, Y, Z; // adding Z prevents automatic inlining of ConstX/Y

        public static StructThing ConstXAggro { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new StructThing(1, 0, 0); }
        public static StructThing ConstYAggro { [MethodImpl(MethodImplOptions.AggressiveInlining)] get => new StructThing(0, 1, 0); }

        public static StructThing ConstX { get => new StructThing(1, 0, 0); }
        public static StructThing ConstY { get => new StructThing(0, 1, 0); }

        public static readonly StructThing ReadonlyX = new StructThing(1, 0, 0);
        public static readonly StructThing ReadonlyY = new StructThing(0, 1, 0);

        public StructThing(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public readonly StructThing DoMath(StructThing b)
        {
            return new StructThing(
                Y * b.X - X * b.Y,
                X * b.Y - Y * b.X,
                Z * b.Z);
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class StaticConstants
    {
        readonly StructThing[] arr = new StructThing[500000];

        public StaticConstants()
        {
            var rnd1 = new Random(1);
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new StructThing(rnd1.NextDouble(), rnd1.NextDouble(), rnd1.NextDouble());
        }

        [Benchmark]
        public void ReadonlyConstant()
        {
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = StructThing.ReadonlyX.DoMath(StructThing.ReadonlyY);
        }

        [Benchmark]
        public void PropertyConstant()
        {
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = StructThing.ConstX.DoMath(StructThing.ConstY);
        }

        [Benchmark]
        public void PropertyConstantAggro()
        {
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = StructThing.ConstXAggro.DoMath(StructThing.ConstYAggro);
        }
    }
}
