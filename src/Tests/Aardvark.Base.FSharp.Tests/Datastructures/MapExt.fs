﻿namespace Aardvark.Base.FSharp.Tests

open FsCheck
open FsCheck.NUnit
open Aardvark.Base
open System.Collections.Generic

module MapExt =

    module List =
        let all (l : list<bool>) =
            l |> List.fold (&&) true

    [<Property>]
    let ``[MapExt] alter`` (m : Map<int, int>) (v : int) (f : Option<int> -> Option<int>) =
        not (Map.containsKey v m) ==> lazy (
            let me = MapExt.ofSeq (Map.toSeq m)

            let alter (key : int) (f : Option<int> -> Option<int>) (m : Map<int, int>) =
                match f (Map.tryFind key m) with
                    | Some v -> Map.add key v m
                    | None -> Map.remove key m

            List.all [
                MapExt.toList (MapExt.alter v f me) = Map.toList (alter v f m)
                MapExt.toList (MapExt.alter v f (MapExt.add v v me)) = Map.toList (alter v f (Map.add v v m))
            ]
        )

    [<Property>]
    let ``[MapExt] choose`` (m : Map<int, int>) (f : int -> int -> Option<int>) =
        let me = MapExt.ofSeq (Map.toSeq m)

        let choose (f : int -> int -> Option<int>) (m : Map<int, int>) =
            let mutable res = Map.empty
            for (k,v) in Map.toSeq m do
                match f k v with
                    | Some v -> res <- Map.add k v res
                    | _ -> ()

            res

        List.all [
            MapExt.toList (MapExt.choose f me) = Map.toList (choose f m)
        ]

    [<Property>]
    let ``[MapExt] range`` (m : Map<int, int>) (min : int) (max : int) =
        (min <= max) ==> lazy (
            let me = MapExt.ofSeq (Map.toSeq m)
            let expected = me |> MapExt.filter (fun k _ -> k >= min && k <= max)
            let actual = me |> MapExt.range min max
            expected = actual
        )

    [<Property>]
    let ``[MapExt] IDictionary item`` (m: Map<int, int>) =
        let me = MapExt.ofSeq (Map.toSeq m)
        let keys = m.Keys |> List.ofSeq
        
        keys |> List.map (fun key ->
            let expected = (m :> IDictionary<_, _>).[key]
            let actual = (me :> IDictionary<_, _>).[key]
            expected = actual  
        )
        |> List.all

    [<Property>]
    let ``[MapExt] IDictionary keys`` (m: Map<int, int>) =
        let me = MapExt.ofSeq (Map.toSeq m)
        let kc = (me :> IDictionary<_, _>).Keys
        let keys = m.Keys |> Array.ofSeq |> Array.sort

        let contains =
            keys |> Array.map kc.Contains

        let enumerated =
            kc |> Array.ofSeq |> Array.sort

        let copied =
            let arr = Array.zeroCreate<int> m.Count
            kc.CopyTo(arr, 0)
            Array.sort arr

        List.all [
            kc.Count = m.Count
            copied = keys
            enumerated = keys
            yield! contains
        ]

    [<Property>]
    let ``[MapExt] IDictionary values`` (m: Map<int, int>) =
        let me = MapExt.ofSeq (Map.toSeq m)
        let vc = (me :> IDictionary<_, _>).Values
        let values = m.Values |> Array.ofSeq |> Array.sort

        let contains =
            values |> Array.map vc.Contains

        let enumerated =
            vc |> Array.ofSeq |> Array.sort

        let copied =
            let arr = Array.zeroCreate<int> m.Count
            vc.CopyTo(arr, 0)
            Array.sort arr

        List.all [
            vc.Count = m.Count
            copied = values
            enumerated = values
            yield! contains
        ]

    [<Property>]
    let ``[MapExt] IDictionary TryGetValue`` (m: Map<int, int>) =
        let me = MapExt.ofSeq (Map.toSeq m)
        let keys = m.Keys |> List.ofSeq

        keys |> List.map (fun key ->
            let exp, expv = (m :> IDictionary<_, _>).TryGetValue key
            let act, actv = (me :> IDictionary<_, _>).TryGetValue key
            act = exp && actv = expv
        )
        |> List.all

    [<Property>]
    let ``[MapExt] IReadOnlyDictionary TryGetValue`` (m: Map<int, int>) =
        let me = MapExt.ofSeq (Map.toSeq m)
        let keys = m.Keys |> List.ofSeq

        keys |> List.map (fun key ->
            let exp, expv = (m :> IReadOnlyDictionary<_, _>).TryGetValue key
            let act, actv = (me :> IReadOnlyDictionary<_, _>).TryGetValue key
            act = exp && actv = expv
        )
        |> List.all