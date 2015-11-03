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



    open System.Threading
    [<Test>]
    let ``[Mod] memory test`` () =

        //total e9206773e69bfe38e737c434d5496c9d56332160: 
        //172870528L
        //, per mod: 
        //1728.70528
        // (bytes)
        //10

        let t = Thread(ThreadStart(fun () ->
            
            let mutable m = Mod.init 10 :> IMod<_>
            let mutable f = id
            let mutable m = Mod.map f m
            
            let size = 10000
            ignore (Mod.force m)
            let mutable i = 0
            System.GC.Collect(3, System.GCCollectionMode.Forced, true)
            let mem = System.GC.GetTotalMemory(true)

            while i < size do
                m <- Mod.map f m
                i <- i + 1

            ignore (Mod.force m)

            System.GC.Collect(3, System.GCCollectionMode.Forced, true)
            let memAfter = System.GC.GetTotalMemory(true)
            let diff = memAfter - mem
            let sizePerMod = float diff / float size

            printfn "per mod:   %Abyte" sizePerMod

            printfn "%A" (Mod.force m)
        ), 100000000)
        t.Start()
        t.Join()


    [<Test>]
    let ``[VolatileCollection] memory test`` () =
        let count = 10000
        let arr = Array.zeroCreate count
        let objects = Array.init count (fun _ -> obj())

        let before = System.GC.GetTotalMemory(true)

        for i in 0..count-1 do
            arr.[i] <- VolatileCollection()
            ignore (arr.[i].Add(objects.[i]))

        let after = System.GC.GetTotalMemory(true)

        let mem = after - before
        let perInstance = float mem / float count
        System.Console.WriteLine("real:      {0}", perInstance)

        let dummy = arr |> Array.exists (fun c -> c.IsEmpty)
        if dummy then
            printfn "asdlkasndksajmdlkasmdl"

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
