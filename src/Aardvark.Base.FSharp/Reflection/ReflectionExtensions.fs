namespace Aardvark.Base

open System
open System.Reflection
open System.Runtime.CompilerServices
open FSharp.Data.Adaptive


[<AbstractClass; Sealed; Extension>]
type InheritanceTypeExtensions private() = 


    static let topoSort (types : Type[]) =
        let rec cmp (a : Type) (b : Type) =
            if a = b then 0

            elif a.IsGenericParameter && b.IsGenericParameter then compare a.Name b.Name
            elif a.IsGenericParameter then 1
            elif b.IsGenericParameter then -1

            elif a.IsAssignableFrom b then 1
            elif b.IsAssignableFrom a then -1


            elif a.IsInterface && b.IsInterface then compare a.FullName b.FullName
            elif a.IsInterface then 1
            elif b.IsInterface then -1

            elif a.IsGenericType && b.IsGenericType then compare a.FullName b.FullName
            elif a.IsGenericType then -1
            elif b.IsGenericType then 1
            else compare a.FullName b.FullName
        Array.sortWith cmp types
        
    static let rec baseTypes (t : Type) =
        if isNull t.BaseType || t.BaseType = t then 
            []
        else 
            let b = t.BaseType
            if b = typeof<System.ValueType> then [ typeof<obj> ]
            else b :: baseTypes b

    /// Gets the type's base-types (including interfaces) in topological order (from most specific to obj)
    [<Extension>]
    static member GetBaseTypes(this : Type) =
        if this.IsByRef then
            [||]
        elif this.IsInterface then
            this.GetInterfaces() |> topoSort
        else
            let ifaces = this.GetInterfaces()
            let types = baseTypes this |> List.toArray
            Array.append ifaces types |> topoSort
               
    /// Gets the type's base-types (including interfaces) in topological order (from most specific to obj)
    [<Extension>]
    static member GetBaseTypesAndSelf(this : Type) =
        if this.IsByRef then
            [| this |]
        elif this.IsInterface then
            Array.append [|this|] (this.GetInterfaces()) |> topoSort
        else
            let ifaces = this.GetInterfaces()
            let types = this :: baseTypes this |> List.toArray
            Array.append ifaces types |> topoSort
               
    /// Replaces all occurences of p in x with v
    [<Extension>]
    static member Substitute(x : Type, p : Type, v : Type) =
        if x = p then
            v
        elif x.IsGenericParameter then
            x

        elif x.IsByRef then
            let e = x.GetElementType().Substitute(p, v)
            e.MakeByRefType()

        elif x.IsArray then
            let r = x.GetArrayRank()
            let e = x.GetElementType().Substitute(p, v)
            e.MakeArrayType(r)

        elif x.IsGenericType then
            let tdef = x.GetGenericTypeDefinition()
            let targs = x.GetGenericArguments() |> Array.map (fun a -> a.Substitute(p, v))
            tdef.MakeGenericType targs

        else
            x
         
    /// Replaces all generic parameters using the given mapping.
    [<Extension>]
    static member SubstituteParameters(x : Type, mapping : Type -> Type) =
        if x.ContainsGenericParameters then
            if x.IsGenericParameter then
                mapping x

            elif x.IsByRef then
                let e = x.GetElementType().SubstituteParameters mapping
                e.MakeByRefType()

            elif x.IsArray then
                let r = x.GetArrayRank()
                let e = x.GetElementType().SubstituteParameters mapping
                e.MakeArrayType(r)

            elif x.IsGenericType then
                let tdef = x.GetGenericTypeDefinition()
                let targs = x.GetGenericArguments() |> Array.map (fun a -> a.SubstituteParameters mapping)
                tdef.MakeGenericType targs

            else
                x
        else
            x
       
    [<Extension>]
    static member FreeVariables(x : Type) =
        if x.ContainsGenericParameters then
            if x.IsGenericParameter then
                HashSet.single x

            elif x.IsArray || x.IsByRef then
                x.GetElementType().FreeVariables()

            elif x.IsGenericType then
                (HashSet.empty, x.GetGenericArguments()) ||> Array.fold (fun s t ->
                    HashSet.union s (t.FreeVariables())
                ) 
            else
                failwith "bad"
        else
            HashSet.empty
            
    /// topologically sorts the given types (from most specific to obj)
    [<Extension>]
    static member TopologicalSort(this : seq<Type>) =
        match this with
        | :? array<Type> as this -> topoSort this
        | _ -> topoSort (Seq.toArray this)


