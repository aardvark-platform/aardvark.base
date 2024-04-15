namespace Aardvark.Base.FSharp.Benchmarks

open Aardvark.Base
open Aardvark.Base.FSharp.Tests
open BenchmarkDotNet.Attributes

// ================================================================================================================
// Tensor4
// ================================================================================================================

module Tensor4 =

    let inline iteri_naive ([<InlineIfLambda>] action: int64 -> unit) (t: Tensor4<'T>) =
        let s0 = t.DSX
        let j0 = t.Info.JX0
        let s1 = t.DSY
        let j1 = t.Info.JYX
        let s2 = t.DSZ
        let j2 = t.Info.JZY
        let s3 = t.DSW
        let j3 = t.Info.JWZ

        let mutable i = t.FirstIndex

        let e3 = i + s3
        while i <> e3 do
            let e2 = i + s2
            while i <> e2 do
                let e1 = i + s1
                while i <> e1 do
                    let e0 = i + s0
                    while i <> e0 do
                        action i
                        i <- i + j0
                    i <- i + j1
                i <- i + j2
            i <- i + j3

    let inline iteri2_naive ([<InlineIfLambda>] action: int64 -> int64 -> unit) (t1: Tensor4<'T1>) (t2: Tensor4<'T2>) =
        if t1.Size <> t2.Size then
            raise <| System.ArgumentException($"Mismatching Tensor4 size ({t1.Size} != {t2.Size})")

        let s0_1 = t1.DSX
        let j0_1 = t1.Info.JX0
        let j0_2 = t2.Info.JX0
        let s1_1 = t1.DSY
        let j1_1 = t1.Info.JYX
        let j1_2 = t2.Info.JYX
        let s2_1 = t1.DSZ
        let j2_1 = t1.Info.JZY
        let j2_2 = t2.Info.JZY
        let s3_1 = t1.DSW
        let j3_1 = t1.Info.JWZ
        let j3_2 = t2.Info.JWZ

        let mutable i1 = t1.FirstIndex
        let mutable i2 = t2.FirstIndex

        let e3_1 = i1 + s3_1
        while i1 <> e3_1 do
            let e2_1 = i1 + s2_1
            while i1 <> e2_1 do
                let e1_1 = i1 + s1_1
                while i1 <> e1_1 do
                    let e0_1 = i1 + s0_1
                    while i1 <> e0_1 do
                        action i1 i2
                        i1 <- i1 + j0_1
                        i2 <- i2 + j0_2
                    i1 <- i1 + j1_1
                    i2 <- i2 + j1_2
                i1 <- i1 + j2_1
                i2 <- i2 + j2_2
            i1 <- i1 + j3_1
            i2 <- i2 + j3_2

[<AbstractClass>]
type Tensor4Benchmark() =

    member _.Sizes = [
        V4i(4, 4, 4, 4)
        V4i(64, 32, 4, 3)
        V4i(4, 128, 1, 256)
        V4i(256, 256, 4, 3)
        V4i(32, 32, 32, 32)
    ]

    [<DefaultValue>]
    val mutable Tensor : Tensor4<float>

    [<DefaultValue; ParamsSource("Sizes", Priority = 0)>]
    val mutable Size : V4i

    [<DefaultValue; Params(TensorKind.Full, TensorKind.Sub, Priority = 1)>]
    val mutable Kind : TensorKind

    [<DefaultValue; Params(false, true, Priority = 3)>]
    val mutable InnerW : bool

    abstract member Setup : unit -> unit

    [<GlobalSetup>]
    default x.Setup() =
        TensorGeneration.init 0
        x.Tensor <- TensorGeneration.getTensor4OfSize x.Kind x.InnerW x.Size

[<AbstractClass>]
type Tensor4Benchmark2() =
    inherit Tensor4Benchmark()

    [<DefaultValue>]
    val mutable Tensor2 : Tensor4<float>

    [<DefaultValue; Params(TensorKind.Full, TensorKind.Sub, Priority = 2)>]
    val mutable Kind2 : TensorKind

    [<DefaultValue; Params(false, true, Priority = 4)>]
    val mutable InnerW2 : bool

    override x.Setup() =
        base.Setup()
        x.Tensor2 <- TensorGeneration.getTensor4OfSize x.Kind2 x.InnerW2 x.Size

[<MemoryDiagnoser>]
type Tensor4_Iter() =
    inherit Tensor4Benchmark()

    [<Benchmark(Description = "Tensor4<'T>.ForeachIndex()")>]
    member x.CSharp() =
        let t = x.Tensor
        let mutable result = 0.0
        t.ForeachIndex(fun i -> result <- result + t.Data.[int i])
        result

    [<Benchmark(Description = "Tensor4.iter (naive)", Baseline = true)>]
    member x.FSharpNaive() =
        let t = x.Tensor
        let mutable result = 0.0
        t |> Tensor4.iteri_naive (fun i -> result <- result + t.Data.[int i])
        result

    [<Benchmark(Description = "Tensor4.iter (optimized)")>]
    member x.FSharp() =
        let t = x.Tensor
        let mutable result = 0.0
        t |> Tensor4.iteri (fun i -> result <- result + t.Data.[int i])
        result

[<MemoryDiagnoser>]
type Tensor4_Iter2() =
    inherit Tensor4Benchmark2()

    [<Benchmark(Description = "Tensor4<'T>.ForeachIndex()")>]
    member x.CSharp() =
        let t1 = x.Tensor
        let t2 = x.Tensor2
        let mutable result = 0.0
        t1.ForeachIndex(t2.Info, fun i1 i2 -> result <- result + t1.Data.[int i1] + t2.Data.[int i2])
        result

    [<Benchmark(Description = "Tensor4.iter2 (naive)", Baseline = true)>]
    member x.FSharpNaive() =
        let t1 = x.Tensor
        let t2 = x.Tensor2
        let mutable result = 0.0
        (t1, t2) ||> Tensor4.iteri2_naive (fun i1 i2 -> result <- result + t1.Data.[int i1] + t2.Data.[int i2])
        result

    [<Benchmark(Description = "Tensor4.iter2 (optimized)")>]
    member x.FSharp() =
        let t1 = x.Tensor
        let t2 = x.Tensor2
        let mutable result = 0.0
        (t1, t2) ||> Tensor4.iteri2 (fun i1 i2 -> result <- result + t1.Data.[int i1] + t2.Data.[int i2])
        result