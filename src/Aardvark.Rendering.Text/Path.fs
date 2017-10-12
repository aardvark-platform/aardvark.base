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
        let PathOffsetAndScale = Symbol.Create "PathOffsetAndScale"
        let PathColor = Symbol.Create "PathColor"

    type KLMKindAttribute() = inherit FShade.SemanticAttribute(Attributes.KLMKind |> string)
    type PathOffsetAndScaleAttribute() = inherit FShade.SemanticAttribute(Attributes.PathOffsetAndScale |> string)
    type PathColorAttribute() = inherit FShade.SemanticAttribute(Attributes.PathColor |> string)

    [<ReflectedDefinition>]
    module Shader = 
        open FShade

        type UniformScope with
            member x.FillGlyphs : bool = uniform?FillGlyphs
            member x.Antialias : bool = uniform?Antialias
            member x.BoundaryColor : V4d = uniform?BoundaryColor

        type Vertex =
            {
                [<Position>] p : V4d
                [<Interpolation(InterpolationMode.Sample); KLMKind>] klm : V4d
                [<PathOffsetAndScale>] offset : V4d
                [<PathColor>] color : V4d
                [<VertexId>] vertexId : int
                [<SamplePosition>] samplePos : V2d
                [<SampleId>] sampleId : int
                [<SampleMask>] sampleMask : Arr<1 N, int>
            }

        type Fragment =
            {
                [<Color>] color : V4d
                [<SampleMask>] sampleMask : Arr<1 N, int>
            }

        let pathVertex (v : Vertex) =
            vertex {
                let pi = v.offset.XY + v.p.XY * v.offset.ZW
                let p = V4d(pi.X, pi.Y, v.p.Z, v.p.W)

                return { v with p = p }
            }

        let pathTrafo (v : Vertex) =
            vertex {
                let p = uniform.ModelViewProjTrafo * v.p
                return { v with p = p }
            }

        let samplePattern =
            Array.map (fun v -> v / 16.0) [|
                V2d(1,1);   V2d(-1,-3); V2d(-3,2);  V2d(4,-1);
                V2d(-5,-2); V2d(2,5);   V2d(5,3);   V2d(3,-5); 
                V2d(-2,6);  V2d(0,-7);  V2d(-4,-6); V2d(-6,4); 
                V2d(-8,0);  V2d(7,-4);  V2d(6,7);   V2d(-7,-8)
            |] |> Arr<16 N, _>

