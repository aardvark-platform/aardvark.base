using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace Aardvark.Base.Benchmarks
{
    //BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.2006 (21H2)
    //AMD Ryzen 5 1500X, 1 CPU, 8 logical and 4 physical cores
    //.NET SDK= 6.0.401
    //  [Host] : .NET Core 3.1.29 (CoreCLR 4.700.22.41602, CoreFX 4.700.22.41702), X64 RyuJIT

    //Job=InProcess Toolchain = InProcessEmitToolchain

    //|                 Method |      Mean |     Error |    StdDev |    Median | Ratio |
    //|----------------------- |----------:|----------:|----------:|----------:|------:|
    //|  'C3b to C3f (Lambda)' | 14.354 us | 0.0366 us | 0.0305 us | 14.357 us |  1.00 |
    //| 'C3b to C3f (Inlined)' |  1.603 us | 0.0317 us | 0.0716 us |  1.570 us |  0.12 |

    [InProcess]
    public class ColorConversion
    {
        private static class LocalCol
        {
            private const float c_byteToFloat = 1.0f / 255.0f;

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static float ColByteToFloat(byte b) { return c_byteToFloat * (float)b; }

            public static readonly Func<byte, float> FloatFromByte = ColByteToFloat;

        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static C3f C3bToC3f(C3b color)
            => new C3f(
                LocalCol.FloatFromByte(color.R),
                LocalCol.FloatFromByte(color.G),
                LocalCol.FloatFromByte(color.B)
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static C3f C3bToC3fInlined(C3b color)
            => new C3f(
                LocalCol.ColByteToFloat(color.R),
                LocalCol.ColByteToFloat(color.G),
                LocalCol.ColByteToFloat(color.B)
            );


        C3b[] _cols;

        [GlobalSetup]
        public void Init()
        {
            var rnd = new RandomSystem(0);
            _cols = new C3b[1000].SetByIndex(i => rnd.UniformC3f().ToC3b());
        }

        [Benchmark(Baseline = true, Description = "C3b to C3f (Lambda)")]
        public C3f ToC3fFunc()
        {
            C3f sum = C3f.Black;
            foreach (var x in _cols)
                sum += C3bToC3f(x);
            return sum;
        }

        [Benchmark(Description = "C3b to C3f (Inlined)")]
        public C3f ToC3fInline()
        {
            C3f sum = C3f.Black;
            foreach (var x in _cols)
                sum += C3bToC3fInlined(x);
            return sum;
        }
    }
}