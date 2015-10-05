namespace Aardvark.Base.Incremental.Tests

open System
open System.Threading
open System.Threading.Tasks
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit

module ``collect tests`` =
    type Tree<'a> = Node of aset_check<Tree<'a>> | Leaf of aset_check<'a>

    type DeltaList<'a>() =
        let store = List<list<Delta<'a>>>()

        member x.push (deltas : list<Delta<'a>>) =
            store.Add deltas 

        member x.read() =
            let res = store |> Seq.toList
            store.Clear()
            res


    [<Test>]
    let ``[ASet] duplicate handling in collect``() =
        let i0 = CSetCheck.ofList [1;2;3]
        let i1 = CSetCheck.ofList [3;4;5]
        let s = CSetCheck.ofList [i0 :> aset_check<_>]

        // union {{1,2,3}}
        let d = s |> ASetCheck.collect id
        let r = d.GetReader()
        r.GetDelta() |> should setEqual [Add 1; Add 2; Add 3]
        r.Content |> should setEqual [1;2;3]
        
        // union {{1,2,3}, {3,4,5}} -> { Add 4, Add 5 }
        transact (fun () ->
            s.Add i1 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 4; Add 5]
        r.Content |> should setEqual [1;2;3;4;5]

        // union {{3,4,5}} -> { Rem 1, Rem 2 }
        transact (fun () ->
            s.Remove i0 |> should equal true
        )
        r.GetDelta() |> should setEqual [Rem 1; Rem 2]
        r.Content |> should setEqual [3;4;5]

        // union {{4,5}} -> { Rem 3 }
        transact (fun () ->
            i1.Remove 3 |> should equal true
        )
        r.GetDelta() |> should setEqual [Rem 3]
        r.Content |> should setEqual [4;5]

        // union {{1,2,3}, {4,5}} -> { Add 1, Add 2, Add 3 }
        transact (fun () ->
            s.Add i0 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 1; Add 2; Add 3]
        r.Content |> should setEqual [1;2;3;4;5]

        // union {{1,2,3}, {3,4,5}} -> { }
        transact (fun () ->
            i1.Add 3 |> should equal true
        )
        r.GetDelta() |> should setEqual []
        r.Content |> should setEqual [1;2;3;4;5]

        // union {{1,2}, {3,4,5}} -> { }
        transact (fun () ->
            i0.Remove 3 |> should equal true
        )
        r.GetDelta() |> should setEqual []
        r.Content |> should setEqual [1;2;3;4;5]

        // union {{1,2}, {4,5}} -> { Rem 3 }
        transact (fun () ->
            i1.Remove 3 |> should equal true
        )
        r.GetDelta() |> should setEqual [Rem 3]
        r.Content |> should setEqual [1;2;4;5]

    [<Test>]
    let ``[ASet] move test``() =
        let c0 = CSetCheck.ofList [1]
        let c1 = CSetCheck.ofList [2]

        let s = CSetCheck.ofList[c0 :> aset_check<_>;c1 :> aset_check<_>]

        let res = s |> ASetCheck.collect id
        let r = res.GetReader()

        // union {{1},{2}}
        r.GetDelta() |> should setEqual [Add 1; Add 2]
        r.Content |> should setEqual [1;2]

        // union {{2},{1}}
        transact (fun () ->
            c0.Remove 1 |> should equal true
            c1.Add 1 |> should equal true
            c1.Remove 2 |> should equal true
            c0.Add 2 |> should equal true
        )
        r.GetDelta() |> should setEqual []
        r.Content |> should setEqual [1;2]
    
    [<Test>]
    let ``[ASet] tree flatten test``() =
        let rec flatten (t : Tree<'a>) =
            match t with
                | Node children -> children |> ASetCheck.collect (flatten)
                | Leaf values -> values

        let l2 = CSetCheck.ofList [1]
        let l1 = CSetCheck.ofList [Leaf l2]
        let l0 = CSetCheck.ofList [Node l1]
        let t = Node (l0)

        // Node { Node { Leaf { 1 } } }
        let s = flatten t
        let r = s.GetReader()
        r.GetDelta() |> should setEqual [Add 1]
        r.Content |> should setEqual [1]


        // Node { Node { Leaf { 1, 3 }, Leaf { 2 } } }
        let l22 = CSetCheck.ofList [2] :> aset_check<_> |> Leaf
        transact (fun () ->
            l1.Add l22 |> should equal true
            l2.Add 3 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 2; Add 3]
        r.Content |> should setEqual [1; 2; 3]


        // Node { Leaf { 1, 5 } , Node { Leaf { 1, 3 }, Leaf { 2 } } }
        let l12 = CSetCheck.ofList [1;5] :> aset_check<_> |> Leaf
        transact (fun () ->
            l0.Add l12 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 5]
        r.Content |> should setEqual [1; 2; 3; 5]

        // Node { Leaf { 1, 5 } , Node { Leaf { 2 } } }
        transact (fun () ->
            l1.Remove (Leaf l2) |> should equal true
        )
        r.GetDelta() |> should setEqual [Rem 3]
        r.Content |> should setEqual [1; 2; 5]

        // Node { Leaf { 1, 5 } , Node { Leaf { 2 }, Node { Leaf { 17 } } } }
        let n = Node (CSetCheck.ofList [Leaf (CSetCheck.ofList [17])])
        transact (fun () ->
            l1.Add n |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 17]
        r.Content |> should setEqual [1; 2; 5; 17]

        ()

    [<Test>]
    let ``[ASet] callback test``() =

        let deltas = DeltaList<int>()

        let set = cset [1;2;3;4]
        let m = set |> ASet.map (fun a -> 2 * a)
        let s = m |> ASet.unsafeRegisterCallbackNoGcRoot deltas.push

        // { 1, 2, 3, 4 } -> [Add 2; Add 4; Add 6; Add 8]
        deltas.read() |> should deltaListEqual [[Add 2; Add 4; Add 6; Add 8]]

        // { 1, 3, 4, 5 } -> [Add 10; Rem 4]
        transact (fun () ->
            set.Add 5 |> ignore
            set.Remove 2 |> ignore
        )
        deltas.read() |> should deltaListEqual [[Rem 4; Add 10]]

        // { 1, 4, 5, 6 } -> [Add 12; Rem 6]
        transact (fun () ->
            set.Add 6 |> ignore
            set.Remove 3 |> ignore
        )
        deltas.read() |> should deltaListEqual [[Rem 6; Add 12]]

        // { 1, 4, 5, 6, 7 } -> [Add 14]
        transact (fun () -> set.Add 7 |> ignore)

        // { 1, 4, 5, 6, 7, 8 } -> [Add 16]
        transact (fun () -> set.Add 8 |> ignore)
        deltas.read() |> should deltaListEqual [[Add 14]; [Add 16]]


        s.Dispose()

        // { 1, 4, 5, 6, 7 } -> [] (unsubscribed)
        transact (fun () -> set.Add 9 |> ignore)
        deltas.read() |> should deltaListEqual ([] : list<list<Delta<int>>>)




        ()

    [<Test>]
    let ``[ASet] callback changing other list``() =
        
        let set = cset [1;2]

        let derived = set |> ASet.map (fun a -> 1 + a)
        let inner = cset []

        let s = derived |> ASet.unsafeRegisterCallbackNoGcRoot (fun delta ->
            for d in delta do
                match d with
                    | Add v -> inner.Add v |> ignore
                    | Rem v -> inner.Remove v |> ignore
        )

        
        let dervivedInner = inner |> ASet.map id

        let r = dervivedInner.GetReader()
        r.GetDelta() |> should setEqual [Add 2; Add 3]

        transact (fun () ->
            set.Add 3 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 4]

    [<Test>]
    let ``[ASet] toMod triggering even with equal set referece``() =
        
        let s = CSet.empty

        let triggerCount = ref 0
        let hasTriggered() =
            let c = !triggerCount
            triggerCount := 0
            c > 0

        let leak = 
            s |> ASet.toMod |> Mod.unsafeRegisterCallbackNoGcRoot (fun set ->
                triggerCount := !triggerCount + 1
            )

        hasTriggered() |> should equal true


        transact(fun () ->
            CSet.add 1 s |> ignore
        )
        hasTriggered() |> should equal true

        transact(fun () ->
            CSet.add 1 s |> ignore
        )
        hasTriggered() |> should equal false

        transact(fun () ->
            CSet.remove 1 s |> ignore
        )
        hasTriggered() |> should equal true


        //pretend leak is used
        ignore leak



    [<Test>]
    let ``[ASet] concurrency collect test``() =

        let l = obj()
        let set = CSet.empty
        let derived = set |> ASet.collect id
        use reader = derived.GetReader()
        let numbers = [0..9999]

        use sem = new SemaphoreSlim(0)
        use cancel = new CancellationTokenSource()
        let ct = cancel.Token

        // pull from the system
        Task.Factory.StartNew(fun () ->
            while true do
                ct.ThrowIfCancellationRequested()
                let delta = reader.GetDelta()
                //Thread.Sleep(10)
                ()
        ) |> ignore


        // submit into the system
        for n in numbers do
            let s = ASet.single n
            Task.Factory.StartNew(fun () ->
                transact (fun () ->
                    lock l (fun () ->
                        set.Add(s) |> ignore
                    )
                )
                sem.Release() |> ignore
            ) |> ignore

        // wait for all submissions to be done
        for n in numbers do
            sem.Wait()

        cancel.Cancel()

        reader.GetDelta() |> ignore


        let content = reader.Content |> Seq.toList |> List.sort

        content |> should equal numbers

    [<Test>]
    let ``[ASet] concurrency multi reader test``() =

        let l = obj()
        let set = CSet.empty
        let derived = set |> ASet.collect id
        let numbers = [0..9999]

        use sem = new SemaphoreSlim(0)
        use cancel = new CancellationTokenSource()
        let ct = cancel.Token


        let readers = [0..3] |> List.map (fun _ -> derived.GetReader())
        // pull from the system

        for r in readers do
            Task.Factory.StartNew(fun () ->
                while true do
                    ct.ThrowIfCancellationRequested()
                    let delta = r.GetDelta()
                    if not (List.isEmpty delta) then
                        Console.WriteLine("delta: {0}", List.length delta)
                    //Thread.Sleep(1)
                    ()
            ) |> ignore


        // submit into the system
        for n in numbers do
            let s = ASet.single n
            Task.Factory.StartNew(fun () ->
                transact (fun () ->
                    lock l (fun () ->
                        set.Add(s) |> ignore
                    )
                )
                sem.Release() |> ignore
            ) |> ignore

        // wait for all submissions to be done
        for n in numbers do
            sem.Wait()

        cancel.Cancel()

        for r in readers do
            r.GetDelta() |> ignore
            let content = r.Content |> Seq.toList |> List.sort
            content |> should equal numbers


    [<Test>]
    let ``[ASet] concurrency buffered reader test``() =

        let l = obj()
        let set = CSet.empty

        let derived = set |> ASet.collect id |> ASet.map id // |> ASet.choose Some |> ASet.collect ASet.single |> ASet.collect ASet.single
        use reader = derived.GetReader()
        let numbers = [0..9999]

        use sem = new SemaphoreSlim(0)
        use cancel = new CancellationTokenSource()
        let ct = cancel.Token

        // pull from the system
        Task.Factory.StartNew(fun () ->
            while true do
                ct.ThrowIfCancellationRequested()
                let delta = reader.GetDelta()
                //Thread.Sleep(10)
                ()
        ) |> ignore


        // submit into the system
        for n in numbers do
            let s = ASet.single n
            Task.Factory.StartNew(fun () ->
                transact (fun () ->
                    lock l (fun () ->
                        set.Add(s) |> ignore
                    )
                )
                sem.Release() |> ignore
            ) |> ignore

        // wait for all submissions to be done
        for n in numbers do
            sem.Wait()

        cancel.Cancel()

        reader.GetDelta() |> ignore


        let content = reader.Content |> Seq.toList |> List.sort

        content |> should equal numbers


    [<Test>]
    let ``[ASet] concurrency overkill test``() =

        let l = obj()
        let set = CSet.empty

        let derived = set |> ASet.collect id |> ASet.map id |> ASet.choose Some |> ASet.collect ASet.single |> ASet.collect ASet.single
        let readers = [1..10] |> List.map (fun _ -> derived.GetReader())
        let numbers = [0..9999]

        use sem = new SemaphoreSlim(0)
        use cancel = new CancellationTokenSource()
        let ct = cancel.Token


        for r in readers do
            // pull from the system
            Task.Factory.StartNew(fun () ->
                while true do
                    ct.ThrowIfCancellationRequested()
                    let delta = r.GetDelta()
                    //Thread.Sleep(10)
                    ()
            ) |> ignore


        // submit into the system
        for n in numbers do
            let s = CSet.ofList [n]
            Task.Factory.StartNew(fun () ->
                transact (fun () ->
                    lock l (fun () ->
                        set.Add(s) |> ignore
                    )
                )
                sem.Release() |> ignore
            ) |> ignore

        // wait for all submissions to be done
        for n in numbers do
            sem.Wait()

        cancel.Cancel()

        for r in readers do
            r.GetDelta() |> ignore


        for r in readers do
            let content = r.Content |> Seq.toList |> List.sort

            content |> should equal numbers

    [<Test>]
    let ``[ASet] stacked deltas test`` () =

        let set = CSet.empty

        let derived = set |> ASet.collect id |> ASet.map id |> ASet.choose Some |> ASet.collect ASet.single |> ASet.collect ASet.single

        let readers = Array.init 10 (fun _ -> derived.GetReader())

        for r in readers do
            r.Update()
            r.Content |> Seq.toList |> should equal []


        transact (fun () ->
            
            set.Add (ASet.ofList [1;3;4]) |> ignore
            set.Add (ASet.single 2) |> ignore
        )

        for r in readers do
            r.GetDelta() |> should setEqual [Add 1; Add 2; Add 3; Add 4]
            r.Content |> Seq.sort |> Seq.toList |> should equal [1; 2; 3; 4]


        transact (fun () ->
            
            set.Add (ASet.ofList [5;6;7]) |> ignore
            set.Add (ASet.single 8) |> ignore
        )

        for r in readers do
            r.GetDelta() |> should setEqual [Add 5; Add 6; Add 7; Add 8]
            r.Content |> Seq.sort |> Seq.toList |> should equal [1; 2; 3; 4; 5; 6; 7; 8]





        ()


    [<Test>]
    let ``[ASet] concurrent readers``() =

        let l = obj()
        let set = CSet.empty
        let derived = set |> ASet.collect id
        let numbers = [0..9999]

        use cancel = new CancellationTokenSource()
        let ct = cancel.Token


        let readers = [0..5] |> List.map (fun _ -> derived.GetReader())
        // pull from the system

        for r in readers do
            Task.Factory.StartNew(fun () ->
                while true do
                    ct.ThrowIfCancellationRequested()
                    let delta = r.GetDelta()
                    if not (List.isEmpty delta) then
                        Console.WriteLine("delta: {0}", List.length delta)
                    //Thread.Sleep(1)
                    ()
            ) |> ignore


        // submit into the system
        for n in numbers do
            if n % 2 = 0 then
                let s = ASet.single n
                transact (fun () ->
                    lock l (fun () ->
                        set.Add(s) |> ignore
                        set.Add(ASet.single (n+1)) |> ignore
                    )
                )

        cancel.Cancel()

        for r in readers do
            r.Update()
            let content = r.Content |> Seq.toList |> List.sort
            content |> should equal numbers

    [<Test>]
    let ``[CSet] concurrent reader-reset``() =
        
        let set = CSet.ofList [1..100]

        let r = (set :> aset<_>).GetReader()
        let running = ref true

        Task.Factory.StartNew(fun () ->
            while !running do
                set.Clear() // causing reset
                set.UnionWith [1..100] // changing content
        ) |> ignore

        for i in 0..1000 do
            r.GetDelta() |> ignore

        running := false











    [<Test>]
    let ``[ASet] reader disposal``() =
        
        let input = CSet.ofList [1;2]
        let level0 = input |> ASet.map id |> ASet.choose Some |> ASet.collect ASet.single

        let a = level0 |> ASet.map id
        let b = level0 |> ASet.collect ASet.single

        level0.ReaderCount |> should equal 0
        input.Readers |> Seq.length |> should equal 0

        let ra = a.GetReader()
        level0.ReaderCount |> should equal 1
        input.Readers |> Seq.length |> should equal 1

        let rb = b.GetReader()
        level0.ReaderCount |> should equal 2
        input.Readers |> Seq.length |> should equal 1


        ra.Dispose()
        level0.ReaderCount |> should equal 1
        input.Readers |> Seq.length |> should equal 1


        rb.Dispose()
        level0.ReaderCount |> should equal 0
        input.Readers |> Seq.length |> should equal 0


        ()
        

    module GCHelper =

        let ``create, registerCallback and return and make sure that the stack frame dies``  () =
            let cset = CSet.ofList [ ]
            let mutable x = cset |> ASet.map ((*)2) |> ASet.filter ((>)(-1000)) |> ASet.groupBy id |> AMap.toASet
        
            let called = ref (-2)
            let y = ASet.unsafeRegisterCallbackNoGcRoot (fun _ -> called := !called + 1; ) x

            x <- Unchecked.defaultof<_>
            called, cset, y

        let ``create, registerCallbackAndKeepDisposable and return and make sure that the stack frame dies``  () =
            let cset = CSet.ofList [ ]
            let mutable x = cset |> ASet.map ((*)2) |> ASet.filter ((>)(-1000)) |> ASet.groupBy id |> AMap.toASet
        
            let called = ref (-2)
            let y = ASet.unsafeRegisterCallbackKeepDisposable (fun _ -> called := !called + 1; ) x

            x <- Unchecked.defaultof<_>
            called, cset

        let ``create, register marking and return and make sure that the stack frame dies`` () =
            let cset = CSet.ofList [ ]
            let mutable x = cset |> ASet.map ((*)2) |> ASet.filter ((>)(-1000)) |> ASet.groupBy id |> AMap.toASet
        
            let called = ref 0
            let reader = x.GetReader()
            let mutable y = reader.AddMarkingCallback (fun _ -> called := !called + 1; ) 

            y <- null
            x <- Unchecked.defaultof<_>
            called, cset, reader
            

    [<Test>]
    let ``[ASet] registerCallback holds no gc root``() =
        let called, inputSet, d = GCHelper.``create, registerCallback and return and make sure that the stack frame dies`` ()

        let cnt = 1000
        for i in 0 .. cnt do
            transact (fun () -> CSet.add i inputSet |> ignore)
            //printfn "should equal i=%d called=%d" i !called
            should equal  i !called
            Thread.Sleep 5
            if i % 100 = 0 then printfn "done: %d/%d" i cnt; GC.Collect()

        printfn "%A" d

    [<Test>]
    let ``[ASet] registerCallbackKeepDisposable holds gc root``() =
        let called, inputSet = GCHelper.``create, registerCallbackAndKeepDisposable and return and make sure that the stack frame dies`` ()

        let cnt = 1000
        for i in 0 .. cnt do
            transact (fun () -> CSet.add i inputSet |> ignore)
            //printfn "should equal i=%d called=%d" i !called
            should equal  i !called
            Thread.Sleep 5
            if i % 100 = 0 then printfn "done: %d/%d" i cnt; GC.Collect()

    [<Test>]
    let ``[ASet] markingCallback holds gc root``() =
        let called, inputSet, reader = GCHelper.``create, register marking and return and make sure that the stack frame dies`` ()

        let cnt = 1000
        for i in 0 .. cnt do
            transact (fun () -> CSet.add i inputSet |> ignore)
            //printfn "should equal i=%d called=%d" i !called
            reader.GetDelta() |> ignore
            should equal  i !called
            Thread.Sleep 5
            if i % 100 = 0 then printfn "done: %d/%d" i cnt; GC.Collect()


    [<Test>]
    let ``[ASet] async registerCallback``() =
        
        let inputSet = CSet.ofSeq [0; 1; 2; 3]
        let adaptive = inputSet |> ASet.map ((*)2)

        let mutable threadCount = 0
        let cnt = 1000
        for i in 0 .. cnt - 1 do
            Task.Factory.StartNew(fun () ->
                
                    //printfn "thread In";
                    let foo = adaptive |> ASet.unsafeRegisterCallbackNoGcRoot (fun d -> ())
                    //printfn "thread Out";

                    System.Threading.Interlocked.Increment(&threadCount) |> ignore
                    
                    ()
                ) |> ignore
        ()

        printfn "all threads started...."

        let maxWait = 10000
        let mutable waitCnt = 0
        while (threadCount <> cnt && waitCnt < maxWait) do
            //printfn "wating for threads to finish %d / %d" threadCount cnt
            waitCnt <- waitCnt + 1
            Thread.Sleep(10)

        let passed = threadCount = cnt || waitCnt < maxWait
        printfn "passed %A" passed

        should equal passed true

        ()
