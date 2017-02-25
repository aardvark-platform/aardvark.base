namespace Aardvark.Base

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base

module private ListHelpers =
    
    let rec alter (key : 'k) (f : Option<'v> -> Option<'v>) (l : list<'k * 'v>) =
        match l with
            | [] ->
                match f None with
                    | Some v -> [key,v], None, Some v
                    | None -> [], None, None

            | ((hk,hv) as h) :: rest ->
                if Unchecked.equals hk key then
                    match f (Some hv) with
                        | Some v -> ((key,v) :: rest), Some hv, Some v
                        | None -> rest, Some hv, None
                else
                    let res, o, n = alter key f rest
                    (h :: res, o, n)

/// a persistent hashmap-implementation
[<StructuredFormatDisplay("{AsString}")>]
type pmap<'k, 'v> private(count : int, content : Map<int, list<'k * 'v>>) =
    static let empty = pmap<'k, 'v>(0, Map.empty)

    static member Empty = empty

    member inline private x.alter (key : 'k) (f : Option<'v> -> Option<'v>) =
        let hash = Unchecked.hash key

        match Map.tryFind hash content with
            | Some list ->
                let list, o, n = ListHelpers.alter key f list

                let add hash list content =
                    match list with
                        | [] -> Map.remove hash content
                        | _ -> Map.add hash list content

                let newMap = 
                    match o, n with
                        | None, None -> x
                        | Some o, Some n when Unchecked.equals o n -> x
                        | Some _, Some _ -> pmap(count, add hash list content)
                        | Some o, None -> pmap(count - 1, add hash list content)
                        | None, Some n -> pmap(count + 1, add hash list content)
                newMap, o, n
            | _ ->
                match f None with
                    | Some v -> pmap(count + 1, Map.add hash [key, v] content), None, Some v
                    | None -> x, None, None

    member inline private x.alter' (key : 'k)(f : Option<'v> -> Option<'v>) =
        let (m,_,_) = x.alter key f
        m

    member inline private x.alter2 (other : pmap<'k, 'v>, value : 'k -> Option<'v> -> Option<'v> -> Option<'v>) =
        let mutable res = x
        let mutable self = x
        let mutable other = other
        for (k,v) in other.AsSeq do
            res <- res.alter' k (fun o -> value k o (Some v))
            self <- self.Remove k

        for (k,v) in self.AsSeq do
            res <- res.alter' k (fun o -> value k o None)

        res



    member x.Count = count

    member x.Alter (key : 'k, update : Option<'v> -> Option<'v>) = x.alter' key update
    member x.Update (key : 'k, update : Option<'v> -> 'v) = x.alter' key (fun o -> Some (update o))
    member x.AddOrUpdate (key : 'k, create : unit -> 'v, update : 'v -> 'v) = x.alter' key (function Some o -> Some (update o) | None -> Some (create()))
    member x.Add (key : 'k, value : 'v) = x.alter' key (fun _ -> Some value)
    member x.Remove (key : 'k) = x.alter' key (fun _ -> None)

    member x.TryRemove (key : 'k) = 
        let m, o, _ = x.alter key (fun _ -> None)
        match o with
            | Some o -> Some (m, o)
            | None -> None

    member x.GetOrCreate(key : 'k, f : 'k -> 'v) =
        let map, _, n =
            x.alter key (fun o ->
                match o with
                    | Some o -> Some o
                    | None -> Some (f key)
            )
        map, n.Value

    member x.TryFind (key : 'k) =
        let hash = Unchecked.hash key
        match Map.tryFind hash content with
            | Some l ->
                l |> List.tryPick (fun (k,v) -> if Unchecked.equals k key then Some v else None)
            | None ->
                None

    member x.ContainsKey (key : 'k) =
        let hash = Unchecked.hash key
        match Map.tryFind hash content with
            | Some l ->
                l |> List.exists (fun (k,v) -> Unchecked.equals k key)
            | None ->
                false

    member x.AsSeq = content |> Map.toSeq |> Seq.collect snd

    member x.AsList = content |> Map.toList |> List.collect snd

    member x.AsArray =
        let arr = Array.zeroCreate count
        let mutable index = 0
        for (KeyValue(k,v)) in content do
            for e in v do
                arr.[index] <- e
                index <- index + 1
        arr


    static member OfSeq (s : seq<'k * 'v>) =
        let rec set (k : 'k) (t : 'k * 'v) (l : list<'k * 'v>) =
            match l with
                | [] -> [t], true
                | ((hk,_) as h) :: rest ->
                    if Unchecked.equals hk k then t :: rest, false
                    else 
                        let l, isNew = set k t rest
                        h :: l, isNew

        let mutable cnt = 0
        let mutable res = Map.empty
        for ((k, _) as t) in s do
            let hash = Unchecked.hash k
            let newList, isNew = 
                match Map.tryFind hash res with
                    | Some l -> set k t l
                    | None -> [t], true

            res <- Map.add hash newList res
            if isNew then cnt <- cnt + 1

        pmap(cnt, res)

    static member OfList (l : list<'k * 'v>) =
        pmap<'k, 'v>.OfSeq l

    static member OfArray (l : array<'k * 'v>) =
        pmap<'k, 'v>.OfSeq l


    member x.Union (other : pmap<'k, 'v>) =
        x.alter2(other, fun _ m o ->
            match o with
                | Some o -> Some o
                | None -> m
        )

    member x.UnionWith (f : 'k -> 'v -> 'v -> Option<'v>, other : pmap<'k, 'v>) =
        x.alter2(other, fun k m o ->
            match o with
                | Some o -> 
                    match m with
                        | Some m -> f k o m
                        | _ -> Some o
                | None -> m
        )

    member x.Alter2 (f : 'k -> Option<'v> -> Option<'v> -> Option<'v>, other : pmap<'k, 'v>) =
        x.alter2(other, f)

    member x.Map (f : 'k -> 'v -> 'r) : pmap<'k, 'r> =
        let mutable res = pmap<'k, 'r>.Empty
        for (k,v) in x.AsArray do
            res <- res.Add(k, f k v)
        res

    member private x.AsString =
        x.ToString()

    

    override x.ToString() =
        content |> Map.toSeq |> Seq.collect snd |> Seq.map (fun (k,v) -> sprintf "%A->%A" k v) |> String.concat "; " |> sprintf "pmap [%s]"

    interface IEnumerable with
        member x.GetEnumerator() = new PMapEnumerator<'k, 'v>(content) :> IEnumerator

    interface IEnumerable<'k * 'v> with
        member x.GetEnumerator() = new PMapEnumerator<'k, 'v>(content) :> IEnumerator<_>

and private PMapEnumerator<'k, 'v>(content : Map<int, list<'k * 'v>>) =
    let outer = (content :> seq<_>).GetEnumerator()
    let mutable inner : Option<IEnumerator<_>> = None

    member x.MoveNext() =
        match inner with
            | Some i ->
                if i.MoveNext() then
                    true
                else
                    i.Dispose()
                    inner <- None
                    x.MoveNext()
            | None ->
                if outer.MoveNext() then
                    inner <- (outer.Current.Value :> seq<_>).GetEnumerator() |> Some
                    x.MoveNext()
                else
                    false

    member x.Current =
        match inner with
            | Some i -> i.Current
            | None -> failwith "PMap enumerator not moved"

    member x.Reset() =
        match inner with
            | Some i -> 
                i.Dispose()
                inner <- None
            | None -> 
                ()
        outer.Reset()

    member x.Dispose() =
        match inner with
            | Some i -> 
                i.Dispose()
                inner <- None
            | None -> 
                ()
        outer.Dispose()
        
    interface IEnumerator with
        member x.MoveNext() = x.MoveNext()
        member x.Current = x.Current :> obj
        member x.Reset() = x.Reset()

    interface IEnumerator<'k * 'v> with
        member x.Current = x.Current
        member x.Dispose() = x.Dispose()


module PMap =
    /// an empty map
    let inline empty<'k, 'v> = pmap<'k, 'v>.Empty

    /// creates a map containing a single key-value-pair
    let inline single (key : 'k) (value : 'v) = pmap<'k, 'v>.OfSeq [ key, value ]

    /// creates a sequence containing all map-entries as tuples
    let inline toSeq (m : pmap<'k, 'v>) = m.AsSeq

    /// creates a list containing all map-entries as tuples
    let inline toList (m : pmap<'k, 'v>) = m.AsList

    /// creates an array containing all map-entries as tuples
    let inline toArray (m : pmap<'k, 'v>) = m.AsArray

    /// creates a map containing all entries from the griven sequence (override semantic)
    let inline ofSeq (m : seq<'k * 'v>) = pmap<'k, 'v>.OfSeq m

    /// creates a map containing all entries from the griven list (override semantic)
    let inline ofList (m : list<'k * 'v>) = pmap<'k, 'v>.OfList m

    /// creates a map containing all entries from the griven array (override semantic)
    let inline ofArray (m : array<'k * 'v>) = pmap<'k, 'v>.OfArray m

    /// creates a new map by adding the given key-value-pair
    let inline add (key : 'k) (value : 'v) (m : pmap<'k, 'v>) = m.Add(key, value)

    /// creates a new map by removing the given key (and its value) from the map
    let inline remove (key : 'k) (m : pmap<'k, 'v>) = m.Remove(key)

    /// creates a new map containing the elements from both given ones (override semantic)
    let inline union (l : pmap<'k, 'v>) (r : pmap<'k, 'v>) = l.Union r

    /// creates a new map containing the elements from both given ones
    /// and resolves conflicts using the given function
    let inline unionWith (f : 'k -> 'v -> 'v -> Option<'v>) (l : pmap<'k, 'v>) (r : pmap<'k, 'v>) = l.UnionWith(f,r)

    /// gets or creates the value associated with key and returns the (possibly) new map accompanied by the value
    let inline getOrCreate (key : 'k) (create : 'k -> 'v) (m : pmap<'k, 'v>) = m.GetOrCreate(key, create)

    /// tries to remove the given key (and its value) from the map and optionally returns a new map (with the key removed) and the value
    let inline tryRemove (key : 'k) (m : pmap<'k, 'v>) = m.TryRemove(key)

    /// tries to find the value associated with the given key
    let inline tryFind (key : 'k) (m : pmap<'k, 'v>) = m.TryFind key

    /// creates a new map combining the given ones using the given function
    let inline alter2 (f : 'k -> Option<'v> -> Option<'v> -> Option<'v>) (l : pmap<'k, 'v>) (r : pmap<'k, 'v>) = l.Alter2(f,r)
    
    /// creates a new map using the given function on the value for key
    let inline alter (key : 'k) (f : Option<'v> -> Option<'v>) (m : pmap<'k, 'v>) = m.Alter(key, f)

    /// returns the element-count for the given map O(1)
    let inline count (m : pmap<'k, 'v>) = m.Count

    /// returns true if the map is empty
    let inline isEmpty (m : pmap<'k, 'v>) = m.Count = 0

    /// creates a new map by applying the given functions to all key-value-pairs
    let inline map (f : 'k -> 'v -> 'r) (m : pmap<'k, 'v>) = m.Map f
    
    /// creates a new map using the given function on the value for key
    let inline update (key : 'k) (f : Option<'v> -> 'v) (m : pmap<'k, 'v>) =
        m.Alter(key, fun o -> Some (f o))


