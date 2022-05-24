namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open System.IO

open NUnit.Framework
open FsUnit
open FsCheck
open FsCheck.NUnit

open FSharp.Data.Adaptive

module PixLoaderTests =

    let private rnd = RandomSystem(1)

    module private PixImage =

        let random (format : Col.Format) (width : int) (height : int) =
            let pi = PixImage<uint8>(format, V2i(width, height))
            for c in pi.ChannelArray do
                c.SetByIndex(ignore >> rnd.UniformUInt >> uint8) |> ignore
            pi

        let checkerboard (format : Col.Format) (width : int) (height : int) =
            let mutable colors = HashMap.empty

            let pi = PixImage<byte>(format, V2i(width, height))
            pi.GetMatrix<C4b>().SetByCoord(fun (c : V2l) ->
                let c = c / 11L
                if (c.X + c.Y) % 2L = 0L then

                    C4b.White
                else
                    match colors |> HashMap.tryFind c with
                    | Some c -> c
                    | _ ->
                        let color = C4b(rnd.UniformInt(256), rnd.UniformInt(256), rnd.UniformInt(256), 255)
                        colors <- colors |> HashMap.add c color
                        color
            ) |> ignore
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

    module Generators =

        //[<RequireQualifiedAccess>]
        //type Loader =
        //    | ImageSharp
        //    | DevIL
        //    | FreeImage
        //    | SystemImage

        //let formats = [
        //        PixFileFormat.Jpeg
        //        PixFileFormat.Png
        //        PixFileFormat.Tiff
        //        PixFileFormat.Bmp
        //        PixFileFormat.Gif
        //    ]

        //let colorFormats =
        //    [
        //        Col.Format.RGB
        //        Col.Format.BGR
        //        Col.Format.RGBA
        //        Col.Format.BGRA
        //    ]

        type Loader private () =

            static member Loader =
                let loaders = PixImage.GetLoaders() |> Seq.filter (fun l -> l.Name <> "Aardvark PGM")
                Arb.fromGen (Gen.elements loaders)

        type CheckerboardPix private () =

            static member PixImage =
                gen {
                    let! w = Gen.choose (64, 513)
                    let! h = Gen.choose (64, 513)
                    let! fmt = Gen.elements [Col.Format.RGBA; Col.Format.BGRA]
                    return PixImage.checkerboard fmt w h
                } |> Arb.fromGen

    let private tempFile (f : string -> 'T) =
        let filename = Path.GetTempFileName()

        try
            f filename
        finally
            if File.Exists filename then
                File.Delete filename

    [<SetUp>]
    let setup() =
        CachingProperties.CustomCacheDirectory <- "./.cache"
        Aardvark.Init()
        Report.Verbosity <- 3

    [<Property(Arbitrary = [| typeof<Generators.Loader>; typeof<Generators.CheckerboardPix> |])>]
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

    [<Property(Arbitrary = [| typeof<Generators.CheckerboardPix> |])>]
    let ``[PixLoader] PNG compression level`` (pi : PixImage<byte>) =
        printfn "size = %A, format = %A" pi.Size pi.Format

        tempFile (fun file0 ->
            tempFile (fun file9 ->
                pi.SaveAsPng(file0, 0, false, PixImageSharp.Loader)
                pi.SaveAsPng(file9, 6, false, PixImageSharp.Loader)

                // check equal
                let pi0 = PixImage<uint8>(file0, PixImageSharp.Loader)
                let pi9 = PixImage<uint8>(file9, PixImageSharp.Loader)

                PixImage.equal pi0 pi9

                // check size
                let i0 = FileInfo(file0)
                let i9 = FileInfo(file9)

                i0.Length |> should be (greaterThan i9.Length)
            )
        )

    [<Test>]
    let ``[PixLoader] Add and remove loaders``() =
        let asm = System.Threading.Thread.GetDomain().GetAssemblies()


        let count = PixImage.GetLoaders() |> Seq.length

        PixImage.AddLoader(PixImageDevil.Loader, 1337)
        PixImage.GetLoaders() |> Seq.head |> should equal PixImageDevil.Loader

        let ids = System.Runtime.Serialization.ObjectIDGenerator()

        let priorities = PixImage.GetLoadersWithPriority()

        if priorities.Count <> count then
            priorities |> Seq.iter (fun entry ->
                printfn "Name: %s, Priority: %d, ID: %A" entry.Key.Name entry.Value (ids.GetId(entry.Key))
            )

            let distinct =
                priorities |> Seq.map (fun entry -> entry.Key) |> Seq.distinct

            printfn "%A" distinct

        priorities.Count |> should equal count
        priorities.Get(PixImageDevil.Loader) |> should equal 1337

        PixImage.RemoveLoader(PixImageSharp.Loader)
        PixImage.RemoveLoader(PixImageDevil.Loader)
        PixImage.GetLoaders() |> Seq.length |> should equal (count - 2)