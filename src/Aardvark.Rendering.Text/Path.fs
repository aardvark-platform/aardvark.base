namespace Aardvark.Rendering.Text

open System
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Rendering

[<RequireQualifiedAccess>]
type WindingRule =
    | NonZero
    | Positive
    | Negative
    | EvenOdd
    | AbsGreaterEqualTwo

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
                [<Depth>] depth : float
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
                else
                    if kind > 1.5 && kind < 3.5 then
                        color <- V4d.IOOI
                    elif kind > 3.5 && kind < 5.5 then
                        color <- V4d.OIOI
                    elif kind > 5.5  then
                        color <- V4d.OOII


                let sp = 0.5 * v.p.Z / v.p.W + 0.5
                let bias = 255.0 * v.layer * uniform.DepthBias
                return { color = color; depth = sp - bias }
                    
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
        let bounds = arr |> Array.fold (fun l r -> Box.Union(l,PathSegment.bounds r)) Box2d.Invalid
        { bounds = bounds; outline = arr }

    /// creates a path using the given segments
    let ofList (segments : list<PathSegment>) =
        let arr = List.toArray segments
        let bounds = arr |> Array.fold (fun l r -> Box.Union(l,PathSegment.bounds r)) Box2d.Invalid
        { bounds = bounds; outline = arr }

    /// creates a path using the given segments
    let ofArray (segments : PathSegment[]) =
        let bounds = segments |> Array.fold (fun l r -> Box.Union(l,PathSegment.bounds r)) Box2d.Invalid
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
        { bounds = Box.Union(l.bounds, r.bounds); outline = Array.append l.outline r.outline }

    /// concatenates a sequence paths
    let concat (l : seq<Path>) =
        let bounds = l |> Seq.fold (fun l r -> Box.Union(l, r.bounds)) Box2d.Invalid
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
   
    /// creates a geometry using the !!closed!! path which contains the left-hand-side of
    /// the outline.
    /// The returned geometry contains Positions and a 4-dimensional vector (KLMKind) describing the
    /// (k,l,m) coordinates for boundary triangles in its xyz components and
    /// the kind of the triangle (inner = 0, boundary = 1) in its w component
    let toGeometry (rule : WindingRule) (p : Path) =
        let rule =
            match rule with
            | WindingRule.Positive -> LibTessDotNet.Double.WindingRule.Positive
            | WindingRule.Negative -> LibTessDotNet.Double.WindingRule.Negative
            | WindingRule.NonZero -> LibTessDotNet.Double.WindingRule.NonZero
            | WindingRule.EvenOdd -> LibTessDotNet.Double.WindingRule.EvenOdd
            | WindingRule.AbsGreaterEqualTwo -> LibTessDotNet.Double.WindingRule.AbsGeqTwo

        Tessellator.toGeometry rule p.bounds p.outline
       
