namespace Aardvark.Base.FSharp.Benchmarks

open Aardvark.Base
open BenchmarkDotNet.Attributes;

module List =
    let zip' (x : 'T1 list) (y : 'T2 list) =
        List.map2 (fun a b -> struct (a, b)) x y

module Array =
    let zip' (x : 'T1[]) (y : 'T2[]) =
        Array.map2 (fun a b -> struct (a, b)) x y


//# var types = new[] { "Short", "Float", "Double", "V2d", "V4d", "M44d", "Trafo3d" };
//# var generators =
//#     new[] {
//#         "fun () -> int16 <| rnd.UniformInt()",
//#         "rnd.UniformFloat",
//#         "rnd.UniformDouble",
//#         "rnd.UniformV2d",
//#         "rnd.UniformV4d",
//#         "rnd.UniformM44d",
//#         "fun () -> Trafo3d(rnd.UniformM44d(), rnd.UniformM44d())"
//#     };
//#
//# var collections = new[] { "List", "Array" };
//# for(int i = 0; i < types.Length; i++) {
    //# var t = types[i];
    //# var gen = generators[i];
    //#
    //# foreach (var c in collections) {
[<MemoryDiagnoser>]
type Zip__t____c__s() =
    let mutable dataA = __c__.empty
    let mutable dataB = __c__.empty

    [<DefaultValue; Params(10, 1000, 100000)>]
    val mutable Count : int

    [<GlobalSetup>]
    member x.Init() =
        let rnd = RandomSystem(0)
        dataA <- __c__.init x.Count (ignore >> __gen__)
        dataB <- __c__.init x.Count (ignore >> __gen__)

    [<Benchmark(Baseline = true)>]
    member x.Zip() =
        __c__.zip dataA dataB

    [<Benchmark>]
    member x.ZipValue() =
        __c__.zip' dataA dataB


//# } }