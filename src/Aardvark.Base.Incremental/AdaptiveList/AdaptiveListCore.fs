namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open Aardvark.Base

type IListReader<'a> =
    inherit IDisposable
    inherit IAdaptiveObject
    abstract member RootTime : Time
    abstract member Content : TimeList<'a>
    abstract member GetDelta : unit -> Change<Time * 'a>

[<CompiledName("IAdaptiveList")>]
type alist<'a> =
    abstract member GetReader : unit -> IListReader<'a>


module TimeMappings =

    type ITimeMapping =
        inherit IDisposable
        abstract member GetTime : Time -> Time
        abstract member TryGetTime : Time -> Option<Time>
        abstract member RemoveTime : Time -> unit

    type SparseTimeMapping private(root : Time) =
        let mutable tree : AVL.Tree<Time * ref<Time>> = AVL.custom (fun (l,_) (r,_) -> compare l r)
        let cache = Dictionary<Time, Time>()

        let getTime (t : Time) =
            match cache.TryGetValue t with
                | (true, r) -> r
                | _ ->
                    let result = ref (Unchecked.defaultof<Time>)
                    let value = (t, result)

                    AVL.insertNeighbourhood tree value (fun l r -> 
                        result :=
                            match l with
                                | Some (_,l) -> Time.after !l
                                | None -> Time.after root
                    ) |> ignore

                    cache.Add(t, !result)

                    !result

        member x.Dispose() =
            cache.Clear()
            if root.Prev <> root then
                Time.deleteRangeOpenClosed root root.Prev
            tree <- AVL.custom (fun (l,_) (r,_) -> compare l r)

        member x.GetTime (t : Time) =
            getTime t

        member x.TryGetTime (t : Time) =
            match cache.TryGetValue t with
                | (true,t) -> Some t
                | _ -> None

        member x.RemoveTime(t : Time) =
            match cache.TryGetValue t with
                | (true, r) ->
                    AVL.removeCmp tree (fun (_,t) -> compare r !t) |> ignore
                    cache.Remove t |> ignore
                    Time.delete r

                | _ -> ()

        static member Create (root : Time) =
            new SparseTimeMapping(root)

        interface ITimeMapping with
            member x.Dispose() = x.Dispose()
            member x.GetTime t = x.GetTime t
            member x.TryGetTime t = x.TryGetTime t
            member x.RemoveTime t = x.RemoveTime t

    type NestedTimeMapping private(root : Time) =
        let cmp (lo : Time, li : Time) (ro : Time, ri : Time) =
            let o = compare lo ro
            if o = 0 then
                compare li ri
            else
                o

        let mutable tree : AVL.Tree<(Time * Time) * ref<Time>> = AVL.custom (fun (l,_) (r,_) -> cmp l r)

        let cache = Dictionary<Time * Time, Time>()

        let getTime (outer : Time) (inner : Time) =
            let t = (outer, inner)
            match cache.TryGetValue t with
                | (true, r) -> r
                | _ ->
                    let result = ref (Unchecked.defaultof<Time>)
                    let value = (t, result)

                    AVL.insertNeighbourhood tree value (fun l r -> 
                        result :=
                            match l with
                                | Some (_,l) -> Time.after !l
                                | None -> Time.after root
                    ) |> ignore

                    cache.Add(t, !result)

                    !result

        member x.Dispose() =
            cache.Clear()
            tree <- AVL.custom (fun (l,_) (r,_) -> cmp l r)

        member x.GetTime (outer : Time) (inner : Time) =
            getTime outer inner

        member x.TryGetTime (outer : Time) (inner : Time) =
            match cache.TryGetValue( (outer, inner) ) with
                | (true,t) -> Some t
                | _ -> None

        member x.RemoveTime (outer : Time) (inner : Time) =
            let t = (outer, inner)
            match cache.TryGetValue t with
                | (true, r) ->
                    AVL.removeCmp tree (fun (_,t) -> compare r !t) |> ignore
                    cache.Remove t |> ignore
                    Time.delete r

                | _ -> ()

        member x.RemoveAllTimes (outer : Time) =
            let allTimes = cache |> Seq.filter (fun (KeyValue((o,i),v)) -> o = outer) |> Seq.toList

            if not <| List.isEmpty allTimes then
                for (KeyValue(t, r)) in allTimes do
                    cache.Remove t |> ignore
                    AVL.removeCmp tree (fun (_,t) -> compare r !t) |> ignore
                    cache.Remove t |> ignore
                    Time.delete r

        member x.GetPartial (outer : Time) =
            NestedTimeMappingPartial.Create(x, outer)

        static member Create (root : Time) =
            NestedTimeMapping(root)  

    and NestedTimeMappingPartial private (mapping : NestedTimeMapping, outer : Time) =
        member x.Dispose() = mapping.RemoveAllTimes outer

        member x.GetTime (t : Time) =
            mapping.GetTime outer t

        member x.TryGetTime (t : Time) =
            mapping.TryGetTime outer t

        member x.RemoveTime(t : Time) =
            mapping.RemoveTime outer t

        static member Create (mapping : NestedTimeMapping, outer : Time) =
            new NestedTimeMappingPartial(mapping, outer)  
             
        interface ITimeMapping with
            member x.Dispose() = x.Dispose()
            member x.GetTime t = x.GetTime t
            member x.TryGetTime t = x.TryGetTime t
            member x.RemoveTime t = x.RemoveTime t

