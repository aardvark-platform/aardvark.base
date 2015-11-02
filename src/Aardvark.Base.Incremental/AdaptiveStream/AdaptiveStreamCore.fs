namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open System.Collections.Concurrent
open Aardvark.Base

type EventHistory<'a> =
    | Cancel
    | Faulted of Exception
    | History of list<DateTime * 'a>

module EventHistory =

    type private EmptyImpl<'a>() =
        static let instance : EventHistory<'a> = History []
        static member Instance = instance
 
    let rec private concatTimeLists (l : list<DateTime * 'a>) (r : list<DateTime * 'a>) =
        match l, r with
            | [], r -> r
            | l, [] -> l
            | (lt,lv)::lr, (rt,rv)::rr ->
                if lt < rt then (lt,lv)::(concatTimeLists lr r)
                else (rt,rv)::(concatTimeLists l rr)




    let empty<'a> : EventHistory<'a> = EmptyImpl<'a>.Instance

    let isEmpty (h : EventHistory<'a>) =
        match h with
            | Cancel -> true
            | Faulted _ -> true
            | History l -> List.isEmpty l

    let map (f : 'a -> 'b) (h : EventHistory<'a>) =
        match h with
            | Cancel -> Cancel
            | Faulted e -> Faulted e
            | History a -> a |> List.map (fun (t,v) -> (t,f v)) |> History

    let collectSeq (f : 'a -> EventHistory<'b>) (h : seq<'a>) =
        h |> Seq.fold (fun r hi ->
            match r with
                | History current ->
                    match f hi with
                        | Cancel -> History current
                        | Faulted e -> Faulted e
                        | History vi -> History (current @ vi)
                | _ -> r
        ) (History [])
  

    let collect (f : 'a -> EventHistory<'b>) (h : EventHistory<'a>) =
        match h with
            | Cancel -> Cancel
            | Faulted e -> Faulted e
            | History a ->
                a |> List.fold (fun r (_,hi) ->
                    match r with
                        | History current ->
                            match f hi with
                                | Cancel -> History current
                                | Faulted e -> Faulted e
                                | History vi -> History (current @ vi)
                        | _ -> r
                ) (History [])
  
    let choose (f : 'a -> Option<'b>) (h : EventHistory<'a>) =
        match h with
            | Cancel -> Cancel
            | Faulted e -> Faulted e
            | History h ->
                h |> List.choose (fun (t,v) ->
                    match f v with
                        | Some r -> Some(t,r)
                        | None -> None
                ) |> History

    let filter (f : 'a -> bool) (h : EventHistory<'a>) =
        match h with
            | Cancel -> Cancel
            | Faulted e -> Faulted e
            | History h ->
                h |> List.filter (fun (t,v) -> f v)
                  |> History

    
    let union (l : EventHistory<'a>) (r : EventHistory<'a>) =
        match l,r with
            | Cancel, r -> r
            | l, Cancel -> l

            | Faulted l, Faulted r -> Faulted (AggregateException(l,r))
            | Faulted e, _ -> Faulted e
            | _, Faulted e -> Faulted e


            | History l, History r ->
                History (concatTimeLists l r)
            
    let rec concat (histories : list<EventHistory<'a>>) =
        match histories with
            | [] -> empty
            | [h] -> h
            | f::rest ->
                union f (concat rest)

type IStreamReader<'a> =
    inherit IDisposable
    inherit IAdaptiveObject
    abstract member GetHistory : IAdaptiveObject -> EventHistory<'a>
    abstract member SubscribeOnEvaluate : (EventHistory<'a> -> unit) -> IDisposable

[<CompiledName("IAdaptiveStream")>]
type astream<'a> =
    abstract member GetReader : unit -> IStreamReader<'a>

module AStreamReaders =
    
    [<AbstractClass>]
    type AbstractReader<'a>() =
        inherit AdaptiveObject()

        let callbacks = HashSet<EventHistory<'a> -> unit>()

        member x.Callbacks = callbacks

        abstract member Release : unit -> unit
        abstract member ComputeHistory : unit -> EventHistory<'a>
        abstract member GetHistory : IAdaptiveObject -> EventHistory<'a>
 
        default x.GetHistory(caller) =
            x.EvaluateIfNeeded caller EventHistory.empty (fun () ->
                let deltas = x.ComputeHistory()

                if not (EventHistory.isEmpty deltas) then
                    for cb in callbacks do cb deltas

                deltas
            )

        override x.Finalize() =
            try x.Dispose()
            with _ -> ()

        member x.Dispose() =
            x.Release()
            callbacks.Clear()

        member x.SubscribeOnEvaluate (cb : EventHistory<'a> -> unit) =
            lock x (fun () ->
                if callbacks.Add cb then
                    { new IDisposable with 
                        member __.Dispose() = 
                            lock x (fun () ->
                                callbacks.Remove cb |> ignore 
                            )
                    }
                else
                    { new IDisposable with member __.Dispose() = () }
            )

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IStreamReader<'a> with
            member x.GetHistory(caller) = x.GetHistory(caller)
            member x.SubscribeOnEvaluate cb = x.SubscribeOnEvaluate cb


    type FixedSizeCache<'a, 'b>(scope : Ag.Scope, size : int, f : 'a -> 'b) =
        let history = Queue<'a>()
        let cache = Dictionary<obj, 'b>()

        let invoke (v : 'a) =
            match cache.TryGetValue (v :> obj) with
                | (true, r) -> r
                | _ -> 
                    let r = Ag.useScope scope (fun () -> f v)
                    history.Enqueue v
                    cache.[v] <- r
                    if history.Count > size then
                        let fst = history.Dequeue()
                        cache.Remove fst |> ignore
                    r

        member x.Invoke(v) = invoke v

        member x.Clear(f : 'b -> unit) =
            for e in cache.Values do
                f e
            cache.Clear()
            history.Clear()


    type MapReader<'a, 'b>(scope : Ag.Scope, source : IStreamReader<'a>, f : 'a -> 'b) as this =
        inherit AbstractReader<'b>()
        do source.AddOutput this  

        let cache = FixedSizeCache(scope, 32, f)

        override x.Release() =
            source.RemoveOutput this
            source.Dispose()
            cache.Clear(ignore)

        override x.ComputeHistory() =
            source.GetHistory(x) |> EventHistory.map (fun a ->
                cache.Invoke a
            )

    type CollectSetReader<'a, 'b>(scope : Ag.Scope, source : IReader<'a>, f : 'a -> IStreamReader<'b>) as this =
        inherit AbstractReader<'b>()
        do source.AddOutput this

        let dirtyInner = VolatileDirtySet(fun (r : IStreamReader<'b>) -> r.GetHistory(this))
        let f = Cache(scope, f)

        override x.Release() =
            source.RemoveOutput this
            source.Dispose()
            f.Clear(fun r -> r.RemoveOutput this; r.Dispose())
            dirtyInner.Clear()

        override x.InputChanged(o : IAdaptiveObject) =
            match o with
                | :? IStreamReader<'b> as o -> dirtyInner.Push o
                | _ -> ()

        override x.ComputeHistory() =
            for d in source.GetDelta(x) do
                match d with
                    | Add v ->
                        let r = f.Invoke v

                        // we're an output of the new reader
                        r.AddOutput this

                        // listen to marking of r (reader cannot be OutOfDate due to GetDelta above)
                        dirtyInner.Add r


                    | Rem v ->
                        let (last,r) = f.RevokeAndGetDeleted v

                        // remove the reader from the listen-set
                        dirtyInner.Remove r

                        // since the reader is no longer contained we don't want
                        // to be notified anymore
                        r.RemoveOutput this

                        // if the reader's reference count got 0 we dispose it 
                        // since no one can ever reference it again
                        if last then r.Dispose()

            dirtyInner.Evaluate() |> EventHistory.concat

    type ChooseReader<'a, 'b>(scope : Ag.Scope, source : IStreamReader<'a>, f : 'a -> Option<'b>) as this =
        inherit AbstractReader<'b>()
        do source.AddOutput this  

        let cache = FixedSizeCache(scope, 32, f)

        override x.Release() =
            source.RemoveOutput this
            source.Dispose()
            cache.Clear(ignore)

        override x.ComputeHistory() =
            source.GetHistory(x) |> EventHistory.choose (fun a ->
                cache.Invoke a
            )

    type ModReader<'a>(source : IMod<'a>) as this =
        inherit AbstractReader<'a>()
        do source.AddOutput this

        override x.Release() = ()

        override x.ComputeHistory() =
            History [(DateTime.Now, source.GetValue(this))]

    type OneShotReader<'a>(deltas : EventHistory<'a>) =  
        inherit AbstractReader<'a>()
        
        let mutable deltas = deltas

        override x.ComputeHistory() =
            let res = deltas
            deltas <- EventHistory.empty
            res

        override x.Release() =
            deltas <- EventHistory.empty


    type CopyReader<'a>(inputReader : IStreamReader<'a>, dispose : CopyReader<'a> -> unit) as this =
        inherit AbstractReader<'a>()
            
        let mutable deltas = EventHistory.empty

        let emit (d : EventHistory<'a>) =
            lock this (fun () ->
//                if reset.IsNone then
//                    let N = inputReader.Content.Count
//                    let M = this.Content.Count
//                    let D = deltas.Count + (List.length d)
//                    if D > N + 2 * M then
//                        reset <- Some (inputReader.Content :> _)
//                        deltas.Clear()
//                    else
//                        deltas.AddRange d

                deltas <- EventHistory.union deltas d
            
                if not this.OutOfDate then 
                    // TODO: why is that happening sometimes?

                    // A good case: 
                    //     Suppose the inputReader and this one have been marked and
                    //     a "neighbour" reader (of this one) has not yet been marked.
                    //     In that case the neighbouring reader will not be OutOfDate when
                    //     its emit function is called.
                    //     However it should be on the marking-queue since the input was
                    //     marked and therefore the reader should get "eventually consistent"
                    // Thoughts:
                    //     Since the CopyReader's GetDelta starts with acquiring the lock
                    //     to the inputReader only one CopyReader will be able to call its ComputeDelta
                    //     at a time. Therefore only one can call inputReader.Update() at a time and 
                    //     again therefore only one thread may call emit here.
                    // To conclude I could find one good case and couldn't come up with
                    // a bad one. Nevertheless this is not really a proof of its inexistence (hence the print)
                    Aardvark.Base.Log.warn "[AStreamReaders.CopyReader] potentially bad emit with: %A" d
            )

        do inputReader.AddOutput this
        let subscription = inputReader.SubscribeOnEvaluate emit

        override x.GetHistory(caller) =
            lock inputReader (fun () ->
                x.EvaluateIfNeeded caller EventHistory.empty (fun () ->
                    let deltas = x.ComputeHistory()

                    if not (EventHistory.isEmpty deltas) then
                        for cb in x.Callbacks do cb deltas

                    deltas
                )
            )

        override x.ComputeHistory() =
            inputReader.GetHistory(x) |> ignore

            let res = deltas
            deltas <- EventHistory.empty
            res

        override x.Release() =
            inputReader.RemoveOutput x
            subscription.Dispose()
            dispose(x)


    // finally some utility functions reducing "noise" in the code using readers
    let map scope (f : 'a -> 'b) (input : IStreamReader<'a>) =
        new MapReader<_, _>(scope, input, f) :> IStreamReader<_>

    let collect scope (f : 'a -> IStreamReader<'b>) (input : IReader<'a>) =
        new CollectSetReader<_, _>(scope, input, f) :> IStreamReader<_>

    let bind scope (f : 'a -> IStreamReader<'b>) (input : IMod<'a>) =
        new CollectSetReader<_,_>(scope, new ASetReaders.ModReader<_>(input), f) :> IStreamReader<_>

    let bind2 scope (f : 'a -> 'b -> IStreamReader<'c>) (ma : IMod<'a>)  (mb : IMod<'b>)=
        let tup = Mod.map2 (fun a b -> (a,b)) ma mb
        new CollectSetReader<_,_>(scope, new ASetReaders.ModReader<_>(tup),  fun (a,b) -> f a b) :> IStreamReader<_>

    let choose scope (f : 'a -> Option<'b>) (input : IStreamReader<'a>) =
        new ChooseReader<_, _>(scope, input, f) :> IStreamReader<_>

    let ofMod (m : IMod<'a>) =
        new ModReader<_>(m) :> IStreamReader<_>
