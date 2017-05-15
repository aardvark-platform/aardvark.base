namespace Aardvark.Base.Incremental.Tests

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open System.Diagnostics
open System.Linq

type ISnapshot =
    abstract member Restore : unit -> unit

type ISnapshotThingy =
    abstract member NewSnapshot : unit -> ISnapshot

type Snapshot(inner : list<ISnapshot>) =
    member x.Restore() =
        for i in inner do i.Restore()

    member x.Count = List.length inner


    interface ISnapshot with
        member x.Restore() = x.Restore()



type UndoRedoScope() =
    let dirty = HashSet<ISnapshotThingy>()

    member x.Changed(o : ISnapshotThingy) =
        lock dirty (fun () -> dirty.Add o |> ignore)

    member x.Snapshot() =
        let dirty =
            lock dirty (fun () ->
                let res = dirty.ToArray()
                dirty.Clear()
                res |> Array.toList
            )

        dirty
            |> List.map (fun d -> d.NewSnapshot() )
            |> Snapshot


[<AutoOpen>]
module ScopeExtensions =

    type private UndoRedoModRef<'a>(scope : UndoRedoScope, value : 'a) =
        inherit Mod.AbstractMod<'a>()

        let mutable value = value

        member x.Value
            with get() = value
            and set v =
                value <- v
                scope.Changed x
                x.MarkOutdated()

        member x.UnsafeCache
            with get() = value
            and set v = value <- v

        interface ISnapshotThingy with
            member x.NewSnapshot() =
                let v = x.GetValue()
                { new ISnapshot with member y.Restore() = x.Value <- v }
    

        interface IModRef<'a> with
            member x.Value
                with get() = value
                and set v = x.Value <- v

            member x.UnsafeCache
                with get() = value
                and set v = value <- v

            [<CLIEvent>]
            member x.Changed = Unchecked.defaultof<IEvent<_,_>>

        override x.Compute(t) =
            value


    type UndoRedoScope with
        member x.initMod (v : 'a) = 
            let res = UndoRedoModRef(x, v)
            x.Changed res
            res :> IModRef<_>


module SimpleTest =
    
    let run() =
        let scope = UndoRedoScope()

        let m = scope.initMod 10
        let d = m |> Mod.map (fun a -> a * 2)
        d |> Mod.force |> printfn "d = %A"


        // create a snapshot
        let s0 = scope.Snapshot()


        // change the system a little
        transact (fun () ->
            Mod.change m 1000
        )
        d |> Mod.force |> printfn "d = %A"

        // create a second snapshot
        let s1 = scope.Snapshot()

        // restore s0
        transact (fun () -> s0.Restore())
        d |> Mod.force |> printfn "s0: d = %A"
        
        // restore s1
        transact (fun () -> s1.Restore())
        d |> Mod.force |> printfn "s1: d = %A"


