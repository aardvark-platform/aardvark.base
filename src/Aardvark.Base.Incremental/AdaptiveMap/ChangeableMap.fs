namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental.AMapReaders

[<CompiledName("ChangeableMap")>]
type cmap<'k, 'v when 'k : equality>(initial : seq<'k * 'v>) =

    let content = VersionedDictionary(Dictionary.ofSeq initial)
    let contentCollection = content :> ICollection<_>
    let readers = WeakSet<BufferedReader<'k, 'v>>()

    interface amap<'k, 'v> with
        member x.GetReader() =
            let r = new BufferedReader<'k, 'v>(fun r -> readers.Remove r |> ignore)
            r.Emit(content, None)
            readers.Add r |> ignore
            r :> _

    member x.Count = content.Count

    member x.ContainsKey v = content.ContainsKey v

    member x.Add(k,v) =
        content.Add(k,v)
        for r in readers do 
            r.Emit(content, Some [Set(k,v)])

    member x.Remove(k) =
        match content.TryGetValue k with
            | (true, v) ->
                content.Remove k |> ignore
                for r in readers do 
                    r.Emit(content, Some [Remove(k,v)])
                true
            | _ -> 
                false

    member x.Item
        with get k = content.[k]
        and set k v =
            content.[k] <- v
            for r in readers do 
                r.Emit(content, Some [Set(k,v)])

    member x.Clear() =
        content.Clear()
        for r in readers do 
            r.Emit(content, None)

    member x.Keys = content.Keys :> ICollection<_>
    member x.Values = content.Values :> ICollection<_>

    member x.TryGetValue(k : 'k, v : byref<'v>) =
        content.TryGetValue(k, &v)

    interface ICollection<KeyValuePair<'k, 'v>> with
        member x.IsReadOnly = false
        member x.Count = x.Count
        member x.Add(KeyValue(k,v)) = x.[k] <- v
        member x.Clear() = x.Clear()
        member x.Contains(kvp) = contentCollection.Contains kvp
        member x.CopyTo(arr, index) = contentCollection.CopyTo(arr, index)
        member x.Remove(KeyValue(k,v)) = x.Remove k
        member x.GetEnumerator() = contentCollection.GetEnumerator()
        member x.GetEnumerator() = contentCollection.GetEnumerator() :> IEnumerator

    interface IDictionary<'k, 'v> with
        member x.Add(k,v) = x.Add(k,v)
        member x.Remove(k) = x.Remove(k)
        member x.Item
            with get k = x.[k]
            and set k v = x.[k] <- v

        member x.TryGetValue(k : 'k, v : byref<'v>) =
            content.TryGetValue(k, &v)

        member x.ContainsKey(k) = content.ContainsKey k
        member x.Keys = content.Keys :> ICollection<_>
        member x.Values = content.Values :> ICollection<_>

module CMap =
    let empty<'k, 'v when 'k : equality> : cmap<'k, 'v> = cmap []

    let ofSeq (s : seq<'k * 'v>) = cmap<'k, 'v> s
    let ofList (s : list<'k * 'v>) = cmap<'k, 'v> s
    let ofArray (s : array<'k * 'v>) = cmap<'k, 'v> s

    let add (key : 'k) (value : 'v) (map : cmap<'k, 'v>) = map.Add(key, value)
    let set (key : 'k) (value : 'v) (map : cmap<'k, 'v>) = map.[key] <- value

    let remove (key : 'k) (map : cmap<'k, 'v>) = map.Remove key

    let clear (map : cmap<'k, 'v>) = map.Clear()

    let containsKey (key : 'k) (map : cmap<'k, 'v>) = map.ContainsKey key

    let count (map : cmap<'k, 'v>) = map.Count

    let toSeq (map : cmap<'k, 'v>) = map |> Seq.map (fun (KeyValue(k,v)) -> k,v)

    let toList (map : cmap<'k, 'v>) = map |> toSeq |> Seq.toList

    let toArray (map : cmap<'k, 'v>) = map |> toSeq |> Seq.toArray

    let unionWith (other : IDictionary<'k, 'v>) (map : cmap<'k, 'v>) =
        for (KeyValue(k,v)) in other do
            map.[k] <- v

    let applyDeltas (deltas : list<MapDelta<'k, 'v>>) (xs : cmap<'k, 'v>) =
        for d in deltas do
            match d with 
              | Set(k,v) -> xs.[k] <- v
              | Remove(k,v) -> xs.Remove k |> ignore
