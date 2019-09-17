namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open System.Threading
open System.Threading.Tasks
open System.Runtime.CompilerServices
open System.Diagnostics

module ``Basic Mod Tests`` =
    open System.Collections.Generic
    open System.Diagnostics

    type ExecutionTracker() =
        let ops = List<string>()

        member x.push fmt =
            Printf.kprintf (fun str -> ops.Add str) fmt

        member x.read() =
            let l = ops |> Seq.toList
            ops.Clear()
            l
    
    [<Test>]
    let ``[Mod] basic map test``() =
        let cell = Mod.init 1

        let derived = cell |> Mod.map (fun a -> 2 * a)

        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 2

        transact (fun () ->
            Mod.change cell 2
        )

        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 4

    [<Test>]
    let ``[Mod] constant map test``() =
        let cell = Mod.constant 1

        let derived = cell |> Mod.map (fun a -> 2 * a)

        derived.IsConstant |> should equal true
        derived |> Mod.force |> should equal 2


    [<Test>]
    let ``[Mod] basic map2 test``() =
        let cell1 = Mod.init 1
        let cell2 = Mod.init 1

        let derived = Mod.map2 (fun a b -> a + b) cell1 cell2

        derived.IsConstant |> should equal false
        derived |> Mod.force |> should equal 2

        transact (fun () ->
            Mod.change cell1 2
            Mod.change cell2 3
        )

        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 5

        transact (fun () ->
            Mod.change cell1 1
        )

        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 4

        transact (fun () ->
            Mod.change cell2 2
        )

        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 3

    [<Test>]
    let ``[Mod] constant map2 test``() =
        let c1 = Mod.constant 1
        let c2 = Mod.constant 1
        let m1 = Mod.init 1
        let m2 = Mod.init 1

        let derivedc = Mod.map2 (fun a b -> a + b) c1 c2
        let derived1 = Mod.map2 (fun a b -> a + b) m1 c2
        let derived2 = Mod.map2 (fun a b -> a + b) c1 m2

        derivedc.IsConstant |> should equal true
        derivedc |> Mod.force |> should equal 2

        derived1.IsConstant |> should equal false
        derived1 |> Mod.force |> should equal 2

        derived2.IsConstant |> should equal false
        derived2 |> Mod.force |> should equal 2


        transact (fun () ->
            Mod.change m1 2
        )
        derived1.OutOfDate |> should equal true
        derived1 |> Mod.force |> should equal 3


        transact (fun () ->
            Mod.change m2 2
        )
        derived2.OutOfDate |> should equal true
        derived2 |> Mod.force |> should equal 3

    [<Test>]
    let ``[Mod] transaction with onPush``() =
        let a0 = Mod.init 0
        let a1 = a0 |> Mod.onPush

        let b0 = Mod.init 1
        let b1 = b0 |> Mod.map (fun x -> 
            transact (fun () -> Mod.change a0 10)
            x + a1.GetValue())
        let b2 = b1 |> Mod.map (fun x -> x + 1 )
        let b3 = b2 |> Mod.map (fun x -> x + 1)

        b3.GetValue() |> should equal 13

    [<Test>]
    let ``[Mod] basic bind test``() =
        let cell1 = Mod.init true
        let cell2 = Mod.init 2
        let cell3 = Mod.init 3

        let derived =
            Mod.bind (fun v -> if v then cell2 else cell3) cell1

        derived.IsConstant |> should equal false
        derived |> Mod.force |> should equal 2

        transact (fun () ->
            Mod.change cell1 false
        )
        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 3

        transact (fun () ->
            Mod.change cell2 5
        )
        derived.OutOfDate |> should equal false


        transact (fun () ->
            Mod.change cell3 100
        )
        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 100

        transact (fun () ->
            Mod.change cell1 true
        )
        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 5

    [<Test>]
    [<Ignore("eager evaluation broken atm.")>]
    let ``[Mod] level changing bind``() =
        let ex = ExecutionTracker()
        let a = ModRef 1 
        let a0 = a :> IMod<_>
        let a1 = a |> Mod.map id |> Mod.map id |> Mod.map id |> Mod.map (fun a -> ex.push "%A * 2" a; a * 2)
        let c = ModRef true

        let res = 
            c |> Mod.bind (fun c ->
                ex.push "bind: %A" c
                if c then a0
                else a1
            ) |> Mod.onPush

        let res = res |> Mod.map (fun a -> ex.push "cont"; a)// |> Mod.always

        let s = res |> Mod.unsafeRegisterCallbackNoGcRoot (fun v -> printfn "cb: %A" v)

        res.GetValue() |> should equal 1
        ex.read() |> should equal ["bind: true"; "cont"]

        transact(fun () ->
            a.Value <- 10
            c.Value <- false
        )

        res.GetValue() |> should equal 20
        
        let read = ex.read()
        read |> should equal ["bind: false"; "10 * 2"; "cont"]

        s.Dispose()

        transact(fun () ->
            a.Value <- 20
            c.Value <- true
        )
        res.GetValue() |> should equal 20
        ex.read() |> should equal ["bind: true"]


        ()

    [<Test>]
    [<Ignore("eager evaluation broken atm.")>]
    let ``[Mod] bind in bind``() =
        let a = Mod.init false
        let b = Mod.init 10
        let c =
            adaptive {
                let! a = a 
                if a then
                    let! b = b 
                    return b
                else
                    return 0
            }

        let c = c |> Mod.onPush

        c.GetValue() |> should equal 0

        transact(fun () -> Mod.change a true)
        c.GetValue() |> should equal 10

        transact(fun () -> Mod.change a false)
        c.GetValue() |> should equal 0


    [<Test>]
    [<Ignore("takes way too long")>]
    let ``[Mod] mod concurrency test``() =
        
        let pulledValues = List<int>()

        let input = Mod.init 1

        let derived = input |> Mod.map id //|> Mod.map id 

        let sem = new SemaphoreSlim(0)
        let trigger = new ManualResetEventSlim()
        for t in 1..1000 do
            Task.Factory.StartNew(fun () ->
                trigger.Wait()
                let v = derived |> Mod.force

                lock pulledValues (fun () -> pulledValues.Add v)

                sem.Release() |> ignore
            ) |> ignore

        trigger.Set()

        for i in 1..1000 do
            sem.Wait()


        let values = pulledValues |> Seq.countBy id |> Map.ofSeq
        System.Console.WriteLine("{0}", sprintf "%A" values)
        values |> Map.toSeq |> Seq.map fst |> should equal [1]
        

    [<Test>]
    let ``[Mod] fan out, different eval strategy``() =

        let input = Mod.init 10

        let middle = input |> Mod.map id
        let outputA = middle |> Mod.map id
        let outputB = middle |> Mod.map id
        let mutable marked = false
        let v = outputB.AddMarkingCallback(fun () -> marked <- true)

        outputB |> Mod.force |> ignore // comment and reactivate the one after transact
        transact (fun () -> Mod.change input 11)
        //outputB |> Mod.force |> ignore

        outputA |> Mod.force |> should equal 11
        marked |> should equal true
        v.Dispose()


    [<Test>]
    [<Ignore("takes way too long")>]
    let ``[Mod] mod concurrency test super crazy callbacks``() =
        
        let pulledValues = List<int>()

        let input = Mod.init 1
        let middle = Mod.init 1

        let derived = middle |> Mod.map id |> Mod.map (fun s -> Thread.Sleep 5; s) 

        let maxCnt = 400
        let mutable abort = false
        let mutable currentValue = 0
        let inputThread = System.Threading.Thread(ThreadStart(fun _ ->
            for i in 1 ..maxCnt do
                let newValue = Interlocked.Increment(&currentValue)
                transact (fun () -> Mod.change input newValue)
            abort <- true
        ))

        let mutable marked = 0
        let d = derived.AddMarkingCallback (fun _ -> Interlocked.Increment(&marked) |> ignore)

        let collection = new System.Collections.Concurrent.BlockingCollection<int>()

        let middle = System.Threading.Thread(ThreadStart(fun _ ->
            let result = ref Unchecked.defaultof<_>
            while collection.Count > 0 || not abort do
                if collection.TryTake(result, 10) then
                    transact (fun () -> Mod.change middle !result)
        ))

        input |> Mod.unsafeRegisterCallbackKeepDisposable (fun v ->
             collection.Add v |> ignore
        ) |> ignore

        let mutable result = 0
        let resultCb = derived |>  Mod.unsafeRegisterCallbackKeepDisposable (fun v ->
            result <- v
        )

        let mutable r = 0
        let puller = System.Threading.Thread(ThreadStart(fun _ ->
            while not abort do
                r <- derived |> Mod.force
        ))


        puller.Start()
        middle.Start()
        inputThread.Start()

        inputThread.Join()
        middle.Join()
        puller.Join()

        //collection.Count |> should equal 0
        printfn "awaited"
        result |> should equal currentValue
        derived |> Mod.force |> should equal currentValue
        printfn "done"


    [<Test>]
    let ``[Mod] DefaultingModRef can be set`` () =

        let source = Mod.init 10
        let x = Mod.initDefault source

        x |> Mod.force |> should equal 10

        transact (fun () -> Mod.change source 5)
        x |> Mod.force |> should equal 5

        transact (fun () -> Mod.change x 100)
        x |> Mod.force |> should equal 100

        transact (fun () -> Mod.change source 7)

        x.OutOfDate |> should be False
        x |> Mod.force |> should equal 100

        transact (fun () -> x.Reset())
        x |> Mod.force |> should equal 7



    [<Test>]
    let ``[Mod] bind bind`` () =

        let otherInner = Mod.init 3
        let inner = Mod.init 10
        let x = Mod.init inner

        let r = Mod.bind id x

        r |> Mod.force |> should equal 10

        transact (fun () -> Mod.change inner 11)
        transact (fun () -> Mod.change x otherInner)

        r |> Mod.force |> should equal 3


