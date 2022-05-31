namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open System
open System.IO

open NUnit.Framework
open FsUnit
open FsCheck
open FsCheck.NUnit
open Expecto

open FSharp.Data.Adaptive

module PixLoaderTests =

    let private rnd = RandomSystem(1)

    let private tempFile (f : string -> 'T) =
        let filename = Path.GetTempFileName()

        try
            f filename
        finally
            if File.Exists filename then
                File.Delete filename

    module private PixImage =

        let private desktopPath =
            Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop)

        let saveToDesktop (fileName : string) (img : #PixImage) =
            let dir = Path.combine [desktopPath; "UnitTests"]
            Directory.CreateDirectory(dir) |> ignore
            img.Save(Path.combine [dir; fileName])

        let checkerboard (format : Col.Format) (width : int) (height : int) =
            let mutable colors = HashMap.empty<V2l, C4b>

            let pi = PixImage<byte>(format, V2i(width, height))

            for channel = 0 to pi.ChannelCount - 1 do
                pi.GetChannel(int64 channel).SetByCoord(fun (c : V2l) ->
                    let c = c / 11L
                    if (c.X + c.Y) % 2L = 0L then
                        255uy
                    else
                        match colors |> HashMap.tryFind c with
                        | Some c -> c.[channel]
                        | _ ->
                            let color = rnd.UniformC4d().ToC4b()
                            colors <- colors |> HashMap.add c color
                            color.[channel]
                ) |> ignore

            pi

        let compare (input : PixImage<'T>) (output : PixImage<'T>) =
            let channels = min input.ChannelCount output.ChannelCount

            for x in 0 .. output.Size.X - 1 do
                for y in 0 .. output.Size.Y - 1 do
                    for c in 0 .. channels - 1 do
                        let inputData = input.GetChannel(int64 c)
                        let outputData = output.GetChannel(int64 c)

                        let coord = V2i(x, y)

                        let ref =
                            if Vec.allGreaterOrEqual coord V2i.Zero && Vec.allSmaller coord input.Size then
                                inputData.[coord]
                            else
                                Unchecked.defaultof<'T>

                        let message =
                            let t = if c < 4 then "color" else "alpha"
                            $"PixImage {t} data mismatch at [{x}, {y}]"

                        Expect.equal outputData.[x, y] ref message

    module private Gen =

        let colorFormat =
            [
                Col.Format.RGBA
                Col.Format.BGRA
                Col.Format.RGB
                Col.Format.BGR
            ]
            |> Gen.elements

        let imageFileFormat =
            [
                PixFileFormat.Png
                PixFileFormat.Tiff
                PixFileFormat.Bmp
                PixFileFormat.Jpeg
                PixFileFormat.Gif
            ]
            |> Gen.elements

        let checkerboardPix (format : Col.Format) =
            gen {
                let! w = Gen.choose (64, 513)
                let! h = Gen.choose (64, 513)
                return PixImage.checkerboard format w h
            }

        let pixLoader =
            let loaders = PixImage.GetLoaders() |> Seq.filter (fun l -> l.Name <> "Aardvark PGM")
            Gen.elements loaders

        let pixEncoder (useStream : bool) (format : PixFileFormat) =
            pixLoader
            |> Gen.filter (fun loader ->
                not (loader.Name = "DevIL" && format = PixFileFormat.Gif) &&            // DevIL does not support saving GIFs
                not (loader.Name = "DevIL" && format = PixFileFormat.Tiff && useStream) // DevIL does not support saving TIFFs to streams
            )

        let colorAndImageFileFormat =
            gen {
                let! cf = colorFormat
                let! iff = imageFileFormat

                let isValid =
                    iff = PixFileFormat.Png || cf = Col.Format.RGB || cf = Col.Format.BGR

                return cf, iff, isValid
            }
            |> Gen.filter (fun (_, _, valid) -> valid)
            |> Gen.map (fun (cf, iff, _) -> cf, iff)


    type SaveLoadInput =
        {
            Image       : PixImage<byte>
            SaveParams  : PixSaveParams
            Encoder     : IPixLoader
            Decoder     : IPixLoader
            UseStream   : bool
        }

    type Generator private () =

        static member Loader =
            Arb.fromGen Gen.pixLoader

        static member PixImage =
            gen {
                let! format = Gen.colorFormat
                return! Gen.checkerboardPix format
            }
            |> Arb.fromGen

        static member SaveLoadInput =
            gen {
                let! cf, iff = Gen.colorAndImageFileFormat
                let! pix = Gen.checkerboardPix cf
                let! useStream = Gen.elements [false; true]
                let! encoder = Gen.pixEncoder useStream iff
                let! decoder = Gen.pixLoader

                let saveParams =
                    match iff with
                    | PixFileFormat.Jpeg -> PixJpegSaveParams(quality = 100) :> PixSaveParams
                    | fmt -> PixSaveParams fmt

                return {
                    Image = pix
                    SaveParams = saveParams
                    Encoder = encoder
                    Decoder = decoder
                    UseStream = useStream
                }
            }
            |> Arb.fromGen

    [<SetUp>]
    let setup() =
        Aardvark.Init()
        Report.Verbosity <- 3


    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[PixLoader] Save and load`` (input : SaveLoadInput) =
        printfn "encoder = %s, decoder = %s, size = %A, color format = %A, file format = %A, use stream = %b"
                input.Encoder.Name input.Decoder.Name input.Image.Size input.Image.Format input.SaveParams.Format input.UseStream

        tempFile (fun file ->
            let output =
                if input.UseStream then
                    use stream = File.Open(file, FileMode.Create, FileAccess.ReadWrite)
                    input.Image.Save(stream, input.SaveParams, input.Encoder)

                    stream.Position <- 0L
                    PixImage<byte>(stream, input.Decoder)
                else
                    input.Image.Save(file, input.SaveParams, false, input.Encoder)
                    PixImage<byte>(file, input.Decoder)

            match input.SaveParams.Format with
            | PixFileFormat.Jpeg | PixFileFormat.Gif ->
                let psnr = PixImage.peakSignalToNoiseRatio input.Image output
                Expect.isGreaterThan psnr 20.0 "Bad peak-signal-to-noise ratio"

            | _ ->
                PixImage.compare input.Image output
        )


    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[PixLoader] Aardvark PGM writer`` (pi : PixImage<byte>) =
        printfn "size = %A" pi.Size

        let pi = PixImage<byte>(Col.Format.Gray, pi.GetChannel 0L)
        let loader = PixImage.GetLoaders() |> Seq.pick (fun l -> if l.Name = "Aardvark PGM" then Some l else None)

        tempFile (fun file ->
            pi.Save(file, PixFileFormat.Pgm, false, loader)
            let out = PixImage<byte>(file)

            PixImage.compare pi out
        )


    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[PixLoader] JPEG quality`` (loader : IPixLoader) (pi : PixImage<byte>) =
        printfn "loader = %s, size = %A, format = %A" loader.Name pi.Size pi.Format

        tempFile (fun file50 ->
            tempFile (fun file90 ->
                pi.SaveAsJpeg(file50, 50, false, loader)
                pi.SaveAsJpeg(file90, 90, false, loader)

                // check size
                let i50 = FileInfo(file50)
                let i90 = FileInfo(file90)

                i50.Length |> should be (lessThan i90.Length)
            )
        )


    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[PixLoader] PNG compression level`` (pi : PixImage<byte>) =
        printfn "size = %A, format = %A" pi.Size pi.Format

        tempFile (fun file0 ->
            tempFile (fun file9 ->
                pi.SaveAsPng(file0, 0, false, PixImageSharp.Loader)
                pi.SaveAsPng(file9, 6, false, PixImageSharp.Loader)

                // check equal
                let pi0 = PixImage<uint8>(file0, PixImageSharp.Loader)
                let pi9 = PixImage<uint8>(file9, PixImageSharp.Loader)

                PixImage.compare pi0 pi9

                // check size
                let i0 = FileInfo(file0)
                let i9 = FileInfo(file9)

                i0.Length |> should be (greaterThan i9.Length)
            )
        )


    [<Test>]
    let ``[PixLoader] Add and remove loaders``() =
        let count = PixImage.GetLoaders() |> Seq.length

        PixImage.SetLoader(PixImageDevil.Loader, 1337)
        PixImage.GetLoaders() |> Seq.head |> should equal PixImageDevil.Loader

        let priorities = PixImage.GetLoadersWithPriority()
        priorities.Count |> should equal count
        priorities.Get(PixImageDevil.Loader) |> should equal 1337

        PixImage.RemoveLoader(PixImageSharp.Loader)
        PixImage.RemoveLoader(PixImageDevil.Loader)
        PixImage.GetLoaders() |> Seq.length |> should equal (count - 2)