namespace Aardvark.Base

open System
open System.IO.MemoryMappedFiles
open System.Runtime.InteropServices

[<Flags>]
type SharedMemoryAccess =
    | Read    = 0 // Always implied
    | Write   = 1
    | Execute = 2

[<AllowNullLiteral>]
type ISharedMemory =
    inherit IDisposable
    abstract member Name : string
    abstract member Size : int64
    abstract member Access : SharedMemoryAccess
    abstract member Pointer : nativeint

module internal SharedMemoryImpl =

    module Windows =

        let create (name: string) (openExisting: bool) (access: SharedMemoryAccess) (size: int64) =
            let fileAccess =
                if access.HasFlag SharedMemoryAccess.Execute then
                    if access.HasFlag SharedMemoryAccess.Write then
                        MemoryMappedFileAccess.ReadWriteExecute
                    else
                        MemoryMappedFileAccess.ReadExecute
                else
                    if access.HasFlag SharedMemoryAccess.Write then
                        MemoryMappedFileAccess.ReadWrite // CreateViewAccessor() requires Read
                    else
                        MemoryMappedFileAccess.Read

            let file =
                if openExisting then
                    let mutable rights = enum<MemoryMappedFileRights> 0
                    if access.HasFlag SharedMemoryAccess.Read then &rights |||= MemoryMappedFileRights.Read
                    if access.HasFlag SharedMemoryAccess.Write then &rights |||= MemoryMappedFileRights.Write
                    if access.HasFlag SharedMemoryAccess.Execute then &rights |||= MemoryMappedFileRights.Execute
                    MemoryMappedFile.OpenExisting(name, rights)
                else
                    MemoryMappedFile.CreateNew(name, size, fileAccess)

            let view = file.CreateViewAccessor(0L, size, fileAccess)
            let handle = view.SafeMemoryMappedViewHandle.DangerousGetHandle()

            { new ISharedMemory with
                member _.Name = name
                member _.Size = size
                member _.Access = access
                member _.Pointer = handle
                member _.Dispose() =
                    view.Dispose()
                    file.Dispose() }

    module Posix =

        [<Flags>]
        type Protection =
            | Read    = 0x01
            | Write   = 0x02
            | Execute = 0x04

        type SharedMemoryAccess with
            member this.Protection =
                let mutable prot = enum<Protection> 0
                if this.HasFlag SharedMemoryAccess.Read then &prot |||= Protection.Read
                if this.HasFlag SharedMemoryAccess.Write then &prot |||= Protection.Write
                if this.HasFlag SharedMemoryAccess.Execute then &prot |||= Protection.Execute
                prot

        [<Flags>]
        type FileMode =
            | Execute = 0x01
            | Write   = 0x02
            | Read    = 0x04

        type SharedMemoryAccess with
            member this.FileMode =
                let mutable mode = enum<FileMode> 0
                if this.HasFlag SharedMemoryAccess.Read then &mode |||= FileMode.Read
                if this.HasFlag SharedMemoryAccess.Write then &mode |||= FileMode.Write
                if this.HasFlag SharedMemoryAccess.Execute then &mode |||= FileMode.Execute
                mode

        [<StructLayout(LayoutKind.Sequential); StructuredFormatDisplay("{AsString}")>]
        type FileHandle =
            struct
                val mutable public Id : int
                override x.ToString() = sprintf "f%d" x.Id
                member private x.AsString = x.ToString()
                member x.IsValid = x.Id >= 0
            end

        [<StructLayout(LayoutKind.Sequential); StructuredFormatDisplay("{AsString}")>]
        type Permission =
            struct
                val mutable public Mask : uint16

                member x.Owner
                    with get() =
                        (x.Mask >>> 6) &&& 7us |> int |> enum<FileMode>
                    and set (v : FileMode) =
                        x.Mask <- (x.Mask &&& 0xFE3Fus) ||| ((uint16 v &&& 7us) <<< 6)

                member x.Group
                    with get() =
                        (x.Mask >>> 3) &&& 7us |> int |> enum<FileMode>
                    and set (v : FileMode) =
                        x.Mask <- (x.Mask &&& 0xFFC7us) ||| ((uint16 v &&& 7us) <<< 3)

                member x.Other
                    with get() =
                        x.Mask &&& 7us |> int |> enum<FileMode>
                    and set (v : FileMode) =
                        x.Mask <- (x.Mask &&& 0xFFF8us) ||| (uint16 v &&& 7us)


                member private x.AsString = x.ToString()
                override x.ToString() =
                    let u = x.Owner
                    let g = x.Group
                    let o = x.Other

                    let inline str (p : FileMode) =
                        (if p.HasFlag FileMode.Execute then "x" else "-") +
                        (if p.HasFlag FileMode.Write then "w" else "-") +
                        (if p.HasFlag FileMode.Read then "r" else "-")

                    str u + str g + str o

                new(user: FileMode, group: FileMode, other: FileMode) =
                    {
                        Mask = ((uint16 user &&& 7us) <<< 6) ||| ((uint16 group &&& 7us) <<< 3) ||| (uint16 other &&& 7us)
                    }

                new (mode: FileMode) =
                    Permission(mode, mode, mode)
            end

        module Mac =
            [<Flags>]
            type MapFlags =
                | Shared       = 0x0001
                | Private      = 0x0002
                | Fixed        = 0x0010
                | Rename       = 0x0020
                | NoReserve    = 0x0040
                | NoExtend     = 0x0100
                | HasSemaphore = 0x0200
                | NoCache      = 0x0400
                | Jit          = 0x0800
                | Anonymous    = 0x1000

            [<Flags>]
            type SharedMemoryFlags =
                | ReadOnly      = 0x0000
                | WriteOnly     = 0x0001
                | ReadWrite     = 0x0002
                | SharedLock    = 0x0010
                | ExclusiveLock = 0x0020
                | Async         = 0x0040
                | NoFollow      = 0x0100
                | Create        = 0x0200
                | Truncate      = 0x0400
                | Exclusive     = 0x0800
                | NonBlocking   = 0x0004
                | Append        = 0x0008

            module private LibC =

                [<DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError=true, EntryPoint="shm_open")>]
                extern int private _shmopen(string name, SharedMemoryFlags oflag, Permission mode)

                [<DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError=true, EntryPoint="shm_open")>]
                extern int private _shmopenArm64(string name, SharedMemoryFlags oflag, int c, int d, int e, int f, int g, int h, Permission mode)

                // See https://github.com/dotnet/runtime/issues/48752
                let shmopen (name, oflag, mode) =
                    if RuntimeInformation.ProcessArchitecture = Architecture.Arm64 then
                        _shmopenArm64(name, oflag, 0, 0, 0, 0, 0, 0, mode)
                    else
                        _shmopen(name, oflag, mode)

                [<DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError=true)>]
                extern nativeint mmap(nativeint addr, unativeint size, Protection prot, MapFlags flags, int fd, unativeint offset)

                [<DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError=true)>]
                extern int munmap(nativeint ptr, unativeint size)

                [<DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError=true, EntryPoint="shm_unlink")>]
                extern int shmunlink(string name)

                [<DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError=true)>]
                extern int ftruncate(int fd, unativeint size)

                [<DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError=true)>]
                extern int close(int fd)

                [<DllImport("libc", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl, SetLastError=true, EntryPoint="strerror")>]
                extern nativeint strerror(int code)

            let private fail (message: string) (code: int) =
                let error = code |> LibC.strerror |> Marshal.PtrToStringAnsi
                failwith $"{message} (ERROR: {error})"

            let create (name: string) (openExisting: bool) (access: SharedMemoryAccess) (size: int64) =
                let flags =
                    if openExisting then
                        if access.HasFlag SharedMemoryAccess.Write then
                            SharedMemoryFlags.ReadWrite
                        else
                            SharedMemoryFlags.ReadOnly

                    else
                        SharedMemoryFlags.Create ||| SharedMemoryFlags.Exclusive ||| SharedMemoryFlags.ReadWrite

                let fd = LibC.shmopen(name, flags, Permission(access.FileMode))
                if fd < 0 then
                    let err = Marshal.GetLastWin32Error()
                    fail $"Could not open shared memory object \"{name}\"" err

                if not openExisting && LibC.ftruncate(fd, unativeint size) <> 0 then
                    let err = Marshal.GetLastWin32Error()
                    LibC.shmunlink name |> ignore
                    fail $"Could not resize shared memory object \"{name}\" to {size} bytes" err

                let ptr = LibC.mmap(0n, unativeint size, access.Protection, MapFlags.Shared, fd, 0un)
                if ptr = -1n then
                    let err = Marshal.GetLastWin32Error()
                    if not openExisting then LibC.shmunlink name |> ignore
                    fail $"Could not map shared memory object \"{name}\"" err

                { new ISharedMemory with
                    member _.Name = name
                    member _.Size = size
                    member _.Access = access
                    member _.Pointer = ptr
                    member _.Dispose() =
                        if LibC.munmap(ptr, unativeint size) <> 0 then
                            let err = Marshal.GetLastWin32Error()
                            LibC.close fd |> ignore
                            if not openExisting then LibC.shmunlink name |> ignore
                            fail $"Could not unmap shared memory object \"{name}\"" err

                        if LibC.close fd <> 0 then
                            let err = Marshal.GetLastWin32Error()
                            if not openExisting then LibC.shmunlink name |> ignore
                            fail $"Could not close shared memory file \"{name}\"" err

                        if not openExisting then
                            LibC.shmunlink name |> ignore // May have been closed by someone else -> fails
                }

        module Linux =
            [<Flags>]
            type MapFlags =
                | Shared  = 0x1
                | Private = 0x2
                | Fixed   = 0x10

            [<Flags>]
            type SharedMemoryFlags =
                | ReadOnly  = 0x0
                | WriteOnly = 0x1
                | ReadWrite = 0x2
                | Create    = 0x40
                | Truncate  = 0x200
                | Exclusive = 0x80

            module LibC =
                [<DllImport("libc", CharSet = CharSet.Ansi, SetLastError=true, EntryPoint="shm_open")>]
                extern FileHandle shmopen(string name, SharedMemoryFlags oflag, Permission mode)

                [<DllImport("libc", CharSet = CharSet.Ansi, SetLastError=true)>]
                extern nativeint mmap(nativeint addr, unativeint size, Protection prot, MapFlags flags, FileHandle fd, unativeint offset)

                [<DllImport("libc", CharSet = CharSet.Ansi, SetLastError=true)>]
                extern int munmap(nativeint ptr, unativeint size)

                [<DllImport("libc", CharSet = CharSet.Ansi, SetLastError=true, EntryPoint="shm_unlink")>]
                extern int shmunlink(string name)

                [<DllImport("libc", CharSet = CharSet.Ansi, SetLastError=true)>]
                extern int ftruncate(FileHandle fd, unativeint size)

                [<DllImport("libc", CharSet = CharSet.Ansi, SetLastError=true)>]
                extern int close(FileHandle fd)

                [<DllImport("libc", CharSet = CharSet.Ansi, SetLastError=true, EntryPoint="strerror")>]
                extern nativeint strerror(int code)

            let private fail (message: string) (code: int) =
                let error = code |> LibC.strerror |> Marshal.PtrToStringAnsi
                failwith $"{message} (ERROR: {error})"

            let create (name: string) (openExisting: bool) (access: SharedMemoryAccess) (size: int64) =
                let mapName = if name.StartsWith("/") then name else "/" + name

                let flags =
                    if openExisting then
                        if access.HasFlag SharedMemoryAccess.Write then
                            SharedMemoryFlags.ReadWrite
                        else
                            SharedMemoryFlags.ReadOnly
                    else
                        SharedMemoryFlags.Create ||| SharedMemoryFlags.Exclusive ||| SharedMemoryFlags.ReadWrite

                let fd = LibC.shmopen(mapName, flags, Permission(access.FileMode))
                if not fd.IsValid then
                    let err = Marshal.GetLastWin32Error()
                    fail $"Could not open shared memory object \"{mapName}\"" err

                if not openExisting && LibC.ftruncate(fd, unativeint size) <> 0 then
                    let err = Marshal.GetLastWin32Error()
                    LibC.shmunlink mapName |> ignore
                    fail $"Could not resize shared memory object \"{mapName}\" to {size} bytes" err

                let ptr = LibC.mmap(0n, unativeint size, access.Protection, MapFlags.Shared, fd, 0un)
                if ptr = -1n then
                    let err = Marshal.GetLastWin32Error()
                    if not openExisting then LibC.shmunlink mapName |> ignore
                    fail $"Could not map shared memory object \"{mapName}\"" err

                { new ISharedMemory with
                    member _.Name = name
                    member _.Size = size
                    member _.Access = access
                    member _.Pointer = ptr
                    member _.Dispose() =
                        if LibC.munmap(ptr, unativeint size) <> 0 then
                            let err = Marshal.GetLastWin32Error()
                            LibC.close fd |> ignore
                            LibC.shmunlink mapName |> ignore
                            fail $"Could not unmap shared memory object \"{mapName}\"" err

                        if LibC.close fd <> 0 then
                            let err = Marshal.GetLastWin32Error()
                            if not openExisting then LibC.shmunlink mapName |> ignore
                            fail $"Could not close shared memory file \"{mapName}\"" err

                        if not openExisting then
                            LibC.shmunlink mapName |> ignore // May have been closed by someone else -> fails
                }

