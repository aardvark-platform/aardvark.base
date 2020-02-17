namespace Aardvark.Rendering.Text

open System
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Rendering

type Path = private { bounds : Box2d; outline : PathSegment[] }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Path =

    module Attributes = 
        let KLMKind = Symbol.Create "KLMKind"
        let ShapeTrafoR0 = Symbol.Create "ShapeTrafoR0"
        let ShapeTrafoR1 = Symbol.Create "ShapeTrafoR1"
        let PathColor = Symbol.Create "PathColor"
        let TrafoOffsetAndScale = Symbol.Create "PathTrafoOffsetAndScale"

    type KLMKindAttribute() = inherit FShade.SemanticAttribute(Attributes.KLMKind |> string)
    type ShapeTrafoR0Attribute() = inherit FShade.SemanticAttribute(Attributes.ShapeTrafoR0 |> string)
    type ShapeTrafoR1Attribute() = inherit FShade.SemanticAttribute(Attributes.ShapeTrafoR1 |> string)
    type PathColorAttribute() = inherit FShade.SemanticAttribute(Attributes.PathColor |> string)
    type TrafoOffsetAndScaleAttribute() = inherit FShade.SemanticAttribute(Attributes.TrafoOffsetAndScale |> string)
    type KindAttribute() = inherit FShade.SemanticAttribute("Kind")
    type KLMAttribute() = inherit FShade.SemanticAttribute("KLM")
    type DepthLayerAttribute() = inherit FShade.SemanticAttribute("DepthLayer")

    [<ReflectedDefinition>]
    module Shader = 
        open FShade

        type UniformScope with
            member x.FillGlyphs : bool = uniform?FillGlyphs
            member x.Antialias : bool = uniform?Antialias
            member x.BoundaryColor : V4d = uniform?BoundaryColor
            member x.DepthBias : float = uniform?DepthBias

        type Vertex =
            {
                [<Position>] p : V4d
                [<Interpolation(InterpolationMode.Sample); KLMKind>] klmKind : V4d
                [<ShapeTrafoR0>] tr0 : V4d
                [<ShapeTrafoR1>] tr1 : V4d
                [<PathColor>] color : V4d

                [<SamplePosition>] samplePos : V2d
                
                [<DepthLayer>] layer : float


                [<TrafoOffsetAndScale>] instanceTrafo : M34d
            }

        let eps = 0.00001
        [<Inline>]
        let keepsWinding (isOrtho : bool) (t : M44d) =
            if isOrtho then
                t.M00 > 0.0
            else
                let c = V3d(t.M03, t.M13, t.M23)
                let z = V3d(t.M02, t.M12, t.M22)
                Vec.dot c z < 0.0
                
        [<Inline>]
        let isOrtho (proj : M44d) = 
            abs proj.M30 < eps &&
            abs proj.M31 < eps &&
            abs proj.M32 < eps

        let pathVertex (v : Vertex) =
            vertex {
                let trafo = uniform.ModelViewTrafo

                let mutable p = V4d.Zero

                let flip = v.tr0.W < 0.0
                let pm = 
                    V2d(
                        Vec.dot v.tr0.XYZ (V3d(v.p.XY, 1.0)),
                        Vec.dot v.tr1.XYZ (V3d(v.p.XY, 1.0))
                    )

                if flip then
                    if keepsWinding (isOrtho uniform.ProjTrafo) trafo then
                        p <- trafo * V4d( pm.X, pm.Y, v.p.Z, v.p.W)
                    else
                        p <- trafo * V4d(-pm.X, pm.Y, v.p.Z, v.p.W)
                else
                    p <- trafo * V4d(pm.X, pm.Y, v.p.Z, v.p.W)
                    
                return { 
                    v with 
                        p = uniform.ProjTrafo * p
                        //kind = v.klmKind.W
                        layer = 0.0
                        //klm = v.klmKind.XYZ 
                        color = v.color
                    }
            }
            
        let pathVertexInstanced (v : Vertex) =
            vertex {
                let instanceTrafo = M44d.op_Explicit v.instanceTrafo //M44d.FromRows(v.instanceTrafo.R0, v.instanceTrafo.R1, v.instanceTrafo.R2, V4d.OOOI)
                let trafo = uniform.ModelViewTrafo * instanceTrafo

                let flip = v.tr0.W < 0.0
                let pm = 
                    V2d(
                        Vec.dot v.tr0.XYZ (V3d(v.p.XY, 1.0)),
                        Vec.dot v.tr1.XYZ (V3d(v.p.XY, 1.0))
                    )

                let mutable p = V4d.Zero

                if flip then
                    if keepsWinding (isOrtho uniform.ProjTrafo) trafo then
                        p <- trafo * V4d( pm.X, pm.Y, v.p.Z, v.p.W)
                    else
                        p <- trafo * V4d(-pm.X, pm.Y, v.p.Z, v.p.W)
                else
                    p <- trafo * V4d(pm.X, pm.Y, v.p.Z, v.p.W)
                
                return { 
                    v with 
                        p = uniform.ProjTrafo * p 
                        //kind = v.klmKind.W
                        layer = v.color.W
                        //klm = v.klmKind.XYZ
                        color = V4d(v.color.XYZ, 1.0)
                }
            }
            
        let pathVertexBillboard (v : Vertex) =
            vertex {
                let trafo = uniform.ModelViewTrafo

                let mvi = trafo.Transposed
                let right = mvi.C0.XYZ |> Vec.normalize
                let up = mvi.C1.XYZ |> Vec.normalize

                let pm = 
                    V2d(
                        Vec.dot v.tr0.XYZ (V3d(v.p.XY, 1.0)),
                        Vec.dot v.tr1.XYZ (V3d(v.p.XY, 1.0))
                    )

                let mutable p = V4d.Zero


                let pm = right * pm.X + up * pm.Y + V3d(0.0, 0.0, v.p.Z)

                p <- trafo * V4d(pm, 1.0)
                    
                return { 
                    v with 
                        p = uniform.ProjTrafo * p
                        layer = 0.0
                        color = v.color
                    }
            }
            
        let pathVertexInstancedBillboard (v : Vertex) =
            vertex {
                let instanceTrafo = M44d.op_Explicit v.instanceTrafo
                let trafo = uniform.ModelViewTrafo * instanceTrafo
                
                let mvi = trafo.Transposed
                let right = mvi.C0.XYZ |> Vec.normalize
                let up = mvi.C1.XYZ |> Vec.normalize


                
                let flip = v.tr0.W < 0.0
                let pm = 
                    V2d(
                        Vec.dot v.tr0.XYZ (V3d(v.p.XY, 1.0)),
                        Vec.dot v.tr1.XYZ (V3d(v.p.XY, 1.0))
                    )

                let p = trafo * V4d(right * pm.X + up * pm.Y + V3d(0.0, 0.0, v.p.Z), v.p.W)
                
                return { 
                    v with 
                        p = uniform.ProjTrafo * p 
                        layer = v.color.W
                        color = V4d(v.color.XYZ, 1.0)
                }
            }

        type Frag =
            {
                [<Color>] color : V4d
                [<Depth>] d : float
            }

        let pathFragment(v : Vertex) =
            fragment {
                let kind = v.klmKind.W + 0.001 * v.samplePos.X
   
                let mutable color = v.color

                if uniform.FillGlyphs then
                    if kind > 1.5 && kind < 3.5 then
                        // bezier2
                        let ci = v.klmKind.XYZ
                        let f = (ci.X * ci.X - ci.Y) * ci.Z
                        if f > 0.0 then
                            discard()
                        
                    elif kind > 3.5 && kind < 5.5 then
                        // arc
                        let ci = v.klmKind.XYZ
                        let f = ((ci.X * ci.X + ci.Y*ci.Y) - 1.0) * ci.Z
                    
                        if f > 0.0 then
                            discard()

                     elif kind > 5.5  then
                        let ci = v.klmKind.XYZ
                        let f = ci.X * ci.X * ci.X - ci.Y * ci.Z
                        if f > 0.0 then
                            discard()


                let sp = 0.5 * v.p.Z / v.p.W + 0.5
                let bias = 255.0 * v.layer * uniform.DepthBias
                return { color = color; d = sp - bias }
                    
            }
        
        let boundaryVertex (v : Vertex) =
            vertex {
                return { v with p = uniform.ModelViewProjTrafo * v.p }
            }

        let boundary (v : Vertex) =
            fragment {
                return uniform.BoundaryColor
            }


    let empty =
        { bounds = Box2d.Invalid; outline = [||] }

    /// create a path using a single segment
    let single (seg : PathSegment) =
        { bounds = PathSegment.bounds seg; outline = [| seg |] }

    /// creates a path using the given segments
    let ofSeq (segments : seq<PathSegment>) =
        let arr = Seq.toArray segments
        let bounds = arr |> Array.fold (fun l r -> Box2d.Union(l,PathSegment.bounds r)) Box2d.Invalid
        { bounds = bounds; outline = arr }

    /// creates a path using the given segments
    let ofList (segments : list<PathSegment>) =
        let arr = List.toArray segments
        let bounds = arr |> Array.fold (fun l r -> Box2d.Union(l,PathSegment.bounds r)) Box2d.Invalid
        { bounds = bounds; outline = arr }

    /// creates a path using the given segments
    let ofArray (segments : PathSegment[]) =
        let bounds = segments |> Array.fold (fun l r -> Box2d.Union(l,PathSegment.bounds r)) Box2d.Invalid
        { bounds = bounds; outline = segments }

    /// returns all path segments
    let toSeq (p : Path) =
        p.outline :> seq<_>

    /// returns all path segments
    let toList (p : Path) =
        p.outline |> Array.toList

    /// returns all path segments
    let toArray (p : Path) =
        p.outline |> Array.copy

    /// concatenates two paths
    let append (l : Path) (r : Path) =
        { bounds = Box2d.Union(l.bounds, r.bounds); outline = Array.append l.outline r.outline }

    /// concatenates a sequence paths
    let concat (l : seq<Path>) =
        let bounds = l |> Seq.fold (fun l r -> Box2d.Union(l, r.bounds)) Box2d.Invalid
        let arr = l |> Seq.collect toArray |> Seq.toArray
        { bounds = bounds; outline = arr }

    /// reverses the entrie path
    let reverse (p : Path) =
        { bounds = p.bounds; outline = p.outline |> Array.map PathSegment.reverse |> Array.rev }

    /// gets an axis-aligned bounding box for the path
    let bounds (p : Path) =
        p.outline |> Seq.map PathSegment.bounds |> Box2d

    /// gets the segment count for the path
    let count (p : Path) =
        p.outline.Length

    /// gets the i-th segment from the path
    let item (i : int) (p : Path) =
        p.outline.[i]

    /// applies the given transformation to all points used by the path
    let transform (f : V2d -> V2d) (p : Path) =
        p.outline |> Array.map (PathSegment.transform f) |> ofArray
        
    type PathBuilderState =
        {
            currentStart : Option<V2d>
            current : Option<V2d>
            segments : list<PathSegment>
        }

    type PathBuilder() =
        member x.Yield(()) =
            {
                currentStart = None
                current = None
                segments = []
            }

        [<CustomOperation("start")>]
        member x.Start(s : PathBuilderState, pt : V2d) = 
            { s with current = Some pt; currentStart = Some pt }
            
        [<CustomOperation("lineTo")>]
        member x.LineTo(s : PathBuilderState, p1 : V2d) = 
            match s.current with
                | Some p0 ->
                    match PathSegment.tryLine p0 p1 with
                        | Some seg -> 
                            { s with current = Some p1; segments = seg :: s.segments }
                        | None ->
                            s
                | None ->
                    failwith "cannot use lineTo without starting the path"

        [<CustomOperation("bezierTo")>]
        member x.BezierTo(s : PathBuilderState, pc : V2d, p1 : V2d) = 
            match s.current with
                | Some p0 ->
                    match PathSegment.tryBezier2 p0 pc p1 with
                        | Some seg -> 
                            { s with current = Some p1; segments = seg :: s.segments }
                        | None ->
                            s
                | None ->
                    failwith "cannot use lineTo without starting the path"
            
        [<CustomOperation("arc")>]
        member x.Arc(s : PathBuilderState, p1 : V2d, p2 : V2d) = 
            match s.current with
                | Some p0 ->
                    match PathSegment.tryArcSegment p0 p1 p2 with
                        | Some seg -> 
                            { s with current = Some p2; segments = seg :: s.segments }
                        | None ->
                            s
                | None ->
                    failwith "cannot use lineTo without starting the path"
                            
        [<CustomOperation("bezierTo3")>]
        member x.BezierTo3(s : PathBuilderState, pc0 : V2d, pc1 : V2d, p1 : V2d) = 
            match s.current with
                | Some p0 ->
                    match PathSegment.tryBezier3 p0 pc0 pc1 p1 with
                        | Some seg -> 
                            { s with current = Some p1; segments = seg :: s.segments }
                        | None ->
                            s
                | None ->
                    failwith "cannot use lineTo without starting the path"
                    
        [<CustomOperation("stop")>]
        member x.Stop(s : PathBuilderState) =
            { s with current = None; currentStart = None }
            
        [<CustomOperation("close")>]
        member x.CloseLine(s : PathBuilderState) =
            match s.current, s.currentStart with
                | Some current, Some start ->
                    let s = { s with current = None; currentStart = None }     
                    
                    match PathSegment.tryLine current start with
                        | Some seg -> { s with segments = seg :: s.segments }
                        | None -> s
                | _ ->
                    failwith "cannot close without starting the path"
      
        member x.Run(s : PathBuilderState) =
            ofList (List.rev s.segments)

    let build = PathBuilder()
    

    type private Triangle2dBound(p0 : V2d, p1 : V2d, p2 : V2d, b0 : bool, b1 : bool, b2 : bool) =
        member x.P0 = p0
        member x.P1 = p1
        member x.P2 = p2
        member x.B0 = b0
        member x.B1 = b1
        member x.B2 = b2

    type Triangle2d<'a>(p0 : V2d, c0 : 'a, p1 : V2d, c1 : 'a, p2 : V2d, c2 : 'a) =
        member x.P0 = p0
        member x.P1 = p1
        member x.P2 = p2
        member x.C0 = c0
        member x.C1 = c1
        member x.C2 = c2

    type Polygon2d<'a>(points : V2d[], coords : 'a[]) =
        member x.PointCount = points.Length
        member x.Points = points
        member x.Coords = coords

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module private LibTessInternal =
        open LibTessDotNet.Double

        module private ContourVertex =
            let ofV2d (v : V2d) =
                let mutable res = ContourVertex()
                res.Position <- Vec3(X = v.X, Y = v.Y)
                res

            let toV2d (v : ContourVertex) =
                V2d(v.Position.X, v.Position.Y)

        module private Contour =
            let closed (path : array<V3d>) (att : Option<array<'a>>) =
                let isClosed = path.[0] = path.[path.Length-1]
           
                let arr = Array.zeroCreate (path.Length + (if isClosed then 0 else 1))

                match att with
                    | Some att ->
                        for i in 0 .. path.Length - 1 do
                            let pt = path.[i]
                            let att = att.[i]
                            arr.[i] <- ContourVertex(Position = Vec3(X = pt.X, Y = pt.Y, Z = pt.Z), Data = att)
                    | None ->
                        for i in 0 .. path.Length - 1 do
                            let pt = path.[i]
                            arr.[i] <- ContourVertex(Position = Vec3(X = pt.X, Y = pt.Y, Z = pt.Z))
                        
                if not isClosed then
                    arr.[path.Length] <- arr.[0]

                arr

            let ofPolygon2d (p : Polygon2d) =
                let arr = Array.zeroCreate (p.PointCount + 1)
                for i in 0 .. p.PointCount - 1 do
                    arr.[i] <- ContourVertex.ofV2d p.[i]
                arr.[p.PointCount] <- arr.[0]
                arr

        let tessellateWithAttributes (interpolate : V3d -> array<float * 'a> -> 'a) (paths : seq<V3d[] * 'a[]>) =
            let tess = Tess()
            let mutable cnt = 0
            for (path, att) in paths do
                if path.Length > 0 then
                    cnt <- cnt + 1
                    tess.AddContour(Contour.closed path (Some att))

            if cnt > 0 then
                let combiner =
                    CombineCallback(fun p values weights ->
                        let pos = V3d(p.X, p.Y, p.Z)
                        let zip = Array.map2 (fun w v -> (w, unbox<'a> v)) weights values 
                        interpolate pos zip :> obj
                    )

                tess.Tessellate(WindingRule.Positive, ElementType.Polygons, 3, combiner)
                let index = tess.Elements
                let vertices = tess.Vertices |> Array.map (fun p -> V3d(p.Position.X, p.Position.Y, p.Position.Z))
                let attributes = tess.Vertices |> Array.map (fun p -> unbox<'a> p.Data)
                index, vertices, attributes
            else
                [||], [||], [||]

        let tessellate (evenOdd : bool) (paths : seq<V3d[]>) =
            let tess = Tess()
            let mutable cnt = 0
            for (path) in paths do
                if path.Length > 0 then
                    cnt <- cnt + 1
                    tess.AddContour(Contour.closed path None)
            if cnt > 0 then
                let rule = if evenOdd then WindingRule.EvenOdd else WindingRule.Positive
                tess.Tessellate(rule, ElementType.Polygons, 3)
                let index = tess.Elements
                let vertices = tess.Vertices |> Array.map (fun p -> V3d(p.Position.X, p.Position.Y, p.Position.Z))
                index, vertices
            else
                [||], [||]

        let tessellateN (n : int) (evenOdd : bool) (paths : seq<V3d[]>) =
            let tess = Tess()
            let mutable cnt = 0
            for (path) in paths do
                if path.Length > 0 then
                    cnt <- cnt + 1
                    tess.AddContour(Contour.closed path None)
            if cnt > 0 then
                let rule = if evenOdd then WindingRule.EvenOdd else WindingRule.Positive
                tess.Tessellate(rule, ElementType.Polygons, n)
                let index = tess.Elements
                let vertices = tess.Vertices |> Array.map (fun p -> V3d(p.Position.X, p.Position.Y, p.Position.Z))
                index, vertices
            else
                [||], [||]

    type LibTess private() =
        static member TessellateIndexed(polys : seq<Polygon2d>, evenOdd : bool) =
            let points = polys |> Seq.map (fun p -> p.Points |> Seq.toArray |> Array.map (fun v -> V3d(v, 0.0)))
            let index, pos = LibTessInternal.tessellate evenOdd points
            let pos = pos |> Array.map Vec.xy
            index, pos

        static member Tessellate(polys : seq<Polygon2d>, evenOdd : bool) =
            let points = polys |> Seq.map (fun p -> p.Points |> Seq.toArray |> Array.map (fun v -> V3d(v, 0.0)))
            let index, pos = LibTessInternal.tessellate evenOdd points

            let pos = pos |> Array.map Vec.xy

            let triangles = Array.zeroCreate (index.Length / 3)
            for ti in 0 .. triangles.Length-1 do
                let i0 = index.[3 * ti + 0]
                let i1 = index.[3 * ti + 1]
                let i2 = index.[3 * ti + 2]
                triangles.[ti] <- Triangle2d(pos.[i0], pos.[i1], pos.[i2])

            triangles

        static member TessellateN(polys : seq<Polygon2d>, evenOdd : bool, n : int) =
            let points = polys |> Seq.map (fun p -> p.Points |> Seq.toArray |> Array.map (fun v -> V3d(v, 0.0)))
            let index, pos = LibTessInternal.tessellateN n evenOdd points

            let pos = pos |> Array.map Vec.xy

            let result = Array.zeroCreate (index.Length / n)
            for ti in 0 .. result.Length-1 do
                let baseIndex = n * ti
                let points = 
                    [| 0 .. n - 1 |] |> Array.choose (fun pi ->
                        let vi = index.[baseIndex + pi]
                        if vi < 0 then
                            None
                        else
                            Some pos.[vi]
                    ) 
                result.[ti] <- Polygon2d(points)

            result

        static member TessellateEvenOdd(polys : seq<Polygon2d>) =
            LibTess.Tessellate(polys, true)

        static member Tessellate(polys : seq<Polygon2d>) =
            LibTess.Tessellate(polys, false)

        static member TessellateN(polys : seq<Polygon2d>, n : int) =
            LibTess.TessellateN(polys, false, n)

        static member Tessellate(polys : seq<Polygon2d<'a>>, add : 'a -> 'a -> 'a, mul : float -> 'a -> 'a, zero : 'a) =
            let paths = polys |> Seq.map (fun p -> (Array.map (fun (v : V2d) -> V3d(v, 0.0)) p.Points, p.Coords))

            let interpolate (p : V3d) (wvs : array<float * 'a>) =
                wvs |> Array.fold (fun s (w,v) -> add s (mul w v)) zero

            let index, pos, att = LibTessInternal.tessellateWithAttributes interpolate paths

            let pos = pos |> Array.map Vec.xy

            let triangles = Array.zeroCreate (index.Length / 3)
            for ti in 0 .. triangles.Length-1 do
                let i0 = index.[3 * ti + 0]
                let i1 = index.[3 * ti + 1]
                let i2 = index.[3 * ti + 2]
                triangles.[ti] <- Triangle2d<'a>(pos.[i0], att.[i0], pos.[i1], att.[i1], pos.[i2], att.[i2])

            triangles

        static member inline Tessellate(polys : seq<Polygon2d<'a>>, mul : float -> 'a -> 'a) =
            LibTess.Tessellate(polys, (+), mul, LanguagePrimitives.GenericZero)

//
//        let tessellate (paths : seq<Polygon2d>) =
//            let tess = Tess()
//            for path in paths do
//                tess.AddContour(Contour.ofPolygon2d path)
//
//            tess.Tessellate(WindingRule.Positive, ElementType.Polygons, 3)
//            let index = tess.Elements
//            let vertices = tess.Vertices |> Array.map ContourVertex.toV2d
//            Array.init tess.ElementCount (fun ti ->
//                Triangle2d(
//                    vertices.[index.[3 * ti + 0]],
//                    vertices.[index.[3 * ti + 1]],
//                    vertices.[index.[3 * ti + 2]]
//                )
//            )


    /// finds all closed subpaths for the given path
    let findClosedSubPaths (p : Path) =
        let rec traverse (components : list<Path>) (currentComponent : list<PathSegment>) (index : int) (current : V2d) (arr : PathSegment[]) =
            if index >= arr.Length then
                match currentComponent with
                    | [] -> components
                    | _ ->
                        let c = currentComponent |> List.rev |> ofList
                        c :: components
            else
                let s = arr.[index]
                let p0 = PathSegment.startPoint s
                let p1 = PathSegment.endPoint s

                if p0 = current then
                    traverse components (s :: currentComponent) (index + 1) p1 arr
                else
                    match currentComponent with
                        | [] -> traverse components [s] (index + 1) p1 arr
                        | _ ->
                            let c = currentComponent |> List.rev |> ofList
                            traverse (c :: components) [s] (index + 1) p1 arr

        traverse [] [] 0 V2d.NaN p.outline
            
    // calculates the (k,l,m) coordinates for a given bezier-segment as
    // shown by Blinn 2003: http://www.msr-waypoint.net/en-us/um/people/cloop/LoopBlinn05.pdf
    // returns the (k,l,m) triples for the four control-points
    let private texCoordsOld(p0 : V2d, p1 : V2d, p2 : V2d, p3 : V2d) =
        let p0 = V3d(p0, 1.0)
        let p1 = V3d(p1, 1.0)
        let p2 = V3d(p2, 1.0)
        let p3 = V3d(p3, 1.0)

        let a1 = Vec.dot p0 (Vec.cross p3 p2)
        let a2 = Vec.dot p1 (Vec.cross p0 p3)
        let a3 = Vec.dot p2 (Vec.cross p1 p0)

        let d1 =  a1 - 2.0 * a2 + 3.0 * a3
        let d2 = -a2 + 3.0 * a3
        let d3 =  3.0 * a3
        let discr = d1*d1*(3.0*d2*d2 - 4.0*d1*d3)
        
        let inline flip (v : V3d) = V3d(-v.X, -v.Y, v.Z)

        if Fun.IsTiny(discr, 1E-8) && Fun.IsTiny(d1, 1E-8) && not (Fun.IsTiny(d2, 1E-8)) then 
            // cusp

            let l = V2d(d3, 3.0*d2).Normalized
            let ls = l.X
            let lt = l.Y

            let w0 = V3d(ls, ls*ls*ls, 1.0)
            let w1 = V3d(ls - lt/3.0, ls*ls*(ls - lt), 1.0)
            let w2 = V3d(ls - 2.0*lt/3.0, ls*sqr(ls - lt), 1.0)
            let w3 = V3d(ls - lt, (ls-lt)**3.0, 1.0)
            Choice1Of2(6, w0, w1, w2, w3)

        elif discr >= 0.0 then   
            // serpentine
            let q = sqrt(9.0*d2*d2 - 12.0*d1*d3)
            let l = V2d(3.0*d2 - q, 6.0*d1).Normalized
            let m = V2d(3.0*d2 + q, 6.0*d1).Normalized

            let ls = l.X
            let lt = l.Y
            let ms = m.X
            let mt = m.Y

            let w0 = V3d(ls*ms, ls*ls*ls, ms*ms*ms)
            let w1 = V3d((3.0*ls*ms - ls*mt - lt*ms)/3.0, ls*ls*(ls - lt), ms*ms*(ms-mt))
            let w2 = V3d((lt*(mt-2.0*ms) + ls*(3.0*ms - 2.0*mt))/3.0, sqr (lt-ls) * ls, sqr (mt - ms) * ms)
            let w3 = V3d((lt-ls)*(mt-ms), -(lt-ls)**3.0, -(mt-ms)**3.0)
            
            if d1 < -1E-8 then Choice1Of2(7, flip w0, flip w1, flip w2, flip w3)
            else Choice1Of2(7, w0, w1, w2, w3)
        else
            // loop

            //V3d.NOO, V3d.NOO, V3d.NOO, V3d.NOO

            let m3 =
                M44d(
                    +1.0,  0.0,  0.0,  0.0,
                    -3.0,  3.0,  0.0,  0.0,
                    +3.0, -6.0,  3.0,  0.0,
                    -1.0,  3.0, -3.0,  1.0
                )
                
            let m3i =
                M44d(
                    1.0,  0.0,      0.0,     0.0,
                    1.0,  1.0/3.0,  0.0,     0.0,
                    1.0,  2.0/3.0,  1.0/3.0, 0.0,
                    1.0,  1.0,      1.0,     1.0
                ) 

            let b =
                M44d.FromRows(
                    V4d(p0.X, p0.Y, 0.0, 1.0),
                    V4d(p1.X, p1.Y, 0.0, 1.0),
                    V4d(p2.X, p2.Y, 0.0, 1.0),
                    V4d(p3.X, p3.Y, 0.0, 1.0)
                )

            let bla = m3 * b
            let p0 = bla.R0.XYW
            let p1 = bla.R1.XYW
            let p2 = bla.R2.XYW
            let p3 = bla.R3.XYW

            let d0 =  M33d.FromRows(p3, p2, p1).Det
            let d1 = -M33d.FromRows(p3, p2, p0).Det
            let d2 =  M33d.FromRows(p3, p1, p0).Det
            let d3 = -M33d.FromRows(p2, p1, p0).Det

            let delta1 = d0*d2 - d1*d1
            let delta2 = d1*d2 - d0*d3
            let delta3 = d1*d3 - d2*d2
            let discr = 4.0*delta1*delta3 - delta2*delta2


            let d = V2d(d2 + sqrt(4.0*d1*d3 - 3.0*d2*d2), 2.0*d1).Normalized
            let e = V2d(d2 - sqrt(4.0*d1*d3 - 3.0*d2*d2), 2.0*d1).Normalized

            let td = d.X
            let sd = d.Y
            let te = e.X
            let se = e.Y

            let a = td / sd
            let b = te / se

            let va = a >= 1E-3 && a <= 1.0 - 1E-3
            let vb = b >= 1E-3 && b <= 1.0 - 1E-3

            if va then Log.warn "split %A" a; Choice2Of2 a
            elif vb then Log.warn "split %A" b; Choice2Of2 b
            else
                let F =
                    M44d(
                        td*te,          td*td*te,                   td*te*te,                   1.0,
                        -se*td-sd*te,   -se*td*td-2.0*sd*te*td,     -sd*te*te-2.0*se*td*te,     0.0,
                        sd*se,          te*sd*sd+2.0*se*td*sd,      td*se*se+2.0*sd*te*se,      0.0,
                        0.0,            -sd*sd*se,                  -sd*se*se,                  0.0
                    )

                let w = m3i * F

                let shouldNotFlip = (d1 > 0.0 && w.M10 < 0.0) || (d1 < 0.0 && w.M10 > 0.0)

                if not shouldNotFlip then Choice1Of2(8, flip w.R0.XYZ, flip w.R1.XYZ, flip w.R2.XYZ, flip w.R3.XYZ)
                else Choice1Of2(8, w.R0.XYZ, w.R1.XYZ, w.R2.XYZ, w.R3.XYZ)

            
    // calculates the (k,l,m) coordinates for a given bezier-segment as
    // shown by Blinn 2003: http://www.msr-waypoint.net/en-us/um/people/cloop/LoopBlinn05.pdf
    // returns the (k,l,m) triples for the four control-points
    let private texCoords(p0 : V2d, p1 : V2d, p2 : V2d, p3 : V2d) =
        let m3 =
            M44d(
                +1.0,  0.0,  0.0,  0.0,
                -3.0,  3.0,  0.0,  0.0,
                +3.0, -6.0,  3.0,  0.0,
                -1.0,  3.0, -3.0,  1.0
            )
                
        let m3i =
            M44d(
                1.0,  0.0,      0.0,     0.0,
                1.0,  1.0/3.0,  0.0,     0.0,
                1.0,  2.0/3.0,  1.0/3.0, 0.0,
                1.0,  1.0,      1.0,     1.0
            ) 

        let b =
            M44d.FromRows(
                V4d(p0.X, p0.Y, 0.0, 1.0),
                V4d(p1.X, p1.Y, 0.0, 1.0),
                V4d(p2.X, p2.Y, 0.0, 1.0),
                V4d(p3.X, p3.Y, 0.0, 1.0)
            )

        let bla = m3 * b
        let p0 = bla.R0.XYW
        let p1 = bla.R1.XYW
        let p2 = bla.R2.XYW
        let p3 = bla.R3.XYW

        let d0 =  M33d.FromRows(p3, p2, p1).Det
        let d1 = -M33d.FromRows(p3, p2, p0).Det
        let d2 =  M33d.FromRows(p3, p1, p0).Det
        let d3 = -M33d.FromRows(p2, p1, p0).Det

        let delta1 = d0*d2 - d1*d1
        let delta2 = d1*d2 - d0*d3
        let delta3 = d1*d3 - d2*d2
        let discr = 4.0*delta1*delta3 - delta2*delta2
        
        let inline flip (v : V3d) = V3d(-v.X, -v.Y, v.Z)

        let qs2 = 3.0*d2*d2 - 4.0*d1*d3

        if qs2 >= 0.0 && not (Fun.IsTiny(d1, 1E-8)) then   
            // serpentine / Cusp with inflection at infinity
            let qs = sqrt(qs2 / 3.0)
            let l = V2d(d2 + qs, 2.0*d1).Normalized
            let m = V2d(d2 - qs, 2.0*d1).Normalized

            let tl = l.X
            let sl = l.Y
            let tm = m.X
            let sm = m.Y

            let F =
                M44d(
                    tl*tm,          tl*tl*tl,                   tm*tm*tm,                   1.0,
                    -sm*tl-sl*tm,   -3.0*sl*tl*tl,              -3.0*sm*tm*tm,              0.0,
                    sl*sm,          3.0*sl*sl*tl,               3.0*sm*sm*tm,               0.0,
                    0.0,            -sl*sl*sl,                  -sm*sm*sm,                  0.0
                )

            let shouldFlip = d1 > 0.0 // TODO
            let w = m3i * F
            if shouldFlip then Choice1Of2(7, flip w.R0.XYZ, flip w.R1.XYZ, flip w.R2.XYZ, flip w.R3.XYZ)
            else Choice1Of2(7, w.R0.XYZ, w.R1.XYZ, w.R2.XYZ, w.R3.XYZ)

        elif qs2 < 0.0 && not (Fun.IsTiny(d1, 1E-8)) then 
            // loop
            let ql = sqrt(-qs2)
            let d = V2d(d2 + ql, 2.0*d1).Normalized
            let e = V2d(d2 - ql, 2.0*d1).Normalized

            let td = d.X
            let sd = d.Y
            let te = e.X
            let se = e.Y

            let a = td / sd
            let b = te / se

            let va = a >= 1E-3 && a <= 1.0 - 1E-3
            let vb = b >= 1E-3 && b <= 1.0 - 1E-3

            if va then 
                Log.warn "split %A" a
                Choice2Of2 a
            elif vb then 
                Log.warn "split %A" b
                Choice2Of2 b
            else
                let F =
                    M44d(
                        td*te,          td*td*te,                   td*te*te,                   1.0,
                        -se*td-sd*te,   -se*td*td-2.0*sd*te*td,     -sd*te*te-2.0*se*td*te,     0.0,
                        sd*se,          te*sd*sd+2.0*se*td*sd,      td*se*se+2.0*sd*te*se,      0.0,
                        0.0,            -sd*sd*se,                  -sd*se*se,                  0.0
                    )

                let w = m3i * F

                let shouldNotFlip = (d1 > 0.0 && w.M10 < 0.0) || (d1 < 0.0 && w.M10 > 0.0)

                if not shouldNotFlip then Choice1Of2(8, flip w.R0.XYZ, flip w.R1.XYZ, flip w.R2.XYZ, flip w.R3.XYZ)
                else Choice1Of2(8, w.R0.XYZ, w.R1.XYZ, w.R2.XYZ, w.R3.XYZ)
        
        elif Fun.IsTiny(d1, 1E-8) && not (Fun.IsTiny(d1, 1E-8)) then
            // Cusp with cusp at infinity


            let l = V2d(d3, 3.0*d2).Normalized
            let tl = l.X
            let sl = l.Y

            let F =
                M44d(
                    tl,         tl*tl*tl,       1.0, 1.0,
                    -sl,        -3.0*sl*tl*tl,  0.0, 0.0,
                    0.0,        3.0*sl*sl*tl,   0.0, 0.0,
                    0.0,        -sl*sl*sl,      0.0, 0.0
                ) 
            
            let w = m3i * F
            Choice1Of2(6, w.R0.XYZ, w.R1.XYZ, w.R2.XYZ, w.R3.XYZ)
        
        elif Fun.IsTiny(d1, 1E-8) && Fun.IsTiny(d2, 1E-8) && not (Fun.IsTiny(d3, 1E-8)) then
            // quadratic
            failwith "quadratic"

        else
            failwith "line or point"

    /// creates a geometry using the !!closed!! path which contains the left-hand-side of
    /// the outline.
    /// The returned geometry contains Positions and a 4-dimensional vector (KLMKind) describing the
    /// (k,l,m) coordinates for boundary triangles in its xyz components and
    /// the kind of the triangle (inner = 0, boundary = 1) in its w component
    let toGeometry (p : Path) =
        let bounds = bounds p
        let innerPoints = List<List<V2d>>()
        let mutable current = V2d.NaN
        
        let boundaryTriangles = List<V2d>()
        let boundaryCoords = List<V4d>()

        let start (p : V2d) =
            if not (Fun.ApproximateEquals(current, p, 1E-8)) then
                innerPoints.Add(List())
                current <- V2d.NaN
                
        let add (p : V2d) =
            if not (Fun.ApproximateEquals(current, p, 1E-8)) then 
                innerPoints.[innerPoints.Count-1].Add p
                current <- p
                
        let overlap (q0 : Polygon2d) (q1 : Polygon2d) =
            q0.Intersects q1
            //q0.ClippedBy(q1, epsilon) |> Seq.isEmpty |> not
            //if q0.IsFullyContainedInside(q1) then true
            //elif q1.IsFullyContainedInside(q0) then true
            //else
            //    let realIntersections =
            //        seq {
            //            for u in q0.EdgeLines do
            //                let mutable t = nan
            //                let r = u.Ray2d
            //                for v in q1.EdgeLines do
            //                    match r.Intersects(v, &t) with
            //                        | true when t > 0.001 && t < 0.999 -> yield 1
            //                        | _ -> ()
            //        }
            //    realIntersections |> Seq.isEmpty |> not
            
        let components = System.Collections.Generic.List<PathSegment>(p.outline)

        let toPolygon (l : PathSegment) =
            match l with
                | Arc(p0, p1, a0, da, e) -> 
                    let steps = abs da / Constant.PiHalf |> round |> int |> max 1

                    let res = System.Collections.Generic.List()
                    let step = da / float steps
                    let mutable a = a0
                    res.Add p0
                    for i in 1 .. steps do
                        let n = a + step
                        let np = if i = steps then p1 else e.GetPoint n
                        let c = e.GetControlPoint(a, n)
                        res.Add c
                        res.Add np
                        a <- n
                    Polygon2d (Seq.toArray res) |> Some
                | Bezier2(p0, p1, p2) -> Polygon2d [| p0; p1; p2 |] |> Some
                | Bezier3(p0, p1, p2, p3) -> Polygon2d [| p0; p1; p2; p3 |] |> Some
                | _ -> None

        let overlapping (l : PathSegment) (r : PathSegment) =
            let lpoly = toPolygon l
            let rpoly = toPolygon r
            match lpoly, rpoly with
                | Some l, Some r -> overlap l r
                | _ -> false

        let subdivide (segment : PathSegment) = 
            let l, r = PathSegment.split 0.5 segment
            l.Value, r.Value

        let mutable i = 0
        while i < components.Count do
            match toPolygon components.[i] with
                | Some pi ->
                    let mutable pi = pi
                    let mutable j = i + 1
                    while j < components.Count do
                        match toPolygon components.[j] with
                            | Some pj when overlap pi pj ->
                                let pi0, pi1 = subdivide components.[i]
                                let pj0, pj1 = subdivide components.[j]

                                //pi <- toPolygon pi0 |> Option.get
                                components.[i] <- pi0
                                components.Insert(i+1, pi1)

                                components.[j+1] <- pj0
                                components.Insert(j+2, pj1)
                                
                                match toPolygon pi0 with
                                    | Some p -> 
                                        pi <- p
                                        j <- j + 1
                                    | None -> 
                                        j <- components.Count

                            | _ ->
                                j <- j + 1

                | _ ->
                    ()
                    
            i <- i + 1



        let arcCoords (p0 : V2d) (p1 : V2d) (p2 : V2d) (ellipse : Ellipse2d) =
            let uv2World = M33d.FromCols(V3d(ellipse.Axis0, 0.0), V3d(ellipse.Axis1, 0.0), V3d(ellipse.Center, 1.0))
            let world2UV = uv2World.Inverse

            let uv0 = world2UV.TransformPos p0
            let uv1 = world2UV.TransformPos p1
            let uv2 = world2UV.TransformPos p2

            (uv0, uv1, uv2)
            
            
        let rec run (l : list<PathSegment>) =
            match l with
                | Line(p0, p1) :: rest ->
                    start p0
                    add p0
                    add p1
                    run rest

                | (Arc(p0, p1, a0, da, ellipse) as a) :: rest ->
                    let steps = abs da / Constant.PiHalf |> round |> int |> max 1
                    start p0
                    add p0

                    let step = da / float steps
                    let mutable a = a0
                    let mutable q0 = p0
                    for i in 1 .. steps do
                        let n = a + step
                        let q1 = if i = steps then p1 else ellipse.GetPoint n
                        let c = ellipse.GetControlPoint(a, n)
                        let c0, c1, c2 = arcCoords q0 c q1 ellipse
                            
                        
                        let controlInside = 
                            c.PosLeftOfLineValue(q0, q1) < 0.0

                        if controlInside then
                            add c
                            boundaryCoords.AddRange [V4d(c0.X, c0.Y,-1.0, 4.0); V4d(c1.X, c1.Y,-1.0, 4.0); V4d(c2.X, c2.Y,-1.0,4.0)]
                        else 
                            boundaryCoords.AddRange [V4d(c0.X, c0.Y,1.0, 5.0); V4d(c1.X, c1.Y,1.0, 5.0); V4d(c2.X, c2.Y,1.0,5.0)]

                        add q1
                        boundaryTriangles.AddRange [q0; c; q1]

                        a <- n
                        q0 <- q1
                            
                    add p1
                    current <- p1
                    run rest


                | Bezier2(p0, p1, p2) :: rest ->
                    let q = Polygon2d(p0, p1, p2)
                    let p1Inside = p1.PosLeftOfLineValue(p0, p2) < 0.0
                    
                    start p0
                    add p0
                        

                    if p1Inside then 
                        add p1

                    boundaryTriangles.AddRange [p0; p1; p2]
                    if p1Inside then
                        boundaryCoords.AddRange [V4d(0,0,-1,2); V4d(-0.5, 0.0,-1.0, 2.0); V4d(-1,1,-1,2)]
                    else
                        boundaryCoords.AddRange [V4d(0,0,1,3); V4d(0.5, 0.0, 1.0,3.0); V4d(1,1,1,3)]

                    add p2

                    current <- p2
                    run rest

                | (Bezier3(p0, p1, p2, p3) as s) :: rest ->
                    //match PathSegment.inflectionParameters s with
                    //| [t0] when t0 >= 1E-8 && t0 <= 1.0 - 1E-8 ->
                    //    let l, r = PathSegment.split t0 s

                    //    match l, r with
                    //    | Some l, Some r ->
                    //        run (l :: r :: rest)
                    //    | Some s, None | None, Some s ->
                    //        run (s :: rest)
                    //    | None, None ->
                    //        run rest
                    //| _ -> 

                        match texCoords(p0, p1, p2, p3) with
                        | Choice1Of2(kind, w0, w1, w2, w3) ->
                            
                            let points  = [|p0;p1;p2;p3|]
                            let weights = [|w0;w1;w2;w3|]
                            let hull = Polygon2d(points).ComputeConvexHullIndexPolygon().Indices |> Seq.toArray 

                            let wnan = weights |> Array.exists (fun w -> w.AnyNaN || w.AnyInfinity)
                            if wnan then Log.warn "asdasda"
                            let kind = float kind

                            if hull.Length > 2 then
                                start p0
                                add p0

                                let p1Inside = Array.exists (fun i -> i = 1) hull && p1.PosLeftOfLineValue(p0, p3) <= 0.0
                                let p2Inside = Array.exists (fun i -> i = 2) hull && p2.PosLeftOfLineValue(p0, p3) <= 0.0
                                if p1Inside then add p1 
                                if p2Inside then add p2

                                let ws = hull |> Array.map (fun i -> weights.[i])
                                let ps = hull |> Array.map (fun i -> points.[i])

                                if hull.Length = 3 then
                                    //let m = Array.init 4 id |> Array.find (fun v -> not (Array.contains v hull))

                                    //boundaryTriangles.AddRange [ps.[0]; ps.[1]; points.[m]]
                                    //boundaryCoords.AddRange [V4d(ws.[0], kind); V4d(ws.[1], kind); V4d(weights.[m], kind)]
                                    
                                    //boundaryTriangles.AddRange [ps.[1]; ps.[2]; points.[m]]
                                    //boundaryCoords.AddRange [V4d(ws.[1], kind); V4d(ws.[2], kind); V4d(weights.[m], kind)]
                                    
                                    //boundaryTriangles.AddRange [ps.[2]; ps.[0]; points.[m]]
                                    //boundaryCoords.AddRange [V4d(ws.[2], kind); V4d(ws.[0], kind); V4d(weights.[m], kind)]
                                    boundaryTriangles.AddRange [ps.[0]; ps.[1]; ps.[2]]
                                    boundaryCoords.AddRange [V4d(ws.[0], kind); V4d(ws.[1], kind); V4d(ws.[2], kind)]

                                elif hull.Length = 4 then

                                    //let o = V2d(0.0, 10.0)
                                    //boundaryTriangles.AddRange [o+ps.[0]; o+ps.[1]; o+ps.[2]]
                                    //boundaryCoords.AddRange [V4d(ws.[0], kind); V4d(ws.[1], kind); V4d(ws.[2], kind)]
                                    //boundaryTriangles.AddRange [o+ps.[0]; o+ps.[2]; o+ps.[3]]
                                    //boundaryCoords.AddRange [V4d(ws.[0], kind); V4d(ws.[2], kind); V4d(ws.[3], kind)]

                                    boundaryTriangles.AddRange [ps.[0]; ps.[1]; ps.[3]]
                                    boundaryCoords.AddRange [V4d(ws.[0], kind); V4d(ws.[1], kind); V4d(ws.[3], kind)]
                                    boundaryTriangles.AddRange [ps.[3]; ps.[1]; ps.[2]]
                                    boundaryCoords.AddRange [V4d(ws.[3], kind); V4d(ws.[1], kind); V4d(ws.[2], kind)]

                                add p3

                            current <- p3
                            run rest

                        | Choice2Of2 t ->
                            let l, r = PathSegment.split t s

                            let rest =
                                match r with
                                | Some r -> r :: rest
                                | None -> rest

                            let rest =
                                match l with
                                | Some l -> l :: rest
                                | None -> rest
                                
                            run rest

                | [] -> ()

        let components = CSharpList.toList components
        //let components = 
        //    CSharpList.toList components |> List.collect (fun s ->
        //        match s with
        //            | Arc _ -> 
        //                let (b0, b1) = subdivide s
        //                let (a0, a1) = subdivide b0
        //                let (a2, a3) = subdivide b1
        //                [a0; a1; a2; a3]
        //            | _ -> 
        //                [s]
        //    )

        run components

        // merge the interior polygons (respecting holes)
        let innerPoints = 
            innerPoints |> Seq.map (fun l -> 
                l |> CSharpList.toArray |> Array.take (l.Count-1) |> Polygon2d
            ) |> Seq.toArray

        let index, positions = LibTess.TessellateIndexed(innerPoints, true)

        let trianglePositions = List<V3d>(positions.Length * 2)
        let triangleCoords = List<V4d>(positions.Length * 2)

        let rec appendTriangle (i0 : int) (i1 : int) (i2 : int) (p0 : V2d) (p1 : V2d) (p2 : V2d) =
            let kind = 0
            trianglePositions.AddRange [ V3d(p0, 0.0); V3d(p1, 0.0); V3d(p2, 0.0) ]
            triangleCoords.AddRange [ V4d(1,0,0,0); V4d(0,1,0,0); V4d(0,0,1,0) ]

        for ti in 0 .. index.Length/3 - 1 do
            let i0 = index.[3 * ti + 0]
            let i1 = index.[3 * ti + 1]
            let i2 = index.[3 * ti + 2]

            let p0 = positions.[i0]
            let p1 = positions.[i1]
            let p2 = positions.[i2]
            appendTriangle i0 i1 i2 p0 p1 p2


                  
        // union the interior with the bounary triangles
        let boundaryTriangles = boundaryTriangles |> Seq.map (fun v -> V3d(v.X, v.Y, 0.0)) |> Seq.toArray
        let boundaryCoords = boundaryCoords |> CSharpList.toArray
        let pos = Array.append (CSharpList.toArray trianglePositions) boundaryTriangles
        let tex = Array.append (CSharpList.toArray triangleCoords) boundaryCoords

        // use the merged vertex-data for creating the final geometry
        IndexedGeometry(
            Mode = IndexedGeometryMode.TriangleList,
            IndexedAttributes =
                SymDict.ofList [
                    DefaultSemantic.Positions,  pos |> Array.map (V3f.op_Explicit) :> Array
                    Attributes.KLMKind,         tex |> Array.map (V4f.op_Explicit) :> Array
                ]
        )
