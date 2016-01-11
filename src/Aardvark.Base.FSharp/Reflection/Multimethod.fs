#if COMPILED
namespace Aardvark.Base
#else
#I @"..\..\..\bin\Release\"
#r @"Aardvark.Base.dll"
#r @"Aardvark.Base.TypeProviders.dll"
#r @"Aardvark.Base.FSharp.dll"
open Aardvark.Base
#endif
open System
open System.Reflection
open System.Collections.Generic
open System.Runtime.InteropServices
open System.Threading
open Microsoft.FSharp.Reflection

module private Helpers =
    
    type State = { assignment : HashMap<Type, Type> }
    type Result<'a> = { run : State -> Option<State * 'a> }

    type ResultBuilder() =
        member x.Bind(r : Result<'a>, f : 'a -> Result<'b>) =
            { run = fun s ->
                match r.run s with
                    | Some (s, v) ->
                        (f v).run s
                    | None ->
                        None
            }

        member x.Return(v : 'a) =
            { run = fun s -> Some(s,v) }

        member x.Zero() =
            { run = fun s -> Some(s, ()) }

        member x.Delay(f : unit -> Result<'a>) =
            { run = fun s -> f().run s }

        member x.Combine(l : Result<unit>, r : Result<'a>) =
            { run = fun s ->
                match l.run s with
                    | Some(s,()) -> r.run s
                    | None -> None
            }

        member x.For(seq : seq<'a>, f : 'a -> Result<unit>) =
            { run = fun s -> 
                let e = seq.GetEnumerator()

                let rec run(s) =
                    if e.MoveNext() then
                        match (f e.Current).run s with
                            | Some(s,()) ->
                                run s
                            | None ->
                                None
                    else
                        Some(s,())

                let res = run s
                e.Dispose()
                res
            }

        member x.While(guard : unit -> bool, body : Result<unit>) =
            { run = fun s ->
                if guard() then
                    match body.run s with
                        | Some (s,()) -> x.While(guard, body).run s
                        | None -> None
                else
                    Some(s,())
            }

    let result = ResultBuilder()

    let assign (t : Type) (o : Type) =
        { run = fun s ->
            match HashMap.tryFind t s.assignment with
                | Some(old) when old <> o ->
                    None
                | _ ->
                    Some({ s with assignment = HashMap.add t o s.assignment }, ())
        }

    let fail<'a> : Result<'a> = { run = fun s -> None }

    let success = { run = fun s -> Some(s, ()) }

    let rec tryInstantiateType (argType : Type) (realType : Type) =
        result {
            let genArg = if argType.IsGenericType then argType.GetGenericTypeDefinition() else argType

            if argType = realType then
                do! success
            elif argType.IsAssignableFrom realType then
                do! success

            elif argType.IsGenericParameter then
                do! assign argType realType

            elif argType.ContainsGenericParameters && realType.IsGenericType && realType.GetGenericTypeDefinition() = genArg then
                let argArgs = argType.GetGenericArguments()
                let realGen = realType.GetGenericTypeDefinition()
                let realArgs = realType.GetGenericArguments()

                for i in 0..realArgs.Length-1 do
                    let r = realArgs.[i]
                    let a = argArgs.[i]
                    do! tryInstantiateType a r

            elif argType.IsInterface then
                let iface = 
                    realType.GetInterfaces()
                        |> Array.tryFind (fun iface -> 
                            let gen = if iface.IsGenericType then iface.GetGenericTypeDefinition() else iface
                            gen = genArg
                        )

                match iface with
                    | Some iface ->
                        if iface.IsGenericType then
                            let ifaceArgs = iface.GetGenericArguments()
                            let argArgs = argType.GetGenericArguments()
                            for i in 0..ifaceArgs.Length - 1 do
                                do! tryInstantiateType argArgs.[i] ifaceArgs.[i]
                        else
                            ()
                    | None ->
                        do! fail

            else
                let baseType = realType.BaseType
                if isNull baseType then do! fail
                else do! tryInstantiateType argType baseType
        }

    let tryInstantiateMethodInfo (mi : MethodInfo) (real : Type[]) =
        result {
            let p = mi.GetParameters()
            if p.Length = real.Length then
                for i in 0..p.Length-1 do
                    do! tryInstantiateType (p.[i].ParameterType) real.[i]
            else
                do! fail
        }
            

    let tryInstantiate (mi : MethodInfo) (args : Type[]) =
        let parameters = mi.GetParameters()
        if parameters.Length = args.Length then
            if mi.IsGenericMethod then
                let mi = mi.GetGenericMethodDefinition()
                let m = tryInstantiateMethodInfo mi args
                match m.run { assignment = HashMap.empty } with
                    | Some (s,()) ->
                        let args = mi.GetGenericArguments() |> Array.map (fun a -> HashMap.find a s.assignment)
                        mi.MakeGenericMethod(args) |> Some

                    | None -> 
                        None
            else
                let works = Array.forall2 (fun (p : ParameterInfo) a -> p.ParameterType.IsAssignableFrom a) parameters args
                if works then Some mi
                else None
        else
            None

type IRuntimeMethod =
    abstract member Invoke : obj[] -> obj

module RuntimeMethodBuilder =
    open System.Reflection.Emit

    let private bAss = AppDomain.CurrentDomain.DefineDynamicAssembly(AssemblyName("IRuntimeMethods"), AssemblyBuilderAccess.RunAndSave)
    let private bMod = bAss.DefineDynamicModule("MainModule")

    let wrap (target : obj) (mi : MethodInfo) =
        let parameters = mi.GetParameters()
        let bType = bMod.DefineType(Guid.NewGuid().ToString())

        let bTarget = 
            if mi.IsStatic then null
            else bType.DefineField("_target", mi.DeclaringType, FieldAttributes.Private)

        let bMeth = bType.DefineMethod("Invoke", MethodAttributes.Public ||| MethodAttributes.Virtual, typeof<obj>, [|typeof<obj[]>|])
        let il = bMeth.GetILGenerator()

        il.Emit(OpCodes.Nop)
        if not mi.IsStatic then
            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldfld, bTarget)
            

        for i in 0..parameters.Length - 1 do
            let p = parameters.[i]
            il.Emit(OpCodes.Ldarg_1)
            il.Emit(OpCodes.Ldc_I4, i)
            il.Emit(OpCodes.Ldelem_Ref)
            il.Emit(OpCodes.Unbox_Any, p.ParameterType)


        il.EmitCall(OpCodes.Call, mi, null)
        if mi.ReturnType = typeof<System.Void> then
            il.Emit(OpCodes.Ldnull)
        elif mi.ReturnType.IsValueType then
            il.Emit(OpCodes.Box, mi.ReturnType)
        else
            il.Emit(OpCodes.Unbox_Any, typeof<obj>)
        il.Emit(OpCodes.Ret)


        bType.AddInterfaceImplementation(typeof<IRuntimeMethod>)
        bType.DefineMethodOverride(bMeth, typeof<IRuntimeMethod>.GetMethod "Invoke")


        if mi.IsStatic then
            let bCtor = bType.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, [||])
            let il = bCtor.GetILGenerator()
            il.Emit(OpCodes.Call, typeof<obj>.GetConstructor [||])
            il.Emit(OpCodes.Ret)
        else
            let bCtor = bType.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, [|mi.DeclaringType|])
            let il = bCtor.GetILGenerator()
            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Call, typeof<obj>.GetConstructor [||])
            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldarg_1)
            il.Emit(OpCodes.Stfld, bTarget)
            il.Emit(OpCodes.Ret)


        let t = bType.CreateType()

        if mi.IsStatic then
            let ctor = t.GetConstructor [||]
            ctor.Invoke [||] |> unbox<IRuntimeMethod>
        else
            let ctor = t.GetConstructor [|mi.DeclaringType|]
            ctor.Invoke [|target|] |> unbox<IRuntimeMethod>

    exception UnknownException of Type[]
    exception UnsupportedTypesException of Type[]

    let inlineCache (argCount : int) (f : Type[] -> Option<IRuntimeMethod>) : obj[] -> obj =
        let created = ref false
        let current = ref Unchecked.defaultof<IRuntimeMethod>

        let all = List<Type[] * IRuntimeMethod>()

        let rebuild(newArgTypes : Type[]) =
            created := true
            let meth = f newArgTypes
            match meth with
                | Some meth ->
                    all.Add(newArgTypes, meth)

                    let bType = bMod.DefineType(Guid.NewGuid().ToString())
                    let bMeth = bType.DefineMethod("Invoke", MethodAttributes.Public ||| MethodAttributes.Virtual, typeof<obj>, [|typeof<obj[]>|])
                    let il = bMeth.GetILGenerator()

                    let bMethods = bType.DefineField("methods", typeof<IRuntimeMethod[]>, FieldAttributes.Private)

                    let typeVars = Array.init argCount (fun i -> il.DeclareLocal(typeof<Type>))
                    let arr = il.DeclareLocal(typeof<Type[]>)

                    for i in 0..argCount-1 do
                        il.Emit(OpCodes.Ldarg_1)
                        il.Emit(OpCodes.Ldc_I4, i)
                        il.Emit(OpCodes.Ldelem_Ref)
                        il.EmitCall(OpCodes.Callvirt, typeof<obj>.GetMethod "GetType", null)
                        il.Emit(OpCodes.Stloc, typeVars.[i])

                    for ai in 0..all.Count-1 do
                        let (t, m) = all.[ai]

                        let fLabel = il.DefineLabel()

                        for i in 0..argCount-1 do
                            let ti = t.[i]
                            il.Emit(OpCodes.Ldloc, typeVars.[i])
                            il.Emit(OpCodes.Ldtoken, ti)
                            il.Emit(OpCodes.Bne_Un, fLabel)

                        il.Emit(OpCodes.Ldarg_0)
                        il.Emit(OpCodes.Ldfld, bMethods)
                        il.Emit(OpCodes.Ldc_I4, ai)
                        il.Emit(OpCodes.Ldelem_Ref)
                        il.Emit(OpCodes.Ldarg_1)
                        il.EmitCall(OpCodes.Callvirt, typeof<IRuntimeMethod>.GetMethod "Invoke", null)
                        il.Emit(OpCodes.Ret)

                        il.MarkLabel(fLabel)


                    il.Emit(OpCodes.Ldc_I4, argCount)
                    il.Emit(OpCodes.Newarr, typeof<Type>)
                    il.Emit(OpCodes.Stloc, arr)

                    for i in 0..argCount-1 do
                        il.Emit(OpCodes.Ldloc, arr)
                        il.Emit(OpCodes.Ldc_I4, i)
                        il.Emit(OpCodes.Ldloc, typeVars.[i])
                        il.Emit(OpCodes.Stelem_Ref)

            
                    il.Emit(OpCodes.Ldloc, arr)
                    il.Emit(OpCodes.Newobj, typeof<UnknownException>.GetConstructor [|typeof<Type[]>|])
                    il.Emit(OpCodes.Throw)
                    il.Emit(OpCodes.Ret)


                    let bCtor = bType.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, [|typeof<IRuntimeMethod[]>|])
                    let il = bCtor.GetILGenerator()
                    il.Emit(OpCodes.Ldarg_0)
                    il.Emit(OpCodes.Call, typeof<obj>.GetConstructor [||])
                    il.Emit(OpCodes.Ldarg_0)
                    il.Emit(OpCodes.Ldarg_1)
                    il.Emit(OpCodes.Stfld, bMethods)
                    il.Emit(OpCodes.Ret)

                    bType.AddInterfaceImplementation(typeof<IRuntimeMethod>)
                    bType.DefineMethodOverride(bMeth, typeof<IRuntimeMethod>.GetMethod "Invoke")

                    let t = bType.CreateType()

                    let ctor = t.GetConstructor [|typeof<IRuntimeMethod[]> |]
                    let methods = all |> Seq.map snd |> Seq.toArray
                    ctor.Invoke([|methods :> obj|]) |> unbox<IRuntimeMethod>
                | _ ->
                    raise <| UnsupportedTypesException(newArgTypes)

        let rec invoke(args) =
            if !created then
                try current.Value.Invoke args
                with UnknownException arr -> 
                    current := rebuild arr
                    current.Value.Invoke(args)
            else
                current := rebuild (args |> Array.map (fun a -> a.GetType()))
                current.Value.Invoke(args)

        invoke

