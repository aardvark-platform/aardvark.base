namespace Aardvark.Base.FSharp.Benchmarks

open Aardvark.Base
open Aardvark.Base.FSharp.Tests
open BenchmarkDotNet.Attributes

// ================================================================================================================
// Vector
// ================================================================================================================

[<AbstractClass>]
type TensorVectorBenchmark() =

    member _.Sizes = [
        16
        128
        512
        1024
        4096
    ]

    [<DefaultValue>]
    val mutable Vector : Vector<float>

    [<DefaultValue; ParamsSource("Sizes", Priority = 0)>]
    val mutable Size : int

    [<DefaultValue; Params(TensorKind.Full, TensorKind.Sub, Priority = 1)>]
    val mutable Kind : TensorKind

    abstract member Setup : unit -> unit

    [<GlobalSetup>]
    default x.Setup() =
        TensorGeneration.init 0
        x.Vector <- TensorGeneration.getVectorOfSize x.Kind x.Size

[<AbstractClass>]
type TensorVectorBenchmark2() =
    inherit TensorVectorBenchmark()

    [<DefaultValue>]
    val mutable Vector2 : Vector<float>

    [<DefaultValue; Params(TensorKind.Full, TensorKind.Sub, Priority = 2)>]
    val mutable Kind2 : TensorKind

    override x.Setup() =
        base.Setup()
        x.Vector2 <- TensorGeneration.getVectorOfSize x.Kind2 x.Size

[<MemoryDiagnoser>]
type TensorVector_Equals() =
    inherit TensorVectorBenchmark()

    [<Benchmark(Description = "Vector<'T>.EqualTo()", Baseline = true)>]
    member x.CSharp() =
        x.Vector.EqualTo x.Vector

    [<Benchmark(Description = "Vector.equals")>]
    member x.FSharp() =
        (x.Vector, x.Vector) ||> Vector.equals

[<MemoryDiagnoser>]
type TensorVector_DotProduct() =
    inherit TensorVectorBenchmark2()

    [<Benchmark(Description = "Vector<'T>.DotProduct()", Baseline = true)>]
    member x.CSharp() =
        x.Vector.DotProduct x.Vector2

    [<Benchmark(Description = "Vector.dot")>]
    member x.FSharp() =
        (x.Vector, x.Vector2) ||> Vector.dot

[<MemoryDiagnoser>]
type TensorVector_Distance() =
    inherit TensorVectorBenchmark2()

    [<Benchmark(Description = "Vector<'T>.Dist2()", Baseline = true)>]
    member x.CSharp() =
        x.Vector.Dist2 x.Vector2

    [<Benchmark(Description = "Vector.distance")>]
    member x.FSharp() =
        (x.Vector, x.Vector2) ||> Vector.distance

[<MemoryDiagnoser>]
type TensorVector_MultiplyTransposed() =
    inherit TensorVectorBenchmark2()

    [<Benchmark(Description = "Vector<'T>.MultiplyTransposed()", Baseline = true)>]
    member x.CSharp() =
        let r = x.Vector.MultiplyTransposed x.Vector2
        r.Data[int r.Origin]

    [<Benchmark(Description = "Vector.multiplyTransposed")>]
    member x.FSharp() =
        let r = (x.Vector, x.Vector2) ||> Vector.multiplyTransposed
        r.Data[int r.Origin]

// ================================================================================================================
// Matrix
// ================================================================================================================

[<AbstractClass>]
type TensorMatrixBenchmark() =

    member _.Sizes = [
        V2i(4, 4)
        V2i(64, 128)
        V2i(256, 512)
        V2i(1024, 32)
        V2i(2048, 512)
    ]

    [<DefaultValue>]
    val mutable Matrix : Matrix<float>

    [<DefaultValue; ParamsSource("Sizes", Priority = 0)>]
    val mutable Size : V2i

    [<DefaultValue; Params(TensorKind.Full, TensorKind.Sub, Priority = 1)>]
    val mutable Kind : TensorKind

    [<DefaultValue; Params(false, true, Priority = 3)>]
    val mutable RowMajor : bool

    abstract member Setup : unit -> unit

    [<GlobalSetup>]
    default x.Setup() =
        TensorGeneration.init 0
        x.Matrix <- TensorGeneration.getMatrixOfSize x.Kind x.RowMajor x.Size

[<AbstractClass>]
type TensorMatrixBenchmark2() =
    inherit TensorMatrixBenchmark()

    [<DefaultValue>]
    val mutable Matrix2 : Matrix<float>

    [<DefaultValue; Params(TensorKind.Full, TensorKind.Sub, Priority = 2)>]
    val mutable Kind2 : TensorKind

    [<DefaultValue; Params(false, true, Priority = 4)>]
    val mutable RowMajor2 : bool

    override x.Setup() =
        base.Setup()
        x.Matrix2 <- TensorGeneration.getMatrixOfSize x.Kind2 x.RowMajor2 x.Size

[<MemoryDiagnoser>]
type TensorMatrix_Add() =
    inherit TensorMatrixBenchmark2()

    [<Benchmark(Description = "Matrix<'T>.Add()", Baseline = true)>]
    member x.CSharp() =
        let m1 = x.Matrix
        let m2 = x.Matrix2
        let r = m1.Add(m2)
        r.Data.[int r.Origin]

    [<Benchmark(Description = "Matrix.add")>]
    member x.FSharp() =
        let m1 = x.Matrix
        let m2 = x.Matrix2
        let r = (m1, m2) ||> Matrix.add
        r.Data.[int r.Origin]

[<MemoryDiagnoser>]
type TensorMatrix_Transform() =
    inherit TensorMatrixBenchmark2()

    [<Benchmark(Description = "Matrix<'T>.Multiply()", Baseline = true)>]
    member x.CSharp() =
        let m = x.Matrix
        let v = x.Matrix2.Row 0L
        let r = m.Multiply(v)
        r.Data.[int r.Origin]

    [<Benchmark(Description = "Matrix.transform")>]
    member x.FSharp() =
        let m = x.Matrix
        let v = x.Matrix2.Row 0L
        let r = (m, v) ||> Matrix.transform
        r.Data.[int r.Origin]

[<MemoryDiagnoser>]
type TensorMatrix_Multiply() =
    inherit TensorMatrixBenchmark2()

    [<Benchmark(Description = "Matrix<'T>.Multiply()", Baseline = true)>]
    member x.CSharp() =
        let m1 = x.Matrix
        let m2 = x.Matrix2.Transposed
        let r = m1.Multiply(m2)
        r.Data.[int r.Origin]

    [<Benchmark(Description = "Matrix.multiply")>]
    member x.FSharp() =
        let m1 = x.Matrix
        let m2 = x.Matrix2.Transposed
        let r = (m1, m2) ||> Matrix.multiply
        r.Data.[int r.Origin]