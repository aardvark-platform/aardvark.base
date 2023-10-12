namespace Aardvark.Base.FSharp.Benchmarks

#nowarn "44"

open System
open Aardvark.Base

module RangeSetTests =
    open FsUnit
    open NUnit.Framework

    [<Theory>]
    let ``[RangeSet] Insert`` (maxValue : bool) (mergeWithMaxValue : bool) =
        let mutable s1 = RangeSet64.empty
        let mutable s2 = RangeSet1l.empty

        let add (r : Range1l) =
            s1 <- s1 |> RangeSet64.insert r
            s2 <- s2 |> RangeSet1l.add r

        let equal (expected : Range1l list) =
            // Old implementation broken for MaxValue!
            if not maxValue then
                Seq.toList s1 |> should equal (Seq.toList s2)

            Seq.toList s2 |> should equal expected

        add <| Range1l(0L, 1L)
        add <| Range1l(1L, 2L)
        equal [ Range1l(0L, 2L) ]

        add <| Range1l(3L, 4L)
        equal [ Range1l(0L, 4L) ]

        add <| Range1l(6L, 8L)
        equal [ Range1l(0L, 4L); Range1l(6L, 8L) ]

        if maxValue then
            add <| Range1l Int64.MaxValue
            equal [ Range1l(0L, 4L); Range1l(6L, 8L); Range1l(Int64.MaxValue) ]

            add <| Range1l(9L, 14L)
            equal [ Range1l(0L, 4L); Range1l(6L, 14L); Range1l(Int64.MaxValue) ]

            add <| Range1l(42L, if mergeWithMaxValue then Int64.MaxValue - 1L else Int64.MaxValue)
            equal [ Range1l(0L, 4L); Range1l(6L, 14L); Range1l(42L, Int64.MaxValue) ]

    [<Theory>]
    let ``[RangeSet] Remove`` (maxValue : bool) =
        let init = [ Range1l(0L, 4L); Range1l(6L, 8L); if maxValue then Range1l(42L, Int64.MaxValue) ]
        let mutable s1 = RangeSet64.ofList init
        let mutable s2 = RangeSet1l.ofList init

        let rem (r : Range1l) =
            s1 <- s1 |> RangeSet64.remove r
            s2 <- s2 |> RangeSet1l.remove r

        let equal (expected : Range1l list) =
            // Old implementation broken for MaxValue!
            if not maxValue then
                Seq.toList s1 |> should equal (Seq.toList s2)

            Seq.toList s2 |> should equal expected

        rem <| Range1l(3L, 3L)
        equal [ Range1l(0L, 2L); Range1l(4L, 4L); Range1l(6L, 8L); if maxValue then Range1l(42L, Int64.MaxValue) ]

        rem <| Range1l(4L, 4L)
        equal [ Range1l(0L, 2L); Range1l(6L, 8L); if maxValue then Range1l(42L, Int64.MaxValue) ]

        rem <| Range1l(2L, 7L)
        equal [ Range1l(0L, 1L); Range1l(8L, 8L); if maxValue then Range1l(42L, Int64.MaxValue) ]

        rem <| Range1l(-12L, 7L)
        equal [ Range1l(8L, 8L); if maxValue then Range1l(42L, Int64.MaxValue) ]

        if maxValue then
            rem <| Range1l(0L, Int64.MaxValue - 1L)
            equal [ Range1l(Int64.MaxValue) ]

            rem <| Range1l(Int64.MaxValue)
            equal []

            s2 <- RangeSet1l.ofList [ Range1l(0L, Int64.MaxValue) ]
            rem <| Range1l(Int64.MaxValue)
            equal [ Range1l(0L, Int64.MaxValue - 1L) ]

    [<Theory>]
    let ``[RangeSet] ToList`` (maxValue : bool) =
        let init = [ Range1l(0L, 4L); Range1l(6L, 8L); if maxValue then Range1l(10L, Int64.MaxValue)  ]
        let mutable s1 = RangeSet64.ofList init
        let mutable s2 = RangeSet1l.ofList init

        RangeSet64.toList s1 |> should equal init
        RangeSet1l.toList s2 |> should equal init

    [<Theory>]
    let ``[RangeSet] ToArray`` (maxValue : bool) =
        let init = [ Range1l(0L, 4L); Range1l(6L, 8L); if maxValue then Range1l(10L, Int64.MaxValue) ]
        let mutable s1 = RangeSet64.ofList init
        let mutable s2 = RangeSet1l.ofList init

        RangeSet64.toArray s1 |> should equal (Array.ofList init)
        RangeSet1l.toArray s2 |> should equal (Array.ofList init)

    [<Theory>]
    let ``[RangeSet] ToSeq`` (maxValue : bool) =
        let init = [ Range1l(0L, 4L); Range1l(6L, 8L); if maxValue then Range1l(10L, Int64.MaxValue) ]
        let mutable s1 = RangeSet64.ofList init
        let mutable s2 = RangeSet1l.ofList init

        RangeSet64.toSeq s1 |> Seq.toList |> should equal init
        RangeSet1l.toSeq s2 |> Seq.toList |> should equal init

    [<Theory>]
    let ``[RangeSet] Min / Max / Range`` (maxValue : bool) =
        let init = [ Range1l(-3L, -2L); Range1l(1L, 4L); Range1l(6L, 8L); if maxValue then Range1l(10L, Int64.MaxValue) ]
        let min = init |> List.map (fun r -> r.Min) |> List.min
        let max = init |> List.map (fun r -> r.Max) |> List.max

        let mutable s1 = RangeSet64.ofList init
        let mutable s2 = RangeSet1l.ofList init

        s1.Min |> should equal s2.Min
        s2.Min |> should equal min

        s1.Max |> should equal (max + 1L)   // Old implementation is inconsistent!
        s2.Max |> should equal max

        s1.Range |> should equal (Range1l(min, max + 1L))
        s2.Range |> should equal (Range1l(min, max))

    [<Theory>]
    let ``[RangeSet] Intersect`` (hasMaxValue : bool) (testMaxValue : bool) =
        let init = [ Range1l(-3L, -2L); Range1l(1L, 4L); Range1l(6L, 8L); if hasMaxValue then Range1l(10L, Int64.MaxValue)]
        let s1 = RangeSet64.ofList init
        let s2 = RangeSet1l.ofList init

        let intersect (r : Range1l) =
            let r1 = s1 |> RangeSet64.window r
            let r2 = s2 |> RangeSet1l.intersect r

            // Old implementation broken for MaxValue!
            if not (hasMaxValue || testMaxValue) then
                (RangeSet64.toList r1) |> should equal (RangeSet1l.toList r2)

            RangeSet1l.toList r2

        if testMaxValue then
            Range1l(2L, Int64.MaxValue)   |> intersect |> should equal [ Range1l(2L, 4L); Range1l(6L, 8L); if hasMaxValue then Range1l(10L, Int64.MaxValue) ]
            Range1l(5L, Int64.MaxValue)   |> intersect |> should equal [ Range1l(6L, 8L); if hasMaxValue then Range1l(10L, Int64.MaxValue) ]
            Range1l(1L, Int64.MaxValue)   |> intersect |> should equal [ Range1l(1L, 4L); Range1l(6L, 8L); if hasMaxValue then Range1l(10L, Int64.MaxValue) ]
            Range1l(4L, Int64.MaxValue)   |> intersect |> should equal [ Range1l(4L, 4L); Range1l(6L, 8L); if hasMaxValue then Range1l(10L, Int64.MaxValue) ]
            Range1l(-3L, Int64.MaxValue)  |> intersect |> should equal init
            Range1l(-42L, Int64.MaxValue) |> intersect |> should equal init
        else
            Range1l(2L, 3L)    |> intersect |> should equal [ Range1l(2L, 3L) ]
            Range1l(5L, 6L)    |> intersect |> should equal [ Range1l(6L, 6L) ]
            Range1l(-3L, 8L)   |> intersect |> should equal [ Range1l(-3L, -2L); Range1l(1L, 4L); Range1l(6L, 8L) ]
            Range1l(-42L, 42L) |> intersect |> should equal [ Range1l(-3L, -2L); Range1l(1L, 4L); Range1l(6L, 8L); if hasMaxValue then Range1l(10L, 42L) ]
            Range1l(-3L, -2L)  |> intersect |> should equal [ Range1l(-3L, -2L) ]
            Range1l(-2L, -1L)  |> intersect |> should equal [ Range1l(-2L, -2L); ]
            Range1l(-2L, 1L)   |> intersect |> should equal [ Range1l(-2L, -2L); Range1l(1L, 1L) ]
            Range1l(-3L, 3L)   |> intersect |> should equal [ Range1l(-3L, -2L); Range1l(1L, 3L) ]
            Range1l(-2L, 8L)   |> intersect |> should equal [ Range1l(-2L, -2L); Range1l(1L, 4L); Range1l(6L, 8L) ]

    [<Theory>]
    let ``[RangeSet] Enumerator`` (maxValue : bool) =
        let init = [ Range1l(-3L, -2L); Range1l(1L, 4L); if maxValue then Range1l(6L, Int64.MaxValue) ]
        let s1 = RangeSet64.ofList init
        let s2 = RangeSet1l.ofList init

        let expected = Seq.ofList init
        s1 :> seq<_> |> should equal (s2 :> seq<_>)
        s2 :> seq<_> |> should equal expected

    [<Theory>]
    let ``[RangeSet] Equality`` (maxValue : bool) =
        let init = [ Range1l(-3L, -2L); Range1l(1L, 4L); Range1l(6L, 8L); if maxValue then Range1l(34L, Int64.MaxValue) ]
        let mutable a1 = RangeSet64.ofList init
        let mutable b1 = RangeSet64.ofList init
        let mutable a2 = RangeSet1l.ofList init
        let mutable b2 = RangeSet1l.ofList init

        (a1 = b1) |> should be True
        (a2 = b2) |> should be True

    [<Test>]
    let ``[RangeSet] ofList`` () =
        let rnd = RandomSystem(0)

        let ranges =
            List.init 5 (fun _ ->
                let mutable l = Int64.MaxValue
                let mutable r = Int64.MinValue

                while l > r do
                    l <- rnd.UniformLong()
                    r <- l + rnd.UniformLong(100000L)

                Range1l(l, r)
            )

        let expected = (RangeSet1l.empty, ranges) ||> List.fold (fun s r -> RangeSet1l.add r s)
        let actual = RangeSet1l.ofList ranges
        actual |> should equal expected


