#I @"..\..\..\bin\Release"
#I @"..\..\..\Packages\Rx-Core\lib\net45"
#I @"..\..\..\Packages\Rx-Interfaces\lib\net45"
#I @"..\..\..\Packages\Rx-Linq\lib\net45"
#r "Aardvark.Base.dll"
#r "Aardvark.Base.Essentials.dll"
#r "Aardvark.Base.TypeProviders.dll"
#r "Aardvark.Base.FSharp.dll"
#r "Aardvark.Base.Incremental.dll"
#r "System.Reactive.Core.dll"
#r "System.Reactive.Interfaces.dll"
#r "System.Reactive.Linq.dll"

open System
open System.Threading
open System.Linq
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental
open System.Reactive
open System.Reactive.Linq



type IBuffer =
    inherit IAdaptiveObject
    inherit IDisposable
    abstract member IsEmpty : bool

type IBuffer<'a> =
    inherit IBuffer
    abstract member Dequeue : IAdaptiveObject -> Option<'a>


type Token =
    | Time
    | Reactive of (unit -> IBuffer)

type Context(caller : IAdaptiveObject) =
    static let startTicks = DateTime.Now.Ticks
    let buffers = Dict<Token, IBuffer>()
    let mutable containsTime = false
    let mutable timePulled = false
    let mutable lastTime = DateTime.MinValue
    let mutable timeIndex = 0
    member x.Caller = caller

    member x.Restart() =
        timePulled <- false

    member x.TimePulled =
        timePulled

    member x.IsEmpty =
        lock buffers (fun () -> buffers.Values |> Seq.forall (fun b -> b.IsEmpty))


    member x.Listen(t : Token) =
        match t with
            | Time -> 
                containsTime <- true
            | Reactive create ->
                lock buffers (fun () -> buffers.[t] <- create())

    member x.Unlisten(t : Token) =
        match t with
            | Time ->
                containsTime <- false
            | Reactive _ ->
                match lock buffers (fun () -> buffers.TryRemove t) with
                    | (true, b) -> b.Dispose()
                    | _ -> ()

    member x.DequeValue<'a>(t : Token) : Option<'a> =
        match t with
            | Time ->
                let now = DateTime.Now

                let ticks = 
                    if now = lastTime then
                        let ti = Interlocked.Increment(&timeIndex)
                        float (now.Ticks - startTicks) + (float ti * (1.0 / 4000.0))
                    else
                        timeIndex <- 0
                        lastTime <- now
                        float (now.Ticks - startTicks)

                timePulled <- true
                (float ticks / float TimeSpan.TicksPerSecond) |> unbox |> Some
            | _ ->
                match lock buffers (fun () -> buffers.TryGetValue t) with
                    | (true, (:? IBuffer<'a> as b)) ->
                        b.Dequeue caller
                    | _ ->
                        None



type Event<'a> =
    | Event of 'a
    | NoEvent

type sf<'a, 'b> =
    abstract member run : Context -> 'a -> 'b * sf<'a, 'b>
    abstract member dependencies : list<Token>

module SF =

    exception private EndExn

    type private ModBuffer<'a>(m : IMod<'a>) as this =
        inherit AdaptiveObject()

        let mutable live = 1
        let q = Queue<'a> [m.GetValue this]
        

        override x.Mark() =
            if live = 1 then
                let v = m.GetValue x
                q.Enqueue v

            true

        member x.Dispose() =
            let wasLive = Interlocked.Exchange(&live,0)
            if wasLive = 1 then
                lock x (fun () ->
                    m.RemoveOutput x
                    q.Clear()
                )


        member x.IsEmpty = lock x (fun () -> q.Count = 0)

        member x.Dequeue(caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                if live = 1 then 
                    if q.Count = 0 then None
                    else q.Dequeue() |> Some
                else 
                    None
            )

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IBuffer with
            member x.IsEmpty = x.IsEmpty

        interface IBuffer<'a> with
            member x.Dequeue caller = x.Dequeue caller

    type private ObservableBuffer<'a>(o : IObservable<'a>) as this =
        inherit AdaptiveObject()

        let mutable live = 1
        let mutable markingPending = 0
        let q = Queue<'a>()



        let push (v : 'a) =
            lock this (fun () ->
                q.Enqueue v
            )

            let alreadyMarking = Interlocked.Exchange(&markingPending, 1)
            if alreadyMarking = 0 then
                transact (fun () -> this.MarkOutdated())

        let sub = o.Subscribe push

        member x.Dispose() =
            let wasLive = Interlocked.Exchange(&live,0)
            if wasLive = 1 then
                sub.Dispose()
                lock x (fun () -> q.Clear())


        member x.IsEmpty = lock q (fun () -> q.Count = 0)

        member x.Dequeue(caller : IAdaptiveObject) =
            x.EvaluateAlways caller (fun () ->
                if live = 1 then 
                    if q.Count = 0 then None
                    else q.Dequeue() |> Some
                else 
                    None
            )

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IBuffer with
            member x.IsEmpty = x.IsEmpty

        interface IBuffer<'a> with
            member x.Dequeue caller = x.Dequeue caller



    type private ModSignal<'a, 'b>(e : IMod<'b>) =
        let token = Reactive (fun () -> new ModBuffer<'b>(e) :> IBuffer)

        member x.run (ctx : Context) (i : 'a) =
            match ctx.DequeValue<'b> token with
                | Some b -> Event b, x :> sf<_,_>
                | None -> NoEvent, x :> sf<_,_>

        interface sf<'a, Event<'b>> with
            member x.run ctx i = x.run ctx i
            member x.dependencies = [token]
               
    type private EventSignal<'a, 'b>(e : IObservable<'b>) =
        let token = Reactive (fun () -> new ObservableBuffer<'b>(e) :> IBuffer)     

        member x.run (ctx : Context) (i : 'a) =
            match ctx.DequeValue<'b> token with
                | Some b -> Event b, x :> sf<_,_>
                | None -> NoEvent, x :> sf<_,_>

        interface sf<'a, Event<'b>> with
            member x.run ctx i = x.run ctx i
            member x.dependencies = [token]
         
    type private SimpleSignal<'a, 'b>(f : 'a -> 'b) =
        interface sf<'a, 'b> with
            member x.run _ i = f i, x :> sf<_,_>
            member x.dependencies = []
      
    type private ConstantSignal<'a, 'b>(value : 'b) =
        interface sf<'a, 'b> with
            member x.run _ _ = value, x :> sf<_,_>
            member x.dependencies = []

    type private CustomSignal<'a, 'b>(f : sf<'a,'b> -> Context -> 'a -> 'b * sf<'a, 'b>, deps : list<Token>) as this =
        let f = f this
        interface sf<'a, 'b> with
            member x.run ctx v = f ctx v
            member x.dependencies = deps
     
    type private EndSignal<'a, 'b> private() =
        static let instance = EndSignal<'a,'b>() :> sf<'a, 'b>
        
        static member Instance = instance


        interface sf<'a, 'b> with
            member x.run _ _ = raise EndExn
            member x.dependencies = []   

    type private TimeSignal<'a> private() =
        static let instance = TimeSignal<'a>() :> sf<'a, float>
        let token = Time

        static member Instance = instance

        interface sf<'a, float> with
            member x.run ctx _ = ctx.DequeValue<float>(token).Value, instance
            member x.dependencies = [Time]

   



    let time<'a> = TimeSignal<'a>.Instance

    let stop<'a, 'b> = EndSignal<'a, 'b>.Instance

    let ofMod (m : IMod<'a>) : sf<'x, Event<'a>> =
        ModSignal(m) :> _



    let ofObservable (m : IObservable<'a>) : sf<'x, Event<'a>> =
        EventSignal(m) :> _

    let arr (f : 'a -> 'b) : sf<'a, 'b> =
        SimpleSignal(f) :> _

    let constant (v : 'b) : sf<'a, 'b> =
        ConstantSignal(v) :> _

    let custom (dependencies : list<Token>) (f : sf<'a, 'b> -> Context -> 'a -> 'b * sf<'a, 'b>) : sf<'a, 'b> =
        CustomSignal(f, dependencies) :> _

    let rec mapOut (f : 'b -> 'c) (sf : sf<'a, 'b>) =
        custom sf.dependencies (fun self ctx a ->
            let (b, cont) = sf.run ctx a
            (f b), mapOut f cont
        )

    let rec mapIn (f : 'a -> 'b) (sf : sf<'b, 'c>) =
        custom sf.dependencies (fun self ctx a ->
            let (b, cont) = sf.run ctx (f a)
            b, mapIn f cont
        )

    let rec switch (guard : sf<'a, Event<'b>>) (onEvent : sf<'b, 'c>) (noEvent : sf<'a, 'c>) : sf<'a, 'c> =
        let deps = List.concat [guard.dependencies; onEvent.dependencies; noEvent.dependencies]
        custom deps (fun _ ctx a ->
            let (gv, gc) = guard.run ctx a
            match gv with
                | Event b ->
                    let (ev, ec) = onEvent.run ctx b
                    ev, switch gc ec noEvent

                | NoEvent ->
                    let (nv, nc) = noEvent.run ctx a
                    nv, switch gc onEvent nc
        )

    let rec switch2 (guard : sf<'x, Event<'b>>) (onEvent : 'b -> sf<'x, 'c>) (noEvent : sf<'x, 'c>) : sf<'x, 'c> =
        let deps = List.concat [guard.dependencies; onEvent.dependencies; noEvent.dependencies]
        custom deps (fun _ ctx a ->
            let (gv, gc) = guard.run ctx a
            match gv with
                | Event b ->
                    let (ev, ec) = onEvent(b).run ctx a
                    ev, switch2 gc (fun _ -> ec) noEvent

                | NoEvent ->
                    let (nv, nc) = noEvent.run ctx a
                    nv, switch2 gc onEvent nc
        )


    let rec fold (seed : 's) (acc : 's -> 'a -> 's) (m : sf<'x, 'a>) =
        custom m.dependencies (fun _ ctx x ->
            let (mv,mc) = m.run ctx x
            let s = acc seed mv
            s, fold s acc mc
        )

    let rec zip (l : sf<'a, 'b>) (r : sf<'a, 'c>) =
        custom (l.dependencies @ r.dependencies) (fun self ctx a ->
            let (lv, lc) = l.run ctx a
            let (rv, rc) = r.run ctx a

            (lv,rv), zip lc rc

        )

    let inline integrate (f : sf<'a, 'b>) =
        f |> fold LanguagePrimitives.GenericZero (+)

    let inline differentiate (y : sf<'a, 'y>) (x : sf<'a, 'x>) =
        zip y x 
            |> fold None (fun last (y,x) ->
                match last with
                    | Some (_, oy,ox) ->
                        let v = (y - oy) / (x - ox)
                        Some(v, y, x)
                    | None ->
                        Some(LanguagePrimitives.GenericZero, y, x)
               ) 
            |> mapOut (fun o ->
                match o with
                    | Some(v,_,_) -> v
                    | _ -> LanguagePrimitives.GenericZero
               )


    let switchTrue (guard : sf<'a, bool>) (ifTrue : sf<'a, 'b>) (ifFalse : sf<'a, 'b>) =
        let rec run (currentlyTrue : Option<bool>) (guard : sf<'a, bool>) (ifTrue : sf<'a, 'b>) (ifFalse : sf<'a, 'b>) =
            let deps = 
                match currentlyTrue with
                    | Some true -> guard.dependencies @ ifTrue.dependencies
                    | Some false -> guard.dependencies @ ifFalse.dependencies
                    | None -> List.concat [guard.dependencies; ifTrue.dependencies; ifFalse.dependencies]

            custom deps (fun _ ctx a ->
                let (gv, gc) = guard.run ctx a
                if gv then
                    let (ev, ec) = ifTrue.run ctx a
                    ev, run (Some true) gc ec ifFalse

                else
                    let (nv, nc) = ifFalse.run ctx a
                    nv, run (Some false) gc ifTrue nc
            )
 
        run None guard ifTrue ifFalse

    let bind (f : 'a -> sf<'x, 'y>) (m : IMod<'a>) : sf<'x, 'y> =
        let rec run (oldA : Option<'a * sf<'x, 'y>>) =
            let deps =
                match oldA with 
                    | Some(_,old) -> old.dependencies
                    | None -> []

            custom deps (fun self ctx x ->
                let a = m.GetValue ctx.Caller
                match oldA with
                    | Some(oa,old) when System.Object.Equals(oa, a) ->
                        let (v,cont) = old.run ctx x
                        v, run (Some(a, cont))
                    | _ ->
                        let inner = f a
                        let (y, cont) = inner.run ctx x

                        y, run (Some(a, cont))

            )


        run None 

    let latest (m : IMod<'a>) : sf<'x, 'a> =
        custom [] (fun self ctx _ ->
            let latestValue = m.GetValue ctx.Caller

            latestValue, self
        )

    let withCancellation (ct : CancellationToken) (inner : sf<'a, 'b>) : sf<'a, 'b> =
        let m = Mod.init false
        let reg = ct.Register(fun () -> transact (fun () -> Mod.change m true))

        switchTrue (latest m)
            (stop |> mapIn (fun _ -> reg.Dispose()))
            inner

    //loop :: a (b, d) (c, d) -> a b c

    //loop : sf<'b * 'd, 'c * 'd> -> sf<'b, 'c>
    // sf<a * c, b * c> -> sf<a, b>


    type private Reactimator<'a, 'b>(sf : sf<'a, 'b>, input : 'a, output : 'b -> unit) as this =
        inherit AdaptiveObject()

        let ctx = Context(this)
        let evt = new ManualResetEventSlim(true)
        let mutable initial = true
        let mutable oldValue = None
        let mutable current = sf
        let mutable currentDeps =
            let res = sf.dependencies |> HashSet
            for a in res do ctx.Listen a
            res

        member x.StepEvent = evt

        member x.Step() =
            x.EvaluateAlways null (fun () ->
                
                ctx.Restart()

                let rec run (top : bool) (latest : Option<'b>) =
                    if not top && ctx.IsEmpty then
                        if not ctx.TimePulled then
                            match oldValue, latest with
                                | Some o, Some l  when System.Object.Equals(o,l) ->
                                    evt.Reset()
                                | None, None ->
                                    evt.Reset()
                                | _ -> ()

                        oldValue <- latest
                        true
                    else
                        let res = 
                            try current.run ctx input |> Some
                            with EndExn -> None
                    
                        match res with
                            | Some(v, newSf) ->
                                output v

                        
                                let newDeps = newSf.dependencies |> HashSet

                                let added = newDeps |> Seq.filter (currentDeps.Contains >> not)
                                let removed = currentDeps |> Seq.filter (newDeps.Contains >> not)

                                for r in removed do ctx.Unlisten r
                                for a in added do ctx.Listen a


                                current <- newSf
                                currentDeps <- newDeps
                                run false (Some v)

                            | None ->
                                // todo: cleanup
                                false

                run true oldValue
            )

        override x.Mark() =
            evt.Set()
            true

    let reactimate (output : 'b -> unit) (sf : sf<unit, 'b>) =
        
        let r = Reactimator(sf, (), output)
        if r.Step() then

            let rec run() =
                async {
                    do! Async.SwitchToThreadPool()

                    let! worked = Async.AwaitWaitHandle r.StepEvent.WaitHandle

                    if not worked then
                        printfn "reactimate failed"
                    else
                        if r.Step() then
                            return! run()
                        else
                            printfn "finished"

                }

            run()
        else
            async { return () }

    let flatten (f : 'a -> sf<'x, 'y>) : sf<'a * 'x, 'y> =
        let rec run (old : Option<sf<'x, 'y>>) =
            let deps = 
                match old with
                    | Some old -> old.dependencies
                    | _ -> []
            
            custom deps (fun self ctx (a,x) ->
                let(y,cont) = (f a).run ctx x
                y, run (Some cont)
            )

        run None


[<AutoOpen>]
module Operators =

    type SFBuilder() =
        member x.Bind(m : IMod<'a>, f : 'a -> sf<'x, 'y>) =
            m |> SF.bind f

        member x.For(o : IMod<'a>, f : 'a -> sf<'x, 'x>) =
            SF.switch2 (SF.ofMod o)
                (SF.flatten f)
                (SF.arr id)

        member x.For(o : IObservable<'a>, f : 'a -> sf<'x, 'x>) =
            SF.switch2 (SF.ofObservable o)
                (SF.flatten f)
                (SF.arr id)

        member x.Return(f : 'a -> 'b) =
            SF.arr f

        member x.ReturnFrom(f : sf<'a, 'b>) =
            f

        member x.Yield(f : 'a -> 'b) =
            SF.arr f

        member x.Zero() = SF.arr id

    let sf = SFBuilder()


    let test (active : IMod<bool>) (clicks : IObservable<V2i>) =
        sf {
            let! a = active
            if a then
                for p in clicks do
                    if p.X > 1000 then
                        return! SF.stop
                    else
                        return fun (l : list<V2i>) ->
                            p::l
        }


    let rec (>>>) (l : sf<'a, 'b>) (r : sf<'b, 'c>) : sf<'a, 'c> =
        SF.custom (l.dependencies @ r.dependencies) (fun self ctx a ->
            let (lv, lc) = l.run ctx a
            let (rv, rc) = r.run ctx lv
            rv, lc >>> rc
        )

    let (<<<) (r : sf<'b, 'c>) (l : sf<'a, 'b>) : sf<'a, 'c> =
        l >>> r

    let (||>) (l : sf<'a, 'b>) (r : 'b -> 'c) : sf<'a, 'c> =
        l |> SF.mapOut r

    let (<||) (r : 'b -> 'c) (l : sf<'a, 'b>) : sf<'a, 'c> =
        l |> SF.mapOut r




let test () = 
    let step = Mod.init 0

    // whenever we have a step use it otherwise step by 0
    let delta = 
        SF.switch (SF.ofMod step) 
            (SF.arr (fun v -> [v]))
            (SF.constant [])

    // integrate all steps
    let res = 
        delta 
            |> SF.fold [] (fun l e -> l @ e)
            ||> List.rev
            

    let cts = new CancellationTokenSource()


    // start the sf as task
    res
        |> SF.withCancellation cts.Token
        |> SF.reactimate (printfn "out: %A") 
        |> Async.Start

    


    transact(fun () -> Mod.change step 10)

    let mutable finish = false
    while not finish do
        let l = Console.ReadLine()

        match Int32.TryParse l with
            | (true, i) -> 
                transact(fun () -> Mod.change step i)
            | _ -> 
                if l.StartsWith "c" then 
                    cts.Cancel()
                elif l.StartsWith "q" then 
                    finish <- true
                else ()

    ()

let test2 () = 
    let active = Mod.init false
    let noStep = Mod.init 0.0

    let stepper = SF.constant 1.0 |> SF.integrate
    let dt = SF.differentiate SF.time stepper

//    let delta =
//        SF.switchTrue (SF.latest active)
//            (dt ||> fun v -> if v > 0.1 then 0.0 else v)
//            (SF.constant 0.0)

    let delta =
        active |> SF.bind (fun v ->
            if v then 
                dt ||> fun v -> if v > 0.1 then 0.0 else v
            else 
                SF.constant 0.0
        )

    let result = SF.integrate delta


    let cts = new CancellationTokenSource()


    // start the sf as task
    result 
        |> SF.withCancellation cts.Token
        |> SF.reactimate (printfn "out: %A") 
        |> Async.Start


    while true do
        Console.ReadLine() |> ignore
        transact(fun () -> Mod.change active (not active.Value))