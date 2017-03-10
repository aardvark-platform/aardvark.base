namespace Aardvark.Base.Incremental

open System
open System.Collections
open System.Collections.Generic
open System.Runtime.CompilerServices
open Aardvark.Base.Incremental
open Aardvark.Base

[<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
[<RequireQualifiedAccess>]
module AMap =


    [<AutoOpen>]
    module Implementation = 
        type EmptyReader<'a, 'b> private() =
            inherit ConstantObject()

            static let instance = new EmptyReader<'a, 'b>() :> IOpReader<_,_>
            static member Instance = instance

            interface IOpReader<hdeltamap<'a, 'b>> with
                member x.Dispose() =
                    ()

                member x.GetOperations caller =
                    HDeltaMap.empty

            interface IOpReader<hmap<'a, 'b>, hdeltamap<'a, 'b>> with
                member x.State = HMap.empty

        type EmptyMap<'a, 'b> private() =
            let content = Mod.constant HMap.empty

            static let instance = EmptyMap<'a, 'b>() :> amap<_,_>

            static member Instance = instance

            interface amap<'a, 'b> with
                member x.IsConstant = true
                member x.Content = content
                member x.GetReader() = EmptyReader.Instance

        type ConstantMap<'a, 'b>(content : Lazy<hmap<'a, 'b>>) =
            let deltas = lazy ( HMap.computeDelta HMap.empty content.Value )
            let mcontent = ConstantMod<hmap<'a, 'b>>(content) :> IMod<_>

            interface amap<'a, 'b> with
                member x.IsConstant = true
                member x.GetReader() = new History.Readers.ConstantReader<_,_>(HMap.trace, deltas, content) :> IMapReader<_,_>
                member x.Content = mcontent
        
            new(content : hmap<'a, 'b>) = ConstantMap<'a, 'b>(Lazy.CreateFromValue content)

        type AdaptiveMap<'a, 'b>(newReader : unit -> IOpReader<hdeltamap<'a, 'b>>) =
            let h = History.ofReader HMap.trace newReader
            interface amap<'a, 'b> with
                member x.IsConstant = false
                member x.Content = h :> IMod<_>
                member x.GetReader() = h.NewReader()
                
        let inline amap (f : Ag.Scope -> #IOpReader<hdeltamap<'a, 'b>>) =
            let scope = Ag.getContext()
            AdaptiveMap<'a, 'b>(fun () -> f scope :> IOpReader<_>) :> amap<_,_>
            
        let inline constant (l : Lazy<hmap<'a, 'b>>) =
            ConstantMap<'a, 'b>(l) :> amap<_,_>

    module Readers =
        
        type MapReader<'k, 'a, 'b>(scope : Ag.Scope, input : amap<'k, 'a>, f : 'k -> 'a -> 'b) =
            inherit AbstractReader<hdeltamap<'k, 'b>>(scope, HDeltaMap.monoid)

            let reader = input.GetReader()

            override x.Release() =
                reader.Dispose()

            override x.Compute(token) =
                let ops = reader.GetOperations token
                ops |> HMap.map (fun k op ->
                    match op with
                        | Set v -> Set (f k v)
                        | Remove -> Remove
                )

        type ChooseReader<'k, 'a, 'b>(scope : Ag.Scope, input : amap<'k, 'a>, f : 'k -> 'a -> Option<'b>) =
            inherit AbstractReader<hdeltamap<'k, 'b>>(scope, HDeltaMap.monoid)

            let reader = input.GetReader()
            let cache = Dict<'k, bool>()

            override x.Release() =
                reader.Dispose()

            override x.Compute(token) =
                let ops = reader.GetOperations token
                ops |> HMap.choose (fun k op ->
                    match op with
                        | Set v -> 
                            match f k v with
                                | Some n ->
                                    cache.[k] <- true
                                    Some (Set n)
                                | None ->
                                    cache.[k] <- false
                                    None
                        | Remove -> 
                            match cache.TryRemove k with
                                | (true, wasExisting) ->
                                    if wasExisting then Some Remove
                                    else None
                                | _ ->
                                    failwithf "[AMap] could not remove entry %A" k
                )


    let empty<'k, 'v> = EmptyMap<'k, 'v>.Instance

    let ofHMap (map : hmap<'k, 'v>) = ConstantMap(map) :> amap<_,_>
    let ofSeq (seq : seq<'k * 'v>) = ConstantMap(HMap.ofSeq seq) :> amap<_,_>
    let ofList (list : list<'k * 'v>) = ConstantMap(HMap.ofList list) :> amap<_,_>
    let ofArray (arr : array<'k * 'v>) = ConstantMap(HMap.ofArray arr) :> amap<_,_>


    let map (mapping : 'k -> 'a -> 'b) (map : amap<'k, 'a>) =
        if map.IsConstant then
            constant <| lazy ( map.Content |> Mod.force |> HMap.map mapping )
        else
            amap <| fun scope -> new Readers.MapReader<'k, 'a, 'b>(scope, map, mapping)

    let choose (mapping : 'k -> 'a -> Option<'b>) (map : amap<'k, 'a>) =
        if map.IsConstant then
            constant <| lazy ( map.Content |> Mod.force |> HMap.choose mapping )
        else
            amap <| fun scope -> new Readers.ChooseReader<'k, 'a, 'b>(scope, map, mapping)
        
    let filter (predicate : 'k -> 'a -> bool) (map : amap<'k, 'a>) =
        choose (fun k v -> if predicate k v then Some v else None) map

    let toMod (map : amap<'k, 'v>) = map.Content