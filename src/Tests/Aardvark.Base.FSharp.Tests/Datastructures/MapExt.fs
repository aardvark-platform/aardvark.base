namespace Aardvark.Base.FSharp.Tests

open FsCheck
open FsCheck.NUnit
open FsUnit
open Aardvark.Base
open System.Collections.Generic

module MapExt =

    module List =
        let all (l : list<bool>) =
            l |> List.fold (&&) true

    module MapExt =
        let ofMap (m : Map<'K, 'V>) =
            m |> Map.toSeq |> MapExt.ofSeq

        let isValid (m : MapExt<'K, 'V>) =
            let l = m |> MapExt.toList |> List.map fst
            l = List.sort l

        let equalToMap (m : Map<'K, 'V>) (me : MapExt<'K, 'V>) =
            MapExt.toList me = Map.toList m

    [<Property>]
    let ``[MapExt] alter`` (m : Map<int, int>) (v : int) (f : Option<int> -> Option<int>) =
        not (Map.containsKey v m) ==> lazy (
            let me = MapExt.ofMap m

            let alter (key : int) (f : Option<int> -> Option<int>) (m : Map<int, int>) =
                match f (Map.tryFind key m) with
                    | Some v -> Map.add key v m
                    | None -> Map.remove key m

            List.all [
                MapExt.equalToMap (alter v f m) (MapExt.alter v f me)
                MapExt.equalToMap (alter v f (Map.add v v m)) (MapExt.alter v f (MapExt.add v v me))
            ]
        )

    [<Property>]
    let ``[MapExt] choose`` (m : Map<int, int>) (f : int -> int -> Option<int>) =
        let me = MapExt.ofMap m
        let res = MapExt.choose f me

        let choose (f : int -> int -> Option<int>) (m : Map<int, int>) =
            let mutable res = Map.empty
            for (k,v) in Map.toSeq m do
                match f k v with
                    | Some v -> res <- Map.add k v res
                    | _ -> ()

            res

        List.all [
            MapExt.isValid res
            MapExt.equalToMap (choose f m) res
        ]

    type int2 = (struct (int * int))

    [<Property(EndSize = 100000)>]
    let ``[MapExt] choose2`` (k1: Set<int>) (k2: Set<int>) (f : int -> int2 option -> int2 option -> int2 option) =
        let m1 = k1 |> Seq.map (fun k -> k, struct (1, k)) |> Map.ofSeq
        let m2 = k2 |> Seq.map (fun k -> k, struct (2, k)) |> Map.ofSeq

        let me1 = MapExt.ofMap m1
        let me2 = MapExt.ofMap m2

        let res =
            (me1, me2) ||> MapExt.choose2 (fun k v1 v2 ->
                v1 |> Option.iter (fstv >> should equal 1)
                v2 |> Option.iter (fstv >> should equal 2)
                f k v1 v2
            )

        let choose2 f (m1 : Map<_, _>) (m2 : Map<_, _>) =
            let mutable res = Map.empty
            for k in Map.keys (Map.union m1 m2) do
                let v1 = Map.tryFind k m1
                let v2 = Map.tryFind k m2
                match f k v1 v2 with
                | Some v -> res <- Map.add k v res
                | _ -> ()
            res

        let valid = MapExt.isValid res

        List.all [
            valid
            MapExt.equalToMap (choose2 f m1 m2) res
        ]

    [<Property(EndSize = 100000)>]
    let ``[MapExt] map2`` (k1: Set<int>) (k2: Set<int>) (f : int -> int2 option -> int2 option -> int2) =
        let m1 = k1 |> Seq.map (fun k -> k, struct (1, k)) |> Map.ofSeq
        let m2 = k2 |> Seq.map (fun k -> k, struct (2, k)) |> Map.ofSeq

        let me1 = MapExt.ofMap m1
        let me2 = MapExt.ofMap m2

        let res =
            (me1, me2) ||> MapExt.map2 (fun k v1 v2 ->
                v1 |> Option.iter (fstv >> should equal 1)
                v2 |> Option.iter (fstv >> should equal 2)
                f k v1 v2
            )

        let map2 f (m1 : Map<_, _>) (m2 : Map<_, _>) =
            let mutable res = Map.empty
            for k in Map.keys (Map.union m1 m2) do
                let v1 = Map.tryFind k m1
                let v2 = Map.tryFind k m2
                let v = f k v1 v2
                res <- Map.add k v res
            res

        let valid = MapExt.isValid res

        List.all [
            valid
            MapExt.equalToMap (map2 f m1 m2) res
        ]

    [<Property>]
    let ``[MapExt] range`` (m : Map<int, int>) (min : int) (max : int) =
        (min <= max) ==> lazy (
            let me = MapExt.ofMap m
            let expected = me |> MapExt.filter (fun k _ -> k >= min && k <= max)
            let actual = me |> MapExt.range min max
            expected = actual
        )

    [<Property>]
    let ``[MapExt] IDictionary item`` (m: Map<int, int>) =
        let me = MapExt.ofMap m
        let keys = m.Keys |> List.ofSeq
        
        keys |> List.map (fun key ->
            let expected = (m :> IDictionary<_, _>).[key]
            let actual = (me :> IDictionary<_, _>).[key]
            expected = actual  
        )
        |> List.all

    [<Property>]
    let ``[MapExt] IDictionary keys`` (m: Map<int, int>) =
        let me = MapExt.ofMap m
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
        let me = MapExt.ofMap m
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
        let me = MapExt.ofMap m
        let keys = m.Keys |> List.ofSeq

        keys |> List.map (fun key ->
            let exp, expv = (m :> IDictionary<_, _>).TryGetValue key
            let act, actv = (me :> IDictionary<_, _>).TryGetValue key
            act = exp && actv = expv
        )
        |> List.all

    [<Property>]
    let ``[MapExt] IReadOnlyDictionary TryGetValue`` (m: Map<int, int>) =
        let me = MapExt.ofMap m
        let keys = m.Keys |> List.ofSeq

        keys |> List.map (fun key ->
            let exp, expv = (m :> IReadOnlyDictionary<_, _>).TryGetValue key
            let act, actv = (me :> IReadOnlyDictionary<_, _>).TryGetValue key
            act = exp && actv = expv
        )
        |> List.all