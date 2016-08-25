namespace React

open System
open Aardvark.Base
open Aardvark.Base.Monads.State
open Aardvark.Base.Incremental

type Event<'a> = IObservable<'a>

type EventState<'s> =
    {
        time : float
        state : 's
    }

module EventState =

    let lift (m : State<'s, 'a>) : State<EventState<'s>, 'a> =
        { new State<EventState<'s>, 'a>() with
            member x.Run(state) =
                let mutable user = state.state
                let result = m.Run(&user)
                state <- { state with state = user }
                result
        }

    let time<'s> : State<EventState<'s>, float> =
        State.get |> State.map (fun s -> s.time)


open System.Collections.Generic

type IEventSource =
    abstract member Subscribe : (obj -> unit) -> IDisposable

type IEventSource<'a> =
    inherit IEventSource
    abstract member Subscribe : ('a -> unit) -> IDisposable

module EventSource =
    type private Obs<'a>(o : IObservable<'a>) =
        interface IEventSource with
            member x.Subscribe cb = o.Subscribe(fun v -> cb (v :> obj))

        interface IEventSource<'a> with
            member x.Subscribe cb = o.Subscribe(fun v -> cb v)

    type private Mod<'a>(o : IMod<'a>) =
        interface IEventSource with
            member x.Subscribe cb = o |> Mod.unsafeRegisterCallbackKeepDisposable (fun v -> cb (v :> obj))

        interface IEventSource<'a> with
            member x.Subscribe cb = o  |> Mod.unsafeRegisterCallbackKeepDisposable (fun v -> cb v)



    let ofObservable (o : IObservable<'a>) =
        Obs<'a>(o) :> IEventSource<_>

    let ofMod (o : IMod<'a>) =
        Mod<'a>(o) :> IEventSource<_>

type IPattern =
    abstract member Relevant : PersistentHashSet<IEventSource>
    abstract member Run : IEventSource * obj-> Option<obj>

[<AbstractClass>]
type Pattern(relevant : PersistentHashSet<IEventSource>) =
    member x.Relevant = relevant
    abstract member Run : IEventSource * obj -> Option<obj>
        
    new(s : seq<IEventSource>) = Pattern(PersistentHashSet.ofSeq s)
    new(s : list<IEventSource>) = Pattern(PersistentHashSet.ofList s)

    interface IPattern with
        member x.Relevant = x.Relevant
        member x.Run(s,o) = x.Run(s,o)

type AnyPattern<'a>(e : IEventSource<'a>) =
    inherit Pattern([e :> IEventSource])
    override x.Run (s,v) =
        if Object.Equals(s,e) then Some v
        else None

type AmbPattern<'a, 'b>(a : Pattern, b : Pattern) =
    inherit Pattern(PersistentHashSet.union a.Relevant b.Relevant)
    override x.Run (s,v) =
        match a.Run(s,v) with
            | Some r -> Some (Choice<obj, obj>.Choice1Of2 r :> obj)
            | None -> 
                match b.Run(s,v) with
                    | Some r -> Some (Choice<obj, obj>.Choice2Of2 r :> obj)
                    | None -> None

type TimePattern private () =
    inherit Pattern(PersistentHashSet.empty)
    static let instance = TimePattern() :> Pattern
    static member Instance = instance
    
    override x.Run(s,v) =
        Some null

type NeverPattern private () =
    inherit Pattern(PersistentHashSet.empty)
    static let instance = NeverPattern() :> Pattern
    static member Instance = instance
    
    override x.Run(s,v) =
        None


type MultiDict<'k, 'v when 'k : equality>() =
    let store = Dictionary<'k, List<'v>>()

    member x.ContainsKey (key : 'k) =
        store.ContainsKey key

    member x.Add(key : 'k, value : 'v) =
        let list = 
            match store.TryGetValue key with
                | (true, l) -> l
                | _ ->
                    let list = List()
                    store.[key] <- list
                    list

        list.Add value

    member x.Keys = store.Keys

    interface System.Collections.IEnumerable with
        member x.GetEnumerator() = store.GetEnumerator() :> _

    interface System.Collections.Generic.IEnumerable<KeyValuePair<'k, List<'v>>> with
        member x.GetEnumerator() = store.GetEnumerator() :> _


type Runner<'s>(state : 's) =
    inherit Mod.AbstractMod<'s>()

    let subscriptions = Dictionary<IEventSource, IDisposable>()

    let mutable pending : MultiDict<Pattern, obj -> SF<'s, unit>> = MultiDict()
    let queue = Queue<IEventSource * obj * float>()
    let mutable state = { time = 0.0; state = state }
    let sw = System.Diagnostics.Stopwatch()
    do sw.Start()

    let mutable dependsOnTime = false


    let now () = sw.Elapsed.TotalSeconds

    override x.Compute() =
        x.Evaluate()
            
        if dependsOnTime then
            AdaptiveObject.Time.Outputs.Add x |> ignore
            
        state.state

    member private x.Push(source : IEventSource, value : obj) =
        queue.Enqueue(source, value, now())
        transact (fun () -> x.MarkOutdated())

    member x.Enqueue(sf : SF<'s, unit>) =
        lock x (fun () ->
            match sf.run.Run(&state) with
                | Finished _ -> ()
                | Continue(p, cont) ->
                    pending.Add(p, cont)

                    if p = TimePattern.Instance then
                        dependsOnTime <- true

                    for r in p.Relevant do
                        if not (subscriptions.ContainsKey r) then
                            subscriptions.[r] <- r.Subscribe(fun o -> x.Push(r, o))
        )
                            
    member private x.Step() =
        lock x (fun () ->
            while queue.Count > 0 do
                let (s,e,t) = queue.Dequeue()
                state <- { state with time = t }

                let mutable newPending = MultiDict()
                for KeyValue(p, conts) in pending do
                    match p.Run(s,e) with
                        | Some v -> 
                            for cont in conts do
                                let c = cont v
                                match c.run.Run(&state) with
                                    | Continue(p,c) ->
                                        newPending.Add(p, c)
                                    | _ ->
                                        ()
                        | None ->
                            for cont in conts do
                                newPending.Add(p,cont)

                let oldEvents   = pending.Keys |> Seq.collect (fun p -> p.Relevant) |> HashSet
                let newEvents   = newPending.Keys |> Seq.collect (fun p -> p.Relevant) |> HashSet
                let removed     = oldEvents |> Seq.filter (newEvents.Contains >> not) |> Seq.toList
                let added       = newEvents |> Seq.filter (oldEvents.Contains >> not) |> Seq.toList

                for a in added do
                    subscriptions.[a] <- a.Subscribe(fun o -> x.Push(a, o))

                for r in removed do
                    match subscriptions.TryGetValue r with
                        | (true, s) -> 
                            subscriptions.Remove r |> ignore
                            s.Dispose()
                        | _ -> ()

                pending <- newPending

            dependsOnTime <- pending.ContainsKey TimePattern.Instance

        )

    member x.Evaluate() =
        lock x (fun () ->
            queue.Enqueue(Unchecked.defaultof<_>, null, now ())
            x.Step()
        )

    member x.State = x :> IMod<_>



and SF<'s, 'a> = { run : State<EventState<'s>, Result<'s, 'a>> }

and Result<'s, 'a> =
    | Finished of 'a
    | Continue of Pattern * (obj -> SF<'s, 'a>)


module SF =
    let value (v : 'a) =
        { run = State.value (Finished v) }

    let ofEventSource (e : IEventSource<'a>) =
        { run = Continue(AnyPattern(e), fun v -> value (unbox<'a> v)) |> State.value }
        
    let ofObservable (o : IObservable<'a>) =
        o |> EventSource.ofObservable |> ofEventSource

    let ofMod (m : IMod<'a>) =
        m |> EventSource.ofMod |> ofEventSource

    let now<'s> : SF<'s, float> =
        { run = State.get |> State.map (fun s -> Finished s.time) }

    let nextTime<'s> : SF<'s, float> =
        { run = State.get |> State.map (fun s -> Continue(TimePattern.Instance,fun _ -> { run = State.get |> State.map (fun s -> Finished s.time) })) }

    let dt<'s> : SF<'s, float> =
        { run = 
            State.get |> State.map (fun s -> 
                let start = s.time
                Continue(TimePattern.Instance,fun _ -> { run = State.get |> State.map (fun s -> Finished (s.time - start)) })
            ) 
        }


    let rec map (f : 'a -> 'b) (m : SF<'s, 'a>) =
        { run =
            state {
                let! m = m.run
                match m with
                    | Finished v -> return v |> f |> Finished
                    | Continue(p, cont) -> return Continue(p, cont >> map f)
            }
        }
    
    let rec bind (f : 'a -> SF<'s, 'b>) (m : SF<'s, 'a>) =
        { run =
            state {
                let! r = m.run
                match r with
                    | Finished v -> return! f(v).run
                    | Continue(p,cont) -> return Continue(p, cont >> bind f)
            }
        }

    let rec amb (l : SF<'s, 'a>) (r : SF<'s, 'b>) =
        { run =
            state {
                let! l' = l.run
                let! r' = r.run
                match l', r' with
                    | Finished l,_ -> return Finished (Choice1Of2 l)
                    | _, Finished r -> return Finished (Choice2Of2 r)
                    | Continue(lp, lcont), Continue(rp, rcont) ->
                        return Continue(AmbPattern(lp, rp), fun o ->
                            match unbox<Choice<obj, obj>> o with
                                | Choice1Of2 l -> amb (lcont l) r
                                | Choice2Of2 r -> amb l (rcont r)
                        )
                            
            }
        }

    let guarded (guard : SF<'s, 'a>) (inner : SF<'s, 'b>) =
        let cancelOrM = amb guard inner

        let rec guarded (m : SF<'s, Choice<'a, 'b>>) =
            { run =
                state {
                    let! r = m.run
                    match r with
                        | Finished v ->
                            match v with
                                | Choice1Of2 _ -> return Finished None
                                | Choice2Of2 v -> return Finished (Some v)
                    
                        | Continue(p,c) ->
                            return Continue(p, c >> guarded)
                }
            }

        guarded cancelOrM 

    let rec append (l : SF<'s, unit>) (r : SF<'s, 'a>) =
        { run =
            state {
                let! res = l.run
                match res with
                    | Finished v -> return! r.run
                    | Continue(p,cont) -> return Continue(p, fun o -> append (cont o) r)
            }
        }

    let inline ignore (m : SF<'s, 'a>) =
        map ignore m

    let repeatUntil (guard : SF<'s, 'a>) (body : SF<'s, unit>) =
        let cancelOrM = amb guard body

        let rec repeatUntil (m : SF<'s, Choice<'a, unit>>) =
            { run =
                state {
                    let! r = m.run
                    match r with
                        | Finished v ->
                            match v with
                                | Choice1Of2 a -> return Finished ()
                                | Choice2Of2 () -> return! repeatUntil(cancelOrM).run

                        | Continue(p, c) ->
                            return Continue(p, c >> bind (function Choice1Of2 _ -> value () | _ -> repeatUntil cancelOrM))

                }
            }

        repeatUntil cancelOrM

    let rec repeatWhile (guard : SF<'s, bool>) (body : SF<'s, unit>) =
        { run =
            state {
                let! g = guard.run
                match g with
                    | Finished true ->
                        let! r = body.run
                        match r with
                            | Finished () -> return! (repeatWhile guard body).run

                            | Continue(p, c) ->
                                return Continue(p, fun o -> append (c o) (repeatWhile guard body))
                    | _ ->
                        return Finished ()

            }
        }


    let rec repeat (inner : SF<'s, unit>) =
        { run =
            state {
                let! v = inner.run
                match v with
                    | Finished () -> return! repeat(inner).run
                    | Continue(p,cont) -> return Continue(p, fun o -> append (cont o) (repeat inner))
            }
        }

    let never<'s, 'a> : SF<'s, 'a> =
        { run =
            state {
                return Continue(NeverPattern.Instance, fun _ -> failwith "")
            }
        }




type Pattern<'s, 'a> =
    | Not of SF<'s, 'a>


[<AutoOpen>]
module ``SF Builders`` =
    type SFBuilder() =

        member x.Bind(m : IObservable<'a>, f : 'a -> SF<'s, 'b>) =
            SF.bind f (SF.ofObservable m)

        member x.Bind(m : SF<'s, 'a>, f : 'a -> SF<'s, 'b>) =
            SF.bind f m

        member x.Bind(m : State<'s, 'a>, f : 'a -> SF<'s, 'b>) =
            SF.bind f { run = m |> EventState.lift |> State.map Finished }

//        member x.Bind(m : Value<'s, 'a>, f : 'a -> SF<'s, 'b>) =
//            SF.bind f { run = m.eval |> State.map Finished }

        member x.Return(v : 'a) = 
            SF.value v

        member x.ReturnFrom(sf : SF<'s, 'a>) =
            sf

        member x.While(guard : unit -> Pattern<'s, 'a>, body : SF<'s, unit>) =
            match guard() with
                | Not cancel -> SF.repeatUntil cancel body

        member x.While(guard : unit -> bool, body : SF<'s, unit>) =
            SF.repeatWhile { run = state { return guard() |> Finished } } body

//        member x.While(guard : unit -> Value<'s, bool>, body : SF<'s, unit>) =
//            SF.repeatWhile { eval = state { return! guard().eval }} body


        member x.Delay(f : unit -> SF<'s, 'a>) =
            { run =
                state {
                    return! f().run
                }
            }


        member x.Combine(l : SF<'s, unit>, r : SF<'s, 'a>) =
            SF.append l r

        member x.Zero() = SF.value ()

    let sf = SFBuilder()


module Test =
    open System.Drawing
    open System.Drawing.Drawing2D
    open System.Windows.Forms
    

    type SketchState =
        {
            current : list<V2i>
            finished : list<list<V2i>>
        }

    let append (v : V2i) =
        State.modify (fun s -> { s with current = s.current @ [v] } )

    let emit (v : list<V2i>) =
        State.modify (fun s -> { s with current = []; finished = s.finished @ [v] } )

    let clear =
        State.modify (fun s -> { s with current = [] } )


    let paintThingy (down : IObservable<MouseEventArgs>) (up : IObservable<MouseEventArgs>) (move : IObservable<MouseEventArgs>) =
        sf {
            let! d = down
            let pd = V2i(d.X, d.Y)
            do! append pd

            let mutable res = [pd]
            while Not (SF.ofObservable up) do
                let! m = move
                let pm = V2i(m.X, m.Y)
                do! append pm
                res <- res @ [pm]
                
            do! clear
            return res

        }

    let paintManyThings (down : IObservable<MouseEventArgs>) (up : IObservable<MouseEventArgs>) (move : IObservable<MouseEventArgs>) =
        sf {
            while true do
                let! a = paintThingy down up move
                do! emit a
        }

    let rec printTime (sumOfDeltas : float) : SF<'s, unit> =
        sf {
            let! t = SF.now
            let! dt = SF.dt
            printfn "%.4f: %.3fms" t (1000.0 * dt)
            if t < 3.0 then 
                return! printTime (sumOfDeltas + dt)
            else
                let err = abs(sumOfDeltas - t)
                if err = 0.0 then printfn "no error"
                else printfn "error: %.8fms" (1000.0 * abs(sumOfDeltas - t))
        }

    module UI =
        type Graphics with
            member x.Line(color : Color, ps : list<V2i>) =
                match ps with
                    | [] | [_] -> ()
                    | _ ->
                        use p = new Pen(color, 3.0f)
                        x.DrawLines(p, ps |> List.map (fun p -> Point(p.X, p.Y)) |> List.toArray)

            member x.Polygon(color : Color, ps : list<V2i>) =
                match ps with
                    | [] | [_] -> ()
                    | _ ->            
                        use p = new HatchBrush(HatchStyle.DottedGrid, color)
                        x.FillPolygon(p, ps |> List.map (fun p -> Point(p.X, p.Y)) |> List.toArray)

        type PaintForm(m : IMod<SketchState>) as this =
            inherit Form()
            do base.SetStyle(ControlStyles.DoubleBuffer, true)
               base.SetStyle(ControlStyles.AllPaintingInWmPaint, true)

            let subscription =
                m.AddMarkingCallback(fun () ->
                    this.BeginInvoke(new System.Action(fun () -> this.Invalidate())) |> ignore
                )

            override x.Dispose(d) =
                base.Dispose(d)
                subscription.Dispose()

            override x.OnPaintBackground(e) =
                ()

            override x.OnPaint(e) =
                e.Graphics.SmoothingMode <- SmoothingMode.AntiAlias

                let s = m.GetValue()
                let g = e.Graphics
                g.Clear(Color.Black)


                for p in s.finished do
                    g.Polygon(Color.Green, p)

                g.Line(Color.Red, s.current)

    let run() =
        let runner = Runner { current = []; finished = [] }
        use form = new UI.PaintForm(runner.State)


        let thing = paintManyThings form.MouseDown form.MouseUp form.MouseMove
        runner.Enqueue thing
        runner.Enqueue (printTime 0.0) //(printTime 0.0)


        Application.Run(form)
