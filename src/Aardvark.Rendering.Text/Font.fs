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
            let list = System.Collections.Generic.List<PathSegment>()

            member x.ToArray() = list.ToArray()

            interface IGlyphTranslator with
                member x.BeginRead(_) = ()
                member x.EndRead() = ()

                member x.CloseContour() = 
                    match start, pos with
                        | Some s, Some p ->
                            if p <> s then
                                list.Add(PathSegment.line p s)
                        | _ ->
                            ()
                    start <- None
                    pos <- None
                    ()

                member x.MoveTo(xa,ya) =
                    let pa = V2d(xa, ya) * scale
                    pos <- Some pa
                    match start with
                        | None -> start <- Some pa
                        | _ -> ()

                member x.LineTo(xa, ya) = 
                    match pos with
                        | Some p0 ->
                            let pa = V2d(xa, ya) * scale
                            list.Add(PathSegment.line p0 pa)
                            pos <- Some pa
                        | None ->
                            failwith "asdasd"
                    ()
                member x.Curve3(xa, ya, xb, yb) =
                    match pos with
                        | Some p0 ->
                            let pa = V2d(xa, ya) * scale
                            let pb = V2d(xb, yb) * scale
                            
                            list.Add(PathSegment.bezier2 p0 pa pb)
                            pos <- Some pb
                        | None ->
                            failwith "asdasd"
                    
                member x.Curve4(xa, ya, xb, yb, xc, yc) = 
                    match pos with
                        | Some p0 ->
                            let pa = V2d(xa, ya) * scale
                            let pb = V2d(xb, yb) * scale
                            let pc = V2d(xc, yc) * scale
                            
                            list.Add(PathSegment.bezier3 p0 pa pb pc)
                            pos <- Some pc
                        | None ->
                            failwith "asdasd"

        let ofGlyph (scale : float) (g : Glyph) =

            let tx = PathTranslator(scale)
            tx.Read(g.GlyphPoints, g.EndPoints)
            let segments = tx.ToArray()
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
                
            { bounds = Bounds.toBox2d scale g.Bounds; outline = segments }
            

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


type Glyph internal(g : Typography.OpenFont.Glyph, scale : float, advance : float, bearing : float, c : char) =
    inherit Shape(Path.ofGlyph scale g)

    let widths = 
        let width = float (g.MaxX - g.MinX) * scale
        V3d(bearing, width, advance - width - bearing)
        
    member x.Character = c
    
    member x.Bounds = base.Path.bounds

    member x.Path = base.Path
    member x.Advance = 
        let sizes = widths
        sizes.X + sizes.Y + sizes.Z

    member x.Before = -0.1666

type private FontImpl(file : string) =
    let f =
        use stream = System.IO.File.OpenRead file
        let reader = OpenFontReader()
        reader.Read(stream, ReadFlags.Full)

    let scale = f.CalculateScaleToPixel 1.0f |> float

    let glyphCache = Dict<char, Glyph>()
    
    let lineHeight = float (f.Ascender - f.Descender + f.LineGap) * scale
    let spacing = float (f.GetAdvanceWidth(f.LookupIndex(int ' ') |> int)) * scale

    let get (c : char) =
        lock glyphCache (fun () ->
            glyphCache.GetOrCreate(c, fun c ->
                let idx = f.LookupIndex(int c)
                let glyph = f.Glyphs.[int idx]

                let advance = 
                    if glyph.HasOriginalAdvancedWidth then
                        float glyph.OriginalAdvanceWidth  * scale
                    else
                        let a = f.GetHAdvanceWidthFromGlyphIndex(int idx)
                        float a * scale
                        
                let bearing = 
                    let a = f.GetHFrontSideBearingFromGlyphIndex(int idx)
                    float a * scale
                Glyph(glyph, scale, advance, bearing, c)
            )
        )

    member x.Family = f.Name
    member x.LineHeight = lineHeight
    member x.Style = FontStyle.Regular
    member x.Spacing = spacing

    member x.GetGlyph(c : char) =
        get c

    member x.GetKerning(l : char, r : char) =
        let l = f.LookupIndex (int l)
        let r = f.LookupIndex (int r)
        let d = f.GetKernDistance(l, r)
        float d * scale

