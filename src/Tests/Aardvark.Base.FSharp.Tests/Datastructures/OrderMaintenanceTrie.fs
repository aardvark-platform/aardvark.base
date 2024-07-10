namespace Aardvark.Base.FSharp.Tests

#nowarn "1336"

open Aardvark.Base.OrderMaintenanceTrieImplementation
open System.Collections.Generic
open NUnit.Framework
open Aardvark.Base
open FSharp.Data.Adaptive

module OrderMaintenanceTrieTest =
    let rec validateTree (x : OrderMaintenanceTrieNode<_,_>) =
        if x.IsEmpty && not (List.isEmpty x.Path) then failwithf "empty node with key: %A" (List.rev x.Path)
        
        if not (isNull x.ChildrenDict) then
            let mutable reached = HashSet.empty
            let mutable missing = HashSet.ofSeq x.ChildrenDict.Keys
            let mutable f = x.ChildrenDict.First
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

                validateTree f

                f <- f.Next
                lastKey <- ValueSome k

            if missing.Count > 0 then 
                failwithf "not all keys reached: %A" missing

    let validateInternal (getComparer : int -> option<IComparer<'k>>) (t : OrderMaintenanceTrie<'k, 'v>) =
        validateTree t.Root

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

        member x.Set(key : list<'k>, value : 'v) =
            trie.Set(key, value) |> ignore
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


    [<Test>]
    let ``[OrderMaintenanceTrie] validation``() =
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
                t.Set(k, v)
            else
                // existing key 
                let k = t.Content |> Seq.item (rand.UniformInt t.Count) |> fst
                updates <- updates + 1
                if rand.UniformDouble() > 0.5 then
                    t.Set(k, rand.UniformInt())
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
                
        let iter = 50000
        Log.startTimed "random ops"
        for i in 1 .. iter do
            randomOp()

        Log.line "%d adds" adds
        Log.line "%d updates" updates
        Log.line "%d removes" removes
        Log.line "%d removes of non existing" dummyRemoves
        Log.line "%d maximal entries" maxCount
        Log.stop()

        Log.start "content"
        for (k, v) in t.Trie do
            Log.line "%A: %A" k v
        Log.stop()