exception MultimethodException              
type Multimethod(parameterCount : int, initial : seq<MethodInfo>) as this =
    let implementations = HashSet<MethodInfo>(initial)
    let mutable initialized = false

    static let instances = Dictionary<Type, obj>()

    static let createInstance (t : Type) =
        lock instances (fun () ->
            match instances.TryGetValue t with
                | (true, i) -> i
                | _ ->
                    let i = Activator.CreateInstance t
                    instances.[t] <- i
                    i
        )



    let methodCache = 
        RuntimeMethodBuilder.inlineCache parameterCount (fun types -> 
            initialized <- true
            match this.TryCreateMethod(typeof<obj>, types) with
                | (true, rm) -> Some rm
                | _ -> None
        )

    member x.Add(mi : MethodInfo) =
        if initialized then
            failwith "cannot modify Multimethod after first call"

        if mi.GetParameters().Length <> parameterCount then
            failwithf "cannot add %A to Multimethod due to parameter-count mismatch" mi

        if mi.IsGenericMethod then
            let mi = mi.GetGenericMethodDefinition()
            implementations.Add(mi) |> ignore
        else
            implementations.Add(mi) |> ignore

    member x.Remove(mi : MethodInfo) =
        if initialized then
            failwith "cannot modify Multimethod after first call"

        implementations.Remove mi |> ignore

    member x.TryGetMethodInfo(retType : Type, types : Type[], [<Out>] result : byref<MethodInfo>) =
        let goodOnes = 
            implementations 
                |> Seq.choose (fun mi -> Helpers.tryInstantiate mi types)
                |> Seq.filter (fun mi -> retType.IsAssignableFrom mi.ReturnType)
                |> Seq.cast
                |> Seq.toArray

        if goodOnes.Length > 0 then
            let selected = 
                Type.DefaultBinder.SelectMethod(
                    BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.InvokeMethod,
                    goodOnes,
                    types,
                    types |> Array.map (fun _ -> ParameterModifier())
                ) |> unbox<MethodInfo>

            result <- selected
            true
        else
            false

    member x.TryCreateMethod(retType : Type, types : Type[], [<Out>] result : byref<IRuntimeMethod>) =
        match x.TryGetMethodInfo(retType, types) with
            | (true, mi) ->
                if mi.IsStatic then result <- RuntimeMethodBuilder.wrap null mi
                else result <- RuntimeMethodBuilder.wrap (createInstance mi.DeclaringType) mi
                true
            | _ ->
                false

    member x.TryInvoke(args : obj[], [<Out>] result : byref<obj>) : bool =
        if args.Length <> parameterCount then
            false
        else
            try
                let res = methodCache args
                result <- res
                true

            with :? RuntimeMethodBuilder.UnsupportedTypesException ->
                false

    member x.Invoke(args : obj[]) =
        if args.Length <> parameterCount then
            raise <| MultimethodException
        else
            try
                methodCache args
            with :? RuntimeMethodBuilder.UnsupportedTypesException ->
                raise <| MultimethodException

    new(parameterCount : int) = 
        Multimethod(parameterCount, Seq.empty)

    new(methods : seq<MethodInfo>) = 
        let count = (methods |> Seq.head).GetParameters().Length
        Multimethod(count, methods)

