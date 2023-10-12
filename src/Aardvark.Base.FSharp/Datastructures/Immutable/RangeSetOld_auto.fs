namespace Aardvark.Base

open System
open System.Collections
open System.Collections.Generic
open FingerTreeImplementation

#nowarn "44"

[<CustomEquality; CustomComparison>]
type private HalfRange =
    struct
        val mutable public IsMax : bool
        val mutable public Value : int32

        new(m,v) = { IsMax = m; Value = v }

        override x.GetHashCode() =
            if x.IsMax then 0
            else x.Value.GetHashCode()

        override x.Equals o =
            match o with
            | :? HalfRange as o -> 
                x.IsMax = o.IsMax && x.Value = o.Value
            | _ ->
                false

        member x.CompareTo (o : HalfRange) =
            let c = x.Value.CompareTo o.Value
            if c = 0 then 
                if x.IsMax = o.IsMax then 0
                else (if x.IsMax then 1 else -1)
            else
                c

        interface IComparable with
            member x.CompareTo o =
                match o with
                | :? HalfRange as o -> x.CompareTo(o)
                | _ -> failwith "uncomparable"
    end


[<Obsolete("Use RangeSet1i instead. Note that upper bounds returned by Max and Range are now inclusive.")>]
[<StructuredFormatDisplay("{AsString}")>]
type RangeSet = private { root : FingerTreeNode<HalfRange, HalfRange> } with
    
    member private x.AsString =
        x |> Seq.map (sprintf "%A")
          |> String.concat "; "
          |> sprintf "set [%s]"

    member x.Min =
        match x.root |> FingerTreeNode.firstOpt with
        | Some f -> f.Value
        | _ -> Int32.MaxValue

    member x.Max =
        match x.root |> FingerTreeNode.lastOpt with
        | Some f -> f.Value
        | _ -> Int32.MinValue

    member x.Range =
        match FingerTreeNode.firstOpt x.root, FingerTreeNode.lastOpt x.root with
        | Some min, Some max -> Range1i(min.Value, max.Value)
        | _ -> Range1i.Invalid

    interface IEnumerable with
        member x.GetEnumerator() = new RangeSetEnumerator(FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root) :> IEnumerator

    interface IEnumerable<Range1i> with
        member x.GetEnumerator() = new RangeSetEnumerator(FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root) :> _


and private RangeSetEnumerator(i : IEnumerator<HalfRange>) =
    
    let mutable last = HalfRange()
    let mutable current = HalfRange()

    member x.Current = Range1i(last.Value, current.Value - 1)

    interface IEnumerator with
        member x.MoveNext() =
            if i.MoveNext() then
                last <- i.Current
                if i.MoveNext() then
                    current <- i.Current
                    true
                else
                    false
            else
                false

        member x.Current = x.Current :> obj

        member x.Reset() =
            i.Reset()

    interface IEnumerator<Range1i> with
        member x.Current = x.Current
        member x.Dispose() = i.Dispose()

