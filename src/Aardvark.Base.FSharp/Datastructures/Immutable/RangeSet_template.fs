namespace Aardvark.Base

open System
open System.Collections
open System.Collections.Generic
open FingerTreeImplementation

//# foreach (var isLong in new[] { false, true }) {
//#   var halfrange = isLong ? "HalfRange64" : "HalfRange";
//#   var rangeset = isLong ? "RangeSet64" : "RangeSet";
//#   var rangesetenumerator = isLong ? "RangeSet64Enumerator" : "RangeSetEnumerator";
//#   var range = isLong ? "Range1l" : "Range1i";
//#   var systype = isLong ? "Int64" : "Int32";
//#   var ft = isLong ? "int64" : "int32";
//#   var one = isLong ? "1L" : "1";
[<CustomEquality; CustomComparison>]
type private __halfrange__ =
    struct
        val mutable public IsMax : bool
        val mutable public Value : __ft__

        new(m,v) = { IsMax = m; Value = v }

        override x.GetHashCode() =
            if x.IsMax then 0
            else x.Value.GetHashCode()

        override x.Equals o =
            match o with
            | :? __halfrange__ as o -> 
                x.IsMax = o.IsMax && x.Value = o.Value
            | _ ->
                false

        member x.CompareTo (o : __halfrange__) =
            let c = x.Value.CompareTo o.Value
            if c = 0 then 
                if x.IsMax = o.IsMax then 0
                else (if x.IsMax then 1 else -1)
            else
                c

        interface IComparable with
            member x.CompareTo o =
                match o with
                | :? __halfrange__ as o -> x.CompareTo(o)
                | _ -> failwith "uncomparable"
    end


[<StructuredFormatDisplay("{AsString}")>]
type __rangeset__ = private { root : FingerTreeNode<__halfrange__, __halfrange__> } with
    
    member private x.AsString =
        x |> Seq.map (sprintf "%A")
          |> String.concat "; "
          |> sprintf "set [%s]"

    member x.Min =
        match x.root |> FingerTreeNode.firstOpt with
        | Some f -> f.Value
        | _ -> __systype__.MaxValue

    member x.Max =
        match x.root |> FingerTreeNode.lastOpt with
        | Some f -> f.Value
        | _ -> __systype__.MinValue

    member x.Range =
        match FingerTreeNode.firstOpt x.root, FingerTreeNode.lastOpt x.root with
        | Some min, Some max -> __range__(min.Value, max.Value)
        | _ -> __range__.Invalid

    interface IEnumerable with
        member x.GetEnumerator() = new __rangesetenumerator__(FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root) :> IEnumerator

    interface IEnumerable<__range__> with
        member x.GetEnumerator() = new __rangesetenumerator__(FingerTreeImplementation.FingerTreeNode.getEnumeratorFw x.root) :> _


and private __rangesetenumerator__(i : IEnumerator<__halfrange__>) =
    
    let mutable last = __halfrange__()
    let mutable current = __halfrange__()

    member x.Current = __range__(last.Value, current.Value - __one__)

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

    interface IEnumerator<__range__> with
        member x.Current = x.Current
        member x.Dispose() = i.Dispose()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module __rangeset__ =
    let private mm =
        {
            quantify = fun (r : __halfrange__) -> r
            mempty = __halfrange__(false, __systype__.MinValue)
            mappend = fun l r -> if l.CompareTo r > 0 then l else r
        }

    let private minRange = __halfrange__(false, __systype__.MinValue)

    let inline private leq v = __halfrange__(true, v)
    let inline private geq v = __halfrange__(false, v)

    let inline private (|Leq|Geq|) (r : __halfrange__) =
        if r.IsMax then Leq r.Value
        else Geq r.Value

    let empty = { root = Empty }

    let insert (range : __range__) (t : __rangeset__) =
        let rangeMax = range.Max + __one__

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

    let remove (range : __range__) (t : __rangeset__) =
        let rangeMax = range.Max + __one__

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

    let ofSeq (s : seq<__range__>) =
        let mutable res = empty
        for e in s do res <- insert e res
        res

    let inline ofList (l : list<__range__>) = ofSeq l
    let inline ofArray (l : __range__[]) = ofSeq l

    let toSeq (r : __rangeset__) = r :> seq<_>
    let toList (r : __rangeset__) = r |> Seq.toList
    let toArray (r : __rangeset__) = r |> Seq.toArray

    let inline min (t : __rangeset__) = t.Min
    let inline max (t : __rangeset__) = t.Max
    let inline range (t : __rangeset__) = t.Range

    let window (window : __range__) (set : __rangeset__) =
        let rangeMax = window.Max + __one__

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

//# }