//            Array.map (fun v -> v - V2d(0.5, 0.5)) [|
//                V2d(0.5625, 0.3125); V2d(0.4375, 0.6875); V2d(0.8125, 0.5625); V2d(0.3125, 0.1875);
//                V2d(0.1875, 0.8125); V2d(0.0625, 0.4375); V2d(0.6875, 0.9375); V2d(0.9375, 0.0625)
//
//            |] |> Arr<8 N, _>

        let pathFragmentAA (v : Vertex) =
            fragment {
                //let pos = v.samplePos
                let kind = v.klm.W

                let c = v.klm.XYZ
                let mutable alpha = 1.0
                let cx = ddx(v.klm.XYZ)
                let cy = ddy(v.klm.XYZ)

                if kind < 0.5 then
                    if uniform.Antialias then
                        let mutable sum = 0.0
                        for s in samplePattern do
                            let ci = c + s.X * cx + s.Y * cy
                            if ci.X >= 0.0 && ci.Y >= 0.0 && ci.Z >= 0.0 then
                                sum <- sum + 1.0
            
                        alpha <- sum / 16.0

                elif kind > 0.5 && kind < 1.5 then 
                    // outside triangle
                    alpha <- 0.0

                elif kind > 1.5 && kind < 3.5 then
                    // quadratic bezier
                    if uniform.Antialias then
                        let mutable sum = 0.0
                        for s in samplePattern do
                            let ci = c + s.X * cx + s.Y * cy
                            let f = (ci.X * ci.X - ci.Y) * ci.Z
                            if f <= 0.0 then
                                sum <- sum + 1.0

                        alpha <- sum / 16.0
                    else
                        let f = (c.X * c.X - c.Y) * c.Z
                        if f > 0.0 then alpha <- 0.0
                        


                return V4d(v.color.XYZ, alpha)
            }

        let pathFragment(v : Vertex) =
            fragment {
                //let pos = v.samplePos
                let kind = v.klm.W  + 0.001 * v.samplePos.X


                if kind > 1.5 && kind < 3.5 then
                    let ci = v.klm.XYZ
                    let f = (ci.X * ci.X - ci.Y) * ci.Z
                    if f > 0.0 then
                        discard()
//
//                if kind > 1.5 && kind < 3.5 then
//                    let f = (uv.X * uv.X - uv.Y) * uv.Z
//                    if f > 0.0 then
//                        discard()

                return v.color
            }
        let boundaryVertex (v : Vertex) =
            vertex {
                return { v with p = uniform.ModelViewProjTrafo * v.p }
            }

        let boundary (v : Vertex) =
            fragment {

                return uniform.BoundaryColor
            }


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
    let private texCoords(p0 : V2d, p1 : V2d, p2 : V2d, p3 : V2d) =
        let p0 = V3d(p0, 1.0)
        let p1 = V3d(p1, 1.0)
        let p2 = V3d(p2, 1.0)
        let p3 = V3d(p3, 1.0)

        let M3 =
            M44d(
                    +1.0,   0.0,  0.0,  0.0,
                    -3.0,   3.0,  0.0,  0.0,
                    +3.0,  -6.0,  3.0,  0.0,
                    -1.0,   3.0, -3.0,  1.0
            )

        let M3Inverse =
            M44d(
                    1.0,   0.0,        0.0,        0.0,
                    1.0,   1.0/3.0,    0.0,        0.0,
                    1.0,   2.0/3.0,    1.0/3.0,    0.0,
                    1.0,   1.0,        1.0,        1.0
            )

        let v0 =  1.0*p0
        let v1 = -3.0*p0 + 3.0*p1 
        let v2 =  3.0*p0 - 6.0*p1 + 3.0*p2
        let v3 = -1.0*p0 + 3.0*p1 - 3.0*p2 + 1.0*p3

        let det (r0 : V3d) (r1 : V3d) (r2 : V3d) =
            M33d.FromRows(r0, r1, r2).Det

        let d0 =  (det v3 v2 v1)
        let d1 = -(det v3 v2 v0)
        let d2 =  (det v3 v1 v0)
        let d3 = -(det v2 v1 v0)


        let O(a : V3d,b : V3d,c : V3d,d : V3d) =
            V3d(-a.X, -a.Y, a.Z),
            V3d(-b.X, -b.Y, b.Z),
            V3d(-c.X, -c.Y, c.Z),
            V3d(-d.X, -d.Y, d.Z)



        let zero v = Fun.IsTiny(v, 1.0E-5)
        let nonzero v = Fun.IsTiny(v, 1.0E-5) |> not

        let v = 3.0 * d2 * d2 - 4.0*d1*d3
        let d1z = zero d1
        let d1nz = d1z |> not


        // 1. The Serpentine
        // 3a. Cusp with inflection at infinity
        if d1nz && v >= 0.0 then
            // serpentine
            // Cusp with inflection at infinity

            let r = sqrt((3.0*d2*d2 - 4.0*d1*d3) / 3.0)
            let tl = d2 + r
            let sl = 2.0 * d1
            let tm = d2 - r
            let sm = sl


            let F =
                M44d(
                        tl * tm,            tl * tl * tl,           tm * tm * tm,       1.0,
                    -sm*tl - sl*tm,     -3.0*sl*tl*tl,          -3.0*sm*tm*tm,       0.0,
                        sl*sm,              3.0*sl*sl*tl,           3.0*sm*sm*tm,       0.0,
                        0.0,               -sl*sl*sl,              -sm*sm*sm,           0.0
                )

            let weights = M3Inverse * F

            let w0 = weights.R0.XYZ
            let w1 = weights.R1.XYZ
            let w2 = weights.R2.XYZ
            let w3 = weights.R3.XYZ

            let res = w0, w1, w2, w3
            if d1 < 0.0 then O res
            else res

        // 2. The Loop
        elif d1nz && v < 0.0 then
            // loop

            let r = sqrt(4.0 * d1 * d3 - 3.0*d2*d2)

            let td = d2 + r
            let sd = 2.0*d1

            let te = d2 - r
            let se = sd


            let F =
                M44d(
                        td*te,               td*td*te,                   td*te*te,                       1.0,
                    -se*td - sd*te,      -se*td*td - 2.0*sd*te*td,   -sd*te*te - 2.0*se*td*te,        0.0,
                        sd * se,             te*sd*sd + 2.0*se*td*sd,    td*se*se + 2.0*sd*te*se,        0.0,
                        0.0,                -sd*sd*se,                  -sd*se*se,                       0.0
                )

            let weights = M3Inverse * F

            let w0 = weights.R0.XYZ
            let w1 = weights.R1.XYZ
            let w2 = weights.R2.XYZ
            let w3 = weights.R3.XYZ

            let res = w0, w1, w2, w3
            if d1 < 0.0 then O res
            else res

        // 4. Quadratic
        elif zero d1 && zero d2 && nonzero d3 then
            let w0 = V3d(0.0,0.0,0.0)
            let w1 = V3d(1.0/3.0,0.0,1.0/3.0)
            let w2 = V3d(2.0/3.0,1.0/3.0,2.0/3.0)
            let w3 = V3d(1.0,1.0,1.0)


            let res = w0,w1,w2,w3
            if d3 < 0.0 then O res
            else res

        // 3b. Cusp with cusp at infinity
        elif d1z && zero v then
            let tl = d3
            let sl = 3.0*d2
            let tm = 1.0
            let sm = 0.0


            let F =
                M44d(
                    tl,     tl*tl*tl,       1.0, 1.0,
                    -sl,    -3.0*sl*tl*tl,   0.0, 0.0,
                    0.0,     3.0*sl*sl*tl,   0.0, 0.0,
                    0.0,    -sl*sl*sl,       0.0, 0.0
                )

            let weights = M3Inverse * F
            let w0 = weights.R0.XYZ
            let w1 = weights.R1.XYZ
            let w2 = weights.R2.XYZ
            let w3 = weights.R3.XYZ

            w0, w1, w2, w3


        elif Fun.IsTiny d1 && Fun.IsTiny d2 && Fun.IsTiny d3 then
            failwith "line or point"


        else
            failwith "not possible"

    /// creates a geometry using the !!closed!! path which contains the left-hand-side of
    /// the outline.
    /// The returned geometry contains Positions and a 4-dimensional vector (KLMKind) describing the
    /// (k,l,m) coordinates for boundary triangles in its xyz components and
    /// the kind of the triangle (inner = 0, boundary = 1) in its w component
    let toGeometry (p : Path) =
        let bounds = bounds p
        let innerPoints = List<List<V2d>>()
        let outerPoints = List<List<V2d>>()
        let boundaryEdges = HashSet<V2d * V2d>()
        let boundaryPoints = HashSet<V2d>()
        let boundaryTriangles = List<V2d>()
        let boundaryCoords = List<V4d>()

        let mutable current = V2d.NaN
        let mutable currentOuter = V2d.NaN

        let start (p : V2d) =
            if current <> p then
                innerPoints.Add(List())
                current <- V2d.NaN

        let startOuter (p : V2d) =
            if currentOuter <> p then
                outerPoints.Add(List())
                current <- V2d.NaN
            

        let add (p : V2d) =
            if current <> p then 
                innerPoints.[innerPoints.Count-1].Add p
                current <- p

        let addOuter (p : V2d) =
            if currentOuter <> p then 
                outerPoints.[outerPoints.Count-1].Add p
                currentOuter <- p
            

        let overlap (q0 : Polygon2d) (q1 : Polygon2d) =
            let realIntersections =
                seq {
                    for u in q0.EdgeLines do
                        let mutable t = nan
                        let r = u.Ray2d
                        for v in q1.EdgeLines do
                            match r.Intersects(v, &t) with
                                | true when t > 0.001 && t < 0.999 -> yield 1
                                | _ -> ()
                }
            realIntersections |> Seq.isEmpty |> not
        
        let allSplines = 
            p.outline |> Array.choose (fun s ->
                match s with
                    | Bezier3(p0, p1, p2, p3) -> Some(Polygon2d(p0, p1, p2, p3))
                    | Bezier2(p0, p1, p2) -> Some(Polygon2d(p0, p1, p2))
                    | _ -> None
            )

        let rec run (l : list<PathSegment>) =
            match l with
                | Line(p0, p1) :: rest ->
                    start p0
                    startOuter p0
                    add p0
                    addOuter p0
                    boundaryEdges.Add(p0, p1) |> ignore
                    boundaryPoints.Add(p0) |> ignore
                    boundaryPoints.Add(p1) |> ignore
                    add p1
                    addOuter p1
                    run rest

                | Bezier2(p0, p1, p2) :: rest ->
                    let q = Polygon2d(p0, p1, p2)
                    let p1Inside = p1.PosLeftOfLineValue(p0, p2) < 0.0

                    let overlapping = allSplines |> Seq.exists (overlap q)
                    if overlapping then
                        let m0 = 0.5 * (p0 + p1)
                        let m1 = 0.5 * (p1 + p2)
                        let pp = 0.5 * (m0 + m1)

                        run (Bezier2(p0,m0,pp)::Bezier2(pp, m1, p2)::rest)

                    else
                        start p0
                        add p0

                        startOuter p0
                        addOuter p0

                        if p1Inside then 
                            add p1
                        else
                            addOuter p1

                        boundaryTriangles.AddRange [p0; p1; p2]
                        boundaryPoints.Add(p0) |> ignore
                        boundaryPoints.Add(p2) |> ignore

                        if p1Inside then
                            boundaryCoords.AddRange [V4d(0,0,-1,2); V4d(-0.5, 0.0,-1.0, 2.0); V4d(-1,1,-1,2)]
                        else
                            boundaryCoords.AddRange [V4d(0,0,1,3); V4d(0.5, 0.0, 1.0,3.0); V4d(1,1,1,3)]

                        add p2
                        addOuter p2

                        current <- p2
                        run rest

                | (Bezier3(p0, p1, p2, p3) as s) :: rest ->
                        

                    let q = Polygon2d(p0, p1, p2, p3)
                    let p1Inside = p1.PosLeftOfLineValue(p0, p3) < 0.0
                    let p2Inside = p2.PosLeftOfLineValue(p0, p3) < 0.0

                    let overlapping = allSplines |> Seq.exists (overlap q)
                    if overlapping then

                        let m0 = 0.5 * (p1 + p0)
                        let m1 = 0.5 * (p1 + p2)
                        let m2 = 0.5 * (p2 + p3)
                        let q0 = 0.5 * (m0 + m1)
                        let q1 = 0.5 * (m1 + m2)
                        let p = 0.5 * (q0 + q1)

                        let s0 = Bezier3(p0, m0, q0, p)
                        let s1 = Bezier3(p, q1, m2, p3)

                        run (s0::s1::rest)

                    else
                        start p0
                        add p0

                        startOuter p0
                        addOuter p0

                        let kind = 
                            if p1Inside && p2Inside then
                                add p1
                                add p2
                                4.0
                            elif not p1Inside && not p2Inside then
                                addOuter p1
                                addOuter p2
                                5.0
                            else
                                6.0
                            

                        

                        let w0,w1,w2,w3 = texCoords(p0, p1, p2, p3)
                        boundaryTriangles.AddRange [p0; p1; p2]
                        boundaryCoords.AddRange [V4d(w0, kind); V4d(w1, kind); V4d(w2, kind)]
                        boundaryTriangles.AddRange [p0; p2; p3]
                        boundaryCoords.AddRange [V4d(w0, kind); V4d(w2, kind); V4d(w3, kind)]
                        
                        boundaryPoints.Add(p0) |> ignore
                        boundaryPoints.Add(p3) |> ignore

                        add p3
                        addOuter p3
                        current <- p3

                        run rest


                | [] -> ()

        run (Array.toList p.outline)

        // merge the interior polygons (respecting holes)
        let innerPoints = 
            innerPoints |> Seq.map (fun l -> 
                let p = l |> CSharpList.toArray |> Array.take (l.Count-1) |> Polygon2d
                p //Clipper.clip p
            ) |> Seq.toArray

        let isBoundary (p0 : V2d) (p1 : V2d) =
            boundaryEdges.Contains(p0, p1) || boundaryEdges.Contains(p1, p0)

        let isBoundaryPoint (p : V2d) =
            boundaryPoints.Contains p


        let index, positions = LibTess.TessellateIndexed(innerPoints, false)
        let boundaryPoint = positions |> Array.map isBoundaryPoint
        
        let trianglePositions = List<V3d>(positions.Length * 2)
        let triangleCoords = List<V4d>(positions.Length * 2)

        let rec appendTriangle (i0 : int) (i1 : int) (i2 : int) (p0 : V2d) (p1 : V2d) (p2 : V2d) =
            let kind = 0
