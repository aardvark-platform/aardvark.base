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
    abstract member GetReader : unit -> IReader<'a>

    abstract member IsConstant : bool

    abstract member ReaderCount : int

/// <summary>
/// ASetReaders contains implementations of IReader&lt;a&gt; representing
/// the available combinators and is used by the aset-system internally (hence private)
/// </summary>
module ASetReaders =

    let apply (set : ReferenceCountingSet<'a>) (deltas : list<Delta<'a>>) =
        deltas 
            |> Delta.clean 
            |> List.filter (fun d ->
                match d with
                    | Add v -> set.Add v
                    | Rem v -> set.Remove v
               )

    [<AbstractClass>]
    type AbstractReader<'a>() =
        inherit AdaptiveObject()

        let content = ReferenceCountingSet<'a>()
        let callbacks = HashSet<Change<'a> -> unit>()


        abstract member Release : unit -> unit
        abstract member ComputeDelta : unit -> Change<'a>
        abstract member Update : unit -> unit
        abstract member GetDelta : unit -> Change<'a>
 
        member x.Content = content
        member x.Callbacks = callbacks

        default x.GetDelta() =
            x.EvaluateIfNeeded [] (fun () ->
                let deltas = x.ComputeDelta()
                let finalDeltas = deltas |> apply content

                if not (List.isEmpty finalDeltas) then
                    for cb in callbacks do cb finalDeltas

                finalDeltas
            )

        default x.Update() = x.GetDelta() |> ignore

        override x.Finalize() =
            try x.Dispose()
            with _ -> ()

        member x.Dispose() =
            x.Release()
            content.Clear()

        member x.SubscribeOnEvaluate (cb : Change<'a> -> unit) =
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

        interface IReader<'a> with
            member x.Content = content
            member x.Update() = x.Update()
            member x.GetDelta() = x.GetDelta()
            member x.SubscribeOnEvaluate cb = x.SubscribeOnEvaluate cb


    /// <summary>
    /// A reader representing "map" operations
    /// NOTE that the reader actually takes a function "a -> list&lt;b&gt;" instead of the
    ///      usual "a -> b" since it is convenient for some use-cases and does not make 
    ///      the implementation harder.
    /// </summary>
    type MapReader<'a, 'b>(scope, source : IReader<'a>, f : 'a -> list<'b>) as this =
        inherit AbstractReader<'b>()
        do source.AddOutput this  
        
        let f = Cache(scope, f)

        override x.Release() =
            source.RemoveOutput this
            source.Dispose()
            f.Clear(ignore)

        override x.ComputeDelta() =
            source.GetDelta() 
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
        do source.AddOutput this

        let f = Cache(scope, f)
        let dirtyInner = VolatileDirtySet(fun (r : IReader<'b>) -> r.GetDelta())

        override x.InputChanged (o : IAdaptiveObject) = 
            match o with
                | :? IReader<'b> as o -> dirtyInner.Add o
                | _ -> ()


        override x.Release() =
            source.RemoveOutput this
            source.Dispose()
            f.Clear(fun r -> r.RemoveOutput this; r.Dispose())
            dirtyInner.Clear()

        override x.ComputeDelta() =
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
                            r.RemoveOutput this
   
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
    type ChooseReader<'a, 'b>(scope, source : IReader<'a>, f : 'a -> Option<'b>) as this =
        inherit AbstractReader<'b>()
        do source.AddOutput this

        let f = Cache(scope, f)

        override x.Release() =
            source.RemoveOutput this
            source.Dispose()
            f.Clear(ignore)

        override x.ComputeDelta() =
            let xs = source.GetDelta()
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
    type ModReader<'a>(source : IMod<'a>) as this =  
        inherit AbstractReader<'a>()
        do source.AddOutput this
        let hasChanged = ChangeTracker.track<'a>
        let mutable old = None

        override x.Release() =
            source.RemoveOutput this
            old <- None

        override x.ComputeDelta() =
            let v = source.GetValue()
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

    
    type CopyReader<'a>(inputReader : IReader<'a>, dispose : CopyReader<'a> -> unit) as this =
        inherit AbstractReader<'a>()
        
        let deltas = List()
        let mutable reset = Some (inputReader.Content :> ISet<_>)

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



        do inputReader.AddOutput this
        let subscription = inputReader.SubscribeOnEvaluate emit

        override x.GetDelta() =
            lock inputReader (fun () ->
                x.EvaluateIfNeeded [] (fun () ->
                    let deltas = x.ComputeDelta()
                    let finalDeltas = deltas |> apply x.Content

                    if not (List.isEmpty finalDeltas) then
                        for cb in x.Callbacks do cb finalDeltas

                    finalDeltas
                )
            )

        override x.ComputeDelta() =
            inputReader.Update()
            inputReader.Outputs.Add x |> ignore

            match reset with
                | Some c ->
                    let content = x.Content
                    reset <- None
                    deltas.Clear()
                    let add = c |> Seq.filter (not << content.Contains) |> Seq.map Add
                    let rem = content |> Seq.filter (not << c.Contains) |> Seq.map Rem

                    Seq.append add rem |> Seq.toList
                | None ->
                    let res = deltas |> Seq.toList
                    deltas.Clear()
                    res

        override x.Release() =
            inputReader.RemoveOutput x
            subscription.Dispose()
            dispose(x)

    type OneShotReader<'a>(deltas : Change<'a>) =  
        inherit AbstractReader<'a>()
        
        let mutable deltas = deltas

        override x.ComputeDelta() =
            let res = deltas
            deltas <- []
            res

        override x.Release() =
            deltas <- []


    type UseReader<'a when 'a :> IDisposable>(inputReader : IReader<'a>) as this =
        inherit AbstractReader<'a>()
        do inputReader.AddOutput this

        override x.Release() =
            for c in x.Content do
                try c.Dispose() with _ -> ()
            inputReader.RemoveOutput x
            inputReader.Dispose()

        override x.ComputeDelta() =
            let deltas = inputReader.GetDelta()

            for d in deltas do
                match d with
                    | Rem v -> v.Dispose()
                    | _ -> ()

            deltas




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

    let using (r : IReader<'a>) =
        new UseReader<'a>(r) :> IReader<_>