open SharedMemoryImpl

[<AbstractClass; Sealed>]
type SharedMemory =

    static let newName() =
        let str = Convert.ToBase64String(Guid.NewGuid().ToByteArray())
        str.TrimEnd('=').Replace("/", "-")

    static member private CreateOrOpen(name: string, sizeInBytes: int64, access: SharedMemoryAccess, openExisting: bool) =
        if RuntimeInformation.IsOSPlatform OSPlatform.Windows then
            Windows.create name openExisting access sizeInBytes
        elif RuntimeInformation.IsOSPlatform OSPlatform.OSX then
            Posix.Mac.create name openExisting access sizeInBytes
        elif RuntimeInformation.IsOSPlatform OSPlatform.Linux then
            Posix.Linux.create name openExisting access sizeInBytes
        else
            raise <| NotSupportedException("Shared memory not support on current platform.")

    /// <summary>
    /// Creates a new shared memory object and maps it.
    /// Fails if a shared memory object with the given name already exists.
    /// </summary>
    /// <param name="name">Name of the shared memory object.</param>
    /// <param name="sizeInBytes">Size of the shared memory object (in bytes).</param>
    /// <param name="access">Access permissions. Default is read and write.</param>
    /// <returns>The mapped shared memory object.</returns>
    static member Create(name: string, sizeInBytes: int64,
                         [<Optional; DefaultParameterValue(SharedMemoryAccess.Write)>] access: SharedMemoryAccess) =
        SharedMemory.CreateOrOpen(name, sizeInBytes, access, openExisting = false)

    /// <summary>
    /// Creates a new shared memory object with a randomly generated name and maps it.
    /// </summary>
    /// <param name="sizeInBytes">Size of the shared memory object (in bytes).</param>
    /// <param name="access">Access permissions. Default is read and write.</param>
    /// <returns>The mapped shared memory object.</returns>
    static member Create(sizeInBytes: int64,
                         [<Optional; DefaultParameterValue(SharedMemoryAccess.Write)>] access: SharedMemoryAccess) =
        let mutable result = null
        let mutable retry = 1

        while isNull result do
            try
                result <- SharedMemory.CreateOrOpen(newName(), sizeInBytes, access, openExisting = false)
            with _ ->
                if retry >= 3 then reraise()
                inc &retry
        result

    /// <summary>
    /// Opens a shared memory object and maps it.
    /// Fails if a shared memory object with the given name does not exist.
    /// </summary>
    /// <param name="name">Name of the shared memory object.</param>
    /// <param name="sizeInBytes">Size of the shared memory object (in bytes).</param>
    /// <param name="access">Access permissions. Default is read and write.</param>
    /// <returns>The mapped shared memory object.</returns>
    static member Open(name: string, sizeInBytes: int64,
                       [<Optional; DefaultParameterValue(SharedMemoryAccess.Write)>] access: SharedMemoryAccess) =
        SharedMemory.CreateOrOpen(name, sizeInBytes, access, openExisting = true)