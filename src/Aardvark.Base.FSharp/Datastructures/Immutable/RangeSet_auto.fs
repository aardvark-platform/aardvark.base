namespace Aardvark.Base

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base

type internal HalfRangeKind =
    | Left = 0
    | Right = 1

module private RangeSetUtils =

    let inline inc (value : 'T) =
        let res = value + LanguagePrimitives.GenericOne<'T>
        if res = Constant<'T>.ParseableMinValue then struct (Constant<'T>.ParseableMaxValue, true)
        else struct (res, false)

    module MapExt =
        let inline splitAt (key : 'K) (map : MapExt<'K, 'V>) =
            let struct (l, _, _, _, r) = MapExt.splitV key map
            struct (l, r)

        let inline tryMinValue (map : MapExt<'K, 'V>) =
            MapExt.tryMinV map |> ValueOption.map (fun mk -> map.[mk])

        let inline tryMaxValue (map : MapExt<'K, 'V>) =
            MapExt.tryMaxV map |> ValueOption.map (fun mk -> map.[mk])

        let inline maxValue (map : MapExt<'K, 'V>) =
            map.[MapExt.max map]

open RangeSetUtils

/// Set of ranges where overlapping and neighboring ranges are coalesced.
/// Note that ranges describe closed intervals.
[<StructuredFormatDisplay("{AsString}")>]
type RangeSet1i internal (store : MapExt<int32, HalfRangeKind>) =
    static let empty = RangeSet1i(MapExt.empty)

    /// Empty range set.
    static member Empty = empty

    member inline private x.Store = store

    // We cannot directly describe a range that ends at Int32.MaxValue since the right half-range is inserted
    // at max + 1. In that case the right-half range will be missing and the total count is odd.
    member inline private x.HasMaxValue = store.Count % 2 = 1

    /// Returns the minimum value in the range set or Int32.MaxValue if the range is empty.
    member x.Min =
        match store.TryMinKeyV with
        | ValueSome min -> min
        | _ -> Int32.MaxValue

    /// Returns the maximum value in the range set or Int32.MinValue if the range is empty.
    member x.Max =
        if x.HasMaxValue then Int32.MaxValue
        else
            match store.TryMaxKeyV with
            | ValueSome max -> max - 1
            | _ -> Int32.MinValue

    /// Returns the total range spanned by the range set, i.e. [min, max].
    member inline x.Range =
        Range1i(x.Min, x.Max)

    /// Adds the given range to the set.
    member x.Add(r : Range1i) =
        if r.Max < r.Min then
            x
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let struct (lm, inner) = MapExt.splitAt min store
            let rm =
                if overflow then MapExt.empty
                else sndv <| MapExt.splitAt max inner

            let before = MapExt.tryMaxValue lm
            let after = MapExt.tryMinValue rm

            // If the set contained Int32.MaxValue or we have overflown, we must not add an explicit right half-range.
            // Int32.MaxValue is stored implicitly.
            let fixRightBoundary =
                if x.HasMaxValue || overflow then
                    id
                else
                    MapExt.add max HalfRangeKind.Right

            let newStore =
                match before, after with
                | ValueNone, ValueNone ->
                    MapExt.ofListV [
                        struct (min, HalfRangeKind.Left)
                    ]
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Right, ValueNone ->
                    lm
                    |> MapExt.add min HalfRangeKind.Left
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Left, ValueNone ->
                    lm
                    |> fixRightBoundary

                | ValueNone, ValueSome HalfRangeKind.Left ->
                    rm
                    |> MapExt.add min HalfRangeKind.Left
                    |> MapExt.add max HalfRangeKind.Right

                | ValueNone, ValueSome HalfRangeKind.Right ->
                    rm
                    |> MapExt.add min HalfRangeKind.Left

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Left); struct (max, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (max, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Right ->
                    MapExt.union lm rm

                | _ ->
                    failwithf "impossible"

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1i(newStore)

    /// Removes the given range from the set.
    member x.Remove(r : Range1i) =
        if r.Max < r.Min then
            x
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let struct (lm, inner) = MapExt.splitAt min store
            let rm =
                if overflow then MapExt.empty
                else sndv <| MapExt.splitAt max inner

            let before = MapExt.tryMaxValue lm
            let after = MapExt.tryMinValue rm

            // If the set contained Int32.MaxValue and we have not overflown, there is still a range [max, Int32.MaxValue]
            let fixRightBoundary =
                if x.HasMaxValue && not overflow then
                    MapExt.add max HalfRangeKind.Left
                else
                    id

            let newStore =
                match before, after with
                | ValueNone, ValueNone ->
                    MapExt.empty
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Right, ValueNone ->
                    lm
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Left, ValueNone ->
                    lm
                    |> MapExt.add min HalfRangeKind.Right
                    |> fixRightBoundary

                | ValueNone, ValueSome HalfRangeKind.Left ->
                    rm

                | ValueNone, ValueSome HalfRangeKind.Right ->
                    rm
                    |> MapExt.add max HalfRangeKind.Left

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Left ->
                    MapExt.union lm rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (max, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Right); struct (max, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | _ ->
                    failwithf "impossible"

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1i(newStore)

    /// Returns the union of the set with the given set.
    member inline x.Union(other : RangeSet1i) =
        let mutable res = x
        for r in other do
            res <- res.Add r
        res

    /// Returns the intersection of the set with the given range.
    member x.Intersect(r : Range1i) =
        if r.Max < r.Min then
            empty
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let inner =
                store
                |> MapExt.splitAt min |> sndv
                |> if not overflow then MapExt.splitAt max >> fstv else id

            let newStore =
                inner
                |> if x.Contains r.Min then MapExt.add min HalfRangeKind.Left else id
                |> if x.Contains r.Max && not overflow then MapExt.add max HalfRangeKind.Right else id

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1i(newStore)

    member private x.TryFindLeftBoundary(v : int32) =
        let struct (l, s, _) = MapExt.neighboursV v store
        match s with
        | ValueSome (i, k) -> if k = HalfRangeKind.Left then ValueSome i else ValueNone
        | _ ->
            match l with
            | ValueSome (i, HalfRangeKind.Left) -> ValueSome i
            | _ -> ValueNone

    /// Returns whether the given value is contained in the range set.
    member x.Contains(v : int32) =
        x.TryFindLeftBoundary v |> ValueOption.isSome

    /// Returns whether the given range is contained in the set.
    member x.Contains(r : Range1i) =
        if r.Max < r.Min then false
        elif r.Min = r.Max then x.Contains r.Min
        else
            match x.TryFindLeftBoundary r.Min, x.TryFindLeftBoundary r.Max with
            | ValueSome l, ValueSome r -> l = r
            | _ -> false

    /// Returns the number of disjoint ranges in the set.
    member x.Count =
        (store.Count + 1) / 2

    /// Returns whether the set is empty.
    member inline x.IsEmpty =
        x.Count = 0

    /// Builds an array from the range set.
    member x.ToArray() =
        let arr = Array.zeroCreate x.Count

        let rec write (i : int) (l : struct (int32 * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                arr.[i] <- Range1i(l, r - 1)
                write (i + 1) rest

            | [struct (l, HalfRangeKind.Left)] when i = x.Count - 1 ->
                arr.[i] <- Range1i(l, Int32.MaxValue)

            | [_] -> failwith "bad RangeSet"

            | [] -> ()

        store |> MapExt.toListV |> write 0
        arr

    /// Builds a list from the range set.
    member x.ToList() =
        let rec build (accum : Range1i list) (l : struct (int32 * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                build (Range1i(l, r - 1) :: accum) rest

            | [struct (l, HalfRangeKind.Left)] ->
                build (Range1i(l, Int32.MaxValue) :: accum) []

            | [_] -> failwith "bad RangeSet"

            | [] -> List.rev accum

        store |> MapExt.toListV |> build []

    /// Views the range set as a sequence.
    member x.ToSeq() =
        x :> seq<_>

    member inline private x.Equals(other : RangeSet1i) =
        store = other.Store

    override x.Equals(other : obj) =
        match other with
        | :? RangeSet1i as o -> x.Equals o
        | _ -> false

    override x.GetHashCode() =
        store.GetHashCode()

    member private x.AsString = x.ToString()

    override x.ToString() =
        let content =
            x |> Seq.map (fun r ->
                $"[{r.Min}, {r.Max}]"
            )
            |> String.concat "; "

        $"ranges [{content}]"

    member x.GetEnumerator() =
        new RangeSetEnumerator1i((store :> seq<_>).GetEnumerator())

    interface IEquatable<RangeSet1i> with
        member x.Equals(other) = x.Equals(other)

    interface IEnumerable with
        member x.GetEnumerator() = new RangeSetEnumerator1i((store :> seq<_>).GetEnumerator()) :> _

    interface IEnumerable<Range1i> with
        member x.GetEnumerator() = new RangeSetEnumerator1i((store :> seq<_>).GetEnumerator()) :> _

// TODO: MapExt should use a struct enumerator and return it directly.
// That way we could get rid of allocations.
and RangeSetEnumerator1i =
    struct
        val private Inner : IEnumerator<KeyValuePair<int32, HalfRangeKind>>
        val mutable private Left : KeyValuePair<int32, HalfRangeKind>
        val mutable private Right : KeyValuePair<int32, HalfRangeKind>

        internal new (inner : IEnumerator<KeyValuePair<int32, HalfRangeKind>>) =
            { Inner = inner
              Left = Unchecked.defaultof<_>
              Right = Unchecked.defaultof<_> }

        member x.MoveNext() =
            if x.Inner.MoveNext() then
                x.Left <- x.Inner.Current
                if x.Inner.MoveNext() then
                    x.Right <- x.Inner.Current
                    true
                else
                    if x.Left.Value = HalfRangeKind.Left then
                        x.Right <- KeyValuePair(Int32.MinValue, HalfRangeKind.Right) // MaxValue + 1
                        true
                    else
                        failwithf "bad RangeSet"
            else
                false

        member x.Reset() =
            x.Inner.Reset()
            x.Left <- Unchecked.defaultof<_>
            x.Right <- Unchecked.defaultof<_>

        member x.Current =
            assert (x.Left.Value = HalfRangeKind.Left && x.Right.Value = HalfRangeKind.Right)
            Range1i(x.Left.Key, x.Right.Key - 1)

        member x.Dispose() =
            x.Inner.Dispose()
            x.Left <- Unchecked.defaultof<_>
            x.Right <- Unchecked.defaultof<_>

        interface IEnumerator with
            member x.MoveNext() = x.MoveNext()
            member x.Current = x.Current :> obj
            member x.Reset() = x.Reset()

        interface IEnumerator<Range1i> with
            member x.Dispose() = x.Dispose()
            member x.Current = x.Current
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RangeSet1i =

    /// Empty range set.
    let empty = RangeSet1i.Empty

    /// Returns the minimum value in the range set or Int32.MaxValue if the range is empty.
    let inline min (set : RangeSet1i) = set.Min

    /// Returns the maximum value in the range set or Int32.MinValue if the range is empty.
    let inline max (set : RangeSet1i) = set.Max

    /// Returns the total range spanned by the range set, i.e. [min, max].
    let inline range (set : RangeSet1i) = set.Range

    let inline private getHalfRanges (r : Range1i) =
        [ struct (r.Min, HalfRangeKind.Left)
          if r.Max < Int32.MaxValue then struct (r.Max + 1, HalfRangeKind.Right) ]

    let inline private ofRange (r : Range1i) =
        RangeSet1i(MapExt.ofListV <| getHalfRanges r)

    let private ofRanges (ranges : seq<Range1i>) =
        let halves =
            ranges
            |> Seq.toList
            |> List.collect getHalfRanges
            |> List.sortBy fstv

        let mutable level = 0
        let result = ResizeArray()

        for (struct (i, k) as h) in halves do
            if k = HalfRangeKind.Left then
                if level = 0 then result.Add h
                level <- level + 1
            else
                level <- level - 1
                if level = 0 then result.Add h

        RangeSet1i(MapExt.ofSeqV result)

    /// Builds a range set of the given list of ranges.
    let ofList (ranges : Range1i list) =
        match ranges with
        | [] -> empty
        | [r] -> ofRange r
        | _ -> ofRanges ranges

    /// Builds a range set of the given array of ranges.
    let ofArray (ranges : Range1i[]) =
        if ranges.Length = 0 then empty
        elif ranges.Length = 1 then ofRange ranges.[0]
        else ofRanges ranges

    /// Builds a range set of the given sequence of ranges.
    let inline ofSeq (ranges : seq<Range1i>) =
        ofList <| Seq.toList ranges

    /// Adds the given range to the set.
    let inline add (range : Range1i) (set : RangeSet1i) = set.Add range

    /// Removes the given range from the set.
    let inline remove (range : Range1i) (set : RangeSet1i) = set.Remove range

    /// Returns the union of two sets.
    let inline union (l : RangeSet1i) (r : RangeSet1i) = l.Union r

    /// Returns the intersection of the set with the given range.
    let inline intersect (range : Range1i) (set : RangeSet1i) = set.Intersect range

    /// Returns whether the given value is contained in the range set.
    let inline contains (value : int32) (set : RangeSet1i) = set.Contains value

    /// Returns whether the given range is contained in the set.
    let inline containsRange (range : Range1i) (set : RangeSet1i) = set.Contains range

    /// Returns the number of disjoint ranges in the set.
    let inline count (set : RangeSet1i) = set.Count

    /// Returns whether the set is empty.
    let inline isEmpty (set : RangeSet1i) = set.IsEmpty

    /// Views the range set as a sequence.
    let inline toSeq (set : RangeSet1i) = set :> seq<_>

    /// Builds a list from the range set.
    let inline toList (set : RangeSet1i) = set.ToList()

    /// Builds an array from the range set.
    let inline toArray (set : RangeSet1i) = set.ToArray()


/// Set of ranges where overlapping and neighboring ranges are coalesced.
/// Note that ranges describe closed intervals.
[<StructuredFormatDisplay("{AsString}")>]
type RangeSet1ui internal (store : MapExt<uint32, HalfRangeKind>) =
    static let empty = RangeSet1ui(MapExt.empty)

    /// Empty range set.
    static member Empty = empty

    member inline private x.Store = store

    // We cannot directly describe a range that ends at UInt32.MaxValue since the right half-range is inserted
    // at max + 1. In that case the right-half range will be missing and the total count is odd.
    member inline private x.HasMaxValue = store.Count % 2 = 1

    /// Returns the minimum value in the range set or UInt32.MaxValue if the range is empty.
    member x.Min =
        match store.TryMinKeyV with
        | ValueSome min -> min
        | _ -> UInt32.MaxValue

    /// Returns the maximum value in the range set or UInt32.MinValue if the range is empty.
    member x.Max =
        if x.HasMaxValue then UInt32.MaxValue
        else
            match store.TryMaxKeyV with
            | ValueSome max -> max - 1u
            | _ -> UInt32.MinValue

    /// Returns the total range spanned by the range set, i.e. [min, max].
    member inline x.Range =
        Range1ui(x.Min, x.Max)

    /// Adds the given range to the set.
    member x.Add(r : Range1ui) =
        if r.Max < r.Min then
            x
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let struct (lm, inner) = MapExt.splitAt min store
            let rm =
                if overflow then MapExt.empty
                else sndv <| MapExt.splitAt max inner

            let before = MapExt.tryMaxValue lm
            let after = MapExt.tryMinValue rm

            // If the set contained UInt32.MaxValue or we have overflown, we must not add an explicit right half-range.
            // UInt32.MaxValue is stored implicitly.
            let fixRightBoundary =
                if x.HasMaxValue || overflow then
                    id
                else
                    MapExt.add max HalfRangeKind.Right

            let newStore =
                match before, after with
                | ValueNone, ValueNone ->
                    MapExt.ofListV [
                        struct (min, HalfRangeKind.Left)
                    ]
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Right, ValueNone ->
                    lm
                    |> MapExt.add min HalfRangeKind.Left
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Left, ValueNone ->
                    lm
                    |> fixRightBoundary

                | ValueNone, ValueSome HalfRangeKind.Left ->
                    rm
                    |> MapExt.add min HalfRangeKind.Left
                    |> MapExt.add max HalfRangeKind.Right

                | ValueNone, ValueSome HalfRangeKind.Right ->
                    rm
                    |> MapExt.add min HalfRangeKind.Left

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Left); struct (max, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (max, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Right ->
                    MapExt.union lm rm

                | _ ->
                    failwithf "impossible"

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1ui(newStore)

    /// Removes the given range from the set.
    member x.Remove(r : Range1ui) =
        if r.Max < r.Min then
            x
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let struct (lm, inner) = MapExt.splitAt min store
            let rm =
                if overflow then MapExt.empty
                else sndv <| MapExt.splitAt max inner

            let before = MapExt.tryMaxValue lm
            let after = MapExt.tryMinValue rm

            // If the set contained UInt32.MaxValue and we have not overflown, there is still a range [max, UInt32.MaxValue]
            let fixRightBoundary =
                if x.HasMaxValue && not overflow then
                    MapExt.add max HalfRangeKind.Left
                else
                    id

            let newStore =
                match before, after with
                | ValueNone, ValueNone ->
                    MapExt.empty
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Right, ValueNone ->
                    lm
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Left, ValueNone ->
                    lm
                    |> MapExt.add min HalfRangeKind.Right
                    |> fixRightBoundary

                | ValueNone, ValueSome HalfRangeKind.Left ->
                    rm

                | ValueNone, ValueSome HalfRangeKind.Right ->
                    rm
                    |> MapExt.add max HalfRangeKind.Left

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Left ->
                    MapExt.union lm rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (max, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Right); struct (max, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | _ ->
                    failwithf "impossible"

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1ui(newStore)

    /// Returns the union of the set with the given set.
    member inline x.Union(other : RangeSet1ui) =
        let mutable res = x
        for r in other do
            res <- res.Add r
        res

    /// Returns the intersection of the set with the given range.
    member x.Intersect(r : Range1ui) =
        if r.Max < r.Min then
            empty
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let inner =
                store
                |> MapExt.splitAt min |> sndv
                |> if not overflow then MapExt.splitAt max >> fstv else id

            let newStore =
                inner
                |> if x.Contains r.Min then MapExt.add min HalfRangeKind.Left else id
                |> if x.Contains r.Max && not overflow then MapExt.add max HalfRangeKind.Right else id

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1ui(newStore)

    member private x.TryFindLeftBoundary(v : uint32) =
        let struct (l, s, _) = MapExt.neighboursV v store
        match s with
        | ValueSome (i, k) -> if k = HalfRangeKind.Left then ValueSome i else ValueNone
        | _ ->
            match l with
            | ValueSome (i, HalfRangeKind.Left) -> ValueSome i
            | _ -> ValueNone

    /// Returns whether the given value is contained in the range set.
    member x.Contains(v : uint32) =
        x.TryFindLeftBoundary v |> ValueOption.isSome

    /// Returns whether the given range is contained in the set.
    member x.Contains(r : Range1ui) =
        if r.Max < r.Min then false
        elif r.Min = r.Max then x.Contains r.Min
        else
            match x.TryFindLeftBoundary r.Min, x.TryFindLeftBoundary r.Max with
            | ValueSome l, ValueSome r -> l = r
            | _ -> false

    /// Returns the number of disjoint ranges in the set.
    member x.Count =
        (store.Count + 1) / 2

    /// Returns whether the set is empty.
    member inline x.IsEmpty =
        x.Count = 0

    /// Builds an array from the range set.
    member x.ToArray() =
        let arr = Array.zeroCreate x.Count

        let rec write (i : int) (l : struct (uint32 * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                arr.[i] <- Range1ui(l, r - 1u)
                write (i + 1) rest

            | [struct (l, HalfRangeKind.Left)] when i = x.Count - 1 ->
                arr.[i] <- Range1ui(l, UInt32.MaxValue)

            | [_] -> failwith "bad RangeSet"

            | [] -> ()

        store |> MapExt.toListV |> write 0
        arr

    /// Builds a list from the range set.
    member x.ToList() =
        let rec build (accum : Range1ui list) (l : struct (uint32 * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                build (Range1ui(l, r - 1u) :: accum) rest

            | [struct (l, HalfRangeKind.Left)] ->
                build (Range1ui(l, UInt32.MaxValue) :: accum) []

            | [_] -> failwith "bad RangeSet"

            | [] -> List.rev accum

        store |> MapExt.toListV |> build []

    /// Views the range set as a sequence.
    member x.ToSeq() =
        x :> seq<_>

    member inline private x.Equals(other : RangeSet1ui) =
        store = other.Store

    override x.Equals(other : obj) =
        match other with
        | :? RangeSet1ui as o -> x.Equals o
        | _ -> false

    override x.GetHashCode() =
        store.GetHashCode()

    member private x.AsString = x.ToString()

    override x.ToString() =
        let content =
            x |> Seq.map (fun r ->
                $"[{r.Min}, {r.Max}]"
            )
            |> String.concat "; "

        $"ranges [{content}]"

    member x.GetEnumerator() =
        new RangeSetEnumerator1ui((store :> seq<_>).GetEnumerator())

    interface IEquatable<RangeSet1ui> with
        member x.Equals(other) = x.Equals(other)

    interface IEnumerable with
        member x.GetEnumerator() = new RangeSetEnumerator1ui((store :> seq<_>).GetEnumerator()) :> _

    interface IEnumerable<Range1ui> with
        member x.GetEnumerator() = new RangeSetEnumerator1ui((store :> seq<_>).GetEnumerator()) :> _

// TODO: MapExt should use a struct enumerator and return it directly.
// That way we could get rid of allocations.
and RangeSetEnumerator1ui =
    struct
        val private Inner : IEnumerator<KeyValuePair<uint32, HalfRangeKind>>
        val mutable private Left : KeyValuePair<uint32, HalfRangeKind>
        val mutable private Right : KeyValuePair<uint32, HalfRangeKind>

        internal new (inner : IEnumerator<KeyValuePair<uint32, HalfRangeKind>>) =
            { Inner = inner
              Left = Unchecked.defaultof<_>
              Right = Unchecked.defaultof<_> }

        member x.MoveNext() =
            if x.Inner.MoveNext() then
                x.Left <- x.Inner.Current
                if x.Inner.MoveNext() then
                    x.Right <- x.Inner.Current
                    true
                else
                    if x.Left.Value = HalfRangeKind.Left then
                        x.Right <- KeyValuePair(UInt32.MinValue, HalfRangeKind.Right) // MaxValue + 1
                        true
                    else
                        failwithf "bad RangeSet"
            else
                false

        member x.Reset() =
            x.Inner.Reset()
            x.Left <- Unchecked.defaultof<_>
            x.Right <- Unchecked.defaultof<_>

        member x.Current =
            assert (x.Left.Value = HalfRangeKind.Left && x.Right.Value = HalfRangeKind.Right)
            Range1ui(x.Left.Key, x.Right.Key - 1u)

        member x.Dispose() =
            x.Inner.Dispose()
            x.Left <- Unchecked.defaultof<_>
            x.Right <- Unchecked.defaultof<_>

        interface IEnumerator with
            member x.MoveNext() = x.MoveNext()
            member x.Current = x.Current :> obj
            member x.Reset() = x.Reset()

        interface IEnumerator<Range1ui> with
            member x.Dispose() = x.Dispose()
            member x.Current = x.Current
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RangeSet1ui =

    /// Empty range set.
    let empty = RangeSet1ui.Empty

    /// Returns the minimum value in the range set or UInt32.MaxValue if the range is empty.
    let inline min (set : RangeSet1ui) = set.Min

    /// Returns the maximum value in the range set or UInt32.MinValue if the range is empty.
    let inline max (set : RangeSet1ui) = set.Max

    /// Returns the total range spanned by the range set, i.e. [min, max].
    let inline range (set : RangeSet1ui) = set.Range

    let inline private getHalfRanges (r : Range1ui) =
        [ struct (r.Min, HalfRangeKind.Left)
          if r.Max < UInt32.MaxValue then struct (r.Max + 1u, HalfRangeKind.Right) ]

    let inline private ofRange (r : Range1ui) =
        RangeSet1ui(MapExt.ofListV <| getHalfRanges r)

    let private ofRanges (ranges : seq<Range1ui>) =
        let halves =
            ranges
            |> Seq.toList
            |> List.collect getHalfRanges
            |> List.sortBy fstv

        let mutable level = 0
        let result = ResizeArray()

        for (struct (i, k) as h) in halves do
            if k = HalfRangeKind.Left then
                if level = 0 then result.Add h
                level <- level + 1
            else
                level <- level - 1
                if level = 0 then result.Add h

        RangeSet1ui(MapExt.ofSeqV result)

    /// Builds a range set of the given list of ranges.
    let ofList (ranges : Range1ui list) =
        match ranges with
        | [] -> empty
        | [r] -> ofRange r
        | _ -> ofRanges ranges

    /// Builds a range set of the given array of ranges.
    let ofArray (ranges : Range1ui[]) =
        if ranges.Length = 0 then empty
        elif ranges.Length = 1 then ofRange ranges.[0]
        else ofRanges ranges

    /// Builds a range set of the given sequence of ranges.
    let inline ofSeq (ranges : seq<Range1ui>) =
        ofList <| Seq.toList ranges

    /// Adds the given range to the set.
    let inline add (range : Range1ui) (set : RangeSet1ui) = set.Add range

    /// Removes the given range from the set.
    let inline remove (range : Range1ui) (set : RangeSet1ui) = set.Remove range

    /// Returns the union of two sets.
    let inline union (l : RangeSet1ui) (r : RangeSet1ui) = l.Union r

    /// Returns the intersection of the set with the given range.
    let inline intersect (range : Range1ui) (set : RangeSet1ui) = set.Intersect range

    /// Returns whether the given value is contained in the range set.
    let inline contains (value : uint32) (set : RangeSet1ui) = set.Contains value

    /// Returns whether the given range is contained in the set.
    let inline containsRange (range : Range1ui) (set : RangeSet1ui) = set.Contains range

    /// Returns the number of disjoint ranges in the set.
    let inline count (set : RangeSet1ui) = set.Count

    /// Returns whether the set is empty.
    let inline isEmpty (set : RangeSet1ui) = set.IsEmpty

    /// Views the range set as a sequence.
    let inline toSeq (set : RangeSet1ui) = set :> seq<_>

    /// Builds a list from the range set.
    let inline toList (set : RangeSet1ui) = set.ToList()

    /// Builds an array from the range set.
    let inline toArray (set : RangeSet1ui) = set.ToArray()


/// Set of ranges where overlapping and neighboring ranges are coalesced.
/// Note that ranges describe closed intervals.
[<StructuredFormatDisplay("{AsString}")>]
type RangeSet1l internal (store : MapExt<int64, HalfRangeKind>) =
    static let empty = RangeSet1l(MapExt.empty)

    /// Empty range set.
    static member Empty = empty

    member inline private x.Store = store

    // We cannot directly describe a range that ends at Int64.MaxValue since the right half-range is inserted
    // at max + 1. In that case the right-half range will be missing and the total count is odd.
    member inline private x.HasMaxValue = store.Count % 2 = 1

    /// Returns the minimum value in the range set or Int64.MaxValue if the range is empty.
    member x.Min =
        match store.TryMinKeyV with
        | ValueSome min -> min
        | _ -> Int64.MaxValue

    /// Returns the maximum value in the range set or Int64.MinValue if the range is empty.
    member x.Max =
        if x.HasMaxValue then Int64.MaxValue
        else
            match store.TryMaxKeyV with
            | ValueSome max -> max - 1L
            | _ -> Int64.MinValue

    /// Returns the total range spanned by the range set, i.e. [min, max].
    member inline x.Range =
        Range1l(x.Min, x.Max)

    /// Adds the given range to the set.
    member x.Add(r : Range1l) =
        if r.Max < r.Min then
            x
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let struct (lm, inner) = MapExt.splitAt min store
            let rm =
                if overflow then MapExt.empty
                else sndv <| MapExt.splitAt max inner

            let before = MapExt.tryMaxValue lm
            let after = MapExt.tryMinValue rm

            // If the set contained Int64.MaxValue or we have overflown, we must not add an explicit right half-range.
            // Int64.MaxValue is stored implicitly.
            let fixRightBoundary =
                if x.HasMaxValue || overflow then
                    id
                else
                    MapExt.add max HalfRangeKind.Right

            let newStore =
                match before, after with
                | ValueNone, ValueNone ->
                    MapExt.ofListV [
                        struct (min, HalfRangeKind.Left)
                    ]
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Right, ValueNone ->
                    lm
                    |> MapExt.add min HalfRangeKind.Left
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Left, ValueNone ->
                    lm
                    |> fixRightBoundary

                | ValueNone, ValueSome HalfRangeKind.Left ->
                    rm
                    |> MapExt.add min HalfRangeKind.Left
                    |> MapExt.add max HalfRangeKind.Right

                | ValueNone, ValueSome HalfRangeKind.Right ->
                    rm
                    |> MapExt.add min HalfRangeKind.Left

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Left); struct (max, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (max, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Right ->
                    MapExt.union lm rm

                | _ ->
                    failwithf "impossible"

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1l(newStore)

    /// Removes the given range from the set.
    member x.Remove(r : Range1l) =
        if r.Max < r.Min then
            x
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let struct (lm, inner) = MapExt.splitAt min store
            let rm =
                if overflow then MapExt.empty
                else sndv <| MapExt.splitAt max inner

            let before = MapExt.tryMaxValue lm
            let after = MapExt.tryMinValue rm

            // If the set contained Int64.MaxValue and we have not overflown, there is still a range [max, Int64.MaxValue]
            let fixRightBoundary =
                if x.HasMaxValue && not overflow then
                    MapExt.add max HalfRangeKind.Left
                else
                    id

            let newStore =
                match before, after with
                | ValueNone, ValueNone ->
                    MapExt.empty
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Right, ValueNone ->
                    lm
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Left, ValueNone ->
                    lm
                    |> MapExt.add min HalfRangeKind.Right
                    |> fixRightBoundary

                | ValueNone, ValueSome HalfRangeKind.Left ->
                    rm

                | ValueNone, ValueSome HalfRangeKind.Right ->
                    rm
                    |> MapExt.add max HalfRangeKind.Left

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Left ->
                    MapExt.union lm rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (max, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Right); struct (max, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | _ ->
                    failwithf "impossible"

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1l(newStore)

    /// Returns the union of the set with the given set.
    member inline x.Union(other : RangeSet1l) =
        let mutable res = x
        for r in other do
            res <- res.Add r
        res

    /// Returns the intersection of the set with the given range.
    member x.Intersect(r : Range1l) =
        if r.Max < r.Min then
            empty
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let inner =
                store
                |> MapExt.splitAt min |> sndv
                |> if not overflow then MapExt.splitAt max >> fstv else id

            let newStore =
                inner
                |> if x.Contains r.Min then MapExt.add min HalfRangeKind.Left else id
                |> if x.Contains r.Max && not overflow then MapExt.add max HalfRangeKind.Right else id

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1l(newStore)

    member private x.TryFindLeftBoundary(v : int64) =
        let struct (l, s, _) = MapExt.neighboursV v store
        match s with
        | ValueSome (i, k) -> if k = HalfRangeKind.Left then ValueSome i else ValueNone
        | _ ->
            match l with
            | ValueSome (i, HalfRangeKind.Left) -> ValueSome i
            | _ -> ValueNone

    /// Returns whether the given value is contained in the range set.
    member x.Contains(v : int64) =
        x.TryFindLeftBoundary v |> ValueOption.isSome

    /// Returns whether the given range is contained in the set.
    member x.Contains(r : Range1l) =
        if r.Max < r.Min then false
        elif r.Min = r.Max then x.Contains r.Min
        else
            match x.TryFindLeftBoundary r.Min, x.TryFindLeftBoundary r.Max with
            | ValueSome l, ValueSome r -> l = r
            | _ -> false

    /// Returns the number of disjoint ranges in the set.
    member x.Count =
        (store.Count + 1) / 2

    /// Returns whether the set is empty.
    member inline x.IsEmpty =
        x.Count = 0

    /// Builds an array from the range set.
    member x.ToArray() =
        let arr = Array.zeroCreate x.Count

        let rec write (i : int) (l : struct (int64 * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                arr.[i] <- Range1l(l, r - 1L)
                write (i + 1) rest

            | [struct (l, HalfRangeKind.Left)] when i = x.Count - 1 ->
                arr.[i] <- Range1l(l, Int64.MaxValue)

            | [_] -> failwith "bad RangeSet"

            | [] -> ()

        store |> MapExt.toListV |> write 0
        arr

    /// Builds a list from the range set.
    member x.ToList() =
        let rec build (accum : Range1l list) (l : struct (int64 * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                build (Range1l(l, r - 1L) :: accum) rest

            | [struct (l, HalfRangeKind.Left)] ->
                build (Range1l(l, Int64.MaxValue) :: accum) []

            | [_] -> failwith "bad RangeSet"

            | [] -> List.rev accum

        store |> MapExt.toListV |> build []

    /// Views the range set as a sequence.
    member x.ToSeq() =
        x :> seq<_>

    member inline private x.Equals(other : RangeSet1l) =
        store = other.Store

    override x.Equals(other : obj) =
        match other with
        | :? RangeSet1l as o -> x.Equals o
        | _ -> false

    override x.GetHashCode() =
        store.GetHashCode()

    member private x.AsString = x.ToString()

    override x.ToString() =
        let content =
            x |> Seq.map (fun r ->
                $"[{r.Min}, {r.Max}]"
            )
            |> String.concat "; "

        $"ranges [{content}]"

    member x.GetEnumerator() =
        new RangeSetEnumerator1l((store :> seq<_>).GetEnumerator())

    interface IEquatable<RangeSet1l> with
        member x.Equals(other) = x.Equals(other)

    interface IEnumerable with
        member x.GetEnumerator() = new RangeSetEnumerator1l((store :> seq<_>).GetEnumerator()) :> _

    interface IEnumerable<Range1l> with
        member x.GetEnumerator() = new RangeSetEnumerator1l((store :> seq<_>).GetEnumerator()) :> _

// TODO: MapExt should use a struct enumerator and return it directly.
// That way we could get rid of allocations.
and RangeSetEnumerator1l =
    struct
        val private Inner : IEnumerator<KeyValuePair<int64, HalfRangeKind>>
        val mutable private Left : KeyValuePair<int64, HalfRangeKind>
        val mutable private Right : KeyValuePair<int64, HalfRangeKind>

        internal new (inner : IEnumerator<KeyValuePair<int64, HalfRangeKind>>) =
            { Inner = inner
              Left = Unchecked.defaultof<_>
              Right = Unchecked.defaultof<_> }

        member x.MoveNext() =
            if x.Inner.MoveNext() then
                x.Left <- x.Inner.Current
                if x.Inner.MoveNext() then
                    x.Right <- x.Inner.Current
                    true
                else
                    if x.Left.Value = HalfRangeKind.Left then
                        x.Right <- KeyValuePair(Int64.MinValue, HalfRangeKind.Right) // MaxValue + 1
                        true
                    else
                        failwithf "bad RangeSet"
            else
                false

        member x.Reset() =
            x.Inner.Reset()
            x.Left <- Unchecked.defaultof<_>
            x.Right <- Unchecked.defaultof<_>

        member x.Current =
            assert (x.Left.Value = HalfRangeKind.Left && x.Right.Value = HalfRangeKind.Right)
            Range1l(x.Left.Key, x.Right.Key - 1L)

        member x.Dispose() =
            x.Inner.Dispose()
            x.Left <- Unchecked.defaultof<_>
            x.Right <- Unchecked.defaultof<_>

        interface IEnumerator with
            member x.MoveNext() = x.MoveNext()
            member x.Current = x.Current :> obj
            member x.Reset() = x.Reset()

        interface IEnumerator<Range1l> with
            member x.Dispose() = x.Dispose()
            member x.Current = x.Current
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RangeSet1l =

    /// Empty range set.
    let empty = RangeSet1l.Empty

    /// Returns the minimum value in the range set or Int64.MaxValue if the range is empty.
    let inline min (set : RangeSet1l) = set.Min

    /// Returns the maximum value in the range set or Int64.MinValue if the range is empty.
    let inline max (set : RangeSet1l) = set.Max

    /// Returns the total range spanned by the range set, i.e. [min, max].
    let inline range (set : RangeSet1l) = set.Range

    let inline private getHalfRanges (r : Range1l) =
        [ struct (r.Min, HalfRangeKind.Left)
          if r.Max < Int64.MaxValue then struct (r.Max + 1L, HalfRangeKind.Right) ]

    let inline private ofRange (r : Range1l) =
        RangeSet1l(MapExt.ofListV <| getHalfRanges r)

    let private ofRanges (ranges : seq<Range1l>) =
        let halves =
            ranges
            |> Seq.toList
            |> List.collect getHalfRanges
            |> List.sortBy fstv

        let mutable level = 0
        let result = ResizeArray()

        for (struct (i, k) as h) in halves do
            if k = HalfRangeKind.Left then
                if level = 0 then result.Add h
                level <- level + 1
            else
                level <- level - 1
                if level = 0 then result.Add h

        RangeSet1l(MapExt.ofSeqV result)

    /// Builds a range set of the given list of ranges.
    let ofList (ranges : Range1l list) =
        match ranges with
        | [] -> empty
        | [r] -> ofRange r
        | _ -> ofRanges ranges

    /// Builds a range set of the given array of ranges.
    let ofArray (ranges : Range1l[]) =
        if ranges.Length = 0 then empty
        elif ranges.Length = 1 then ofRange ranges.[0]
        else ofRanges ranges

    /// Builds a range set of the given sequence of ranges.
    let inline ofSeq (ranges : seq<Range1l>) =
        ofList <| Seq.toList ranges

    /// Adds the given range to the set.
    let inline add (range : Range1l) (set : RangeSet1l) = set.Add range

    /// Removes the given range from the set.
    let inline remove (range : Range1l) (set : RangeSet1l) = set.Remove range

    /// Returns the union of two sets.
    let inline union (l : RangeSet1l) (r : RangeSet1l) = l.Union r

    /// Returns the intersection of the set with the given range.
    let inline intersect (range : Range1l) (set : RangeSet1l) = set.Intersect range

    /// Returns whether the given value is contained in the range set.
    let inline contains (value : int64) (set : RangeSet1l) = set.Contains value

    /// Returns whether the given range is contained in the set.
    let inline containsRange (range : Range1l) (set : RangeSet1l) = set.Contains range

    /// Returns the number of disjoint ranges in the set.
    let inline count (set : RangeSet1l) = set.Count

    /// Returns whether the set is empty.
    let inline isEmpty (set : RangeSet1l) = set.IsEmpty

    /// Views the range set as a sequence.
    let inline toSeq (set : RangeSet1l) = set :> seq<_>

    /// Builds a list from the range set.
    let inline toList (set : RangeSet1l) = set.ToList()

    /// Builds an array from the range set.
    let inline toArray (set : RangeSet1l) = set.ToArray()


/// Set of ranges where overlapping and neighboring ranges are coalesced.
/// Note that ranges describe closed intervals.
[<StructuredFormatDisplay("{AsString}")>]
type RangeSet1ul internal (store : MapExt<uint64, HalfRangeKind>) =
    static let empty = RangeSet1ul(MapExt.empty)

    /// Empty range set.
    static member Empty = empty

    member inline private x.Store = store

    // We cannot directly describe a range that ends at UInt64.MaxValue since the right half-range is inserted
    // at max + 1. In that case the right-half range will be missing and the total count is odd.
    member inline private x.HasMaxValue = store.Count % 2 = 1

    /// Returns the minimum value in the range set or UInt64.MaxValue if the range is empty.
    member x.Min =
        match store.TryMinKeyV with
        | ValueSome min -> min
        | _ -> UInt64.MaxValue

    /// Returns the maximum value in the range set or UInt64.MinValue if the range is empty.
    member x.Max =
        if x.HasMaxValue then UInt64.MaxValue
        else
            match store.TryMaxKeyV with
            | ValueSome max -> max - 1UL
            | _ -> UInt64.MinValue

    /// Returns the total range spanned by the range set, i.e. [min, max].
    member inline x.Range =
        Range1ul(x.Min, x.Max)

    /// Adds the given range to the set.
    member x.Add(r : Range1ul) =
        if r.Max < r.Min then
            x
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let struct (lm, inner) = MapExt.splitAt min store
            let rm =
                if overflow then MapExt.empty
                else sndv <| MapExt.splitAt max inner

            let before = MapExt.tryMaxValue lm
            let after = MapExt.tryMinValue rm

            // If the set contained UInt64.MaxValue or we have overflown, we must not add an explicit right half-range.
            // UInt64.MaxValue is stored implicitly.
            let fixRightBoundary =
                if x.HasMaxValue || overflow then
                    id
                else
                    MapExt.add max HalfRangeKind.Right

            let newStore =
                match before, after with
                | ValueNone, ValueNone ->
                    MapExt.ofListV [
                        struct (min, HalfRangeKind.Left)
                    ]
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Right, ValueNone ->
                    lm
                    |> MapExt.add min HalfRangeKind.Left
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Left, ValueNone ->
                    lm
                    |> fixRightBoundary

                | ValueNone, ValueSome HalfRangeKind.Left ->
                    rm
                    |> MapExt.add min HalfRangeKind.Left
                    |> MapExt.add max HalfRangeKind.Right

                | ValueNone, ValueSome HalfRangeKind.Right ->
                    rm
                    |> MapExt.add min HalfRangeKind.Left

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Left); struct (max, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (max, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Right ->
                    MapExt.union lm rm

                | _ ->
                    failwithf "impossible"

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1ul(newStore)

    /// Removes the given range from the set.
    member x.Remove(r : Range1ul) =
        if r.Max < r.Min then
            x
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let struct (lm, inner) = MapExt.splitAt min store
            let rm =
                if overflow then MapExt.empty
                else sndv <| MapExt.splitAt max inner

            let before = MapExt.tryMaxValue lm
            let after = MapExt.tryMinValue rm

            // If the set contained UInt64.MaxValue and we have not overflown, there is still a range [max, UInt64.MaxValue]
            let fixRightBoundary =
                if x.HasMaxValue && not overflow then
                    MapExt.add max HalfRangeKind.Left
                else
                    id

            let newStore =
                match before, after with
                | ValueNone, ValueNone ->
                    MapExt.empty
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Right, ValueNone ->
                    lm
                    |> fixRightBoundary

                | ValueSome HalfRangeKind.Left, ValueNone ->
                    lm
                    |> MapExt.add min HalfRangeKind.Right
                    |> fixRightBoundary

                | ValueNone, ValueSome HalfRangeKind.Left ->
                    rm

                | ValueNone, ValueSome HalfRangeKind.Right ->
                    rm
                    |> MapExt.add max HalfRangeKind.Left

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Left ->
                    MapExt.union lm rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Left ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Right) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Right, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (max, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | ValueSome HalfRangeKind.Left, ValueSome HalfRangeKind.Right ->
                    let self = MapExt.ofListV [ struct (min, HalfRangeKind.Right); struct (max, HalfRangeKind.Left) ]
                    MapExt.union (MapExt.union lm self) rm

                | _ ->
                    failwithf "impossible"

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1ul(newStore)

    /// Returns the union of the set with the given set.
    member inline x.Union(other : RangeSet1ul) =
        let mutable res = x
        for r in other do
            res <- res.Add r
        res

    /// Returns the intersection of the set with the given range.
    member x.Intersect(r : Range1ul) =
        if r.Max < r.Min then
            empty
        else
            let min = r.Min
            let struct (max, overflow) = inc r.Max

            let inner =
                store
                |> MapExt.splitAt min |> sndv
                |> if not overflow then MapExt.splitAt max >> fstv else id

            let newStore =
                inner
                |> if x.Contains r.Min then MapExt.add min HalfRangeKind.Left else id
                |> if x.Contains r.Max && not overflow then MapExt.add max HalfRangeKind.Right else id

            assert (newStore.Count % 2 = 0 || MapExt.maxValue newStore = HalfRangeKind.Left)
            RangeSet1ul(newStore)

    member private x.TryFindLeftBoundary(v : uint64) =
        let struct (l, s, _) = MapExt.neighboursV v store
        match s with
        | ValueSome (i, k) -> if k = HalfRangeKind.Left then ValueSome i else ValueNone
        | _ ->
            match l with
            | ValueSome (i, HalfRangeKind.Left) -> ValueSome i
            | _ -> ValueNone

    /// Returns whether the given value is contained in the range set.
    member x.Contains(v : uint64) =
        x.TryFindLeftBoundary v |> ValueOption.isSome

    /// Returns whether the given range is contained in the set.
    member x.Contains(r : Range1ul) =
        if r.Max < r.Min then false
        elif r.Min = r.Max then x.Contains r.Min
        else
            match x.TryFindLeftBoundary r.Min, x.TryFindLeftBoundary r.Max with
            | ValueSome l, ValueSome r -> l = r
            | _ -> false

    /// Returns the number of disjoint ranges in the set.
    member x.Count =
        (store.Count + 1) / 2

    /// Returns whether the set is empty.
    member inline x.IsEmpty =
        x.Count = 0

    /// Builds an array from the range set.
    member x.ToArray() =
        let arr = Array.zeroCreate x.Count

        let rec write (i : int) (l : struct (uint64 * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                arr.[i] <- Range1ul(l, r - 1UL)
                write (i + 1) rest

            | [struct (l, HalfRangeKind.Left)] when i = x.Count - 1 ->
                arr.[i] <- Range1ul(l, UInt64.MaxValue)

            | [_] -> failwith "bad RangeSet"

            | [] -> ()

        store |> MapExt.toListV |> write 0
        arr

    /// Builds a list from the range set.
    member x.ToList() =
        let rec build (accum : Range1ul list) (l : struct (uint64 * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                build (Range1ul(l, r - 1UL) :: accum) rest

            | [struct (l, HalfRangeKind.Left)] ->
                build (Range1ul(l, UInt64.MaxValue) :: accum) []

            | [_] -> failwith "bad RangeSet"

            | [] -> List.rev accum

        store |> MapExt.toListV |> build []

    /// Views the range set as a sequence.
    member x.ToSeq() =
        x :> seq<_>

    member inline private x.Equals(other : RangeSet1ul) =
        store = other.Store

    override x.Equals(other : obj) =
        match other with
        | :? RangeSet1ul as o -> x.Equals o
        | _ -> false

    override x.GetHashCode() =
        store.GetHashCode()

    member private x.AsString = x.ToString()

    override x.ToString() =
        let content =
            x |> Seq.map (fun r ->
                $"[{r.Min}, {r.Max}]"
            )
            |> String.concat "; "

        $"ranges [{content}]"

    member x.GetEnumerator() =
        new RangeSetEnumerator1ul((store :> seq<_>).GetEnumerator())

    interface IEquatable<RangeSet1ul> with
        member x.Equals(other) = x.Equals(other)

    interface IEnumerable with
        member x.GetEnumerator() = new RangeSetEnumerator1ul((store :> seq<_>).GetEnumerator()) :> _

    interface IEnumerable<Range1ul> with
        member x.GetEnumerator() = new RangeSetEnumerator1ul((store :> seq<_>).GetEnumerator()) :> _

// TODO: MapExt should use a struct enumerator and return it directly.
// That way we could get rid of allocations.
and RangeSetEnumerator1ul =
    struct
        val private Inner : IEnumerator<KeyValuePair<uint64, HalfRangeKind>>
        val mutable private Left : KeyValuePair<uint64, HalfRangeKind>
        val mutable private Right : KeyValuePair<uint64, HalfRangeKind>

        internal new (inner : IEnumerator<KeyValuePair<uint64, HalfRangeKind>>) =
            { Inner = inner
              Left = Unchecked.defaultof<_>
              Right = Unchecked.defaultof<_> }

        member x.MoveNext() =
            if x.Inner.MoveNext() then
                x.Left <- x.Inner.Current
                if x.Inner.MoveNext() then
                    x.Right <- x.Inner.Current
                    true
                else
                    if x.Left.Value = HalfRangeKind.Left then
                        x.Right <- KeyValuePair(UInt64.MinValue, HalfRangeKind.Right) // MaxValue + 1
                        true
                    else
                        failwithf "bad RangeSet"
            else
                false

        member x.Reset() =
            x.Inner.Reset()
            x.Left <- Unchecked.defaultof<_>
            x.Right <- Unchecked.defaultof<_>

        member x.Current =
            assert (x.Left.Value = HalfRangeKind.Left && x.Right.Value = HalfRangeKind.Right)
            Range1ul(x.Left.Key, x.Right.Key - 1UL)

        member x.Dispose() =
            x.Inner.Dispose()
            x.Left <- Unchecked.defaultof<_>
            x.Right <- Unchecked.defaultof<_>

        interface IEnumerator with
            member x.MoveNext() = x.MoveNext()
            member x.Current = x.Current :> obj
            member x.Reset() = x.Reset()

        interface IEnumerator<Range1ul> with
            member x.Dispose() = x.Dispose()
            member x.Current = x.Current
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RangeSet1ul =

    /// Empty range set.
    let empty = RangeSet1ul.Empty

    /// Returns the minimum value in the range set or UInt64.MaxValue if the range is empty.
    let inline min (set : RangeSet1ul) = set.Min

    /// Returns the maximum value in the range set or UInt64.MinValue if the range is empty.
    let inline max (set : RangeSet1ul) = set.Max

    /// Returns the total range spanned by the range set, i.e. [min, max].
    let inline range (set : RangeSet1ul) = set.Range

    let inline private getHalfRanges (r : Range1ul) =
        [ struct (r.Min, HalfRangeKind.Left)
          if r.Max < UInt64.MaxValue then struct (r.Max + 1UL, HalfRangeKind.Right) ]

    let inline private ofRange (r : Range1ul) =
        RangeSet1ul(MapExt.ofListV <| getHalfRanges r)

    let private ofRanges (ranges : seq<Range1ul>) =
        let halves =
            ranges
            |> Seq.toList
            |> List.collect getHalfRanges
            |> List.sortBy fstv

        let mutable level = 0
        let result = ResizeArray()

        for (struct (i, k) as h) in halves do
            if k = HalfRangeKind.Left then
                if level = 0 then result.Add h
                level <- level + 1
            else
                level <- level - 1
                if level = 0 then result.Add h

        RangeSet1ul(MapExt.ofSeqV result)

    /// Builds a range set of the given list of ranges.
    let ofList (ranges : Range1ul list) =
        match ranges with
        | [] -> empty
        | [r] -> ofRange r
        | _ -> ofRanges ranges

    /// Builds a range set of the given array of ranges.
    let ofArray (ranges : Range1ul[]) =
        if ranges.Length = 0 then empty
        elif ranges.Length = 1 then ofRange ranges.[0]
        else ofRanges ranges

    /// Builds a range set of the given sequence of ranges.
    let inline ofSeq (ranges : seq<Range1ul>) =
        ofList <| Seq.toList ranges

    /// Adds the given range to the set.
    let inline add (range : Range1ul) (set : RangeSet1ul) = set.Add range

    /// Removes the given range from the set.
    let inline remove (range : Range1ul) (set : RangeSet1ul) = set.Remove range

    /// Returns the union of two sets.
    let inline union (l : RangeSet1ul) (r : RangeSet1ul) = l.Union r

    /// Returns the intersection of the set with the given range.
    let inline intersect (range : Range1ul) (set : RangeSet1ul) = set.Intersect range

    /// Returns whether the given value is contained in the range set.
    let inline contains (value : uint64) (set : RangeSet1ul) = set.Contains value

    /// Returns whether the given range is contained in the set.
    let inline containsRange (range : Range1ul) (set : RangeSet1ul) = set.Contains range

    /// Returns the number of disjoint ranges in the set.
    let inline count (set : RangeSet1ul) = set.Count

    /// Returns whether the set is empty.
    let inline isEmpty (set : RangeSet1ul) = set.IsEmpty

    /// Views the range set as a sequence.
    let inline toSeq (set : RangeSet1ul) = set :> seq<_>

    /// Builds a list from the range set.
    let inline toList (set : RangeSet1ul) = set.ToList()

    /// Builds an array from the range set.
    let inline toArray (set : RangeSet1ul) = set.ToArray()


