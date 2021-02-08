using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class MatrixMultiply
    {
        M44d[] arr44 = new M44d[100000];
        M34d[] arr34 = new M34d[100000];

        //[GlobalSetup]
        public MatrixMultiply()
        {
            arr44.SetByIndex(i => M44d.Translation(i, i, i));
            arr34.SetByIndex(i => M34d.Translation(i, i, i));
        }

        [Benchmark]
        public void M44d_Multiply()
        {
            var mat = M44d.RotationZ(1);
            var local = arr44;
            for (int i = 0; i < local.Length; i++)
                arr44[i] = mat * local[i];
        }

        [Benchmark]
        public void M34d_Multiply_M44d()
        {
            var mat = M44d.RotationZ(1);
            var local = arr34;
            for (int i = 0; i < local.Length; i++)
                local[i] = local[i] * mat;
        }

        //[Benchmark]
        //public void M44d_Multiply_M33d()
        //{
        //    var mat = M33d.Rotation(1);
        //    var local = arr44;
        //    for (int i = 0; i < local.Length; i++)
        //        local[i] = local[i] * mat;
        //}
    }
}
