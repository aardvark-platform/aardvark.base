namespace Aardvark.Base.FSharp.Tests

open FsCheck
open FsCheck.NUnit
open Aardvark.Base

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