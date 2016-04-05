#if INTERACTIVE
#r "..\\..\\Bin\\Debug\\Aardvark.Base.dll"
open Aardvark.Base
#else
namespace Aardvark.Base
#endif

open System.Linq.Expressions
open System.Runtime.InteropServices
open Microsoft.FSharp.Reflection
open System.Reflection
open System.Reflection.Emit
open System

/// <summary>
/// FunctionReflection provides functions wrapping MethodInfos as F# functions efficiently.
/// It also allows to create n-ary functions using an untyped implementation.
/// </summary>
module FunctionReflection =

    let rec private sequentializeTupleType (t : Type) =
        if FSharpType.IsTuple t then
            t |> FSharpType.GetTupleElements |> Seq.collect sequentializeTupleType
        else
            seq { yield t }

    let private tupleGet (p : ParameterExpression) (i : int) =
        if FSharpType.IsTuple p.Type then
            let name = sprintf "Item%d" (i+1)
            let pi = p.Type.GetProperty(name)
            if not (isNull pi) then
                Expression.Property(p, pi.GetMethod) :> Expression
            else
                failwithf "property %A was not fount for type %A" name p.Type
        else
            if i = 0 then p :> Expression
            else failwithf "cannot access tuple elements of type %A" p.Type

    let rec private getMethodSignatureInternal (t : Type) =
        if FSharpType.IsFunction t then
            let a, ret = FSharpType.GetFunctionElements t
            let args, ret = getMethodSignatureInternal ret

            if a = typeof<unit> then
                (args, ret)
            else
                (a::args, ret)

        else
            if t = typeof<unit> then ([],typeof<System.Void>)
            else ([],t)

    let rec getMethodSignature (t : Type) =
        if FSharpType.IsFunction t then
            let a, ret = FSharpType.GetFunctionElements t
            let args, ret = getMethodSignature ret

            //Here be dragons
            if a = typeof<unit> then
                (args, ret)
            else
                let a = a |> sequentializeTupleType |> Seq.toList
                (List.concat [a; args], ret)

        else
            if t = typeof<unit> then ([],typeof<System.Void>)
            else ([],t)

    

    let rec private buildLambda (args : list<ParameterExpression>) (callArgs : list<Expression>) (target : obj) (argTypes : list<Type>) (mi : MethodInfo) =
        match argTypes with
            | x::xs -> let v = Expression.Variable(x)
                       let x = x |> sequentializeTupleType |> Seq.toList
                       let tupleArgs = x |> List.mapi (fun i xi -> tupleGet v i)
                       buildLambda (v::args) (List.concat [callArgs; tupleArgs]) target xs mi
            | [] -> let args = List.rev args
                    let call = if isNull target then Expression.Call(mi, callArgs)
                               else Expression.Call(Expression.Constant(target), mi, callArgs)

                    Expression.Lambda(
                        call,
                        args)
          
    let private buildLambdaExpression (target : obj) (mi : MethodInfo) (funType : Type) : 'a =
        let (args,ret) = getMethodSignatureInternal funType //mi.GetParameters() |> Seq.map (fun p -> p.ParameterType) |> Seq.toList
        let lambda = buildLambda [] [] target args mi |> unbox<Expression<'a>>

        lambda.Compile()
          
    type FunctionBuilder =

        static member BuildFunction (target : obj,mi : MethodInfo) : 'a -> 'r =
            if typeof<'a> = typeof<unit> then
                if typeof<'r> = typeof<unit> then
                    let a : Action = buildLambdaExpression target mi typeof<'a -> 'r>
                    fun _ -> a.Invoke() |> unbox<'r>
                else
                    let f : Func<'r> = buildLambdaExpression target mi typeof<'a -> 'r>
                    fun _ -> f.Invoke()
            else
                if typeof<'r> = typeof<unit> then
                    let a : Action<'a> = buildLambdaExpression target mi typeof<'a -> 'r>
                    fun a0 -> a.Invoke(a0) |> unbox<'r>
                else
                    let f : Func<'a, 'r> = buildLambdaExpression target mi typeof<'a -> 'r>
                    f.Invoke

        static member BuildFunction (target : obj,mi : MethodInfo) : 'a -> 'b -> 'r =
            if typeof<'r> = typeof<unit> then
                let a : Action<'a, 'b> = buildLambdaExpression target mi typeof<'a -> 'b -> 'r>
                fun a0 a1 -> a.Invoke(a0,a1) |> unbox<'r>
            else
                let f : Func<'a, 'b, 'r> = buildLambdaExpression target mi typeof<'a -> 'b -> 'r>
                fun a0 a1 -> f.Invoke(a0,a1)

        static member BuildFunction (target : obj,mi : MethodInfo) : 'a -> 'b -> 'c -> 'r =
            if typeof<'r> = typeof<unit> then
                let a : Action<'a, 'b, 'c> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'r>
                fun a0 a1 a2 -> a.Invoke(a0,a1,a2) |> unbox<'r>
            else
                let f : Func<'a, 'b, 'c, 'r> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'r>
                fun a0 a1 a2 -> f.Invoke(a0,a1,a2)

        static member BuildFunction (target : obj,mi : MethodInfo) : 'a -> 'b -> 'c -> 'd -> 'r =
            if typeof<'r> = typeof<unit> then
                let a : Action<'a, 'b, 'c, 'd> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'r>
                fun a0 a1 a2 a3 -> a.Invoke(a0,a1,a2,a3) |> unbox<'r>
            else
                let f : Func<'a, 'b, 'c, 'd, 'r> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'r>
                fun a0 a1 a2 a3 -> f.Invoke(a0,a1,a2,a3)

        static member BuildFunction (target : obj,mi : MethodInfo) : 'a -> 'b -> 'c -> 'd -> 'e -> 'r =
            if typeof<'r> = typeof<unit> then
                let a : Action<'a, 'b, 'c, 'd, 'e> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'e -> 'r>
                fun a0 a1 a2 a3 a4 -> a.Invoke(a0,a1,a2,a3,a4) |> unbox<'r>
            else
                let f : Func<'a, 'b, 'c, 'd, 'e, 'r> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'e -> 'r>
                fun a0 a1 a2 a3 a4 -> f.Invoke(a0,a1,a2,a3,a4)

        static member BuildFunction (target : obj,mi : MethodInfo) : 'a -> 'b -> 'c -> 'd -> 'e -> 'f -> 'r =
            if typeof<'r> = typeof<unit> then
                let a : Action<'a, 'b, 'c, 'd, 'e, 'f> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'e -> 'f -> 'r>
                fun a0 a1 a2 a3 a4 a5 -> a.Invoke(a0,a1,a2,a3,a4,a5) |> unbox<'r>
            else
                let f : Func<'a, 'b, 'c, 'd, 'e, 'f, 'r> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'e -> 'f -> 'r>
                fun a0 a1 a2 a3 a4 a5 -> f.Invoke(a0,a1,a2,a3,a4,a5)

        static member BuildFunction (target : obj,mi : MethodInfo) : 'a -> 'b -> 'c -> 'd -> 'e -> 'f -> 'g -> 'r =
            if typeof<'r> = typeof<unit> then
                let a : Action<'a, 'b, 'c, 'd, 'e, 'f, 'g> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'e -> 'f -> 'g -> 'r>
                fun a0 a1 a2 a3 a4 a5 a6 -> a.Invoke(a0,a1,a2,a3,a4,a5,a6) |> unbox<'r>
            else
                let f : Func<'a, 'b, 'c, 'd, 'e, 'f, 'g, 'r> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'e -> 'f -> 'g -> 'r>
                fun a0 a1 a2 a3 a4 a5 a6 -> f.Invoke(a0,a1,a2,a3,a4,a5,a6)

        static member BuildFunction (target : obj,mi : MethodInfo) : 'a -> 'b -> 'c -> 'd -> 'e -> 'f -> 'g -> 'h -> 'r=
            if typeof<'r> = typeof<unit> then
                let a : Action<'a, 'b, 'c, 'd, 'e, 'f, 'g, 'h> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'e -> 'f -> 'g -> 'h -> 'r>
                fun a0 a1 a2 a3 a4 a5 a6 a7 -> a.Invoke(a0,a1,a2,a3,a4,a5,a6,a7) |> unbox<'r>
            else
                let f : Func<'a, 'b, 'c, 'd, 'e, 'f, 'g, 'h, 'r> = buildLambdaExpression target mi typeof<'a -> 'b -> 'c -> 'd -> 'e -> 'f -> 'g -> 'h -> 'r>
                fun a0 a1 a2 a3 a4 a5 a6 a7 -> f.Invoke(a0,a1,a2,a3,a4,a5,a6,a7)

    let private buildFunctionInternal (target : obj) (argTypes : list<Type>) (mi : MethodInfo) : 'a =
        let t = typeof<'a>
        let mutable ret = t
        let args = System.Collections.Generic.List()
        while FSharpType.IsFunction ret do
            let a,r = FSharpType.GetFunctionElements ret
            args.Add a
            ret <- r

        let ret = ret
        let genArgs = seq { yield! args; yield ret } |> Seq.toArray

        let m = typeof<FunctionBuilder>.GetMethods(BindingFlags.Public ||| BindingFlags.NonPublic ||| BindingFlags.Static) 
             |> Seq.filter (fun mi -> mi.Name = "BuildFunction")
             |> Seq.filter (fun mi -> mi.IsGenericMethodDefinition && mi.GetGenericArguments().Length = genArgs.Length)
             |> Seq.tryPick(fun mi -> mi.MakeGenericMethod genArgs |> Some)

        match m with
            | Some m -> m.Invoke(null, [|target; mi :> obj|]) |> unbox<'a>
            | None -> failwithf "could not wrap function of with generic args: %A" genArgs 

    let buildFunction (target : obj) (mi : MethodInfo) = 
        buildFunctionInternal target [] mi

        

    [<System.Diagnostics.DebuggerStepThroughAttribute>]
    let rec private makeNAryFunctionInternal (t : Type) (count : int) (f : obj[] -> obj) : int * (obj[] -> obj) =
        if FSharpType.IsFunction t then
            let index = count
            let (_,ret) = FSharpType.GetFunctionElements t
            let count,c = makeNAryFunctionInternal ret (count + 1) f

            (count, fun args ->
                        FSharpValue.MakeFunction(t, fun o ->
                            args.[index] <- o
                            c(args))
            )

        else
            (count, fun args -> f args)

    [<System.Diagnostics.DebuggerStepThroughAttribute>]
    let makeNAryFunction (f : obj[] -> obj) : 'x =
        let t = typeof<'x>

        let (count, c) = makeNAryFunctionInternal t 0 f
        let args = Array.create count null
        c args |> unbox

    let makeFormatFunction (f : string -> obj[] -> obj) (fmt : Printf.BuilderFormat<'r, obj>) : 'r =
        makeNAryFunction (fun args ->
            (f fmt.Value args)
        )

module QuotationReflectionHelpers =
    open Microsoft.FSharp.Quotations

    //extracts the (optional) top-most method call from an expression
    let rec tryGetMethodInfo (e : Expr) =
        match e with
            | Patterns.Call(_,mi,_) -> 
                if mi.IsGenericMethod then mi.GetGenericMethodDefinition() |> Some
                else mi |> Some
            | ExprShape.ShapeCombination(_, args) -> 
                args |> List.tryPick tryGetMethodInfo
            | ExprShape.ShapeLambda(_,b) ->
                tryGetMethodInfo b
            | _ -> None


    /// <summary>
    /// extracts the top-most method-call from an expression.
    /// When no method-call is found the method will raise an exception
    /// </summary>
    /// <param name="e"></param>
    let getMethodInfo (e : Expr) =
        match tryGetMethodInfo e with
            | Some mi -> mi
            | None -> failwith "could not find a method-call in expression"

module DelegateAdapters =
    type private AdapterFunc<'a> (f : Func<'a>) =
        inherit FSharpFunc<unit, 'a>()
        override x.Invoke(_) = f.Invoke()

    type private AdapterFunc<'a, 'b> (f : Func<'a, 'b>) =
        inherit FSharpFunc<'a, 'b>()
        override x.Invoke(a : 'a) = f.Invoke(a)

    type private AdapterFunc<'a, 'b, 'c> (f : Func<'a, 'b, 'c>) =
        inherit Microsoft.FSharp.Core.OptimizedClosures.FSharpFunc<'a, 'b, 'c>()
        override x.Invoke(a : 'a) = fun b -> f.Invoke(a,b)
        override x.Invoke(a : 'a, b : 'b) = f.Invoke(a,b)

    type private AdapterFunc<'a, 'b, 'c, 'd> (f : Func<'a, 'b, 'c, 'd>) =
        inherit Microsoft.FSharp.Core.OptimizedClosures.FSharpFunc<'a, 'b, 'c, 'd>()
        override x.Invoke(a : 'a) = fun b c -> f.Invoke(a,b,c)
        override x.Invoke(a : 'a, b : 'b, c : 'c) = f.Invoke(a,b,c)

    type private AdapterFunc<'a, 'b, 'c, 'd, 'e> (f : Func<'a, 'b, 'c, 'd, 'e>) =
        inherit Microsoft.FSharp.Core.OptimizedClosures.FSharpFunc<'a, 'b, 'c, 'd, 'e>()
        override x.Invoke(a : 'a) = fun b c d -> f.Invoke(a,b,c,d)
        override x.Invoke(a : 'a, b : 'b, c : 'c, d : 'd) = f.Invoke(a,b,c,d)

    type private AdapterFunc<'a, 'b, 'c, 'd, 'e, 'f> (f : Func<'a, 'b, 'c, 'd, 'e, 'f>) =
        inherit Microsoft.FSharp.Core.OptimizedClosures.FSharpFunc<'a, 'b, 'c, 'd, 'e, 'f>()
        override x.Invoke(a : 'a) = fun b c d e -> f.Invoke(a,b,c,d,e)
        override x.Invoke(a : 'a, b : 'b, c : 'c, d : 'd,e : 'e) = f.Invoke(a,b,c,d,e)



    type private AdapterAction<'x>(f : Action) =
        inherit FSharpFunc<unit, 'x>()
        override x.Invoke(_) = f.Invoke(); Unchecked.defaultof<'x>

    type private AdapterAction<'a, 'x>(f : Action<'a>) =
        inherit FSharpFunc<'a, 'x>()
        override x.Invoke(a) = f.Invoke(a); Unchecked.defaultof<'x>

    type private AdapterAction<'a, 'b, 'x> (f : Action<'a, 'b>) =
        inherit Microsoft.FSharp.Core.OptimizedClosures.FSharpFunc<'a, 'b, 'x>()
        override x.Invoke(a : 'a) = fun b -> f.Invoke(a,b); Unchecked.defaultof<_>
        override x.Invoke(a : 'a, b : 'b) = f.Invoke(a,b); Unchecked.defaultof<_>

    type private AdapterAction<'a, 'b, 'c, 'x> (f : Action<'a, 'b, 'c>) =
        inherit Microsoft.FSharp.Core.OptimizedClosures.FSharpFunc<'a, 'b, 'c, 'x>()
        override x.Invoke(a : 'a) = fun b c -> f.Invoke(a,b,c); Unchecked.defaultof<_>
        override x.Invoke(a : 'a, b : 'b, c : 'c) = f.Invoke(a,b,c); Unchecked.defaultof<_>

    type private AdapterAction<'a, 'b, 'c, 'd, 'x> (f : Action<'a, 'b, 'c, 'd>) =
        inherit Microsoft.FSharp.Core.OptimizedClosures.FSharpFunc<'a, 'b, 'c, 'd, 'x>()
        override x.Invoke(a : 'a) = fun b c d -> f.Invoke(a,b,c,d); Unchecked.defaultof<_>
        override x.Invoke(a : 'a, b : 'b, c : 'c, d : 'd) = f.Invoke(a,b,c,d); Unchecked.defaultof<_>

    type private AdapterAction<'a, 'b, 'c, 'd, 'e, 'x> (f : Action<'a, 'b, 'c, 'd, 'e>) =
        inherit Microsoft.FSharp.Core.OptimizedClosures.FSharpFunc<'a, 'b, 'c, 'd, 'e, 'x>()
        override x.Invoke(a : 'a) = fun b c d e -> f.Invoke(a,b,c,d,e); Unchecked.defaultof<_>
        override x.Invoke(a : 'a, b : 'b, c : 'c, d : 'd,e : 'e) = f.Invoke(a,b,c,d,e); Unchecked.defaultof<_>

    let rec getFunctionSignature (t : Type) =
        if FSharpType.IsFunction t then
            let (d,i) = FSharpType.GetFunctionElements t
            let args, ret = getFunctionSignature i
            d::args, ret
        else
            [], t

    let wrapUntyped (d : Delegate) : obj =
        let mi = d.GetMethodInfo()
        let args = mi.GetParameters() |> Array.map (fun p -> p.ParameterType)
        
        let ret = 
            if mi.ReturnType = typeof<System.Void> then typeof<unit>
            else mi.ReturnType

        let args = Array.append args [|ret|]

        let generic = 
            if mi.ReturnType = typeof<System.Void> then
                Type.GetType(typedefof<AdapterAction<_>>.FullName.Replace("1", string args.Length))
            else
                Type.GetType(typedefof<AdapterFunc<_>>.FullName.Replace("1", string args.Length))

        if isNull generic then
            let code = args |> Seq.map (fun t -> t.Name) |> String.concat " -> "
            failwithf "could not get delegate adapter: %s" code

        let t = generic.MakeGenericType args
        Activator.CreateInstance(t, d)

    let wrap (d : Delegate) : 'a =
        match wrapUntyped d with
            | :? 'a as res -> res
            | _ ->
                failwithf "[DelegateAdapter] cannot convert from delegate %A to %A" (d.GetType()) typeof<'a>
