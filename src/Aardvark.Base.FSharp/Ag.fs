namespace Aardvark.Base

open System
open System.Runtime.CompilerServices
open System.Collections.Generic
open Aardvark.Base
open System.Reflection
open System.Runtime.InteropServices

#nowarn "1337"

[<AttributeUsage(AttributeTargets.Struct ||| AttributeTargets.Class ||| AttributeTargets.Method)>]
type RuleAttribute() =
    inherit Attribute()

module Ag =

    let internal anyObj = obj()

    let private globalValues = ConditionalWeakTable<obj, Dictionary<string, obj>>()

    [<AutoOpen>]
    module private TypeHelpers =
        open Microsoft.FSharp.Reflection
    
        let private genRx = System.Text.RegularExpressions.Regex @"^([^`]*)`[0-9]+$"

        let withBrackets (str : string) =
            if str.Contains " " && not (str.StartsWith "(") then "(" + str + ")"
            else str

        let rec prettyName (t : Type) : string =
            if t.IsByRef then
                let t = prettyName (t.GetElementType())
                sprintf "byref<%s>" t

            elif t.IsArray then
                let d = t.GetArrayRank()
                let t = prettyName (t.GetElementType())
                if d = 1 then sprintf "array<%s>" t
                else sprintf "array%dd<%s>" d t

            elif FSharpType.IsTuple(t) then
                let elems = FSharpType.GetTupleElements t |> Seq.map (prettyName >> withBrackets) |> String.concat " * "
                if t.IsValueType then sprintf "struct(%s)" elems
                else sprintf "%s" elems

            elif FSharpType.IsFunction(t) then
                let a, b = FSharpType.GetFunctionElements t
                sprintf "%s -> %s" (prettyName a) (prettyName b)

            elif t.IsGenericType then
                let def = t.GetGenericTypeDefinition().Name
                let m = genRx.Match def
                let clean = 
                    if m.Success then m.Groups.[1].Value
                    else def

                sprintf "%s<%s>" clean (t.GetGenericArguments() |> Seq.map prettyName |> String.concat ", ")

            else
                t.Name     

    type Scope private(parent : option<Scope>, node : obj, childScopes : ConditionalWeakTable<obj, Scope>) =  
        let inherited = Dictionary<string, option<obj>>()
        let anyChild = Dictionary<string, option<obj>>()

        let name = 
            lazy (
                match parent with
                | None -> "Root"
                | Some p ->
                    let self = sprintf "%s[H%X]" (prettyName(node.GetType())) (node.GetHashCode())
                    p.Name + "/" + self
            )

        [<ThreadStatic; DefaultValue>]
        static val mutable private CurrentScope_ : option<Scope>

        static let root =
            Scope(None, null, ConditionalWeakTable<obj, Scope>())

        static member internal CurrentScope
            with get() = Scope.CurrentScope_
            and set v = Scope.CurrentScope_ <- v

        member x.Node = node
        member x.Parent = parent

        member private x.TryGetAnyChildValue(name : string) =
            lock inherited (fun () ->
                match anyChild.TryGetValue(name) with
                | (true, v) -> Some v
                | _ -> None
            )

        static member internal Pseudo(node : obj, childScope : Scope) =
            let cwt = ConditionalWeakTable<obj, Scope>()
            cwt.Add(childScope.Node, childScope)
            Scope(None, node, cwt)

        member internal x.SetInherited(name : string, value : option<obj>) =
            lock inherited (fun () ->
                inherited.[name] <- value
            )

        member internal x.SetInheritedForChild(child : obj, name : string, value : option<obj>) =
            lock inherited (fun () ->
                if child = anyObj then
                    anyChild.[name] <- value
                else
                    let c = x.GetChildScope(child)
                    c.SetInherited(name, value)
            )

        member private x.TryGetGlobalValue(name : string) =
            match lock globalValues (fun () -> globalValues.TryGetValue(node)) with
            | (true, d) ->
                match lock d (fun () -> d.TryGetValue(name)) with
                | (true, v) -> 
                    Some v
                | _ -> 
                    None
            | _ ->
                None


        member internal x.TryGetInheritedCache(name : string) : option<option<obj>> =
            lock inherited (fun () ->
                match x.TryGetGlobalValue(name) with
                | Some v -> Some (Some v)
                | None -> 
                    match inherited.TryGetValue name with
                    | (true, v) -> Some v
                    | _ ->
                        match parent with
                        | Some p ->
                            match p.TryGetAnyChildValue(name) with
                            | Some v ->
                                inherited.[name] <- v
                                Some v
                            | None ->
                                None
                        | None ->
                            None
            )


        member x.Name : string =
            name.Value

        override x.ToString() =
            name.Value

        /// the root scope
        static member Root = root

        /// get a (possibly cached) child scope for the given node
        member x.GetChildScope<'a when 'a : not struct>(node : 'a) : Scope =
            lock childScopes (fun () ->
                if typeof<'a>.IsValueType then
                    Scope(Some x, node :> obj, ConditionalWeakTable<obj, Scope>())
                else
                    match childScopes.TryGetValue(node :> obj) with
                    | (true, s) -> s
                    | _ ->
                        let s = Scope(Some x, node :> obj, ConditionalWeakTable<obj, Scope>())
                        childScopes.Add(node :> obj, s)
                        s
            )
            
        /// attach a global inherited value to a node (overrides any other inheritance mechanisms).
        static member SetGlobalValue<'a when 'a : not struct>(node : 'a, name : string, value : obj) : unit =
            let dict =
                lock globalValues (fun () ->
                    match globalValues.TryGetValue node with
                    | (true, d) -> d
                    | _ -> 
                        let d = Dictionary()
                        globalValues.Add(node, d)
                        d
                )
            lock dict (fun () ->
                dict.[name] <- value
            )

    type Root<'a>(child : 'a) =
        member x.Child = child

    [<AutoOpen>]
    module private Helpers =
        open System.Reflection.Emit
        open Aardvark.Base.TypeInfo

        type NewRootDelegate = delegate of obj -> obj
        type SynDelegateStatic = delegate of obj * Scope -> obj
        type SynDelegateInstance = delegate of obj * obj * Scope -> obj
        type InhDelegateStatic = delegate of obj * Scope -> unit
        type InhDelegateInstance = delegate of obj * obj * Scope -> unit

        
        [<StructuredFormatDisplay("{AsString}")>]
        type InheritMethod(mi : MethodInfo, invoke : obj -> Scope -> unit) =
            member x.MethodInfo = mi
            member x.Invoke = invoke

            member private x.AsString = x.ToString()
            override x.ToString() =
                let decl = prettyName mi.DeclaringType
                let nodeType = prettyName (mi.GetParameters().[0].ParameterType)
                sprintf "%s.%s(node : %s, _)" decl mi.Name nodeType
                
        [<StructuredFormatDisplay("{AsString}")>]
        type SynMethod(mi : MethodInfo, invoke : obj -> Scope -> obj) =
            member x.MethodInfo = mi
            member x.Invoke = invoke
            
            member private x.AsString = 
                x.ToString()

            override x.ToString() =
                let decl = prettyName mi.DeclaringType
                let ret = prettyName mi.ReturnType
                let nodeType = prettyName (mi.GetParameters().[0].ParameterType)
                sprintf "%s %s.%s(node : %s, _)" ret decl mi.Name nodeType

        let createSynMethod (self : obj) (mi : MethodInfo) (nodeType : Type) : SynMethod =
            let name = sprintf "Syn%s_%s" mi.Name nodeType.Name
            let dyn = 
                DynamicMethod(
                    name,
                    MethodAttributes.Public ||| MethodAttributes.Static,
                    CallingConventions.Standard,
                    typeof<obj>,
                    [| 
                        if not mi.IsStatic then yield typeof<obj>
                        yield typeof<obj>
                        yield typeof<Scope>
                    |],
                    typeof<Scope>,
                    true
                )

            let il = dyn.GetILGenerator()

            if mi.IsStatic then
                il.Emit(OpCodes.Ldarg, 0)
                if nodeType.IsValueType then 
                    il.Emit(OpCodes.Unbox, nodeType)
                    il.Emit(OpCodes.Ldind_Ref)

                il.Emit(OpCodes.Ldarg, 1)
                il.EmitCall(OpCodes.Call, mi, null)
                if mi.ReturnType.IsValueType then il.Emit(OpCodes.Box, mi.ReturnType)
                il.Emit(OpCodes.Ret)

                let del = dyn.CreateDelegate(typeof<SynDelegateStatic>) |> unbox<SynDelegateStatic>
                SynMethod(mi, fun (node : obj) (scope : Scope) -> del.Invoke(node, scope))
            else
                il.Emit(OpCodes.Ldarg, 0)
                if mi.DeclaringType.IsValueType then 
                    il.Emit(OpCodes.Unbox, mi.DeclaringType)
                    il.Emit(OpCodes.Ldind_Ref)

                il.Emit(OpCodes.Ldarg, 1)
                if nodeType.IsValueType then 
                    il.Emit(OpCodes.Unbox, nodeType)
                    il.Emit(OpCodes.Ldind_Ref)

                il.Emit(OpCodes.Ldarg, 2)

                if mi.IsVirtual then il.EmitCall(OpCodes.Callvirt, mi, null)
                else il.EmitCall(OpCodes.Call, mi, null)

                if mi.ReturnType.IsValueType then il.Emit(OpCodes.Box, mi.ReturnType)
                il.Emit(OpCodes.Ret)
                
                let del = dyn.CreateDelegate(typeof<SynDelegateInstance>) |> unbox<SynDelegateInstance>
                SynMethod(mi, fun (node : obj) (scope : Scope) -> del.Invoke(self, node, scope))

        let createInhMethod (self : obj) (mi : MethodInfo) (nodeType : Type) : InheritMethod =
            let name = sprintf "Inh%s_%s" mi.Name nodeType.Name
            let dyn = 
                DynamicMethod(
                    name,
                    MethodAttributes.Public ||| MethodAttributes.Static,
                    CallingConventions.Standard,
                    typeof<System.Void>,
                    [| 
                        if not mi.IsStatic then yield typeof<obj>
                        yield typeof<obj>
                        yield typeof<Scope>
                    |],
                    typeof<Scope>,
                    true
                )

            let il = dyn.GetILGenerator()

            if mi.IsStatic then
                il.Emit(OpCodes.Ldarg, 0)
                if nodeType.IsValueType then 
                    il.Emit(OpCodes.Unbox, nodeType)
                    il.Emit(OpCodes.Ldind_Ref)

                il.Emit(OpCodes.Ldarg, 1)
                il.EmitCall(OpCodes.Call, mi, null)
                il.Emit(OpCodes.Ret)

                let del = dyn.CreateDelegate(typeof<InhDelegateStatic>) |> unbox<InhDelegateStatic>
                InheritMethod(mi, fun (node : obj) (scope : Scope) -> del.Invoke(node, scope))
            else
                il.Emit(OpCodes.Ldarg, 0)
                if mi.DeclaringType.IsValueType then 
                    il.Emit(OpCodes.Unbox, mi.DeclaringType)
                    il.Emit(OpCodes.Ldind_Ref)

                il.Emit(OpCodes.Ldarg, 1)
                if nodeType.IsValueType then 
                    il.Emit(OpCodes.Unbox, nodeType)
                    il.Emit(OpCodes.Ldind_Ref)

                il.Emit(OpCodes.Ldarg, 2)

                if mi.IsVirtual then il.EmitCall(OpCodes.Callvirt, mi, null)
                else il.EmitCall(OpCodes.Call, mi, null)

                il.Emit(OpCodes.Ret)
                
                let del = dyn.CreateDelegate(typeof<InhDelegateInstance>) |> unbox<InhDelegateInstance>
                InheritMethod(mi, fun (node : obj) (scope : Scope) -> del.Invoke(self, node, scope))

        let private rootCreators = System.Collections.Concurrent.ConcurrentDictionary<Type, obj -> obj>()

        let getRootCreator (t : Type) =
            rootCreators.GetOrAdd(t, System.Func<Type,_>(fun nodeType ->
                let name = sprintf "NewRoot%s" nodeType.Name

                let res = typedefof<Root<_>>.MakeGenericType [| nodeType |]
                let ctor = res.GetConstructor([| nodeType |])
                let dyn = 
                    DynamicMethod(
                        name,
                        MethodAttributes.Public ||| MethodAttributes.Static,
                        CallingConventions.Standard,
                        typeof<obj>,
                        [| 
                            typeof<obj>
                        |],
                        typeof<obj>,
                        true
                    )
                let il = dyn.GetILGenerator()
                il.Emit(OpCodes.Ldarg, 0)
                il.Emit(OpCodes.Newobj, ctor)
                il.Emit(OpCodes.Ret)

                let del = dyn.CreateDelegate(typeof<NewRootDelegate>) |> unbox<NewRootDelegate>
                del.Invoke
            ))




        let private instances = ConcurrentDict<Type, option<obj>>(Dict())

        let tryCreateInstance (t : Type) =
            instances.GetOrCreate(t, fun t ->
                let ctor = t.GetConstructor(BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Instance, Type.DefaultBinder, [||], null)
                if isNull ctor then 
                    None
                else 
                    let v = ctor.Invoke([||])
                    Some v
            )
                
        let (|SynMethod|_|) (mi : MethodInfo) =
            let pars = mi.GetParameters()
            if pars.Length = 2 && mi.ReturnType <> typeof<System.Void> && mi.ReturnType <> typeof<unit> then
                if pars.[1].ParameterType.IsAssignableFrom typeof<Scope> then
                    Some(mi.Name, mi)
                else
                    None
            else    
                None

        let (|InhMethod|_|) (mi : MethodInfo) =
            let pars = mi.GetParameters()
            if pars.Length = 2 && (mi.ReturnType = typeof<System.Void> || mi.ReturnType = typeof<unit>) then
                if pars.[1].ParameterType.IsAssignableFrom typeof<Scope> then
                    Some(mi.Name, mi)
                else
                    None
            else    
                None
        

        type RuleTable() = 
            let all =
                let semTypes =
                    Introspection.GetAllTypesWithAttribute<RuleAttribute>()
                    |> Seq.collect (fun struct (t,_) -> t.GetMethods(BindingFlags.Static ||| BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.DeclaredOnly))
                let semMeths = 
                    Introspection.GetAllMethodsWithAttribute<RuleAttribute>()
                    |> Seq.map (fun struct (m,_) -> m)

                Seq.append semTypes semMeths 
                |> Seq.filter (fun m -> 
                    let decl = m.DeclaringType
                    let ps = m.GetParameters()

                    let generated = m.GetCustomAttribute<System.Runtime.CompilerServices.CompilerGeneratedAttribute>()

                    if not (isNull generated) then
                        false
                    elif decl.ContainsGenericParameters then
                        Log.warn "unexpected generic rule-type: %A" decl
                        false
                    elif ps.Length <> 2 || (ps.Length = 2 && ps.[1].ParameterType <> typeof<Scope>) then
                        Log.warn "unexpected rule: %A" m
                        false
                    elif not m.IsStatic then
                        let ctor = decl.GetConstructor(BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Instance, Type.DefaultBinder, [||], null)
                        if isNull ctor then
                            Log.warn "cannot construct rule type: %A" decl
                            false
                        else
                            true
                    else
                        true
                )
                |> Seq.toArray

            let synRules : Dictionary<string, MethodInfo[]> =
                all
                |> Seq.choose (function 
                    | SynMethod(name, mi) -> 
                        Some (name, mi)
                    | _ ->
                        None
                )
                |> Seq.groupBy fst
                |> Seq.map (fun (g, meths) -> g, meths |> Seq.map snd |> Seq.toArray)
                |> Dictionary.ofSeq
            
            let inhRules : Dictionary<string, MethodInfo[]> =
                all
                |> Seq.choose (function 
                    | InhMethod(name, mi) -> 
                        Some (name, mi)
                    | _ ->
                        None
                )
                |> Seq.groupBy fst
                |> Seq.map (fun (g, meths) -> g, meths |> Seq.map snd |> Seq.toArray)
                |> Dictionary.ofSeq

            let synCache = ConcurrentDict(Dict<string * Type * Type, option<SynMethod>>())
            let inhCache = ConcurrentDict(Dict<string * Type, option<InheritMethod>>())

            member x.TryGetSynRule(name : string, nodeType : Type, expectedType : Type) : option<SynMethod> =
                synCache.GetOrCreate((name, nodeType, expectedType), fun (name, nodeType, expectedType) ->
                    match synRules.TryGetValue name with
                    | (true, rules) ->
                        let argTypes = [| nodeType; typeof<Scope> |]
                        let applicable = rules |> Array.choose (fun m -> m.TrySpecialize(argTypes, expectedType))

                        if applicable.Length = 0 then
                            None
                        elif applicable.Length = 1 then
                            let m = applicable.[0]
                        
                            let instance =
                                if m.IsStatic then Some null
                                else tryCreateInstance m.DeclaringType
                            match instance with
                            | Some instance -> createSynMethod instance m nodeType |> Some
                            | None -> None
                        else
                            let mb = applicable |> Array.map (fun m -> m :> MethodBase)
                            try
                                let selected = 
                                    Type.DefaultBinder.SelectMethod(
                                        BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Instance ||| BindingFlags.Static,
                                        mb, argTypes, null
                                    )
                                match selected with
                                | null -> None
                                | :? MethodInfo as m ->
                                    let instance =
                                        if m.IsStatic then Some null
                                        else tryCreateInstance m.DeclaringType
                                    match instance with
                                    | Some instance ->createSynMethod instance m nodeType |> Some
                                    | None -> None
                                | _ -> 
                                    None
                            with _ ->
                                None
                    | _ ->
                        None
                )
         
            member x.TryGetInhRule(name : string, nodeType : Type) : option<InheritMethod> =
                inhCache.GetOrCreate((name, nodeType), fun (name, nodeType) ->
                    match inhRules.TryGetValue name with
                    | (true, rules) ->
                        let argTypes = [| nodeType; typeof<Scope> |]
                        let applicable = rules |> Array.choose (fun m -> m.TrySpecialize(argTypes, typeof<System.Void>))

                        if applicable.Length = 0 then
                            None
                        elif applicable.Length = 1 then
                            let m = applicable.[0]
                        
                            let instance =
                                if m.IsStatic then Some null
                                else tryCreateInstance m.DeclaringType
                            match instance with
                            | Some instance -> createInhMethod instance m nodeType |> Some
                            | None -> None
                        else
                            let mb = applicable |> Array.map (fun m -> m :> MethodBase)
                            try
                                let selected = 
                                    Type.DefaultBinder.SelectMethod(
                                        BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Instance ||| BindingFlags.Static,
                                        mb, argTypes, null
                                    )
                                match selected with
                                | null -> None
                                | :? MethodInfo as m ->
                                    let instance =
                                        if m.IsStatic then Some null
                                        else tryCreateInstance m.DeclaringType
                                    match instance with
                                    | Some instance -> createInhMethod instance m nodeType |> Some
                                    | None -> None
                                | _ -> 
                                    None
                            with _ ->
                                None
                    | _ ->
                        None
                )

        let table = lazy (RuleTable())

    let hasSynRule (nodeType : Type) (expected : Type) (name : string) =
        match table.Value.TryGetSynRule(name, nodeType, expected) with
        | Some _ -> true
        | None -> false

    let rec internal runinh (scope : Scope) (name : string) =
        match scope.TryGetInheritedCache(name) with
        | Some v -> 
            v
        | None ->
            match scope.Parent with
            | Some p ->
                if isNull p.Node then
                    let self = scope.Node.GetType().GetAllBaseTypesAndSelf()

                    let meth =
                        self |> List.tryPick (fun t ->
                            let pseudo = typedefof<Root<_>>.MakeGenericType [| t |]
                            match table.Value.TryGetInhRule(name, pseudo) with
                            | Some m -> Some (t, m)
                            | None-> None
                        )
                    
                    match meth with
                    | Some (t, inh) ->
                        let root = getRootCreator t scope.Node
                        let pseudo = Scope.Pseudo(root, scope)
                        let o = Scope.CurrentScope
                        Scope.CurrentScope <- Some pseudo
                        inh.Invoke root pseudo
                        Scope.CurrentScope <- o
                        match scope.TryGetInheritedCache(name) with
                        | Some v ->     
                            v
                        | None -> 
                            Log.warn "[Ag] bad root inherit method: %A" inh
                            None
                    | None ->
                        // root
                        None
                else

                    match table.Value.TryGetInhRule(name, p.Node.GetType()) with
                    | Some inh ->
                        let o = Scope.CurrentScope
                        Scope.CurrentScope <- Some p
                        inh.Invoke p.Node p
                        Scope.CurrentScope <- o
                        match scope.TryGetInheritedCache(name) with
                        | Some v -> v
                        | None -> 
                            Log.warn "[Ag] bad inherit method: %A" inh
                            None
                    | None ->
                        let res = runinh p name
                        scope.SetInherited(name, res)
                        res
            | None ->
                None

    let internal syn (node : 'a) (scope : Scope) (name : string) (expectedType : Type) =
        if isNull (node :> obj) then 
            None
        else
            let t = node.GetType()
            match table.Value.TryGetSynRule(name, t, expectedType)  with
            | Some syn ->
                let newScope = scope.GetChildScope(node)
                syn.Invoke (node :> obj) newScope |> Some
            | _ ->
                None
   
    
    type System.Object with
        member x.AllChildren = anyObj

    let internal set<'a, 'b when 'a : not struct> (target : 'a) (name : string) (value : 'b) =
        let node = 
            match target :> obj with
            | :? FSharp.Data.Adaptive.IAdaptiveValue as v -> v.GetValueUntyped(FSharp.Data.Adaptive.AdaptiveToken.Top)
            | n -> n
        match Scope.CurrentScope with
        | Some s -> 
            // classic aval unpacking here
            s.SetInheritedForChild(node, name, Some (value :> obj))
        | None ->
            Scope.SetGlobalValue(node, name, value :> obj)

    let (?<-) (target : 'a) (name : string) (value : 'b) = set target name value

    [<CompilerMessage("internal use only", 1337, IsHidden = true)>]
    type Operators private() =
        static member Get(scope : Scope, name : string) : 'a =
            match runinh scope name with
            | Some (:? 'a as v) -> v
            | _ -> failwithf "[Ag] could not get inh attribute %s in scope %A" name scope

        static member Get(node : 'a, name : string) : Scope -> 'b =
            fun s -> 
                match syn (node :> obj) s name typeof<'b> with
                | Some (:? 'b as v) -> 
                    v
                | Some v ->
                    failwithf "[Ag] invalid result for syn attribute %s on node %A: %A" name node v
                | None ->
                    failwithf "[Ag] could not get syn attribute %s on node %A" name node

    let inline private opAux (_d : 'd) (a : 'a) (b : 'b) : 'c =
        ((^a or ^b or ^d) : (static member Get : ^a * ^b -> ^c) (a, b))

    let inline (?) a b = opAux Unchecked.defaultof<Operators> a b


[<AbstractClass; Sealed; Extension>]
type AgScopeExtensions private() =
    [<Extension>]
    static member TryGetInherited(this : Ag.Scope, name : string) =
        Ag.runinh this name
        
    [<Extension>]
    static member TryGetInherited<'a>(this : Ag.Scope, name : string) =
        match Ag.runinh this name with
        | Some (:? 'a as res) -> Some res
        | _ -> None
        
    [<Extension>]
    static member GetInherted(this : Ag.Scope, name : string) =
        match Ag.runinh this name with
        | Some v -> v
        | None -> failwithf "[Ag] could not get inh attribute %s in scope %A" name this
        
    [<Extension>]
    static member GetInherted<'a>(this : Ag.Scope, name : string) =
        match Ag.runinh this name with
        | Some (:? 'a as res) -> res
        | _ -> failwithf "[Ag] could not get inh attribute %s in scope %A" name this
        
    [<Extension>]
    static member TryGetSynthesized<'a>(node : obj, name : string, scope : Ag.Scope) =
        match Ag.syn node scope name typeof<'a> with
        | Some (:? 'a as res) -> Some res 
        | _ -> None
        
    [<Extension>]
    static member TryGetSynthesized<'a>(scope : Ag.Scope, name : string) =
        if isNull scope.Node then failwithf "[Ag] cannot get syn attribute for scope with no node: %A" scope
        match scope.Parent with
        | Some p -> scope.Node.TryGetSynthesized<'a>(name, p)
        | None -> scope.Node.TryGetSynthesized<'a>(name, Ag.Scope.Root)

    [<Extension>]
    static member GetSynthesized<'a>(node : obj, name : string, scope : Ag.Scope) =
        match Ag.syn node scope name typeof<'a> with
        | Some (:? 'a as res) -> res
        | _ -> failwithf "[Ag] could not get syn attribute %s on node %A" name node
        
    [<Extension>]
    static member TryGetAttributeValue(scope : Ag.Scope, name : string) =
        match scope.TryGetSynthesized(name) with
        | Some v -> Some v
        | None -> scope.TryGetInherited(name)

    [<Extension>]
    static member TryGetAttributeValue<'a>(scope : Ag.Scope, name : string) =
        match scope.TryGetSynthesized<'a>(name) with
        | Some v -> Some v
        | None -> scope.TryGetInherited<'a>(name)


//[<AutoOpen>]
//module AgOperators =    
//    open Ag
    
//    type System.Object with
//        member x.AllChildren = anyObj

//    let internal set<'a, 'b when 'a : not struct> (target : 'a) (name : string) (value : 'b) =
//        let node = 
//            match target :> obj with
//            | :? FSharp.Data.Adaptive.IAdaptiveValue as v -> v.GetValueUntyped(FSharp.Data.Adaptive.AdaptiveToken.Top)
//            | n -> n
//        match Scope.CurrentScope with
//        | Some s -> 
//            // classic aval unpacking here
//            s.SetInheritedForChild(node, name, Some (value :> obj))
//        | None ->
//            Scope.SetGlobalValue(node, name, value :> obj)

//    let (?<-) (target : 'a) (name : string) (value : 'b) = set target name value

//    [<CompilerMessage("internal use only", 1337, IsHidden = true)>]
//    type Operators private() =
//        static member Get(scope : Scope, name : string) : 'a =
//            match Ag.runinh scope name with
//            | Some (:? 'a as v) -> v
//            | _ -> failwithf "[Ag] could not get inh attribute %s in scope %A" name scope

//        static member Get(node : 'a, name : string) : Scope -> 'b =
//            fun s -> 
//                match syn (node :> obj) s name typeof<'b> with
//                | Some (:? 'b as v) -> 
//                    v
//                | Some v ->
//                    failwithf "[Ag] invalid result for syn attribute %s on node %A: %A" name node v
//                | None ->
//                    failwithf "[Ag] could not get syn attribute %s on node %A" name node

//    let inline private opAux (_d : 'd) (a : 'a) (b : 'b) : 'c =
//        ((^a or ^b or ^d) : (static member Get : ^a * ^b -> ^c) (a, b))

//    let inline (?) a b = opAux Unchecked.defaultof<Operators> a b

