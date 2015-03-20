namespace Aardvark.Base.Incremental.Tests


open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit

module ``collect tests`` =
    type Tree<'a> = Node of aset_check<Tree<'a>> | Leaf of aset_check<'a>

    type DeltaList<'a>() =
        let store = List<list<Delta<'a>>>()

        member x.push (deltas : list<Delta<'a>>) =
            store.Add deltas 

        member x.read() =
            let res = store |> Seq.toList
            store.Clear()
            res


    [<Test>]
    let ``duplicate handling in collect``() =
        let i0 = CSetCheck.ofList [1;2;3]
        let i1 = CSetCheck.ofList [3;4;5]
        let s = CSetCheck.ofList [i0 :> aset_check<_>]

        // union {{1,2,3}}
        let d = s |> ASetCheck.collect id
        let r = d.GetReader()
        r.GetDelta() |> should setEqual [Add 1; Add 2; Add 3]
        r.Content |> should setEqual [1;2;3]
        
        // union {{1,2,3}, {3,4,5}} -> { Add 4, Add 5 }
        transact (fun () ->
            s.Add i1 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 4; Add 5]
        r.Content |> should setEqual [1;2;3;4;5]

        // union {{3,4,5}} -> { Rem 1, Rem 2 }
        transact (fun () ->
            s.Remove i0 |> should equal true
        )
        r.GetDelta() |> should setEqual [Rem 1; Rem 2]
        r.Content |> should setEqual [3;4;5]

        // union {{4,5}} -> { Rem 3 }
        transact (fun () ->
            i1.Remove 3 |> should equal true
        )
        r.GetDelta() |> should setEqual [Rem 3]
        r.Content |> should setEqual [4;5]

        // union {{1,2,3}, {4,5}} -> { Add 1, Add 2, Add 3 }
        transact (fun () ->
            s.Add i0 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 1; Add 2; Add 3]
        r.Content |> should setEqual [1;2;3;4;5]

        // union {{1,2,3}, {3,4,5}} -> { }
        transact (fun () ->
            i1.Add 3 |> should equal true
        )
        r.GetDelta() |> should setEqual []
        r.Content |> should setEqual [1;2;3;4;5]

        // union {{1,2}, {3,4,5}} -> { }
        transact (fun () ->
            i0.Remove 3 |> should equal true
        )
        r.GetDelta() |> should setEqual []
        r.Content |> should setEqual [1;2;3;4;5]

        // union {{1,2}, {4,5}} -> { Rem 3 }
        transact (fun () ->
            i1.Remove 3 |> should equal true
        )
        r.GetDelta() |> should setEqual [Rem 3]
        r.Content |> should setEqual [1;2;4;5]

    [<Test>]
    let ``move test``() =
        let c0 = CSetCheck.ofList [1]
        let c1 = CSetCheck.ofList [2]

        let s = CSetCheck.ofList[c0 :> aset_check<_>;c1 :> aset_check<_>]

        let res = s |> ASetCheck.collect id
        let r = res.GetReader()

        // union {{1},{2}}
        r.GetDelta() |> should setEqual [Add 1; Add 2]
        r.Content |> should setEqual [1;2]

        // union {{2},{1}}
        transact (fun () ->
            c0.Remove 1 |> should equal true
            c1.Add 1 |> should equal true
            c1.Remove 2 |> should equal true
            c0.Add 2 |> should equal true
        )
        r.GetDelta() |> should setEqual []
        r.Content |> should setEqual [1;2]
    
    [<Test>]
    let ``tree flatten test``() =
        let rec flatten (t : Tree<'a>) =
            match t with
                | Node children -> children |> ASetCheck.collect (flatten)
                | Leaf values -> values

        let l2 = CSetCheck.ofList [1]
        let l1 = CSetCheck.ofList [Leaf l2]
        let l0 = CSetCheck.ofList [Node l1]
        let t = Node (l0)

        // Node { Node { Leaf { 1 } } }
        let s = flatten t
        let r = s.GetReader()
        r.GetDelta() |> should setEqual [Add 1]
        r.Content |> should setEqual [1]


        // Node { Node { Leaf { 1, 3 }, Leaf { 2 } } }
        let l22 = CSetCheck.ofList [2] :> aset_check<_> |> Leaf
        transact (fun () ->
            l1.Add l22 |> should equal true
            l2.Add 3 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 2; Add 3]
        r.Content |> should setEqual [1; 2; 3]


        // Node { Leaf { 1, 5 } , Node { Leaf { 1, 3 }, Leaf { 2 } } }
        let l12 = CSetCheck.ofList [1;5] :> aset_check<_> |> Leaf
        transact (fun () ->
            l0.Add l12 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 5]
        r.Content |> should setEqual [1; 2; 3; 5]

        // Node { Leaf { 1, 5 } , Node { Leaf { 2 } } }
        transact (fun () ->
            l1.Remove (Leaf l2) |> should equal true
        )
        r.GetDelta() |> should setEqual [Rem 3]
        r.Content |> should setEqual [1; 2; 5]

        // Node { Leaf { 1, 5 } , Node { Leaf { 2 }, Node { Leaf { 17 } } } }
        let n = Node (CSetCheck.ofList [Leaf (CSetCheck.ofList [17])])
        transact (fun () ->
            l1.Add n |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 17]
        r.Content |> should setEqual [1; 2; 5; 17]

        ()

    [<Test>]
    let ``callback test``() =

        let deltas = DeltaList<int>()

        let set = cset [1;2;3;4]
        let m = set |> ASet.map (fun a -> 2 * a)
        let s = m |> ASet.registerCallback deltas.push

        // { 1, 2, 3, 4 } -> [Add 2; Add 4; Add 6; Add 8]
        deltas.read() |> should deltaListEqual [[Add 2; Add 4; Add 6; Add 8]]

        // { 1, 3, 4, 5 } -> [Add 10; Rem 4]
        transact (fun () ->
            set.Add 5 |> ignore
            set.Remove 2 |> ignore
        )
        deltas.read() |> should deltaListEqual [[Rem 4; Add 10]]

        // { 1, 4, 5, 6 } -> [Add 12; Rem 6]
        transact (fun () ->
            set.Add 6 |> ignore
            set.Remove 3 |> ignore
        )
        deltas.read() |> should deltaListEqual [[Rem 6; Add 12]]

        // { 1, 4, 5, 6, 7 } -> [Add 14]
        transact (fun () -> set.Add 7 |> ignore)

        // { 1, 4, 5, 6, 7, 8 } -> [Add 16]
        transact (fun () -> set.Add 8 |> ignore)
        deltas.read() |> should deltaListEqual [[Add 14]; [Add 16]]


        s.Dispose()

        // { 1, 4, 5, 6, 7 } -> [] (unsubscribed)
        transact (fun () -> set.Add 9 |> ignore)
        deltas.read() |> should deltaListEqual ([] : list<list<Delta<int>>>)




        ()

    [<Test>]
    let ``callback changing other list``() =
        
        let set = cset [1;2]

        let derived = set |> ASet.map (fun a -> 1 + a)
        let inner = cset []

        let s = derived |> ASet.registerCallback (fun delta ->
            for d in delta do
                match d with
                    | Add v -> inner.Add v |> ignore
                    | Rem v -> inner.Remove v |> ignore
        )

        
        let dervivedInner = inner |> ASet.map id

        let r = dervivedInner.GetReader()
        r.GetDelta() |> should setEqual [Add 2; Add 3]

        transact (fun () ->
            set.Add 3 |> should equal true
        )
        r.GetDelta() |> should setEqual [Add 4]





