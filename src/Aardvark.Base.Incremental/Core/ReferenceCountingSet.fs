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
    let store = Dictionary<obj, 'a * ref<int>>()

    let add (v : 'a) =
        match store.TryGetValue v with
            | (true, (_,r)) -> 
                r := !r + 1
                false
            | _ ->
                let r = v, ref 1
                store.[v] <- r
                true

    let remove (v : 'a) =
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
        store.ContainsKey v

    /// <summary>
    /// clears the entire set
    /// </summary>
    member x.Clear() =
        store.Clear()

    /// <summary>
    /// returns the number of (distinct) elements contained in
    /// the set.
    /// </summary>
    member x.Count = store.Count

    /// <summary>
    /// determines if the set is equal (set) to the given sequence
    /// </summary>
    member x.SetEquals (other : seq<'a>) =
        let count = ref 0
        let res = other |> Seq.exists(fun a -> count := !count + 1; not <| store.ContainsKey a)

        if res then false
        else !count = store.Count

    /// <summary>
    /// gets the current reference count for the given element
    /// </summary>
    member x.GetReferenceCount(v) =
        match store.TryGetValue (v :> obj) with
            | (true, (_,c)) -> !c
            | _ -> 0

    new() = ReferenceCountingSet Seq.empty


    interface ICollection<'a> with
        member x.Add v = x.Add v |> ignore
        member x.Remove v = x.Remove v
        member x.Clear() = x.Clear()
        member x.Count = x.Count
        member x.CopyTo(arr, index) = 
            let mutable i = index
            for e in x do
                arr.[i] <- e
                i <- i + 1

        member x.IsReadOnly = false
        member x.Contains v = store.ContainsKey v

    interface IEnumerable with
        member x.GetEnumerator() =
            new ReferenceCountingSetEnumerator<'a>(store) :> IEnumerator

    interface IEnumerable<'a> with
        member x.GetEnumerator() =
            new ReferenceCountingSetEnumerator<'a>(store) :> IEnumerator<'a>

// define an Enumerator enumerating all (distinct) elements in the set
and private ReferenceCountingSetEnumerator<'a>(store : Dictionary<obj, 'a * ref<int>>) =
    let mutable e = store.GetEnumerator() :> IEnumerator<KeyValuePair<obj, 'a * ref<int>>>

    interface IEnumerator with
        member x.MoveNext() = e.MoveNext()
        member x.Reset() = e.Reset()
        member x.Current = e.Current.Key

    interface IEnumerator<'a> with
        member x.Current = e.Current.Value |> fst
        member x.Dispose() = e.Dispose()
