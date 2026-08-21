namespace Aardvark.Base.FSharp.Benchmarks

open Aardvark.Base
open BenchmarkDotNet.Attributes

[<MemoryDiagnoser>]
type TensorDecompositionBenchmark() =
    let m22 =
        M22d(4.0, -2.0,
             1.5,  3.0)

    let m33 =
        M33d(4.0, -2.0,  1.0,
             1.5,  3.0, -0.5,
             -1.0,  2.0,  5.0)

    let m44 =
        M44d(4.0, -2.0,  1.0,  0.5,
             1.5,  3.0, -0.5,  2.0,
             -1.0,  2.0,  5.0, -1.5,
             0.5, -1.0,  2.5,  3.5)

    [<Benchmark>]
    member _.QrM33d() =
        let struct (q, r) = QR.DecomposeV m33
        q.M00 + r.M00

    [<Benchmark>]
    member _.RqM33d() =
        let struct (r, q) = RQ.DecomposeV m33
        r.M00 + q.M00

    [<Benchmark>]
    member _.BidiagonalizeM33d() =
        let struct (u, b, vt) = QR.BidiagonalizeV m33
        u.M00 + b.M00 + vt.M00

    [<Benchmark>]
    member _.SvdM22d() =
        match SVD.DecomposeV m22 with
        | ValueSome(struct (u, s, vt)) -> u.M00 + s.M00 + vt.M00
        | ValueNone -> nan

    [<Benchmark>]
    member _.SvdM33d() =
        match SVD.DecomposeV m33 with
        | ValueSome(struct (u, s, vt)) -> u.M00 + s.M00 + vt.M00
        | ValueNone -> nan

    [<Benchmark>]
    member _.SvdM44d() =
        match SVD.DecomposeV m44 with
        | ValueSome(struct (u, s, vt)) -> u.M00 + s.M00 + vt.M00
        | ValueNone -> nan
