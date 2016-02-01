module Program

[<EntryPoint>]
let main args =
    //Aardvark.Base.Incremental.Tests.SimpleTest.run()
    //Aardvark.Base.Incremental.Tests.``collect tests``.``[ASet] memory leak test``()
    //Aardvark.Base.Incremental.Tests.``collect tests``.``[ASet] async registerCallback``()
    //Aardvark.Base.Incremental.Tests.AgTests.``[Ag] Leaky leaky test``()
    
    Aardvark.Base.Incremental.Tests.InstancingTest.test()
    0