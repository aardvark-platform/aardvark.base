namespace Aardvark.Base.Runtime

open System
open System.Reflection
open System.Runtime.CompilerServices
open Aardvark.Base


[<CustomEquality; NoComparison>]
type private R<'a when 'a : not struct> =
    struct
        val mutable public Value : 'a

        override x.GetHashCode() = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x.Value)
        override x.Equals o =
            match o with
                | :? R<'a> as o -> Object.ReferenceEquals(x.Value, o.Value)
                | _ -> false

        new(v) = { Value = v }
    end

type RefMap<'a, 'b when 'a : not struct> = private { store : HashMap<R<'a>, 'b> }

module RefMap =
    type private EmptyImpl<'a, 'b when 'a : not struct>() =
        static let instance = { store = HashMap.empty<R<'a>, 'b> }
        static member Instance = instance

    let empty<'a, 'b when 'a : not struct> = EmptyImpl<'a, 'b>.Instance

    let add (key : 'a) (value : 'b) (m : RefMap<'a, 'b>) =
        { store = HashMap.add (R key) value m.store }

    let remove (key : 'a) (m : RefMap<'a, 'b>) =
        { store = HashMap.remove (R key) m.store }

    let containsKey (key : 'a) (m : RefMap<'a, 'b>) =
        HashMap.containsKey (R key) m.store

    let update (key : 'a) (f : Option<'b> -> 'b) (m : RefMap<'a, 'b>) =
        let key = R key
        let old = HashMap.tryFind key m.store
        { store = HashMap.add key (f old) m.store }

    let tryFind (key : 'a) (m : RefMap<'a, 'b>) =
        HashMap.tryFind (R key) m.store



[<AutoOpen>]
module TypePropertyExtensions =
    open System.Runtime.InteropServices
    open System.Collections.Concurrent

    type private TypePropertyImpl<'a> private() =
        static let isBlittable =
            if typeof<'a>.IsValueType then
                try
                    let gc = GCHandle.Alloc(Unchecked.defaultof<'a>, GCHandleType.Pinned)
                    gc.Free()
                    true
                with _ ->
                    false
            else
                false

        static member IsBlittable = isBlittable

    let blittable<'a> = TypePropertyImpl<'a>.IsBlittable

    let private blittableCache = ConcurrentDictionary<Type, bool>()

    type BindingFlags with
        static member AnyInstance = 
            BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Instance

        static member AnyStatic = 
            BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Static

        static member Any = 
            BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Static ||| BindingFlags.Instance

    type Type with
        member x.IsBlittable =
            blittableCache.GetOrAdd(x, fun x ->
                let x = 
                    if x.IsByRef then x.GetElementType()
                    else x

                let tb = typedefof<TypePropertyImpl<_>>.MakeGenericType [|x|]
                let prop = tb.GetProperty("IsBlittable", BindingFlags.Static ||| BindingFlags.Public ||| BindingFlags.NonPublic)
                prop.GetValue(null) |> unbox<bool>
            )

[<AutoOpen>]
module ReflectionReferenceHelpers = 
    open System.Reflection.Emit

    type Clone<'a when 'a : not struct> private() =
        static let copyFields =
            let t = typeof<'a>
            let fields = t.GetFields(BindingFlags.AnyInstance)

            let copyMeth =
                DynamicMethod(
                    sprintf "CopyAllFields_%A" t,
                    typeof<Void>,
                    [| t; t |],
                    t,
                    true
                )

            let il = copyMeth.GetILGenerator()

            for f in fields do
                il.Emit(OpCodes.Ldarg_0)
                il.Emit(OpCodes.Ldarg_1)
                il.Emit(OpCodes.Ldfld, f)
                il.Emit(OpCodes.Stfld, f)

            il.Emit(OpCodes.Ret)

            copyMeth.CreateDelegate(typeof<Action<'a, 'a>>)
                |> unbox<Action<'a, 'a>>

        static member CopyFields (target : 'a, source : 'a) =
            copyFields.Invoke(target, source)

    let copyAllFields (target : 'a) (source : 'a) =
        Clone<'a>.CopyFields(target, source)
