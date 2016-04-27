namespace Aardvark.Rendering.Text

open System
open System.Collections.Generic
open Aardvark.Base


type Path = private { outline : PathSegment[] }

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Path =

    module private Poly2Tri =
        open Poly2Tri
        open Poly2Tri.Triangulation
        open Poly2Tri.Triangulation.Polygon

        let private toInternal (p : Polygon2d) =
            Polygon(p.Points |> Seq.map (fun v -> PolygonPoint(v.X, v.Y)) |> Seq.toArray)

        let triangulate (polygon : Polygon2d) (holes : seq<Polygon2d>) =
            let p = toInternal polygon

            for h in holes do
                p.AddHole(toInternal h)
        
            P2T.Triangulate [p]


            p.Triangles
                |> Seq.map (fun t -> 
                    let p0 = t.Points.[0]
                    let p1 = t.Points.[1]
                    let p2 = t.Points.[2]
                    Triangle2d(V2d(p0.X, p0.Y), V2d(p1.X, p1.Y), V2d(p2.X, p2.Y))
                   )
                |> Seq.toList
            
    
    module Attributes = 
        let KLMKind = Symbol.Create "KLMKind"
        let PathOffset = Symbol.Create "PathOffset"

    type KLMKindAttribute() = inherit FShade.Parameters.SemanticAttribute(Attributes.KLMKind |> string)
    type PathOffsetAttribute() = inherit FShade.Parameters.SemanticAttribute(Attributes.PathOffset |> string)

    module Shader = 
        open FShade

        type UniformScope with
            member x.FillGlyphs : bool = uniform?FillGlyphs
            member x.Antialias : bool = uniform?Antialias

        type Vertex =
            {
                [<Position>] p : V4d
                [<KLMKind>] klm : V4d
                [<PathOffset>] offset : V2d
            }

        let pathVertex (v : Vertex) =
            vertex {
                let p = V4d(v.p.X + v.offset.X, v.p.Y + v.offset.Y, v.p.Z, v.p.W)
                return { v with p = p }
            }

        let pathFragment (v : Vertex) =
            fragment {
                let kind = v.klm.W
                if kind < 0.5 then
                    return V4d.IIII
                else    
                    if uniform.FillGlyphs then
                        let klm = v.klm.XYZ
 
                        if uniform.Antialias then
                            let dx = ddx(klm)
                            let dy = ddy(klm)
                            let f = pow klm.X 3.0 - klm.Y*klm.Z
                            let fx = 3.0*klm.X*klm.X*dx.X - klm.Y*dx.Z - klm.Z*dx.Y
                            let fy = 3.0*klm.X*klm.X*dy.X - klm.Y*dy.Z - klm.Z*dy.Y

                            let sd = f / sqrt (fx*fx + fy*fy)
                            let alpha = 0.5 - sd

                            if alpha > 1.0 then
                                return V4d.IIII
                            elif alpha < 0.0 then
                                return V4d.OOOI
                            else
                                return V4d(alpha, alpha, alpha, 1.0)
                        else
                            if pow klm.X 3.0 > klm.Y*klm.Z then
                                return V4d.OOOI
                            else
                                return V4d.IIII
                    else
                        return V4d.IOOI


            }




    /// create a path using a single segment
    let single (seg : PathSegment) =
        { outline = [| seg |] }

    /// creates a path using the given segments
    let ofSeq (segments : seq<PathSegment>) =
        { outline = Seq.toArray segments }

    /// creates a path using the given segments
    let ofList (segments : list<PathSegment>) =
        { outline = List.toArray segments }

    /// creates a path using the given segments
    let ofArray (segments : PathSegment[]) =
        { outline = Array.copy segments }

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
        { outline = Array.append l.outline r.outline }

    /// concatenates a sequence paths
    let concat (l : seq<Path>) =
        { outline = l |> Seq.toArray |> Array.collect toArray }

    /// reverses the entrie path
    let reverse (p : Path) =
        { outline = p.outline |> Array.map PathSegment.reverse |> Array.rev }

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
        { outline = Array.map (PathSegment.transform f) p.outline }

    /// creates a geometry using the !!closed!! path which contains the left-hand-side of
    /// the outline.
    /// The returned geometry contains Positions and a 4-dimensional vector (KLMKind) describing the
    /// (k,l,m) coordinates for boundary triangles in its xyz components and
    /// the kind of the triangle (inner = 0, boundary = 1) in its w component
    let toGeometry (p : Path) =

        // calculates the (k,l,m) coordinates for a given bezier-segment as
        // shown by Blinn 2003: http://www.msr-waypoint.net/en-us/um/people/cloop/LoopBlinn05.pdf
        // returns the (k,l,m) triples for the four control-points
        let texCoords(p0 : V2d, p1 : V2d, p2 : V2d, p3 : V2d) =
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


        let innerPoints = List<List<V2d>>()
        let boundaryTriangles = List<V2d>()
        let boundaryCoords = List<V3d>()
        let mutable current = V2d.NaN

        let start (p : V2d) =
            if current <> p then
                innerPoints.Add(List())
                current <- V2d.NaN

        let add p =
            if current <> p then 
                innerPoints.[innerPoints.Count-1].Add p
                current <- p


        let overlap (q0 : Polygon2d) (q1 : Polygon2d) =
            let realIntersections =
                seq {
                    for u in q0.EdgeLines do
                        for v in q1.EdgeLines do
                            let mutable t = nan
                            match u.Ray2d.Intersects(v, &t) with
                                | true when t > 0.001 && t < 0.999 -> yield true
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
                    add p0

                    add p1
                    current <- p1
                    run rest

                | Bezier2(p0, p1, p2) :: rest ->
                    let q = Polygon2d(p0, p1, p2)
                    let p1Inside = p1.PosLeftOfLineValue(p0, p2) > 0.0

                    let overlapping = allSplines |> Seq.exists (overlap q)
                    if overlapping then

                        let m0 = 0.5 * (p0 + p1)
                        let m1 = 0.5 * (p1 + p2)
                        let pp = 0.5 * (m0 + m1)

                        run (Bezier2(p0,m0,pp)::Bezier2(pp, m1, p2)::rest)

                    else
                        start p0
                        add p0

                        if p1Inside then add p1

                        boundaryTriangles.AddRange [p0; p1; p2]
                        if p1Inside then
                            boundaryCoords.AddRange [V3d(0,0,0); V3d(-0.5, 0.0, -0.5); V3d(-1,1,-1)]
                        else
                            boundaryCoords.AddRange [V3d(0,0,0); V3d(0.5, 0.0, 0.5); V3d(1,1,1)]

                        add p2
                        current <- p2
                        run rest

                | (Bezier3(p0, p1, p2, p3) as s) :: rest ->
                        

                    let q = Polygon2d(p0, p1, p2, p3)
                    let p1Inside = p1.PosLeftOfLineValue(p0, p3) > 0.0
                    let p2Inside = p2.PosLeftOfLineValue(p0, p3) > 0.0

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

                        if p1Inside && p2Inside then
                            add p1
                            add p2

                        let w0,w1,w2,w3 = texCoords(p0, p1, p2, p3)
                        boundaryTriangles.AddRange [p0; p1; p2]
                        boundaryCoords.AddRange [w0; w1; w2]
                        boundaryTriangles.AddRange [p0; p2; p3]
                        boundaryCoords.AddRange [w0; w2; w3]

                        add p3
                        current <- p3

                        run rest


                | [] -> ()

        run (Array.toList p.outline)

        // merge the interior polygons (respecting holes)
        let innerPoints = innerPoints |> CSharpList.map (CSharpList.toArray >> Polygon2d)



        let mergePolys(a : Polygon2d) (b : Polygon2d) =

            //let b = b.Reversed

            let distances =
                seq {
                    for i in 0..a.PointCount-1 do
                        for j in 0..b.PointCount-1 do
                            let a = a.[i]
                            let b = b.[j]
                            yield (i,j), V2d.Distance(a,b)
                }

            let (i,j) = distances |> Seq.minBy snd |> fst


            let rot (i : int) (points : 'a[]) =
                let i = i % (points.Length-1)
                if i = 0 then points
                else Array.append (Array.skip i points) (Array.sub points 1 i)

            let aperm = a.GetPointArray() |> rot i
            let bperm = b.GetPointArray() |> rot j



            // a0 ... an,a0, b0 ... bn,b0, a0
            Array.concat [ aperm; bperm; [|aperm.[0]|] ] |> Polygon2d

        let polygons = List<Polygon2d * List<Polygon2d>>()

  

        //innerPoints.[0] <- innerPoints.[0].Reversed

        for polygon in innerPoints do
            let last =
                if polygons.Count > 0 then Some polygons.[polygons.Count-1]
                else None

//                let containing =
//                    polygons 
//                        |> Seq.indexed 
//                        |> Seq.tryFind (fun (i,p) -> p.IsFullyContainedInside polygon || polygon.IsFullyContainedInside p)
//      
                
            match last with
//                    | Some (i, other) ->
//                        polygons.[i] <- mergePolys other polygon
//
//                    
                | Some (last, holes) when last.BoundingBox2d.Contains polygon.BoundingBox2d ->
                    holes.Add(polygon)
                    //polygons.[polygons.Count-1] <- mergePolys last polygon

                    
                | Some (last, holes) when polygon.BoundingBox2d.Contains last.BoundingBox2d->
        
                    polygons.[polygons.Count-1] <- (polygon, List [last])
                    polygons.InsertRange(0, holes |> Seq.map (fun p -> p, List()))

                | _ ->
                    polygons.Add (polygon, List())

        // triangulate the interior polygons (marking all vertices as interior ones => fst = 0)
        let interiorTriangles =
            polygons
            |> CSharpList.toArray
            |> Array.collect (fun (p, holes) ->
                let triangles = Poly2Tri.triangulate p holes

                triangles 
                    |> List.collect (fun t -> [t.P0; t.P1; t.P2]) 
                    |> List.map (fun v -> V3d(v, 0.0))
                    |> List.toArray

//                    let p = Polygon2d(p.GetPointArray() |> Array.take (p.PointCount-1))
//                    let poly = p.ToPolygon3d (fun v -> V3d(v, 0.0))
//                    
//                    let idx = poly.ComputeTriangulationOfConcavePolygon(1.0E-6)
//                    idx |> Array.map (fun i -> poly.[i])

//                    let sub = p.ComputeNonConcaveSubPolygons(1.0E-6)
//                    let test = 
//                        sub |> Seq.collect (fun i ->
//                            let p = Polygon2d(i |> Array.map (fun i -> p.[i])).ToPolygon3d(fun v -> V3d(v, 0.0))
//
//                            let idx = p.ComputeTriangulationOfConcavePolygon(1.0E-6)
//
//                            idx |> Array.map (fun i -> p.[i])
//                        )
//                    let index = poly.ComputeTriangulationOfConcavePolygon(Constant.PositiveTinyValue)
//                
//
//                    index |> Array.map (fun i -> poly.[i])
                //test |> Seq.toArray
            )

        // union the interior with the bounary triangles
        let boundaryTriangles = boundaryTriangles |> Seq.map (fun v -> V3d(v.X, v.Y, 0.0)) |> Seq.toArray
        let boundaryCoords = boundaryCoords |> CSharpList.toArray |> Array.map (fun v -> V4d(v, 1.0))
        let pos = Array.append interiorTriangles boundaryTriangles
        let tex = Array.append (Array.create interiorTriangles.Length V4d.Zero) boundaryCoords


        // use the merged vertex-data for creating the final geometry
        IndexedGeometry(
            Mode = IndexedGeometryMode.TriangleList,
            IndexedAttributes =
                SymDict.ofList [
                    DefaultSemantic.Positions,  pos |> Array.map (V3f.op_Explicit) :> Array
                    Attributes.KLMKind,         tex |> Array.map (V4f.op_Explicit) :> Array
                ]
        )

    