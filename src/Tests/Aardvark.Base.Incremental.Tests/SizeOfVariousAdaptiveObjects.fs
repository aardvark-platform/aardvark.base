namespace Aardvark.Base.Incremental.Tests

open System
open System.Threading
open System.Threading.Tasks
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open Aardvark.Base

module SizeOfAdaptiveObjects = 

    [<Test>]
    let ``[MemoryOverhead] memory test`` () =

        //total e9206773e69bfe38e737c434d5496c9d56332160: 
        //total: 
        //5346872L
        //, per aset: 
        //5346.872
        // (bytes)
        let t = Thread(ThreadStart(fun () ->
            
            let mutable m = CSet.ofList [0] :> aset<_>

            let mem = System.GC.GetTotalMemory(true)

            let size = 1000

            let empty = ASet.empty
            for i in 1 .. size do
                m <- ASet.unionTwo m empty

            let r = m.GetReader()
            r.Update()

            let memAfter = System.GC.GetTotalMemory(true)
            let diff = memAfter - mem
            let sizePerMod = float diff / float (2 * size)

            printfn "per aset: %A (bytes)" (float diff / float size)

            printfn "per reader: %A (bytes)" sizePerMod

            r.Update()
            r.Dispose()
        ), 100000000)
        t.Start()
        t.Join()

    [<Test>]
    let ``[MemoryOverhead] adaptiveObject`` () =

        let xs = Array.zeroCreate 100000
        let mem = System.GC.GetTotalMemory(true)

        for x in 0 .. xs.Length - 1 do
            xs.[x] <- AdaptiveObject()

        let after = System.GC.GetTotalMemory(true)
        printfn "%f bytes." (float (after - mem) / float xs.Length)
        printfn "%A" (xs |> Array.sumBy (fun s -> uint64 s.Id))

    [<Test>]
    let ``[MemoryOverhead] copy reader`` () =

        let xs = Array.zeroCreate 100000
        let mem = System.GC.GetTotalMemory(true)

        for x in 0 .. xs.Length - 1 do
            xs.[x] <- new ASetReaders.CopyReader<int>(Unchecked.defaultof<_>)

        let after = System.GC.GetTotalMemory(true)
        printfn "%f bytes." (float (after - mem) / float xs.Length)
        printfn "%A" (xs |> Array.sumBy (fun s -> uint64 s.Id))

    [<Test>]
    let ``[MemoryOverhead] ReaderSize`` () =

        let xs = Array.zeroCreate 100000
        let mem = System.GC.GetTotalMemory(true)

        for x in 0 .. xs.Length - 1 do
            xs.[x] <- ASet.AdaptiveSet<int>(fun () -> Unchecked.defaultof<IReader<int>>)

        let after = System.GC.GetTotalMemory(true)
        printfn "%f bytes." (float (after - mem) / float xs.Length)
        printfn "%A" (xs |> Array.sumBy (fun s -> 0.01 * float (s.GetHashCode())))