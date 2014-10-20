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
    let TV2f = { vectorType = typeof<V2f>; baseType = TFloat32; dimension = 2 }
    let TV3f = { vectorType = typeof<V3f>; baseType = TFloat32; dimension = 3 }
    let TV4f = { vectorType = typeof<V4f>; baseType = TFloat32; dimension = 4 }
    let TV2d = { vectorType = typeof<V2d>; baseType = TFloat64; dimension = 2 }
    let TV3d = { vectorType = typeof<V3d>; baseType = TFloat64; dimension = 3 }
    let TV4d = { vectorType = typeof<V4d>; baseType = TFloat64; dimension = 4 }

    let TM22f = { matrixType = typeof<M22f>; baseType = TFloat32; dimension = new V2i(2,2) }
    let TM33f = { matrixType = typeof<M33f>; baseType = TFloat32; dimension = new V2i(3,3) }
    let TM34f = { matrixType = typeof<M34f>; baseType = TFloat32; dimension = new V2i(4,3) }
    let TM44f = { matrixType = typeof<M44f>; baseType = TFloat32; dimension = new V2i(4,4) }
    let TM22d = { matrixType = typeof<M22d>; baseType = TFloat64; dimension = new V2i(2,2) }
    let TM33d = { matrixType = typeof<M33d>; baseType = TFloat64; dimension = new V2i(3,3) }
    let TM34d = { matrixType = typeof<M34d>; baseType = TFloat64; dimension = new V2i(4,3) }
    let TM44d = { matrixType = typeof<M44d>; baseType = TFloat64; dimension = new V2i(4,4) }

    let IntegralTypes : Set<ITypeInfo>  = Set.ofList [TByte; TSByte; TInt16; TUInt16; TInt32; TUInt32; TInt64; TUInt64]
    let FractionalTypes : Set<ITypeInfo>  = Set.ofList [TFloat32; TFloat64; TDecimal]
    let NumTypes : Set<ITypeInfo>  = Set.union IntegralTypes FractionalTypes
    let VectorTypes  : Set<ITypeInfo> = Set.ofList [TV2i; TV3i; TV4i; TV2f; TV3f; TV4f; TV2d; TV3d; TV4d]
    let MatrixTypes : Set<ITypeInfo>  = Set.ofList [TM22f; TM33f; TM34f; TM44f; TM22d; TM33d; TM34d; TM44d]

    let TRef = { simpleType = typeof<ref<int>>.GetGenericTypeDefinition() }
    let TList = { simpleType = typeof<list<int>>.GetGenericTypeDefinition() }
    let TSeq = { simpleType = typeof<seq<int>>.GetGenericTypeDefinition() }

    let private typeInfo t = { simpleType = t } :> ITypeInfo

    let VectorFields = Set.ofList ['X'; 'Y'; 'Z'; 'W']

    [<AutoOpen>]
    module Patterns =
        
        let (|Bool|_|) (t : Type) = 
            if t = typeof<bool> then Some Bool
            else None

        let (|Byte|_|) (t : Type) = 
            if t = typeof<byte> then Some Byte
            else None

        let (|SByte|_|) (t : Type) = 
            if t = typeof<sbyte> then Some SByte
            else None

        let (|Int16|_|) (t : Type) = 
            if t = typeof<int16> then Some Int16
            else None

        let (|UInt16|_|) (t : Type) = 
            if t = typeof<int16> then Some UInt16
            else None

        let (|Int32|_|) (t : Type) = 
            if t = typeof<int32> then Some Int32
            else None

        let (|UInt32|_|) (t : Type) = 
            if t = typeof<int32> then Some UInt32
            else None

        let (|UInt64|_|) (t : Type) = 
            if t = typeof<uint64> then Some UInt64
            else None

        let (|Int64|_|) (t : Type) = 
            if t = typeof<int64> then Some Int64
            else None

        let (|IntPtr|_|) (t : Type) = 
            if t = typeof<IntPtr> then Some IntPtr
            else None

        let (|Float32|_|) (t : Type) = 
            if t = typeof<float32> then Some Float32
            else None

        let (|Float64|_|) (t : Type) = 
            if t = typeof<float> then Some Float64
            else None


        let (|Decimal|_|) (t : Type) = 
            if t = typeof<decimal> then Some Decimal
            else None


        let (|Char|_|) (t : Type) = 
            if t = typeof<char> then Some Char
            else None

        let (|Unit|_|) (t : Type) = 
            if t = typeof<unit> then Some Unit
            else None

        let (|String|_|) (t : Type) = 
            if t = typeof<string> then Some String
            else None

        let (|Enum|_|) (t : Type) =
            if t.IsEnum then Some Enum
            else None

        let (|Integral|_|) (t : Type) =
            if Set.contains (typeInfo t) IntegralTypes then
                Some Integral
            else
                None

        let (|Fractional|_|) (t : Type) =
            if Set.contains (typeInfo t) FractionalTypes then
                Some Fractional
            else
                None

        let (|Num|_|) (t : Type) =
            if Set.contains (typeInfo t) NumTypes then
                Some Num
            else
                None



        let (|Vector|_|) (t : Type) =
            if Set.contains (typeInfo t) VectorTypes then
                Some Vector
            else
                None

        let (|Matrix|_|) (t : Type) =
            if Set.contains (typeInfo t) MatrixTypes then
                Some Matrix
            else
                None

        let (|VectorOf|_|) (t : Type) =
            let v = VectorTypes |> Set.filter (fun vi -> vi.Type.Name = t.Name) |> Seq.tryFind (fun _ -> true)
            match v with
                | Some(v) -> let vt = v |> unbox<VectorType>
                             VectorOf(vt.dimension, vt.baseType.Type) |> Some
                | None -> None

        let (|MatrixOf|_|) (t : Type) =
            let v = MatrixTypes |> Set.filter (fun vi -> vi.Type.Name = t.Name) |> Seq.tryFind (fun _ -> true)
            match v with
                | Some(v) -> let vt = v |> unbox<MatrixType>
                             MatrixOf(vt.dimension, vt.baseType.Type) |> Some
                | None -> None

        let (|Ref|_|) (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = TRef.simpleType then
                Ref(t.GetGenericArguments().[0]) |> Some
            else
                None

        let (|List|_|) (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = TList.simpleType then
                List(t.GetGenericArguments().[0]) |> Some
            else
                None

        let (|Seq|_|) (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = TSeq.simpleType then
                Seq(t.GetGenericArguments().[0]) |> Some
            else
                None

        let (|SeqOf|_|) (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = TSeq.simpleType then
                SeqOf(t.GetGenericArguments().[0]) |> Some
            else
                None

