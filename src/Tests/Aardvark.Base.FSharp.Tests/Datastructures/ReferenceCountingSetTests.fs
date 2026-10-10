namespace Aardvark.Base.FSharp.Tests

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open FsUnit
open NUnit.Framework

module ReferenceCountingSetTests =

    let private asCollection (set : ReferenceCountingSet<string>) =
        set :> ICollection<string>

    [<Test>]
    let ``[ReferenceCountingSet] Duplicate null adds and removes keep distinct membership stable`` () =
        let set = ReferenceCountingSet<string>()

        set.Add null |> should equal true
        set.Add null |> should equal false
        set.Count |> should equal 1
        set.GetReferenceCount null |> should equal 2
        set.Contains null |> should equal true

        set.Remove null |> should equal false
        set.Count |> should equal 1
        set.GetReferenceCount null |> should equal 1
        set.Contains null |> should equal true

        set.Remove null |> should equal true
        set.Count |> should equal 0
        set.GetReferenceCount null |> should equal 0
        set.Contains null |> should equal false

    [<Test>]
    let ``[ReferenceCountingSet] Removing absent null is a no-op`` () =
        let set = ReferenceCountingSet<string>()

        set.Remove null |> should equal false
        set.GetReferenceCount null |> should equal 0
        set.Count |> should equal 0

        set.Add "a" |> should equal true
        set.Remove null |> should equal false
        set.GetReferenceCount null |> should equal 0
        set.Count |> should equal 1
        set.Contains "a" |> should equal true

    [<Test>]
    let ``[ReferenceCountingSet] ICollection Contains uses public null-aware lookup`` () =
        let set = ReferenceCountingSet<string>()
        let collection = asCollection set

        set.Add null |> ignore
        set.Add "a" |> ignore

        collection.Contains null |> should equal true
        collection.Contains "a" |> should equal true
        collection.Contains "missing" |> should equal false

    [<Test>]
    let ``[ReferenceCountingSet] ICollection CopyTo writes distinct null and non-null values once`` () =
        let set = ReferenceCountingSet<string>()
        let collection = asCollection set

        set.Add null |> ignore
        set.Add null |> ignore
        set.Add "a" |> ignore
        set.Add "b" |> ignore

        let target = [| "before"; "x"; "x"; "x"; "after" |]

        collection.CopyTo(target, 1)

        target.[0] |> should equal "before"
        target.[1] |> should equal null
        CollectionAssert.AreEquivalent([| "a"; "b" |], [| target.[2]; target.[3] |])
        target.[4] |> should equal "after"

    [<Test>]
    let ``[ReferenceCountingSet] ICollection CopyTo validates capacity before writing`` () =
        let set = ReferenceCountingSet<string>()
        let collection = asCollection set

        set.Add null |> ignore
        set.Add "a" |> ignore

        let target = [| "keep"; "keep" |]

        let ex = Assert.Throws<ArgumentException>(fun () -> collection.CopyTo(target, 1))
        ex.ParamName |> should equal "array"
        target |> should equal [| "keep"; "keep" |]

    [<Test>]
    let ``[ReferenceCountingSet] Set algebra accounts for null membership`` () =
        let set = ReferenceCountingSet<string>()
        set.Add null |> ignore
        set.Add "a" |> ignore

        set.SetEquals [ null; "a" ] |> should equal true
        set.SetEquals [ "a" ] |> should equal false
        set.IsSupersetOf [ null ] |> should equal true
        set.IsSupersetOf [ null; "a"; "b" ] |> should equal false
        set.Overlaps [ null; "missing" ] |> should equal true

    [<Test>]
    let ``[ReferenceCountingSet] IntersectWith removes or preserves null membership`` () =
        let removeNull = ReferenceCountingSet<string>()
        removeNull.Add null |> ignore
        removeNull.Add null |> ignore
        removeNull.Add "a" |> ignore

        removeNull.IntersectWith [ "a" ]

        removeNull.Contains null |> should equal false
        removeNull.GetReferenceCount null |> should equal 0
        removeNull.Contains "a" |> should equal true

        let keepNull = ReferenceCountingSet<string>()
        keepNull.Add null |> ignore
        keepNull.Add "a" |> ignore

        keepNull.IntersectWith [ null ]

        keepNull.Contains null |> should equal true
        keepNull.Contains "a" |> should equal false
        keepNull.Count |> should equal 1

    [<Test>]
    let ``[ReferenceCountingSet] SymmetricExceptWith toggles null membership`` () =
        let set = ReferenceCountingSet<string>()

        set.SymmetricExceptWith [ null ]

        set.Contains null |> should equal true
        set.GetReferenceCount null |> should equal 1

        set.SymmetricExceptWith [ null ]

        set.Contains null |> should equal false
        set.GetReferenceCount null |> should equal 0

    let private acquire (generic : bool) (set : ReferenceCountingSet<'a>) : IEnumerator =
        if generic then (set :> IEnumerable<'a>).GetEnumerator() :> IEnumerator
        else (set :> IEnumerable).GetEnumerator()

    let private checkReplay (values : 'a[]) =
        for generic in [false; true] do
            for resetBeforeFirstMove in [false; true] do
                let set = ReferenceCountingSet values
                // Keep these interface views of one boxed enumerator for every reset and traversal.
                let e = acquire generic set
                use cleanup = e :?> IDisposable
                let typed = e :?> IEnumerator<'a>
                let context = sprintf "values=%A, generic=%b, reset before first move=%b" values generic resetBeforeFirstMove
                let read () =
                    let result = ResizeArray<'a>()
                    for i in 0 .. set.Count do
                        let moved = e.MoveNext()
                        Assert.AreEqual(i < set.Count, moved, sprintf "%s, traversal step=%d" context i)
                        if moved then
                            let value = typed.Current
                            Assert.AreEqual(value, unbox<'a> e.Current, context)
                            result.Add value
                    result.ToArray()

                if resetBeforeFirstMove then e.Reset()
                let original = read()
                CollectionAssert.AreEquivalent(Array.distinct values, original, context)
                for prefix in 0 .. original.Length + 1 do
                    e.Reset()
                    for i in 0 .. prefix - 1 do
                        let moved = e.MoveNext()
                        Assert.AreEqual(i < original.Length, moved, sprintf "%s, prefix=%d, step=%d" context prefix i)
                        if moved then Assert.AreEqual(original.[i], typed.Current, context)
                    for replay in 1 .. 3 do
                        // Consecutive resets also remain at the beginning.
                        e.Reset()
                        e.Reset()
                        CollectionAssert.AreEqual(original, read(), sprintf "%s, prefix=%d, replay=%d" context prefix replay)

    [<Test>]
    let ``[ReferenceCountingSet] Reset replays reference and value elements on the same enumerator`` () =
        for values in [ [||]; [| null |]; [| null; null |]; [| "a" |]; [| "a"; "b" |];
                        [| null; "a"; "b" |]; [| "b"; null; "a"; "b"; null; "a" |] ] do
            checkReplay values
        for values in [ [||]; [| 0 |]; [| 0; 1; -7 |]; [| 0; 1; -7; 0; 1; -7 |] ] do
            checkReplay values

    [<Test>]
    let ``[ReferenceCountingSet] Failed reset preserves Add mutation invalidation`` () =
        for generic in [false; true] do
            for containsNull in [false; true] do
                let values = if containsNull then [| null; "a"; "b" |] else [| "a"; "b" |]
                for prefix in [0; 1; values.Length; values.Length + 1] do
                    let set = ReferenceCountingSet values
                    let e = acquire generic set
                    use cleanup = e :?> IDisposable
                    let context = sprintf "generic=%b, null=%b, prefix=%d" generic containsNull prefix
                    for i in 0 .. prefix - 1 do Assert.AreEqual(i < values.Length, e.MoveNext(), context)
                    Assert.IsTrue(set.Add "added", context)
                    for attempt in 1 .. 3 do
                        let state = sprintf "%s, attempt=%d" context attempt
                        Assert.Throws<InvalidOperationException>((fun () -> e.Reset()), state) |> ignore
                        // If null was already consumed, a failed reset must not rearm its emission.
                        if not containsNull || prefix > 0 then
                            Assert.Throws<InvalidOperationException>((fun () -> e.MoveNext() |> ignore), state) |> ignore
                    // Mutation leaves Current undefined, so it is intentionally not inspected.