//    [<Test>]
//    let ``[VolatileCollection] memory test`` () =
//        let count = 10000
//        let arr = Array.zeroCreate count
//        let objects = Array.init count (fun _ -> obj())
//
//        let before = System.GC.GetTotalMemory(true)
//
//        for i in 0..count-1 do
//            arr.[i] <- VolatileCollection()
//            ignore (arr.[i].Add(objects.[i]))
//
//        let after = System.GC.GetTotalMemory(true)
//
//        let mem = after - before
//        let perInstance = float mem / float count
//        System.Console.WriteLine("real:      {0}", perInstance)
//
//        let dummy = arr |> Array.exists (fun c -> c.IsEmpty)
//        if dummy then
//            printfn "asdlkasndksajmdlkasmdl"

    [<Test>]
    let ``[Mod] deep hierarchy test`` () =

        let root = Mod.init (Trafo3d.Translation(1.0, 2.0, 3.0))

        let rec buildTree (m, l, c) =
            
            [for i in 1..c do
                let x = Mod.init (Trafo3d.RotationX 360.0)
                let foo = Mod.map2 ((*)) m x 

                if l < 2 then
                    yield foo
                else
                    yield! buildTree(foo, l - 1, c)
            ]

        let tree = buildTree(root, 5, 5)

        let sw = System.Diagnostics.Stopwatch()
        
        Log.line "LeafCount=%A" tree.Length

        for j in 1..4 do
            
            sw.Restart()
            transact(fun () -> 
                root.Value <- Trafo3d.RotationX (float j)
                )
            sw.Stop()
            Log.line "marking:  %As" sw.Elapsed.TotalSeconds

            sw.Restart()
            for leaf in tree do
                leaf.GetValue() |> ignore
            sw.Stop()
            Log.line "evaluate:  %As" sw.Elapsed.TotalSeconds
            
        ()

        
    [<Test>]
    let ``[Mod] mapN test``() =
        
        let mods = Array.init 10 (fun i -> Mod.init i)

        let sum = mods |> Mod.mapN Seq.sum
        sum |> Mod.force |> should equal 45

        transact (fun () -> Mod.change (mods.[0]) 100)
        sum |> Mod.force |> should equal 145

        transact (fun () -> Mod.change (mods.[5]) 100)
        sum |> Mod.force |> should equal 240


    [<Test>]
    [<Ignore("takes way too long")>]
    let ``[Mod] consistent concurrency test``() =
        let i = Mod.init 10

        let a = i |> Mod.map id |> Mod.map id 
        let b = i |> Mod.map (fun a -> -a)
        
        let mutable values = 0
        let apb = 
            Mod.map2 (fun a b -> 
                Interlocked.Increment(&values) |> ignore
                a + b
            ) a b
        apb |> Mod.force |> ignore

        values <- 0
        let pullers = 1
        let changers = 1

        let countdown = new CountdownEvent(pullers)
        let mutable iterations = 0
        let mutable changes = 0
        let mutable exn = None


        let r = System.Random()
        let changer = 
            async {
                do! Async.SwitchToNewThread()

                do
                    while true do
                        transact (fun () -> Mod.change i (r.Next()))
                        Interlocked.Increment &changes |> ignore
            }


        let puller =
            async {
                let sw = System.Diagnostics.Stopwatch()
                sw.Start()
                do! Async.SwitchToNewThread()
                do
                    try
                        while sw.Elapsed.TotalSeconds < 10.0 do
                            if apb.OutOfDate then
                                let r = Mod.force apb
                                if r <> 0 then 
                                    exn <- Some (sprintf "hate :%d" iterations)
                                    failwithf "hate :%d" iterations
                                Interlocked.Increment(&iterations) |> ignore
                    finally
                        countdown.Signal() |> ignore
            }


        for i in 1 .. changers do 
            Async.Start changer

        for i in 1 .. pullers do 
            Async.Start puller


        countdown.Wait()

        match exn with
            | Some e -> failwith e
            | None -> ()

