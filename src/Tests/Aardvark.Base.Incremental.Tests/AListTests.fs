namespace Aardvark.Base.Incremental.Tests

open System
open System.Collections
open System.Collections.Generic
open Aardvark.Base
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open System.Threading.Tasks
open System.Runtime.CompilerServices
//
//[<AbstractClass; Sealed; Extension>]
//type ListReaderExtensions private() =
//
//    [<Extension>]
//    static member GetSetDelta(x : IListReader<'a>) =
//        let oldContent = x.State.Content
//        x.GetOperations null 
//            |> DeltaList.toSeq
//            |> Seq.map (fun (i,op) ->
//                match op with
//                    | Set v -> Add v
//                    | Remove ->
//                        MapExt.find i oldContent |> Rem
//            )
//            |> Seq.toList

module ``simple list tests`` =
    open Aardvark.Base.IL.Serializer.RefMap
    open Aardvark.Base.CSharpList

    module Delta =
        let map (f : 'a -> 'b) (d : SetOperation<'a>) =
            SetOperation(f d.Value, d.Count)


    [<Test>]
    let ``[AList] clist add / remove``() =
        let l = clist [1;2;3]

        let d = l |> AList.map (fun a -> 2 * a)

        let r = d.GetReader()
        let d = r.GetOperations().ToSeq() |> Seq.map (fun (a,b) -> b)
        d |> should setEqual [Set 2; Set 4; Set 6]

        transact (fun () ->
            l.Append 4 |> ignore
        )

        let d = r.GetOperations().ToSeq() |> Seq.map (fun (a,b) -> b)
        d |> should setEqual [Set 8]

        let k5 =
            transact (fun () ->
                l.Insert(3, 4)
            )

        let d = r.GetOperations().ToSeq() |> Seq.map (fun (a,b) -> b)
        d |> should setEqual [Set 8]


        transact (fun () ->
            l.RemoveAt 3 |> ignore
        )
        let d = r.GetOperations().ToSeq() |> Seq.map (fun (a,b) -> b)
        d |> should setEqual [ElementOperation<int>.Remove]

    [<Test>]
    let ``[COrderedSet] contains``() =
        
        let a = "a"
        let b = "b"
        let c = "c"

        let set = COrderedSet.ofSeq [a; b; c]
                
        set.Contains(a) |> should be True
        set.Contains(b) |> should be True
        set.Contains(c) |> should be True

        ()

    [<Test>]
    let ``[AList] add/clear/evaluate``() =
        
        let l = CList.empty
        let refSet = new HashSet<int>()
        
        let set = AList.toASet l

        let rnd = Random()

        for i in 0..10000 do
            if rnd.NextDouble() < 0.5 then
                printfn "add"
                refSet.Add(i) |> ignore
                transact(fun () -> l.Append i |> ignore)
                printfn " -> cnt=%d" l.Count

            if rnd.NextDouble() < 0.1 then
                printfn "clear"
                refSet.Clear()
                transact(fun () -> l.Clear())
                printfn " -> cnt=%d" l.Count
                should equal 0 l.Count

            if rnd.NextDouble() < 0.2 then
                printfn "eval"
                let test = ASet.toArray set
                printfn " -> list=%d set=%d" l.Count test.Length
                should equal test.Length l.Count
                should equal test.Length refSet.Count
                should equal test (Seq.toArray refSet) 

    [<Test>]
    let ``[AList] toASet``() =
        
        // test should not crash
        let l = CList.empty
        
        let set = AList.toASet l
        
        transact(fun () -> l.Append 1 |> ignore)

        let test = ASet.toArray set
        printf "cnt=%d" test.Length
        should equal test.Length 1
        should equal test.[0] 1

        transact(fun () -> l.Append 2 |> ignore)
        transact(fun () -> l.Clear())
        transact(fun () -> l.Append 3 |> ignore)
        
        let test = ASet.toArray set
        printf "cnt=%d" test.Length
        should equal test.Length 1
        should equal test.[0] 3

        ()


//    [<Test>]
//    let ``[AList] collect``() =
//        let l0 = clist []
//        let l1 = clist []
//        let l2 = clist []
//        let l = clist [l0 :> alist<_>; l1 :> alist<_>]
//
//        let d = l |> AList.concat
//        let r = d.GetReader()
//
//
//        r.GetSetDelta() |> should setEqual []
//
//        let k1 = transact (fun () -> l0.Append 1)
//        r.GetSetDelta() |> should setEqual [Add 1]
//
//        let kl2 = transact (fun () -> l.Append l2)
//        r.GetSetDelta() |> should setEqual []
//
//        let k5 = transact (fun () -> l2.Append 5)
//        r.GetSetDelta() |> should setEqual [Add 5]
//
//
//        transact (fun () ->
//            l2.Append 6 |> ignore
//            l.Remove kl2 |> ignore
//        )
//        r.GetSetDelta() |> should setEqual [Rem 5]
//
//
//    [<Test>]
//    let ``[AList] alist collect outer reomve``() =
//        let l0 = AList.ofList [ 1; 2 ]
//        let l1 = AList.ofList [ 3; 4 ]
//        let l2 = AList.ofList []
//        let l = clist [l0]
//
//        let d = l |> AList.concat
//        let r = d.GetReader()
//
//        r.GetSetDelta() |> should setEqual [ Add 1 ; Add 2]
//        r.State.Count |> should equal 2
//
//        transact (fun () -> l.Append l1 |> ignore; l.RemoveAt 0)
//        r.GetSetDelta() |> should setEqual [Add 3; Add 4; Rem 1; Rem 2]
//        r.State.Count |> should equal 2
//
//        transact (fun () -> l.Append l0 |> ignore; l.RemoveAt 0)
//        r.GetSetDelta() |> should setEqual [Rem 3; Rem 4; Add 1; Add 2]
//        r.State.Count |> should equal 2
//
//        transact (fun () -> l.Append l1 |> ignore; l.RemoveAt 0)
//        r.GetSetDelta() |> should setEqual [Add 3; Add 4; Rem 1; Rem 2]
//        r.State.Count |> should equal 2
//
//        transact (fun () -> l.Append l2 |> ignore; l.RemoveAt 0)
//        r.GetSetDelta() |> should setEqual [Rem 3; Rem 4]
//        r.State.Count |> should equal 0
//
//    [<Test>]
//    let ``[AList] alist order``() =
//        let l0 = clist []
//        let l1 = clist []
//        let l2 = clist []
//        let l = clist []
//
//        let kl0 = transact (fun () -> l.Append (l0 :> alist<_>))
//        let kl1 = transact (fun () -> l.Append (l1 :> alist<_>))
//
//        let d = l |> AList.concat
//
//        d |> AList.toList |> should equal []
//
//        let kl2 = transact (fun () -> l.Append l2)
//        d |> AList.toList |> should equal []
//
//        let k21 = transact (fun () -> l2.Append 1)
//        let ll = d |> AList.toList
//        ll |> should equal [1]
//
//        let k01 = transact (fun () -> l0.Append 1)
//        d |> AList.toList |> should equal [1; 1]
//
//        let k12 = transact (fun () -> l1.Append 2)
//        d |> AList.toList |> should equal [1; 2; 1]
//
//        let k13 = transact (fun () -> l1.Append 3)
//        d |> AList.toList |> should equal [1; 2; 3; 1]
//
//        transact (fun () -> l.Remove kl1)
//        d |> AList.toList |> should equal [1; 1]
//
////    [<Test>]
////    let ``[AList] ordered set test``() =
////        let s = corderedset [1;2;3]
////        let d = s |> AList.map (fun a -> 2 * a)
////
////        let r = d.GetReader()
////        d |> AList.toList |> should equal [2;4;6]
////        r.GetSetDelta() |> should setEqual [Add 2; Add 4; Add 6]
////
////
////        transact (fun () -> s.InsertAfter(2, 6)) |> ignore
////        d |> AList.toList |> should equal [2;4;12;6]
////        r.GetSetDelta() |> should setEqual [Add 12]
////
////    [<Test>]
////    let ``[AList] choose reader time-density``() =
////        let l = corderedset []
////
////        let validateTimeDensity(r : IListReader<'a>) =
////            r.GetDelta() |> ignore
////            ()
//////            let mutable t = r.RootTime.Root.Next
//////            while t <> r.RootTime.Root do
//////                match r.Content.TryGetValue t with
//////                    | (true, v) -> ()
//////                    | _ -> failwithf "no value associated with time %A" t
//////                t <- t.Next
////
////        let add v = l.Add v |> ignore
////        let content (r : IListReader<'a>) = 
////            r.GetDelta() |> ignore
////            r.Content.All |> Seq.sortBy fst |> Seq.map snd |> Seq.toList
////        // get only those elements being even
////        let m2 = l |> AList.choose (fun a -> if a % 2 = 0 then Some a else None)
////        let r = m2.GetReader()
////
////        r |> validateTimeDensity
////
////        transact (fun () -> add 1; add 2)
////        r |> content |> should equal [2]
////        r |> validateTimeDensity
////
////        transact (fun () -> l.InsertAfter(1, 4) |> ignore)
////        r |> content |> should equal [4; 2]
////        r |> validateTimeDensity
////
////        transact (fun () -> l.InsertBefore(4, 6) |> ignore)
////        r |> content |> should equal [6; 4; 2]
////        r |> validateTimeDensity
////
////        transact (fun () -> l.Remove(4) |> ignore)
////        r |> content |> should equal [6; 2]
////        r |> validateTimeDensity
////
////        transact (fun () -> l.Clear())
////        r |> content |> should equal []
////        r |> validateTimeDensity
////
////
////
////        ()
//
////    [<Test>]
////    let ``[AList] callback survive test``() =
////        let subscribe cb (l : alist<'a>) =
////            let s = l |> AList.unsafeRegisterCallbackNoGcRoot cb
////            // s is ignored here (going out of scope)
////            ()
////
////        let deltaRef = ref []
////        let callback (d  : list<Delta<ISortKey * int>>) = deltaRef := !deltaRef @ (d |> List.map (Delta.map snd))
////
////        let deltas() =
////            let l = !deltaRef
////            deltaRef := []
////            l
////
////        let l = corderedset [1]
////        subscribe callback l
////        System.GC.AddMemoryPressure(1L <<< 30)
////        System.GC.Collect()
////
////
////        deltas() |> should setEqual [Add 1]
////
////        transact (fun () -> l.Add 2 |> ignore; l.Add 3 |> ignore)
////        deltas() |> should setEqual [Add 2; Add 3]
////
////        transact (fun () -> l.Remove 1 |> ignore)
////        deltas() |> should setEqual [Rem 1]
////
////
////        ()
//
//    [<Test>]
//    let ``[AList] clist indexing test``() =
//        let c = clist [1;2;3;4]
//        
//        // stupid obfuscated identity
//        let d = c |> AList.collect AList.single |> AList.choose Some
//
//        let r = d.GetReader()
//        let content (r : IListReader<'a>) = 
//            r.GetDelta() |> ignore
//            r.State |> PList.toList
//
//        r |> content |> should equal [1;2;3;4]
//        c |> Seq.length |> should equal 4
//
//        transact (fun () -> c.Insert(0, 0))
//        r |> content |> should equal [0;1;2;3;4]
//        c |> Seq.length |> should equal 5
//
//        transact (fun () -> c.RemoveAt 1)
//        r |> content |> should equal [0;2;3;4]
//        c |> Seq.length |> should equal 4
//
//        transact (fun () -> c.Insert(1, 2); c.Insert(1, 1))
//        r |> content |> should equal [0;1;2;2;3;4]
//        c |> Seq.length |> should equal 6
//
//        transact (fun () -> c |> CList.removeRange 1 2)
//        r |> content |> should equal [0;2;3;4]
//        c |> Seq.length |> should equal 4
//
//        c.[0] |> should equal 0
//        c.[1] |> should equal 2
//        c.[2] |> should equal 3
//        c.[3] |> should equal 4
//
//        transact (fun () -> c.[0] <- 1)
//        r |> content |> should equal [1;2;3;4]
//        c |> CList.count |> should equal 4
//
//        transact (fun () -> c.[3] <- 1)
//        r |> content |> should equal [1;2;3;1]
//        c |> CList.count |> should equal 4
//
//        transact (fun () -> c |> CList.add 5)
//        r |> content |> should equal [1;2;3;1;5]
//        c |> CList.count |> should equal 5
//
//        transact (fun () -> c |> CList.addRange [6;7])
//        r |> content |> should equal [1;2;3;1;5;6;7]
//        c |> CList.count |> should equal 7
//
//        c |> CList.tryGet -1 |> should equal None
//        c |> CList.tryGet 7 |> should equal None
//        c |> CList.tryGet 0 |> should equal (Some 1)
//        c |> CList.tryGet 6 |> should equal (Some 7)
//
//        transact (fun () -> c |> CList.clear)
//        r |> content |> should equal []
//        c |> CList.count |> should equal 0
//
//        
//        CList.empty<int> = CList.empty<int> |> should be False
//
//        ()
//
//    [<Test>]
//    let ``[AList] set sorting``() =
//        let s = CSet.ofList [4;3;2;1]
//
//        let l = s |> ASet.sort
//        let r = l.GetReader()
//
//        let content (r : IListReader<'a>) = 
//            r.GetDelta() |> ignore
//            r.Content.All |> Seq.sortBy fst |> Seq.map snd |> Seq.toList
//
//        r |> content |>  should equal [1;2;3;4]
//        
//        transact (fun () -> CSet.add 5 s |> ignore)
//        r |> content |>  should equal [1;2;3;4;5]
//
//        transact (fun () -> CSet.remove 3 s |> ignore)
//        r |> content |>  should equal [1;2;4;5]
//
//        transact (fun () -> CSet.add 3 s |> ignore)
//        r |> content |>  should equal [1;2;3;4;5]
//
//        transact (fun () -> CSet.clear s )
//        r |> content |>  should equal []
//        
//
//    [<Test>]
//    let ``[AList] alist construction of aset``() =
//
//        let set = CSet.ofList [ 1; 2; 3 ]
//
//        let list = AList.ofASet set
//
//
//        let r = list.GetReader()
//        let content (r : IListReader<'a>) = 
//            r.GetDelta() |> ignore
//            r.Content.All |> Seq.sortBy fst |> Seq.map snd |> Seq.toList
//
//        transact (fun () -> CSet.add 12 set |> ignore)
//        r |> content |> should equal [  1; 2; 3; 12; ]
//
//        transact (fun () -> CSet.remove 3 set |> ignore)
//        r |> content |> should equal [  1; 2; 12 ]
//
//
//    [<Test>]
//    let ``[AList] alist construction of aset with selection``() =
//
//        let set = CSet.ofList [ 1; 2; 3 ]
//        
//        let list =
//            alist {
//                for x in AList.ofASet set do
//                    yield x * 2
//            }
//
//        let r = list.GetReader()
//        let content (r : IListReader<'a>) = 
//            r.GetDelta() |> ignore
//            r.Content.All |> Seq.sortBy fst |> Seq.map snd |> Seq.toList
//
//        transact (fun () -> CSet.add 12 set |> ignore)
//        r |> content |> should equal [ 2; 4; 6; 24]
//
//    [<Test>]
//    let ``[AList] ASet -> sortWith -> AList``() =
//
//        let input = CSet.ofList [ 1..10 ]
//
//        let set = input |> ASet.map id
//
//        let alist = set |> ASet.sortWith (fun _ _ -> 0)
//
//        let mapped = alist |> AList.map id
//
//        let r = mapped.GetReader()
//        let delta (r : IListReader<'a>) = r.GetDelta() |> List.map (Delta.map snd)
//
//        r |> delta |> should setEqual  [ for i in 1 .. 10 do yield Add i] 
//
//        transact (fun () -> CSet.add 11 input |> ignore )
//
//        r |> delta |> should setEqual  [ Add 11 ] 
//
//        transact (fun () -> CSet.add 12 input |> ignore )
//        transact (fun () -> CSet.add 13 input |> ignore )
//
//        r |> delta |> should setEqual  [ Add 12; Add 13 ] 
//
//        r.Content.All |> Seq.sortBy fst |> Seq.map snd |> Seq.toList |> should setEqual [ 1..13 ]
//
//    [<Test>]
//    let ``[AList] toASet test``() =
//        let list = CList.empty
//
//        let lm = AList.toMod list
//        let set = AList.toASet list
//        let setMod = ASet.toMod set
//
//        //let setReader = ASetReaders.ReferenceCountedReader(fun () -> set.GetReader())
//        //let reader = setReader.GetReference()
//
//        let test() =
//            let lc = list.Count 
//            let sc = setMod.GetValue().Count
//            let rc = lm.GetValue().Count
//            //reader.GetDelta() |> ignore
//            //let sc = reader.Content.Count
//            sprintf "ListCount=%d SetCount=%d ReaderCount=%d" lc sc rc |> Console.WriteLine
//            rc |> should equal lc
//            sc |> should equal lc
//
//        test()
//
//        transact (fun () -> list.Add(new Object()) |> ignore )
//
//        test()
//
//        transact (fun () -> list.Add(new Object()) |> ignore )
//
//        test()
//
//        transact (fun () -> list.Clear() )
//
//        test()
//
//        transact (fun () -> list.Add(new Object()) |> ignore )
//        transact (fun () -> list.Add(new Object()) |> ignore )
//        transact (fun () -> list.Add(new Object()) |> ignore )
//        transact (fun () -> list.Add(new Object()) |> ignore )
//
//        test()
//        
//        transact (fun () -> list.Clear() )
//
//        test()
//
//        transact (fun () -> 
//            list.Clear() 
//            list.Clear() 
//            list.Add(new Object()) |> ignore
//            list.Add(new Object()) |> ignore
//            list.Add(new Object()) |> ignore
//        )
//
//        test()
//
//        transact (fun () -> 
//            list.Clear() 
//            list.Add(new Object()) |> ignore
//            list.Add(new Object()) |> ignore
//            list.Add(new Object()) |> ignore
//        )
//
//        test()
//
//        transact (fun () -> list.Clear() )
//
//        transact (fun () -> 
//            list.Add(new Object()) |> ignore
//            list.Add(new Object()) |> ignore
//            list.Add(new Object()) |> ignore
//        )
//
//        test()
//
//        transact (fun () -> 
//            list.Add(new Object()) |> ignore
//        )
//
//        test()
//
//        transact (fun () -> list.Clear() )
//
//        transact (fun () -> 
//            list.Add(new Object()) |> ignore
//        )
//
//        test()
//
//
//
//    [<Test>]
//    let ``[CList] concurrent reader-reset``() =
//        
//        let list = CList.ofList [1..10]
//
//        let r = (list :> alist<_>).GetReader()
//        let running = ref true
//
//        Task.Factory.StartNew(fun () ->
//            while !running do
//                list.Clear() // causing reset
//                list.AddRange [1..10] // changing content
//        ) |> ignore
//
//        for i in 0..100 do
//            r.GetDelta() |> ignore
//
//        running := false
//
//    [<Test>]
//    let ``[COrderedSet] concurrent reader-reset``() =
//        
//        let set = COrderedSet.ofList [1..10]
//
//        let r = (set :> alist<_>).GetReader()
//        let running = ref true
//
//        Task.Factory.StartNew(fun () ->
//            while !running do
//                set.Clear() // causing reset
//                set.UnionWith [1..10] // changing content
//        ) |> ignore
//
//        for i in 0..100 do
//            r.GetDelta() |> ignore
//
//        running := false
//
//    [<Test>]
//    let ``[COrderedSet] add remove clear test``() =
//        
//        let objects = [Object; Object; Object; Object; Object; Object; Object; Object; Object; Object;]
//        let set = COrderedSet.empty
//        
//        let rnd = Random()
//
//        let mutable readerCount = 0
//        let foo = 
//            set |> ASet.map id 
//                |> ASet.unsafeRegisterCallbackNoGcRoot (fun dl -> 
//                    for d in dl do
//                        match d with
//                            | Add _ -> readerCount <- readerCount + 1
//                            | Rem _ -> readerCount <- readerCount - 1
//                   )
//
//        for i in 0..1000 do
//            if (i % 10) = 0 then
//                transact ( fun () -> COrderedSet.clear set )
//                should equal 0 set.Count
//                should equal 0 readerCount
//            else
//                let add = rnd.NextDouble() < 0.5
//                let origCnt = set.Count
//                //let n = rnd.Next(10)
//                let o = objects.[rnd.Next(9)]
//                let contains = set.Contains o
//                let suc = 
//                    transact ( fun () ->
//                        if add then set.Add o
//                        else set.Remove o
//                    )
//
//                if add then
//                    // contains = true -> suc = false
//                    // contains = false -> suc = true
//                    should equal (contains <> suc) true
//                else // if rem
//                    // contains = true -> suc = true
//                    // contains = false -> suc = false
//                    should equal contains suc
//
//                should equal set.Count readerCount
//
//
//        foo.Dispose()
//
//    type Leak(cnt : ref<int>) =
//        let value = !cnt
//        do cnt := !cnt + 1
//        member x.Value = value
//        override x.Finalize() = cnt := !cnt - 1
//
//    [<Test>]
//    let ``[AList] memory leak``() =
//        let list = CList.empty<Leak>;
//        let list2 = list |> AList.map (fun x -> (x.Value) * 2)
//        let cnt = ref 0
//
//        let check() =
//
//            printfn "%A" (list2 |> AList.toList)
//
//            GC.Collect ()
//            GC.WaitForPendingFinalizers()
//        
//            printfn "%A" !cnt
//               
//            !cnt |> should equal list.Count
//
//        // intial adds
//        transact (fun () -> list.Add(Leak(cnt)) |> ignore)
//
//        transact (fun () -> list.Add(Leak(cnt)) |> ignore)
//
//        transact (fun () -> list.Add(Leak(cnt)) |> ignore)
//
//        check() // should be 3
//        
//        transact (fun () -> list.Clear())
//
//        check() // should be 0
//
//        transact (fun () -> list.Add(Leak(cnt)) |> ignore)
//
//        transact (fun () -> list.Add(Leak(cnt)) |> ignore)
//
//        check() // should be 2
//
//        transact (fun () -> list.Add(Leak(cnt)) |> ignore)
//
//        check() // should be 3
//        
//        ()