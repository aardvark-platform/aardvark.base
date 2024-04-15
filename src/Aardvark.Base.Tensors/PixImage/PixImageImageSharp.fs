namespace Aardvark.Base

open Aardvark.Base.Monads
open System
open System.IO
open System.Runtime.InteropServices
open Microsoft.FSharp.NativeInterop
open SixLabors.ImageSharp
open SixLabors.ImageSharp.Processing
open SixLabors.ImageSharp.Metadata.Profiles.Exif
open SixLabors.ImageSharp.Formats
open SixLabors.ImageSharp.PixelFormats
open SixLabors.ImageSharp.Processing.Processors.Transforms
open System.Runtime.CompilerServices

#nowarn "9"

[<AutoOpen>]
module private ImageSharpExtensions =

    /// Extensions for interop between PixImage and ImageSharp Images.
    [<AbstractClass; Sealed; Extension>]
    type ImageSharpImageExtensions private() =

        [<Extension>]
        static member PinRows(this : Image<'T>, action : int -> nativeptr<'T> -> unit) =
            this.ProcessPixelRows(fun rows ->
                for y = 0 to rows.Height - 1 do
                    let span = rows.GetRowSpan(y)

                    SpanPinning.Pin(span, fun address ->
                        let ptr = NativePtr.ofNativeInt address
                        ptr |> action y
                    )
            )

        /// Temporarily pins Image rows as a NativeMatrix.
        [<Extension>]
        static member PinMatrixRows(this : Image<'T>, action : int -> NativeMatrix<'T> -> unit) =
            this.PinRows(fun y ptr ->
                let src =
                    NativeMatrix<'T>(
                        NativePtr.cast ptr,
                        MatrixInfo(
                            0L,
                            V2l(this.Width, 1),
                            V2l(1L, int64 this.Width)
                        )
                    )

                action y src
            )

        /// Temporarily pins Image rows as a NativeVolume.
        [<Extension>]
        static member PinVolumeRows(this : Image<'T>, channels : int, action : int -> NativeVolume<'T> -> unit) =
            this.PinRows(fun y ptr ->
                let src =
                    NativeVolume<'T>(
                        NativePtr.cast ptr,
                        VolumeInfo(
                            0L,
                            V3l(this.Width, 1, channels),
                            V3l(int64 channels, int64 this.Width * int64 channels, 1L)
                        )
                    )

                action y src
            )

        /// Converts the color to a C4b.
        [<Extension>]
        static member inline ToC4b(v : Rgba32) = C4b(v.R, v.G, v.B, v.A)

        /// Converts the color to a C4b.
        [<Extension>]
        static member inline ToC4b(v : Argb32) = C4b(v.R, v.G, v.B, v.A)

        /// Converts the color to a C4b.
        [<Extension>]
        static member inline ToC4b(v : Bgra32) = C4b(v.R, v.G, v.B, v.A)

        /// Converts the color to a C4b.
        [<Extension>]
        static member inline ToC4b(v : Rgb24) = C4b(v.R, v.G, v.B, 255uy)

        /// Converts the color to a C4b.
        [<Extension>]
        static member inline ToC4b(v : Bgr24) = C4b(v.R, v.G, v.B, 255uy)

        /// Converts the color to a C4b.
        [<Extension>]
        static member inline ToC4b(v : Bgra4444) =
            let mutable c = Rgba32()
            v.ToRgba32(&c)
            C4b(c.R, c.G, c.B, c.A)

        /// Converts the color to a C4b.
        [<Extension>]
        static member inline ToC4b(v : Bgra5551) =
            let mutable c = Rgba32()
            v.ToRgba32(&c)
            C4b(c.R, c.G, c.B, c.A)

        /// Converts the color to a C4b.
        [<Extension>]
        static member inline ToC4b(v : PixelFormats.Byte4) =
            let mutable c = Rgba32()
            v.ToRgba32(&c)
            C4b(c.R, c.G, c.B, c.A)

        /// Converts the color to a C4b.
        [<Extension>]
        static member inline ToC4b(v : IPixel) =
            let mutable c = Rgba32()
            v.ToRgba32(&c)
            C4b(c.R, c.G, c.B, c.A)


        /// Converts the color to a C3b.
        [<Extension>]
        static member inline ToC3b(v : Rgba32) = C3b(v.R, v.G, v.B)

        /// Converts the color to a C3b.
        [<Extension>]
        static member inline ToC3b(v : Argb32) = C3b(v.R, v.G, v.B)

        /// Converts the color to a C3b.
        [<Extension>]
        static member inline ToC3b(v : Bgra32) = C3b(v.R, v.G, v.B)

        /// Converts the color to a C3b.
        [<Extension>]
        static member inline ToC3b(v : Rgb24) = C3b(v.R, v.G, v.B)

        /// Converts the color to a C3b.
        [<Extension>]
        static member inline ToC3b(v : Bgr24) = C3b(v.R, v.G, v.B)

        /// Converts the color to a C3b.
        [<Extension>]
        static member inline ToC3b(v : Rgba1010102) =
            let mutable c = Rgba32()
            v.ToRgba32(&c)
            C3b(c.R, c.G, c.B)

        /// Converts the color to a C3b.
        [<Extension>]
        static member inline ToC3b(v : Bgr565) =
            let mutable c = Rgba32()
            v.ToRgba32(&c)
            C3b(c.R, c.G, c.B)

        /// Converts the color to a C3b.
        [<Extension>]
        static member inline ToC3b(v : IPixel) =
            let mutable c = Rgba32()
            v.ToRgba32(&c)
            C3b(c.R, c.G, c.B)

        [<Extension>]
        static member internal TryGetValue(this : ExifProfile, tag : ExifTag<'v>, [<Out>] v: byref<'v>) =
            let r = this.GetValue tag
            if isNull r then false
            else
                v <- r.Value
                true

