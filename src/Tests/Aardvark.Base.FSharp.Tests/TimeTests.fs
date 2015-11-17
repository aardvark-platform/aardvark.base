namespace Aardvark.Base.FSharp.Tests
#nowarn "44"

open System
open Aardvark.Base
open FsUnit
open NUnit.Framework

module ``Time tests`` =
    
    let createTimesAfter (r : Time) (n : int) =
        let mutable current = r
        let mutable l = []
        for i in 1..n do
            current <- Time.after current
            l <- current::l

        l |> List.rev

    let checkTime (r : Time) (l : list<Time>) =
        for i in 0..r.Count-1 do
            match r |> Time.nth i with
                | Some t ->
                    if List.nth l i <> t then
                        failwithf "different at index: %A" i
                | None ->
                    failwithf "could not get time for index: %A" i

    [<Test>]
    let ``[Time] indexing test``() =
        
        let r = Time.newRoot()
        r |> Time.nth 0 |> should equal (Some r)
        r |> Time.nth 1 |> should equal None
        r |> Time.nth -1 |> should equal None


        // create 100 times
        let times = createTimesAfter r 100

        // the count should include the root time
        r.Count |> should equal 101

        let all = r::times

        // lookups should yield correct times
        all |> checkTime r

        // after deleting times the lookups should still be
        // consistent
        let random = Random()
        let newTimes =
            times |> List.filter (fun t ->
                if random.NextDouble() < 0.5 then
                    Time.delete t
                    false
                else
                    true
            )

        let all = r::newTimes
        all |> List.length |> should equal r.Count
        all |> checkTime r

    [<Test>]
    let ``[Time] delete all``() =
        let r = Time.newRoot()
        let someTimes = createTimesAfter r 100


        Time.deleteAll r

        r.Count |> should equal 1
        r.Next |> should equal r
        r.Prev |> should equal r
        r.Representant |> should equal r
        r.Time |> should equal 0UL
        r |> Time.nth 0 |> should equal (Some r)
        r |> Time.nth 1 |> should equal None
        r |> Time.nth -1 |> should equal None

    let ``[Time] insert order test``() =
        let r = Time.newRoot()

        let t0 = Time.after r
        let t1 = Time.after t0
        let t2 = Time.after t1

        r.Next |> should equal t0
        t0.Next |> should equal t1
        t1.Next |> should equal t2
        t2.Next |> should equal r

        r.Prev |> should equal t2
        t2.Prev |> should equal t1
        t1.Prev |> should equal t0
        t0.Prev |> should equal r


        r |> should be (lessThan t0)
        t0 |> should be (lessThan t1)
        t1 |> should be (lessThan t2)

    let ``[Time] delete test``() =
        let r = Time.newRoot()
        let t0 = Time.after r
        let t1 = Time.after t0
        let t2 = Time.after t1

        Time.delete t1
        let mutable failed = false
        try
            compare t1 r |> ignore
        with _ -> 
            failed <- true

        if not failed then failwith "compare should not work on deleted time"
        t0.Next |> should equal t2
        t2.Prev |> should equal t0

        r.[0] |> should equal r
        r.[1] |> should equal t0
        r.[2] |> should equal t2




