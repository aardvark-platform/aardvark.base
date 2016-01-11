namespace Aardvark.Base

open System
open System.Reflection
open System.Collections.Generic
open System.Runtime.InteropServices
open System.Threading
open Microsoft.FSharp.Reflection


module NewAg =
    let private inhFunctions = Dictionary<string, Multimethod>()
    let private synFunctions = Dictionary<string, Multimethod>()
    let private rootFunctions = Dictionary<string, Multimethod>()

    type Scope =
        {
            parent : Scope
            sourceNode : obj
            inhValues : Dictionary<string, Option<obj>>
        }

    let private noScope = { parent = Unchecked.defaultof<_>; sourceNode = null; inhValues = null }
    let private currentScope = new ThreadLocal<ref<Scope>>(fun () -> ref noScope)
    let private tempStore = new ThreadLocal<Dictionary<obj * string, obj>>(fun () -> Dictionary.empty)

    let inline private opt (success : bool, value : 'a) =
        if success then Some value
        else None

    module private Delay =

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

    module private RootFunctions =
        open System.Reflection.Emit


        let contentType (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Ag.Root<_>> then
                t.GetGenericArguments().[0]
            else
                t

        let wrap (target : obj) (mi : MethodInfo) =
            let parameters = mi.GetParameters()
            let parameterTypes = parameters |> Array.map (fun p -> contentType p.ParameterType)
            let bType = RuntimeMethodBuilder.bMod.DefineType(Guid.NewGuid().ToString())

            let bTarget = 
                if mi.IsStatic then null
                else bType.DefineField("_target", mi.DeclaringType, FieldAttributes.Private)

            let bMeth = bType.DefineMethod("Invoke", MethodAttributes.Public, mi.ReturnType, parameterTypes)
            let il = bMeth.GetILGenerator()

            il.Emit(OpCodes.Nop)
            if not mi.IsStatic then
                il.Emit(OpCodes.Ldarg_0)
                il.Emit(OpCodes.Ldfld, bTarget)
            

            for i in 0..parameters.Length - 1 do
                let p = parameters.[i]
                il.Emit(OpCodes.Ldarg, i + 1)
                il.Emit(OpCodes.Unbox_Any, typeof<obj>)
                il.Emit(OpCodes.Newobj, typedefof<Ag.Root<_>>.MakeGenericType(parameterTypes.[i]).GetConstructor [|typeof<obj>|])



            il.EmitCall(OpCodes.Call, mi, null)
            il.Emit(OpCodes.Ret)


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
                ctor.Invoke [||], t.GetMethod "Invoke"
            else
                let ctor = t.GetConstructor [|mi.DeclaringType|]
                ctor.Invoke [|target|], t.GetMethod "Invoke"


    let init() =
        let types = Introspection.GetAllTypesWithAttribute<Ag.Semantic>()

        for t in types do
            let methods = t.E0.GetMethods(BindingFlags.Public ||| BindingFlags.Instance)

            for m in methods do
                let parameters = m.GetParameters()
                if parameters.Length = 1 then
                    let valid = 
                        if m.IsGenericMethod then
                            let free = HashSet(m.GetGenericArguments())
                            let bound = HashSet(m.GetParameters() |> Seq.collect (fun p -> p.ParameterType.GetGenericArguments()))

                            if free.SetEquals bound then
                                true
                            else
                                false
                        else
                            true
                    if not valid then
                        Log.warn "found generic semantic function %A whose parameters are not bound by the arguments" m
                    else 
                        let name = m.Name
                        let parameterType = parameters.[0].ParameterType

                        if parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() = typedefof<Ag.Root<_>> then
                            let mm = 
                                match rootFunctions.TryGetValue name with
                                    | (true, mm) -> mm
                                    | _ ->
                                        let mm = Multimethod(1)
                                        rootFunctions.[name] <- mm
                                        mm
                            let (t,m) = RootFunctions.wrap (Multimethod.GetTarget m) m
                            mm.Add(t, m)


                        elif m.ReturnType = typeof<unit> || m.ReturnType = typeof<System.Void> then
                            match inhFunctions.TryGetValue name with
                                | (true, mm) -> 
                                    mm.Add m
                                | _ -> 
                                    let mm = Multimethod [m]
                                    inhFunctions.[name] <- mm

                        else
                            match synFunctions.TryGetValue name with
                                | (true, mm) -> 
                                    mm.Add m
                                | _ -> 
                                    let mm = Multimethod [m]
                                    synFunctions.[name] <- mm



    let getCurrentScope() : Scope =
        !currentScope.Value

    let useScope (s : Scope) (f : unit -> 'a) =
        let r = currentScope.Value
        let old = !r
        r := s
        try f()
        finally r := old


    let rec private tryInheritInternal (mm : Multimethod) (scopeRef : ref<Scope>) (name : string) =
        let scope = getCurrentScope()

        match scope.inhValues.TryGetValue name with
            | (true, v) -> v
            | _ ->
                if isNull scope.parent.sourceNode then
                    match rootFunctions.TryGetValue(name) with
                        | (true, mm) ->
                            match mm.TryInvoke([|scope.sourceNode|]) with
                                | (true, _) ->
                                    let ts = tempStore.Value
                                    let res =
                                        match ts.TryGetValue((scope.sourceNode, name)) with
                                            | (true, v) -> Some v
                                            | _ -> 
                                                failwithf "[Ag] invalid root method for attribute %s which does not write to %A" name scope.sourceNode
                            
                                    ts.Clear()
                                    res
                                | _ ->
                                    None
                        | _ ->
                            None
                else
                    scopeRef := scope.parent
                    let res =
                        match mm.TryInvoke([|scope.parent.sourceNode|]) with
                            | (true, _) -> 
                                let ts = tempStore.Value
                                let res =
                                    match ts.TryGetValue((scope.sourceNode, name)) with
                                        | (true, v) -> Some v
                                        | _ ->
                                            match ts.TryGetValue((Ag.anyObject, name)) with
                                                | (true, v) -> Some v
                                                | _ ->  
                                                    failwithf "[Ag] invalid inherit method for attribute %s which does not write to %A" name scope.sourceNode
                            
                                ts.Clear()
                                res

                            | _ -> tryInheritInternal mm scopeRef name
                            

                    scope.inhValues.[name] <- res
                    res
     
    let tryInherit (name : string) (o : obj) =
        let ref = currentScope.Value
        if isNull ref.Value.sourceNode then
            None
        else
            if not (Unchecked.equals ref.Value.sourceNode o) then
                failwithf "inheriting %s for %A but current scope induces %A" name o ref.Value.sourceNode

            match inhFunctions.TryGetValue name with
                | (true, mm) ->
                    tryInheritInternal mm ref name
                | _ ->
                    None

    let trySynthesize (name : string) (o : obj) =
        match synFunctions.TryGetValue(name) with
            | (true, f) ->
                useScope { parent = getCurrentScope(); sourceNode = o; inhValues = Dictionary.empty } (fun () ->
                    match f.TryInvoke([|o|]) with
                        | (true, v) -> Some v
                        | _ -> None
                )
            | _ -> None
          
    let tryGetAttributeValue (name : string) (o : obj) =
        match trySynthesize name o with
            | Some v -> Some v
            | None -> tryInherit name o

    let (?<-) (n : obj) (name : string) (value : 'a) =
        tempStore.Value.[(n, name)] <- value
        
    let (?) (n : obj) (name : string) : 'a =
        let t = typeof<'a>
        if t.Name.StartsWith "FSharpFunc" then
            match trySynthesize name n with
                | Some v -> 
                    Delay.delay v
                | None -> 
                    failwithf "[Ag] unable to find synthesized attribute %s for node type: %A" name (n.GetType())
        else
            match n with
                | :? Scope as s ->
                    match useScope s (fun () -> tryInherit name s.sourceNode) with
                        | Some (:? 'a as v) -> v
                        | Some v ->
                            failwithf "[Ag] unexpected type for inherited attribute %s on node type %A: %A" name (n.GetType()) (v.GetType())
                        | _ -> 
                            failwithf "[Ag] unable to find inherited attribute %s on node type: %A" name (s.sourceNode.GetType())
                | _ ->
                    match tryInherit name n with
                        | Some (:? 'a as v) -> v
                        | Some v ->
                            failwithf "[Ag] unexpected type for inherited attribute %s on node type %A: %A" name (n.GetType()) (v.GetType())
                        | None -> 
                            failwithf "[Ag] unable to find inherited attribute %s on node type: %A" name (n.GetType())
                    

module NewAgTest =
    open Ag
    open NewAg

    type INode = interface end

    type Env(a : INode) =
        interface INode
        member x.C = a

    type Leaf(a : int) =
        let mutable a = a
        interface INode
        member x.A
            with get() = a
            and set v = a <- v
        
    type Node(c : INode) =
        interface INode
        member x.C = c 

    [<Ag.Semantic>]
    type Sem() =
        member x.Inh(r : Ag.Root<INode>) =
            r.Child?Inh <- 0
            
        member x.Inh(n : Node) =
            n.AllChildren?Inh <- 1 + n?Inh

        member x.Value(a : Env) : int =
            a.C?Value()

        member x.Value(a : Leaf) =
            a.A + a?Inh

        member x.Value(n : Node) : int =
            n.C?Value()
        


    let run() =
        NewAg.init()
        let l = Leaf 1
        let t = Node(Node(l))
        let v : int = t?Value()
        printfn "%A" v


        l.A <- 5
        let v : int = t?Value()
        printfn "%A" v

        NewAg.tryGetAttributeValue "Sepp" v |> printfn "Sepp = %A"



