namespace Aardvark.Base.FSharp.Tests

open System
open Aardvark.Base
open FsUnit
open NUnit.Framework
open System.Runtime.InteropServices
open System.Diagnostics
open System.Collections.Generic



module SortedSetNeighbours =
    

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
            Console.WriteLine("iteration {0}: {1}", i, d.Count)

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



