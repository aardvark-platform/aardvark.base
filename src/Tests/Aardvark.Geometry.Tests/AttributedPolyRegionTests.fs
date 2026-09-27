namespace Aardvark.Geometry.Tests

open System
open System.Collections.Generic
open NUnit.Framework

open Aardvark.Base
open Aardvark.Geometry

[<TestFixture>]
module AttributedPolyRegionTests =

    let private epsilon = 1E-9

    let private interpolate (weights : float[]) (attributes : V3d[]) =
        Array.fold2 (fun sum weight attribute -> sum + weight * attribute) V3d.Zero weights attributes

    let private affineAttribute (point : V2d) =
        V3d(point.X, point.Y, 3.0 * point.X - 2.0 * point.Y + 7.0)

    let private attributedPolygon (attribute : V2d -> V3d) (points : V2d[]) =
        Polygon2d<V3d>(Array.copy points, points |> Array.map attribute)

    let private attributedRegion attribute points =
        PolyRegion<V3d>(attributedPolygon attribute points, interpolate)

    let private legacyRegion (points : V2d[]) =
        PolyRegion(Polygon2d(Array.copy points))

    let private triangleArea (p0 : V2d) (p1 : V2d) (p2 : V2d) =
        let d0 = p1 - p0
        let d1 = p2 - p0
        0.5 * abs (d0.X * d1.Y - d0.Y * d1.X)

    let private attributedArea (region : PolyRegion<V3d>) =
        region.Triangulate(interpolate)
        |> List.sumBy (fun triangle -> triangleArea triangle.P0 triangle.P1 triangle.P2)

    let private legacyArea (region : PolyRegion) =
        region.Triangulate()
        |> Array.sumBy (fun triangle -> triangleArea triangle.P0 triangle.P1 triangle.P2)

    let private assertNear (expected : float) (actual : float) =
        Assert.That(actual, Is.EqualTo(expected).Within(epsilon))

    let private assertAligned (region : PolyRegion<V3d>) =
        for polygon in region.Polygons do
            Assert.That(polygon.Attributes.Length, Is.EqualTo(polygon.Points.Length))
            for i in 0 .. polygon.Points.Length - 1 do
                assertNear polygon.Points.[i].X polygon.Attributes.[i].X
                assertNear polygon.Points.[i].Y polygon.Attributes.[i].Y

    let private assertTriangulationAligned (region : PolyRegion<V3d>) =
        for triangle in region.Triangulate(interpolate) do
            assertNear triangle.P0.X triangle.A0.X
            assertNear triangle.P0.Y triangle.A0.Y
            assertNear triangle.P1.X triangle.A1.X
            assertNear triangle.P1.Y triangle.A1.Y
            assertNear triangle.P2.X triangle.A2.X
            assertNear triangle.P2.Y triangle.A2.Y

    let private assertGeometryMatches (expected : PolyRegion) (actual : PolyRegion<V3d>) =
        assertNear (legacyArea expected) (attributedArea actual)

    [<Test>]
    let ``Boolean operations preserve attributes and match legacy geometry`` () =
        let leftPoints =
            [| V2d(0.0, 0.0); V2d(3.0, 0.0); V2d(3.0, 2.0); V2d(0.0, 2.0) |]
        let rightPoints =
            [| V2d(1.0, -1.0); V2d(2.0, -1.0); V2d(2.0, 3.0); V2d(1.0, 3.0) |]

        let leftPolygon = attributedPolygon affineAttribute leftPoints
        let rightPolygon = attributedPolygon affineAttribute rightPoints
        let leftPointSnapshot = Array.copy leftPolygon.Points
        let leftAttributeSnapshot = Array.copy leftPolygon.Attributes
        let rightPointSnapshot = Array.copy rightPolygon.Points
        let rightAttributeSnapshot = Array.copy rightPolygon.Attributes

        let left = PolyRegion<V3d>(leftPolygon, interpolate)
        let right = PolyRegion<V3d>(rightPolygon, interpolate)
        let legacyLeft = legacyRegion leftPoints
        let legacyRight = legacyRegion rightPoints

        let union = PolyRegion<V3d>.Union(left, right, interpolate)
        let difference = PolyRegion<V3d>.Difference(left, right, interpolate)
        let intersection = PolyRegion<V3d>.Intersection(left, right, interpolate)
        let xor = PolyRegion<V3d>.Xor(left, right, interpolate)

        assertGeometryMatches (PolyRegion.Union(legacyLeft, legacyRight)) union
        assertGeometryMatches (PolyRegion.Difference(legacyLeft, legacyRight)) difference
        assertGeometryMatches (PolyRegion.Intersection(legacyLeft, legacyRight)) intersection
        assertGeometryMatches (PolyRegion.Xor(legacyLeft, legacyRight)) xor

        [ union; difference; intersection; xor ] |> List.iter assertAligned

        let sourcePoints = HashSet<V2d>(Seq.append leftPoints rightPoints)
        let invented =
            intersection.Polygons
            |> Seq.collect (fun polygon -> Seq.zip polygon.Points polygon.Attributes)
            |> Seq.filter (fun (point, _) -> not (sourcePoints.Contains point))
            |> Seq.toArray

        Assert.That(invented.Length, Is.GreaterThan(0))
        for point, attribute in invented do
            let expected = affineAttribute point
            assertNear expected.X attribute.X
            assertNear expected.Y attribute.Y
            assertNear expected.Z attribute.Z

        CollectionAssert.AreEqual(leftPointSnapshot, leftPolygon.Points)
        CollectionAssert.AreEqual(leftAttributeSnapshot, leftPolygon.Attributes)
        CollectionAssert.AreEqual(rightPointSnapshot, rightPolygon.Points)
        CollectionAssert.AreEqual(rightAttributeSnapshot, rightPolygon.Attributes)

    [<Test>]
    let ``Contained subtraction keeps reversed operand attributes and triangulates the hole`` () =
        let outerPoints =
            [| V2d(0.0, 0.0); V2d(4.0, 0.0); V2d(4.0, 4.0); V2d(0.0, 4.0) |]
        let innerPoints =
            [| V2d(1.0, 1.0); V2d(3.0, 1.0); V2d(3.0, 3.0); V2d(1.0, 3.0); V2d(1.0, 1.0) |]
        let innerAttributes =
            [| V3d(1.0, 1.0, 10.0); V3d(3.0, 1.0, 11.0); V3d(3.0, 3.0, 12.0)
               V3d(1.0, 3.0, 13.0); V3d(1.0, 1.0, 10.0) |]

        let outer = attributedRegion affineAttribute outerPoints
        let innerPolygon = Polygon2d<V3d>(Array.copy innerPoints, Array.copy innerAttributes)
        let inner = PolyRegion<V3d>(innerPolygon, TessellationRule.EvenOdd, interpolate)
        let result = PolyRegion<V3d>.Difference(outer, inner, interpolate)

        let expectedByPoint = Dictionary<V2d, V3d>()
        for i in 0 .. innerPoints.Length - 2 do
            expectedByPoint.[innerPoints.[i]] <- innerAttributes.[i]

        let retainedInnerVertices =
            result.Polygons
            |> Seq.collect (fun polygon -> Seq.zip polygon.Points polygon.Attributes)
            |> Seq.filter (fun (point, _) -> expectedByPoint.ContainsKey point)
            |> Seq.toArray

        Assert.That(retainedInnerVertices.Length, Is.EqualTo(4))
        for point, attribute in retainedInnerVertices do
            Assert.That(attribute, Is.EqualTo(expectedByPoint.[point]))

        assertNear 12.0 (attributedArea result)
        assertGeometryMatches
            (PolyRegion.Difference(legacyRegion outerPoints, legacyRegion innerPoints))
            result
        assertAligned result
        assertTriangulationAligned result
        CollectionAssert.AreEqual(innerPoints, innerPolygon.Points)
        CollectionAssert.AreEqual(innerAttributes, innerPolygon.Attributes)

    [<Test>]
    let ``Disjoint and empty regions retain expected geometry`` () =
        let leftPoints =
            [| V2d(0.0, 0.0); V2d(1.0, 0.0); V2d(1.0, 1.0); V2d(0.0, 1.0) |]
        let rightPoints =
            [| V2d(3.0, 0.0); V2d(4.0, 0.0); V2d(4.0, 1.0); V2d(3.0, 1.0) |]

        let left = attributedRegion affineAttribute leftPoints
        let right = attributedRegion affineAttribute rightPoints
        let intersection = PolyRegion<V3d>.Intersection(left, right, interpolate)
        let union = PolyRegion<V3d>.Union(left, right, interpolate)
        let xor = PolyRegion<V3d>.Xor(left, right, interpolate)
        let difference = PolyRegion<V3d>.Difference(left, right, interpolate)

        Assert.That(intersection.IsEmpty, Is.True)
        Assert.That(intersection.Triangulate(interpolate), Is.Empty)
        Assert.That(PolyRegion<V3d>.Empty.IsEmpty, Is.True)
        Assert.That(PolyRegion<V3d>.Empty.Triangulate(interpolate), Is.Empty)
        assertNear 2.0 (attributedArea union)
        assertNear 2.0 (attributedArea xor)
        assertNear 1.0 (attributedArea difference)
        [ union; xor; difference ] |> List.iter assertAligned

    [<Test>]
    let ``Constructor keeps attributes aligned through collinear cleanup and orientation reversal`` () =
        let points =
            [| V2d(0.0, 0.0); V2d(0.0, 1.0); V2d(0.0, 2.0)
               V2d(2.0, 2.0); V2d(2.0, 0.0) |]
        let attributes =
            points |> Array.mapi (fun i point -> V3d(point.X, point.Y, 20.0 + float i))
        let polygon = Polygon2d<V3d>(Array.copy points, Array.copy attributes)
        let region = PolyRegion<V3d>(polygon, TessellationRule.EvenOdd, interpolate)

        Assert.That(region.Polygons.Length, Is.EqualTo(1))
        let result = region.Polygons.Head
        Assert.That(result.Points.Length, Is.EqualTo(4))
        Assert.That(Polygon2d(result.Points).IsCcw(), Is.True)

        let expectedByPoint = Dictionary<V2d, V3d>()
        for i in 0 .. points.Length - 1 do
            expectedByPoint.[points.[i]] <- attributes.[i]

        for i in 0 .. result.Points.Length - 1 do
            Assert.That(result.Attributes.[i], Is.EqualTo(expectedByPoint.[result.Points.[i]]))

        Assert.That(result.Points, Does.Not.Contain(V2d(0.0, 1.0)))
        assertAligned region
        CollectionAssert.AreEqual(points, polygon.Points)
        CollectionAssert.AreEqual(attributes, polygon.Attributes)
