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

//# var ltypes          = new[] { "int32", "uint32", "int64", "uint64" };
//# var sltypes         = new[] { "Int32", "UInt32", "Int64", "UInt64" };
//# var suffixes        = new[] { "i", "ui", "l", "ul" };
//# var literalSuffixes = new[] { "", "u", "L", "UL" };
//#
//# for (int i = 0; i < ltypes.Length; i++) {
//#     var ltype = ltypes[i];
//#     var suffix = suffixes[i];
//#     var range = "Range1" + suffix;
//#     var rangeset = "RangeSet1" + suffix;
//#     var enumerator = "RangeSetEnumerator1" + suffix;
//#     var one = "1" + literalSuffixes[i];
//#     var minvalue = sltypes[i] + ".MinValue";
//#     var maxvalue = sltypes[i] + ".MaxValue";
/// Set of ranges where overlapping and neighboring ranges are coalesced.
/// Note that ranges describe closed intervals.
[<StructuredFormatDisplay("{AsString}")>]
type __rangeset__ internal (store : MapExt<__ltype__, HalfRangeKind>) =
    static let empty = __rangeset__(MapExt.empty)

    /// Empty range set.
    static member Empty = empty

    /// Builds a range set of the given sequence of ranges.
    static member OfSeq(ranges : __range__ seq) =
        let arr = ranges |> Seq.toArray
        if arr.Length = 0 then
            empty
        elif arr.Length = 1 then
            let r = arr.[0]
            __rangeset__(MapExt.ofListV [
                struct (r.Min, HalfRangeKind.Left)
                if r.Max < __maxvalue__ then struct (r.Max + __one__, HalfRangeKind.Right)
            ])
        else
            // TODO: better impl possible (sort array and traverse)
            arr |> Array.fold (fun s r -> s.Add r) empty

    member inline private x.Store = store

    // We cannot directly describe a range that ends at __maxvalue__ since the right half-range is inserted
    // at max + 1. In that case the right-half range will be missing and the total count is odd.
    member inline private x.HasMaxValue = store.Count % 2 = 1

    /// Returns the minimum value in the range set or __maxvalue__ if the range is empty.
    member x.Min =
        match store.TryMinKeyV with
        | ValueSome min -> min
        | _ -> __maxvalue__

    /// Returns the maximum value in the range set or __minvalue__ if the range is empty.
    member x.Max =
        if x.HasMaxValue then __maxvalue__
        else
            match store.TryMaxKeyV with
            | ValueSome max -> max - __one__
            | _ -> __minvalue__

    /// Returns the total range spanned by the range set, i.e. [min, max].
    member inline x.Range =
        __range__(x.Min, x.Max)

    /// Adds the given range to the set.
    member x.Add(r : __range__) =
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

            // If the set contained __maxvalue__ or we have overflown, we must not add an explicit right half-range.
            // __maxvalue__ is stored implicitly.
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
            __rangeset__(newStore)

    /// Removes the given range from the set.
    member x.Remove(r : __range__) =
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

            // If the set contained __maxvalue__ and we have not overflown, there is still a range [max, __maxvalue__]
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
            __rangeset__(newStore)

    /// Returns the intersection of the set with the given range.
    member x.Intersect(r : __range__) =
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
            __rangeset__(newStore)

    /// Returns whether the given value is contained in the range set.
    member x.Contains(v : __ltype__) =
        let struct (l, s, _) = MapExt.neighboursV v store
        match s with
        | ValueSome (_, k) -> k = HalfRangeKind.Left
        | _ ->
            match l with
            | ValueSome (_, HalfRangeKind.Left) -> true
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

        let rec write (i : int) (l : struct (__ltype__ * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                arr.[i] <- __range__(l, r - __one__)
                write (i + 1) rest

            | [struct (l, HalfRangeKind.Left)] when i = x.Count - 1 ->
                arr.[i] <- __range__(l, __maxvalue__)

            | [_] -> failwith "bad RangeSet"

            | [] -> ()

        store |> MapExt.toListV |> write 0
        arr

    /// Builds a list from the range set.
    member x.ToList() =
        let rec build (accum : __range__ list) (l : struct (__ltype__ * HalfRangeKind) list) =
            match l with
            | struct (l, _) :: struct (r, _) :: rest ->
                build (__range__(l, r - __one__) :: accum) rest

            | [struct (l, HalfRangeKind.Left)] ->
                build (__range__(l, __maxvalue__) :: accum) []

            | [_] -> failwith "bad RangeSet"

            | [] -> List.rev accum

        store |> MapExt.toListV |> build []

    /// Views the range set as a sequence.
    member x.ToSeq() =
        x :> seq<_>

    member inline private x.Equals(other : __rangeset__) =
        store = other.Store

    override x.Equals(other : obj) =
        match other with
        | :? __rangeset__ as o -> x.Equals o
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
        new __enumerator__((store :> seq<_>).GetEnumerator())

    interface IEquatable<__rangeset__> with
        member x.Equals(other) = x.Equals(other)

    interface IEnumerable with
        member x.GetEnumerator() = new __enumerator__((store :> seq<_>).GetEnumerator()) :> _

    interface IEnumerable<__range__> with
        member x.GetEnumerator() = new __enumerator__((store :> seq<_>).GetEnumerator()) :> _

// TODO: MapExt should use a struct enumerator and return it directly.
// That way we could get rid of allocations.
and __enumerator__ =
    struct
        val private Inner : IEnumerator<KeyValuePair<__ltype__, HalfRangeKind>>
        val mutable private Left : KeyValuePair<__ltype__, HalfRangeKind>
        val mutable private Right : KeyValuePair<__ltype__, HalfRangeKind>

        internal new (inner : IEnumerator<KeyValuePair<__ltype__, HalfRangeKind>>) =
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
                        x.Right <- KeyValuePair(__minvalue__, HalfRangeKind.Right) // MaxValue + 1
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
            __range__(x.Left.Key, x.Right.Key - __one__)

        member x.Dispose() =
            x.Inner.Dispose()
            x.Left <- Unchecked.defaultof<_>
            x.Right <- Unchecked.defaultof<_>

        interface IEnumerator with
            member x.MoveNext() = x.MoveNext()
            member x.Current = x.Current :> obj
            member x.Reset() = x.Reset()

        interface IEnumerator<__range__> with
            member x.Dispose() = x.Dispose()
            member x.Current = x.Current
    end

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module __rangeset__ =

    /// Empty range set.
    let empty = __rangeset__.Empty

    /// Returns the minimum value in the range set or __maxvalue__ if the range is empty.
    let inline min (set : __rangeset__) = set.Min

    /// Returns the maximum value in the range set or __minvalue__ if the range is empty.
    let inline max (set : __rangeset__) = set.Max

    /// Returns the total range spanned by the range set, i.e. [min, max].
    let inline range (set : __rangeset__) = set.Range

    /// Builds a range set of the given sequence of ranges.
    let inline ofSeq (ranges : seq<__range__>) = __rangeset__.OfSeq ranges

    /// Builds a range set of the given list of ranges.
    let inline ofList (ranges : __range__ list) = __rangeset__.OfSeq ranges

    /// Builds a range set of the given array of ranges.
    let inline ofArray (ranges : __range__[]) = __rangeset__.OfSeq ranges

    /// Adds the given range to the set.
    let inline add (range : __range__) (set : __rangeset__) = set.Add range

    [<Obsolete("Use __rangeset__.add instead.")>]
    let inline insert (range : __range__) (set : __rangeset__) = set.Add range

    /// Removes the given range from the set.
    let inline remove (range : __range__) (set : __rangeset__) = set.Remove range

    /// Returns the intersection of the set with the given range.
    let inline intersect (range : __range__) (set : __rangeset__) = set.Intersect range

    [<Obsolete("Use __rangeset__.intersect instead.")>]
    let inline window (range : __range__) (set : __rangeset__) = intersect range set

    /// Returns whether the given value is contained in the range set.
    let inline contains (value : __ltype__) (set : __rangeset__) = set.Contains value

    /// Returns the number of disjoint ranges in the set.
    let inline count (set : __rangeset__) = set.Count

    /// Returns whether the set is empty.
    let inline isEmpty (set : __rangeset__) = set.IsEmpty

    /// Views the range set as a sequence.
    let inline toSeq (set : __rangeset__) = set :> seq<_>

    /// Builds a list from the range set.
    let inline toList (set : __rangeset__) = set.ToList()

    /// Builds an array from the range set.
    let inline toArray (set : __rangeset__) = set.ToArray()


//# }