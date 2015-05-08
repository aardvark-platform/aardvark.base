namespace Aardvark.Base.Incremental


open System
open System.Threading
open System.Collections
open System.Collections.Generic
open System.Collections.Concurrent
open System.Runtime.CompilerServices

type Change<'a> = list<Delta<'a>>



type IReader<'a> =
    inherit IDisposable
    inherit IAdaptiveObject
    abstract member Content : ReferenceCountingSet<'a>
    abstract member GetDelta : unit -> Change<'a>

[<CompiledName("IAdaptiveSet")>]
type aset<'a> =
    abstract member GetReader : unit -> IReader<'a>



module private ASetReaders =


    let apply (set : ReferenceCountingSet<'a>) (deltas : list<Delta<'a>>) =
        deltas 
            |> Delta.clean 
            |> List.filter (fun d ->
                match d with
                    | Add v -> set.Add v
                    | Rem v -> set.Remove v
               )

    type DirtyReaderSet<'a>() =
        let subscriptions = Dictionary<IReader<'a>, unit -> unit>()
        let dirty = HashSet<IReader<'a>>()

        member x.Listen(r : IReader<'a>) =
            // create and register a callback function
            let onMarking () = 
                dirty.Add r |> ignore
                subscriptions.Remove r |> ignore

            r.MarkingCallbacks.Add onMarking |> ignore

            // store the "subscription" for each reader in order
            // to allow removals of readers
            subscriptions.[r] <- fun () -> r.MarkingCallbacks.Remove onMarking |> ignore

            // if the reader is currently outdated add it to the
            // dirty set immediately
            lock r (fun () -> if r.OutOfDate then dirty.Add r |> ignore)
            
        member x.Destroy(r : IReader<'a>) =
            // if there exists a subscription we remove and dispose it
            match subscriptions.TryGetValue r with
                | (true, d) -> 
                    subscriptions.Remove r |> ignore
                    d()
                | _ -> ()

            // if the reader is already in the dirty-set remove it from there too
            dirty.Remove r |> ignore

        member x.GetDeltas() =
            [ for d in dirty do
                // get deltas for all dirty readers and re-register
                // marking callbacks
                let c = d.GetDelta()
                x.Listen d
                yield! c
            ]

        member x.Dispose() =
            for (KeyValue(_, d)) in subscriptions do d()
            dirty.Clear()
            subscriptions.Clear()

        interface IDisposable with
            member x.Dispose() = x.Dispose()
 

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

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'b> with
            member x.Content = content
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

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'b> with
            member x.Content = content
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
                                    dirtyInner.Destroy r

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

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'b> with
            member x.Content = content
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

    type ModReader<'a>(source : IMod<'a>) as this =  
        inherit AdaptiveObject()
        do source.AddOutput this
        let mutable cache : Option<'a> = None
        let content = ReferenceCountingSet()

        member x.Dispose() =
            source.RemoveOutput this
            cache <- None

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'a> with
            member x.Content = content
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let v = source.GetValue()
                    let resultDeltas =
                        match cache with
                            | Some c ->
                                if not <| System.Object.Equals(v, c) then
                                    cache <- Some v
                                    [Rem c; Add v]
                                else
                                    []
                            | None ->
                                cache <- Some v
                                [Add v]

                    resultDeltas |> apply content
                )

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
            match d with 
                | Some d ->
                    deltas.AddRange d
//                    let N = c.Count
//                    let M = content.Count
//                    let D = deltas.Count + (List.length d)
//                    if D > N + 2 * M then
//                        reset <- Some c
//                        deltas.Clear()
//                    else
//                        deltas.AddRange d

                | None ->
                    reset <- Some c
                    deltas.Clear()

            x.MarkOutdated()


        new(dispose) = new BufferedReader<'a>(id, dispose)
        new() = new BufferedReader<'a>(id, ignore)

        member x.Dispose() =
            dispose(x)

        override x.Finalize() = 
            try x.Dispose()
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IReader<'a> with
            member x.GetDelta() =
                update()
                x.EvaluateIfNeeded [] (fun () ->
                    let l = getDeltas()
                    l |> apply content
                )

            member x.Content = content


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