namespace Aardvark.Base.Incremental

open System.Collections
open System.Collections.Generic
open System.Collections.Concurrent

/// <summary>
/// represents a set of elements having a reference count.
/// this means that an element is contained when it has been
/// added at least once more than removed.
/// </summary>
type ReferenceCountingSet<'a>(initial : seq<'a>) =
    let mutable nullCount = 0
    let store = Dictionary<obj, 'a * ref<int>>()

    let toCollection (s : seq<'a>) =
        match s with
            | :? ICollection<'a> as s -> s
            | _ -> HashSet s :> ICollection<_>

    let compareSeq (other : seq<'a>) =
        let distinctOther = toCollection other

        let mutable both = 0
        let mutable onlyMe = 0

        for (o,_) in store.Values do
            if distinctOther.Contains o then
                both <- both + 1
            else
                onlyMe <- onlyMe + 1

        let onlyOther = distinctOther.Count - both

        (both, onlyMe, onlyOther)


    let add (v : 'a) =
        if (v :> obj) = null then
            nullCount <- nullCount + 1
            nullCount = 1
        else 
            match store.TryGetValue v with
                | (true, (_,r)) -> 
                    r := !r + 1
                    false
                | _ ->
                    let r = v, ref 1
                    store.[v] <- r
                    true

    let remove (v : 'a) =
        if (v :> obj) = null then
            nullCount <- nullCount - 1
            nullCount = 0
        else 
            match store.TryGetValue v with
                | (true, (_,r)) ->
                    r := !r - 1
                    if !r = 0 then
                        store.Remove v
                    else
                        false
                | _ ->
                    false 

    do for e in initial do
        add e |> ignore

    /// <summary>
    /// adds an element to the ReferenceCountingSet and returns
    /// true if the element was not contained in the set before
    /// this operation.
    /// </summary>
    member x.Add (v : 'a) = add v

    /// <summary>
    /// removes an element from the ReferenceCountingSet and returns
    /// true if the element is no longer contained after the operation.
    /// </summary>
    member x.Remove(v : 'a) = remove v

    /// <summary>
    /// checks if the set contains a specific element
    /// </summary>
    member x.Contains (v : 'a) =
        if (v :> obj) = null then
            nullCount > 0
        else
            store.ContainsKey v

    /// <summary>
    /// clears the entire set
    /// </summary>
    member x.Clear() =
        nullCount <- 0
        store.Clear()

    /// <summary>
    /// returns the number of (distinct) elements contained in
    /// the set.
    /// </summary>
    member x.Count = 
        (if nullCount > 0 then 1 else 0) + store.Count


    /// <summary>
    /// gets the current reference count for the given element
    /// </summary>
    member x.GetReferenceCount(v) =
        if (v :> obj) = null then nullCount
        else
            match store.TryGetValue (v :> obj) with
                | (true, (_,c)) -> !c
                | _ -> 0

    /// <summary>
    /// Remove items in other from this set. Modifies this set.
    /// </summary>  
    member x.ExceptWith (items : seq<'a>) =
        for o in items do
            x.Remove o |> ignore

    /// <summary>
    /// Take the union of this set with other. Modifies this set.
    /// </summary>
    member x.UnionWith (other : seq<'a>) =
        for o in other do
            x.Add o |> ignore

    /// <summary>
    /// Takes the intersection of this set with other. Modifies this set.
    /// </summary>
    member x.IntersectWith (other : seq<'a>) =
        let other = toCollection other
        for (v,r) in store.Values do
            if not <| other.Contains v then
                x.Remove v |> ignore

    /// <summary>
    /// Takes symmetric difference (XOR) with other and this set. Modifies this set.
    /// </summary>
    member x.SymmetricExceptWith (other : seq<'a>) =
        for o in other do
            if not <| x.Remove o then
                x.Add o |> ignore

    /// <summary>
    /// determines if the set is a subset of the given sequence
    /// </summary>
    member x.IsSubsetOf (other : seq<'a>) =
        match compareSeq other with
            | (_, 0, _) -> true
            | _ -> false

    /// <summary>
    /// determines if the set is a superset of the given sequence
    /// </summary>
    member x.IsSupersetOf (other : seq<'a>) =
        match compareSeq other with
            | (_, _, 0) -> true
            | _ -> false

    /// <summary>
    /// determines if the set is a proper subset of the given sequence
    /// </summary>
    member x.IsProperSubsetOf (other : seq<'a>) =
        match compareSeq other with
            | (_, 0, o) -> o > 0
            | _ -> false

    /// <summary>
    /// determines if the set is a proper superset of the given sequence
    /// </summary>
    member x.IsProperSupersetOf (other : seq<'a>) =
        match compareSeq other with
            | (_, m, 0) -> m > 0
            | _ -> false

    /// <summary>
    /// determines if the set and the given sequence overlap
    /// </summary>
    member x.Overlaps (other : seq<'a>) =
        let (b,_,_) = compareSeq other
        b > 0

    /// <summary>
    /// determines if the set is equal (set) to the given sequence
    /// </summary>
    member x.SetEquals (other : seq<'a>) =
        match compareSeq other with
            | (_,0,0) -> true
            | _ -> false


    new() = ReferenceCountingSet Seq.empty


    interface ICollection<'a> with
        member x.Add v = x.Add v |> ignore
        member x.Remove v = x.Remove v
        member x.Clear() = x.Clear()
        member x.Count = x.Count
        member x.CopyTo(arr, index) = 
            let mutable i = index

            if nullCount > 0 then
                arr.[i] <- Unchecked.defaultof<_>
                i <- i + 1

            for e in x do
                arr.[i] <- e
                i <- i + 1

        member x.IsReadOnly = false
        member x.Contains v = store.ContainsKey v

    interface ISet<'a> with
        member x.Add item = x.Add item
        member x.ExceptWith other = x.ExceptWith other
        member x.IntersectWith other = x.IntersectWith other
        member x.UnionWith other = x.UnionWith other
        member x.SymmetricExceptWith other = x.SymmetricExceptWith other

        member x.IsProperSubsetOf other = x.IsProperSubsetOf other
        member x.IsProperSupersetOf other = x.IsProperSupersetOf other
        member x.IsSubsetOf other = x.IsSubsetOf other
        member x.IsSupersetOf other = x.IsSupersetOf other
        member x.Overlaps other = x.Overlaps other
        member x.SetEquals other = x.SetEquals other


    interface IEnumerable with
        member x.GetEnumerator() =
            new ReferenceCountingSetEnumerator<'a>(nullCount > 0, store) :> IEnumerator

    interface IEnumerable<'a> with
        member x.GetEnumerator() =
            new ReferenceCountingSetEnumerator<'a>(nullCount > 0, store) :> IEnumerator<'a>

// define an Enumerator enumerating all (distinct) elements in the set
and private ReferenceCountingSetEnumerator<'a>(containsNull : bool, store : Dictionary<obj, 'a * ref<int>>) =
    let mutable emitNull = containsNull
    let mutable e = store.GetEnumerator() :> IEnumerator<KeyValuePair<obj, 'a * ref<int>>>

    interface IEnumerator with
        member x.MoveNext() = 
            if emitNull then
                emitNull <- false
                true
            else 
                e.MoveNext()

        member x.Reset() = 
            emitNull <- containsNull
            e.Reset()

        member x.Current = 
            if emitNull then
                Unchecked.defaultof<_>
            else
                e.Current.Key

    interface IEnumerator<'a> with
        member x.Current = e.Current.Value |> fst
        member x.Dispose() = e.Dispose()
