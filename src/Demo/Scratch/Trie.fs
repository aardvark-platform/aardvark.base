namespace Trie

open Aardvark.Base
open System.Collections.Generic
open FSharp.Data.Adaptive

[<AllowNullLiteral>]
type TrieReference<'k, 'a> =
    abstract member Key : list<'k>
    abstract member Prev : voption<TrieReference<'k, 'a>>
    abstract member Next : voption<TrieReference<'k, 'a>>
    abstract member Value : 'a with get, set

[<AutoOpen>]
module private OrderMaintenanceTrieImplementation = 
    [<AllowNullLiteral>]
    type Linked<'s when 's :> Linked<'s> and 's : null> =
        abstract member Prev : 's with get, set
        abstract member Next : 's with get, set

    type IOrderedDict<'k, 'v when 'v :> Linked<'v> and 'v : null> =
        abstract member Keys : seq<'k>
        abstract member TryRemove : key : 'k -> voption<'v>
        abstract member TryGet : key : 'k -> voption<'v>
        abstract member GetOrCreate : key : 'k * create : ('k -> 'v -> 'v -> 'v) -> 'v
        abstract member First : 'v
        abstract member Last : 'v

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
        val mutable public Children : IOrderedDict<'k, OrderMaintenanceTrieNode<'k, 'a>>
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
                if isNull x.Children.First then
                    if isNull x.Next then 
                        if isNull x.Parent then ValueNone
                        else x.Parent.AfterLast()
                    else 
                        x.Next.First
                else
                    x.Children.First.First

            member x.Value
                with get() = match x.Value with | ValueSome v -> v | _ -> failwith "bad"
                and set v = x.Value <- ValueSome v

        member x.Last : voption<TrieReference<'k, 'a>> =
            if isNull x.Children.Last then
                match x.Value with
                | ValueSome _ -> ValueSome (x :> TrieReference<_,_>)
                | ValueNone -> 
                    if isNull x.Prev then ValueNone
                    else x.Prev.Last
            else
                x.Children.Last.Last

        member x.First : voption<TrieReference<'k, 'a>> = 
            match x.Value with
            | ValueSome _ ->
                ValueSome (x :> TrieReference<_,_>)
            | ValueNone ->
                if isNull x.Children.First then
                    if isNull x.Next then ValueNone
                    else x.Next.First
                else
                    x.Children.First.First

        member x.IsEmpty =
            ValueOption.isNone x.Value && isNull x.Children.First

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
                match x.Children.TryGet h with
                | ValueSome c ->
                    match c.TryRemove t with
                    | ValueSome (prev,next) ->
                        if c.IsEmpty then
                            x.Children.TryRemove h |> ignore
                    
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
                let node = 
                    x.Children.GetOrCreate(h, fun k l r ->
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
                let node = 
                    x.Children.GetOrCreate(h, fun k l r ->
                        let n = OrderMaintenanceTrieNode<'k, 'a>(x.GetComparer, x.Level + 1, k :: x.Path)
                        n.Prev <- l
                        n.Next <- r
                        n.Parent <- x
                        n
                    )
                node.AddOrUpdate(t, create)

        member x.Iter (action : list<'k> -> 'a -> unit) =
            match x.Value with
            | ValueSome v -> action (List.rev x.Path) v
            | ValueNone -> ()

            let mutable c = x.Children.First
            while not (isNull c) do
                c.Iter action
                c <- c.Next

        member x.Validate() =
            if x.IsEmpty && not (List.isEmpty x.Path) then failwithf "empty node with key: %A" (List.rev x.Path)
        
            let mutable reached = HashSet.empty
            let mutable missing = HashSet.ofSeq x.Children.Keys
            let mutable f = x.Children.First
            let mutable lastKey = ValueNone

            let cmp = x.GetComparer(x.Level + 1)

            while not (isNull f) do
                let k = List.head f.Path

                match lastKey with
                | ValueSome last ->
                    match cmp with
                    | Some cmp -> 
                        if cmp.Compare(last, k) > 0 then failwithf "inconsistent order on level %d (%A > %A)" x.Level last k
                    | None ->
                        ()
                | ValueNone ->
                    ()

                missing <- HashSet.remove k missing

                if HashSet.contains k reached then
                    failwithf "duplicate key: %A" k
                reached <- HashSet.add k reached

                if f.Parent <> x then
                    failwithf "bad parent for %A: %A" (List.rev f.Path) (List.rev f.Parent.Path)

                f.Validate()

                f <- f.Next
                lastKey <- ValueSome k

            if missing.Count > 0 then 
                failwithf "not all keys reached: %A" missing


        new(getComparer : int -> option<IComparer<'k>>, l : int, ks : list<'k>) = 
            { 
                GetComparer = getComparer; Level = l
                Path = ks
                Children = OrderedDict.ofComparer (getComparer (l + 1))
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
 
type OrderMaintenanceTrie<'k, 'a>(getComparer : int -> option<IComparer<'k>>) =
    let comparerCache = IntDict<option<IComparer<'k>>>()
    let getCachedComparer level = comparerCache.GetOrCreate(level, System.Func<_,_>(getComparer))
    let mutable root = OrderMaintenanceTrieNode<'k, 'a>(getCachedComparer, -1, [])

    member x.IsEmpty = 
        root.IsEmpty

    member x.Add (key : list<'k>, value : 'a) =
        root.Add(key, value)

    member x.Iter (action : list<'k> -> 'a -> unit) =
        root.Iter(action)

    member x.TryRemove(key : list<'k>) =
        root.TryRemove key

    member x.AddOrUpdate(key : list<'k>, create : voption<'a> -> 'a) =
        root.AddOrUpdate(key, create)
        
    member x.Clear() =
        root <- OrderMaintenanceTrieNode<'k, 'a>(getCachedComparer, 0, [])

    member internal x.Validate() = root.Validate()
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

module OrderMaintenanceTrieTest =

    let validateInternal (getComparer : int -> option<IComparer<'k>>) (t : OrderMaintenanceTrie<'k, 'v>) =
        t.Validate()

        let forward = 
            let res = System.Collections.Generic.List<list<'k> * 'v>()
            let mutable c = t.First
            while (ValueOption.isSome c) do
                res.Add(c.Value.Key, c.Value.Value)
                c <- c.Value.Next
            res

        let backward = 
            let res = System.Collections.Generic.List<list<'k> * 'v>()
            let mutable c = t.Last
            while (ValueOption.isSome c) do
                res.Add(c.Value.Key, c.Value.Value)
                c <- c.Value.Prev
            res

        let all =
            let res = System.Collections.Generic.List<list<'k> * 'v>()
            t.Iter(fun k v -> res.Add(k,v))
            res


        if forward.Count <> all.Count then 
            failwithf "inconsistent forward count: %A vs %A" forward.Count all.Count

        if backward.Count <> all.Count then 
            failwithf "inconsistent backward counts: %A vs %A" backward.Count all.Count
            
        let mutable j = backward.Count - 1
        for i in 0 .. forward.Count - 1 do
            if forward.[i] <> backward.[j] then
                failwithf "error at position %d: %A vs %A" i forward.[i] backward.[j]
            if forward.[i] <> all.[i] then
                failwithf "error at position %d: %A vs %A" i forward.[i] all.[j]
            j <- j - 1

    let validateAgainst (t : OrderMaintenanceTrie<'k, 'v>) (m : HashMap<list<'k>, 'v>) =
        let mutable missing = m
        t.Iter (fun k v ->
            match HashMap.tryRemove k missing with
            | Some (hv, r) ->
                if v <> hv then failwithf "inconsistent value %A vs %A" v hv
                missing <- r
            | None ->
                failwithf "superflous value for %A: %A" k v
        )

        if missing.Count > 0 then 
            failwithf "missing values: %A" missing

    type ValidationTrie<'k, 'v when 'k : equality and 'v : equality>(getComparer : int -> option<IComparer<'k>>) =

        let trie = OrderMaintenanceTrie<'k, 'v>(getComparer)
        let mutable store = HashMap.empty

        let check() =
            validateInternal getComparer trie
            validateAgainst trie store

        member x.Forward =
            let rec s (r : voption<TrieReference<_,_>>) =
                match r with
                | ValueSome r -> 
                    Seq.append
                        (Seq.singleton (r.Key, r.Value))
                        (Seq.delay (fun () -> s r.Next))
                | ValueNone ->
                    Seq.empty
            s trie.First
            
        member x.Count = store.Count
        member x.Content = store
        member x.Trie = trie

        member x.Add(key : list<'k>, value : 'v) =
            trie.Add(key, value) |> ignore
            store <- HashMap.add key value store
            check()

        member x.Remove(key : list<'k>) =
            trie.TryRemove(key) |> ignore
            store <- HashMap.remove key store
            check()

        member x.AddOrUpdate(key : list<'k>, create : voption<'v> -> 'v) =

            let mutable other = None
            let realCreate (o : voption<'v>) =
                match other with
                | Some (oo, on) ->
                    if oo <> o then failwithf "bad old value: %A vs %A" oo o
                    on
                | None ->
                    let r = create o
                    other <- Some (o, r)
                    r

            trie.AddOrUpdate(key, realCreate) |> ignore
            store <- store.AlterV(key, realCreate >> ValueSome)
            check()

    let run() =
        let rand = RandomSystem()
        
        let comparers =
            Seq.initInfinite (fun i ->
                if i % 2 = 0 then Comparer<int>.Default :> IComparer<_> |> Some
                else None
            ) |> Seq.cache
        

        let t = 
            ValidationTrie (fun l ->
                Seq.item l comparers
            )

        let mutable updates = 0
        let mutable adds = 0
        let mutable removes = 0
        let mutable dummyRemoves = 0
        let mutable maxCount = 0

        let add() =
            if rand.UniformDouble() > 0.2 || t.Count = 0 then
                // new key
                let k = 
                    let len = rand.UniformInt(5) + 1
                    List.init len (fun _ -> rand.UniformInt(20))

                adds <- adds + 1
                let v = List.sum k
                t.Add(k, v)
            else
                // existing key 
                let k = t.Content |> Seq.item (rand.UniformInt t.Count) |> fst
                updates <- updates + 1
                if rand.UniformDouble() > 0.5 then
                    t.Add(k, rand.UniformInt())
                else
                    t.AddOrUpdate(k, function ValueSome o -> 2*o | _ -> failwith "should exist")
            maxCount <- max maxCount t.Count

        let rem() = 
            if rand.UniformDouble() > 0.05 && t.Count > 0 then
                // existing key
                removes <- removes + 1
                let k = t.Content |> Seq.item (rand.UniformInt t.Count) |> fst
                t.Remove(k)
            else
                dummyRemoves <- dummyRemoves + 1
                // non existing key
                let k = 
                    let len = rand.UniformInt(5) + 1
                    List.init len (fun _ -> rand.UniformInt(100))
                t.Remove k
            maxCount <- max maxCount t.Count

        let randomOp () =
            if t.Count > 1 then
                if rand.UniformDouble() > 0.55 then
                    rem()
                else
                    add()
            else
                add()
                
        let iter = 10000
        Log.startTimed "random ops"
        for i in 1 .. iter do
            randomOp()
            Report.Progress(float i / float iter)

        Log.line "%d adds" adds
        Log.line "%d updates" updates
        Log.line "%d removes" removes
        Log.line "%d removes of non existing" dummyRemoves
        Log.line "%d maximal entries" maxCount
        Log.stop()

        t.Trie.Validate()
        Log.start "content"
        for (k, v) in t.Trie do
            Log.line "%A: %A" k v
        Log.stop()

