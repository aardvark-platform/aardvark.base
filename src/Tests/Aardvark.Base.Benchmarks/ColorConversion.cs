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


    //BenchmarkDotNet=v0.13.1, OS=Windows 10.0.19044.2006 (21H2)
    //AMD Ryzen 5 1500X, 1 CPU, 8 logical and 4 physical cores
    //.NET SDK=6.0.401
    //  [Host] : .NET Core 3.1.29 (CoreCLR 4.700.22.41602, CoreFX 4.700.22.41702), X64 RyuJIT

    //Job=InProcess  Toolchain=InProcessEmitToolchain

    //|                Method |     Mean |    Error |   StdDev | Ratio | RatioSD | Allocated |
    //|---------------------- |---------:|---------:|---------:|------:|--------:|----------:|
    //| 'C3f to HSL (Method)' | 20.95 us | 0.382 us | 0.339 us |  1.00 |    0.00 |         - |
    //| 'C3f to HSL (Lambda)' | 22.47 us | 0.091 us | 0.085 us |  1.07 |    0.02 |         - |

    [InProcess]
    [MemoryDiagnoser]
    public class ColorHSLConversion
    {
        private static class LocalCol
        {
            public static HSLf ToHSLf(C3f c)
            {
                double cr = 0.0, h6 = 0.0, l = 0.0;
                if (c.R > c.G)
                {
                    if (c.R > c.B)
                    {
                        if (c.G > c.B) { cr = c.R - c.B; l = 0.5 * (c.R + c.B); }  // R > G > B
                        else { cr = c.R - c.G; l = 0.5 * (c.R + c.G); }  // R > B >= G
                        h6 = 0 + (c.G - c.B) / cr;
                    }
                    else                                                            // B >= R > G
                    {
                        cr = c.B - c.G; l = 0.5 * (c.B + c.G);
                        h6 = 4 + (c.R - c.G) / cr;
                    }
                }
                else                                                                // G >= R
                {
                    if (c.G > c.B)
                    {
                        if (c.R > c.B) { cr = c.G - c.B; l = 0.5 * (c.G + c.B); }  // G >= R > B
                        else { cr = c.G - c.R; l = 0.5 * (c.G + c.R); }  // G > B >= R
                        h6 = 2 + (c.B - c.R) / cr;
                    }
                    else                                                            // B >= G >= R
                    {
                        cr = c.B - c.R; l = 0.5 * (c.B + c.R);
                        if (cr > 0.0) h6 = 4 + (c.R - c.G) / cr;
                    }
                }
                return new HSLf(Fun.Frac(h6 * 1.0 / 6.0),
                                cr > Constant<float>.PositiveTinyValue
                                    ? cr / (1.0 - Fun.Abs(2.0 * l - 1.0)) : 0.0,
                                l);
            }

            public static readonly Func<C3f, HSLf> HSLfFromC3f = ToHSLf;
        }

        C3f[] _cols;

        [GlobalSetup]
        public void Init()
        {
            var rnd = new RandomSystem(0);
            _cols = new C3f[1000].SetByIndex(i => rnd.UniformC3f());
        }

        [Benchmark(Baseline = true, Description = "C3f to HSL (Method)")]
        public HSLf C3fToHSLFunc()
        {
            HSLf sum = new HSLf();
            foreach (var c in _cols)
            {
                var h = LocalCol.ToHSLf(c);
                sum = new HSLf(sum.H + h.H, sum.S + h.S, sum.L + h.L);
            }
            return sum;
        }

        [Benchmark(Description = "C3f to HSL (Lambda)")]
        public HSLf C3fToHSL()
        {
            HSLf sum = new HSLf();
            foreach (var c in _cols)
            {
                var h = LocalCol.HSLfFromC3f(c);
                sum = new HSLf(sum.H + h.H, sum.S + h.S, sum.L + h.L);
            }
            return sum;
        }
    }
}