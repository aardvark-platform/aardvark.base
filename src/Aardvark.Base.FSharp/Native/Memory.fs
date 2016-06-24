namespace Aardvark.Base.Native

open System
open System.Threading
open System.IO
open System.IO.MemoryMappedFiles
open System.Runtime.InteropServices
open Aardvark.Base

module OldImpl = 
    [<AbstractClass>]
    type Memory() =
        abstract member Alloc   : int64 -> sizedptr
        abstract member Free    : sizedptr -> unit
        abstract member Realloc : sizedptr * int64 -> unit



    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Memory =
        type private Ptr =
            class
                inherit sizedptr
                val mutable public Value : nativeint
                val mutable public PointerSize : int64

                override x.Pointer = x.Value
                override x.IsValid = x.Value <> 0n
                override x.Size = x.PointerSize

                new(v, s) = { Value = v; PointerSize = s}
            end

        type private FilePtr =
            class
                inherit sizedptr

                val mutable public FileSize     : int64
                val mutable public Path         : Option<string>
                val mutable public MapName      : string
                val mutable public File         : MemoryMappedFile
                val mutable public Accessor     : MemoryMappedViewAccessor
                val mutable public Value        : nativeint

                override x.Pointer = x.Value
                override x.IsValid = x.Value <> 0n
                override x.Size = x.FileSize

                new(size, path, name, file, acc, value) = { FileSize = size; Path = path; MapName = name; File = file; Accessor = acc; Value = value }

            end

        type private ManagedPtr =
            class
                inherit sizedptr

                val mutable public Array : byte[]
                val mutable public Handle : GCHandle
                val mutable public Value : nativeint

                override x.Pointer = x.Value
                override x.IsValid = x.Value <> 0n
                override x.Size = 
                    if isNull x.Array then 0L
                    else x.Array.LongLength

                new(arr, handle, value) = { Array = arr; Handle = handle; Value = value }
            end

        type private ViewPointer =
            class
                inherit sizedptr

                val mutable public Base : ptr
                val mutable public Offset : nativeint
                val mutable public PointerSize : int64

       
                override x.IsValid = x.Base.IsValid
                override x.Pointer = x.Base.Pointer + x.Offset
                override x.Size = x.PointerSize

                new(ptr, o, s) = { Base = ptr; Offset = o; PointerSize = s }
            end


        let managed =
            { new Memory() with
                member x.Alloc(size) = 
                    let arr = Array.zeroCreate (int size)
                    let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)
                    let v = gc.AddrOfPinnedObject()
                    ManagedPtr(arr, gc, v) :> _

                member x.Free(ptr) = 
                    let ptr = unbox<ManagedPtr> ptr
                    ptr.Handle.Free()
                    ptr.Array <- null
                    ptr.Handle <- Unchecked.defaultof<_>
                    ptr.Value <- 0n

                member x.Realloc(ptr, size) =
                    let ptr = unbox<ManagedPtr> ptr
                    ptr.Handle.Free()
                    Array.Resize(&ptr.Array, int size)
                    let gc = GCHandle.Alloc(ptr.Array, GCHandleType.Pinned)
                    let v = gc.AddrOfPinnedObject()
                    ptr.Handle <- gc
                    ptr.Value <- v
                
            }

        let hglobal =
            { new Memory() with
                member x.Alloc(size) = 
                    Ptr(Marshal.AllocHGlobal(nativeint size), size) :> _

                member x.Free(ptr) = 
                    let ptr = unbox<Ptr> ptr
                    Marshal.FreeHGlobal ptr.Value
                    ptr.Value <- 0n

                member x.Realloc(ptr, size) =
                    let ptr = unbox<Ptr> ptr
                    ptr.Value <- Marshal.ReAllocHGlobal(ptr.Value, nativeint size)
                
            }

        let cotask =
            { new Memory() with
                member x.Alloc(size) = 
                    Ptr(Marshal.AllocCoTaskMem(int size), size) :> _

                member x.Free(ptr) = 
                    let ptr = unbox<Ptr> ptr
                    Marshal.FreeCoTaskMem ptr.Value
                    ptr.Value <- 0n

                member x.Realloc(ptr, size) =
                    let ptr = unbox<Ptr> ptr
                    ptr.Value <- Marshal.ReAllocCoTaskMem(ptr.Value, int size)
                
            }

        let newFile (path : string) =
            { new Memory() with
                member x.Alloc(size) = 
                    let mapName = Guid.NewGuid() |> string
                    let file = MemoryMappedFile.CreateFromFile(path, FileMode.Create, mapName, size, MemoryMappedFileAccess.ReadWrite)
                    let acc = file.CreateViewAccessor()
                    let ptr = acc.SafeMemoryMappedViewHandle.DangerousGetHandle()

                    FilePtr(size, Some path, mapName, file, acc, ptr) :> _

                member x.Free(ptr) = 
                    let ptr = unbox<FilePtr> ptr
                    ptr.Accessor.Dispose()
                    ptr.File.Dispose()

                    ptr.FileSize <- 0L
                    ptr.Path <- None
                    ptr.MapName <- null
                    ptr.File <- null
                    ptr.Accessor <- null
                    ptr.Value <- 0n

                member x.Realloc(ptr, size) =
                    let ptr = unbox<FilePtr> ptr
                
                    ptr.Accessor.Dispose()
                    ptr.File.Dispose()

                    let file = MemoryMappedFile.CreateFromFile(ptr.Path.Value, FileMode.Open, ptr.MapName, size, MemoryMappedFileAccess.ReadWrite)
                    let acc = file.CreateViewAccessor()
                    let value = acc.SafeMemoryMappedViewHandle.DangerousGetHandle()
                
                    ptr.FileSize <- size
                    ptr.File <- file
                    ptr.Accessor <- acc
                    ptr.Value <- value

            }
     
        let ofFile (path : string) =
            let ptr = 
                if File.Exists path then
                    let size = FileInfo(path).Length
                    let mapName = Guid.NewGuid() |> string
                    let file = MemoryMappedFile.CreateFromFile(path, FileMode.Open, mapName, size, MemoryMappedFileAccess.ReadWrite)
                    let acc = file.CreateViewAccessor()
                    let value = acc.SafeMemoryMappedViewHandle.DangerousGetHandle()

                    FilePtr(size, Some path, mapName, file, acc, value) :> sizedptr
                else
                    sizedptr.Null

            let heap = newFile path

            ptr, heap


        let split (mem : Memory) =
        
            let real = mem.Alloc(8L)
            let leftSizePtr = Ptr.cast<int64> real
            let leftSize = leftSizePtr.Value

            let mutable total = 0L
            let leftPtr = ViewPointer(real, 8n, leftSize)
            let rightPtr = ViewPointer(real, 8n + nativeint leftSize, real.Size - leftSize)

            let mutable live = 0

            let left =
                { new Memory() with
                    member x.Realloc(old : sizedptr, size : int64) =
                        if size <> leftPtr.PointerSize then
                            let rightShift = size - leftPtr.PointerSize
                            leftPtr.PointerSize <- size
                            leftSizePtr.Value <- size

                            // if the overall size changed realloc the underlying store
                            let newTotal = size + rightPtr.PointerSize
                            if total <> newTotal then
                                mem.Realloc(real, 8L + newTotal)
                                total <- newTotal


                            // move the right-block 
                            if rightPtr.PointerSize > 0L then
                                let newRight = rightPtr + rightShift
                                Marshal.Move(rightPtr.Pointer, newRight.Pointer, rightPtr.PointerSize)
                                rightPtr.Offset <- nativeint leftPtr.PointerSize
                
                    member x.Alloc s =
                        Interlocked.Increment(&live) |> ignore
                        x.Realloc(sizedptr.Null, s)
                        leftPtr :> sizedptr

                    member x.Free(ptr) =
                        if Interlocked.Decrement(&live) = 0 then
                            mem.Free(real)
                }

            let right =
                { new Memory() with
                    member x.Realloc(old : sizedptr, size : int64) =
                        if size <> rightPtr.PointerSize then
                            rightPtr.PointerSize <- size

                            // if the overall size changed realloc the underlying store
                            let newTotal = leftPtr.PointerSize + size
                            if total <> newTotal then
                                mem.Realloc(real, 8L + newTotal)
                                total <- newTotal


                    member x.Alloc s =
                        Interlocked.Increment(&live) |> ignore
                        x.Realloc(sizedptr.Null, s)
                        rightPtr :> _

                    member x.Free(ptr) =
                        if Interlocked.Decrement(&live) = 0 then
                            mem.Free(real)
                }

            left, right



