namespace Aardvark.Base.FSharp.Benchmarks

open System
open System.IO
open System.Runtime.Loader
open Aardvark.Base
open BenchmarkDotNet.Attributes

// One selected implementation per process; defaults to the current assembly.
// Set AARDVARK_RANGESET_ASSEMBLY to the baseline Aardvark.Base.FSharp.dll to compare.
[<MemoryDiagnoser>]
type RangeSetConstructionBenchmark() =
    let mutable construct : unit -> obj = Unchecked.defaultof<_>

    [<Params("Int32", "UInt64")>]
    member val Kind = "Int32" with get, set

    [<Params("array", "list", "seq")>]
    member val Constructor = "array" with get, set

    member _.Inputs =
        [| yield "Empty"; yield "Singleton"
           for count in [32; 4096] do
               for shape in ["Disjoint"; "Adjacent"; "Overlapping"] do
                   for order in ["Sorted"; "Reversed"; "Shuffled"] do
                       yield $"{shape}-{order}-{count}" |]

    [<ParamsSource("Inputs")>]
    member val Input = "Empty" with get, set

    member private x.MakeFactory<'R>(moduleType : Type, ranges : 'R[]) =
        let factoryMethod = moduleType.GetMethod("of" + (match x.Constructor with "array" -> "Array" | "list" -> "List" | _ -> "Seq"))
        match x.Constructor with
        | "array" ->
            let call = factoryMethod.CreateDelegate(typeof<Func<'R[], obj>>) :?> Func<'R[], obj>
            fun () -> call.Invoke ranges
        | "list" ->
            let input = Array.toList ranges
            let call = factoryMethod.CreateDelegate(typeof<Func<'R list, obj>>) :?> Func<'R list, obj>
            fun () -> call.Invoke input
        | _ ->
            // A genuine sequence, not an array/list runtime-type shortcut.
            let input = seq { yield! ranges }
            let call = factoryMethod.CreateDelegate(typeof<Func<seq<'R>, obj>>) :?> Func<seq<'R>, obj>
            fun () -> call.Invoke input

    [<GlobalSetup>]
    member x.Setup() =
        let path =
            match Environment.GetEnvironmentVariable "AARDVARK_RANGESET_ASSEMBLY" with
            | null -> typeof<RangeSet1i>.Assembly.Location
            | path -> Path.GetFullPath path
        let context = AssemblyLoadContext("RangeSet benchmark target")
        let assembly = context.LoadFromAssemblyPath path
        let pairs =
            match x.Input with
            | "Empty" -> [||]
            | "Singleton" -> [| 3, 7 |]
            | _ ->
                let parts = x.Input.Split '-'
                let count = int parts.[2]
                let data = Array.init count (fun i ->
                    match parts.[0] with
                    | "Disjoint" -> 4 * i, 4 * i + 1
                    | "Adjacent" -> 2 * i, 2 * i + 1
                    | _ -> i, i + 16)
                match parts.[1] with
                | "Reversed" -> Array.Reverse data
                | "Shuffled" ->
                    let random = Random(35791)
                    for i in data.Length - 1 .. -1 .. 1 do
                        let j = random.Next(i + 1)
                        let tmp = data.[i]
                        data.[i] <- data.[j]
                        data.[j] <- tmp
                | _ -> ()
                data
        if x.Kind = "Int32" then
            let ranges = pairs |> Array.map (fun (l, r) -> Range1i(l, r))
            construct <- x.MakeFactory(assembly.GetType("Aardvark.Base.RangeSet1iModule", true), ranges)
            let expected = ranges |> Array.fold (fun (s : RangeSet1i) r -> s.Add r) RangeSet1i.empty |> Seq.toArray
            let valid =
                try (construct() :?> seq<Range1i> |> Seq.toArray) = expected
                with _ -> false
            printfn "Construction result agrees with repeated Add: %b" valid
        else
            let ranges = pairs |> Array.map (fun (l, r) -> Range1ul(uint64 l, uint64 r))
            construct <- x.MakeFactory(assembly.GetType("Aardvark.Base.RangeSet1ulModule", true), ranges)
            let expected = ranges |> Array.fold (fun (s : RangeSet1ul) r -> s.Add r) RangeSet1ul.empty |> Seq.toArray
            let valid =
                try (construct() :?> seq<Range1ul> |> Seq.toArray) = expected
                with _ -> false
            printfn "Construction result agrees with repeated Add: %b" valid

    [<Benchmark>]
    member _.Construct() = construct()

// Empty construction is close to harness overhead. Batch it independently to
// amortize invocation/overhead estimation rather than infer regressions from tenths of a ns.
[<MemoryDiagnoser>]
type RangeSetEmptyConstructionBenchmark() =
    let mutable subject = Unchecked.defaultof<RangeSetConstructionBenchmark>

    [<Params("Int32", "UInt64")>]
    member val Kind = "Int32" with get, set

    [<Params("array", "list", "seq")>]
    member val Constructor = "array" with get, set

    [<GlobalSetup>]
    member x.Setup() =
        subject <- RangeSetConstructionBenchmark(Kind = x.Kind, Constructor = x.Constructor, Input = "Empty")
        subject.Setup()

    [<Benchmark(OperationsPerInvoke = 1024)>]
    member _.Construct() =
        let mutable result = null
        for _ in 1 .. 1024 do result <- subject.Construct()
        result
