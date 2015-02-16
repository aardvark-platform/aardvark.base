#if INTERACTIVE
#r "Microsoft.CSharp.dll"
#r "..\..\Bin\Release\Aardvark.Base.dll"
#r "..\..\Bin\Release\Aardvark.VRVis.Base.dll"
#r "..\..\Bin\Debug\Aardvark.Base.FSharp.dll"

open Aardvark.Base
#else
namespace Aardvark.Base
#endif

[<AutoOpen>]
module AgHelpers =
    open System.Runtime.CompilerServices
    open Microsoft.FSharp.Reflection
    open System.Reflection
    open System
    open System.Linq
    open System.Threading
    open System.Runtime.InteropServices
    open System.Collections.Generic   
    open System.Linq.Expressions

    type Semantic() =
        inherit System.Attribute()

    type internal SemanticFunction(sem : obj, semType : Type, m : MethodInfo) =
        let argType = m.GetParameters().[0].ParameterType
        let param = Expression.Parameter(typeof<obj>)
        let expression = if m.ReturnType = typeof<System.Void> then
                            Expression.Lambda<Func<obj, obj>>(Expression.Block(Expression.Call(Expression.Constant(sem), m, [Expression.Convert(param, argType) :> Expression]), Expression.Constant(obj())), param) 
                         else
                            Expression.Lambda<Func<obj, obj>>(Expression.Convert(Expression.Call(Expression.Constant(sem), m, [Expression.Convert(param, argType) :> Expression]), typeof<obj>), param) 
        let compiled = expression.Compile()

        member x.Type = semType
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
    
    let rec private tryUnifyTypes(argType : Type, concrete : Type, [<Out>] distance : ref<int>) =
        if concrete = null then
            distance := -1
            None
        elif argType.IsGenericParameter || argType = concrete then
            distance := 0
            Some(concrete)
        elif concrete.IsGenericType && argType.IsGenericType && concrete.GetGenericTypeDefinition() = argType.GetGenericTypeDefinition() then
            distance := 0
            Some(concrete)
        else
            if concrete.IsPrimitive || concrete.IsPointer || concrete.BaseType = null || concrete.BaseType = concrete then
                distance := -1
                None
            else 
                let cd = ref 0
                match tryUnifyTypes(argType, concrete.BaseType, cd) with
                    | Some(r) -> distance := !cd + 1
                                 Some(r)
                    | None    -> match concrete.GetInterfaces() |> Array.tryPick(fun ti -> tryUnifyTypes(argType, ti, cd)) with
                                    | Some(r) -> distance := !cd + 1
                                                 Some(r)
                                    | None -> if argType.IsAssignableFrom(concrete) then
                                                distance := 100
                                                Some(concrete)
                                                else
                                                distance := -1
                                                None

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
            | None    -> if concrete.BaseType <> null && concrete.BaseType <> concrete then
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
                               for (a,t) in ass do
                                map.Add(a, t)

                               if mi.ReturnType.IsGenericParameter then failwithf "return types of semantic functions must be non-generic. annotate the semfun: %A" mi

                               let targs = targs |> Array.map (fun t -> map.[t]) // if there is key not found maybe fixup generics
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

    let private addSemanticFunction (nodeType : Type, name : string) (sf : Option<SemanticFunction>) =
        match sfCache.TryGetValue(nodeType) with
            | (true, c) -> c.[name] <- sf
                           sf
            | _ -> let c = Dictionary<string, Option<SemanticFunction>>()
                   sfCache.[nodeType] <- c
                   c.[name] <- sf
                   sf

    let private tryGetSemanticFunction (nodeType : Type, name : string) =
        match sfCache.TryGetValue nodeType with
            | (true, c) -> match c.TryGetValue name with
                              | (true,v) -> Some v
                              | _ -> None
            | _ -> None

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

                                           if mi = null then
                                                reg None
                                           else
                                                reg <| Some(SemanticFunction(getSemanticObject(mi.DeclaringType), mi.DeclaringType, mi :?> MethodInfo))
                        | _ -> reg None

    let internal glInit() =
        try
            let ass = Assembly.LoadFile (System.IO.Path.Combine(System.Environment.CurrentDirectory, "OpenTK.dll"))
            let t = ass.GetType("OpenTK.GameWindow")

            let w = System.Activator.CreateInstance(t) |> unbox<System.IDisposable>
            let m = w.GetType().GetMethod("Close")
            m.Invoke(w, null) |> ignore
            w.Dispose()

        with e ->
            ()

    let internal register (t : System.Type) = 
        let mi = t.GetMethods(System.Reflection.BindingFlags.Public ||| System.Reflection.BindingFlags.Instance)
        let attNames = seq {
                            for m in mi do
                                if (m.DeclaringType.Equals(t) && m.GetParameters().Length = 1) then
                                    yield ( m.Name, m.GetParameters().[0].ParameterType, m )
                        }
        if t.GetConstructor([||]) <> null then
            let semObj = Activator.CreateInstance(t)
            for (name,t,m) in attNames do
                let list = match m_semanticMap.TryGetValue(name) with
                                | (true,l) -> l
                                | (false,_) -> let l = List<MethodBase>()
                                               m_semanticMap.[name] <- l
                                               l
                list.Add(m)

    let internal registerAssembly (a : Assembly) =
        try 
            let types = a.GetTypes()

            let rec attTypes (types : Type[]) =
                seq {
                    for t in types do
                        if (t.GetCustomAttributes(false).OfType<Semantic>().Count() > 0) then
                            yield t 
                            yield! attTypes ( t.GetNestedTypes() )
                }
            let attTypes = attTypes types

            attTypes |> Seq.toList |> Seq.iter register 
        with
            | _ -> ()

    let internal initializeAg =
        let registered = ref false
        fun () ->
         if not !registered then
            //glInit()
            registered.Value <- true 

            AppDomain.CurrentDomain.AssemblyLoad.Add(
                fun loadArgs ->
                    registerAssembly loadArgs.LoadedAssembly)

            let assemblies = AppDomain.CurrentDomain.GetAssemblies() |> Seq.toList
            Seq.iter registerAssembly assemblies

    let mutable public unpack : obj -> obj = id

    [<Literal>]
    let logging = false

