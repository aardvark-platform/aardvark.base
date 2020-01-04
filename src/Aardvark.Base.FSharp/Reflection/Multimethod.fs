namespace Aardvark.Base

open System
open System.Reflection
open System.Collections.Generic
open System.Runtime.InteropServices
open System.Threading
open Microsoft.FSharp.Reflection
open FSharp.Data.Adaptive

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
    
    
    let internal bAss = AssemblyBuilder.DefineDynamicAssembly(AssemblyName("IRuntimeMethods"), AssemblyBuilderAccess.Run)
    let internal bMod = bAss.DefineDynamicModule("MainModule")

    exception UnknownException of Type[]
    exception UnsupportedTypesException of Type[]

    
    type MultimethodImpl(argCount : int, f : Type[] -> Option<obj * MethodInfo>) =
        let all = List<Type[] * obj * MethodInfo>()
        let current = ref unbox
        let mutable callCount = 0
        let mutable init = true

        let buildMatch(x : MultimethodImpl) =
            let funType = typeof<FSharpFunc<obj[], obj>>
            let bType = bMod.DefineType(Guid.NewGuid().ToString(), TypeAttributes.Class ||| TypeAttributes.Public, funType)
            let bMeth = bType.DefineMethod("Invoke", MethodAttributes.Public ||| MethodAttributes.Virtual, typeof<obj>, [|typeof<obj[]>|])
            let il = bMeth.GetILGenerator()

            let bTargets = bType.DefineField("targets", typeof<obj[]>, FieldAttributes.Private)
            let bSelf = bType.DefineField("parent", typeof<MultimethodImpl>, FieldAttributes.Private)

            let args = Array.init argCount (fun _ -> il.DeclareLocal(typeof<obj>))
            let typeVars = Array.init argCount (fun i -> if sizeof<nativeint> = 8 then il.DeclareLocal(typeof<int64>) else il.DeclareLocal(typeof<int>))
            let arr = il.DeclareLocal(typeof<Type[]>)

            for i in 0..argCount-1 do
                // load the args into args.[i]
                il.Emit(OpCodes.Ldarg_1)
                il.Emit(OpCodes.Ldc_I4, i)
                il.Emit(OpCodes.Ldelem, typeof<obj>)
                il.Emit(OpCodes.Stloc, args.[i])

                // read the object's type-pointer
                il.Emit(OpCodes.Ldloc, args.[i])
                if sizeof<nativeint> = 8 then il.Emit(OpCodes.Ldobj, typeof<int64>)
                else il.Emit(OpCodes.Ldobj, typeof<int>)
                il.Emit(OpCodes.Stloc, typeVars.[i])

            for ai in 0..all.Count-1 do
                let (t, target, meth) = all.[ai]
                let paramters = meth.GetParameters()

                let fLabel = il.DefineLabel()

                // check all type-ids
                for i in 0..argCount-1 do
                    let ti = t.[i]
                    il.Emit(OpCodes.Ldloc, typeVars.[i])
                    if sizeof<nativeint> = 8 then il.Emit(OpCodes.Ldc_I8, int64 ti.TypeHandle.Value)
                    else il.Emit(OpCodes.Ldc_I4, int ti.TypeHandle.Value)
                    il.Emit(OpCodes.Bne_Un_S, fLabel)

                // load the target (if any)
                if not meth.IsStatic then
                    il.Emit(OpCodes.Ldarg_0)
                    il.Emit(OpCodes.Ldfld, bTargets)
                    il.Emit(OpCodes.Ldc_I4, ai)
                    il.Emit(OpCodes.Ldelem_Ref)

                        
                // load and unbox the arguments
                for i in 0..argCount-1 do
                    let at = paramters.[i].ParameterType
                    il.Emit(OpCodes.Ldloc, args.[i])

                    // strange version of a cast
                    let tmp = il.DeclareLocal(at)
                    il.Emit(OpCodes.Stloc, tmp)
                    il.Emit(OpCodes.Ldloc, tmp)


                // finally call the method
                if meth.ReturnType = typeof<obj> then
                    il.Emit(OpCodes.Tailcall)
                    il.EmitCall(OpCodes.Call, meth, null)
                    il.Emit(OpCodes.Ret)
                else
                    il.EmitCall(OpCodes.Call, meth, null)
                    if meth.ReturnType = typeof<System.Void> then
                        il.Emit(OpCodes.Ldnull)
                    elif meth.ReturnType.IsValueType then
                        il.Emit(OpCodes.Box, meth.ReturnType)
                    else
                        let tmp = il.DeclareLocal(typeof<obj>)
                        il.Emit(OpCodes.Stloc, tmp)
                        il.Emit(OpCodes.Ldloc, tmp)

                    il.Emit(OpCodes.Ret)

                il.MarkLabel(fLabel)


            il.Emit(OpCodes.Ldc_I4, argCount)
            il.Emit(OpCodes.Newarr, typeof<Type>)
            il.Emit(OpCodes.Stloc, arr)

            for i in 0..argCount-1 do
                il.Emit(OpCodes.Ldloc, arr)
                il.Emit(OpCodes.Ldc_I4, i)

                il.Emit(OpCodes.Ldloc, args.[i])
                il.EmitCall(OpCodes.Callvirt, typeof<obj>.GetMethod "GetType", null)

                il.Emit(OpCodes.Stelem_Ref)

            
            let rebuild = typeof<MultimethodImpl>.GetMethod("Rebuild", BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.Public)
            let invoke = typeof<MultimethodImpl>.GetMethod("Invoke")
            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldfld, bSelf)
            il.Emit(OpCodes.Dup)
            il.Emit(OpCodes.Ldloc, arr)
            
            il.EmitCall(OpCodes.Call, rebuild, null)
           
            il.Emit(OpCodes.Ldarg_1)
            il.Emit(OpCodes.Tailcall)
            il.EmitCall(OpCodes.Call, invoke, null)
            il.Emit(OpCodes.Ret)
                


            let bCtor = bType.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, [|typeof<obj[]>; typeof<MultimethodImpl>|])
            let il = bCtor.GetILGenerator()
            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Call, typeof<obj>.GetConstructor [||])
            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldarg_1)
            il.Emit(OpCodes.Stfld, bTargets)

            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldarg_2)
            il.Emit(OpCodes.Stfld, bSelf)

            il.Emit(OpCodes.Ret)

            bType.AddInterfaceImplementation(typeof<IRuntimeMethod>)
            bType.DefineMethodOverride(bMeth, typeof<IRuntimeMethod>.GetMethod "Invoke")
            bType.DefineMethodOverride(bMeth, funType.GetMethod "Invoke")

            let t = bType.CreateTypeInfo()

            let ctor = t.GetConstructor [|typeof<obj[]>; typeof<MultimethodImpl>|]
            let targets = all |> Seq.map (fun (_,t,_) -> t) |> Seq.toArray
            ctor.Invoke([|targets :> obj; x :> obj|]) |> unbox<obj[] -> obj>

        let buildTable(x : MultimethodImpl) =
            if argCount <> 1 then
                failwith "jump table only works with one argument atm."

            let funType = typeof<FSharpFunc<obj[], obj>>
            let bType = bMod.DefineType(Guid.NewGuid().ToString(), TypeAttributes.Class ||| TypeAttributes.Public, funType)
            let bMeth = bType.DefineMethod("Invoke", MethodAttributes.Public ||| MethodAttributes.Virtual, typeof<obj>, [|typeof<obj[]>|])
            let il = bMeth.GetILGenerator()

            let bTargets = bType.DefineField("targets", typeof<obj[]>, FieldAttributes.Private)
            let bSelf = bType.DefineField("parent", typeof<MultimethodImpl>, FieldAttributes.Private)

            let args = Array.init argCount (fun _ -> il.DeclareLocal(typeof<obj>))
            let typeVars = Array.init argCount (fun i -> if sizeof<nativeint> = 8 then il.DeclareLocal(typeof<int64>) else il.DeclareLocal(typeof<int>))
            let arr = il.DeclareLocal(typeof<Type[]>)

            for i in 0..argCount-1 do
                // load the args into args.[i]
                il.Emit(OpCodes.Ldarg_1)
                il.Emit(OpCodes.Ldc_I4, i)
                il.Emit(OpCodes.Ldelem, typeof<obj>)
                il.Emit(OpCodes.Stloc, args.[i])

                // read the object's type-pointer
                il.Emit(OpCodes.Ldloc, args.[i])
                if sizeof<nativeint> = 8 then il.Emit(OpCodes.Ldobj, typeof<int64>)
                else il.Emit(OpCodes.Ldobj, typeof<int>)
                il.Emit(OpCodes.Stloc, typeVars.[i])

            
            let minId = all |> Seq.map (fun (t,_,_) -> t.[0].TypeHandle.Value) |> Seq.min

            let rec createTable (size : int) =
                let table = Array.zeroCreate size
                let mutable success = true
                
                for (t,tar,meth) in all do
                    let e = int (t.[0].TypeHandle.Value - minId) % table.Length
                    match table.[e] with
                        | None -> 
                            table.[e] <- Some (t, tar, meth)
                        | _ ->
                            success <- false

                if success then
                    table
                else
                    createTable (size + 1)

            let table = createTable all.Count

            let notFound = il.DefineLabel()
            let labels = 
                table |> Array.map (fun f ->
                    il.DefineLabel()
                )

            il.Emit(OpCodes.Ldloc, typeVars.[0])
            if sizeof<nativeint> = 8 then il.Emit(OpCodes.Ldc_I8, int64 minId)
            else il.Emit(OpCodes.Ldc_I4, int minId)
            il.Emit(OpCodes.Sub)

            if sizeof<nativeint> = 8 then il.Emit(OpCodes.Ldc_I8, int64 labels.Length)
            else il.Emit(OpCodes.Ldc_I4, int64 labels.Length)
            il.Emit(OpCodes.Rem)
            il.Emit(OpCodes.Conv_I4)

            il.Emit(OpCodes.Switch, labels)
            il.Emit(OpCodes.Br, notFound)

            for ai in 0..table.Length-1 do
                match table.[ai] with
                    | Some (t, target, meth) -> 
                        let paramters = meth.GetParameters()

                        il.MarkLabel(labels.[ai])

                        // check all type-ids
                        for i in 0..argCount-1 do
                            let ti = t.[i]
                            il.Emit(OpCodes.Ldloc, typeVars.[i])
                            if sizeof<nativeint> = 8 then il.Emit(OpCodes.Ldc_I8, int64 ti.TypeHandle.Value)
                            else il.Emit(OpCodes.Ldc_I4, int ti.TypeHandle.Value)
                            il.Emit(OpCodes.Bne_Un, notFound)

                        // load the target (if any)
                        if not meth.IsStatic then
                            il.Emit(OpCodes.Ldarg_0)
                            il.Emit(OpCodes.Ldfld, bTargets)
                            il.Emit(OpCodes.Ldc_I4, ai)
                            il.Emit(OpCodes.Ldelem_Ref)

                        
                        // load and unbox the arguments
                        for i in 0..argCount-1 do
                            let at = paramters.[i].ParameterType
                            il.Emit(OpCodes.Ldloc, args.[i])

                            // strange version of a cast
                            let tmp = il.DeclareLocal(at)
                            il.Emit(OpCodes.Stloc, tmp)
                            il.Emit(OpCodes.Ldloc, tmp)


                        // finally call the method
                        if meth.ReturnType = typeof<obj> then
                            il.Emit(OpCodes.Tailcall)
                            il.EmitCall(OpCodes.Call, meth, null)
                            il.Emit(OpCodes.Ret)
                        else
                            il.EmitCall(OpCodes.Call, meth, null)
                            if meth.ReturnType = typeof<System.Void> then
                                il.Emit(OpCodes.Ldnull)
                            elif meth.ReturnType.IsValueType then
                                il.Emit(OpCodes.Box, meth.ReturnType)
                            else
                                let tmp = il.DeclareLocal(typeof<obj>)
                                il.Emit(OpCodes.Stloc, tmp)
                                il.Emit(OpCodes.Ldloc, tmp)

                            il.Emit(OpCodes.Ret)
                    | None ->
                        il.MarkLabel(labels.[ai])
                        il.Emit(OpCodes.Br, notFound)

            il.MarkLabel(notFound)
            il.Emit(OpCodes.Ldc_I4, argCount)
            il.Emit(OpCodes.Newarr, typeof<Type>)
            il.Emit(OpCodes.Stloc, arr)

            for i in 0..argCount-1 do
                il.Emit(OpCodes.Ldloc, arr)
                il.Emit(OpCodes.Ldc_I4, i)

                il.Emit(OpCodes.Ldloc, args.[i])
                il.EmitCall(OpCodes.Callvirt, typeof<obj>.GetMethod "GetType", null)

                il.Emit(OpCodes.Stelem_Ref)

            
            let rebuild = typeof<MultimethodImpl>.GetMethod("Rebuild", BindingFlags.NonPublic ||| BindingFlags.Instance ||| BindingFlags.Public)
            let invoke = typeof<MultimethodImpl>.GetMethod("Invoke")
            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldfld, bSelf)
            il.Emit(OpCodes.Dup)
            il.Emit(OpCodes.Ldloc, arr)
            
            il.EmitCall(OpCodes.Call, rebuild, null)
           
            il.Emit(OpCodes.Ldarg_1)
            il.Emit(OpCodes.Tailcall)
            il.EmitCall(OpCodes.Call, invoke, null)
            il.Emit(OpCodes.Ret)
                


            let bCtor = bType.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, [|typeof<obj[]>; typeof<MultimethodImpl>|])
            let il = bCtor.GetILGenerator()
            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Call, typeof<obj>.GetConstructor [||])
            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldarg_1)
            il.Emit(OpCodes.Stfld, bTargets)

            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldarg_2)
            il.Emit(OpCodes.Stfld, bSelf)

            il.Emit(OpCodes.Ret)

            bType.AddInterfaceImplementation(typeof<IRuntimeMethod>)
            bType.DefineMethodOverride(bMeth, typeof<IRuntimeMethod>.GetMethod "Invoke")
            bType.DefineMethodOverride(bMeth, funType.GetMethod "Invoke")

            let t = bType.CreateTypeInfo()

            let ctor = t.GetConstructor [|typeof<obj[]>; typeof<MultimethodImpl>|]
            let targets = all |> Seq.map (fun (_,t,_) -> t) |> Seq.toArray
            ctor.Invoke([|targets :> obj; x :> obj|]) |> unbox<obj[] -> obj>


        let rebuild (self : MultimethodImpl) (newArgTypes : Type[]) =
            let meth = f newArgTypes
            match meth with
                | Some (target, meth) ->
                    all.Add(newArgTypes, target, meth)
                    if argCount < 0 then
                        current := buildTable(self)
                    else
                        current := buildMatch(self)
                | _ ->
                    raise <| UnsupportedTypesException(newArgTypes)

        member x.Init() =
            current :=
                fun args ->
                    rebuild x (args |> Array.map (fun a -> a.GetType()))
                    current.Value(args)

        
        member x.Rebuild(newArgTypes : Type[]) =
            rebuild x newArgTypes

        member x.Invoke(args : obj[]) =
            current.Value args

    let inlineCache (argCount : int) (f : Type[] -> Option<obj * MethodInfo>) =
        let f = MultimethodImpl(argCount, f)
        f.Init()
        f

