namespace Aardvark.Base.Fonts

open System
open System.IO.Compression
open System.Runtime.CompilerServices
open System.Runtime.InteropServices
open Aardvark.Base

[<Flags>]
type FontStyle =
    | Regular = 0
    | Bold = 1
    | Italic = 2
    | BoldItalic = 3

module internal FontStyle =

    let toWeightAndItalic (style: FontStyle) =
        let weight =
            match style with
            | FontStyle.Bold | FontStyle.BoldItalic -> 700
            | _ -> 400

        let italic =
            match style with
            | FontStyle.Italic | FontStyle.BoldItalic -> true
            | _ -> false

        struct (weight, italic)

type Shape(path : Path, windingRule : WindingRule) =

    static let quad =
        Shape(
            Path.ofList [
                PathSegment.line V2d.OO V2d.OI
                PathSegment.line V2d.OI V2d.II
                PathSegment.line V2d.II V2d.IO
                PathSegment.line V2d.IO V2d.OO
            ],
            WindingRule.NonZero
        )

    let mutable geometry = ValueNone

    static member Quad = quad

    member x.Path = path

    member x.Geometry =
        match geometry with
        | ValueSome g -> g
        | ValueNone ->
            let g = Path.toGeometry windingRule path
            geometry <- ValueSome g
            g

    new(path : Path) = Shape(path, WindingRule.NonZero)

open Typography.OpenFont

