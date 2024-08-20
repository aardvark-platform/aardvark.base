namespace Aardvark.Base

open System

/// Contains metadata associated with types and provides active patterns for deconstructing types.
module TypeMeta =

    [<Struct>]
    type VectorType =
        {
            Type      : Type
            FieldType : Type
            Dimension : int
        }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module VectorType =

        /// List of all supported vector types.
        let all : VectorType list =
            [
                { Type = typeof<V2i>; FieldType = typeof<int32>; Dimension = 2 }
                { Type = typeof<V2ui>; FieldType = typeof<uint32>; Dimension = 2 }
                { Type = typeof<V2l>; FieldType = typeof<int64>; Dimension = 2 }
                { Type = typeof<V2f>; FieldType = typeof<float32>; Dimension = 2 }
                { Type = typeof<V2d>; FieldType = typeof<float>; Dimension = 2 }
                { Type = typeof<V3i>; FieldType = typeof<int32>; Dimension = 3 }
                { Type = typeof<V3ui>; FieldType = typeof<uint32>; Dimension = 3 }
                { Type = typeof<V3l>; FieldType = typeof<int64>; Dimension = 3 }
                { Type = typeof<V3f>; FieldType = typeof<float32>; Dimension = 3 }
                { Type = typeof<V3d>; FieldType = typeof<float>; Dimension = 3 }
                { Type = typeof<V4i>; FieldType = typeof<int32>; Dimension = 4 }
                { Type = typeof<V4ui>; FieldType = typeof<uint32>; Dimension = 4 }
                { Type = typeof<V4l>; FieldType = typeof<int64>; Dimension = 4 }
                { Type = typeof<V4f>; FieldType = typeof<float32>; Dimension = 4 }
                { Type = typeof<V4d>; FieldType = typeof<float>; Dimension = 4 }
            ]

        let private lookupTable =
            all
            |> List.map (fun t -> struct (struct (t.Dimension, t.FieldType), t.Type))
            |> Dictionary.ofListV

        /// Tries to get the vector type of the given field type and dimension.
        let tryGetV (fieldType: Type) (dimension: int) =
            lookupTable.TryFindV(struct (dimension, fieldType))

        /// Tries to get the vector type of the given field type and dimension.
        let tryGet (fieldType: Type) (dimension: int) =
            lookupTable.TryFind(struct (dimension, fieldType))

        /// Gets the vector type of the given field type and dimension.
        let inline get (fieldType: Type) (dimension: int) =
            match tryGetV fieldType dimension with
            | ValueSome t -> t
            | _ -> raise <| ArgumentException($"Vector type with field type {fieldType} and dimension {dimension} does not exist.")

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module ColorType =

        /// List of all supported color types.
        let all : VectorType list =
            [
                { Type = typeof<C3b>; FieldType = typeof<uint8>; Dimension = 3 }
                { Type = typeof<C3us>; FieldType = typeof<uint16>; Dimension = 3 }
                { Type = typeof<C3ui>; FieldType = typeof<uint32>; Dimension = 3 }
                { Type = typeof<C3f>; FieldType = typeof<float32>; Dimension = 3 }
                { Type = typeof<C3d>; FieldType = typeof<float>; Dimension = 3 }
                { Type = typeof<C4b>; FieldType = typeof<uint8>; Dimension = 4 }
                { Type = typeof<C4us>; FieldType = typeof<uint16>; Dimension = 4 }
                { Type = typeof<C4ui>; FieldType = typeof<uint32>; Dimension = 4 }
                { Type = typeof<C4f>; FieldType = typeof<float32>; Dimension = 4 }
                { Type = typeof<C4d>; FieldType = typeof<float>; Dimension = 4 }
            ]

        let private lookupTable =
            all
            |> List.map (fun t -> struct (struct (t.Dimension, t.FieldType), t.Type))
            |> Dictionary.ofListV

        /// Tries to get the color type of the given field type and dimension.
        let tryGetV (fieldType: Type) (dimension: int) =
            lookupTable.TryFindV(struct (dimension, fieldType))

        /// Tries to get the color type of the given field type and dimension.
        let tryGet (fieldType: Type) (dimension: int) =
            lookupTable.TryFind(struct (dimension, fieldType))

        /// Gets the color type of the given field type and dimension.
        let inline get (fieldType: Type) (dimension: int) =
            match tryGetV fieldType dimension with
            | ValueSome t -> t
            | _ -> raise <| ArgumentException($"Color type with field type {fieldType} and dimension {dimension} does not exist.")

    [<Struct>]
    type MatrixType =
        {
            Type      : Type
            FieldType : Type
            Dimension : V2i
        }

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module MatrixType =

        /// List of all supported matrix types.
        let all : MatrixType list =
            [
                { Type = typeof<M22i>; FieldType = typeof<int32>; Dimension = V2i(2, 2) }
                { Type = typeof<M22l>; FieldType = typeof<int64>; Dimension = V2i(2, 2) }
                { Type = typeof<M22f>; FieldType = typeof<float32>; Dimension = V2i(2, 2) }
                { Type = typeof<M22d>; FieldType = typeof<float>; Dimension = V2i(2, 2) }
                { Type = typeof<M23i>; FieldType = typeof<int32>; Dimension = V2i(3, 2) }
                { Type = typeof<M23l>; FieldType = typeof<int64>; Dimension = V2i(3, 2) }
                { Type = typeof<M23f>; FieldType = typeof<float32>; Dimension = V2i(3, 2) }
                { Type = typeof<M23d>; FieldType = typeof<float>; Dimension = V2i(3, 2) }
                { Type = typeof<M33i>; FieldType = typeof<int32>; Dimension = V2i(3, 3) }
                { Type = typeof<M33l>; FieldType = typeof<int64>; Dimension = V2i(3, 3) }
                { Type = typeof<M33f>; FieldType = typeof<float32>; Dimension = V2i(3, 3) }
                { Type = typeof<M33d>; FieldType = typeof<float>; Dimension = V2i(3, 3) }
                { Type = typeof<M34i>; FieldType = typeof<int32>; Dimension = V2i(4, 3) }
                { Type = typeof<M34l>; FieldType = typeof<int64>; Dimension = V2i(4, 3) }
                { Type = typeof<M34f>; FieldType = typeof<float32>; Dimension = V2i(4, 3) }
                { Type = typeof<M34d>; FieldType = typeof<float>; Dimension = V2i(4, 3) }
                { Type = typeof<M44i>; FieldType = typeof<int32>; Dimension = V2i(4, 4) }
                { Type = typeof<M44l>; FieldType = typeof<int64>; Dimension = V2i(4, 4) }
                { Type = typeof<M44f>; FieldType = typeof<float32>; Dimension = V2i(4, 4) }
                { Type = typeof<M44d>; FieldType = typeof<float>; Dimension = V2i(4, 4) }
            ]

        let private lookupTable =
            all
            |> List.map (fun t -> struct (struct (t.Dimension, t.FieldType), t.Type))
            |> Dictionary.ofListV

        /// Tries to get the matrix type of the given field type and dimension.
        let tryGetV (fieldType: Type) (dimension: V2i) =
            lookupTable.TryFindV(struct (dimension, fieldType))

        /// Tries to get the matrix type of the given field type and dimension.
        let tryGet (fieldType: Type) (dimension: V2i) =
            lookupTable.TryFind(struct (dimension, fieldType))

        /// Gets the matrix type of the given field type and dimension.
        let inline get (fieldType: Type) (dimension: V2i) =
            match tryGetV fieldType dimension with
            | ValueSome t -> t
            | _ -> raise <| ArgumentException($"Matrix type with field type {fieldType} and dimension {dimension} does not exist.")

    [<AutoOpen>]
    module Patterns =

        [<AutoOpen>]
        module private Lookup =

            let vectorTypes =
                VectorType.all
                |> List.map (fun vt -> struct (vt.Type, struct (vt.Dimension, vt.FieldType)))
                |> Dictionary.ofListV

            let colorTypes =
                ColorType.all
                |> List.map (fun ct -> struct (ct.Type, struct (ct.Dimension, ct.FieldType)))
                |> Dictionary.ofListV

            let matrixTypes =
                MatrixType.all
                |> List.map (fun mt -> struct (mt.Type, struct (mt.Dimension, mt.FieldType)))
                |> Dictionary.ofListV

            let integralTypes =
                [
                    typeof<uint8>
                    typeof<int8>
                    typeof<int16>
                    typeof<uint16>
                    typeof<int32>
                    typeof<uint32>
                    typeof<int64>
                    typeof<uint64>
                ]
                |> HashSet.ofList

            let fractionalTypes =
                [ typeof<float16>; typeof<float32>; typeof<float>; typeof<decimal> ]
                |> HashSet.ofList

            let numericTypes =
                HashSet.union [ integralTypes; fractionalTypes ]

        /// Determines if the given type is a bool.
        [<return: Struct>]
        let inline (|Bool|_|) (t: Type) =
            if t = typeof<bool> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a uint8.
        [<return: Struct>]
        let inline (|UInt8|_|) (t: Type) =
            if t = typeof<uint8> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a int8.
        [<return: Struct>]
        let inline (|Int8|_|) (t: Type) =
            if t = typeof<int8> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a int16.
        [<return: Struct>]
        let inline (|Int16|_|) (t: Type) =
            if t = typeof<int16> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a uint16.
        [<return: Struct>]
        let inline (|UInt16|_|) (t: Type) =
            if t = typeof<uint16> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a int32.
        [<return: Struct>]
        let inline (|Int32|_|) (t: Type) =
            if t = typeof<int32> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a uint32.
        [<return: Struct>]
        let inline (|UInt32|_|) (t: Type) =
            if t = typeof<uint32> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a int64.
        [<return: Struct>]
        let inline (|Int64|_|) (t: Type) =
            if t = typeof<int64> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a uint64.
        [<return: Struct>]
        let inline (|UInt64|_|) (t: Type) =
            if t = typeof<uint64> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a float32.
        [<return: Struct>]
        let inline (|Float32|_|) (t: Type) =
            if t = typeof<float32> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a float.
        [<return: Struct>]
        let inline (|Float64|_|) (t: Type) =
            if t = typeof<float> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a float16.
        [<return: Struct>]
        let inline (|Float16|_|) (t: Type) =
            if t = typeof<float16> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a decimal.
        [<return: Struct>]
        let inline (|Decimal|_|) (t: Type) =
            if t = typeof<decimal> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a char.
        [<return: Struct>]
        let inline (|Char|_|) (t: Type) =
            if t = typeof<char> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a string.
        [<return: Struct>]
        let inline (|String|_|) (t: Type) =
            if t = typeof<string> then ValueSome ()
            else ValueNone

        /// Determines if the given type is a IntPtr.
        [<return: Struct>]
        let inline (|IntPtr|_|) (t : Type) =
            if t = typeof<IntPtr> then ValueSome ()
            else ValueNone

        /// Determines if the given type is unit.
        [<return: Struct>]
        let inline (|Unit|_|) (t : Type) =
            if t = typeof<unit> then ValueSome ()
            else ValueNone

        /// Determines if the given type is an enumeration.
        [<return: Struct>]
        let inline (|Enum|_|) (t : Type) =
            if t.IsEnum then ValueSome ()
            else ValueNone

        /// Determines if the given type is an integral type.
        [<return: Struct>]
        let (|Integral|_|) (t : Type) =
            if integralTypes.Contains t then ValueSome ()
            else ValueNone

        /// Determines if the given type is half, float, double, or decimal.
        [<return: Struct>]
        let (|Fractional|_|) (t : Type) =
            if fractionalTypes.Contains t then ValueSome ()
            else ValueNone

        /// Determines if the given type is a numeric type (i.e. half, float, double, decimal, or an integral type).
        [<return: Struct>]
        let (|Numeric|_|) (t : Type) =
            if numericTypes.Contains t then ValueSome ()
            else ValueNone

        /// Determines if the given type is a vector type.
        [<return: Struct>]
        let (|Vector|_|) (t: Type) =
            if vectorTypes.ContainsKey t then ValueSome ()
            else ValueNone

        /// Determines if the given type is a color type.
        [<return: Struct>]
        let (|Color|_|) (t: Type) =
            if colorTypes.ContainsKey t then ValueSome ()
            else ValueNone

        /// Determines if the given type is a matrix type.
        [<return: Struct>]
        let (|Matrix|_|) (t: Type) =
            if matrixTypes.ContainsKey t then ValueSome ()
            else ValueNone

        /// Determines if the given type is a vector type.
        /// Returns the dimension and the underlying type.
        [<return: Struct>]
        let (|VectorOf|_|) (t: Type) =
            vectorTypes.TryFindV t

        /// Determines if the given type is a color type.
        /// Returns the dimension and the underlying type.
        [<return: Struct>]
        let (|ColorOf|_|) (t: Type) =
            colorTypes.TryFindV t

        /// Determines if the given type is a matrix type.
        /// Returns the dimension (columns, rows) and the underlying type.
        [<return: Struct>]
        let (|MatrixOf|_|) (t: Type) =
            matrixTypes.TryFindV t

        /// Determines if the given type is a reference type.
        /// Returns the underlying type.
        [<return: Struct>]
        let inline (|RefOf|_|) (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<ref<_>> then ValueSome <| t.GetGenericArguments().[0]
            else ValueNone

        /// Determines if the given type is a list type.
        /// Returns the underlying type.
        [<return: Struct>]
        let inline (|ListOf|_|) (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<list<_>> then ValueSome <| t.GetGenericArguments().[0]
            else ValueNone

        /// Determines if the given type is an array type.
        /// Returns the underlying type.
        [<return: Struct>]
        let inline (|ArrayOf|_|) (t : Type) =
            if t.IsArray then ValueSome(t.GetElementType())
            else ValueNone

        /// Determines if the given type is an Arr type.
        /// Returns the dimension and the underlying type.
        [<return: Struct>]
        let inline (|ArrOf|_|) (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Arr<_,_>> then
                let targs = t.GetGenericArguments()
                let len = targs.[0] |> Peano.getSize
                let content = targs.[1]
                ValueSome struct (len, content)
            else
                ValueNone

        /// Determines if the given type is a sequence type.
        /// Returns the underlying type.
        [<return: Struct>]
        let inline (|SeqOf|_|) (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<seq<_>> then ValueSome <| t.GetGenericArguments().[0]
            else ValueNone

        /// Determines if the given type supports the IEnumerable<'T> interface.
        /// Returns the underlying type.
        [<return: Struct>]
        let inline (|EnumerableOf|_|) (t : Type) =
            if t.IsArray then
                ValueSome (t.GetElementType())

            elif t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<seq<_>> then
                ValueSome (t.GetGenericArguments().[0])

            else
                let iface = t.GetInterface(typedefof<seq<_>>.Name)
                if isNull iface then
                    ValueNone
                else
                    ValueSome (iface.GetGenericArguments().[0])