module AListReaders =

    let apply (set : TimeList<'a>) (deltas : list<Delta<Time * 'a>>) =
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
        let dirty = HashSet<Time * IListReader<'a>>()
        let occurances = Dictionary<IListReader<'a>, HashSet<Time>>()

        let addOccurance (r : IListReader<'a>) (t : Time) =
            match occurances.TryGetValue r with
                | (true, times) ->
                    times.Add t |> ignore
                    false
                | _ ->
                    let times = HashSet [t]
                    occurances.[r] <- times
                    true

        let removeOccurance (r : IListReader<'a>) (t : Time) =
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
                | (true, times) -> times :> seq<Time>
                | _ -> Seq.empty

        let start (r : IListReader<'a>) (t : Time) =
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

        let stop (r : IListReader<'a>) (t : Time) =
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

        member x.Listen(t : Time, r : IListReader<'a>) = start r t
            
        member x.Destroy(t : Time, r : IListReader<'a>) = stop r t

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

    type CollectReader<'a, 'b>(scope, input : IListReader<'a>, f : 'a -> IListReader<'b>) as this =
        inherit AdaptiveObject()
        do input.AddOutput this

        let content = TimeList<'b>()
        let dirtyInner = new DirtyListReaderSet<'b>()
        let f = Cache<'a, IListReader<'b>>(scope, f)

        let mutable rootTime = Time.newRoot()
        let mapping = TimeMappings.NestedTimeMapping.Create(rootTime)

        let outputTime (outer : Time) (inner : Time) =
            mapping.GetTime outer inner

        let removeOutputTime (outer : Time) (inner : Time) =
            match mapping.TryGetTime outer inner with
                | Some t ->
                    mapping.RemoveTime outer inner
                    t
                | None ->
                    failwith "unknown time"

        member x.Dispose() =
            input.RemoveOutput x
            input.Dispose()
            f.Clear(fun r -> r.RemoveOutput x; r.Dispose())
            content.Clear()
            dirtyInner.Dispose()
            mapping.Dispose()
            rootTime <- Unchecked.defaultof<Time>

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IListReader<'b> with
            member x.RootTime = rootTime
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
                                    let additions = r.Content |> Seq.map (fun (i,v) -> Add(outputTime t i, v)) |> Seq.toList
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
                                    let removals = r.Content |> Seq.map (fun (i,v) -> Rem(outputTime t i, v))  |> Seq.toList

                                    // remove all times created for this specific occurance of r
                                    mapping.RemoveAllTimes t
                                    removals
                        )

                    // all dirty inner readers must be registered 
                    // in dirtyInner. Even if the outer set did not change we
                    // need to collect those inner deltas.
                    let innerDeltas = 
                        dirtyInner.GetDeltas() |> List.map (fun d ->
                            match d with
                                | Add (o,i,v) -> Add(outputTime o i, v)
                                | Rem (o,i,v) -> Rem(removeOutputTime o i, v)
                                
                        )

                    // concat inner and outer deltas 
                    let deltas = List.append outerDeltas innerDeltas

                    deltas |> apply content
                )

    type ChooseReader<'a, 'b>(scope, input : IListReader<'a>, f : 'a -> Option<'b>) as this =
        inherit AdaptiveObject()

        do input.AddOutput this

        let content = TimeList<'b>()
        let f = Cache<'a, Option<'b>>(scope, f)

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
                        deltas |> List.choose (fun d ->
                            match d with
                                | Add(t, v) -> 
                                    match f.Invoke v with
                                        | Some v -> Add (t, v) |> Some
                                        | None -> None
                                | Rem(t, v) -> 
                                    match f.Revoke v with
                                        | Some v -> Rem (t, v) |> Some
                                        | None -> None
                        )

                    deltas |> apply content
                )

    type ModReader<'a>(source : IMod<'a>) as this =  
        inherit AdaptiveObject()
        do source.AddOutput this
        let mutable old : Option<Time * 'a> = None
        let content = TimeList()
        let mutable rootTime = Time.newRoot()
        let hasChanged = ChangeTracker.track<'a>

        member x.Dispose() =
            source.RemoveOutput x
            match old with
                | Some (t,_) ->
                    Time.delete t
                    old <- None
                | None -> ()
            content.Clear()
            Time.delete rootTime
            rootTime <- Unchecked.defaultof<Time>

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IListReader<'a> with
            member x.RootTime = rootTime
            member x.Content = content
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let v = source.GetValue()
                    let resultDeltas =
                        if hasChanged v then
                            match old with
                                | None ->
                                    let t = Time.after rootTime
                                    old <- Some (t,v)
                                    [Add (t,v)]
                                | Some (t,c) ->
                                    let tNew = Time.after rootTime
                                    Time.delete t
                                    old <- Some (tNew, v)
                                    [Rem (t, c); Add (tNew, v)]
                        else
                            []

                    resultDeltas |> apply content
                )

    type BufferedReader<'a>(rootTime : Time, update : unit -> unit, dispose : BufferedReader<'a> -> unit) =
        inherit AdaptiveObject()
        let deltas = List()
        let mutable reset : Option<ICollection<Time * 'a>> = None

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

        member x.Reset(c : ICollection<Time * 'a>) =
            reset <- Some c
            deltas.Clear()
            x.MarkOutdated()

        member x.Emit (d : list<Delta<Time * 'a>>) =
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


    let map (scope) (f : 'a -> 'b) (input : IListReader<'a>) =
        new MapReader<_, _>(scope, f, input) :> IListReader<_>

    let bind scope (f : 'a -> IListReader<'b>) (input : IMod<'a>) =
        new CollectReader<_,_>(scope, new ModReader<_>(input), f) :> IListReader<_>

    let bind2 scope (f : 'a -> 'b -> IListReader<'c>) (ma : IMod<'a>)  (mb : IMod<'b>)=
        let tup = Mod.map2 (fun a b -> (a,b)) ma mb
        new CollectReader<_,_>(scope, new ModReader<_>(tup),  fun (a,b) -> f a b) :> IListReader<_>

    let collect (scope) (f : 'a -> IListReader<'b>) (input : IListReader<'a>) =
        new CollectReader<_, _>(scope, input, f) :> IListReader<_>

    let choose (scope) (f : 'a -> Option<'b>) (input : IListReader<'a>) =
        new ChooseReader<_, _>(scope, input, f) :> IListReader<_>


    let ofMod (m : IMod<'a>) : IListReader<'a> =
        new ModReader<'a>(m) :> IListReader<'a>