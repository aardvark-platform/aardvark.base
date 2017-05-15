namespace Aardvark.Base.Geometry

open Aardvark.Base

type KdBuildInfo =
    struct
        val mutable public MaxCount : int
        val mutable public Splits : int

        static member Default = KdBuildInfo(50, 3)
        static member High = KdBuildInfo(50, 5)

        new(cnt, splits) = { MaxCount = cnt; Splits = splits }

    end

[<RequireQualifiedAccess>]
type KdNode =
    | Empty
    | Leaf of int[]
    | Node of Plane3d * KdNode * KdNode
 
[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module KdNode = 

    type private Split =
        struct
            val mutable public LeftCount    : int
            val mutable public RightCount   : int
            val mutable public BothCount    : int
            val mutable public Plane        : Plane3d
            val mutable public Cost         : float
        end

    let private count (spatial : Spatial<'a>) (plane : Plane3d) (indices : int[]) (data : 'a[]) =
        let mutable left = 0
        let mutable right = 0
        let mutable both = 0

        for i in indices do
            let side = spatial.PlaneSide(plane, data.[i])
            if side &&& PlaneSide.Both = PlaneSide.Both then
                inc &both

            elif side &&& PlaneSide.Above = PlaneSide.Above then
                inc &right

            elif side &&& PlaneSide.Below = PlaneSide.Below then
                inc &left

                     
        (left, both, right)

    let private cost (parent : Box3d) (lBox : Box3d) (rBox : Box3d) (left : int) (both : int) (right : int) =
        let sl = lBox.SurfaceArea
        let sr = rBox.SurfaceArea
        let sp = parent.SurfaceArea

        sl * (float left + 2.0 * float both) +
        sr * (float right + 2.0 * float both)

    let rec build (spatial : Spatial<'a>) (info : KdBuildInfo) (data : 'a[]) (bounds : Box3d) (indices : int[]) =
        if indices.Length = 0 then
            KdNode.Empty
        elif indices.Length < info.MaxCount then
            KdNode.Leaf indices
        else
            let mutable best = Split(Cost = System.Double.PositiveInfinity)

            let size = bounds.Size

            for d in 0 .. 2 do
                let mutable n = V3d.Zero
                n.[d] <- 1.0

                let s = size.[d] / float (info.Splits + 1)
                let mutable o = bounds.Min.[d] + s
                let mutable lBox = bounds
                let mutable rBox = bounds

                for _ in 1 .. info.Splits do
                    let p = Plane3d(n, o)
                    lBox.Max.[d] <- o
                    rBox.Min.[d] <- o

                    let l,b,r = count spatial p indices data

                    let cost = cost bounds lBox rBox l b r

                    if cost < best.Cost then
                        best.LeftCount <- l
                        best.BothCount <- b
                        best.RightCount <- r
                        best.Cost <- cost
                        best.Plane <- p

                    o <- o + s


            if best.BothCount = indices.Length then
                KdNode.Leaf indices
            else
                     
                let mutable lBox = bounds
                let mutable rBox = bounds
                lBox.Max.[best.Plane.Normal.MajorDim] <- best.Plane.Distance
                rBox.Min.[best.Plane.Normal.MajorDim] <- best.Plane.Distance

                let l = Array.zeroCreate (best.LeftCount + best.BothCount)
                let r = Array.zeroCreate (best.RightCount + best.BothCount)
                let mutable li = 0
                let mutable ri = 0

                for i in indices do
                    let side = spatial.PlaneSide(best.Plane, data.[i])

                    if (side &&& PlaneSide.Below) <> PlaneSide.None then
                        l.[li] <- i
                        inc &li

                    if (side &&& PlaneSide.Above) <> PlaneSide.None then
                        r.[ri] <- i
                        inc &ri

                                

                let left = build spatial info data lBox l
                let right = build spatial info data rBox r

                KdNode.Node(best.Plane, left, right)

    let rec intersect (tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>) (data : 'a[]) (part : RayPart) (node : KdNode) =
        if part.TMax < part.TMin then
            None
        else
            match node with
                | KdNode.Empty -> 
                    None

                | KdNode.Leaf indices ->
                    let hits = indices |> Seq.choose (Array.get data >> tryIntersect part) |> Seq.toList
                    match hits with
                        | [] -> None
                        | hits -> hits |> List.minBy (fun h -> h.T) |> Some

                | KdNode.Node(plane, l, r) ->
                    let inline intersect p n = intersect tryIntersect data p n
                    let mutable t = 0.0
                    let mutable p = V3d.Zero

                    let dir = Vec.dot part.Ray.Ray.Direction plane.Normal
                    part.Ray.Ray.Intersects(plane, &t, &p) |> ignore

                    if dir > 0.0 then
                        if t >= part.TMin && t <= part.TMax then
                            match intersect (RayPart(part.Ray, part.TMin, t)) l with
                                | Some hit -> 
                                    Some hit
                                | None ->
                                    intersect (RayPart(part.Ray, t, part.TMax)) r

                        elif t < part.TMin then
                            intersect part r

                        else (* t > part.TMax *)
                            intersect part l
                            
                    elif dir < 0.0 then
                        if t >= part.TMin && t <= part.TMax then
                            match intersect (RayPart(part.Ray, part.TMin, t)) r with
                                | Some hit -> 
                                    Some hit
                                | None ->
                                    intersect (RayPart(part.Ray, t, part.TMax)) l

                        elif t < part.TMin then
                            intersect part l

                        else (* t > part.TMax *)
                            intersect part r

                    else
                        let h = plane.Height part.Ray.Ray.Origin
                        if h > 0.0 then intersect part r
                        else intersect part l


type KdTree<'a>(data : 'a[], root : KdNode, bounds : Box3d) =
    member x.Bounds = bounds
    member x.Data = data
    member x.Root = root

    member x.Intersect(tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>, ray : RayPart) : Option<RayHit<'r>> =
        let mutable ray = ray
        if ray.Ray.Intersects(bounds, &ray.TMin, &ray.TMax) then
            KdNode.intersect tryIntersect data ray root
        else
            None

    interface IRayIntersectable<'a> with
        member x.Intersect(tryIntersect, ray) = x.Intersect(tryIntersect, ray)

    new(spatial : Spatial<'a>, info : KdBuildInfo, data : 'a[]) =
        let bounds = spatial.ComputeBounds (data :> seq<_>)
        let root = KdNode.build spatial info data bounds (Array.init data.Length id)
        KdTree(data, root, bounds)

    new(spatial : Spatial<'a>, data : 'a[]) =
        KdTree(spatial, KdBuildInfo.Default, data)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module KdTree =

    let inline bounds (tree : KdTree<'a>) =
        tree.Bounds
        
    let inline root (tree : KdTree<'a>) =
        tree.Root

    let inline data (tree : KdTree<'a>) =
        tree.Data

    let inline build (spatial : Spatial<'a>) (info : KdBuildInfo) (data : 'a[]) =
        KdTree(spatial, info, data)
            
    let inline intersect (tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>) (ray : RayPart) (tree : KdTree<'a>) =
        tree.Intersect(tryIntersect, ray)