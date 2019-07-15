﻿module PList

open System
open NUnit
open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open Aardvark.Base


module List =
    let all (l : list<bool>) =
        l |> List.fold (&&) true


[<Property>]
let ``[PList] equality`` (l : list<int>)  =
    let list1 = l |> PList.ofList
    let list2 = l |> PList.ofList
    let list3 = PList.append 4 list1

    List.all [
        list1 = list1
        list1 = list2
        list2 = list2
        PList.computeDelta list1 list2 = PDeltaList.empty
        PList.computeDelta list2 list1 = PDeltaList.empty
        PList.computeDelta PList.empty list1 = (list1.Content |> MapExt.map (fun _ v -> Set v) |> PDeltaList.ofMap)
        PList.computeDelta list1 PList.empty = (list1.Content |> MapExt.map (fun _ v -> Remove) |> PDeltaList.ofMap)
        list3 <> list1
        (match PDeltaList.toList (PList.computeDelta list1 list3) with | [_,Set 4] -> true | _ -> false)
    ]


[<Property>]
let ``[PList] count`` (l : list<int>) (a : int)  =
    let list = l |> PList.ofList

    List.all [
        PList.count PList.empty = 0
        PList.count (PList.append a list) = PList.count list + 1
        PList.count (PList.prepend a list) = PList.count list + 1
        PList.count (PList.removeAt 0 (PList.append a list)) = PList.count list
        PList.count list = l.Length
        PList.count (PList.choose (fun v -> Some v) list) = PList.count list
        PList.count (PList.filter (fun _ -> false) list) = 0
        PList.count (PList.map (fun v -> 2 * v) list) = PList.count list
        PList.count (PList.single a) = 1
    ]
    
[<Property>]
let ``[PList] choose`` (l : list<int>) (f : int -> Option<int>) =
    let tl = l |> List.choose f
    let tp = l |> PList.ofList |> PList.choose f |> PList.toList
    tl = tp   

[<Property>]
let ``[PList] map`` (l : list<int>) (f : int -> int) =
    let tl = l |> List.map f
    let tp = l |> PList.ofList |> PList.map f |> PList.toList
    tl = tp

[<Property>]
let ``[PList] filter`` (l : list<int>) (f : int -> bool) =
    let tl = l |> List.filter f
    let tp = l |> PList.ofList |> PList.filter f |> PList.toList
    tl = tp

[<Property>]
let ``[PList] append`` (l : list<int>) (v : int) =
    let tl = l @ [v]
    let tp = PList.append v (PList.ofList l) |> PList.toList
    tl = tp

[<Property>]
let ``[PList] prepend`` (l : list<int>) (v : int) =
    let tl = v :: l
    let tp = PList.prepend v (PList.ofList l) |> PList.toList
    tl = tp

[<Property>]
let ``[PList] setAt`` (l : list<int>) =
    l.Length >= 3 ==> lazy (
        let tl = l |> List.mapi (fun i v -> if i = 2 then 27 else v)
        let tp = PList.setAt 2 27 (PList.ofList l) |> PList.toList
        tl = tp
    )
    
[<Property>]
let ``[PList] removeAt`` (i : int) (l : list<int>) =
    (i >= 0 && i < l.Length) ==> lazy (
        let tl = l |> List.mapi (fun ii v -> if ii = i then None else Some v) |> List.choose id
        let tp = PList.removeAt i (PList.ofList l) |> PList.toList
        tl = tp
    )  

[<Property>]
let ``[PList] insertAt`` (i : int) (l : list<int>) =
    (i >= 0 && i <= l.Length) ==> lazy (
        let tl = 
            if i = 0 then 27 :: l
            elif i = l.Length then l @ [27]
            else l |> List.mapi (fun ii v -> if ii = i then [27;v] else [v]) |> List.collect id

        let tp = 
            PList.insertAt i 27 (PList.ofList l) |> PList.toList

        tl = tp
    )

[<Property>]
let ``[PList] computeDelta`` (l : list<int>) (r : list<int>) =
    let l = PList.ofList l
    let r = PList.ofList r

    let lr = PList.computeDelta l r
    let rl = PList.computeDelta r l

    List.all [
        PList.computeDelta l l |> PDeltaList.isEmpty
        PList.applyDelta l lr |> fst |> PList.toList = PList.toList r
        PList.applyDelta r rl |> fst |> PList.toList = PList.toList l
        PList.applyDelta l lr |> snd |> PDeltaList.toList = PDeltaList.toList lr
        PList.applyDelta r rl |> snd |> PDeltaList.toList = PDeltaList.toList rl
    ]
    
[<Property>]
let ``[PList] insertAfter`` (i : int) (l : list<int>) =
    (i >= 0 && i < l.Length) ==> lazy (
        let mutable p = PList.empty
        let mutable last = Index.zero
        let mutable idx = 0
        let mutable key = Index.zero
        for e in l do
            let id = Index.after last
            p <- p.Set(id, e)
            if idx = i then key <- id
            last <- id
            idx <- idx + 1
    
        let check = l |> List.mapi (fun ii v -> if i = ii then [v;123] else [v]) |> List.concat
        let test = PList.insertAfter key 123 p

        PList.toList test = check
    )    
[<Property>]
let ``[PList] insertBefore`` (i : int) (l : list<int>) =
    (i >= 0 && i < l.Length) ==> lazy (
        let mutable p = PList.empty
        let mutable last = Index.zero
        let mutable idx = 0
        let mutable key = Index.zero
        for e in l do
            let id = Index.after last
            p <- p.Set(id, e)
            if idx = i then key <- id
            last <- id
            idx <- idx + 1
    
        let check = l |> List.mapi (fun ii v -> if i = ii then [123; v] else [v]) |> List.concat
        let test = PList.insertBefore key 123 p

        PList.toList test = check
    )

