namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.InteropServices
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

    let tryAt (index : int) =
        if index < 0 || index >= content.Count then None
        else rootTime.TryAt (index + 1)

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

    member x.TryGetKey (index : int, [<Out>] key : byref<clistkey>) =
        match tryAt index with
            | Some t ->
                key <- clistkey t
                true
            | None ->
                false

    member x.Item
        with get (k : clistkey) =
            let t = k.Time
            content.[t]
        and set (k : clistkey) (value : 'a) =
            let tOld = k.Time
            let vOld = content.[tOld]
            let tNew = Time.after tOld

            content.Remove tOld |> ignore
            content.Add(tNew, value)
            Time.delete tOld

            for r in readers do 
                if r.IsIncremental then r.Emit [Rem (tOld, vOld); Add (tNew, value)]
                else r.Reset content

    member x.Item
        with get (index : int) =
            match tryAt index with
                | Some t -> x.[clistkey t]
                | None -> raise <| IndexOutOfRangeException()
        and set (index : int) (value : 'a) =
            match tryAt index with
                | Some t -> x.[clistkey t] <- value
                | None -> raise <| IndexOutOfRangeException()

    member x.Insert(index : int, value : 'a) =
        match tryAt index with
            | Some t ->
                x.InsertBefore(clistkey t, value)
            | None ->
                raise <| IndexOutOfRangeException()

    member x.InsertRange(index : int, s : seq<'a>) =
        match x.TryGetKey index with
            | (true, k) ->
                for e in s do
                    x.InsertBefore(k, e) |> ignore

            | _ ->
                raise <| IndexOutOfRangeException()

    member x.RemoveAt(index : int) =
        match tryAt index with
            | Some t ->
                x.Remove(clistkey t) |> ignore
            | None ->
                raise <| IndexOutOfRangeException()

    member x.RemoveRange (start : int, count : int) =
        if start + count <= x.Count then
            match x.TryGetKey start with
                | (true, k) ->
                    let mutable t = k.Time
                    for i in 0..count-1 do
                        let n = t.Next
                        x.Remove (clistkey t)
                        t <- n
                | _ ->
                    raise <| IndexOutOfRangeException()
        else
            raise <| IndexOutOfRangeException()

    member x.Count = content.Count

    member x.Remove(key : clistkey) =
        let c = content.[key.Time]
        content.Remove key.Time |> ignore

        Time.delete key.Time
        for r in readers do 
            if r.IsIncremental then r.Emit [Rem (key.Time, c)]
            else r.Reset content

    member x.InsertAfter(key : clistkey, value : 'a) : clistkey =
        clistkey (insertAfter key.Time value)

    member x.InsertBefore(key : clistkey, value : 'a) : clistkey =
        x.InsertAfter(clistkey key.Time.Prev, value)

    member x.Add(value : 'a) =
        x.InsertAfter(clistkey rootTime.Prev, value)

    member x.AddRange(s : seq<'a>) =
        s |> Seq.iter (fun e -> x.Add e |> ignore)

    member x.Clear() =
        let deltas = content |> Seq.map Rem |> Seq.toList
        content.Clear()

        Time.deleteAll rootTime
        
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

    new() = clist Seq.empty

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
        Time.deleteAll rootTime
        
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
                    insertAfter t.Prev value |> ignore
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
                    insertAfter rootTime.Prev value |> ignore
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

    new() = corderedset Seq.empty

module CList =
    
    let empty<'a> : clist<'a> = clist()
    
    let count (l : clist<'a>) = l.Count

    let add (v : 'a) (l : clist<'a>) =
        l.Add v |> ignore
      
    let addRange (s : seq<'a>) (l : clist<'a>) =
        l.AddRange(s)
        
    let insert (index : int) (v : 'a) (l : clist<'a>) =
        l.Insert(index, v) |> ignore
        
    let insertRange (index : int) (s : seq<'a>) (l : clist<'a>) =
        l.InsertRange(index, s)

    let remove (index : int) (l : clist<'a>) =
        l.RemoveAt(index)

    let removeRange (start : int) (count : int) (l : clist<'a>) =
        l.RemoveRange(start, count)

    let clear (l : clist<'a>) = l.Clear()

    let tryGet (index : int) (l : clist<'a>) =
        if index >= 0 && index < l.Count then Some l.[index]
        else None

    let get (index : int) (l : clist<'a>) = l.[index]

    let set (index : int) (value : 'a) (l : clist<'a>) = l.[index] <- value

    let ofSeq (s : seq<'a>) = clist s

    let ofList (l : list<'a>) = clist l

    let ofArray (l : 'a[]) = clist l

    let toSeq (c : clist<'a>) = c :> seq<_>

    let toList (c : clist<'a>) = c |> Seq.toList

    let toArray (c : clist<'a>) = c |> Seq.toArray

module CListRel =
    
    let empty<'a> : clist<'a> = clist()
    
    let count (l : clist<'a>) = l.Count

    let add (v : 'a) (l : clist<'a>) =
        l.Add v

    let insertAfter (key : clistkey) (v : 'a) (l : clist<'a>) =
        l.InsertAfter(key, v)
   
    let insertBefore (key : clistkey) (v : 'a) (l : clist<'a>) =
        l.InsertBefore(key, v)
        
    let remove (key : clistkey) (l : clist<'a>) =
        l.Remove(key)

    let removeRange (start : int) (keys : seq<clistkey>)  (l : clist<'a>) =
        keys |> Seq.iter (flip remove l)

    let clear (l : clist<'a>) = l.Clear()

    let get (key : clistkey) (l : clist<'a>) = l.[key]

    let set (key : clistkey) (value : 'a) (l : clist<'a>) = l.[key] <- value

    let ofSeq (s : seq<'a>) = clist s

    let ofList (l : list<'a>) = clist l

    let ofArray (l : 'a[]) = clist l

    let toSeq (c : clist<'a>) = c :> seq<_>

    let toList (c : clist<'a>) = c |> Seq.toList

    let toArray (c : clist<'a>) = c |> Seq.toArray

module CSetOrdered =
    
    let count (s : corderedset<'a>) = s.Count

    let add (v : 'a) (s : corderedset<'a>) = s.Add v

    let remove (v : 'a) (s : corderedset<'a>) = s.Remove v

    let insertAfter (anchor : 'a) (v : 'a) (s : corderedset<'a>) =
        s.InsertAfter(anchor, v)

    let insertBefore (anchor : 'a) (v : 'a) (s : corderedset<'a>) =
        s.InsertBefore(anchor, v)
//
//    let insertFirst (v : 'a) (s : corderedset<'a>) =
//        s.InsertBefore(s.[0], v)