module private MethodResolver =

    type TypeDef =
        | Generic of Type * Type[]
        | Array of dim : int * Type
        | ByRef of Type

        static member GetDefinition(x : Type) =
            if x.IsArray then Array (x.GetArrayRank(), x.GetElementType())
            elif x.IsByRef then ByRef (x.GetElementType())
            elif x.IsGenericType then Generic (x.GetGenericTypeDefinition(), x.GetGenericArguments())
            else Generic(x, [||])

        member x.EqualDefinition(other : TypeDef) =
            match x, other with
            | Generic(a,_), Generic(b,_) -> a = b
            | Array(a,_), Array(b,_) -> a = b
            | ByRef _, ByRef _ -> true
            | _ -> false

        member x.IsInterface =
            match x with
            | Generic (t, _) -> t.IsInterface
            | _ -> false

        member x.IsSealed =
            match x with
            | Generic (t, _) -> t.IsSealed
            | _ -> true

        member x.IsValueType =
            match x with
            | Generic (t, _) -> t.IsValueType
            | Array _ -> false
            | _ -> true
                
        member x.FullName =
            match x with
            | Generic (t, _) -> t.FullName
            | _ -> ""

        member x.GenericArguments =
            match x with
            | Generic(_, args) -> args
            | Array(dim, t) -> [| t |]
            | ByRef t -> [| t |]

        member x.Make(args : Type[]) =
            match x with
            | Generic(t, _) -> t.MakeGenericType args
            | Array(dim,_) -> args.[0].MakeArrayType(dim)
            | ByRef _ -> args.[0].MakeByRefType()

        member x.BaseType =
            match x with
            | Generic(t, targs) ->
                let self = if targs.Length > 0 then t.MakeGenericType(targs) else t
                if not (isNull self.BaseType) && self.BaseType <> self then
                    let baseDef = self.BaseType |> TypeDef.GetDefinition
                    Some baseDef
                else
                    None
            | Array(1, t) ->
                Generic(typeof<System.Array>, [||]) |> Some
            | _ ->
                None
                
        member x.TryGetBaseType(t : TypeDef) =
            if x.EqualDefinition t then Some x
            elif t.IsSealed || t.IsValueType then None
            else
                match x.BaseType with
                | Some b -> b.TryGetBaseType t
                | None -> None


    type System.Type with
        member x.GetDefinition() =
            if x.IsArray then Array (x.GetArrayRank(), x.GetElementType())
            elif x.IsByRef then ByRef (x.GetElementType())
            elif x.IsGenericType then Generic (x.GetGenericTypeDefinition(), x.GetGenericArguments())
            else Generic(x, [||])

    module SolverNoVariance =
        let rec tryUnify (mapping : HashMap<Type, Type>) (p : Type) (a : Type) =
            let pd = p.GetDefinition()
            let ad = a.GetDefinition()

            if p = a then 
                Some mapping
            elif p.IsGenericParameter then
                match HashMap.tryFind p mapping with
                | Some old ->
                    if a.ContainsGenericParameters then
                        let m = HashMap.remove p mapping
                        tryUnify m old a
                    else
                        if old <> a then None
                        else Some mapping
                | None ->
                    Some (HashMap.add p a mapping)
            elif a.IsGenericParameter then
                match HashMap.tryFind a mapping with
                | Some old ->
                    if p.ContainsGenericParameters then
                        let m = HashMap.remove a mapping
                        tryUnify m old p
                    else
                        if old <> p then None
                        else Some mapping
                | None ->
                    Some (HashMap.add a p mapping)
            elif pd.EqualDefinition ad then
                (Some mapping, pd.GenericArguments, ad.GenericArguments) |||> Array.fold2 (fun s pi ai ->
                    match s with
                    | Some s -> tryUnify s pi ai
                    | None -> None
                )
            elif p.IsInterface then
                let ri = a.GetInterface(pd.FullName)
                if isNull ri then 
                    None
                else
                    tryUnify mapping p ri
            else
                a.GetBaseTypes() |> Array.tryPick (tryUnify mapping p)

    let tryMakeApplicable (args : Type[]) (ret : Type) (meth : MethodInfo) : option<MethodInfo> =
        let pars = meth.GetParameters()
        if pars.Length = args.Length then
            if meth.ContainsGenericParameters then
                let def = meth.GetGenericMethodDefinition()
                let tpars = def.GetGenericArguments()

                let argumentConstraints = 
                    (Some HashMap.empty, pars, args) |||> Array.fold2 (fun ass p a -> 
                        match ass with
                        | Some ass -> 
                            SolverNoVariance.tryUnify ass p.ParameterType a
                            //Solver.tryUnify ass p.ParameterType a ConstraintKind.SuperType
                        | None ->
                            None
                    )

                let finalConstraints =
                    match argumentConstraints with
                    | Some a -> 
                        SolverNoVariance.tryUnify a ret meth.ReturnType
                        //Solver.tryUnify a meth.ReturnType ret ConstraintKind.SubType
                    | None -> None


                match finalConstraints with
                | Some solution ->
                    let instantiation = tpars |> Array.map (fun p -> match HashMap.tryFind p solution with | Some t -> t | None -> typeof<obj>)
                    if instantiation.Length = tpars.Length then
                        let final = def.MakeGenericMethod instantiation
                        if ret.IsAssignableFrom final.ReturnType then
                            Some final
                        else
                            // bad return type
                            None
                    else    
                        // internal error: assignment not complete
                        None
                | None ->
                    // could not unify
                    None
            else
                if ret.IsAssignableFrom meth.ReturnType then
                    let argsMatch = (pars, args) ||> Array.forall2 (fun p a -> p.ParameterType.IsAssignableFrom a)
                    if argsMatch then
                        Some meth
                    else
                        // bad parameter types
                        None
                else
                    // bad return type
                    None
        else
            // bad parameter count
            None


