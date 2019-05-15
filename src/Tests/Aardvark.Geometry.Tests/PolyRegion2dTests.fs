namespace Aardvark.Geometry.Tests

open NUnit.Framework
open FsUnit

open Aardvark.Base
open Aardvark.Geometry

module PolyRegion2dTests =
    
    [<Test>]
    let ``Substract small from large PolyRegion``() =
        
        let A = PolyRegion.ofList [ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0)]
        let B = PolyRegion.ofList [ V2d(1.5, 1.5); V2d(0.5, 1.5); V2d(0.5, 0.5); V2d(1.5, 0.5)]

        let Diff = (A - B).Polygons.Head.Points |> Seq.toList
        
        Diff |> should equal ([ V2d(-1, 1); V2d(-1, -1); V2d(1, -1); V2d(1.0, 0.5); V2d(0.5, 0.5); V2d(0.5, 1.0);])

    [<Test>]
    let ``Substract large from small PolyRegion``() =
        
        let A = PolyRegion.ofList [ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0)]
        let B = PolyRegion.ofList [ V2d(10, 10); V2d(0.5, 10.0); V2d(0.5, 0.5); V2d(10.0, 0.5)]

        let Diff = (A - B).Polygons.Head.Points |> Seq.toList
        
        Diff |> should equal ([ V2d(-1, 1); V2d(-1, -1); V2d(1, -1); V2d(1.0, 0.5); V2d(0.5, 0.5); V2d(0.5, 1.0);])

    [<Test>]
    let ``Substract combined PolyRegion from PolyRegion``() =
        
        let A = PolyRegion.ofList [ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0)]
        
        let B = PolyRegion.ofList ([ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0) ] |> List.map (fun v -> v + V2d(0.5, 0.5)))
        let C = PolyRegion.ofList ([ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0) ] |> List.map (fun v -> v - V2d(0.5, 0.5)))

        let Diff = (A - (B + C))
        
        let Diff0 = (Diff.Polygons.Item 0).Points |> Seq.toList
        let Diff1 = (Diff.Polygons.Item 1).Points |> Seq.toList

        Diff0 |> should equal ([ V2d(-0.5, 1.0); V2d(-1, 1); V2d(-1.0, 0.5); V2d(-0.5, 0.5)])
        Diff1 |> should equal ([ V2d(1.0, -0.5); V2d(0.5, -0.5); V2d(0.5, -1.0); V2d(1, -1)])

    [<Test>]
    let ``Substract intersection with combined PolyRegion from PolyRegion``() =
        
        let A = PolyRegion.ofList [ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0)]
        
        let B = PolyRegion.ofList ([ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0) ] |> List.map (fun v -> v + V2d(0.5, 0.5)))
        let C = PolyRegion.ofList ([ V2d(1.0, 1.0); V2d(-1.0, 1.0); V2d(-1.0, -1.0); V2d(1.0, -1.0) ] |> List.map (fun v -> v - V2d(0.5, 0.5)))

        let Diff = (A - (A * (B + C)))
        
        let Diff0 = (Diff.Polygons.Item 0).Points |> Seq.toList
        let Diff1 = (Diff.Polygons.Item 1).Points |> Seq.toList

        Diff0 |> should equal ([ V2d(-0.5, 1.0); V2d(-1, 1); V2d(-1.0, 0.5); V2d(-0.5, 0.5)])
        Diff1 |> should equal ([ V2d(1.0, -0.5); V2d(0.5, -0.5); V2d(0.5, -1.0); V2d(1, -1)])

    [<Test>]
    let ``Substract intersection with large combined PolyRegion from PolyRegion``() =
        
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
