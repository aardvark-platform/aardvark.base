namespace Aardvark.Base.FSharp.Benchmarks

#nowarn "44"

open System
open Aardvark.Base
open Aardvark.Base.TypeInfo
open BenchmarkDotNet.Attributes

module PatternsImpl =

    module PatternsNaive =
        let inline private typeInfo t = { simpleType = t } :> ITypeInfo

        [<return: Struct>]
        let inline (|Matrix|_|) (t : Type) =
            if Set.contains (typeInfo t) MatrixTypes then
                ValueSome ()
            else
                ValueNone

        [<return: Struct>]
        let inline (|MatrixOf|_|) (t : Type) =
            MatrixTypes
            |> Seq.tryFindV (fun vi -> vi.Type.Name = t.Name)
            |> ValueOption.map (fun t ->
                let mt = unbox<MatrixType> t
                mt.dimension, mt.baseType.Type
            )

        [<return: Struct>]
        let inline (|List|_|) (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = TList.simpleType then
                ValueSome <| t.GetGenericArguments().[0]
            else
                ValueNone

[<MemoryDiagnoser>]
type ``TypePatterns Matrix``() =

    member inline x.IsMatrixNaive (t: Type) =
        match t with
        | PatternsImpl.PatternsNaive.Matrix -> true
        | _ -> false

    member inline x.IsMatrixOptimized (t: Type) =
        match t with
        | TypeMeta.Patterns.Matrix -> true
        | _ -> false

    [<Benchmark(Description = "Naive", Baseline = true)>]
    member x.Naive() =
        x.IsMatrixNaive typeof<M44d> && x.IsMatrixNaive typeof<unit>

    [<Benchmark(Description = "Optimized")>]
    member x.Optimized() =
        x.IsMatrixOptimized typeof<M44d> && x.IsMatrixOptimized typeof<unit>

[<MemoryDiagnoser>]
type ``TypePatterns MatrixOf``() =

    member inline x.IsMatrixNaive (t: Type) =
        match t with
        | PatternsImpl.PatternsNaive.MatrixOf(dim, baseType) -> true
        | _ -> false

    member inline x.IsMatrixOptimized (t: Type) =
        match t with
        | TypeMeta.Patterns.MatrixOf(dim, baseType) -> true
        | _ -> false

    [<Benchmark(Description = "Naive", Baseline = true)>]
    member x.Naive() =
        x.IsMatrixNaive typeof<M44d> && x.IsMatrixNaive typeof<unit>

    [<Benchmark(Description = "Optimized")>]
    member x.Optimized() =
        x.IsMatrixOptimized typeof<M44d> && x.IsMatrixOptimized typeof<unit>

[<MemoryDiagnoser>]
type ``TypePatterns List``() =

    member inline x.IsListNaive (t: Type) =
        match t with
        | PatternsImpl.PatternsNaive.List(baseType) -> true
        | _ -> false

    member inline x.IsListOptimized (t: Type) =
        match t with
        | TypeMeta.Patterns.ListOf(baseType) -> true
        | _ -> false

    [<Benchmark(Description = "Naive", Baseline = true)>]
    member x.Naive() =
        x.IsListNaive typeof<list<M44d>> && x.IsListNaive typeof<unit>

    [<Benchmark(Description = "Optimized")>]
    member x.Optimized() =
        x.IsListOptimized typeof<list<M44d>> && x.IsListOptimized typeof<unit>