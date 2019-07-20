namespace Aardvark.Base.Incremental.Tests

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open System.Diagnostics
open Aardvark.Base

module ``performance tests`` =

    let timed name f =
        let sw = Stopwatch()
        sw.Start()
        let r = f()
        sw.Stop()
        let str = sprintf "%s took: %.2fms" name sw.Elapsed.TotalMilliseconds
        Console.WriteLine str
        r

    [<Test>]
    let ``[ASet] collect performance``() =
        
        let input = CSet.ofList []

        let step (s : aset<'a>) =
            s |> ASet.collect ASet.single

        let rec stepN n s =
            if n <= 0 then s
            else stepN (n-1) (step s)
            
           

        let test = stepN 100 input
        let r = test.GetReader()
        r.GetOperations() |> ignore


        for i in 0..200 do
            transact (fun () ->
                input.UnionWith [i]
            )
            r.GetOperations() |> ignore

        transact (fun () ->
            input.Clear()
        )
        r.GetOperations() |> ignore
  


        let changeTime = Stopwatch()
        let evalTime = Stopwatch()
        let iter = 200
        for i in 1..iter do
            changeTime.Start()
            transact (fun () ->
                input.Add i |> ignore
            )
            changeTime.Stop()

            evalTime.Start()
            r.GetOperations() |> ignore
            evalTime.Stop()

        Console.WriteLine("Aset.collect change: {0:0.00000}ms eval: {1:0.00000}ms", 
            changeTime.Elapsed.TotalMilliseconds / float iter, 
            evalTime.Elapsed.TotalMilliseconds / float iter)
        ()

    [<Test>]
    let ``[ASet] map performance``() =
        
        let input = CSet.ofList []

        let step (s : aset<'a>) =
            s |> ASet.map id

        let rec stepN n s =
            if n <= 0 then s
            else stepN (n-1) (step s)
            
                
        let test = stepN 100 input
        let r = test.GetReader()

        r.GetOperations() |> ignore


        for i in 0..400 do
            transact (fun () ->
                input.UnionWith [i]
            )
            r.GetOperations() |> ignore

        transact (fun () ->
            input.Clear()
        )
        r.GetOperations() |> ignore


        let changeTime = Stopwatch()
        let evalTime = Stopwatch()
        let iter = 400
        for i in 1..iter do
            changeTime.Start()
            transact (fun () ->
                input.Add i |> ignore
            )
            changeTime.Stop()

            evalTime.Start()
            r.GetOperations() |> ignore
            evalTime.Stop()

        Console.WriteLine("ASet.map     change: {0:0.00000}ms eval: {1:0.00000}ms", 
            changeTime.Elapsed.TotalMilliseconds / float iter, 
            evalTime.Elapsed.TotalMilliseconds / float iter)
        ()

    [<Test>]
    let ``[Mod] bind performance``() =
        
        let input = Mod.init 0

        let step (s : IMod<'a>) =
            s |> Mod.bind (Mod.constant)

        let rec stepN n s =
            if n <= 0 then s
            else stepN (n-1) (step s)
            
                

        let test = stepN 100 input
        
        Mod.force test |> ignore

        for i in 0..100 do
            transact (fun () ->
                Mod.change input i
            )
            Mod.force test |> ignore

        transact (fun () ->
            Mod.change input -1
        )
        Mod.force test |> ignore


        let changeTime = Stopwatch()
        let evalTime = Stopwatch()
        let iter = 1000
        for i in 1..iter do
            changeTime.Start()
            transact (fun () ->
                Mod.change input i
            )
            changeTime.Stop()

            evalTime.Start()
            Mod.force test |> ignore
            evalTime.Stop()

        Console.WriteLine("Mod.bind     change: {0:0.00000}ms eval: {1:0.00000}ms", 
            changeTime.Elapsed.TotalMilliseconds / float iter, 
            evalTime.Elapsed.TotalMilliseconds / float iter)
        ()

    [<Test>]
    let ``[Mod] map performance``() =
        
        let input = Mod.init 0

        let step (s : IMod<'a>) =
            s |> Mod.map id

        let rec stepN n s =
            if n <= 0 then s
            else stepN (n-1) (step s)
            
                

        let test = stepN 100 input
        
        Mod.force test |> ignore

        for i in 0..100 do
            transact (fun () ->
                Mod.change input i
            )
            Mod.force test |> ignore

        transact (fun () ->
            Mod.change input -1
        )
        Mod.force test |> ignore


        let changeTime = Stopwatch()
        let evalTime = Stopwatch()
        let iter = 1000
        for i in 1..iter do
            changeTime.Start()
            transact (fun () ->
                Mod.change input i
            )
            changeTime.Stop()

            evalTime.Start()
            Mod.force test |> ignore
            evalTime.Stop()

        Console.WriteLine("Mod.map      change: {0:0.00000}ms eval: {1:0.00000}ms", 
            changeTime.Elapsed.TotalMilliseconds / float iter, 
            evalTime.Elapsed.TotalMilliseconds / float iter)
        ()
