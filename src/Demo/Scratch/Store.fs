module StoreTest

open System
open System.Text
open System.Diagnostics
open System.Security.Cryptography
open Aardvark.Base
open Aardvark.Base.Native
open System.IO

#nowarn "9"

let hash = MD5.Create()
let md5 (str : string) =
    str |> Encoding.Unicode.GetBytes |> hash.ComputeHash |> Guid


let sizeInvariantBench() =
    let sw = Stopwatch()

    let ids = Array.init (1 <<< 20) (fun i -> i |> string |> md5)
    let data : byte[] = Array.init 128 byte
    let data2 : byte[] = Array.zeroCreate 128

    let path = @"C:\Users\Schorsch\Desktop\store.csv"
    File.WriteAllLines(path, ["size;get [ns];write [ns];update [ns];delete[ns]"])

    for cnt in 4096 .. 4096 .. 1 <<< 20 do
        let ids = Array.take cnt ids
        let mem = Memory.hglobal 0L
        use store = new Store(mem)

        printf "%d: " cnt
        
        sw.Restart()
        let files = ids |> Array.map store.Get
        sw.Stop()
        let tget = sw.MicroTime / store.FileCount

        sw.Restart()
        for fi in 0..files.Length-1 do
            files.[fi].Write data
        sw.Stop()
        let twrite = sw.MicroTime / files.Length
        
        sw.Restart()
        for fi in 0..files.Length-1 do
            files.[fi].Write data2
        sw.Stop()
        let tupdate = sw.MicroTime / files.Length

        sw.Restart()
        for fi in 0..files.Length-1 do
            files.[fi].Delete()
        sw.Stop()
        let tdelete = sw.MicroTime / files.Length
        
        printfn "{ get = %A; write = %A; update = %A; delete = %A }" tget twrite tupdate tdelete
        File.AppendAllLines(path, [sprintf "%d;%d;%d;%d;%d" cnt tget.TotalNanoseconds twrite.TotalNanoseconds tupdate.TotalNanoseconds tdelete.TotalNanoseconds])



let perf() =
    let path = @"C:\Users\schorsch\Desktop\test.bin"
    if File.Exists path then File.Delete path
    let sw = Stopwatch()
    let mem = Memory.mapped path

    let ids = Array.init (1 <<< 20) (fun i -> i |> string |> md5)

    Log.startTimed "import"
    sw.Restart()
    use store = new Store(mem)
    sw.Stop()
    if store.FileCount > 0 then
        Log.line "files:        %d" store.FileCount
        Log.line "memory:       %A" store.DataSize
        Log.line "fileTable:    %A" store.FileTableSize
        Log.line "per file:     %A" (sw.MicroTime / store.FileCount)
    Log.stop()

    Log.startTimed "get"
    sw.Restart()
    let files = ids |> Array.map store.Get
    sw.Stop()
    Log.line "per file:     %A" (sw.MicroTime / store.FileCount)
    Log.stop()


    let data : byte[] = Array.zeroCreate 128
    let data2 : byte[] = Array.init 128 byte

    Log.startTimed "write"
    sw.Restart()
    for fi in 0..files.Length-1 do
        files.[fi].Write data
    sw.Stop()
    Log.line "per file:     %A" (sw.MicroTime / files.Length)
    Log.stop()

    Log.startTimed "update"
    sw.Restart()
    for fi in 0..files.Length-1 do
        files.[fi].Write data2
    sw.Stop()
    Log.line "per file:     %A" (sw.MicroTime / files.Length)
    Log.stop()

    Log.startTimed "delete"
    sw.Restart()
    for fi in 0..files.Length-1 do
        files.[fi].Delete()
    sw.Stop()
    Log.line "per file:     %A" (sw.MicroTime / files.Length)
    Log.stop()

    Log.startTimed "re-write"
    sw.Restart()
    for fi in 0..files.Length-1 do
        files.[fi].Write data
    sw.Stop()
    Log.line "per file:     %A" (sw.MicroTime / files.Length)
    Log.stop()

    ()