module MultimethodTest =

    type IA<'a> =
        abstract member V : int
        abstract member Sepp : unit -> int

    type A(v : int) = 
        member x.V = v
        interface IA<float32> with
            member x.V = v
            member x.Sepp() = v

    type B(a : int) =
        inherit A(a)
        member x.A = a

    type C(v : int) =
        interface IA<obj> with
            member x.V = v
            member x.Sepp() = v

    type D(v : int) =
        inherit B(v)

    type Sepp() =
        member x.Value(ia : IA<'a>) =
            printfn "IA<%A>" typeof<'a>
            ia.V

        member x.Value(a : A) =
            printfn "A"
            a.V

        member x.Value(b : B) =
            b.A

    open System.Diagnostics


    let run() =
        let m = Multimethod(typeof<Sepp>.GetMethods() |> Seq.filter (fun mi -> mi.Name = "Value"))


//        let invoke (o : obj) =
//            match m.TryInvoke([|o|]) with 
//                | (true, res) -> printfn "value(%A) = %A" o res
//                | _ -> () 

        let (_,mi) = m.TryGetMethodInfo(typeof<obj>, [|typeof<D>|])
        let target = Activator.CreateInstance mi.DeclaringType

        let res = RuntimeMethodBuilder.wrap target mi

        let ours() =
            let args = [|D 10 :> obj|]
            let sw = Stopwatch()
            let mutable iter = 0
            let mutable res = Unchecked.defaultof<obj>

            for i in 0..1000 do
                m.TryInvoke(args, &res) |> ignore

            sw.Start()
            while sw.Elapsed.TotalMilliseconds < 1000.0 do 
                m.TryInvoke(args, &res) |> ignore
                iter <- iter + 1
            sw.Stop()
            printfn "ours: %.3fns" (1000000.0 * sw.Elapsed.TotalMilliseconds / float iter)

        let sepp() =
            let s = Sepp()
            let args = D 10 :> IA<float32>
            let sw = Stopwatch()
            let mutable iter = 0
            let mutable res = Unchecked.defaultof<obj>

            for i in 0..1000 do
                args.Sepp() |> ignore


            sw.Start()
            while sw.Elapsed.TotalMilliseconds < 1000.0 do 
                args.Sepp() |> ignore
                iter <- iter + 1
            sw.Stop()
            printfn "sepp: %.3fns" (1000000.0 * sw.Elapsed.TotalMilliseconds / float iter)



        let naive() =
            let args = [|D 10 :> obj|]
            let sw = Stopwatch()
            let mutable iter = 0

            for i in 0..1000 do
                mi.Invoke(target, args) |> ignore

            sw.Start()
            while sw.Elapsed.TotalMilliseconds < 1000.0 do 
                mi.Invoke(target, args) |> ignore
                iter <- iter + 1
            sw.Stop()
            printfn "naive: %.3fns" (1000000.0 * sw.Elapsed.TotalMilliseconds / float iter)


        sepp()
        ours()
        naive()

