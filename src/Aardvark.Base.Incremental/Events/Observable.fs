namespace Aardvark.Base.Incremental

open System
open System.Linq
open System.Threading
open System.Reactive.Linq
open System.Reactive.Subjects
open System.Collections.Generic
open Aardvark.Base

module Obs =
    let private noDisposable = { new IDisposable with member x.Dispose() = () }

    type private ConstantObservable<'a>(values : list<'a>) =
        interface IObservable<'a> with
            member x.Subscribe(obs : IObserver<'a>) =
                for v in values do obs.OnNext v
                obs.OnCompleted()
                noDisposable


    type private ObservableMod<'a>(input : IMod<'a>, emitFirstValue : bool) =
        inherit Mod.AbstractMod<'a>()

        let mutable subscriptionCount = 0
        let subscriptions = HashSet<IObserver<'a>>()

        interface IObservable<'a> with
            member x.Subscribe(obs : IObserver<'a>) =
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

    type private ObservableNextValue<'a>(input : IMod<'a>) =
        inherit Mod.AbstractMod<'a>()


        let mutable subscriptionCount = 0
        let subscriptions = HashSet<IObserver<'a>>()

        interface IObservable<'a> with
            member x.Subscribe(obs : IObserver<'a>) =
                Interlocked.Increment &subscriptionCount |> ignore

                // force the evaluation (in order to see subsequent markings)
                let v = x.GetValue null

                lock subscriptions (fun () -> subscriptions.Add obs |> ignore )
                { new IDisposable with 
                    member x.Dispose() =
                        Interlocked.Decrement &subscriptionCount |> ignore
                        lock subscriptions (fun () -> subscriptions.Remove obs |> ignore)
                }

        override x.Mark() =
            if subscriptionCount > 0 then
                let v = x.GetValue null
                let all = 
                    lock subscriptions (fun s -> 
                        let arr = subscriptions.ToArray()
                        subscriptions.Clear()
                        arr
                    )

                for s in all do 
                    s.OnNext v
                    s.OnCompleted()

            false

        override x.Compute() =
            input.GetValue x

    type private ObserverMod<'a>(value : 'a, obs : IObservable<'a>) as this =
        inherit Mod.AbstractMod<'a>()

        let mutable value = value

        let push v =
            value <- v
            let outdated = lock this (fun () -> this.OutOfDate)
            if not outdated then transact (fun () -> this.MarkOutdated())

        let subscription = obs.Subscribe push


        member x.Dispose() =
            subscription.Dispose()

        override x.Finalize() =
            try subscription.Dispose()
            with _ -> ()

        override x.Compute() =
            value

        interface IDisposable with
            member x.Dispose() = x.Dispose()

    type private OptionalObserverMod<'a>(obs : IObservable<'a>) as this =
        inherit Mod.AbstractMod<Option<'a>>()

        let mutable value = None

        let push v =
            value <- Some v
            let outdated = lock this (fun () -> this.OutOfDate)
            if not outdated then transact (fun () -> this.MarkOutdated())

        let subscription = obs.Subscribe push


        member x.Dispose() =
            subscription.Dispose()

        override x.Finalize() =
            try subscription.Dispose()
            with _ -> ()

        override x.Compute() =
            value

        interface IDisposable with
            member x.Dispose() = x.Dispose()

    type private ObservableReader<'a>(obs : IObservable<Delta<'a>>, container : ref<Option<ObservableReader<'a>>>) as this =
        inherit ASetReaders.AbstractReader<'a>()

        let store = List<Delta<'a>>()

        let push (d : Delta<'a>) =
            lock store (fun () -> store.Add d)
            let outdated = lock this (fun () -> this.OutOfDate)
            if not outdated then transact (fun () -> this.MarkOutdated())

        let sub = obs.Subscribe(fun d -> push d)

        let consume() =
            lock store (fun () ->
                let l = store |> Seq.toList
                store.Clear()
                l
            )

        override x.ComputeDelta() =
            consume()

        override x.Release() =
            container := None
            store.Clear()
            sub.Dispose()

    type private EmptyImpl<'a>() =
        static let instance = Observable.Empty<'a>()
        static member Instance = instance

    // constants
    [<GeneralizableValue>]
    let empty<'a> : IObservable<'a> = EmptyImpl<'a>.Instance
    let ofList (l : list<'a>) = ConstantObservable l :> IObservable<'a>
    let inline ofSeq (s : seq<'a>) = s |> Seq.toList |> ofList
    let inline ofArray (s : 'a[]) = s |> Array.toList |> ofList
    let inline single (v : 'a) = ofList [v] 


    let subject() = new Subject<'a>()


    // from adaptive to observable
    let nextOf (m : IMod<'a>) : IObservable<'a> =
        ObservableNextValue(m) :> _

    let futureOf (m : IMod<'a>) : IObservable<'a> =
        ObservableMod(m, false) :> _

    let valuesOf (m : IMod<'a>) : IObservable<'a> =
        ObservableMod(m, true) :> _

    // from observable to adaptive
    let latest (initial : 'a) (o : IObservable<'a>) : IMod<'a> =
        new ObserverMod<_>(initial, o) :> _

    let optionalLatest (o : IObservable<'a>) : IMod<Option<'a>> =
        new OptionalObserverMod<_>(o) :> _    

    let toASet (deltas : IObservable<Delta<'a>>) : aset<'a> =
        let original = ref Unchecked.defaultof<_>
        original := new ObservableReader<'a>(deltas, original) |> Some

        ASet.AdaptiveSet(fun () -> 
            match !original with
                | Some r -> 
                    r :> IReader<_>
                | None ->
                    let r = new ObservableReader<'a>(deltas, original)
                    original := Some r
                    r :> IReader<_>
        ) :> aset<_>

    let all (values : IObservable<'a>) : aset<'a> =
        values.Select Add |> toASet

    
    

    // higher order combinators
    let map (f : 'a -> 'b) (o : IObservable<'a>) =
        o.Select f

    let collect (f : 'a -> IObservable<'b>) (o : IObservable<'a>) =
        o.SelectMany f

    let choose (f : 'a -> Option<'b>) (o : IObservable<'a>) =
        Observable.choose f o

    let filter (f : 'a -> bool) (o : IObservable<'a>) =
        o.Where f

    let foldMap (project : 's -> 'x) (f : 's -> 'a -> 's) (seed : 's) (o : IObservable<'a>) =
        Observable.Create (fun (obs : IObserver<'x>) ->
            let seed = ref seed
            o.Subscribe {
                new IObserver<'a> with
                    member x.OnNext v = 
                        let s = f !seed v
                        seed := s
                        obs.OnNext (project s)
                    member x.OnCompleted() = obs.OnCompleted()
                    member x.OnError e = obs.OnError e
            }

        )

    let inline fold (f : 's -> 'a -> 's) (seed : 's) (o : IObservable<'a>) =
        foldMap id f seed o

    let inline sum (o : IObservable<'a>) = fold (+) LanguagePrimitives.GenericZero o

    let inline average (o : IObservable<'a>) : IObservable<'a> = 
        foldMap (uncurry LanguagePrimitives.DivideByInt) (fun (l, c) r -> (l + r, c+1)) (LanguagePrimitives.GenericZero, 0) o
        
    let inline sumBy (f : 'a -> 'b) (o : IObservable<'a>) : IObservable<'b> =
        fold (fun l r -> l + f r) LanguagePrimitives.GenericZero o
        
    let inline averageBy (f : 'a -> 'b) (o : IObservable<'a>) : IObservable<'b> =
        foldMap (uncurry LanguagePrimitives.DivideByInt) (fun (l, c) r -> (l + f r, c+1)) (LanguagePrimitives.GenericZero, 0) o
        

    let skip (n : int) (o : IObservable<'a>) : IObservable<'a> = 
        Observable.Create (fun (obs : IObserver<'a>) ->
            let skip = ref n
            o.Subscribe {
                new IObserver<'a> with
                    member x.OnNext(v) =
                        let c = Interlocked.Decrement &skip.contents
                        if c < 0 then obs.OnNext v
                    member x.OnError e =
                        obs.OnError e
                    member x.OnCompleted() =
                        obs.OnCompleted()
            }

        )
        
    let take (n : int) (o : IObservable<'a>) : IObservable<'a> =
        Observable.Create (fun (obs : IObserver<'a>) ->
            let take = ref n
            let sub = ref noDisposable
            sub := 
                o.Subscribe {
                    new IObserver<'a> with
                        member x.OnNext(v) =
                            let t = Interlocked.Decrement &take.contents
                            if t > 0 then 
                                obs.OnNext v
                            else 
                                obs.OnCompleted()
                                sub.Value.Dispose()
                                sub := noDisposable
                        member x.OnError e =
                            obs.OnError e
                        member x.OnCompleted() =
                            obs.OnCompleted()
                }

            { new IDisposable with member x.Dispose() = sub.Value.Dispose() }

        )


    let takeWhile (m : IMod<bool>) (o : unit -> IObservable<'a>) =
        Observable.Create (fun (obs : IObserver<'a>) ->

            let m = valuesOf m

            let active = ref false
            let modSub = ref noDisposable
            let innerSub = ref noDisposable


            let stop(completed : bool) =
                innerSub.Value.Dispose()
                modSub.Value.Dispose()
                innerSub := noDisposable
                modSub := noDisposable
                if completed then obs.OnCompleted()

            let start() =
                let inner = o()
                innerSub :=
                    inner.Subscribe { 
                        new IObserver<'a> with
                            member x.OnNext(v) = obs.OnNext(v)
                            member x.OnCompleted() = stop true
                            member x.OnError(e) = 
                                obs.OnError(e)
                                stop false
                    }


            modSub :=
                m.Subscribe (fun v ->
                    if not !active && v then
                        start()
                    elif !active && not v then
                        stop true
                        

                    active := v
                )

            { new IDisposable with member x.Dispose() = modSub.Value.Dispose(); innerSub.Value.Dispose() }
        )

    let bind (f : 'a -> IObservable<'b>) (m : IMod<'a>) =
        let m = valuesOf m
        Observable.Create(fun (obs : IObserver<'b>) ->
            let latest : ref<Option<IDisposable>> = ref None
            let outer = 
                m.Subscribe (fun v -> 
                    match !latest with
                        | Some l -> l.Dispose()
                        | None -> ()

                    let inner = f v
                    latest := Some (inner.Subscribe obs)
                )

            { new IDisposable with
                member x.Dispose() = outer.Dispose()
            }
        )


    // ground combinators
    let append (l : IObservable<'a>) (r : IObservable<'a>) =
        l.Concat(r)

    let concat (obs : seq<IObservable<'a>>) =
        Observable.Concat(obs)

    let merge (obs : seq<IObservable<'a>>) =
        Observable.Merge(obs)

    let intersperse (l : IObservable<'a>) (r : IObservable<'b>) : IObservable<Either<'a, 'b>> =
        Observable.Create(fun (obs : IObserver<Either<'a, 'b>>) ->
            let pending = ref 2
            let comlete() =
                let p = Interlocked.Decrement &pending.contents
                if p = 0 then obs.OnCompleted()


            let left = 
                l.Subscribe {
                    new IObserver<'a> with
                        member x.OnNext v = obs.OnNext (Left v)
                        member x.OnError e = obs.OnError e
                        member x.OnCompleted() = comlete()
                }

            let right = 
                r.Subscribe {
                    new IObserver<'b> with
                        member x.OnNext v = obs.OnNext (Right v)
                        member x.OnError e = obs.OnError e
                        member x.OnCompleted() = comlete()
                }

            { new IDisposable with member x.Dispose() = left.Dispose(); right.Dispose() }
        )

    let intersperse3 (o0 : IObservable<'a>) (o1 : IObservable<'b>) (o2 : IObservable<'c>) : IObservable<Choice<'a, 'b, 'c>> =
        Observable.Create(fun (obs : IObserver<Choice<'a, 'b, 'c>>) ->
            let pending = ref 3
            let comlete() =
                let p = Interlocked.Decrement &pending.contents
                if p = 0 then obs.OnCompleted()


            let s0 = 
                o0.Subscribe {
                    new IObserver<'a> with
                        member x.OnNext v = obs.OnNext (Choice1Of3 v)
                        member x.OnError e = obs.OnError e
                        member x.OnCompleted() = comlete()
                }

            let s1 = 
                o1.Subscribe {
                    new IObserver<'b> with
                        member x.OnNext v = obs.OnNext (Choice2Of3 v)
                        member x.OnError e = obs.OnError e
                        member x.OnCompleted() = comlete()
                }

            let s2 = 
                o2.Subscribe {
                    new IObserver<'c> with
                        member x.OnNext v = obs.OnNext (Choice3Of3 v)
                        member x.OnError e = obs.OnError e
                        member x.OnCompleted() = comlete()
                }

            { new IDisposable with member x.Dispose() = s0.Dispose(); s1.Dispose(); s2.Dispose() }
        )

[<AutoOpen>]
module ``Observable builder`` =
    
    type ObservableBuilder() =
        member x.For(m : IObservable<'a>, f : 'a -> IObservable<'b>) =
            m |> Obs.collect f

        member x.For(m : IMod<'a>, f : 'a -> IObservable<'b>) =
            m |> Obs.valuesOf |> Obs.collect f


        member x.Bind(m : IMod<'a>, f : 'a -> IObservable<'b>) =
            m |> Obs.bind f

        member x.While(guard : unit -> bool, body : unit -> IObservable<'a>) =
            if guard() then
                Obs.append (body()) (Observable.Defer (fun () -> x.While(guard, body)))
            else
                Obs.empty

        member x.While(guard : unit -> #IMod<bool>, body : unit -> IObservable<'a>) =
            Obs.takeWhile (guard() :> IMod<_>) body

        member x.Yield(v : 'a) =
            Obs.single v

        member x.YieldFrom (l : seq<'a>) =
            Obs.ofSeq l

        member x.YieldFrom(o : IObservable<'a>) = o

        member x.Delay(f : unit -> IObservable<'a>) = f

        member x.Combine(l : IObservable<'a>, r : unit -> IObservable<'a>) =
            Obs.append l (Observable.Defer r)

        member x.Run(f : unit -> IObservable<'a>) = f()
       
        member x.Zero() = Obs.empty

    let obs = ObservableBuilder()

