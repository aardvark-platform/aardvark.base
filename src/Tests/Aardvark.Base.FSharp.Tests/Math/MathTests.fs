namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open NUnit.Framework
open FsUnit
open FsCheck
open FsCheck.NUnit
open System

module MathTests =

    let equalWithin x eps = (new NUnit.Framework.Constraints.EqualConstraint(x)).Within(eps)

    type RotationTestCase2D =
        {
            Src: V2d
            Dst: V2d
            Angle: float
        }

    module Gen =
        let floatUnit =
            gen {
               let! n = Arb.generate<uint32>
               return float n / float UInt32.MaxValue
            }

        let direction2d =
            gen {
                let! t = floatUnit
                return Rot2d(t * Constant.PiTimesTwo) * V2d.XAxis
            }

    type Generator private () =
        static member AngleTestCase =
            gen {
                let! t = Gen.floatUnit
                let angle = (t - 0.5) * Constant.PiTimesTwo

                let! src = Gen.direction2d
                let rot = Rot2d angle
                let dst = rot * src

                return { Src = src; Dst = dst; Angle = angle }
            }
            |> Arb.fromGen

    [<Test>]
    let ``[Math] lerp`` () =
        // Just making sure the parameter order is correct :S
        lerp 50uy 100uy 0.5                     |> should equal (Fun.Lerp(0.5, 50uy, 100uy))
        lerp 50uy 100uy 0.5f                    |> should equal (Fun.Lerp(0.5f, 50uy, 100uy))
        lerp 50y 100y 0.5                       |> should equal (Fun.Lerp(0.5, 50y, 100y))
        lerp 50y 100y 0.5f                      |> should equal (Fun.Lerp(0.5f, 50y, 100y))
        lerp 50us 100us 0.5                     |> should equal (Fun.Lerp(0.5, 50us, 100us))
        lerp 50us 100us 0.5f                    |> should equal (Fun.Lerp(0.5f, 50us, 100us))
        lerp 50s 100s 0.5                       |> should equal (Fun.Lerp(0.5, 50s, 100s))
        lerp 50s 100s 0.5f                      |> should equal (Fun.Lerp(0.5f, 50s, 100s))
        lerp 50 100 0.5                         |> should equal (Fun.Lerp(0.5, 50, 100))
        lerp 50 100 0.5f                        |> should equal (Fun.Lerp(0.5f, 50, 100))
        lerp 50u 100u 0.5                       |> should equal (Fun.Lerp(0.5, 50u, 100u))
        lerp 50u 100u 0.5f                      |> should equal (Fun.Lerp(0.5f, 50u, 100u))
        lerp 50UL 100UL 0.5                     |> should equal (Fun.Lerp(0.5, 50UL, 100UL))
        lerp 50UL 100UL 0.5f                    |> should equal (Fun.Lerp(0.5f, 50UL, 100UL))
        lerp 50L 100L 0.5                       |> should equal (Fun.Lerp(0.5, 50L, 100L))
        lerp 50L 100L 0.5f                      |> should equal (Fun.Lerp(0.5f, 50L, 100L))
        lerp 50.0 100.0 0.5                     |> should equal (Fun.Lerp(0.5, 50.0, 100.0))
        lerp 50.0f 100.0f 0.5f                  |> should equal (Fun.Lerp(0.5f, 50.0f, 100.0f))

        lerp (V2i(50)) (V2i(100)) 0.5           |> should equal (Fun.Lerp(0.5, V2i(50), V2i(100)))
        lerp (V2i(50)) (V2i(100)) 0.5f          |> should equal (Fun.Lerp(0.5f, V2i(50), V2i(100)))
        lerp (V2i(50)) (V2i(100)) (V2d(0.5))    |> should equal (Fun.Lerp(V2d(0.5), V2i(50), V2i(100)))
        lerp (V2i(50)) (V2i(100)) (V2f(0.5f))   |> should equal (Fun.Lerp(V2f(0.5f), V2i(50), V2i(100)))

        lerp (V2ui(50)) (V2ui(100)) 0.5         |> should equal (Fun.Lerp(0.5, V2ui(50), V2ui(100)))
        lerp (V2ui(50)) (V2ui(100)) 0.5f        |> should equal (Fun.Lerp(0.5f, V2ui(50), V2ui(100)))
        lerp (V2ui(50)) (V2ui(100)) (V2d(0.5))  |> should equal (Fun.Lerp(V2d(0.5), V2ui(50), V2ui(100)))
        lerp (V2ui(50)) (V2ui(100)) (V2f(0.5f)) |> should equal (Fun.Lerp(V2f(0.5f), V2ui(50), V2ui(100)))

        lerp (V2l(50)) (V2l(100)) 0.5           |> should equal (Fun.Lerp(0.5, V2l(50), V2l(100)))
        lerp (V2l(50)) (V2l(100)) 0.5f          |> should equal (Fun.Lerp(0.5f, V2l(50), V2l(100)))
        lerp (V2l(50)) (V2l(100)) (V2d(0.5))    |> should equal (Fun.Lerp(V2d(0.5), V2l(50), V2l(100)))
        lerp (V2l(50)) (V2l(100)) (V2f(0.5f))   |> should equal (Fun.Lerp(V2f(0.5f), V2l(50), V2l(100)))

        lerp (V2f(50)) (V2f(100)) 0.5f          |> should equal (Fun.Lerp(0.5f, V2f(50), V2f(100)))
        lerp (V2f(50)) (V2f(100)) (V2f(0.5f))   |> should equal (Fun.Lerp(V2f(0.5f), V2f(50), V2f(100)))

        lerp (V2d(50)) (V2d(100)) 0.5           |> should equal (Fun.Lerp(0.5, V2d(50), V2d(100)))
        lerp (V2d(50)) (V2d(100)) (V2d(0.5))    |> should equal (Fun.Lerp(V2d(0.5), V2d(50), V2d(100)))

        lerp (C4d(50.0)) (C4d(100.0)) 0.5       |> should equal (Fun.Lerp(0.5, C4d(50.0), C4d(100.0)))

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[Math] AngleBetween 2D`` (input: RotationTestCase2D) =
        let result = Vec.AngleBetween(input.Src, input.Dst)
        result |> should be (equalWithin (abs input.Angle) 0.00001)

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[Math] AngleBetweenFast 2D`` (input: RotationTestCase2D) =
        let result = Vec.AngleBetweenFast(input.Src, input.Dst)
        result |> should be (equalWithin (abs input.Angle) 0.00001)

    [<Property(Arbitrary = [| typeof<Generator> |])>]
    let ``[Math] AngleBetweenSigned 2D`` (input: RotationTestCase2D) =
        let result = Vec.AngleBetweenSigned(input.Src, input.Dst)
        result |> should be (equalWithin input.Angle 0.00001)