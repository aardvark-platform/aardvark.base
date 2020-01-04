namespace Aardvark.Base.Runtime

open System
open System.Reflection
open System.Collections.Generic
open System.Runtime.CompilerServices
open Microsoft.FSharp.Reflection
open Aardvark.Base
open FSharp.Data.Adaptive


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
        { store = HashMap.update key f m.store }

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


module OverloadResolution =
    type ExtensionMethod =
        {
            definition  : MethodInfo
            selfType    : Type
            parameters  : Type[]
        }

    let private extensions =
        Introspection.GetAllMethodsWithAttribute<ExtensionAttribute>()
            |> Seq.choose (fun struct (meth, _) ->

                let parameterTypes = meth.GetParameters() |> Array.map (fun p -> p.ParameterType)
                
                if parameterTypes.Length > 0 then
                    //printfn "extension %s(%s)" meth.Name parameterTypes.[0].FullName
                    Some {
                        definition = meth
                        selfType = parameterTypes.[0]
                        parameters = Array.sub parameterTypes 1 (parameterTypes.Length-1)
                    }
                else
                    None
               )
            |> Seq.toArray
            |> Seq.groupBy (fun m -> m.definition.Name)
            |> Seq.map (fun (name, exts) -> name, Seq.toArray (HashSet exts))
            |> Dictionary.ofSeq

    let tryUnifyTypes (decl : Type) (real : Type) =
        let assignment = Dictionary<Type, Type>()

        let rec recurse (decl : Type) (real : Type) =
            if decl = real then
                true

            elif decl.IsGenericParameter then
                match assignment.TryGetValue decl with
                    | (true, old) ->
                        if old.IsAssignableFrom real then 
                            true

                        elif real.IsAssignableFrom old then
                            assignment.[decl] <- real
                            true

                        else 
                            false
                    | _ ->
                        assignment.[decl] <- real
                        true
            
            elif decl.IsArray then
                if real.IsArray then
                    let de = decl.GetElementType()
                    let re = real.GetElementType()
                    recurse de re
                else
                    false

            elif decl.ContainsGenericParameters then
                let dgen = decl.GetGenericTypeDefinition()
                let rgen = 
                    if real.IsGenericType then real.GetGenericTypeDefinition()
                    else real

                if dgen = rgen then
                    let dargs = decl.GetGenericArguments()
                    let rargs = real.GetGenericArguments()
                    Array.forall2 recurse dargs rargs

                elif dgen.IsInterface then
                    let rface = real.GetInterface(dgen.FullName)
                    if isNull rface then
                        false
                    else
                        recurse decl rface

                elif not (isNull real.BaseType) then
                    recurse decl real.BaseType

                else
                    false

            elif decl.IsAssignableFrom real then
                true

            else
                false


        if recurse decl real then
            Some (assignment |> Dictionary.toSeq |> HashMap.ofSeq)
        else
            None

    let trySpecialize (self : Type) (m : ExtensionMethod)  =
        match tryUnifyTypes m.selfType self with
            | Some ass ->
                if m.definition.IsGenericMethodDefinition then
                    let holes = m.definition.GetGenericArguments()

                    let filled = 
                        holes |> Array.map (fun h ->
                            match HashMap.tryFind h ass with
                                | Some r -> r
                                | None -> h
                        )

                    let grounded = 
                        filled |> Array.forall (fun t -> t.ContainsGenericParameters |> not)

                    if grounded then
                        let m = m.definition.MakeGenericMethod filled
                        let parTypes = m.GetParameters() |> Array.map (fun p -> p.ParameterType)
                        Some {
                            definition = m
                            selfType = parTypes.[0]
                            parameters = Array.skip 1 parTypes
                        }
                    else
                        None

                else
                    Some m
            
            | _ ->
                None

    let tryFindExtension (self : Type) (name : string) (args : Type[]) =
        match extensions.TryGetValue name with
            | (true, possible) ->
                let good = 
                    possible 
                        |> Array.filter (fun ext -> ext.parameters.Length = args.Length)
                        |> Array.choose (trySpecialize self)
                        |> Array.filter (fun ext ->
                            Array.forall2 (fun (l : Type) r -> l.IsAssignableFrom r) ext.parameters args
                           )

                if good.Length = 0 then
                    None
                elif good.Length = 1 then
                    Some good.[0].definition
                else
                    let selected = 
                        Type.DefaultBinder.SelectMethod(
                            BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.Static,
                            good |> Array.map (fun m -> m.definition :> MethodBase),
                            Array.append [|self|] args,
                            null
                        )

                    if isNull selected then None
                    else Some (unbox<MethodInfo> selected)
            
            | _ -> None

[<AbstractClass; Sealed; Extension>]
type TypeExtensions private() =

        [<Extension>]
        static member TryUnifyWith (x : Type, concrete : Type) =
            OverloadResolution.tryUnifyTypes x concrete

        [<Extension>]
        static member TryGetExtensionMethod(x : Type, name : string, args : Type[]) =
            OverloadResolution.tryFindExtension x name args

        [<Extension>]
        static member UnifyWith (x : Type, concrete : Type) =
            OverloadResolution.tryUnifyTypes x concrete |> Option.get

        [<Extension>]
        static member GetExtensionMethod(x : Type, name : string, args : Type[]) =
            match OverloadResolution.tryFindExtension x name args with
                | Some mi -> mi
                | None -> null

        [<Extension>]
        static member GetMethodOrExtension(x : Type, name : string, args : Type[]) =
            let meth = x.GetMethod(name, BindingFlags.AnyInstance, Type.DefaultBinder, args, null)
            if isNull meth then
                match OverloadResolution.tryFindExtension x name args with
                    | Some mi -> mi
                    | None -> null
            else
                meth

