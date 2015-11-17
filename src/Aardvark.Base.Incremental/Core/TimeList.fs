namespace Aardvark.Base.Incremental

open Aardvark.Base
open System
open System.Collections
open System.Collections.Generic
open System.Runtime.InteropServices


/// <summary>
/// represents a sequence containing values annotated with
/// a Time (order maintenance). This list can be used to 
/// efficiently insert/remove timed values.
/// </summary>
type TimeList<'a>() =
    static let emptyOrder = SimpleOrder.create() :> IOrder
    let cache = Dictionary<ISortKey, 'a>()
    let mutable rep = emptyOrder

    member x.Count = cache.Count

    member x.Clear() =
        cache.Clear()

    member x.Add(t : ISortKey, v : 'a) =
        rep <- t.Clock
        cache.[t] <- v
         
    member x.Item
        with get(t) = 
            cache.[t]
        and set(t : ISortKey) (value : 'a) =
            rep <- t.Clock
            cache.[t] <- value

    member x.TryGetValue(t : ISortKey, [<Out>] value : byref<'a>) =
        cache.TryGetValue(t, &value)

    member x.Remove(t : ISortKey) =
        cache.Remove t

    member x.Contains (t : ISortKey, v : 'a) =
        match cache.TryGetValue t with
            | (true, v') -> Object.Equals(v,v')
            | _ -> false
//
//    member x.Values =
//        x.All |> Seq.sortBy fst
//              |> Seq.map snd

    member x.UnorderedValues =
        x.All |> Seq.map snd

    member x.All = cache |> Seq.map (fun (KeyValue(k,v)) -> k,v)

//    interface ICollection<ISortKey * 'a> with
//        member x.Add item = x.Add item |> ignore
//        member x.Contains item = x.Contains item
//        
//        member x.CopyTo(array , arrayIndex)  = 
//            let mutable index = arrayIndex
//            for e in cache do
//                array.[index] <- (e.Key, e.Value)
//                index <- index + 1
//        
//        member x.Count = cache.Count
//        member x.IsReadOnly = false
//        member x.Remove((t,_)) = x.Remove t
//        member x.Clear() = x.Clear()
//
//    interface IEnumerable with
//        member x.GetEnumerator() = new TimeListEnumerator<'a>(rep, cache) :> IEnumerator
//
//    interface IEnumerable<ISortKey * 'a> with
//        member x.GetEnumerator() = new TimeListEnumerator<'a>(rep, cache) :> IEnumerator<ISortKey * 'a>
//
//and private TimeListEnumerator<'a>(t : IOrder, cache : Dictionary<ISortKey, 'a>) =
//    let mutable current = t.Root
//    let mutable currentValue = Unchecked.defaultof<ISortKey * 'a>
//
//    let rec moveNext() =
//        if current.Next <> t.Root then
//            current <- current.Next
//            match cache.TryGetValue current with
//                | (true, v) ->
//                    currentValue <- current, v
//                    true
//                | _ ->
//                    moveNext()
//        else
//            false
//
//    interface IEnumerator with
//        member x.MoveNext() = moveNext()
//        member x.Reset() = current <- t.Root
//        member x.Current = currentValue :> obj
//
//    interface IEnumerator<ISortKey * 'a> with
//        member x.Current = currentValue
//        member x.Dispose() =
//            current <- Unchecked.defaultof<_>
//            currentValue <- Unchecked.defaultof<ISortKey * 'a>
//
