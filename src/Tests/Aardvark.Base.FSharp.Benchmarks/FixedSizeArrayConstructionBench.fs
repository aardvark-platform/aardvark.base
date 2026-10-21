namespace Aardvark.Base.FSharp.Benchmarks

open System
open System.Diagnostics
open System.IO
open System.Linq.Expressions
open System.Runtime.Loader
open Aardvark.Base
open BenchmarkDotNet.Attributes

// Each isolated process loads one implementation. Both subjects use the same
// compiled public-constructor delegate; reflection and input creation are untimed.
[<MemoryDiagnoser>]
type FixedSizeArrayConstructionBenchmark() =
    let mutable construct : Func<seq<int>, obj> = Unchecked.defaultof<_>
    let mutable source : seq<int> = Seq.empty

    [<Params("array", "list", "seq")>]
    member val Source = "array" with get, set

    [<Params("Zero", "Empty1", "Exact1", "Oversized1",
             "Empty4", "Short4", "Exact4", "Oversized4", "Million4",
             "Empty32", "Short32", "Exact32", "Oversized32")>]
    member val Input = "Exact4" with get, set

    [<GlobalSetup>]
    member x.Setup() =
        let capacity, count =
            match x.Input with
            | "Zero" -> 0, 32
            | "Empty1" -> 1, 0
            | "Exact1" -> 1, 1
            | "Oversized1" -> 1, 4096
            | "Empty4" -> 4, 0
            | "Short4" -> 4, 2
            | "Exact4" -> 4, 4
            | "Oversized4" -> 4, 4096
            | "Million4" -> 4, 1000000
            | "Empty32" -> 32, 0
            | "Short32" -> 32, 2
            | "Exact32" -> 32, 32
            | "Oversized32" -> 32, 4096
            | _ -> invalidArg "Input" "unknown input"
        let data = Array.init count (fun i -> i + 1)
        source <-
            match x.Source with
            | "array" -> data :> seq<int>
            | "list" -> Array.toList data :> seq<int>
            | "seq" -> seq { for i in 0 .. count - 1 do yield i + 1 }
            | _ -> invalidArg "Source" "unknown source kind"
        let path =
            match Environment.GetEnvironmentVariable "AARDVARK_FIXEDARRAY_ASSEMBLY" with
            | null -> typeof<Arr<N<4>, int>>.Assembly.Location
            | path -> Path.GetFullPath path
        let context = AssemblyLoadContext("Fixed array benchmark target")
        let assembly = context.LoadFromAssemblyPath path
        let dimension =
            match capacity with
            | 0 -> typeof<N<0>>
            | 1 -> typeof<N<1>>
            | 4 -> typeof<N<4>>
            | _ -> typeof<N<32>>
        let definition = typeof<Arr<N<4>, int>>.GetGenericTypeDefinition()
        let target = assembly.GetType(definition.FullName, true).MakeGenericType(dimension, typeof<int>)
        let argument = Expression.Parameter(typeof<seq<int>>, "source")
        let body = Expression.Convert(Expression.New(target.GetConstructor([| typeof<seq<int>> |]), argument), typeof<obj>)
        construct <- Expression.Lambda<Func<seq<int>, obj>>(body, argument).Compile()
        let result = target.GetProperty("Data").GetValue(construct.Invoke source) :?> int[]
        let expected = Array.init capacity (fun i -> if i < count then i + 1 else 0)
        if result <> expected then failwith "constructor result mismatch"
        // Warm the selected constructor before BDN sizes its pilot workload.
        // This prevents tier transitions from making the target iterations tiny.
        let timer = Stopwatch.StartNew()
        while timer.Elapsed.TotalSeconds < 2.0 do
            for _ in 1 .. 64 do GC.KeepAlive(construct.Invoke source)

    [<Benchmark(OperationsPerInvoke = 64)>]
    member _.Construct() =
        let mutable result = null
        for _ in 1 .. 64 do result <- construct.Invoke source
        result
