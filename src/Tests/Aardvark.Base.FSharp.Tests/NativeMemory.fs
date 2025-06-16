namespace Aardvark.Base.FSharp.Tests
#nowarn "44"

open System
open System.Runtime.InteropServices
open System.Diagnostics
open System.Threading
open System.Threading.Tasks
open Aardvark.Base

open FsUnit
open NUnit.Framework

module NativeUtilitiesTests =

    [<Test>]
    let ``[NativePtr] Pin array``() =
        let array = [| 1UL; 2UL; 3UL; 4UL; 5UL |]

        array |> NativePtr.pinArr (fun ptr ->
            ptr.[0] <- 42UL
            ptr.[3] <- 43UL
            ptr.[4] <- 44UL
        )

        array |> should equal [| 42UL; 2UL; 3UL; 43UL; 44UL |]

    [<Test>]
    let ``[NativePtr] Pin array with index``() =
        let array = [| 1UL; 2UL; 3UL; 4UL; 5UL |]

        (0, array) ||> NativePtr.pinArri (fun ptr -> ptr.[0] <- 42UL)
        array |> should equal [| 42UL; 2UL; 3UL; 4UL; 5UL |]

        (3, array) ||> NativePtr.pinArri (fun ptr -> ptr.[0] <- 43UL)
        array |> should equal [| 42UL; 2UL; 3UL; 43UL; 5UL |]

        (4, array) ||> NativePtr.pinArri (fun ptr -> ptr.[0] <- 44UL)
        array |> should equal [| 42UL; 2UL; 3UL; 43UL; 44UL |]