type Font(family : string, style : FontStyle) =
    static let table = System.Collections.Concurrent.ConcurrentDictionary<string * FontStyle, FontImpl>()

    let impl =
        table.GetOrAdd((family, style), fun (family, style) ->
            let impl = FontImpl(FontResolver.resolveFont family style)
            impl
        )

    member x.Family = family
    member x.LineHeight = impl.LineHeight
    member x.Style = style
    member x.Spacing = impl.Spacing
    member x.GetGlyph(c : char) = impl.GetGlyph c
    member x.GetKerning(l : char, r : char) = impl.GetKerning(l,r)

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

//    let signature =
//        r.CreateFramebufferSignature(
//            1, 
//            [
//                DefaultSemantic.Colors, RenderbufferFormat.Rgba8
//                //Path.Src1Color, RenderbufferFormat.Rgba8
//                DefaultSemantic.Depth, RenderbufferFormat.Depth24Stencil8
//            ]
//        )

    let surfaceCache = ConcurrentDictionary<IFramebufferSignature, IBackendSurface>()
    let boundarySurfaceCache = ConcurrentDictionary<IFramebufferSignature, IBackendSurface>()

    let effect =
        FShade.Effect.compose [
            Path.Shader.pathVertex      |> toEffect
            Path.Shader.pathTrafo       |> toEffect
            Path.Shader.pathFragment    |> toEffect
        ]
        
    let boundaryEffect =
        FShade.Effect.compose [
            Path.Shader.boundaryVertex  |> toEffect
            Path.Shader.boundary        |> toEffect
        ]


    let surface (s : IFramebufferSignature) =
        surfaceCache.GetOrAdd(s, fun s -> 
            r.PrepareEffect(
                s, [
                    Path.Shader.pathVertex      |> toEffect
                    Path.Shader.pathTrafo       |> toEffect
                    Path.Shader.pathFragment    |> toEffect
                ]
            )
        )

    let boundarySurface (s : IFramebufferSignature) =
        boundarySurfaceCache.GetOrAdd(s, fun s ->
            r.PrepareEffect(
                s, [
                    //DefaultSurfaces.trafo |> toEffect
                    Path.Shader.boundaryVertex  |> toEffect
                    Path.Shader.boundary        |> toEffect
                ]
            )
        )

    do 
        r.OnDispose.Add(fun () ->
            boundarySurfaceCache.Values |> Seq.iter r.DeleteSurface
            surfaceCache.Values |> Seq.iter r.DeleteSurface
            pool.Dispose()
            ranges.Clear()
            cache.Clear()
        )

    let vertexBuffers =
        { new IAttributeProvider with
            member x.TryGetAttribute(sem) =
                match pool.TryGetBufferView sem with
                    | Some bufferView ->
                        //bufferView.Buffer.Level <- max bufferView.Buffer.Level 3000
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
    member x.BoundaryEffect = boundaryEffect
    member x.Surface s = surface s :> ISurface
    member x.BoundarySurface s = boundarySurface s :> ISurface
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
    static member PrepareGlyphs(r : IRuntime, f : Font, chars : seq<char>) =
        let cache = ShapeCache.GetOrCreateCache r

        for c in chars do
            cache.GetBufferRange (f.GetGlyph c) |> ignore
            
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

    let inline glyph (c : char) (f : Font) = f.GetGlyph c
    let inline kerning (l : char) (r : char) (f : Font) = f.GetKerning(l,r)

    let inline create (family : string) (style : FontStyle) = Font(family, style)


[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Glyph =
    let inline advance (g : Glyph) = g.Advance
    let inline path (g : Glyph) = g.Path
    let inline geometry (g : Glyph) = g.Geometry
    let inline char (g : Glyph) = g.Character




