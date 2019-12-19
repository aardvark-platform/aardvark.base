namespace Aardvark.Base

open System

/// Provides generic math functions that work for both scalars and vectors (element-wise).
/// Functions already provided by the F# core library are only redefined if necessary 
/// (e.g. different signature)
[<AutoOpen>]
module Math =

    module Helpers =
        type Signum() =
            static member inline Signum(x : sbyte) = sign x |> sbyte
            static member inline Signum(x : int16) = sign x |> int16
            static member inline Signum(x : int32) = sign x |> int32
            static member inline Signum(x : int64) = sign x |> int64

            static member inline Signum(x : nativeint) = sign x |> nativeint
            static member inline Signum(x : float) = sign x |> float
            static member inline Signum(x : float32) = sign x |> float32
            static member inline Signum(x : decimal) = sign x |> decimal
        
        type Signumi() =
            static member inline Signumi(x : sbyte) = sign x
            static member inline Signumi(x : int16) = sign x
            static member inline Signumi(x : int32) = sign x
            static member inline Signumi(x : int64) = sign x

            static member inline Signumi(x : nativeint) = sign x
            static member inline Signumi(x : float) = sign x
            static member inline Signumi(x : float32) = sign x
            static member inline Signumi(x : decimal) = sign x

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

        type Min() =
            static member inline Min(x : sbyte, y : sbyte) = min x y
            static member inline Min(x : int16, y : int16) = min x y
            static member inline Min(x : int32, y : int32) = min x y
            static member inline Min(x : int64, y : int64) = min x y

            static member inline Min(x : byte, y : byte) = min x y
            static member inline Min(x : uint16, y : uint16) = min x y
            static member inline Min(x : uint32, y : uint32) = min x y
            static member inline Min(x : uint64, y : uint64) = min x y

            static member inline Min(x : nativeint, y : nativeint) = min x y
            static member inline Min(x : float, y : float) = min x y
            static member inline Min(x : float32, y : float32) = min x y
            static member inline Min(x : decimal, y : decimal) = min x y

        type Max() =
            static member inline Max(x : sbyte, y : sbyte) = max x y
            static member inline Max(x : int16, y : int16) = max x y
            static member inline Max(x : int32, y : int32) = max x y
            static member inline Max(x : int64, y : int64) = max x y

            static member inline Max(x : byte, y : byte) = max x y
            static member inline Max(x : uint16, y : uint16) = max x y
            static member inline Max(x : uint32, y : uint32) = max x y
            static member inline Max(x : uint64, y : uint64) = max x y

            static member inline Max(x : nativeint, y : nativeint) = max x y
            static member inline Max(x : float, y : float) = max x y
            static member inline Max(x : float32, y : float32) = max x y
            static member inline Max(x : decimal, y : decimal) = max x y

    [<AutoOpen>]
    module private Aux =
        let inline signumAux (_ : ^z) (x : ^a) =
            ((^z or ^a) : (static member Signum : ^a  -> ^a) x)

        let inline signumiAux (_ : ^z) (x : ^a) =
            ((^z or ^a) : (static member Signumi : ^a  -> ^b) x)

        let inline powerAux (_ : ^z) (x : ^a) (y : ^a) =
            ((^z or ^a) : (static member Power : ^a * ^a -> ^a) (x, y))
        
        let inline minAux (_ : ^z) (x : ^a) (y : ^b) =
            ((^z or ^a or ^b) : (static member Min : ^a * ^b -> ^a) (x, y))

        let inline maxAux (_ : ^z) (x : ^a) (y : ^b) =
            ((^z or ^a or ^b) : (static member Max : ^a * ^b -> ^a) (x, y))

    /// Resolves to the zero value for any scalar or vector type.
    let inline zero< ^T when ^T : (static member Zero : ^T) > : ^T =
        LanguagePrimitives.GenericZero

    /// Resolves to the one value for any scalar or vector type.
    let inline one< ^T when ^T : (static member One : ^T) > : ^T =
        LanguagePrimitives.GenericOne

    /// Returns -1 if x is less than zero, 0 if x is equal to zero, and 1 if
    /// x is greater than zero. The result has the same type as the input.
    let inline signum x =
        signumAux Unchecked.defaultof<Helpers.Signum> x

    /// Returns -1 if x is less than zero, 0 if x is equal to zero, and 1 if
    /// x is greater than zero.
    let inline signumi x =
        signumiAux Unchecked.defaultof<Helpers.Signumi> x

    /// Returns x raised by the power of y.
    // F# variant does not support integers!
    let inline pow x y =
        powerAux Unchecked.defaultof<Helpers.Power> x y

    /// Returns the smaller of x and y.
    let inline min (y : ^b) (x : ^a) : ^a =
        minAux Unchecked.defaultof<Helpers.Min> x y

    /// Returns the larger of x and y.
    let inline max (y : ^b) (x : ^a) : ^a =
        maxAux Unchecked.defaultof<Helpers.Max> x y

    /// Clamps x to the interval [a, b]
    let inline clamp (a : ^b) (b : ^b) (x : ^a) : ^a =
        x |> max a |> min b

    /// Clamps x to the interval [0, 1]
    let inline saturate (x : ^a) =
        x |> clamp zero<'a> one<'a>

    /// Linearly interpolates between x and y
    let inline lerp (x : ^a) (y : ^a) (t : ^b) =
        (one - t) * x + t * y

    /// Performs Hermite interpolation between a and b
    let inline smoothstep (a : ^a) (b : ^a) (x : ^b) =
        let two : ^b = one + one
        let three : ^b = two + one
        let t = saturate ((x - a) / (b - a))
        t * t * (three - two * t)