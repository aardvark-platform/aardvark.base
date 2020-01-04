namespace Aardvark.Base.Incremental

open FSharp.Data.Adaptive

/// provides special operators for composing IMods
module Operators =
    
    /// adaptively adds two values
    let inline (%+) (l : aval<'a>) (r : aval<'b>) = AVal.map2 (+) l r

    /// adaptively subtracts two values
    let inline (%-) (l : aval<'a>) (r : aval<'b>) = AVal.map2 (-) l r

    /// adaptively mutiplies two values
    let inline (%*) (l : aval<'a>) (r : aval<'b>) = AVal.map2 (*) l r

    /// adaptively divides two values
    let inline (%/) (l : aval<'a>) (r : aval<'b>) = AVal.map2 (/) l r

    /// creates an adaptive cell providing the value of "l && r"
    let inline (%&&) (l : aval<bool>) (r : aval<bool>) = AVal.map2 (&&) l r

    /// creates an adaptive cell providing the value of "l || r"
    let inline (%||) (l : aval<bool>) (r : aval<bool>) = AVal.map2 (||) l r

    /// expresses an adaptive "if then else" expression (e.g. m %? a %. b <=> if m then a else b)
    let (%?) (l : aval<bool>) (vt : 'a) : 'a -> aval<'a> = fun vf -> l |> AVal.map (fun v -> if v then vt else vf)

    /// expresses an adaptive "if then else" expression (e.g. m %? a %. b <=> if m then a else b)
    let (%.) (l : 'a -> aval<'a>) (r : 'a) =  l r

    /// forces the value of a cell
    let inline (!!) (m : aval<'a>) = AVal.force m

    /// creates a constant cell containing the given value
    let inline (~~) (v : 'a) = AVal.constant v

    /// creates a changeable cell containing the given value
    let inline mref (v : 'a) = AVal.init v

open System.Runtime.CompilerServices

[<AbstractClass; Sealed; Extension>]
type EvaluationExtensions() =

    [<Extension>]
    static member inline GetValue(x : AdaptiveValue) = x.GetValueUntyped(AdaptiveToken.Top)

    [<Extension>]
    static member inline GetValue(x : aval<'a>) = x.GetValue(AdaptiveToken.Top)

    [<Extension>]
    static member inline GetOperations(x : FSharp.Data.Traceable.IOpReader<'a>) = x.GetChanges(AdaptiveToken.Top)

    [<Extension>]
    static member inline Update(x : FSharp.Data.Traceable.IOpReader<'a>) = x.GetChanges(AdaptiveToken.Top) |> ignore

    //[<Extension>]
    //static member inline Evaluate(x : afun<'a, 'b>, v : 'a) = x.Evaluate(AdaptiveToken.Top, v)