module FileSystem =
    open System.Runtime.InteropServices

    [<Literal>]
    let KeySize = 16

    type Key = Guid

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Key =
        let empty = Guid.Empty

        let inline equals (a : Key) (b : Key) = a.Equals b
        
    [<StructLayout(LayoutKind.Sequential, Size = 96)>]
    type header =
        struct
            val mutable public Magic : Guid
            val mutable public Version : int
            val mutable public FileCount : int
            val mutable public FreeCount : int
            val mutable public FreeList : int
            val mutable public CapacityIndex : int
            val mutable public Capacity : int
            val mutable public BlockCount : int

            static member inline Size = 96n
        end

    [<StructLayout(LayoutKind.Sequential)>]
    type block =
        struct
            val mutable public Offset : int64
            val mutable public Length : int64
            val mutable public Next : int
            val mutable public Prev : int

            static member inline Size = 24n

        end

    [<StructLayout(LayoutKind.Sequential)>]
    type entry =
        struct
            val mutable public Value : block
            val mutable public HashCode : int
            val mutable public Next : int
            val mutable public Key : Key

            static member inline Size = 32n + nativeint KeySize
        end


    type pheader(p : ptr) = 
        inherit ptr<header>(p)

        member x.Magic
            with get()  = x.Read<Guid>()
            and set v   = x.Write<Guid>(v)

        member x.Version
            with get()  = (x :> ptr + 16n).Read<int>()
            and set v   = (x :> ptr + 16n).Write<int>(v)

        member x.FileCount
            with get()  = (x :> ptr + 20n).Read<int>()
            and set v   = (x :> ptr + 20n).Write<int>(v)

        member x.FreeCount
            with get()  = (x :> ptr + 24n).Read<int>()
            and set v   = (x :> ptr + 24n).Write<int>(v)

        member x.FreeList
            with get()  = (x :> ptr + 28n).Read<int>()
            and set v   = (x :> ptr + 28n).Write<int>(v)

        member x.CapacityIndex
            with get()  = (x :> ptr + 32n).Read<int>()
            and set v   = (x :> ptr + 32n).Write<int>(v)

        member x.Capacity
            with get()  = (x :> ptr + 36n).Read<int>()
            and set v   = (x :> ptr + 36n).Write<int>(v)

        member x.BlockCount
            with get()  = (x :> ptr + 40n).Read<int>()
            and set v   = (x :> ptr + 40n).Write<int>(v)

    type pblock(p : ptr) = 
        inherit ptr<block>(p)

        member private x.Pointer = p

        member x.Offset
            with get()  = x.Read<int64>()
            and set v   = x.Write<int64>(v)

        member x.Length
            with get()  = (x :> ptr + 8n).Read<int64>()
            and set v   = (x :> ptr + 8n).Write<int64>(v)

        member x.Next
            with get()  = (x :> ptr + 16n).Read<int>()
            and set v   = (x :> ptr + 16n).Write<int>(v)

        member x.Prev
            with get()  = (x :> ptr + 20n).Read<int>()
            and set v   = (x :> ptr + 20n).Write<int>(v)

        static member (+) (pb : pblock, r : int) = pblock(pb.Pointer + nativeint r * block.Size)
        static member (-) (pb : pblock, r : int) = pblock(pb.Pointer - nativeint r * block.Size)

    type pentry(p : ptr) = 
        inherit ptr<entry>(p)

        member x.Pointer = p


        member x.ValuePtr = pblock(p)

        member x.Value
            with get()  = x.Read<block>()
            and set v   = x.Write<block>(v)

        member x.HashCode
            with get()  = (x :> ptr + 24n).Read<int>()
            and set v   = (x :> ptr + 24n).Write<int>(v)

        member x.Next
            with get()  = (x :> ptr + 28n).Read<int>()
            and set v   = (x :> ptr + 28n).Write<int>(v)

        member x.Key
            with get()  = (x :> ptr + 32n).Read<Key>()
            and set v   = (x :> ptr + 32n).Write<Key>(v)

        static member (+) (pb : pentry, r : int) = pentry(pb.Pointer + nativeint r * entry.Size)
        static member (-) (pb : pentry, r : int) = pentry(pb.Pointer - nativeint r * entry.Size)
        static member inline (+) (pb : pentry, r : 'a) = pentry(pb.Pointer + nativeint r * entry.Size)
        static member inline (-) (pb : pentry, r : 'a) = pentry(pb.Pointer - nativeint r * entry.Size)


    type Store =
        {
            mem         : Memory
            header      : pheader
            entries     : pentry
            buckets     : ptr<int>
        }

    type Table() =
        let store : Store = failwith "asdasd"
        let header = store.header

        member x.Grow() =
            ()

        member x.FindEntry(key : Key, hashCode : int) =
            let index = hashCode % header.Capacity

            let id = store.buckets.[index] - 1
            if id < 0 then
                -1
            else
                let e = store.entries + id
                if e.HashCode = hashCode && Key.equals e.Key key then
                    id
                else
                    let next = e.Next
                    if next < 0 then
                        -1
                    else
                        let rec search (id : int) (e : pentry) =
                            if e.HashCode = hashCode && Key.equals e.Key key then
                                id
                            else
                                let next = e.Next
                                if next >= 0 then
                                    search next (store.entries + next)
                                else
                                    -1

                        search next (store.entries + next)


        member x.Alloc() =
            let freeCount = header.FreeCount
            if freeCount > 0 then
                let id = header.FreeList
                let entry = store.entries + id
                header.FreeList <- entry.Next
                header.FreeCount <- freeCount - 1
                entry.Next <- 0
                id
            else
                let count = header.BlockCount
                if count = header.Capacity then
                    x.Grow()

                header.BlockCount <- count + 1
                count

        member x.Free(eid : int) =
            let next = header.FreeList
            let e = store.entries + eid

            e.Value <- block()
            e.HashCode <- -1
            e.Next <- next
            e.Key <- Key.empty


            header.FreeList <- eid
            header.FreeCount <- header.FreeCount + 1
            
        member x.Get(eid : int) =
            store.entries + eid



    //[<CustomEquality; NoComparison>]
    type Block(t : Table, id : int) =
        
        member x.Id = id

        override x.GetHashCode() =
            id

        override x.Equals o =
            match o with
                | :? Block as o -> x.Id = o.Id
                | _ -> false

        member x.Offset
            with get() = t.Get(id).ValuePtr.Offset
            and set (n : int64) = t.Get(id).ValuePtr.Offset <- n

        member x.Size
            with get() = t.Get(id).ValuePtr.Length
            and set (n : int64) = t.Get(id).ValuePtr.Length <- n

        member x.Next
            with get() = Block(t, t.Get(id).ValuePtr.Next)
            and set (n : Block) = t.Get(id).ValuePtr.Next <- n.Id

        member x.Prev
            with get() = Block(t, t.Get(id).ValuePtr.Prev)
            and set (n : Block) = t.Get(id).ValuePtr.Prev <- n.Id

