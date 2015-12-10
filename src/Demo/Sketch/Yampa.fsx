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


module Yampa =
    type SF<'s, 'a, 'b> = { run : 's -> 'a -> 's * 'b }

    module SF =
        let arr (f : 'a -> 'b) =
            { run = fun s a -> s,f a }

        let compose(g : SF<'s, 'b, 'c>) (f : SF<'s, 'a, 'b>) =
            { run = fun s a ->
                let (s,b) = f.run s a
                g.run s b
            }

        let first (f : SF<'s, 'a, 'b>) : SF<'s, 'x * 'a, 'x * 'b> =
            { run = fun s (x,a) ->
                let (s,b) = f.run s a
                (s, (x,b))
            }

        let second (f : SF<'s, 'a, 'b>) : SF<'s, 'a * 'x, 'b * 'x> =
            { run = fun s (a,x) ->
                let (s,b) = f.run s a
                (s, (b,x))
            }

        let map (f : 'b -> 'c) (m : SF<'s, 'a, 'b>) =
            { run = fun s v ->
                let (s,r) = m.run s v
                (s, f r)
            }

        let par (f : SF<'s, 'a, 'b>) (g : SF<'s, 'a, 'c>) =
            { run = fun s v ->
                let (s, b) = f.run s v
                let (s, c) = g.run s v

                (s, (b,c))

            }

        let zip (f : SF<'s, 'a, 'b>) (g : SF<'s, 'x, 'y>) =
            { run = fun s (a,x) ->
                let (s,b) = f.run s a
                let (s,y) = g.run s x
                (s,(b,y))
            }


module FrpModEvents =

    open Aardvark.Base
    open Aardvark.Base.Incremental

    [<ReferenceEquality;NoComparison>]
    type Token = Token of IAdaptiveObject

    let time = Token null
    let mutable executionPending = 1

    type private ObservableMod(input : IMod, emitFirstValue : bool) =
        inherit Mod.AbstractMod<obj>()

        let mutable subscriptionCount = 0
        let subscriptions = HashSet<IObserver<obj>>()

        interface IObservable<obj> with
            member x.Subscribe(obs : IObserver<obj>) =
                Interlocked.Increment &subscriptionCount |> ignore

                // force the evaluation (in order to see subsequent markings)
                let v = x.GetValue null
                if emitFirstValue then
                    obs.OnNext v

                lock subscriptions (fun () -> subscriptions.Add obs |> ignore )
                { new IDisposable with 
                    member x.Dispose() =
                        Interlocked.Decrement &subscriptionCount |> ignore
                        lock subscriptions (fun () -> subscriptions.Remove obs |> ignore)
                }

        override x.Mark() =
            if subscriptionCount > 0 then
                let v = x.GetValue null
                let all = lock subscriptions (fun s -> subscriptions.ToArray())
                for s in all do s.OnNext v

            true

        override x.Compute() =
            input.GetValue x


    type IContext =
        abstract member ConsumeValue : Token -> Option<'a>

    open System.Collections.Concurrent

    type Context<'a, 'b>(sf : ISF<'a,'b>, initial : 'a, output : 'b -> unit) =
        inherit AdaptiveObject()
        let buffer = ConcurrentDict<Token, ConcurrentQueue<obj>>(Dict())
        let buffers = Dictionary<Token,IDisposable>()

        let mutable sf = sf
        let mutable oldDeps = HashSet()

        let ev = new System.Threading.ManualResetEventSlim(true)

        let isDone () =
            buffer.Count = 0

        let mutable iterations = 1

        member x.ShouldProcess = ev

        member x.NextFixPoint() =
            
            //printfn "running iteration: %A" iterations
            x.EvaluateAlways null (fun () ->
                let newDeps = sf.Deps |> HashSet 
                let added = newDeps |> Seq.filter (oldDeps.Contains >> not)
                let removed = oldDeps |> Seq.filter (newDeps.Contains >> not)
                for a in added do x.Listen a
                for r in removed do x.UnListen r
                
                let sfNew,b = sf.Run(x,initial)
                output b
                sf <- sfNew
                oldDeps <- newDeps

                if isDone () then 
                    Interlocked.Exchange(&executionPending, 0) |> ignore
                    ev.Reset()
            )
            
            //printfn "done iteration: %A" iterations
            iterations <- iterations + 1

        member x.Listen(Token(o) as token) =
            match o with 
             | null -> 
                buffer.GetOrCreate(token, fun _ -> ConcurrentQueue()) |> ignore

             | :? IMod as m -> 
                let obs = ObservableMod(m, true)
                
                let queue() = buffer.GetOrCreate(token, fun _ -> ConcurrentQueue())
                let d = obs.Subscribe(fun v -> 
                    queue().Enqueue v; 
                    let pending = Interlocked.Exchange(&executionPending, 1)
                    if pending = 0 then
                        transact (fun () -> x.MarkOutdated())
                )
                buffers.Add(token, d) |> ignore
             | _ -> 
                failwith""

        member x.UnListen(Token(o) as token) =
            match o with 
             | null -> ()
             | _ -> 
                o.Outputs.Remove x |> ignore
                buffers.[token].Dispose()
                buffer.Remove token |> ignore
                buffers.Remove token |> ignore
                
        member x.ConsumeValue (t : Token) : Option<'x> =
            match buffer.TryGetValue t with
             | (true,v) ->
                let res = 
                    match v.TryDequeue() with
                        | (true, v) -> v |> unbox<'x> |> Some
                        | _ -> None

                if v.Count = 0 then buffer.Remove t |> ignore

                res

             | _ -> 
                None

        override x.Mark() = 
            ev.Set()
            false

        interface IContext with
            member x.ConsumeValue(t : Token) : Option<'x> = x.ConsumeValue t


    and ISF<'a,'b> =
        abstract member Run : IContext * 'a -> ISF<'a,'b> * 'b
        abstract member Deps : list<Token>

    type SF<'a,'b>(f : IContext * 'a -> ISF<'a,'b> * 'b, deps : list<Token>) =
        interface ISF<'a,'b> with
            member x.Run (ctx,v) = f (ctx,v)
            member x.Deps = deps

    type Event<'a> = Event of 'a | NoEvent

    type Ev<'a,'b>(o : IMod<'b>) =
        let t = Token o
        interface ISF<'a,Event<'b>> with
            member x.Run (ctx,_) = 
                let r = ctx.ConsumeValue t
                let self = x :> ISF<'a,Event<'b>>
                match r with 
                 | Some v -> self, Event v
                 | None -> self, NoEvent
            member x.Deps = [t]

    let ev o = Ev<_,_>(o) :> ISF<_,_>
    let sf f d = SF<_,_>(f,d) :> ISF<_,_>

    let arr f = 
        let self = ref Unchecked.defaultof<_>
        self := sf (fun v -> !self, f v) []
        !self

    let arrU d f = 
        let self = ref Unchecked.defaultof<_>
        self := sf (fun (_,v) -> !self, f v) d
        !self

    let rec (>>>) (a:ISF<'a,'b>) (b:ISF<'b,'c>) : ISF<'a,'c> = 
        let f (ctx, v) = 
            let c, r1 = a.Run(ctx, v)
            let d, r2 = b.Run(ctx, r1)
            c >>> d, r2
        sf f (a.Deps @ b.Deps)

    let click = Mod.init 1111
    let clickEv : ISF<unit,Event<int>> = ev click

    let nowTime = arrU [Token null] (fun e -> e,DateTime.Now)

    let urdar = clickEv >>> nowTime

    let start (init : unit -> 'a) 
              (output : 'b -> unit) 
              (sfT : ISF<'a,'b>) =
        

        let ctx = Context(sfT, init(), output)
        ctx.NextFixPoint()

        let t = System.Threading.Thread(ThreadStart(fun _ -> 
            while true do
                ctx.ShouldProcess.Wait()
                ctx.NextFixPoint()
        ))
        t.IsBackground <- true
        t.Start()

        t

    let toMod (sf : SF<'a,'b>) (v : 'a) =
        let r = Mod.init Unchecked.defaultof<_>
        let ctx = Context(sf, v, (fun v -> Mod.change r v))
        ctx.NextFixPoint()
        let t = System.Threading.Thread(ThreadStart(fun _ -> 
            while true do
                ctx.ShouldProcess.Wait()
                ctx.NextFixPoint()
        ))
        t.IsBackground <- true
        t.Start()
        r :> IMod<_>

//    let rec flatten (f : ISF<'x, ISF<'x, 'y>>) : ISF<'x, 'y> =
//        let run (ctx,x) =
//            let cont1,inner = f.Run(ctx,x)
//            let cont, v = inner.Run(ctx,x)
//            failwith ""
//        failwith ""
//
//    let sepp (i : ISF<'x,'a>) (f : 'a -> ISF<'x, 'y>) : ISF<'x, 'y> =
//        failwith ""

    let test () =
        let t = start (constF ()) (fun (e,v) -> match e with | Event(_) as v -> printfn "%A" v | _ -> ()) urdar

        for i in 0 .. 5 do
            transact (fun () -> Mod.change click i)
        
        ()
