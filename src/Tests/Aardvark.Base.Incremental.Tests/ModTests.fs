namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open System.Threading
open System.Threading.Tasks

module ``Basic Mod Tests`` =
    open System.Collections.Generic

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
        outputB.AddMarkingCallback(fun () -> marked <- true) |> ignore

        outputB |> Mod.force |> ignore // comment and reactivate the one after transact
        transact (fun () -> Mod.change input 11)
        //outputB |> Mod.force |> ignore

        outputA |> Mod.force |> should equal 11
        marked |> should equal true


    [<Test>]
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


    [<Test>]
    let ``[VolatileCollection] memory test`` () =
        ()
//        let count = 10000
//        let arr = Array.zeroCreate count
//        let objects = Array.init count (fun _ -> { new IWeakable<_> with member x.Weak = o })
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
//        let dummy = arr |> Array.exists (fun c -> c.Weak.IsEmpty)
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
    let ``[Mod] toObservable test``() =
        let m = Mod.init 10
        let mutable exec = 0
        let cnt() =
            let v = exec
            exec <- 0
            v

        let d = m |> Mod.map (fun a -> exec <- exec + 1; a)


        let obs = d |> Mod.toObservable

        let s0 = obs.Subscribe (fun v -> v |> should equal m.Value)
        cnt() |> should equal 1

        let s1 = obs.Subscribe (fun v -> v |> should equal m.Value)
        cnt() |> should equal 0


        transact (fun () -> Mod.change m 11)
        cnt() |> should equal 1

        s0.Dispose()
        cnt() |> should equal 0

        s1.Dispose()
        cnt() |> should equal 0

        transact (fun () -> Mod.change m 11)
        cnt() |> should equal 0



        ()

    open System
    open System.Reactive
    open System.Reactive.Linq
    [<Test>]
    let ``[Mod] observable builder``() =
        
        let down = Mod.init false
        let pos = Mod.init V2i.Zero



        let test =
            obs {
                while true do
                    System.Console.WriteLine("new polygon")
                    let polygon = List<V2i>()

                    // wait for down to become true
                    while down do
                        System.Console.WriteLine("in while")
                    
                        // while down accumulate all points in polygon and
                        // yield intermediate representations.
                        for p in pos do
                            polygon.Add p
                            yield Left (Seq.toList polygon)

                    // when the button is up again yield the final polygon
                    yield Right (Seq.toList polygon)
                    System.Console.WriteLine("finished polygon ({0} points)", polygon.Count)

            }


        let l = List<_>()
        let s = test.Subscribe(fun v -> l.Add v)
//        let latest = 
//            test 
//                |> Observable.choose (fun v -> match v with | Left i -> Some i | _ -> None) 
//                |> Observable.latest

        

        transact (fun () -> Mod.change down true)
        transact (fun () -> Mod.change pos V2i.II)
        transact (fun () -> Mod.change pos V2i.IO)
        transact (fun () -> Mod.change down false)
        transact (fun () -> Mod.change pos V2i.Zero)
        transact (fun () -> Mod.change pos V2i.OI)

        pos.OutOfDate |> should be True

        let all = l |> Seq.toList
        all |> should equal [Left [V2i.Zero]; Left [V2i.Zero; V2i.II]; Left [V2i.Zero; V2i.II; V2i.IO]; Right [V2i.Zero; V2i.II; V2i.IO]]
        l.Clear()


        transact (fun () -> Mod.change down true)
        transact (fun () -> Mod.change pos V2i.II)
        transact (fun () -> Mod.change pos V2i.IO)
        transact (fun () -> Mod.change down false)
        let all = l |> Seq.toList
        all |> should equal [Left [V2i.OI]; Left [V2i.OI; V2i.II]; Left [V2i.OI; V2i.II; V2i.IO]; Right [V2i.OI; V2i.II; V2i.IO]]
        l.Clear()



        ()





    [<AutoOpen>]
    module Validation =
        open Aardvark.Base.Incremental.Validation
        [<Test>]
        let ``[Mod] DotSerialization`` () =
            let x = Mod.init 10
            let y = Mod.init 20
            let z = Mod.map2 ((+)) x y
            z.DumpDotFile (1000, "test.dot") |> ignore

        [<Test>]
        let ``[Mod] DgmlSerialization`` () =
            let x = Mod.init 10
            let y = Mod.init 20
            let z = Mod.map2 ((+)) x y
            z.DumpDgml (1000, "test.dgml") |> ignore