[<Obsolete("Use RangeSet1i instead. Note that upper bounds returned by max and range are now inclusive.")>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RangeSet =
    let private mm =
        {
            quantify = fun (r : HalfRange) -> r
            mempty = HalfRange(false, Int32.MinValue)
            mappend = fun l r -> if l.CompareTo r > 0 then l else r
        }

    let private minRange = HalfRange(false, Int32.MinValue)

    let inline private leq v = HalfRange(true, v)
    let inline private geq v = HalfRange(false, v)

    let inline private (|Leq|Geq|) (r : HalfRange) =
        if r.IsMax then Leq r.Value
        else Geq r.Value

    let empty = { root = Empty }

    let insert (range : Range1i) (t : RangeSet) =
        let rangeMax = range.Max + 1

        let (l,rest) = t.root |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo range.Min >= 0) minRange
        let (_,r) = rest |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo rangeMax > 0) minRange

        let max = leq rangeMax
        let min = geq range.Min

        match FingerTreeNode.lastOpt l, FingerTreeNode.firstOpt r with
        | None, None -> 
            { root = Deep(max, One(min), Empty, One(max)) }

        | Some lmax, None ->
            match lmax with
            | Leq _ -> { root = l |> FingerTreeNode.append mm min |> FingerTreeNode.append mm max }
            | Geq _ -> { root = l |> FingerTreeNode.append mm max }

        | None, Some rmin ->
            match rmin with
            | Leq _ -> { root = r |> FingerTreeNode.prepend mm min }
            | Geq _ -> { root = r |> FingerTreeNode.prepend mm max |> FingerTreeNode.prepend mm min }

        | Some lmax, Some rmin ->
            match lmax, rmin with
            | Leq _, Geq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [min;max] r }

            | Geq _, Leq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [] r }

            | Leq _, Leq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [min] r }

            | Geq _, Geq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [max] r }

    let remove (range : Range1i) (t : RangeSet) =
        let rangeMax = range.Max + 1

        let (l,rest) = t.root |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo range.Min >= 0) minRange
        let (_,r) = rest |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo rangeMax > 0) minRange

        let max = geq rangeMax
        let min = leq range.Min

        match FingerTreeNode.lastOpt l, FingerTreeNode.firstOpt r with
        | None, None -> 
            { root = Empty }

        | Some lmax, None ->
            match lmax with
            | Leq _ -> { root = l }
            | Geq _ -> { root = l |> FingerTreeNode.append mm min }

        | None, Some rmin ->
            match rmin with
            | Leq _ -> { root = r |> FingerTreeNode.prepend mm max }
            | Geq _ -> { root = r }

        | Some lmax, Some rmin ->
            match lmax, rmin with
            | Leq _, Geq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [] r }

            | Geq _, Leq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [min; max] r }

            | Leq _, Leq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [max] r }

            | Geq _, Geq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [min] r }

    let ofSeq (s : seq<Range1i>) =
        let mutable res = empty
        for e in s do res <- insert e res
        res

    let inline ofList (l : list<Range1i>) = ofSeq l
    let inline ofArray (l : Range1i[]) = ofSeq l

    let toSeq (r : RangeSet) = r :> seq<_>
    let toList (r : RangeSet) = r |> Seq.toList
    let toArray (r : RangeSet) = r |> Seq.toArray

    let inline min (t : RangeSet) = t.Min
    let inline max (t : RangeSet) = t.Max
    let inline range (t : RangeSet) = t.Range

    let window (window : Range1i) (set : RangeSet) =
        let rangeMax = window.Max + 1

        let (l,rest) = set.root |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo window.Min > 0) minRange
        let (inner,r) = rest |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo rangeMax >= 0) minRange

        let inner =
            match FingerTreeNode.lastOpt l with
            | Some (Geq _) -> FingerTreeNode.prepend mm (geq window.Min) inner
            | _ -> inner

        let inner =
            match FingerTreeNode.firstOpt r with
            | Some (Leq _) -> FingerTreeNode.append mm (leq rangeMax) inner
            | _ -> inner

        { root = inner }

[<CustomEquality; CustomComparison>]
type private HalfRange64 =
    struct
        val mutable public IsMax : bool
        val mutable public Value : int64

        new(m,v) = { IsMax = m; Value = v }

        override x.GetHashCode() =
            if x.IsMax then 0
            else x.Value.GetHashCode()

        override x.Equals o =
            match o with
            | :? HalfRange64 as o -> 
                x.IsMax = o.IsMax && x.Value = o.Value
            | _ ->
                false

        member x.CompareTo (o : HalfRange64) =
            let c = x.Value.CompareTo o.Value
            if c = 0 then 
                if x.IsMax = o.IsMax then 0
                else (if x.IsMax then 1 else -1)
            else
                c

        interface IComparable with
            member x.CompareTo o =
                match o with
                | :? HalfRange64 as o -> x.CompareTo(o)
                | _ -> failwith "uncomparable"
    end


[<Obsolete("Use RangeSet1l instead. Note that upper bounds returned by Max and Range are now inclusive.")>]
[<StructuredFormatDisplay("{AsString}")>]
type RangeSet64 = private { root : FingerTreeNode<HalfRange64, HalfRange64> } with
    
    member private x.AsString =
        x |> Seq.map (sprintf "%A")
          |> String.concat "; "
          |> sprintf "set [%s]"

    member x.Min =
        match x.root |> FingerTreeNode.firstOpt with
        | Some f -> f.Value
        | _ -> Int64.MaxValue

    member x.Max =
        match x.root |> FingerTreeNode.lastOpt with
        | Some f -> f.Value
        | _ -> Int64.MinValue

    member x.Range =
        match FingerTreeNode.firstOpt x.root, FingerTreeNode.lastOpt x.root with
        | Some min, Some max -> Range1l(min.Value, max.Value)
        | _ -> Range1l.Invalid

    interface IEnumerable with
        member x.GetEnumerator() = new RangeSet64Enumerator(FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root) :> IEnumerator

    interface IEnumerable<Range1l> with
        member x.GetEnumerator() = new RangeSet64Enumerator(FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root) :> _


