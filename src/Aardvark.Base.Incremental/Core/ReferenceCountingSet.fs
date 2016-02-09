namespace Aardvark.Base.Incremental

open Aardvark.Base
open System.Collections
open System.Collections.Generic
open System.Collections.Concurrent
open System.Threading

/// <summary>
/// represents a set of elements having a reference count.
/// this means that an element is contained when it has been
/// added at least once more than removed.
/// </summary>
type ReferenceCountingSet<'a>(initial : seq<'a>) =
    let mutable nullCount = 0
    let mutable version = 0
    let mutable store = Dictionary<obj, 'a * ref<int>>(1)


    static let rec checkDeltas (l : list<Delta<'a>>) =
        let set = HashSet<obj>()
        for d in l do
            let value = 
                match d with
                    | Add v -> v
                    | Rem v -> v

            if not (set.Add value) then
                failwithf "found duplicate value %A in delta-list: %A" value l

    let hasChanged() =
        Interlocked.Increment &version |> ignore


    

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
        if isNull (v :> obj) then
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

    member internal x.Apply(deltas : list<Delta<'a>>) =
        match deltas with
            | [] -> []
            | [v] -> 
                match v with
                    | Add v -> 
                        if x.Add v then [Add v]
                        else []
                    | Rem v ->
                        if x.Remove v then [Rem v]
                        else []

            | _ -> 
                let mutable originalNullRefs = nullCount
                let touched = Dictionary<obj, 'a * bool * ref<int>>()
                for d in deltas do
                    match d with
                        | Add v ->
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
                        | Rem v ->
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


                // checkDeltas result

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


    //TODO: implement without modification
    ///// <summary>
    ///// Sets the contents of the set to other's contents returning
    ///// a list of Deltas which need to be performed in order to reproduce
    ///// the transition.
    ///// Runtime: O[(LookUp + AddRem) * (this.Count + other.Count)]
    ///// </summary>
    //member x.SetTo(other : ReferenceCountingSet<'a>) : list<Delta<'a>> =
    //    let result = ref []
    //    let emit n v = 
    //        for i in 1..n do
    //            result := v::!result
    //
    //    // start by adjusting the null count
    //    let otherNull = other.NullCount
    //    if otherNull > nullCount then emit (otherNull - nullCount) (Add Unchecked.defaultof<'a>)
    //    elif otherNull < nullCount then emit (nullCount - otherNull) (Rem Unchecked.defaultof<'a>)
    //
    //
    //
    //    // finally we need to check if this contains objects which are
    //    // not contained in other. 
    //
    //    // this loop performs <this.Count> lookups 
    //    // and (in the worst case) <this.Count> removals
    //    // and therefore has runtime: O [ (Lookup + AddRem) * this.Count ]
    //    for (KeyValue(k,(v, myRef))) in store do
    //        if not <| other.Store.ContainsKey(k) then
    //            emit !myRef (Rem v)
    //            store.Remove k |> ignore
    //
    //
    //    // adjust all reference counts for elements in other. 
    //    // add the elements if they are not contained in this
    //    // and adjust their reference-count otherwise.
    //
    //    // this loop performs <other.Count> lookups
    //    // and (in the worst case) <other.Count> additions
    //    // and therefore has runtime: O [ (Lookup + AddRem) * other.Count ]
    //    for (KeyValue(k,(v,otherRef))) in other.Store do
    //        match store.TryGetValue k with
    //            | (true, (_,myRef)) ->
    //                // if the element was found in both sets
    //                // we need to emit the difference delta
    //                // according to the refCounts
    //                let diff = !otherRef - !myRef
    //                if diff <> 0 then
    //                    if diff > 0 then emit diff (Add v)
    //                    else emit -diff (Rem v)
    //
    //                    // we don't need to check for removals here since
    //                    // otherRef cannot be zero by convention
    //                    myRef := !otherRef
    //
    //            | _ ->
    //                // other contains a value which is not in this
    //                emit !otherRef (Add v)
    //                store.[k] <- (v,ref !otherRef)
    //
    //    // The entire procedure has runtime 
    //    // O [ (LookUp + AddRem) * (this.Count + other.Count) ]
    //
    //    !result

    /// <summary>
    /// Sets the contents of the set to other's contents returning
    /// a list of Deltas which need to be performed in order to reproduce
    /// the transition.
    /// Runtime: O[(LookUp + AddRem) * (this.Count + other.Count)]
    /// </summary>  
//
//    member x.SetTo(other : ISet<'a>) : list<Delta<'a>> =
//        match other with
//            | :? ReferenceCountingSet<'a> as r -> x.SetTo(r)
//            | _ ->
//                let result = ref []
//                let removals = ref []
//                let add v = result := (Add v)::!result
//                let rem v = 
//                    removals := v::!removals
//                    result := (Rem v)::!result
//                
//                for e in x do
//                    if not <| other.Contains e then rem e 
//
//                for o in other do
//                    if x.Add o then add o
//
//                for r in !removals do
//                    x.Remove r |> ignore
//
//                !result

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


    interface IVersionedSet<'a> with
        member x.Version = version

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
    let mutable currentIsNull = Unchecked.defaultof<bool> // does not matter here

    member x.Current =
         if currentIsNull then
            Unchecked.defaultof<_>
         else
            e.Current.Value |> fst

    interface IEnumerator with
        member x.MoveNext() = 
            if emitNull then
                emitNull <- false
                currentIsNull <- true
                true
            else 
                currentIsNull <- false
                e.MoveNext()

        member x.Reset() = 
            emitNull <- containsNull
            e.Reset()

        member x.Current = x.Current :> obj

    interface IEnumerator<'a> with
        member x.Current = x.Current 
        member x.Dispose() = e.Dispose()
