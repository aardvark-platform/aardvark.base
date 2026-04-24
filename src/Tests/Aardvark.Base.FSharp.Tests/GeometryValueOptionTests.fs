namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base
open Aardvark.Base.Geometry

open NUnit.Framework
open FsUnit

module GeometryValueOptionTests =

    let private toOption value =
        match value with
        | ValueSome value -> Some value
        | ValueNone -> None

    let private hitData<'a> (hit : option<RayHit<'a>>) =
        hit |> Option.map (fun h -> h.T, h.Value)

    let private intersectionsData<'a> (hits : seq<RayHit<'a>>) =
        hits |> Seq.map (fun h -> h.T, h.Value) |> Seq.toArray

    let private sphere center radius =
        Sphere3d.FromCenterAndRadius(center, radius)

    let private box center size =
        Box3d.FromCenterAndSize(center, size)

    let private hitRay =
        RayPart.ofRay <| FastRay3d(V3d.Zero, V3d.OOI)

    let private missRay =
        RayPart.ofRay <| FastRay3d(V3d(3.0, 0.0, 0.0), V3d.OOI)

    let private bvhData =
        [|
            let s = sphere (V3d(0.0, 0.0, 5.0)) 1.0
            (0, s), s.BoundingBox3d

            let s = sphere (V3d(0.0, 0.0, 8.0)) 1.0
            (1, s), s.BoundingBox3d

            let s = sphere (V3d(4.0, 0.0, 5.0)) 1.0
            (2, s), s.BoundingBox3d
        |]

    let private bvh = BvhTree.ofArray bvhData

    let private trySphere part ((id, sphere) : int * Sphere3d) =
        match RayPart.intersect part sphere with
        | Some t -> Some <| RayHit(t, id)
        | None -> None

    let private trySphereV part ((id, sphere) : int * Sphere3d) =
        match RayPart.intersectV part sphere with
        | ValueSome t -> ValueSome <| RayHit(t, id)
        | ValueNone -> ValueNone

    let private kdBoxes =
        [|
            box (V3d(0.0, 0.0, 5.0)) (V3d(2.0, 2.0, 2.0))
            box (V3d(0.0, 0.0, 8.0)) (V3d(2.0, 2.0, 2.0))
            box (V3d(5.0, 0.0, 5.0)) (V3d(2.0, 2.0, 2.0))
        |]

    let private kdTree = KdTree(Spatial.box, kdBoxes)

    let private tryBox part (box : Box3d) =
        match RayPart.intersect part box with
        | Some t -> Some <| RayHit(t, box.Center.Z)
        | None -> None

    let private tryBoxV part (box : Box3d) =
        match RayPart.intersectV part box with
        | ValueSome t -> ValueSome <| RayHit(t, box.Center.Z)
        | ValueNone -> ValueNone

    [<Test>]
    let ``[Geometry] BvhTree.intersectV matches intersect for hit and miss rays`` () =
        let hit =
            BvhTree.intersect trySphere hitRay bvh
            |> hitData

        let hitV =
            BvhTree.intersectV trySphereV hitRay bvh
            |> toOption
            |> hitData

        let miss =
            BvhTree.intersect trySphere missRay bvh
            |> hitData

        let missV =
            BvhTree.intersectV trySphereV missRay bvh
            |> toOption
            |> hitData

        hitV |> should equal hit
        missV |> should equal miss

    [<Test>]
    let ``[Geometry] BvhTree.IntersectionsV preserves ordered hit sequence`` () =
        let hits =
            bvh.Intersections(trySphere, hitRay)
            |> intersectionsData

        let hitsV =
            bvh.IntersectionsV(trySphereV, hitRay)
            |> intersectionsData

        hitsV |> should equal hits
        hitsV |> should equal [| (4.0, 0); (7.0, 1) |]

    [<Test>]
    let ``[Geometry] KdTree.intersectV matches intersect for hit and miss rays`` () =
        let hit =
            KdTree.intersect tryBox hitRay kdTree
            |> hitData

        let hitV =
            KdTree.intersectV tryBoxV hitRay kdTree
            |> toOption
            |> hitData

        let miss =
            KdTree.intersect tryBox missRay kdTree
            |> hitData

        let missV =
            KdTree.intersectV tryBoxV missRay kdTree
            |> toOption
            |> hitData

        hitV |> should equal hit
        missV |> should equal miss

    [<Test>]
    let ``[Geometry] nearest hit is preserved for BvhTree and KdTree`` () =
        let bvhHit =
            BvhTree.intersectV trySphereV hitRay bvh
            |> toOption
            |> hitData

        let kdHit =
            KdTree.intersectV tryBoxV hitRay kdTree
            |> toOption
            |> hitData

        bvhHit |> should equal (Some (4.0, 0))
        kdHit |> should equal (Some (4.0, 5.0))
