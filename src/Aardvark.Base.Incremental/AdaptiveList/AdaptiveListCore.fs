namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open Aardvark.Base

/// <summary>
/// IListReader is the base interface for all adaptive list-readers.
/// Readers are stateful and may not be used by multiple callers
/// since pulling changes mutates the reader internally.
/// </summary>
type IListReader<'a> =
    inherit IDisposable
    inherit IAdaptiveObject

    /// <summary>
    /// The root-time used for all deltas returned by the reader
    /// NOTE that each time is associated with a value except for the 
    ///      root-time itself
    /// </summary>
    abstract member RootTime : IOrder
    
    /// <summary>
    /// The reader's current content. 
    /// All changes returned by GetDelta are "relative" to that state.
    /// NOTE that calling GetDelta modifies the reader's content
    /// </summary>
    abstract member Content : TimeList<'a>

    /// <summary>
    /// Pulls the reader's deltas relative to its current content.
    /// NOTE that GetDelta also "applies" the deltas to the reader's state
    /// </summary>
    abstract member GetDelta : unit -> Change<ISortKey * 'a>

/// <summary>
/// alist serves as the base interface for all adaptive lists.
/// </summary>
[<CompiledName("IAdaptiveList")>]
type alist<'a> =
    /// <summary>
    /// Returns a NEW reader for the list which will initially return
    /// the entire list-content as deltas.
    /// </summary>
    abstract member GetReader : unit -> IListReader<'a>