//        let sw = System.Diagnostics.Stopwatch()
//        sw.Start()
//        while sw.Elapsed.TotalSeconds < 10.0 do
//            let r = Mod.force apb
//            if r <> 0 then failwithf "hate :%d" iterations
//            iterations <- iterations + 1
//            if iterations % 10000 = 0 then
//                let progress = sw.Elapsed.TotalSeconds / 10.0
//                printfn "%.2f%%" (100.0 * progress)

        printfn "values: %A" values
        printfn "done: %A" iterations
        printfn "changes: %A" changes
    
    [<Test>]
    [<Ignore("takes way too long")>]
    let ``[Mod] consistent concurrency dirty set``() =
        //let i = Mod.init 10

        let set = CSet.empty
        let v = set |> ASet.toMod |> Mod.map (fun s -> s.Count)

        let a = v |> Mod.map id |> Mod.map id 
        let b = v |> Mod.map (fun a -> -a)

        let r = System.Random()
        let changer = 
            async {
                do! Async.SwitchToNewThread()
                while true do
                    //do! Async.Sleep 0
                    transact (fun () -> 
                        set.Add (r.Next()) |> ignore
                        //Mod.change i (r.Next())
                    )
            }



        for i in 0 .. 3 do 
            Async.Start changer

        let mutable markings = 0
        let ab =
            let scratch = Dict<obj, HashSet<IMod<int>>>()
            let dirty = HashSet<IMod<int>> [a;b]
            let mutable initial = true
            { new Mod.AbstractDirtyTrackingMod<IMod<int>, int>() with
                member x.Compute(token, dirty) =
                    if initial then
                        a.GetValue(token) |> ignore
                        b.GetValue(token) |> ignore
                        2
                    else
                        let cnt = dirty.Count
                        for d in dirty do d.GetValue(token) |> ignore
                        dirty.Count
            }
