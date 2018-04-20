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

    type private NewableDict<'a, 'b>() =
        let dict = Dict<'a, 'b>()
        member x.Dict = dict


    let private attachedValues = System.Runtime.CompilerServices.ConditionalWeakTable<obj, NewableDict<string, obj>>()


    let private attachValue (name : string) (node : obj) (value : obj) =
        let dict = attachedValues.GetOrCreateValue(node)
        dict.Dict.[name] <- value

    let private tryGetAttachedValue (name : string) (node : obj) =
        match attachedValues.TryGetValue node with
            | (true, d) -> d.Dict |> Dict.tryFind name
            | _ -> None


    type Semantic() =
        inherit System.Attribute()

    type Root<'a>(child : obj) =
        member x.Child = child

    type Scope =
        {
            parent : Scope
            sourceNode : obj
            inhValues : Dictionary<string, Option<obj>>
        } with

        member x.GetChildScope(node : obj) =
            { parent = x; sourceNode = node; inhValues = Dictionary.empty }

    type ThreadState =
        class
            val mutable public OneElementArray : obj[]
            val mutable public ObjRef : obj
            val mutable public Scope : Scope
            new(s) = { OneElementArray = Array.zeroCreate 1; ObjRef = null; Scope = s }
        end

    let mutable unpack : obj -> obj = id
    let emptyScope = { parent = Unchecked.defaultof<_>; sourceNode = null; inhValues = null }
    let internal anyObject = obj()

    type System.Object with
        member x.AllChildren = anyObject



    [<AutoOpen>]
    module Scoping =
        let threadState = new ThreadLocal<ThreadState>(fun () -> ThreadState(emptyScope))
        let tempStore = new ThreadLocal<Dictionary<obj * string, obj>>(fun () -> Dictionary.empty)

    let inline private opt (success : bool, value : 'a) =
        if success then Some value
        else None

    module Delay =

        open System
        open System.Reflection
        open System.Collections.Generic
        open Microsoft.FSharp.Reflection
        open System.Reflection.Emit

        type CreatorImpl<'a> private() =
            
            static let instance : obj -> 'a = fun _ -> failwith ""
//                let funType = typeof<FSharpFunc<obj, 'a>>
//                let bType = RuntimeMethodBuilder.bMod.DefineType(Guid.NewGuid().ToString(), TypeAttributes.Class ||| TypeAttributes.Public, funType)
//                let bMeth = bType.DefineMethod("Invoke", MethodAttributes.Public ||| MethodAttributes.Virtual, typeof<'a>, [|typeof<obj>|])
//                let il = bMeth.GetILGenerator()
//
//
//                let _,res = FSharpType.GetFunctionElements typeof<'a>
//                let fType = typedefof<FSharpFuncConst<_>>.MakeGenericType [|res|]
//                let ctor = fType.GetConstructor [|res|]
//
//                let l = il.DeclareLocal(res)
//                il.Emit(OpCodes.Ldarg_1)
//                il.Emit(OpCodes.Unbox_Any, res)
//                il.Emit(OpCodes.Newobj, ctor)
//                il.Emit(OpCodes.Unbox_Any, typeof<'a>)
//                il.Emit(OpCodes.Ret)
//
//                let bCtor = bType.DefineConstructor(MethodAttributes.Public, CallingConventions.Standard, [||])
//                let il = bCtor.GetILGenerator()
//                il.Emit(OpCodes.Ldarg_0)
//                il.Emit(OpCodes.Call, typeof<obj>.GetConstructor [||])
//                il.Emit(OpCodes.Ret)
//
//                bType.DefineMethodOverride(bMeth, funType.GetMethod "Invoke")
//                let t = bType.CreateType()
//                
//                Activator.CreateInstance t |> unbox<obj -> 'a>

            static member Instance = instance

        let inline creator<'a> : obj -> 'a = CreatorImpl<'a>.Instance
            
        let inline delay (value : obj) : 'a = creator<'a> value


    module private RootFunctions =
        open System.Reflection.Emit


        let contentType (t : Type) =
            if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Root<_>> then
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
                il.Emit(OpCodes.Newobj, typedefof<Root<_>>.MakeGenericType(parameterTypes.[i]).GetConstructor [|typeof<obj>|])



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


            let t = bType.CreateTypeInfo()

            if mi.IsStatic then
                let ctor = t.GetConstructor [||]
                ctor.Invoke [||], t.GetMethod "Invoke"
            else
                let ctor = t.GetConstructor [|mi.DeclaringType|]
                ctor.Invoke [|target|], t.GetMethod "Invoke"


    [<Obsolete>]
    let initialize() =
        ()

    [<OnAardvarkInit>]
    let init() =
        let types = Introspection.GetAllTypesWithAttribute<Semantic>()

        for struct (t, _) in types do
            let methods = t.GetMethods(BindingFlags.Public ||| BindingFlags.Instance)

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

                        if parameterType.IsGenericType && parameterType.GetGenericTypeDefinition() = typedefof<Root<_>> then
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



    let inline getCurrentScope() : Scope =
        threadState.Value.Scope

    let inline useScope (s : Scope) (f : unit -> 'a) =
        let state = threadState.Value
        let old = state.Scope
        state.Scope <- s
        try f()
        finally state.Scope <- old

    let inline useScope' (s : Scope) (f : ThreadState -> 'a) =
        let state = threadState.Value
        let old = state.Scope
        state.Scope <- s
        try f(state)
        finally state.Scope <- old


    let inline pushScope (sourceNode : obj) (f : ThreadState -> 'a) =
        let state = threadState.Value
        let scope = { parent = state.Scope; sourceNode = sourceNode; inhValues = Dictionary.empty}
        state.Scope <- scope
        let res = f(state)
        state.Scope <- scope.parent
        res

    let inline unscoped f = useScope emptyScope f

    let rec private tryFindAttachedValue (scope : Scope) (name : string) =
        if isNull scope.sourceNode then
            None
        else
            match tryGetAttachedValue name scope.sourceNode with
                | Some v -> Some v
                | None ->
                    tryFindAttachedValue scope.parent name



    let rec private tryInheritInternal (mm : Multimethod) (state : ThreadState) (name : string) =
        let scope = state.Scope

        match scope.inhValues.TryGetValue name with
            | (true, v) -> v
            | _ ->
                if isNull scope.parent.sourceNode then
                    match tryGetAttachedValue name scope.sourceNode with
                        | Some v -> Some v 
                        | None ->
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
                    state.Scope <- scope.parent
                    let res =
                        match tryGetAttachedValue name scope.sourceNode with
                            | Some v -> Some v 
                            | None ->
                                match mm.TryInvoke([|scope.parent.sourceNode|]) with
                                    | (true, _) -> 
                                        let ts = tempStore.Value
                                        let res =
                                            match ts.TryGetValue((scope.sourceNode, name)) with
                                                | (true, v) -> Some v
                                                | _ ->
                                                    match ts.TryGetValue((anyObject, name)) with
                                                        | (true, v) -> Some v
                                                        | _ ->  
                                                            failwithf "[Ag] invalid inherit method for attribute %s which does not write to %A" name scope.sourceNode
                            
                                        ts.Clear()
                                        res

                                    | _ -> tryInheritInternal mm state name
                            
                    state.Scope <- scope
                    scope.inhValues.[name] <- res
                    res
     


    let tryInherit (name : string) (o : obj) =
        match o with
            | :? Scope as s ->
                useScope' s (fun state ->
                    match inhFunctions.TryGetValue name with
                        | (true, mm) ->
                            tryInheritInternal mm state name
                        | _ ->
                            tryFindAttachedValue state.Scope name
                )
            | _ -> 
                let state = threadState.Value
                if isNull state.Scope.sourceNode then
                    None
                else
//                    if not (Unchecked.equals state.Scope.sourceNode o) then
//                        failwithf "inheriting %s for %A but current scope induces %A" name o state.Scope.sourceNode

                    match inhFunctions.TryGetValue name with
                        | (true, mm) ->
                            tryInheritInternal mm state name
                        | _ ->
                            tryFindAttachedValue state.Scope name

    let trySynthesize (name : string) (o : obj) =
        match synFunctions.TryGetValue(name) with
            | (true, f) ->
                match o with
                    | :? Scope as s ->
                        useScope' s (fun temp ->
                            temp.OneElementArray.[0] <- s.sourceNode
                            if f.TryInvoke(temp.OneElementArray, &temp.ObjRef) then
                                Some temp.ObjRef
                            else
                                None
                        )
                    | _ ->
                        pushScope o (fun temp ->
                            temp.OneElementArray.[0] <- o
                            if f.TryInvoke(temp.OneElementArray, &temp.ObjRef) then
                                Some temp.ObjRef
                            else
                                None
                        )
            | _ -> None
          
    let tryGet (name : string) (o : obj) =
        match trySynthesize name o with
            | Some v -> Some v
            | None -> tryInherit name o

    let (?<-) (n : obj) (name : string) (value : 'a) =
        let s = getCurrentScope()
        if isNull s.sourceNode then
            attachValue name n value
        else
            tempStore.Value.[(unpack n, name)] <- value
        
    type IsFunctionType<'a> private() =
        static let value = FSharpType.IsFunction typeof<'a>
        static member Value = value

    let inline isfunction<'a> = IsFunctionType<'a>.Value

    let (?) (n : obj) (name : string) : 'a =
        if isfunction<'a> then
            match trySynthesize name n with
                | Some v -> 
                    Delay.delay v
                | None -> 
                    failwithf "[Ag] unable to find synthesized attribute %s for node type: %A" name (n.GetType())
        else
            match n with
                | :? Scope as s ->
                    match useScope s (fun parent -> tryInherit name s.sourceNode) with
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
             
         
    [<Obsolete("use tryInherit instead")>]    
    let inline tryGetInhAttribute (node : obj) (name : string) =
        tryInherit name node
    
    [<Obsolete("use trySynthesize instead")>]    
    let inline tryGetSynAttribute (o : obj) (name : string) =
        trySynthesize name o
    
    [<Obsolete("use tryGet instead")>]    
    let tryGetAttributeValue (o : obj) (name:string) : Error<'a> =
        match tryGet name o with
            | Some v -> Success (v |> unbox<'a>)
            | None -> sprintf "attribute %A not found" name |> Error
                
    let getContext() = getCurrentScope()
    let setContext(v) = threadState.Value.Scope <- v

    [<AutoOpen>]
    module CapturedExtensions =
        type Scope with
            member x.TryGetAttributeValue(name : string) : Error<'a> =
                match tryGet name x with
                    | Some v -> Success (v |> unbox<'a>)
                    | None -> sprintf "attribute %A not found" name |> Error
module NewAgTest =
    open NewAg
    open System.Diagnostics

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
        


    type IList<'a> = interface end

    type Cons<'a>(head : 'a, tail : IList<'a>) =
        interface IList<'a>
        member x.Head = head
        member x.Tail = tail

    type Nil<'a>() =
        interface IList<'a>

    [<Ag.Semantic>]
    type ListSem() =
        member x.Sum(l : Cons<int>) =
            l.Head + l.Tail?Sum()
            
        member x.Sum(n : Nil<int>) =
            0 

    let rec directSum (l : IList<int>) =
        match l with
            | :? Nil<int> -> 0
            | :? Cons<int> as c -> c.Head + directSum c.Tail
            | _ -> 0

    let run() =
        Aardvark.Init()

        let rec longList (l : int) =
            if l <= 0 then Nil<int>() :> IList<_>
            else Cons(1, longList (l-1)) :> IList<_>


        let l = longList (100)

        let direct() =
            let sw = Stopwatch()
            let mutable iter = 0
            let mutable total = 0
            sw.Start()
            for i in 1..(1 <<< 22) do
                total <- total + directSum l
                iter <- iter + 1
            sw.Stop()
            printfn "direct: %.3fµs (%A)" (1000.0 * sw.Elapsed.TotalMilliseconds / float iter) (total /iter)


        let ag() =
            let sw = Stopwatch()
            let mutable iter = 0
            let mutable total = 0
            sw.Start()
            for i in 1..(1 <<< 16) do
                total <- total + l?Sum()
                iter <- iter + 1
            sw.Stop()
            printfn "ag:     %.3fµs (%A)" (1000.0 * sw.Elapsed.TotalMilliseconds / float iter) (total /iter)


        let profile() =
            let sw = Stopwatch()
            let mutable iter = 0
            let mutable total = 0
            sw.Start()
            for i in 1..(1 <<< 28) do
                total <- total + l?Sum()
                iter <- iter + 1
            sw.Stop()
            printfn "ag:     %.3fµs (%A)" (1000.0 * sw.Elapsed.TotalMilliseconds / float iter) (total /iter)


        let directv = directSum l
        printfn "direct: %A" directv
        let agv : int = l?Sum()
        printfn "ag:     %A" agv


        direct()
        ag()
        direct()
        ag()

        profile()

        Environment.Exit 0


        let l = Leaf 1
        let t = Node(Node(l))
        let v : int = t?Value()
        printfn "%A" v


        l.A <- 5
        let v : int = t?Value()
        printfn "%A" v

        NewAg.tryGet "Sepp" v |> printfn "Sepp = %A"



