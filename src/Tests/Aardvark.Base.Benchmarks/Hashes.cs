using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base.Benchmarks
{

    //BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 10 (10.0.19045.4412/22H2/2022Update)
    //Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
    //.NET SDK 6.0.422
    //  [Host]     : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2
    //  DefaultJob : .NET 6.0.30 (6.0.3024.21525), X64 RyuJIT AVX2


    //| Method     | Count | Mean       | Error     | StdDev    | Median     | Code Size | Gen0   | Allocated |
    //|----------- |------ |-----------:|----------:|----------:|-----------:|----------:|-------:|----------:|
    //| SHA1_Net48 | 16    |   788.3 ns |  10.64 ns |   9.43 ns |   788.6 ns |     262 B | 0.0629 |     400 B |
    //| SHA1_Array | 16    |   440.3 ns |   6.48 ns |   5.41 ns |   437.2 ns |     107 B | 0.0076 |      48 B |
    //| SHA1_T     | 16    |   406.7 ns |   8.08 ns |  13.04 ns |   403.7 ns |     149 B | 0.0076 |      48 B |
    //| SHA1_Net48 | 128   | 1,566.4 ns |  30.87 ns |  65.11 ns | 1,541.1 ns |     262 B | 0.0629 |     400 B |
    //| SHA1_Array | 128   | 1,090.6 ns |   9.32 ns |   8.26 ns | 1,090.7 ns |     107 B | 0.0076 |      48 B |
    //| SHA1_T     | 128   | 1,063.4 ns |  17.39 ns |  17.85 ns | 1,060.4 ns |     149 B | 0.0076 |      48 B |
    //| SHA1_Net48 | 1024  | 6,761.1 ns | 134.11 ns | 143.50 ns | 6,717.4 ns |     262 B | 0.0610 |     400 B |
    //| SHA1_Array | 1024  | 6,169.2 ns |  80.47 ns |  75.27 ns | 6,140.3 ns |     107 B | 0.0076 |      48 B |
    //| SHA1_T     | 1024  | 6,128.8 ns |  68.52 ns |  57.22 ns | 6,116.5 ns |     149 B | 0.0076 |      48 B |
    [MemoryDiagnoser, DisassemblyDiagnoser]
    public class HashBench
    {
        internal static unsafe T UseAsStream<T>(Array data, Func<UnmanagedMemoryStream, T> action)
        {
            var gc = GCHandle.Alloc(data, GCHandleType.Pinned);
            var l = data.GetType().GetElementType().GetCLRSize() * data.Length;
            try
            {
                using (var stream =
                       new UnmanagedMemoryStream(((byte*)gc.AddrOfPinnedObject())!, l, l, FileAccess.Read))
                {
                    return action(stream);
                }
            }
            finally
            {
                gc.Free();
            }
        }

        public static unsafe byte[] ComputeSHA1HashNet48(Array data)
        {
            using (var sha = SHA1.Create())
            {
                return UseAsStream(data, (stream) => sha.ComputeHash(stream));
            }
        }

        [Params(16, 128, 1024)]
        public int Count;

        float[] data;
        Array dataArr;

        [GlobalSetup]
        public void Init()
        {
            data = new float[Count];
            for (int i = 0; i < data.Length; i++)
                data[i] = Random.Shared.NextSingle();
            dataArr = data;
        }

        [Benchmark]
        public byte[] SHA1_Net48()
        {
            return ComputeSHA1HashNet48(data);
        }

        [Benchmark]
        public byte[] SHA1_Array()
        {
            return dataArr.ComputeSHA1Hash();
        }

        [Benchmark]
        public byte[] SHA1_T()
        {
            return data.ComputeSHA1Hash();
        }
    }
}