[<AbstractClass; Sealed; Extension>]
type MethodInfoGenericExtensions private() =    
    [<Extension>]
    static member TrySpecialize(this : MethodInfo, args : Type[], ret : Type) =
        MethodResolver.tryMakeApplicable args ret this

    [<Extension>]
    static member Specialize(this : MethodInfo, args : Type[], ret : Type) =
        match MethodResolver.tryMakeApplicable args ret this with
        | Some m -> m
        | None -> null


[<System.Runtime.CompilerServices.Extension>]
module ReflectionHelpers =
    open Microsoft.FSharp.Reflection

    let private lockObj = obj()

    let private prettyNames =
        Dict.ofList [
            typeof<sbyte>, "sbyte"
            typeof<byte>, "byte"
            typeof<int16>, "int16"
            typeof<uint16>, "uint16"
            typeof<int>, "int"
            typeof<uint32>, "uint32"
            typeof<int64>, "int64"
            typeof<uint64>, "uint64"
            typeof<nativeint>, "nativeint"
            typeof<unativeint>, "unativeint"

            typeof<char>, "char"
            typeof<string>, "string"


            typeof<float32>, "float32"
            typeof<float>, "float"
            typeof<decimal>, "decimal"

            typeof<obj>, "obj"
            typeof<unit>, "unit"
            typeof<System.Void>, "void"

        ]

    let private genericPrettyNames =
        Dict.ofList [
            typedefof<list<_>>, "list"
            typedefof<Option<_>>, "Option"
            typedefof<Set<_>>, "Set"
            typedefof<Map<_,_>>, "Map"
            typedefof<seq<_>>, "seq"

        ]

    let private idRx = System.Text.RegularExpressions.Regex @"[a-zA-Z_][a-zA-Z_0-9]*"

    let rec private getPrettyNameInternal (t : Type) =
        let res = 
            match prettyNames.TryGetValue t with
                | (true, n) -> n
                | _ ->
                    if t.IsArray then
                        t.GetElementType() |> getPrettyNameInternal |> sprintf "%s[]"

                    elif FSharpType.IsTuple t then
                        FSharpType.GetTupleElements t |> Seq.map getPrettyNameInternal |> String.concat " * "

                    elif FSharpType.IsFunction t then
                        let (arg, res) = FSharpType.GetFunctionElements t

                        sprintf "%s -> %s" (getPrettyNameInternal arg) (getPrettyNameInternal res)

                    elif typeof<Aardvark.Base.INatural>.IsAssignableFrom t then
                        let s = Aardvark.Base.Peano.getSize t
                        sprintf "N%d" s

                    elif t.IsGenericType then
                        let args = t.GetGenericArguments() |> Seq.map getPrettyNameInternal |> String.concat ", "
                        let bt = t.GetGenericTypeDefinition()
                        match genericPrettyNames.TryGetValue bt with
                            | (true, gen) ->
                                sprintf "%s<%s>" gen args
                            | _ ->
                                let gen = idRx.Match bt.Name
                                sprintf "%s<%s>" gen.Value args


                    else
                        t.Name

        prettyNames.[t] <- res
        res

    [<System.Runtime.CompilerServices.Extension; CompiledName("GetPrettyName")>]
    let getPrettyName(t : Type) =
        lock lockObj (fun () ->
            getPrettyNameInternal t
        )

    type Type with
        member x.PrettyName =
            lock lockObj (fun () ->
                getPrettyNameInternal x
            )


