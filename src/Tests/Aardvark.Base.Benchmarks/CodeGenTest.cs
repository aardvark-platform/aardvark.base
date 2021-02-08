using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Jobs;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.Intrinsics;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace Aardvark.Base.Benchmarks
{
    struct MyVector4
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public MyVector4(float x, float y, float z, float w)
        {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public float Length_Ref
        {
            get { return MathF.Sqrt(X * X + Y * Y + Z * Z + W * W); }
        }

        public float Length_Sse_V1
        {
            get
            {
                unsafe
                {
                    fixed (MyVector4* pthis = &this)
                    {
                        var mmx = Sse.LoadVector128((float*)pthis);
                        mmx = Sse41.DotProduct(mmx, mmx, 0xF1);
                        var l2 = mmx.GetElement(0);
                        return MathF.Sqrt(l2);
                    }
                }
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        static unsafe float Length_Sse_V2_Helper(MyVector4 vec)
        {
            var ptr = (float*)&vec;
            var mmx = Sse.LoadVector128(ptr);
            mmx = Sse41.DotProduct(mmx, mmx, 0xF1);
            var l2 = mmx.GetElement(0);
            return MathF.Sqrt(l2);
        }

        public float Length_Sse_V2
        {
            get { return Length_Sse_V2_Helper(this); }
        }

        public float Length_Sse_V3
        {
            get 
            {
                unsafe
                {
                    var vec = this;
                    var ptr = (float*)&vec;
                    var mmx = Sse.LoadVector128(ptr);
                    mmx = Sse41.DotProduct(mmx, mmx, 0xF1);
                    var l2 = mmx.GetElement(0);
                    return MathF.Sqrt(l2);
                }
            }
        }
    }

    [SimpleJob(RuntimeMoniker.NetCoreApp31)]
    [DisassemblyDiagnoser(printSource: true)]
    public class CodeGenTests
    {
        MyVector4[] arr = new MyVector4[1000000];

        //[GlobalSetup]
        public CodeGenTests()
        {
            var rnd1 = new Random(1);
            for (int i = 0; i < arr.Length; i++)
                arr[i] = new MyVector4((float)rnd1.NextDouble(), (float)rnd1.NextDouble(), (float)rnd1.NextDouble(), (float)rnd1.NextDouble());
        }

        [Benchmark]
        public float Vec4Length_Reference()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
                sum += local[i].Length_Ref;
            return sum;
        }

        [Benchmark]
        public float Vec4Length_Sse_V1()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
                sum += local[i].Length_Sse_V1;
            return sum;
        }

        [Benchmark]
        public float Vec4Length_Sse_V2()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
                sum += local[i].Length_Sse_V2;
            return sum;
        }

        [Benchmark]
        public float Vec4Length_Sse_V3()
        {
            var local = arr;
            var sum = 0.0f;
            for (int i = 0; i < local.Length; i++)
                sum += local[i].Length_Sse_V3;
            return sum;
        }
    }
}
