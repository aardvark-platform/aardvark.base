namespace Aardvark.Base

open System.Collections
open System.Collections.Generic

module private HMapList =
    let rec alter (k : 'k) (f : Option<'v> -> Option<'v>) (l : list<'k * 'v>) =
        match l with
            | [] ->
                match f None with
                    | None -> []
                    | Some v -> [k,v]

            | (k1, v1) :: rest ->
                if Unchecked.equals k k1 then
                    match f (Some v1) with
                        | None -> rest
                        | Some v2 -> (k1, v2) :: rest
                else
                    (k1, v1) :: alter k f rest

    let rec update (k : 'k) (f : Option<'v> -> 'v) (l : list<'k * 'v>) =
        match l with
            | [] -> 
                let v = f None
                [k,v]

            | (k1, v1) :: rest ->
                if Unchecked.equals k k1 then
                    let v2 = f (Some v1)
                    (k1, v2) :: rest
                else
                    (k1, v1) :: update k f rest

    let rec add (cnt : byref<int>) (k : 'k) (v : 'v) (l : list<'k * 'v>) =
        match l with
            | [] ->     
                cnt <- cnt + 1
                [k,v]
            | (k1, v1) :: rest ->
                if Unchecked.equals k k1 then
                    (k1, v) :: rest
                else
                    (k1, v1) :: add &cnt k v rest

    let rec remove (cnt : byref<int>) (k : 'k) (l : list<'k * 'v>) =
        match l with
            | [] -> []
            | (k1, v1) :: rest ->
                if Unchecked.equals k k1 then
                    cnt <- cnt - 1
                    rest
                else
                    (k1, v1) :: remove &cnt k rest

    let rec tryRemove (k : 'k) (l : list<'k * 'v>) =
        match l with
            | [] -> None
            | (k1,v1) :: rest ->
                if Unchecked.equals k k1 then
                    Some (v1, rest)
                else
                    match tryRemove k rest with
                        | None -> None
                        | Some(v,rest) -> Some(v, (k1,v1)::rest)

    let rec unionWith (f : 'k -> 'v -> 'v -> 'v) (l : list<'k * 'v>) (r : list<'k * 'v>) =
        let newL = 
            l |> List.map (fun (lk, lv) ->
                let other = r |> List.tryFind (fun (rk, rv) -> Unchecked.equals rk lk)
                match other with
                    | Some (_,rv) -> (lk, f lk lv rv)
                    | None -> lk, lv
            )
        let newR =
            r |> List.filter (fun (rk,_) ->
                l |> List.forall (fun (lk,_) -> not (Unchecked.equals lk rk))
            )

        newL @ newR

    let rec mergeWith (f : 'k -> Option<'a> -> Option<'b> -> 'c) (l : list<'k * 'a>) (r : list<'k * 'b>) =
        let newL = 
            l |> List.choose (fun (lk, lv) ->
                let other = r |> List.tryFind (fun (rk, rv) -> Unchecked.equals rk lk)
                match other with
                    | Some (_,rv) -> 
                        Some (lk, f lk (Some lv) (Some rv))
                    | None -> 
                        Some (lk, f lk (Some lv) None)
            )
        let newR =
            r |> List.choose (fun (rk,rv) ->
                if l |> List.forall (fun (lk,_) -> not (Unchecked.equals lk rk)) then
                    Some(rk, f rk None (Some rv))
                else 
                    None
            )

        newL @ newR

    let rec mergeWithOption (f : 'k -> Option<'a> -> Option<'b> -> Option<'c>) (l : list<'k * 'a>) (r : list<'k * 'b>) =
        let newL = 
            l |> List.choose (fun (lk, lv) ->
                let other = r |> List.tryFind (fun (rk, rv) -> Unchecked.equals rk lk)
                match other with
                    | Some (_,rv) -> 
                        match f lk (Some lv) (Some rv) with
                            | Some r -> Some (lk, r)
                            | None -> None
                    | None -> 
                        match f lk (Some lv) None with
                            | Some r -> Some (lk, r)
                            | None -> None
            )
        let newR =
            r |> List.choose (fun (rk,rv) ->
                if l |> List.forall (fun (lk,_) -> not (Unchecked.equals lk rk)) then
                    match f rk None (Some rv) with
                        | Some r -> Some(rk, r)
                        | None -> None
                else 
                    None
            )

        newL @ newR

    let rec mergeWithOption' (f : 'k -> Option<'a> -> Option<'b> -> Option<'c>) (l : list<'k * 'a>) (r : list<'k * 'b>) =
        let newL = 
            l |> List.choose (fun (lk,lv) ->
                let other = r |> List.tryFind (fun (rk,_) -> Unchecked.equals rk lk) |> Option.map snd
 
                match f lk (Some lv) other with
                    | Some r -> Some (lk, r)
                    | None -> None
            )
        let newR =
            r |> List.choose (fun (rk, rv) ->
                if l |> List.forall (fun (lk,_) -> not (Unchecked.equals lk rk)) then
                    match f rk None (Some rv) with
                        | Some r -> Some(rk, r)
                        | None -> None
                else 
                    None
            )
        match newL with
            | [] -> 
                match newR with
                    | [] -> None
                    | _ -> Some newR
            | _ ->
                match newR with
                    | [] -> Some newL
                    | _ -> Some (newL @ newR)
[<Struct>]
[<StructuredFormatDisplay("{AsString}")>]
type hmap<'k, 'v>(cnt : int, store : intmap<list<'k * 'v>>) =
    static let empty = hmap<'k, 'v>(0, IntMap.empty)

    static member Empty = empty

    member internal x.Store = store

    member x.IsEmpty = cnt = 0

    member x.Count = cnt

    member x.Alter (key : 'k, f : Option<'v> -> Option<'v>) =
        let hash = Unchecked.hash key
        let mutable cnt = cnt
        let f old =
            let res = f old
            if Option.isNone old && Option.isSome res then cnt <- cnt + 1
            elif Option.isSome old && Option.isNone res then cnt <- cnt - 1
            res

        let newMap = 
            store |> IntMap.alter (fun l ->
                match l with
                    | Some l -> 
                        match HMapList.alter key f l with
                            | [] -> None
                            | l -> Some l

                    | None ->
                        match f None with
                            | None -> None
                            | Some v -> Some [key,v]
            ) hash

        hmap(cnt, newMap)

    member x.Update (key : 'k, f : Option<'v> -> 'v) =
        let mutable cnt = cnt
        let f old =
            if Option.isNone old then cnt <- cnt + 1
            f old

        let hash = Unchecked.hash key
        let newMap = 
            store |> IntMap.alter (fun l ->
                match l with
                    | Some l -> 
                        match HMapList.update key f l with
                            | [] -> None
                            | l -> Some l

                    | None ->
                        let v = f None
                        Some [key, v]
            ) hash

        hmap(cnt, newMap)

    member x.Add (key : 'k, value : 'v) =
        let hash = Unchecked.hash key
        let mutable cnt = cnt
        let newMap = 
            store |> IntMap.alter (fun l ->
                match l with
                    | None -> 
                        cnt <- cnt + 1
                        Some [key,value]
                    | Some l -> 
                        Some (HMapList.add &cnt key value l)
            ) hash
        hmap(cnt, newMap)

    member x.Remove (key : 'k) =
        let mutable cnt = cnt
        let hash = Unchecked.hash key
        let newMap = 
            store |> IntMap.update (fun l ->
                match HMapList.remove &cnt key l with
                    | [] -> None
                    | l -> Some l
            ) hash
        hmap(cnt, newMap)

    member x.ContainsKey (key : 'k) =
        let hash = Unchecked.hash key
        match IntMap.tryFind hash store with
            | Some l -> l |> List.exists (fun (k,_) -> Unchecked.equals k key)
            | None -> false
        
    member x.Map(mapping : 'k -> 'v -> 'b) =
        let newStore = 
            store 
                |> IntMap.map (fun l -> l |> List.map (fun (k,v) -> (k, mapping k v)))
        hmap(cnt, newStore)

    member x.Choose(mapping : 'k -> 'v -> Option<'b>) =
        let mutable cnt = 0
        let mapping (k : 'k, v : 'v) =
            match mapping k v with
                | Some b -> 
                    cnt <- cnt + 1
                    Some (k,b)
                | None -> 
                    None

        let newStore = 
            store
                |> IntMap.mapOption (fun l ->
                    match List.choose mapping l with
                        | [] -> None
                        | l -> 
                            
                            Some l
                   )
        hmap(cnt, newStore)

    member x.Filter(predicate : 'k -> 'v -> bool) =
        let mutable cnt = 0
        let predicate (k, v) =
            if predicate k v then
                cnt <- cnt + 1
                true
            else
                false

        let newStore = 
            store |> IntMap.mapOption (fun l ->
                match l |> List.filter predicate with
                    | [] -> None
                    | l -> Some l
            )
        hmap(cnt, newStore)

    member x.Iter(iter : 'k -> 'v -> unit) =
        store |> IntMap.toSeq |> Seq.iter (fun (_,l) ->
            l |> List.iter (fun (k,v) -> iter k v)
        )
        
    member x.Exists(predicate : 'k -> 'v -> bool) =
        store |> IntMap.toSeq |> Seq.exists (fun (_,v) ->
            v |> List.exists (fun (k,v) -> predicate k v)
        )

    member x.Forall(predicate : 'k -> 'v -> bool) =
        store |> IntMap.toSeq |> Seq.forall (fun (_,v) ->
            v |> List.forall (fun (k,v) -> predicate k v)
        )

    member x.Fold(seed : 's, folder : 's -> 'k -> 'v -> 's) =
        store |> IntMap.fold (fun s l ->
            l |> List.fold (fun s (k,v) -> folder s k v) s
        ) seed
        
    member x.UnionWith(other : hmap<'k, 'v>, f : 'k -> 'v -> 'v -> 'v) =
        let mutable cnt = cnt + other.Count
        let f k l r =
            cnt <- cnt - 1
            f k l r
            
        let newStore = 
            IntMap.appendWith (HMapList.unionWith f) store other.Store
        
        hmap(cnt, newStore)

    member x.Choose2(other : hmap<'k, 'a>, f : 'k -> Option<'v> -> Option<'a> -> Option<'c>) =
        let mutable cnt = 0
        let f k l r =
            match f k l r with
                | Some r -> 
                    cnt <- cnt + 1
                    Some r
                | None -> 
                    None

        let both (hash : int) (l : list<'k * 'v>) (r : list<'k * 'a>) =
            match HMapList.mergeWithOption f l r with
                | [] -> None
                | l -> Some l

        let onlyLeft (l : intmap<list<'k * 'v>>) =
            l |> IntMap.mapOption (fun l -> 
                match l |> List.choose (fun (lk, lv) -> match f lk (Some lv) None with | Some r -> Some (lk,r) | None -> None) with
                    | [] -> None
                    | l -> Some l
            )
            
        let onlyRight (r : intmap<list<'k * 'a>>) =
            r |> IntMap.mapOption (fun r -> 
                match r |> List.choose (fun (rk, rv) -> match f rk None (Some rv) with | Some r -> Some (rk,r) | None -> None) with
                    | [] -> None
                    | r -> Some r
            )

        let newStore =
            IntMap.mergeWithKey both onlyLeft onlyRight store other.Store

        hmap(cnt, newStore)

    member x.Map2(other : hmap<'k, 'a>, f : 'k -> Option<'v> -> Option<'a> -> 'c) =
        let mutable cnt = 0
        let f k l r =
            cnt <- cnt + 1
            f k l r

        let both (hash : int) (l : list<'k * 'v>) (r : list<'k * 'a>) =
            match HMapList.mergeWith f l r with
                | [] -> None
                | l -> Some l

        let onlyLeft (l : intmap<list<'k * 'v>>) =
            l |> IntMap.mapOption (fun l -> 
                match l |> List.map (fun (lk, lv) -> lk, f lk (Some lv) None) with
                    | [] -> None
                    | l -> Some l
            )
            
        let onlyRight (r : intmap<list<'k * 'a>>) =
            r |> IntMap.mapOption (fun r -> 
                match r |> List.map (fun (rk, rv) -> rk, f rk None (Some rv)) with
                    | [] -> None
                    | r -> Some r
            )

        let newStore =
            IntMap.mergeWithKey both onlyLeft onlyRight store other.Store

        hmap(cnt, newStore)

    member x.Union(other : hmap<'k, 'v>) =
        x.UnionWith(other, fun _ _ r -> r)

    member x.TryRemove(key : 'k) =
        let hash = Unchecked.hash key
        let mutable removed = None
        let newStore =
            store |> IntMap.update (fun o ->
                match HMapList.tryRemove key o with
                    | Some(v,l) ->
                        removed <- Some v
                        match l with
                            | [] -> None
                            | l -> Some l
                    | None -> Some o
            ) hash

        match removed with
            | Some rem -> Some(rem, hmap(cnt - 1, newStore))
            | None -> None
       
    member x.TryFind(key : 'k) =
        let hash = Unchecked.hash key
        match IntMap.tryFind hash store with
            | Some l ->
                l |> List.tryPick (fun (k,v) ->
                    if Unchecked.equals k key then
                        Some v
                    else
                        None
                )
            | None ->
                None

    member x.Find(key : 'k) =
        match x.TryFind key with
            | Some v -> v
            | None -> raise <| System.Collections.Generic.KeyNotFoundException()

    member x.Item
        with get (key : 'k) = x.Find key

    member x.ToSeq() =
        store |> IntMap.toSeq |> Seq.collect snd 

    member x.ToList() =
        store |> IntMap.toList |> List.collect snd 

    member x.ToArray() =
        store |> IntMap.toSeq |> Seq.collect snd |> Seq.toArray

    static member OfSeq (seq : seq<'k * 'v>) =
        let mutable res = empty
        for (k,v) in seq do
            res <- res.Add(k,v)
        res

    static member OfList (list : list<'k * 'v>) =
        hmap.OfSeq list
        
    static member OfArray (list : array<'k * 'v>) =
        hmap.OfSeq list

    override x.ToString() =
        let suffix =
            if x.Count > 5 then "; ..."
            else ""

        let content =
            x.ToSeq() |> Seq.truncate 5 |> Seq.map (sprintf "%A") |> String.concat "; "

        "hmap [" + content + suffix + "]"

    member private x.AsString = x.ToString()

    interface IEnumerable with
        member x.GetEnumerator() = 
            new HMapEnumerator<_,_>(store) :> _

    interface IEnumerable<'k * 'v> with
        member x.GetEnumerator() = 
            new HMapEnumerator<_,_>(store) :> _

and hdeltamap<'k, 'v> = hmap<'k, ElementOperation<'v>>


and private HMapEnumerator<'k, 'v>(m : intmap<list<'k * 'v>>) =
    
    let mutable stack = [m]
    let mutable inner = []
    let mutable current = Unchecked.defaultof<_>

    let rec moveNext() =
        match inner with
            | [] ->
                match stack with
                    | [] -> false
                    | h :: s ->
                        stack <- s
                        match h with
                            | Tip(k,v) -> 
                                match v with
                                    | [] -> failwith "asdasdsadasd"
                                    | v :: rest ->
                                        current <- v
                                        inner <- rest
                                        true

                            | Nil ->
                                moveNext()

                            | Bin(_,_,l,r) ->
                                stack <- l :: r :: stack
                                moveNext()

            | h :: rest ->
                current <- h
                inner <- rest
                true

    member x.MoveNext() =
        moveNext()

    member x.Current = current

    member x.Reset() =
        stack <- [m]
        inner <- []
        current <- Unchecked.defaultof<_>

    member x.Dispose() =
        stack <- []
        inner <- []
        current <- Unchecked.defaultof<_>


    interface IEnumerator with
        member x.MoveNext() = x.MoveNext()
        member x.Current = x.Current :> obj
        member x.Reset() = x.Reset()

    interface IEnumerator<'k * 'v> with
        member x.Current = x.Current
        member x.Dispose() = x.Dispose()



[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HMap =

    /// <summary>The empty map.</summary>
    [<GeneralizableValue>]
    let empty<'k, 'v> = hmap<'k, 'v>.Empty
    
    let inline single (k : 'k) (v : 'v) = 
        hmap.OfList [k,v]

    /// <summary>
    /// Returns a new map made from the given bindings. <para />
    /// O(n * min(n,32))
    /// </summary>
    let inline ofSeq (seq : seq<'k * 'v>) = 
        hmap.OfSeq seq

    /// <summary>
    /// Returns a new map made from the given bindings. <para />
    /// O(n * min(n,32))
    /// </summary>
    let inline ofMap (map : Map<'k, 'v>) = 
        map |> Map.toSeq |> ofSeq

    /// <summary>
    /// Returns a new map made from the given bindings. <para />
    /// O(n * min(n,32))
    /// </summary>
    let inline ofMapExt (map : MapExt<'k, 'v>) = 
        map |> MapExt.toSeq |> ofSeq
        
        
    /// <summary>
    /// Returns a new map made from the given bindings. <para />
    /// O(n * min(n,32))
    /// </summary>
    let inline ofList (list : list<'k * 'v>) = 
        hmap.OfList list
        
    /// <summary>
    /// Returns a new map made from the given bindings. <para />
    /// O(n * min(n,32))
    /// </summary>
    let inline ofArray (arr : array<'k * 'v>) = 
        hmap.OfArray arr

    
    /// <summary>
    /// Views the collection as an enumerable sequence of pairs. <para />
    /// O(n)
    /// </summary>
    let inline toSeq (map : hmap<'k, 'v>) = 
        map.ToSeq()

    /// <summary>
    /// Returns a list of all key-value pairs in the mapping. <para />
    /// O(n)
    /// </summary>
    let inline toList (map : hmap<'k, 'v>) = 
        map.ToList()

    /// <summary>
    /// Returns an array of all key-value pairs in the mapping. <para />
    /// O(n)
    /// </summary>
    let inline toArray (map : hmap<'k, 'v>) = 
        map.ToArray()

    /// <summary>
    /// Returns a map of all key-value pairs in the mapping. <para />
    /// O(n)
    /// </summary>
    let inline toMap (map : hmap<'k, 'v>) = 
        let mutable res = Map.empty
        for (k,v) in map do
            res <- Map.add k v res
        res

    /// <summary>
    /// Returns a map of all key-value pairs in the mapping. <para />
    /// O(n)
    /// </summary>
    let inline toMapExt (map : hmap<'k, 'v>) = 
        let mutable res = MapExt.empty
        for (k,v) in map do
            res <- MapExt.add k v res
        res


    /// <summary>
    /// Returns a new map with the binding added to the given map.
    /// If a binding with the given key already exists in the input map, the existing binding is replaced by the new binding in the result map. <para />
    /// O(min(n,32))
    /// </summary>
    let inline add (key : 'k) (value : 'v) (map : hmap<'k, 'v>) =
        map.Add(key, value)

    /// <summary>
    /// Removes an element from the domain of the map. No exception is raised if the element is not present. <para />
    /// O(min(n,32))
    /// </summary>
    let inline remove (key : 'k) (map : hmap<'k, 'v>) =
        map.Remove(key)

    // O(min(n,32))
    let inline alter (key : 'k) (mapping : Option<'v> -> Option<'v>) (map : hmap<'k, 'v>) =
        map.Alter(key, mapping)
        
    // O(min(n,32))
    let inline update (key : 'k) (mapping : Option<'v> -> 'v) (map : hmap<'k, 'v>) =
        map.Update(key, mapping)

    // O(n+m)
    let inline unionWith (resolve : 'k -> 'v -> 'v -> 'v) (l : hmap<'k, 'v>) (r : hmap<'k, 'v>) =
        l.UnionWith(r, resolve)
        
    // O(n+m)
    let inline union (l : hmap<'k, 'v>) (r : hmap<'k, 'v>) =
        l.Union r

    // O(min(n,32))
    let inline tryRemove (key : 'k) (map : hmap<'k, 'v>) =
        map.TryRemove key


    /// <summary>
    /// Builds a new collection whose elements are the results of applying the given function
    /// to each of the elements of the collection. The key passed to the
    /// function indicates the key of element being transformed. <para />
    /// O(n)
    /// </summary>
    let inline map (mapping : 'k -> 'a -> 'b) (map : hmap<'k, 'a>) =
        map.Map(mapping)
        
    // O(n)
    let inline choose (mapping : 'k -> 'a -> Option<'b>) (map : hmap<'k, 'a>) =
        map.Choose mapping

    // O(n)
    let inline filter (predicate : 'k -> 'v -> bool) (map : hmap<'k, 'v>) =
        map.Filter predicate

    // O(n)
    let inline iter (iter : 'k -> 'v -> unit) (map : hmap<'k, 'v>) =
        map.Iter iter

    // O(n)
    let inline fold (folder : 's -> 'k -> 'v -> 's) (seed : 's) (map : hmap<'k, 'v>) =
        map.Fold(seed, folder)
        
    // O(n)
    let inline exists (predicate : 'k -> 'v -> bool) (map : hmap<'k, 'v>) =
        map.Exists(predicate)

    // O(n)
    let inline forall (predicate : 'k -> 'v -> bool) (map : hmap<'k, 'v>) =
        map.Forall(predicate)

    // O(n+m)
    let inline map2 (mapping : 'k -> Option<'a> -> Option<'b> -> 'c) (l : hmap<'k, 'a>) (r : hmap<'k, 'b>) =
        l.Map2(r, mapping)

    // O(n+m)
    let inline choose2 (mapping : 'k -> Option<'a> -> Option<'b> -> Option<'c>) (l : hmap<'k, 'a>) (r : hmap<'k, 'b>) =
        l.Choose2(r, mapping)




    // O(min(n,32))
    let inline tryFind (key : 'k) (map : hmap<'k, 'v>) =
        map.TryFind key
        
    // O(min(n,32))
    let inline find (key : 'k) (map : hmap<'k, 'v>) =
        map.Find key
        
    // O(min(n,32))
    let inline containsKey (key : 'k) (map : hmap<'k, 'v>) =
        map.ContainsKey key

    // O(1)
    let inline count (map : hmap<'k, 'v>) = map.Count

    // O(1)
    let inline isEmpty (map : hmap<'k, 'v>) = map.IsEmpty


    module Lens =
        let item (key : 'k) =
            { new Lens<_, _>() with
                member x.Get s = 
                    tryFind key s

                member x.Set(s,r) =
                    match r with
                        | Some r -> add key r s
                        | None -> remove key s

                member x.Update(s,f) =
                    alter key f s
            }  
