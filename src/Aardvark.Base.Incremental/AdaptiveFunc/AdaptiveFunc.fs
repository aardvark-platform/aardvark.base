namespace Aardvark.Base.Incremental

[<CompiledName("IAdaptiveFunc")>]
type afun<'a, 'b> =
    inherit IAdaptiveObject
    abstract member Evaluate : 'a -> 'b

module AFun =

    type AdaptiveFun<'a, 'b>(f : IMod<'a -> 'b>) as this =
        inherit AdaptiveObject()
        do f.AddOutput this

        member x.Evaluate v = 
            base.EvaluateAlways (fun () ->
                Mod.force f v
            )

        interface afun<'a, 'b> with
            member x.Evaluate v = x.Evaluate v

        new(f) = AdaptiveFun (Mod.constant f)

    type ConstantFun<'a, 'b>(value : IMod<'b>) =
        inherit AdaptiveObject()

        member x.Evaluate (v : 'a) = Mod.force value

        interface afun<'a, 'b> with
            member x.Evaluate v = x.Evaluate v

    let run (v : 'a) (f : afun<'a, 'b>) =
        [f :> IAdaptiveObject] |> Mod.mapCustom (fun () -> f.Evaluate v)

    let apply (v : IMod<'a>) (f : afun<'a, 'b>) =
        [v :> IAdaptiveObject; f :> IAdaptiveObject]
            |> Mod.mapCustom (fun () -> f.Evaluate (Mod.force v))

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
                let f = mf.GetValue()

                match !inner with
                    | Some f' when f' <> f ->
                        f'.RemoveOutput !self
                        f.AddOutput !self
                    | None ->
                        f.AddOutput !self
                    | _ ->
                        ()
                mf.GetValue().Evaluate x
            )
        mf.AddOutput !self
        !self :> afun<_,_>

    let compose (g : afun<'b, 'c>) (f : afun<'a, 'b>) =
        let res = AdaptiveFun(fun v -> g.Evaluate (f.Evaluate v)) :> afun<_,_>
        f.AddOutput res
        g.AddOutput res
        res

    let zipWith (combine : 'b -> 'c -> 'd) (f : afun<'a,'b>) (g : afun<'a, 'c>) =
        let res = AdaptiveFun(fun v -> combine (f.Evaluate v) (g.Evaluate v)) :> afun<_,_>
        f.AddOutput res
        g.AddOutput res
        res

    let zip (f : afun<'a,'b>) (g : afun<'a, 'c>) =
        zipWith (fun a b -> (a,b)) f g

    let rec chain (l : list<afun<'a, 'a>>) =
        match l with
            | [] -> create id
            | [f] -> f
            | f::fs -> compose (chain fs) f

    let runChain l initial =
        l |> chain |> run initial

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
                let (s, v) = m.runState.Evaluate s

                let c = tracker v
                match !cache with
                    | Some old when not c ->
                        old.Evaluate s
                    | _ ->
                        let inner = (f v).runState
                        match !cache with
                            | Some old -> old.RemoveOutput !self
                            | None -> ()
                        inner.AddOutput !self
                        cache := Some inner

                        inner.Evaluate s
            )

        m.runState.AddOutput !self

        { runState = !self }

    let bindMod (f : 'a -> astate<'s, 'b>) (m : IMod<'a>) =
        let mf = m |> Mod.map f

        let inner : ref<Option<afun<_,_>>> = ref None
        let self = ref Unchecked.defaultof<_>
        self :=
            AFun.AdaptiveFun(fun s -> 
                let run = mf.GetValue().runState
                
                match !inner with
                    | Some old when old <> run ->
                        old.RemoveOutput !self
                        run.AddOutput !self
                        inner := Some run

                    | None ->
                        run.AddOutput !self
                        inner := Some run 

                    | _ -> ()

                run.Evaluate s
            )

        mf.AddOutput !self
        { runState = !self :> afun<_,_> }

    let ofMod (m : IMod<'a>) : astate<'s, 'a> =
        let run = AFun.ofMod(m |> Mod.map (fun v s -> (s,v)))
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
            let v = m.GetValue()
            AState.create (f v v)
        else
//            astate {
//                let! v = m
//                let! s = modifyState' (fun s -> { s with pulled = Map.add m.Id (v :> obj) s.pulled })
//                let last =
//                    match Map.tryFind m.Id s.prev with
//                        | Some (:? 'a as v) -> v
//                        | _ -> v
//                return f last v
//            }
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

    let inline differentiate (m : IMod<'a>) = preWith (-) m



    type ControllerBuilder() =
        let time = [AdaptiveObject.Time] |> Mod.mapCustom (fun () -> ())
        member x.Bind(m : Controller<'a>, f : 'a -> Controller<'b>) : Controller<'b> =
            AState.bind f m

        member x.Bind(m : IMod<'a>, f : 'a -> Controller<'b>) : Controller<'b> =
            AState.bindMod f m

        member x.Return(v : 'a) : Controller<'a> =
            AState.create v

        member x.ReturnFrom(v : IMod<'a -> 'b>) : Controller<'a -> 'b> =
            AState.ofMod v

        member x.Run(c : Controller<'a -> 'b>) : afun<'a, 'b> =
            let initial = { prev = Map.empty; pulled = Map.empty }
            let state = Mod.init initial
            let res = c.runState |> AFun.apply state
            let stateChanged = ChangeTracker.track<ControllerState>
            initial |> stateChanged |> ignore

            let mf =
                res |> Mod.map (fun (newState, v) ->
                    time.GetValue()

                    if stateChanged newState then
                        time.MarkingCallbacks.Add (fun () ->
                            Mod.change state { prev = newState.pulled; pulled = Map.empty }
                        ) |> ignore

                    v
                )
            AFun.ofMod mf

    let controller = ControllerBuilder()

    open System


module TestController =

    let cc (m : IMod<bool>) (p : IMod<int>) =
        controller {
            let! d = m
            if d then
                let! dp = differentiate p
                return (fun s -> s + 0.02 * float dp)
            else
                return id
        }

    let wsad (dir : IMod<int>) (t : IMod<float>) =
        controller {
            let! d = dir
            if d <> 0 then
                let! dt = differentiate t
                return (fun s -> s + dt * 0.01)
            else
                return id
        }

    let integrate l initial =
        l |> AFun.chain |> AFun.run initial

    let test  =
        AFun.runChain [ 
            cc    (Mod.init false)    (Mod.init 10)
            wsad  (Mod.init 0)        (Mod.init 0.0)
        ] 