[<AutoOpen>]
module private Typography =

    module GlyphPoint =
        let toV2d (scale : float) (p : GlyphPointF) = scale * V2d(p.X, p.Y)

    module Bounds =
        let toBox2d (scale : float) (b : Bounds) =
            Box2d(scale * float b.XMin, scale * float b.YMin, scale * float b.XMax, scale * float b.YMax)

    module Path =
        type PathTranslator(scale : float) =
            let mutable start : Option<V2d> = None
            let mutable pos : Option<V2d> = None
            let mutable bb = Box2d.Invalid
            let list = System.Collections.Generic.List<PathSegment>()

            member x.ToArray() = list.ToArray()

            member x.Bounds = (Box2d.Invalid, list) ||> Seq.fold (fun b s -> Box.Union(b, PathSegment.bounds s))

            interface IGlyphTranslator with
                member x.BeginRead(_) = ()
                member x.EndRead() = ()

                member x.CloseContour() =
                    match start, pos with
                        | Some s, Some p ->
                            if not (Fun.ApproximateEquals(p, s, bb.Size.NormMax * 1E-8)) then
                                match PathSegment.tryLine p s with
                                | Some l -> list.Add l
                                | None -> ()
                        | _ ->
                            ()
                    start <- None
                    pos <- None
                    ()

                member x.MoveTo(xa,ya) =
                    let pa = V2d(xa, ya) * scale
                    bb.ExtendBy pa
                    pos <- Some pa
                    match start with
                        | None -> start <- Some pa
                        | _ -> ()

                member x.LineTo(xa, ya) =
                    match pos with
                        | Some p0 ->
                            let pa = V2d(xa, ya) * scale
                            bb.ExtendBy pa
                            match PathSegment.tryLine p0 pa with
                            | Some l -> list.Add(l)
                            | None -> ()
                            pos <- Some pa
                        | None ->
                            failwith "asdasd"
                    ()
                member x.Curve3(xa, ya, xb, yb) =
                    match pos with
                        | Some p0 ->
                            let pa = V2d(xa, ya) * scale
                            let pb = V2d(xb, yb) * scale
                            bb.ExtendBy pa
                            bb.ExtendBy pb
                            match PathSegment.tryBezier2 p0 pa pb with
                            | Some b -> list.Add b
                            | None -> ()
                            pos <- Some pb
                        | None ->
                            ()
                            //failwith "asdasd"

                member x.Curve4(xa, ya, xb, yb, xc, yc) =
                    match pos with
                        | Some p0 ->
                            let pa = V2d(xa, ya) * scale
                            let pb = V2d(xb, yb) * scale
                            let pc = V2d(xc, yc) * scale
                            bb.ExtendBy pa
                            bb.ExtendBy pb
                            bb.ExtendBy pc

                            match PathSegment.tryBezier3 p0 pa pb pc with
                            | Some b -> list.Add b
                            | None -> ()
                            pos <- Some pc
                        | None ->
                            failwith "asdasd"

        let ofGlyph (scale : float) (g : Glyph) =
            if g.HasGlyphInstructions then
                let tx = PathTranslator(scale)
                tx.Read(g.GlyphPoints, g.EndPoints)
                let segments = tx.ToArray()
                { bounds = tx.Bounds; outline = segments }
            elif g.IsCffGlyph then
                let data = g.GetCff1GlyphData()
                let tx = PathTranslator(scale / 1000.0)
                let e : Typography.OpenFont.CFF.CffEvaluationEngine = Typography.OpenFont.CFF.CffEvaluationEngine()
                e.Run(tx, g.GetOwnerCff(), data, 1000.0f)
                let segments = tx.ToArray()
                { bounds = tx.Bounds; outline = segments }
            else
                { bounds = Bounds.toBox2d scale g.Bounds; outline = [||] }
                //[|
                //    let mutable i = 0
                //    while i < g.GlyphPoints.Length do
                //        let mutable o = i + 1
                //        while o < g.GlyphPoints.Length && not g.GlyphPoints.[o].OnCurve do
                //            o <- o + 1

                //        if o < g.GlyphPoints.Length then
                //            let len = 1 + o - i
                //            match len with
                //                | 2 ->
                //                    let p0  = g.GlyphPoints.[i] |> GlyphPoint.toV2d scale
                //                    let p1  = g.GlyphPoints.[o] |> GlyphPoint.toV2d scale
                //                    yield PathSegment.line p0 p1
                //                | 3 ->
                //                    let p0  = g.GlyphPoints.[i] |> GlyphPoint.toV2d scale
                //                    let a   = g.GlyphPoints.[i+1] |> GlyphPoint.toV2d scale
                //                    let p1  = g.GlyphPoints.[o] |> GlyphPoint.toV2d scale
                //                    yield PathSegment.bezier2 p0 a p1
                //                | 4 ->
                //                    let p0  = g.GlyphPoints.[i] |> GlyphPoint.toV2d scale
                //                    let a   = g.GlyphPoints.[i+1] |> GlyphPoint.toV2d scale
                //                    let b   = g.GlyphPoints.[i+2] |> GlyphPoint.toV2d scale
                //                    let p1  = g.GlyphPoints.[o] |> GlyphPoint.toV2d scale
                //                    yield PathSegment.bezier3 p0 a b p1

                //                | _ ->
                //                    failwithf "invalid path segment: %A" len

                //        i <- o
                //|]





[<Struct; StructuredFormatDisplay("{AsString}")>]
type CodePoint(value : int) =
    member x.Value = value

    static member Invalid = CodePoint(-1)

    member private x.AsString = x.ToString()

    override x.ToString() =
        if value < 0 then "invalid"
        else sprintf "U+%X" value

    member x.String =
        if value &&& 0x10000 = 0 then
            System.String(char value, 1)
        else
            let value = value - 65536
            let c0 = char (0xD800 ||| ((value >>> 10)))
            let c1 = char (0xDC00 ||| (value &&& 0x3FF))
            System.String [| c0; c1 |]

    new (c : char) =
        if (uint16 c &&& 0xF800us) = 0xD800us then CodePoint(-1)
        else CodePoint(int c)

