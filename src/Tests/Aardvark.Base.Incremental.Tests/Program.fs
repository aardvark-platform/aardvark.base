module Program

open Aardvark.Base.Incremental.Tests

[<EntryPoint>]
let main args =
    //Aardvark.Base.Incremental.Tests.SimpleTest.run()
    //Aardvark.Base.Incremental.Tests.``collect tests``.``[ASet] memory leak test``()
    //Aardvark.Base.Incremental.Tests.``collect tests``.``[ASet] async registerCallback``()
    //Aardvark.Base.Incremental.Tests.AgTests.``[Ag] Leaky leaky test``()
    //Aardvark.Base.Incremental.Tests.SimplePerfTests.``[ASet] value dependent nop change``()
    //Aardvark.Base.Incremental.Tests.InstancingTest.test()
    ``Basic Mod Tests``.``[Mod] consistent concurrency test``()
    //Aardvark.Base.Incremental.Tests.ConcurrentDeltaQueueTests.``[ASet ConcurrentDeltaQueue] concurrent delta queue test``()
    0