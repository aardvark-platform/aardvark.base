namespace Aardvark.Base

open System
open System.Reflection
open System.Runtime.CompilerServices
open FSharp.Data.Adaptive

            
module private MethodResolver =
    type ConstraintKind =
        | Equals
        | SubType
        | SuperType

    type SkolemType private(tid : int) =
        inherit System.Type()
        static let mutable currentId = -1
            
        let guid = Guid.NewGuid()

        let name =
            let mutable str = ""
            let mutable r = tid
            while r > 0 || str.Length = 0 do
                let v = r % 26
                let c = char (int 'A' + v)
                str <- System.String(c, 1) + str
                r <- r / 26
            "'" + str

        new() = SkolemType(System.Threading.Interlocked.Increment(&currentId))

        member x.Id = tid

        override x.GetGenericParameterConstraints() = [||]
        override x.ToString() = name
        override x.GetHashCode() = tid.GetHashCode()
        override x.Equals (o : obj) =
            match o with
            | :? SkolemType as o -> o.Id = tid
            | _ -> false

        override x.GenericParameterAttributes = GenericParameterAttributes.None
        override x.IsGenericParameter = true
        override x.Assembly = null
        override x.AssemblyQualifiedName = name
        override x.BaseType = typeof<obj>
        override x.FullName = name
        override x.Module = Unchecked.defaultof<Module>
        override x.Namespace = ""
        override x.UnderlyingSystemType = null
        override x.GetAttributeFlagsImpl() = TypeAttributes.Class
        override x.GUID = guid
        override x.GetConstructorImpl(_,_,_,_,_) = null
        override x.GetConstructors(_) = [||]
        override x.GetElementType() = null
        override x.GetEvent(_,_) = null
        override x.GetEvents(_) = [||]
        override x.GetField(_,_) = null
        override x.GetFields(_) = [||]
        override x.GetInterface(_,_) = null
        override x.GetInterfaces() = [||]
        override x.GetMembers(_) = [||]
        override x.GetMethodImpl(_,_,_,_,_,_) = null
        override x.GetMethods(_) = [||]
        override x.GetNestedType(_,_) = null
        override x.GetNestedTypes(_) = [||]
        override x.GetProperties(_) = [||]
        override x.GetPropertyImpl(_,_,_,_,_,_) = null
        override x.InvokeMember(_,_,_,_,_,_,_,_) = failwith "sadsad"
        override x.HasElementTypeImpl() = false
        override x.IsArrayImpl() = false
        override x.IsByRefImpl() = false
        override x.IsCOMObjectImpl() = false
        override x.IsPointerImpl() = false
        override x.IsPrimitiveImpl() = false
        override x.Name = name
        override x.GetCustomAttributes(_) = [||]
        override x.GetCustomAttributes(_,_) = [||]
        override x.IsDefined(_,_) = false

    type TypeDef =
        | Generic of Type * Type[]
        | Array of dim : int * Type
        | ByRef of Type

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
                
        member x.FullName =
            match x with
            | Generic (t, _) -> t.FullName
            | _ -> ""

        member x.GenericParameters =
            match x with
            | Generic(t, _) -> t.GetGenericArguments()
            | Array _ -> [| SkolemType() :> Type |]
            | ByRef _ -> [| SkolemType() :> Type |]
                
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


    type System.Type with
        member x.GetDefinition() =
            if x.IsArray then Array (x.GetArrayRank(), x.GetElementType())
            elif x.IsByRef then ByRef (x.GetElementType())
            elif x.IsGenericType then Generic (x.GetGenericTypeDefinition(), x.GetGenericArguments())
            else Generic(x, [||])

        member x.Substitute(p : Type, v : Type) =
            if x = p then
                v
            elif x.IsGenericParameter then
                x
            elif x.ContainsGenericParameters then
                let tdef = x.GetGenericTypeDefinition()
                let targs = x.GetGenericArguments() |> Array.map (fun a -> a.Substitute(p, v))
                tdef.MakeGenericType targs
            else
                x
        member x.GetBaseTypes() =
            if x.IsInterface then
                x.GetInterfaces() |> Array.toList
            else
                let b = x.BaseType
                if isNull b || b = x then []
                else b :: b.GetBaseTypes()

        member x.GetAllSuperTypesAndSelf() =
            if x.IsValueType then 
                [x]
            elif x.IsInterface then
                x :: (Array.toList (x.GetInterfaces()))
            else
                let b = x.GetBaseTypes()
                x :: (Array.toList (x.GetInterfaces())) @ b

        member x.ParameterConstraint =
            if x.IsGenericParameter then
                //ConstraintKind.Equals
                if x.GenericParameterAttributes.HasFlag(GenericParameterAttributes.Covariant) then ConstraintKind.SubType
                elif x.GenericParameterAttributes.HasFlag(GenericParameterAttributes.Contravariant) then ConstraintKind.SuperType
                else ConstraintKind.Equals
            else
                failwith "bad param"

    module Solver = 
        let inline conflicting (a : ConstraintKind, at : Type) (b : ConstraintKind, bt : Type) =
            // TODO: early exists
            false

        let addConstraintFw (p : Type) (c : ConstraintKind, t : Type) (constraints : HashMap<Type, list<ConstraintKind * Type>>) =
            if p = t then
                Some constraints
            else
                let c = 
                    if t.IsValueType then Equals
                    elif t.IsSealed then c
                        //match c with
                        //| SubType -> Equals
                        //| _ -> c
                    else
                        c
                match HashMap.tryFind p constraints with
                | Some cs ->
                    let conflict = cs |> List.exists (conflicting (c,t))
                    if conflict then 
                        None
                    else
                        let c1 = HashMap.add p ((c,t) :: cs) constraints
                        Some c1
                | None ->
                    let c1 = HashMap.add p [c,t] constraints
                    Some c1
                      
        let addConstraint (p : Type) (c : ConstraintKind, t : Type) (constraints : HashMap<Type, list<ConstraintKind * Type>>) =
            let a = addConstraintFw p (c,t) constraints
            match a with
            | Some a ->
                if t.IsGenericParameter then
                    let inv =
                        match c with
                        | Equals -> Equals
                        | SuperType -> SubType
                        | SubType -> SuperType

                    addConstraintFw t (inv, p) a
                else
                    Some a
            | None ->
                None

        let rec tryUnify (current : HashMap<Type, list<ConstraintKind * Type>>) (p : Type) (a : Type) (relationKind : ConstraintKind) : option<HashMap<Type, list<ConstraintKind * Type>>> =

            if p.IsGenericParameter then
                addConstraint p (relationKind, a) current

            elif a.IsGenericParameter then
                let inv =
                    match relationKind with
                    | Equals -> Equals
                    | SuperType -> SubType
                    | SubType -> SuperType
                addConstraint a (inv, p) current

            elif a.ContainsGenericParameters || p.ContainsGenericParameters then
                let ad = a.GetDefinition()
                let pd = p.GetDefinition()

                if ad.EqualDefinition(pd) then
                    let pargs = pd.GenericArguments
                    let ppars = 
                        pd.GenericParameters |> Array.map (fun p -> 
                            let hole = SkolemType()
                            hole, p.ParameterConstraint
                        )

                    let aargs = ad.GenericArguments
                    let apars = 
                        ad.GenericParameters |> Array.map (fun p -> 
                            let hole = SkolemType()
                            hole, p.ParameterConstraint
                        )


                    let current = 
                        (Some current, ppars, pargs) |||> Array.fold2 (fun c (pd,kind) p -> 
                            match c with
                            | Some c -> addConstraint pd (kind, p) c
                            | None -> None
                        )

                    let current = 
                        (current, apars, aargs) |||> Array.fold2 (fun c (pd, kind) p -> 
                            match c with
                            | Some c -> addConstraint pd (kind, p) c
                            | None -> None
                        )

                    (current, ppars, apars) |||> Array.fold2 (fun ass (pd, _) (ad, _) -> 
                        match ass with
                        | Some ass -> tryUnify ass pd ad Equals
                        | None -> None
                    )
                else
                    match relationKind with
                    | Equals -> 
                        None
                    | SuperType ->
                        if pd.IsInterface then
                            let aiface = a.GetInterface(pd.FullName)
                            if isNull aiface then
                                None
                            else
                                tryUnify current p aiface relationKind
                        else
                            let matching = 
                                a.GetBaseTypes() |> Seq.tryPick (fun b ->
                                    let bd = b.GetDefinition()
                                    if pd.EqualDefinition(bd) then
                                        Some b
                                    else
                                        None
                                )
                            match matching with
                            | Some m -> tryUnify current p m Equals
                            | None -> None
                    | SubType ->
                        tryUnify current a p SuperType
            else 
                match relationKind with
                | Equals -> 
                    if p = a then Some current
                    else None
                | SuperType ->
                    if p.IsAssignableFrom a then Some current
                    else None
                | SubType ->
                    if a.IsAssignableFrom p then Some current
                    else None

        let check (a : Type) (k : ConstraintKind, p : Type)  =
            if a.IsGenericParameter || p.IsGenericParameter then 
                true
            else
                match k with
                | SuperType ->
                    if a = p then true
                    elif p.IsValueType || a.IsValueType then false
                    elif a.IsInterface then p.GetInterface(a.FullName) |> isNull |> not
                    else a.IsAssignableFrom p
                | SubType -> 
                    if a = p then true
                    elif p.IsValueType || a.IsValueType then false
                    elif p.IsInterface then a.GetInterface(p.FullName) |> isNull |> not
                    else p.IsAssignableFrom a
                | Equals -> 
                    p = a

        let tryRemoveBestRule (current : HashMap<Type, list<ConstraintKind * Type>>) =
            if current.Count = 0 then
                None
            else
                let withCost = 
                    current |> HashMap.map (fun p constraints ->
                        let sorted = 
                            constraints
                            |> List.map (fun (k,v) ->
                                if v.ContainsGenericParameters then 
                                    1000,(k,v)
                                else
                                    match k with
                                    | Equals ->     
                                        0,(k,v)
                                    | SuperType -> 
                                        if v.IsInterface then 3, (k,v)
                                        else 1, (k,v)
                                    | SubType ->
                                        if v.IsInterface then 4, (k,v)
                                        else 2,(k,v)
                            )
                            |> List.sortBy fst
                        sorted
                    )
                match withCost |> Seq.minBy (snd >> List.head >> fst) with 
                | (p, (_,(k,a)) :: rest) ->
                    let newConstraints = HashMap.remove p current
                    let tests = rest |> List.map snd

                    let solutions =
                        match k with
                        | Equals -> [a]
                        | SuperType -> a.GetAllSuperTypesAndSelf()
                        | SubType -> [a]

                    let sols = solutions |> List.filter (fun t -> List.forall (check t) tests)

                    Some (p, sols, newConstraints)
                | _, [] ->
                    failwith ""

        let solve (current : HashMap<Type, list<ConstraintKind * Type>>) =
            let rec solve (sol : HashMap<Type, Type>) (current : HashMap<Type, list<ConstraintKind * Type>>) =
                match tryRemoveBestRule current with
                | Some (p, values, current) ->
                    use e = (values :> seq<_>).GetEnumerator()
                    let mutable final = None
                    while Option.isNone final && e.MoveNext() do
                        let typ = e.Current
                        let substituted = 
                            current |> HashMap.map (fun _ l ->
                                l |> List.map (fun (k,a) -> k, a.Substitute(p, typ))
                            )
                        //Log.start "%A = %A" p typ

                        //Log.start "rules"
                        //for (p, cs) in substituted do
                        //    for (k, a) in cs do
                        //        match k with
                        //        | Equals -> Log.line "%A = %A" p a
                        //        | SuperType -> Log.line "%A <- %A" p a
                        //        | SubType -> Log.line "%A <- %A" a p
                        //Log.stop()

                        final <- solve (HashMap.add p typ sol) substituted
                        //Log.stop ()
                    final
                | None ->
                    if current.Count = 0 then Some sol
                    else None

            solve HashMap.empty current


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
                            Solver.tryUnify ass p.ParameterType a ConstraintKind.SuperType
                        | None ->
                            None
                    )

                let finalConstraints =
                    match argumentConstraints with
                    | Some a -> Solver.tryUnify a meth.ReturnType ret ConstraintKind.SubType
                    | None -> None


                match finalConstraints with
                | Some assignment ->
                    //for (p, cs) in assignment do
                    //    for (k, a) in cs do
                    //        match k with
                    //        | Equals -> printfn "%A = %A" p a
                    //        | SuperType -> printfn "%A <- %A" p a
                    //        | SubType -> printfn "%A <- %A" a p
                            
                    match Solver.solve assignment with
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

    let getApplicableMethods (args : Type[]) (ret : Type) (methods : MethodInfo[]) =
        methods |> Array.choose (tryMakeApplicable args ret)

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
      
    [<Extension>]
    static member GetBaseTypes(x : Type) =
        if x.IsInterface then
            x.GetInterfaces() |> Array.toList
        else
            let b = x.BaseType
            if isNull b || b = x then []
            else b :: b.GetBaseTypes()
  
    [<Extension>]
    static member GetAllBaseTypesAndSelf(x : Type) =
        if x.IsValueType then 
            [x]
        elif x.IsInterface then
            x :: (Array.toList (x.GetInterfaces()))
        else
            let b = x.GetBaseTypes()
            x :: (Array.toList (x.GetInterfaces())) @ b
