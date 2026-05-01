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

    let private assertNullSourceArg (action : unit -> 'T) =
        let ex =
            NUnit.Framework.Assert.Throws<System.ArgumentNullException>(fun () -> action() |> ignore)

        NUnit.Framework.Assert.That(ex.ParamName, NUnit.Framework.Is.EqualTo("source"))

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

        let foldi (folder: int -> int -> int -> int) (state: int) (data: int seq) =
            let mutable state = state
            let mutable i = 0
            for d in data do
                state <- folder i state d
                i <- i + 1
            state

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

    [<Property>]
    let ``[Seq] foldi`` (data: int list) =
        let folder i state value = state + value + (i * 17)

        let result =
            data
            |> Seq.foldi folder 0

        let expected =
            data
            |> Ref.foldi folder 0

        result = expected

    [<NUnit.Framework.Test>]
    let ``[Seq] foldi observes distinct indices`` () =
        let data = [ 10; 20; 30 ]

        let seen =
            data
            |> Seq.foldi (fun i acc value -> (i, value) :: acc) []
            |> List.rev

        let expected = [ (0, 10); (1, 20); (2, 30) ]
        NUnit.Framework.Assert.That(seen, NUnit.Framework.Is.EqualTo(expected))

    [<NUnit.Framework.Test>]
    let ``[Seq] choosei rejects null source`` () =
        assertNullSourceArg (fun () -> Seq.choosei (fun _ value -> Some value) (null : int seq) |> Seq.toArray)

    [<NUnit.Framework.Test>]
    let ``[Seq] collecti rejects null source`` () =
        assertNullSourceArg (fun () -> Seq.collecti (fun _ value -> Seq.singleton value) (null : int seq) |> Seq.toArray)

    [<NUnit.Framework.Test>]
    let ``[Seq] foldi rejects null source`` () =
        assertNullSourceArg (fun () -> Seq.foldi (fun _ state value -> state + value) 0 (null : int seq))

    [<NUnit.Framework.Test>]
    let ``[Seq] tryPickV rejects null source`` () =
        assertNullSourceArg (fun () -> Seq.tryPickV (fun value -> ValueSome value) (null : int seq))

    [<NUnit.Framework.Test>]
    let ``[Seq] pickV rejects null source`` () =
        assertNullSourceArg (fun () -> Seq.pickV (fun value -> ValueSome value) (null : int seq))

    [<NUnit.Framework.Test>]
    let ``[Seq] tryFindV rejects null source`` () =
        assertNullSourceArg (fun () -> Seq.tryFindV (fun _ -> true) (null : int seq))

    [<NUnit.Framework.Test>]
    let ``[Seq] tryHeadV rejects null source`` () =
        assertNullSourceArg (fun () -> Seq.tryHeadV (null : int seq))

    [<NUnit.Framework.Test>]
    let ``[Seq] tryLastV rejects null source`` () =
        assertNullSourceArg (fun () -> Seq.tryLastV (null : int seq))

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

    let private assertNullArrayArg (action : unit -> 'T) =
        let ex =
            NUnit.Framework.Assert.Throws<System.ArgumentNullException>(fun () -> action() |> ignore)

        NUnit.Framework.Assert.That(ex.ParamName, NUnit.Framework.Is.EqualTo("array"))

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
    let ``[Array] zipV and unzipV support empty arrays`` () =
        let zipped =
            Array.zipV ([||] : string[]) ([||] : string[])

        let struct (left : string[], right : string[]) =
            Array.unzipV ([||] : struct (string * string)[])

        NUnit.Framework.Assert.That(zipped, NUnit.Framework.Is.EqualTo([||] : struct (string * string)[]))
        NUnit.Framework.Assert.That(left, NUnit.Framework.Is.EqualTo([||] : string[]))
        NUnit.Framework.Assert.That(right, NUnit.Framework.Is.EqualTo([||] : string[]))

    [<NUnit.Framework.Test>]
    let ``[Array] zipV and unzipV preserve reference payloads and nulls`` () =
        let left = [| "alpha"; null; "gamma" |]
        let right = [| "one"; "two"; null |]

        let zipped =
            Array.zipV left right

        let expected =
            [|
                struct ("alpha", "one")
                struct (null, "two")
                struct ("gamma", null)
            |]

        let struct (unzippedLeft, unzippedRight) =
            Array.unzipV zipped

        NUnit.Framework.Assert.That(zipped, NUnit.Framework.Is.EqualTo(expected))
        NUnit.Framework.Assert.That(unzippedLeft, NUnit.Framework.Is.EqualTo(left))
        NUnit.Framework.Assert.That(unzippedRight, NUnit.Framework.Is.EqualTo(right))

    [<NUnit.Framework.Test>]
    let ``[Array] zipV throws for different-length inputs`` () =
        let left = [| 1; 2 |]
        let right = [| 3 |]

        let ex =
            NUnit.Framework.Assert.Throws<System.ArgumentException>(fun () -> Array.zipV left right |> ignore)

        NUnit.Framework.StringAssert.Contains("array1.Length = 2", ex.Message)
        NUnit.Framework.StringAssert.Contains("array2.Length = 1", ex.Message)

    [<NUnit.Framework.Test>]
    let ``[Array] zipV throws ArgumentNullException for null left input`` () =
        let left : string[] = null
        let right = [| "value" |]

        let ex =
            NUnit.Framework.Assert.Throws<System.ArgumentNullException>(fun () -> Array.zipV left right |> ignore)

        NUnit.Framework.Assert.That(ex.ParamName, NUnit.Framework.Is.EqualTo("array1"))

    [<NUnit.Framework.Test>]
    let ``[Array] zipV throws ArgumentNullException for null right input`` () =
        let left = [| "value" |]
        let right : string[] = null

        let ex =
            NUnit.Framework.Assert.Throws<System.ArgumentNullException>(fun () -> Array.zipV left right |> ignore)

        NUnit.Framework.Assert.That(ex.ParamName, NUnit.Framework.Is.EqualTo("array2"))

    [<NUnit.Framework.Test>]
    let ``[Array] unzipV throws ArgumentNullException for null input`` () =
        let data : struct (string * string)[] = null

        let ex =
            NUnit.Framework.Assert.Throws<System.ArgumentNullException>(fun () -> Array.unzipV data |> ignore)

        NUnit.Framework.Assert.That(ex.ParamName, NUnit.Framework.Is.EqualTo("array"))

    [<NUnit.Framework.Test>]
    let ``[Array] chooseV throws ArgumentNullException for null input`` () =
        let data : int[] = null
        assertNullArrayArg (fun () -> Array.chooseV (fun x -> ValueSome x) data)

    [<NUnit.Framework.Test>]
    let ``[Array] chooseiV throws ArgumentNullException for null input`` () =
        let data : int[] = null
        assertNullArrayArg (fun () -> Array.chooseiV (fun _ x -> ValueSome x) data)

    [<NUnit.Framework.Test>]
    let ``[Array] foldi throws ArgumentNullException for null input`` () =
        let data : int[] = null
        assertNullArrayArg (fun () -> Array.foldi (fun _ state value -> state + value) 0 data)

    [<NUnit.Framework.Test>]
    let ``[Array] tryPickV throws ArgumentNullException for null input`` () =
        let data : int[] = null
        assertNullArrayArg (fun () -> Array.tryPickV (fun x -> ValueSome x) data)

    [<NUnit.Framework.Test>]
    let ``[Array] pickV throws ArgumentNullException for null input`` () =
        let data : int[] = null
        assertNullArrayArg (fun () -> Array.pickV (fun x -> ValueSome x) data)

    [<NUnit.Framework.Test>]
    let ``[Array] tryFindV throws ArgumentNullException for null input`` () =
        let data : int[] = null
        assertNullArrayArg (fun () -> Array.tryFindV (fun _ -> true) data)

    [<NUnit.Framework.Test>]
    let ``[Array] tryHeadV throws ArgumentNullException for null input`` () =
        let data : int[] = null
        assertNullArrayArg (fun () -> Array.tryHeadV data)

    [<NUnit.Framework.Test>]
    let ``[Array] tryLastV throws ArgumentNullException for null input`` () =
        let data : int[] = null
        assertNullArrayArg (fun () -> Array.tryLastV data)

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
