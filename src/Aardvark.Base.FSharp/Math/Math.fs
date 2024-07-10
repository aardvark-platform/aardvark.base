namespace Aardvark.Base

open System

/// Provides generic math functions that work for both scalars and vectors (element-wise).
/// Functions already provided by the F# core library are only redefined if necessary
/// (e.g. different signature)
[<AutoOpen>]
module FSharpMath =

    module Helpers =

        // Some of these SRTP member methods have odd names to avoid clashes with
        // methods added in .NET 7 (e.g. Double.Log2). If we use those standard names in our helper types we
        // get build errors due to ambiguities when targeting .NET 7+.
        [<Sealed; AbstractClass>]
        type Log2() =
            static member inline LogBinary(x: int8)    : float   = Fun.Log2 x
            static member inline LogBinary(x: int16)   : float   = Fun.Log2 x
            static member inline LogBinary(x: int32)   : float   = Fun.Log2 x
            static member inline LogBinary(x: int64)   : float   = Fun.Log2 x
            static member inline LogBinary(x: uint8)   : float   = Fun.Log2 x
            static member inline LogBinary(x: uint16)  : float   = Fun.Log2 x
            static member inline LogBinary(x: uint32)  : float   = Fun.Log2 x
            static member inline LogBinary(x: uint64)  : float   = Fun.Log2 x
            static member inline LogBinary(x: float)   : float   = Fun.Log2 x
            static member inline LogBinary(x: float32) : float32 = Fun.Log2 x

        [<Sealed; AbstractClass>]
        type Acosh() =
            static member inline Acoshb(x: float)   : float   = Fun.Acosh x
            static member inline Acoshb(x: float32) : float32 = Fun.Acosh x

        [<Sealed; AbstractClass>]
        type Asinh() =
            static member inline Asinhb(x: float)   : float   = Fun.Asinh x
            static member inline Asinhb(x: float32) : float32 = Fun.Asinh x

        [<Sealed; AbstractClass>]
        type Atanh() =
            static member inline Atanhb(x: float)   : float   = Fun.Atanh x
            static member inline Atanhb(x: float32) : float32 = Fun.Atanh x

        [<Sealed; AbstractClass>]
        type Cbrt() =
            static member inline CubeRoot(x: int8)    : float   = Fun.Cbrt x
            static member inline CubeRoot(x: int16)   : float   = Fun.Cbrt x
            static member inline CubeRoot(x: int32)   : float   = Fun.Cbrt x
            static member inline CubeRoot(x: int64)   : float   = Fun.Cbrt x
            static member inline CubeRoot(x: uint8)   : float   = Fun.Cbrt x
            static member inline CubeRoot(x: uint16)  : float   = Fun.Cbrt x
            static member inline CubeRoot(x: uint32)  : float   = Fun.Cbrt x
            static member inline CubeRoot(x: uint64)  : float   = Fun.Cbrt x
            static member inline CubeRoot(x: float)   : float   = Fun.Cbrt x
            static member inline CubeRoot(x: float32) : float32 = Fun.Cbrt x

        [<Sealed; AbstractClass>]
        type Lerp() =
            static member inline LinearInterp(t: float, a: int8, b: int8)         : int8    = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float32, a: int8, b: int8)       : int8    = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float, a: uint8, b: uint8)       : uint8   = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float32, a: uint8, b: uint8)     : uint8   = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float, a: int16, b: int16)       : int16   = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float32, a: int16, b: int16)     : int16   = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float, a: uint16, b: uint16)     : uint16  = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float32, a: uint16, b: uint16)   : uint16  = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float, a: int32, b: int32)       : int32   = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float32, a: int32, b: int32)     : int32   = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float, a: uint32, b: uint32)     : uint32  = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float32, a: uint32, b: uint32)   : uint32  = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float, a: int64, b: int64)       : int64   = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float32, a: int64, b: int64)     : int64   = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float, a: uint64, b: uint64)     : uint64  = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float32, a: uint64, b: uint64)   : uint64  = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: decimal, a: decimal, b: decimal) : decimal = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float, a: float, b: float)       : float   = Fun.Lerp(t, a, b)
            static member inline LinearInterp(t: float32, a: float32, b: float32) : float32 = Fun.Lerp(t, a, b)

        [<Sealed; AbstractClass>]
        type CopySign() =
            static member inline CopySgn(value: float, sign: float)     : float   = Fun.CopySign(value, sign)
            static member inline CopySgn(value: float32, sign: float32) : float32 = Fun.CopySign(value, sign)

        [<Sealed; AbstractClass>]
        type Comparison() =
            static member inline Min< ^T when ^T : comparison>(a : ^T, b : ^T) = Operators.min a b

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

            static member inline Max< ^T when ^T : comparison>(a : ^T, b : ^T) = Operators.max a b

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

        [<Sealed; AbstractClass>]
        type Saturate() =
            static member inline Saturate(x : sbyte) = x |> max 0y |> min 1y
            static member inline Saturate(x : int16) = x |> max 0s |> min 1s
            static member inline Saturate(x : int32) = x |> max 0 |> min 1
            static member inline Saturate(x : int64) = x |> max 0L |> min 1L

            static member inline Saturate(x : byte) = x |> max 0uy |> min 1uy
            static member inline Saturate(x : uint16) = x |> max 0us |> min 1us
            static member inline Saturate(x : uint32) = x |> max 0u |> min 1u
            static member inline Saturate(x : uint64) = x |> max 0UL |> min 1UL

            static member inline Saturate(x : nativeint) = x |> max 0n |> min 1n
            static member inline Saturate(x : float) = x |> max 0.0 |> min 1.0
            static member inline Saturate(x : float16) = x |> max float16.Zero |> min float16.One
            static member inline Saturate(x : float32) = x |> max 0.0f |> min 1.0f
            static member inline Saturate(x : decimal) = x |> max 0m |> min 1m

        [<Sealed; AbstractClass>]
        type Infinity() =
            static member inline IsNaN< ^T when ^T : (member IsNaN : bool)> (x : ^T) : bool =
                (^T : (member IsNaN : bool) x)

            static member inline IsInfinity< ^T when ^T : (member IsInfinity : bool)> (x : ^T) : bool =
                (^T : (member IsInfinity : bool) x)

            static member inline IsPositiveInfinity< ^T when ^T : (member IsPositiveInfinity : bool)> (x : ^T) : bool =
                (^T : (member IsPositiveInfinity : bool) x)

            static member inline IsNegativeInfinity< ^T when ^T : (member IsNegativeInfinity : bool)> (x : ^T) : bool =
                (^T : (member IsNegativeInfinity : bool) x)

        [<Sealed; AbstractClass>]
        type InfinityS() =
            static member inline IsNaN< ^T when ^T : (static member IsNaN : ^T -> bool)> (x : ^T) : bool =
                (^T : (static member IsNaN : ^T -> bool) x)

            static member inline IsInfinity< ^T when ^T : (static member IsInfinity : ^T -> bool)> (x : ^T) : bool =
                (^T : (static member IsInfinity : ^T -> bool) x)

            static member inline IsPositiveInfinity< ^T when ^T : (static member IsPositiveInfinity : ^T -> bool)> (x : ^T) : bool =
                (^T : (static member IsPositiveInfinity : ^T -> bool) x)

            static member inline IsNegativeInfinity< ^T when ^T : (static member IsNegativeInfinity : ^T -> bool)> (x : ^T) : bool =
                (^T : (static member IsNegativeInfinity : ^T -> bool) x)

    [<AutoOpen>]
    module private Aux =
        let inline signumAux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member Signum : ^T -> ^T) x)

        let inline signumiAux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member Signumi : ^T -> ^U) x)

        // Making the power functions more general with ^T -> ^U -> ^V
        // generally works but requires manual type annotations in some cases
        // It's not worth the hassle so we restrict them to ^T -> ^U -> ^T
        // Also using the default pow / Pow() doesn't work with code quotations
        // for some reason.
        let inline powAux (_ : ^Z) (x : ^T) (y : ^U) =
            ((^Z or ^T or ^U) : (static member Power : ^T * ^U -> ^T) (x, y))

        let inline pownAux (_ : ^Z) (x : ^T) (y : ^U) =
            ((^Z or ^T or ^U) : (static member Pown : ^T * ^U -> ^T) (x, y))

        let inline exp2Aux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member PowerOfTwo : ^T -> ^T) (x))

        let inline log2Aux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member LogBinary : ^T -> ^T) x)

        let inline log2intAux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member Log2Int : ^T -> ^U) x)

        let inline asinhAux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member Asinhb : ^T -> ^T) x)

        let inline acoshAux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member Acoshb : ^T -> ^T) x)

        let inline atanhAux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member Atanhb : ^T -> ^T) x)

        let inline cbrtAux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member CubeRoot : ^T -> ^T) x)

        // See comment for powAux
        let inline minAux (_ : ^Z) (x : ^T) (y : ^U) =
            ((^Z or ^T or ^U) : (static member Min : ^T * ^U -> ^U) (x, y))

        let inline maxAux (_ : ^Z) (x : ^T) (y : ^U) =
            ((^Z or ^T or ^U) : (static member Max : ^T * ^U -> ^U) (x, y))

        // Simply using min and max directly will resolve to the comparison overload for some reason.
        // Therefore we need to do it the dumb (incomplete) way. E.g. won't work for Version.
        let inline saturateAux (_ : ^Z) (x : ^T) =
            ((^Z or ^T) : (static member Saturate : ^T -> ^T) (x))

        let inline lerpAux (_ : ^Z) (x : ^T) (y : ^T) (t : ^U) =
            ((^Z or ^T or ^U) : (static member LinearInterp : ^U * ^T * ^T -> ^T) (t, x, y))

        let inline invLerpAux (_ : ^Z) (a : ^T) (b : ^T) (y : ^T) =
            ((^Z or ^T or ^U) : (static member InvLerp : ^T * ^T * ^T -> ^U) (y, a, b))

        let inline stepAux (_ : ^Z) (edge : ^T) (x : ^U) =
            ((^Z or ^T or ^U) : (static member Step : ^U * ^T -> ^U) (x, edge))

        let inline linearstepAux (_ : ^Z) (edge0 : ^T) (edge1 : ^T) (x : ^U) =
            ((^Z or ^T or ^U) : (static member Linearstep : ^U * ^T * ^T -> ^U) (x, edge0, edge1))

        let inline smoothstepAux (_ : ^Z) (edge0 : ^T) (edge1 : ^T) (x : ^U) =
            ((^Z or ^T or ^U) : (static member Smoothstep : ^U * ^T * ^T -> ^U) (x, edge0, edge1))

        // See comment for powAux
        let inline copysignAux (_ : ^Z) (value : ^T) (sign : ^U) =
            ((^Z or ^T or ^U) : (static member CopySgn : ^T * ^U -> ^T) (value, sign))

        let inline degreesAux (_ : ^Z) (radians : ^T) =
            ((^Z or ^T) : (static member DegreesFromRadians : ^T -> ^T) radians)

        let inline radiansAux (_ : ^Z) (degrees : ^T) =
            ((^Z or ^T) : (static member RadiansFromDegrees : ^T -> ^T) degrees)

        let inline maddAux (_ : ^Z) (x : ^T) (y : ^U) (z : ^T) =
            ((^Z or ^T or ^U) : (static member MultiplyAdd : ^T * ^U * ^T -> ^T) (x, y, z))

        let inline isNanAux (_ : ^Z) (_ : ^Y) (x : ^T) =
            ((^Z or ^Y or ^T) : (static member IsNaN : ^T -> bool) x)

        let inline isInfAux (_ : ^Z) (_ : ^Y) (x : ^T) =
            ((^Z or ^Y or ^T) : (static member IsInfinity : ^T -> bool) x)

        let inline isPosInfAux (_ : ^Z) (_ : ^Y) (x : ^T) =
            ((^Z or ^Y or ^T) : (static member IsPositiveInfinity : ^T -> bool) x)

        let inline isNegInfAux (_ : ^Z) (_ : ^Y) (x : ^T) =
            ((^Z or ^Y or ^T) : (static member IsNegativeInfinity : ^T -> bool) x)

    /// Resolves to the zero value for any scalar or vector type.
    [<GeneralizableValue>]
    let inline zero< ^T when ^T : (static member Zero : ^T) > : ^T =
        LanguagePrimitives.GenericZero

    /// Resolves to the one value for any scalar or vector type.
    [<GeneralizableValue>]
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

    /// Returns x raised to the power of y (must be float or double).
    // F# variant does not support integers!
    let inline pow x y =
        powAux Unchecked.defaultof<Fun> x y

    /// Returns x raised to the power of y.
    // F# variant does not support integers!
    let inline ( ** ) x y =
        pow x y

    /// Returns x raised to the integer power of y (must not be negative).
    // F# variant has signature a' -> int -> 'a, which does not permit for example V2f -> V2i -> V2f
    let inline pown x y =
        pownAux Unchecked.defaultof<Fun> x y

    /// Returns 2 raised to the power of x (must be float, double, or uint64).
    let inline exp2 x =
        exp2Aux Unchecked.defaultof<Fun> x

    /// Returns the base 2 logarithm of x.
    let inline log2 x =
        log2Aux Unchecked.defaultof<Helpers.Log2> x

    /// Returns the base 2 logarithm of x rounded to the next integer towards negative infinity.
    let inline log2int x =
        log2intAux Unchecked.defaultof<Fun> x

    /// Returns the inverse hyperbolic sine of x.
    let inline asinh x =
        asinhAux Unchecked.defaultof<Helpers.Asinh> x

    /// Returns the inverse hyperbolic cosine of x.
    let inline acosh x =
        acoshAux Unchecked.defaultof<Helpers.Acosh> x

    /// Returns the inverse hyperbolic tangent of x.
    let inline atanh x =
        atanhAux Unchecked.defaultof<Helpers.Atanh> x

    /// Returns x^2
    let inline sqr x = x * x

    /// Returns the cubic root of x.
    let inline cbrt x =
        cbrtAux Unchecked.defaultof<Helpers.Cbrt> x

    /// Returns the smaller of x and y.
    let inline min x y = minAux Unchecked.defaultof<Helpers.Comparison> x y

    /// Returns the larger of x and y.
    let inline max x y = maxAux Unchecked.defaultof<Helpers.Comparison> x y

    /// Clamps x to the interval [a, b].
    let inline clamp a b x =
        x |> max a |> min b

    /// Clamps x to the interval [0, 1].
    let inline saturate (x : ^T) =
        saturateAux Unchecked.defaultof<Helpers.Saturate> x

    /// Linearly interpolates between x and y.
    let inline lerp x y t =
        lerpAux Unchecked.defaultof<Helpers.Lerp> x y t

    /// Inverse linear interpolation. Computes t of y = a * (1 - t) + b * t.
    let inline invLerp a b y =
        invLerpAux Unchecked.defaultof<Fun> a b y

    /// Returns 0 if x < edge, and 1 otherwise.
    let inline step (edge : ^T) (x : ^U) =
        stepAux Unchecked.defaultof<Fun> edge x

    /// Inverse linear interpolation. Clamped to [0, 1].
    let inline linearstep (edge0 : ^T) (edge1 : ^T) (x : ^U) : ^U =
        linearstepAux Unchecked.defaultof<Fun> edge0 edge1 x

    /// Performs smooth Hermite interpolation between 0 and 1 when edge0 < x < edge1.
    let inline smoothstep (edge0 : ^T) (edge1 : ^T) (x : ^U) =
        smoothstepAux Unchecked.defaultof<Fun> edge0 edge1 x

    /// Returns a value with the magnitude of the first argument and the sign of the second argument.
    let inline copysign (value : ^T) (sign : ^U) =
        copysignAux Unchecked.defaultof<Helpers.CopySign> value sign

    /// Converts an angle given in radians to degrees.
    let inline degrees (radians : ^T) =
        degreesAux Unchecked.defaultof<Conversion> radians

    /// Converts an angle given in degrees to radians.
    let inline radians (degrees : ^T) =
        radiansAux Unchecked.defaultof<Conversion> degrees

    /// Returns (x * y) + z
    let inline madd (x : ^T) (y : ^U) (z : ^T) =
        maddAux Unchecked.defaultof<Fun> x y z

    /// Returns whether x is NaN.
    let inline isNaN (x : ^T) =
        isNanAux Unchecked.defaultof<Helpers.Infinity> Unchecked.defaultof<Helpers.InfinityS> x

    /// Returns whether x is infinity (positive or negative).
    let inline isInfinity (x : ^T) =
        isInfAux Unchecked.defaultof<Helpers.Infinity> Unchecked.defaultof<Helpers.InfinityS> x

    /// Returns whether x is positive infinity.
    let inline isPositiveInfinity (x : ^T) =
        isPosInfAux Unchecked.defaultof<Helpers.Infinity> Unchecked.defaultof<Helpers.InfinityS> x

    /// Returns whether x is negative infinity.
    let inline isNegativeInfinity (x : ^T) =
        isNegInfAux Unchecked.defaultof<Helpers.Infinity> Unchecked.defaultof<Helpers.InfinityS> x

    /// Returns whether x is finite (i.e. not NaN and not infinity).
    let inline isFinite (x : ^T) =
        (x |> isInfinity |> not) && (x |> isNaN |> not)

    [<CompilerMessage("testing purposes", 1337, IsHidden = true)>]
    module ``Math compiler tests 😀😁`` =
        type MyCustomNumericTypeExtensionTestTypeForInternalTesting() =
            static member IsNaN(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = false
            static member Power(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting, e : float) = h
            static member Pown(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting, e : int) = h
            static member (*)(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting, e : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h
            static member (*)(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting, e : int) = h
            static member (+)(h : int, e : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = e
            static member (+)(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting, e : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h
            static member (-)(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting, e : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h
            static member (/)(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting, e : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h

            static member Signum(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h
            static member Signumi(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = 0
            static member CubeRoot(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h

            static member Log(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h
            static member LogBinary(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h
            static member Log2Int(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = 0

            static member Acoshb(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h
            static member Asinhb(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h
            static member Atanhb(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h

            static member Sqrt(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h

            static member inline MultiplyAdd(a : MyCustomNumericTypeExtensionTestTypeForInternalTesting, b : int, c : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = a

            static member DegreesFromRadians(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h
            static member RadiansFromDegrees(h : MyCustomNumericTypeExtensionTestTypeForInternalTesting) = h

        let fsharpCoreWorking() =

            let absWorking() =
                let a : V2i = abs V2i.One
                let a : V3f = abs V3f.One
                ()

            let acosWorking() =
                let a : V2f = acos V2f.One
                let a : V3d = acos V3d.One
                let a : ComplexD = acos ComplexD.One
                ()

            let asinWorking() =
                let a : V2f = asin V2f.One
                let a : V3d = asin V3d.One
                let a : ComplexD = asin ComplexD.One
                ()

            let atanWorking() =
                let a : V2f = atan V2f.One
                let a : V3d = atan V3d.One
                let a : ComplexD = atan ComplexD.One
                ()

            let atan2Working() =
                let a : V2f = atan2 V2f.One V2f.Zero
                let a : V3d = atan2 V3d.One V3d.Zero
                ()

            let ceilWorking() =
                let a : V2f = ceil V2f.One
                let a : V3d = ceil V3d.One
                ()

            let expWorking() =
                let a : V2f = exp V2f.One
                let a : V3d = exp V3d.One
                let a : ComplexD = exp ComplexD.One
                ()

            let floorWorking() =
                let a : V2f = floor V2f.One
                let a : V3d = floor V3d.One
                ()

            let truncateWorking() =
                let a : V2f = truncate V2f.One
                let a : V3d = truncate V3d.One
                ()

            let roundWorking() =
                let a : V2f = round V2f.One
                let a : V3d = round V3d.One
                ()

            let logWorking() =
                let a : V2f = log V2f.One
                let a : V3d = log V3d.One
                let a : ComplexD = log ComplexD.One
                ()

            let log10Working() =
                let a : V2f = log10 V2f.One
                let a : V3d = log10 V3d.One
                let a : ComplexD = log10 ComplexD.One
                ()

            let sqrtWorking() =
                let a : V2f = sqrt V2f.One
                let a : V3d = sqrt V3d.One
                let a : ComplexD = sqrt ComplexD.One
                ()

            let cosWorking() =
                let a : V2f = cos V2f.One
                let a : V3d = cos V3d.One
                let a : ComplexD = cos ComplexD.One
                ()

            let coshWorking() =
                let a : V2f = cosh V2f.One
                let a : V3d = cosh V3d.One
                let a : ComplexD = cosh ComplexD.One
                ()

            let sinWorking() =
                let a : V2f = sin V2f.One
                let a : V3d = sin V3d.One
                let a : ComplexD = sin ComplexD.One
                ()

            let sinhWorking() =
                let a : V2f = sinh V2f.One
                let a : V3d = sinh V3d.One
                let a : ComplexD = sinh ComplexD.One
                ()

            let tanWorking() =
                let a : V2f = tan V2f.One
                let a : V3d = tan V3d.One
                let a : ComplexD = tan ComplexD.One
                ()

            let tanhWorking() =
                let a : V2f = tanh V2f.One
                let a : V3d = tanh V3d.One
                let a : ComplexD = tanh ComplexD.One
                ()

            let listAverageWorking() =
                let a : V2f = List.average [V2f.One; V2f.Zero]
                let a : C4us = List.average [C4us.Black; C4us.White]
                let a : M34d = List.average [M34d.Zero; M34d.Zero]
                let a : ComplexD = List.average [ComplexD.One; ComplexD.Zero]
                ()

            ()

        let zeroWorking() =
            let a : int = zero
            let a : float = zero
            let a : float16 = zero
            let a : V2d = zero
            let a : ComplexD = zero
            ()

        let oneWorking() =
            let a : int = one
            let a : float = one
            let a : float16 = one
            let a : V2d = one
            let a : ComplexD = one
            ()

        let inline indirectSignum (x : ^T) =
            signum x

        let signumWorking() =
            let a : float = signum 1.0
            let b : int = signum 1
            let s : V2d = signum V2d.II
            let a : decimal = signum 1.0m
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = signum (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = indirectSignum (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let signumIntWorking() =
            let a : int = signumi 1.0
            let b : int = signumi 1
            let s : V2i = signumi V2d.II
            let a : int = signumi (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let inline indirectPown (x : ^T) (y : int) =
            pown x y

        let pownWorking() =
            let a = pown (MyCustomNumericTypeExtensionTestTypeForInternalTesting()) 6
            let a = indirectPown (MyCustomNumericTypeExtensionTestTypeForInternalTesting()) 6
            let a : float = pown 1.0 2
            let a : int = pown 1 2
            let a : uint16 = pown 1us 2
            let a : uint16 = pown 1us 2us
            let a : int64 = pown 1L 2
            let a : int64 = pown 1L 2L

            let a = 2.0 * (pown V3d.III V3i.III)
            let a = 2.0 * (indirectPown V3d.III 6)

            let a : V2f = pown V2f.One 1
            let a : V2f = pown V2f.One V2i.One

            let a : V2i = pown V2i.One 1
            let a : V2i = pown V2i.One V2i.One

            let a : V2l = pown V2l.One 1
            let a : V2l = pown V2l.One 1L
            let a : V2l = pown V2l.One V2i.One
            let a : V2l = pown V2l.One V2l.One

            ()

        let inline indirectExp2 (x : ^T) =
            exp2 x

        let exp2Working() =
            let a : float = exp2 1.0
            let a : float32 = exp2 1.0f
            let a : uint64 = exp2 1UL

            let a = 2.0 * (exp2 V3d.III)
            let a = 2.0 * (indirectExp2 V3d.III)

            let a : V2f = exp2 V2f.One
            let a : V2d = exp2 V2d.One

            ()

        let inline indirectPow (x : ^T) (y : ^U) =
            pow x y

        let powWorking() =
            let a : float = pow 1.0 2.0
            let a : float = indirectPow 1.0 2.0
            let a : float32 = pow 1.0f 2.0f
            let a : float32 = indirectPow 1.0f 2.0f
            let a = log (2.0 + 8.0 * (pow V3d.III V3d.III))
            let a = log (2.0 + 8.0 * (indirectPow V3d.III V3d.III))

            let a = pow (MyCustomNumericTypeExtensionTestTypeForInternalTesting()) 12.

            // This doesn't work if we just extend the built-in pow function
            // by adding Pow() members to the vector types...
            let a : V2d -> float -> V2d = pow
            let a : V2d -> float -> V2d = indirectPow
            let a : V2d -> float -> V2d = ( ** )

            let a : V2d = pow V2d.II V2d.II
            let a : V2d = pow V2d.II 1.0

            let a : ComplexD = pow ComplexD.One ComplexD.One
            let a : ComplexD = pow ComplexD.One 1.0
            ()

        let powOpWorking() =
            let a : float = 1.0 ** 2.0
            let a : float32 = 1.0f ** 2.0f

            let a = MyCustomNumericTypeExtensionTestTypeForInternalTesting() ** 12.0

            let a : V2d = V2d.II ** V2d.II
            let a : V2d = V2d.II ** 1.0

            let a : ComplexD = ComplexD.One ** ComplexD.One
            let a : ComplexD = ComplexD.One ** 1.0
            ()

        let inline indirectLog2 (x : ^T) : ^T =
            log2 x

        let log2Working() =
            let a : float = log2 10.0
            let a : V2d =  log2 V2d.II
            let a : ComplexD = log2 ComplexD.One
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = log2 (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = indirectLog2 (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let inline indirectLog2int (x : ^T) =
            log2int x

        let log2intWorking() =
            let a : int = log2int 10.0
            let a : V2i = log2int V2d.II
            let a : int = log2int (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            let a : int = indirectLog2int (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let acoshWorking() =
            let a : float = acosh 1.0
            let a : float32 = acosh 1.0f
            let a : V2f = acosh V2f.One
            let a : V3d = acosh V3d.One
            let a : ComplexD = acosh ComplexD.One
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = acosh (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let asinhWorking() =
            let a : float = asinh 1.0
            let a : float32 = asinh 1.0f
            let a : V2f = asinh V2f.One
            let a : V3d = asinh V3d.One
            let a : ComplexD = asinh ComplexD.One
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = asinh (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let atanhWorking() =
            let a : float = atanh 1.0
            let a : float32 = atanh 1.0f
            let a : V2f = atanh V2f.One
            let a : V3d = atanh V3d.One
            let a : ComplexD = atanh ComplexD.One
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = atanh (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let cbrtWorking() =
            let a : float = cbrt 10.0
            let a : V2d =  cbrt V2d.II
            let a : ComplexD = cbrt ComplexD.One
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = cbrt (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let sqrWorking() =
            let a : byte = sqr 4uy
            let a : float = sqr 10.0
            let a : V2d = sqr V2d.II
            let a : V2i = sqr V2i.II
            let a : ComplexD = sqr ComplexD.One
            ()

        let clampWorking() =
            let a : int = clamp 1 2 3
            let a : float16 = clamp (float16 1) (float16 2) (float16 3)
            let a : Version = clamp (Version(1,2,3)) (Version(3,2,3)) (Version(4,5,6))
            let a : V2d = clamp V2d.Zero V2d.One V2d.Half
            let a : V3f = clamp 0.5f 1.5f V3f.Zero
            let a = exp ((V3f.Zero |> clamp 0.5f V3f.One) * 0.5f)
            ()

        let inline indirectMin x y =
            min x y

        let minWorking() =
            let a : V2d = min V2d.II V2d.OO
            let a : V2d = min 0.0 V2d.II
            let a = (V2d.II |> min 1.0) * 2.0 - 0.5
            let a = (V2d.II |> indirectMin 1.0) * 2.0 - 0.5
            let a : float = min 1.0 2.0
            let a : float16 = min (float16 1.0) (float16 2.0)
            let a : uint32 = min 1u 2u
            let a : nativeint = min 1n 2n
            let a : Version = min (Version(1,2,3)) (Version(3,2,3))
            ()

        let maxWorking() =
            let a : V2d = max V2d.II V2d.OO
            let a : V2d = max 0.0 V2d.II
            let a : float = max 1.0 2.0
            let a : float16 = max (float16 1.0) (float16 2.0)
            let a : uint32 = max 1u 2u
            let a : nativeint = max 1n 2n
            let a : Version = max (Version(1,2,3)) (Version(3,2,3))
            ()

        let inline indirectSaturate (x : ^T) =
            saturate x

        let saturateWorking() =
            let a : int = saturate 3
            let a : int = indirectSaturate 3
            let a : float = saturate 3.0
            let a : float16 = saturate (float16 3.0)
            let a : uint32 = saturate 3u
            let a : nativeint = saturate 3n
            let a : V2d = saturate V2d.One
            let a : V2d = indirectSaturate V2d.One
            let a : V4i = saturate V4i.One
            ()

        let lerpWorking() =
            let a : int = lerp 1 10 0.5
            let a : int = lerp 1 10 0.5f
            let a : float = lerp 1.0 10.0 0.5
            let a : float32 = lerp 1.0f 10.0f 0.5f
            let a : V2i = lerp V2i.Zero V2i.One 0.5
            let a : V4i = lerp V4i.Zero V4i.One V4d.Half
            let a : V2i = lerp V2i.Zero V2i.One 0.5f
            let a : V4i = lerp V4i.Zero V4i.One V4f.Half
            let a : C4b = lerp C4b.Black C4b.Black 0.5f
            ()

        let invLerpWorking() =
            let a : float = 1 |> invLerp 1 10
            let a : float = 1.0 |> invLerp 1.0 10.0
            let a : float32 = 1.0f |> invLerp 1.0f 10.0f
            let a : V2d = V2i.Zero |> invLerp V2i.Zero V2i.One
            let a : V4d = V4i.Zero |> invLerp V4i.Zero V4i.One
            let a : V2f = V2f.Zero |> invLerp V2f.Zero V2f.One
            let a : V4f = V4f.Zero |> invLerp V4f.Zero V4f.One
            let a : V2d = V2d.Zero |> invLerp V2d.Zero V2d.One
            let a : V4d = V4d.Zero |> invLerp V4d.Zero V4d.One
            ()

        let stepWorking() =
            let a : float = step 0.0 1.0
            let a : float32 = step 0.0f 0.5f
            let a : uint8 = step 32uy 64uy
            let a : int8 = step 32y 64y
            let a : int16 = step 32s 64s
            let a : uint16 = step 32us 64us
            let a : int32 = step 32 64
            let a : int64 = step 32L 64L
            let a : decimal = step 32m 64m
            let a : V2i = step 0 V2i.One
            let a : V2l = step 0L V2l.One
            let a : V2f = step 0.0f V2f.Half
            let a : V4d = step V4d.Zero V4d.Half
            ()

        let linearstepWorking() =
            let a : float = linearstep 0.0 1.0 0.5
            let a : float32 = linearstep 0.0f 1.0f 0.5f
            let a : V2f = linearstep 0.0f 1.0f V2f.Half
            let a : V4d = linearstep V4d.Zero V4d.One V4d.Half
            ()

        let smoothstepWorking() =
            let a : float = smoothstep 0.0 1.0 0.5
            let a : float32 = smoothstep 0.0f 1.0f 0.5f
            let a : V2f = smoothstep 0.0f 1.0f V2f.Half
            let a : V4d = smoothstep V4d.Zero V4d.One V4d.Half
            ()

        let copysignWorking() =
            let a : float = copysign 0.0 1.0
            let a : float32 = copysign 0.0f 10.0f
            let a : V3d = copysign V3d.Zero V3d.One
            let a : V3d = copysign V3d.Zero -1.0
            let a = exp (2.0 * (copysign V3d.Zero -1.0))
            ()

        let conversionWorking() =
            let a : float = degrees 0.0
            let a : float32 = radians 0.0f
            let a : V3d = degrees V3d.Zero
            let a : V4f = radians V4f.Zero
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = radians (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = degrees (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let inline indirectMultiplyAdd (x : ^T) (y : ^U) (z : ^T) =
            madd x y z

        let multiplyAddWorking() =
            let a : float = madd 0.0 1.0 2.0
            let a : float32 = madd 0.0f 1.0f 2.0f
            let a : V3d = madd V3d.Zero 1.0 V3d.Zero
            let a : V3d = indirectMultiplyAdd V3d.Zero 1.0 V3d.Zero
            let a : V3d = madd V3d.Zero V3d.Zero V3d.Zero
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = madd (MyCustomNumericTypeExtensionTestTypeForInternalTesting()) 1 (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            let a : MyCustomNumericTypeExtensionTestTypeForInternalTesting = indirectMultiplyAdd (MyCustomNumericTypeExtensionTestTypeForInternalTesting()) 1 (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            ()

        let inline indirectIsNaN (x : ^T) =
            isNaN x

        let specialFloatingPointValuesWorking() =
            let a : bool = isNaN 0.0
            let a : bool = isNaN (float16 0.0)
            let a : bool = indirectIsNaN 0.0
            let a : bool = indirectIsNaN (float16 0.0)
            let a : bool = isNaN 0.0f
            let a : bool = isNaN ComplexD.Zero
            let a : bool = isNaN V3d.Zero
            let a : bool = isNaN C3d.Black
            let a : bool = isNaN M44d.Identity
            let a : bool = isNaN (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            let a : bool = indirectIsNaN (MyCustomNumericTypeExtensionTestTypeForInternalTesting())
            let a : bool = isFinite 0.0
            let a : bool = isFinite 0.0f
            let a : bool = isFinite (float16 0.0f)
            let a : bool = isFinite ComplexD.Zero
            let a : bool = isFinite V3d.Zero
            let a : bool = isFinite C3d.Black
            let a : bool = isFinite M44d.Identity
            let a : bool = isInfinity 0.0
            let a : bool = isInfinity 0.0f
            let a : bool = isInfinity (float16 0.0f)
            let a : bool = isInfinity ComplexD.Zero
            let a : bool = isInfinity V3d.Zero
            let a : bool = isInfinity C3d.Black
            let a : bool = isInfinity M44d.Identity
            let a : bool = isPositiveInfinity 0.0
            let a : bool = isPositiveInfinity 0.0f
            let a : bool = isPositiveInfinity (float16 0.0f)
            let a : bool = isPositiveInfinity ComplexD.Zero
            let a : bool = isPositiveInfinity C3d.Black
            let a : bool = isPositiveInfinity V3d.Zero
            let a : bool = isPositiveInfinity M33d.Zero
            let a : bool = isNegativeInfinity 0.0
            let a : bool = isNegativeInfinity 0.0f
            let a : bool = isNegativeInfinity (float16 0.0f)
            let a : bool = isNegativeInfinity ComplexD.Zero
            let a : bool = isNegativeInfinity C3d.Black
            let a : bool = isNegativeInfinity V3d.Zero
            let a : bool = isNegativeInfinity M33d.Zero
            ()