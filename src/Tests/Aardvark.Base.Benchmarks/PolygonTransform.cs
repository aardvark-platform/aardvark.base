using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    //BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
    //Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
    //.NET Core SDK = 5.0.201

    // [Host]     : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT
    //  DefaultJob : .NET Core 3.1.13 (CoreCLR 4.700.21.11102, CoreFX 4.700.21.11602), X64 RyuJIT

    //|             Method | Count |      Mean |    Error |   StdDev |  Gen 0 |  Gen 1 | Gen 2 | Allocated |
    //|------------------- |------ |----------:|---------:|---------:|-------:|-------:|------:|----------:|
    //|    TransformMatrix |     5 |  40.59 ns | 0.301 ns | 0.281 ns | 0.0166 |      - |     - |     104 B |
    //| TransformMatrixNew |     5 |  60.41 ns | 0.307 ns | 0.287 ns | 0.0408 |      - |     - |     256 B |
    //|  TransformShiftMap |     5 |  31.36 ns | 0.113 ns | 0.094 ns | 0.0318 |      - |     - |     200 B |
    //|  TransformShiftNew |     5 |  16.92 ns | 0.144 ns | 0.128 ns | 0.0166 |      - |     - |     104 B |
    //|    TransformMatrix |    20 | 128.61 ns | 0.418 ns | 0.391 ns | 0.0548 |      - |     - |     344 B |
    //| TransformMatrixNew |    20 | 180.79 ns | 0.921 ns | 0.861 ns | 0.0789 |      - |     - |     496 B |
    //|  TransformShiftMap |    20 |  71.36 ns | 0.551 ns | 0.515 ns | 0.0701 | 0.0001 |     - |     440 B |
    //|  TransformShiftNew |    20 |  43.27 ns | 0.820 ns | 0.767 ns | 0.0548 | 0.0001 |     - |     344 B |
    //|    TransformMatrix |   100 | 531.93 ns | 1.654 ns | 1.466 ns | 0.2584 | 0.0010 |     - |    1624 B |
    //| TransformMatrixNew |   100 | 834.72 ns | 3.112 ns | 2.911 ns | 0.2823 | 0.0010 |     - |    1776 B |
    //|  TransformShiftMap |   100 | 370.42 ns | 1.636 ns | 1.530 ns | 0.2737 | 0.0014 |     - |    1720 B |
    //|  TransformShiftNew |   100 | 198.36 ns | 0.890 ns | 0.833 ns | 0.2587 | 0.0014 |     - |    1624 B |

    [PlainExporter, MemoryDiagnoser]
    public class PolygonTransform
    {
        public V2d offset = V2d.Zero;
        public Polygon2d polyArray = new Polygon2d(new V2d[5].SetByIndex(i => new V2d(i)));

        [Params(5, 20, 100)]
        public int Count;

        [GlobalSetup]
        public void GlobalSetup()
        {
            polyArray = new Polygon2d(new V2d[Count].SetByIndex(i => new V2d(i)));
        }

        [Benchmark]
        public Polygon2d TransformMatrix()
        {
            return polyArray.Transformed(M33d.Translation(offset));
        }

        [Benchmark]
        public Polygon2d TransformMatrixNew()
        {
            var m = M33d.Translation(offset);
            return polyArray.Map(v => m.TransformPos(v));
        }

        [Benchmark]
        public Polygon2d TransformShiftMap()
        {
            var o = offset;
            return polyArray.Map(v => v + o);
        }

        [Benchmark]
        public Polygon2d TransformShiftNew()
        {
            return polyArray.Transformed(new Shift2d(offset));
        }
    }
}
