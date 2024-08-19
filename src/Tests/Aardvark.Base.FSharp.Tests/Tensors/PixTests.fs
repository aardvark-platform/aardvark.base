namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open System
open NUnit.Framework
open FsUnit
open FsCheck
open FsCheck.NUnit
open Expecto

module PixTests =

    type PixType =
        | UInt8   = 0
        | UInt16  = 1
        | UInt32  = 2
        | Float16 = 3
        | Float32 = 4
        | Float64 = 5

    module private PixVolume =

        [<AutoOpen>]
        module private Generators =

            let private rng = RandomSystem(1)

            let private random<'T> (getValue : unit -> 'T) (format : Col.Format) (size : V3i) : PixVolume =
                let pi = PixVolume<'T>(format, size)
                for c in pi.ChannelArray do
                    c.SetByIndex(ignore >> getValue) |> ignore
                pi

            let random8ui  = random<uint8> (rng.UniformInt >> uint8)
            let random16ui = random<uint16> (rng.UniformInt >> uint16)
            let random32ui = random<uint32> (rng.UniformInt >> uint32)
            let random16f  = random<float16> (rng.UniformFloat >> float16)
            let random32f  = random<float32> (rng.UniformFloat)
            let random64f  = random<float> (rng.UniformDouble)

        let random (typ : PixType) : Col.Format -> V3i -> PixVolume =
            match typ with
            | PixType.UInt8   -> random8ui
            | PixType.UInt16  -> random16ui
            | PixType.UInt32  -> random32ui
            | PixType.Float16 -> random16f
            | PixType.Float32 -> random32f
            | PixType.Float64 -> random64f
            | _ -> failwithf "Unknown pix type %A" typ

        let toTypeAndFormat (typ : PixType) (format : Col.Format) (pv : PixVolume) : PixVolume =
            match typ with
            | PixType.UInt8   -> new PixVolume<uint8>(format, pv)
            | PixType.UInt16  -> new PixVolume<uint16>(format, pv)
            | PixType.UInt32  -> new PixVolume<uint32>(format, pv)
            | PixType.Float16 -> new PixVolume<float16>(format, pv)
            | PixType.Float32 -> new PixVolume<float32>(format, pv)
            | PixType.Float64 -> new PixVolume<float>(format, pv)
            | _ -> failwithf "Unknown pix type %A" typ

    module private PixImage =

        [<AutoOpen>]
        module private Generators =

            let private rng = RandomSystem(1)

            let private random<'T> (getValue : unit -> 'T) (format : Col.Format) (size : V2i) : PixImage =
                let pi = PixImage<'T>(format, size)
                for c in pi.ChannelArray do
                    c.SetByIndex(ignore >> getValue) |> ignore
                pi

            let random8ui  = random<uint8> (rng.UniformInt >> uint8)
            let random16ui = random<uint16> (rng.UniformInt >> uint16)
            let random32ui = random<uint32> (rng.UniformInt >> uint32)
            let random16f  = random<float16> (rng.UniformFloat >> float16)
            let random32f  = random<float32> (rng.UniformFloat)
            let random64f  = random<float> (rng.UniformDouble)

        let random (typ : PixType) : Col.Format -> V2i -> PixImage =
            match typ with
            | PixType.UInt8   -> random8ui
            | PixType.UInt16  -> random16ui
            | PixType.UInt32  -> random32ui
            | PixType.Float16 -> random16f
            | PixType.Float32 -> random32f
            | PixType.Float64 -> random64f
            | _ -> failwithf "Unknown pix type %A" typ

        let toTypeAndFormat (typ : PixType) (format : Col.Format) (pi : PixImage) : PixImage =
            match typ with
            | PixType.UInt8   -> new PixImage<uint8>(format, pi)
            | PixType.UInt16  -> new PixImage<uint16>(format, pi)
            | PixType.UInt32  -> new PixImage<uint32>(format, pi)
            | PixType.Float16 -> new PixImage<float16>(format, pi)
            | PixType.Float32 -> new PixImage<float32>(format, pi)
            | PixType.Float64 -> new PixImage<float>(format, pi)
            | _ -> failwithf "Unknown pix type %A" typ


    module private Gen =

        let format =
            [ Col.Format.RGBA
              Col.Format.BGRA
              Col.Format.RGB
              Col.Format.BGR ]
            |> Gen.elements

        let pixType =
            [ PixType.UInt8
              PixType.UInt16
              PixType.UInt32
              PixType.Float16
              PixType.Float32
              PixType.Float64 ]
            |> Gen.elements

        let size =
            gen {
                let! w = Gen.choose (32, 143)
                let! h = Gen.choose (32, 256)
                let! d = Gen.choose (8, 16)
                return V3i(w, h, d)
            }

        let scaleFactor =
            gen {
                let! x = Gen.choose (0, 3000)
                let! y = Gen.choose (0, 3000)
                return V2d(x, y) / float 3000
            }

        let filter =
            [ ImageInterpolation.Near
              ImageInterpolation.Linear
              ImageInterpolation.Cubic
              ImageInterpolation.Lanczos ]
            |> Gen.elements

    type ConversionTestCase =
        { Size : V3i
          Type : PixType
          SourceFormat : Col.Format
          TargetFormat : Col.Format }

    type ScalingTestCase =
        { Size : V2i
          Type : PixType
          Format : Col.Format
          Scale : V2d
          Filter : ImageInterpolation }

    type Generator private () =

        static member ConversionTestCase =
            gen {
                let! size = Gen.size
                let! typ = Gen.pixType
                let! sourceFormat = Gen.format

                let! swap = Gen.elements [true; false]

                let targetFormat =
                    if swap then
                        match sourceFormat with
                        | Col.Format.RGBA -> Col.Format.BGRA
                        | Col.Format.BGRA -> Col.Format.RGBA
                        | Col.Format.RGB  -> Col.Format.BGR
                        | Col.Format.BGR  -> Col.Format.RGB
                        | _ -> sourceFormat
                    else
                        sourceFormat

                return {
                    Size = size
                    Type = typ
                    SourceFormat = sourceFormat
                    TargetFormat = targetFormat
                }
            }
            |> Arb.fromGen

        static member ScalingTestCase =
            gen {
                let! size = Gen.size |> Gen.map v2i
                let! typ = Gen.pixType
                let! format = Gen.format
                let! scale = Gen.scaleFactor
                let! filter = Gen.filter

                return {
                    Size = size
                    Type = typ
                    Format = format
                    Scale = scale
                    Filter = filter
                }
            }
            |> Arb.fromGen

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[PixImage] Float16 Format and Type Conversion`` (input : ConversionTestCase) =
        printfn "size = %A, type = %A, source format = %A, target format = %A"
            input.Size input.Type input.SourceFormat input.TargetFormat

        let src = PixImage.random PixType.Float16 input.SourceFormat input.Size.XY
        let tmp = src |> PixImage.toTypeAndFormat input.Type input.TargetFormat
        let dst = tmp |> PixImage.toTypeAndFormat PixType.Float16 input.SourceFormat

        let src = src.AsPixImage<float16>()
        let dst = dst.AsPixImage<float16>()

        let eps = 0.005f

        for c = 0 to src.ChannelCount - 1 do
            let src = src.GetChannel(int64 c)
            let dst = dst.GetChannel(int64 c)

            for x = 0 to input.Size.X - 1 do
                for y = 0 to input.Size.Y - 1 do
                    let value = float32 dst.[x, y]
                    let expected = float32 src.[x, y]

                    if not <| Fun.ApproximateEquals(value, expected, eps) then
                        value |> should be (equalWithin eps expected)


    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[PixVolume] Float16 Format and Type Conversion`` (input : ConversionTestCase) =
        printfn "size = %A, type = %A, source format = %A, target format = %A"
            input.Size input.Type input.SourceFormat input.TargetFormat

        let src = PixVolume.random PixType.Float16 input.SourceFormat input.Size
        let tmp = src |> PixVolume.toTypeAndFormat input.Type input.TargetFormat
        let dst = tmp |> PixVolume.toTypeAndFormat PixType.Float16 input.SourceFormat

        let src = src.AsPixVolume<float16>()
        let dst = dst.AsPixVolume<float16>()

        let eps = 0.005

        for c = 0 to src.ChannelCount - 1 do
            let src = src.GetChannel(int64 c)
            let dst = dst.GetChannel(int64 c)

            for x = 0 to input.Size.X - 1 do
                for y = 0 to input.Size.Y - 1 do
                    for z = 0 to input.Size.Z - 1 do
                        let value = float dst.[x, y, z]
                        let expected = float src.[x, y, z]

                        if not <| Fun.ApproximateEquals(value, expected, eps) then
                            value |> should be (equalWithin eps expected)


    let computePSNR =
        let table : Type -> (PixImage -> PixImage -> float) =
            LookupTable.lookup [
                typeof<uint8>,   fun src dst -> PixImage.peakSignalToNoiseRatio (src.AsPixImage<uint8>()) (dst.AsPixImage<uint8>())
                typeof<uint16>,  fun src dst -> PixImage.peakSignalToNoiseRatio (src.AsPixImage<uint16>()) (dst.AsPixImage<uint16>())
                typeof<uint32>,  fun src dst -> PixImage.peakSignalToNoiseRatio (src.AsPixImage<uint32>()) (dst.AsPixImage<uint32>())
                typeof<float16>, fun src dst -> PixImage.peakSignalToNoiseRatio (src.AsPixImage<float16>()) (dst.AsPixImage<float16>())
                typeof<float32>, fun src dst -> PixImage.peakSignalToNoiseRatio (src.AsPixImage<float32>()) (dst.AsPixImage<float32>())
                typeof<float>,   fun src dst -> PixImage.peakSignalToNoiseRatio (src.AsPixImage<float>()) (dst.AsPixImage<float>())
            ]

        fun (src : PixImage) (dst : PixImage) ->
            table src.PixFormat.Type src dst

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[PixImage] Scaling`` (input : ScalingTestCase) =
        let src = PixImage.random input.Type input.Format input.Size
        let upScaled = src.ScaledPixImage(1.0 + input.Scale, input.Filter)
        let dst = upScaled.ResizedPixImage(input.Size, input.Filter)

        let psnr = computePSNR src dst
        psnr |> should be (greaterThan 10.0)