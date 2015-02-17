namespace Aardvark.Base.Incremental

open System.Collections
open System.Collections.Generic
open System.Collections.Concurrent


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

    member x.Add (v : 'a) = add v

    member x.Remove(v : 'a) = remove v

    member x.Contains (v : 'a) =
        store.ContainsKey v

    member x.Clear() =
        store.Clear()

    member x.Count = store.Count

    member x.SetEquals (other : seq<'a>) =
        let count = ref 0
        let res = other |> Seq.exists(fun a -> count := !count + 1; not <| store.ContainsKey a)

        if res then false
        else !count = store.Count


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

and private ReferenceCountingSetEnumerator<'a>(store : Dictionary<obj, 'a * ref<int>>) =
    let mutable e = store.GetEnumerator() :> IEnumerator<KeyValuePair<obj, 'a * ref<int>>>

    interface IEnumerator with
        member x.MoveNext() = e.MoveNext()
        member x.Reset() = e.Reset()
        member x.Current = e.Current.Key

    interface IEnumerator<'a> with
        member x.Current = e.Current.Value |> fst
        member x.Dispose() = e.Dispose()
