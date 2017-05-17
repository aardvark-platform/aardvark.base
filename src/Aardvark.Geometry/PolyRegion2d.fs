namespace Aardvark.Geometry

open System
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

            let results = 
                List.init (indices.Length / 2) (fun pi ->
                    let start = indices.[2 * pi + 0]
                    let count = indices.[2 * pi + 1]
//                    
//                    let mutable dir = V2d.NaN
//                    let mutable last = vertices.[start + count - 1]
//                    for i in 0 .. count - 1 do
//                        let current = vertices.[start + i]
//                        let d = current - last
//                        V2d.Dot(d, dir)
//
//
//


                    Polygon2d (Array.init count (fun vi -> vertices.[start + vi]))
                )

            results

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


type PolyRegion private(polygons : list<Polygon2d>) =
    static let empty = PolyRegion []
    
    static member Empty = empty
    static member Zero = empty

    member x.polygons = polygons

    member x.Triangulate() =
        LibTess.triangulate WindingRule.EvenOdd polygons

    member x.Contains(pt : V2d) =
        polygons |> Seq.exists (fun p ->
            p.Contains pt
        )

    member x.Transformed(m : M33d) =
        PolyRegion (polygons |> List.map (fun p -> p.Transformed m))

    member x.IsEmpty =
        List.isEmpty polygons

    static member Union(l : PolyRegion, r : PolyRegion) =
        LibTess.boundary WindingRule.Positive [l.polygons; r.polygons] |> PolyRegion

    static member Difference(l : PolyRegion, r : PolyRegion) =
        let rev = r.polygons |> List.map (fun p -> p.Reversed)
        LibTess.boundary WindingRule.Positive [l.polygons; rev] |> PolyRegion

    static member Intersection(l : PolyRegion, r : PolyRegion) =
        LibTess.boundary WindingRule.AbsGeqTwo [l.polygons; r.polygons] |> PolyRegion

    static member Xor(l : PolyRegion, r : PolyRegion) =
        LibTess.boundary WindingRule.EvenOdd [l.polygons; r.polygons] |> PolyRegion

    static member inline (^^^) (l : PolyRegion, r : PolyRegion) = PolyRegion.Xor(l,r)
    static member inline (*) (l : PolyRegion, r : PolyRegion) = PolyRegion.Intersection(l,r)
    static member inline (+) (l : PolyRegion, r : PolyRegion) = PolyRegion.Union(l,r)
    static member inline (-) (l : PolyRegion, r : PolyRegion) = PolyRegion.Difference(l,r)

    member x.Contains(contained : PolyRegion) =
        (contained - x).IsEmpty

    member x.Overlaps(other : PolyRegion) =
        (other * x).IsEmpty |> not

    new(p : Polygon2d) =
        let res = LibTess.boundary WindingRule.EvenOdd [[p]]
        let res = res |> List.map (fun p -> if p.IsCcw() then p else p.Reversed)
        PolyRegion(res)
            

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module PolyRegion =
    let empty = PolyRegion.Empty

    let inline ofPolygon (p : Polygon2d) = PolyRegion p
    let inline ofSeq (p : seq<V2d>) = p |> Polygon2d |> ofPolygon
    let inline ofList (p : list<V2d>) = p |> Polygon2d |> ofPolygon
    let inline ofArray (p : array<V2d>) = p |> Polygon2d |> ofPolygon
    let inline ofBox (b : Box2d) = b.ToPolygon2dCCW() |> ofPolygon
    
    let inline transformed (m : M33d) (r : PolyRegion) = r.Transformed m

    let inline toPolygons (r : PolyRegion) = r.polygons
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
