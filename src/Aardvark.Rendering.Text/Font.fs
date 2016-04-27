namespace Aardvark.Rendering.Text

open System
open System.Collections.Concurrent
open Aardvark.Base
open Aardvark.Base.Rendering
open SharpFont

type FontStyle =
    | Regular = 0
    | Bold = 1
    | Italic = 2

type Glyph internal(path : Path, glyph : SharpFont.Glyph) =
    let mutable geometry = None

    member x.Path = path
    member x.Size = V2d(glyph.Width, glyph.Height)
    member x.Advance = glyph.HorizontalMetrics.Advance |> float

    member x.Geometry =
        match geometry with
            | Some g -> g
            | None ->
                let g = Path.toGeometry path
                geometry <- Some g
                g

type Font private(face : SharpFont.FontFace) =
    let kerningTable =
        if isNull face.kernTable then 
            Dictionary.empty
        else
            let charTable = face.charMap.table |> Dictionary.toSeq |> Seq.map (fun (a,b) -> (b,char a.value)) |> Dictionary.ofSeq

            Dictionary.ofList [
                for (k,v) in Dictionary.toSeq face.kernTable.table do
                    let l = charTable.[(k >>> 16) &&& 0xFFFFu |> int]
                    let r = charTable.[k &&& 0xFFFFu |> int]

                    yield (l,r), (float v / float face.unitsPerEm)
            ]

    let lineHeight = float face.lineHeight / float face.unitsPerEm

    let getPath (g : SharpFont.Glyph) =
        
        let points = g.Points |> Array.map (fun p -> V2d(p.P.X, p.P.Y))
        let pointTypes = g.Points |> Array.map (fun p -> p.Type)
        let contours = g.Contours


        let rec buildPath (startPoint : V2d) (startType : PointType) (first : int) (last : int) (current : list<V2d>) =
            if first > last then
                match current with
                    | [p] ->
                        
                        if V2d.ApproxEqual(p, startPoint) then []
                        else    
                            match startType with
                                | PointType.OnCurve -> [PathSegment.line p startPoint]
                                | _ -> []

                    | [p0; p1] ->
                        match startType with
                            | PointType.OnCurve ->
                                if V2d.ApproxEqual(p0, p1) && V2d.ApproxEqual(p1, startPoint) then 
                                    []
                                else
                                    [PathSegment.bezier2 p0 p1 startPoint]
                            | _ ->
                                let p2 = 0.5 * (p1 + startPoint)
                                if V2d.ApproxEqual(p0, p1) && V2d.ApproxEqual(p1, p2) then 
                                    []
                                else
                                    [PathSegment.bezier2 p0 p1 p2]

                    | _ ->
                        []
            else
                let p = points.[first]
                let pt = pointTypes.[first]

                match current, pt with
                    | [], PointType.OnCurve -> 
                        buildPath p pt (first + 1) last [p]

                    | [], PointType.Quadratic ->
                        let lp = points.[last]
                        match pointTypes.[last] with
                            | PointType.OnCurve -> buildPath lp pt first last [lp]
                            | PointType.Quadratic -> 
                                let sp = 0.5 * (p + lp)
                                buildPath sp pt first last [sp]
                            | _ ->  failwithf "[Font] paths cannot start with cubic splines"

                    | [], _ -> 
                        failwithf "[Font] paths cannot start with cubic splines"


                    | [p0], PointType.OnCurve ->
                        if V2d.ApproxEqual(p0, p) then
                            buildPath startPoint startType (first + 1) last [p]
                        else
                            let seg = PathSegment.line p0 p
                            seg :: buildPath startPoint startType (first + 1) last [p]

                    | [p0; p1], PointType.OnCurve ->
                        if V2d.ApproxEqual(p0, p1) && V2d.ApproxEqual(p1, p) then 
                            buildPath startPoint startType (first + 1) last [p]
                        else
                            let seg = PathSegment.bezier2 p0 p1 p
                            seg :: buildPath startPoint startType (first + 1) last [p]

                    | [p0; p1; p2], PointType.OnCurve ->
                        let seg = PathSegment.bezier3 p0 p1 p2 p
                        seg :: buildPath startPoint startType (first + 1) last [p]




                    | [p0; p1], PointType.Quadratic ->
                        let p2 = 0.5 * (p1 + p)
                        if V2d.ApproxEqual(p0, p1) && V2d.ApproxEqual(p1, p2) then 
                            buildPath startPoint startType (first + 1) last [p2; p]
                        else
                            let seg = PathSegment.bezier2 p0 p1 p2
                            seg :: buildPath startPoint startType (first + 1) last [p2; p]

                    | [p0;p1;p2], PointType.Cubic ->
                        let seg = PathSegment.bezier3 p0 p1 p2 p
                        seg :: buildPath startPoint startType (first + 1) last [p]
                         
                    | current, _ ->
                        buildPath startPoint startType (first + 1) last (List.append current [p])


        let path = 
            Path.ofList [
                let mutable first = 0
                for last in contours do
                    
                    let segments = buildPath V2d.Zero PointType.OnCurve first last [] |> List.map PathSegment.reverse |> List.rev
                    yield! segments

                    first <- last + 1
            ]

        path

    let glyphCache = ConcurrentDictionary<char, Glyph>()

    let get (c : char) =
        glyphCache.GetOrAdd(c, fun c ->
            let glyph = face.GetGlyph(CodePoint(c), 1.0f)
            
            let path = getPath glyph
            Glyph(path, glyph)
        )

    let cache = ConcurrentDictionary<IRuntime, FontCache>()

    member x.GetOrCreateCache(r : IRuntime) =
        cache.GetOrAdd(r, fun r ->
            new FontCache(r, x)
        )

    member x.Family = face.Family
    member x.IsMonospaced = face.IsFixedWidth
    member x.LineHeight = lineHeight
    member x.Style = unbox<FontStyle> (int face.Style)
    member x.Spacing = 1.0

    member x.GetGlyph(c : char) =
        get c

    member x.GetKerning(l : char, r : char) =
        match kerningTable.TryGetValue ((l,r)) with
            | (true, v) -> v
            | _ -> 0.0

    new(family : string, style : FontStyle) = 
        new Font(FontCollection.SystemFonts.Load(family, FontWeight.Normal, FontStretch.Normal, unbox (int style)))

    new(family : string) = 
        new Font(FontCollection.SystemFonts.Load(family))