and private RangeSet64Enumerator(i : IEnumerator<HalfRange64>) =
    
    let mutable last = HalfRange64()
    let mutable current = HalfRange64()

    member x.Current = Range1l(last.Value, current.Value - 1L)

    interface IEnumerator with
        member x.MoveNext() =
            if i.MoveNext() then
                last <- i.Current
                if i.MoveNext() then
                    current <- i.Current
                    true
                else
                    false
            else
                false

        member x.Current = x.Current :> obj

        member x.Reset() =
            i.Reset()

    interface IEnumerator<Range1l> with
        member x.Current = x.Current
        member x.Dispose() = i.Dispose()

[<Obsolete("Use RangeSet1l instead. Note that upper bounds returned by max and range are now inclusive.")>]
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module RangeSet64 =
    let private mm =
        {
            quantify = fun (r : HalfRange64) -> r
            mempty = HalfRange64(false, Int64.MinValue)
            mappend = fun l r -> if l.CompareTo r > 0 then l else r
        }

    let private minRange = HalfRange64(false, Int64.MinValue)

    let inline private leq v = HalfRange64(true, v)
    let inline private geq v = HalfRange64(false, v)

    let inline private (|Leq|Geq|) (r : HalfRange64) =
        if r.IsMax then Leq r.Value
        else Geq r.Value

    let empty = { root = Empty }

    let insert (range : Range1l) (t : RangeSet64) =
        let rangeMax = range.Max + 1L

        let (l,rest) = t.root |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo range.Min >= 0) minRange
        let (_,r) = rest |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo rangeMax > 0) minRange

        let max = leq rangeMax
        let min = geq range.Min

        match FingerTreeNode.lastOpt l, FingerTreeNode.firstOpt r with
        | None, None -> 
            { root = Deep(max, One(min), Empty, One(max)) }

        | Some lmax, None ->
            match lmax with
            | Leq _ -> { root = l |> FingerTreeNode.append mm min |> FingerTreeNode.append mm max }
            | Geq _ -> { root = l |> FingerTreeNode.append mm max }

        | None, Some rmin ->
            match rmin with
            | Leq _ -> { root = r |> FingerTreeNode.prepend mm min }
            | Geq _ -> { root = r |> FingerTreeNode.prepend mm max |> FingerTreeNode.prepend mm min }

        | Some lmax, Some rmin ->
            match lmax, rmin with
            | Leq _, Geq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [min;max] r }

            | Geq _, Leq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [] r }

            | Leq _, Leq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [min] r }

            | Geq _, Geq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [max] r }

    let remove (range : Range1l) (t : RangeSet64) =
        let rangeMax = range.Max + 1L

        let (l,rest) = t.root |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo range.Min >= 0) minRange
        let (_,r) = rest |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo rangeMax > 0) minRange

        let max = geq rangeMax
        let min = leq range.Min

        match FingerTreeNode.lastOpt l, FingerTreeNode.firstOpt r with
        | None, None -> 
            { root = Empty }

        | Some lmax, None ->
            match lmax with
            | Leq _ -> { root = l }
            | Geq _ -> { root = l |> FingerTreeNode.append mm min }

        | None, Some rmin ->
            match rmin with
            | Leq _ -> { root = r |> FingerTreeNode.prepend mm max }
            | Geq _ -> { root = r }

        | Some lmax, Some rmin ->
            match lmax, rmin with
            | Leq _, Geq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [] r }

            | Geq _, Leq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [min; max] r }

            | Leq _, Leq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [max] r }

            | Geq _, Geq _ ->
                { root = FingerTreeNode.concatWithMiddle mm l [min] r }

    let ofSeq (s : seq<Range1l>) =
        let mutable res = empty
        for e in s do res <- insert e res
        res

    let inline ofList (l : list<Range1l>) = ofSeq l
    let inline ofArray (l : Range1l[]) = ofSeq l

    let toSeq (r : RangeSet64) = r :> seq<_>
    let toList (r : RangeSet64) = r |> Seq.toList
    let toArray (r : RangeSet64) = r |> Seq.toArray

    let inline min (t : RangeSet64) = t.Min
    let inline max (t : RangeSet64) = t.Max
    let inline range (t : RangeSet64) = t.Range

    let window (window : Range1l) (set : RangeSet64) =
        let rangeMax = window.Max + 1L

        let (l,rest) = set.root |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo window.Min > 0) minRange
        let (inner,r) = rest |> FingerTreeNode.splitFirstRight mm (fun v -> v.Value.CompareTo rangeMax >= 0) minRange

        let inner =
            match FingerTreeNode.lastOpt l with
            | Some (Geq _) -> FingerTreeNode.prepend mm (geq window.Min) inner
            | _ -> inner

        let inner =
            match FingerTreeNode.firstOpt r with
            | Some (Leq _) -> FingerTreeNode.append mm (leq rangeMax) inner
            | _ -> inner

        { root = inner }

