module StoreTest

open System
open System.Text
open System.Diagnostics
open System.Security.Cryptography
open Aardvark.Base
open Aardvark.Base.Native
open Aardvark.Base.Native.FileManager
open System.IO
open Microsoft.FSharp.NativeInterop
open System.Threading

#nowarn "9"

let hash = MD5.Create()
let md5 (str : string) =
    str |> Encoding.Unicode.GetBytes |> hash.ComputeHash |> Guid


module FS = 
    module MemoryHelpers =
        let mapped (f : Memory -> 'a) =
            let path = Path.GetTempFileName()
            if System.IO.File.Exists path then System.IO.File.Delete path

            let mem = Memory.mapped path
            Log.start "File"
            try f mem
            finally 
                Log.stop()
                mem.Dispose()
                System.IO.File.Delete path

        let hglobal (f : Memory -> 'a) =
            let mem = Memory.hglobal 0L
            Log.start "HGlobal"
            try f mem
            finally 
                Log.stop()
                mem.Dispose()

    let test() =
        use s = new FileManager(Memory.hglobal 0L)
        
        let data = Array.init 200 byte
        let e = s.Alloc(data.LongLength)

        s.Write(e, data)
        let r = s.Read(e)
        printfn "%A" r

        s.Free(e)

    let moreTest () =
        //if File.Exists @"C:\Users\Schorsch\Desktop\test.bin" then File.Delete @"C:\Users\Schorsch\Desktop\test.bin"

        use s = new FileManager(Memory.mapped @"C:\Users\Schorsch\Desktop\test.bin")

        printfn "(%d/%A)" s.Entries s.TotalMemory

        let r = System.Random()
        let allocations = System.Collections.Generic.List()

        let run () =
            let op = r.NextDouble() 
            if op < 0.8 || allocations.Count <= 0 then
                let ssize = 1+r.Next(255)
                let e = s.Alloc(int64 ssize)
                s.Write(e,Array.create ssize (byte e))
                allocations.Add(e)
            else 
                let index = r.Next(0,allocations.Count-1)
                let a = allocations.[index]
                let arr = s.Read a
                let r = arr |> Array.forall (fun b -> b = byte a)
                if not r then printfn "furious anger"
                s.Free(a)
                allocations.RemoveAt index

        for i in 0 .. 1000000 do
            if i%100 = 0 then printfn "it: %d (%d/%A)" i s.Entries s.TotalMemory
            run ()

    let allocPerf() =

        let log = @"C:\Users\schorsch\Desktop\store.csv"

        let test (kind : string) (cnt : int) (mem : Memory) =
            mem.Clear(1024L)
            use s = new FileManager(mem)
            let r = System.Random()


            let mutable blocks = Array.zeroCreate cnt
            let sizes = Array.init cnt (fun _ -> int64 (1 + r.Next 255))
            let data = Array.zeroCreate 1024

            let sw = Stopwatch()
            sw.Restart()
            for i in 0 .. cnt-1 do
                blocks.[i] <- s.Alloc sizes.[i]
            sw.Stop()
            let talloc = sw.MicroTime / float cnt
            Log.line "alloc: %A" talloc


            sw.Restart()
            for i in 0 .. cnt-1 do
                s.Write(blocks.[i], data, 0L, sizes.[i])
            sw.Stop()
            let twrite = sw.MicroTime / float cnt
            Log.line "write: %A" twrite

            blocks <- blocks.RandomOrder() |> Seq.toArray
            sw.Restart()
            for i in 0 .. cnt-1 do
                s.Free(blocks.[i])
            sw.Stop()
            let tfreer = sw.MicroTime / float cnt
            Log.line "free:  %A (random order)" tfreer

            mem.Clear(1024L)
            use s = new FileManager(mem)
            for i in 0 .. cnt-1 do
                blocks.[i] <- s.Alloc sizes.[i]
            sw.Restart()
            for i in 0 .. cnt-1 do
                s.Free(blocks.[i])
            sw.Stop()
            let tfrees = sw.MicroTime / float cnt
            Log.line "free:  %A (sequential)" tfrees



            File.AppendAllLines(log, [sprintf "%s;%d;%d;%d;%d;%d" kind cnt talloc.TotalNanoseconds twrite.TotalNanoseconds tfreer.TotalNanoseconds tfrees.TotalNanoseconds])


        File.WriteAllLines(log, ["mem;size;alloc [ns];write [ns];free (random) [ns]; free (sequential) [ns]"])
        for i in 10 .. 20 do
            let cnt = 1 <<< i
            MemoryHelpers.hglobal (test "hglobal" cnt)
            MemoryHelpers.mapped (test "mapped" cnt)

    let fileTest() =
        use s = new BlobStore(Memory.mapped @"C:\Users\Schorsch\Desktop\test.bin")

        let a = s.Get (Guid "d5993e65-6e27-4596-85c1-2a59491477c0")
        let b = s.Get (Guid "d5993e65-6e27-4596-85c1-2a59491477c0")

        
        match a.Exists with
            | true -> 
                let arr = a.Read() 
                Log.line "a = %A" arr
                let arr = Array.append arr [|123uy|]
                a.Write arr

                

            | _ ->
                a.Write(Array.init 10 byte)
                a.Read() |> Log.line "a = %A (created)"

        s.Print()