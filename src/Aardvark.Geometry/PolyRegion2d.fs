namespace Aardvark.Geometry

open System
open System.Runtime.CompilerServices
open Aardvark.Base
open LibTessDotNet.Double

module private LibTess =
    let closed (path : array<V2d>) =
        let isClosed = path.[0] = path.[path.Length-1]
           
        let arr = Array.zeroCreate (path.Length + (if isClosed then 0 else 1))

        for i in 0 .. path.Length - 1 do
            let pt = path.[i]
            arr.[i] <- ContourVertex(Position = Vec3(X = pt.X, Y = pt.Y, Z = 0.0))
                        
        if not isClosed then
            arr.[path.Length] <- arr.[0]


        arr

    let closed3 (path : array<V3d>) =
        let isClosed = path.[0] = path.[path.Length-1]
           
        let arr = Array.zeroCreate (path.Length + (if isClosed then 0 else 1))

        for i in 0 .. path.Length - 1 do
            let pt = path.[i]
            arr.[i] <- ContourVertex(Position = Vec3(X = pt.X, Y = pt.Y, Z = pt.Z))
                        
        if not isClosed then
            arr.[path.Length] <- arr.[0]


        arr

    let nonRedundantPoints (angleEps : float) (p : V2d[]) : Option<int[]> =
        if p.Length < 3 then
            None
        else
            let inline angle (a : V2d) (b : V2d) =
                let d = Vec.dot a b
                let c = a.X * b.Y - a.Y * b.X
                if d > 0.0 then asin c
                else Constant.Pi - asin c
                    
            
            let mutable pl = p.[p.Length - 1]
            let mutable pc = p.[0]
            let mutable pn = p.[1]

            let mutable dlc = pc - pl |> Vec.normalize
            let mutable dcn = pn - pc |> Vec.normalize

            let points = System.Collections.Generic.List<int>()
            for i in 0 .. p.Length - 1 do
                let a = angle dlc dcn
                if abs a > angleEps then
                    points.Add(i)

                pl <- pc
                pc <- pn
                pn <- p.[(i + 2) % p.Length]
                dlc <- dcn
                dcn <- pn - pc |> Vec.normalize 

            if points.Count < 3 then 
                None
            else 
                Some (points.ToArray())

    let boundary (rule : WindingRule) (regions : seq<list<Polygon2d>>) : list<Polygon2d> =
        let t = Tess()
                
        for r in regions do
            for p in r do
                let contour = closed (Seq.toArray p.Points)
                t.AddContour(contour)

        t.Tessellate(rule, ElementType.BoundaryContours, 3)

        if isNull t.Elements || isNull t.Vertices then 
            []
        else
       
            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun p -> V2d(p.Position.X, p.Position.Y))

            let results = System.Collections.Generic.List<Polygon2d>()

            for pi in 0 .. 2 .. indices.Length - 1 do
                let start = indices.[pi + 0]
                let count = indices.[pi + 1]
                let poly = Array.init count (fun vi -> vertices.[start + vi])
                match nonRedundantPoints Constant.PositiveTinyValue poly with
                    | Some index -> results.Add (Polygon2d (index |> Array.map (Array.get poly)))
                    | _ -> ()

            results |> CSharpList.toList

    let triangulate (rule : WindingRule) (region : list<Polygon2d>) : Triangle2d[] =
        let t = Tess()
                
        for p in region do
            let contour = closed (Seq.toArray p.Points)
            t.AddContour(contour)

        t.Tessellate(rule, ElementType.Polygons, 3)

        if isNull t.Elements || isNull t.Vertices then 
            [||]
        else
            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun p -> V2d(p.Position.X, p.Position.Y))

            Array.init (indices.Length / 3) (fun pi ->
                let i0 = indices.[3 * pi + 0]
                let i1 = indices.[3 * pi + 1]
                let i2 = indices.[3 * pi + 2]
                Triangle2d(vertices.[i0], vertices.[i1], vertices.[i2])
            )

type TessellationRule =
    | EvenOdd = 0
    | NonZero = 1
    | Positive = 2
    | Negative = 3
    | AbsGreater1 = 4