//
//            { new Mod.AbstractMod<int>() with
//                override x.InputChanged(t,i) =
//                    match i with
//                        | :? IMod<int> as i ->
//                            lock scratch (fun () ->
//                                let set = scratch.GetOrCreate(t, fun t -> HashSet())
//                                set.Add i |> ignore
//                            )
//                        | _ -> ()
//
//                override x.AllInputsProcessed(t) =
//                    markings <- markings + 1
//                    match lock scratch (fun () -> scratch.TryRemove t) with
//                        | (true, s) -> dirty.UnionWith s
//                        | _ -> ()
//
//                override x.Compute() =
//                    let cnt = dirty.Count
//                    for d in dirty do d.GetValue(x) |> ignore
//                    dirty.Clear()
//                    cnt
//            }

        let sw = System.Diagnostics.Stopwatch()
        let mutable iterations = 0
        sw.Start()
        while sw.Elapsed.TotalSeconds < 10.0 do
            //Thread.Sleep(1)
            let r = Mod.force ab
            if r <> 2 then failwithf "hate : %d/%d" r iterations
            iterations <- iterations + 1

        printfn "markings: %A" markings
        printfn "evals:    %A" iterations

    [<Test>]
    [<Ignore("takes way too long")>]
    let ``[Mod] consistent concurrency multiple evaluators test``() =
        let i = Mod.init 10

        let a = i |> Mod.map id |> Mod.map id 
        let b = i |> Mod.map (fun a -> -a)

        let apb = Mod.map2 (+) a b

        let r = System.Random()

        let mutable badResults = 0
        let mutable evaluations = 0
        let mutable changes = 0
        let mutable running = true

        let changers = 20
        let evaluators = 5
        let sem = new SemaphoreSlim(0)

        let changer = 
            async {
                do! Async.SwitchToNewThread()
                while true do
                    //do! Async.Sleep 0
                    transact (fun () -> Mod.change i (r.Next()))
                    Interlocked.Increment &changes |> ignore
            }

        let evaluator =
            async {
                do! Async.SwitchToNewThread()
                while Volatile.Read(&running) do
                    let r = Mod.force apb
                    Interlocked.Increment &evaluations |> ignore
                    if r <> 0 then Interlocked.Increment(&badResults) |> ignore
                sem.Release() |> ignore
            }

        for i in 1 .. changers do 
            Async.Start changer

        for i in 1 .. evaluators do 
            Async.Start evaluator



        let sw = System.Diagnostics.Stopwatch()
        sw.Start()
        while sw.Elapsed.TotalSeconds < 10.0 do
            Thread.Sleep(500)
            let progress = sw.Elapsed.TotalSeconds / 10.0 |> clamp 0.0 1.0
            printfn "%.2f%%" (100.0 * progress)

            if badResults > 0 then
                failwithf "hate: %A" badResults

        running <- false
        for i in 1..evaluators do sem.Wait()
        
        printfn "evaluations: %A" evaluations
        printfn "changes:     %A" changes
        if badResults > 0 then
            failwithf "hate: %A" badResults
             
        printfn "done"

    [<Test>]
    [<Ignore("eager evaluation broken atm.")>]
    let ``[Mod] eager mod trigger test``() =
        let a = Mod.init 1
        let b = Mod.onPushCustomEq (fun x y -> y < 5) a
        
        let mutable count = 0
        b |> Mod.unsafeRegisterCallbackKeepDisposable (fun v -> count <- count + 1; printfn "%A" v) |> ignore

        should equal count 1

        transact(fun () ->
            Mod.change a 2)

        should equal count 1
                
        transact(fun () ->
            Mod.change a 3)

        should equal count 1

        transact(fun () ->
            Mod.change a 4)

        should equal count 1

        transact(fun () ->
            Mod.change a 5)

        should equal count 2

        ()


    [<Test>]
    [<Ignore("throwing exception during evaluation will result in deadlock atm.")>]
    let ``[Mod] exception during evaluation`` () =

        let input = Mod.init 10
        let input2 = Mod.init 2
        
        let res = input |> Mod.map (fun v -> if (v &&& 0x1) <> 0 then failwith "does not divide by 2" else v >>> 1)
        //let res = res |> Mod.map (fun v -> v * 2)
        
        transact(fun () -> Mod.change input 6)
        try
            printfn "%d" (Mod.force res) 
        with e -> printfn "%A" e

        transact(fun () -> Mod.change input 5)
        try
            printfn "%d" (Mod.force res)  // evaluation will throw exception -> AdaptiveObject locks are not release
        with e -> printfn "%A" e

        transact(fun () -> Mod.change input 4) // marking will block as AdaptiveObject locks have not been release due to exception
        try
            printfn "%d" (Mod.force res) 
        with e -> printfn "%A" e
        

    [<Test>]
    [<Ignore("takes way too long")>]
    let ``[Mod] parallel consistent concurrency test``() =
        let a = Mod.init 10
        let b = Mod.init -10

        let a' = a |> Mod.map id |> Mod.map id 
        let b' = b :> IMod<_>

        let apb = Mod.map2 (+) a' b'

        let r = System.Random()
        let changer = 
            async {
                do! Async.SwitchToNewThread()
                while true do
                    //do! Async.Sleep 0
                    let v = r.Next()

                    transact (fun () -> 
                        Mod.change a v
                        Mod.change b -v
                    )
            }



        for i in 0 .. 3 do 
            Async.Start changer

        let mutable finished = false
        try
            let sw = System.Diagnostics.Stopwatch()
            let mutable iterations = 0
            sw.Start()
            while sw.Elapsed.TotalSeconds < 10.0 do
                let r = Mod.force apb
                if r <> 0 then failwithf "hate :%d" iterations
                iterations <- iterations + 1
            finished <- true
        with _ ->
            ()

        if finished then failwith "that was unexpected"


    [<Test>]
    let ``[Mod] recursive transact`` () =

        let x = Mod.init 10
        let y = Mod.init 20

        let cb = x |> Mod.unsafeRegisterCallbackNoGcRoot (fun v ->
                        transact(fun () ->
                            Mod.change y v))

        transact(fun () ->
            Mod.change x 5)

        let t = getCurrentTransaction()

        t.IsNone |> should be True
        y |> Mod.force |> should equal 5

        let cb2 = x |> Mod.unsafeRegisterCallbackNoGcRoot (fun v ->
                        using(Aardvark.Base.Incremental.CSharp.Adaptive.Transaction) (fun _ -> Mod.change y v)
                    )

        using(Aardvark.Base.Incremental.CSharp.Adaptive.Transaction) (fun _ -> Mod.change x 3)

        let t = getCurrentTransaction()

        t.IsNone |> should be True
        y |> Mod.force |> should equal 3

    [<Test>]
    let ``[Mod] eval and mark performance`` () =

        let mutable garbage = [ ]

        for j in 0..0 do // loop used for repeated testing in standalone executable
            let mutable totalMarkTime = 0.0
            let mutable totalEvalTime = 0.0
            let iter = 1
            for k in 1..iter do
                //let root = Mod.init Trafo3d.Identity
                let root = Mod.init 10
                let mutable leafs = [ root :> IMod<_> ]
                let rnd = RandomSystem(1)

                // configuration 1: average of 2-3 outputs, less depth
                let depth = int (10.0 * pow 1.1 (float k))
                let stdWidth = 1.2

                // configuration 2: average 1-2 outputs, deeper tree
                //let depth = int (10.0 * pow 1.5 (float k))
                //let stdWidth = 0.3

                let mutable nodeCount = 0
                //Log.startTimed "Create Tree: Depth=%d StdWidth=%.2f" depth stdWidth
                for i in 1..depth do
                    let mutable nextLevel = [ ]
                    for l in leafs do
                        let subCnt = rnd.Gaussian(0.0, stdWidth)
                        let refine = (abs subCnt) |> int
                        for r in 0..refine do
                            let n = l |> Mod.map (fun x -> x * 2)
                            //let xx = Mod.init Trafo3d.Identity
                            //let n = Mod.map2 (fun a b -> a * b) xx l
                            nextLevel <- nextLevel @ [n]
                            nodeCount <- nodeCount + 1 
                        ()
                    leafs <- nextLevel
                //Log.stop ""

                let leafCount = List.length leafs
                let runs = 10 + 2000000 / nodeCount 
                let swMark = Stopwatch()
                let swEval = Stopwatch()
            
                //Log.start "Depth=%-3d StdWidth=%.2f Nodes/Leafs=%-11s" depth stdWidth (sprintf "%d/%d" nodeCount leafCount)
                for i in 1..runs do
                    if i > 5 then swMark.Start()
                    //transact(fun () -> Mod.change root (Trafo3d.Translation(V3d(i, i, i))))
                    transact(fun () -> Mod.change root i)
                    if i > 5 then swMark.Stop()

                    if i > 5 then swEval.Start()
                    for l in leafs do
                        l |> Mod.force |> ignore
                    if i > 5 then swEval.Stop()

                let markTime = (float swMark.ElapsedTicks / float Stopwatch.Frequency)
                let evalTime = (float swEval.ElapsedTicks / float Stopwatch.Frequency)
                //Log.stop " MarkTime=%.2fs EvalTime=%.2fs" markTime evalTime

                totalMarkTime <- totalMarkTime + markTime
                totalEvalTime <- totalEvalTime + evalTime
            
                garbage <- garbage @ leafs

            Log.line "Total: MarkTime=%.2fs EvalTime=%.2fs" totalMarkTime totalEvalTime
        
    [<Test>]
    let ``[AdaptiveObject] callback dispose test``() =

        let a = Mod.init true
        let b = Mod.init 1

        let mutable registered = false
        let mutable rx = Unchecked.defaultof<System.IDisposable>
        let cb1 = a |> Mod.unsafeRegisterCallbackNoGcRoot (fun x -> 
                            Log.line "outer Callback %A" x
                            if rx <> null then
                                rx.Dispose()
                                rx <- null
                                registered <- false

                            if x then
                                registered <- true
                                rx <- b |> Mod.unsafeRegisterCallbackNoGcRoot(fun x -> Log.line "inner Callback %d" x; if not registered then failwith "invoked disposed callback")
                        )

        transact(fun () ->
            b.Value <- 2)
        
        transact(fun () ->
            a.Value <- false)

        transact(fun () ->
            a.Value <- true)

        // this transaction will push both callbacks to marking queue  
        // the evaluation will first trigger the outer callback then disposes the inner
        // as it is already on the marking queue, it will be still invoked -> this should not happen
        transact(fun () ->
            a.Value <- false
            b.Value <- 3)

        ()

    [<AutoOpen>]
    module Validation =
        open Aardvark.Base.Incremental.Validation
        [<Test>]
        let ``[Mod] DotSerialization`` () =
            let x = Mod.init 10
            let y = Mod.init 20
            let z = Mod.map2 ((+)) x y
            let path = System.IO.Path.GetTempFileName()
            z.DumpDotFile (1000, path) |> ignore

        [<Test>]
        let ``[Mod] DgmlSerialization`` () =
            let x = Mod.init 10
            let y = Mod.init 20
            let z = Mod.map2 ((+)) x y
            let path = System.IO.Path.GetTempFileName()
            z.DumpDgml (1000, path) |> ignore

            
    type RuntimeObject(name : string) =

        static let mutable objCount = 0

        let data = Array.zeroCreate 100000

        let number = Interlocked.Increment &objCount

        do
            Log.line "Create %s %d" name number

        static member ObjCount = objCount

        member x.Name 
            with get() = name

        override x.Finalize() = 
            Interlocked.Decrement &objCount |> ignore
            Log.line "Collect %s %d" name number

    [<Test>]
    let ``[Caches] BinaryCache Mod``() =
    
        let a = Mod.init "hugo" 

        let funf : IMod<string> -> IMod<string> -> IMod<string> = 
            //BinaryCache<IMod<string>, IMod<string>, IMod<string>>(Mod.map2 (+)).Invoke
            BinaryCache<IMod<string>, IMod<string>, IMod<string>>(Mod.map2 (fun a b -> a + b)).Invoke

        let sw = Stopwatch.StartNew()
        let mutable i = 0
        while sw.Elapsed.TotalSeconds < 10.0 do
        
            let temp = Mod.init (RuntimeObject(sprintf "run-%i" i))
            let str = temp |> Mod.map (fun r -> r.Name)
        
            let cacheFun = funf str // creates function closure that holds str ? // function will be used as key ???
            let result = cacheFun a

            Log.line "created: %s" (result.GetValue())

            i <- i + 1 

            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore

        System.GC.Collect(3)
        System.GC.WaitForFullGCComplete() |> ignore

        Log.line "RuntimeObject Count=%d" RuntimeObject.ObjCount

        if RuntimeObject.ObjCount > 100 then
            failwith "leak"


    [<Test>]
    let ``[Mod] Concurrency + callback + GetValue``() =
        let a = Mod.init 4
        let b = Mod.init 13

        let comp = Mod.map2 (fun a b -> let mutable res = 0 
                                        for i in 0..a do
                                             for j in 0..b do
                                                 res <- res ^^^ (i * j)
                                        res) a b

        // if marking callback is registered to "comp" instead of "a" it works fine
        //  -> not allowed to do this !!
        //let foo = a.AddMarkingCallback (fun () -> 
        //                                        let res = comp |> Mod.force
        //                                        Log.line "mark: %d" res)

        // safe way of realizing similar functionality
        let foo = a.AddMarkingCallback (fun () -> 
                                Transaction.Running.Value.AddFinalizer(fun () ->
                                                let res = comp |> Mod.force
                                                Log.line "mark: %d" res))
                                                
        let mutable updateCount = 0
        let mutable running = true

        let readerThread = System.Threading.Thread(ThreadStart(fun _ ->
            while running do
                let res = comp |> Mod.force
                Log.line "read: %d" res
                ()
            ()
        ))

        let updateThread = System.Threading.Thread(ThreadStart(fun _ ->
            let rnd = System.Random(55)
            while running do
                transact(fun () ->
                    a.Value <- rnd.Next(19999)
                )
                Log.line "update"
                updateCount <- updateCount + 1
            ()
        ))

        updateThread.Start()
        readerThread.Start()

        // run for 10 sec
        let mutable lastCount = -1
        Thread.Sleep(100)
        for i in 0..100 do 
            if lastCount = updateCount then
                failwith "deadlock"
            lastCount <- updateCount
            Thread.Sleep(100)
        ()

        running <- false