module MemoryManagerTests =
    
    let create() = new MemoryManager(16n, Marshal.AllocHGlobal, fun ptr _ -> Marshal.FreeHGlobal ptr)

    type Interlocked with
        static member Change(location : byref<int>, f : int -> int) =
            let mutable v = location
            let mutable res = f v
            let mutable r = Interlocked.CompareExchange(&location, res, v)

            while v <> r do
                v <- r
                res <- f v
                r <- Interlocked.CompareExchange(&location, res, v)

            res



    [<Test>]
    let ``[Memory] simple alloc test``() =
        let m = create()

        let b0 = m.Alloc(10n)
        let b1 = m.Alloc(6n)

        m.Validate()

    [<Test>]
    let ``[Memory] simple free test``() =
        let m = create()

        let b0 = m.Alloc(10n)
        m.Free b0


        let b1 = m.Alloc(16n)
        
        m.Validate()

    [<Test>]
    let ``[Memory] free collapse left``() =
        let m = create()

        let b0 = m.Alloc(2n)
        let b1 = m.Alloc(2n)
        let b2 = m.Alloc(2n)
        m.Free b0
        m.Free b1

        
        m.Validate()

    [<Test>]
    let ``[Memory] free collapse right``() =
        let m = create()

        let b0 = m.Alloc(2n)
        let b1 = m.Alloc(2n)
        let b2 = m.Alloc(2n)
        let r = m.Alloc(10n)
        m.Free b2
        m.Free b1

        
        m.Validate()

    [<Test>]
    let ``[Memory] free collapse both``() =
        let m = create()

        let b0 = m.Alloc(2n)
        let b1 = m.Alloc(2n)
        let b2 = m.Alloc(2n)
        let r = m.Alloc(10n)
        m.Free b2
        m.Free b0
        m.Free b1

        
        m.Validate()


    [<Test>]
    let ``[Memory] realloc move``() =
        let m = create()

        let b0 = m.Alloc(2n)
        let b1 = m.Alloc(2n)
        
        m.Realloc(b0, 4n) |> should be True
        m.Validate()

        b0.Size |> should equal 4n
        b0.Offset |> should equal 4n


    [<Test>]
    let ``[Memory] realloc exact space left``() =
        let m = create()

        let b0 = m.Alloc(2n)
        let b1 = m.Alloc(4n)
        let b2 = m.Alloc(2n)
        
        m.Free(b1)

        m.Realloc(b0, 6n) |> should be False
        m.Validate()


    [<Test>]
    let ``[Memory] realloc more space left``() =
        let m = create()

        let b0 = m.Alloc(2n)
        let b1 = m.Alloc(5n)
        let b2 = m.Alloc(2n)
        
        m.Free(b1)

        m.Realloc(b0, 6n) |> should be False
        m.Validate()


    [<Test>]
    let ``[Memory] realloc shrink``() =
        let m = create()

        let b0 = m.Alloc(2n)
        let b1 = m.Alloc(4n)

        m.Realloc(b0, 1n) |> should be False
        m.Validate()


    [<Test>]
    let ``[Memory] realloc 0``() =
        let m = create()

        let b0 = m.Alloc(2n)
        let b1 = m.Alloc(4n)

        m.Realloc(b0, 0n) |> should be False
        m.Validate()

        b0.Size |> should equal 0n


    [<Test>]
    let ``[Memory] resize``() =
        let m = create()

        let b0 = m.Alloc(10n)
        let b1 = m.Alloc(100n)

        m.Validate()


    [<Test>]
    let ``[Memory Performance] allocations``() =
        let m = create()
        let r = Random()


        // warm-up
        for i in 0..100 do
            m.Free(m.Alloc(r.Next(1 <<< 5) + 1 |> nativeint))


        let sw = Stopwatch()
        let mutable iterations = 0

        sw.Start()
        while sw.Elapsed.TotalMilliseconds < 1000.0 do
            m.Alloc(r.Next(1 <<< 5) + 1 |> nativeint) |> ignore
            iterations <- iterations + 1

        sw.Stop()
        let microseconds = sw.Elapsed.TotalMilliseconds * 1000.0

        Console.WriteLine("{0} µs/allocation", microseconds / float iterations)

    [<Test>]
    let ``[Memory Performance] free``() =
        let m = create()
        let r = Random()

        // warm-up
        for i in 0..100 do
            m.Free(m.Alloc(r.Next(1 <<< 5) + 1 |> nativeint))

        let sw = Stopwatch()

        let blocks = Array.init (1 <<< 17) (fun _ -> m.Alloc(r.Next(1 <<< 5) + 1 |> nativeint))
        let blocks = blocks.RandomOrder() |> Seq.toArray

        sw.Start()
        for i in 0..blocks.Length-1 do
            m.Free(blocks.[i])
        sw.Stop()
        let microseconds = sw.Elapsed.TotalMilliseconds * 1000.0

        Console.WriteLine("{0} µs/free", microseconds / float blocks.Length)


    [<Test>]
    let ``[Memory Performance] realloc no space``() =
        let m = create()
        let r = Random()

        // warm-up
        for i in 0..100 do
            let b = m.Alloc(r.Next(1 <<< 5) + 1 |> nativeint)
            m.Realloc(b, b.Size + 1n) |> ignore
            m.Free(b)

        let sw = Stopwatch()

        let blocks = Array.init (1 <<< 17) (fun _ -> m.Alloc(r.Next(1 <<< 5) + 1 |> nativeint))
        let blocks = blocks.RandomOrder() |> Seq.toArray

        sw.Start()
        for i in 0..blocks.Length-1 do
            m.Realloc(blocks.[i], blocks.[i].Size + 1n) |> ignore
        sw.Stop()
        let microseconds = sw.Elapsed.TotalMilliseconds * 1000.0

        Console.WriteLine("{0} µs/realloc (no space left)", microseconds / float blocks.Length)


    [<Test>]
    let ``[Memory Performance] realloc next free``() =
        let m = create()
        let r = Random()

        // warm-up
        for i in 0..100 do
            let b = m.Alloc(r.Next(1 <<< 5) + 1 |> nativeint)
            m.Realloc(b, b.Size + 1n) |> ignore
            m.Free(b)


        let sw = Stopwatch()

        let blocks = Array.init (1 <<< 18) (fun _ -> m.Alloc(r.Next(1 <<< 5) + 1 |> nativeint))
        for i in 0..2..blocks.Length-1 do
            m.Free(blocks.[i])

        let blocks = blocks |> Array.mapi (fun i a -> if i % 2 <> 0 then Some a else None) 
                            |> Array.choose id

        let blocks = blocks.RandomOrder() |> Seq.toArray

        sw.Start()
        for i in 0..blocks.Length-1 do
            m.Realloc(blocks.[i], blocks.[i].Size + 1n) |> ignore
        sw.Stop()
        let microseconds = sw.Elapsed.TotalMilliseconds * 1000.0

        Console.WriteLine("{0} µs/realloc (next free)", microseconds / float blocks.Length)


    let startTask (f : unit -> unit) =
        Task.Factory.StartNew(f, TaskCreationOptions.LongRunning) |> ignore

    let run (f : unit -> unit) =
        f()


    [<Test; Ignore("Broken. Fails sporadically")>]
    let ``[Memory] concurrent allocations``() =
        let cnt = 200uy
        let mem = MemoryManager.createHGlobal()

        let r = Random()
        let start = new ManualResetEventSlim(false)
        let finished = new SemaphoreSlim(0)
        let allblocks = ref Map.empty
        let currentWrites = ref 0
        let maxParallelWrites = ref 0

        for i in 0uy..cnt - 1uy do
            startTask (fun () ->
                let size = r.Next 100 + 1 |> nativeint
                start.Wait()

                let b = mem |> MemoryManager.alloc size

                let current = Interlocked.Increment(&currentWrites.contents)
                Interlocked.Change(&maxParallelWrites.contents, max current) |> ignore
                b.Write(0, Array.create (int size) i)
                Interlocked.Decrement(&currentWrites.contents) |> ignore

                

                Interlocked.Change(&allblocks.contents, Map.add i b) |> ignore

                finished.Release() |> ignore
            )

        start.Set()

        for i in 1uy..cnt do
            finished.Wait()

        mem.Validate()

        for (i,b) in Map.toSeq !allblocks do
            let data : byte[] = b |> ManagedPtr.readArray 0 
            data |> should equal (Array.create (int b.Size) i)

        Console.WriteLine("parallel writes: {0}", !maxParallelWrites)
        allblocks.Value.Count |> should equal (int cnt)
        !maxParallelWrites |> should greaterThan 1


    [<Test>]
    let ``[Memory] concurrent allocations / frees``() =
        let cnt = 200uy
        let mem = MemoryManager.createHGlobal()

        let r = Random()
        let start = new ManualResetEventSlim(false)
        let finished = new SemaphoreSlim(0)
        let allblocks = ref Map.empty



        for i in 0uy..cnt - 1uy do
            startTask (fun () ->
                let size = r.Next 100 + 1 |> nativeint
                start.Wait()

                let b = mem |> MemoryManager.alloc size
                b.Write(0, Array.create (int size) i)

                Interlocked.Change(&allblocks.contents, Map.add i b) |> ignore

                finished.Release() |> ignore
            )

        start.Set()

        for i in 1uy..cnt do
            finished.Wait()

        mem.Validate()



        
        start.Reset()

        for i in 0uy..cnt - 1uy do
            startTask (fun () ->
                let free = r.Next(1) = 0
                let b = Map.find i !allblocks
                start.Wait()

                if free then
                    ManagedPtr.free b
                else
                    b |> ManagedPtr.realloc (b.Size + 2n) |> ignore
                    b |> ManagedPtr.writeArray (int b.Size-2) [|i;i|]

                finished.Release() |> ignore
            )

        start.Set()

        for i in 1uy..cnt do
            finished.Wait()

        mem.Validate()

        for (i,b) in Map.toSeq !allblocks do
            if not b.Free then
                let data : byte[] = b |> ManagedPtr.readArray 0 
                data |> should equal (Array.create (int b.Size) i)
            else
                b.Size |> should equal 0n

        allblocks.Value.Count |> should equal (int cnt)


    [<Test>]
    let ``[Memory] concurrent random operations``() =
        let mem = MemoryManager.createHGlobal()
        let r = Random()

        let blocks = ref []

        let removeAny (s : list<managedptr>) =
            match s with
                | h::t -> t, Some h
                | _ -> [], None

        let add (ptr : managedptr) (l : list<managedptr>) =
            ptr::l

        let cnt = 2000
        let exns = ref []
        let sem = new SemaphoreSlim(0)

        for i in 1..cnt do
            startTask (fun () ->
            
                let op = r.Next(6)

                try
                    try
                        match op with
                            | 0 | 1 | 2 -> 
                                let b = mem.Alloc (r.Next(100) + 1 |> nativeint)
                                Interlocked.Change(&blocks.contents, add b) |> ignore

                            | 3 -> 
                                let b = Interlocked.Change(&blocks.contents, removeAny)
                                match b with
                                    | Some b -> b |> ManagedPtr.free
                                    | None -> ()

                            | 4 -> 
                                let b = Interlocked.Change(&blocks.contents, removeAny)
                                match b with
                                    | Some b -> 
                                        b |> ManagedPtr.realloc (r.Next(100) + 1 |> nativeint) |> ignore
                                        if b.Size > 0n then
                                            Interlocked.Change(&blocks.contents, add b) |> ignore
                                    | _ -> ()

                            | _ ->
                                let b = Interlocked.Change(&blocks.contents, removeAny)
                                match b with
                                    | Some b -> 
                                        let newBlock = ManagedPtr.spill b

                                        Interlocked.Change(&blocks.contents, add b >> add newBlock) |> ignore
                                    | _ -> 
                                        ()

                    with e ->
                        Interlocked.Change(&exns.contents, fun l -> e::l) |> ignore
                finally 
                    sem.Release() |> ignore
            )

        for i in 1..cnt do
            sem.Wait()

        for e in !exns do
            Console.WriteLine("{0}", e)

        !exns |> should be Empty

    [<Test>]
    let ``[Memory] random operations``() =
        let mem = MemoryManager.createHGlobal()
        let r = Random()

        let blocks = ref []

        let removeAny (s : list<managedptr>) =
            match s with
                | h::t -> t, Some h
                | _ -> [], None

        let add (ptr : managedptr) (l : list<managedptr>) =
            ptr::l

        let cnt = 2000
        let exns = ref []
        let sem = new SemaphoreSlim(0)

        for i in 1..cnt do
            run (fun () ->
            
                let op = r.Next(6)

                try
                    try
                        match op with
                            | 0 | 1 | 2 -> 
                                let b = mem.Alloc (r.Next(100) + 1 |> nativeint)
                                Interlocked.Change(&blocks.contents, add b) |> ignore

                            | 3 -> 
                                let b = Interlocked.Change(&blocks.contents, removeAny)
                                match b with
                                    | Some b -> b |> ManagedPtr.free
                                    | None -> ()

                            | 4 -> 
                                let b = Interlocked.Change(&blocks.contents, removeAny)
                                match b with
                                    | Some b -> 
                                        b |> ManagedPtr.realloc (r.Next(100) + 1 |> nativeint) |> ignore
                                        if b.Size > 0n then
                                            Interlocked.Change(&blocks.contents, add b) |> ignore
                                    | _ -> ()

                            | _ ->
                                let b = Interlocked.Change(&blocks.contents, removeAny)
                                match b with
                                    | Some b -> 
                                        let newBlock = ManagedPtr.spill b

                                        Interlocked.Change(&blocks.contents, add b >> add newBlock) |> ignore
                                    | _ -> 
                                        ()

                    with e ->
                        Interlocked.Change(&exns.contents, fun l -> e::l) |> ignore
                finally 
                    sem.Release() |> ignore
            )

        for i in 1..cnt do
            sem.Wait()

        for e in !exns do
            Console.WriteLine("{0}", e)

        !exns |> should be Empty

