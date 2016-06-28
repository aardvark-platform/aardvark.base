module StoreTest

open System
open System.Text
open System.Diagnostics
open System.Security.Cryptography
open Aardvark.Base
open Aardvark.Base.Native
open System.IO
open Microsoft.FSharp.NativeInterop

#nowarn "9"

let hash = MD5.Create()
let md5 (str : string) =
    str |> Encoding.Unicode.GetBytes |> hash.ComputeHash |> Guid

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

    Log.line "memory:       %A" store.DataSize
    Log.line "fileTable:    %A" store.FileTableSize
    if store.FileCount > 0 then
        Log.line "files:        %d" store.FileCount
        Log.line "per file:     %A" (sw.MicroTime / store.FileCount)
    Log.stop()

    Log.startTimed "get"
    sw.Restart()
    let files = ids |> Array.map store.Get
    sw.Stop()
    Log.line "per file:     %A" (sw.MicroTime / files.Length)
    Log.stop()


    let data : byte[] = Array.zeroCreate 44

    Log.startTimed "write"
    sw.Restart()
    for fi in 0..files.Length-1 do
        files.[fi].Write data
    sw.Stop()
    Log.line "per file:     %A" (sw.MicroTime / files.Length)
    Log.stop()




    ()




module FS = 

    let inline (++) (ptr : nativeptr<'a>) (v : 'a) = NativePtr.add ptr (int v)
    let inline (!!) (ptr : nativeptr<'a>) = NativePtr.read ptr
    let inline (<--) (ptr : nativeptr<'a>) c = NativePtr.write ptr c

    type Block =
        struct
            val mutable public Offset   : int64
            val mutable public Size     : int64
            val mutable public IsFree   : int
            val mutable public Prev     : int
            val mutable public Next     : int

        end

    type Entry =
        struct
            val mutable public Block : Block
            val mutable public HashCode : int
            val mutable public Next : int
            val mutable public Key : Guid
        end

    type Node =
        struct  
            val mutable public Size : int
            val mutable public Entry : int
            val mutable public Left : int
            val mutable public Right : int
            val mutable public Parent : int

        end


    type blockptr =
        struct
            val mutable public ptr : nativeint

            static member inline (+) (ptr : blockptr, index : int) = blockptr(ptr.ptr + nativeint (sizeof<Block> * index))

            member inline x.Offset      = NativePtr.ofNativeInt<int64>  x.ptr
            member inline x.Size        = NativePtr.ofNativeInt<int64>  (8n + x.ptr)
            member inline x.IsFree      = NativePtr.ofNativeInt<int>    (16n + x.ptr)
            member inline x.Prev        = NativePtr.ofNativeInt<int>    (20n + x.ptr)
            member inline x.Next        = NativePtr.ofNativeInt<int>    (24n + x.ptr)
 
            new(p) = { ptr = p }

        end

    type entryptr =
        struct
            val mutable public ptr : nativeint

            member inline x.Block       = blockptr x.ptr
            member inline x.HashCode    = NativePtr.ofNativeInt<int> (28n + x.ptr)
            member inline x.Next        = NativePtr.ofNativeInt<int> (32n + x.ptr)
            member inline x.Key         = NativePtr.ofNativeInt<Guid> (36n + x.ptr)

            static member inline (+) (ptr : entryptr, index : int) = entryptr(ptr.ptr + nativeint (sizeof<Entry> * index))

            new(p) = { ptr = p }
        end

    type nodeptr =
        struct
            val mutable public ptr : nativeint

            member inline x.Size        = NativePtr.ofNativeInt<int> (x.ptr)
            member inline x.Entry       = NativePtr.ofNativeInt<int> (4n + x.ptr)
            member inline x.Left        = NativePtr.ofNativeInt<int> (8n + x.ptr)
            member inline x.Right       = NativePtr.ofNativeInt<int> (12n + x.ptr)
            member inline x.Parent      = NativePtr.ofNativeInt<int> (16n + x.ptr)


            static member inline (+) (ptr : nodeptr, index : int) = nodeptr(ptr.ptr + nativeint (sizeof<Node> * index))

            new(p) = { ptr = p }
        end



    type Store =
        class
            
            val mutable public First    : int
            val mutable public Last     : int
            val mutable public Root     : int
            val mutable public Capacity : int

            val mutable public nodes    : nodeptr
            val mutable public FreeList : nativeptr<int>
            val mutable public Entries  : entryptr
            val mutable public Buckets  : nativeptr<int>

            member x.RemoveNode(ptr : nodeptr) =
                failwith "implement me"

            member x.AddNode(ptr : Node) : nodeptr =
                failwith "implement me"
                

            member x.GrowTable() =
                failwith "implement me"

            member x.GrowData(needed : int64) =
                failwith "implement me"


            member x.GetFree(size : int64) =
                let mutable best = nodeptr(0n)
                let mutable current = x.Root

                // search for the smallest node >= size
                while current >= 0 && current < x.Capacity do
                    let pNode = x.nodes + current

                    let c = compare (int size) !!pNode.Size

                    if c > 0 then
                        current <- !!pNode.Right
                    elif c < 0 then
                        best <- pNode
                        current <- !!pNode.Left
                    else
                        best <- pNode
                        current <- -1

                // if we found a best-fit use it (removing it from the tree)
                if best.ptr <> 0n then
                    let e = !!best.Entry
                    let pEntry = x.Entries + e

                    let n = !!pEntry.Next
                    if n >= 0 then
                        best.Entry <-- n
                    else
                        x.RemoveNode best

                    pEntry
                
                // otherwise increase memory-size and retry
                else
                    x.GrowData size
                    x.GetFree size

            member x.Alloc(size : int64) =
                let pEntry = x.GetFree(size)
                let pBlock = pEntry.Block
                if !!pBlock.Size > size then
                    failwith "split me"

                pBlock.Size <-- size
                pBlock.IsFree <-- 0

                pEntry

            member x.NewFile(key : Guid, size : int64) =
                let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
                let pEntry = x.Alloc size
                pEntry.Key <-- key
                pEntry.HashCode <-- hashCode

                let index = (pEntry.ptr - x.Entries.ptr) / nativeint sizeof<Entry> |> int
                let pBucket = x.Buckets ++ hashCode % x.Capacity
                let ei = !!pBucket - 1

                if ei < 0 then
                    pBucket <-- index + 1
                else
                    pEntry.Next <-- ei
                    pBucket <-- index + 1

            member x.FindEntry(key : Guid, hashCode : int) =
                let pBucket = x.Buckets ++ hashCode % x.Capacity
                let ei = !!pBucket - 1

                if ei < 0 then 
                    -1
                else
                    let pEntry = x.Entries + ei
                    if !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
                        ei
                    else
                        let rec search (id : int) (pEntry : entryptr) =
                            if id < 0 then
                                -1
                            elif !!pEntry.HashCode = hashCode && !!pEntry.Key = key then
                                id
                            else
                                let n = !!pEntry.Next
                                search n (x.Entries + n)

                        let n = !!pEntry.Next
                        search n (x.Entries + n)

            member x.Delete(key : Guid) =
                let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
                let id = x.FindEntry(key, hashCode)
                if id >= 0 then
                    


                    true
                else
                    false

        end

