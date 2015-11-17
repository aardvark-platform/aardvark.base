namespace Aardvark.Base


module internal AgHelpers =
    open System.Runtime.CompilerServices
    open Microsoft.FSharp.Reflection
    open System.Reflection
    open System
    open System.Linq
    open System.Threading
    open System.Runtime.InteropServices
    open System.Collections.Generic   
    open System.Linq.Expressions


    type internal SemanticFunction(sem : obj, semType : Type, m : MethodInfo) =
        let argType = m.GetParameters().[0].ParameterType
        let param = Expression.Parameter(typeof<obj>)
        let expression = if m.ReturnType = typeof<System.Void> then
                            Expression.Lambda<Func<obj, obj>>(Expression.Block(Expression.Call(Expression.Constant(sem), m, [Expression.Convert(param, argType) :> Expression]), Expression.Constant(obj())), param) 
                         else
                            Expression.Lambda<Func<obj, obj>>(Expression.Convert(Expression.Call(Expression.Constant(sem), m, [Expression.Convert(param, argType) :> Expression]), typeof<obj>), param) 
        let compiled = expression.Compile()

        member x.ReturnType = m.ReturnType
        member x.Type = semType
        member x.ArgType = argType
        member x.Method = m
        member x.Fun o = compiled.Invoke(o)
        override x.ToString() = sprintf "semType: %A, member name: %s, argType: %s" semType m.Name argType.Name
 
    type internal ValueStore() =
        let dict = Dictionary<string * obj, obj>()

        member x.Set(o : obj, name : string, value : obj) =
            dict.[(name,o)] <- value
        member x.TryGetValue(o : obj, name : string, [<Out>] value : byref<obj>) =
            match dict.TryGetValue((name, o)) with
                | (true,v) -> value <- v
                              true
                | _        -> false

        member x.Clear() =
            dict.Clear()

    let private m_semanticObjects = System.Collections.Concurrent.ConcurrentDictionary<Type, obj>()
    let private m_semanticMap = System.Collections.Concurrent.ConcurrentDictionary<string, List<MethodBase>>()

    let sprintSemanticFunctions (methodName : string) =
        match m_semanticMap.TryGetValue methodName with
         | (true,v) -> v.ToArray() |> Array.fold (fun s e -> sprintf "%s method: %s on type: %s" s e.Name e.DeclaringType.Name) ""
         | _ -> sprintf "no sem objs for type: %s (m_semanticMap.Count=%d): %s" methodName m_semanticMap.Count (m_semanticMap |> Seq.fold (fun s (KeyValue(k,v)) -> sprintf "%s %s --> %A\n" s k v) "")
    
    (*=======================================================================================================================*)
    (*                                        Generic overload resolution helpers                                            *)
    (*=======================================================================================================================*)
    let private getAllBaseTypes(t : Type) =
        seq {
            let current = ref t
            while t = !current || (not (!current).IsPrimitive && not (!current).IsPointer && !current <> typeof<obj>) do
                yield !current
                current := (!current).BaseType
        }

    let private getAllInterfaces(t : Type) =
        t.GetInterfaces() |> Array.toSeq

    let rec private tryMakeApplicableDirect(argType : Type, concrete : Type) =
        if argType.IsGenericParameter then
            Some([(argType, concrete)])
        elif argType.IsAssignableFrom(concrete) then
            Some(List.empty)
        elif argType.IsGenericType && concrete.IsGenericType && concrete.GetGenericTypeDefinition() = argType.GetGenericTypeDefinition() then
            let argGen = argType.GetGenericArguments()
            let conGen = concrete.GetGenericArguments()
            
            let zipped = Array.zip argGen conGen
            let applied = zipped |> Array.map (fun (a,c) -> tryMakeApplicable(a,c))

            let isValid = applied |> Array.fold (fun s e -> s && Option.isSome(e)) true
            
            if isValid then
                let l = (applied |> Array.fold (fun m e -> List.concat [m; Option.get(e)]) [])
                Some(l)
            else
                None
        else
            None

    and private tryMakeApplicableBase(argType : Type, concrete : Type) =

        let direct = tryMakeApplicableDirect(argType, concrete)
        match direct with
            | Some(r) -> Some(r)
            | None    -> if not (isNull concrete.BaseType) && concrete.BaseType <> concrete then
                            tryMakeApplicable(argType, concrete.BaseType)
                         else
                            None

    and private tryMakeApplicableInterface(argType : Type, concrete : Type) =
        let interfaces = concrete.GetInterfaces()
        interfaces |> Array.tryPick (fun ti -> tryMakeApplicable(argType, ti))

    and private tryMakeApplicable(argType : Type, concrete : Type) =
        if argType.IsAssignableFrom(concrete) then
            Some([])
        else
            match tryMakeApplicableBase(argType, concrete) with
                | Some(m) -> Some(m)
                | None    -> match tryMakeApplicableInterface(argType, concrete) with
                                | Some(m) -> Some(m)
                                | None -> None

    let private trySpecialize(mi : MethodInfo, argType : Type) =
        let args = mi.GetParameters()
        if args.Length <> 1 then
            None
        else 
            let paramType = args.[0]
            match tryMakeApplicable(paramType.ParameterType, argType) with
                | Some(ass) -> let targs = mi.GetGenericArguments()
                               let map = Dictionary<Type, Type>()
                               for (a,t) in ass do map.Add(a, t)

                               let targs = targs |> Array.map (fun t -> 
                                    match map.TryGetValue t with
                                     | (true,v) -> v
                                     | _ -> failwithf "unbound generic type (might be due to generic return type. MethodInfo: %A)" mi) 
                               if mi.IsGenericMethod && targs.Length > 0 then
                                Some(mi.MakeGenericMethod(targs))
                               else 
                                Some(mi)
                | None -> None

    let private getSemanticObject(t : Type) =
        match m_semanticObjects.TryGetValue(t) with
            | (true,v) -> v
            | _ -> let v = Activator.CreateInstance(t)
                   m_semanticObjects.[t] <- v
                   v

    let private sfCache = Dictionary<Type, Dictionary<string, Option<SemanticFunction>>>()
    let private rootSfCache = Dictionary<Type * string, Option<SemanticFunction*Func<obj,obj>>>()

    let private addSemanticFunction (nodeType : Type, name : string) (sf : Option<SemanticFunction>) =
        lock sfCache (fun () ->
            match sfCache.TryGetValue(nodeType) with
                | (true, c) -> c.[name] <- sf
                               sf
                | _ -> let c = Dictionary<string, Option<SemanticFunction>>()
                       sfCache.[nodeType] <- c
                       c.[name] <- sf
                       sf
        )

    let private tryGetSemanticFunction (nodeType : Type, name : string) =
        lock sfCache (fun () ->
            match sfCache.TryGetValue nodeType with
                | (true, c) -> match c.TryGetValue name with
                                  | (true,v) -> Some v
                                  | _ -> None
                | _ -> None
        )

    let internal tryFindSemanticFunction(nodeType : Type, name : string) =
        
        match tryGetSemanticFunction(nodeType, name) with
            | Some sf -> sf
            | _ -> let reg = addSemanticFunction (nodeType, name)
                   match m_semanticMap.TryGetValue(name) with
                        | (true, v) -> let applicable = seq {
                                            for a in v do
                                                match trySpecialize(a :?> MethodInfo,nodeType) with
                                                    | Some(v) -> yield v :> MethodBase
                                                    | None -> ()
                                       } 
                                       let arr = applicable |> Seq.toArray
                                       if arr.Length = 0 then
                                           reg None
                                       else 
                                           let mi = Type.DefaultBinder.SelectMethod(
                                                        BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.InvokeMethod,
                                                        arr,
                                                        [|nodeType|],
                                                        [|ParameterModifier(1)|])

                                           if isNull mi then
                                                reg None
                                           else
                                                reg <| Some(SemanticFunction(getSemanticObject(mi.DeclaringType), mi.DeclaringType, mi :?> MethodInfo))
                        | _ -> reg None


    let rec getallInterfaces (t : Type) =
        [
            let directInterfaces = t.GetInterfaces()
            if not (isNull t.BaseType) then yield! getallInterfaces t.BaseType
            for i in directInterfaces do
                yield! getallInterfaces i
                yield i
        ]

    let internal tryGetSemanticFunctionDirect(nodeType : Type, name : string) =
        
        match m_semanticMap.TryGetValue(name) with
            | (true, v) -> 
                let applicable = 
                    seq {
                        for a in v do
                            match trySpecialize(a :?> MethodInfo,nodeType) with
                                | Some(v) -> yield v :> MethodBase
                                | None -> ()
                    } 
                let arr = applicable |> Seq.toArray
                if arr.Length = 0 then
                    None
                else 
                    let mi = Type.DefaultBinder.SelectMethod(
                                BindingFlags.Instance ||| BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.InvokeMethod,
                                arr,
                                [|nodeType|],
                                [|ParameterModifier(1)|])

                    if isNull mi then
                        None
                    else
                        Some(SemanticFunction(getSemanticObject(mi.DeclaringType), mi.DeclaringType, mi :?> MethodInfo))
            | _ -> None

    // finds the semantic function s for Root<S when S :> callerType) and returns s,S where S is
    // the interface of the syntactic family.
    // Note that, due to OOPness the lookup might be expensive if a type implements many interfaces
    // which are required for querying a syntax base type.
    // This is not too bad, since there should not be to many of them. Additionally the result is 
    // cached and occurs only when querying the root inh semantics the first time.
    let rec internal tryFindRootSemantics(genericRoot : Type, callerType : Type, name : string) : Option<SemanticFunction * Func<obj,obj>> =
           
        let wrap (v : SemanticFunction) =
            let concreteRootType = v.ArgType
            let ctor = concreteRootType.GetConstructor([|typeof<obj>|])
            let parameter = Expression.Parameter(typeof<obj>) 
            let creatorE = Expression.Lambda<Func<obj,obj>>(Expression.New(ctor,parameter),parameter)
            let creatorCompiled = creatorE.Compile()
            (v, creatorCompiled)

        if isNull callerType then None
        else
            let key = callerType,name
            lock rootSfCache (fun () ->
                match rootSfCache.TryGetValue key with
                 | (true,v) -> v
                 | _ ->
                    let concreteRootType = genericRoot.MakeGenericType([|callerType|])    
                    match tryGetSemanticFunctionDirect(concreteRootType, name) with
                     | Some v -> 
                         let result = Some (wrap v)
                         rootSfCache.[key] <- result
                         result
                     | None ->  
                        let res = 
                            if isNull callerType.BaseType |> not then
                                let concreteRootType = genericRoot.MakeGenericType([|callerType.BaseType|])  
                                match tryGetSemanticFunctionDirect(concreteRootType,name) with
                                    | Some v -> 
                                        let result = Some (wrap v)
                                        rootSfCache.[key] <- result
                                        result
                                    | None ->
                                        None
                            else None
                        match res with
                         | None ->
                            match callerType.GetInterfaces() |> Seq.tryPick (fun t -> tryFindRootSemantics(genericRoot, t, name)) with
                                | Some v -> 
                                    rootSfCache.[key] <- Some v
                                    Some v
                                | None when isNull callerType.BaseType |> not ->
                                    let result = tryFindRootSemantics(genericRoot, callerType.BaseType,name)
                                    rootSfCache.[key] <- result
                                    result     
                                | _ -> 
                                    rootSfCache.[key] <- None
                                    None
                         | Some v -> Some v
        )       


    let internal register (t : System.Type) = 
        let mi = t.GetMethods(System.Reflection.BindingFlags.Public ||| System.Reflection.BindingFlags.Instance)
        let attNames = seq {
                            for m in mi do
                                if (m.DeclaringType.Equals(t) && m.GetParameters().Length = 1) then
                                    yield ( m.Name, m.GetParameters().[0].ParameterType, m )
                        }
        if not (isNull (t.GetConstructor [||])) then
            let semObj = Activator.CreateInstance(t)
            for (name,t,m) in attNames do
                let list = match m_semanticMap.TryGetValue(name) with
                                | (true,l) -> l
                                | (false,_) -> let l = List<MethodBase>()
                                               m_semanticMap.[name] <- l
                                               l
                list.Add(m)

