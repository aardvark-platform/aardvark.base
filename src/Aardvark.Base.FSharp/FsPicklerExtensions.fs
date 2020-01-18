namespace MBrace.FsPickler

open System
open Aardvark.Base

open FSharp.Data.Adaptive
open FSharp.Data.Traceable

type CustomPicklerProviderAttribute() =
    inherit Attribute()

[<AutoOpen>]
module PicklerExtensions =
    open System.Reflection
    open System.Reflection.Emit
    open MBrace.FsPickler


    let private tryUnifyTypes (decl : Type) (real : Type) =
        let assignment = System.Collections.Generic.Dictionary<Type, Type>()

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

    type private PicklerRegistry(types : list<Type>, fallback : ICustomPicklerRegistry) =

        let picklerGen = typedefof<Pickler<_>>
        let allMeths = types |> List.collect (fun t -> t.GetMethods(BindingFlags.Static ||| BindingFlags.Public ||| BindingFlags.NonPublic) |> Array.toList)

        let upcastToPicker (mi : MethodInfo) =
            let meth = 
                DynamicMethod(
                    sprintf "upcasted.%s" mi.Name,
                    MethodAttributes.Public ||| MethodAttributes.Static,
                    CallingConventions.Standard,
                    typeof<Pickler>,
                    [| typeof<IPicklerResolver> |],
                    typeof<obj>,
                    true
                )
            let il = meth.GetILGenerator()

            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Tailcall)
            il.EmitCall(OpCodes.Call, mi, null)
            il.Emit(OpCodes.Ret)
            let func = 
                meth.CreateDelegate(typeof<Func<IPicklerResolver, Pickler>>) 
                    |> unbox<Func<IPicklerResolver, Pickler>>        
            fun (r : IPicklerResolver) -> func.Invoke(r)

        let genericThings = 
            allMeths
            |> List.filter (fun mi -> mi.GetGenericArguments().Length > 0)
            |> List.choose (fun mi ->
                let ret = mi.ReturnType
                if ret.IsGenericType && ret.GetGenericTypeDefinition() = picklerGen && mi.GetParameters().Length = 1 then
                    let pickledType = ret.GetGenericArguments().[0]

                    let tryInstantiate (t : Type) =
                        match tryUnifyTypes pickledType t with
                            | Some ass ->
                                let targs = mi.GetGenericArguments() |> Array.map (fun a -> ass.[a])
                                let mi = mi.MakeGenericMethod targs
                                Some (upcastToPicker mi)
                                            
                            | None ->
                                None
                                        

                    Some tryInstantiate
                else
                    None
            )

        let nonGenericThings = 
            allMeths
            |> List.filter (fun mi -> mi.GetGenericArguments().Length = 0)
            |> List.choose (fun mi ->
                let ret = mi.ReturnType
                if ret.IsGenericType && ret.GetGenericTypeDefinition() = picklerGen && mi.GetParameters().Length = 1 then
                    let pickledType = ret.GetGenericArguments().[0]

                    let create = upcastToPicker mi
                    Some (pickledType, create)

                else
                    None
            )
            |> Dictionary.ofList
   
        member x.GetRegistration(t : Type) : CustomPicklerRegistration =
            if t.IsGenericType then
                match genericThings |> List.tryPick (fun a -> a t) with
                | Some r -> 
                    CustomPicklerRegistration.CustomPickler r
                | None ->
                    match nonGenericThings.TryGetValue t with   
                    | (true, r) -> CustomPicklerRegistration.CustomPickler r
                    | _ -> fallback.GetRegistration t
            else
                match nonGenericThings.TryGetValue t with   
                | (true, r) -> CustomPicklerRegistration.CustomPickler r
                | _ -> fallback.GetRegistration t

        interface ICustomPicklerRegistry with
            member x.GetRegistration(t : Type) = x.GetRegistration t

    let private installCustomPicklers(types : list<Type>) =
        let instance = MBrace.FsPickler.PicklerCache.Instance
        lock instance (fun () ->
            let t = instance.GetType()

            let registryField =
                t.GetFields(BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Instance)
                |> Array.tryFind (fun f -> f.FieldType = typeof<ICustomPicklerRegistry>)

            match registryField with
            | Some field ->
                let old = 
                    match field.GetValue(instance) with
                    | null -> EmptyPicklerRegistry() :> ICustomPicklerRegistry
                    | :? ICustomPicklerRegistry as o -> o
                    | _ -> EmptyPicklerRegistry() :> ICustomPicklerRegistry
                let reg = PicklerRegistry(types, old) :> ICustomPicklerRegistry
                field.SetValue(instance, reg)
            | None ->
                Log.warn "cannot register custom picklers"
        )

    type FsPickler with
        static member AddCustomPicklers (types : list<Type>) =
            installCustomPicklers types

module CustomPicklerProvider =
    open MBrace.FsPickler

    let mutable private initialized = false

    [<OnAardvarkInit>]
    let init() =
        if not initialized then
            initialized <- true
            let custom = 
                Introspection.GetAllTypesWithAttribute<CustomPicklerProviderAttribute>()
                |> Seq.map (fun struct (t,_) -> t)
                |> Seq.filter (not << isNull)
                |> Seq.toList
                
            match custom with
            | [] -> 
                Report.Line(4, "no custom picklers found")
            | _ -> 
                for c in custom do
                    Report.Line(4, "installing pickler: {0}", c)
                FsPickler.AddCustomPicklers custom
