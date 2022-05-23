namespace Aardvark.Geometry.Tests

open System.IO

open FsCheck
open FsCheck.NUnit

open Aardvark.Base


module ImageLoadingTests =
    
    type Impl = Devil | PixImageSharp
    type Config = { impl : Impl; saveParams : PixSaveParams }

    let getSave (config : Config) (pi : PixImage) (fileName : string) =
        match config.impl with
        | Devil ->
            pi.Save(fileName, config.saveParams, normalizeFilename = false, loader = PixImageDevil.Loader)
        | PixImageSharp ->
            pi.Save(fileName, config.saveParams, normalizeFilename = false, loader = PixImageSharp.Loader)

    let getLoad (config : Config) : string -> PixImage  =
        match config.impl with
        | Devil -> 
            fun s -> PixImage.Load(s, PixImageDevil.Loader)
        | PixImageSharp -> 
            fun s -> PixImage.Load(s, PixImageSharp.Loader)

    let fmts = 
        [Col.Format.RGB; Col.Format.BGR; Col.Format.RGBA; Col.Format.BGRA]

    let fileFormats = 
        [PixFileFormat.Png; PixFileFormat.Jpeg]

    let col =  Gen.four (Gen.choose (0, 255)) |> Gen.map (fun (r,g,b,a) -> C4b(r,g,b,a))

    type Generators private() =

        static member PixImage =
            gen {
                let! w = Gen.choose (1, 129)
                let! h = Gen.choose (1, 129)
                let! fmt = Gen.elements fmts

                let pi = PixImage<byte>(fmt, V2i(w,h))

                return pi

            } |> Arb.fromGen

        static member Config = 
            gen {
                let! impl = Arb.toGen Arb.from<Impl>
                let! fmt = Gen.elements fileFormats

                let saveParams =
                    match fmt with
                    | PixFileFormat.Jpeg -> PixJpegSaveParams(100) :> PixSaveParams
                    | _ -> PixSaveParams(fmt)

                return { impl = impl; saveParams = saveParams }
            } |> Arb.fromGen

    do Aardvark.Init()

    do Report.Verbosity <- 3

    [<Property(Arbitrary = [| typeof<Generators> |])>]
    let saveLoadTest (pi : PixImage<byte>) (saveConfig : Config) (loadConfig : Config) =
        printfn "size %A, %A %A" pi.Size saveConfig loadConfig
        let rnd = new RandomSystem()
        pi.GetMatrix<C4b>().SetByIndex(fun i -> rnd.UniformC4d() |> C4b) |> ignore
        let fileName = Path.GetTempFileName()
        try
            let saver = getSave saveConfig
            let loader = getLoad loadConfig
            saver pi fileName
            let loaded = loader fileName |> unbox<PixImage<byte>>
            // better comparison neede
            //let r = loaded.GetMatrix<C4b>()
            //let i = pi.GetMatrix<C4b>()
            //r.ForeachCoord(fun c ->   
            //    let d = r.[c] - i.[c]
            //    if d.R > 10uy || d.G > 10uy || d.B > 10uy then failwithf "not equal: %A vs %A on %A (%A, %s)" r.[c] i.[c] c d fileName
            //)
            true

        finally 
            if File.Exists fileName then File.Delete fileName