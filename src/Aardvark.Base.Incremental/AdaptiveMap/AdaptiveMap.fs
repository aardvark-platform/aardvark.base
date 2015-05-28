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

