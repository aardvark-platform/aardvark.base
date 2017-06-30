namespace Aardvark.Base.Incremental.Tests

open System
open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open System.Threading
open System.Threading.Tasks

module AListTestsNew =


    type MkDispose(mkSource : string) =
        let mutable isDisposed = false
        member x.Source = mkSource
        member x.IsDisposed = isDisposed
        interface IDisposable with
            member x.Dispose () = 
                if isDisposed then failwith "doublefree"
                printfn "dispose: %A" mkSource
                isDisposed <- true

    let testMapUse () =
        
        let input = ["a";"b";"b";"c"] |> CList.ofSeq 
        let mkd = input |> AList.mapUse  (fun a -> new MkDispose(a))
        let m = AList.toMod mkd

        let original = m |> Mod.force |> PList.toArray

        let check wanted =
            let is = m |> Mod.force |> PList.toList |> List.map (fun a -> a.Source)
            should equal is wanted

        transact (fun _ -> 
            let r = CList.removeElement "c" input
            r |> should equal true
        )
        check ["a";"b";"b"]
        should equal original.[3].IsDisposed true

        transact (fun _ -> 
            let r = CList.removeElement "b" input
            r |> should equal true
        )
        check ["a";"b"]
        should equal original.[1].IsDisposed false

        transact (fun _ -> 
            let r = CList.removeElement "b" input
            r |> should equal true
        )
        check ["a"]
        should equal original.[1].IsDisposed true
        
        transact (fun _ -> 
            let r = CList.removeElement "a" input
            r |> should equal true
        )
        check []
        should equal original.[0].IsDisposed true

        for a in original do should equal a.IsDisposed true

        ()

