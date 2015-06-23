namespace Aardvark.Base.Incremental

[<CompiledName("IAdaptiveFunc")>]
type afun<'a, 'b> =
    inherit IAdaptiveObject
    abstract member IsConstant : bool
    abstract member Eval : 'a -> 'b


module AFun =

    type AdaptiveFunc<'a, 'b>(f : 'a -> 'b) =
        inherit AdaptiveObject()

        member x.Eval v = 
            base.EvaluateAlways (fun () ->
                f v
            )

        interface afun<'a, 'b> with
            member x.IsConstant = false
            member x.Eval v = x.Eval v

    type ConstantFunc<'a, 'b>(f : 'a -> 'b) =
        inherit ConstantObject()

        member x.Eval v = f v

        interface afun<'a, 'b> with
            member x.IsConstant = true
            member x.Eval v = x.Eval v

    let create (f : 'a -> 'b) =
        ConstantFunc(f) :> afun<_,_>

    let compose (g : afun<'b, 'c>) (f : afun<'a, 'b>) =
        match f.IsConstant, g.IsConstant with
            | true, true -> 
                ConstantFunc(fun v -> g.Eval (f.Eval v)) :> afun<_,_>
            | _ ->
                let res = AdaptiveFunc(fun v -> g.Eval (f.Eval v)) :> afun<_,_>
                f.AddOutput res
                g.AddOutput res
                res
