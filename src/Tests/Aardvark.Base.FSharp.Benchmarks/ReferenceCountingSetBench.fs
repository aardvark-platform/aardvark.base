namespace Aardvark.Base.FSharp.Benchmarks

open Aardvark.Base
open BenchmarkDotNet.Attributes
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

[<MemoryDiagnoser>]
type RefCountingSet() =

    let mutable refSet : ReferenceCountingSet<Object> = Unchecked.defaultof<_>
    let mutable cnt = 0

    [<Params(1, 10, 100, 1000)>]
    member x.Count
        with get() = cnt
        and set v = cnt <- v
    
    [<GlobalSetup>]
    member x.Setup() =

        let stuff = Array.init x.Count  (fun i -> Object())
        refSet <- ReferenceCountingSet<Object>(stuff)

    [<Benchmark>]
    member x.Enumerate() : int =
        let mutable xx = 0
        for x in refSet do
            xx <- xx ^^^ x.GetHashCode()
        xx