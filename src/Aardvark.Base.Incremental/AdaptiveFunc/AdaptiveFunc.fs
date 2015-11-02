namespace Aardvark.Base.Incremental

[<CompiledName("IAdaptiveFunc")>]
type afun<'a, 'b> =
    inherit IAdaptiveObject
    abstract member Evaluate : IAdaptiveObject * 'a -> 'b

module AFun =

    type AdaptiveFun<'a, 'b>(f : IMod<'a -> 'b>) as this =
        inherit AdaptiveObject()
        do f.AddOutput this

        member x.Evaluate (caller, v) = 
            x.EvaluateAlways caller (fun () ->
                this.OutOfDate <- true
                f.GetValue x v
            )

        interface afun<'a, 'b> with
            member x.Evaluate (c, v) = x.Evaluate(c, v)

        new(f) = AdaptiveFun (Mod.constant f)

    type ConstantFun<'a, 'b>(value : IMod<'b>) =
        inherit AdaptiveObject()

        member x.Evaluate (caller : IAdaptiveObject, v : 'a) = 
            x.EvaluateAlways caller (fun () ->
                value.GetValue x
            )

        interface afun<'a, 'b> with
            member x.Evaluate (caller, v) = x.Evaluate (caller, v)

    let run (v : 'a) (f : afun<'a, 'b>) =
        [f :> IAdaptiveObject] |> Mod.mapCustom (fun s -> 
            f.Evaluate (s, v)
        )

    let apply (v : IMod<'a>) (f : afun<'a, 'b>) =
        [v :> IAdaptiveObject; f :> IAdaptiveObject]
            |> Mod.mapCustom (fun s -> 
                f.Evaluate (s, v.GetValue s)
            )

    let create (f : 'a -> 'b) =
        AdaptiveFun(f) :> afun<_,_>

    let constant (v : 'b) : afun<'a, 'b> =
        ConstantFun(Mod.constant v) :> afun<_,_>

    let ofMod (f : IMod<'a -> 'b>) =
        AdaptiveFun(f) :> afun<_,_>

    let bind (f : 'a -> afun<'x, 'y>) (m : IMod<'a>) =
        let mf = m |> Mod.map f

        let inner = ref None
        let self = ref Unchecked.defaultof<_>
        self :=
            AdaptiveFun(fun x ->
                let f = mf.GetValue(!self)

                match !inner with
                    | Some f' when f' <> f ->
                        f'.RemoveOutput !self
                        f.AddOutput !self
                    | None ->
                        f.AddOutput !self
                    | _ ->
                        ()
                mf.GetValue(!self).Evaluate(!self, x)
            )
        mf.AddOutput !self
        !self :> afun<_,_>

    let compose (g : afun<'b, 'c>) (f : afun<'a, 'b>) =
        let res = ref Unchecked.defaultof<_>
        res := AdaptiveFun(fun v -> g.Evaluate(!res, f.Evaluate(!res, v))) :> afun<_,_>
        f.AddOutput !res
        g.AddOutput !res
        !res

    let zipWith (combine : 'b -> 'c -> 'd) (f : afun<'a,'b>) (g : afun<'a, 'c>) =
        let res = ref Unchecked.defaultof<_>
        res := AdaptiveFun(fun v -> combine (f.Evaluate(!res, v)) (g.Evaluate(!res, v))) :> afun<_,_>
        f.AddOutput !res
        g.AddOutput !res
        !res

    let zip (f : afun<'a,'b>) (g : afun<'a, 'c>) =
        zipWith (fun a b -> (a,b)) f g

    let rec chain (l : list<afun<'a, 'a>>) =
        match l with
            | [] -> create id
            | [f] -> f
            | f::fs -> compose (chain fs) f

    let runChain l initial =
        l |> chain |> run initial

    let integrate (f : afun<'a, 'a>) (initial : 'a) =
        let input = Mod.init initial
        let inputChanged = ChangeTracker.track<'a>
        initial |> inputChanged |> ignore

        [f :> IAdaptiveObject; input :> IAdaptiveObject] 
            |> Mod.mapCustom (fun s -> 
                AdaptiveObject.Time.Outputs.Remove input |> ignore
                let v = input.GetValue s
                let res = f.Evaluate(s, v)
                if inputChanged res then
                    input.UnsafeCache <- res
                    AdaptiveObject.Time.Outputs.Add input |> ignore

                res
               )

[<AutoOpen>]
module ``AFun Builder`` =

    type AFunBuilder() =
        member x.Bind(m : IMod<'a>, f : 'a -> afun<'x, 'y>) =
            AFun.bind f m

        member x.Return(f : 'a -> 'b) =
            AFun.create f

        member x.ReturnFrom(f : afun<'a, 'b>) = 
            f

        member x.ReturnFrom(m : IMod<'a -> 'b>) =
            AFun.ofMod m

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
        let self = ref Unchecked.defaultof<_>
        let cache : ref<Option<afun<_,_>>> = ref None
        let tracker = ChangeTracker.track<'a>

        self := 
            AFun.AdaptiveFun(fun s ->
                let (s, v) = m.runState.Evaluate (!self, s)

                let c = tracker v
                match !cache with
                    | Some old when not c ->
                        old.Evaluate (!self, s)
                    | _ ->
                        let inner = (f v).runState
                        match !cache with
                            | Some old -> old.RemoveOutput !self
                            | None -> ()
                        inner.AddOutput !self
                        cache := Some inner

                        inner.Evaluate (!self, s)
            )

        m.runState.AddOutput !self

        { runState = !self }

    let bindMod (f : 'a -> astate<'s, 'b>) (m : IMod<'a>) =
        let mf = m |> Mod.map f

        let inner : ref<Option<afun<_,_>>> = ref None
        let self = ref Unchecked.defaultof<_>
        self :=
            AFun.AdaptiveFun(fun s -> 
                let run = mf.GetValue(!self).runState
                
                match !inner with
                    | Some old when old <> run ->
                        old.RemoveOutput !self
                        run.AddOutput !self
                        inner := Some run

                    | None ->
                        run.AddOutput !self
                        inner := Some run 

                    | _ -> ()

                run.Evaluate (!self, s)
            )

        mf.AddOutput !self
        { runState = !self :> afun<_,_> }

    let ofMod (m : IMod<'a>) : astate<'s, 'a> =
        let run = AFun.ofMod(m |> Mod.map (fun v s -> (s,v)))
        { runState = run }

    let ofAFun (m : afun<'a, 'b>) : astate<'s, 'a -> 'b> =
        let run = ref Unchecked.defaultof<_>
        run := AFun.create (fun s -> (s,fun v -> m.Evaluate(!run,v)))
        m.AddOutput !run
        { runState = !run }

    let getState<'s> = { runState = AFun.create (fun s -> (s,s)) }
    let putState (s : 's) = { runState = AFun.create (fun _ -> (s,())) }
    let modifyState (f : 's -> 's) = { runState = AFun.create (fun s -> (f s,())) }
    let modifyState' (f : 's -> 's) = { runState = AFun.create (fun s -> (f s,s)) }


[<AutoOpen>]
module ``AState Builder`` =
    type AStateBuilder() =
        member x.Bind(m : astate<'s, 'a>, f : 'a -> astate<'s, 'b>) =
            AState.bind f m

        member x.Bind(m : IMod<'a>, f : 'a -> astate<'s, 'b>) =
            AState.bindMod f m

        member x.Return(v : 'a) =
            AState.create v

    let astate = AStateBuilder()



type ControllerState = { prev : Map<int, obj>; pulled : Map<int, obj> }
type Controller<'a> = astate<ControllerState, 'a>

[<AutoOpen>]
module ``Controller Builder`` =
    open AState

    let preWith (f : 'a -> 'a -> 'b) (m : IMod<'a>) =
        if m.IsConstant then
            let v = m.GetValue(null)
            AState.create (f v v)
        else
            m |> AState.bindMod (fun v ->
                modifyState' (fun s -> { s with pulled = Map.add m.Id (v :> obj) s.pulled })
                    |> AState.map (fun s ->
                        let last =
                            match Map.tryFind m.Id s.prev with
                                | Some (:? 'a as v) -> v
                                | _ -> v
                        f last v
                       )
            )

    let inline withLast (m : IMod<'a>) = preWith (fun a b -> (a,b)) m

    let inline pre (m : IMod<'a>) = preWith (fun a _ -> a) m

    let inline differentiate (m : IMod<'a>) = preWith (fun o n -> n - o) m



    type ControllerBuilder() =

        member x.Bind(m : Controller<'a>, f : 'a -> Controller<'b>) : Controller<'b> =
            AState.bind f m

        member x.Bind(m : IMod<'a>, f : 'a -> Controller<'b>) : Controller<'b> =
            AState.bindMod f m

        member x.Return(v : 'a) : Controller<'a> =
            AState.create v

        member x.ReturnFrom(v : IMod<'a -> 'b>) : Controller<'a -> 'b> =
            AState.ofMod v

        member x.ReturnFrom(v : afun<'a, 'b>) : Controller<'a -> 'b> =
            AState.ofAFun v

        member x.Zero() : Controller<'a -> 'a> =
            AState.create id

        member x.Run(c : Controller<'a -> 'b>) : afun<'a, 'b> =
            let initial = { prev = Map.empty; pulled = Map.empty }
            let state = Mod.init initial
            let res = c.runState |> AFun.apply state
            let stateChanged = ChangeTracker.track<ControllerState>
            initial |> stateChanged |> ignore

            let mf =
                res |> Mod.map (fun (newState, v) ->
                    AdaptiveObject.Time.Outputs.Remove state |> ignore

                    if newState.pulled <> newState.prev then
                        state.UnsafeCache <-  { prev = newState.pulled; pulled = Map.empty }
                        AdaptiveObject.Time.Outputs.Add state |> ignore

                    v
                )
            AFun.ofMod mf

    let controller = ControllerBuilder()
