namespace Aardvark.Base

open System.Collections
open System.Collections.Generic

module private HSetList =
    let rec add (cnt : byref<int>) (value : 'a) (list : list<'a>) =
        match list with
            | [] -> 
                cnt <- cnt + 1
                [value]
            | h :: tail ->
                if Unchecked.equals h value then
                    list
                else
                    h :: add &cnt value tail

    let rec remove (cnt : byref<int>) (value : 'a) (list : list<'a>) =
        match list with
            | [] ->
                []
            | h :: tail ->
                if Unchecked.equals h value then
                    cnt <- cnt - 1
                    tail
                else
                    h :: remove &cnt value tail

    let rec union (dupl : byref<int>) (l : list<'a>) (r : list<'a>) =
        let mutable d = dupl
        let newR = 
            r |> List.filter (fun r ->
                if l |> List.exists (Unchecked.equals r) then
                    d <- d + 1
                    false
                else
                    true
            )

        dupl <- d
        l @ newR

    let rec difference (cnt : byref<int>) (l : list<'a>) (r : list<'a>) =
        match l with
            | [] -> []
            | h :: tail ->
                if List.exists (Unchecked.equals h) r then
                    difference &cnt tail r
                else
                    cnt <- cnt + 1
                    h :: difference &cnt tail r

    let rec intersect (cnt : byref<int>) (l : list<'a>) (r : list<'a>) =
        match l with
            | [] -> []
            | h :: tail ->
                if List.exists (Unchecked.equals h) r then
                    cnt <- cnt + 1
                    h :: intersect &cnt tail r
                else
                    intersect &cnt tail r

