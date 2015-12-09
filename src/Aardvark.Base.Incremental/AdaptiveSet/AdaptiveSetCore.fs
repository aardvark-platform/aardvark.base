namespace Aardvark.Base.Incremental


open System
open System.Threading
open System.Collections
open System.Collections.Generic
open System.Collections.Concurrent
open System.Runtime.CompilerServices
open Aardvark.Base

type Change<'a> = list<Delta<'a>>



/// <summary>
/// IReader is the base interface for all adaptive set-readers.
/// Readers are stateful and may not be used by multiple callers
/// since pulling changes mutates the reader internally.
/// </summary>
type IReader<'a> =
    inherit IDisposable
    inherit IAdaptiveObject

    /// <summary>
    /// The reader's current content. 
    /// All changes returned by GetDelta are "relative" to that state.
    /// NOTE that calling GetDelta modifies the reader's content
    /// </summary>
    abstract member Content : ReferenceCountingSet<'a>

    /// <summary>
    /// Update brings the reader's content up-to date without
    /// calculating deltas. 
    /// All other effects are equal to those caused by calling GetDelta
    /// NOTE that Update will cause subsequent calls of GetDelta
    ///      to return empty deltas (as they are always relative to the current content)
    /// For some readers Update can be implemented more efficiently than GetDelta
    /// and in some scenarios the real deltas are not needed.
    /// </summary>
    abstract member Update : IAdaptiveObject -> unit

    /// <summary>
    /// Pulls the reader's deltas relative to its current content.
    /// NOTE that GetDelta also "applies" the deltas to the reader's state
    /// </summary>
    abstract member GetDelta : IAdaptiveObject -> Change<'a>

    abstract member SubscribeOnEvaluate : (Change<'a> -> unit) -> IDisposable




/// <summary>
/// aset serves as the base interface for all adaptive sets.
/// </summary>
[<CompiledName("IAdaptiveSet")>]
type aset<'a> =
    /// <summary>
    /// Returns a NEW reader for the set which will initially return
    /// the entire set-content as deltas.
    /// </summary>
    abstract member Copy : aset<'a>

    abstract member GetReader : unit -> IReader<'a>

    abstract member IsConstant : bool

    abstract member ReaderCount : int

/// <summary>
/// ASetReaders contains implementations of IReader&lt;a&gt; representing
/// the available combinators and is used by the aset-system internally (hence private)
/// </summary>
module ASetReaders =
    let private OneShotReaderEvaluateProbe = Symbol.Create "[ASet] oneshot eval"
    let private ReaderEvaluateProbe = Symbol.Create "[ASet] evaluation"
    let private ReaderComputeProbe = Symbol.Create "[ASet] compute"
    let private ReaderCallbackProbe = Symbol.Create "[ASet] eval callback"
    let private ApplyDeltaProbe = Symbol.Create "[ASet] apply deltas"

    let apply (set : ReferenceCountingSet<'a>) (deltas : list<Delta<'a>>) =
        Telemetry.timed ApplyDeltaProbe (fun () ->
            set.Apply deltas
//            deltas 
//                |> Delta.clean 
//                |> List.filter (fun d ->
//                    match d with
//                        | Add v -> set.Add v
//                        | Rem v -> set.Remove v
//                   )
        )

    [<AbstractClass>]
    type AbstractReader<'a>() =
        inherit AdaptiveObject()



        let mutable isDisposed = 0
        let content = ReferenceCountingSet<'a>()
        let mutable callbacks : HashSet<Change<'a> -> unit> = null


        abstract member Release : unit -> unit
        abstract member ComputeDelta : unit -> Change<'a>
        abstract member Update : IAdaptiveObject -> unit

        member x.Content = content
        member internal x.Callbacks = callbacks

        member x.GetDelta(caller) =
            x.EvaluateIfNeeded caller [] (fun () ->
                Telemetry.timed ReaderEvaluateProbe (fun () ->
                    let deltas = Telemetry.timed ReaderComputeProbe x.ComputeDelta
                    let finalDeltas = Telemetry.timed ApplyDeltaProbe (fun () -> deltas |> apply content)

                    if not (isNull callbacks) then
                        Telemetry.timed ReaderCallbackProbe (fun () ->
                            if not (List.isEmpty finalDeltas) then
                                for cb in callbacks do cb finalDeltas
                        )

                    finalDeltas
                )
            )

        default x.Update(caller) = x.GetDelta(caller) |> ignore

        override x.Finalize() =
            try x.Dispose()
            with _ -> ()

        member x.Dispose() =
            if Interlocked.Exchange(&isDisposed,1) = 0 then
                x.Release()
                content.Clear()

        member x.SubscribeOnEvaluate (cb : Change<'a> -> unit) =
            lock x (fun () ->
                if isNull callbacks then
                    callbacks <- HashSet()

                if callbacks.Add cb then
                    { new IDisposable with 
                        member __.Dispose() = 
                            lock x (fun () ->
                                callbacks.Remove cb |> ignore 
                                if callbacks.Count = 0 then
                                    callbacks <- null
                            )
                    }
                else
                    { new IDisposable with member __.Dispose() = () }
            )

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'a> with
            member x.Content = content
            member x.Update(caller) = x.Update(caller)
            member x.GetDelta(caller) = x.GetDelta(caller)
            member x.SubscribeOnEvaluate cb = x.SubscribeOnEvaluate cb

    type UnionReader<'a>(readers : list<IReader<'a>>) as this =
        inherit AbstractReader<'a>()

        let dirty = MutableVolatileDirtySet<IReader<'a>, list<Delta<'a>>>(fun r -> r.GetDelta(this))
        
        let mutable initial = true

        override x.InputChanged(i : IAdaptiveObject) =
            match i with
                | :? IReader<'a> as r -> dirty.Push r
                | _ -> ()

        override x.Inputs =
            readers |> Seq.cast

        override x.ComputeDelta() =
            if initial then 
                initial <- false
                readers |> List.collect (fun r -> r.GetDelta x)
            else 
                dirty.Evaluate() |> List.concat

        override x.Release() =
            dirty.Clear()

    type BindReader<'a, 'b>(scope : Ag.Scope, m : IMod<'a>, f : 'a -> IReader<'b>) =
        inherit AbstractReader<'b>()

        let f v = Ag.useScope scope (fun () -> f v)
        let hasChanged = ChangeTracker.track<'a>
        let mutable current : Option<IReader<'b>> = None
        let mutable mChanged = true

        override x.InputChanged(o : IAdaptiveObject) =
            mChanged <- Object.ReferenceEquals(o, m)

        override x.Inputs =
            seq {
                yield m :> _
                match current with 
                    | Some o -> yield o :> _ 
                    | _ -> ()
            }

        override x.Release() =
            match current with
                | Some c -> 
                    c.Dispose()
                    current <- None
                | _ -> ()

        override x.ComputeDelta() =
            let moutdated = mChanged
            mChanged <- false
            
            match current with
                | Some old ->
                    if moutdated then
                        let v = m.GetValue x
                        if hasChanged v then
                            let removal = old.Content |> Seq.map Rem |> Seq.toList
                            old.Dispose()
                            let r = f v 
                            current <- Some r
                            removal @ r.GetDelta x
                        else
                            old.GetDelta x
                    else
                        old.GetDelta x
                | None ->
                    let v = m.GetValue x
                    v |> hasChanged |> ignore
                    let r = f v
                    current <- Some r
                    r.GetDelta x
       
    type BindTaskReader<'a, 'b>(scope : Ag.Scope, m : System.Threading.Tasks.Task<'a>, f : 'a -> IReader<'b>) as this =
        inherit AbstractReader<'b>()

        let mutable f = fun v -> Ag.useScope scope (fun () -> f v)
        let mutable current : Option<IReader<'b>> = None
        let mutable completed = false
        let mutable awaiter = m.GetAwaiter()

        do 
            if m.IsCompleted then
                completed <- true
            else
                awaiter.OnCompleted(fun () -> 
                    completed <- true
                    transact (fun () -> this.MarkOutdated())
                )

        override x.Inputs =
            seq {
                match current with 
                    | Some o -> yield o :> _ 
                    | _ -> ()
            }

        override x.Release() =
            
            match current with
                | Some c -> 
                    c.Dispose()
                    current <- None
                | _ -> ()

        override x.ComputeDelta() =
            if completed then
                let r = 
                    match current with
                        | Some c -> c
                        | None ->
                            let v = awaiter.GetResult()
                            let c = f v
                            current <- Some c
                            awaiter <- Unchecked.defaultof<_>
                            f <- Unchecked.defaultof<_>
                            c

                r.GetDelta x
            else
                []
           
                
    type MultiConstantCollectReader<'a, 'b>(scope : Ag.Scope, sources : seq<IReader<'a>>, f : 'a -> seq<'b>) as this =
        inherit AbstractReader<'b>()
        let sources = Seq.toList sources

        let mutable initial = true
        let cache = Cache(scope, f >> Seq.toList)
        let dirty = MutableVolatileDirtySet<IReader<'a>, list<Delta<'a>>>(fun r -> r.GetDelta this)

        override x.Inputs =
            sources |> Seq.cast

        override x.InputChanged(i : IAdaptiveObject) =
            match i with
                | :? IReader<'a> as r -> dirty.Push r
                | _ -> ()

        override x.Release() =
            for s in sources do s.Dispose()
            cache.Clear ignore
            dirty.Clear()

        override x.ComputeDelta() =
            if initial then
                initial <- false
                sources |> List.collect (fun r ->
                    r.GetDelta x 
                        |> List.collect (fun d ->
                            match d with
                                | Add v -> cache.Invoke v |> List.map Add
                                | Rem v -> cache.Revoke v |> List.map Rem
                        )
                )
            else
                dirty.Evaluate() 
                    |> List.concat
                    |> List.collect (fun d ->
                        match d with
                            | Add v -> cache.Invoke v |> List.map Add
                            | Rem v -> cache.Revoke v |> List.map Rem
                    )

    type ConstantCollectReader<'a, 'b>(scope, source : IReader<'a>, f : 'a -> seq<'b>) =
        inherit AbstractReader<'b>()

        let cache = Cache(scope, f >> Seq.toList)

        override x.Inputs =
            Seq.singleton (source :> IAdaptiveObject)

        override x.Release() =
            source.Dispose()
            cache.Clear ignore

        override x.ComputeDelta() =
            source.GetDelta x
                |> List.collect (fun v ->
                    match v with
                        | Add v -> cache.Invoke v |> List.map Add
                        | Rem v -> cache.Revoke v |> List.map Rem
                )


    /// <summary>
    /// A reader representing "map" operations
    /// NOTE that the reader actually takes a function "a -> list&lt;b&gt;" instead of the
    ///      usual "a -> b" since it is convenient for some use-cases and does not make 
    ///      the implementation harder.
    /// </summary>
    type MapReader<'a, 'b>(scope, source : IReader<'a>, f : 'a -> list<'b>) =
        inherit AbstractReader<'b>() 
        
        let f = Cache(scope, f)

        override x.Inputs = Seq.singleton (source :> IAdaptiveObject)

        override x.Release() =
            source.RemoveOutput x
            source.Dispose()
            f.Clear(ignore)

        override x.ComputeDelta() =
            source.GetDelta x 
                |> List.collect (fun d ->
                    match d with
                        | Add v -> f.Invoke v |> List.map Add
                        | Rem v -> f.Revoke v |> List.map Rem
                )

          
    /// <summary>
    /// A reader representing "collect" operations
    /// NOTE that this is THE core implementation of the entire aset-system and every 
    ///      other reader could be simulated using this one.
    /// </summary>   
    type CollectReader<'a, 'b>(scope, source : IReader<'a>, f : 'a -> IReader<'b>) as this =
        inherit AbstractReader<'b>()

        let f = Cache(scope, f)
        let dirtyInner = MutableVolatileDirtySet(fun (r : IReader<'b>) -> r.GetDelta(this))

        override x.Inputs =
            seq {
                yield source :> IAdaptiveObject
                for c in f.Values do yield c :> IAdaptiveObject
            }


        override x.InputChanged (o : IAdaptiveObject) = 
            match o with
                | :? IReader<'b> as o -> dirtyInner.Add o
                | _ -> ()


        override x.Release() =
            source.RemoveOutput x
            source.Dispose()
            f.Clear(fun r -> r.RemoveOutput x; r.Dispose())
            dirtyInner.Clear()

        override x.ComputeDelta() =
            let xs = source.GetDelta x
            let outerDeltas =
                xs |> List.collect (fun d ->
                    match d with
                        | Add v ->
                            let r = f.Invoke v

                            // we're an output of the new reader
                            // bring the reader's content up-to-date by calling GetDelta
                            r.GetDelta x |> ignore

                            // listen to marking of r (reader cannot be OutOfDate due to GetDelta above)
                            dirtyInner.Add r
                                    
                            // since the entire reader is new we add its content
                            // which must be up-to-date here (due to calling GetDelta above)
                            r.Content |> Seq.map Add |> Seq.toList

                        | Rem v ->
                            let (last, r) = f.RevokeAndGetDeleted v

                            // remove the reader from the listen-set
                            dirtyInner.Remove r

                            // since the reader is no longer contained we don't want
                            // to be notified anymore
                            r.RemoveOutput x
   
                            // the entire content of the reader is removed
                            // Note that the content here might be OutOfDate
                            // TODO: think about implications here when we do not "own" the reader
                            //       exclusively
                            let res = r.Content |> Seq.map Rem |> Seq.toList

                            // if the reader's reference count got 0 we dispose it 
                            // since no one can ever reference it again
                            if last then r.Dispose()

                            res
                )

            // all dirty inner readers must be registered 
            // in dirtyInner. Even if the outer set did not change we
            // need to collect those inner deltas.
            let innerDeltas = dirtyInner.Evaluate() |> List.concat

            // concat inner and outer deltas 
            List.append outerDeltas innerDeltas


    /// <summary>
    /// A reader representing "choose" operations
    /// </summary>   
    type ChooseReader<'a, 'b>(scope, source : IReader<'a>, f : 'a -> Option<'b>) =
        inherit AbstractReader<'b>()

        let f = Cache(scope, f)

        override x.Inputs = Seq.singleton (source :> IAdaptiveObject)

        override x.Release() =
            source.RemoveOutput x
            source.Dispose()
            f.Clear(ignore)

        override x.ComputeDelta() =
            let xs = source.GetDelta x
            xs |> List.choose (fun d ->
                match d with
                    | Add v ->
                        let r = f.Invoke v

                        match r with
                            | Some r -> Some (Add r)
                            | None -> None

                    | Rem v ->
                        let r = f.Revoke v

                        match r with
                            | Some r -> Some (Rem r)
                            | None -> None

            )

    /// <summary>
    /// A reader for using IMod&lt;a&gt; as a single-valued-set
    /// </summary>   
    type ModReader<'a>(source : IMod<'a>) =  
        inherit AbstractReader<'a>()
        let hasChanged = ChangeTracker.track<'a>
        let mutable old = None

        override x.Inputs = Seq.singleton (source :> IAdaptiveObject)

        override x.Release() =
            source.RemoveOutput x
            old <- None

        override x.ComputeDelta() =
            let v = source.GetValue(x)
            if hasChanged v then
                match old with
                    | Some c -> 
                        old <- Some v
                        [Rem c; Add v]
                    | None ->
                        old <- Some v
                        [Add v]
            else
                []

    /// <summary>
    /// A reader which allows changes to be pushed from the outside (e.g. cset)
    /// NOTE that atm. EmitReader may keep very long histories since the code fixing that
    ///      is mostly untested and will be "activated" on demand (if someone needs it)
    /// NOTE that it is safe to call Emit from various threads since it is synchronized internally
    /// </summary>   
    type EmitReader<'a>(lockObj : obj, dispose : EmitReader<'a> -> unit) =
        inherit AbstractReader<'a>()

        let deltas = List<Delta<'a>>()
        let mutable reset : Option<ISet<'a>> = None

        override x.Inputs : seq<IAdaptiveObject> = Seq.empty

        member x.Emit (c : ISet<'a>, d : Option<list<Delta<'a>>>) =
            lock x (fun () ->
                match reset with
                    | Some r ->
                        reset <- Some c
                    | None -> 
                        match d with 
                            | Some d ->
                                deltas.AddRange d
        //                        let N = c.Count
        //                        let M = content.Count
        //                        let D = deltas.Count + (List.length d)
        //                        if D > N + 2 * M then
        //                            reset <- Some c
        //                            deltas.Clear()
        //                        else
        //                            deltas.AddRange d

                            | None ->
                                reset <- Some c
                                deltas.Clear()

                if not x.OutOfDate then
                    match getCurrentTransaction() with
                        | Some t ->
                            t.Enqueue x
                        | _ ->
                            failwith "[EmitReader] cannot emit without transaction"
            )

        override x.Release() =
            dispose x
            deltas.Clear()
            reset <- None

        override x.ComputeDelta() =
            let content = x.Content
            match reset with
                | Some c ->
                    lock lockObj (fun () ->
                        //Interlocked.Increment(&resetCount) |> ignore
                        reset <- None
                        let add = c |> Seq.filter (not << content.Contains) |> Seq.map Add
                        let rem = content |> Seq.filter (not << c.Contains) |> Seq.map Rem

                        Seq.append add rem |> Seq.toList
                    )
                | None ->
                    //Interlocked.Increment(&incrementalCount) |> ignore
                    let res = deltas |> Seq.toList
                    deltas.Clear()
                    res


    type ReferenceCountedReader<'a>(newReader : unit -> IReader<'a>) =
        static let noNewReader : unit -> IReader<'a> = 
            fun () -> failwith "[ASet] implementation claimed that no new readers would be allocated"

        let lockObj = obj()
        let mutable newReader = newReader
        let mutable reader = None
        let mutable refCount = 0
        let containgSetDied = EventSourceSlim ()
        let onlyReader = EventSourceSlim true

        member x.ContainingSetDied() =
            lock lockObj (fun () ->
                containgSetDied.Emit ()
                //newReader <- noNewReader
                
            )

        member x.OnlyReader = onlyReader :> IEvent<bool>

        member x.ReferenceCount = 
            lock lockObj (fun () -> refCount)

        member x.ContainingSetDiedEvent = containgSetDied :> IEvent<_>

        member x.GetReference() =
            lock lockObj (fun () ->
                let reader = 
                    match reader with
                        | None ->
                            let r = newReader()
                            reader <- Some r
                            r
                        | Some r -> r

                refCount <- refCount + 1
                if refCount = 1 then onlyReader.Emit(true)
                else onlyReader.Emit(false)
                reader
            )

        member x.RemoveReference() =
            lock lockObj (fun () ->
                refCount <- refCount - 1

                if refCount = 1 then onlyReader.Emit(true)
                elif refCount = 0 then
                    reader.Value.Dispose()
                    reader <- None
            )



    type CopyReader<'a>(input : ReferenceCountedReader<'a>) as this =
        inherit AdaptiveObject()
          
        let inputReader = input.GetReference()

        let mutable containgSetDead = false
        let mutable initial = true
        let mutable passThru        : bool                          = true
        let mutable deltas          : List<Delta<'a>>               = null
        let mutable reset           : Option<ISet<'a>>              = None 
        let mutable subscription    : IDisposable                   = null
        let mutable content         : ReferenceCountingSet<'a>      = Unchecked.defaultof<_>
        let mutable callbacks       : HashSet<Change<'a> -> unit>   = null

        let emit (d : list<Delta<'a>>) =
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


                if reset.IsNone then
                    deltas.AddRange d
            
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
                    Aardvark.Base.Log.warn "[ASetReaders.CopyReader] potentially bad emit with: %A" d
            )
 
        let mutable isDisposed = 0
        let mutable deadSubscription = input.ContainingSetDiedEvent.Values.Subscribe this.ContainingSetDied
        let mutable onlySubscription = input.OnlyReader.Values.Subscribe(fun pass -> this.SetPassThru(pass, passThru))


        member x.SetPassThru(active : bool, copyContent : bool) =
            lock inputReader (fun () ->
                lock this (fun () ->
                    if active <> passThru then
                        passThru <- active
                        if passThru then
                            deltas <- null
                            reset <- None
                            subscription.Dispose()
                            subscription <- null
                            content <- Unchecked.defaultof<_>
                        else
                            deltas <- List()
                            content <- ReferenceCountingSet()
                            if copyContent then
                                reset <- None
                                content.SetTo(inputReader.Content)
                            else
                                reset <- Some (inputReader.Content :> ISet<_>)
                            subscription <- inputReader.SubscribeOnEvaluate(emit)
                            ()
                )
            )

        member private x.ContainingSetDied() =
            deadSubscription.Dispose()
            if not input.OnlyReader.Latest then
                deadSubscription <- input.OnlyReader.Values.Subscribe(fun pass -> if pass then x.ContainingSetDied())
            else
                containgSetDead <- true
                x.Optimize()

        member x.PassThru =
            passThru

        member private x.Optimize() =
            () //Log.line "optimize: input: %A -> %A" inputReader.Inputs inputReader

        override x.Inputs = Seq.singleton (inputReader :> IAdaptiveObject)

        member x.Content = 
            lock x (fun () ->
                if passThru then
                    inputReader.Content
                else
                    content
            )

        member x.ComputeDelta() =
            if passThru then
                if initial then
                    inputReader.Update x
                    inputReader.Content |> Seq.map Add |> Seq.toList
                else
                    inputReader.GetDelta x

            else
                inputReader.Update x
                inputReader.Outputs.Add x |> ignore

                match reset with
                    | Some c ->
                        reset <- None
                        deltas.Clear()
                        let add = c |> Seq.filter (not << content.Contains) |> Seq.map Add
                        let rem = content |> Seq.filter (not << c.Contains) |> Seq.map Rem

                        Telemetry.timed ApplyDeltaProbe (fun () -> Seq.append add rem |> Seq.toList |> apply content)
                    | None ->
                        let res = deltas |> Seq.toList
                        deltas.Clear()
                        Telemetry.timed ApplyDeltaProbe (fun () -> res |> apply content)

        member x.GetDelta(caller) =
            lock inputReader (fun () ->
                x.EvaluateIfNeeded caller [] (fun () ->
                    Telemetry.timed ReaderEvaluateProbe (fun () ->
                        let deltas = Telemetry.timed ReaderComputeProbe x.ComputeDelta
          
                        if not (isNull callbacks) then
                            Telemetry.timed ReaderCallbackProbe (fun () ->
                                if not (List.isEmpty deltas) then
                                    for cb in callbacks do cb deltas
                            )

                        initial <- false
                        deltas
                    )
                )
            )

        member x.Update(caller) = x.GetDelta(caller) |> ignore

        override x.Finalize() =
            try x.Dispose()
            with e -> Report.Warn("finalizer faulted: {0}", e.Message)

        member x.Dispose() =
            if Interlocked.Exchange(&isDisposed, 1) = 0 then
                onlySubscription.Dispose()
                deadSubscription.Dispose()
                inputReader.RemoveOutput x
                if not passThru then
                    subscription.Dispose()
                    content.Clear()


                input.RemoveReference()
                //dispose(x)

        member x.SubscribeOnEvaluate (cb : Change<'a> -> unit) =
            lock x (fun () ->
                if isNull callbacks then
                    callbacks <- HashSet()

                if callbacks.Add cb then
                    { new IDisposable with 
                        member __.Dispose() = 
                            lock x (fun () ->
                                callbacks.Remove cb |> ignore 
                                if callbacks.Count = 0 then
                                    callbacks <- null
                            )
                    }
                else
                    { new IDisposable with member __.Dispose() = () }
            )

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'a> with
            member x.Content = x.Content
            member x.Update(caller) = x.Update(caller)
            member x.GetDelta(caller) = x.GetDelta(caller)
            member x.SubscribeOnEvaluate cb = x.SubscribeOnEvaluate cb


    type OneShotReader<'a>(content : ReferenceCountingSet<'a>) =  
        inherit ConstantObject()
        let mutable callbacks : HashSet<list<Delta<'a>> -> unit> = null
        static let empty = ReferenceCountingSet<'a>()

        let toDeltaList (set : ReferenceCountingSet<'a>) =
            match set.Count with
                | 0 -> []
                | 1 -> [Add (set.FirstOrDefault(Unchecked.defaultof<_>))]
                | _ -> 
                    set.ToArray(set.Count) |> Array.toList |> List.map Add

        let mutable initial = true

        member x.SubscribeOnEvaluate(cb : list<Delta<'a>> -> unit) =
            lock x (fun () ->
                if isNull callbacks then
                    callbacks <- HashSet()

                if callbacks.Add cb then
                    { new IDisposable with 
                        member __.Dispose() = 
                            lock x (fun () ->
                                callbacks.Remove cb |> ignore 
                                if callbacks.Count = 0 then
                                    callbacks <- null
                            )
                    }
                else
                    { new IDisposable with member __.Dispose() = () }
            )
        member x.GetDelta(caller : IAdaptiveObject) =
            Telemetry.timed OneShotReaderEvaluateProbe (fun () ->
                lock x (fun () ->
                    if initial then
                        initial <- false
                        let deltas = toDeltaList content

                        if not (isNull callbacks) then
                            for cb in callbacks do
                                cb deltas
                            callbacks <- null

                        deltas
                    else
                        []
                )
            )

        interface IReader<'a> with
            member x.GetDelta(caller) = x.GetDelta(caller)
            member x.SubscribeOnEvaluate(cb) = x.SubscribeOnEvaluate cb
            member x.Update(caller) = 
                x.GetDelta(caller) |> ignore
            member x.Dispose() = ()
            member x.Content = 
                if initial then empty
                else content

    type UseReader<'a when 'a :> IDisposable>(inputReader : IReader<'a>) =
        inherit AbstractReader<'a>()

        override x.Inputs = Seq.singleton (inputReader :> IAdaptiveObject)

        override x.Release() =
            for c in x.Content do
                try c.Dispose() with _ -> ()
            inputReader.RemoveOutput x
            inputReader.Dispose()

        override x.ComputeDelta() =
            let deltas = inputReader.GetDelta(x)

            for d in deltas do
                match d with
                    | Rem v -> v.Dispose()
                    | _ -> ()

            deltas

    type MapUseReader<'a, 'b when 'b :> IDisposable>(scope, source : IReader<'a>, f : 'a -> list<'b>) =
        inherit AbstractReader<'b>() 
        
        let f = Cache(scope, f)

        override x.Inputs = Seq.singleton (source :> IAdaptiveObject)

        override x.Release() =
            source.RemoveOutput x
            source.Dispose()
            f.Clear(fun (b : list<'b>) -> b |> List.iter (fun b -> b.Dispose()))

        override x.ComputeDelta() =
            source.GetDelta x 
                |> List.collect (fun d ->
                    match d with
                        | Add v -> 
                            f.Invoke v |> List.map Add
                        | Rem v -> 
                            let last, rem = f.RevokeAndGetDeleted v 
                            
                            if last then
                                rem |> List.iter (fun d -> d.Dispose())
                            
                            rem |> List.map Rem
                )



    // finally some utility functions reducing "noise" in the code using readers
    let map scope (f : 'a -> 'b) (input : IReader<'a>) =
        new MapReader<_, _>(scope, input, fun c -> [f c]) :> IReader<_>

    let collect scope (f : 'a -> IReader<'b>) (input : IReader<'a>) =
        new CollectReader<_, _>(scope, input, f) :> IReader<_>

    let collect' scope (f : 'a -> seq<'b>) (input : IReader<'a>) =
        new ConstantCollectReader<_, _>(scope, input, f) :> IReader<_>



    let union (input : list<IReader<'a>>) =
        new UnionReader<_>(input) :> IReader<_>

    let bind scope (f : 'a -> IReader<'b>) (input : IMod<'a>) =
        new CollectReader<_,_>(scope, new ModReader<_>(input), f) :> IReader<_>

    let bind2 scope (f : 'a -> 'b -> IReader<'c>) (ma : IMod<'a>)  (mb : IMod<'b>)=
        let tup = Mod.map2 (fun a b -> (a,b)) ma mb
        new BindReader<_,_>(scope, tup,  fun (a,b) -> f a b) :> IReader<_>

    let bindTask scope (f : 'a -> IReader<'b>) (input : System.Threading.Tasks.Task<'a>) =
        new BindTaskReader<_,_>(scope, input, f) :> IReader<_>


    let choose scope (f : 'a -> Option<'b>) (input : IReader<'a>) =
        new ChooseReader<_, _>(scope, input, f) :> IReader<_>

    let ofMod (m : IMod<'a>) =
        new ModReader<_>(m) :> IReader<_>

    let using (r : IReader<'a>) =
        new UseReader<'a>(r) :> IReader<_>

    let mapUsing scope (f : 'a -> 'b) (r : IReader<'a>) =
        new MapUseReader<'a, 'b>(scope, r, fun v -> [f v]) :> IReader<_>