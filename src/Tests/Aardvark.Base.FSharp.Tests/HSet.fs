module HSet

open System
open NUnit
open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open Aardvark.Base
open System.Diagnostics

module List =
    let all (l : list<bool>) =
        l |> List.fold (&&) true


[<Property(Verbose = true)>]
let ``[HSet] compute delta`` (l : Set<int>) (r : Set<int>) (b : bool) =
    let a = 10
    let l, r =
        if b then
            HRefSet.ofSeq l, HRefSet.ofSeq r
        else
            let l = HRefSet.ofSeq (Set.remove a l)
            let r = HRefSet.ofSeq (Set.add a r)
            l, r

    List.all [
        HRefSet.applyDelta l (HRefSet.computeDelta l r) |> fst |> Set.ofSeq |> ((=) (Set.ofSeq r))
        HRefSet.applyDelta r (HRefSet.computeDelta r l) |> fst |> Set.ofSeq |> ((=) (Set.ofSeq l))

        b || HRefSet.computeDelta l (HRefSet.add a l) |> HDeltaSet.toList |> ((=) [Add a])
        b || HRefSet.computeDelta r (HRefSet.remove a r) |> HDeltaSet.toList |> ((=) [Rem a])

        b || HRefSet.computeDelta r (HRefSet.add a r) |> HDeltaSet.toList |> ((=) [])
        b || HRefSet.computeDelta l (HRefSet.remove a l) |> HDeltaSet.toList |> ((=) [])

        HDeltaSet.toList (HRefSet.computeDelta l r) = (HRefSet.computeDelta r l |> HDeltaSet.toList |> List.map (fun op -> op.Inverse))

    ]

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

[<Test>]
let ``[HSet] performance``() =
    let old = System.Runtime.GCSettings.LatencyMode
    try
        System.Runtime.GCSettings.LatencyMode <- System.Runtime.GCLatencyMode.Batch
        let rand = RandomSystem()

        for initialSize in 100 .. 100 .. 3000 do
            Log.start "size %d" initialSize
            let hash = System.Collections.Generic.HashSet<int>()
            let mutable hs = HSet.empty
    
            let values = Array.init initialSize (fun _ -> rand.UniformInt())
            for i in values do
                hash.Add i |> ignore

            for i in values do
                hs <- HSet.add i hs

            GC.Collect()
            GC.WaitForFullGCComplete() |> ignore
            GC.WaitForFullGCComplete() |> ignore

            let values = Array.init 100000 (fun _ -> rand.UniformInt())
            let sw = System.Diagnostics.Stopwatch()
            sw.Start()
            for i in 0 .. values.Length - 1 do
                hash.Add values.[i] |> ignore
            sw.Stop()
            let thash = sw.MicroTime / values.Length
            Log.line "HashSet.Add: %A" thash

            GC.Collect()
            GC.WaitForFullGCComplete() |> ignore
            GC.WaitForFullGCComplete() |> ignore


            sw.Restart()
            for i in 0 .. values.Length - 1 do
                HSet.add values.[i] hs |> ignore
            sw.Stop()
            let ths = sw.MicroTime / values.Length
            Log.line "HSet.add: %A" ths

            Log.line "factor: %.3f" (ths / thash)
    
            Log.stop()
            GC.Collect()
            GC.WaitForFullGCComplete() |> ignore
            GC.WaitForFullGCComplete() |> ignore
            ()

    finally
        System.Runtime.GCSettings.LatencyMode <- old