namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.InteropServices
open Aardvark.Base
open Aardvark.Base.Incremental.AListReaders

[<CompiledName("ChangeableListKey"); AllowNullLiteral>]
type clistkey internal (t : SkipOrder.SortKey) =
    member x.SortKey = t
    member x.IsDeleted = t.IsDeleted
    
[<CompiledName("ChangeableList")>]
type clist<'a>(initial : seq<'a>) =
    let content = TimeList<'a>()
    let order = SkipOrder.create()
    let readers = WeakSet<EmitReader<'a>>()

    let insertAfter (t : SkipOrder.SortKey) (value : 'a) =
        let newTime = order.After t
        content.Add(newTime, value)

        for r in readers do 
            r.Emit(content, Some [Add (newTime :> ISortKey, value)])

        newTime

    let tryAt (index : int) =
        if index < 0 || index >= content.Count then None
        else order.TryAt (index + 1)

    do  
        let mutable current = order.Root
        for e in initial do
            current <- insertAfter current e

    interface alist<'a> with
        member x.GetReader() =
            lock x (fun () ->
                let r = new EmitReader<'a>(x, order, fun r -> readers.Remove r |> ignore)
                r.Emit (content, None)
                readers.Add r |> ignore
                r :> _
            )

    /// gets a unique key associated with the element at the given index which will
    /// not be changed when inserting/removing other elements. [runtime: O(log N)]
    member x.TryGetKey (index : int, [<Out>] key : byref<clistkey>) = 
        match lock x (fun () -> tryAt index) with
            | Some t ->
                key <- clistkey t
                true
            | None ->
                false

    /// gets or sets the element associated with the given unique key [runtime: O(1)]
    member x.Item
        with get (k : clistkey) =
            lock x (fun () ->
                let t = k.SortKey
                content.[t]
            )
        and set (k : clistkey) (value : 'a) =
            lock x (fun () ->
                let tOld = k.SortKey
                let vOld = content.[tOld]
                let tNew = order.After tOld

                content.Remove tOld |> ignore
                content.Add(tNew, value)
                order.Delete tOld

                for r in readers do 
                    r.Emit(content, Some [Rem (tOld :> ISortKey, vOld); Add (tNew :> ISortKey, value)])
            )

    /// gets or sets the element at the given index [runtime: O(log N)]
    member x.Item
        with get (index : int) =
            lock x (fun () ->
                match tryAt index with
                    | Some t -> x.[clistkey t]
                    | None -> raise <| IndexOutOfRangeException()
            )
        and set (index : int) (value : 'a) =
            lock x (fun () ->
                match tryAt index with
                    | Some t -> x.[clistkey t] <- value
                    | None -> raise <| IndexOutOfRangeException()
            )

    /// inserts the given element at the given index [runtime: O(log N)]
    member x.Insert(index : int, value : 'a) =
        lock x (fun () ->
            match tryAt index with
                | Some t ->
                    x.InsertBefore(clistkey t, value)
                | None ->
                    raise <| IndexOutOfRangeException()
        )

    /// inserts a sequence of elements at the given index [runtime O(m * log N)]
    member x.InsertRange(index : int, s : seq<'a>) =
        lock x (fun () ->
            match tryAt index with
                | Some k ->
                    let mutable current = 
                        if k = order.Root then k
                        else k.Prev

                    for e in s do
                        current <- insertAfter current e

                | _ ->
                    raise <| IndexOutOfRangeException()
        )

    /// removes the element at the given index [runtime: O(log N)]
    member x.RemoveAt(index : int) =
        lock x (fun () ->
            match tryAt index with
                | Some t ->
                    x.Remove(clistkey t) |> ignore
                | None ->
                    raise <| IndexOutOfRangeException()
        )

    /// removes the given range of indices 
    member x.RemoveRange (start : int, count : int) =
        lock x (fun () ->
            if start >= 0 && count >= 0 && start + count <= x.Count then
                match x.TryGetKey start with
                    | (true, k) ->
                        let mutable t = k.SortKey
                        for i in 0..count-1 do
                            let n = t.Next
                            x.Remove (clistkey t)
                            t <- n
                    | _ ->
                        raise <| IndexOutOfRangeException()
            else
                raise <| IndexOutOfRangeException()
        )

    member x.Count = lock x (fun () -> content.Count)

    member x.Remove(key : clistkey) =
        lock x (fun () ->
            let c = content.[key.SortKey]
            content.Remove key.SortKey |> ignore

            order.Delete key.SortKey
            for r in readers do 
                r.Emit(content, Some [Rem (key.SortKey :> ISortKey, c)])
        )

    member x.InsertAfter(key : clistkey, value : 'a) : clistkey =
        lock x (fun () ->
            clistkey (insertAfter key.SortKey value)
        )

    member x.InsertBefore(key : clistkey, value : 'a) : clistkey =
        lock x (fun () ->
            x.InsertAfter(clistkey key.SortKey.Prev, value)
        )

    member x.Add(value : 'a) =
        lock x (fun () ->
            x.InsertAfter(clistkey order.Root.Prev, value)
        )

    member x.AddRange(s : seq<'a>) =
        lock x (fun () ->
            s |> Seq.iter (fun e -> x.Add e |> ignore)
        )

    member x.Clear() =
        lock x (fun () ->
            content.Clear()
            order.Clear()
        
            for r in readers do 
                r.Emit(content, None)
        )

    member x.Find(item : 'a) =
        lock x (fun () ->
            let t = content |> Seq.tryPick (fun (t,v) -> if Object.Equals(v,item) then Some t else None)
            match t with
                | Some t -> clistkey (unbox t)
                | None -> null
        )

    member x.IndexOf (item : 'a) =
        lock x (fun () ->
            match x.Find item with
                | null -> -1
                | k -> order.TryGetIndex k.SortKey
        )

    member x.CopyTo(arr : 'a[], index : int) =
        lock x (fun () ->
            let mutable i = index
            for e in content.Values do
                arr.[i] <- e
                i <- i + 1
        )

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() =
            (content.Values :> System.Collections.IEnumerable).GetEnumerator()

    interface System.Collections.Generic.IEnumerable<'a> with
        member x.GetEnumerator() =
            (content.Values :> seq<'a>).GetEnumerator()

    interface IList<'a> with
        member x.Add v = x.Add v |> ignore
        member x.Item
            with get (i : int) = x.[i]
            and set (i : int) (v : 'a) = x.[i] <- v
        member x.IndexOf v = x.IndexOf v
        member x.Insert(index, item) = x.Insert(index, item) |> ignore
        member x.RemoveAt(index) = x.RemoveAt index
        member x.Clear() = x.Clear()
        member x.Count = x.Count
        member x.IsReadOnly = false
        member x.Contains(item) = lock x (fun () -> content |> Seq.exists(fun (_,i) -> System.Object.Equals(i, item)))
        member x.CopyTo(arr, index) = x.CopyTo(arr, index)
        member x.Remove(item) = lock x (fun () -> match x.Find item with | null -> false | key -> x.Remove key; true)

    new() = clist Seq.empty

[<CompiledName("ChangeableOrderedSet")>]
type corderedset<'a>(initial : seq<'a>) =
    let content = TimeList<'a>()
    let order = SimpleOrder.create()
    let listReaders = WeakSet<EmitReader<'a>>()
    let setReaders = WeakSet<ASetReaders.EmitReader<'a>>()
    let set = HashSet<'a>()
    let times = Dict<'a, SimpleOrder.SortKey>()

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

    let setTime (value : 'a) (t : SimpleOrder.SortKey) =
        match times.TryGetValue value with
            | (true, t) -> failwith "duplicate time"
            | _ -> times.[value] <- t


    let insertAfter (t : SimpleOrder.SortKey) (value : 'a) =
        let newTime = order.After t
        content.Add(newTime, value)
        set.Add value |> ignore

        for r in listReaders do 
            r.Emit(content, Some [Add (newTime :> ISortKey, value)])

        for r in setReaders do 
            r.Emit(set, Some [Add value])

        setTime value newTime
        newTime

    let remove (value : 'a) =
        match tryRemoveTime value with
            | Some t ->
                content.Remove t |> ignore
                set.Remove value |> ignore

                order.Delete t
                for r in listReaders do 
                    r.Emit(content, Some [Rem (t :> ISortKey, value)])

                for r in setReaders do 
                    r.Emit(set, Some [Rem value])

                true

            | None ->
                false

    let clear() =
        let deltas = content |> Seq.map Rem |> Seq.toList
        content.Clear()
        order.Clear()
        
        for r in listReaders do 
            r.Emit(content, None)

        set.Clear()
        for r in setReaders do 
            r.Emit(set, None)


    do  
        let mutable current = order.Root
        for e in initial do
            current <- insertAfter current e

    interface alist<'a> with
        member x.GetReader() =
            lock x (fun () ->
                let r = new EmitReader<'a>(x, order, fun r -> listReaders.Remove r |> ignore)
                r.Emit (content, None)
                listReaders.Add r |> ignore
                r :> _
            )

    interface aset<'a> with
        member x.ReaderCount = lock x (fun () -> setReaders.Count)
        member x.IsConstant = false
        member x.GetReader() =
            lock x (fun () ->
                let r = new ASetReaders.EmitReader<'a>(x, fun r -> setReaders.Remove r |> ignore)
                r.Emit(set, None)
                setReaders.Add r |> ignore
                r :> _
            )

    member x.Count = lock x (fun () -> content.Count)

    member x.Remove(item : 'a) =
        lock x (fun () -> 
            if set.Contains item then
                remove item
            else
                false
        )

    member x.InsertAfter(prev : 'a, value : 'a) =
        lock x (fun () -> 
            if set.Contains value then
                false
            else
                match tryGetTime prev with
                    | Some t -> 
                        insertAfter t value |> ignore
                        true
                    | None ->
                        failwithf "set does not contain element: %A" prev
        )

    member x.InsertBefore(next : 'a, value : 'a) =
        lock x (fun () -> 
            if set.Contains value then
                false
            else
                match tryGetTime next with
                    | Some t -> 
                        insertAfter t.Prev value |> ignore
                        true
                    | None ->
                        failwithf "set does not contain element: %A" next
        )

    member x.Add(value : 'a) =
        lock x (fun () -> 
            if set.Contains value then
                false
            else
                match tryGetTime value with
                    | Some t -> false
                    | None -> 
                        insertAfter order.Root.Prev value |> ignore
                        true
        )

    member x.UnionWith(values : seq<'a>) =
        lock x (fun () ->
            for e in values do
                match tryGetTime e with
                    | Some t -> ()
                    | None -> insertAfter order.Root.Prev e |> ignore
                            
        )

    member x.ExceptWith(values : seq<'a>) =
        lock x (fun () ->
            for e in values do
                remove e |> ignore
                            
        )  

    member x.IntersectWith(values : seq<'a>) : unit =
        failwith "not implemented"

    member x.SymmetricExceptWith(values : seq<'a>) : unit =
        failwith "not implemented"


    member x.CopyTo(arr : 'a[], index : int) =
        lock x (fun () ->
            let mutable i = index
            let mutable t = order.Root.Next

            while t <> order.Root do
                match content.TryGetValue t with
                    | (true, v) -> arr.[i] <- v
                    | _ -> failwith "[COrderedSet] invalid state in CopyTo('a[],int)"

                i <- i + 1
                t <- t.Next
        )

    member x.Clear() =
        lock x (fun () -> clear())

    member x.Contains item =
        lock x (fun () -> set.Contains item)

    member x.IsProperSubsetOf(other: IEnumerable<'a>): bool = lock x (fun () -> set.IsProperSubsetOf other)
    member x.IsProperSupersetOf(other: IEnumerable<'a>): bool = lock x (fun () -> set.IsProperSupersetOf other)
    member x.IsSubsetOf(other: IEnumerable<'a>): bool = lock x (fun () -> set.IsSubsetOf other)
    member x.IsSupersetOf(other: IEnumerable<'a>): bool = lock x (fun () -> set.IsSupersetOf other)
    member x.Overlaps(other: IEnumerable<'a>): bool = lock x (fun () -> set.Overlaps other)
    member x.SetEquals(other: IEnumerable<'a>): bool = lock x (fun () -> set.SetEquals other)

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() =
            (content.Values :> System.Collections.IEnumerable).GetEnumerator()

    interface System.Collections.Generic.IEnumerable<'a> with
        member x.GetEnumerator() =
            (content.Values :> seq<'a>).GetEnumerator()

    new() = corderedset Seq.empty

    interface ISet<'a> with
        member x.Add(item: 'a): bool = x.Add item
        member x.Add(item: 'a): unit = x.Add item |> ignore
        member x.Clear(): unit = x.Clear()
        member x.Contains(item: 'a): bool = x.Contains item
        member x.CopyTo(array: 'a [], arrayIndex: int): unit = x.CopyTo(array, arrayIndex)
        member x.Count: int = x.Count
        member x.ExceptWith(other: IEnumerable<'a>): unit = x.ExceptWith other
        member x.IntersectWith(other: IEnumerable<'a>): unit = x.IntersectWith other
        member x.IsProperSubsetOf(other: IEnumerable<'a>): bool = x.IsProperSubsetOf other
        member x.IsProperSupersetOf(other: IEnumerable<'a>): bool = x.IsProperSupersetOf other
        member x.IsSubsetOf(other: IEnumerable<'a>): bool = x.IsSubsetOf other
        member x.IsSupersetOf(other: IEnumerable<'a>): bool = x.IsSupersetOf other
        member x.Overlaps(other: IEnumerable<'a>): bool = x.Overlaps other
        member x.IsReadOnly: bool = false
        member x.Remove(item: 'a): bool = x.Remove item
        member x.SetEquals(other: IEnumerable<'a>): bool = x.SetEquals other
        member x.SymmetricExceptWith(other: IEnumerable<'a>): unit = x.SymmetricExceptWith other
        member x.UnionWith(other: IEnumerable<'a>): unit = x.UnionWith other



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

module COrderedSet =
    
    let empty<'a> : corderedset<'a> = corderedset []

    let ofSeq (s : seq<'a>) = corderedset s
    let ofList (s : list<'a>) = corderedset s
    let ofArray (s : 'a[]) = corderedset s

    let toSeq (s : corderedset<'a>) = s :> seq<_>
    let toList (s : corderedset<'a>) = lock s (fun () -> s |> Seq.toList)
    let toArray (s : corderedset<'a>) = lock s (fun () -> s |> Seq.toArray)

    let count (s : corderedset<'a>) = s.Count

    let contains (e : 'a) (s : corderedset<'a>) = s.Contains e

    let add (v : 'a) (s : corderedset<'a>) = s.Add v

    let remove (v : 'a) (s : corderedset<'a>) = s.Remove v

    let clear (s : corderedset<'a>) = s.Clear()

    let unionWith (other : seq<'a>) (s : corderedset<'a>) = s.UnionWith other

    let exceptWith (other : seq<'a>) (s : corderedset<'a>) = s.ExceptWith other

    let insertAfter (anchor : 'a) (v : 'a) (s : corderedset<'a>) =
        s.InsertAfter(anchor, v)

    let insertBefore (anchor : 'a) (v : 'a) (s : corderedset<'a>) =
        s.InsertBefore(anchor, v)

    let equals (other : seq<'a>) (s : corderedset<'a>) = s.SetEquals other



//
//    let insertFirst (v : 'a) (s : corderedset<'a>) =
//        s.InsertBefore(s.[0], v)