namespace Aardvark.Base.FSharp.Tests

open System
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
