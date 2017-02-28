namespace Aardvark.Base.Incremental.Tests

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open System.Diagnostics
open Aardvark.Base

module SimplePerfTests =

    [<Test>]
    let ``[Mod Performance] deep bind``() =
        
        let mods = List.init 500 (fun i -> Mod.init i)

        let rec flatten l =
            match l with
                | [] -> Mod.constant 0
                | x::xs ->
                    adaptive {
                        let! v = x
                        let! rest = flatten xs
                        return v + rest
                    }

        let sum = flatten mods

        // warmup
        sum |> Mod.force |> ignore


        let sum = flatten mods
        let sw = Stopwatch()
        sw.Start()
        sum |> Mod.force |> ignore
        sw.Stop()

        Console.WriteLine("elapsed: {0}ms", sw.Elapsed.TotalMilliseconds)


    [<Test>]
    let ``[Mod Performance] deep bind with fixed levels``() =
        
        let mods = List.init 500 (fun i -> Mod.init i)

        let rec flatten level l =
            match l with
                | [] -> Mod.constant 0
                | x::xs ->
                    let res = 
                        adaptive {
                            let! v = x
                            let! rest = flatten (level - 3) xs
                            return v + rest
                        }
                    res.Level <- level
                    res

        let sum = flatten 2000 mods

        // warmup
        sum |> Mod.force |> ignore


        let sum = flatten 2000 mods
        let sw = Stopwatch()
        sw.Start()
        sum |> Mod.force |> ignore
        sw.Stop()

        Console.WriteLine("elapsed: {0}ms", sw.Elapsed.TotalMilliseconds)


    [<Test>]
    let ``[Mod Performance] flat sum``() =
        
        let mods = List.init 500 (fun i -> Mod.init i)

        let sum = 
            mods |> List.map (fun m -> m :> IAdaptiveObject) |> Mod.mapCustom (fun self ->
                mods |> List.map (fun s -> s.GetValue self) |> List.sum
            )
        // warmup
        sum |> Mod.force |> ignore


        let sum = 
            mods |> List.map (fun m -> m :> IAdaptiveObject) |> Mod.mapCustom (fun self ->
                mods |> List.map (fun s -> s.GetValue self) |> List.sum
            )

        let sw = Stopwatch()
        sw.Start()
        sum |> Mod.force |> ignore
        sw.Stop()

        Console.WriteLine("elapsed: {0}ms", sw.Elapsed.TotalMilliseconds)

    [<Test>]
    let ``[ASet] value dependent nop change``() =
        let vt = Mod.init V3d.Zero
        let iter = 100

        let instances =
            aset {
                let mutable i = 0
                for x in 0..20 do
                    for y in 0..20 do
                        for z in 0..20 do
                            yield (i, fun (i : int) -> obj()) //V3i(x,y,z)
                            i <- i + 1
            }

        let current = Array.zeroCreate (21*21*21)
        let getLevel'(index : int, f) (m : V3d) =
            let level = 
                match V3d.Distance(m, m) with   
                    | v when v < 100.0 -> 0
                    | _ -> 0

            match current.[index] with
                | Some(ol,ov) when ol = level -> ov
                | _ ->
                    let n = f level
                    current.[index] <- Some(level, n)
                    n

        let getLevel (m : IMod<V3d>) (f : int -> obj) =
            let mutable current = None
            m |> Mod.map (fun m ->
                let level = 
                    match V3d.Distance(m, m) with   
                        | v when v < 100.0 -> 0
                        | _ -> 0

                match current with
                    | Some (l,v) when l = level ->
                        v
                    | _ ->
                        let v = f level
                        current <- Some (level, v)
                        v
            )

        let final =
            vt |> ASet.bind (fun v ->
                instances |> ASet.map (fun a -> getLevel' a v)
            )
//
//        let finaldsfsdf = 
//            instances |> ASet.mapM failwith ""//(getLevel vt)

        let r = final.GetReader()

        r.GetDelta() |> ignore

        printf "started incremental version: "
        let sw = Stopwatch()
        sw.Start()
        for i in 1..iter do
            transact (fun () -> Mod.change vt (vt.Value + V3d.III))
            r.GetDelta() |> ignore
        sw.Stop()
        printfn "%.3fms" (sw.Elapsed.TotalMilliseconds / float iter)
        r.Dispose()


        let creators = instances |> ASet.toArray |> Array.map snd
        let final = cset()
                

        let r = (final :> aset<_>).GetReader()
        printf "started non-incremental version: "
        let sw = Stopwatch()
        sw.Start()
        let current = Array.zeroCreate creators.Length

        let update (m : V3d) =
            for i in 0..creators.Length-1 do
                let level = 
                    match V3d.Distance(m, m) with   
                        | v when v < 100.0 -> 0
                        | _ -> 0

                match current.[i] with
                    | Some (l,v) ->
                        if l <> level then
                            let n = creators.[i] level
                            current.[i] <- Some (level, n)
                            transact (fun () -> 
                                CSet.remove v final |> ignore
                                CSet.add n final |> ignore
                            )

                    | _ ->
                        let v = creators.[i] level
                        current.[i] <- Some (level, v)
                        transact (fun () -> CSet.add v final |> ignore)

        update V3d.Zero

        for i in 1..iter do
            let m = V3d(i,i,i)
            update m
            r.GetDelta() |> ignore

        sw.Stop()
        printfn "%.3fms" (sw.Elapsed.TotalMilliseconds / float iter)
        r.Dispose()




      




