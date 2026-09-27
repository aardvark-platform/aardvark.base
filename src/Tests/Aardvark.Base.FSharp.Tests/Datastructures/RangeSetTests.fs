namespace Aardvark.Base.FSharp.Tests

open System
open Aardvark.Base

module RangeSetTests =
    open FsUnit
    open NUnit.Framework

    [<Theory>]
    let ``[RangeSet] Insert`` (maxValue : bool) (mergeWithMaxValue : bool) =
        let mutable s2 = RangeSet1l.empty

        let add (r : Range1l) =
            s2 <- s2 |> RangeSet1l.add r

        let equal (expected : Range1l list) =
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
        let mutable s2 = RangeSet1l.ofList init

        let rem (r : Range1l) =
            s2 <- s2 |> RangeSet1l.remove r

        let equal (expected : Range1l list) =
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
        let mutable s2 = RangeSet1l.ofList init
        RangeSet1l.toList s2 |> should equal init

    [<Theory>]
    let ``[RangeSet] ToArray`` (maxValue : bool) =
        let init = [ Range1l(0L, 4L); Range1l(6L, 8L); if maxValue then Range1l(10L, Int64.MaxValue) ]
        let mutable s2 = RangeSet1l.ofList init
        RangeSet1l.toArray s2 |> should equal (Array.ofList init)

    [<Theory>]
    let ``[RangeSet] ToSeq`` (maxValue : bool) =
        let init = [ Range1l(0L, 4L); Range1l(6L, 8L); if maxValue then Range1l(10L, Int64.MaxValue) ]
        let mutable s2 = RangeSet1l.ofList init
        RangeSet1l.toSeq s2 |> Seq.toList |> should equal init

    [<Theory>]
    let ``[RangeSet] Min / Max / Range`` (maxValue : bool) =
        let init = [ Range1l(-3L, -2L); Range1l(1L, 4L); Range1l(6L, 8L); if maxValue then Range1l(10L, Int64.MaxValue) ]
        let min = init |> List.map (fun r -> r.Min) |> List.min
        let max = init |> List.map (fun r -> r.Max) |> List.max

        let mutable s2 = RangeSet1l.ofList init

        s2.Min |> should equal min
        s2.Max |> should equal max
        s2.Range |> should equal (Range1l(min, max))

    [<Theory>]
    let ``[RangeSet] Intersect`` (hasMaxValue : bool) (testMaxValue : bool) =
        let init = [ Range1l(-3L, -2L); Range1l(1L, 4L); Range1l(6L, 8L); if hasMaxValue then Range1l(10L, Int64.MaxValue)]
        let s2 = RangeSet1l.ofList init

        let intersect (r : Range1l) =
            let r2 = s2 |> RangeSet1l.intersect r
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
        let s2 = RangeSet1l.ofList init

        let expected = Seq.ofList init
        s2 :> seq<_> |> should equal expected

    [<Theory>]
    let ``[RangeSet] Equality`` (maxValue : bool) =
        let init = [ Range1l(-3L, -2L); Range1l(1L, 4L); Range1l(6L, 8L); if maxValue then Range1l(34L, Int64.MaxValue) ]
        let mutable a2 = RangeSet1l.ofList init
        let mutable b2 = RangeSet1l.ofList init
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

    [<Theory>]
    let ``[RangeSet] Contains Range`` (maxValue : bool) =
        let set = RangeSet1l.ofList [ Range1l(-3L, -2L); Range1l(1L, 4L); Range1l(6L, 8L); Range1l(10L, if maxValue then Int64.MaxValue else 30L)]

        set |> RangeSet1l.containsRange (Range1l(-3L, -2L)) |> should be True
        set |> RangeSet1l.containsRange (Range1l(-3L, -3L)) |> should be True
        set |> RangeSet1l.containsRange (Range1l(-3L, -2L)) |> should be True
        set |> RangeSet1l.containsRange (Range1l(-2L, -2L)) |> should be True

        set |> RangeSet1l.containsRange (Range1l(2L, 3L)) |> should be True
        set |> RangeSet1l.containsRange (Range1l(3L, 3L)) |> should be True

        set |> RangeSet1l.containsRange (Range1l(-2L, 3L)) |> should be False

        set |> RangeSet1l.containsRange (Range1l(20L, Int64.MaxValue)) |> should equal maxValue

