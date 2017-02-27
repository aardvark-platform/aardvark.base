module HSet

open System
open NUnit
open FsCheck
open FsCheck.NUnit
open Aardvark.Base

module List =
    let all (l : list<bool>) =
        l |> List.fold (&&) true


[<Property(Verbose = true)>]
let ``[HSet] count`` (l : Set<int>) (a : int)  =
    not (Set.contains a l) ==> lazy (
        let set = l |> Set.toList |> HSet.ofList
        let setWithA = HSet.add a set

        List.all [
            HSet.count HSet.empty = 0
            HSet.count setWithA = HSet.count set + 1
            HSet.count (HSet.remove a setWithA) = HSet.count set
            HSet.count set = l.Count
            HSet.count (HSet.union set set) = HSet.count set
            HSet.count (HSet.union set setWithA) = HSet.count setWithA
            HSet.count (HSet.difference setWithA set) = 1
            HSet.count (HSet.intersect setWithA set) = HSet.count set
            HSet.count (HSet.map (fun v -> v) set) = HSet.count set
            HSet.count (HSet.filter (fun _ -> true) set) = HSet.count set
            HSet.count (HSet.filter (fun _ -> false) set) = 0
            HSet.count (HSet.choose (fun v -> Some v) set) = HSet.count set
            HSet.count (HSet.choose (fun _ -> None) set) = 0
            HSet.count (HSet.choose (fun _ -> Some 1) setWithA) = 1
            HSet.count (HSet.alter a (fun _ -> false) setWithA) = HSet.count set
            HSet.count (HSet.alter a (fun _ -> true) setWithA) = HSet.count setWithA
        ]
    )

[<Property(Verbose = true)>]
let ``[HSet] contains`` (l : Set<int>) (a : int)  =
    not (Set.contains a l) ==> lazy (
        let set = l |> Set.toList |> HSet.ofList
        let setWithA = HSet.add a set
        
        List.all [
            HSet.contains a setWithA = true
            HSet.contains a set = false
            HSet.contains a (HSet.add a setWithA) = true
            HSet.contains a (HSet.add a set) = true
            HSet.contains a (HSet.remove a setWithA) = false
            HSet.contains a (HSet.union set setWithA) = true
            HSet.contains a (HSet.difference setWithA set) = true
            HSet.contains a (HSet.intersect setWithA set) = false
            HSet.contains a (HSet.alter a (fun o -> true) setWithA) = true
            HSet.contains a (HSet.alter a (fun o -> false) setWithA) = false
            HSet.contains a (HSet.choose (fun v -> Some v) setWithA) = true
            HSet.contains a (HSet.choose (fun v -> None) setWithA) = false
            HSet.contains 7 (HSet.choose (fun v -> Some 7) setWithA) = true
            HSet.contains a (HSet.filter (fun v -> true) setWithA) = true
            HSet.contains a (HSet.filter (fun v -> false) setWithA) = false

        ]

    )

[<Property(Verbose = true)>]
let ``[HSet] ofList`` (l : list<int>) =
    HSet.toList (HSet.ofList l) |> List.sort = Set.toList (Set.ofList l)
