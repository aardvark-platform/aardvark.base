namespace Aardvark.Base

open System

/// Provides generic math functions that work for both scalars and vectors (element-wise).
/// Functions already provided by the F# core library are only redefined if necessary 
/// (e.g. different signature)
[<AutoOpen>]
module Math =

    module Helpers =
        type Power() =
            static member inline Power(x : sbyte, y : sbyte) = pown x (int y)
            static member inline Power(x : int16, y : int16) = pown x (int y)
            static member inline Power(x : int32, y : int32) = pown x (int y)
            static member inline Power(x : int64, y : int64) = pown x (int y)

            static member inline Power(x : byte, y : byte) = pown x (int y)
            static member inline Power(x : uint16, y : uint16) = pown x (int y)
            static member inline Power(x : uint32, y : uint32) = pown x (int y)
            static member inline Power(x : uint64, y : uint64) = pown x (int y)

            static member inline Power(x : nativeint, y : nativeint) = pown x (int y)
            static member inline Power(x : float, y : float) = x ** y
            static member inline Power(x : float32, y : float32) = x ** y
            static member inline Power(x : decimal, y : decimal) = Math.Pow(float x, float y) |> decimal

        type NativeInt() =
            static member inline Min(x : nativeint, y : nativeint) = min x y
            static member inline Max(x : nativeint, y : nativeint) = max x y
            static member inline Square(x : nativeint) = x * x

    [<AutoOpen>]
    module private Aux =
        let inline signumAux (_ : ^z) (x : ^a) =
            ((^z or ^a) : (static member Signum : ^a  -> ^a) x)

        let inline signumiAux (_ : ^z) (x : ^a) =
            ((^z or ^a) : (static member Signumi : ^a  -> ^b) x)

        let inline powerAux (_ : ^z) (x : ^a) (y : ^a) =
            ((^z or ^a) : (static member Power : ^a * ^a -> ^a) (x, y))

        let inline log2Aux (_ : ^z) (x : ^a) =
            ((^z or ^a) : (static member Log2 : ^a  -> ^a) x)

        let inline squareAux (_ : ^z) (_ : ^w) (x : ^a) =
            ((^z or ^w or ^a) : (static member Square : ^a -> ^a) x)

        let inline cbrtAux (_ : ^z) (x : ^a) =
            ((^z or ^a) : (static member Cbrt : ^a  -> ^a) x)
        
        let inline minAux (_ : ^z) (_ : ^w) (x : ^a) (y : ^b) =
            ((^z or ^w or ^a or ^b) : (static member Min : ^a * ^b -> ^a) (x, y))

        let inline maxAux (_ : ^z) (_ : ^w) (x : ^a) (y : ^b) =
            ((^z or ^w or ^a or ^b) : (static member Max : ^a * ^b -> ^a) (x, y))

        let inline clampAux (_ : ^z) (a : ^b) (b : ^b) (x : ^a) =
            ((^z or ^a or ^b) : (static member Clamp : ^a * ^b * ^b -> ^a) (x, a, b))

    /// Resolves to the zero value for any scalar or vector type.
    let inline zero< ^T when ^T : (static member Zero : ^T) > : ^T =
        LanguagePrimitives.GenericZero

    /// Resolves to the one value for any scalar or vector type.
    let inline one< ^T when ^T : (static member One : ^T) > : ^T =
        LanguagePrimitives.GenericOne

    /// Returns -1 if x is less than zero, 0 if x is equal to zero, and 1 if
    /// x is greater than zero. The result has the same type as the input.
    let inline signum x =
        signumAux Unchecked.defaultof<Fun> x

    /// Returns -1 if x is less than zero, 0 if x is equal to zero, and 1 if
    /// x is greater than zero.
    let inline signumi x =
        signumiAux Unchecked.defaultof<Fun> x

    /// Returns x raised by the power of y.
    // F# variant does not support integers!
    let inline pow x y =
        powerAux Unchecked.defaultof<Helpers.Power> x y

    /// Returns the base 2 logarithm of x.
    let inline log2 x =
        log2Aux Unchecked.defaultof<Fun> x

    /// Returns x raised by the power of 2.
    let inline sqr (x : ^a) =
        squareAux Unchecked.defaultof<Fun> Unchecked.defaultof<Helpers.NativeInt> x

    /// Returns the cubic root of x.
    let inline cbrt x =
        cbrtAux Unchecked.defaultof<Fun> x

    /// Returns the smaller of x and y.
    let inline min (y : ^b) (x : ^a) : ^a =
        minAux Unchecked.defaultof<Fun> Unchecked.defaultof<Helpers.NativeInt> x y

    /// Returns the larger of x and y.
    let inline max (y : ^b) (x : ^a) : ^a =
        maxAux Unchecked.defaultof<Fun> Unchecked.defaultof<Helpers.NativeInt> x y

    /// Clamps x to the interval [a, b].
    let inline clamp (a : ^b) (b : ^b) (x : ^a) : ^a =
        clampAux Unchecked.defaultof<Fun> a b x

    /// Clamps x to the interval [0, 1].
    let inline saturate (x : ^a) =
        x |> clamp zero<'a> one<'a>

    /// Linearly interpolates between x and y.
    let inline lerp (x : ^a) (y : ^a) (t : ^b) =
        (one - t) * x + t * y

    /// Performs Hermite interpolation between a and b.
    let inline smoothstep (edge0 : ^a) (edge1 : ^a) (x : ^b) =
        let two : ^b = one + one
        let three : ^b = two + one
        let t = saturate ((x - edge0) / (edge1 - edge0))
        t * t * (three - two * t)