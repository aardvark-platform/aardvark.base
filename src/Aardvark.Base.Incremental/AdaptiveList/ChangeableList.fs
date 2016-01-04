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

    let submitDeltas (a, deltas : Change<ISortKey * 'a>) =
        lock readers (fun () ->
            for r in readers do 
                r.Emit(content, Some deltas)
        )
        a

    let submit () =
        lock readers (fun () ->
            for r in readers do 
                r.Emit(content, None)
        )

    let insertAfter (t : SkipOrder.SortKey) (value : 'a) =
        let newTime = order.After t
        content.Add(newTime, value)

        newTime, [Add (newTime :> ISortKey, value)]

    let insertRange (t : SkipOrder.SortKey) (values : seq<'a>) =
        let mutable current = t

        let deltas = List()
        for e in values do
            let newTime = order.After current
            content.Add(newTime, e)
            current <- newTime
            deltas.Add(Add(newTime :> ISortKey, e))

        (), Seq.toList deltas

    let remove (t : SkipOrder.SortKey) =
        let c = content.[t]
        content.Remove t |> ignore

        order.Delete t
        (), [Rem (t :> ISortKey, c)]

    let set (tOld : SkipOrder.SortKey) (value : 'a) =
        let vOld = content.[tOld]
        let tNew = order.After tOld

        content.Remove tOld |> ignore
        content.Add(tNew, value)
        order.Delete tOld
        (), [Rem (tOld :> ISortKey, vOld); Add (tNew :> ISortKey, value)]

    let removeRange (t : SkipOrder.SortKey) (count : int) =
        let list = List()
        let mutable current = t
        for i in 1..count do
            let next = current.Next
            remove current |> snd |> list.AddRange
            current <- next

        (), Seq.toList list

    let tryAt (index : int) =
        if index < 0 || index >= content.Count then None
        else order.TryAt (index + 1)

    let toSeq() =
        let rec toSeq(start : SkipOrder.SortKey) =
            if start = order.Root then 
                Seq.empty
            else
                seq {
                    match content.TryGetValue start with
                        | (true, v) -> yield v
                        | _ -> ()

                    yield! toSeq start.Next
                }

        toSeq order.Root.Next

    do insertRange order.Root initial |> ignore

    interface alist<'a> with
        member x.GetReader() =
            lock readers (fun () ->
                let r = new EmitReader<'a>(content, order, fun r -> lock readers (fun () -> readers.Remove r |> ignore))
                r.Emit (content, None)
                readers.Add r |> ignore
                r :> _
            )

    /// gets a unique key associated with the element at the given index which will
    /// not be changed when inserting/removing other elements. [runtime: O(log N)]
    member x.TryGetKey (index : int, [<Out>] key : byref<clistkey>) = 
        match lock content (fun () -> tryAt index) with
            | Some t ->
                key <- clistkey t
                true
            | None ->
                false

    /// gets or sets the element associated with the given unique key [runtime: O(1)]
    member x.Item
        with get (k : clistkey) = lock content (fun () -> content.[k.SortKey])
        and set (k : clistkey) (value : 'a) =
            lock content (fun () ->  set k.SortKey value) |> submitDeltas |> ignore
 

    /// gets or sets the element at the given index [runtime: O(log N)]
    member x.Item
        with get (index : int) =
            lock content (fun () ->
                match tryAt index with
                    | Some t -> x.[clistkey t]
                    | None -> raise <| IndexOutOfRangeException()
            )
        and set (index : int) (value : 'a) =
            let res = 
                lock content (fun () ->
                    match tryAt index with
                        | Some t -> set t value
                        | None -> raise <| IndexOutOfRangeException()
                )
            res |> submitDeltas |> ignore

    /// inserts the given element at the given index [runtime: O(log N)]
    member x.Insert(index : int, value : 'a) =
        let res = 
            lock content (fun () ->
                match tryAt index with
                    | Some t ->  insertAfter t.Prev value 
                    | None -> raise <| IndexOutOfRangeException()
            )
        clistkey(res |> submitDeltas)

    /// inserts a sequence of elements at the given index [runtime O(m * log N)]
    member x.InsertRange(index : int, s : seq<'a>) =
        let res = 
            lock content (fun () ->
                match tryAt index with
                    | Some k -> insertRange k.Prev s
                    | _ -> raise <| IndexOutOfRangeException()
            )
        res |> submitDeltas |> ignore

    /// removes the element at the given index [runtime: O(log N)]
    member x.RemoveAt(index : int) =
        let res = 
            lock content (fun () ->
                match tryAt index with
                    | Some t -> remove t
                    | None ->raise <| IndexOutOfRangeException()
            )
        res |> submitDeltas

    /// removes the given range of indices 
    member x.RemoveRange (start : int, count : int) =
        let res = 
            lock content (fun () ->
                if start >= 0 && count >= 0 && start + count <= x.Count then
                    match tryAt start with
                        | Some k -> removeRange k count
                        | _ -> raise <| IndexOutOfRangeException()
                else
                    raise <| IndexOutOfRangeException()
            )
        res |> submitDeltas

    member x.Count = lock content (fun () -> content.Count)

    member x.Remove(key : clistkey) =
        let res = lock content (fun () -> remove key.SortKey)
        res |> submitDeltas

    member x.InsertAfter(key : clistkey, value : 'a) : clistkey =
        let res = lock content (fun () -> insertAfter key.SortKey value)
        clistkey(res |> submitDeltas)

    member x.InsertBefore(key : clistkey, value : 'a) : clistkey =
        let res = lock content (fun () -> insertAfter key.SortKey.Prev value)
        clistkey(res |> submitDeltas)


    member x.Add(value : 'a) =
        let res = lock content (fun () -> insertAfter order.Root.Prev value)
        clistkey(res |> submitDeltas)

    member x.AddRange(s : seq<'a>) =
        let res = lock content (fun () -> insertRange order.Root.Prev s)
        res |> submitDeltas

    member x.Clear() =
        let change = 
            lock content (fun () -> 
                if content.Count > 0 then
                    content.Clear();
                    order.Clear();
                    true
                else
                    false
            )
        if change then
            submit()

    member x.Find(item : 'a) =
        lock content (fun () ->
            let t = content.All |> Seq.tryPick (fun (t,v) -> if Object.Equals(v,item) then Some t else None)
            match t with
                | Some t -> clistkey (unbox t)
                | None -> null
        )

    member x.IndexOf (item : 'a) =
        lock content (fun () ->
            match x.Find item with
                | null -> -1
                | k -> order.TryGetIndex k.SortKey
        )

    member x.CopyTo(arr : 'a[], index : int) =
        lock content (fun () ->
            let mutable i = index
            for v in toSeq() do
                arr.[i] <- v
                i <- i + 1
        )

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() =
            (toSeq() :> System.Collections.IEnumerable).GetEnumerator()

    interface System.Collections.Generic.IEnumerable<'a> with
        member x.GetEnumerator() =
            (toSeq() :> seq<'a>).GetEnumerator()

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
        member x.Contains(item) = lock content (fun () -> content.All |> Seq.exists(fun (_,i) -> System.Object.Equals(i, item)))
        member x.CopyTo(arr, index) = x.CopyTo(arr, index)
        member x.Remove(item) = lock content (fun () -> match x.Find item with | null -> false | key -> x.Remove key; true)

    new() = clist Seq.empty

[<CompiledName("ChangeableOrderedSet")>]
type corderedset<'a>(initial : seq<'a>) =
    let content = TimeList<'a>()
    let order = SimpleOrder.create()
    let listReaders = WeakSet<EmitReader<'a>>()
    let setReaders = WeakSet<ASetReaders.EmitReader<'a>>()
    let set = HashSet<'a>()
    let times = Dict.empty<'a, SimpleOrder.SortKey>

    let submitDeltas (v, listDeltas, setDeltas) =
        lock listReaders (fun () ->
            for r in listReaders do 
                r.Emit(content, Some listDeltas)
        )

        lock setReaders (fun () ->
            for r in setReaders do 
                r.Emit(set, Some setDeltas)
        )

        v

    let submit() =
        lock listReaders (fun () ->
            for r in listReaders do 
                r.Emit(content, None)
        )

        lock setReaders (fun () ->
            for r in setReaders do 
                r.Emit(set, None)
        )
 

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
        if set.Add value then
            let newTime = order.After t
            content.Add(newTime, value)
            set.Add value |> ignore

            setTime value newTime
            true, [Add (newTime :> ISortKey, value)], [Add value]
        else
            false, [], []

    let insertRangeAfter (t : SimpleOrder.SortKey) (values : seq<'a>) =
        let listDeltas = List()
        let setDeltas = List()

        let mutable current = t
        for e in values do
            if set.Add e then
                let newTime = order.After current
                content.Add(newTime, e)
                setTime e newTime

                setDeltas.Add(Add e)
                listDeltas.Add(Add (newTime :> ISortKey, e))

                current <- newTime

        (), Seq.toList listDeltas, Seq.toList setDeltas

    let remove (value : 'a) =
        if set.Remove value then
            match tryRemoveTime value with
                | Some t ->
                    content.Remove t |> ignore
                    order.Delete t
                    true, [Rem (t :> ISortKey, value)], [Rem value]

                | None ->
                    false, [], []
        else
            false, [], []

    let removeRange (values : seq<'a>) =
        let setDeltas = List()
        let listDeltas = List()

        for e in values do
            if set.Remove e then
                match tryRemoveTime e with
                    | Some t ->
                        content.Remove t |> ignore
                        order.Delete t
                        listDeltas.Add(Rem (t :> ISortKey, e))
                        setDeltas.Add(Rem e)

                    | None -> ()

        (), Seq.toList listDeltas, Seq.toList setDeltas



    let clear() =
        if content.Count > 0 then
            content.Clear()
            order.Clear()
            times.Clear()
            set.Clear()
            true
        else
            false

    let toSeq() =
        let rec toSeq(start : SimpleOrder.SortKey) =
            if start = order.Root then 
                Seq.empty
            else
                seq {
                    match content.TryGetValue start with
                        | (true, v) -> yield v
                        | _ -> ()

                    yield! toSeq start.Next
                }

        toSeq order.Root.Next

    do insertRangeAfter order.Root initial |> ignore

    interface alist<'a> with
        member x.GetReader() =
            lock listReaders (fun () ->
                let r = new EmitReader<'a>(content, order, fun r -> lock listReaders (fun () -> listReaders.Remove r |> ignore))
                r.Emit (content, None)
                listReaders.Add r |> ignore
                r :> _
            )

    interface aset<'a> with
        member x.ReaderCount = lock setReaders (fun () -> setReaders.Count)
        member x.IsConstant = false

        member x.Copy = x :> aset<_> 

        member x.GetReader() =
            lock setReaders (fun () ->
                let r = new ASetReaders.EmitReader<'a>(content, fun r -> lock setReaders (fun () -> setReaders.Remove r |> ignore))
                r.Emit(set, None)
                setReaders.Add r |> ignore
                r :> _
            )

    member x.Count = lock content (fun () -> content.Count)

    member x.Remove(item : 'a) =
        let res = lock content (fun () -> remove item)
        res |> submitDeltas

    member x.InsertAfter(prev : 'a, value : 'a) =
        let res = 
            lock content (fun () -> 
                match tryGetTime prev with
                    | Some t -> 
                        insertAfter t value
                    | None ->
                        failwithf "set does not contain element: %A" prev
            )
        res |> submitDeltas

    member x.InsertBefore(next : 'a, value : 'a) =
        let res = 
            lock content (fun () -> 
                match tryGetTime next with
                    | Some t -> 
                        insertAfter t.Prev value
                    | None ->
                        failwithf "set does not contain element: %A" next
            )
        res |> submitDeltas

    member x.TryGetNext(value : 'a, [<Out>] next : byref<'a>) =
        match times.TryGetValue(value) with
            | (true, t) ->
                if order.Root <> t.Next then 
                    match content.TryGetValue t.Next with
                        | (true, v) ->
                            next <- v
                            true
                        | _ -> 
                            false
                else
                    false
            | _ -> false

    member x.TryGetPrev(value : 'a, [<Out>] prev : byref<'a>) =
        match times.TryGetValue(value) with
            | (true, t) ->
                if order.Root <> t.Prev then 
                    match content.TryGetValue t.Prev with
                        | (true, v) ->
                            prev <- v
                            true
                        | _ -> 
                            false
                else
                    false
            | _ -> false


    member x.Add(value : 'a) =
        let res = lock content (fun () -> insertAfter order.Root.Prev value)
        res |> submitDeltas

    member x.UnionWith(values : seq<'a>) =
        let res = lock content (fun () -> insertRangeAfter order.Root.Prev values)
        res |> submitDeltas

    member x.ExceptWith(values : seq<'a>) =
        let res = lock content (fun () -> removeRange values)     
        res |> submitDeltas

    member x.IntersectWith(values : seq<'a>) : unit =
        failwith "not implemented"

    member x.SymmetricExceptWith(values : seq<'a>) : unit =
        failwith "not implemented"


    member x.CopyTo(arr : 'a[], index : int) =
        lock content (fun () ->
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
        if lock content (fun () -> clear()) then
            submit()

    member x.Contains item =
        lock content (fun () -> set.Contains item)

    member x.IsProperSubsetOf(other: IEnumerable<'a>): bool = lock content (fun () -> set.IsProperSubsetOf other)
    member x.IsProperSupersetOf(other: IEnumerable<'a>): bool = lock content (fun () -> set.IsProperSupersetOf other)
    member x.IsSubsetOf(other: IEnumerable<'a>): bool = lock content (fun () -> set.IsSubsetOf other)
    member x.IsSupersetOf(other: IEnumerable<'a>): bool = lock content (fun () -> set.IsSupersetOf other)
    member x.Overlaps(other: IEnumerable<'a>): bool = lock content (fun () -> set.Overlaps other)
    member x.SetEquals(other: IEnumerable<'a>): bool = lock content (fun () -> set.SetEquals other)

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() =
            (toSeq() :> System.Collections.IEnumerable).GetEnumerator()

    interface System.Collections.Generic.IEnumerable<'a> with
        member x.GetEnumerator() =
            (toSeq() :> seq<'a>).GetEnumerator()

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