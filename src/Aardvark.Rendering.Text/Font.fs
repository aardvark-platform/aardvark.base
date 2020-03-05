namespace Aardvark.Rendering.Text

#nowarn "9"
#nowarn "51"

open System
open System.Linq
open System.Threading
open System.Collections.Generic
open System.Collections.Concurrent
open System.Runtime.CompilerServices
open Aardvark.Base
open Aardvark.Base.Rendering
open System.Drawing


[<Flags>]
type FontStyle =
    | Regular = 0
    | Bold = 1
    | Italic = 2
    | BoldItalic = 3

type Shape(path : Path) =

    static let quad = 
        Path.ofList [
            PathSegment.line V2d.OO V2d.OI
            PathSegment.line V2d.OI V2d.II
            PathSegment.line V2d.II V2d.IO
            PathSegment.line V2d.IO V2d.OO
        ] |> Shape

    let mutable geometry = None
    
    static member Quad = quad

    member x.Path = path

    member x.Geometry =
        match geometry with
            | Some g -> g
            | None ->
                let g = Path.toGeometry path
                geometry <- Some g
                g

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
                
            
            

    module FontResolver =
        module private Win32 =
            open Microsoft.FSharp.NativeInterop
            open System.Runtime.InteropServices
            open System.Security

            type HKey =
                | HKEY_CLASSES_ROOT = 0x80000000
                | HKEY_CURRENT_USER = 0x80000001
                | HKEY_LOCAL_MACHINE = 0x80000002
                | HKEY_USERS = 0x80000003
                | HKEY_PERFORMANCE_DATA = 0x80000004
                | HKEY_CURRENT_CONFIG = 0x80000005
                | HKEY_DYN_DATA = 0x80000006

            type Flags =
                | RRF_RT_ANY = 0x0000ffff
                | RRF_RT_DWORD = 0x00000018
                | RRF_RT_QWORD = 0x00000048
                | RRF_RT_REG_BINARY = 0x00000008
                | RRF_RT_REG_DWORD = 0x00000010
                | RRF_RT_REG_EXPAND_SZ = 0x00000004
                | RRF_RT_REG_MULTI_SZ = 0x00000020
                | RRF_RT_REG_NONE = 0x00000001
                | RRF_RT_REG_QWORD = 0x00000040
                | RRF_RT_REG_SZ = 0x00000002

            [<DllImport("kernel32.dll")>]
            extern int RegGetValue(HKey hkey, string lpSubKey, string lpValue, Flags dwFlags, uint32& pdwType, nativeint pvData, uint32& pcbData)
            
            let tryGetFontFileName (family : string) (style : FontStyle) =

                let suffix =
                    match style with
                        | FontStyle.BoldItalic -> " Bold Italic"
                        | FontStyle.Bold -> " Bold"
                        | FontStyle.Italic -> " Italic"
                        | _ -> ""

                let name = sprintf "%s%s (TrueType)" family suffix
                let arr : byte[] = Array.zeroCreate 1024
                let gc = GCHandle.Alloc(arr, GCHandleType.Pinned)

                try
                    let ptr = gc.AddrOfPinnedObject()
                    let mutable pdwType = 0u
                    let mutable pcbData = uint32 arr.Length
                    if RegGetValue(HKey.HKEY_LOCAL_MACHINE, "software\\microsoft\\windows nt\\currentversion\\Fonts", name, Flags.RRF_RT_REG_SZ, &pdwType, ptr, &pcbData) = 0 then
                        if pcbData > 0u && arr.[int pcbData - 1] = 0uy then pcbData <- pcbData - 1u
                        let file = System.Text.Encoding.UTF8.GetString(arr, 0, int pcbData)
                        let path = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), file)
                        Some path
                    else
                        None
                finally
                    gc.Free()


        let tryResolveFont (family : string) (style : FontStyle) : Option<string> =
            match Environment.OSVersion with
                | Windows -> Win32.tryGetFontFileName family style
                | _ -> failwith "not implemented"


        let resolveFont (family : string) (style : FontStyle) =
            match tryResolveFont family style with
                | Some file -> file
                | None -> failwithf "[Text] could not get font %s %A" family style




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
    inherit Shape(Path.ofGlyph scale g)

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