//            let border u v w = V4d(u, v, w, kind)
            trianglePositions.AddRange [ V3d(p0, 0.0); V3d(p1, 0.0); V3d(p2, 0.0) ]
            triangleCoords.AddRange [ V4d(1,0,0,0); V4d(0,1,0,0); V4d(0,0,1,0) ]
//
//            let b0 = if i0 >= 0 then boundaryPoint.[i0] else false
//            let b1 = if i1 >= 0 then boundaryPoint.[i1] else false
//            let b2 = if i2 >= 0 then boundaryPoint.[i2] else false
//            
//            let insideCoord = V4d(-1,0,0,kind)
//            let borderCoord = V4d(0,0,0,kind)
//
//            let insideCoord = V4d(-1,-1,-1,kind)
//            let borderCoord = V4d(0,0,0,kind)
//
//            let border u v w = V4d(u, v, w, float kind)
//
//            let full = V4d(1,0,0,kind)
//
//            let coord (pi : int) (isBorder : bool) =
//                if isBorder then V4d(0,0,0,kind)
//                else V4d(-1,0,0,kind)
//
//            match b0, b1, b2 with
//                | false, false, false -> 
//                    trianglePositions.AddRange [ V3d(p0, 0.0); V3d(p1, 0.0); V3d(p2, 0.0) ]
//                    triangleCoords.AddRange [ full; full; full ]
//
//                | true, false, false ->
//                    trianglePositions.AddRange [ V3d(p0, 0.0); V3d(p1, 0.0); V3d(p2, 0.0) ]
//                    triangleCoords.AddRange [ coord 0 false; coord 1 false; coord 2 false ]
//
//                | false, true, false ->
//                    trianglePositions.AddRange [ V3d(p0, 0.0); V3d(p1, 0.0); V3d(p2, 0.0) ]
//                    triangleCoords.AddRange [ coord 0 false; coord 1 false; coord 2 false ]
//
//                | false, false, true ->
//                    trianglePositions.AddRange [ V3d(p0, 0.0); V3d(p1, 0.0); V3d(p2, 0.0) ]
//                    triangleCoords.AddRange [ coord 0 false; coord 1 false; coord 2 false ]
//
//
//                | true, false, true ->
//                    if isBoundary p0 p2 then
//                        trianglePositions.AddRange [ V3d(p0, 0.0); V3d(p1, 0.0); V3d(p2, 0.0) ]
//                        triangleCoords.AddRange [ coord 0 true; coord 1 false; coord 2 true ]
//                    else
//                        let p = 0.5 * (p0 + p2)
//                        appendTriangle i0 i1 -1 p0 p1 p
//                        appendTriangle -1 i1 i2 p p1 p2
//
//                | true, true, false ->
//                    if isBoundary p0 p1 then
//                        trianglePositions.AddRange [ V3d(p0, 0.0); V3d(p1, 0.0); V3d(p2, 0.0) ]
//                        triangleCoords.AddRange [ coord 0 true; coord 1 true; coord 2 false ]
//                    else
//                        let p = 0.5 * (p0 + p1)
//                        appendTriangle i0 -1 i2 p0 p p2
//                        appendTriangle -1 i1 i2 p p1 p2
//
//                | false, true, true ->
//                    if isBoundary p1 p2 then
//                        trianglePositions.AddRange [ V3d(p0, 0.0); V3d(p1, 0.0); V3d(p2, 0.0) ]
//                        triangleCoords.AddRange [ coord 0 false; coord 1 true; coord 2 true ]
//                    else
//                        let p = 0.5 * (p1 + p2)
//                        appendTriangle i0 i1 -1 p0 p1 p
//                        appendTriangle i0 -1 i2 p0 p p2
//
//                | true, true, true ->
//                    let c = (p0 + p1 + p2) / 3.0
//                    appendTriangle i0 i1 -1 p0 p1 c
//                    appendTriangle i1 i2 -1 p1 p2 c
//                    appendTriangle i2 i0 -1 p2 p0 c

        for ti in 0 .. index.Length/3 - 1 do
            let i0 = index.[3 * ti + 0]
            let i1 = index.[3 * ti + 1]
            let i2 = index.[3 * ti + 2]

            let p0 = positions.[i0]
            let p1 = positions.[i1]
            let p2 = positions.[i2]

            appendTriangle i0 i1 i2 p0 p1 p2


        let triangles =
            innerPoints
                |> LibTess.Tessellate
                |> Array.map (fun t -> true, t)


