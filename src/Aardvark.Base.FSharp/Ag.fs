namespace Aardvark.Base

open System.Collections.Generic
open System.Runtime.CompilerServices
open Microsoft.FSharp.Reflection
open Aardvark.Base
open System.Threading
open AgHelpers


// Implementation of an embedded domain specific language for attribute grammars. The library works with any types,
// semantic functions can be added by defining modules annotated with [<Semantic>] attributes.
// More information can be found here: https://github.com/vrvis/attribute-grammars-for-incremental-scenegraph-rendering and
// the paper if accepted ;)
module Ag =

    type Semantic() =
        inherit System.Attribute()

    let mutable unpack : obj -> obj = id
    
    #if DEBUGSCOPES
    let allScopes = HashSet<obj>()
    #endif
    
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
                                   #if DEBUGSCOPES
                                   allScopes.Add (System.WeakReference<obj>(x :> obj)) |> ignore
                                   #endif
                                   x.children.Add(child, c)
                                   c
                    )


        member x.TryGetChildScope(child : obj) =
            lock x (fun () ->
                        match x.children.TryGetValue child with
                            | (true,c) -> Some c
                            | _ -> None
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

        #if DEBUGSCOPES 
        override x.Finalize() =
            if not <| allScopes.Remove (System.WeakReference<_>(x :> obj)) then printfn "scope not registered"
        #endif
        
        member x.Path = 
            match x.path with
                | Some p -> p
                | None -> let p = match x.parent with 
                                    | Some parent -> parent.Path
                                    | None -> "Root"
                          let p = sprintf "%s/%s(%d)" p (x.source.GetType().Name) (System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x.source))
                          x.path <- Some p
                          p


    
    type Root<'a>(child : obj) =
        member x.Child = child

    let rootType = typeof<Root<_>>.GetGenericTypeDefinition()

    //since some use-cases may need to deferr attribute-evaluation we provide a
    //mechanism to capture the state needed therefore. this is realized by simply
    //capturing the current scope. 
    type Captured = Scope

    let emptyScope = { parent = None; source = null; children = newCWT(); cache = null; path = Some "Root"}
    
    let mutable private rootScope = new ThreadLocal<Scope>(fun () -> { parent = None; source = null; children = newCWT(); cache = null; path = Some "Root" })

    let mutable currentScope = new ThreadLocal<Scope>(fun () -> rootScope.Value)

    //temporary storage for inherited values
    let internal anyObject = obj()
    let private valueStore = new ThreadLocal<Dictionary<obj * string, obj>>(fun () -> Dictionary<obj * string, obj>())
    let private setValueStore (node : obj) (name : string) (value : obj) = let node = unpack node
                                                                           valueStore.Value.[(node,name)] <- value
    let private getValueStore (node : obj) (name : string) = let node = unpack node
                                                             match valueStore.Value.TryGetValue((node, name)) with
                                                                | (true,v) -> Some v
                                                                | _ -> match valueStore.Value.TryGetValue((anyObject, name)) with
                                                                        | (true, v) -> Some v
                                                                        | _ -> 
                                                                            match rootScope.Value.TryGetChildScope(node) with
                                                                                | Some s -> match s.TryFindCacheValue name with
                                                                                                | Some v -> v
                                                                                                | _ -> None
                                                                                | None -> None
    let private clearValueStore() = valueStore.Value.Clear()
    
    type System.Object with
        member x.AllChildren = anyObject


    [<OnAardvarkInit>]
    let initialize () : unit =  AgHelpers.initializeAg<Semantic>()

    let reinitialize () : unit =  
        AgHelpers.reIninitializeAg<Semantic>()
        rootScope.Dispose()
        currentScope.Dispose()
        rootScope <- new ThreadLocal<Scope>(fun () -> { parent = None; source = null; children = newCWT(); cache = null; path = Some "Root" })
        currentScope <- new ThreadLocal<Scope>(fun () -> rootScope.Value)

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
    let inline useScope (s : Scope) (f : unit -> 'a) : 'a =
        let oldScope = currentScope.Value
        currentScope.Value <- s
        let r = f()
        currentScope.Value <- oldScope
        r

    let unscoped f = useScope rootScope.Value f

    //given a scope we can search for inherited attributes by navigating to parent scopes 
    //when needed.
    let rec private tryGetInhAttributeScopeInternal (node : obj) (cacheScopes : List<Scope>) (scope : Scope) (name : string) : Option<obj> =
        if logging then Log.start "tryGetInhAttributeScopeInternal %s on scope: %A " name scope.Path
        match scope.TryFindCacheValue name with
            //if the current scope contains a cache-value for <name> we may simply return it
            | Some(v) -> if logging then Log.stop ()
                         v

            //otherwise we need to search upwards for the attribute
            | _ -> let parent = scope.parent
                   match parent with
                        //if there is a valid parent-scope we continue the search
                        | Some(parent) when not (isNull parent.source) ->
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
                                           let r = tryGetInhAttributeScopeInternal parent.source cacheScopes parent name
                                           if logging then Log.stop ()
                                           r

                        //otherwise the attribute could not be found
                        | _ -> 
                            // find root semantic function by querying Root<S>, where S :> sourceType.
                            let sourceType = node.GetType()
                            match tryFindRootSemantics(rootType,sourceType, name) with
                                | Some(f,rootCreator) -> 
                                    if logging then Log.line "invoking sem: %A" f
                                    // TODO: further optimize this
                                    let rootInstance = rootCreator.Invoke scope.source 
                                    f.Fun(rootInstance) |> ignore
                                    match getValueStore scope.source name with
                                        | Some v -> clearValueStore()
                                                    if logging then Log.stop "no cache, searched attrib, found inh sem, adding to scope cache"
                                                    scope.AddCache name (Some v)
                                        | _ -> failwithf "invalid inherit method %A" f.Method
                                | None ->
                                    if logging then Log.stop "sem fun not found. adding none."
                                    scope.AddCache name None

    let inline private tryGetInhAttributeScope (node : obj) (scope : Scope) (name : string) : Option<obj> =
        if logging then Log.start "tryGetInhAttributeScope %s on scope: %A " name scope.Path
        let l = List<Scope>()
        let r = tryGetInhAttributeScopeInternal node l scope name
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
    let tryGetInhAttribute (node : obj) (name : string) =
        let inline up () =  
            match currentScope.Value.parent with
                | Some parent -> tryGetInhAttributeScope node (parent.GetChildScope node) name
                | None -> None
        match node with
            | :? Scope as scope -> tryGetInhAttributeScope node scope name
            | _ -> 
                match rootScope.Value.TryGetChildScope node with
                 | Some s -> 
                    match s.cache.TryGetValue name with
                        | (true, v) -> v
                        | _ ->
                            up ()
                 | None -> up ()

    let tryGetSynAttribute (o : obj) (name : string) =
        match o with
            | :? Scope as scope -> tryGetSynAttributeScope scope name
            | _ -> tryGetSynAttributeScope (currentScope.Value.GetChildScope o) name


    let tryGetAttributeType (attribute : string) (nodeType : System.Type) =
        match AgHelpers.tryFindSemanticFunction(nodeType, attribute) with
            | Some sf -> Some sf.ReturnType
            | None -> None

    //dynamic operators
    let (?<-) (node : obj) (name : string) (value : obj) : unit =
        if logging then Log.line "top level inh write for sem: %s on syntactic entity: %A" name (node.GetType())
        
        if currentScope.Value = rootScope.Value then
            let scope = currentScope.Value.GetChildScope(node)
            scope.AddCache name (Some value) |> ignore
        else
            //simply write to valueStore since the caller will then get the desired
            //value from there
            setValueStore node name value

   

    let (?) (node : obj) (name : string) : 'a =
        let t = typeof<'a>

        //when using an attribute like a function e.g. "a?Att()" we search for synthesized attributes
        //otherwise we search for inherited attributes.
        //WARNING: Inherited attributes can currently not be Functions (obviously)
        if t.Name.StartsWith "FSharpFunc" then
            if logging then Log.line "top level syn seach for sem: %s on syntactic entity: %A" name ( node.GetType() )
            match tryGetSynAttribute node name with
                | Some v -> Delay.delay v
                | None -> failwithf "synthesized attribute %A for type %A not found on path: %s \ncandidates: %s" name (node.GetType()) currentScope.Value.Path (AgHelpers.sprintSemanticFunctions name)
        else
            // in the case of an inherited attribute, check if the query is on the anyObject
            // e.g.: x.AllChildren?AttName this way an inherited attribute can be queried directly on the
            // applicator node (conversely to x.AllChildren?AttName <- att) in that case a temporary child scope 
            // is used before the inherited attribute is queried, which will at first pop that scope again, 
            // but uses the supplied scope for storage. since the attribute itself is given by the local object, 
            // it is also used as scope in order to allow the garbage collection to dispose the attribute together with the object
            // NOTE: does not work on <scope>.AllChildren?AttName, either use Ag.useScope or manually build the temporary child scope
            if System.Object.ReferenceEquals(node, anyObject) then
                let local = currentScope.Value
                let fakeChild = local.GetChildScope(local.source)
                match tryGetInhAttribute fakeChild name with
                    | Some v -> v |> unbox<'a>
                    | None -> failwithf "inherited attribute %A for type %A not found on path: %s\ncandidates: %s" name (node.GetType()) currentScope.Value.Path (AgHelpers.sprintSemanticFunctions name)
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
                    | None -> match tryGetInhAttributeScope null x name with
                                | Some v -> Success (v |> unbox<'a>)
                                | None -> Error "not found"
