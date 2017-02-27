module HMap

open System
open NUnit
open FsCheck
open FsCheck.NUnit
open Aardvark.Base

module List =
    let all (l : list<bool>) =
        l |> List.fold (&&) true

[<Property(Verbose = true)>]
let ``[HMap] count`` (l : Map<int, int>) (a : int)  =
    not (Map.containsKey a l) ==> lazy (
        let map = l |> Map.toList |> HMap.ofList
        let mapWithA = HMap.add a a map

        List.all [
            HMap.count HMap.empty = 0
            HMap.count mapWithA = HMap.count map + 1
            HMap.count (HMap.remove a mapWithA) = HMap.count map
            HMap.count map = l.Count
            HMap.count (HMap.union map map) = HMap.count map
            HMap.count (HMap.map (fun _ v -> v) map) = HMap.count map
            HMap.count (HMap.filter (fun _ _ -> true) map) = HMap.count map
            HMap.count (HMap.filter (fun _ _ -> false) map) = 0
            HMap.count (HMap.choose (fun _ v -> Some v) map) = HMap.count map
            HMap.count (HMap.choose (fun _ _ -> None) map) = 0
            HMap.count (HMap.alter a (fun _ -> None) mapWithA) = HMap.count map
            HMap.count (HMap.alter a (fun _ -> Some 5) mapWithA) = HMap.count mapWithA
            HMap.count (HMap.update a (fun _ -> 5) mapWithA) = HMap.count mapWithA
        ]
    )

[<Property(Verbose = true)>]
let ``[HMap] tryFind`` (l : Map<int, int>) (a : int)  =
    not (Map.containsKey a l) ==> lazy (
        let map = l |> Map.toList |> HMap.ofList
        let mapWithA = HMap.add a a map
        
        List.all [
            HMap.tryFind a mapWithA = Some a
            HMap.tryFind a map = None
            HMap.tryFind a (HMap.add a 7 mapWithA) = Some 7
            HMap.tryFind a (HMap.add a 7 map) = Some 7
            HMap.tryFind a (HMap.remove a mapWithA) = None
            HMap.tryFind a (HMap.union map mapWithA) = Some a
            HMap.tryFind a (HMap.alter a (fun o -> Some 100) mapWithA) = Some 100
            HMap.tryFind a (HMap.alter a (fun o -> None) mapWithA) = None
            HMap.tryFind a (HMap.update a (fun o -> 123) mapWithA) = Some 123
            HMap.tryFind a (HMap.update a (fun o -> 123) map) = Some 123
            HMap.tryFind a (HMap.choose (fun _ v -> Some v) mapWithA) = Some a
            HMap.tryFind a (HMap.choose (fun _ v -> None) mapWithA) = None
            HMap.tryFind a (HMap.choose (fun _ v -> Some 7) mapWithA) = Some 7
            HMap.tryFind a (HMap.filter (fun _ v -> true) mapWithA) = Some a
            HMap.tryFind a (HMap.filter (fun _ v -> false) mapWithA) = None

        ]

    )

[<Property(Verbose = true)>]
let ``[HMap] containsKey`` (l : Map<int, int>) (a : int)  =
    let map = l |> Map.toList |> HMap.ofList
    HMap.containsKey a map = Option.isSome (HMap.tryFind a map)

[<Property(Verbose = true)>]
let ``[HMap] find`` (l : Map<int, int>) (a : int)  =
    let map = l |> Map.toList |> HMap.ofList
    let res = HMap.tryFind a map
    Option.isSome res ==> lazy (
        HMap.find a map = res.Value
    )

[<Property(Verbose = true)>]
let ``[HMap] ofList`` (l : list<int * int>) =
    List.sortBy fst (HMap.toList (HMap.ofList l)) = Map.toList (Map.ofList l)

[<Property(Verbose = true)>]
let ``[HMap] map2/choose2`` (lm : Map<int, int>) (rm : Map<int, int>) =
    let l = lm |> Map.toList |> HMap.ofList
    let r = rm |> Map.toList |> HMap.ofList

    let map2 (f : 'k -> Option<'a> -> Option<'b> -> 'c) (l : Map<'k, 'a>) (r : Map<'k, 'b>) =
        let mutable res = Map.empty

        for (lk,lv) in Map.toSeq l do
            match Map.tryFind lk r with
                | Some rv -> res <- Map.add lk (f lk (Some lv) (Some rv)) res
                | None -> res <- Map.add lk (f lk (Some lv) None) res

        for (rk,rv) in Map.toSeq r do
            match Map.tryFind rk l with
                | Some _ -> ()
                | None -> res <- Map.add rk (f rk None (Some rv)) res

        res

    let choose2 (f : 'k -> Option<'a> -> Option<'b> -> Option<'c>) (l : Map<'k, 'a>) (r : Map<'k, 'b>) =
        let mutable res = Map.empty

        for (lk,lv) in Map.toSeq l do
            match Map.tryFind lk r with
                | Some rv -> 
                    match f lk (Some lv) (Some rv) with
                        | Some r ->
                            res <- Map.add lk r res
                        | None ->
                            ()
                | None -> 
                    match f lk (Some lv) None with
                        | Some r -> res <- Map.add lk r res
                        | None -> ()

        for (rk,rv) in Map.toSeq r do
            match Map.tryFind rk l with
                | Some _ -> ()
                | None -> 
                    match f rk None (Some rv) with
                        | Some r -> res <- Map.add rk r res
                        | None -> ()

        res

    let equal (l : hmap<'k, 'v>) (r : Map<'k, 'v>) =
        let l = l |> HMap.toList |> List.sortBy fst
        let r = r |> Map.toList
        l = r

    let add (k : int) (l : Option<int>) (r : Option<int>) =
        match l, r with
            | Some l, Some r -> l + r
            | None, Some r -> r
            | Some l, None -> l
            | None, None -> failwith "that's bad (Map invented a key)"

    let add2 (k : int) (l : Option<int>) (r : Option<int>) =
        match l, r with
            | Some l, Some r -> if l > r then Some r else None
            | None, Some r -> Some r
            | Some l, None -> Some l
            | None, None -> failwith "that's bad (Map invented a key)"

    List.all [
        equal (HMap.map2 add l r) (map2 add lm rm)
        equal (HMap.choose2 (fun k l r -> add k l r |> Some) l r) (map2 add lm rm)
        equal (HMap.choose2 (fun k l r -> add2 k l r) l r) (choose2 add2 lm rm)
    ]



