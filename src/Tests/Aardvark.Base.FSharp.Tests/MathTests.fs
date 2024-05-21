namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open NUnit.Framework
open FsUnit

module MathTests =

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

