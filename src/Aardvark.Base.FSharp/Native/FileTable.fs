namespace Aardvark.Base.Native

#nowarn "9"

open System
open System.Runtime.InteropServices
open System.Collections.Generic
open Aardvark.Base

[<StructLayout(LayoutKind.Sequential)>]
type Entry =
    struct
        val mutable public Key      : Guid
        val mutable public Offset   : int64
        val mutable public Size     : int64
        val mutable public HashCode : int
        val mutable public Next     : int

        override x.ToString() =
            sprintf "{ Key = %A; Offset = %d; Size = %d }" x.Key x.Offset x.Size

        new(k : Guid, o : int64, s : int64, h : int, n : int) = { Key = k; Offset = o; Size = s; HashCode = h; Next = n }
    end


[<StructLayout(LayoutKind.Explicit, Size = 40)>]
type private DictHeader =
    struct
        [<FieldOffset(0)>]
        val mutable public Magic            : int64

        [<FieldOffset(8)>]
        val mutable public CapacityIndex    : int

        [<FieldOffset(12)>]
        val mutable public Capacity         : int

        [<FieldOffset(16)>]
        val mutable public FreeList         : int

        [<FieldOffset(24)>]
        val mutable public Count            : int

        [<FieldOffset(28)>]
        val mutable public FreeCount        : int
    end 