[<AutoOpen>]
module NewImpl =
    
    [<AbstractClass>]
    type Memory() =
        inherit sizedptr()


        abstract member Clear : int64 -> unit
        abstract member Realloc : int64 -> unit
        abstract member Free : unit -> unit

        member private x.Dispose(disposing : bool) =
            if x.IsValid then
                if disposing then GC.SuppressFinalize x
                x.Free()

        member x.Dispose() = x.Dispose(true)
        override x.Finalize() = x.Dispose(false)

        interface IDisposable with
            member x.Dispose() = x.Dispose(true)

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Memory =
        
        type HGlobalMemory(size : int64) =
            inherit Memory()

            let mutable size = size
            let mutable ptr = Marshal.AllocHGlobal (nativeint size)

            override x.IsValid = ptr <> 0n
            override x.Pointer = ptr
            override x.Size = size
            override x.Realloc (newSize : int64) =
                if newSize <> size then
                    ptr <- Marshal.ReAllocHGlobal(ptr, nativeint size)
                    size <- newSize

            override x.Clear(newSize : int64) =
                Marshal.FreeHGlobal(ptr)
                ptr <- Marshal.AllocHGlobal (nativeint newSize)
                size <- newSize

            override x.Free() =
                let p = Interlocked.Exchange(&ptr, 0n)
                if p <> 0n then
                    size <- 0L
                    Marshal.FreeHGlobal p

        type CoTaskMemory(size : int64) =
            inherit Memory()

            let mutable size = size
            let mutable ptr = Marshal.AllocCoTaskMem (int size)

            override x.IsValid = ptr <> 0n
            override x.Pointer = ptr
            override x.Size = size
            override x.Realloc (newSize : int64) =
                if newSize <> size then
                    ptr <- Marshal.ReAllocCoTaskMem(ptr, int size)
                    size <- newSize

            override x.Clear(newSize : int64) =
                Marshal.FreeCoTaskMem(ptr)
                ptr <- Marshal.AllocCoTaskMem (int newSize)
                size <- newSize

            override x.Free() =
                let p = Interlocked.Exchange(&ptr, 0n)
                if p <> 0n then
                    size <- 0L
                    Marshal.FreeCoTaskMem p

        type FileOperation =
            | Open of string
            | Create of string * int64

        type MappedFileMemory(file : FileOperation) =
            inherit Memory()

            let mapName = Guid.NewGuid() |> string

            let mutable fileName, file, size = 
                match file with
                    | Open(fileName) ->
                        if File.Exists fileName then
                            let info = FileInfo(fileName)
                            let file = MemoryMappedFile.CreateFromFile(fileName, FileMode.Open, mapName, info.Length, MemoryMappedFileAccess.ReadWrite)
                            fileName, file, info.Length
                        else
                            let file = MemoryMappedFile.CreateFromFile(fileName, FileMode.Create, mapName, 0L, MemoryMappedFileAccess.ReadWrite)
                            fileName, file, 0L

                    | Create(fileName, capacity) ->
                        let file = MemoryMappedFile.CreateFromFile(fileName, FileMode.Create, mapName, capacity, MemoryMappedFileAccess.ReadWrite)
                        fileName, file, capacity
            
            let mutable accessor = file.CreateViewAccessor()
            let mutable ptr = accessor.SafeMemoryMappedViewHandle.DangerousGetHandle()

            override x.IsValid = not (isNull file)
            override x.Pointer = ptr
            override x.Size = size
            override x.Realloc (newSize : int64) =
                if newSize <> size then
                    accessor.Dispose()
                    file.Dispose()
                    file <- MemoryMappedFile.CreateFromFile(fileName, FileMode.Open, mapName, newSize, MemoryMappedFileAccess.ReadWrite)
                    accessor <- file.CreateViewAccessor()
                    ptr <- accessor.SafeMemoryMappedViewHandle.DangerousGetHandle()
                    size <- newSize

            override x.Clear (newSize : int64) =
                accessor.Dispose()
                file.Dispose()
                file <- MemoryMappedFile.CreateFromFile(fileName, FileMode.Create, mapName, newSize, MemoryMappedFileAccess.ReadWrite)
                accessor <- file.CreateViewAccessor()
                ptr <- accessor.SafeMemoryMappedViewHandle.DangerousGetHandle()
                size <- newSize

            override x.Free() =
                let f = Interlocked.Exchange(&file, null)
                if not (isNull f) then
                    accessor.Dispose()
                    f.Dispose()
                    fileName <- null
                    file <- null
                    size <- 0L
                    accessor <- null
                    ptr <- 0n


        let hglobal (size : int64) =
            new HGlobalMemory(size) :> Memory

        let cotask (size : int64) =
            new CoTaskMemory(size) :> Memory

        let mapped (file : string) =
            new MappedFileMemory(Open file) :> Memory




        type private ViewPointer =
            class
                inherit sizedptr

                val mutable public Base : ptr
                val mutable public Offset : nativeint
                val mutable public PointerSize : int64

       
                override x.IsValid = x.Base.IsValid
                override x.Pointer = x.Base.Pointer + x.Offset
                override x.Size = x.PointerSize

                new(ptr, o, s) = { Base = ptr; Offset = o; PointerSize = s }
            end

        let split (mem : Memory) =
            let leftSizePtr = Ptr.cast<int64> mem
            
            let mutable leftSize =
                if mem.Size < 8L then 
                    mem.Clear 8L
                    0L
                else 
                    leftSizePtr.Value
            
            let mutable rightSize =
                mem.Size - leftSize - 8L    

            let mutable live = 2

            let left =
                { new Memory() with
                    override x.IsValid = 
                        mem.IsValid

                    override x.Pointer = 
                        mem.Pointer + 8n

                    override x.Size =
                        leftSize

                    override x.Realloc (newSize : int64) =
                        if newSize > leftSize then
                            // realloc the underlying memory
                            mem.Realloc (8L + newSize + rightSize)

                            // move the right memory 
                            let rightShift = newSize - leftSize
                            let rightStart = mem.Pointer + 8n + nativeint leftSize
                            Marshal.Move(rightStart, rightStart + nativeint rightShift, rightSize)

                            // set the new left-memory to 0
                            Marshal.Set(rightStart, 0, rightShift)

                            // store the new leftSize
                            leftSizePtr.Value <- newSize
                            leftSize <- newSize

                        elif newSize < leftSize then
                            
                            // move the right memory
                            let rightShift = newSize - leftSize
                            let rightStart = mem.Pointer + 8n + nativeint leftSize
                            Marshal.Move(rightStart, rightStart + nativeint rightShift, rightSize)


                            // realloc the underlying memory
                            mem.Realloc (8L + newSize + rightSize)
                            
                            // store the new leftSize
                            leftSizePtr.Value <- newSize
                            leftSize <- newSize


                    override x.Clear (newSize : int64) =
                        if newSize > leftSize then
                            // realloc the underlying memory
                            mem.Realloc (8L + newSize + rightSize)

                            // move the right memory 
                            let rightShift = newSize - leftSize
                            let rightStart = mem.Pointer + 8n + nativeint leftSize
                            Marshal.Move(rightStart, rightStart + nativeint rightShift, rightSize)

                            // set the left-memory to 0
                            Marshal.Set(mem.Pointer + 8n, 0, newSize)

                            // store the new leftSize
                            leftSizePtr.Value <- newSize
                            leftSize <- newSize

                        elif newSize < leftSize then
                            
                            // move the right memory
                            let rightShift = newSize - leftSize
                            let rightStart = mem.Pointer + 8n + nativeint leftSize
                            Marshal.Move(rightStart, rightStart + nativeint rightShift, rightSize)


                            // realloc the underlying memory
                            mem.Realloc (8L + newSize + rightSize)
                            
                            // set the left-memory to 0
                            Marshal.Set(mem.Pointer + 8n, 0, newSize)

                            // store the new leftSize
                            leftSizePtr.Value <- newSize
                            leftSize <- newSize

                        else
                            Marshal.Set(mem.Pointer + 8n, 0, leftSize)

                    override x.Free() =
                        if Interlocked.Decrement(&live) = 0 then
                            mem.Free()
                }

            let right =
                { new Memory() with
                    override x.IsValid = 
                        mem.IsValid

                    override x.Pointer = 
                        mem.Pointer + 8n + nativeint leftSize

                    override x.Size =
                        rightSize

                    override x.Realloc (newSize : int64) =
                        if newSize > rightSize then
                            // realloc the underlying memory
                            mem.Realloc (8L + leftSize + newSize)

                            // store the new leftSize
                            rightSize <- newSize

                        elif newSize < leftSize then
                            
                            // realloc the leftSize memory
                            mem.Realloc (8L + leftSize + newSize)
                            
                            // store the new leftSize
                            rightSize <- newSize


                    override x.Clear (newSize : int64) =
                        x.Realloc(newSize)
                        Marshal.Set(mem.Pointer + 8n + nativeint leftSize, 0, newSize)

                    override x.Free() =
                        if Interlocked.Decrement(&live) = 0 then
                            mem.Free()
                }



            left, right


