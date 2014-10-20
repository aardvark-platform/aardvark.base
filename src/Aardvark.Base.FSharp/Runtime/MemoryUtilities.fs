#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif

open System.Linq.Expressions
open System.Runtime.InteropServices
open Microsoft.FSharp.Reflection
open System.Reflection
open System.Reflection.Emit
open System

/// <summary>
/// MSVCRT wraps memory-functions provided by msvcrt.dll on windows systems.
/// </summary>
module private MSVCRT =
    open System
    open System.Runtime.InteropServices

    [<DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
    extern nativeint private memcpy_internal(nativeint dest, nativeint src, UIntPtr size);

    [<DllImport("msvcrt.dll", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
    extern int private memcmp_internal(nativeint ptr1, nativeint ptr2, UIntPtr size);

    [<DllImport("msvcrt.dll", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
    extern nativeint private memset_internal(nativeint ptr, int value, UIntPtr size);

    [<DllImport("msvcrt.dll", EntryPoint = "memmove", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
    extern nativeint private memmove_internal(nativeint dest, nativeint src, UIntPtr size);


    let memcpy(target : nativeint, source : nativeint, size : int) =
        memcpy_internal(target, source, UIntPtr (uint32 size)) |> ignore

    let memcmp(ptr1 : nativeint, ptr2 : nativeint, size : int) =
        memcmp_internal(ptr1, ptr2, UIntPtr (uint32 size))

    let memset(ptr : nativeint, value : int, size : int) =
        memset_internal(ptr, value, UIntPtr (uint32 size)) |> ignore

    let memmove(target : nativeint, source : nativeint, size : int) =
        memmove_internal(target, source, UIntPtr (uint32 size)) |> ignore

/// <summary>
/// LibC wraps memory-functions provided by libc on linux systems.
/// </summary>
module private LibC =
    open System
    open System.Runtime.InteropServices

    [<DllImport("libc", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
    extern nativeint private memcpy_internal(nativeint dest, nativeint src, UIntPtr size);

    [<DllImport("libc", EntryPoint = "memcmp", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
    extern int private memcmp_internal(nativeint ptr1, nativeint ptr2, UIntPtr size);

    [<DllImport("libc", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
    extern nativeint private memset_internal(nativeint ptr, int value, UIntPtr size);

    [<DllImport("libc", EntryPoint = "memmove", CallingConvention = CallingConvention.Cdecl, SetLastError = false)>]
    extern nativeint private memmove_internal(nativeint dest, nativeint src, UIntPtr size);


    let memcpy(target : nativeint, source : nativeint, size : int) =
        memcpy_internal(target, source, UIntPtr (uint32 size)) |> ignore

    let memcmp(ptr1 : nativeint, ptr2 : nativeint, size : int) =
        memcmp_internal(ptr1, ptr2, UIntPtr (uint32 size))

    let memset(ptr : nativeint, value : int, size : int) =
        memset_internal(ptr, value, UIntPtr (uint32 size)) |> ignore

    let memmove(target : nativeint, source : nativeint, size : int) =
        memmove_internal(target, source, UIntPtr (uint32 size)) |> ignore

/// <summary>
/// extends Marshal with some missing functions (such as memcpy) by using the
/// platform specific functions defined above.
/// </summary>
[<AutoOpen>]
module MarshalExtensions =
    open System.Runtime.InteropServices

    let private os = System.Environment.OSVersion
    let private notimp() = raise <| NotImplementedException()

    type Marshal with
        static member Copy(source : nativeint, destination : nativeint, length : int) =
            match os with
                | Windows -> MSVCRT.memcpy(destination, source, length)
                | Linux -> LibC.memcpy(destination, source, length)
                | Mac -> notimp()

        static member Move(source : nativeint, destination : nativeint, length : int) =
            match os with
                | Windows -> MSVCRT.memmove(destination, source, length)
                | Linux -> LibC.memmove(destination, source, length)
                | Mac -> notimp()

        static member Set(memory : nativeint, value : int, length : int) =
            match os with
                | Windows -> MSVCRT.memset(memory, value, length)
                | Linux -> LibC.memset(memory, value, length)
                | Mac -> notimp()

        static member Compare(source : nativeint, destination : nativeint, length : int) =
            match os with
                | Windows -> MSVCRT.memcmp(destination, source, length)
                | Linux -> LibC.memcmp(destination, source, length)
                | Mac -> notimp()