exception MultimethodException              
type Multimethod(parameterCount : int, initial : seq<MethodInfo * obj>)  =
    let implementations = Dictionary.ofSeq (initial)
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

    static let createTarget (mi : MethodInfo) =
        if mi.IsStatic then null
        else createInstance mi.DeclaringType

    let tryGetMethodInfo(retType : Type) (types : Type[])=
        let goodOnes = 
            implementations.Keys
                |> Seq.choose (fun mi -> Helpers.tryInstantiate mi types)
                |> Seq.filter (fun mi -> retType.IsAssignableFrom mi.ReturnType)
                |> Seq.cast
                |> Seq.toArray

        if goodOnes.Length > 0 then
            let selected =
                if goodOnes.Length = 1 then goodOnes.[0] |> unbox<MethodInfo>
                else 
                    Type.DefaultBinder.SelectMethod(
                        BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.InvokeMethod,
                        goodOnes,
                        types,
                        types |> Array.map (fun _ -> ParameterModifier())
                    ) |> unbox<MethodInfo>
                
            if selected.IsGenericMethod then
                Some (selected, implementations.[selected.GetGenericMethodDefinition()])
            else
                Some (selected, implementations.[selected])
        else
            None

    let methodCache = 
        RuntimeMethodBuilder.inlineCache parameterCount (fun types -> 
            initialized <- true
            match tryGetMethodInfo typeof<obj> types with
                | Some(mi,target) -> Some(target, mi)
                | _ -> None
        )


    static member GetTarget (mi : MethodInfo) = createTarget mi 

    member x.Add(mi : MethodInfo) =
        if initialized then
            failwith "cannot modify Multimethod after first call"

        if mi.GetParameters().Length <> parameterCount then
            failwithf "cannot add %A to Multimethod due to parameter-count mismatch" mi

        if mi.IsGenericMethod then
            let mi = mi.GetGenericMethodDefinition()
            implementations.Add(mi, createTarget mi) |> ignore
        else
            implementations.Add(mi, createTarget mi) |> ignore

    member x.Add(target : obj, mi : MethodInfo) =
        if initialized then
            failwith "cannot modify Multimethod after first call"

        if mi.GetParameters().Length <> parameterCount then
            failwithf "cannot add %A to Multimethod due to parameter-count mismatch" mi

        if mi.IsGenericMethod then
            let mi = mi.GetGenericMethodDefinition()
            implementations.Add(mi, target) |> ignore
        else
            implementations.Add(mi, target) |> ignore


    member x.TryInvoke(args : obj[], [<Out>] result : byref<obj>) : bool =
        if args.Length <> parameterCount then
            false
        else
            try
                let res = methodCache.Invoke args
                result <- res
                true

            with :? RuntimeMethodBuilder.UnsupportedTypesException ->
                false

    member x.InvokeUnsafe(args : obj[]) : obj =
        methodCache.Invoke args


    member x.Invoke(args : obj[]) =
        if args.Length <> parameterCount then
            raise <| MultimethodException
        else
            try
                methodCache.Invoke args
            with :? RuntimeMethodBuilder.UnsupportedTypesException ->
                raise <| MultimethodException

    new(parameterCount : int) = 
        Multimethod(parameterCount, Seq.empty)

    new(methods : seq<MethodInfo>) = 
        let count = (methods |> Seq.head).GetParameters().Length
        Multimethod(count, methods |> Seq.map (fun mi -> mi, createTarget mi))

