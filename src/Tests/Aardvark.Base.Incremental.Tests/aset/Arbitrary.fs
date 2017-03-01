namespace Aardvark.Base.Incremental.Tests

open System.Collections.Generic
open NUnit.Framework
open FsCheck
open FsCheck.NUnit
open Aardvark.Base
open Aardvark.Base.Incremental

type ASetChange<'a> = { perform : unit -> unit; desired : hrefset<'a>; name : string }

module ASetChange =
    let map (f : hrefset<'a> -> hrefset<'b>) (change : ASetChange<'a>) =
        { perform = change.perform; desired = f change.desired; name = change.name }

type ASetProgram(set : aset<int>, changes : list<ASetChange<int>>) =
    
    static let checkEqual (is : hrefset<'a>) (should : hrefset<'a>) =
        let his = HashSet is
        let hshould = HashSet should
        if not (his.SetEquals hshould) then
            let isButNotShould = HRefSet.difference is should
            let shouldButNotIs = HRefSet.difference should is


            match HRefSet.isEmpty isButNotShould, HRefSet.isEmpty shouldButNotIs with
                | true, true ->
                    failwith "[ASet] PRefSet.difference produced empty sets"

                | false, true ->
                    failwithf "[ASet] values %A exist but should not." isButNotShould

                | true, false ->
                    failwithf "[ASet] values %A don't exist but should." shouldButNotIs

                | false, false ->
                    failwithf "[ASet] values %A exist but should not and %A don't but should" isButNotShould shouldButNotIs
                    
                    
            

            ()
    
    static let checkEqualDelta (is : hdeltaset<'a>) (should : hdeltaset<'a>) =
        let his = HashSet is
        let hshould = HashSet should
        if not (his.SetEquals hshould) then
            failwithf "[ASet] got deltas %A but effective are %A" is should


    member x.Set = set
    member x.Changes = changes

    member x.Check() =
        use r = set.GetReader()

        for c in changes do
            transact c.perform
            let oldState = r.State
            let ops = r.GetOperations null
            let state = r.State

            checkEqual state c.desired

            let (s', ops') = HRefSet.applyDelta oldState ops
            checkEqual state s'

            checkEqualDelta ops ops'

    new(s : aset<int>) = 
        if s.IsConstant then
            ASetProgram(s, [ { perform = id; desired = Mod.force s.Content; name = "initial" } ])
        else
            ASetProgram(s, [])
   
type ASetGenerator() =
    static member ASetProgram() =
        { new Arbitrary<ASetProgram>() with
            override x.Generator =
                gen {
                    let! kind = Arb.generate<int>

                    match abs kind % 3 with
                        | 0 ->
                            let! values = Arb.generate<list<int>>
                            let values = HRefSet.ofSeq (HSet.ofList values)
                            return ASetProgram(ASet.ofSet values)

                        | 1 ->
                            let! initial = Arb.generate<list<int>>
                            let initial = HRefSet.ofSeq (HSet.ofList initial)

                            let set = cset initial
                            let fst = { perform = id; desired = initial; name = "initial" }

                            let! a = Arb.generate<int>
                            let! changes = Arb.generate<list<int>>
                            let changes = a :: changes

                            let mutable state = initial
                            let ops = Array.zeroCreate (List.length changes)
                            let mutable i = 0
                            for value in changes do
                                let! isAdd = Arb.generate<bool>
                                let contained = HRefSet.contains value state

                                if isAdd && not contained then
                                    state <- HRefSet.add value state
                                    ops.[i] <- { desired = state;  name = "add"; perform = fun () -> set.Add value |> ignore }
                                else
                                    state <- HRefSet.remove value state
                                    ops.[i] <- { desired = state; name = "rem"; perform = fun () -> set.Remove value |> ignore}
                                
                                i <- i + 1

                            return ASetProgram(set :> aset<_>, fst :: Array.toList ops)

                        | 2 ->
                            let! f = Arb.generate<int -> int>
                            let! inner = Arb.generate<ASetProgram>
                            let res = ASet.map f inner.Set

                            return ASetProgram(res, inner.Changes |> List.map (ASetChange.map (HRefSet.map f)))
     
             

                        | _ ->
                            return Unchecked.defaultof<_>
                }
        
            override x.Shrinker t = Seq.empty
        }

            
module ASetTesting =
    [<SetUp>]
    let setup() = 
        Arb.register<ASetGenerator>() |> ignore
//        
//        let a = Arb.generate<ASetProgram>
//
//        let programs = a |> Gen.arrayOfLength 1000 |> Gen.eval 1 (Random.StdGen(0,1))
//        for v in programs do
//            v.Check()
//
//        System.Console.WriteLine("Ok, passed 1000 checks")

    [<Property(MaxTest = 1000)>]
    let ``[ASet] validate`` (p : ASetProgram) =
        p.Check()