namespace Aardvark.Base.Incremental

open Aardvark.Base
open System
open System.Collections
open System.Collections.Generic

/// <summary>
/// represents a sequence containing values annotated with
/// a Time (order maintenance). This list can be used to 
/// efficiently insert/remove timed values.
/// </summary>
type TimeList<'a>() =
    let cache = Dictionary<Time, 'a>()
    let mutable rep = Time.newRoot()

    member x.Clear() =
        cache.Clear()

    member x.Add(t : Time, v : 'a) =
        rep <- t.rep
        cache.[t] <- v
                    
    member x.Remove(t : Time) =
        cache.Remove t

    member x.Contains (t : Time, v : 'a) =
        match cache.TryGetValue t with
            | (true, v') -> Object.ReferenceEquals(v,v')
            | _ -> false

    interface ICollection<Time * 'a> with
        member x.Add item = x.Add item |> ignore
        member x.Contains item = x.Contains item
        
        member x.CopyTo(array , arrayIndex)  = 
            let mutable index = arrayIndex
            for e in cache do
                array.[index] <- (e.Key, e.Value)
                index <- index + 1
        
        member x.Count = cache.Count
        member x.IsReadOnly = false
        member x.Remove((t,_)) = x.Remove t
        member x.Clear() = x.Clear()


    interface IEnumerable with
        member x.GetEnumerator() = new TimeListEnumerator<'a>(rep, cache) :> IEnumerator

    interface IEnumerable<Time * 'a> with
        member x.GetEnumerator() = new TimeListEnumerator<'a>(rep, cache) :> IEnumerator<Time * 'a>

and private TimeListEnumerator<'a>(t : Time, cache : Dictionary<Time, 'a>) =
    let mutable current = t.rep
    let mutable currentValue = Unchecked.defaultof<Time * 'a>

    let rec moveNext() =
        match cache.TryGetValue current with
            | (true, v) ->
                currentValue <- current, v
                true
            | _ ->
                if current.next <> current.rep then
                    current <- current.next
                    moveNext()
                else
                    false

    interface IEnumerator with
        member x.MoveNext() = moveNext()
        member x.Reset() = current <- current.rep
        member x.Current = currentValue :> obj

    interface IEnumerator<Time * 'a> with
        member x.Current = currentValue
        member x.Dispose() =
            current <- Unchecked.defaultof<_>
            currentValue <- Unchecked.defaultof<Time * 'a>

