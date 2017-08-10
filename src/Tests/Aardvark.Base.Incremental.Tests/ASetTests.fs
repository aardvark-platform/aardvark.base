namespace Aardvark.Base.Incremental.Tests

open System
open System.Threading
open System.Threading.Tasks
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open Aardvark.Base


[<AutoOpen>]
module ``utils`` =

    type Tree<'a> = Node of aset_check<Tree<'a>> | Leaf of aset_check<'a>

    type DeltaList<'a>() =
        let store = List<list<Delta<'a>>>()

        member x.push (deltas : list<Delta<'a>>) =
            store.Add deltas 

        member x.read() =
            let res = store |> Seq.toList
            store.Clear()
            res

module ``collect tests`` =

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
    
[<AutoOpen>]
module OtherASetTests =

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

        let s = derived |> ASet.unsafeRegisterCallbackKeepDisposable (fun delta ->
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
        let numbers = [0..1000]

        use sem = new SemaphoreSlim(0)
        use cancel = new CancellationTokenSource()
        let ct = cancel.Token


        let readers = [1..3] |> List.map (fun _ -> derived.GetReader())
        // pull from the system

        for r in readers do
            Task.Factory.StartNew(fun () ->
                while true do
                    ct.ThrowIfCancellationRequested()
                    let delta = r.GetDelta()
                    
                    //if not (List.isEmpty delta) then
                    //    Console.WriteLine("delta: {0}", List.length delta)
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


    type Leak(cnt : ref<int>) =
        do cnt := !cnt + 1
        override x.Finalize() = cnt := !cnt - 1

    [<Test>]
    let ``[ASet] memory leak test`` () =
        let mutable independenSource = Mod.init 10
        let cnt = ref 0
        let f () =
            let mutable leak = Leak(cnt)
            let normalSource = CSet.ofList [ leak ]
            let mapped = 
                normalSource 
                    |> ASet.map (fun l -> 
                        let m = independenSource |> Mod.map (fun i -> l)
                        Mod.force m |> ignore
                        m
                    ) 
            mapped |> ASet.toArray |> ignore
            transact (fun () -> normalSource |> CSet.remove leak |> should be True)
            leak <- Unchecked.defaultof<_>
        f()
        GC.Collect ()
        GC.WaitForPendingFinalizers()
        !cnt |> should equal 0  
        printfn "%A" independenSource 
        ()

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
        let readers = [1..3] |> List.map (fun _ -> derived.GetReader())
        let numbers = [0..1000]

        use sem = new SemaphoreSlim(0)
        use cancel = new CancellationTokenSource()
        let ct = cancel.Token


        for r in readers do
            // pull from the system
            Task.Factory.StartNew(fun () ->
                while true do
                    ct.ThrowIfCancellationRequested()
                    let delta = r.GetDelta()
                    Thread.Sleep(1)
                    ()
            , TaskCreationOptions.LongRunning) |> ignore


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
            , TaskCreationOptions.LongRunning) |> ignore

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


        let readers = [|0..5|] |> Array.map (fun _ -> derived.GetReader())
        // pull from the system

        let lists = Array.init readers.Length (fun _ -> System.Collections.Generic.List())


        for i in 0..readers.Length-1 do
            let r = readers.[i]
            let deltas = lists.[i]
            Task.Factory.StartNew(fun () ->
                while true do
                    ct.ThrowIfCancellationRequested()
                    let delta = r.GetDelta()


                    deltas.AddRange delta
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

        for i in 0..readers.Length-1 do
            let r = readers.[i]
            let deltas = lists.[i]
            r.GetDelta() |> deltas.AddRange

            let content = r.Content |> Seq.toList |> List.sort
            content |> should equal numbers

            let deltas = deltas |> Seq.toList |> List.sort
            deltas |> should equal (numbers |> List.map Add)

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
    let ``[CSet] delta elimination``() =
        
        let set = CSet.empty
        let r = (set :> aset<_>).GetReader()

        r.Update()

        transact (fun () -> set.Add 1 |> ignore; set.Remove 1 |> ignore)

        r.GetDelta() |> should equal []
        r.Content.Count |> should equal 0









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
        

    [<Test>]
    let ``[ASet] finalizers working``() =
        let input = CSet.ofList [1]

        let getDerivedReader(input : aset<'a>) =
            let r = input |> ASet.map id |> ASet.map id

            r.GetReader()
        let reader = getDerivedReader input
        //let reader = set.GetReader()

        System.GC.Collect()
        System.GC.WaitForFullGCComplete() |> ignore
        System.GC.Collect()
        System.GC.WaitForFullGCComplete() |> ignore

        reader.GetDelta() |> should setEqual [Add 1]
        reader.Dispose()


    let isPassThru (r : IReader<'a>) =
        match r with
            | :? ASetReaders.CopyReader<int> as r ->    
                r.PassThru
            | _ ->
                failwith "not a copy-reader"    


    [<Test>]
    let ``[ASet] reader creation after pull without change``() =
        let input = CSet.ofList [1;2]

        let derived = input |> ASet.map id


        let r0 = derived.GetReader()
        r0.GetDelta() |> should setEqual [Add 1; Add 2]

        r0 |> isPassThru |> should be True

        let r1 = derived.GetReader()

        r0 |> isPassThru |> should be False
        r1 |> isPassThru |> should be False


        r1.GetDelta() |> should setEqual [Add 1; Add 2]
        r0.GetDelta() |> should setEqual []

    [<Test>]
    let ``[ASet] reader creation after pull with change``() =
        let input = CSet.ofList [1;2]

        let derived = input |> ASet.map id


        let r0 = derived.GetReader()
        r0.GetDelta() |> should setEqual [Add 1; Add 2]
        r0 |> isPassThru |> should be True


        transact (fun () -> CSet.add 3 input |> ignore)
        let r1 = derived.GetReader()
        r0 |> isPassThru |> should be False
        r1 |> isPassThru |> should be False


        r1.GetDelta() |> should setEqual [Add 1; Add 2; Add 3]
        r0.GetDelta() |> should setEqual [Add 3]

    [<Test>]
    let ``[ASet] reader creation after pull dispose original``() =
        let input = CSet.ofList [1;2]

        let derived = input |> ASet.map id


        let r0 = derived.GetReader()
        r0.GetDelta() |> should setEqual [Add 1; Add 2]
        r0 |> isPassThru |> should be True


        transact (fun () -> CSet.add 3 input |> ignore)
        let r1 = derived.GetReader()
        r0 |> isPassThru |> should be False
        r1 |> isPassThru |> should be False

        r0.Dispose()
        r1 |> isPassThru |> should be True


        r1.GetDelta() |> should setEqual [Add 1; Add 2; Add 3]

    [<Test>]
    let ``[ASet] reader creation after pull dispose new one``() =
        let input = CSet.ofList [1;2]

        let derived = input |> ASet.map id


        let r0 = derived.GetReader()
        r0.GetDelta() |> should setEqual [Add 1; Add 2]
        r0 |> isPassThru |> should be True


        transact (fun () -> CSet.add 3 input |> ignore)
        let r1 = derived.GetReader()
        r0 |> isPassThru |> should be False
        r1 |> isPassThru |> should be False

        r1.Dispose()
        r0 |> isPassThru |> should be True
        r0.GetDelta() |> should setEqual [Add 3]

    [<Test>]
    let ``[ASet] reader modification/creation/disposal/pull``() =
        let input = CSet.empty
        let derived = input |> ASet.map id

        let mutable readers = []
        let random = Random()

        for i in 0..10000 do
            
            let op = random.Next(6)

            match op with
                | 0|1|2 ->
                    let r = derived.GetReader()
                    Interlocked.Change(&readers, fun l -> r::l) |> ignore
                | 3 ->
                    let v = random.Next()
                    transact (fun () -> CSet.add v input |> ignore)
                | 4 ->
                    let reader =
                        Interlocked.Change(&readers, fun l ->
                            match l with
                                | r::rest -> (rest, Some r)
                                | _ -> l, None
                        )

                    match reader with
                        | Some r -> r.Dispose()
                        | None -> ()
                | _ ->
                    match readers with
                        | [] -> ()
                        | _ ->
                            let r = readers |> List.item (random.Next(readers.Length)) 
                            let old = HashSet r.Content
                            let deltas = r.GetDelta()

                            for d in deltas do
                                match d with
                                    | Add v -> old.Add v |> ignore
                                    | Rem v -> old.Remove v |> ignore

                            old |> should setEqual r.Content
                            r.Content |> should setEqual input


            let rc = readers.Length
            if rc > 0 then
                input.Readers |> Seq.length |> should equal 1
            else
                input.Readers |> Seq.length |> should equal 0

            derived.ReaderCount |> should equal rc
            ()



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

        let cnt = 100
        for i in 0 .. cnt do
            transact (fun () -> CSet.add i inputSet |> ignore)
            //printfn "should equal i=%d called=%d" i !called
            !called |> should equal i
            Thread.Sleep 5
            if i % 10 = 0 then printfn "done: %d/%d" i cnt; GC.Collect()

        printfn "%A" d

    [<Test>]
    let ``[ASet] registerCallbackKeepDisposable holds gc root``() =
        let called, inputSet = GCHelper.``create, registerCallbackAndKeepDisposable and return and make sure that the stack frame dies`` ()

        let cnt = 100
        for i in 0 .. cnt do
            transact (fun () -> CSet.add i inputSet |> ignore)
            //printfn "should equal i=%d called=%d" i !called
            should equal  i !called
            Thread.Sleep 5
            if i % 10 = 0 then printfn "done: %d/%d" i cnt; GC.Collect()

    [<Test>]
    let ``[ASet] markingCallback holds gc root``() =
        let called, inputSet, reader = GCHelper.``create, register marking and return and make sure that the stack frame dies`` ()

        let cnt = 100
        for i in 0 .. cnt do
            transact (fun () -> CSet.add i inputSet |> ignore)
            //printfn "should equal i=%d called=%d" i !called
            reader.GetDelta() |> ignore
            //printfn "works? %A == %A" i !called
            should equal  i !called
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Thread.Sleep 5
            if i % 10 = 0 then printfn "done: %d/%d" i cnt; GC.Collect()
        printfn "done"


    [<Test>]
    let ``[ASet] Mod.async in asets is a memory leak?``() =
        
        let mutable t = Task.Factory.StartNew(fun () -> 
            System.Threading.Thread.Sleep 10
            1
        )

        let input = Mod.asyncTask t
        t <- null

        let reult = 
            aset {
                let! input = input
                match input with
                 | Some v -> 
                    yield 2
                 | None -> ()
            }
        GC.Collect ()
        GC.WaitForPendingFinalizers()
        printfn "ok"

    let private createLeakAndForgetAboutStackFrame () =
        let refCnt = ref 0
        let mutable l = Leak refCnt

        Mod.init l, refCnt

    [<Test>]
    let ``[ASet] ToMod is a memory leak due to inputs``() =

        // situation: all is leaky. If i remove input tracking
        // leaks get less but still in the end pendinging delta processing
        // holds on to things. 
        
        let mutable tup = createLeakAndForgetAboutStackFrame()
        let mutable spuriousInput = Weak(fst tup)
        let refCnt = snd tup
        let input = Mod.init (Some 1)

        let result = 
            aset {
                let! i = input
                match i with
                 | Some v -> 
                    let! si = spuriousInput.Target
                    yield i
                 | None -> ()
            } |> ASet.toMod
        result |> Mod.force |> ignore
        result |> Mod.unsafeRegisterCallbackKeepDisposable (printfn "%A") |> ignore
        tup <- Unchecked.defaultof<_>
        transact (fun () -> Mod.change input None)
        should equal  1 !refCnt
        //result |> Mod.force |> ignore // without forcing this test fails
        //transact (fun () -> spuriousInput.Target.Value <- Leak (ref 0)) // this does not help
        GC.Collect()
        GC.WaitForPendingFinalizers()
        printfn "now checkit "
        should equal  0 !refCnt


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

    [<Test>]
    let ``[ASet] Mod.async supports zombie mods`` () =
        
        let AwaitTaskVoid : (Task -> Async<unit>) =
            Async.AwaitIAsyncResult >> Async.Ignore

        let set = CSet.ofList [ 1 .. 100 ]

        let result =
            aset {
                for x in set :> aset<_> do
                    let! r = 
                        Mod.async <| 
                            async {
                                let! _ = AwaitTaskVoid <| Task.Delay 1
                                return x * 10
                            }
                    yield r
            }

        let result = ASet.map id result

        let resultReader = result.GetReader()

        let t = Task.Factory.StartNew(fun () ->
            for i in 101 .. 200 do
                transact (fun () -> CSet.add i set |> ignore)
                transact (fun () -> CSet.remove (i-100) set |> ignore)
                System.Threading.Thread.Sleep 2
                if i % 100 = 0 then resultReader.GetDelta() |> ignore
        )
        t.Wait()

        //result |> ASet.unsafeRegisterCallbackKeepDisposable (printfn "%A") |> ignore
        ()


    [<Test>]
    let ``[ASet] task bindings``() =
        
        let start = new ManualResetEventSlim()
        let rand = Random()
        let tasks =
            Array.init 100 (fun i ->
                Async.StartAsTask <| 
                    async {
                        let! _ =  Async.AwaitWaitHandle start.WaitHandle
                        let t = rand.Next(1000)
                        do! Async.Sleep t
                        return i
                    }
            )

        let sets =
            tasks |> Array.map (fun t ->
                aset {
                    let! v = t
                    yield v
                }
            )

        let all = sets |> ASet.union'
        let r = all.GetReader()
        start.Set()

        while r.Content.Count < 100 do
            let deltas = r.GetDelta()
            ()

        r.Content |> Seq.toList |> List.sort |> should equal [0..99]


module ASetPerformance =
    open System.Diagnostics


    let printfn fmt = Printf.kprintf (fun str -> Console.WriteLine("{0}", str)) fmt


    type IProbe =
        abstract member Start : unit -> unit
        abstract member Stop : unit -> unit
        
    [<Struct>]
    type DeltaTime(ticks : float) =
        static let ticksPerSecond = float TimeSpan.TicksPerSecond
        static let ticksPerMillisecond = float TimeSpan.TicksPerMillisecond
        static let ticksPerMicrosecond = ticksPerMillisecond / 1000.0

        member x.Ticks = ticks
        member x.Seconds = ticks / ticksPerSecond
        member x.Milliseconds = ticks / ticksPerMillisecond
        member x.Microseconds = ticks / ticksPerMicrosecond


        override x.ToString() =
            if x.Seconds >= 0.2 then sprintf "%.3fs" x.Seconds
            elif x.Milliseconds >= 2.0 then sprintf "%.3fms" x.Milliseconds
            else sprintf "%.1fµs" x.Microseconds

        static member (+) (l : DeltaTime, r : DeltaTime) =
            DeltaTime(l.Ticks + r.Ticks)

        static member (-) (l : DeltaTime, r : DeltaTime) =
            DeltaTime(l.Ticks - r.Ticks)

        static member (*) (l : DeltaTime, r : float) =
            DeltaTime(l.Ticks * r)

        static member (*) (l : float, r : DeltaTime) =
            DeltaTime(l * r.Ticks)

        static member (*) (l : DeltaTime, r : int) =
            DeltaTime(l.Ticks * float r)

        static member (*) (l : int, r : DeltaTime) =
            DeltaTime(float l * r.Ticks)

        static member (/) (l : DeltaTime, r : float) =
            DeltaTime(l.Ticks / r)

        static member (/) (l : DeltaTime, r : int) =
            DeltaTime(l.Ticks / float r)

    type TimeProbe() =
        let sw = Stopwatch()

        member x.Start() = sw.Start()
        member x.Stop() = sw.Stop()
        member x.Reset() = sw.Reset()
        
        member x.Elapsed = DeltaTime(float sw.Elapsed.Ticks)


        interface IProbe with
            member x.Start() = x.Start()
            member x.Stop() = x.Stop()

    module Probe =
        let private current = new ThreadLocal<Option<IProbe>>(fun _ -> None)

        let time() = TimeProbe()

        let using (f : unit -> 'a) (x : IProbe) =
            let old =
                match current.Value with
                    | Some c -> 
                        c.Stop()
                        Some c
                    | _ -> None
            try
                current.Value <- Some x
                x.Start()
                f()
            finally
                x.Stop()
                match old with
                    | Some o -> 
                        current.Value <- Some o
                        o.Start()
                    | _ ->
                        current.Value <- None



    [<Test>]
    let ``[ASet Performance] map``() =
        
        for s in 1000..1000..10000 do
            System.GC.Collect()
            System.GC.WaitForFullGCApproach() |> ignore
        
            printfn "size: %d" s
            let tTransact = Probe.time()
            let tSubmit = Probe.time()
            let tPull = Probe.time()


            let input = CSet.empty
            let mapped = input |> ASet.map id
            let r = mapped.GetReader()

            transact (fun () ->
                CSet.add 0 input |> ignore
            )
            r.Update()

            let cnt = 10000
            for i in 1..cnt do
                tTransact |> Probe.using (fun () ->
                    transact (fun () ->
                        tSubmit |> Probe.using (fun () ->
                            CSet.add i input |> ignore
                        )
                    )
                )

                tPull |> Probe.using (fun _ ->
                    r.Update()
                )

            printfn "  submit:   %A" (tSubmit.Elapsed / cnt)
            printfn "  transact: %A" (tTransact.Elapsed / cnt)
            printfn "  pull:     %A" (tPull.Elapsed / cnt)


    [<Test>]
    let ``[ASet Performance] collect``() =
        
        for s in 1000..1000..10000 do
            System.GC.Collect()
            System.GC.WaitForFullGCApproach() |> ignore
        
            printfn "size: %d" s
            let tTransact = Probe.time()
            let tSubmit = Probe.time()
            let tPull = Probe.time()


            let input = CSet.empty
            let mapped = input |> ASet.collect ASet.single
            let r = mapped.GetReader()

            transact (fun () ->
                CSet.add 0 input |> ignore
            )
            r.Update()

            let cnt = 10000
            for i in 1..cnt do
                tTransact |> Probe.using (fun () ->
                    transact (fun () ->
                        tSubmit |> Probe.using (fun () ->
                            CSet.add i input |> ignore
                        )
                    )
                )

                tPull |> Probe.using (fun _ ->
                    r.Update()
                )

            printfn "  submit:   %A" (tSubmit.Elapsed / cnt)
            printfn "  transact: %A" (tTransact.Elapsed / cnt)
            printfn "  pull:     %A" (tPull.Elapsed / cnt)
    

    [<Test>]
    let ``[ASet] bind caching``() =

        let set = cset [1;2;3;4;5;6;7;8;9]

        let sel = set |> ASet.map (fun x -> printfn "first map: %A" x; x * 2)

        let modul = Mod.init 3
        
        let tes = ASet.filter (fun n -> (n % 2) = 0) set

        let b = modul |> ASet.bind (fun m -> sel |> ASet.filter (fun n -> (n % m) = 0))
                
        let b2 = b |> ASet.map (fun n -> printfn "second map: %A" n; n * 3)

        let o = ASet.toMod b2

        printfn "initial"

        let force = o.GetValue()

        printfn "change modul"

        transact (fun () -> Mod.change modul 2)

        let force = o.GetValue()

        printfn "done"

        ()

    [<Test>]
    let ``[ASet] bind2``() =

        let setA = cset [1;2;3;4;5]
        let setB = cset [1;2]

        let setAMod = setA |> ASet.toMod
        let setBMod = setB |> ASet.toMod

        let filterAbyBSet = ASet.bind2 (fun (a : IVersionedSet<int>) (b : IVersionedSet<int>) -> a |> Seq.filter (fun x -> not (b.Contains(x))) |> ASet.ofSeq) setAMod setBMod

        printfn "initial"

        let reader = filterAbyBSet.GetReader()
        let force = filterAbyBSet |> ASet.toArray
        let ref = setA |> Seq.filter (fun x -> not (setB.Contains(x))) |> Seq.toArray
        force |> should setEqual ref
        //reader.GetDelta() |> should setEqual [Add 3; Add 4; Add 5]

        printfn "first"

        transact (fun () -> CSet.add 5 setB |> ignore)
        let force = filterAbyBSet |> ASet.toArray
        let ref = setA |> Seq.filter (fun x -> not (setB.Contains(x))) |> Seq.toArray
        force |> should setEqual ref
        //reader.GetDelta() |> should setEqual [Rem 5]

        printfn "second"

        transact (fun () -> CSet.add 10 setA |> ignore)
        let force = filterAbyBSet |> ASet.toArray
        let ref = setA |> Seq.filter (fun x -> not (setB.Contains(x))) |> Seq.toArray
        force |> should setEqual ref
        //reader.GetDelta() |> should setEqual [Add 10]

        printfn "passed"

        ()

[<AutoOpen>]
module ConcurrentDeltaQueueTests =

    let private compilerTest () =
        let r = System.Random()
        let livingThings = System.Collections.Generic.List<_>()
        let mutable freshIds = 0L
        for i in 0 .. 100000 do
            let operations = System.Collections.Generic.List<Delta<int64>>()
            let newThings = 
                [ for i in 0 .. r.Next(0,20) do 
                        freshIds <- freshIds + 1L
                        livingThings.Add freshIds |> ignore
                        operations.Add (Add freshIds)
                ]
            for i in operations do printfn "%A" i

        printfn "okay"


    [<Test>]
    let ``[ASet ConcurrentDeltaQueue] concurrent delta queue test``() =

        
        compilerTest()
        let set = CSet.empty

        let queue = new ConcurrentDeltaQueue<int64>()

        let r = System.Random()
        let mutable freshIds = 0L
        let livingThings = System.Collections.Generic.HashSet<int64>()

        let perIteration = 20

        let producer () =
            for i in 0 .. 100000 do
                let operations = List<Delta<int64>>()

                for i in 0 .. r.Next(0,perIteration) do
                    let arr = livingThings |> Seq.toArray
                    if arr.Length > 100 then
                        let toRemove = arr.[r.Next(0,arr.Length-1)]
                        livingThings.Remove toRemove |> ignore
                        operations.Add (Rem toRemove)
                    
                let newThings = 
                    [| for i in 0 .. r.Next(0,perIteration) do 
                        freshIds <- freshIds + 1L
                        livingThings.Add freshIds |> ignore
                        operations.Add (Add freshIds)
                        yield freshIds
                    |]


                for i in operations do queue.Enqueue i |> ignore

            for x in livingThings |> Seq.toArray do 
                livingThings.Remove x |> ignore
                queue.Enqueue (Rem x) |> ignore


        let conc = System.Collections.Concurrent.ConcurrentHashSet()

        let cts = new System.Threading.CancellationTokenSource()

        let consumer  =
            async {
                do! Async.SwitchToNewThread()

                while true do 
                    let! i = queue.DequeueAsync()
                    match i with
                        | Add v -> if conc.Add( v) then () else failwith "duplicate!?!?!?"
                        | Rem v -> if conc.Remove v then () else failwith "Rem of non existing?!?!?!"
            }

        //let arrs = [ for i in 0 .. 8 do yield Async.StartAsTask( consumer,cancellationToken = cts.Token) ]

        producer()
        cts.Cancel()