[<AutoOpen>]
module private ``DictHeader Extensions`` =
    
    let inline (==) (a : Guid) (b : Guid) =
        a.Equals(b)
    
    let inline private read<'a when 'a : unmanaged> (ptr : ptr) : 'a =
        ptr.Read<'a>()

    let inline private write<'a when 'a : unmanaged> (v : 'a) (ptr : ptr) : unit =
        ptr.Write<'a>(v)



    let inline (!!) (ptr : ptr<'a>) = ptr.Value
    let inline (!=) (ptr : ptr<'a>) (v : 'a) = ptr.Value <- v



    type ptr with
        member x.Magic
            with inline get() = x |> read<int64>
            and inline set m  = x |> write<int64> m

        member x.CapacityIndex
            with inline get() = (8n + x) |> read<int>
            and inline set m  = (8n + x) |> write<int> m

        member x.Capacity
            with inline get() = (12n + x) |> read<int>
            and inline set m  = (12n + x) |> write<int> m


        member x.FreeList
            with inline get() = (16n + x) |> read<int>
            and inline set m  = (16n + x) |> write<int> m

        member x.Count
            with inline get() = (24n + x) |> read<int>
            and inline set m  = (24n + x) |> write<int> m

        member x.FreeCount
            with inline get() = (28n + x) |> read<int>
            and inline set m  = (28n + x) |> write<int> m


        member x.Key
            with inline get() = x |> read<Guid>
            and inline set m  = x |> write<Guid> m

        member x.Offset
            with inline get() = (16n + x) |> read<int64>
            and inline set m  = (16n + x) |> write<int64> m

        member x.Size
            with inline get() = (24n + x) |> read<int64>
            and inline set m  = (24n + x) |> write<int64> m

        member x.HashCode
            with inline get() = (32n + x) |> read<int>
            and inline set m  = (32n + x) |> write<int> m

        member x.Next
            with inline get() = (36n + x) |> read<int>
            and inline set m  = (36n + x) |> write<int> m

    let inline (.+) (ptr : ptr<'a>) (i : int) : ptr<'a> =
        ptr + i


type private DictStore =
    {
        size        : int64
        pointer    : Memory
        header      : ptr<DictHeader>
        entries     : ptr<Entry>
        buckets     : ptr<int>
    }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module private DictStore =
    let private magic = 0x4a3214b10905af7aL
    let private se = sizeof<Entry> |> int64
    let private si = sizeof<int> |> int64
    let private sh = sizeof<DictHeader> |> int64


    let primeSizes =
        [|
            (*    prime no.           prime *)
            (*           2                3       +  1 = 2^2 *)
            (*           4 *) 7                // +  1 = 2^3, minimal size
            (*           6 *) 13               // +  3 = 2^4
            (*          11 *) 31               // +  1 = 2^5
            (*          18 *) 61               // +  3 = 2^6
            (*          31 *) 127              // +  1 = 2^7
            (*          54 *) 251              // +  5 = 2^8
            (*          97 *) 509              // +  3 = 2^9
            (*         172 *) 1021             // +  3 = 2^10
            (*         309 *) 2039             // +  9 = 2^11
            (*         564 *) 4093             // +  3 = 2^12
            (*        1028 *) 8191             // +  1 = 2^13
            (*        1900 *) 16381            // +  3 = 2^14
            (*        3512 *) 32749            // + 19 = 2^15
            (*        6542 *) 65521            // + 15 = 2^16
            (*       12251 *) 131071           // +  1 = 2^17
            (*       23000 *) 262139           // +  5 = 2^18
            (*       43390 *) 524287           // +  1 = 2^19
            (*       82025 *) 1048573          // +  3 = 2^20
            (*      155611 *) 2097143          // +  9 = 2^21
            (*      295947 *) 4194301          // +  3 = 2^22
            (*      564163 *) 8388593          // + 15 = 2^23
            (*     1077871 *) 16777213         // +  3 = 2^24
            (*     2063689 *) 33554393         // + 39 = 2^25
            (*     3957809 *) 67108859         // +  5 = 2^26
            (*     7603553 *) 134217689        // + 39 = 2^27
            (*    14630843 *) 268435399        // + 57 = 2^28
            (*    28192750 *) 536870909        // +  3 = 2^29
            (*    54400028 *) 1073741789       // + 35 = 2^30
            (*   105097565 *) 2147483647       // +  1 = 2^31
        |]


    let empty =
        {
            size = 0L
            pointer = Unchecked.defaultof<_>
            header = ptr<_>.Null
            entries = ptr<_>.Null
            buckets = ptr<_>.Null
        }
        
    let private isDict (ptr : ptr) =
        ptr.Read<int64>() = magic

    let create (mem : Memory) (capacityIndex : int) =      
        let capacity = primeSizes.[capacityIndex]
        // create a new dictionary
        let size = sh + int64 capacity * (se + si)
        mem.Clear size

        let pHeader = Ptr.cast mem
        let pEntries = Ptr.cast (mem + sh)
        let pBuckets = Ptr.cast (mem + sh + (int64 capacity * se))

        pHeader != 
            DictHeader(
                Magic = magic,
                CapacityIndex = capacityIndex,
                Capacity = capacity,
                FreeList = -1,
                Count = 0,
                FreeCount = 0
            )


        {
            size = size
            pointer = mem
            header = pHeader
            entries = pEntries
            buckets = pBuckets
        }

    let getOrCreate (mem : Memory) (capacityIndex : int) =      
        if mem.Size >= 8L && isDict mem then
            // read the dictionary
            let size = mem.Size
            let mapName = Guid.NewGuid() |> string

            let pHeader = Ptr.cast mem
            let header : DictHeader = !!pHeader
            let pEntries = Ptr.cast (mem + nativeint sh)
            let pBuckets = Ptr.cast (mem + nativeint sh + nativeint (int64 header.Capacity * se))

            {
                size = size
                pointer = mem
                header = pHeader
                entries = pEntries
                buckets = pBuckets
            }

        else
            create mem capacityIndex

    let realloc (store : DictStore) (capacityIndex : int) =
        let capacity = primeSizes.[capacityIndex]
        let size = sh + int64 capacity * (se + si)

        store.pointer.Realloc(size)

        let pHeader = Ptr.cast store.pointer
        let pEntries = Ptr.cast (store.pointer + sh)
        let pBuckets = Ptr.cast (store.pointer + sh + (int64 capacity * se))

        Marshal.Set(pBuckets.Pointer, 0, unativeint (sizeof<int> * capacity))

        pHeader.CapacityIndex <- capacityIndex
        pHeader.Capacity <- capacity

        {
            size = size
            pointer = store.pointer
            header = pHeader
            entries = pEntries
            buckets = pBuckets
        }


    let release (s : DictStore) =
        s.pointer.Dispose()


type FileTable(mem : Memory) =

    static let initialExp = 10
    let mutable store = DictStore.getOrCreate mem (initialExp - 3)
    let mutable header = store.header

    member private x.Grow () : unit =
        // TODO: backup!!!!

        let oldHeader = !!header
        let newCapacityIndex = 1 + oldHeader.CapacityIndex
        let count = oldHeader.Count        
        store <- DictStore.realloc store newCapacityIndex
        header <- store.header


        let cap = header.Capacity
        let mutable pEntry = store.entries
        for i in 0 .. count-1 do
            if not (pEntry.Key == Guid.Empty) then
                let bucket = pEntry.HashCode % cap
                pEntry.Next <- store.buckets.[bucket] - 1
                store.buckets.[bucket] <- i + 1

            pEntry <- pEntry .+ 1


        // TODO: remove backup
        ()


    member private x.FindEntry(key : Guid, hashCode : int) =
        let index = hashCode % header.Capacity
        let id = store.buckets.[index] - 1
        if id < 0 then 
            -1
        else
            let mutable pEntry = store.entries .+ id
            if pEntry.HashCode = hashCode && pEntry.Key == key then
                id
            else
                let next = pEntry.Next
                if next >= 0 then
                    let rec search (id : int) (pEntry : ptr<Entry>) =
                        if pEntry.HashCode = hashCode && pEntry.Key == key then 
                            id
                        else
                            let next = pEntry.Next
                            if next >= 0 then
                                search next (store.entries .+ pEntry.Next)
                            else
                                -1

                    search next (store.entries .+ next)
                else
                    -1     

    member private x.FindEntry(key : Guid) =
        let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
        x.FindEntry(key, hashCode)

    member private x.Set(key : Guid, offset : int64, size : int64) =
        let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
        let mutable index = hashCode % header.Capacity
        let id = x.FindEntry(key, hashCode)

        if id >= 0 then
            let p = store.entries .+ id
            p.Offset <- offset
            p.Size <- size
        else
            let freeCount = header.FreeCount
            let entryId = 
                if freeCount > 0 then
                    let newBucket = header.FreeList
                    header.FreeList <- (store.entries .+ newBucket).Next
                    header.FreeCount <- freeCount - 1
                    newBucket
                else
                    let count = header.Count
                    if count = header.Capacity then
                        x.Grow()
                        index <- hashCode % header.Capacity

                    header.Count <- count + 1
                    count

            let pEntry = store.entries .+ entryId
            let pBucket = store.buckets .+ index
            pEntry.Key <- key
            pEntry.Offset <- offset
            pEntry.Size <- size
            pEntry.HashCode <- hashCode
            pEntry.Next <- !!pBucket - 1
            pBucket != 1 + entryId

        
    member private x.Get(key : Guid) =
        let id = x.FindEntry(key)
        if id < 0 then
            failwithf "key not found: %A" key
        else
            let p = store.entries .+ id
            (p.Offset, p.Size)


    member x.Remove(key : Guid) =
        let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
        let index = hashCode % header.Capacity
        let id = store.buckets.[index] - 1
        if id < 0 then 
            false
        else
            let pEntry = store.entries .+ id
            let mutable entryId = -1
             
            if pEntry.HashCode = hashCode && pEntry.Key == key then
                store.buckets.[index] <- pEntry.Next + 1
                entryId <- id
            else
                let next = pEntry.Next
                if next >= 0 then
                    let rec search (pLast : ptr<Entry>) (id : int) (pEntry : ptr<Entry>) =
                        if pEntry.HashCode = hashCode && pEntry.Key == key then 
                            pLast.Next <- pEntry.Next
                            entryId <- id
                        else
                            let next = pEntry.Next
                            if next >= 0 then
                                search pEntry next (store.entries .+ pEntry.Next)

                    search pEntry next (store.entries .+ next)



            if entryId >= 0 then
    
                let pEntry = store.entries .+ entryId
                let nextFree =
                    if header.FreeCount > 0 then header.FreeList
                    else -1
                pEntry != Entry(Guid.Empty, 0L, 0L, -1, nextFree)
                header.FreeList <- entryId
                header.FreeCount <- header.FreeCount + 1

                true
            else
                false

    member x.TryGetValue(key : Guid, [<Out>] value : byref<int64 * int64>) =
        let id = x.FindEntry(key)
        if id < 0 then
            false
        else
            let p = store.entries .+ id
            value <- (p.Offset, p.Size)
            true

    member x.Count =
        header.Count - header.FreeCount

    member x.Item
        with get (key : Guid) = x.Get key
        and set (key : Guid) (offset : int64, size : int64) = x.Set(key, offset, size)

    member x.GetOrAdd(key : Guid, f : Guid -> int64 * int64) =
        let hashCode = key.GetHashCode() &&& 0x7FFFFFFF
        let mutable index = hashCode % header.Capacity
        let id = x.FindEntry(key, hashCode)

        if id >= 0 then
            let p = store.entries .+ id
            (p.Offset, p.Size)
        else
            let offset, size = f key
            let freeCount = header.FreeCount
            let entryId = 
                if freeCount > 0 then
                    let newBucket = header.FreeList
                    header.FreeList <- (store.entries .+ newBucket).Next
                    header.FreeCount <- freeCount - 1
                    newBucket
                else
                    let count = header.Count
                    if count = header.Capacity then
                        x.Grow()
                        index <- hashCode % header.Capacity

                    header.Count <- count + 1
                    count

            let pEntry = store.entries .+ entryId
            let pBucket = store.buckets .+ index
            pEntry.Key <- key
            pEntry.Offset <- offset
            pEntry.Size <- size
            pEntry.HashCode <- hashCode
            pEntry.Next <- !!pBucket - 1
            pBucket != 1 + entryId
            (offset, size)

    member x.ContainsKey (key : Guid) =
        let id = x.FindEntry(key)
        id >= 0


    member x.Dispose() =
        DictStore.release store
        store <- DictStore.empty

    interface IDisposable with
        member x.Dispose() = x.Dispose()

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() = new FileDictEnumerator(store) :> _

    interface System.Collections.Generic.IEnumerable<Entry> with
        member x.GetEnumerator() = new FileDictEnumerator(store) :> _

and private FileDictEnumerator(store : DictStore) =
    static let nullPtr : ptr<Entry> = ptr<_>.Null

    let cnt = store.header.Count
    let mutable i = -1
    let mutable ptr = store.entries .+ -1

    member x.MoveNext() =
        i <- i + 1
        ptr <- ptr .+ 1
        while i < cnt && (store.entries .+ i).HashCode < 0 do
            i <- i + 1
            ptr <- ptr .+ 1

        i < cnt

    member x.Current : Entry =
        !!ptr

    member x.Reset() =
        i <- -1
        ptr <- store.entries .+ -1

    interface System.Collections.IEnumerator with
        member x.Current = x.Current :> obj
        member x.MoveNext() = x.MoveNext()
        member x.Reset() = x.Reset()

    interface IEnumerator<Entry> with
        member x.Current = x.Current
        member x.Dispose() = ()

