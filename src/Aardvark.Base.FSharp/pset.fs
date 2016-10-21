namespace Aardvark.Base

open System.Collections
open System.Collections.Generic


module private List =
    let inline readonly() = failwith "[pset] readonly"

    let rec tryAdd (v : 'a) (l : list<'a>) =
        match l with
            | [] -> Some [v]
            | h :: rest ->
                if Unchecked.equals h v then
                    None
                else
                    match tryAdd v rest with
                        | Some l -> Some (h :: l)
                        | None -> None

    let rec tryRemove (v : 'a) (l : list<'a>) =
        match l with
            | [] -> None
            | h :: rest ->
                if Unchecked.equals h v then
                    Some rest
                else
                    match tryRemove v rest with
                        | Some l -> Some (h :: l)
                        | None -> None

/// <summary>
/// Delta defines the two basic set-operations
/// add and remove with their respective value
/// </summary>
type Delta<'a> = 
    | Add of 'a 
    | Rem of 'a with

    member x.Value =
        match x with
            | Add v -> v
            | Rem v -> v



type pset<'a>(count : int, store : Map<int, list<'a>>) =
    static let empty = pset<'a>(0, Map.empty)

    static member Empty = empty

    member x.Count = count

    member x.Add (v : 'a) =
        let hash = Unchecked.hash v
        match Map.tryFind hash store with
            | Some l ->
                match List.tryAdd v l with
                    | Some l -> pset(count + 1, Map.add hash l store)
                    | None -> x
            | None ->
                pset(count + 1, Map.add hash [v] store)

    member x.Remove (v : 'a) =
        let hash = Unchecked.hash v
        match Map.tryFind hash store with
            | Some l ->
                match List.tryRemove v l with
                    | Some l -> 
                        match l with
                            | [] -> pset(count - 1, Map.remove hash store)
                            | l -> pset(count - 1, Map.add hash l store)
                    | None ->
                        x
            | None ->
                x

    member x.Contains (v : 'a) =
        let hash = Unchecked.hash v
        match Map.tryFind hash store with
            | Some l ->
                l |> List.exists (Unchecked.equals v)
            | None ->
                false

    member x.Union (other : seq<'a>) =
        let mutable res = x
        for e in other do res <- res.Add e
        res

    member x.Difference (other : seq<'a>) =
        let mutable res = x
        for e in other do res <- res.Remove e
        res

    member x.AsSeq = store |> Map.toSeq |> Seq.collect snd
    member x.AsList = store |> Map.toList |> List.collect snd
    member x.AsArray =
        let res = Array.zeroCreate count
        let mutable i = 0
        for e in x.AsList do
            res.[i] <- e
            i <- i + 1
        res

    member x.CopyTo(arr : 'a[], index : int) =
        let mutable index = index
        for e in x.AsList do
            arr.[index] <- e
            index <- index + 1

    member x.ComputeDelta(other : ISet<'a>) =
        let removed = x.AsSeq |> Seq.filter (other.Contains >> not) |> Seq.map Rem
        let added = other |> Seq.filter (x.Contains >> not) |> Seq.map Add
        Seq.append added removed |> Seq.toList

    member x.IsSubsetOf(other : seq<'a>) =
        let other = 
            match other with
                | :? ICollection<'a> as o -> o
                | _ -> HashSet<'a>(other) :> ICollection<_>
                
        if other.Count < count then false
        else x.AsSeq |> Seq.forall other.Contains

    member x.IsSupersetOf(other : seq<'a>) = 
        other |> Seq.forall x.Contains

    member x.IsProperSubsetOf(other : seq<'a>) =
        let other = 
            match other with
                | :? ICollection<'a> as o -> o
                | _ -> HashSet<'a>(other) :> ICollection<_>
                
        if other.Count < count then false
        else
            let subset = x.AsSeq |> Seq.forall other.Contains
            subset && count < other.Count

    member x.IsProperSupersetOf(other : seq<'a>) = 
        let other = 
            match other with
                | :? ICollection<'a> as o -> o
                | _ -> HashSet<'a>(other) :> ICollection<_>
                
        if other.Count > count then false
        else
            let superset = other |> Seq.forall x.Contains
            superset && count > other.Count

    member x.SetEquals(other : seq<'a>) = 
        let other = 
            match other with
                | :? ICollection<'a> as o -> o
                | _ -> HashSet<'a>(other) :> ICollection<_>
                
        if other.Count <> count then false
        else x.AsSeq |> Seq.forall other.Contains


    member x.Overlaps(other : seq<'a>) =
        other |> Seq.exists x.Contains

    static member OfSeq(s : seq<'a>) =
        let mutable res = empty
        for e in s do res <- res.Add e
        res

    static member OfList(l : list<'a>) = pset<'a>.OfSeq l
    static member OfArray(l : array<'a>) = pset<'a>.OfSeq l


    interface IEnumerable with
        member x.GetEnumerator() = new PSetEnumerator<'a>(store) :> _

    interface IEnumerable<'a> with
        member x.GetEnumerator() = new PSetEnumerator<'a>(store) :> _

    interface ISet<'a> with
        member x.Add(item: 'a): bool = List.readonly()
        member x.Add(item: 'a): unit = List.readonly()
        member x.Clear(): unit = List.readonly()
        member x.ExceptWith(other: IEnumerable<'a>): unit = List.readonly()
        member x.IntersectWith(other: IEnumerable<'a>): unit = List.readonly()
        member x.IsProperSubsetOf(other: IEnumerable<'a>): bool = List.readonly()
        member x.IsProperSupersetOf(other: IEnumerable<'a>): bool = List.readonly()
        member x.Remove(item: 'a): bool = List.readonly()
        member x.SymmetricExceptWith(other: IEnumerable<'a>): unit = List.readonly()
        member x.UnionWith(other: IEnumerable<'a>): unit = List.readonly()


        member x.Contains(item: 'a): bool = x.Contains item
        member x.CopyTo(array: 'a [], arrayIndex: int): unit = x.CopyTo(array, arrayIndex)
        member x.Count: int = x.Count
        member x.IsReadOnly: bool = true

        member x.IsSubsetOf(other: IEnumerable<'a>): bool = x.IsSubsetOf other
        member x.IsSupersetOf(other: IEnumerable<'a>): bool = x.IsSupersetOf other
        member x.Overlaps(other: IEnumerable<'a>): bool = x.Overlaps other
        member x.SetEquals(other: IEnumerable<'a>): bool = x.SetEquals other

and private PSetEnumerator<'a>(store : Map<int, list<'a>>) =
    let outer = (store :> seq<_>).GetEnumerator()
    let mutable inner : list<'a> = []
    let mutable current = Unchecked.defaultof<'a>

    member x.MoveNext() =
        match inner with
            | [] ->
                if outer.MoveNext() then
                    inner <- outer.Current.Value
                    x.MoveNext()
                else
                    false
            | h :: rest ->
                current <- h
                inner <- rest
                true
        
    member x.Current = current

    member x.Reset() =
        outer.Reset()
        inner <- []
        current <- Unchecked.defaultof<'a>

    member x.Dispose() =
        outer.Dispose()
        inner <- []
        current <- Unchecked.defaultof<_>

    interface IEnumerator with
        member x.MoveNext() = x.MoveNext()
        member x.Current = x.Current :> obj
        member x.Reset() = x.Reset()

    interface IEnumerator<'a> with
        member x.Dispose() = x.Dispose()
        member x.Current = x.Current

module PSet =
    let inline empty<'a> = pset<'a>.Empty

    let inline ofSeq (s : seq<'a>) = pset<'a>.OfSeq s
    let inline ofList (l : list<'a>) = pset<'a>.OfList l
    let inline ofArray (a : 'a[]) = pset<'a>.OfArray a

    let inline toSeq (s : pset<'a>) = s.AsSeq
    let inline toList (s : pset<'a>) = s.AsList
    let inline toArray (s : pset<'a>) = s.AsArray

    let inline add (v : 'a) (s : pset<'a>) = s.Add v
    let inline remove (v : 'a) (s : pset<'a>) = s.Remove v
    let inline union (l : pset<'a>) (r : seq<'a>) = l.Union r
    let inline difference (l : pset<'a>) (r : seq<'a>) = l.Difference r

    let inline isEmpty (s : pset<'a>) = s.Count = 0
    let inline count (s : pset<'a>) = s.Count

    let inline isSubset (l : pset<'a>) (r : seq<'a>) = l.IsSubsetOf r
    let inline isSuperset (l : pset<'a>) (r : seq<'a>) = l.IsSupersetOf r
    let inline isProperSubset (l : pset<'a>) (r : seq<'a>) = l.IsProperSubsetOf r
    let inline isProperSuperset (l : pset<'a>) (r : seq<'a>) = l.IsProperSupersetOf r
    let inline setEquals (l : pset<'a>) (r : seq<'a>) = l.SetEquals r
    let inline overlaps (l : pset<'a>) (r : seq<'a>) = l.Overlaps r

    let inline computeDelta (l : pset<'a>) (r : ISet<'a>) = l.ComputeDelta r