type Triangle2d<'a> =
    struct
        val mutable public P0 : V2d
        val mutable public P1 : V2d
        val mutable public P2 : V2d

        val mutable public A0 : 'a
        val mutable public A1 : 'a
        val mutable public A2 : 'a

        new(p0, a0, p1, a1, p2, a2) = { P0 = p0; P1 = p1; P2 = p2; A0 = a0; A1 = a1; A2 = a2 }
    end

type Polygon2d<'a> =
    struct
        val mutable public Points : V2d[]
        val mutable public Attributes : 'a[]

        new(p,a) = { Points = p; Attributes = a }
    end

type Triangle3d<'a> =
    struct
        val mutable public P0 : V3d
        val mutable public P1 : V3d
        val mutable public P2 : V3d

        val mutable public A0 : 'a
        val mutable public A1 : 'a
        val mutable public A2 : 'a

        new(p0, a0, p1, a1, p2, a2) = { P0 = p0; P1 = p1; P2 = p2; A0 = a0; A1 = a1; A2 = a2 }
    end

type Polygon3d<'a> =
    struct
        val mutable public Points : V3d[]
        val mutable public Attributes : 'a[]

        new(p,a) = { Points = p; Attributes = a }
    end

[<AbstractClass; Sealed; Extension>]
type PolygonTessellator private() =

    [<Extension>]
    static member Combine (regions : seq<V2d[] * 'a[]>, rule : TessellationRule, interpolate : float[] -> 'a[] -> 'a) =
        let t = Tess()
                
        for (pts, att) in regions do
            let contour = LibTess.closed pts
            for i in 0 .. contour.Length - 1 do contour.[i].Data <- att.[i % att.Length]
            t.AddContour(contour)

        let combine (pos : Vec3) (data : obj[]) (weights : float[]) =
            interpolate weights (data |> Array.map unbox) :> obj

        t.Tessellate(unbox (int rule), ElementType.BoundaryContours, 2, CombineCallback(combine))
        
        if isNull t.Elements || isNull t.Vertices then
            []
        else
            let results = System.Collections.Generic.List<V2d[] * 'a[]>()

            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun v -> V2d(v.Position.X, v.Position.Y), unbox<'a> v.Data)

            for pi in 0 .. 2 .. indices.Length - 2 do
                let start = indices.[pi + 0]
                let count = indices.[pi + 1]

                let points = Array.zeroCreate count
                let att = Array.zeroCreate count
                for vi in 0 .. count - 1 do
                    let (p,a) = vertices.[start + vi]
                    points.[vi] <- p
                    att.[vi] <- a

                results.Add(points, att)

            CSharpList.toList results
            
    [<Extension>]
    static member Combine (regions : seq<V2d[]>, rule : TessellationRule) =
        let t = Tess()
                
        for pts in regions do
            let contour = LibTess.closed pts
            t.AddContour(contour)

        t.Tessellate(unbox (int rule), ElementType.BoundaryContours, 2)
        
        if isNull t.Elements || isNull t.Vertices then
            []
        else
            let results = System.Collections.Generic.List<V2d[]>()

            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun v -> V2d(v.Position.X, v.Position.Y))

            for pi in 0 .. 2 .. indices.Length - 2 do
                let start = indices.[pi + 0]
                let count = indices.[pi + 1]

                let points = Array.init count (fun vi -> vertices.[start + vi])
                results.Add(points)

            CSharpList.toList results
            
    [<Extension>]
    static member Combine (regions : seq<Polygon2d<'a>>, rule : TessellationRule, interpolate : float[] -> 'a[] -> 'a) =
        let regions = regions |> Seq.map (fun p -> p.Points, p.Attributes)
        PolygonTessellator.Combine(regions, rule, interpolate)
        
    [<Extension>]
    static member Combine (regions : seq<Polygon2d>, rule : TessellationRule) =
        let regions = regions |> Seq.map (fun p -> Seq.toArray p.Points)
        PolygonTessellator.Combine(regions, rule)
        
    [<Extension>]
    static member Triangulate (regions : seq<V2d[] * 'a[]>, rule : TessellationRule, interpolate : float[] -> 'a[] -> 'a) : list<Triangle2d<'a>> =
        let t = Tess()
                
        for (pts, att) in regions do
            let contour = LibTess.closed pts
            for i in 0 .. contour.Length - 1 do contour.[i].Data <- att.[i % att.Length]
            t.AddContour(contour)

        let combine (pos : Vec3) (data : obj[]) (weights : float[]) =
            interpolate weights (data |> Array.map unbox) :> obj

        t.Tessellate(unbox (int rule), ElementType.Polygons, 3, CombineCallback(combine))
        
        if isNull t.Elements || isNull t.Vertices then
            []
        else
            let results = System.Collections.Generic.List<Triangle2d<'a>>()

            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun v -> V2d(v.Position.X, v.Position.Y), unbox<'a> v.Data)

            for pi in 0 .. 3 .. indices.Length - 3 do
                let i0 = indices.[pi + 0]
                let i1 = indices.[pi + 1]
                let i2 = indices.[pi + 2]

                let (p0, a0) = vertices.[i0]
                let (p1, a1) = vertices.[i1]
                let (p2, a2) = vertices.[i2]

                results.Add(Triangle2d<'a>(p0, a0, p1, a1, p2, a2))


            CSharpList.toList results
            
    [<Extension>]
    static member Triangulate (regions : seq<V2d[]>, rule : TessellationRule) =
        let t = Tess()
                
        for pts in regions do
            let contour = LibTess.closed pts
            t.AddContour(contour)

        t.Tessellate(unbox (int rule), ElementType.Polygons, 3)
        
        if isNull t.Elements || isNull t.Vertices then
            []
        else
            let results = System.Collections.Generic.List<Triangle2d>()

            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun v -> V2d(v.Position.X, v.Position.Y))

            for pi in 0 .. 3 .. indices.Length - 3 do
                let i0 = indices.[pi + 0]
                let i1 = indices.[pi + 1]
                let i2 = indices.[pi + 2]

                let p0 = vertices.[i0]
                let p1 = vertices.[i1]
                let p2 = vertices.[i2]

                results.Add(Triangle2d(p0, p1, p2))


            CSharpList.toList results

    [<Extension>]
    static member Triangulate (regions : seq<Polygon2d<'a>>, rule : TessellationRule, interpolate : float[] -> 'a[] -> 'a) =
        let regions = regions |> Seq.map (fun p -> p.Points, p.Attributes)
        PolygonTessellator.Triangulate(regions, rule, interpolate)
        
    [<Extension>]
    static member Triangulate (regions : seq<Polygon2d>, rule : TessellationRule) =
        let regions = regions |> Seq.map (fun p -> Seq.toArray p.Points)
        PolygonTessellator.Triangulate(regions, rule)




    [<Extension>]
    static member Combine (regions : seq<V3d[] * 'a[]>, rule : TessellationRule, interpolate : float[] -> 'a[] -> 'a) =
        let t = Tess()
                
        for (pts, att) in regions do
            let contour = LibTess.closed3 pts
            for i in 0 .. contour.Length - 1 do contour.[i].Data <- att.[i % att.Length]
            t.AddContour(contour)

        let combine (pos : Vec3) (data : obj[]) (weights : float[]) =
            interpolate weights (data |> Array.map unbox) :> obj

        t.Tessellate(unbox (int rule), ElementType.BoundaryContours, 2, CombineCallback(combine))
        
        if isNull t.Elements || isNull t.Vertices then
            []
        else
            let results = System.Collections.Generic.List<V3d[] * 'a[]>()

            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun v -> V3d(v.Position.X, v.Position.Y, v.Position.Z), unbox<'a> v.Data)

            for pi in 0 .. 2 .. indices.Length - 2 do
                let start = indices.[pi + 0]
                let count = indices.[pi + 1]

                let points = Array.zeroCreate count
                let att = Array.zeroCreate count
                for vi in 0 .. count - 1 do
                    let (p,a) = vertices.[start + vi]
                    points.[vi] <- p
                    att.[vi] <- a

                results.Add(points, att)

            CSharpList.toList results
         
    [<Extension>]
    static member Combine (regions : seq<V3d[]>, rule : TessellationRule) =
        let t = Tess()
                
        for pts in regions do
            let contour = LibTess.closed3 pts
            t.AddContour(contour)

        t.Tessellate(unbox (int rule), ElementType.BoundaryContours, 2)
        
        if isNull t.Elements || isNull t.Vertices then
            []
        else
            let results = System.Collections.Generic.List<V3d[]>()

            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun v -> V3d(v.Position.X, v.Position.Y, v.Position.Z))

            for pi in 0 .. 2 .. indices.Length - 2 do
                let start = indices.[pi + 0]
                let count = indices.[pi + 1]

                let points = Array.init count (fun vi -> vertices.[start + vi])
                results.Add(points)

            CSharpList.toList results

    [<Extension>]
    static member Combine (regions : seq<Polygon3d<'a>>, rule : TessellationRule, interpolate : float[] -> 'a[] -> 'a) =
        let regions = regions |> Seq.map (fun p -> p.Points, p.Attributes)
        PolygonTessellator.Combine(regions, rule, interpolate)
        
    [<Extension>]
    static member Combine (regions : seq<Polygon3d>, rule : TessellationRule) =
        let regions = regions |> Seq.map (fun p -> Seq.toArray p.Points)
        PolygonTessellator.Combine(regions, rule)
  
    [<Extension>]
    static member Triangulate (regions : seq<V3d[] * 'a[]>, rule : TessellationRule, interpolate : float[] -> 'a[] -> 'a) : list<Triangle3d<'a>> =
        let t = Tess()
                
        for (pts, att) in regions do
            let contour = LibTess.closed3 pts
            for i in 0 .. contour.Length - 1 do contour.[i].Data <- att.[i % att.Length]
            t.AddContour(contour)

        let combine (pos : Vec3) (data : obj[]) (weights : float[]) =
            interpolate weights (data |> Array.map unbox) :> obj

        t.Tessellate(unbox (int rule), ElementType.Polygons, 3, CombineCallback(combine))
        
        if isNull t.Elements || isNull t.Vertices then
            []
        else
            let results = System.Collections.Generic.List<Triangle3d<'a>>()

            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun v -> V3d(v.Position.X, v.Position.Y, v.Position.Z), unbox<'a> v.Data)

            for pi in 0 .. 3 .. indices.Length - 3 do
                let i0 = indices.[pi + 0]
                let i1 = indices.[pi + 1]
                let i2 = indices.[pi + 2]

                let (p0, a0) = vertices.[i0]
                let (p1, a1) = vertices.[i1]
                let (p2, a2) = vertices.[i2]

                results.Add(Triangle3d<'a>(p0, a0, p1, a1, p2, a2))


            CSharpList.toList results
    
    [<Extension>]
    static member Triangulate (regions : seq<V3d[]>, rule : TessellationRule) =
        let t = Tess()
                
        for pts in regions do
            let contour = LibTess.closed3 pts
            t.AddContour(contour)

        t.Tessellate(unbox (int rule), ElementType.Polygons, 3)
        
        if isNull t.Elements || isNull t.Vertices then
            []
        else
            let results = System.Collections.Generic.List<Triangle3d>()

            let indices = t.Elements
            let vertices = t.Vertices |> Array.map (fun v -> V3d(v.Position.X, v.Position.Y, v.Position.Z))

            for pi in 0 .. 3 .. indices.Length - 3 do
                let i0 = indices.[pi + 0]
                let i1 = indices.[pi + 1]
                let i2 = indices.[pi + 2]

                let p0 = vertices.[i0]
                let p1 = vertices.[i1]
                let p2 = vertices.[i2]

                results.Add(Triangle3d(p0, p1, p2))


            CSharpList.toList results
  
    [<Extension>]
    static member Triangulate (regions : seq<Polygon3d<'a>>, rule : TessellationRule, interpolate : float[] -> 'a[] -> 'a) =
        let regions = regions |> Seq.map (fun p -> p.Points, p.Attributes)
        PolygonTessellator.Triangulate(regions, rule, interpolate)
        
    [<Extension>]
    static member Triangulate (regions : seq<Polygon3d>, rule : TessellationRule) =
        let regions = regions |> Seq.map (fun p -> Seq.toArray p.Points)
        PolygonTessellator.Triangulate(regions, rule)
           

/// PolyRegion represents a set of non-overlapping, counterclockwise polygons surrounding
/// the intended region.
/// When constructed using a polygon the implementation enforces these invariants
type PolyRegion private(polygons : list<Polygon2d>) =
    static let empty = PolyRegion []
    
    static member Empty = empty
    static member Zero = empty

    member x.Polygons = polygons

    member x.Triangulate() =
        LibTess.triangulate WindingRule.EvenOdd polygons


    member x.Transformed(m : M33d) =
        PolyRegion (polygons |> List.map (fun p -> p.Transformed m))

    member x.BoundingBox =
        Box2d(polygons |> Seq.collect (fun p -> p.Points))

    interface IBoundingBox2d with
        member x.BoundingBox2d = x.BoundingBox

    member x.IsEmpty =
        List.isEmpty polygons

    static member Union(l : PolyRegion, r : PolyRegion) =
        LibTess.boundary WindingRule.Positive [l.Polygons; r.Polygons] |> PolyRegion

    static member Difference(l : PolyRegion, r : PolyRegion) =
        let rev = r.Polygons |> List.map (fun p -> p.Reversed)
        LibTess.boundary WindingRule.Positive [l.Polygons; rev] |> PolyRegion

    static member Intersection(l : PolyRegion, r : PolyRegion) =
        LibTess.boundary WindingRule.AbsGeqTwo [l.Polygons; r.Polygons] |> PolyRegion

    static member Xor(l : PolyRegion, r : PolyRegion) =
        LibTess.boundary WindingRule.EvenOdd [l.Polygons; r.Polygons] |> PolyRegion

    static member inline (^^^) (l : PolyRegion, r : PolyRegion) = PolyRegion.Xor(l,r)
    static member inline (*) (l : PolyRegion, r : PolyRegion) = PolyRegion.Intersection(l,r)
    static member inline (+) (l : PolyRegion, r : PolyRegion) = PolyRegion.Union(l,r)
    static member inline (-) (l : PolyRegion, r : PolyRegion) = PolyRegion.Difference(l,r)
    
    member x.Contains(pt : V2d) =
        polygons |> Seq.exists (fun p ->
            p.Contains pt
        )

    member x.Contains(contained : PolyRegion) =
        // TODO: better implementation possible
        (contained - x).IsEmpty

    member x.Contains(b : Box2d) =
        // TODO: better implementation possible
        x.Contains(PolyRegion(b.ToPolygon2dCCW()))

    member x.Overlaps(other : PolyRegion) =
        // TODO: better implementation possible
        (other * x).IsEmpty |> not


    new(p : Polygon2d, tess : TessellationRule) =
        if p.PointCount < 3 then
            PolyRegion []
        else
            let res = LibTess.boundary (unbox (int tess)) [[p]]
            let res = res |> List.map (fun p -> if p.IsCcw() then p else p.Reversed)
            PolyRegion(res)

    new(p : Polygon2d) =
        PolyRegion(p, TessellationRule.EvenOdd)
            

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PolyRegion =
    let empty = PolyRegion.Empty

    let inline ofPolygon (p : Polygon2d) = PolyRegion p
    let inline ofSeq (p : seq<V2d>) = p |> Polygon2d |> ofPolygon
    let inline ofList (p : list<V2d>) = p |> Polygon2d |> ofPolygon
    let inline ofArray (p : array<V2d>) = p |> Polygon2d |> ofPolygon
    let inline ofBox (b : Box2d) = b.ToPolygon2dCCW() |> ofPolygon
    
    let inline transformed (m : M33d) (r : PolyRegion) = r.Transformed m
    let inline bounds (r : PolyRegion) = r.BoundingBox

    let inline toPolygons (r : PolyRegion) = r.Polygons
    let inline toTriangles (r : PolyRegion) = r.Triangulate()

    let inline union (l : PolyRegion) (r : PolyRegion) = l + r
    let inline intersection (l : PolyRegion) (r : PolyRegion) = l * r
    let inline xor (l : PolyRegion) (r : PolyRegion) = l ^^^ r
    let inline difference (l : PolyRegion) (r : PolyRegion) = l - r

    let unionMany (l : seq<PolyRegion>) = l |> Seq.fold union empty
    let intersectMany (l : seq<PolyRegion>) = l |> Seq.fold intersection empty

    let inline containsPoint (pt : V2d) (r : PolyRegion) = r.Contains pt
    let inline containsRegion (l : PolyRegion) (r : PolyRegion) = l.Contains r
    let inline overlaps (l : PolyRegion) (r : PolyRegion) = l.Overlaps r

    let inline isEmpty (r : PolyRegion) = r.IsEmpty

    let private viewVolume = Box3d(-V3d.III, V3d.III)
    
    let internal toHull3d (viewProj : Trafo3d) =
        let r0 = viewProj.Forward.R0
        let r1 = viewProj.Forward.R1
        let r2 = viewProj.Forward.R2
        let r3 = viewProj.Forward.R3

        let inline toPlane (v : V4d) =
            Plane3d(-v.XYZ, v.W)

        Hull3d [|
            r3 - r0 |> toPlane  // right
            r3 + r0 |> toPlane  // left
            r3 + r1 |> toPlane  // bottom
            r3 - r1 |> toPlane  // top
            r3 + r2 |> toPlane  // near
            //r3 - r2 |> toPlane  // far
        |]

    let ofProjectedBox (viewProj : Trafo3d) (b : Box3d) =
        let p000 = V3d(b.Min.X, b.Min.Y, b.Min.Z)
        let p001 = V3d(b.Min.X, b.Min.Y, b.Max.Z)
        let p010 = V3d(b.Min.X, b.Max.Y, b.Min.Z)
        let p011 = V3d(b.Min.X, b.Max.Y, b.Max.Z)
        let p100 = V3d(b.Max.X, b.Min.Y, b.Min.Z)
        let p101 = V3d(b.Max.X, b.Min.Y, b.Max.Z)
        let p110 = V3d(b.Max.X, b.Max.Y, b.Min.Z)
        let p111 = V3d(b.Max.X, b.Max.Y, b.Max.Z)
                
        let hull = toHull3d viewProj

        let clippedByPlane (eps : float) (plane : Plane3d) (poly : V3d[]) =
            let cnt = poly.Length
            if cnt < 3 then
                [||]
            else
                let result = System.Collections.Generic.List<V3d>()

                let mutable p0 = poly.[cnt - 1]
                let mutable h0 = plane.Height p0
                let mutable h0p = h0 > eps
                let mutable h0n = h0 < -eps

                for i in 0 .. cnt - 1 do
                    let p1 = poly.[i]
                    let h1 = plane.Height p1
                    let h1p = h1 > eps
                    let h1n = h1 < -eps

                    if h0p && h1n || h0n && h1p then
                        // h0 * (1 - t) + h1 * t = 0
                        // h0 - h0*t + h1 * t = 0
                        // h0 = (h0 - h1) * t
                        // h0 / (h0 - h1) = t
                        let t = h0 / (h0 - h1)
                        result.Add(p0 + (p1 - p0) * t)

                    if not h1p then 
                        result.Add p1

                    p0 <- p1
                    h0 <- h1
                    h0p <- h1p
                    h0n <- h1n

                if result.Count > 2 then
                    CSharpList.toArray result
                else
                    [||]

        let clippedByHull (eps : float) (hull : Hull3d) (polygon : V3d[]) =
            hull.PlaneArray |> Array.fold (flip (clippedByPlane eps)) polygon

        let projectConvex (arr : V3d[]) =

            let poly =
                arr |> clippedByHull Constant.PositiveTinyValue hull
                    |> Array.map (viewProj.Forward.TransformPosProj >> Vec.xy)
                    |> (fun a -> if a.Length = 0 then None else Some a)
            
            match poly with
            | None -> empty
            | Some poly ->
                let center = (poly |> Array.fold (+) V2d.Zero) / float poly.Length
                poly |> Array.map (fun p -> 1.000001 * (p - center) + center)
                     |> ofArray

        unionMany 
            [
                [| p000; p001; p011; p010|] |> projectConvex
                [| p100; p101; p111; p110|] |> projectConvex
                        
                [| p000; p001; p101; p100|] |> projectConvex
                [| p010; p011; p111; p110|] |> projectConvex

                [| p000; p100; p110; p010|] |> projectConvex
                [| p001; p101; p111; p011|] |> projectConvex
            ]
            