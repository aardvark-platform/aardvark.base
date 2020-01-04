namespace Aardvark.Base.Incremental.Tests


open System
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit

module ``simple cmap tests`` =
    open Aardvark.Base.HashMap

//
//    let dictEqual (xs : list<'k * 'v>) (set : IVersionedDictionary<'k,IVersionedSet<'v>>) =
//        let kvs = set |> Seq.collect (fun (KeyValue(k,v)) ->
//            v |> Seq.map (fun v ->
//                k,v
//            )
//        )
//        let set = HashSet(kvs)
//        set.SetEquals(xs) |> should be True
//            
//        
//
//    [<Test>]
//    let ``[CMap] cmap add remove``() =
//        let l = CMap.empty
//
//        transact (fun () -> 
//            l |> CMap.set 1 2 
//            l |> CMap.set 1 3
//        )
//
//        let m = AMap.toMod l
//        m |> Mod.force |> dictEqual [ 1, 3 ]
//
//        transact (fun () -> 
//            l |> CMap.set 5 2 
//            l |> CMap.set 1 6
//        )
//
//        m |> Mod.force |> dictEqual [ 1, 6; 5, 2]
//
//        transact (fun () -> 
//            l |> CMap.remove 5
//        ) |> should be True
//
//        m |> Mod.force |> dictEqual [ 1, 6; ]
//        
//    [<Test>]
//    let ``[AMap] test duplicates``() =
//        let l = CMap.ofList [1,2]
//        let r = CMap.ofList [1,3]
//        let comp = AMap.union' [l;r]
//        let m = comp |> AMap.toMod
//
//        let lookup = AMap.tryFindAll 1 comp
//        let reader = lookup.GetReader()
//        
//        m |> Mod.force |> dictEqual [ 1, 2; 1, 3 ]
//        let delta = reader.GetDelta() 
//        printfn "delt %A" delta
//        delta |> should setEqual [ Add (2); Add (3)]
//
//        transact (fun () -> 
//            l |> CMap.remove 1 
//        ) |> should be True
//
//        m |> Mod.force |> dictEqual [ 1, 3 ]
//        reader.GetDelta() |> should setEqual [ Rem (2) ]
//
//        transact (fun () -> 
//            r |> CMap.remove 1 
//        ) |> should be True
//
//        m |> Mod.force |> dictEqual [ ]
//        reader.GetDelta() |> should setEqual [  Rem (3)]
//
//        transact (fun () -> 
//            r |> CMap.add 1 2
//        ) 
//
//        m |> Mod.force |> dictEqual [ 1,2 ]
//        reader.GetDelta() |> should setEqual [ Add (2)]
//
//    [<Test>]
//    let ``[CMap] test clear``() =
//        let l = CMap.ofList [1,2]
//        let r = l |> AMap.map (fun k v -> v * 2)
//
//        let m = r |> AMap.toMod
//
//        m |> Mod.force |> dictEqual [ 1,4 ]
//
//        transact (fun () ->
//            l.Clear ()
//        )
//        
//        m |> Mod.force |> dictEqual [ ]
//
//
//
//

    [<Test>]
    let ``[AMap] add/clear/evaluate``() =
        
        let m = CMap.empty<int,int>
        let refSet = new HashSet<int*int>()
        
        let set = AMap.toASet m

        let rnd = Random()

        for i in 0..10000 do
            if rnd.NextDouble() < 0.5 then
                printfn "add"
                refSet.Add((i,i)) |> ignore
                transact(fun () -> m.[i] <- i)
                printfn " -> cnt=%d" m.Count

            if rnd.NextDouble() < 0.1 then
                printfn "clear"
                refSet.Clear()
                transact(fun () -> m.Clear())
                printfn " -> cnt=%d" m.Count
                should equal 0 m.Count

            if rnd.NextDouble() < 0.2 then
                printfn "eval"
                let test = ASet.toArray set
                printfn " -> map=%d set=%d" m.Count test.Length
                should equal test.Length m.Count
                should equal test.Length refSet.Count
                should setEqual test (Seq.toArray refSet) 
        ()
