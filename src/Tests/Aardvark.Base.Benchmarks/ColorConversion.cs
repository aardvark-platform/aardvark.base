using BenchmarkDotNet.Attributes;
using System;
using System.Runtime.CompilerServices;

namespace Aardvark.Base.Benchmarks
{   static class LocalCol
    {
        private const float c_byteToFloat = 1.0f / 255.0f;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ByteToFloat(byte b) { return c_byteToFloat * (float)b; }

        public static readonly Func<byte, float> ByteToFloatFunc = ByteToFloat;

    }

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static C3f C3bToC3f(C3b color)
            => new C3f(
                LocalCol.ByteToFloatFunc(color.R),
                LocalCol.ByteToFloatFunc(color.G),
                LocalCol.ByteToFloatFunc(color.B)
            );

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static C3f C3bToC3fInlined(C3b color)
            => new C3f(
                LocalCol.ByteToFloat(color.R),
                LocalCol.ByteToFloat(color.G),
                LocalCol.ByteToFloat(color.B)
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


    //BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.2006 (21H2)
    //AMD Ryzen 5 1500X, 1 CPU, 8 logical and 4 physical cores
    //.NET SDK= 6.0.401
    //  [Host] : .NET Core 3.1.29 (CoreCLR 4.700.22.41602, CoreFX 4.700.22.41702), X64 RyuJIT

    //Job=InProcess Toolchain = InProcessEmitToolchain

    //|                    Method |     Mean |    Error |   StdDev | Ratio | RatioSD |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
    //|-------------------------- |---------:|---------:|---------:|------:|--------:|----------:|----------:|----------:|----------:|
    //|  'Byte to Float (Lambda)' | 690.6 ms | 13.07 ms | 12.83 ms |  1.00 |    0.00 | 2000.0000 | 2000.0000 | 2000.0000 |    400 MB |
    //| 'Byte to Float (Inlined)' | 656.8 ms | 12.83 ms | 16.68 ms |  0.95 |    0.02 | 2000.0000 | 2000.0000 | 2000.0000 |    400 MB |

    [InProcess]
    [MemoryDiagnoser]
    public class ColorVolumeConversion
    {
        private static class Volume
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Volume<float> ToFloatColor(Volume<byte> volume)
                => volume.MapToImageWindow(LocalCol.ByteToFloat);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static Volume<float> ToFloatColorFunc(Volume<byte> volume)
                => volume.MapToImageWindow(LocalCol.ByteToFloatFunc);

        }

        Volume<byte>[] volumes;

        [GlobalSetup]
        public void Init()
        {
            var rnd = new RandomSystem(0);
            var size = new V3i(64, 128, 128);

            volumes =
                new Volume<byte>[100].SetByIndex(i =>
                {
                    var v = new Volume<byte>(size);
                    v.SetByIndex(j => (byte)rnd.UniformInt());
                    return v;
                });
        }

        [Benchmark(Baseline = true, Description = "Byte to Float (Lambda)")]
        public float ByteToFloatFunc()
        {
            float sum = 0;
            foreach (var x in volumes)
            {
                var v = Volume.ToFloatColorFunc(x);
                sum += v.Data[0];
            }
            return sum;
        }

        [Benchmark(Description = "Byte to Float (Inlined)")]
        public float ByteToFloatInline()
        {
            float sum = 0;
            foreach (var x in volumes)
            {
                var v = Volume.ToFloatColor(x);
                sum += v.Data[0];
            }
            return sum;
        }
    }
}