namespace Aardvark.Geometry.Tests

open NUnit.Framework
open FsUnit

open Aardvark.Base
open Aardvark.Geometry

module PolyRegion2dTests =

    let private assertRegionEqual (name : string) (actual : PolyRegion) (expected : Polygon2d list) =
        actual.Polygons.Length |> should equal expected.Length

        (actual.Polygons, expected)
        ||> List.iter2 (fun a e ->
            let actualPoints = a.Points |> Seq.toArray
            let expectedPoints = e.Points |> Seq.toArray
            actualPoints.Length |> should equal expectedPoints.Length

            (actualPoints, expectedPoints)
            ||> Array.iter2 (fun ap ep ->
                ap.ApproximateEquals(ep, 1e-10) |> should equal true
            )
        )
    
    [<Test>]
    let ``Subtract small from large PolyRegion``() =
        
        let A = PolyRegion.ofList [ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0)]
        let B = PolyRegion.ofList [ V2d(1.5, 1.5); V2d(0.5, 1.5); V2d(0.5, 0.5); V2d(1.5, 0.5)]

        let Diff = (A - B).Polygons.Head.Points |> Seq.toList
        
        Diff |> should equal ([ V2d(-1, 1); V2d(-1, -1); V2d(1, -1); V2d(1.0, 0.5); V2d(0.5, 0.5); V2d(0.5, 1.0);])

    [<Test>]
    let ``Subtract large from small PolyRegion``() =
        
        let A = PolyRegion.ofList [ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0)]
        let B = PolyRegion.ofList [ V2d(10, 10); V2d(0.5, 10.0); V2d(0.5, 0.5); V2d(10.0, 0.5)]

        let Diff = (A - B).Polygons.Head.Points |> Seq.toList
        
        Diff |> should equal ([ V2d(-1, 1); V2d(-1, -1); V2d(1, -1); V2d(1.0, 0.5); V2d(0.5, 0.5); V2d(0.5, 1.0);])

    [<Test>]
    let ``Subtract combined PolyRegion from PolyRegion``() =
        
        let A = PolyRegion.ofList [ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0)]
        
        let B = PolyRegion.ofList ([ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0) ] |> List.map (fun v -> v + V2d(0.5, 0.5)))
        let C = PolyRegion.ofList ([ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0) ] |> List.map (fun v -> v - V2d(0.5, 0.5)))

        let Diff = (A - (B + C))
        
        let Diff0 = (Diff.Polygons.Item 0).Points |> Seq.toList
        let Diff1 = (Diff.Polygons.Item 1).Points |> Seq.toList

        Diff0 |> should equal ([ V2d(-0.5, 1.0); V2d(-1, 1); V2d(-1.0, 0.5); V2d(-0.5, 0.5)])
        Diff1 |> should equal ([ V2d(1.0, -0.5); V2d(0.5, -0.5); V2d(0.5, -1.0); V2d(1, -1)])

    [<Test>]
    let ``Subtract intersection with combined PolyRegion from PolyRegion``() =
        
        let A = PolyRegion.ofList [ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0)]
        
        let B = PolyRegion.ofList ([ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0) ] |> List.map (fun v -> v + V2d(0.5, 0.5)))
        let C = PolyRegion.ofList ([ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0) ] |> List.map (fun v -> v - V2d(0.5, 0.5)))

        let Diff = (A - (A * (B + C)))
        
        let Diff0 = (Diff.Polygons.Item 0).Points |> Seq.toList
        let Diff1 = (Diff.Polygons.Item 1).Points |> Seq.toList

        Diff0 |> should equal ([ V2d(-0.5, 1.0); V2d(-1, 1); V2d(-1.0, 0.5); V2d(-0.5, 0.5)])
        Diff1 |> should equal ([ V2d(1.0, -0.5); V2d(0.5, -0.5); V2d(0.5, -1.0); V2d(1, -1)])

    [<Test>]
    [<Ignore("are the expected results correct?")>]
    let ``Subtract intersection with large combined PolyRegion from PolyRegion``() =
        
        let A = PolyRegion.ofList [ V2d(8, 3); V2d(0, 3); V2d(0, 0); V2d(8, 0)]
        
        let B = PolyRegion.ofList ([ V2d(7.5, 2.5); V2d(-4.0, 2.5); V2d(-4, -4); V2d(7.5, -4.0) ])
        let C = PolyRegion.ofList ([ V2d(14.0, 7.5); V2d(0.4, 7.5); V2d(0.4, 0.3); V2d(14.0, 0.3)])
        
        let Diff = (A - (A * (B + C)))

        let Diff0 = (Diff.Polygons.Item 0).Points |> Seq.toList
        let Diff1 = (Diff.Polygons.Item 1).Points |> Seq.toList

        Diff0 |> should equal ([ V2d(0.4, 3.0); V2d(0, 3); V2d(0.0, 2.5); V2d(0.4, 2.5)])
        Diff1 |> should equal ([ V2d(8.0, 0.3); V2d(7.5, 0.3); V2d(7.5, 0.0); V2d(8, 0)])

    [<Test>]
    let ``Small PolyRegion contained in large PolyRegion`` () =

        let p1 = PolyRegion.ofArray([|V2d(0.5,0.5);V2d(-0.5,0.5);V2d(-0.5,-0.5);V2d(0.5,-0.5)|])
        let p2 = PolyRegion.ofArray([|V2d(1.0,1.0);V2d(-1.0,1.0);V2d(-1.0,-1.0);V2d(1.0,-1.0)|])
        
        p1 |> PolyRegion.containsRegion p2 |> should equal true

    [<Test>]
    let ``PolyRegion inverse transforms mirror polygon inverse transforms`` () =
        let region =
            PolyRegion.ofList [
                V2d(-2.0, -1.0)
                V2d(3.0, -1.0)
                V2d(3.0, 2.0)
                V2d(-2.0, 2.0)
            ]

        let euclidean = Euclidean2d(Rot2d.FromDegrees(17.0), V2d(2.5, -1.75))
        let similarity = Similarity2d(1.35, Rot2d.FromDegrees(-21.0), V2d(-3.0, 4.25))
        let shift = Shift2d(3.5, -1.25)
        let rotation = Rot2d.FromDegrees(37.0)
        let scale = Scale2d(1.5, 0.8)

        let cases : (string * PolyRegion * Polygon2d list * PolyRegion) list =
            [
                "Euclidean2d", region.InvTransformed euclidean, region.Polygons |> List.map (fun p -> p.InvTransformed euclidean), PolyRegion.invTransformedEuclidean euclidean region
                "Similarity2d", region.InvTransformed similarity, region.Polygons |> List.map (fun p -> p.InvTransformed similarity), PolyRegion.invTransformedSimilarity similarity region
                "Shift2d", region.InvTransformed shift, region.Polygons |> List.map (fun p -> p.InvTransformed shift), PolyRegion.invTransformedShift shift region
                "Rot2d", region.InvTransformed rotation, region.Polygons |> List.map (fun p -> p.InvTransformed rotation), PolyRegion.invTransformedRot rotation region
                "Scale2d", region.InvTransformed scale, region.Polygons |> List.map (fun p -> p.InvTransformed scale), PolyRegion.invTransformedScale scale region
            ]

        for (name, actual, expected, helperResult) in cases do
            assertRegionEqual $"{name} instance overload" actual expected
            assertRegionEqual $"{name} module helper" helperResult expected
