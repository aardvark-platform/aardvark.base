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
                //# foreach (var vt in Meta.VecTypes) {
                { Type = typeof<__vt.Name__>; FieldType = typeof<__vt.FieldType.FSharpName__>; Dimension = __vt.Len__ }
                //# }
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
                //# foreach (var ct in Meta.ColorTypes) {
                { Type = typeof<__ct.Name__>; FieldType = typeof<__ct.FieldType.FSharpName__>; Dimension = __ct.Len__ }
                //# }
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
                //# foreach (var mt in Meta.MatTypes) {
                { Type = typeof<__mt.Name__>; FieldType = typeof<__mt.FieldType.FSharpName__>; Dimension = V2i(__mt.Cols__, __mt.Rows__) }
                //# }
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
                    //# foreach(var t in Meta.IntegerTypes) {
                    typeof<__t.FSharpName__>
                    //# }
                ]
                |> HashSet.ofList

            let fractionalTypes =
                [ typeof<float16>; typeof<float32>; typeof<float>; typeof<decimal> ]
                |> HashSet.ofList

            let numericTypes =
                HashSet.union [ integralTypes; fractionalTypes ]

        //# var patternTypes =
        //#    Meta.BuiltInTypes
        //#    .Concat(Meta.HalfType.IntoArray())
        //#    .Concat(Meta.DecimalType.IntoArray())
        //#    .Concat(Meta.CharType.IntoArray())
        //#    .Concat(Meta.StringType.IntoArray());
        //# foreach (var t in patternTypes) {
        //# var name = t.FSharpName;
        //# var caps = name.StartsWith("uint") ? 2 : 1;
        //# var patternName = name.Substring(0, caps).ToUpperInvariant() + name.Substring(caps);
        //# if (patternName == "Float") patternName = "Float64";
        //# var pattern = $"(|{patternName}|_|)";
        /// Determines if the given type is a __name__.
        [<return: Struct>]
        let inline __pattern__ (t: Type) =
            if t = typeof<__name__> then ValueSome ()
            else ValueNone

        //# }
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