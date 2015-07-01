namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental.AListReaders

[<CompiledName("ChangeableListKey"); AllowNullLiteral>]
type clistkey internal (t : Time) =
    member internal x.Time = t

[<CompiledName("ChangeableList")>]
type clist<'a>(initial : seq<'a>) =
    let content = TimeList<'a>()
    let rootTime = Time.newRoot()
    let readers = WeakSet<BufferedReader<'a>>()

    let insertAfter (t : Time) (value : 'a) =
        let newTime = Time.after t
        content.Add(newTime, value)

        for r in readers do 
            if r.IsIncremental then r.Emit [Add (newTime, value)]
            else r.Reset content

        newTime

    do  
        let mutable current = rootTime
        for e in initial do
            current <- insertAfter current e

    interface alist<'a> with
        member x.GetReader() =
            let r = new BufferedReader<'a>(rootTime, fun r -> readers.Remove r |> ignore)
            r.Emit (content |> Seq.map Add |> Seq.toList)
            readers.Add r |> ignore
            r :> _

    member x.Count = content.Count

    member x.Remove(key : clistkey) =
        let c = content.[key.Time]
        content.Remove key.Time |> ignore

        Time.delete key.Time
        for r in readers do 
            if r.IsIncremental then r.Emit [Rem (key.Time, c)]
            else r.Reset content

    member x.InsertAfter(key : clistkey, value : 'a) =
        clistkey (insertAfter key.Time value)

    member x.InsertBefore(key : clistkey, value : 'a) =
        x.InsertAfter(clistkey key.Time.prev, value)

    member x.Add(value : 'a) =
        x.InsertAfter(clistkey rootTime.prev, value)

    member x.Clear() =
        let deltas = content |> Seq.map Rem |> Seq.toList
        content.Clear()
        rootTime.next <- rootTime
        rootTime.prev <- rootTime
        
        for r in readers do 
            if r.IsIncremental then r.Emit deltas
            else r.Reset content

    member x.Find(item : 'a) =
        let t = content |> Seq.tryPick (fun (t,v) -> if Object.Equals(v,item) then Some t else None)
        match t with
            | Some t -> clistkey t
            | None -> null

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() =
            (content.Values :> System.Collections.IEnumerable).GetEnumerator()

    interface System.Collections.Generic.IEnumerable<'a> with
        member x.GetEnumerator() =
            (content.Values :> seq<'a>).GetEnumerator()

[<CompiledName("ChangeableOrderedSet")>]
type corderedset<'a>(initial : seq<'a>) =
    let content = TimeList<'a>()
    let rootTime = Time.newRoot()
    let listReaders = WeakSet<BufferedReader<'a>>()
    let setReaders = WeakSet<ASetReaders.BufferedReader<'a>>()
    let set = HashSet<'a>()
    let times = Dict<'a, Time>()

    let tryGetTime (value : 'a) =
        match times.TryGetValue value with
            | (true, t) -> Some t
            | _ -> None

    let getTime(value : 'a) =
        times.[value]

    let tryRemoveTime (value : 'a) =
        match times.TryGetValue value with
            | (true, t) -> 
                times.Remove value |> ignore
                Some t
            | _ -> None

    let setTime (value : 'a) (t : Time) =
        match times.TryGetValue value with
            | (true, t) -> failwith "duplicate time"
            | _ -> times.[value] <- t


    let insertAfter (t : Time) (value : 'a) =
        let newTime = Time.after t
        content.Add(newTime, value)
        set.Add value |> ignore

        for r in listReaders do 
            if r.IsIncremental then r.Emit [Add (newTime, value)]
            else r.Reset content

        for r in setReaders do 
            r.Emit(set, Some [Add value])

        setTime value newTime
        newTime

    let remove (value : 'a) =
        match tryRemoveTime value with
            | Some t ->
                content.Remove t |> ignore
                set.Remove value |> ignore

                Time.delete t
                for r in listReaders do 
                    if r.IsIncremental then r.Emit [Rem (t, value)]
                    else r.Reset content

                for r in setReaders do 
                    r.Emit(set, Some [Rem value])

                true

            | None ->
                false

    let clear() =
        let deltas = content |> Seq.map Rem |> Seq.toList
        content.Clear()
        rootTime.next <- rootTime
        rootTime.prev <- rootTime
        
        for r in listReaders do 
            if r.IsIncremental then r.Emit deltas
            else r.Reset content

        set.Clear()
        for r in setReaders do 
            r.Emit(set, None)

    do  
        let mutable current = rootTime
        for e in initial do
            current <- insertAfter current e

    interface alist<'a> with
        member x.GetReader() =
            let r = new BufferedReader<'a>(rootTime, fun r -> listReaders.Remove r |> ignore)
            r.Emit (content |> Seq.map Add |> Seq.toList)
            listReaders.Add r |> ignore
            r :> _

    interface aset<'a> with
        member x.GetReader() =
            let r = new ASetReaders.BufferedReader<'a>(fun r -> setReaders.Remove r |> ignore)
            r.Emit(set, None)
            setReaders.Add r |> ignore
            r :> _

    member x.Count = content.Count

    member x.Remove(item : 'a) =
        if set.Contains item then
            remove item
        else
            false

    member x.InsertAfter(prev : 'a, value : 'a) =
        if set.Contains value then
            false
        else
            match tryGetTime prev with
                | Some t -> 
                    insertAfter t value |> ignore
                    true
                | None ->
                    failwithf "set does not contain element: %A" prev

    member x.InsertBefore(next : 'a, value : 'a) =
        if set.Contains value then
            false
        else
            match tryGetTime next with
                | Some t -> 
                    insertAfter t.prev value |> ignore
                    true
                | None ->
                    failwithf "set does not contain element: %A" next

    member x.Add(value : 'a) =
        if set.Contains value then
            false
        else
            match tryGetTime value with
                | Some t -> false
                | None -> 
                    insertAfter rootTime.prev value |> ignore
                    true

    member x.Clear() =
        clear()

    member x.Contains item =
        set.Contains item

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() =
            (content.Values :> System.Collections.IEnumerable).GetEnumerator()

    interface System.Collections.Generic.IEnumerable<'a> with
        member x.GetEnumerator() =
            (content.Values :> seq<'a>).GetEnumerator()

[<CompiledName("ChangeableIndexedList")>]
type cilist<'a>(initial : seq<'a>) =
    let clist = clist (initial)

    let content = AVL.custom (fun (a,_) (b,_) -> compare a b)

    member x.InsertAt(index : int, value : 'a) =
        ()

    member x.RemoveAt(index : int) =
        ()

    member x.Item
        with get (i : int) : 'a = failwith ""
        and set (i : int) (value : 'a) = ()

