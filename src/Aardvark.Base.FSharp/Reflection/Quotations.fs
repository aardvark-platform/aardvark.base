namespace Aardvark.Base.Quotations


open System
open System.Reflection
open Aardvark.Base
open Aardvark.Base.IL
open Microsoft.FSharp.Quotations
open Microsoft.FSharp.Quotations.Patterns

module QuotationDisassembler =
    open Aardvark.Base.IL.StateMonad

    type DisassemblerState =
        {
            labels      : list<Label>
            entries     : Map<Label, list<Instruction>>
            stack       : list<Expr>
            locals      : Map<Local, Var>
            args        : Var[]
            ret         : Type
        }

    type Disasm<'a> = State<DisassemblerState, 'a>

    module Disasm =
        let pop : Disasm<Expr> =
            fun s ->
                match s.stack with
                    | h::stack ->
                        let s = { s with stack = stack }
                        s, h
                    | _ ->
                        failwithf "[Disasm] cannot pop from empty stack"

        let push (e : Expr) : Disasm<unit> =
            fun s -> { s with stack = e::s.stack }, ()

        let getArg (i : int) : Disasm<Var> =
            fun s -> s, s.args.[i]

        let getLocal (l : Local) : Disasm<bool * Var> =
            fun s ->
                match Map.tryFind l s.locals with
                    | Some v -> s,(false, v)
                    | None ->
                        let v = Var("var" + string l.Id, l.Type, true)
                        let s = { s with locals = Map.add l v s.locals }
                        s,(true, v)

        let getCode (l : Label) : Disasm<list<Instruction>> =
            fun s ->
                match Map.tryFind l s.entries with
                    | Some e -> s,e
                    | None -> failwithf "invalid label: %A" l

        let pushLabel (l : Label) : Disasm<unit> =
            fun s -> { s with labels = l::s.labels }, ()

        let peekLabel : Disasm<Option<Label>> =
            fun s -> 
                match s.labels with
                    | [] -> s, None
                    | l::_ -> s, Some l

        let popLabel : Disasm<unit> =
            fun s -> 
                match s.labels with
                    | [] -> s, ()
                    | _::rest -> { s with labels = rest }, ()


    let private add = QuotationReflectionHelpers.getMethodInfo <@ (+) @>
    let private sub = QuotationReflectionHelpers.getMethodInfo <@ (-) @>

    let private less = QuotationReflectionHelpers.getMethodInfo <@ (<) @>
    let private lessEqual = QuotationReflectionHelpers.getMethodInfo <@ (<=) @>
    let private greater = QuotationReflectionHelpers.getMethodInfo <@ (>) @>
    let private greaterEqual = QuotationReflectionHelpers.getMethodInfo <@ (>=) @>
    let private equal = QuotationReflectionHelpers.getMethodInfo <@ (=) @>
    let private notEqual = QuotationReflectionHelpers.getMethodInfo <@ (<>) @>


    type Expr with
        static member Add (l : Expr, r : Expr) =
            let m = add.MakeGenericMethod [|l.Type; r.Type; r.Type|]
            Expr.Call(m, [l;r])

        static member Sub (l : Expr, r : Expr) =
            let m = sub.MakeGenericMethod [|l.Type; r.Type; r.Type|]
            Expr.Call(m, [l;r])

        static member Compare(op : JumpCondition, l : Expr, r : Expr) =
            let meth = 
                match op with
                    | Less              -> less.MakeGenericMethod [|l.Type|]
                    | LessOrEqual       -> lessEqual.MakeGenericMethod [|l.Type|]
                    | Greater           -> greater.MakeGenericMethod [|l.Type|]
                    | GreaterOrEqual    -> greaterEqual.MakeGenericMethod [|l.Type|]
                    | Equal             -> equal.MakeGenericMethod [|l.Type|]
                    | NotEqual          -> notEqual.MakeGenericMethod [|l.Type|]
                    | _                 -> failwithf "%A is not a binary comparison" op

            Expr.Call(meth, [l;r])

        static member NegatedCompare(op : JumpCondition, l : Expr, r : Expr) =
            let meth = 
                match op with
                    | Less              -> greaterEqual.MakeGenericMethod [|l.Type|]
                    | LessOrEqual       -> greater.MakeGenericMethod [|l.Type|]
                    | Greater           -> lessEqual.MakeGenericMethod [|l.Type|]
                    | GreaterOrEqual    -> less.MakeGenericMethod [|l.Type|]
                    | Equal             -> notEqual.MakeGenericMethod [|l.Type|]
                    | NotEqual          -> equal.MakeGenericMethod [|l.Type|]
                    | _                 -> failwithf "%A is not a binary comparison" op

            Expr.Call(meth, [l;r])

        static member Constant(c : Constant) =
            match c with
                | Int8 v -> Expr.Value(v)
                | UInt8 v -> Expr.Value(v)
                | Int16 v -> Expr.Value(v)
                | UInt16 v -> Expr.Value(v)
                | Int32 v -> Expr.Value(v)
                | UInt32 v -> Expr.Value(v)
                | Int64 v -> Expr.Value(v)
                | UInt64 v -> Expr.Value(v)
                | NativeInt v -> Expr.Value(v)
                | UNativeInt v -> Expr.Value(v)
                | Float32 v -> Expr.Value(v)
                | Float64 v -> Expr.Value(v)
                | String str -> Expr.Value(str)

    let rec private simulate (code : list<Instruction>) =
        state {
            match code with
                | [] -> 
                    return failwithf "code ends without ret"
                | Nop :: rest -> 
                    return! simulate rest

                | Ldarg i :: rest ->
                    let! arg = Disasm.getArg i 
                    do! Disasm.push (Expr.Var arg)
                    return! simulate rest

                | Ldloc l :: rest ->
                    let! (_,l) = Disasm.getLocal l
                    do! Disasm.push (Expr.Var l)
                    return! simulate rest

                | Stloc l :: rest ->
                    let! (isNew, l) = Disasm.getLocal l
                    let! v = Disasm.pop
                    let! rest = simulate rest
                    if isNew then
                        return Expr.Let(l, v, rest)
                    else
                        return Expr.Sequential(Expr.VarSet(l, v), rest)

                | LdlocA l :: rest ->   
                    let! (_,l) = Disasm.getLocal l
                    do! Disasm.push (Expr.AddressOf (Expr.Var l))
                    return! simulate rest

                | Add :: rest ->
                    let! r = Disasm.pop
                    let! l = Disasm.pop
                    do! Disasm.push (Expr.Add(l,r))
                    return! simulate rest

                | Sub :: rest ->
                    let! r = Disasm.pop
                    let! l = Disasm.pop
                    do! Disasm.push (Expr.Sub(l,r))
                    return! simulate rest


                | Ret :: _ ->
                    return! Disasm.pop

                | Mark l :: rest ->
                    do! Disasm.pushLabel l
                    return! simulate rest

                | LdConst c :: rest ->
                    do! Disasm.push (Expr.Constant c)
                    return! simulate rest

                | ConditionalJump(c, label) :: rest ->
                    let! r = Disasm.pop
                    let! l = Disasm.pop

                    let! trueCode = Disasm.getCode label

                    let! ifFalse = simulate rest
                    let! ifTrue = simulate trueCode

                    let cmp = Expr.NegatedCompare(c, l, r)
                    if ifFalse.Type <> ifTrue.Type then
                        return Expr.Sequential(Expr.WhileLoop(cmp, ifFalse), ifTrue)
                    else
                        return Expr.IfThenElse(cmp, ifFalse, ifTrue)

                | Jump label :: _ ->
                    let! l = Disasm.peekLabel
                    match l with
                        | Some l when l = label ->
                            do! Disasm.popLabel
                            return Expr.Value(())
                        | _ -> 
                            let! cont = Disasm.getCode label
                            return! simulate cont

                | _ ->
                    return failwithf "unsupported instruction: %A" code
        }

    let disassemble (m : MethodDefinition) =

        let rec buildEntries (current : Map<Label, list<Instruction>>) (code : list<Instruction>) =
            match code with
                | Mark l :: rest ->
                    match Map.tryFind l current with
                        | Some _ -> current
                        | _ ->
                            let current = Map.add l rest current
                            buildEntries current rest
                | a::rest ->
                    
                    buildEntries current rest


                | [] ->
                    current

        let entries = buildEntries Map.empty m.Body

        let (state,res) =
            simulate m.Body {
                labels = []
                entries = entries
                stack = []
                locals = Map.empty
                args = m.ArgumentTypes |> Array.mapi (fun i a -> Var(sprintf "arg%d" i, a, true))
                ret = m.ReturnType
            }

        res
            

module QuotationDisassemblerTest =
    

    type Dummy =
        static member Method (a : int, b : int) =
            let mutable c = a
            for i in 0..a do
                c <- c - b
            c

    let run() =
        let def = Disassembler.disassemble (typeof<Dummy>.GetMethod "Method")

        Log.start "Method"

        for i in def.Body do
            Log.line "%A" i

        Log.stop()

        let e = QuotationDisassembler.disassemble def

        printfn "%A" e