[<AbstractClass; Sealed; Extension>]
type CodePointStringExtensions private() =

    [<Extension>]
    static member ToCodePointArray(str : string) =
        if str.Length > 0 then
            let mutable arr : CodePoint[] = Array.zeroCreate str.Length
            let mutable oi = 0
            let mutable c0 = str.[0] |> uint16
            let mutable lastSurrogate = (c0 &&& 0xF800us) = 0xD800us
            if lastSurrogate then
                c0 <- c0 &&& 0x27FFus
            else
                arr.[oi] <- CodePoint(int c0)
                oi <- oi + 1

            for ii in 1 .. str.Length - 1 do
                let c1 = str.[ii] |> uint16
                if lastSurrogate then
                    let v1 = c1 &&& 0x23FFus
                    let code = 0x10000 ||| (int c0 <<< 10) ||| int v1
                    arr.[oi] <- CodePoint(code)
                    oi <- oi + 1
                    lastSurrogate <- false
                elif (c1 &&& 0xF800us) = 0xD800us then
                    c0 <- c1 &&& 0x27FFus
                    lastSurrogate <- true
                else
                    arr.[oi] <- CodePoint(int c1)
                    oi <- oi + 1

            if arr.Length > oi then System.Array.Resize(&arr, oi)
            arr
        else
            [||]

    [<Extension>]
    static member GetString(codepoints : CodePoint[]) =
        let sb = System.Text.StringBuilder()

        for c in codepoints do
            let value = c.Value
            if value &&& 0x10000 = 0 then
                sb.Append(char value) |> ignore
            else
                let value = value - 65536
                let c0 = char (0xD800 ||| ((value >>> 10)))
                let c1 = char (0xDC00 ||| (value &&& 0x3FF))
                sb.Append c0 |> ignore
                sb.Append c1 |> ignore

        sb.ToString()

type Glyph internal(g : Typography.OpenFont.Glyph, isValid : bool, scale : float, advance : float, bearing : float, c : CodePoint) =
    inherit Shape(Path.ofGlyph scale g, WindingRule.NonZero)

    let widths =
        let width = float (g.MaxX - g.MinX) * scale
        V3d(bearing, width, advance - width - bearing)

    member x.CodePoint = c
    member x.IsValid = isValid
    member x.Bounds = base.Path.bounds

    member x.Path = base.Path
    member x.Advance =
        let sizes = widths
        sizes.X + sizes.Y + sizes.Z

    member x.Before = -0.1666