module Ag =

    //choose the WeakTable implementation via the type-alias:
    type private WeakTable<'a, 'b when 'a : not struct and 'b : not struct> = System.Runtime.CompilerServices.ConditionalWeakTable<'a, 'b>
    //type private WeakTable<'a, 'b when 'a : not struct and 'b : not struct> = Aardvark.Base.WeakTable.WeakTable<'a, 'b>

    open System.Collections.Generic
    open System.Runtime.CompilerServices
    open Microsoft.FSharp.Reflection
    open Aardvark.Base
    open System.Threading

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


    [<ReferenceEquality>]
    type Scope = { parent       : Option<Scope>
                   source       : obj
                   children     : WeakTable<obj, Scope>
                   cache        : Dictionary<string, Option<obj>>
                   mutable path         : Option<string> } with

        member x.GetChildScope(child : obj) =
            lock x (fun () ->
                        match x.children.TryGetValue child with
                            | (true,c) -> c
                            | _ -> let c = { parent = Some(x); source = child; children = newCWT(); cache = Dictionary(); path = None}
                                   x.children.Add(child, c)
                                   c
                    )

        member x.TryFindCacheValue (name : string) =
            if enableCacheWrites then
                match x.cache.TryGetValue name with
                    | (true,v) -> Some v
                    | _ -> None
            else None

        member x.AddCache (name : string) (value : Option<obj>) =
            if enableCacheWrites then x.cache.[name] <- value
            value

        override x.Finalize() =
            freeCWT x.children
            ()
        
        member x.Path = 
            match x.path with
                | Some p -> p
                | None -> let p = match x.parent with 
                                    | Some parent -> parent.Path
                                    | None -> "Root"
                          let p = sprintf "%s/%s(%d)" p (x.source.GetType().Name) (System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x.source))
                          x.path <- Some p
                          p


    


    type Root = { Child : obj }

    //since some use-cases may need to deferr attribute-evaluation we provide a
    //mechanism to capture the state needed therefore. this is realized by simply
    //capturing the current scope. 
    type Captured = Scope

    let emptyScope = { parent = None; source = null; children = newCWT(); cache = null; path = Some "Root"}
    
    let mutable private rootScope = new ThreadLocal<Scope>(fun () -> { parent = None; source = null; children = newCWT(); cache = null; path = Some "Root" })

    let mutable private currentScope = new ThreadLocal<Scope>(fun () -> rootScope.Value)

    //temporary storage for inherited values
    let private anyObject = obj()
    let private valueStore = new ThreadLocal<Dictionary<obj * string, obj>>(fun () -> Dictionary<obj * string, obj>())
    let private setValueStore (node : obj) (name : string) (value : obj) = let node = unpack node
                                                                           valueStore.Value.[(node,name)] <- value
    let private getValueStore (node : obj) (name : string) = let node = unpack node
                                                             match valueStore.Value.TryGetValue((node, name)) with
                                                                | (true,v) -> Some v
                                                                | _ -> match valueStore.Value.TryGetValue((anyObject, name)) with
                                                                        | (true, v) -> Some v
                                                                        | _ -> None
    let private clearValueStore() = valueStore.Value.Clear()
    
    type System.Object with
        member x.AllChildren = anyObject


    [<OnAardvarkInit>]
    let initialize () : unit =  AgHelpers.initializeAg()

    let clearCaches () =    
        rootScope.Dispose()
        currentScope.Dispose()
        rootScope <- new ThreadLocal<Scope>(fun () -> { parent = None; source = null; children = newCWT(); cache = null; path = Some "Root" })
        currentScope <- new ThreadLocal<Scope>(fun () -> rootScope.Value)
        System.GC.Collect()
        System.GC.WaitForPendingFinalizers()
        System.GC.WaitForFullGCApproach() |> ignore
        

    // this is for profiling only - old benchmarks use getAttributeValueCount to estimate cache efficiencies etc.
    let mutable getAttributeValueCount = 0

    //attribute-search functions
    //since we don't have control over semantic functions all attribute-evaluations
    //have to communicate through global variables (which are ThreadLocal in order
    //to avoid concurrency-problems). Using this technique we maintain a "call-stack"
    //representing the current trace in the evaluation-tree. (Scope)

    //whenever we call a semantic-function the currentScope needs to be set
    //correctly since the ?-operators used inside the function need to know
    //their current scope. therefore useScope executes a function in the given 
    //scope and restores everything afterwards.
    let useScope (s : Scope) (f : unit -> 'a) : 'a =
        let oldScope = currentScope.Value
        currentScope.Value <- s
        let r = f()
        currentScope.Value <- oldScope
        r

    let unscoped f = useScope rootScope.Value f

    //given a scope we can search for inherited attributes by navigating to parent scopes 
    //when needed.
    let rec private tryGetInhAttributeScopeInternal (cacheScopes : List<Scope>) (scope : Scope) (name : string) : Option<obj> =
        if logging then Log.start "tryGetInhAttributeScopeInternal %s on scope: %A " name scope.Path
        match scope.TryFindCacheValue name with
            //if the current scope contains a cache-value for <name> we may simply return it
            | Some(v) -> if logging then Log.stop ()
                         v

            //otherwise we need to search upwards for the attribute
            | _ -> let parent = scope.parent
                   match parent with
                        //if there is a valid parent-scope we continue the search
                        | Some(parent) when parent.source <> null ->
                                let parentType = parent.source.GetType()
                                match tryFindSemanticFunction(parentType, name) with
                                    //if there is a semantic-function applicable to the parent-node
                                    //we run it "hoping" that it will write the desired value to valueStore
                                    | Some(f) -> useScope parent (fun () -> if logging then Log.start "invoking inh method for sem %s for type %A. sf=%A" name parentType f
                                                                            let r = f.Fun parent.source |> ignore
                                                                            if logging then Log.stop ())
                                                
                                                 //if the semantic-function did not write the value to valueStore
                                                 //it's considered invalid
                                                 match getValueStore scope.source name with
                                                        | Some v -> clearValueStore()
                                                                    if logging then Log.stop "no cache, direct match in parent, found inh sem, adding to scope cache"
                                                                    scope.AddCache name (Some v)
                                                        | _ -> if logging then Log.stop (); 
                                                               let domeagain =  getValueStore scope.source name
                                                               failwithf "invalid inherit method %A" f.Method

                                    //if there is no such function we continue to search up
                                    //the tree. (auto-inherit)
                                    | _ -> cacheScopes.Add(scope)
                                           if logging then Log.line "autoinh."
                                           let r = tryGetInhAttributeScopeInternal cacheScopes parent name
                                           if logging then Log.stop ()
                                           r

                        //otherwise the attribute could not be found
                        | _ -> 
                            match tryFindSemanticFunction(typeof<Root>, name) with
                                | Some(f) -> 
                                    if logging then Log.line "invoking sem: %A" f
                                    f.Fun({ Child = scope.source }) |> ignore
                                    match getValueStore scope.source name with
                                        | Some v -> clearValueStore()
                                                    if logging then Log.stop "no cache, searched attrib, found inh sem, adding to scope cache"
                                                    scope.AddCache name (Some v)
                                        | _ -> failwithf "invalid inherit method %A" f.Method
                                | None ->
                                    if logging then Log.stop "sem fun not found. adding none."
                                    scope.AddCache name None

    let inline private tryGetInhAttributeScope (scope : Scope) (name : string) : Option<obj> =
        if logging then Log.start "tryGetInhAttributeScope %s on scope: %A " name scope.Path
        let l = List<Scope>()
        let r = tryGetInhAttributeScopeInternal l scope name
        l |> Seq.iter (fun si -> si.AddCache name r |> ignore)
        if logging then if r.IsNone then Log.stop " --> result: none" else Log.stop " --> result: some"
        r 

    //synthesized attributes can directly be found for any node. but since they can
    //depend on inherited attributes (most likely) they also need their scope.
    let private tryGetSynAttributeScope (scope : Scope) (name : string) =
        if logging then Log.start "tryGetSynAttributeScope %s on scope: %A" name scope.Path
        match scope.TryFindCacheValue name with
            //if the current scope contains a cache-value for <name> we may simply return it
            | Some v -> if logging then Log.stop "cache found."
                        v

            //otherwise there needs to be a semantic-function creating the attribute value
            | _ -> let sourceNode = scope.source
                   let sourceType = sourceNode.GetType()
                   match tryFindSemanticFunction(sourceType, name) with
                        //if there is a semantic-function we run it and add a cache entry
                        | Some(sf) -> let result = useScope scope (fun () -> if logging then Log.start "invoking sf: %A" sf
                                                                             let r = sf.Fun sourceNode
                                                                             if logging then Log.stop ()
                                                                             r)
                                      if logging then Log.stop ()
                                      scope.AddCache name (Some result)

                        //otherwise the synthesized attribute cannot be calculated
                        | None -> if logging then Log.stop "not found."
                                  None


    //when not given a scope we need to use the currentScope
    //these functions are called by the ?-operator
    let inline private tryGetInhAttribute (o : obj) (name : string) =
        match o with
            | :? Scope as scope -> tryGetInhAttributeScope scope name
            | _ -> match currentScope.Value.parent with
                        | Some parent -> tryGetInhAttributeScope (parent.GetChildScope o) name
                        | None -> None

    let private tryGetSynAttribute (o : obj) (name : string) =
        match o with
            | :? Scope as scope -> tryGetSynAttributeScope scope name
            | _ -> tryGetSynAttributeScope (currentScope.Value.GetChildScope o) name


    //dynamic operators
    let (?<-) (node : obj) (name : string) (value : obj) : unit =
        if logging then Log.line "top level inh write for sem: %s on syntactic entity: %A" name (node.GetType())
        //simply write to valueStore since the caller will then get the desired
        //value from there
        setValueStore node name value

    let private functions = Dictionary<System.Type, ref<obj> * obj>()
    
    let getFunction (value : obj) : 'a =
        match functions.TryGetValue typeof<'a> with
            | (true, (r,f)) -> r := value
                               f |> unbox
            | _ -> let r = ref value
                   let f = FSharpValue.MakeFunction(typeof<'a>, fun _ -> let res = !r in r := null; res)
                   functions.[typeof<'a>] <- (r, f)
                   f |> unbox


    let (?) (node : obj) (name : string) : 'a =
        let t = typeof<'a>

        //when using an attribute like a function e.g. "a?Att()" we search for synthesized attributes
        //otherwise we search for inherited attributes.
        //WARNING: Inherited attributes can currently not be Functions (obviously)
        if t.Name.StartsWith "FSharpFunc" then
            if logging then Log.line "top level syn seach for sem: %s on syntactic entity: %A" name ( node.GetType() )
            match tryGetSynAttribute node name with
                | Some v -> getFunction v
                | None -> failwithf "synthesized attribute %A for type %A not found on path: %s \ncandidates: %s" name (node.GetType()) currentScope.Value.Path (AgHelpers.sprintSemanticFunctions name)
        else
            if logging then Log.line "top level inh seach for sem: %s on syntactic entity: %A" name ( node.GetType() )
            match tryGetInhAttribute node name with
                | Some v -> v |> unbox<'a>
                | None -> failwithf "inherited attribute %A for type %A not found on path: %s\ncandidates: %s" name (node.GetType()) currentScope.Value.Path (AgHelpers.sprintSemanticFunctions name)


    //public entry points            
    let tryGetAttributeValue(o : obj) (name:string) : Error<'a> =
        match tryGetSynAttribute o name with
            | Some v -> Success (v |> unbox<'a>)
            | None -> match tryGetInhAttribute o name with
                        | Some v -> Success (v |> unbox<'a>)
                        | _ -> sprintf "attribute %A not found" name |> Error

    //the mod-system needs to be able to capture a Ag-State/Context for re-execution
    let getContext() = currentScope.Value
    let setContext(v) = currentScope.Value <- v

    [<AutoOpen>]
    module CapturedExtensions =
        type Scope with
            member x.TryGetAttributeValue(name : string) : Error<'a> =
                match tryGetSynAttributeScope x name with
                    | Some v -> Success (v |> unbox<'a>)
                    | None -> match tryGetInhAttributeScope x name with
                                | Some v -> Success (v |> unbox<'a>)
                                | None -> Error "not found"
