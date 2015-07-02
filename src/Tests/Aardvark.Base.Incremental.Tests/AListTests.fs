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
        d |> AList.toList |> should equal [1]

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

            let mutable t = r.RootTime.Next
            while t <> r.RootTime do
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
        let callback (d  : list<Delta<Time * int>>) = deltaRef := !deltaRef @ (d |> List.map (Delta.map snd))

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








