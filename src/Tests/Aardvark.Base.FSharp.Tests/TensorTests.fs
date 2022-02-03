namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open FsUnit
open NUnit.Framework
open Microsoft.FSharp.NativeInterop

#nowarn "9"

module TensorTests =

    module private PixImage =

        let rng = RandomSystem(1)

        let random (size : V2i) =
            let pi = PixImage<uint8>(Col.Format.RGBA, size)
            for c in pi.ChannelArray do
                c.SetByIndex(ignore >> rng.UniformUInt >> uint8) |> ignore
            pi

    [<Test>]
    [<TestCase(2)>]
    [<TestCase(4)>]
    [<TestCase(16)>]
    [<TestCase(123)>]
    let ``[TensorTests] PixImage copy mirrored as NativeTensor4``(size : int) =

        let size = V2i size
        let src = PixImage.random <| V2i size
        let dst = PixImage<uint8>(Col.Format.RGBA, size)

        NativeVolume.using src.Volume (fun srcVolume ->
            let srcTensor = srcVolume.MirrorY().ToXYWTensor4()

            NativeVolume.using dst.Volume (fun dstVolume ->
                let dstTensor = dstVolume.ToXYWTensor4()
                NativeTensor4.copy srcTensor dstTensor
            )
        )

        let srcMatrix = src.GetMatrix<C4b>()
        let dstMatrix = dst.GetMatrix<C4b>()

        for x = 0 to size.X - 1 do
            for y = 0 to size.Y - 1 do
                dstMatrix.[x, y] |> should equal srcMatrix.[x, size.Y - 1 - y]

    [<Test>]
    let ``[TensorTests] iterPtr with non-byte elements``() =
        let data = [| 42; 32; 108; -34 |]
        let info = Tensor4Info(V4l(2, 2, 1, 1), V4l(1, 2, 4, 4))

        pinned data (fun ptr ->
            let nv = NativeTensor4<int>(NativePtr.ofNativeInt ptr, info)

            let mutable index = 0
            nv |> NativeTensor4.iterPtr (fun coord ptr ->
                let value = NativePtr.read ptr
                value |> should equal data.[index]
                index <- index + 1
            )
        )

    [<Test>]
    let ``[TensorTests] iterPtr2 with different element types``() =
        let srcArray = [| 42; 32; 108; -34 |]
        let dstArray = [| 0L; 0L; 0L; 0L |]
        let info = Tensor4Info(V4l(2, 2, 1, 1), V4l(2, 1, 4, 4))

        pinned srcArray (fun srcPtr ->
            pinned dstArray (fun dstPtr ->
                let src = NativeTensor4<int>(NativePtr.ofNativeInt srcPtr, info)
                let dst = NativeTensor4<int64>(NativePtr.ofNativeInt dstPtr, info)

                (src, dst) ||> NativeTensor4.iterPtr2 (fun coord src dst ->
                    let value = NativePtr.read src
                    value |> int64 |> NativePtr.write dst

                    value |> should equal srcArray.[int <| coord.X * info.DX + coord.Y * info.DY]
                )
            )
        )

        (srcArray, dstArray) ||> Array.iter2 (fun a b ->
            (int64 a) |> should equal b
        )