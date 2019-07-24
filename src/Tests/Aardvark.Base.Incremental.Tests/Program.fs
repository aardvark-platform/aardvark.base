module Program
open Aardvark.Base
open Aardvark.Base.Incremental
open Aardvark.Base.Incremental.Tests
open System.Threading

let performanceTest() =
    SimSetTest.``[ASet] validation``()

    Log.startTimed "time\r\n"
    for i in 0..49 do
        ``performance tests``.``[ASet] collect performance``()
    Log.stop()

    Log.startTimed "time\r\n"
    for i in 0..49 do
        ``performance tests``.``[ASet] map performance``()
    Log.stop()


[<EntryPoint>]
let main args =


//    let sepp = Mod.custom (fun self -> printfn "sepp"; 1)
//    let res = reeval |> Mod.bind (fun v -> sepp |> Mod.map (fun c -> v * c))
//
//    res.GetValue() |> printfn "%A"
//    res.GetValue() |> printfn "%A"
//    res.GetValue() |> printfn "%A"
//    res.GetValue() |> printfn "%A"
//    res.GetValue() |> printfn "%A"

    performanceTest()

    //Aardvark.Base.Incremental.Tests.SimpleTest.run()
    //Aardvark.Base.Incremental.Tests.``collect tests``.``[ASet] memory leak test``()
    //Aardvark.Base.Incremental.Tests.``collect tests``.``[ASet] async registerCallback``()
    //Aardvark.Base.Incremental.Tests.AgTests.``[Ag] Leaky leaky test``()
    //Aardvark.Base.Incremental.Tests.ASetPerformance.``[ASet Performance] map``()
    //Aardvark.Base.Incremental.Tests.InstancingTest.test()
    //SimListTest.``[AList] validation``()

    //AListTestsNew.testMapUse()
    //System.Environment.Exit 0
    //``Basic Mod Tests``.``[Mod] eval and mark performance``()
    //Aardvark.Base.Incremental.Tests.ConcurrentDeltaQueueTests.``[ASet ConcurrentDeltaQueue] concurrent delta queue test``()
    0