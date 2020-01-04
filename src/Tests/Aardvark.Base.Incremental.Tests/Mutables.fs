namespace Aardvark.Base.Incremental.Tests

open System
open System.Threading
open System.Threading.Tasks
open System.Collections
open System.Collections.Generic
open Aardvark.Base.Incremental
open NUnit.Framework
open FsUnit
open Aardvark.Base
open FsCheck


module MutableTests =
    let seed() = 
        let r = Random()
        r.Next(), r.Next()

    [<Test>]
    let ``[MSet] update working as expected``() =
        let generator = Arb.generate<Set<int>>
        let seed = Random.StdGen (seed())

        let dst = MSet.empty
        let reader = (dst :> aset<_>).GetReader()

        let generator = generator |> Gen.listOfLength 100

        let sets = generator |> Gen.eval 100 seed
        for set in sets do
            transact (fun () -> dst.Value <- HSet.ofSeq set)
            printfn "%A" set
            let ops = reader.GetOperations()
            let content = reader.State |> Set.ofSeq
            content |> should equal set

    [<Test>]
    let ``[MList] update working as expected``() =
        let generator = Arb.generate<list<int>>
        let seed = Random.StdGen (seed())

        let dst = MList.empty
        let reader = (dst :> alist<_>).GetReader()

        let generator = generator |> Gen.listOfLength 100

        let sets = generator |> Gen.eval 100 seed
        for set in sets do
            transact (fun () -> dst.Value <- PList.ofList set)
            printfn "%A" set
            let ops = reader.GetOperations()
            let content = reader.State |> PList.toList
            content |> should equal set

    [<Test>]
    let ``[MMap] update working as expected``() =
        let generator = Arb.generate<Map<int, int>>
        let seed = Random.StdGen (seed())

        let dst = MMap.empty
        let reader = (dst :> amap<_,_>).GetReader()

        let generator = generator |> Gen.listOfLength 100

        let sets = generator |> Gen.eval 100 seed
        for set in sets do
            transact (fun () -> dst.Value <- HashMap.ofMap set)
            printfn "%A" set
            let ops = reader.GetOperations()
            let content = reader.State |> HashMap.toMap
            content |> should equal set