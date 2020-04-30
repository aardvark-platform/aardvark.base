namespace Aardvark.Base

open Aardvark.Base
open System.Collections.Generic
open FSharp.Data.Adaptive

#nowarn "1336"

[<AllowNullLiteral>]
type TrieReference<'k, 'a> =
    abstract member Key : list<'k>
    abstract member Prev : voption<TrieReference<'k, 'a>>
    abstract member Next : voption<TrieReference<'k, 'a>>
    abstract member Value : 'a with get, set

[<CompilerMessage("for internal use only", 1336, IsHidden = true)>]
module OrderMaintenanceTrieImplementation = 
    [<AllowNullLiteral>]
    type Linked<'s when 's :> Linked<'s> and 's : null> =
        abstract member Prev : 's with get, set
        abstract member Next : 's with get, set

        
    [<AllowNullLiteral>]
    type IOrderedDict<'k, 'v when 'v :> Linked<'v> and 'v : null> =
        abstract member Keys : seq<'k>
        abstract member TryRemove : key : 'k -> voption<'v>
        abstract member TryGet : key : 'k -> voption<'v>
        abstract member GetOrCreate : key : 'k * create : ('k -> 'v -> 'v -> 'v) -> 'v
        abstract member First : 'v
        abstract member Last : 'v
        
    [<AllowNullLiteral>]
    type UnorderedDict<'k, 'v when 'v :> Linked<'v> and 'v : null>() =
        let mutable first : 'v = null
        let mutable last : 'v = null
        let store = Dict<'k, 'v>()

        member x.GetOrCreate(key : 'k, create : 'k -> 'v -> 'v -> 'v) =
            store.GetOrCreate(key, fun k ->
                let n = create k last null
                if isNull last then first <- n
                else last.Next <- n
                last <- n
                n
            )

        member x.TryRemove(key : 'k) =
            match store.TryRemove key with
            | (true, node) ->
                if isNull node.Prev then first <- node.Next
                else node.Prev.Next <- node.Next

                if isNull node.Next then last <- node.Prev
                else node.Next.Prev <- node.Prev

                node.Prev <- null
                node.Next <- null

                ValueSome node
            | _ ->
                ValueNone
        
        member x.TryGet(key : 'k) =
            match store.TryGetValue key with
            | (true, node) -> ValueSome node
            | _ -> ValueNone

        interface IOrderedDict<'k, 'v> with
            member x.Keys = store.Keys
            member x.First = first
            member x.Last = last
            member x.GetOrCreate(key, create) = x.GetOrCreate(key, create)
            member x.TryRemove(key) = x.TryRemove(key)
            member x.TryGet(key) = x.TryGet(key)
        
    [<AllowNullLiteral>]
    type OrderedDict<'k, 'v when 'v :> Linked<'v> and 'v : null>(cmp : IComparer<'k>) =
        let mutable first : 'v = null
        let mutable last : 'v = null

        static let vo (o : Optional<struct('k * 'v)>) =
            if o.HasValue then
                let struct(_, v) = o.Value
                v
            else
                null


        let store = 
            SortedSetExt<struct ('k * 'v)> { 
                new IComparer<struct ('k * 'v)> with
                    member x.Compare((struct(lk,_)), (struct(rk, _))) = cmp.Compare(lk, rk)
            }

        member x.GetOrCreate(key : 'k, create : 'k -> 'v -> 'v -> 'v) =
            let l, s, r = store.FindNeighbours(struct (key, null))

            if s.HasValue then
                let struct(_,v) = s.Value
                v
            else
                let l = vo l
                let r = vo r
                let node = create key l r
                store.Add(struct(key, node)) |> ignore

                if isNull l then first <- node
                else l.Next <- node

                if isNull r then last <- node
                else r.Prev <- node

                node

        member x.TryRemove(key : 'k) =
            let _, s, _ = store.FindNeighbours(struct (key, null))

            if s.HasValue then
                let struct (_, node) = s.Value
                store.Remove(struct (key, node)) |> ignore
                let l = node.Prev
                let r = node.Next

                if isNull l then first <- r
                else l.Next <- r

                if isNull r then last <- l
                else r.Prev <- l

                node.Prev <- null
                node.Next <- null

                ValueSome node
            else
                ValueNone
        
        member x.TryGet(key : 'k) =
            let _, s, _ = store.FindNeighbours(struct (key, null))
            if s.HasValue then
                let struct (_, node) = s.Value
                ValueSome node
            else
                ValueNone

        interface IOrderedDict<'k, 'v> with
            member x.Keys = store |> Seq.map (fun struct (k,_) -> k)
            member x.First = first
            member x.Last = last
            member x.GetOrCreate(key, create) = x.GetOrCreate(key, create)
            member x.TryRemove(key) = x.TryRemove(key)
            member x.TryGet(key) = x.TryGet(key)

    module OrderedDict =
        let ofComparer (cmp : option<System.Collections.Generic.IComparer<'k>>) =
            match cmp with
            | None ->
                UnorderedDict<'k, 'v>() :> IOrderedDict<_,_>
            | Some cmp ->
                OrderedDict<'k, 'v>(cmp) :> IOrderedDict<_,_>
 
    [<AllowNullLiteral>]
    type OrderMaintenanceTrieNode<'k, 'a> =
        val mutable public GetComparer : int -> option<IComparer<'k>>
        val mutable public Level : int
        val mutable public Path : list<'k>
        val mutable public ChildrenDict : IOrderedDict<'k, OrderMaintenanceTrieNode<'k, 'a>>
        val mutable public Value : ValueOption<'a>
    
        val mutable public Parent : OrderMaintenanceTrieNode<'k, 'a>
        val mutable public Prev : OrderMaintenanceTrieNode<'k, 'a>
        val mutable public Next : OrderMaintenanceTrieNode<'k, 'a>

        member private x.AfterLast() =
            if isNull x.Next then 
                if isNull x.Parent then ValueNone
                else x.Parent.AfterLast()
            else
                x.Next.First

        interface Linked<OrderMaintenanceTrieNode<'k, 'a>> with
            member x.Prev
                with get() = x.Prev
                and set p = x.Prev <- p
            member x.Next
                with get() = x.Next
                and set p = x.Next <- p

        interface TrieReference<'k, 'a> with
            member x.Key = List.rev x.Path
            member x.Prev =
                if isNull x.Prev then
                    if isNull x.Parent then
                        ValueNone
                    elif ValueOption.isSome x.Parent.Value then
                        ValueSome (x.Parent :> TrieReference<_,_>)
                    else
                        (x.Parent :> TrieReference<_,_>).Prev
                else
                    x.Prev.Last
            member x.Next =
                if isNull x.ChildrenDict || isNull x.ChildrenDict.First then
                    if isNull x.Next then 
                        if isNull x.Parent then ValueNone
                        else x.Parent.AfterLast()
                    else 
                        x.Next.First
                else
                    x.ChildrenDict.First.First

            member x.Value
                with get() = match x.Value with | ValueSome v -> v | _ -> failwith "bad"
                and set v = x.Value <- ValueSome v

        member x.Last : voption<TrieReference<'k, 'a>> =
            if isNull x.ChildrenDict || isNull x.ChildrenDict.Last then
                match x.Value with
                | ValueSome _ -> ValueSome (x :> TrieReference<_,_>)
                | ValueNone -> 
                    if isNull x.Prev then ValueNone
                    else x.Prev.Last
            else
                x.ChildrenDict.Last.Last

        member x.First : voption<TrieReference<'k, 'a>> = 
            match x.Value with
            | ValueSome _ ->
                ValueSome (x :> TrieReference<_,_>)
            | ValueNone ->
                if isNull x.ChildrenDict || isNull x.ChildrenDict.First then
                    if isNull x.Next then ValueNone
                    else x.Next.First
                else
                    x.ChildrenDict.First.First

        member x.IsEmpty =
            ValueOption.isNone x.Value && (isNull x.ChildrenDict || isNull x.ChildrenDict.First)

        member x.TryRemove(k : list<'k>) =
            match k with
            | [] ->
                match x.Value with
                | ValueSome _ -> 
                    let r = x :> TrieReference<_,_>
                    let p = r.Prev
                    let n = r.Next
                    x.Value <- ValueNone
                    ValueSome (p, n)
                | ValueNone ->
                    ValueNone
            | h :: t ->
                if isNull x.ChildrenDict then
                    ValueNone
                else
                    match x.ChildrenDict.TryGet h with
                    | ValueSome c ->
                        match c.TryRemove t with
                        | ValueSome (prev,next) ->
                            if c.IsEmpty then
                                x.ChildrenDict.TryRemove h |> ignore
                    
                            ValueSome(prev,next)
                        | _ ->
                            ValueNone
                    | _ ->
                        ValueNone

        member x.Add(k : list<'k>, value : 'a) =
            match k with
            | [] ->
                x.Value <- ValueSome value
                x :> TrieReference<_,_>
            | h :: t ->
                let children = 
                    if isNull x.ChildrenDict then 
                        let d = OrderedDict.ofComparer (x.GetComparer (x.Level + 1))
                        x.ChildrenDict <- d
                        d
                    else 
                        x.ChildrenDict
                    
                let node = 
                    children.GetOrCreate(h, fun k l r ->
                        let n = OrderMaintenanceTrieNode<'k, 'a>(x.GetComparer, x.Level + 1, k :: x.Path)
                        n.Prev <- l
                        n.Next <- r
                        n.Parent <- x
                        n
                    )
                node.Add(t, value)
            
        member x.AddOrUpdate(k : list<'k>, create : voption<'a> -> 'a) =
            match k with
            | [] ->
                x.Value <- ValueSome (create x.Value)
                x :> TrieReference<_,_>
            | h :: t ->
                let children = 
                    if isNull x.ChildrenDict then 
                        let d = OrderedDict.ofComparer (x.GetComparer (x.Level + 1))
                        x.ChildrenDict <- d
                        d
                    else 
                        x.ChildrenDict
                let node = 
                    children.GetOrCreate(h, fun k l r ->
                        let n = OrderMaintenanceTrieNode<'k, 'a>(x.GetComparer, x.Level + 1, k :: x.Path)
                        n.Prev <- l
                        n.Next <- r
                        n.Parent <- x
                        n
                    )
                node.AddOrUpdate(t, create)

        member x.Alter(k : list<'k>, update : voption<'a> -> voption<'a>) =
            match k with
            | [] ->
                x.Value <- update x.Value
                match x.Value with
                | ValueSome _ ->
                    ValueSome (x :> TrieReference<_,_>)
                | _ ->
                    ValueNone
            | h :: t ->
                if isNull x.ChildrenDict then
                    match update ValueNone with
                    | ValueNone ->
                        ValueNone
                    | ValueSome v ->
                        x.Add(h :: t, v) |> ValueSome
                else
                    match x.ChildrenDict.TryGet h with
                    | ValueSome c ->
                        let res = c.Alter(t, update)
                        match res with
                        | ValueNone ->  
                            if c.IsEmpty then
                                x.ChildrenDict.TryRemove h |> ignore
                        | _ ->
                            ()
                        res
                    | ValueNone ->
                        match update ValueNone with
                        | ValueNone ->
                            ValueNone
                        | ValueSome v ->
                            x.Add(h :: t, v) |> ValueSome
           

        member x.Iter (action : list<'k> -> 'a -> unit) =
            match x.Value with
            | ValueSome v -> action (List.rev x.Path) v
            | ValueNone -> ()

            if not (isNull x.ChildrenDict) then
            
                let mutable c = x.ChildrenDict.First
                while not (isNull c) do
                    c.Iter action
                    c <- c.Next

        member x.TryGetValue (key : list<'k>) =
            match key with
            | [] ->
                x.Value
            | h :: t ->
                if isNull x.ChildrenDict then
                    ValueNone
                else
                    match x.ChildrenDict.TryGet(h) with
                    | ValueSome c -> c.TryGetValue t
                    | ValueNone -> ValueNone
                    
        member x.TryGetReference(key : list<'k>) =
            match key with
            | [] ->
                match x.Value with
                | ValueSome _ -> ValueSome (x :> TrieReference<_,_>)
                | ValueNone -> ValueNone
            | h :: t ->
                if isNull x.ChildrenDict then
                    ValueNone
                else
                    match x.ChildrenDict.TryGet(h) with
                    | ValueSome c -> c.TryGetReference t
                    | ValueNone -> ValueNone

        member x.ContainsKey (key : list<'k>) =
            match key with
            | [] ->
                ValueOption.isSome x.Value
            | h :: t ->
                if isNull x.ChildrenDict then
                    false
                else
                    match x.ChildrenDict.TryGet(h) with
                    | ValueSome c -> c.ContainsKey t
                    | ValueNone -> false
            

        new(getComparer : int -> option<IComparer<'k>>, l : int, ks : list<'k>) = 
            { 
                GetComparer = getComparer; Level = l
                Path = ks
                ChildrenDict = null //OrderedDict.ofComparer (getComparer (l + 1))
                Value = ValueNone
                Parent = null
                Prev = null; Next = null
            }

    type TrieReferenceForwardEnumerator<'k, 'v>(initial : unit -> voption<TrieReference<'k, 'v>>) =
        let mutable initial = initial
        let mutable started = false
        let mutable current : TrieReference<'k, 'v> = null

        member x.MoveNext() =
            if not started then
                started <- true
                match initial() with
                | ValueSome initial ->
                    current <- initial
                    true
                | ValueNone ->
                    false
            elif not (isNull current) then
                match current.Next with
                | ValueSome n ->
                    current <- n
                    true
                | ValueNone ->
                    current <- null
                    false
            else
                false
               
        member x.Reset() =
            started <- false
            current <- null

        member x.Dispose() =
            started <- false 
            current <- null
            initial <- fun _ -> ValueNone

        member x.Current =
            current.Key, current.Value
            
        interface System.Collections.IEnumerator with
            member x.MoveNext() = x.MoveNext()
            member x.Reset() = x.Reset()
            member x.Current = x.Current :> obj

        interface IEnumerator<list<'k> * 'v> with
            member x.Dispose() = x.Dispose()
            member x.Current = x.Current
 
open OrderMaintenanceTrieImplementation

type OrderMaintenanceTrie<'k, 'a>(getComparer : int -> option<IComparer<'k>>) =
    let comparerCache = IntDict<option<IComparer<'k>>>()
    let getCachedComparer level = comparerCache.GetOrCreate(level, System.Func<_,_>(getComparer))
    let mutable root = OrderMaintenanceTrieNode<'k, 'a>(getCachedComparer, -1, [])

    member x.IsEmpty = 
        root.IsEmpty

    member x.Set (key : list<'k>, value : 'a) =
        root.Add(key, value)

    member x.Iter (action : list<'k> -> 'a -> unit) =
        root.Iter(action)

    member x.TryRemove(key : list<'k>) =
        root.TryRemove key

    member x.AddOrUpdate(key : list<'k>, create : voption<'a> -> 'a) =
        root.AddOrUpdate(key, create)
        
    member x.Alter(key : list<'k>, update : voption<'a> -> voption<'a>) =
        root.Alter(key, update)

    member x.TryGetValue (key : list<'k>) =
        root.TryGetValue key
                    
    member x.TryGetReference(key : list<'k>) =
        root.TryGetReference key

    member x.ContainsKey (key : list<'k>) =
        root.ContainsKey key

    member x.Clear() =
        root <- OrderMaintenanceTrieNode<'k, 'a>(getCachedComparer, 0, [])

    member x.Root = root
    member x.First = root.First
    member x.Last = root.Last

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() = new TrieReferenceForwardEnumerator<_,_>(fun () -> x.First) :> _
        
    interface IEnumerable<list<'k> * 'a> with
        member x.GetEnumerator() = new TrieReferenceForwardEnumerator<_,_>(fun () -> x.First) :> _

    new() = 
        OrderMaintenanceTrie(fun _ -> None)

    new(cmp : IComparer<'k>) = 
        let res = Some cmp
        OrderMaintenanceTrie(fun _ -> res)
