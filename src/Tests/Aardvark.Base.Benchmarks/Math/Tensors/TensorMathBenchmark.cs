using Aardvark.Base.IL;
using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;
using static Aardvark.Base.Monads.Optics;
using System.Runtime.Intrinsics.X86;

namespace Aardvark.Base.Benchmarks
{
    /*
     * Benchmark indicates that inlined implementations are noticeably more efficient than
     * simply using Norm or InnerProduct and passing lambda functions. Therefore it may be wise to implement
     * common tensor operations in a dumb inline way for optimization purposes. The InlineIfLambda attribute
     * in F# might mitigate this but we are dealing with C# here.
     *
     * BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 10 (10.0.19045.4170/22H2/2022Update)
     * AMD Ryzen 5 5600X, 1 CPU, 12 logical and 6 physical cores
     * .NET SDK 6.0.420
     *   [Host] : .NET 6.0.28 (6.0.2824.12007), X64 RyuJIT AVX2
     *
     * Job = InProcess  Toolchain=InProcessEmitToolchain
     *
     * | Method                     | Mean       | Error    | StdDev   | Ratio | RatioSD | Allocated | Alloc Ratio |
     * |--------------------------- |-----------:|---------:|---------:|------:|--------:|----------:|------------:|
     * | NormSquared                | 1,377.5 us | 24.30 us | 23.87 us |  1.00 |    0.00 |       6 B |        1.00 |
     * | 'NormSquared (Predefined)' | 1,771.9 us | 35.15 us | 40.48 us |  1.28 |    0.04 |       1 B |        0.17 |
     * | 'NormSquared (Inline)'     |   340.1 us |  1.77 us |  1.48 us |  0.25 |    0.00 |         - |        0.00 |
     */

    [InProcess]
    [MemoryDiagnoser]
    public class VectorNormSquared
    {
        private static readonly Func<float, float> SquareFunc = Fun.Square;
        private static readonly Func<float, float, float> AddFunc = (x, y) => x + y;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float NormSquared(Vector<float> vector)
            => vector.Norm(x => x * x, 0.0f, (x, y) => x + y);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float NormSquaredPredefined(Vector<float> vector)
            => vector.Norm(SquareFunc, 0.0f, AddFunc);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float NormSquaredInline(Vector<float> v)
        {
            float result = 0.0f;
            long i = v.FirstIndex;
            if (v.Info.DX == 1)
            {
                long xs = v.Info.SX;
                for (long xe = i + xs; i != xe; i++)
                {
                    result += v.Data[i] * v.Data[i];
                }
            }
            else
            {
                long xs = v.Info.DSX, xj = v.Info.JX;
                for (long xe = i + xs; i != xe; i += xj)
                {
                    result += v.Data[i] * v.Data[i];
                }
            }
            return result;
        }

        private static Vector<float> GenerateVector(RandomSystem rnd)
            => Vector.Create(new float[4 + rnd.UniformInt(1000)].SetByIndex(i => rnd.UniformFloat()));

        Vector<float>[] _data;

        [GlobalSetup]
        public void Init()
        {
            var rnd = new RandomSystem(0);
            _data = new Vector<float>[1000].SetByIndex(i => GenerateVector(rnd));
        }

        [Benchmark(Baseline = true, Description = "NormSquared")]
        public float NormSquared()
        {
            float sum = 0;
            foreach (var x in _data)
                sum += NormSquared(x);
            return sum;
        }

        [Benchmark(Description = "NormSquared (Predefined)")]
        public float NormSquaredPredfined()
        {
            float sum = 0;
            foreach (var x in _data)
                sum += NormSquaredPredefined(x);
            return sum;
        }

        [Benchmark(Description = "NormSquared (Inline)")]
        public float NormSquaredInline()
        {
            float sum = 0;
            foreach (var x in _data)
                sum += NormSquaredInline(x);
            return sum;
        }
    }
}
