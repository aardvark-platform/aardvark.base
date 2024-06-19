namespace Aardvark.Base.IL

open System
open System.Reflection
open System.Reflection.Emit
open Aardvark.Base
open FSharp.Data.Adaptive

module TypeBuilderOld =
    
    let private bAss = 
        AssemblyBuilder.DefineDynamicAssembly(
            AssemblyName "TypeBuilder", 
            AssemblyBuilderAccess.Run
        )

    let private bMod =
        bAss.DefineDynamicModule("MainModule")


    let implementInterface (iface : Type) (methods : list<Type * string * list<Type> * CodeGen<unit>>) =

        let bType = 
            bMod.DefineType(
                Guid.NewGuid() |> string,
                TypeAttributes.Class,
                typeof<obj>,
                [|iface|]
            )

        let rec tryFindMethod (name : string) (args : Type[]) (ret : Type) (t : Type)=
            let m = t.GetMethod(name, BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Public, Type.DefaultBinder, args, null)
            if isNull m || m.ReturnType <> ret then
                let ifaces = t.GetInterfaces()
                ifaces |> Array.tryPick (tryFindMethod name args ret)
            else
                Some m

        let fields = 
            methods |> List.map (fun (ret, name, args, code) ->
                
                let prototype = 
                    match tryFindMethod name (List.toArray args) ret iface with
                        | Some meth -> meth
                        | _ -> null



                let fieldType = 
                    let args = iface::args |> List.toArray
                    if ret = typeof<System.Void> then
                        let actionType = Type.GetType(typedefof<Action<_>>.FullName.Replace("1", string args.Length))
                        actionType.MakeGenericType args
                    else
                        let funcType = Type.GetType(typedefof<Func<_>>.FullName.Replace("1", string (1 + args.Length)))
                        funcType.MakeGenericType (Array.append args [|ret|])

                let field = bType.DefineField("m_" + name, fieldType, FieldAttributes.Private)

                prototype, field, ret, List.toArray args, code
            )

        for (prototype, field, ret, args,_) in fields do
            let meth = 
                bType.DefineMethod(
                    field.Name.Substring 2, 
                    MethodAttributes.Virtual ||| MethodAttributes.Public,
                    ret,
                    args
                )

            let il = meth.GetILGenerator()

            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldfld, field)


            il.Emit(OpCodes.Ldarg_0)
            for i in 1..args.Length do
                il.Emit(OpCodes.Ldarg, i)

            let invoke = field.FieldType.GetMethod("Invoke")

            //il.Emit(OpCodes.Tailcall)
            il.EmitCall(OpCodes.Callvirt, invoke, null)
            il.Emit(OpCodes.Ret)

            if not (isNull prototype) then
                bType.DefineMethodOverride(meth, prototype)

        let fieldTypes = fields |> List.map (fun (_,f,_,_,_) -> f.FieldType) |> List.toArray

        let ctor = 
            bType.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                fieldTypes
            )

        let il = ctor.GetILGenerator()

        il.Emit(OpCodes.Ldarg_0)
        il.Emit(OpCodes.Call, typeof<obj>.GetConstructor [||])     


        let mutable index = 1
        for (_, field, _, _, _) in fields do

            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldarg, index)
            il.Emit(OpCodes.Stfld, field)
            

            index <- index + 1

        il.Emit(OpCodes.Ret)

        let t = bType.CreateTypeInfo()

        let delegates =
            fields |> List.map (fun (_, f, _,_,code) ->
                Assembler.assembleDelegateInternal f.FieldType (CodeGen.run code) :> obj
            )

        let ctorArgs = fields |> List.map (fun (_,f,_,_,_) -> f.FieldType) |> List.toArray
        let ctor = t.GetConstructor ctorArgs
        ctor.Invoke(List.toArray delegates)

    let implementSubType (baseType : Type) (methods : list<Type * string * list<Type> * CodeGen<unit>>) =

        let bType = 
            bMod.DefineType(
                Guid.NewGuid() |> string,
                TypeAttributes.Class,
                baseType,
                [||]
            )

        let rec tryFindMethod (name : string) (args : Type[]) (ret : Type) (t : Type)=
            let m = t.GetMethod(name, BindingFlags.Instance ||| BindingFlags.NonPublic ||| BindingFlags.Public, Type.DefaultBinder, args, null)
            if isNull m || m.ReturnType <> ret then
                let baseType = t.BaseType
                if isNull baseType then None
                else tryFindMethod name args ret baseType
            else
                Some m


        let fields = 
            methods |> List.map (fun (ret, name, args, code) ->
                
                let prototype = 
                    match tryFindMethod name (List.toArray args) ret baseType with
                        | Some meth -> meth
                        | _ -> null


                let fieldType = 
                    let args = baseType::args |> List.toArray
                    if ret = typeof<System.Void> then
                        let actionType = Type.GetType(typedefof<Action<_>>.FullName.Replace("1", string args.Length))
                        actionType.MakeGenericType args
                    else
                        let funcType = Type.GetType(typedefof<Func<_>>.FullName.Replace("1", string (1 + args.Length)))
                        funcType.MakeGenericType (Array.append args [|ret|])

                let field = bType.DefineField("m_" + name, fieldType, FieldAttributes.Private)

                prototype, field, ret, List.toArray args, code
            )

        for (prototype, field, ret, args, _) in fields do

            let meth = 
                bType.DefineMethod(
                    field.Name.Substring 2, 
                    MethodAttributes.Virtual ||| MethodAttributes.Public,
                    ret,
                    args
                )

            let il = meth.GetILGenerator()

            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldfld, field)

            il.Emit(OpCodes.Ldarg_0)
            for i in 1..args.Length do
                il.Emit(OpCodes.Ldarg, i)

            let invoke = field.FieldType.GetMethod("Invoke")

            il.Emit(OpCodes.Tailcall)
            il.EmitCall(OpCodes.Callvirt, invoke, null)
            il.Emit(OpCodes.Ret)

            if not (isNull prototype) then
                bType.DefineMethodOverride(meth, prototype)

        let fieldTypes = fields |> List.map (fun (_,f,_,_,_) -> f.FieldType) |> List.toArray

        let ctor = 
            bType.DefineConstructor(
                MethodAttributes.Public,
                CallingConventions.Standard,
                fieldTypes
            )

        let il = ctor.GetILGenerator()

        let baseCtor = baseType.GetConstructor(BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Instance, Type.DefaultBinder, [||], null)
        il.Emit(OpCodes.Ldarg_0)
        il.Emit(OpCodes.Call, baseCtor)     


        let mutable index = 1
        for (name, field,_,_, _) in fields do

            il.Emit(OpCodes.Ldarg_0)
            il.Emit(OpCodes.Ldarg, index)
            il.Emit(OpCodes.Stfld, field)
            

            index <- index + 1

        il.Emit(OpCodes.Ret)

        let t = bType.CreateTypeInfo()

        let delegates =
            fields |> List.map (fun (name, f,_,_, code) ->
                Assembler.assembleDelegateInternal f.FieldType (CodeGen.run code) :> obj
            )

        let ctorArgs = fields |> List.map (fun (_,f,_,_,_) -> f.FieldType) |> List.toArray
        let ctor = t.GetConstructor ctorArgs
        ctor.Invoke(List.toArray delegates)

    let implement<'a> (methods : list<Type * string * list<Type> * CodeGen<unit>>) : 'a =
        let t = typeof<'a>
        if t.IsInterface then implementInterface t methods |> unbox<'a>
        else implementSubType t methods |> unbox<'a>


    let private funcType (args : list<Type>) (ret : Type) = 
        match args with
            | [] -> failwithf "[TypeBuilder] not a function-type: %A" ret
            | [x] -> typedefof<FSharpFunc<_,_>>.MakeGenericType [|x; ret|]
            | args ->
                let args = List.append args [ret] |> List.toArray
                let name = typedefof<Microsoft.FSharp.Core.OptimizedClosures.FSharpFunc<_,_,_>>.AssemblyQualifiedName.Replace("`3", "`" + string args.Length)
                let tgen = Type.GetType(name)
                tgen.MakeGenericType args

    open Microsoft.FSharp.Core.OptimizedClosures
    type private Pap =
        static member Pap(f : FSharpFunc<'a, 'b, 'c>, a : 'a) = fun b -> f.Invoke(a, b)
        static member Pap(f : FSharpFunc<'a, 'b, 'c, 'd>, a : 'a) = fun b c -> f.Invoke(a, b, c)
        static member Pap(f : FSharpFunc<'a, 'b, 'c, 'd, 'e>, a : 'a) = fun b c d -> f.Invoke(a, b, c, d)
        static member Pap(f : FSharpFunc<'a, 'b, 'c, 'd, 'e, 'f>, a : 'a) = fun b c d e -> f.Invoke(a, b, c, d, e)


    let func<'a> (code : CodeGen<unit>) : 'a =
        let args, ret = DelegateAdapters.getFunctionSignature typeof<'a>
        let fType = funcType args ret

        implementSubType fType [
            yield ret, "Invoke", args, code

            match args with
                | a::_::_ ->
                    let targs = List.toArray (List.append args [ret])
                    let paps = typeof<Pap>.GetMethods(BindingFlags.NonPublic ||| BindingFlags.Static)
                    let pap =
                        paps |> Array.find(fun m ->
                            m.Name = "Pap" && m.GetGenericArguments().Length = targs.Length
                        )

                    yield 
                        pap.ReturnType, "Invoke", [a],
                        codegen {
                            
                            do! IL.ldarg 0
                            do! IL.ldarg 1

                            let paps = typeof<Pap>.GetMethods(BindingFlags.NonPublic ||| BindingFlags.Static)
                            let pap =
                                paps |> Array.find(fun m ->
                                    m.Name = "Pap" && m.GetGenericArguments().Length = targs.Length
                                )

                            let pap = pap.MakeGenericMethod targs
                            do! IL.call pap
                            do! IL.ret
                        }
                | _ -> ()

        ] |> unbox<'a>

module TypeBuilder =
    open System.Collections
    open System.Collections.Generic

    type TypeBuilderState =
        {
            builder : TypeBuilder
            baseType : Type
            delegates : HashMap<FieldBuilder, Type -> Delegate>
        }

    type TypeBuilder<'a> = TypeBuilderState -> TypeBuilderState * 'a

    [<AutoOpen>]
    module NewType =

        module DelegateTypes =
            type FuncVal<'a> = delegate of unit -> 'a
            type FuncVal<'a, 'b> = delegate of 'a -> 'b
            type FuncRef<'a, 'b> = delegate of byref<'a> -> 'b


        let private getDelegateType (args : list<Type>) (ret : Type) =
            let arr = System.Collections.Generic.List()
            arr.AddRange args
            arr.Add ret
            System.Linq.Expressions.Expression.GetDelegateType(arr.ToArray())



        type Type with
            member x.GetMethod(name : string, args : Type[], ret : Type) =
                let mi = x.GetMethod(name, BindingFlags.All, Type.DefaultBinder, args, null)
                if isNull mi || mi.ReturnType <> ret then
                    let baseMeth =
                        if isNull x.BaseType then null
                        else x.BaseType.GetMethod(name, args, ret)

                    if isNull baseMeth then
                        let iface = x.GetInterfaces()
                        let inner = 
                            iface |> Seq.tryPick (fun i ->
                                let m = i.GetMethod(name, args, ret)
                                if isNull m then None
                                else Some m
                            )
                        match inner with
                            | Some m -> m
                            | None -> null
                    else
                        baseMeth
                else
                    mi

            member x.GetMethod(mb : MethodBuilder) =
                let ret = mb.ReturnType
                let args = mb.GetParameters() |> Array.map (fun p -> p.ParameterType)
                x.GetMethod(mb.Name, args, ret)


        let inh (t : Type) =
            fun s ->
                s.builder.SetParent t
                s, ()

        let fld (name : string) (t : Type) =
            fun (b : TypeBuilderState) ->
                let fld = b.builder.DefineField(name, t, FieldAttributes.Public)
                b, fld

        let sfld (name : string) (t : Type) =
            fun (b : TypeBuilderState) ->
                let fld = b.builder.DefineField(name, t, FieldAttributes.Public ||| FieldAttributes.Static)
                b, fld

        let private recmemint (r : Type) (n : string) (a : list<Type>) (body : MethodInfo -> CodeGen<unit>) : TypeBuilder<MethodBuilder> =
            fun (b : TypeBuilderState) ->
                let mutable b = b
                let args = b.baseType::a
                let fieldType = 
                    getDelegateType args r
//                    if r = typeof<System.Void> then
//                        let actionType = Type.GetType(typedefof<Action<_>>.FullName.Replace("1", string args.Length))
//                        actionType.MakeGenericType args
//                    else
//                        let funcType = Type.GetType(typedefof<Func<_>>.FullName.Replace("1", string (1 + args.Length)))
//                        funcType.MakeGenericType (Array.append args [|r|])



                let fieldName = "s_" + n

                let field = 
                    b.builder.DefineField(
                        fieldName, fieldType, 
                        FieldAttributes.Static ||| FieldAttributes.Private ||| FieldAttributes.InitOnly
                    )


                let args = List.toArray a
                let meth = 
                    b.builder.DefineMethod(
                        n, MethodAttributes.Public ||| MethodAttributes.Virtual,
                        CallingConventions.Standard,
                        r,
                        args
                    )

                let il = meth.GetILGenerator()

                il.Emit(OpCodes.Ldsfld, field)
                il.Emit(OpCodes.Ldarg_0)
                for i in 0..args.Length-1 do
                    il.Emit(OpCodes.Ldarg, i + 1)
                il.EmitCall(OpCodes.Callvirt, fieldType.GetMethod "Invoke", null)
                il.Emit(OpCodes.Ret)

                let baseMeth = b.builder.BaseType.GetMethod(n, args, r)

                if not (isNull baseMeth) then
                    b.builder.DefineMethodOverride(meth, baseMeth)

                let code (t : Type) = 
                    let mi = t.GetMethod(meth)
                    let code =
                        body mi
                            |> CodeGen.run
                            |> List.map (fun i ->
                                match i with
                                    | Call mi ->
                                        match mi with
                                            | :? MethodBuilder as mb ->
                                                let mi = t.GetMethod(mb)
                                                Call mi
                                            | _ -> i

                                    | Ldfld fb ->
                                        match fb with
                                            | :? FieldBuilder as fb ->
                                                let f = t.GetField(fb.Name, BindingFlags.All ||| BindingFlags.Static)
                                                Ldfld f
                                            | _ -> i
                                    | i -> i
                            )

                    code |> Assembler.assembleDelegateInternal fieldType

                { b with delegates = HashMap.add field code b.delegates }, meth

        let recmem (r : Type) (n : string) (a : list<Type>) (body : MethodInfo -> CodeGen<unit>) =
            fun s -> 
                let (s,_) = recmemint r n a body s
                s, ()
        
        let mem (r : Type) (n : string) (a : list<Type>) (body : CodeGen<unit>) =
            fun s -> 
                let (s,_) = recmemint r n a (fun _ -> body) s
                s, ()

        let iface (iface : Type) (impl : list<Type * string * list<Type> * CodeGen<unit>>) : TypeBuilder<unit> =
            fun (b : TypeBuilderState) ->
                let mutable b = b
                b.builder.AddInterfaceImplementation(iface)

                for r,n,a,body in impl do
                    
                    let s,meth = recmemint r n a (fun _ -> body) b

                    let prototype = iface.GetMethod(n,List.toArray a,r)
                    if not (isNull prototype) then
                        b.builder.DefineMethodOverride(meth, prototype)
                    
                    b <- s

                b, ()


        let ctor (args : list<Type>) (body : CodeGen<unit>) : TypeBuilder<unit> =
            fun (b : TypeBuilderState) ->
                
                let ctor = 
                    b.builder.DefineConstructor(
                        MethodAttributes.Public,
                        CallingConventions.Standard,
                        List.toArray args
                    )

                let il = ctor.GetILGenerator()

                body 
                    |> CodeGen.run 
                    |> Assembler.assembleTo il


                b,()


        type NewTypeBuilder() =
            let bAss =
                AssemblyBuilder.DefineDynamicAssembly(
                    AssemblyName "asdsads",
                    AssemblyBuilderAccess.Run
                )

            let bMod = bAss.DefineDynamicModule("MainModule")

            member x.Zero() : TypeBuilder<unit> = fun s -> s,()

            member x.Yield(u : unit) = x.Zero()

            member x.For(m : seq<'a>, f : 'a -> TypeBuilder<unit>) : TypeBuilder<unit> =
                fun (b : TypeBuilderState) ->
                    let mutable c = b
                    for e in m do
                        let (s,()) = f e c
                        c <- s
                    c, ()

            member x.Combine(l : TypeBuilder<unit>, r : TypeBuilder<'a>) =
                fun b ->
                    let (b,()) = l b
                    r b

            member x.Delay(f : unit -> TypeBuilder<'a>) =
                fun b -> f () b


            member x.Bind (m : TypeBuilder<'a>, f : 'a -> TypeBuilder<'b>) : TypeBuilder<'b> =
                fun (b : TypeBuilderState) ->
                    let (b, v) = m b
                    (f v) b

            member x.Return v =
                fun (b : TypeBuilderState) -> b,v

            member x.Run(b : TypeBuilderState -> TypeBuilderState * unit) =
                let bType = bMod.DefineType(Guid.NewGuid() |> string)
                let (s, ()) = b { baseType = typeof<obj>; builder = bType; delegates = HashMap.empty }

                let t = s.builder.CreateTypeInfo()

                for (v,k) in HashMap.toSeq s.delegates do
                    let f = t.GetField(v.Name, BindingFlags.Static ||| BindingFlags.NonPublic)
                    f.SetValue(null, k t)

                t :> Type

        let newtype = NewTypeBuilder()

#nowarn "9"
#nowarn "51"

module Serializer =
    
    [<CustomEquality; NoComparison>]
    type R<'a when 'a : not struct> =
        struct
            val mutable public Value : 'a

            override x.GetHashCode() = System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(x.Value)
            override x.Equals o =
                match o with
                    | :? R<'a> as o -> Object.ReferenceEquals(x.Value, o.Value)
                    | _ -> false

            new(v) = { Value = v }
        end

    type RefMap<'a, 'b when 'a : not struct> = private { store : HashMap<R<'a>, 'b> }

    module RefMap =
        type private EmptyImpl<'a, 'b when 'a : not struct>() =
            static let instance = { store = HashMap.empty<R<'a>, 'b> }
            static member Instance = instance

        let empty<'a, 'b when 'a : not struct> = EmptyImpl<'a, 'b>.Instance

        let add (key : 'a) (value : 'b) (m : RefMap<'a, 'b>) =
            { store = HashMap.add (R key) value m.store }

        let remove (key : 'a) (m : RefMap<'a, 'b>) =
            { store = HashMap.remove (R key) m.store }

        let containsKey (key : 'a) (m : RefMap<'a, 'b>) =
            HashMap.containsKey (R key) m.store

        let update (key : 'a) (f : Option<'b> -> 'b) (m : RefMap<'a, 'b>) =
            let key = R key
            { store = HashMap.update key f m.store }

        let tryFind (key : 'a) (m : RefMap<'a, 'b>) =
            HashMap.tryFind (R key) m.store

    type IStore =
        abstract member TryLoad     : Guid * byref<'a> -> bool
        abstract member TryStore    : 'a * byref<Guid> -> bool

    type NopStore private () =
        static let instance = NopStore() :> IStore
        static member Instance = instance
        interface IStore with
            member x.TryLoad(_,_) = false
            member x.TryStore(_,_) = false


    type CoderState =
        {
            debug           : bool
            isReading       : bool
            externalStore   : IStore
            references      : RefMap<obj, Guid>
            values          : Map<Guid, obj>
            namestack       : list<string>
        }

    type Coder<'a> = { code : CoderState -> CoderState * 'a }


    type CoderBuilder() =
        member x.Bind(m : Coder<'a>, f : 'a -> Coder<'b>) =
            { code = fun s ->
                let (s,v) = m.code s
                (f v).code s
            }
        
        member x.Return(v : 'a) =
            { code = fun s -> s, v }

        member x.ReturnFrom(c : Coder<'a>) =
            c

        member x.Zero() =
            { code = fun s -> s, () }

        member x.Delay(f : unit -> Coder<'a>) =
            { code = fun s -> f().code s }

        member x.Combine (l : Coder<unit>, r : Coder<'a>) =
            { code = fun s ->
                let (s,()) = l.code s
                r.code s
            }

        member x.For(elements : seq<'a>, f : 'a -> Coder<unit>) =
            { code = fun s ->
                let mutable c = s
                for e in elements do
                    let (s, ()) = (f e).code c
                    c <- s
                c, ()
            }

        member x.While(guard : unit -> bool, body : Coder<unit>) =
            { code = fun s ->
                let mutable c = s
                
                while guard() do
                    let (s,()) = body.code c
                    c <- s
                    
                c, ()    
                
            }

    let code = CoderBuilder()



    type IReader =
        abstract member ReadPrimitive<'a when 'a : unmanaged> : unit -> 'a
        abstract member ReadBool : unit -> bool
        abstract member ReadString : unit -> string

    type IWriter =
        abstract member WritePrimitive<'a when 'a : unmanaged> : 'a -> unit
        abstract member WriteBool : bool -> unit
        abstract member WriteString : string -> unit

    type IValueCoder =
        abstract member WriteAny : IWriter * 'b -> Coder<unit>
        abstract member ReadAny : IReader -> Coder<'b>

    type IValueCoder<'a> =
        abstract member Write : IWriter * 'a -> Coder<unit>
        abstract member Read : IReader -> Coder<'a>

    module Coder =
        let map (f : 'a -> 'b) (m : Coder<'a>) = 
            { code = fun s ->
                let (s,v) = m.code s
                s, f v
            }

        let state = { code = fun s -> s,s }
        let put (s : CoderState) = { code = fun _ -> s,() }
        let modify (f : CoderState -> CoderState) = { code = fun s -> f s,() }
        let custom (f : CoderState -> CoderState * 'a) = { code = f }

        let tryStore (value : 'a) =
            { code = fun s ->
                if isNull (value :> obj) then
                    s, (false, Guid.Empty)
                else
                    let mutable id = Guid.Empty
                    if s.externalStore.TryStore(value, &id) then
                        s, (false, id)
                    else
                        let v = value :> obj
                        match RefMap.tryFind v s.references with
                            | Some r -> 
                                s, (false, r)
                            | None ->
                                let id = Guid.NewGuid()
                                let s = { s with references = RefMap.add v id s.references }
                                s, (true, id)
            }


        let tryLoad (id : Guid) : Coder<Option<'a>> =
            { code = fun s ->
                if id = Guid.Empty then
                    s, Some (Unchecked.defaultof<'a>)
                else
                    let mutable value = Unchecked.defaultof<'a>
                    if s.externalStore.TryLoad(id, &value) then
                        s, Some value
                    else
                        match Map.tryFind id s.values with
                            | Some (:? 'a as v) -> 
                                s, Some v
                            | _ ->
                                s, None
            } 

        let tryStoreLocal (value : 'a) =
            { code = fun s ->
                if isNull (value :> obj) then
                    s, (false, Guid.Empty)
                else
                    let v = value :> obj
                    match RefMap.tryFind v s.references with
                        | Some r -> 
                            s, (false, r)
                        | None ->
                            let id = Guid.NewGuid()
                            let s = { s with references = RefMap.add v id s.references }
                            s, (true, id)
            }

        let tryLoadLocal (id : Guid) : Coder<Option<'a>> =
            { code = fun s ->
                if id = Guid.Empty then
                    s, Some (Unchecked.defaultof<'a>)
                else
                    match Map.tryFind id s.values with
                        | Some (:? 'a as v) -> 
                            s, Some v
                        | _ ->
                            s, None
            } 


        let storeValue (id : Guid) (v : 'a) =
            { code = fun s ->
                if id = Guid.Empty then s, ()
                else { s with values = Map.add id (v :> obj) s.values }, ()
            }


        let pushName n = 
            { code = fun s -> 
                if s.debug then Log.start "%s" n
                { s with namestack = n::s.namestack }, () 
            }

        let popName = 
            { code = fun s -> 
                if s.debug then Log.stop()
                { s with namestack = List.tail s.namestack }, () 
            }



    



    [<AttributeUsage(AttributeTargets.Field ||| AttributeTargets.Property)>]
    type StoreExternAttribute() = inherit Attribute()

    [<AutoOpen>]
    module TypePropertyExtensions =
        open System.Runtime.InteropServices
        open System.Collections.Concurrent

        type private TypePropertyImpl<'a> private() =
            static let isBlittable =
                if typeof<'a>.IsValueType then
                    try
                        let gc = GCHandle.Alloc(Unchecked.defaultof<'a>, GCHandleType.Pinned)
                        gc.Free()
                        true
                    with _ ->
                        false
                else
                    false

            static member IsBlittable = isBlittable

        let blittable<'a> = TypePropertyImpl<'a>.IsBlittable

        let private blittableCache = ConcurrentDictionary<Type, bool>()

        type Type with
            member x.IsBlittable =
                blittableCache.GetOrAdd(x, fun x ->
                    let tb = typedefof<TypePropertyImpl<_>>.MakeGenericType [|x|]
                    let prop = tb.GetProperty("IsBlittable", BindingFlags.Static ||| BindingFlags.Public ||| BindingFlags.NonPublic)
                    prop.GetValue(null) |> unbox<bool>
                )


    module ValueCoders = 
        open TypeBuilder
        open Microsoft.FSharp.Reflection

        [<AbstractClass>]
        type AbstractValueCoder<'a>() =
            abstract member Read : IReader -> Coder<'a>
            abstract member Write : IWriter * 'a -> Coder<unit>


            interface IValueCoder with
                member x.ReadAny r = 
                    x.Read r |> Coder.map unbox

                member x.WriteAny(w,v) = 
                    x.Write(w,unbox v)

            interface IValueCoder<'a> with
                member x.Read r = x.Read r
                member x.Write(w,v) = x.Write(w,v)

        type BoolCoder() =
            inherit AbstractValueCoder<bool>()
            override x.Read (r : IReader) = code { return r.ReadBool() }
            override x.Write(w,v) = code { w.WriteBool(v) }

        type StringCoder() =
            inherit AbstractValueCoder<string>()
            override x.Read (r : IReader) = code { return r.ReadString() }
            override x.Write(w,v) = code { w.WriteString(v) }

        type PrimitiveValueCoder<'a when 'a : unmanaged>() =
            inherit AbstractValueCoder<'a>()

            override x.Read (r : IReader) = code { return r.ReadPrimitive() }
            override x.Write(w,v) = code { w.WritePrimitive(v) }


        [<AbstractClass>]
        type ReferenceCoder<'a>() =
            inherit AbstractValueCoder<'a>()

            abstract member CreateEmptyInstance : unit -> 'a
            abstract member ReadRef : IReader * 'a -> Coder<unit>
            abstract member WriteRef : IWriter * 'a -> Coder<unit>

            default x.CreateEmptyInstance() =
#if NET8_0_OR_GREATER
                System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof<'a>) |> unbox<'a>
#else
                System.Runtime.Serialization.FormatterServices.GetSafeUninitializedObject(typeof<'a>) |> unbox<'a>
#endif

            override x.Read(r : IReader) =
                code {
                    let id = r.ReadPrimitive()
                    let! res = Coder.tryLoad id 
                    match res with
                        | Some v -> return v
                        | None ->
                            let i = x.CreateEmptyInstance()
                            do! Coder.storeValue id i
                            do! x.ReadRef(r, i)
                            return i
                }

            override x.Write(w : IWriter, value : 'a) =
                code {
                    let! (isNew, id) = Coder.tryStore value
                    w.WritePrimitive id

                    if isNew then
                        do! x.WriteRef(w, value)

                }


        type TypeCoder() =
            inherit AbstractValueCoder<Type>()
            override x.Read (r : IReader) = 
                code { 
                    let name = r.ReadString() 
                    return Type.GetType name
                }

            override x.Write(w,v) = 
                code { 
                    w.WriteString(v.AssemblyQualifiedName)
                }

        type PrimitiveArrayCoder<'a when 'a : unmanaged>() =
            inherit AbstractValueCoder<'a[]>()

            override x.Read (r : IReader) =
                let read () =
                    let cnt = r.ReadPrimitive()
                    let arr = Array.zeroCreate cnt
                    for i in 0..arr.Length-1 do
                        arr.[i] <- r.ReadPrimitive()
                    arr
                code { return read() }

            override x.Write(w,v) =
                let write() =
                    w.WritePrimitive v.Length
                    for e in v do w.WritePrimitive e

                code { write() }

        type ExternArrayCoder<'a>(resolve : Type * FieldInfo -> IValueCoder) =
            inherit AbstractValueCoder<'a[]>()

            let valueCoder = resolve (typeof<'a>, null) |> unbox<IValueCoder<'a>>

            override x.Read (r : IReader) = 
                code {
                    
                    let id = r.ReadPrimitive()
                    let! v = Coder.tryLoad id

                    match v with
                        | Some v -> return v
                        | _ ->
                            do! Coder.pushName "Length"
                            let cnt = r.ReadPrimitive()
                            do! Coder.popName

                            let arr = Array.zeroCreate cnt
                            do! Coder.storeValue id arr
                            do! Coder.pushName "Data"
                            for i in 0..arr.Length - 1 do
                                let! v = valueCoder.Read(r)
                                arr.[i] <- v
                            do! Coder.popName

                            return arr
                }

            override x.Write(w,v) =
                code {
                    let! (isNew, id) = Coder.tryStore v
                    w.WritePrimitive id

                    if isNew then
                        do! Coder.pushName "Length"
                        w.WritePrimitive v.Length
                        do! Coder.popName
                        
                        do! Coder.pushName "Data"
                        for e in v do
                            do! valueCoder.Write(w, e)
                        do! Coder.popName

                }

        type InlineArrayCoder<'a>(resolve : Type * FieldInfo -> IValueCoder) =
            inherit AbstractValueCoder<'a[]>()

            let valueCoder = resolve (typeof<'a>, null) |> unbox<IValueCoder<'a>>

            override x.Read (r : IReader) = 
                code {
                    do! Coder.pushName "Length"
                    let cnt = r.ReadPrimitive()
                    let arr = Array.zeroCreate cnt
                    do! Coder.popName

                    
                    do! Coder.pushName "Data"
                    for i in 0..arr.Length - 1 do
                        let! v = valueCoder.Read(r)
                        arr.[i] <- v
                    do! Coder.popName

                    return arr
                }

            override x.Write(w,v) =
                code {
                    
                    do! Coder.pushName "Length"
                    w.WritePrimitive v.Length
                    do! Coder.popName
                    
                    do! Coder.pushName "Data"
                    for e in v do
                        do! valueCoder.Write(w, e)
                    do! Coder.popName

                }

        type private Marker =
            class
                val mutable public DynamicValue : obj
            end

        let dynValue = typeof<Marker>.GetField("DynamicValue", BindingFlags.All)

        type DynamicCoder<'a>(resolve : Type * FieldInfo -> IValueCoder) =
            inherit AbstractValueCoder<'a>()

            let typeCoder = resolve (typeof<Type>, null) |> unbox<IValueCoder<Type>>
            

            override x.Read(r) =
                code {
                    do! Coder.pushName "Type"
                    let! t = typeCoder.Read r
                    do! Coder.popName 
                    if isNull t then
                        return Unchecked.defaultof<'a>
                    else
                        do! Coder.pushName "Value"
                        let coder = resolve(t.MakeByRefType(), null)
                        let! res = coder.ReadAny(r)
                        do! Coder.popName
                        return res
                }

            override x.Write(w,v) =
                code {
                    do! Coder.pushName "Type"
                    if isNull (v :> obj) then
                        do! typeCoder.Write(w, null)
                        do! Coder.popName 
                    else
                        let t = v.GetType()
                        do! typeCoder.Write(w, t)
                        do! Coder.popName 

                        do! Coder.pushName "Value"
                        let coder = resolve(t.MakeByRefType(), null)
                        do! coder.WriteAny(w,v)
                        do! Coder.popName
                }
            


        module AutoCoder = 
            type Helpers() =
                static member NewObj() : 'a =
#if NET8_0_OR_GREATER
                    System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof<'a>) |> unbox<'a>
#else
                    System.Runtime.Serialization.FormatterServices.GetSafeUninitializedObject(typeof<'a>) |> unbox<'a>
#endif

            [<AbstractClass>]
            type AbstractStateCoder<'a>() =
                inherit AbstractValueCoder<'a>()

                override x.Read r =
                    { code = fun s ->
                        let s = ref s
                        let v = x.ReadState(r, s)
                        !s, v
                    }

                override x.Write(w,v) =
                    { code = fun s ->
                        let s = ref s
                        x.WriteState(w, v, s)
                        !s, ()
                    }

                abstract member ReadState : IReader * byref<CoderState> -> 'a
                abstract member WriteState : IWriter * 'a * byref<CoderState> -> unit

            [<AbstractClass>]
            type AbstractStateReferenceCoder<'a when 'a : not struct>() =
                inherit ReferenceCoder<'a>()

                override x.ReadRef(r,v) =
                    { code = fun s ->
                        let s = ref s
                        let v = x.ReadState(r, v, s)
                        !s, ()
                    }

                override x.WriteRef(w,v) =
                    { code = fun s ->
                        let s = ref s
                        x.WriteState(w, v, s)
                        !s, ()
                    }

                abstract member ReadState : IReader * 'a * byref<CoderState> -> unit
                abstract member WriteState : IWriter * 'a * byref<CoderState> -> unit


            let private invoke = typeof<Type * FieldInfo -> IValueCoder>.GetMethod("Invoke", [| typeof<Type * FieldInfo> |])


            let createFieldCoder (t : Type) =
                let fields = t.GetFields(BindingFlags.NonPublic ||| BindingFlags.Public ||| BindingFlags.Instance)

                let baseType = typedefof<AbstractStateCoder<_>>.MakeGenericType [|t|]
                let resType = typedefof<Coder<_>>.MakeGenericType [|t|]
                let baseCtor = baseType.GetConstructor [||]
            
                let unitCoderType = typeof<Coder<unit>>

                let types = fields |> Array.map (fun f -> f)

                let newInstance = typeof<Helpers>.GetMethod("NewObj").MakeGenericMethod [|t|]

                let coderFields =
                    fields |> Array.map (fun fi ->
                        let fieldType = typedefof<IValueCoder<_>>.MakeGenericType [|fi.FieldType|]

                        fieldType, fi.Name, fi
                    )

                let stateType = typeof<CoderState>.MakeByRefType()
                //let refContents = typeof<ref<SerializerState>>.GetField("contents@")

                newtype {
                    do! inh baseType

                    let cf = Array.zeroCreate coderFields.Length
                    for i in 0..coderFields.Length-1 do
                        let (t,n,f) = coderFields.[i]
                        let! h = fld n t
                        cf.[i] <- (h :> FieldInfo, f)
                        ()

                    do! mem typeof<System.Void> "WriteState" [typeof<IWriter>; t; stateType] (
                            codegen {
                                for (coderField, field) in cf do
                                    let tupType = typeof<CoderState *unit>
                                    let codeType = typeof<CoderState -> CoderState *unit>

                                    do! IL.ldarg 0
                                    do! IL.ldfld coderField

                                    do! IL.ldarg 1

                                    do! IL.ldarg 2
                                    do! IL.ldfld field

                                    do! IL.call (coderField.FieldType.GetMethod "Write")
                                    do! IL.call (unitCoderType.GetProperty("code").GetMethod)


                                    let! tup = IL.newlocal tupType
                                    do! IL.ldarg 3
                                    do! IL.ldind ValueType.Object
                                    do! IL.call (codeType.GetMethod "Invoke")
                                    do! IL.stloc tup

                                    do! IL.ldarg 3
                                    do! IL.ldloc tup
                                    do! IL.call (tupType.GetProperty("Item1").GetMethod)
                                    do! IL.stind ValueType.Object


                                do! IL.ret
                            }
                        )

                    do! mem t "ReadState" [typeof<IReader>; stateType] (
                            codegen {
                                let! res = IL.newlocal t
                                do! IL.call newInstance
                                do! IL.stloc res


                                for (coderField, field) in cf do
                                    let resType = typedefof<Coder<_>>.MakeGenericType [|field.FieldType|]
                                    let tupType = FSharpType.MakeTupleType [|typeof<CoderState>; field.FieldType|]
                                    let codeType = FSharpType.MakeFunctionType(typeof<CoderState>, tupType)

                                    do! IL.ldarg 0
                                    do! IL.ldfld coderField

                                    do! IL.ldarg 1
                                    do! IL.call (coderField.FieldType.GetMethod "Read")
                                    do! IL.call (resType.GetProperty("code").GetMethod)


                                    let! tup = IL.newlocal tupType
                                    do! IL.ldarg 2
                                    do! IL.ldind ValueType.Object
                                    do! IL.call (codeType.GetMethod "Invoke")
                                    do! IL.stloc tup

                                    do! IL.ldarg 2
                                    do! IL.ldloc tup
                                    do! IL.call (tupType.GetProperty("Item1").GetMethod)
                                    do! IL.stind ValueType.Object


                                    do! IL.ldloc res
                                    do! IL.ldloc tup
                                    do! IL.call (tupType.GetProperty("Item2").GetMethod)
                                    do! IL.stfld field


                                do! IL.ldloc res
                                do! IL.ret
                            }
                        )

                    do! ctor [typeof<Type * FieldInfo -> IValueCoder> ] (
                            codegen {
                                do! IL.ldarg 0
                                do! IL.call baseCtor

                                for (coder, f) in cf do
                                    do! IL.ldarg 0
                                
                                    do! IL.ldarg 1
                                    do! IL.ldtoken f.FieldType
                                    do! IL.call <@ Type.GetTypeFromHandle @>

                                    do! IL.ldtoken f
                                    do! IL.ldtoken f.DeclaringType
                                    do! IL.call <@ FieldInfo.GetFieldFromHandle : _ * _ -> _ @>

                                    do! IL.newobj (typeof<Type * FieldInfo>.GetConstructor [|typeof<Type>; typeof<FieldInfo>|])
                                    do! IL.call invoke

                                    do! IL.stfld coder


                                do! IL.ret
                            }
                        )

                }

            let createUnionCoder (t : Type) =
                let baseType = typedefof<AbstractStateCoder<_>>.MakeGenericType [|t|]

                let cases = FSharpType.GetUnionCases (t, true) |> Array.sortBy (fun c -> c.Tag)
                let stateType = typeof<CoderState>.MakeByRefType()
                let tag = 
                    let prop = t.GetProperty("Tag", BindingFlags.All)
                    if isNull prop then null
                    else prop.GetMethod

                let resType = typedefof<Coder<_>>.MakeGenericType [|t|]
                let baseCtor = baseType.GetConstructor [||]
            
                let unitCoderType = typeof<Coder<unit>>

                let caseTypes = 
                    cases |> Array.map (fun ci ->
                        let caseType = t.GetNestedType(ci.Name,BindingFlags.NonPublic ||| BindingFlags.Public)
                        ci.Tag, ci
                    )

                let innerCoders =
                    caseTypes 
                        |> Seq.collect (fun (_,ci) -> ci.GetFields() )
                        |> Seq.map (fun fi -> fi.PropertyType)
                        |> HashSet.ofSeq
                        |> Seq.toArray

                let coderFields =
                    innerCoders
                        |> Array.mapi (fun i ft ->
                            let name = sprintf "m_coder%d" i
                            let t = typedefof<IValueCoder<_>>.MakeGenericType [|ft|]
                            name, t, ft
                        )

                let coderFieldIndex =
                    innerCoders 
                        |> Array.mapi (fun i t -> t,i)
                        |> Dictionary.ofArray

                newtype {
                    do! inh baseType
                    
                    let cf = Array.zeroCreate coderFields.Length
                    for i in 0..coderFields.Length-1 do
                        let (n,t,valueType) = coderFields.[i]
                        let! h = fld n t
                        cf.[i] <- valueType, h :> FieldInfo
                        ()

                    let getCf (t : Type) =
                        cf.[coderFieldIndex.[t]] |> snd


                    do! mem typeof<System.Void> "WriteState" [typeof<IWriter>; t; stateType] (
                            codegen {
                                do! IL.ldarg 2
                                do! IL.call tag

                                let labels = Array.zeroCreate cases.Length
                                for i in 0..cases.Length-1 do
                                    let! li = IL.newlabel 
                                    labels.[i] <- li



                                do! IL.Switch labels
                                do! IL.ret

                                for c in cases do
                                    do! IL.mark labels.[c.Tag]

                                    do! IL.ldarg 1
                                    do! IL.ldconst c.Tag
                                    do! IL.call <@ fun (w : IWriter) -> w.WritePrimitive(0) @>

                                    let fields = c.GetFields()

                                    for f in fields do
                                        let tupType = typeof<CoderState * unit>
                                        let codeType = typeof<CoderState -> CoderState * unit>

                                        let cf = getCf f.PropertyType
                                        do! IL.ldarg 0
                                        do! IL.ldfld cf

                                        do! IL.ldarg 1

                                        do! IL.ldarg 2
                                        do! IL.call f.GetMethod

                                        do! IL.call (cf.FieldType.GetMethod "Write")
                                        do! IL.call (unitCoderType.GetProperty("code").GetMethod)


                                        let! tup = IL.newlocal tupType
                                        do! IL.ldarg 3
                                        do! IL.ldind ValueType.Object
                                        do! IL.call (codeType.GetMethod "Invoke")
                                        do! IL.stloc tup

                                        do! IL.ldarg 3
                                        do! IL.ldloc tup
                                        do! IL.call (tupType.GetProperty("Item1").GetMethod)
                                        do! IL.stind ValueType.Object

                                    do! IL.ret


                                ()
                            }
                        )

                    do! mem t "ReadState" [typeof<IReader>; stateType] (
                            codegen {
                                do! IL.ldarg 1
                                do! IL.call <@ fun (r : IReader) -> r.ReadPrimitive() : int @>

                                let labels = Array.zeroCreate cases.Length
                                for i in 0..cases.Length-1 do
                                    let! li = IL.newlabel 
                                    labels.[i] <- li



                                do! IL.Switch labels
                                do! IL.ldnull
                                do! IL.ret

                                for c in cases do
                                    do! IL.mark labels.[c.Tag]
                                    // let caseType = t.GetNestedType(c.Name,BindingFlags.NonPublic ||| BindingFlags.Public)
                                    let ctor = 
                                        let m = c.DeclaringType.GetMethod(c.Name, BindingFlags.Static ||| BindingFlags.Public)
                                        if isNull m then
                                            let m = c.DeclaringType.GetMethod("New" + c.Name, BindingFlags.Static ||| BindingFlags.Public)
                                            if isNull m then
                                                let prop = c.DeclaringType.GetProperty(c.Name, BindingFlags.Static ||| BindingFlags.Public)
                                                if isNull prop then null
                                                else prop.GetMethod
                                            else
                                                m
                                        else
                                            m
                                            
                                    let fields = c.GetFields()

                                    let locals = Array.zeroCreate fields.Length

                                    for fi in 0..fields.Length-1 do
                                        let f = fields.[fi]
                                        let! loc = IL.newlocal f.PropertyType
                                        locals.[fi] <- loc

                                        let resType = typedefof<Coder<_>>.MakeGenericType [|f.PropertyType|]
                                        let tupType = FSharpType.MakeTupleType [|typeof<CoderState>; f.PropertyType|]
                                        let codeType = FSharpType.MakeFunctionType(typeof<CoderState>, tupType)
                                        
                                        let cf = getCf f.PropertyType

                                        do! IL.ldarg 0
                                        do! IL.ldfld cf

                                        do! IL.ldarg 1

                                        do! IL.call (cf.FieldType.GetMethod "Read")
                                        do! IL.call (resType.GetProperty("code").GetMethod)


                                        let! tup = IL.newlocal tupType
                                        do! IL.ldarg 2
                                        do! IL.ldind ValueType.Object
                                        do! IL.call (codeType.GetMethod "Invoke")
                                        do! IL.stloc tup

                                        do! IL.ldarg 2
                                        do! IL.ldloc tup
                                        do! IL.call (tupType.GetProperty("Item1").GetMethod)
                                        do! IL.stind ValueType.Object

                                        do! IL.ldloc tup
                                        do! IL.call (tupType.GetProperty("Item2").GetMethod)
                                        do! IL.stloc locals.[fi]


                                    if isNull ctor then 
                                        do! IL.ldnull
                                    else 
                                        for l in locals do
                                            do! IL.ldloc l
                                        
                                        do! IL.call ctor

                                    do! IL.ret


                            }
                        )


                    do! ctor [typeof<Type * FieldInfo -> IValueCoder> ] (
                            codegen {
                                do! IL.ldarg 0
                                do! IL.call baseCtor

                                for (t,cf) in cf do
                                    do! IL.ldarg 0
                                
                                    do! IL.ldarg 1
                                    do! IL.ldtoken t
                                    do! IL.call <@ Type.GetTypeFromHandle @>

                                    do! IL.ldnull

                                    do! IL.newobj (typeof<Type * FieldInfo>.GetConstructor [|typeof<Type>; typeof<FieldInfo>|])
                                    do! IL.call invoke

                                    do! IL.stfld cf


                                do! IL.ret
                            }
                        )
                }

            type OptionCoder<'a>(resolve : Type * FieldInfo -> IValueCoder) =
                inherit AbstractValueCoder<Option<'a>>()

                let valueCoder = resolve(typeof<'a>, null) |> unbox<IValueCoder<'a>>

                override x.Write(w,v) =
                    code {
                        match v with
                            | Some v -> do! valueCoder.Write(w,v)
                            | _ -> ()
                    }

                override x.Read(r) =
                    code {
                        let! v = valueCoder.Read(r)
                        return Some v
                    }

            let private unionCoderCache = System.Collections.Concurrent.ConcurrentDictionary<Type, Type>()

            let create (t : Type) =
                
                if FSharpType.IsUnion t then
                    if t.IsGenericType && t.GetGenericTypeDefinition() = typedefof<Option<_>> then
                        typedefof<OptionCoder<_>>.MakeGenericType (t.GetGenericArguments())
                    else
                        let tparent = 
                            if t.IsNested then t.DeclaringType
                            else t

                        let tparent = 
                            if tparent.IsGenericTypeDefinition then tparent.MakeGenericType (t.GetGenericArguments())
                            else tparent

                        unionCoderCache.GetOrAdd(tparent, createUnionCoder)
                else
                    createFieldCoder t

    module Coders =
        open ValueCoders
        open System.Collections.Concurrent

        let private cache = ConcurrentDictionary<Type * bool, IValueCoder>()
        let private fieldCache = ConcurrentDictionary<Type * FieldInfo, IValueCoder>()

        type private Resolver = Type * FieldInfo -> IValueCoder
        type private Creator<'a>() =
            static let create =
                let coderType = typeof<'a>
                let resolveCtor = coderType.GetConstructor [| typeof<Resolver> |]
                let emptyCtor = coderType.GetConstructor [||]

                if isNull resolveCtor then
                    if isNull emptyCtor then failwithf "[Resolve] cannot create coder of type %A" coderType
                    else 
                        let value = emptyCtor.Invoke [||] |> unbox<IValueCoder>
                        fun (r : Resolver) -> value
                else
                    let d = DynamicMethod(sprintf "Create%A" coderType, typeof<IValueCoder>, [| typeof<Resolver> |], true)
                    let il = d.GetILGenerator()

                    il.Emit(OpCodes.Ldarg_0)
                    il.Emit(OpCodes.Newobj, resolveCtor)
                    il.Emit(OpCodes.Castclass, typeof<IValueCoder>)
                    il.Emit(OpCodes.Ret)

                    let creator = 
                        d.CreateDelegate(typeof<Func<Resolver, IValueCoder>>)
                            |> unbox<Func<Resolver, IValueCoder>>


                    fun (r : Resolver) -> creator.Invoke(r)

            static member Create = create

        let private creatorCache = ConcurrentDictionary<Type, Resolver -> IValueCoder>()

        type FieldCoder<'a>(field : FieldInfo, inner : IValueCoder<'a>) =
            inherit AbstractValueCoder<'a>()

            override x.Write(w, v) =
                code {
                    do! Coder.pushName field.Name
                    let! res = inner.Write(w,v)
                    do! Coder.popName
                    return res
                }

             override x.Read(r) =
                code {
                    do! Coder.pushName field.Name
                    let! res = inner.Read(r)
                    do! Coder.popName
                    return res
                }               

        type LocalNonRecusiveRefCoder<'a>(inner : IValueCoder<'a>) =
            inherit AbstractValueCoder<'a>()

            override x.Write(w,v) =
                code {
                    let! (isNew,id) = Coder.tryStoreLocal v

                    do! Coder.pushName "ReferenceId"
                    w.WritePrimitive id
                    do! Coder.popName
                    
                    
                    if isNew then
                        do! Coder.pushName "ReferenceValue"
                        do! inner.Write(w,v)
                        do! Coder.popName

                }

            override x.Read(r) =
                code {
                    do! Coder.pushName "ReferenceId"
                    let id = r.ReadPrimitive()
                    do! Coder.popName

                    let! res = Coder.tryLoadLocal id 
                    match res with
                        | Some v -> return v
                        | None ->
                            do! Coder.pushName "ReferenceValue"
                            let! result = inner.Read(r)
                            do! Coder.storeValue id result
                            do! Coder.popName
                            return result
                }


        type Copy<'a> private() =
            static let copy =
                let fields = typeof<'a>.GetFields(BindingFlags.All)

                let copyAll =
                    DynamicMethod(
                        "CopyAll" + typeof<'a>.Name,
                        typeof<System.Void>,
                        [|typeof<'a>; typeof<'a>|],
                        typeof<'a>,
                        true
                    )

                let il = copyAll.GetILGenerator()

                for f in fields do
                    il.Emit(OpCodes.Ldarg_0)
                    il.Emit(OpCodes.Ldarg_1)
                    il.Emit(OpCodes.Ldfld, f)
                    il.Emit(OpCodes.Stfld, f)


                il.Emit(OpCodes.Ret)

                copyAll.CreateDelegate(typeof<Action<'a, 'a>>)
                    |> unbox<Action<'a, 'a>>

            static member Copy(target : 'a, source : 'a) =
                copy.Invoke(target, source)

        type LocalRefCoder<'a>(inner : IValueCoder<'a>) =
            inherit AbstractValueCoder<'a>()


            override x.Write(w,v) =
                code {
                    let! (isNew,id) = Coder.tryStoreLocal v

                    do! Coder.pushName "ReferenceId"
                    w.WritePrimitive id
                    do! Coder.popName
                    
                    
                    if isNew then
                        do! Coder.pushName "ReferenceValue"
                        do! inner.Write(w,v)
                        do! Coder.popName

                }

            override x.Read(r) =
                code {
                    do! Coder.pushName "ReferenceId"
                    let id = r.ReadPrimitive()
                    do! Coder.popName

                    let! res = Coder.tryLoadLocal id 
                    match res with
                        | Some v -> return v
                        | None ->
                            do! Coder.pushName "ReferenceValue"
#if NET8_0_OR_GREATER
                            let res = System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof<'a>) |> unbox<'a>
#else
                            let res = System.Runtime.Serialization.FormatterServices.GetSafeUninitializedObject typeof<'a> |> unbox<'a>
#endif
                            do! Coder.storeValue id res

                            let! result = inner.Read(r)
                            Copy<'a>.Copy(res, result)
                            

                            do! Coder.popName
                            return res
                }

        type ExternRefCoder<'a>(inner : IValueCoder<'a>) =
            inherit AbstractValueCoder<'a>()

            override x.Write(w,v) =
                code {
                    let! (isNew,id) = Coder.tryStore v

                    do! Coder.pushName "ReferenceId"
                    w.WritePrimitive id
                    do! Coder.popName
                    
                    
                    if isNew then
                        do! Coder.pushName "ReferenceValue"
                        do! inner.Write(w,v)
                        do! Coder.popName

                }

            override x.Read(r) =
                code {
                    do! Coder.pushName "ReferenceId"
                    let id = r.ReadPrimitive()
                    do! Coder.popName

                    let! res = Coder.tryLoad id 
                    match res with
                        | Some v -> return v
                        | None ->
                            do! Coder.pushName "ReferenceValue"
#if NET8_0_OR_GREATER
                            let res = System.Runtime.CompilerServices.RuntimeHelpers.GetUninitializedObject(typeof<'a>) |> unbox<'a>
#else
                            let res = System.Runtime.Serialization.FormatterServices.GetSafeUninitializedObject typeof<'a> |> unbox<'a>
#endif
                            do! Coder.storeValue id res

                            let! result = inner.Read(r)
                            Copy<'a>.Copy(res, result)
                            

                            do! Coder.popName
                            return res
                }


        let rec resolve (t : Type) (inlineStore : bool) : IValueCoder =
            let createCoder (coderType : Type) =
                let creator = 
                    creatorCache.GetOrAdd(coderType, Func<_,_>(fun t ->
                        let tc = typedefof<Creator<_>>.MakeGenericType [|t|]
                        tc.GetProperty("Create", BindingFlags.Static ||| BindingFlags.All).GetValue(null) |> unbox<Resolver -> IValueCoder>
                    ))
                creator (uncurry getField)


            let isGround, t = 
                if t.IsValueType then true, t
                elif t.IsByRef then true, t.GetElementType()
                elif Microsoft.FSharp.Reflection.FSharpType.IsUnion t then true, t
                else false, t

            let wrapRef (c : IValueCoder) =
                if t.IsValueType then 
                    c

                elif typeof<Type>.IsAssignableFrom t || t.IsArray  then
                    let decorator = typedefof<LocalNonRecusiveRefCoder<_>>.MakeGenericType [|t|]
                    Activator.CreateInstance(decorator, [|c :> obj|]) |> unbox<IValueCoder>
                    
                elif Microsoft.FSharp.Reflection.FSharpType.IsUnion t then
                    c

                else
                    if inlineStore then
                        let decorator = typedefof<LocalRefCoder<_>>.MakeGenericType [|t|]
                        Activator.CreateInstance(decorator, [|c :> obj|]) |> unbox<IValueCoder>
                    else
                        let decorator = typedefof<ExternRefCoder<_>>.MakeGenericType [|t|]
                        Activator.CreateInstance(decorator, [|c :> obj|]) |> unbox<IValueCoder>
                        
            let typeCoder =
                if t = typeof<bool> then
                    BoolCoder() :> IValueCoder

                elif t = typeof<string> then
                    StringCoder() :> IValueCoder

                elif typeof<Type>.IsAssignableFrom t then
                    TypeCoder() 
                        |> wrapRef

                elif t.IsBlittable then
                    typedefof<PrimitiveValueCoder<int>>.MakeGenericType [|t|]
                        |> createCoder

                elif t.IsArray then
                    let et = t.GetElementType()

                    if et.IsBlittable then 
                        typedefof<PrimitiveArrayCoder<int>>.MakeGenericType [|et|]
                            |> createCoder
                            |> wrapRef
                    else 
                        if inlineStore then 
                            typedefof<InlineArrayCoder<_>>.MakeGenericType [|et|]
                                |> createCoder
                                |> wrapRef
                        else 
                            typedefof<ExternArrayCoder<_>>.MakeGenericType [|et|]
                                |> createCoder

                else
                    if isGround then
                        AutoCoder.create t
                            |> createCoder
                            |> wrapRef
                    else
                        typedefof<DynamicCoder<_>>.MakeGenericType [|t|]
                            |> createCoder

            typeCoder

        and get (t : Type) (inlineStore : bool) : IValueCoder = 
            let isNew = ref false
            let res =
                cache.GetOrAdd((t, inlineStore), Func<_,_>(fun (a,b) ->
                    isNew := true
                    resolve a b
                ))

            res


        and getField (t : Type) (f : FieldInfo) : IValueCoder =
            let wrapField (c : IValueCoder) =
                if isNull f then 
                    c
                else
                    let decorator = typedefof<FieldCoder<_>>.MakeGenericType [|t|]
                    Activator.CreateInstance(decorator, [|f :> obj; c :> obj|]) |> unbox<IValueCoder>
            

            fieldCache.GetOrAdd((t, f), Func<_,_>(fun (t : Type ,f : FieldInfo) ->
                let inlineStore = isNull f || f.GetCustomAttributes<StoreExternAttribute>() |> Seq.isEmpty
                let inner = get t inlineStore
                wrapField inner
            ))



        type private Coder<'a> private() =
            static let res = lazy ( get typeof<'a> true |> unbox<IValueCoder<'a>> )
            static let ext = lazy ( get typeof<'a> false |> unbox<IValueCoder<'a>> )
            static member Intern = res.Value
            static member Extern = ext.Value

        let coder<'a> = Coder<'a>.Intern
        let externCoder<'a> = Coder<'a>.Extern
        
    
    [<AutoOpen>]
    module ReaderWriterExtensions =
        type IReader with
            member x.Read() : Coder<'a> =
                let c = Coders.coder<'a>
                c.Read(x)

            member x.ReadExtern() : Coder<'a> =
                let c = Coders.externCoder<'a>
                c.Read(x)

        type IWriter with
            member x.Write(v : 'a) : Coder<unit> =
                let c = Coders.coder<'a>
                c.Write(x, v)
                
            member x.WriteExtern(v : 'a) : Coder<unit> =
                let c = Coders.externCoder<'a>
                c.Write(x, v)

    open System.Runtime.CompilerServices
    [<AbstractClass; Sealed; Extension>]
    type IReaderWriterExtensions private()  =

        [<Extension>]
        static member ReadValue(this : IReader, store : IStore) : 'a =
            let (_,value) =
                this.Read().code {
                    debug = false
                    isReading = true 
                    externalStore = store
                    references = RefMap.empty
                    values = Map.empty
                    namestack = []
                }
            value
        
        [<Extension>]
        static member WriteValue(this : IWriter, store : IStore, value : 'a) : unit =
            this.Write(value).code {
                debug = false
                isReading = false 
                externalStore = store
                references = RefMap.empty
                values = Map.empty
                namestack = []
            } |> ignore


        [<Extension>]
        static member ReadValue(this : IReader) : 'a =
            let (_,value) =
                this.Read().code {
                    debug = false
                    isReading = true 
                    externalStore = NopStore.Instance
                    references = RefMap.empty
                    values = Map.empty
                    namestack = []
                }
            value
        
        [<Extension>]
        static member WriteValue(this : IWriter, value : 'a) : unit =
            this.Write(value).code {
                debug = false
                isReading = false 
                externalStore = NopStore.Instance
                references = RefMap.empty
                values = Map.empty
                namestack = []
            } |> ignore
           
        [<Extension>]
        static member ReadValueDebug(this : IReader) : 'a =
            let (_,value) =
                this.Read().code {
                    debug = true
                    isReading = true 
                    externalStore = NopStore.Instance
                    references = RefMap.empty
                    values = Map.empty
                    namestack = []
                }
            value
        
        [<Extension>]
        static member WriteValueDebug(this : IWriter, value : 'a) : unit =
            this.Write(value).code {
                debug = true
                isReading = false 
                externalStore = NopStore.Instance
                references = RefMap.empty
                values = Map.empty
                namestack = []
            } |> ignore
           




    type DummDb() =
        interface IStore with
            member x.TryStore(value : 'a, id : byref<Guid>) =
                Log.line "tryStore %A" value
                false

            member x.TryLoad(id : Guid, value : byref<'a>) =
                Log.line "tryLoad %A" value
                false

    type DummyWriter() =
        interface IWriter with
            member x.WritePrimitive(a) = Log.line "prim %A" a
            member x.WriteBool(a) = Log.line "bool %A" a
            member x.WriteString(a) = Log.line "str %A" a

    open System.IO
    open Microsoft.FSharp.NativeInterop
    open System.Runtime.InteropServices

    type BinaryWritingCoder(stream : Stream) =
        let w = new BinaryWriter(stream)

        interface IWriter with
            member x.WritePrimitive(a : 'a) =
                let mutable a = a
                let ptr : nativeptr<byte> = &&a |> NativePtr.cast
                let bytes = ptr |> NativePtr.toArray sizeof<'a>
                w.Write(bytes)

            member x.WriteBool(v : bool) =
                if v then w.Write(1uy)
                else w.Write(0uy)

            member x.WriteString(str : string) =
                w.Write(str.Length)
                let bytes = Text.Encoding.Unicode.GetBytes(str)
                w.Write(bytes)

    type BinaryReadingCoder(stream : Stream) =
        let r = new BinaryReader(stream)

        interface IReader with
            member x.ReadPrimitive() : 'a =
                let bytes = r.ReadBytes sizeof<'a>
                let gc = GCHandle.Alloc(bytes, GCHandleType.Pinned)
                try
                    NativePtr.read (NativePtr.ofNativeInt<'a> (gc.AddrOfPinnedObject()))
                finally
                    gc.Free()

            member x.ReadBool() =
                r.ReadByte() = 1uy

            member x.ReadString() =
                let length = r.ReadInt32()
                let bytes = r.ReadBytes (2 * length)
                Text.Encoding.Unicode.GetString(bytes)


    let toArray<'a> (data : 'a) =
        use ms = new MemoryStream()
        let w = BinaryWritingCoder(ms)
        w.WriteValue data
        ms.ToArray()

    let ofArray<'a> (bytes : byte[]) : 'a =
        use ms = new MemoryStream(bytes)
        let r = BinaryReadingCoder(ms)
        r.ReadValue()

    type Rec = { a : int; b : obj }

    type Something =
        class
            val mutable public A : obj
            val mutable public B : V3d
            val mutable public C : string[]
            val mutable public D : obj[]

            new(a,b) = { A = a; B = b; C = [| "A"; "B" |]; D = null }
        end


    let test() =
        let w = DummyWriter()
        let db = DummDb()

        let value = Something(1,V3d.III)
        value.A <- value
        let n : Option<int> = None

        let values = [|Choice1Of2 1; Choice2Of2 "bla"|]

        value.D <- [|Left 1 :> obj; Some 2 :> obj; n; {a = 1; b = null }; (1,2); values|]
        w.WriteValueDebug(value)


        let data = toArray value
        let test = data |> ofArray<Something>
        printfn "size = %A" data.Length
        printfn "%A" test.D

        printfn "%A" (test :> obj == test.A)

        


module TypeBuilderTest =
    open TypeBuilder

    let buildTest() =
        newtype { 
            do! inh typeof<obj>

            let! m_fld = NewType.fld "m_fld" typeof<int>

            do! iface typeof<IComparable> [
                    typeof<int>, "CompareTo", [typeof<obj>], (
                        codegen {
                            do! IL.ldconst 0
                            do! IL.ret
                        }
                    )
                ]

            do! recmem typeof<int> "Bla" [typeof<int>; typeof<int>] (fun self -> 
                codegen {
                    let! l = IL.newlabel
                        
                    do! IL.ldarg 1
                    do! IL.ldconst 1
                    do! IL.jmp LessOrEqual l

                    do! IL.ldarg 0
                    do! IL.ldfld m_fld
                    do! IL.ldarg 1
                    do! IL.ldarg 2
                    do! IL.add
                    do! IL.mul
                    do! IL.ret


                    do! IL.mark l
                    do! IL.ldarg 0

                    do! IL.ldarg 1
                    do! IL.ldconst 1
                    do! IL.add

                    do! IL.ldarg 2
                    do! IL.call self
                    do! IL.ret
                })

            do! mem typeof<int> "Blub" [] (
                    codegen {
                        do! IL.ldconst 1
                        do! IL.ret
                    }
                )

            do! mem typeof<int> "GetHashCode" [] (
                    codegen {
                        do! IL.printfn "hashcode"
                        do! IL.ldconst 27
                        do! IL.ret
                    }
                )

            do! ctor [typeof<int>] (
                    codegen {
                        do! IL.ldarg 0
                        do! IL.ldarg 1
                        do! IL.stfld m_fld
                        do! IL.ret
                    }
                )

        }

    let buildFunc() =
        newtype {
            let! off = fld "off" typeof<int>

            do! inh typeof<FSharpFunc<int, int>>

            do! mem typeof<int> "Invoke" [typeof<int>] (
                    codegen {
                        do! IL.ldarg 0
                        do! IL.ldfld off


                        do! IL.ldarg 1
                        do! IL.add
                        do! IL.ret
                    }
                )

            do! ctor [typeof<int>] (
                    codegen {
                        do! IL.ldarg 0
                        do! IL.ldarg 1
                        do! IL.stfld off
                        do! IL.ret
                    }
                )

        }

    let run() =
        Serializer.test()
//
//        let test = buildFunc()
//        let ctor = test.GetConstructor [|typeof<int>|]
//        let v = ctor.Invoke [|10|] |> unbox<int -> int>
//        let w = ctor.Invoke [|6|] |> unbox<int -> int>
//
//        printfn "v 10 = %A" (v 10)
//        printfn "w 10 = %A" (w 10)