module RangeSetConstructionTests =
    open NUnit.Framework

    // The oracle enumerates individual values; it does not sort interval endpoints.
    let private smallUnion (ranges : (bigint * bigint)[]) =
        let values = ranges |> Seq.collect (fun (l, r) -> seq { l .. r }) |> Set.ofSeq
        let result = ResizeArray<bigint * bigint>()
        for value in values do
            if result.Count > 0 && snd result.[result.Count - 1] + 1I = value then
                let l, _ = result.[result.Count - 1]
                result.[result.Count - 1] <- (l, value)
            else
                result.Add(value, value)
        result.ToArray()

    let inline private verify minimum maximum toValue toRange fromRange (empty : ^S)
                              ofList ofArray ofSeq constructor (pairs : (bigint * bigint)[]) (expected : (bigint * bigint)[]) =
        let ranges : ^R[] = Array.map toRange pairs
        let original = Array.copy ranges
        let mutable enumerations = 0
        let mutable disposals = 0
        let input = seq {
            enumerations <- enumerations + 1
            if enumerations <> 1 then failwith "sequence was enumerated twice"
            try yield! ranges
            finally disposals <- disposals + 1
        }
        let actual : ^S =
            match constructor with
            | "list" -> ofList (Array.toList ranges)
            | "array" -> ofArray ranges
            | "seq" -> ofSeq input
            | _ -> failwith "unknown constructor"
        if constructor = "seq" then
            Assert.That(enumerations, Is.EqualTo(1))
            Assert.That(disposals, Is.EqualTo(1))
        Assert.That(ranges, Is.EqualTo(original), "caller array changed")
        let added = original |> Array.fold (fun (s : ^S) r -> (^S : (member Add : ^R -> ^S) (s, r))) empty
        Assert.That(actual, Is.EqualTo(added), "bulk versus repeated Add")
        Assert.That(actual.GetHashCode(), Is.EqualTo(added.GetHashCode()))
        // Mutating caller storage after construction must not affect the immutable set either.
        if ranges.Length > 0 then ranges.[0] <- toRange (maximum, minimum)
        let array = (^S : (member ToArray : unit -> ^R[]) actual) |> Array.map fromRange
        let list = (^S : (member ToList : unit -> ^R list) actual) |> List.map fromRange |> List.toArray
        let enumerated = (actual :> seq<^R>) |> Seq.map fromRange |> Seq.toArray
        Assert.That(array, Is.EqualTo(expected), "ToArray")
        Assert.That(list, Is.EqualTo(expected), "ToList")
        Assert.That(enumerated, Is.EqualTo(expected), "enumeration")
        Assert.That((^S : (member Count : int) actual), Is.EqualTo(expected.Length))
        Assert.That((^S : (member IsEmpty : bool) actual), Is.EqualTo(expected.Length = 0))
        let expectedMin = if expected.Length = 0 then maximum else fst expected.[0]
        let expectedMax = if expected.Length = 0 then minimum else snd expected.[expected.Length - 1]
        Assert.That((^S : (member Min : ^V) actual), Is.EqualTo(toValue expectedMin))
        Assert.That((^S : (member Max : ^V) actual), Is.EqualTo(toValue expectedMax))
        let probes =
            seq {
                yield minimum; yield maximum
                yield! seq { 0I .. 35I }
                for l, r in pairs do
                    yield l; yield r
                    if l > minimum then yield l - 1I
                    if r < maximum then yield r + 1I
            }
        for p in probes do
            if p >= minimum && p <= maximum then
                let contains = expected |> Array.exists (fun (l, r) -> l <= p && p <= r)
                Assert.That((^S : (member Contains : ^V -> bool) (actual, toValue p)), Is.EqualTo(contains), string p)

    let private check kind constructor pairs expected =
        match kind with
        | "i" ->
            verify (bigint Int32.MinValue) (bigint Int32.MaxValue) int
                (fun (l, r) -> Range1i(int l, int r)) (fun (r : Range1i) -> bigint r.Min, bigint r.Max)
                RangeSet1i.empty RangeSet1i.ofList RangeSet1i.ofArray RangeSet1i.ofSeq constructor pairs expected
        | "ui" ->
            verify 0I (bigint UInt32.MaxValue) uint32
                (fun (l, r) -> Range1ui(uint32 l, uint32 r)) (fun (r : Range1ui) -> bigint r.Min, bigint r.Max)
                RangeSet1ui.empty RangeSet1ui.ofList RangeSet1ui.ofArray RangeSet1ui.ofSeq constructor pairs expected
        | "l" ->
            verify (bigint Int64.MinValue) (bigint Int64.MaxValue) int64
                (fun (l, r) -> Range1l(int64 l, int64 r)) (fun (r : Range1l) -> bigint r.Min, bigint r.Max)
                RangeSet1l.empty RangeSet1l.ofList RangeSet1l.ofArray RangeSet1l.ofSeq constructor pairs expected
        | "ul" ->
            verify 0I (bigint UInt64.MaxValue) uint64
                (fun (l, r) -> Range1ul(uint64 l, uint64 r)) (fun (r : Range1ul) -> bigint r.Min, bigint r.Max)
                RangeSet1ul.empty RangeSet1ul.ofList RangeSet1ul.ofArray RangeSet1ul.ofSeq constructor pairs expected
        | _ -> failwith "unknown type"

    let private limits kind =
        match kind with
        | "i" -> bigint Int32.MinValue, bigint Int32.MaxValue
        | "ui" -> 0I, bigint UInt32.MaxValue
        | "l" -> bigint Int64.MinValue, bigint Int64.MaxValue
        | "ul" -> 0I, bigint UInt64.MaxValue
        | _ -> failwith "unknown type"

    [<Test>]
    let ``Empty and singleton construction`` ([<Values("i", "ui", "l", "ul")>] kind) ([<Values("list", "array", "seq")>] constructor) =
        check kind constructor [||] [||]
        check kind constructor [| 0I, 0I |] [| 0I, 0I |]
        check kind constructor [| 4I, 9I |] [| 4I, 9I |]

    [<Test>]
    let ``Inverted and Invalid construction`` ([<Values("i", "ui", "l", "ul")>] kind) ([<Values("list", "array", "seq")>] constructor) =
        let low, high = limits kind
        for invalid in [| high, low; 3I, 2I |] do
            check kind constructor [| invalid |] [||]
            check kind constructor [| invalid; invalid |] [||]
            check kind constructor [| invalid; 0I, 1I; invalid |] [| 0I, 1I |]

    [<Test>]
    let ``Order-independent closed interval union`` ([<Values("i", "ui", "l", "ul")>] kind) ([<Values("list", "array", "seq")>] constructor) =
        let cases = [|
            [| 0I, 0I; 1I, 1I |]
            [| 0I, 0I; 1I, 1I; 2I, 3I; 4I, 4I |]
            [| 2I, 7I; 2I, 7I; 2I, 7I |]
            [| 0I, 12I; 2I, 4I; 6I, 10I |]
            [| 0I, 2I; 7I, 10I; 14I, 15I |]
            [| 0I, 3I; 2I, 8I; 8I, 10I; 12I, 15I |]
            [| 2I, 1I; 4I, 3I; 0I, 0I; 1I, 1I |]
        |]
        for input in cases do
            let expected = smallUnion input
            check kind constructor input expected
            check kind constructor (Array.rev input) expected
            for shift in 1 .. input.Length - 1 do
                check kind constructor (Array.append input.[shift..] input.[..shift-1]) expected

    [<Test>]
    let ``Extreme endpoints and terminal adjacency`` ([<Values("i", "ui", "l", "ul")>] kind) ([<Values("list", "array", "seq")>] constructor) =
        let low, high = limits kind
        let cases = [|
            [| high, high |], [| high, high |]
            [| low, low |], [| low, low |]
            [| low, high |], [| low, high |]
            [| low, low; low + 1I, low + 1I |], [| low, low + 1I |]
            [| high - 1I, high - 1I; high, high |], [| high - 1I, high |]
            [| high - 4I, high - 3I; high - 2I, high - 1I; high, high |], [| high - 4I, high |]
            [| high, high; high, high |], [| high, high |]
            [| 0I, 0I; high - 2I, high - 1I; high, high |], [| 0I, 0I; high - 2I, high |]
            [| low, low; high, high |], [| low, low; high, high |]
            [| low, high; high, high; high, low |], [| low, high |]
        |]
        for input, expected in cases do
            check kind constructor input expected
            check kind constructor (Array.rev input) expected

    [<Test>]
    let ``Seeded small-domain union oracle`` ([<Values("i", "ui", "l", "ul")>] kind) ([<Values("list", "array", "seq")>] constructor) =
        let low, _ = limits kind
        let random = Random(591273)
        for iteration in 0 .. 299 do
            let shift = if low < 0I then -16I else 0I
            let input = Array.init (random.Next(0, 30)) (fun _ -> bigint (random.Next(32)) + shift, bigint (random.Next(32)) + shift)
            let expected = smallUnion input
            check kind constructor input expected
            check kind constructor (Array.rev input) expected
