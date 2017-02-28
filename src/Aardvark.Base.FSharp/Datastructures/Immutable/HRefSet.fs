namespace Aardvark.Base

open System.Collections
open System.Collections.Generic

[<Struct>]
type hrefset<'a>(store : hmap<'a, int>) =
    
    static member Empty = hrefset<'a>(HMap.empty)

    member x.IsEmpty = store.IsEmpty

    member x.Count = store.Count

    member private x.Store = store


    member x.Add(value : 'a) =
        store
        |> HMap.update value (fun o -> 
            match o with
                | Some o -> o + 1
                | None -> 1
        )
        |> hrefset

    member x.Remove(value : 'a) =
        store
        |> HMap.alter value (fun o ->
            match o with
                | Some 1 -> None
                | Some c -> Some (c - 1)
                | None -> None
        )
        |> hrefset

    member x.Union(other : hrefset<'a>) =
        HMap.map2 (fun k l r ->
            match l, r with 
                | Some l, Some r -> l + r
                | Some l, None -> l
                | None, Some r -> r
                | None, None -> 0
        ) store other.Store
        |> hrefset

    member x.UnionWith(other : hrefset<'a>, f : int -> int -> int) =
        HMap.map2 (fun k l r ->
            match l, r with 
                | Some l, Some r -> f l r
                | Some l, None -> f l 0
                | None, Some r -> f 0 r
                | None, None -> f 0 0
        ) store other.Store
        |> hrefset

    member x.ToHMap() =
        store

    member x.ToSeq() =
        store.ToSeq() |> Seq.map fst

    member x.ToList() =
        store.ToList() |> List.map fst

    member x.ToArray() =
        store.ToSeq() |> Seq.map fst |> Seq.toArray


    member x.Map(mapping : 'a -> 'b) =
        let mutable res = HMap.empty
        for (k,v) in HMap.toSeq store do
            let k = mapping k
            res <- res |> HMap.update k (fun o -> defaultArg o 0 + v) 

        hrefset res

    member x.Choose(mapping : 'a -> Option<'b>) =
        let mutable res = HMap.empty
        for (k,v) in HMap.toSeq store do
            match mapping k with
                | Some k ->
                    res <- res |> HMap.update k (fun o -> defaultArg o 0 + v) 
                | None ->
                    ()

        hrefset res

    member x.Filter(predicate : 'a -> bool) =
        store |> HMap.filter (fun k _ -> predicate k) |> hrefset

    member x.Collect(mapping : 'a -> hrefset<'b>) =
        let mutable res = hrefset<'b>.Empty
        for (k,ro) in store.ToSeq() do
            let r = mapping k
            if ro = 1 then
                res <- res.Union r
            else
                res <- res.UnionWith(r, fun li ri -> li + ro * ri)
        res

    member x.Iter (iterator : 'a -> unit) =
        store |> HMap.iter (fun k _ -> iterator k)

    member x.Exists (predicate : 'a -> bool) =
        store |> HMap.exists (fun k _ -> predicate k)

    member x.Forall (predicate : 'a -> bool) =
        store |> HMap.forall (fun k _ -> predicate k)

    member x.Fold (seed : 's, folder : 's -> 'a -> 's) =
        store |> HMap.fold (fun s k _ -> folder s k) seed 


    static member OfSeq (seq : seq<'a>) =
        let mutable res = hrefset<'a>.Empty
        for e in seq do
            res <- res.Add e
        res

    static member OfList (list : list<'a>) =
        hrefset<'a>.OfSeq list

    static member OfArray (arr : 'a[]) =
        hrefset<'a>.OfSeq arr

    static member OfHMap (map : hmap<'a, int>) =
        hrefset map

    member x.ComputeDelta(other : hrefset<'a>) =
        HMap.choose2 (fun k l r -> 
            match l, r with
                | None, Some _ -> Some 1
                | Some _, None -> Some -1
                | None, None -> None
                | Some l, Some r -> None
           ) store other.Store
        |> HDeltaSet.ofHMap

    member x.ApplyDelta (deltas : hdeltaset<'a>) =
        let mutable res = store

        let effective =
            deltas |> HDeltaSet.choose (fun d ->
                let mutable delta = Unchecked.defaultof<SetDelta<'a>>
                let value = d.Value
                res <- res |> HMap.alter value (fun cnt ->
                    let o = defaultArg cnt 0
                    let n = o + d.Count
                    if n > 0 && o = 0 then 
                        delta <- Add(value)
                    elif n = 0 && o > 0 then
                        delta <- Rem(value)

                    Some n
                )

                if delta.Count <> 0 then Some delta
                else None
            )

        hrefset res, effective

    member x.ApplyDeltaNoRefCount (deltas : hdeltaset<'a>) =
        let mutable res = store

        let effective =
            deltas |> HDeltaSet.choose (fun d ->
                let mutable delta = Unchecked.defaultof<SetDelta<'a>>
                let value = d.Value
                res <- res |> HMap.alter value (fun cnt ->
                    let o = defaultArg cnt 0
                    let n = 
                        if d.Count > 0 then 1 
                        elif d.Count < 0 then 0
                        else o

                    if n > 0 && o = 0 then 
                        delta <- Add(value)
                    elif n = 0 && o > 0 then
                        delta <- Rem(value)

                    Some n
                )

                if delta.Count <> 0 then Some delta
                else None
            )

        hrefset res, effective




    interface IEnumerable with
        member x.GetEnumerator() = x.ToSeq().GetEnumerator() :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = x.ToSeq().GetEnumerator()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HRefSet =
    let inline empty<'a> = hrefset<'a>.Empty

    // O(1)
    let inline toHMap (set : hrefset<'a>) = set.ToHMap()

    // O(n)
    let inline toSeq (set : hrefset<'a>) = set.ToSeq()
    
    // O(n)
    let inline toList (set : hrefset<'a>) = set.ToList()

    // O(n)
    let inline toArray (set : hrefset<'a>) = set.ToArray()
    

    // O(1)
    let inline ofHMap (map : hmap<'a, int>) = hrefset.OfHMap map

    // O(n)
    let inline ofSeq (seq : seq<'a>) = hrefset.OfSeq seq

    // O(n)
    let inline ofList (list : list<'a>) = hrefset.OfList list

    // O(n)
    let inline ofArray (arr : 'a[]) = hrefset.OfArray arr




    // O(1)
    let inline isEmpty (set : hrefset<'a>) =
        set.IsEmpty

    // O(1)
    let inline count (set : hrefset<'a>) =
        set.Count

    // O(min(n,32))
    let inline add (value : 'a) (set : hrefset<'a>) =
        set.Add value
        
    // O(min(n,32))
    let inline remove (value : 'a) (set : hrefset<'a>) =
        set.Remove value

    // O(n + m)
    let inline union (l : hrefset<'a>) (r : hrefset<'a>) =
        l.Union r

    // O(n)
    let inline map (mapping : 'a -> 'b) (set : hrefset<'a>) =
        set.Map mapping

    // O(n)
    let inline choose (mapping : 'a -> Option<'b>) (set : hrefset<'a>) =
        set.Choose mapping

    // O(n)
    let inline filter (predicate : 'a -> bool) (set : hrefset<'a>) =
        set.Filter predicate

    // O(sum ni)
    let inline collect (mapping : 'a -> hrefset<'b>) (set : hrefset<'a>) =
        set.Collect mapping

    // O(n)
    let inline iter (iterator : 'a -> unit) (set : hrefset<'a>) =
        set.Iter iterator

    // O(n)
    let inline exists (predicate : 'a -> bool) (set : hrefset<'a>) =
        set.Exists predicate

    // O(n)
    let inline forall (predicate : 'a -> bool) (set : hrefset<'a>) =
        set.Forall predicate

    // O(n)
    let inline fold (folder : 's -> 'a -> 's) (seed : 's) (set : hrefset<'a>) =
        set.Fold(seed, folder)



    // O(n + m)
    let inline computeDelta (src : hrefset<'a>) (dst : hrefset<'a>) =
        src.ComputeDelta dst

    // O(|delta| * min(32, n))
    let inline applyDelta (set : hrefset<'a>) (delta : hdeltaset<'a>) =
        set.ApplyDelta delta

    // O(|delta| * min(32, n))
    let inline applyDeltaNoRefCount (set : hrefset<'a>) (delta : hdeltaset<'a>) =
        set.ApplyDeltaNoRefCount delta