//    let internal registerAssembly (a : Assembly) =
//        try 
//            let types = a.GetTypes()
//
//            let rec attTypes (types : Type[]) =
//                seq {
//                    for t in types do
//                        if (t.GetCustomAttributes(false).OfType<Semantic>().Count() > 0) then
//                            yield t 
//                            yield! attTypes ( t.GetNestedTypes() )
//                }
//            let attTypes = attTypes types
//
//            attTypes |> Seq.toList |> Seq.iter register 
//        with
//            | _ -> ()

    let private registered = ref false

    let internal initializeAg<'semanticType when 'semanticType :> Attribute> ()  =
         if not !registered then
            //glInit()
            //Aardvark.Base.Report.BeginTimed "initializing attribute grammar"
            registered.Value <- true 

            for t in Introspection.GetAllTypesWithAttribute<'semanticType>() do
                register t.E0

            //AppDomain.CurrentDomain.AssemblyLoad.Add(
            //    fun loadArgs ->
            //        registerAssembly loadArgs.LoadedAssembly)

            //let assemblies = AppDomain.CurrentDomain.GetAssemblies() |> Seq.toList
            //Seq.iter registerAssembly assemblies
            //Aardvark.Base.Report.End() |> ignore

    let internal reIninitializeAg<'semanticType when 'semanticType :> Attribute>() =
        if !registered then
            m_semanticMap.Clear()
            m_semanticObjects.Clear()
            lock sfCache (fun () -> sfCache.Clear())
            registered := false

        initializeAg<'semanticType>()

    [<Literal>]
    let logging = false

    //choose the WeakTable implementation via the type-alias:
    type internal WeakTable<'a, 'b when 'a : not struct and 'b : not struct> = System.Runtime.CompilerServices.ConditionalWeakTable<'a, 'b>
    //type private WeakTable<'a, 'b when 'a : not struct and 'b : not struct> = Aardvark.Base.WeakTable.WeakTable<'a, 'b>

    let enableCacheWrites = true
    let tablePool = Queue<WeakTable<obj, obj>>()

    let newCWT() =
        if tablePool.Count > 0 then
            tablePool.Dequeue() |> unbox
        else
            WeakTable()

    let freeCWT(c : WeakTable<_,_>) =
        //c.Clear()
        //tablePool.Enqueue (c :> obj)
        ()


    module Delay =

        open System
        open System.Reflection
        open System.Collections.Generic
        open Microsoft.FSharp.Reflection

        type FSharpFuncConst<'a>(value) =
            inherit FSharpFunc<unit, 'a>()

            override x.Invoke(u : unit) =
                value

        let ctorCache = Dictionary<Type, ConstructorInfo>()

        let getCtor (fType : Type) =
            lock ctorCache (fun () ->
                match ctorCache.TryGetValue fType with
                    | (true, ctor) -> ctor
                    | _ ->
                        let (ta, tr) = FSharpType.GetFunctionElements fType
                        if ta <> typeof<unit> then 
                            failwithf "unexpected arg-type: %A" ta
                        let t = typedefof<FSharpFuncConst<_>>.MakeGenericType [|tr|]
                        let ctor = t.GetConstructor [|tr|]
                        ctorCache.[fType] <- ctor
                        ctor
            )


        let delay (value : obj) : 'a =
            let ctor = getCtor typeof<'a>
            ctor.Invoke [|value|] |> unbox<'a>