module MultimethodTest =


    type IA<'a> =
        abstract member V : obj
        abstract member Sepp : unit -> obj

    type A = 
        class
            val mutable public V : obj

            abstract member Sepp : unit -> obj
            default x.Sepp() = x.V

            interface IA<float32> with
                member x.V = x.V
                member x.Sepp() = x.Sepp()

            new(v) = { V = v }
        end

    type B(a : obj) =
        inherit A(a)

        override x.Sepp() = x.V

        member x.A = a

    type C(v : obj) =
        interface IA<obj> with
            member x.V = v
            member x.Sepp() = v

    type D(v : obj) = inherit B(v)
    type E(v : obj) = inherit B(v)
    type F(v : obj) = inherit B(v)
    type G(v : obj) = inherit B(v)
    type H(v : obj) = inherit B(v)
    type I(v : obj) = inherit B(v)
    type J(v : obj) = inherit B(v)
    type K(v : obj) = inherit B(v)
    type L(v : obj) = inherit B(v)
    type M(v : obj) = inherit B(v)
    type N(v : obj) = inherit B(v)

    open System.Runtime.CompilerServices

    type Sepp() =
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Value(ia : IA<'a>) =
            ia.V
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Value(a : A) =
            a.V
            
        [<MethodImpl(MethodImplOptions.NoInlining)>]
        static member Value(b : B) =
            b.V

    open System.Diagnostics
    open System.IO
    let inline (==) (a : 'a) (b : 'a) =
        System.Object.ReferenceEquals(a, b)

    let inline (!=) (a : 'a) (b : 'a) =
        not (System.Object.ReferenceEquals(a, b))


    let dt = typeof<D>
    let dispatcher (o : obj[]) =
        let a = o.[0]
        match a with
            | :? D as d -> Sepp.Value(d)
            | _ -> 
                let t = a.GetType()
                raise <| RuntimeMethodBuilder.UnsupportedTypesException [|t|]


    let run() =
        let m = Multimethod(typeof<Sepp>.GetMethods() |> Seq.filter (fun mi -> mi.Name = "Value"))

        let iterations = 1 <<< 26

        let nop() =
            let args = [|D 10 :> obj|]
            let sw = Stopwatch()
            let mutable iter = 0

            System.GC.Collect()
            System.GC.WaitForFullGCComplete() |> ignore

            sw.Start()
            for i in 1..iterations do
                iter <- iter + 1
            sw.Stop()
            let t = (1000000.0 * sw.Elapsed.TotalMilliseconds / float iter)
            printfn "nop:  %.3fns" t
            t


        let ours() =
            let args = [|D 10 :> obj|]
            let sw = Stopwatch()
            let mutable iter = 0
            let mutable res = Unchecked.defaultof<obj>

            let badInit() =
                let mutable res = Unchecked.defaultof<obj>
                m.TryInvoke([|A 10 :> obj|], &res) |> ignore
                m.TryInvoke([|B 10 :> obj|], &res) |> ignore
                m.TryInvoke([|C 10 :> obj|], &res) |> ignore
                m.TryInvoke([|E 10 :> obj|], &res) |> ignore
                m.TryInvoke([|F 10 :> obj|], &res) |> ignore
                m.TryInvoke([|G 10 :> obj|], &res) |> ignore
                m.TryInvoke([|H 10 :> obj|], &res) |> ignore
                m.TryInvoke([|I 10 :> obj|], &res) |> ignore
                m.TryInvoke([|J 10 :> obj|], &res) |> ignore
                m.TryInvoke([|K 10 :> obj|], &res) |> ignore
                m.TryInvoke([|L 10 :> obj|], &res) |> ignore
                m.TryInvoke([|M 10 :> obj|], &res) |> ignore
                m.TryInvoke([|N 10 :> obj|], &res) |> ignore


            
            //badInit()
            m.TryInvoke(args, &res) |> ignore

            for i in 0..1000 do
                m.InvokeUnsafe(args) |> ignore


            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore

            sw.Start()
            for i in 1..iterations do
                m.InvokeUnsafe(args) |> ignore
                iter <- iter + 1
            sw.Stop()
            let t = (1000000.0 * sw.Elapsed.TotalMilliseconds / float iter)
            printfn "ours: %.3fns" t
            t

        let dispatcher() =
            let args = [|D 10 :> obj|]
            let sw = Stopwatch()
            let mutable iter = 0
            let mutable res = Unchecked.defaultof<obj>

            for i in 0..1000 do
                dispatcher(args) |> ignore


            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore

            sw.Start()
            for i in 1..iterations do
                dispatcher(args) |> ignore
                iter <- iter + 1
            sw.Stop()
            let t = (1000000.0 * sw.Elapsed.TotalMilliseconds / float iter)
            printfn "disp: %.3fns" t
            t

        let virtualCall() =
            let s = Sepp()
            let args = D 10 :> IA<float32>
            let sw = Stopwatch()
            let mutable iter = 0
            let mutable res = Unchecked.defaultof<obj>

            for i in 0..1000 do
                args.Sepp() |> ignore


            System.GC.Collect(3)
            System.GC.WaitForFullGCComplete() |> ignore

            sw.Start()
            for i in 1..iterations do
                args.Sepp() |> ignore
                iter <- iter + 1
            sw.Stop()
            let t = (1000000.0 * sw.Elapsed.TotalMilliseconds / float iter)
            printfn "virt: %.3fns" t
            t

        let staticCall() =
            let s = Sepp()
            let args = D 10
            let sw = Stopwatch()
            let mutable iter = 0
            let mutable res = Unchecked.defaultof<obj>

            for i in 0..1000 do
                Sepp.Value(args) |> ignore


            System.GC.Collect()
            System.GC.WaitForFullGCComplete() |> ignore

            sw.Start()
            for i in 1..iterations do
                Sepp.Value(args) |> ignore
                iter <- iter + 1
            sw.Stop()
            let t = (1000000.0 * sw.Elapsed.TotalMilliseconds / float iter)
            printfn "stat: %.3fns" t
            t


        let log = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "callPerf.csv")
        File.WriteAllText(log, "nop;virtual;dispatcher;multimethods;static\r\n")
        for i in 1..50 do
            let tNop = nop()
            let tOurs = ours()
            let tVirt = virtualCall()
            let tStat = staticCall()
            let tDisp = dispatcher()
            let line = sprintf "%.3f;%.3f;%.3f;%.3f;%.3f" tNop tVirt tDisp tOurs tStat
            File.AppendAllLines(log, [line])
            System.GC.Collect()
            System.GC.WaitForFullGCComplete() |> ignore