type private FontImpl internal(f : Typeface, familyName : string, weight : int, italic : bool) =
    let scale = f.CalculateScaleToPixel 1.0f |> float

    let glyphCache = Dict<CodePoint, Glyph>()

    let lineHeight = float (f.Ascender - f.Descender + f.LineGap) * scale

    let spacing =
        let idx = f.GetGlyphIndex(int ' ') |> int

        if idx >= 0 && idx < f.Glyphs.Length then
            let g = f.Glyphs.[idx]
            if g.HasOriginalAdvancedWidth then
                float g.OriginalAdvanceWidth  * scale
            else
                let a = f.GetHAdvanceWidthFromGlyphIndex(uint16 idx)
                float a * scale
        else
            float (f.GetAdvanceWidth(idx)) * scale

    // https://docs.microsoft.com/en-us/windows/desktop/gdi/string-widths-and-heights
    let ascent = float f.Ascender * scale
    let descent = float -f.Descender * scale
    let lineGap = float f.LineGap * scale
    let internalLeading = float ((f.Bounds.YMax - f.Bounds.YMin) - (f.Ascender - f.Descender) - f.LineGap) * scale
    let externalLeading = lineGap - internalLeading

    static let symbola =
        lazy (
            let assembly = typeof<FontImpl>.Assembly
            FontImpl(assembly, "Symbola.zip", "Symbola.ttf", 400, false)
        )

    let get (c : CodePoint) (self : FontImpl) =
        lock glyphCache (fun () ->
            glyphCache.GetOrCreate(c, fun c ->
                let idx = f.GetGlyphIndex(c.Value)
                if idx = 0us && c.Value > 65535 && symbola.Value <> self then
                    symbola.Value.GetGlyph c
                else
                    let glyph = f.Glyphs.[int idx]

                    let advance =
                        if glyph.HasOriginalAdvancedWidth then
                            float glyph.OriginalAdvanceWidth  * scale
                        else
                            let a = f.GetHAdvanceWidthFromGlyphIndex(idx)
                            float a * scale

                    let bearing =
                        let a = f.GetHFrontSideBearingFromGlyphIndex(idx)
                        float a * scale
                    Glyph(glyph, idx > 0us, scale, advance, bearing, c)
            )
        )

    static member Symbola = symbola.Value

    member x.Family = familyName
    member x.LineHeight = lineHeight
    member x.Descent = descent
    member x.Ascent = ascent
    member x.LineGap = lineGap
    member x.InternalLeading = internalLeading
    member x.ExternalLeading = externalLeading
    member x.Weight = weight
    member x.Italic = italic

    member x.Style =
        if weight >= 700 then
            if italic then FontStyle.BoldItalic
            else FontStyle.Bold
        else
            if italic then FontStyle.Italic
            else FontStyle.Regular

    member x.Spacing = spacing

    member x.GetGlyph(c : CodePoint) =
        get c x

    member x.GetKerning(l : CodePoint, r : CodePoint) =
        let l = f.GetGlyphIndex (l.Value)
        let r = f.GetGlyphIndex (r.Value)
        let d = f.GetKernDistance(l, r)
        float d * scale

    new (file : string, weight : int, italic : bool) =
        let data = File.readAllBytes file
        let family = try System.IO.Path.GetFileNameWithoutExtension file with _ -> null
        FontImpl(data, family, weight, italic)

    new (data : uint8[], family : string, weight : int, italic : bool) =
        let openStream() =
            new System.IO.MemoryStream(data) :> System.IO.Stream

        let entry =
            FontResolver.FontTableEntries.ofStream () openStream
            |> FontResolver.FontTableEntries.chooseBestEntry weight italic

        let family =
            if String.IsNullOrEmpty entry.FamilyName then family
            else entry.FamilyName

        let face = entry |> FontResolver.FontTableEntries.read openStream
        FontImpl(face, family, entry.Weight, entry.Italic)

    new (assembly : System.Reflection.Assembly, file : string, entryName : string, weight : int, italic : bool) =
        if isNull assembly then raise <| ArgumentNullException(nameof assembly)
        let names = assembly.GetManifestResourceNames()

        let resourceName =
            names
            |> Array.tryFindV (fun n -> n.EndsWith file)
            |> ValueOption.defaultWith (fun _ ->
                raise <| IO.FileNotFoundException($"Could not find embedded resource with name '{file}' in assembly '{assembly}'.", file)
            )

        let fontData =
            if String.IsNullOrEmpty entryName then
                use data = assembly.GetManifestResourceStream resourceName
                Stream.readAllBytes data
            else
                use zip =
                    let s = assembly.GetManifestResourceStream resourceName
                    new ZipArchive(s, ZipArchiveMode.Read)

                let entry = zip.GetEntry entryName
                if isNull entry then
                    raise <| IO.FileNotFoundException($"Could not find entry with name '{entryName}' in ZIP archive '{file}' in assembly '{assembly}'.", entryName)

                use data = entry.Open()
                Stream.readAllBytes data

        let family =
            let path = if String.IsNullOrEmpty entryName then file else entryName
            try System.IO.Path.GetFileNameWithoutExtension path with _ -> null

        FontImpl(fontData, family, weight, italic)


