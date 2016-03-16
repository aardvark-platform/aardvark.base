namespace Aardvark.Base

open System
open System.Reflection
open System.Reflection.Emit
open System.Collections.Concurrent
open Microsoft.FSharp.Reflection

module ValueCoderTypes =
    
    let cache = ConcurrentDictionary<Type, Type>()

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




    type LocalReferenceCoder<'a when 'a : not struct>(inner : IValueCoder<'a>) =
        inherit AbstractValueCoder<'a>()

        static let newInstance() =
            System.Runtime.Serialization.FormatterServices.GetSafeUninitializedObject(typeof<'a>) |> unbox<'a>

        override x.Write(w, v) =
            code {
                let! (isNew, id) = Code.tryStoreLocal v
                do! w.WritePrimitive id
                if isNew then
                    do! inner.Write(w,v)
            }

        override x.Read(r) =
            code {
                let! id = r.ReadPrimitive()
                let! res = Code.tryLoadLocal id
                match res with
                    | Some v -> return v
                    | None ->
                        let res = newInstance()
                        do! Code.storeLocal id res

                        let! value = inner.Read r
                        copyAllFields res value

                        return res
            }

    let rec resolve (valueType : Type) : Type =

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
                FSharpType.IsUnion valueType ||
                FSharpType.IsTuple valueType

            let valueType =
                if valueType.IsByRef then valueType.GetElementType()
                else valueType

            let isNullable =
                not valueType.IsValueType

            failwith ""

    and get (t : Type) =
        cache.GetOrAdd(t, resolve)

