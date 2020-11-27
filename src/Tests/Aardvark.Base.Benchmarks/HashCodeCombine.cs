using BenchmarkDotNet.Attributes;
using System;
using Aardvark.Base;

// NOTE: A namespace with Aardvark.Base.XX automatically allows using Aardvark.Base methods and somehow also prefers 
//       the Aardvark.Base.HashCode combine and does not result in error CS0104: 
//       'HashCode' is an ambiguous reference between 'Aardvark.Base.HashCode' and 'System.HashCode'
namespace SomeApp
//namespace Aardvark.Base.Benchmarks
{
    //BenchmarkDotNet=v0.12.0, OS=Windows 10.0.19041
    //Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
    //.NET Core SDK = 5.0.100

    // [Host]     : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT
    //  DefaultJob : .NET Core 3.1.9 (CoreCLR 4.700.20.47201, CoreFX 4.700.20.47203), X64 RyuJIT

    //|                  Method | Count |     Mean |    Error |   StdDev |   Gen 0 | Gen 1 | Gen 2 | Allocated |
    //|------------------------ |------ |---------:|---------:|---------:|--------:|------:|------:|----------:|
    //|       Combine2_Aardvark | 10000 | 14.88 us | 0.075 us | 0.070 us |       - |     - |     - |         - |
    //|         Combine2_System | 10000 | 20.48 us | 0.249 us | 0.233 us |       - |     - |     - |         - |
    //|       Combine3_Aardvark | 10000 | 25.43 us | 0.471 us | 0.462 us | 16.9983 |     - |     - |  106656 B |
    //| Combine3_Aardvark (NEW) | 10000 | 13.83 us | 0.157 us | 0.147 us |       - |     - |     - |         - |
    //|         Combine3_System | 10000 | 17.73 us | 0.096 us | 0.090 us |       - |     - |     - |         - |
    //|       Combine4_Aardvark | 10000 | 22.78 us | 0.209 us | 0.195 us | 12.7258 |     - |     - |   80000 B |
    //| Combine4_Aardvark (NEW) | 10000 | 13.27 us | 0.076 us | 0.068 us |       - |     - |     - |         - |
    //|         Combine4_System | 10000 | 17.97 us | 0.303 us | 0.284 us |       - |     - |     - |         - |
    //|       Combine8_Aardvark | 10000 | 21.55 us | 0.243 us | 0.203 us |  9.5520 |     - |     - |   60000 B |
    //| Combine8_Aardvark (NEW) | 10000 | 13.32 us | 0.114 us | 0.101 us |       - |     - |     - |         - |
    //|         Combine8_System | 10000 | 16.31 us | 0.061 us | 0.054 us |       - |     - |     - |         - |

    //|                          Method | Count |        Mean |     Error |    StdDev |    Gen 0 | Gen 1 | Gen 2 | Allocated |
    //|-------------------------------- |------ |------------:|----------:|----------:|---------:|------:|------:|----------:|
    //|       M44d_GetHashCode_Aardvark | 10000 |   568.10 us |  7.470 us |  6.630 us | 126.9531 |     - |     - |  800001 B |
    //| M44d_GetHashCode_Aardvark (NEW) | 10000 |   388.70 us |  3.600 us |  3.370 us |        - |     - |     - |       3 B |
    //|     M44d_HashCode_SystemDefault | 10000 | 1,268.00 us |  9.000 us |  7.980 us | 228.5156 |     - |     - | 1440009 B |
    //|      M44d_HashCode_SystemCustom | 10000 |   475.20 us |  3.880 us |  3.630 us |        - |     - |     - |         - |
    //|        V4d_GetHashCode_Aardvark | 10000 |    85.05 us |  0.612 us |  0.511 us |        - |     - |     - |       1 B |
    //|      V4d_HashCode_SystemDefault | 10000 | 1,079.64 us | 11.487 us | 10.745 us |  76.1719 |     - |     - |  480009 B |
    //|       V4d_HashCode_SystemCustom | 10000 |   122.04 us |  2.286 us |  2.245 us |        - |     - |     - |         - |

    [PlainExporter, MemoryDiagnoser]
    public class HashCodeCombine
    {
        public double[] m_data;
        public M44d[] m_mat;
        public MyM44d[] m_myMat;
        public V4d[] m_vec;
        public MyV4d[] m_myVec;

        [Params(10000)]
        public int Count;

        [GlobalSetup]
        public void GlobalSetup()
        {
            m_data = new double[Count].SetByIndex(i => i + 1);
            m_mat = new M44d[Count].SetByIndex(i => new M44d(i));
            m_myMat = new MyM44d[Count].SetByIndex(i => new MyM44d());
            m_vec = new V4d[Count].SetByIndex(i => new V4d(i));
            m_myVec = new MyV4d[Count].SetByIndex(i => new MyV4d());
        }

        [Benchmark]
        public int Combine2_Aardvark()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 2;)
                sum += Aardvark.Base.HashCode.Combine(
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode());
            return sum;
        }

        //[Benchmark]
        //public int Combine2_System()
        //{
        //    int sum = 0;
        //    for (int i = 0; i <= m_data.Length - 2;)
        //        sum += System.HashCode.Combine(
        //                    m_data[i++],
        //                    m_data[i++]);
        //    return sum;
        //}

        [Benchmark]
        public int Combine3_Aardvark()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 3;)
                sum += Aardvark.Base.HashCode.Combine(
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode());
            return sum;
        }

        [Benchmark]
        public int Combine3_System()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 3;)
                sum += System.HashCode.Combine(
                            m_data[i++],
                            m_data[i++],
                            m_data[i++]);
            return sum;
        }

        [Benchmark]
        public int Combine4_Aardvark()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 4;)
                sum += Aardvark.Base.HashCode.Combine(
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode());
            return sum;
        }

        [Benchmark]
        public int Combine4_System()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 4;)
                sum += System.HashCode.Combine(
                            m_data[i++],
                            m_data[i++],
                            m_data[i++],
                            m_data[i++]);
            return sum;
        }

        [Benchmark]
        public int Combine8_Aardvark()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 8;)
                sum += Aardvark.Base.HashCode.Combine(
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode(),
                            m_data[i++].GetHashCode());
            return sum;
        }

        [Benchmark]
        public int Combine8_System()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 8;)
                sum += System.HashCode.Combine(
                            m_data[i++],
                            m_data[i++],
                            m_data[i++],
                            m_data[i++],
                            m_data[i++],
                            m_data[i++],
                            m_data[i++],
                            m_data[i++]);
            return sum;
        }

        [Benchmark]
        public int M44d_GetHashCode_Aardvark()
        {
            int sum = 0;
            for (int i = 0; i < m_mat.Length; i++)
                sum += m_mat[i].GetHashCode();
            return sum;
        }

        [Benchmark]
        public int M44d_HashCode_SystemDefault()
        {
            int sum = 0;
            for (int i = 0; i < m_myMat.Length; i++)
                sum += m_myMat[i].GetHashCode();
            return sum;
        }

        [Benchmark]
        public int M44d_HashCode_SystemCustom()
        {
            int sum = 0;
            for (int i = 0; i < m_myMat.Length; i++)
                sum += m_myMat[i].CustomHashCode();
            return sum;
        }

        [Benchmark]
        public int V4d_GetHashCode_Aardvark()
        {
            int sum = 0;
            for (int i = 0; i < m_vec.Length; i++)
                sum += m_vec[i].GetHashCode();
            return sum;
        }

        [Benchmark]
        public int V4d_HashCode_SystemDefault()
        {
            int sum = 0;
            for (int i = 0; i < m_myVec.Length; i++)
                sum += m_myVec[i].GetHashCode();
            return sum;
        }

        [Benchmark]
        public int V4d_HashCode_SystemCustom()
        {
            int sum = 0;
            for (int i = 0; i < m_myVec.Length; i++)
                sum += m_myVec[i].CustomHashCode();
            return sum;
        }

        public struct MyM44d
        {

#pragma warning disable CS0649
            double x0, x1, x2, x3, x4, x5, x6, x7, x8, x9, x10, x11, x12, x13, x14, x15;
#pragma warning restore CS0649

            public int CustomHashCode()
            {
                return System.HashCode.Combine(
                            System.HashCode.Combine(x0, x1, x2, x3, x4, x5, x6, x7),
                            System.HashCode.Combine(x8, x9, x10, x11, x12, x13, x14, x15));
            }
        }

        public struct MyV4d
        {
#pragma warning disable CS0649
            double x0, x1, x2, x3;
#pragma warning restore CS0649

            public int CustomHashCode()
            {
                return System.HashCode.Combine(x0, x1, x2, x3);
            }
        }
    }
}
