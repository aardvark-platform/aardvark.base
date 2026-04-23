namespace Aardvark.Base.FSharp.Benchmarks

open System
open Aardvark.Base
open BenchmarkDotNet.Attributes

// | Method            | Size  | Density | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0    | Gen1   | Allocated | Alloc Ratio |
// |------------------ |------ |-------- |----------:|----------:|----------:|------:|--------:|--------:|-------:|----------:|------------:|
// | Choose            | 10000 | 0       |  6.635 us | 0.1194 us | 0.1058 us |  1.00 |    0.00 |       - |      - |      24 B |        1.00 |
// | ChooseV           | 10000 | 0       |  6.524 us | 0.0078 us | 0.0065 us |  0.98 |    0.02 |       - |      - |         - |        0.00 |
// | ResizeArrayChoose | 10000 | 0       |  4.733 us | 0.0099 us | 0.0092 us |  0.71 |    0.01 |  1.1902 | 0.0763 |   20056 B |      835.67 |
// |                   |       |         |           |           |           |       |         |         |        |           |             |
// | Choose            | 10000 | 0.1     | 25.416 us | 0.1203 us | 0.0939 us |  1.00 |    0.00 |  3.1738 | 0.0916 |   53392 B |        1.00 |
// | ChooseV           | 10000 | 0.1     | 11.060 us | 0.0299 us | 0.0280 us |  0.43 |    0.00 |  1.6785 | 0.0458 |   28384 B |        0.53 |
// | ResizeArrayChoose | 10000 | 0.1     |  6.715 us | 0.0361 us | 0.0338 us |  0.26 |    0.00 |  1.6937 | 0.1144 |   28408 B |        0.53 |
// |                   |       |         |           |           |           |       |         |         |        |           |             |
// | Choose            | 10000 | 0.5     | 34.344 us | 0.1547 us | 0.1292 us |  1.00 |    0.00 | 13.1836 | 1.5869 |  221376 B |        1.00 |
// | ChooseV           | 10000 | 0.5     | 11.886 us | 0.0927 us | 0.0821 us |  0.35 |    0.00 |  5.9967 | 0.4883 |  100656 B |        0.45 |
// | ResizeArrayChoose | 10000 | 0.5     | 18.194 us | 0.1907 us | 0.1690 us |  0.53 |    0.01 | 10.7422 | 2.6550 |  180360 B |        0.81 |
// |                   |       |         |           |           |           |       |         |         |        |           |             |
// | Choose            | 10000 | 0.9     | 42.948 us | 0.4031 us | 0.3366 us |  1.00 |    0.00 | 21.8506 | 3.2349 |  366232 B |        1.00 |
// | ChooseV           | 10000 | 0.9     | 12.218 us | 0.0899 us | 0.0751 us |  0.28 |    0.00 |  8.9264 |      - |  149896 B |        0.41 |
// | ResizeArrayChoose | 10000 | 0.9     | 23.243 us | 0.0698 us | 0.0619 us |  0.54 |    0.00 | 12.6343 |      - |  212232 B |        0.58 |
// |                   |       |         |           |           |           |       |         |         |        |           |             |
// | Choose            | 10000 | 1       | 42.972 us | 0.4753 us | 0.3969 us |  1.00 |    0.00 | 23.8037 | 0.0610 |  400096 B |        1.00 |
// | ChooseV           | 10000 | 1       | 10.537 us | 0.0346 us | 0.0289 us |  0.25 |    0.00 |  9.5215 |      - |  160072 B |        0.40 |
// | ResizeArrayChoose | 10000 | 1       | 20.142 us | 0.1999 us | 0.1772 us |  0.47 |    0.00 | 13.0920 | 3.2654 |  220128 B |        0.55 |

[<AutoOpen>]
module private ChooseImpl =

    module Array =
        let inline chooseV_Simple ([<InlineIfLambda>] chooser: 'T -> 'U voption) (array: 'T array) =
            let res = ResizeArray<'U>(array.Length / 4)
            for i = 0 to array.Length - 1 do
                match chooser array.[i] with
                | ValueSome b -> res.Add(b)
                | ValueNone -> ()
            res.ToArray()

[<MemoryDiagnoser>]
type ChooseBenchmark() =
    let mutable data = [||]

    [<Params(10_000)>]
    member val Size : int = 0 with get, set

    [<Params(0.0, 0.1, 0.5, 0.9, 1.0)>]
    member val Density : float = 0.0 with get, set

    [<GlobalSetup>]
    member self.Setup() =
        let r = Random(42)
        data <- Array.init self.Size (fun _ -> r.NextDouble())

    [<Benchmark(Baseline = true)>]
    member self.Choose() =
        data |> Array.choose (fun x -> if x < self.Density then Some x else None)

    [<Benchmark>]
    member self.ChooseV() =
        data |> Array.chooseV (fun x -> if x < self.Density then ValueSome x else ValueNone)

    [<Benchmark>]
    member self.ResizeArrayChoose() =
        data |> Array.chooseV_Simple (fun x -> if x < self.Density then ValueSome x else ValueNone)