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

[<CompiledName("AdaptiveSet")>]
type aset<'a> =
    abstract member GetReader : unit -> IReader<'a>



module ASetReaders =


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
            if r.OutOfDate then dirty.Add r |> ignore
            
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
 

    type MapReader<'a, 'b>(source : IReader<'a>, f : 'a -> list<'b>) as this =
        inherit AdaptiveObject()
        do source.AddOutput this  
        
        let f = Cache(f)
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
                if x.OutOfDate then
                    
                    let input = source.GetDelta()
                    
                    let result = 
                        input |> List.collect (fun d ->
                            match d with
                                | Add v -> f.Invoke v |> List.map Add
                                | Rem v -> f.Revoke v |> List.map Rem
                        ) |> apply content

                    x.OutOfDate <- false
                    result

                else
                    []       
          
    type CollectReader<'a, 'b>(source : IReader<'a>, f : 'a -> IReader<'b>) as this =
        inherit AdaptiveObject()
        do source.AddOutput this

        let f = Cache(f)
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
                if x.OutOfDate then

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

                    x.OutOfDate <- false
                    deltas |> apply content

                else
                    []

    type ChooseReader<'a, 'b>(source : IReader<'a>, f : 'a -> Option<'b>) as this =
        inherit AdaptiveObject()
        do source.AddOutput this

        let f = Cache(f)
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
                if x.OutOfDate then

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

                    x.OutOfDate <- false
                    resultDeltas |> apply content

                else
                    []

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
                if x.OutOfDate then

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

                    x.OutOfDate <- false

                    resultDeltas |> apply content
                else
                    []

    type BindReader<'a, 'b>(source : IMod<'a>, f : 'a -> IReader<'b>) as this =
        inherit AdaptiveObject()
        do source.AddOutput this

        let f = Cache(f)
        let mutable cache = None
        let content = ReferenceCountingSet<'b>()
        let dirtyInner = new DirtyReaderSet<'b>()

        member x.Dispose() =
            source.RemoveOutput this
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
                if x.OutOfDate then

                    let v = source.GetValue()
                    let deltas =
                        match cache with
                            | Some c ->
                                if System.Object.Equals(c,v) then []
                                else 
                                    cache <- Some v
                                    [Add v; Rem c]
                            | None ->
                                cache <- Some v
                                [Add v]


                    let outerDeltas =
                        deltas |> List.collect (fun d ->
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

                    x.OutOfDate <- false
                    deltas |> apply content

                else
                    []

    type BufferedReader<'a>(update : unit -> unit, dispose : BufferedReader<'a> -> unit) =
        inherit AdaptiveObject()
        let deltas = List()
        let mutable reset : Option<ICollection<'a>> = None

        let content = ReferenceCountingSet<'a>()

        
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

        member x.Reset(c : ICollection<'a>) =
            reset <- Some c
            deltas.Clear()
            x.MarkOutdated()

        member x.Emit (d : list<Delta<'a>>) =
            deltas.AddRange d
            
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
                if x.OutOfDate then
                    let l = getDeltas()
                    x.OutOfDate <- false
                    l |> apply content
                else
                    []

            member x.Content = content

    let map (f : 'a -> 'b) (input : IReader<'a>) =
        new MapReader<_, _>(input, fun c -> [f c]) :> IReader<_>

    let collect (f : 'a -> IReader<'b>) (input : IReader<'a>) =
        new CollectReader<_, _>(input, f) :> IReader<_>

    let bind (f : 'a -> IReader<'b>) (input : IMod<'a>) =
        new BindReader<_, _>(input, f) :> IReader<_>


    let choose (f : 'a -> Option<'b>) (input : IReader<'a>) =
        new ChooseReader<_, _>(input, f) :> IReader<_>

    let ofMod (m : IMod<'a>) =
        new ModReader<_>(m) :> IReader<_>