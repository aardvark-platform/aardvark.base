namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open System.IO

open FsUnit
open NUnit.Framework

module ImageSharpTests =

    let private rnd = RandomSystem(1)

    module private PixImage =

        let random (size : V2i) =
            let pi = PixImage<uint8>(Col.Format.RGBA, size)
            for c in pi.ChannelArray do
                c.SetByIndex(ignore >> rnd.UniformUInt >> uint8) |> ignore
            pi

        let equal (input : PixImage<'T>) (output : PixImage<'T>) =
            for x in 0 .. output.Size.X - 1 do
                for y in 0 .. output.Size.Y - 1 do
                    for c in 0 .. output.ChannelCount - 1 do
                        let inputData = input.GetChannel(int64 c)
                        let outputData = output.GetChannel(int64 c)

                        let coord = V2i(x, y)

                        let ref =
                            if Vec.allGreaterOrEqual coord V2i.Zero && Vec.allSmaller coord input.Size then
                                inputData.[coord]
                            else
                                Unchecked.defaultof<'T>

                        outputData.[x, y] |> should equal ref

        let save (path : string) (pi : PixImage<'T>) =
            use stream = File.OpenWrite(path)
            PixImageSharp.SaveImageSharp(pi, stream, PixFileFormat.Png, 100)

        let load<'T> (path : string) =
            PixImageSharp.Create(path).AsPixImage<'T>()

    [<Test>]
    let ``[ImageSharpTests] save and load``() =

        for _i = 0 to 10 do
            let size = rnd.UniformV2i(1024) + 1
            let pi = PixImage.random size

            let path = Path.GetTempFileName()
            try
                pi |> PixImage.save path

                let loaded =
                    PixImage.load<uint8> path

                PixImage.equal pi loaded

            finally
                if File.Exists path then File.Delete path