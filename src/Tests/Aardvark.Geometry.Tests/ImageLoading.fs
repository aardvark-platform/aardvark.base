namespace Aardvark.Geometry.Tests

open System.IO

open FsCheck
open FsCheck.NUnit

open Aardvark.Base


module ImageLoadingTests =
    
    type Impl = Devil | PixImageSharp
    type Config = { impl : Impl; fmt : PixFileFormat; quality : int }

    let getSave (config : Config) (pi : PixImage) (fileName : string) =
        match config.impl with
        | Devil -> 
            let r = PixImageDevil.SaveAsImageDevil(pi, fileName, config.fmt, PixSaveOptions.Default, config.quality)
            if not r then failwith "could not save image"
        | PixImageSharp -> 
            use s = File.OpenWrite(fileName)
            PixImageSharp.SaveImageSharp(pi |> unbox<PixImage<byte>>, s, config.fmt, config.quality) 

    let getLoad (config : Config) : string -> PixImage  =
        match config.impl with
        | Devil -> 
            fun s -> PixImageDevil.CreateRawDevil(s)
        | PixImageSharp -> 
            fun s -> PixImageSharp.Create(s)

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
                let! fmt = Gen.elements fileFormats
                let! impl = Arb.toGen Arb.from<Impl>
                let! quality = Gen.choose (100, 100)
                return { impl = impl; fmt = fmt; quality = quality }
            } |> Arb.fromGen

    do Aardvark.Init()

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