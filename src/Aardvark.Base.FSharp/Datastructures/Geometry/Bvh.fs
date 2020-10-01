﻿namespace Aardvark.Base.Geometry

open Aardvark.Base

[<RequireQualifiedAccess>]
type BvhNode =
    | Leaf of id : int
    | Node of lBox : Box3d * rBox : Box3d * left : BvhNode * right : BvhNode

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module BvhNode =

    open Aardvark.Base.Sorting

    [<Literal>]
    let private splitPenalty = 1.0

    type private BvhSplit =
        {
            leftCount : int
            rightCount : int
            sortedIndexArray : int[]
            leftBox : Box3d
            rightBox : Box3d
        }


    let private cost (lBox : Box3d) (rBox : Box3d) (lWeight : float) (rWeight : float) (invBoxArea : float) =
        let cBox = Box.Intersection(lBox, rBox)
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

    let private calculateSplit (indexArray : int[]) (start : int) (count : int) (box : Box3d) (boxes : Box3d[]) =
        let mutable bestLeft = 0
        let mutable bestCost = System.Double.PositiveInfinity
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


            for s in 1 .. count - 1 do
                let lBox = bbLeftArray.[s-1]
                let rBox = bbRightArray.[s]

                let lWeight = float s
                let rWeight = float (count - s)

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
        
    let rec build (boxes : Box3d[]) (indices : int[]) (start : int) (count : int) (box : Box3d) =
        if count = 1 then
            BvhNode.Leaf(indices.[start])
        else
            let split = calculateSplit indices start count box boxes
            let left = build boxes split.sortedIndexArray 0 split.leftCount split.leftBox
            let right = build boxes split.sortedIndexArray split.leftCount split.rightCount split.rightBox
            BvhNode.Node(split.leftBox, split.rightBox, left, right)

    let rec intersect (tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>) (data : 'a[]) (part : RayPart) (node : BvhNode) =
        match node with
            | BvhNode.Leaf id ->
                tryIntersect part data.[id]

            | BvhNode.Node(lBox, rBox, left, right) ->
                let inline intersect part node = intersect tryIntersect data part node
                
                let mutable lpart = part
                let mutable rpart = part

                let il = lpart.Ray.Intersects(lBox, &lpart.TMin, &lpart.TMax)
                let ir = lpart.Ray.Intersects(rBox, &rpart.TMin, &rpart.TMax)

                match il, ir with   
                    | false, false -> 
                        None

                    | true, false ->
                        intersect lpart left

                    | false, true ->
                        intersect rpart right

                    | true, true ->
                        if lpart.TMin < rpart.TMin then
                            match intersect lpart left with 
                                | Some hit ->
                                    if hit.T > rpart.TMin then
                                        rpart.TMax <- hit.T
                                        match intersect rpart right with
                                            | Some hit -> Some hit
                                            | None -> Some hit
                                    else
                                        Some hit
                                | None ->
                                    intersect rpart right
                        else 
                            match intersect rpart right with
                                | Some hit ->
                                    if hit.T > lpart.TMin then
                                        lpart.TMax <- hit.T
                                        match intersect lpart left with
                                            | Some hit -> Some hit
                                            | None -> Some hit
                                    else
                                        Some hit
                                | None ->
                                    intersect lpart left


    module Seq = 
        open System.Collections.Generic

        let rec private mergeSortedAux (cmp : 'a -> 'a -> int) (cl : Option<'a>) (cr : Option<'a>) (l : IEnumerator<'a>) (r : IEnumerator<'a>) =
            seq {
                let cl =
                    match cl with
                        | Some c -> Some c
                        | None -> 
                            if l.MoveNext() then Some l.Current
                            else None

                let cr = 
                    match cr with
                        | Some c -> Some c
                        | None -> 
                            if r.MoveNext() then Some r.Current
                            else None

                match cl, cr with
                    | None, None -> 
                        ()
                    | Some cl, None -> 
                        yield cl
                        yield! mergeSortedAux cmp None None l r

                    | None, Some cr ->
                        yield cr
                        yield! mergeSortedAux cmp None None l r

                    | Some vl, Some vr ->
                        let c = cmp vl vr
                        if c < 0 then
                            yield vl
                            yield! mergeSortedAux cmp None cr l r
                        else
                            yield vr
                            yield! mergeSortedAux cmp cl None l r

            }

        let mergeSorted (cmp : 'a -> 'a -> int) (l : seq<'a>) (r : seq<'a>) =
            seq {
                use le = l.GetEnumerator()
                use re = r.GetEnumerator()
                yield! mergeSortedAux cmp None None le re

            }

    let rec intersections (tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>) (data : 'a[]) (part : RayPart) (node : BvhNode) =
        match node with
            | BvhNode.Leaf id ->
                match tryIntersect part data.[id] with
                    | Some h -> Seq.singleton h
                    | None -> Seq.empty

            | BvhNode.Node(lBox, rBox, left, right) ->
                let mutable lpart = part
                let mutable rpart = part

                let il = lpart.Ray.Intersects(lBox, &lpart.TMin, &lpart.TMax)
                let ir = lpart.Ray.Intersects(rBox, &rpart.TMin, &rpart.TMax)

                match il, ir with   
                    | false, false -> 
                        Seq.empty

                    | true, false ->
                        intersections tryIntersect data lpart left

                    | false, true ->
                        intersections tryIntersect data rpart right

                    | true, true ->
                        seq {

                            if lpart.TMax <= rpart.TMin then
                                // l strictly before r
                                yield! intersections tryIntersect data lpart left
                                yield! intersections tryIntersect data rpart right

                            elif rpart.TMax <= lpart.TMin then
                                // r strictly before l then
                                yield! intersections tryIntersect data rpart right
                                yield! intersections tryIntersect data lpart left

                            else
                                let li = intersections tryIntersect data lpart left
                                let ri = intersections tryIntersect data rpart right
                                yield! Seq.mergeSorted (fun (lh : RayHit<_>) (rh : RayHit<_>) -> compare lh.T rh.T) li ri
                        }
                            

    let cull (hull : FastHull3d) (node : BvhNode) =
        let rec cullAcc (hull : FastHull3d) (res : System.Collections.Generic.List<int>) (node : BvhNode) =
            match node with
                | BvhNode.Leaf id -> 
                    res.Add id

                | BvhNode.Node(lBox, rBox, left, right) ->
                    if hull.Intersects(lBox) then
                        cullAcc hull res left

                    if hull.Intersects(rBox) then
                        cullAcc hull res right
        let res = System.Collections.Generic.List()
        cullAcc hull res node
        res |> CSharpList.toList
                    

type BvhTree<'a>(data : 'a[], bounds : Box3d, root : Option<BvhNode>) =
    member x.Data = data
    member x.Root = root
    member x.Bounds = bounds

    member x.Intersect (tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>, part : RayPart) =
        match root with
            | Some root ->
                let mutable part = part
                if part.Ray.Intersects(bounds, &part.TMin, &part.TMax) then
                    BvhNode.intersect tryIntersect data part root
                else
                    None
            | None ->
                None

    member x.Intersections (tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>, part : RayPart) =
        match root with
            | Some root ->
                let mutable part = part
                if part.Ray.Intersects(bounds, &part.TMin, &part.TMax) then
                    BvhNode.intersections tryIntersect data part root
                else
                    Seq.empty
            | None ->
                Seq.empty

    member x.Cull (hull : FastHull3d) =
        match root with
            | Some root ->
                if hull.Intersects(bounds) then
                    BvhNode.cull hull root |> List.map (Array.get data)
                else
                    []
            | None ->
                []

    interface IRayIntersectable<'a> with
        member x.Intersect(tryIntersect, ray) = x.Intersect(tryIntersect, ray)

    interface ICullable<'a> with
        member x.Cull hull = x.Cull hull

    new(data : array<'a * Box3d>) =
        let data = 
            data |> Array.choose (fun (k, b) ->
                if b.IsValid then   
                    let nb = Box3d.FromCenterAndSize(b.Center, b.Size + V3d(1E-8, 1E-8, 1E-8))
                    Some (k, nb)
                else
                    None
            )
        if data.Length > 0 then
            let data, boxes = data |> Array.unzip
            let bounds = Box3d(boxes)
            let root = BvhNode.build boxes (Array.init boxes.Length id) 0 boxes.Length bounds
            BvhTree<'a>(data, bounds, Some root)
        else
            BvhTree<'a>([||], Box3d.Invalid, None)

    new(getBounds : 'a -> Box3d, data : 'a[]) =
        let boxes = data |> Array.map(fun d -> d, getBounds d)
        BvhTree<'a>(boxes)

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
module BvhTree =
    
    let inline ofArray (data : array<'a * Box3d>) = BvhTree(data)
    let inline create (bounds : 'a -> Box3d) (data : array<'a>) = BvhTree(bounds, data)

    let inline data (tree : BvhTree<'a>) = tree.Data
    let inline root (tree : BvhTree<'a>) = tree.Root
    let inline bounds (tree : BvhTree<'a>) = tree.Bounds

    let inline intersect (tryIntersect : RayPart -> 'a -> Option<RayHit<'r>>) (part : RayPart) (tree : BvhTree<'a>) =
        tree.Intersect(tryIntersect, part)

    let inline cull (hull : FastHull3d) (tree : BvhTree<'a>) =
        tree.Cull(hull)

    let inline cullFrustum (viewProj : Trafo3d) (tree : BvhTree<'a>) =
        tree.CullFrustum(viewProj)