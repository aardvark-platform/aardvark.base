using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    //BenchmarkDotNet=v0.12.0, OS=Windows 10.0.18362
    //Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
    //.NET Core SDK = 3.1.201

    // [Host]     : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT
    // Job-HKXJIN : .NET Core 3.1.3 (CoreCLR 4.700.20.11803, CoreFX 4.700.20.12001), X64 RyuJIT

    //Runtime=.NET Core 3.0

    //|               Method |       Mean |   Error |  StdDev |
    //|--------------------- |-----------:|--------:|--------:|
    //|  TrafoM44d_Transform |   711.2 us | 7.49 us | 6.64 us |
    //|  Euclidean_Transform |   711.1 us | 4.77 us | 4.46 us |
    //| Similarity_Transform | 1,639.8 us | 6.79 us | 6.35 us |
    //|     Affine_Transform |   545.6 us | 3.12 us | 2.92 us |

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class TransformV3d
    {
        readonly V3d[] arr = new V3d[100000];

        M44d trafo = M44d.RotationX(Constant.Pi);
        Euclidean3d euclidean = Euclidean3d.RotationX(Constant.Pi);
        Similarity3d similarity = Similarity3d.RotationX(Constant.Pi);
        Affine3d affine = Affine3d.RotationX(Constant.Pi);

        [Benchmark]
        public void TrafoM44d_Transform()
        {
            var t = trafo;
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = t.TransformPos(arr[i]);
        }

        [Benchmark]
        public void Euclidean_Transform()
        {
            var t = euclidean;
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = t.TransformPos(arr[i]);
        }

        [Benchmark]
        public void Similarity_Transform()
        {
            var t = similarity;
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = t.TransformPos(arr[i]);
        }

        [Benchmark]
        public void Affine_Transform()
        {
            var t = affine;
            var local = arr;
            for (int i = 0; i < local.Length; i++)
                arr[i] = t.TransformPos(arr[i]);
        }
    }
}
