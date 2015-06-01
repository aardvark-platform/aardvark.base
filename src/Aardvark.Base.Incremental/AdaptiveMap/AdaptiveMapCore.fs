namespace Aardvark.Base.Incremental

open System
open System.Collections.Generic
open Aardvark.Base
open System.Runtime.InteropServices
open System.Runtime.CompilerServices


type MapDelta<'k, 'v> =
    | Set of 'k * 'v
    | Remove of 'k * 'v

type IMapReader<'k, 'v when 'k : equality> =
    inherit IDisposable
    inherit IAdaptiveObject
    abstract member Content : IVersionedDictionary<'k, 'v>
    abstract member GetDelta : unit -> list<MapDelta<'k, 'v>>


[<CompiledName("IAdaptiveMap")>]
type amap<'k, 'v when 'k : equality> =
    abstract member GetReader : unit -> IMapReader<'k, 'v>

module private AMapReaders =

//    let apply (set : Dictionary<'k, 'v>) (deltas : list<MapDelta<'k, 'v>>) =
//        deltas 
//            |> List.filter (fun d ->
//                match d with
//                    | Set(k,v) -> set.[k] <- v; true
//                    | Remove(k,v) -> set.Remove k
//               )

    
    let takeNew _ b = b

    let clean (conflict : 'v -> 'v -> 'v) (deltas : list<MapDelta<'k, 'v>>) =
        // TODO: implement
        deltas

    let apply (conflict : 'v -> 'v -> 'v) (content : IDictionary<'k, 'v>) (deltas : list<MapDelta<'k, 'v>>) =
        deltas 
            |> clean conflict
            |> List.filter (fun d ->
                match d with
                    | Set(k,v) -> 
                        match content.TryGetValue k with
                            | (true, old) ->
                                content.[k] <- v
                                not <| System.Object.Equals(v,old)
                            | _ ->
                                content.[k] <- v
                                true
                    | Remove(k,_) -> content.Remove k
               )

    type DirtyMapReaderSet<'k, 'v when 'k : equality>() =
        let subscriptions = Dictionary<IMapReader<'k, 'v>, unit -> unit>()
        let dirty = HashSet<IMapReader<'k, 'v>>()

        member x.Listen(r : IMapReader<'k, 'v>) =
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
            
        member x.Destroy(r : IMapReader<'k, 'v>) =
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
 



    type MapReader<'k, 'v1, 'v2 when 'k : equality>(scope : Ag.Scope, source : IMapReader<'k, 'v1>, f : 'k -> 'v1 -> 'v2) as this =
        inherit AdaptiveObject()
        do source.AddOutput this  

        let f = Cache2(scope, f)
        let content = VersionedDictionary(Dictionary<'k, 'v2>())

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

        interface IMapReader<'k, 'v2> with
            member x.Content = content :> _

            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let input = source.GetDelta()
                    
                    let result = 
                        input |> List.map (fun d ->
                            match d with
                                | Set(k,v) -> Set(k, f.Invoke (k, v))
                                | Remove(k,v) -> Remove (k, f.Revoke (k, v))
                        ) |> apply takeNew content

                    result
                )     

    type ChooseReader<'k, 'v1, 'v2 when 'k : equality>(scope : Ag.Scope, source : IMapReader<'k, 'v1>, f : 'k -> 'v1 -> Option<'v2>) as this =
        inherit AdaptiveObject()
        do source.AddOutput this  

        let f = Cache2(scope, f)
        let content = VersionedDictionary(Dictionary<'k, 'v2>())

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

        interface IMapReader<'k, 'v2> with
            member x.Content = content :> _

            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let input = source.GetDelta()
                    
                    let result = 
                        input |> List.choose (fun d ->
                            match d with
                                | Set(k,v) -> 
                                    match f.Invoke (k, v) with
                                        | Some v -> Set(k, v) |> Some
                                        | None -> None

                                | Remove(k,v) -> 
                                    match f.Revoke (k, v) with
                                        | Some v -> Remove (k, v) |> Some
                                        | None -> None

                        ) |> apply takeNew content

                    result
                )     

    type UnionReader<'k, 'v when 'k : equality>(source : IReader<IMapReader<'k, 'v>>, conflict : 'v -> 'v -> 'v) as this =
        inherit AdaptiveObject()
        do source.AddOutput this  

        let content = VersionedDictionary<'k, 'v>()
        let dirtyInner = new DirtyMapReaderSet<'k, 'v>()

        member x.Dispose() =
            source.RemoveOutput this
            source.Dispose()
            content.Clear()

        override x.Finalize() =
            try x.Dispose()
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IMapReader<'k, 'v> with
            member x.Content = content :> _

            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let xs = source.GetDelta()
                    let outerDeltas =
                        xs |> List.collect (fun d ->
                            match d with
                                | Add r ->
                                    // we're an output of the new reader
                                    r.AddOutput this

                                    // bring the reader's content up-to-date by calling GetDelta
                                    r.GetDelta() |> ignore

                                    // listen to marking of r (reader cannot be OutOfDate due to GetDelta above)
                                    dirtyInner.Listen r
                                    
                                    // since the entire reader is new we add its content
                                    // which must be up-to-date here (due to calling GetDelta above)
                                    r.Content |> Seq.map (fun (KeyValue(k,v)) -> Set(k,v)) |> Seq.toList

                                | Rem r ->
                                    // remove the reader from the listen-set
                                    dirtyInner.Destroy r

                                    // since the reader is no longer contained we don't want
                                    // to be notified anymore
                                    r.RemoveOutput this
                                    
                                    // the entire content of the reader is removed
                                    // Note that the content here might be OutOfDate
                                    // TODO: think about implications here when we do not "own" the reader
                                    //       exclusively
                                    r.Content |> Seq.map (fun (KeyValue(k,v)) -> Remove(k,v)) |> Seq.toList

                        )


                    // all dirty inner readers must be registered 
                    // in dirtyInner. Even if the outer set did not change we
                    // need to collect those inner deltas.
                    let innerDeltas = dirtyInner.GetDeltas()

                    // concat inner and outer deltas 
                    let deltas = List.append outerDeltas innerDeltas
                    deltas |> apply conflict content
                )     

    type ModReader<'k, 'v when 'k : equality>(source : IMod<'k * 'v>) as this =  
        inherit AdaptiveObject()
        do source.AddOutput this
        let mutable cache : Option<'k * 'v> = None
        let content = VersionedDictionary()

        member x.Dispose() =
            source.RemoveOutput this
            cache <- None

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IMapReader<'k, 'v> with
            member x.Content = content :> _
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let (k,v) = source.GetValue()
                    let resultDeltas =
                        match cache with
                            | Some c ->
                                if not <| System.Object.Equals(v, c) then
                                    cache <- Some (k,v)
                                    [Set(k,v)]
                                else
                                    []
                            | None ->
                                cache <- Some (k,v)
                                [Set(k,v)]

                    resultDeltas |> apply takeNew content
                )

    type BindReader<'a, 'k, 'v when 'k : equality>(scope, source : IMod<'a>, f : 'a -> IMapReader<'k, 'v>) as this =
        inherit AdaptiveObject()
        do source.AddOutput this

        let mutable cache : Option<'a * IMapReader<'k, 'v>> = None
        let content = VersionedDictionary<'k, 'v>()

        member x.Dispose() =
            source.RemoveOutput this
            match cache with
                | Some(_,r) -> 
                    r.RemoveOutput this 
                    r.Dispose()
                    cache <- None
                | None ->
                    ()
            content.Clear()

        override x.Finalize() =
            try x.Dispose() 
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IMapReader<'k, 'v> with
            member x.Content = content :> _
            member x.GetDelta() =
                x.EvaluateIfNeeded [] (fun () ->
                    let xs = source.GetValue()

                    let deltas = 
                        match cache with
                            | Some(v,r) when System.Object.Equals(v, xs) ->
                                r.GetDelta()
                            | _ ->
     
                                let r = Ag.useScope scope (fun () -> f xs)
                                r.AddOutput this

                                r.GetDelta() |> ignore

                                let removals =
                                    match cache with
                                        | Some (_,old) -> 
                                            old.RemoveOutput this
                                            old.Content |> Seq.map (fun (KeyValue(k,v)) -> Remove(k,v)) |> Seq.toList
                                        | None -> 
                                            []

                                let additions = r.Content |> Seq.map (fun (KeyValue(k,v)) -> Set(k,v)) |> Seq.toList

                                cache <- Some(xs, r)
                                removals @ additions


                    deltas |> apply takeNew content

                )


    type BufferedReader<'k, 'v when 'k : equality>(update : unit -> unit, dispose : BufferedReader<'k, 'v> -> unit) =
        inherit AdaptiveObject()
        let deltas = List()
        let mutable reset : Option<IDictionary<'k, 'v>> = None

        static let mutable resetCount = 0
        static let mutable incrementalCount = 0


        let content = VersionedDictionary<'k, 'v>()

        let contains (kvp : KeyValuePair<'k, 'v>) =
            match content.TryGetValue kvp.Key with
                | (true, v) -> System.Object.Equals(kvp.Value, v)
                | _ -> false
        
        let getDeltas() =
            match reset with
                | Some c ->
                    reset <- None
                    let add = c |> Seq.filter (not << contains) |> Seq.map (fun (KeyValue(k,v)) -> Set(k,v))
                    let rem = content |> Seq.filter (not << c.Contains) |> Seq.map (fun (KeyValue(k,v)) -> Remove(k,v))

                    Seq.append add rem |> Seq.toList
                | None ->
                    let res = deltas |> Seq.toList
                    deltas.Clear()
                    res

        member x.Emit (c : IDictionary<'k, 'v>, d : Option<list<MapDelta<'k, 'v>>>) =
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


        new(dispose) = new BufferedReader<'k, 'v>(id, dispose)
        new() = new BufferedReader<'k, 'v>(id, ignore)

        member x.Dispose() =
            dispose(x)

        override x.Finalize() = 
            try x.Dispose()
            with _ -> ()

        interface IDisposable with
            member x.Dispose() = x.Dispose()

        interface IMapReader<'k, 'v> with
            member x.GetDelta() =
                update()
                x.EvaluateIfNeeded [] (fun () ->
                    let l = getDeltas()
                    l |> apply takeNew content
                )

            member x.Content = content :> _


    let map scope (f : 'k -> 'v1 -> 'v2) (input : IMapReader<'k, 'v1>) =
        new MapReader<_, _, _>(scope, input, f) :> IMapReader<_,_>

    let union (conflict : 'v -> 'v -> 'v) (input : IReader<IMapReader<'k, 'v>>)  =
        new UnionReader<_, _>(input, conflict) :> IMapReader<_,_>

    let bind scope (f : 'a -> IMapReader<'k, 'v>) (input : IMod<'a>) : IMapReader<'k, 'v> =
        new BindReader<_,_,_>(scope, input, f) :> IMapReader<_,_>

    let bind2 scope (f : 'a -> 'b -> IMapReader<'k, 'v>) (ma : IMod<'a>)  (mb : IMod<'b>)=
        let tup = Mod.map2 (fun a b -> (a,b)) ma mb
        bind scope (fun (a,b) -> f a b) tup


    let choose scope (f : 'k -> 'v1 -> Option<'v2>) (input : IMapReader<'k, 'v1>) =
        new ChooseReader<_, _,_>(scope, input, f) :> IMapReader<_, _>

    let ofMod (m : IMod<'k * 'v>) =
        new ModReader<_,_>(m) :> IMapReader<_,_>
