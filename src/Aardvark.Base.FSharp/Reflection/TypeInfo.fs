#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif

/// <summary>
/// TypeInfo contains metadata associated with types and provides active patterns
/// deconstructing vector/matrix types etc.
/// </summary>
[<System.Obsolete("Use TypeMeta module instead.")>]
module TypeInfo =
    open System
    open System.Collections.Generic

    type ITypeInfo =
        inherit IComparable
        abstract member Type : Type

    [<CustomEquality>]
    [<CustomComparison>]
    type SimpleType = { simpleType : Type } with
        interface ITypeInfo with
            member x.Type = x.simpleType
        interface IComparable with
            member x.CompareTo o =
                match o with
                    | :? ITypeInfo as o -> x.simpleType.Name.CompareTo o.Type.Name
                    | _ -> failwith ""
        override x.Equals o =
            match o with
                | :? ITypeInfo as o -> x.simpleType.Name = o.Type.Name
                | _ -> false
        override x.GetHashCode() =
            x.simpleType.Name.GetHashCode()

    [<CustomEquality>]
    [<CustomComparison>]
    type VectorType = { vectorType : Type; baseType : ITypeInfo; dimension : int } with
        interface ITypeInfo with
            member x.Type = x.vectorType
        interface IComparable with
            member x.CompareTo o =
                match o with
                    | :? ITypeInfo as o -> x.vectorType.Name.CompareTo o.Type.Name
                    | _ -> failwith ""
        override x.Equals o =
            match o with
                | :? ITypeInfo as o -> x.vectorType.Name = o.Type.Name
                | _ -> false
        override x.GetHashCode() =
            x.vectorType.Name.GetHashCode()

    [<CustomEquality>]
    [<CustomComparison>]
    type MatrixType = { matrixType : Type; baseType : ITypeInfo; dimension : V2i } with 
        interface ITypeInfo with
            member x.Type = x.matrixType
        interface IComparable with
            member x.CompareTo o =
                match o with
                    | :? ITypeInfo as o -> x.matrixType.Name.CompareTo o.Type.Name
                    | _ -> failwith ""
        override x.Equals o =
            match o with
                | :? ITypeInfo as o -> x.matrixType.Name = o.Type.Name
                | _ -> false
        override x.GetHashCode() =
            x.matrixType.Name.GetHashCode()


    let TByte = { simpleType = typeof<byte> }
    let TSByte = { simpleType = typeof<sbyte> }
    let TInt16= { simpleType = typeof<int16> }
    let TUInt16 = { simpleType = typeof<uint16> }
    let TInt32 = { simpleType = typeof<int> }
    let TUInt32 = { simpleType = typeof<uint32> }
    let TInt64 = { simpleType = typeof<int64> }
    let TUInt64 = { simpleType = typeof<uint64> }
    let TFloat32 = { simpleType = typeof<float32> }
    let TFloat64 = { simpleType = typeof<float> }
    let TDecimal = { simpleType = typeof<decimal> }

    let TUnit = { simpleType = typeof<unit> }
    let TChar = { simpleType = typeof<char> }
    let TString = { simpleType = typeof<string> }

    let TV2i = { vectorType = typeof<V2i>; baseType = TInt32; dimension = 2 }
    let TV3i = { vectorType = typeof<V3i>; baseType = TInt32; dimension = 3 }
    let TV4i = { vectorType = typeof<V4i>; baseType = TInt32; dimension = 4 }

    let TV2ui = { vectorType = typeof<V2ui>; baseType = TUInt32; dimension = 2 }
    let TV3ui = { vectorType = typeof<V3ui>; baseType = TUInt32; dimension = 3 }
    let TV4ui = { vectorType = typeof<V4ui>; baseType = TUInt32; dimension = 4 }

    let TV2l = { vectorType = typeof<V2l>; baseType = TInt64; dimension = 2 }
    let TV3l = { vectorType = typeof<V3l>; baseType = TInt64; dimension = 3 }
    let TV4l = { vectorType = typeof<V4l>; baseType = TInt64; dimension = 4 }

    let TV2f = { vectorType = typeof<V2f>; baseType = TFloat32; dimension = 2 }
    let TV3f = { vectorType = typeof<V3f>; baseType = TFloat32; dimension = 3 }
    let TV4f = { vectorType = typeof<V4f>; baseType = TFloat32; dimension = 4 }

    let TV2d = { vectorType = typeof<V2d>; baseType = TFloat64; dimension = 2 }
    let TV3d = { vectorType = typeof<V3d>; baseType = TFloat64; dimension = 3 }
    let TV4d = { vectorType = typeof<V4d>; baseType = TFloat64; dimension = 4 }

    let TC3b  = { vectorType = typeof<C3b>;  baseType = TByte;    dimension = 3 }
    let TC3us = { vectorType = typeof<C3us>; baseType = TUInt16;  dimension = 3 }
    let TC3ui = { vectorType = typeof<C3ui>; baseType = TUInt32;  dimension = 3 }
    let TC3f  = { vectorType = typeof<C3f>;  baseType = TFloat32; dimension = 3 }
    let TC3d  = { vectorType = typeof<C3d>;  baseType = TFloat64; dimension = 3 }

    let TC4b  = { vectorType = typeof<C4b>;  baseType = TByte;    dimension = 4 }
    let TC4us = { vectorType = typeof<C4us>; baseType = TUInt16;  dimension = 4 }
    let TC4ui = { vectorType = typeof<C4ui>; baseType = TUInt32;  dimension = 4 }
    let TC4f  = { vectorType = typeof<C4f>;  baseType = TFloat32; dimension = 4 }
    let TC4d  = { vectorType = typeof<C4d>;  baseType = TFloat64; dimension = 4 }

    let TM22i = { matrixType = typeof<M22i>; baseType = TInt32; dimension = V2i(2,2) }
    let TM23i = { matrixType = typeof<M23i>; baseType = TInt32; dimension = V2i(3,2) }
    let TM33i = { matrixType = typeof<M33i>; baseType = TInt32; dimension = V2i(3,3) }
    let TM34i = { matrixType = typeof<M34i>; baseType = TInt32; dimension = V2i(4,3) }
    let TM44i = { matrixType = typeof<M44i>; baseType = TInt32; dimension = V2i(4,4) }

    let TM22l = { matrixType = typeof<M22l>; baseType = TInt64; dimension = V2i(2,2) }
    let TM23l = { matrixType = typeof<M23l>; baseType = TInt64; dimension = V2i(3,2) }
    let TM33l = { matrixType = typeof<M33l>; baseType = TInt64; dimension = V2i(3,3) }
    let TM34l = { matrixType = typeof<M34l>; baseType = TInt64; dimension = V2i(4,3) }
    let TM44l = { matrixType = typeof<M44l>; baseType = TInt64; dimension = V2i(4,4) }

    let TM22f = { matrixType = typeof<M22f>; baseType = TFloat32; dimension = V2i(2,2) }
    let TM23f = { matrixType = typeof<M23f>; baseType = TFloat32; dimension = V2i(3,2) }
    let TM33f = { matrixType = typeof<M33f>; baseType = TFloat32; dimension = V2i(3,3) }
    let TM34f = { matrixType = typeof<M34f>; baseType = TFloat32; dimension = V2i(4,3) }
    let TM44f = { matrixType = typeof<M44f>; baseType = TFloat32; dimension = V2i(4,4) }

    let TM22d = { matrixType = typeof<M22d>; baseType = TFloat64; dimension = V2i(2,2) }
    let TM23d = { matrixType = typeof<M23d>; baseType = TFloat64; dimension = V2i(3,2) }
    let TM33d = { matrixType = typeof<M33d>; baseType = TFloat64; dimension = V2i(3,3) }
    let TM34d = { matrixType = typeof<M34d>; baseType = TFloat64; dimension = V2i(4,3) }
    let TM44d = { matrixType = typeof<M44d>; baseType = TFloat64; dimension = V2i(4,4) }

    let IntegralTypes : Set<ITypeInfo>  = Set.ofList [TByte; TSByte; TInt16; TUInt16; TInt32; TUInt32; TInt64; TUInt64]
    let FractionalTypes : Set<ITypeInfo>  = Set.ofList [TFloat32; TFloat64; TDecimal]
    let NumTypes : Set<ITypeInfo>  = Set.union IntegralTypes FractionalTypes

    let VectorTypes  : Set<ITypeInfo> =
        Set.ofList [
            TV2i; TV3i; TV4i
            TV2ui; TV3ui; TV4ui
            TV2l; TV3l; TV4l
            TV2f; TV3f; TV4f
            TV2d; TV3d; TV4d
        ]

    let ColorTypes : Set<ITypeInfo> =
        Set.ofList [
            TC3b; TC3us; TC3ui; TC3f; TC3d
            TC4b; TC4us; TC4ui; TC4f; TC4d
        ]

    let MatrixTypes : Set<ITypeInfo> =
        Set.ofList [
            TM22i; TM23i; TM33i; TM34i; TM44i
            TM22l; TM23l; TM33l; TM34l; TM44l
            TM22f; TM23f; TM33f; TM34f; TM44f
            TM22d; TM23d; TM33d; TM34d; TM44d
        ]

    let TRef = { simpleType = typeof<ref<int>>.GetGenericTypeDefinition() }
    let TList = { simpleType = typeof<list<int>>.GetGenericTypeDefinition() }
    let TSeq = { simpleType = typeof<seq<int>>.GetGenericTypeDefinition() }

    let inline private typeInfo t = { simpleType = t } :> ITypeInfo

    let VectorFields = Set.ofList ['X'; 'Y'; 'Z'; 'W']

    [<AutoOpen>]
    module Patterns =

        [<return: Struct>]
        let inline (|Bool|_|) (t : Type) =
            if t = typeof<bool> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Byte|_|) (t : Type) =
            if t = typeof<byte> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|SByte|_|) (t : Type) =
            if t = typeof<sbyte> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Int16|_|) (t : Type) =
            if t = typeof<int16> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|UInt16|_|) (t : Type) =
            if t = typeof<uint16> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Int32|_|) (t : Type) =
            if t = typeof<int32> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|UInt32|_|) (t : Type) =
            if t = typeof<uint32> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|UInt64|_|) (t : Type) =
            if t = typeof<uint64> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Int64|_|) (t : Type) =
            if t = typeof<int64> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|IntPtr|_|) (t : Type) =
            if t = typeof<IntPtr> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Float32|_|) (t : Type) =
            if t = typeof<float32> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Float64|_|) (t : Type) =
            if t = typeof<float> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Decimal|_|) (t : Type) =
            if t = typeof<decimal> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Char|_|) (t : Type) =
            if t = typeof<char> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Unit|_|) (t : Type) =
            if t = typeof<unit> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|String|_|) (t : Type) =
            if t = typeof<string> then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Enum|_|) (t : Type) =
            if t.IsEnum then ValueSome ()
            else ValueNone

        [<return: Struct>]
        let inline (|Integral|_|) (t : Type) =
            TypeMeta.Patterns.(|Integral|_|) t

        [<return: Struct>]
        let inline (|Fractional|_|) (t : Type) =
            TypeMeta.Patterns.(|Fractional|_|) t

        [<return: Struct>]
        let inline (|Num|_|) (t : Type) =
            TypeMeta.Patterns.(|Numeric|_|) t

        [<return: Struct>]
        let inline (|Vector|_|) (t : Type) =
            TypeMeta.Patterns.(|Vector|_|) t

        [<return: Struct>]
        let inline (|Color|_|) (t : Type) =
            TypeMeta.Patterns.(|Color|_|) t

        [<return: Struct>]
        let inline (|Matrix|_|) (t : Type) =
            TypeMeta.Patterns.(|Matrix|_|) t

        [<return: Struct>]
        let inline (|VectorOf|_|) (t : Type) =
            TypeMeta.Patterns.(|VectorOf|_|) t |> ValueOption.map (fun struct (d, t) -> d, t)

        [<return: Struct>]
        let inline (|ColorOf|_|) (t : Type) =
            TypeMeta.Patterns.(|ColorOf|_|) t |> ValueOption.map (fun struct (d, t) -> d, t)

        [<return: Struct>]
        let inline (|MatrixOf|_|) (t : Type) =
            TypeMeta.Patterns.(|MatrixOf|_|) t |> ValueOption.map (fun struct (d, t) -> d, t)

        [<return: Struct>]
        let inline (|Ref|_|) (t : Type) =
            TypeMeta.Patterns.(|RefOf|_|) t

        [<return: Struct>]
        let inline (|List|_|) (t : Type) =
            TypeMeta.Patterns.(|ListOf|_|) t

        [<return: Struct>]
        let inline (|Seq|_|) (t : Type) =
            TypeMeta.Patterns.(|SeqOf|_|) t

        [<Obsolete("Use Seq instead.")>]
        [<return: Struct>]
        let (|SeqOf|_|) = (|Seq|_|)

