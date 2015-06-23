namespace Aardvark.Base.Incremental

open System
open System.Runtime.CompilerServices
open System.Collections.Concurrent
open Aardvark.Base
open Aardvark.Base.Incremental.AMapReaders
open System.Collections.Generic

module AMap =

    type AdaptiveMap<'k, 'v when 'k : equality>(newReader : unit -> IMapReader<'k, 'v>) =
        let state = Dictionary<'k, 'v>()
        let readers = WeakSet<BufferedReader<'k, 'v>>()

        let mutable inputReader = None
        let getReader() =
            match inputReader with
                | Some r -> r
                | None ->
                    let r = newReader()
                    inputReader <- Some r
                    r

        let bringUpToDate () =
            let r = getReader()
            let delta = r.GetDelta ()
            if not <| List.isEmpty delta then
                delta |> apply takeNew state |> ignore
                readers  |> Seq.iter (fun ri ->
                    ri.Emit(state, Some delta)
                )

        interface amap<'k, 'v> with
            member x.GetReader () =
                bringUpToDate()
                let r = getReader()

                let remove ri =
                    r.RemoveOutput ri
                    readers.Remove ri |> ignore

                    if readers.IsEmpty then
                        r.Dispose()
                        inputReader <- None

                let reader = new BufferedReader<'k, 'v>(bringUpToDate, remove)
                reader.Emit (state, None)
                r.AddOutput reader
                readers.Add reader |> ignore

                reader :> _

    type ConstantMap<'k, 'v when 'k : equality>(content : seq<'k * 'v>) =
        let content = Dictionary.ofSeq content

        interface amap<'k, 'v> with
            member x.GetReader () =
                let r = new BufferedReader<'k, 'v>()
                r.Emit(content, None)
                r :> IMapReader<_,_>

    type private EmptyMapImpl<'k, 'v when 'k : equality> private() =
        static let emptySet = ConstantMap [] :> amap<'k, 'v>
        static member Instance = emptySet

    let empty<'k, 'v when 'k : equality> : amap<'k, 'v> =
        EmptyMapImpl<'k, 'v>.Instance

    let single (key : 'k) (value : 'v) =
        ConstantMap [key, value] :> amap<_,_>

    let ofSeq (s : seq<'k * 'v>) =
        ConstantMap(s) :> amap<'k, 'v>
    
    let ofList (l : list<'k * 'v>) =
        ConstantMap(l) :> amap<'k, 'v>

    let ofArray (a : array<'k * 'v>) =
        ConstantMap(a) :> amap<'k, 'v>

    let toList (set : amap<'k, 'v>) =
        use r = set.GetReader()
        r.GetDelta() |> ignore
        r.Content |> Seq.map (fun (KeyValue(k,v)) -> k,v) |> Seq.toList

    let toSeq (set : amap<'k, 'v>) =
        let l = toList set
        l :> seq<_>

    let toArray (set : amap<'k, 'v>) =
        use r = set.GetReader()
        r.GetDelta() |> ignore
        r.Content |> Seq.map (fun (KeyValue(k,v)) -> k,v) |> Seq.toArray

    let toMod (s : amap<'k, 'v>) =
        let r = s.GetReader()
        let c = r.Content :> IVersionedDictionary<_,_>

        let m = Mod.custom (fun () ->
            r.GetDelta() |> ignore
            c
        )
        r.AddOutput m
        m

    let containsKey (key : 'k) (set : amap<'k, 'v>) =
        set |> toMod |> Mod.map (fun s -> s.ContainsKey key)

    let tryFind (key : 'k) (set : amap<'k, 'v>) : IMod<Option<'v>> =
        set |> toMod |> Mod.map (fun s -> match s.TryGetValue key with | (true, v) -> Some v | _ -> None)

    let map (f : 'k -> 'v -> 'v1) (m : amap<'k, 'v>) = 
        let scope = Ag.getContext()
        AdaptiveMap(fun () -> m.GetReader() |> map scope f) :> amap<'k, 'v1>

    let mapValue (f : 'v -> 'v1) (m : amap<'k, 'v>) = 
        map (fun _ v -> f v) m

    let bind (f : 'a -> amap<'k, 'v>) (m : IMod<'a>) =
        if m.IsConstant then m |> Mod.force |> f
        else
            let scope = Ag.getContext()
            AdaptiveMap(fun () -> m |> bind scope (fun v -> (f v).GetReader())) :> amap<'k, 'v>

    let bind2 (f : 'a -> 'b -> amap<'k, 'v>) (ma : IMod<'a>) (mb : IMod<'b>) =
        match ma.IsConstant, mb.IsConstant with
            | true, true -> f (Mod.force ma) (Mod.force mb)
            | true, false -> let va = Mod.force ma in bind (fun b -> f va b) mb
            | false, true -> let vb = Mod.force mb in bind (fun a -> f a vb) ma
            | false, false->
                let scope = Ag.getContext()
                AdaptiveMap(fun () -> bind2 scope (fun a b -> (f a b).GetReader()) ma mb) :> amap<'k, 'v>

    let union (set : aset<amap<'k, 'v>>) = 
        let scope = Ag.getContext()
        AdaptiveMap(fun () -> set.GetReader() |> ASetReaders.map scope (fun r -> r.GetReader()) |> union takeNew) :> amap<'k,'v>

    let union' (set : seq<amap<'k, 'v>>) =
        set |> ASet.ofSeq |> union

    let choose (f : 'k -> 'v -> Option<'v1>) (m : amap<'k, 'v>) = 
        let scope = Ag.getContext()
        AdaptiveMap(fun () -> m.GetReader() |> choose scope f)

    let filter (f : 'k -> 'v -> bool) (m : amap<'k, 'v>) = 
        let scope = Ag.getContext()
        AdaptiveMap(fun () -> m.GetReader() |> AMapReaders.choose scope (fun k v -> if f k v then Some v else None))


module NewImpl = 
    type IMapReader<'k, 'v> =
        inherit IReader<'k * 'v>
        abstract member Content : IVersionedDictionary<'k, IVersionedSet<'v>>

    [<CompiledName("IAdaptiveMap")>]
    type amap<'k, 'v when 'k : equality> =
        abstract member ASet : aset<'k * 'v>
        abstract member GetReader : unit -> IMapReader<'k, 'v>

    module AMap =
    
        type private SetMapReader<'k, 'v when 'k : equality>(r : IReader<'k * 'v>) =
            let content = VersionedDictionary()

            let add (key : 'k) (v : 'v) =
                let set = 
                    match content.TryGetValue key with
                        | (true, set) -> set
                        | _ ->
                            let set = ReferenceCountingSet<'v>() :> IVersionedSet<_>
                            content.[key] <- set
                            set
                set.Add v
           
            let remove (key : 'k) (v : 'v) =
                match content.TryGetValue key with
                    | (true, set) ->
                        if set.Remove v then
                            if set.Count = 0 then content.Remove key |> ignore
                            true
                        else
                            false
                    | _ ->
                        false         

            let apply (deltas : list<Delta<'k * 'v>>) =
                [
                    for d in deltas do
                        match d with
                            | Add (k,v) ->
                                if add k v then yield d
                            | Rem (k,v) ->
                                if remove k v then yield d
                ]

            interface IAdaptiveObject with
                member x.Id = r.Id
                member x.Level
                    with get() = r.Level
                    and set v = r.Level <- v
                member x.Mark() = r.Mark()
                member x.Inputs = r.Inputs
                member x.Outputs = r.Outputs
                member x.MarkingCallbacks = r.MarkingCallbacks
                member x.OutOfDate
                    with get() = r.OutOfDate
                    and set o = r.OutOfDate <- o

            interface IReader<'k * 'v> with
                member x.Content = r.Content
                member x.GetDelta() = r.GetDelta() |> apply
                member x.Dispose() = r.Dispose()
                member x.Update() = r.Update()

            interface IMapReader<'k, 'v> with
                member x.Content = content :> IVersionedDictionary<_,_>

        type private SetMap<'k, 'v when 'k : equality>(aset : aset<'k * 'v>) =
            interface amap<'k, 'v> with
                member x.ASet = aset
                member x.GetReader() = new SetMapReader<'k, 'v>(aset.GetReader()) :> IMapReader<_,_>

        type private LookupReader<'k, 'v  when 'k : equality>(input : IMapReader<'k, 'v>, key : 'k) as this =
            inherit AdaptiveObject()
            static let emptySet = ReferenceCountingSet<'v>()
            do input.AddOutput this

            let mutable initial = true

            interface IReader<'v> with
                member x.Content =
                    match input.Content.TryGetValue key with
                        | (true, set) -> set |> unbox<ReferenceCountingSet<_>>
                        | _ -> emptySet

                member x.GetDelta() = 
                    x.EvaluateIfNeeded [] (fun () ->
                        if initial then
                            initial <- false
                            input.GetDelta() |> ignore
                            match input.Content.TryGetValue key with
                                | (true, set) -> set |> Seq.map Add |> Seq.toList
                                | _ -> []
                        else
                            input.GetDelta() 
                                |> List.choose (fun d -> 
                                    match d with
                                        | Add (k,v) when k = key -> Some (Add v)
                                        | Rem (k,v) when k = key -> Some (Rem v)
                                        | _ -> None
                                   )
                    )

                member x.Update() =
                    x.EvaluateIfNeeded () (fun () ->
                        input.Update()
                    )   

                member x.Dispose() =
                    input.RemoveOutput this

        let private wrap (f : 'k -> 'v -> 'a) =
            fun (k,v) -> (k, f k v)

        let private setmap (set : aset<'k * 'v>) =
            SetMap(set) :> amap<_,_>


        let empty<'k, 'v when 'k : equality> : amap<'k, 'v> = ASet.empty |> setmap

        let single (k : 'k) (v : 'v) = (k,v) |> ASet.single |> setmap

        let ofASet (s : aset<'k * 'v>) = s |> setmap

        let toASet (m : amap<'k, 'v>) = m.ASet

        let ofSeq (s : seq<'k * 'v>) = s |> ASet.ofSeq |> setmap
        
        let ofList (s : list<'k * 'v>) = s |> ASet.ofList |> setmap
    
        let ofArray (s : array<'k * 'v>) = s |> ASet.ofArray |> setmap
    
        let toMod (m : amap<'k, 'v>) =
            let r = m.GetReader()
            let res = Mod.custom(fun () ->
                r.GetDelta() |> ignore
                r.Content
            )
            r.AddOutput res
            res

        let map (f : 'k -> 'v -> 'a) (m : amap<'k, 'v>) =
            m.ASet |> ASet.map (wrap f) |> setmap

        let collect (f : 'k -> 'v -> amap<'k1, 'v1>) (m : amap<'k, 'v>) =
            m.ASet |> ASet.collect (fun (k,v) -> (f k v).ASet) |> setmap

        let choose (f : 'k -> 'v -> Option<'a>) (m : amap<'k, 'v>) =
            m.ASet |> ASet.choose (fun (k,v) -> match f k v with | Some a -> Some (k,a) | None -> None) |> setmap

        let filter (f : 'k -> 'v -> bool) (m : amap<'k, 'v>) =
            m.ASet |> ASet.filter (uncurry f) |> setmap

        let union (maps : aset<amap<'k, 'v>>) =
            maps |> ASet.collect toASet |> setmap

        let tryFindAll (key : 'k) (m : amap<'k, 'v>) =
            ASet.AdaptiveSet(fun () -> new LookupReader<'k, 'v>(m.GetReader(), key) :> IReader<'v>) :> aset<_>

        let tryFind (key : 'k) (m : amap<'k, 'v>) =
            tryFindAll key m 
                |> ASet.toMod 
                |> Mod.map (fun set -> 
                    if set.Count = 0 then 
                        None 
                    else 
                        Some (set |> Seq.head)
                  )