[<StructuredFormatDisplay("{AsString}")>]
type hset<'a>(cnt : int, store : intmap<list<'a>>) =
    static let empty = hset(0, IntMap.empty)

    static member Empty : hset<'a> = empty
    
    member private x.Store = store

    member x.Count = cnt

    member x.IsEmpty = cnt = 0

    member x.Add (value : 'a) =
        let hash = Unchecked.hash value
        let mutable cnt = cnt

        let newStore = 
            store |> IntMap.alter (fun o ->
                match o with
                    | None -> 
                        cnt <- cnt + 1 
                        Some [value]
                    | Some old -> 
                        HSetList.add &cnt value old |> Some
            ) hash

        hset(cnt, newStore)

    member x.Remove (value : 'a) =
        let hash = Unchecked.hash value
        let mutable cnt = cnt
        
        let newStore = 
            store |> IntMap.alter (fun o ->
                match o with
                    | None -> None
                    | Some old -> HSetList.remove &cnt value old |> Some
            ) hash

        hset(cnt, newStore)

    member x.Contains (value : 'a) =
        let hash = Unchecked.hash value
        match IntMap.tryFind hash store with
            | Some l -> l |> List.exists (Unchecked.equals value)
            | None -> false
        
    member x.Alter(key : 'a, f : bool -> bool) =
        let hash = Unchecked.hash key
        let mutable cnt = cnt

        let newStore =  
            store |> IntMap.alter (fun ol ->
                match ol with
                    | None ->
                        if f false then
                            cnt <- cnt + 1
                            Some [key]
                        else
                            None
                    | Some ol ->
                        let mutable was = List.exists (Unchecked.equals key) ol
                        let should = f was
                        if should && not was then 
                            cnt <- cnt + 1
                            Some (key :: ol)
                        elif not should && was then
                            cnt <- cnt - 1
                            match List.filter (Unchecked.equals key >> not) ol with
                                | [] -> None
                                | l -> Some l
                        else
                            Some ol
            ) hash

        hset(cnt, newStore)

    member x.Map (mapping : 'a -> 'b) =
        let mutable res = hset.Empty
        for e in x.ToSeq() do
            res <- res.Add(mapping e)
        res

    member x.Choose (mapping : 'a -> Option<'b>) =
        let mutable res = hset.Empty
        for e in x.ToSeq() do
            match mapping e with
                | Some e ->
                    res <- res.Add(e)
                | None ->
                    ()
        res


    member x.Filter (predicate : 'a -> bool) =
        let mutable cnt = 0
        let predicate v =
            if predicate v then
                cnt <- cnt + 1
                true
            else
                false

        let newStore =
            store |> IntMap.mapOption (fun l ->
                match List.filter predicate l with
                    | [] -> None
                    | l -> Some l
            )

        hset(cnt, newStore)

    member x.Collect (mapping : 'a -> hset<'b>) =
        let mutable res = hset<'b>.Empty
        for (_,l) in IntMap.toSeq store do
            for e in l do
                res <- res.Union (mapping e)
        res

    member x.Iter (iter : 'a -> unit) =
        store |> IntMap.toSeq |> Seq.iter (fun (_,l) -> l |> List.iter iter)

    member x.Exists (predicate : 'a -> bool) =
        store |> IntMap.toSeq |> Seq.exists (fun (_,l) -> l |> List.exists predicate)

    member x.Forall (predicate : 'a -> bool) =
        store |> IntMap.toSeq |> Seq.forall (fun (_,l) -> l |> List.forall predicate)

    member x.Fold (seed : 's, folder : 's -> 'a -> 's) =
        store |> IntMap.toSeq |> Seq.fold (fun s (_,l) ->
            l |> List.fold folder s
        ) seed

    member x.Union (other : hset<'a>) : hset<'a> =
        let mutable dupl = 0
        let newStore = IntMap.appendWith (fun l r -> HSetList.union &dupl l r) store other.Store
        hset(cnt + other.Count - dupl, newStore)

    member x.Difference (other : hset<'a>) : hset<'a> =
        let mutable cnt = 0
        let newStore =
            IntMap.mergeWithKey 
                (fun k ll rl -> match HSetList.difference &cnt ll rl with [] -> None | l -> Some l) 
                (fun l -> cnt <- l |> IntMap.fold (fun s l -> s + List.length l) cnt; l)
                (fun r -> IntMap.empty) 
                store 
                other.Store

        hset(cnt, newStore)

    member x.Intersect (other : hset<'a>) : hset<'a> =
        let mutable cnt = 0
        let newStore =
            IntMap.mergeWithKey 
                (fun k ll rl -> match HSetList.intersect &cnt ll rl with [] -> None | l -> Some l) 
                (fun l -> IntMap.empty)
                (fun r -> IntMap.empty) 
                store 
                other.Store

        hset(cnt, newStore)
        
    member x.ToSeq() =
        store |> IntMap.toSeq |> Seq.collect snd

    member x.ToList() =
        store |> IntMap.toList |> List.collect snd

    member x.ToArray() =
        x.ToSeq() |> Seq.toArray

    static member OfSeq (seq : seq<'a>) =
        let mutable res = empty
        for e in seq do
            res <- res.Add e
        res

    static member OfList (list : list<'a>) =
        hset.OfSeq list

    static member OfArray (arr : array<'a>) =
        hset.OfSeq arr

    override x.ToString() =
        let suffix =
            if x.Count > 5 then "; ..."
            else ""

        let content =
            x.ToSeq() |> Seq.truncate 5 |> Seq.map (sprintf "%A") |> String.concat "; "

        "hset [" + content + suffix + "]"

    member private x.AsString = x.ToString()

    interface IEnumerable with
        member x.GetEnumerator() = x.ToSeq().GetEnumerator() :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = x.ToSeq().GetEnumerator()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module HSet =

    /// <summary>The empty set.</summary>
    [<GeneralizableValue>]
    let empty<'a> = hset<'a>.Empty

    let inline ofSeq (seq : seq<'a>) =
        hset.OfSeq seq

    let inline ofList (list : list<'a>) =
        hset.OfList list

    let inline ofArray (arr : 'a[]) =
        hset.OfArray arr


    let inline toSeq (set : hset<'a>) =
        set.ToSeq()

    let inline toList (set : hset<'a>) =
        set.ToList()

    let inline toArray (set : hset<'a>) =
        set.ToArray()


    let inline add (value : 'a) (set : hset<'a>) =
        set.Add value

    let inline remove (value : 'a) (set : hset<'a>) =
        set.Remove value

    let inline alter (value : 'a) (mapping : bool -> bool) (set : hset<'a>) =
        set.Alter(value, mapping)

    let inline union (l : hset<'a>) (r : hset<'a>) =
        l.Union r

    let inline unionMany (sets : seq<hset<'a>>) =
        sets |> Seq.fold union empty

    let inline difference (l : hset<'a>) (r : hset<'a>) =
        l.Difference r

    let inline intersect (l : hset<'a>) (r : hset<'a>) =
        l.Intersect r

    let inline map (mapping : 'a -> 'b) (set : hset<'a>) =
        set.Map mapping

    let inline choose (mapping : 'a -> Option<'b>) (set : hset<'a>) =
        set.Choose mapping

    let inline filter (predicate : 'a -> bool) (set : hset<'a>) =
        set.Filter predicate

    let inline collect (mapping : 'a -> hset<'b>) (set : hset<'a>) =
        set.Collect mapping
        
    let inline iter (mapping : 'a -> unit) (set : hset<'a>) =
        set.Iter mapping

    let inline exists (predicate : 'a -> bool) (set : hset<'a>) =
        set.Exists predicate

    let inline forall (predicate : 'a -> bool) (set : hset<'a>) =
        set.Forall predicate

    let inline fold (folder : 's -> 'a -> 's) (seed : 's) (set : hset<'a>) =
        set.Fold(seed, folder)

    
    let inline isEmpty (set : hset<'a>) =
        set.IsEmpty

    let inline count (set : hset<'a>) =
        set.Count

    let inline contains (value : 'a) (set : hset<'a>) =
        set.Contains value


    module Lens =
        let contains (key : 'k) =
            { new Lens<_, _>() with
                member x.Get s = 
                    contains key s

                member x.Set(s,r) =
                    match r with
                        | true -> add key s
                        | false -> remove key s

                member x.Update(s,f) =
                    alter key f s
            }  