/// <summary>
/// Defines a number of active patterns for matching expressions. Includes some
/// functionality missing in F#.
/// </summary>
[<AutoOpen>]
module ReflectionPatterns =
    open Microsoft.FSharp.Quotations
    open QuotationReflectionHelpers

    let private typePrefixPattern = System.Text.RegularExpressions.Regex @"^.*\.(?<methodName>.*)$"
    let (|Method|_|)  (mi : MethodInfo) =
        let args = mi.GetParameters() |> Seq.map(fun p -> p.ParameterType)
        let parameters = if mi.IsStatic then
                            args
                            else
                            seq { yield mi.DeclaringType; yield! args }

        let m = typePrefixPattern.Match mi.Name
        let name =
            if m.Success then m.Groups.["methodName"].Value
            else mi.Name

        Method (name, parameters |> Seq.toList) |> Some

    let private compareMethods (template : MethodInfo) (m : MethodInfo) =
        if template.IsGenericMethod && m.IsGenericMethod then
            if template.GetGenericMethodDefinition() = m.GetGenericMethodDefinition() then
                let targs = template.GetGenericArguments() |> Array.toList
                let margs = m.GetGenericArguments() |> Array.toList

                let zip = List.zip targs margs

                let args = zip |> List.filter(fun (l,r) -> l.IsGenericParameter) |> List.map (fun (_,a) -> a)

                Some args
            else
                None
        elif template = m then
            Some []
        else
            None          

    let (|MethodQuote|_|) (e : Expr) (mi : MethodInfo) =
        let m = tryGetMethodInfo e
        match m with
            | Some m -> match compareMethods m mi with
                            | Some a -> MethodQuote(a) |> Some
                            | None -> None
            | _ -> None


    let (|Create|_|) (c : ConstructorInfo) =
        Create(c.DeclaringType, c.GetParameters() |> Seq.toList) |> Some