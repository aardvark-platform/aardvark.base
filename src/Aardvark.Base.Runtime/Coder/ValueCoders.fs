namespace Aardvark.Base

open System
open System.Reflection
open System.Reflection.Emit
open System.Collections.Concurrent
open System.Runtime.InteropServices
open System.Runtime.CompilerServices
open Microsoft.FSharp.Reflection

module AutoCoders =
    open Aardvark.Base.IL
    open Aardvark.Base.IL.TypeBuilder

    type private NewObj<'a>() =
        static member Create() : 'a =
            System.Runtime.Serialization.FormatterServices.GetSafeUninitializedObject(typeof<'a>) |> unbox<'a>

    // creates a ValueCoder-Type for the given valueType
    // reading/writing all fields sequentially (currently ordered by name)
    // NOTE: the generated type provides a constructor expecting a
    //       "FieldInfo -> IValueCoder" function as argument
    let createFieldCoder (valueType : Type) =
        let baseType = typedefof<AbstractByRefValueCoder<_>>.MakeGenericType [|valueType|]
        let stateRefType = typeof<CodeState>.MakeByRefType()

        // TODO: find appropriate order and think about versioning
        let fields = 
            valueType.GetFields(BindingFlags.AnyInstance)
                |> Array.sortBy (fun fi -> fi.Name)


        newtype {
            do! inh baseType

            // define fields holding coder instances for
            // all of the valueType's fields
            let coderFields = Array.zeroCreate fields.Length
            for i in 0..fields.Length-1 do
                let coderFieldType = typedefof<IValueCoder<_>>.MakeGenericType [| fields.[i].FieldType |]
                let! coderField = fld (sprintf "m_coder_%s" fields.[i].Name) coderFieldType
                coderFields.[i] <- coderField

            // override WriteState
            do! mem typeof<Void> "WriteState" [ typeof<IWriter>; valueType; stateRefType ] (
                    codegen {
                        
                        for fi in 0..fields.Length-1 do
                            let valueField = fields.[fi]
                            let coderField = coderFields.[fi]

                            // the following sets up arguments for the call to:
                            // ValueCoderExtensions.WriteState (this : IValueCoder<'a>, w : IWriter, value : 'a, state : byref<CodeState>)

                            // load the associated valuecoder
                            //   stack = [valueCoder]
                            do! IL.ldarg 0
                            do! IL.ldfld coderField

                            // load the writer
                            //   stack = [writer; valueCoder]
                            do! IL.ldarg 1

                            // load the value
                            //   stack = [value; writer; valueCoder]
                            do! IL.ldarg 2

                            // load the field 
                            //   stack = [value.field; writer; valueCoder]
                            do! IL.ldfld valueField

                            // load the ref containing state
                            //   stack = [&state; value.field; writer; valueCoder]
                            do! IL.ldarg 3

                            // call the coder's write method
                            //   stack = []
                            let meth = ValueCoderExtensions.GetWriteMethod(coderField.FieldType, valueField.FieldType)
                            do! IL.call meth

                        do! IL.ret
                    }
                )

            // override ReadState
            do! mem typeof<Void> "ReadState" [ typeof<IReader>; valueType.MakeByRefType(); stateRefType ] (
                    codegen {
//                        // create a new instance of the given type
//                        //   stack = [result]
//                        let not = typedefof<NewObj<_>>.MakeGenericType [|valueType|]
//                        do! IL.call (not.GetMethod("Create", BindingFlags.AnyStatic))
//
//                        // store the result to a local ref
//                        //   stack = []
//                        let! result = IL.newlocal valueType
//                        do! IL.stloc result

                        
                        for fi in 0..fields.Length-1 do
                            let valueField : FieldInfo = fields.[fi]
                            let coderField = coderFields.[fi]

                            // the following sets up arguments for the call to:
                            // ValueCoderExtensions.ReadState (this : IValueCoder<'a>, w : IReader, state : byref<CodeState>)
                            
                            // load the overall result
                            //   stack = [result]
                            do! IL.ldarg 2
                            if not valueType.IsValueType then
                                do! IL.ldind ValueType.Object


                            // load the associated valuecoder
                            //   stack = [valueCoder; result]
                            do! IL.ldarg 0
                            do! IL.ldfld coderField

                            // load the writer
                            //   stack = [reader; valueCoder; result]
                            do! IL.ldarg 1

                            // load the state ref
                            //   stack = [&state; writer; valueCoder; result]
                            do! IL.ldarg 3

                            // read the value
                            //   stack = [value; result]
                            let meth = ValueCoderExtensions.GetReadMethod(coderField.FieldType, valueField.FieldType)
                            do! IL.call meth

                            // store the value we just read
                            //   stack = []
                            do! IL.stfld valueField
   
                        // load and return the result
                        do! IL.ret
                    }
                )

            // define a ctor resolving all needed value-coders
            do! ctor [ typeof<FieldInfo -> IValueCoder> ] (
                    codegen {
                        do! IL.ldarg 0
                        do! IL.call (baseType.GetConstructor [||])

                        for fi in 0..fields.Length-1 do
                            let f = fields.[fi]
                            let coderField = coderFields.[fi]

                            // load this
                            //   stack = [this]
                            do! IL.ldarg 0

                            // load the resolve-lambda
                            //   stack = [resolve; this]
                            do! IL.ldarg 1

                            // load the fieldInfo
                            //   stack = [fieldInfo; resolve; this]
                            do! IL.ldtoken f
                            do! IL.ldtoken valueType
                            do! IL.call <@ FieldInfo.GetFieldFromHandle : _ * _ -> _ @>

                            // resolve the field-coder
                            //   stack = [IValueCoder; this]
                            do! IL.call ( typeof<FieldInfo -> IValueCoder>.GetMethod "Invoke" )

                            // store the resolved coder
                            //   stack = []
                            do! IL.stfld coderField

                        do! IL.ret
                    }
                )

        }


module CustomCoders =
    open Aardvark.Base.IL
    open Aardvark.Base.IL.TypeBuilder

    let private extensions =
        Introspection.GetAllMethodsWithAttribute<ExtensionAttribute>()
            |> Seq.choose (fun t -> 
                let meth = t.E0
                let args = meth.GetParameters() |> Array.map (fun p -> p.ParameterType)
                
                if meth.Name = "Code" && args.Length = 2 && args.[0] = typeof<ICoder> then
                    let codedType = args.[1]
                    let ret = meth.ReturnType
                    if codedType.IsGenericType && codedType.GetGenericTypeDefinition() = typedefof<ref<_>> && ret = typeof<Code<unit>> then
                        let valueType = codedType.GetGenericArguments().[0]

                        if valueType.ContainsGenericParameters then
                            Some (valueType.GetGenericTypeDefinition(), meth)
                        else
                            Some (valueType, meth)
                    else
                        None

                else
                    None
                    
               )
            |> Dictionary.ofSeq

    type Ref<'a>() =
        static member Create() = ref Unchecked.defaultof<'a>

    let buildCustomCoder (valueType : Type) (codeMethod : MethodInfo) =
        
        let baseType = typedefof<CustomCoder<_>>.MakeGenericType [|valueType|]
        let stateRefType = typeof<CodeState>.MakeByRefType()
        let run = typeof<State<CodeState, unit>>.GetMethod "RunUnit"

        let valueRef = typedefof<ref<_>>.MakeGenericType [|valueType|]
        let refField = valueRef.GetField("contents@")

        let emptyRef = typedefof<Ref<_>>.MakeGenericType([|valueType|]).GetMethod("Create", BindingFlags.AnyStatic)

        newtype {
            do! inh baseType

            // override WriteState
            do! mem typeof<Void> "WriteState" [ typeof<IWriter>; valueType; stateRefType ] (
                    codegen {
                        // load writer
                        //   stack = [writer]
                        do! IL.ldarg 1

                        // load the value and wrap it in a ref
                        //   stack = [ref value; writer]
                        do! IL.ldarg 2
                        let ctor = valueRef.GetConstructor [|valueType|]
                        do! IL.newobj ctor

                        // call the code method
                        //   stack = [coder]
                        do! IL.call codeMethod

                        // load state-ref
                        //   stack = [&state; coder]
                        do! IL.ldarg 3

                        // call Code<unit>.RunUnit
                        //   stack = []
                        do! IL.call run

                        do! IL.ret
                    }
                )

            // override ReadState
            do! mem valueType "ReadState" [ typeof<IReader>; stateRefType ] (
                    codegen {
                        let! r = IL.newlocal valueRef
                        
                        // load reader 
                        //   stack = [reader]
                        do! IL.ldarg 1


                        // create a ref and load it onto the stack
                        //   stack = [ref value; reader]
                        do! IL.call emptyRef
                        do! IL.stloc r
                        do! IL.ldloc r

                        // call the code method
                        //   stack = [coder]
                        do! IL.call codeMethod

                        // load state-ref
                        //   stack = [&state; coder]
                        do! IL.ldarg 2

                        // call Code<unit>.RunUnit
                        //   stack = []
                        do! IL.call run


                        // dereference the ref
                        //   stack = [!ref]
                        do! IL.ldloc r
                        do! IL.ldfld refField


                        do! IL.ret
                    }
                )

            // define a ctor resolving all needed value-coders
            do! ctor [ ] (
                    codegen {
                        do! IL.ldarg 0
                        do! IL.call (baseType.GetConstructor [||])
                        do! IL.ret
                    }
                )
        }




    let tryGetCustomCoderType (valueType : Type) =

        let valueType =
            if valueType.IsByRef then valueType.GetElementType()
            else valueType

        let meth = valueType.GetMethod("Code", BindingFlags.AnyStatic, Type.DefaultBinder, [|typeof<ICoder>; valueType.MakeByRefType()|], null)

        if not (isNull meth) && meth.ReturnType = typeof<Code<unit>> then
            let coder = buildCustomCoder valueType meth
            Some coder
        else
            let lookup = 
                if valueType.IsGenericType then valueType.GetGenericTypeDefinition()
                else valueType 

            match extensions.TryGetValue lookup with
                | (true, meth) ->
                    let meth = 
                        if lookup = valueType then Some meth
                        else 
                            let parType = meth.GetParameters().[1].ParameterType.GetGenericArguments().[0]

                            let assignment =
                                Array.zip (parType.GetGenericArguments()) (valueType.GetGenericArguments())
                                    |> Dictionary.ofArray

                            let methArgs = 
                                meth.GetGenericArguments()
                                    |> Array.map (fun a -> assignment.[a])

                            let meth = meth.MakeGenericMethod methArgs

                            Some meth

                    match meth with
                        | Some meth ->    
                            let coder = buildCustomCoder valueType meth
                            Some coder
                        | _ ->
                            None
                | _ ->
                    None

module ValueCoderTypes =
    
    let coderTypeCache = ConcurrentDictionary<Type, Type>()
    let coderCache = ConcurrentDictionary<Type, IValueCoder>()

    type PrimitiveCoder<'a when 'a : unmanaged>() =
        inherit AbstractValueCoder<'a>()
        override x.Read(r) = r.ReadPrimitive()
        override x.Write(w,v) = w.WritePrimitive(v)

    type PrimitiveArrayCoder<'a when 'a : unmanaged>() =
        inherit AbstractValueCoder<'a[]>()
        override x.Read(r) = r.ReadPrimitiveArray()
        override x.Write(w,v) = w.WritePrimitiveArray(v)


    type StringCoder() =
        inherit AbstractValueCoder<string>()
        override x.Read(r) = r.ReadString()
        override x.Write(w,v) = w.WriteString(v)

    type BoolCoder() =
        inherit AbstractValueCoder<bool>()
        override x.Read(r) = r.ReadBool()
        override x.Write(w,v) = w.WriteBool(v)

    type TypeCoder() =
        inherit AbstractValueCoder<Type>()

        override x.Read(r) =
            code {
                let! name = r.ReadString()
                if name = "(null)" then
                    return null
                else
                    return Type.GetType name
            }

        override x.Write(w, t) =
            if isNull t then
                w.WriteString "(null)"
            else
                w.WriteString t.AssemblyQualifiedName


    type Array1dCoder<'a>(useExternalStore : bool, resolve : Type -> IValueCoder) =
        inherit AbstractValueCoder<'a[]>()

        let elementCoder = resolve typeof<'a> |> unbox<IValueCoder<'a>>

        override x.Read(r : IReader) =
            code {
                let! id = r.ReadPrimitive()
                if id = Guid.Empty then
                    return null
                else
                    let! ref =
                        if useExternalStore then Code.tryLoad id
                        else Code.tryLoadLocal id

                    match ref with
                        | Some r -> 
                            return r

                        | _ ->
                            let! length = r.ReadPrimitive()
                            let arr = Array.zeroCreate length
                            do! Code.storeLocal id arr

                            for i in 0..length-1 do
                                let! v = elementCoder.Read(r)
                                arr.[i] <- v

                            return arr

            }

        override x.Write(w : IWriter, value : 'a[]) =
            code {
                if isNull value then
                    do! w.WritePrimitive Guid.Empty
                else
                    let! (isNew, id) = 
                        if useExternalStore then Code.tryStore value
                        else Code.tryStoreLocal value

                    do! w.WritePrimitive id
                    if isNew then
                        do! w.WritePrimitive value.Length

                        for i in 0..value.Length-1 do
                            do! elementCoder.Write(w, value.[i])




            }

        new(resolve) = Array1dCoder(false, resolve)

    type Array2dCoder<'a>(useExternalStore : bool, resolve : Type -> IValueCoder) =
        inherit AbstractValueCoder<'a[,]>()

        let elementCoder = resolve typeof<'a> |> unbox<IValueCoder<'a>>

        override x.Read(r : IReader) =
            code {
                let! id = r.ReadPrimitive()
                if id = Guid.Empty then
                    return null
                else
                    let! ref =
                        if useExternalStore then Code.tryLoad id
                        else Code.tryLoadLocal id

                    match ref with
                        | Some r -> 
                            return r

                        | _ ->
                            let! sx = r.ReadPrimitive()
                            let! sy = r.ReadPrimitive()

                            let arr = Array2D.zeroCreate sx sy
                            do! Code.storeLocal id arr

                            for x in 0..sx-1 do
                                for y in 0..sy-1 do
                                    let! v = elementCoder.Read(r)
                                    arr.[x,y] <- v

                            return arr

            }

        override x.Write(w : IWriter, value : 'a[,]) =
            code {
                if isNull value then
                    do! w.WritePrimitive Guid.Empty
                else
                    let! (isNew, id) = 
                        if useExternalStore then Code.tryStore value
                        else Code.tryStoreLocal value

                    do! w.WritePrimitive id
                    if isNew then
                        let sx = value.GetLength 0
                        let sy = value.GetLength 1
                        do! w.WritePrimitive sx
                        do! w.WritePrimitive sy

                        for x in 0..sx-1 do
                            for y in 0..sy-1 do
                                do! elementCoder.Write(w, value.[x,y])




            }

        new(resolve) = Array2dCoder(false, resolve)

    type Array3dCoder<'a>(useExternalStore : bool, resolve : Type -> IValueCoder) =
        inherit AbstractValueCoder<'a[,,]>()

        let elementCoder = resolve typeof<'a> |> unbox<IValueCoder<'a>>

        override x.Read(r : IReader) =
            code {
                let! id = r.ReadPrimitive()
                if id = Guid.Empty then
                    return null
                else
                    let! ref =
                        if useExternalStore then Code.tryLoad id
                        else Code.tryLoadLocal id

                    match ref with
                        | Some r -> 
                            return r

                        | _ ->
                            let! sx = r.ReadPrimitive()
                            let! sy = r.ReadPrimitive()
                            let! sz = r.ReadPrimitive()

                            let arr = Array3D.zeroCreate sx sy sz
                            do! Code.storeLocal id arr

                            for x in 0..sx-1 do
                                for y in 0..sy-1 do
                                    for z in 0..sz-1 do
                                        let! v = elementCoder.Read(r)
                                        arr.[x,y,z] <- v

                            return arr

            }

        override x.Write(w : IWriter, value : 'a[,,]) =
            code {
                if isNull value then
                    do! w.WritePrimitive Guid.Empty
                else
                    let! (isNew, id) = 
                        if useExternalStore then Code.tryStore value
                        else Code.tryStoreLocal value

                    do! w.WritePrimitive id
                    if isNew then
                        let sx = value.GetLength 0
                        let sy = value.GetLength 1
                        let sz = value.GetLength 2
                        do! w.WritePrimitive sx
                        do! w.WritePrimitive sy
                        do! w.WritePrimitive sz

                        for x in 0..sx-1 do
                            for y in 0..sy-1 do
                                for z in 0..sz-1 do
                                    do! elementCoder.Write(w, value.[x,y,z])




            }

        new(resolve) = Array3dCoder(false, resolve)




    type DynamicCoder<'a when 'a : not struct and 'a : null>(resolve : Type -> IValueCoder) =
        inherit AbstractValueCoder<'a>()

        static let invariantTypes = ConcurrentDictionary<Type, Type>()

        let resolveInvariant (t : Type) =
            let t = invariantTypes.GetOrAdd(t, fun t -> t.MakeByRefType())
            resolve t

        let typeCoder = resolve typeof<Type> |> unbox<IValueCoder<Type>>

        override x.CodesType = true

        override x.Read(r) =
            code {
                let! t = typeCoder.Read r
                if isNull t then
                    return null
                else
                    let coder = resolveInvariant t
                    return! coder.ReadUnsafe(r)
            }

        override x.Write(w, v) =
            code {
                if isNull v then
                    do! typeCoder.Write(w, null)
                else
                    let t = v.GetType()
                    do! typeCoder.Write(w, t)
                    let coder = resolveInvariant t
                    do! coder.WriteUnsafe(w, v)
            }

    type WithTypeCoder<'a>(inner : IValueCoder<'a>, resolve : Type -> IValueCoder) =
        inherit AbstractValueCoder<'a>()

        let typeCoder = resolve typeof<Type> |> unbox<IValueCoder<Type>>

        override x.CodesType = true

        override x.Write(w,v) =
            code {
                do! typeCoder.Write(w, typeof<'a>)
                do! inner.Write(w, v)
            }

        override x.Read(r) =
            code {
                let! t = typeCoder.Read(r)
                if typeof<'a>.IsAssignableFrom t then
                    return! inner.Read(r)
                else
                    return failwithf "cannot reinterpret %A as %A" t typeof<'a>
            }

    type EnumCoder<'e, 'v when 'v : unmanaged>() =
        inherit AbstractValueCoder<'e>()
        let inner = PrimitiveCoder<'v>()

        override x.Read(r) =
            inner.Read(r) |> State.map unbox<'e>

        override x.Write(w, v) =
            inner.Write(w, unbox v)

    let tryGetSpecialCoderType (t : Type) : Option<Type> =
        CustomCoders.tryGetCustomCoderType t

    let rec resolve (valueType : Type) : Type =
        match tryGetSpecialCoderType valueType with
            | Some coder -> coder
            | _ ->

                let isInvariant =
                    valueType.IsValueType || 
                    valueType.IsSealed || 
                    valueType.IsByRef ||
                    FSharpType.IsTuple valueType // TODO: is that a reasonable assumption??

                let valueType =
                    if valueType.IsByRef then valueType.GetElementType()
                    else valueType

                if valueType.IsBlittable then
                    typedefof<PrimitiveCoder<int>>.MakeGenericType [|valueType|]

                elif valueType.IsEnum then
                    let realType = valueType.GetEnumUnderlyingType()
                    typedefof<EnumCoder<_,int>>.MakeGenericType [|valueType; realType|]

                elif valueType = typeof<string> then
                    typeof<StringCoder>

                elif valueType = typeof<bool> then
                    typeof<BoolCoder>

                elif typeof<Type>.IsAssignableFrom valueType then
                    typeof<TypeCoder>

                elif valueType.IsArray then
                    let rank = valueType.GetArrayRank()
                    let elementType = valueType.GetElementType()

                    match rank with
                        | 1 -> 
                            if elementType.IsBlittable then
                                typedefof<PrimitiveArrayCoder<int>>.MakeGenericType [|elementType|]
                            else
                                typedefof<Array1dCoder<_>>.MakeGenericType [|elementType|]

                        | 2 -> typedefof<Array2dCoder<_>>.MakeGenericType [|elementType|]
                        | 3 -> typedefof<Array3dCoder<_>>.MakeGenericType [|elementType|]
                        | _ -> failwithf "[Coder] arrays of rank %d not supported atm." rank


                else
                    if isInvariant then
                        AutoCoders.createFieldCoder valueType
                    else
                        typedefof<DynamicCoder<_>>.MakeGenericType [| valueType |]

    and get (t : Type) =
        coderTypeCache.GetOrAdd(t, resolve)



module ValueCoder =
    open ValueCoderTypes

    

    type private Resolver private() =
        
        static let resolveLambas =
            typeof<Resolver>.GetMethods(BindingFlags.AnyStatic)
                |> Array.filter (fun mi -> mi.Name = "GetCoder")
                |> Array.map (fun mi -> 
                    let t = mi.GetParameters().[0].ParameterType

                    let delType = System.Linq.Expressions.Expression.GetDelegateType [|t; typeof<IValueCoder>|]
                    let del = Delegate.CreateDelegate(delType, mi)
                    let lambda = DelegateAdapters.wrapUntyped del

                    let funType = typedefof<FSharpFunc<_,_>>.MakeGenericType [| t; typeof<IValueCoder> |]

                    funType, lambda 
                   )
                |> Dictionary.ofArray

        static let coderCache = ConcurrentDictionary<obj, IValueCoder>()

        static let createInstance (coderType : Type) =
            let ctors = 
                coderType.GetConstructors(BindingFlags.AnyInstance)
                    |> Array.filter (fun c -> c.GetParameters().Length = 1)

            let instance =
                ctors |> Array.tryPick (fun ctor ->
                    let arg = ctor.GetParameters().[0].ParameterType
                    match resolveLambas.TryGetValue arg with
                        | (true, arg) -> ctor.Invoke [|arg|] |> unbox<IValueCoder> |> Some
                        | _ -> None
                )

            match instance with
                | Some i -> i
                | None ->
                    let emptyCtor = coderType.GetConstructor(BindingFlags.AnyInstance, Type.DefaultBinder, [||], null)
                    if isNull emptyCtor then failwithf "[Resolver] cannot create coder of type: %A" coderType
                    else emptyCtor.Invoke [||] |> unbox<IValueCoder>



        static member GetCoder (f : FieldInfo) : IValueCoder =
            coderCache.GetOrAdd(f, fun _ ->
                let typeCoder = Resolver.GetCoder(f.FieldType)

                match typeCoder with
                    | :? IReferenceCoder as r ->
                        let useExternStore = false // TODO: real logic here
                        if useExternStore then r.WithExternalStore
                        else r.WithInternalStore
                    | _ ->
                        typeCoder
            )

        static member GetCoder (t : Type) : IValueCoder =
            coderCache.GetOrAdd(t, fun _ ->
                let coderType = get t
                let coder = createInstance coderType
                coder
            )

    let get (t : Type) = Resolver.GetCoder t
    let getField (f : FieldInfo) = Resolver.GetCoder f

    type private CoderGenericImpl<'a>() =
        static let coder = Resolver.GetCoder typeof<'a> |> unbox<IValueCoder<'a>>

        static let topLevelCoder =
            match coder with
                | :? AbstractValueCoder<'a> as c when c.CodesType -> 
                    coder
                | _ ->
                    WithTypeCoder<'a>(coder, Resolver.GetCoder) :> IValueCoder<'a>

        static member Coder = coder
        static member TopLevelCoder = topLevelCoder

    let coder<'a> : IValueCoder<'a> = 
        CoderGenericImpl<'a>.Coder

    let topLevelCoder<'a> : IValueCoder<'a> =
        CoderGenericImpl<'a>.TopLevelCoder


[<AbstractClass; Sealed; Extension>]
type ReaderWriterExtensions() =
    static let emptyState =
        {
            Version         = Version(0,0,1)
            MemberStack     = []
            References      = RefMap.empty
            Values          = Map.empty
            Database        = Unchecked.defaultof<_>
        }

    [<Extension>]
    static member Write(this : IWriter, value : 'a) =
        let c = ValueCoder.topLevelCoder<'a>
        let mutable s = emptyState
        c.Write(this, value).Run(&s)

    [<Extension>]
    static member Read(this : IReader) : 'a =
        let c = ValueCoder.topLevelCoder<'a>
        let mutable s = emptyState
        c.Read(this).Run(&s)



    [<Extension>]
    static member WriteState(this : IWriter, value : 'a) =
        let c = ValueCoder.coder<'a>
        c.Write(this, value)

    [<Extension>]
    static member ReadState(this : IReader) : Code<'a> =
        let c = ValueCoder.coder<'a>
        c.Read(this)