//        let trianglePositions = List<V3d>(3 * triangles.Length)
//        let triangleCoords = List<V4d>(3 * triangles.Length)
//
//        let spikeCount = Dict<V2d, HashSet<int>>()
//
//        let addSpike (pt : V2d) = 
//            let index = trianglePositions.Count / 3
//            let r = spikeCount.GetOrCreate(pt, fun _ -> HashSet())
//            r.Add index |> ignore
//
//        let rec appendTriangle (inside : bool) (t : Triangle2d) =
//            let kind = if inside then 0 else 1
//
//            let b0 = isBoundaryPoint t.P0
//            let b1 = isBoundaryPoint t.P1
//            let b2 = isBoundaryPoint t.P2
//            
//            let insideCoord = V4d(-1,0,0,kind)
//            let borderCoord = V4d(0,0,0,kind)
//
//            let insideCoord = V4d(-1,-1,-1,kind)
//            let borderCoord = V4d(0,0,0,kind)
//
//            let border u v w = V4d(u, v, w, kind)
//
//
//
//            match b0, b1, b2 with
//                | false, false, false -> 
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ insideCoord; insideCoord; insideCoord ]
//
//                | true, false, false ->
//                    addSpike t.P0
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ border 1 0 10; border 0 1 10; border 0 1 10 ]
//
//                | false, true, false ->
//                    addSpike t.P1
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ border 0 1 10; border 1 0 10; border 0 1 10 ]
//
//                | false, false, true ->
//                    addSpike t.P2
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ border 0 1 10; border 0 1 10; border 1 0 10 ]
//
//
//                | true, false, true ->
//                    if isBoundary t.P0 t.P2 then
//                        trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                        triangleCoords.AddRange [ border 1 0 0; border 0 1 0; border 0 0 1 ]
//                    else
//                        let p = 0.5 * (t.P0 + t.P2)
//                        appendTriangle inside (Triangle2d(t.P0, t.P1, p))
//                        appendTriangle inside (Triangle2d(p, t.P1, t.P2))
//
//                | true, true, false ->
//                    if isBoundary t.P0 t.P1 then
//                        trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                        triangleCoords.AddRange [ border 1 0 0; border 0 0 1; border 0 1 0 ]
//                    else
//                        let p = 0.5 * (t.P0 + t.P1)
//                        appendTriangle inside (Triangle2d(t.P0, p, t.P2))
//                        appendTriangle inside (Triangle2d(p, t.P1, t.P2))
//
//                | false, true, true ->
//                    if isBoundary t.P1 t.P2 then
//                        trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                        triangleCoords.AddRange [ border 0 1 0; border 1 0 0; border 0 0 1 ]
//                    else
//                        let p = 0.5 * (t.P1 + t.P2)
//                        appendTriangle inside (Triangle2d(t.P0, t.P1, p))
//                        appendTriangle inside (Triangle2d(t.P0, p, t.P2))
//
//                | true, true, true ->
//                    let c = (t.P0 + t.P1 + t.P2) / 3.0
//                    appendTriangle inside (Triangle2d(t.P0, t.P1, c))
//                    appendTriangle inside (Triangle2d(t.P1, t.P2, c))
//                    appendTriangle inside (Triangle2d(t.P2, t.P0, c))
//
//        for inside, t in triangles do
//            appendTriangle inside t
//          
//        for indices in spikeCount.Values do
//            let count = float indices.Count
//
//            for ti in indices do
//                for pi in 0 .. 2 do
//                    let i = 3 * ti + pi
//                    let c = triangleCoords.[i]
//                    triangleCoords.[i] <- V4d(c.X, c.Y, 5.0 + count, c.W)
//

        let triangles = ()
                    