module RangeSetBenchmarks =
    open BenchmarkDotNet.Attributes;

    [<MemoryDiagnoser>]
    type ``RangeSets``() =
        let rnd = RandomSystem(0)

        let mutable setOld = RangeSet64.empty
        let mutable setNew = RangeSet1l.empty
        let mutable ranges = List.empty
        let mutable ranges2 = List.empty

        [<DefaultValue; Params(10, 100, 1000, 10000, 100000)>]
        val mutable Count : int

        member private x.RandomRange() =
            let mutable l = Int64.MaxValue
            let mutable r = Int64.MinValue

            while l > r do
                l <- rnd.UniformLong()
                r <- l + rnd.UniformLong(100000L)

            Range1l(l, r)

        member private x.Generate(count : int) =
            List.init count (ignore >> x.RandomRange)

        [<GlobalSetup>]
        member x.Init() =
            ranges <- x.Generate x.Count
            ranges2 <- x.Generate x.Count
            setOld <- RangeSet64.ofList ranges
            setNew <- RangeSet1l.ofList ranges


        [<Benchmark(Description = "OfList (Old)")>]
        member x.OfListOld() =
            RangeSet64.ofList ranges

        [<Benchmark(Description = "OfList (New)")>]
        member x.OfListNew() =
            RangeSet1l.ofList ranges


        [<Benchmark(Description = "Insert (Old)")>]
        member x.InsertOld() =
            let mutable set = RangeSet64.empty

            for r in ranges do
                set <- set |> RangeSet64.insert r

            set

        [<Benchmark(Description = "Insert (New)")>]
        member x.InsertNew() =
            let mutable set = RangeSet1l.empty

            for r in ranges do
                set <- set |> RangeSet1l.add r

            set


        [<Benchmark(Description = "Remove (Old)")>]
        member x.RemoveOld() =
            for r in ranges2 do
                setOld <- setOld |> RangeSet64.remove r

            setOld

        [<Benchmark(Description = "Remove (New)")>]
        member x.RemoveNew() =
            for r in ranges2 do
                setNew <- setNew |> RangeSet1l.remove r

            setNew


        [<Benchmark(Description = "Iterate (Old)")>]
        member x.IterateOld() =
            let mutable cnt = Int64.MinValue

            for r in setOld do
                cnt <- cnt + r.Min

            cnt

        [<Benchmark(Description = "Iterate (New)")>]
        member x.IterateNew() =
            let mutable cnt = Int64.MinValue

            for r in setNew do
                cnt <- cnt + r.Min

            cnt