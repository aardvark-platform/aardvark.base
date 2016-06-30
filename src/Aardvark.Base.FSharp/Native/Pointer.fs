namespace Aardvark.Base.Native

#nowarn "9"


open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
open Aardvark.Base

[<AbstractClass>]
type ptr() =
    static let nullptr = 
        { new ptr() with 
            member x.Pointer = 0n
            member x.IsValid = false
        }

    abstract member Pointer : nativeint
    abstract member IsValid : bool
    abstract member RealPointer : ptr
    default x.RealPointer = x


    member inline x.Read<'a when 'a : unmanaged>() = NativePtr.read (NativePtr.ofNativeInt<'a> x.Pointer)
    member inline x.Write<'a when 'a : unmanaged>(value : 'a) = NativePtr.write (NativePtr.ofNativeInt<'a> x.Pointer) value
    member inline x.Get<'a when 'a : unmanaged>(index : int) = NativePtr.get (NativePtr.ofNativeInt<'a> x.Pointer) index
    member inline x.Set<'a when 'a : unmanaged>(index : int, value : 'a) = NativePtr.set (NativePtr.ofNativeInt<'a> x.Pointer) index value

    member x.Write(source : byte[], offset : int64, length : int64) =
        let gc = GCHandle.Alloc(source, GCHandleType.Pinned)
        try
            let src = gc.AddrOfPinnedObject()
            Marshal.Copy(src + nativeint offset, x.Pointer, length)
        finally
            gc.Free()

    member x.Read(target : byte[], offset : int64, length : int64) =
        let gc = GCHandle.Alloc(target, GCHandleType.Pinned)
        try
            let src = gc.AddrOfPinnedObject()
            Marshal.Copy(x.Pointer, src + nativeint offset, length)
        finally
            gc.Free()

    member inline x.CopyTo(target : nativeint, length : int64) =
        Marshal.Copy(x.Pointer, target, length)

    member inline x.CopyTo(target : ptr, length : int64) =
        Marshal.Copy(x.Pointer, target.Pointer, length)

    member x.CopyTo(target : byte[], offset : int64, length : int64) =
        let gc = GCHandle.Alloc(target, GCHandleType.Pinned)
        try
            let src = gc.AddrOfPinnedObject()
            Marshal.Copy(x.Pointer, src + nativeint offset, length)
        finally
            gc.Free()

    member inline x.CopyFrom(source : nativeint, length : int64) =
        Marshal.Copy(source, x.Pointer, length)

    member inline x.CopyFrom(source : ptr, length : int64) =
        Marshal.Copy(source.Pointer, x.Pointer, length)

    member x.CopyFrom(source : byte[], offset : int64, length : int64) =
        let gc = GCHandle.Alloc(source, GCHandleType.Pinned)
        try
            let src = gc.AddrOfPinnedObject()
            Marshal.Copy(src + nativeint offset, x.Pointer, length)
        finally
            gc.Free()


    member x.ToByteArray (length : int64) =
        let arr : byte[] = Array.zeroCreate (int length)
        x.CopyTo(arr, 0L, length)
        arr


    override x.ToString() =
        sprintf "ptr 0x%X" x.Pointer

    override x.GetHashCode() =
        if x.IsValid then 
            x.Pointer.GetHashCode()
        else
            0

    override x.Equals o =
        match o with
            | :? ptr as o -> 
                match x.IsValid, o.IsValid with
                    | true, true -> x.Pointer = o.Pointer
                    | false, false -> true
                    | _ -> false
            | _ ->
                false

    static member Zero = nullptr
    static member Null = nullptr

    static member (+) (p : ptr, offset : nativeint) = 
        match p with
            | :? ViewPointer as vp -> 
                let o = vp.Offset + offset
                if o = 0n then vp.Source
                else ViewPointer(vp.Source, o) :> ptr
            | _ ->
                ViewPointer(p, offset) :> ptr

    static member inline (-) (p : ptr, offset : nativeint) = p + (-offset)
    static member inline (+) (p : ptr, offset : 'a) = p + nativeint offset
    static member inline (-) (p : ptr, offset : 'a) = p + nativeint -offset

    static member inline (+) (offset : nativeint, p : ptr) = p + offset
    static member inline (+) (offset : 'a, p : ptr) = p + nativeint offset

    static member inline op_Equality (ptr : ptr, v : nativeint) = ptr.Pointer = v

and private ViewPointer(p : ptr, offset : nativeint) =
    inherit ptr()

    member x.Source = p
    member x.Offset = offset

    override x.IsValid = p.IsValid
    override x.Pointer = p.Pointer + offset

type ptr<'a when 'a : unmanaged>(p : ptr) =
    inherit ptr()
    static let nullptr = ptr<'a>(ptr.Null)

    static let sa = sizeof<'a>
    static let sa64 = int64 sa

    override x.Pointer = p.Pointer
    override x.RealPointer = p
    override x.IsValid = p.IsValid

    static member Zero = nullptr
    static member  Null = nullptr

    member x.Item
        with get (i : int) : 'a = NativePtr.get (NativePtr.ofNativeInt p.Pointer) i
        and set (i : int) (value : 'a) = NativePtr.set (NativePtr.ofNativeInt p.Pointer) i value

    member x.Value 
        with get() : 'a = NativePtr.read (NativePtr.ofNativeInt p.Pointer)
        and set (v : 'a) = NativePtr.write (NativePtr.ofNativeInt p.Pointer) v


    member x.CopyTo(target : nativeint, count : int64) =
        Marshal.Copy(x.Pointer, target, sa64 * count)

    member x.CopyTo(target : ptr, count : int64) =
        Marshal.Copy(x.Pointer, target.Pointer, sa64 * count)

    member x.CopyTo(target : ptr<'a>, count : int64) =
        Marshal.Copy(x.Pointer, target.Pointer, sa64 * count)

    member x.CopyTo(target : 'a[], offset : int64, count : int64) =
        let gc = GCHandle.Alloc(target, GCHandleType.Pinned)
        try
            let src = gc.AddrOfPinnedObject()
            Marshal.Copy(x.Pointer, src + nativeint offset * nativeint sa, count * sa64)
        finally
            gc.Free()



    member x.CopyFrom(source : nativeint, count : int64) =
        Marshal.Copy(source, x.Pointer, sa64 * count)

    member x.CopyFrom(source : ptr, count : int64) =
        Marshal.Copy(source.Pointer, x.Pointer, sa64 * count)

    member x.CopyFrom(source : ptr<'a>, count : int64) =
        Marshal.Copy(source.Pointer, x.Pointer, sa64 * count)

    member x.CopyFrom(source : 'a[], offset : int64, count : int64) =
        let gc = GCHandle.Alloc(source, GCHandleType.Pinned)
        try
            let src = gc.AddrOfPinnedObject()
            Marshal.Copy(src + nativeint offset * nativeint sa, x.Pointer, count * sa64)
        finally
            gc.Free()


    member x.ToArray(count : int) =
        let target : 'a[] = Array.zeroCreate count
        x.CopyTo(target, 0L, int64 count)
        target


    static member (+) (l : ptr<'a>, index : int) = ptr<'a>(l.RealPointer + sa * index)
    static member (-) (l : ptr<'a>, index : int) = ptr<'a>(l.RealPointer - sa * index)
    static member (+) (l : ptr<'a>, index : int64) = ptr<'a>(l.RealPointer + sa64 * index)
    static member (-) (l : ptr<'a>, index : int64) = ptr<'a>(l.RealPointer - sa64 * index)

[<AbstractClass>]
type sizedptr() =
    inherit ptr()

    static let nullptr = 
        { new sizedptr() with 
            member x.Pointer = 0n
            member x.IsValid = false
            member x.Size = 0L
        }

    static member Null = nullptr
    static member Zero = nullptr

    abstract member Size : int64

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Ptr =
    let zero = ptr.Zero

    let inline isValid (p : ptr) = p.IsValid
    let inline isNull (p : ptr) = not p.IsValid

    let inline cast<'a when 'a : unmanaged> (p : ptr) : ptr<'a> =
        ptr<'a>(p.RealPointer)
    
    let inline get (i : int) (p : ptr<'a>) = p.[i]
    let inline set (i : int) (value : 'a) (p : ptr<'a>) = p.[i] <- value

    let inline read (p : ptr<'a>) = p.Value
    let inline write (value : 'a) (p : ptr<'a>) = p.Value <- value


    let inline toByteArray (length : int64) (ptr : ptr) = ptr.ToByteArray(length)
    let inline toArray (length : int) (ptr : ptr<'a>) = ptr.ToArray(length)