type private FontImpl private(f : Typeface) =
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
            let resName = typeof<FontImpl>.Assembly.GetManifestResourceNames() |> Array.find (fun n -> n.EndsWith "Symbola.ttf")
            use s = typeof<FontImpl>.Assembly.GetManifestResourceStream(resName)
            new FontImpl(s)
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

    static let getTypeFace (file : string) =
        try 
            use stream = System.IO.File.OpenRead file
            let reader = OpenFontReader()
            reader.Read(stream, 0, ReadFlags.Full)
        with e ->
            failwithf "could not load font %s: %A" file e.Message

    static member Symbola = symbola.Value

    member x.Family = f.Name
    member x.LineHeight = lineHeight
    member x.Descent = descent
    member x.Ascent = ascent
    member x.LineGap = lineGap
    member x.InternalLeading = internalLeading
    member x.ExternalLeading = externalLeading
    member x.Style = FontStyle.Regular
    member x.Spacing = spacing



    member x.GetGlyph(c : CodePoint) =
        get c x

    member x.GetKerning(l : CodePoint, r : CodePoint) =
        let l = f.GetGlyphIndex (l.Value)
        let r = f.GetGlyphIndex (r.Value)
        let d = f.GetKernDistance(l, r)
        float d * scale

    new(file : string) = 
        FontImpl(getTypeFace file)

    new(stream : System.IO.Stream) = 
        let reader = OpenFontReader()
        FontImpl(reader.Read(stream, 0, ReadFlags.Full))



type Font private(impl : FontImpl, family : string, style : FontStyle) =

    static let symbola = 
        lazy (
            let impl = FontImpl.Symbola
            new Font(impl, impl.Family, impl.Style)
        )

    static let systemTable = System.Collections.Concurrent.ConcurrentDictionary<string * FontStyle, FontImpl>()
    static let fileTable = System.Collections.Concurrent.ConcurrentDictionary<string, FontImpl>()

    static member Symbola = symbola.Value

    member x.Family = family
    member x.LineHeight = impl.LineHeight
    member x.Descent = impl.Descent
    member x.Ascent = impl.Ascent
    member x.LineGap = impl.LineGap
    member x.InternalLeading = impl.InternalLeading
    member x.ExternalLeading = impl.ExternalLeading
    member x.Style = style
    member x.Spacing = impl.Spacing
    member x.GetGlyph(c : CodePoint) = impl.GetGlyph(c)
    member x.GetKerning(l : CodePoint, r : CodePoint) = impl.GetKerning(l,r)

    static member Load(file : string) =
        let impl =
            fileTable.GetOrAdd(file, fun file ->
                let impl = FontImpl(file)
                impl
            )
        Font(impl, impl.Family, impl.Style)

    new(family : string, style : FontStyle) =
        let impl =
            systemTable.GetOrAdd((family, style), fun (family, style) ->
                let impl = FontImpl(FontResolver.resolveFont family style)
                impl
            )
        Font(impl, family, style)
    
    new(family : string) = Font(family, FontStyle.Regular)

