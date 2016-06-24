namespace Aardvark.Base.Native

open System
open System.Threading
open System.Runtime.CompilerServices
open System.Collections.Concurrent
open Aardvark.Base
open Aardvark.Base.Native


type File(id : Guid, table : FileTable, data : MemoryManager, block : Block) =
    let mutable block = block

    override x.ToString() =
        if isNull block then "File Empty"
        else sprintf "File { offset = %d; size = %d }" block.Offset block.Size
            


    member x.Id = id

    member x.Exists =
        not (isNull block)

    member x.Size =
        if isNull block then 0L
        else block.Size

    member x.Delete() =
        if not (isNull block) then
            lock table (fun () -> table.Remove(id) |> ignore)
            data.Free(block)
            block <- null

    member x.Read() =
        if not (isNull block) then block.Read()
        else [||]

    member x.Write(arr : byte[]) =
        if isNull block then
            block <- data.Alloc arr.LongLength
            lock table (fun () -> table.[id] <- (block.Offset, block.Size))

        elif block.Size <> arr.LongLength then
            block.Realloc arr.LongLength
            lock table (fun () -> table.[id] <- (block.Offset, block.Size))

        block.Write(arr)

type Store(mem : Memory) =
    let managed, table = Memory.split mem

    let fileTable = new FileTable(table)
    let data = new MemoryManager(managed)

    let fileCache = ConcurrentDictionary<Guid, File>()


    let import() =
        if fileTable.Count > 0 then
            let arr = Array.zeroCreate fileTable.Count
            let mutable i = 0
            for e in fileTable do
                arr.[i] <- e.Key, e.Offset, e.Size
                i <- i + 1

            Array.sortInPlaceBy (fun (_,o,_) -> o) arr
            data.ReadEntries(arr, fun id b -> 
                let file = File(id, fileTable, data, b)
                fileCache.[id] <- file
            )
    
    do import()

    member x.FileCount = fileCache.Count
    
    member x.FileTableSize = Mem table.Size
    member x.DataSize = Mem managed.Size
    member x.Memory = Mem mem.Size

    member x.Create() =
        let mutable id = Guid.NewGuid()
        while fileCache.ContainsKey id do
            Log.error "Congratulations!!!!!"
            Log.error "you just got a GUID collision in your store"
            Log.error "the colliding guid was %A" id
            Log.error "which was already a file-id used by the file %A" fileCache.[id]
            Log.error "re-creating random guid"
            id <- Guid.NewGuid()

        let f = File(id, fileTable, data, null)
        fileCache.[id] <- f
        f

    member x.Get(id : Guid) =
        fileCache.GetOrAdd(id, fun id ->
            File(id, fileTable, data, null)
        )


    member x.Dispose() =
        mem.Dispose()

    interface IDisposable with
        member x.Dispose() = x.Dispose()
