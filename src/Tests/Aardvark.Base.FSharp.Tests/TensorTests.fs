namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open FsUnit
open NUnit.Framework

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