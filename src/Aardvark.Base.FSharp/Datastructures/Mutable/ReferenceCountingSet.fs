namespace Aardvark.Base

open System.Collections
open System.Collections.Generic
open System.Threading
open FSharp.Data.Adaptive

/// <summary>
/// represents a set of elements having a reference count.
/// this means that an element is contained when it has been
/// added at least once more than removed.
/// </summary>
type ReferenceCountingSet<'a>(initial : seq<'a>) =
    let mutable nullCount = 0
    let mutable version = 0
    let mutable store = Dictionary<obj, struct('a * ref<int>)>(1)

    let hasChanged() =
        Interlocked.Increment &version |> ignore

    let toCollection (s : seq<'a>) =
        match s with
            | :? ICollection<'a> as s -> s
            | _ -> System.Collections.Generic.HashSet s :> ICollection<_>

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
        if isNull (v :> obj) then
            nullCount <- nullCount + 1
            nullCount = 1
        else
            match store.TryGetValue v with
            | (true, (_,r)) ->
                r := !r + 1
                false
            | _ ->
                let r = struct(v, ref 1)
                store.[v] <- r
                hasChanged()
                true

    let remove (v : 'a) =
        if isNull (v :> obj) then
            nullCount <- nullCount - 1
            nullCount = 0
        else
            match store.TryGetValue v with
            | (true, (_,r)) ->
                r := !r - 1
                if !r = 0 then
                    hasChanged()
                    store.Remove v
                else
                    false
            | _ ->
                false

    do for e in initial do
        add e |> ignore

    member private x.Version = version
    member private x.Store = store
    member private x.NullCount = nullCount


    member internal x.SetTo(other : ReferenceCountingSet<'a>) =
        nullCount <- other.NullCount
        version <- other.Version

        store <- Dictionary(other.Store.Count)
        for (KeyValue(k,(v,r))) in other.Store do
            store.[k] <- (v, ref !r)

    member internal x.Apply(deltas : list<SetOperation<'a>>) =
        match deltas with
        | [] -> []
        | [v] ->
            match v with
                | Add(_,v) ->
                    if x.Add v then [Add v]
                    else []
                | Rem(_,v) ->
                    if x.Remove v then [Rem v]
                    else []

        | _ ->
            let mutable originalNullRefs = nullCount
            let touched = Dictionary<obj, 'a * bool * ref<int>>()
            for d in deltas do
                match d with
                | Add(_,v) ->
                    let o = v :> obj
                    if isNull o then
                        nullCount <- nullCount + 1
                    else
                        match store.TryGetValue o with
                        | (true, (_,r)) ->
                            r := !r + 1
                        | _ ->
                            let r = ref 1
                            touched.[o] <- (v, false, r)
                            store.[o] <- (v, r)
                | Rem(_, v) ->
                    let o = v :> obj
                    if isNull o then
                        nullCount <- nullCount - 1
                    else
                        match store.TryGetValue o with
                        | (true, (_,r)) ->
                            r := !r - 1
                            if !r = 0 && not (touched.ContainsKey o) then
                                touched.[o] <- (v, true, r)
                        | _ ->
                            let r = ref -1
                            touched.[o] <- (v, false, r)
                            store.[o] <- (v, r)

                    ()

            let valueDeltas =
                touched.Values
                    |> Seq.choose (fun (value, wasContained, refCount) ->
                        let r = !refCount
                        if r > 0 then
                            if wasContained then None
                            else Some (Add value)
                        else
                            store.Remove value |> ignore
                            if wasContained then Some (Rem value)
                            else None
                        )
                    |> Seq.toList

            let result =
                if nullCount = 0 && originalNullRefs > 0 then
                    (Rem Unchecked.defaultof<_>)::valueDeltas
                elif nullCount > 0 && originalNullRefs = 0 then
                    (Add Unchecked.defaultof<_>)::valueDeltas
                else
                    valueDeltas

            if not (List.isEmpty result) then
                hasChanged()

            result

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
        if isNull (v :> obj) then
            nullCount > 0
        else
            store.ContainsKey v

    /// <summary>
    /// clears the entire set
    /// </summary>
    member x.Clear() =
        if x.Count <> 0 then
            nullCount <- 0
            store.Clear()
            hasChanged()

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
        if isNull (v :> obj) then nullCount
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
        for (v,_) in store.Values do
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

    member x.GetEnumerator() = new ReferenceCountingSetEnumerator<'a>(nullCount > 0, store)

    interface IEnumerable with
        member x.GetEnumerator() =
            new ReferenceCountingSetEnumerator<'a>(nullCount > 0, store) :> IEnumerator

    interface IEnumerable<'a> with
        member x.GetEnumerator() =
            new ReferenceCountingSetEnumerator<'a>(nullCount > 0, store) :> IEnumerator<'a>

// define an Enumerator enumerating all (distinct) elements in the set
and ReferenceCountingSetEnumerator<'a> =
    struct 
        val mutable private containsNull : bool
        val mutable private emitNull : bool
        val mutable private currentIsNull : bool
        val mutable private e : Dictionary<obj, struct('a * ref<int>)>.Enumerator

        member x.Current =
            if x.currentIsNull then
                Unchecked.defaultof<_>
            else
                x.e.Current.Value |> fstv

        member x.MoveNext() =
            if x.emitNull then
                x.emitNull <- false
                x.currentIsNull <- true
                true
            else
                x.currentIsNull <- false
                x.e.MoveNext()

        interface IEnumerator with
            member x.MoveNext() = 
                x.MoveNext()

            member x.Reset() =
                x.emitNull <- x.containsNull
                (x.e :> IEnumerator).Reset()

            member x.Current = x.Current :> obj

        interface IEnumerator<'a> with
            member x.Current = x.Current
            member x.Dispose() = x.e.Dispose()

        internal new(containsNull : bool, store : Dictionary<obj, struct('a * ref<int>)>) =
            {
                containsNull = containsNull
                emitNull = containsNull
                currentIsNull = false
                e = store.GetEnumerator()
            }

    end