module AListReaders =

    let apply (set : TimeList<'a>) (deltas : list<Delta<ISortKey * 'a>>) =
        deltas 
            |> Delta.clean 
            |> List.map (fun d ->
                match d with
                    | Add v -> set.Add v
                    | Rem (t,_) -> set.Remove t |> ignore

                d
               )

    type DirtyListReaderSet<'a>() =
        let subscriptions = Dictionary<IListReader<'a>, unit -> unit>()
        let dirty = HashSet<ISortKey * IListReader<'a>>()
        let occurances = Dictionary<IListReader<'a>, HashSet<ISortKey>>()

        let addOccurance (r : IListReader<'a>) (t : ISortKey) =
            match occurances.TryGetValue r with
                | (true, times) ->
                    times.Add t |> ignore
                    false
                | _ ->
                    let times = HashSet [t]
                    occurances.[r] <- times
                    true

        let removeOccurance (r : IListReader<'a>) (t : ISortKey) =
            match occurances.TryGetValue r with
                | (true, times) ->
                    if times.Remove t then
                        if times.Count = 0 then
                            occurances.Remove r |> ignore
                            true
                        else
                            false
                    else
                        false
                | _ ->
                    false

        let allOccurances (r : IListReader<'a>) =
            match occurances.TryGetValue r with
                | (true, times) -> times :> seq<ISortKey>
                | _ -> Seq.empty

        let start (r : IListReader<'a>) (t : ISortKey) =
            if addOccurance r t then
                // create and register a callback function
                let onMarking () =
                    for t in allOccurances r do 
                        dirty.Add(t,r) |> ignore
                        subscriptions.Remove r |> ignore

                r.MarkingCallbacks.Add onMarking |> ignore

                // store the "subscription" for each reader in order
                // to allow removals of readers
                subscriptions.[r] <- fun () -> r.MarkingCallbacks.Remove onMarking |> ignore

                // if the reader is currently outdated add it to the
                // dirty set immediately
                lock r (fun () -> if r.OutOfDate then dirty.Add (t,r) |> ignore)
                true
            else
                false

        let stop (r : IListReader<'a>) (t : ISortKey) =
            if removeOccurance r t then
                // if there exists a subscription we remove and dispose it
                match subscriptions.TryGetValue r with
                    | (true, d) -> 
                        subscriptions.Remove r |> ignore
                        d()
                    | _ -> ()

                // if the reader is already in the dirty-set remove it from there too
                dirty.Remove(t,r) |> ignore
                true
            else
                false

        member x.Listen(t : ISortKey, r : IListReader<'a>) = start r t
            
        member x.Destroy(t : ISortKey, r : IListReader<'a>) = stop r t

        member x.GetDeltas() =
            [ for (t,d) in dirty do
                // get deltas for all dirty readers and re-register
                // marking callbacks
                let c = d.GetDelta()
                x.Listen(t,d) |> ignore
                yield! c |> List.map (fun d -> 
                                match d with
                                    | Add(i,v) -> Add(t, i,v)
                                    | Rem(i,v) -> Rem(t,i,v)
                            )
            ]

        member x.Dispose() =
            occurances.Clear()
            for (KeyValue(_, d)) in subscriptions do d()
            dirty.Clear()
            subscriptions.Clear()

        interface IDisposable with
            member x.Dispose() = x.Dispose()
 

    type MapReader<'a, 'b>(scope, f : 'a -> 'b, input : IListReader<'a>) as this =
        inherit AdaptiveObject()
        do input.AddOutput this
        
        let content = TimeList<'b>()
        let f = Cache<'a, 'b>(scope, f)

        member x.Dispose() =
            input.RemoveOutput this
            input.Dispose()
            content.Clear()
            f.Clear(ignore)

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IListReader<'b> with
            member x.RootTime = input.RootTime
            member x.Content = content
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let deltas = input.GetDelta()

                    let deltas = 
                        deltas |> List.map (fun d ->
                            match d with
                                | Add(t, v) -> Add (t, f.Invoke v)
                                | Rem(t, v) -> Rem (t, f.Revoke v)
                        )

                    deltas |> apply content
                )

    type MapKeyReader<'a, 'b>(scope, f : ISortKey -> 'a -> 'b, input : IListReader<'a>) as this =
        inherit AdaptiveObject()
        do input.AddOutput this
        
        let content = TimeList<'b>()
        let f = Cache<ISortKey * 'a, 'b>(scope, fun (k,v) -> f k v)

        member x.Dispose() =
            input.RemoveOutput this
            input.Dispose()
            content.Clear()
            f.Clear(ignore)

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IListReader<'b> with
            member x.RootTime = input.RootTime
            member x.Content = content
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let deltas = input.GetDelta()

                    let deltas = 
                        deltas |> List.map (fun d ->
                            match d with
                                | Add(t, v) -> Add (t, f.Invoke (t,v))
                                | Rem(t, v) -> Rem (t, f.Revoke (t,v))
                        )

                    deltas |> apply content
                )



    type CollectReader<'a, 'b>(scope, input : IListReader<'a>, f : 'a -> IListReader<'b>) as this =
        inherit AdaptiveObject()
        do input.AddOutput this

        let content = TimeList<'b>()
        let dirtyInner = new DirtyListReaderSet<'b>()
        let f = Cache<'a, IListReader<'b>>(scope, f)

        let mutable mapping = NestedOrderMapping()
        let mutable rootTime = mapping.Root

        member x.Dispose() =
            input.RemoveOutput x
            input.Dispose()
            f.Clear(fun r -> r.RemoveOutput x; r.Dispose())
            content.Clear()
            dirtyInner.Dispose()
            mapping.Clear()
            rootTime <- Unchecked.defaultof<ISortKey>

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IListReader<'b> with
            member x.RootTime = mapping.Order
            member x.Content = content
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let xs = input.GetDelta()
                
                    let outerDeltas =
                        xs |> List.collect (fun d ->
                            match d with
                                | Add (t,v) ->
                                    let r = f.Invoke v

                                    // bring the reader's content up-to-date by calling GetDelta
                                    r.GetDelta() |> ignore

                                    // listen to marking of r (reader cannot be OutOfDate due to GetDelta above)
                                    if dirtyInner.Listen(t, r) then
                                        // we're an output of the new reader
                                        r.AddOutput x

                                    // since the entire reader is new we add its content
                                    // which must be up-to-date here (due to calling GetDelta above)
                                    let additions = r.Content |> Seq.map (fun (i,v) -> Add(mapping.Invoke(t, i), v)) |> Seq.toList
                                    additions

                                | Rem (t,v) ->
                                    let r = f.Revoke v
                                
                                    // remove the reader-occurance from the listen-set
                                    if dirtyInner.Destroy(t, r) then
                                        // since the reader is no longer contained we don't want
                                        // to be notified anymore
                                        r.RemoveOutput x

                                    // the entire content of the reader is removed
                                    // Note that the content here might be OutOfDate
                                    // TODO: think about implications here when we do not "own" the reader
                                    //       exclusively
                                    let removals = r.Content |> Seq.map (fun (i,v) -> Rem(mapping.Revoke(t, i), v))  |> Seq.toList

                                    // remove all times created for this specific occurance of r
                                    mapping.RevokeAll t |> ignore
                                    removals
                        )

                    // all dirty inner readers must be registered 
                    // in dirtyInner. Even if the outer set did not change we
                    // need to collect those inner deltas.
                    let innerDeltas = 
                        dirtyInner.GetDeltas() |> List.map (fun d ->
                            match d with
                                | Add (o,i,v) -> Add(mapping.Invoke(o, i), v)
                                | Rem (o,i,v) -> Rem(mapping.Revoke(o, i), v)
                                
                        )

                    // concat inner and outer deltas 
                    let deltas = List.append outerDeltas innerDeltas

                    deltas |> apply content
                )

    type ChooseReader<'a, 'b>(scope, input : IListReader<'a>, f : 'a -> Option<'b>) as this =
        inherit AdaptiveObject()

        do input.AddOutput this

        
        let mapping = OrderMapping()
        let root = mapping.Root
        let content = TimeList<'b>()
        let f = Cache<'a, Option<'b>>(scope, f)

        member x.Dispose() =
            input.RemoveOutput this
            input.Dispose()
            content.Clear()
            mapping.Clear()
            f.Clear(ignore)

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IListReader<'b> with
            member x.RootTime = mapping.Order
            member x.Content = content
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let deltas = input.GetDelta()

                    let deltas = 
                        deltas |> List.choose (fun d ->
                            match d with
                                | Add(t, v) -> 
                                    match f.Invoke v with
                                        | Some v -> 
                                            let t' = mapping.Invoke t
                                            Add (t', v) |> Some
                                        | None -> None
                                | Rem(t, v) -> 
                                    match f.Revoke v with
                                        | Some v -> 
                                            let t' = mapping.Revoke t
                                            Rem (t', v) |> Some
                                        | None -> None
                        )

                    deltas |> apply content
                )

    type ModReader<'a>(source : IMod<'a>) as this =  
        inherit AdaptiveObject()
        do source.AddOutput this
        let mutable old : Option<SimpleOrder.SortKey * 'a> = None
        let content = TimeList()
        let mutable order = SimpleOrder.create()
        let hasChanged = ChangeTracker.track<'a>

        member x.Dispose() =
            source.RemoveOutput x
            match old with
                | Some (t,_) ->
                    order.Delete t
                    old <- None
                | None -> ()
            content.Clear()


        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IListReader<'a> with
            member x.RootTime = order :> IOrder
            member x.Content = content
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let v = source.GetValue()
                    let resultDeltas =
                        if hasChanged v then
                            match old with
                                | None ->
                                    let t = order.After order.Root //Time.after rootTime
                                    old <- Some (t,v)
                                    [Add (t :> ISortKey,v)]
                                | Some (t,c) ->
                                    let tNew = order.After order.Root
                                    order.Delete t
                                    old <- Some (tNew, v)
                                    [Rem (t :> ISortKey, c); Add (tNew :> ISortKey, v)]
                        else
                            []

                    resultDeltas |> apply content
                )

    type SetReader<'a>(input : IReader<'a>) as this =
        inherit AdaptiveObject()
        do input.AddOutput this
        
        let order = SimpleOrder.create()
        let times = Dictionary<obj, SimpleOrder.SortKey>()
        let content = TimeList()

        member x.Dispose() =
            input.RemoveOutput x
            order.Clear()
            content.Clear()

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        member x.GetDelta() =
            x.EvaluateIfNeeded [] (fun () ->
                let deltas = input.GetDelta()

                let listDeltas =
                    deltas |> List.map (fun d ->
                        match d with
                            | Add v -> 
                                let t = order.After order.Root.Prev
                                times.Add(d, t)
                                Add(t :> ISortKey, v)
                            | Rem v ->
                                match times.TryGetValue v with
                                    | (true, t) ->
                                        order.Delete t
                                        Rem(t :> ISortKey, v)
                                    | _ -> 
                                        failwith "removal of unknown value"
                    )

                listDeltas |> apply content
            )

        interface IListReader<'a> with
            member x.GetDelta() = x.GetDelta()
            member x.Content = content
            member x.RootTime = order :> IOrder
            member x.Dispose() = x.Dispose()

    type BufferedReader<'a>(rootTime : IOrder, update : unit -> unit, dispose : BufferedReader<'a> -> unit) =
        inherit AdaptiveObject()
        let deltas = List()
        let mutable reset : Option<ICollection<ISortKey * 'a>> = None

        let content = TimeList<'a>()

        
        let getDeltas() =
            match reset with
                | Some c ->
                    reset <- None
                    let add = c |> Seq.filter (not << content.Contains) |> Seq.map Add
                    let rem = content |> Seq.filter (not << c.Contains) |> Seq.map Rem

                    Seq.append add rem |> Seq.toList
                | None ->
                    let res = deltas |> Seq.toList
                    deltas.Clear()
                    res

        member x.IsIncremental = 
            true

        member x.Reset(c : ICollection<ISortKey * 'a>) =
            reset <- Some c
            deltas.Clear()
            x.MarkOutdated()

        member x.Emit (d : list<Delta<ISortKey * 'a>>) =
            deltas.AddRange d
            x.MarkOutdated()

        new(rootTime, dispose) = new BufferedReader<'a>(rootTime, id, dispose)
        new(rootTime) = new BufferedReader<'a>(rootTime, id, ignore)

        member x.Dispose() =
            dispose(x)

        override x.Finalize() = 
            try x.Dispose()
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IListReader<'a> with
            member x.RootTime = rootTime
            member x.GetDelta() =
                update()
                x.EvaluateIfNeeded [] (fun () ->
                    let l = getDeltas()
                    l |> apply content
                )

            member x.Content = content

    type SortWithReader<'a when 'a : equality>(input : IReader<'a>, cmp : 'a -> 'a -> int) as this =
        inherit AdaptiveObject()
        do input.AddOutput this

        let content = TimeList<'a>()
        let tree = OrderMaintenance<'a>(cmp)
        let root = tree.Root

        member x.Dispose() =
            input.RemoveOutput this
            input.Dispose()
            content.Clear()
            

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IListReader<'a> with
            member x.RootTime = tree.Order
            member x.Content = content
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let deltas = input.GetDelta()

                    let deltas = 
                        deltas |> List.map (fun d ->
                            match d with
                                | Add v ->
                                    let t = tree.Invoke v
                                    Add(t, v)
                                | Rem v -> 
                                    let t = tree.Revoke v
                                    Rem (t, v)
                        )

                    deltas |> apply content
                )

    type ListSetReader<'a>(input : IListReader<'a>) as this =
        inherit AdaptiveObject()

        do input.AddOutput this
        let content = ReferenceCountingSet()

        member x.Dispose() =
            input.RemoveOutput x
            content.Clear()

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        member x.GetDelta() =
            x.EvaluateIfNeeded [] (fun () ->
                let deltas = input.GetDelta()

                let setDeltas =
                    deltas |> List.map (fun d ->
                        match d with
                            | Add (_,v) -> 
                                Add v
                            | Rem (_,v) ->
                                Rem v
                    )

                setDeltas 
                    |> Delta.clean 
                    |> ASetReaders.apply content
            )

        interface IReader<'a> with
            member x.Update() = x.GetDelta() |> ignore
            member x.GetDelta() = x.GetDelta()
            member x.Content = content
            member x.Dispose() = x.Dispose()



    let map (scope) (f : 'a -> 'b) (input : IListReader<'a>) =
        new MapReader<_, _>(scope, f, input) :> IListReader<_>

    let mapKey (scope) (f : ISortKey -> 'a -> 'b) (input : IListReader<'a>) =
        new MapKeyReader<_, _>(scope, f, input) :> IListReader<_>


    let bind scope (f : 'a -> IListReader<'b>) (input : IMod<'a>) =
        new CollectReader<_,_>(scope, new ModReader<_>(input), f) :> IListReader<_>

    let bind2 scope (f : 'a -> 'b -> IListReader<'c>) (ma : IMod<'a>)  (mb : IMod<'b>)=
        let tup = Mod.map2 (fun a b -> (a,b)) ma mb
        new CollectReader<_,_>(scope, new ModReader<_>(tup),  fun (a,b) -> f a b) :> IListReader<_>

    let collect (scope) (f : 'a -> IListReader<'b>) (input : IListReader<'a>) =
        new CollectReader<_, _>(scope, input, f) :> IListReader<_>

    let choose (scope) (f : 'a -> Option<'b>) (input : IListReader<'a>) =
        new ChooseReader<_, _>(scope, input, f) :> IListReader<_>

    let sortWith (cmp : 'a -> 'a -> int) (input : IReader<'a>) =
        new SortWithReader<'a>(input, cmp) :> IListReader<_>

    let ofMod (m : IMod<'a>) : IListReader<'a> =
        new ModReader<'a>(m) :> IListReader<'a>

    let ofSet (m : aset<'a>) : IListReader<'a> =
        new SetReader<'a>(m.GetReader()) :> IListReader<'a>

    let toSetReader (m : alist<'a>) : IReader<'a> =
        new ListSetReader<'a>(m.GetReader()) :> IReader<'a>