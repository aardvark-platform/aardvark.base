namespace Aardvark.Base.Incremental

open FSharp.Data.Adaptive

[<CompiledName("IAdaptiveFunc")>]
type afun<'a, 'b> =
    inherit IAdaptiveObject
    abstract member Evaluate : AdaptiveToken * 'a -> 'b

module AFun =

    type AdaptiveFun<'a, 'b>(f : aval<AdaptiveToken -> 'a -> 'b>) =
        inherit AdaptiveObject()

        member x.Evaluate (caller, v) = 
            x.EvaluateAlways caller (fun token ->
                x.OutOfDate <- true
                f.GetValue token token v
            )

        interface afun<'a, 'b> with
            member x.Evaluate (c, v) = x.Evaluate(c, v)

        new(f) = AdaptiveFun (AVal.constant f)

    type ConstantFun<'a, 'b>(value : aval<'b>) =
        inherit AdaptiveObject()

        member x.Evaluate (token : AdaptiveToken, v : 'a) = 
            x.EvaluateAlways token (fun token ->
                value.GetValue token
            )

        interface afun<'a, 'b> with
            member x.Evaluate (token, v) = x.Evaluate (token, v)

    let run (v : 'a) (f : afun<'a, 'b>) =
        AVal.custom (fun s -> 
            f.Evaluate (s, v)
        )

    let apply (v : aval<'a>) (f : afun<'a, 'b>) =
        AVal.custom (fun s -> 
            f.Evaluate (s, v.GetValue s)
        )

    let create (f : 'a -> 'b) =
        AdaptiveFun(fun _ -> f) :> afun<_,_>

    let constant (v : 'b) : afun<'a, 'b> =
        ConstantFun(AVal.constant v) :> afun<_,_>

    let ofAVal (f : aval<'a -> 'b>) =
        AdaptiveFun(f |> AVal.map (fun f _ -> f)) :> afun<_,_>

    let bind (f : 'a -> afun<'x, 'y>) (m : aval<'a>) =
        let mf = m |> AVal.map f

        let inner = ref None
        let self = ref Unchecked.defaultof<_>
        self :=
            AdaptiveFun(fun token x ->
                let f = mf.GetValue(token)

                match !inner with
                    | Some f' when f' <> f ->
                        f'.Outputs.Remove !self |> ignore
                    | _ ->
                        ()
                mf.GetValue(token).Evaluate(token, x)
            )

        !self :> afun<_,_>

    let compose (g : afun<'b, 'c>) (f : afun<'a, 'b>) =
        AdaptiveFun(fun token v -> g.Evaluate(token, f.Evaluate(token, v))) :> afun<_,_>

    let zipWith (combine : 'b -> 'c -> 'd) (f : afun<'a,'b>) (g : afun<'a, 'c>) =
        AdaptiveFun(fun token v -> combine (f.Evaluate(token, v)) (g.Evaluate(token, v))) :> afun<_,_>

    let zip (f : afun<'a,'b>) (g : afun<'a, 'c>) =
        zipWith (fun a b -> (a,b)) f g

    let rec chain (l : list<afun<'a, 'a>>) =
        match l with
            | [] -> create id
            | [f] -> f
            | f::fs -> compose (chain fs) f

    let chainM (l : aval<list<afun<'a, 'a>>>) =
        l |> AVal.map chain |> bind id

    let runChain l initial =
        l |> chain |> run initial

    let integrate (f : afun<'a, 'a>) (initial : 'a) =
        let input = AVal.init initial
        let inputChanged = ChangeTracker.track<'a>
        initial |> inputChanged |> ignore

        //let ti = AdaptiveObject.Time
        AVal.custom (fun s -> 
            //lock ti (fun () -> ti.Outputs.Remove input |> ignore)
            let v = input.GetValue s
            let res = f.Evaluate(s, v)
            if inputChanged res then
                AdaptiveObject.RunAfterEvaluate(fun () ->
                    input.Value <- res
                )

            res
        )

[<AutoOpen>]
module ``AFun Builder`` =

    type AFunBuilder() =
        member x.Bind(m : aval<'a>, f : 'a -> afun<'x, 'y>) =
            AFun.bind f m

        member x.Return(f : 'a -> 'b) =
            AFun.create f

        member x.ReturnFrom(f : afun<'a, 'b>) = 
            f

        member x.ReturnFrom(m : aval<'a -> 'b>) =
            AFun.ofAVal m

    let inline (>>.) (f : afun<'a, 'b>) (g : afun<'b, 'c>) =
        AFun.compose g f

    let inline (<<.) (g : afun<'b, 'c>) (f : afun<'a, 'b>) =
        AFun.compose g f

    let afun = AFunBuilder()



[<CompiledName("IAdaptiveState")>]
type astate<'s, 'a> = { [<CompiledName("RunState")>] runState : afun<'s, 's * 'a> }

module AState =
    let create (v : 'a) =
        { runState = AFun.create (fun s -> s,v) }

    let map (f : 'a -> 'b) (m : astate<'s, 'a>) =
        let changed = ChangeTracker.track<'a>
        let cache = ref None
        { runState = 
            m.runState 
                |> AFun.compose (
                    AFun.create (fun (s,v) -> 
                        let c = changed v
                        match !cache with
                            | Some o when not c -> (s,o)
                            | _ ->
                                let n = f v
                                cache := Some n
                                (s,n)
                    )
                   ) 
        }

    let bind (f : 'a -> astate<'s, 'b>) (m : astate<'s, 'a>) =
        let cache : ref<Option<afun<_,_>>> = ref None
        let tracker = ChangeTracker.track<'a>

        let self = 
            AFun.AdaptiveFun(fun token s ->
                let (s, v) = m.runState.Evaluate (token, s)

                let c = tracker v
                match !cache with
                    | Some old when not c ->
                        old.Evaluate (token, s)
                    | _ ->
                        let inner = (f v).runState
                        match !cache with
                            | Some old -> old.Outputs.Remove token.Caller.Value |> ignore
                            | None -> ()
                        cache := Some inner

                        inner.Evaluate (token, s)
            )

        { runState = self }

    let bindAVal (f : 'a -> astate<'s, 'b>) (m : aval<'a>) =
        let mf = m |> AVal.map f

        let inner : ref<Option<afun<_,_>>> = ref None
        let self =
            AFun.AdaptiveFun(fun token s -> 
                let run = mf.GetValue(token).runState
                
                match !inner with
                    | Some old when old <> run ->
                        old.Outputs.Remove token.Caller.Value |> ignore

                    | _ -> ()

                inner := Some run 
                run.Evaluate (token, s)
            )

        { runState = self :> afun<_,_> }

    let ofAVal (m : aval<'a>) : astate<'s, 'a> =
        let run = AFun.ofAVal(m |> AVal.map (fun v s -> (s,v)))
        { runState = run }

    let ofAFun (m : afun<'a, 'b>) : astate<'s, 'a -> 'b> =
        let run = AFun.AdaptiveFun(fun token s -> (s,fun v -> m.Evaluate(token,v)))
        { runState = run }

    let getState<'s> = { runState = AFun.create (fun s -> (s,s)) }
    let putState (s : 's) = { runState = AFun.create (fun _ -> (s,())) }
    let modifyState (f : 's -> 's) = { runState = AFun.create (fun s -> (f s,())) }
    let modifyState' (f : 's -> 's) = { runState = AFun.create (fun s -> (f s,s)) }


[<AutoOpen>]
module ``AState Builder`` =
    type AStateBuilder() =
        member x.Bind(m : astate<'s, 'a>, f : 'a -> astate<'s, 'b>) =
            AState.bind f m

        member x.Bind(m : aval<'a>, f : 'a -> astate<'s, 'b>) =
            AState.bindAVal f m

        member x.Return(v : 'a) =
            AState.create v

    let astate = AStateBuilder()



type ControllerState = { prev : HashMap<obj, obj>; pulled : HashMap<obj, obj> }
type Controller<'a> = astate<ControllerState, 'a>

[<AutoOpen>]
module ``Controller Builder`` =
    open AState

    let preWith (f : 'a -> 'a -> 'b) (m : aval<'a>) =
        if m.IsConstant then
            let v = m.GetValue(AdaptiveToken.Top)
            AState.create (f v v)
        else
            m |> AState.bindAVal (fun v ->
                modifyState' (fun s -> { s with pulled = HashMap.add (m :> obj) (v :> obj) s.pulled })
                    |> AState.map (fun s ->
                        let last =
                            match HashMap.tryFind (m :> obj) s.prev with
                                | Some (:? 'a as v) -> v
                                | _ -> v
                        f last v
                       )
            )

    let inline withLast (m : aval<'a>) = preWith (fun a b -> (a,b)) m

    let inline pre (m : aval<'a>) = preWith (fun a _ -> a) m

    let inline differentiate (m : aval<'a>) = preWith (fun o n -> n - o) m



    type ControllerBuilder() =

        member x.Bind(m : Controller<'a>, f : 'a -> Controller<'b>) : Controller<'b> =
            AState.bind f m

        member x.Bind(m : aval<'a>, f : 'a -> Controller<'b>) : Controller<'b> =
            AState.bindAVal f m

        member x.Return(v : 'a) : Controller<'a> =
            AState.create v

        member x.ReturnFrom(v : aval<'a -> 'b>) : Controller<'a -> 'b> =
            AState.ofAVal v

        member x.ReturnFrom(v : afun<'a, 'b>) : Controller<'a -> 'b> =
            AState.ofAFun v

        member x.Zero() : Controller<'a -> 'a> =
            AState.create id

        member x.Run(c : Controller<'a -> 'b>) : afun<'a, 'b> =
            let initial = { prev = HashMap.empty; pulled = HashMap.empty }
            let state = AVal.init initial
            let res = c.runState |> AFun.apply state
            let stateChanged = ChangeTracker.track<ControllerState>
            initial |> stateChanged |> ignore

            let mf =
                res |> AVal.map (fun (newState, v) ->
                    AdaptiveObject.RunAfterEvaluate(fun () ->
                        state.Value <- { prev = newState.pulled; pulled = HashMap.empty }
                    )
                    v
                )
            AFun.ofAVal mf

    let controller = ControllerBuilder()
