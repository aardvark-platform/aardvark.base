namespace Aardvark.Base

open System
open Microsoft.FSharp.NativeInterop
open System.Runtime.InteropServices
open System.Runtime.CompilerServices


#nowarn "9"
#nowarn "51"
#nowarn "1337"

[<AutoOpen>]
module ``NativeTensor Pinning`` =
    [<CompilerMessage("use NativeTensor.using instead", 1337, IsHidden = true)>]
    type Pinning private() =

        static let pin (a : 'a) (f : nativeint -> 'r) =
            let gc = GCHandle.Alloc(a, GCHandleType.Pinned)
            try f (gc.AddrOfPinnedObject())
            finally gc.Free()

        // =======================================================================================
        // ARRAYS and TENSORS
        // =======================================================================================
        static member Using(m : byref<Vector<'a>>, action : NativeVector<'a> -> 'r) =
            NativeVector.using m action
            
        static member Using(m : byref<'a[]>, action : NativeVector<'a> -> 'r) =
            use ptr = fixed m
            let vector =
                NativeVector<'a>(
                    ptr,
                    VectorInfo(
                        0L,
                        int64 m.Length,
                        1L
                    )
                )
            action vector

        static member Using(m : byref<Matrix<'a>>, action : NativeMatrix<'a> -> 'r) =
            NativeMatrix.using m action
            
        static member Using(m : byref<'a[,]>, action : NativeMatrix<'a> -> 'r) =
            let r = m.GetLongLength(0)
            let c = m.GetLongLength(1)
            pin m (fun pM ->
                let matrix =
                    NativeMatrix<'a>(
                        NativePtr.ofNativeInt pM,
                        MatrixInfo(
                            0L,
                            V2l(c, r),
                            V2l(1L, c)
                        )
                    )
                action matrix
            )

        // =======================================================================================
        // VECTORS
        // =======================================================================================

        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V2i>, action : NativeVector<int> -> 'r) =
            let vector =
                NativeVector<int>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        2L,
                        1L
                    )
                )
            action vector
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V3i>, action : NativeVector<int> -> 'r) =
            let vector =
                NativeVector<int>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        3L,
                        1L
                    )
                )
            action vector
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V4i>, action : NativeVector<int> -> 'r) =
            let vector =
                NativeVector<int>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        4L,
                        1L
                    )
                )
            action vector
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V2l>, action : NativeVector<int64> -> 'r) =
            let vector =
                NativeVector<int64>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        2L,
                        1L
                    )
                )
            action vector
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V3l>, action : NativeVector<int64> -> 'r) =
            let vector =
                NativeVector<int64>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        3L,
                        1L
                    )
                )
            action vector
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V4l>, action : NativeVector<int64> -> 'r) =
            let vector =
                NativeVector<int64>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        4L,
                        1L
                    )
                )
            action vector
   
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V2f>, action : NativeVector<float32> -> 'r) =
            let vector =
                NativeVector<float32>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        2L,
                        1L
                    )
                )
            action vector
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V3f>, action : NativeVector<float32> -> 'r) =
            let vector =
                NativeVector<float32>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        3L,
                        1L
                    )
                )
            action vector
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V4f>, action : NativeVector<float32> -> 'r) =
            let vector =
                NativeVector<float32>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        4L,
                        1L
                    )
                )
            action vector
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V2d>, action : NativeVector<float> -> 'r) =
            let vector =
                NativeVector<float>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        2L,
                        1L
                    )
                )
            action vector
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V3d>, action : NativeVector<float> -> 'r) =
            let vector =
                NativeVector<float>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        3L,
                        1L
                    )
                )
            action vector
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<V4d>, action : NativeVector<float> -> 'r) =
            let vector =
                NativeVector<float>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        4L,
                        1L
                    )
                )
            action vector
 
        // =======================================================================================
        // COLORS
        // =======================================================================================
        
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<C3b>, action : NativeVector<byte> -> 'r) =
            let vector =
                NativeVector<byte>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        3L,
                        1L
                    )
                )
            action vector
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<C4b>, action : NativeVector<byte> -> 'r) =
            let vector =
                NativeVector<byte>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        4L,
                        1L
                    )
                )
            action vector
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<C3us>, action : NativeVector<uint16> -> 'r) =
            let vector =
                NativeVector<uint16>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        3L,
                        1L
                    )
                )
            action vector
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<C4us>, action : NativeVector<uint16> -> 'r) =
            let vector =
                NativeVector<uint16>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        4L,
                        1L
                    )
                )
            action vector
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<C3ui>, action : NativeVector<uint32> -> 'r) =
            let vector =
                NativeVector<uint32>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        3L,
                        1L
                    )
                )
            action vector
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<C4ui>, action : NativeVector<uint32> -> 'r) =
            let vector =
                NativeVector<uint32>(
                    NativePtr.cast &&m,
                    VectorInfo(
                        0L,
                        4L,
                        1L
                    )
                )
            action vector
 
        // =======================================================================================
        // MATRICES
        // =======================================================================================
        
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M22d>, action : NativeMatrix<float> -> 'r) =
            let matrix =
                NativeMatrix<float>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(2, 2),
                        V2l(1, 2)
                    )
                )
            action matrix
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M23d>, action : NativeMatrix<float> -> 'r) =
            let matrix =
                NativeMatrix<float>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(3, 2),
                        V2l(1, 3)
                    )
                )
            action matrix
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M33d>, action : NativeMatrix<float> -> 'r) =
            let matrix =
                NativeMatrix<float>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(3, 3),
                        V2l(1, 3)
                    )
                )
            action matrix
        
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M34d>, action : NativeMatrix<float> -> 'r) =
            let matrix =
                NativeMatrix<float>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(4, 3),
                        V2l(1, 4)
                    )
                )
            action matrix
        
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M44d>, action : NativeMatrix<float> -> 'r) =
            let matrix =
                NativeMatrix<float>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(4, 4),
                        V2l(1, 4)
                    )
                )
            action matrix

            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M22f>, action : NativeMatrix<float32> -> 'r) =
            let matrix =
                NativeMatrix<float32>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(2, 2),
                        V2l(1, 2)
                    )
                )
            action matrix
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M23f>, action : NativeMatrix<float32> -> 'r) =
            let matrix =
                NativeMatrix<float32>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(3, 2),
                        V2l(1, 3)
                    )
                )
            action matrix
 
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M33f>, action : NativeMatrix<float32> -> 'r) =
            let matrix =
                NativeMatrix<float32>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(3, 3),
                        V2l(1, 3)
                    )
                )
            action matrix
        
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M34f>, action : NativeMatrix<float32> -> 'r) =
            let matrix =
                NativeMatrix<float32>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(4, 3),
                        V2l(1, 4)
                    )
                )
            action matrix
        
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Using(m : byref<M44f>, action : NativeMatrix<float32> -> 'r) =
            let matrix =
                NativeMatrix<float32>(
                    NativePtr.cast &&m,
                    MatrixInfo(
                        0L,
                        V2l(4, 4),
                        V2l(1, 4)
                    )
                )
            action matrix
    
    module NativeTensor =
        [<CompilerMessage("use NativeTensor.using instead", 1337, IsHidden = true)>]
        let inline us< ^p, ^m, ^n, ^r, ^x when (^m or ^p) : (static member Using : byref< ^m > * (^n -> ^r) -> ^x) > (p : ^p) (m :  byref< ^m >) (action : ^n -> ^r) : ^x =
            ((^m or ^p) : (static member Using : byref< ^m >  * (^n -> ^r) -> ^x) (&m, action))

        let inline using (m : byref<_>) action = us Unchecked.defaultof<Pinning> &m action
        
        type PinBuilder() =
            member inline x.Bind(m : byref<_>, f) = using &m f
            member inline x.Delay(f : unit -> 'a) = f
            member inline x.Return v = v
            member inline x.Run(f : unit -> 'a) = f()
            member inline x.Combine(l : unit, r : unit -> 'a) = r()
            member inline x.For(s,a) = for e in s do a s
            member inline x.TryFinally(l : unit -> 'r, r : unit -> unit) = try l() finally r()
            member inline x.TryWith(l : unit -> 'r, r : exn -> 'r) = try l() with e -> r e
            member inline x.Using<'a, 'r when 'a :> IDisposable> (a : 'a, f : 'a -> 'r) : 'r = try f a finally a.Dispose()
            member inline x.While(g,b) = while g() do b()
            member inline x.Zero() = ()

    let tensor = NativeTensor.PinBuilder()

    let private test() =
        let mutable a = M22d()
        let b = M33d()
            
        let mutable v2i = V2i()
        let mutable c3b = C3b.Green
            
        let mutable v = Vector<float>()
        let mutable m = Matrix<float>()
        let mutable a2d : float[,] = Array2D.zeroCreate 10 10
        let mutable arr = [|1;2;3|]
        tensor {
            try
                try
                    use x = { new IDisposable with member x.Dispose() = () }
                    let! p0 = &a
                    let! p1 = &arr

                    let! p2 = &v2i
                    let! p3 = &c3b

                    let! p4 = &m
                    let! p5 = &v
                    for i in 1 .. 100 do
                        let! p6 = &a2d
                        ()

                    while p4.[0,0] > 1.0 do
                        let! p7 = &arr
                        p4.[0,0] <- 1.0
                        ()
                    ()
                with e ->
                    ()
            finally
                printfn "asdasd"
        }

        ()

    type NativeMatrix<'a when 'a : unmanaged> with
        member x.Transposed = NativeMatrix<'a>(x.Pointer, x.Info.Transposed)

[<AbstractClass; Sealed; Extension>]
type NativeTensorExtensions private() =

    [<Extension>]
    static member MirrorX(this : NativeVector<'a>) =
        NativeVector<'a>(
            this.Pointer,
            VectorInfo(
                (this.SX - 1L) * this.DX,
                this.SX,
                -this.DX
            )
        )

    [<Extension>]
    static member MirrorX(this : NativeMatrix<'a>) =
        NativeMatrix<'a>(
            this.Pointer,
            MatrixInfo(
                this.Info.Index(this.SX - 1L, 0L),
                this.Size,
                V2l(-this.DX, this.DY)
            )
        )
        
    [<Extension>]
    static member MirrorX(this : NativeVolume<'a>) =
        NativeVolume<'a>(
            this.Pointer,
            VolumeInfo(
                this.Info.Index(this.SX - 1L, 0L, 0L),
                this.Size,
                V3l(-this.DX, this.DY, this.DZ)
            )
        )

    [<Extension>]
    static member MirrorX(this : NativeTensor4<'a>) =
        NativeTensor4<'a>(
            this.Pointer,
            Tensor4Info(
                this.Info.Index(this.SX - 1L, 0L, 0L, 0L),
                this.Size,
                V4l(-this.DX, this.DY, this.DZ, this.DW)
            )
        )
 
    [<Extension>]
    static member MirrorY(this : NativeMatrix<'a>) =
        NativeMatrix<'a>(
            this.Pointer,
            MatrixInfo(
                this.Info.Index(0L, this.SY - 1L),
                this.Size,
                V2l(this.DX, -this.DY)
            )
        )
        
    [<Extension>]
    static member MirrorY(this : NativeVolume<'a>) =
        NativeVolume<'a>(
            this.Pointer,
            VolumeInfo(
                this.Info.Index(0L, this.SY - 1L, 0L),
                this.Size,
                V3l(this.DX, -this.DY, this.DZ)
            )
        )
        
    [<Extension>]
    static member MirrorY(this : NativeTensor4<'a>) =
        NativeTensor4<'a>(
            this.Pointer,
            Tensor4Info(
                this.Info.Index(0L, this.SY - 1L, 0L, 0L),
                this.Size,
                V4l(this.DX, -this.DY, this.DZ, this.DW)
            )
        )        

    [<Extension>]
    static member MirrorZ(this : NativeVolume<'a>) =
        NativeVolume<'a>(
            this.Pointer,
            VolumeInfo(
                this.Info.Index(0L, 0L, this.SZ - 1L),
                this.Size,
                V3l(this.DX, this.DY, -this.DZ)
            )
        )
        
    [<Extension>]
    static member MirrorZ(this : NativeTensor4<'a>) =
        NativeTensor4<'a>(
            this.Pointer,
            Tensor4Info(
                this.Info.Index(0L, 0L, this.SZ - 1L, 0L),
                this.Size,
                V4l(this.DX, this.DY, -this.DZ, this.DW)
            )
        )

    [<Extension>]
    static member MirrorW(this : NativeTensor4<'a>) =
        NativeTensor4<'a>(
            this.Pointer,
            Tensor4Info(
                this.Info.Index(0L, 0L, 0L, this.SW - 1L),
                this.Size,
                V4l(this.DX, this.DY, this.DZ, -this.DW)
            )
        )



    [<Extension>]
    static member ToXYWTensor4(this : NativeVolume<'a>) =
        let info = this.Info
        let li = 1L + Vec.dot info.Delta (info.S - V3l.III)
        
        NativeTensor4<'a>(
            this.Pointer,
            Tensor4Info(
                info.Origin,
                V4l(info.SX, info.SY, 1L, info.SZ),
                V4l(info.DX, info.DY, li, info.DZ)
            )
        )