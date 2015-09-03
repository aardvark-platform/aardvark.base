namespace Aardvark.Base.Incremental.Tests


open System.Collections
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit

module ``simple list tests`` =

    module Delta =
        let map (f : 'a -> 'b) (d : Delta<'a>) =
            match d with
                | Add v -> Add (f v)
                | Rem v -> Rem (f v)

    type IListReader<'a> with
        member x.GetSetDelta() =
            x.GetDelta() |> List.map (Delta.map snd)

    [<Test>]
    let ``clist add / remove``() =
        let l = clist [1;2;3]

        let d = l |> AList.map (fun a -> 2 * a)

        let r = d.GetReader()

        let d = r.GetDelta() |> List.map (Delta.map snd)
        d |> should setEqual [Add 2; Add 4; Add 6]

        let k4 =
            transact (fun () ->
                l.Add 4
            )

        let d = r.GetDelta() |> List.map (Delta.map snd)
        d |> should setEqual [Add 8]

        let k5 =
            transact (fun () ->
                l.InsertBefore(k4, 4)
            )

        let d = r.GetDelta() |> List.map (Delta.map snd)
        d |> should setEqual [Add 8]


        transact (fun () ->
            l.Remove k4
        )
        let d = r.GetDelta() |> List.map (Delta.map snd)
        d |> should setEqual [Rem 8]

    [<Test>]
    let ``alist collect``() =
        let l0 = clist []
        let l1 = clist []
        let l2 = clist []
        let l = clist [l0 :> alist<_>; l1 :> alist<_>]

        let d = l |> AList.concat
        let r = d.GetReader()


        r.GetSetDelta() |> should setEqual []

        let k1 = transact (fun () -> l0.Add 1)
        r.GetSetDelta() |> should setEqual [Add 1]

        let kl2 = transact (fun () -> l.Add l2)
        r.GetSetDelta() |> should setEqual []

        let k5 = transact (fun () -> l2.Add 5)
        r.GetSetDelta() |> should setEqual [Add 5]


        transact (fun () ->
            l2.Add 6 |> ignore
            l.Remove kl2
        )
        r.GetSetDelta() |> should setEqual [Rem 5]


    [<Test>]
    let ``alist collect outer reomve``() =
        let l0 = AList.ofList [ 1; 2 ]
        let l1 = AList.ofList [ 3; 4 ]
        let l2 = AList.ofList []
        let l = clist [l0]

        let d = l |> AList.concat
        let r = d.GetReader()

        r.GetSetDelta() |> should setEqual [ Add 1 ; Add 2]
        r.Content |> Seq.forall (fun (k,v) -> not k.IsDeleted) |> should be True
        r.Content.Count |> should equal 2

        transact (fun () -> l.Add l1 |> ignore; l.RemoveAt 0)
        r.GetSetDelta() |> should setEqual [Add 3; Add 4; Rem 1; Rem 2]
        r.Content |> Seq.forall (fun (k,v) -> not k.IsDeleted) |> should be True
        r.Content.Count |> should equal 2

        transact (fun () -> l.Add l0 |> ignore; l.RemoveAt 0)
        r.GetSetDelta() |> should setEqual [Rem 3; Rem 4; Add 1; Add 2]
        r.Content |> Seq.forall (fun (k,v) -> not k.IsDeleted) |> should be True
        r.Content.Count |> should equal 2

        transact (fun () -> l.Add l1 |> ignore; l.RemoveAt 0)
        r.GetSetDelta() |> should setEqual [Add 3; Add 4; Rem 1; Rem 2]
        r.Content |> Seq.forall (fun (k,v) -> not k.IsDeleted) |> should be True
        r.Content.Count |> should equal 2

        transact (fun () -> l.Add l2 |> ignore; l.RemoveAt 0)
        r.GetSetDelta() |> should setEqual [Rem 3; Rem 4]
        r.Content |> Seq.forall (fun (k,v) -> not k.IsDeleted) |> should be True
        r.Content.Count |> should equal 0

    [<Test>]
    let ``alist order``() =
        let l0 = clist []
        let l1 = clist []
        let l2 = clist []
        let l = clist []

        let kl0 = transact (fun () -> l.Add (l0 :> alist<_>))
        let kl1 = transact (fun () -> l.Add (l1 :> alist<_>))

        let d = l |> AList.concat

        d |> AList.toList |> should equal []

        let kl2 = transact (fun () -> l.Add l2)
        d |> AList.toList |> should equal []

        let k21 = transact (fun () -> l2.Add 1)
        let ll = d |> AList.toList
        ll |> should equal [1]

        let k01 = transact (fun () -> l0.Add 1)
        d |> AList.toList |> should equal [1; 1]

        let k12 = transact (fun () -> l1.Add 2)
        d |> AList.toList |> should equal [1; 2; 1]

        let k13 = transact (fun () -> l1.Add 3)
        d |> AList.toList |> should equal [1; 2; 3; 1]

        transact (fun () -> l.Remove kl1)
        d |> AList.toList |> should equal [1; 1]

    [<Test>]
    let ``ordered set test``() =
        let s = corderedset [1;2;3]
        let d = s |> AList.map (fun a -> 2 * a)

        let r = d.GetReader()
        d |> AList.toList |> should equal [2;4;6]
        r.GetSetDelta() |> should setEqual [Add 2; Add 4; Add 6]


        transact (fun () -> s.InsertAfter(2, 6)) |> ignore
        d |> AList.toList |> should equal [2;4;12;6]
        r.GetSetDelta() |> should setEqual [Add 12]

    [<Test>]
    let ``choose reader time-density``() =
        let l = corderedset []

        let validateTimeDensity(r : IListReader<'a>) =
            r.GetDelta() |> ignore

            let mutable t = r.RootTime.Root.Next
            while t <> r.RootTime.Root do
                match r.Content.TryGetValue t with
                    | (true, v) -> ()
                    | _ -> failwithf "no value associated with time %A" t
                t <- t.Next

        let add v = l.Add v |> ignore
        let content (r : IListReader<'a>) = 
            r.GetDelta() |> ignore
            r.Content |> Seq.sortBy fst |> Seq.map snd |> Seq.toList
        // get only those elements being even
        let m2 = l |> AList.choose (fun a -> if a % 2 = 0 then Some a else None)
        let r = m2.GetReader()

        r |> validateTimeDensity

        transact (fun () -> add 1; add 2)
        r |> content |> should equal [2]
        r |> validateTimeDensity

        transact (fun () -> l.InsertAfter(1, 4) |> ignore)
        r |> content |> should equal [4; 2]
        r |> validateTimeDensity

        transact (fun () -> l.InsertBefore(4, 6) |> ignore)
        r |> content |> should equal [6; 4; 2]
        r |> validateTimeDensity

        transact (fun () -> l.Remove(4) |> ignore)
        r |> content |> should equal [6; 2]
        r |> validateTimeDensity

        transact (fun () -> l.Clear())
        r |> content |> should equal []
        r |> validateTimeDensity



        ()

    [<Test>]
    let ``callback survive test``() =
        let subscribe cb (l : alist<'a>) =
            let s = l |> AList.registerCallback cb
            // s is ignored here (going out of scope)
            ()

        let deltaRef = ref []
        let callback (d  : list<Delta<ISortKey * int>>) = deltaRef := !deltaRef @ (d |> List.map (Delta.map snd))

        let deltas() =
            let l = !deltaRef
            deltaRef := []
            l

        let l = corderedset [1]
        subscribe callback l
        System.GC.AddMemoryPressure(1L <<< 30)
        System.GC.Collect()


        deltas() |> should setEqual [Add 1]

        transact (fun () -> l.Add 2 |> ignore; l.Add 3 |> ignore)
        deltas() |> should setEqual [Add 2; Add 3]

        transact (fun () -> l.Remove 1 |> ignore)
        deltas() |> should setEqual [Rem 1]


        ()

    [<Test>]
    let ``clist indexing test``() =
        let c = CList.ofList [1;2;3;4]
        
        // stupid obfuscated identity
        let d = c |> AList.collect AList.single |> AList.choose Some

        let r = d.GetReader()
        let content (r : IListReader<'a>) = 
            r.GetDelta() |> ignore
            r.Content |> Seq.sortBy fst |> Seq.map snd |> Seq.toList

        r |> content |> should equal [1;2;3;4]
        c |> CList.count |> should equal 4

        transact (fun () -> c |> CList.insert 0 0)
        r |> content |> should equal [0;1;2;3;4]
        c |> CList.count |> should equal 5

        transact (fun () -> c |> CList.remove 1)
        r |> content |> should equal [0;2;3;4]
        c |> CList.count |> should equal 4

        transact (fun () -> c |> CList.insertRange 1 [1;2])
        r |> content |> should equal [0;1;2;2;3;4]
        c |> CList.count |> should equal 6

        transact (fun () -> c |> CList.removeRange 1 2)
        r |> content |> should equal [0;2;3;4]
        c |> CList.count |> should equal 4

        c.[0] |> should equal 0
        c.[1] |> should equal 2
        c.[2] |> should equal 3
        c.[3] |> should equal 4

        transact (fun () -> c.[0] <- 1)
        r |> content |> should equal [1;2;3;4]
        c |> CList.count |> should equal 4

        transact (fun () -> c.[3] <- 1)
        r |> content |> should equal [1;2;3;1]
        c |> CList.count |> should equal 4

        transact (fun () -> c |> CList.add 5)
        r |> content |> should equal [1;2;3;1;5]
        c |> CList.count |> should equal 5

        transact (fun () -> c |> CList.addRange [6;7])
        r |> content |> should equal [1;2;3;1;5;6;7]
        c |> CList.count |> should equal 7

        c |> CList.tryGet -1 |> should equal None
        c |> CList.tryGet 7 |> should equal None
        c |> CList.tryGet 0 |> should equal (Some 1)
        c |> CList.tryGet 6 |> should equal (Some 7)

        transact (fun () -> c |> CList.clear)
        r |> content |> should equal []
        c |> CList.count |> should equal 0

        
        CList.empty<int> = CList.empty<int> |> should be False

        ()

    [<Test>]
    let ``set sorting``() =
        let s = CSet.ofList [4;3;2;1]

        let l = s |> ASet.sort
        let r = l.GetReader()

        let content (r : IListReader<'a>) = 
            r.GetDelta() |> ignore
            r.Content |> Seq.sortBy fst |> Seq.map snd |> Seq.toList

        r |> content |>  should equal [1;2;3;4]
        
        transact (fun () -> CSet.add 5 s |> ignore)
        r |> content |>  should equal [1;2;3;4;5]

        transact (fun () -> CSet.remove 3 s |> ignore)
        r |> content |>  should equal [1;2;4;5]

        transact (fun () -> CSet.add 3 s |> ignore)
        r |> content |>  should equal [1;2;3;4;5]

        transact (fun () -> CSet.clear s )
        r |> content |>  should equal []
        

    [<Test>]
    let ``alist construction of aset``() =

        let set = CSet.ofList [ 1; 2; 3 ]

        let list = AList.ofASet set


        let r = list.GetReader()
        let content (r : IListReader<'a>) = 
            r.GetDelta() |> ignore
            r.Content |> Seq.sortBy fst |> Seq.map snd |> Seq.toList

        transact (fun () -> CSet.add 12 set |> ignore)
        r |> content |> should equal [  1; 2; 3; 12; ]

        transact (fun () -> CSet.remove 3 set |> ignore)
        r |> content |> should equal [  1; 2; 12 ]


    [<Test>]
    let ``alist construction of aset with selection``() =

        let set = CSet.ofList [ 1; 2; 3 ]
        
        let list =
            alist {
                for x in AList.ofASet set do
                    yield x * 2
            }

        let r = list.GetReader()
        let content (r : IListReader<'a>) = 
            r.GetDelta() |> ignore
            r.Content |> Seq.sortBy fst |> Seq.map snd |> Seq.toList

        transact (fun () -> CSet.add 12 set |> ignore)
        r |> content |> should equal [ 2; 4; 6; 24]


