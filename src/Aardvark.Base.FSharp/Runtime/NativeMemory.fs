namespace Aardvark.Base

open System
open System.Collections.Generic
open System.Threading
open System.Runtime.InteropServices
open Aardvark.Base
open Microsoft.FSharp.NativeInterop

#nowarn "9"

type FreeList<'k, 'v when 'k : comparison>() =
    static let comparer = { new IComparer<'k * HashSet<'v>> with member x.Compare((l,_), (r,_)) = compare l r }
    let sortedSet = SortedSetExt comparer
    let sets = Dictionary<'k, HashSet<'v>>()

    let tryGet (minimal : 'k) =
        let _, self, right = sortedSet.FindNeighbours((minimal, Unchecked.defaultof<_>))
    
        let fitting =
            if self.HasValue then Some self.Value
            elif right.HasValue then Some right.Value
            else None
        
        match fitting with
            | Some (k,container) -> 

                if container.Count <= 0 then
                    raise <| ArgumentException "invalid memory manager state"

                let any = container |> Seq.head
                container.Remove any |> ignore

                // if the container just got empty we remove it from the
                // sorted set and the cache-dictionary
                if container.Count = 0 then
                   sortedSet.Remove(k, container) |> ignore
                   sets.Remove(k) |> ignore

                Some any

            | None -> None

    let insert (k : 'k) (v : 'v) =
        match sets.TryGetValue k with
            | (true, container) ->
                container.Add(v) |> ignore
            | _ ->
                let container = HashSet [v]
                sortedSet.Add((k, container)) |> ignore
                sets.[k] <- container

    let remove (k : 'k) (v : 'v) =
        let _, self, _ = sortedSet.FindNeighbours((k, Unchecked.defaultof<_>))
   
        if self.HasValue then
            let (_,container) = self.Value

            if container.Count <= 0 then
                raise <| ArgumentException "invalid memory manager state"

            let res = container.Remove v

            // if the container just got empty we remove it from the
            // sorted set and the cache-dictionary
            if container.Count = 0 then
                sortedSet.Remove(k, container) |> ignore
                sets.Remove(k) |> ignore

            res
        else 
            false

    member x.TryGetGreaterOrEqual (minimal : 'k) = tryGet minimal
    member x.Insert (key : 'k, value : 'v) = insert key value
    member x.Remove (key : 'k, value : 'v) = remove key value
    member x.Clear() =
        sortedSet.Clear()
        sets.Clear()

module ReaderWriterLock =
    let read (l : ReaderWriterLockSlim) (f : unit -> 'a) =
        if l.IsReadLockHeld || l.IsWriteLockHeld then
            f()
        else
            try
                l.EnterReadLock()
                f()
            finally
                if l.IsReadLockHeld then
                    l.ExitReadLock()

    let write (l : ReaderWriterLockSlim) (f : unit -> 'a) =
        if l.IsWriteLockHeld then
            f()
        else
            try
                l.EnterWriteLock()
                f()
            finally
                if l.IsWriteLockHeld then
                    l.ExitWriteLock()


[<AllowNullLiteral>]
type MemoryBlock =
    class
        val mutable public Parent : MemoryManager
        val mutable public Offset : nativeint
        val mutable public Size : int
        val mutable public Prev : MemoryBlock
        val mutable public Next : MemoryBlock
        val mutable public Free : bool

        member x.Write(offset : int, source : nativeint, size : int) =
            if offset + size > x.Size then failwith "[Memory] write exceeding size"

            ReaderWriterLock.read x.Parent.PointerLock (fun () ->
                let target = x.Parent.Pointer + x.Offset
                Marshal.Copy(source, target, size)
            )

        member x.Read(offset : int, target : nativeint, size : int) =
            if offset + size > x.Size then failwith "[Memory] read exceeding size"

            ReaderWriterLock.read x.Parent.PointerLock (fun () ->
                let source = x.Parent.Pointer + x.Offset
                Marshal.Copy(source, target, size)
            )

        member x.Write(offset : int, data : 'a) =
            if offset + sizeof<'a> > x.Size then failwith "[Memory] write exceeding size"

            ReaderWriterLock.read x.Parent.PointerLock (fun () ->
                let ptr = x.Parent.Pointer + x.Offset |> NativePtr.ofNativeInt
                NativePtr.write ptr data
            )

        member x.Read(offset : int) : 'a =
            if offset + sizeof<'a> > x.Size then failwith "[Memory] read exceeding size"

            ReaderWriterLock.read x.Parent.PointerLock (fun () ->
                let ptr = x.Parent.Pointer + x.Offset |> NativePtr.ofNativeInt
                NativePtr.read ptr
            )

        member x.Write(offset : int, data : 'a[]) =
            if offset + sizeof<'a> * data.Length > x.Size then failwith "[Memory] write exceeding size"

            ReaderWriterLock.read x.Parent.PointerLock (fun () ->
                let mutable ptr = x.Parent.Pointer + x.Offset |> NativePtr.ofNativeInt
                for i in 0..data.Length-1 do
                    NativePtr.set ptr i data.[i]
            )

        member x.Read(offset : int, data : 'a[]) =
            if offset + sizeof<'a> * data.Length > x.Size then failwith "[Memory] read exceeding size"

            ReaderWriterLock.read x.Parent.PointerLock (fun () ->
                let mutable ptr = x.Parent.Pointer + x.Offset |> NativePtr.ofNativeInt
                for i in 0..data.Length-1 do
                    data.[i] <- NativePtr.get ptr i
            )

        member x.Read(offset : int, count : int) : 'a[] =
            if offset + sizeof<'a> * count > x.Size then failwith "[Memory] read exceeding size"

            let arr = Array.zeroCreate count
            x.Read(offset, arr)
            arr

        member x.Int8Array      : int8[]    = x.Read(0, x.Size)
        member x.Int16Array     : int16[]   = x.Read(0, x.Size / 2)
        member x.Int32Array     : int[]     = x.Read(0, x.Size / 4)
        member x.Int64Array     : int64[]   = x.Read(0, x.Size / 8)

        member x.UInt8Array     : uint8[]   = x.Read(0, x.Size)
        member x.UInt16Array    : uint16[]  = x.Read(0, x.Size / 2)
        member x.UInt32Array    : uint32[]  = x.Read(0, x.Size / 4)
        member x.UInt64Array    : uint64[]  = x.Read(0, x.Size / 8)



        override x.ToString() =
            if x.Size < 0 then "invalid"
            else sprintf "[%d:%d]" x.Offset (x.Offset + nativeint x.Size)

        new(parent, o,s,p,n,f) = { Parent = parent; Offset = o; Size = s; Prev = p; Next = n; Free = f }
    end

and MemoryManager(capacity : int, malloc : int -> nativeint, mfree : nativeint -> int -> unit) as this =
    let mutable capacity = capacity
    let mutable ptr = malloc capacity
    let freeList = FreeList<int, MemoryBlock>()
    let l = obj()
    let pointerLock = new ReaderWriterLockSlim()
    let structureLock = new ReaderWriterLockSlim()

    
    let mutable firstBlock = MemoryBlock(this, 0n, capacity, null, null, true)
    let mutable lastBlock = firstBlock
    do freeList.Insert(firstBlock.Size, firstBlock)

    let readPointer (f : unit -> 'a) = ReaderWriterLock.read pointerLock f
    let writePointer (f : unit -> 'a) = ReaderWriterLock.write pointerLock f
    let readStructure (f : unit -> 'a) = ReaderWriterLock.read structureLock f
    let writeStructure (f : unit -> 'a) = ReaderWriterLock.write structureLock f

    let destroy (b : MemoryBlock) =
        b.Offset <- -1n
        b.Size <- -1
        b.Prev <- null
        b.Next <- null
        b.Free <- false

    let swap (l : MemoryBlock) (r : MemoryBlock) =
        if isNull l.Prev then firstBlock <- r
        else l.Prev.Next <- r
        if isNull l.Next then lastBlock <- r
        else l.Next.Prev <- r

        if isNull r.Prev then firstBlock <- l
        else r.Prev.Next <- l
        if isNull r.Next then lastBlock <- l
        else r.Next.Prev <- l

        Fun.Swap(&l.Offset, &r.Offset)
        Fun.Swap(&l.Size, &r.Size)
        Fun.Swap(&l.Prev, &r.Prev)
        Fun.Swap(&l.Next, &r.Next)
        Fun.Swap(&l.Free, &r.Free)

    let free (b : MemoryBlock) : unit =
        assert (not (isNull b))

        if not b.Free && b.Size > 0 then
            writeStructure (fun () ->
                // merge b with its prev (if it's free)
                let prev = b.Prev
                if not (isNull prev) && prev.Free then 
                    if freeList.Remove(prev.Size, prev) then

                        // b now occupies the memory of both blocks
                        b.Offset <- prev.Offset
                        b.Size <- prev.Size + b.Size

                        // all links to prev now link to b
                        if isNull prev.Prev then firstBlock <- b
                        else prev.Prev.Next <- b

                        // b's prev now links wherever prev's prev linked to
                        b.Prev <- prev.Prev
                    
                        // destroy all of prev's fields
                        destroy prev

                    else
                        failwithf "[Memory] could not remove %A from freeList" prev

                // merge b with its next (if it's free)
                let next = b.Next
                if not (isNull next) && next.Free then 
                    if freeList.Remove(next.Size, next) then

                        // b now occupies the memory of both blocks
                        b.Size <- next.Size + b.Size

                        // all links to next now link to b
                        if isNull next.Next then lastBlock <- b
                        else next.Next.Prev <- b

                        // b's prev now links wherever prev's prev linked to
                        b.Next <- next.Next

                        // destroy all of next's fields
                        destroy next
                    else
                        failwithf "[Memory] could not remove %A from freeList" prev

                // tell b that it's free and add it to the freeList
                b.Free <- true
                freeList.Insert(b.Size, b)
            )

    let resize (additional : int) =
        assert (additional > 0)

        let oldCapacity = capacity
        let newCapacity = Fun.NextPowerOfTwo(capacity + additional)
        writePointer (fun () ->
            let n = malloc newCapacity
            Marshal.Copy(ptr, n, oldCapacity)

            mfree ptr capacity
            ptr <- n
            capacity <- newCapacity
        )

        writeStructure (fun () ->
            let newMemory = MemoryBlock(this, nativeint oldCapacity, newCapacity - oldCapacity, lastBlock, null, false)
            lastBlock.Next <- newMemory
            lastBlock <- newMemory

            free newMemory
        )

    let rec alloc (size : int) : MemoryBlock =
        if size <= 0 then
            MemoryBlock(this, 0n, 0, null, null, false)
        else
            match writeStructure (fun () -> freeList.TryGetGreaterOrEqual(size)) with
                | Some block ->
                    block.Free <- false

                    if block.Size > size then
                        writeStructure (fun () ->
                            let rest = MemoryBlock(this, block.Offset + nativeint size, block.Size - size, block, block.Next, false)

                            if isNull rest.Next then lastBlock <- rest
                            else rest.Next.Prev <- rest
                            block.Next <- rest

                            block.Size <- size

                            free rest
                        )

                    block
                | None ->
                    // if there was no block of sufficient size resize the entire
                    // memory and retry
                    resize size
                    alloc size

    let rec realloc (b : MemoryBlock) (size : int) : bool =
        writeStructure (fun () ->

            if b.Free then
                failwithf "[Memory] cannot realloc free block: %A" b

            elif size = 0 then
                // if a block gets re-allocated with size 0 it is now free
                
                let n = MemoryBlock(this, b.Offset, b.Size, b.Prev, b.Next, b.Free)

                if isNull b.Prev then firstBlock <- n
                else b.Prev.Next <- n
                if isNull b.Next then lastBlock <- n
                else b.Next.Prev <- n

                free n

                b.Offset <- 0n
                b.Size <- 0
                b.Prev <- null
                b.Next <- null
                b.Free <- false


                false

            elif size > b.Size then

                if isNull b.Next || not b.Next.Free || b.Next.Size + b.Size < size then
                    let n = alloc size

                    writePointer (fun () -> Marshal.Copy(ptr + b.Offset, ptr + n.Offset, b.Size))
                    swap n b

                    free n

                    // alloc a completely new block and copy the contents there
                    true

                else
                    let next = b.Next
                    if freeList.Remove(next.Size, next) then
                        next.Free <- false

                        let additionalSize = size - b.Size
                        b.Size <- size

                        if next.Size > additionalSize then
                            // if next is larger than needed we free the rest
                            next.Offset <- b.Offset + nativeint size
                            next.Size <- next.Size - additionalSize

                            free next
                        else
                            // if next fits our requirements exactly it is deleted from the
                            // linked list and finally destroyed
                            b.Next <- next.Next
                            if isNull next.Next then lastBlock <- b
                            else next.Next.Prev <- b

                            destroy next


                        false
                    else 
                        failwith "[Memory] could not remove free block from freeList"

            elif size < b.Size then
                // if the block "shrinked" we can simply create and free the
                // leftover memory
                let rest = MemoryBlock(this, b.Offset + nativeint size, b.Size - size, b, b.Next, false)
                b.Size <- size

                b.Next <- rest
                if isNull rest.Next then lastBlock <- rest
                else rest.Next.Prev <- rest

                free rest

                false

            else
                false
        )

    let spill (b : MemoryBlock) : MemoryBlock =
        if b.Free then failwithf "[Memory] cannot spill free block: %A" b

        writeStructure (fun () ->
            let n = alloc size

            writePointer (fun () -> Marshal.Copy(ptr + b.Offset, ptr + n.Offset, b.Size))
            swap n b

            n
        )

    member x.Pointer : nativeint = ptr
    member internal x.StructureLock : ReaderWriterLockSlim = structureLock
    member internal x.PointerLock : ReaderWriterLockSlim = pointerLock

    member x.FirstBlock = firstBlock
    member x.LastBlock = lastBlock

    member x.Dispose() =
        if ptr <> 0n then
            mfree ptr capacity
            ptr <- 0n
            capacity <- 0
            freeList.Clear()
            pointerLock.Dispose()
            structureLock.Dispose()
            firstBlock <- null
            lastBlock <- null

    member x.Alloc (size : int) = lock l (fun () -> alloc size)
    member x.Free (block : MemoryBlock) = 
        lock l (fun () -> 
            let b = MemoryBlock(this, 0n, 0, block.Prev, block.Next, true)
            swap b block

            if isNull b.Prev then firstBlock <- b
            else b.Prev.Next <- b

            if isNull b.Next then lastBlock <- b
            else b.Next.Prev <- b

            free b
        )

    member x.Realloc (block : MemoryBlock, size : int) = lock l (fun () -> realloc block size)
    member x.Spill (block : MemoryBlock) = lock l (fun () -> spill block)

    interface IDisposable with
        member x.Dispose() = x.Dispose()

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module MemoryManager =
    
    let createHGlobal() = new MemoryManager(16, Marshal.AllocHGlobal, fun ptr _ -> Marshal.FreeHGlobal ptr)
    let createCoTaskMem() = new MemoryManager(16, Marshal.AllocCoTaskMem, fun ptr _ -> Marshal.FreeCoTaskMem ptr)
    let createExecutable() = new MemoryManager(16, ExecutableMemory.alloc, ExecutableMemory.free)

    let inline alloc (size : int) (m : MemoryManager) =
        m.Alloc size

    let inline free (b : MemoryBlock) (m : MemoryManager) =
        m.Free b

    let inline realloc (b : MemoryBlock) (size : int) (m : MemoryManager) =
        m.Realloc(b, size)

    let inline spill (b : MemoryBlock) (m : MemoryManager) =
        m.Spill b

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module MemoryBlock =
    let inline parent (b : MemoryBlock) = b.Parent
    let inline size (b : MemoryBlock) = b.Size
    let inline offset (b : MemoryBlock) = b.Offset
    let inline prev (b : MemoryBlock) = b.Prev
    let inline next (b : MemoryBlock) = b.Next
    let inline isFree (b : MemoryBlock) = b.Free

    let inline realloc (size : int) (b : MemoryBlock) =
        b.Parent.Realloc(b, size)

    let inline free (b : MemoryBlock) =
        b.Parent.Free(b)

    let inline spill (b : MemoryBlock) =
        b.Parent.Spill(b)
