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

    //|            Method | Count |     Mean |    Error |   StdDev |
    //|------------------ |------ |---------:|---------:|---------:|
    //| Combine2_Aardvark | 10000 | 13.80 us | 0.037 us | 0.029 us |
    //|   Combine2_System | 10000 | 21.70 us | 0.159 us | 0.149 us |
    //| Combine3_Aardvark | 10000 | 23.77 us | 0.240 us | 0.212 us |
    //|   Combine3_System | 10000 | 18.88 us | 0.164 us | 0.153 us |
    //| Combine4_Aardvark | 10000 | 22.38 us | 0.150 us | 0.133 us |
    //|   Combine4_System | 10000 | 17.86 us | 0.032 us | 0.030 us |
    //| Combine8_Aardvark | 10000 | 21.50 us | 0.097 us | 0.091 us |
    //|   Combine8_System | 10000 | 16.55 us | 0.170 us | 0.150 us |

    public class HashCodeCombine
    {
        public double[] m_data;

        [Params(10000)]
        public int Count;

        [GlobalSetup]
        public void GlobalSetup()
        {
            m_data = new double[Count].SetByIndex(i => i + 1);
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

        [Benchmark]
        public int Combine2_System()
        {
            int sum = 0;
            for (int i = 0; i <= m_data.Length - 2;)
                sum += System.HashCode.Combine(
                            m_data[i++],
                            m_data[i++]);
            return sum;
        }

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
    }
}
