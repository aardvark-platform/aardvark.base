namespace Aardvark.Base.FSharp.Tests

open FsCheck
open FsCheck.NUnit
open Aardvark.Base
open System.Collections.Generic

module Map =

    type Key =
        | A = 0
        | B = 1
        | C = 2

    [<Property>]
    let ``[Map] ofSeqWithDuplicates`` (data: list<Key * int>) =
        let result = Map.ofSeqWithDuplicates data
        let expected =
            let d = Dictionary()
            for k, v in data do
                let set = d |> Dictionary.tryFind k |> Option.defaultValue Set.empty
                d.[k] <- set |> Set.add v
            d

        result = Dictionary.toMap expected

module Seq =

    module Ref =

        let choosei (chooser: int -> int -> int option) (data: int seq) =
            let r = ResizeArray<int>()
            let mutable i = 0
            for d in data do
                match chooser i d with
                | Some v -> r.Add v
                | _ -> ()
                i <- i + 1
            r :> seq<_>

        let collecti (mapping: int -> int -> #seq<int>) (data: int seq) =
            let r = ResizeArray<int>()
            let mutable i = 0
            for d in data do
                r.AddRange(mapping i d)
                i <- i + 1
            r :> seq<_>

    [<Property>]
    let ``[Seq] choosei`` (chooser: int -> int -> int option) (data: int list) =
        let result =
            data
            |> Seq.choosei chooser
            |> Seq.toArray

        let expected =
            data
            |> Ref.choosei chooser
            |> Seq.toArray

        result = expected

    [<Property>]
    let ``[Seq] collecti`` (mapping: int -> int -> int list) (data: int list) =
        let result =
            data
            |> Seq.collecti (fun i v -> mapping i v)
            |> Seq.toArray

        let expected =
            data
            |> Ref.collecti mapping
            |> Seq.toArray

        result = expected

module List =

    [<Property>]
    let ``[List] choosei`` (chooser: int -> int -> int option) (data: int list) =
        let result =
            data
            |> List.choosei chooser
            |> List.toArray

        let expected =
            data
            |> Seq.Ref.choosei chooser
            |> Seq.toArray

        result = expected

    [<Property>]
    let ``[List] collecti`` (mapping: int -> int -> int list) (data: int list) =
        let result =
            data
            |> List.collecti mapping
            |> List.toArray

        let expected =
            data
            |> Seq.Ref.collecti mapping
            |> Seq.toArray

        result = expected

module Array =

    let private toValueOption chooser value =
        match chooser value with
        | Some value -> ValueSome value
        | None -> ValueNone

    let private toIndexedValueOption chooser index value =
        match chooser index value with
        | Some value -> ValueSome value
        | None -> ValueNone

    let private toStructTupleArray (data : ('a * 'b)[]) =
        data |> Array.map (fun (a, b) -> struct (a, b))

    [<Property>]
    let ``[Array] chooseV`` (chooser: int -> int option) (data: int[]) =
        let result =
            data
            |> Array.chooseV (toValueOption chooser)

        let expected =
            data
            |> Array.choose chooser

        result = expected

    [<Property>]
    let ``[Array] choosei`` (chooser: int -> int -> int option) (data: int[]) =
        let result =
            data
            |> Array.choosei chooser

        let expected =
            data
            |> Seq.Ref.choosei chooser
            |> Seq.toArray

        result = expected

    [<Property>]
    let ``[Array] chooseiV`` (chooser: int -> int -> int option) (data: int[]) =
        let result =
            data
            |> Array.chooseiV (toIndexedValueOption chooser)

        let expected =
            data
            |> Seq.Ref.choosei chooser
            |> Seq.toArray

        result = expected

    [<Property>]
    let ``[Array] zipV`` (data : (int * int)[]) =
        let left, right = Array.unzip data

        let result =
            Array.zipV left right

        let expected =
            Array.zip left right
            |> toStructTupleArray

        result = expected

    [<Property>]
    let ``[Array] unzipV`` (data : (int * int)[]) =
        let structData = toStructTupleArray data

        let struct (result1, result2) =
            Array.unzipV structData

        let expected1, expected2 =
            Array.unzip data

        result1 = expected1 && result2 = expected2

    [<Property>]
    let ``[Array] unzipV and zipV round-trip struct tuples`` (data : (int * int)[]) =
        let structData = toStructTupleArray data
        let struct (left, right) = Array.unzipV structData

        let result =
            Array.zipV left right

        result = structData

    [<NUnit.Framework.Test>]
    let ``[Array] zipV throws for different-length inputs`` () =
        let left = [| 1; 2 |]
        let right = [| 3 |]

        NUnit.Framework.Assert.Throws<System.ArgumentException>(fun () -> Array.zipV left right |> ignore)
        |> ignore

    [<Property>]
    let ``[Array] collecti`` (mapping: int -> int -> int[]) (data: int[]) =
        let result =
            data
            |> Array.collecti mapping

        let expected =
            data
            |> Seq.Ref.collecti mapping
            |> Seq.toArray

        result = expected
