namespace Aardvark.Base

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base

[<StructuredFormatDisplay("{AsString}")>]
type prefset<'a>(content : pmap<'a, int>) =
    static let empty = prefset<'a>(PMap.empty)
    static member Empty = empty
    
    member private x.content = content

    member inline private x.alter(v : 'a, f : int -> int) =
        content
            |> PMap.alter v (fun o ->
                let o = defaultArg o 0
                let n = f o
                if n = 0 then None
                else Some n
               )
            |> prefset

    member inline private x.alter2 (other : prefset<'a>) (f : int -> int -> int) =
        let merge (key : 'a) (l : Option<int>) (r : Option<int>) =
            let l = defaultArg l 0
            let r = defaultArg r 0
            let n = f l r
            if n = 0 then None
            else Some n

        PMap.alter2 merge content other.content |> prefset

    static member OfSeq (s : seq<'a>) =
        let mutable res = empty
        for e in s do res <- res.Add e
        res

    static member OfList (s : list<'a>) =
        let mutable res = empty
        for e in s do res <- res.Add e
        res

    static member OfArray (s : array<'a>) =
        let mutable res = empty
        for e in s do res <- res.Add e
        res

    member private x.AsString = x.ToString()
    
    override x.ToString() =
        content |> PMap.toSeq |> Seq.map (fun (v,c) -> if c = 1 then sprintf "%A" v else sprintf "%d*%A" c v) |> String.concat "; " |> sprintf "prefset [%s]"

    member x.Union(other : prefset<'a>) = x.alter2 other (+)
    member x.Difference(other : prefset<'a>) = x.alter2 other (-)
    member x.Intersect(other : prefset<'a>) = x.alter2 other min
    member x.Add(v : 'a) = x.alter(v, fun o -> o + 1)
    member x.Remove(v : 'a) = x.alter(v, fun o -> o - 1)
    member x.Add(cnt : int, v : 'a) = x.alter(v, fun o -> o + cnt)
    member x.Remove(cnt : int, v : 'a) = x.alter(v, fun o -> o - cnt)
    member x.AddIfNotPresent(v : 'a) = x.alter(v, fun o -> max o 1)
    member x.Count = content.Count

    member x.AsSeq = content |> PMap.toSeq |> Seq.map (fun (k,_) -> k)
    member x.AsList = content |> PMap.toList |> List.map (fun (k,_) -> k)
    member x.AsArray = content |> PMap.toArray |> Array.map (fun (k,_) -> k)
    
    member x.Contains a =
        match PMap.tryFind a content with
            | Some _ -> true
            | _ -> false

    member x.ComputeDeltas (other : prefset<'a>) =
        let mine = content
        let mutable other = other.content
        let mutable res = deltaset<'a>.Empty

        for (k,mc) in mine.AsSeq do
            match PMap.tryRemove k other with
                | Some (o, oc) ->
                    other <- o
                    let delta = oc - mc
                    if delta <> 0 then
                        res <- res.Add(SetDelta(k, sign delta))
                | None ->
                    res <- res.Add(SetDelta(k, -1))

        for (k,oc) in other.AsSeq do
            res <- res.Add(SetDelta(k, 1))

        res

    member x.ApplyDeltas (deltas : deltaset<'a>) =
        let mutable res = x

        let effective = 
            deltas.AsList |> List.choose (fun delta ->
                let v = delta.Value
                let cnt = res.Count
                res <- res.alter(v, fun o -> max 0 (o + delta.Count))
                if res.Count <> cnt then
                    let s = sign delta.Count
                    Some (v, s)
                else
                    None
            ) 

        res, deltaset(PMap.ofList effective)

    member x.ApplyDeltasNoRefCount (deltas : deltaset<'a>) =
        let mutable res = x

        let effective = 
            deltas.AsList |> List.choose (fun delta ->
                let cnt = res.Count
                let n = if delta.Count > 0 then 1 else 0
                res <- res.alter(delta.Value, fun o -> n)
                if res.Count <> cnt then
                    let s = if delta.Count > 0 then 1 else -1
                    Some (delta.Value, s)
                else
                    None
            ) 

        res, deltaset(PMap.ofList effective)

    member x.Map(f : 'a -> 'b) =
        let mutable res = prefset<'b>.Empty
        for (v,c) in content do
            res <- res.Add(c, f v)
        res

    member x.Choose(f : 'a -> Option<'b>) =
        let mutable res = prefset<'b>.Empty
        for (v,c) in content do
            match f v with
                | Some v -> 
                    res <- res.Add(c, v)
                | None ->
                    ()
        res
        
    member x.Filter(f : 'a -> bool) =
        let mutable res = prefset<'a>.Empty
        for (v,c) in content do
            if f v then
                res <- res.Add(c,v)
        res

    member x.Fold(f : 's -> 'a -> 's, initial : 's) =
        let mutable res = initial
        for (v,c) in content do
            res <- f res v
        res

    member x.Collect(f : 'a -> prefset<'b>) =
        let mutable res = prefset<'b>.Empty
        for (ov,oc) in content do
            let inner = f ov
            for (v, ic) in inner.content do
                res <- res.Add(oc * ic, v)
        res

    static member Compare(l : prefset<'a>, r : prefset<'a>) =
        let mutable lo = 0
        let mutable ro = 0
        let mutable both = 0
        
        let mutable r = r
        for lv in l do
            if r.Contains lv then
                both <- both + 1
                r <- r.Remove lv
            else
                lo <- lo + 1

        ro <- r.Count
        lo, both, ro



    interface IEnumerable with
        member x.GetEnumerator() = (x.AsSeq :> IEnumerable).GetEnumerator()

    interface IEnumerable<'a> with
        member x.GetEnumerator() = x.AsSeq.GetEnumerator()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PRefSet =

    let inline empty<'a> = prefset<'a>.Empty
    let inline single (v : 'a) = prefset<'a>.OfList [v]

    let inline ofSeq (s : seq<'a>) = prefset<'a>.OfSeq s
    let inline ofList (l : list<'a>) = prefset<'a>.OfList l
    let inline ofArray (a : array<'a>) = prefset<'a>.OfArray a
    let inline toSeq (s : prefset<'a>) = s.AsSeq
    let inline toList (s : prefset<'a>) = s.AsList
    let inline toArray (s : prefset<'a>) = s.AsArray

    let inline computeDeltas (l : prefset<'a>) (r : prefset<'a>) = l.ComputeDeltas r
    let inline applyDeltas (l : prefset<'a>) (deltas : deltaset<'a>) = l.ApplyDeltas deltas
    let inline applyDeltasNoRefCount (l : prefset<'a>) (deltas : deltaset<'a>) = l.ApplyDeltasNoRefCount deltas
    let inline union (l : prefset<'a>) (r : prefset<'a>) = l.Union r
    let inline difference (l : prefset<'a>) (r : prefset<'a>) = l.Difference r
    let inline intersect (l : prefset<'a>) (r : prefset<'a>) = l.Intersect r

    let inline count (l : prefset<'a>) = l.Count 
    let inline isEmpty (l : prefset<'a>) = l.Count = 0
    let inline contains (v : 'a) (s : prefset<'a>) = s.Contains v
    
    let inline fold (f : 's -> 'a -> 's) (initial : 's) (s : prefset<'a>) = s.Fold(f, initial)
    let inline map (f : 'a -> 'b) (s : prefset<'a>) = s.Map f
    let inline choose (f : 'a -> Option<'b>) (s : prefset<'a>) = s.Choose f
    let inline filter (f : 'a -> bool) (s : prefset<'a>) = s.Filter f
    let inline collect (f : 'a -> prefset<'b>) (s : prefset<'a>) = s.Collect f

    type private TraceImpl<'a>() =

        static let trace : Traceable<prefset<'a>, deltaset<'a>> =
            {
                empty = empty
                apply = applyDeltas
                compute = computeDeltas
                collapse = fun _ _ -> false
                ops = DeltaSet.monoid
            }

        static let traceNoRef : Traceable<prefset<'a>, deltaset<'a>> =
            {
                empty = empty
                apply = applyDeltasNoRefCount
                compute = computeDeltas
                collapse = fun _ _ -> false
                ops = DeltaSet.monoid
            }


        static member Instance = trace
        static member NoRefInstance = traceNoRef

    let trace<'a> = TraceImpl<'a>.Instance
    let traceNoRefCount<'a> = TraceImpl<'a>.NoRefInstance