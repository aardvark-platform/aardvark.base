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
        let baseType = typedefof<AbstractStateValueCoder<_>>.MakeGenericType [|valueType|]
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
                let! coderField = fld "" coderFieldType
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
            do! mem valueType "ReadState" [ typeof<IReader>; stateRefType ] (
                    codegen {
                        // create a new instance of the given type
                        //   stack = [result]
                        let not = typedefof<NewObj<_>>.MakeGenericType [|valueType|]
                        do! IL.call (not.GetMethod("Create", BindingFlags.AnyStatic))

                        // store the result to a local ref
                        //   stack = []
                        let! result = IL.newlocal valueType
                        do! IL.stloc result

                        
                        for fi in 0..fields.Length-1 do
                            let valueField : FieldInfo = fields.[fi]
                            let coderField = coderFields.[fi]

                            // the following sets up arguments for the call to:
                            // ValueCoderExtensions.ReadState (this : IValueCoder<'a>, w : IReader, state : byref<CodeState>)
                            
                            // load the overall result
                            //   stack = [result]
                            do! IL.ldloc result

                            // load the associated valuecoder
                            //   stack = [valueCoder; result]
                            do! IL.ldarg 0
                            do! IL.ldfld coderField

                            // load the writer
                            //   stack = [reader; valueCoder; result]
                            do! IL.ldarg 1

                            // load the state ref
                            //   stack = [&state; writer; valueCoder; result]
                            do! IL.ldarg 2

                            // read the value
                            //   stack = [value; result]
                            let meth = ValueCoderExtensions.GetReadMethod(coderField.FieldType, valueField.FieldType)
                            do! IL.call meth

                            // store the value we just read
                            //   stack = []
                            do! IL.stfld valueField
   
                        // load and return the result
                        do! IL.ldloc result
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
                            do! IL.ldtoken valueType
                            do! IL.ldtoken f
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

    let createUnionCoder (unionType : Type) =
        let cases = FSharpType.GetUnionCases unionType

        // just to be sure we don't see a concrete union-case-type here
        let unionType = cases.[0].DeclaringType
        let baseType = typedefof<AbstractStateValueCoder<_>>.MakeGenericType [|unionType|]
        let stateRefType = typeof<CodeState>.MakeByRefType()

        newtype {
            do! inh baseType

            let! stringCoder = fld "m_stringCoder" typeof<IValueCoder<string>>
            
            let writeStringMethod = ValueCoderExtensions.GetWriteMethod(stringCoder.FieldType, typeof<string>)

            let writeString (str : string) =
                codegen {
                    // stack = [m_stringCoder]
                    do! IL.ldarg 0
                    do! IL.ldfld stringCoder
                    
                    // stack = [writer; m_stringCoder]
                    do! IL.ldarg 1

                    // stack = [str; writer; m_stringCoder]
                    do! IL.ldconst (String str)

                    // stack = [&state; str; writer; m_stringCoder]
                    do! IL.ldarg 3

                    // stack = []
                    do! IL.call writeStringMethod
                }

            do! mem typeof<Void> "WriteState" [typeof<IWriter>; unionType; stateRefType] (
                    codegen {
                        let! nullLabel = IL.newlabel

                        // check if the value is null
                        do! IL.ldarg 2
                        do! IL.jmp False nullLabel

                        for c in cases do
                            // TODO: check if value is instance of case and skip if not
                            
                            
                            ()

                        do! IL.printfn "should be unreachable code"
                        do! IL.ret

                        // write a special tag for null
                        do! IL.mark nullLabel
                        do! writeString "__NULL"
                        do! IL.ret
                    }
                )

            // TODO: read and ctor
        }




module ValueCoderTypes =
    
    let coderTypeCache = ConcurrentDictionary<Type, Type>()
    let coderCache = ConcurrentDictionary<Type, IValueCoder>()

    type PrimitiveCoder<'a when 'a : unmanaged>() =
        inherit AbstractValueCoder<'a>()
        override x.Read(r) = r.ReadPrimitive()
        override x.Write(w,v) = w.WritePrimitive(v)

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
                return Type.GetType name
            }

        override x.Write(w, t) =
            w.WriteString t.AssemblyQualifiedName

    type DynamicCoder<'a when 'a : not struct and 'a : null>(resolve : Type -> IValueCoder) =
        inherit AbstractValueCoder<'a>()

        static let invariantTypes = ConcurrentDictionary<Type, Type>()

        let resolveInvariant (t : Type) =
            let t = invariantTypes.GetOrAdd(t, fun t -> t.MakeByRefType())
            resolve t

        let typeCoder = resolve typeof<Type> |> unbox<IValueCoder<Type>>

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

    
    type IReferenceCoder<'a> =
        abstract member WithInternalStore : IValueCoder<'a>
        abstract member WithExternalStore : IValueCoder<'a>
        abstract member Inner : IValueCoder<'a>

    type ReferenceCoder<'a when 'a : not struct>(externalStore : bool, inner : IValueCoder<'a>) =
        inherit AbstractValueCoder<'a>()

        static let newInstance() =
            System.Runtime.Serialization.FormatterServices.GetSafeUninitializedObject(typeof<'a>) |> unbox<'a>

        interface IReferenceCoder<'a> with
            member x.WithInternalStore = 
                if externalStore then ReferenceCoder(false, inner) :> IValueCoder<'a>
                else x :> IValueCoder<_>

            member x.WithExternalStore = 
                if externalStore then x :> IValueCoder<_>
                else ReferenceCoder(true, inner) :> IValueCoder<'a>

            member x.Inner = inner

        override x.Write(w, v) =
            code {
                
                let! (isNew, id) = 
                    if externalStore then Code.tryStore v
                    else Code.tryStoreLocal v

                do! w.WritePrimitive id
                if isNew then
                    do! inner.Write(w,v)
            }

        override x.Read(r) =
            code {
                let! id = r.ReadPrimitive()
                let! res = 
                    if externalStore then Code.tryLoad id
                    else Code.tryLoadLocal id

                match res with
                    | Some v -> return v
                    | None ->
                        let res = newInstance()
                        do! Code.storeLocal id res

                        let! value = inner.Read r
                        copyAllFields res value

                        return res
            }

        new(resolve : Type -> IValueCoder) = ReferenceCoder<'a>(false, typeof<'a> |> resolve |> unbox)

    [<AbstractClass; Sealed; Extension>]
    type IValueCoderExtensions private() =
        
        [<Extension>]
        static member MakeReferenceCoder(this : IValueCoder<'a>, useExternalStore : bool) =
            match this with
                | :? IReferenceCoder<'a> as r -> 
                    if useExternalStore then r.WithExternalStore
                    else r.WithInternalStore
                | _ ->
                    let t = typeof<'a> 

                    if t.IsArray then
                        failwith "yet another special case"
                    else
                        ReferenceCoder<'a>(useExternalStore, this) :> IValueCoder<_>

        [<Extension>]
        static member MakeReferenceCoder(this : IValueCoder<'a>) =
            IValueCoderExtensions.MakeReferenceCoder(this, false)


    let tryGetSpecialCoderType (t : Type) : Option<Type> =
        // TODO: resolve user-given coder-types
        None



    let rec resolve (valueType : Type) : Type =
        match tryGetSpecialCoderType valueType with
            | Some coder -> coder
            | _ ->
                if valueType.IsBlittable then
                    typedefof<PrimitiveCoder<int>>.MakeGenericType [|valueType|]

                elif valueType = typeof<string> then
                    typeof<StringCoder>

                elif valueType = typeof<bool> then
                    typeof<BoolCoder>

                elif typeof<Type>.IsAssignableFrom valueType then
                    typeof<TypeCoder>

                else
                    let isInvariant =
                        valueType.IsValueType || 
                        valueType.IsSealed || 
                        valueType.IsByRef ||
                        FSharpType.IsTuple valueType

                    if isInvariant then
                        let valueType =
                            if valueType.IsByRef then valueType.GetElementType()
                            else valueType

    
                        if valueType.IsValueType then
                            failwith "ValueTypes not implemented yet"
                        else
                            AutoCoders.createFieldCoder valueType
                    else
                        
                        typedefof<DynamicCoder<_>>.MakeGenericType [| valueType |]

    and get (t : Type) =
        coderTypeCache.GetOrAdd(t, resolve)

    and getCoder (t : Type) =
        let createInstance (t : Type) =
            let ctor = t.GetConstructor(BindingFlags.AnyInstance, Type.DefaultBinder, [|typeof<Type -> IValueCoder>|], null)
            if isNull ctor then
                let ctor = t.GetConstructor(BindingFlags.AnyInstance, Type.DefaultBinder, [||], null)
                if isNull ctor then failwithf "[ValueCoder] cannot create instance of type: %A" t
                else ctor.Invoke [||] |> unbox<IValueCoder>
            else
                ctor.Invoke [| getCoder :> obj |] |> unbox<IValueCoder>

        coderCache.GetOrAdd(t, fun t ->
            let coderType = get t
            let coder = createInstance coderType
            coder
        )
