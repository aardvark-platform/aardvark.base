namespace Aardvark.Base

open System
open System.Reflection
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop

#nowarn "9"



module NativeTensors =

    type NativeVolume<'a when 'a : unmanaged> =
        struct
            val mutable public DX : nativeint
            val mutable public DY : nativeint
            val mutable public DZ : nativeint
            val mutable public SX : nativeint
            val mutable public SY : nativeint
            val mutable public SZ : nativeint
            val mutable public Origin : nativeptr<'a>

            member inline x.ForEachPtr(f : nativeptr<'a> -> unit) =
            
                let mutable i = NativePtr.toNativeInt x.Origin

                let zs = x.SZ * x.DZ
                let zj = x.DZ - x.SY * x.DY

                let ys = x.SY * x.DY
                let yj = x.DY - x.SX * x.DX

                let xs = x.SX * x.DX
                let xj = x.DX


                let ze = i + zs
                while i <> ze do
                    let ye = i + ys
                    while i <> ye do
                        let xe = i + xs
                        while i <> xe do
                            f (NativePtr.ofNativeInt i) 
                            i <- i + xj

                        i <- i + yj

                    i <- i + zj



                ()


            member inline private x.ForEachPtrZYX(other : NativeVolume<'b>, f : nativeptr<'a> -> nativeptr<'b> -> unit) =  
                let mutable i = NativePtr.toNativeInt x.Origin
                let mutable i1 = NativePtr.toNativeInt other.Origin

                let zs = x.SZ * x.DZ
                let zj = x.DZ - x.SY * x.DY
                let zj1 = other.DZ - other.SY * other.DY

                let ys = x.SY * x.DY
                let yj = x.DY - x.SX * x.DX
                let yj1 = other.DY - other.SX * other.DX

                let xs = x.SX * x.DX
                let xj = x.DX
                let xj1 = other.DX


                let ze = i + zs
                while i <> ze do
                    let ye = i + ys
                    while i <> ye do
                        let xe = i + xs
                        while i <> xe do
                            f (NativePtr.ofNativeInt i) (NativePtr.ofNativeInt i1)
                            i <- i + xj
                            i1 <- i1 + xj1

                        i <- i + yj
                        i1 <- i1 + yj1

                    i <- i + zj
                    i1 <- i1 + zj1



                ()

            member inline private x.ForEachPtrYXZ(other : NativeVolume<'b>, f : nativeptr<'a> -> nativeptr<'b> -> unit) =  
                let mutable i = NativePtr.toNativeInt x.Origin
                let mutable i1 = NativePtr.toNativeInt other.Origin
    
                let ys = x.SY * x.DY
                let yj = x.DY - x.SX * x.DX
                let yj1 = other.DY - other.SX * other.DX

                let xs = x.SX * x.DX
                let xj = x.DX - x.SZ * x.DZ
                let xj1 = other.DX - other.SZ * other.DZ

                let zs = x.SZ * x.DZ
                let zj = x.DZ
                let zj1 = other.DZ


                let ye = i + ys
                while i <> ye do
                    let xe = i + xs
                    while i <> xe do
                        let ze = i + zs
                        while i <> ze do
                            f (NativePtr.ofNativeInt i) (NativePtr.ofNativeInt i1)
                            i <- i + zj
                            i1 <- i1 + zj1

                        i <- i + xj
                        i1 <- i1 + xj1

                    i <- i + yj
                    i1 <- i1 + yj1

            member inline private x.ForEachPtrXYZ(other : NativeVolume<'b>, f : nativeptr<'a> -> nativeptr<'b> -> unit) =  
                let mutable i = NativePtr.toNativeInt x.Origin
                let mutable i1 = NativePtr.toNativeInt other.Origin
    
                let xs = x.SX * x.DX
                let xj = x.DX - x.SY * x.DY
                let xj1 = other.DX - other.SY * other.DY

                let ys = x.SY * x.DY
                let yj = x.DY - x.SZ * x.DZ
                let yj1 = other.DY - other.SZ * other.DZ

                let zs = x.SZ * x.DZ
                let zj = x.DZ
                let zj1 = other.DZ


                let xe = i + xs
                while i <> xe do
                    let ye = i + ys
                    while i <> ye do
                        let ze = i + zs
                        while i <> ze do
                            f (NativePtr.ofNativeInt i) (NativePtr.ofNativeInt i1)
                            i <- i + zj
                            i1 <- i1 + zj1

                        i <- i + yj
                        i1 <- i1 + yj1

                    i <- i + xj
                    i1 <- i1 + xj1



            member inline x.ForEachPtr(other : NativeVolume<'b>, f : nativeptr<'a> -> nativeptr<'b> -> unit) =  
                if x.SX <> other.SX || x.SY <> other.SY || x.SZ <> other.SZ then
                    failwithf "NativeVolume sizes do not match { src = %A; dst = %A }" (x.SX, x.SY, x.SY) (other.SX, other.SY, other.SY)

                let cxy = compare (abs x.DX) (abs x.DY)
                let cxz = compare (abs x.DX) (abs x.DZ)
                let cyz = compare (abs x.DY) (abs x.DZ)

                if cxz > 0 && cyz > 0 then
                    // z is mincomponent
                    if cxy < 0 then 
                        x.ForEachPtrYXZ(other, f) // typical piximage case
                    else 
                        x.ForEachPtrXYZ(other, f)            // transposed image
                elif cxy > 0 && cyz < 0 then
                    // y is mincomponent (very rare)
                    // TODO: efficiency 
                    x.ForEachPtrZYX(other, f)
                else
                    // x is mincomponent
                    x.ForEachPtrZYX(other, f)

            new(ptr : nativeptr<'a>, vi : VolumeInfo) =
                let sa = int64 sizeof<'a>
                {
                    DX = nativeint (sa * vi.DX); DY = nativeint (sa * vi.DY); DZ = nativeint (sa * vi.DZ)
                    SX = nativeint vi.SX; SY = nativeint vi.SY; SZ = nativeint vi.SZ
                    Origin = NativePtr.ofNativeInt (NativePtr.toNativeInt ptr + nativeint (sa * vi.Origin))
                }

        end

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module NativeVolume =
        open System.Reflection

        let inline ofNativeInt (info : VolumeInfo) (ptr : nativeint) =
            NativeVolume(NativePtr.ofNativeInt ptr, info)

        let inline iter (f : nativeptr<'a>-> unit) (l : NativeVolume<'a>) =
            l.ForEachPtr(f) 
        
        let inline iter2 (l : NativeVolume<'a>) (r : NativeVolume<'b>) (f : nativeptr<'a> -> nativeptr<'b> -> unit) =
            l.ForEachPtr(r, f) 

        let pin (f : NativeVolume<'a> -> 'b) (pi : PixImage<'a>) : 'b =
            let gc = GCHandle.Alloc(pi.Array, GCHandleType.Pinned)
            let nv = gc.AddrOfPinnedObject() |> ofNativeInt pi.VolumeInfo
            try
                f nv
            finally
                gc.Free()

        let pin2 (l : PixImage<'a>) (r : PixImage<'b>) (f : NativeVolume<'a> -> NativeVolume<'b> -> 'c)  : 'c =
            let lgc = GCHandle.Alloc(l.Array, GCHandleType.Pinned)
            let lv = lgc.AddrOfPinnedObject() |> ofNativeInt l.VolumeInfo

            let rgc = GCHandle.Alloc(r.Array, GCHandleType.Pinned)
            let rv = rgc.AddrOfPinnedObject() |> ofNativeInt r.VolumeInfo

            try
                f lv rv
            finally
                lgc.Free()
                rgc.Free()
  
        type private CopyImpl<'a when 'a : unmanaged>() =
            static member CopyImageToNative (src : PixImage<'a>, dst : nativeint, dstInfo : VolumeInfo) =
                let dst = NativeVolume(NativePtr.ofNativeInt dst, dstInfo)
                src |> pin (fun src ->
                    iter2 src dst (fun s d -> NativePtr.write d (NativePtr.read s))
                )

            static member CopyNativeToImage (src : nativeint, srcInfo : VolumeInfo, dst : PixImage<'a>) =
                let src = NativeVolume(NativePtr.ofNativeInt src, srcInfo)
                dst |> pin (fun dst ->
                    iter2 src dst (fun s d -> NativePtr.write d (NativePtr.read s))
                )

        let copy (src : PixImage<'a>) (dst : NativeVolume<'a>) =
            src |> pin (fun src ->
                iter2 src dst (fun s d -> NativePtr.write d (NativePtr.read s))
            )

        let copyImageToNative (src : PixImage) (dst : nativeint) (dstInfo : VolumeInfo) =
            let t = typedefof<CopyImpl<byte>>.MakeGenericType [| src.PixFormat.Type |]
            let mi = t.GetMethod("CopyImageToNative", BindingFlags.Static ||| BindingFlags.NonPublic ||| BindingFlags.Public)
            mi.Invoke(null, [|src; dst; dstInfo|]) |> ignore

        let copyNativeToImage (src : nativeint) (srcInfo : VolumeInfo) (dst : PixImage)=
            let t = typedefof<CopyImpl<byte>>.MakeGenericType [| dst.PixFormat.Type |]
            let mi = t.GetMethod("CopyNativeToImage", BindingFlags.Static ||| BindingFlags.NonPublic ||| BindingFlags.Public)
            mi.Invoke(null, [|src; srcInfo; dst|]) |> ignore