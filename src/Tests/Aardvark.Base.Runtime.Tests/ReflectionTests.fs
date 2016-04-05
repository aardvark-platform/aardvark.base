namespace Aardvark.Base.Runtime.Tests

open System
open System.Threading
open System.Threading.Tasks
open System.Collections.Generic
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Microsoft.FSharp.NativeInterop
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.Base.Runtime
open Aardvark.Base.Runtime
open NUnit.Framework
open FsUnit
open System.IO


module RefelectionTests =
    open Aardvark.Base.Runtime.FieldCoders
    type private Marker = Marker

    let mutable initialized = false

    let init() =
        if not initialized then
            initialized <- true
            let ass = typeof<Marker>.Assembly
            Report.LogFileName <- System.IO.Path.GetDirectoryName(ass.Location) + "\\Aardvark.log"
            System.Environment.CurrentDirectory <- System.IO.Path.GetDirectoryName(ass.Location)
            IntrospectionProperties.CustomEntryAssembly <- ass
            Aardvark.Init()

    do init()

    let tryUnify (decl : Type) (real : Type) =
        match decl.TryUnifyWith real with
            | Some ass ->
                decl.GetGenericArguments() |> Array.map (fun a -> HashMap.find a ass) |> Array.toList |> Some
            | None ->
                None

    let shouldWork (l : Option<list<Type>>) =
        match l with
            | Some [] -> ()
            | _ -> failwithf "got %A but expected %A" l (Some [])



    [<Test>]
    let ``[Unification] seq<'a> <-> list<int>``() =
        tryUnify typedefof<seq<_>> typeof<list<int>> |> should equal (Some [typeof<int>])

    [<Test>]
    let ``[Unification] seq<'a> <-> Dictionary<int, float>``() =
        tryUnify typedefof<seq<_>> typeof<Dictionary<int, float>> |> should equal (Some [typeof<KeyValuePair<int, float>>])

    [<Test>]
    let ``[Unification] FSharpFunc<'a, 'b> <-> lambda``() =
        let test = fun a -> a + 1
        tryUnify typedefof<_ -> _> (test.GetType()) |> should equal (Some [typeof<int>; typeof<int>])

    [<Test>]
    let ``[Unification] IEnumerable <-> List<int>``() =
        tryUnify typeof<System.Collections.IEnumerable> typeof<List<int>> |> shouldWork

    [<Test>]
    let ``[Unification] Array <-> int[]``() =
        tryUnify typedefof<Array> typeof<int[]> |> shouldWork


    [<Test>]
    let ``[Extensions] List<int>.Set[int]``() =
        init()
        typeof<List<int>>.GetMethodOrExtension("Set", [|typeof<int>|]) |> should not' (be Null)

    [<Test>]
    let ``[Extensions] int[].Set[int]``() =
        init()
        typeof<int[]>.GetMethodOrExtension("Set", [|typeof<int>|]) |> should not' (be Null)

    [<Test>]
    let ``[Extensions] int[].SetByIndex[Func<int, int>]``() =
        init()
        typeof<int[]>.GetMethodOrExtension("SetByIndex", [| typeof<Func<int, int>> |]) |> should not' (be Null)