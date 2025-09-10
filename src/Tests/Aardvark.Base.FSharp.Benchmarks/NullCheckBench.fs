namespace Aardvark.Base.FSharp.Benchmarks

open System
open BenchmarkDotNet.Attributes

// | Method              | Mean       | Error     | StdDev   | Ratio |
// |-------------------- |-----------:|----------:|---------:|------:|
// | 'F# Equality'       | 3,270.6 ns | 455.46 ns | 24.97 ns |  1.00 |
// | 'Pattern match'     |   227.1 ns |  91.41 ns |  5.01 ns |  0.07 |
// | obj.ReferenceEquals |   218.3 ns |  63.45 ns |  3.48 ns |  0.07 |

type ``Null check``() =

    [<DefaultValue>]
    val mutable Data : obj[]

    [<GlobalSetup>]
    member this.Setup() =
        let rnd = Random 0
        this.Data <- Array.init 512 (fun _ -> if rnd.NextDouble() < 0.5 then obj() else null)

    [<Benchmark(Description = "F# Equality", Baseline = true)>]
    member this.FSharpEquality() =
        let mutable count = 0
        for obj in this.Data do if obj = null then count <- count + 1
        count

    [<Benchmark(Description = "Pattern match")>]
    member this.Match() =
        let mutable count = 0
        for obj in this.Data do match obj with null -> count <- count + 1 | _ -> ()
        count

    [<Benchmark(Description = "obj.ReferenceEquals")>]
    member this.ReferenceEquality() =
        let mutable count = 0
        for obj in this.Data do if Object.ReferenceEquals(obj, null) then count <- count + 1
        count