//        for inside, t in triangles do
//            let kind = if inside then 0 else 1
//
//            let b01 = isBoundary t.P0 t.P1
//            let b12 = isBoundary t.P1 t.P2
//            let b20 = isBoundary t.P2 t.P0
//
//            let divide = (b01 && b12) || (b12 && b20) || (b20 && b01)
//
//            let insideCoord = V4d(-1,0,0,kind)
//            let borderCoord = V4d(0,0,0,kind)
//
//            match b01, b12, b20 with
//                | false,    false,      false -> 
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ insideCoord; insideCoord; insideCoord ]
//                        
//                | true,     false,      false -> 
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ borderCoord; borderCoord; insideCoord ]
//
//                | false,    true,       false ->  
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ insideCoord; borderCoord; borderCoord ]
//
//                | false,    false,      true  ->  
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ borderCoord; insideCoord; borderCoord ]
//
//                | true,     true,       false -> 
//                    //01 and 12 edges
//                    let c = (t.P0 + t.P2) * 0.5
//
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(c, 0.0) ]
//                    triangleCoords.AddRange [ borderCoord; borderCoord; insideCoord ]
//                    trianglePositions.AddRange [ V3d(c, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ insideCoord; borderCoord; borderCoord ]
//
//                | true,     false,      true  -> 
//                    // 01 and 20 edges
//                    let c = (t.P1 + t.P2) * 0.5
//
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(c, 0.0) ]
//                    triangleCoords.AddRange [ borderCoord; borderCoord; insideCoord ]
//                    trianglePositions.AddRange [ V3d(c, 0.0); V3d(t.P2, 0.0); V3d(t.P0, 0.0) ]
//                    triangleCoords.AddRange [ insideCoord; borderCoord; borderCoord ]
//
//                | false,    true,       true  -> 
//                    // 12 and 20 edges
//                    let c = (t.P0 + t.P1) * 0.5
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(c, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ borderCoord; insideCoord; borderCoord ]
//                    trianglePositions.AddRange [ V3d(c, 0.0); V3d(t.P1, 0.0); V3d(t.P2, 0.0) ]
//                    triangleCoords.AddRange [ insideCoord; borderCoord; borderCoord ]
//
//
//                | true,     true,       true  -> 
//                    let c = (t.P0 + t.P1 + t.P2) / 3.0
//
//                    trianglePositions.AddRange [ V3d(t.P0, 0.0); V3d(t.P1, 0.0); V3d(c, 0.0) ]
//                    triangleCoords.AddRange [ borderCoord; borderCoord; insideCoord ]
//
//                    trianglePositions.AddRange [ V3d(t.P1, 0.0); V3d(t.P2, 0.0); V3d(c, 0.0) ]
//                    triangleCoords.AddRange [ borderCoord; borderCoord; insideCoord ]
//
//                    trianglePositions.AddRange [ V3d(t.P2, 0.0); V3d(t.P0, 0.0); V3d(c, 0.0) ]
//                    triangleCoords.AddRange [ borderCoord; borderCoord; insideCoord ]
//
//
//
//            ()



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
