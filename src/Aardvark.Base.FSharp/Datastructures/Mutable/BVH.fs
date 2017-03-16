namespace Aardvark.Base

open System


type private BvhNode =
    struct
        val mutable public LeftBox : Box3d
        val mutable public RightBox : Box3d
        val mutable public Left : int
        val mutable public Right : int
        val mutable public Parent : int
        val mutable public Weight : float
    end

type private BvhSplit =
    {
        leftCount : int
        rightCount : int
        sortedIndexArray : int[]
        leftBox : Box3d
        rightBox : Box3d
    }

type private BvhData =
    {
        box : Box3d
        boxes : Box3d[]
        weights : float[]
        nodes : BvhNode[]
        leafParentArray : int[]
        mutable nodeCount : int

    }

type RayPart =
    struct
        val mutable public Ray  : FastRay3d
        val mutable public TMin : float
        val mutable public TMax : float
        
        new(ray, min, max) = { Ray = ray; TMin = min; TMax = max }
        new(ray) = { Ray = ray; TMin = 0.0; TMax = Double.PositiveInfinity }
    end

type RayHit<'r> =
    struct
        val mutable public T : float
        val mutable public Value : 'r

        new(t, value) = { T = t; Value = value }
    end

type Bvh<'a>(boxes : Box3d[], values : 'a[], weight : 'a -> float) =

    static let empty = Bvh<'a>([||], [||], fun _ -> 1.0)

    [<Literal>]
    static let splitPenalty = 1.0

    static let cost (lBox : Box3d) (rBox : Box3d) (lWeight : float) (rWeight : float) (invBoxArea : float) =
        let cBox = Box3d.Intersection(lBox, rBox)
        let cSize =
            V3d(
                (if cBox.Max.X > cBox.Min.X then cBox.Max.X - cBox.Min.X else 0.0),
                (if cBox.Max.Y > cBox.Min.Y then cBox.Max.Y - cBox.Min.Y else 0.0),
                (if cBox.Max.Z > cBox.Min.Z then cBox.Max.Z - cBox.Min.Z else 0.0)
            )

        let leftP = lBox.SurfaceArea * invBoxArea
        let rightP = rBox.SurfaceArea * invBoxArea
        let commonP = 2.0 * (cSize.X * cSize.Y + cSize.X * cSize.Z + cSize.Y * cSize.Z) * invBoxArea

        (leftP + (commonP * splitPenalty)) * (1.0 + lWeight) +
        (rightP + (commonP * splitPenalty)) * (1.0 + rWeight)

    static let calculateSplit (indexArray : int[]) (start : int) (count : int) (box : Box3d) (data : BvhData)  =
        let boxes = data.boxes
        let weights = data.weights
        let mutable bestLeft = 0
        let mutable bestCost = Double.PositiveInfinity
        let mutable bestLeftBox = Box3d.Invalid
        let mutable bestRightBox = Box3d.Invalid
        let mutable bestDim = -1
        let invBoxArea = 1.0 / box.SurfaceArea

        let iaa = Array.init 3 (fun _ -> Array.sub indexArray start count)

        for d in 0 .. 2 do
            let ia = iaa.[d]
            ia.PermutationQuickSortAscending(boxes, fun ba i -> ba.[int i].Center.[d])

            let bbLeftArray = ia.ScanLeft(Box3d.Invalid, fun b i -> Box3d(b, boxes.[i]))
            let bbRightArray = ia.ScanRight((fun i b -> Box3d(boxes.[i], b)), Box3d.Invalid)

            let wLeftArray, wRightArray =
                if isNull weights then
                    null, null
                else
                    ia.ScanLeft(0.0, fun c i -> c + weights.[i]),
                    ia.ScanRight((fun i c -> c + weights.[i]), 0.0)

            for s in 1 .. count - 1 do
                let lBox = bbLeftArray.[s-1]
                let rBox = bbRightArray.[s]

                let lWeight = if isNull wLeftArray then float s else wLeftArray.[s - 1]
                let rWeight = if isNull wRightArray then float (count - s) else wRightArray.[s]

                let cost = cost lBox rBox lWeight rWeight invBoxArea
                if cost < bestCost then
                    bestCost <- cost
                    bestDim <- d
                    bestLeft <- s
                    bestLeftBox <- lBox
                    bestRightBox <- rBox
            
        {
            leftCount = bestLeft
            rightCount = count - bestLeft
            sortedIndexArray = iaa.[bestDim]
            leftBox = bestLeftBox
            rightBox = bestRightBox
        }

    static let rec create (data : BvhData) (indices : int[]) (start : int) (count : int) (box : Box3d) : int =
        let ni = data.nodeCount
        data.nodeCount <- data.nodeCount + 1

        let split = calculateSplit indices start count box data

        let left, leftWeight =
            if split.leftCount > 1 then
                let left = create data split.sortedIndexArray 0 split.leftCount split.leftBox
                data.nodes.[left].Parent <- ni
                left, data.nodes.[left].Weight
            else
                let li = split.sortedIndexArray.[0]
                data.leafParentArray.[li] <- ni
                let left = -1 - li
                left, data.weights.[li]

        let right, rightWeight =
            if split.rightCount > 1 then
                let right = create data split.sortedIndexArray split.leftCount split.rightCount split.rightBox
                data.nodes.[right].Parent <- ni
                right, data.nodes.[right].Weight
            else
                let ri = split.sortedIndexArray.[count - 1]
                data.leafParentArray.[ri] <- ni
                let right = -1 - ri
                right, data.weights.[ri]


        let a = &data.nodes.[ni]
        a.Left <- left
        a.Right <- right
        a.LeftBox <- split.leftBox
        a.RightBox <- split.rightBox
        a.Weight <- leftWeight + rightWeight

        ni


    let data =
        {
            box = Box3d(boxes)
            boxes = boxes
            weights = values |> Array.map weight
            nodes = if boxes.Length > 1 then Array.zeroCreate (boxes.Length - 1) else null
            leafParentArray = Array.zeroCreate boxes.Length
            nodeCount = 0
        }

    do if boxes.Length > 1 then
        create data (Array.init boxes.Length id) 0 boxes.Length data.box |> ignore
        data.nodes.[0].Parent <- -1


    let rec intersectNode (node : int) (bounds : Box3d) (ray : FastRay3d) (tmin : float) (tmax : float) (tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>) =
        if node < 0 then
            // leaf
            let node = -1 - node
            tryIntersect (RayPart(ray, tmin, tmax)) values.[node]
        else
            let node = data.nodes.[node]
            
            let mutable lmin = tmin
            let mutable lmax = tmax
            let mutable rmin = tmin
            let mutable rmax = tmax

            let il = ray.Intersects(node.LeftBox, &lmin, &lmax)
            let ir = ray.Intersects(node.LeftBox, &rmin, &rmax)


            match il, ir with
                | true, true ->
                    // both intersect
                    if lmin < rmin then
                        // left node is closer to the origin
                        match intersectNode node.Left node.LeftBox ray lmin lmax tryIntersect with
                            | Some hit -> 
                                if hit.T > rmin then
                                    // the left hit lies inside the right box.
                                    // therefore there may exist a hit in (rmin, hit.T) that is closer
                                    match intersectNode node.Right node.RightBox ray rmin hit.T tryIntersect with
                                        | Some hit -> Some hit
                                        | None -> Some hit

                                else
                                    Some hit

                            | None ->
                                intersectNode node.Right node.RightBox ray rmin rmax tryIntersect
                    else
                        match intersectNode node.Right node.RightBox ray rmin rmax tryIntersect with
                            | Some hit -> 
                                if hit.T > lmin then
                                    // the right hit lies inside the left box.
                                    //therefore there may exist a hit in (lmin, hit.T) that is closer
                                    match intersectNode node.Left node.LeftBox ray lmin hit.T tryIntersect with
                                        | Some hit -> Some hit
                                        | None -> Some hit
                                else
                                    Some hit
                            | None ->
                                intersectNode node.Left node.LeftBox ray lmin lmax tryIntersect

                | true, false ->
                    // only left intersects
                    intersectNode node.Left node.LeftBox ray lmin lmax tryIntersect
                | false, true ->
                    // only right intersects
                    intersectNode node.Right node.RightBox ray rmin rmax tryIntersect

                | false, false ->
                    // none intersects
                    None

    static member Empty = empty

    member x.Intersect(ray : FastRay3d, tmin : float, tmax : float, tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>) =
        if boxes.Length = 0 then
            // empty tree
            None
        else
            let mutable tmin = tmin
            let mutable tmax = tmax
            if ray.Intersects(data.box, &tmin, &tmax) then
                if boxes.Length = 1 then    
                    // single element
                    tryIntersect (RayPart(ray, tmin, tmax)) values.[0]
                else
                    // deep tree
                    intersectNode 0 data.box ray tmin tmax tryIntersect
            else
                None

    member x.Intersect(hull : FastHull3d) =
        let rec allNodesAcc (res : System.Collections.Generic.List<'a * Box3d>) (box : Box3d) (n : int) : unit =
            if n < 0 then
                let i = -1 - n
                res.Add (values.[i], box)
            else
                let node = data.nodes.[n]
                if hull.Intersects(node.LeftBox) then
                    allNodesAcc res node.LeftBox node.Left
                
                if hull.Intersects(node.RightBox) then
                    allNodesAcc res node.RightBox node.Right


        if values.Length = 0 then 
            []

        elif hull.Intersects(data.box) then
            if values.Length = 1 then 
                [values.[0], data.box]
            else
                let res = System.Collections.Generic.List<'a * Box3d>()
                allNodesAcc res data.box 0
                CSharpList.toList res
                
        else
            []

    member x.IntersectViewProj(projection : M44d) =
        let r0 = projection.R0
        let r1 = projection.R1
        let r2 = projection.R2
        let r3 = projection.R3

        let inline plane(v : V4d) = Plane3d(v.XYZ, -v.W)

        let hull =
            Hull3d [|
                plane (r3 + r0) // left
                plane (r3 - r0) // right
                plane (r3 + r1) // top
                plane (r3 - r1) // bottom
                plane (r3 + r2) // near
                plane (r3 - r2) // far
            |]

        x.Intersect(FastHull3d hull)


    member x.AsList =
        Seq.zip values boxes |> Seq.toList

    member x.AsArray =
        Array.zip values boxes

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() = new BvhEnumerator<_>(boxes, values) :> _

    interface System.Collections.Generic.IEnumerable<'a * Box3d> with
        member x.GetEnumerator() = new BvhEnumerator<_>(boxes, values) :> _


    new(boxes : Box3d[], values : 'a[]) =
        Bvh<'a>(boxes, values, fun _ -> 1.0)
        
and private BvhEnumerator<'a>(boxes : Box3d[], values : 'a[]) =
    let mutable index = -1

    member x.MoveNext() =
        index <- index + 1
        index < boxes.Length

    member x.Current =
        values.[index], boxes.[index]

    interface System.Collections.IEnumerator with
        member x.MoveNext() = x.MoveNext()
        member x.Reset() = index <- -1
        member x.Current = x.Current :> obj

    interface System.Collections.Generic.IEnumerator<'a * Box3d> with
        member x.Current = x.Current
        member x.Dispose() = index <- -1

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module Bvh =

    let empty<'a> = Bvh<'a>.Empty

    let singleton (value : 'a) (bounds : Box3d) =
        Bvh<'a>([|bounds|], [|value|], fun _ -> 1.0)

    let ofSeq (data : seq<'a * Box3d>) =
        let values = data |> Seq.map fst |> Seq.toArray
        let boxes = data |> Seq.map snd |> Seq.toArray
        Bvh<'a>(boxes, values)

    let ofList (data : list<'a * Box3d>) =
        let values = data |> List.map fst |> List.toArray
        let boxes = data |> List.map snd |> List.toArray
        Bvh<'a>(boxes, values)
        
    let ofArray (data : array<'a * Box3d>) =
        let values, boxes = data |> Array.unzip
        Bvh<'a>(boxes, values)

    let toSeq (bvh : Bvh<'a>) =
        bvh :> seq<_>

    let toList (bvh : Bvh<'a>) =
        bvh.AsList

    let toArray (bvh : Bvh<'a>) =
        bvh.AsArray

    let intersectFull (intersectObject : RayPart -> 'a -> Option<RayHit<'r>>) (ray : Ray3d) (tmin : float) (tmax : float) (bvh : Bvh<'a>) =
        bvh.Intersect(FastRay3d(ray), tmin, tmax, intersectObject)
        
    let intersectNode (ray : Ray3d) (tmin : float) (tmax : float) (bvh : Bvh<'a>) =
        bvh.Intersect(FastRay3d(ray), tmin, tmax, fun p a -> Some(RayHit(p.TMin, a)))
        
    let cull (hull : Hull3d) (bvh : Bvh<'a>) =
        bvh.Intersect(FastHull3d hull)

    let cullViewProj (viewProj : M44d) (bvh : Bvh<'a>) =
        bvh.IntersectViewProj viewProj