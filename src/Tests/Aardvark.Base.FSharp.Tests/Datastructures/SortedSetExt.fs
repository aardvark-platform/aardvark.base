namespace Aardvark.Base.FSharp.Tests

open System
open Aardvark.Base
open FsUnit
open NUnit.Framework
open System.Runtime.InteropServices
open System.Diagnostics
open System.Collections.Generic



module SortedSetNeighbours =

    [<TestCase(false)>]
    [<TestCase(true)>]
    let ``[SortedSet] live bounded neighbourhood`` (descending : bool) =
        let comparer = Comparer<int>.Create(fun a b -> if descending then compare b a else compare a b)
        let parent = SortedSetExt<int>(seq { 1 .. 7 }, comparer)
        let view = parent.GetViewBetween((if descending then 5 else 3), (if descending then 3 else 5))
        let nested = view.GetViewBetween(4, 4)
        let check query expected = SortedSet.neighbourhood query view |> should equal expected
        if descending then
            check 0 (Some 3, None, None)
            check 3 (Some 4, Some 3, None)
            check 5 (None, Some 5, Some 4)
            check 8 (None, None, Some 5)
        else
            check 0 (None, None, Some 3)
            check 3 (None, Some 3, Some 4)
            check 5 (Some 4, Some 5, None)
            check 8 (Some 5, None, None)
        parent.Clear()
        check 4 (None, None, None)
        SortedSet.neighbourhood 4 nested |> should equal (None, None, None)
        parent.Add 4 |> ignore
        parent.Add 3 |> ignore
        parent.Add 5 |> ignore
        check 4 (Some (if descending then 5 else 3), Some 4, Some (if descending then 3 else 5))
        SortedSet.neighbourhood 4 nested |> should equal (None, Some 4, None)
        parent.Remove 4 |> ignore
        SortedSet.neighbourhood 4 nested |> should equal (None, None, None)

    [<Test>]
    let ``[SortedDict] neighbours``() =
        
        let values = HashSet [1..50000]
        let r = Random()
        let d = SortedDictionaryExt(compare)
        let content = SortedSet []
        
        let slowNeighbours (v : int) =
            let mutable last = None
            let l = Seq.toList content

            let rec find (v : int) (last : Option<int * int>) (l : list<int>) =
                match l with
                    | [] -> last,None
                    | k::rest ->
                        if k >= v then
                            if k = v then
                                last, (match rest with | r::_ -> Some (r,r) | _ -> None)
                            else
                                last, Some (k,k)
                        else
                            find v (Some (k,k)) rest

            find v None l
                    


        for i in 0..10000 do
            
            let remove = r.NextDouble() > 0.5 && d.Count > 0

            if remove && i > 200 then
                let kvp = d |> Seq.item (r.Next(d.Count))
                d.Remove kvp.Key |> should be True
                values.Add kvp.Key |> should be True
                content.Remove kvp.Key |> should be True


                let l,s,r = d |> SortedDictionary.neighbourhood kvp.Key
                let lc,rc = slowNeighbours kvp.Key

                s |> should equal None
                l |> should equal lc
                r |> should equal rc

                ()
            else
                let v = values |> Seq.item (r.Next values.Count)
                values.Remove v |> should be True

                let l,s,r = d |> SortedDictionary.neighbourhood v
                let lc,rc = slowNeighbours v

                s |> should equal None
                l |> should equal lc
                r |> should equal rc



                d.Add(v,v)
                content.Add v |> should be True

                ()



