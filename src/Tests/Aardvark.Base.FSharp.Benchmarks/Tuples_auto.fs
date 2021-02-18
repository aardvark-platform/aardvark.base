namespace Aardvark.Base.FSharp.Benchmarks

open Aardvark.Base
open BenchmarkDotNet.Attributes;

module List =
    let zip' (x : 'T1 list) (y : 'T2 list) =
        List.map2 (fun a b -> struct (a, b)) x y

module Array =
    let zip' (x : 'T1[]) (y : 'T2[]) =
        Array.map2 (fun a b -> struct (a, b)) x y


[<MemoryDiagnoser>]
type ZipShortLists() =
    let mutable dataA = List.empty
    let mutable dataB = List.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- List.init x.Count (ignore >> fun () -> int16 <| rnd.UniformInt())
        dataB <- List.init x.Count (ignore >> fun () -> int16 <| rnd.UniformInt())

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        List.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        List.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipShortArrays() =
    let mutable dataA = Array.empty
    let mutable dataB = Array.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- Array.init x.Count (ignore >> fun () -> int16 <| rnd.UniformInt())
        dataB <- Array.init x.Count (ignore >> fun () -> int16 <| rnd.UniformInt())

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        Array.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        Array.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipFloatLists() =
    let mutable dataA = List.empty
    let mutable dataB = List.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- List.init x.Count (ignore >> rnd.UniformFloat)
        dataB <- List.init x.Count (ignore >> rnd.UniformFloat)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        List.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        List.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipFloatArrays() =
    let mutable dataA = Array.empty
    let mutable dataB = Array.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- Array.init x.Count (ignore >> rnd.UniformFloat)
        dataB <- Array.init x.Count (ignore >> rnd.UniformFloat)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        Array.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        Array.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipDoubleLists() =
    let mutable dataA = List.empty
    let mutable dataB = List.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- List.init x.Count (ignore >> rnd.UniformDouble)
        dataB <- List.init x.Count (ignore >> rnd.UniformDouble)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        List.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        List.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipDoubleArrays() =
    let mutable dataA = Array.empty
    let mutable dataB = Array.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- Array.init x.Count (ignore >> rnd.UniformDouble)
        dataB <- Array.init x.Count (ignore >> rnd.UniformDouble)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        Array.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        Array.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipV2dLists() =
    let mutable dataA = List.empty
    let mutable dataB = List.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- List.init x.Count (ignore >> rnd.UniformV2d)
        dataB <- List.init x.Count (ignore >> rnd.UniformV2d)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        List.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        List.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipV2dArrays() =
    let mutable dataA = Array.empty
    let mutable dataB = Array.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- Array.init x.Count (ignore >> rnd.UniformV2d)
        dataB <- Array.init x.Count (ignore >> rnd.UniformV2d)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        Array.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        Array.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipV4dLists() =
    let mutable dataA = List.empty
    let mutable dataB = List.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- List.init x.Count (ignore >> rnd.UniformV4d)
        dataB <- List.init x.Count (ignore >> rnd.UniformV4d)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        List.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        List.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipV4dArrays() =
    let mutable dataA = Array.empty
    let mutable dataB = Array.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- Array.init x.Count (ignore >> rnd.UniformV4d)
        dataB <- Array.init x.Count (ignore >> rnd.UniformV4d)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        Array.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        Array.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipM44dLists() =
    let mutable dataA = List.empty
    let mutable dataB = List.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- List.init x.Count (ignore >> rnd.UniformM44d)
        dataB <- List.init x.Count (ignore >> rnd.UniformM44d)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        List.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        List.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipM44dArrays() =
    let mutable dataA = Array.empty
    let mutable dataB = Array.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- Array.init x.Count (ignore >> rnd.UniformM44d)
        dataB <- Array.init x.Count (ignore >> rnd.UniformM44d)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        Array.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        Array.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipTrafo3dLists() =
    let mutable dataA = List.empty
    let mutable dataB = List.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- List.init x.Count (ignore >> fun () -> Trafo3d(rnd.UniformM44d(), rnd.UniformM44d()))
        dataB <- List.init x.Count (ignore >> fun () -> Trafo3d(rnd.UniformM44d(), rnd.UniformM44d()))

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        List.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        List.zip' dataA dataB


[<MemoryDiagnoser>]
type ZipTrafo3dArrays() =
    let mutable dataA = Array.empty
    let mutable dataB = Array.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- Array.init x.Count (ignore >> fun () -> Trafo3d(rnd.UniformM44d(), rnd.UniformM44d()))
        dataB <- Array.init x.Count (ignore >> fun () -> Trafo3d(rnd.UniformM44d(), rnd.UniformM44d()))

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        Array.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        Array.zip' dataA dataB


