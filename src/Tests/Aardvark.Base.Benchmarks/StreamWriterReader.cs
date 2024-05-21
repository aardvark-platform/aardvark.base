using Aardvark.Base.Coder;
using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aardvark.Base.Benchmarks
{
    //BenchmarkDotNet v0.13.9+228a464e8be6c580ad9408e98f18813f6407fb5a, Windows 10 (10.0.19045.4291/22H2/2022Update)
    //Intel Core i7-8700K CPU 3.70GHz(Coffee Lake), 1 CPU, 12 logical and 6 physical cores
    //.NET SDK 6.0.421
    //  [Host] : .NET 6.0.29 (6.0.2924.17105), X64 RyuJIT AVX2

    //Job = InProcess  Toolchain=InProcessEmitToolchain

    //| Method     | Size | Mean      | Error    | StdDev   | Allocated |
    //|----------- |----- |----------:|---------:|---------:|----------:|
    //| WriteArray | 10   |  39.21 ns | 0.062 ns | 0.048 ns |         - |
    //| ReadArray  | 10   |  12.73 ns | 0.098 ns | 0.092 ns |         - |
    //| WriteArray | 100  |  54.25 ns | 0.259 ns | 0.229 ns |         - |
    //| ReadArray  | 100  |  17.63 ns | 0.129 ns | 0.114 ns |         - |
    //| WriteArray | 1000 | 139.65 ns | 0.589 ns | 0.522 ns |         - |
    //| ReadArray  | 1000 |  48.70 ns | 0.275 ns | 0.258 ns |         - |

    // OLD: 
    //| Method     | Size | Mean        | Error     | StdDev    | Allocated |
    //|----------- |----- |------------:|----------:|----------:|----------:|
    //| WriteArray | 10   |    61.18 ns |  0.357 ns |  0.334 ns |         - |
    //| ReadArray  | 10   |    34.82 ns |  0.189 ns |  0.158 ns |         - |
    //| WriteArray | 100  |   316.98 ns |  0.696 ns |  0.617 ns |         - |
    //| ReadArray  | 100  |    39.43 ns |  0.132 ns |  0.110 ns |         - |
    //| WriteArray | 1000 | 2,883.07 ns | 24.201 ns | 20.209 ns |         - |
    //| ReadArray  | 1000 |    74.27 ns |  0.193 ns |  0.171 ns |         - |

    [InProcess]
    [PlainExporter, MemoryDiagnoser]
    public class StreamWriterReader
    {
        float[] _data;
        StreamCodeWriter _writer;
        StreamCodeReader _reader;

        [Params(10, 100, 1000)]
        public int Size;

        [GlobalSetup]
        public void GlobalSetup()
        {
            _data = new float[Size].SetByIndex(i => i);
            _writer = new StreamCodeWriter(new MemoryStream(_data.Length * 4));

            var tmpWriter = new StreamCodeWriter(new MemoryStream(_data.Length * 4));
            tmpWriter.WriteArray(_data, 0, _data.Length);
            _reader = new StreamCodeReader(tmpWriter.BaseStream);
        }

        [Benchmark]
        public void WriteArray()
        {
            _writer.BaseStream.Position = 0;
            _writer.WriteArray(_data, 0, _data.Length);
        }

        [Benchmark]
        public void ReadArray()
        {
            _reader.BaseStream.Position = 0;
            _reader.ReadArray(_data, 0, _data.Length);
        }
    }
}
