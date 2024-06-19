namespace Aardvark.Base

open System
open System.Collections.Generic
open System.Threading
open System.Runtime.InteropServices
open Aardvark.Base
open Microsoft.FSharp.NativeInterop

#nowarn "9"
#nowarn "44"

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

    let contains (k : 'k) (v : 'v) =
        let _, self, _ = sortedSet.FindNeighbours((k, Unchecked.defaultof<_>))
   
        if self.HasValue then
            let (_,container) = self.Value
            container.Contains v
        else 
            false


    member x.Values =
        seq {
            for _,s in sortedSet do
                yield! s
        }


    member x.TryGetGreaterOrEqual (minimal : 'k) = tryGet minimal
    member x.Insert (key : 'k, value : 'v) = insert key value
    member x.Remove (key : 'k, value : 'v) = remove key value
    member x.Contains (key : 'k, value : 'v) = contains key value
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


type MemoryManagerConfig =
    {
        malloc : nativeint -> nativeint
        mfree : nativeint -> nativeint -> unit
        mcopy : nativeint -> nativeint -> nativeint -> unit

    }


[<AllowNullLiteral>]
type internal Block =
    class
        val mutable public Parent : MemoryManager
        val mutable public Offset : nativeint
        val mutable public Size : nativeint
        val mutable public Prev : Block
        val mutable public Next : Block
        val mutable public Free : bool

        override x.ToString() =
            if x.Size < 0n then "invalid"
            else sprintf "[%d:%d]" x.Offset (x.Offset + x.Size)


        static member Invalid = Block(Unchecked.defaultof<_>, 0n, 0n, null, null, true)

        new(parent, o,s,p,n,f) = { Parent = parent; Offset = o; Size = s; Prev = p; Next = n; Free = f }
    end

and [<AllowNullLiteral>] managedptr internal(block : Block) =
    let mutable block = block

    #if DEBUG
    let check() =
        if isNull block then raise <| ObjectDisposedException("MemoryBlock")
    #else
    let check() = ()
    #endif


    member x.Parent = check(); block.Parent
    member x.Size = if isNull block then 0n else block.Size
    member x.Offset = if isNull block then 0n else block.Offset
    member x.Free = if isNull block then true else block.Free

    override x.ToString() = 
        if isNull block then "(null)"
        else string block

    member internal x.Block
        with get() = block
        and set b = block <- b

    member x.Use(action : nativeint -> 'r) =
        ReaderWriterLock.read block.Parent.PointerLock (fun () ->
            action (block.Parent.Pointer + block.Offset)
        )

    member x.Write(offset : int, source : nativeint, size : int) =
        check()
        if nativeint (offset + size) > block.Size then failwith "[Memory] write exceeding size"

        ReaderWriterLock.read block.Parent.PointerLock (fun () ->
            let target = block.Parent.Pointer + block.Offset + nativeint offset
            Marshal.Copy(source, target, size)
        )

    member x.Read(offset : int, target : nativeint, size : int) =
        check()
        if nativeint (offset + size) > block.Size then failwith "[Memory] read exceeding size"

        ReaderWriterLock.read block.Parent.PointerLock (fun () ->
            let source = block.Parent.Pointer + block.Offset + nativeint offset
            Marshal.Copy(source, target, size)
        )

    member x.Write(offset : int, data : 'a) =
        check()
        if nativeint (offset + sizeof<'a>) > block.Size then failwith "[Memory] write exceeding size"

        ReaderWriterLock.read block.Parent.PointerLock (fun () ->
            let ptr = block.Parent.Pointer + block.Offset + nativeint offset |> NativePtr.ofNativeInt
            NativePtr.write ptr data
        )

    member x.Read(offset : int) : 'a =
        check()
        if nativeint (offset + sizeof<'a>) > block.Size then failwith "[Memory] read exceeding size"

        ReaderWriterLock.read block.Parent.PointerLock (fun () ->
            let ptr = block.Parent.Pointer + block.Offset + nativeint offset |> NativePtr.ofNativeInt
            NativePtr.read ptr
        )

    member x.Write(offset : int, data : 'a[]) =
        check()
        if nativeint (offset + sizeof<'a> * data.Length) > block.Size then failwith "[Memory] write exceeding size"

        ReaderWriterLock.read block.Parent.PointerLock (fun () ->
            let mutable ptr = block.Parent.Pointer + block.Offset + nativeint offset |> NativePtr.ofNativeInt
            for i in 0..data.Length-1 do
                NativePtr.set ptr i data.[i]
        )


    member x.Read(offset : int, data : 'a[]) =
        check()
        if nativeint (offset + sizeof<'a> * data.Length) > block.Size then failwith "[Memory] read exceeding size"

        ReaderWriterLock.read block.Parent.PointerLock (fun () ->
            let mutable ptr = block.Parent.Pointer + block.Offset + nativeint offset |> NativePtr.ofNativeInt
            for i in 0..data.Length-1 do
                data.[i] <- NativePtr.get ptr i
        )

    member x.Read(offset : int, count : int) : 'a[] =
        check()
        if offset + sizeof<'a> * count > int block.Size then failwith "[Memory] read exceeding size"

        let arr = Array.zeroCreate count
        x.Read(offset, arr)
        arr

    member x.Move(sourceOffset : int, targetOffset : int, length : int) =
        check()
        ReaderWriterLock.read block.Parent.PointerLock (fun () ->
            Marshal.Move(
                x.Parent.Pointer + x.Offset + nativeint sourceOffset,
                x.Parent.Pointer + x.Offset + nativeint targetOffset,
                length
            )
        )

    member x.CopyTo(target : managedptr) =
        check()
        ReaderWriterLock.read target.Parent.PointerLock (fun () ->
            ReaderWriterLock.read block.Parent.PointerLock (fun () ->
                Marshal.Move(
                    block.Parent.Pointer + block.Offset,
                    target.Parent.Pointer + target.Offset,
                    min block.Size target.Size
                )
            )
        )

    member x.Int8Array      : int8[]    = x.Read(0, int block.Size)
    member x.Int16Array     : int16[]   = x.Read(0, int block.Size / 2)
    member x.Int32Array     : int[]     = x.Read(0, int block.Size / 4)
    member x.Int64Array     : int64[]   = x.Read(0, int block.Size / 8)

    member x.UInt8Array     : uint8[]   = x.Read(0, int block.Size)
    member x.UInt16Array    : uint16[]  = x.Read(0, int block.Size / 2)
    member x.UInt32Array    : uint32[]  = x.Read(0, int block.Size / 4)
    member x.UInt64Array    : uint64[]  = x.Read(0, int block.Size / 8)

and MemoryManager(capacity : nativeint, config : MemoryManagerConfig) as this =
    let mutable capacity = capacity
    let mutable allocated = 0n

    let mutable ptr = config.malloc capacity
    let freeList = FreeList<nativeint, Block>()
    let l = obj()
    let pointerLock = new ReaderWriterLockSlim()
   
    
    let mutable firstBlock = Block(this, 0n, capacity, null, null, true)
    let mutable lastBlock = firstBlock
    do freeList.Insert(firstBlock.Size, firstBlock)

    let readPointer (f : unit -> 'a) = ReaderWriterLock.read pointerLock f
    let writePointer (f : unit -> 'a) = ReaderWriterLock.write pointerLock f

    let validation () =
        let mutable last = null
        let mutable current = firstBlock
        while not (isNull current) do
            assert (current.Prev = last)

            assert(current.Size > 0n)
            assert(current.Offset >= 0n)

            if not (isNull last) then
                assert(last.Offset + nativeint last.Size = current.Offset)

                if last.Free then
                    assert(last.Free <> current.Free)
                    assert(freeList.Contains(last.Size, last))
                else
                    assert(not <| freeList.Contains(last.Size, last))
            last <- current
            current <- current.Next

    let free (b : Block) : unit =
        assert (not (isNull b))

        if not b.Free && b.Size > 0n then
            allocated <- allocated - b.Size
            // merge b with its prev (if it's free)
            let prev = b.Prev
            if not (isNull prev) && prev.Free then 
                assert(prev.Next = b)
                assert(prev.Offset + nativeint prev.Size = b.Offset)

                if freeList.Remove(prev.Size, prev) then
                    // b now occupies the memory of both blocks
                    b.Offset <- prev.Offset
                    b.Size <- prev.Size + b.Size

                    // all links to prev now link to b
                    if isNull prev.Prev then firstBlock <- b
                    else prev.Prev.Next <- b

                    // b's prev now links wherever prev's prev linked to
                    b.Prev <- prev.Prev

                    assert(isNull firstBlock.Prev)

                else
                    failwithf "[Memory] could not remove %A from freeList" prev

            // merge b with its next (if it's free)
            let next = b.Next
            if not (isNull next) && next.Free then 
                assert(next.Prev = b)
                assert(next.Offset = b.Offset + nativeint b.Size)

                if freeList.Remove(next.Size, next) then

                    // b now occupies the memory of both blocks
                    b.Size <- next.Size + b.Size

                    // all links to next now link to b
                    if isNull next.Next then lastBlock <- b
                    else next.Next.Prev <- b

                    // b's prev now links wherever prev's prev linked to
                    b.Next <- next.Next

                    assert(isNull lastBlock.Next)

                else
                    failwithf "[Memory] could not remove %A from freeList" prev

            // tell b that it's free and add it to the freeList
            b.Free <- true
            freeList.Insert(b.Size, b)

    let resize (additional : nativeint) =
        assert (additional > 0n)

        let oldCapacity = capacity
        let newCapacity = Fun.NextPowerOfTwo(int64 (capacity + additional)) |> nativeint
        writePointer (fun () ->
            let n = config.malloc newCapacity
            config.mcopy ptr n oldCapacity

            config.mfree ptr capacity
            ptr <- n
            capacity <- newCapacity
        )

        let newMemory = Block(this, nativeint oldCapacity, newCapacity - oldCapacity, lastBlock, null, false)
        lastBlock.Next <- newMemory
        lastBlock <- newMemory
        allocated <- allocated + newMemory.Size

        free newMemory


    let rec alloc (size : nativeint) : Block =
        if size <= 0n then
            null
        else
            match freeList.TryGetGreaterOrEqual(size) with
                | Some block ->
                    block.Free <- false
                    allocated <- allocated + block.Size

                    if block.Size > size then
                        let rest = Block(this, block.Offset + size, block.Size - size, block, block.Next, false)

                        if isNull block.Next then lastBlock <- rest
                        else block.Next.Prev <- rest

                        assert(isNull lastBlock.Next)

                        block.Next <- rest
                        block.Size <- size

                        free rest

                    block
                | None ->
                    // if there was no block of sufficient size resize the entire
                    // memory and retry
                    resize size
                    alloc size
        
    let rec realloc (b : Block) (size : nativeint) : bool * Block =

        if b.Size = size then
            false, b

        elif b.Free then
            failwithf "[Memory] cannot realloc free block: %A" b

        elif size > b.Size then
            if not (isNull b.Next) && b.Next.Free && isNull b.Next.Next && b.Next.Size + b.Size < size then
                
                resize (size - b.Next.Size - b.Size)
                realloc b size

            elif isNull b.Next || not b.Next.Free || b.Next.Size + b.Size < size then
                // alloc a completely new block and copy the contents there
                let n = alloc size
                readPointer (fun () -> config.mcopy (ptr + b.Offset) (ptr + n.Offset) b.Size)

                free b

                true, n

            else
                let next = b.Next
                assert(next.Offset = b.Offset + b.Size)

                if freeList.Remove(next.Size, next) then
                    allocated <- allocated + next.Size
                    next.Free <- false

                    let additionalSize = size - b.Size
                    b.Size <- size


                    if next.Size > additionalSize then
                        // if next is larger than needed we free the rest
                        next.Offset <- b.Offset + nativeint size
                        next.Size <- next.Size - additionalSize

                        assert(next.Offset = b.Offset + b.Size)

                        free next
                    else
                        // if next fits our requirements exactly it is deleted from the
                        // linked list and finally destroyed
                        b.Next <- next.Next
                        if isNull next.Next then lastBlock <- b
                        else next.Next.Prev <- b

                        assert(isNull lastBlock.Next)
                        //destroy next


                    false, b
                else 
                    failwith "[Memory] could not remove free block from freeList"

        else (* if size < b.Size then *)
            // if the block "shrinked" we can simply create and free the
            // leftover memory
            let rest = Block(this, b.Offset + size, b.Size - size, b, b.Next, false)
            b.Size <- size

            if isNull b.Next then lastBlock <- rest
            else b.Next.Prev <- rest
            b.Next <- rest

            assert(isNull lastBlock.Next)
            assert(b.Offset + b.Size = rest.Offset)

            free rest

            false, b

    let allocAligned (align : nativeint) (size : nativeint) : Block =
        let maxSize = size + align - 1n
        let block = alloc maxSize
        
        let prev = block.Prev
        let next = block.Next


        let r = block.Offset % align
        let alignedOffset = 
            if r = 0n then block.Offset
            else align + block.Offset - r

        let leftOffset = block.Offset
        let leftSize =
            if r = 0n then 0n
            else align - r

        let rightOffset = alignedOffset + size
        let rightSize = maxSize - size - leftSize
        

        assert (leftSize + size + rightSize = maxSize)
        assert (leftOffset + leftSize = alignedOffset)

        if leftSize <> 0n then
            let l = Block(this, leftOffset, leftSize, prev, block, false)

            if isNull prev then firstBlock <- l
            else prev.Next <- l
            block.Prev <- l

            block.Offset <- alignedOffset
            block.Size <- block.Size - leftSize
            free l

        if rightSize <> 0n then
            let r = Block(this, rightOffset, rightSize, block, next, false)

            if isNull next then lastBlock <- r
            else next.Prev <- r
            block.Next <- r

            block.Size <- block.Size - rightSize
            free r

        assert (block.Size = size)
        assert (block.Offset % align = 0n)
        block





    let clone (b : Block) : Block =
        if b.Free then failwithf "[Memory] cannot spill free block: %A" b

        let n = alloc b.Size
        readPointer (fun () -> Marshal.Copy(ptr + b.Offset, ptr + n.Offset, b.Size))
        n

    member x.Pointer : nativeint = ptr
    member x.PointerLock : ReaderWriterLockSlim = pointerLock

    member x.Capacity = capacity
    member x.AllocatedBytes = allocated
    member x.FreeBytes = capacity - allocated

    member x.FreeList =
        freeList.Values |> Seq.map (fun b -> managedptr(b))

    member x.FirstUsedByte = 
        if firstBlock.Free then firstBlock.Offset + nativeint firstBlock.Size
        else firstBlock.Offset

    member x.LastUsedByte =
        if lastBlock.Free then lastBlock.Offset - 1n
        else lastBlock.Offset + nativeint lastBlock.Size - 1n

    member x.Dispose() =
        if ptr <> 0n then
            config.mfree ptr capacity
            ptr <- 0n
            capacity <- 0n
            allocated <- 0n
            freeList.Clear()
            pointerLock.Dispose()
            firstBlock <- null
            lastBlock <- null

    member x.Alloc (size : nativeint) = 
        lock l (fun () ->
            let block = alloc size
            managedptr block
        )

    member x.AllocAligned (align : nativeint, size : nativeint) = 
        lock l (fun () ->
            let block = allocAligned align size
            managedptr block
        )

    member x.Free (ptr : managedptr) = 
        lock l (fun () -> 
            if not (isNull ptr.Block) then
                ptr.Block |> free
                ptr.Block <- null
        )

    member x.Realloc (ptr : managedptr, size : nativeint) = 
        lock l (fun () -> 
            if not (isNull ptr.Block) then
                if size <= 0n then
                    free ptr.Block
                    ptr.Block <- null
                    false
                else
                    let (moved, newBlock) = realloc ptr.Block size
                    ptr.Block <- newBlock
                    moved
            else
                if size <= 0n then false
                else failwith "[Memory] cannot realloc free managedptr"
        )

    member x.Spill (ptr : managedptr) = 
        lock l (fun () -> 
            if not (isNull ptr.Block) then
                let block = ptr.Block
                let cloned = clone block
                ptr.Block <- cloned
                managedptr(block)
            else
                failwith "[Memory] cannot spill free managedptr"
        )

    member x.Validate() =
        lock l (fun () ->
            validation()
        )

    member x.Lock = l

    new(cap,malloc,mfree) = new MemoryManager(cap, { malloc = malloc; mfree = mfree; mcopy = fun a b s -> Marshal.Copy(a, b, s)})

    interface IDisposable with
        member x.Dispose() = x.Dispose()



[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module MemoryManager =
    let createHGlobal() = new MemoryManager(16n, Marshal.AllocHGlobal, fun ptr _ -> Marshal.FreeHGlobal ptr)

    let private nopConfig = { malloc = (fun _ -> 0n); mfree = (fun _ _ -> ()); mcopy = (fun _ _ _ -> ()) }
    let createNop() = new MemoryManager(16n, nopConfig)

    let inline alloc (size : nativeint) (m : MemoryManager) =
        m.Alloc size

    let inline free (b : managedptr) (m : MemoryManager) =
        m.Free b

    let inline realloc (b : managedptr) (size : nativeint) (m : MemoryManager) =
        m.Realloc(b, size)

    let inline spill (b : managedptr) (m : MemoryManager) =
        m.Spill b

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module ManagedPtr =
    let inline parent (b : managedptr) = b.Parent
    let inline size (b : managedptr) = b.Size
    let inline offset (b : managedptr) = b.Offset
    let inline isFree (b : managedptr) = b.Free

    let inline realloc (size : nativeint) (b : managedptr) =
        if b.Free then
            if size <> 0n then failwith "[Memory] cannot realloc free managedptr"
            false
        else
            b.Parent.Realloc(b, size)

    let inline free (b : managedptr) =
        if not b.Free then
            b.Parent.Free(b)

    let inline spill (b : managedptr) =
        if b.Free then
            failwith "[Memory] cannot spill free managedptr"
        else
            b.Parent.Spill(b)

    let inline write (offset : int) (value : 'a) (b : managedptr) =
        b.Write(offset, value)

    let inline writeArray (offset : int) (value : 'a[]) (b : managedptr) =
        b.Write(offset, value)

    let inline read (offset : int) (b : managedptr) =
        b.Read(offset)

    let inline readArray (offset : int) (b : managedptr) : 'a[] =
        b.Read(offset, int (b.Size / nativeint sizeof<'a>))

    let inline move (sourceOffset : int) (targetOffset : int) (length : int) (b : managedptr) =
        b.Move(sourceOffset, targetOffset, length)



