namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open System.IO

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
            PixImage.pin2 input output (fun x y ->
                (x, y) ||> NativeVolume.iter2 (fun _ x y ->
                    if x <> y then failwith "Wrong"
                )
            )

        let save (path : string) (pi : PixImage<'T>) =
            use stream = File.OpenWrite(path)
            PixImageSharp.SaveImageSharp(pi, stream, PixFileFormat.Png, 100)

        let load<'T> (path : string) =
            PixImageSharp.Create(path).AsPixImage<'T>()

    module private EmbeddedResource =
        open System.Reflection
        open System.Text.RegularExpressions

        let get (path : string) =
            let asm = Assembly.GetExecutingAssembly()
            let name = Regex.Replace(asm.ManifestModule.Name, @"\.(exe|dll)$", "", RegexOptions.IgnoreCase)
            let path = Regex.Replace(path, @"(\\|\/)", ".")
            asm.GetManifestResourceStream(name + "." + path)

        let loadPixImage<'T> (path : string) =
            use stream = get path
            PixImageSharp.Create(stream).AsPixImage<'T>()

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

    [<Test>]
    let ``[ImageSharpTests] load from stream``() =
        let pi = EmbeddedResource.loadPixImage<byte> "data/test.jpg"

        let path = Path.GetTempFileName()
        try
            pi |> PixImage.save path

            let loaded =
                PixImage.load<uint8> path

            PixImage.equal pi loaded

        finally
            if File.Exists path then File.Delete path