/// Internal tools for copying beetween PixImage and ImageSharp Images.
[<AutoOpen>]
module private ImageSharpHelpers =

    let (!!) (a : bool, v : 'a) = if a then Some v else None

    let piDirect<'TPixel, 'T when 'T : unmanaged and 'TPixel : (new : unit -> 'TPixel) and 'TPixel : struct and 'TPixel :> IPixel<'TPixel> and 'TPixel : unmanaged> (img : Image<'TPixel>, dst : PixImage<'T>) : unit =
        img.PinVolumeRows(dst.ChannelCount, fun y src ->
            let src = src.Address |> NativeVolume.ofNativeInt<'T> src.Info
            let dst = dst.Volume.ImageRow(int64 y)

            NativeVolume.using dst (fun dst ->
                NativeVolume.copy src dst
            )
        )

    let piChannel<'TPixel, 'T when 'T : unmanaged and 'TPixel : (new : unit -> 'TPixel) and 'TPixel : struct and 'TPixel :> IPixel<'TPixel> and 'TPixel : unmanaged> (img : Image<'TPixel>, dst : PixImage<'T>, order : int[]) : unit =
        let channels = dst.ChannelCount

        img.PinVolumeRows(channels, fun y src ->
            let src = src.Address |> NativeVolume.ofNativeInt<'T> src.Info
            let dst = dst.Volume.ImageRow(int64 y)

            NativeVolume.using dst (fun dst ->
                for dc in 0 .. channels - 1 do
                    let sc = order.[dc]
                    NativeMatrix.copy src.[*,*, sc] dst.[*,*,dc]
            )
        )

    let piMapping<'TPixel, 'T, 'C when 'T : unmanaged and 'C : unmanaged and 'TPixel : (new : unit -> 'TPixel) and 'TPixel : struct and 'TPixel :> IPixel<'TPixel> and 'TPixel : unmanaged> (img : Image<'TPixel>, dst : PixImage<'T>, convert : 'TPixel -> 'C) =
        let size = V2i(img.Width, img.Height)
        img.PinMatrixRows (fun y src ->
            NativeVolume.using dst.Volume (fun dst ->
                let dst =
                    NativeMatrix<'C>(
                        NativePtr.cast dst.Pointer,
                        MatrixInfo(
                            0L,
                            V2l(size.X, size.Y),
                            V2l(1L, int64 size.X)
                        )
                    ).SubMatrix(V2i(0, y), V2i(size.X, 1))
                NativeMatrix.copyWith convert src dst
            )
        )

    let imgDirect<'T, 'TPixel when 'T : unmanaged and 'TPixel : (new : unit -> 'TPixel) and 'TPixel : struct and 'TPixel :> IPixel<'TPixel> and 'TPixel : unmanaged> (img : PixImage<'T>) : Image<'TPixel> =
        let res = new Image<'TPixel>(img.Size.X, img.Size.Y)
        res.PinVolumeRows(img.ChannelCount, fun y dst ->
            let src = img.Volume.ImageRow(int64 y)
            let dst = dst.Address |> NativeVolume.ofNativeInt dst.Info

            NativeVolume.using src (fun src ->
                NativeVolume.copy src dst
            )
        )
        res

    let usingTransformed<'T> (fmt : Col.Format) (w : int) (h : int) (trafo : ImageTrafo) (action : PixImage<'T> -> unit) =
        let s = ImageTrafo.transformSize (V2i(w, h)) trafo
        let inv = ImageTrafo.inverse trafo
        let img = PixImage<'T>(fmt, s)
        let local = unbox<PixImage<'T>>(img.Transformed inv)
        action local
        img

/// Basic Camera information for an Image.
[<StructuredFormatDisplay("{AsString}")>]
type CameraInfo =
    {
        size        : V2i
        cropFactor  : float
        make        : string
        model       : string
        focal       : float
        aperture    : option<float>
        lensMake    : option<string>
        lensModel   : option<string>
    }

    member x.sensorSize =
        let d = 43.26661531 / (x.cropFactor * x.size.Length)
        V2d x.size * d

    member x.normalizedFocalLengths =
        x.focal / x.sensorSize

    member private x.AsString =
        x.ToString()

    override x.ToString() =
        let props =
            [
                "size", string x.size
                "crop", sprintf "%.3f" x.cropFactor

                let model =
                    if x.model.ToLower().StartsWith (x.make.ToLower()) then x.model.Substring(x.make.Length).Trim()
                    else x.model

                "name", sprintf "%s %s" x.make model

                let lensMake =
                    match x.lensMake with
                    | Some l -> l
                    | None -> x.make
                match x.lensModel with
                | Some m ->
                    let m =
                        if m.ToLower().StartsWith (lensMake.ToLower()) then m.Substring lensMake.Length
                        else m
                    "lens", sprintf "%s %s" lensMake m
                | None ->
                    if lensMake <> x.make then
                        "lens", lensMake
                //match x.lensModel with
                //| Some l -> "lensModel", sprintf "%A" l
                //| None -> ()

                "focal", sprintf "%.3g (%s)" x.focal (x.normalizedFocalLengths.ToString "0.###")
                "sensor", x.sensorSize.ToString("0.###")
                match x.aperture with
                | Some a -> "aperture", sprintf "%.3g" a
                | None -> ()
            ]
        props
        |> Seq.map (fun (n,v) -> sprintf "%s: %s" n v)
        |> String.concat ", "
        |> sprintf "CameraInfo { %s }"


type private ImageSharpPixLoader() =
    member x.Name = "ImageSharp"

    interface IPixLoader with
        member x.Name = x.Name
        member x.LoadFromFile(filename) = PixImageSharp.Create(filename)
        member x.LoadFromStream(stream) = PixImageSharp.Create(stream)
        member x.SaveToFile(filename, image, saveParams) = image.SaveImageSharp(filename, saveParams)
        member x.SaveToStream(stream, image, saveParams) = image.SaveImageSharp(stream, saveParams)
        member x.GetInfoFromFile(filename) = PixImageSharp.GetPixImageInfo(filename)
        member x.GetInfoFromStream(stream) = PixImageSharp.GetPixImageInfo(stream)

/// PixImage operations implemented with ImageSharp.
and [<AbstractClass; Sealed; Extension>] PixImageSharp private() =

    static let loader = ImageSharpPixLoader() :> IPixLoader

    static let toNullable (q : int) =
        if q < 0 then Unchecked.defaultof<Nullable<int>>
        else Nullable (min q 100)

    static let toPixImage =
        LookupTable.lookupTable [
            typeof<PixelFormats.A8>,        (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.Gray img.Width img.Height trafo (fun dst -> piDirect<A8, byte>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.L8>,         (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.Gray img.Width img.Height trafo (fun dst -> piDirect<L8, byte>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.Argb32>,        (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.RGBA img.Width img.Height trafo (fun dst -> piChannel<Argb32, byte>(unbox img, dst, [|1;2;3;0|])) :> PixImage)
            typeof<PixelFormats.Rgb24>,         (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.RGB  img.Width img.Height trafo (fun dst -> piDirect<Rgb24, byte>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.Rgba32>,        (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.RGBA img.Width img.Height trafo (fun dst -> piDirect<Rgba32, byte>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.Bgr24>,         (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.BGR  img.Width img.Height trafo (fun dst -> piDirect<Bgr24, byte>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.Bgra32>,        (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.BGRA img.Width img.Height trafo (fun dst -> piDirect<Bgra32, byte>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.L16>,        (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.Gray img.Width img.Height trafo (fun dst -> piDirect<L16, uint16>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.Rgb48>,         (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.RGB  img.Width img.Height trafo (fun dst -> piDirect<Rgb48, uint16>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.Rgba64>,        (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.RGBA img.Width img.Height trafo (fun dst -> piDirect<Rgba64, uint16>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.Bgr565>,        (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.BGR  img.Width img.Height trafo (fun dst -> piMapping<Bgr565, byte, C3b>(unbox img, dst, ImageSharpImageExtensions.ToC3b)) :> PixImage)
            typeof<PixelFormats.Rgba1010102>,   (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.BGR  img.Width img.Height trafo (fun dst -> piMapping<Rgba1010102, byte, C3b>(unbox img, dst, ImageSharpImageExtensions.ToC3b)) :> PixImage)
            typeof<PixelFormats.Bgra4444>,      (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.BGRA img.Width img.Height trafo (fun dst -> piMapping<Bgra4444, byte, C4b>(unbox img, dst, ImageSharpImageExtensions.ToC4b)) :> PixImage)
            typeof<PixelFormats.Bgra5551>,      (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.BGRA img.Width img.Height trafo (fun dst -> piMapping<Bgra5551, byte, C4b>(unbox img, dst, ImageSharpImageExtensions.ToC4b)) :> PixImage)
            typeof<PixelFormats.Byte4>,         (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.BGRA img.Width img.Height trafo (fun dst -> piMapping<Byte4, byte, C4b>(unbox img, dst, ImageSharpImageExtensions.ToC4b)) :> PixImage)
            typeof<PixelFormats.Rg32>,          (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.RG img.Width img.Height trafo (fun dst -> piDirect<Rg32, uint16>(unbox img, dst)) :> PixImage)
            typeof<PixelFormats.HalfSingle>,    (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.Gray img.Width img.Height trafo (fun dst -> piMapping<HalfSingle, float16, float16>(unbox img, dst, fun v -> Half.ToHalf v.PackedValue)) :> PixImage)
            typeof<PixelFormats.RgbaVector>,    (fun (img : Image) (trafo : ImageTrafo) -> usingTransformed Col.Format.RGBA img.Width img.Height trafo (fun dst -> piDirect<RgbaVector, float32>(unbox img, dst)) :> PixImage)
        ]

    static let toImage =
        LookupTable.lookupTable [
            PixFormat.ByteGray,             (fun (img : PixImage) -> imgDirect<byte, L8>(unbox img) :> Image)
            PixFormat.ByteBGR,              (fun (img : PixImage) -> imgDirect<byte, Bgr24>(unbox img) :> Image)
            PixFormat.ByteBGRA,             (fun (img : PixImage) -> imgDirect<byte, Bgra32>(unbox img) :> Image)
            PixFormat.ByteRGB,              (fun (img : PixImage) -> imgDirect<byte, Rgb24>(unbox img) :> Image)
            PixFormat.ByteRGBA,             (fun (img : PixImage) -> imgDirect<byte, Rgba32>(unbox img) :> Image)

            PixFormat.UShortGray,           (fun (img : PixImage) -> imgDirect<uint16, L16>(unbox img) :> Image)
            PixFormat.UShortRGB,            (fun (img : PixImage) -> imgDirect<uint16, Rgb48>(unbox img) :> Image)
            PixFormat.UShortRGBA,           (fun (img : PixImage) -> imgDirect<uint16, Rgba64>(unbox img) :> Image)

            PixFormat.FloatRGBA,            (fun (img : PixImage) -> imgDirect<float32, RgbaVector>(unbox img) :> Image)
        ]

    static let getRotateAndFlipMode (img : IImageInfo) =
        if isNull img.Metadata.ExifProfile then
            RotateMode.None, FlipMode.None
        else
            match img.Metadata.ExifProfile.TryGetValue ExifTag.Orientation with
            | (true, v) ->
                match unbox<uint16> v with
                | 1us -> RotateMode.None, FlipMode.None             // Horizontal (normal)
                | 2us -> RotateMode.None, FlipMode.Horizontal       // Mirror horizontal
                | 3us -> RotateMode.Rotate180, FlipMode.None        // Rotate 180
                | 4us -> RotateMode.None, FlipMode.Vertical         // Mirror vertical
                | 5us -> RotateMode.Rotate270, FlipMode.Horizontal  // Mirror horizontal and rotate 270 CW
                | 6us -> RotateMode.Rotate90, FlipMode.None         // Rotate 90 CW
                | 7us -> RotateMode.Rotate90, FlipMode.Horizontal   // Mirror horizontal and rotate 90 CW
                | 8us -> RotateMode.Rotate270, FlipMode.None        // Rotate 270 CW
                | _   -> RotateMode.None, FlipMode.None
            | _ ->
                RotateMode.None, FlipMode.None

    static let getImageTrafo (img : IImageInfo) =
        if isNull img.Metadata.ExifProfile then
            ImageTrafo.Identity
        else
            match img.Metadata.ExifProfile.TryGetValue ExifTag.Orientation with
            | (true, v) ->
                match unbox<uint16> v with
                | 1us -> ImageTrafo.Identity     // Horizontal (normal)
                | 2us -> ImageTrafo.MirrorX      // Mirror horizontal
                | 3us -> ImageTrafo.Rot180       // Rotate 180
                | 4us -> ImageTrafo.MirrorY      // Mirror vertical
                | 5us -> ImageTrafo.Transpose    // Mirror horizontal and rotate 270 CW
                | 6us -> ImageTrafo.Rot270       // Rotate 90 CW
                | 7us -> ImageTrafo.Transverse   // Mirror horizontal and rotate 90 CW
                | 8us -> ImageTrafo.Rot90        // Rotate 270 CW
                | _   -> ImageTrafo.Identity
            | _ ->
                ImageTrafo.Identity

    static let tryGetEncoder (saveParams : PixSaveParams) =
        match saveParams with
        | :? PixJpegSaveParams as jpeg ->
            Some (Jpeg.JpegEncoder(Quality = Nullable<int>(jpeg.Quality)) :> IImageEncoder)

        | :? PixPngSaveParams as png ->
            Some (Png.PngEncoder(CompressionLevel = unbox png.CompressionLevel) :> IImageEncoder)

        | _ ->
            match saveParams.Format with
            | PixFileFormat.Bmp    -> Some (Bmp.BmpEncoder() :> IImageEncoder)
            | PixFileFormat.Jpeg
            | PixFileFormat.J2k    -> Some (Jpeg.JpegEncoder() :> IImageEncoder)
            | PixFileFormat.Pbm
            | PixFileFormat.PbmRaw -> Some (Pbm.PbmEncoder() :> IImageEncoder)
            | PixFileFormat.Targa  -> Some (Tga.TgaEncoder() :> IImageEncoder)
            | PixFileFormat.Tiff   -> Some (Tiff.TiffEncoder() :> IImageEncoder)
            | PixFileFormat.Gif    -> Some (Gif.GifEncoder() :> IImageEncoder)
            | PixFileFormat.Png    -> Some (Png.PngEncoder() :> IImageEncoder)
            | _ -> None


    static member Loader = loader

    /// Installs ImageSharp loading/saving/etc. in PixImage.
    [<OnAardvarkInit>]
    static member Init() =
        PixImage.AddLoader(loader)

    /// Tries to read a PixImage from the given Stream.
    static member TryCreate(stream : Stream) =
        try
            use img = Image.Load stream
            let tp = img.GetType().GetGenericArguments().[0]
            let trafo = getImageTrafo img
            toPixImage tp img trafo |> Some
        with _ ->
            None

    /// Tries to read a PixImage from the given File.
    static member TryCreate(file : string) =
        if File.Exists file then
            use s = File.OpenRead file
            PixImageSharp.TryCreate s
        else
            None

    /// Reads a PixImage from the given Stream or fails if not an image.
    static member Create(stream : Stream) =
        use img = Image.Load stream
        let tp = img.GetType().GetGenericArguments().[0]
        let trafo = getImageTrafo img
        toPixImage tp img trafo

    /// Reads a Image from the given Stream or fails if not an image.
    static member CreateImage(stream : Stream) =
        let img = Image.Load stream
        img.Mutate(fun x -> x.AutoOrient() |> ignore)
        img

    /// Reads a Image from the given File or fails if not an image.
    static member CreateImage(file : string) =
        use s = File.OpenRead file
        PixImageSharp.CreateImage s

    /// Reads a PixImage from the given File or fails if not an image.
    static member Create(file : string) =
        use s = File.OpenRead file
        PixImageSharp.Create s

    /// Tries to get the EXIF-thumb for the image.
    static member TryGetThumb(stream : Stream) =
        try
            let info = Image.Identify(stream)
            let trafo = getImageTrafo info
            let thumb = info.Metadata.ExifProfile.CreateThumbnail<Rgba32>()
            toPixImage (thumb.GetType().GetGenericArguments().[0]) thumb trafo |> Some
        with _ ->
            None

    /// Tries to get the EXIF-thumb for the image.
    static member TryGetThumb(file : string) =
        if File.Exists file then
            use s = File.OpenRead file
            PixImageSharp.TryGetThumb s
        else
            None

    /// Gets basic information from the Image without loading its content.
    static member GetPixImageInfo(stream : Stream) =
        try
            let info = Image.Identify stream
            let trafo = getImageTrafo info
            let size = trafo |> ImageTrafo.transformSize (V2i(info.Width, info.Height))
            PixImageInfo(PixFormat.ByteRGBA, size)
        with _ ->
            null

    /// Gets basic information from the Image without loading its content.
    static member GetPixImageInfo(file : string) =
        if File.Exists file then
            use s = File.OpenRead file
            PixImageSharp.GetPixImageInfo s
        else
            null


    /// Gets the (optional) CameraInfo for the given image.
    [<Extension>]
    static member TryGetCameraInfo(info : IImageInfo) =

        option {
            if not (isNull info.Metadata.ExifProfile) then
                let! make = !!info.Metadata.ExifProfile.TryGetValue(ExifTag.Make)
                let! model = !!info.Metadata.ExifProfile.TryGetValue(ExifTag.Model)
                let! focal = !!info.Metadata.ExifProfile.TryGetValue(ExifTag.FocalLength)
                let aperture1 = !!info.Metadata.ExifProfile.TryGetValue(ExifTag.ApertureValue)
                let lensMake = !!info.Metadata.ExifProfile.TryGetValue(ExifTag.LensMake)
                let lensModel = !!info.Metadata.ExifProfile.TryGetValue(ExifTag.LensModel)


                let aperture =
                    match aperture1 with
                    | Some a -> Some a
                    | None -> !!info.Metadata.ExifProfile.TryGetValue(ExifTag.MaxApertureValue)

                let focal35 =
                    !!info.Metadata.ExifProfile.TryGetValue(ExifTag.FocalLengthIn35mmFilm)

                let crop =
                    match focal35 with
                    | Some f35 -> float f35 / focal.ToDouble()
                    | None ->
                        if make.ToLower().Contains "canon" then 1.6 else 1.5

                try
                    return {
                        size        = info |> getImageTrafo |> ImageTrafo.transformSize (V2i(info.Width, info.Height))
                        cropFactor  = crop
                        make        = make.Trim()
                        model       = model.Trim()
                        focal       = focal.ToDouble()
                        aperture    = aperture |> Option.map (fun s -> s.ToDouble())
                        lensMake    = lensMake |> Option.map (fun s -> s.Trim())
                        lensModel   = lensModel |> Option.map (fun s -> s.Trim())
                    }
                with _ ->
                    return! None
            else
                return! None

        }

    [<Extension>]
    static member GetMetadata(image : IImageInfo) =
        let mutable res = Map.empty
        let exif = image.Metadata.ExifProfile
        if not (isNull exif) then
            for t in exif.Values do
                let name = sprintf "%A" t.Tag
                let value = t
                res <- Map.add name value res

        let icc = image.Metadata.IccProfile
        if not (isNull icc) then
            for t in icc.Entries do
                Log.warn "%A" t

        res
    /// Gets the (optional) CameraInfo for the given file.
    static member TryGetCameraInfo(file : string) =
        try
            use s = File.OpenRead file
            let info = Image.Identify s
            info.TryGetCameraInfo()
        with _ ->
            None

    /// Gets the CameraInfo for the given image or fails if not existing.
    [<Extension>]
    static member GetCameraInfo(info : IImageInfo) =
        match info.TryGetCameraInfo() with
        | Some info -> info
        | None -> failwithf "image does not contain CameraInfo: %A" info

    /// Gets the CameraInfo for the given file or fails if no camera info was found.
    static member GetCameraInfo(file : string) =
        match PixImageSharp.TryGetCameraInfo file with
        | Some info -> info
        | None -> failwithf "%s does not contain CameraInfo" file

    /// Converts the PixImage to a ImageSharp Image.
    [<Extension>]
    static member ToImage(image : PixImage) =
        toImage image.PixFormat image

    /// Converts the ImageSharp Image to a PixImage using the given transformation.
    [<Extension>]
    static member ToPixImage(image : Image, trafo : ImageTrafo) =
        if isNull image then raise <| NullReferenceException("Image")
        let t = image.GetType().GetGenericArguments().[0]
        toPixImage t image trafo

    /// Converts the ImageSharp Image to a PixImage.
    [<Extension>]
    static member ToPixImage(image : Image) =
        image.ToPixImage(ImageTrafo.Identity)

    [<Extension>]
    static member SaveImageSharp(image : PixImage<'T>, file : string, saveParams : PixSaveParams) =
        use img = toImage image.PixFormat image

        match tryGetEncoder saveParams with
        | Some encoder -> img.Save(file, encoder)
        | _ -> img.Save(file)

    [<Extension>]
    static member SaveImageSharp(image : PixImage<'T>, stream : Stream, saveParams : PixSaveParams) =
        use img = toImage image.PixFormat image

        match tryGetEncoder saveParams with
        | Some encoder -> img.Save(stream, encoder)
        | _ -> failwithf "[ImageSharp] format %A not supported" saveParams.Format

    /// Saves the Image to a file using ImageSharp.
    [<Extension>]
    static member SaveImageSharp(image : PixImage<'T>, file : string) =
        use img = toImage image.PixFormat image
        img.Save file

    /// Saves the Image to a file using ImageSharp.
    [<Extension>]
    static member SaveImageSharp(image : PixImage<'T>, file : string, quality : int) =
        use img = toImage image.PixFormat image
        let ext = Path.GetExtension(file).ToLower()
        if ext = ".jpg" || ext = ".jpeg" then
            img.Save(file, Jpeg.JpegEncoder(Quality = toNullable quality))
        else
            img.Save(file)

    /// Saves the Image to a Stream using the given format and quality.
    [<Extension>]
    static member SaveImageSharp(image : PixImage<'T>, stream : Stream, fmt : PixFileFormat, quality : int) =
        use img = toImage image.PixFormat image
        match fmt with
        | PixFileFormat.J2k | PixFileFormat.Jpeg ->
            img.SaveAsJpeg(stream, Jpeg.JpegEncoder(Quality = toNullable quality))
        | PixFileFormat.Png ->
            img.SaveAsPng(stream)
        | PixFileFormat.Bmp ->
            img.SaveAsBmp(stream)
        | PixFileFormat.Gif ->
            img.SaveAsGif(stream)
        | _ ->
            failwithf "[ImageSharp] format %A not supported" fmt

    /// Saves the Image to a Stream using the given format.
    [<Extension>]
    static member SaveImageSharp(image : PixImage<'T>, stream : Stream, fmt : PixFileFormat) =
        PixImageSharp.SaveImageSharp(image, stream, fmt, -1)

    /// Converts an Image to binary data with the given format and quality.
    [<Extension>]
    static member ToBinary(img : Image, fmt : PixFileFormat, quality: int) =
        use stream = new MemoryStream()
        match fmt with
        | PixFileFormat.J2k | PixFileFormat.Jpeg ->
            img.SaveAsJpeg(stream, Jpeg.JpegEncoder(Quality = toNullable quality))
        | PixFileFormat.Png ->
            img.SaveAsPng(stream)
        | PixFileFormat.Bmp ->
            img.SaveAsBmp(stream)
        | PixFileFormat.Gif ->
            img.SaveAsGif(stream)
        | _ ->
            failwithf "[ImageSharp] format %A not supported" fmt
        stream.ToArray()

    /// Converts a PixImage to binary data with the given format and quality.
    [<Extension>]
    static member ToBinary(image : PixImage<'T>, fmt : PixFileFormat, quality: int) =
        use ms = new MemoryStream()
        image.SaveImageSharp(ms, fmt, quality)
        ms.ToArray()

    /// Converts a PixImage to binary data with the given format.
    [<Extension>]
    static member ToBinary(image : PixImage<'T>, fmt : PixFileFormat) =
        use ms = new MemoryStream()
        image.SaveImageSharp(ms, fmt)
        ms.ToArray()

    /// Converts a PixImage to png data.
    [<Extension>]
    static member ToPngData(image : PixImage<'T>) = image.ToBinary(PixFileFormat.Png)

    /// Converts a PixImage to jpeg data with the specified quality.
    [<Extension>]
    static member ToJpegData(image : PixImage<'T>, quality : int) = image.ToBinary(PixFileFormat.Jpeg, quality)

    /// Converts a PixImage to jpeg data.
    [<Extension>]
    static member ToJpegData(image : PixImage<'T>) = image.ToBinary(PixFileFormat.Jpeg)

    /// Converts a PixImage to bmp data.
    [<Extension>]
    static member ToBmpData(image : PixImage<'T>) = image.ToBinary(PixFileFormat.Bmp)

    /// Converts a PixImage to gif data.
    [<Extension>]
    static member ToGifData(image : PixImage<'T>) = image.ToBinary(PixFileFormat.Gif)

    /// Resizes the image to the specified size using the given resampler.
    [<Extension>]
    static member ResizedImageSharp(image : PixImage<'T>, size : V2i, sampler : IResampler) =
        use img = toImage image.PixFormat image
        img.Mutate(fun x ->
            x.Resize(
                ResizeOptions(
                    Size = Size(size.X, size.Y),
                    Mode = ResizeMode.Stretch,
                    Sampler = sampler,
                    Position = AnchorPositionMode.Center
                )
            )
            |> ignore
        )

        let tp = img.GetType().GetGenericArguments().[0]
        toPixImage tp img ImageTrafo.Identity |> unbox<PixImage<'T>>

    /// Resizes the image to the specified size using the given resampler.
    [<Extension>]
    static member ResizedImageSharp(image : Image, size : V2i, sampler : IResampler) =
        let img = image.Clone(System.Action<_>(ignore))
        img.Mutate(fun x ->
            let o = ResizeOptions()
            o.Size <- Size(size.X, size.Y)
            o.Mode <- ResizeMode.Stretch
            o.Sampler <- sampler
            o.Position <- AnchorPositionMode.Center
            x.Resize(o)
            |> ignore
        )
        img

    /// Resizes the image to the specified size using the given interpolation mode.
    [<Extension>]
    static member ResizedImageSharp(image : Image, size : V2i, interpolation : ImageInterpolation) =
        let sampler =
            match interpolation with
            | ImageInterpolation.Near -> KnownResamplers.NearestNeighbor
            | ImageInterpolation.Linear -> KnownResamplers.Triangle
            | ImageInterpolation.Cubic -> KnownResamplers.Bicubic
            | ImageInterpolation.Lanczos -> KnownResamplers.Lanczos3
            | ImageInterpolation.SuperSample -> KnownResamplers.Welch
            | _ -> KnownResamplers.Bicubic

        image.ResizedImageSharp(size, sampler)

    /// Resizes the image to the specified size using the given interpolation mode.
    [<Extension>]
    static member ResizedImageSharp(image : PixImage<'T>, size : V2i, interpolation : ImageInterpolation) =
        let sampler =
            match interpolation with
            | ImageInterpolation.Near -> KnownResamplers.NearestNeighbor
            | ImageInterpolation.Linear -> KnownResamplers.Triangle
            | ImageInterpolation.Cubic -> KnownResamplers.Bicubic
            | ImageInterpolation.Lanczos -> KnownResamplers.Lanczos3
            | ImageInterpolation.SuperSample -> KnownResamplers.Welch
            | _ -> KnownResamplers.Bicubic

        image.ResizedImageSharp(size, sampler)

    /// Sobel edge detection.
    [<Extension>]
    static member EdgeDetect(img : PixImage<'T>) =
        use img = toImage img.PixFormat img
        img.Mutate(fun image ->
            image
                .Grayscale()
                .DetectEdges(Processing.Processors.Convolution.EdgeDetectorCompassKernel.Robinson)
                |> ignore
        )
        img.ToPixImage() |> unbox<PixImage<'T>>

    /// Sobel edge detection.
    [<Extension>]
    static member EdgeDetect(image : PixImage) =
        image.Visit {
            new IPixImageVisitor<PixImage> with
                member x.Visit<'T>(image : PixImage<'T>) = image.EdgeDetect() :> PixImage
        }

    //[<Extension>]
    //static member EqualizeHistogram(image : Image) =
    //    image.Clone(fun ctx ->
    //        ctx.HistogramEqualization()
    //        |> ignore
    //    )
    [<Extension>]
    static member ToGrayByte(image : Image) =
        image.Clone(fun ctx ->
            //let o = Processors.Normalization.HistogramEqualizationOptions()
            //o.Method <- HistogramEqualizationMethod.AdaptiveSlidingWindow
            //o.ClipHistogram <- true
            //o.NumberOfTiles <- 20
            ctx.Grayscale().HistogramEqualization()
            |> ignore
        ).CloneAs<L8>()

    [<Extension>]
    static member SaveImageSharp(image : PixImage, file : string, saveParams : PixSaveParams) =
        image.Visit {
            new IPixImageVisitor<obj> with
                member x.Visit<'T>(image : PixImage<'T>) = image.SaveImageSharp(file, saveParams); null
        } |> ignore

    [<Extension>]
    static member SaveImageSharp(image : PixImage, stream : Stream, saveParams : PixSaveParams) =
        image.Visit {
            new IPixImageVisitor<obj> with
                member x.Visit<'T>(image : PixImage<'T>) = image.SaveImageSharp(stream, saveParams); null
        } |> ignore

    /// Saves the Image to a file using ImageSharp.
    [<Extension>]
    static member SaveImageSharp(image : PixImage, file : string, quality : int) =
        image.Visit {
            new IPixImageVisitor<obj> with
                member x.Visit<'T>(image : PixImage<'T>) = image.SaveImageSharp(file, quality); null
        } |> ignore

    /// Saves the Image to a file using ImageSharp.
    [<Extension>]
    static member SaveImageSharp(image : PixImage, file : string) =
        image.Visit {
            new IPixImageVisitor<obj> with
                member x.Visit<'T>(image : PixImage<'T>) = image.SaveImageSharp(file); null
        } |> ignore

    /// Saves the Image to a Stream using the given format and quality.
    [<Extension>]
    static member SaveImageSharp(image : PixImage, stream : Stream, fmt : PixFileFormat, quality : int) =
        image.Visit {
            new IPixImageVisitor<obj> with
                member x.Visit<'T>(image : PixImage<'T>) = image.SaveImageSharp(stream, fmt, quality); null
        } |> ignore

    /// Saves the Image to a Stream using the given format.
    [<Extension>]
    static member SaveImageSharp(image : PixImage, stream : Stream, fmt : PixFileFormat) =
        image.Visit {
            new IPixImageVisitor<obj> with
                member x.Visit<'T>(image : PixImage<'T>) = image.SaveImageSharp(stream, fmt); null
        } |> ignore

    /// Converts a PixImage to binary data with the given format and quality.
    [<Extension>]
    static member ToBinary(image : PixImage, fmt : PixFileFormat, quality: int) =
        image.Visit {
            new IPixImageVisitor<byte[]> with
                member x.Visit<'T>(image : PixImage<'T>) = image.ToBinary(fmt, quality)
        }

    /// Converts a PixImage to binary data with the given format.
    [<Extension>]
    static member ToBinary(image : PixImage, fmt : PixFileFormat) =
        image.Visit {
            new IPixImageVisitor<byte[]> with
                member x.Visit<'T>(image : PixImage<'T>) = image.ToBinary(fmt)
        }

    /// Converts a PixImage to png data with the given quality.
    [<Extension>]
    static member ToPngData(image : PixImage, quality : int) = image.ToBinary(PixFileFormat.Jpeg, quality)

    /// Converts a PixImage to jpeg data with the given quality.
    [<Extension>]
    static member ToJpegData(image : PixImage, quality : int) = image.ToBinary(PixFileFormat.Jpeg, quality)

    /// Converts a PixImage to jpeg data.
    [<Extension>]
    static member ToJpegData(image : PixImage) = image.ToBinary(PixFileFormat.Jpeg)

    /// Converts a PixImage to bmp data.
    [<Extension>]
    static member ToBmpData(image : PixImage) = image.ToBinary(PixFileFormat.Bmp)

    /// Converts a PixImage to gif data.
    [<Extension>]
    static member ToGifData(image : PixImage) = image.ToBinary(PixFileFormat.Gif)

    /// Resizes the image to the specified size using the given resampler.
    [<Extension>]
    static member ResizedImageSharp(image : PixImage, size : V2i, sampler : IResampler) =
        image.Visit {
            new IPixImageVisitor<PixImage> with
                member x.Visit<'T>(image : PixImage<'T>) = image.ResizedImageSharp(size, sampler) :> PixImage
        }

    /// Resizes the image to the specified size using the given interpoation mode.
    [<Extension>]
    static member ResizedImageSharp(image : PixImage, size : V2i, interpolation : ImageInterpolation) =
        image.Visit {
            new IPixImageVisitor<PixImage> with
                member x.Visit<'T>(image : PixImage<'T>) = image.ResizedImageSharp(size, interpolation) :> PixImage
        }

module PixImageSharp =
    type Resample =
        static member Nearest = KnownResamplers.NearestNeighbor
        static member Linear = KnownResamplers.Triangle
        static member Cubic = KnownResamplers.Bicubic
        static member Welch = KnownResamplers.Welch
        static member CatmullRom = KnownResamplers.CatmullRom
        static member Hermite = KnownResamplers.Hermite
        static member MitchellNetravali = KnownResamplers.MitchellNetravali
        static member Robidoux = KnownResamplers.Robidoux
        static member RobidouxSharp = KnownResamplers.RobidouxSharp
        static member Spline = KnownResamplers.Spline
        static member Box = KnownResamplers.Box
        static member Lanczos2 = KnownResamplers.Lanczos2
        static member Lanczos3 = KnownResamplers.Lanczos3
        static member Lanczos5 = KnownResamplers.Lanczos5
        static member Lanczos8 = KnownResamplers.Lanczos8