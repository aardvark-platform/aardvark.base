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


    type Scope =
        {
            parent : Scope
            sourceNode : obj
            inhValues : Dictionary<string, Option<obj>>
        }

    let private noScope = { parent = Unchecked.defaultof<_>; sourceNode = null; inhValues = null }
    let private currentScope = new ThreadLocal<ref<Scope>>(fun () -> ref noScope)
    let private tempStore = new ThreadLocal<Dictionary<obj * string, obj>>(fun () -> Dictionary.empty)

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

    let init() =
        let types = Introspection.GetAllTypesWithAttribute<Ag.Semantic>()

        for t in types do
            let methods = t.E0.GetMethods(BindingFlags.Public ||| BindingFlags.Instance)

            for m in methods do
                if m.GetParameters().Length = 1 then
                    let name = m.Name
                    if m.ReturnType = typeof<unit> || m.ReturnType = typeof<System.Void> then
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





        ()


    let getCurrentScope() : Scope =
        !currentScope.Value

    let useScope (s : Scope) (f : unit -> 'a) =
        let r = currentScope.Value
        let old = !r
        r := s
        try f()
        finally r := old

    let trySynthesize (name : string) (o : obj) =
        useScope { parent = getCurrentScope(); sourceNode = o; inhValues = Dictionary.empty } (fun () ->
            match synFunctions.[name].TryInvoke([|o|]) with
                | (true, v) -> Some v
                | _ -> None
        )

    let rec tryInhherit (scopeRef : ref<Scope>) (name : string) =
        let scope = getCurrentScope()

        match scope.inhValues.TryGetValue name with
            | (true, v) -> v
            | _ ->
                scopeRef := scope.parent
                let res =
                    match inhFunctions.[name].TryInvoke([|scope.parent.sourceNode|]) with
                        | (true, _) -> 
                            let ts = tempStore.Value
                            let res =
                                match ts.TryGetValue((scope.sourceNode, name)) with
                                    | (true, v) -> Some v
                                    | _ -> None
                            
                            ts.Clear()
                            res

                        | _ -> tryInhherit scopeRef name

                scope.inhValues.[name] <- res
                res
                

    let (?<-) (n : obj) (name : string) (value : 'a) =
        tempStore.Value.[(n, name)] <- value
        
    let (?) (n : obj) (name : string) : 'a =
        let t = typeof<'a>
        if t.Name.StartsWith "FSharpFunc" then
            match trySynthesize name n with
                | Some v -> Delay.delay v
                | None -> failwith "sadsadasd"
        else
            let ref = currentScope.Value
            match tryInhherit ref name with
                | Some (:? 'a as v) -> v
                | _ -> failwith "askdnkasjdksajdl"

module NewAgTest =
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
        member x.Inh(r : Env) =
            r.C?Inh <- 0
            
        member x.Inh(n : Node) =
            n.C?Inh <- 1 + n?Inh

        member x.Value(a : Env) : int =
            a.C?Value()

        member x.Value(a : Leaf) =
            a.A + a?Inh

        member x.Value(n : Node) : int =
            n.C?Value()
        


    let run() =
        NewAg.init()
        let l = Leaf 1
        let t = Env(Node(Node(l)))
        let v : int = t?Value()
        printfn "%A" v


        l.A <- 5
        let v : int = t?Value()
        printfn "%A" v