and FontCache(r : IRuntime, f : Font) =
    let pool = Aardvark.Base.Rendering.GeometryPool.createAsync r
    let ranges = ConcurrentDictionary<Glyph, Range1i>()

    let types =
        Map.ofList [
            DefaultSemantic.Positions, typeof<V3f>
            Path.Attributes.KLMKind, typeof<V4f>
        ]

    let vertexBuffers =
        { new IAttributeProvider with
            member x.TryGetAttribute(sem) =
                match Map.tryFind sem types with    
                    | Some t -> BufferView(pool.GetBuffer sem, t) |> Some
                    | _ -> None

            member x.All = Seq.empty
            member x.Dispose() = ()
        }

    member x.VertexBuffers = vertexBuffers

    member x.GetBufferRange(glyph : Glyph) =
        ranges.GetOrAdd(glyph, fun glyph ->
            let range = pool.Add(glyph.Geometry)
            range
        )

    member x.Dispose() =
        //pool.Dispose()
        ranges.Clear()



[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Font =
    let inline glyph (c : char) (f : Font) = f.GetGlyph c
    let inline kerning (l : char) (r : char) (f : Font) = f.GetKerning(l,r)
    let inline lineHeight (f : Font) = f.LineHeight
    let inline isMonospaced (f : Font) = f.IsMonospaced

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Glyph =
    let inline advance (g : Glyph) = g.Advance
    let inline size (g : Glyph) = g.Size
    let inline path (g : Glyph) = g.Path
    let inline geometry (g : Glyph) = g.Geometry




type TextAlignment =
    | Left = 0
    | Right = 1
    | Center = 2

type TextWrapMode =
    | None = 0
    | WrapChar = 1
    | WrapWord = 2

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Text =
    open System.Text.RegularExpressions
    open System.Collections.Generic

    let lineBreak = Regex @"\r?\n|\r"

    let lines (str : string) =
        
        lineBreak.Split(str)


    let layout (font : Font) (align : TextAlignment) (bounds : Box2d) (text : string) =
        [|
            let chars = List<float * Glyph>()

            let mutable cy = 0.0
            let allLines = lines text

            for l in allLines do
                let mutable cx = 0.0
                let mutable last = '\n'
                chars.Clear()

                for c in l do
                    let kerning = font.GetKerning(last, c)
                    cx <- cx + kerning

                    match c with
                        | ' ' -> cx <- cx + font.Spacing
                        | '\t' -> cx <- cx + 4.0 + font.Spacing
                        | c ->
                            let g = font |> Font.glyph c
                            chars.Add(cx, g)
                            cx <- cx + g.Advance

                    last <- c

                let shift = 
                    match align with
                        | TextAlignment.Left -> 0.0
                        | TextAlignment.Right -> bounds.SizeX - cx
                        | TextAlignment.Center -> (bounds.SizeX - cx) / 2.0
                        | _ -> failwithf "[Text] bad align: %A" align

                let y = cy
                yield! chars |> Seq.map (fun (x,g) -> 
                    let pos = bounds.Min + V2d(shift + x,y)
                    pos, g
                )

                cy <- cy - font.LineHeight

        |]






module FontTest =
    open System.IO
    open SharpFont




    let run() =
        
        let font = Font "Times New Roman"


        let test = Text.layout font TextAlignment.Left (Box2d(0.0, 0.0, 10000.0, 10000.0)) "hi there!"

        printfn "%A" test

        ()