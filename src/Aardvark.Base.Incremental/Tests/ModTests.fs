namespace Aardvark.Base.Incremental.Tests

open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit

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
    let ``basic map test``() =
        let cell = Mod.initMod 1

        let derived = cell |> Mod.map (fun a -> 2 * a)

        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 2

        transact (fun () ->
            Mod.change cell 2
        )

        derived.OutOfDate |> should equal true
        derived |> Mod.force |> should equal 4

    [<Test>]
    let ``constant map test``() =
        let cell = Mod.initConstant 1

        let derived = cell |> Mod.map (fun a -> 2 * a)

        derived.IsConstant |> should equal true
        derived |> Mod.force |> should equal 2


    [<Test>]
    let ``basic map2 test``() =
        let cell1 = Mod.initMod 1
        let cell2 = Mod.initMod 1

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
    let ``constant map2 test``() =
        let c1 = Mod.initConstant 1
        let c2 = Mod.initConstant 1
        let m1 = Mod.initMod 1
        let m2 = Mod.initMod 1

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
    let ``basic bind test``() =
        let cell1 = Mod.initMod true
        let cell2 = Mod.initMod 2
        let cell3 = Mod.initMod 3

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
    let ``level changing bind``() =
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
            ) |> Mod.always

        let res = res |> Mod.map (fun a -> ex.push "cont"; a)// |> Mod.always

        let s = res |> Mod.registerCallback (fun v -> printfn "cb: %A" v)

        res.GetValue() |> should equal 1
        ex.read() |> should equal ["bind: true"; "cont"]

        transact(fun () ->
            a.Value <- 10
            c.Value <- false
        )

        res.GetValue() |> should equal 20
        ex.read() |> should equal ["bind: false"; "10 * 2"; "cont"]

        s.Dispose()

        transact(fun () ->
            a.Value <- 20
            c.Value <- true
        )
        res.GetValue() |> should equal 20
        ex.read() |> should equal ["bind: true"]


        ()

    [<Test>]
    let ``bind in bind``() =
        let a = Mod.initMod false
        let b = Mod.initMod 10
        let c =
            adaptive {
                let! a = a 
                if a then
                    let! b = b 
                    return b
                else
                    return 0
            }

        let c = c |> Mod.always

        c.GetValue() |> should equal 0

        transact(fun () -> Mod.change a true)
        c.GetValue() |> should equal 10

        transact(fun () -> Mod.change a false)
        c.GetValue() |> should equal 0