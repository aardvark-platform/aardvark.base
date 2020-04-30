using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp30)]
    [DisassemblyDiagnoser(printAsm: true)]
    public class TransformV3d
    {
        V3d[] arr = new V3d[100000];

        M44d trafo = M44d.RotationX(Constant.Pi);
        Euclidean3d euclidean = Euclidean3d.RotationX(Constant.Pi);
        Similarity3d similarity = Similarity3d.RotationX(Constant.Pi);
        Affine3d affine = Affine3d.RotationX(Constant.Pi);

        [Benchmark]
        public void Trafo_Transform()
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
