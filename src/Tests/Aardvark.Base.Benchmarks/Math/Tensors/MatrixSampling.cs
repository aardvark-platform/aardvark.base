using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    //BenchmarkDotNet=v0.12.0, OS=Windows 10.0.19041
    //Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
    //.NET Core SDK = 5.0.100

    // [Host]     : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT
    // DefaultJob : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT

    //|                  Method |     Mean |    Error |   StdDev |    Gen 0 | Gen 1 | Gen 2 | Allocated |
    //|------------------------ |---------:|---------:|---------:|---------:|------:|------:|----------:|
    //|          Sample4Clamped | 641.1 us | 12.65 us | 18.53 us | 203.1250 |     - |     - |   1.22 MB |
    //|    Sample4Clamped (Old) | 140.0 us |  0.46 us |  0.43 us |        - |     - |     - |       1 B |
    //| Sample4Clamped (ValTup) | 140.5 us |  0.86 us |  0.72 us |        - |     - |     - |       1 B |

    [PlainExporter, MemoryDiagnoser]
    public class MatrixSampling
    {
        public Matrix<float> m_mat;

        [GlobalSetup]
        public void GlobalSetup()
        {
            m_mat = new Matrix<float>(100, 100, 1.0f);
        }

[Benchmark]
public float Sample4Clamped()
{
    var sum = 0f;
    for(int x = 0; x < 100; x++)
        for (int y = 0; y < 100; y++)
            sum += m_mat.Sample4Clamped(x, x, (t, a, b) => Fun.Lerp((float)t, a, b), (t, a, b) => Fun.Lerp((float)t, a, b));
    return sum;
}
    }
}
