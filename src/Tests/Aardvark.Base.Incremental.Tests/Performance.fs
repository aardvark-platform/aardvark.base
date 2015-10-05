namespace Aardvark.Base.Incremental.Tests

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open System.Diagnostics


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
            mods |> List.map (fun m -> m :> IAdaptiveObject) |> Mod.mapCustom (fun () ->
                mods |> List.map Mod.force |> List.sum
            )
        // warmup
        sum |> Mod.force |> ignore


        let sum = 
            mods |> List.map (fun m -> m :> IAdaptiveObject) |> Mod.mapCustom (fun () ->
                mods |> List.map Mod.force |> List.sum
            )

        let sw = Stopwatch()
        sw.Start()
        sum |> Mod.force |> ignore
        sw.Stop()

        Console.WriteLine("elapsed: {0}ms", sw.Elapsed.TotalMilliseconds)




