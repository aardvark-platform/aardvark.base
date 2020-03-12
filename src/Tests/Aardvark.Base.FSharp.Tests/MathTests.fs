namespace Aardvark.Base.FSharp.Tests

open Aardvark.Base

open FsUnit
open NUnit.Framework

module MathTests =

    let private iterations = 10000

    type Value(value : float) =
        
        member x.Value = value

        static member One = Value(1.0)

        static member (+) (a : Value, b : Value) = Value(a.Value + b.Value)
        static member (-) (a : Value, b : Value) = Value(a.Value - b.Value)
        static member (*) (a : Value, b : Value) = Value(a.Value * b.Value)
        static member (/) (a : Value, b : Value) = Value(a.Value / b.Value)
        
        static member Sqrt (x : Value) = Value(sqrt x.Value)
        static member Log (x : Value) = Value(log x.Value)

        override x.ToString() = string value

        override x.GetHashCode() = hash value

        override x.Equals other =
            match other with
            | :? Value as v -> value.Equals(v.Value) || value.ApproximateEquals(v.Value)
            | _ -> false

    [<Test>]
    let ``[MathTests] Inverse hyperbolic functions``() =

        let rnd = RandomSystem(0);

        for _ in 1 .. iterations do
            let x = (rnd.UniformDouble() - 0.5) * 2.0
            let value = Value(x)
            
            asinh value |> should equal (Value (asinh x))
            acosh value |> should equal (Value (acosh x))
            atanh value |> should equal (Value (atanh x))

        ()