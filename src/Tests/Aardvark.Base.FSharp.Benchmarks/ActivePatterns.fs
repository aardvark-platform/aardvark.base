namespace Aardvark.Base.FSharp.Benchmarks

open Aardvark.Base
open BenchmarkDotNet.Attributes;

// BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4170/22H2/2022Update)
// Intel Core i5-4690K CPU 3.50GHz (Haswell), 1 CPU, 4 logical and 4 physical cores
// .NET SDK 8.0.202
//   [Host] : .NET 8.0.3 (8.0.324.11423), X64 RyuJIT AVX2 DEBUG
//
// Job=ShortRun  Toolchain=InProcessEmitToolchain  IterationCount=3
// LaunchCount=1  WarmupCount=3
//
// | Method                    | Count | Mean         | Error         | StdDev     | Gen0    | Allocated |
// |-------------------------- |------ |-------------:|--------------:|-----------:|--------:|----------:|
// | 'No arguments'            | 10    |     25.14 ns |      2.280 ns |   0.125 ns |  0.0382 |     120 B |
// | 'No arguments (struct)'   | 10    |     16.40 ns |      0.291 ns |   0.016 ns |       - |         - |
// | 'Tuple argument'          | 10    |     50.27 ns |     10.427 ns |   0.572 ns |  0.0765 |     240 B |
// | 'Tuple argument (struct)' | 10    |     30.82 ns |      1.718 ns |   0.094 ns |  0.0382 |     120 B |
// | 'No arguments'            | 100   |    226.12 ns |     33.508 ns |   1.837 ns |  0.3595 |    1128 B |
// | 'No arguments (struct)'   | 100   |    160.52 ns |      2.508 ns |   0.137 ns |       - |         - |
// | 'Tuple argument'          | 100   |    484.78 ns |    131.758 ns |   7.222 ns |  0.7191 |    2256 B |
// | 'Tuple argument (struct)' | 100   |    299.99 ns |     24.918 ns |   1.366 ns |  0.3595 |    1128 B |
// | 'No arguments'            | 1000  |  2,357.40 ns |  1,561.574 ns |  85.595 ns |  3.7613 |   11808 B |
// | 'No arguments (struct)'   | 1000  |  1,613.16 ns |    145.925 ns |   7.999 ns |       - |         - |
// | 'Tuple argument'          | 1000  |  6,036.04 ns |    203.476 ns |  11.153 ns |  7.5226 |   23616 B |
// | 'Tuple argument (struct)' | 1000  |  3,176.44 ns |    361.053 ns |  19.791 ns |  3.7613 |   11808 B |
// | 'No arguments'            | 10000 | 47,907.05 ns |  4,206.151 ns | 230.553 ns | 37.7808 |  118536 B |
// | 'No arguments (struct)'   | 10000 | 38,801.61 ns |    171.534 ns |   9.402 ns |       - |         - |
// | 'Tuple argument'          | 10000 | 83,620.55 ns |  6,288.851 ns | 344.713 ns | 75.5615 |  237072 B |
// | 'Tuple argument (struct)' | 10000 | 64,376.48 ns | 11,741.490 ns | 643.591 ns | 37.7197 |  118536 B |

module ActivePatternsBenchmarks =

    let inline (|Odd|_|) (value: int) =
        if value % 2 = 1 then Some ()
        else None

    [<return: Struct>]
    let inline (|OddV|_|) (value: int) =
        if value % 2 = 1 then ValueSome ()
        else ValueNone

    let inline (|OddTuple|_|) (value: int) =
        if value % 2 = 1 then Some (value, value)
        else None

    [<return: Struct>]
    let inline (|OddTupleV|_|) (value: int) =
        if value % 2 = 1 then ValueSome (value, value)
        else ValueNone

    [<MemoryDiagnoser>]
    type ``Active Patterns``() =
        let rnd = RandomSystem(0)
        let mutable data = null

        [<DefaultValue; Params(10, 100, 1000, 10000)>]
        val mutable Count : int

        [<GlobalSetup>]
        member x.Init() =
            data <- Array.init x.Count (ignore >> rnd.UniformInt)

        [<Benchmark(Description = "No arguments")>]
        member x.NoArguments() =
            let mutable cnt = 0

            for value in data do
                match value with
                | Odd -> cnt <- cnt + 1
                | _ -> ()

            cnt

        [<Benchmark(Description = "No arguments (struct)")>]
        member x.NoArgumentsStruct() =
            let mutable cnt = 0

            for value in data do
                match value with
                | OddV -> cnt <- cnt + 1
                | _ -> ()

            cnt

        [<Benchmark(Description = "Tuple argument")>]
        member x.TupleArgument() =
            let mutable sum = 0

            for value in data do
                match value with
                | OddTuple (a, b) -> sum <- a + b
                | _ -> ()

            sum

        [<Benchmark(Description = "Tuple argument (struct)")>]
        member x.TupleArgumentStruct() =
            let mutable sum = 0

            for value in data do
                match value with
                | OddTupleV (a, b) -> sum <- a + b
                | _ -> ()

            sum