namespace Aardvark.Base.FSharp.Benchmarks

open Aardvark.Base
open Aardvark.Base.Geometry
open BenchmarkDotNet.Attributes

// | Method                | TreeSize | RayCount | Mean         | Error      | StdDev     | Ratio | RatioSD | Allocated | Alloc Ratio |
// |---------------------- |--------- |--------- |-------------:|-----------:|-----------:|------:|--------:|----------:|------------:|
// | Intersect_Option      | 1000     | 1000     |     2.347 ms |  0.0469 ms |  0.0461 ms |  1.00 |    0.00 |   11195 B |       1.000 |
// | Intersect_ValueOption | 1000     | 1000     |     2.374 ms |  0.0288 ms |  0.0255 ms |  1.01 |    0.02 |       3 B |       0.000 |
// |                       |          |          |              |            |            |       |         |           |             |
// | Intersect_Option      | 1000     | 100000   |   235.429 ms |  1.9680 ms |  1.6434 ms |  1.00 |    0.00 | 1143877 B |       1.000 |
// | Intersect_ValueOption | 1000     | 100000   |   232.502 ms |  3.6250 ms |  3.3908 ms |  0.99 |    0.02 |     229 B |       0.000 |
// |                       |          |          |              |            |            |       |         |           |             |
// | Intersect_Option      | 10000    | 1000     |    22.131 ms |  0.2575 ms |  0.2409 ms |  1.00 |    0.00 |   90430 B |       1.000 |
// | Intersect_ValueOption | 10000    | 1000     |    22.212 ms |  0.3387 ms |  0.2828 ms |  1.00 |    0.02 |      22 B |       0.000 |
// |                       |          |          |              |            |            |       |         |           |             |
// | Intersect_Option      | 10000    | 100000   | 2,237.003 ms | 34.4206 ms | 32.1970 ms |  1.00 |    0.00 | 8953728 B |       1.000 |
// | Intersect_ValueOption | 10000    | 100000   | 2,197.432 ms | 28.2317 ms | 26.4080 ms |  0.98 |    0.01 |     688 B |       0.000 |

[<MemoryDiagnoser>]
type BvhIntersectBenchmark() =
    let mutable bvh = BvhTree.ofArray Array.empty
    let mutable rays = [||]

    [<Params(1000, 10_000)>]
    member val TreeSize : int = 0 with get, set

    [<Params(1000, 100_000)>]
    member val RayCount : int = 0 with get, set

    [<GlobalSetup>]
    member self.Setup() =
        let rnd = RandomSystem(42)

        let objects = Array.init self.TreeSize (fun _ ->
            let c = rnd.UniformV3d() * 10.0
            let s = Sphere3d.FromCenterAndRadius(c, 0.1)
            struct (s, s.BoundingBox3d)
        )
        bvh <- BvhTree.ofArrayV objects

        rays <- Array.init self.RayCount (fun _ ->
            let origin = rnd.UniformV3d() * 10.0
            RayPart.ofRay <| FastRay3d(origin, V3d.OII)
        )

    [<Benchmark(Baseline = true)>]
    member self.Intersect_Option() =
        let mutable hits = 0

        let tryHit (ray: RayPart) (sphere: Sphere3d) =
            match RayPart.intersect ray sphere with
            | Some t -> Some <| RayHit(t, t)
            | _ -> None

        for ray in rays do
            match BvhTree.intersect tryHit ray bvh with
            | Some _ -> hits <- hits + 1
            | None -> ()

        hits

    [<Benchmark>]
    member self.Intersect_ValueOption() =
        let mutable hits = 0

        let tryHit (ray: RayPart) (sphere: Sphere3d) =
            match RayPart.intersectV ray sphere with
            | ValueSome t -> ValueSome <| RayHit(t, t)
            | _ -> ValueNone

        for ray in rays do
            match BvhTree.intersectV tryHit ray bvh with
            | ValueSome _ -> hits <- hits + 1
            | ValueNone -> ()

        hits