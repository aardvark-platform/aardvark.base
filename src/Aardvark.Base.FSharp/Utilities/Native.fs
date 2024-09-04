namespace Aardvark.Base

#nowarn "9"

open System
open System.Runtime.InteropServices
open FSharp.NativeInterop

[<AutoOpen>]
module NativeUtilities =

    let private os = System.Environment.OSVersion

    /// <summary>
    /// MSVCRT wraps memory-functions provided by msvcrt.dll on windows systems.
    /// </summary>
    module internal MSVCRT =

        [<DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memcpy_internal(nativeint dest, nativeint src, UIntPtr size);

        [<DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern int private memcmp_internal(nativeint ptr1, nativeint ptr2, UIntPtr size);

        [<DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memset_internal(nativeint ptr, int value, UIntPtr size);

        [<DllImport("msvcrt.dll", EntryPoint = "memmove", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memmove_internal(nativeint dest, nativeint src, UIntPtr size);

        let memcpy(target : nativeint, source : nativeint, size : unativeint) =
            memcpy_internal(target, source, size) |> ignore

        let memcmp(ptr1 : nativeint, ptr2 : nativeint, size : unativeint) =
            memcmp_internal(ptr1, ptr2, size)

        let memset(ptr : nativeint, value : int, size : unativeint) =
            memset_internal(ptr, value, size) |> ignore

        let memmove(target : nativeint, source : nativeint, size : unativeint) =
            memmove_internal(target, source, size) |> ignore

    /// <summary>
    /// LibC wraps memory-functions provided by libc on linux systems.
    /// </summary>
    module internal LibC =

        [<DllImport("libc", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memcpy_internal(nativeint dest, nativeint src, UIntPtr size);

        [<DllImport("libc", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern int private memcmp_internal(nativeint ptr1, nativeint ptr2, UIntPtr size);

        [<DllImport("libc", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memset_internal(nativeint ptr, int value, UIntPtr size);

        [<DllImport("libc", EntryPoint = "memmove", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern nativeint private memmove_internal(nativeint dest, nativeint src, UIntPtr size);

        [<DllImport("libc", EntryPoint = "uname", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
        extern int private uname_intern(nativeint buf);

        let mutable osname = null
        let uname() =
            if isNull osname then
                let ptr : nativeptr<byte> = NativePtr.stackalloc 8192
                if uname_intern(NativePtr.toNativeInt ptr) = 0 then
                    osname <- Marshal.PtrToStringAnsi(NativePtr.toNativeInt ptr)
                else
                    failwith "could not get os-name"
            osname

        let memcpy(target : nativeint, source : nativeint, size : unativeint) =
            memcpy_internal(target, source, size) |> ignore

        let memcmp(ptr1 : nativeint, ptr2 : nativeint, size : unativeint) =
            memcmp_internal(ptr1, ptr2, size)

        let memset(ptr : nativeint, value : int, size : unativeint) =
            memset_internal(ptr, value, size) |> ignore

        let memmove(target : nativeint, source : nativeint, size : unativeint) =
            memmove_internal(target, source, size) |> ignore

    [<AutoOpen>]
    module PlatformStuff =

        let (|Windows|Linux|Mac|) (p : System.OperatingSystem) =
            match p.Platform with
                | System.PlatformID.Unix ->
                    if LibC.uname() = "Darwin" then Mac
                    else Linux
                | System.PlatformID.MacOSX -> Mac
                | _ -> Windows

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module NativeInt =
        let memcpy (src : nativeint) (dst : nativeint) (size : int) =
            match os with
                | Windows -> MSVCRT.memcpy(dst, src, unativeint size)
                | _ -> LibC.memcpy(dst, src, unativeint size)

        let memmove (src : nativeint) (dst : nativeint) (size : int) =
            match os with
                | Windows -> MSVCRT.memmove(dst, src, unativeint size)
                | _ -> LibC.memmove(dst, src, unativeint size)

        let memset (dst : nativeint) (value : int) (size : int) =
            match os with
                | Windows -> MSVCRT.memset(dst, value, unativeint size)
                | _ -> LibC.memset(dst, value, unativeint size)

        let memcmp (src : nativeint) (dst : nativeint) (size : int) =
            match os with
                | Windows -> MSVCRT.memcmp(dst, src, unativeint size)
                | _ -> LibC.memcmp(dst, src, unativeint size)

        let inline read<'a when 'a : unmanaged> (ptr : nativeint) =
            NativePtr.read (NativePtr.ofNativeInt<'a> ptr)

        let inline write<'a when 'a : unmanaged> (ptr : nativeint) (value : 'a) =
            NativePtr.write (NativePtr.ofNativeInt<'a> ptr)  value

        let inline get<'a when 'a : unmanaged> (ptr : nativeint) (index : int) =
            NativePtr.get (NativePtr.ofNativeInt<'a> ptr) index

        let inline set<'a when 'a : unmanaged> (ptr : nativeint) (index : int) (value : 'a)=
            NativePtr.set (NativePtr.ofNativeInt<'a> ptr) index value

        /// Pins the given object and invokes the action with its address.
        let inline pin ([<InlineIfLambda>] action: nativeint -> 'T) (value: obj) =
            let gc = GCHandle.Alloc(value, GCHandleType.Pinned)
            try action <| gc.AddrOfPinnedObject()
            finally gc.Free()

    type Marshal with
        static member Copy(source : nativeint, destination : nativeint, length : unativeint) =
            match os with
                | Windows -> MSVCRT.memcpy(destination, source, length)
                | _ -> LibC.memcpy(destination, source, length)

        static member Move(source : nativeint, destination : nativeint, length : unativeint) =
            match os with
                | Windows -> MSVCRT.memmove(destination, source, length)
                | _ -> LibC.memmove(destination, source, length)

        static member Set(memory : nativeint, value : int, length : unativeint) =
            match os with
                | Windows -> MSVCRT.memset(memory, value, length)
                | _ -> LibC.memset(memory, value, length)

        static member Compare(source : nativeint, destination : nativeint, length : unativeint) =
            match os with
                | Windows -> MSVCRT.memcmp(destination, source, length)
                | _ -> LibC.memcmp(destination, source, length)


        static member Copy(source : nativeint, destination : nativeint, length : int) =
            Marshal.Copy(source, destination, unativeint length)

        static member Move(source : nativeint, destination : nativeint, length : int) =
            Marshal.Move(source, destination, unativeint length)

        static member Set(memory : nativeint, value : int, length : int) =
            Marshal.Set(memory, value, unativeint length)

        static member Compare(source : nativeint, destination : nativeint, length : int) =
            Marshal.Compare(source, destination, unativeint length)


        static member inline Copy(source : nativeint, destination : nativeint, length : 'a) =
            Marshal.Copy(source, destination, unativeint length)

        static member inline Move(source : nativeint, destination : nativeint, length : 'a) =
            Marshal.Move(source, destination, unativeint length)

        static member inline Set(memory : nativeint, value : int, length : 'a) =
            Marshal.Set(memory, value, unativeint length)

        static member inline Compare(source : nativeint, destination : nativeint, length : 'a) =
            Marshal.Compare(source, destination, unativeint length)

    [<Obsolete("Use NativeInt.pin instead.")>]
    let pinned (a : obj) f =
        NativeInt.pin f a

    [<Obsolete("Use NativePtr.pin instead.")>]
    let inline pin ([<InlineIfLambda>] f: nativeptr<'T> -> 'U) (value: 'T) =
        NativePtr.pin f value

    [<Obsolete("Use NativePtr.pinArr instead.")>]
    let inline pinArr ([<InlineIfLambda>] f: nativeptr<'T> -> 'U) (array: 'T[])  =
        NativePtr.pinArr f array

[<AutoOpen>]
module MarshalDelegateExtensions =
    open System.Collections.Concurrent

    let private pinnedDelegates = ConcurrentDictionary<Delegate, nativeint>()
    type PinnedDelegate internal(d : Delegate, ptr : nativeint) =
        member x.Pointer = ptr
        member x.Dispose() = pinnedDelegates.TryRemove d |> ignore

        interface IDisposable with
            member x.Dispose() = x.Dispose()

    type Marshal with
        static member PinDelegate(d : Delegate) =
            let ptr = pinnedDelegates.GetOrAdd(d, fun _ -> Marshal.GetFunctionPointerForDelegate d)
            new PinnedDelegate(d, ptr)

        static member PinFunction(f : 'a -> 'b) =
            Marshal.PinDelegate(Func<'a, 'b>(f))

        static member PinFunction(f : 'a -> 'b -> 'c) =
            Marshal.PinDelegate(Func<'a, 'b, 'c>(f))

        static member PinFunction(f : 'a -> 'b -> 'c -> 'd) =
            Marshal.PinDelegate(Func<'a, 'b, 'c, 'd>(f))