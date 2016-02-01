namespace Aardvark.Base.Incremental.Tests

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open System.Diagnostics
open System.Linq
open Aardvark.Base

module InstancingTest =


    let test () =
        let m = Mod.init 10
        let s = Hole("a" |> Symbol.Create)
        let m2 = InstancedMod.ofMod m
        let m3 = m2 |> InstancedMod.bind (fun a -> InstancedMod.map (fun s -> s * a) s)

        let c1 = Mod.constant 10 
        let ctx1 = Map.ofList [ ("a" |> Symbol.Create, c1 :> IMod) ] 
        let usage1 = m3 |> InstancedMod.instantiate ctx1
        printfn "%A" ( usage1  |> Mod.force )


        let c2 =  Mod.init 6 
        let ctx2 = Map.ofList [ ("a" |> Symbol.Create, c2 :> IMod) ] 
        let usage2 =  m3 |> InstancedMod.instantiate ctx2
        printfn "%A" (usage2 |> Mod.force)


        transact (fun () -> Mod.change m 20) 
        ()