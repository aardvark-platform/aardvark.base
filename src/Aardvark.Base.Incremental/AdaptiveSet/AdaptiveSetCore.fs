namespace Aardvark.Base.Incremental


open System
open System.Threading
open System.Collections
open System.Collections.Generic
open System.Collections.Concurrent
open System.Runtime.CompilerServices


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
    abstract member Update : unit -> unit

    /// <summary>
    /// Pulls the reader's deltas relative to its current content.
    /// NOTE that GetDelta also "applies" the deltas to the reader's state
    /// </summary>
    abstract member GetDelta : unit -> Change<'a>

/// <summary>
/// aset serves as the base interface for all adaptive sets.
/// </summary>
[<CompiledName("IAdaptiveSet")>]
type aset<'a> =
    /// <summary>
    /// Returns a NEW reader for the set which will initially return
    /// the entire set-content as deltas.
    /// </summary>
    abstract member GetReader : unit -> IReader<'a>


/// <summary>
/// ASetReaders contains implementations of IReader&lt;a&gt; representing
/// the available combinators and is used by the aset-system internally (hence private)
/// </summary>
module private ASetReaders =

    let apply (set : ReferenceCountingSet<'a>) (deltas : list<Delta<'a>>) =
        deltas 
            |> Delta.clean 
            |> List.filter (fun d ->
                match d with
                    | Add v -> set.Add v
                    | Rem v -> set.Remove v
               )

    /// <summary>
    /// A simple datastructure for efficiently pulling changes from
    /// a set of readers.
    /// </summary>
    type DirtyReaderSet<'a>() =
        let subscriptions = ConcurrentDictionary<IReader<'a>, unit -> unit>()
        let dirty = HashSet<IReader<'a>>()

        /// <summary>
        /// Starts "listening" to changes of a certain reader
        /// </summary>
        member x.Listen(r : IReader<'a>) =
            // create and register a callback function
            let onMarking () = 
                lock dirty (fun () -> dirty.Add r |> ignore )
                subscriptions.TryRemove r |> ignore


            r.MarkingCallbacks.Add onMarking |> ignore

            // store the "subscription" for each reader in order
            // to allow removals of readers
            subscriptions.[r] <- fun () -> r.MarkingCallbacks.Remove onMarking |> ignore

            // if the reader is currently outdated add it to the
            // dirty set immediately
            lock r (fun () -> 
                if r.OutOfDate then lock dirty (fun () -> dirty.Add r |> ignore)
            )
            
        /// <summary>
        /// Stops "listening" to changes of a certain reader
        /// </summary>
        member x.Unlisten(r : IReader<'a>) =
            // if there exists a subscription we remove and dispose it
            match subscriptions.TryRemove r with
                | (true, d) -> d()
                | _ -> ()

            // if the reader is already in the dirty-set remove it from there too
            lock dirty (fun () -> dirty.Remove r |> ignore)

        /// <summary>
        /// Gets the (concatenated) deltas from all "dirty" readers
        /// </summary>
        member x.GetDeltas() =
            let mine = 
                lock dirty (fun () -> 
                    let arr = dirty |> Seq.toList
                    dirty.Clear()
                    arr
                )

            mine |> List.collect (fun d ->
                // get deltas for all dirty readers and re-register
                // marking callbacks
                lock d (fun () ->
                    let c = d.GetDelta()
                    x.Listen d
                    c
                )
            )

        /// <summary>
        /// Releases the entire structure
        /// </summary>
        member x.Dispose() =
            for (KeyValue(_, d)) in subscriptions do d()
            lock dirty (fun () -> dirty.Clear())
            subscriptions.Clear()

        interface IDisposable with
            member x.Dispose() = x.Dispose()



    /// <summary>
    /// A reader representing "map" operations
    /// NOTE that the reader actually takes a function "a -> list&lt;b&gt;" instead of the
    ///      usual "a -> b" since it is convenient for some use-cases and does not make 
    ///      the implementation harder.
    /// </summary>
    type MapReader<'a, 'b>(scope, source : IReader<'a>, f : 'a -> list<'b>) as this =
        inherit AdaptiveObject()
        do source.AddOutput this  
        
        let f = Cache(scope, f)
        let content = ReferenceCountingSet<'b>()

        member x.Dispose() =
            source.RemoveOutput this
            source.Dispose()
            content.Clear()
            f.Clear(ignore)

        override x.Finalize() =
            try x.Dispose()
            with _ -> ()

        member x.GetDelta() =
            x.EvaluateIfNeeded [] (fun () ->
                let input = source.GetDelta()
                    
                let result = 
                    input |> List.collect (fun d ->
                        match d with
                            | Add v -> f.Invoke v |> List.map Add
                            | Rem v -> f.Revoke v |> List.map Rem
                    ) |> apply content

                result
            )     
          

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'b> with
            member x.Content = content
            member x.Update() = x.GetDelta() |> ignore 
            member x.GetDelta() = x.GetDelta()
       
    /// <summary>
    /// A reader representing "collect" operations
    /// NOTE that this is THE core implementation of the entire aset-system and every 
    ///      other reader could be simulated using this one.
    /// </summary>   
    type CollectReader<'a, 'b>(scope, source : IReader<'a>, f : 'a -> IReader<'b>) as this =
        inherit AdaptiveObject()
        do source.AddOutput this

        let f = Cache(scope, f)
        let content = ReferenceCountingSet<'b>()
        let dirtyInner = new DirtyReaderSet<'b>()

        member x.Dispose() =
            source.RemoveOutput this
            source.Dispose()
            f.Clear(fun r -> r.RemoveOutput this; r.Dispose())
            content.Clear()
            dirtyInner.Dispose()

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        member x.GetDelta() =
            x.EvaluateIfNeeded [] (fun () ->
                let xs = source.GetDelta()
                let outerDeltas =
                    xs |> List.collect (fun d ->
                        match d with
                            | Add v ->
                                let r = f.Invoke v

                                // we're an output of the new reader
                                r.AddOutput this

                                // bring the reader's content up-to-date by calling GetDelta
                                r.GetDelta() |> ignore

                                // listen to marking of r (reader cannot be OutOfDate due to GetDelta above)
                                dirtyInner.Listen r
                                    
                                // since the entire reader is new we add its content
                                // which must be up-to-date here (due to calling GetDelta above)
                                r.Content |> Seq.map Add |> Seq.toList

                            | Rem v ->
                                let r = f.Revoke v

                                // remove the reader from the listen-set
                                dirtyInner.Unlisten r

                                // since the reader is no longer contained we don't want
                                // to be notified anymore
                                r.RemoveOutput this
                                    
                                // the entire content of the reader is removed
                                // Note that the content here might be OutOfDate
                                // TODO: think about implications here when we do not "own" the reader
                                //       exclusively
                                r.Content |> Seq.map Rem |> Seq.toList

                    )

                // all dirty inner readers must be registered 
                // in dirtyInner. Even if the outer set did not change we
                // need to collect those inner deltas.
                let innerDeltas = dirtyInner.GetDeltas()

                // concat inner and outer deltas 
                let deltas = List.append outerDeltas innerDeltas
                deltas |> apply content

            )


        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'b> with
            member x.Content = content
            member x.GetDelta() = x.GetDelta()
            member x.Update() = x.GetDelta() |> ignore

    /// <summary>
    /// A reader representing "choose" operations
    /// </summary>   
    type ChooseReader<'a, 'b>(scope, source : IReader<'a>, f : 'a -> Option<'b>) as this =
        inherit AdaptiveObject()
        do source.AddOutput this

        let f = Cache(scope, f)
        let content = ReferenceCountingSet<'b>()

        member x.Dispose() =
            source.RemoveOutput this
            source.Dispose()
            f.Clear(ignore)
            content.Clear()

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        member x.GetDelta() =
            x.EvaluateIfNeeded [] (fun () ->
                let xs = source.GetDelta()
                let resultDeltas =
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

                resultDeltas |> apply content
            )

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'b> with
            member x.Content = content
            member x.GetDelta() = x.GetDelta()
            member x.Update() = x.GetDelta() |> ignore

    /// <summary>
    /// A reader for using IMod&lt;a&gt; as a single-valued-set
    /// </summary>   
    type ModReader<'a>(source : IMod<'a>) as this =  
        inherit AdaptiveObject()
        do source.AddOutput this
        let content = ReferenceCountingSet()
        let hasChanged = ChangeTracker.track<'a>
        let mutable old = None

        member x.Dispose() =
            source.RemoveOutput this
            old <- None

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        member x.GetDelta() =
            x.EvaluateIfNeeded [] (fun () ->
                let v = source.GetValue()
                let resultDeltas =
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

                resultDeltas |> apply content
            )


        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'a> with
            member x.Content = content
            member x.GetDelta() = x.GetDelta()
            member x.Update() = x.GetDelta() |> ignore

    /// <summary>
    /// A reader which allows changes to be pushed by the supplied update function.
    /// NOTE that BufferedReader shall not be used outside the system unless you understand
    ///      its behaviour very clearly.
    /// NOTE that atm. BufferedReader may keep very long histories since the code fixing that
    ///      is mostly untested and will be "activated" on demand (if someone needs it)
    /// </summary> 
    type BufferedReader<'a>(update : unit -> unit, dispose : BufferedReader<'a> -> unit) =
        inherit AdaptiveObject()
        let deltas = List()
        let mutable reset : Option<ISet<'a>> = None

        static let mutable resetCount = 0
        static let mutable incrementalCount = 0


        let content = ReferenceCountingSet<'a>()

        
        let getDeltas() =
            match reset with
                | Some c ->
                    Interlocked.Increment(&resetCount) |> ignore
                    reset <- None
                    let add = c |> Seq.filter (not << content.Contains) |> Seq.map Add
                    let rem = content |> Seq.filter (not << c.Contains) |> Seq.map Rem

                    Seq.append add rem |> Seq.toList
                | None ->
                    Interlocked.Increment(&incrementalCount) |> ignore
                    let res = deltas |> Seq.toList
                    deltas.Clear()
                    res

        member x.Emit (c : ISet<'a>, d : Option<list<Delta<'a>>>) =
            lock x (fun () ->
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

                x.MarkOutdated()
            )


        new(dispose) = new BufferedReader<'a>(id, dispose)
        new() = new BufferedReader<'a>(id, ignore)

        member x.Dispose() =
            dispose(x)

        override x.Finalize() = 
            try x.Dispose()
            with _ -> ()

        member x.GetDelta() =
            x.EvaluateAlways (fun () ->
                update()
                if x.OutOfDate then
                    let l = getDeltas()
                    l |> apply content
                else
                    []
            
            )

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'a> with
            member x.GetDelta() = x.GetDelta()
            member x.Update() = x.GetDelta() |> ignore

            member x.Content = content


    // finally some utility functions reducing "noise" in the code using readers
    let map scope (f : 'a -> 'b) (input : IReader<'a>) =
        new MapReader<_, _>(scope, input, fun c -> [f c]) :> IReader<_>

    let collect scope (f : 'a -> IReader<'b>) (input : IReader<'a>) =
        new CollectReader<_, _>(scope, input, f) :> IReader<_>

    let bind scope (f : 'a -> IReader<'b>) (input : IMod<'a>) =
        new CollectReader<_,_>(scope, new ModReader<_>(input), f) :> IReader<_>

    let bind2 scope (f : 'a -> 'b -> IReader<'c>) (ma : IMod<'a>)  (mb : IMod<'b>)=
        let tup = Mod.map2 (fun a b -> (a,b)) ma mb
        new CollectReader<_,_>(scope, new ModReader<_>(tup),  fun (a,b) -> f a b) :> IReader<_>

    let choose scope (f : 'a -> Option<'b>) (input : IReader<'a>) =
        new ChooseReader<_, _>(scope, input, f) :> IReader<_>

    let ofMod (m : IMod<'a>) =
        new ModReader<_>(m) :> IReader<_>