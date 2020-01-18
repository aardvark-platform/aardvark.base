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
            static member inline Power(x : int32, y : int32) = pown x y
            static member inline Power(x : int64, y : int64) = pown x (int y)

            static member inline Power(x : byte, y : byte) = pown x (int y)
            static member inline Power(x : uint16, y : uint16) = pown x (int y)
            static member inline Power(x : uint32, y : uint32) = pown x (int y)
            static member inline Power(x : uint64, y : uint64) = pown x (int y)

            static member inline Power(x : nativeint, y : nativeint) = pown x (int y)
            static member inline Power(x : float, y : float) = x ** y
            static member inline Power(x : float32, y : float32) = x ** y
            static member inline Power(x : decimal, y : decimal) = Math.Pow(float x, float y) |> decimal

        type MinMaxHelpers() =
            static member inline Min<'a when 'a : comparison>(a : 'a, b : 'a) = Operators.min a b

            static member inline Min(a : float, b : V2d) = V2d.Min(b, a)
            static member inline Min(a : float, b : V3d) = V3d.Min(b, a)
            static member inline Min(a : float, b : V4d) = V4d.Min(b, a)
            static member inline Min(a : float32, b : V2f) = V2f.Min(b, a)
            static member inline Min(a : float32, b : V3f) = V3f.Min(b, a)
            static member inline Min(a : float32, b : V4f) = V4f.Min(b, a)
            static member inline Min(a : int, b : V2i) = V2i.Min(b, a)
            static member inline Min(a : int, b : V3i) = V3i.Min(b, a)
            static member inline Min(a : int, b : V4i) = V4i.Min(b, a)
            static member inline Min(a : int64, b : V2l) = V2l.Min(b, a)
            static member inline Min(a : int64, b : V3l) = V3l.Min(b, a)
            static member inline Min(a : int64, b : V4l) = V4l.Min(b, a)



            
            static member inline Max<'a when 'a : comparison>(a : 'a, b : 'a) = Operators.max a b

            static member inline Max(a : float, b : V2d) = V2d.Max(b, a)
            static member inline Max(a : float, b : V3d) = V3d.Max(b, a)
            static member inline Max(a : float, b : V4d) = V4d.Max(b, a)
            static member inline Max(a : float32, b : V2f) = V2f.Max(b, a)
            static member inline Max(a : float32, b : V3f) = V3f.Max(b, a)
            static member inline Max(a : float32, b : V4f) = V4f.Max(b, a)
            static member inline Max(a : int, b : V2i) = V2i.Max(b, a)
            static member inline Max(a : int, b : V3i) = V3i.Max(b, a)
            static member inline Max(a : int, b : V4i) = V4i.Max(b, a)
            static member inline Max(a : int64, b : V2l) = V2l.Max(b, a)
            static member inline Max(a : int64, b : V3l) = V3l.Max(b, a)
            static member inline Max(a : int64, b : V4l) = V4l.Max(b, a)


            static member inline Clamp(x : int, a : int, b : int) = Fun.Clamp(x, a, b)
            static member inline Clamp(x : float, a : float, b : float) = Fun.Clamp(x, a, b)

            static member inline Clamp< ^a when ^a : comparison>(x : ^a, min : ^a, max : ^a) =
                if x > max then max
                elif x < min then min
                else x




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

        let inline cbrtAux (_ : ^z) (x : ^a) =
            ((^z or ^a) : (static member Cbrt : ^a  -> ^a) x)
        
        let inline minAux (_ : ^z) (x : ^a) (y : ^b) =
            ((^z or ^a or ^b or ^c) : (static member Min : ^a * ^b -> ^c) (x, y))

        let inline maxAux (_ : ^z) (x : ^a) (y : ^b) =
            ((^z or ^a or ^b or ^c) : (static member Max : ^a * ^b -> ^c) (x, y))

        let inline clampAux (_ : ^z) (a : ^a) (b : ^b) (x : ^c) =
            ((^z or ^a or ^b or ^c or ^d) : (static member Clamp : ^c * ^a * ^b -> ^d) (x, a, b))

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

    /// Returns x^2
    let inline sqr (x : ^a) = x * x
        //squareAux Unchecked.defaultof<Fun> Unchecked.defaultof<Helpers.NativeInt> x

    /// Returns the cubic root of x.
    let inline cbrt x =
        cbrtAux Unchecked.defaultof<Fun> x

    /// Returns the smaller of x and y.
    let inline min x y = minAux Unchecked.defaultof<Helpers.MinMaxHelpers> x y

    /// Returns the larger of x and y.
    let inline max x y = maxAux Unchecked.defaultof<Helpers.MinMaxHelpers> x y

    /// Clamps x to the interval [a, b].
    let inline clamp a b x = clampAux Unchecked.defaultof<Helpers.MinMaxHelpers> a b x

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

    module private CompilerTests =

        let zeroWorking() =
            let a : int = zero
            let a : float = zero
            let a : V2d = zero
            ()

        let oneWorking() =
            let a : int = one
            let a : float = one
            let a : V2d = one
            ()

        let signumWorking() =
            let a : float = signum 1.0
            let b : int = signum 1
            let s : V2d = signum V2d.II
            let a : decimal = signum 1.0m
            ()

        let signumIntWorking() =
            let a : int = signumi 1.0
            let b : int = signumi 1
            let s : V2i = signumi V2d.II
            ()

        let powerWorking() =
            let a : float = pow 1.0 2.0
            let a : decimal = pow 1.0m 2.0m
            let a : V2d = pow V2d.II V2d.II
            ()
            
        let log2Working() =
            let a : float = log2 10.0
            let a : V2d =  log2 V2d.II
            ()
            
        let cbrtWorking() =
            let a : float = cbrt 10.0
            let a : V2d =  cbrt V2d.II
            ()
            
        let sqrWorking() =
            let a : float = sqr 10.0
            let a : V2d =  sqr V2d.II
            ()


        let clampWorking() =
            let a : int = clamp 1 2 3
            let a : Version = clamp (Version(1,2,3)) (Version(3,2,3)) (Version(4,5,6))
            let a : V2d = clamp V2d.Zero V2d.One V2d.Half
            ()
        let minWorking() =
            let a : V2d = min V2d.II V2d.OO
            let a : V2d = min V2d.II 0.0
            let a : V2d = min 0.0 V2d.II
            let a : float = min 1.0 2.0
            let a : uint32 = min 1u 2u
            let a : nativeint = min 1n 2n
            let a : Version = min (Version(1,2,3)) (Version(3,2,3))
            ()
        let maxWorking() =
            let a : V2d = max V2d.II V2d.OO
            let a : V2d = max V2d.II 0.0
            let a : V2d = max 0.0 V2d.II
            let a : float = max 1.0 2.0
            let a : uint32 = max 1u 2u
            let a : nativeint = max 1n 2n
            let a : Version = max (Version(1,2,3)) (Version(3,2,3))
            ()
