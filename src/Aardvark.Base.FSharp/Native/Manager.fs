namespace Aardvark.Base.Native

open System
open System.Threading
open System.Runtime.CompilerServices
open Aardvark.Base


type MemoryManager(store : Memory) as this =
    let mutable capacity = store.Size
    let mutable allocated = 0L
    let free = FreeList<int64, Block>()

    let mutable first : Block = null
    let mutable last : Block = null

    do
        if store.Size <> 0L then
            let b = Block(this, 0L, store.Size)
            free.Insert(b.Size, b)
            first <- b
            last <- b

    let mutable pointer = new ReaderWriterLockSlim()

    member x.ReadEntries(entries : array<'a * int64 * int64>, newBlock : 'a -> Block -> unit) =
        lock x (fun () ->
            if entries.Length > 0 then
                let (_,lo,ls) = entries.[entries.Length-1]
                let maxUsed = lo + ls
                x.Resize(false, maxUsed + 1L)
                free.Clear()

                let mutable current = first
                let mutable offset = first.Size
                for (name, off, size) in entries do
                    if off <> offset then
                        let between = Block(x, offset, off - offset, Prev = current, Next = null, IsFree = true)
                        free.Insert(between.Size, between)
                        offset <- off
                        current <- between

                    let b = Block(x, off, size, Prev = current, Next = null, IsFree = false)
                    current.Next <- b
                    offset <- offset + size
                    allocated <- allocated + size

                    newBlock name b

                    current <- b

                let off = current.Offset + current.Size
                if off < capacity then
                    let between = Block(x, off, capacity - off, Prev = current, Next = null, IsFree = true)
                    free.Insert(between.Size, between)
                    last <- between
                else
                    last <- current


        )

    member x.FillRate = float allocated / float capacity

    member x.Allocated = allocated
    member x.Capacity = capacity


    member x.Compact() =
        lock x (fun () ->
            let mutable offset = 0L
            let mutable prev = null
            let mutable current = first
            free.Clear()

            while not (isNull current) do
                if not current.IsFree then
                    let pSource = store + current.Offset
                    let pTarget = store + offset

                    let temp = Array.zeroCreate (int current.Size)
                    pSource.Read(temp, 0L, temp.LongLength)
                    //mem.Read(store.Value, current.Offset, temp, 0L, temp.LongLength)

                    current.Offset <- offset
                    current.Prev <- prev
                    if isNull prev then first <- current
                    else prev.Next <- current

                    pTarget.Write(temp, 0L, temp.LongLength)
                    offset <- offset + current.Size


                prev <- current
                current <- current.Next

            let off = current.Offset + current.Size
            if off < capacity then
                let between = Block(x, off, capacity - off, Prev = current, Next = null, IsFree = true)
                free.Insert(between.Size, between)
                last <- between
            else
                last <- current

            x.Resize(true, off)
                

        )

    member private x.Swap(l : Block, r : Block) =
        let l,r =
            if l.Offset < r.Offset then l,r
            else r,l

        Fun.Swap(&l.Offset, &r.Offset)
        Fun.Swap(&l.Size, &r.Size)

        if l.IsFree || r.IsFree then
            failwith "[MemoryManager] cannot swap free blocks"

        if l.Next = r then
            if isNull l.Prev then 
                first <- r
                r.Prev <- null
            else 
                r.Prev <- l.Prev

            if isNull r.Next then 
                last <- l
                l.Next <- null
            else 
                l.Next <- r.Next

            r.Next <- l
            l.Prev <- r
        else
            Fun.Swap(&l.Next, &r.Next)
            Fun.Swap(&l.Prev, &r.Prev)

            if isNull l.Next then last <- l
            else l.Next.Prev <- l
            if isNull l.Prev then first <- l
            else l.Prev.Next <- l


            if isNull r.Next then last <- r
            else r.Next.Prev <- r
            if isNull r.Prev then first <- r
            else r.Prev.Next <- r

    member private x.Resize (hard : bool, minCapacity : int64) =
        lock x (fun () ->
            let c = 
                if hard then minCapacity
                else Fun.NextPowerOfTwo (max 1024L minCapacity)

            if c <> capacity then
                ReaderWriterLock.write pointer (fun () ->
                    store.Realloc(c)

                    if c > capacity then
                        let newBlock = Block(x, capacity, c - capacity, Prev = last, Next = null, IsFree = false)
                        if not (isNull last) then last.Next <- newBlock
                        allocated <- allocated + newBlock.Size
                        x.Free newBlock
                    else
                        if last.IsFree && last.Offset <= c then
                            free.Remove(last.Size, last) |> ignore
                            last.Size <- c - last.Offset
                            free.Insert(last.Size, last)
                        else
                            Log.warn "cannot shrink"
                    capacity <- c
                )
        )

    member x.Free (b : Block) =
        lock b (fun () ->
            if not b.IsFree then
                lock x (fun () ->
                    let prev = b.Prev
                    let next = b.Next

                    if isNull prev then
                        first <- b

                    elif prev.IsFree then
                        free.Remove(prev.Size, prev) |> ignore
                        b.Offset <- prev.Offset
                        b.Size <- b.Size + prev.Size

                        b.Prev <- prev.Prev
                        if isNull b.Prev then first <- b
                        else b.Prev.Next <- b


                    if isNull next then
                        last <- b

                    elif next.IsFree then
                        free.Remove(next.Size, next) |> ignore
                        b.Size <- b.Size + next.Size

                        b.Next <- next.Next
                        if isNull b.Next then last <- b
                        else b.Next.Prev <- b


                    allocated <- allocated - b.Size
                    free.Insert(b.Size, b)
                    b.IsFree <- true
                )
        )

    member x.Alloc (size : int64) =
        lock x (fun () ->
            match free.TryGetGreaterOrEqualV(size) with
                | ValueSome b ->
                    b.IsFree <- false

                    if b.Size > size then
                        let n = Block(x, b.Offset + size, b.Size - size, Prev = b, Next = b.Next, IsFree = false)

                        if isNull n.Next then last <- n
                        else n.Next.Prev <- n
                            
                        b.Next <- n

                        b.Size <- size
                        x.Free n

                    allocated <- allocated + size
                    b

                | ValueNone ->
                    x.Resize(false, capacity + size)
                    x.Alloc(size)
        )

    member x.Realloc (b : Block, newSize : int64) =
        lock b (fun () ->
            lock x (fun () ->
                if b.Size <> newSize then
                    let n = x.Alloc(newSize)
                    x.Swap(b, n)
                    x.Free(n)


//                    let mutable missing = newSize - b.Size
//                    if missing > 0L then
//                        let next = b.Next
//                        if not (isNull next) && next.IsFree && next.Size >= missing then
//                            free.Remove(next.Size, next) |> ignore
//                            next.Offset <- next.Offset + missing
//                            next.Size <- next.Size - missing
//                            free.Insert(next.Size, next)
//                            allocated <- allocated + missing
//
//                            b.Size <- newSize
//                        else
//                            let n = x.Alloc(newSize)
//                            x.Swap(b, n)
//                            x.Free(n)
//
//
//                    elif missing < 0L then
//                        let rest = Block(x, b.Offset + newSize, b.Size + missing, Prev = b, Next = b.Next, IsFree = false)
//                    
//                        if isNull rest.Next then last <- rest
//                        else rest.Next.Prev <- rest
//                        b.Next <- rest
//
//                        b.Size <- newSize
//
//                    else
//                        ()
            )
        )

    member x.Write (b : Block, data : byte[]) =
        lock b (fun () ->
            if b.IsFree then failwith "out of range"
            let ptr = store + b.Offset
            ReaderWriterLock.read pointer (fun () ->
                ptr.Write(data, 0L, data.LongLength)
            )
        )

    member x.Read (b : Block, data : byte[]) =
        lock b (fun () ->
            if b.IsFree then failwith "out of range"
            let ptr = store + b.Offset
            ReaderWriterLock.read pointer (fun () ->
                ptr.Read(data, 0L, data.LongLength)
            )
        )

    member x.Dispose() =
        lock x (fun () ->
            store.Free()
            capacity <- 0L
            free.Clear()
            first <- null
            last <- null
            pointer.Dispose()
        )

    interface IDisposable with
        member x.Dispose() = x.Dispose()

and [<AllowNullLiteral>]
    Block =
        class
            val mutable public Memory   : MemoryManager
            val mutable public Offset   : int64
            val mutable public Size     : int64
            val mutable public Next     : Block
            val mutable public Prev     : Block
            val mutable public IsFree   : bool

            //member x.Write(source : nativeint, size : int64) = x.Memory.Write(x, source, size)

            new(mem, off, size) = { Memory = mem; Offset = off; Size = size; Prev = null; Next = null; IsFree = true }
        end

[<AbstractClass; Sealed; Extension>]
type BlockExtensions private() =
    [<Extension>]
    static member Realloc(this : Block, size : int64) = 
        this.Memory.Realloc(this, size)

    [<Extension>]
    static member Read(this : Block, target : byte[]) = 
        this.Memory.Read(this, target)

    [<Extension>]
    static member Write(this : Block, source : byte[]) = 
        this.Memory.Write(this, source)

    [<Extension>]
    static member Read(this : Block) =
        let target : byte[] = Array.zeroCreate (int this.Size)
        this.Memory.Read(this, target)
        target




