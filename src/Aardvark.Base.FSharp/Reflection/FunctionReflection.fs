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