type ShapeCache(r : IRuntime) =
    static let cache = ConcurrentDictionary<IRuntime, ShapeCache>()

    let types =
        Map.ofList [
            DefaultSemantic.Positions, typeof<V3f>
            Path.Attributes.KLMKind, typeof<V4f>
        ]

    let pool = r.CreateGeometryPool(types)
    let ranges = ConcurrentDictionary<Shape, Range1i>()


    let surfaceCache = ConcurrentDictionary<IFramebufferSignature, IBackendSurface>()
    let boundarySurfaceCache = ConcurrentDictionary<IFramebufferSignature, IBackendSurface>()
    let billboardSurfaceCache = ConcurrentDictionary<IFramebufferSignature, IBackendSurface>()

    let effect =
        FShade.Effect.compose [
            Path.Shader.pathVertex      |> toEffect
            //Path.Shader.pathTrafo       |> toEffect
            Path.Shader.pathFragment    |> toEffect
        ]
        
    let instancedEffect =
        FShade.Effect.compose [
            Path.Shader.pathVertexInstanced |> toEffect
            Path.Shader.pathFragment        |> toEffect
        ]

    let boundaryEffect =
        FShade.Effect.compose [
            Path.Shader.boundaryVertex  |> toEffect
            Path.Shader.boundary        |> toEffect
        ]

    let instancedBoundaryEffect =
        FShade.Effect.compose [
            DefaultSurfaces.instanceTrafo   |> toEffect
            Path.Shader.boundaryVertex      |> toEffect
            Path.Shader.boundary            |> toEffect
        ]

    let billboardEffect =
        FShade.Effect.compose [
            Path.Shader.pathVertexBillboard |> toEffect
            Path.Shader.pathFragment    |> toEffect
        ]
        
    let instancedBillboardEffect =
        FShade.Effect.compose [
            Path.Shader.pathVertexInstancedBillboard |> toEffect
            Path.Shader.pathFragment        |> toEffect
        ]

    let surface (s : IFramebufferSignature) =
        surfaceCache.GetOrAdd(s, fun s -> 
            r.PrepareEffect(
                s, [
                    Path.Shader.pathVertex      |> toEffect
                    Path.Shader.pathFragment    |> toEffect
                ]
            )
        )

    let boundarySurface (s : IFramebufferSignature) =
        boundarySurfaceCache.GetOrAdd(s, fun s ->
            r.PrepareEffect(
                s, [
                    Path.Shader.boundaryVertex  |> toEffect
                    Path.Shader.boundary        |> toEffect
                ]
            )
        )

    let billboardSurface (s : IFramebufferSignature) =
        billboardSurfaceCache.GetOrAdd(s, fun s ->
            r.PrepareEffect(
                s, [
                    Path.Shader.pathVertexBillboard  |> toEffect
                    Path.Shader.boundary        |> toEffect
                ]
            )
        )
        

    do 
        r.OnDispose.Add(fun () ->
            boundarySurfaceCache.Values |> Seq.iter r.DeleteSurface
            surfaceCache.Values |> Seq.iter r.DeleteSurface
            billboardSurfaceCache.Values |> Seq.iter r.DeleteSurface
            pool.Dispose()
            ranges.Clear()
            cache.Clear()
        )

    let vertexBuffers =
        { new IAttributeProvider with
            member x.TryGetAttribute(sem) =
                match pool.TryGetBufferView sem with
                    | Some bufferView ->
                        Some bufferView
                    | None ->
                        None

            member x.All = Seq.empty
            member x.Dispose() = ()
        }

    static member GetOrCreateCache(r : IRuntime) =
        cache.GetOrAdd(r, fun r ->
            new ShapeCache(r)
        )

    member x.Effect = effect
    member x.InstancedEffect = instancedEffect
    member x.BoundaryEffect = boundaryEffect
    member x.InstancedBoundaryEffect = instancedBoundaryEffect
    member x.BillboardEffect = billboardEffect
    member x.InstancedBillboardEffect = instancedBillboardEffect
    member x.Surface s = surface s :> ISurface
    member x.BoundarySurface s = boundarySurface s :> ISurface
    member x.BillboardSurface s = billboardSurface s :> ISurface
    member x.VertexBuffers = vertexBuffers

    member x.GetBufferRange(shape : Shape) =
        ranges.GetOrAdd(shape, fun shape ->
            let ptr = pool.Alloc(shape.Geometry)
            let last = ptr.Offset + ptr.Size - 1n |> int
            let first = ptr.Offset |> int
            Range1i(first, last)
        )

    member x.PrepareShaders(signature : IFramebufferSignature) =
        let _ = surface signature
        let _ = boundarySurface signature
        ()


    member x.Dispose() =
        pool.Dispose()
        ranges.Clear()

[<AbstractClass; Sealed; Extension>]
type PrepareFontExtensions private() =

    [<Extension>]
    static member PrepareGlyphs(r : IRuntime, f : Font, chars : seq<CodePoint>) =
        let cache = ShapeCache.GetOrCreateCache r

        for c in chars do
            cache.GetBufferRange (f.GetGlyph(c)) |> ignore
            
    [<Extension>]
    static member PrepareGlyphs(r : IRuntime, f : Font, chars : seq<char>) =
        let cache = ShapeCache.GetOrCreateCache r

        for c in chars do
            cache.GetBufferRange (f.GetGlyph(CodePoint c)) |> ignore
            
    [<Extension>]
    static member PrepareTextShaders(r : IRuntime, f : Font, signature : IFramebufferSignature) =
        let cache = ShapeCache.GetOrCreateCache r
        cache.PrepareShaders signature



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




