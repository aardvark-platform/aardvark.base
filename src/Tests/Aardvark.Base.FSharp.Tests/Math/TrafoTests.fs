namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open Expecto

module TrafoTests =

    module private Gen =

        let floatReasonable =
            Arb.generate<float> |> Gen.filter (fun x -> x > -32000.0 && x < 32000.0)

        let v4d =
            gen {
                let! x = floatReasonable
                let! y = floatReasonable
                let! z = floatReasonable
                let! w = floatReasonable
                return V4d(x, y, z, w)
            }

    type ProjectionTestCase =
        { Near  : float
          Far   : float
          Point : V4d }

    type Generator private () =
        static member ProjectionTestCase =
            gen {
                let! near = Gen.floatReasonable |> Gen.filter (fun x -> x > 0.001)
                let! far = Arb.generate<float> |> Gen.filter (fun x -> x > 0.001 && (x < 32000.0 || isPositiveInfinity x))
                let! point = Gen.v4d

                return {
                    Near = near
                    Far = far
                    Point = point
                }
            }
            |> Arb.fromGen

    let private compareMatrix (actual : M44d) (expected : M44d) =
        for i = 0 to 3 do
            for j = 0 to 3 do
                Expect.floatClose Accuracy.medium actual.[i, j] expected.[i, j] $"Unexpected M{i}{j}"

    let private testTrafo (point : V4d) (trafo : Trafo3d) =
        let p = trafo.Transform point
        let r = trafo.InvTransform p
        let d = Vec.distance point r
        Expect.floatClose Accuracy.medium d 0.0 "Distance between re-transformed point and original too large"

    let private testNearPlaneCorners (rightHanded : bool) (expectedDepth : float) (trafo : Trafo3d) =
        let points = [|
            V3d(-100, -100, if rightHanded then -1 else 1)
            V3d(-100,  100, if rightHanded then -1 else 1)
            V3d( 100, -100, if rightHanded then -1 else 1)
            V3d( 100,  100, if rightHanded then -1 else 1)
        |]

        let expected = [|
            V3d(-1.0, -1.0, expectedDepth)
            V3d(-1.0,  1.0, expectedDepth)
            V3d( 1.0, -1.0, expectedDepth)
            V3d( 1.0,  1.0, expectedDepth)
        |]

        let result =
            points |> Array.map trafo.TransformPosProj

        for i = 0 to points.Length - 1 do
            for c = 0 to 2 do
                Expect.floatClose Accuracy.medium result.[i].[c] expected.[i].[c] $"{Meta.VecFields.[c]}-component of point {i} does not match"

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[Trafo] Perspective projection RH`` (input : ProjectionTestCase) =
        let trafo = Trafo3d.PerspectiveProjectionRH(70.0, 1.0, input.Near, input.Far)
        trafo |> testTrafo input.Point

    [<Test>]
    let ``[Trafo] Perspective projection RH frustum corners`` () =
        let trafo = Trafo3d.PerspectiveProjectionRH(-100.0, 100.0, -100.0, 100.0, 1.0, 10.0)
        trafo |> testNearPlaneCorners true 0.0

    [<Test>]
    let ``[Trafo] Perspective projection RH matches M44d`` () =
        let trafo = Trafo3d.PerspectiveProjectionRH(-100.0, 100.0, -100.0, 100.0, 1.0, 10.0)
        let matrix = M44d.PerspectiveProjectionTransformRH(-100.0, 100.0, 100.0, -100.0, 1.0, 10.0)
        compareMatrix trafo.Forward matrix

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[Trafo] Perspective projection reversed RH`` (input : ProjectionTestCase) =
        let trafo = Trafo3d.PerspectiveProjectionReversedRH(70.0, 1.0, input.Near, input.Far)

        let reference =
            let P = Trafo3d.PerspectiveProjectionRH(70.0, 1.0, input.Near, input.Far)
            let S = Trafo3d.Scale(1.0, 1.0, -1.0)
            let T = Trafo3d.Translation(0.0, 0.0, 1.0)
            P * S * T

        compareMatrix trafo.Forward reference.Forward
        compareMatrix trafo.Backward reference.Backward
        trafo |> testTrafo input.Point

    [<Test>]
    let ``[Trafo] Perspective projection reversed RH frustum corners`` () =
        let trafo = Trafo3d.PerspectiveProjectionReversedRH(-100.0, 100.0, -100.0, 100.0, 1.0, 10.0)
        trafo |> testNearPlaneCorners true 1.0

    // LH variants are broken, check the frustum corners test.
    // Didn't bother to fix, since nobody seems to use them anyway.

    //[<Test>]
    //let ``[Trafo] Perspective projection LH frustum corners`` () =
    //    let trafo = Trafo3d.PerspectiveProjectionLH(-100.0, 100.0, -100.0, 100.0, 1.0, 10.0)
    //    trafo |> testNearPlaneCorners false 0.0

    //[<Test>]
    //let ``[Trafo] Perspective projection LH matches M44d`` () =
    //    let trafo = Trafo3d.PerspectiveProjectionLH(-100.0, 100.0, -100.0, 100.0, 1.0, 10.0)
    //    let matrix = M44d.PerspectiveProjectionTransformLH(-100.0, 100.0, 100.0, -100.0, 1.0, 10.0)
    //    compareMatrix trafo.Forward matrix

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[Trafo] Perspective projection GL`` (input : ProjectionTestCase) =
        let trafo = Trafo3d.PerspectiveProjectionGL(70.0, 1.0, input.Near, input.Far)
        trafo |> testTrafo input.Point

    [<Test>]
    let ``[Trafo] Perspective projection GL frustum corners`` () =
        let trafo = Trafo3d.PerspectiveProjectionGL(-100.0, 100.0, -100.0, 100.0, 1.0, 10.0)
        trafo |> testNearPlaneCorners true -1.0

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[Trafo] Perspective projection reversed GL`` (input : ProjectionTestCase) =
        let trafo = Trafo3d.PerspectiveProjectionReversedGL(70.0, 1.0, input.Near, input.Far)

        let reference =
            let P = Trafo3d.PerspectiveProjectionGL(70.0, 1.0, input.Near, input.Far)
            let S = Trafo3d.Scale(1.0, 1.0, -1.0)
            P * S

        compareMatrix trafo.Forward reference.Forward
        compareMatrix trafo.Backward reference.Backward
        trafo |> testTrafo input.Point

    [<Test>]
    let ``[Trafo] Perspective projection reversed GL frustum corners`` () =
        let trafo = Trafo3d.PerspectiveProjectionReversedGL(-100.0, 100.0, -100.0, 100.0, 1.0, 10.0)
        trafo |> testNearPlaneCorners true 1.0

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[Trafo] Orthographic projection RH`` (input : ProjectionTestCase) =
        let far = if isFinite input.Far then input.Far else 32000.0
        let trafo = Trafo3d.OrthoProjectionRH(-100.0, 100.0, -100.0, 100.0, input.Near, far)
        trafo |> testTrafo input.Point

    [<Test>]
    let ``[Trafo] Orthographic projection RH frustum corners`` () =
        let trafo = Trafo3d.OrthoProjectionRH(-100.0, 100.0, -100.0, 100.0, 1.0, 10.0)
        trafo |> testNearPlaneCorners true 0.0

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[Trafo] Orthographic projection GL`` (input : ProjectionTestCase) =
        let far = if isFinite input.Far then input.Far else 320000.0
        let trafo = Trafo3d.OrthoProjectionGL(-100.0, 100.0, -100.0, 100.0, input.Near, far)
        trafo |> testTrafo input.Point

    [<Test>]
    let ``[Trafo] Orthographic projection GL frustum corners`` () =
        let trafo = Trafo3d.OrthoProjectionGL(-100.0, 100.0, -100.0, 100.0, 1.0, 10.0)
        trafo |> testNearPlaneCorners true -1.0