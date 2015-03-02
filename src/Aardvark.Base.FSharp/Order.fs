#if INTERACTIVE
#r "..\\..\\Packages\\Rx-Core.2.2.4\\lib\\net45\\System.Reactive.Core.dll"
#r "..\\..\\Packages\\NUnit.2.6.3\\lib\\nunit.framework.dll"
#r "..\\..\\Bin\\Release\\Aardvark.Base.dll"
#r "..\\..\\Bin\\Release\\Aardvark.VRVis.Base.dll"
#r "..\\..\\Bin\\Release\\Aardvark.Preliminary.dll"
#r "..\\..\\Bin\\Release\\Aardvark.Base.FSharp.dll"
#r "..\\..\\Bin\\Release\\Aardvark.VRVis.FSharp.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif


open System

[<CustomComparison>]
[<CustomEquality>]
type Time = { mutable time : uint64; mutable next : Time; mutable prev : Time; mutable rep : Time } with
    member x.GetTime() = 
        x.time - x.rep.time

    member x.Prev = x.prev
    member x.Next = x.next
    member x.Root = x.rep

    interface IComparable with
        member x.CompareTo o =
            match o with
                | :? Time as o -> x.GetTime().CompareTo (o.GetTime())
                | _ -> failwith ""

    override x.GetHashCode() = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x) //x.time.GetHashCode()
    override x.Equals o = System.Object.ReferenceEquals(x,o)
    override x.ToString() = 
        sprintf "%.5f" ((float <| x.GetTime()) / (float System.UInt64.MaxValue))


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Time =
    let mutable private root = { time = 0UL; next = Unchecked.defaultof<Time>; prev = Unchecked.defaultof<Time>; rep = Unchecked.defaultof<Time> }
    do root.next <- root
       root.prev <- root
       root.rep <- root

    let newRoot() =
       let root = { time = 0UL; next = Unchecked.defaultof<Time>; prev = Unchecked.defaultof<Time>; rep = Unchecked.defaultof<Time> }
       root.next <- root
       root.prev <- root
       root.rep <- root
       root

    let reset() =
       root <- { time = 0UL; next = Unchecked.defaultof<Time>; prev = Unchecked.defaultof<Time>; rep = Unchecked.defaultof<Time> }
       root.next <- root
       root.prev <- root
       root.rep <- root

    let distance (t0 : Time) (t1 : Time) =
        if t0 = t1 then System.UInt64.MaxValue
        else t1.GetTime() - t0.GetTime()

    let after (t : Time) =
        let mutable dn = distance t t.next
        if dn = 1UL then
            let mutable current = t.next
            let mutable j = 1UL
            while distance t current < 1UL + j * j do
                current <- current.next
                j <- j + 1UL

            let step = (distance t current) / j
            current <- t.next
            let mutable currentTime = t.time + step

            for k in 1UL..(j-1UL) do
                current.time <- currentTime
                current <- current.next
                currentTime <- currentTime + step


            dn <- step
        
        let res = { rep = t.rep; time = t.time + dn / 2UL; next = t.next; prev = t }
        t.next.prev <- res
        t.next <- res
        res

    let before (t : Time) =
        if t.rep = t then failwith "cannot insert before root time"
        after t.prev

    let delete (t : Time) =
        t.next.prev <- t.prev
        t.prev.next <- t.next
        t.next <- Unchecked.defaultof<Time>
        t.prev <- Unchecked.defaultof<Time>
        t.time <- 0UL

    let deleteRangeOpenClosed (s : Time) (e : Time) =
        s.next <- e.next
        e.next.prev <- s

    let deleteAll (t : Time) =
        if t = t.rep then
            t.next <- t
            t.prev <- t
            t.time <- 0UL
        else
            failwith "deleteAll shall only be called on root-times"