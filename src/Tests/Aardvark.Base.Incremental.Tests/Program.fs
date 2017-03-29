module Program
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.Base.Incremental.Tests
open System.Threading


let reevalSet =
    ASet.custom (fun token state ->
        if isNull token.Tag then
            HDeltaSet.empty
        else
            token.Caller.Reevaluate <- true
            HDeltaSet.ofList [Add (unbox<int> token.Tag)]
    )

let reevalSet2 =
    ASet.custom (fun token state ->
        if isNull token.Tag then
            HDeltaSet.empty
        else
            token.Caller.Reevaluate <- true
            HDeltaSet.ofList [Add (unbox<int> token.Tag)]
    )

let tag<'a> : IMod<Option<'a>> =
    Mod.custom (fun token ->
        token.Caller.Reevaluate <- true
        match token.Tag with
            | :? 'a as a -> Some a
            | _ -> None
    )
    

let reeval =
    let mutable id = 0
    Mod.custom (fun token ->
        token.Caller.Reevaluate <- true
        unbox<int> token.Tag
    )

[<EntryPoint>]
let main args =
    let a = Mod.init 2
    let test = Mod.map2 (*) reeval a 

    let eval (v : int) (m : IMod<'a>) = 
        m.GetValue(AdaptiveToken.Top.WithTag v)

    test |> eval 0 |> printfn "%A"
    test |> eval 1 |> printfn "%A"
    transact (fun () -> a.Value <- 10)
    test |> eval 2 |> printfn "%A"
    test |> eval 3 |> printfn "%A"
    test |> eval 4 |> printfn "%A"
    test |> eval 5 |> printfn "%A"

    let test = ASet.difference (ASet.map (fun c -> c + 1) reevalSet) reevalSet

    let r = test.GetReader()
    let eval (v : obj) (m : ISetReader<'a>) =
        let outDated = m.OutOfDate
        let ops = m.GetOperations(AdaptiveToken.Top.WithTag v)
        printfn "%A: %A (outdated = %A)" v ops outDated

    r |> eval 0 
    r |> eval 1 
    r |> eval 2 
    r |> eval 3 
    r |> eval 4 
    r |> eval null
    r |> eval 6


//    let sepp = Mod.custom (fun self -> printfn "sepp"; 1)
//    let res = reeval |> Mod.bind (fun v -> sepp |> Mod.map (fun c -> v * c))
//
//    res.GetValue() |> printfn "%A"
//    res.GetValue() |> printfn "%A"
//    res.GetValue() |> printfn "%A"
//    res.GetValue() |> printfn "%A"
//    res.GetValue() |> printfn "%A"



    //Aardvark.Base.Incremental.Tests.SimpleTest.run()
    //Aardvark.Base.Incremental.Tests.``collect tests``.``[ASet] memory leak test``()
    //Aardvark.Base.Incremental.Tests.``collect tests``.``[ASet] async registerCallback``()
    //Aardvark.Base.Incremental.Tests.AgTests.``[Ag] Leaky leaky test``()
    //Aardvark.Base.Incremental.Tests.ASetPerformance.``[ASet Performance] map``()
    //Aardvark.Base.Incremental.Tests.InstancingTest.test()
    //``Basic Mod Tests``.``[Mod] consistent concurrency test``()
    //Aardvark.Base.Incremental.Tests.ConcurrentDeltaQueueTests.``[ASet ConcurrentDeltaQueue] concurrent delta queue test``()
    0