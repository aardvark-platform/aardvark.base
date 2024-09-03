namespace Aardvark.Base.FSharp.Benchmarks

open Aardvark.Base
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Order
open System

//BenchmarkDotNet v0.13.12, Windows 10 (10.0.19045.4780/22H2/2022Update)
//AMD Ryzen 9 7900, 1 CPU, 24 logical and 12 physical cores
//.NET SDK 8.0.400
//  [Host] : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI DEBUG

//Job=ShortRun  Toolchain=InProcessEmitToolchain  IterationCount=3
//LaunchCount=1  WarmupCount=3

//| Method    | Count | Mean        | Error       | StdDev     | Gen0   | Allocated |
//|---------- |------ |------------:|------------:|-----------:|-------:|----------:|
//| Enumerate | 1     |    25.37 ns |    11.08 ns |   0.607 ns | 0.0052 |      88 B |
//| Enumerate | 10    |   125.34 ns |    30.77 ns |   1.686 ns | 0.0052 |      88 B |
//| Enumerate | 100   | 1,033.09 ns |   289.20 ns |  15.852 ns | 0.0038 |      88 B |
//| Enumerate | 1000  | 9,871.15 ns | 1,857.34 ns | 101.807 ns |      - |      88 B |

// struct enumerator
//| Method    | Count | Mean         | Error       | StdDev    | Allocated |
//|---------- |------ |-------------:|------------:|----------:|----------:|
//| Enumerate | 1     |     2.789 ns |   0.0885 ns | 0.0049 ns |         - |
//| Enumerate | 10    |    17.053 ns |   0.3215 ns | 0.0176 ns |         - |
//| Enumerate | 100   |   154.362 ns |  10.5647 ns | 0.5791 ns |         - |
//| Enumerate | 1000  | 1,560.785 ns | 122.1086 ns | 6.6932 ns |         - |

// struct enumerator + struct tuple store
//| Method    | Count | Mean         | Error      | StdDev    | Allocated |
//|---------- |------ |-------------:|-----------:|----------:|----------:|
//| Enumerate | 1     |     2.738 ns |  0.4081 ns | 0.0224 ns |         - |
//| Enumerate | 10    |    17.059 ns |  0.9612 ns | 0.0527 ns |         - |
//| Enumerate | 100   |   155.314 ns |  9.7062 ns | 0.5320 ns |         - |
//| Enumerate | 1000  | 1,560.410 ns | 92.5061 ns | 5.0706 ns |         - |


// value tuple
//| Method | Count | Mean           | Error            | StdDev         | Gen0   | Gen1   | Allocated |
//|------- |------ |---------------:|-----------------:|---------------:|-------:|-------:|----------:|
//| AddNew | 1     |     364.166 ns |     2,432.966 ns |    133.3591 ns | 0.0048 | 0.0043 |      80 B |
//| AddNew | 10    |   3,000.746 ns |     3,364.066 ns |    184.3958 ns | 0.0458 | 0.0420 |     800 B |
//| AddNew | 100   |  36,692.566 ns |   152,969.685 ns |  8,384.7846 ns | 0.4578 | 0.4272 |    8000 B |
//| AddNew | 1000  | 359,488.420 ns | 1,355,066.252 ns | 74,275.7537 ns | 4.6387 | 4.3945 |   80000 B |
//| AddRef | 1     |       7.813 ns |         3.144 ns |      0.1723 ns |      - |      - |         - |
//| AddRef | 10    |      66.662 ns |         2.616 ns |      0.1434 ns |      - |      - |         - |
//| AddRef | 100   |     672.546 ns |        22.958 ns |      1.2584 ns |      - |      - |         - |
//| AddRef | 1000  |   6,932.414 ns |       237.903 ns |     13.0403 ns |      - |      - |         - |

// struct tuple
//| Method | Count | Mean           | Error             | StdDev         | Median         | Gen0   | Gen1   | Allocated |
//|------- |------ |---------------:|------------------:|---------------:|---------------:|-------:|-------:|----------:|
//| AddNew | 1     |     315.227 ns |     1,498.7478 ns |     82.1514 ns |     279.821 ns | 0.0029 | 0.0024 |      48 B |
//| AddNew | 10    |   2,816.089 ns |     3,086.4605 ns |    169.1793 ns |   2,846.089 ns | 0.0267 | 0.0229 |     480 B |
//| AddNew | 100   |  40,502.010 ns |   333,244.1749 ns | 18,266.2377 ns |  31,060.260 ns | 0.2747 | 0.2441 |    4800 B |
//| AddNew | 1000  | 304,053.206 ns | 1,264,386.0962 ns | 69,305.2684 ns | 265,995.361 ns | 2.4414 | 1.9531 |   48000 B |
//| AddRef | 1     |       7.152 ns |         0.3564 ns |      0.0195 ns |       7.162 ns |      - |      - |         - |
//| AddRef | 10    |      66.858 ns |         2.1365 ns |      0.1171 ns |      66.877 ns |      - |      - |         - |
//| AddRef | 100   |     664.219 ns |        24.2442 ns |      1.3289 ns |     663.463 ns |      - |      - |         - |
//| AddRef | 1000  |   6,797.345 ns |       158.1447 ns |      8.6684 ns |   6,795.523 ns |      - |      - |         - |

[<MemoryDiagnoser>]
[<Orderer(BenchmarkDotNet.Order.SummaryOrderPolicy.Method)>]
type RefCountingSet() =
    let mutable stuff : Object[] = Unchecked.defaultof<_>
    let mutable refSet : ReferenceCountingSet<Object> = Unchecked.defaultof<_>
    let mutable cnt = 0

    [<Params(1, 10, 100, 1000)>]
    member x.Count
        with get() = cnt
        and set v = cnt <- v
    
    [<GlobalSetup>]
    member x.Setup() =
        stuff <- Array.init x.Count  (fun i -> Object())
        refSet <- ReferenceCountingSet<Object>(stuff)

    //[<Benchmark>]
    //member x.Enumerate() : int =
    //    let mutable xx = 0
    //    for x in refSet do
    //        xx <- xx ^^^ x.GetHashCode()
    //    xx

    [<Benchmark>]
    member x.AddRef() =
        for x in stuff do
            refSet.Add x |> ignore

    [<Benchmark>]
    [<IterationCount(3)>]
    member x.AddNew() =
        for i in 1..(x.Count) do
            refSet.Add (Object()) |> ignore