type Font private(impl : FontImpl) =

    static let symbola =
        lazy (
            let impl = FontImpl.Symbola
            Font impl
        )

    static let systemTable = System.Collections.Concurrent.ConcurrentDictionary<string * int * bool, FontImpl>()
    static let fileTable = System.Collections.Concurrent.ConcurrentDictionary<string, FontImpl>()

    static member Symbola = symbola.Value

    member x.Family = impl.Family
    member x.LineHeight = impl.LineHeight
    member x.Descent = impl.Descent
    member x.Ascent = impl.Ascent
    member x.LineGap = impl.LineGap
    member x.InternalLeading = impl.InternalLeading
    member x.ExternalLeading = impl.ExternalLeading
    member x.Style = impl.Style
    member x.Weight = impl.Weight
    member x.Italic = impl.Italic
    member x.Spacing = impl.Spacing
    member x.GetGlyph(c : CodePoint) = impl.GetGlyph(c)
    member x.GetKerning(l : CodePoint, r : CodePoint) = impl.GetKerning(l,r)

    static member Load(file : string, weight : int, italic : bool) =
        let impl =
            fileTable.GetOrAdd(file, fun file ->
                let impl = FontImpl(file, weight, italic)
                impl
            )
        Font(impl)

    static member Load(file : string) =
        Font.Load(file, 400, false)

    static member Load(file : string, style : FontStyle) =
        let struct (weight, italic) = FontStyle.toWeightAndItalic style
        Font.Load(file, weight, italic)

    /// <summary>
    /// Loads a font embedded in an assembly as a ZIP archive.
    /// The name of the ZIP archive does not have to be exact; the first embedded resource that ends with the given name is used.
    /// </summary>
    /// <param name="assembly">Assembly to load the embedded font from.</param>
    /// <param name="zipArchive">Name of the embedded ZIP archive.</param>
    /// <param name="entryName">Name of the ZIP entry containing the font file.</param>
    /// <param name="weight">Weight of the font to load.</param>
    /// <param name="italic">Indicates whether italic variant is desired.</param>
    static member LoadFromAssembly(assembly: System.Reflection.Assembly, zipArchive: string, entryName: string,
                                   [<Optional; DefaultParameterValue(400)>] weight: int,
                                   [<Optional; DefaultParameterValue(false)>] italic: bool) =
        let impl = FontImpl(assembly, zipArchive, entryName, weight, italic)
        Font(impl)

    /// <summary>
    /// Loads a font embedded in the calling assembly as a ZIP archive.
    /// The name of the ZIP archive does not have to be exact; the first embedded resource that ends with the given name is used.
    /// </summary>
    /// <param name="zipArchive">Name of the embedded ZIP archive.</param>
    /// <param name="entryName">Name of the ZIP entry containing the font file.</param>
    /// <param name="weight">Weight of the font to load.</param>
    /// <param name="italic">Indicates whether italic variant is desired.</param>
    static member LoadFromAssembly(zipArchive: string, entryName: string,
                                   [<Optional; DefaultParameterValue(400)>] weight: int,
                                   [<Optional; DefaultParameterValue(false)>] italic: bool) =
        let assembly = System.Reflection.Assembly.GetCallingAssembly()
        Font.LoadFromAssembly(assembly, zipArchive, entryName, weight, italic)

    /// <summary>
    /// Loads a font embedded in an assembly.
    /// The name of the font file does not have to be exact; the first embedded resource that ends with the given name is used.
    /// </summary>
    /// <param name="assembly">Assembly to load the embedded font from.</param>
    /// <param name="file">Name of the embedded font file.</param>
    /// <param name="weight">Weight of the font to load.</param>
    /// <param name="italic">Indicates whether italic variant is desired.</param>
    static member LoadFromAssembly(assembly: System.Reflection.Assembly, file: string,
                                   [<Optional; DefaultParameterValue(400)>] weight: int,
                                   [<Optional; DefaultParameterValue(false)>] italic: bool) =
        Font.LoadFromAssembly(assembly, file, null, weight, italic)

    /// <summary>
    /// Loads a font embedded in the calling assembly.
    /// The name of the font file does not have to be exact; the first embedded resource that ends with the given name is used.
    /// </summary>
    /// <param name="file">Name of the embedded font file.</param>
    /// <param name="weight">Weight of the font to load.</param>
    /// <param name="italic">Indicates whether italic variant is desired.</param>
    static member LoadFromAssembly(file: string,
                                   [<Optional; DefaultParameterValue(400)>] weight: int,
                                   [<Optional; DefaultParameterValue(false)>] italic: bool) =
        let assembly = System.Reflection.Assembly.GetCallingAssembly()
        Font.LoadFromAssembly(assembly, file, null, weight, italic)

    /// <summary>
    /// Loads a font embedded in an assembly as a ZIP archive.
    /// The name of the ZIP archive does not have to be exact; the first embedded resource that ends with the given name is used.
    /// </summary>
    /// <param name="assembly">Assembly to load the embedded font from.</param>
    /// <param name="zipArchive">Name of the embedded ZIP archive.</param>
    /// <param name="entryName">Name of the ZIP entry containing the font file.</param>
    /// <param name="style">Variant of the font to load.</param>
    static member LoadFromAssembly(assembly: System.Reflection.Assembly, zipArchive: string, entryName: string, style: FontStyle) =
        let struct (weight, italic) = FontStyle.toWeightAndItalic style
        Font.LoadFromAssembly(assembly, zipArchive, entryName, weight, italic)

    /// <summary>
    /// Loads a font embedded in the calling assembly as a ZIP archive.
    /// The name of the ZIP archive does not have to be exact; the first embedded resource that ends with the given name is used.
    /// </summary>
    /// <param name="zipArchive">Name of the embedded ZIP archive.</param>
    /// <param name="entryName">Name of the ZIP entry containing the font file.</param>
    /// <param name="style">Variant of the font to load.</param>
    static member LoadFromAssembly(zipArchive: string, entryName: string, style: FontStyle) =
        let assembly = System.Reflection.Assembly.GetCallingAssembly()
        Font.LoadFromAssembly(assembly, zipArchive, entryName, style)

    /// <summary>
    /// Loads a font embedded in an assembly.
    /// The name of the font file does not have to be exact; the first embedded resource that ends with the given name is used.
    /// </summary>
    /// <param name="assembly">Assembly to load the embedded font from.</param>
    /// <param name="file">Name of the embedded font file.</param>
    /// <param name="style">Variant of the font to load.</param>
    static member LoadFromAssembly(assembly: System.Reflection.Assembly, file: string, style: FontStyle) =
        Font.LoadFromAssembly(assembly, file, null, style)

    /// <summary>
    /// Loads a font embedded in the calling assembly.
    /// The name of the font file does not have to be exact; the first embedded resource that ends with the given name is used.
    /// </summary>
    /// <param name="file">Name of the embedded font file.</param>
    /// <param name="style">Variant of the font to load.</param>
    static member LoadFromAssembly(file: string, style: FontStyle) =
        let assembly = System.Reflection.Assembly.GetCallingAssembly()
        Font.LoadFromAssembly(assembly, file, null, style)

    new(stream : System.IO.Stream, weight : int, italic : bool) =
        let data = Stream.readAllBytes stream
        let impl = FontImpl(data, null, weight, italic)
        Font(impl)

    new(stream : System.IO.Stream, style : FontStyle) =
        let struct (weight, italic) = FontStyle.toWeightAndItalic style
        Font(stream, weight, italic)

    new(stream : System.IO.Stream) =
        Font(stream, 400, false)

    new(family : string, weight : int, italic : bool) =
        let impl =
            systemTable.GetOrAdd((family, weight, italic), fun (family, weight, italic) ->
                let (face, familyName, weight, italic) = FontResolver.loadTypeface family weight italic
                let impl = FontImpl(face, familyName, weight, italic)
                impl
            )
        Font(impl)

    new(family : string, style : FontStyle) =
        let struct (weight, italic) = FontStyle.toWeightAndItalic style
        Font(family, weight, italic)

    new(family : string) = Font(family, 400, false)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Font =
    let inline family (f : Font) = f.Family
    let inline style (f : Font) = f.Style
    let inline spacing (f : Font) = f.Spacing
    let inline lineHeight (f : Font) = f.LineHeight

    let inline glyph (c : CodePoint) (f : Font) = f.GetGlyph c
    let inline kerning (l : CodePoint) (r : CodePoint) (f : Font) = f.GetKerning(l,r)

    let inline create (family : string) (style : FontStyle) = Font(family, style)


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Glyph =
    let inline advance (g : Glyph) = g.Advance
    let inline path (g : Glyph) = g.Path
    let inline geometry (g : Glyph) = g.Geometry
    let inline codePoint (g : Glyph) = g.CodePoint
    let inline string (g : Glyph) = g.CodePoint.String