module Program

[<EntryPoint>]
let main args =
    Aardvark.Base.Incremental.Tests.SizeOfAdaptiveObjects.``[MemoryOverhead] memory test``()
    //Aardvark.Base.Incremental.Tests.TreeFlattenPerformance.test()
    //Aardvark.Base.Incremental.Tests.SimpleTest.run()
    //Aardvark.Base.Incremental.Tests.``collect tests``.``[ASet] async registerCallback``()
    0